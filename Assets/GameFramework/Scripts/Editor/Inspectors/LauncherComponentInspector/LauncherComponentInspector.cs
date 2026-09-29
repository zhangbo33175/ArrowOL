/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Editor
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  LauncherComponentInspector.cs
 * author:    云毅
 * created:   2026
 * descrip:   启动器组件编辑器
 *            项目核心控制面板：运行模式、热更新、Lua调试、帧率、倍速、设备分级
 ***************************************************************/

using System;
using System.IO;
using Honor.Runtime;
using UnityEditor;
using UnityEngine;

namespace Honor.Editor
{
    /// <summary>
    /// 【启动器组件编辑器】
    /// 功能：项目核心控制面板，管理运行模式、热更新、Lua调试、帧率、倍速、设备分级
    /// 作用：编辑器下一键切换开发/生产环境、调试、性能配置
    /// </summary>
    [CustomEditor(typeof(LauncherComponent))]
    public class LauncherComponentInspector : HonorComponentInspector
    {
        #region 常量配置
        /// <summary>游戏倍速可选值表</summary>
        private static readonly float[] s_GameSpeedValues = { 0f, 0.01f, 0.1f, 0.25f, 0.5f, 1f, 1.5f, 2f, 4f, 8f };

        /// <summary>游戏倍速显示文本表（与 s_GameSpeedValues 按下标一一对应）</summary>
        private static readonly string[] s_GameSpeedLabels = { "0x", "0.01x", "0.1x", "0.25x", "0.5x", "1x", "1.5x", "2x", "4x", "8x" };

        /// <summary>三平台显示名称</summary>
        private static readonly string[] s_PlatformNames = { "Editor", "Android", "iOS" };
        #endregion

        #region 序列化字段
        /// <summary>是否启用编辑器资源模式（否则设备强制走 AB 模式）</summary>
        private SerializedProperty m_EditorResourceMode;

        /// <summary>编辑器启动语言</summary>
        private SerializedProperty m_EditorLanguage;

        /// <summary>是否开发模式（控制开发/生产环境配置）</summary>
        private SerializedProperty m_DevelopMode;

        /// <summary>是否连接本地测试服务器</summary>
        private SerializedProperty m_IsLocalServer;

        /// <summary>是否启用编辑器下实时调试热更新</summary>
        private SerializedProperty m_IsRealTimeDebuggerHotfixForEditor;

        /// <summary>是否启用 Luac 字节码模式（启用后不可调试）</summary>
        private SerializedProperty m_LuacMode;

        /// <summary>是否启用 Lua 热重载（免重启运行 Lua 改动）</summary>
        private SerializedProperty m_LuaHotReloadMode;

        /// <summary>调试器是否激活窗口</summary>
        private SerializedProperty m_DebuggerActiveWindow;

        /// <summary>运行帧率</summary>
        private SerializedProperty m_FrameRate;

        /// <summary>运行倍速</summary>
        private SerializedProperty m_GameSpeed;

        /// <summary>是否后台运行</summary>
        private SerializedProperty m_RunInBackground;

        /// <summary>是否屏幕常亮（不休眠）</summary>
        private SerializedProperty m_NeverSleep;

        /// <summary>当前 Lua 调试模式（运行时内存值，非序列化）</summary>
        private GameDefinitions.DebugMode m_LuaDebugMode;

        /// <summary>是否自定义硬件性能分级</summary>
        private SerializedProperty m_CustomDevicePerformance;

        /// <summary>是否启用硬件性能分级/评级</summary>
        private SerializedProperty m_UseDevicePerformance;

        /// <summary>三平台（Editor/Android/iOS）性能配置属性数组</summary>
        private readonly SerializedProperty[] m_Performances = new SerializedProperty[3];

        /// <summary>三平台 CPU 核心数属性数组</summary>
        private readonly SerializedProperty[] m_ProcessorCounts = new SerializedProperty[3];

        /// <summary>三平台高端机显存基准属性数组</summary>
        private readonly SerializedProperty[] m_GraphicsMemorySizeHighBases = new SerializedProperty[3];

        /// <summary>三平台中端机显存基准属性数组</summary>
        private readonly SerializedProperty[] m_GraphicsMemorySizeMidBases = new SerializedProperty[3];

        /// <summary>三平台高端机内存基准属性数组</summary>
        private readonly SerializedProperty[] m_SystemMemorySizeHighBases = new SerializedProperty[3];

