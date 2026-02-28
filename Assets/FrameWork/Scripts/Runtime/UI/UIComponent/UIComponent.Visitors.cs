using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Honor.Runtime
{
    public partial class UIComponent
    {
        /// <summary>
        /// 屏幕UI下Canvas画布
        /// </summary>
        private Canvas m_ScreenUICanvas;

        /// <summary>
        /// 屏幕UI下Canvas画布定标器
        /// </summary>
        private CanvasScaler m_ScreenUICanvasScaler;

        /// <summary>
        /// 屏幕UI下Canvas画布绘制投射器
        /// </summary>
        private GraphicRaycaster m_ScreenUIGraphicRaycaster;

        /// <summary>
        /// 屏幕UI下Canvas画布合体组件
        /// </summary>
        private CanvasGroup m_ScreenUICanvasGroup;

        /// <summary>
        /// 屏幕UI相机列表
        /// </summary>
        private List<Camera> m_ScreenUICameras;

        /// <summary>
        /// 场景UI下Canvas画布
        /// </summary>
        private Canvas m_SceneUICanvas;

        /// <summary>
        /// WebUI下Canvas画布（WebGL专用）
        /// </summary>
        private static Canvas m_WebUICanvas;

        public static Canvas WebUICanvas
        {
            get => m_WebUICanvas;
            set => m_WebUICanvas = value;
        }

        /// <summary>
        /// 场景UI下Canvas画布定标器
        /// </summary>
        private CanvasScaler m_SceneUICanvasScaler;

        /// <summary>
        /// 场景UI下Canvas画布绘制投射器
        /// </summary>
        private GraphicRaycaster m_SceneUIGraphicRaycaster;

        /// <summary>
        /// 场景UI下Canvas画布合体组件
        /// </summary>
        private CanvasGroup m_SceneUICanvasGroup;

        /// <summary>
        /// 场景UI相机列表
        /// </summary>
        private List<Camera> m_SceneUICameras;

        /// <summary>
        /// UI字体集合
        /// 类型：Font 或 TMP_FontAsset
        /// </summary>
        private List<Object> m_Fonts;

        public List<Object> Fonts
        {
            set { m_Fonts = value; }
            get { return m_Fonts; }
        }

        /// <summary>
        /// 上一次UI字体集合
        /// 类型：Font 或 TMP_FontAsset
        /// </summary>
        private List<Object> m_LastFonts;

        public List<Object> LastFonts
        {
            set { m_LastFonts = value; }
            get { return m_LastFonts; }
        }

        /// <summary>
        /// 阻塞所有模态UI的开关
        /// </summary>
        private bool m_BlockModalUIsSwitch;

        public bool BlockModalUIsSwitch
        {
            set { m_BlockModalUIsSwitch = value; }
            get { return m_BlockModalUIsSwitch; }
        }

        /// <summary>
        /// 阻塞所有UI的按键抬起事件响应
        /// </summary>
        private bool m_BlockAllUIsKeyUpSwitch;

        public bool BlockAllUIsKeyUpSwitch
        {
            set { m_BlockAllUIsKeyUpSwitch = value; }
            get { return m_BlockAllUIsKeyUpSwitch; }
        }

        /// <summary>
        /// 常驻内存的UI集合
        /// </summary>
        private List<GameObject> m_PermanentUIs;

        /// <summary>
        /// [屏幕UI] 菊花等待UI（常驻内存）
        /// </summary>
        private UIWaitingBehaviour m_WaitingUI;

        public UIWaitingBehaviour WaitingUI
        {
            get { return m_WaitingUI; }
        }

        /// <summary>
        /// [屏幕UI] 菊花等待UI引用计数
        /// </summary>
        private int m_WaitingUIRefCount;

        public int WaitingUIRefCount
        {
            set { m_WaitingUIRefCount = value; }
            get { return m_WaitingUIRefCount; }
        }

        /// <summary>
        /// [屏幕UI] 流程切换过渡UI（常驻内存）
        /// </summary>
        private UITransitionBehaviour m_TransitionUI;

        public UITransitionBehaviour TransitionUI
        {
            get { return m_TransitionUI; }
        }

        /// <summary>
        /// [屏幕UI] 当前模态UI实例
        /// </summary>
        private UIFlagBehaviour m_CurModalUI;

        public UIFlagBehaviour CurModalUI
        {
            get { return m_CurModalUI; }
        }

        /// <summary>
        /// [屏幕UI] 模态UI信息队列
        /// </summary>
        private readonly List<UIInfo> m_ModalUIInfoList;

        public List<UIInfo> ModalUIInfoList
        {
            get { return m_ModalUIInfoList; }
        }

        /// <summary>
        /// [屏幕UI] 非模态UI实例列表
        /// </summary>
        private readonly List<UIFlagBehaviour> m_UnModalUIList;

        public List<UIFlagBehaviour> UnModalUIList
        {
            get { return m_UnModalUIList; }
        }

        /// <summary>
        /// [场景UI] 场景UI实例列表
        /// </summary>
        private readonly List<UIFlagBehaviour> m_SceneUIList;

        public List<UIFlagBehaviour> SceneUIList
        {
            get { return m_SceneUIList; }
        }

        /// <summary>
        /// 附加UI实例列表
        /// <UI类型, UI列表>
        /// </summary>
        private readonly Dictionary<UIType, List<UIFlagBehaviour>> m_SubUIList;

        public Dictionary<UIType, List<UIFlagBehaviour>> SubUIList
        {
            get { return m_SubUIList; }
        }

        /// <summary>
        /// UI卸载列表
        /// </summary>
        private readonly List<UIFlagBehaviour> m_UnloadUIList;

        public List<UIFlagBehaviour> UnloadUIList
        {
            get { return m_UnloadUIList; }
        }

        /// <summary>
        /// 每帧最多销毁ui数量
        /// </summary>
        private int m_DestroyMaxNumPerFrame = 1;

        public int DestroyMaxNumPerFrame
        {
            set { m_DestroyMaxNumPerFrame = value; }
            get { return m_DestroyMaxNumPerFrame; }
        }

        /// <summary>
        /// 是否启用TextLocalizing脚本检测
        /// </summary>
        private bool m_CheckTextLocalizings;

        public bool CheckTextLocalizings
        {
            get { return m_CheckTextLocalizings; }
        }

        /// <summary>
        /// 菊花等待界面AB路径
        /// </summary>
        private string m_WaitingUIABPath;

        /// <summary>
        /// 菊花等待界面Asset资源名称
        /// </summary>
        private string m_WaitingUIAssetName;

        /// <summary>
        /// 飘字界面AB路径
        /// </summary>
        private string m_FloatWordsUIABPath;

        /// <summary>
        /// 飘字界面Asset资源名称
        /// </summary>
        private string m_FloatWordsUIAssetName;

        /// <summary>
        /// 飘字界面持续时间
        /// </summary>
        private float m_FloatWordsDuration;

        public float FloatWordsDuration
        {
            set { m_FloatWordsDuration = value; }
            get { return m_FloatWordsDuration; }
        }

        /// <summary>
        /// Canvas下的屏幕刘海尺寸，计算依据是Screen.safeArea
        /// </summary>
        private Vector2 m_UIBangsSize = new Vector2(-1, -1);

        public Vector2 UIBangsSize
        {
            set { m_UIBangsSize = value; }
            get { return m_UIBangsSize; }
        }

        /// <summary>
        /// 屏幕方向
        /// </summary>
        private ScreenOrientation m_ScreenOrientation = ScreenOrientation.Unknown;

        public ScreenOrientation ScreenOrientation
        {
            set { m_ScreenOrientation = value; }
            get { return m_ScreenOrientation; }
        }
    }
}