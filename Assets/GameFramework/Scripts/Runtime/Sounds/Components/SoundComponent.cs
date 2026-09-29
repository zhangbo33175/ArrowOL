/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * -------------------------------------------------------------
 * filename:  SoundComponent.cs
 * author:  云毅
 * created:
 * descrip:   音频管理组件 - 游戏全局声音总入口（分组/播放/暂停/停止/淡入淡出/Lua）
 ***************************************************************/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using XLua;

namespace Honor.Runtime
{
    /// <summary>
    /// 音频管理组件（游戏全局声音总入口）
    /// 功能：声音分组管理、播放/暂停/停止/淡入淡出、音量控制、Lua调用接口
    /// 外部统一通过 GameMainRoot.Sound 访问
    /// </summary>
    [DisallowMultipleComponent]
    public sealed partial class SoundComponent : GameComponent
    {
        //=========================================================================
        #region 生命周期
        //=========================================================================

        protected override void Awake()
        {
            base.Awake();

            // 初始化音频管理器
            m_AudioService = new SoundManager();
            if (m_AudioService == null)
            {
                Log.Fatal("Sound manager 无效。");
                return;
            }

            // 添加 AudioListener（必须存在才能听到声音）
            m_AudioListener = gameObject.GetOrAddComponent<AudioListener>();
        }

        private void Start()
        {
            // 初始化配置的所有声音组
            foreach (SoundGroupShell shell in m_SoundGroupShells)
            {
                if (AddSoundGroup(
                        shell.Name,
                        shell.AvoidBeingReplacedBySamePriority,
                        shell.Mute,
                        shell.Volume,
                        shell.AgentCount))
                {
                    continue;
                }

                Log.Warning("添加 Sound Group '{0}' 失败。", shell.Name);
            }
        }

        private void OnDestroy()
        {
        }

        #endregion

        //=========================================================================
        #region 声音组管理
        //=========================================================================

        /// <summary>
        /// 是否存在指定声音组
        /// </summary>
        public bool HasSoundGroup(string soundGroupName)
        {
            return m_AudioService.HasSoundGroup(soundGroupName);
        }

        /// <summary>
        /// 获取指定声音组
        /// </summary>
        public SoundGroup GetSoundGroup(string soundGroupName)
        {
            return m_AudioService.GetSoundGroup(soundGroupName);
        }

        /// <summary>
        /// 获取所有声音组
        /// </summary>
        public SoundGroup[] GetAllSoundGroups()
        {
            return m_AudioService.GetAllSoundGroups();
        }

        /// <summary>
        /// 获取所有声音组（填充到外部 List，避免分配数组）
        /// </summary>
        /// <param name="results">结果列表（会被清空后填充）</param>
        public void GetAllSoundGroups(List<SoundGroup> results)
        {
            m_AudioService.GetAllSoundGroups(results);
        }

        /// <summary>
        /// 添加声音组（最简重载）
        /// </summary>
        public bool AddSoundGroup(string soundGroupName, int soundAgentCount)
        {
            return AddSoundGroup(soundGroupName, false, SoundConstant.DefaultMute, SoundConstant.DefaultVolume, soundAgentCount);
        }

        /// <summary>
        /// 添加声音组（完整参数）
        /// </summary>
        public bool AddSoundGroup(
            string soundGroupName,
            bool soundGroupAvoidBeingReplacedBySamePriority,
            bool soundGroupMute,
            float soundGroupVolume,
            int soundAgentCount)
        {
            // 防重复添加
            if (m_AudioService.HasSoundGroup(soundGroupName))
                return false;

            // 创建声音组节点
            SoundGroupHelper soundGroupHelper = new GameObject(AorTxt.Format("SoundGroup - {0}", soundGroupName))
                .AddComponent<SoundGroupHelper>();
            if (soundGroupHelper == null)
            {
                Log.Error("创建 Sound group helper 失败。");
                return false;
            }

            soundGroupHelper.transform.SetParent(transform);

            // 绑定 AudioMixer 分组
            if (m_AudioMixer != null)
            {
                AudioMixerGroup masterGroup = m_AudioMixer.FindMatchingGroups("Master")[0];
                soundGroupHelper.AudioMixerGroup = ResolveMixerGroup(
                    AorTxt.Format("Master/{0}", soundGroupName), masterGroup);
            }

            // 添加到管理器
            if (!m_AudioService.AddSoundGroup(soundGroupName, soundGroupAvoidBeingReplacedBySamePriority,
                    soundGroupMute, soundGroupVolume, soundGroupHelper))
                return false;

            // 创建声音播放器（AudioSource）
            for (int i = 1; i <= soundAgentCount; i++)
            {
                if (!AddSoundAgent(soundGroupName, soundGroupHelper, i))
                    return false;
            }

            return true;
        }

        #endregion

        //=========================================================================
        #region 加载状态查询
        //=========================================================================

        /// <summary>
        /// 获取所有正在加载的声音ID
        /// </summary>
        public int[] GetAllLoadingSoundSerialIDs()
        {
            return m_AudioService.GetAllLoadingSoundSerialIDs();
        }

        /// <summary>
        /// 获取所有正在加载的声音ID（填充到外部 List）
        /// </summary>
        /// <param name="results">结果列表（会被清空后填充）</param>
        public void GetAllLoadingSoundSerialIDs(List<int> results)
        {
            m_AudioService.GetAllLoadingSoundSerialIDs(results);
        }