        /// <summary>三平台中端机内存基准属性数组</summary>
        private readonly SerializedProperty[] m_SystemMemorySizeMidBases = new SerializedProperty[3];
        #endregion

        #region 生命周期
        /// <summary>
        /// 启用时绑定所有序列化属性
        /// </summary>
        private void OnEnable()
        {
            m_EditorResourceMode               = serializedObject.FindProperty("m_EditorResourceMode");
            m_EditorLanguage                   = serializedObject.FindProperty("m_EditorLanguage");
            m_DevelopMode                      = serializedObject.FindProperty("m_DevelopMode");
            m_IsLocalServer                    = serializedObject.FindProperty("m_IsLocalServer");
            m_IsRealTimeDebuggerHotfixForEditor = serializedObject.FindProperty("m_IsRealTimeDebuggerHotfixForEditor");
            m_LuacMode                         = serializedObject.FindProperty("m_LuacMode");
            m_LuaHotReloadMode                 = serializedObject.FindProperty("m_LuaHotReloadMode");
            m_DebuggerActiveWindow             = serializedObject.FindProperty("m_DebuggerActiveWindow");
            m_FrameRate                        = serializedObject.FindProperty("m_FrameRate");
            m_GameSpeed                        = serializedObject.FindProperty("m_GameSpeed");
            m_RunInBackground                  = serializedObject.FindProperty("m_RunInBackground");
            m_NeverSleep                       = serializedObject.FindProperty("m_NeverSleep");

            m_LuaDebugMode = GameDefinitions.DebugMode.None;
            ReadLuaDebugModeFromLibraryConfigFile();

            m_CustomDevicePerformance = serializedObject.FindProperty("m_CustomDevicePerformance");
            m_UseDevicePerformance    = serializedObject.FindProperty("m_UseDevicePerformance");

            m_Performances[0] = serializedObject.FindProperty("m_EditorPerformance");
            m_Performances[1] = serializedObject.FindProperty("m_AndroidPerformance");
            m_Performances[2] = serializedObject.FindProperty("m_iOSPerformance");

            for (int platformIndex = 0; platformIndex < m_Performances.Length; platformIndex++)
            {
                m_ProcessorCounts[platformIndex]            = m_Performances[platformIndex].FindPropertyRelative("ProcessorCount");
                m_GraphicsMemorySizeHighBases[platformIndex] = m_Performances[platformIndex].FindPropertyRelative("GraphicsMemorySizeHighBase");
                m_GraphicsMemorySizeMidBases[platformIndex] = m_Performances[platformIndex].FindPropertyRelative("GraphicsMemorySizeMidBase");
                m_SystemMemorySizeHighBases[platformIndex]  = m_Performances[platformIndex].FindPropertyRelative("SystemMemorySizeHighBase");
                m_SystemMemorySizeMidBases[platformIndex]   = m_Performances[platformIndex].FindPropertyRelative("SystemMemorySizeMidBase");
            }
        }

        /// <summary>
        /// 绘制面板
        /// </summary>
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();

            LauncherComponent targetComponent = (LauncherComponent)target;

            DrawResourceModeSection();
            DrawDevelopmentSection();
            DrawRuntimeParametersSection(targetComponent);
            DrawDevicePerformanceSection();

            serializedObject.ApplyModifiedProperties();
            Repaint();
        }
        #endregion

        #region 资源模式段
        /// <summary>
        /// 绘制编辑器资源模式及其联动的实时调试、启动语言设置
        /// </summary>
        private void DrawResourceModeSection()
        {
            // ====================== 编辑器资源模式 ======================
            m_EditorResourceMode.boolValue = EditorGUILayout.Toggle("编辑器资源模式", m_EditorResourceMode.boolValue);

            if (!m_EditorResourceMode.boolValue)
                EditorGUI.BeginDisabledGroup(true);

            {
                EditorGUILayout.HelpBox("编辑器资源模式仅在编辑器下有效，设备将强制使用AB模式", MessageType.Warning);

                m_IsRealTimeDebuggerHotfixForEditor.boolValue =
                    EditorGUILayout.Toggle("实时调试热更新", m_IsRealTimeDebuggerHotfixForEditor.boolValue);

                EditorGUILayout.HelpBox("仅编辑器资源模式下生效，不实际下载热更文件", MessageType.Info);

                // 语言设置
                string[] langList = Enum.GetNames(typeof(GameDefinitions.Language));
                if (Application.isPlaying)
                {
                    string langName = ((GameDefinitions.Language)m_EditorLanguage.enumValueIndex).ToString();
                    EditorGUILayout.LabelField("编辑器启动语言", $"{langName}({GameDefinitions.LanguageDesc[m_EditorLanguage.enumValueIndex]})");
                }
                else
                {
                    m_EditorLanguage.enumValueIndex = EditorGUILayout.Popup("编辑器启动语言", m_EditorLanguage.enumValueIndex, langList);
                }
            }

            if (!m_EditorResourceMode.boolValue)
                EditorGUI.EndDisabledGroup();
        }
        #endregion

