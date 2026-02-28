using System.Collections.Generic;
using GameLib;
using UnityEngine;
using UnityEngine.Networking;

namespace Honor.Runtime
{
    /// <summary>
    /// 预制体加载完成的回调
    /// </summary>
    public delegate void LoadPrefabOverCallback(LoadPrefabData prefabObject, GameObject gameObject);

    /// <summary>
    /// 资源加载完成的回调 LoadAssetSuccessCallback
    /// </summary>
    public delegate void LoadAssetOverCallback(LoadAssetData assetObject, Object asset);

    /// <summary>
    /// 资源卸载完成的回调
    /// </summary>
    public delegate void UnloadAssetOverCallback(LoadAssetData assetObject);

    /// <summary>
    /// AB资源加载完成的回调
    /// </summary>
    public delegate void LoadAssetBundleOverCallBack(LoadABData abObject, AssetBundle ab);

    public partial class AssetManager
    {
        public LoadAssetComponent m_LoadAssetComponent;

        public LoadPrefabComponent m_LoadPrefabComponent;

        public LoadABComponent m_LoadABComponent;

        #region AB的申明

        /// <summary>
        /// 允许同时加载的AB包最大数量
        /// </summary>
        public const int MAX_LOADING_COUNT = 10;

        /// <summary>
        /// 创建临时存储变量
        /// 用于提升性能
        /// </summary>
        public List<LoadABData> m_TempLoadedsAB;

        /// <summary>
        /// WebGL的AB请求
        /// </summary>
        public UnityWebRequest m_WebGLRequestAB;

        /// <summary>
        /// 全局依赖资源
        /// 记录了Manifest中所有AB的依赖资源关系
        /// </summary>
        public Dictionary<string, string[]> m_DependsDataABList;

        public Dictionary<string, string[]> DependsDataABList
        {
            get { return m_DependsDataABList; }
        }

        /// <summary>
        /// 预备加载的列表
        /// </summary>
        public Dictionary<string, LoadABData> m_ReadyABList;

        public Dictionary<string, LoadABData> ReadyABList
        {
            get { return m_ReadyABList; }
        }

        /// <summary>
        /// 正在加载的列表
        /// 用于存放异步加载的AB封装对象
        /// </summary>
        public Dictionary<string, LoadABData> m_LoadingABList;

        public Dictionary<string, LoadABData> LoadingABList
        {
            get { return m_LoadingABList; }
        }

        /// <summary>
        /// 加载完成的列表
        /// </summary>
        public Dictionary<string, LoadABData> m_LoadedABList;

        public Dictionary<string, LoadABData> LoadedABList
        {
            get { return m_LoadedABList; }
        }

        /// <summary>
        /// 准备卸载的列表
        /// </summary>
        public Dictionary<string, LoadABData> m_UnloadABList;

        public Dictionary<string, LoadABData> UnloadABList
        {
            get { return m_UnloadABList; }
        }

        /// <summary>
        /// AB格式化路径缓存
        /// </summary>
        public Dictionary<string, string> m_ABFormatPathCaches;

        #endregion

        #region Asset的申明

        /// <summary>
        /// 卸载最低延迟帧数
        /// Asset资源过期帧数，60*60：相当于1分钟
        /// </summary>
        public int m_UnloadAssetDelayFrameNum = 60 * 60;

        public int UnloadAssetDelayFrameNum
        {
            set { m_UnloadAssetDelayFrameNum = value; }
        }

        /// <summary>
        /// 每轮资源清理所需要加载完成资源的累计个数
        /// </summary>
        public int m_LoadedMaxNumToCleanMemery = 50;

        public int LoadedMaxNumToCleanMemery
        {
            set { m_LoadedMaxNumToCleanMemery = value; }
        }

        /// <summary>
        /// 创建临时存储变量
        /// 用于提升性能
        /// </summary>
        public List<LoadAssetData> m_TempAssetLoadeds =
            new List<LoadAssetData>();

