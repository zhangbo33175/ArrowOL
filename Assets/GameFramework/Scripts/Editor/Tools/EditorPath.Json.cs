/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Game
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  EditorPath.Json.cs
 * author:    云毅
 *  created:   2026
 * descrip:   Honor框架 编辑器路径配置 - Json配置表路径
 ***************************************************************/

using UnityEngine;

namespace Honor.Editor
{
    #region 编辑器全局路径配置
    public static partial class EditorPath
    {
        #region Json 配置路径
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
        #endregion
    }
    #endregion
}