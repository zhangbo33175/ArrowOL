using System.Collections.Generic;

namespace Honor.Runtime
{
    public sealed partial class SceneManager
    {
        /// <summary>
        /// Scene管理器构造
        /// </summary>
        public SceneManager()
        {
            m_PreLoadSceneAssetNames = new List<List<string>>();
            m_LoadingSceneAssetNames = new List<List<string>>();
            m_LoadedSceneAssetNames = new List<List<string>>();
            m_UnloadingSceneAssetNames = new List<List<string>>();

            m_AssetComponent = GameComponentsGroup.GetComponent<AssetComponent>();
            if (m_AssetComponent == null)
            {
                Log.Fatal("Asset component 无效。");
                return;
            }
        }

        /// <summary>
        /// 异步预加载场景。
        /// </summary>
        /// <param name="abPath">ab路径。</param>
        /// <param name="assetName">asset资源名称。</param>
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

            // 注意：preload队列中的scene通过AssetComponent加载完毕后将直接记录到loaded队列中，中间态loading将不做表现。
            m_PreLoadSceneAssetNames.Add(new List<string> { abPath, assetName });
            m_AssetComponent.PreLoadSceneAsync(abPath, assetName, (AssetObject assetObject, UnityEngine.Object asset) =>
            {
                RemoveAssetNamesFromList(m_PreLoadSceneAssetNames, abPath, assetName);
                m_LoadedSceneAssetNames.Add(new List<string> { abPath, assetName });
            }, true);
        }

        /// <summary>
        /// 异步加载场景。
        /// </summary>
        /// <param name="abPath">ab路径。</param>
        /// <param name="assetName">asset资源名称。</param>
        /// <param name="overCallback">场景加载完毕回调。</param>
        public void LoadSceneAsync(string abPath, string assetName, SceneLoadOverCallback overCallback = null)
        {
            if (string.IsNullOrEmpty(abPath))
            {
                throw new GameException("Scene abPath 无效。");
            }

            if (string.IsNullOrEmpty(assetName))
            {
                throw new GameException("Scene assetName 无效。");
            }

            if (SceneIsUnloading(abPath, assetName))
            {
                throw new GameException(AorTxt.Format("Scene asset abPath = '{0}' assetName = '{1}' 正在被卸载。", abPath, assetName));
            }

            if (SceneIsLoading(abPath, assetName))
            {
                throw new GameException(AorTxt.Format("Scene asset abPath = '{0}' assetName = '{1}' 正在被加载。", abPath, assetName));
            }

            if (SceneIsLoaded(abPath, assetName))
            {
                throw new GameException(AorTxt.Format("Scene asset abPath = '{0}' assetName = '{1}' 已经被加载。", abPath, assetName));
            }

            m_LoadingSceneAssetNames.Add(new List<string> { abPath, assetName });
            m_AssetComponent.LoadSceneAsync(abPath, assetName, (AssetObject assetObject, UnityEngine.Object asset) =>
            {
                RemoveAssetNamesFromList(m_PreLoadSceneAssetNames, abPath, assetName);
                RemoveAssetNamesFromList(m_LoadingSceneAssetNames, abPath, assetName);
                m_LoadedSceneAssetNames.Add(new List<string> { abPath, assetName });
                if (overCallback != null)
                {
                    overCallback(abPath, assetName, UnityEngine.SceneManagement.SceneManager.GetSceneByName(assetName));
                }
            });

        }

        /// <summary>
        /// 同步加载场景。
        /// </summary>
        /// <param name="abPath">ab路径。</param>
        /// <param name="assetName">asset资源名称。</param>
        public UnityEngine.SceneManagement.Scene LoadSceneSync(string abPath, string assetName)
        {
            if (string.IsNullOrEmpty(abPath))
            {
                throw new GameException("Scene abPath 无效。");
            }

            if (string.IsNullOrEmpty(assetName))
            {
                throw new GameException("Scene assetName 无效。");
            }

            if (SceneIsUnloading(abPath, assetName))
            {
                throw new GameException(AorTxt.Format("Scene asset abPath = '{0}' assetName = '{1}' 正在被卸载。", abPath, assetName));
            }

            if (SceneIsLoading(abPath, assetName))
            {
                throw new GameException(AorTxt.Format("Scene asset abPath = '{0}' assetName = '{1}' 正在被加载。", abPath, assetName));
            }

            if (SceneIsLoaded(abPath, assetName))
            {
                throw new GameException(AorTxt.Format("Scene asset abPath = '{0}' assetName = '{1}' 已经被加载。", abPath, assetName));
            }

            RemoveAssetNamesFromList(m_PreLoadSceneAssetNames, abPath, assetName);

            m_LoadedSceneAssetNames.Add(new List<string> { abPath, assetName });
            m_AssetComponent.LoadSceneSync(abPath, assetName);

            return UnityEngine.SceneManagement.SceneManager.GetSceneByName(assetName);
        }

        /// <summary>
        /// 卸载场景。
        /// </summary>
        /// <param name="sceneName">场景名称。</param>
        /// <param name="overCallback">场景卸载完毕回调。</param>
        public void UnloadScene(string sceneName, SceneUnloadOverCallback overCallback = null)
        {
            if (string.IsNullOrEmpty(sceneName))
            {
                throw new GameException("Scene sceneName 无效。");
            }

            if(IsAssetNameExistInList(m_LoadedSceneAssetNames, sceneName))
            {
                if(!IsAssetNameExistInList(m_UnloadingSceneAssetNames, sceneName))
                {
                    m_UnloadingSceneAssetNames.Add(new List<string>() { sceneName });
                    m_AssetComponent.UnloadScene(sceneName, (AssetObject assetObject) =>
                    {
                        RemoveAssetNamesFromList(m_LoadedSceneAssetNames, assetObject.AssetBundlePath, assetObject.AssetName);
                        RemoveAssetNamesFromList(m_UnloadingSceneAssetNames, assetObject.AssetBundlePath, assetObject.AssetName);
                        if (overCallback != null)
                        {
                            overCallback(assetObject.AssetBundlePath, assetObject.AssetName);
                        }
                    });
                }
            }
        }

