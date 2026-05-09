using System;
using UnityEngine;
using XLua;

namespace Honor.Runtime
{
    /// <summary>
    /// 资源管理组件（运行时核心）
    /// 负责 AssetBundle / 普通资源的同步/异步加载、卸载、场景管理、Prefab 实例化
    /// 与 Lua 深度绑定，是游戏资源加载的唯一入口
    /// </summary>
    [DisallowMultipleComponent]
    public sealed partial class AssetComponent : GameComponent
    {
        /// <summary>
        /// 初始化资源管理器
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            // 获取启动器组件
            m_LauncherComponent = GameComponentsGroup.GetComponent<LauncherComponent>();
            if (m_LauncherComponent == null)
            {
                Log.Fatal("Launcher Component 无效。");
                return;
            }

            // 初始化资源加载管理器（负责底层资源加载/卸载）
            m_AssetLoadManager = new AssetLoadManager(m_LauncherComponent.EditorResourceMode,
                m_UnloadAssetDelayFrameNum, m_LoadedMaxNumToCleanMemery);
            if (m_AssetLoadManager == null)
            {
                Log.Fatal("AssetLoadManager 无效。");
                return;
            }

            // 初始化预制体加载管理器（负责实例化、克隆、Lua 传参）
            m_PrefabLoadManager = new PrefabLoadManager(m_AssetLoadManager);
            if (m_PrefabLoadManager == null)
            {
                Log.Fatal("PrefabLoadManager 无效。");
                return;
            }
        }

        /// <summary>
        /// 启动回调（当前无逻辑）
        /// </summary>
        private void Start()
        {
        }

        /// <summary>
        /// 每帧更新管理器（执行异步任务、延迟卸载、内存清理）
        /// </summary>
        private void Update()
        {
            if (m_AssetLoadManager != null)
            {
                m_AssetLoadManager.Update();
            }

            if (m_PrefabLoadManager != null)
            {
                m_PrefabLoadManager.Update();
            }
        }

        /// <summary
        /// 销毁回调（当前无逻辑）
        /// </summary>
        private void OnDestroy()
        {
        }

        /// <summary>
        /// 加载 AssetBundle 清单文件（非编辑器模式下必须调用）
        /// </summary>
        public void LoadManifest()
        {
            if (!m_LauncherComponent.EditorResourceMode)
            {
                if (m_AssetLoadManager != null)
                {
                    m_AssetLoadManager.AssetBundleLoadManager.LoadManifest();
                }
            }
        }

        /// <summary>
        /// 同步加载 Prefab 并自动实例化
        /// </summary>
        /// <param name="abPath">AB 包路径（必须以 Assets 开头）</param>
        /// <param name="assetName">资源名称</param>
        /// <param name="parent">父节点</param>
        /// <param name="luaParams">Lua 传入参数</param>
        /// <returns>实例化后的 GameObject</returns>
        public GameObject LoadPrefabSync(string abPath, string assetName, Transform parent, LuaTable luaParams = null)
        {
            if (string.IsNullOrEmpty(abPath))
            {
                Log.Error("AssetComponent.LoadPrefabSync abPath 无效。");
                return null;
            }

            if (!abPath.StartsWith("Assets"))
            {
                Log.Error("AssetComponent.LoadPrefabSync abPath {0} 未以Assets开头。", abPath);
                return null;
            }

            if (string.IsNullOrEmpty(assetName))
            {
                Log.Error("AssetComponent.LoadPrefabSync assetName 无效。");
                return null;
            }

            if (parent == null)
            {
                Log.Error("AssetComponent.LoadPrefabSync parent 无效。");
                return null;
            }

            return m_PrefabLoadManager.LoadSync(abPath, assetName, parent, luaParams);
        }

        /// <summary>
        /// 异步加载 Prefab 并自动实例化
        /// </summary>
        /// <param name="abPath">AB 包路径</param>
        /// <param name="assetName">资源名称</param>
        /// <param name="parent">父节点</param>
        /// <param name="luaParams">Lua 传参</param>
        /// <param name="overCallback">实例化完成回调</param>
        public void LoadPrefabAsync(string abPath, string assetName, Transform parent, LuaTable luaParams = null,
            PrefabLoadOverCallback overCallback = null)
        {
            if (overCallback == null)
            {
                Log.Error("AssetComponent.LoadPrefabAsync overCallback 无效。");
                return;
            }

            if (string.IsNullOrEmpty(abPath))
            {
                Log.Error("AssetComponent.LoadPrefabAsync abPath 无效。");
                return;
            }

            if (!abPath.StartsWith("Assets"))
            {
                Log.Error("AssetComponent.LoadPrefabAsync abPath: {0} 未以Assets开头。", abPath);
                return;
            }

            if (string.IsNullOrEmpty(assetName))
            {
                Log.Error("AssetComponent.LoadPrefabAsync assetName 无效。");
                return;
            }

            if (parent == null)
            {
                Log.Error("AssetComponent.LoadPrefabAsync parent 无效。");
                return;
            }

            m_PrefabLoadManager.LoadAsync(abPath, assetName, parent, luaParams, overCallback);
        }

