/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  MapWindow.cs
 * author:    云毅
 * created:   2026
 * descrip:   地图编辑器 - 新建地图弹窗（配置、路径、JSON创建）
 ***************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Editor.MapEditor
{
    /// <summary>
    /// 新建地图弹窗窗口
    /// 功能：配置地图名称、保存路径、背景图、Item资源路径，并创建初始JSON地图文件
    /// </summary>
    public class MapWindow : EditorWindow
    {
        //=========================================================================
        // 窗口参数
        //=========================================================================
        #region Window Parameters
        /// <summary>
        /// 地图名称（默认前缀）
        /// </summary>
        private string _mapName = "level_map_";

        /// <summary>
        /// 地图JSON文件保存路径
        /// </summary>
        private string _mapSavePath;

        /// <summary>
        /// 背景图片路径
        /// </summary>
        private string _bgImagePath = "";

        /// <summary>
        /// Item图标资源文件夹路径
        /// </summary>
        private string _bgIconPath = "";

        /// <summary>
        /// 地图创建成功回调（通知主窗口刷新）
        /// </summary>
        public Action OnMapCreated;
        #endregion

        //=========================================================================
        // 窗口显示
        //=========================================================================
        #region Show Window
        /// <summary>
        /// 打开新建地图弹窗（模态窗口，阻塞主界面）
        /// </summary>
        public static MapWindow ShowWindow()
        {
            MapWindow window = CreateInstance<MapWindow>();
            window.titleContent = new GUIContent("新建地图");
            // 固定窗口大小
            window.minSize = new Vector2(400, 250);
            window.maxSize = new Vector2(400, 250);
            window.ShowModal();
            return window;
        }
        #endregion

        //=========================================================================
        // 生命周期
        //=========================================================================
        #region Lifecycle
        /// <summary>
        /// 窗口启用时初始化默认路径
        /// </summary>
        private void OnEnable()
        {
            // 默认使用地图编辑器配置的关卡保存目录
            _mapSavePath = MapBuildEditor.PathUtils.GetLevelDirectoryFullPath();
        }
        #endregion

        //=========================================================================
        // GUI 绘制
        //=========================================================================
        #region GUI Drawing
        /// <summary>
        /// 绘制窗口UI界面
        /// </summary>
        private void OnGUI()
        {
            GUILayout.Label("新建地图配置", EditorStyles.boldLabel);
            GUILayout.Space(5);

            // 地图名称输入框
            GUILayout.Label("地图名称", EditorStyles.label);
            _mapName = GUILayout.TextField(_mapName, GUILayout.Height(24));
            GUILayout.Space(3);

            // 地图保存路径 + 选择文件夹按钮
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

            // 背景图片路径 + 选择文件按钮
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

            // Item图标文件夹路径 + 选择文件夹按钮
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

            // 底部：取消 / 创建 按钮
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            // 取消按钮
            if (GUILayout.Button("取消", GUILayout.Height(32), GUILayout.Width(100)))
            {
                Close();
            }

            // 创建按钮：名称和路径不为空才可点击
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
        #endregion

        //=========================================================================
        // 新建地图逻辑
        //=========================================================================
        #region Create New Map Logic
        /// <summary>
        /// 执行新建地图逻辑
        /// 1. 检查路径
        /// 2. 覆盖提示
        /// 3. 创建JSON配置文件
        /// 4. 触发成功回调
        /// </summary>
        private void CreateNewMap()
        {
            try
            {
                // 目录不存在则创建
                if (!Directory.Exists(_mapSavePath))
                {
                    Directory.CreateDirectory(_mapSavePath);
                }

                // 拼接JSON文件完整路径
                string jsonFilePath = Path.Combine(_mapSavePath, $"{_mapName}.json");

                // 文件已存在 → 提示是否覆盖
                if (File.Exists(jsonFilePath))
                {
                    if (!EditorUtility.DisplayDialog("提示", $"地图文件 {_mapName}.json 已存在，是否覆盖？", "覆盖", "取消"))
                    {
                        return;
                    }
                }

                // 构建地图数据实体
                RMapChapterTypeData rMapChapterTypeData = new RMapChapterTypeData
                {
                    m_MapName = _mapName,
                    m_MapObjectData = new List<RMapData>(), // 空物体列表
                    m_BackgroundPath = _bgImagePath, // 背景图路径
                    m_CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") // 创建时间
                };

                // 构建JSON文件信息
                MapBuildEditor.MapJsonInfo jsonInfo = new MapBuildEditor.MapJsonInfo
                {
                    m_FilePath = _mapSavePath,
                    m_FileName = _mapName,
                    m_DisplayName = _mapName + ".json",
                    mRMapChapterTypeData = rMapChapterTypeData
                };

                // 序列化为JSON并写入文件
                string jsonContent = JsonUtility.ToJson(jsonInfo, true);
                File.WriteAllText(jsonFilePath, jsonContent);

                // 提示成功
                EditorUtility.DisplayDialog("成功", $"地图 {_mapName} 创建完成！", "确定");

                // 通知主窗口刷新列表
                OnMapCreated?.Invoke();

                // 关闭窗口
                Close();
            }
            catch (Exception e)
            {
                // 异常提示
                EditorUtility.DisplayDialog("错误", $"创建地图失败：{e.Message}", "确定");
            }
        }
        #endregion
    }
}