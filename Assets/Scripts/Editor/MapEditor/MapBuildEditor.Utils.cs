using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using Honor.Editor;
using Honor.Runtime;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Editor.MapEditor
{
    public sealed partial class MapBuildEditor
    {
        /// <summary>
        /// 地图文件的显示list
        /// </summary>
        private Vector2 m_MapListView;
        /// <summary>
        /// Icon图标的显示list
        /// </summary>
        private Vector2 m_IconListView;
        /// <summary>
        /// 地图显示的
        /// </summary>
        private Vector2 m_MapPanel;
        /// <summary>
        /// 地图宽
        /// </summary>
        private static int m_MapWidth = 2048;
        /// <summary>
        /// 地图高
        /// </summary>
        private static int m_MapHeight = 2048;
        /// <summary>
        /// 章节名称
        /// </summary>
        private string mapName;
        /// <summary>
        /// 章节ID
        /// </summary>
        private string mapId;
        /// <summary>
        /// 地图的缩放
        /// </summary>
        private static float m_MapZoom = 1.0f;
        /// <summary>
        /// 样式初始化标记
        /// </summary>
        private bool m_IsStylesInitialized = false;
        /// <summary>
        /// 地图存储JSONList文件信息
        /// </summary>
        private static List<MapJsonInfo> m_MapJsonList = new List<MapJsonInfo>();
        
        /// <summary>
        /// 地图存储JSONList文件信息
        /// </summary>
        private static List<IconData> m_IconList = new List<IconData>();
        
        /// <summary>
        /// 
        /// </summary>
        private static GameObject m_UiInstance;

        /// <summary>
        /// 
        /// </summary>
        private static Camera m_PreviewCamera;

        /// <summary>
        /// 
        /// </summary>
        private static Vector2 m_Drag;
        
        /// <summary>
        /// 
        /// </summary>
        private static Object m_CurrentPrefab;

        /// <summary>
        /// 预览绘制区域
        /// </summary>
        private static Rect m_PreviewRect;
        
        // 相机基础参数
        /// <summary>
        /// 适配2048x2048的地图
        /// </summary>
        private static float m_BaseOrthoSize =5.12f;
        /// <summary>
        /// 相机基础参数
        /// </summary>
        private static Vector3 m_BaseCameraPos = new Vector3(0, 0, -10f);

        
        /// <summary>
        /// 新增：要添加到地图上的预制体
        /// </summary>
        private static Object m_PrefabToAdd;
        /// <summary>
        /// 新增：记录所有添加到地图上的物体，方便清理
        /// </summary>
        private static List<GameObject> m_AddObjects = new List<GameObject>();
        /// <summary>
        /// 专用射线检测相机
        /// </summary>
        private static Camera _uiRaycastCamera; 
        
        /// <summary>
        /// 新增变量：防抖时间记录
        /// </summary>
        private static double _lastClickTime;
        /// <summary>
        /// 点击检测阈值（防止误触）
        /// </summary>
        private static float _clickDetectionThreshold = 0.1f;

        #region 获取地图配置表

        /// <summary>
        /// 地图章节信息列表
        /// </summary>
        private static List<TableMainLevels> m_TableMainLevelsList;
        /// <summary>
        /// 寻物列表
        /// </summary>
        private static List<ItemIcon> m_TableItemsList;
        /// <summary>
        /// 小节地图信息列表
        /// </summary>
        private static List<LevelData> m_TableDailyLevelsList;
        /// <summary>
        /// 地图章节信息
        /// </summary>
        private static TableMainLevels m_TableMainLevels;
        /// <summary>
        /// 寻物
        /// </summary>
        private static ItemIcon m_ItemIcon;
        /// <summary>
        /// 地图章节
        /// </summary>
        private static LevelData m_LevelData;
        
        /// <summary>
        /// 章节表路径
        /// </summary>
        private static string m_TableMainLevelsPath =>Path.Combine(Application.dataPath, "../Docs/Designs/Excels/Tables/AlTables/Maps/TableMainLevels.xlsm");
        /// <summary>
        /// 小节地图信息列表表路径
        /// </summary>
        private static string m_TableItemsPath =>Path.Combine(Application.dataPath, "../Docs/Designs/Excels/Tables/AlTables/Maps/TableItems.xlsm");

        /// <summary>
        /// 小节地图信息表路径
        /// </summary>
        private static string m_TableDailyLevelsPath =>Path.Combine(Application.dataPath, "../Docs/Designs/Excels/Tables/AlTables/Maps/TableDailyLevels.xlsm");



        #endregion
      
        /// <summary>
        /// 存储JSON文件信息的实体类
        /// </summary>
        [Serializable]
        public class MapJsonInfo
        {
            [FormerlySerializedAs("FilePath")] 
            public string m_FilePath; // 文件完整路径
            [FormerlySerializedAs("FileName")] 
            public string m_FileName; // 文件名（带后缀）
            [FormerlySerializedAs("DisplayName")] 
            public string m_DisplayName; // 显示名称（去掉后缀）
            [FormerlySerializedAs("m_MapData")] [FormerlySerializedAs("MapData")] 
            public MapAssetData mMapAssetData;
        }
        /// <summary>
        /// ICON预览数据结构
        /// </summary>
        [Serializable]
        private class IconData
        {
            [FormerlySerializedAs("name")] 
            public string m_IconName;          // 名称
            [FormerlySerializedAs("texture")] 
            public Texture2D m_IconTexture;    // 纹理
            [FormerlySerializedAs("assetPath")] 
            public string m_IconAssetPath;     // 资源路径
            [FormerlySerializedAs("isLoaded")] 
            public bool m_IocnIsLoaded;        // 是否加载成功
        }
        
        
        public enum Type
        {
            LoadBackground,
            LoadingIcon
        }
        
        /// <summary>
        /// 
        /// </summary>
        public static class PathUtils
        {
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public static string GetLevelDirectoryFullPath()
            {
                return AorTxt.Format("{0}/{1}", Application.dataPath, "../Docs/Designs/Json/Levels");
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public static string GetMapGameMapPath()
            {
                return AorTxt.Format("{0}", "Assets/Res/Prefabs/Map/GameMap.prefab");
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public static string GetItemMapGameMapPath()
            {
                return AorTxt.Format("{0}","Assets/Res/Prefabs/Item/ItemMap.prefab");
            }
            
             /// <summary>
             /// 获取背景图片路径
             /// </summary>
             /// <param name="backgroundName">背景名称</param>
             /// <returns></returns>
            public static string GetItemStrPath(string backgroundName)
            {
                return AorTxt.Format("{0}/{1}.png","Assets/Res/Textures/Map/LevelBackground",backgroundName);
            }
             
             /// <summary>
             /// 获取图片路径
             /// </summary>
             /// <param name="_iconName">图片名称</param>
             /// <returns></returns>
             public static string GetItemIconPath(string _iconName)
             {
                 return AorTxt.Format("{0}/{1}.png","Assets/Res/Textures/Map/Icon",_iconName);
             }
        }

        /// <summary>
        /// 
        /// </summary>
        public static class Utils
        {
            /// <summary>
            /// 加载指定文件夹下的所有JSON文件
            /// </summary>
            public static void LoadJsonFiles()
            {
                try
                {
                    m_MapJsonList.Clear();
                    if (!Directory.Exists(PathUtils.GetLevelDirectoryFullPath()))
                    {
                        Directory.CreateDirectory(PathUtils.GetLevelDirectoryFullPath());
                        Debug.Log($"创建JSON文件夹：{PathUtils.GetLevelDirectoryFullPath()}");
                        return;
                    }

                    var jsonFiles = Directory.GetFiles(PathUtils.GetLevelDirectoryFullPath(), "*.json");
                    foreach (var filePath in jsonFiles)
                    {
                        string jsonContent = File.ReadAllText(filePath);
                        MapJsonInfo mapConfig = JsonUtility.FromJson<MapJsonInfo>(jsonContent);
                        m_MapJsonList.Add(mapConfig);
                    }

                    Debug.Log($"加载到 {m_MapJsonList.Count} 个JSON地图文件");
                }
                catch (Exception e)
                {
                    Debug.Log($"加载失败" + e);
                }
            }
            
            /// <summary>
            /// 获取Icon预制体
            /// </summary>
            /// <returns></returns>
            public static GameObject GetIconItem()
            {
                string assetPath = PathUtils.GetItemMapGameMapPath();
                if (assetPath==null)
                {
                    return null;
                }
                if (!PathUtils.GetItemMapGameMapPath().StartsWith("Assets/"))
                {
                    assetPath = FileUtil.GetProjectRelativePath(PathUtils.GetItemMapGameMapPath());
                }
                
                GameObject addUiInstance = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
                if (addUiInstance == null)
                {
                    Debug.LogError("无效的Prefab文件");
                    return null;
                }
                return addUiInstance;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public static GameObject GetMap()
            {
                string assetPath = PathUtils.GetMapGameMapPath();
                if (assetPath==null)
                {
                    return null;
                }
                if (!PathUtils.GetMapGameMapPath().StartsWith("Assets/"))
                {
                   assetPath = FileUtil.GetProjectRelativePath(PathUtils.GetMapGameMapPath());
                }
             
                GameObject addUiInstance = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
                if (addUiInstance == null)
                {
                    Debug.LogError("无效的Prefab文件");
                    return null;
                }
                return addUiInstance;
            }

            #region 替换预制体指定节点的图片
            /// <summary>
            /// 核心方法：替换预制体指定节点的图片
            /// </summary>
            /// <param name="prefabPath">预制体</param>
            /// <param name="targetNodePath">目标节点路径</param>
            /// <param name="imageSourcePath">图片地址</param>
            /// <param name="isSavePrefab">是否保存修改</param>
            /// <returns>是否替换成功</returns>
            public static GameObject ReplaceImageInPrefab(GameObject prefabPath, string targetNodePath, string imageSourcePath, bool isSavePrefab,Type type)
            {
                if (prefabPath==null)
                {
                    Debug.LogError($"预制体不存在：{prefabPath}");
                    return null;
                }

                if (string.IsNullOrEmpty(targetNodePath))
                {
                    Debug.LogError("目标节点路径不能为空！");
                    return null;
                }

                if (string.IsNullOrEmpty(imageSourcePath))
                {
                    Debug.LogError("图片地址不能为空！");
                    return null;
                }
                try
                {
                    Transform targetTransform = FindTargetTransform(prefabPath.transform, targetNodePath);
                    if (targetTransform == null)
                    {
                        Debug.LogError($"未找到目标节点：{targetNodePath}");
                        return null;
                    }
                    
                    RawImage targetRawImage = targetTransform.GetComponent<RawImage>();
                    Image targetImage = targetTransform.GetComponent<Image>();
                    if (targetRawImage == null&&targetImage == null)
                    {
                        Debug.LogError($"目标节点 {targetNodePath} 没有targetRawImage组件！");
                        return null;
                    }
                    
                    Texture2D texture = null;
                    if (type==Type.LoadBackground)
                    {
                        texture = LoadImageTexture(PathUtils.GetItemStrPath(imageSourcePath));
                    }
                    else
                    {
                        texture = LoadImageTexture(PathUtils.GetItemIconPath(imageSourcePath));
                    }

                    Sprite newSprite = null;
                    if (targetRawImage != null)
                    {
                        targetRawImage.texture = texture as Texture2D;
                    }
                    else if (targetImage != null)
                    {
                        newSprite = Sprite.Create(
                            texture,
                            new Rect(0, 0, texture.width, texture.height),
                            new Vector2(0.5f, 0.5f), // 中心点
                            100f // Pixels Per Unit
                        );
                    }

                    if (texture == null)
                    {
                        Debug.LogError($"目标节点 {targetNodePath} 没有设置图片！");
                        return null;
                    }
                    
                    if (targetImage != null)
                    {
                        targetImage.sprite = newSprite;
                        targetImage.SetNativeSize();
                        targetImage.preserveAspect = true;
                        targetImage.SetAllDirty();
                    }
                    else if (targetRawImage != null)
                    {
                        targetRawImage.texture = texture;
                        targetRawImage.SetNativeSize();
                    }
                    return prefabPath;
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"替换图片异常：{e.Message}\n{e.StackTrace}");
                    return null;
                }
            }
            
            /// <summary>
            /// 查找目标节点（支持直接名称/层级路径）
            /// </summary>
            /// <param name="rootTrans">预制体根节点</param>
            /// <param name="nodePath">节点路径（如：IconImage 或 Root/IconImage）</param>
            /// <returns>目标节点Transform</returns>
            private static Transform FindTargetTransform(Transform rootTrans, string nodePath)
            {
                // 直接名称查找
                if (!nodePath.Contains("/"))
                {
                    return rootTrans.Find(nodePath);
                }

                // 层级路径查找（如：Root/IconImage）
                string[] pathParts = nodePath.Split('/');
                Transform currentTrans = rootTrans;

                foreach (string part in pathParts)
                {
                    currentTrans = currentTrans.Find(part);
                    if (currentTrans == null)
                    {
                        return null;
                    }
                }

                return currentTrans;
            }
            /// <summary>
            /// 加载图片纹理（支持本地路径/AssetDatabase路径/网络URL）
            /// </summary>
            /// <param name="imagePath">图片地址</param>
            /// <returns>纹理资源</returns>
            public static Texture2D LoadImageTexture(string imagePath)
            {
                Texture2D texture = null;
                if (imagePath.StartsWith("Assets/"))
                {
                    texture = AssetDatabase.LoadAssetAtPath<Texture2D>(imagePath);
                    if (texture != null)
                    {
                        return texture;
                    }
                }
                if (File.Exists(imagePath))
                {
                    byte[] imageBytes = File.ReadAllBytes(imagePath);
                    texture = new Texture2D(2, 2);
                    texture.LoadImage(imageBytes);
                    return texture;
                }
                Debug.LogError($"没有找到对应的图片资源：{imagePath}");
                return null;
            }
            #endregion

            
            /// <summary>
            /// 收集所有添加物体的信息：名称、位置、大小、图片路径
            /// </summary>
            public static MapAssetData CollectMapData()
            {
                MapAssetData assetData = new MapAssetData();
                assetData.m_MapHeight = m_MapWidth;
                assetData.m_MapHeight = m_MapHeight;

                List<MapObjectData> objList = new List<MapObjectData>();

                foreach (GameObject go in m_AddObjects)
                {
                    if (go == null) continue;

                    MapObjectData objData = new MapObjectData();
                    objData.name = go.name;

                    // 位置
                    RectTransform rt = go.GetComponent<RectTransform>();
                    if (rt != null)
                    {
                        objData.position = rt.anchoredPosition;
                        objData.size = rt.sizeDelta;
                    }

                    // 提取图片路径
                    Graphic graphic = go.GetComponent<Graphic>();
                    if (graphic != null && graphic.mainTexture != null)
                    {
                        string path = AssetDatabase.GetAssetPath(graphic.mainTexture);
                        objData.m_IconPath = path;
                    }

                    objList.Add(objData);
                }

                assetData.m_MapObjectData = objList;
                return assetData;
            }
            
            /// <summary>
            /// 转换成Int[]
            /// </summary>
            /// <param name="???"></param>
            /// <returns></returns>
            public static int[] GetIntList(string _list)
            {
                if (string.IsNullOrWhiteSpace(_list))
                {
                    return new int[0];  
                }
                string clean = _list.Trim('{', '}');
                clean = clean.Replace("\"", "");
                return clean.Split(',').Where(s => !string.IsNullOrWhiteSpace(s))
                    .Select(int.Parse)
                    .ToArray();
            }
            
        }

        /// <summary>
        /// 在相机中显示
        /// </summary>
        public static class SetShowCameraView
        {
            /// <summary>
            /// 初始化预览工具
            /// </summary>
            public static void InitRenderUtility()
            {
                if (m_RenderCam!=null)
                {
                    return;
                }
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
                m_RenderCam.cullingMask = -1; // 渲染所有层
                
                // 核心：设置专用射线检测相机
                _uiRaycastCamera = m_RenderCam;
                _lastClickTime = 0;
            }
        }
        /// <summary>
        /// 读取配置信息
        /// </summary>
        public static class LoadConfig
        {
            /// <summary>
            /// 获取配置文件信息
            /// </summary>
            public static void LoadConfigs()
            {
                //读取常规信息列表
                var _chapterData = TableExportEditorUtility.GetExcelData(m_TableMainLevelsPath);
                m_TableMainLevelsList = _chapterData.ToList<TableMainLevels>();
                //读取摆件信息列表
                var _itemIcon = TableExportEditorUtility.GetExcelData(m_TableItemsPath);
                m_TableItemsList = _itemIcon.ToList<ItemIcon>();
                //读取特殊地图信息列表
                var _levelData = TableExportEditorUtility.GetExcelData(m_TableDailyLevelsPath);
                m_TableDailyLevelsList = _levelData.ToList<LevelData>();
            }
        }
    }
}