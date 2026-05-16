/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * -------------------------------------------------------------
 * filename:  SoundGroup.cs
 * author:    云毅
 * created:   2026
 * descrip:   声音组 —— 管理BGM/音效/UI等分组，负责播放器调度、优先级、音量统一控制
 ***************************************************************/

using System.Collections.Generic;
using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// 声音组（BGM / 音效 / UI / 角色 等分组）
    /// 作用：统一管理本组内所有声音播放器，处理优先级、复用、音量、静音
    /// </summary>
    public sealed class SoundGroup
    {
        /// <summary>
        /// 声音组名称（如 BGM、Effect、UI）
        /// </summary>
        private readonly string m_Name;

        /// <summary>
        /// 声音组辅助对象（挂载在 GameObject 上）
        /// </summary>
        private readonly SoundGroupHelper m_SoundGroupHelper;

        /// <summary>
        /// 本组所有声音播放器列表（对象池）
        /// </summary>
        private readonly List<SoundAgent> m_SoundAgents;

        /// <summary>
        /// 同优先级声音是否禁止互相替换
        /// </summary>
        private bool m_AvoidBeingReplacedBySamePriority;

        /// <summary>
        /// 本组全局静音
        /// </summary>
        private bool m_Mute;

        /// <summary>
        /// 本组全局音量
        /// </summary>
        private float m_Volume;

        /// <summary>
        /// 声音组名称
        /// </summary>
        public string Name => m_Name;

        /// <summary>
        /// 声音组辅助对象
        /// </summary>
        public SoundGroupHelper SoundGroupHelper => m_SoundGroupHelper;

        /// <summary>
        /// 本组播放器数量
        /// </summary>
        public int SoundAgentCount => m_SoundAgents.Count;

        /// <summary>
        /// 同优先级声音是否禁止互相替换
        /// </summary>
        public bool AvoidBeingReplacedBySamePriority
        {
            get => m_AvoidBeingReplacedBySamePriority;
            set => m_AvoidBeingReplacedBySamePriority = value;
        }

        /// <summary>
        /// 本组全局静音
        /// 设置时自动刷新所有播放器
        /// </summary>
        public bool Mute
        {
            get => m_Mute;
            set
            {
                m_Mute = value;
                foreach (SoundAgent agent in m_SoundAgents)
                {
                    agent.RefreshMute();
                }
            }
        }

        /// <summary>
        /// 本组全局音量
        /// 设置时自动刷新所有播放器
        /// </summary>
        public float Volume
        {
            get => m_Volume;
            set
            {
                m_Volume = value;
                foreach (SoundAgent agent in m_SoundAgents)
                {
                    agent.RefreshVolume();
                }
            }
        }

        /// <summary>
        /// 构造：创建声音组
        /// </summary>
        public SoundGroup(string name, SoundGroupHelper soundGroupHelper)
        {
            if (string.IsNullOrEmpty(name))
                throw new GameException("Sound group name 无效。");
            if (soundGroupHelper == null)
                throw new GameException("Sound group helper 无效。");

            m_Name = name;
            m_SoundGroupHelper = soundGroupHelper;
            m_SoundAgents = new List<SoundAgent>();
        }

        /// <summary>
        /// 添加一个声音播放器到本组
        /// </summary>
        public void AddSoundAgent(SoundManager soundManager, SoundAgentHelper soundAgentHelper)
        {
            m_SoundAgents.Add(new SoundAgent(this, soundManager, soundAgentHelper));
        }

        /// <summary>
        /// 核心方法：播放声音（自动选择空闲/可淘汰播放器）
        /// </summary>
        public SoundAgent PlaySound(int serialID, Object soundAsset, PlaySoundParams playSoundParams, out PlaySoundErrorCode? errorCode)
        {
            errorCode = null;
            SoundAgent candidateAgent = null;
            int busyCount = 0;

            // 遍历所有播放器，找最合适的
            foreach (SoundAgent agent in m_SoundAgents)
            {
                // 1. 优先用空闲播放器
                if (!agent.IsPlaying)
                {
                    candidateAgent = agent;
                    break;
                }

                busyCount++;

                // 2. 找优先级更低的，可以被顶替
                if (agent.Priority < playSoundParams.Priority)
                {
                    if (candidateAgent == null || agent.Priority < candidateAgent.Priority)
                        candidateAgent = agent;
                }
                // 3. 同优先级，允许顶替最早播放的
                else if (!m_AvoidBeingReplacedBySamePriority && agent.Priority == playSoundParams.Priority)
                {
                    if (candidateAgent == null || agent.SetSoundAssetTime < candidateAgent.SetSoundAssetTime)
                        candidateAgent = agent;
                }
            }

            // 没有可用播放器
            if (candidateAgent == null)
            {
                errorCode = busyCount == m_SoundAgents.Count 
                    ? PlaySoundErrorCode.SoundGroupHasNotEnoughAgent 
                    : PlaySoundErrorCode.IgnoredDueToLowPriority;
                return null;
            }

            // 设置音频资源失败
            if (!candidateAgent.SetSoundAsset(soundAsset))
            {
                errorCode = PlaySoundErrorCode.SetSoundAssetFailure;
                return null;
            }

            // 赋值播放参数
            candidateAgent.SerialID = serialID;
            candidateAgent.Time = playSoundParams.Time;
            candidateAgent.MuteInSoundGroup = playSoundParams.MuteInSoundGroup;
            candidateAgent.Loop = playSoundParams.Loop;
            candidateAgent.Priority = playSoundParams.Priority;
            candidateAgent.VolumeInSoundGroup = playSoundParams.VolumeInSoundGroup;
            candidateAgent.Pitch = playSoundParams.Pitch;
            candidateAgent.PanStereo = playSoundParams.PanStereo;
            candidateAgent.SpatialBlend = playSoundParams.SpatialBlend;
            candidateAgent.MaxDistance = playSoundParams.MaxDistance;
            candidateAgent.DopplerLevel = playSoundParams.DopplerLevel;
            
            // 开始播放
            candidateAgent.Play(playSoundParams.FadeInSeconds);
            return candidateAgent;
        }

        /// <summary>
        /// 停止指定声音
        /// </summary>
        public bool StopSound(int serialID, float fadeOutSeconds)
        {
            foreach (SoundAgent agent in m_SoundAgents)
            {
                if (agent.SerialID == serialID)
                {
                    agent.Stop(fadeOutSeconds);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 暂停指定声音
        /// </summary>
        public bool PauseSound(int serialID, float fadeOutSeconds)
        {
            foreach (SoundAgent agent in m_SoundAgents)
            {
                if (agent.SerialID == serialID)
                {
                    agent.Pause(fadeOutSeconds);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 恢复指定声音
        /// </summary>
        public bool ResumeSound(int serialID, float fadeInSeconds)
        {
            foreach (SoundAgent agent in m_SoundAgents)
            {
                if (agent.SerialID == serialID)
                {
                    agent.Resume(fadeInSeconds);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 停止本组所有声音
        /// </summary>
        public void StopAllLoadedSounds()
        {
            foreach (SoundAgent agent in m_SoundAgents)
            {
                if (agent.IsPlaying)
                    agent.Stop();
            }
        }

        /// <summary>
        /// 停止本组所有声音（带淡出）
        /// </summary>
        public void StopAllLoadedSounds(float fadeOutSeconds)
        {
            foreach (SoundAgent agent in m_SoundAgents)
            {
                agent.Stop(fadeOutSeconds);
            }
        }

        /// <summary>
        /// 暂停本组所有声音
        /// </summary>
        public void PauseAllLoadedSounds(float fadeOutSeconds)
        {
            foreach (SoundAgent agent in m_SoundAgents)
            {
                agent.Pause(fadeOutSeconds);
            }
        }

        /// <summary>
        /// 恢复本组所有声音
        /// </summary>
        public void ResumeAllLoadedSounds(float fadeInSeconds)
        {
            foreach (SoundAgent agent in m_SoundAgents)
            {
                agent.Resume(fadeInSeconds);
            }
        }
    }
}