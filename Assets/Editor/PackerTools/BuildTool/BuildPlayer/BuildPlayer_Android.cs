/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Editor
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  BuildPlayer_Android.cs
 * author:    云毅
 * created:   2026
 * descrip:   Android APK 玩家包打包
 *            约定（参照 BuildPackage_APK.cs）：
 *            - LZ4 压缩：BuildOptions.CompressWithLz4
 *            - Development 构建叠加 BuildOptions.Development
 *            - Development 构建输出调试符号：androidCreateSymbols = Debugging
 ***************************************************************/

using System;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Honor.Editor.BuildTool
{
    /// <summary>
    /// Android 玩家包（APK）打包器。
    /// 说明：仅负责打 APK，不负责资源 AB；StreamingAssets 下已由资源打包任务
    /// 放置好 AssetBundles/Android 目录，随 APK 一起发布。
    /// </summary>
    public static class BuildPlayer_Android
    {
        /// <summary>
        /// 执行 Android APK 打包。
        /// </summary>
        /// <param name="ctx">平台打包上下文</param>
        /// <returns>打包成功返回 true；异常或 BuildResult 失败返回 false</returns>
        public static bool Build(BuildPlatformContext ctx)
        {
            try
            {
                // 1. 应用 PlayerSettings 与 keystore 签名
                BuildPlayer_Common.ApplyPlayerSettings(ctx);
                BuildPlayer_Common.ApplyAndroidKeystore(ctx);

                // 2. 计算输出路径（.apk）
                string outPath = BuildPlayer_Common.GetOutPath(ctx, ".apk");

                // 3. 构造 BuildOptions：LZ4 压缩；Development 构建叠加 Development 标志
                BuildOptions options = BuildOptions.CompressWithLz4;
                if (ctx.DevelopmentBuild)
                {
                    options |= BuildOptions.Development;
                    // 开发包输出调试符号，便于分析 native 崩溃（参照工程约定）
                    EditorUserBuildSettings.androidCreateSymbols = AndroidCreateSymbols.Debugging;
                }

                // 4. 调用 BuildPipeline 打 APK
                BuildReport report = BuildPipeline.BuildPlayer(
                    BuildPlayer_Common.GetScenes(ctx),
                    outPath,
                    BuildTarget.Android,
                    options);

                bool ok = report != null && report.summary.result == BuildResult.Succeeded;
                BuildPlayer_Common.LogBuildResult(ok, outPath, "Android");
                return ok;
            }
            catch (Exception e)
            {
                Debug.LogError($"[BuildPlayer_Android] 打包异常: {e}");
                return false;
            }
        }
    }
}
