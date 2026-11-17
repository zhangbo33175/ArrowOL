/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  SenceMapTool.Methods.cs
 * author:    云毅
 * created:   2026
 * descrip:   场景地图编辑器主窗口 - 私有方法部分（模块装配/模式切换/事件转发）
 ***************************************************************/

using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using Honor.Editor.MapTool.ResourceOrganize;

namespace Honor.Editor.MapTool
{
    /// <summary>
    /// 场景地图编辑器窗口（UIToolkit） - 私有方法
    /// 负责切页按钮查找、编辑模块装配、模式切换与子模块事件转发
    /// </summary>
    public partial class SenceMapTool : EditorWindow
    {
        /// <summary>
        /// 初始化各编辑模块（左侧面板/场景/物品元件/迷雾/资源整理）
        /// 三个并列编辑模式统一按 IMapEditModule 契约注册，主窗口不再逐个写切换分支
        /// </summary>
        /// <param name="root">视觉树根节点</param>
        private void InitModules(VisualElement root)
        {
            // 左侧面板模块（章节树 + 资源列表，常驻显示，不参与模式切换）
            var leftPanel = root.Q<VisualElement>(className: MapToolUiNames.LeftPanelClass);
            if (leftPanel != null)
            {
                m_LeftPanelModule = new LeftPanelModule();
                m_LeftPanelModule.Init(leftPanel);

                // 监听关卡选择事件
                m_LeftPanelModule.OnLevelSelected += OnLevelSelected;
                m_LeftPanelModule.OnResourceSelected += OnResourceSelected;
                m_LeftPanelModule.OnTabChanged += OnTabChanged;
            }

            // 场景编辑模块
            var scenePanel = root.QByName<VisualElement>(MapToolUiNames.SceneModePanel);
            if (scenePanel != null)
            {
                m_SceneModule = new SceneEditModule();
                m_SceneModule.Init(scenePanel);

                // 监听预制体变化事件
                m_SceneModule.OnMapPrefabChanged += OnMapPrefabChanged;
                // 切回场景模式时刷新左下角物件列表（配置表可能已被资源整理 Tab 修改）
                RegisterEditTab(root, MapToolUiNames.TabScene, m_SceneModule, () => m_LeftPanelModule?.RefreshResourceList());
            }

            // 物品元件编辑模块
            var itemElementPanel = root.QByName<VisualElement>(MapToolUiNames.ItemElementModePanel);
            if (itemElementPanel != null)
            {
                m_ItemElementModule = new ItemElementEditModule();
                m_ItemElementModule.Init(itemElementPanel);
                // 模板保存后，转发场景模块按最新模板同步已摆放实例
                m_ItemElementModule.OnTemplatesSaved += () => m_SceneModule?.SyncInstancesFromTemplates();
                // 切回元件模式时刷新元件列表（配置表可能已被资源整理 Tab 修改）
                RegisterEditTab(root, MapToolUiNames.TabItemElement, m_ItemElementModule, () => m_ItemElementModule.RefreshItemList());
            }

            // 迷雾编辑模块
            var fogPanel = root.QByName<VisualElement>(MapToolUiNames.FogModePanel);
            if (fogPanel != null)
            {
                m_FogModule = new FogEditModule();
                m_FogModule.Init(fogPanel);
                RegisterEditTab(root, MapToolUiNames.TabFog, m_FogModule);
            }

            // 资源整理模块（自行管理资源面板与 Tab；切走时回调隐藏其他模式）
            m_ResourceOrganizeModule = new ResourceOrganizeModule(root);
            m_ResourceOrganizeModule.FindElements(root);
            m_ResourceOrganizeModule.BindEvents();
            m_ResourceOrganizeModule.OnSetActiveTab = SetActiveTab;
            m_ResourceOrganizeModule.OnOtherModesHidden = HideAllEditModules;
        }

        /// <summary>
        /// 注册一个编辑模式 Tab（按钮 ↔ 模块 ↔ 激活后动作）
        /// </summary>
        /// <param name="root">视觉树根节点</param>
        /// <param name="buttonName">Tab 按钮元素名（见 MapToolUiNames）</param>
        /// <param name="module">编辑模块</param>
        /// <param name="onActivated">激活后的刷新动作（可空）</param>
        private void RegisterEditTab(VisualElement root, string buttonName, IMapEditModule module, System.Action onActivated = null)
        {
            if (root == null || module == null) return;

            Button button = root.Q<Button>(name: buttonName);
            if (button == null) return;

            var binding = new EditTabBinding
            {
                Button = button,
                Module = module,
                OnActivated = onActivated,
            };
            m_EditTabs.Add(binding);
            button.clicked += () => ActivateEditTab(binding);
        }

