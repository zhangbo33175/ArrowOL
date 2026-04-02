using UnityEngine;
using UnityEngine.Audio;

namespace Honor.Runtime
{
    public class SoundGroupHelper : MonoBehaviour
    {
        /// <summary>
        /// 混音组
        /// </summary>
        [SerializeField]
        private AudioMixerGroup m_AudioMixerGroup = null;
        public AudioMixerGroup AudioMixerGroup
        {
            get
            {
                return m_AudioMixerGroup;
            }
            set
            {
                m_AudioMixerGroup = value;
            }
        }

    }
}


