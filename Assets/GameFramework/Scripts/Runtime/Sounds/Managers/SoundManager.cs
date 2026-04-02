using System.Collections.Generic;
using UnityEngine;

namespace Honor.Runtime
{
    public sealed partial class SoundManager
    {
        /// <summary>
        /// 初始化声音管理器的新实例
        /// </summary>
        public SoundManager()
        {
            m_SoundGroups = new Dictionary<string, SoundGroup>();
            m_SoundsLoading = new List<int>();
            m_SoundsToReleaseOnLoad = new HashSet<int>();
            m_Serial = 0;

            m_AssetComponent = GameComponentsGroup.GetComponent<AssetComponent>();
            if (m_AssetComponent == null)
            {
                Log.Fatal("Asset Component 无效。");
                return;
            }

        }

        /// <summary>
        /// 获取声音组数量
        /// </summary>
        public int SoundGroupCount
        {
            get
            {
                return m_SoundGroups.Count;
            }
        }

        /// <summary>
        /// 关闭并清理声音管理器
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
        /// <param name="soundGroupName">声音组名称。</param>
        /// <returns>指定声音组是否存在。</returns>
        public bool HasSoundGroup(string soundGroupName)
        {
            if (string.IsNullOrEmpty(soundGroupName))
            {
                throw new GameException("Sound group name 无效。");
            }

            return m_SoundGroups.ContainsKey(soundGroupName);
        }

        /// <summary>
        /// 获取指定声音组
        /// </summary>
        /// <param name="soundGroupName">声音组名称。</param>
        /// <returns>要获取的声音组。</returns>
        public SoundGroup GetSoundGroup(string soundGroupName)
        {
            if (string.IsNullOrEmpty(soundGroupName))
            {
                throw new GameException("Sound group name 无效。");
            }

            SoundGroup soundGroup = null;
            if (m_SoundGroups.TryGetValue(soundGroupName, out soundGroup))
            {
                return soundGroup;
            }

            return null;
        }

        /// <summary>
        /// 获取所有声音组
        /// </summary>
        /// <returns>所有声音组。</returns>
        public SoundGroup[] GetAllSoundGroups()
        {
            int index = 0;
            SoundGroup[] results = new SoundGroup[m_SoundGroups.Count];
            foreach (KeyValuePair<string, SoundGroup> soundGroup in m_SoundGroups)
            {
                results[index++] = soundGroup.Value;
            }

            return results;
        }

        /// <summary>
        /// 获取所有声音组
        /// </summary>
        /// <param name="results">所有声音组。</param>
        public void GetAllSoundGroups(List<SoundGroup> results)
        {
            if (results == null)
            {
                throw new GameException("Results 无效。");
            }

            results.Clear();
            foreach (KeyValuePair<string, SoundGroup> soundGroup in m_SoundGroups)
            {
                results.Add(soundGroup.Value);
            }
        }

        /// <summary>
        /// 增加声音组
        /// </summary>
        /// <param name="soundGroupName">声音组名称。</param>
        /// <param name="soundGroupHelper">声音组辅助器。</param>
        /// <returns>是否增加声音组成功。</returns>
        public bool AddSoundGroup(string soundGroupName, SoundGroupHelper soundGroupHelper)
        {
            return AddSoundGroup(soundGroupName, false, SoundConstant.DefaultMute, SoundConstant.DefaultVolume, soundGroupHelper);
        }

