using UnityEngine;

namespace Honor.Editor
{
    public static partial class EditorPath
    {
        public static class Localization
        {
            /// <summary>
            /// 获取Localizations的Lua脚本根目录的绝对路径
            /// </summary>
            public static string LuaFolderFullPath = $"{Application.dataPath}/LuaScripts/Game/Localizations";

            /// <summary>
            /// 获取Localization的Excel根目录的绝对路径
            /// </summary>
            public static string ExcelFolderFullPath = $"{Application.dataPath}/../Docs/Designs/Excels/Localizations";

            /// <summary>
            /// 获取Localizations的Excel文件绝对路径
            /// </summary>
            public static string ExcelFileFullPath = $"{Application.dataPath}/../Docs/Designs/Excels/Localizations/Localizations.xlsm";

            /// <summary>
            /// 获取LocalizationsDefault的Excel文件绝对路径
            /// </summary>
            public static string ExcelDefaultFileFullPath = $"{Application.dataPath}/../Docs/Designs/Excels/Localizations/LocalizationsDefault.xlsm";

            /// <summary>
            /// 获取LocalizationFont的Excel文件绝对路径
            /// </summary>
            public static string ExcelFontFileFullPath = $"{Application.dataPath}/../Docs/Designs/Excels/Localizations/LocalizationFonts.xlsm";

            /// <summary>
            /// 获取导出Lua增量文件导出路径
            /// </summary>
            public static string LuaIncreaseFolder = $"{Application.dataPath}/LuaScripts/Game/LocalizationsIncrease";
            
            /// <summary>
            /// 获取导出Lua增量文件导出路径
            /// </summary>
            public static string LuaIncreaseFolderName = "LocalizationsIncrease";
            
            /// <summary>
            /// 获取Lua脚本根目录的绝对路径
            /// </summary>
            public static string LuaScriptsFolderFullPath = $"{Application.dataPath}/LuaScripts";
        }  
    } 
}