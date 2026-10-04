/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Editor
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  BuildSettingsStore.cs
 * author:    云毅
 * created:   2026
 * descrip:   打包流水线 - 全局打包设置持久化
 *            所有窗口/CI 可改的打包参数统一经 EditorPrefs 存取，
 *            key 前缀 "honor_build_"；静态构造函数自动 Load()。
 ***************************************************************/

using UnityEditor;
using UnityEngine;

namespace Honor.Editor.BuildTool
{
    /// <summary>
    /// 打包全局设置持久化（EditorPrefs）
    /// 说明：窗口与命令行共享同一份设置，BuildBegin 前由 BuildOrderHandler 统一 Save。
    /// </summary>
    public static class BuildSettingsStore
    {
        #region 常量（EditorPrefs key）
        private const string KeyPrefix = "honor_build_";

        private const string KeyCurrentPlatform = KeyPrefix + "current_platform";
        private const string KeyIncremental = KeyPrefix + "incremental";
        private const string KeyLuaEncrypt = KeyPrefix + "lua_encrypt";
        private const string KeyAppVersion = KeyPrefix + "app_version";
        private const string KeyBundleVersionCode = KeyPrefix + "bundle_version_code";
        private const string KeyDevelopmentBuild = KeyPrefix + "development_build";
        private const string KeyAppName = KeyPrefix + "app_name";
        private const string KeyPackageName = KeyPrefix + "package_name";
        private const string KeyOutRoot = KeyPrefix + "out_root";
        private const string KeyScenes = KeyPrefix + "scenes";
        private const string KeyKeystorePath = KeyPrefix + "keystore_path";
        private const string KeyKeystorePass = KeyPrefix + "keystore_pass";
        private const string KeyKeyAliasName = KeyPrefix + "key_alias_name";
        private const string KeyKeyAliasPass = KeyPrefix + "key_alias_pass";

        private const string DefaultScenes = "Assets/Scenes/GameApp.unity";
        private const string DefaultKeystorePath = "Assets/Editor/BuildTool/qixia_sg.jks";
        private const string DefaultKeyAliasName = "qixia_sg";
        #endregion

        #region 属性（自动属性，Load 时从 EditorPrefs 回填，Save 时写回）
        /// <summary>当前目标平台（窗口/CI 可改；BuildAB/BuildRes/BuildLua/BuildCustom 跟随此值）</summary>
        public static BuildTarget CurrentPlatform { get; set; }

        /// <summary>是否增量打包</summary>
        public static bool Incremental { get; set; }

        /// <summary>Lua 是否加密</summary>
        public static bool LuaEncrypt { get; set; }

        /// <summary>App 版本号（默认取 PlayerSettings.bundleVersion）</summary>
        public static string AppVersion { get; set; }

        /// <summary>Bundle 版本号（默认取 PlayerSettings.Android.bundleVersionCode）</summary>
        public static int BundleVersionCode { get; set; }

        /// <summary>是否 Development Build</summary>
        public static bool DevelopmentBuild { get; set; }

        /// <summary>产品名（默认取 PlayerSettings.productName）</summary>
        public static string AppName { get; set; }

        /// <summary>包名（默认取 PlayerSettings.applicationIdentifier）</summary>
        public static string PackageName { get; set; }

        /// <summary>玩家包输出根目录（默认 工程根/Builds）</summary>
        public static string OutRoot { get; set; }

        /// <summary>打包场景列表（分号分隔，默认 Assets/Scenes/GameApp.unity）</summary>
        public static string Scenes { get; set; }

        /// <summary>Keystore 路径</summary>
        public static string KeystorePath { get; set; }

        /// <summary>Keystore 密码</summary>
        public static string KeystorePass { get; set; }

        /// <summary>KeyAlias 名（默认 qixia_sg）</summary>
        public static string KeyAliasName { get; set; }

