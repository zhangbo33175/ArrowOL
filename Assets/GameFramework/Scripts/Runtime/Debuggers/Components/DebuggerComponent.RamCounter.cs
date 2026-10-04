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
        private sealed class RamCounter
        {
            public float AllocatedRam { get; private set; }
            public float ReservedRam { get; private set; }
            public float UnusedReservedRam { get; private set; }
            public float AllocatedMonoRam { get; private set; }
            public float ReservedMonoRam { get; private set; }

            public void Update(float elapseSeconds, float realElapseSeconds)
            {
                AllocatedRam = UnityEngine.Profiling.Profiler.GetTotalAllocatedMemoryLong() / 1048576f;
                ReservedRam = UnityEngine.Profiling.Profiler.GetTotalReservedMemoryLong() / 1048576f;
                UnusedReservedRam = UnityEngine.Profiling.Profiler.GetTotalUnusedReservedMemoryLong() / 1048576f;

                AllocatedMonoRam = UnityEngine.Profiling.Profiler.GetMonoUsedSizeLong() / 1048576f;
                ReservedMonoRam = UnityEngine.Profiling.Profiler.GetMonoHeapSizeLong() / 1048576f;
            }
        }
    }
}


