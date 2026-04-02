using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace Honor.Runtime
{
    public sealed partial class PrefabLoadManager
    {
        /// <summary>
        /// Prefab加载管理器构造方法
        /// </summary>
        public PrefabLoadManager(AssetLoadManager assetLoadManager)
        {
            m_LoadedList = new Dictionary<string, PrefabObject>();
            m_LoadedAsyncTmpAgentList = new List<PrefabObject>();
            m_GOInstanceIDList = new Dictionary<int, PrefabObject>();

            if (assetLoadManager == null)
            {
                Log.Fatal("AssetLoadManager 无效。");
                return;
            }

            m_AssetLoadManager = assetLoadManager;

            AssetComponent assetComponent = GameComponentsGroup.GetComponent<AssetComponent>();
            if (assetComponent == null)
            {
                Log.Fatal("Asset component 无效。");
                return;
            }
        }

        /// <summary>
        /// 同步加载Prefab并实例化
        /// </summary>
        /// <param name="abPath">ab路径</param>
        /// <param name="assetName">asset名称</param>
        /// <param name="parent">为实例指定的父对象</param>
        /// <param name="luaParams">自定义lua传入参数</param>
        /// <returns></returns>
        public GameObject LoadSync(string abPath, string assetName, Transform parent, LuaTable luaParams = null)
        {
            string assetPath = m_AssetLoadManager.GetAssetPath("GameObject", abPath, assetName);

            PrefabObject prefabObj = null;
            if (m_LoadedList.ContainsKey(assetPath))
            {
                prefabObj = m_LoadedList[assetPath];
                prefabObj.RefCount++;

                // 如果当前prefab正在异步加载中（不能影响异步加载，加载后需要释放）
                if (prefabObj.Asset == null)
                {
                    prefabObj.Asset = m_AssetLoadManager.LoadSync("GameObject", abPath, assetName);
                    var newGo = InstanceGO(prefabObj, parent, luaParams);
                    m_AssetLoadManager.Unload(prefabObj.Asset);
                    prefabObj.Asset = null; // 等待异步加载结束后重新赋值
                    return newGo;
                }
                else
                {
                    return InstanceGO(prefabObj, parent, luaParams);
                }
            }

            prefabObj = new PrefabObject();
            prefabObj.AssetBundlePath = abPath;
            prefabObj.AssetName = assetName;
            prefabObj.AssetPath = assetPath;
            prefabObj.RefCount = 1;
            prefabObj.Asset = m_AssetLoadManager.LoadSync("GameObject", abPath, assetName);

            m_LoadedList.Add(assetPath, prefabObj);

            return InstanceGO(prefabObj, parent, luaParams);
        }

        /// <summary>
        /// 异步加载Prefab并实例化
        /// </summary>
        /// <param name="abPath">ab路径</param>
        /// <param name="assetName">asset名称</param>
        /// <param name="parent">为实例指定的父节点</param>
        /// <param name="luaParams">自定义lua传入参数</param>
        /// <param name="overCallback">prefab加载完成并实例化结束的异步回调</param>
        public void LoadAsync(string abPath, string assetName, Transform parent, LuaTable luaParams = null,
            PrefabLoadOverCallback overCallback = null)
        {
            string assetPath = m_AssetLoadManager.GetAssetPath("GameObject", abPath, assetName);

            PrefabObject prefabObj = null;
            if (m_LoadedList.ContainsKey(assetPath))
            {
                prefabObj = m_LoadedList[assetPath];
                prefabObj.PrefabLoadOverCallbackList.Add(overCallback);
                prefabObj.PrefabLoadLuaTableParamList.Add(luaParams);
                prefabObj.PrefabInstancingGOParentList.Add(parent);
                prefabObj.RefCount++;
                if (prefabObj.Asset != null)
                {
                    m_LoadedAsyncTmpAgentList.Add(prefabObj);
                }

                return;
            }

            prefabObj = new PrefabObject();
            prefabObj.AssetBundlePath = abPath;
            prefabObj.AssetName = assetName;
            prefabObj.AssetPath = assetPath;
            prefabObj.PrefabLoadOverCallbackList.Add(overCallback);
            prefabObj.PrefabLoadLuaTableParamList.Add(luaParams);
            prefabObj.PrefabInstancingGOParentList.Add(parent);
            prefabObj.RefCount = 1;

            m_LoadedList.Add(assetPath, prefabObj);

            m_AssetLoadManager.LoadAsync("GameObject", abPath, assetName,
                (AssetObject assetObject, UnityEngine.Object obj) =>
                {
                    prefabObj.Asset = obj;
                    // 异步加载完成后最大程度收集接下来需要进行回调的数量
                    prefabObj.LockCallbackCount = prefabObj.PrefabLoadOverCallbackList.Count;
                    // 异步回调处理
                    InstanceGOWithCallback(prefabObj);
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
                // 保证GameObject active一次，ObjInfo才能触发Awake，未Awake的脚本不能触发OnDestroy，不触发Awake和OnDestroy的情况下引用计数会出错
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
            string assetPath = m_AssetLoadManager.GetAssetPath("GameObject", abPath, assetName);

            if (!m_LoadedList.ContainsKey(assetPath))
            {
                return;
            }

            PrefabObject prefabObj = m_LoadedList[assetPath];

            int instanceID = go.GetInstanceID();
            if (!m_GOInstanceIDList.ContainsKey(instanceID))
            {
                prefabObj.RefCount++;
                prefabObj.GOInstanceIDs.Add(instanceID);
                m_GOInstanceIDList.Add(instanceID, prefabObj);
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

            if (!m_GOInstanceIDList.ContainsKey(instanceID))
            {
                // 非从本类创建的资源，直接销毁即可
                if (go is GameObject)
                {
                    UnityEngine.Object.Destroy(go);
                }
                else
                {
                    Log.Error("PrefabLoadMgr destroy 无GameObject name = {0} type = {1} ", go.name, go.GetType().Name);
                }

                return;
            }

            var prefabObj = m_GOInstanceIDList[instanceID];
            if (prefabObj.GOInstanceIDs.Contains(instanceID))
            {
                prefabObj.RefCount--;
                prefabObj.GOInstanceIDs.Remove(instanceID);
                m_GOInstanceIDList.Remove(instanceID);

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
                m_LoadedList.Remove(prefabObj.AssetPath);
                m_AssetLoadManager.Unload(prefabObj.Asset, null, rightNow);
                prefabObj.Asset = null;
            }
        }

        /// <summary>
        /// 解绑回调
        /// </summary>
        /// <param name="abPath">ab路径</param>
        /// <param name="assetName">asset名称</param>
        /// <param name="overCallback">prefab加载完成并实例化结束的异步回调</param>
        public void RemoveCallBack(string abPath, string assetName, PrefabLoadOverCallback overCallback)
        {
            if (overCallback == null) return;

            string assetPath = m_AssetLoadManager.GetAssetPath("GameObject", abPath, assetName);

            PrefabObject prefabObj = null;
            if (m_LoadedList.ContainsKey(assetPath))
            {
                prefabObj = m_LoadedList[assetPath];
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
                    m_LoadedList.Remove(prefabObj.AssetPath);
                    m_AssetLoadManager.Unload(prefabObj.Asset);
                    prefabObj.Asset = null;
                }
            }
        }
    }
}