using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Honor.Runtime
{
    public sealed partial class UIComponent : GameComponent
    {
        /// <summary>
        /// 屏幕设计分辨率
        /// </summary>
        [SerializeField]
        private Vector2 m_ScreenDesignedResolution = new Vector2(1366, 768);
        public Vector2 ScreenDesignedResolution
        {
            get
            {
                return m_ScreenDesignedResolution;
            }
        }

        /// <summary>
        /// 屏幕宽高适比例配阀值
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
            get
            {
                return m_ScreenWidthHeightMatchValue;
            }
        }

        /// <summary>
        /// 每帧最多销毁ui数量
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
            get
            {
                return m_DestroyMaxNumPerFrame;
            }
        }

        /// <summary>
        /// 菊花等待界面AB路径
        /// </summary>
        [SerializeField]
        private string m_WaitingUIABPath;

        /// <summary>
        /// 菊花等待界面Asset资源名称
        /// </summary>
        [SerializeField]
        private string m_WaitingUIAssetName;

        /// <summary>
        /// 飘字界面AB路径
        /// </summary>
        [SerializeField]
        private string m_FloatWordsUIABPath;

        /// <summary>
        /// 飘字界面Asset资源名称
        /// </summary>
        [SerializeField]
        private string m_FloatWordsUIAssetName;

        /// <summary>
        /// 飘字界面持续时间
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
            get
            {
                return m_FloatWordsDuration;
            }
        }

        /// <summary>
        /// 按钮点击有效间隔
        /// </summary>
        [SerializeField]
        private float m_ButtonInteractDuration;
        public float ButtonInteractDuration
        {
            set
            {
                m_ButtonInteractDuration = value;
            }
            get
            {
                return m_ButtonInteractDuration;
            }
        }

        /// <summary>
        /// 屏幕UI相机列表
        /// </summary>
        [SerializeField]
        private List<Camera> m_ScreenUICameras;
        public List<Camera> ScreenUICameras
        {
            set
            {
                m_ScreenUICameras = value;
            }
            get
            {
                return m_ScreenUICameras;
            }
        }

        /// <summary>
        /// 场景UI相机列表
        /// </summary>
        [SerializeField]
        private List<Camera> m_SceneUICameras;
        public List<Camera> SceneUICameras
        {
            get
            {
                return m_SceneUICameras;
            }
        }

        /// <summary>
        /// 屏幕UI画布Canvas
        /// </summary>
        [SerializeField]
        private Canvas m_ScreenUICanvas;
        public Canvas ScreenUICanvas
        {
            set
            {
                m_ScreenUICanvas = value;
            }
            get
            {
                return m_ScreenUICanvas;
            }
        }

        /// <summary>
        /// 场景UI画布Canvas
        /// </summary>
        [SerializeField]
        private Canvas m_SceneUICanvas;
        public Canvas SceneUICanvas
        {
            set
            {
                m_SceneUICanvas = value;
            }
            get
            {
                return m_SceneUICanvas;
            }
        }

        /// <summary>
        /// 是否启用屏幕方向变动检测
        /// </summary>
        [SerializeField]
        private bool m_CheckOrientationState;
        public bool CheckOrientationState
        {
            set
            {
                m_CheckOrientationState = value;
            }
            get
            {
                return m_CheckOrientationState;
            }
        }

        /// <summary>
        /// 是否启用TextLocalizing脚本检测
        /// </summary>
        [SerializeField]
        private bool m_CheckTextLocalizings;
        public bool CheckTextLocalizings
        {
            get
            {
                return m_CheckTextLocalizings;
            }
            set
            {
                m_CheckTextLocalizings = value;
            }
        }
		
        /// <summary>
        /// 获取刘海尺寸
        /// </summary>
        public Vector2 UIBangsSize
        {
            get
            {
                return m_UIManager.UIBangsSize;
            }
        }

        /// <summary>
        /// 阻塞所有模态UI的开关
        /// </summary>
        public bool BlockModalUIsSwitch
        {
            set
            {
                m_UIManager.BlockModalUIsSwitch = value;
            }
            get
            {
                return m_UIManager.BlockModalUIsSwitch;
            }
        }

        /// <summary>
        /// [屏幕UI] 菊花等待UI（常驻内存）
        /// </summary>
        public UIConnectionWaitingView ConnectionWaitingUIConnection
        {
            get
            {
                return m_UIManager.ConnectionWaitingUIConnection;
            }
        }

        /// <summary>
        /// [屏幕UI] 菊花等待UI引用计数
        /// </summary>
        public int WaitingUIRefCount
        {
            get
            {
                return m_UIManager.WaitingUIRefCount;
            }
        }

        /// <summary>
        /// [屏幕UI] 流程切换过渡UI（常驻内存）
        /// </summary>
        public UILauncherLogoView ProcedureTransitionUI
        {
            get
            {
                return m_UIManager.TransitionUI;
            }
        }

        /// <summary>
        /// UI卸载列表
        /// </summary>
        public List<UIFlagBehaviour> UnloadUIList
        {
            get
            {
                return m_UIManager.UnloadUIList;
            }
        }

        /// <summary>
        /// [屏幕UI] 当前模态UI实例
        /// </summary>
        public UIFlagBehaviour CurModalUI
        {
            get
            {
                return m_UIManager.CurModalUI;
            }
        }

        /// <summary>
        /// [屏幕UI] 非模态UI实例列表
        /// </summary>
        public List<UIFlagBehaviour> UnModalUIList
        {
            get
            {
                return m_UIManager.UnModalUIList;
            }
        }

        /// <summary>
        /// [场景UI] 场景UI实例列表
        /// </summary>
        public List<UIFlagBehaviour> SceneUIList
        {
            get
            {
                return m_UIManager.SceneUIList;
            }
        }

        /// <summary>
        /// 附加UI实例列表
        /// <UI类型, UI列表>
        /// </summary>
        public Dictionary<UIType, List<UIFlagBehaviour>> SubUIList
        {
            get
            {
                return m_UIManager.SubUIList;
            }
        }

        /// <summary>
        /// [屏幕UI] 模态UI信息队列
        /// </summary>
        public List<UIInfo> ModalUIInfoList
        {
            get
            {
                return m_UIManager.ModalUIInfoList;
            }
        }

        /// <summary>
        /// Asset组件
        /// </summary>
        private AssetComponent m_AssetComponent;

        /// <summary>
        /// Localization组件
        /// </summary>
        private LocalizationComponent m_LocalizationComponent;

        /// <summary>
        /// Config组件
        /// </summary>
        private ConfigComponent m_ConfigComponent;

        /// <summary>
        /// UI管理器
        /// </summary>
        private UIManager m_UIManager;

        /// <summary>
        /// 缓冲JObject数据
        /// </summary>
        private JObject m_CachedJsonObject;
    }

}


