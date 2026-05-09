using UnityEngine;
using UnityEngine.Audio;

namespace Honor.Runtime
{
    /// <summary>
    /// 音频管理组件（私有方法分部类）
    /// 包含：声音代理创建、音频监听器管理等工具方法
    /// </summary>
    public sealed partial class SoundComponent : GameComponent
    {
        /// <summary>
        /// 添加声音代理（AudioSource 播放器）
        /// 每个声音代理对应一个 AudioSource，用于播放音频
        /// </summary>
        /// <param name="soundGroupName">声音组名称</param>
        /// <param name="soundGroupHelper">声音组辅助对象</param>
        /// <param name="index">当前代理序号</param>
        /// <returns>是否创建成功</returns>
        private bool AddSoundAgent(string soundGroupName, SoundGroupHelper soundGroupHelper, int index)
        {
            // 创建声音代理对象
            SoundAgentHelper soundAgentHelper = new GameObject(
                AorTxt.Format("SoundAgent - {0} - {1}", soundGroupName, index.ToString())
            ).AddComponent<SoundAgentHelper>();

            if (soundAgentHelper == null)
            {
                Log.Error("创建 Sound agent helper 失败。");
                return false;
            }

            // 设置父物体，保持层级整洁
            soundAgentHelper.transform.SetParent(soundGroupHelper.transform);

            // 绑定 AudioMixer 混音器轨道（支持按代理分组）
            if (m_AudioMixer != null)
            {
                // 优先找独立的混音轨道
                AudioMixerGroup[] audioMixerGroups =
                    m_AudioMixer.FindMatchingGroups($"Master/{soundGroupName}/{soundGroupName}_{index}");
                // 找不到则使用组轨道
                soundAgentHelper.AudioMixerGroup = audioMixerGroups.Length > 0
                    ? audioMixerGroups[0]
                    : soundGroupHelper.AudioMixerGroup;
            }

            // 将代理添加到管理器中统一调度
            m_SoundManager.AddSoundAgent(soundGroupName, soundAgentHelper);
            return true;
        }

        /// <summary>
        /// 刷新音频监听器状态
        /// 确保全局只有一个 AudioListener 生效，避免声音异常
        /// </summary>
        private void RefreshAudioListener()
        {
            // 如果场景中只有一个 AudioListener，则启用；否则禁用自身
            m_AudioListener.enabled = Object.FindObjectsOfType<AudioListener>().Length <= 1;
        }
    }
}