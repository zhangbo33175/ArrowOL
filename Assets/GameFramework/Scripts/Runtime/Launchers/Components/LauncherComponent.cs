/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  LauncherComponent.cs
 * author:    云毅
 * created:   2026
 * descrip:   框架启动器组件 - 核心初始化与生命周期
 ***************************************************************/

using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// 框架启动器组件
    /// 负责框架初始化、资源模式、脚本模式、游戏速度控制、应用生命周期管理
    /// </summary>
    [DisallowMultipleComponent]
    public sealed partial class LauncherComponent : GameComponent
    {
        //=========================================================================
        // 生命周期
        //=========================================================================
        #region MonoBehaviour
        /// <summary>
        /// 框架初始化（Awake 生命周期）
        /// 初始化资源模式、脚本模式、性能选项、注册系统事件
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            // 获取Debugger组件并初始化Debugger的激活窗口
            DebuggerComponent debuggerComponent = GameComponentsGroup.GetComponent<DebuggerComponent>();
            if (debuggerComponent == null)
            {
                Log.Fatal("Debugger Component 无效。");
            }
            else
            {
                debuggerComponent.SetActiveWindow();
            }

            // 编辑器资源模式仅在编辑器内生效
            m_EditorResourceMode &= Application.isEditor;
            Log.Info(m_EditorResourceMode 
                ? "运行过程中框架将使用【编辑器资源】文件模式。" 
                : "运行过程中框架将使用【AssetBundle资源】文件模式。");

            // Lua 脚本模式：按用户决策暂以明文 lua 运行（打包侧 Assets/LuaScripts 明文）。
            // 注意：场景 GameApp.unity 的 m_LuacMode 曾误置为 true，导致运行时按 assets@luacscripts
            //       查找，与明文产物 assets@luascripts 不匹配，8 个 Lua 模块全部加载失败。
            //       此处强制明文以对齐产物（不改场景开关，避免内存场景覆盖磁盘改动）。
            // 如需恢复生产级加密：打包器产出 Assets/LuacScripts/**/*.lua.bytes（用运行时密钥
            //   LuaComponent.s_LuacEncrytionKey XOR 加密，按 assets@luacscripts 命名），
            //   届时把本行改回 m_LuacMode = 场景值即可。
            m_LuacMode = false;
            Log.Info(m_LuacMode 
                ? "运行过程中框架将使用 Luac 脚本。" 
                : "运行过程中框架将使用 Lua 脚本。");

            // Lua 热重载仅编辑器内有效
            m_LuaHotReloadMode = Application.isEditor ? m_LuaHotReloadMode : false;
            if (m_LuaHotReloadMode)
            {
                Log.Info("运行过程中框架将使用【Lua热重载】模式。");
            }

            // 初始化性能配置
            InitPerformanceOptions();

            // 注册低内存警告
            Application.lowMemory += OnLowMemory;

            // 输出版本信息
            Log.Info("UNITY版本号: {0}", Application.unityVersion);
            Log.Info("Honor版本号: {0}", GameConstants.HonorVersion);
            Log.Info("APP版本号: {0}", Application.version);
        }

        /// <summary>
        /// 生命周期 Start（暂未使用）
        /// </summary>
        private void Start()
        {
        }

        /// <summary>
        /// 组件销毁时注销系统事件
        /// </summary>
        private void OnDestroy()
        {
            Application.lowMemory -= OnLowMemory;
        }
        #endregion

        //=========================================================================
        // 应用生命周期回调
        //=========================================================================
        #region Application Events
        /// <summary>
        /// 应用挂起/恢复回调
        /// </summary>
        /// <param name="pause">true=挂起 / false=恢复</param>
        public void OnApplicationPause(bool pause)
        {
            Log.Info(pause ? "APP挂起。" : "APP恢复。");

            // cmd 统一分发到 Lua（cmd: 1011 应用暂停/唤醒）
            LuaProtocolRunTime.NotifyLua((int)LuaProtocolCmd.P_LuaApplicationPauseFromCSEventDelegate,
                new ApplicationPause { Pause = pause },
                (int)LuaProtocolDispatcher.LuaProtocolReceiveType.Success);
        }

        /// <summary>
        /// 应用退出回调
        /// </summary>
        public void OnApplicationQuit()
        {
            // cmd 统一分发到 Lua（cmd: 1012 应用退出）
            LuaProtocolRunTime.NotifyLua((int)LuaProtocolCmd.P_LuaApplicationQuitFromCSEventDelegate,
                new ApplicationQuit(),
                (int)LuaProtocolDispatcher.LuaProtocolReceiveType.Success);

            Log.Info("APP退出。");
        }
        #endregion

        //=========================================================================
        // 游戏速度控制
        //=========================================================================
        #region Game Speed Control
        /// <summary>
        /// 暂停游戏（缓存当前速度并设为 0）
        /// </summary>
        public void PauseGame()
        {
            if (IsGamePaused)
                return;

            m_GameSpeedCache = GameSpeed;
            GameSpeed = 0f;
        }

        /// <summary>
        /// 恢复游戏（回到暂停前的速度）
        /// </summary>
        public void ResumeGame()
        {
            if (!IsGamePaused)
                return;

            GameSpeed = m_GameSpeedCache;
        }

        /// <summary>
        /// 重置游戏速度为正常 1 倍速
        /// </summary>
        public void ResetNormalGameSpeed()
        {
            if (IsNormalGameSpeed)
                return;

            GameSpeed = 1f;
        }
        #endregion

        //=========================================================================
        // 框架关闭
        //=========================================================================
        #region Shutdown
        /// <summary>
        /// 完全关闭框架并退出游戏
        /// 销毁组件、清空管理器、退出应用
        /// </summary>
        public void Shutdown()
        {
            Log.Info("关闭框架Honor...");

            OnApplicationPause(true);
            OnApplicationQuit();
            Destroy(gameObject);

            // 清空所有框架组件
            GameComponentsGroup.Clear();

            // 退出游戏
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
        #endregion
        
    }
}