using UnityEngine;

namespace Honor.Runtime
{
    public sealed partial class LauncherComponent : GameComponent
    {
        /// <summary>
        /// 初始化性能指标
        /// </summary>
        private void InitPerformanceOptions()
        {
            // 运行帧率设定
            Application.targetFrameRate = m_FrameRate;

            // 运行速率设定
            Time.timeScale = m_GameSpeed;

            // 后台运行设定
            Application.runInBackground = m_RunInBackground;

            // 屏幕常亮设定
            Screen.sleepTimeout = m_NeverSleep ? SleepTimeout.NeverSleep : SleepTimeout.SystemSetting;

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
