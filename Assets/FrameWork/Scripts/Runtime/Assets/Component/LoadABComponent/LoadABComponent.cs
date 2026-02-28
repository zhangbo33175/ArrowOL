using System.IO;
using UnityEngine;

namespace Honor.Runtime
{
    public partial class LoadABComponent
    {
        /// <summary>
        /// 加载Manifest信息
        /// </summary>
        public void LoadABManifest()
        {
            string abFormatPath = AorTxt.Format("{0}.bundle", GamePathUtils.PlatformName);
            //获取ab包在磁盘上的加载路径
            AssetManager.Instance.GetABLoadPathOnDisk(abFormatPath, out string path, out ResourceType origin);
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            AssetManager.Instance.m_DependsDataABList.Clear();
            AssetBundle ab = null;
            //加载资源
            ab = AssetBundle.LoadFromFile(path);
            if (ab == null)
            {
                Log.Error("加载AB包{0}错误！", GamePathUtils.PlatformName);
                return;
            }

            AssetBundleManifest mainfest = ab.LoadAsset("AssetBundleManifest") as AssetBundleManifest;
            if (mainfest == null)
            {
                Log.Error("加载{0}.Manifest信息错误！", GamePathUtils.PlatformName);
                return;
            }

            string[] assetBundleNames = mainfest.GetAllAssetBundles();
            foreach (string assetBundleName in assetBundleNames)
            {
                string[] dpsNames = mainfest.GetAllDependencies(assetBundleName);
                AssetManager.Instance.m_DependsDataABList.Add(assetBundleName, dpsNames);
            }

            ab.Unload(true);
            ab = null;

            Log.Info("AB加载管理器中全局依赖资源数量：{0}", AssetManager.Instance.m_DependsDataABList.Count);
        }

        /// <summary>
        /// 同步加载
        /// </summary>
        /// <param name="abPath">ab路径</param>
        /// <returns></returns>
        public AssetBundle LoadABSync(string abPath)
        {
            string formatPath = GetABFormatPath(abPath);
            var abObj = Internal_Load_AB_Asset_Bundle_Sync(formatPath);
            return abObj.AssetBundle;
        }

        /// <summary>
        /// 异步加载
        /// </summary>
        /// <param name="abPath">ab路径</param>
        /// <param name="abOverCallback">加载完成回调函数</param>
        public void LoadABAsync(string abPath, LoadAssetBundleOverCallBack abOverCallback)
        {
            string formatPath = GetABFormatPath(abPath);
            InternalLoadABAssetBundleAsync(formatPath, abOverCallback);
        }

        /// <summary>
        /// 卸载（异步）
        /// </summary>
        /// <param name="abPath">ab路径</param>
        public void UnloadAB(string abPath)
        {
            string formatPath = GetABFormatPath(abPath);
            InternalUnloadABAssetBundleAsync(formatPath);
        }

        /// <summary>
        /// AB资源文件是否存在
        /// </summary>
        /// <param name="abPath">ab路径</param>
        /// <returns></returns>
        public bool IsABExist(string abPath)
        {
            string formatPath = GetABFormatPath(abPath);
            return AssetManager.Instance.m_DependsDataABList.ContainsKey(formatPath);
        }

        /// <summary>
        /// AB资源文件在Persistent读写目录中是否存在
        /// </summary>
        /// <param name="abPath">ab路径</param>
        /// <returns></returns>
        public bool IsABExistInPersistentDataPath(string abPath)
        {
            string formatPath = GetABFormatPath(abPath);
            string filePath = GamePathUtils.AssetBundleAB.Persistent.GetFileFullPath(formatPath);
            return File.Exists(filePath);
        }

        /// <summary>
        /// 获取AB包的格式化路径
        /// </summary>
        /// <param name="abPath">ab路径</param>
        /// <returns></returns>
        public  string GetABFormatPath(string abPath)
        {
            if (!AssetManager.Instance.m_ABFormatPathCaches.ContainsKey(abPath))
            {
                AssetManager.Instance.m_ABFormatPathCaches.Add(abPath, $"{abPath.Replace('/', '@').ToLower()}.bundle");
            }

            return AssetManager.Instance.m_ABFormatPathCaches[abPath];
        }

