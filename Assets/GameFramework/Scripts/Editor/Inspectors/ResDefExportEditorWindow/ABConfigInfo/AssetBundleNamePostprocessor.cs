/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Editor
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  AssetBundleNamePostprocessor.cs
 * author:    云毅
 * created:   2026
 * descrip:   AssetBundle 自动命名、打包规则配置、增量分组管理工具
 ***************************************************************/
using System.Collections.Generic;
using System.IO;
using Honor.Runtime;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;

namespace Honor.Editor
{
    /// <summary>
    /// AssetBundle 命名自动处理器
    /// 根据配置自动设置资源 AB 包名、增量分组、公用分组等信息
    /// </summary>
    public class AssetBundleNamePostprocessor : AssetPostprocessor
    {
        #region 静态配置缓存
        /// <summary>
        /// AB 配置信息映射 <资源路径, ABConfigInfo>
        /// </summary>
        public static Dictionary<string, ABConfigInfo> s_ABConfigs = new Dictionary<string, ABConfigInfo>();

        /// <summary>
        /// AB 是否为增量包 <abName, bool>
        /// </summary>
        private static Dictionary<string, bool> s_ABIsIncreaser = new Dictionary<string, bool>();

        /// <summary>
        /// AB 是否为公用增量包 <abName, bool>
        /// </summary>
        private static Dictionary<string, bool> s_ABIsCommonIncreaser = new Dictionary<string, bool>();

        /// <summary>
        /// AB 对应分组名称 <abName, groupName>
        /// </summary>
        private static Dictionary<string, string> s_ABGroupName = new Dictionary<string, string>();
        #endregion

        #region 配置刷新
        /// <summary>
        /// 从 ABConfigs.json 重新加载所有 AB 配置
        /// </summary>
        public static void RefreshABConfigs()
        {
            string filePath = GamePathUtils.Json.GetRootDirectoryFullPath() + "/ABConfigs.json";
            string content = File.ReadAllText(filePath);

            if (string.IsNullOrEmpty(content))
                return;

            JObject jObject = JObject.Parse(content);
            s_ABConfigs.Clear();

            foreach (var item in jObject)
            {
                JToken data = item.Value;
                string path = data["Path"].ToString();

                if (!s_ABConfigs.ContainsKey(path))
                {
                    ABConfigInfo info = new ABConfigInfo(
                        int.Parse(data["ID"].ToString()),
                        path,
                        int.Parse(data["PackageMeasureType"].ToString()),
                        data["Rename"].ToString(),
                        data["GroupName"].ToString(),
                        bool.Parse(data["IsIncreaserGroup"].ToString()),
                        bool.Parse(data["IsCommonIncreaserGroup"].ToString())
                    );

                    s_ABConfigs.Add(path, info);
                }
            }
        }

        /// <summary>
        /// 清空工程中所有 AssetBundle 名称
        /// </summary>
        public static void CleanAllAsssetBundleNames()
        {
            RefreshABConfigs();

            string[] bundleNames = AssetDatabase.GetAllAssetBundleNames();
            for (int i = 0; i < bundleNames.Length; i++)
            {
                AssetDatabase.RemoveAssetBundleName(bundleNames[i], true);
            }

            AssetDatabase.Refresh();
        }
        #endregion

