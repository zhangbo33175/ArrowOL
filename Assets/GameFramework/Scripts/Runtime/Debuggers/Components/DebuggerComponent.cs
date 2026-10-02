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
using UnityEngine.EventSystems;
using UnityEngine.Experimental.Rendering;

namespace Honor.Runtime
{
    [DisallowMultipleComponent]
    public sealed partial class DebuggerComponent : GameComponent, ICanvasRaycastFilter, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler,
        IInitializePotentialDragHandler, IPointerDownHandler, IPointerUpHandler

    {
        /// <summary>
        /// 游戏框架组件初始化。
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            m_DebuggerManager = new DebuggerManager();
            if (m_DebuggerManager == null)
            {
                Log.Fatal("Debugger manager 无效。");
                return;
            }

            m_FpsCounter = new FpsCounter(0.5f);
            m_RamCounter = new RamCounter();
            OnTimingReset();

            // 初始化触摸区域大小
            Transform blockTransform = transform.Find("Canvas/Image");
            if (blockTransform != null)
                m_BlockRectTransform = blockTransform.rectTransform();
            else
                Log.Warning("Debugger 未找到 \"Canvas/Image\" 子节点，触摸拦截区域不可用，可在场景中为该 Debugger 对象配置 Canvas/Image 层级启用。");
            RegisterDebuggerWindow(this, "Console", m_ConsoleWindow);
            RegisterDebuggerWindow(this, "Information/System", m_SystemInformationWindow);
            RegisterDebuggerWindow(this, "Information/Environment", m_EnvironmentInformationWindow);
            RegisterDebuggerWindow(this, "Information/Screen", m_ScreenInformationWindow);
            RegisterDebuggerWindow(this, "Information/Graphics", m_GraphicsInformationWindow);
            RegisterDebuggerWindow(this, "Information/Input/Summary", m_InputSummaryInformationWindow);
            RegisterDebuggerWindow(this, "Information/Input/Touch", m_InputTouchInformationWindow);
            RegisterDebuggerWindow(this, "Information/Input/Acceleration", m_InputAccelerationInformationWindow);
            RegisterDebuggerWindow(this, "Information/Input/Gyroscope", m_InputGyroscopeInformationWindow);
            RegisterDebuggerWindow(this, "Information/Input/Compass", m_InputCompassInformationWindow);
            RegisterDebuggerWindow(this, "Information/Other/Scene", m_SceneInformationWindow);
            RegisterDebuggerWindow(this, "Information/Other/Path", m_PathInformationWindow);
            RegisterDebuggerWindow(this, "Information/Other/Time", m_TimeInformationWindow);
            RegisterDebuggerWindow(this, "Information/Other/Quality", m_QualityInformationWindow);
            RegisterDebuggerWindow(this, "Information/Other/Web Player", m_WebPlayerInformationWindow);
            RegisterDebuggerWindow(this, "Profiler/Summary", m_ProfilerInformationWindow);
            RegisterDebuggerWindow(this, "Profiler/Memory/Summary", m_RuntimeMemorySummaryWindow);
            RegisterDebuggerWindow(this, "Profiler/Memory/All", m_RuntimeMemoryAllInformationWindow);
            RegisterDebuggerWindow(this, "Profiler/Memory/Texture", m_RuntimeMemoryTextureInformationWindow);
            RegisterDebuggerWindow(this, "Profiler/Memory/Mesh", m_RuntimeMemoryMeshInformationWindow);
            RegisterDebuggerWindow(this, "Profiler/Memory/Material", m_RuntimeMemoryMaterialInformationWindow);
            RegisterDebuggerWindow(this, "Profiler/Memory/Shader", m_RuntimeMemoryShaderInformationWindow);
            RegisterDebuggerWindow(this, "Profiler/Memory/AnimationClip", m_RuntimeMemoryAnimationClipInformationWindow);
            RegisterDebuggerWindow(this, "Profiler/Memory/AudioClip", m_RuntimeMemoryAudioClipInformationWindow);
            RegisterDebuggerWindow(this, "Profiler/Memory/Font", m_RuntimeMemoryFontInformationWindow);
            RegisterDebuggerWindow(this, "Profiler/Memory/TextAsset", m_RuntimeMemoryTextAssetInformationWindow);
            RegisterDebuggerWindow(this, "Profiler/Memory/ScriptableObject", m_RuntimeMemoryScriptableObjectInformationWindow);
        }

