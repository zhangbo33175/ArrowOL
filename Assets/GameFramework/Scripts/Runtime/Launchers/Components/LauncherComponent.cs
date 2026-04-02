using UnityEngine;

namespace Honor.Runtime
{
    [DisallowMultipleComponent]
    public sealed partial class LauncherComponent : GameComponent
    {
        /// <summary>
        /// 初始化唤醒工作
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            m_EditorResourceMode &= Application.isEditor;
            if (m_EditorResourceMode)
            {
                Log.Info("运行过程中框架将使用【编辑器资源】文件模式。");
            }
            else
            {
                Log.Info("运行过程中框架将使用【AssetBundle资源】文件模式。");
            }

            // 校正一下当前是否为Luac模式运行（仅编辑器内可以切换Luac模式，非编辑器内只能使用Luac）
            m_LuacMode = Application.isEditor ? m_LuacMode : true;
            if (m_LuacMode)
            {
                Log.Info("运行过程中框架将使用 Luac 脚本。");
            }
            else
            {
                Log.Info("运行过程中框架将使用 Lua 脚本。");
            }

            // 校正一下当前是否为Lua热重载模式运行（仅编辑器内有效）
            m_LuaHotReloadMode = Application.isEditor ? m_LuaHotReloadMode : false;
            if (m_LuaHotReloadMode)
            {
                Log.Info("运行过程中框架将使用【Lua热重载】模式。");
            }

            // 初始化性能指标
            InitPerformanceOptions();

            // 注册内存不足预警回调
            Application.lowMemory += OnLowMemory;

            // 打印引擎版本信息
            Log.Info("UNITY版本号: {0}", Application.unityVersion);

            // 打印Honor版本信息
            Log.Info("Honor版本号: {0}", GameConstants.HonorVersion);

            // 打印应用版本信息
            Log.Info("APP版本号: {0}", Application.version);

        }

        private void Start()
        {

        }

        private void OnDestroy()
        {
            // 注销内存不足预警回调
            Application.lowMemory -= OnLowMemory;

        }

        /// <summary>
        /// App挂起和恢复挂起时的回调
        /// </summary>
        /// <param name="pause">是否为挂起</param>
        public void OnApplicationPause(bool pause)
        {
            Log.Info(pause ? "APP挂起。" : "APP恢复。");

            LuaComponent luaComponent = GameComponentsGroup.GetComponent<LuaComponent>();
            if (luaComponent != null && luaComponent.LuaApplicationPauseFromCSEventDelegate != null)
            {
                luaComponent.LuaApplicationPauseFromCSEventDelegate(pause);
            }
        }

        /// <summary>
        /// App退出时的回调
        /// </summary>
        public void OnApplicationQuit()
        {
            LuaComponent luaComponent = GameComponentsGroup.GetComponent<LuaComponent>();
            if (luaComponent != null && luaComponent.LuaApplicationQuitFromCSEventDelegate != null)
            {
                luaComponent.LuaApplicationQuitFromCSEventDelegate();
            }
            Log.Info("APP退出。");
        }

        /// <summary>
        /// 暂停游戏
        /// </summary>
        public void PauseGame()
        {
            if (IsGamePaused)
            {
                return;
            }

            m_GameSpeedCache = GameSpeed;
            GameSpeed = 0f;
        }

        /// <summary>
        /// 恢复游戏
        /// </summary>
        public void ResumeGame()
        {
            if (!IsGamePaused)
            {
                return;
            }

            GameSpeed = m_GameSpeedCache;
        }

        /// <summary>
        /// 重置为正常游戏速度
        /// </summary>
        public void ResetNormalGameSpeed()
        {
            if (IsNormalGameSpeed)
            {
                return;
            }

            GameSpeed = 1f;
        }

        /// <summary>
        /// 关闭游戏
        /// </summary>
        public void Shutdown()
        {
            Log.Info("关闭框架Honor...");

            // 先销毁框架启动器（销毁掉启动器下的所有组件对象）
            OnApplicationPause(true);
            OnApplicationQuit();
            Destroy(gameObject);

            // 清空已注册的所有框架组件
            GameComponentsGroup.Clear();

            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            return;
        }       
        
    }

}