        #region 开发模式段
        /// <summary>
        /// 绘制开发模式、本地服务器、Lua调试、Luac与热重载开关
        /// </summary>
        private void DrawDevelopmentSection()
        {
            // ====================== 开发模式 ======================
            if (Application.isPlaying)
                EditorGUI.BeginDisabledGroup(true);

            {
                m_DevelopMode.boolValue = EditorGUILayout.Toggle("开发模式", m_DevelopMode.boolValue);
                EditorGUILayout.HelpBox("切换开发/生产环境配置", MessageType.Info);

                if (m_DevelopMode.boolValue)
                {
                    m_IsLocalServer.boolValue = EditorGUILayout.Toggle("本地服务器", m_IsLocalServer.boolValue);
                    EditorGUILayout.HelpBox("使用测试服务器[172.16.35.16:8081]", MessageType.Info);
                }

                DrawLuaDebugModeBlock();

                // Luac 模式
                m_LuacMode.boolValue = EditorGUILayout.Toggle("Luac模式", m_LuacMode.boolValue);
                EditorGUILayout.HelpBox("设备强制使用Luac字节码", MessageType.Info);

                // Lua热重载
                if (m_EditorResourceMode.boolValue && !m_LuacMode.boolValue)
                {
                    m_LuaHotReloadMode.boolValue = EditorGUILayout.Toggle("Lua热重载模式", m_LuaHotReloadMode.boolValue);
                    EditorGUILayout.HelpBox("无需重启运行Lua代码修改", MessageType.Info);
                }
                else
                {
                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUILayout.Toggle("Lua热重载模式", false);
                    EditorGUILayout.HelpBox("当前禁用", MessageType.Info);
                    EditorGUI.EndDisabledGroup();
                }
            }

            if (Application.isPlaying)
                EditorGUI.EndDisabledGroup();
        }

        /// <summary>
        /// 绘制 Lua 调试模式选择；Luac 开启时强制禁用并复位调试模式
        /// </summary>
        private void DrawLuaDebugModeBlock()
        {
            // Luac模式下不可调试，强制复位为 None
            if (m_LuacMode.boolValue)
            {
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.EnumPopup("Lua调试", GameDefinitions.DebugMode.None);
                EditorGUILayout.HelpBox("Luac模式下不可使用调试！", MessageType.Info);
                EditorGUI.EndDisabledGroup();
                m_LuaDebugMode = GameDefinitions.DebugMode.None;
                return;
            }

            GameDefinitions.DebugMode newDebugMode = (GameDefinitions.DebugMode)EditorGUILayout.EnumPopup("Lua调试", m_LuaDebugMode);
            EditorGUILayout.HelpBox("None=关闭 | IDE First=调试器优先 | Unity First=Unity优先", MessageType.Info);

            if (newDebugMode == m_LuaDebugMode)
            {
                return;
            }

            m_LuaDebugMode = newDebugMode;
            WriteDebugModeToLibraryConfigFile();
        }
        #endregion

