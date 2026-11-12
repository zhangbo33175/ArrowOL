/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  AtlasPathUtils.cs
 * author:    云毅
 * created:   2026
 * descrip:   路径转换工具类
 ***************************************************************/

using System.IO;
using UnityEditor;
using UnityEngine;

namespace Honor.Editor.MapTool.AtlasPacker
{
    /// <summary>
    /// 路径转换工具
    /// 处理 Assets/... 路径和磁盘全路径之间的转换
    /// </summary>
    internal static class AtlasPathUtils
    {
        /// <summary>
        /// 把 Assets/... 资源路径转成磁盘全路径
        /// </summary>
        public static string AssetToFullPath(string assetPath)
        {
            if (string.IsNullOrEmpty(assetPath)) return null;

            // 如果已经是全路径，直接返回
            if (Path.IsPathRooted(assetPath)) return assetPath;

            // 拼接项目磁盘路径
            string projectPath = Application.dataPath;
            // Application.dataPath 返回 .../Assets，我们要去掉 Assets
            projectPath = projectPath.Substring(0, projectPath.Length - "Assets".Length);
            return Path.Combine(projectPath, assetPath);
        }

        /// <summary>
        /// 把磁盘全路径转成 Assets/... 资源路径
        /// </summary>
        public static string FullToAssetPath(string fullPath)
        {
            if (string.IsNullOrEmpty(fullPath)) return null;

            // 统一路径分隔符为正斜杠
            fullPath = fullPath.Replace('\\', '/');

            string projectPath = Application.dataPath.Replace('\\', '/');
            // 去掉项目前缀
            if (fullPath.StartsWith(projectPath))
            {
                return "Assets" + fullPath.Substring(projectPath.Length);
            }
            return fullPath;
        }

        /// <summary>
        /// 计算下一个 2 的幂（POT 对齐）
        /// </summary>
        public static int NextPOT(int value)
        {
            if (value <= 0) return 1;
            value--;
            value |= value >> 1;
            value |= value >> 2;
            value |= value >> 4;
            value |= value >> 8;
            value |= value >> 16;
            return value + 1;
        }

        /// <summary>
        /// 确保目录存在
        /// </summary>
        public static void EnsureDirectoryExists(string path)
        {
            string dir = Path.GetDirectoryName(path);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }

        /// <summary>
        /// 获取文件名（不带扩展名）
        /// </summary>
        public static string GetFileNameWithoutExtension(string path)
        {
            return Path.GetFileNameWithoutExtension(path);
        }

        /// <summary>
        /// 获取文件扩展名（带点）
        /// </summary>
        public static string GetExtension(string path)
        {
            return Path.GetExtension(path);
        }
    }
}
