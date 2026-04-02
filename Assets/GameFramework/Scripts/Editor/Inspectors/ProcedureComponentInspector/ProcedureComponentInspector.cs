using System;
using System.Collections.Generic;
using System.Text;
using Honor.Runtime;
using UnityEditor;
using UnityEngine;

namespace Honor.Editor
{
    [CustomEditor(typeof(ProcedureComponent))]
    internal sealed class ProcedureComponentInspector : HonorComponentInspector
    {
         private string m_UIABPathDefault = GamePathUtils.Prefab.GetFrameworkRootDirectoryRelativePath();
        private string m_UISplashAssetNameDefault = "UILauncherView";
        private string m_UITransitionAssetNameDefault = "UILauncherLogoView";
        private string m_UIPreloadAssetNameDefault = "UIPreloadDefault";

        private SerializedProperty m_ProcedureTypeNames = null;
        private SerializedProperty m_ProcedureTransitionEnterFlags = null;
        private SerializedProperty m_ProcedureTransitionEnterDurations = null;
        private SerializedProperty m_ProcedureTransitionEnterBlockRaycasts = null;
        private SerializedProperty m_ProcedureTransitionExitFlags = null;
        private SerializedProperty m_ProcedureTransitionExitDurations = null;
        private SerializedProperty m_ProcedureTransitionExitBlockRaycasts = null;
        private SerializedProperty m_ProcedureTransitionEnterFlagFromProcedureHotfix = null;
        private SerializedProperty m_ProcedureTransitionEnterDurationFromProcedureHotfix = null;
        private SerializedProperty m_ProcedureTransitionEnterBlockRaycastFromProcedureHotfix = null;
        private SerializedProperty m_ProcedureTransitionExitFlagFromProcedureHotfix = null;
        private SerializedProperty m_ProcedureTransitionExitDurationFromProcedureHotfix = null;
        private SerializedProperty m_ProcedureTransitionExitBlockRaycastFromProcedureHotfix = null;
        private SerializedProperty m_EntryProcedureTypeName = null;

        private SerializedProperty m_UITransitionABPath = null;
        private SerializedProperty m_UITransitionAssetName = null;

        private SerializedProperty m_UISplashABPath = null;
        private SerializedProperty m_UISplashAssetName = null;
        private SerializedProperty m_SplashProcedureDuration = null;

        private SerializedProperty m_UseUIPreload = null;
        private SerializedProperty m_UIPreloadABPath = null;
        private SerializedProperty m_UIPreloadAssetName = null;

        private int m_EntryProcedureIndex = -1;

