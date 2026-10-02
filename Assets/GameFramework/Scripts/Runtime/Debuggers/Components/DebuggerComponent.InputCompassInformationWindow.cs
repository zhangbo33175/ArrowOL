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
        private sealed class InputCompassInformationWindow : ScrollableDebuggerWindowBase
        {
            protected override void OnDrawScrollableWindow()
            {
                GUILayout.Label("<b>Input Compass Information</b>");
                GUILayout.BeginVertical("box");
                {
                    GUILayout.BeginHorizontal();
                    {
                        if (GUILayout.Button("Enable"))
                        {
                            Input.compass.enabled = true;
                        }
                        if (GUILayout.Button("Disable"))
                        {
                            Input.compass.enabled = false;
                        }
                    }
                    GUILayout.EndHorizontal();

                    DrawItem("Enabled", Input.compass.enabled.ToString());
                    if (Input.compass.enabled)
                    {
                        DrawItem("Heading Accuracy", Input.compass.headingAccuracy.ToString());
                        DrawItem("Magnetic Heading", Input.compass.magneticHeading.ToString());
                        DrawItem("Raw Vector", Input.compass.rawVector.ToString());
                        DrawItem("Timestamp", Input.compass.timestamp.ToString());
                        DrawItem("True Heading", Input.compass.trueHeading.ToString());
                    }
                }
                GUILayout.EndVertical();
            }
        }
    }
}


