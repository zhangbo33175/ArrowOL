/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * -------------------------------------------------------------
 * filename:  SoundConstant.cs
 * author:  云毅
 * created:
 * descrip:   音频系统常量 —— 统一管理默认音量、优先级、3D参数等全局配置
 ***************************************************************/

namespace Honor.Runtime
{
    /// <summary>
    /// 音频系统常量（内部静态类）
    /// 作用：统一管理所有音频组件的默认值，方便全局修改、维护、规范配置
    /// </summary>
    internal static class SoundConstant
    {
        /// <summary>
        /// 默认播放起始时间（从头播放）
        /// </summary>
        internal const float InitialTime = 0f;

        /// <summary>
        /// 默认静音状态（不静音）
        /// </summary>
        internal const bool InitialMute = false;

        /// <summary>
        /// 默认循环状态（不循环）
        /// </summary>
        internal const bool InitialLoop = false;

        /// <summary>
        /// 默认声音优先级（0为最高）
        /// </summary>
        internal const int InitialPriority = 0;

        /// <summary>
        /// 默认音量（最大音量1.0）
        /// </summary>
        internal const float InitialVolume = 1f;

        /// <summary>
        /// 默认淡入时间（无淡入）
        /// </summary>
        internal const float InitialFadeInSeconds = 0f;

        /// <summary>
        /// 默认淡出时间（无淡出）
        /// </summary>
        internal const float InitialFadeOutSeconds = 0f;

        /// <summary>
        /// 默认音调（正常速度1.0）
        /// </summary>
        internal const float InitialPitch = 1f;

        /// <summary>
        /// 默认立体声相位（居中）
        /// </summary>
        internal const float InitialPanStereo = 0f;

        /// <summary>
        /// 默认空间混合（2D音效）
        /// </summary>
        internal const float InitialSpatialBlend = 0f;

        /// <summary>
        /// 默认3D声音最大距离
        /// </summary>
        internal const float InitialMaxDistance = 100f;

        /// <summary>
        /// 默认多普勒效果等级
        /// </summary>
        internal const float InitialDopplerLevel = 1f;
    }
}