        private void OnEnable()
        {
            m_ProcedureTypeNames = serializedObject.FindProperty("m_ProcedureTypeNames");
            m_ProcedureTransitionEnterFlags = serializedObject.FindProperty("m_ProcedureTransitionEnterFlags");
            m_ProcedureTransitionEnterDurations = serializedObject.FindProperty("m_ProcedureTransitionEnterDurations");
            m_ProcedureTransitionEnterBlockRaycasts = serializedObject.FindProperty("m_ProcedureTransitionEnterBlockRaycasts");
            m_ProcedureTransitionExitFlags = serializedObject.FindProperty("m_ProcedureTransitionExitFlags");
            m_ProcedureTransitionExitDurations = serializedObject.FindProperty("m_ProcedureTransitionExitDurations");
            m_ProcedureTransitionExitBlockRaycasts = serializedObject.FindProperty("m_ProcedureTransitionExitBlockRaycasts");
            m_ProcedureTransitionEnterFlagFromProcedureHotfix = serializedObject.FindProperty("m_ProcedureTransitionEnterFlagFromProcedureHotfix");
            m_ProcedureTransitionEnterDurationFromProcedureHotfix = serializedObject.FindProperty("m_ProcedureTransitionEnterDurationFromProcedureHotfix");
            m_ProcedureTransitionEnterBlockRaycastFromProcedureHotfix = serializedObject.FindProperty("m_ProcedureTransitionEnterBlockRaycastFromProcedureHotfix");
            m_ProcedureTransitionExitFlagFromProcedureHotfix = serializedObject.FindProperty("m_ProcedureTransitionExitFlagFromProcedureHotfix");
            m_ProcedureTransitionExitDurationFromProcedureHotfix = serializedObject.FindProperty("m_ProcedureTransitionExitDurationFromProcedureHotfix");
            m_ProcedureTransitionExitBlockRaycastFromProcedureHotfix = serializedObject.FindProperty("m_ProcedureTransitionExitBlockRaycastFromProcedureHotfix");
            m_EntryProcedureTypeName = serializedObject.FindProperty("m_EntryProcedureTypeName");
            m_UITransitionABPath = serializedObject.FindProperty("m_UITransitionABPath");
            m_UITransitionAssetName = serializedObject.FindProperty("m_UITransitionAssetName");
            m_UISplashABPath = serializedObject.FindProperty("m_UISplashABPath");
            m_UISplashAssetName = serializedObject.FindProperty("m_UISplashAssetName");
            m_SplashProcedureDuration = serializedObject.FindProperty("m_SplashProcedureDuration");
            m_UseUIPreload = serializedObject.FindProperty("m_UseUIPreload");
            m_UIPreloadABPath = serializedObject.FindProperty("m_UIPreloadABPath");
            m_UIPreloadAssetName = serializedObject.FindProperty("m_UIPreloadAssetName");

            // 获取流程状态名称集合
            string[] procedureNames = AorType.GetTypeNames(typeof(ProcedureState));

            // 初始化切换过渡标记位
            int numOffset = procedureNames.Length - m_ProcedureTransitionEnterFlags.arraySize;
            if (numOffset >= 0)
            {
                for (int index = 0; index < numOffset; index++)
                {
                    m_ProcedureTransitionEnterFlags.InsertArrayElementAtIndex(index);
                    m_ProcedureTransitionEnterFlags.GetArrayElementAtIndex(index).boolValue = false;
                }
            }
            else
            {
                for (int index = 0; index < -numOffset; index++)
                {
                    m_ProcedureTransitionEnterFlags.DeleteArrayElementAtIndex(index);
                }
            }

            numOffset = procedureNames.Length - m_ProcedureTransitionEnterDurations.arraySize;
            if (numOffset >= 0)
            {
                for (int index = 0; index < numOffset; index++)
                {
                    m_ProcedureTransitionEnterDurations.InsertArrayElementAtIndex(index);
                    m_ProcedureTransitionEnterDurations.GetArrayElementAtIndex(index).floatValue = 1f;
                }
            }
            else
            {
                for (int index = 0; index < -numOffset; index++)
                {
                    m_ProcedureTransitionEnterDurations.DeleteArrayElementAtIndex(index);
                }
            }

            numOffset = procedureNames.Length - m_ProcedureTransitionEnterBlockRaycasts.arraySize;
            if (numOffset >= 0)
            {
                for (int index = 0; index < numOffset; index++)
                {
                    m_ProcedureTransitionEnterBlockRaycasts.InsertArrayElementAtIndex(index);
                    m_ProcedureTransitionEnterBlockRaycasts.GetArrayElementAtIndex(index).boolValue = true;
                }
            }
            else
            {
                for (int index = 0; index < -numOffset; index++)
                {
                    m_ProcedureTransitionEnterBlockRaycasts.DeleteArrayElementAtIndex(index);
                }
            }

            numOffset = procedureNames.Length - m_ProcedureTransitionExitFlags.arraySize;
            if (numOffset >= 0)
            {
                for (int index = 0; index < numOffset; index++)
                {
                    m_ProcedureTransitionExitFlags.InsertArrayElementAtIndex(index);
                    m_ProcedureTransitionExitFlags.GetArrayElementAtIndex(index).boolValue = false;
                }
            }
            else
            {
                for (int index = 0; index < -numOffset; index++)
                {
                    m_ProcedureTransitionExitFlags.DeleteArrayElementAtIndex(index);
                }
            }

            numOffset = procedureNames.Length - m_ProcedureTransitionExitDurations.arraySize;
            if (numOffset >= 0)
            {
                for (int index = 0; index < numOffset; index++)
                {
                    m_ProcedureTransitionExitDurations.InsertArrayElementAtIndex(index);
                    m_ProcedureTransitionExitDurations.GetArrayElementAtIndex(index).floatValue = 1f;
                }
            }
            else
            {
                for (int index = 0; index < -numOffset; index++)
                {
                    m_ProcedureTransitionExitDurations.DeleteArrayElementAtIndex(index);
                }
            }

            numOffset = procedureNames.Length - m_ProcedureTransitionExitBlockRaycasts.arraySize;
            if (numOffset >= 0)
            {
                for (int index = 0; index < numOffset; index++)
                {
                    m_ProcedureTransitionExitBlockRaycasts.InsertArrayElementAtIndex(index);
                    m_ProcedureTransitionExitBlockRaycasts.GetArrayElementAtIndex(index).boolValue = true;
                }
            }
            else
            {
                for (int index = 0; index < -numOffset; index++)
                {
                    m_ProcedureTransitionExitBlockRaycasts.DeleteArrayElementAtIndex(index);
                }
            }

            // 初始化序列化队列数据
            m_ProcedureTypeNames.ClearArray();
            for (int index = 0; index < procedureNames.Length; index++)
            {
                m_ProcedureTypeNames.InsertArrayElementAtIndex(index);
                m_ProcedureTypeNames.GetArrayElementAtIndex(index).stringValue = procedureNames[index];

                // 判定已经选中的入口流程
                if (m_EntryProcedureTypeName.stringValue == procedureNames[index])
                {
                    m_EntryProcedureIndex = index;
                }

                if (procedureNames[index] == "Honor.Runtime.ProcedureLaunch")
                {
                    m_ProcedureTransitionEnterFlags.GetArrayElementAtIndex(index).boolValue = false;
                    m_ProcedureTransitionEnterDurations.GetArrayElementAtIndex(index).floatValue = 0f;
                    m_ProcedureTransitionEnterBlockRaycasts.GetArrayElementAtIndex(index).boolValue = true;

                    m_ProcedureTransitionExitFlags.GetArrayElementAtIndex(index).boolValue = false;
                    m_ProcedureTransitionExitDurations.GetArrayElementAtIndex(index).floatValue = 0f;
                    m_ProcedureTransitionExitBlockRaycasts.GetArrayElementAtIndex(index).boolValue = true;
                }
            }

            if (string.IsNullOrEmpty(m_UISplashABPath.stringValue))
            {
                m_UISplashABPath.stringValue = m_UIABPathDefault;
            }

            if (string.IsNullOrEmpty(m_UISplashAssetName.stringValue))
            {
                m_UISplashAssetName.stringValue = m_UISplashAssetNameDefault;
            }

            if (string.IsNullOrEmpty(m_UITransitionABPath.stringValue))
            {
                m_UITransitionABPath.stringValue = m_UIABPathDefault;
            }

            if (string.IsNullOrEmpty(m_UITransitionAssetName.stringValue))
            {
                m_UITransitionAssetName.stringValue = m_UITransitionAssetNameDefault;
            }

            if (string.IsNullOrEmpty(m_UIPreloadABPath.stringValue))
            {
                m_UIPreloadABPath.stringValue = m_UIABPathDefault;
            }

            if (string.IsNullOrEmpty(m_UIPreloadAssetName.stringValue))
            {
                m_UIPreloadAssetName.stringValue = m_UIPreloadAssetNameDefault;
            }

            serializedObject.ApplyModifiedProperties();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            ProcedureComponent t = (ProcedureComponent)target;

            if (string.IsNullOrEmpty(m_EntryProcedureTypeName.stringValue))
            {
                EditorGUILayout.HelpBox("入口流程无效。", MessageType.Error);
            }
            else if (EditorApplication.isPlaying)
            {
                EditorGUILayout.LabelField("当前流程", t.CurrentProcedure == null ? "None" : t.CurrentProcedure.GetType().ToString());
            }

            EditorGUI.BeginDisabledGroup(EditorApplication.isPlayingOrWillChangePlaymode);
            {
                List<string> procedureTypeNames = new List<string>();
                EditorGUILayout.LabelField("可用流程");
                if (m_ProcedureTypeNames.arraySize > 0)
                {
                    EditorGUILayout.BeginVertical("box");
                    {
                        for (int i = 0; i < m_ProcedureTypeNames.arraySize; i++)
                        {
                            string procedureName = m_ProcedureTypeNames.GetArrayElementAtIndex(i).stringValue;

                            string[] words = m_ProcedureTypeNames.GetArrayElementAtIndex(i).stringValue.Split('.');
                            string luaScriptName = words[words.Length - 1];

                            procedureTypeNames.Add(procedureName);

                            EditorGUILayout.BeginVertical("box");
                            EditorGUILayout.LabelField(AorTxt.Format("[{0}]", m_ProcedureTypeNames.GetArrayElementAtIndex(i).stringValue));

                            if (procedureName == "Honor.Runtime.ProcedureLaunch")
                            {
                                EditorGUI.BeginDisabledGroup(true);
                            }

                            if (procedureName == "Honor.Runtime.ProcedurePreload")
                            {
                                EditorGUILayout.BeginHorizontal("box", new[] { GUILayout.Width(500) });
                                m_ProcedureTransitionEnterFlags.GetArrayElementAtIndex(i).boolValue = EditorGUILayout.ToggleLeft("进入式-切换过渡（前次流程非热更新时有效）", m_ProcedureTransitionEnterFlags.GetArrayElementAtIndex(i).boolValue, new[] { GUILayout.Width(350) });
                                EditorGUI.BeginDisabledGroup(!m_ProcedureTransitionEnterFlags.GetArrayElementAtIndex(i).boolValue);
                                {
                                    GUILayout.FlexibleSpace();
                                    EditorGUILayout.LabelField("过渡时间", new[] { GUILayout.Width(60) });
                                    m_ProcedureTransitionEnterDurations.GetArrayElementAtIndex(i).floatValue = EditorGUILayout.FloatField(m_ProcedureTransitionEnterDurations.GetArrayElementAtIndex(i).floatValue, new[] { GUILayout.Width(60) });
                                    GUILayout.FlexibleSpace();
                                    m_ProcedureTransitionEnterBlockRaycasts.GetArrayElementAtIndex(i).boolValue = EditorGUILayout.ToggleLeft("阻塞射线", m_ProcedureTransitionEnterBlockRaycasts.GetArrayElementAtIndex(i).boolValue, new[] { GUILayout.Width(100) });
                                }
                                EditorGUI.EndDisabledGroup();
                                EditorGUILayout.EndHorizontal();

                                EditorGUILayout.BeginHorizontal("box", new[] { GUILayout.Width(500) });
                                m_ProcedureTransitionExitFlags.GetArrayElementAtIndex(i).boolValue = EditorGUILayout.ToggleLeft("退出式-切换过渡（前次流程非热更新时有效）", m_ProcedureTransitionExitFlags.GetArrayElementAtIndex(i).boolValue, new[] { GUILayout.Width(350) });
                                EditorGUI.BeginDisabledGroup(!m_ProcedureTransitionExitFlags.GetArrayElementAtIndex(i).boolValue);
                                {
                                    GUILayout.FlexibleSpace();
                                    EditorGUILayout.LabelField("过渡时间", new[] { GUILayout.Width(60) });
                                    m_ProcedureTransitionExitDurations.GetArrayElementAtIndex(i).floatValue = EditorGUILayout.FloatField(m_ProcedureTransitionExitDurations.GetArrayElementAtIndex(i).floatValue, new[] { GUILayout.Width(60) });
                                    GUILayout.FlexibleSpace();
                                    m_ProcedureTransitionExitBlockRaycasts.GetArrayElementAtIndex(i).boolValue = EditorGUILayout.ToggleLeft("阻塞射线", m_ProcedureTransitionExitBlockRaycasts.GetArrayElementAtIndex(i).boolValue, new[] { GUILayout.Width(100) });
                                }
                                EditorGUI.EndDisabledGroup();
                                EditorGUILayout.EndHorizontal();

                                EditorGUILayout.BeginHorizontal("box", new[] { GUILayout.Width(500) });
                                m_ProcedureTransitionEnterFlagFromProcedureHotfix.boolValue = EditorGUILayout.ToggleLeft("进入式-切换过渡（前次流程为热更新时有效）", m_ProcedureTransitionEnterFlagFromProcedureHotfix.boolValue, new[] { GUILayout.Width(350) });
                                EditorGUI.BeginDisabledGroup(!m_ProcedureTransitionEnterFlagFromProcedureHotfix.boolValue);
                                {
                                    GUILayout.FlexibleSpace();
                                    EditorGUILayout.LabelField("过渡时间", new[] { GUILayout.Width(60) });
                                    m_ProcedureTransitionEnterDurationFromProcedureHotfix.floatValue = EditorGUILayout.FloatField(m_ProcedureTransitionEnterDurationFromProcedureHotfix.floatValue, new[] { GUILayout.Width(60) });
                                    GUILayout.FlexibleSpace();
                                    m_ProcedureTransitionEnterBlockRaycastFromProcedureHotfix.boolValue = EditorGUILayout.ToggleLeft("阻塞射线", m_ProcedureTransitionEnterBlockRaycastFromProcedureHotfix.boolValue, new[] { GUILayout.Width(100) });
                                }
                                EditorGUI.EndDisabledGroup();
                                EditorGUILayout.EndHorizontal();

                                EditorGUILayout.BeginHorizontal("box", new[] { GUILayout.Width(500) });
                                m_ProcedureTransitionExitFlagFromProcedureHotfix.boolValue = EditorGUILayout.ToggleLeft("退出式-切换过渡（前次流程为热更新时有效）", m_ProcedureTransitionExitFlagFromProcedureHotfix.boolValue, new[] { GUILayout.Width(350) });
                                EditorGUI.BeginDisabledGroup(!m_ProcedureTransitionExitFlagFromProcedureHotfix.boolValue);
                                {
                                    GUILayout.FlexibleSpace();
                                    EditorGUILayout.LabelField("过渡时间", new[] { GUILayout.Width(60) });
                                    m_ProcedureTransitionExitDurationFromProcedureHotfix.floatValue = EditorGUILayout.FloatField(m_ProcedureTransitionExitDurationFromProcedureHotfix.floatValue, new[] { GUILayout.Width(60) });
                                    GUILayout.FlexibleSpace();
                                    m_ProcedureTransitionExitBlockRaycastFromProcedureHotfix.boolValue = EditorGUILayout.ToggleLeft("阻塞射线", m_ProcedureTransitionExitBlockRaycastFromProcedureHotfix.boolValue, new[] { GUILayout.Width(100) });
                                }
                                EditorGUI.EndDisabledGroup();
                                EditorGUILayout.EndHorizontal();
                            }
                            else
                            {
                                EditorGUILayout.BeginHorizontal("box", new[] { GUILayout.Width(400) });
                                m_ProcedureTransitionEnterFlags.GetArrayElementAtIndex(i).boolValue = EditorGUILayout.ToggleLeft("进入式-切换过渡", m_ProcedureTransitionEnterFlags.GetArrayElementAtIndex(i).boolValue, new[] { GUILayout.Width(120) });
                                EditorGUI.BeginDisabledGroup(!m_ProcedureTransitionEnterFlags.GetArrayElementAtIndex(i).boolValue);
                                {
                                    GUILayout.FlexibleSpace();
                                    EditorGUILayout.LabelField("过渡时间", new[] { GUILayout.Width(60) });
                                    m_ProcedureTransitionEnterDurations.GetArrayElementAtIndex(i).floatValue = EditorGUILayout.FloatField(m_ProcedureTransitionEnterDurations.GetArrayElementAtIndex(i).floatValue, new[] { GUILayout.Width(60) });
                                    GUILayout.FlexibleSpace();
                                    m_ProcedureTransitionEnterBlockRaycasts.GetArrayElementAtIndex(i).boolValue = EditorGUILayout.ToggleLeft("阻塞射线", m_ProcedureTransitionEnterBlockRaycasts.GetArrayElementAtIndex(i).boolValue, new[] { GUILayout.Width(100) });
                                }
                                EditorGUI.EndDisabledGroup();
                                EditorGUILayout.EndHorizontal();

                                EditorGUILayout.BeginHorizontal("box", new[] { GUILayout.Width(400) });
                                m_ProcedureTransitionExitFlags.GetArrayElementAtIndex(i).boolValue = EditorGUILayout.ToggleLeft("退出式-切换过渡", m_ProcedureTransitionExitFlags.GetArrayElementAtIndex(i).boolValue, new[] { GUILayout.Width(120) });
                                EditorGUI.BeginDisabledGroup(!m_ProcedureTransitionExitFlags.GetArrayElementAtIndex(i).boolValue);
                                {
                                    GUILayout.FlexibleSpace();
                                    EditorGUILayout.LabelField("过渡时间", new[] { GUILayout.Width(60) });
                                    m_ProcedureTransitionExitDurations.GetArrayElementAtIndex(i).floatValue = EditorGUILayout.FloatField(m_ProcedureTransitionExitDurations.GetArrayElementAtIndex(i).floatValue, new[] { GUILayout.Width(60) });
                                    GUILayout.FlexibleSpace();
                                    m_ProcedureTransitionExitBlockRaycasts.GetArrayElementAtIndex(i).boolValue = EditorGUILayout.ToggleLeft("阻塞射线", m_ProcedureTransitionExitBlockRaycasts.GetArrayElementAtIndex(i).boolValue, new[] { GUILayout.Width(100) });
                                }
                                EditorGUI.EndDisabledGroup();
                                EditorGUILayout.EndHorizontal();
                            }

                            if (ProcedureComponent.LuaScriptWhiteNameList.Contains(luaScriptName))
                            {
                                if (GUILayout.Button("生成/刷新Lua脚本"))
                                {
                                    string luaRootPath = AorTxt.Format("{0}{1}", Application.dataPath.Substring(0, Application.dataPath.Length - "Assets".Length), GamePathUtils.LuaScript.Game.GetRootDirectoryRelativePath(true));
                                    if (!System.IO.Directory.Exists(luaRootPath))
                                    {
                                        System.IO.Directory.CreateDirectory(luaRootPath);
                                        AssetDatabase.Refresh();
                                    }
                                    string[] fileFullPaths = System.IO.Directory.GetFiles(luaRootPath, AorTxt.Format("{0}.lua.txt", luaScriptName), System.IO.SearchOption.AllDirectories);
                                    // luaRootPath目录及其子目录中没有该名称的lua文件时需要调起保存窗口
                                    if (fileFullPaths.Length == 0)
                                    {
                                        string exportFileName = EditorUtility.SaveFilePanel(AorTxt.Format("生成{0}.lua.txt文件", luaScriptName), luaRootPath, AorTxt.Format("{0}.lua.txt", luaScriptName), string.Empty);
                                        if (!string.IsNullOrEmpty(exportFileName))
                                        {
                                            try
                                            {
                                                StringBuilder stringBuilderCommentLines = GenerateCommentLines(luaScriptName);
                                                StringBuilder stringBuilderEmptyCodeLines = GenerateEmptyCodeLines(luaScriptName);
                                                string content = AorTxt.Format("{0}{1}", stringBuilderCommentLines.ToString(), stringBuilderEmptyCodeLines.ToString());
                                                System.IO.File.WriteAllText(exportFileName, content, new System.Text.UTF8Encoding(false));

                                                Log.Debug(AorTxt.Format("生成{0}文件成功。", exportFileName));

                                                AssetDatabase.Refresh();
                                            }
                                            catch (Exception exception)
                                            {
                                                Log.Error(AorTxt.Format("生成{0}文件失败, 异常信息： '{1}'.", exportFileName, exception.ToString()));
                                            }
                                        }

                                    }
                                    // luaRootPath目录及其子目录中存在该名称的lua文件时需要刷新文件内容
                                    else if (fileFullPaths.Length == 1)
                                    {
                                        try
                                        {
                                            StringBuilder stringBuilderCommentLines = GenerateCommentLines(luaScriptName);
                                            StringBuilder stringBuilderCodeLines = GenerateCodeLines(luaScriptName, fileFullPaths[0]);
                                            string content = AorTxt.Format("{0}{1}", stringBuilderCommentLines.ToString(), stringBuilderCodeLines.ToString());
                                            System.IO.File.WriteAllText(fileFullPaths[0], content, new System.Text.UTF8Encoding(false));

                                            Log.Debug(AorTxt.Format("刷新{0}文件成功。", fileFullPaths[0]));

                                            AssetDatabase.Refresh();
                                        }
                                        catch (Exception exception)
                                        {
                                            Log.Error(AorTxt.Format("刷新{0}文件失败, 异常信息： '{1}'.", fileFullPaths[0], exception.ToString()));
                                        }
                                    }
                                    else
                                    {
                                        Log.Error(AorTxt.Format("目录 {0} 及其子目录中遍历到多个文件 {1}.txt ！", luaRootPath, luaScriptName));
                                    }
                                    GUIUtility.ExitGUI();
                                }
                            }

                            if (procedureName == "Honor.Runtime.ProcedureLaunch")
                            {
                                EditorGUI.EndDisabledGroup();
                            }

                            EditorGUILayout.EndVertical();
                        }
                    }
                    EditorGUILayout.EndVertical();
                }
                else
                {
                    EditorGUILayout.HelpBox("没有可用流程", MessageType.Warning);
                }

                if (m_ProcedureTypeNames.arraySize > 0)
                {
                    EditorGUILayout.Separator();

                    int selectedIndex = EditorGUILayout.Popup("入口流程", m_EntryProcedureIndex, procedureTypeNames.ToArray());
                    if (selectedIndex != m_EntryProcedureIndex)
                    {
                        m_EntryProcedureIndex = selectedIndex;
                        m_EntryProcedureTypeName.stringValue = procedureTypeNames[selectedIndex];
                    }
                    EditorGUILayout.HelpBox("请以ProcedureLaunch为入口流程，不要修改为其他流程，除非明确了解流程！", MessageType.Info);
                }
                else
                {
                    EditorGUILayout.HelpBox("先选择可用流程。", MessageType.Info);
                }
            }
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.BeginVertical("box");

            m_UITransitionABPath.stringValue = EditorGUILayout.TextField("切换过渡界面路径", m_UITransitionABPath.stringValue);
            m_UITransitionAssetName.stringValue = EditorGUILayout.TextField("切换过渡界面资源名称", m_UITransitionAssetName.stringValue);

            if (GUILayout.Button("使用默认切换过渡界面"))
            {
                m_UITransitionABPath.stringValue = m_UIABPathDefault;
                m_UITransitionAssetName.stringValue = m_UITransitionAssetNameDefault;
                serializedObject.ApplyModifiedProperties();
                GUIUtility.ExitGUI();
            }

            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical("box");

            m_UISplashABPath.stringValue = EditorGUILayout.TextField("闪屏界面路径", m_UISplashABPath.stringValue);
            m_UISplashAssetName.stringValue = EditorGUILayout.TextField("闪屏界面资源名称", m_UISplashAssetName.stringValue);
            m_SplashProcedureDuration.intValue = EditorGUILayout.IntField("闪屏流程持续时间", m_SplashProcedureDuration.intValue);

            if (GUILayout.Button("使用默认闪屏界面"))
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
                m_UIPreloadABPath.stringValue = EditorGUILayout.TextField("预加载界面路径", m_UIPreloadABPath.stringValue);
                m_UIPreloadAssetName.stringValue = EditorGUILayout.TextField("预加载界面资源名称", m_UIPreloadAssetName.stringValue);
                if (GUILayout.Button("使用默认预加载界面"))
                {
                    m_UIPreloadABPath.stringValue = m_UIABPathDefault;
                    m_UIPreloadAssetName.stringValue = m_UIPreloadAssetNameDefault;
                    serializedObject.ApplyModifiedProperties();
                    GUIUtility.ExitGUI();
                }
            }
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.EndVertical();

