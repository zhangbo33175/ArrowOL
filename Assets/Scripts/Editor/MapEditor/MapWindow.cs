using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Editor.MapEditor
{
    public class MapWindow : EditorWindow
    {
        // 表单数据
        private string _mapName = "level_map_"; // 默认地图名称
        private string _mapSavePath = MapBuildEditor.PathUtils.GetLevelDirectoryFullPath(); // 地图JSON保存路径
        private string _bgImagePath = ""; // 背景图片路径
        private string _bgIconPath = ""; // 背景图片路径
        public System.Action OnMapCreated;
        public static MapWindow ShowWindow()
        {
            var window = CreateInstance<MapWindow>();
            window.titleContent = new GUIContent("新建地图");
            window.minSize = new Vector2(400, 250);
            window.maxSize = new Vector2(400, 250);
            window.ShowModal(); // 模态窗口（阻塞主窗口操作）
            return window;
        }

        private void OnEnable()
        {
            // 默认地图保存路径设为主窗口的JSON文件夹
            _mapSavePath = MapBuildEditor.PathUtils.GetLevelDirectoryFullPath();
        }

        private void OnGUI()
        {
            GUILayout.Label("新建地图配置", EditorStyles.boldLabel);
            GUILayout.Space(5);
     
            GUILayout.Label("地图名称", EditorStyles.label);
            _mapName = GUILayout.TextField(_mapName, GUILayout.Height(24));
            GUILayout.Space(3);
            GUILayout.Label("地图保存路径", EditorStyles.label);
            GUILayout.BeginHorizontal();
            GUILayout.TextField(_mapSavePath, GUILayout.Height(24), GUILayout.ExpandWidth(true));
            if (GUILayout.Button("选择", GUILayout.Height(24), GUILayout.Width(80)))
            {
                string selectPath = EditorUtility.OpenFolderPanel("选择地图保存路径", _mapSavePath, "");
                if (!string.IsNullOrEmpty(selectPath))
                {
                    _mapSavePath = selectPath + "/";
                }
            }

            GUILayout.EndHorizontal();
            GUILayout.Space(3);
            GUILayout.Label("背景图片路径", EditorStyles.label);
            GUILayout.BeginHorizontal();
            GUILayout.TextField(_bgImagePath, GUILayout.Height(18), GUILayout.ExpandWidth(true));
            if (GUILayout.Button("选择", GUILayout.Height(18), GUILayout.Width(80)))
            {
                string selectPath = EditorUtility.OpenFilePanel("选择背景图片", "", "png,jpg,jpeg");
                if (!string.IsNullOrEmpty(selectPath))
                {
                    _bgImagePath = selectPath;
                }
            }

            GUILayout.EndHorizontal();

            GUILayout.Space(3);
            GUILayout.Label("Item图片信息", EditorStyles.label);
            GUILayout.BeginHorizontal();
            GUILayout.TextField(_bgIconPath, GUILayout.Height(18), GUILayout.ExpandWidth(true));
            if (GUILayout.Button("选择", GUILayout.Height(18), GUILayout.Width(80)))
            {
                string folderPath = EditorUtility.OpenFolderPanel("选择图片文件夹", Application.dataPath, "");
                if (!string.IsNullOrEmpty(folderPath))
                {
                    _bgIconPath = folderPath;
                }
            }

            GUILayout.EndHorizontal();
    
            GUILayout.Space(8);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("取消", GUILayout.Height(32), GUILayout.Width(100)))
            {
                Close();
            }

            bool canCreate = !string.IsNullOrEmpty(_mapName) && !string.IsNullOrEmpty(_mapSavePath);
            using (new EditorGUI.DisabledScope(!canCreate))
            {
                if (GUILayout.Button("创建", GUILayout.Height(32), GUILayout.Width(100)))
                {
                    CreateNewMap();
                }
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        /// <summary>
        /// 执行新建地图逻辑（创建JSON文件）
        /// </summary>
        private void CreateNewMap()
        {
            try
            {
                if (!Directory.Exists(_mapSavePath))
                {
                    Directory.CreateDirectory(_mapSavePath);
                }
                string jsonFilePath = Path.Combine(_mapSavePath, $"{_mapName}.json");
                
                if (File.Exists(jsonFilePath))
                {
                    if (!EditorUtility.DisplayDialog("提示", $"地图文件 {_mapName}.json 已存在，是否覆盖？", "覆盖", "取消"))
                    {
                        return;
                    }
                }
                ///
                LevelData levelData = new LevelData();
                //
                MapAssetData mapAssetData = new MapAssetData
                {
                    m_MapName = _mapName,
                    m_MapObjectData = new List<MapObjectData>(),
                    m_IconPath = _bgIconPath,
                    m_BackgroundPath = _bgImagePath,
                    m_CcreateTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                };
                MapBuildEditor.MapJsonInfo jsonInfo = new MapBuildEditor.MapJsonInfo
                {
                    m_FilePath = _mapSavePath,
                    m_FileName = _mapName,
                    m_DisplayName = _mapName + ".json",
                    mMapAssetData = mapAssetData
                };
                string jsonContent = JsonUtility.ToJson(jsonInfo, true);
                File.WriteAllText(jsonFilePath, jsonContent);
                
                EditorUtility.DisplayDialog("成功", $"地图 {_mapName} 创建完成！", "确定");
                OnMapCreated?.Invoke(); 
                Close(); 
            }
            catch (System.Exception e)
            {
                EditorUtility.DisplayDialog("错误", $"创建地图失败：{e.Message}", "确定");
            }
        }
    }
}