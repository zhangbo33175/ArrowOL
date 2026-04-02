using XLua;

namespace Honor.Runtime
{
    public class VibrateInfo
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="intensity">强度（0-1）</param>
        /// <param name="sharpness">感知度（0-1）</param>
        /// <param name="preDuration">前奏空闲持续时间（>0）</param>
        /// <param name="duration">持续时间（Custom）or 间隔时间（Emphasis）（>0）</param>
        public VibrateInfo(float intensity, float sharpness, float preDuration, float duration)
        {
            Intensity = intensity;
            Sharpness = sharpness;
            PreDuration = preDuration;
            Duration = duration;
        }

        /// <summary>
        /// 强度（0-1）
        /// </summary>
        public float Intensity
        {
            set;
            get;
        }

        /// <summary>
        /// 感知度（0-1）
        /// </summary>
        public float Sharpness
        {
            set;
            get;
        }

        /// <summary>
        /// 前奏空闲持续时间（>0）
        /// </summary>
        public float PreDuration
        {
            set;
            get;
        }

        /// <summary>
        /// 持续时间（Custom）or 间隔时间（Emphasis）（>0）
        /// </summary>
        public float Duration
        {
            set;
            get;
        }
    }
}