            if (Application.isPlaying)
            {
                EditorGUILayout.BeginVertical("box");
                EditorGUILayout.HelpBox("运行时流程跳转示意图。", MessageType.Info);
                for (int index = 0; index < t.RuntimeProcedureRecordInfos.Count; index++)
                {
                    EditorGUILayout.LabelField(t.RuntimeProcedureRecordInfos[index]);
                    EditorGUILayout.LabelField("↓");
                    if (index == t.RuntimeProcedureRecordInfos.Count - 1)
                    {
                        EditorGUILayout.LabelField(AorTxt.Format("{0}\t（{1:N2}秒）", t.CurrentProcedure.GetType().ToString(), t.CurrentProcedureTime));
                    }
                }
                if (GUILayout.Button("导出 CSV 数据"))
                {
                    string exportFileName = EditorUtility.SaveFilePanel("导出 CSV 数据", string.Empty, AorTxt.Format("流程跳转信息 {0}.csv", DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss")), string.Empty);
                    if (!string.IsNullOrEmpty(exportFileName))
                    {
                        try
                        {
                            int index = 0;
                            string[] data = new string[t.RuntimeProcedureRecordInfos.Count + 2];
                            data[index++] = "Procedure信息（按从上到下的顺序）";
                            for (; index < t.RuntimeProcedureRecordInfos.Count; index++)
                            {
                                data[index] = t.RuntimeProcedureRecordInfos[index];
                            }
                            data[index] = AorTxt.Format("{0}\t（{1:N2}秒）", t.CurrentProcedure.GetType().ToString(), t.CurrentProcedureTime);

                            System.IO.File.WriteAllLines(exportFileName, data, new System.Text.UTF8Encoding(false));
                            Log.Debug(AorTxt.Format("导出 CSV 数据到 '{0}' 成功。", exportFileName));
                        }
                        catch (Exception exception)
                        {
                            Log.Error(AorTxt.Format("导出 CSV 数据到 '{0}' 失败, 异常信息： '{1}'.", exportFileName, exception.ToString()));
                        }
                    }
                    GUIUtility.ExitGUI();
                }
                EditorGUILayout.EndVertical();
            }

            serializedObject.ApplyModifiedProperties();

            Repaint();
        }

        protected override void OnCompileComplete()
        {
            base.OnCompileComplete();

        }

        /// <summary>
        /// 生成注释行信息
        /// </summary>
        private StringBuilder GenerateCommentLines(string luaScriptName)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("--=====================================================================================================")
                         .AppendLine("-- (c) copyright 2026 - 2030, Honor.Game")
                         .AppendLine("-- All Rights Reserved.")
                         .AppendLine("-- ----------------------------------------------------------------------------------------------------")
                         .AppendLine(AorTxt.Format("-- filename:  {0}.lua", luaScriptName))
                         .AppendLine("--=====================================================================================================")
                         .AppendLine("");
            return stringBuilder;
        }

