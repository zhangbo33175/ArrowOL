using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Honor.Runtime
{
    public sealed partial class UIComponent : GameComponent
    {
        /// <summary>
        /// UI 设计基准分辨率（屏幕UI适配用）
        /// </summary>
        [SerializeField]
        private Vector2 m_ScreenDesignedResolution = new Vector2(1366, 768);
        public Vector2 ScreenDesignedResolution
        {
            get => m_ScreenDesignedResolution;
        }

        /// <summary>
        /// 屏幕宽高比适配阈值（Canvas 匹配模式用）
        /// </summary>
        [SerializeField]
        private float m_ScreenWidthHeightMatchValue = 1f;
        public float ScreenWidthHeightMatchValue
        {
            set
            {
                m_ScreenWidthHeightMatchValue = value;
                m_UIManager.RefreshScreenMatchValue(m_ScreenWidthHeightMatchValue);
            }
            get => m_ScreenWidthHeightMatchValue;
        }

        /// <summary>
        /// 每帧最大销毁UI数量（防止卡顿）
        /// </summary>
        [SerializeField]
        private int m_DestroyMaxNumPerFrame = 1;
        public int DestroyMaxNumPerFrame
        {
            set
            {
                m_DestroyMaxNumPerFrame = value;
                m_UIManager.DestroyMaxNumPerFrame = m_DestroyMaxNumPerFrame;
            }
            get => m_DestroyMaxNumPerFrame;
        }

        /// <summary>
        /// 等待菊花UI的AB包路径
        /// </summary>
        [SerializeField]
        private string m_WaitingUIABPath;

        /// <summary>
        /// 等待菊花UI的资源名称
        /// </summary>
        [SerializeField]
        private string m_WaitingUIAssetName;

        /// <summary>
        /// 飘字UI的AB包路径
        /// </summary>
        [SerializeField]
        private string m_FloatWordsUIABPath;

        /// <summary>
        /// 飘字UI的资源名称
        /// </summary>
        [SerializeField]
        private string m_FloatWordsUIAssetName;

        /// <summary>
        /// 飘字默认显示时长
        /// </summary>
        [SerializeField]
        private float m_FloatWordsDuration;
        public float FloatWordsDuration
        {
            set
            {
                m_FloatWordsDuration = value;
                m_UIManager.FloatWordsDuration = m_FloatWordsDuration;
            }
            get => m_FloatWordsDuration;
        }

        /// <summary>
        /// 按钮防重复点击有效间隔
        /// </summary>
        [SerializeField]
        private float m_ButtonInteractDuration;
        public float ButtonInteractDuration
        {
            set => m_ButtonInteractDuration = value;
            get => m_ButtonInteractDuration;
        }

        /// <summary>
        /// 屏幕层UI相机列表
        /// </summary>
        [SerializeField]
        private List<Camera> m_ScreenUICameras;
        public List<Camera> ScreenUICameras
        {
            set => m_ScreenUICameras = value;
            get => m_ScreenUICameras;
        }

        /// <summary>
        /// 场景层UI相机列表
        /// </summary>
        [SerializeField]
        private List<Camera> m_SceneUICameras;
        public List<Camera> SceneUICameras
        {
            get => m_SceneUICameras;
        }

        /// <summary>
        /// 屏幕层UI根Canvas
        /// </summary>
        [SerializeField]
        private Canvas m_ScreenUICanvas;
        public Canvas ScreenUICanvas
        {
            set => m_ScreenUICanvas = value;
            get => m_ScreenUICanvas;
        }

        /// <summary>
        /// 场景层UI根Canvas
        /// </summary>
        [SerializeField]
        private Canvas m_SceneUICanvas;
        public Canvas SceneUICanvas
        {
            set => m_SceneUICanvas = value;
            get => m_SceneUICanvas;
        }

        /// <summary>
        /// 是否开启屏幕方向变化检测
        /// </summary>
        [SerializeField]
        private bool m_CheckOrientationState;
        public bool CheckOrientationState
        {
            set => m_CheckOrientationState = value;
            get => m_CheckOrientationState;
        }

        /// <summary>
        /// 是否开启TextLocalizing脚本检测（多语言文本）
        /// </summary>
        [SerializeField]
        private bool m_CheckTextLocalizings;
        public bool CheckTextLocalizings
        {
            get => m_CheckTextLocalizings;
            set => m_CheckTextLocalizings = value;
        }

        /// <summary>
        /// UI刘海屏安全区域尺寸
        /// </summary>
        public Vector2 UIBangsSize => m_UIManager.UIBangsSize;

        /// <summary>
        /// 全局阻塞所有模态UI开关
        /// </summary>
        public bool BlockModalUIsSwitch
        {
            set => m_UIManager.BlockModalUIsSwitch = value;
            get => m_UIManager.BlockModalUIsSwitch;
        }

        /// <summary>
        /// 屏幕层 - 等待菊花UI实例（常驻）
        /// </summary>
        public UIConnectionWaitingView ConnectionWaitingUIConnection => m_UIManager.ConnectionWaitingUIConnection;

        /// <summary>
        /// 等待菊花UI引用计数
        /// </summary>
        public int WaitingUIRefCount => m_UIManager.WaitingUIRefCount;

        /// <summary>
        /// 屏幕层 - 流程切换过渡UI实例（常驻）
        /// </summary>
        public UILauncherLogoView ProcedureTransitionUI => m_UIManager.TransitionUI;

        /// <summary>
        /// 待卸载UI列表
        /// </summary>
        public List<UIFlagBehaviour> UnloadUIList => m_UIManager.UnloadUIList;

        /// <summary>
        /// 屏幕层 - 当前显示的模态UI
        /// </summary>
        public UIFlagBehaviour CurModalUI => m_UIManager.CurModalUI;

        /// <summary>
        /// 屏幕层 - 非模态UI列表
        /// </summary>
        public List<UIFlagBehaviour> UnModalUIList => m_UIManager.UnModalUIList;

        /// <summary>
        /// 场景层 - 场景UI列表
        /// </summary>
        public List<UIFlagBehaviour> SceneUIList => m_UIManager.SceneUIList;

        /// <summary>
        /// 子/附加UI实例字典（按UI类型分类）
        /// </summary>
        public Dictionary<UIType, List<UIFlagBehaviour>> SubUIList => m_UIManager.SubUIList;

        /// <summary>
        /// 屏幕层 - 模态UI等待队列
        /// </summary>
        public List<UIInfo> ModalUIInfoList => m_UIManager.ModalUIInfoList;

        /// <summary>
        /// 资源管理组件
        /// </summary>
        private AssetComponent m_AssetComponent;

        /// <summary>
        /// 多语言管理组件
        /// </summary>
        private LocalizationComponent m_LocalizationComponent;

        /// <summary>
        /// 配置管理组件
        /// </summary>
        private ConfigComponent m_ConfigComponent;

        /// <summary>
        /// UI核心管理器
        /// </summary>
        private UIManager m_UIManager;

        /// <summary>
        /// LuaTable 转 UIInfo 专用缓存对象
        /// </summary>
        private JObject m_CachedJsonObject;
    }
}