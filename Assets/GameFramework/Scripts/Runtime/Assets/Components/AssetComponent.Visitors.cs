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
        /// <summary>
        /// 资源自动卸载延迟帧数
        /// 计算公式：60帧 * 60秒 = 1分钟（默认值）
        /// </summary>
        [SerializeField] private int m_UnloadAssetDelayFrameNum = 60 * 60;

        /// <summary>
        /// 设置资源自动卸载延迟帧数
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

        /// <summary>
        /// 触发内存清理的最大加载资源数量
        /// </summary>
        [SerializeField] private int m_LoadedMaxNumToCleanMemery = 50;

        /// <summary>
        /// 设置触发内存清理的资源数量阈值
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

        /// <summary>
        /// 启动器组件引用
        /// </summary>
        private LauncherComponent m_LauncherComponent = null;

        /// <summary>
        /// 获取启动器组件
        /// </summary>
        public LauncherComponent LauncherComponent
        {
            get { return m_LauncherComponent; }
        }

        /// <summary>
        /// 底层资源加载管理器（负责 Asset/AB 加载卸载）
        /// </summary>
        private AssetLoadManager m_AssetLoadManager = null;

        /// <summary>
        /// 获取资源加载管理器实例
        /// </summary>
        public AssetLoadManager AssetLoadManager
        {
            get { return m_AssetLoadManager; }
        }

        /// <summary>
        /// 预制体加载管理器（负责 Prefab 实例化）
        /// </summary>
        private PrefabLoadManager m_PrefabLoadManager = null;

        /// <summary>
        /// 获取预制体加载管理器实例
        /// </summary>
        public PrefabLoadManager PrefabLoadManager
        {
            get { return m_PrefabLoadManager; }
        }

        /// <summary>
        /// 全局依赖资源映射表
        /// 记录 Manifest 中所有 AB 包的依赖关系
        /// </summary>
        public Dictionary<string, string[]> DependsDataList
        {
            get { return m_AssetLoadManager.AssetBundleLoadManager.DependsDataList; }
        }

        /// <summary>
        /// 是否处于编辑器资源模式（Editor 模式下直接加载资源，不打 AB 包）
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

        /// <summary>
        /// 已加载完成的预制体对象列表
        /// </summary>
        public Dictionary<string, PrefabObject> LoadedPrefabList
        {
            get { return m_PrefabLoadManager.LoadedList; }
        }

        /// <summary>
        /// 正在加载中的资源对象列表
        /// </summary>
        public Dictionary<string, AssetObject> LoadingAssetList
        {
            get { return m_AssetLoadManager.LoadingList; }
        }

        /// <summary>
        /// 已加载完成的资源对象列表
        /// </summary>
        public Dictionary<string, AssetObject> LoadedAssetList
        {
            get { return m_AssetLoadManager.LoadedList; }
        }

        /// <summary>
        /// 等待延迟卸载的资源对象列表
        /// </summary>
        public Dictionary<string, AssetObject> UnloadAssetList
        {
            get { return m_AssetLoadManager.UnloadList; }
        }

        /// <summary>
        /// 预加载资源队列
        /// </summary>
        public Queue<PreloadAssetObject> PreloadedAssetList
        {
            get { return m_AssetLoadManager.PreloadedAsyncList; }
        }

        /// <summary>
        /// 准备就绪的 AB 包列表
        /// </summary>
        public Dictionary<string, AssetBundleObject> ReadyABList
        {
            get { return m_AssetLoadManager.AssetBundleLoadManager.ReadyAssetBundleList; }
        }

        /// <summary>
        /// 正在加载中的 AB 包列表
        /// </summary>
        public Dictionary<string, AssetBundleObject> LoadingABList
        {
            get { return m_AssetLoadManager.AssetBundleLoadManager.LoadingAssetBundleList; }
        }

        /// <summary>
        /// 已加载完成的 AB 包列表
        /// </summary>
        public Dictionary<string, AssetBundleObject> LoadedABList
        {
            get { return m_AssetLoadManager.AssetBundleLoadManager.LoadedAssetBundleList; }
        }

        /// <summary>
        /// 等待卸载的 AB 包列表
        /// </summary>
        public Dictionary<string, AssetBundleObject> UnloadABList
        {
            get { return m_AssetLoadManager.AssetBundleLoadManager.UnloadAssetBundleList; }
        }

        /// <summary>
        /// 当前已激活的场景资源集合
        /// </summary>
        public List<AssetObject> Scenes
        {
            get { return m_AssetLoadManager.Scenes; }
        }

        /// <summary>
        /// 是否开启严格校验模式
        /// </summary>
        private bool m_StrictCheck = true;

        /// <summary>
        /// 设置或获取严格校验开关
        /// </summary>
        public bool StrictCheck
        {
            set { m_StrictCheck = value; }
            get { return m_StrictCheck; }
        }
    }
}