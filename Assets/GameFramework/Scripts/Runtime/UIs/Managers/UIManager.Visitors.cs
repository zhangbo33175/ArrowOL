using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Honor.Runtime
{
    /// <summary>
    /// UI 管理器（核心单例类）
    /// 负责全局 UI 的创建、显示、隐藏、销毁、层级管理、多语言适配、屏幕适配等核心逻辑
    /// </summary>
    public sealed partial class UIManager
    {
        /// <summary>
        /// 资源管理组件
        /// 用于加载/卸载 UI 相关资源（预制体、字体、图片等）
        /// </summary>
        private AssetComponent m_AssetComponent;

        /// <summary>
        /// 多语言管理组件
        /// 用于 UI 文本的多语言切换与本地化处理
        /// </summary>
        private LocalizationComponent m_LocalizationComponent;

        /// <summary>
        /// UI 顶层组件
        /// 管理所有 UI 根节点、层级、生命周期的核心组件
        /// </summary>
        private UIComponent m_UIComponent;

        /// <summary>
        /// 屏幕 UI 根画布
        /// 所有屏幕 2D UI 的父级画布，渲染层级最高
        /// </summary>
        private Canvas m_ScreenUICanvas;

        /// <summary>
        /// 屏幕 UI 画布适配组件
        /// 控制屏幕 UI 的分辨率适配、缩放模式
        /// </summary>
        private CanvasScaler m_ScreenUICanvasScaler;

        /// <summary>
        /// 屏幕 UI 图形射线投射组件
        /// 用于响应屏幕 UI 的点击、拖拽等交互事件
        /// </summary>
        private GraphicRaycaster m_ScreenUIGraphicRaycaster;

        /// <summary>
        /// 屏幕 UI 画布组组件
        /// 用于控制屏幕 UI 整体的显隐、射线拦截、透明度
        /// </summary>
        private CanvasGroup m_ScreenUICanvasGroup;

        /// <summary>
        /// 屏幕 UI 相机列表
        /// 渲染屏幕 UI 所使用的相机集合
        /// </summary>
        private List<Camera> m_ScreenUICameras;

        /// <summary>
        /// 场景 UI 根画布
        /// 3D 场景内 UI（血条、头顶名称等）的父级画布
        /// </summary>
        private Canvas m_SceneUICanvas;

        /// <summary>
        /// WebGL 平台专用 Web UI 根画布
        /// 仅在 WebGL 平台生效，用于浏览器内嵌 UI 渲染
        /// </summary>
        private static Canvas m_WebUICanvas;
        
        /// <summary>
        /// 获取或设置 WebGL 专用 Web UI 根画布
        /// </summary>
        public static Canvas WebUICanvas { get => m_WebUICanvas; set => m_WebUICanvas = value; }

        /// <summary>
        /// 场景 UI 画布适配组件
        /// 控制场景 UI 的分辨率适配逻辑
        /// </summary>
        private CanvasScaler m_SceneUICanvasScaler;

        /// <summary>
        /// 场景 UI 图形射线投射组件
        /// 用于响应场景 UI 的交互事件
        /// </summary>
        private GraphicRaycaster m_SceneUIGraphicRaycaster;

        /// <summary>
        /// 场景 UI 画布组组件
        /// 控制场景 UI 整体显隐与交互开关
        /// </summary>
        private CanvasGroup m_SceneUICanvasGroup;

        /// <summary>
        /// 场景 UI 相机列表
        /// 渲染场景 UI 所使用的相机集合
        /// </summary>
        private List<Camera> m_SceneUICameras;

        /// <summary>
        /// UI 字体集合
        /// 支持 Unity 原生 Font 与 TextMeshPro 字体资源
        /// </summary>
        private List<Object> m_Fonts;
        
        /// <summary>
        /// 获取或设置 UI 全局字体集合
        /// </summary>
        public List<Object> Fonts
        {
            set => m_Fonts = value;
            get => m_Fonts;
        }

        /// <summary>
        /// 上一次使用的字体集合
        /// 用于字体切换时对比、还原使用
        /// </summary>
        private List<Object> m_LastFonts;
        
        /// <summary>
        /// 获取或设置上一次使用的字体集合
        /// </summary>
        public List<Object> LastFonts
        {
            set => m_LastFonts = value;
            get => m_LastFonts;
        }

        /// <summary>
        /// 全局模态 UI 阻塞开关
        /// 开启后将屏蔽所有模态窗口的弹出与交互
        /// </summary>
        private bool m_BlockModalUIsSwitch;
        
        /// <summary>
        /// 获取或设置全局模态 UI 阻塞开关
        /// </summary>
        public bool BlockModalUIsSwitch
        {
            set => m_BlockModalUIsSwitch = value;
            get => m_BlockModalUIsSwitch;
        }

        /// <summary>
        /// 全局 UI 按键抬起事件阻塞开关
        /// 开启后所有 UI 不响应 KeyUp 按键事件
        /// </summary>
        private bool m_BlockAllUIsKeyUpSwitch;
        
        /// <summary>
        /// 获取或设置全局 UI 按键抬起事件阻塞开关
        /// </summary>
        public bool BlockAllUIsKeyUpSwitch
        {
            set => m_BlockAllUIsKeyUpSwitch = value;
            get => m_BlockAllUIsKeyUpSwitch;
        }

        /// <summary>
        /// 常驻内存 UI 列表
        /// 这些 UI 不会被自动销毁，始终保持在内存中
        /// </summary>
        private List<GameObject> m_PermanentUIs;

        /// <summary>
        /// 网络等待加载 UI 实例（常驻）
        /// </summary>
        private UIConnectionWaitingView _mConnectionWaitingUIConnection;
        
        /// <summary>
        /// 获取网络等待加载 UI 实例
        /// </summary>
        public UIConnectionWaitingView ConnectionWaitingUIConnection
        {
            get => _mConnectionWaitingUIConnection;
        }

        /// <summary>
        /// 等待 UI 引用计数
        /// 用于控制等待 UI 的显示/隐藏次数，避免重复开启/关闭
        /// </summary>
        private int m_WaitingUIRefCount;
        
        /// <summary>
        /// 获取或设置等待 UI 引用计数
        /// </summary>
        public int WaitingUIRefCount
        {
            set => m_WaitingUIRefCount = value;
            get => m_WaitingUIRefCount;
        }

        /// <summary>
        /// 流程切换过渡动画 UI（常驻）
        /// 用于场景切换、模块跳转时的转场动画
        /// </summary>
        private UILauncherLogoView m_TransitionUI;
        
        /// <summary>
        /// 获取流程切换过渡动画 UI 实例
        /// </summary>
        public UILauncherLogoView TransitionUI
        {
            get => m_TransitionUI;
        }

        /// <summary>
        /// 当前显示的模态 UI
        /// 同一时间仅允许一个模态 UI 处于激活状态
        /// </summary>
        private UIFlagBehaviour m_CurModalUI;
        
        /// <summary>
        /// 获取当前显示的模态 UI
        /// </summary>
        public UIFlagBehaviour CurModalUI
        {
            get => m_CurModalUI;
        }

        /// <summary>
        /// 模态 UI 等待队列
        /// 按顺序缓存待弹出的模态 UI，等待当前模态 UI 关闭后依次显示
        /// </summary>
        private readonly List<UIInfo> m_ModalUIInfoList;
        
        /// <summary>
        /// 获取模态 UI 等待队列
        /// </summary>
        public List<UIInfo> ModalUIInfoList
        {
            get => m_ModalUIInfoList;
        }

        /// <summary>
        /// 非模态 UI 列表
        /// 可同时显示、无遮挡优先级的普通 UI
        /// </summary>
        private readonly List<UIFlagBehaviour> m_UnModalUIList;
        
        /// <summary>
        /// 获取非模态 UI 列表
        /// </summary>
        public List<UIFlagBehaviour> UnModalUIList
        {
            get => m_UnModalUIList;
        }

        /// <summary>
        /// 场景 UI 列表
        /// 所有 3D 场景内挂载的 UI 集合
        /// </summary>
        private readonly List<UIFlagBehaviour> m_SceneUIList;
        
        /// <summary>
        /// 获取场景 UI 列表
        /// </summary>
        public List<UIFlagBehaviour> SceneUIList
        {
            get => m_SceneUIList;
        }

        /// <summary>
        /// 子/附加 UI 字典
        /// Key：UI 类型，Value：对应类型的子 UI 列表
        /// </summary>
        private readonly Dictionary<UIType, List<UIFlagBehaviour>> m_SubUIList;
        
        /// <summary>
        /// 获取子/附加 UI 字典
        /// </summary>
        public Dictionary<UIType, List<UIFlagBehaviour>> SubUIList
        {
            get => m_SubUIList;
        }

        /// <summary>
        /// 待卸载 UI 列表
        /// 缓存需要销毁的 UI，分帧销毁以避免卡顿
        /// </summary>
        private readonly List<UIFlagBehaviour> m_UnloadUIList;
        
        /// <summary>
        /// 获取待卸载 UI 列表
        /// </summary>
        public List<UIFlagBehaviour> UnloadUIList
        {
            get => m_UnloadUIList;
        }

        /// <summary>
        /// 每帧最大销毁 UI 数量
        /// 限制每帧销毁数量，防止大量 UI 同时销毁导致游戏卡顿
        /// </summary>
        private int m_DestroyMaxNumPerFrame = 1;
        
        /// <summary>
        /// 获取或设置每帧最大销毁 UI 数量
        /// </summary>
        public int DestroyMaxNumPerFrame
        {
            set => m_DestroyMaxNumPerFrame = value;
            get => m_DestroyMaxNumPerFrame;
        }

        /// <summary>
        /// 多语言文本检测开关
        /// 开启后自动检查 UI 文本是否完成本地化配置
        /// </summary>
        private bool m_CheckTextLocalizings;
        
        /// <summary>
        /// 获取多语言文本检测开关状态
        /// </summary>
        public bool CheckTextLocalizings
        {
            get => m_CheckTextLocalizings;
        }

        /// <summary>
        /// 等待 UI 的 AB 包路径
        /// </summary>
        private string m_WaitingUIABPath;

        /// <summary>
        /// 等待 UI 的资源名称
        /// </summary>
        private string m_WaitingUIAssetName;

        /// <summary>
        /// 飘字 UI 的 AB 包路径
        /// </summary>
        private string m_FloatWordsUIABPath;

        /// <summary>
        /// 飘字 UI 的资源名称
        /// </summary>
        private string m_FloatWordsUIAssetName;

        /// <summary>
        /// 飘字默认显示时长
        /// </summary>
        private float m_FloatWordsDuration;
        
        /// <summary>
        /// 获取或设置飘字默认显示时长
        /// </summary>
        public float FloatWordsDuration
        {
            set => m_FloatWordsDuration = value;
            get => m_FloatWordsDuration;
        }

        /// <summary>
        /// 刘海屏安全区域偏移尺寸
        /// 基于 Screen.safeArea 计算，用于适配异形屏
        /// </summary>
        private Vector2 m_UIBangsSize = new Vector2(-1, -1);
        
        /// <summary>
        /// 获取或设置刘海屏安全区域偏移尺寸
        /// </summary>
        public Vector2 UIBangsSize
        {
            set => m_UIBangsSize = value;
            get => m_UIBangsSize;
        }

        /// <summary>
        /// 当前屏幕方向
        /// 用于判断横竖屏，动态调整 UI 布局
        /// </summary>
        private ScreenOrientation m_ScreenOrientation = ScreenOrientation.Unknown;
        
        /// <summary>
        /// 获取或设置当前屏幕方向
        /// </summary>
        public ScreenOrientation ScreenOrientation
        {
            set => m_ScreenOrientation = value;
            get => m_ScreenOrientation;
        }
    }
}