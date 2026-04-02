using UnityEngine;

namespace Honor.Editor
{
    public static partial class EditorPath
    {
        /// <summary>
        /// 平台名称
        /// </summary>
#if UNITY_IOS
        public static string PlatformName = "iOS";
#elif UNITY_WEBGL
        public static string PlatformName = "WebGL";
#else
        public static string PlatformName = "Android";
#endif
        /// <summary>
        /// 获取Excel根目录的绝对路径
        /// </summary>
        public static string ExcelFolderFullPath = $"{Application.dataPath}/../Docs/Designs/Excels";

        /// <summary>
        /// Lua脚本相关路径信息
        /// </summary>
        public static class LuaScript
        {
            public static class Framework
            {
                /// <summary>
                /// 获取Lua脚本根目录的相对路径
                /// </summary>
                public static string FolderPath = "Assets/LuaScripts";

                /// <summary>
                /// 获取Lua脚本根目录的绝对路径
                /// </summary>
                public static string FolderFullPath = $"{Application.dataPath}/LuaScripts";
            }

            public static class Game
            {
                /// <summary>
                /// 获取Lua脚本根目录的相对路径
                /// </summary>
                public static string FolderPath = "Assets/LuaScripts/Game";

                /// <summary>
                /// 获取Lua脚本根目录的绝对路径
                /// </summary>
                public static string FolderFullPath = $"{Application.dataPath}/LuaScripts/Game";
            }
        }
    }
}