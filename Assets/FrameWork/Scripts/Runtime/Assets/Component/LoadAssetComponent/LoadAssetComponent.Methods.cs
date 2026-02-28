using System;
using GameLib;
using UnityEngine;

namespace Honor.Runtime
{
    public partial class LoadAssetComponent
    {
        /// <summary>
        /// 根据回调解绑回调
        /// </summary>
        /// <param name="overCallback">asset资源加载完成的异步回调</param>
        private  void Remove_CallBack_Asset_By_CallBack(LoadAssetOverCallback overCallback)
        {
            foreach (var assetObj in AssetManager.Instance.m_LoadingAssetList.Values)
            {
                if (assetObj.LoadAssetOverCallbackList.Count == 0) continue;
                int index = assetObj.LoadAssetOverCallbackList.IndexOf(overCallback);
                if (index >= 0)
                {
                    assetObj.LoadAssetOverCallbackList.RemoveAt(index);
                }
            }

            foreach (var assetObj in AssetManager.Instance.m_LoadedAssetList.Values)
            {
                if (assetObj.LoadAssetOverCallbackList.Count == 0) continue;
                int index = assetObj.LoadAssetOverCallbackList.IndexOf(overCallback);
                if (index >= 0)
                {
                    assetObj.LoadAssetOverCallbackList.RemoveAt(index);
                }
            }
        }

        /// <summary>
        /// 内部执行Asset资源加载完成的回调
        /// </summary>
        /// <param name="assetObj">asset封装对象</param>
        private void Do_Remove_Asset_Callback(LoadAssetData assetObj)
        {
            if (assetObj.LoadAssetOverCallbackList.Count == 0)
            {
                return;
            }

            // 先提取count，保证回调中有加载需求不加载
            int count = assetObj.LockCallbackCount;
            for (int i = 0; i < count; i++)
            {
                if (assetObj.LoadAssetOverCallbackList[i] != null)
                {
                    // 每次回调，引用计数+1
                    assetObj.RefCount++;

                    try
                    {
                        assetObj.LoadAssetOverCallbackList[i](assetObj, assetObj.Asset);
                    }
                    catch (GameException e)
                    {
                        Log.Error(e);
                    }
                }
            }

            assetObj.LoadAssetOverCallbackList.RemoveRange(0, count);
        }

        /// <summary>
        /// 内部执行Asset资源的卸载
        /// </summary>
        /// <param name="assetObj">asset封装对象</param>
        private void Do_Remove_Asset_Unload(LoadAssetData assetObj)
        {
            if (assetObj.IsScene)
            {
                UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(assetObj.AssetName);
            }

            if (!GameConfig.instance.m_EditorResourceMode)
            {
                AssetManager.Instance.m_LoadABComponent.UnloadAB(assetObj.ABPath);
            }

            assetObj.Asset = null;

            if (assetObj.IsScene)
            {
                AssetManager.Instance.m_ScenesAsset.Remove(assetObj);
            }
            else
            {
                if (AssetManager.Instance.m_AssetInstanceIDAssetList.ContainsKey(assetObj.InstanceID))
                {
                    AssetManager.Instance.m_AssetInstanceIDAssetList.Remove(assetObj.InstanceID);
                }
            }

            // unload完毕回调
            if (assetObj.UnloadAssetOverCallbackList != null)
            {
                foreach (UnloadAssetOverCallback overCallback in assetObj.UnloadAssetOverCallbackList)
                {
                    if (overCallback != null)
                    {
                        overCallback(assetObj);
                    }
                }
            }
        }

        /// <summary>
        /// “预加载列表”心跳管理
        /// </summary>
        private  void Update_Asset_Preload()
        {
            if (AssetManager.Instance.m_LoadingAssetList.Count > 0 || AssetManager.Instance.m_PreloadedAsyncAssetList.Count == 0)
            {
                return;
            }

            // 从队列取出一个Asset封装对象进行异步加载
            PreloadAssetData plAssetObj = null;
            while (AssetManager.Instance.m_PreloadedAsyncAssetList.Count > 0 && plAssetObj == null)
            {
                plAssetObj = AssetManager.Instance.m_PreloadedAsyncAssetList.Dequeue();

                if (AssetManager.Instance.m_LoadingAssetList.ContainsKey(plAssetObj.AssetPath))
                {
                    AssetManager.Instance.m_LoadingAssetList[plAssetObj.AssetPath].IsWeak = plAssetObj.IsWeak;
                }
                else if (AssetManager.Instance.m_LoadedAssetList.ContainsKey(plAssetObj.AssetPath))
                {
                    AssetManager.Instance.m_LoadedAssetList[plAssetObj.AssetPath].IsWeak = plAssetObj.IsWeak;
                    plAssetObj = null;
                }
                else
                {
                    Load_Asset_Async(plAssetObj.TypeName, plAssetObj.ABPath, plAssetObj.AssetName,
                        plAssetObj.LoadAssetOverCallback);
                    if (AssetManager.Instance.m_LoadingAssetList.ContainsKey(plAssetObj.AssetPath))
                    {
                        AssetManager.Instance.m_LoadingAssetList[plAssetObj.AssetPath].IsWeak = plAssetObj.IsWeak;
                    }
                    else if (AssetManager.Instance.m_LoadedAssetList.ContainsKey(plAssetObj.AssetPath))
                    {
                        AssetManager.Instance.m_LoadedAssetList[plAssetObj.AssetPath].IsWeak = plAssetObj.IsWeak;
                    }
                }
            }
        }

