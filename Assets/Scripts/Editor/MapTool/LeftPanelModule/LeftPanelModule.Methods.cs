/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  LeftPanelModule.Methods.cs
 * author:    云毅
 * created:   2026
 * descrip:   左侧面板模块 - 私有方法部分（列表构建/选项卡切换/配置读取）
 ***************************************************************/

using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System.Linq;
using Editor.MapEditor;

namespace Honor.Editor.MapTool
{
    /// <summary>
    /// 左侧面板模块 - 私有方法（列表构建/选项卡切换/配置读取）
    /// </summary>
    public partial class LeftPanelModule
    {
        /// <summary>
        /// 绑定事件
        /// </summary>
        private void BindEvents()
        {
            if (m_ConfigBtn != null)
                m_ConfigBtn.clicked += OnConfigClicked;
            if (m_DeleteBtn != null)
                m_DeleteBtn.clicked += OnDeleteClicked;
            if (m_BuildingTabBtn != null)
                m_BuildingTabBtn.clicked += () => SetActiveTab(0);
            if (m_GroundTabBtn != null)
                m_GroundTabBtn.clicked += () => SetActiveTab(1);
            if (m_PropTabBtn != null)
                m_PropTabBtn.clicked += () => SetActiveTab(2);
            if (m_RefreshBtn != null)
                m_RefreshBtn.clicked += OnRefreshClicked;
        }

        /// <summary>
        /// 配置按钮点击：弹出地图预制体配置窗口
        /// </summary>
        private void OnConfigClicked()
        {
            Debug.Log("配置地图预制体");
            // 未选中章节时默认使用第一章
            string chapterId = string.IsNullOrEmpty(SelectedChapterId) ? "default" : SelectedChapterId;
            // 获取当前章节已配置的路径
            m_ChapterMapPaths.TryGetValue(chapterId, out string currentPath);
            // 弹出配置窗口
            MapConfigWindow.ShowWindow(chapterId, currentPath, OnMapConfigConfirmed);
        }

        /// <summary>
        /// 地图配置确认回调
        /// </summary>
        private void OnMapConfigConfirmed(string chapterId, string prefabPath)
        {
            Debug.Log($"地图配置确认: 章节={chapterId}, 路径={prefabPath}");
            SaveChapterMapPath(chapterId, prefabPath);
        }

        /// <summary>
        /// 删除按钮点击（预留：删除章节）
        /// </summary>
        private void OnDeleteClicked()
        {
            Debug.Log("删除章节");
        }

        /// <summary>
        /// 刷新按钮点击：刷新资源列表
        /// </summary>
        private void OnRefreshClicked()
        {
            Debug.Log("刷新资源列表");
            RefreshResourceList();
        }

        /// <summary>
        /// 加载章节配置表（仅首次加载，之后使用缓存）
        /// </summary>
        private void LoadChapterConfig()
        {
            if (m_ChapterDataList.Count > 0) return;
            m_ChapterDataList = MapToolEditorUtils.LoadTableData<TableChapterEditor>(MapToolConstants.TableChaptersPath);
            Debug.Log($"加载章节配置表成功，共 {m_ChapterDataList.Count} 个章节");
        }

        /// <summary>
        /// 设置激活的资源分类Tab
        /// </summary>
        /// <param name="index">Tab索引：0=建筑 1=人物 2=物件</param>
        private void SetActiveTab(int index)
        {
            m_CurrentTabIndex = index;
            // 切换按钮激活态
            Button[] tabButtons = { m_BuildingTabBtn, m_GroundTabBtn, m_PropTabBtn };
            for (int i = 0; i < tabButtons.Length; i++)
            {
                if (tabButtons[i] == null) continue;
                if (i == index)
                    tabButtons[i].AddToClassList("active");
                else
                    tabButtons[i].RemoveFromClassList("active");
            }
            RefreshResourceList();
        }

