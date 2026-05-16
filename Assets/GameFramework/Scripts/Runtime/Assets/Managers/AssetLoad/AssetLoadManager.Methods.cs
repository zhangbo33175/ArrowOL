/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  AssetLoadManager.cs
 * author:    云毅
 * created:   2026
 * descrip:   资源加载管理器 - 内部工具方法 & 帧更新驱动实现
 *            回调管理、卸载逻辑、预加载、Update驱动流程
 ***************************************************************/
using System;
using UnityEngine;

namespace Honor.Runtime
{
    //=========================================================================
    // 资源加载管理器 - 内部实现 & 帧更新逻辑
    // 回调处理、卸载执行、预加载调度、Update 驱动
    //=========================================================================
    /// <summary>
    /// 资源加载管理器（内部逻辑分部类）
    /// 包含异步回调、资源卸载、帧更新驱动、场景查找等内部实现
    /// </summary>
    public sealed partial class AssetLoadManager
    {
        #region 全局解绑加载完成回调
        /// <summary>
        /// 根据回调委托，全局解绑加载完成回调
        /// 遍历加载中/已加载列表，移除指定回调
        /// </summary>
        /// <param name="overCallback">要移除的加载回调</param>
        private void RemoveCallBackByCallBack(AssetLoadOverCallback overCallback)
        {
            foreach (var assetObj in m_LoadingList.Values)
            {
                if (assetObj.AssetLoadOverCallbackList.Count == 0) 
                    continue;

                int index = assetObj.AssetLoadOverCallbackList.IndexOf(overCallback);
                if (index >= 0)
                {
                    assetObj.AssetLoadOverCallbackList.RemoveAt(index);
                }
            }

            foreach (var assetObj in m_LoadedList.Values)
            {
                if (assetObj.AssetLoadOverCallbackList.Count == 0) 
                    continue;

                int index = assetObj.AssetLoadOverCallbackList.IndexOf(overCallback);
                if (index >= 0)
                {
                    assetObj.AssetLoadOverCallbackList.RemoveAt(index);
                }
            }
        }
        #endregion

        #region 执行资源加载完成回调
        /// <summary>
        /// 执行资源加载完成回调
        /// 先锁定回调数量，防止回调体内再次修改列表导致异常
        /// </summary>
        /// <param name="assetObj">资源包装对象</param>
        private void DoAssetCallback(AssetObject assetObj)
        {
            if (assetObj.AssetLoadOverCallbackList.Count == 0)
                return;

            // 先提取count，保证回调中有加载需求不影响本次执行
            int count = assetObj.LockCallbackCount;
            for (int i = 0; i < count; i++)
            {
                if (assetObj.AssetLoadOverCallbackList[i] != null)
                {
                    // 回调期间引用计数 +1，保证资源不被释放
                    assetObj.RefCount++;

                    try
                    {
                        assetObj.AssetLoadOverCallbackList[i](assetObj, assetObj.Asset);
                    }
                    catch (GameException e)
                    {
                        Log.Error(e);
                    }
                }
            }

            assetObj.AssetLoadOverCallbackList.RemoveRange(0, count);
        }
        #endregion

        #region 内部执行资源卸载逻辑
        /// <summary>
        /// 内部执行资源卸载逻辑
        /// 处理场景卸载、AB包引用递减、实例ID清理、卸载回调
        /// </summary>
        /// <param name="assetObj">资源包装对象</param>
        private void DoUnload(AssetObject assetObj)
        {
            if (assetObj.IsScene)
            {
                UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(assetObj.AssetName);
            }

            if (!m_EditorResourceMode)
            {
                _mAssetBundleLoadManager.Unload(assetObj.AssetBundlePath);
            }

            assetObj.Asset = null;

            if (assetObj.IsScene)
            {
                m_Scenes.Remove(assetObj);
            }
            else
            {
                if (m_AssetInstanceIDList.ContainsKey(assetObj.InstanceID))
                {
                    m_AssetInstanceIDList.Remove(assetObj.InstanceID);
                }
            }

            // 触发卸载完成回调
            if (assetObj.AssetUnloadOverCallbackList != null)
            {
                foreach (AssetUnloadOverCallback overCallback in assetObj.AssetUnloadOverCallbackList)
                {
                    overCallback?.Invoke(assetObj);
                }
            }
        }
        #endregion

