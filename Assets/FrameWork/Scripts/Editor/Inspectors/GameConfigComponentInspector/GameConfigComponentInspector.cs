using System.Collections.Generic;
using System.IO;
using System.Text;
using GameLib;
using Honor.Runtime;
using UnityEditor;
using ProcedureState = Honor.Runtime.ProcedureState;

namespace Honor.Editor
{
    [CustomEditor(typeof(GameConfig))]
    internal sealed partial class GameConfigComponentInspector : HonorComponentInspector
    {
        private static readonly float[] GameSpeed = new float[] { 0f, 0.01f, 0.1f, 0.25f, 0.5f, 1f, 1.5f, 2f, 4f, 8f };

        private static readonly string[] GameSpeedTexts = new string[]
            { "0x", "0.01x", "0.1x", "0.25x", "0.5x", "1x", "1.5x", "2x", "4x", "8x" };

        private string m_UIABPathDefault = GamePathUtils.Prefab.GetFrameworkRootDirectoryRelativePath();
        private string m_UISplashAssetNameDefault = "UISplashDefault";
        private string m_UITransitionAssetNameDefault = "UITransitionDefault";
        private string m_UIPreloadAssetNameDefault = "UIPreloadDefault";

        /// <summary>
        /// 是否为本地服务器 
        /// </summary>
        private SerializedProperty m_IsLocalServer = null;

        /// <summary>
        /// 启动入口流程名称
        /// </summary>
        private SerializedProperty m_EntryProcedureTypeName = null;

        /// <summary>
        /// 是否启用屏幕方向变动检测
        /// </summary>
        private SerializedProperty m_CheckOrientationState = null;

        /// <summary>
        /// 屏幕方向
        /// </summary>
        private SerializedProperty m_ScreenOrientation = null;

        /// <summary>
        /// 语言类型
        /// </summary>
        private SerializedProperty m_Language = null;

        /// <summary>
        /// 开启字体自动适配
        /// </summary>
        private SerializedProperty m_AutoFontAdapt = null;

        /// <summary>
        /// 编辑器资源模式
        /// 是否以编辑器资源模式运行（仅编辑器内有效），在手机上时会自动校正归为false
        /// </summary>
        private SerializedProperty m_EditorResourceMode = null;

        /// <summary>
        /// 运行时前次语言类型
        /// 启动时默认强制为“简体中文”
        /// </summary>
        private SerializedProperty m_RuntimeLastLanguage = null;

        /// <summary>
        /// 编辑器语言类型
        /// 编辑器运行模式下的语言类型
        /// </summary>
        private SerializedProperty m_EditorLanguage = null;

        /// <summary>
        /// 流程类型名称集合
        /// </summary>
        private SerializedProperty m_ProcedureTypeNames = null;

        private SerializedProperty openDebug = null;

        /// <summary>
        /// 启用后台运行
        /// 游戏挂起时系统后台是否继续运行游戏
        /// </summary>
        private SerializedProperty m_RunInBackground = null;

        //-----------------------------------------------------------------
        /// <summary>
        /// Lua运行时分析模式
        /// 仅Editor中有效
        /// </summary>
        private SerializedProperty m_LuaRuntimeProfilerMode = null;

        /// <summary>
        /// Lua热重载
        /// 是否启用Lua热重载
        /// </summary>
        private SerializedProperty m_LuaHotReloadMode = null;

        /// <summary>
        /// Luac模式
        /// 是否以Luac模式运行
        /// </summary>
        private SerializedProperty m_LuacMode = null;

        /// <summary>
        /// 启用屏幕常亮
        /// 游戏运行一段时间后屏幕是否常亮以避免设备进入锁定状态
        /// </summary>
        private SerializedProperty m_NeverSleep = null;

        /// <summary>
        /// 运行帧率
        /// 游戏运行时每秒的最高帧数
        /// </summary>
        private SerializedProperty m_FrameRate = null;

