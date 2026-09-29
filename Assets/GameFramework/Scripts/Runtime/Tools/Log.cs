/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  Log.cs
 * author:    云毅
 * created:   2026
 * descrip:   游戏全局分级日志工具，支持条件编译剥离，带帧计数输出
 ***************************************************************/
using System;
using System.Diagnostics;
using UnityEngine;

namespace Honor.Runtime
{
    //=========================================================================
    // 游戏全局日志工具类
    //=========================================================================
    /// <summary>
    /// 游戏全局日志工具类
    /// 提供分级日志输出：Debug / Info / Warning / Error / Fatal
    /// 支持多条件预编译开关，可在发布包中完全剥离日志，提升性能
    /// </summary>
    public static class Log
    {
        #region 日志等级定义
        /// <summary>
        /// 日志等级枚举
        /// </summary>
        public enum LogLevel : byte
        {
            /// <summary>
            /// 调试日志（开发调试使用）
            /// </summary>
            Debug = 0,

            /// <summary>
            /// 普通信息日志（流程正常输出）
            /// </summary>
            Info,

            /// <summary>
            /// 警告日志（非关键性问题）
            /// </summary>
            Warning,

            /// <summary>
            /// 错误日志（功能异常）
            /// </summary>
            Error,

            /// <summary>
            /// 致命错误（可能崩溃）
            /// </summary>
            Fatal
        }
        #endregion

        #region Debug 级别日志
        /// <summary>
        /// 打印调试级别日志
        /// 仅在开启 ENABLE_LOG / ENABLE_DEBUG_LOG / ENABLE_DEBUG_AND_ABOVE_LOG 时生效
        /// </summary>
        /// <param name="message">日志内容</param>
        [Conditional("ENABLE_LOG")]
        [Conditional("ENABLE_DEBUG_LOG")]
        [Conditional("ENABLE_DEBUG_AND_ABOVE_LOG")]
        public static void Debug(object message)
        {
            RouteToUnityLogger(LogLevel.Debug, message);
        }

        /// <summary>
        /// 打印调试级别日志
        /// </summary>
        /// <param name="message">日志内容</param>
        [Conditional("ENABLE_LOG")]
        [Conditional("ENABLE_DEBUG_LOG")]
        [Conditional("ENABLE_DEBUG_AND_ABOVE_LOG")]
        public static void Debug(string message)
        {
            RouteToUnityLogger(LogLevel.Debug, message);
        }

        /// <summary>
        /// 格式化打印调试级别日志
        /// </summary>
        /// <param name="format">格式字符串</param>
        /// <param name="arg0">参数1</param>
        [Conditional("ENABLE_LOG")]
        [Conditional("ENABLE_DEBUG_LOG")]
        [Conditional("ENABLE_DEBUG_AND_ABOVE_LOG")]
        public static void Debug(string format, object arg0)
        {
            RouteToUnityLogger(LogLevel.Debug, AorTxt.Format(format, arg0));
        }

        /// <summary>
        /// 格式化打印调试级别日志
        /// </summary>
        /// <param name="format">格式字符串</param>
        /// <param name="arg0">参数1</param>
        /// <param name="arg1">参数2</param>
        [Conditional("ENABLE_LOG")]
        [Conditional("ENABLE_DEBUG_LOG")]
        [Conditional("ENABLE_DEBUG_AND_ABOVE_LOG")]
        public static void Debug(string format, object arg0, object arg1)
        {
            RouteToUnityLogger(LogLevel.Debug, AorTxt.Format(format, arg0, arg1));
        }

        /// <summary>
        /// 格式化打印调试级别日志
        /// </summary>
        /// <param name="format">格式字符串</param>
        /// <param name="arg0">参数1</param>
        /// <param name="arg1">参数2</param>
        /// <param name="arg2">参数3</param>
        [Conditional("ENABLE_LOG")]
        [Conditional("ENABLE_DEBUG_LOG")]
        [Conditional("ENABLE_DEBUG_AND_ABOVE_LOG")]
        public static void Debug(string format, object arg0, object arg1, object arg2)
        {
            RouteToUnityLogger(LogLevel.Debug, AorTxt.Format(format, arg0, arg1, arg2));
        }

        /// <summary>
        /// 格式化打印调试级别日志
        /// </summary>
        /// <param name="format">格式字符串</param>
        /// <param name="args">参数数组</param>
        [Conditional("ENABLE_LOG")]
        [Conditional("ENABLE_DEBUG_LOG")]
        [Conditional("ENABLE_DEBUG_AND_ABOVE_LOG")]
        public static void Debug(string format, params object[] args)
        {
            RouteToUnityLogger(LogLevel.Debug, AorTxt.Format(format, args));
        }
        #endregion