        private void Start()
        {
            m_IconRect = Rect.zero;
            m_winMiniTitle = $"<b><color=#FFFFFFFF>HONOR - </color>{GetDevicePerformanceLevelInfo()}</b>";

            // window大小
            Vector2 screenCanvasSize = GameMainRoot.UI.ScreenUICanvas.rectTransform().sizeDelta;
            m_UICanvasSize = screenCanvasSize;
            Log.Info($"屏幕分辨率： {Screen.width} X {Screen.height}。");
            Log.Info($"屏幕画布尺寸： {screenCanvasSize.x} X {screenCanvasSize.y}，屏幕画布缩放比例：{(screenCanvasSize.x / Screen.width).ToString("f2")}。");
            m_WindowScale = 1;
            m_Portrail = Screen.height > Screen.width;
            if (screenCanvasSize.x > 10)
            {
                m_WindowScale = Screen.width / screenCanvasSize.x;
            }
            m_WindowRect = new Rect((screenCanvasSize.x - screenCanvasSize.x * 0.95f) / 2, (screenCanvasSize.y - screenCanvasSize.y * 0.9f) / 2, screenCanvasSize.x * 0.95f, screenCanvasSize.y * 0.9f);
            m_LogPopWindow = new Rect(0, 0, screenCanvasSize.x, screenCanvasSize.y);
            m_FullWinMaxWidth = m_WindowRect.width - m_FullWinUIBorder;
            m_FullWinMaxHeight = m_WindowRect.height;
            m_FullWinTitleHeight = 60;
            float logStatrY = m_FullWinTitleHeight + (m_CustomButtonHeight + 10) * 3;
            if (m_Portrail)
            {
                logStatrY += m_CustomButtonHeight + 35;
                m_CustomScrollBarValue *= 2;
            }

            // 计算EXECUTE的高度
            GUIStyle style = new GUIStyle
            {
                normal = new GUIStyleState { background = Texture2D.whiteTexture, textColor = Color.white },
                contentOffset = new Vector2(5, 5),
                fontSize = 40,
            };
            float executeHeight = 45;// style.CalcHeight(new GUIContent("TEST"), m_FullWinMaxWidth) + 5;
            m_LogScrollRect = new Rect(m_FullWinUIBorder * 0.5f, logStatrY, m_FullWinMaxWidth - m_CustomScrollBarValue - 2, m_FullWinMaxHeight - logStatrY - 10 - executeHeight);
            m_LogScrollBarRect = new Rect(m_LogScrollRect.x + m_LogScrollRect.width + 2, logStatrY, m_CustomScrollBarValue, m_FullWinMaxHeight - logStatrY - 10 - executeHeight);
            m_LogScrollBarHeight = m_LogScrollBarRect.height - m_CustomScrollBarValue - m_CustomScrollBarValue;
            m_LogScrollBarTop = m_LogScrollBarRect.y + m_CustomScrollBarValue;
            m_LogScrollBarBottom = m_LogScrollBarRect.y + m_LogScrollBarRect.height - m_CustomScrollBarValue;
            // 重设skin数值
            if (m_Skin != null)
            {
                m_Skin.button.fixedHeight = m_CustomButtonHeight;
                m_Skin.verticalScrollbar.fixedWidth = m_CustomScrollBarValue;
                m_Skin.verticalScrollbarThumb.fixedWidth = m_CustomScrollBarValue;

                m_Skin.verticalScrollbarUpButton.fixedWidth = m_CustomScrollBarValue;
                m_Skin.verticalScrollbarUpButton.fixedHeight = m_CustomScrollBarValue * 0.5f;
                m_Skin.verticalScrollbarDownButton.fixedWidth = m_CustomScrollBarValue;
                m_Skin.verticalScrollbarDownButton.fixedHeight = m_CustomScrollBarValue * 0.5f;

                m_Skin.horizontalScrollbar.fixedHeight = m_CustomScrollBarValue;
                m_Skin.horizontalScrollbarThumb.fixedHeight = m_CustomScrollBarValue;

                m_Skin.horizontalScrollbarLeftButton.fixedWidth = m_CustomScrollBarValue * 0.5f;
                m_Skin.horizontalScrollbarLeftButton.fixedHeight = m_CustomScrollBarValue;
                m_Skin.horizontalScrollbarRightButton.fixedWidth = m_CustomScrollBarValue * 0.5f;
                m_Skin.horizontalScrollbarRightButton.fixedHeight = m_CustomScrollBarValue;
            }
        }

        private void OnDestroy()
        {
        }

        private void Update()
        {
            // 根据时间判断是否要进入idle状态
            CheckIdleState();

            if(m_TargetPos != m_DefaultTargetPos)
            {
                m_IconRect.x = Mathf.Lerp(m_IconRect.x, m_TargetPos.x, 0.3f);
                m_IconRect.y = Mathf.Lerp(m_IconRect.y, m_TargetPos.y, 0.3f);
            }

            m_FpsCounter.Update(Time.deltaTime, Time.unscaledDeltaTime);
            m_RamCounter.Update(Time.deltaTime, Time.unscaledDeltaTime);
            m_DebuggerManager.Update(Time.deltaTime, Time.unscaledDeltaTime);
        }

        private void LateUpdate()
        {
            float deltaTime = Time.unscaledDeltaTime;
            if (m_WinModel == GameDefinitions.DebugWindowModel.FullWindow)
            {
                if (m_Inertia)
                {
                    if (m_FullWindowInDrag)
                    {
                        Vector3 newVelocity = new Vector3(0, m_LastScrollOffsetY, 0) / deltaTime;
                        m_Velocity = Vector3.Lerp(m_Velocity, newVelocity, deltaTime * 10);
                    }
                    else
                    {
                        m_Velocity[1] *= Mathf.Pow(m_DecelerationRate, deltaTime);
                        if (Mathf.Abs(m_Velocity[1]) < 0.001f)
                            m_Velocity[1] = 0;
                        m_SubWindowDragY = m_Velocity[1] * deltaTime;
                    }
                }
            }

        }

