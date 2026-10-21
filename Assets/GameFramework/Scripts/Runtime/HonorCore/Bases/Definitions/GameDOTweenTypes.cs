/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  GameDOTweenTypes.cs
 * author:    云毅
 * created:   2026
 * descrip:   游戏全局DOTween动画唯一标识常量定义
 *            统一管理所有Tween动画ID，用于动画管理、强制回收、防止冲突
 ***************************************************************/

namespace Honor.Runtime
{
    #region 游戏全局 DOTween 动画唯一标识常量
    /// <summary>
    /// 游戏全局 DOTween 动画唯一标识常量
    /// </summary>
    /// <remarks>
    /// 用于统一管理所有 Tween 动画的 ID，方便缓存、管理、强制回收
    /// 避免动画冲突、内存泄漏，全局唯一标识
    /// </remarks>
    public static partial class GameDOTweenTypes
    {
        //=========================================================================
        // 流程切换动画
        //=========================================================================
        /// <summary>
        /// 流程切换 - 进入过渡动画ID
        /// </summary>
        public const string ProcedureTransitionInTween = "__DOTween__ProcedureTransitionEnterAnimation__";

        /// <summary>
        /// 流程切换 - 退出过渡动画ID
        /// </summary>
        public const string ProcedureTransitionOutTween = "__DOTween__ProcedureTransitionExitAnimation__";

        //=========================================================================
        // 闪屏界面动画
        //=========================================================================
        /// <summary>
        /// 闪屏界面 - 延时展示动画ID
        /// </summary>
        public const string SplashShowTween = "__DOTween__SplashDurationAnimation__";

        /// <summary>
        /// 启动加载界面 - 进入/退出过渡动画ID
        /// </summary>
        public const string LauncherLoadingViewTween = "__DOTween__LauncherLoadingViewTransition__";

        //=========================================================================
        // 相机相关动画
        //=========================================================================
        /// <summary>
        /// 相机综合动画ID
        /// </summary>
        public const string CameraMasterTween = "__DOTween__CameraAnimation__";

        /// <summary>
        /// 相机 - 位移动画ID
        /// </summary>
        public const string CameraMoveTween = "__DOTween__CameraMoveAnimation__";

        /// <summary>
        /// 相机 - 旋转动画ID
        /// </summary>
        public const string CameraRotateTween = "__DOTween__CameraRotateAnimation__";

        /// <summary>
        /// 相机 - 缩放动画ID
        /// </summary>
        public const string CameraScaleTween = "__DOTween__CameraScaleAnimation__";

        //=========================================================================
        // 震动效果动画
        //=========================================================================
        /// <summary>
        /// 自定义震动 - 前置延时计时器ID
        /// </summary>
        public const string CustomVibrateDelayTween = "__DOTween__CustomVibratePreDuration__";

        /// <summary>
        /// 自定义震动 - 持续时间计时器ID
        /// </summary>
        public const string CustomVibrateSustainTween = "__DOTween__CustomVibrateDuration__";

        /// <summary>
        /// 重点震动 - 前置延时计时器ID
        /// </summary>
        public const string EmphasisVibrateDelayTween = "__DOTween__EmphasisVibratePreDuration__";

        /// <summary>
        /// 重点震动 - 持续时间计时器ID
        /// </summary>
        public const string EmphasisVibrateSustainTween = "__DOTween__EmphasisVibrateDuration__";

        //=========================================================================
        // MAX 广告动画
        //=========================================================================
        /// <summary>
        /// MAX 广告 - 插屏广告计时器ID
        /// </summary>
        public const string MaxInterstitialAdTween = "__DOTween__MaxHelperInterAd__";

        /// <summary>
        /// MAX 广告 - Banner广告计时器ID
        /// </summary>
        public const string MaxBannerAdTween = "__DOTween__MaxHelperBannerAd__";

        /// <summary>
        /// MAX 广告 - 激励视频广告计时器ID
        /// </summary>
        public const string MaxRewardedVideoAdTween = "__DOTween__MaxHelperRVAd__";

        //=========================================================================
        // UI 通用动画
        //=========================================================================
        /// <summary>
        /// UI淡入淡出器动画ID
        /// </summary>
        public const string UiFadeTween = "__DOTween__UIFader__";

        //=========================================================================
        // 调试工具动画
        //=========================================================================
        /// <summary>
        /// 调试工具 - 上传日志文字动画ID
        /// </summary>
        public const string DebuggerLogTextTween = "__DOTween__DebuggerUploadWordsChanging__";
    }
    #endregion
}