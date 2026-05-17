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
        // ======================= 常量配置 =======================
        /// <summary>游戏倍速数组</summary>
        private static readonly float[] GameSpeed = new float[] 
            { 0f, 0.01f, 0.1f, 0.25f, 0.5f, 1f, 1.5f, 2f, 4f, 8f };

        /// <summary>游戏倍速显示文本</summary>
        private static readonly string[] GameSpeedTexts = new string[]
            { "0x", "0.01x", "0.1x", "0.25x", "0.5x", "1x", "1.5x", "2x", "4x", "8x" };

        /// <summary>平台名称</summary>
        private static readonly string[] PlatformStrings = new string[] { "Editor", "Android", "iOS" };

        // ======================= 序列化字段 =======================
        private SerializedProperty m_EditorResourceMode;
        private SerializedProperty m_EditorLanguage;
        private SerializedProperty m_DevelopMode;
        private SerializedProperty m_IsLocalServer;
        private SerializedProperty m_IsRealTimeDebuggerHotfixForEditor;
        private SerializedProperty m_LuacMode;
        private SerializedProperty m_LuaHotReloadMode;
        private SerializedProperty m_DebuggerActiveWindow;
        private SerializedProperty m_FrameRate;
        private SerializedProperty m_GameSpeed;
        private SerializedProperty m_RunInBackground;
        private SerializedProperty m_NeverSleep;
        
        private GameDefinitions.DebugMode m_LuaDebugMode;
        
        private SerializedProperty m_CustomDevicePerformance;
        private SerializedProperty m_UseDevicePerformance;

        private readonly SerializedProperty[] m_Performances = new SerializedProperty[3];
        private readonly SerializedProperty[] m_ProcessorCounts = new SerializedProperty[3];
        private readonly SerializedProperty[] m_GraphicsMemorySizeHighBases = new SerializedProperty[3];
        private readonly SerializedProperty[] m_GraphicsMemorySizeMidBases = new SerializedProperty[3];
        private readonly SerializedProperty[] m_SystemMemorySizeHighBases = new SerializedProperty[3];
        private readonly SerializedProperty[] m_SystemMemorySizeMidBases = new SerializedProperty[3];

        // ======================= 生命周期 =======================
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

            for (int i = 0; i < m_Performances.Length; i++)
            {
                m_ProcessorCounts[i]                = m_Performances[i].FindPropertyRelative("ProcessorCount");
                m_GraphicsMemorySizeHighBases[i]    = m_Performances[i].FindPropertyRelative("GraphicsMemorySizeHighBase");
                m_GraphicsMemorySizeMidBases[i]     = m_Performances[i].FindPropertyRelative("GraphicsMemorySizeMidBase");
                m_SystemMemorySizeHighBases[i]     = m_Performances[i].FindPropertyRelative("SystemMemorySizeHighBase");
                m_SystemMemorySizeMidBases[i]      = m_Performances[i].FindPropertyRelative("SystemMemorySizeMidBase");
            }
        }

        /// <summary>
        /// 绘制面板
        /// </summary>
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();

            LauncherComponent t = (LauncherComponent)target;

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

                // Lua调试模式
                if (m_LuacMode.boolValue)
                {
                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUILayout.EnumPopup("Lua调试", GameDefinitions.DebugMode.None);
                    EditorGUILayout.HelpBox("Luac模式下不可使用调试！", MessageType.Info);
                    EditorGUI.EndDisabledGroup();
                    m_LuaDebugMode = GameDefinitions.DebugMode.None;
                }
                else
                {
                    GameDefinitions.DebugMode newDebugMode = (GameDefinitions.DebugMode)EditorGUILayout.EnumPopup("Lua调试", m_LuaDebugMode);
                    EditorGUILayout.HelpBox("None=关闭 | IDE First=调试器优先 | Unity First=Unity优先", MessageType.Info);
                    
                    if (newDebugMode != m_LuaDebugMode)
                    {
                        m_LuaDebugMode = newDebugMode;
                        WriteDebugModeToLibraryConfigFile();
                    }
                }

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

            // ====================== 运行时参数 ======================
            // 帧率
            int frameRate = EditorGUILayout.IntSlider("运行帧率", m_FrameRate.intValue, 1, 120);
            if (frameRate != m_FrameRate.intValue)
            {
                if (EditorApplication.isPlaying) 
                    t.FrameRate = frameRate;
                else 
                    m_FrameRate.intValue = frameRate;
            }

            // 游戏倍速
            EditorGUILayout.BeginVertical("box");
            {
                float gameSpeed = EditorGUILayout.Slider("运行速率", m_GameSpeed.floatValue, 0f, 8f);
                int speedIndex = GUILayout.SelectionGrid(GetSelectedGameSpeed(gameSpeed), GameSpeedTexts, 5);
                
                if (speedIndex >= 0) 
                    gameSpeed = GetGameSpeed(speedIndex);

                if (gameSpeed != m_GameSpeed.floatValue)
                {
                    if (EditorApplication.isPlaying) 
                        t.GameSpeed = gameSpeed;
                    else 
                        m_GameSpeed.floatValue = gameSpeed;
                }
            }
            EditorGUILayout.EndVertical();

            // 后台运行
            bool runInBackground = EditorGUILayout.Toggle("启用后台运行", m_RunInBackground.boolValue);
            EditorGUILayout.HelpBox("移动端无效，false可触发挂起/恢复", MessageType.Info);
            
            if (runInBackground != m_RunInBackground.boolValue)
            {
                if (EditorApplication.isPlaying) 
                    t.RunInBackground = runInBackground;
                else 
                    m_RunInBackground.boolValue = runInBackground;
            }

            // 屏幕常亮
            bool neverSleep = EditorGUILayout.Toggle("启用屏幕常亮", m_NeverSleep.boolValue);
            if (neverSleep != m_NeverSleep.boolValue)
            {
                if (EditorApplication.isPlaying) 
                    t.NeverSleep = neverSleep;
                else 
                    m_NeverSleep.boolValue = neverSleep;
            }

            // ====================== 设备性能分级 ======================
            m_UseDevicePerformance.boolValue = EditorGUILayout.Toggle("开启硬件性能分级/评级", m_UseDevicePerformance.boolValue);
            m_CustomDevicePerformance.boolValue = EditorGUILayout.Toggle("自定义硬件性能分级/评级", m_CustomDevicePerformance.boolValue);

            if (m_CustomDevicePerformance.boolValue)
            {
                EditorGUILayout.BeginVertical("box");
                {
                    // CPU核心数
                    EditorGUILayout.Separator();
                    int newData;
                    
                    for (int i = 0; i < m_ProcessorCounts.Length; i++)
                    {
                        string input = EditorGUILayout.TextField($"{PlatformStrings[i]}-CPU核心数：", m_ProcessorCounts[i].intValue.ToString());
                        if (int.TryParse(input, out newData) && newData != m_ProcessorCounts[i].intValue)
                            m_ProcessorCounts[i].intValue = newData;
                    }

                    EditorGUILayout.HelpBox("低于此值 = 低端机", MessageType.Info);

                    // 高端机
                    EditorGUILayout.Separator();
                    EditorGUILayout.LabelField("高端机", new GUIStyle
                    {
                        normal = { textColor = DevicePerformance.GetDevicePerformanceLevelColor(DevicePerformanceLevel.High) }
                    });
                    
                    for (int i = 0; i < 3; i++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField(PlatformStrings[i], GUILayout.Width(60));
                        EditorGUILayout.LabelField("显存>=", GUILayout.Width(60));
                        
                        string gfxHigh = EditorGUILayout.TextField("", m_GraphicsMemorySizeHighBases[i].intValue.ToString(), GUILayout.Width(100));
                        if (int.TryParse(gfxHigh, out newData)) 
                            m_GraphicsMemorySizeHighBases[i].intValue = newData;

                        EditorGUILayout.LabelField("内存>=", GUILayout.Width(60));
                        string memHigh = EditorGUILayout.TextField("", m_SystemMemorySizeHighBases[i].intValue.ToString(), GUILayout.Width(100));
                        if (int.TryParse(memHigh, out newData)) 
                            m_SystemMemorySizeHighBases[i].intValue = newData;
                        
                        EditorGUILayout.EndHorizontal();
                    }

                    // 中端机
                    EditorGUILayout.Separator();
                    EditorGUILayout.LabelField("中端机", new GUIStyle
                    {
                        normal = { textColor = DevicePerformance.GetDevicePerformanceLevelColor(DevicePerformanceLevel.Mid) }
                    });
                    
                    for (int i = 0; i < 3; i++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField(PlatformStrings[i], GUILayout.Width(60));
                        EditorGUILayout.LabelField("显存>=", GUILayout.Width(60));
                        
                        string gfxMid = EditorGUILayout.TextField("", m_GraphicsMemorySizeMidBases[i].intValue.ToString(), GUILayout.Width(100));
                        if (int.TryParse(gfxMid, out newData)) 
                            m_GraphicsMemorySizeMidBases[i].intValue = newData;

                        EditorGUILayout.LabelField("内存>=", GUILayout.Width(60));
                        string memMid = EditorGUILayout.TextField("", m_SystemMemorySizeMidBases[i].intValue.ToString(), GUILayout.Width(100));
                        if (int.TryParse(memMid, out newData)) 
                            m_SystemMemorySizeMidBases[i].intValue = newData;
                        
                        EditorGUILayout.EndHorizontal();
                    }

                    // 低端机自动判定
                    EditorGUILayout.Separator();
                    EditorGUILayout.LabelField("低端机", new GUIStyle
                    {
                        normal = { textColor = DevicePerformance.GetDevicePerformanceLevelColor(DevicePerformanceLevel.Low) }
                    });
                    
                    for (int i = 0; i < 3; i++)
                    {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField(PlatformStrings[i], GUILayout.Width(80));
                        EditorGUILayout.LabelField($"显存 < {m_GraphicsMemorySizeMidBases[i].intValue} 或 内存 < {m_SystemMemorySizeMidBases[i].intValue}");
                        EditorGUILayout.EndHorizontal();
                    }
                }
                EditorGUILayout.EndVertical();
            }

            serializedObject.ApplyModifiedProperties();
            Repaint();
        }

        // ======================= 工具方法 =======================
        /// <summary>获取倍速值</summary>
        private float GetGameSpeed(int index) =>
            index >= 0 && index < GameSpeed.Length ? GameSpeed[index] : GameSpeed[0];

        /// <summary>获取倍速索引</summary>
        private int GetSelectedGameSpeed(float speed) => Array.IndexOf(GameSpeed, speed);

        /// <summary>从文件读取Lua调试模式</summary>
        private void ReadLuaDebugModeFromLibraryConfigFile()
        {
            string path = GamePathUtils.IDEDebugger.GetLuaDebugModeLibraryConfigFileFullPath();
            if (File.Exists(path))
            {
                string content = File.ReadAllText(path);
                m_LuaDebugMode = (GameDefinitions.DebugMode)int.Parse(content);
            }
            else 
                WriteDebugModeToLibraryConfigFile();
        }

        /// <summary>写入Lua调试模式</summary>
        private void WriteDebugModeToLibraryConfigFile()
        {
            string path = GamePathUtils.IDEDebugger.GetLuaDebugModeLibraryConfigFileFullPath();
            File.WriteAllText(path, ((int)m_LuaDebugMode).ToString());
        }
    }
}