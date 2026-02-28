using System;
using GameLib;
using Honor.Runtime;
using UnityEditor;
using UnityEngine;

namespace Honor.Editor
{
    internal sealed partial class GameConfigComponentInspector
    {
        /// <summary>
        /// 编辑器资源模式
        /// </summary>
        public void OnEditorResourceMode()
        {
            EditorGUILayout.Space(3);
            m_DevelopMode.boolValue = EditorGUILayout.Toggle("开发模式", m_DevelopMode.boolValue);
            EditorGUILayout.HelpBox("开发模式：用于切换开发环境与生产环境的系统配置，模式切换等操作。", MessageType.Info);
            EditorGUILayout.Space(3);
            EditorGUILayout.Separator();
            m_IsLocalServer.boolValue = EditorGUILayout.Toggle("是否为本地服务器", m_IsLocalServer.boolValue);
            EditorGUILayout.Space(3);
            m_EditorResourceMode.boolValue = EditorGUILayout.Toggle("编辑器资源模式", m_EditorResourceMode.boolValue);
            EditorGUILayout.Space(3);
            m_CheckOrientationState.boolValue = EditorGUILayout.Toggle("监测屏幕方向变动", m_CheckOrientationState.boolValue);
            EditorGUILayout.Space(3);
            openDebug.boolValue = EditorGUILayout.Toggle("打开LOG", openDebug.boolValue);
            EditorGUILayout.Space(3);
            m_AutoFontAdapt.boolValue = EditorGUILayout.Toggle("开启字体自动适配", m_AutoFontAdapt.boolValue);
            EditorGUILayout.Space(3);
            m_RunInBackground.boolValue = EditorGUILayout.Toggle("启用后台运行", m_RunInBackground.boolValue);
            EditorGUILayout.Space(3);
            OnNeverSleep();
            EditorGUILayout.Space(3);
            m_LuaHotReloadMode.boolValue = EditorGUILayout.Toggle("Lua热重载模式", m_LuaHotReloadMode.boolValue);
            EditorGUILayout.Space(3);
            OnLuaDebugMode();
            EditorGUILayout.Space(3);
            OnFrameRate();
            EditorGUILayout.Space(3);
            OnGameSpeed();
            EditorGUILayout.Space(3);
        }

        private void OnNeverSleep()
        {
            GameConfig t = (GameConfig)target;
            bool neverSleep = EditorGUILayout.Toggle("启用屏幕常亮", m_NeverSleep.boolValue);
            if (neverSleep != m_NeverSleep.boolValue)
            {
                if (EditorApplication.isPlaying)
                {
                    t.NeverSleep = neverSleep;
                }
                else
                {
                    m_NeverSleep.boolValue = neverSleep;
                }
            }
        }

        private void OnLuaDebugMode()
        {
            if (m_LuacMode.boolValue)
            {
                GameDefinitions.DebugMode debugMode = m_LuaDebugMode;
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.EnumPopup("Lua调试", GameDefinitions.DebugMode.None);
                EditorGUILayout.HelpBox("注：调试模式在Luac模式下是不可使用的！", MessageType.Info);
                EditorGUI.EndDisabledGroup();
                m_LuaDebugMode = GameDefinitions.DebugMode.None;
                if (debugMode != m_LuaDebugMode)
                {
                    WriteDebugModeToLibraryConfigFile();
                }
            }
            else
            {
                int debugModeIndex = (int)(GameDefinitions.DebugMode)EditorGUILayout.EnumPopup("Lua调试", m_LuaDebugMode);
                EditorGUILayout.HelpBox(
                    "调试模式：用于切换日常Lua断点调试联调的开关。\n[None]：非调试模式\n[IDE First]：调试模式-IDE优先启动模式\n[Unity First]：调试模式-Unity优先启动模式",
                    MessageType.Info);
                if (debugModeIndex != (int)m_LuaDebugMode)
                {
                    m_LuaDebugMode = (GameDefinitions.DebugMode)debugModeIndex;
                    WriteDebugModeToLibraryConfigFile();
                }
            }

            m_LuacMode.boolValue = EditorGUILayout.Toggle("Luac模式", m_LuacMode.boolValue);
            EditorGUILayout.HelpBox("Luac模式：是否使用Luac脚本。\n设备中将强制切换为Luac模式。", MessageType.Info);
        }

        private void OnGameSpeed()
        {
            GameConfig t = (GameConfig)target;
            EditorGUILayout.BeginVertical("box");
            {
                float gameSpeed = EditorGUILayout.Slider("运行速率", m_GameSpeed.floatValue, 0f, 8f);
                int selectedGameSpeed = GUILayout.SelectionGrid(GetSelectedGameSpeed(gameSpeed), GameSpeedTexts, 5);
                if (selectedGameSpeed >= 0)
                {
                    gameSpeed = GetGameSpeed(selectedGameSpeed);
                }

                if (gameSpeed != m_GameSpeed.floatValue)
                {
                    if (EditorApplication.isPlaying)
                    {
                        t.GameSpeed = gameSpeed;
                    }
                    else
                    {
                        m_GameSpeed.floatValue = gameSpeed;
                    }
                }
            }
            EditorGUILayout.EndVertical();
        }

