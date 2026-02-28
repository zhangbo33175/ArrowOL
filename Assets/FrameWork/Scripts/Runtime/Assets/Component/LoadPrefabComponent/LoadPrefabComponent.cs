using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace Honor.Runtime
{
    public partial class LoadPrefabComponent
    {
        /// <summary>
        /// 同步加载Prefab并实例化
        /// </summary>
        /// <param name="abPath">ab路径</param>
        /// <param name="assetName">asset名称</param>
        /// <param name="parent">为实例指定的父对象</param>
        /// <param name="luaParams">自定义lua传入参数</param>
        /// <returns></returns>
        public GameObject Load_Prefab_Sync(string abPath, string assetName, Transform parent, LuaTable luaParams = null)
        {
            string assetPath = AssetManager.Instance.GetAssetPath("GameObject", abPath, assetName);
            LoadPrefabData _PrefabData = null;
            //检查路径下是否存在
            if (AssetManager.Instance.CheckAssetPath(AssetManager.Instance.m_LoadedPrefabList, assetPath))
            {
                _PrefabData = AssetManager.Instance.m_LoadedPrefabList[assetPath];
                //引用基数
                _PrefabData.RefCount++;
                //如果当前的Prefab正在一步加载中
                if (_PrefabData.Asset == null)
                {
                    //加载文件
                    _PrefabData.Asset = AssetManager.Instance.GetLoadAssetSync("GameObject", abPath, assetName);
                    //实例化对象
                    var newGo = Instance_Prefab_GO(_PrefabData, parent, luaParams);
                    AssetManager.Instance.m_LoadAssetComponent.Unload_Asset(_PrefabData.Asset);
                    // 等待异步加载结束后重新赋值
                    _PrefabData.Asset = null;
                    return newGo;
                }
                else
                {
                    return Instance_Prefab_GO(_PrefabData, parent, luaParams);
                }
            }

            _PrefabData = new LoadPrefabData();
            _PrefabData.ABPath = abPath;
            _PrefabData.AssetName = assetName;
            _PrefabData.AssetPath = assetPath;
            _PrefabData.RefCount = 1;
            _PrefabData.Asset = AssetManager.Instance.m_LoadAssetComponent.Load_Asset_Sync("GameObject", abPath, assetName);

            AssetManager.Instance.m_LoadedPrefabList.Add(assetPath, _PrefabData);

            return Instance_Prefab_GO(_PrefabData, parent, luaParams);
        }

        /// <summary>
        /// 异步加载Prefab并实例化
        /// </summary>
        /// <param name="abPath">ab路径</param>
        /// <param name="assetName">asset名称</param>
        /// <param name="parent">为实例指定的父节点</param>
        /// <param name="luaParams">自定义lua传入参数</param>
        /// <param name="overCallback">prefab加载完成并实例化结束的异步回调</param>
        public void Load_Prefab_Async(string abPath, string assetName, Transform parent,
            LuaTable luaParams = null,
            LoadPrefabOverCallback overCallback = null)
        {
            string assetPath = AssetManager.Instance.GetAssetPath("GameObject", abPath, assetName);

            LoadPrefabData prefabObj = null;
            if (AssetManager.Instance.m_LoadedPrefabList.ContainsKey(assetPath))
            {
                prefabObj = AssetManager.Instance.m_LoadedPrefabList[assetPath];
                prefabObj.PrefabLoadOverCallbackList.Add(overCallback);
                prefabObj.PrefabLoadLuaTableParamList.Add(luaParams);
                prefabObj.PrefabInstancingGOParentList.Add(parent);
                prefabObj.RefCount++;
                if (prefabObj.Asset != null)
                {
                    AssetManager.Instance.m_LoadedPrefabAsyncTmpAgentList.Add(prefabObj);
                }

                return;
            }

            prefabObj = new LoadPrefabData();
            prefabObj.ABPath = abPath;
            prefabObj.AssetName = assetName;
            prefabObj.AssetPath = assetPath;
            prefabObj.PrefabLoadOverCallbackList.Add(overCallback);
            prefabObj.PrefabLoadLuaTableParamList.Add(luaParams);
            prefabObj.PrefabInstancingGOParentList.Add(parent);
            prefabObj.RefCount = 1;

            AssetManager.Instance.m_LoadedPrefabList.Add(assetPath, prefabObj);

            AssetManager.Instance.m_LoadAssetComponent.Load_Asset_Async("GameObject", abPath, assetName,
                (LoadAssetData assetObject, UnityEngine.Object obj) =>
                {
                    prefabObj.Asset = obj;
                    // 异步加载完成后最大程度收集接下来需要进行回调的数量
                    prefabObj.LockCallbackCount = prefabObj.PrefabLoadOverCallbackList.Count;
                    // 异步回调处理
                    Instance_Prefab_GO_With_Callback(prefabObj);
                });
        }

        /// <summary>
        /// 挂载模板的克隆对象到指定节点上
        /// 注：当GO是一个不被各类Manager管控的对象时，可以使用该克隆接口，其他情况禁止使用该克隆接口，因为克隆得到的对象将不受各类Manager的管控！
        ///     比如：UI对象，只能通过UIManager进行实例化，克隆方式的实例化对象并不在UIManager管理容器内！
        /// </summary>
        /// <param name="parent">父节点</param>
        /// <param name="childTemplateGO">模板节点</param>
        /// <param name="luaParams">传入参数</param>
        /// <returns>克隆得到的节点对象</returns>
        public GameObject InstantiateGO(Transform parent, GameObject childTemplateGO, LuaTable luaParams)
        {
            GameObject go = GameObject.Instantiate(childTemplateGO, parent, false) as GameObject;
            go.name = go.name.Replace("(Clone)", "");
            PrefabInstanceGOBehaviour goBehaviour = go.AddComponent<PrefabInstanceGOBehaviour>();

            if (!go.activeSelf)
            {
                LuaBehaviour luaBehaviour = go.GetComponent<LuaBehaviour>();
                go.SetActive(true);
                go.SetActive(false);
            }

            int instanceID = go.GetInstanceID();
            if (goBehaviour != null)
            {
                goBehaviour.InstanceID = instanceID;
                LuaBehaviour luaBehaviour = go.GetComponent<LuaBehaviour>();
                List<LuaBehaviour> childBehaviours = new List<LuaBehaviour>();
                go.GetComponentsInChildren(true, childBehaviours);
                childBehaviours.Sort((child1, child2) =>
                {
                    return child2.transform.GetRouteNum() - child1.transform.GetRouteNum();
                });

                foreach (var childBehaviour in childBehaviours)
                {
                    if (childBehaviour != luaBehaviour)
                    {
                        if (!childBehaviour.gameObject.activeSelf)
                        {
                            // 保证GameObject active一次，ObjInfo才能触发Awake，未Awake的脚本不能触发OnDestroy，不触发Awake和OnDestroy的情况下引用计数会出错
                            childBehaviour.gameObject.SetActive(true);
                            childBehaviour.gameObject.SetActive(false);
                        }
                        else
                        {
                            // 向上追溯最近的hierarchy中inactive的父节点
                            if (!childBehaviour.gameObject.activeInHierarchy)
                            {
                                GameObject nearestInactiveParentInHierarchy =
                                    GetNearestInactiveParentInHierarchy(childBehaviour.gameObject);
                                nearestInactiveParentInHierarchy.gameObject.SetActive(true);
                                nearestInactiveParentInHierarchy.gameObject.SetActive(false);
                            }
                        }

                        childBehaviour.AwakeAppended();
                        childBehaviour.OnEnableAppended();
                    }
                }

                if (luaBehaviour != null)
                {
                    goBehaviour.LuaBehaviour = luaBehaviour;
                    luaBehaviour.LuaParams = luaParams;
                    luaBehaviour.AwakeAppended();
                    luaBehaviour.OnEnableAppended();
                }
            }

            return go;
        }

        /// <summary>
        /// 管理器心跳
        /// </summary>
        public void Update()
        {
            UpdateLoadedAsync();
        }

        /// <summary>
        /// 针对特定资源需要添加引用计数以保证引用计数的正确
        /// </summary>
        /// <param name="abPath">ab路径</param>
        /// <param name="assetName">asset名称</param>
        /// <param name="go">实例</param>
        public void AddAssetRef(string abPath, string assetName, GameObject go)
        {
            string assetPath = AssetManager.Instance.GetAssetPath("GameObject", abPath, assetName);

            if (!AssetManager.Instance.m_LoadedPrefabList.ContainsKey(assetPath))
            {
                return;
            }

            LoadPrefabData prefabObj = AssetManager.Instance.m_LoadedPrefabList[assetPath];

            int instanceID = go.GetInstanceID();
            if (!AssetManager.Instance.m_GOInstanceIDPrefabList.ContainsKey(instanceID))
            {
                prefabObj.RefCount++;
                prefabObj.GOInstanceIDs.Add(instanceID);
                AssetManager.Instance.m_GOInstanceIDPrefabList.Add(instanceID, prefabObj);
            }
        }

        /// <summary>
        /// 销毁实例
        /// </summary>
        /// <param name="go">实例</param>
        /// <param name="rightNow">马上</param>
        public void Destroy(GameObject go, bool rightNow = false)
        {
            if (go == null) return;

            int instanceID = go.GetInstanceID();

            if (!AssetManager.Instance.m_GOInstanceIDPrefabList.ContainsKey(instanceID))
            {
                // 非从本类创建的资源，直接销毁即可
                if (go is GameObject)
                {
                    UnityEngine.Object.Destroy(go);
                }
                else
                {
                    Log.Error("PrefabLoadMgr destroy 无GameObject name = {0} type = {1} ", go.name,
                        go.GetType().Name);
                }

                return;
            }

            var prefabObj = AssetManager.Instance.m_GOInstanceIDPrefabList[instanceID];
            if (prefabObj.GOInstanceIDs.Contains(instanceID))
            {
                prefabObj.RefCount--;
                prefabObj.GOInstanceIDs.Remove(instanceID);
                AssetManager.Instance.m_GOInstanceIDPrefabList.Remove(instanceID);

                UnityEngine.Object.Destroy(go);
            }
            else
            {
                Log.Error("PrefabLoadMgr Destroy 错误！ assetName:{0}", prefabObj.AssetName);
                return;
            }

            if (prefabObj.RefCount < 0)
            {
                Log.Error("PrefabLoadMgr Destroy 引用计数错误！ assetName:{0}", prefabObj.AssetName);
                return;
            }

            if (prefabObj.RefCount == 0)
            {
                AssetManager.Instance.m_LoadedPrefabList.Remove(prefabObj.AssetPath);
                AssetManager.Instance.m_LoadAssetComponent.Unload_Asset(prefabObj.Asset, null, rightNow);
                prefabObj.Asset = null;
            }
        }

        /// <summary>
        /// 解绑回调
        /// </summary>
        /// <param name="abPath">ab路径</param>
        /// <param name="assetName">asset名称</param>
        /// <param name="overCallback">prefab加载完成并实例化结束的异步回调</param>
        public void RemoveCallBack(string abPath, string assetName, LoadPrefabOverCallback overCallback)
        {
            if (overCallback == null) return;

            string assetPath = AssetManager.Instance.GetAssetPath("GameObject", abPath, assetName);

            LoadPrefabData prefabObj = null;
            if (AssetManager.Instance.m_LoadedPrefabList.ContainsKey(assetPath))
            {
                prefabObj = AssetManager.Instance.m_LoadedPrefabList[assetPath];
            }

            if (prefabObj != null)
            {
                int index = prefabObj.PrefabLoadOverCallbackList.IndexOf(overCallback);
                if (index >= 0)
                {
                    prefabObj.RefCount--;
                    prefabObj.PrefabLoadOverCallbackList.RemoveAt(index);
                    prefabObj.PrefabLoadLuaTableParamList.RemoveAt(index);
                    prefabObj.PrefabInstancingGOParentList.RemoveAt(index);

                    // 如果是加载回调过程中解绑回调，需要降低lock个数
                    if (index < prefabObj.LockCallbackCount)
                    {
                        prefabObj.LockCallbackCount--;
                    }
                }

                if (prefabObj.RefCount < 0)
                {
                    Log.Error("PrefabLoadMgr Destroy 引用计数错误！ assetName:{0}", prefabObj.AssetName);
                    return;
                }

                if (prefabObj.RefCount == 0)
                {
                    AssetManager.Instance.m_LoadedPrefabList.Remove(prefabObj.AssetPath);
                    AssetManager.Instance.m_LoadAssetComponent.Unload_Asset(prefabObj.Asset);
                    prefabObj.Asset = null;
                }
            }
        }
    }
}