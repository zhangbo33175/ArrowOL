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
            m_TabScene = visualTree.Q<Button>(name: "tab-scene");
            m_TabItemElement = visualTree.Q<Button>(name: "tab-item-element");
            m_TabFog = visualTree.Q<Button>(name: "tab-fog");
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

                // 监听章节选择事件
                m_LeftPanelModule.OnChapterSelected += OnChapterSelected;
                m_LeftPanelModule.OnResourceSelected += OnResourceSelected;
            }

            // 场景编辑模块
            var scenePanel = root.Q<VisualElement>(name: "scene-mode-panel");
            if (scenePanel != null)
            {
                m_SceneModule = new SceneEditModule();
                m_SceneModule.Init(scenePanel);

                // 监听预制体变化事件
                m_SceneModule.OnMapPrefabChanged += OnMapPrefabChanged;
            }

            // 物品元件编辑模块
            var itemElementPanel = root.Q<VisualElement>(name: "item-element-mode-panel");
            if (itemElementPanel != null)
            {
                m_ItemElementModule = new ItemElementEditModule();
                m_ItemElementModule.Init(itemElementPanel);
            }

            // 迷雾编辑模块
            var fogPanel = root.Q<VisualElement>(name: "fog-mode-panel");
            if (fogPanel != null)
            {
                m_FogModule = new FogEditModule();
                m_FogModule.Init(fogPanel);
            }
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

            activeBtn.AddToClassList("active");
        }

        /// <summary>
        /// 章节选择变化：加载对应章节的地图
        /// </summary>
        /// <param name="chapterId">章节ID</param>
        /// <param name="prefabPath">地图预制体路径</param>
        /// <param name="chapterInfoID">章节信息配置ID（分区来源）</param>
        private void OnChapterSelected(string chapterId, string prefabPath, string chapterInfoID)
        {
            Debug.Log($"加载章节地图: {chapterId}, 预制体: {prefabPath}, 分区数: {chapterInfoID}");
            m_SceneModule?.LoadChapterMap(chapterId, prefabPath, chapterInfoID);
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
        /// 地图预制体变化：保存到左侧面板章节配置
        /// </summary>
        /// <param name="prefabPath">预制体路径</param>
        private void OnMapPrefabChanged(string prefabPath)
        {
            Debug.Log($"地图预制体变化: {prefabPath}");

            string chapterId = m_LeftPanelModule?.SelectedChapterId;
            if (!string.IsNullOrEmpty(chapterId))
            {
                m_LeftPanelModule?.SaveChapterMapPath(chapterId, prefabPath);
            }
        }
    }
}
