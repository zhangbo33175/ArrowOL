/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  PrefabLoadManager.cs
 * author:    云毅
 * created:   2026
 * descrip:   Prefab 加载管理器
 *            负责 Prefab 同步/异步加载、实例化、引用计数、自动销毁、Lua 绑定
 ***************************************************************/
using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace Honor.Runtime
{
    /// <summary>
    /// Prefab 加载管理器（密封分部类）
    /// 基于 AssetLoadManager 封装，专门管理 GameObject 实例化与生命周期
    /// </summary>
    public sealed partial class PrefabLoadManager
    {
        /// <summary>
        /// Prefab 加载管理器构造函数
        /// 初始化容器、依赖引用、日志校验
        /// </summary>
        /// <param name="assetLoadManager">资源加载管理器实例</param>
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
        /// 同步加载 Prefab 并实例化
        /// 自动管理引用计数、异步转同步
        /// </summary>
        /// <param name="abPath">AB 包路径</param>
        /// <param name="assetName">资源名称</param>
        /// <param name="parent">父节点</param>
        /// <param name="luaParams">Lua 传入参数</param>
        /// <returns>实例化后的 GameObject</returns>
        public GameObject LoadSync(string abPath, string assetName, Transform parent, LuaTable luaParams = null)
        {
            string assetPath = m_AssetLoadManager.GetAssetPath("GameObject", abPath, assetName);

            PrefabObject prefabObj = null;
            if (m_LoadedList.ContainsKey(assetPath))
            {
                prefabObj = m_LoadedList[assetPath];
                prefabObj.RefCount++;

                // 异步加载未完成 → 同步加载并临时使用，使用后立即释放
                if (prefabObj.Asset == null)
                {
                    prefabObj.Asset = m_AssetLoadManager.LoadSync("GameObject", abPath, assetName);
                    GameObject newGo = InstanceGO(prefabObj, parent, luaParams);
                    m_AssetLoadManager.Unload(prefabObj.Asset);
                    prefabObj.Asset = null;
                    return newGo;
                }
                else
                {
                    return InstanceGO(prefabObj, parent, luaParams);
                }
            }

            // 全新加载
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
        /// 异步加载 Prefab 并实例化
        /// 支持批量回调、父节点缓存、参数缓存
        /// </summary>
        /// <param name="abPath">AB 包路径</param>
        /// <param name="assetName">资源名称</param>
        /// <param name="parent">父节点</param>
        /// <param name="luaParams">Lua 参数</param>
        /// <param name="overCallback">加载完成回调</param>
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

            // 新建异步加载对象
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
                    // 锁定回调数量，防止加载过程中列表变更
                    prefabObj.LockCallbackCount = prefabObj.PrefabLoadOverCallbackList.Count;
                    InstanceGOWithCallback(prefabObj);
                });
        }

        /// <summary>
        /// 直接克隆 GameObject（非托管方式）
        /// 注意：仅用于不受管理器管控的对象
        /// </summary>
        /// <param name="parent">父节点</param>
        /// <param name="childTemplateGO">模板对象</param>
        /// <param name="luaParams">Lua 参数</param>
        /// <returns>克隆后的对象</returns>
        public GameObject InstantiateGO(Transform parent, GameObject childTemplateGO, LuaTable luaParams)
        {
            GameObject go = GameObject.Instantiate(childTemplateGO, parent, false);
            go.name = go.name.Replace("(Clone)", string.Empty);
            PrefabInstanceGOBehaviour goBehaviour = go.AddComponent<PrefabInstanceGOBehaviour>();

            // 强制激活一次确保 Awake/OnDestroy 正常执行
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
                    child2.transform.GetRouteNum() - child1.transform.GetRouteNum());

                foreach (var childBehaviour in childBehaviours)
                {
                    if (childBehaviour != luaBehaviour)
                    {
                        if (!childBehaviour.gameObject.activeSelf)
                        {
                            childBehaviour.gameObject.SetActive(true);
                            childBehaviour.gameObject.SetActive(false);
                        }
                        else
                        {
                            if (!childBehaviour.gameObject.activeInHierarchy)
                            {
                                GameObject nearestInactiveParentInHierarchy =
                                    GetNearestInactiveParentInHierarchy(childBehaviour.gameObject);
                                nearestInactiveParentInHierarchy.SetActive(true);
                                nearestInactiveParentInHierarchy.SetActive(false);
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
        /// 管理器帧更新
        /// 驱动异步回调
        /// </summary>
        public void Update()
        {
            UpdateLoadedAsync();
        }

        /// <summary>
        /// 手动增加资源引用计数（用于克隆对象）
        /// </summary>
        /// <param name="abPath">AB 路径</param>
        /// <param name="assetName">资源名</param>
        /// <param name="go">实例对象</param>
        public void AddAssetRef(string abPath, string assetName, GameObject go)
        {
            string assetPath = m_AssetLoadManager.GetAssetPath("GameObject", abPath, assetName);

            if (!m_LoadedList.ContainsKey(assetPath))
                return;

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
        /// 销毁 GameObject 实例
        /// 自动维护引用计数，计数为 0 时释放资源
        /// </summary>
        /// <param name="go">对象</param>
        /// <param name="rightNow">是否立即卸载</param>
        public void Destroy(GameObject go, bool rightNow = false)
        {
            if (go == null) 
                return;

            int instanceID = go.GetInstanceID();

            if (!m_GOInstanceIDList.ContainsKey(instanceID))
            {
                UnityEngine.Object.Destroy(go);
                return;
            }

            PrefabObject prefabObj = m_GOInstanceIDList[instanceID];
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

            // 引用归零 → 释放资源
            if (prefabObj.RefCount == 0)
            {
                m_LoadedList.Remove(prefabObj.AssetPath);
                m_AssetLoadManager.Unload(prefabObj.Asset, null, rightNow);
                prefabObj.Asset = null;
            }
        }

        /// <summary>
        /// 移除异步加载回调
        /// 同时维护计数、参数、父节点列表
        /// </summary>
        /// <param name="abPath">AB 路径</param>
        /// <param name="assetName">资源名</param>
        /// <param name="overCallback">回调</param>
        public void RemoveCallBack(string abPath, string assetName, PrefabLoadOverCallback overCallback)
        {
            if (overCallback == null) 
                return;

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

                    // 修正锁定计数
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