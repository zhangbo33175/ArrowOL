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
        private sealed class InputTouchInformationWindow : ScrollableDebuggerWindowBase
        {
            protected override void OnDrawScrollableWindow()
            {
                GUILayout.Label("<b>Input Touch Information</b>");
                GUILayout.BeginVertical("box");
                {
                    DrawItem("Touch Supported", Input.touchSupported.ToString());
                    DrawItem("Touch Pressure Supported", Input.touchPressureSupported.ToString());
                    DrawItem("Stylus Touch Supported", Input.stylusTouchSupported.ToString());
                    DrawItem("Simulate Mouse With Touches", Input.simulateMouseWithTouches.ToString());
                    DrawItem("Multi Touch Enabled", Input.multiTouchEnabled.ToString());
                    DrawItem("Touch Count", Input.touchCount.ToString());
                    DrawItem("Touches", GetTouchesString(Input.touches));
                }
                GUILayout.EndVertical();
            }

            private string GetTouchString(Touch touch)
            {
                return $"{touch.position}, {touch.deltaPosition}, {touch.rawPosition}, {touch.pressure}, {touch.phase}";
            }

            private string GetTouchesString(Touch[] touches)
            {
                string[] touchStrings = new string[touches.Length];
                for (int i = 0; i < touches.Length; i++)
                {
                    touchStrings[i] = GetTouchString(touches[i]);
                }

                return string.Join("; ", touchStrings);
            }
        }
    }
}


