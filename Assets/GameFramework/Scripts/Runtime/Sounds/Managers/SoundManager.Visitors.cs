using System.Collections.Generic;

namespace Honor.Runtime
{
    public sealed partial class SoundManager
    {
        /// <summary>
        /// 声音组集合
        /// </summary>
        private readonly Dictionary<string, SoundGroup> m_SoundGroups;

        /// <summary>
        /// 正在加载中的声音集合
        /// </summary>
        private readonly List<int> m_SoundsLoading;

        /// <summary>
        /// 加载完毕后需要立即释放的声音集合
        /// </summary>
        private readonly HashSet<int> m_SoundsToReleaseOnLoad;

        /// <summary>
        /// Asset组件
        /// </summary>
        private AssetComponent m_AssetComponent;

        /// <summary>
        /// 序列ID
        /// 只增不减
        /// </summary>
        private int m_Serial;

    }
}


