/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  MapToolUiFactory.cs
 * author:    云毅
 * created:   2026
 * descrip:   地图编辑器 UI Toolkit 控件工厂
 *            统一动态创建控件的样式口径（占满剩余宽度等），各模块不再各自 new + 设样式
 ***************************************************************/

using System.Collections.Generic;
using UnityEngine.UIElements;

namespace Honor.Editor.MapTool
{
    /// <summary>
    /// 地图编辑器 UI Toolkit 控件工厂
    /// </summary>
    public static class MapToolUiFactory
    {
        /// <summary>
        /// 创建占满剩余宽度的下拉控件
        /// </summary>
        /// <param name="choices">选项列表</param>
        /// <param name="defaultValue">默认选项（null 表示不预设）</param>
        /// <param name="onChanged">变更回调（可空）</param>
        /// <returns>下拉控件</returns>
        public static DropdownField CreateDropdown(List<string> choices, string defaultValue,
            EventCallback<ChangeEvent<string>> onChanged = null)
        {
            var field = new DropdownField();
            if (choices != null) field.choices = choices;
            if (!string.IsNullOrEmpty(defaultValue)) field.value = defaultValue;
            field.style.flexGrow = 1;
            if (onChanged != null) field.RegisterValueChangedCallback(onChanged);
            return field;
        }

        /// <summary>
        /// 创建带样式类的文本标签
        /// 替代「new Label(x); label.AddToClassList(...)」两行写法，样式类口径统一走 MapToolStyleClasses
        /// </summary>
        /// <param name="text">标签文本</param>
        /// <param name="className">USS 类名（取值见 MapToolStyleClasses，null 表示不加类）</param>
        /// <returns>标签控件</returns>
        public static Label CreateLabel(string text, string className = null)
        {
            var label = new Label(text);
            if (!string.IsNullOrEmpty(className))
                label.AddToClassList(className);
            return label;
        }

        /// <summary>
        /// 在指定元素名的容器内创建下拉控件
        /// 替代「Q 容器 + 判空 + CreateDropdown + Add」四行重复结构，容器缺失时静默返回 null
        /// </summary>
        /// <param name="root">查找容器的根元素</param>
        /// <param name="containerName">容器元素名（取值见 MapToolUiNames）</param>
        /// <param name="choices">选项列表</param>
        /// <param name="defaultValue">默认选项（null 表示不预设）</param>
        /// <param name="onChanged">变更回调（可空）</param>
        /// <returns>创建的下拉控件；容器缺失时返回 null</returns>
        public static DropdownField CreateDropdownIn(VisualElement root, string containerName,
            List<string> choices, string defaultValue = null,
            EventCallback<ChangeEvent<string>> onChanged = null)
        {
            var field = CreateDropdown(choices, defaultValue, onChanged);
            return MountIn(root, containerName, field);
        }

        /// <summary>
        /// 把已创建的控件挂到指定元素名的容器内
        /// 替代「Q 容器 + 判空 + Add」三行重复结构，容器缺失时静默返回 null
        /// </summary>
        /// <typeparam name="T">控件类型</typeparam>
        /// <param name="root">查找容器的根元素</param>
        /// <param name="containerName">容器元素名（取值见 MapToolUiNames）</param>
        /// <param name="field">控件实例</param>
        /// <returns>传入的控件；容器缺失时返回 null</returns>
        public static T MountIn<T>(VisualElement root, string containerName, T field)
            where T : VisualElement
        {
            var container = root.QByName<VisualElement>(containerName);
            if (container == null) return null;

            container.Add(field);
            return field;
        }
    }
}
