using System.Collections.Generic;
using UnityEngine.Networking;

namespace Honor.Runtime
{
    public sealed partial class AssetBundleLoadManager
    {
        #region 异步加载并发限制常量
        /// <summary>
        /// 最大同时异步加载数量限制
        /// 防止同时加载过多AB包导致性能与IO压力过大
        /// </summary>
        private const int MAX_LOADING_COUNT = 10;
        #endregion

        #region 临时缓存集合（Update 专用）
        /// <summary>
        /// 临时缓存列表（帧更新专用）
        /// 用于Update中遍历处理时避免GC，提升性能
        /// </summary>
        private readonly List<AssetBundleObject> m_TempLoadeds;
        #endregion

        #region WebGL 平台专用加载对象
        /// <summary>
        /// WebGL平台专用请求对象
        /// 用于WebGL环境加载AB包
        /// </summary>
        private UnityWebRequest m_WebGLRequest;
        #endregion

        #region AB 依赖关系管理
        /// <summary>
        /// 全局AB依赖关系表
        /// 从Manifest中读取，记录所有AB包的依赖路径
        /// Key：AB格式化路径
        /// Value：依赖的AB路径数组
        /// </summary>
        private readonly Dictionary<string, string[]> m_DependsDataList;
        
        /// <summary>
        /// 获取全局AB依赖表
        /// </summary>
        public Dictionary<string, string[]> DependsDataList
        {
            get
            {
                return m_DependsDataList;
            }
        }
        #endregion

        #region 等待加载队列管理
        /// <summary>
        /// 等待加载队列
        /// 超出最大加载数量时，暂存等待的AB包
        /// </summary>
        private readonly Dictionary<string, AssetBundleObject> _mReadyAssetBundleList;
        
        /// <summary>
        /// 获取等待加载的AB列表
        /// </summary>
        public Dictionary<string, AssetBundleObject> ReadyAssetBundleList
        {
            get
            {
                return _mReadyAssetBundleList;
            }
        }
        #endregion

        #region 异步加载中列表管理
        /// <summary>
        /// 异步加载中列表
        /// 正在异步加载的AB包对象
        /// </summary>
        private readonly Dictionary<string, AssetBundleObject> m_LoadingAssetBundleList;
        
        /// <summary>
        /// 获取正在异步加载的AB列表
        /// </summary>
        public Dictionary<string, AssetBundleObject> LoadingAssetBundleList
        {
            get
            {
                return m_LoadingAssetBundleList;
            }
        }
        #endregion

        #region 已加载完成列表管理
        /// <summary>
        /// 已加载完成列表
        /// 加载成功、可正常使用的AB包对象
        /// </summary>
        private readonly Dictionary<string, AssetBundleObject> m_LoadedAssetBundleList;
        
        /// <summary>
        /// 获取已加载完成的AB列表
        /// </summary>
        public Dictionary<string, AssetBundleObject> LoadedAssetBundleList
        {
            get
            {
                return m_LoadedAssetBundleList;
            }
        }
        #endregion

        #region 待卸载列表管理
        /// <summary>
        /// 待卸载列表
        /// 引用计数为0，等待帧更新自动卸载的AB包
        /// </summary>
        private readonly Dictionary<string, AssetBundleObject> m_UnloadAssetBundleList;
        
        /// <summary>
        /// 获取等待卸载的AB列表
        /// </summary>
        public Dictionary<string, AssetBundleObject> UnloadAssetBundleList
        {
            get
            {
                return m_UnloadAssetBundleList;
            }
        }
        #endregion

        #region 路径格式化缓存
        /// <summary>
        /// AB格式化路径缓存
        /// 避免重复字符串处理，提升路径转换效率
        /// </summary>
        private Dictionary<string, string> m_AssetBundleFormatPathCaches;
        #endregion
    }
}