/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Editor
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  BaseToolkitEditorWindow.cs
 * author:    云毅
 * created:   2026
 * descrip:   UI Toolkit 编辑器窗口基类：UXML/USS 外壳 + IMGUIContainer 动态列表
 *            子类只需提供 UXML/USS 路径并在 OnBindUI 中绑定静态控件
 ***************************************************************/

using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Honor.Editor
{
    /// <summary>
    /// 基于 UI Toolkit 的编辑器窗口基类。
    /// 外壳由 UXML + USS 描述，复杂动态列表通过 <see cref="IMGUIContainer"/> 托管，
    /// 避免在 UI Toolkit 里重写整套数据绑定。
    /// </summary>
    /// <typeparam name="T">具体窗口类型</typeparam>
    public abstract class BaseToolkitEditorWindow<T> : BaseEditorWindow<T> where T : EditorWindow
    {
        #region 子类提供
        /// <summary>UXML/USS 资源所在目录（相对工程根，以 / 结尾）</summary>
        protected abstract string UIAssetDir { get; }

        /// <summary>UXML 文件名（不含扩展名）</summary>
        protected abstract string UxmlName { get; }

        /// <summary>USS 文件名（不含扩展名），默认与 UXML 同名</summary>
        protected virtual string UssName => UxmlName;
        #endregion

        #region 子类钩子
        /// <summary>数据初始化（在构建 UI 前调用）</summary>
        protected virtual void OnInitData() { }

        /// <summary>UXML 克隆完成后，绑定静态控件与 IMGUI 容器</summary>
        protected abstract void OnBindUI();
        #endregion

        #region 生命周期
        /// <summary>
        /// 启用：初始化数据 → 构建 UI → 启动持续重绘
        /// 注意：Unity 的 OnEnable 非 C# virtual，不可用 override，直接按方法名声明即可
        /// </summary>
        protected void OnEnable()
        {
            OnInitData();
            BuildUI();
            EditorApplication.update += OnEditorTick;
        }

        /// <summary>
        /// 禁用：注销持续重绘
        /// </summary>
        protected void OnDisable()
        {
            EditorApplication.update -= OnEditorTick;
        }

        /// <summary>脏标记：数据变化时设为 true，下一帧重绘后自动清除</summary>
        private bool m_RepaintPending;

        /// <summary>请求下一帧重绘 IMGUI 容器（数据变化后调用）</summary>
        protected void RequestRepaint()
        {
            m_RepaintPending = true;
        }

        /// <summary>
        /// 编辑器每帧回调：仅在脏标记时重绘，避免每帧强制 Layout 干扰 IMGUI
        /// </summary>
        private void OnEditorTick()
        {
            if (!m_RepaintPending) return;
            m_RepaintPending = false;
            rootVisualElement?.MarkDirtyRepaint();
        }
        #endregion

        #region UI 构建
        /// <summary>
        /// 加载 UXML/USS，克隆树并触发子类绑定
        /// </summary>
        private void BuildUI()
        {
            rootVisualElement.Clear();

            string uxmlPath = $"{UIAssetDir}{UxmlName}.uxml";
            string ussPath = $"{UIAssetDir}{UssName}.uss";

            var uxml = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(uxmlPath);
            var uss = AssetDatabase.LoadAssetAtPath<StyleSheet>(ussPath);
            if (uxml == null || uss == null)
            {
                rootVisualElement.Add(new Label($"[错误] 未找到 UI 资源：{uxmlPath} / {ussPath}"));
                return;
            }

            uxml.CloneTree(rootVisualElement);
            rootVisualElement.styleSheets.Add(uss);

            OnBindUI();
        }
        #endregion

        #region 子类便捷方法
        /// <summary>
        /// 按名称查找 UI 元素
        /// </summary>
        protected TElement Q<TElement>(string name) where TElement : VisualElement
        {
            return rootVisualElement.Q<TElement>(name);
        }

        /// <summary>
        /// 将 IMGUI 绘制委托挂到指定 IMGUIContainer
        /// </summary>
        /// <param name="containerName">IMGUIContainer 的 name</param>
        /// <param name="onGUI">IMGUI 绘制回调</param>
        protected void BindImgui(string containerName, Action onGUI)
        {
            var container = rootVisualElement.Q<IMGUIContainer>(containerName);
            if (container != null)
            {
                container.onGUIHandler = onGUI;
            }
        }

        /// <summary>
        /// 绑定按钮点击事件
        /// </summary>
        /// <param name="buttonName">Button 的 name</param>
        /// <param name="action">点击回调</param>
        protected void BindClick(string buttonName, Action action)
        {
            var button = rootVisualElement.Q<Button>(buttonName);
            if (button != null)
            {
                button.clicked += action;
            }
        }
        #endregion
    }
}
