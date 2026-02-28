using GameLib;
using UnityEngine;

namespace Honor.Runtime
{
    public sealed partial class LauncherManager
    {
        /// <summary>
        /// 初始化性能指标
        /// </summary>
        private void InitPerformanceOptions()
        {
            // 运行帧率设定
            Application.targetFrameRate = GameConfig.instance.m_FrameRate;

            // 运行速率设定
            Time.timeScale = GameConfig.instance.m_GameSpeed;

            // 后台运行设定
            Application.runInBackground = GameConfig.instance.m_RunInBackground;

            // 屏幕常亮设定
            Screen.sleepTimeout = GameConfig.instance.m_NeverSleep ? SleepTimeout.NeverSleep : SleepTimeout.SystemSetting;

        }

        /// <summary>
        /// 内存不足预警回调
        /// </summary>
        private void OnLowMemory()
        {
            Log.Info("内存即将不足，开始释放无用对象池和无用资源......");

            // 强制调用GC
            System.GC.Collect();

        }
    }
}