/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  DebuggerComponent.cs
 * author:    taoye
 * created:   2020/8/26
 * descrip:   调试组件
 ***************************************************************/
using System;
using UnityEngine;

namespace Honor.Runtime
{
    public sealed partial class DebuggerComponent : GameComponent
    {
        [Serializable]
        /// <summary>
        /// 日志记录结点。
        /// </summary>
        public sealed class LogNode
        {
            private int m_LogFrameCount;
            public string LogTime;
            public LogType LogType;
            public string LogMessage;
            public string StackTrack;
            // log中的UI信息
            public float LogOrderY = 0;
            public Rect LogRect = Rect.zero;
            public ulong LogIndex = 0;

            /// <summary>
            /// 初始化日志记录结点的新实例。
            /// </summary>
            public LogNode()
            {
                m_LogFrameCount = 0;
                LogTime = null;
                LogType = LogType.Error;
                LogMessage = null;
                StackTrack = null;
            }

            /// <summary>
            /// 获取日志帧计数。
            /// </summary>
            public int LogFrameCount
            {
                get
                {
                    return m_LogFrameCount;
                }
            }

            /// <summary>
            /// 创建日志记录结点。
            /// </summary>
            /// <param name="logType">日志类型。</param>
            /// <param name="logMessage">日志内容。</param>
            /// <param name="stackTrack">日志堆栈信息。</param>
            /// <returns>创建的日志记录结点。</returns>
            public static LogNode Create(DebuggerComponent debugComponent, LogType logType, string logMessage, string stackTrack, ulong logIndex)
            {
                LogNode logNode = new LogNode();
                logNode.m_LogFrameCount = Time.frameCount;
                logNode.LogTime = DateTime.Now.ToString("HH:mm:ss.fff");
                logNode.LogType = logType;
                logNode.LogIndex = logIndex;
                logNode.LogMessage = $"<color=#00FFFB>● [{logIndex}] [{logNode.LogTime}] [{Time.frameCount}] </color><color=#{ColorUtility.ToHtmlStringRGBA(GetLogStringColor(debugComponent, logType))}> {logMessage}</color>";
                logNode.StackTrack = stackTrack;
                logNode.RefreshCount();
                return logNode;
            }

            /// <summary>
            /// 创建log节点，适配多线程log调用
            /// 子线程不能调用主线程的Time接口，所以直接传入frameCount
            /// </summary>
            /// <param name="logType"></param>
            /// <param name="logMessage"></param>
            /// <param name="stackTrack"></param>
            /// <param name="frameCount"></param>
            /// <returns></returns>
            // by liuyu
            public static LogNode CreateThreadedNode(DebuggerComponent debugComponent, LogType logType, string logMessage, string stackTrack, int frameCount, ulong logIndex)
            {
                LogNode logNode = new LogNode();
                logNode.m_LogFrameCount = frameCount;
                logNode.LogTime = DateTime.Now.ToString("HH:mm:ss.fff");
                logNode.LogType = logType;
                logNode.LogMessage = $"<color=#00FFFB>● [{logIndex}] [{logNode.LogTime}] [{frameCount}] </color><color=#{ColorUtility.ToHtmlStringRGBA(GetLogStringColor(debugComponent, logType))}> {logMessage}</color>";
                logNode.StackTrack = stackTrack;
                logNode.RefreshCount();
                return logNode;
            }

            /// <summary>
            /// 清理日志记录结点。
            /// </summary>
            public void Clear()
            {
                m_LogFrameCount = 0;
                LogTime = null;
                LogType = LogType.Error;
                LogMessage = null;
                StackTrack = null;
            }

            public void RefreshCount()
            {
                switch (LogType)
                {
                    case LogType.Log:
                        s_InfoCount++;
                        break;

                    case LogType.Warning:
                        s_WarningCount++;
                        break;

                    case LogType.Error:
                        s_ErrorCount++;
                        break;

                    case LogType.Exception:
                        s_FatalCount++;
                        break;
                }
            }
            
            /// <summary>
            /// 获取不同Log类型对应的颜色
            /// </summary>
            /// <param name="logType"></param>
            /// <param name="idleModel"></param>
            /// <returns></returns>
            public static Color32 GetLogStringColor(DebuggerComponent debugComponent, LogType logType, bool idleModel = false)
            {
                Color32 color = Color.white;
                switch (logType)
                {
                    case LogType.Log:
                        color = debugComponent.InfoColor;
                        break;

                    case LogType.Warning:
                        color = debugComponent.WarningColor;
                        break;

                    case LogType.Error:
                        color = debugComponent.ErrorColor;
                        break;

                    case LogType.Exception:
                        color = debugComponent.FatalColor;
                        break;
                }
                color.a = 255;
                if (idleModel)
                {
                    color.a = 80;
                }
                return color;
            }


            /// <summary>
            /// 普通信息数量
            /// </summary>
            private static int s_InfoCount = 0;
            public static int InfoCount { get => s_InfoCount; }

            /// <summary>
            /// 警告信息数量
            /// </summary>
            private static int s_WarningCount = 0;
            public static int WarningCount { get => s_WarningCount; }

            /// <summary>
            /// 错误信息数量
            /// </summary>
            private static int s_ErrorCount = 0;
            public static int ErrorCount { get => s_ErrorCount; }

            /// <summary>
            /// 严重信息数量
            /// </summary>
            private static int s_FatalCount = 0;
            public static int FatalCount { get => s_FatalCount; }

            /// <summary>
            /// 重置信息数量
            /// </summary>
            public static void Reset()
            {
                s_InfoCount = 0;
                s_WarningCount = 0;
                s_ErrorCount = 0;
                s_FatalCount = 0;
            }
        }
    }
}


