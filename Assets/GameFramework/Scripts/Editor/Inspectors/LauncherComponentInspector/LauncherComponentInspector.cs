using System;
using System.IO;
using Honor.Runtime;
using UnityEditor;
using UnityEngine;

namespace Honor.Editor
{
    [CustomEditor(typeof(LauncherComponent))]
    public class LauncherComponentInspector: HonorComponentInspector
    {
         private static readonly float[] GameSpeed = new float[] { 0f, 0.01f, 0.1f, 0.25f, 0.5f, 1f, 1.5f, 2f, 4f, 8f };
        private static readonly string[] GameSpeedTexts = new string[] { "0x", "0.01x", "0.1x", "0.25x", "0.5x", "1x", "1.5x", "2x", "4x", "8x" };
        private static readonly string[] PlatformStrings = new string[] { "Editor", "Android", "iOS"};

        private SerializedProperty m_EditorResourceMode = null;
        private SerializedProperty m_EditorLanguage = null;
        private SerializedProperty m_DevelopMode = null;
        private SerializedProperty m_IsLocalServer = null;
        private SerializedProperty m_IsRealTimeDebuggerHotfixForEditor = null;
        private SerializedProperty m_LuacMode = null;
        private SerializedProperty m_LuaHotReloadMode = null;
        private SerializedProperty m_DebuggerActiveWindow = null;
        private SerializedProperty m_FrameRate = null;
        private SerializedProperty m_GameSpeed = null;
        private SerializedProperty m_RunInBackground = null;
        private SerializedProperty m_NeverSleep = null;

        private GameDefinitions.DebugMode m_LuaDebugMode = GameDefinitions.DebugMode.None;

        private SerializedProperty m_CustomDevicePerformance = null;
        private SerializedProperty m_UseDevicePerformance = null;
        
        // Editor:0 Android:1 iOS:2
        private SerializedProperty[] m_Performances = new SerializedProperty[3]; 

        private SerializedProperty[] m_ProcessorCounts = new SerializedProperty[3]; 
        private SerializedProperty[] m_GraphicsMemorySizeHighBases = new SerializedProperty[3]; 
        private SerializedProperty[] m_GraphicsMemorySizeMidBases = new SerializedProperty[3]; 
        private SerializedProperty[] m_SystemMemorySizeHighBases = new SerializedProperty[3]; 
        private SerializedProperty[] m_SystemMemorySizeMidBases = new SerializedProperty[3]; 