        /// <summary>
        /// 获取场景是否已加载。
        /// </summary>
        /// <param name="abPath">AB路径。</param>
        /// <param name="assetName">Asset资源名称。</param>
        /// <returns>场景是否已加载。</returns>
        public bool SceneIsLoaded(string abPath, string assetName)
        {
            if (string.IsNullOrEmpty(abPath))
            {
                throw new GameException("Scene abPath 无效。");
            }

            if (string.IsNullOrEmpty(assetName))
            {
                throw new GameException("Scene assetName 无效。");
            }

            for (int index = 0; index < m_LoadedSceneAssetNames.Count; index++)
            {
                if (!string.IsNullOrEmpty(m_LoadedSceneAssetNames[index][0]) && !string.IsNullOrEmpty(m_LoadedSceneAssetNames[index][1]))
                {
                    if (m_LoadedSceneAssetNames[index][0] == abPath && m_LoadedSceneAssetNames[index][1] == assetName)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 获取已加载场景的资源名称。
        /// </summary>
        /// <returns>已加载场景的资源名称。</returns>
        public List<List<string>> GetLoadedSceneAssetNames()
        {
            return m_LoadedSceneAssetNames;
        }

        /// <summary>
        /// 获取已加载场景的资源名称。
        /// </summary>
        /// <param name="results">已加载场景的资源名称。</param>
        public void GetLoadedSceneAssetNames(List<List<string>> results)
        {
            if (results == null)
            {
                throw new GameException("Results 无效。");
            }

            results.Clear();
            results.AddRange(m_LoadedSceneAssetNames);
        }

        /// <summary>
        /// 获取场景是否正在加载。
        /// </summary>
        /// <param name="abPath">ab路径。</param>
        /// <param name="assetName">asset资源名称。</param>
        /// <returns>场景是否正在加载。</returns>
        public bool SceneIsLoading(string abPath, string assetName)
        {
            if (string.IsNullOrEmpty(abPath))
            {
                throw new GameException("Scene abPath 无效。");
            }

            if (string.IsNullOrEmpty(assetName))
            {
                throw new GameException("Scene assetName 无效。");
            }

            for (int index = 0; index < m_LoadingSceneAssetNames.Count; index++)
            {
                if (!string.IsNullOrEmpty(m_LoadingSceneAssetNames[index][0]) && !string.IsNullOrEmpty(m_LoadingSceneAssetNames[index][1]))
                {
                    if (m_LoadingSceneAssetNames[index][0] == abPath && m_LoadingSceneAssetNames[index][1] == assetName)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 获取正在加载场景的资源名称。
        /// </summary>
        /// <returns>正在加载场景的资源名称。</returns>
        public List<List<string>> GetLoadingSceneAssetNames()
        {
            return m_LoadingSceneAssetNames;
        }

        /// <summary>
        /// 获取正在加载场景的资源名称。
        /// </summary>
        /// <param name="results">正在加载场景的资源名称。</param>
        public void GetLoadingSceneAssetNames(List<List<string>> results)
        {
            if (results == null)
            {
                throw new GameException("Results 无效。");
            }

            results.Clear();
            results.AddRange(m_LoadingSceneAssetNames);
        }

        /// <summary>
        /// 获取场景是否正在卸载。
        /// </summary>
        /// <param name="abPath">ab路径。</param>
        /// <param name="assetName">asset资源名称。</param>
        /// <returns>场景是否正在卸载。</returns>
        public bool SceneIsUnloading(string abPath, string assetName)
        {
            if (string.IsNullOrEmpty(abPath))
            {
                throw new GameException("Scene abPath 无效。");
            }

            if (string.IsNullOrEmpty(assetName))
            {
                throw new GameException("Scene assetName 无效。");
            }

            for (int index = 0; index < m_UnloadingSceneAssetNames.Count; index++)
            {
                if (!string.IsNullOrEmpty(m_UnloadingSceneAssetNames[index][0]))
                {
                    if (m_UnloadingSceneAssetNames[index][0] == assetName)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 获取正在卸载场景的资源名称。
        /// </summary>
        /// <returns>正在卸载场景的资源名称。</returns>
        public List<List<string>> GetUnloadingSceneAssetNames()
        {
            return m_UnloadingSceneAssetNames;
        }

        /// <summary>
        /// 获取正在卸载场景的资源名称。
        /// </summary>
        /// <param name="results">正在卸载场景的资源名称。</param>
        public void GetUnloadingSceneAssetNames(List<List<string>> results)
        {
            if (results == null)
            {
                throw new GameException("Results 无效。");
            }

            results.Clear();
            results.AddRange(m_UnloadingSceneAssetNames);
        }

        /// <summary>
        /// 检查场景资源是否存在。
        /// </summary>
        /// <param name="abPath">ab路径。</param>
        /// <param name="assetName">asset资源名称。</param>
        /// <returns>场景资源是否存在。</returns>
        public bool HasScene(string abPath, string assetName)
        {
            return m_AssetComponent.IsAssetExist("Scene", abPath, assetName);
        }

    }
}


