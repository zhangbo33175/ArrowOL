/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  DebuggerComponent.cs
 * author:    taoye
 * created:   2020/8/26
 * descrip:   调试组件
 ***************************************************************/

namespace Honor.Runtime
{
    public sealed partial class DebuggerComponent : GameComponent
    {
        private sealed class FpsCounter
        {
            private float m_UpdateInterval;
            private float m_CurrentFps;
            private float m_AverageFps;
            private float m_CurrentMsPerFrame;
            private float m_AverageMsPerFrame;
            private int m_Frames;
            private float m_Accumulator;
            private float m_TimeLeft;

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

            public float CurrentFps
            {
                get
                {
                    return m_CurrentFps;
                }
            }

            public float CurrentMsPerFrame
            {
                get
                {
                    return m_CurrentMsPerFrame;
                }
            }

            public float AverageFps
            {
                get
                {
                    return m_AverageFps;
                }
            }

            public float AverageMsPerFrame
            {
                get
                {
                    return m_AverageMsPerFrame;
                }
            }

            public void Update(float elapseSeconds, float realElapseSeconds)
            {
                m_Frames++;
                m_Accumulator += realElapseSeconds;
                m_TimeLeft -= realElapseSeconds;

                if (m_TimeLeft <= 0f)
                {
                    // CurrentFps
                    m_CurrentFps = m_Accumulator > 0f ? m_Frames / m_Accumulator : 0f;
                    m_CurrentMsPerFrame = m_Accumulator > 0f ? m_Accumulator / m_Frames * 1000f : 0f;

                    // AverageFps
                    m_AverageFps = (m_AverageFps + m_CurrentFps) / 2f;
                    m_AverageMsPerFrame = (m_AverageMsPerFrame + m_CurrentMsPerFrame) / 2f;

                    m_Frames = 0;
                    m_Accumulator = 0f;
                    m_TimeLeft += m_UpdateInterval;
                }
            }

            private void Reset()
            {
                m_CurrentFps = 0f;
                m_Frames = 0;
                m_Accumulator = 0f;
                m_TimeLeft = 0f;
            }
        }
    }
}


