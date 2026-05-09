using System.IO;
using UnityEditor;
using UnityEngine;

namespace Editor.MapEditor
{
    public sealed partial class MapBuildEditor
    {
        /// <summary>
        /// 绘制编辑器【左侧功能面板】整体UI
        /// 包含：地图操作按钮、地图配置列表、物件操作按钮、图标素材列表
        /// </summary>
        private void SetLeftPanel()
        {
            // ===================== 左侧面板 总布局 =====================
            GUILayout.BeginVertical(GUILayout.Width(200), GUILayout.Height(860));

            // -------------------- 上方：地图管理区域 --------------------
            GUI.backgroundColor = new Color(0.3f, 0.3f, 0.3f, 1);
            GUILayout.BeginVertical("Box"); // 面板容器 1
            GUILayout.Space(5);

            // 新建 / 删除 按钮行
            GUILayout.BeginHorizontal(); // 按钮容器 2
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("新建", GUILayout.Width(80), GUILayout.Height(24)))
            {
                BuildMap(); // 打开新建地图窗口
            }

            if (GUILayout.Button("删除", GUILayout.Width(80), GUILayout.Height(24)))
            {
                DeleteSelectedObject(); // 删除选中的地图配置
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal(); // 关闭按钮容器 2

            // 地图配置列表区域
            GUI.backgroundColor = new Color(0.3f, 0.3f, 0.3f, 0.2f);
            GUILayout.BeginVertical("Box", GUILayout.Height(276)); // 列表容器 3
            OnSetMapJsonList(); // 绘制地图JSON配置列表
            GUILayout.EndVertical(); // 关闭列表容器 3

            GUILayout.EndVertical(); // 关闭面板容器 1
            // -------------------- 上方区域结束 --------------------

            // 分割线
            GUILayout.Space(5);
            GUILayout.Box("", GUILayout.Height(3), GUILayout.ExpandWidth(true));
            GUILayout.Space(5);

            // -------------------- 下方：物件/图标管理区域 --------------------
            GUI.backgroundColor = new Color(0.3f, 0.3f, 0.3f, 1);
            GUILayout.BeginVertical("Box"); // 面板容器 4

            // 物件 / 刷新 按钮行
            GUILayout.BeginHorizontal(); // 按钮容器 5
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("物件", GUILayout.Width(80), GUILayout.Height(24)))
            {
                CreateObjectItem(); // 加载/显示可拖拽的物件图标
            }

            GUILayout.Space(10);
            if (GUILayout.Button("刷新", GUILayout.Width(80), GUILayout.Height(24)))
            {
                RefreshMapList(); // 刷新整个地图配置数据
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal(); // 关闭按钮容器 5

            // 图标素材列表区域（可点击选中，用于添加到地图）
            GUI.backgroundColor = new Color(0.3f, 0.3f, 0.3f, 1);
            GUILayout.BeginVertical("Box", GUILayout.Height(710)); // 列表容器 6
            OnSetIconList(); // 绘制图标素材列表
            GUILayout.EndVertical(); // 关闭列表容器 6

            GUILayout.EndVertical(); // 关闭面板容器 4
            // -------------------- 下方区域结束 --------------------

            GUILayout.EndVertical(); // 关闭最外层总布局
            // ===================== 左侧面板结束 =====================
        }

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
        /// 加载/显示【可拖拽物件图标列表】
        /// 读取图标路径并构建预览列表
        /// </summary>
        private void CreateObjectItem()
        {
            Debug.Log("CreateObjectItem");
            GetIconPath("");
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
            LoadConfig.LoadConfigs(); // 读取地图配置
            LoadPrefabAndExtractImages(PathUtils.GetMapGameMapPath()); // 加载预制体并提取图片
        }
    }
}