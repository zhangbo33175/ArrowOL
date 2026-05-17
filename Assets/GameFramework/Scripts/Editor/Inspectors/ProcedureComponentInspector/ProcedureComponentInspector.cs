/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Game
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  ProcedureComponentInspector.cs
 * author:    云毅
 *  created:   2026
 * descrip:   流程系统编辑器面板
 *            可视化配置流程过渡、UI、自动生成 Lua 脚本、运行时监控
 ***************************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using Honor.Runtime;
using UnityEditor;
using UnityEngine;

namespace Honor.Editor
{
    /// <summary>
    /// 【流程系统编辑器面板】
    /// 功能：游戏流程（状态机）的可视化配置、过渡动画设置、Lua脚本自动生成、运行时监控
    /// 作用：管理游戏从启动→闪屏→加载→主界面→战斗的整个流程切换
    /// 属于：框架 -> 流程(Procedure)系统 -> 编辑器扩展
    /// </summary>
    [CustomEditor(typeof(ProcedureComponent))]
    internal sealed class ProcedureComponentInspector : HonorComponentInspector
    {
        private static readonly string UI_AB_PATH_DEFAULT = GamePathUtils.Prefab.GetFrameworkRootDirectoryRelativePath();

        private const string UI_SPLASH_DEFAULT           = "UILauncherView";
        private const string UI_TRANSITION_DEFAULT       = "UILauncherLogoView";
        private const string UI_PRELOAD_DEFAULT          = "UIPreloadDefault";

        /// <summary>
        /// 流程列表序列化属性
        /// </summary>
        private SerializedProperty m_ProcedureTypeNames;
        private SerializedProperty m_ProcedureTransitionEnterFlags;
        private SerializedProperty m_ProcedureTransitionEnterDurations;
        private SerializedProperty m_ProcedureTransitionEnterBlockRaycasts;
        private SerializedProperty m_ProcedureTransitionExitFlags;
        private SerializedProperty m_ProcedureTransitionExitDurations;
        private SerializedProperty m_ProcedureTransitionExitBlockRaycasts;
        private SerializedProperty m_ProcedureTransitionEnterFlagFromProcedureHotfix;
        private SerializedProperty m_ProcedureTransitionEnterDurationFromProcedureHotfix;
        private SerializedProperty m_ProcedureTransitionEnterBlockRaycastFromProcedureHotfix;
        private SerializedProperty m_ProcedureTransitionExitFlagFromProcedureHotfix;
        private SerializedProperty m_ProcedureTransitionExitDurationFromProcedureHotfix;
        private SerializedProperty m_ProcedureTransitionExitBlockRaycastFromProcedureHotfix;
        private SerializedProperty m_EntryProcedureTypeName;

        /// <summary>
        /// 流程关联 UI 配置
        /// </summary>
        private SerializedProperty m_UITransitionABPath;
        private SerializedProperty m_UITransitionAssetName;
        private SerializedProperty m_UISplashABPath;
        private SerializedProperty m_UISplashAssetName;
        private SerializedProperty m_SplashProcedureDuration;
        private SerializedProperty m_UseUIPreload;
        private SerializedProperty m_UIPreloadABPath;
        private SerializedProperty m_UIPreloadAssetName;

        /// <summary>
        /// 入口流程索引
        /// </summary>
        private int m_EntryProcedureIndex = -1;

