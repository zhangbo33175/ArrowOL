using GameLib;
using UnityEngine;

namespace Honor.Runtime
{
    public sealed partial class LauncherManager: Singleton<LauncherManager>
    {
        private void OnDestroy()
        {
            // 注销内存不足预警回调
            Application.lowMemory -= OnLowMemory;
        }

        // /// <summary>
        // /// App挂起和恢复挂起时的回调
        // /// </summary>
        // /// <param name="pause">是否为挂起</param>
        // public void OnApplicationPause(bool pause)
        // {
        //     Log.Info(pause ? "APP挂起。" : "APP恢复。");
        //
        //     LuaComponent luaComponent = SolarComponentsGroup.GetComponent<LuaComponent>();
        //     if (luaComponent != null && luaComponent.LuaApplicationPauseFromCSEventDelegate != null)
        //     {
        //         luaComponent.LuaApplicationPauseFromCSEventDelegate(pause);
        //     }
        // }
        //
        // /// <summary>
        // /// App退出时的回调
        // /// </summary>
        // public void OnApplicationQuit()
        // {
        //     LuaComponent luaComponent = SolarComponentsGroup.GetComponent<LuaComponent>();
        //     if (luaComponent != null && luaComponent.LuaApplicationQuitFromCSEventDelegate != null)
        //     {
        //         luaComponent.LuaApplicationQuitFromCSEventDelegate();
        //     }
        //
        //     Log.Info("APP退出。");
        // }

        /// <summary>
        /// 暂停游戏
        /// </summary>
        public void PauseGame()
        {
            if (IsGamePaused)
            {
                return;
            }

            GameConfig.instance.m_GameSpeedCache = GameSpeed;
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

            GameSpeed = GameConfig.instance.m_GameSpeedCache;
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
            Log.Info("关闭框架Solar...");

            // // 先销毁框架启动器（销毁掉启动器下的所有组件对象）
            // OnApplicationPause(true);
            // OnApplicationQuit();
            // Destroy(gameObject);

            // 清空已注册的所有框架组件
            GameComponentsGroup.Clear();

            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            return;
        }

        public override void Dispose()
        {
            throw new System.NotImplementedException();
        }
    }
}