        /// <summary>
        /// 生成空行
        /// </summary>
        private StringBuilder GenerateEmptyCodeLines(string luaScriptName)
        {
            string luaName = luaScriptName;

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(AorTxt.Format("---@class {0} : ProcedureSuper", luaName))
                         .AppendLine(AorTxt.Format("local {0} = class('{1}', import('ProcedureSuper'))", luaName, luaName))
                         .AppendLine(AorTxt.Format(""))
                         .AppendLine(AorTxt.Format("---构造函数"))
                         .AppendLine(AorTxt.Format("---@type fun(args:table):void"))
                         .AppendLine(AorTxt.Format("---@param args table @自定义参数"))
                         .AppendLine(AorTxt.Format("function {0}:ctor(args)", luaName))
                         .AppendLine(AorTxt.Format("    {0}.super.ctor(self, args)", luaName))
                         .AppendLine(AorTxt.Format(""))
                         .AppendLine(AorTxt.Format("end"))
                         .AppendLine(AorTxt.Format(""))
                         .AppendLine(AorTxt.Format("---创建函数"))
                         .AppendLine(AorTxt.Format("---@type fun(args:table):{0}", luaName))
                         .AppendLine(AorTxt.Format("---@param args table @自定义参数"))
                         .AppendLine(AorTxt.Format("---@return {0} @流程实例", luaName))
                         .AppendLine(AorTxt.Format("function {0}:Create(args)", luaName))
                         .AppendLine(AorTxt.Format("    local obj = {0}.new(args)", luaName))
                         .AppendLine(AorTxt.Format("    return obj"))
                         .AppendLine(AorTxt.Format("end"))
                         .AppendLine(AorTxt.Format(""))
                         .AppendLine(AorTxt.Format("---进入"))
                         .AppendLine(AorTxt.Format("---@type fun(ownerMachine: userdata):void"))
                         .AppendLine(AorTxt.Format("---@param ownerMachine userdata @流程状态"))
                         .AppendLine(AorTxt.Format("function {0}:OnEnter(ownerMachine)", luaName))
                         .AppendLine(AorTxt.Format("    {0}.super.OnEnter(self, ownerMachine)", luaName))
                         .AppendLine(AorTxt.Format(""))
                         .AppendLine(AorTxt.Format("end"))
                         .AppendLine(AorTxt.Format(""))
                         .AppendLine(AorTxt.Format("---心跳"))
                         .AppendLine(AorTxt.Format("---@type fun(ownerMachine: userdata):void"))
                         .AppendLine(AorTxt.Format("---@param ownerMachine userdata @流程状态"))
                         .AppendLine(AorTxt.Format("function {0}:OnUpdate(ownerMachine)", luaName))
                         .AppendLine(AorTxt.Format("    {0}.super.OnUpdate(self, ownerMachine)", luaName))
                         .AppendLine(AorTxt.Format(""))
                         .AppendLine(AorTxt.Format("end"))
                         .AppendLine(AorTxt.Format(""))
                         .AppendLine(AorTxt.Format("---离开"))
                         .AppendLine(AorTxt.Format("---@type fun(ownerMachine: userdata):void"))
                         .AppendLine(AorTxt.Format("---@param ownerMachine userdata @流程状态"))
                         .AppendLine(AorTxt.Format("function {0}:OnLeave(ownerMachine)", luaName))
                         .AppendLine(AorTxt.Format("    {0}.super.OnLeave(self, ownerMachine)", luaName))
                         .AppendLine(AorTxt.Format(""))
                         .AppendLine(AorTxt.Format("end"))
                         .AppendLine(AorTxt.Format(""))
                         .AppendLine(AorTxt.Format("return {0}", luaName))
                         .AppendLine(AorTxt.Format(""));
            return stringBuilder;
        }

