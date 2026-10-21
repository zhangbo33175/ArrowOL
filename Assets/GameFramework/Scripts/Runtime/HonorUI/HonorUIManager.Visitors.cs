/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  HonorUIManager.Visitors.cs
 * author:    云毅
 * created:   2026
 * descrip:   UI 模块门面管理器 - 便捷状态与访问（partial）
 *            提供 UI 相关组件引用与门面就绪状态，提高调用可读性
 ***************************************************************/

using System.Collections.Generic;
using UnityEngine;

namespace Honor.Runtime
{
    public sealed partial class HonorUIManager
    {
        //=========================================================================

        #region 组件引用

        //=========================================================================

        private LauncherComponent m_Launcher;
        private UIComponent m_UI;
        private ProcedureComponent m_Procedure;
        private SceneComponent m_Scene;
        private DebuggerComponent m_Debugger;

        /// <summary>启动器组件（游戏初始化、运行模式）</summary>
        public LauncherComponent Launcher
        {
            get { EnsureComponentsReady(); return m_Launcher; }
            private set { m_Launcher = value; }
        }

        /// <summary>UI管理组件（界面显示/隐藏/层级）</summary>
        public UIComponent UI
        {
            get { EnsureComponentsReady(); return m_UI; }
            private set { m_UI = value; }
        }

        /// <summary>流程状态机组件（游戏流程切换）</summary>
        public ProcedureComponent Procedure
        {
            get { EnsureComponentsReady(); return m_Procedure; }
            private set { m_Procedure = value; }
        }

        /// <summary>场景管理组件（场景加载/卸载/对象管理）</summary>
        public SceneComponent Scene
        {
            get { EnsureComponentsReady(); return m_Scene; }
            private set { m_Scene = value; }
        }

        /// <summary>调试组件</summary>
        public DebuggerComponent Debugger
        {
            get { EnsureComponentsReady(); return m_Debugger; }
            private set { m_Debugger = value; }
        }

        #endregion

        //=========================================================================

        #region 便捷状态与访问

        //=========================================================================

        /// <summary>
        /// 启动器组件是否已就绪
        /// </summary>
        public bool IsLauncherReady => Launcher != null;

        /// <summary>
        /// UI 组件是否已就绪
        /// </summary>
        public bool IsUIReady => UI != null;

        /// <summary>
        /// 流程组件是否已就绪
        /// </summary>
        public bool IsProcedureReady => Procedure != null;

        /// <summary>
        /// 场景组件是否已就绪
        /// </summary>
        public bool IsSceneReady => Scene != null;

        /// <summary>
        /// 调试组件是否已就绪
        /// </summary>
        public bool IsDebuggerReady => Debugger != null;

        /// <summary>
        /// UI 相关组件是否全部就绪
        /// </summary>
        public bool IsReady => Launcher != null && UI != null && Procedure != null && Scene != null && Debugger != null;

        #endregion

        //=========================================================================

        #region 门面收敛 API（跨模块 + Lua）

        //=========================================================================

        /// <summary>
        /// 场景相机列表（转发自 SceneComponent）
        /// </summary>
        public List<Camera> SceneCameras => Scene != null ? Scene.SceneCameras : null;

        /// <summary>
        /// 屏幕层 UI 相机列表（转发自 UIComponent）
        /// </summary>
        public List<Camera> ScreenUICameras => UI != null ? UI.ScreenUICameras : null;

        /// <summary>
        /// 屏幕层 UI 根 Canvas（转发自 UIComponent）
        /// </summary>
        public Canvas ScreenUICanvas => UI != null ? UI.ScreenUICanvas : null;

        /// <summary>
        /// UI 设计基准分辨率（转发自 UIComponent）
        /// </summary>
        public Vector2 ScreenDesignedResolution => UI != null ? UI.ScreenDesignedResolution : Vector2.zero;

        /// <summary>
        /// 按钮防重复点击有效间隔（秒，转发自 UIComponent）
        /// </summary>
        public float ButtonInteractDuration => UI != null ? UI.ButtonInteractDuration : 0f;

        /// <summary>
        /// 场景根节点（转发自 SceneComponent）
        /// </summary>
        public GameObject SceneRootGO => Scene != null ? Scene.SceneRootGO : null;

        /// <summary>
        /// 编辑器性能配置（转发自 LauncherComponent）
        /// </summary>
        public LauncherComponent.DevicePerformanceData EditorPerformance => Launcher != null ? Launcher.EditorPerformance : null;

        /// <summary>
        /// iOS 性能配置（转发自 LauncherComponent）
        /// </summary>
        public LauncherComponent.DevicePerformanceData iOSPerformance => Launcher != null ? Launcher.iOSPerformance : null;

        /// <summary>
        /// Android 性能配置（转发自 LauncherComponent）
        /// </summary>
        public LauncherComponent.DevicePerformanceData AndroidPerformance => Launcher != null ? Launcher.AndroidPerformance : null;

        /// <summary>
        /// 是否亚马逊应用商店（转发自 LauncherComponent）
        /// </summary>
        public bool IsAmazonStore => Launcher != null && Launcher.IsAmazonStore;

        /// <summary>
        /// 是否开发模式（转发自 LauncherComponent）
        /// </summary>
        public bool DevelopMode => Launcher != null && Launcher.DevelopMode;

        /// <summary>
        /// 是否本地服务器（转发自 LauncherComponent）
        /// </summary>
        public bool IsLocalServer => Launcher != null && Launcher.IsLocalServer;

        /// <summary>
        /// 是否编辑器资源模式（转发自 LauncherComponent）
        /// </summary>
        public bool EditorResourceMode => Launcher != null && Launcher.EditorResourceMode;

        /// <summary>
        /// 编辑器启动语言（转发自 LauncherComponent）
        /// </summary>
        public GameDefinitions.Language EditorLanguage => Launcher != null ? Launcher.EditorLanguage : default(GameDefinitions.Language);

        /// <summary>
        /// 是否 Luac 模式（转发自 LauncherComponent）
        /// </summary>
        public bool LuacMode => Launcher != null && Launcher.LuacMode;

        /// <summary>
        /// 是否 Lua 热重载模式（转发自 LauncherComponent）
        /// </summary>
        public bool LuaHotReloadMode => Launcher != null && Launcher.LuaHotReloadMode;

        /// <summary>
        /// 当前运行中的流程（流程组件未就绪时返回 null，转发自 ProcedureComponent）
        /// </summary>
        public ProcedureState CurrentProcedure => Procedure != null ? Procedure.CurrentProcedure : null;

        #endregion
    }
}
