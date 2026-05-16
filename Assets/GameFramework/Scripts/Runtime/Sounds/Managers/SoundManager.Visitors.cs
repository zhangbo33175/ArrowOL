/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  SoundManager.Variable.cs
 * author:    云毅
 * created:   2026
 * descrip:   声音管理器 - 成员变量分部类
 *           存放音频系统核心全局变量，与逻辑实现分离，结构更清晰
 ***************************************************************/

using System.Collections.Generic;

namespace Honor.Runtime
{
    /// <summary>
    /// 声音管理器（成员变量分部类）
    /// 存放音频系统核心全局变量，与逻辑实现分离，结构更清晰
    /// </summary>
    public sealed partial class SoundManager
    {
        //=========================================================================
        // 核心成员变量
        //=========================================================================
        #region 核心成员变量
        
        /// <summary>
        /// 声音组字典
        /// 键：声音组名称（如 BGM、Effect、UI、Voice）
        /// 值：对应声音组实例，统一管理各类音频
        /// </summary>
        private readonly Dictionary<string, SoundGroup> m_SoundGroups;

        /// <summary>
        /// 正在加载中的声音序列ID列表
        /// 用于标记异步加载中的音频，防止重复加载、支持中途停止
        /// </summary>
        private readonly List<int> m_SoundsLoading;

        /// <summary>
        /// 加载完成后需要立即释放的声音ID集合
        /// 如果音频在加载过程中被调用停止，加载完成后直接释放，不播放
        /// </summary>
        private readonly HashSet<int> m_SoundsToReleaseOnLoad;

        /// <summary>
        /// 资源组件
        /// 负责音频资源的异步加载、卸载、引用计数管理
        /// </summary>
        private AssetComponent m_AssetComponent;

        /// <summary>
        /// 声音唯一序列ID（自增）
        /// 每播放一个声音就+1，用于全局唯一标识、控制播放/暂停/停止
        /// </summary>
        private int m_Serial;

        #endregion
    }
}