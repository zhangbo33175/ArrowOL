/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  DebuggerActiveWindowType.cs
 * author:    云毅
 * created:   2026
 * descrip:   调试器激活窗口类型
 ***************************************************************/

namespace Honor.Runtime
{
    /// <summary>
    /// 调试器激活窗口类型。
    /// </summary>
    public enum DebuggerActiveWindowType : byte
    {
        /// <summary>
        /// 总是打开。
        /// </summary>
        AlwaysOpen = 0,

        /// <summary>
        /// 仅在开发模式时打开。
        /// </summary>
        OnlyOpenWhenDevelopment,

        /// <summary>
        /// 仅在编辑器中打开。
        /// </summary>
        OnlyOpenInEditor,

        /// <summary>
        /// 总是关闭。
        /// </summary>
        AlwaysClose,
    }
}