        #region 运行时参数段
        /// <summary>
        /// 绘制帧率、倍速、后台运行、屏幕常亮等运行时参数
        /// </summary>
        /// <param name="targetComponent">当前选中的启动器组件实例</param>
        private void DrawRuntimeParametersSection(LauncherComponent targetComponent)
        {
            // ====================== 运行时参数 ======================
            // 帧率
            int frameRate = EditorGUILayout.IntSlider("运行帧率", m_FrameRate.intValue, 1, 120);
            ApplyScalarWhenPlayingOrSave(frameRate, m_FrameRate, value => targetComponent.FrameRate = value);

            // 游戏倍速
            EditorGUILayout.BeginVertical("box");
            {
                float gameSpeed = EditorGUILayout.Slider("运行速率", m_GameSpeed.floatValue, 0f, 8f);
                int speedIndex = GUILayout.SelectionGrid(GetSelectedGameSpeed(gameSpeed), s_GameSpeedLabels, 5);

                if (speedIndex >= 0)
                    gameSpeed = GetGameSpeed(speedIndex);

                ApplyScalarWhenPlayingOrSave(gameSpeed, m_GameSpeed, value => targetComponent.GameSpeed = value);
            }
            EditorGUILayout.EndVertical();

            // 后台运行
            bool runInBackground = EditorGUILayout.Toggle("启用后台运行", m_RunInBackground.boolValue);
            EditorGUILayout.HelpBox("移动端无效，false可触发挂起/恢复", MessageType.Info);
            ApplyScalarWhenPlayingOrSave(runInBackground, m_RunInBackground, value => targetComponent.RunInBackground = value);

            // 屏幕常亮
            bool neverSleep = EditorGUILayout.Toggle("启用屏幕常亮", m_NeverSleep.boolValue);
            ApplyScalarWhenPlayingOrSave(neverSleep, m_NeverSleep, value => targetComponent.NeverSleep = value);
        }

        /// <summary>
        /// 整型标量同步：有变化时，播放期写运行时实例，编辑期写序列化属性
        /// </summary>
        private void ApplyScalarWhenPlayingOrSave(int newValue, SerializedProperty property, Action<int> applyToRuntime)
        {
            if (newValue == property.intValue)
            {
                return;
            }

            if (EditorApplication.isPlaying)
            {
                applyToRuntime(newValue);
            }
            else
            {
                property.intValue = newValue;
            }
        }

        /// <summary>
        /// 浮点标量同步：有变化时，播放期写运行时实例，编辑期写序列化属性
        /// </summary>
        private void ApplyScalarWhenPlayingOrSave(float newValue, SerializedProperty property, Action<float> applyToRuntime)
        {
            if (newValue == property.floatValue)
            {
                return;
            }

            if (EditorApplication.isPlaying)
            {
                applyToRuntime(newValue);
            }
            else
            {
                property.floatValue = newValue;
            }
        }

        /// <summary>
        /// 布尔标量同步：有变化时，播放期写运行时实例，编辑期写序列化属性
        /// </summary>
        private void ApplyScalarWhenPlayingOrSave(bool newValue, SerializedProperty property, Action<bool> applyToRuntime)
        {
            if (newValue == property.boolValue)
            {
                return;
            }

            if (EditorApplication.isPlaying)
            {
                applyToRuntime(newValue);
            }
            else
            {
                property.boolValue = newValue;
            }
        }
        #endregion

