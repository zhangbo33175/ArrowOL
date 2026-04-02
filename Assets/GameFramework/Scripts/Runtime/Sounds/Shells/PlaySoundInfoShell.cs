using UnityEngine;

namespace Honor.Runtime
{
    public sealed class PlaySoundInfoShell
    {
        private Vector3 m_WorldPosition;
        public Vector3 WorldPosition
        {
            get
            {
                return m_WorldPosition;
            }
        }

        public PlaySoundInfoShell()
        {
            m_WorldPosition = Vector3.zero;
        }

        public static PlaySoundInfoShell Create(Vector3 worldPosition)
        {
            PlaySoundInfoShell playSoundInfo = new PlaySoundInfoShell();
            playSoundInfo.m_WorldPosition = worldPosition;
            return playSoundInfo;
        }

        public void Clear()
        {
            m_WorldPosition = Vector3.zero;
        }
    }
}


