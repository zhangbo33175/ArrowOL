using XLua;

namespace Honor.Runtime
{
    /// <summary>
    /// 震动信息数据类
    /// 用于封装设备震动（手机振动）的所有参数，支持Lua调用
    /// </summary>
    [CSharpCallLua] // 标记可被XLua调用
    public class VibrateInfo
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="intensity">震动强度（0-1）</param>
        /// <param name="sharpness">震动尖锐度/感知度（0-1）</param>
        /// <param name="preDuration">震动前延迟时间（秒）>0</param>
        /// <param name="duration">震动总持续时间（秒）>0</param>
        public VibrateInfo(float intensity, float sharpness, float preDuration, float duration)
        {
            Intensity = intensity;
            Sharpness = sharpness;
            PreDuration = preDuration;
            Duration = duration;
        }

        /// <summary>
        /// 震动强度（0~1）
        /// </summary>
        public float Intensity { get; set; }

        /// <summary>
        /// 震动尖锐度 / 触感清晰度（0~1）
        /// </summary>
        public float Sharpness { get; set; }

        /// <summary>
        /// 前置延迟时间（开始震动前等待多久，单位：秒）
        /// </summary>
        public float PreDuration { get; set; }

        /// <summary>
        /// 震动持续总时间（单位：秒）
        /// </summary>
        public float Duration { get; set; }
    }
}