        /// <summary>
        /// “加载完成列表”心跳管理
        /// </summary>
        private  void Update_Loaded_Asset_Async()
        {
            if (AssetManager.Instance.m_LoadedAsyncTmpAgentAssetList.Count == 0) return;

            int count = AssetManager.Instance.m_LoadedAsyncTmpAgentAssetList.Count;
            for (int i = 0; i < count; i++)
            {
                // 先锁定回调数量，保证异步成立
                AssetManager.Instance.m_LoadedAsyncTmpAgentAssetList[i].LockCallbackCount =
                    AssetManager.Instance.m_LoadedAsyncTmpAgentAssetList[i].LoadAssetOverCallbackList.Count;
            }

            for (int i = 0; i < count; i++)
            {
                Do_Remove_Asset_Callback(AssetManager.Instance.m_LoadedAsyncTmpAgentAssetList[i]);
            }

            AssetManager.Instance.m_LoadedAsyncTmpAgentAssetList.RemoveRange(0, count);

            if (AssetManager.Instance.m_LoadingAssetList.Count == 0 &&
                AssetManager.Instance.m_LoadingIntervalCount > AssetManager.Instance.m_LoadedMaxNumToCleanMemery)
            {
                // 在连续的大量加载后，强制调用一次gc
                AssetManager.Instance.m_LoadingIntervalCount = 0;
                System.GC.Collect();
            }
        }

        /// <summary>
        /// “加载中列表”心跳管理
        /// </summary>
        private void Update_Loading_Asset()
        {
            if (AssetManager.Instance.m_LoadingAssetList.Count == 0) return;

            // 检测加载完的
            AssetManager.Instance.m_TempAssetLoadeds.Clear();
            foreach (var assetObj in AssetManager.Instance.m_LoadingAssetList.Values)
            {
                if (GameConfig.instance.m_EditorResourceMode)
                {
#if UNITY_EDITOR
                    if (assetObj.IsScene)
                    {
                        if (assetObj.Request != null && assetObj.Request.isDone)
                        {
                            AssetManager.Instance.m_ScenesAsset.Add(assetObj);
                            assetObj.Request = null;
                            AssetManager.Instance.m_TempAssetLoadeds.Add(assetObj);
                        }
                    }
                    else
                    {
                        string assetRelativeFullPath = AssetManager.Instance.GetAssetRelativeFullPath(assetObj.TypeName,
                            assetObj.ABPath,
                            assetObj.AssetName);
                        Type assetType = Assembly.GetType(AorTxt.Format("UnityEngine.{0}", assetObj.TypeName));
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

                        if (assetObj.Asset == null)
                        {
                            // 提取的资源失败，从加载列表删除
                            AssetManager.Instance.m_LoadingAssetList.Remove(assetObj.AssetPath);
                            Log.Error("AssetLoadManager assetObj.Asset Null : {0}", assetObj.AssetPath);
                            break;
                        }

                        assetObj.InstanceID = assetObj.Asset.GetInstanceID();
                        if (!AssetManager.Instance.m_AssetInstanceIDAssetList.ContainsKey(assetObj.InstanceID))
                        {
                            AssetManager.Instance.m_AssetInstanceIDAssetList.Add(assetObj.InstanceID, assetObj);
                        }
                        else
                        {
                            Log.Error(
                                "AssetLoadManager.UpdateLoading assetObj.InstanceID '{0}' 已经存在。Name: {1} ,请检查这个资源各个地方的AB路径配置是否一致",
                                assetObj.InstanceID, assetObj.AssetName);
                        }

                        assetObj.Request = null;
                        AssetManager.Instance.m_TempAssetLoadeds.Add(assetObj);
                    }
#endif
                }
                else
                {
                    if (assetObj.Request != null && assetObj.Request.isDone)
                    {
                        if (assetObj.IsScene)
                        {
                            AssetManager.Instance.m_ScenesAsset.Add(assetObj);
                        }
                        else
                        {
                            // 加载完进行数据清理
                            if (assetObj.Request is AssetBundleRequest)
                            {
                                assetObj.Asset = (assetObj.Request as AssetBundleRequest).asset;
                            }

                            if (assetObj.Asset == null)
                            {
                                // 提取的资源失败，从加载列表删除
                                AssetManager.Instance.m_LoadingAssetList.Remove(assetObj.AssetPath);
                                Log.Error("AssetLoadManager assetObj.Asset Null : {0}", assetObj.AssetPath);
                                break;
                            }

                            assetObj.InstanceID = assetObj.Asset.GetInstanceID();
                            if (!AssetManager.Instance.m_AssetInstanceIDAssetList.ContainsKey(assetObj.InstanceID))
                            {
                                AssetManager.Instance.m_AssetInstanceIDAssetList.Add(assetObj.InstanceID, assetObj);
                            }
                            else
                            {
                                Log.Error("AssetLoadManager.LoadSync assetObj.InstanceID '{0}' 已经存在。",
                                    assetObj.InstanceID);
                            }
                        }

                        assetObj.Request = null;
                        AssetManager.Instance.m_TempAssetLoadeds.Add(assetObj);
                    }
                }
            }

            // 回调中有可能对m_LoadingList进行操作，先移动
            foreach (var assetObj in AssetManager.Instance.m_TempAssetLoadeds)
            {
                AssetManager.Instance.m_LoadingAssetList.Remove(assetObj.AssetPath);
                AssetManager.Instance.m_LoadedAssetList.Add(assetObj.AssetPath, assetObj);

                // 统计本轮加载的数量
                AssetManager.Instance.m_LoadingIntervalCount++;

                // 先锁定回调数量，保证异步成立
                assetObj.LockCallbackCount = assetObj.LoadAssetOverCallbackList.Count;
            }

            // 统一进行加载完成的回调派发
            foreach (var assetObj in AssetManager.Instance.m_TempAssetLoadeds)
            {
                Do_Remove_Asset_Callback(assetObj);
            }
        }

