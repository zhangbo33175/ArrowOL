using UnityEngine;

namespace Honor.Editor
{
    public static partial class EditorPath
    {
        /// <summary>
        /// Json相关路径信息
        /// </summary>
        public static class Json
        {
            /// <summary>
            /// 获取JSON根目录的绝对路径
            /// </summary>
            public static string FolderFullPath = $"{Application.dataPath}/Game/Jsons";
        }
    }
}