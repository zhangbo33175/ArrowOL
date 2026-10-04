/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Editor
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  BuildPlayer_iOS.cs
 * author:    Honor.Editor
 * created:   2026
 * descrip:   iOS Xcode 工程打包（导出，非直接出 ipa）
 *            约定：LZ4 压缩；Development 构建叠加 Development 标志
 *            注意：iOS 导出必须在 macOS + Xcode 环境进行，
 *                  Windows 上 BuildPipeline.BuildPlayer 会抛异常，此处捕获并提示。
 ***************************************************************/

using System;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Honor.Editor.BuildTool
{
    /// <summary>
    /// iOS 玩家包打包器（导出 Xcode 工程）。
    /// 说明：GetOutPath 传入空后缀，产物为一个 Xcode 工程目录，
    /// 需在 macOS 上用 Xcode 打开后再 Archive / 出 ipa。
    /// </summary>
    public static class BuildPlayer_iOS
    {
        /// <summary>
        /// 执行 iOS Xcode 工程导出。
        /// </summary>
        /// <param name="ctx">平台打包上下文</param>
        /// <returns>成功返回 true；Windows 环境或 BuildResult 失败返回 false</returns>
        public static bool Build(BuildPlatformContext ctx)
        {
            try
            {
                // 1. 应用 PlayerSettings（iOS 走 iOS NamedBuildTarget）
                BuildPlayer_Common.ApplyPlayerSettings(ctx);

                // 2. 计算输出路径（空后缀 => Xcode 工程目录）
                string outPath = BuildPlayer_Common.GetOutPath(ctx, "");

                // 3. 构造 BuildOptions：LZ4；Development 叠加 Development 标志
                BuildOptions options = BuildOptions.CompressWithLz4;
                if (ctx.DevelopmentBuild)
                {
                    options |= BuildOptions.Development;
                }

                // 4. 导出 Xcode 工程
                BuildReport report = BuildPipeline.BuildPlayer(
                    BuildPlayer_Common.GetScenes(ctx),
                    outPath,
                    BuildTarget.iOS,
                    options);

                bool ok = report != null && report.summary.result == BuildResult.Succeeded;
                BuildPlayer_Common.LogBuildResult(ok, outPath, "iOS");
                return ok;
            }
            catch (Exception e)
            {
                // Windows 上无法执行 iOS BuildPipeline，捕获后给出明确提示
                Debug.LogError("iOS 打包需在 macOS 上进行：" + e);
                return false;
            }
        }
    }
}
