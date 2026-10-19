/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  DebuggerComponent.cs
 * author:    taoye
 * created:   2020/8/26
 * descrip:   调试组件
 ***************************************************************/
using BestHTTP;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json.Linq;
using UnityEngine;
using DG.Tweening;

namespace Honor.Runtime
{
    public sealed partial class DebuggerComponent : GameComponent
    {
        [Serializable]
        public sealed class ConsoleWindow : IDebuggerWindow
        {
            private LogNode m_SelectedNode = null;
            public LogNode SelectedNode
            { 
                get 
                { 
                    return m_SelectedNode; 
                } 
                set
                {
                    m_SelectedNode = value; 
                }
            }

            private class LogNodeJson
            {
                public List<LogNode> AllLogNodes = new List<LogNode>();
            }

            private readonly LogNodeJson m_LogNodeJson = new LogNodeJson();
            private readonly List<int> m_InFilterLogs = new List<int>();
            private readonly string m_StartTime = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
            private ulong m_LogTotalIdx = 1;
            // log总高度
            private float m_AllLogHeight = 0;
            // 新加入的等待在draw接口中获取高度的log索引
            private List<int> m_WaitingCalcHeightIdxs = new List<int>();
            private int m_FliterLogCount = 0;
            private float m_InDrawBorder = 0;
            public List<int> m_InShowLogs = new List<int>();
            // 当前滚动框拖动比例
            private float m_ScrollValue = 0;
            // 拖动前滚动框比例
            private float m_CurScrollValue = 0;
            private bool m_InBarModify = false;
            // 当前滚动滑块大小
            private float m_ScrollBarSize = 0;
            // 拖动滚动框和拖动滚动条最终都会修改m_DragOffsetY
            private float m_DragOffsetY = 0;
            private float m_LastDragOffsetY = 0;
            // 每条Log之间的空格
            private float m_LogInv = 5;
            private float m_SpeedBase = 35;
            // 当前Log窗口搜索字符串
            private string m_InputSearchInfo = null;
            // 上一个Log窗口搜索字符串
            private string m_LastInputSearchInfo = null;
            // 是否正处于搜索状态
            private bool m_InSearch = false;

            private Dictionary<LogType, int> m_SearchLogCount = new Dictionary<LogType, int>();

            private Dictionary<LogType, bool> m_LastFilterState = new Dictionary<LogType, bool>();

            private Dictionary<LogType, bool> m_CurFilterState = new Dictionary<LogType, bool>();

            private Dictionary<LogType, string> m_LogSaveKeys = new Dictionary<LogType, string>();

            private static LogType[] m_ShowLogTypes = new LogType[] { LogType.Log, LogType.Warning, LogType.Error, LogType.Exception };

            private PersistComponent m_PersistComponent = null;

            private DebuggerComponent m_DebugComponent = null;
            // 是否锁定Log
            private bool m_LastLockScroll = true;
            private bool m_LockScroll = true;
            /// <summary>
            /// log队列写入和读取互斥锁 by liuyu
            /// </summary>
            private static readonly object m_LogqueueLocker = new object();
            /// <summary>
            /// 当前帧数，记录log发生的时机 by liuyu
            /// </summary>
            private int m_FrameCount = 0;

            private int m_MaxLine = int.MaxValue - 1;

            public bool LockScroll { get => m_LockScroll; set => m_LockScroll = value; }

            public int MaxLine { get => m_MaxLine; set => m_MaxLine = value; }


            private string m_UploadButtonWords = "上传日志";
			
            public void Initialize(DebuggerComponent rootDebug, params object[] args)
            {
                m_DebugComponent = rootDebug;
                // by liuyu
                //  Application.logMessageReceived += OnLogMessageReceived;
                Application.logMessageReceivedThreaded += OnLogMessageThreadReceived;
                // 初始化数据
                ResetSearchLogCount();
                // 初始化运行时Log开关状态
                InitLogShowState();
                // 设置存档关键字
                InitLogSaveKeys();
            }

            /// <summary>
            /// 重置当前Log显示状态
            /// </summary>
            private void InitLogShowState()
            {
                m_LastFilterState.Clear();
                m_LastFilterState.Add(LogType.Error, true);
                m_LastFilterState.Add(LogType.Assert, true);
                m_LastFilterState.Add(LogType.Warning, true);
                m_LastFilterState.Add(LogType.Log, true);
                m_LastFilterState.Add(LogType.Exception, true);
                m_CurFilterState.Clear();
                m_CurFilterState.Add(LogType.Error, true);
                m_CurFilterState.Add(LogType.Assert, true);
                m_CurFilterState.Add(LogType.Warning, true);
                m_CurFilterState.Add(LogType.Log, true);
                m_CurFilterState.Add(LogType.Exception, true);
            }

