/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  UIManager.Define.cs
 * author:  云毅
 * created: 2026
 * descrip:   UI 管理器 - 成员变量、属性、核心定义
 ***************************************************************/
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
        #region 核心组件引用
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
        #endregion

        #region 画布与根节点 (Screen / Scene / WebGL)
        /// <summary>
        /// 屏幕 UI 根画布
        /// 所有屏幕 2D UI 的父级画布，渲染层级最高
        /// </summary>
        private Canvas m_ScreenUICanvas;

        /// <summary>
        /// 屏幕 UI 画布适配组件
        /// </summary>
        private CanvasScaler m_ScreenUICanvasScaler;

        /// <summary>
        /// 屏幕 UI 射线投射组件
        /// </summary>
        private GraphicRaycaster m_ScreenUIGraphicRaycaster;

        /// <summary>
        /// 屏幕 UI 画布组组件
        /// </summary>
        private CanvasGroup m_ScreenUICanvasGroup;

        /// <summary>
        /// 屏幕 UI 相机列表
        /// </summary>
        private List<Camera> m_ScreenUICameras;

        /// <summary>
        /// 场景 UI 根画布
        /// 3D 场景内 UI（血条、头顶名称等）
        /// </summary>
        private Canvas m_SceneUICanvas;

        /// <summary>
        /// 场景 UI 画布适配组件
        /// </summary>
        private CanvasScaler m_SceneUICanvasScaler;

        /// <summary>
        /// 场景 UI 射线投射组件
        /// </summary>
        private GraphicRaycaster m_SceneUIGraphicRaycaster;

        /// <summary>
        /// 场景 UI 画布组组件
        /// </summary>
        private CanvasGroup m_SceneUICanvasGroup;

        /// <summary>
        /// 场景 UI 相机列表
        /// </summary>
        private List<Camera> m_SceneUICameras;

        /// <summary>
        /// WebGL 平台专用 Web UI 根画布
        /// </summary>
        private static Canvas m_WebUICanvas;

        /// <summary>
        /// 获取或设置 WebGL 专用 Web UI 根画布
        /// </summary>
        public static Canvas WebUICanvas
        {
            get => m_WebUICanvas;
            set => m_WebUICanvas = value;
        }
        #endregion

        #region 字体与多语言
        /// <summary>
        /// UI 字体集合
        /// 支持 Unity 原生 Font 与 TextMeshPro
        /// </summary>
        private List<Object> m_Fonts;

        /// <summary>
        /// 获取或设置 UI 全局字体集合
        /// </summary>
        public List<Object> Fonts
        {
            get => m_Fonts;
            set => m_Fonts = value;
        }

        /// <summary>
        /// 上一次使用的字体集合
        /// </summary>
        private List<Object> m_LastFonts;

        /// <summary>
        /// 获取或设置上一次使用的字体集合
        /// </summary>
        public List<Object> LastFonts
        {
            get => m_LastFonts;
            set => m_LastFonts = value;
        }

        /// <summary>
        /// 多语言文本检测开关
        /// </summary>
        private bool m_CheckTextLocalizings;

        /// <summary>
        /// 获取多语言文本检测开关状态
        /// </summary>
        public bool CheckTextLocalizings
        {
            get => m_CheckTextLocalizings;
        }
        #endregion

        #region 全局开关与阻塞
        /// <summary>
        /// 全局模态 UI 阻塞开关
        /// 开启后屏蔽所有模态窗口弹出
        /// </summary>
        private bool m_BlockModalUIsSwitch;

        /// <summary>
        /// 获取或设置全局模态 UI 阻塞开关
        /// </summary>
        public bool BlockModalUIsSwitch
        {
            get => m_BlockModalUIsSwitch;
            set => m_BlockModalUIsSwitch = value;
        }

        /// <summary>
        /// 全局 UI 按键抬起事件阻塞开关
        /// </summary>
        private bool m_BlockAllUIsKeyUpSwitch;

        /// <summary>
        /// 获取或设置全局 UI 按键阻塞开关
        /// </summary>
        public bool BlockAllUIsKeyUpSwitch
        {
            get => m_BlockAllUIsKeyUpSwitch;
            set => m_BlockAllUIsKeyUpSwitch = value;
        }
        #endregion

        #region 常驻 UI 与实例
        /// <summary>
        /// 常驻内存 UI 列表
        /// </summary>
        private List<GameObject> m_PermanentUIs;

        /// <summary>
        /// 网络等待加载 UI 实例
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
        /// 流程切换过渡动画 UI
        /// </summary>
        private UILauncherLogoView m_TransitionUI;

        /// <summary>
        /// 获取过渡动画 UI 实例
        /// </summary>
        public UILauncherLogoView TransitionUI
        {
            get => m_TransitionUI;
        }
        #endregion

        #region UI 管理队列
        /// <summary>
        /// 当前显示的模态 UI
        /// </summary>
        private UIFlagBehaviour m_CurModalUI;

        /// <summary>
        /// 获取当前模态 UI
        /// </summary>
        public UIFlagBehaviour CurModalUI
        {
            get => m_CurModalUI;
        }

        /// <summary>
        /// 模态 UI 等待队列
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
        /// </summary>
        private readonly Dictionary<UIType, List<UIFlagBehaviour>> m_SubUIList;

        /// <summary>
        /// 获取附加 UI 字典
        /// </summary>
        public Dictionary<UIType, List<UIFlagBehaviour>> SubUIList
        {
            get => m_SubUIList;
        }

        /// <summary>
        /// 待卸载 UI 列表（分帧销毁）
        /// </summary>
        private readonly List<UIFlagBehaviour> m_UnloadUIList;

        /// <summary>
        /// 获取待卸载 UI 列表
        /// </summary>
        public List<UIFlagBehaviour> UnloadUIList
        {
            get => m_UnloadUIList;
        }
        #endregion

        #region 配置与参数
        /// <summary>
        /// 每帧最大销毁 UI 数量
        /// </summary>
        private int m_DestroyMaxNumPerFrame = 1;

        /// <summary>
        /// 获取或设置每帧最大销毁数量
        /// </summary>
        public int DestroyMaxNumPerFrame
        {
            get => m_DestroyMaxNumPerFrame;
            set => m_DestroyMaxNumPerFrame = value;
        }

        /// <summary>
        /// 等待 UI 引用计数
        /// </summary>
        private int m_WaitingUIRefCount;

        /// <summary>
        /// 获取或设置等待 UI 引用计数
        /// </summary>
        public int WaitingUIRefCount
        {
            get => m_WaitingUIRefCount;
            set => m_WaitingUIRefCount = value;
        }

        /// <summary>
        /// 等待 UI 资源路径
        /// </summary>
        private string m_WaitingUIABPath;
        private string m_WaitingUIAssetName;

        /// <summary>
        /// 飘字 UI 资源路径
        /// </summary>
        private string m_FloatWordsUIABPath;
        private string m_FloatWordsUIAssetName;

        /// <summary>
        /// 飘字默认显示时长
        /// </summary>
        private float m_FloatWordsDuration;

        /// <summary>
        /// 获取或设置飘字默认时长
        /// </summary>
        public float FloatWordsDuration
        {
            get => m_FloatWordsDuration;
            set => m_FloatWordsDuration = value;
        }
        #endregion

        #region 屏幕适配
        /// <summary>
        /// 刘海屏安全区域偏移
        /// </summary>
        private Vector2 m_UIBangsSize = new Vector2(-1, -1);

        /// <summary>
        /// 获取或设置刘海屏偏移
        /// </summary>
        public Vector2 UIBangsSize
        {
            get => m_UIBangsSize;
            set => m_UIBangsSize = value;
        }

        /// <summary>
        /// 当前屏幕方向
        /// </summary>
        private ScreenOrientation m_ScreenOrientation = ScreenOrientation.Unknown;

        /// <summary>
        /// 获取或设置屏幕方向
        /// </summary>
        public ScreenOrientation ScreenOrientation
        {
            get => m_ScreenOrientation;
            set => m_ScreenOrientation = value;
        }
        #endregion
    }
}