/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  AssetBundleLoadManager.cs
 * author:    云毅
 * created:   2026
 * descrip:   AssetBundle 加载管理器，负责 AB 包的同步/异步加载、
 *            异步卸载、依赖清单管理、路径格式化与还原等功能
 ***************************************************************/

using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Honor.Runtime
{
    //=========================================================================
    // AssetBundle 加载管理器
    // 管理所有 AssetBundle 的生命周期、依赖关系、加载与卸载流程
    //=========================================================================
    /// <summary>
    /// AssetBundle 加载核心管理器（密封分部类）
    /// 统一管理 AB 包加载、卸载、依赖解析、状态控制
    /// </summary>
    public sealed partial class AssetBundleLoadManager
    {
        #region 构造函数

        /// <summary>
        /// 构造函数
        /// 初始化所有 AB 包管理容器与缓存字典
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

        #endregion

        #region 管理器帧更新

        /// <summary>
        /// 每帧更新
        /// 驱动加载中、就绪、卸载队列的逻辑执行
        /// </summary>
        public void Update()
        {
            UpdateLoadingList();
            UpdateReadyList();
            UpdateUnLoadList();
        }

        #endregion

        #region 加载 AB 依赖清单

        /// <summary>
        /// 加载 AssetBundle 依赖清单文件
        /// 解析并缓存所有 AB 包的依赖关系
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

        #endregion

        #region 同步加载 AB 包

        /// <summary>
        /// 同步加载 AssetBundle 包
        /// </summary>
        /// <param name="abPath">原始 AB 包路径</param>
        /// <returns>加载完成的 AssetBundle 实例</returns>
        public AssetBundle LoadSync(string abPath)
        {
            string formatPath = GetABFormatPath(abPath);
            AssetBundleObject abObj = InternalLoadAssetBundleSync(formatPath);
            return abObj.AssetBundles;
        }

        #endregion

        #region 异步加载 AB 包

        /// <summary>
        /// 异步加载 AssetBundle 包
        /// </summary>
        /// <param name="abPath">原始 AB 包路径</param>
        /// <param name="abLoadOverCallback">加载完成回调函数</param>
        public void LoadAsync(string abPath, AssetBundleLoadOverCallBack abLoadOverCallback)
        {
            string formatPath = GetABFormatPath(abPath);
            InternalLoadAssetBundleAsync(formatPath, abLoadOverCallback);
        }

        #endregion

        #region 异步卸载 AB 包

        /// <summary>
        /// 异步卸载 AssetBundle 包
        /// </summary>
        /// <param name="abPath">原始 AB 包路径</param>
        public void Unload(string abPath)
        {
            string formatPath = GetABFormatPath(abPath);
            InternalUnloadAssetBundleAsync(formatPath);
        }

        #endregion

        #region 判断 AB 包是否存在

        /// <summary>
        /// 判断 AB 包是否存在于依赖清单中
        /// </summary>
        /// <param name="abPath">原始 AB 包路径</param>
        /// <returns>存在返回 true，不存在返回 false</returns>
        public bool IsABExist(string abPath)
        {
            string formatPath = GetABFormatPath(abPath);
            return m_DependsDataList.ContainsKey(formatPath);
        }

        #endregion

        #region 判断 AB 包是否存在于持久化目录

        /// <summary>
        /// 判断 AB 包是否存在于可读写持久化目录
        /// </summary>
        /// <param name="abPath">原始 AB 包路径</param>
        /// <returns>存在返回 true，不存在返回 false</returns>
        public bool IsABExistInPersistentDataPath(string abPath)
        {
            string formatPath = GetABFormatPath(abPath);
            string filePath = GamePathUtils.AB.Persistent.GetFileFullPath(formatPath);
            return File.Exists(filePath);
        }

        #endregion

        #region 获取格式化 AB 路径

        /// <summary>
        /// 获取标准化格式化 AB 路径
        /// 规则：替换 / 为 @ + 全小写 + 添加 .bundle 后缀
        /// </summary>
        /// <param name="abPath">原始 AB 包路径</param>
        /// <returns>格式化后的标准路径</returns>
        public string GetABFormatPath(string abPath)
        {
            if (!m_AssetBundleFormatPathCaches.ContainsKey(abPath))
            {
                m_AssetBundleFormatPathCaches.Add(abPath, $"{abPath.Replace('/', '@').ToLower()}.bundle");
            }

            return m_AssetBundleFormatPathCaches[abPath];
        }

        #endregion

        #region 还原 AB 原始路径

        /// <summary>
        /// 将格式化路径还原为原始资源路径
        /// 优先从缓存读取，无缓存则自动解析还原
        /// </summary>
        /// <param name="abFormatPath">格式化后的 AB 路径</param>
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

        #endregion
    }
}