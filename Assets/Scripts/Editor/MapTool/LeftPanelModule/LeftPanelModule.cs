/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  LeftPanelModule.cs
 * author:    云毅
 * created:   2026
 * descrip:   左侧面板模块 - 公有接口部分（章节/资源列表刷新与保存）
 ***************************************************************/

using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System.Linq;
using Editor.MapEditor;

namespace Honor.Editor.MapTool
{
    /// <summary>
    /// 左侧面板模块 - 公有接口（章节/资源列表刷新与保存）
    /// </summary>
    public partial class LeftPanelModule
    {
        /// <summary>
        /// 初始化模块
        /// </summary>
        /// <param name="root">根节点</param>
        public void Init(VisualElement root)
        {
            m_Root = root;
            FindElements();
            BindEvents();
            RefreshChapterList();
            RefreshResourceList();
        }

        /// <summary>
        /// 保存章节地图预制体路径（内存缓存）
        /// </summary>
        public void SaveChapterMapPath(string chapterId, string prefabPath)
        {
            m_ChapterMapPaths[chapterId] = prefabPath;
            Debug.Log($"保存配置到字典: 章节={chapterId}, 路径={prefabPath}");
        }

        /// <summary>
        /// 刷新章节-关卡树列表（从配置表读取章节数据，展开显示下属关卡）
        /// </summary>
        public void RefreshChapterList()
        {
            if (m_ChapterList == null) return;
            m_ChapterList.Clear();
            LoadChapterConfig();
            foreach (var chapter in m_ChapterDataList)
            {
                // 章节根节点（可展开/折叠）
                var chapterItem = new VisualElement();
                chapterItem.AddToClassList("chapter-item");

                // 章节标题行（带展开箭头）
                var chapterHeader = new VisualElement();
                chapterHeader.AddToClassList("chapter-header");

                var arrowLabel = new Label("▼");
                arrowLabel.AddToClassList("expand-arrow");

                var titleLabel = new Label(chapter.ChapterTitle);
                titleLabel.AddToClassList("chapter-title");

                chapterHeader.Add(arrowLabel);
                chapterHeader.Add(titleLabel);
                chapterItem.Add(chapterHeader);

                // 关卡子节点容器
                var levelContainer = new VisualElement();
                levelContainer.AddToClassList("level-container");
                chapterItem.Add(levelContainer);

                // 解析关卡ID列表（从章节配置 ChapterInfoID 解析）
                string[] levelIds = MapToolEditorUtils.ParseRegionIds(chapter.ChapterInfoID);
                for (int levelIndex = 0; levelIndex < levelIds.Length; levelIndex++)
                {
                    string levelId = levelIds[levelIndex];
                    // 显示序号从 1 开始，加载地图用原始 levelId
                    var levelItem = new Label($"关卡 {levelIndex + 1}");
                    levelItem.AddToClassList("level-item");

                    // 选中态高亮
                    if (chapter.ID == SelectedChapterId && levelId == SelectedLevelId)
                        levelItem.AddToClassList("selected");

                    // 点击关卡（闭包捕获 id，避免循环变量共享）
                    string cid = chapter.ID;
                    string lid = levelId;
                    levelItem.RegisterCallback<ClickEvent>(evt => OnLevelItemClicked(cid, lid, levelItem));
                    levelContainer.Add(levelItem);
                }

                // 点击章节标题行：展开/折叠关卡列表
                chapterHeader.RegisterCallback<ClickEvent>(evt =>
                {
                    bool collapsed = levelContainer.style.display == DisplayStyle.None;
                    levelContainer.style.display = collapsed ? DisplayStyle.Flex : DisplayStyle.None;
                    arrowLabel.text = collapsed ? "▼" : "▶";
                });

                m_ChapterList.Add(chapterItem);
            }
        }

        /// <summary>
        /// 刷新资源列表（按当前Tab分类）
        /// </summary>
        public void RefreshResourceList()
        {
            if (m_ResourceList == null) return;
            m_ResourceList.Clear();
            m_ResourceItems.Clear();
            List<string> resources = GetResourceDataByTab(m_CurrentTabIndex);
            foreach (var resourceName in resources)
            {
                var item = CreateResourceItem(resourceName);
                m_ResourceList.Add(item);
                m_ResourceItems.Add(item);
            }
        }

    }
}