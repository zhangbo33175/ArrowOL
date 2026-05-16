/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  PrefabLoadManager.cs
 * author:    云毅
 * created:   2026
 * descrip:   Prefab 加载管理器 - 内部实例化 & 回调实现
 *            包含 GameObject 实例化、Lua 绑定、异步回调派发、层级修复
 ***************************************************************/
using System;
using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace Honor.Runtime
{
    /// <summary>
    /// Prefab 加载管理器（内部实现分部类）
    /// </summary>
    public sealed partial class PrefabLoadManager
    {
        /// <summary>
        /// 内部实例化 GameObject
        /// 处理父节点异常、生命周期激活、Lua 脚本绑定、引用计数管理
        /// </summary>
        /// <param name="prefabObj">Prefab 包装对象</param>
        /// <param name="parent">父节点</param>
        /// <param name="luaParams">Lua 传入参数</param>
        /// <returns>实例化完成的对象</returns>
        private GameObject InstanceGO(PrefabObject prefabObj, Transform parent, LuaTable luaParams)
        {
            Transform tempParent = parent;

            // 父对象无效或处于非激活状态 → 临时挂载到根节点，确保 Awake 能正常执行
            if (parent == null || parent.gameObject == null || !parent.gameObject.activeInHierarchy)
            {
                tempParent = GameMainRoot.Asset.transform;
            }

            // 实例化对象并去除 (Clone) 后缀
            GameObject go = GameObject.Instantiate(prefabObj.Asset, tempParent, false) as GameObject;
            go.name = go.name.Replace("(Clone)", "");
            PrefabInstanceGOBehaviour goBehaviour = go.AddComponent<PrefabInstanceGOBehaviour>();

            // 强制激活一次，确保 Awake / OnDestroy 生命周期正常
            if (!go.activeSelf)
            {
                LuaBehaviour luaBehaviour = go.GetComponent<LuaBehaviour>();
                go.SetActive(true);
                go.SetActive(false);
            }

            // 恢复真实父节点
            if (parent != null)
            {
                go.transform.SetParent(parent, false);
            }

            int instanceID = go.GetInstanceID();
            if (goBehaviour != null)
            {
                goBehaviour.InstanceID = instanceID;
                goBehaviour.ABPath = prefabObj.AssetBundlePath;
                goBehaviour.AssetName = prefabObj.AssetName;

                LuaBehaviour luaBehaviour = go.GetComponent<LuaBehaviour>();
                List<LuaBehaviour> childBehaviours = new List<LuaBehaviour>();
                go.GetComponentsInChildren(true, childBehaviours);
                childBehaviours.Sort((child1, child2)=> { return child2.transform.GetRouteNum() - child1.transform.GetRouteNum(); });

                foreach (var childBehaviour in childBehaviours)
                {
                    if (childBehaviour != luaBehaviour)
                    {
                        // 未激活对象强制激活一次，保证生命周期正常
                        if (!childBehaviour.gameObject.activeSelf)
                        {
                            childBehaviour.gameObject.SetActive(true);
                            childBehaviour.gameObject.SetActive(false);
                        }
                        else
                        {
                            // 层级未激活 → 向上查找最近未激活父节点并临时激活
                            if (!childBehaviour.gameObject.activeInHierarchy)
                            {
                                GameObject nearestInactiveParentInHierarchy = GetNearestInactiveParentInHierarchy(childBehaviour.gameObject);
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

            // 记录实例 ID，维护引用计数
            prefabObj.GOInstanceIDs.Add(instanceID);
            m_GOInstanceIDList.Add(instanceID, prefabObj);

            return go;
        }

        /// <summary>
        /// 实例化并执行异步回调
        /// 先缓存回调列表，防止执行过程中列表被修改
        /// </summary>
        /// <param name="prefabObj">Prefab 包装对象</param>
        private void InstanceGOWithCallback(PrefabObject prefabObj)
        {
            if (prefabObj.PrefabLoadOverCallbackList.Count == 0) return;

            // 锁定回调数据，避免回调体内操作导致列表异常
            int count = prefabObj.LockCallbackCount;
            var callbackList = prefabObj.PrefabLoadOverCallbackList.GetRange(0, count);
            var luaParamList = prefabObj.PrefabLoadLuaTableParamList.GetRange(0, count);
            var callParentList = prefabObj.PrefabInstancingGOParentList.GetRange(0, count);

            // 清空已锁定的回调
            prefabObj.LockCallbackCount = 0;
            prefabObj.PrefabLoadOverCallbackList.RemoveRange(0, count);
            prefabObj.PrefabLoadLuaTableParamList.RemoveRange(0, count);
            prefabObj.PrefabInstancingGOParentList.RemoveRange(0, count);

            for (int i = 0; i < count; i++)
            {
                if (callbackList[i] != null)
                {
                    GameObject go = InstanceGO(prefabObj, callParentList[i], luaParamList[i]);

                    try
                    {
                        callbackList[i](prefabObj, go);
                    }
                    catch (GameException e)
                    {
                        Log.Error(e);
                    }
                }
            }
        }

        /// <summary>
        /// 异步加载完成回调派发
        /// 处理“已加载完成但需要异步回调”的对象
        /// </summary>
        private void UpdateLoadedAsync()
        {
            if (m_LoadedAsyncTmpAgentList.Count == 0) return;

            int count = m_LoadedAsyncTmpAgentList.Count;
            for (int i = 0; i < count; i++)
            {
                // 提前锁定回调数量
                m_LoadedAsyncTmpAgentList[i].LockCallbackCount = m_LoadedAsyncTmpAgentList[i].PrefabLoadOverCallbackList.Count;
            }

            for (int i = 0; i < count; i++)
            {
                InstanceGOWithCallback(m_LoadedAsyncTmpAgentList[i]);
            }
            m_LoadedAsyncTmpAgentList.RemoveRange(0, count);
        }

        /// <summary>
        /// 向上查找 Hierarchy 中最近的未激活父物体
        /// 用于修复非激活节点无法触发 Awake 的问题
        /// </summary>
        private GameObject GetNearestInactiveParentInHierarchy(GameObject go)
        {
            Transform parent = go.transform.parent;
            while (parent.gameObject.activeSelf || !parent.parent.gameObject.activeSelf)
            {
                parent = parent.parent;
            }

            return parent.gameObject;
        }
    }
}