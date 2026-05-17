/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  Log.cs
 * author:    云毅
 * created:   2026
 * descrip:   游戏全局分级日志工具，支持条件编译剥离，带帧计数输出
 ***************************************************************/
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
            InternalLog(LogLevel.Debug, message);
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
            InternalLog(LogLevel.Debug, message);
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
            InternalLog(LogLevel.Debug, AorTxt.Format(format, arg0));
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
            InternalLog(LogLevel.Debug, AorTxt.Format(format, arg0, arg1));
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
            InternalLog(LogLevel.Debug, AorTxt.Format(format, arg0, arg1, arg2));
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
            InternalLog(LogLevel.Debug, AorTxt.Format(format, args));
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
            InternalLog(LogLevel.Info, message);
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
            InternalLog(LogLevel.Info, message);
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
            InternalLog(LogLevel.Info, AorTxt.Format(format, arg0));
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
            InternalLog(LogLevel.Info, AorTxt.Format(format, arg0, arg1));
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
            InternalLog(LogLevel.Info, AorTxt.Format(format, arg0, arg1, arg2));
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
            InternalLog(LogLevel.Info, AorTxt.Format(format, args));
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
            InternalLog(LogLevel.Warning, message);
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
            InternalLog(LogLevel.Warning, message);
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
            InternalLog(LogLevel.Warning, AorTxt.Format(format, arg0));
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
            InternalLog(LogLevel.Warning, AorTxt.Format(format, arg0, arg1));
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
            InternalLog(LogLevel.Warning, AorTxt.Format(format, arg0, arg1, arg2));
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
            InternalLog(LogLevel.Warning, AorTxt.Format(format, args));
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
            InternalLog(LogLevel.Error, message);
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
            InternalLog(LogLevel.Error, message);
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
            InternalLog(LogLevel.Error, AorTxt.Format(format, arg0));
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
            InternalLog(LogLevel.Error, AorTxt.Format(format, arg0, arg1));
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
            InternalLog(LogLevel.Error, AorTxt.Format(format, arg0, arg1, arg2));
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
            InternalLog(LogLevel.Error, AorTxt.Format(format, args));
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
            InternalLog(LogLevel.Fatal, message);
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
            InternalLog(LogLevel.Fatal, message);
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
            InternalLog(LogLevel.Fatal, AorTxt.Format(format, arg0));
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
            InternalLog(LogLevel.Fatal, AorTxt.Format(format, arg0, arg1));
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
            InternalLog(LogLevel.Fatal, AorTxt.Format(format, arg0, arg1, arg2));
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
            InternalLog(LogLevel.Fatal, AorTxt.Format(format, args));
        }
        #endregion

        #region 内部日志实现
        /// <summary>
        /// 日志内部实现方法
        /// 根据等级输出不同颜色与类型的日志
        /// </summary>
        /// <param name="level">日志等级</param>
        /// <param name="message">日志内容</param>
        private static void InternalLog(LogLevel level, object message)
        {
            switch (level)
            {
                case LogLevel.Debug:
                    UnityEngine.Debug.Log(FormattingFrameCount(AorTxt.Format("<color=#888888>{0}</color>", message.ToString())));
                    break;

                case LogLevel.Info:
                    UnityEngine.Debug.Log(FormattingFrameCount(message.ToString()));
                    break;

                case LogLevel.Warning:
                    UnityEngine.Debug.LogWarning(FormattingFrameCount(message.ToString()));
                    break;

                case LogLevel.Error:
                    UnityEngine.Debug.LogError(FormattingFrameCount(message.ToString()));
                    break;

                default:
                    throw new GameException(FormattingFrameCount(message.ToString()));
            }
        }

        /// <summary>
        /// 为日志添加帧计数格式化（便于定位时序问题）
        /// </summary>
        /// <param name="msg">原始日志</param>
        /// <returns>带帧号的格式化日志</returns>
        private static string FormattingFrameCount(string msg)
        {
            int frameCount = Time.frameCount;
            return AorTxt.Format("<color=#30F5FB>[{0}]</color>,{1}", frameCount, msg);
        }
        #endregion
    }
}