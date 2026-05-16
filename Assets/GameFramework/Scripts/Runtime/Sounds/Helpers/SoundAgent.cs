/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * -------------------------------------------------------------
 * filename:  SoundAgent.cs
 * author:  云毅
 * created:
 * descrip:   声音代理 —— 封装单个AudioSource，负责实际播放、暂停、停止、重置
 ***************************************************************/

using System;

namespace Honor.Runtime
{
    /// <summary>
    /// 声音代理（真正的声音播放器）
    /// 每个 SoundAgent 对应一个 AudioSource，负责播放单个音频
    /// 属于声音池对象，循环复用，避免频繁创建销毁
    /// </summary>
    public sealed class SoundAgent
    {
        /// <summary>
        /// 所属声音管理器
        /// </summary>
        private readonly SoundManager m_SoundManager;

        /// <summary>
        /// 所属声音组（BGM/音效/UI等）
        /// </summary>
        private readonly SoundGroup m_SoundGroup;

        /// <summary>
        /// 声音辅助器（封装 AudioSource 具体操作）
        /// </summary>
        private readonly SoundAgentHelper m_SoundAgentHelper;

        /// <summary>
        /// 声音唯一序列ID
        /// </summary>
        private int m_SerialID;

        /// <summary>
        /// 当前播放的音频资源
        /// </summary>
        private UnityEngine.Object m_SoundAsset;

        /// <summary>
        /// 设置音频资源的时间（用于优先级淘汰）
        /// </summary>
        private DateTime m_SetSoundAssetTime;

        /// <summary>
        /// 本组内单独静音标记
        /// </summary>
        private bool m_MuteInSoundGroup;

        /// <summary>
        /// 本组内相对音量
        /// </summary>
        private float m_VolumeInSoundGroup;

        /// <summary>
        /// 所属声音组
        /// </summary>
        public SoundGroup SoundGroup => m_SoundGroup;

        /// <summary>
        /// 声音唯一序列ID
        /// </summary>
        public int SerialID
        {
            get => m_SerialID;
            set => m_SerialID = value;
        }

        /// <summary>
        /// 设置音频资源的时间
        /// </summary>
        public DateTime SetSoundAssetTime => m_SetSoundAssetTime;

        /// <summary>
        /// 当前是否正在播放
        /// </summary>
        public bool IsPlaying => m_SoundAgentHelper.IsPlaying;

        /// <summary>
        /// 音频长度
        /// </summary>
        public float Length => m_SoundAgentHelper.Length;

        /// <summary>
        /// 播放位置（时间）
        /// </summary>
        public float Time
        {
            get => m_SoundAgentHelper.Time;
            set => m_SoundAgentHelper.Time = value;
        }

        /// <summary>
        /// 当前是否静音（最终结果）
        /// </summary>
        public bool Mute => m_SoundAgentHelper.Mute;

        /// <summary>
        /// 本组内单独静音
        /// </summary>
        public bool MuteInSoundGroup
        {
            get => m_MuteInSoundGroup;
            set
            {
                m_MuteInSoundGroup = value;
                RefreshMute();
            }
        }

        /// <summary>
        /// 是否循环
        /// </summary>
        public bool Loop
        {
            get => m_SoundAgentHelper.Loop;
            set => m_SoundAgentHelper.Loop = value;
        }

        /// <summary>
        /// 优先级
        /// </summary>
        public int Priority
        {
            get => m_SoundAgentHelper.Priority;
            set => m_SoundAgentHelper.Priority = value;
        }

        /// <summary>
        /// 最终音量（组音量 * 自身音量）
        /// </summary>
        public float Volume => m_SoundAgentHelper.Volume;

        /// <summary>
        /// 本组内相对音量
        /// </summary>
        public float VolumeInSoundGroup
        {
            get => m_VolumeInSoundGroup;
            set
            {
                m_VolumeInSoundGroup = value;
                RefreshVolume();
            }
        }

        /// <summary>
        /// 音调
        /// </summary>
        public float Pitch
        {
            get => m_SoundAgentHelper.Pitch;
            set => m_SoundAgentHelper.Pitch = value;
        }

        /// <summary>
        /// 立体声相位
        /// </summary>
        public float PanStereo
        {
            get => m_SoundAgentHelper.PanStereo;
            set => m_SoundAgentHelper.PanStereo = value;
        }

        /// <summary>
        /// 空间混合（0=2D，1=3D）
        /// </summary>
        public float SpatialBlend
        {
            get => m_SoundAgentHelper.SpatialBlend;
            set => m_SoundAgentHelper.SpatialBlend = value;
        }

        /// <summary>
        /// 3D最大距离
        /// </summary>
        public float MaxDistance
        {
            get => m_SoundAgentHelper.MaxDistance;
            set => m_SoundAgentHelper.MaxDistance = value;
        }

