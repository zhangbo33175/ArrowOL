namespace Honor.Runtime
{
    /// <summary>
    /// 设计模式类型
    /// </summary>
    public enum PatternType
    {
        /// <summary>
        /// 普通模式
        /// </summary>
        None = 0,

        /// <summary>
        /// MVVM模式
        /// </summary>
        MVVM,

    }

    /// <summary>
    /// None设计模式子类型
    /// </summary>
    public enum NonePatternType
    {
        /// <summary>
        /// 默认
        /// </summary>
        Default = 0,

        /// <summary>
        /// 总数量
        /// </summary>
        TotalNum,

    }

    /// <summary>
    /// MVVM设计模式子类型
    /// </summary>
    public enum MVVMPatternType
    {
        /// <summary>
        /// View视图
        /// </summary>
        View = 0,

        /// <summary>
        /// ViewModel视图模型
        /// </summary>
        ViewModel,

        /// <summary>
        /// 总数量
        /// </summary>
        TotalNum,
    }

}


