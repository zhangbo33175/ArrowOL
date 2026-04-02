using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Honor.Runtime
{
    public sealed partial class AssetBundleLoadManager
    {
        /// <summary>
        /// 内部同步加载AB
        /// </summary>
        /// <param name="abFormatPath">ab格式化路径</param>
        /// <returns></returns>
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
            // 如果处在【加载中列表（异步方式）】中则立即将异步改成同步加载得到结果（自身与其递归到的所有依赖AB项引用计数全部+1）
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
            // 如果处在【准备加载列表】中则立即进行同步加载得到结果（自身与其递归到的所有依赖AB项引用计数全部+1）
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

        /// <summary>
        /// 内部异步加载AB
        /// </summary>
        /// <param name="abFormatPath">ab格式化路径</param>
        /// <param name="abLoadOverCallback">加载完成回调函数</param>
        /// <returns></returns>
        private AssetBundleObject InternalLoadAssetBundleAsync(string abFormatPath,
            AssetBundleLoadOverCallBack abLoadOverCallback)
        {
            AssetBundleObject assetBundleObj = null;

            // 如果处在【加载完成列表】中则直接返回（自身与其所有的依赖AB项引用计数全部+1）
            if (m_LoadedAssetBundleList.ContainsKey(abFormatPath))
            {
                assetBundleObj = m_LoadedAssetBundleList[abFormatPath];
                AddSelfAndDependsRef(assetBundleObj);
                abLoadOverCallback(assetBundleObj, assetBundleObj.AssetBundles);
                return assetBundleObj;
            }
            // 如果处在【加载中列表（异步方式）】中则立即将新传入的回调函数加入ab封装对象中，等待异步结束后回调（自身与其所有的依赖AB项引用计数全部+1）
            else if (m_LoadingAssetBundleList.ContainsKey(abFormatPath))
            {
                assetBundleObj = m_LoadingAssetBundleList[abFormatPath];
                AddSelfAndDependsRef(assetBundleObj);
                assetBundleObj.AssetBundleLoadOverCallbacksList.Add(abLoadOverCallback);
                return assetBundleObj;
            }
            // 在准备加载中
            // 如果处在【准备加载列表】中则立即将新传入的回调函数加入ab封装对象中，等待异步开始到结束（自身与其所有的依赖AB项引用计数全部+1）
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
                // 将即将开始异步加载的依赖资源的总数量赋给当前新创建的ab资源中，等待后面异步加载依赖资源时对该数量的刷新
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

        /// <summary>
        /// 内部异步卸载AB
        /// </summary>
        /// <param name="abFormatPath">ab格式化路径</param>
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

        /// <summary>
        /// WebGL读取AssetBundle
        /// </summary>
        /// <param name="abFormatPath">ab格式化路径</param>
        /// <returns></returns>
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

        /// <summary>
        /// WebGL内部赋值WebRequest（多线程）
        /// </summary>
        /// <param name="abFormatPath">ab格式化路径</param>
        private async void InternalEvaluateWebRequestFromWebGL(string abFormatPath)
        {
            m_WebGLRequest = await InternalGetWebRequestFromWebGL(abFormatPath);
        }

        /// <summary>
        /// WebGL内部获取WebRequest（多线程）
        /// </summary>
        /// <param name="abFormatPath">ab格式化路径</param>
        private Task<UnityWebRequest> InternalGetWebRequestFromWebGL(string abFormatPath)
        {
            TaskCompletionSource<UnityWebRequest> taskSource = new TaskCompletionSource<UnityWebRequest>();
            string url = GamePathUtils.AB.Streaming.GetFileFullPath(abFormatPath);

            return taskSource.Task;
        }

        /// <summary>
        /// ab自身与所有依赖项AB引用计数+1
        /// 递归调用
        /// </summary>
        /// <param name="assetBundleObj"></param>
        private void AddSelfAndDependsRef(AssetBundleObject assetBundleObj)
        {
            assetBundleObj.RefCount++;

            if (assetBundleObj.Depends.Count == 0) return;
            foreach (var dpObj in assetBundleObj.Depends)
            {
                // 递归依赖项，加载完
                AddSelfAndDependsRef(dpObj);
            }
        }

        /// <summary>
        /// 异步加载AB资源
        /// </summary>
        /// <param name="assetBundleObj">AB封装对象</param>
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

        /// <summary>
        /// 卸载AB资源
        /// </summary>
        /// <param name="assetBundleObj">AB封装对象</param>
        private void DoUnload(AssetBundleObject assetBundleObj)
        {
            // 这里用true，卸载Asset内存，实现指定卸载
            if (assetBundleObj.AssetBundles == null)
            {
                Log.Error("卸载AB包时错误！ab名称:{0}", assetBundleObj.FormatPath);
                return;
            }

            assetBundleObj.AssetBundles.Unload(true);
            assetBundleObj.AssetBundles = null;
        }

        /// <summary>
        /// 正常或强制加载完成并触发回调
        /// </summary>
        /// <param name="assetBundleObj">AB封装对象</param>
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

        /// <summary>
        /// 获取ab包在磁盘上的加载路径
        /// </summary>
        /// <param name="formatPath">ab格式化路径</param>
        /// <param name="path">ab资源路径</param>
        /// <param name="origin">ab资源来源</param>
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

        /// <summary>
        /// “加载中列表”心跳管理
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
                // 加载完进行回调
                NormalOrForceLoadOverAndCallBack(abObj);
            }
        }

        /// <summary>
        /// “卸载列表”心跳管理
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

        /// <summary>
        /// “准备列表”心跳管理
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
    }
}