        /// <summary>
        /// 获取AB包还原后的路径
        /// 注意：还原后的路径大小写不敏感
        /// </summary>
        /// <param name="abFormatPath">ab格式化路径</param>
        /// <returns></returns>
        public  string GetABRestoredPath(string abFormatPath)
        {
            string restoredPath = string.Empty;
            foreach (var itr in AssetManager.Instance.m_ABFormatPathCaches)
            {
                if (itr.Value.Equals(abFormatPath))
                {
                    restoredPath = itr.Key;
                    break;
                }
            }

            if (string.IsNullOrEmpty(restoredPath))
            {
                string[] picesNames = abFormatPath.Replace(".bundle", string.Empty).Split('@');
                foreach (var piceName in picesNames)
                {
                    string name = $"{piceName.Substring(0, 1).ToUpper()}{piceName.Substring(1)}";
                    restoredPath = string.IsNullOrEmpty(restoredPath) ? name : $"{restoredPath}/{name}";
                }
            }

            return restoredPath;
        }

        /// <summary>
        /// 管理器心跳
        /// </summary>
        public void Update()
        {
            UpdateLoadingList();
            UpdateReadyList();
            UpdateUnLoadList();
        }

        /// <summary>
        /// “加载中列表”心跳管理
        /// </summary>
        private void UpdateLoadingList()
        {
            if (AssetManager.Instance.m_LoadingABList.Count == 0) return;

            // 检测加载完的AB
            AssetManager.Instance.m_TempLoadedsAB.Clear();
            foreach (var abObj in AssetManager.Instance.m_LoadingABList.Values)
            {
                if (abObj.DependLoadingCount == 0)
                {
#if UNITY_WEBGL
                    m_TempLoadeds.Add(abObj);
#else
                    if (abObj.Request != null && abObj.Request.isDone)
                    {
                        AssetManager.Instance.m_TempLoadedsAB.Add(abObj);
                    }
#endif
                }
            }

            // 回调中有可能对m_LoadingABList进行操作，提取后回调
            foreach (var abObj in AssetManager.Instance.m_TempLoadedsAB)
            {
                // 加载完进行回调
                NormalOrForceLoadABOverAndCallBack(abObj);
            }
        }

        /// <summary>
        /// “卸载列表”心跳管理
        /// </summary>
        private  void UpdateUnLoadList()
        {
            if (AssetManager.Instance.m_UnloadABList.Count == 0) return;

            AssetManager.Instance.m_TempLoadedsAB.Clear();
            foreach (var abObj in AssetManager.Instance.m_UnloadABList.Values)
            {
                if (abObj.RefCount == 0 && abObj.AssetBundle != null)
                {
                    // 引用计数为0并且已经加载完，没加载完等加载完销毁
                    DoUnloadAB(abObj);

                    AssetManager.Instance.m_LoadedABList.Remove(abObj.FormatPath);
                    AssetManager.Instance.m_TempLoadedsAB.Add(abObj);
                }

                if (abObj.RefCount > 0)
                {
                    // 引用计数加回来（销毁又瞬间重新加载，不销毁，从销毁列表移除）
                    AssetManager.Instance.m_TempLoadedsAB.Add(abObj);
                }
            }

            foreach (var abObj in AssetManager.Instance.m_TempLoadedsAB)
            {
                AssetManager.Instance.m_UnloadABList.Remove(abObj.FormatPath);
            }
        }

        /// <summary>
        /// “准备列表”心跳管理
        /// </summary>
        private  void UpdateReadyList()
        {
            if (AssetManager.Instance.m_ReadyABList.Count == 0) return;
            if (AssetManager.Instance.m_LoadingABList.Count >= AssetManager.MAX_LOADING_COUNT) return;

            AssetManager.Instance.m_TempLoadedsAB.Clear();
            foreach (var abObj in AssetManager.Instance.m_ReadyABList.Values)
            {
                DoLoadABAsync(abObj);

                AssetManager.Instance.m_TempLoadedsAB.Add(abObj);
                AssetManager.Instance.m_LoadingABList.Add(abObj.FormatPath, abObj);

                if (AssetManager.Instance.m_LoadingABList.Count >= AssetManager.MAX_LOADING_COUNT)
                {
                    break;
                }
            }

            foreach (var abObj in AssetManager.Instance.m_TempLoadedsAB)
            {
                AssetManager.Instance.m_ReadyABList.Remove(abObj.FormatPath);
            }
        }

        /*public override void Dispose()
        {
            Log.Info("LoadABManager");
        }*/
    }
}