        #region 自动设置 AB 包名（核心）
        /// <summary>
        /// 根据配置自动刷新所有资源的 AssetBundle 名称
        /// </summary>
        /// <param name="forceCheckPath">是否强制校验路径有效性</param>
        public static void RefreshAllAssetBundleNames(bool forceCheckPath = false)
        {
            CleanAllAsssetBundleNames();

            s_ABIsIncreaser.Clear();
            s_ABIsCommonIncreaser.Clear();
            s_ABGroupName.Clear();

            foreach (var config in s_ABConfigs)
            {
                ABConfigInfo info = config.Value;

                // 0 = 文件/目录单独打包
                if (info.PackageMeasureType == 0)
                {
                    ProcessPackageType_SingleFileOrDir(info, forceCheckPath);
                }
                // 1 = 目录下每个文件单独打包
                else if (info.PackageMeasureType == 1)
                {
                    ProcessPackageType_EachFile(info, forceCheckPath);
                }
                // 2 = 目录下一级子目录单独打包
                else if (info.PackageMeasureType == 2)
                {
                    ProcessPackageType_EachSubDir(info, forceCheckPath);
                }
                // 3 = 目录下一级文件 + 子目录分别打包
                else if (info.PackageMeasureType == 3)
                {
                    ProcessPackageType_FileAndSubDir(info, forceCheckPath);
                }
                // 4 = 小游戏专用打包规则
                else if (info.PackageMeasureType == 4)
                {
                    ProcessPackageType_MiniGame(info, forceCheckPath);
                }
            }

            AssetDatabase.Refresh();
        }
        #endregion

        #region 各种打包规则实现
        /// <summary>
        /// 类型0：单个文件/目录打包
        /// </summary>
        private static void ProcessPackageType_SingleFileOrDir(ABConfigInfo info, bool forceCheckPath)
        {
            if (info.IsPlatformManifest || File.Exists(info.Path) || Directory.Exists(info.Path))
            {
                AssetImporter importer = AssetImporter.GetAtPath(info.Path);
                if (importer == null) return;

                if (!string.IsNullOrEmpty(info.Rename))
                {
                    importer.assetBundleName = info.Rename.ToLower();
                }
                else
                {
                    int suffixIndex = info.Path.LastIndexOf('.');
                    suffixIndex = suffixIndex < 0 ? info.Path.Length : suffixIndex;
                    string bundleName = $"{info.Path.Substring(0, suffixIndex).Replace('/', '@').ToLower()}.bundle";
                    importer.assetBundleName = bundleName;
                }

                CacheABInfo(importer.assetBundleName, info);
            }
            else
            {
                if (forceCheckPath)
                    Log.Error($"[AB打包] ID:{info.ID} 路径无效：{info.Path}");
            }
        }

        /// <summary>
        /// 类型1：目录下每个文件单独打包
        /// </summary>
        private static void ProcessPackageType_EachFile(ABConfigInfo info, bool forceCheckPath)
        {
            if (!Directory.Exists(info.Path))
            {
                if (forceCheckPath) Log.Error($"[AB打包] ID:{info.ID} 目录无效：{info.Path}");
                return;
            }

            string root = Path.Combine(Application.dataPath[..^6], info.Path).Replace('\\', '/');
            string[] files = Directory.GetFiles(root);

            foreach (string fullPath in files)
            {
                if (fullPath.EndsWith(".meta")) continue;

                string assetPath = "Assets/" + fullPath[(Application.dataPath.Length + 1)..];
                AssetImporter importer = AssetImporter.GetAtPath(assetPath);
                if (importer == null) continue;

                int suffixIndex = assetPath.LastIndexOf('.');
                suffixIndex = suffixIndex < 0 ? assetPath.Length : suffixIndex;
                string bundleName = $"{assetPath.Substring(0, suffixIndex).Replace('/', '@').ToLower()}.bundle";
                importer.assetBundleName = bundleName;

                CacheABInfo(bundleName, info);
            }
        }

        /// <summary>
        /// 类型2：一级子目录单独打包
        /// </summary>
        private static void ProcessPackageType_EachSubDir(ABConfigInfo info, bool forceCheckPath)
        {
            if (!Directory.Exists(info.Path))
            {
                if (forceCheckPath) Log.Error($"[AB打包] ID:{info.ID} 目录无效：{info.Path}");
                return;
            }

            string root = Path.Combine(Application.dataPath[..^6], info.Path).Replace('\\', '/');
            string[] dirs = Directory.GetDirectories(root);

            foreach (string fullPath in dirs)
            {
                string assetPath = "Assets/" + fullPath[(Application.dataPath.Length + 1)..];
                AssetImporter importer = AssetImporter.GetAtPath(assetPath);
                if (importer == null) continue;

                string bundleName = $"{assetPath.Replace('/', '@').ToLower()}.bundle";
                importer.assetBundleName = bundleName;

                CacheABInfo(bundleName, info);
            }
        }

