using System;
using UnityEngine;

namespace Honor.Runtime
{
    [Serializable]
    public sealed class SoundGroupShell
    {
        [SerializeField]
        private string m_Name = null;
        public string Name
        {
            get
            {
                return m_Name;
            }
        }

        [SerializeField]
        private bool m_AvoidBeingReplacedBySamePriority = false;
        public bool AvoidBeingReplacedBySamePriority
        {
            get
            {
                return m_AvoidBeingReplacedBySamePriority;
            }
        }

        [SerializeField]
        private bool m_Mute = false;
        public bool Mute
        {
            get
            {
                return m_Mute;
            }
        }

        [SerializeField, Range(0f, 1f)]
        private float m_Volume = 1f;
        public float Volume
        {
            get
            {
                return m_Volume;
            }
        }

        [SerializeField]
        private int m_AgentCount = 1;
        public int AgentCount
        {
            get
            {
                return m_AgentCount;
            }
        }

    }
}


