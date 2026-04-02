using UnityEngine;

namespace Honor.Runtime
{
    public sealed partial class LauncherComponent : GameComponent
    {
        // 测试
        [System.Serializable]
        public class DevicePerformanceData
        {
            public DevicePerformanceData(int processorCount, int graphicsMemorySizeHighBase, int graphicsMemorySizeMidBase, int systemMemorySizeHighBase, int systemMemorySizeMidBase)
            {
                ProcessorCount = processorCount;
                GraphicsMemorySizeHighBase = graphicsMemorySizeHighBase;
                GraphicsMemorySizeMidBase = graphicsMemorySizeMidBase;
                SystemMemorySizeHighBase = systemMemorySizeHighBase;
                SystemMemorySizeMidBase = systemMemorySizeMidBase;
            }
            public int ProcessorCount;

            public int GraphicsMemorySizeHighBase;

            public int GraphicsMemorySizeMidBase;

            public int SystemMemorySizeHighBase;

            public int SystemMemorySizeMidBase;
        }
    }

}
