/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Editor
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  BuildPlayer_Common.cs
 * author:    Honor.Editor
 * created:   2026
 * descrip:   玩家包打包公共工具
 *            - 输出路径计算（带时间戳，自动建目录）
 *            - PlayerSettings 应用（bundleVersion / bundleVersionCode /
 *              applicationIdentifier via NamedBuildTarget）
 *            - Android keystore 签名应用（文件缺失仅警告，不硬编码口令）
 *            - 场景列表收集（ctx -> Store -> 默认主场景）
 *            - 打包结果日志（batchmode 下不弹 RevealInFinder）
 ***************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

namespace Honor.Editor.BuildTool
{
    /// <summary>
    /// 玩家包打包公共静态工具类。
    /// 说明：Android / PC / iOS 三平台玩家包共用的设置与路径逻辑集中于此，
    /// 避免各平台重复实现；本类不触发资源打包，资源 AB 由并行任务输出到
    /// StreamingAssets/AssetBundles/{PlatformName}，打玩家包时直接包含。
    /// </summary>
    public static class BuildPlayer_Common
    {
        #region 输出路径
        /// <summary>
        /// 计算玩家包输出绝对路径。
        /// 规则：{PlayerOutPath}/{AppName}_{AppVersion}_{yyyyMMdd_HHmmss}{extension}
        /// 当 <paramref name="ctx"/>.PlayerOutPath 为空时，回退到
        /// BuildSettingsStore.OutRoot + "/{PlatformName}"。
        /// </summary>
        /// <param name="ctx">平台打包上下文</param>
        /// <param name="extension">文件后缀（".apk"/".exe"；iOS 传 "" 表示 Xcode 工程目录）</param>
        /// <returns>输出文件（或 iOS 工程目录）的绝对路径</returns>
        public static string GetOutPath(BuildPlatformContext ctx, string extension)
        {
            // 1. 确定根目录：优先使用上下文指定的玩家包输出目录
            string dir = ctx.PlayerOutPath;
            if (string.IsNullOrEmpty(dir))
            {
                dir = $"{BuildSettingsStore.OutRoot}/{ctx.PlatformName}";
            }

            // 2. 自动创建目录（CreateDirectory 已存在时不报错）
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            // 3. 拼接文件名：AppName_AppVersion_时间戳 + 后缀
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string fileName = $"{ctx.AppName}_{ctx.AppVersion}_{timestamp}{extension}";
            string combined = $"{dir}/{fileName}";

            // 4. 返回绝对路径（统一斜杠，便于日志与 RevealInFinder）
            string fullPath = Path.GetFullPath(combined);
            Debug.Log($"[BuildPlayer] 输出路径({ctx.PlatformName}): {fullPath}");
            return fullPath;
        }
        #endregion

        #region PlayerSettings 应用
        /// <summary>
        /// 将上下文中的版本号 / 包名 / Android versionCode 写入 PlayerSettings。
        /// 说明：
        /// - bundleVersion 为空时跳过，不清空工程已有版本；
        /// - applicationIdentifier 必须通过 NamedBuildTarget 写入，
        ///   以避免 Unity 2022.3 中已废弃的 PlayerSettings.applicationIdentifier
        ///   直接赋值带来的歧义与警告。
        /// </summary>
        /// <param name="ctx">平台打包上下文</param>
        public static void ApplyPlayerSettings(BuildPlatformContext ctx)
        {
            // --- bundleVersion（通用版本号） ---
            if (!string.IsNullOrEmpty(ctx.AppVersion))
            {
                PlayerSettings.bundleVersion = ctx.AppVersion;
                Debug.Log($"[BuildPlayer] bundleVersion = {ctx.AppVersion}");
            }
            else
            {
                Debug.Log("[BuildPlayer] AppVersion 为空，跳过 bundleVersion 设置");
            }

            // --- Android bundleVersionCode（仅 Android） ---
            if (ctx.Target == BuildTarget.Android)
            {
                PlayerSettings.Android.bundleVersionCode = ctx.BundleVersionCode;
                Debug.Log($"[BuildPlayer] Android.bundleVersionCode = {ctx.BundleVersionCode}");
            }

            // --- applicationIdentifier（按目标平台走 NamedBuildTarget） ---
            if (!string.IsNullOrEmpty(ctx.PackageName))
            {
                NamedBuildTarget nbt = ToNamedBuildTarget(ctx.Target);
                PlayerSettings.SetApplicationIdentifier(nbt, ctx.PackageName);
                Debug.Log($"[BuildPlayer] applicationIdentifier[{nbt}] = {ctx.PackageName}");
            }
            else
            {
                Debug.Log("[BuildPlayer] PackageName 为空，跳过 applicationIdentifier 设置");
            }
        }

