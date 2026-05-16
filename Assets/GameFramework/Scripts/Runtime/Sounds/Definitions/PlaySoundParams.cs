/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * -------------------------------------------------------------
 * filename:  PlaySoundParams.cs
 * author:  云毅
 * created:
 * descrip:   声音播放参数类 —— 统一封装音量、循环、优先级、3D音效等全部参数
 ***************************************************************/

namespace Honor.Runtime
{
    /// <summary>
    /// 声音播放参数类
    /// 作用：封装播放音频所需的所有配置参数，统一传递、复用、重置
    /// 支持：音量、循环、优先级、音调、3D空间、多普勒、淡入等
    /// </summary>
    public sealed class PlaySoundParams
    {
        /// <summary>
        /// 播放起始时间点
        /// </summary>
        private float m_Time;

        /// <summary>
        /// 在本组内单独静音
        /// </summary>
        private bool m_MuteInSoundGroup;

        /// <summary>
        /// 是否循环播放
        /// </summary>
        private bool m_Loop;

        /// <summary>
        /// 声音优先级（值越小优先级越高）
        /// </summary>
        private int m_Priority;

        /// <summary>
        /// 本组内的相对音量
        /// </summary>
        private float m_VolumeInSoundGroup;

        /// <summary>
        /// 淡入时间（秒）
        /// </summary>
        private float m_FadeInSeconds;

        /// <summary>
        /// 音调（播放速度）
        /// </summary>
        private float m_Pitch;

        /// <summary>
        /// 立体声相位（左右声道）
        /// </summary>
        private float m_PanStereo;

        /// <summary>
        /// 空间混合量（0=2D，1=3D）
        /// </summary>
        private float m_SpatialBlend;

        /// <summary>
        /// 3D声音最大距离
        /// </summary>
        private float m_MaxDistance;

        /// <summary>
        /// 多普勒效应强度
        /// </summary>
        private float m_DopplerLevel;

        /// <summary>
        /// 构造函数：使用默认常量初始化所有参数
        /// </summary>
        public PlaySoundParams()
        {
            Clear();
        }

        /// <summary>
        /// 播放起始位置
        /// </summary>
        public float Time
        {
            get => m_Time;
            set => m_Time = value;
        }

        /// <summary>
        /// 是否在本组内单独静音
        /// </summary>
        public bool MuteInSoundGroup
        {
            get => m_MuteInSoundGroup;
            set => m_MuteInSoundGroup = value;
        }

        /// <summary>
        /// 是否循环播放
        /// </summary>
        public bool Loop
        {
            get => m_Loop;
            set => m_Loop = value;
        }

        /// <summary>
        /// 声音优先级（数值越小，优先级越高）
        /// </summary>
        public int Priority
        {
            get => m_Priority;
            set => m_Priority = value;
        }

        /// <summary>
        /// 在本组内的相对音量
        /// </summary>
        public float VolumeInSoundGroup
        {
            get => m_VolumeInSoundGroup;
            set => m_VolumeInSoundGroup = value;
        }

        /// <summary>
        /// 淡入时间（秒）
        /// </summary>
        public float FadeInSeconds
        {
            get => m_FadeInSeconds;
            set => m_FadeInSeconds = value;
        }

        /// <summary>
        /// 音调 / 播放速度
        /// </summary>
        public float Pitch
        {
            get => m_Pitch;
            set => m_Pitch = value;
        }

        /// <summary>
        /// 立体声相位（-1左声道，1右声道）
        /// </summary>
        public float PanStereo
        {
            get => m_PanStereo;
            set => m_PanStereo = value;
        }

        /// <summary>
        /// 空间混合量：0=2D音效，1=3D音效
        /// </summary>
        public float SpatialBlend
        {
            get => m_SpatialBlend;
            set => m_SpatialBlend = value;
        }

        /// <summary>
        /// 3D声音最大衰减距离
        /// </summary>
        public float MaxDistance
        {
            get => m_MaxDistance;
            set => m_MaxDistance = value;
        }

        /// <summary>
        /// 多普勒效应强度
        /// </summary>
        public float DopplerLevel
        {
            get => m_DopplerLevel;
            set => m_DopplerLevel = value;
        }

        /// <summary>
        /// 静态创建方法（工厂模式）
        /// </summary>
        public static PlaySoundParams Create()
        {
            return new PlaySoundParams();
        }

        /// <summary>
        /// 重置所有参数为默认值（对象池复用）
        /// </summary>
        public void Clear()
        {
            m_Time                  = SoundConstant.DefaultTime;
            m_MuteInSoundGroup      = SoundConstant.DefaultMute;
            m_Loop                  = SoundConstant.DefaultLoop;
            m_Priority              = SoundConstant.DefaultPriority;
            m_VolumeInSoundGroup    = SoundConstant.DefaultVolume;
            m_FadeInSeconds         = SoundConstant.DefaultFadeInSeconds;
            m_Pitch                 = SoundConstant.DefaultPitch;
            m_PanStereo             = SoundConstant.DefaultPanStereo;
            m_SpatialBlend          = SoundConstant.DefaultSpatialBlend;
            m_MaxDistance           = SoundConstant.DefaultMaxDistance;
            m_DopplerLevel          = SoundConstant.DefaultDopplerLevel;
        }
    }
}