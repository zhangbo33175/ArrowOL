/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Editor
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  BuildPlatformContext.cs
 * author:    云毅
 * created:   2026
 * descrip:   打包流水线 - 平台上下文对象
 *            一次打包流程的全部目标平台信息（目标平台、输出目录、版本号、
 *            签名配置、场景列表等），由 BuildOrderHandler 在 BuildBegin 时
 *            装配并注入 BuildContext，供各 BuildOrder 读取。
 ***************************************************************/

using System.Collections.Generic;
using UnityEditor;

namespace Honor.Editor.BuildTool
{
    /// <summary>
    /// 平台上下文对象（注入 BuildContext）
    /// 说明：字段名冻结，并行任务的各 BuildOrder 实现按此名字读取，勿改动。
    /// </summary>
    public class BuildPlatformContext : IContextObject
    {
        #region 平台与输出
        /// <summary>构建目标平台（BuildTarget）</summary>
        public BuildTarget Target;

        /// <summary>平台名（与运行时 GamePathUtils.PlatformName 对齐，如 Android/iOS）</summary>
        public string PlatformName;

        /// <summary>AB 输出根目录（绝对路径）</summary>
        public string ABOutputRoot;

        /// <summary>资源映射表（respathtab）输出路径（绝对路径）</summary>
        public string ResPathTabPath;

        /// <summary>玩家包（APK/EXE/iOS工程）输出根目录</summary>
        public string PlayerOutPath;
        #endregion

        #region 版本与构建选项
        /// <summary>App 版本号（bundleVersion）</summary>
        public string AppVersion;

        /// <summary>Bundle 版本号（versionCode）</summary>
        public int BundleVersionCode;

        /// <summary>是否 Development Build</summary>
        public bool DevelopmentBuild;

        /// <summary>产品名（productName）</summary>
        public string AppName;

        /// <summary>包名（applicationIdentifier / bundleId）</summary>
        public string PackageName;

        /// <summary>打包场景列表（工程相对路径）</summary>
        public List<string> Scenes = new List<string>();

        /// <summary>Lua 是否加密</summary>
        public bool LuaEncrypt;

        /// <summary>是否增量打包</summary>
        public bool Incremental;
        #endregion

        #region 签名（Android Keystore）
        /// <summary>Keystore 路径</summary>
        public string KeystorePath;

        /// <summary>Keystore 密码</summary>
        public string KeystorePass;

        /// <summary>别名（KeyAlias）名</summary>
        public string KeyAliasName;

        /// <summary>别名密码</summary>
        public string KeyAliasPass;
        #endregion
    }
}
