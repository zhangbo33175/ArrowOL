using UnityEngine;

namespace Honor.Runtime
{
    public partial class LoadABComponent
    {
        /// <summary>
        /// 内部同步加载AB
        /// </summary>
        /// <param name="abFormatPath">ab格式化路径</param>
        /// <returns></returns>
        private  LoadABData Internal_Load_AB_Asset_Bundle_Sync(string abFormatPath)
        {
            LoadABData abObj = null;
            // 如果处在【加载完成列表】中则直接返回（自身与其递归到的所有依赖AB项引用计数全部+1）
            if (AssetManager.Instance.CheckAssetPath(AssetManager.Instance.m_LoadedABList, abFormatPath))
            {
                abObj = AssetManager.Instance.m_LoadedABList[abFormatPath];
                abObj.RefCount++;

                foreach (var dpObj in abObj.Depends)
                {
                    Internal_Load_AB_Asset_Bundle_Sync(dpObj.FormatPath);
                }

                return abObj;
            }
            // 如果处在【加载中列表（异步方式）】中则立即将异步改成同步加载得到结果（自身与其递归到的所有依赖AB项引用计数全部+1）
            else if (AssetManager.Instance.m_LoadingABList.ContainsKey(abFormatPath))
            {
                abObj = AssetManager.Instance.m_LoadingABList[abFormatPath];
                abObj.RefCount++;

                foreach (var dpObj in abObj.Depends)
                {
                    Internal_Load_AB_Asset_Bundle_Sync(dpObj.FormatPath);
                }

                // 强制加载完成并回调
                NormalOrForceLoadABOverAndCallBack(abObj);

                return abObj;
            }
            // 如果处在【准备加载列表】中则立即进行同步加载得到结果（自身与其递归到的所有依赖AB项引用计数全部+1）
            else if (AssetManager.Instance.m_ReadyABList.ContainsKey(abFormatPath))
            {
                abObj = AssetManager.Instance.m_ReadyABList[abFormatPath];
                abObj.RefCount++;

                foreach (var dpObj in abObj.Depends)
                {
                    Internal_Load_AB_Asset_Bundle_Sync(dpObj.FormatPath);
                }

                string path1;
                ResourceType origin1;
                AssetManager.Instance.GetABLoadPathOnDisk(abFormatPath, out path1, out origin1);

#if UNITY_WEBGL
                   abObj.AB = LoadAssetBundleFromWebGL(abFormatPath);
#else
                abObj.AssetBundle = AssetBundle.LoadFromFile(path1);
#endif
                abObj.Resource = origin1;

                AssetManager.Instance.m_ReadyABList.Remove(abObj.FormatPath);
                AssetManager.Instance.m_LoadedABList.Add(abObj.FormatPath, abObj);

                // 强制加载完成并回调
                NormalOrForceLoadABOverAndCallBack(abObj);

                return abObj;
            }

            // 如果三种列表中都不存在则直接创建一个新的同步加载
            abObj = new LoadABData();
            abObj.FormatPath = abFormatPath;
            abObj.RefCount = 1;

            string path;
            ResourceType origin;
            AssetManager.Instance.GetABLoadPathOnDisk(abFormatPath, out path, out origin);

#if UNITY_WEBGL
            abObj.AB = LoadAssetBundleFromWebGL(abFormatPath);
#else
            abObj.AssetBundle = AssetBundle.LoadFromFile(path);
#endif
            abObj.Resource = origin;

            // 用同步的方式加载依赖项
            string[] dependsData = null;
            if (AssetManager.Instance.m_DependsDataABList.ContainsKey(abFormatPath))
            {
                dependsData = AssetManager.Instance.m_DependsDataABList[abFormatPath];
            }

            if (dependsData != null && dependsData.Length > 0)
            {
                // 同步加载后将使加载中的依赖资源数量清零
                abObj.DependLoadingCount = 0;

                // 对依赖资源进行同步加载并记录依赖资源
                foreach (var dpFormatName in dependsData)
                {
                    var dpObj = Internal_Load_AB_Asset_Bundle_Sync(dpFormatName);
                    abObj.Depends.Add(dpObj);
                }
            }

            // 将新创建的同步加载得到的资源加入到【已加载完成列表】中
            AssetManager.Instance.m_LoadedABList.Add(abObj.FormatPath, abObj);

            return abObj;
        }

