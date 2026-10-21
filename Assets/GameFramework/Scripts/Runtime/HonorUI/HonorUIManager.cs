/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  HonorUIManager.cs
 * author:    云毅
 * created:   2026
 * descrip:   UI 模块门面管理器 - HonorUI 分类统一入口（主文件）
 *            统一管理 UI 相关 Mono 组件（Launcher/UI/Procedure/Scene/Debugger），
 *            外部 UI 相关调用统一走本管理器
 *            组件引用与便捷状态见 HonorUIManager.Visitors.cs；便捷业务方法见 HonorUIManager.Methods.cs
 ***************************************************************/

namespace Honor.Runtime
{
    /// <summary>
    /// UI 模块门面管理器（HonorUI 分类统一入口）
    /// 功能：集中持有 UI 相关 Mono 组件（Launcher/UI/Procedure/Scene/Debugger）
    /// 外部调用：HonorUIManager.Instance.UI.xxx，或 .OpenUIAsyncByInfo(...) 等便捷方法
    /// 说明：采用纯代码单例，首次访问 Instance 自动调用 Init()
    /// </summary>
    public sealed partial class HonorUIManager : Singleton<HonorUIManager>
    {
        //=========================================================================

        #region 生命周期

        //=========================================================================

        // 组件引用就绪标记：false 表示场景组件尚未全部注册（惰性单例过早被访问），访问时自动补取
        private bool m_ComponentsReady;

        // 未就绪告警标记：仅首次提示一次，避免启动早期反复访问导致日志刷屏
        private bool m_WarnedNotReady;

        /// <summary>
        /// 初始化：经组件组获取全部 UI 相关 Mono 组件
        /// <para>采用延迟补取机制（见 <see cref="EnsureComponentsReady"/>），
        /// 规避惰性单例在场景组件 Awake 完成前被访问导致组件引用被缓存为 null 的时序问题。</para>
        /// </summary>
        public override void Init()
        {
            EnsureComponentsReady();
        }

        /// <summary>
        /// 释放：置空组件引用
        /// <para>组件为场景 Mono 对象，由场景生命周期销毁，此处仅断开门面引用。</para>
        /// </summary>
        public override void Dispose()
        {
            m_Launcher = null;
            m_UI = null;
            m_Procedure = null;
            m_Scene = null;
            m_Debugger = null;
            m_ComponentsReady = false;
            m_WarnedNotReady = false;
        }

        /// <summary>
        /// 确保组件引用就绪：组件未取齐时经 GameComponentsGroup 逐个补取
        /// <para>解决惰性单例时序问题：若门面在场景组件（Launcher/UI/Procedure/Scene/Debugger）
        /// Awake 完成前被首次访问（例：LauncherComponent.Awake 动态实例化 Debugger 并调用
        /// SetActiveWindow），会把尚未注册的组件引用缓存为 null。
        /// 本方法采用逐个补取：组件各自 Awake 注册时序不定，已注册的组件立即缓存、立即可用，
        /// 未注册的保持 null，待下次访问再补取，直到核心组件（Launcher/UI/Procedure/Scene）就绪；
        /// Debugger 可配置关闭（showDebugger 为 false 时不创建），允许为空。</para>
        /// </summary>
        private void EnsureComponentsReady()
        {
            if (m_ComponentsReady)
            {
                return;
            }

            // 逐个补取：已注册的立即缓存，未注册的保持 null，下次访问再补取
            if (m_Launcher == null) { m_Launcher = GameComponentsGroup.GetComponent<LauncherComponent>(); }
            if (m_UI == null) { m_UI = GameComponentsGroup.GetComponent<UIComponent>(); }
            if (m_Procedure == null) { m_Procedure = GameComponentsGroup.GetComponent<ProcedureComponent>(); }
            if (m_Scene == null) { m_Scene = GameComponentsGroup.GetComponent<SceneComponent>(); }
            if (m_Debugger == null) { m_Debugger = GameComponentsGroup.GetComponent<DebuggerComponent>(); }

            // 核心组件（Launcher/UI/Procedure/Scene）已全部取到即视为就绪；Debugger 可配置关闭，允许为空
            if (m_Launcher != null && m_UI != null && m_Procedure != null && m_Scene != null)
            {
                m_ComponentsReady = true;
            }
            else if (!m_WarnedNotReady)
            {
                m_WarnedNotReady = true;
                Log.Warning("HonorUIManager 核心组件未全部就绪，访问时将自动补取。可能是在场景组件 Awake 完成前触发了门面初始化。");
            }
        }

        /// <summary>
        /// 每帧驱动：推进 UI 相关组件生命周期
        /// <para>当前 5 个组件均为 MonoBehaviour，由 Unity 生命周期直接驱动（各自 Update），
        /// 本方法作为门面生命周期统一入口预留；若后续组件改造为纯代码单例，在本方法内驱动。</para>
        /// </summary>
        public void DriveUpdate()
        {
            // 预留：Mono 组件由 Unity 驱动，无需额外每帧调用
        }

        #endregion
    }
}