        /// <summary>
        /// “卸载列表”心跳管理
        /// </summary>
        private void Update_Unload_Asset()
        {
            if (AssetManager.Instance.m_UnloadAssetList.Count == 0) return;

            AssetManager.Instance.m_TempAssetLoadeds.Clear();
            foreach (var assetObj in AssetManager.Instance.m_UnloadAssetList.Values)
            {
                if (assetObj.IsWeak && assetObj.RefCount == 0 && assetObj.LoadAssetOverCallbackList.Count == 0)
                {
                    // 引用计数为0，延迟卸载时间到，且没有需要回调的函数，销毁
                    if (assetObj.UnloadTickNum < 0)
                    {
                        AssetManager.Instance.m_LoadedAssetList.Remove(assetObj.AssetPath);
                        Do_Remove_Asset_Unload(assetObj);
                        AssetManager.Instance.m_TempAssetLoadeds.Add(assetObj);
                    }
                    else
                    {
                        assetObj.UnloadTickNum--;
                    }
                }

                // 正常在unload列表中对象的引用计数应该为0，如果此时发现外界对该对象又进行了重新load从而导致引用计数>0，这时需要从unload列表中马上移除来取消后面即将触发的释放操作。
                if (assetObj.RefCount > 0 || !assetObj.IsWeak)
                {
                    // 延迟卸载帧数清0
                    assetObj.UnloadTickNum = 0;
                    // 引用计数增加（销毁期间有加载）
                    AssetManager.Instance.m_TempAssetLoadeds.Add(assetObj);
                }
            }

            foreach (var assetObj in AssetManager.Instance.m_TempAssetLoadeds)
            {
                AssetManager.Instance.m_UnloadAssetList.Remove(assetObj.AssetPath);
            }
        }

        /// <summary>
        /// 根据场景对象从当前已经激活的场景集合中获取对应的Asset封装对象
        /// </summary>
        /// <param name="scene">场景对象</param>
        /// <returns></returns>
        private  LoadAssetData Get_Scene_Asset_Object_By_Scene(UnityEngine.SceneManagement.Scene scene)
        {
            LoadAssetData result = AssetManager.Instance.m_ScenesAsset.Find((assetObject) =>
            {
                if (assetObject.AssetName == scene.name)
                {
                    return true;
                }

                return false;
            });
            return result;
        }
    }
}