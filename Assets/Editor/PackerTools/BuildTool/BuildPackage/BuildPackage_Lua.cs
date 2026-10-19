/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Editor
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  BuildPackage_Lua.cs
 * author:    云毅
 * created:   2026
 * descrip:   AssetBundle 打包工具 - Lua 脚本资源收集与（可选）加密
 *            收集 Assets/LuaScripts 下脚本/配置，按"每文件一个 bundle"
 *            生成 bundle 名 -> 资产路径 映射，供 BuildLuaOnly / BuildAllWithExtraBundles 使用。
 *            bundle 命名与运行时 LuaComponent.InitAssetBundleLuaPaths 完全对称：
 *            运行时用 GetABFormatPath("Assets/LuaScripts") 前缀匹配 manifest bundle 名，
 *            再用 GetABRestoredPath 还原加载地址，故 lua bundle 名必须基于
 *            "Assets/LuaScripts 全路径（仅去掉 .txt）"。
 ***************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Honor.Editor.BuildPackage
{
    //=========================================================================
    // Lua 资源收集器
    // 负责扫描 Assets/LuaScripts、计算 lua bundle 名并分组
    //=========================================================================
    /// <summary>
    /// Lua 脚本资源收集器
    /// 说明：递归收集 Assets/LuaScripts 下 *.lua.txt / *.json / *.bytes（排除 .meta），
    ///       按文件分组生成"bundle 名 -> 资产路径"映射
    /// </summary>
    public static class BuildPackage_Lua
    {
        #region 常量
        /// <summary>
        /// Lua 脚本根目录（相对 Assets）
        /// </summary>
        public const string LuaScriptsRoot = "Assets/LuaScripts";

        /// <summary>
        /// 配置类资源根目录（相对 Assets）
        /// 说明：运行时 Config/Localization 均以"目录级 abPath"加载配置
        /// （如 ConfigComponent.LoadConfigs 用 Assets/LuaScripts/Config/LuaJson + "Configs"），
        /// 故该目录下的 .json/.bytes 需打包为"目录粒度" bundle（bundle 名=目录路径格式化），
        /// 而非每文件粒度，才能命中运行时的 IsABExist(目录)。
        /// </summary>
        public const string ConfigRoot = "Assets/LuaScripts/Config/";
        #endregion

        #region Lua Bundle 映射收集
        /// <summary>
        /// 递归收集 Assets/LuaScripts 下全部 lua/配置资源，生成 bundle 分组
        /// 说明：
        /// <list type="bullet">
        /// <item>收录后缀：*.lua.txt（编辑模式 Lua 脚本）、*.json、*.bytes；排除 .meta；</item>
        /// <item>lua 脚本每文件一个 bundle：bundle 名 = FormatBundleName(GetLuaAddress(资产路径))；</item>
        /// <item>Config 目录下的 .json/.bytes 按"目录粒度"打包为一个 bundle
        ///       （bundle 名 = FormatBundleName(目录路径)），以匹配运行时以目录 abPath 加载配置的约定；</item>
        /// <item>GetLuaAddress：以 ".txt" 结尾则仅去掉 ".txt"（保留 ".lua"），
        ///       如 Assets/LuaScripts/Game/GameMain.lua.txt → Assets/LuaScripts/Game/GameMain.lua；
        ///       否则去掉最后一个扩展名。</item>
        /// </list>
        /// 命名依据：运行时 LuaComponent.InitAssetBundleLuaPaths 用
        /// GetABFormatPath("Assets/LuaScripts") 前缀匹配 manifest 中的 bundle 名，
        /// 再用 GetABRestoredPath 还原出加载地址。因此 lua bundle 名必须基于
        /// "Assets/LuaScripts 全路径（只去 .txt）"，不能用相对 LuaScripts 的短路径。
        /// </summary>
        /// <returns>bundle 名 -> 资产路径列表</returns>
        public static Dictionary<string, List<string>> CollectLuaBundleMap()
        {
            Dictionary<string, List<string>> result = new Dictionary<string, List<string>>();

            if (!Directory.Exists(LuaScriptsRoot))
            {
                Debug.LogWarning($"[AB打包] Lua 脚本目录不存在：{LuaScriptsRoot}");
                return result;
            }

            // 复用既有收集器（递归、排除 .meta）
            List<string> allFiles = BuildPackage_AssetMap.CollectAssetFiles(LuaScriptsRoot);
            foreach (string assetPath in allFiles)
            {
                string normalized = assetPath.Replace('\\', '/');
                if (!IsCollectableLuaAsset(normalized))
                    continue;

                // 计算 bundle 名：
                // - Config 目录下的 .json/.bytes：按"目录粒度"打包（bundle 名=目录路径格式化），
                //   匹配运行时 Config/Localization 以目录 abPath 加载的约定；
                // - 其余 lua/资源：按"每文件"粒度（bundle 名=去扩展名后的完整路径格式化）。
                string bundleName;
                if (IsConfigAsset(normalized))
                {
                    string dir = normalized.Substring(0, normalized.LastIndexOf('/'));
                    bundleName = BuildPackage_AssetMap.FormatBundleName(dir);
                }
                else
                {
                    string address = GetLuaAddress(normalized);
                    bundleName = BuildPackage_AssetMap.FormatBundleName(address);
                }

                if (!result.TryGetValue(bundleName, out List<string> assets))
                {
                    assets = new List<string>();
                    result.Add(bundleName, assets);
                }

                if (!assets.Contains(normalized))
                    assets.Add(normalized);
            }

            Debug.Log($"[AB打包] 收集 lua bundle 数：{result.Count}（目录 {LuaScriptsRoot}）");
            return result;
        }

        /// <summary>
        /// 判断是否为配置类资源（*.json / *.bytes，且位于 Config 根目录下）
        /// 说明：这类资源运行时按"目录 abPath"加载，需打包为目录粒度 bundle
        /// </summary>
        /// <param name="assetPath">资源相对路径（Assets 开头，正斜杠）</param>
        /// <returns>是否为配置类资源</returns>
        public static bool IsConfigAsset(string assetPath)
        {
            if (!assetPath.StartsWith(ConfigRoot, StringComparison.OrdinalIgnoreCase))
                return false;

            string ext = Path.GetExtension(assetPath);
            return ext.Equals(".json", StringComparison.OrdinalIgnoreCase)
                || ext.Equals(".bytes", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 判断是否为可打包的 lua/配置资源（*.lua.txt / *.json / *.bytes）
        /// </summary>
        /// <param name="assetPath">资源相对路径（Assets 开头，正斜杠）</param>
        /// <returns>是否收录</returns>
        private static bool IsCollectableLuaAsset(string assetPath)
        {
            if (assetPath.EndsWith(".lua.txt", StringComparison.OrdinalIgnoreCase))
                return true;

            string ext = Path.GetExtension(assetPath);
            return ext.Equals(".json", StringComparison.OrdinalIgnoreCase)
                || ext.Equals(".bytes", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 计算 lua 资源对应的"逻辑地址"（用于 bundle 命名）
        /// 规则：以 ".txt" 结尾则仅去掉 ".txt"（保留 ".lua"，
        /// 如 Assets/LuaScripts/Game/GameMain.lua.txt → Assets/LuaScripts/Game/GameMain.lua）；
        /// 否则去掉最后一个扩展名（如 x.json → x）。
        /// </summary>
        /// <param name="assetPath">资产相对路径（Assets 开头）</param>
        /// <returns>逻辑地址（正斜杠）</returns>
        public static string GetLuaAddress(string assetPath)
        {
            string normalized = (assetPath ?? string.Empty).Replace('\\', '/');

            if (normalized.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
                return normalized.Substring(0, normalized.Length - ".txt".Length);

            return Path.ChangeExtension(normalized, null);
        }

        /// <summary>
        /// 计算 lua 资源在 respathtab.txt 中的 key
        /// 规则：去掉 "Assets/" 前缀、去掉尾部 ".txt"
        /// （如 Assets/LuaScripts/Game/GameMain.lua.txt → LuaScripts/Game/GameMain.lua）
        /// </summary>
        /// <param name="assetPath">资产相对路径（Assets 开头）</param>
        /// <returns>respathtab key</returns>
        public static string GetLuaResPathTabKey(string assetPath)
        {
            string normalized = (assetPath ?? string.Empty).Replace('\\', '/');

            const string assetsPrefix = "Assets/";
            if (normalized.StartsWith(assetsPrefix, StringComparison.OrdinalIgnoreCase))
                normalized = normalized.Substring(assetsPrefix.Length);

            if (normalized.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
                normalized = normalized.Substring(0, normalized.Length - ".txt".Length);

            return normalized;
        }
        #endregion
    }

    //=========================================================================
    // Lua 脚本 XOR 混淆（加密可选预留）
    //=========================================================================
    /// <summary>
    /// Lua 脚本 XOR 加解密工具
    /// 说明：此工具为"加密可选"预留，默认不接入打包流水线；
    ///       运行时需对应 LuacMode / 解密逻辑方可消费，
    ///       生产开启前务必与运行时约定一致（key、是否逐字节循环异或、输出格式）。
    /// </summary>
    public static class LuaEncrypt
    {
        #region 密钥与算法
        /// <summary>
        /// XOR 混淆密钥（byte 序列，对称加解密）
        /// </summary>
        public static readonly byte[] s_XorKey = { 0x6f, 0x72, 0x6e, 0x6f, 0x72 };

        /// <summary>
        /// 字节数组 XOR 混淆（加密，与解密同一函数，对称）
        /// </summary>
        /// <param name="data">原始字节（不修改入参，返回新数组）</param>
        /// <returns>混淆后字节副本</returns>
        public static byte[] EncryptBytes(byte[] data)
        {
            if (data == null || data.Length == 0)
                return data ?? Array.Empty<byte>();

            byte[] result = (byte[])data.Clone();
            int keyLen = s_XorKey.Length;
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = (byte)(result[i] ^ s_XorKey[i % keyLen]);
            }

            return result;
        }

        /// <summary>
        /// 字节数组 XOR 解密（与加密同一函数，对称）
        /// </summary>
        /// <param name="data">混淆字节（不修改入参，返回新数组）</param>
        /// <returns>还原后字节副本</returns>
        public static byte[] DecryptBytes(byte[] data)
        {
            // XOR 对称：解密 == 加密
            return EncryptBytes(data);
        }

        /// <summary>
        /// 将 lua 源文件做 XOR 混淆后写出为 .bytes 副本
        /// 注意：当前为"加密可选"预留，默认不接入打包流水线；
        ///       开启前务必与运行时 LuacMode / 解密逻辑保持一致。
        /// </summary>
        /// <param name="srcLuaPath">lua 源文件绝对路径</param>
        /// <param name="dstBytesPath">输出 .bytes 文件绝对路径</param>
        public static void EncryptFileToBytes(string srcLuaPath, string dstBytesPath)
        {
            if (!File.Exists(srcLuaPath))
            {
                Debug.LogWarning($"[AB打包] Lua 加密：源文件不存在 {srcLuaPath}");
                return;
            }

            byte[] raw = File.ReadAllBytes(srcLuaPath);
            byte[] encrypted = EncryptBytes(raw);

            string dir = Path.GetDirectoryName(dstBytesPath);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            File.WriteAllBytes(dstBytesPath, encrypted);
            Debug.Log($"[AB打包] Lua 加密副本已写出：{dstBytesPath}");
        }
        #endregion
    }
}
