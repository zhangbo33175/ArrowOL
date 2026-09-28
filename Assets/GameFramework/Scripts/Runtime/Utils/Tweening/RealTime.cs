/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * -------------------------------------------------------------
 * filename:  RealTime.cs
 * author:  云毅
 * created:
 * descrip:   真实时间工具 - 不受 Time.timeScale 影响的计时（暂停菜单等场景适用）
 * 优化记录: 由旧版 HonorUtils.Tweening.RealTime 迁移，统一命名空间
 ***************************************************************/

using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// 真实时间工具
    /// 功能：提供不受 timeScale 影响的帧间隔与累计时间，用于 UI 补间等需要"暂停不中断"的逻辑
    /// </summary>
    public static class RealTime
    {
        #region 属性

        /// <summary>
        /// 真实累计时间（等价 Time.realtimeSinceStartup，可无缝替换）
        /// </summary>
        public static float time
        {
            get { return Time.realtimeSinceStartup; }
        }

        /// <summary>
        /// 真实帧间隔（不受 timeScale 影响）
        /// </summary>
        public static float deltaTime
        {
            get
            {
                // 使用两次真实时间的差值，避免 Time.unscaledDeltaTime 在编辑器暂停态为 0 的问题
                return Mathf.Clamp(Time.realtimeSinceStartup - m_LastTime, 0f, 0.2f);
            }
        }

        #endregion

        #region 字段

        /// <summary>上一帧真实时间</summary>
        private static float m_LastTime;

        #endregion

        #region 驱动

        /// <summary>
        /// 每帧推进时间基准（由 UITweener 内部调用，业务无需关心）
        /// </summary>
        internal static void UpdateLastTime()
        {
            m_LastTime = Time.realtimeSinceStartup;
        }

        #endregion
    }
}
