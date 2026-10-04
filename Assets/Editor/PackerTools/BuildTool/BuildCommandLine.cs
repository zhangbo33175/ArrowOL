/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Editor
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  BuildCommandLine.cs
 * author:    Honor.Editor
 * created:   2026
 * descrip:   命令行 CI 打包入口（供 Unity -batchmode -executeMethod 调用）
 *            支持：打 AB / 打资源 / 打 Lua / 打 APK / 打 PC / 打 iOS / 全流程
 *
 * 用法示例（Windows 命令行，路径按实际工程调整）：
 *   Unity.exe -batchmode -projectPath "E:/Game/CatGame/CatGameCodeOptimizationBuild" ^
 *     -executeMethod Honor.Editor.BuildTool.BuildCommandLine.BuildAPK ^
 *     -buildPlatform Android -version 1.0.0 -bundleCode 1 -development false ^
 *     -outPath "E:/Builds" -appName CatGame -packageName com.honor.catgame ^
 *     -scenes "Assets/Scenes/GameApp.unity" ^
 *     -keystorePath "Assets/Editor/BuildTool/qixia_sg.jks" -keyaliasName qixia_sg ^
 *     -logFile -
 *
 *   # 打 PC exe
 *   Unity.exe -batchmode -projectPath <proj> -executeMethod Honor.Editor.BuildTool.BuildCommandLine.BuildPC ^
 *     -buildPlatform PC -version 1.0.0 -development false -outPath "E:/Builds"
 *
 *   # 导出 iOS Xcode 工程（须在 macOS 上执行）
 *   Unity.exe -batchmode -projectPath <proj> -executeMethod Honor.Editor.BuildTool.BuildCommandLine.BuildIOS ^
 *     -buildPlatform iOS -version 1.0.0
 *
 *   # 仅打资源 AB / 仅打 Lua / 全流程
 *   ... -executeMethod Honor.Editor.BuildTool.BuildCommandLine.BuildRes  -buildPlatform Android
 *   ... -executeMethod Honor.Editor.BuildTool.BuildCommandLine.BuildLua  -buildPlatform Android
 *   ... -executeMethod Honor.Editor.BuildTool.BuildCommandLine.BuildAll  -buildPlatform Android
 *
 *   # 查看完整参数说明
 *   ... -executeMethod Honor.Editor.BuildTool.BuildCommandLine.BuildAPK -help
 *
 * 退出码：0 = 成功；1 = 失败（订单 action 内 throw 或异常均置 1，便于 Jenkins 判定）。
 ***************************************************************/