        /// <summary>
        /// 截取现有有效代码行
        /// </summary>
        /// <param name="fullPath">全路径</param>
        /// <returns></returns>
        private StringBuilder GenerateCodeLines(string luaScriptName, string fullPath)
        {
            string luaName = luaScriptName;

            string content = System.IO.File.ReadAllText(fullPath);
            string endCommentStr = "--=====================================================================================================";
            int endCommentStrIndex = content.LastIndexOf(endCommentStr) + endCommentStr.Length + 4;

            content = content.Remove(0, endCommentStrIndex);

            List<List<string>> commentDefs = new List<List<string>>();
            commentDefs.Add(new List<string>(new string[] { AorTxt.Format("---构造函数"), AorTxt.Format("---@type fun(args:table):void"), AorTxt.Format("---@param args table @自定义参数") }));
            commentDefs.Add(new List<string>(new string[] { AorTxt.Format("---创建函数"), AorTxt.Format("---@type fun(args:table):{0}", luaName), AorTxt.Format("---@param args table @自定义参数"), AorTxt.Format("---@return {0} @流程实例", luaName) }));
            commentDefs.Add(new List<string>(new string[] { AorTxt.Format("---进入"), AorTxt.Format("---@type fun(ownerMachine: userdata):void"), AorTxt.Format("---@param ownerMachine userdata @流程状态") }));
            commentDefs.Add(new List<string>(new string[] { AorTxt.Format("---心跳"), AorTxt.Format("---@type fun(ownerMachine: userdata):void"), AorTxt.Format("---@param ownerMachine userdata @流程状态") }));
            commentDefs.Add(new List<string>(new string[] { AorTxt.Format("---离开"), AorTxt.Format("---@type fun(ownerMachine: userdata):void"), AorTxt.Format("---@param ownerMachine userdata @流程状态") }));

            List<string> functionDefs = new List<string>();
            functionDefs.Add(AorTxt.Format("function {0}:ctor(args)", luaName));
            functionDefs.Add(AorTxt.Format("function {0}:Create(args)", luaName));
            functionDefs.Add(AorTxt.Format("function {0}:OnEnter(ownerMachine)", luaName));
            functionDefs.Add(AorTxt.Format("function {0}:OnUpdate(ownerMachine)", luaName));
            functionDefs.Add(AorTxt.Format("function {0}:OnLeave(ownerMachine)", luaName));
            int index = 0;
            foreach (var functionDef in functionDefs)
            {
                if (!content.Contains(functionDef))
                {
                    foreach (var commentDef in commentDefs[index])
                    {
                        content = AorTxt.Format("{0}{1}\r\n", content, commentDef);
                    }
                    content = AorTxt.Format("{0}{1}\r\n", content, functionDef);
                    content = AorTxt.Format("{0}\r\n", content);
                    content = AorTxt.Format("{0}end\r\n", content);
                    content = AorTxt.Format("{0}\r\n", content);
                }
                index++;
            }

            string returnEndStr = AorTxt.Format("return {0}\r\n", luaName);
            content = content.Remove(content.LastIndexOf(returnEndStr), returnEndStr.Length);
            content = AorTxt.Format("{0}{1}", content, returnEndStr);

            StringBuilder stringBuilder = new StringBuilder(content);
            return stringBuilder;
        }
    }
}