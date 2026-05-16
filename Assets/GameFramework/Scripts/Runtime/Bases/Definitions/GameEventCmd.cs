namespace Honor.Runtime
{
    #region 游戏全局事件枚举
    /// <summary>
    /// 游戏全局事件枚举（事件中心/消息中心命令）
    /// 用于框架内各模块解耦通信，所有事件统一在这里定义
    /// </summary>
    public enum GameEventCmd
    {
        //=========================================================================
        // 基础通用事件
        //=========================================================================
        /// <summary>
        /// 无效事件 / 默认值
        /// </summary>
        None = 0,

        //=========================================================================
        // 流程控制事件
        //=========================================================================
        /// <summary>
        /// 流程放行（流程节点允许继续执行）
        /// </summary>
        FlowPermit,

        /// <summary>
        /// 流程禁止（流程节点暂停执行）
        /// </summary>
        FlowUnPermit,

        /// <summary>
        /// 流程切换过渡动画 - 进入完成
        /// </summary>
        ProcedureTransitionEnterOver,

        /// <summary>
        /// 流程切换过渡动画 - 退出完成
        /// </summary>
        ProcedureTransitionExitOver,

        //=========================================================================
        // 多语言与UI事件
        //=========================================================================
        /// <summary>
        /// 文本多语言本地化刷新
        /// </summary>
        TextLocalizingRefresh,

        /// <summary>
        /// UI 淡入动画开始/完成
        /// </summary>
        UIFadeIn,

        /// <summary>
        /// UI 淡出动画开始/完成
        /// </summary>
        UIFadeOut,

        /// <summary>
        /// UI 淡入淡出动画强制停止
        /// </summary>
        UIFadeStop,

        //=========================================================================
        // 资源加载与热更新事件
        //=========================================================================
        /// <summary>
        /// 资源加载进度更新
        /// </summary>
        LoadProgress,

        /// <summary>
        /// 热更新跳过
        /// </summary>
        HotfixSkip,

        /// <summary>
        /// 热更新准备完成
        /// </summary>
        HotfixReady,

        /// <summary>
        /// 热更新进度更新
        /// </summary>
        HotfixProgress,

        /// <summary>
        /// 热更新全部流程结束
        /// </summary>
        HotfixAllOver,

        /// <summary>
        /// 热更新发生异常
        /// </summary>
        HotfixError,

        /// <summary>
        /// 触发大版本更新（App商店更新）
        /// </summary>
        AppDownload,

        //=========================================================================
        // 游戏服务与系统事件
        //=========================================================================
        /// <summary>
        /// Unity 游戏服务初始化完成
        /// </summary>
        GameServiceInitialized,

        /// <summary>
        /// GDPR 隐私授权流程结束
        /// </summary>
        GDPROver,

        /// <summary>
        /// 屏幕方向发生改变
        /// </summary>
        ScreenOrientationChanged,

        //=========================================================================
        // 游戏核心逻辑事件
        //=========================================================================
        /// <summary>
        /// 角色相关事件（创建/销毁/切换）
        /// </summary>
        Character,

        /// <summary>
        /// 生命周期事件（加载/启动/销毁）
        /// </summary>
        LifeCycle,

        /// <summary>
        /// 受到伤害
        /// </summary>
        DamageTaken,

        /// <summary>
        /// 生命值发生变化
        /// </summary>
        HealthChange,

        /// <summary>
        /// 状态机状态切换完成
        /// </summary>
        StateChange,
    }
    #endregion
}