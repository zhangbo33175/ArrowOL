/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  MapBuildEditor.SetMapListScrollView.cs
 * author:    云毅
 * created:   2026
 * descrip:   地图编辑器 - 章节&小节列表管理模块（列表绘制、选中交互、数据加载）
 ***************************************************************/

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

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
        /// 当前选中的小节 ID（-1 = 未选中）
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
            // 绘制地图列表滚动视图，固定高度 260
            m_MapListView = GUILayout.BeginScrollView(m_MapListView, GUILayout.MinHeight(260), GUILayout.Height(260));

            GUILayout.BeginVertical();

            // 无数据（列表为空或配置尚未加载完成）时显示空提示，避免空引用
            if (m_TableChapterList == null || m_TableChapterList.Count == 0)
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
        /// <param name="tableChapterEditor">当前项的地图配置数据</param>
        /// <param name="index">当前项在列表中的索引</param>
        private void DrawClickableListItem(TableChapterEditor tableChapterEditor, int index)
        {
            // 安全判断：防止越界
            if (index < 0 || m_TableChapterList == null || index >= m_TableChapterList.Count)
                return;

            bool isSelected = _selectedIndex == index;

            // 整体项容器（始终与 EndVertical 配对，保证 GUI 布局栈平衡）
            GUILayout.BeginVertical(isSelected ? "Box" : GUIStyle.none);
            {
                // ===================== 章节标题行（左对齐） =====================
                DrawChapterTitleRow(tableChapterEditor, index);

                // ===================== 选中时展开小节（左对齐 + 缩进） =====================
                if (isSelected)
                {
                    // DrawExpandedSubLevels 恒为真且内部布局完整闭合，不会提前中断
                    DrawExpandedSubLevels(tableChapterEditor);
                }
            }
            GUILayout.EndVertical();
        }

        /// <summary>
        /// 绘制章节标题行（点击切换选中章节并清空小节选中）
        /// </summary>
        /// <param name="tableChapterEditor">当前项的地图配置数据</param>
        /// <param name="index">当前项在列表中的索引</param>
        private void DrawChapterTitleRow(TableChapterEditor tableChapterEditor, int index)
        {
            GUILayout.BeginHorizontal();
            {
                // 左对齐关键：去掉固定宽度，使用 Left 对齐
                if (GUILayout.Button($"第 {tableChapterEditor.ID} 章",
                        EditorStyles.label,
                        GUILayout.ExpandWidth(true)))
                {
                    _selectedIndex = index;
                    _selectedSubId = -1;
                }
            }
            GUILayout.EndHorizontal();
        }

        /// <summary>
        /// 绘制选中章节下展开的小节列表（缩进显示，点击小节触发选中回调）
        /// <para>注意：该方法始终完整闭合所有 Begin/End 布局，且恒返回 true，
        /// 避免提前 return 破坏 IMGUI 布局栈（GUIClip 失衡）导致后续绘制异常。</para>
        /// </summary>
        /// <param name="tableChapterEditor">当前项的地图配置数据</param>
        private void DrawExpandedSubLevels(TableChapterEditor tableChapterEditor)
        {
            GUILayout.Space(4);

            // 缩进 20 像素，保证左对齐
            GUILayout.BeginHorizontal();
            GUILayout.Space(20);
            GUILayout.BeginVertical();
            {
                int[] subLevelIds = SetTypeConversion.OnStringToInt(tableChapterEditor.ChapterInfoID);

                if (subLevelIds.Length == 0)
                {
                    GUILayout.Label("暂无小节数据", EditorStyles.miniLabel);
                }
                else
                {
                    for (int i = 0; i < subLevelIds.Length; i++)
                    {
                        int subId = subLevelIds[i];

                        // 判断当前小节是否被选中
                        bool isSubSelected = _selectedSubId == subId;

                        // 选中：加粗显示
                        GUIStyle style = isSubSelected ? EditorStyles.boldLabel : EditorStyles.miniLabel;

                        // 可点击的小节按钮
                        if (GUILayout.Button($"第 {subId} 节", style, GUILayout.ExpandWidth(true)))
                        {
                            _selectedSubId = subId;
                            Debug.Log($"已选中 → 第 {tableChapterEditor.ID} 章 ----->第 {subId} 节");

                            // 越界安全判断：索引不合法时仅告警并跳过，不再中断当前布局
                            if (!IsValidSubLevelIndex(subId))
                            {
                                Debug.LogWarning($"小节 {subId} 在主关卡列表中不存在，已忽略本次选择。");
                                continue;
                            }

                            // 选中小节后的逻辑
                            ChooseItem(m_TableMainLevelsList[subId - 1], tableChapterEditor, i);
                        }
                    }
                }
            }
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }

        /// <summary>
        /// 校验小节索引是否指向有效的主关卡列表元素
        /// <para>小节 ID 与主关卡列表按「ID - 1」映射，需保证映射后索引在合法范围内。</para>
        /// </summary>
        /// <param name="subId">小节 ID</param>
        /// <returns>true 表示索引合法；false 表示越界或主关卡列表为空</returns>
        private bool IsValidSubLevelIndex(int subId)
        {
            return subId - 1 >= 0
                   && m_TableMainLevelsList != null
                   && subId - 1 < m_TableMainLevelsList.Count;
        }
        #endregion

        #region 选中项处理
        /// <summary>
        /// 【选中回调】点击列表项后执行：加载对应地图、切换背景、刷新图标列表
        /// </summary>
        /// <param name="tableMainLevelsEditor">选中的地图配置数据</param>
        /// <param name="tableChapterEditor">所属章节配置数据</param>
        /// <param name="subLevelIndex">当前小节在章节小节数组中的索引（从 0 起始）</param>
        private void ChooseItem(TableMainLevelsEditor tableMainLevelsEditor, TableChapterEditor tableChapterEditor, int subLevelIndex)
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

                _mTablesElectedLevelsEditor = new TablesElectedLevelsEditor();
                // 地图章节 ID
                _mTablesElectedLevelsEditor.ChapterId = tableChapterEditor.ID;
                // 地图预制体名称
                _mTablesElectedLevelsEditor.MapName = tableChapterEditor.MapName;

                _mTablesElectedLevelsEditor.LevelId = tableMainLevelsEditor.ID.ToString();
                _mTablesElectedLevelsEditor.MapId = subLevelIndex + 1;

                // 保存当前选中的地图数据
                mapName = tableMainLevelsEditor.Level_bg;
                _mTablesElectedLevelsEditor.Background = tableMainLevelsEditor.Level_bg;

                // 记录当前主关卡 ID（静态字段，底部面板用于展示场景 ID）
                MapBuildEditor.mapId = tableMainLevelsEditor.ID;

                // 重置图标列表标记，刷新图标显示
                m_IsInitIconList = false;
                SetIconItem(tableMainLevelsEditor);

                Debug.Log($"选中地图：{tableMainLevelsEditor.LevelID}");
            }
            catch (Exception e)
            {
                // 选中失败不阻断编辑器，记录错误便于定位
                Debug.LogError($"选中地图失败：{e}");
            }
        }
        #endregion
    }
}
