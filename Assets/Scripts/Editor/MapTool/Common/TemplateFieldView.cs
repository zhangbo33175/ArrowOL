/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  TemplateFieldView.cs
 * author:    云毅
 * created:   2026
 * descrip:   模板字段视图绑定（一个字段 = 一对「回填 + 写回」操作）
 *            属性面板的读（回填）与写（回调）共用同一处定义，避免两张平铺表维护
 ***************************************************************/

using System;
using UnityEngine.UIElements;

namespace Honor.Editor.MapTool
{
    /// <summary>
    /// 模板字段视图：把「控件 ← 模板」与「控件 → 模板」收敛为一个描述对象，
    /// 字段增删只改一处，回填与写回不会再出现漏改
    /// </summary>
    public sealed class TemplateFieldView
    {
        private readonly Action<ObjectTemplate> m_Load;
        private readonly Action m_RegisterCallbacks;

        /// <summary>
        /// 构造字段视图
        /// </summary>
        /// <param name="load">回填操作（模板 → 控件）</param>
        /// <param name="registerCallbacks">注册写回回调（控件 → 模板）</param>
        private TemplateFieldView(Action<ObjectTemplate> load, Action registerCallbacks)
        {
            m_Load = load;
            m_RegisterCallbacks = registerCallbacks;
        }

        /// <summary>
        /// 创建字段视图（控件为空时回填与写回均为空操作）
        /// </summary>
        /// <typeparam name="T">控件值类型</typeparam>
        /// <param name="field">目标控件</param>
        /// <param name="read">模板 → 控件值（含格式化：如浮点保留一位小数、枚举取显示名）</param>
        /// <param name="write">控件值 → 模板（含解析与归一：如容错解析、枚举转 code）</param>
        /// <param name="getTarget">取当前编辑目标（可返回 null）</param>
        /// <returns>字段视图</returns>
        public static TemplateFieldView Create<T>(BaseField<T> field,
            Func<ObjectTemplate, T> read,
            Action<ObjectTemplate, T> write,
            Func<ObjectTemplate> getTarget)
        {
            if (field == null) return new TemplateFieldView(_ => { }, () => { });

            return new TemplateFieldView(
                template =>
                {
                    if (template != null) field.SetValueSafe(read(template));
                },
                () => field.RegisterValueChangedCallback(evt =>
                {
                    var target = getTarget();
                    if (target != null) write(target, evt.newValue);
                }));
        }

        /// <summary>
        /// 用模板值回填控件
        /// </summary>
        /// <param name="template">模板数据</param>
        public void LoadValue(ObjectTemplate template)
        {
            m_Load(template);
        }

        /// <summary>
        /// 注册控件变更写回模板
        /// </summary>
        public void BindCallbacks()
        {
            m_RegisterCallbacks();
        }
    }
}
