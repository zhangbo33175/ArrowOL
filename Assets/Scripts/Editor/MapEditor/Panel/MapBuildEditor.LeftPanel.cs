/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  MapBuildEditor.LeftPanel.cs
 * author:    云毅
 * created:   2026
 * descrip:   地图编辑器 - 左侧功能面板（地图管理 + 图标素材管理）
 ***************************************************************/

using System.IO;
using UnityEditor;
using UnityEngine;

namespace Editor.MapEditor
{
    /// <summary>
    /// 地图编辑器 - 左侧面板UI绘制模块
    /// </summary>
    public sealed partial class MapBuildEditor
    {
        #region 左侧面板整体绘制
        /// <summary>
        /// 绘制编辑器【左侧功能面板】整体UI
        /// 包含：地图操作按钮、地图配置列表、物件操作按钮、图标素材列表
        /// </summary>
        private void SetLeftPanel()
        {
            //=========================================================================
            // 左侧面板 总布局
            //=========================================================================
            GUILayout.BeginVertical(GUILayout.Width(200), GUILayout.Height(860));

            //=========================================================================
            // 上方：地图管理区域
            //=========================================================================
            GUI.backgroundColor = new Color(0.3f, 0.3f, 0.3f, 1);
            GUILayout.BeginVertical("Box");
            GUILayout.Space(5);

            // 新建 / 删除 按钮行
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            
            if (GUILayout.Button("新建", GUILayout.Width(80), GUILayout.Height(24)))
            {
                BuildMap();
            }
            
            if (GUILayout.Button("删除", GUILayout.Width(80), GUILayout.Height(24)))
            {
                DeleteSelectedObject();
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            // 地图配置列表区域
            GUI.backgroundColor = new Color(0.3f, 0.3f, 0.3f, 0.2f);
            GUILayout.BeginVertical("Box", GUILayout.Height(276));
            OnSetMapJsonList();
            GUILayout.EndVertical();

            GUILayout.EndVertical();
            //=========================================================================
            // 上方区域结束
            //=========================================================================

            // 分割线
            GUILayout.Space(5);
            GUILayout.Box("", GUILayout.Height(3), GUILayout.ExpandWidth(true));
            GUILayout.Space(5);

            //=========================================================================
            // 下方：物件/图标管理区域
            //=========================================================================
            GUI.backgroundColor = new Color(0.3f, 0.3f, 0.3f, 1);
            GUILayout.BeginVertical("Box");

            // 物件 / 刷新 按钮行
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            
            if (GUILayout.Button("物件", GUILayout.Width(80), GUILayout.Height(24)))
            {
                CreateObjectItem();
            }

            GUILayout.Space(10);
            
            if (GUILayout.Button("刷新", GUILayout.Width(80), GUILayout.Height(24)))
            {
                RefreshMapList();
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            // 图标素材列表区域
            GUI.backgroundColor = new Color(0.3f, 0.3f, 0.3f, 1);
            GUILayout.BeginVertical("Box", GUILayout.Height(710));
            OnSetIconList();
            GUILayout.EndVertical();

            GUILayout.EndVertical();
            //=========================================================================
            // 下方区域结束
            //=========================================================================

            GUILayout.EndVertical();
            //=========================================================================
            // 左侧面板结束
            //=========================================================================
        }
        #endregion

        #region 地图操作功能
        /// <summary>
        /// 删除【选中的地图配置文件】
        /// 从磁盘删除JSON文件，并自动刷新列表
        /// </summary>
        private void DeleteSelectedObject()
        {
            Debug.Log("DeleteSelectedObject");

            // 校验：未选中 或 列表为空
            if (_selectedIndex == -1 || m_MapJsonList.Count == 0)
            {
                Debug.LogWarning("请先选中要删除的地图文件");
                return;
            }

            // 获取选中项的文件路径
            var selectedJson = m_MapJsonList[_selectedIndex];
            string selectedName = selectedJson.m_DisplayName;
            string filePath = selectedJson.m_FilePath + "/" + selectedName;

            // 删除文件并刷新
            if (selectedJson != null && File.Exists(filePath))
            {
                File.Delete(filePath);
                Debug.Log($"删除地图文件：{filePath}");

                // 刷新列表并重置选中状态
                RefreshMapList();
                _selectedIndex = m_MapJsonList.Count > 0 ? 0 : -1;
            }
        }

        /// <summary>
        /// 打开【新建地图】配置窗口
        /// </summary>
        private void BuildMap()
        {
            Debug.Log("BuildMap");
            
            // 打开新建地图弹窗
            var mapWindow = MapWindow.ShowWindow();
            // 绑定创建成功回调：创建完成后自动刷新地图列表
            mapWindow.OnMapCreated = RefreshMapList;
        }

        /// <summary>
        /// 刷新地图列表
        /// 重新读取配置文件 + 重新加载地图预制体与图标
        /// </summary>
        private void RefreshMapList()
        {
            Debug.Log("刷新地图列表");
            
            LoadConfig.LoadConfigs();
            LoadPrefabAndExtractImages(PathUtils.GetMapGameMapPath());
        }
        #endregion

        #region 图标素材操作
        /// <summary>
        /// 加载/显示【可拖拽物件图标列表】
        /// 读取图标路径并构建预览列表
        /// </summary>
        private void CreateObjectItem()
        {
            Debug.Log("CreateObjectItem");
            
            GetIconPath("");
        }
        #endregion
    }
}