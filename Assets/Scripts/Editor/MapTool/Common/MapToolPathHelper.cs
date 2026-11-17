/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  MapToolPathHelper.cs
 * author:    云毅
 * created:   2026
 * descrip:   地图编辑器路径转换工具（Assets/xxx ↔ 磁盘绝对路径）
 *            消除 SaveMapConfig / LoadMapConfig 等多处重复的路径拼接代码
 ***************************************************************/

using System.IO;
using UnityEngine;

namespace Honor.Editor.MapTool
{
    /// <summary>
    /// 地图编辑器路径转换工具
    /// 统一处理 "Assets/xxx" 与磁盘绝对路径之间的转换
    /// </summary>
    public static class MapToolPathHelper
    {
        /// <summary>
        /// Assets 前缀（常量，避免散落字符串）
        /// </summary>
        public const string AssetsPrefix = "Assets/";

        /// <summary>
        /// Assets 目录名（不带斜杠，用于 CreateFolder / 路径截取等只需要目录名的场景）
        /// </summary>
        public const string AssetsFolderName = "Assets";

        /// <summary>
        /// 将任意路径规范化为 "Assets/xxx" 形式
        /// 处理：反斜杠转正斜杠、补 Assets/ 前缀、去掉前导斜杠
        /// </summary>
        /// <param name="path">原始路径（可能是 Assets/xxx、xxx/yyy 或绝对路径）</param>
        /// <returns>规范化后的 Assets 相对路径</returns>
        public static string NormalizeToAssetRelative(string path)
        {
            if (string.IsNullOrEmpty(path)) return string.Empty;
            string normalized = path.Replace("\\", "/").TrimStart('/');
            if (!normalized.StartsWith(AssetsPrefix))
                normalized = AssetsPrefix + normalized;
            return normalized;
        }

        /// <summary>
        /// 将 "Assets/xxx" 转换为磁盘绝对路径
        /// </summary>
        /// <param name="assetRelativePath">Assets 相对路径</param>
        /// <returns>磁盘绝对路径；输入为空返回空字符串</returns>
        public static string ToAbsolutePath(string assetRelativePath)
        {
            if (string.IsNullOrEmpty(assetRelativePath)) return string.Empty;
            string normalized = NormalizeToAssetRelative(assetRelativePath);
            string relativePart = normalized.Substring(AssetsPrefix.Length);
            return Path.Combine(Application.dataPath, relativePart);
        }

        /// <summary>
        /// 将磁盘绝对路径转换为 "Assets/xxx" 形式
        /// </summary>
        /// <param name="fullPath">磁盘绝对路径</param>
        /// <returns>Assets 相对路径；不在 Assets 目录下返回原路径</returns>
        public static string ToAssetRelative(string fullPath)
        {
            if (string.IsNullOrEmpty(fullPath)) return string.Empty;
            string normalized = fullPath.Replace("\\", "/");
            string dataPath = Application.dataPath.Replace("\\", "/");
            if (normalized.StartsWith(dataPath))
            {
                normalized = AssetsPrefix + normalized.Substring(dataPath.Length + 1);
            }
            return normalized;
        }

        /// <summary>
        /// 项目根目录（Assets 的上一级磁盘路径）
        /// 统一口径：不再各处手写 Path.GetDirectoryName(Application.dataPath) 或 Path.Combine(dataPath, "..")
        /// </summary>
        public static string ProjectRoot
        {
            get { return Path.GetFullPath(Path.GetDirectoryName(Application.dataPath)); }
        }

        /// <summary>
        /// 将项目根目录相对路径转换为磁盘绝对路径（如 "Docs/Designs/xx.xlsx"）
        /// </summary>
        /// <param name="projectRelativePath">相对项目根目录的路径</param>
        /// <returns>磁盘绝对路径；输入为空返回空字符串</returns>
        public static string ToProjectAbsolutePath(string projectRelativePath)
        {
            if (string.IsNullOrEmpty(projectRelativePath)) return string.Empty;
            return Path.Combine(ProjectRoot, projectRelativePath.Replace("\\", "/"));
        }

    }
}