        /// <summary>
        /// 正在加载的列表
        /// 用于存放异步加载的Asset封装对象
        /// </summary>
        public Dictionary<string, LoadAssetData> m_LoadingAssetList;

        public Dictionary<string, LoadAssetData> LoadingAssetList
        {
            get { return m_LoadingAssetList; }
        }

        /// <summary>
        /// 加载完成的列表
        /// </summary>
        public Dictionary<string, LoadAssetData> m_LoadedAssetList;

        public Dictionary<string, LoadAssetData> LoadedAssetList
        {
            get { return m_LoadedAssetList; }
        }

        /// <summary>
        /// 异步预加载队列
        /// 用于空闲时加载
        /// </summary>
        public  Queue<PreloadAssetData> m_PreloadedAsyncAssetList;

        public  Queue<PreloadAssetData> PreloadedAsyncAssetList
        {
            get { return m_PreloadedAsyncAssetList; }
        }

        /// <summary>
        /// 准备卸载的列表
        /// </summary>
        public  Dictionary<string, LoadAssetData> m_UnloadAssetList;

        public  Dictionary<string, LoadAssetData> UnloadAssetList
        {
            get { return m_UnloadAssetList; }
        }

        /// <summary>
        /// 异步加载临时中转列表
        /// 用于延迟回调
        /// </summary>
        public  List<LoadAssetData> m_LoadedAsyncTmpAgentAssetList;

        /// <summary>
        /// 所有Asset资源instanceID所对应的Asset封装对象
        /// 注意：Scene是不包含在内的！
        /// </summary>
        public  Dictionary<int, LoadAssetData> m_AssetInstanceIDAssetList;

        /// <summary>
        /// 当前激活正在使用的Scene集合
        /// 不包括Launching.unity
        /// </summary>
        public  List<LoadAssetData> m_ScenesAsset;

        public  List<LoadAssetData> ScenesAsset
        {
            get { return m_ScenesAsset; }
        }

        /// <summary>
        /// 异步加载完成的资源数量计数器
        /// 每次达到上限后数据会自动清零等待下一次上限
        /// </summary>
        public  int m_LoadingIntervalCount;

        /// <summary>
        /// 编辑器资源模式
        /// </summary>
        public  bool m_EditorResourceMode;

        #endregion

        #region Prefab

        /// <summary>
        /// 加载完成列表
        /// </summary>
        public  Dictionary<string, LoadPrefabData> m_LoadedPrefabList;

        public  Dictionary<string, LoadPrefabData> LoadedPrefabList
        {
            get { return m_LoadedPrefabList; }
        }

        /// <summary>
        /// 异步加载临时中转列表
        /// 用于延迟回调
        /// 当调用异步加载时由于之前已经同步加载过了，所以为了遵循异步加载的异步回调规则，这里临时记录，方便接下来的异步回调
        /// </summary>
        public  List<LoadPrefabData> m_LoadedPrefabAsyncTmpAgentList;

        /// <summary>
        /// 创建的所有实例的实例ID所对应的Prefab封装对象
        /// </summary>
        public  Dictionary<int, LoadPrefabData> m_GOInstanceIDPrefabList;

        public  Dictionary<int, LoadPrefabData> GOInstanceIDPrefabList
        {
            get { return m_GOInstanceIDPrefabList; }
        }

        #endregion

        #region AssetUtils

        /// <summary>
        /// 是否为编辑器资源模式
        /// </summary>
        /// <returns></returns>
        public  bool EditorResourceMode
        {
            get
            {
                if (LauncherManager.Instance != null)
                {
                    return LauncherManager.Instance.EditorResourceMode;
                }

                return false;
            }
        }

        /// <summary>
        /// 获取当前所有激活的场景集合
        /// </summary>
        public  List<LoadAssetData> Scenes
        {
            get { return ScenesAsset; }
        }

        /// <summary>
        /// 是否严格检测
        /// </summary>
        public  bool StrictCheck
        {
            set { GameConfig.instance.m_StrictCheck = value; }
            get { return GameConfig.instance.m_StrictCheck; }
        }

        #endregion
    }
}