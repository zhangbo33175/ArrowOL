namespace Honor.Runtime
{
    public enum GameEventCmd
    {
        /// <summary>
        /// 无效
        /// </summary>
        None = 0,

        /// <summary>
        /// 流程放行
        /// </summary>
        FlowPermit,

        /// <summary>
        /// 流程禁止
        /// </summary>
        FlowUnPermit,

        /// <summary>
        /// 流程切换过渡进入完毕
        /// </summary>
        ProcedureTransitionEnterOver,

        /// <summary>
        /// 流程切换过渡退出完毕
        /// </summary>
        ProcedureTransitionExitOver,

        /// <summary>
        /// 文本本地化刷新
        /// </summary>
        TextLocalizingRefresh,

        /// <summary>
        /// 加载进度
        /// </summary>
        LoadProgress,

        /// <summary>
        /// 热更跳过
        /// </summary>
        HotfixSkip,

        /// <summary>
        /// 热更准备
        /// </summary>
        HotfixReady,

        /// <summary>
        /// 热更进度
        /// </summary>
        HotfixProgress,

        /// <summary>
        /// 热更新全部结束
        /// </summary>
        HotfixAllOver,

        /// <summary>
        /// 热更异常
        /// </summary>
        HotfixError,

        /// <summary>
        /// 大版本更新
        /// </summary>
        AppDownload,

        /// <summary>
        /// UnityGameService初始化结束
        /// </summary>
        GameServiceInitialized,

        /// <summary>
        /// GDPR结束
        /// </summary>
        GDPROver,

        /// <summary>
        /// 屏幕方向发生改变
        /// </summary>
        ScreenOrientationChanged,

        /// <summary>
        /// UI渐变器渐变进入
        /// </summary>
        UIFadeIn,

        /// <summary>
        /// UI渐变器退出
        /// </summary>
        UIFadeOut,

        /// <summary>
        /// UI渐变器停止
        /// </summary>
        UIFadeStop,

        /// <summary>
        /// 角色
        /// </summary>
        Character,

        /// <summary>
        /// 生命周期
        /// </summary>
        LifeCycle,

        /// <summary>
        /// 受伤
        /// </summary>
        DamageTaken,

        /// <summary>
        /// 受伤
        /// </summary>
        HealthChange,

        /// <summary>
        /// 状态机状态切换
        /// </summary>
        StateChange,
    }
}