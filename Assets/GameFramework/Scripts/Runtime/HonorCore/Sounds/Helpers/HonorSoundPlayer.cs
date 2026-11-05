/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  HonorSoundPlayer.cs
 * author:    云毅
 * created:   2026
 * descrip:   声音代理 —— 封装单个AudioSource，负责实际播放、暂停、停止、重置
 ***************************************************************/

using System;

namespace Honor.Runtime
{
    /// <summary>
    /// 声音代理（真正的声音播放器）
    /// 每个 HonorSoundPlayer 对应一个 AudioSource，负责播放单个音频
    /// 属于声音池对象，循环复用，避免频繁创建销毁
    /// </summary>
    public sealed class HonorSoundPlayer
    {
        /// <summary>
        /// 所属声音管理器
        /// </summary>
        private readonly HonorSoundManager m_HonorSoundManager;

        /// <summary>
        /// 所属声音组（BGM/音效/UI等）
        /// </summary>
        private readonly HonorSoundCategory m_HonorSoundCategory;

        /// <summary>
        /// 声音辅助器（封装 AudioSource 具体操作）
        /// </summary>
        private readonly HonorSoundPlayerHelper m_HonorSoundPlayerHelper;

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
        public HonorSoundCategory HonorSoundCategory => m_HonorSoundCategory;

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
        public bool IsPlaying => m_HonorSoundPlayerHelper.IsPlaying;

        /// <summary>
        /// 音频长度
        /// </summary>
        public float Length => m_HonorSoundPlayerHelper.Length;

        /// <summary>
        /// 播放位置（时间）
        /// </summary>
        public float Time
        {
            get => m_HonorSoundPlayerHelper.Time;
            set => m_HonorSoundPlayerHelper.Time = value;
        }

        /// <summary>
        /// 当前是否静音（最终结果）
        /// </summary>
        public bool Mute => m_HonorSoundPlayerHelper.Mute;

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
            get => m_HonorSoundPlayerHelper.Loop;
            set => m_HonorSoundPlayerHelper.Loop = value;
        }

        /// <summary>
        /// 优先级
        /// </summary>
        public int Priority
        {
            get => m_HonorSoundPlayerHelper.Priority;
            set => m_HonorSoundPlayerHelper.Priority = value;
        }

