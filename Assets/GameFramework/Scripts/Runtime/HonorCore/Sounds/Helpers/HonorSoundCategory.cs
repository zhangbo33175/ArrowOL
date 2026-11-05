/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * -------------------------------------------------------------
 * filename:  HonorSoundCategory.cs
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
    public sealed class HonorSoundCategory
    {
        /// <summary>
        /// 声音组名称（如 BGM、Effect、UI）
        /// </summary>
        private readonly string m_Name;

        /// <summary>
        /// 声音组辅助对象（挂载在 GameObject 上）
        /// </summary>
        private readonly HonorSoundCategoryHelper m_HonorSoundCategoryHelper;

        /// <summary>
        /// 本组所有声音播放器列表（对象池）
        /// </summary>
        private readonly List<HonorSoundPlayer> m_SoundAgents;

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
        public HonorSoundCategoryHelper HonorSoundCategoryHelper => m_HonorSoundCategoryHelper;

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
                foreach (HonorSoundPlayer agent in m_SoundAgents)
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
                foreach (HonorSoundPlayer agent in m_SoundAgents)
                {
                    agent.RefreshVolume();
                }
            }
        }

        /// <summary>
        /// 构造：创建声音组
        /// </summary>
        public HonorSoundCategory(string name, HonorSoundCategoryHelper HonorSoundCategoryHelper)
        {
            if (string.IsNullOrEmpty(name))
                throw new GameException("Sound group name 无效。");
            if (HonorSoundCategoryHelper == null)
                throw new GameException("Sound group helper 无效。");

            m_Name = name;
            m_HonorSoundCategoryHelper = HonorSoundCategoryHelper;
            m_SoundAgents = new List<HonorSoundPlayer>();
        }

        /// <summary>
        /// 添加一个声音播放器到本组
        /// </summary>
        public void AddSoundAgent(HonorSoundManager HonorSoundManager, HonorSoundPlayerHelper HonorSoundPlayerHelper)
        {
            m_SoundAgents.Add(new HonorSoundPlayer(this, HonorSoundManager, HonorSoundPlayerHelper));
        }

        /// <summary>
        /// 核心方法：播放声音（自动选择空闲/可淘汰播放器）
        /// </summary>
        public HonorSoundPlayer PlaySound(int serialID, Object soundAsset, HonorPlaySoundParams HonorPlaySoundParams, out HonorSoundErrorCode? errorCode)
        {
            errorCode = null;
            int busyCount = 0;

            // 遍历所有播放器，找最合适的
            HonorSoundPlayer candidateAgent = SelectCandidateAgent(HonorPlaySoundParams, ref busyCount);

            // 没有可用播放器
            if (candidateAgent == null)
            {
                errorCode = busyCount == m_SoundAgents.Count
                    ? HonorSoundErrorCode.SoundGroupInsufficientAgents
                    : HonorSoundErrorCode.IgnoredDueToLowPriority;
                return null;
            }

            // 设置音频资源失败
            if (!candidateAgent.SetSoundAsset(soundAsset))
            {
                errorCode = HonorSoundErrorCode.SetSoundAssetFailure;
                return null;
            }

            // 赋值播放参数
            ApplyPlaySoundParams(candidateAgent, serialID, HonorPlaySoundParams);

            // 开始播放
            candidateAgent.Play(HonorPlaySoundParams.FadeInSeconds);

            return candidateAgent;
        }

        /// <summary>
        /// 按"空闲优先→低优先级可顶替→同优先级最早播放"策略挑选候选播放器
        /// </summary>
        /// <param name="HonorPlaySoundParams">播放参数</param>
        /// <param name="busyCount">忙碌播放器计数输出</param>
        /// <returns>选中的候选播放器</returns>
        private HonorSoundPlayer SelectCandidateAgent(HonorPlaySoundParams HonorPlaySoundParams, ref int busyCount)
        {
            HonorSoundPlayer candidateAgent = null;

            // 遍历所有播放器，找最合适的
            foreach (HonorSoundPlayer agent in m_SoundAgents)
            {
                // 1. 优先用空闲播放器
                if (!agent.IsPlaying)
                {
                    candidateAgent = agent;
                    break;
                }

                busyCount++;

                // 2. 找优先级更低的，可以被顶替
                if (agent.Priority < HonorPlaySoundParams.Priority)
                {
                    if (candidateAgent == null || agent.Priority < candidateAgent.Priority)
                        candidateAgent = agent;
                }
                // 3. 同优先级，允许顶替最早播放的
                else if (!m_AvoidBeingReplacedBySamePriority && agent.Priority == HonorPlaySoundParams.Priority)
                {
                    if (candidateAgent == null || agent.SetSoundAssetTime < candidateAgent.SetSoundAssetTime)
                        candidateAgent = agent;
                }
            }

            return candidateAgent;
        }

        /// <summary>
        /// 将播放参数赋值到候选播放器
        /// </summary>
        /// <param name="candidateAgent">候选播放器</param>
        /// <param name="serialID">声音序列ID</param>
        /// <param name="HonorPlaySoundParams">播放参数</param>
        private void ApplyPlaySoundParams(HonorSoundPlayer candidateAgent, int serialID, HonorPlaySoundParams HonorPlaySoundParams)
        {
            // 赋值播放参数
            candidateAgent.SerialID = serialID;
            candidateAgent.Time = HonorPlaySoundParams.Time;
            candidateAgent.MuteInSoundGroup = HonorPlaySoundParams.MuteInSoundGroup;
            candidateAgent.Loop = HonorPlaySoundParams.Loop;
            candidateAgent.Priority = HonorPlaySoundParams.Priority;
            candidateAgent.VolumeInSoundGroup = HonorPlaySoundParams.VolumeInSoundGroup;
            candidateAgent.Pitch = HonorPlaySoundParams.Pitch;
            candidateAgent.PanStereo = HonorPlaySoundParams.PanStereo;
            candidateAgent.SpatialBlend = HonorPlaySoundParams.SpatialBlend;
            candidateAgent.MaxDistance = HonorPlaySoundParams.MaxDistance;
            candidateAgent.DopplerLevel = HonorPlaySoundParams.DopplerLevel;
        }

        /// <summary>
        /// 停止指定声音
        /// </summary>
        public bool StopSound(int serialID, float fadeOutSeconds)
        {
            foreach (HonorSoundPlayer agent in m_SoundAgents)
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
            foreach (HonorSoundPlayer agent in m_SoundAgents)
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
            foreach (HonorSoundPlayer agent in m_SoundAgents)
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
            foreach (HonorSoundPlayer agent in m_SoundAgents)
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
            foreach (HonorSoundPlayer agent in m_SoundAgents)
            {
                agent.Stop(fadeOutSeconds);
            }
        }

        /// <summary>
        /// 暂停本组所有声音
        /// </summary>
        public void PauseAllLoadedSounds(float fadeOutSeconds)
        {
            foreach (HonorSoundPlayer agent in m_SoundAgents)
            {
                agent.Pause(fadeOutSeconds);
            }
        }

        /// <summary>
        /// 恢复本组所有声音
        /// </summary>
        public void ResumeAllLoadedSounds(float fadeInSeconds)
        {
            foreach (HonorSoundPlayer agent in m_SoundAgents)
            {
                agent.Resume(fadeInSeconds);
            }
        }
    }
}