        #region 预加载队列帧更新
        /// <summary>
        /// 预加载队列帧更新
        /// 当加载队列为空时，从预加载队列取一个进行加载
        /// </summary>
        private void UpdatePreload()
        {
            if (m_LoadingList.Count > 0 || m_PreloadedAsyncList.Count == 0)
                return;

            PreloadAssetObject plAssetObj = null;
            while (m_PreloadedAsyncList.Count > 0 && plAssetObj == null)
            {
                plAssetObj = m_PreloadedAsyncList.Dequeue();

                if (m_LoadingList.ContainsKey(plAssetObj.AssetPath))
                {
                    m_LoadingList[plAssetObj.AssetPath].IsWeak = plAssetObj.IsWeak;
                }
                else if (m_LoadedList.ContainsKey(plAssetObj.AssetPath))
                {
                    m_LoadedList[plAssetObj.AssetPath].IsWeak = plAssetObj.IsWeak;
                    plAssetObj = null;
                }
                else
                {
                    LoadAsync(plAssetObj.TypeName, plAssetObj.AssetBundlePath, plAssetObj.AssetName, plAssetObj.AssetLoadOverCallback);
                    
                    if (m_LoadingList.ContainsKey(plAssetObj.AssetPath))
                    {
                        m_LoadingList[plAssetObj.AssetPath].IsWeak = plAssetObj.IsWeak;
                    }
                    else if (m_LoadedList.ContainsKey(plAssetObj.AssetPath))
                    {
                        m_LoadedList[plAssetObj.AssetPath].IsWeak = plAssetObj.IsWeak;
                    }
                }
            }
        }
        #endregion

        #region 异步加载完成回调派发
        /// <summary>
        /// 异步加载完成回调派发
        /// 统一触发已加载完成资源的回调
        /// </summary>
        private void UpdateLoadedAsync()
        {
            if (m_LoadedAsyncTmpAgentList.Count == 0) 
                return;

            int count = m_LoadedAsyncTmpAgentList.Count;
            for (int i = 0; i < count; i++)
            {
                // 锁定回调数量，防止异步过程中列表变化
                m_LoadedAsyncTmpAgentList[i].LockCallbackCount = m_LoadedAsyncTmpAgentList[i].AssetLoadOverCallbackList.Count;
            }

            for (int i = 0; i < count; i++)
            {
                DoAssetCallback(m_LoadedAsyncTmpAgentList[i]);
            }

            m_LoadedAsyncTmpAgentList.RemoveRange(0, count);

            // 大量加载完成后触发一次GC优化内存
            if (m_LoadingList.Count == 0 && m_LoadingIntervalCount > m_LoadedMaxNumToCleanMemery)
            {
                m_LoadingIntervalCount = 0;
                GC.Collect();
            }
        }
        #endregion

