/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Editor
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  BuildPackage_Builder.cs
 * author:    云毅
 * created:   2026
 * descrip:   AssetBundle 打包工具 - 打包执行器
 *            将 Bundle 分组映射转为 AssetBundleBuild 列表，
 *            调用 BuildPipeline 打包到 StreamingAssets，并生成资源映射文件。
 *            扩展（指定平台打包）：
 *            1) BuildAll 增加 BuildTarget 重载，按目标平台输出到
 *               StreamingAssets/AssetBundles/{平台}；
 *            2) BuildPipeline 生成的清单文件（输出目录末级文件夹名，无扩展名）
 *               打包后改名为运行时要求的 {平台名}.bundle；
 *            3) 支持 res+lua 合并单次打包（单一 manifest）、lua 单独打包（追加 respathtab）。
 ***************************************************************/

using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Honor.Editor.BuildPackage
{
    //=========================================================================
    // 打包执行器
    // 负责 BuildMap -> AssetBundleBuild[] -> BuildPipeline 打包 -> 资源映射导出
    //=========================================================================
    /// <summary>
    /// 打包执行器
    /// 说明：将 <see cref="BuildPackage_AssetMap.BundleMap"/> 转为可执行的 AssetBundleBuild 列表，
    ///       调用 BuildPipeline.BuildAssetBundles 输出到 StreamingAssets，并生成资源映射文件。
    ///       同时提供按指定平台（<see cref="BuildTarget"/>）打包的扩展重载。
    /// </summary>
    public static class BuildPackage_Builder
    {
        #region 一键打包（活动平台，兼容旧窗口/菜单）
        /// <summary>
        /// 一键打包 AB（兼容旧入口：跟随当前编辑器活动构建目标）
        /// 说明：保留原公开签名，内部委托到 <see cref="BuildAll(BuildTarget, List{BuildPackageConfig}, bool)"/>，
        ///       行为与历史版本一致，旧窗口/菜单调用点无需修改。
        /// </summary>
        /// <param name="configs">打包配置列表（为空则用默认配置）</param>
        /// <param name="incremental">是否启用增量打包（无内容变更则跳过打包）</param>
        /// <returns>打包是否成功</returns>
        public static bool BuildAll(List<BuildPackageConfig> configs = null, bool incremental = false)
        {
            return BuildAll(BuildPackage_Define.CurrentBuildTarget, configs, incremental);
        }
        #endregion

        #region 一键打包（指定平台）
        /// <summary>
        /// 一键打包 AB 到指定平台输出目录
        /// 流程：BuildMap -> 增量判定 -> 生成 AssetBundleBuild[] -> BuildPipeline 打包
        ///       -> 改名 manifest 为 {平台名}.bundle -> 保存增量签名 -> 生成 respathtab
        /// </summary>
        /// <param name="target">目标构建平台（决定 StreamingAssets/AssetBundles/{平台} 输出目录）</param>
        /// <param name="configs">打包配置列表（为空则用 <see cref="BuildPackage_DefaultConfigs.GetDefaults"/>）</param>
        /// <param name="incremental">是否启用增量打包（无内容变更则跳过打包）</param>
        /// <returns>打包是否成功</returns>
        public static bool BuildAll(BuildTarget target, List<BuildPackageConfig> configs = null, bool incremental = false)
        {
            if (configs == null || configs.Count == 0)
                configs = BuildPackage_DefaultConfigs.GetDefaults();

            // 1. 构建 Bundle 分组映射（填充静态 BundleMap）
            int groupCount = BuildPackage_AssetMap.BuildMap(configs);
            if (groupCount == 0)
            {
                Debug.LogError("[AB打包] 未收集到任何资源，打包终止。");
                return false;
            }

            // 2. 增量判定：无变更则跳过打包
            if (incremental && !BuildPackage_Incremental.HasChanges(BuildPackage_AssetMap.BundleMap))
            {
                Debug.Log("[AB打包] 增量：所有 bundle 均无内容变更，跳过打包。");
                return true;
            }

            // 3. 转为 AssetBundleBuild 列表并打包
            AssetBundleBuild[] builds = BuildAssetBundleBuilds();
            if (builds == null || builds.Length == 0)
            {
                Debug.LogError("[AB打包] AssetBundleBuild 列表为空，打包终止。");
                return false;
            }

            string outRoot = BuildPackage_Define.GetBuildOutputFullRoot(target);
            if (!BuildAssetBundles(outRoot, target, builds))
                return false;

            // 4. 打包成功后更新增量签名缓存
            BuildPackage_Incremental.SaveSignatures(BuildPackage_AssetMap.BundleMap);

            // 5. 生成资源映射文件
            WriteResMapping(outRoot, BuildPackage_AssetMap.BundleMap);

            AssetDatabase.Refresh();
            Debug.Log($"[AB打包] 打包完成，共 {groupCount} 个 bundle，输出目录：{outRoot}");
            return true;
        }

        /// <summary>
        /// 一次 BuildAssetBundles 同时打包 res 与 lua（合并分组）
        /// 说明：res 由 <paramref name="configs"/> 经 BuildMap 收集；lua 等额外分组
        ///       <paramref name="extraBundles"/> 合并进同一打包列表，保证只产出单一 manifest，
        ///       运行时 LuaComponent 才能在同一份 manifest 中扫到 lua bundle。
        ///       bundle 名冲突时合并资产列表（去重）。
        /// </summary>
        /// <param name="target">目标构建平台</param>
        /// <param name="configs">res 打包配置列表（为空则用默认配置）</param>
        /// <param name="extraBundles">额外分组（如 lua）：bundle 名 -> 资产路径列表</param>
        /// <param name="incremental">是否启用增量打包（按合并后的完整映射判定/保存）</param>
        /// <returns>打包是否成功</returns>
        public static bool BuildAllWithExtraBundles(BuildTarget target, List<BuildPackageConfig> configs, Dictionary<string, List<string>> extraBundles, bool incremental)
        {
            if (configs == null || configs.Count == 0)
                configs = BuildPackage_DefaultConfigs.GetDefaults();

            // 1. res 映射（填充静态 BundleMap）
            BuildPackage_AssetMap.BuildMap(configs);

            // 2. 合并 res + extraBundles 到本地 merged 字典（bundle 名冲突时合并资产列表）
            Dictionary<string, List<string>> merged = new Dictionary<string, List<string>>();
            foreach (KeyValuePair<string, List<string>> kv in BuildPackage_AssetMap.BundleMap)
            {
                merged[kv.Key] = new List<string>(kv.Value);
            }

            if (extraBundles != null)
            {
                foreach (KeyValuePair<string, List<string>> kv in extraBundles)
                {
                    if (string.IsNullOrEmpty(kv.Key) || kv.Value == null || kv.Value.Count == 0)
                        continue;

                    if (!merged.TryGetValue(kv.Key, out List<string> assets))
                    {
                        assets = new List<string>();
                        merged[kv.Key] = assets;
                    }

                    foreach (string asset in kv.Value)
                    {
                        if (!assets.Contains(asset))
                            assets.Add(asset);
                    }
                }
            }

            if (merged.Count == 0)
            {
                Debug.LogError("[AB打包] res+lua 合并后未收集到任何资源，打包终止。");
                return false;
            }

            // 3. 增量判定（按合并后的完整映射）
            if (incremental && !BuildPackage_Incremental.HasChanges(merged))
            {
                Debug.Log("[AB打包] 增量：res+lua 所有 bundle 均无内容变更，跳过打包。");
                return true;
            }

            // 4. 一次性 BuildAssetBundles（单一 manifest 同时含 res+lua）
            AssetBundleBuild[] builds = ToAssetBundleBuilds(merged);
            string outRoot = BuildPackage_Define.GetBuildOutputFullRoot(target);
            if (!BuildAssetBundles(outRoot, target, builds))
                return false;

            // 5. 增量签名与资源映射均按 merged 写出
            BuildPackage_Incremental.SaveSignatures(merged);
            WriteResMapping(outRoot, merged);

            AssetDatabase.Refresh();
            Debug.Log($"[AB打包] res+lua 合并打包完成，共 {merged.Count} 个 bundle，输出目录：{outRoot}");
            return true;
        }

        /// <summary>
        /// 仅打包 lua（或其他纯脚本分组）到指定平台输出根
        /// 说明：不清理输出目录——保留此前 PackageRes 已产出的 res bundle 与 respathtab，
        ///       仅新增/覆盖 lua bundle，并将 lua 条目追加进 respathtab。
        ///       注意：BuildPipeline 会重写 manifest 为本次分组，若需 res+lua 同一份 manifest，
        ///       应使用 <see cref="BuildAllWithExtraBundles"/>。
        /// </summary>
        /// <param name="target">目标构建平台</param>
        /// <param name="luaMap">lua 分组：bundle 名 -> 资产路径列表</param>
        /// <param name="incremental">是否启用增量打包（按 luaMap 判定/保存）</param>
        /// <returns>打包是否成功</returns>
        public static bool BuildLuaOnly(BuildTarget target, Dictionary<string, List<string>> luaMap, bool incremental)
        {
            if (luaMap == null || luaMap.Count == 0)
            {
                Debug.LogError("[AB打包] Lua 分组为空，Lua AB 打包终止。");
                return false;
            }

            // 1. 增量判定（仅按 luaMap）
            if (incremental && !BuildPackage_Incremental.HasChanges(luaMap))
            {
                Debug.Log("[AB打包] 增量：lua bundle 无内容变更，跳过 Lua AB 打包。");
                return true;
            }

            // 2. 生成 AssetBundleBuild 并打包（不清理输出目录，保留 res 产物）
            AssetBundleBuild[] builds = ToAssetBundleBuilds(luaMap);
            string outRoot = BuildPackage_Define.GetBuildOutputFullRoot(target);
            if (!BuildLuaAssetBundles(outRoot, target, builds))
                return false;

            // 3. 保存 lua 增量签名，并把 lua 条目追加进 respathtab
            BuildPackage_Incremental.SaveSignatures(luaMap);
            WriteResMapping(outRoot, luaMap);

            AssetDatabase.Refresh();
            Debug.Log($"[AB打包] Lua AB 打包完成，共 {luaMap.Count} 个 bundle，输出目录：{outRoot}");
            return true;
        }
        #endregion

        #region 输出目录清理
        /// <summary>
        /// 清理指定平台的 AB 输出根目录
        /// 说明：删除 StreamingAssets/AssetBundles/{平台} 整个目录后刷新 AssetDatabase
        /// </summary>
        /// <param name="target">目标构建平台</param>
        public static void ClearOutput(BuildTarget target)
        {
            string outRoot = BuildPackage_Define.GetBuildOutputFullRoot(target);
            if (Directory.Exists(outRoot))
            {
                Directory.Delete(outRoot, true);
                AssetDatabase.Refresh();
                Debug.Log($"[AB打包] 已清理输出目录：{outRoot}");
            }
            else
            {
                Debug.Log($"[AB打包] 输出目录不存在，无需清理：{outRoot}");
            }
        }
        #endregion

        #region BundleMap -> AssetBundleBuild[]
        /// <summary>
        /// 将静态 <see cref="BuildPackage_AssetMap.BundleMap"/> 转为 AssetBundleBuild 列表
        /// </summary>
        /// <returns>AssetBundleBuild 数组</returns>
        private static AssetBundleBuild[] BuildAssetBundleBuilds()
        {
            return ToAssetBundleBuilds(BuildPackage_AssetMap.BundleMap);
        }

        /// <summary>
        /// 将任意"bundle 名 -> 资产路径列表"映射转为 AssetBundleBuild 列表
        /// </summary>
        /// <param name="bundleMap">分组映射</param>
        /// <returns>AssetBundleBuild 数组</returns>
        private static AssetBundleBuild[] ToAssetBundleBuilds(Dictionary<string, List<string>> bundleMap)
        {
            List<AssetBundleBuild> list = new List<AssetBundleBuild>();
            foreach (KeyValuePair<string, List<string>> kv in bundleMap)
            {
                AssetBundleBuild build = new AssetBundleBuild
                {
                    assetBundleName = kv.Key,
                    assetNames = kv.Value.ToArray(),
                };
                list.Add(build);
            }

            return list.ToArray();
        }
        #endregion

        #region 执行 BuildPipeline
        /// <summary>
        /// 调用 BuildPipeline 打包 AssetBundle（清空重建输出目录）
        /// 流程：清空重建 outRoot -> BuildPipeline.BuildAssetBundles ->
        ///       将 BuildPipeline 生成的清单文件（输出目录末级文件夹名，无扩展名）
        ///       拷贝改名为运行时要求的 {平台名}.bundle。
        /// </summary>
        /// <param name="outRoot">输出根目录（StreamingAssets/AssetBundles/{平台}）</param>
        /// <param name="target">目标构建平台</param>
        /// <param name="builds">AssetBundleBuild 列表</param>
        /// <returns>是否成功</returns>
        private static bool BuildAssetBundles(string outRoot, BuildTarget target, AssetBundleBuild[] builds)
        {
            // 清空旧输出，避免残留过期 bundle
            if (Directory.Exists(outRoot))
                Directory.Delete(outRoot, true);
            Directory.CreateDirectory(outRoot);

            return InvokeBuildAndRenameManifest(outRoot, target, builds);
        }

        /// <summary>
        /// 调用 BuildPipeline 打包 AssetBundle（不清空输出目录）
        /// 说明：供 lua-only 打包使用——保留输出目录内已有的 res bundle 与 respathtab，
        ///       仅写入本次分组并刷新 manifest 清单文件。
        /// </summary>
        /// <param name="outRoot">输出根目录（StreamingAssets/AssetBundles/{平台}）</param>
        /// <param name="target">目标构建平台</param>
        /// <param name="builds">AssetBundleBuild 列表</param>
        /// <returns>是否成功</returns>
        private static bool BuildLuaAssetBundles(string outRoot, BuildTarget target, AssetBundleBuild[] builds)
        {
            if (!Directory.Exists(outRoot))
                Directory.CreateDirectory(outRoot);

            return InvokeBuildAndRenameManifest(outRoot, target, builds);
        }

        /// <summary>
        /// 执行 BuildPipeline.BuildAssetBundles 并完成运行时清单改名
        /// 说明：BuildPipeline 以输出目录末级文件夹名作为清单文件名（无扩展名），
        ///       运行时 LoadManifest 读取的是 {平台名}.bundle，故打包后必须拷贝改名。
        /// </summary>
        /// <param name="outRoot">输出根目录</param>
        /// <param name="target">目标构建平台</param>
        /// <param name="builds">AssetBundleBuild 列表</param>
        /// <returns>是否成功</returns>
        private static bool InvokeBuildAndRenameManifest(string outRoot, BuildTarget target, AssetBundleBuild[] builds)
        {
            EditorUtility.DisplayProgressBar("AB打包", "开始打包资源...", 0f);

            try
            {
                AssetBundleManifest manifest = BuildPipeline.BuildAssetBundles(
                    outRoot,
                    builds,
                    BuildPackage_Define.DefaultBuildOptions,
                    target);

                if (manifest == null)
                {
                    Debug.LogError("[AB打包] BuildPipeline.BuildAssetBundles 返回空 manifest，打包失败。");
                    return false;
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[AB打包] 打包异常：{e}");
                return false;
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }

            // BuildPipeline 生成的清单文件名为输出目录末级文件夹名（无扩展名），
            // 运行时要求的清单名为 {平台名}.bundle，这里拷贝改名（先删目标再拷）。
            string platformName = BuildPackage_Define.GetPlatformName(target);
            string manifestSrc = $"{outRoot}/{platformName}";
            string manifestDst = $"{outRoot}/{BuildPackage_Define.GetPlatformManifestBundleName(target)}";

            if (File.Exists(manifestSrc))
            {
                if (File.Exists(manifestDst))
                    File.Delete(manifestDst);
                File.Copy(manifestSrc, manifestDst);
                Debug.Log($"[AB打包] 清单已改名：{manifestSrc} -> {manifestDst}");
            }
            else
            {
                Debug.LogWarning($"[AB打包] 未找到 BuildPipeline 清单文件：{manifestSrc}（清单改名跳过）");
            }

            return true;
        }
        #endregion

        #region 资源映射导出
        /// <summary>
        /// 生成资源映射文件（respathtab.txt）
        /// 格式：key=bundle名=文件名（fileName 取资源完整文件名）
        /// key 规则：
        /// <list type="bullet">
        /// <item>Assets/Res/ 下资源：相对 Res 的路径（不含后缀），沿用既有语义；</item>
        /// <item>Assets/LuaScripts/ 下资源：形如 "LuaScripts/Game/GameMain.lua"
        ///       （去掉 "Assets/" 前缀、去掉尾部 ".txt"，保留 ".lua"）——
        ///       与运行时 LuaComponent 按 LuaScripts/ 前缀定位脚本的约定一致；</item>
        /// <item>其他路径跳过。</item>
        /// </list>
        /// 追加语义：若文件已存在（如 lua-only 打包，此前已由 res 打包生成 respathtab），
        /// 则追加写入，避免覆盖既有 res 条目。
        /// </summary>
        /// <param name="outRoot">输出根目录</param>
        /// <param name="bundleMap">分组映射：bundle 名 -> 资产路径列表</param>
        private static void WriteResMapping(string outRoot, Dictionary<string, List<string>> bundleMap)
        {
            string filePath = $"{outRoot}/{BuildPackage_Define.ResMappingFileName}";

            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<string, List<string>> kv in bundleMap)
            {
                string bundleName = kv.Key;
                foreach (string assetPath in kv.Value)
                {
                    string normalized = assetPath.Replace('\\', '/');
                    string key = null;

                    if (normalized.StartsWith(BuildPackage_Define.AssetResRoot + "/", System.StringComparison.OrdinalIgnoreCase))
                    {
                        // res 资源：沿用既有相对 Res（不含后缀）语义
                        key = BuildPackage_AssetMap.GetRelativePathWithoutExt(normalized);
                    }
                    else if (normalized.StartsWith("Assets/LuaScripts/", System.StringComparison.OrdinalIgnoreCase))
                    {
                        // lua 资源：key = "LuaScripts/" + 相对路径，去掉尾部 ".txt"
                        // 例：Assets/LuaScripts/Game/GameMain.lua.txt -> LuaScripts/Game/GameMain.lua
                        string rel = normalized.Substring("Assets/".Length);
                        if (rel.EndsWith(".txt", System.StringComparison.OrdinalIgnoreCase))
                            rel = rel.Substring(0, rel.Length - ".txt".Length);
                        key = rel;
                    }
                    else
                    {
                        // 非 res / 非 lua 路径不写映射
                        continue;
                    }

                    string fileName = Path.GetFileName(normalized);
                    sb.AppendLine($"{key}={bundleName}={fileName}");
                }
            }

            if (sb.Length == 0)
            {
                Debug.LogWarning($"[AB打包] 无可写入的资源映射条目，跳过：{filePath}");
                return;
            }

            if (File.Exists(filePath))
            {
                // 追加（供 lua-only 打包：保留此前 res 条目，追加 lua 条目）
                File.AppendAllText(filePath, sb.ToString(), Encoding.UTF8);
                Debug.Log($"[AB打包] 资源映射已追加：{filePath}");
            }
            else
            {
                File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
                Debug.Log($"[AB打包] 资源映射已生成：{filePath}");
            }
        }
        #endregion
    }
}
