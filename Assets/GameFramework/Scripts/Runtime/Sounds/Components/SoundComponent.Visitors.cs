/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * -------------------------------------------------------------
 * filename:  SoundComponent.Property.cs
 * author:  云毅
 * created:
 * descrip:   音频组件 - 变量与属性定义分部类（编辑器配置 + 对外接口）
 ***************************************************************/

using UnityEngine;
using UnityEngine.Audio;

namespace Honor.Runtime
{
    /// <summary>
    /// 音频管理组件（变量/属性定义分部类）
    /// 包含：混音器、声音组配置、管理器实例、监听器、对外只读属性
    /// </summary>
    public sealed partial class SoundComponent : GameComponent
    {
        //=========================================================================
        #region 序列化字段（Inspector 配置）
        //=========================================================================

        /// <summary>
        /// 音频混音器（Unity AudioMixer）
        /// 用于分组控制音量、静音、混音效果，编辑器配置
        /// </summary>
        [SerializeField] 
        private AudioMixer m_AudioMixer = null;

        /// <summary>
        /// 声音组配置数组（编辑器配置）
        /// 用于在Inspector中预设声音组：BGM、Effect、UI、Voice等
        /// </summary>
        [SerializeField] 
        private SoundGroupShell[] m_SoundGroupShells = null;

        #endregion

        //=========================================================================
        #region 私有成员变量
        //=========================================================================

        /// <summary>
        /// 声音管理器（底层逻辑核心）
        /// 真正处理声音播放、暂停、停止、池化的逻辑类
        /// </summary>
        private SoundManager m_SoundManager = null;

        /// <summary>
        /// 音频监听器
        /// 全局唯一，相当于“耳朵”，用于接收场景声音
        /// </summary>
        private AudioListener m_AudioListener = null;

        #endregion

        //=========================================================================
        #region 公共属性（对外只读）
        //=========================================================================

        /// <summary>
        /// 音频混音器
        /// </summary>
        public AudioMixer AudioMixer => m_AudioMixer;

        /// <summary>
        /// 当前声音组数量
        /// </summary>
        public int SoundGroupCount => m_SoundManager.SoundGroupCount;

        #endregion
    }
}