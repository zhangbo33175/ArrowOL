using System.Collections.Generic;
using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// 资源管理组件 - 字段与属性定义部分
    /// 负责提供资源加载的配置项、管理器引用、状态查询接口
    /// </summary>
    public sealed partial class AssetComponent : GameComponent
    {
        #region 【序列化配置字段】
        //=========================================================================
        // 资源自动卸载延迟帧数
        // 计算公式：60帧 * 60秒 = 1分钟（默认值）
        //=========================================================================
        /// <summary>
        /// 资源自动卸载延迟帧数，默认 60秒 = 60 * 60 帧
        /// </summary>
        [SerializeField] private int m_UnloadAssetDelayFrameNum = 60 * 60;

        //=========================================================================
        // 触发内存清理的最大加载资源数量
        //=========================================================================
        /// <summary>
        /// 触发自动内存清理的最大加载资源数量阈值
        /// </summary>
        [SerializeField] private int m_LoadedMaxNumToCleanMemery = 50;

        //=========================================================================
        // 是否开启严格校验模式
        //=========================================================================
        /// <summary>
        /// 是否开启资源路径、参数严格校验模式
        /// </summary>
        private bool m_StrictCheck = true;
        #endregion

        #region 【内部组件引用】
        //=========================================================================
        // 启动器组件引用
        //=========================================================================
        /// <summary>
        /// 启动器组件引用
        /// </summary>
        private LauncherComponent m_LauncherComponent = null;

        //=========================================================================
        // 底层资源加载管理器（负责 Asset/AB 加载卸载）
        //=========================================================================
        /// <summary>
        /// 底层资源加载管理器，负责 Asset/AB 包加载与卸载
        /// </summary>
        private AssetLoadManager m_AssetLoadManager = null;

        //=========================================================================
        // 预制体加载管理器（负责 Prefab 实例化）
        //=========================================================================
        /// <summary>
        /// 预制体加载管理器，负责 Prefab 加载、实例化、克隆
        /// </summary>
        private PrefabLoadManager m_PrefabLoadManager = null;
        #endregion

        #region 【公开属性 - 配置】
        //=========================================================================
        // 设置资源自动卸载延迟帧数
        //=========================================================================
        /// <summary>
        /// 设置资源自动卸载延迟帧数，同步到底层管理器
        /// </summary>
        public int UnloadAssetDelayFrameNum
        {
            set
            {
                m_UnloadAssetDelayFrameNum = value;
                if (m_AssetLoadManager != null)
                {
                    m_AssetLoadManager.UnloadAssetDelayFrameNum = m_UnloadAssetDelayFrameNum;
                }
            }
        }

        //=========================================================================
        // 设置触发内存清理的资源数量阈值
        //=========================================================================
        /// <summary>
        /// 设置触发自动内存清理的资源数量阈值，同步到底层管理器
        /// </summary>
        public int LoadedMaxNumToCleanMemery
        {
            set
            {
                m_LoadedMaxNumToCleanMemery = value;
                if (m_AssetLoadManager != null)
                {
                    m_AssetLoadManager.LoadedMaxNumToCleanMemery = m_LoadedMaxNumToCleanMemery;
                }
            }
        }

        //=========================================================================
        // 设置或获取严格校验开关
        //=========================================================================
        /// <summary>
        /// 设置或获取资源严格校验开关
        /// </summary>
        public bool StrictCheck
        {
            set { m_StrictCheck = value; }
            get { return m_StrictCheck; }
        }
        #endregion

        #region 【公开属性 - 管理器实例】
        //=========================================================================
        // 获取启动器组件
        //=========================================================================
        /// <summary>
        /// 获取启动器组件实例
        /// </summary>
        public LauncherComponent LauncherComponent
        {
            get { return m_LauncherComponent; }
        }

        //=========================================================================
        // 获取资源加载管理器实例
        //=========================================================================
        /// <summary>
        /// 获取底层资源加载管理器实例
        /// </summary>
        public AssetLoadManager AssetLoadManager
        {
            get { return m_AssetLoadManager; }
        }

        //=========================================================================
        // 获取预制体加载管理器实例
        //=========================================================================
        /// <summary>
        /// 获取预制体加载管理器实例
        /// </summary>
        public PrefabLoadManager PrefabLoadManager
        {
            get { return m_PrefabLoadManager; }
        }
        #endregion

        #region 【公开属性 - 运行状态】
        //=========================================================================
        // 是否处于编辑器资源模式（Editor 模式下直接加载资源，不打 AB 包）
        //=========================================================================
        /// <summary>
        /// 是否为编辑器资源模式，Editor下直接加载资源，不使用AB包
        /// </summary>
        public bool EditorResourceMode
        {
            get
            {
                LauncherComponent launcherComponent = GameComponentsGroup.GetComponent<LauncherComponent>();
                if (launcherComponent)
                {
                    return launcherComponent.EditorResourceMode;
                }
                return false;
            }
        }

        //=========================================================================
        // 全局依赖资源映射表
        // 记录 Manifest 中所有 AB 包的依赖关系
        //=========================================================================
        /// <summary>
        /// 全局AB包依赖关系映射表
        /// </summary>
        public Dictionary<string, string[]> DependsDataList
        {
            get { return m_AssetLoadManager.AssetBundleLoadManager.DependsDataList; }
        }

        //=========================================================================
        // 当前已激活的场景资源集合
        //=========================================================================
        /// <summary>
        /// 当前已加载激活的场景资源对象集合
        /// </summary>
        public List<AssetObject> Scenes
        {
            get { return m_AssetLoadManager.Scenes; }
        }
        #endregion

        #region 【公开属性 - 各类加载列表】
        //=========================================================================
        // 已加载完成的预制体对象列表
        //=========================================================================
        /// <summary>
        /// 已加载完成的预制体对象字典（路径 -> 预制体对象）
        /// </summary>
        public Dictionary<string, PrefabObject> LoadedPrefabList
        {
            get { return m_PrefabLoadManager.LoadedList; }
        }

        //=========================================================================
        // 正在加载中的资源对象列表
        //=========================================================================
        /// <summary>
        /// 正在异步/同步加载中的资源对象字典
        /// </summary>
        public Dictionary<string, AssetObject> LoadingAssetList
        {
            get { return m_AssetLoadManager.LoadingList; }
        }

        //=========================================================================
        // 已加载完成的资源对象列表
        //=========================================================================
        /// <summary>
        /// 已加载完成并缓存的资源对象字典
        /// </summary>
        public Dictionary<string, AssetObject> LoadedAssetList
        {
            get { return m_AssetLoadManager.LoadedList; }
        }

        //=========================================================================
        // 等待延迟卸载的资源对象列表
        //=========================================================================
        /// <summary>
        /// 等待延迟自动卸载的资源对象字典
        /// </summary>
        public Dictionary<string, AssetObject> UnloadAssetList
        {
            get { return m_AssetLoadManager.UnloadList; }
        }

        //=========================================================================
        // 预加载资源队列
        //=========================================================================
        /// <summary>
        /// 异步预加载资源队列
        /// </summary>
        public Queue<PreloadAssetObject> PreloadedAssetList
        {
            get { return m_AssetLoadManager.PreloadedAsyncList; }
        }

        //=========================================================================
        // 准备就绪的 AB 包列表
        //=========================================================================
        /// <summary>
        /// 已加载并就绪可使用的AB包字典
        /// </summary>
        public Dictionary<string, AssetBundleObject> ReadyABList
        {
            get { return m_AssetLoadManager.AssetBundleLoadManager.ReadyAssetBundleList; }
        }

        //=========================================================================
        // 正在加载中的 AB 包列表
        //=========================================================================
        /// <summary>
        /// 正在加载中的AB包字典
        /// </summary>
        public Dictionary<string, AssetBundleObject> LoadingABList
        {
            get { return m_AssetLoadManager.AssetBundleLoadManager.LoadingAssetBundleList; }
        }

        //=========================================================================
        // 已加载完成的 AB 包列表
        //=========================================================================
        /// <summary>
        /// 已加载完成的AB包字典
        /// </summary>
        public Dictionary<string, AssetBundleObject> LoadedABList
        {
            get { return m_AssetLoadManager.AssetBundleLoadManager.LoadedAssetBundleList; }
        }

        //=========================================================================
        // 等待卸载的 AB 包列表
        //=========================================================================
        /// <summary>
        /// 等待延迟卸载的AB包字典
        /// </summary>
        public Dictionary<string, AssetBundleObject> UnloadABList
        {
            get { return m_AssetLoadManager.AssetBundleLoadManager.UnloadAssetBundleList; }
        }
        #endregion
    }
}