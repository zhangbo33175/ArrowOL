/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * -------------------------------------------------------------
 * filename:  SoundComponent.Methods.cs
 * author:  云毅
 * created:
 * descrip:   音频组件 - 私有工具方法分部类（声音代理、监听器管理）
 ***************************************************************/

using UnityEngine;
using UnityEngine.Audio;

namespace Honor.Runtime
{
    /// <summary>
    /// 音频管理组件（私有方法分部类）
    /// 包含：声音代理创建、音频监听器管理、混音轨道解析等工具方法
    /// </summary>
    public sealed partial class SoundComponent : GameComponent
    {
        //=========================================================================
        #region 私有工具方法
        //=========================================================================

        /// <summary>
        /// 添加声音代理（AudioSource 播放器）
        /// 每个声音代理对应一个 AudioSource，用于播放音频
        /// </summary>
        /// <param name="soundGroupName">声音组名称</param>
        /// <param name="soundGroupHelper">声音组辅助对象</param>
        /// <param name="agentIndex">当前代理序号</param>
        /// <returns>是否创建成功</returns>
        private bool AddSoundAgent(string soundGroupName, SoundGroupHelper soundGroupHelper, int agentIndex)
        {
            // 创建声音代理对象
            SoundAgentHelper soundAgentHelper = new GameObject(
                AorTxt.Format("SoundAgent - {0} - {1}", soundGroupName, agentIndex))
            .AddComponent<SoundAgentHelper>();

            if (soundAgentHelper == null)
            {
                Log.Error("创建 Sound agent helper 失败。");
                return false;
            }

            // 设置父物体，保持层级整洁
            soundAgentHelper.transform.SetParent(soundGroupHelper.transform);

            // 绑定 AudioMixer 混音器轨道（优先独立代理轨道，否则回退到组轨道）
            if (m_AudioMixer != null)
            {
                string agentRoute = AorTxt.Format("Master/{0}/{0}_{1}", soundGroupName, agentIndex);
                soundAgentHelper.AudioMixerGroup = ResolveMixerGroup(agentRoute, soundGroupHelper.AudioMixerGroup);
            }

            // 将代理添加到管理器中统一调度
            m_AudioService.AddSoundAgent(soundGroupName, soundAgentHelper);
            return true;
        }

        /// <summary>
        /// 在 AudioMixer 中按路由路径解析分组轨道；找不到时回退到默认轨道
        /// </summary>
        /// <param name="routePath">混音器内的路由路径</param>
        /// <param name="fallbackGroup">未匹配到时使用的回退轨道</param>
        /// <returns>解析到的混音轨道；无混音器时返回 null</returns>
        private AudioMixerGroup ResolveMixerGroup(string routePath, AudioMixerGroup fallbackGroup)
        {
            if (m_AudioMixer == null)
            {
                return null;
            }

            AudioMixerGroup[] matchedGroups = m_AudioMixer.FindMatchingGroups(routePath);
            return matchedGroups.Length > 0 ? matchedGroups[0] : fallbackGroup;
        }

        /// <summary>
        /// 刷新音频监听器状态
        /// 确保全局只有一个 AudioListener 生效，避免声音异常、定位错误
        /// </summary>
        private void RefreshAudioListener()
        {
            // 如果场景中只有一个 AudioListener，则启用；否则禁用自身
            m_AudioListener.enabled = Object.FindObjectsByType<AudioListener>(FindObjectsSortMode.None).Length <= 1;
        }

        #endregion
    }
}
