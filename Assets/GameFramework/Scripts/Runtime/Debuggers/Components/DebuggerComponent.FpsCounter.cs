/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  DebuggerComponent.cs
 * author:    云毅
 * created:   2026
 * descrip:   调试组件
 ***************************************************************/

namespace Honor.Runtime
{
    public sealed partial class DebuggerComponent : GameComponent
    {
        //=========================================================================
        // FPS 计数器（内部私有类）
        //=========================================================================
        #region FpsCounter
        /// <summary>
        /// FPS 计数器
        /// 按指定时间间隔统计当前帧率、平均帧率及每帧耗时
        /// </summary>
        private sealed class FpsCounter
        {
            #region 私有字段
            private float m_UpdateInterval;
            private float m_CurrentFps;
            private float m_AverageFps;
            private float m_CurrentMsPerFrame;
            private float m_AverageMsPerFrame;
            private int m_Frames;
            private float m_Accumulator;
            private float m_TimeLeft;
            #endregion

            #region 构造与属性
            /// <summary>
            /// 初始化 FPS 计数器
            /// </summary>
            /// <param name="updateInterval">统计间隔（秒），必须大于 0</param>
            public FpsCounter(float updateInterval)
            {
                if (updateInterval <= 0f)
                {
                    Log.Error("Update interval 无效。");
                    return;
                }

                m_UpdateInterval = updateInterval;
                Reset();
            }

            /// <summary>
            /// 获取或设置统计间隔（秒）
            /// </summary>
            public float UpdateInterval
            {
                get
                {
                    return m_UpdateInterval;
                }
                set
                {
                    if (value <= 0f)
                    {
                        Log.Error("Update interval 无效。");
                        return;
                    }

                    m_UpdateInterval = value;
                    Reset();
                }
            }

            /// <summary>
            /// 获取当前帧率
            /// </summary>
            public float CurrentFps
            {
                get
                {
                    return m_CurrentFps;
                }
            }

            /// <summary>
            /// 获取当前每帧耗时（毫秒）
            /// </summary>
            public float CurrentMsPerFrame
            {
                get
                {
                    return m_CurrentMsPerFrame;
                }
            }

            /// <summary>
            /// 获取历史平均帧率
            /// </summary>
            public float AverageFps
            {
                get
                {
                    return m_AverageFps;
                }
            }

            /// <summary>
            /// 获取历史平均每帧耗时（毫秒）
            /// </summary>
            public float AverageMsPerFrame
            {
                get
                {
                    return m_AverageMsPerFrame;
                }
            }
            #endregion

            #region 帧更新
            /// <summary>
            /// 每帧更新计数，到达统计间隔时计算 FPS 并累加平均值
            /// </summary>
            /// <param name="elapseSeconds">逻辑流逝时间</param>
            /// <param name="realElapseSeconds">真实流逝时间</param>
            public void Update(float elapseSeconds, float realElapseSeconds)
            {
                m_Frames++;
                m_Accumulator += realElapseSeconds;
                m_TimeLeft -= realElapseSeconds;

                if (m_TimeLeft <= 0f)
                {
                    // TODO: 优化——原逻辑用 (avg+cur)/2 简单滑动平均，保持原行为不变
                    m_CurrentFps = m_Accumulator > 0f ? m_Frames / m_Accumulator : 0f;
                    m_CurrentMsPerFrame = m_Accumulator > 0f ? m_Accumulator / m_Frames * 1000f : 0f;

                    m_AverageFps = (m_AverageFps + m_CurrentFps) / 2f;
                    m_AverageMsPerFrame = (m_AverageMsPerFrame + m_CurrentMsPerFrame) / 2f;

                    m_Frames = 0;
                    m_Accumulator = 0f;
                    m_TimeLeft += m_UpdateInterval;
                }
            }

            /// <summary>
            /// 重置所有统计数据
            /// </summary>
            private void Reset()
            {
                m_CurrentFps = 0f;
                m_Frames = 0;
                m_Accumulator = 0f;
                m_TimeLeft = 0f;
            }
            #endregion
        }
        #endregion
    }
}

