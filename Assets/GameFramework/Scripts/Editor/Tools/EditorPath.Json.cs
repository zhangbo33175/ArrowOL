using UnityEngine;

namespace Honor.Editor
{
    public static partial class EditorPath
    {
        /// <summary>
        /// 【Json 配置路径】
        /// 管理框架中所有 Json 文件的导出/读取路径
        /// </summary>
        public static class Json
        {
            /// <summary>
            /// Json 配置文件根目录（绝对路径）
            /// 存放从 Excel 导出的 Lua/Json 配置表
            /// </summary>
            public static string FolderFullPath = $"{Application.dataPath}/LuaScripts/Config/LuaJson";
        }
    }
}