        #region Info 级别日志
        /// <summary>
        /// 打印信息级别日志
        /// </summary>
        /// <param name="message">日志内容</param>
        [Conditional("ENABLE_LOG")]
        [Conditional("ENABLE_INFO_LOG")]
        [Conditional("ENABLE_DEBUG_AND_ABOVE_LOG")]
        [Conditional("ENABLE_INFO_AND_ABOVE_LOG")]
        public static void Info(object message)
        {
            RouteToUnityLogger(LogLevel.Info, message);
        }

        /// <summary>
        /// 打印信息级别日志
        /// </summary>
        /// <param name="message">日志内容</param>
        [Conditional("ENABLE_LOG")]
        [Conditional("ENABLE_INFO_LOG")]
        [Conditional("ENABLE_DEBUG_AND_ABOVE_LOG")]
        [Conditional("ENABLE_INFO_AND_ABOVE_LOG")]
        public static void Info(string message)
        {
            RouteToUnityLogger(LogLevel.Info, message);
        }

        /// <summary>
        /// 格式化打印信息级别日志
        /// </summary>
        [Conditional("ENABLE_LOG")]
        [Conditional("ENABLE_INFO_LOG")]
        [Conditional("ENABLE_DEBUG_AND_ABOVE_LOG")]
        [Conditional("ENABLE_INFO_AND_ABOVE_LOG")]
        public static void Info(string format, object arg0)
        {
            RouteToUnityLogger(LogLevel.Info, AorTxt.Format(format, arg0));
        }

        /// <summary>
        /// 格式化打印信息级别日志
        /// </summary>
        [Conditional("ENABLE_LOG")]
        [Conditional("ENABLE_INFO_LOG")]
        [Conditional("ENABLE_DEBUG_AND_ABOVE_LOG")]
        [Conditional("ENABLE_INFO_AND_ABOVE_LOG")]
        public static void Info(string format, object arg0, object arg1)
        {
            RouteToUnityLogger(LogLevel.Info, AorTxt.Format(format, arg0, arg1));
        }

        /// <summary>
        /// 格式化打印信息级别日志
        /// </summary>
        [Conditional("ENABLE_LOG")]
        [Conditional("ENABLE_INFO_LOG")]
        [Conditional("ENABLE_DEBUG_AND_ABOVE_LOG")]
        [Conditional("ENABLE_INFO_AND_ABOVE_LOG")]
        public static void Info(string format, object arg0, object arg1, object arg2)
        {
            RouteToUnityLogger(LogLevel.Info, AorTxt.Format(format, arg0, arg1, arg2));
        }

        /// <summary>
        /// 格式化打印信息级别日志
        /// </summary>
        [Conditional("ENABLE_LOG")]
        [Conditional("ENABLE_INFO_LOG")]
        [Conditional("ENABLE_DEBUG_AND_ABOVE_LOG")]
        [Conditional("ENABLE_INFO_AND_ABOVE_LOG")]
        public static void Info(string format, params object[] args)
        {
            RouteToUnityLogger(LogLevel.Info, AorTxt.Format(format, args));
        }
        #endregion

        #region Warning 级别日志
        /// <summary>
        /// 打印警告级别日志
        /// 局部功能异常，不影响主流程
        /// </summary>
        [Conditional("ENABLE_LOG")]
        [Conditional("ENABLE_WARNING_LOG")]
        [Conditional("ENABLE_DEBUG_AND_ABOVE_LOG")]
        [Conditional("ENABLE_INFO_AND_ABOVE_LOG")]
        [Conditional("ENABLE_WARNING_AND_ABOVE_LOG")]
        public static void Warning(object message)
        {
            RouteToUnityLogger(LogLevel.Warning, message);
        }

        /// <summary>
        /// 打印警告级别日志
        /// </summary>
        [Conditional("ENABLE_LOG")]
        [Conditional("ENABLE_WARNING_LOG")]
        [Conditional("ENABLE_DEBUG_AND_ABOVE_LOG")]
        [Conditional("ENABLE_INFO_AND_ABOVE_LOG")]
        [Conditional("ENABLE_WARNING_AND_ABOVE_LOG")]
        public static void Warning(string message)
        {
            RouteToUnityLogger(LogLevel.Warning, message);
        }

