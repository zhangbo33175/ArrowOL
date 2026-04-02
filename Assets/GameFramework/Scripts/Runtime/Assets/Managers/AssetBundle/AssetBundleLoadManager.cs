using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Honor.Runtime
{
    public sealed partial class AssetBundleLoadManager
    {
        /// <summary>
        /// AB包加载管理器构造
        /// </summary>
        public AssetBundleLoadManager()
        {
            m_TempLoadeds = new List<AssetBundleObject>();
            m_DependsDataList = new Dictionary<string, string[]>();
            _mReadyAssetBundleList = new Dictionary<string, AssetBundleObject>();
            m_LoadingAssetBundleList = new Dictionary<string, AssetBundleObject>();
            m_LoadedAssetBundleList = new Dictionary<string, AssetBundleObject>();
            m_UnloadAssetBundleList = new Dictionary<string, AssetBundleObject>();
            m_AssetBundleFormatPathCaches = new Dictionary<string, string>();
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
        /// 加载Manifest信息
        /// </summary>
        public void LoadManifest()
        {
            string abFormatPath = AorTxt.Format("{0}.bundle", GamePathUtils.PlatformName);
            GetABLoadPathOnDisk(abFormatPath, out string path, out OriginType origin);

            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            m_DependsDataList.Clear();

            AssetBundle ab = AssetBundle.LoadFromFile(path);
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
                m_DependsDataList.Add(assetBundleName, dpsNames);
            }

            ab.Unload(true);
            ab = null;

            Log.Info("AB加载管理器中全局依赖资源数量：{0}", m_DependsDataList.Count);
        }

        /// <summary>
        /// 同步加载
        /// </summary>
        /// <param name="abPath">ab路径</param>
        /// <returns></returns>
        public AssetBundle LoadSync(string abPath)
        {
            string formatPath = GetABFormatPath(abPath);
            var abObj = InternalLoadAssetBundleSync(formatPath);
            return abObj.AssetBundles;
        }

        /// <summary>
        /// 异步加载
        /// </summary>
        /// <param name="abPath">ab路径</param>
        /// <param name="abLoadOverCallback">加载完成回调函数</param>
        public void LoadAsync(string abPath, AssetBundleLoadOverCallBack abLoadOverCallback)
        {
            string formatPath = GetABFormatPath(abPath);
            InternalLoadAssetBundleAsync(formatPath, abLoadOverCallback);
        }

        /// <summary>
        /// 卸载（异步）
        /// </summary>
        /// <param name="abPath">ab路径</param>
        public void Unload(string abPath)
        {
            string formatPath = GetABFormatPath(abPath);
            InternalUnloadAssetBundleAsync(formatPath);
        }

        /// <summary>
        /// AB资源文件是否存在
        /// </summary>
        /// <param name="abPath">ab路径</param>
        /// <returns></returns>
        public bool IsABExist(string abPath)
        {
            string formatPath = GetABFormatPath(abPath);
            return m_DependsDataList.ContainsKey(formatPath);
        }

        /// <summary>
        /// AB资源文件在Persistent读写目录中是否存在
        /// </summary>
        /// <param name="abPath">ab路径</param>
        /// <returns></returns>
        public bool IsABExistInPersistentDataPath(string abPath)
        {
            string formatPath = GetABFormatPath(abPath);
            string filePath = GamePathUtils.AB.Persistent.GetFileFullPath(formatPath);
            return File.Exists(filePath);
        }

        /// <summary>
        /// 获取AB包的格式化路径
        /// </summary>
        /// <param name="abPath">ab路径</param>
        /// <returns></returns>
        public string GetABFormatPath(string abPath)
        {
            if (!m_AssetBundleFormatPathCaches.ContainsKey(abPath))
            {
                m_AssetBundleFormatPathCaches.Add(abPath, $"{abPath.Replace('/', '@').ToLower()}.bundle");
            }

            return m_AssetBundleFormatPathCaches[abPath];
        }

        /// <summary>
        /// 获取AB包还原后的路径
        /// 注意：还原后的路径大小写不敏感
        /// </summary>
        /// <param name="abFormatPath">ab格式化路径</param>
        /// <returns></returns>
        public string GetABRestoredPath(string abFormatPath)
        {
            string restoredPath = string.Empty;
            foreach (var itr in m_AssetBundleFormatPathCaches)
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
    }
}