        /// <summary>
        /// 激活指定编辑模式：显示目标模块、隐藏其余模块、关闭资源面板、刷新 Tab 高亮
        /// </summary>
        /// <param name="target">目标 Tab 绑定项</param>
        private void ActivateEditTab(EditTabBinding target)
        {
            if (target == null) return;

            foreach (var binding in m_EditTabs)
            {
                if (ReferenceEquals(binding, target))
                    binding.Module.Show();
                else
                    binding.Module.Hide();
            }

            m_ResourceOrganizeModule?.ShowResourcePanel(false);
            SetActiveTab(target.Button);
            target.OnActivated?.Invoke();
        }

        /// <summary>
        /// 隐藏全部编辑模式模块（切到资源整理模式时调用）
        /// </summary>
        private void HideAllEditModules()
        {
            foreach (var binding in m_EditTabs)
                binding.Module.Hide();
        }

        /// <summary>
        /// 设置激活的Tab（仅保留 activeBtn 的激活态，含资源整理 Tab）
        /// </summary>
        /// <param name="activeBtn">当前激活的Tab按钮</param>
        private void SetActiveTab(Button activeBtn)
        {
            if (activeBtn == null)
                return;

            foreach (var binding in m_EditTabs)
                binding.Button.SetActiveClass(binding.Button == activeBtn);

            if (m_ResourceOrganizeModule != null)
                m_ResourceOrganizeModule.TabResource.SetActiveClass(m_ResourceOrganizeModule.TabResource == activeBtn);

            activeBtn.SetActiveClass(true);
        }

        /// <summary>
        /// 关卡选择变化：加载对应关卡的地图
        /// </summary>
        /// <param name="chapterId">章节ID</param>
        /// <param name="levelId">关卡ID</param>
        /// <param name="prefabPath">地图预制体路径</param>
        private void OnLevelSelected(string chapterId, string levelId, string prefabPath)
        {
            MapToolLog.Info($"加载关卡地图: 章节={chapterId}, 关卡={levelId}, 预制体={prefabPath}");
            m_SceneModule?.LoadLevelMap(chapterId, levelId, prefabPath);
        }

        /// <summary>
        /// 资源选中事件：转发给场景模块设置为当前放置物体
        /// </summary>
        /// <param name="resourceName">资源名称（格式：TemplateId_TemplateName）</param>
        /// <param name="type">类型：0=建筑 1=人物 2=物件</param>
        private void OnResourceSelected(string resourceName, int type)
        {
            MapToolLog.Info($"选中资源: {resourceName}, 类型: {type}");

            // 拆分资源名称：格式为 "TemplateId_TemplateName"（口径见 MapToolEditorUtils.SplitTemplateKey）
            MapToolEditorUtils.SplitTemplateKey(resourceName, out string objectId, out string objectName);

            m_SceneModule?.SetSelectedObject(objectId, objectName, type);
        }

        /// <summary>
        /// Tab 切换事件：建筑/人物/物件 Tab 切换时联动顶部图层 Toggle
        /// </summary>
        /// <param name="type">类型：0=建筑 1=人物 2=物件</param>
        private void OnTabChanged(int type)
        {
            m_SceneModule?.SetPlaceType(type);
        }

        /// <summary>
        /// 地图预制体变化：保存到左侧面板关卡配置
        /// </summary>
        /// <param name="prefabPath">预制体路径</param>
        private void OnMapPrefabChanged(string prefabPath)
        {
            MapToolLog.Info($"地图预制体变化: {prefabPath}");

            string chapterId = m_LeftPanelModule?.SelectedChapterId;
            string levelId = m_LeftPanelModule?.SelectedLevelId;
            if (!string.IsNullOrEmpty(chapterId) && !string.IsNullOrEmpty(levelId))
            {
                string cacheKey = $"{chapterId}_{levelId}";
                m_LeftPanelModule?.SaveChapterMapPath(cacheKey, prefabPath);
            }
        }
    }
}
