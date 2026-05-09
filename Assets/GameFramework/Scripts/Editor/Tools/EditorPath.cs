using UnityEngine;

namespace Honor.Editor
{
    /// <summary>
    /// 编辑器全局路径配置（partial 分部类）
    /// 作用：统一管理平台名称、Excel 配置表目录、Lua 脚本目录
    /// </summary>
    public static partial class EditorPath
    {
        /// <summary>
        /// 【当前打包平台名称】
        /// 根据宏自动识别：Android / iOS / WebGL
        /// </summary>
#if UNITY_IOS
        public static string PlatformName = "iOS";
#elif UNITY_WEBGL
        public static string PlatformName = "WebGL";
#else
        public static string PlatformName = "Android";
#endif

        /// <summary>
        /// 【Excel 配置表根目录】
        /// 策划配置表总目录（绝对路径）
        /// </summary>
        public static string ExcelFolderFullPath = $"{Application.dataPath}/../Docs/Designs/Excels";

        /// <summary>
        /// 【Lua 脚本路径分类】
        /// 分框架底层 Lua & 游戏业务 Lua
        /// </summary>
        public static class LuaScript
        {
            /// <summary>
            /// 框架底层 Lua（公共模块、工具、核心逻辑）
            /// </summary>
            public static class Framework
            {
                /// <summary>
                /// Lua 根目录（相对路径，用于 AssetDatabase）
                /// </summary>
                public static string FolderPath = "Assets/LuaScripts";

                /// <summary>
                /// Lua 根目录（绝对路径，用于文件读写）
                /// </summary>
                public static string FolderFullPath = $"{Application.dataPath}/LuaScripts";
            }

            /// <summary>
            /// 游戏业务 Lua（UI、流程、配置、逻辑）
            /// </summary>
            public static class Game
            {
                /// <summary>
                /// 游戏 Lua 目录（相对路径）
                /// </summary>
                public static string FolderPath = "Assets/LuaScripts/Game";

                /// <summary>
                /// 游戏 Lua 目录（绝对路径）
                /// </summary>
                public static string FolderFullPath = $"{Application.dataPath}/LuaScripts/Game";
            }
        }
    }
}