using UnityEngine;
using UnityEngine.Audio;

namespace Honor.Runtime
{
    public sealed partial class SoundComponent : GameComponent
    {
        /// <summary>
        /// 声音混响器
        /// </summary>
        [SerializeField]
        private AudioMixer m_AudioMixer = null;
        public AudioMixer AudioMixer
        {
            get
            {
                return m_AudioMixer;
            }
        }

        /// <summary>
        /// 声音组-外壳
        /// </summary>
        [SerializeField]
        private SoundGroupShell[] m_SoundGroupShells = null;

        /// <summary>
        /// 声音管理器
        /// </summary>
        private SoundManager m_SoundManager = null;

        /// <summary>
        /// 声音监听器
        /// </summary>
        private AudioListener m_AudioListener = null;

        /// <summary>
        /// 获取声音组数量。
        /// </summary>
        public int SoundGroupCount
        {
            get
            {
                return m_SoundManager.SoundGroupCount;
            }
        }

    }
}