        /// <summary>
        /// 格式化打印警告级别日志
        /// </summary>
        [Conditional("ENABLE_LOG")]
        [Conditional("ENABLE_WARNING_LOG")]
        [Conditional("ENABLE_DEBUG_AND_ABOVE_LOG")]
        [Conditional("ENABLE_INFO_AND_ABOVE_LOG")]
        [Conditional("ENABLE_WARNING_AND_ABOVE_LOG")]
        public static void Warning(string format, object arg0)
        {
            RouteToUnityLogger(LogLevel.Warning, AorTxt.Format(format, arg0));
        }

        /// <summary>
        /// 格式化打印警告级别日志
        /// </summary>
        [Conditional("ENABLE_LOG")]
        [Conditional("ENABLE_WARNING_LOG")]
        [Conditional("ENABLE_DEBUG_AND_ABOVE_LOG")]
        [Conditional("ENABLE_INFO_AND_ABOVE_LOG")]
        [Conditional("ENABLE_WARNING_AND_ABOVE_LOG")]
        public static void Warning(string format, object arg0, object arg1)
        {
            RouteToUnityLogger(LogLevel.Warning, AorTxt.Format(format, arg0, arg1));
        }

        /// <summary>
        /// 格式化打印警告级别日志
        /// </summary>
        [Conditional("ENABLE_LOG")]
        [Conditional("ENABLE_WARNING_LOG")]
        [Conditional("ENABLE_DEBUG_AND_ABOVE_LOG")]
        [Conditional("ENABLE_INFO_AND_ABOVE_LOG")]
        [Conditional("ENABLE_WARNING_AND_ABOVE_LOG")]
        public static void Warning(string format, object arg0, object arg1, object arg2)
        {
            RouteToUnityLogger(LogLevel.Warning, AorTxt.Format(format, arg0, arg1, arg2));
        }

        /// <summary>
        /// 格式化打印警告级别日志
        /// </summary>
        [Conditional("ENABLE_LOG")]
        [Conditional("ENABLE_WARNING_LOG")]
        [Conditional("ENABLE_DEBUG_AND_ABOVE_LOG")]
        [Conditional("ENABLE_INFO_AND_ABOVE_LOG")]
        [Conditional("ENABLE_WARNING_AND_ABOVE_LOG")]
        public static void Warning(string format, params object[] args)
        {
            RouteToUnityLogger(LogLevel.Warning, AorTxt.Format(format, args));
        }
        #endregion

        #region Error 级别日志
        /// <summary>
        /// 打印错误级别日志
        /// 功能逻辑异常，需要修复
        /// </summary>
        [Conditional("ENABLE_LOG")]
        [Conditional("ENABLE_ERROR_LOG")]
        [Conditional("ENABLE_DEBUG_AND_ABOVE_LOG")]
        [Conditional("ENABLE_INFO_AND_ABOVE_LOG")]
        [Conditional("ENABLE_WARNING_AND_ABOVE_LOG")]
        [Conditional("ENABLE_ERROR_AND_ABOVE_LOG")]
        public static void Error(object message)
        {
            RouteToUnityLogger(LogLevel.Error, message);
        }

        /// <summary>
        /// 打印错误级别日志
        /// </summary>
        [Conditional("ENABLE_LOG")]
        [Conditional("ENABLE_ERROR_LOG")]
        [Conditional("ENABLE_DEBUG_AND_ABOVE_LOG")]
        [Conditional("ENABLE_INFO_AND_ABOVE_LOG")]
        [Conditional("ENABLE_WARNING_AND_ABOVE_LOG")]
        [Conditional("ENABLE_ERROR_AND_ABOVE_LOG")]
        public static void Error(string message)
        {
            RouteToUnityLogger(LogLevel.Error, message);
        }

        /// <summary>
        /// 格式化打印错误级别日志
        /// </summary>
        [Conditional("ENABLE_LOG")]
        [Conditional("ENABLE_ERROR_LOG")]
        [Conditional("ENABLE_DEBUG_AND_ABOVE_LOG")]
        [Conditional("ENABLE_INFO_AND_ABOVE_LOG")]
        [Conditional("ENABLE_WARNING_AND_ABOVE_LOG")]
        [Conditional("ENABLE_ERROR_AND_ABOVE_LOG")]
        public static void Error(string format, object arg0)
        {
            RouteToUnityLogger(LogLevel.Error, AorTxt.Format(format, arg0));
        }

