using System.Collections.Generic;

namespace Honor.Runtime
{
    public sealed partial class AssetLoadManager
    {
        /// <summary>
        /// 资源自动卸载延迟帧数
        /// 计算公式：60 * 60 = 1分钟（基于60帧）
        /// </summary>
        private int m_UnloadAssetDelayFrameNum = 60 * 60;
        
        /// <summary>
        /// 设置资源自动卸载延迟帧数
        /// </summary>
        public int UnloadAssetDelayFrameNum
        {
            set
            {
                m_UnloadAssetDelayFrameNum = value;
            }
        }

        /// <summary>
        /// 触发自动内存清理的加载数量上限
        /// 累计加载此数量后自动 GC
        /// </summary>
        private int m_LoadedMaxNumToCleanMemery = 50;
        
        /// <summary>
        /// 设置自动内存清理上限
        /// </summary>
        public int LoadedMaxNumToCleanMemery
        {
            set
            {
                m_LoadedMaxNumToCleanMemery = value;
            }
        }

        /// <summary>
        /// 临时缓存列表（帧更新专用）
        /// 用于Update遍历，减少GC分配，提升性能
        /// </summary>
        private List<AssetObject> m_TempLoadeds = new List<AssetObject>();

        /// <summary>
        /// 异步加载中列表
        /// 存储正在异步加载的资源包装对象
        /// Key：资源唯一路径
        /// </summary>
        private readonly Dictionary<string, AssetObject> m_LoadingList;
        
        /// <summary>
        /// 获取异步加载中列表
        /// </summary>
        public Dictionary<string, AssetObject> LoadingList
        {
            get
            {
                return m_LoadingList;
            }
        }

        /// <summary>
        /// 已加载完成列表
        /// 存储加载成功、可正常使用的资源对象
        /// </summary>
        private readonly Dictionary<string, AssetObject> m_LoadedList;
        
        /// <summary>
        /// 获取已加载完成列表
        /// </summary>
        public Dictionary<string, AssetObject> LoadedList
        {
            get
            {
                return m_LoadedList;
            }
        }

        /// <summary>
        /// 等待卸载列表
        /// 引用计数为0，等待延迟卸载的资源
        /// </summary>
        private readonly Dictionary<string, AssetObject> m_UnloadList;
        
        /// <summary>
        /// 获取等待卸载列表
        /// </summary>
        public Dictionary<string, AssetObject> UnloadList
        {
            get
            {
                return m_UnloadList;
            }
        }

        /// <summary>
        /// 异步预加载队列
        /// 空闲时按顺序预加载，不阻塞主线程
        /// </summary>
        private readonly Queue<PreloadAssetObject> m_PreloadedAsyncList;
        
        /// <summary>
        /// 获取异步预加载队列
        /// </summary>
        public Queue<PreloadAssetObject> PreloadedAsyncList
        {
            get
            {
                return m_PreloadedAsyncList;
            }
        }

        /// <summary>
        /// 资源管理组件引用
        /// </summary>
        private readonly AssetComponent m_AssetComponent = null;

        /// <summary>
        /// AssetBundle 加载管理器
        /// </summary>
        private readonly AssetBundleLoadManager _mAssetBundleLoadManager = null;
        
        /// <summary>
        /// 获取 AssetBundle 加载管理器
        /// </summary>
        public AssetBundleLoadManager AssetBundleLoadManager
        {
            get
            {
                return _mAssetBundleLoadManager;
            }
        }

        /// <summary>
        /// 异步加载完成临时中转列表
        /// 用于延迟统一派发回调，防止嵌套加载异常
        /// </summary>
        private readonly List<AssetObject> m_LoadedAsyncTmpAgentList;

        /// <summary>
        /// 资源实例ID映射表
        /// 通过InstanceID快速查找资源对象
        /// 注意：场景资源不存入此表
        /// </summary>
        private readonly Dictionary<int, AssetObject> m_AssetInstanceIDList;

        /// <summary>
        /// 当前已加载的场景列表
        /// 不包含启动场景 Launching.unity
        /// </summary>
        private List<AssetObject> m_Scenes;
        
        /// <summary>
        /// 获取已加载场景列表
        /// </summary>
        public List<AssetObject> Scenes
        {
            get
            {
                return m_Scenes;
            }
        }

        /// <summary>
        /// 异步加载数量计数器
        /// 统计累计加载数量，达到阈值后触发GC
        /// </summary>
        private int m_LoadingIntervalCount;

        /// <summary>
        /// "Assets" 字符串固定长度
        /// 用于路径裁剪优化
        /// </summary>
        private static readonly int s_AssetsStringLength = "Assets".Length;

        /// <summary>
        /// 是否为编辑器资源模式
        /// true：直接加载Asset文件
        /// false：加载AB包
        /// </summary>
        private bool m_EditorResourceMode;
    }
}