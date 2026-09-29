/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  AssetBundleLoadManager.Methods.cs
 * author:    云毅
 * created:   2026
 * descrip:   AssetBundle 加载管理器 - 内部实现部分
 *            同步/异步加载、依赖管理、异步卸载、WebGL专用加载、
 *            并发控制、生命周期管理
 ***************************************************************/
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Honor.Runtime
{
    //=========================================================================
    // AssetBundle 加载管理器 - 内部实现
    // 核心加载/卸载逻辑、状态调度、WebGL支持、依赖处理
    //=========================================================================
    /// <summary>
    /// AssetBundle 加载管理器（内部实现分部类）
    /// 包含同步/异步加载、卸载、并发控制、WebGL平台加载、Update驱动逻辑
    /// </summary>
    public sealed partial class AssetBundleLoadManager
    {
        #region 内部状态查询
        /// <summary>
        /// 在已加载/加载中/准备三张表中查找指定路径的 AB 包装对象
        /// </summary>
        /// <param name="formatPath">AB 格式化路径</param>
        /// <param name="tracked">命中的 AB 包装对象（未命中为 null）</param>
        /// <returns>命中返回 true</returns>
        private bool TryFindTrackedAB(string formatPath, out AssetBundleObject tracked)
        {
            if (m_LoadedAssetBundleList.TryGetValue(formatPath, out tracked))
            {
                return true;
            }

            if (m_LoadingAssetBundleList.TryGetValue(formatPath, out tracked))
            {
                return true;
            }

            if (_mReadyAssetBundleList.TryGetValue(formatPath, out tracked))
            {
                return true;
            }

            tracked = null;
            return false;
        }
        #endregion

        #region 内部同步加载 AssetBundle
        /// <summary>
        /// 内部同步加载 AssetBundle
        /// 自动处理已加载、加载中、准备中状态，递归加载依赖包
        /// </summary>
        /// <param name="abFormatPath">AB 格式化路径</param>
        /// <returns>AB 包装对象</returns>
        private AssetBundleObject InternalLoadAssetBundleSync(string abFormatPath)
        {
            // 如果处在【加载完成列表】中则直接返回（自身与其递归到的所有依赖AB项引用计数全部+1）
            if (m_LoadedAssetBundleList.TryGetValue(abFormatPath, out AssetBundleObject loadedEntry))
            {
                AddSelfAndDependsSyncRef(loadedEntry);
                return loadedEntry;
            }

            // 如果处在【加载中列表（异步方式）】中则立即将异步改成同步加载得到结果
            if (m_LoadingAssetBundleList.TryGetValue(abFormatPath, out AssetBundleObject loadingEntry))
            {
                AddSelfAndDependsSyncRef(loadingEntry);
                // 强制加载完成并回调
                NormalOrForceLoadOverAndCallBack(loadingEntry);
                return loadingEntry;
            }

            // 如果处在【准备加载列表】中则立即进行同步加载得到结果
            if (_mReadyAssetBundleList.TryGetValue(abFormatPath, out AssetBundleObject readyEntry))
            {
                AddSelfAndDependsSyncRef(readyEntry);
                LoadReadyAssetBundleSync(readyEntry, abFormatPath);
                // 强制加载完成并回调
                NormalOrForceLoadOverAndCallBack(readyEntry);
                return readyEntry;
            }

            // 如果三种列表中都不存在则直接创建一个新的同步加载
            return LoadBrandNewAssetBundleSync(abFormatPath);
        }

        /// <summary>
        /// 同步方式：自身引用计数+1，并递归同步加载所有依赖AB
        /// </summary>
        /// <param name="abObject">AB 包装对象</param>
        private void AddSelfAndDependsSyncRef(AssetBundleObject abObject)
        {
            abObject.RefCount++;

            foreach (AssetBundleObject depObj in abObject.Depends)
            {
                InternalLoadAssetBundleSync(depObj.FormatPath);
            }
        }

        /// <summary>
        /// 同步加载准备列表中的AB并迁移到已加载列表
        /// </summary>
        /// <param name="abObject">AB 包装对象</param>
        /// <param name="abFormatPath">AB 格式化路径</param>
        private void LoadReadyAssetBundleSync(AssetBundleObject abObject, string abFormatPath)
        {
            GetABLoadPathOnDisk(abFormatPath, out string diskPath, out OriginType origin);

            abObject.AssetBundles = AssetBundle.LoadFromFile(diskPath);
            abObject.Origin = origin;

            _mReadyAssetBundleList.Remove(abObject.FormatPath);
            m_LoadedAssetBundleList.Add(abObject.FormatPath, abObject);
        }

        /// <summary>
        /// 新建一个同步AB加载：从磁盘加载并递归同步加载依赖项
        /// </summary>
        /// <param name="abFormatPath">AB 格式化路径</param>
        /// <returns>新建并加载完成的 AB 包装对象</returns>
        private AssetBundleObject LoadBrandNewAssetBundleSync(string abFormatPath)
        {
            AssetBundleObject abObject = new AssetBundleObject();
            abObject.FormatPath = abFormatPath;
            abObject.RefCount = 1;

            GetABLoadPathOnDisk(abFormatPath, out string diskPath, out OriginType origin);
            abObject.AssetBundles = AssetBundle.LoadFromFile(diskPath);
            abObject.Origin = origin;

            // 用同步的方式加载依赖项
            string[] dependsData = null;
            if (m_DependsDataList.ContainsKey(abFormatPath))
            {
                dependsData = m_DependsDataList[abFormatPath];
            }

            if (dependsData != null && dependsData.Length > 0)
            {
                // 同步加载后将使加载中的依赖资源数量清零
                abObject.DependLoadingCount = 0;

                // 对依赖资源进行同步加载并记录依赖资源
                foreach (string depFormatName in dependsData)
                {
                    AssetBundleObject depObj = InternalLoadAssetBundleSync(depFormatName);
                    abObject.Depends.Add(depObj);
                }
            }

            // 将新创建的同步加载得到的资源加入到【已加载完成列表】中
            m_LoadedAssetBundleList.Add(abObject.FormatPath, abObject);

            return abObject;
        }
        #endregion

        #region 内部异步加载 AssetBundle
        /// <summary>
        /// 内部异步加载 AssetBundle
        /// 自动处理依赖加载、并发限制、多状态管理
        /// </summary>
        /// <param name="abFormatPath">AB 格式化路径</param>
        /// <param name="abLoadOverCallback">加载完成回调</param>
        /// <returns>AB 包装对象</returns>
        private AssetBundleObject InternalLoadAssetBundleAsync(string abFormatPath, AssetBundleLoadOverCallBack abLoadOverCallback)
        {
            // 如果处在【加载完成列表】中则直接返回
            if (m_LoadedAssetBundleList.TryGetValue(abFormatPath, out AssetBundleObject loadedEntry))
            {
                AddSelfAndDependsRef(loadedEntry);
                abLoadOverCallback(loadedEntry, loadedEntry.AssetBundles);
                return loadedEntry;
            }

            // 如果处在【加载中列表】则合并回调
            if (m_LoadingAssetBundleList.TryGetValue(abFormatPath, out AssetBundleObject loadingEntry))
            {
                AddSelfAndDependsRef(loadingEntry);
                loadingEntry.AssetBundleLoadOverCallbacksList.Add(abLoadOverCallback);
                return loadingEntry;
            }

            // 如果处在【准备加载列表】则合并回调
            if (_mReadyAssetBundleList.TryGetValue(abFormatPath, out AssetBundleObject readyEntry))
            {
                AddSelfAndDependsRef(readyEntry);
                readyEntry.AssetBundleLoadOverCallbacksList.Add(abLoadOverCallback);
                return readyEntry;
            }

            // 如果三种列表中都不存在则直接创建一个新的异步加载
            AssetBundleObject abObject = new AssetBundleObject();
            abObject.FormatPath = abFormatPath;

            abObject.RefCount = 1;
            abObject.AssetBundleLoadOverCallbacksList.Add(abLoadOverCallback);

            SetupAsyncDepends(abObject, abFormatPath);
            EnqueueAsyncLoad(abObject, abFormatPath);

            return abObject;
        }

        /// <summary>
        /// 初始化新异步加载对象的依赖项：记录依赖数并递归异步加载依赖AB
        /// </summary>
        /// <param name="abObject">新建的AB包装对象</param>
        /// <param name="abFormatPath">AB 格式化路径</param>
        private void SetupAsyncDepends(AssetBundleObject abObject, string abFormatPath)
        {
            // 加载依赖项
            string[] dependsData = null;
            if (m_DependsDataList.ContainsKey(abFormatPath))
            {
                dependsData = m_DependsDataList[abFormatPath];
            }

            if (dependsData == null || dependsData.Length == 0)
            {
                return;
            }

            // 记录依赖数量，等待异步加载完成
            abObject.DependLoadingCount = dependsData.Length;
            foreach (string depFormatName in dependsData)
            {
                AssetBundleObject depObj = InternalLoadAssetBundleAsync(depFormatName, (AssetBundleObject abEntry, AssetBundle _) =>
                {
                    if (abObject.DependLoadingCount <= 0)
                    {
                        Log.Error("加载依赖AB错误，ab名称:{0}", abFormatPath);
                        return;
                    }

                    // 完成1个依赖资源的加载后，数量-1
                    abObject.DependLoadingCount--;

                    // 当所有依赖全部加载完毕后，触发正常加载完成的逻辑并触发回调
                    if (abObject.DependLoadingCount == 0
                        && abObject.Request != null && abObject.Request.isDone)
                    {
                        NormalOrForceLoadOverAndCallBack(abObject);
                    }
                });
                // 将依赖资源记录到当前新创建的ab资源中
                abObject.Depends.Add(depObj);
            }
        }

        /// <summary>
        /// 根据并发上限决定立即加载还是放入准备队列
        /// </summary>
        /// <param name="abObject">新建的AB包装对象</param>
        /// <param name="abFormatPath">AB 格式化路径</param>
        private void EnqueueAsyncLoad(AssetBundleObject abObject, string abFormatPath)
        {
            // 正在加载的数量不能超过上限
            if (m_LoadingAssetBundleList.Count < MAX_LOADING_COUNT)
            {
                // 立即开始异步加载，并加入到【加载中列表】中
                DoLoadAsync(abObject);
                m_LoadingAssetBundleList.Add(abFormatPath, abObject);
            }
            else
            {
                // 如果超过了上限，则暂时放入准备列表中
                _mReadyAssetBundleList.Add(abFormatPath, abObject);
            }
        }
        #endregion

        #region 内部异步卸载 AssetBundle
        /// <summary>
        /// 内部异步卸载 AssetBundle
        /// 递归减少自身与依赖引用计数，计数为0时加入卸载队列
        /// </summary>
        /// <param name="abFormatPath">AB 格式化路径</param>
        private void InternalUnloadAssetBundleAsync(string abFormatPath)
        {
            // 获得可能存在于三种列表中的AB封装资源对象
            if (!TryFindTrackedAB(abFormatPath, out AssetBundleObject abObject) || abObject == null)
            {
                Log.Error("卸载AB包错误，ab名称:{0}", abFormatPath);
                return;
            }

            if (abObject.RefCount == 0)
            {
                Log.Error("卸载AB包时引用计数错误！ab名称:{0}", abFormatPath);
                return;
            }

            // 自身与其递归得到所有依赖AB资源的引用计数全部-1
            abObject.RefCount--;
            foreach (AssetBundleObject depObj in abObject.Depends)
            {
                InternalUnloadAssetBundleAsync(depObj.FormatPath);
            }

            // 引用计数-1后如果为0则加入到【卸载列表】中
            if (abObject.RefCount == 0 && !m_UnloadAssetBundleList.ContainsKey(abObject.FormatPath))
            {
                m_UnloadAssetBundleList.Add(abObject.FormatPath, abObject);
            }
        }
        #endregion

        #region WebGL 平台专用：从 Web 加载 AssetBundle
        /// <summary>
        /// WebGL 平台专用：从 Web 加载 AssetBundle
        /// 阻塞主线程等待加载完成
        /// </summary>
        /// <param name="abFormatPath">AB 格式化路径</param>
        /// <returns>加载完成的 AB</returns>
        private AssetBundle LoadAssetBundleFromWebGL(string abFormatPath)
        {
            // 主线程调用多线程
            InternalEvaluateWebRequestFromWebGL(abFormatPath);

            // 在主线程中阻塞等待
            while (m_WebGLRequest == null || !m_WebGLRequest.isDone)
            {
            }

            AssetBundle loadedAB = null;
            if (m_WebGLRequest.isNetworkError || m_WebGLRequest.isHttpError)
            {
                Log.Warning("下载文件 {0} 时出错！", abFormatPath);
            }
            else
            {
                loadedAB = DownloadHandlerAssetBundle.GetContent(m_WebGLRequest);
            }

            m_WebGLRequest.Dispose();
            m_WebGLRequest = null;
            return loadedAB;
        }
        #endregion

        #region WebGL 异步请求赋值（异步线程）
        /// <summary>
        /// WebGL 异步请求赋值
        /// 运行在异步线程
        /// </summary>
        /// <param name="abFormatPath">AB格式化路径</param>
        private async void InternalEvaluateWebRequestFromWebGL(string abFormatPath)
        {
            m_WebGLRequest = await InternalGetWebRequestFromWebGL(abFormatPath);
        }
        #endregion

        #region WebGL 创建 WebRequest 任务
        /// <summary>
        /// WebGL 创建 WebRequest 异步任务
        /// </summary>
        /// <param name="abFormatPath">AB格式化路径</param>
        /// <returns>UnityWebRequest异步任务</returns>
        private Task<UnityWebRequest> InternalGetWebRequestFromWebGL(string abFormatPath)
        {
            TaskCompletionSource<UnityWebRequest> pendingRequest = new TaskCompletionSource<UnityWebRequest>();
            string url = GamePathUtils.AB.Streaming.GetFileFullPath(abFormatPath);

            return pendingRequest.Task;
        }
        #endregion

        #region AB 自身 + 所有依赖 引用计数 +1（递归）
        /// <summary>
        /// AB 自身与所有依赖 引用计数 +1
        /// 递归处理所有依赖包
        /// </summary>
        /// <param name="abObject">AB 包装对象</param>
        private void AddSelfAndDependsRef(AssetBundleObject abObject)
        {
            abObject.RefCount++;

            if (abObject.Depends.Count == 0)
            {
                return;
            }

            foreach (AssetBundleObject depObj in abObject.Depends)
            {
                AddSelfAndDependsRef(depObj);
            }
        }
        #endregion

        #region 执行异步加载 AB
        /// <summary>
        /// 执行异步加载 AB
        /// 创建异步加载请求
        /// </summary>
        /// <param name="abObject">AB 包装对象</param>
        private void DoLoadAsync(AssetBundleObject abObject)
        {
            GetABLoadPathOnDisk(abObject.FormatPath, out string diskPath, out OriginType origin);

            abObject.Request = AssetBundle.LoadFromFileAsync(diskPath);
            if (abObject.Request == null)
            {
                Log.Error("加载AB包时的路径错误！ab名称:{0}", abObject.FormatPath);
            }

            abObject.Origin = origin;
        }
        #endregion

        #region 执行卸载 AB（包含内存卸载）
        /// <summary>
        /// 执行卸载 AB
        /// 卸载资源并释放内存
        /// </summary>
        /// <param name="abObject">AB 包装对象</param>
        private void DoUnload(AssetBundleObject abObject)
        {
            if (abObject.AssetBundles == null)
            {
                Log.Error("卸载AB包时错误！ab名称:{0}", abObject.FormatPath);
                return;
            }

            abObject.AssetBundles.Unload(true);
            abObject.AssetBundles = null;
        }
        #endregion

        #region 正常/强制完成加载，并触发所有回调
        /// <summary>
        /// 正常/强制完成加载并触发回调
        /// 支持异步转同步加载
        /// </summary>
        /// <param name="abObject">AB 包装对象</param>
        private void NormalOrForceLoadOverAndCallBack(AssetBundleObject abObject)
        {
            // 从异步中提取ab
            if (abObject.Request != null)
            {
                // 如果没加载完，通过API立刻拿到资源（变异步为同步）
                abObject.AssetBundles = abObject.Request.assetBundle;
                abObject.Request = null;
                m_LoadingAssetBundleList.Remove(abObject.FormatPath);
                m_LoadedAssetBundleList.Add(abObject.FormatPath, abObject);
            }

            if (abObject.AssetBundles == null)
            {
                GetABLoadPathOnDisk(abObject.FormatPath, out string diskPath, out OriginType origin);
                abObject.AssetBundles = AssetBundle.LoadFromFile(diskPath);
                abObject.Origin = origin;
            }

            // 运行回调
            foreach (AssetBundleLoadOverCallBack invocation in abObject.AssetBundleLoadOverCallbacksList)
            {
                invocation(abObject, abObject.AssetBundles);
            }

            abObject.AssetBundleLoadOverCallbacksList.Clear();
        }
        #endregion

        #region 获取 AB 在磁盘中的实际路径
        /// <summary>
        /// 获取 AB 在磁盘中的实际路径
        /// 优先加载持久化目录，其次加载流资源目录
        /// </summary>
        /// <param name="formatPath">AB 格式化路径</param>
        /// <param name="diskPath">输出实际加载路径</param>
        /// <param name="origin">输出资源来源类型</param>
        private void GetABLoadPathOnDisk(string formatPath, out string diskPath, out OriginType origin)
        {
            // 优先检查读写区域的资源是否存在，如果存在则加载读写区域的资源，否则加载只读区域
            string filePersistentPath = GamePathUtils.AB.Persistent.GetFileFullPath(formatPath);
            if (File.Exists(filePersistentPath))
            {
                diskPath = filePersistentPath;
                origin = OriginType.Persistent;
            }
            else
            {
                diskPath = GamePathUtils.AB.Streaming.GetFileFullPath(formatPath);
                origin = OriginType.Streaming;
            }
        }
        #endregion

        #region Update 驱动：管理加载中列表
        /// <summary>
        /// Update 驱动：管理加载中列表
        /// 检测加载完成的AB并执行回调
        /// </summary>
        private void UpdateLoadingList()
        {
            if (m_LoadingAssetBundleList.Count == 0)
            {
                return;
            }

            // 检测加载完的AB
            m_TempLoadeds.Clear();
            foreach (AssetBundleObject abEntry in m_LoadingAssetBundleList.Values)
            {
                if (abEntry.DependLoadingCount == 0 && abEntry.Request != null && abEntry.Request.isDone)
                {
                    m_TempLoadeds.Add(abEntry);
                }
            }

            // 回调中有可能对m_LoadingABList进行操作，提取后回调
            foreach (AssetBundleObject abEntry in m_TempLoadeds)
            {
                NormalOrForceLoadOverAndCallBack(abEntry);
            }
        }
        #endregion

        #region Update 驱动：管理卸载列表
        /// <summary>
        /// Update 驱动：管理卸载列表
        /// 执行引用计数为0的AB卸载
        /// </summary>
        private void UpdateUnLoadList()
        {
            if (m_UnloadAssetBundleList.Count == 0)
            {
                return;
            }

            m_TempLoadeds.Clear();
            foreach (AssetBundleObject abEntry in m_UnloadAssetBundleList.Values)
            {
                if (abEntry.RefCount == 0 && abEntry.AssetBundles != null)
                {
                    // 引用计数为0并且已经加载完，没加载完等加载完销毁
                    DoUnload(abEntry);

                    m_LoadedAssetBundleList.Remove(abEntry.FormatPath);
                    m_TempLoadeds.Add(abEntry);
                }

                if (abEntry.RefCount > 0)
                {
                    // 引用计数加回来（销毁又瞬间重新加载，不销毁，从销毁列表移除）
                    m_TempLoadeds.Add(abEntry);
                }
            }

            foreach (AssetBundleObject abEntry in m_TempLoadeds)
            {
                m_UnloadAssetBundleList.Remove(abEntry.FormatPath);
            }
        }
        #endregion

        #region Update 驱动：管理准备列表
        /// <summary>
        /// Update 驱动：管理准备列表
        /// 控制并发加载数量，依次启动准备队列中的加载
        /// </summary>
        private void UpdateReadyList()
        {
            if (_mReadyAssetBundleList.Count == 0 || m_LoadingAssetBundleList.Count >= MAX_LOADING_COUNT)
            {
                return;
            }

            m_TempLoadeds.Clear();
            foreach (AssetBundleObject abEntry in _mReadyAssetBundleList.Values)
            {
                DoLoadAsync(abEntry);

                m_TempLoadeds.Add(abEntry);
                m_LoadingAssetBundleList.Add(abEntry.FormatPath, abEntry);

                if (m_LoadingAssetBundleList.Count >= MAX_LOADING_COUNT)
                {
                    break;
                }
            }

            foreach (AssetBundleObject abEntry in m_TempLoadeds)
            {
                _mReadyAssetBundleList.Remove(abEntry.FormatPath);
            }
        }
        #endregion
    }
}
