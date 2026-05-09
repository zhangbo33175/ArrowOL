using System.Collections.Generic;
using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// 声音总管理器（音频系统顶层入口）
    /// 功能：管理所有声音组、处理资源加载、播放/暂停/停止调度、全局音量控制
    /// </summary>
    public sealed partial class SoundManager
    {
        /// <summary>
        /// 声音组字典（key=组名，value=声音组实例）
        /// </summary>
        public SoundManager()
        {
            m_SoundGroups = new Dictionary<string, SoundGroup>();
            m_SoundsLoading = new List<int>();
            m_SoundsToReleaseOnLoad = new HashSet<int>();
            m_Serial = 0;

            // 获取全局资源组件
            m_AssetComponent = GameComponentsGroup.GetComponent<AssetComponent>();
            if (m_AssetComponent == null)
            {
                Log.Fatal("Asset Component 无效。");
                return;
            }
        }

        /// <summary>
        /// 声音组数量
        /// </summary>
        public int SoundGroupCount
        {
            get { return m_SoundGroups.Count; }
        }

        /// <summary>
        /// 关闭音频系统，释放所有声音
        /// </summary>
        public void Shutdown()
        {
            StopAllLoadedSounds();
            m_SoundGroups.Clear();
            m_SoundsLoading.Clear();
            m_SoundsToReleaseOnLoad.Clear();
        }

        /// <summary>
        /// 是否存在指定声音组
        /// </summary>
        public bool HasSoundGroup(string soundGroupName)
        {
            if (string.IsNullOrEmpty(soundGroupName))
                throw new GameException("Sound group name 无效。");

            return m_SoundGroups.ContainsKey(soundGroupName);
        }

        /// <summary>
        /// 获取指定声音组
        /// </summary>
        public SoundGroup GetSoundGroup(string soundGroupName)
        {
            if (string.IsNullOrEmpty(soundGroupName))
                throw new GameException("Sound group name 无效。");

            m_SoundGroups.TryGetValue(soundGroupName, out SoundGroup soundGroup);
            return soundGroup;
        }

        /// <summary>
        /// 获取所有声音组
        /// </summary>
        public SoundGroup[] GetAllSoundGroups()
        {
            int index = 0;
            SoundGroup[] results = new SoundGroup[m_SoundGroups.Count];
            foreach (var pair in m_SoundGroups)
                results[index++] = pair.Value;

            return results;
        }

        /// <summary>
        /// 获取所有声音组（List 版本）
        /// </summary>
        public void GetAllSoundGroups(List<SoundGroup> results)
        {
            if (results == null)
                throw new GameException("Results 无效。");

            results.Clear();
            foreach (var pair in m_SoundGroups)
                results.Add(pair.Value);
        }

        /// <summary>
        /// 添加声音组（默认参数）
        /// </summary>
        public bool AddSoundGroup(string soundGroupName, SoundGroupHelper soundGroupHelper)
        {
            return AddSoundGroup(
                soundGroupName,
                false,
                SoundConstant.DefaultMute,
                SoundConstant.DefaultVolume,
                soundGroupHelper);
        }

        /// <summary>
        /// 添加声音组（完整参数）
        /// </summary>
        public bool AddSoundGroup(
            string soundGroupName,
            bool soundGroupAvoidBeingReplacedBySamePriority,
            bool soundGroupMute,
            float soundGroupVolume,
            SoundGroupHelper soundGroupHelper)
        {
            if (string.IsNullOrEmpty(soundGroupName))
                throw new GameException("Sound group name 无效。");
            if (soundGroupHelper == null)
                throw new GameException("Sound group helper 无效。");
            if (HasSoundGroup(soundGroupName))
                return false;

            SoundGroup soundGroup = new SoundGroup(soundGroupName, soundGroupHelper)
            {
                AvoidBeingReplacedBySamePriority = soundGroupAvoidBeingReplacedBySamePriority,
                Mute = soundGroupMute,
                Volume = soundGroupVolume
            };

            m_SoundGroups.Add(soundGroupName, soundGroup);
            return true;
        }

        /// <summary>
        /// 给指定声音组添加一个声音播放器（AudioSource）
        /// </summary>
        public void AddSoundAgent(string soundGroupName, SoundAgentHelper soundAgentHelper)
        {
            SoundGroup soundGroup = GetSoundGroup(soundGroupName);
            if (soundGroup == null)
                throw new GameException(AorTxt.Format("Sound group '{0}' 不存在。", soundGroupName));

            soundGroup.AddSoundAgent(this, soundAgentHelper);
        }

        /// <summary>
        /// 获取所有正在加载的声音ID
        /// </summary>
        public int[] GetAllLoadingSoundSerialIDs()
        {
            return m_SoundsLoading.ToArray();
        }

        /// <summary>
        /// 获取所有正在加载的声音ID（List 版本）
        /// </summary>
        public void GetAllLoadingSoundSerialIDs(List<int> results)
        {
            if (results == null)
                throw new GameException("Results 无效。");

            results.Clear();
            results.AddRange(m_SoundsLoading);
        }

        /// <summary>
        /// 某个声音是否正在加载中
        /// </summary>
        public bool IsLoadingSound(int serialID)
        {
            return m_SoundsLoading.Contains(serialID);
        }

        /// <summary>
        /// 【核心接口】播放声音（异步加载AB包 + 自动调度播放器）
        /// </summary>
        /// <returns>声音唯一ID，用于暂停/停止</returns>
        public int PlaySound(
            string abPath,
            string assetName,
            string soundGroupName,
            PlaySoundParams playSoundParams = null,
            PlaySoundInfoShell playSoundInfoShell = null)
        {
            if (playSoundParams == null)
                playSoundParams = PlaySoundParams.Create();

            // 生成唯一ID
            int serialID = ++m_Serial;
            PlaySoundErrorCode? errorCode = null;
            string errorMessage = null;

            SoundGroup soundGroup = GetSoundGroup(soundGroupName);
            if (soundGroup == null)
            {
                errorCode = PlaySoundErrorCode.SoundGroupNotExist;
                errorMessage = AorTxt.Format("Sound group '{0}' 不存在，errorCode = '{1}'", soundGroupName, errorCode);
            }
            else if (soundGroup.SoundAgentCount <= 0)
            {
                errorCode = PlaySoundErrorCode.SoundGroupHasNotEnoughAgent;
                errorMessage = AorTxt.Format("Sound group '{0}' 没有足够的 sound agent，errorCode = '{1}'", soundGroupName, errorCode);
            }

            if (errorCode.HasValue)
                Log.Warning(errorMessage);

            // 标记为加载中
            m_SoundsLoading.Add(serialID);

            // 包装播放信息
            PlaySoundInfo playSoundInfo = PlaySoundInfo.Create(serialID, soundGroup, playSoundParams, playSoundInfoShell);

            // 异步加载音频资源
            m_AssetComponent.LoadAssetAsync("Sound", abPath, assetName, (AssetObject assetObject, Object soundAsset) =>
            {
                // 如果加载过程中被标记为释放，直接卸载
                if (m_SoundsToReleaseOnLoad.Contains(playSoundInfo.SerialID))
                {
                    m_SoundsToReleaseOnLoad.Remove(playSoundInfo.SerialID);
                    ReleaseSoundAsset(soundAsset);
                    return;
                }

                // 加载完成，移除加载标记
                m_SoundsLoading.Remove(playSoundInfo.SerialID);

                // 交给声音组播放
                SoundAgent soundAgent = playSoundInfo.SoundGroup.PlaySound(
                    playSoundInfo.SerialID,
                    soundAsset,
                    playSoundInfo.PlaySoundParams,
                    out errorCode);

                // 播放失败 → 释放资源
                if (soundAgent == null)
                {
                    ReleaseSoundAsset(soundAsset);
                    errorMessage = AorTxt.Format("Sound group '{0}' 播放声音 '{1}' 失败，errorCode = '{2}'",
                        playSoundInfo.SoundGroup.Name, assetName, errorCode);
                    Log.Warning(errorMessage);
                }
            });

            return serialID;
        }

        /// <summary>
        /// 停止声音（默认淡出）
        /// </summary>
        public bool StopSound(int serialID)
        {
            return StopSound(serialID, SoundConstant.DefaultFadeOutSeconds);
        }

        /// <summary>
        /// 停止声音（支持淡出）
        /// </summary>
        public bool StopSound(int serialID, float fadeOutSeconds)
        {
            // 如果正在加载，标记加载完成后自动释放
            if (IsLoadingSound(serialID))
            {
                m_SoundsToReleaseOnLoad.Add(serialID);
                m_SoundsLoading.Remove(serialID);
                return true;
            }

            // 遍历所有组停止声音
            foreach (var pair in m_SoundGroups)
            {
                if (pair.Value.StopSound(serialID, fadeOutSeconds))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 停止所有已加载声音
        /// </summary>
        public void StopAllLoadedSounds()
        {
            StopAllLoadedSounds(SoundConstant.DefaultFadeOutSeconds);
        }

        /// <summary>
        /// 停止所有已加载声音（带淡出）
        /// </summary>
        public void StopAllLoadedSounds(float fadeOutSeconds)
        {
            foreach (var pair in m_SoundGroups)
                pair.Value.StopAllLoadedSounds(fadeOutSeconds);
        }

        /// <summary>
        /// 停止所有正在加载的声音
        /// </summary>
        public void StopAllLoadingSounds()
        {
            foreach (int serialID in m_SoundsLoading)
                m_SoundsToReleaseOnLoad.Add(serialID);
        }

        /// <summary>
        /// 暂停声音（默认淡出）
        /// </summary>
        public void PauseSound(int serialID)
        {
            PauseSound(serialID, SoundConstant.DefaultFadeOutSeconds);
        }

        /// <summary>
        /// 暂停声音（支持淡出）
        /// </summary>
        public void PauseSound(int serialID, float fadeOutSeconds)
        {
            foreach (var pair in m_SoundGroups)
            {
                if (pair.Value.PauseSound(serialID, fadeOutSeconds))
                    return;
            }

            throw new GameException(AorTxt.Format("无法找到 Sound '{0}'。", serialID));
        }

        /// <summary>
        /// 暂停整个声音组
        /// </summary>
        public bool PauseGroupSound(string groupName)
        {
            foreach (var pair in m_SoundGroups)
            {
                if (pair.Value.Name == groupName)
                {
                    Log.Info($"暂停整个声音组的音乐播放  name = {groupName}");
                    pair.Value.PauseAllLoadedSounds(SoundConstant.DefaultFadeOutSeconds);
                    return true;
                }
            }

            Log.Error($"暂停整个声音组的音乐播放 未找到要设置的音乐组  name = {groupName}");
            return false;
        }

        /// <summary>
        /// 恢复声音（默认淡入）
        /// </summary>
        public bool ResumeSound(int serialID)
        {
            return ResumeSound(serialID, SoundConstant.DefaultFadeInSeconds);
        }

        /// <summary>
        /// 恢复声音（支持淡入）
        /// </summary>
        public bool ResumeSound(int serialID, float fadeInSeconds)
        {
            foreach (var pair in m_SoundGroups)
            {
                if (pair.Value.ResumeSound(serialID, fadeInSeconds))
                    return true;
            }

            Log.Warning(AorTxt.Format("无法找到 Sound '{0}'。", serialID));
            return false;
        }

        /// <summary>
        /// 恢复整个声音组
        /// </summary>
        public bool ResumeGroupSound(string groupName)
        {
            foreach (var pair in m_SoundGroups)
            {
                if (pair.Value.Name == groupName)
                {
                    Log.Info($"恢复声音组音乐播放  name = {groupName}");
                    pair.Value.ResumeAllLoadedSounds(SoundConstant.DefaultFadeInSeconds);
                    return true;
                }
            }

            Log.Error($"恢复声音组音乐播放 未找到要设置的音乐组  name = {groupName}");
            return false;
        }

        /// <summary>
        /// 停止整个声音组
        /// </summary>
        public bool StopGroupSound(string groupName)
        {
            foreach (var pair in m_SoundGroups)
            {
                if (pair.Value.Name == groupName)
                {
                    Log.Info($"停止声音组音乐播放  name = {groupName}");
                    pair.Value.StopAllLoadedSounds(SoundConstant.DefaultFadeOutSeconds);
                    return true;
                }
            }

            Log.Error($"停止声音组音乐播放 未找到要设置的音乐组  name = {groupName}");
            return false;
        }

        /// <summary>
        /// 释放音频资源（交给资源组件卸载）
        /// </summary>
        public void ReleaseSoundAsset(Object soundAsset)
        {
            m_AssetComponent.UnloadAsset(soundAsset);
        }

        /// <summary>
        /// 设置指定声音组的全局音量
        /// </summary>
        public void SetAllSoundVolume(float newVolume, string groupName)
        {
            foreach (var pair in m_SoundGroups)
            {
                if (pair.Value.Name == groupName)
                {
                    Log.Info($"设置音乐组音量  name = {groupName}");
                    pair.Value.Volume = newVolume;
                    return;
                }
            }

            Log.Error($"设置音乐组音量 未找到要设置的音乐组  name = {groupName}");
        }
    }
}