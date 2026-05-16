namespace Honor.Runtime
{
    #region 游戏全局 DOTween 动画唯一标识常量
    /// <summary>
    /// 游戏全局 DOTween 动画唯一标识常量
    /// 用于统一管理所有 Tween 动画的 ID，方便缓存、管理、强制回收
    /// 避免动画冲突、内存泄漏
    /// </summary>
    public static partial class GameDOTweenTypes
    {
        //=========================================================================
        // 流程切换动画
        //=========================================================================
        /// <summary>
        /// 流程切换 - 进入过渡动画 ID
        /// </summary>
        public const string ProcedureTransitionEnterAnimation = "__DOTween__ProcedureTransitionEnterAnimation__";

        /// <summary>
        /// 流程切换 - 退出过渡动画 ID
        /// </summary>
        public const string ProcedureTransitionExitAnimation = "__DOTween__ProcedureTransitionExitAnimation__";

        //=========================================================================
        // 闪屏界面动画
        //=========================================================================
        /// <summary>
        /// 闪屏界面 - 延时展示动画 ID
        /// </summary>
        public const string SplashDurationAnimation = "__DOTween__SplashDurationAnimation__";

        //=========================================================================
        // 相机相关动画
        //=========================================================================
        /// <summary>
        /// 相机综合动画 ID
        /// </summary>
        public const string CameraAnimation = "__DOTween__CameraAnimation__";

        /// <summary>
        /// 相机 - 位移动画 ID
        /// </summary>
        public const string CameraMoveAnimation = "__DOTween__CameraMoveAnimation__";

        /// <summary>
        /// 相机 - 旋转动画 ID
        /// </summary>
        public const string CameraRotateAnimation = "__DOTween__CameraRotateAnimation__";

        /// <summary>
        /// 相机 - 缩放动画 ID
        /// </summary>
        public const string CameraScaleAnimation = "__DOTween__CameraScaleAnimation__";

        //=========================================================================
        // 震动效果动画
        //=========================================================================
        /// <summary>
        /// 自定义震动 - 前置延时计时器 ID
        /// </summary>
        public const string CustomVibratePreDuration = "__DOTween__CustomVibratePreDuration__";

        /// <summary>
        /// 自定义震动 - 持续时间计时器 ID
        /// </summary>
        public const string CustomVibrateDuration = "__DOTween__CustomVibrateDuration__";

        /// <summary>
        /// 重点震动 - 前置延时计时器 ID
        /// </summary>
        public const string EmphasisVibratePreDuration = "__DOTween__EmphasisVibratePreDuration__";

        /// <summary>
        /// 重点震动 - 持续时间计时器 ID
        /// </summary>
        public const string EmphasisVibrateDuration = "__DOTween__EmphasisVibrateDuration__";

        //=========================================================================
        // MAX 广告动画
        //=========================================================================
        /// <summary>
        /// MAX 广告 - 插屏广告计时器 ID
        /// </summary>
        public const string MaxHelperInterAd = "__DOTween__MaxHelperInterAd__";

        /// <summary>
        /// MAX 广告 - Banner 广告计时器 ID
        /// </summary>
        public const string MaxHelperBannerAd = "__DOTween__MaxHelperBannerAd__";

        /// <summary>
        /// MAX 广告 - 激励视频广告计时器 ID
        /// </summary>
        public const string MaxHelperRVAd = "__DOTween__MaxHelperRVAd__";

        //=========================================================================
        // UI 通用动画
        //=========================================================================
        /// <summary>
        /// UI 淡入淡出器动画 ID
        /// </summary>
        public const string UIFader = "__DOTween__UIFader__";

        //=========================================================================
        // 调试工具动画
        //=========================================================================
        /// <summary>
        /// 调试工具 - 上传日志文字动画 ID
        /// </summary>
        public const string DebuggerUploadWordsChanging = "__DOTween__DebuggerUploadWordsChanging__";
    }
    #endregion
}