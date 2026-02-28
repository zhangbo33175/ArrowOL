using System;
using System.IO;
using GameLib;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Honor.Runtime
{
    public partial class LoadAssetComponent
    {
        /// <summary>
        /// 同步加载
        /// </summary>
        /// <param name="typeName">资源类型名称</param>
        /// <param name="abPath">ab路径</param>
        /// <param name="assetName">asset名称</param>
        /// <returns></returns>
        public Object Load_Asset_Sync(string typeName, string abPath, string assetName)
        {
            string assetPath = AssetManager.Instance.GetAssetPath(typeName, abPath, assetName);
            if (!Is_File_Exist(typeName, abPath, assetName))
            {
                return null;
            }

            LoadAssetData assetObj = null;
            //在加载完成列表中查询
            if (AssetManager.Instance.m_LoadedAssetList.ContainsKey(assetPath))
            {
                assetObj = AssetManager.Instance.m_LoadedAssetList[assetPath];
                assetObj.RefCount++;
                return assetObj.Asset;
            }
            else if (AssetManager.Instance.m_LoadingAssetList.ContainsKey(assetPath))
            {
                assetObj = AssetManager.Instance.m_LoadingAssetList[assetPath];

                // 异步加载未完成
                if (assetObj.Request != null)
                {
                    // 异步转同步（直接取asset）
                    if (assetObj.Request is AssetBundleRequest)
                    {
                        assetObj.Asset = (assetObj.Request as AssetBundleRequest).asset;
                    }

                    assetObj.Request = null;
                }
                else // 异步加载已经完成
                {
                    if (GameConfig.instance.m_EditorResourceMode)
                    {
#if UNITY_EDITOR
                        if (assetObj.IsScene)
                        {
                            // 开始加载场景
                            UnityEngine.SceneManagement.SceneManager.LoadScene(assetName,
                                UnityEngine.SceneManagement.LoadSceneMode.Additive);
                        }
                        else
                        {
                            string assetRelativeFullPath =
                                AssetManager.Instance.GetAssetRelativeFullPath(typeName, abPath, assetName);
                            Type assetType = Assembly.GetType(AorTxt.Get_Type_String_By_Name(typeName));
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
                        // 异步转同步，按照同步的方式重新加载
                        AssetBundle ab = AssetManager.Instance.m_LoadABComponent.LoadABSync(abPath);

                        if (assetObj.IsScene)
                        {
                            // 开始加载场景
                            UnityEngine.SceneManagement.SceneManager.LoadScene(assetName,
                                UnityEngine.SceneManagement.LoadSceneMode.Additive);
                        }
                        else
                        {
                            Type assetType = Assembly.GetType(AorTxt.Get_Type_String_By_Name(typeName));
                            if (assetType != null)
                            {
                                assetObj.Asset = ab.LoadAsset(assetName, assetType);
                            }
                            else
                            {
                                assetObj.Asset = ab.LoadAsset(assetName);
                            }
                        }

                        // 由于AB刚刚进行了异步转同步，即多进行了一次引用计数的增加，所以为了确保引用计数的正确性，需要卸载AB异步时的引用计数
                        AssetManager.Instance.m_LoadABComponent.UnloadAB(abPath);
                    }
                }

                if (assetObj.IsScene)
                {
                    AssetManager.Instance.m_ScenesAsset.Add(assetObj);
                }
                else
                {
                    if (assetObj.Asset == null)
                    {
                        // 提取的资源失败，从加载列表删除
                        return null;
                    }

                    assetObj.InstanceID = assetObj.Asset.GetInstanceID();
                    if (!AssetManager.Instance.m_AssetInstanceIDAssetList.ContainsKey(assetObj.InstanceID))
                    {
                        AssetManager.Instance.m_AssetInstanceIDAssetList.Add(assetObj.InstanceID, assetObj);
                    }
                    else
                    {
                        Log.Error("AssetLoadManager.LoadSync assetObj.InstanceID '{0}' 已经存在。 AssetPath:'{1}'",
                            assetObj.InstanceID, assetObj.AssetPath);
                    }
                }

                AssetManager.Instance.m_LoadingAssetList.Remove(assetObj.AssetPath);
                AssetManager.Instance.m_LoadedAssetList.Add(assetObj.AssetPath, assetObj);
                // 原先是异步加载的Asset封装对象，加入异步列表
                AssetManager.Instance.m_LoadedAsyncTmpAgentAssetList.Add(assetObj);

                assetObj.RefCount++;

                return assetObj.Asset;
            }

            assetObj = new LoadAssetData();
            assetObj.TypeName = typeName;
            assetObj.ABPath = abPath;
            assetObj.AssetName = assetName;
            assetObj.AssetPath = assetPath;
            assetObj.IsScene = typeName.Equals("Scene");

            if (GameConfig.instance.m_EditorResourceMode)
            {
#if UNITY_EDITOR
                if (assetObj.IsScene)
                {
                    // 以追加的方式开始加载场景
                    UnityEngine.SceneManagement.SceneManager.LoadScene(assetName,
                        UnityEngine.SceneManagement.LoadSceneMode.Additive);
                }
                else
                {
                    string assetRelativeFullPath =
                        AssetManager.Instance.GetAssetRelativeFullPath(typeName, abPath, assetName);
                    Type assetType = Assembly.GetType(AorTxt.Get_Type_String_By_Name(typeName));
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
                assetObj.Resource = ResourceType.Editor;
            }
            else
            {
                if (AssetManager.Instance.m_LoadABComponent.IsABExist(abPath))
                {
                    AssetBundle ab = AssetManager.Instance.m_LoadABComponent.LoadABSync(abPath);
                    if (assetObj.IsScene)
                    {
                        UnityEngine.SceneManagement.SceneManager.LoadScene(assetName,
                            UnityEngine.SceneManagement.LoadSceneMode.Additive);
                    }
                    else
                    {
                        Type assetType = Assembly.GetType(AorTxt.Get_Type_String_By_Name(typeName));
                        if (assetType != null)
                        {
                            assetObj.Asset = ab.LoadAsset(assetName, assetType);
                        }
                        else
                        {
                            assetObj.Asset = ab.LoadAsset(assetName);
                        }
                    }

                    assetObj.Resource = AssetManager.Instance.LoadedABList[AssetManager.Instance.m_LoadABComponent.GetABFormatPath(assetObj.ABPath)]
                        .Resource;
                }
            }

            if (assetObj.IsScene)
            {
                AssetManager.Instance.m_ScenesAsset.Add(assetObj);
            }
            else
            {
                if (assetObj.Asset == null)
                {
                    return null;
                }

                assetObj.InstanceID = assetObj.Asset.GetInstanceID();
                if (!AssetManager.Instance.m_AssetInstanceIDAssetList.ContainsKey(assetObj.InstanceID))
                {
                    AssetManager.Instance.m_AssetInstanceIDAssetList.Add(assetObj.InstanceID, assetObj);
                }
                else
                {
                    Log.Error("AssetLoadManager.LoadSync assetObj.InstanceID '{0}' 已经存在。 AssetPath:'{1}'",
                        assetObj.InstanceID, assetObj.AssetPath);
                }
            }

            AssetManager.Instance.m_LoadedAssetList.Add(assetPath, assetObj);

            assetObj.RefCount = 1;

            return assetObj.Asset;
        }

        /// <summary>
        /// 异步加载
        /// 即使资源已经加载完成也会异步回调
        /// </summary>
        /// <param name="typeName">资源类型名称</param>
        /// <param name="abPath">ab路径</param>
        /// <param name="assetName">asset名称</param>
        /// <param name="overCallback">asset资源加载完成的异步回调</param>
        public void Load_Asset_Async(string typeName, string abPath, string assetName,
            LoadAssetOverCallback overCallback)
        {
            string assetPath = AssetManager.Instance.GetAssetPath(typeName, abPath, assetName);
            if (!Is_File_Exist(typeName, abPath, assetName))
            {
                return;
            }

            LoadAssetData assetObj = null;
            if (AssetManager.Instance.m_LoadedAssetList.ContainsKey(assetPath))
            {
                assetObj = AssetManager.Instance.m_LoadedAssetList[assetPath];
                assetObj.LoadAssetOverCallbackList.Add(overCallback);
                AssetManager.Instance.m_LoadedAsyncTmpAgentAssetList.Add(assetObj);
                return;
            }
            else if (AssetManager.Instance.m_LoadingAssetList.ContainsKey(assetPath))
            {
                assetObj = AssetManager.Instance.m_LoadingAssetList[assetPath];
                assetObj.LoadAssetOverCallbackList.Add(overCallback);
                return;
            }

            assetObj = new LoadAssetData();
            assetObj.TypeName = typeName;
            assetObj.ABPath = abPath;
            assetObj.AssetName = assetName;
            assetObj.AssetPath = assetPath;
            assetObj.IsScene = typeName.Equals("Scene");

            assetObj.LoadAssetOverCallbackList.Add(overCallback);

            if (GameConfig.instance.m_EditorResourceMode)
            {
                if (assetObj.IsScene)
                {
                    assetObj.Request = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(assetName,
                        UnityEngine.SceneManagement.LoadSceneMode.Additive);
                }

                assetObj.Resource = ResourceType.Editor;
                AssetManager.Instance.m_LoadingAssetList.Add(assetPath, assetObj);
            }
            else
            {
                if (AssetManager.Instance.m_LoadABComponent.IsABExist(abPath))
                {
                    AssetManager.Instance.m_LoadingAssetList.Add(assetPath, assetObj);
                    AssetManager.Instance.m_LoadABComponent.LoadABAsync(abPath, (LoadABData abObject, AssetBundle ab) =>
                    {
                        if (ab == null)
                        {
                            AssetManager.Instance.m_LoadingAssetList.Remove(assetPath);
                            return;
                        }

                        if (AssetManager.Instance.m_LoadingAssetList.ContainsKey(assetPath) && assetObj.Request == null)
                        {
                            if (assetObj.IsScene)
                            {
                                assetObj.Request =
                                    UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(assetName,
                                        UnityEngine.SceneManagement.LoadSceneMode.Additive);
                            }
                            else
                            {
                                if (assetObj.Asset == null)
                                {
                                    Type assetType = Assembly.GetType(AorTxt.Get_Type_String_By_Name(typeName));
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
                        }
                    });

                    LoadABData abObjectTryGet;
                    string path = AssetManager.Instance.m_LoadABComponent.GetABFormatPath(assetObj.ABPath);
                    if (AssetManager.Instance.LoadedABList.TryGetValue(path, out abObjectTryGet) ||
                        AssetManager.Instance.LoadingABList.TryGetValue(path, out abObjectTryGet))
                    {
                        assetObj.Resource = abObjectTryGet.Resource;
                    }
                }
                else
                {
                    return;
                }
            }
        }

        /// <summary>
        /// 预加载
        /// </summary>
        /// <param name="typeName">资源类型名称</param>
        /// <param name="abPath">ab路径</param>
        /// <param name="assetName">asset名称</param>
        /// <param name="isWeak">弱引用(当为true时表示使用过后会销毁，为false时将不会销毁，常驻内存，慎用)</param>
        public void Pre_Load_Asset_Async(string typeName, string abPath, string assetName,
            LoadAssetOverCallback overCallback, bool isWeak = true)
        {
            string assetPath = AssetManager.Instance.GetAssetPath(typeName, abPath, assetName);
            LoadAssetData assetObj = null;
            if (AssetManager.Instance.m_LoadedAssetList.ContainsKey(assetPath))
            {
                assetObj = AssetManager.Instance.m_LoadedAssetList[assetPath];
            }
            else if (AssetManager.Instance.m_LoadingAssetList.ContainsKey(assetPath))
            {
                assetObj = AssetManager.Instance.m_LoadingAssetList[assetPath];
            }

            // 如果已经存在，改变其弱引用关系
            if (assetObj != null)
            {
                assetObj.IsWeak = isWeak;
                if (isWeak && assetObj.RefCount == 0 && !AssetManager.Instance.m_UnloadAssetList.ContainsKey(assetPath))
                {
                    AssetManager.Instance.m_UnloadAssetList.Add(assetPath, assetObj);
                }

                return;
            }

            PreloadAssetData plAssetObj = new PreloadAssetData();
            plAssetObj.TypeName = typeName;
            plAssetObj.ABPath = abPath;
            plAssetObj.AssetName = assetName;
            plAssetObj.AssetPath = assetPath;
            plAssetObj.IsScene = typeName.Equals("Scene");

            plAssetObj.IsWeak = isWeak;

            if (overCallback != null)
            {
                plAssetObj.LoadAssetOverCallback = overCallback;
            }

            AssetManager.Instance.m_PreloadedAsyncAssetList.Enqueue(plAssetObj);
        }

        /// <summary>
        /// 资源销毁
        /// 请保证资源销毁都要调用这个接口
        /// </summary>
        /// <param name="oriAsset">asset资源（原始资源）</param>
        /// <param name="rightNow">是否立刻卸载</param>
        public void Unload_Asset(object oriAsset, UnloadAssetOverCallback overCallback = null,
            bool rightNow = false)
        {
            if (oriAsset == null) return;

            if (oriAsset is UnityEngine.SceneManagement.Scene)
            {
                LoadAssetData matchedScene =
                    Get_Scene_Asset_Object_By_Scene((UnityEngine.SceneManagement.Scene)oriAsset);
                if (matchedScene != null)
                {
                    matchedScene.RefCount--;
                    if (matchedScene.RefCount < 0)
                    {
                        return;
                    }

                    if (matchedScene.RefCount == 0 &&
                        !AssetManager.Instance.m_UnloadAssetList.ContainsKey(matchedScene.AssetPath))
                    {
                        matchedScene.UnloadTickNum = -1;
                        if (overCallback != null)
                        {
                            matchedScene.UnloadAssetOverCallbackList.Add(overCallback);
                        }

                        AssetManager.Instance.m_UnloadAssetList.Add(matchedScene.AssetPath, matchedScene);
                        if (matchedScene.UnloadTickNum < 0)
                        {
                            Update_Unload_Asset();
                        }
                    }
                }
            }
            else
            {
                UnityEngine.Object asset = oriAsset as UnityEngine.Object;
                int instanceID = asset.GetInstanceID();

                if (!AssetManager.Instance.m_AssetInstanceIDAssetList.ContainsKey(instanceID))
                {
                    // 非从本类创建的资源，直接销毁即可
                    if (asset is GameObject)
                    {
                        UnityEngine.Object.Destroy(asset);
                    }

                    //else
                    //{
                    //    if (m_AssetComponent.StrictCheck)
                    //    {
                    //        Log.Error("AssetLoadManager Destroy 无 GameObject name=" + asset.name + " type=" + asset.GetType().Name);
                    //    }
                    //}
                    return;
                }

                var assetObj = AssetManager.Instance.m_AssetInstanceIDAssetList[instanceID];
                if (assetObj.InstanceID == instanceID)
                {
                    assetObj.RefCount--;
                }
                else
                {
                    return;
                }

                if (assetObj.RefCount < 0)
                {
                    return;
                }

                if (assetObj.RefCount == 0 && !AssetManager.Instance.m_UnloadAssetList.ContainsKey(assetObj.AssetPath))
                {
                    assetObj.UnloadTickNum = rightNow
                        ? -1
                        : AssetManager.Instance.m_UnloadAssetDelayFrameNum + AssetManager.Instance.m_UnloadAssetList.Count;
                    if (overCallback != null)
                    {
                        assetObj.UnloadAssetOverCallbackList.Add(overCallback);
                    }

                    AssetManager.Instance.m_UnloadAssetList.Add(assetObj.AssetPath, assetObj);
                    if (assetObj.UnloadTickNum < 0)
                    {
                        Update_Unload_Asset();
                    }
                }
            }
        }

        /// <summary>
        /// 强制卸载未使用的Asset资源（异步）
        /// </summary>
        /// <param name="overcallback">结束回调</param>
        public void Force_Unload_Unused_Assets(Action overcallback = null)
        {
            if (AssetManager.Instance.m_UnloadAssetList.Count > 0)
            {
                AssetManager.Instance.m_TempAssetLoadeds.Clear();
                foreach (var assetObj in AssetManager.Instance.m_UnloadAssetList.Values)
                {
                    if (assetObj.IsWeak && assetObj.RefCount == 0 && assetObj.LoadAssetOverCallbackList.Count == 0)
                    {
                        AssetManager.Instance.m_LoadedAssetList.Remove(assetObj.AssetPath);
                        Do_Remove_Asset_Unload(assetObj);
                        AssetManager.Instance.m_TempAssetLoadeds.Add(assetObj);
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

            AsyncOperation operation = Resources.UnloadUnusedAssets();
            // GC.Collect(); // 等待资源释放完成后再进行GC
            operation.completed += (oper) =>
            {
                GC.Collect();
                if (overcallback != null)
                {
                    overcallback();
                }
            };
        }

        /// <summary>
        /// 管理器心跳
        /// </summary>
        public void Update_Asset()
        {
            Update_Asset_Preload();
            Update_Loaded_Asset_Async();
            Update_Loading_Asset();
            Update_Unload_Asset();
            AssetManager.Instance.m_LoadABComponent.Update();
        }

        /// <summary>
        /// 判断资源文件是否存在
        /// 对打入atlas的图片无法判断，图片请用AtlasLoadMgr
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="abPath"></param>
        /// <param name="assetName"></param>
        /// <returns></returns>
        public  bool Is_File_Exist(string typeName, string abPath, string assetName)
        {
            if (GameConfig.instance.m_EditorResourceMode)
            {
                string absoluteFullPath = AssetManager.Instance.GetAssetAbsoluteFullPath(typeName, abPath, assetName);
                if (string.IsNullOrEmpty(absoluteFullPath))
                {
                    return false;
                }

                return File.Exists(absoluteFullPath);
            }
            else
            {
                return AssetManager.Instance.m_LoadABComponent.IsABExist(abPath);
            }
        }


        /// <summary>
        /// 将外部加载得到的资源加入管理器（给其他地方调用）
        /// </summary>
        /// <param name="typeName">资源类型名称</param>
        /// <param name="abPath">ab路径</param>
        /// <param name="assetName">asset名称</param>
        /// <param name="asset">asset原始资源</param>
        public  void Add_Asset(string typeName, string abPath, string assetName, UnityEngine.Object asset)
        {
            string assetPath = AssetManager.Instance.GetAssetPath(typeName, abPath, assetName);

            var assetObj = new LoadAssetData();
            assetObj.TypeName = typeName;
            assetObj.ABPath = abPath;
            assetObj.AssetName = assetName;
            assetObj.AssetPath = assetPath;
            assetObj.IsScene = typeName.Equals("Scene");

            assetObj.RefCount = 1;

            if (assetObj.IsScene)
            {
                AssetManager.Instance.m_ScenesAsset.Add(assetObj);
            }
            else
            {
                assetObj.InstanceID = asset.GetInstanceID();
                assetObj.Asset = asset;
                if (!AssetManager.Instance.m_AssetInstanceIDAssetList.ContainsKey(assetObj.InstanceID))
                {
                    AssetManager.Instance.m_AssetInstanceIDAssetList.Add(assetObj.InstanceID, assetObj);
                }
                else
                {
                    Log.Error("AssetLoadManager.AddAsset assetObj.InstanceID '{0}' 已经存在。", assetObj.InstanceID);
                }
            }

            AssetManager.Instance.m_LoadedAssetList.Add(assetObj.AssetPath, assetObj);
        }

        /// <summary>
        /// 针对特定资源需要添加引用计数以保证引用计数的正确
        /// </summary>
        /// <param name="typeName">资源类型名称</param>
        /// <param name="abPath">ab路径</param>
        /// <param name="assetName">asset名称</param>
        public  void Add_Asset_Ref(string typeName, string abPath, string assetName)
        {
            string assetPath = AssetManager.Instance.GetAssetPath(typeName, abPath, assetName);

            if (!AssetManager.Instance.m_LoadedAssetList.ContainsKey(assetPath))
            {
                return;
            }

            var assetObj = AssetManager.Instance.m_LoadedAssetList[assetPath];
            assetObj.RefCount++;
        }

        /// <summary>
        /// 解绑回调
        /// </summary>
        /// <param name="typeName">资源类型名称</param>
        /// <param name="abPath">ab路径</param>
        /// <param name="assetName">asset名称</param>
        /// <param name="overCallback">asset资源加载完成的异步回调</param>
        public void Remove_Call_Back(string typeName, string abPath, string assetName,
            LoadAssetOverCallback overCallback)
        {
            if (overCallback == null)
            {
                return;
            }

            string assetPath = AssetManager.Instance.GetAssetPath(typeName, abPath, assetName);

            // 对于不确定asset的回调，将根据回调函数删除
            if (string.IsNullOrEmpty(assetPath))
            {
                Remove_CallBack_Asset_By_CallBack(overCallback);
            }

            LoadAssetData assetObj = null;
            if (AssetManager.Instance.m_LoadedAssetList.ContainsKey(assetPath))
            {
                assetObj = AssetManager.Instance.m_LoadedAssetList[assetPath];
            }
            else if (AssetManager.Instance.m_LoadingAssetList.ContainsKey(assetPath))
            {
                assetObj = AssetManager.Instance.m_LoadingAssetList[assetPath];
            }

            if (assetObj != null)
            {
                int index = assetObj.LoadAssetOverCallbackList.IndexOf(overCallback);
                if (index >= 0)
                {
                    assetObj.LoadAssetOverCallbackList.RemoveAt(index);
                }
            }
        }

        /// <summary>
        /// 从列表中获取加载中的AssetObject封装对象
        /// </summary>
        /// <param name="typeName">类型名称</param>
        /// <param name="abPath">ab资源路径</param>
        /// <param name="assetName">asset名称</param>
        /// <returns></returns>
        public  LoadAssetData Get_Loading_Asset_Object_From_List(string typeName, string abPath, string assetName)
        {
            string assetPath = AssetManager.Instance.GetAssetPath(typeName, abPath, assetName);
            if (!string.IsNullOrEmpty(assetPath) && AssetManager.Instance.m_LoadingAssetList.ContainsKey(assetPath))
            {
                return AssetManager.Instance.m_LoadingAssetList[assetPath];
            }

            return null;
        }

        /// <summary>
        /// 从列表中获取已加载的AssetObject封装对象
        /// </summary>
        /// <param name="typeName">类型名称</param>
        /// <param name="abPath">ab资源路径</param>
        /// <param name="assetName">asset名称</param>
        /// <returns></returns>
        public  LoadAssetData Get_Loaded_Asset_Object_From_List(string typeName, string abPath, string assetName)
        {
            string assetPath = AssetManager.Instance.GetAssetPath(typeName, abPath, assetName);
            if (!string.IsNullOrEmpty(assetPath) && AssetManager.Instance.m_LoadedAssetList.ContainsKey(assetPath))
            {
                return AssetManager.Instance.m_LoadedAssetList[assetPath];
            }

            return null;
        }

        /// <summary>
        /// 从列表中获取卸载中的AssetObject封装对象
        /// </summary>
        /// <param name="typeName">类型名称</param>
        /// <param name="abPath">ab资源路径</param>
        /// <param name="assetName">asset名称</param>
        /// <returns></returns>
        public  LoadAssetData Get_Un_Load_Asset_Object_From_List(string typeName, string abPath, string assetName)
        {
            string assetPath = AssetManager.Instance.GetAssetPath(typeName, abPath, assetName);
            if (!string.IsNullOrEmpty(assetPath) && AssetManager.Instance.m_UnloadAssetList.ContainsKey(assetPath))
            {
                return AssetManager.Instance.m_UnloadAssetList[assetPath];
            }

            return null;
        }
    }
}