        private void OnGUI()
        {
            if (m_DebuggerManager == null || !m_DebuggerManager.ActiveWindow)
            {
                return;
            }

            GUISkin cachedGuiSkin = GUI.skin;
            Matrix4x4 cachedMatrix = GUI.matrix;

            GUI.matrix = Matrix4x4.Scale(new Vector3(m_WindowScale, m_WindowScale, 1f));

            if (m_WinModel == GameDefinitions.DebugWindowModel.FullWindow || m_WinModel == GameDefinitions.DebugWindowModel.PopWindow)
            {
                GUI.skin = m_Skin;
                m_WindowRect = GUILayout.Window(0, m_WindowRect, DrawWindow, $"<b>HONOR DEBUGGER - </b>{GetDevicePerformanceLevelInfo()}");
                // 显示选中节点内容
                if (m_WinModel == GameDefinitions.DebugWindowModel.PopWindow)
                {
                    GUI.skin = m_PopWindowSkin;
                    m_LogPopWindow = GUILayout.Window(100, m_LogPopWindow, DrawWindowConsoleLog, $"<b>HONOR DEBUGGER SELECT LOG - </b>{GetDevicePerformanceLevelInfo()}");
                }
                RefreshBlockRect(m_WindowRect);
                m_IconRect.width = 0;
                m_IconRect.height = 0;
            }
            else
            {
                GUI.skin = m_MiniSkin;
                if (m_IdleState)
                {
                    m_IconRect = GUILayout.Window(0, m_IconRect, DrawDebuggerWindowIcon, m_winMiniTitle, m_MiniSkin.customStyles[1]);
                }
                else
                {
                    m_IconRect = GUILayout.Window(0, m_IconRect, DrawDebuggerWindowIcon, m_winMiniTitle);
                }

                if (m_StayBorder)
                {
                    if (!m_WindowInDrag)
                    {
                        if (m_TargetPos == m_DefaultTargetPos)
                        {
                            // 多跳一帧再来刷新目标位置
                            m_TargetPos = m_SkipTargetPos;
                        }
                        else if (m_TargetPos == m_SkipTargetPos)
                        {
                            m_TargetPos = UpdateWindowPositionWithStayBordor(m_IconRect);
                        }
                    }
                }

                RefreshBlockRect(m_IconRect);
            }

            GUI.matrix = cachedMatrix;
            //    GUI.skin = cachedGuiSkin;
        }

        public void SetActiveWindow()
        {
            LauncherComponent launcherComponent = GameComponentsGroup.GetComponent<LauncherComponent>();
            if (launcherComponent == null)
            {
                Log.Fatal("Launcher Component 无效。");
                return;
            }

            switch (launcherComponent.DebuggerActiveWindow)
            {
                case DebuggerActiveWindowType.AlwaysOpen:
                    ActiveWindow = true;
                    break;

                case DebuggerActiveWindowType.OnlyOpenWhenDevelopment:
                    ActiveWindow = launcherComponent.DevelopMode;
                    break;

                case DebuggerActiveWindowType.OnlyOpenInEditor:
                    ActiveWindow = Application.isEditor;
                    break;

                default:
                    ActiveWindow = false;
                    break;
            }
        }

        /// <summary>
        /// 刷新阻止触摸区域大小，位置
        /// </summary>
        /// <param name=""></param>
        public void RefreshBlockRect(Rect newRect)
        {
            if (m_BlockRectTransform != null)
            {
                switch (m_WinModel)
                {
                    case GameDefinitions.DebugWindowModel.MiniWindow:
                    case GameDefinitions.DebugWindowModel.FPSWindow:
                    case GameDefinitions.DebugWindowModel.FullWindow:
                        m_BlockRectTransform.position = new Vector2(newRect.x * m_WindowScale, Screen.height - newRect.y * m_WindowScale);
                        m_BlockRectTransform.sizeDelta = new Vector2(newRect.width * m_WindowScale, newRect.height * m_WindowScale);
                        break;
                    case GameDefinitions.DebugWindowModel.PopWindow:
                        m_BlockRectTransform.position = new Vector2(0, Screen.height * m_WindowScale);
                        m_BlockRectTransform.sizeDelta = new Vector2(Screen.width * m_WindowScale, Screen.height * m_WindowScale);
                        break;
                }
            }
        }


        /// <summary>
        /// 阻止触摸向下穿透
        /// </summary>
        /// <param name="screenPos"></param>
        /// <param name="eventCamera"></param>
        /// <returns></returns>
        bool ICanvasRaycastFilter.IsRaycastLocationValid(Vector2 screenPos, Camera eventCamera)
        {
            // 如果当前是debug模式，返回false
            if (GameMainRoot.Launcher.DebuggerActiveWindow == DebuggerActiveWindowType.AlwaysOpen)
            {
                // 等待下面的判断
            }
            else if (GameMainRoot.Launcher.DebuggerActiveWindow == DebuggerActiveWindowType.OnlyOpenWhenDevelopment)
            {
                if (GameMainRoot.Launcher.DevelopMode == false) return false;
            }
            else if (GameMainRoot.Launcher.DebuggerActiveWindow == DebuggerActiveWindowType.OnlyOpenInEditor)
            {
                if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
                {
                    return false;
                }
            }
            else if (GameMainRoot.Launcher.DebuggerActiveWindow == DebuggerActiveWindowType.AlwaysClose)
            {
                return false;
            }
            if (m_BlockRectTransform == null) return false;
            return RectTransformUtility.RectangleContainsScreenPoint(m_BlockRectTransform, screenPos, eventCamera);
        }


