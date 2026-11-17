/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  MapToolUiExtensions.cs
 * author:    云毅
 * created:   2026
 * descrip:   地图编辑器 UI Toolkit 通用扩展（显隐 / 激活态样式类），
 *            消除散落的 style.display 与 AddToClassList(MapToolStyleClasses.Active) 硬编码
 ***************************************************************/

using UnityEngine.UIElements;

namespace Honor.Editor.MapTool
{
    /// <summary>
    /// UI Toolkit 元素通用扩展
    /// </summary>
    public static class MapToolUiExtensions
    {
        /// <summary>激活态样式类名（Tab/子Tab 高亮）</summary>
        public const string ActiveClass = MapToolUiNames.ActiveClass;

        /// <summary>
        /// 设置显隐（null 元素安全跳过）
        /// </summary>
        /// <param name="element">目标元素</param>
        /// <param name="visible">true=显示，false=隐藏</param>
        public static void SetVisible(this VisualElement element, bool visible)
        {
            if (element == null)
                return;
            element.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
        }

        /// <summary>显示元素（null 安全）</summary>
        public static void Show(this VisualElement element)
        {
            element.SetVisible(true);
        }

        /// <summary>隐藏元素（null 安全）</summary>
        public static void Hide(this VisualElement element)
        {
            element.SetVisible(false);
        }

        /// <summary>
        /// 设置控件值（null 安全、不触发变更回调）
        /// 用于属性面板回填：替代「if (field != null) field.SetValueWithoutNotify(value)」的重复分支。
        /// </summary>
        /// <typeparam name="T">控件值类型</param>
        /// <param name="field">目标控件</param>
        /// <param name="value">要回填的值</param>
        public static void SetValueSafe<T>(this BaseField<T> field, T value)
        {
            if (field == null)
                return;
            field.SetValueWithoutNotify(value);
        }

        /// <summary>
        /// 设置控件值（null 安全，会触发变更回调）
        /// 用于项目文件加载 / 默认值重置等需要联动刷新的场景；
        /// 若只想回填界面而不触发回调，请改用 <see cref="SetValueSafe{T}"/>。
        /// </summary>
        /// <typeparam name="T">控件值类型</typeparam>
        /// <param name="field">目标控件</param>
        /// <param name="value">要写入的值</param>
        public static void SetValue<T>(this BaseField<T> field, T value)
        {
            if (field == null)
                return;
            field.value = value;
        }

        /// <summary>
        /// 设置激活态样式类（null 安全）
        /// </summary>
        /// <param name="element">目标元素</param>
        /// <param name="active">是否激活</param>
        public static void SetActiveClass(this VisualElement element, bool active)
        {
            if (element == null)
                return;
            if (active)
                element.AddToClassList(ActiveClass);
            else
                element.RemoveFromClassList(ActiveClass);
        }

        /// <summary>
        /// 按元素名查询子元素（null 安全，空名直接返回 null）
        /// 统一替代散落的「panel?.Q&lt;T&gt;(name: MapToolUiNames.X)」写法：调用方不再需要 ?. 分支。
        /// </summary>
        /// <typeparam name="T">元素类型</typeparam>
        /// <param name="root">查询根元素（可为 null）</param>
        /// <param name="name">元素名（取值见 MapToolUiNames）</param>
        /// <returns>匹配元素；找不到或根为空返回 null</returns>
        public static T QByName<T>(this VisualElement root, string name) where T : VisualElement
        {
            if (root == null || string.IsNullOrEmpty(name))
                return null;
            return root.Q<T>(name: name);
        }
    }
}