        private void OnSetUI()
        {
            EditorGUILayout.BeginVertical("box");

            m_UITransitionABPath.stringValue =
                EditorGUILayout.TextField("切换过渡界面AB路径", m_UITransitionABPath.stringValue);
            m_UITransitionAssetName.stringValue =
                EditorGUILayout.TextField("切换过渡界面Asset资源名称", m_UITransitionAssetName.stringValue);

            if (GUILayout.Button("使用框架默认切换过渡界面"))
            {
                m_UITransitionABPath.stringValue = m_UIABPathDefault;
                m_UITransitionAssetName.stringValue = m_UITransitionAssetNameDefault;
                serializedObject.ApplyModifiedProperties();
                GUIUtility.ExitGUI();
            }

            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical("box");

            m_UISplashABPath.stringValue = EditorGUILayout.TextField("闪屏界面AB路径", m_UISplashABPath.stringValue);
            m_UISplashAssetName.stringValue =
                EditorGUILayout.TextField("闪屏界面Asset资源名称", m_UISplashAssetName.stringValue);
            m_SplashProcedureDuration.intValue =
                EditorGUILayout.IntField("闪屏流程持续时间", m_SplashProcedureDuration.intValue);

            if (GUILayout.Button("使用框架默认闪屏界面"))
            {
                m_UISplashABPath.stringValue = m_UIABPathDefault;
                m_UISplashAssetName.stringValue = m_UISplashAssetNameDefault;
                serializedObject.ApplyModifiedProperties();
                GUIUtility.ExitGUI();
            }

            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical("box");

            m_UseUIPreload.boolValue = EditorGUILayout.ToggleLeft("使用预加载界面", m_UseUIPreload.boolValue);
            EditorGUI.BeginDisabledGroup(!m_UseUIPreload.boolValue);
            {
                m_UIPreloadABPath.stringValue = EditorGUILayout.TextField("预加载界面AB路径", m_UIPreloadABPath.stringValue);
                m_UIPreloadAssetName.stringValue =
                    EditorGUILayout.TextField("预加载界面Asset资源名称", m_UIPreloadAssetName.stringValue);
                if (GUILayout.Button("使用框架默认预加载界面"))
                {
                    m_UIPreloadABPath.stringValue = m_UIABPathDefault;
                    m_UIPreloadAssetName.stringValue = m_UIPreloadAssetNameDefault;
                    serializedObject.ApplyModifiedProperties();
                    GUIUtility.ExitGUI();
                }
            }
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.EndVertical();
        }

        /// <summary>
        /// 運行
        /// </summary>
        private void OnFrameRate()
        {
            GameConfig t = (GameConfig)target;
            int frameRate = EditorGUILayout.IntSlider("运行帧率", m_FrameRate.intValue, 1, 120);
            if (frameRate != m_FrameRate.intValue)
            {
                if (EditorApplication.isPlaying)
                {
                    t.FrameRate = frameRate;
                }
                else
                {
                    m_FrameRate.intValue = frameRate;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="selectedGameSpeed"></param>
        /// <returns></returns>
        private float GetGameSpeed(int selectedGameSpeed)
        {
            if (selectedGameSpeed < 0)
            {
                return GameSpeed[0];
            }

            if (selectedGameSpeed >= GameSpeed.Length)
            {
                return GameSpeed[GameSpeed.Length - 1];
            }

            return GameSpeed[selectedGameSpeed];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameSpeed"></param>
        /// <returns></returns>
        private int GetSelectedGameSpeed(float gameSpeed)
        {
            for (int i = 0; i < GameSpeed.Length; i++)
            {
                if (gameSpeed == GameSpeed[i])
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// 编辑器启动语言类型
        /// </summary>
        public void OnEditorLanguage()
        {
            EditorGUILayout.Space(3);
            EditorGUILayout.Separator();
            string[] list = Enum.GetNames(typeof(GameDefinitions.Language));
            string codeName = ((GameDefinitions.Language)m_EditorLanguage.enumValueIndex).ToString();
            if (Application.isPlaying)
            {
                EditorGUILayout.LabelField("编辑器启动语言类型",
                    AorTxt.Format("{0}({1})", codeName, GameDefinitions.LanguageDesc[m_EditorLanguage.enumValueIndex]));
            }
            else
            {
                m_EditorLanguage.enumValueIndex =
                    EditorGUILayout.Popup("编辑器启动语言类型", m_EditorLanguage.enumValueIndex, list);
                Log.Warning("编辑器启动语言类型" + list[m_EditorLanguage.enumValueIndex]);
            }

            EditorGUILayout.Space(3);
            EditorGUILayout.LabelField("当前语言类型",
                AorTxt.Format("{0}({1})", codeName, GameDefinitions.LanguageDesc[m_Language.enumValueIndex]));
            EditorGUI.EndDisabledGroup();


            EditorGUILayout.Space(3);
        }

        /// <summary>
        /// 编辑器启动入口流程
        /// </summary>
        public void OnEntryProcedureIndex()
        {
            EditorGUILayout.Space(3);
            EditorGUILayout.Separator();
            if (procedureTypeNames.Count > 0)
            {
                int selectedIndex = EditorGUILayout.Popup("入口流程", m_EntryProcedureIndex, procedureTypeNames.ToArray());
                if (selectedIndex != m_EntryProcedureIndex)
                {
                    m_EntryProcedureIndex = selectedIndex;
                    m_EntryProcedureTypeName.stringValue = procedureTypeNames[selectedIndex];
                }

                EditorGUILayout.HelpBox("请以ProcedureLaunch为入口流程，根据游戏需要调整启动！", MessageType.Info);
            }

            EditorGUI.EndDisabledGroup();
            EditorGUILayout.Space(3);
            OnSetUI();
            EditorGUILayout.Space(3);
        }
    }
}