/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  AssetLoadManager.cs
 * author:    云毅
 * created:   2026
 * descrip:   资源加载管理器 - 成员变量 & 属性定义分部类
 *            包含所有缓存集合、配置、管理器引用、状态标记
 ***************************************************************/
using System.Collections.Generic;

namespace Honor.Runtime
{
    //=========================================================================
    // 资源加载管理器 - 成员变量 & 属性定义
    // 所有字段、集合、配置、对外属性统一声明
    //=========================================================================
    /// <summary>
    /// 资源加载管理器（成员定义分部类）
    /// </summary>
    public sealed partial class AssetLoadManager
    {
        #region 自动卸载延迟配置
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
            set => m_UnloadAssetDelayFrameNum = value;
        }
        #endregion

        #region 自动内存清理配置
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
            set => m_LoadedMaxNumToCleanMemery = value;
        }
        #endregion

        #region 临时缓存集合（Update 专用）
        /// <summary>
        /// 临时缓存列表（帧更新专用）
        /// 用于Update遍历，减少GC分配，提升性能
        /// </summary>
        private List<AssetObject> m_TempLoadeds = new List<AssetObject>();
        #endregion

        #region 异步加载中列表管理
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
            get => m_LoadingList;
        }
        #endregion

        #region 已加载完成列表管理
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
            get => m_LoadedList;
        }
        #endregion

        #region 等待卸载列表管理
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
            get => m_UnloadList;
        }
        #endregion

        #region 异步预加载队列管理
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
            get => m_PreloadedAsyncList;
        }
        #endregion

        #region 组件与子管理器引用
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
            get => _mAssetBundleLoadManager;
        }
        #endregion

        #region 异步回调中转管理
        /// <summary>
        /// 异步加载完成临时中转列表
        /// 用于延迟统一派发回调，防止嵌套加载异常
        /// </summary>
        private readonly List<AssetObject> m_LoadedAsyncTmpAgentList;
        #endregion

        #region 资源实例ID映射表
        /// <summary>
        /// 资源实例ID映射表
        /// 通过InstanceID快速查找资源对象
        /// 注意：场景资源不存入此表
        /// </summary>
        private readonly Dictionary<int, AssetObject> m_AssetInstanceIDList;
        #endregion

        #region 场景资源管理
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
            get => m_Scenes;
        }
        #endregion

        #region 加载计数与GC控制
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
        #endregion

        #region 运行模式标识
        /// <summary>
        /// 是否为编辑器资源模式
        /// true：直接加载Asset文件
        /// false：加载AB包
        /// </summary>
        private bool m_EditorResourceMode;
        #endregion
    }
}