            /// <summary>
            /// 设置存档关键字
            /// </summary>
            private void InitLogSaveKeys()
            {
                m_LogSaveKeys.Clear();
                m_LogSaveKeys.Add(LogType.Error, "ErrorFilter");
                m_LogSaveKeys.Add(LogType.Assert, "ErrorFilter");
                m_LogSaveKeys.Add(LogType.Warning, "WarningFilter");
                m_LogSaveKeys.Add(LogType.Log, "InfoFilter");
                m_LogSaveKeys.Add(LogType.Exception, "DebuggerConsole");
            }

            /// <summary>
            /// 从存档中读取log显示状态并进行显示列表的初始化
            /// </summary>
            public void InitLogShowStateFromSaveData()
            {
                if (m_PersistComponent == null)
                {
                    m_PersistComponent = GameComponentsGroup.GetComponent<PersistComponent>();
                    if (m_PersistComponent != null)
                    {
                        m_LockScroll = m_LastLockScroll = m_PersistComponent.GetBool(PersistWayType.FileFragment, "DebuggerConsole", "LockScroll", true);

                        for (int i = 0; i < m_ShowLogTypes.Length; i++)
                        {
                            m_CurFilterState[m_ShowLogTypes[i]] = m_LastFilterState[m_ShowLogTypes[i]] = m_PersistComponent.GetBool(PersistWayType.FileFragment, "DebuggerConsole", m_LogSaveKeys[m_ShowLogTypes[i]], true);
                        }
                    }
                }
            }


            public void Shutdown()
            {
                // by liuyu
                //  Application.logMessageReceived -= OnLogMessageReceived;
                Application.logMessageReceivedThreaded -= OnLogMessageThreadReceived;
                SaveLogFile();
                Clear();
            }