        /// <summary>
        /// 类型3：一级文件 + 子目录分别打包
        /// </summary>
        private static void ProcessPackageType_FileAndSubDir(ABConfigInfo info, bool forceCheckPath)
        {
            if (!Directory.Exists(info.Path))
            {
                if (forceCheckPath) Log.Error($"[AB打包] ID:{info.ID} 目录无效：{info.Path}");
                return;
            }

            string root = Path.Combine(Application.dataPath[..^6], info.Path).Replace('\\', '/');

            // 文件
            foreach (string fullPath in Directory.GetFiles(root))
            {
                if (fullPath.EndsWith(".meta")) continue;
                string assetPath = "Assets/" + fullPath[(Application.dataPath.Length + 1)..];
                AssetImporter importer = AssetImporter.GetAtPath(assetPath);
                if (importer == null) continue;

                int suffixIndex = assetPath.LastIndexOf('.');
                suffixIndex = suffixIndex < 0 ? assetPath.Length : suffixIndex;
                string bundleName = $"{assetPath.Substring(0, suffixIndex).Replace('/', '@').ToLower()}.bundle";
                importer.assetBundleName = bundleName;
                CacheABInfo(bundleName, info);
            }

            // 目录
            foreach (string fullPath in Directory.GetDirectories(root))
            {
                string assetPath = "Assets/" + fullPath[(Application.dataPath.Length + 1)..];
                AssetImporter importer = AssetImporter.GetAtPath(assetPath);
                if (importer == null) continue;

                string bundleName = $"{assetPath.Replace('/', '@').ToLower()}.bundle";
                importer.assetBundleName = bundleName;
                CacheABInfo(bundleName, info);
            }
        }

        /// <summary>
        /// 类型4：小游戏专用打包规则（Lua/Atlas/Textures 特殊处理）
        /// </summary>
        private static void ProcessPackageType_MiniGame(ABConfigInfo info, bool forceCheckPath)
        {
            if (!Directory.Exists(info.Path))
            {
                if (forceCheckPath) Log.Error($"[AB打包] ID:{info.ID} 目录无效：{info.Path}");
                return;
            }

            string root = Path.Combine(Application.dataPath[..^6], info.Path).Replace('\\', '/');
            RemoveDirectoryMetaFileAssetBundleName(root, true);

            string bundleName = $"{info.Path.Replace('/', '@').ToLower()}.bundle";
            string[] subDirs = Directory.GetDirectories(root);

            foreach (string dir in subDirs)
            {
                string assetPath = "Assets/" + dir[(Application.dataPath.Length + 1)..];
                string dirName = Path.GetFileNameWithoutExtension(assetPath);

                // 跳过 Lua
                if (dirName == "LuaScripts") continue;

                // 公用目录
                if (dirName == "Common")
                {
                    foreach (string commonDir in Directory.GetDirectories(dir))
                    {
                        string commonAssetPath = "Assets/" + commonDir[(Application.dataPath.Length + 1)..];
                        string commonDirName = Path.GetFileNameWithoutExtension(commonAssetPath);
                        if (commonDirName == "LuaScripts") continue;

                        AssetImporter importer = AssetImporter.GetAtPath(commonAssetPath);
                        if (importer != null) importer.assetBundleName = bundleName;
                    }
                    continue;
                }

                // 贴图目录（跳过 Atlas）
                if (dirName == "Textures")
                {
                    foreach (string texDir in Directory.GetDirectories(dir))
                    {
                        string texAssetPath = "Assets/" + texDir[(Application.dataPath.Length + 1)..];
                        string texDirName = Path.GetFileNameWithoutExtension(texAssetPath);
                        if (texDirName == "Atlas") continue;

                        AssetImporter importer = AssetImporter.GetAtPath(texAssetPath);
                        if (importer != null) importer.assetBundleName = bundleName;
                    }
                    continue;
                }

                // 普通目录
                AssetImporter imp = AssetImporter.GetAtPath(assetPath);
                if (imp != null) imp.assetBundleName = bundleName;
            }

            CacheABInfo(bundleName, info);
        }