        /// <summary>
        /// 内部异步加载AB
        /// </summary>
        /// <param name="abFormatPath">ab格式化路径</param>
        /// <param name="abOverCallback">加载完成回调函数</param>
        /// <returns></returns>
        private  LoadABData InternalLoadABAssetBundleAsync(string abFormatPath,
            LoadAssetBundleOverCallBack abOverCallback)
        {
            LoadABData abObj = null;

            // 如果处在【加载完成列表】中则直接返回（自身与其所有的依赖AB项引用计数全部+1）
            if (AssetManager.Instance.m_LoadedABList.ContainsKey(abFormatPath))
            {
                abObj = AssetManager.Instance.m_LoadedABList[abFormatPath];
                AddSelfABAndDependsRef(abObj);
                abOverCallback(abObj, abObj.AssetBundle);
                return abObj;
            }
            // 如果处在【加载中列表（异步方式）】中则立即将新传入的回调函数加入ab封装对象中，等待异步结束后回调（自身与其所有的依赖AB项引用计数全部+1）
            else if (AssetManager.Instance.m_LoadingABList.ContainsKey(abFormatPath))
            {
                abObj = AssetManager.Instance.m_LoadingABList[abFormatPath];
                AddSelfABAndDependsRef(abObj);
                abObj.ABLoadOverCallbacksList.Add(abOverCallback);
                return abObj;
            }
            // 在准备加载中
            // 如果处在【准备加载列表】中则立即将新传入的回调函数加入ab封装对象中，等待异步开始到结束（自身与其所有的依赖AB项引用计数全部+1）
            else if (AssetManager.Instance.m_ReadyABList.ContainsKey(abFormatPath))
            {
                abObj = AssetManager.Instance.m_ReadyABList[abFormatPath];
                AddSelfABAndDependsRef(abObj);
                abObj.ABLoadOverCallbacksList.Add(abOverCallback);
                return abObj;
            }

            // 如果三种列表中都不存在则直接创建一个新的异步加载
            abObj = new LoadABData();
            abObj.FormatPath = abFormatPath;

            abObj.RefCount = 1;
            abObj.ABLoadOverCallbacksList.Add(abOverCallback);

            // 加载依赖项
            string[] dependsData = null;
            if (AssetManager.Instance.m_DependsDataABList.ContainsKey(abFormatPath))
            {
                dependsData = AssetManager.Instance.m_DependsDataABList[abFormatPath];
            }

            if (dependsData != null && dependsData.Length > 0)
            {
                // 将即将开始异步加载的依赖资源的总数量赋给当前新创建的ab资源中，等待后面异步加载依赖资源时对该数量的刷新
                abObj.DependLoadingCount = dependsData.Length;
                foreach (var dpFormatName in dependsData)
                {
                    var dpObj = InternalLoadABAssetBundleAsync(dpFormatName,
                        (LoadABData abObject, AssetBundle _ab) =>
                        {
                            if (abObj.DependLoadingCount <= 0)
                            {
                                Log.Error("加载依赖AB错误，ab名称:{0}", abFormatPath);
                                return;
                            }

                            // 完成1个依赖资源的加载后，数量-1
                            abObj.DependLoadingCount--;

                            // 当所有依赖全部加载完毕后，触发正常加载完成的逻辑并触发回调
                            if (abObj.DependLoadingCount == 0)
                            {
#if UNITY_WEBGL
                                 NormalOrForceLoadOverAndCallBack(abObj);
#else
                                if (abObj.Request != null && abObj.Request.isDone)
                                {
                                    NormalOrForceLoadABOverAndCallBack(abObj);
                                }
#endif
                            }
                        }
                    );
                    // 将依赖资源记录到当前新创建的ab资源中
                    abObj.Depends.Add(dpObj);
                }
            }

            // 正在加载的数量不能超过上限
            if (AssetManager.Instance.m_LoadingABList.Count < AssetManager.MAX_LOADING_COUNT)
            {
                // 立即开始异步加载，并加入到【加载中列表】中
                DoLoadABAsync(abObj);
                AssetManager.Instance.m_LoadingABList.Add(abFormatPath, abObj);
            }
            else
            {
                // 如果超过了上限，则暂时放入准备列表中
                AssetManager.Instance.m_ReadyABList.Add(abFormatPath, abObj);
            }

            return abObj;
        }

