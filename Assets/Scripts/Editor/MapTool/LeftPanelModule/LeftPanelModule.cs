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
        /// 刷新章节列表（从配置表读取章节数据）
        /// </summary>
        public void RefreshChapterList()
        {
            if (m_ChapterList == null) return;
            m_ChapterList.Clear();
            LoadChapterConfig();
            foreach (var chapter in m_ChapterDataList)
            {
                var item = new Label(chapter.ChapterTitle);
                item.AddToClassList("chapter-item");
                if (chapter.ID == SelectedChapterId)
                    item.AddToClassList("selected");
                // 点击章节（闭包捕获 id，避免循环变量共享）
                string id = chapter.ID;
                item.RegisterCallback<ClickEvent>(evt => OnChapterItemClicked(id, item));
                m_ChapterList.Add(item);
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