        /// <summary>
        /// 格式化打印错误级别日志
        /// </summary>
        [Conditional("ENABLE_LOG")]
        [Conditional("ENABLE_ERROR_LOG")]
        [Conditional("ENABLE_DEBUG_AND_ABOVE_LOG")]
        [Conditional("ENABLE_INFO_AND_ABOVE_LOG")]
        [Conditional("ENABLE_WARNING_AND_ABOVE_LOG")]
        [Conditional("ENABLE_ERROR_AND_ABOVE_LOG")]
        public static void Error(string format, object arg0, object arg1)
        {
            RouteToUnityLogger(LogLevel.Error, AorTxt.Format(format, arg0, arg1));
        }

        /// <summary>
        /// 格式化打印错误级别日志
        /// </summary>
        [Conditional("ENABLE_LOG")]
        [Conditional("ENABLE_ERROR_LOG")]
        [Conditional("ENABLE_DEBUG_AND_ABOVE_LOG")]
        [Conditional("ENABLE_INFO_AND_ABOVE_LOG")]
        [Conditional("ENABLE_WARNING_AND_ABOVE_LOG")]
        [Conditional("ENABLE_ERROR_AND_ABOVE_LOG")]
        public static void Error(string format, object arg0, object arg1, object arg2)
        {
            RouteToUnityLogger(LogLevel.Error, AorTxt.Format(format, arg0, arg1, arg2));
        }

        /// <summary>
        /// 格式化打印错误级别日志
        /// </summary>
        [Conditional("ENABLE_LOG")]
        [Conditional("ENABLE_ERROR_LOG")]
        [Conditional("ENABLE_DEBUG_AND_ABOVE_LOG")]
        [Conditional("ENABLE_INFO_AND_ABOVE_LOG")]
        [Conditional("ENABLE_WARNING_AND_ABOVE_LOG")]
        [Conditional("ENABLE_ERROR_AND_ABOVE_LOG")]
        public static void Error(string format, params object[] args)
        {
            RouteToUnityLogger(LogLevel.Error, AorTxt.Format(format, args));
        }
        #endregion

        #region Fatal 级别日志
        /// <summary>
        /// 打印致命错误级别日志
        /// 可能导致游戏崩溃
        /// </summary>
        [Conditional("ENABLE_LOG")]
        [Conditional("ENABLE_FATAL_LOG")]
        [Conditional("ENABLE_DEBUG_AND_ABOVE_LOG")]
        [Conditional("ENABLE_INFO_AND_ABOVE_LOG")]
        [Conditional("ENABLE_WARNING_AND_ABOVE_LOG")]
        [Conditional("ENABLE_ERROR_AND_ABOVE_LOG")]
        [Conditional("ENABLE_FATAL_AND_ABOVE_LOG")]
        public static void Fatal(object message)
        {
            RouteToUnityLogger(LogLevel.Fatal, message);
        }

        /// <summary>
        /// 打印致命错误级别日志
        /// </summary>
        [Conditional("ENABLE_LOG")]
        [Conditional("ENABLE_FATAL_LOG")]
        [Conditional("ENABLE_DEBUG_AND_ABOVE_LOG")]
        [Conditional("ENABLE_INFO_AND_ABOVE_LOG")]
        [Conditional("ENABLE_WARNING_AND_ABOVE_LOG")]
        [Conditional("ENABLE_ERROR_AND_ABOVE_LOG")]
        [Conditional("ENABLE_FATAL_AND_ABOVE_LOG")]
        public static void Fatal(string message)
        {
            RouteToUnityLogger(LogLevel.Fatal, message);
        }

        /// <summary>
        /// 格式化打印致命错误级别日志
        /// </summary>
        [Conditional("ENABLE_LOG")]
        [Conditional("ENABLE_FATAL_LOG")]
        [Conditional("ENABLE_DEBUG_AND_ABOVE_LOG")]
        [Conditional("ENABLE_INFO_AND_ABOVE_LOG")]
        [Conditional("ENABLE_WARNING_AND_ABOVE_LOG")]
        [Conditional("ENABLE_ERROR_AND_ABOVE_LOG")]
        [Conditional("ENABLE_FATAL_AND_ABOVE_LOG")]
        public static void Fatal(string format, object arg0)
        {
            RouteToUnityLogger(LogLevel.Fatal, AorTxt.Format(format, arg0));
        }

