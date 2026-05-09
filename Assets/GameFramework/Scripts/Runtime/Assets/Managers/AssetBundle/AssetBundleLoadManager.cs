using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Honor.Runtime
{
    public sealed partial class AssetBundleLoadManager
    {
        /// <summary>
        /// 构造函数：初始化所有AB包管理容器
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
        /// 每帧更新：驱动加载、就绪、卸载流程
        /// </summary>
        public void Update()
        {
            UpdateLoadingList();
            UpdateReadyList();
            UpdateUnLoadList();
        }

        /// <summary>
        /// 加载AB依赖清单文件（AssetBundleManifest）
        /// 读取所有AB包的依赖关系，存入全局依赖表
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
        /// 同步加载AB包
        /// </summary>
        /// <param name="abPath">原始AB路径</param>
        /// <returns>加载完成的AB包</returns>
        public AssetBundle LoadSync(string abPath)
        {
            string formatPath = GetABFormatPath(abPath);
            var abObj = InternalLoadAssetBundleSync(formatPath);
            return abObj.AssetBundles;
        }

        /// <summary>
        /// 异步加载AB包
        /// </summary>
        /// <param name="abPath">原始AB路径</param>
        /// <param name="abLoadOverCallback">加载完成回调</param>
        public void LoadAsync(string abPath, AssetBundleLoadOverCallBack abLoadOverCallback)
        {
            string formatPath = GetABFormatPath(abPath);
            InternalLoadAssetBundleAsync(formatPath, abLoadOverCallback);
        }

        /// <summary>
        /// 异步卸载AB包
        /// </summary>
        /// <param name="abPath">原始AB路径</param>
        public void Unload(string abPath)
        {
            string formatPath = GetABFormatPath(abPath);
            InternalUnloadAssetBundleAsync(formatPath);
        }

        /// <summary>
        /// 判断AB包是否存在（依赖表中存在）
        /// </summary>
        /// <param name="abPath">原始AB路径</param>
        /// <returns>是否存在</returns>
        public bool IsABExist(string abPath)
        {
            string formatPath = GetABFormatPath(abPath);
            return m_DependsDataList.ContainsKey(formatPath);
        }

        /// <summary>
        /// 判断AB包是否存在于可读写目录(Persistent)
        /// </summary>
        /// <param name="abPath">原始AB路径</param>
        /// <returns>是否存在</returns>
        public bool IsABExistInPersistentDataPath(string abPath)
        {
            string formatPath = GetABFormatPath(abPath);
            string filePath = GamePathUtils.AB.Persistent.GetFileFullPath(formatPath);
            return File.Exists(filePath);
        }

        /// <summary>
        /// 获取格式化后的AB包路径（统一命名规则）
        /// 格式：路径替换@符号 + 小写 + .bundle
        /// </summary>
        /// <param name="abPath">原始AB路径</param>
        /// <returns>格式化路径</returns>
        public string GetABFormatPath(string abPath)
        {
            if (!m_AssetBundleFormatPathCaches.ContainsKey(abPath))
            {
                m_AssetBundleFormatPathCaches.Add(abPath, $"{abPath.Replace('/', '@').ToLower()}.bundle");
            }

            return m_AssetBundleFormatPathCaches[abPath];
        }

        /// <summary>
        /// 将格式化路径还原为原始AB路径
        /// </summary>
        /// <param name="abFormatPath">格式化路径</param>
        /// <returns>还原后的原始路径</returns>
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