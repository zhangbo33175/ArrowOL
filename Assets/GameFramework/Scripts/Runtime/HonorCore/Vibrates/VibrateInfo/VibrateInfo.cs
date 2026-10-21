/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  VibrateInfo.cs
 * author:    云毅
 * created:   2026
 * descrip:   震动信息数据类 - 封装震动参数，支持 Lua 调用
 ***************************************************************/
using XLua;

namespace Honor.Runtime
{
    /// <summary>
    /// 震动信息数据类
    /// 用于封装设备震动（手机振动）的所有参数，支持Lua调用
    /// </summary>
    [CSharpCallLua]
    public class VibrateInfo
    {
        /// <summary>
        /// 震动强度（0~1）
        /// </summary>
        public float Intensity { get; set; }

        /// <summary>
        /// 震动尖锐度 / 触感清晰度（0~1）
        /// </summary>
        public float Sharpness { get; set; }

        /// <summary>
        /// 前置延迟时间（秒）
        /// </summary>
        public float PreDuration { get; set; }

        /// <summary>
        /// 震动持续总时间（秒）
        /// </summary>
        public float Duration { get; set; }

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="intensity">震动强度</param>
        /// <param name="sharpness">震动尖锐度</param>
        /// <param name="preDuration">前置延迟</param>
        /// <param name="duration">持续时间</param>
        public VibrateInfo(float intensity, float sharpness, float preDuration, float duration)
        {
            Intensity    = intensity;
            Sharpness    = sharpness;
            PreDuration  = preDuration;
            Duration     = duration;
        }
    }
}