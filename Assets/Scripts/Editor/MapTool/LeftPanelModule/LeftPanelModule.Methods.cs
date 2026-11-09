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
            // 未选中关卡时默认使用第一章第一关
            string chapterId = string.IsNullOrEmpty(SelectedChapterId) ? "1" : SelectedChapterId;
            // 获取当前关卡已配置的路径
            string cacheKey = $"{chapterId}_{SelectedLevelId}";
            m_ChapterMapPaths.TryGetValue(cacheKey, out string currentPath);
            // 弹出配置窗口
            MapConfigWindow.ShowWindow(chapterId, currentPath, OnMapConfigConfirmed);
        }

        /// <summary>
        /// 地图配置确认回调
        /// </summary>
        private void OnMapConfigConfirmed(string chapterId, string prefabPath)
        {
            Debug.Log($"地图配置确认: 章节={chapterId}, 路径={prefabPath}");
            string cacheKey = $"{chapterId}_{SelectedLevelId}";
            SaveChapterMapPath(cacheKey, prefabPath);
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

            // 加载章节表
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
            // 通知外部：Tab 切换了（用于联动顶部图层 Toggle）
            OnTabChanged?.Invoke(index);
        }

        /// <summary>
        /// 关卡项点击：更新选中态并触发关卡选择事件
        /// </summary>
        private void OnLevelItemClicked(string chapterId, string levelId, Label item)
        {
            SelectedChapterId = chapterId;
            SelectedLevelId = levelId;

            // 移除所有关卡项的选中状态
            foreach (var chapterElem in m_ChapterList.Children())
            {
                var levelContainer = chapterElem.Children().ElementAtOrDefault(1);
                if (levelContainer == null) continue;
                foreach (var levelItem in levelContainer.Children())
                    levelItem.RemoveFromClassList("selected");
            }
            item.AddToClassList("selected");

            Debug.Log($"选中关卡: 章节={chapterId}, 关卡={levelId}");

            // 获取该关卡配置的预制体路径，未配置时回退全局模板路径
            string cacheKey = $"{chapterId}_{levelId}";
            m_ChapterMapPaths.TryGetValue(cacheKey, out string prefabPath);
            if (string.IsNullOrEmpty(prefabPath))
            {
                prefabPath = UnityEditor.EditorPrefs.GetString(MapToolConstants.TemplatePrefabPrefsKey, "");
            }

            // 触发关卡选择事件
            OnLevelSelected?.Invoke(chapterId, levelId, prefabPath);
        }

        /// <summary>
        /// 创建资源列表项（图标 + 名称）
        /// </summary>
        private VisualElement CreateResourceItem(string resourceName)
        {
            var item = new VisualElement();
            item.AddToClassList("resource-item");

            // 资源图标 - 加载实际预览图，失败则用纯色占位
            var icon = new Image();
            icon.AddToClassList("resource-icon");
            icon.scaleMode = ScaleMode.ScaleToFit;
            // 强制固定 32x32 尺寸，防止不同大小的图片撑开行高
            icon.style.width = 32;
            icon.style.height = 32;
            icon.style.overflow = Overflow.Hidden;
            icon.style.flexShrink = 0;

            // 通过 TemplateId 反查模板，用 ImagePath 加载图片
            string imageFileName = null;
            int firstUnderscore = resourceName.IndexOf('_');
            if (firstUnderscore > 0)
            {
                string idStr = resourceName.Substring(0, firstUnderscore);
                int tempId;
                if (int.TryParse(idStr, out tempId))
                {
                    var template = Honor.Editor.MapTool.ObjectTemplateManager.Instance.GetTemplateById(idStr);
                    if (template != null && !string.IsNullOrEmpty(template.ImagePath))
                    {
                        imageFileName = template.ImagePath;
                    }
                }
            }

            // 如果没找到模板，回退到从名称中提取
            if (string.IsNullOrEmpty(imageFileName))
            {
                imageFileName = resourceName;
                if (firstUnderscore > 0)
                {
                    string prefix = resourceName.Substring(0, firstUnderscore);
                    int tempId;
                    if (int.TryParse(prefix, out tempId))
                    {
                        imageFileName = resourceName.Substring(firstUnderscore + 1);
                    }
                }
            }

            // 按文件名查找纹理
            Texture tex = null;
            string[] guids = UnityEditor.AssetDatabase.FindAssets(
                $"{imageFileName} t:Texture2D",
                new[] { MapToolConstants.ResourceScanDir });

            if (guids.Length > 0)
            {
                string texPath = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[0]);
                // 尝试用 Texture2D 加载，失败则尝试用 Sprite 加载
                tex = UnityEditor.AssetDatabase.LoadAssetAtPath<Texture2D>(texPath);
                if (tex == null)
                {
                    var sprite = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>(texPath);
                    if (sprite != null) tex = sprite.texture;
                }
            }

            if (tex != null)
            {
                icon.image = tex;
            }
            else
            {
                icon.style.backgroundColor = new Color(0.4f, 0.3f, 0.6f);
            }

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
            // 根据 TemplateId 反查资源类型（人物 Tab 含 NPC + 猫猫，不能用 Tab 索引）
            string templateId = resourceName.Split('_')[0];
            var template = ObjectTemplateManager.Instance.GetTemplateById(templateId);
            int objectType = template != null ? (int)template.Type : m_CurrentTabIndex;
            OnResourceSelected?.Invoke(resourceName, objectType);
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
                case 1: // 人物（含 NPC + 猫猫）
                    foreach (var t in templates.FindAll(t => t.Type == MapObjectType.Character || t.Type == MapObjectType.Cat))
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