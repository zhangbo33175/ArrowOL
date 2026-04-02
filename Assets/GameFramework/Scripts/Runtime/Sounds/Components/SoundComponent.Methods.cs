using UnityEngine;
using UnityEngine.Audio;

namespace Honor.Runtime
{
    public sealed partial class SoundComponent : GameComponent
    {
        /// <summary>
        /// 增加声音代理
        /// </summary>
        /// <param name="soundGroupName">声音组名称。</param>
        /// <param name="soundGroupHelper">声音组辅助器。</param>
        /// <param name="index">声音代理索引。</param>
        /// <returns>是否增加声音代理成功。</returns>
        private bool AddSoundAgent(string soundGroupName, SoundGroupHelper soundGroupHelper, int index)
        {
            SoundAgentHelper soundAgentHelper = new GameObject(AorTxt.Format("SoundAgent - {0} - {1}", soundGroupName, index.ToString())).AddComponent<SoundAgentHelper>();
            if (soundAgentHelper == null)
            {
                Log.Error("创建 Sound agent helper 失败。");
                return false;
            }

            soundAgentHelper.transform.SetParent(soundGroupHelper.transform);

            if (m_AudioMixer != null)
            {
                AudioMixerGroup[] audioMixerGroups = m_AudioMixer.FindMatchingGroups($"Master/{soundGroupName}/{soundGroupName}_{index}");
                if (audioMixerGroups.Length > 0)
                {
                    soundAgentHelper.AudioMixerGroup = audioMixerGroups[0];
                }
                else
                {
                    soundAgentHelper.AudioMixerGroup = soundGroupHelper.AudioMixerGroup;
                }
            }

            m_SoundManager.AddSoundAgent(soundGroupName, soundAgentHelper);

            return true;
        }

        /// <summary>
        /// 刷新声音监听器
        /// </summary>
        private void RefreshAudioListener()
        {
            m_AudioListener.enabled = FindObjectsOfType<AudioListener>().Length <= 1;
        }

    }
}