        /// <summary>
        /// 最终音量（组音量 * 自身音量）
        /// </summary>
        public float Volume => m_HonorSoundPlayerHelper.Volume;

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
            get => m_HonorSoundPlayerHelper.Pitch;
            set => m_HonorSoundPlayerHelper.Pitch = value;
        }

        /// <summary>
        /// 立体声相位
        /// </summary>
        public float PanStereo
        {
            get => m_HonorSoundPlayerHelper.PanStereo;
            set => m_HonorSoundPlayerHelper.PanStereo = value;
        }

        /// <summary>
        /// 空间混合（0=2D，1=3D）
        /// </summary>
        public float SpatialBlend
        {
            get => m_HonorSoundPlayerHelper.SpatialBlend;
            set => m_HonorSoundPlayerHelper.SpatialBlend = value;
        }

        /// <summary>
        /// 3D最大距离
        /// </summary>
        public float MaxDistance
        {
            get => m_HonorSoundPlayerHelper.MaxDistance;
            set => m_HonorSoundPlayerHelper.MaxDistance = value;
        }

        /// <summary>
        /// 多普勒强度
        /// </summary>
        public float DopplerLevel
        {
            get => m_HonorSoundPlayerHelper.DopplerLevel;
            set => m_HonorSoundPlayerHelper.DopplerLevel = value;
        }

        /// <summary>
        /// 获取辅助器对象
        /// </summary>
        public HonorSoundPlayerHelper Helper => m_HonorSoundPlayerHelper;

        /// <summary>
        /// 构造函数：初始化声音播放器
        /// </summary>
        public HonorSoundPlayer(HonorSoundCategory HonorSoundCategory, HonorSoundManager HonorSoundManager, HonorSoundPlayerHelper HonorSoundPlayerHelper)
        {
            if (HonorSoundCategory == null)
                throw new HonorException("Sound group 无效。");
            if (HonorSoundManager == null)
                throw new HonorException("Sound manager 无效。");
            if (HonorSoundPlayerHelper == null)
                throw new HonorException("Sound agent helper 无效。");

            m_HonorSoundManager = HonorSoundManager;
            m_HonorSoundCategory = HonorSoundCategory;
            m_HonorSoundPlayerHelper = HonorSoundPlayerHelper;
            m_SerialID = 0;
            m_SoundAsset = null;
            Reset();
        }

        /// <summary>
        /// 播放声音（默认淡入）
        /// </summary>
        public void Play()
        {
            Play(HonorSoundConstant.InitialFadeInSeconds);
        }

        /// <summary>
        /// 播放声音（自定义淡入）
        /// </summary>
        public void Play(float fadeInSeconds)
        {
            m_HonorSoundPlayerHelper.Play(fadeInSeconds);
        }

        /// <summary>
        /// 停止声音（默认淡出）
        /// </summary>
        public void Stop()
        {
            Stop(HonorSoundConstant.InitialFadeOutSeconds);
        }

        /// <summary>
        /// 停止声音（自定义淡出）
        /// </summary>
        public void Stop(float fadeOutSeconds)
        {
            m_HonorSoundPlayerHelper.Stop(fadeOutSeconds);
        }

        /// <summary>
        /// 暂停声音（默认淡出）
        /// </summary>
        public void Pause()
        {
            Pause(HonorSoundConstant.InitialFadeOutSeconds);
        }

        /// <summary>
        /// 暂停声音（自定义淡出）
        /// </summary>
        public void Pause(float fadeOutSeconds)
        {
            m_HonorSoundPlayerHelper.Pause(fadeOutSeconds);
        }

        /// <summary>
        /// 恢复播放（默认淡入）
        /// </summary>
        public void Resume()
        {
            Resume(HonorSoundConstant.InitialFadeInSeconds);
        }

        /// <summary>
        /// 恢复播放（自定义淡入）
        /// </summary>
        public void Resume(float fadeInSeconds)
        {
            m_HonorSoundPlayerHelper.Resume(fadeInSeconds);
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
                m_HonorSoundManager.ReleaseSoundAsset(m_SoundAsset);
                m_SoundAsset = null;
            }

            m_SetSoundAssetTime = DateTime.MinValue;
            Time = HonorSoundConstant.InitialTime;
            MuteInSoundGroup = HonorSoundConstant.InitialMute;
            Loop = HonorSoundConstant.InitialLoop;
            Priority = HonorSoundConstant.InitialPriority;
            VolumeInSoundGroup = HonorSoundConstant.InitialVolume;
            Pitch = HonorSoundConstant.InitialPitch;
            PanStereo = HonorSoundConstant.InitialPanStereo;
            SpatialBlend = HonorSoundConstant.InitialSpatialBlend;
            MaxDistance = HonorSoundConstant.InitialMaxDistance;
            DopplerLevel = HonorSoundConstant.InitialDopplerLevel;

            // 重置辅助器
            m_HonorSoundPlayerHelper.Reset();
        }

        /// <summary>
        /// 设置要播放的音频资源
        /// </summary>
        public bool SetSoundAsset(UnityEngine.Object soundAsset)
        {
            Reset();
            m_SoundAsset = soundAsset;
            m_SetSoundAssetTime = DateTime.Now;
            return m_HonorSoundPlayerHelper.SetSoundAsset(soundAsset);
        }

        /// <summary>
        /// 刷新静音状态 = 组静音 || 自身静音
        /// </summary>
        public void RefreshMute()
        {
            m_HonorSoundPlayerHelper.Mute = m_HonorSoundCategory.Mute || m_MuteInSoundGroup;
        }

        /// <summary>
        /// 刷新最终音量 = 组音量 * 自身音量
        /// </summary>
        public void RefreshVolume()
        {
            m_HonorSoundPlayerHelper.Volume = m_HonorSoundCategory.Volume * m_VolumeInSoundGroup;
        }
    }
}