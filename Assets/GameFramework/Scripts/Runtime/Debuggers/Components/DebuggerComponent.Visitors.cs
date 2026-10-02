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

namespace Honor.Runtime
{
    public sealed partial class DebuggerComponent : GameComponent, ICanvasRaycastFilter, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler,
        IInitializePotentialDragHandler, IPointerDownHandler, IPointerUpHandler

    {
        #region MiniWin
        /// <summary>
        /// Debugger管理器
        /// </summary>
        private DebuggerManager m_DebuggerManager = null;

        /// <summary>
        /// Mini悬浮Icon内嵌按钮文字字号
        /// </summary>
        private int m_MiniIconInnerButtonFontSize = 30;

        /// <summary>
        /// Mini悬浮Icon内嵌按钮宽度
        /// </summary>
        private float m_MiniIconInnerButtonWidth = 180f;

        /// <summary>
        /// Mini悬浮Icon内嵌按钮高度
        /// </summary>
        private float m_MiniIconInnerButtonHeight = 70f;
        #endregion MiniWin

        [SerializeField]
        private Color32 m_InfoColor = Color.white;
        public Color32 InfoColor
        { 
            get { return m_InfoColor; }
        }

        [SerializeField]
        private Color32 m_WarningColor = Color.yellow;
        public Color32 WarningColor
        {
            get { return m_WarningColor; }
        }

        [SerializeField]
        private Color32 m_ErrorColor = Color.red;
        public Color32 ErrorColor
        {
            get { return m_ErrorColor; }
        }

        [SerializeField]
        private Color32 m_FatalColor = new Color(0.7f, 0.2f, 0.2f);
        public Color32 FatalColor
        {
            get { return m_FatalColor; }
        }

        /// <summary>
        /// 悬浮Icon内嵌按钮文字字号
        /// </summary>
        private int m_IconInnerButtonFontSize = 30;

        /// <summary>
        /// 悬浮Icon内嵌按钮宽度
        /// </summary>
        private float m_IconInnerButtonWidth = 440f;

        /// <summary>
        /// 悬浮Icon内嵌按钮高度
        /// </summary>
        private float m_IconInnerButtonHeight = 300f;

        /// <summary>
        /// 弹窗内按钮高度
        /// </summary>
        private int m_CustomButtonHeight = 80;

        /// <summary>
        /// 弹窗内按钮文字大小
        /// </summary>
        private int m_CustomButtonFontSize = 28;

        /// <summary>
        /// 弹窗内滚动栏宽度或高度
        /// </summary>
        private int m_CustomScrollBarValue = 50;

        /// <summary>
        /// Mini悬浮Icon可拖拽矩形大小（相对）
        /// </summary>
        private Rect m_MiniDragIconRect = new Rect(0f, 0f, float.MaxValue, 45f);

        /// <summary>
        /// 悬浮Icon可拖拽矩形大小（相对）
        /// </summary>
        private Rect m_DragIconRect = new Rect(0f, 0f, float.MaxValue, 52f);

        /// <summary>
        /// 弹窗可拖拽矩形大小（相对）
        /// </summary>
        private Rect m_FullWindowDragRect = new Rect(0f, 0f, float.MaxValue, 60f);

        /// <summary>
        /// 悬浮Icon可视矩形大小
        /// </summary>
        private Rect m_IconRect = new Rect(0f, 0f, 200, 140);

        /// <summary>
        /// 弹窗可视矩形大小
        /// </summary>
        private Rect m_WindowRect = Rect.zero;

        /// <summary>
        /// 窗口大小
        /// </summary>
        private Vector2 m_UICanvasSize;
        
        /// <summary>
        /// 弹窗可视矩形大小缩放比例
        /// </summary>
        private float m_WindowScale = 2f;

        private string m_winMiniTitle = "<b><color=#FFFFFFFF>HONOR</color></b>";
        private string m_winMiniStyle = "window";

        /// <summary>
        /// 悬浮Icon状态
        /// </summary>
        private GameDefinitions.DebugWindowModel m_WinModel = GameDefinitions.DebugWindowModel.MiniWindow;
        public GameDefinitions.DebugWindowModel WinModel
        {
            set { m_WinModel = value; }
            get { return m_WinModel; }
        }

        /// <summary>
        /// 显示选中log详细信息的弹窗rect
        /// </summary>
        private Rect m_LogPopWindow = Rect.zero;
        /// <summary>
        /// 显示选中log详细信息的滚动框显示区域
        /// </summary>
        private Vector2 m_StackScrollPosition = Vector2.zero;
        /// <summary>
        /// 复制选中log的TextEditor
        /// </summary>
        private readonly TextEditor m_TextEditor = new TextEditor();
        /// <summary>
        /// 选中要弹窗显示的log
        /// </summary>
        private string m_SelLogMsg = "";
        public string SelLogString
        {
            set { m_SelLogMsg = value; }
            get { return m_SelLogMsg; }
        }
        /// <summary>
        /// 选中要弹窗显示的堆栈信息
        /// </summary>
        private string m_SelLogStackTrack = "";
        public string SelLogStackTrack
        {
            set { m_SelLogStackTrack = value; }
            get { return m_SelLogStackTrack; }
        }
        /// <summary>
        /// 非全屏模式下是否正在进行拖拽
        /// </summary>
        private bool m_WindowInDrag = false;
        public bool WindowInDrag
        {
            get { return m_WindowInDrag; }
        }
        //全屏模式下是否正在进行拖拽
        private bool m_FullWindowInDrag = false;
        public bool FullWindowInDrag
        {
            get { return m_FullWindowInDrag; }
        }
        private float m_SubWindowDragY = 0;
        /// <summary>
        /// 
        /// </summary>
        private Vector2 m_FullWindowDownPos = Vector2.zero;
        public Vector2 FullWindowDownPos
        {
            get { return m_FullWindowDownPos; }
        }

        private bool m_WindowInTouchDown = false;
        private bool m_FullWindowInTouchDown = false;

        /// <summary>
        /// 触摸屏蔽层
        /// </summary>
        private RectTransform m_BlockRectTransform = null;

        /// <summary>
        /// 是否吸附在屏幕边缘
        /// </summary>
        [SerializeField]
        private bool m_StayBorder = false;
        public bool StayBorder
        {
            set { StayBorder = value; }
            get { return StayBorder; }
        }

        /// <summary>
        /// 是否处于空闲状态
        /// </summary>
        [SerializeField]
        private bool m_IdleState = false;
        public bool IdleState
        {
            set { m_IdleState = value; }
            get { return m_IdleState; }
        }

        /// <summary>
        /// 停止操作debug窗口多长时间后进入空闲状态
        /// </summary>
        [SerializeField]
        private int m_EnterIdleTime = 30;

        private long m_IdleStartSecond = 0;

        /// <summary>
        /// 悬浮目标位置
        /// </summary>
        private Vector2 m_TargetPos;

        /// <summary>
        /// 悬浮目标位置（默认）
        /// </summary>
        private Vector2 m_DefaultTargetPos = new Vector2(-999999, -999999);

        /// <summary>
        /// 悬浮目标位置（跳过）
        /// </summary>
        private Vector2 m_SkipTargetPos = new Vector2(-9999999, -9999990);


        [SerializeField]
        private GUISkin m_Skin = null;

        /// <summary>
        /// Mini状态下窗口皮肤
        /// </summary>
        [SerializeField]
        private GUISkin m_MiniSkin = null;

        /// <summary>
        /// 弹出窗口使用的皮肤配置
        /// </summary>
        [SerializeField]
        private GUISkin m_PopWindowSkin = null;

        [SerializeField]
        private DebuggerActiveWindowType m_ActiveWindow = DebuggerActiveWindowType.AlwaysOpen;

        [SerializeField]
        private bool m_ShowFullWindow = false;

        [SerializeField]
        private ConsoleWindow m_ConsoleWindow = new ConsoleWindow();

        private SystemInformationWindow m_SystemInformationWindow = new SystemInformationWindow();
        private EnvironmentInformationWindow m_EnvironmentInformationWindow = new EnvironmentInformationWindow();
        private ScreenInformationWindow m_ScreenInformationWindow = new ScreenInformationWindow();
        private GraphicsInformationWindow m_GraphicsInformationWindow = new GraphicsInformationWindow();
        private InputSummaryInformationWindow m_InputSummaryInformationWindow = new InputSummaryInformationWindow();
        private InputTouchInformationWindow m_InputTouchInformationWindow = new InputTouchInformationWindow();
        private InputAccelerationInformationWindow m_InputAccelerationInformationWindow = new InputAccelerationInformationWindow();
        private InputGyroscopeInformationWindow m_InputGyroscopeInformationWindow = new InputGyroscopeInformationWindow();
        private InputCompassInformationWindow m_InputCompassInformationWindow = new InputCompassInformationWindow();
        private PathInformationWindow m_PathInformationWindow = new PathInformationWindow();
        private SceneInformationWindow m_SceneInformationWindow = new SceneInformationWindow();
        private TimeInformationWindow m_TimeInformationWindow = new TimeInformationWindow();
        private QualityInformationWindow m_QualityInformationWindow = new QualityInformationWindow();
        private ProfilerInformationWindow m_ProfilerInformationWindow = new ProfilerInformationWindow();
        private WebPlayerInformationWindow m_WebPlayerInformationWindow = new WebPlayerInformationWindow();
        private RuntimeMemorySummaryWindow m_RuntimeMemorySummaryWindow = new RuntimeMemorySummaryWindow();
        private RuntimeMemoryInformationWindow<UnityEngine.Object> m_RuntimeMemoryAllInformationWindow = new RuntimeMemoryInformationWindow<UnityEngine.Object>();
        private RuntimeMemoryInformationWindow<Texture> m_RuntimeMemoryTextureInformationWindow = new RuntimeMemoryInformationWindow<Texture>();
        private RuntimeMemoryInformationWindow<Mesh> m_RuntimeMemoryMeshInformationWindow = new RuntimeMemoryInformationWindow<Mesh>();
        private RuntimeMemoryInformationWindow<Material> m_RuntimeMemoryMaterialInformationWindow = new RuntimeMemoryInformationWindow<Material>();
        private RuntimeMemoryInformationWindow<Shader> m_RuntimeMemoryShaderInformationWindow = new RuntimeMemoryInformationWindow<Shader>();
        private RuntimeMemoryInformationWindow<AnimationClip> m_RuntimeMemoryAnimationClipInformationWindow = new RuntimeMemoryInformationWindow<AnimationClip>();
        private RuntimeMemoryInformationWindow<AudioClip> m_RuntimeMemoryAudioClipInformationWindow = new RuntimeMemoryInformationWindow<AudioClip>();
        private RuntimeMemoryInformationWindow<Font> m_RuntimeMemoryFontInformationWindow = new RuntimeMemoryInformationWindow<Font>();
        private RuntimeMemoryInformationWindow<TextAsset> m_RuntimeMemoryTextAssetInformationWindow = new RuntimeMemoryInformationWindow<TextAsset>();
        private RuntimeMemoryInformationWindow<ScriptableObject> m_RuntimeMemoryScriptableObjectInformationWindow = new RuntimeMemoryInformationWindow<ScriptableObject>();

        private FpsCounter m_FpsCounter = null;
        private RamCounter m_RamCounter = null;

        /// <summary>
        /// 获取或设置调试器窗口是否激活。
        /// </summary>
        public bool ActiveWindow
        {
            get
            {
                return m_DebuggerManager.ActiveWindow;
            }
            set
            {
                m_DebuggerManager.ActiveWindow = value;
            }
        }

        /// <summary>
        /// 获取或设置是否显示完整调试器界面。
        /// </summary>
        public bool ShowFullWindow
        {
            get
            {
                return m_ShowFullWindow;
            }
            set
            {
                m_ShowFullWindow = value;
            }
        }

        /// <summary>
        /// 获取或设置调试器漂浮框大小。
        /// </summary>
        public Rect IconRect
        {
            get
            {
                return m_IconRect;
            }
            set
            {
                m_IconRect = value;
            }
        }

        /// <summary>
        /// 获取或设置调试器窗口大小。
        /// </summary>
        public Rect WindowRect
        {
            get
            {
                return m_WindowRect;
            }
            set
            {
                m_WindowRect = value;
            }
        }

        /// <summary>
        /// 获取或设置调试器窗口缩放比例。
        /// </summary>
        public float WindowScale
        {
            get
            {
                return m_WindowScale;
            }
            set
            {
                m_WindowScale = value;
            }
        }

        /// <summary>
        /// 获取或设置所有按钮高度
        /// </summary>
        public int CustomButtonHeight
        {
            get
            {
                return m_CustomButtonHeight;
            }
            set
            {
                m_CustomButtonHeight = value;
            }

        }

        /// <summary>
        /// 获取或设置所有按钮上面文字大小
        /// </summary>
        public int CustomButtonFontSize
        {
            get => m_CustomButtonFontSize;
            set => m_CustomButtonFontSize = value;
        }

        /// <summary>
        /// 设置或者获取滚动框内可以拖拽的滚动条的宽度或者高度
        /// 横向滚动条修改高度
        /// 竖向滚动条修改宽度
        /// </summary>
        public int CustomScrollBarValue
        {
            get => m_CustomScrollBarValue;
            set => m_CustomScrollBarValue = value;
        }

        /// <summary>
        /// 当前帧率
        /// </summary>
        public string CurrentFPS
        {
            get
            {
                if (m_FpsCounter != null)
                {
                    return m_FpsCounter.CurrentFps.ToString("F2");
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// 弹窗最大宽度
        /// </summary>
        private float m_FullWinMaxWidth = 0;
        public float FullWinMaxWidth 
        { 
            get => m_FullWinMaxWidth; 
            set => m_FullWinMaxWidth = value; 
        }

        /// <summary>
        /// 全屏显示时UI宽度边距
        /// </summary>
        private float m_FullWinUIBorder = 20;
        public float FullWinUIBorder
        {
            get => m_FullWinUIBorder;
            set => m_FullWinUIBorder = value;
        }

        /// <summary>
        /// 弹窗最大高度
        /// </summary>
        private float m_FullWinMaxHeight = 0;
        public float FullWinMaxHeight 
        { 
            get => m_FullWinMaxHeight; 
            set => m_FullWinMaxHeight = value; 
        }

        /// <summary>
        /// 全屏窗口下，标题栏的高度
        /// </summary>
        private float m_FullWinTitleHeight = -1;
        public float FullWinTitleHeight
        {
            get => m_FullWinTitleHeight;
        }

        /// <summary>
        /// Debug窗口显示Log信息区域
        /// </summary>
        private Rect m_LogScrollRect = Rect.zero;
        public Rect LogScrollRect
        {
            set => m_LogScrollRect = value;
            get => m_LogScrollRect;
        }

        /// <summary>
        /// Debug窗口Log信息滚动条
        /// </summary>
        private Rect m_LogScrollBarRect = Rect.zero;
        public Rect LogScrollBarRect
        {
            set => m_LogScrollBarRect = value;
            get => m_LogScrollBarRect;
        }

        /// <summary>
        /// Debug窗口Log信息顶端坐标
        /// </summary>
        private float m_LogScrollBarTop = 0;
        public float LogScrollBarTop
        {
            set => m_LogScrollBarTop = value;
            get => m_LogScrollBarTop;
        }

        /// <summary>
        /// Debug窗口Log信息底端坐标
        /// </summary>
        private float m_LogScrollBarBottom = 0;
        public float LogScrollBarBottom
        {
            set => m_LogScrollBarBottom = value;
            get => m_LogScrollBarBottom;
        }

        private float m_LogScrollBarHeight = 0;
        public float LogScrollBarHeight
        {
            set => m_LogScrollBarHeight = value;
            get => m_LogScrollBarHeight;
        }

        /// <summary>
        /// 当前是否是竖屏模式
        /// </summary>
        private bool m_Portrail = false;
        public bool Portrail
        {
            set => m_Portrail = value;
            get => m_Portrail;
        }

        /// <summary>
        /// 是否使用滚动惯性
        /// </summary>
        private bool m_Inertia = true;
        public bool Inertia
        {
            set => m_Inertia = value;
            get => m_Inertia;
        }

        /// <summary>
        /// 滚动速率
        /// </summary>
        private float m_DecelerationRate = 0.135f; 
        /// <summary>
        /// 滚动速度
        /// </summary>
        private Vector2 m_Velocity = Vector2.zero;
        public Vector2 Velocity
        {
            set => m_Velocity = value;
            get => m_Velocity;
        }

        /// <summary>
        /// 上一次滚动偏移量
        /// </summary>
        private float m_LastScrollOffsetY = 0;
        public float LastScrollOffsetY
        {
            set 
            {
                if (m_FullWindowInDrag)
                {
                    m_LastScrollOffsetY = value;
                }
            }
            get => m_LastScrollOffsetY;
        }


    }
}


