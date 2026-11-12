/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  AtlasCliPacker.cs
 * author:    云毅
 * created:   2026
 * descrip:   调用本机 TexturePacker.exe 命令行作为官方打包后端，
 *            产出与 TexturePacker 一致的 PNG + 描述文件；
 *            仅做命令行封装，不修改、不绕过其授权与水印。
 ***************************************************************/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Honor.Editor.MapTool.AtlasPacker
{
    /// <summary>
    /// CLI 打包参数快照（由窗口从 UI 控件收集后传入）。
    /// 字段口径与我方存档/UI 保持一致，内部负责翻译成 TexturePacker 命令行取值。
    /// </summary>
    internal class AtlasCliOptions
    {
        public string dataFormat = "Unity3D";
        public int maxWidth = 4096;
        public int maxHeight = 4096;
        public int fixedWidth = 0;
        public int fixedHeight = 0;
        public string sizeConstraints = "POT";

        public int borderPadding = 2;
        public int shapePadding = 2;
        public int innerPadding = 0;
        public int extrude = 0;

        public bool allowRotation = true;
        public string trimMode = "Trim";
        public int trimThreshold = 1;

        public string algorithm = "MaxRects";
        public string heuristics = "Best";

        public float scale = 1f;
        public string scaleMode = "Smooth";
        public bool premultiplyAlpha = false;
        public bool reduceBorderArtifacts = false;
        public bool heuristicMask = false;
        public bool trimSpriteNames = false;
        public string texturePath = "";
        public string imageFormat = "RGBA8888";
        public string dithering = "NearestNeighbour";
        public int dpi = 72;
        public bool autoAlias = true;
    }

    /// <summary>
    /// TexturePacker 命令行打包后端。
    /// 调用本机已安装的 TexturePacker.exe，按官方算法导出图集；
    /// CLI 缺失或调用失败时由上层回退到自研后端。
    /// </summary>
    internal class AtlasCliPacker
    {
        /// <summary>本机 TexturePacker CLI 路径（如需迁移安装位置，改此处即可）。</summary>
        public const string CliExePath = @"E:\TexturePacker\bin\TexturePacker.exe";

        /// <summary>CLI 是否可用：可执行文件存在。</summary>
        public bool IsAvailable => File.Exists(CliExePath);

        /// <summary>
        /// 调用 CLI 打包。
        /// </summary>
        /// <param name="opts">参数快照</param>
        /// <param name="fullSpritePaths">待打包精灵的磁盘全路径列表（含别名重复项，由 CLI 自行去重）</param>
        /// <param name="pngFull">输出 PNG 全路径</param>
        /// <param name="dataFull">输出描述文件全路径</param>
        /// <param name="log">过程/错误输出</param>
        /// <returns>是否成功（退出码 0 且两个产物均落盘）</returns>
        public bool Pack(AtlasCliOptions opts, List<string> fullSpritePaths,
            string pngFull, string dataFull, out string log)
        {
            var sb = new StringBuilder();
            if (!IsAvailable)
            {
                log = $"未找到 TexturePacker CLI：{CliExePath}";
                return false;
            }
            if (fullSpritePaths == null || fullSpritePaths.Count == 0)
            {
                log = "CLI 后端：精灵列表为空";
                return false;
            }

            var args = BuildArguments(opts, fullSpritePaths, pngFull, dataFull);

            var psi = new ProcessStartInfo
            {
                FileName = CliExePath,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                WorkingDirectory = Path.GetDirectoryName(pngFull) ?? Environment.CurrentDirectory,
                StandardOutputEncoding = Encoding.Default,
                StandardErrorEncoding = Encoding.Default,
            };
            // Unity Mono 下 ArgumentList 不参与命令行生成，改为手动拼接 Arguments 并逐参数加引号。
            psi.Arguments = BuildArgumentString(args);

            try
            {
                var proc = Process.Start(psi);
                if (proc == null) { log = "CLI 进程启动失败"; return false; }
                string stdout = proc.StandardOutput.ReadToEnd();
                string stderr = proc.StandardError.ReadToEnd();
                proc.WaitForExit();

                sb.AppendLine(stdout);
                if (!string.IsNullOrWhiteSpace(stderr)) sb.AppendLine(stderr);

                bool okExit = proc.ExitCode == 0;
                bool pngOk = File.Exists(pngFull) && new FileInfo(pngFull).Length > 0;
                bool dataOk = File.Exists(dataFull) && new FileInfo(dataFull).Length > 0;

                log = sb.ToString().Trim();
                return okExit && pngOk && dataOk;
            }
            catch (Exception ex)
            {
                log = $"CLI 调用异常：{ex.Message}";
                return false;
            }
        }

        /// <summary>把我方参数快照翻译成 TexturePacker 命令行参数列表。</summary>
        private static List<string> BuildArguments(AtlasCliOptions o, List<string> sprites, string pngFull, string dataFull)
        {
            var args = new List<string>
            {
                "--sheet", pngFull,
                "--data", dataFull,
                "--format", MapDataFormat(o.dataFormat),
                "--max-width", o.maxWidth.ToString(),
                "--max-height", o.maxHeight.ToString(),
                "--shape-padding", o.shapePadding.ToString(),
                "--border-padding", o.borderPadding.ToString(),
                "--inner-padding", o.innerPadding.ToString(),
                "--trim-threshold", o.trimThreshold.ToString(),
            };

            // 固定尺寸：0 的一侧仍走自适应上限
            if (o.fixedWidth > 0) { args.Add("--width"); args.Add(o.fixedWidth.ToString()); }
            if (o.fixedHeight > 0) { args.Add("--height"); args.Add(o.fixedHeight.ToString()); }

            // NPOT：允许非 2 的幂；POT 不传该开关
            if (string.Equals(o.sizeConstraints, "NPOT", StringComparison.OrdinalIgnoreCase))
                args.Add("--allow-free-size");

            // 旋转
            args.Add(o.allowRotation ? "--enable-rotation" : "--disable-rotation");

            // Trim 模式
            switch (o.trimMode)
            {
                case "None": args.Add("--no-trim"); break;
                case "Crop": args.Add("--crop"); break;
                default: args.Add("--trim"); break;
            }

            // 算法与启发式
            args.Add("--algorithm"); args.Add(o.algorithm);
            if (string.Equals(o.algorithm, "MaxRects", StringComparison.OrdinalIgnoreCase))
            {
                args.Add("--maxrects-heuristics");
                args.Add(MapHeuristics(o.heuristics));
            }

            // 几何 / 导出
            if (o.extrude > 0) { args.Add("--extrude"); args.Add(o.extrude.ToString()); }
            if (Math.Abs(o.scale - 1f) > 1e-6f) { args.Add("--scale"); args.Add(o.scale.ToString("0.###", System.Globalization.CultureInfo.InvariantCulture)); }
            args.Add("--scale-mode"); args.Add(MapScaleMode(o.scaleMode));
            if (o.premultiplyAlpha) args.Add("--premultiply-alpha");
            if (o.reduceBorderArtifacts) args.Add("--reduce-border-artifacts");
            if (o.heuristicMask) args.Add("--heuristic-mask");
            if (o.trimSpriteNames) args.Add("--trim-sprite-names");
            if (!string.IsNullOrEmpty(o.texturePath)) { args.Add("--texturepath"); args.Add(o.texturePath); }
            args.Add("--opt"); args.Add(o.imageFormat);
            args.Add(MapDithering(o.dithering));
            args.Add("--dpi"); args.Add(o.dpi.ToString());
            if (!o.autoAlias) args.Add("--disable-auto-alias");

            // 精灵文件列表（位置参数置于末尾）
            args.AddRange(sprites);
            return args;
        }

        // ===== 取值映射：我方 UI 名 → CLI 官方名 =====

        private static string MapDataFormat(string format)
        {
            switch ((format ?? "").ToLowerInvariant())
            {
                case "unity3d": return "unity";
                case "cocos2d":
                case "cocos2d-iphone":
                case "plist": return "cocos2d";
                case "json": return "json";
                case "xml": return "xml";
                case "css": return "css";
                case "starling": return "sparrow";
                default: return "unity";
            }
        }

        private static string MapHeuristics(string h)
        {
            switch ((h ?? "").ToLowerInvariant())
            {
                case "shortside": return "shortSide";
                case "longside": return "longSide";
                case "minarea": return "area";
                case "minwidth": return "bottomLeft";
                case "maxaspect": return "contactPoint";
                case "best":
                default: return "best";
            }
        }

        private static string MapScaleMode(string mode)
        {
            switch ((mode ?? "").ToLowerInvariant())
            {
                case "nearest": return "fast";
                case "smooth":
                default: return "smooth";
            }
        }

        private static string MapDithering(string d)
        {
            switch ((d ?? "").ToLowerInvariant())
            {
                case "bilinear": return "--dither-none-linear";
                case "nearestneighbour":
                default: return "--dither-none-nn";
            }
        }

        /// <summary>把参数列表拼成 Windows 命令行字符串，逐参数用双引号包裹。</summary>
        private static string BuildArgumentString(List<string> args)
        {
            return string.Join(" ", args.Select(QuoteArg));
        }

        /// <summary>单个参数加引号并转义内部双引号。</summary>
        private static string QuoteArg(string arg)
        {
            if (string.IsNullOrEmpty(arg)) return "\"\"";
            var sb = new StringBuilder(arg.Length + 2);
            sb.Append('"');
            foreach (char c in arg)
            {
                if (c == '"') sb.Append("\\\"");
                else sb.Append(c);
            }
            sb.Append('"');
            return sb.ToString();
        }
    }
}
