/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  VibrateComponent.Define.cs
 * author:    云毅
 * created:   2026
 * descrip:   震动组件 - 成员变量定义分部类（与逻辑分离）
 ***************************************************************/
using UnityEngine;
using UnityEngine.Audio;

namespace Honor.Runtime
{
    /// <summary>
    /// 震动组件（成员变量分部类）
    /// 作用：声明震动系统的核心成员变量，与逻辑实现分离，保持代码结构清晰
    /// </summary>
    public sealed partial class VibrateComponent : GameComponent
    {
        /// <summary>
        /// 震动逻辑管理器
        /// 真正执行震动播放、停止、组合逻辑
        /// </summary>
        private VibrateManager m_VibrateManager = null;
    }
}