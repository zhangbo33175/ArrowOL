/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Editor
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  BuildPackage_Define.cs
 * author:    云毅
 * created:   2026
 * descrip:   AssetBundle 打包工具 - 全局常量与枚举定义
 *            基于参照工程 BuildPackage 框架移植，适配 Honor 运行时约定：
 *            bundle 名 = 相对路径 Replace('/','@').ToLower() + ".bundle"
 *            （与 AssetBundleLoadManager.GetABFormatPath 完全一致）
 ***************************************************************/

using UnityEditor;
using UnityEngine;

namespace Honor.Editor.BuildPackage
{
    //=========================================================================
    // AB 打包全局常量
    // 统一资源根目录、AB 后缀、输出目录、平台名等配置
    //=========================================================================
    /// <summary>
    /// AB 打包全局常量定义
    /// 说明：所有打包相关路径与命名约定集中于此，便于维护与跨模块复用
    /// </summary>
    public static class BuildPackage_Define
    {
        #region 资源根目录
        /// <summary>
        /// 打包资源根目录（相对 Assets 的路径，用于 AssetDatabase）
        /// 说明：工程资源统一存放在该目录下，打包时按此根目录计算相对路径作为 bundle 名
        /// </summary>
        public const string AssetResRoot = "Assets/Res";

        /// <summary>
        /// 打包资源根目录（绝对路径，用于文件读写）
        /// </summary>
        public static string AssetResRootFullPath => $"{Application.dataPath}/{AssetResRoot.Substring("Assets/".Length)}";
        #endregion

        #region AB 命名约定
        /// <summary>
        /// AB 包后缀（与运行时 AssetBundleLoadManager 约定一致）
        /// </summary>
        public const string BundleSuffix = ".bundle";

        /// <summary>
        /// 路径分隔符替换符：'/' 替换为 '@'（与运行时约定一致）
        /// </summary>
        public const char PathSeparatorReplace = '@';

        /// <summary>
        /// 平台清单包名（运行时 LoadManifest 按此名加载）
        /// 形如 "Android.bundle"
        /// </summary>
        public static string PlatformManifestBundleName => $"{EditorPath.PlatformName}{BundleSuffix}";
        #endregion

        #region 打包输出目录
        /// <summary>
        /// AB 输出根目录（相对 StreamingAssets，运行时 Streaming.GetRootDirectoryFullPath 读取此处）
        /// 形如 "AssetBundles/Android"
        /// </summary>
        public static string BuildOutputRelativeRoot => $"AssetBundles/{EditorPath.PlatformName}";

        /// <summary>
        /// AB 输出根目录（绝对路径）
        /// </summary>
        public static string BuildOutputFullRoot => $"{Application.streamingAssetsPath}/{BuildOutputRelativeRoot}";

        /// <summary>
        /// 资源映射文件名（运行时可据此由资源名反查 bundle 名）
        /// </summary>
        public const string ResMappingFileName = "respathtab.txt";
        #endregion

        #region 打包选项
        /// <summary>
        /// 默认构建目标平台（跟随当前编辑器活动构建目标）
        /// </summary>
        public static BuildTarget CurrentBuildTarget => EditorUserBuildSettings.activeBuildTarget;

        /// <summary>
        /// 打包选项：确定性 + 按块压缩（体积与可缓存性兼顾）
        /// </summary>
        public const BuildAssetBundleOptions DefaultBuildOptions = BuildAssetBundleOptions.DeterministicAssetBundle | BuildAssetBundleOptions.ChunkBasedCompression;
        #endregion

        #region 平台名（编辑宏识别）
        /// <summary>
        /// 获取当前平台名称（与运行时 GamePathUtils.PlatformName 逻辑一致）
        /// </summary>
        public static string GetPlatformName()
        {
#if UNITY_IOS
            return "iOS";
#elif UNITY_WEBGL
            return "WebGL";
#else
            return "Android";
#endif
        }
        #endregion