        /// <summary>
        /// 运行速率
        /// 游戏运行时的最快速率
        /// </summary>
        private SerializedProperty m_GameSpeed = null;
        //-----------------------------------------------------------------

        /// <summary>
        /// 切换过渡界面AB路径
        /// </summary>
        private SerializedProperty m_UITransitionABPath = null;

        /// <summary>
        /// 切换过渡界面Asset资源名称
        /// </summary>
        private SerializedProperty m_UITransitionAssetName = null;


        /// <summary>
        /// 切换过渡界面AB路径
        /// </summary>
        private SerializedProperty m_UISplashABPath = null;

        /// <summary>
        /// 切换过渡界面Asset资源名称
        /// </summary>
        private SerializedProperty m_UISplashAssetName = null;


        /// <summary>
        /// 切换过渡界面AB路径
        /// </summary>
        private SerializedProperty m_UIPreloadABPath = null;

        /// <summary>
        /// 切换过渡界面Asset资源名称
        /// </summary>
        private SerializedProperty m_UIPreloadAssetName = null;

        private SerializedProperty m_SplashProcedureDuration = null;

        private SerializedProperty m_UseUIPreload = null;

        private SerializedProperty m_DevelopMode = null;


        /// <summary>
        /// 
        /// </summary>
        private List<string> procedureTypeNames = new List<string>();

        /// <summary>
        /// 
        /// </summary>
        private int m_EntryProcedureIndex = -1;

        private GameDefinitions.DebugMode m_LuaDebugMode = GameDefinitions.DebugMode.None;

