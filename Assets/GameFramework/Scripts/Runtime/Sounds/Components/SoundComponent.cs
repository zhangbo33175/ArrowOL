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
        protected override void Awake()
        {
            base.Awake();

            // 初始化音频管理器
            m_SoundManager = new SoundManager();
            if (m_SoundManager == null)
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
            for (int i = 0; i < m_SoundGroupShells.Length; i++)
            {
                if (!AddSoundGroup(
                        m_SoundGroupShells[i].Name,
                        m_SoundGroupShells[i].AvoidBeingReplacedBySamePriority,
                        m_SoundGroupShells[i].Mute,
                        m_SoundGroupShells[i].Volume,
                        m_SoundGroupShells[i].AgentCount))
                {
                    Log.Warning("添加 Sound Group '{0}' 失败。", m_SoundGroupShells[i].Name);
                    continue;
                }
            }
        }

        private void OnDestroy()
        {
        }

        #region 声音组管理

        /// <summary>
        /// 是否存在指定声音组
        /// </summary>
        public bool HasSoundGroup(string soundGroupName)
        {
            return m_SoundManager.HasSoundGroup(soundGroupName);
        }

        /// <summary>
        /// 获取指定声音组
        /// </summary>
        public SoundGroup GetSoundGroup(string soundGroupName)
        {
            return m_SoundManager.GetSoundGroup(soundGroupName);
        }

        /// <summary>
        /// 获取所有声音组
        /// </summary>
        public SoundGroup[] GetAllSoundGroups()
        {
            return m_SoundManager.GetAllSoundGroups();
        }

        public void GetAllSoundGroups(List<SoundGroup> results)
        {
            m_SoundManager.GetAllSoundGroups(results);
        }

        /// <summary>
        /// 添加声音组（最简重载）
        /// </summary>
        public bool AddSoundGroup(string soundGroupName, int soundAgentCount)
        {
            return AddSoundGroup(soundGroupName, false, SoundConstant.DefaultMute, SoundConstant.DefaultVolume,
                soundAgentCount);
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
            if (m_SoundManager.HasSoundGroup(soundGroupName))
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
                AudioMixerGroup[] audioMixerGroups =
                    m_AudioMixer.FindMatchingGroups(AorTxt.Format("Master/{0}", soundGroupName));
                soundGroupHelper.AudioMixerGroup = audioMixerGroups.Length > 0
                    ? audioMixerGroups[0]
                    : m_AudioMixer.FindMatchingGroups("Master")[0];
            }

            // 添加到管理器
            if (!m_SoundManager.AddSoundGroup(soundGroupName, soundGroupAvoidBeingReplacedBySamePriority,
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

        #region 加载状态查询

        /// <summary>
        /// 获取所有正在加载的声音ID
        /// </summary>
        public int[] GetAllLoadingSoundSerialIDs()
        {
            return m_SoundManager.GetAllLoadingSoundSerialIDs();
        }

        public void GetAllLoadingSoundSerialIDs(List<int> results)
        {
            m_SoundManager.GetAllLoadingSoundSerialIDs(results);
        }

        /// <summary>
        /// 检查声音是否正在加载
        /// </summary>
        public bool IsLoadingSound(int serialID)
        {
            return m_SoundManager.IsLoadingSound(serialID);
        }

        #endregion

        #region 播放声音

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
            PlaySoundParams playSoundParams = null, Vector3 worldPosition = default(Vector3))
        {
            return m_SoundManager.PlaySound(abPath, assetName, soundGroupName, playSoundParams,
                PlaySoundInfoShell.Create(worldPosition));
        }

        #endregion

        #region 停止声音

        /// <summary>
        /// 停止指定声音
        /// </summary>
        public bool StopSound(int serialID)
        {
            return m_SoundManager.StopSound(serialID);
        }

        /// <summary>
        /// 停止指定声音（带淡出）
        /// </summary>
        public bool StopSound(int serialID, float fadeOutSeconds)
        {
            return m_SoundManager.StopSound(serialID, fadeOutSeconds);
        }

        /// <summary>
        /// 停止所有已加载声音
        /// </summary>
        public void StopAllLoadedSounds()
        {
            m_SoundManager.StopAllLoadedSounds();
        }

        public void StopAllLoadedSounds(float fadeOutSeconds)
        {
            m_SoundManager.StopAllLoadedSounds(fadeOutSeconds);
        }

        /// <summary>
        /// 停止所有正在加载的声音
        /// </summary>
        public void StopAllLoadingSounds()
        {
            m_SoundManager.StopAllLoadingSounds();
        }

        #endregion

        #region 暂停 / 恢复

        /// <summary>
        /// 暂停声音
        /// </summary>
        public void PauseSound(int serialID)
        {
            m_SoundManager.PauseSound(serialID);
        }

        public void PauseSound(int serialID, float fadeOutSeconds)
        {
            m_SoundManager.PauseSound(serialID, fadeOutSeconds);
        }

        /// <summary>
        /// 恢复声音
        /// </summary>
        public bool ResumeSound(int serialID)
        {
            return m_SoundManager.ResumeSound(serialID);
        }

        public bool ResumeSound(int serialID, float fadeInSeconds)
        {
            return m_SoundManager.ResumeSound(serialID, fadeInSeconds);
        }

        #endregion

        #region 分组控制

        /// <summary>
        /// 暂停整个声音组
        /// </summary>
        public bool PauseGourpSound(string groupName)
        {
            return m_SoundManager.PauseGroupSound(groupName);
        }

        /// <summary>
        /// 恢复整个声音组
        /// </summary>
        public bool ResumeGroupSound(string groupName)
        {
            return m_SoundManager.ResumeGroupSound(groupName);
        }

        /// <summary>
        /// 停止整个声音组
        /// </summary>
        public bool StopGroupSound(string groupName)
        {
            return m_SoundManager.StopGroupSound(groupName);
        }

        #endregion

        #region 音量控制

        /// <summary>
        /// 设置指定组的全局音量
        /// </summary>
        public void SetAllSoundVolume(float newVolume, string groupName)
        {
            m_SoundManager.SetAllSoundVolume(newVolume, groupName);
        }

        #endregion
    }
}