        /// <summary>
        /// 检查声音是否正在加载
        /// </summary>
        public bool IsLoadingSound(int serialID)
        {
            return m_AudioService.IsLoadingSound(serialID);
        }

        #endregion

        //=========================================================================
        #region 播放声音
        //=========================================================================

        /// <summary>
        /// Lua 调用播放声音（自动解析参数表）
        /// </summary>
        public int PlaySound(LuaTable luaTable)
        {
            if (luaTable == null)
            {
                Log.Error("SoundComponent.PlaySound luaTable 无效。");
                return 0;
            }

            // 从Lua表读取参数
            luaTable.Get("ABPath", out string abPath);
            luaTable.Get("AssetName", out string assetName);
            luaTable.Get("GroupName", out string groupName);
            luaTable.Get("Priority", out int priority);
            luaTable.Get("Loop", out bool loop);
            luaTable.Get("Volume", out float volume);
            luaTable.Get("SpatialBlend", out float spatialBlend);
            luaTable.Get("MaxDistance", out float maxDistance);
            luaTable.Get("WorldPosition", out Vector3 worldPosition);
            luaTable.Get("Speed", out float speed);

            // 构建播放参数
            PlaySoundParams playSoundParams = PlaySoundParams.Create();
            playSoundParams.Priority = priority;
            playSoundParams.Loop = loop;
            playSoundParams.VolumeInSoundGroup = volume;
            playSoundParams.SpatialBlend = spatialBlend;
            playSoundParams.MaxDistance = maxDistance;
            playSoundParams.Pitch = speed;

            return PlaySound(abPath, assetName, groupName, playSoundParams, worldPosition);
        }

        /// <summary>
        /// 播放声音（C# 标准接口）
        /// </summary>
        public int PlaySound(string abPath, string assetName, string soundGroupName,
            PlaySoundParams playSoundParams = null, Vector3 worldPosition = default)
        {
            return m_AudioService.PlaySound(abPath, assetName, soundGroupName, playSoundParams,
                PlaySoundInfoShell.Create(worldPosition));
        }

        #endregion

        //=========================================================================
        #region 停止声音
        //=========================================================================

        /// <summary>
        /// 停止指定声音
        /// </summary>
        public bool StopSound(int serialID)
        {
            return m_AudioService.StopSound(serialID);
        }

        /// <summary>
        /// 停止指定声音（带淡出）
        /// </summary>
        public bool StopSound(int serialID, float fadeOutSeconds)
        {
            return m_AudioService.StopSound(serialID, fadeOutSeconds);
        }

        /// <summary>
        /// 停止所有已加载声音
        /// </summary>
        public void StopAllLoadedSounds()
        {
            m_AudioService.StopAllLoadedSounds();
        }

        /// <summary>
        /// 停止所有已加载声音（带淡出）
        /// </summary>
        /// <param name="fadeOutSeconds">淡出时间（秒）</param>
        public void StopAllLoadedSounds(float fadeOutSeconds)
        {
            m_AudioService.StopAllLoadedSounds(fadeOutSeconds);
        }

        /// <summary>
        /// 停止所有正在加载的声音
        /// </summary>
        public void StopAllLoadingSounds()
        {
            m_AudioService.StopAllLoadingSounds();
        }

        #endregion

        //=========================================================================
        #region 暂停 / 恢复
        //=========================================================================

        /// <summary>
        /// 暂停声音
        /// </summary>
        public void PauseSound(int serialID)
        {
            m_AudioService.PauseSound(serialID);
        }

        /// <summary>
        /// 暂停声音（带淡出）
        /// </summary>
        /// <param name="serialID">声音唯一ID</param>
        /// <param name="fadeOutSeconds">淡出时间（秒）</param>
        public void PauseSound(int serialID, float fadeOutSeconds)
        {
            m_AudioService.PauseSound(serialID, fadeOutSeconds);
        }

        /// <summary>
        /// 恢复声音
        /// </summary>
        public bool ResumeSound(int serialID)
        {
            return m_AudioService.ResumeSound(serialID);
        }

        /// <summary>
        /// 恢复声音（带淡入）
        /// </summary>
        /// <param name="serialID">声音唯一ID</param>
        /// <param name="fadeInSeconds">淡入时间（秒）</param>
        public bool ResumeSound(int serialID, float fadeInSeconds)
        {
            return m_AudioService.ResumeSound(serialID, fadeInSeconds);
        }

        #endregion

        //=========================================================================
        #region 分组控制
        //=========================================================================

        /// <summary>
        /// 暂停整个声音组
        /// </summary>
        public bool PauseGourpSound(string groupName)
        {
            return m_AudioService.PauseGroupSound(groupName);
        }

        /// <summary>
        /// 恢复整个声音组
        /// </summary>
        public bool ResumeGroupSound(string groupName)
        {
            return m_AudioService.ResumeGroupSound(groupName);
        }

        /// <summary>
        /// 停止整个声音组
        /// </summary>
        public bool StopGroupSound(string groupName)
        {
            return m_AudioService.StopGroupSound(groupName);
        }

        #endregion

        //=========================================================================
        #region 音量控制
        //=========================================================================

        /// <summary>
        /// 设置指定组的全局音量
        /// </summary>
        public void SetAllSoundVolume(float newVolume, string groupName)
        {
            m_AudioService.SetAllSoundVolume(newVolume, groupName);
        }

        #endregion
    }
}
