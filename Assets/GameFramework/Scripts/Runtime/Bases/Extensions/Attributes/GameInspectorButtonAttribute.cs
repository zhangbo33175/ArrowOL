/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  GameInspectorButtonAttribute.cs
 * author:    云毅
 * created:   2026
 * descrip:   Inspector面板按钮生成特性，标记字段自动生成调用方法的按钮
 *            配合编辑器扩展实现一键调用逻辑，提升开发调试效率
 ***************************************************************/

using UnityEngine;

namespace Honor.Runtime
{
    #region Inspector 按钮生成特性
    /// <summary>
    /// Inspector 按钮生成特性
    /// </summary>
    /// <remarks>
    /// 标记在字段上，可在编辑器面板自动生成调用指定方法的按钮
    /// 配合框架编辑器扩展使用，简化开发调试流程
    /// </remarks>
    [System.AttributeUsage(System.AttributeTargets.Field)]
    public class GameInspectorButtonAttribute : PropertyAttribute
    {
        //=========================================================================
        // 公共字段
        //=========================================================================
        /// <summary>
        /// 按钮点击后执行的目标方法名称
        /// </summary>
        public readonly string MethodName;

        //=========================================================================
        // 构造函数
        //=========================================================================
        /// <summary>
        /// 构造 Inspector 按钮特性
        /// </summary>
        /// <param name="methodName">按钮触发的方法名（推荐使用 nameof() 保证安全）</param>
        public GameInspectorButtonAttribute(string methodName)
        {
            MethodName = methodName;
        }
    }
    #endregion
}