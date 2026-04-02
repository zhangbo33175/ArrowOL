using System.Collections.Generic;
using UnityEngine.Networking;

namespace Honor.Runtime
{
    public sealed partial class AssetBundleLoadManager
    {
        /// <summary>
        /// 允许同时加载的AB包最大数量
        /// </summary>
        private const int MAX_LOADING_COUNT = 10;

        /// <summary>
        /// 创建临时存储变量
        /// 用于提升性能
        /// </summary>
        private readonly List<AssetBundleObject> m_TempLoadeds;

        /// <summary>
        /// WebGL的AB请求
        /// </summary>
        private UnityWebRequest m_WebGLRequest;

        /// <summary>
        /// 全局依赖资源
        /// 记录了Manifest中所有AB的依赖资源关系
        /// </summary>
        private readonly Dictionary<string, string[]> m_DependsDataList;
        public Dictionary<string, string[]> DependsDataList
        {
            get
            {
                return m_DependsDataList;
            }
        }

        /// <summary>
        /// 预备加载的列表
        /// </summary>
        private readonly Dictionary<string, AssetBundleObject> _mReadyAssetBundleList;
        public Dictionary<string, AssetBundleObject> ReadyAssetBundleList
        {
            get
            {
                return _mReadyAssetBundleList;
            }
        }

        /// <summary>
        /// 正在加载的列表
        /// 用于存放异步加载的AB封装对象
        /// </summary>
        private readonly Dictionary<string, AssetBundleObject> m_LoadingAssetBundleList;
        public Dictionary<string, AssetBundleObject> LoadingAssetBundleList
        {
            get
            {
                return m_LoadingAssetBundleList;
            }
        }

        /// <summary>
        /// 加载完成的列表
        /// </summary>
        private readonly Dictionary<string, AssetBundleObject> m_LoadedAssetBundleList;
        public Dictionary<string, AssetBundleObject> LoadedAssetBundleList
        {
            get
            {
                return m_LoadedAssetBundleList;
            }
        }

        /// <summary>
        /// 准备卸载的列表
        /// </summary>
        private readonly Dictionary<string, AssetBundleObject> m_UnloadAssetBundleList;
        public Dictionary<string, AssetBundleObject> UnloadAssetBundleList
        {
            get
            {
                return m_UnloadAssetBundleList;
            }
        }

        /// <summary>
        /// AB格式化路径缓存
        /// </summary>
        private Dictionary<string, string> m_AssetBundleFormatPathCaches;

    }
}


