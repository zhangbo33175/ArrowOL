/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  DebuggerComponent.cs
 * author:    taoye
 * created:   2020/8/26
 * descrip:   调试组件
 ***************************************************************/
using UnityEngine;

namespace Honor.Runtime
{
    public sealed partial class DebuggerComponent : GameComponent
    {
        private abstract class ScrollableDebuggerWindowBase : IDebuggerWindow
        {
            private const float TitleWidth = 240f;
            private Vector2 m_ScrollPosition = Vector2.zero;

            public virtual void Initialize(DebuggerComponent drawComponent, params object[] args)
            {
            }

            public virtual void Shutdown()
            {
            }

            public virtual void OnEnter()
            {
            }

            public virtual void OnLeave()
            {
            }

            public virtual void OnUpdate(float elapseSeconds, float realElapseSeconds)
            {
            }

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

            protected abstract void OnDrawScrollableWindow();

            protected static void DrawItem(string title, string content)
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label(title, GUILayout.Width(TitleWidth));
                    GUILayout.Label(content);
                }
                GUILayout.EndHorizontal();
            }

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
    }
}


