/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  SoundGroupShell.cs
 * author:    云毅
 * created:   2026
 * descrip:   声音组配置外壳 - 用于Inspector面板可视化配置声音组参数
 ***************************************************************/

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
        //=========================================================================
        // 配置字段（序列化到Inspector）
        //=========================================================================
        #region 序列化配置字段

        [Header("声音组名称（如 BGM、Effect、UI）")]
        [SerializeField]
        private string m_Name = null;

        [Header("同优先级声音是否不互相顶替")]
        [SerializeField]
        private bool m_AvoidBeingReplacedBySamePriority = false;

        [Header("是否默认静音")]
        [SerializeField]
        private bool m_Mute = false;

        [Header("默认音量")]
        [SerializeField, Range(0f, 1f)]
        private float m_Volume = 1f;

        [Header("该组预设播放器数量")]
        [SerializeField]
        private int m_AgentCount = 1;

        #endregion


        //=========================================================================
        // 公共只读属性
        //=========================================================================
        #region 公共属性

        /// <summary>
        /// 声音组名称
        /// </summary>
        public string Name => m_Name;

        /// <summary>
        /// 同优先级声音是否不互相顶替
        /// </summary>
        public bool AvoidBeingReplacedBySamePriority => m_AvoidBeingReplacedBySamePriority;

        /// <summary>
        /// 是否默认静音
        /// </summary>
        public bool Mute => m_Mute;

        /// <summary>
        /// 默认音量
        /// </summary>
        public float Volume => m_Volume;

        /// <summary>
        /// 该组预设播放器数量
        /// </summary>
        public int AgentCount => m_AgentCount;

        #endregion
    }
}