        private void OnEnable()
        {
            m_EditorResourceMode = serializedObject.FindProperty("m_EditorResourceMode");
            m_EditorLanguage = serializedObject.FindProperty("m_EditorLanguage");
            m_DevelopMode = serializedObject.FindProperty("m_DevelopMode");
            m_IsLocalServer = serializedObject.FindProperty("m_IsLocalServer");
            m_IsRealTimeDebuggerHotfixForEditor = serializedObject.FindProperty("m_IsRealTimeDebuggerHotfixForEditor");
            m_LuacMode = serializedObject.FindProperty("m_LuacMode");
            m_LuaHotReloadMode = serializedObject.FindProperty("m_LuaHotReloadMode");
            m_DebuggerActiveWindow = serializedObject.FindProperty("m_DebuggerActiveWindow");
            m_FrameRate = serializedObject.FindProperty("m_FrameRate");
            m_GameSpeed = serializedObject.FindProperty("m_GameSpeed");
            m_RunInBackground = serializedObject.FindProperty("m_RunInBackground");
            m_NeverSleep = serializedObject.FindProperty("m_NeverSleep");

            m_LuaDebugMode = GameDefinitions.DebugMode.None;

            // 读取LuaDebugMode信息
            ReadLuaDebugModeFromLibraryConfigFile();

            // 读取设备信息
            m_CustomDevicePerformance = serializedObject.FindProperty("m_CustomDevicePerformance");
            m_UseDevicePerformance = serializedObject.FindProperty("m_UseDevicePerformance");
            m_Performances[0] = serializedObject.FindProperty("m_EditorPerformance");
            m_Performances[1] = serializedObject.FindProperty("m_AndroidPerformance");
            m_Performances[2] = serializedObject.FindProperty("m_iOSPerformance");
            for (int i = 0; i < m_Performances.Length; i++)
            {
                m_ProcessorCounts[i] = m_Performances[i].FindPropertyRelative("ProcessorCount");
                m_GraphicsMemorySizeHighBases[i] = m_Performances[i].FindPropertyRelative("GraphicsMemorySizeHighBase");
                m_GraphicsMemorySizeMidBases[i] = m_Performances[i].FindPropertyRelative("GraphicsMemorySizeMidBase");
                m_SystemMemorySizeHighBases[i] = m_Performances[i].FindPropertyRelative("SystemMemorySizeHighBase");
                m_SystemMemorySizeMidBases[i] = m_Performances[i].FindPropertyRelative("SystemMemorySizeMidBase");
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();
            
            LauncherComponent t = (LauncherComponent)target;

            m_EditorResourceMode.boolValue = EditorGUILayout.Toggle("编辑器资源模式", m_EditorResourceMode.boolValue);
            {
                if (!m_EditorResourceMode.boolValue)
                {
                    EditorGUI.BeginDisabledGroup(true);
                }
                EditorGUILayout.HelpBox("编辑器资源模式仅在编辑器下有效，框架将使用编辑器资源文件。\n设备中将强制切换为AssetBundle加载方式。", MessageType.Warning);
                m_IsRealTimeDebuggerHotfixForEditor.boolValue = EditorGUILayout.Toggle("实时调试热更新", m_IsRealTimeDebuggerHotfixForEditor.boolValue);
                EditorGUILayout.HelpBox("编辑器资源模式下实时调试Lua热更新，但下载的热更新文件并不会产生实际作用", MessageType.Info);
                string[] list = Enum.GetNames(typeof(GameDefinitions.Language));
                string codeName = ((GameDefinitions.Language)m_EditorLanguage.enumValueIndex).ToString();
                if (Application.isPlaying)
                {
                    EditorGUILayout.LabelField("编辑器启动语言类型", AorTxt.Format("{0}({1})", codeName, GameDefinitions.LanguageDesc[m_EditorLanguage.enumValueIndex]));
                }
                else
                {
                    m_EditorLanguage.enumValueIndex = EditorGUILayout.Popup("编辑器启动语言类型", m_EditorLanguage.enumValueIndex, list);
                }
                EditorGUILayout.HelpBox("编辑器资源模式：若有设定好的语言类型，则直接使用设置好的语言，否则使用系统当前语言。（编辑器启动语言选项仅适用于编辑器下的本地化测试。）\n非编辑器资源模式：若持久化存档中存在设定好的语言类型，则直接使用设置好的语言，否则使用当前系统语言，不支持系统语言类型时则直接使用通用语言-英语。", MessageType.Info);
                if (!m_EditorResourceMode.boolValue)
                {
                    EditorGUI.EndDisabledGroup();
                }
            }

            if (Application.isPlaying)
            {
                EditorGUI.BeginDisabledGroup(true);
            }

            m_DevelopMode.boolValue = EditorGUILayout.Toggle("开发模式", m_DevelopMode.boolValue);
            EditorGUILayout.HelpBox("开发模式：用于切换开发环境与生产环境的系统配置，模式切换等操作。", MessageType.Info);
            if ( m_DevelopMode.boolValue)
            {
                m_IsLocalServer.boolValue = EditorGUILayout.Toggle("本地服务器", m_IsLocalServer.boolValue);
                EditorGUILayout.HelpBox("本地服务器[172.16.35.16:8081]：用于打包机部署的测试服务器，快速打包测试需求", MessageType.Info);
            }
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
                EditorGUILayout.HelpBox("调试模式：用于切换日常Lua断点调试联调的开关。\n[None]：非调试模式\n[IDE First]：调试模式-IDE优先启动模式\n[Unity First]：调试模式-Unity优先启动模式", MessageType.Info);
                if (debugModeIndex != (int)m_LuaDebugMode)
                {
                    m_LuaDebugMode = (GameDefinitions.DebugMode)debugModeIndex;
                    WriteDebugModeToLibraryConfigFile();
                }
            }

            m_LuacMode.boolValue = EditorGUILayout.Toggle("Luac模式", m_LuacMode.boolValue);
            EditorGUILayout.HelpBox("Luac模式：是否使用Luac脚本。\n设备中将强制切换为Luac模式。", MessageType.Info);

            if (m_EditorResourceMode.boolValue && !m_LuacMode.boolValue)
            {
                m_LuaHotReloadMode.boolValue = EditorGUILayout.Toggle("Lua热重载模式", m_LuaHotReloadMode.boolValue);
                EditorGUILayout.HelpBox("Lua热重载模式：是否使用Lua热重载模式。\n无需重新运行项目便可使修改过的Lua代码即刻生效（存在时效性限制）。\n设备中、非编辑器资源模式下、Luac模式下将强制关闭该模式。", MessageType.Info);
            }
            else
            {
                EditorGUI.BeginDisabledGroup(true);
                m_LuaHotReloadMode.boolValue = EditorGUILayout.Toggle("Lua热重载模式", false);
                EditorGUILayout.HelpBox("Lua热重载模式：当前禁用。", MessageType.Info);
                EditorGUI.EndDisabledGroup();
            }

            if (Application.isPlaying)
            {
                EditorGUI.EndDisabledGroup();
            }

            if (Application.isPlaying)
            {
                EditorGUI.BeginDisabledGroup(true);
            }
            if (Application.isPlaying)
            {
                EditorGUI.EndDisabledGroup();
            }

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

            bool runInBackground = EditorGUILayout.Toggle("启用后台运行", m_RunInBackground.boolValue);
            EditorGUILayout.HelpBox("在移动设备上该属性值会被忽略掉，当为false时，可在编辑器模式下触发应用挂起/恢复回调。", MessageType.Info);
            if (runInBackground != m_RunInBackground.boolValue)
            {
                if (EditorApplication.isPlaying)
                {
                    t.RunInBackground = runInBackground;
                }
                else
                {
                    m_RunInBackground.boolValue = runInBackground;
                }
            }

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

            m_UseDevicePerformance.boolValue = EditorGUILayout.Toggle("开启硬件性能分级/评级", m_UseDevicePerformance.boolValue);
            m_CustomDevicePerformance.boolValue = EditorGUILayout.Toggle("自定义硬件性能分级/评级", m_CustomDevicePerformance.boolValue);
            if (m_CustomDevicePerformance.boolValue)
            {
                int newData = 0;
                EditorGUILayout.BeginVertical("box");
                // 设置cpu核心数量
                EditorGUILayout.Separator();
                for (int i = 0; i < m_ProcessorCounts.Length; i++)
                {
                    string inputProcessorCountEditor = EditorGUILayout.TextField($"{PlatformStrings[i]}-CPU核心数：", m_ProcessorCounts[i].intValue.ToString());
                    if (int.TryParse(inputProcessorCountEditor, out newData) && newData != m_ProcessorCounts[i].intValue)
                    {
                        m_ProcessorCounts[i].intValue = newData;
                    }
                }
                EditorGUILayout.HelpBox("设置通过CPU核心数量判断设备等级的标准。\n低于此标准则为低端机，高于此标准将继续使用下面的参数进行判断", MessageType.Info);

                // 设置显卡
                GUIStyle deviceInfoStyle = new GUIStyle() { alignment = TextAnchor.MiddleRight, normal = { textColor = Color.white } };
                EditorGUILayout.Separator();
                EditorGUILayout.LabelField("高端机", new GUIStyle() { normal = { textColor = DevicePerformance.GetDevicePerformanceLevelColor(DevicePerformanceLevel.High) } });
                for (int i = 0; i < m_GraphicsMemorySizeHighBases.Length; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField($"{PlatformStrings[i]}", GUILayout.Width(60));

                    EditorGUILayout.LabelField("显存>=", deviceInfoStyle, GUILayout.Width(60));
                    string inputGraphicsHighBase = EditorGUILayout.TextField("", m_GraphicsMemorySizeHighBases[i].intValue.ToString(), GUILayout.Width(100));
                    if (int.TryParse(inputGraphicsHighBase, out newData) && newData != m_GraphicsMemorySizeHighBases[i].intValue)
                    {
                        m_GraphicsMemorySizeHighBases[i].intValue = newData;
                    }
                    EditorGUILayout.LabelField("内存>=", deviceInfoStyle, GUILayout.Width(60));
                    string inputMemoryHighBase = EditorGUILayout.TextField("", m_SystemMemorySizeHighBases[i].intValue.ToString(), GUILayout.Width(100));
                    if (int.TryParse(inputMemoryHighBase, out newData) && newData != m_SystemMemorySizeHighBases[i].intValue)
                    {
                        m_SystemMemorySizeHighBases[i].intValue = newData;
                    }
                    EditorGUILayout.EndHorizontal();
                }

                // 开始设置中端机数据
                EditorGUILayout.Separator();
                EditorGUILayout.LabelField("中端机", new GUIStyle() { normal = { textColor = DevicePerformance.GetDevicePerformanceLevelColor(DevicePerformanceLevel.Mid) } });
                for (int i = 0; i < m_GraphicsMemorySizeMidBases.Length; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField($"{PlatformStrings[i]}", GUILayout.Width(60));
                    EditorGUILayout.LabelField("显存>=", deviceInfoStyle, GUILayout.Width(60));
                    if (m_GraphicsMemorySizeHighBases[i].intValue < m_GraphicsMemorySizeMidBases[i].intValue)
                    {
                        GUI.color = Color.red;
                    }
                    string inputGraphicsMidBase = EditorGUILayout.TextField("", m_GraphicsMemorySizeMidBases[i].intValue.ToString(), GUILayout.Width(100));
                    if (int.TryParse(inputGraphicsMidBase, out newData) && newData != m_GraphicsMemorySizeMidBases[i].intValue)
                    {
                        m_GraphicsMemorySizeMidBases[i].intValue = newData;
                    }
                    GUI.color = Color.white;

                    EditorGUILayout.LabelField("内存>=", deviceInfoStyle, GUILayout.Width(60));
                    if (m_SystemMemorySizeHighBases[i].intValue < m_SystemMemorySizeMidBases[i].intValue)
                    {
                        GUI.color = Color.red;
                    }
                    string inputMemoryMidBase = EditorGUILayout.TextField("", m_SystemMemorySizeMidBases[i].intValue.ToString(), GUILayout.Width(100));
                    if (int.TryParse(inputMemoryMidBase, out newData) && newData != m_SystemMemorySizeMidBases[i].intValue)
                    {
                        m_SystemMemorySizeMidBases[i].intValue = newData;
                    }
                    GUI.color = Color.white;
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.HelpBox("高端机和中端机必须要同时满足内存和显卡的数值条件才可以判定为对应的等级", MessageType.Info);

                // 开始展示低端机数据
                EditorGUILayout.Separator();
                EditorGUILayout.LabelField("低端机", new GUIStyle() { normal = { textColor = DevicePerformance.GetDevicePerformanceLevelColor(DevicePerformanceLevel.Low) } });
                for (int i = 0; i < m_GraphicsMemorySizeMidBases.Length; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField($"{PlatformStrings[i]}", GUILayout.Width(80));
                    EditorGUILayout.LabelField($"显存 < {m_GraphicsMemorySizeMidBases[i].intValue}  ||  内存 < {m_SystemMemorySizeMidBases[i].intValue}");
                    EditorGUILayout.EndHorizontal();
                }

                EditorGUILayout.EndVertical();
            }

            serializedObject.ApplyModifiedProperties();

            Repaint();
        }

        protected override void OnCompileStart()
        {
            base.OnCompileStart();

        }

        protected override void OnCompileComplete()
        {
            base.OnCompileComplete();

        }

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
        /// 读取LuaDebugMode信息
        /// </summary>
        private void ReadLuaDebugModeFromLibraryConfigFile()
        {
            string dataPath = GamePathUtils.IDEDebugger.GetLuaDebugModeLibraryConfigFileFullPath();
            if (File.Exists(dataPath))
            {
                string content = System.IO.File.ReadAllText(dataPath);
                m_LuaDebugMode = (GameDefinitions.DebugMode)int.Parse(content);
            }
            else
            {
                WriteDebugModeToLibraryConfigFile();
            }
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