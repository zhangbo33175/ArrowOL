/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  LauncherComponent.PerformanceLogic.cs
 * author:    云毅
 * created: 2025
 * descrip:   框架启动器 - 性能配置与低内存处理（partial）
 ***************************************************************/

using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// 框架启动器 - 性能配置与内存管理部分
    /// </summary>
    public sealed partial class LauncherComponent : GameComponent
    {
        //=========================================================================
        // 性能与内存管理
        //=========================================================================
        #region Performance & Memory
        /// <summary>
        /// 初始化游戏性能相关配置
        /// 包括帧率、游戏速度、后台运行、屏幕常亮等
        /// </summary>
        private void InitPerformanceOptions()
        {
            // 设置目标帧率
            Application.targetFrameRate = m_FrameRate;

            // 设置游戏时间缩放速度
            Time.timeScale = m_GameSpeed;

            // 设置是否允许后台运行
            Application.runInBackground = m_RunInBackground;

            // 设置屏幕是否常亮不休眠
            Screen.sleepTimeout = m_NeverSleep ? SleepTimeout.NeverSleep : SleepTimeout.SystemSetting;
        }

        /// <summary>
        /// 系统低内存警告回调
        /// 触发时主动进行 GC 回收，减少内存占用
        /// </summary>
        private void OnLowMemory()
        {
            Log.Info("内存即将不足，开始释放无用对象池和无用资源......");

            // 强制垃圾回收
            System.GC.Collect();
        }
        #endregion
    }
}