        /// <summary>
        /// 注册调试器窗口。
        /// </summary>
        /// <param name="path">调试器窗口路径。</param>
        /// <param name="debuggerWindow">要注册的调试器窗口。</param>
        /// <param name="args">初始化调试器窗口参数。</param>
        public void RegisterDebuggerWindow(DebuggerComponent debugComponent, string path, IDebuggerWindow debuggerWindow, params object[] args)
        {
            m_DebuggerManager.RegisterDebuggerWindow(debugComponent, path, debuggerWindow, args);
        }

        /// <summary>
        /// 获取调试器窗口。
        /// </summary>
        /// <param name="path">调试器窗口路径。</param>
        /// <returns>要获取的调试器窗口。</returns>
        public IDebuggerWindow GetDebuggerWindow(string path)
        {
            return m_DebuggerManager.GetDebuggerWindow(path);
        }

        /// <summary>
        /// 选中调试器窗口。
        /// </summary>
        /// <param name="path">调试器窗口路径。</param>
        /// <returns>是否成功选中调试器窗口。</returns>
        public bool SelectDebuggerWindow(string path)
        {
            return m_DebuggerManager.SelectDebuggerWindow(path);
        }

        /// <summary>
        /// 还原调试器窗口布局。
        /// </summary>
        public void ResetLayout()
        {
            IconRect = Rect.zero;
            WindowRect = Rect.zero;
        }

        /// <summary>
        /// 获取记录的全部日志。
        /// </summary>
        /// <param name="results">要获取的日志。</param>
        public void GetRecentLogs(List<LogNode> results)
        {
            m_ConsoleWindow.GetRecentLogs(results);
        }

        /// <summary>
        /// 获取记录的最近日志。
        /// </summary>
        /// <param name="results">要获取的日志。</param>
        /// <param name="count">要获取最近日志的数量。</param>
        public void GetRecentLogs(List<LogNode> results, int count)
        {
            m_ConsoleWindow.GetRecentLogs(results, count);
        }

        private void DrawWindow(int windowId)
        {
            GUI.DragWindow(m_FullWindowDragRect);
            DrawDebuggerWindowGroup(m_DebuggerManager.DebuggerWindowRoot);
        }

        /// <summary>
        /// 显示全屏debug信息窗口
        /// </summary>
        /// <param name="debuggerWindowGroup"></param>
        private void DrawDebuggerWindowGroup(IDebuggerWindowGroup debuggerWindowGroup)
        {
            if (debuggerWindowGroup == null)
            {
                return;
            }

            List<string> names = new List<string>();
            string[] debuggerWindowNames = debuggerWindowGroup.GetDebuggerWindowNames();
            for (int i = 0; i < debuggerWindowNames.Length; i++)
            {
                names.Add($"<b>{debuggerWindowNames[i]}</b>");
            }

            if (debuggerWindowGroup == m_DebuggerManager.DebuggerWindowRoot)
            {
                names.Add("<b>Close</b>");
            }

            int fontSize = GUI.skin.button.fontSize;
            float fixedHeight = GUI.skin.button.fixedHeight;
            GUI.skin.button.fontSize = m_CustomButtonFontSize;
            GUI.skin.button.fixedHeight = m_CustomButtonHeight;
            if (m_Portrail)
            {
                if (names.Count > 6)
                {
                    GUI.skin.button.fontSize = (int)(m_CustomButtonFontSize * 0.75);
                    GUI.skin.button.fixedHeight = (int)(m_CustomButtonHeight * 2.5);
                }
                else if (names.Count > 4)
                {
                    GUI.skin.button.fontSize = (int)(m_CustomButtonFontSize * 0.75);
                    GUI.skin.button.fixedHeight = (int)(m_CustomButtonHeight * 1.25);
                }
            }

            int toolbarIndex = GUILayout.Toolbar(debuggerWindowGroup.SelectedIndex, names.ToArray(), GUILayout.MaxWidth(m_FullWinMaxWidth));
            if (toolbarIndex >= debuggerWindowGroup.DebuggerWindowCount)
            {
                if(m_WinModel == GameDefinitions.DebugWindowModel.FullWindow)
                {
                    OnTimingReset();
                    m_WinModel = GameDefinitions.DebugWindowModel.MiniWindow;
                    return;
                }
            }

            if (debuggerWindowGroup.SelectedWindow == null)
            {
                return;
            }

            if (debuggerWindowGroup.SelectedIndex != toolbarIndex)
            {
                debuggerWindowGroup.SelectedWindow.OnLeave();
                debuggerWindowGroup.SelectedIndex = toolbarIndex;
                debuggerWindowGroup.SelectedWindow.OnEnter();
            }

            IDebuggerWindowGroup subDebuggerWindowGroup = debuggerWindowGroup.SelectedWindow as IDebuggerWindowGroup;
            if (subDebuggerWindowGroup != null)
            {
                DrawDebuggerWindowGroup(subDebuggerWindowGroup);
            }

            debuggerWindowGroup.SelectedWindow.OnDraw(this, m_SubWindowDragY);
            m_SubWindowDragY = 0;

            GUI.skin.button.fontSize = fontSize;
            GUI.skin.button.fixedHeight = fixedHeight;
        }

