using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Honor.Runtime
{
    public sealed partial class AssetLoadManager
    {
        #region 构造函数
        /// <summary>
        /// 资源加载管理器构造方法
        /// </summary>
        /// <param name="editorResourceMode">是否为编辑器资源模式</param>
        /// <param name="unloadAssetDelayFrameNum">资源延迟卸载帧数</param>
        /// <param name="loadedMaxNumToCleanMemery">触发内存清理的最大资源数量</param>
        public AssetLoadManager(bool editorResourceMode, int unloadAssetDelayFrameNum, int loadedMaxNumToCleanMemery)
        {
            m_EditorResourceMode = editorResourceMode;
            m_UnloadAssetDelayFrameNum = unloadAssetDelayFrameNum;
            m_LoadedMaxNumToCleanMemery = loadedMaxNumToCleanMemery;

            m_LoadingList = new Dictionary<string, AssetObject>();
            m_LoadedList = new Dictionary<string, AssetObject>();
            m_UnloadList = new Dictionary<string, AssetObject>();
            m_LoadedAsyncTmpAgentList = new List<AssetObject>();
            m_PreloadedAsyncList = new Queue<PreloadAssetObject>();
            m_AssetInstanceIDList = new Dictionary<int, AssetObject>();
            m_Scenes = new List<AssetObject>();

            m_AssetComponent = GameComponentsGroup.GetComponent<AssetComponent>();
            if (m_AssetComponent == null)
            {
                Log.Fatal("Asset Component 无效。");
                return;
            }

            _mAssetBundleLoadManager = new AssetBundleLoadManager();
        }
        #endregion

        #region 同步加载资源
        /// <summary>
        /// 同步加载资源
        /// 支持普通资源/场景，自动处理异步转同步、已加载、加载中等状态
        /// </summary>
        /// <param name="typeName">资源类型名称</param>
        /// <param name="abPath">AB包路径</param>
        /// <param name="assetName">资源名称</param>
        /// <returns>加载完成的资源对象</returns>
        public UnityEngine.Object LoadSync(string typeName, string abPath, string assetName)
        {
            string assetPath = GetAssetPath(typeName, abPath, assetName);
            if (!IsFileExist(typeName, abPath, assetName))
            {
                if (m_AssetComponent.StrictCheck)
                {
                    Log.Error("AssetLoadManager 资源文件不存在 : {0}", assetPath);
                }

                return null;
            }

            AssetObject assetObj = null;
            if (m_LoadedList.ContainsKey(assetPath))
            {
                assetObj = m_LoadedList[assetPath];
                assetObj.RefCount++;
                return assetObj.Asset;
            }
            else if (m_LoadingList.ContainsKey(assetPath))
            {
                assetObj = m_LoadingList[assetPath];

                // 异步加载未完成，直接提取资源（异步转同步）
                if (assetObj.Request != null)
                {
                    if (assetObj.Request is AssetBundleRequest)
                    {
                        assetObj.Asset = (assetObj.Request as AssetBundleRequest).asset;
                    }

                    assetObj.Request = null;
                }
                else // 异步加载已完成，重新加载
                {
                    if (m_EditorResourceMode)
                    {
#if UNITY_EDITOR
                        if (assetObj.IsScene)
                        {
                            UnityEngine.SceneManagement.SceneManager.LoadScene(assetName,
                                UnityEngine.SceneManagement.LoadSceneMode.Additive);
                        }
                        else
                        {
                            string assetRelativeFullPath = GetAssetRelativeFullPath(typeName, abPath, assetName);
                            Type assetType = Assembly.GetType(AorTxt.GetTypeStringByName(typeName));
                            if (assetType != null)
                            {
                                assetObj.Asset =
                                    UnityEditor.AssetDatabase.LoadAssetAtPath(assetRelativeFullPath, assetType);
                            }
                            else
                            {
                                assetObj.Asset = UnityEditor.AssetDatabase.LoadAssetAtPath(assetRelativeFullPath,
                                    typeof(UnityEngine.Object));
                            }
                        }
#endif
                    }
                    else
                    {
                        AssetBundle ab = _mAssetBundleLoadManager.LoadSync(abPath);

                        if (assetObj.IsScene)
                        {
                            UnityEngine.SceneManagement.SceneManager.LoadScene(assetName,
                                UnityEngine.SceneManagement.LoadSceneMode.Additive);
                        }
                        else
                        {
                            Type assetType = Assembly.GetType(AorTxt.GetTypeStringByName(typeName));
                            if (assetType != null)
                            {
                                assetObj.Asset = ab.LoadAsset(assetName, assetType);
                            }
                            else
                            {
                                assetObj.Asset = ab.LoadAsset(assetName);
                            }
                        }

                        // 修正异步转同步带来的额外引用计数
                        _mAssetBundleLoadManager.Unload(abPath);
                    }
                }

                if (assetObj.IsScene)
                {
                    m_Scenes.Add(assetObj);
                }
                else
                {
                    if (assetObj.Asset == null)
                    {
                        m_LoadingList.Remove(assetObj.AssetPath);
                        if (m_AssetComponent.StrictCheck)
                        {
                            Log.Error("AssetLoadManager.LoadSync assetObj.Asset '{0}' 为空。", assetObj.AssetPath);
                        }

                        return null;
                    }

                    assetObj.InstanceID = assetObj.Asset.GetInstanceID();
                    if (!m_AssetInstanceIDList.ContainsKey(assetObj.InstanceID))
                    {
                        m_AssetInstanceIDList.Add(assetObj.InstanceID, assetObj);
                    }
                    else
                    {
                        Log.Error("AssetLoadManager.LoadSync assetObj.InstanceID '{0}' 已存在。Path:{1}",
                            assetObj.InstanceID, assetObj.AssetPath);
                    }
                }

                m_LoadingList.Remove(assetObj.AssetPath);
                m_LoadedList.Add(assetObj.AssetPath, assetObj);
                m_LoadedAsyncTmpAgentList.Add(assetObj);

                assetObj.RefCount++;
                return assetObj.Asset;
            }

            // 全新同步加载
            assetObj = new AssetObject();
            assetObj.TypeName = typeName;
            assetObj.AssetBundlePath = abPath;
            assetObj.AssetName = assetName;
            assetObj.AssetPath = assetPath;
            assetObj.IsScene = typeName.Equals("Scene");

            if (m_EditorResourceMode)
            {
#if UNITY_EDITOR
                if (assetObj.IsScene)
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene(assetName,
                        UnityEngine.SceneManagement.LoadSceneMode.Additive);
                }
                else
                {
                    string assetRelativeFullPath = GetAssetRelativeFullPath(typeName, abPath, assetName);
                    Type assetType = Assembly.GetType(AorTxt.GetTypeStringByName(typeName));
                    if (assetType != null)
                    {
                        assetObj.Asset = UnityEditor.AssetDatabase.LoadAssetAtPath(assetRelativeFullPath, assetType);
                    }
                    else
                    {
                        assetObj.Asset =UnityEditor.AssetDatabase.LoadAssetAtPath(assetRelativeFullPath,typeof(UnityEngine.Object));
                    }
                }
#endif
                assetObj.Origin = OriginType.Editor;
            }
            else
            {
                if (_mAssetBundleLoadManager.IsABExist(abPath))
                {
                    AssetBundle ab = _mAssetBundleLoadManager.LoadSync(abPath);
                    if (assetObj.IsScene)
                    {
                        UnityEngine.SceneManagement.SceneManager.LoadScene(assetName,
                            UnityEngine.SceneManagement.LoadSceneMode.Additive);
                    }
                    else
                    {
                        Type assetType = Assembly.GetType(AorTxt.GetTypeStringByName(typeName));
                        if (assetType != null)
                        {
                            assetObj.Asset = ab.LoadAsset(assetName, assetType);
                        }
                        else
                        {
                            assetObj.Asset = ab.LoadAsset(assetName);
                        }
                    }

                    assetObj.Origin = _mAssetBundleLoadManager
                        .LoadedAssetBundleList[_mAssetBundleLoadManager.GetABFormatPath(assetObj.AssetBundlePath)]
                        .Origin;
                }
            }

            if (assetObj.IsScene)
            {
                m_Scenes.Add(assetObj);
            }
            else
            {
                if (assetObj.Asset == null)
                {
                    if (m_AssetComponent.StrictCheck)
                    {
                        Log.Error("AssetLoadManager.LoadSync assetObj.Asset '{0}' 为空。", assetObj.AssetPath);
                    }

                    return null;
                }

                assetObj.InstanceID = assetObj.Asset.GetInstanceID();
                if (!m_AssetInstanceIDList.ContainsKey(assetObj.InstanceID))
                {
                    m_AssetInstanceIDList.Add(assetObj.InstanceID, assetObj);
                }
                else
                {
                    Log.Error("AssetLoadManager.LoadSync assetObj.InstanceID '{0}' 已存在。Path:{1}", assetObj.InstanceID,
                        assetObj.AssetPath);
                }
            }

            m_LoadedList.Add(assetPath, assetObj);
            assetObj.RefCount = 1;

            return assetObj.Asset;
        }
        #endregion

        #region 异步加载资源
        /// <summary>
        /// 异步加载资源
        /// 自动处理依赖、并发、回调合并
        /// </summary>
        /// <param name="typeName">资源类型</param>
        /// <param name="abPath">AB路径</param>
        /// <param name="assetName">资源名</param>
        /// <param name="overCallback">加载完成回调</param>
        public void LoadAsync(string typeName, string abPath, string assetName, AssetLoadOverCallback overCallback)
        {
            string assetPath = GetAssetPath(typeName, abPath, assetName);
            if (!IsFileExist(typeName, abPath, assetName))
            {
                if (m_AssetComponent.StrictCheck)
                {
                    Log.Error("AssetLoadManager Asset 文件不存在 : '{0}'。", assetPath);
                }

                return;
            }

            AssetObject assetObj = null;
            if (m_LoadedList.ContainsKey(assetPath))
            {
                assetObj = m_LoadedList[assetPath];
                assetObj.AssetLoadOverCallbackList.Add(overCallback);
                m_LoadedAsyncTmpAgentList.Add(assetObj);
                return;
            }
            else if (m_LoadingList.ContainsKey(assetPath))
            {
                assetObj = m_LoadingList[assetPath];
                assetObj.AssetLoadOverCallbackList.Add(overCallback);
                return;
            }

            // 新建异步加载对象
            assetObj = new AssetObject();
            assetObj.TypeName = typeName;
            assetObj.AssetBundlePath = abPath;
            assetObj.AssetName = assetName;
            assetObj.AssetPath = assetPath;
            assetObj.IsScene = typeName.Equals("Scene");
            assetObj.AssetLoadOverCallbackList.Add(overCallback);

            if (m_EditorResourceMode)
            {
                if (assetObj.IsScene)
                {
                    assetObj.Request = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(assetName,
                        UnityEngine.SceneManagement.LoadSceneMode.Additive);
                }

                assetObj.Origin = OriginType.Editor;
                m_LoadingList.Add(assetPath, assetObj);
            }
            else
            {
                if (_mAssetBundleLoadManager.IsABExist(abPath))
                {
                    m_LoadingList.Add(assetPath, assetObj);
                    _mAssetBundleLoadManager.LoadAsync(abPath, (AssetBundleObject abObject, AssetBundle ab) =>
                    {
                        if (ab == null)
                        {
                            if (m_AssetComponent.StrictCheck)
                            {
                                Log.Error("AssetLoadManager.LoadAsync异步加载错误！{0}", assetObj.AssetPath);
                            }

                            m_LoadingList.Remove(assetPath);
                            return;
                        }

                        if (m_LoadingList.ContainsKey(assetPath) && assetObj.Request == null)
                        {
                            if (assetObj.IsScene)
                            {
                                assetObj.Request = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(assetName,
                                    UnityEngine.SceneManagement.LoadSceneMode.Additive);
                            }
                            else
                            {
                                Type assetType = Assembly.GetType(AorTxt.GetTypeStringByName(typeName));
                                if (assetType != null)
                                {
                                    assetObj.Request = ab.LoadAssetAsync(assetName, assetType);
                                }
                                else
                                {
                                    assetObj.Request = ab.LoadAssetAsync(assetName);
                                }
                            }
                        }
                    });

                    AssetBundleObject assetBundleObjectTryGet;
                    string path = AssetBundleLoadManager.GetABFormatPath(assetObj.AssetBundlePath);
                    if (AssetBundleLoadManager.LoadedAssetBundleList.TryGetValue(path, out assetBundleObjectTryGet) ||
                        AssetBundleLoadManager.LoadingAssetBundleList.TryGetValue(path, out assetBundleObjectTryGet))
                    {
                        assetObj.Origin = assetBundleObjectTryGet.Origin;
                    }
                }
            }
        }
        #endregion

        #region 异步预加载资源
        /// <summary>
        /// 异步预加载资源
        /// 支持弱引用（自动卸载）/强引用（常驻内存）
        /// </summary>
        /// <param name="typeName">资源类型</param>
        /// <param name="abPath">AB路径</param>
        /// <param name="assetName">资源名</param>
        /// <param name="overCallback">完成回调</param>
        /// <param name="isWeak">是否为弱引用</param>
        public void PreLoadAsync(string typeName, string abPath, string assetName, AssetLoadOverCallback overCallback,
            bool isWeak = true)
        {
            string assetPath = GetAssetPath(typeName, abPath, assetName);
            AssetObject assetObj = null;
            if (m_LoadedList.ContainsKey(assetPath))
            {
                assetObj = m_LoadedList[assetPath];
            }
            else if (m_LoadingList.ContainsKey(assetPath))
            {
                assetObj = m_LoadingList[assetPath];
            }

            // 已存在则修改引用类型
            if (assetObj != null)
            {
                assetObj.IsWeak = isWeak;
                if (isWeak && assetObj.RefCount == 0 && !m_UnloadList.ContainsKey(assetPath))
                {
                    m_UnloadList.Add(assetPath, assetObj);
                }

                return;
            }

            // 新建预加载对象
            PreloadAssetObject plAssetObj = new PreloadAssetObject();
            plAssetObj.TypeName = typeName;
            plAssetObj.AssetBundlePath = abPath;
            plAssetObj.AssetName = assetName;
            plAssetObj.AssetPath = assetPath;
            plAssetObj.IsScene = typeName.Equals("Scene");
            plAssetObj.IsWeak = isWeak;

            if (overCallback != null)
            {
                plAssetObj.AssetLoadOverCallback = overCallback;
            }

            m_PreloadedAsyncList.Enqueue(plAssetObj);
        }
        #endregion

        #region 卸载资源（统一入口）
        /// <summary>
        /// 卸载资源（统一入口）
        /// 支持场景/普通对象，自动管理引用计数
        /// </summary>
        /// <param name="oriAsset">要卸载的资源</param>
        /// <param name="overCallback">卸载完成回调</param>
        /// <param name="rightNow">是否立即卸载</param>
        public void Unload(object oriAsset, AssetUnloadOverCallback overCallback = null, bool rightNow = false)
        {
            if (oriAsset == null) return;

            // 卸载场景
            if (oriAsset is UnityEngine.SceneManagement.Scene)
            {
                AssetObject matchedScene = GetSceneAssetObjectByScene((UnityEngine.SceneManagement.Scene)oriAsset);
                if (matchedScene != null)
                {
                    matchedScene.RefCount--;
                    if (matchedScene.RefCount < 0)
                    {
                        if (m_AssetComponent.StrictCheck)
                        {
                            Log.Error("AssetLoadManager Destroy 引用计数错误 ! assetName:{0}", matchedScene.AssetPath);
                        }

                        return;
                    }

                    if (matchedScene.RefCount == 0 && !m_UnloadList.ContainsKey(matchedScene.AssetPath))
                    {
                        matchedScene.UnloadTickNum = -1;
                        if (overCallback != null)
                        {
                            matchedScene.AssetUnloadOverCallbackList.Add(overCallback);
                        }

                        m_UnloadList.Add(matchedScene.AssetPath, matchedScene);
                        if (matchedScene.UnloadTickNum < 0)
                        {
                            UpdateUnload();
                        }
                    }
                }
            }
            // 卸载普通资源
            else
            {
                UnityEngine.Object asset = oriAsset as UnityEngine.Object;
                int instanceID = asset.GetInstanceID();

                if (!m_AssetInstanceIDList.ContainsKey(instanceID))
                {
                    if (asset is GameObject)
                    {
                        UnityEngine.Object.Destroy(asset);
                    }

                    return;
                }

                var assetObj = m_AssetInstanceIDList[instanceID];
                if (assetObj.InstanceID == instanceID)
                {
                    assetObj.RefCount--;
                }
                else
                {
                    if (m_AssetComponent.StrictCheck)
                    {
                        Log.Error("AssetLoadManager Destroy 错误 ! assetName:{0}", assetObj.AssetPath);
                    }

                    return;
                }

                if (assetObj.RefCount < 0)
                {
                    if (m_AssetComponent.StrictCheck)
                    {
                        Log.Error("AssetLoadManager Destroy 引用计数错误 ! assetName:{0}", assetObj.AssetPath);
                    }

                    return;
                }

                if (assetObj.RefCount == 0 && !m_UnloadList.ContainsKey(assetObj.AssetPath))
                {
                    assetObj.UnloadTickNum = rightNow ? -1 : m_UnloadAssetDelayFrameNum + m_UnloadList.Count;
                    if (overCallback != null)
                    {
                        assetObj.AssetUnloadOverCallbackList.Add(overCallback);
                    }

                    m_UnloadList.Add(assetObj.AssetPath, assetObj);
                    if (assetObj.UnloadTickNum < 0)
                    {
                        UpdateUnload();
                    }
                }
            }
        }
        #endregion

        #region 强制卸载所有未使用资源
        /// <summary>
        /// 强制卸载所有未使用资源
        /// 会触发 GC，适合场景切换时调用
        /// </summary>
        /// <param name="overcallback">完成回调</param>
        public void ForceUnloadUnusedAssets(Action overcallback = null)
        {
            if (m_UnloadList.Count > 0)
            {
                m_TempLoadeds.Clear();
                foreach (var assetObj in m_UnloadList.Values)
                {
                    if (assetObj.IsWeak && assetObj.RefCount == 0 && assetObj.AssetLoadOverCallbackList.Count == 0)
                    {
                        m_LoadedList.Remove(assetObj.AssetPath);
                        DoUnload(assetObj);
                        m_TempLoadeds.Add(assetObj);
                    }

                    // 引用计数恢复则取消卸载
                    if (assetObj.RefCount > 0 || !assetObj.IsWeak)
                    {
                        assetObj.UnloadTickNum = 0;
                        m_TempLoadeds.Add(assetObj);
                    }
                }

                foreach (var assetObj in m_TempLoadeds)
                {
                    m_UnloadList.Remove(assetObj.AssetPath);
                }
            }

            AsyncOperation operation = Resources.UnloadUnusedAssets();
            operation.completed += (oper) =>
            {
                GC.Collect();
                overcallback?.Invoke();
            };
        }
        #endregion

        #region 管理器帧更新
        /// <summary>
        /// 管理器帧更新
        /// 驱动预加载、异步完成、卸载、AB管理器更新
        /// </summary>
        public void Update()
        {
            UpdatePreload();
            UpdateLoadedAsync();
            UpdateLoading();
            UpdateUnload();
            _mAssetBundleLoadManager.Update();
        }
        #endregion

        #region 判断资源是否存在
        /// <summary>
        /// 判断资源是否存在
        /// Editor模式检查文件，运行时检查AB清单
        /// </summary>
        public bool IsFileExist(string typeName, string abPath, string assetName)
        {
            if (m_EditorResourceMode)
            {
                string absoluteFullPath = GetAssetAbsoluteFullPath(typeName, abPath, assetName);
                return !string.IsNullOrEmpty(absoluteFullPath) && File.Exists(absoluteFullPath);
            }
            else
            {
                return _mAssetBundleLoadManager.IsABExist(abPath);
            }
        }
        #endregion

        #region 获取资源文件后缀
        /// <summary>
        /// 获取资源文件后缀
        /// </summary>
        public string GetAssetSuffix(string typeName)
        {
            string suffix = string.Empty;
            if (m_EditorResourceMode)
            {
                if (Enum.TryParse(typeof(GameDefinitions.AssetType), typeName, out object tmpAssetType))
                {
                    suffix = GameDefinitions.AssetSuffix[(GameDefinitions.AssetType)tmpAssetType];
                }
            }

            return suffix;
        }
        #endregion

        #region 将外部资源加入管理器统一管理
        /// <summary>
        /// 将外部资源加入管理器统一管理
        /// </summary>
        public void AddAsset(string typeName, string abPath, string assetName, UnityEngine.Object asset)
        {
            string assetPath = GetAssetPath(typeName, abPath, assetName);

            var assetObj = new AssetObject();
            assetObj.TypeName = typeName;
            assetObj.AssetBundlePath = abPath;
            assetObj.AssetName = assetName;
            assetObj.AssetPath = assetPath;
            assetObj.IsScene = typeName.Equals("Scene");
            assetObj.RefCount = 1;

            if (assetObj.IsScene)
            {
                m_Scenes.Add(assetObj);
            }
            else
            {
                assetObj.InstanceID = asset.GetInstanceID();
                assetObj.Asset = asset;
                if (!m_AssetInstanceIDList.ContainsKey(assetObj.InstanceID))
                {
                    m_AssetInstanceIDList.Add(assetObj.InstanceID, assetObj);
                }
                else
                {
                    Log.Error("AssetLoadManager.AddAsset InstanceID '{0}' 已存在", assetObj.InstanceID);
                }
            }

            m_LoadedList.Add(assetObj.AssetPath, assetObj);
        }
        #endregion

        #region 手动增加资源引用计数
        /// <summary>
        /// 手动增加资源引用计数
        /// </summary>
        public void AddAssetRef(string typeName, string abPath, string assetName)
        {
            string assetPath = GetAssetPath(typeName, abPath, assetName);

            if (!m_LoadedList.ContainsKey(assetPath))
            {
                if (m_AssetComponent.StrictCheck)
                {
                    Log.Error("AssetLoadManager 追加引用计数错误 : {0}", assetPath);
                }

                return;
            }

            var assetObj = m_LoadedList[assetPath];
            assetObj.RefCount++;
        }
        #endregion

        #region 移除资源加载回调
        /// <summary>
        /// 移除资源加载回调
        /// </summary>
        public void RemoveCallBack(string typeName, string abPath, string assetName, AssetLoadOverCallback overCallback)
        {
            if (overCallback == null) return;

            string assetPath = GetAssetPath(typeName, abPath, assetName);
            if (string.IsNullOrEmpty(assetPath))
            {
                RemoveCallBackByCallBack(overCallback);
            }

            AssetObject assetObj = null;
            if (m_LoadedList.ContainsKey(assetPath))
            {
                assetObj = m_LoadedList[assetPath];
            }
            else if (m_LoadingList.ContainsKey(assetPath))
            {
                assetObj = m_LoadingList[assetPath];
            }

            if (assetObj != null)
            {
                int index = assetObj.AssetLoadOverCallbackList.IndexOf(overCallback);
                if (index >= 0)
                {
                    assetObj.AssetLoadOverCallbackList.RemoveAt(index);
                }
            }
        }
        #endregion

        #region 获取资源绝对路径（Editor专用）
        /// <summary>
        /// 获取资源绝对路径（Editor专用）
        /// </summary>
        public string GetAssetAbsoluteFullPath(string typeName, string abPath, string assetName)
        {
            string abFullPath = AorTxt.Format("{0}{1}",Application.dataPath.Substring(0, Application.dataPath.Length - s_AssetsStringLength), abPath);
            string tryFileName = AorTxt.Format("{0}{1}", abFullPath, GetAssetSuffix(typeName));
            if (File.Exists(tryFileName))
            {
                return tryFileName;
            }
            else
            {
                string[] fileFullPaths = Directory.GetFiles(abFullPath,AorTxt.Format("{0}{1}", assetName, GetAssetSuffix(typeName)), SearchOption.AllDirectories);
                if (fileFullPaths.Length == 0)
                {
                    if (m_AssetComponent.StrictCheck)
                    {
                        Log.Warning("目录 {0} 未找到文件 {1}{2}", abFullPath, assetName, GetAssetSuffix(typeName));
                    }

                    return null;
                }
                else if (fileFullPaths.Length != 1)
                {
                    if (m_AssetComponent.StrictCheck)
                    {
                        Log.Warning("目录 {0} 找到多个文件 {1}{2}", abFullPath, assetName, GetAssetSuffix(typeName));
                    }

                    return null;
                }

                return fileFullPaths[0].Replace('\\', '/');
            }
        }
        #endregion

        #region 获取资源相对路径（Assets 开头）
        /// <summary>
        /// 获取资源相对路径（Assets 开头）
        /// </summary>
        public string GetAssetRelativeFullPath(string typeName, string abPath, string assetName)
        {
            string absoluteFullPath = GetAssetAbsoluteFullPath(typeName, abPath, assetName);
            string relativeFullPath = absoluteFullPath.Substring(Application.dataPath.Length - s_AssetsStringLength);
            return relativeFullPath;
        }
        #endregion

        #region 生成资源唯一标识路径
        /// <summary>
        /// 生成资源唯一标识路径
        /// </summary>
        public string GetAssetPath(string typeName, string abPath, string assetName)
        {
            return AorTxt.Format("{0}/{1}{2}", abPath, assetName, GetAssetSuffix(typeName));
        }
        #endregion

        #region 获取加载中的资源包装对象
        /// <summary>
        /// 获取加载中的资源包装对象
        /// </summary>
        public AssetObject GetLoadingAssetObjectFromList(string typeName, string abPath, string assetName)
        {
            string assetPath = GetAssetPath(typeName, abPath, assetName);
            if (!string.IsNullOrEmpty(assetPath) && m_LoadingList.ContainsKey(assetPath))
            {
                return m_LoadingList[assetPath];
            }

            return null;
        }
        #endregion

        #region 获取已加载的资源包装对象
        /// <summary>
        /// 获取已加载的资源包装对象
        /// </summary>
        public AssetObject GetLoadedAssetObjectFromList(string typeName, string abPath, string assetName)
        {
            string assetPath = GetAssetPath(typeName, abPath, assetName);
            if (!string.IsNullOrEmpty(assetPath) && m_LoadedList.ContainsKey(assetPath))
            {
                return m_LoadedList[assetPath];
            }

            return null;
        }
        #endregion

        #region 获取等待卸载的资源包装对象
        /// <summary>
        /// 获取等待卸载的资源包装对象
        /// </summary>
        public AssetObject GetUnLoadAssetObjectFromList(string typeName, string abPath, string assetName)
        {
            string assetPath = GetAssetPath(typeName, abPath, assetName);
            if (!string.IsNullOrEmpty(assetPath) && m_UnloadList.ContainsKey(assetPath))
            {
                return m_UnloadList[assetPath];
            }

            return null;
        }
        #endregion
    }
}