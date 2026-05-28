/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  MapBuildEditor.ChapterList.cs
 * author:    云毅
 * created:   2026
 * descrip:   地图编辑器 - 章节&小节列表管理模块（列表绘制、选中交互、数据加载）
 ***************************************************************/

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Editor.MapEditor
{
    /// <summary>
    /// 地图编辑器 partial 类 - 章节与小节列表管理
    /// </summary>
    public sealed partial class MapBuildEditor
    {
        #region 地图章节列表字段
        /// <summary>
        /// 存储所有读取到的地图章节配置数据列表
        /// </summary>
        private List<RMapChapterTypeData> _mapConfigs = new List<RMapChapterTypeData>();

        /// <summary>
        /// 地图配置列表当前选中项的索引（-1 = 未选中）
        /// </summary>
        private int _selectedIndex = -1;

        /// <summary>
        /// 当前选中的小节ID（-1 = 未选中）
        /// </summary>
        private int _selectedSubId = -1;
        #endregion

        #region 章节列表绘制
        /// <summary>
        /// 【核心UI方法】在滚动视图中绘制地图配置数据列表
        /// 负责绘制列表容器、空数据提示、遍历渲染每一个可点击列表项
        /// </summary>
        private void OnSetMapJsonList()
        {
            // 绘制地图列表滚动视图，固定高度260
            m_MapListView = GUILayout.BeginScrollView(m_MapListView, GUILayout.MinHeight(260), GUILayout.Height(260));

            GUILayout.BeginVertical();

            // 无数据时显示空提示
            if (m_TableChapterList.Count == 0)
            {
                GUILayout.Space(20);
                GUILayout.Label("暂无地图配置数据", EditorStyles.centeredGreyMiniLabel);
            }
            else
            {
                // 遍历所有地图配置，绘制每一个列表项
                for (int i = 0; i < m_TableChapterList.Count; i++)
                {
                    GUILayout.BeginVertical("Box");
                    DrawClickableListItem(m_TableChapterList[i], i);
                    GUILayout.EndVertical();
                    GUILayout.Space(2);
                }
            }

            GUILayout.EndVertical();
            GUILayout.EndScrollView();
        }

        /// <summary>
        /// 绘制单个可点击的地图列表项（支持展开小节）
        /// 根据选中状态切换样式，点击后触发选中逻辑
        /// </summary>
        /// <param name="tableMainLevelsEditor">当前项的地图配置数据</param>
        /// <param name="index">当前项在列表中的索引</param>
        private void DrawClickableListItem(TableChapterEditor tableMainLevelsEditor, int index)
        {
            // 安全判断：防止越界
            if (index < 0 || m_TableChapterList == null || index >= m_TableChapterList.Count)
                return;
            
            bool isSelected = _selectedIndex == index;

            // 整体项容器
            GUILayout.BeginVertical(isSelected ? "Box" : GUIStyle.none);
            {
                // ===================== 章节标题行（左对齐） =====================
                GUILayout.BeginHorizontal();
                {
                    // 左对齐关键：去掉固定宽度，使用 Left 对齐
                    if (GUILayout.Button($"第 {tableMainLevelsEditor.ID} 章", 
                            EditorStyles.label, 
                            GUILayout.ExpandWidth(true)))
                    {
                        _selectedIndex = index;
                        _selectedSubId = -1;
                    }
                }
                GUILayout.EndHorizontal();

                // ===================== 选中时展开小节（左对齐 + 缩进） =====================
                if (isSelected)
                {
                    GUILayout.Space(4);
            
                    // 缩进20像素，保证左对齐
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(20);
                    GUILayout.BeginVertical();
                    {
                        int[] subLevelIds = SetTypeConversion.OnStringToInt(tableMainLevelsEditor.ChapterInfoID);
                
                        if (subLevelIds.Length == 0)
                        {
                            GUILayout.Label("暂无小节数据", EditorStyles.miniLabel);
                        }
                        else
                        {
                            foreach (int subId in subLevelIds)
                            {
                                // 判断当前小节是否被选中
                                bool isSubSelected = _selectedSubId == subId;

                                // 选中：加粗显示
                                GUIStyle style = isSubSelected ? EditorStyles.boldLabel : EditorStyles.miniLabel;

                                // 可点击的小节按钮
                                if (GUILayout.Button($"第 {subId} 节", style, GUILayout.ExpandWidth(true)))
                                {
                                    _selectedSubId = subId;
                                    Debug.Log($"已选中 → 第 {tableMainLevelsEditor.ID} 章 ----->第 {subId} 节");
        
                                    // 安全判断：防止越界
                                    if (subId-1 < 0 || m_TableMainLevelsList == null || subId-1 >= m_TableMainLevelsList.Count)
                                        return;
                                    
                                    // 选中小节后的逻辑
                                    ChooseItem(m_TableMainLevelsList[subId-1]);
                                }
                            }
                        }
                    }
                    GUILayout.EndVertical();
                    GUILayout.EndHorizontal();
                }
            }
            GUILayout.EndVertical();
        }
        #endregion

        #region 选中项处理
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
        #endregion
    }
}