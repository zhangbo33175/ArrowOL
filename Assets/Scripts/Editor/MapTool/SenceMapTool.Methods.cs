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
        /// 查找切页按钮引用
        /// </summary>
        /// <param name="visualTree">实例化后的视觉树</param>
        private void FindElements(VisualElement visualTree)
        {
            m_TabScene = visualTree.Q<Button>(name: MapToolUiNames.TabScene);
            m_TabItemElement = visualTree.Q<Button>(name: MapToolUiNames.TabItemElement);
            m_TabFog = visualTree.Q<Button>(name: MapToolUiNames.TabFog);
            m_TabResource = visualTree.Q<Button>(name: MapToolUiNames.TabResource);
            m_ResourcePanel = visualTree.Q<VisualElement>(name: MapToolUiNames.ResourcePanel);
        }

        /// <summary>
        /// 初始化各编辑模块（左侧面板/场景/物品元件/迷雾）
        /// 并订阅子模块事件（章节选择/资源选中/预制体变化）
        /// </summary>
        /// <param name="root">视觉树根节点</param>
        private void InitModules(VisualElement root)
        {
            // 左侧面板模块
            var leftPanel = root.Q<VisualElement>(className: "left-panel");
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
            var scenePanel = root.Q<VisualElement>(name: MapToolUiNames.SceneModePanel);
            if (scenePanel != null)
            {
                m_SceneModule = new SceneEditModule();
                m_SceneModule.Init(scenePanel);

                // 监听预制体变化事件
                m_SceneModule.OnMapPrefabChanged += OnMapPrefabChanged;
            }

            // 物品元件编辑模块
            var itemElementPanel = root.Q<VisualElement>(name: MapToolUiNames.ItemElementModePanel);
            if (itemElementPanel != null)
            {
                m_ItemElementModule = new ItemElementEditModule();
                m_ItemElementModule.Init(itemElementPanel);
            }

            // 迷雾编辑模块
            var fogPanel = root.Q<VisualElement>(name: MapToolUiNames.FogModePanel);
            if (fogPanel != null)
            {
                m_FogModule = new FogEditModule();
                m_FogModule.Init(fogPanel);
            }

            // 资源整理模块
            m_ResourceOrganizeModule = new ResourceOrganizeModule(root);
            m_ResourceOrganizeModule.FindElements(root);
            m_ResourceOrganizeModule.BindEvents();
            m_ResourceOrganizeModule.OnSetActiveTab = SetActiveTab;
            m_ResourceOrganizeModule.OnOtherModesHidden = () =>
            {
                m_SceneModule?.Hide();
                m_ItemElementModule?.Hide();
                m_FogModule?.Hide();
            };
        }

        /// <summary>
        /// 绑定切页按钮点击事件
        /// </summary>
        private void BindTabEvents()
        {
            if (m_TabScene != null)
                m_TabScene.clicked += ShowSceneMode;
            if (m_TabItemElement != null)
                m_TabItemElement.clicked += ShowItemElementMode;
            if (m_TabFog != null)
                m_TabFog.clicked += ShowFogMode;
            // 资源整理 Tab 按钮已由 ResourceOrganizeModule 绑定
        }

        /// <summary>
        /// 切换到场景地图编辑模式
        /// </summary>
        private void ShowSceneMode()
        {
            SetActiveTab(m_TabScene);
            m_SceneModule?.Show();
            m_ItemElementModule?.Hide();
            m_FogModule?.Hide();
            m_ResourceOrganizeModule?.ShowResourcePanel(false);
            // 刷新左下角物件列表（配置表可能已被资源整理 Tab 修改）
            m_LeftPanelModule?.RefreshResourceList();
        }

        /// <summary>
        /// 切换到物品元件编辑模式
        /// </summary>
        private void ShowItemElementMode()
        {
            SetActiveTab(m_TabItemElement);
            m_SceneModule?.Hide();
            m_ItemElementModule?.Show();
            m_FogModule?.Hide();
            m_ResourceOrganizeModule?.ShowResourcePanel(false);
            // 刷新元件列表（配置表可能已被资源整理 Tab 修改）
            m_ItemElementModule?.RefreshItemList();
        }

        /// <summary>
        /// 切换到迷雾编辑模式
        /// </summary>
        private void ShowFogMode()
        {
            SetActiveTab(m_TabFog);
            m_SceneModule?.Hide();
            m_ItemElementModule?.Hide();
            m_FogModule?.Show();
            m_ResourceOrganizeModule?.ShowResourcePanel(false);
        }

        /// <summary>
        /// 设置激活的Tab（移除其余按钮激活态）
        /// </summary>
        /// <param name="activeBtn">当前激活的Tab按钮</param>
        private void SetActiveTab(Button activeBtn)
        {
            if (activeBtn == null)
                return;

            m_TabScene?.RemoveFromClassList("active");
            m_TabItemElement?.RemoveFromClassList("active");
            m_TabFog?.RemoveFromClassList("active");
            m_TabResource?.RemoveFromClassList("active");

            activeBtn.AddToClassList("active");
        }

        /// <summary>
        /// 关卡选择变化：加载对应关卡的地图
        /// </summary>
        /// <param name="chapterId">章节ID</param>
        /// <param name="levelId">关卡ID</param>
        /// <param name="prefabPath">地图预制体路径</param>
        private void OnLevelSelected(string chapterId, string levelId, string prefabPath)
        {
            Debug.Log($"加载关卡地图: 章节={chapterId}, 关卡={levelId}, 预制体={prefabPath}");
            m_SceneModule?.LoadLevelMap(chapterId, levelId, prefabPath);
        }

        /// <summary>
        /// 资源选中事件：转发给场景模块设置为当前放置物体
        /// </summary>
        /// <param name="resourceName">资源名称（格式：TemplateId_TemplateName）</param>
        /// <param name="type">类型：0=建筑 1=人物 2=物件</param>
        private void OnResourceSelected(string resourceName, int type)
        {
            Debug.Log($"选中资源: {resourceName}, 类型: {type}");

            // 拆分资源名称：格式为 "TemplateId_TemplateName"
            string objectId = resourceName;
            string objectName = resourceName;

            int underscoreIndex = resourceName.IndexOf('_');
            if (underscoreIndex > 0)
            {
                objectId = resourceName.Substring(0, underscoreIndex);
                objectName = resourceName.Substring(underscoreIndex + 1);
            }

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
        /// 从 PNG 序列导入结果应用到场景编辑模块（菜单入口调用）
        /// </summary>
        public void ApplyImageImportToScene(MapImageImportResult result)
        {
            m_SceneModule?.ApplyImageImport(result);
        }

        /// <summary>
        /// 地图预制体变化：保存到左侧面板关卡配置
        /// </summary>
        /// <param name="prefabPath">预制体路径</param>
        private void OnMapPrefabChanged(string prefabPath)
        {
            Debug.Log($"地图预制体变化: {prefabPath}");

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
