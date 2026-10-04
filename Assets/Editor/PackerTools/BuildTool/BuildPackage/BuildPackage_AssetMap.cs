/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Editor
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  BuildPackage_AssetMap.cs
 * author:    云毅
 * created:   2026
 * descrip:   AssetBundle 打包工具 - 资源收集与 Bundle 分组
 *            依据打包配置收集资源文件，按目录粒度计算 bundle 名并分组，
 *            生成"bundle 名 → 资源路径列表"映射（参照 BuildPackage_CfgAB 架构）
 ***************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Honor.Editor.BuildPackage
{
    //=========================================================================
    // Bundle 分组映射构建器
    // 负责资源收集、后缀过滤、bundle 名计算与分组
    //=========================================================================
    /// <summary>
    /// Bundle 分组映射构建器
    /// 说明：核心打包算法——按配置收集资源、过滤后缀、按目录粒度计算 bundle 名并分组
    /// </summary>
    public static class BuildPackage_AssetMap
    {
        #region 成员
        /// <summary>
        /// Bundle 分组映射：bundle 名 → 资源路径列表
        /// </summary>
        public static readonly Dictionary<string, List<string>> BundleMap = new Dictionary<string, List<string>>();

        /// <summary>
        /// 资源黑名单（路径前缀命中即跳过，用于排除不需要打包的目录）
        /// 说明：UILauncher 启动预制体（UILauncherLogoView / UILauncherView）由启动流程
        ///       以 AssetBundle 模式从 AB 加载，绝不能排除；故当前黑名单为空。
        ///       如需排除其它目录，在此追加路径前缀即可（如 "Assets/Res/MapRes/Terrains"）。
        /// </summary>
        private static readonly List<string> s_BlackListPrefix = new List<string>
        {
            // （无）—— 启动闪屏/加载界面必须打进 AB，运行时按 AssetBundle 模式加载
        };
        #endregion

        #region 一键构建映射
        /// <summary>
        /// 依据配置列表构建 Bundle 分组映射（打包前调用）
        /// </summary>
        /// <param name="configs">打包配置列表</param>
        /// <returns>构建的 bundle 分组数</returns>
        public static int BuildMap(List<BuildPackageConfig> configs)
        {
            BundleMap.Clear();

            if (configs == null)
                return 0;

            foreach (BuildPackageConfig cfg in configs)
            {
                if (cfg == null || !cfg.Enabled)
                    continue;

                Pack(cfg);
            }

            Debug.Log($"[AB打包] 生成 bundle 分组数：{BundleMap.Count}");
            return BundleMap.Count;
        }

        /// <summary>
        /// 处理单个打包配置：收集目录下资源并按粒度分桶
        /// </summary>
        /// <param name="cfg">打包配置</param>
        private static void Pack(BuildPackageConfig cfg)
        {
            if (string.IsNullOrEmpty(cfg.Path) || !Directory.Exists(cfg.Path))
            {
                Debug.LogWarning($"[AB打包] 配置路径无效或不存在：{cfg.Path}");
                return;
            }

            // 收集目录下全部资源文件（递归）
            List<string> assetList = CollectAssetFiles(cfg.Path);

            // 先处理子目录细分（命中子目录的走子规则并从列表移除）
            if (cfg.SubConfigs != null && cfg.SubConfigs.Count > 0)
            {
                foreach (BuildPackageSubConfig sub in cfg.SubConfigs)
                {
                    if (sub == null || string.IsNullOrEmpty(sub.SubDirStr))
                        continue;

                    string[] subPatterns = SplitPattern(sub.SubPattern);
                    for (int i = assetList.Count - 1; i >= 0; i--)
                    {
                        string asset = assetList[i];
                        if (asset.IndexOf(sub.SubDirStr, StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            PackFile(asset, sub.SubFormat, subPatterns);
                            assetList.RemoveAt(i);
                        }
                    }
                }
            }

            // 处理剩余资源（父配置规则）
            string[] patterns = cfg.GetPatterns();
            foreach (string asset in assetList)
            {
                PackFile(asset, cfg.Format, patterns);
            }
        }

        /// <summary>
        /// 处理单个文件：过滤后缀、计算 bundle 名并分桶
        /// </summary>
        /// <param name="assetPath">资源相对路径（Assets 开头）</param>
        /// <param name="format">打包粒度</param>
        /// <param name="searchPattern">后缀过滤数组（空表示不过滤）</param>
        private static void PackFile(string assetPath, BuildPackageFormat format, string[] searchPattern)
        {
            // 黑名单过滤
            foreach (string prefix in s_BlackListPrefix)
            {
                if (assetPath.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                    return;
            }

            // 后缀过滤
            if (searchPattern != null && searchPattern.Length > 0)
            {
                string ext = Path.GetExtension(assetPath).ToLower();
                if (Array.IndexOf(searchPattern, ext) < 0)
                    return;
            }

            // 计算 bundle 名
            string bundleName = GetBundleName(assetPath, format);
            if (string.IsNullOrEmpty(bundleName))
                return;

            // 分桶（同 bundle 内去重）
            if (!BundleMap.TryGetValue(bundleName, out List<string> assets))
            {
                assets = new List<string>();
                BundleMap.Add(bundleName, assets);
            }

            if (!assets.Contains(assetPath))
                assets.Add(assetPath);
        }
        #endregion

        #region Bundle 名计算
        /// <summary>
        /// 计算资源对应的 bundle 名
        /// 规则：资源相对根目录的路径（不含后缀）→ '/' 替换为 '@' → 全小写 → 加 ".bundle"
        /// 目录粒度：粒度越大，取越靠前的目录层级作为 bundle 名（同目录资源合包）
        /// </summary>
        /// <param name="assetPath">资源相对路径</param>
        /// <param name="format">打包粒度</param>
        /// <returns>格式化 bundle 名（含 .bundle 后缀）</returns>
        public static string GetBundleName(string assetPath, BuildPackageFormat format)
        {
            // 取资源相对根目录的路径（不含后缀）
            string relative = GetRelativePathWithoutExt(assetPath);
            if (string.IsNullOrEmpty(relative))
                return string.Empty;

            string[] segments = relative.Split('/');

            // 按粒度截取目录层级
            string groupPath;
            switch (format)
            {
                case BuildPackageFormat.CurrentDir:
                case BuildPackageFormat.FileDir:
                    // 取父目录（去掉最后一个文件/目录段）
                    groupPath = segments.Length > 1
                        ? string.Join("/", segments, 0, segments.Length - 1)
                        : relative;
                    break;

                case BuildPackageFormat.Level1Dir:
                    // 取一级目录
                    groupPath = segments.Length > 0 ? segments[0] : relative;
                    break;

                case BuildPackageFormat.Level2Dir:
                    // 取一、二级目录
                    groupPath = segments.Length > 1
                        ? string.Join("/", segments, 0, Math.Min(2, segments.Length))
                        : relative;
                    break;

                case BuildPackageFormat.Level3Dir:
                    // 取一、二、三级目录
                    groupPath = segments.Length > 2
                        ? string.Join("/", segments, 0, Math.Min(3, segments.Length))
                        : relative;
                    break;

                case BuildPackageFormat.EachFile:
                default:
                    // 每个文件单独打包：用完整相对路径（不含后缀）
                    groupPath = relative;
                    break;
            }

            // 格式化：'/' → '@'，全小写，加后缀
            return FormatBundleName(groupPath);
        }

        /// <summary>
        /// 获取资源相对根目录的路径（不含后缀）
        /// 说明：剥离 Assets/Res/ 前缀并去掉文件扩展名
        /// </summary>
        /// <param name="assetPath">资源相对路径</param>
        /// <returns>相对根目录路径（不含后缀，正斜杠分隔）</returns>
        public static string GetRelativePathWithoutExt(string assetPath)
        {
            if (string.IsNullOrEmpty(assetPath))
                return string.Empty;

            string normalized = assetPath.Replace('\\', '/');

            // 剥离根目录前缀
            string root = BuildPackage_Define.AssetResRoot + "/";
            if (normalized.StartsWith(root, StringComparison.OrdinalIgnoreCase))
                normalized = normalized.Substring(root.Length);
            else if (normalized.StartsWith(BuildPackage_Define.AssetResRoot, StringComparison.OrdinalIgnoreCase))
                normalized = normalized.Substring(BuildPackage_Define.AssetResRoot.Length).TrimStart('/');

            // 去掉扩展名（保留最后一个 '.' 之前的部分）
            int extIndex = normalized.LastIndexOf('.');
            if (extIndex > 0)
                normalized = normalized.Substring(0, extIndex);

            return normalized;
        }

        /// <summary>
        /// 格式化 bundle 名：'/' → '@'，全小写，加 ".bundle"
        /// 与运行时 AssetBundleLoadManager.GetABFormatPath 完全一致
        /// </summary>
        /// <param name="path">原始路径（正斜杠）</param>
        /// <returns>格式化 bundle 名</returns>
        public static string FormatBundleName(string path)
        {
            return $"{path.Replace('/', BuildPackage_Define.PathSeparatorReplace).ToLower()}{BuildPackage_Define.BundleSuffix}";
        }
        #endregion

        #region 工具方法
        /// <summary>
        /// 收集目录下所有资源文件（递归，排除 .meta）
        /// </summary>
        /// <param name="directory">目录路径（相对 Assets）</param>
        /// <returns>资源相对路径列表（Assets 开头）</returns>
        public static List<string> CollectAssetFiles(string directory)
        {
            List<string> result = new List<string>();
            if (!Directory.Exists(directory))
                return result;

            string fullRoot = directory.Replace('\\', '/');

            foreach (string fullPath in Directory.GetFiles(directory, "*", SearchOption.AllDirectories))
            {
                if (fullPath.EndsWith(".meta", StringComparison.OrdinalIgnoreCase))
                    continue;

                string assetPath = fullPath.Replace('\\', '/');
                result.Add(assetPath);
            }

            return result;
        }

        /// <summary>
        /// 解析分号分隔的后缀过滤为小写数组
        /// </summary>
        /// <param name="pattern">后缀字符串（分号分隔）</param>
        /// <returns>小写后缀数组</returns>
        private static string[] SplitPattern(string pattern)
        {
            if (string.IsNullOrEmpty(pattern))
                return Array.Empty<string>();

            List<string> list = new List<string>();
            foreach (string p in pattern.Split(';'))
            {
                string t = p.Trim().ToLower();
                if (!string.IsNullOrEmpty(t))
                    list.Add(t);
            }

            return list.ToArray();
        }
        #endregion
    }
}