            public void SaveLogFile()
            {
#if UNITY_EDITOR
                StringBuilder path = new StringBuilder(System.IO.Path.GetFullPath(".") + "/Library/LogFiles");
                if (!System.IO.Directory.Exists(path.ToString()))
                {
                    System.IO.Directory.CreateDirectory(path.ToString());
                }
                using (FileStream fs = new FileStream(path.AppendFormat("/log@{0}.json", m_StartTime).ToString(), FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    var json = JsonConvert.SerializeObject(m_LogNodeJson);
                    var bytes = System.Text.Encoding.UTF8.GetBytes(json);
                    fs.Write(bytes, 0, bytes.Length);
                }
#endif
            }


            public void OnEnter()
            {
            }

            public void OnLeave()
            {
                
            }

            public void OnUpdate(float elapseSeconds, float realElapseSeconds)
            {
                m_FrameCount = Time.frameCount;
            }

            /// <summary>
            /// 获取当前log节点渲染高度
            /// </summary>
            /// <param name="message"></param>
            /// <returns></returns>
            public float CalcLogHeight(string message, float baseOnWidth)
            {
                GUIStyle style = new GUIStyle(GUI.skin.label);
                style.richText = true;

                float newHeight = style.CalcHeight(new GUIContent(message), baseOnWidth);
                return newHeight;
            }

            /// <summary>
            /// 更新Log显示状态
            /// </summary>
            /// <param name="checkType"></param>
            public void UpdateLogShowState(LogType checkType)
            {
                if (m_CurFilterState[checkType] != m_LastFilterState[checkType])
                {
                    GameMainRoot.Debugger.ControlInput();
                    m_LastFilterState[checkType] = m_CurFilterState[checkType];
                    m_PersistComponent.SetBool(PersistWayType.FileFragment, "DebuggerConsole", m_LogSaveKeys[checkType], m_CurFilterState[checkType]);
                    m_PersistComponent.Save(PersistWayType.FileFragment);
                    ModefyShowLogsAndResetTotalHeight();
                }
            }

            /// <summary>
            /// 修改在滚动框中要显示的log类型
            /// </summary>
            /// <param name="logType"></param>
            /// <param name="showState"></param>
            public void SaveLogShowSetting(string logType, bool showState)
            {
                m_PersistComponent.SetBool(PersistWayType.FileFragment, "DebuggerConsole", logType, showState);
                m_PersistComponent.Save(PersistWayType.FileFragment);
            }

            /// <summary>
            /// 是否包含搜索字符串
            /// </summary>
            /// <param name="checkLog"></param>
            /// <returns></returns>
            public bool CanShowLogBySearchInfo(LogNode checkLog)
            {
                if (checkLog != null && !string.IsNullOrEmpty(checkLog.LogMessage) && checkLog.LogMessage.IndexOf(m_InputSearchInfo, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return true;
                }
                return false;
            }

            /// <summary>
            /// 重置Log显示区域的拖拽偏移值
            /// </summary>
            private void ResetOffsetHeight()
            {
                if (m_AllLogHeight > GameMainRoot.Debugger.LogScrollRect.height)
                {
                    if (m_DragOffsetY < (GameMainRoot.Debugger.LogScrollRect.height - m_AllLogHeight))
                    {
                        m_DragOffsetY = GameMainRoot.Debugger.LogScrollRect.height - m_AllLogHeight;
                    }
                    else if (m_DragOffsetY > 0)
                    {
                        m_DragOffsetY = 0;
                    }
                }
                else
                {
                    m_DragOffsetY = 0;
                }
            }

            /// <summary>
            /// 当选中要显示的log类型发生变动时，重置log数据
            /// </summary>
            public void ModefyShowLogsAndResetTotalHeight()
            {
                m_InFilterLogs.Clear();
                ResetSearchLogCount();
                // 重置总高度
                m_AllLogHeight = 0;
                // 是否需要过滤搜索字符串
                bool needCheckSearchInfo = !string.IsNullOrEmpty(m_LastInputSearchInfo);
                for (int i = 0; i < m_LogNodeJson.AllLogNodes.Count; i++)
                {
                    if (m_CurFilterState[m_LogNodeJson.AllLogNodes[i].LogType])
                    {
                        if (!needCheckSearchInfo || (needCheckSearchInfo && CanShowLogBySearchInfo(m_LogNodeJson.AllLogNodes[i])))
                        {
                            m_InFilterLogs.Add(i);
                            m_LogNodeJson.AllLogNodes[i].LogRect.y = m_AllLogHeight;
                            m_LogNodeJson.AllLogNodes[i].LogOrderY = m_AllLogHeight;
                            m_AllLogHeight += m_LogNodeJson.AllLogNodes[i].LogRect.height;
                            m_SearchLogCount[m_LogNodeJson.AllLogNodes[i].LogType]++;
                        }
                    }
                }
                m_ScrollBarSize = GameMainRoot.Debugger.LogScrollBarHeight;
                if (GameMainRoot.Debugger.LogScrollRect.height < m_AllLogHeight)
                {
                    m_ScrollBarSize = GameMainRoot.Debugger.LogScrollBarHeight * GameMainRoot.Debugger.LogScrollRect.height / m_AllLogHeight;
                    if (m_DebugComponent.Portrail)
                    {
                        m_ScrollBarSize = Mathf.Clamp(m_ScrollBarSize, 100, GameMainRoot.Debugger.LogScrollBarHeight);
                    }
                    else
                    {
                        m_ScrollBarSize = Mathf.Clamp(m_ScrollBarSize, 80, GameMainRoot.Debugger.LogScrollBarHeight);
                    }
                }
                ResetOffsetHeight();
            }

            /// <summary>
            /// 获取当前Log数量信息
            /// </summary>
            /// <returns></returns>
            public string GetLogCountInfo(LogType curLogType)
            {
                bool needCheckSearchInfo = !string.IsNullOrEmpty(m_LastInputSearchInfo);
                switch (curLogType)
                {
                    case LogType.Error:
                        if (needCheckSearchInfo)
                        {
                            return $"Error ({m_SearchLogCount[curLogType]}/{LogNode.ErrorCount})";
                        }
                        else
                        {
                            return $"Error ({LogNode.ErrorCount})";
                        }
                        break;
                    case LogType.Assert:
                        if (needCheckSearchInfo)
                        {
                            return $"Error ({m_SearchLogCount[curLogType]}/{LogNode.ErrorCount})";
                        }
                        else
                        {
                            return $"Error ({LogNode.ErrorCount})";
                        }
                        break;
                    case LogType.Warning:
                        if (needCheckSearchInfo)
                        {
                            return $"Warn ({m_SearchLogCount[curLogType]}/{LogNode.WarningCount})";
                        }
                        else
                        {
                            return $"Warn ({LogNode.WarningCount})";
                        }
                        break;
                    case LogType.Log:
                        if (needCheckSearchInfo)
                        {
                            return $"Info ({m_SearchLogCount[curLogType]}/{LogNode.InfoCount})";
                        }
                        else
                        {
                            return $"Info ({LogNode.InfoCount})";
                        }
                        break;
                    case LogType.Exception:
                        if (needCheckSearchInfo)
                        {
                            return $"Fatal ({m_SearchLogCount[curLogType]}/{LogNode.FatalCount})";
                        }
                        else
                        {
                            return $"Fatal ({LogNode.FatalCount})";
                        }
                        break;
                    default:
                        break;
                }
                return "";
            }


            /// <summary>
            /// 开始搜索
            /// </summary>
            public void StartSearch()
            {
                if (!string.IsNullOrEmpty(m_InputSearchInfo) && m_InputSearchInfo != m_LastInputSearchInfo)
                {
                    m_LastInputSearchInfo = m_InputSearchInfo;
                    m_InSearch = true;
                    ModefyShowLogsAndResetTotalHeight();
                }
            }

            /// <summary>
            /// 重置搜索，回复正常显示
            /// </summary>
            public void ResetSearch()
            {
                m_InSearch = false;
                m_LastInputSearchInfo = "";
                m_InputSearchInfo = "";
                ModefyShowLogsAndResetTotalHeight();
            }

            /// <summary>
            /// 显示Log搜索功能
            /// </summary>
            /// <param name="debugComponent"></param>
            /// <param name="offsetY"></param>
            public void OnDrawSearch(DebuggerComponent debugComponent, float offsetY, float showWidth, float Inv)
            {
                Rect inputRect = GUILayoutUtility.GetRect(new GUIContent(""), new GUIStyle() { fontSize = GUI.skin.button.fontSize, alignment = TextAnchor.MiddleLeft, fixedHeight = GUI.skin.button.fixedHeight });
                m_InputSearchInfo = GUI.TextField(inputRect, m_InputSearchInfo);
                GUILayout.Space(Inv);

                if (GUILayout.Button("搜索", new GUILayoutOption[] { GUILayout.Width(showWidth) }))
                {
                    debugComponent.ControlInput();
                    StartSearch();
                    GUIUtility.ExitGUI();
                }

                // 在搜索状态下，清空输入框，自动回到正常显示状态
                if (m_InSearch && string.IsNullOrEmpty(m_InputSearchInfo))
                {
                    ResetSearch();
                }
            }

            /// <summary>
            /// 显示清空日志功能
            /// </summary>
            /// <param name="debugComponent"></param>
            /// <param name="offsetY"></param>
            public void OnDrawClearLog(DebuggerComponent debugComponent, float offsetY)
            {
                if (GUILayout.Button("清空日志", new GUILayoutOption[] { GUILayout.Width(debugComponent.FullWinMaxWidth * 0.25f)}))
                {
                    debugComponent.ControlInput();
                    Clear();
                    GUIUtility.ExitGUI();
                }
            }

            /// <summary>
            /// 显示上传日志功能
            /// </summary>
            /// <param name="debugComponent"></param>
            /// <param name="offsetY"></param>
            public void OnDrawUploadLog(DebuggerComponent debugComponent, float offsetY)
            {
                if (GUILayout.Button(m_UploadButtonWords))
                {
                    debugComponent.ControlInput();
                    UploadLog(debugComponent);
                    GUIUtility.ExitGUI();
                }
            }
            
            /// <summary>
            /// 显示滚动区域滚动锁定
            /// </summary>
            public void OnDrawLockScroll(DebuggerComponent debugComponent, float offsetY)
            {
                m_LockScroll = GUILayout.Toggle(m_LockScroll, "Lock Scroll", "ToolToggle", GUILayout.Height(GUI.skin.button.fixedHeight));
                if (m_LastLockScroll != m_LockScroll)
                {
                    debugComponent.ControlInput();
                    m_LastLockScroll = m_LockScroll;
                    m_PersistComponent.SetBool(PersistWayType.FileFragment, "DebuggerConsole", "LockScroll", m_LockScroll);
                    m_PersistComponent.Save(PersistWayType.FileFragment);
                    ModefyShowLogsAndResetTotalHeight();
                }
            }

            /// <summary>
            /// 显示指定的日志开关
            /// </summary>
            public void OnDrawSpecifiedLog(DebuggerComponent debugComponent, float offsetY, LogType showType)
            {
                m_CurFilterState[showType] = GUILayout.Toggle(m_CurFilterState[showType], GetLogCountInfo(showType), "ToolToggle", new GUILayoutOption[] { GUILayout.Height(GUI.skin.button.fixedHeight)});
                UpdateLogShowState(showType);
            }

            /// <summary>
            /// 显示所有Log开关Toggle
            /// </summary>
            /// <param name="debugComponent"></param>
            /// <param name="offsetY"></param>
            public void OnDrawLogs(DebuggerComponent debugComponent, float offsetY)
            {
                // 显示所有Log开关
                for (int i = 0; i < m_ShowLogTypes.Length; i++)
                {
                    m_CurFilterState[m_ShowLogTypes[i]] = GUILayout.Toggle(m_CurFilterState[m_ShowLogTypes[i]], GetLogCountInfo(m_ShowLogTypes[i]), "ToolToggle", GUILayout.Height(debugComponent.m_CustomButtonHeight));
                    UpdateLogShowState(m_ShowLogTypes[i]);
                }
            }

            /// <summary>
            /// 计算滚动条位置
            /// </summary>
            public void CalcScrollValue()
            {
                m_ScrollValue = -m_DragOffsetY / (m_AllLogHeight - m_DebugComponent.LogScrollRect.height) * (m_DebugComponent.LogScrollBarHeight - m_ScrollBarSize) + m_DebugComponent.LogScrollBarTop;
                m_ScrollValue = Mathf.Clamp(m_ScrollValue, m_DebugComponent.LogScrollBarTop, m_DebugComponent.LogScrollBarBottom);
            }

            public void OnDraw(DebuggerComponent debugComponent, float ScrollOffsetY)
            {
                // 首先计算新添加的log的高度
                if (m_WaitingCalcHeightIdxs.Count > 0)
                {
                    for (int i = 0; i < m_WaitingCalcHeightIdxs.Count; i++)
                    {
                        float curLogHeight = CalcLogHeight(m_LogNodeJson.AllLogNodes[m_WaitingCalcHeightIdxs[i]].LogMessage, debugComponent.LogScrollRect.width);
                        m_LogNodeJson.AllLogNodes[m_WaitingCalcHeightIdxs[i]].LogRect.height = curLogHeight + m_LogInv;
                        m_LogNodeJson.AllLogNodes[m_WaitingCalcHeightIdxs[i]].LogRect.width = debugComponent.LogScrollRect.width;
                    }
                    m_WaitingCalcHeightIdxs.Clear();
                    ModefyShowLogsAndResetTotalHeight();
                    // 重置滚动条
                    CalcScrollValue();
                }

                // 从存档中获取开关状态
                InitLogShowStateFromSaveData();

                lock (m_LogqueueLocker)
                {
                    GUILayout.BeginVertical("box");
                    {
                        if (debugComponent.Portrail)
                        {
                            // 搜索
                            GUILayout.BeginHorizontal("box");
                            OnDrawSearch(debugComponent, ScrollOffsetY, debugComponent.FullWinMaxWidth * 0.3f, 10);
                            GUILayout.EndHorizontal();
                            GUILayout.Space(10);

                            // 清空日志和滚动锁定
                            GUILayout.BeginHorizontal("box");
                            OnDrawLockScroll(debugComponent, ScrollOffsetY);
                            OnDrawSpecifiedLog(debugComponent, ScrollOffsetY, LogType.Log);
                            OnDrawClearLog(debugComponent, ScrollOffsetY);
							OnDrawUploadLog(debugComponent, ScrollOffsetY);
                            GUILayout.EndHorizontal();
                            GUILayout.Space(10);

                            // 日志开关
                            GUILayout.BeginHorizontal("box");
                            OnDrawSpecifiedLog(debugComponent, ScrollOffsetY, LogType.Warning);
                            OnDrawSpecifiedLog(debugComponent, ScrollOffsetY, LogType.Error);
                            OnDrawSpecifiedLog(debugComponent, ScrollOffsetY, LogType.Exception);
                            GUILayout.EndHorizontal();
                        }
                        else
                        {
                            // 搜索和清空日志
                            GUILayout.BeginHorizontal();
                            OnDrawSearch(debugComponent, ScrollOffsetY, debugComponent.FullWinMaxWidth * 0.25f, 0);
                            OnDrawClearLog(debugComponent, ScrollOffsetY);
							OnDrawUploadLog(debugComponent, ScrollOffsetY);
                            GUILayout.EndHorizontal();
                            GUILayout.Space(10);

                            // 滚动锁定和日志开关
                            GUILayout.BeginHorizontal();
                            OnDrawLockScroll(debugComponent, ScrollOffsetY);
                            OnDrawLogs(debugComponent, ScrollOffsetY);
                            GUILayout.EndHorizontal();
                        }
                    }
                    GUILayout.EndVertical();

                    bool isDragMofidy = false;
                    // 计算并开始渲染log区域
                    {
                        //if (debugComponent.FullWindowInDrag && debugComponent.WinModel == GameDefinitions.DebugWindowModel.FullWindow)
                        //{
                        //    m_SelectedNode = null;
                        //    if (debugComponent.FullWindowDownPos.y < debugComponent.LogScrollRect.y)
                        //    {
                        //        debugComponent.LastScrollOffsetY = 0;
                        //        isDragMofidy = true;
                        //    }
                        //}
                        if (m_LockScroll || m_SelectedNode != null)
                        {
                            debugComponent.LastScrollOffsetY = 0;
                            isDragMofidy = true;
                            if (m_LockScroll)
                            {
                                m_ScrollValue = debugComponent.LogScrollBarTop;
                                m_ScrollBarSize = debugComponent.LogScrollBarHeight;
                            }
                        }
                        else
                        {
                            m_DragOffsetY -= ScrollOffsetY;
                            ResetOffsetHeight();
                            if (m_DebugComponent.Portrail)
                            {
                                m_SpeedBase = 50;
                            }
                            if (Mathf.Abs(m_LastDragOffsetY - m_DragOffsetY) > m_SpeedBase)
                            {
                                debugComponent.LastScrollOffsetY = m_LastDragOffsetY - m_DragOffsetY;
                            }
                            m_LastDragOffsetY = m_DragOffsetY;
                            if (!m_InBarModify && Mathf.Abs(ScrollOffsetY) > 0.001f && m_DragOffsetY < 0 && m_AllLogHeight > GameMainRoot.Debugger.LogScrollRect.height + 1)
                            {
                                CalcScrollValue();
                                isDragMofidy = true;
                            }
                        }
                        m_InBarModify = false;

                        //自动填充Log区域
                        GUILayoutUtility.GetRect(new GUIContent(""), new GUIStyle() {fixedHeight = debugComponent.LogScrollRect.height + 10});

                        GUILayout.BeginArea(debugComponent.LogScrollRect);
                        {
                            bool selected = false;
                            m_FliterLogCount = m_InFilterLogs.Count;
                            m_InDrawBorder = debugComponent.LogScrollRect.height + 10;
                            m_InShowLogs.Clear();
                            for (int i = 0; i < m_FliterLogCount; i++)
                            {
                                m_LogNodeJson.AllLogNodes[m_InFilterLogs[i]].LogRect.y = m_LogNodeJson.AllLogNodes[m_InFilterLogs[i]].LogOrderY + m_DragOffsetY;
                                if (m_LogNodeJson.AllLogNodes[m_InFilterLogs[i]].LogRect.y + m_LogNodeJson.AllLogNodes[m_InFilterLogs[i]].LogRect.height < -10)
                                {
                                    // 不属于渲染范围
                                }
                                else if (m_LogNodeJson.AllLogNodes[m_InFilterLogs[i]].LogRect.y > m_InDrawBorder)
                                {
                                    break;
                                }
                                else
                                {
                                    m_InShowLogs.Add(m_InFilterLogs[i]);
                                    GUI.Label(m_LogNodeJson.AllLogNodes[m_InFilterLogs[i]].LogRect, m_LogNodeJson.AllLogNodes[m_InFilterLogs[i]].LogMessage);
                                }
                                //showedHeight += logNode.LogHeigth + m_LogInv;
                            }
                            if (!selected)
                            {
                                m_SelectedNode = null;
                            }
                        }
                    }
                    GUILayout.EndArea();

                    if (m_LockScroll || m_SelectedNode != null)
                    {
                        GUI.VerticalScrollbar(debugComponent.LogScrollBarRect, m_ScrollValue, m_ScrollBarSize, debugComponent.LogScrollBarTop, debugComponent.LogScrollBarBottom);
                    }
                    else
                    {
                        m_CurScrollValue = GUI.VerticalScrollbar(debugComponent.LogScrollBarRect, m_ScrollValue, m_ScrollBarSize, debugComponent.LogScrollBarTop, debugComponent.LogScrollBarBottom);
                        if (!isDragMofidy && Mathf.Abs(m_CurScrollValue - m_ScrollValue) > 0.001f && m_AllLogHeight > debugComponent.LogScrollRect.height)
                        {
                            m_DragOffsetY = -(m_AllLogHeight - debugComponent.LogScrollRect.height) * ((m_CurScrollValue - debugComponent.LogScrollBarTop) / (debugComponent.LogScrollBarHeight - m_ScrollBarSize));
                            ResetOffsetHeight();
                            m_InBarModify = true;
                            debugComponent.LastScrollOffsetY = 0;
                            //Log.Warning($"m_DragOffsetY = {m_DragOffsetY}, total = {m_AllLogHeight}, m_CurScrollValue = {m_CurScrollValue}, top = {debugComponent.LogScrollBarTop}, down = {debugComponent.LogScrollBarBottom}");
                        }
                        m_ScrollValue = m_CurScrollValue;
                    }
                }
                // ConsoleDebuggerGUI.DrawGUI(); // Game 层命令系统(UnityConsole)，未随 Debugger 框架迁移，故注释

            }


            public string GetSelectLogString()
            {
                if (m_SelectedNode != null)
                {
                    return m_SelectedNode.LogMessage;
                }
                return null;
            }

            private void Clear()
            {
                lock (m_LogqueueLocker)
                {
                    m_LogNodeJson.AllLogNodes.Clear();
                    m_InFilterLogs.Clear();
                    m_InShowLogs.Clear();
                    ResetSearchLogCount();
                    m_AllLogHeight = 0;
                    m_ScrollValue = m_DebugComponent.LogScrollBarTop;
                    m_ScrollBarSize = m_DebugComponent.LogScrollBarHeight;
                    LogNode.Reset();
                }
            }

            private void ResetSearchLogCount()
            {
                m_SearchLogCount.Clear();
                m_SearchLogCount.Add(LogType.Error, 0);
                m_SearchLogCount.Add(LogType.Assert, 0);
                m_SearchLogCount.Add(LogType.Warning, 0);
                m_SearchLogCount.Add(LogType.Log, 0);
                m_SearchLogCount.Add(LogType.Exception, 0);
            }
            /// <summary>
            /// 上传日志
            /// </summary>
            private void UploadLog(DebuggerComponent debugComponent)
            {
                string serverUrl = GameMainRoot.Config.GetString("UploadLog");

                JObject customJsonData = new JObject();
                // GM后台的appId
                customJsonData["app_id"] = GameMainRoot.Config.GetInt("MGAppID", true);
           /*     // 自有服务器用户ID
                customJsonData["user_id"] = GameMainRoot.SDK.TGAHelper.AccountID;*/
                // 用户设备ID
                customJsonData["package"] = Application.identifier;
                
                // 生成Log文本内容
                StringBuilder stringBuilder = new StringBuilder();
                for (int index = 0; index < m_InFilterLogs.Count; index++)
                {
                    LogNode logNode = m_LogNodeJson.AllLogNodes[m_InFilterLogs[index]];
                    stringBuilder.AppendLine(logNode.LogMessage);
                    stringBuilder.AppendLine(logNode.StackTrack);
                }
                string fileContents = stringBuilder.ToString();
                byte[] fileBytes = Converter.GetBytesByString(fileContents);

                /*   string fileName = $"Log_appid_{GameMainRoot.Config.GetInt("MGAppID", true)}_uid_{GameMainRoot.SDK.TGAHelper.AccountID}_device_{SystemInfo.deviceModel.Replace(" ", "")}_time_{DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss.fff")}.txt";
                   */

                string fileName = "";
                // 上传日志请求
                GameMainRoot.Network.HttpRequestOnPostWithFile(serverUrl, customJsonData.ToString(), fileBytes, fileName, (HTTPRequest req, HTTPResponse resp) =>
                {
                    switch (req.State)
                    {
                        case HTTPRequestStates.Finished:
                            {
                                string finalContent = Encoding.UTF8.GetString(resp.Data);
                                if (!string.IsNullOrEmpty(finalContent))
                                {
                                    var jsonData = JObject.Parse(finalContent);
                                    if (jsonData.ContainsKey("code") && (int)jsonData["code"] == 0)
                                    {
                                        m_UploadButtonWords = "<color=#00FF00FF>上传日志成功</color>";
                                        DOTween.Kill(GameDOTweenTypes.DebuggerLogTextTween);
                                        DOTween.Sequence().AppendInterval(3).OnComplete(() =>{m_UploadButtonWords = "上传日志";}).SetUpdate(true).SetId(GameDOTweenTypes.DebuggerLogTextTween);
                                        Log.Debug($"日志 {fileName} 上传成功。");       
                                    }
                                }
                            } 
                            break;
                        case HTTPRequestStates.Error:
                        case HTTPRequestStates.Aborted:
                        case HTTPRequestStates.ConnectionTimedOut:
                        case HTTPRequestStates.TimedOut:
                            {
                                m_UploadButtonWords = "<color=#FF0000FF>上传日志失败</color>";
                                DOTween.Kill(GameDOTweenTypes.DebuggerLogTextTween);
                                DOTween.Sequence().AppendInterval(3).OnComplete(() =>{m_UploadButtonWords = "上传日志";}).SetUpdate(true).SetId(GameDOTweenTypes.DebuggerLogTextTween);
                                Log.Debug($"日志 {fileName} 上传失败。");
                            } 
                            break;
                    }
                }, true, -1, -1, GetUrlHeader());
                m_UploadButtonWords = "<color=#FFFF00FF>上传日志中...</color>";
                DOTween.Kill(GameDOTweenTypes.DebuggerLogTextTween);
                DOTween.Sequence().AppendInterval(30).OnComplete(() =>{m_UploadButtonWords = "上传日志";}).SetUpdate(true).SetId(GameDOTweenTypes.DebuggerLogTextTween);
                Log.Debug($"日志 {fileName} 正在上传中......");
            }

            /// <summary>
            /// 获取通用协议头
            /// </summary>
            /// <returns></returns>
            private string GetUrlHeader()
            {
                JObject headerJson = new JObject();
                headerJson.Add("Content-Type", "multipart/form-data");
                return JsonConvert.SerializeObject(headerJson);
            }
            public void OnStartDown(Vector2 downPoint)
            {
            }

            public void OnEndDown(float downPointY)
            {
                //Log.Warning($"OnEndDown = {downPointY}");
                for (int i = 0; i < m_InShowLogs.Count; i++)
                {
                    //Log.Warning($"i = {i},y = {m_LogNodeJson.AllLogNodes[m_InDrawIndexs[i]].LogRect.y}, endy = {m_LogNodeJson.AllLogNodes[m_InDrawIndexs[i]].LogRect.y + m_LogNodeJson.AllLogNodes[m_InDrawIndexs[i]].LogRect.height}");
                    if (m_LogNodeJson.AllLogNodes[m_InShowLogs[i]].LogRect.y <= downPointY
                        && downPointY <= m_LogNodeJson.AllLogNodes[m_InShowLogs[i]].LogRect.y + m_LogNodeJson.AllLogNodes[m_InShowLogs[i]].LogRect.height)
                    {
                        m_DebugComponent.ControlInput();
                        if (m_DebugComponent.WinModel == GameDefinitions.DebugWindowModel.FullWindow)
                        {
                            if (m_SelectedNode != m_LogNodeJson.AllLogNodes[m_InShowLogs[i]] && m_DebugComponent.FullWindowInDrag == false)
                            {
                                m_SelectedNode = m_LogNodeJson.AllLogNodes[m_InShowLogs[i]];
                                m_DebugComponent.WinModel = GameDefinitions.DebugWindowModel.PopWindow;
                                m_DebugComponent.SelLogStackTrack = m_LogNodeJson.AllLogNodes[m_InShowLogs[i]].StackTrack;
                                m_DebugComponent.SelLogString = GetSelectLogString();
                            }
                        }
                    }
                }
            }

            public void GetRecentLogs(List<LogNode> results)
            {
                lock (m_LogqueueLocker)
                {
                    if (results == null)
                    {
                        Log.Error("Results 无效。");
                        return;
                    }

                    results.Clear();
                    foreach (LogNode logNode in m_LogNodeJson.AllLogNodes)
                    {
                        results.Add(logNode);
                    }
                }
            }

            public void GetRecentLogs(List<LogNode> results, int count)
            {
                lock (m_LogqueueLocker)
                {
                    if (results == null)
                    {
                        Log.Error("Results 无效。");
                        return;
                    }

                    if (count <= 0)
                    {
                        Log.Error("Count 无效。");
                        return;
                    }

                    int position = m_LogNodeJson.AllLogNodes.Count - count;
                    if (position < 0)
                    {
                        position = 0;
                    }

                    int index = 0;
                    results.Clear();
                    foreach (LogNode logNode in m_LogNodeJson.AllLogNodes)
                    {
                        if (index++ < position)
                        {
                            continue;
                        }

                        results.Add(logNode);
                    }
                }
            }

            // 不能接受从子线程打印的log，暂时屏蔽
            //private void OnLogMessageReceived(string logMessage, string stackTrace, LogType logType)
            //{
            //    if (logType == LogType.Assert)
            //    {
            //        logType = LogType.Error;
            //    }

            //    m_LogNodes.Enqueue(LogNode.Create(logType, logMessage, stackTrace));
            //    while (m_LogNodes.Count > m_MaxLine)
            //    {
            //        m_LogNodes.Dequeue();
            //    }
            //}

            /// <summary>
            /// 因为第三方sdk可能在子线程运行，将log的接受函数改为主线程和子线程都可以调用的
            /// </summary>
            /// <param name="logMessage"></param>
            /// <param name="stackTrace"></param>
            /// <param name="logType"></param>
            // by liuyu
            private void OnLogMessageThreadReceived(string logMessage, string stackTrace, LogType logType)
            {
#if !UNITY_EDITOR
                if (m_LogNodeJson.AllLogNodes.Count > 100000)
                {
                    Clear();
                }
#endif

                lock (m_LogqueueLocker)
                {
                    if (logType == LogType.Assert)
                    {
                        logType = LogType.Error;
                    }

                    LogNode logNode = LogNode.CreateThreadedNode(m_DebugComponent, logType, logMessage, stackTrace, m_FrameCount, m_LogTotalIdx++);
                    m_LogNodeJson.AllLogNodes.Add(logNode);
                    m_WaitingCalcHeightIdxs.Add(m_LogNodeJson.AllLogNodes.Count - 1);
                    //if (m_LogNodeJson.AllLogNodes.Count - m_LogStartIndex <= m_MaxLine)
                    //{
                    //    m_LogNodes.Enqueue(logNode);
                    //    if (m_LogNodes.Count >= m_MaxLine)
                    //    {
                    //        m_LogStartIndex += m_MaxLine;
                    //        m_LogNodes.Clear();
                    //    }
                    //}
                }
            }
        }
    }
}


