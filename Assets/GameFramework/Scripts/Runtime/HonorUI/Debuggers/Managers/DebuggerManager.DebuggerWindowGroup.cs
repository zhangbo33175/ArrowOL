/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  DebuggerManager.DebuggerWindowGroup.cs
 * author:    云毅
 * created:   2026
 * descrip:   调试器窗口组
 ***************************************************************/
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Honor.Runtime
{
    public sealed partial class DebuggerManager
    {
        //=========================================================================
        // 调试器窗口组（内部私有类）
        //=========================================================================
        #region DebuggerWindowGroup
        /// <summary>
        /// 调试器窗口组
        /// 支持层级路径注册、选中切换与递归查找
        /// </summary>
        private sealed class DebuggerWindowGroup : IDebuggerWindowGroup
        {
            #region 私有字段
            private readonly List<KeyValuePair<string, IDebuggerWindow>> m_DebuggerWindows;
            private int m_SelectedIndex;
            private string[] m_DebuggerWindowNames;
            #endregion

            /// <summary>
            /// 初始化调试器窗口组
            /// </summary>
            public DebuggerWindowGroup()
            {
                m_DebuggerWindows = new List<KeyValuePair<string, IDebuggerWindow>>();
                m_SelectedIndex = 0;
                m_DebuggerWindowNames = null;
            }

            #region IDebuggerWindowGroup 属性
            /// <summary>
            /// 获取调试器窗口数量。
            /// </summary>
            public int DebuggerWindowCount
            {
                get
                {
                    return m_DebuggerWindows.Count;
                }
            }

            /// <summary>
            /// 获取或设置当前选中的调试器窗口索引。
            /// </summary>
            public int SelectedIndex
            {
                get
                {
                    return m_SelectedIndex;
                }
                set
                {
                    m_SelectedIndex = value;
                }
            }

            /// <summary>
            /// 获取当前选中的调试器窗口。
            /// </summary>
            public IDebuggerWindow SelectedWindow
            {
                get
                {
                    if (m_SelectedIndex >= m_DebuggerWindows.Count)
                    {
                        return null;
                    }

                    return m_DebuggerWindows[m_SelectedIndex].Value;
                }
            }
            #endregion

            #region IDebuggerWindow 生命周期
            /// <inheritdoc/>
            public void Initialize(DebuggerComponent drawComponent, params object[] args)
            {
            }

            /// <inheritdoc/>
            public void Shutdown()
            {
                foreach (KeyValuePair<string, IDebuggerWindow> debuggerWindow in m_DebuggerWindows)
                {
                    debuggerWindow.Value.Shutdown();
                }

                m_DebuggerWindows.Clear();
            }

            /// <inheritdoc/>
            public void OnEnter()
            {
                SelectedWindow?.OnEnter();
            }

            /// <inheritdoc/>
            public void OnLeave()
            {
                SelectedWindow?.OnLeave();
            }

            /// <inheritdoc/>
            public void OnUpdate(float elapseSeconds, float realElapseSeconds)
            {
                SelectedWindow?.OnUpdate(elapseSeconds, realElapseSeconds);
            }

            /// <inheritdoc/>
            public void OnDraw(DebuggerComponent drawComponent, float offsetY)
            {
            }
            #endregion

            #region 窗口查找与注册
            /// <summary>
            /// 刷新窗口名称缓存数组
            /// </summary>
            private void RefreshDebuggerWindowNames()
            {
                m_DebuggerWindowNames = new string[m_DebuggerWindows.Count];
                int index = 0;
                foreach (KeyValuePair<string, IDebuggerWindow> debuggerWindow in m_DebuggerWindows)
                {
                    m_DebuggerWindowNames[index++] = debuggerWindow.Key;
                }
            }

            /// <summary>
            /// 获取调试组的调试器窗口名称集合。
            /// </summary>
            public string[] GetDebuggerWindowNames()
            {
                return m_DebuggerWindowNames;
            }

            /// <summary>
            /// 获取调试器窗口（支持递归路径）。
            /// </summary>
            /// <param name="path">调试器窗口路径。</param>
            /// <returns>要获取的调试器窗口。</returns>
            public IDebuggerWindow GetDebuggerWindow(string path)
            {
                if (string.IsNullOrEmpty(path))
                {
                    return null;
                }

                int pos = path.IndexOf('/');
                if (pos < 0 || pos >= path.Length - 1)
                {
                    return FindWindowByName(path);
                }

                string groupName = path.Substring(0, pos);
                string leftPath = path.Substring(pos + 1);
                DebuggerWindowGroup debuggerWindowGroup = FindWindowByName(groupName) as DebuggerWindowGroup;
                if (debuggerWindowGroup == null)
                {
                    return null;
                }

                return debuggerWindowGroup.GetDebuggerWindow(leftPath);
            }

            /// <summary>
            /// 选中调试器窗口（支持递归路径）。
            /// </summary>
            /// <param name="path">调试器窗口路径。</param>
            /// <returns>是否成功选中调试器窗口。</returns>
            public bool SelectDebuggerWindow(string path)
            {
                if (string.IsNullOrEmpty(path))
                {
                    return false;
                }

                int pos = path.IndexOf('/');
                if (pos < 0 || pos >= path.Length - 1)
                {
                    return SelectWindowByName(path);
                }

                string groupName = path.Substring(0, pos);
                string leftPath = path.Substring(pos + 1);
                DebuggerWindowGroup debuggerWindowGroup = FindWindowByName(groupName) as DebuggerWindowGroup;
                if (debuggerWindowGroup == null || !SelectWindowByName(groupName))
                {
                    return false;
                }

                return debuggerWindowGroup.SelectDebuggerWindow(leftPath);
            }

            /// <summary>
            /// 注册调试器窗口（支持层级路径）。
            /// </summary>
            /// <param name="path">调试器窗口路径。</param>
            /// <param name="debuggerWindow">要注册的调试器窗口。</param>
            public void RegisterDebuggerWindow(string path, IDebuggerWindow debuggerWindow)
            {
                if (string.IsNullOrEmpty(path))
                {
                    throw new Exception("Path 无效。");
                }

                int pos = path.IndexOf('/');
                if (pos < 0 || pos >= path.Length - 1)
                {
                    if (FindWindowByName(path) != null)
                    {
                        throw new Exception("Debugger window has been registered.");
                    }

                    m_DebuggerWindows.Add(new KeyValuePair<string, IDebuggerWindow>(path, debuggerWindow));
                    RefreshDebuggerWindowNames();
                }
                else
                {
                    string groupName = path.Substring(0, pos);
                    string leftPath = path.Substring(pos + 1);
                    DebuggerWindowGroup debuggerWindowGroup = FindWindowByName(groupName) as DebuggerWindowGroup;
                    if (debuggerWindowGroup == null)
                    {
                        if (FindWindowByName(groupName) != null)
                        {
                            throw new Exception("Debugger window has been registered, can not create debugger window group.");
                        }

                        debuggerWindowGroup = new DebuggerWindowGroup();
                        m_DebuggerWindows.Add(new KeyValuePair<string, IDebuggerWindow>(groupName, debuggerWindowGroup));
                        RefreshDebuggerWindowNames();
                    }

                    debuggerWindowGroup.RegisterDebuggerWindow(leftPath, debuggerWindow);
                }
            }

            /// <summary>
            /// 按名称在当前层查找窗口
            /// </summary>
            /// <param name="name">窗口名称</param>
            /// <returns>找到的窗口，未找到返回 null</returns>
            private IDebuggerWindow FindWindowByName(string name)
            {
                foreach (KeyValuePair<string, IDebuggerWindow> debuggerWindow in m_DebuggerWindows)
                {
                    if (debuggerWindow.Key == name)
                    {
                        return debuggerWindow.Value;
                    }
                }

                return null;
            }

            /// <summary>
            /// 按名称在当前层选中窗口
            /// </summary>
            /// <param name="name">窗口名称</param>
            /// <returns>是否选中成功</returns>
            private bool SelectWindowByName(string name)
            {
                for (int i = 0; i < m_DebuggerWindows.Count; i++)
                {
                    if (m_DebuggerWindows[i].Key == name)
                    {
                        m_SelectedIndex = i;
                        return true;
                    }
                }

                return false;
            }
            #endregion
        }
        #endregion
    }
}

