using UnityEngine;

namespace Honor.Runtime
{
    public sealed partial class LauncherComponent
    {
        /// <summary>
        /// 是否为本地服务器
        /// </summary>
        [SerializeField] public bool m_IsLocalServer = false;

        /// <summary>
        /// 在编辑器模式下，是否实时调试热更新,默认为false
        /// </summary>
        [SerializeField] public bool m_IsRealTimeDebuggerHotfixForEditor = false;

        /// <summary>
        /// 是否为本地服务器
        /// </summary>
        public bool IsLocalServer
        {
            get => m_IsLocalServer;
            set => m_IsLocalServer = value;
        }
        
        /// <summary>
        /// 在编辑器模式下，是否实时调试热更新,默认为false
        /// </summary>
        public bool IsRealTimeDebuggerHotfixForEditor
        {
            get => m_IsRealTimeDebuggerHotfixForEditor;
            set => m_IsRealTimeDebuggerHotfixForEditor = value;
        }
        
    }
}