        /// <summary>
        /// 格式化打印致命错误级别日志
        /// </summary>
        [Conditional("ENABLE_LOG")]
        [Conditional("ENABLE_FATAL_LOG")]
        [Conditional("ENABLE_DEBUG_AND_ABOVE_LOG")]
        [Conditional("ENABLE_INFO_AND_ABOVE_LOG")]
        [Conditional("ENABLE_WARNING_AND_ABOVE_LOG")]
        [Conditional("ENABLE_ERROR_AND_ABOVE_LOG")]
        [Conditional("ENABLE_FATAL_AND_ABOVE_LOG")]
        public static void Fatal(string format, object arg0, object arg1)
        {
            RouteToUnityLogger(LogLevel.Fatal, AorTxt.Format(format, arg0, arg1));
        }

        /// <summary>
        /// 格式化打印致命错误级别日志
        /// </summary>
        [Conditional("ENABLE_LOG")]
        [Conditional("ENABLE_FATAL_LOG")]
        [Conditional("ENABLE_DEBUG_AND_ABOVE_LOG")]
        [Conditional("ENABLE_INFO_AND_ABOVE_LOG")]
        [Conditional("ENABLE_WARNING_AND_ABOVE_LOG")]
        [Conditional("ENABLE_ERROR_AND_ABOVE_LOG")]
        [Conditional("ENABLE_FATAL_AND_ABOVE_LOG")]
        public static void Fatal(string format, object arg0, object arg1, object arg2)
        {
            RouteToUnityLogger(LogLevel.Fatal, AorTxt.Format(format, arg0, arg1, arg2));
        }

        /// <summary>
        /// 格式化打印致命错误级别日志
        /// </summary>
        [Conditional("ENABLE_LOG")]
        [Conditional("ENABLE_FATAL_LOG")]
        [Conditional("ENABLE_DEBUG_AND_ABOVE_LOG")]
        [Conditional("ENABLE_INFO_AND_ABOVE_LOG")]
        [Conditional("ENABLE_WARNING_AND_ABOVE_LOG")]
        [Conditional("ENABLE_ERROR_AND_ABOVE_LOG")]
        [Conditional("ENABLE_FATAL_AND_ABOVE_LOG")]
        public static void Fatal(string format, params object[] args)
        {
            RouteToUnityLogger(LogLevel.Fatal, AorTxt.Format(format, args));
        }
        #endregion

        #region 内部日志实现
        /// <summary>
        /// 各等级的文本装饰器：在追加帧号前对原文做着色等处理
        /// </summary>
        private static readonly Func<string, string>[] LevelDecorators = BuildLevelDecorators();

        /// <summary>
        /// 各等级最终写入 Unity 控制台的委托
        /// </summary>
        private static readonly Action<string>[] LevelWriters = BuildLevelWriters();

        /// <summary>
        /// 构建等级装饰表（与 LogLevel 数值一一对应）
        /// </summary>
        private static Func<string, string>[] BuildLevelDecorators()
        {
            return new Func<string, string>[]
            {
                raw => AorTxt.Format("<color=#888888>{0}</color>", raw), // Debug：灰色
                raw => raw,                                              // Info
                raw => raw,                                              // Warning
                raw => raw,                                              // Error
            };
        }

        /// <summary>
        /// 构建等级写入表（与 LogLevel 数值一一对应）
        /// </summary>
        private static Action<string>[] BuildLevelWriters()
        {
            return new Action<string>[]
            {
                line => UnityEngine.Debug.Log(line),        // Debug
                line => UnityEngine.Debug.Log(line),        // Info
                line => UnityEngine.Debug.LogWarning(line), // Warning
                line => UnityEngine.Debug.LogError(line),   // Error
            };
        }

        /// <summary>
        /// 日志内部实现方法：按等级查表分发到对应的 Unity 写入接口
        /// </summary>
        /// <param name="level">日志等级</param>
        /// <param name="message">日志内容</param>
        private static void RouteToUnityLogger(LogLevel level, object message)
        {
            string rawText = message.ToString();
            int slot = (int)level;

            if (slot < 0 || slot >= LevelDecorators.Length)
            {
                // Fatal 及其它未定义等级：直接抛出游戏异常
                throw new GameException(DecorateWithFrameTag(rawText));
            }

            string colored = LevelDecorators[slot](rawText);
            string line = DecorateWithFrameTag(colored);
            LevelWriters[slot](line);
        }

        /// <summary>
        /// 为日志添加帧计数格式化（便于定位时序问题）
        /// </summary>
        /// <param name="rawText">原始日志</param>
        /// <returns>带帧号的格式化日志</returns>
        private static string DecorateWithFrameTag(string rawText)
        {
            int currentFrame = Time.frameCount;
            return AorTxt.Format("<color=#30F5FB>[{0}]</color>,{1}", currentFrame, rawText);
        }
        #endregion
    }
}
