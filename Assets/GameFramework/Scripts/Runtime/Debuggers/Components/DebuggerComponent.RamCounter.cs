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
        // 内存计数器（内部私有类）
        //=========================================================================
        #region RamCounter
        /// <summary>
        /// 内存计数器
        /// 通过 Unity Profiler 采集已分配/保留的原生内存与 Mono 托管内存
        /// </summary>
        private sealed class RamCounter
        {
            /// <summary>已分配的原生内存（MB）</summary>
            public float AllocatedRam { get; private set; }
            /// <summary>保留的原生内存（MB）</summary>
            public float ReservedRam { get; private set; }
            /// <summary>保留但未使用的原生内存（MB）</summary>
            public float UnusedReservedRam { get; private set; }
            /// <summary>已分配的 Mono 托管内存（MB）</summary>
            public float AllocatedMonoRam { get; private set; }
            /// <summary>Mono 堆保留内存（MB）</summary>
            public float ReservedMonoRam { get; private set; }

            /// <summary>MB 换算系数（1024 * 1024）</summary>
            private const long BytesPerMb = 1048576L;

            /// <summary>
            /// 每帧刷新内存数据
            /// </summary>
            /// <param name="elapseSeconds">逻辑流逝时间</param>
            /// <param name="realElapseSeconds">真实流逝时间</param>
            public void Update(float elapseSeconds, float realElapseSeconds)
            {
                // TODO: 抽离重复的 /1048576f 换算为常量 BytesPerMb
                AllocatedRam = UnityEngine.Profiling.Profiler.GetTotalAllocatedMemoryLong() / (float)BytesPerMb;
                ReservedRam = UnityEngine.Profiling.Profiler.GetTotalReservedMemoryLong() / (float)BytesPerMb;
                UnusedReservedRam = UnityEngine.Profiling.Profiler.GetTotalUnusedReservedMemoryLong() / (float)BytesPerMb;

                AllocatedMonoRam = UnityEngine.Profiling.Profiler.GetMonoUsedSizeLong() / (float)BytesPerMb;
                ReservedMonoRam = UnityEngine.Profiling.Profiler.GetMonoHeapSizeLong() / (float)BytesPerMb;
            }
        }
        #endregion
    }
}

