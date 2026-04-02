using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using XLua;

namespace Honor.Runtime
{
    [DisallowMultipleComponent]
    public sealed partial class SoundComponent : GameComponent
    {
        protected override void Awake()
        {
            base.Awake();

            m_SoundManager = new SoundManager();
            if (m_SoundManager == null)
            {
                Log.Fatal("Sound manager 无效。");
                return;
            }

            m_AudioListener = gameObject.GetOrAddComponent<AudioListener>();

        }

        private void Start()
        {
            for (int i = 0; i < m_SoundGroupShells.Length; i++)
            {
                if (!AddSoundGroup(m_SoundGroupShells[i].Name, m_SoundGroupShells[i].AvoidBeingReplacedBySamePriority, m_SoundGroupShells[i].Mute, m_SoundGroupShells[i].Volume, m_SoundGroupShells[i].AgentCount))
                {
                    Log.Warning("添加 Sound Group '{0}' 失败。", m_SoundGroupShells[i].Name);
                    continue;
                }
            }
        }

        private void OnDestroy()
        {

        }

        /// <summary>
        /// 是否存在指定声音组
        /// </summary>
        /// <param name="soundGroupName">声音组名称。</param>
        /// <returns>指定声音组是否存在。</returns>
        public bool HasSoundGroup(string soundGroupName)
        {
            return m_SoundManager.HasSoundGroup(soundGroupName);
        }

        /// <summary>
        /// 获取指定声音组
        /// </summary>
        /// <param name="soundGroupName">声音组名称。</param>
        /// <returns>要获取的声音组。</returns>
        public SoundGroup GetSoundGroup(string soundGroupName)
        {
            return m_SoundManager.GetSoundGroup(soundGroupName);
        }

        /// <summary>
        /// 获取所有声音组
        /// </summary>
        /// <returns>所有声音组。</returns>
        public SoundGroup[] GetAllSoundGroups()
        {
            return m_SoundManager.GetAllSoundGroups();
        }

        /// <summary>
        /// 获取所有声音组
        /// </summary>
        /// <param name="results">所有声音组。</param>
        public void GetAllSoundGroups(List<SoundGroup> results)
        {
            m_SoundManager.GetAllSoundGroups(results);
        }

        /// <summary>
        /// 增加声音组
        /// </summary>
        /// <param name="soundGroupName">声音组名称。</param>
        /// <param name="soundAgentCount">声音代理数量。</param>
        /// <returns>是否增加声音组成功。</returns>
        public bool AddSoundGroup(string soundGroupName, int soundAgentCount)
        {
            return AddSoundGroup(soundGroupName, false, SoundConstant.DefaultMute, SoundConstant.DefaultVolume, soundAgentCount);
        }

