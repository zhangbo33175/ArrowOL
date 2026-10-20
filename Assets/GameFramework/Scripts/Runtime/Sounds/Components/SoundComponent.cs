/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  SoundComponent.cs
 * author:    云毅
 * created:   2026
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
    public sealed partial class SoundComponent : Singleton<SoundComponent>
    {
        //=========================================================================
        #region 生命周期
        //=========================================================================

        public void Init()
        {
            m_AudioService = new SoundManager();
            if (m_AudioService == null) { Log.Fatal("Sound manager 无效。"); return; }

            m_SoundGo = new GameObject("[SoundComponent]");
            Object.DontDestroyOnLoad(m_SoundGo);
            m_AudioListener = m_SoundGo.GetOrAddComponent<AudioListener>();

            // 确保场景中只有一个 AudioListener 生效：
            // 若场景 Camera 已自带 AudioListener，则禁用自身监听器（避免多个监听器
            // 冲突导致无声，恢复与 GameCatDevelopment 一致的有声行为）；
            // 若场景无监听器，则由本组件提供唯一的监听器。
            RefreshAudioListener();

            // 从音频配置资产读取声音组与混音器；资产缺失/未配置时回退到默认硬编码
            InitializeFromSoundConfig();
        }

        /// <summary>
        /// 根据 SoundConfig 资产初始化声音组与混音器
        /// <para>优先读取 SoundConfig.asset（AudioMixer + 声音组预设），资产缺失或未配置时
        /// 回退到默认声音组（Music / UISound），保证旧工程行为不变、运行稳健。</para>
        /// </summary>
        private void InitializeFromSoundConfig()
        {
            SoundConfig soundConfig = LoadSoundConfigAsset();
            if (soundConfig == null)
            {
                // 默认声音组（未配置音频资产时的回退）
                Log.Info("[SoundComponent] SoundConfig 资产未加载，回退默认声音组 Music/UISound。");
                AddSoundGroup("Music", 4);
                AddSoundGroup("UISound", 2);
                return;
            }

            // 应用混音器（供声音组轨道绑定使用）
            // 诊断确认：播放触发、AudioListener 唯一且 enabled、无失败警告，但绑定 AudioMixer 后无声。
            // 参照工程 GameCatDevelopment 绑 mixer 有声，本工程 AudioMixer 运行时输出链路疑似异常。
            // 故恢复运行时默认不绑定 AudioMixer（与改动前有声行为一致）；配置仍保存/读取。
            // 若需启用 AudioMixer 混音效果，请先在 Unity 的 Audio Mixer 面板确认 Master 组 Output 已连 AudioListener。
            // m_AudioMixer = soundConfig.AudioMixer;
            Log.Info(string.Format("[SoundComponent] 加载 SoundConfig 成功，AudioMixer={0}（运行时未绑定，保持有声）。",
                soundConfig.AudioMixer != null ? soundConfig.AudioMixer.name : "null"));

            // 按资产预设创建声音组；空配置时回退默认
            SoundGroupShell[] shells = soundConfig.SoundGroupShells;
            if (shells == null || shells.Length == 0)
            {
                Log.Info("[SoundComponent] SoundConfig 声音分组为空，回退默认声音组 Music/UISound。");
                AddSoundGroup("Music", 4);
                AddSoundGroup("UISound", 2);
                return;
            }

            Log.Info(string.Format("[SoundComponent] 按 SoundConfig 创建声音组，共 {0} 个预设。", shells.Length));
            for (int i = 0; i < shells.Length; i++)
            {
                SoundGroupShell shell = shells[i];
                if (shell == null || string.IsNullOrEmpty(shell.Name))
                {
                    continue; // 跳过未配置/空名的预设项，保证健壮性
                }

                bool ok = AddSoundGroup(
                    shell.Name,
                    shell.AvoidBeingReplacedBySamePriority,
                    shell.Mute,
                    shell.Volume,
                    shell.AgentCount);
                Log.Info(string.Format("[SoundComponent] 创建声音组 {0} (Agent={1}) => {2}。",
                    shell.Name, shell.AgentCount, ok ? "成功" : "失败"));
            }
        }

        /// <summary>
        /// 加载音频配置资产
        /// <para>统一走 <see cref="SoundConfig.Instance"/>（带缓存），资产未生成时返回 null，
        /// 调用方回退默认值。</para>
        /// </summary>
        /// <returns>音频配置资产；未找到返回 null</returns>
        private static SoundConfig LoadSoundConfigAsset()
        {
            return SoundConfig.Instance;
        }

        public void Cleanup()
        {
            m_AudioService = null;
            if (m_SoundGo != null) Object.Destroy(m_SoundGo);
            m_SoundGo = null;
        }

        public override void Dispose() { Cleanup(); }

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
            return AddSoundGroup(soundGroupName, false, SoundConstant.InitialMute, SoundConstant.InitialVolume, soundAgentCount);
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

            soundGroupHelper.transform.SetParent(m_SoundGo.transform);

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