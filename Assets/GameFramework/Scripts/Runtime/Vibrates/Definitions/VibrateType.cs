
namespace Honor.Runtime
{
    public enum VibrateType : byte
    {
        /// <summary>
        /// 无效
        /// </summary>
        None = 0,

        /// <summary>
        /// 极轻微振动（可用于点击按钮）
        /// </summary>
        Selection,

        /// <summary>
        /// 普通振动（可用于游戏胜利）
        /// </summary>
        Success,

        /// <summary>
        /// 普通振动（可用于游戏提示）
        /// </summary>
        Warning,

        /// <summary>
        /// 普通振动（可用于游戏失败）
        /// </summary>
        Failure,

        /// <summary>
        /// 轻微振动
        /// </summary>
        LightImpact,

        /// <summary>
        /// 中度振动
        /// </summary>
        MediumImpact,

        /// <summary>
        /// 重度振动
        /// </summary>
        HeavyImpact,

        /// <summary>
        /// 强硬而时间稍长一些的振动
        /// </summary>
        RigidImpact,

        /// <summary>
        /// 柔软而时间稍长一些的振动
        /// </summary>
        SoftImpact,

    }
}