        /// <summary>
        /// 内部异步卸载AB
        /// </summary>
        /// <param name="abFormatPath">ab格式化路径</param>
        private  void InternalUnloadABAssetBundleAsync(string abFormatPath)
        {
            LoadABData abObj = null;

            // 获得可能存在于三种列表中的AB封装资源对象
            if (AssetManager.Instance.m_LoadedABList.ContainsKey(abFormatPath))
            {
                abObj = AssetManager.Instance.m_LoadedABList[abFormatPath];
            }
            else if (AssetManager.Instance.m_LoadingABList.ContainsKey(abFormatPath))
            {
                abObj = AssetManager.Instance.m_LoadingABList[abFormatPath];
            }
            else if (AssetManager.Instance.m_ReadyABList.ContainsKey(abFormatPath))
            {
                abObj = AssetManager.Instance.m_ReadyABList[abFormatPath];
            }

            if (abObj == null)
            {
                Log.Error("卸载AB包错误，ab名称:{0}", abFormatPath);
                return;
            }

            if (abObj.RefCount == 0)
            {
                Log.Error("卸载AB包时引用计数错误！ab名称:{0}", abFormatPath);
                return;
            }

            // 自身与其递归得到所有依赖AB资源的引用计数全部-1
            abObj.RefCount--;
            foreach (var dpObj in abObj.Depends)
            {
                InternalUnloadABAssetBundleAsync(dpObj.FormatPath);
            }

            // 引用计数-1后如果为0则加入到【卸载列表】中
            if (abObj.RefCount == 0)
            {
                if (!AssetManager.Instance.m_UnloadABList.ContainsKey(abObj.FormatPath))
                {
                    AssetManager.Instance.m_UnloadABList.Add(abObj.FormatPath, abObj);
                }
            }
        }

        /// <summary>
        /// ab自身与所有依赖项AB引用计数+1
        /// 递归调用
        /// </summary>
        /// <param name="abObj"></param>
        private  void AddSelfABAndDependsRef(LoadABData abObj)
        {
            abObj.RefCount++;

            if (abObj.Depends.Count == 0) return;
            foreach (var dpObj in abObj.Depends)
            {
                // 递归依赖项，加载完
                AddSelfABAndDependsRef(dpObj);
            }
        }

        /// <summary>
        /// 异步加载AB资源
        /// </summary>
        /// <param name="abObj">AB封装对象</param>
        private  void DoLoadABAsync(LoadABData abObj)
        {
            string path;
            ResourceType origin;
            AssetManager.Instance.GetABLoadPathOnDisk(abObj.FormatPath, out path, out origin);
#if UNITY_WEBGL
                abObj.AB = LoadAssetBundleFromWebGL(abObj.FormatPath);
#else
            abObj.Request = AssetBundle.LoadFromFileAsync(path);
            if (abObj.Request == null)
            {
                Log.Error("加载AB包时的路径错误！ab名称:{0}", abObj.FormatPath);
            }
#endif
            abObj.Resource = origin;
        }

        /// <summary>
        /// 卸载AB资源
        /// </summary>
        /// <param name="abObj">AB封装对象</param>
        private  void DoUnloadAB(LoadABData abObj)
        {
            // 这里用true，卸载Asset内存，实现指定卸载
            if (abObj.AssetBundle == null)
            {
                Log.Error("卸载AB包时错误！ab名称:{0}", abObj.FormatPath);
                return;
            }

            abObj.AssetBundle.Unload(true);
            abObj.AssetBundle = null;
        }

        /// <summary>
        /// 正常或强制加载完成并触发回调
        /// </summary>
        /// <param name="abObj">AB封装对象</param>
        private  void NormalOrForceLoadABOverAndCallBack(LoadABData abObj)
        {
            // 从异步中提取ab
            if (abObj.Request != null)
            {
                // 如果没加载完，通过API立刻拿到资源（变异步为同步）
                abObj.AssetBundle = abObj.Request.assetBundle;
                abObj.Request = null;
                AssetManager.Instance.m_LoadingABList.Remove(abObj.FormatPath);
                AssetManager.Instance.m_LoadedABList.Add(abObj.FormatPath, abObj);
            }

            if (abObj.AssetBundle == null)
            {
                string path;
                ResourceType origin;
                AssetManager.Instance.GetABLoadPathOnDisk(abObj.FormatPath, out path, out origin);
#if UNITY_WEBGL
                    abObj.AB = LoadAssetBundleFromWebGL(abObj.FormatPath);
#else
                abObj.AssetBundle = AssetBundle.LoadFromFile(path);
#endif
                abObj.Resource = origin;
            }

            // 运行回调
            foreach (var callback in abObj.ABLoadOverCallbacksList)
            {
                callback(abObj, abObj.AssetBundle);
            }

            abObj.ABLoadOverCallbacksList.Clear();
        }
    }
}