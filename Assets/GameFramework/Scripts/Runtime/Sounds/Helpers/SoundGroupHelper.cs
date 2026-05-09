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

        public AudioMixerGroup AudioMixerGroup
        {
            get => m_AudioMixerGroup;
            set => m_AudioMixerGroup = value;
        }
    }
}