        /// <summary>
        /// 直接克隆 GameObject（不经过资源加载流程）
        /// 注意：仅用于非托管模板对象，UI/场景对象禁止使用
        /// </summary>
        /// <param name="parent">父节点</param>
        /// <param name="childTemplateGO">模板对象</param>
        /// <param name="luaParams">Lua 参数</param>
        /// <returns>克隆后的对象</returns>
        public GameObject InstantiateGO(Transform parent, GameObject childTemplateGO, LuaTable luaParams = null)
        {
            if (parent == null)
            {
                Log.Error("AssetComponent.Instantiate parent 无效。");
                return null;
            }

            if (childTemplateGO == null)
            {
                Log.Error("AssetComponent.Instantiate childTemplateGO 无效。");
                return null;
            }

            return m_PrefabLoadManager.InstantiateGO(parent, childTemplateGO, luaParams);
        }

        /// <summary>
        /// 同步加载任意资源（非实例化，仅加载原始资源）
        /// </summary>
        /// <param name="typeName">资源类型名</param>
        /// <param name="abPath">AB 路径</param>
        /// <param name="assetName">资源名</param>
        /// <returns>UnityEngine.Object 资源</returns>
        public UnityEngine.Object LoadAssetSync(string typeName, string abPath, string assetName)
        {
            if (string.IsNullOrEmpty(typeName))
            {
                return null;
            }

            if (string.IsNullOrEmpty(abPath))
            {
                Log.Error("AssetComponent.LoadAssetSync abPath 无效。");
                return null;
            }

            if (!abPath.StartsWith("Assets"))
            {
                Log.Error("AssetComponent.LoadAssetSync abPath {0} 未以Assets开头。", abPath);
                return null;
            }

            if (string.IsNullOrEmpty(assetName))
            {
                Log.Error("AssetComponent.LoadAssetSync assetName 无效。");
                return null;
            }

            return m_AssetLoadManager.LoadSync(typeName, abPath, assetName);
        }

        /// <summary>
        /// 异步加载任意资源
        /// </summary>
        /// <param name="typeName">类型名</param>
        /// <param name="abPath">AB 路径</param>
        /// <param name="assetName">资源名</param>
        /// <param name="overCallback">加载完成回调</param>
        public void LoadAssetAsync(string typeName, string abPath, string assetName, AssetLoadOverCallback overCallback)
        {
            if (overCallback == null)
            {
                Log.Error("AssetComponent.LoadAssetAsync overCallback 无效。");
                return;
            }

            if (string.IsNullOrEmpty(typeName))
            {
                Log.Error("AssetComponent.LoadAssetAsync typeName 无效。");
                return;
            }

            if (string.IsNullOrEmpty(abPath))
            {
                Log.Error("AssetComponent.LoadAssetAsync abPath 无效。");
                return;
            }

            if (!abPath.StartsWith("Assets"))
            {
                Log.Error("AssetComponent.LoadAssetAsync abPath: {0} 未以Assets开头。", abPath);
                return;
            }

            if (string.IsNullOrEmpty(assetName))
            {
                Log.Error("AssetComponent.LoadAssetAsync assetName 无效。");
                return;
            }

            m_AssetLoadManager.LoadAsync(typeName, abPath, assetName, overCallback);
        }

        /// <summary>
        /// 异步预加载资源（可设置弱引用）
        /// </summary>
        /// <param name="typeName">类型名</param>
        /// <param name="abPath">AB 路径</param>
        /// <param name="assetName">资源名</param>
        /// <param name="overCallback">回调</param>
        /// <param name="isWeak">是否弱引用（true=用完自动释放）</param>
        public void PreLoadAssetAsync(string typeName, string abPath, string assetName,
            AssetLoadOverCallback overCallback, bool isWeak = true)
        {
            if (string.IsNullOrEmpty(typeName))
            {
                Log.Error("AssetComponent.PreLoadAssetAsync typeName 无效。");
                return;
            }

            if (string.IsNullOrEmpty(abPath))
            {
                Log.Error("AssetComponent.PreLoadAssetAsync abPath 无效。");
                return;
            }

            if (!abPath.StartsWith("Assets"))
            {
                Log.Error("AssetComponent.PreLoadAssetAsync abPath: {0} 未以Assets开头。", abPath);
                return;
            }

            if (string.IsNullOrEmpty(assetName))
            {
                Log.Error("AssetComponent.PreLoadAssetAsync assetName 无效。");
                return;
            }

            m_AssetLoadManager.PreLoadAsync(typeName, abPath, assetName, overCallback, isWeak);
        }