        private void OnEnable()
        {
            // 编辑器资源模式
            // 是否以编辑器资源模式运行（仅编辑器内有效），在手机上时会自动校正归为false
            m_EditorResourceMode = serializedObject.FindProperty("m_EditorResourceMode");
            // 运行时前次语言类型
            // 启动时默认强制为“简体中文”
            m_RuntimeLastLanguage = serializedObject.FindProperty("m_RuntimeLastLanguage");
            // 编辑器语言类型
            // 编辑器运行模式下的语言类型
            m_EditorLanguage = serializedObject.FindProperty("m_EditorLanguage");
            // 开启字体自动适配
            m_AutoFontAdapt = serializedObject.FindProperty("m_AutoFontAdapt");
            // 语言类型
            m_Language = serializedObject.FindProperty("m_Language");
            // 屏幕方向
            m_ScreenOrientation = serializedObject.FindProperty("m_ScreenOrientation");
            // 是否启用屏幕方向变动检测
            m_CheckOrientationState = serializedObject.FindProperty("m_CheckOrientationState");
            // 是否为本地服务器
            m_IsLocalServer = serializedObject.FindProperty("m_IsLocalServer");
            // 流程类型名称集合
            m_ProcedureTypeNames = serializedObject.FindProperty("m_ProcedureTypeNames");
            // 启动入口流程名称
            m_EntryProcedureTypeName = serializedObject.FindProperty("m_EntryProcedureTypeName");

            openDebug = serializedObject.FindProperty("openDebug");
            // 启用后台运行游戏挂起时系统后台是否继续运行游戏
            m_RunInBackground = serializedObject.FindProperty("m_RunInBackground");
            //-----------------------------------------------------------------
            // Lua运行时分析模式
            // 仅Editor中有效
            m_LuaRuntimeProfilerMode = serializedObject.FindProperty("m_LuaRuntimeProfilerMode");
            // Lua热重载
            // 是否启用Lua热重载
            m_LuaHotReloadMode = serializedObject.FindProperty("m_LuaHotReloadMode");
            // Luac模式
            // 是否以Luac模式运行
            m_LuacMode = serializedObject.FindProperty("m_LuacMode");
            // 启用屏幕常亮
            // 游戏运行一段时间后屏幕是否常亮以避免设备进入锁定状态
            m_NeverSleep = serializedObject.FindProperty("m_NeverSleep");
            // 运行帧率
            // 游戏运行时每秒的最高帧数
            m_FrameRate = serializedObject.FindProperty("m_FrameRate");
            // 运行速率
            // 游戏运行时的最快速率
            m_GameSpeed = serializedObject.FindProperty("m_GameSpeed");
            m_LuaDebugMode = GameDefinitions.DebugMode.None;
            //-----------------------------------------------------------------
            // 切换过渡界面AB路径
            m_UITransitionABPath = serializedObject.FindProperty("m_UITransitionABPath");
            // 切换过渡界面Asset资源名称
            m_UITransitionAssetName = serializedObject.FindProperty("m_UITransitionAssetName");
            // 切换过渡界面AB路径
            m_UISplashABPath = serializedObject.FindProperty("m_UISplashABPath");
            // 切换过渡界面Asset资源名称
            m_UISplashAssetName = serializedObject.FindProperty("m_UISplashAssetName");
            // 切换过渡界面AB路径
            m_UIPreloadABPath = serializedObject.FindProperty("m_UIPreloadABPath");
            // 切换过渡界面Asset资源名称
            m_UIPreloadAssetName = serializedObject.FindProperty("m_UIPreloadAssetName");

            m_SplashProcedureDuration = serializedObject.FindProperty("m_SplashProcedureDuration");
            m_UseUIPreload = serializedObject.FindProperty("m_UseUIPreload");
            m_DevelopMode = serializedObject.FindProperty("m_DevelopMode");
            // 获取流程状态名称集合
            string[] procedureNames = AorType.GetTypeNames(typeof(ProcedureState));
            // 初始化序列化队列数据
            m_ProcedureTypeNames.ClearArray();
            for (int index = 0; index < procedureNames.Length; index++)
            {
                m_ProcedureTypeNames.InsertArrayElementAtIndex(index);
                m_ProcedureTypeNames.GetArrayElementAtIndex(index).stringValue = procedureNames[index];
                string procedureName = m_ProcedureTypeNames.GetArrayElementAtIndex(index).stringValue;
                procedureTypeNames.Add(procedureName);
                // 判定已经选中的入口流程
                if (m_EntryProcedureTypeName.stringValue == procedureNames[index])
                {
                    m_EntryProcedureIndex = index;
                }
            }

            serializedObject.ApplyModifiedProperties();
        }


        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            // 更新序列化对象，以便反映任何变化
            serializedObject.Update();
            GameConfig t = (GameConfig)target;
            if (EditorApplication.isPlaying)
            {
                EditorGUILayout.LabelField("当前流程",
                    t.CurrentProcedure == null ? "None" : t.CurrentProcedure.GetType().ToString());
            }

            #region 是否为本地服务器

            OnEntryProcedureIndex();
            OnEditorResourceMode();
            OnEditorLanguage();

            #endregion

            // 应用更改
            serializedObject.ApplyModifiedProperties();
            Repaint();
        }

        /// <summary>
        /// 生成注释行信息
        /// </summary>
        private StringBuilder GenerateCommentLines(string luaScriptName)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder
                .AppendLine(
                    "--=====================================================================================================")
                .AppendLine("-- (c) copyright 2025 - 2026, Honor.Game")
                .AppendLine("-- All Rights Reserved.")
                .AppendLine(
                    "-- ----------------------------------------------------------------------------------------------------")
                .AppendLine(AorTxt.Format("-- filename:  {0}.lua.txt", luaScriptName))
                .AppendLine(
                    "--=====================================================================================================")
                .AppendLine("");
            return stringBuilder;
        }

        /// <summary>
        /// 写入LuaDebugMode信息
        /// </summary>
        private void WriteDebugModeToLibraryConfigFile()
        {
            string dataPath = GamePathUtils.IDEDebugger.GetLuaDebugModeLibraryConfigFileFullPath();
            File.WriteAllText(dataPath, ((int)m_LuaDebugMode).ToString());
        }
    }
}