        /// <summary>
        /// 启用初始化
        /// </summary>
        private void OnEnable()
        {
            // 绑定序列化字段
            BindSerializedProperties();

            // 反射获取所有流程
            string[] procedureNames = AorType.GetTypeNames(typeof(ProcedureState));

            // 自动初始化过渡数组
            AutoInitTransitionArrays(procedureNames);

            // 刷新流程列表
            RefreshProcedureList(procedureNames);

            // 设置 UI 默认值
            SetUIDefaultValues();

            serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        /// 绑定所有序列化属性
        /// </summary>
        private void BindSerializedProperties()
        {
            m_ProcedureTypeNames                                      = serializedObject.FindProperty("m_ProcedureTypeNames");
            m_ProcedureTransitionEnterFlags                            = serializedObject.FindProperty("m_ProcedureTransitionEnterFlags");
            m_ProcedureTransitionEnterDurations                        = serializedObject.FindProperty("m_ProcedureTransitionEnterDurations");
            m_ProcedureTransitionEnterBlockRaycasts                   = serializedObject.FindProperty("m_ProcedureTransitionEnterBlockRaycasts");
            m_ProcedureTransitionExitFlags                             = serializedObject.FindProperty("m_ProcedureTransitionExitFlags");
            m_ProcedureTransitionExitDurations                         = serializedObject.FindProperty("m_ProcedureTransitionExitDurations");
            m_ProcedureTransitionExitBlockRaycasts                    = serializedObject.FindProperty("m_ProcedureTransitionExitBlockRaycasts");
            m_ProcedureTransitionEnterFlagFromProcedureHotfix         = serializedObject.FindProperty("m_ProcedureTransitionEnterFlagFromProcedureHotfix");
            m_ProcedureTransitionEnterDurationFromProcedureHotfix     = serializedObject.FindProperty("m_ProcedureTransitionEnterDurationFromProcedureHotfix");
            m_ProcedureTransitionEnterBlockRaycastFromProcedureHotfix = serializedObject.FindProperty("m_ProcedureTransitionEnterBlockRaycastFromProcedureHotfix");
            m_ProcedureTransitionExitFlagFromProcedureHotfix          = serializedObject.FindProperty("m_ProcedureTransitionExitFlagFromProcedureHotfix");
            m_ProcedureTransitionExitDurationFromProcedureHotfix      = serializedObject.FindProperty("m_ProcedureTransitionExitDurationFromProcedureHotfix");
            m_ProcedureTransitionExitBlockRaycastFromProcedureHotfix  = serializedObject.FindProperty("m_ProcedureTransitionExitBlockRaycastFromProcedureHotfix");
            m_EntryProcedureTypeName                                   = serializedObject.FindProperty("m_EntryProcedureTypeName");
            
            m_UITransitionABPath          = serializedObject.FindProperty("m_UITransitionABPath");
            m_UITransitionAssetName       = serializedObject.FindProperty("m_UITransitionAssetName");
            m_UISplashABPath              = serializedObject.FindProperty("m_UISplashABPath");
            m_UISplashAssetName           = serializedObject.FindProperty("m_UISplashAssetName");
            m_SplashProcedureDuration     = serializedObject.FindProperty("m_SplashProcedureDuration");
            m_UseUIPreload                = serializedObject.FindProperty("m_UseUIPreload");
            m_UIPreloadABPath             = serializedObject.FindProperty("m_UIPreloadABPath");
            m_UIPreloadAssetName          = serializedObject.FindProperty("m_UIPreloadAssetName");
        }

        /// <summary>
        /// 自动初始化过渡数组尺寸
        /// </summary>
        private void AutoInitTransitionArrays(string[] procedureNames)
        {
            AutoResizeBoolArray(m_ProcedureTransitionEnterFlags,           procedureNames.Length, false);
            AutoResizeFloatArray(m_ProcedureTransitionEnterDurations,       procedureNames.Length, 1f);
            AutoResizeBoolArray(m_ProcedureTransitionEnterBlockRaycasts,  procedureNames.Length, true);
            AutoResizeBoolArray(m_ProcedureTransitionExitFlags,            procedureNames.Length, false);
            AutoResizeFloatArray(m_ProcedureTransitionExitDurations,        procedureNames.Length, 1f);
            AutoResizeBoolArray(m_ProcedureTransitionExitBlockRaycasts,   procedureNames.Length, true);
        }

        /// <summary>
        /// 自动调整 bool 数组大小
        /// </summary>
        private void AutoResizeBoolArray(SerializedProperty prop, int targetLen, bool defaultValue)
        {
            int offset = targetLen - prop.arraySize;
            if (offset >= 0)
            {
                for (int i = 0; i < offset; i++)
                {
                    prop.InsertArrayElementAtIndex(i);
                    prop.GetArrayElementAtIndex(i).boolValue = defaultValue;
                }
            }
            else
            {
                for (int i = 0; i < -offset; i++)
                {
                    prop.DeleteArrayElementAtIndex(0);
                }
            }
        }

        /// <summary>
        /// 自动调整 float 数组大小
        /// </summary>
        private void AutoResizeFloatArray(SerializedProperty prop, int targetLen, float defaultValue)
        {
            int offset = targetLen - prop.arraySize;
            if (offset >= 0)
            {
                for (int i = 0; i < offset; i++)
                {
                    prop.InsertArrayElementAtIndex(i);
                    prop.GetArrayElementAtIndex(i).floatValue = defaultValue;
                }
            }
            else
            {
                for (int i = 0; i < -offset; i++)
                {
                    prop.DeleteArrayElementAtIndex(0);
                }
            }
        }

        /// <summary>
        /// 刷新流程列表并设置启动流程默认过渡
        /// </summary>
        private void RefreshProcedureList(string[] procedureNames)
        {
            m_ProcedureTypeNames.ClearArray();
            m_EntryProcedureIndex = -1;

            for (int i = 0; i < procedureNames.Length; i++)
            {
                m_ProcedureTypeNames.InsertArrayElementAtIndex(i);
                m_ProcedureTypeNames.GetArrayElementAtIndex(i).stringValue = procedureNames[i];

                // 记录入口流程
                if (m_EntryProcedureTypeName.stringValue == procedureNames[i])
                    m_EntryProcedureIndex = i;

                // 启动流程默认无过渡
                if (procedureNames[i] == "Honor.Runtime.ProcedureLaunch")
                {
                    m_ProcedureTransitionEnterFlags.GetArrayElementAtIndex(i).boolValue = false;
                    m_ProcedureTransitionEnterDurations.GetArrayElementAtIndex(i).floatValue = 0f;
                    m_ProcedureTransitionEnterBlockRaycasts.GetArrayElementAtIndex(i).boolValue = true;

                    m_ProcedureTransitionExitFlags.GetArrayElementAtIndex(i).boolValue = false;
                    m_ProcedureTransitionExitDurations.GetArrayElementAtIndex(i).floatValue = 0f;
                    m_ProcedureTransitionExitBlockRaycasts.GetArrayElementAtIndex(i).boolValue = true;
                }
            }
        }

        /// <summary>
        /// 设置 UI 路径默认值
        /// </summary>
        private void SetUIDefaultValues()
        {
            if (string.IsNullOrEmpty(m_UISplashABPath.stringValue))
                m_UISplashABPath.stringValue = UI_AB_PATH_DEFAULT;
            if (string.IsNullOrEmpty(m_UISplashAssetName.stringValue))
                m_UISplashAssetName.stringValue = UI_SPLASH_DEFAULT;

            if (string.IsNullOrEmpty(m_UITransitionABPath.stringValue))
                m_UITransitionABPath.stringValue = UI_AB_PATH_DEFAULT;
            if (string.IsNullOrEmpty(m_UITransitionAssetName.stringValue))
                m_UITransitionAssetName.stringValue = UI_TRANSITION_DEFAULT;

            if (string.IsNullOrEmpty(m_UIPreloadABPath.stringValue))
                m_UIPreloadABPath.stringValue = UI_AB_PATH_DEFAULT;
            if (string.IsNullOrEmpty(m_UIPreloadAssetName.stringValue))
                m_UIPreloadAssetName.stringValue = UI_PRELOAD_DEFAULT;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();

            ProcedureComponent targetComp = (ProcedureComponent)target;

            // 运行时提示
            DrawRuntimeInfo(targetComp);

            // 编辑模式流程配置
            EditorGUI.BeginDisabledGroup(EditorApplication.isPlayingOrWillChangePlaymode);
            {
                DrawProcedureConfig();
                DrawEntryProcedureSelector();
            }
            EditorGUI.EndDisabledGroup();

            // UI 配置
            DrawUIConfig();

            // 运行时流程记录
            if (Application.isPlaying)
                DrawRuntimeProcedureRecorder(targetComp);

            serializedObject.ApplyModifiedProperties();
            Repaint();
        }

        /// <summary>
        /// 绘制运行时状态信息
        /// </summary>
        private void DrawRuntimeInfo(ProcedureComponent targetComp)
        {
            if (string.IsNullOrEmpty(m_EntryProcedureTypeName.stringValue))
                EditorGUILayout.HelpBox("入口流程无效。", MessageType.Error);
            else if (EditorApplication.isPlaying)
            {
                EditorGUILayout.LabelField("当前流程", 
                    targetComp.CurrentProcedure == null ? "None" : targetComp.CurrentProcedure.GetType().ToString());
            }
        }

        /// <summary>
        /// 绘制流程配置面板
        /// </summary>
        private void DrawProcedureConfig()
        {
            EditorGUILayout.LabelField("可用流程");
            List<string> procedureList = new List<string>();

            if (m_ProcedureTypeNames.arraySize <= 0)
            {
                EditorGUILayout.HelpBox("没有可用流程", MessageType.Warning);
                return;
            }

            EditorGUILayout.BeginVertical("box");
            {
                for (int i = 0; i < m_ProcedureTypeNames.arraySize; i++)
                {
                    string fullName = m_ProcedureTypeNames.GetArrayElementAtIndex(i).stringValue;
                    string[] nameWords = fullName.Split('.');
                    string luaName = nameWords[nameWords.Length - 1];
                    procedureList.Add(fullName);

                    EditorGUILayout.BeginVertical("box");
                    EditorGUILayout.LabelField($"[{fullName}]");

                    // 启动流程禁止编辑
                    bool isLaunch = fullName == "Honor.Runtime.ProcedureLaunch";
                    if (isLaunch) EditorGUI.BeginDisabledGroup(true);

                    // 预加载流程特殊配置
                    if (fullName == "Honor.Runtime.ProcedurePreload")
                        DrawPreloadProcedureTransition(i);
                    else
                        DrawNormalProcedureTransition(i);

                    // Lua 脚本生成按钮
                    if (ProcedureComponent.LuaScriptWhiteNameList.Contains(luaName))
                        DrawGenerateLuaButton(luaName);

                    if (isLaunch) EditorGUI.EndDisabledGroup();
                    EditorGUILayout.EndVertical();
                }
            }
            EditorGUILayout.EndVertical();
        }

        /// <summary>
        /// 绘制普通流程过渡配置
        /// </summary>
        private void DrawNormalProcedureTransition(int index)
        {
            // 进入过渡
            EditorGUILayout.BeginHorizontal("box", GUILayout.Width(400));
            m_ProcedureTransitionEnterFlags.GetArrayElementAtIndex(index).boolValue =
                EditorGUILayout.ToggleLeft("进入式-切换过渡", 
                    m_ProcedureTransitionEnterFlags.GetArrayElementAtIndex(index).boolValue, GUILayout.Width(120));
            
            EditorGUI.BeginDisabledGroup(!m_ProcedureTransitionEnterFlags.GetArrayElementAtIndex(index).boolValue);
            {
                GUILayout.FlexibleSpace();
                EditorGUILayout.LabelField("过渡时间", GUILayout.Width(60));
                m_ProcedureTransitionEnterDurations.GetArrayElementAtIndex(index).floatValue =
                    EditorGUILayout.FloatField(m_ProcedureTransitionEnterDurations.GetArrayElementAtIndex(index).floatValue, GUILayout.Width(60));
                
                GUILayout.FlexibleSpace();
                m_ProcedureTransitionEnterBlockRaycasts.GetArrayElementAtIndex(index).boolValue =
                    EditorGUILayout.ToggleLeft("阻塞射线", 
                        m_ProcedureTransitionEnterBlockRaycasts.GetArrayElementAtIndex(index).boolValue, GUILayout.Width(100));
            }
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndHorizontal();

            // 退出过渡
            EditorGUILayout.BeginHorizontal("box", GUILayout.Width(400));
            m_ProcedureTransitionExitFlags.GetArrayElementAtIndex(index).boolValue =
                EditorGUILayout.ToggleLeft("退出式-切换过渡", 
                    m_ProcedureTransitionExitFlags.GetArrayElementAtIndex(index).boolValue, GUILayout.Width(120));
            
            EditorGUI.BeginDisabledGroup(!m_ProcedureTransitionExitFlags.GetArrayElementAtIndex(index).boolValue);
            {
                GUILayout.FlexibleSpace();
                EditorGUILayout.LabelField("过渡时间", GUILayout.Width(60));
                m_ProcedureTransitionExitDurations.GetArrayElementAtIndex(index).floatValue =
                    EditorGUILayout.FloatField(m_ProcedureTransitionExitDurations.GetArrayElementAtIndex(index).floatValue, GUILayout.Width(60));
                
                GUILayout.FlexibleSpace();
                m_ProcedureTransitionExitBlockRaycasts.GetArrayElementAtIndex(index).boolValue =
                    EditorGUILayout.ToggleLeft("阻塞射线", 
                        m_ProcedureTransitionExitBlockRaycasts.GetArrayElementAtIndex(index).boolValue, GUILayout.Width(100));
            }
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// 绘制预加载流程过渡配置
        /// </summary>
        private void DrawPreloadProcedureTransition(int index)
        {
            // 进入（非热更）
            EditorGUILayout.BeginHorizontal("box", GUILayout.Width(500));
            m_ProcedureTransitionEnterFlags.GetArrayElementAtIndex(index).boolValue =
                EditorGUILayout.ToggleLeft("进入式-切换过渡（前次流程非热更新）", 
                    m_ProcedureTransitionEnterFlags.GetArrayElementAtIndex(index).boolValue, GUILayout.Width(350));
            
            EditorGUI.BeginDisabledGroup(!m_ProcedureTransitionEnterFlags.GetArrayElementAtIndex(index).boolValue);
            {
                GUILayout.FlexibleSpace();
                EditorGUILayout.LabelField("时间", GUILayout.Width(60));
                m_ProcedureTransitionEnterDurations.GetArrayElementAtIndex(index).floatValue =
                    EditorGUILayout.FloatField(m_ProcedureTransitionEnterDurations.GetArrayElementAtIndex(index).floatValue, GUILayout.Width(60));
                m_ProcedureTransitionEnterBlockRaycasts.GetArrayElementAtIndex(index).boolValue =
                    EditorGUILayout.ToggleLeft("阻塞射线", 
                        m_ProcedureTransitionEnterBlockRaycasts.GetArrayElementAtIndex(index).boolValue, GUILayout.Width(100));
            }
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndHorizontal();

            // 退出（非热更）
            EditorGUILayout.BeginHorizontal("box", GUILayout.Width(500));
            m_ProcedureTransitionExitFlags.GetArrayElementAtIndex(index).boolValue =
                EditorGUILayout.ToggleLeft("退出式-切换过渡（前次流程非热更新）", 
                    m_ProcedureTransitionExitFlags.GetArrayElementAtIndex(index).boolValue, GUILayout.Width(350));
            
            EditorGUI.BeginDisabledGroup(!m_ProcedureTransitionExitFlags.GetArrayElementAtIndex(index).boolValue);
            {
                GUILayout.FlexibleSpace();
                EditorGUILayout.LabelField("时间", GUILayout.Width(60));
                m_ProcedureTransitionExitDurations.GetArrayElementAtIndex(index).floatValue =
                    EditorGUILayout.FloatField(m_ProcedureTransitionExitDurations.GetArrayElementAtIndex(index).floatValue, GUILayout.Width(60));
                m_ProcedureTransitionExitBlockRaycasts.GetArrayElementAtIndex(index).boolValue =
                    EditorGUILayout.ToggleLeft("阻塞射线", 
                        m_ProcedureTransitionExitBlockRaycasts.GetArrayElementAtIndex(index).boolValue, GUILayout.Width(100));
            }
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndHorizontal();

            // 进入（热更）
            EditorGUILayout.BeginHorizontal("box", GUILayout.Width(500));
            m_ProcedureTransitionEnterFlagFromProcedureHotfix.boolValue =
                EditorGUILayout.ToggleLeft("进入式-切换过渡（前次流程为热更新）", 
                    m_ProcedureTransitionEnterFlagFromProcedureHotfix.boolValue, GUILayout.Width(350));
            
            EditorGUI.BeginDisabledGroup(!m_ProcedureTransitionEnterFlagFromProcedureHotfix.boolValue);
            {
                GUILayout.FlexibleSpace();
                EditorGUILayout.LabelField("时间", GUILayout.Width(60));
                m_ProcedureTransitionEnterDurationFromProcedureHotfix.floatValue =
                    EditorGUILayout.FloatField(m_ProcedureTransitionEnterDurationFromProcedureHotfix.floatValue, GUILayout.Width(60));
                m_ProcedureTransitionEnterBlockRaycastFromProcedureHotfix.boolValue =
                    EditorGUILayout.ToggleLeft("阻塞射线", 
                        m_ProcedureTransitionEnterBlockRaycastFromProcedureHotfix.boolValue, GUILayout.Width(100));
            }
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndHorizontal();

            // 退出（热更）
            EditorGUILayout.BeginHorizontal("box", GUILayout.Width(500));
            m_ProcedureTransitionExitFlagFromProcedureHotfix.boolValue =
                EditorGUILayout.ToggleLeft("退出式-切换过渡（前次流程为热更新）", 
                    m_ProcedureTransitionExitFlagFromProcedureHotfix.boolValue, GUILayout.Width(350));
            
            EditorGUI.BeginDisabledGroup(!m_ProcedureTransitionExitFlagFromProcedureHotfix.boolValue);
            {
                GUILayout.FlexibleSpace();
                EditorGUILayout.LabelField("时间", GUILayout.Width(60));
                m_ProcedureTransitionExitDurationFromProcedureHotfix.floatValue =
                    EditorGUILayout.FloatField(m_ProcedureTransitionExitDurationFromProcedureHotfix.floatValue, GUILayout.Width(60));
                m_ProcedureTransitionExitBlockRaycastFromProcedureHotfix.boolValue =
                    EditorGUILayout.ToggleLeft("阻塞射线", 
                        m_ProcedureTransitionExitBlockRaycastFromProcedureHotfix.boolValue, GUILayout.Width(100));
            }
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// 绘制生成 Lua 脚本按钮
        /// </summary>
        private void DrawGenerateLuaButton(string luaName)
        {
            if (GUILayout.Button("生成/刷新Lua脚本"))
            {
                string luaRoot = $"{Application.dataPath.Substring(0, Application.dataPath.Length - 6)}{GamePathUtils.LuaScript.Game.GetRootDirectoryRelativePath(true)}";
                
                if (!System.IO.Directory.Exists(luaRoot))
                {
                    System.IO.Directory.CreateDirectory(luaRoot);
                    AssetDatabase.Refresh();
                }

                string[] files = System.IO.Directory.GetFiles(luaRoot, $"{luaName}.lua.txt", System.IO.SearchOption.AllDirectories);

                if (files.Length == 0)
                {
                    string path = EditorUtility.SaveFilePanel($"生成 {luaName}.lua.txt", luaRoot, $"{luaName}.lua.txt", string.Empty);
                    if (!string.IsNullOrEmpty(path))
                    {
                        try
                        {
                            string content = $"{GenerateCommentLines(luaName)}{GenerateEmptyCodeLines(luaName)}";
                            System.IO.File.WriteAllText(path, content, new UTF8Encoding(false));
                            Log.Debug($"生成 {path} 成功");
                            AssetDatabase.Refresh();
                        }
                        catch (Exception e)
                        {
                            Log.Error($"生成失败：{e}");
                        }
                    }
                }
                else if (files.Length == 1)
                {
                    try
                    {
                        string content = $"{GenerateCommentLines(luaName)}{GenerateCodeLines(luaName, files[0])}";
                        System.IO.File.WriteAllText(files[0], content, new UTF8Encoding(false));
                        Log.Debug($"刷新 {files[0]} 成功");
                        AssetDatabase.Refresh();
                    }
                    catch (Exception e)
                    {
                        Log.Error($"刷新失败：{e}");
                    }
                }
                else
                {
                    Log.Error($"找到多个 {luaName}.lua.txt 文件！");
                }
                GUIUtility.ExitGUI();
            }
        }

        /// <summary>
        /// 绘制入口流程选择器
        /// </summary>
        private void DrawEntryProcedureSelector()
        {
            EditorGUILayout.Separator();
            List<string> procedureNames = new List<string>();
            for (int i = 0; i < m_ProcedureTypeNames.arraySize; i++)
                procedureNames.Add(m_ProcedureTypeNames.GetArrayElementAtIndex(i).stringValue);

            int sel = EditorGUILayout.Popup("入口流程", m_EntryProcedureIndex, procedureNames.ToArray());
            if (sel != m_EntryProcedureIndex)
            {
                m_EntryProcedureIndex = sel;
                m_EntryProcedureTypeName.stringValue = procedureNames[sel];
            }
            EditorGUILayout.HelpBox("请以 ProcedureLaunch 为入口流程，除非明确了解流程！", MessageType.Info);
        }

        /// <summary>
        /// 绘制 UI 配置
        /// </summary>
        private void DrawUIConfig()
        {
            // 过渡界面
            EditorGUILayout.BeginVertical("box");
            m_UITransitionABPath.stringValue = EditorGUILayout.TextField("切换过渡界面路径", m_UITransitionABPath.stringValue);
            m_UITransitionAssetName.stringValue = EditorGUILayout.TextField("切换过渡界面资源名称", m_UITransitionAssetName.stringValue);
            if (GUILayout.Button("使用默认切换过渡界面"))
            {
                m_UITransitionABPath.stringValue = UI_AB_PATH_DEFAULT;
                m_UITransitionAssetName.stringValue = UI_TRANSITION_DEFAULT;
                serializedObject.ApplyModifiedProperties();
                GUIUtility.ExitGUI();
            }
            EditorGUILayout.EndVertical();

            // 闪屏界面
            EditorGUILayout.BeginVertical("box");
            m_UISplashABPath.stringValue = EditorGUILayout.TextField("闪屏界面路径", m_UISplashABPath.stringValue);
            m_UISplashAssetName.stringValue = EditorGUILayout.TextField("闪屏界面资源名称", m_UISplashAssetName.stringValue);
            m_SplashProcedureDuration.intValue = EditorGUILayout.IntField("闪屏流程持续时间", m_SplashProcedureDuration.intValue);
            if (GUILayout.Button("使用默认闪屏界面"))
            {
                m_UISplashABPath.stringValue = UI_AB_PATH_DEFAULT;
                m_UISplashAssetName.stringValue = UI_SPLASH_DEFAULT;
                serializedObject.ApplyModifiedProperties();
                GUIUtility.ExitGUI();
            }
            EditorGUILayout.EndVertical();

            // 预加载界面
            EditorGUILayout.BeginVertical("box");
            m_UseUIPreload.boolValue = EditorGUILayout.ToggleLeft("使用预加载界面", m_UseUIPreload.boolValue);
            EditorGUI.BeginDisabledGroup(!m_UseUIPreload.boolValue);
            {
                m_UIPreloadABPath.stringValue = EditorGUILayout.TextField("预加载界面路径", m_UIPreloadABPath.stringValue);
                m_UIPreloadAssetName.stringValue = EditorGUILayout.TextField("预加载界面资源名称", m_UIPreloadAssetName.stringValue);
                if (GUILayout.Button("使用默认预加载界面"))
                {
                    m_UIPreloadABPath.stringValue = UI_AB_PATH_DEFAULT;
                    m_UIPreloadAssetName.stringValue = UI_PRELOAD_DEFAULT;
                    serializedObject.ApplyModifiedProperties();
                    GUIUtility.ExitGUI();
                }
            }
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndVertical();
        }

        /// <summary>
        /// 绘制运行时流程记录
        /// </summary>
        private void DrawRuntimeProcedureRecorder(ProcedureComponent targetComp)
        {
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.HelpBox("运行时流程跳转示意图", MessageType.Info);

            for (int i = 0; i < targetComp.RuntimeProcedureRecordInfos.Count; i++)
            {
                EditorGUILayout.LabelField(targetComp.RuntimeProcedureRecordInfos[i]);
                EditorGUILayout.LabelField("↓");
                if (i == targetComp.RuntimeProcedureRecordInfos.Count - 1)
                {
                    EditorGUILayout.LabelField($"{targetComp.CurrentProcedure.GetType()}\t（{targetComp.CurrentProcedureTime:N2}秒）");
                }
            }

            if (GUILayout.Button("导出 CSV 数据"))
            {
                string file = EditorUtility.SaveFilePanel("导出 CSV", "", $"流程记录 {DateTime.Now:yyyy-MM-dd HH-mm-ss}.csv", "");
                if (!string.IsNullOrEmpty(file))
                {
                    try
                    {
                        List<string> lines = new List<string> { "Procedure信息（从上到下）" };
                        lines.AddRange(targetComp.RuntimeProcedureRecordInfos);
                        lines.Add($"{targetComp.CurrentProcedure.GetType()}\t（{targetComp.CurrentProcedureTime:N2}秒）");
                        System.IO.File.WriteAllLines(file, lines, new UTF8Encoding(false));
                        Log.Debug($"导出成功：{file}");
                    }
                    catch (Exception e)
                    {
                        Log.Error($"导出失败：{e}");
                    }
                }
                GUIUtility.ExitGUI();
            }
            EditorGUILayout.EndVertical();
        }

        protected override void OnCompileComplete()
        {
            base.OnCompileComplete();
        }

        /// <summary>
        /// 生成 Lua 文件头注释
        /// </summary>
        private StringBuilder GenerateCommentLines(string luaName)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("--=====================================================================================================")
              .AppendLine("-- (c) copyright 2026 - 2030, Honor.Game")
              .AppendLine("-- All Rights Reserved.")
              .AppendLine("-- ----------------------------------------------------------------------------------------------------")
              .AppendLine($"-- filename:  {luaName}.lua")
              .AppendLine("--=====================================================================================================")
              .AppendLine();
            return sb;
        }

        /// <summary>
        /// 生成空的 Lua 流程模板
        /// </summary>
        private StringBuilder GenerateEmptyCodeLines(string luaName)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"---@class {luaName} : ProcedureSuper")
              .AppendLine($"local {luaName} = class('{luaName}', import('ProcedureSuper'))")
              .AppendLine()
              .AppendLine("---构造函数")
              .AppendLine("---@type fun(args:table):void")
              .AppendLine("---@param args table @自定义参数")
              .AppendLine($"function {luaName}:ctor(args)")
              .AppendLine($"    {luaName}.super.ctor(self, args)")
              .AppendLine()
              .AppendLine("end")
              .AppendLine()
              .AppendLine("---创建函数")
              .AppendLine($"---@type fun(args:table):{luaName}")
              .AppendLine("---@param args table @自定义参数")
              .AppendLine($"---@return {luaName} @流程实例")
              .AppendLine($"function {luaName}:Create(args)")
              .AppendLine($"    local obj = {luaName}.new(args)")
              .AppendLine("    return obj")
              .AppendLine("end")
              .AppendLine()
              .AppendLine("---进入")
              .AppendLine("---@type fun(ownerMachine: userdata):void")
              .AppendLine("---@param ownerMachine userdata @流程状态")
              .AppendLine($"function {luaName}:OnEnter(ownerMachine)")
              .AppendLine($"    {luaName}.super.OnEnter(self, ownerMachine)")
              .AppendLine()
              .AppendLine("end")
              .AppendLine()
              .AppendLine("---心跳")
              .AppendLine("---@type fun(ownerMachine: userdata):void")
              .AppendLine("---@param ownerMachine userdata @流程状态")
              .AppendLine($"function {luaName}:OnUpdate(ownerMachine)")
              .AppendLine($"    {luaName}.super.OnUpdate(self, ownerMachine)")
              .AppendLine()
              .AppendLine("end")
              .AppendLine()
              .AppendLine("---离开")
              .AppendLine("---@type fun(ownerMachine: userdata):void")
              .AppendLine("---@param ownerMachine userdata @流程状态")
              .AppendLine($"function {luaName}:OnLeave(ownerMachine)")
              .AppendLine($"    {luaName}.super.OnLeave(self, ownerMachine)")
              .AppendLine()
              .AppendLine("end")
              .AppendLine()
              .AppendLine($"return {luaName}")
              .AppendLine();
            return sb;
        }

        /// <summary>
        /// 生成带原有逻辑的 Lua 代码
        /// </summary>
        private StringBuilder GenerateCodeLines(string luaName, string fullPath)
        {
            string content = System.IO.File.ReadAllText(fullPath);
            string endMark = "--=====================================================================================================";
            int endIndex = content.LastIndexOf(endMark) + endMark.Length + 4;
            if (endIndex > 0) content = content.Remove(0, endIndex);

            // 自动补全缺失函数
            List<string> functions = new List<string>
            {
                $"function {luaName}:ctor(args)",
                $"function {luaName}:Create(args)",
                $"function {luaName}:OnEnter(ownerMachine)",
                $"function {luaName}:OnUpdate(ownerMachine)",
                $"function {luaName}:OnLeave(ownerMachine)"
            };

            foreach (var func in functions)
            {
                if (!content.Contains(func))
                {
                    content += func + "\r\n\r\nend\r\n\r\n";
                }
            }

            // 确保 return 语句正确
            string returnStr = $"return {luaName}\r\n";
            if (content.Contains(returnStr))
                content = content.Remove(content.LastIndexOf(returnStr), returnStr.Length);
            content += returnStr;

            return new StringBuilder(content);
        }
    }
}