        /// <summary>
        /// 增加声音组
        /// </summary>
        /// <param name="soundGroupName">声音组名称。</param>
        /// <param name="soundGroupAvoidBeingReplacedBySamePriority">声音组中的声音是否避免被同优先级声音替换。</param>
        /// <param name="soundGroupMute">声音组是否静音。</param>
        /// <param name="soundGroupVolume">声音组音量。</param>
        /// <param name="soundGroupHelper">声音组辅助器。</param>
        /// <returns>是否增加声音组成功。</returns>
        public bool AddSoundGroup(string soundGroupName, bool soundGroupAvoidBeingReplacedBySamePriority, bool soundGroupMute, float soundGroupVolume, SoundGroupHelper soundGroupHelper)
        {
            if (string.IsNullOrEmpty(soundGroupName))
            {
                throw new GameException("Sound group name 无效。");
            }

            if (soundGroupHelper == null)
            {
                throw new GameException("Sound group helper 无效。");
            }

            if (HasSoundGroup(soundGroupName))
            {
                return false;
            }

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
        /// 增加声音代理
        /// </summary>
        /// <param name="soundGroupName">声音组名称。</param>
        /// <param name="soundAgentHelper">要增加的声音代理辅助器。</param>
        public void AddSoundAgent(string soundGroupName, SoundAgentHelper soundAgentHelper)
        {
            SoundGroup soundGroup = GetSoundGroup(soundGroupName);
            if (soundGroup == null)
            {
                throw new GameException(AorTxt.Format("Sound group '{0}' 不存在。", soundGroupName));
            }

            soundGroup.AddSoundAgent(this, soundAgentHelper);
        }

        /// <summary>
        /// 获取所有正在加载声音的序列编号
        /// </summary>
        /// <returns>所有正在加载声音的序列编号。</returns>
        public int[] GetAllLoadingSoundSerialIDs()
        {
            return m_SoundsLoading.ToArray();
        }

        /// <summary>
        /// 获取所有正在加载声音的序列编号
        /// </summary>
        /// <param name="results">所有正在加载声音的序列编号。</param>
        public void GetAllLoadingSoundSerialIDs(List<int> results)
        {
            if (results == null)
            {
                throw new GameException("Results 无效。");
            }

            results.Clear();
            results.AddRange(m_SoundsLoading);
        }

        /// <summary>
        /// 是否正在加载声音
        /// </summary>
        /// <param name="serialID">声音序列编号。</param>
        /// <returns>是否正在加载声音。</returns>
        public bool IsLoadingSound(int serialID)
        {
            return m_SoundsLoading.Contains(serialID);
        }

        /// <summary>
        /// 播放声音
        /// </summary>
        /// <param name="abPath">ab资源路径</param>
        /// <param name="assetName">asset资源名称</param>
        /// <param name="soundGroupName">声音组名称。</param>
        /// <param name="playSoundParams">播放声音参数。</param>
        /// <returns>声音的序列编号。</returns>
        public int PlaySound(string abPath, string assetName, string soundGroupName, PlaySoundParams playSoundParams = null, PlaySoundInfoShell playSoundInfoShell = null)
        {
            if (playSoundParams == null)
            {
                playSoundParams = PlaySoundParams.Create();
            }

            int serialID = ++m_Serial;
            PlaySoundErrorCode? errorCode = null;
            string errorMessage = null;
            SoundGroup soundGroup = GetSoundGroup(soundGroupName);
            if (soundGroup == null)
            {
                errorCode = PlaySoundErrorCode.SoundGroupNotExist;
                errorMessage = AorTxt.Format("Sound group '{0}' 不存在，errorCode = '{1}'", soundGroupName, errorCode.ToString());
            }
            else if (soundGroup.SoundAgentCount <= 0)
            {
                errorCode = PlaySoundErrorCode.SoundGroupHasNotEnoughAgent;
                errorMessage = AorTxt.Format("Sound group '{0}' 没有足够的 sound agent，errorCode = '{1}'", soundGroupName, errorCode.ToString());
            }

            if (errorCode.HasValue)
            {
                Log.Warning(errorMessage);
            }

            m_SoundsLoading.Add(serialID);

            PlaySoundInfo playSoundInfo = PlaySoundInfo.Create(serialID, soundGroup, playSoundParams, playSoundInfoShell);
            m_AssetComponent.LoadAssetAsync("Sound", abPath, assetName, (AssetObject assetObject, Object soundAsset) =>
            {
                if (m_SoundsToReleaseOnLoad.Contains(playSoundInfo.SerialID))
                {
                    m_SoundsToReleaseOnLoad.Remove(playSoundInfo.SerialID);
                    ReleaseSoundAsset(soundAsset);
                    return;
                }

                m_SoundsLoading.Remove(playSoundInfo.SerialID);

                SoundAgent soundAgent = playSoundInfo.SoundGroup.PlaySound(playSoundInfo.SerialID, soundAsset, playSoundInfo.PlaySoundParams, out errorCode);
                if (soundAgent == null)
                {
                    m_SoundsToReleaseOnLoad.Remove(playSoundInfo.SerialID);
                    ReleaseSoundAsset(soundAsset);
                    errorMessage = AorTxt.Format("Sound group '{0}' 播放声音 '{1}' 失败，errorCode = '{2}'", playSoundInfo.SoundGroup.Name, assetName, errorCode.ToString());
                    Log.Warning(errorMessage);
                }
            });
            return serialID;
        }

        /// <summary>
        /// 停止播放声音
        /// </summary>
        /// <param name="serialID">要停止播放声音的序列编号。</param>
        /// <returns>是否停止播放声音成功。</returns>
        public bool StopSound(int serialID)
        {
            return StopSound(serialID, SoundConstant.DefaultFadeOutSeconds);
        }

        /// <summary>
        /// 停止播放声音
        /// </summary>
        /// <param name="serialID">要停止播放声音的序列编号。</param>
        /// <param name="fadeOutSeconds">声音淡出时间，以秒为单位。</param>
        /// <returns>是否停止播放声音成功。</returns>
        public bool StopSound(int serialID, float fadeOutSeconds)
        {
            if (IsLoadingSound(serialID))
            {
                m_SoundsToReleaseOnLoad.Add(serialID);
                m_SoundsLoading.Remove(serialID);
                return true;
            }

            foreach (KeyValuePair<string, SoundGroup> soundGroup in m_SoundGroups)
            {
                if (soundGroup.Value.StopSound(serialID, fadeOutSeconds))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 停止所有已加载的声音
        /// </summary>
        public void StopAllLoadedSounds()
        {
            StopAllLoadedSounds(SoundConstant.DefaultFadeOutSeconds);
        }

        /// <summary>
        /// 停止所有已加载的声音
        /// </summary>
        /// <param name="fadeOutSeconds">声音淡出时间，以秒为单位。</param>
        public void StopAllLoadedSounds(float fadeOutSeconds)
        {
            foreach (KeyValuePair<string, SoundGroup> soundGroup in m_SoundGroups)
            {
                soundGroup.Value.StopAllLoadedSounds(fadeOutSeconds);
            }
        }

        /// <summary>
        /// 停止所有正在加载的声音
        /// </summary>
        public void StopAllLoadingSounds()
        {
            foreach (int serialID in m_SoundsLoading)
            {
                m_SoundsToReleaseOnLoad.Add(serialID);
            }
        }

        /// <summary>
        /// 暂停播放声音
        /// </summary>
        /// <param name="serialID">要暂停播放声音的序列编号。</param>
        public void PauseSound(int serialID)
        {
            PauseSound(serialID, SoundConstant.DefaultFadeOutSeconds);
        }

        /// <summary>
        /// 暂停播放声音
        /// </summary>
        /// <param name="serialID">要暂停播放声音的序列编号。</param>
        /// <param name="fadeOutSeconds">声音淡出时间，以秒为单位。</param>
        public void PauseSound(int serialID, float fadeOutSeconds)
        {
            foreach (KeyValuePair<string, SoundGroup> soundGroup in m_SoundGroups)
            {
                if (soundGroup.Value.PauseSound(serialID, fadeOutSeconds))
                {
                    return;
                }
            }

            throw new GameException(AorTxt.Format("无法找到 Sound '{0}'。", serialID.ToString()));
        }

        /// <summary>
        /// 暂停整个声音组的音乐播放
        /// </summary>
        /// <param name="groupName"></param>
        public bool PauseGroupSound(string groupName)
        {
            foreach (KeyValuePair<string, SoundGroup> soundGroup in m_SoundGroups)
            {
                if (soundGroup.Value.Name == groupName)
                {
                    Log.Info($"暂停整个声音组的音乐播放  name = {soundGroup.Value.Name}");
                    soundGroup.Value.PauseAllLoadedSounds(SoundConstant.DefaultFadeOutSeconds);
                    return true;
                }
            }
            Log.Error($"暂停整个声音组的音乐播放 未找到要设置的音乐组  name = {groupName}");
            return false;
        }

        /// <summary>
        /// 恢复播放声音
        /// </summary>
        /// <param name="serialID">要恢复播放声音的序列编号。</param>
        public bool ResumeSound(int serialID)
        {
            return ResumeSound(serialID, SoundConstant.DefaultFadeInSeconds);
        }

        /// <summary>
        /// 恢复播放声音
        /// </summary>
        /// <param name="serialID">要恢复播放声音的序列编号。</param>
        /// <param name="fadeInSeconds">声音淡入时间，以秒为单位。</param>
        public bool ResumeSound(int serialID, float fadeInSeconds)
        {
            foreach (KeyValuePair<string, SoundGroup> soundGroup in m_SoundGroups)
            {
                if (soundGroup.Value.ResumeSound(serialID, fadeInSeconds))
                {
                    return true;
                }
            }

            Log.Warning(AorTxt.Format("无法找到 Sound '{0}'。", serialID.ToString()));
            return false;
        }

        /// <summary>
        /// 恢复声音组音乐播放
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public bool ResumeGroupSound(string groupName)
        {
            foreach (KeyValuePair<string, SoundGroup> soundGroup in m_SoundGroups)
            {
                if (soundGroup.Value.Name == groupName)
                {
                    Log.Info($"恢复声音组音乐播放  name = {soundGroup.Value.Name}");
                    soundGroup.Value.ResumeAllLoadedSounds(SoundConstant.DefaultFadeInSeconds);
                    return true;
                }
            }
            Log.Error($"恢复声音组音乐播放 未找到要设置的音乐组  name = {groupName}");
            return false;
        }

        /// <summary>
        /// 停止声音组音乐播放
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public bool StopGroupSound(string groupName)
        {
            foreach (KeyValuePair<string, SoundGroup> soundGroup in m_SoundGroups)
            {
                if (soundGroup.Value.Name == groupName)
                {
                    Log.Info($"停止声音组音乐播放  name = {soundGroup.Value.Name}");
                    soundGroup.Value.StopAllLoadedSounds(SoundConstant.DefaultFadeOutSeconds);
                    return true;
                }
            }
            Log.Error($"停止声音组音乐播放 未找到要设置的音乐组  name = {groupName}");
            return false;
        }

        /// <summary>
        /// 释放声音资源
        /// </summary>
        /// <param name="soundAsset"></param>
        public void ReleaseSoundAsset(Object soundAsset)
        {
            m_AssetComponent.UnloadAsset(soundAsset);
        }

        /// <summary>
        /// 在播放过程中修改音量,对传入的名称组起作用
        /// </summary>
        /// <param name="newVolume">要设置的声音组音量</param>
        /// <param name="groupName">要设置的声音组名称</param>

        public void SetAllSoundVolume(float newVolume, string groupName)
        {
            foreach (KeyValuePair<string, SoundGroup> soundGroup in m_SoundGroups)
            {
                if (soundGroup.Value.Name == groupName)
                {
                    Log.Info($"设置音乐组音量  name = {soundGroup.Value.Name}");
                    soundGroup.Value.Volume = newVolume;
                    return;
                }
            }
            Log.Error($"设置音乐组音量 未找到要设置的音乐组  name = {groupName}");
        }

    }
}


