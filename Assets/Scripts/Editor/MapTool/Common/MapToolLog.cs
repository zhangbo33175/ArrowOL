/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  MapToolLog.cs
 * author:    云毅
 * created:   2026
 * descrip:   地图编辑器日志门面
 *            统一 Editor 目录的日志出口，后续如需接入等级过滤或日志面板只需改本文件
 ***************************************************************/

using UnityEngine;

namespace Honor.Editor.MapTool
{
    /// <summary>
    /// 地图编辑器日志门面
    /// 输出文案与原 Debug.Log/LogWarning/LogError 完全一致，仅收敛调用入口。
    /// </summary>
    public static class MapToolLog
    {
        /// <summary>是否输出普通日志（关闭后仅保留警告与错误）</summary>
        public static bool VerboseEnabled { get; set; } = true;

        /// <summary>
        /// 普通日志
        /// </summary>
        /// <param name="message">日志内容</param>
        public static void Info(object message)
        {
            if (!VerboseEnabled) return;
            Debug.Log(message);
        }

        /// <summary>
        /// 警告日志
        /// </summary>
        /// <param name="message">日志内容</param>
        public static void Warn(object message)
        {
            Debug.LogWarning(message);
        }

        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="message">日志内容</param>
        public static void Error(object message)
        {
            Debug.LogError(message);
        }
    }
}