        /// <summary>
        /// 缓存 AB 包的增量/公用/分组信息
        /// </summary>
        private static void CacheABInfo(string bundleName, ABConfigInfo info)
        {
            if (!s_ABIsIncreaser.ContainsKey(bundleName))
                s_ABIsIncreaser.Add(bundleName, info.IsIncreaserGroup);

            if (!s_ABIsCommonIncreaser.ContainsKey(bundleName))
                s_ABIsCommonIncreaser.Add(bundleName, info.IsCommonIncreaserGroup);

            if (!s_ABGroupName.ContainsKey(bundleName))
                s_ABGroupName.Add(bundleName, info.GroupName);
        }
        #endregion

        #region 外部查询接口
        /// <summary>
        /// 判断 AB 是否为增量包
        /// </summary>
        public static bool IsIncreaserAssetBundle(string assetBundleName)
        {
            if (string.IsNullOrEmpty(assetBundleName)) return false;
            if (s_ABIsIncreaser.Count == 0) RefreshAllAssetBundleNames();
            return s_ABIsIncreaser.TryGetValue(assetBundleName, out bool res) && res;
        }

        /// <summary>
        /// 判断 AB 是否为公用增量包
        /// </summary>
        public static bool IsCommonIncreaserAssetBundle(string assetBundleName)
        {
            if (string.IsNullOrEmpty(assetBundleName)) return false;
            if (s_ABIsCommonIncreaser.Count == 0) RefreshAllAssetBundleNames();
            return s_ABIsCommonIncreaser.TryGetValue(assetBundleName, out bool res) && res;
        }

        /// <summary>
        /// 判断两个 AB 是否属于同一分组
        /// </summary>
        public static bool IsAssetBundleInSameGroup(string assetBundleName1, string assetBundleName2)
        {
            if (string.IsNullOrEmpty(assetBundleName1) || string.IsNullOrEmpty(assetBundleName2))
                return false;

            if (s_ABGroupName.Count == 0)
                RefreshAllAssetBundleNames();

            s_ABGroupName.TryGetValue(assetBundleName1, out string g1);
            s_ABGroupName.TryGetValue(assetBundleName2, out string g2);
            return g1 == g2;
        }
        #endregion

        #region Meta 文件清理
        /// <summary>
        /// 清除单个 meta 文件中的 assetBundleName
        /// </summary>
        private static void RemoveMetaFileAssetBundleName(string metaPath)
        {
            if (!File.Exists(metaPath)) return;

            string content = File.ReadAllText(metaPath);
            string key = "assetBundleName:";
            int start = content.IndexOf(key);

            if (start < 0) return;

            int end = content.IndexOf('\n', start);
            if (end - start - key.Length > 2)
            {
                content = content.Remove(start + key.Length, end - start - key.Length);
                File.WriteAllText(metaPath, content);
            }
        }

        /// <summary>
        /// 清除目录下所有 meta 的 AB 名称
        /// </summary>
        private static void RemoveDirectoryMetaFileAssetBundleName(string directoryPath, bool isIncludeSelf = false)
        {
            string[] metas = Directory.GetFiles(directoryPath, "*.meta", SearchOption.AllDirectories);
            foreach (string meta in metas)
                RemoveMetaFileAssetBundleName(meta);

            if (isIncludeSelf)
                RemoveMetaFileAssetBundleName(directoryPath + ".meta");
        }
        #endregion
    }
}