/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  DebuggerComponent.cs
 * author:    云毅
 * created:   2026
 * descrip:   调试组件
 ***************************************************************/
using UnityEngine;

namespace Honor.Runtime
{
    public sealed partial class DebuggerComponent : GameComponent
    {
        //=========================================================================
        // 可滚动调试窗口基类
        //=========================================================================
        #region ScrollableDebuggerWindowBase
        /// <summary>
        /// 可滚动调试窗口基类
        /// 封装滚动区域、标题宽度常量及字节长度格式化工具，供各信息窗口复用
        /// </summary>
        private abstract class ScrollableDebuggerWindowBase : IDebuggerWindow
        {
            /// <summary>标题列固定宽度</summary>
            private const float TitleWidth = 240f;
            private Vector2 m_ScrollPosition = Vector2.zero;

            /// <inheritdoc/>
            public virtual void Initialize(DebuggerComponent drawComponent, params object[] args)
            {
            }

            /// <inheritdoc/>
            public virtual void Shutdown()
            {
            }

            /// <inheritdoc/>
            public virtual void OnEnter()
            {
            }

            /// <inheritdoc/>
            public virtual void OnLeave()
            {
            }

            /// <inheritdoc/>
            public virtual void OnUpdate(float elapseSeconds, float realElapseSeconds)
            {
            }

            /// <inheritdoc/>
            public void OnDraw(DebuggerComponent drawComponent, float offsetY)
            {
                if (drawComponent.FullWindowDownPos.y < 330)
                {
                    offsetY = 0;
                }
                m_ScrollPosition.y += offsetY;
                m_ScrollPosition = GUILayout.BeginScrollView(m_ScrollPosition);
                {
                    OnDrawScrollableWindow();
                }
                GUILayout.EndScrollView();
            }

            /// <summary>
            /// 子类实现：绘制可滚动窗口内容
            /// </summary>
            protected abstract void OnDrawScrollableWindow();

            /// <summary>
            /// 绘制一行「标题 - 内容」信息
            /// </summary>
            /// <param name="title">标题文本</param>
            /// <param name="content">内容文本</param>
            protected static void DrawItem(string title, string content)
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label(title, GUILayout.Width(TitleWidth));
                    GUILayout.Label(content);
                }
                GUILayout.EndHorizontal();
            }

            /// <summary>
            /// 将字节数格式化为人类可读字符串（B/KB/MB/GB...）
            /// </summary>
            /// <param name="byteLength">字节数</param>
            /// <returns>格式化后的字符串</returns>
            protected static string GetByteLengthString(long byteLength)
            {
                if (byteLength < 1024L) // 2 ^ 10
                {
                    return $"{byteLength} B";
                }

                if (byteLength < 1048576L) // 2 ^ 20
                {
                    return $"{(byteLength / 1024f).ToString("F2")} KB";
                }

                if (byteLength < 1073741824L) // 2 ^ 30
                {
                    return $"{(byteLength / 1048576f).ToString("F2")} MB";
                }

                if (byteLength < 1099511627776L) // 2 ^ 40
                {
                    return $"{(byteLength / 1073741824f).ToString("F2")} GB";
                }

                if (byteLength < 1125899906842624L) // 2 ^ 50
                {
                    return $"{(byteLength / 1099511627776f).ToString("F2")} TB";
                }

                if (byteLength < 1152921504606846976L) // 2 ^ 60
                {
                    return $"{(byteLength / 1125899906842624f).ToString("F2")} PB";
                }

                return $"{(byteLength / 1152921504606846976f).ToString("F2")} EB";
            }
        }
        #endregion
    }
}

