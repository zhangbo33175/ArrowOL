/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * -------------------------------------------------------------
 * filename:  SceneManager.cs
 * author:  云毅
 * created:
 * descrip:   场景底层管理器 - 核心驱动：异步/同步加载、卸载、预加载、状态防呆
 ***************************************************************/

using System.Collections.Generic;

namespace Honor.Runtime
{
    /// <summary>
    /// 场景管理器（底层核心）
    /// 功能：场景异步/同步加载、卸载、预加载、状态管理、重复加载防呆
    /// 基于 AssetComponent 实现，是 SceneComponent 的底层驱动
    /// </summary>
    public sealed partial class SceneManager
    {
        #region 公共属性
        //=========================================================================

        /// <summary>
        /// 构造函数：初始化状态列表 + 获取资源组件
        /// </summary>
        public SceneManager()
        {
            // 初始化所有场景状态队列
            m_PreLoadSceneAssetNames = new List<List<string>>();
            m_LoadingSceneAssetNames = new List<List<string>>();
            m_LoadedSceneAssetNames = new List<List<string>>();
            m_UnloadingSceneAssetNames = new List<List<string>>();

            // 获取全局资源管理组件
            m_AssetComponent = GameComponentsGroup.GetComponent<AssetComponent>();
            if (m_AssetComponent == null)
            {
                Log.Fatal("Asset component 无效。");
                return;
            }
        }

        #endregion

        //=========================================================================
        #region 场景加载 / 卸载
        //=========================================================================

        /// <summary>
        /// 异步预加载场景（后台加载，不激活）
        /// </summary>
        /// <param name="abPath">AB包路径</param>
        /// <param name="assetName">场景名称</param>
        public void PreLoadSceneAsync(string abPath, string assetName)
        {
            if (string.IsNullOrEmpty(abPath))
            {
                throw new GameException("Scene abPath 无效。");
            }

            if (string.IsNullOrEmpty(assetName))
            {
                throw new GameException("Scene assetName 无效。");
            }

            // 加入预加载队列
            m_PreLoadSceneAssetNames.Add(new List<string> { abPath, assetName });

            // 调用资源组件异步预加载
            m_AssetComponent.PreLoadSceneAsync(abPath, assetName, (AssetObject assetObject, UnityEngine.Object asset) =>
            {
                // 加载完成：移出预加载队列，加入已加载队列
                RemoveAssetNamesFromList(m_PreLoadSceneAssetNames, abPath, assetName);
                m_LoadedSceneAssetNames.Add(new List<string> { abPath, assetName });
            }, true);
        }

        /// <summary>
        /// 异步加载场景（可回调）
        /// 自带防重复加载/卸载中判断
        /// </summary>
        public void LoadSceneAsync(string abPath, string assetName, SceneLoadOverCallback overCallback = null)
        {
            if (string.IsNullOrEmpty(abPath))
                throw new GameException("Scene abPath 无效。");
            if (string.IsNullOrEmpty(assetName))
                throw new GameException("Scene assetName 无效。");

            // 防呆：正在卸载 / 正在加载 / 已加载 → 直接抛异常
            if (SceneIsUnloading(abPath, assetName))
                throw new GameException($"场景 {abPath}/{assetName} 正在卸载中，无法加载");
            if (SceneIsLoading(abPath, assetName))
                throw new GameException($"场景 {abPath}/{assetName} 正在加载中，不可重复加载");
            if (SceneIsLoaded(abPath, assetName))
                throw new GameException($"场景 {abPath}/{assetName} 已加载，不可重复加载");

            // 加入加载队列
            m_LoadingSceneAssetNames.Add(new List<string> { abPath, assetName });

            // 异步加载场景
            m_AssetComponent.LoadSceneAsync(abPath, assetName, (AssetObject assetObject, UnityEngine.Object asset) =>
            {
                // 加载完成：更新队列状态
                RemoveAssetNamesFromList(m_PreLoadSceneAssetNames, abPath, assetName);
                RemoveAssetNamesFromList(m_LoadingSceneAssetNames, abPath, assetName);
                m_LoadedSceneAssetNames.Add(new List<string> { abPath, assetName });

                // 执行外部回调
                overCallback?.Invoke(abPath, assetName,
                    UnityEngine.SceneManagement.SceneManager.GetSceneByName(assetName));
            });
        }

