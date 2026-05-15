using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Honor.Runtime
{
    public sealed partial class AssetBundleLoadManager
    {
        #region 内部同步加载 AssetBundle
        /// <summary>
        /// 内部同步加载 AssetBundle
        /// 自动处理已加载、加载中、准备中状态，递归加载依赖包
        /// </summary>
        /// <param name="abFormatPath">AB 格式化路径</param>
        /// <returns>AB 包装对象</returns>
        private AssetBundleObject InternalLoadAssetBundleSync(string abFormatPath)
        {
            AssetBundleObject assetBundleObj = null;
            // 如果处在【加载完成列表】中则直接返回（自身与其递归到的所有依赖AB项引用计数全部+1）
            if (m_LoadedAssetBundleList.ContainsKey(abFormatPath))
            {
                assetBundleObj = m_LoadedAssetBundleList[abFormatPath];
                assetBundleObj.RefCount++;

                foreach (var dpObj in assetBundleObj.Depends)
                {
                    InternalLoadAssetBundleSync(dpObj.FormatPath);
                }

                return assetBundleObj;
            }
            // 如果处在【加载中列表（异步方式）】中则立即将异步改成同步加载得到结果
            else if (m_LoadingAssetBundleList.ContainsKey(abFormatPath))
            {
                assetBundleObj = m_LoadingAssetBundleList[abFormatPath];
                assetBundleObj.RefCount++;

                foreach (var dpObj in assetBundleObj.Depends)
                {
                    InternalLoadAssetBundleSync(dpObj.FormatPath);
                }

                // 强制加载完成并回调
                NormalOrForceLoadOverAndCallBack(assetBundleObj);

                return assetBundleObj;
            }
            // 如果处在【准备加载列表】中则立即进行同步加载得到结果
            else if (_mReadyAssetBundleList.ContainsKey(abFormatPath))
            {
                assetBundleObj = _mReadyAssetBundleList[abFormatPath];
                assetBundleObj.RefCount++;

                foreach (var dpObj in assetBundleObj.Depends)
                {
                    InternalLoadAssetBundleSync(dpObj.FormatPath);
                }

                string path1;
                OriginType origin1;
                GetABLoadPathOnDisk(abFormatPath, out path1, out origin1);

                assetBundleObj.AssetBundles = AssetBundle.LoadFromFile(path1);
                assetBundleObj.Origin = origin1;

                _mReadyAssetBundleList.Remove(assetBundleObj.FormatPath);
                m_LoadedAssetBundleList.Add(assetBundleObj.FormatPath, assetBundleObj);

                // 强制加载完成并回调
                NormalOrForceLoadOverAndCallBack(assetBundleObj);

                return assetBundleObj;
            }

            // 如果三种列表中都不存在则直接创建一个新的同步加载
            assetBundleObj = new AssetBundleObject();
            assetBundleObj.FormatPath = abFormatPath;
            assetBundleObj.RefCount = 1;

            string path;
            OriginType origin;
            GetABLoadPathOnDisk(abFormatPath, out path, out origin);
            assetBundleObj.AssetBundles = AssetBundle.LoadFromFile(path);
            assetBundleObj.Origin = origin;

            // 用同步的方式加载依赖项
            string[] dependsData = null;
            if (m_DependsDataList.ContainsKey(abFormatPath))
            {
                dependsData = m_DependsDataList[abFormatPath];
            }

            if (dependsData != null && dependsData.Length > 0)
            {
                // 同步加载后将使加载中的依赖资源数量清零
                assetBundleObj.DependLoadingCount = 0;

                // 对依赖资源进行同步加载并记录依赖资源
                foreach (var dpFormatName in dependsData)
                {
                    var dpObj = InternalLoadAssetBundleSync(dpFormatName);
                    assetBundleObj.Depends.Add(dpObj);
                }
            }

            // 将新创建的同步加载得到的资源加入到【已加载完成列表】中
            m_LoadedAssetBundleList.Add(assetBundleObj.FormatPath, assetBundleObj);

            return assetBundleObj;
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
        private AssetBundleObject InternalLoadAssetBundleAsync(string abFormatPath,
            AssetBundleLoadOverCallBack abLoadOverCallback)
        {
            AssetBundleObject assetBundleObj = null;

            // 如果处在【加载完成列表】中则直接返回
            if (m_LoadedAssetBundleList.ContainsKey(abFormatPath))
            {
                assetBundleObj = m_LoadedAssetBundleList[abFormatPath];
                AddSelfAndDependsRef(assetBundleObj);
                abLoadOverCallback(assetBundleObj, assetBundleObj.AssetBundles);
                return assetBundleObj;
            }
            // 如果处在【加载中列表】则合并回调
            else if (m_LoadingAssetBundleList.ContainsKey(abFormatPath))
            {
                assetBundleObj = m_LoadingAssetBundleList[abFormatPath];
                AddSelfAndDependsRef(assetBundleObj);
                assetBundleObj.AssetBundleLoadOverCallbacksList.Add(abLoadOverCallback);
                return assetBundleObj;
            }
            // 如果处在【准备加载列表】则合并回调
            else if (_mReadyAssetBundleList.ContainsKey(abFormatPath))
            {
                assetBundleObj = _mReadyAssetBundleList[abFormatPath];
                AddSelfAndDependsRef(assetBundleObj);
                assetBundleObj.AssetBundleLoadOverCallbacksList.Add(abLoadOverCallback);
                return assetBundleObj;
            }

            // 如果三种列表中都不存在则直接创建一个新的异步加载
            assetBundleObj = new AssetBundleObject();
            assetBundleObj.FormatPath = abFormatPath;

            assetBundleObj.RefCount = 1;
            assetBundleObj.AssetBundleLoadOverCallbacksList.Add(abLoadOverCallback);

            // 加载依赖项
            string[] dependsData = null;
            if (m_DependsDataList.ContainsKey(abFormatPath))
            {
                dependsData = m_DependsDataList[abFormatPath];
            }

            if (dependsData != null && dependsData.Length > 0)
            {
                // 记录依赖数量，等待异步加载完成
                assetBundleObj.DependLoadingCount = dependsData.Length;
                foreach (var dpFormatName in dependsData)
                {
                    var dpObj = InternalLoadAssetBundleAsync(dpFormatName, (AssetBundleObject abObject, AssetBundle _ab) =>
                        {
                            if (assetBundleObj.DependLoadingCount <= 0)
                            {
                                Log.Error("加载依赖AB错误，ab名称:{0}", abFormatPath);
                                return;
                            }

                            // 完成1个依赖资源的加载后，数量-1
                            assetBundleObj.DependLoadingCount--;

                            // 当所有依赖全部加载完毕后，触发正常加载完成的逻辑并触发回调
                            if (assetBundleObj.DependLoadingCount == 0)
                            {
                                if (assetBundleObj.Request != null && assetBundleObj.Request.isDone)
                                {
                                    NormalOrForceLoadOverAndCallBack(assetBundleObj);
                                }
                            }
                        }
                    );
                    // 将依赖资源记录到当前新创建的ab资源中
                    assetBundleObj.Depends.Add(dpObj);
                }
            }

            // 正在加载的数量不能超过上限
            if (m_LoadingAssetBundleList.Count < MAX_LOADING_COUNT)
            {
                // 立即开始异步加载，并加入到【加载中列表】中
                DoLoadAsync(assetBundleObj);
                m_LoadingAssetBundleList.Add(abFormatPath, assetBundleObj);
            }
            else
            {
                // 如果超过了上限，则暂时放入准备列表中
                _mReadyAssetBundleList.Add(abFormatPath, assetBundleObj);
            }

            return assetBundleObj;
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
            AssetBundleObject assetBundleObj = null;

            // 获得可能存在于三种列表中的AB封装资源对象
            if (m_LoadedAssetBundleList.ContainsKey(abFormatPath))
            {
                assetBundleObj = m_LoadedAssetBundleList[abFormatPath];
            }
            else if (m_LoadingAssetBundleList.ContainsKey(abFormatPath))
            {
                assetBundleObj = m_LoadingAssetBundleList[abFormatPath];
            }
            else if (_mReadyAssetBundleList.ContainsKey(abFormatPath))
            {
                assetBundleObj = _mReadyAssetBundleList[abFormatPath];
            }

            if (assetBundleObj == null)
            {
                Log.Error("卸载AB包错误，ab名称:{0}", abFormatPath);
                return;
            }

            if (assetBundleObj.RefCount == 0)
            {
                Log.Error("卸载AB包时引用计数错误！ab名称:{0}", abFormatPath);
                return;
            }

            // 自身与其递归得到所有依赖AB资源的引用计数全部-1
            assetBundleObj.RefCount--;
            foreach (var dpObj in assetBundleObj.Depends)
            {
                InternalUnloadAssetBundleAsync(dpObj.FormatPath);
            }

            // 引用计数-1后如果为0则加入到【卸载列表】中
            if (assetBundleObj.RefCount == 0)
            {
                if (!m_UnloadAssetBundleList.ContainsKey(assetBundleObj.FormatPath))
                {
                    m_UnloadAssetBundleList.Add(assetBundleObj.FormatPath, assetBundleObj);
                }
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

            AssetBundle ab = null;
            if (m_WebGLRequest.isNetworkError || m_WebGLRequest.isHttpError)
            {
                Log.Warning("下载文件 {0} 时出错！", abFormatPath);
            }
            else
            {
                ab = DownloadHandlerAssetBundle.GetContent(m_WebGLRequest);
            }

            m_WebGLRequest.Dispose();
            m_WebGLRequest = null;
            return ab;
        }
        #endregion

        #region WebGL 异步请求赋值（异步线程）
        /// <summary>
        /// WebGL 异步请求赋值
        /// 运行在异步线程
        /// </summary>
        private async void InternalEvaluateWebRequestFromWebGL(string abFormatPath)
        {
            m_WebGLRequest = await InternalGetWebRequestFromWebGL(abFormatPath);
        }
        #endregion

        #region WebGL 创建 WebRequest 任务
        /// <summary>
        /// WebGL 创建 WebRequest 异步任务
        /// </summary>
        private Task<UnityWebRequest> InternalGetWebRequestFromWebGL(string abFormatPath)
        {
            TaskCompletionSource<UnityWebRequest> taskSource = new TaskCompletionSource<UnityWebRequest>();
            string url = GamePathUtils.AB.Streaming.GetFileFullPath(abFormatPath);

            return taskSource.Task;
        }
        #endregion

        #region AB 自身 + 所有依赖 引用计数 +1（递归）
        /// <summary>
        /// AB 自身与所有依赖 引用计数 +1
        /// 递归处理所有依赖包
        /// </summary>
        /// <param name="assetBundleObj">AB 包装对象</param>
        private void AddSelfAndDependsRef(AssetBundleObject assetBundleObj)
        {
            assetBundleObj.RefCount++;

            if (assetBundleObj.Depends.Count == 0) return;
            foreach (var dpObj in assetBundleObj.Depends)
            {
                AddSelfAndDependsRef(dpObj);
            }
        }
        #endregion

        #region 执行异步加载 AB
        /// <summary>
        /// 执行异步加载 AB
        /// 创建异步加载请求
        /// </summary>
        /// <param name="assetBundleObj">AB 包装对象</param>
        private void DoLoadAsync(AssetBundleObject assetBundleObj)
        {
            string path;
            OriginType origin;
            GetABLoadPathOnDisk(assetBundleObj.FormatPath, out path, out origin);

            assetBundleObj.Request = AssetBundle.LoadFromFileAsync(path);
            if (assetBundleObj.Request == null)
            {
                Log.Error("加载AB包时的路径错误！ab名称:{0}", assetBundleObj.FormatPath);
            }

            assetBundleObj.Origin = origin;
        }
        #endregion

        #region 执行卸载 AB（包含内存卸载）
        /// <summary>
        /// 执行卸载 AB
        /// 卸载资源并释放内存
        /// </summary>
        /// <param name="assetBundleObj">AB 包装对象</param>
        private void DoUnload(AssetBundleObject assetBundleObj)
        {
            if (assetBundleObj.AssetBundles == null)
            {
                Log.Error("卸载AB包时错误！ab名称:{0}", assetBundleObj.FormatPath);
                return;
            }

            assetBundleObj.AssetBundles.Unload(true);
            assetBundleObj.AssetBundles = null;
        }
        #endregion

        #region 正常/强制完成加载，并触发所有回调
        /// <summary>
        /// 正常/强制完成加载并触发回调
        /// 支持异步转同步加载
        /// </summary>
        /// <param name="assetBundleObj">AB 包装对象</param>
        private void NormalOrForceLoadOverAndCallBack(AssetBundleObject assetBundleObj)
        {
            // 从异步中提取ab
            if (assetBundleObj.Request != null)
            {
                // 如果没加载完，通过API立刻拿到资源（变异步为同步）
                assetBundleObj.AssetBundles = assetBundleObj.Request.assetBundle;
                assetBundleObj.Request = null;
                m_LoadingAssetBundleList.Remove(assetBundleObj.FormatPath);
                m_LoadedAssetBundleList.Add(assetBundleObj.FormatPath, assetBundleObj);
            }

            if (assetBundleObj.AssetBundles == null)
            {
                string path;
                OriginType origin;
                GetABLoadPathOnDisk(assetBundleObj.FormatPath, out path, out origin);
                assetBundleObj.AssetBundles = AssetBundle.LoadFromFile(path);
                assetBundleObj.Origin = origin;
            }

            // 运行回调
            foreach (var callback in assetBundleObj.AssetBundleLoadOverCallbacksList)
            {
                callback(assetBundleObj, assetBundleObj.AssetBundles);
            }

            assetBundleObj.AssetBundleLoadOverCallbacksList.Clear();
        }
        #endregion

        #region 获取 AB 在磁盘中的实际路径
        /// <summary>
        /// 获取 AB 在磁盘中的实际路径
        /// 优先加载持久化目录，其次加载流资源目录
        /// </summary>
        /// <param name="formatPath">AB 格式化路径</param>
        /// <param name="path">输出实际加载路径</param>
        /// <param name="origin">输出资源来源类型</param>
        private void GetABLoadPathOnDisk(string formatPath, out string path, out OriginType origin)
        {
            // 优先检查读写区域的资源是否存在，如果存在则加载读写区域的资源，否则加载只读区域
            string filePersistentPath = GamePathUtils.AB.Persistent.GetFileFullPath(formatPath);
            if (File.Exists(filePersistentPath))
            {
                path = filePersistentPath;
                origin = OriginType.Persistent;
            }
            else
            {
                path = GamePathUtils.AB.Streaming.GetFileFullPath(formatPath);
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
            if (m_LoadingAssetBundleList.Count == 0) return;

            // 检测加载完的AB
            m_TempLoadeds.Clear();
            foreach (var abObj in m_LoadingAssetBundleList.Values)
            {
                if (abObj.DependLoadingCount == 0)
                {
                    if (abObj.Request != null && abObj.Request.isDone)
                    {
                        m_TempLoadeds.Add(abObj);
                    }
                }
            }

            // 回调中有可能对m_LoadingABList进行操作，提取后回调
            foreach (var abObj in m_TempLoadeds)
            {
                NormalOrForceLoadOverAndCallBack(abObj);
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
            if (m_UnloadAssetBundleList.Count == 0) return;

            m_TempLoadeds.Clear();
            foreach (var abObj in m_UnloadAssetBundleList.Values)
            {
                if (abObj.RefCount == 0 && abObj.AssetBundles != null)
                {
                    // 引用计数为0并且已经加载完，没加载完等加载完销毁
                    DoUnload(abObj);

                    m_LoadedAssetBundleList.Remove(abObj.FormatPath);
                    m_TempLoadeds.Add(abObj);
                }

                if (abObj.RefCount > 0)
                {
                    // 引用计数加回来（销毁又瞬间重新加载，不销毁，从销毁列表移除）
                    m_TempLoadeds.Add(abObj);
                }
            }

            foreach (var abObj in m_TempLoadeds)
            {
                m_UnloadAssetBundleList.Remove(abObj.FormatPath);
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
            if (_mReadyAssetBundleList.Count == 0) return;
            if (m_LoadingAssetBundleList.Count >= MAX_LOADING_COUNT) return;

            m_TempLoadeds.Clear();
            foreach (var abObj in _mReadyAssetBundleList.Values)
            {
                DoLoadAsync(abObj);

                m_TempLoadeds.Add(abObj);
                m_LoadingAssetBundleList.Add(abObj.FormatPath, abObj);

                if (m_LoadingAssetBundleList.Count >= MAX_LOADING_COUNT)
                {
                    break;
                }
            }

            foreach (var abObj in m_TempLoadeds)
            {
                _mReadyAssetBundleList.Remove(abObj.FormatPath);
            }
        }
        #endregion
    }
}