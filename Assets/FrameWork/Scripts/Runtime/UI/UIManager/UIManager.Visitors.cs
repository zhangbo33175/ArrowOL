using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Honor.Runtime
{
    public sealed partial class UIManager
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
                m_UIComponent.RefreshScreenMatchValue(m_ScreenWidthHeightMatchValue);
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
                m_UIComponent.DestroyMaxNumPerFrame = m_DestroyMaxNumPerFrame;
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
                m_UIComponent.FloatWordsDuration = m_FloatWordsDuration;
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
        /// WebUI画布Canvas（WebGL专用）
        /// </summary>
        [SerializeField]
        private Canvas m_WebUICanvas;
        public Canvas WebUICanvas
        {
            set
            {
                m_WebUICanvas = value;
            }
            get
            {
                return m_WebUICanvas;
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
                return m_UIComponent.UIBangsSize;
            }
        }

        /// <summary>
        /// 阻塞所有模态UI的开关
        /// </summary>
        public bool BlockModalUIsSwitch
        {
            set
            {
                m_UIComponent.BlockModalUIsSwitch = value;
            }
            get
            {
                return m_UIComponent.BlockModalUIsSwitch;
            }
        }

        /// <summary>
        /// [屏幕UI] 菊花等待UI（常驻内存）
        /// </summary>
        public UIWaitingBehaviour WaitingUI
        {
            get
            {
                return m_UIComponent.WaitingUI;
            }
        }

        /// <summary>
        /// [屏幕UI] 菊花等待UI引用计数
        /// </summary>
        public int WaitingUIRefCount
        {
            get
            {
                return m_UIComponent.WaitingUIRefCount;
            }
        }

        /// <summary>
        /// [屏幕UI] 流程切换过渡UI（常驻内存）
        /// </summary>
        public UITransitionBehaviour ProcedureTransitionUI
        {
            get
            {
                return m_UIComponent.TransitionUI;
            }
        }

        /// <summary>
        /// UI卸载列表
        /// </summary>
        public List<UIFlagBehaviour> UnloadUIList
        {
            get
            {
                return m_UIComponent.UnloadUIList;
            }
        }

        /// <summary>
        /// [屏幕UI] 当前模态UI实例
        /// </summary>
        public UIFlagBehaviour CurModalUI
        {
            get
            {
                return m_UIComponent.CurModalUI;
            }
        }

        /// <summary>
        /// [屏幕UI] 非模态UI实例列表
        /// </summary>
        public List<UIFlagBehaviour> UnModalUIList
        {
            get
            {
                return m_UIComponent.UnModalUIList;
            }
        }

        /// <summary>
        /// [场景UI] 场景UI实例列表
        /// </summary>
        public List<UIFlagBehaviour> SceneUIList
        {
            get
            {
                return m_UIComponent.SceneUIList;
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
                return m_UIComponent.SubUIList;
            }
        }

        /// <summary>
        /// [屏幕UI] 模态UI信息队列
        /// </summary>
        public List<UIInfo> ModalUIInfoList
        {
            get
            {
                return m_UIComponent.ModalUIInfoList;
            }
        }
        
        /// <summary>
        /// UI管理器
        /// </summary>
        private UIComponent m_UIComponent;
        /// <summary>
        /// 缓冲JObject数据
        /// </summary>
        private JObject m_CachedJsonObject;
    }
}