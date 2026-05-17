/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * -------------------------------------------------------------
 * filename:  PatternEnums.cs
 * author:    云毅
 * created:   2026
 * descrip:  框架设计模式相关枚举定义
 ***************************************************************/

namespace Honor.Runtime
{
    /// <summary>
    /// 设计模式类型
    /// </summary>
    public enum PatternType
    {
        /// <summary>
        /// 普通标准模式
        /// </summary>
        None = 0,

        /// <summary>
        /// MVVM 模式（Model - View - ViewModel）
        /// </summary>
        MVVM = 1,
    }

    /// <summary>
    /// 普通模式（None）子类型枚举
    /// </summary>
    public enum NonePatternType
    {
        /// <summary>
        /// 默认单一脚本
        /// </summary>
        Default = 0,

        /// <summary>
        /// 总数量（用于数组长度）
        /// </summary>
        TotalNum = 1,
    }

    /// <summary>
    /// MVVM 模式子类型枚举
    /// </summary>
    public enum MVVMPatternType
    {
        /// <summary>
        /// 视图层（View）
        /// </summary>
        View = 0,

        /// <summary>
        /// 视图模型层（ViewModel）
        /// </summary>
        ViewModel = 1,

        /// <summary>
        /// 总数量（用于数组长度）
        /// </summary>
        TotalNum = 2,
    }
}