        /// <summary>
        /// 显示log信息弹窗框
        /// </summary>
        /// <param name="windowId"></param>
        private void DrawWindowConsoleLog(int windowId)
        {
            ConsoleWindow curWindow = m_DebuggerManager.DebuggerWindowRoot.SelectedWindow as ConsoleWindow;

            GUILayout.BeginVertical("box");
            {
                m_StackScrollPosition.y += m_SubWindowDragY;
                m_SubWindowDragY = 0;
                m_StackScrollPosition = GUILayout.BeginScrollView(m_StackScrollPosition, GUILayout.MaxHeight(m_LogPopWindow.height * 0.70f));
                {
                    GUILayout.Label(m_SelLogMsg);
                    GUILayout.Label(m_SelLogStackTrack);
                    GUILayout.EndScrollView();
                }
            }
            GUILayout.EndVertical();

            GUILayout.BeginHorizontal("box");
            {
                if (GUILayout.Button("CLOSE"))
                {
                    m_WinModel = GameDefinitions.DebugWindowModel.FullWindow;
                    m_SelLogMsg = "";
                    m_SelLogStackTrack = "";
                }
                GUILayout.Space(50);
                if (GUILayout.Button("COPY"))
                {
                    m_TextEditor.text = $"{m_SelLogMsg}\r\n{m_SelLogStackTrack}";
                    m_TextEditor.OnFocus();
                    m_TextEditor.Copy();
                    m_TextEditor.text = null;
                }
            }
            GUILayout.EndHorizontal();

        }


        private void DrawDebuggerWindowIcon(int windowId)
        {
            DrawIconState0();
            DrawIconState1();
        }

        /// <summary>
        /// 根据当前log的层级获取文本颜色
        /// </summary>
        /// <returns></returns>
        private Color32 GetCurTextColor()
        {
            Color32 color = Color.white;

            if (LogNode.FatalCount > 0)
            {
                color = m_FatalColor;// m_ConsoleWindow.GetLogStringColor(LogType.Exception, IdleState);
            }
            else if (LogNode.ErrorCount > 0)
            {
                color = m_ErrorColor;// m_ConsoleWindow.GetLogStringColor(LogType.Error, IdleState);
            }
            else if (LogNode.WarningCount > 0)
            {
                color = m_WarningColor;// m_ConsoleWindow.GetLogStringColor(LogType.Warning, IdleState);
            }
            else
            {
                color = m_InfoColor;// m_ConsoleWindow.GetLogStringColor(LogType.Log, IdleState);
            }
            return color;
        }

        private string GetDevicePerformanceLevelInfo()
        {
            if(GameMainRoot.Launcher.UseDevicePerformance)
            {
                DevicePerformanceLevel level = DevicePerformance.GetDevicePerformanceLevel();
                Color32 defaultColor = DevicePerformance.GetDevicePerformanceLevelColor(level);
                if (m_IdleState)
                {
                    defaultColor.a = 0x50;
                }
                switch (level)
                {
                    case DevicePerformanceLevel.High:
                        // 绿色
                        return $"<color=#{ColorUtility.ToHtmlStringRGBA(defaultColor)}><b>HIGH</b></color>";
                    case DevicePerformanceLevel.Mid:
                        // 黄色
                        return $"<color=#{ColorUtility.ToHtmlStringRGBA(defaultColor)}><b>MID</b></color>";
                    case DevicePerformanceLevel.Low:
                        // 紫色
                        return $"<color=#{ColorUtility.ToHtmlStringRGBA(defaultColor)}><b>LOW</b></color>";
                }
            }
            return $"<b>Default</b>";
        }

        /// <summary>
        /// 显示最小的窗口状态
        /// </summary>
        private void DrawIconState0()
        {
            if (m_WinModel == GameDefinitions.DebugWindowModel.MiniWindow)
            {
                Color32 color = GetCurTextColor();

                GUI.DragWindow(new Rect(0,0,m_IconRect.width,m_IconRect.height));
                GUILayout.Space(20);
                string content = $"\n<color=#{ColorUtility.ToHtmlStringRGBA(color)}><b>FPS:{m_FpsCounter.CurrentFps.ToString("F2")}</b></color>\n";
                GUI.skin.button.fontSize = m_CustomButtonFontSize;
                GUILayout.Label(content, new GUIStyle() { fontSize = m_MiniIconInnerButtonFontSize, alignment = TextAnchor.MiddleCenter }, GUILayout.Width(m_MiniIconInnerButtonWidth), GUILayout.Height(m_MiniIconInnerButtonHeight));
                content = null;
            }
        }