        /// <summary>
        /// 同步加载场景（阻塞主线程）
        /// </summary>
        public UnityEngine.SceneManagement.Scene LoadSceneSync(string abPath, string assetName)
        {
            if (string.IsNullOrEmpty(abPath))
                throw new GameException("Scene abPath 无效。");
            if (string.IsNullOrEmpty(assetName))
                throw new GameException("Scene assetName 无效。");

            // 防重复加载判断
            if (SceneIsUnloading(abPath, assetName))
                throw new GameException($"场景 {abPath}/{assetName} 正在卸载");
            if (SceneIsLoading(abPath, assetName))
                throw new GameException($"场景 {abPath}/{assetName} 正在加载");
            if (SceneIsLoaded(abPath, assetName))
                throw new GameException($"场景 {abPath}/{assetName} 已加载");

            // 清理预加载队列
            RemoveAssetNamesFromList(m_PreLoadSceneAssetNames, abPath, assetName);

            // 加入已加载队列并同步加载
            m_LoadedSceneAssetNames.Add(new List<string> { abPath, assetName });
            m_AssetComponent.LoadSceneSync(abPath, assetName);

            // 返回加载完成的场景
            return UnityEngine.SceneManagement.SceneManager.GetSceneByName(assetName);
        }

        /// <summary>
        /// 异步卸载场景
        /// </summary>
        public void UnloadScene(string sceneName, SceneUnloadOverCallback overCallback = null)
        {
            if (string.IsNullOrEmpty(sceneName))
                throw new GameException("Scene sceneName 无效。");

            // 只有已加载的场景才能卸载
            if (IsAssetNameExistInList(m_LoadedSceneAssetNames, sceneName))
            {
                // 防止重复卸载
                if (!IsAssetNameExistInList(m_UnloadingSceneAssetNames, sceneName))
                {
                    m_UnloadingSceneAssetNames.Add(new List<string>() { sceneName });

                    // 调用资源组件卸载
                    m_AssetComponent.UnloadScene(sceneName, (AssetObject assetObject) =>
                    {
                        // 卸载完成：更新队列
                        RemoveAssetNamesFromList(m_LoadedSceneAssetNames, assetObject.AssetBundlePath,
                            assetObject.AssetName);
                        RemoveAssetNamesFromList(m_UnloadingSceneAssetNames, assetObject.AssetBundlePath,
                            assetObject.AssetName);

                        // 执行回调
                        overCallback?.Invoke(assetObject.AssetBundlePath, assetObject.AssetName);
                    });
                }
            }
        }

        #endregion

        //=========================================================================
        #region 场景状态检查
        //=========================================================================

        /// <summary>
        /// 检查场景是否已加载
        /// </summary>
        public bool SceneIsLoaded(string abPath, string assetName)
        {
            if (string.IsNullOrEmpty(abPath) || string.IsNullOrEmpty(assetName))
                return false;

            foreach (var item in m_LoadedSceneAssetNames)
            {
                if (item[0] == abPath && item[1] == assetName)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 检查场景是否正在加载
        /// </summary>
        public bool SceneIsLoading(string abPath, string assetName)
        {
            if (string.IsNullOrEmpty(abPath) || string.IsNullOrEmpty(assetName))
                return false;

            foreach (var item in m_LoadingSceneAssetNames)
            {
                if (item[0] == abPath && item[1] == assetName)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 检查场景是否正在卸载
        /// </summary>
        public bool SceneIsUnloading(string abPath, string assetName)
        {
            if (string.IsNullOrEmpty(abPath) || string.IsNullOrEmpty(assetName))
                return false;

            foreach (var item in m_UnloadingSceneAssetNames)
            {
                if (item[0] == assetName)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 检查场景资源是否存在（配置表/AB包）
        /// </summary>
        public bool HasScene(string abPath, string assetName)
        {
            return m_AssetComponent.IsAssetExist("Scene", abPath, assetName);
        }

        #endregion

        //=========================================================================
        #region 状态列表获取
        //=========================================================================

        public List<List<string>> GetLoadedSceneAssetNames() => m_LoadedSceneAssetNames;

        public void GetLoadedSceneAssetNames(List<List<string>> results)
        {
            results.Clear();
            results.AddRange(m_LoadedSceneAssetNames);
        }

        public List<List<string>> GetLoadingSceneAssetNames() => m_LoadingSceneAssetNames;

        public void GetLoadingSceneAssetNames(List<List<string>> results)
        {
            results.Clear();
            results.AddRange(m_LoadingSceneAssetNames);
        }

        public List<List<string>> GetUnloadingSceneAssetNames() => m_UnloadingSceneAssetNames;

        public void GetUnloadingSceneAssetNames(List<List<string>> results)
        {
            results.Clear();
            results.AddRange(m_UnloadingSceneAssetNames);
        }

        #endregion
    }
}