        /// <summary>
        /// 卸载资源
        /// </summary>
        /// <param name="asset">资源对象</param>
        /// <param name="overCallback">卸载完成回调</param>
        /// <param name="rightNow">是否立即卸载</param>
        public void UnloadAsset(UnityEngine.Object asset, AssetUnloadOverCallback overCallback = null,
            bool rightNow = false)
        {
            if (asset == null)
            {
                Log.Error("AssetComponent.UnloadAsset asset 无效。");
                return;
            }

            m_AssetLoadManager.Unload(asset, overCallback, rightNow);
        }

        /// <summary>
        /// 强制卸载所有未使用资源（异步）
        /// 注意：开销大，必须谨慎使用，适合场景切换时调用
        /// </summary>
        /// <param name="overCallback">完成回调</param>
        public void ForceUnloadUnusedAssets(Action overCallback = null)
        {
            m_AssetLoadManager.ForceUnloadUnusedAssets(overCallback);
        }

        /// <summary>
        /// 同步加载场景
        /// </summary>
        /// <param name="abPath">AB 路径</param>
        /// <param name="sceneName">场景名</param>
        public void LoadSceneSync(string abPath, string sceneName)
        {
            if (string.IsNullOrEmpty(abPath))
            {
                Log.Error("AssetComponent.LoadSceneSync abPath 无效。");
                return;
            }

            if (!abPath.StartsWith("Assets"))
            {
                Log.Error("AssetComponent.LoadSceneSync abPath: {0} 未以Assets开头。", abPath);
                return;
            }

            if (string.IsNullOrEmpty(sceneName))
            {
                Log.Error("AssetComponent.LoadSceneSync sceneName 无效。");
                return;
            }

            m_AssetLoadManager.LoadSync("Scene", abPath, sceneName);
        }

        /// <summary>
        /// 异步加载场景
        /// </summary>
        /// <param name="abPath">AB 路径</param>
        /// <param name="sceneName">场景名</param>
        /// <param name="overCallback">加载完成回调</param>
        public void LoadSceneAsync(string abPath, string sceneName, AssetLoadOverCallback overCallback)
        {
            if (overCallback == null)
            {
                Log.Error("AssetComponent.LoadSceneAsync overCallback 无效。");
                return;
            }

            if (string.IsNullOrEmpty(abPath))
            {
                Log.Error("AssetComponent.LoadSceneAsync abPath 无效。");
                return;
            }

            if (!abPath.StartsWith("Assets"))
            {
                Log.Error("AssetComponent.LoadSceneAsync abPath: {0} 未以Assets开头。", abPath);
                return;
            }

            if (string.IsNullOrEmpty(sceneName))
            {
                Log.Error("AssetComponent.LoadSceneAsync sceneName 无效。");
                return;
            }

            m_AssetLoadManager.LoadAsync("Scene", abPath, sceneName, overCallback);
        }

        /// <summary>
        /// 异步预加载场景
        /// </summary>
        /// <param name="abPath">AB 路径</param>
        /// <param name="sceneName">场景名</param>
        /// <param name="overCallback">回调</param>
        /// <param name="isWeak">弱引用</param>
        public void PreLoadSceneAsync(string abPath, string sceneName, AssetLoadOverCallback overCallback,
            bool isWeak = true)
        {
            if (string.IsNullOrEmpty(abPath))
            {
                Log.Error("AssetComponent.PreLoadSceneAsync abPath 无效。");
                return;
            }

            if (!abPath.StartsWith("Assets"))
            {
                Log.Error("AssetComponent.PreLoadSceneAsync abPath: {0} 未以Assets开头。", abPath);
                return;
            }

            if (string.IsNullOrEmpty(sceneName))
            {
                Log.Error("AssetComponent.PreLoadSceneAsync sceneName 无效。");
                return;
            }

            m_AssetLoadManager.PreLoadAsync("Scene", abPath, sceneName, overCallback, isWeak);
        }

        /// <summary>
        /// 卸载场景
        /// </summary>
        /// <param name="sceneName">场景名</param>
        /// <param name="overCallback">完成回调</param>
        public void UnloadScene(string sceneName, AssetUnloadOverCallback overCallback = null)
        {
            if (string.IsNullOrEmpty(sceneName))
            {
                Log.Error("AssetComponent.UnloadScene sceneName 无效。");
                return;
            }

            UnityEngine.SceneManagement.Scene
                scene = UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneName);

            if (scene == null)
            {
                Log.Error("AssetComponent.UnloadScene scene 无效。");
                return;
            }

            m_AssetLoadManager.Unload(scene, overCallback);
        }

        /// <summary>
        /// 判断资源是否存在
        /// </summary>
        /// <param name="typeName">类型名</param>
        /// <param name="abPath">AB 路径</param>
        /// <param name="assetName">资源名</param>
        /// <returns>是否存在</returns>
        public bool IsAssetExist(string typeName, string abPath, string assetName)
        {
            if (string.IsNullOrEmpty(typeName))
            {
                return false;
            }

            if (string.IsNullOrEmpty(abPath))
            {
                return false;
            }

            if (string.IsNullOrEmpty(assetName))
            {
                return false;
            }

            return m_AssetLoadManager.IsFileExist(typeName, abPath, assetName);
        }
    }
}