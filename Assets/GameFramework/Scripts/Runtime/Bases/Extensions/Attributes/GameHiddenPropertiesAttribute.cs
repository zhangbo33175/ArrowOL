using System;

namespace Honor.Runtime
{
    /// <summary>
    /// 游戏框架 - 隐藏属性特性
    /// 用于标记在类上，指定需要在 Inspector 中隐藏的属性名称
    /// 配合自定义编辑器使用，实现批量隐藏默认属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class GameHiddenPropertiesAttribute : Attribute
    {
        /// <summary>
        /// 需要隐藏的属性名称数组
        /// </summary>
        public string[] PropertiesNames { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="propertiesNames">需要隐藏的属性名称</param>
        public GameHiddenPropertiesAttribute(params string[] propertiesNames)
        {
            PropertiesNames = propertiesNames;
        }
    }
}