/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  LauncherComponent.cs
 * author:    云毅
 * created:   2026
 * descrip:   启动器组件配置
 *            服务端环境、热更调试模式等启动配置管理
 ***************************************************************/

using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// 启动器组件配置（服务端、热更调试模式）
    /// </summary>
    public sealed partial class LauncherComponent
    {
        #region 序列化配置字段
        //=========================================================================
        // 序列化配置字段
        //=========================================================================
        /// <summary>
        /// 是否使用本地服务器
        /// </summary>
        [SerializeField] public bool m_IsLocalServer = false;

        /// <summary>
        /// 编辑器下是否实时调试热更新（默认关闭）
        /// </summary>
        [SerializeField] public bool m_IsRealTimeDebuggerHotfixForEditor = false;
        #endregion

        #region 公共访问属性
        //=========================================================================
        // 公共访问属性
        //=========================================================================
        /// <summary>
        /// 是否使用本地服务器（外部访问属性）
        /// </summary>
        public bool IsLocalServer
        {
            get => m_IsLocalServer;
            set => m_IsLocalServer = value;
        }

        /// <summary>
        /// 编辑器下是否实时调试热更新（外部访问属性）
        /// </summary>
        public bool IsRealTimeDebuggerHotfixForEditor
        {
            get => m_IsRealTimeDebuggerHotfixForEditor;
            set => m_IsRealTimeDebuggerHotfixForEditor = value;
        }
        #endregion
    }
}