        #region 指定平台打包（扩展：按 BuildTarget 显式指定输出目录与清单名）
        //=====================================================================
        // 以下为"指定平台打包"扩展接口：与上方基于活动平台/编辑宏的视图接口并存。
        // 旧窗口/菜单仍使用 BuildOutputFullRoot / PlatformManifestBundleName 等
        // 活动平台视图属性；新的打包流水线（BuildOrders）按 BuildTarget 调用本组接口，
        // 实现一次编辑器会话内交叉打包多个目标平台。
        //=====================================================================

        /// <summary>
        /// 根据 BuildTarget 解析运行时平台目录名（与 GamePathUtils.PlatformName 对应）
        /// 说明：
        /// <list type="bullet">
        /// <item>Android → "Android"</item>
        /// <item>iOS → "iOS"</item>
        /// <item>WebGL → "WebGL"</item>
        /// <item>StandaloneWindows / StandaloneWindows64 → "Android"（见下方说明）</item>
        /// <item>其他平台 → 告警后兜底返回 "Android"</item>
        /// </list>
        /// 特别注意 PC(StandaloneWindows64)：运行时 GamePathUtils.PlatformName 没有
        /// Standalone 分支，编译 PC 包时落到 #else 默认宏即 "Android"。
        /// 为保证 PC 运行时能在 StreamingAssets/AssetBundles/Android 下找到 AB 与清单，
        /// PC 包的 AB 输出目录与 {平台名}.bundle 清单一律沿用 Android 命名。
        /// </summary>
        /// <param name="target">BuildPipeline 目标构建平台</param>
        /// <returns>运行时平台目录名（Android / iOS / WebGL）</returns>
        public static string GetPlatformName(BuildTarget target)
        {
            switch (target)
            {
                case BuildTarget.Android:
                    return "Android";
                case BuildTarget.iOS:
                    return "iOS";
                case BuildTarget.WebGL:
                    return "WebGL";
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                    // PC 包沿用 Android 目录：运行时宏无 Standalone 分支，
                    // GamePathUtils.PlatformName 在 PC 下恒为 "Android"，
                    // 故 PC 的 AB 输出目录/清单名必须与 Android 一致。
                    return "Android";
                default:
                    Debug.LogWarning($"[AB打包] 未显式支持的 BuildTarget={target}，兜底使用 \"Android\" 输出目录。");
                    return "Android";
            }
        }

        /// <summary>
        /// 指定平台的 AB 输出根目录（相对 StreamingAssets 的相对路径）
        /// 形如 "AssetBundles/Android"
        /// </summary>
        /// <param name="target">BuildPipeline 目标构建平台</param>
        /// <returns>相对 StreamingAssets 的输出根目录</returns>
        public static string GetBuildOutputRelativeRoot(BuildTarget target) => $"AssetBundles/{GetPlatformName(target)}";

        /// <summary>
        /// 指定平台的 AB 输出根目录（绝对路径，指向 StreamingAssets 下）
        /// 形如 "{StreamingAssets}/AssetBundles/Android"
        /// </summary>
        /// <param name="target">BuildPipeline 目标构建平台</param>
        /// <returns>输出根目录绝对路径</returns>
        public static string GetBuildOutputFullRoot(BuildTarget target) => $"{Application.streamingAssetsPath}/{GetBuildOutputRelativeRoot(target)}";

        /// <summary>
        /// 指定平台的清单 bundle 名（运行时 LoadManifest 按此名加载）
        /// 形如 "Android.bundle"
        /// 说明：BuildPipeline.BuildAssetBundles 生成的清单文件名为输出目录末级文件夹名
        /// （无扩展名），打包后须由打包器改名/拷贝为本名，运行时据此加载 manifest。
        /// </summary>
        /// <param name="target">BuildPipeline 目标构建平台</param>
        /// <returns>清单 bundle 文件名（含 .bundle 后缀）</returns>
        public static string GetPlatformManifestBundleName(BuildTarget target) => $"{GetPlatformName(target)}{BundleSuffix}";
        #endregion
    }
}
