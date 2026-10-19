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

        /// <summary>
        /// 挑战关卡分组（Tables.DailyLevels）是否选中展开
        /// </summary>
        private bool _isChallengeGroupSelected;

        /// <summary>
        /// 当前选中的挑战关卡小节索引（-1 = 未选中）
        /// </summary>
        private int _selectedChallengeIndex = -1;

        /// <summary>
        /// 挑战关卡固定章节标识（保存目录 / Lua 表名前缀使用，与运行时 ChallengeLevelHelper 约定一致）
        /// </summary>
        private const string kChallengeChapterId = "Daily";
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

                // 追加挑战关卡分组（Tables.DailyLevels 每日挑战关卡地图编辑入口）
                DrawChallengeLevelListItem();
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

        #region 挑战关卡分组（Tables.DailyLevels 地图编辑）
        /// <summary>
        /// 绘制挑战关卡分组列表项
        /// 挑战关卡（每日挑战）独立于主线章节，数据源为 m_TableDailyLevelsList（TableDailyLevels）；
        /// 点击分组标题展开挑战关卡小节列表，选中小节后进入挑战关卡地图编辑
        /// </summary>
        private void DrawChallengeLevelListItem()
        {
            GUILayout.BeginVertical(_isChallengeGroupSelected ? "Box" : GUIStyle.none);
            {
                // 挑战关卡分组标题行（点击切换展开/收起）
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("挑战关卡",
                        EditorStyles.label,
                        GUILayout.ExpandWidth(true)))
                {
                    _isChallengeGroupSelected = !_isChallengeGroupSelected;
                    _selectedChallengeIndex = -1;
                }
                GUILayout.EndHorizontal();

                // 选中分组时展开挑战关卡小节列表（缩进显示，与主线小节同风格）
                if (_isChallengeGroupSelected)
                {
                    GUILayout.Space(4);
                    GUILayout.BeginHorizontal();
                    GUILayout.Space(20);
                    GUILayout.BeginVertical();
                    {
                        if (m_TableDailyLevelsList == null || m_TableDailyLevelsList.Count == 0)
                        {
                            GUILayout.Label("暂无挑战关卡数据", EditorStyles.miniLabel);
                        }
                        else
                        {
                            for (int i = 0; i < m_TableDailyLevelsList.Count; i++)
                            {
                                int subId = i + 1;
                                bool isSubSelected = _selectedChallengeIndex == subId;
                                GUIStyle style = isSubSelected ? EditorStyles.boldLabel : EditorStyles.miniLabel;

                                if (GUILayout.Button($"第 {subId} 节", style, GUILayout.ExpandWidth(true)))
                                {
                                    _selectedChallengeIndex = subId;
                                    ChooseChallengeItem(m_TableDailyLevelsList[i], subId);
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

        /// <summary>
        /// 【挑战关卡选中回调】加载挑战关卡地图背景并设置编辑上下文
        /// 保存复用主线 JSON+Lua 链路：ChapterId 固定为 "Daily"，Lua 输出 TableLevelData_Daily_{小节序号}
        /// </summary>
        /// <param name="challengeLevel">挑战关卡配置数据（LevelDataEditor，源自 Tables.DailyLevels）</param>
        /// <param name="subIndex">挑战关卡小节序号（1 起始，对应挑战表主键）</param>
        private void ChooseChallengeItem(LevelDataEditor challengeLevel, int subIndex)
        {
            try
            {
                if (challengeLevel == null)
                    return;

                // 挑战关卡运行时 ID = 10000 + 表序号（与 ChallengeLevelHelper._challengeIdOffset 约定一致）
                int challengeLevelId = 10000 + subIndex;

                // 挑战关卡地图背景（挑战表 Level_bg 列；为空时可经底部"更换地图背景"手动设置）
                string levelBg = challengeLevel.Level_bg;

                // 【诊断日志】确认挑战表字段映射是否生效：
                // Level_bg 应有 "Level_bg_main2" 等值、LevelItems 应有 "{\"1001\",...}" 数组串，
                // 若两者为空 → 说明 LevelDataEditor 新字段未重编译（需 Unity 重新编译编辑器脚本）
                Debug.Log($"[挑战关卡] 选中第 {subIndex} 节 | Level_bg=[{levelBg}] | " +
                          $"LevelItems=[{challengeLevel.LevelItems}] | " +
                          $"背景资源=[{PathUtils.GetItemStrPath(levelBg)}]");

                // 加载挑战关卡地图背景
                GameObject prefabPath = Utils.ReplaceImageInPrefab(Utils.GetMap(), "RawImage",
                    levelBg, false, Type.LoadBackground);
                if (prefabPath != null)
                {
                    LoadMapPrefab(prefabPath, () => { SetPreviewRect(); });
                }

                // 设置挑战关卡编辑上下文（ChapterId 固定为挑战标识，保存到独立目录与 Lua）
                _mTablesElectedLevelsEditor = new TablesElectedLevelsEditor
                {
                    ChapterId = kChallengeChapterId,          // "Daily"
                    LevelId = challengeLevelId.ToString(),    // 挑战关卡运行时 ID（10001 起）
                    MapId = subIndex,                          // 挑战表序号（Lua 后缀）
                    ChapterTitle = challengeLevel.ChapterName,
                    MapName = levelBg,
                    Background = levelBg,
                    SavePath = string.Empty,
                };

                mapName = levelBg;
                MapBuildEditor.mapId = challengeLevelId.ToString();

                // 刷新挑战关卡图标列表（基于挑战关卡 LevelItems 寻物目标集合）
                // 仅当"选中项变化"或"列表为空"时重新加载；同一节重复选中不重复加载，避免反复解析/匹配
                string challengeSourceKey = $"challenge:{subIndex}";
                if (_iconListSourceKey != challengeSourceKey || m_IconList.Count == 0)
                {
                    _iconListSourceKey = challengeSourceKey;
                    m_IconListView = Vector2.zero;   // 切换节时重置滚动到顶部，避免残留上次滚动位置（1080）导致首项不可见
                    SetChallengeIconItem(challengeLevel);
                }

                // 强制立即刷新窗口，确保下方物件列表（m_IconList）立刻可见，避免显示上一帧旧内容
                Repaint();

                Debug.Log($"选中挑战关卡：{challengeLevel.LevelId}（第 {subIndex} 节）");
            }
            catch (Exception e)
            {
                // 选中失败不阻断编辑器，记录错误便于定位
                Debug.LogError($"选中挑战关卡失败：{e}");
            }
        }

        /// <summary>
        /// 加载挑战关卡可使用的图标列表（挑战关卡 LevelItems 寻物目标 → 全局物品表匹配，逻辑与主线 SetIconItem 一致）
        /// </summary>
        /// <param name="challengeLevel">挑战关卡配置数据</param>
        private void SetChallengeIconItem(LevelDataEditor challengeLevel)
        {
            if (challengeLevel == null)
            {
                Debug.LogWarning("挑战关卡配置数据为空，无法加载图标列表！");
                return;
            }

            // 清空旧图标列表，避免历史数据残留
            m_IconList.Clear();

            // 从挑战关卡配置中解析：该地图允许使用的寻物目标物品 ID 数组（挑战表 LevelItems 列）
            int[] itemIds = Utils.GetIntList(challengeLevel.LevelItems);

            // 【诊断日志】确认解析是否成功（LevelItems 应解析出 30 个 ID）
            Debug.Log($"[挑战关卡图标] LevelItems=[{challengeLevel.LevelItems}] → 解析出 {itemIds?.Length ?? 0} 个ID | 物品表 {m_TableItemsList?.Count ?? 0} 条");

            // 安全校验：物品 ID 数组不能为空
            if (itemIds == null || itemIds.Length == 0)
            {
                Debug.LogWarning("当前挑战关卡未配置任何可使用物品 ID（挑战表 LevelItems 为空）！");
                return;
            }

            // 遍历挑战关卡物品 ID，匹配全局物品表
            int matchedCount = 0;
            foreach (int currentId in itemIds)
            {
                var targetItem = m_TableItemsList.Find(item => item.ID == currentId);
                if (targetItem != null)
                {
                    m_IconList.Add(new ItemIconDataEditor
                    {
                        m_ID = targetItem.ID,
                        m_IconName = targetItem.Icon,
                        m_IconAssetPath = targetItem.Icon,
                        m_IsChoose = false,
                        m_Type = RMapIconType.Dissipate
                    });
                    matchedCount++;
                }
            }

            // 【诊断日志】确认匹配结果（挑战关卡 LevelItems 与物品表 TableCharacters 匹配数量）
            Debug.Log($"[挑战关卡图标] 匹配 {matchedCount} 个图标 → m_IconList.Count={m_IconList.Count}");
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

                // 刷新主线关卡图标列表
                // 仅当"选中项变化"或"列表为空"时重新加载；同一关卡重复选中不重复加载
                string mainSourceKey = $"main:{tableMainLevelsEditor.ID}";
                if (_iconListSourceKey != mainSourceKey || m_IconList.Count == 0)
                {
                    _iconListSourceKey = mainSourceKey;
                    m_IconListView = Vector2.zero;   // 切换关卡时重置滚动到顶部
                    SetIconItem(tableMainLevelsEditor);
                }

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