        #region 设备性能分级段
        /// <summary>
        /// 绘制硬件性能分级开关与三平台高/中/低端基准配置
        /// </summary>
        private void DrawDevicePerformanceSection()
        {
            // ====================== 设备性能分级 ======================
            m_UseDevicePerformance.boolValue = EditorGUILayout.Toggle("开启硬件性能分级/评级", m_UseDevicePerformance.boolValue);
            m_CustomDevicePerformance.boolValue = EditorGUILayout.Toggle("自定义硬件性能分级/评级", m_CustomDevicePerformance.boolValue);

            if (!m_CustomDevicePerformance.boolValue)
            {
                return;
            }

            EditorGUILayout.BeginVertical("box");
            {
                // CPU核心数
                EditorGUILayout.Separator();
                for (int platformIndex = 0; platformIndex < m_ProcessorCounts.Length; platformIndex++)
                {
                    string input = EditorGUILayout.TextField($"{s_PlatformNames[platformIndex]}-CPU核心数：", m_ProcessorCounts[platformIndex].intValue.ToString());
                    if (int.TryParse(input, out int parsedCpu) && parsedCpu != m_ProcessorCounts[platformIndex].intValue)
                    {
                        m_ProcessorCounts[platformIndex].intValue = parsedCpu;
                    }
                }

                EditorGUILayout.HelpBox("低于此值 = 低端机", MessageType.Info);

                // 高端机
                EditorGUILayout.Separator();
                EditorGUILayout.LabelField("高端机", GetLevelLabelStyle(DevicePerformanceLevel.High));
                for (int platformIndex = 0; platformIndex < s_PlatformNames.Length; platformIndex++)
                {
                    DrawPlatformBasisRow(platformIndex, m_GraphicsMemorySizeHighBases[platformIndex], m_SystemMemorySizeHighBases[platformIndex]);
                }

                // 中端机
                EditorGUILayout.Separator();
                EditorGUILayout.LabelField("中端机", GetLevelLabelStyle(DevicePerformanceLevel.Mid));
                for (int platformIndex = 0; platformIndex < s_PlatformNames.Length; platformIndex++)
                {
                    DrawPlatformBasisRow(platformIndex, m_GraphicsMemorySizeMidBases[platformIndex], m_SystemMemorySizeMidBases[platformIndex]);
                }

                // 低端机自动判定
                EditorGUILayout.Separator();
                EditorGUILayout.LabelField("低端机", GetLevelLabelStyle(DevicePerformanceLevel.Low));
                for (int platformIndex = 0; platformIndex < s_PlatformNames.Length; platformIndex++)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(s_PlatformNames[platformIndex], GUILayout.Width(80));
                    EditorGUILayout.LabelField($"显存 < {m_GraphicsMemorySizeMidBases[platformIndex].intValue} 或 内存 < {m_SystemMemorySizeMidBases[platformIndex].intValue}");
                    EditorGUILayout.EndHorizontal();
                }
            }
            EditorGUILayout.EndVertical();
        }

        /// <summary>
        /// 绘制单行平台显存/内存基准输入（高端机与中端机共用）
        /// </summary>
        /// <param name="platformIndex">平台下标</param>
        /// <param name="gfxBaseProperty">显存基准序列化属性</param>
        /// <param name="memBaseProperty">内存基准序列化属性</param>
        private void DrawPlatformBasisRow(int platformIndex, SerializedProperty gfxBaseProperty, SerializedProperty memBaseProperty)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(s_PlatformNames[platformIndex], GUILayout.Width(60));
            EditorGUILayout.LabelField("显存>=", GUILayout.Width(60));

            string gfxText = EditorGUILayout.TextField("", gfxBaseProperty.intValue.ToString(), GUILayout.Width(100));
            if (int.TryParse(gfxText, out int parsedGfx))
            {
                gfxBaseProperty.intValue = parsedGfx;
            }

            EditorGUILayout.LabelField("内存>=", GUILayout.Width(60));
            string memText = EditorGUILayout.TextField("", memBaseProperty.intValue.ToString(), GUILayout.Width(100));
            if (int.TryParse(memText, out int parsedMem))
            {
                memBaseProperty.intValue = parsedMem;
            }

            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// 按设备分级颜色生成标题标签样式
        /// </summary>
        /// <param name="level">设备性能分级</param>
        private static GUIStyle GetLevelLabelStyle(DevicePerformanceLevel level)
        {
            return new GUIStyle
            {
                normal = { textColor = DevicePerformance.GetDevicePerformanceLevelColor(level) }
            };
        }
        #endregion

        #region 工具方法
        /// <summary>获取倍速值</summary>
        private float GetGameSpeed(int speedIndex) =>
            speedIndex >= 0 && speedIndex < s_GameSpeedValues.Length ? s_GameSpeedValues[speedIndex] : s_GameSpeedValues[0];

        /// <summary>获取倍速索引</summary>
        private int GetSelectedGameSpeed(float speed) => Array.IndexOf(s_GameSpeedValues, speed);

        /// <summary>从文件读取Lua调试模式</summary>
        private void ReadLuaDebugModeFromLibraryConfigFile()
        {
            string configPath = GamePathUtils.IDEDebugger.GetLuaDebugModeLibraryConfigFileFullPath();
            if (File.Exists(configPath))
            {
                string fileContent = File.ReadAllText(configPath);
                m_LuaDebugMode = (GameDefinitions.DebugMode)int.Parse(fileContent);
            }
            else
            {
                WriteDebugModeToLibraryConfigFile();
            }
        }

        /// <summary>写入Lua调试模式</summary>
        private void WriteDebugModeToLibraryConfigFile()
        {
            string configPath = GamePathUtils.IDEDebugger.GetLuaDebugModeLibraryConfigFileFullPath();
            File.WriteAllText(configPath, ((int)m_LuaDebugMode).ToString());
        }
        #endregion
    }
}
