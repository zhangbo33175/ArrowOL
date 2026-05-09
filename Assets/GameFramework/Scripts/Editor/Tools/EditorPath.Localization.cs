using UnityEngine;

namespace Honor.Editor
{
    /// <summary>
    /// 编辑器路径配置（静态全局类）
    /// 专门存放【多语言系统】的所有路径常量
    /// 作用：统一管理多语言 Excel、导出目录，避免路径散写在代码中
    /// </summary>
    public static partial class EditorPath
    {
        /// <summary>
        /// 多语言（Localization）模块专用路径配置
        /// </summary>
        public static class Localization
        {
            /// <summary>
            /// 多语言导出的 Lua 脚本存放目录（最终 Lua 代码）
            /// 例：多语言文本表
            /// </summary>
            public static string LuaFolderFullPath = $"{Application.dataPath}/LuaScripts/Game/Localizations";

            /// <summary>
            /// 多语言 Excel 配置表根目录（策划填写的表）
            /// </summary>
            public static string ExcelFolderFullPath = $"{Application.dataPath}/../Docs/Designs/Excels/Localizations";

            /// <summary>
            /// 主多语言 Excel 路径（Localizations.xlsm）
            /// </summary>
            public static string ExcelFileFullPath = $"{Application.dataPath}/../Docs/Designs/Excels/Localizations/Localizations.xlsm";

            /// <summary>
            /// 默认语言 Excel 路径（LocalizationsDefault.xlsm）
            /// </summary>
            public static string ExcelDefaultFileFullPath = $"{Application.dataPath}/../Docs/Designs/Excels/Localizations/LocalizationsDefault.xlsm";

            /// <summary>
            /// 多语言字体配置表路径（LocalizationFonts.xlsm）
            /// 用于配置不同语言使用什么字体
            /// </summary>
            public static string ExcelFontFileFullPath = $"{Application.dataPath}/../Docs/Designs/Excels/Localizations/LocalizationFonts.xlsm";

            /// <summary>
            /// 多语言增量导出目录（用于增量更新、热更语言包）
            /// </summary>
            public static string LuaIncreaseFolder = $"{Application.dataPath}/LuaScripts/Game/LocalizationsIncrease";

            /// <summary>
            /// 增量文件夹名称（给编辑器工具显示用）
            /// </summary>
            public static string LuaIncreaseFolderName = "LocalizationsIncrease";

            /// <summary>
            /// Lua 脚本根目录
            /// </summary>
            public static string LuaScriptsFolderFullPath = $"{Application.dataPath}/LuaScripts";
        }  
    } 
}