        /// <summary>
        /// 显示中间状态的窗口
        /// </summary>
        private void DrawIconState1()
        {
            if (m_WinModel == GameDefinitions.DebugWindowModel.FPSWindow)
            {
                Color32 color = GetCurTextColor();

                GUI.DragWindow(new Rect(0,0,m_IconRect.width,m_IconRect.height));
                string colorFps1 = m_IdleState? "<color=#00858450>":"<color=#008584FF>";
                string colorFps2 = m_IdleState ? "<color=#500D3F50>" : "<color=#500D3FFF>";
                string colorReservedRam = m_IdleState ? "<color=#8900A450>" : "<color=#8900A4FF>";
                string colorAllocatedRam = m_IdleState ? "<color=#8A5C0050>" : "<color=#8A5C00FF>";
                string colorReservedMonoRam = m_IdleState ? "<color=#00821A50>" : "<color=#00821AFF>";
                string colorAllocatedMonoRam = m_IdleState ? "<color=#500D3F50>" : "<color=#500D3FFF>";
                GUILayout.Space(5);
                string currentFps = string.Format("  <color=#{0}><b>FPS-CUR: {3}{1}</color> / {4}{2}</color> ms</b></color>   \n", ColorUtility.ToHtmlStringRGBA(color), m_FpsCounter.CurrentFps.ToString("F2"), m_FpsCounter.CurrentMsPerFrame.ToString("F1"), colorFps1, colorFps2);
                string averageFps = string.Format("  <color=#{0}><b>FPS-AVG: {3}{1}</color> / {4}{2}</color> ms</b></color>   \n", ColorUtility.ToHtmlStringRGBA(color), m_FpsCounter.AverageFps.ToString("F2"), m_FpsCounter.AverageMsPerFrame.ToString("F1"), colorFps1, colorFps2);
                string reservedRam = string.Format("  <color=#{0}><b>RAM-Reserved: {2}{1}</color> MB</b></color>   \n", ColorUtility.ToHtmlStringRGBA(color), (int)m_RamCounter.ReservedRam, colorReservedRam);
                string allocatedRam = string.Format("  <color=#{0}><b>RAM-Allocated: {2}{1}</color> MB</b></color>   \n", ColorUtility.ToHtmlStringRGBA(color), (int)m_RamCounter.AllocatedRam, colorAllocatedRam);
                string reservedMonoRam = string.Format("  <color=#{0}><b>MONO-Reserved: {2}{1}</color> MB</b></color>   \n", ColorUtility.ToHtmlStringRGBA(color), (int)m_RamCounter.ReservedMonoRam, colorReservedMonoRam);
                string allocatedMonoRam = string.Format("  <color=#{0}><b>MONO-Allocated: {2}{1}</color> MB</b></color>   \n", ColorUtility.ToHtmlStringRGBA(color), (int)m_RamCounter.AllocatedMonoRam, colorAllocatedMonoRam);
                string content = string.Format("\n\n{0}{1}\n{2}{3}\n{4}{5}", currentFps, averageFps, reservedRam, allocatedRam, reservedMonoRam, allocatedMonoRam);
                GUI.skin.button.fontSize = m_CustomButtonFontSize;
                GUILayout.Label(content, new GUIStyle() { fontSize = m_IconInnerButtonFontSize, alignment = TextAnchor.MiddleLeft }, GUILayout.Width(m_IconInnerButtonWidth), GUILayout.Height(m_IconInnerButtonHeight));
                GUILayout.Label(string.Empty);
                content = null;
            }
        }

        /// <summary>
        /// 点击其它按钮时，让当前输入控件失去焦点
        /// </summary>
        public void ControlInput()
        {
            GUIUtility.keyboardControl = 0;
        }

        /// <summary>
        /// debug窗口是否激活
        /// </summary>
        /// <returns></returns>
        public bool IsDebugWinActive()
        {
            if (GameMainRoot.Launcher.DebuggerActiveWindow == null)
            {
                return false;
            }
            switch (GameMainRoot.Launcher.DebuggerActiveWindow)
            {
                case DebuggerActiveWindowType.AlwaysOpen:
                    return true;
                case DebuggerActiveWindowType.OnlyOpenWhenDevelopment:
                    return GameMainRoot.Launcher.DevelopMode;
                case DebuggerActiveWindowType.OnlyOpenInEditor:
                    if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
                    {
                        return true;
                    }
                    return false;
                case DebuggerActiveWindowType.AlwaysClose:
                    return false;
                default:
                    return false;
            }
            return false;
        }