        /// <summary>
        /// 将 BuildTarget 映射为 Unity 2022.3 的 NamedBuildTarget。
        /// 说明：SetApplicationIdentifier 必须使用 NamedBuildTarget 以区分多平台包名。
        /// </summary>
        private static NamedBuildTarget ToNamedBuildTarget(BuildTarget target)
        {
            switch (target)
            {
                case BuildTarget.Android:
                    return NamedBuildTarget.Android;
                case BuildTarget.iOS:
                    return NamedBuildTarget.iOS;
                case BuildTarget.WebGL:
                    return NamedBuildTarget.WebGL;
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                default:
                    return NamedBuildTarget.Standalone;
            }
        }
        #endregion

        #region Android Keystore 签名
        /// <summary>
        /// 应用 Android keystore 签名配置。
        /// 仅在 Target == Android 且 KeystorePath 非空时执行；
        /// keystore 文件不存在时仅 LogWarning 并跳过（不阻塞、不硬编码口令进日志）。
        /// </summary>
        /// <param name="ctx">平台打包上下文</param>
        public static void ApplyAndroidKeystore(BuildPlatformContext ctx)
        {
            // 非 Android 平台无需签名
            if (ctx.Target != BuildTarget.Android)
            {
                return;
            }

            // 路径为空：不设置签名（使用工程默认）
            if (string.IsNullOrEmpty(ctx.KeystorePath))
            {
                Debug.Log("[BuildPlayer] KeystorePath 为空，跳过签名设置");
                return;
            }

            // 文件不存在：警告并跳过（绝不把密码打印进日志）
            if (!File.Exists(ctx.KeystorePath))
            {
                Debug.LogWarning("keystore 不存在，跳过签名设置，请检查路径：" + ctx.KeystorePath);
                return;
            }

            // 应用签名（密码来自上下文，默认空串，不做硬编码）
            PlayerSettings.Android.keystoreName = ctx.KeystorePath;
            PlayerSettings.Android.keystorePass = ctx.KeystorePass;
            PlayerSettings.Android.keyaliasName = ctx.KeyAliasName;
            PlayerSettings.Android.keyaliasPass = ctx.KeyAliasPass;
            Debug.Log($"[BuildPlayer] Android keystore 已应用: {ctx.KeystorePath} (alias={ctx.KeyAliasName})");
        }
        #endregion

        #region 场景收集
        /// <summary>
        /// 收集参与打包的场景列表。
        /// 优先级：ctx.Scenes > BuildSettingsStore.Scenes（分号分隔）>
        /// 默认主场景 Assets/Scenes/GameApp.unity。
        /// 说明：逐个校验场景资产是否存在，缺失仅 LogWarning 但仍保留，不阻塞打包。
        /// </summary>
        public static string[] GetScenes(BuildPlatformContext ctx)
        {
            var list = new List<string>();

            // 1. 上下文显式指定
            if (ctx.Scenes != null && ctx.Scenes.Count > 0)
            {
                list.AddRange(ctx.Scenes);
            }
            // 2. 编辑器偏好中保存的分号串
            else if (!string.IsNullOrEmpty(BuildSettingsStore.Scenes))
            {
                string[] arr = BuildSettingsStore.Scenes.Split(
                    new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                list.AddRange(arr);
            }
            // 3. 兜底默认主场景
            else
            {
                list.Add("Assets/Scenes/GameApp.unity");
            }

            // 4. 校验场景资产存在性（缺失不阻塞，仅警告）
            for (int i = 0; i < list.Count; i++)
            {
                string scenePath = list[i];
                SceneAsset scene = AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath);
                if (scene == null)
                {
                    Debug.LogWarning($"[BuildPlayer] 场景缺失，仍保留进打包列表：{scenePath}");
                }
            }

            return list.ToArray();
        }
        #endregion

        #region 结果日志
        /// <summary>
        /// 打印打包结果；成功且处于编辑器交互模式时在 Finder/资源管理器中定位产物。
        /// 说明：batchmode（CI）下禁止调用任何弹窗/定位 API，直接打日志即可。
        /// </summary>
        /// <param name="ok">是否成功</param>
        /// <param name="outPath">产物路径</param>
        /// <param name="platformName">平台名（Android/PC/iOS）</param>
        public static void LogBuildResult(bool ok, string outPath, string platformName)
        {
            if (ok)
            {
                Debug.Log($"打包成功：{platformName} -> {outPath}");
                // 仅在非 batchmode（编辑器交互模式）下打开定位，CI 下跳过
                if (!Application.isBatchMode)
                {
                    EditorUtility.RevealInFinder(outPath);
                }
            }
            else
            {
                Debug.LogError($"打包失败：{platformName} -> {outPath}");
            }
        }
        #endregion
    }
}