        /// <summary>KeyAlias 密码</summary>
        public static string KeyAliasPass { get; set; }
        #endregion

        #region 构造
        /// <summary>静态构造：首次访问时自动从 EditorPrefs 加载</summary>
        static BuildSettingsStore()
        {
            Load();
        }
        #endregion

        #region 默认值
        /// <summary>
        /// 默认玩家包输出根目录：工程根目录/Builds
        /// 说明：Application.dataPath 形如 <工程根>/Assets，向上去掉 "/Assets" 即工程根。
        /// </summary>
        private static string DefaultOutRoot
        {
            get
            {
                string projRoot = Application.dataPath.Replace("/Assets", string.Empty);
                return projRoot + "/Builds";
            }
        }
        #endregion

        #region Load / Save
        /// <summary>
        /// 从 EditorPrefs 加载全部设置（缺省 key 时回退到工程默认值）
        /// </summary>
        public static void Load()
        {
            CurrentPlatform = (BuildTarget)EditorPrefs.GetInt(KeyCurrentPlatform, (int)BuildTarget.Android);
            Incremental = EditorPrefs.GetBool(KeyIncremental, false);
            LuaEncrypt = EditorPrefs.GetBool(KeyLuaEncrypt, false);
            AppVersion = EditorPrefs.GetString(KeyAppVersion, PlayerSettings.bundleVersion);
            BundleVersionCode = EditorPrefs.GetInt(KeyBundleVersionCode, PlayerSettings.Android.bundleVersionCode);
            DevelopmentBuild = EditorPrefs.GetBool(KeyDevelopmentBuild, false);
            AppName = EditorPrefs.GetString(KeyAppName, PlayerSettings.productName);
            PackageName = EditorPrefs.GetString(KeyPackageName, PlayerSettings.applicationIdentifier);
            OutRoot = EditorPrefs.GetString(KeyOutRoot, DefaultOutRoot);
            Scenes = EditorPrefs.GetString(KeyScenes, DefaultScenes);
            KeystorePath = EditorPrefs.GetString(KeyKeystorePath, DefaultKeystorePath);
            KeystorePass = EditorPrefs.GetString(KeyKeystorePass, string.Empty);
            KeyAliasName = EditorPrefs.GetString(KeyKeyAliasName, DefaultKeyAliasName);
            KeyAliasPass = EditorPrefs.GetString(KeyKeyAliasPass, string.Empty);
        }

        /// <summary>
        /// 把当前全部设置写回 EditorPrefs（BuildBegin 与窗口确认时调用）
        /// </summary>
        public static void Save()
        {
            EditorPrefs.SetInt(KeyCurrentPlatform, (int)CurrentPlatform);
            EditorPrefs.SetBool(KeyIncremental, Incremental);
            EditorPrefs.SetBool(KeyLuaEncrypt, LuaEncrypt);
            EditorPrefs.SetString(KeyAppVersion, AppVersion ?? string.Empty);
            EditorPrefs.SetInt(KeyBundleVersionCode, BundleVersionCode);
            EditorPrefs.SetBool(KeyDevelopmentBuild, DevelopmentBuild);
            EditorPrefs.SetString(KeyAppName, AppName ?? string.Empty);
            EditorPrefs.SetString(KeyPackageName, PackageName ?? string.Empty);
            EditorPrefs.SetString(KeyOutRoot, OutRoot ?? string.Empty);
            EditorPrefs.SetString(KeyScenes, Scenes ?? string.Empty);
            EditorPrefs.SetString(KeyKeystorePath, KeystorePath ?? string.Empty);
            EditorPrefs.SetString(KeyKeystorePass, KeystorePass ?? string.Empty);
            EditorPrefs.SetString(KeyKeyAliasName, KeyAliasName ?? string.Empty);
            EditorPrefs.SetString(KeyKeyAliasPass, KeyAliasPass ?? string.Empty);
        }
        #endregion
    }
}