        /// <summary>
        /// 增加声音组
        /// </summary>
        /// <param name="soundGroupName">声音组名称。</param>
        /// <param name="soundGroupAvoidBeingReplacedBySamePriority">声音组中的声音是否避免被同优先级声音替换。</param>
        /// <param name="soundGroupMute">声音组是否静音。</param>
        /// <param name="soundGroupVolume">声音组音量。</param>
        /// <param name="soundAgentCount">声音代理数量。</param>
        /// <returns>是否增加声音组成功。</returns>
        public bool AddSoundGroup(string soundGroupName, bool soundGroupAvoidBeingReplacedBySamePriority, bool soundGroupMute, float soundGroupVolume, int soundAgentCount)
        {
            if (m_SoundManager.HasSoundGroup(soundGroupName))
            {
                return false;
            }

            SoundGroupHelper soundGroupHelper = new GameObject(AorTxt.Format("SoundGroup - {0}", soundGroupName)).AddComponent<SoundGroupHelper>();
            if (soundGroupHelper == null)
            {
                Log.Error("创建 Sound group helper 失败。");
                return false;
            }

            soundGroupHelper.transform.SetParent(transform);

            if (m_AudioMixer != null)
            {
                AudioMixerGroup[] audioMixerGroups = m_AudioMixer.FindMatchingGroups(AorTxt.Format("Master/{0}", soundGroupName));
                if (audioMixerGroups.Length > 0)
                {
                    soundGroupHelper.AudioMixerGroup = audioMixerGroups[0];
                }
                else
                {
                    soundGroupHelper.AudioMixerGroup = m_AudioMixer.FindMatchingGroups("Master")[0];
                }
            }

            if (!m_SoundManager.AddSoundGroup(soundGroupName, soundGroupAvoidBeingReplacedBySamePriority, soundGroupMute, soundGroupVolume, soundGroupHelper))
            {
                return false;
            }

            for (int i = 1; i <= soundAgentCount; i++)
            {
                if (!AddSoundAgent(soundGroupName, soundGroupHelper, i))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 获取所有正在加载声音的序列编号
        /// </summary>
        /// <returns>所有正在加载声音的序列编号。</returns>
        public int[] GetAllLoadingSoundSerialIDs()
        {
            return m_SoundManager.GetAllLoadingSoundSerialIDs();
        }

        /// <summary>
        /// 获取所有正在加载声音的序列编号
        /// </summary>
        /// <param name="results">所有正在加载声音的序列编号。</param>
        public void GetAllLoadingSoundSerialIDs(List<int> results)
        {
            m_SoundManager.GetAllLoadingSoundSerialIDs(results);
        }

        /// <summary>
        /// 是否正在加载声音
        /// </summary>
        /// <param name="serialID">声音序列编号。</param>
        /// <returns>是否正在加载声音。</returns>
        public bool IsLoadingSound(int serialID)
        {
            return m_SoundManager.IsLoadingSound(serialID);
        }

        /// <summary>
        /// 播放声音
        /// </summary>
        /// <param name="luaTable">luaTable</param>
        /// <returns>声音的序列编号。</returns>
        public int PlaySound(LuaTable luaTable)
        {
            if (luaTable == null)
            {
                Log.Error("SoundComponent.PlaySound luaTable 无效。");
                return 0;
            }

            string abPath = string.Empty;
            string assetName = string.Empty;
            string groupName = string.Empty;
            int priority = 0;
            bool loop = false;
            float volume = 1;
            float spatialBlend = 0;
            float maxDistance = 0;
            float speed = 1;
            Vector3 worldPosition = Vector3.zero;

            luaTable.Get("ABPath", out abPath);
            luaTable.Get("AssetName", out assetName);
            luaTable.Get("GroupName", out groupName);
            luaTable.Get("Priority", out priority);
            luaTable.Get("Loop", out loop);
            luaTable.Get("Volume", out volume);
            luaTable.Get("SpatialBlend", out spatialBlend);
            luaTable.Get("MaxDistance", out maxDistance);
            luaTable.Get("WorldPosition", out worldPosition);
            luaTable.Get("Speed", out speed);

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
        /// 播放声音
        /// </summary>
        /// <param name="abPath">ab资源路径</param>
        /// <param name="assetName">asset资源名称</param>
        /// <param name="soundGroupName">声音组名称。</param>
        /// <param name="playSoundParams">播放声音参数。</param>
        /// <returns>声音的序列编号。</returns>
        public int PlaySound(string abPath, string assetName, string soundGroupName, PlaySoundParams playSoundParams = null, Vector3 worldPosition = default(Vector3))
        {
            return m_SoundManager.PlaySound(abPath, assetName, soundGroupName, playSoundParams, PlaySoundInfoShell.Create(worldPosition));
        }

        /// <summary>
        /// 停止播放声音
        /// </summary>
        /// <param name="serialID">要停止播放声音的序列编号。</param>
        /// <returns>是否停止播放声音成功。</returns>
        public bool StopSound(int serialID)
        {
            return m_SoundManager.StopSound(serialID);
        }

        /// <summary>
        /// 停止播放声音
        /// </summary>
        /// <param name="serialID">要停止播放声音的序列编号。</param>
        /// <param name="fadeOutSeconds">声音淡出时间，以秒为单位。</param>
        /// <returns>是否停止播放声音成功。</returns>
        public bool StopSound(int serialID, float fadeOutSeconds)
        {
            return m_SoundManager.StopSound(serialID, fadeOutSeconds);
        }

        /// <summary>
        /// 停止所有已加载的声音
        /// </summary>
        public void StopAllLoadedSounds()
        {
            m_SoundManager.StopAllLoadedSounds();
        }

        /// <summary>
        /// 停止所有已加载的声音
        /// </summary>
        /// <param name="fadeOutSeconds">声音淡出时间，以秒为单位。</param>
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

        /// <summary>
        /// 暂停播放声音
        /// </summary>
        /// <param name="serialID">要暂停播放声音的序列编号。</param>
        public void PauseSound(int serialID)
        {
            m_SoundManager.PauseSound(serialID);
        }

        /// <summary>
        /// 暂停播放声音
        /// </summary>
        /// <param name="serialID">要暂停播放声音的序列编号。</param>
        /// <param name="fadeOutSeconds">声音淡出时间，以秒为单位。</param>
        public void PauseSound(int serialID, float fadeOutSeconds)
        {
            m_SoundManager.PauseSound(serialID, fadeOutSeconds);
        }

        /// <summary>
        /// 恢复播放声音
        /// </summary>
        /// <param name="serialID">要恢复播放声音的序列编号。</param>
        public bool ResumeSound(int serialID)
        {
            return m_SoundManager.ResumeSound(serialID);
        }

        /// <summary>
        /// 恢复播放声音
        /// </summary>
        /// <param name="serialID">要恢复播放声音的序列编号。</param>
        /// <param name="fadeInSeconds">声音淡入时间，以秒为单位。</param>
        public bool ResumeSound(int serialID, float fadeInSeconds)
        {
            return m_SoundManager.ResumeSound(serialID, fadeInSeconds);
        }

        /// <summary>
        /// 暂停整个声音组的音乐播放
        /// </summary>
        /// <param name="groupName"></param>
        public bool PauseGourpSound(string groupName)
        {
            return m_SoundManager.PauseGroupSound(groupName);
        }

        /// <summary>
        /// 恢复整个声音组的音乐播放
        /// </summary>
        /// <param name="groupName"></param>
        public bool ResumeGroupSound(string groupName)
        {
            return m_SoundManager.ResumeGroupSound(groupName);
        }

        /// <summary>
        /// 停止整个声音组的音乐播放
        /// </summary>
        /// <param name="groupName"></param>
        public bool StopGroupSound(string groupName)
        {
            return m_SoundManager.StopGroupSound(groupName);
        }


        /// <summary>
        /// 在播放过程中修改音量,对传入的名称组起作用
        /// </summary>
        /// <param name="newVolume">要设置的声音组音量</param>
        /// <param name="groupName">要设置的声音组名称</param>

        public void SetAllSoundVolume(float newVolume, string groupName)
        {
            m_SoundManager.SetAllSoundVolume(newVolume, groupName);
        }

    }
}


