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
    }
}