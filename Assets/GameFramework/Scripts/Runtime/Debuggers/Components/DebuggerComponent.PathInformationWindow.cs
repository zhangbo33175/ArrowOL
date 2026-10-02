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
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Honor.Runtime
{
    public sealed partial class DebuggerComponent : GameComponent
    {
        private sealed class PathInformationWindow : ScrollableDebuggerWindowBase
        {
            protected override void OnDrawScrollableWindow()
            {
                GUILayout.Label("<b>Path Information</b>");
                GUILayout.BeginVertical("box");
                {
                    DrawItem("Data Path", Application.dataPath);
                    DrawItem("Persistent Data Path", Application.persistentDataPath);
                    DrawItem("Streaming Assets Path", Application.streamingAssetsPath);
                    DrawItem("Temporary Cache Path", Application.temporaryCachePath);
#if UNITY_2018_3_OR_NEWER
                    DrawItem("Console Log Path", Application.consoleLogPath);
#endif
                }
                GUILayout.EndVertical();
            }
        }
    }
}


