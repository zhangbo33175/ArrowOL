/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  MapBuildEditor.Global.cs
 * author:    云毅
 * created:   2026
 * descrip:   地图编辑器 - 全局变量、配置、工具类、路径管理
 ***************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Editor.Utility;
using Honor.Editor;
using Honor.Runtime;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Editor.MapEditor
{
    /// <summary>
    /// 地图编辑器 - 全局变量、配置、工具类、路径管理
    /// 包含：数据结构、路径工具、Excel配置加载、图片替换、数据保存/读取
    /// </summary>
    public sealed partial class MapBuildEditor
    {
        //=========================================================================
        // 全局成员变量
        //=========================================================================
        #region Global Variables
        /// <summary>
        /// 地图文件列表滚动位置
        /// </summary>
        private Vector2 m_MapListView;

        /// <summary>
        /// 图标列表滚动位置
        /// </summary>
        private Vector2 m_IconListView;

        /// <summary>
        /// 地图面板滚动位置
        /// </summary>
        private Vector2 m_MapPanel;

        /// <summary>
        /// 地图默认宽度
        /// </summary>
        private static int m_MapWidth = 2048;

        /// <summary>
        /// 地图默认高度
        /// </summary>
        private static int m_MapHeight = 2048;

        /// <summary>
        /// 当前编辑的地图/关卡名称
        /// </summary>
        private static string mapName;

        /// <summary>
        /// 当前编辑的地图/关卡ID
        /// </summary>
        private static string mapId;
        /// <summary>
        /// 当前编辑的地图关卡ID
        /// </summary>
        private static string mapLevelId;
        /// <summary>
        /// 地图预览缩放比例
        /// </summary>
        private static float m_MapZoom = 1.0f;

        /// <summary>
        /// UI样式是否已初始化
        /// </summary>
        private bool m_IsStylesInitialized = false;

        /// <summary>
        /// 地图JSON文件信息列表
        /// </summary>
        private static List<MapJsonInfo> m_MapJsonList = new List<MapJsonInfo>();

        /// <summary>
        /// 关卡数据列表
        /// </summary>
        private static List<LevelDataEditor> m_LevelDataList = new List<LevelDataEditor>();

        /// <summary>
        /// 图标数据列表（可拖拽到地图上的物件）
        /// </summary>
        private static List<ItemIconDataEditor> m_IconList = new List<ItemIconDataEditor>();

        /// <summary>
        /// 地图UI预览实例
        /// </summary>
        private static GameObject m_UiInstance;

        /// <summary>
        /// 预览相机
        /// </summary>
        private static Camera m_PreviewCamera;

        /// <summary>
        /// 拖动偏移
        /// </summary>
        private static Vector2 m_Drag;

        /// <summary>
        /// 当前加载的预制体
        /// </summary>
        private static Object m_CurrentPrefab;

        /// <summary>
        /// 地图预览绘制区域
        /// </summary>
        private static Rect m_PreviewRect;

        /// <summary>
        /// 相机基础正交大小（适配2048x2048地图）
        /// </summary>
        private static float m_BaseOrthoSize = 5.12f;

        /// <summary>
        /// 相机默认位置
        /// </summary>
        private static Vector3 m_BaseCameraPos = new Vector3(0, 0, -10f);

        /// <summary>
        /// 待添加到地图的预制体
        /// </summary>
        private static Object m_PrefabToAdd;

        /// <summary>
        /// 已添加到地图上的所有物体
        /// </summary>
        private static List<GameObject> m_AddObjects = new List<GameObject>();

        /// <summary>
        /// 物体与地图数据的绑定关系
        /// </summary>
        private static Dictionary<GameObject, RMapData> m_RMapDatas = new Dictionary<GameObject, RMapData>();

        /// <summary>
        /// UI射线检测专用相机
        /// </summary>
        private static Camera _uiRaycastCamera;

        /// <summary>
        /// 上一次点击时间（用于防抖）
        /// </summary>
        private static double _lastClickTime;

        /// <summary>
        /// 点击防抖间隔
        /// </summary>
        private static float _clickDetectionThreshold = 0.1f;
        #endregion

        //=========================================================================
        // 表格配置数据 & 路径
        //=========================================================================
        #region Table Config Data & Paths
        /// <summary>
        /// 主关卡配置表
        /// </summary>
        private static List<TableMainLevelsEditor> m_TableMainLevelsList;

        /// <summary>
        /// 物品配置表
        /// </summary>
        private static List<TableItemEditor> m_TableItemsList;

        /// <summary>
        /// 每日/小节关卡配置表
        /// </summary>
        private static List<LevelDataEditor> m_TableDailyLevelsList;

        /// <summary>
        /// 章节信息
        /// </summary>
        private static List<TableChapterEditor> m_TableChapterList;

        /// <summary>
        /// 当前选中的主关卡数据
        /// </summary>
        private static TableMainLevelsEditor _mTableMainLevelsEditor;

        /// <summary>
        /// 当前选中的物品数据
        /// </summary>
        private static TableItemEditor _mTableItemEditor;

        /// <summary>
        /// 当前选中的图标数据
        /// </summary>
        private static ItemIconDataEditor _mItemIconDataEditor;

        /// <summary>
        /// 当前选中的小节关卡数据
        /// </summary>
        private static LevelDataEditor _mLevelDataEditor;

        
        /// <summary>
        /// 当前选中的主关卡数据
        /// </summary>
        private static TablesElectedLevelsEditor _mTablesElectedLevelsEditor;
        
        
        /// <summary>
        /// 主关卡Excel路径
        /// </summary>
        private static string m_TableMainLevelsPath => Path.Combine(Application.dataPath,
            "../Docs/Designs/Excels/Tables/AlTables/LevelConfigs/TableMainLevels.xlsm");

        /// <summary>
        /// 物品Excel路径
        /// </summary>
        private static string m_TableCharactersPath => Path.Combine(Application.dataPath,
            "../Docs/Designs/Excels/Tables/AlTables/Character/TableCharacters.xlsm");

        /// <summary>
        /// 每日关卡Excel路径
        /// </summary>
        private static string m_TableDailyLevelsPath => Path.Combine(Application.dataPath,
            "../Docs/Designs/Excels/Tables/AlTables/LevelConfigs/TableDailyLevels.xlsm");

        /// <summary>
        /// 章节Excel路径
        /// </summary>
        private static string m_TableChaptersPath => Path.Combine(Application.dataPath,
            "../Docs/Designs/Excels/Tables/AlTables/LevelConfigs/TableChapters.xlsm");

        /// <summary>
        /// JSON保存根路径
        /// </summary>
        private static string LevelSaveRootPath => Path.Combine(Application.dataPath, "../Docs/Designs/Json/Levels");

        /// <summary>
        /// Lua配置保存路径
        /// </summary>
        private static string levelLuaSavePath =>
            Path.Combine(GamePathUtils.Table.GetLuaScriptRootDirectoryFullPath(), "Levels");
        #endregion

        //=========================================================================
        // 数据结构定义
        //=========================================================================
        #region Data Structures
        /// <summary>
        /// 地图JSON文件信息结构
        /// </summary>
        [Serializable]
        public class MapJsonInfo
        {
            public string m_FilePath; // 文件完整路径
            public string m_FileName; // 文件名（带后缀）
            public string m_DisplayName; // 显示名称（无后缀）
            public RMapChapterTypeData mRMapChapterTypeData; // 地图数据实体
        }

        /// <summary>
        /// 资源加载类型
        /// </summary>
        public enum Type
        {
            LoadBackground, // 加载背景图
            LoadingIcon     // 加载图标
        }
        #endregion

        //=========================================================================
        // 路径工具类
        //=========================================================================
        #region Path Utility
        /// <summary>
        /// 路径工具类（统一管理资源路径）
        /// </summary>
        public static class PathUtils
        {
            /// <summary>
            /// 关卡JSON目录
            /// </summary>
            public static string GetLevelDirectoryFullPath()
            {
                return AorTxt.Format("{0}/{1}", Application.dataPath, "../Docs/Designs/Json/Levels");
            }

            /// <summary>
            /// 地图主预制体路径
            /// </summary>
            public static string GetMapGameMapPath()
            {
                return "Assets/Res/Prefabs/Map/GameMap.prefab";
            }

            /// <summary>
            /// 物品图标预制体路径
            /// </summary>
            public static string GetItemMapGameMapPath()
            {
                return "Assets/Res/Prefabs/Item/ItemMap.prefab";
            }

            /// <summary>
            /// 背景图完整路径
            /// </summary>
            public static string GetItemStrPath(string backgroundName)
            {
                return AorTxt.Format("{0}/{1}.png", "Assets/Res/Textures/Map/LevelBackground", backgroundName);
            }

            /// <summary>
            /// 图标完整路径
            /// </summary>
            public static string GetItemIconPath(string _iconName)
            {
                return AorTxt.Format("{0}/{1}.png", "Assets/Res/Textures/Map/Icon", _iconName);
            }
        }
        #endregion

        //=========================================================================
        // 通用工具类
        //=========================================================================
        #region Common Utility
        /// <summary>
        /// 通用工具类：加载、替换图片、解析数据、获取预制体
        /// </summary>
        public static class Utils
        {
            /// <summary>
            /// 加载关卡目录下所有JSON地图数据
            /// </summary>
            public static void LoadJsonFiles()
            {
                try
                {
                    m_MapJsonList.Clear();
                    if (!Directory.Exists(PathUtils.GetLevelDirectoryFullPath()))
                    {
                        Directory.CreateDirectory(PathUtils.GetLevelDirectoryFullPath());
                        return;
                    }

                    var jsonFiles = Directory.GetFiles(PathUtils.GetLevelDirectoryFullPath(), "*.json");
                    foreach (var filePath in jsonFiles)
                    {
                        string jsonContent = File.ReadAllText(filePath);
                        MapJsonInfo mapConfig = JsonUtility.FromJson<MapJsonInfo>(jsonContent);
                        m_MapJsonList.Add(mapConfig);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError($"JSON加载失败: {e}");
                }
            }

            /// <summary>
            /// 加载物品图标预制体
            /// </summary>
            public static GameObject GetIconItem()
            {
                string assetPath = PathUtils.GetItemMapGameMapPath();
                return AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
            }

            /// <summary>
            /// 加载地图主预制体
            /// </summary>
            public static GameObject GetMap()
            {
                string assetPath = PathUtils.GetMapGameMapPath();
                return AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
            }

            #region 图片替换工具
            /// <summary>
            /// 替换预制体上指定节点的图片
            /// </summary>
            public static GameObject ReplaceImageInPrefab(GameObject prefabPath, string targetNodePath,
                string imageSourcePath, bool isSavePrefab, Type type)
            {
                if (prefabPath == null || string.IsNullOrEmpty(targetNodePath) || string.IsNullOrEmpty(imageSourcePath))
                    return null;

                try
                {
                    Transform targetTransform = FindTargetTransform(prefabPath.transform, targetNodePath);
                    if (targetTransform == null) return null;

                    RawImage rawImage = targetTransform.GetComponent<RawImage>();
                    Image image = targetTransform.GetComponent<Image>();
                    if (rawImage == null && image == null) return null;

                    // 加载图片
                    Texture2D texture = type == Type.LoadBackground
                        ? LoadImageTexture(PathUtils.GetItemStrPath(imageSourcePath))
                        : LoadImageTexture(PathUtils.GetItemIconPath(imageSourcePath));

                    if (texture == null) return null;

                    // 设置图片
                    if (rawImage != null)
                    {
                        rawImage.texture = texture;
                        rawImage.SetNativeSize();
                    }
                    else if (image != null)
                    {
                        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height),
                            new Vector2(0.5f, 0.5f));
                        image.sprite = sprite;
                        image.SetNativeSize();
                        image.preserveAspect = true;
                    }

                    return prefabPath;
                }
                catch (Exception e)
                {
                    Debug.LogError($"替换图片异常: {e}");
                    return null;
                }
            }

            /// <summary>
            /// 递归查找子节点
            /// </summary>
            private static Transform FindTargetTransform(Transform rootTrans, string nodePath)
            {
                if (!nodePath.Contains("/"))
                    return rootTrans.Find(nodePath);

                Transform current = rootTrans;
                foreach (var part in nodePath.Split('/'))
                {
                    current = current.Find(part);
                    if (current == null) return null;
                }

                return current;
            }

            /// <summary>
            /// 从本地/AB包路径加载图片
            /// </summary>
            public static Texture2D LoadImageTexture(string imagePath)
            {
                if (imagePath.StartsWith("Assets/"))
                {
                    var tex = AssetDatabase.LoadAssetAtPath<Texture2D>(imagePath);
                    if (tex != null) return tex;
                }

                if (File.Exists(imagePath))
                {
                    byte[] bytes = File.ReadAllBytes(imagePath);
                    Texture2D tex = new Texture2D(2, 2);
                    tex.LoadImage(bytes);
                    return tex;
                }

                Debug.LogError($"图片不存在: {imagePath}");
                return null;
            }
            #endregion

            /// <summary>
            /// 收集地图上所有物体数据，用于保存JSON/Lua
            /// </summary>
            public static RMapChapterTypeData GetCurLevelTypeEditorData()
            {
                RMapChapterTypeData data = new RMapChapterTypeData();
                data.ChapterId = _mTablesElectedLevelsEditor.ChapterId;
                data.LevelId = int.Parse(_mTablesElectedLevelsEditor.LevelId);
                data.m_MapName = _mTablesElectedLevelsEditor.MapName;
                data.m_CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                data.m_BackgroundPath=_mTablesElectedLevelsEditor.Background;
                List<RMapData> objList = new List<RMapData>();
                foreach (GameObject go in m_AddObjects)
                {
                    if (go == null) continue;

                    RMapData mapData = m_RMapDatas.TryGetValue(go, out var d) ? d : new RMapData();
                    mapData.m_Name = go.name;

                    RectTransform rt = go.GetComponent<RectTransform>();
                    if (rt != null)
                    {
                        mapData.m_Position = rt.anchoredPosition;
                        mapData.m_Size = rt.sizeDelta;
                    }

                    Graphic graphic = go.GetComponent<Graphic>();
                    if (graphic != null && graphic.mainTexture != null)
                    {
                        mapData.m_Sprite = AssetDatabase.GetAssetPath(graphic.mainTexture);
                    }

                    objList.Add(mapData);
                }

                data.m_MapObjectData = objList;
                return data;
            }

            /// <summary>
            /// 将字符串数组解析为 int[]
            /// </summary>
            public static int[] GetIntList(string _list)
            {
                if (string.IsNullOrWhiteSpace(_list)) return new int[0];
                string clean = _list.Trim('{', '}').Replace("\"", "");
                return clean.Split(',').Where(s => !string.IsNullOrWhiteSpace(s)).Select(int.Parse).ToArray();
            }
        }
        #endregion

        //=========================================================================
        // 预览相机工具
        //=========================================================================
        #region Preview Camera Utility
        /// <summary>
        /// 预览相机初始化工具
        /// </summary>
        public static class SetShowCameraView
        {
            /// <summary>
            /// 创建并初始化预览相机
            /// </summary>
            public static void InitRenderUtility()
            {
                if (m_RenderCam != null) return;

                m_RenderCam = new GameObject("MapPreviewCam").AddComponent<Camera>();
                m_RenderCam.hideFlags = HideFlags.HideAndDontSave;
                m_RenderCam.orthographic = true;
                m_RenderCam.orthographicSize = m_BaseOrthoSize;
                m_RenderCam.nearClipPlane = 0.1f;
                m_RenderCam.farClipPlane = 100f;
                m_RenderCam.transform.position = m_BaseCameraPos;
                m_RenderCam.transform.rotation = Quaternion.identity;
                m_RenderCam.clearFlags = CameraClearFlags.SolidColor;
                m_RenderCam.backgroundColor = new Color(0.2f, 0.2f, 0.2f, 1f);
                m_RenderCam.cullingMask = -1;

                _uiRaycastCamera = m_RenderCam;
                _lastClickTime = 0;
            }
        }
        #endregion

        //=========================================================================
        // Excel配置加载
        //=========================================================================
        #region Excel Config Load
        /// <summary>
        /// Excel配置表加载工具
        /// </summary>
        public static class LoadConfig
        {
            /// <summary>
            /// 加载所有表格：关卡、物品、每日关卡
            /// </summary>
            public static void LoadConfigs()
            {
                try
                {
                    m_TableChapterList = TableExportEditorUtility.GetExcelData(m_TableChaptersPath).ToList<TableChapterEditor>();
                    m_TableMainLevelsList = TableExportEditorUtility.GetExcelData(m_TableMainLevelsPath).ToList<TableMainLevelsEditor>();
                    m_TableItemsList = TableExportEditorUtility.GetExcelData(m_TableCharactersPath).ToList<TableItemEditor>();
                    m_TableDailyLevelsList = TableExportEditorUtility.GetExcelData(m_TableDailyLevelsPath).ToList<LevelDataEditor>();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            public static void SetMapList()
            {
            }
        }
        #endregion

        //=========================================================================
        // 数据保存（JSON + Lua）
        //=========================================================================
        #region Data Save (JSON + Lua)
        /// <summary>
        /// 保存配置（JSON + Lua）
        /// </summary>
        public static class SaveConfig
        {
            /// <summary>
            /// JSON保存路径
            /// </summary>
            public static string LevelSavePath
            {
                get
                {
                    var data = Utils.GetCurLevelTypeEditorData();
                    if (!string.IsNullOrEmpty(data?.SavePath))
                        return Path.Combine(LevelSaveRootPath, _mTablesElectedLevelsEditor.ChapterId, data.SavePath);
                    return Path.Combine(LevelSaveRootPath, _mTablesElectedLevelsEditor.ChapterId);
                }
            }

            /// <summary>
            /// 获取目前选择的关卡类型数据
            /// </summary>
            private static TableChapterEditor GetCurLevelTypeEditorData()
            {
                return GetLevelTypeEditorDataByIndex(Convert.ToInt32(_mTableMainLevelsEditor.ID));
            }

            /// <summary>
            /// 通过索引获取关卡类型数据
            /// </summary>
            private static TableChapterEditor GetLevelTypeEditorDataByIndex(int index)
            {
                if (index < 0 || index >= m_TableChapterList.Count)
                {
                    return null;
                }

                return m_TableChapterList[index];
            }

            /// <summary>
            /// 保存Lua表格文件
            /// </summary>
            public static void MapSaveLua(RMapChapterTypeData data, bool hasLevelTypeSavePath = false)
            {
                string fileName = $"TableLevelData_{data.ChapterId}_{data.LevelId}";
                string luaPath = $"{levelLuaSavePath}/{data.ChapterId}/{fileName}.lua.txt";

                if (hasLevelTypeSavePath)
                    luaPath = $"{levelLuaSavePath}/{data.ChapterId}/{data.SavePath}/{fileName}.lua.txt";

                string indexPath = $"{levelLuaSavePath}/TableLevelData.lua.txt";
                StringBuilder sb = new StringBuilder();

                sb.AppendLine("--=====================================================================================================");
                sb.AppendLine("-- Auto-Generated by MapBuildEditor");
                sb.AppendLine("-- 关卡配置信息表");
                sb.AppendLine("--=====================================================================================================");
                sb.AppendLine();

                File.WriteAllText(indexPath, sb.ToString(), new UTF8Encoding(false));
                MapLevelDataUtil.SaveDataLua(data, luaPath);
                AssetDatabase.Refresh();
            }
        }
        #endregion

        //=========================================================================
        // 类型转换工具
        //=========================================================================
        #region 类型转换工具
        public static class SetTypeConversion
        {
            /// <summary>
            /// 安全地把类似 "[1,2,3]" 的字符串转 int[]
            /// 解析失败的项直接丢弃，不补0；空/无效返回空数组
            /// </summary>
            public static int[] OnStringToInt(string s)
            {
                // 空值直接返回空数组
                if (string.IsNullOrWhiteSpace(s))
                    return Array.Empty<int>();

                // 核心：清理所有不需要的符号 { } " , 空格
                string clean = s
                    .Replace("{", "")
                    .Replace("}", "")
                    .Replace("\"", "")
                    .Replace(" ", "")
                    .Trim();

                // 分割成字符串数组
                string[] parts = clean.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                // 转 int
                List<int> result = new List<int>();
                foreach (string part in parts)
                {
                    if (int.TryParse(part, out int num))
                    {
                        result.Add(num);
                    }
                }

                return result.ToArray();
            }
            
            /// <summary>
            /// 把 "(296.17, 693.17, 0.00)" 格式转成字符串数组 { "296.17", "693.17", "0.00" }
            /// </summary>
            public static string[] ParseVectorString(string str)
            {
                if (string.IsNullOrEmpty(str))
                    return new string[0];

                // 去掉括号 ( )
                string clean = str.Trim('(', ')');

                // 按逗号分割
                string[] parts = clean.Split(',');

                // 去掉每个值前后空格
                for (int i = 0; i < parts.Length; i++)
                {
                    parts[i] = parts[i].Trim();
                }

                return parts;
            }

            /// <summary>
            /// 把 "(296.17, 693.17, 0.00)" 直接转 float[]
            /// </summary>
            public static float[] ParseVectorToFloat(string str)
            {
                var parts = ParseVectorString(str);
                float[] result = new float[parts.Length];

                for (int i = 0; i < parts.Length; i++)
                {
                    float.TryParse(parts[i], out result[i]);
                }

                return result;
            }
            
            /// <summary>
            /// 将 Vector3 转换为保留2位小数的字符串数组：{ "x", "y", "z" }
            /// 格式：296.17、693.17、0.00
            /// </summary>
            public static string[] Vector3ToStringArray(Vector3 vec)
            {
                return new[]
                {
                    vec.x.ToString("F2"),
                    vec.y.ToString("F2"),
                    vec.z.ToString("F2")
                };
            }
            /// <summary>
            /// Vector3 → string：{"296.17","693.17","0.00"}
            /// </summary>
            public static string Vector3ToLuaString(Vector3 vec)
            {
                return $"{{\"{vec.x:F2}\",\"{vec.y:F2}\",\"{vec.z:F2}\"}}";
            }
        }
        #endregion
    }
}