        /// <summary>
        /// 处理触摸开始拖拽事件，在OnPointerDown之后响应
        /// </summary>
        /// <param name="eventData"></param>
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!IsDebugWinActive())
            {
                return;
            }
            m_WindowInDrag = false;
            m_WindowInTouchDown = false;
            m_FullWindowInTouchDown = false;
            m_FullWindowInDrag = false;
            m_TargetPos = m_DefaultTargetPos;
            m_LastScrollOffsetY = 0;

            ControlInput();

            switch (m_WinModel)
            {
                case GameDefinitions.DebugWindowModel.MiniWindow:
                case GameDefinitions.DebugWindowModel.FPSWindow:
                    //Runtime.Log.Info("DebugWindow MiniWindow/FPSWindow OnBeginDrag");
                    m_WindowInDrag = true;
                    break;
                case GameDefinitions.DebugWindowModel.FullWindow:
                    //Runtime.Log.Info("DebugWindow FullWindow OnBeginDrag");
                    Vector2 downPos = new Vector2(eventData.pressPosition.x / m_WindowScale, (Screen.height - eventData.pressPosition.y) / m_WindowScale);
                    Vector2 winStartPos = new Vector2(m_WindowRect.x + m_LogScrollRect.x, m_WindowRect.y + m_LogScrollRect.y);
                    if (downPos.x >= winStartPos.x && downPos.x <= winStartPos.x + m_LogScrollRect.width
                         && downPos.y >= winStartPos.y && downPos.y <= winStartPos.y + m_LogScrollRect.height)
                    {
                        m_FullWindowInDrag = true;
                        m_FullWindowDownPos = downPos;
                    }
                    break;
                case GameDefinitions.DebugWindowModel.PopWindow:
                    m_FullWindowInDrag = true;
                    m_FullWindowDownPos = new Vector2(eventData.pressPosition.x / m_WindowScale, (Screen.height - eventData.pressPosition.y) / m_WindowScale);
                    break;
            }
            // 重置时间
            OnTimingReset();
        }
        /// <summary>
        /// 处理触摸拖动事件
        /// </summary>
        /// <param name="eventData"></param>
        public void OnDrag(PointerEventData eventData)
        {
            if (!IsDebugWinActive())
            {
                return;
            }
            m_WindowInTouchDown = false;
            m_FullWindowInTouchDown = false;
            m_TargetPos = m_DefaultTargetPos;
            ControlInput();
            switch (m_WinModel)
            {
                case GameDefinitions.DebugWindowModel.MiniWindow:
                case GameDefinitions.DebugWindowModel.FPSWindow:
                    //Runtime.Log.Info("DebugWindow MiniWindow/FPSWindow OnDrag");
                    // if (m_WindowInDrag)
                    // {
                    //     m_IconRect = new Rect(m_IconRect.x + eventData.delta.x, m_IconRect.y - eventData.delta.y, m_IconRect.width, m_IconRect.height);
                    // }
                    break;
                case GameDefinitions.DebugWindowModel.FullWindow:
                case GameDefinitions.DebugWindowModel.PopWindow:
                    //Runtime.Log.Info("DebugWindow FullWindow/PopWindow OnDrag");
                    if (m_FullWindowInDrag)
                    {
                        m_SubWindowDragY = eventData.delta.y;
                    }
                    break;
            }
            // 重置时间
            OnTimingReset();
        }

        /// <summary>
        /// 处理触摸抬起事件，OnDrop先于OnEndDrag被响应
        /// 在editor里面，当鼠标拖拽时移出屏幕区域后抬起，则不会调用OnDrop
        /// </summary>
        /// <param name="eventData"></param>
        public void OnDrop(PointerEventData eventData)
        {
            //Runtime.Log.Info("DebugWindow MiniWindow/FPSWindow OnDrop");
            // 重置时间
            if (!IsDebugWinActive())
            {
                return;
            }
            OnTimingReset();
            m_TargetPos = m_DefaultTargetPos;
        }

        /// <summary>
        /// 处理触摸抬起事件
        /// </summary>
        /// <param name="eventData"></param>
        public void OnEndDrag(PointerEventData eventData)
        {
            if (!IsDebugWinActive())
            {
                return;
            }
            m_TargetPos = m_DefaultTargetPos;
            ControlInput();
            //Runtime.Log.Info("DebugWindow MiniWindow/FPSWindow OnEndDrag");
            switch (m_WinModel)
            {
                case GameDefinitions.DebugWindowModel.MiniWindow:
                case GameDefinitions.DebugWindowModel.FPSWindow:
                    //Runtime.Log.Info("DebugWindow MiniWindow/FPSWindow OnEndDrag");
                    m_WindowInDrag = false;
                    m_WindowInTouchDown = false;
                    m_FullWindowInTouchDown = false;
                    break;
                case GameDefinitions.DebugWindowModel.FullWindow:
                case GameDefinitions.DebugWindowModel.PopWindow:
                    //Runtime.Log.Info("DebugWindow FullWindow/PopWindow OnDrop");
                    m_FullWindowInDrag = false;
                    m_SubWindowDragY = 0;
                    break;
            }
            // 重置时间
            OnTimingReset();
        }


        public void OnInitializePotentialDrag(PointerEventData eventData)
        {
            if (!IsDebugWinActive())
            {
                return;
            }
            //当鼠标在A对象按下还没开始拖拽时 A对象响应此事件
            //注：此接口事件与IPointerDownHandler接口事件类似
            //    有兴趣的朋友可以测试下二者的执行顺序这里不再赘述
            //Runtime.Log.Info("OnInitializePotentialDrag ");
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            m_LastScrollOffsetY = 0;
            m_Velocity = Vector2.zero;
        }

        /// <summary>
        /// 处理触摸按下事件
        /// </summary>
        /// <param name="eventData"></param>

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!IsDebugWinActive())
            {
                return;
            }
            m_WindowInDrag = false;
            m_WindowInTouchDown = false;
            m_FullWindowInTouchDown = false;
            m_FullWindowInDrag = false;
            m_TargetPos = m_DefaultTargetPos;
            m_LastScrollOffsetY = 0;
            // 让当前输入控件失去焦点
            GUIUtility.keyboardControl = 0;

            switch (m_WinModel)
            {
                case GameDefinitions.DebugWindowModel.MiniWindow:
                case GameDefinitions.DebugWindowModel.FPSWindow:
                    //Runtime.Log.Info("DebugWindow MiniWindow/FPSWindow OnPointerDown");
                    if (!m_IdleState)
                    {
                        m_WindowInTouchDown = true;
                    }
                    break;
                case GameDefinitions.DebugWindowModel.FullWindow:
                    m_FullWindowInTouchDown = true;
                    break;
                case GameDefinitions.DebugWindowModel.PopWindow:
                    break;
            }
            // 重置时间
            OnTimingReset();
        }

        /// <summary>
        /// 处理触摸非拖拽后抬起事件
        /// </summary>
        /// <param name="eventData"></param>

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!IsDebugWinActive())
            {
                return;
            }
            m_TargetPos = m_DefaultTargetPos;
            switch (m_WinModel)
            {
                case GameDefinitions.DebugWindowModel.MiniWindow:
                    {
                        //Runtime.Log.Info("DebugWindow MiniWindow OnPointerUp");
                        if (m_WindowInTouchDown && !m_WindowInDrag)
                        {
                            m_WinModel = GameDefinitions.DebugWindowModel.FPSWindow;
                        }
                    }
                    break;
                case GameDefinitions.DebugWindowModel.FPSWindow:
                    { 
                        //Runtime.Log.Info("DebugWindow FPSWindow OnPointerUp");
                        if (m_WindowInTouchDown && !m_WindowInDrag)
                        {
                            m_WinModel = GameDefinitions.DebugWindowModel.FullWindow;
                            m_IconRect.width = 0;
                            m_IconRect.height = 0;
                        }
                    }
                    break;
                case GameDefinitions.DebugWindowModel.FullWindow:
                    if (m_FullWindowInTouchDown && !m_FullWindowInDrag && m_ConsoleWindow != null)
                    {
                        Vector2 downPos = new Vector2(eventData.pressPosition.x / m_WindowScale, (Screen.height - eventData.pressPosition.y) / m_WindowScale);
                        Vector2 winStartPos = new Vector2(m_WindowRect.x + m_LogScrollRect.x, m_WindowRect.y + m_LogScrollRect.y);
                        if (downPos.x >= winStartPos.x && downPos.x <= winStartPos.x + m_LogScrollRect.width
                             && downPos.y >= winStartPos.y && downPos.y <= winStartPos.y + m_LogScrollRect.height)
                        {
                            m_ConsoleWindow.OnEndDown(downPos.y - winStartPos.y);
                        }
                    }
                    break;
                case GameDefinitions.DebugWindowModel.PopWindow:
                    break;
            }
            m_WindowInTouchDown = false;
            // 重置时间
            OnTimingReset();
        }

        /// <summary>
        /// 方向枚举定义
        /// </summary>
        enum Direction
        {
            Left = 0,
            Right,
            Top,
            Bottom,
            Total,
        }

        /// <summary>
        /// 当松手时重置debug窗口的位置
        /// </summary>
        private Vector2 UpdateWindowPositionWithStayBordor(Rect winRect)
        {
            Vector2 canvasSize = m_UICanvasSize;//GameMainRoot.UI.ScreenUICanvas.rectTransform().sizeDelta; //new Vector2(Screen.width * m_WindowScale, Screen.height * m_WindowScale);
            // 纠正debug窗口的位置在屏幕内
            Vector2 pos = new Vector2(Mathf.Clamp(winRect.x, 0, canvasSize.x - winRect.width), Mathf.Clamp(winRect.y, 0, canvasSize.y - winRect.height));

            float distToLeft = pos.x;
            float distToRight = canvasSize.x - distToLeft - winRect.width;

            float distToTop = pos.y;
            float distToBottom = canvasSize.y - distToTop - winRect.height;

            float[] dists = new float[(int)Direction.Total];
            dists[(int)Direction.Left] = distToLeft / canvasSize.x;
            dists[(int)Direction.Right] = distToRight / canvasSize.x;
            dists[(int)Direction.Top] = distToTop / canvasSize.y;
            dists[(int)Direction.Bottom] = distToBottom / canvasSize.y;

            Direction minDirection = Direction.Left;
            for(int index = 0; index < dists.Length; index++)
            {
                if(dists[index] < dists[(int)minDirection])
                {
                    minDirection = (Direction)index;
                }
            }

            Vector2 posTarget = Vector2.zero;
            switch (minDirection)
            {
                case Direction.Left: posTarget = new Vector2(0, pos.y); break;
                case Direction.Right: posTarget = new Vector2(canvasSize.x - winRect.width, pos.y); break;
                case Direction.Top: posTarget = new Vector2(pos.x, 0); break;
                case Direction.Bottom: posTarget = new Vector2(pos.x, canvasSize.y - winRect.height); break;
            }

            return posTarget;
        }

        private void OnTimingReset()
        {
            m_IdleStartSecond = System.DateTime.Now.Ticks;
            CheckIdleState();
        }

        private void CheckIdleState()
        {
            bool oldState = m_IdleState;
            switch (m_WinModel)
            {
                case GameDefinitions.DebugWindowModel.MiniWindow:
                case GameDefinitions.DebugWindowModel.FPSWindow:
                    if (m_WindowInDrag || m_WindowInTouchDown)
                    {
                        m_IdleState = false;
                    }
                    else
                    {
                        m_IdleState = ((m_EnterIdleTime > 0) && ((System.DateTime.Now.Ticks - m_IdleStartSecond) > (m_EnterIdleTime * 10000000))) ? true : false;
                    }
                    break;
                case GameDefinitions.DebugWindowModel.FullWindow:
                case GameDefinitions.DebugWindowModel.PopWindow:
                    m_IdleState = false;
                    break;
            }
            if (oldState != m_IdleState)
            {
                if (m_IdleState)
                {
                    m_winMiniTitle = $"<b><color=#FFFFFF50>HONOR - </color></b>{GetDevicePerformanceLevelInfo()}";
                    m_winMiniStyle = "IdleWindow";
                }
                else
                {
                    m_winMiniTitle = $"<b><color=#FFFFFFFF>HONOR - </color></b>{GetDevicePerformanceLevelInfo()}";
                    m_winMiniStyle = "window";
                }
            }
        }

    }
}


