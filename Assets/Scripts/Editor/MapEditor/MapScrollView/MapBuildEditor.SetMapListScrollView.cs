using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.MapEditor
{
    public sealed partial class MapBuildEditor
    {
        /// <summary>
        /// 存储所有读取到的地图章节配置数据列表
        /// </summary>
        private List<RMapChapterTypeData> _mapConfigs = new List<RMapChapterTypeData>();

        /// <summary>
        /// 地图配置列表当前选中项的索引（-1 = 未选中）
        /// </summary>
        private int _selectedIndex = -1;

        /// <summary>
        /// 【核心UI方法】在滚动视图中绘制地图配置数据列表
        /// 负责绘制列表容器、空数据提示、遍历渲染每一个可点击列表项
        /// </summary>
        private void OnSetMapJsonList()
        {
            // 绘制地图列表滚动视图，固定高度260
            m_MapListView = GUILayout.BeginScrollView(m_MapListView,
                GUILayout.MinHeight(260), GUILayout.Height(260));

            GUILayout.BeginVertical();

            // 无数据时显示空提示
            if (m_TableMainLevelsList.Count == 0)
            {
                GUILayout.Space(20);
                GUILayout.Label("暂无地图配置数据", EditorStyles.centeredGreyMiniLabel);
            }
            else
            {
                // 遍历所有地图配置，绘制每一个列表项
                for (int i = 0; i < m_TableMainLevelsList.Count; i++)
                {
                    GUILayout.BeginVertical("Box");
                    DrawClickableListItem(m_TableMainLevelsList[i], i);
                    GUILayout.EndVertical();
                    GUILayout.Space(2);
                }
            }

            GUILayout.EndVertical();
            GUILayout.EndScrollView();
        }

        /// <summary>
        /// 绘制单个可点击的地图列表项
        /// 根据选中状态切换样式，点击后触发选中逻辑
        /// </summary>
        /// <param name="tableMainLevelsEditor">当前项的地图配置数据</param>
        /// <param name="index">当前项在列表中的索引</param>
        private void DrawClickableListItem(TableMainLevelsEditor tableMainLevelsEditor, int index)
        {
            // 当前项被选中：使用Box样式高亮显示
            if (_selectedIndex == index)
            {
                GUILayout.BeginHorizontal("Box");
            }
            else
            {
                GUILayout.BeginHorizontal();
            }

            // 显示关卡ID文字按钮，点击即选中该项
            if (GUILayout.Button($"{tableMainLevelsEditor.LevelID}", EditorStyles.label, GUILayout.Width(165)))
            {
                _selectedIndex = index;
                ChooseItem(tableMainLevelsEditor);
            }

            GUILayout.EndHorizontal();
        }

        /// <summary>
        /// 【选中回调】点击列表项后执行：加载对应地图、切换背景、刷新图标列表
        /// </summary>
        /// <param name="tableMainLevelsEditor">选中的地图配置数据</param>
        private void ChooseItem(TableMainLevelsEditor tableMainLevelsEditor)
        {
            try
            {
                // 替换地图背景图并获取地图预制体
                GameObject prefabPath = Utils.ReplaceImageInPrefab(Utils.GetMap(), "RawImage",
                    tableMainLevelsEditor.Level_bg, false, Type.LoadBackground);

                if (prefabPath != null)
                {
                    // 加载地图预制体，完成后刷新预览区域
                    LoadMapPrefab(prefabPath, () => { SetPreviewRect(); });
                }

                // 保存当前选中的地图数据
                _mTableMainLevelsEditor = tableMainLevelsEditor;
                mapName = tableMainLevelsEditor.LevelID;
                mapId = tableMainLevelsEditor.ID;

                // 重置图标列表标记，刷新图标显示
                m_IsInitIconList = false;
                SetIconItem(tableMainLevelsEditor);

                Debug.Log($"选中地图：{tableMainLevelsEditor.LevelID}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}