using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Honor.Editor.BuildTool
{
    /// <summary>
    /// 命令行 CI 打包入口。
    /// 说明：每个 public static 方法都是一个 -executeMethod 入口；
    /// 统一流程为：解析命令行参数写入 BuildSettingsStore -> 调 BuildOrderHandler
    /// 对应流水线 -> 成功 Exit(0)，异常/订单 throw -> Exit(1)。
    /// </summary>
    public static class BuildCommandLine
    {
        #region 命令行参数读取
        /// <summary>
        /// 获取 Unity 进程完整命令行参数（含 exe 自身路径）。
        /// </summary>
        private static string[] GetCmdArgs()
        {
            return Environment.GetCommandLineArgs();
        }

        /// <summary>
        /// 查找形如 "-key value" 的参数值；未找到时返回 def。
        /// </summary>
        /// <param name="args">命令行参数数组</param>
        /// <param name="key">参数键（含前导短横，如 "-buildPlatform"）</param>
        /// <param name="def">缺省值</param>
        private static string GetArgValue(string[] args, string key, string def)
        {
            for (int i = 0; i < args.Length - 1; i++)
            {
                if (string.Equals(args[i], key, StringComparison.OrdinalIgnoreCase))
                {
                    return args[i + 1];
                }
            }
            return def;
        }

        /// <summary>
        /// 判断命令行是否仅含某个开关参数（无值），如 -help。
        /// </summary>
        private static bool HasArg(string[] args, string key)
        {
            return args.Any(a => string.Equals(a, key, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// 解析布尔参数（"true"/"1"/"yes" 视为真，大小写不敏感）。
        /// </summary>
        private static bool ParseBool(string s, bool def = false)
        {
            if (string.IsNullOrEmpty(s)) return def;
            s = s.Trim().ToLower();
            return s == "true" || s == "1" || s == "yes";
        }

        /// <summary>
        /// 解析 -buildPlatform 字符串到 BuildTarget。
        /// 约定：Android -> Android；iOS -> iOS；PC -> StandaloneWindows64；WebGL -> WebGL。
        /// </summary>
        private static BuildTarget ParseBuildTarget(string platform)
        {
            switch (platform == null ? string.Empty : platform.Trim().ToLower())
            {
                case "android": return BuildTarget.Android;
                case "ios": return BuildTarget.iOS;
                case "pc":
                case "standalone":
                case "windows": return BuildTarget.StandaloneWindows64;
                case "webgl": return BuildTarget.WebGL;
                default:
                    Debug.LogWarning($"[BuildCommandLine] 未知 -buildPlatform '{platform}'，回退 Android");
                    return BuildTarget.Android;
            }
        }
        #endregion

        #region 参数解析与应用
        /// <summary>
        /// 打印用法并退出（-help）。
        /// </summary>
        private static void PrintUsageAndExit()
        {
            Debug.Log(
                "[BuildCommandLine] 可用参数：\n" +
                "  -buildPlatform  Android | iOS | PC | WebGL（PC=StandaloneWindows64）\n" +
                "  -version        版本号字符串，如 1.0.0\n" +
                "  -bundleCode     Android versionCode（int）\n" +
                "  -development    true/false 是否开发包\n" +
                "  -outPath        玩家包输出根目录\n" +
                "  -appName        产物文件名前缀\n" +
                "  -packageName    applicationIdentifier（如 com.honor.catgame）\n" +
                "  -keystorePath   Android 签名库路径\n" +
                "  -keystorePass   keystore 密码\n" +
                "  -keyaliasName   别名\n" +
                "  -keyaliasPass   别名密码\n" +
                "  -scenes         场景列表，分号分隔\n" +
                "  -incremental    true/false 是否增量打包\n" +
                "  -luaEncrypt     true/false 是否加密 Lua\n" +
                "  -help           打印本说明\n");
            EditorApplication.Exit(0);
        }

        /// <summary>
        /// 解析命令行参数并写入 BuildSettingsStore，随后 Save() 持久化。
        /// </summary>
        private static void ParseArgsAndApply()
        {
            string[] args = GetCmdArgs();

            // -help：打印用法并直接退出
            if (HasArg(args, "-help") || HasArg(args, "-h"))
            {
                PrintUsageAndExit();
            }

            // --- 平台 ---
            BuildTarget target = ParseBuildTarget(GetArgValue(args, "-buildPlatform", "Android"));
            BuildSettingsStore.CurrentPlatform = target;

            // --- 版本 / 构建号 ---
            BuildSettingsStore.AppVersion =
                GetArgValue(args, "-version", BuildSettingsStore.AppVersion);

            string bundleCodeStr = GetArgValue(args, "-bundleCode", BuildSettingsStore.BundleVersionCode.ToString());
            if (int.TryParse(bundleCodeStr, out int bundleCode))
            {
                BuildSettingsStore.BundleVersionCode = bundleCode;
            }
            else
            {
                Debug.LogWarning($"[BuildCommandLine] -bundleCode 无法解析为 int: '{bundleCodeStr}'，保持 {BuildSettingsStore.BundleVersionCode}");
            }

            // --- 开发包开关 ---
            string devStr = GetArgValue(args, "-development", BuildSettingsStore.DevelopmentBuild ? "true" : "false");
            BuildSettingsStore.DevelopmentBuild = ParseBool(devStr, BuildSettingsStore.DevelopmentBuild);

            // --- 输出 / 应用信息 ---
            BuildSettingsStore.OutRoot = GetArgValue(args, "-outPath", BuildSettingsStore.OutRoot);
            BuildSettingsStore.AppName = GetArgValue(args, "-appName", BuildSettingsStore.AppName);
            BuildSettingsStore.PackageName = GetArgValue(args, "-packageName", BuildSettingsStore.PackageName);

            // --- Android 签名（密码默认空，不硬编码） ---
            BuildSettingsStore.KeystorePath = GetArgValue(args, "-keystorePath", BuildSettingsStore.KeystorePath);
            BuildSettingsStore.KeystorePass = GetArgValue(args, "-keystorePass", BuildSettingsStore.KeystorePass);
            BuildSettingsStore.KeyAliasName = GetArgValue(args, "-keyaliasName", BuildSettingsStore.KeyAliasName);
            BuildSettingsStore.KeyAliasPass = GetArgValue(args, "-keyaliasPass", BuildSettingsStore.KeyAliasPass);

            // --- 场景列表（分号串） ---
            BuildSettingsStore.Scenes = GetArgValue(args, "-scenes", BuildSettingsStore.Scenes);

            // --- 增量 / Lua 加密开关（同步进上下文持久层） ---
            string incStr = GetArgValue(args, "-incremental", BuildSettingsStore.Incremental ? "true" : "false");
            BuildSettingsStore.Incremental = ParseBool(incStr, BuildSettingsStore.Incremental);

            string luaStr = GetArgValue(args, "-luaEncrypt", BuildSettingsStore.LuaEncrypt ? "true" : "false");
            BuildSettingsStore.LuaEncrypt = ParseBool(luaStr, BuildSettingsStore.LuaEncrypt);

            // 持久化到 EditorPrefs
            BuildSettingsStore.Save();

            Debug.Log(
                $"[BuildCommandLine] 参数已应用: platform={target}, version={BuildSettingsStore.AppVersion}, " +
                $"bundleCode={BuildSettingsStore.BundleVersionCode}, dev={BuildSettingsStore.DevelopmentBuild}, " +
                $"outRoot={BuildSettingsStore.OutRoot}, appName={BuildSettingsStore.AppName}, " +
                $"pkg={BuildSettingsStore.PackageName}, incremental={BuildSettingsStore.Incremental}, " +
                $"luaEncrypt={BuildSettingsStore.LuaEncrypt}");
        }
        #endregion

        #region CI 入口（-executeMethod）
        /// <summary>
        /// CI 入口：打 AssetBundle（资源打包任务公开入口 BuildAB）。
        /// 示例：... -executeMethod Honor.Editor.BuildTool.BuildCommandLine.BuildAB -buildPlatform Android
        /// </summary>
        public static void BuildAB()
        {
            RunCI("BuildAB", () => BuildOrderHandler.Inst.BuildAB());
        }

        /// <summary>
        /// CI 入口：打资源（BuildRes）。
        /// </summary>
        public static void BuildRes()
        {
            RunCI("BuildRes", () => BuildOrderHandler.Inst.BuildRes());
        }

        /// <summary>
        /// CI 入口：打 Lua。
        /// </summary>
        public static void BuildLua()
        {
            RunCI("BuildLua", () => BuildOrderHandler.Inst.BuildLua());
        }

        /// <summary>
        /// CI 入口：打 Android APK 玩家包。
        /// </summary>
        public static void BuildAPK()
        {
            RunCI("BuildAPK", () => BuildOrderHandler.Inst.BuildAPK());
        }

        /// <summary>
        /// CI 入口：打 PC exe 玩家包。
        /// </summary>
        public static void BuildPC()
        {
            RunCI("BuildPC", () => BuildOrderHandler.Inst.BuildPC());
        }

        /// <summary>
        /// CI 入口：导出 iOS Xcode 工程（须在 macOS 执行）。
        /// </summary>
        public static void BuildIOS()
        {
            RunCI("BuildIOS", () => BuildOrderHandler.Inst.BuildIOS());
        }

        /// <summary>
        /// CI 入口：全流程（资源 + Lua + 玩家包），对应 BuildOrderHandler.BuildAllPlatforms。
        /// </summary>
        public static void BuildAll()
        {
            RunCI("BuildAll", () => BuildOrderHandler.Inst.BuildAllPlatforms());
        }

        /// <summary>
        /// CI 统一执行壳：解析参数 -> 执行流水线 -> 成功 Exit(0)，异常 Exit(1)。
        /// 说明：订单 action 内检测到失败会 throw，本壳统一捕获并置非零退出码。
        /// </summary>
        /// <param name="name">入口名（仅用于日志）</param>
        /// <param name="pipeline">BuildOrderHandler 对应流水线调用</param>
        private static void RunCI(string name, Action pipeline)
        {
            try
            {
                ParseArgsAndApply();
                pipeline();
                Debug.Log($"[BuildCommandLine] {name} 成功完成");
                EditorApplication.Exit(0);
            }
            catch (Exception e)
            {
                Debug.LogError($"[BuildCommandLine] {name} 失败: {e}");
                EditorApplication.Exit(1);
            }
        }
        #endregion
    }
}
