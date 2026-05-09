using System.Collections.Generic;
using XLua;

namespace Honor.Runtime
{
    public sealed partial class PrefabLoadManager
    {
        /// <summary>
        /// 已加载完成的 Prefab 缓存列表
        /// Key：资源唯一路径
        /// Value：Prefab 包装对象
        /// </summary>
        private readonly Dictionary<string, PrefabObject> m_LoadedList;
        
        /// <summary>
        /// 获取已加载完成的 Prefab 缓存列表
        /// </summary>
        public Dictionary<string, PrefabObject> LoadedList
        {
            get
            {
                return m_LoadedList;
            }
        }

        /// <summary>
        /// 异步加载临时中转列表
        /// 用于延迟统一派发回调
        /// 解决：异步调用时资源已加载完成，仍需保证异步回调逻辑
        /// </summary>
        private readonly List<PrefabObject> m_LoadedAsyncTmpAgentList;

        /// <summary>
        /// 实例 ID 映射表
        /// 通过 GameObject InstanceID 快速找到所属 Prefab 包装对象
        /// </summary>
        private readonly Dictionary<int, PrefabObject> m_GOInstanceIDList;
        
        /// <summary>
        /// 获取实例 ID 映射表
        /// </summary>
        public Dictionary<int, PrefabObject> GOInstanceIDList
        {
            get
            {
                return m_GOInstanceIDList;
            }
        }

        /// <summary>
        /// 底层资源加载管理器（负责 Asset/AB 加载）
        /// </summary>
        private readonly AssetLoadManager m_AssetLoadManager = null;
        
        /// <summary>
        /// 获取资源加载管理器实例
        /// </summary>
        public AssetLoadManager AssetLoadManager
        {
            get
            {
                return m_AssetLoadManager;
            }
        }
    }
}