using System.IO;
using UnityEditor;
using UnityEngine;

namespace Editor.MapEditor
{
    public sealed partial class MapBuildEditor
    {
        private void SetLeftPanel()
        {
            // ===================== 左侧面板 总布局 =====================
            GUILayout.BeginVertical(GUILayout.Width(200), GUILayout.Height(860));
            // 上方区域
            GUI.backgroundColor = new Color(0.3f, 0.3f, 0.3f, 1);
            GUILayout.BeginVertical("Box"); // 1
            GUILayout.Space(5);
            // 按钮行
            GUILayout.BeginHorizontal(); // 2
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("新建", GUILayout.Width(80), GUILayout.Height(24)))
            {
                RefreshMapList();
            }
            if (GUILayout.Button("删除", GUILayout.Width(80), GUILayout.Height(24)))
            {
                DeleteSelectedObject();
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal(); // 关闭 2
            // 地图列表区域
            GUI.backgroundColor = new Color(0.3f, 0.3f, 0.3f, 0.2f);
            GUILayout.BeginVertical("Box", GUILayout.Height(276)); // 3
            OnSetMapJsonList(); // 你已经修复好的方法
            GUILayout.EndVertical(); // 关闭 3
            GUILayout.EndVertical(); // 关闭 1
            // 上方区域结束
            GUILayout.Space(5);
            GUILayout.Box("", GUILayout.Height(3), GUILayout.ExpandWidth(true));
            GUILayout.Space(5);
            // 下方物件区域
            GUI.backgroundColor = new Color(0.3f, 0.3f, 0.3f, 1);
            GUILayout.BeginVertical("Box"); // 4

            // 按钮行
            GUILayout.BeginHorizontal(); // 5
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
            GUILayout.EndHorizontal(); // 关闭 5
            // 图标列表
            GUI.backgroundColor = new Color(0.3f, 0.3f, 0.3f, 1);
            GUILayout.BeginVertical("Box", GUILayout.Height(710)); // 6
            OnSetIconList();
            GUILayout.EndVertical(); // 关闭 6

            GUILayout.EndVertical(); // 关闭 4
            // 下方物件区域结束
            GUILayout.EndVertical(); // 关闭最外层总布局
            // ===================== 左侧面板结束 =====================
        }

        /// <summary>
        /// 删除选择地图
        /// </summary>
        private void DeleteSelectedObject()
        {
            Debug.Log("DeleteSelectedObject");
            if (_selectedIndex != -1 || m_MapJsonList.Count == 0)
            {
                Debug.LogWarning("请先选中要删除的地图文件");
                return;
            }

            // 获取选中项对应的JSON文件信息
            var selectedName = m_MapJsonList[_selectedIndex].m_DisplayName;
            var selectedJson = m_MapJsonList[_selectedIndex];
            var _FilePath = selectedJson.m_FilePath + "/" + selectedName;
            if (selectedJson != null && File.Exists(_FilePath))
            {
                // 删除文件
                File.Delete(_FilePath);
                Debug.Log($"删除地图文件：{_FilePath}");
                // 刷新列表
                RefreshMapList();
                if (m_MapJsonList.Count > 0)
                {
                    _selectedIndex = 0;
                }
                else
                {
                    _selectedIndex = -1;
                }
            }
        }

        /// <summary>
        /// 创建对应的Item信息
        /// </summary>
        private void CreateObjectItem()
        {
            Debug.Log("CreateObjectItem");
            GetIconPath("");
        }

        /// <summary>
        /// 
        /// </summary>
        private void BuildMap()
        {
            Debug.Log("BuildMap");
            // // 打开新建地图弹窗
            var _MapWindow = MapWindow.ShowWindow();
            // // 绑定创建成功后的回调（刷新地图列表）
            _MapWindow.OnMapCreated = () =>
            {
                RefreshMapList(); // 刷新主窗口的地图列表
            };
        }

        /// <summary>
        /// 刷新地图列表（重新读取JSON文件）
        /// </summary>
        private void RefreshMapList()
        {
            Debug.Log("刷新地图列表");
            LoadConfig.LoadConfigs();
            //Utils.LoadJsonFiles();
            LoadPrefabAndExtractImages(PathUtils.GetMapGameMapPath());
        }
    }
}