        #region 加载中列表帧更新
        /// <summary>
        /// 加载中列表帧更新
        /// 检测异步加载完成的资源，移入已加载列表并触发回调
        /// </summary>
        private void UpdateLoading()
        {
            if (m_LoadingList.Count == 0) 
                return;

            m_TempLoadeds.Clear();
            foreach (var assetObj in m_LoadingList.Values)
            {
                if (m_EditorResourceMode)
                {
#if UNITY_EDITOR
                    if (assetObj.IsScene)
                    {
                        if (assetObj.Request != null && assetObj.Request.isDone)
                        {
                            m_Scenes.Add(assetObj);
                            assetObj.Request = null;
                            m_TempLoadeds.Add(assetObj);
                        }
                    }
                    else
                    {
                        string assetRelativeFullPath = GetAssetRelativeFullPath(assetObj.TypeName, assetObj.AssetBundlePath, assetObj.AssetName);
                        Type assetType = Assembly.GetType(AorTxt.Format("UnityEngine.{0}", assetObj.TypeName));
                        
                        if (assetType != null)
                        {
                            assetObj.Asset = UnityEditor.AssetDatabase.LoadAssetAtPath(assetRelativeFullPath, assetType);
                        }
                        else
                        {
                            assetObj.Asset = UnityEditor.AssetDatabase.LoadAssetAtPath(assetRelativeFullPath, typeof(UnityEngine.Object));
                        }

                        if (assetObj.Asset == null)
                        {
                            m_LoadingList.Remove(assetObj.AssetPath);
                            Log.Error("AssetLoadManager assetObj.Asset Null : {0}", assetObj.AssetPath);
                            break;
                        }

                        assetObj.InstanceID = assetObj.Asset.GetInstanceID();
                        if (!m_AssetInstanceIDList.ContainsKey(assetObj.InstanceID))
                        {
                            m_AssetInstanceIDList.Add(assetObj.InstanceID, assetObj);
                        }
                        else
                        {
                            Log.Error("AssetLoadManager.UpdateLoading assetObj.InstanceID '{0}' 已存在。Name: {1} 请检查AB配置", assetObj.InstanceID, assetObj.AssetName);
                        }

                        assetObj.Request = null;
                        m_TempLoadeds.Add(assetObj);
                    }
#endif
                }
                else
                {
                    if (assetObj.Request != null && assetObj.Request.isDone)
                    {
                        if (assetObj.IsScene)
                        {
                            m_Scenes.Add(assetObj);
                        }
                        else
                        {
                            if (assetObj.Request is AssetBundleRequest)
                            {
                                assetObj.Asset = (assetObj.Request as AssetBundleRequest).asset;
                            }

                            if (assetObj.Asset == null)
                            {
                                m_LoadingList.Remove(assetObj.AssetPath);
                                Log.Error("AssetLoadManager assetObj.Asset Null : {0}", assetObj.AssetPath);
                                break;
                            }

                            assetObj.InstanceID = assetObj.Asset.GetInstanceID();
                            if (!m_AssetInstanceIDList.ContainsKey(assetObj.InstanceID))
                            {
                                m_AssetInstanceIDList.Add(assetObj.InstanceID, assetObj);
                            }
                            else
                            {
                                Log.Error("AssetLoadManager.LoadSync assetObj.InstanceID '{0}' 已存在。", assetObj.InstanceID);
                            }
                        }

                        assetObj.Request = null;
                        m_TempLoadeds.Add(assetObj);
                    }
                }
            }

            // 先移动列表，再统一回调，防止嵌套操作异常
            foreach (var assetObj in m_TempLoadeds)
            {
                m_LoadingList.Remove(assetObj.AssetPath);
                m_LoadedList.Add(assetObj.AssetPath, assetObj);

                m_LoadingIntervalCount++;
                assetObj.LockCallbackCount = assetObj.AssetLoadOverCallbackList.Count;
            }

            // 统一派发加载完成回调
            foreach (var assetObj in m_TempLoadeds)
            {
                DoAssetCallback(assetObj);
            }
        }
        #endregion

        #region 卸载列表帧更新
        /// <summary>
        /// 卸载列表帧更新
        /// 处理延迟卸载、引用计数恢复、弱引用资源释放
        /// </summary>
        private void UpdateUnload()
        {
            if (m_UnloadList.Count == 0) 
                return;

            m_TempLoadeds.Clear();
            foreach (var assetObj in m_UnloadList.Values)
            {
                // 弱引用 + 引用计数为0 + 无回调 → 可以卸载
                if (assetObj.IsWeak && assetObj.RefCount == 0 && assetObj.AssetLoadOverCallbackList.Count == 0)
                {
                    if (assetObj.UnloadTickNum < 0)
                    {
                        m_LoadedList.Remove(assetObj.AssetPath);
                        DoUnload(assetObj);
                        m_TempLoadeds.Add(assetObj);
                    }
                    else
                    {
                        assetObj.UnloadTickNum--;
                    }
                }

                // 引用计数恢复 或 强引用 → 取消卸载
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
        #endregion

        #region 根据场景对象查找资源包装对象
        /// <summary>
        /// 根据场景对象，查找对应的资源包装对象
        /// </summary>
        /// <param name="scene">场景实例</param>
        /// <returns>匹配的AssetObject</returns>
        private AssetObject GetSceneAssetObjectByScene(UnityEngine.SceneManagement.Scene scene)
        {
            AssetObject result = m_Scenes.Find(assetObject => assetObject.AssetName == scene.name);
            return result;
        }
        #endregion
    }
}