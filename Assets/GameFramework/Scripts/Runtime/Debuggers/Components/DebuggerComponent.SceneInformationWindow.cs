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
using UnityEngine.SceneManagement;

namespace Honor.Runtime
{
    public sealed partial class DebuggerComponent : GameComponent
    {
        private sealed class SceneInformationWindow : ScrollableDebuggerWindowBase
        {
            protected override void OnDrawScrollableWindow()
            {
                GUILayout.Label("<b>Scene Information</b>");
                GUILayout.BeginVertical("box");
                {
                    DrawItem("Scene Count", UnityEngine.SceneManagement.SceneManager.sceneCount.ToString());
                    DrawItem("Scene Count In Build Settings", UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings.ToString());

                    Scene activeScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
                    DrawItem("Active Scene Name", activeScene.name);
                    DrawItem("Active Scene Path", activeScene.path);
                    DrawItem("Active Scene Build Index", activeScene.buildIndex.ToString());
                    DrawItem("Active Scene Is Dirty", activeScene.isDirty.ToString());
                    DrawItem("Active Scene Is Loaded", activeScene.isLoaded.ToString());
                    DrawItem("Active Scene Is Valid", activeScene.IsValid().ToString());
                    DrawItem("Active Scene Root Count", activeScene.rootCount.ToString());
                }
                GUILayout.EndVertical();
            }
        }
    }
}


