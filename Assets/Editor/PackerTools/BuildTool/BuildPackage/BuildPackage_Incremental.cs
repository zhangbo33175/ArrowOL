/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Editor
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  BuildPackage_Incremental.cs
 * author:    云毅
 * created:   2026
 * descrip:   AssetBundle 打包工具 - 增量打包缓存
 *            记录每个 bundle 的内容签名；打包前比对，若 bundle 内容未变化则跳过整次打包
 *            签名基于资源导入哈希（AssetDatabase.GetAssetDependencyHash），随资源变更/重导入自动失效
 ***************************************************************/

using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Honor.Editor.BuildPackage
{
    //=========================================================================
    // 增量打包缓存
    // 提供"内容签名计算 / 变更判定 / 签名缓存读写"
    //=========================================================================
    /// <summary>
    /// 增量打包缓存
    /// 说明：每个 bundle 维护一个内容签名；比对当前签名与缓存签名，
    ///       全部未变更则跳过打包，发生变更则触发（全量）重打以保证 manifest 完整
    /// </summary>
    public static class BuildPackage_Incremental
    {
        #region 缓存路径
        /// <summary>
        /// 增量签名缓存文件绝对路径（存放于 Library，不入版本控制、不参与构建）
        /// </summary>
        public static string CacheFilePath => $"{Path.GetDirectoryName(Application.dataPath)}/Library/BuildPackage_Incremental.json";
        #endregion

        #region 签名计算
        /// <summary>
        /// 计算单个 bundle 的内容签名
        /// 规则：拼接"资源路径:导入哈希"的有序集合，再做确定性 MD5 取 hex
        /// </summary>
        /// <param name="assetPaths">bundle 内资源路径列表</param>
        /// <returns>签名 hex 字符串</returns>
        public static string ComputeBundleSignature(List<string> assetPaths)
        {
            if (assetPaths == null || assetPaths.Count == 0)
                return "empty";

            // 排序保证签名与资源收集顺序无关
            List<string> components = new List<string>(assetPaths.Count);
            foreach (string path in assetPaths)
            {
                Hash128 hash = AssetDatabase.GetAssetDependencyHash(path);
                components.Add($"{path}:{hash}");
            }

            components.Sort(System.StringComparer.Ordinal);

            return ComputeMd5(string.Join("|", components));
        }

        /// <summary>
        /// 计算字符串的 MD5 hex（确定性签名）
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <returns>32 位小写 hex</returns>
        private static string ComputeMd5(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder sb = new StringBuilder(data.Length * 2);
                foreach (byte b in data)
                    sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }
        #endregion

        #region 变更判定
        /// <summary>
        /// 判断是否存在需要重打的 bundle（与缓存签名比对）
        /// </summary>
        /// <param name="bundleMap">当前 Bundle 分组映射</param>
        /// <returns>true=有变更需要重打；false=全部未变更</returns>
        public static bool HasChanges(Dictionary<string, List<string>> bundleMap)
        {
            Dictionary<string, string> cache = LoadCache();

            foreach (KeyValuePair<string, List<string>> kv in bundleMap)
            {
                string current = ComputeBundleSignature(kv.Value);
                if (!cache.TryGetValue(kv.Key, out string old) || !string.Equals(old, current, System.StringComparison.Ordinal))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 保存当前 bundle 签名到缓存（打包成功后调用）
        /// </summary>
        /// <param name="bundleMap">当前 Bundle 分组映射</param>
        public static void SaveSignatures(Dictionary<string, List<string>> bundleMap)
        {
            Dictionary<string, string> cache = LoadCache();

            foreach (KeyValuePair<string, List<string>> kv in bundleMap)
            {
                cache[kv.Key] = ComputeBundleSignature(kv.Value);
            }

            WriteCache(cache);
        }
        #endregion

        #region 缓存读写
        /// <summary>
        /// 加载签名缓存
        /// </summary>
        /// <returns>签名字典（不存在或损坏返回空字典）</returns>
        private static Dictionary<string, string> LoadCache()
        {
            Dictionary<string, string> cache = new Dictionary<string, string>();
            if (!File.Exists(CacheFilePath))
                return cache;

            try
            {
                string json = File.ReadAllText(CacheFilePath, Encoding.UTF8);
                CacheData data = JsonUtility.FromJson<CacheData>(json);
                if (data == null || data.Entries == null)
                    return cache;

                foreach (CacheEntry entry in data.Entries)
                {
                    if (entry != null && !string.IsNullOrEmpty(entry.BundleName))
                        cache[entry.BundleName] = entry.Signature;
                }
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"[AB打包] 增量缓存读取失败，将全量打包。\n{e}");
            }

            return cache;
        }

        /// <summary>
        /// 写入签名缓存
        /// </summary>
        /// <param name="cache">签名字典</param>
        private static void WriteCache(Dictionary<string, string> cache)
        {
            CacheData data = new CacheData();
            data.Entries = new List<CacheEntry>();
            foreach (KeyValuePair<string, string> kv in cache)
            {
                data.Entries.Add(new CacheEntry { BundleName = kv.Key, Signature = kv.Value });
            }

            try
            {
                string dir = Path.GetDirectoryName(CacheFilePath);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                File.WriteAllText(CacheFilePath, JsonUtility.ToJson(data, true), Encoding.UTF8);
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"[AB打包] 增量缓存写入失败：\n{e}");
            }
        }
        #endregion
    }

    #region 缓存数据模型
    /// <summary>
    /// 增量缓存数据（JsonUtility 序列化）
    /// </summary>
    [System.Serializable]
    public class CacheData
    {
        /// <summary>签名条目列表</summary>
        public List<CacheEntry> Entries = new List<CacheEntry>();
    }

    /// <summary>
    /// 增量缓存条目
    /// </summary>
    [System.Serializable]
    public class CacheEntry
    {
        /// <summary>bundle 名</summary>
        public string BundleName = string.Empty;

        /// <summary>内容签名</summary>
        public string Signature = string.Empty;
    }
    #endregion
}
