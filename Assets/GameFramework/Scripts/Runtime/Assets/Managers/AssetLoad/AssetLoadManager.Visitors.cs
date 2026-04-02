using System.Collections.Generic;

namespace Honor.Runtime
{
    public sealed partial class AssetLoadManager
    {
        /// <summary>
        /// 卸载最低延迟帧数
        /// Asset资源过期帧数，60*60：相当于1分钟
        /// </summary>
        private int m_UnloadAssetDelayFrameNum = 60 * 60;
        public int UnloadAssetDelayFrameNum
        {
            set
            {
                m_UnloadAssetDelayFrameNum = value;
            }
        }

        /// <summary>
        /// 每轮资源清理所需要加载完成资源的累计个数
        /// </summary>
        private int m_LoadedMaxNumToCleanMemery = 50;
        public int LoadedMaxNumToCleanMemery
        {
            set
            {
                m_LoadedMaxNumToCleanMemery = value;
            }
        }

        /// <summary>
        /// 创建临时存储变量
        /// 用于提升性能
        /// </summary>
        private List<AssetObject> m_TempLoadeds = new List<AssetObject>();

        /// <summary>
        /// 正在加载的列表
        /// 用于存放异步加载的Asset封装对象
        /// </summary>
        private readonly Dictionary<string, AssetObject> m_LoadingList;
        public Dictionary<string, AssetObject> LoadingList
        {
            get
            {
                return m_LoadingList;
            }
        }

        /// <summary>
        /// 加载完成的列表
        /// </summary>
        private readonly Dictionary<string, AssetObject> m_LoadedList;
        public Dictionary<string, AssetObject> LoadedList
        {
            get
            {
                return m_LoadedList;
            }
        }

        /// <summary>
        /// 准备卸载的列表
        /// </summary>
        private readonly Dictionary<string, AssetObject> m_UnloadList;
        public Dictionary<string, AssetObject> UnloadList
        {
            get
            {
                return m_UnloadList;
            }
        }

        /// <summary>
        /// 异步预加载队列
        /// 用于空闲时加载
        /// </summary>
        private readonly Queue<PreloadAssetObject> m_PreloadedAsyncList;
        public Queue<PreloadAssetObject> PreloadedAsyncList
        {
            get
            {
                return m_PreloadedAsyncList;
            }
        }

        /// <summary>
        /// Asset组件
        /// </summary>
        private readonly AssetComponent m_AssetComponent = null;

        /// <summary>
        /// AB加载管理器
        /// </summary>
        private readonly AssetBundleLoadManager _mAssetBundleLoadManager = null;
        public AssetBundleLoadManager AssetBundleLoadManager
        {
            get
            {
                return _mAssetBundleLoadManager;
            }
        }

        /// <summary>
        /// 异步加载临时中转列表
        /// 用于延迟回调
        /// </summary>
        private readonly List<AssetObject> m_LoadedAsyncTmpAgentList;

        /// <summary>
        /// 所有Asset资源instanceID所对应的Asset封装对象
        /// 注意：Scene是不包含在内的！
        /// </summary>
        private readonly Dictionary<int, AssetObject> m_AssetInstanceIDList;

        /// <summary>
        /// 当前激活正在使用的Scene集合
        /// 不包括Launching.unity
        /// </summary>
        private List<AssetObject> m_Scenes;
        public List<AssetObject> Scenes
        {
            get
            {
                return m_Scenes;
            }
        }

        /// <summary>
        /// 异步加载完成的资源数量计数器
        /// 每次达到上限后数据会自动清零等待下一次上限
        /// </summary>
        private int m_LoadingIntervalCount;

        /// <summary>
        /// Assets字符串长度
        /// </summary>
        private static readonly int s_AssetsStringLength = "Assets".Length;

        /// <summary>
        /// 编辑器资源模式
        /// </summary>
        private bool m_EditorResourceMode;

    }
}


