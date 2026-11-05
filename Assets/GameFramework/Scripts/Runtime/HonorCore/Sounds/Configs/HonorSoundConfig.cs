/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  HonorSoundConfig.cs
 * author:    云毅
 * created:   2026
 * descrip:   音频配置资产（ScriptableObject）
 *            承载编辑器配置：AudioMixer 引用 + 声音组预设列表
 *            供「声音表工具」窗口可视化编辑，运行时由 HonorSoundComponent.Init 读取初始化
 * 调用链：   SoundExportEditorWindow(编辑) → HonorSoundConfig.asset → HonorSoundComponent.Init(应用)
 ***************************************************************/

using UnityEngine;
using UnityEngine.Audio;

namespace Honor.Runtime
{
    /// <summary>
    /// 音频全局配置资产
    /// <para>作为编辑器可序列化的真实 <see cref="ScriptableObject"/>，替代原先挂在纯 C# 单例
    /// <see cref="HonorSoundComponent"/> 上失效的 [SerializeField] 字段（纯 C# 单例不走 Unity 序列化，
    /// 无法在 Inspector / 工具窗口绑定）。资产存放于 Resources 目录，运行时可通过
    /// <see cref="Resources.Load{T}"/> 加载；未配置时回退到 HonorSoundComponent 的默认硬编码。</para>
    /// </summary>
    [CreateAssetMenu(fileName = "HonorSoundConfig", menuName = "Honor/Audio/HonorSoundConfig")]
    public sealed class HonorSoundConfig : ScriptableObject
    {
        #region 配置字段
        //=========================================================================
        // 配置字段（序列化到资产）
        //=========================================================================

        /// <summary>
        /// 音频混音器（Unity AudioMixer）
        /// 用于全局分组音量、静音、混音效果控制
        /// </summary>
        [Header("音频混响器（AudioMixer）")]
        [SerializeField]
        private AudioMixer m_AudioMixer = null;

        /// <summary>
        /// 声音组预设列表
        /// 运行时据此创建声音组（名称 / 静音 / 音量 / 播放器数量等）
        /// </summary>
        [Header("声音分组预设")]
        [SerializeField]
        private HonorSoundGroupShell[] m_SoundGroupShells = null;
        #endregion

        #region 公共只读属性
        //=========================================================================
        // 公共只读属性
        //=========================================================================

        /// <summary>
        /// 音频混音器
        /// </summary>
        public AudioMixer AudioMixer => m_AudioMixer;

        /// <summary>
        /// 声音组预设列表（可能为 null，表示未配置）
        /// </summary>
        public HonorSoundGroupShell[] SoundGroupShells => m_SoundGroupShells;
        #endregion

        #region 运行时读取工具
        //=========================================================================
        // 运行时读取工具
        //=========================================================================

        /// <summary>
        /// 运行时加载音频配置资产
        /// <para>资产缺失或尚未配置时返回 null，调用方应自行回退到默认值。</para>
        /// </summary>
        /// <returns>配置资产实例；未找到返回 null</returns>
        public static HonorSoundConfig Load()
        {
            return Resources.Load<HonorSoundConfig>("HonorSoundConfig");
        }

        /// <summary>
        /// 全局访问点（带缓存）
        /// <para>第一次访问时从 Resources 加载并缓存，之后直接返回缓存实例，
        /// 供编辑器/运行时代码统一读取音频配置。资产缺失时返回 null 且不缓存。</para>
        /// </summary>
        private static HonorSoundConfig s_CachedInstance;

        public static HonorSoundConfig Instance
        {
            get
            {
                if (s_CachedInstance == null)
                {
                    s_CachedInstance = Load();
                }

                return s_CachedInstance;
            }
        }

        /// <summary>
        /// 释放缓存的全局访问点实例（用于编辑器在资产改动后强制重新读取）
        /// </summary>
        public static void ReleaseInstance()
        {
            s_CachedInstance = null;
        }
        #endregion
    }
}
