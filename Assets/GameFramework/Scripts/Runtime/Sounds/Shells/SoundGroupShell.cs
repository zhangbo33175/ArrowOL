using System;
using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// 声音组配置外壳（可序列化）
    /// 作用：在 Inspector 面板中配置声音组的参数，用于初始化声音组
    /// </summary>
    [Serializable]
    public sealed class SoundGroupShell
    {
        [Header("声音组名称（如 BGM、Effect、UI）")]
        [SerializeField]
        private string m_Name = null;
        public string Name => m_Name;

        [Header("同优先级声音是否不互相顶替")]
        [SerializeField]
        private bool m_AvoidBeingReplacedBySamePriority = false;
        public bool AvoidBeingReplacedBySamePriority => m_AvoidBeingReplacedBySamePriority;

        [Header("是否默认静音")]
        [SerializeField]
        private bool m_Mute = false;
        public bool Mute => m_Mute;

        [Header("默认音量")]
        [SerializeField, Range(0f, 1f)]
        private float m_Volume = 1f;
        public float Volume => m_Volume;

        [Header("该组预设播放器数量")]
        [SerializeField]
        private int m_AgentCount = 1;
        public int AgentCount => m_AgentCount;
    }
}