        /// <summary>
        /// 多普勒强度
        /// </summary>
        public float DopplerLevel
        {
            get => m_SoundAgentHelper.DopplerLevel;
            set => m_SoundAgentHelper.DopplerLevel = value;
        }

        /// <summary>
        /// 获取辅助器对象
        /// </summary>
        public SoundAgentHelper Helper => m_SoundAgentHelper;

        /// <summary>
        /// 构造函数：初始化声音播放器
        /// </summary>
        public SoundAgent(SoundGroup soundGroup, SoundManager soundManager, SoundAgentHelper soundAgentHelper)
        {
            if (soundGroup == null)
                throw new GameException("Sound group 无效。");
            if (soundManager == null)
                throw new GameException("Sound manager 无效。");
            if (soundAgentHelper == null)
                throw new GameException("Sound agent helper 无效。");

            m_SoundManager = soundManager;
            m_SoundGroup = soundGroup;
            m_SoundAgentHelper = soundAgentHelper;
            m_SerialID = 0;
            m_SoundAsset = null;
            Reset();
        }

        /// <summary>
        /// 播放声音（默认淡入）
        /// </summary>
        public void Play()
        {
            Play(SoundConstant.DefaultFadeInSeconds);
        }

        /// <summary>
        /// 播放声音（自定义淡入）
        /// </summary>
        public void Play(float fadeInSeconds)
        {
            m_SoundAgentHelper.Play(fadeInSeconds);
        }

        /// <summary>
        /// 停止声音（默认淡出）
        /// </summary>
        public void Stop()
        {
            Stop(SoundConstant.DefaultFadeOutSeconds);
        }

        /// <summary>
        /// 停止声音（自定义淡出）
        /// </summary>
        public void Stop(float fadeOutSeconds)
        {
            m_SoundAgentHelper.Stop(fadeOutSeconds);
        }

        /// <summary>
        /// 暂停声音（默认淡出）
        /// </summary>
        public void Pause()
        {
            Pause(SoundConstant.DefaultFadeOutSeconds);
        }

        /// <summary>
        /// 暂停声音（自定义淡出）
        /// </summary>
        public void Pause(float fadeOutSeconds)
        {
            m_SoundAgentHelper.Pause(fadeOutSeconds);
        }

        /// <summary>
        /// 恢复播放（默认淡入）
        /// </summary>
        public void Resume()
        {
            Resume(SoundConstant.DefaultFadeInSeconds);
        }

        /// <summary>
        /// 恢复播放（自定义淡入）
        /// </summary>
        public void Resume(float fadeInSeconds)
        {
            m_SoundAgentHelper.Resume(fadeInSeconds);
        }

        /// <summary>
        /// 重置播放器（回收进对象池）
        /// 释放资源、清空参数、恢复默认值
        /// </summary>
        public void Reset()
        {
            // 释放音频资源
            if (m_SoundAsset != null)
            {
                m_SoundManager.ReleaseSoundAsset(m_SoundAsset);
                m_SoundAsset = null;
            }

            m_SetSoundAssetTime = DateTime.MinValue;
            Time = SoundConstant.DefaultTime;
            MuteInSoundGroup = SoundConstant.DefaultMute;
            Loop = SoundConstant.DefaultLoop;
            Priority = SoundConstant.DefaultPriority;
            VolumeInSoundGroup = SoundConstant.DefaultVolume;
            Pitch = SoundConstant.DefaultPitch;
            PanStereo = SoundConstant.DefaultPanStereo;
            SpatialBlend = SoundConstant.DefaultSpatialBlend;
            MaxDistance = SoundConstant.DefaultMaxDistance;
            DopplerLevel = SoundConstant.DefaultDopplerLevel;

            // 重置辅助器
            m_SoundAgentHelper.Reset();
        }

        /// <summary>
        /// 设置要播放的音频资源
        /// </summary>
        public bool SetSoundAsset(UnityEngine.Object soundAsset)
        {
            Reset();
            m_SoundAsset = soundAsset;
            m_SetSoundAssetTime = DateTime.Now;
            return m_SoundAgentHelper.SetSoundAsset(soundAsset);
        }

        /// <summary>
        /// 刷新静音状态 = 组静音 || 自身静音
        /// </summary>
        public void RefreshMute()
        {
            m_SoundAgentHelper.Mute = m_SoundGroup.Mute || m_MuteInSoundGroup;
        }

        /// <summary>
        /// 刷新最终音量 = 组音量 * 自身音量
        /// </summary>
        public void RefreshVolume()
        {
            m_SoundAgentHelper.Volume = m_SoundGroup.Volume * m_VolumeInSoundGroup;
        }
    }
}