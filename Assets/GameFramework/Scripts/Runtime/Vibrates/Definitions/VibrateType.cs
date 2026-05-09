namespace Honor.Runtime
{
    /// <summary>
    /// 设备震动反馈类型枚举
    /// 对应 Nice Vibrations 插件的预设震动效果，用于游戏中不同场景的触觉反馈
    /// </summary>
    public enum VibrateType : byte
    {
        /// <summary>
        /// 无震动 / 无效类型
        /// </summary>
        None = 0,

        /// <summary>
        /// 极轻微震动（用于按钮点击、选项切换等轻量反馈）
        /// </summary>
        Selection,

        /// <summary>
        /// 成功/胜利震动（关卡胜利、任务完成、解锁成功）
        /// </summary>
        Success,

        /// <summary>
        /// 警告震动（低血量、异常状态、危险提示）
        /// </summary>
        Warning,

        /// <summary>
        /// 失败震动（游戏失败、操作错误、技能打断）
        /// </summary>
        Failure,

        /// <summary>
        /// 轻微撞击震动（轻攻击、道具拾取、UI确认）
        /// </summary>
        LightImpact,

        /// <summary>
        /// 中度撞击震动（普通攻击、UI确认、技能释放）
        /// </summary>
        MediumImpact,

        /// <summary>
        /// 重度撞击震动（重击、爆炸、大招、落地）
        /// </summary>
        HeavyImpact,

        /// <summary>
        /// 刚性撞击震动（坚硬物体碰撞、金属打击、硬直反馈）
        /// </summary>
        RigidImpact,

        /// <summary>
        /// 柔软撞击震动（柔软物体碰撞、跳跃落地、轻柔技能）
        /// </summary>
        SoftImpact,
    }
}