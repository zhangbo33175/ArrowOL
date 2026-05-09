using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// Inspector 按钮生成特性
    /// 标记在字段上，可在编辑器面板自动生成调用指定方法的按钮
    /// 用法：[GameInspectorButton(nameof(MethodName))]
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Field)]
    public class GameInspectorButtonAttribute : PropertyAttribute
    {
        /// <summary>
        /// 按钮点击后调用的方法名
        /// </summary>
        public readonly string MethodName;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="methodName">需要调用的方法名称（推荐使用 nameof）</param>
        public GameInspectorButtonAttribute(string methodName)
        {
            MethodName = methodName;
        }
    }
}