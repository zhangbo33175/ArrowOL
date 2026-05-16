/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * -------------------------------------------------------------
 * filename:  SoundGroupHelper.cs
 * author:  云毅
 * created:
 * descrip:   声音组辅助脚本 —— 声音组的GameObject载体，用于绑定AudioMixer分组轨道
 ***************************************************************/

using UnityEngine;
using UnityEngine.Audio;

namespace Honor.Runtime
{
    /// <summary>
    /// 声音组辅助脚本（MonoBehaviour）
    /// 作用：作为声音组的GameObject载体，主要用于绑定AudioMixer混音轨道
    /// 每个SoundGroup对应一个SoundGroupHelper
    /// </summary>
    public class SoundGroupHelper : MonoBehaviour
    {
        /// <summary>
        /// 音频混音器轨道（用于分组控制音量、静音等）
        /// 例如：Master/BGM、Master/Effect
        /// </summary>
        [SerializeField] 
        private AudioMixerGroup m_AudioMixerGroup = null;

        /// <summary>
        /// 音频混音器轨道
        /// </summary>
        public AudioMixerGroup AudioMixerGroup
        {
            get => m_AudioMixerGroup;
            set => m_AudioMixerGroup = value;
        }
    }
}