        /// <summary>
        /// 章节项点击：更新选中态并触发章节选择事件
        /// </summary>
        private void OnChapterItemClicked(string chapterId, Label item)
        {
            SelectedChapterId = chapterId;
            // 移除所有章节的选中状态
            foreach (var child in m_ChapterList.Children())
                child.RemoveFromClassList("selected");
            item.AddToClassList("selected");
            Debug.Log($"选中章节: {chapterId}");
            // 获取该章节的分区配置（空则后续按默认 9 分区处理）
            var chapterData = m_ChapterDataList.FirstOrDefault(c => c.ID == chapterId);
            string chapterInfoID = chapterData?.ChapterInfoID ?? string.Empty;
            // 获取该章节配置的预制体路径，未配置时回退全局模板路径
            m_ChapterMapPaths.TryGetValue(chapterId, out string prefabPath);
            if (string.IsNullOrEmpty(prefabPath))
            {
                prefabPath = UnityEditor.EditorPrefs.GetString(MapToolConstants.TemplatePrefabPrefsKey, "");
            }
            // 触发章节选择事件
            OnChapterSelected?.Invoke(chapterId, prefabPath, chapterInfoID);
        }

        /// <summary>
        /// 创建资源列表项（图标 + 名称）
        /// </summary>
        private VisualElement CreateResourceItem(string resourceName)
        {
            var item = new VisualElement();
            item.AddToClassList("resource-item");
            // 资源图标
            var icon = new VisualElement();
            icon.AddToClassList("resource-icon");
            item.Add(icon);
            // 资源名称
            var nameLabel = new Label(resourceName);
            nameLabel.AddToClassList("resource-name");
            item.Add(nameLabel);
            // 点击资源（闭包捕获 name，避免循环变量共享）
            string name = resourceName;
            item.RegisterCallback<ClickEvent>(evt => OnResourceItemClicked(name, item));
            return item;
        }

        /// <summary>
        /// 资源项点击：更新选中态并触发资源选中事件
        /// </summary>
        private void OnResourceItemClicked(string resourceName, VisualElement item)
        {
            SelectedResourceName = resourceName;
            foreach (var resourceItem in m_ResourceItems)
                resourceItem.RemoveFromClassList("selected");
            item.AddToClassList("selected");
            Debug.Log($"选中资源: {resourceName}");
            // 触发资源选中事件（类型 = 当前Tab索引）
            OnResourceSelected?.Invoke(resourceName, m_CurrentTabIndex);
        }

        /// <summary>
        /// 查找UI元素
        /// </summary>
        private void FindElements()
        {
            // 顶部按钮
            m_ConfigBtn = m_Root.Q<Button>(name: "btn-config");
            m_DeleteBtn = m_Root.Q<Button>(name: "btn-delete");
            // 章节列表
            m_ChapterList = m_Root.Q<ScrollView>(name: "chapter-list");
            // 资源分类Tab按钮
            m_BuildingTabBtn = m_Root.Q<Button>(name: "tab-building");
            m_GroundTabBtn = m_Root.Q<Button>(name: "tab-ground");
            m_PropTabBtn = m_Root.Q<Button>(name: "tab-prop");
            m_RefreshBtn = m_Root.Q<Button>(name: "btn-refresh");
            // 资源列表
            m_ResourceList = m_Root.Q<ScrollView>(name: "resource-list");
        }

        /// <summary>
        /// 根据Tab索引获取资源数据
        /// </summary>
        private List<string> GetResourceDataByTab(int tabIndex)
        {
            var templates = ObjectTemplateManager.Instance.GetAllTemplates();
            var result = new List<string>();

            switch (tabIndex)
            {
                case 0: // 建筑
                    foreach (var t in templates.FindAll(t => t.Type == MapObjectType.Building))
                        result.Add($"{t.TemplateId}_{t.TemplateName}");
                    break;
                case 1: // 人物
                    foreach (var t in templates.FindAll(t => t.Type == MapObjectType.Character))
                        result.Add($"{t.TemplateId}_{t.TemplateName}");
                    break;
                case 2: // 物件
                    foreach (var t in templates.FindAll(t => t.Type == MapObjectType.Prop))
                        result.Add($"{t.TemplateId}_{t.TemplateName}");
                    break;
            }

            return result;
        }

    }
}