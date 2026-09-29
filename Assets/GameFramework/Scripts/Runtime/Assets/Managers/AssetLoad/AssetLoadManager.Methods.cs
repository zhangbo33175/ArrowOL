/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  AssetLoadManager.Methods.cs
 * author:    云毅
 * created:   2026
 * descrip:   资源加载管理器 - 内部工具方法 & 帧更新驱动实现
 *            回调管理、卸载逻辑、预加载、Update驱动流程
 ***************************************************************/
using System;
using System.Collections.Generic;
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
        /// 从一组资源对象的回调列表中移除指定委托
        /// </summary>
        /// <param name="values">资源对象集合</param>
        /// <param name="target">待移除的回调委托</param>
        private static void DetachCallback(IEnumerable<AssetObject> values, AssetLoadOverCallback target)
        {
            foreach (AssetObject assetObj in values)
            {
                List<AssetLoadOverCallback> callbacks = assetObj.AssetLoadOverCallbackList;
                if (callbacks.Count == 0)
                {
                    continue;
                }

                int hit = callbacks.IndexOf(target);
                if (hit >= 0)
                {
                    callbacks.RemoveAt(hit);
                }
            }
        }

        /// <summary>
        /// 根据回调委托，全局解绑加载完成回调
        /// 遍历加载中/已加载列表，移除指定回调
        /// </summary>
        /// <param name="overCallback">要移除的加载回调</param>
        private void RemoveCallBackByCallBack(AssetLoadOverCallback overCallback)
        {
            DetachCallback(m_LoadingList.Values, overCallback);
            DetachCallback(m_LoadedList.Values, overCallback);
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
            List<AssetLoadOverCallback> callbacks = assetObj.AssetLoadOverCallbackList;
            if (callbacks.Count == 0)
            {
                return;
            }

            // 先提取count，保证回调中有加载需求不影响本次执行
            int callbackCount = assetObj.LockCallbackCount;
            for (int cursor = 0; cursor < callbackCount; cursor++)
            {
                AssetLoadOverCallback invocation = callbacks[cursor];
                if (invocation == null)
                {
                    continue;
                }

                // 回调期间引用计数 +1，保证资源不被释放
                assetObj.RefCount++;
                try
                {
                    invocation(assetObj, assetObj.Asset);
                }
                catch (GameException exception)
                {
                    Log.Error(exception);
                }
            }

            callbacks.RemoveRange(0, callbackCount);
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
                m_AssetBundleLoadManager.Unload(assetObj.AssetBundlePath);
            }

            assetObj.Asset = null;

            if (assetObj.IsScene)
            {
                m_Scenes.Remove(assetObj);
            }
            else if (m_AssetInstanceIDList.ContainsKey(assetObj.InstanceID))
            {
                m_AssetInstanceIDList.Remove(assetObj.InstanceID);
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
        /// 若资源已被加载中/已加载列表跟踪，则更新其弱引用标记
        /// </summary>
        /// <param name="assetPath">资源唯一路径</param>
        /// <param name="isWeak">是否弱引用</param>
        private void ApplyWeakFlag(string assetPath, bool isWeak)
        {
            if (m_LoadingList.TryGetValue(assetPath, out AssetObject loadingEntry))
            {
                loadingEntry.IsWeak = isWeak;
            }
            else if (m_LoadedList.TryGetValue(assetPath, out AssetObject loadedEntry))
            {
                loadedEntry.IsWeak = isWeak;
            }
        }

        /// <summary>
        /// 预加载队列帧更新
        /// 当加载队列为空时，从预加载队列取一个进行加载
        /// </summary>
        private void UpdatePreload()
        {
            if (m_LoadingList.Count > 0 || m_PreloadedAsyncList.Count == 0)
            {
                return;
            }

            PreloadAssetObject preloadEntry = null;
            while (m_PreloadedAsyncList.Count > 0 && preloadEntry == null)
            {
                preloadEntry = m_PreloadedAsyncList.Dequeue();

                if (m_LoadingList.ContainsKey(preloadEntry.AssetPath))
                {
                    ApplyWeakFlag(preloadEntry.AssetPath, preloadEntry.IsWeak);
                }
                else if (m_LoadedList.ContainsKey(preloadEntry.AssetPath))
                {
                    ApplyWeakFlag(preloadEntry.AssetPath, preloadEntry.IsWeak);
                    preloadEntry = null;
                }
                else
                {
                    LoadAsync(preloadEntry.TypeName, preloadEntry.AssetBundlePath, preloadEntry.AssetName, preloadEntry.AssetLoadOverCallback);
                    ApplyWeakFlag(preloadEntry.AssetPath, preloadEntry.IsWeak);
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
            {
                return;
            }

            int agentCount = m_LoadedAsyncTmpAgentList.Count;

            // 锁定回调数量，防止异步过程中列表变化
            for (int cursor = 0; cursor < agentCount; cursor++)
            {
                m_LoadedAsyncTmpAgentList[cursor].LockCallbackCount = m_LoadedAsyncTmpAgentList[cursor].AssetLoadOverCallbackList.Count;
            }

            for (int cursor = 0; cursor < agentCount; cursor++)
            {
                DoAssetCallback(m_LoadedAsyncTmpAgentList[cursor]);
            }

            m_LoadedAsyncTmpAgentList.RemoveRange(0, agentCount);

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
        /// 注册资源实例 ID 到全局映射表（重复时输出错误日志）
        /// </summary>
        /// <param name="assetObj">资源包装对象</param>
        /// <param name="duplicateHint">重复时附加的日志提示</param>
        private void TrackInstanceID(AssetObject assetObj, string duplicateHint)
        {
            assetObj.InstanceID = assetObj.Asset.GetInstanceID();
            if (!m_AssetInstanceIDList.ContainsKey(assetObj.InstanceID))
            {
                m_AssetInstanceIDList.Add(assetObj.InstanceID, assetObj);
                return;
            }

            Log.Error(duplicateHint, assetObj.InstanceID, assetObj.AssetName);
        }

        /// <summary>
        /// 加载中列表帧更新
        /// 检测异步加载完成的资源，移入已加载列表并触发回调
        /// </summary>
        private void UpdateLoading()
        {
            if (m_LoadingList.Count == 0)
            {
                return;
            }

            m_TempLoadeds.Clear();
            foreach (AssetObject assetObj in m_LoadingList.Values)
            {
                bool shouldStop = m_EditorResourceMode
                    ? CollectEditorLoadedAsset(assetObj)
                    : CollectBundleLoadedAsset(assetObj);
                if (shouldStop)
                {
                    break;
                }
            }

            // 先移动列表，再统一回调，防止嵌套操作异常
            foreach (AssetObject assetObj in m_TempLoadeds)
            {
                m_LoadingList.Remove(assetObj.AssetPath);
                m_LoadedList.Add(assetObj.AssetPath, assetObj);

                m_LoadingIntervalCount++;
                assetObj.LockCallbackCount = assetObj.AssetLoadOverCallbackList.Count;
            }

            // 统一派发加载完成回调
            foreach (AssetObject assetObj in m_TempLoadeds)
            {
                DoAssetCallback(assetObj);
            }
        }

        /// <summary>
        /// 编辑器模式下检测加载中资源是否完成，完成则收集到临时列表
        /// </summary>
        /// <param name="assetObj">加载中的资源对象</param>
        /// <returns>是否资源为空需要中断遍历</returns>
        private bool CollectEditorLoadedAsset(AssetObject assetObj)
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
                string assetFullPath = GetAssetRelativeFullPath(assetObj.TypeName, assetObj.AssetBundlePath, assetObj.AssetName);
                Type resolvedType = Assembly.GetType(AorTxt.Format("UnityEngine.{0}", assetObj.TypeName));

                assetObj.Asset = resolvedType != null
                    ? UnityEditor.AssetDatabase.LoadAssetAtPath(assetFullPath, resolvedType)
                    : UnityEditor.AssetDatabase.LoadAssetAtPath(assetFullPath, typeof(UnityEngine.Object));

                if (assetObj.Asset == null)
                {
                    m_LoadingList.Remove(assetObj.AssetPath);
                    Log.Error("AssetLoadManager assetObj.Asset Null : {0}", assetObj.AssetPath);
                    return true;
                }

                TrackInstanceID(assetObj, "AssetLoadManager.UpdateLoading assetObj.InstanceID '{0}' 已存在。Name: {1} 请检查AB配置");

                assetObj.Request = null;
                m_TempLoadeds.Add(assetObj);
            }
#endif
            return false;
        }

        /// <summary>
        /// AB模式下检测加载中资源是否完成，完成则提取资源并收集到临时列表
        /// </summary>
        /// <param name="assetObj">加载中的资源对象</param>
        /// <returns>是否资源为空需要中断遍历</returns>
        private bool CollectBundleLoadedAsset(AssetObject assetObj)
        {
            if (assetObj.Request == null || !assetObj.Request.isDone)
            {
                return false;
            }

            if (assetObj.IsScene)
            {
                m_Scenes.Add(assetObj);
            }
            else
            {
                if (assetObj.Request is AssetBundleRequest bundleRequest)
                {
                    assetObj.Asset = bundleRequest.asset;
                }

                if (assetObj.Asset == null)
                {
                    m_LoadingList.Remove(assetObj.AssetPath);
                    Log.Error("AssetLoadManager assetObj.Asset Null : {0}", assetObj.AssetPath);
                    return true;
                }

                TrackInstanceID(assetObj, "AssetLoadManager.LoadSync assetObj.InstanceID '{0}' 已存在。");
            }

            assetObj.Request = null;
            m_TempLoadeds.Add(assetObj);
            return false;
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
            {
                return;
            }

            m_TempLoadeds.Clear();
            foreach (AssetObject assetObj in m_UnloadList.Values)
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

            foreach (AssetObject assetObj in m_TempLoadeds)
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
            return m_Scenes.Find(assetObject => assetObject.AssetName == scene.name);
        }
        #endregion
    }
}
