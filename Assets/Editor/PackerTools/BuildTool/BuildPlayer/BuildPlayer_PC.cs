/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Editor
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  BuildPlayer_PC.cs
 * author:    Honor.Editor
 * created:   2026
 * descrip:   Windows Standalone(x86_64) PC 玩家包打包
 *            约定：LZ4 压缩；Development 构建叠加 Development 标志
 ***************************************************************/

using System;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Honor.Editor.BuildTool
{
    /// <summary>
    /// PC（Windows StandaloneWindows64）玩家包打包器。
    /// 说明：产物为 .exe，Unity 会在同目录生成 *_Data 数据目录；
    /// StreamingAssets 下 AssetBundles/Android 目录随包发布
    /// （PC 运行时按 Android 宏读取 AB，资源打包任务已按此输出）。
    /// </summary>
    public static class BuildPlayer_PC
    {
        /// <summary>
        /// 执行 PC exe 打包。
        /// </summary>
        /// <param name="ctx">平台打包上下文</param>
        /// <returns>打包成功返回 true；异常或 BuildResult 失败返回 false</returns>
        public static bool Build(BuildPlatformContext ctx)
        {
            try
            {
                // 1. 应用 PlayerSettings（PC 走 Standalone NamedBuildTarget）
                BuildPlayer_Common.ApplyPlayerSettings(ctx);

                // 2. 计算输出路径（.exe）
                string outPath = BuildPlayer_Common.GetOutPath(ctx, ".exe");

                // 3. 构造 BuildOptions：LZ4；Development 叠加 Development 标志
                BuildOptions options = BuildOptions.CompressWithLz4;
                if (ctx.DevelopmentBuild)
                {
                    options |= BuildOptions.Development;
                }

                // 4. 调用 BuildPipeline 打 StandaloneWindows64
                BuildReport report = BuildPipeline.BuildPlayer(
                    BuildPlayer_Common.GetScenes(ctx),
                    outPath,
                    BuildTarget.StandaloneWindows64,
                    options);

                bool ok = report != null && report.summary.result == BuildResult.Succeeded;
                BuildPlayer_Common.LogBuildResult(ok, outPath, "PC");
                return ok;
            }
            catch (Exception e)
            {
                Debug.LogError($"[BuildPlayer_PC] 打包异常: {e}");
                return false;
            }
        }
    }
}
