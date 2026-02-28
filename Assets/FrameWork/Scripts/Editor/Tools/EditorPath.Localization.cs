/***************************************************************
 * descrip:   编辑器路径实用函数-Localization相关路径信息
 ***************************************************************/

using UnityEngine;

namespace Honor.Editor
{
    public static partial class EditorPath
    {
        /// <summary>
        /// Localization相关路径信息
        /// </summary>
        public static class Localization
        {
            /// <summary>
            /// 获取Localization的Excel根目录的绝对路径
            /// </summary>
            public static string ExcelFolderFullPath = $"{Application.dataPath}/../Docs/Designs/Excels/Localizations";

            /// <summary>
            /// 获取Localizations的Excel文件绝对路径
            /// </summary>
            public static string ExcelFileFullPath =$"{Application.dataPath}/../Docs/Designs/Excels/Localizations/Localizations.xlsm";

            /// <summary>
            /// 获取LocalizationsDefault的Excel文件绝对路径
            /// </summary>
            public static string ExcelDefaultFileFullPath = $"{Application.dataPath}/../Docs/Designs/Excels/Localizations/LocalizationsDefault.xlsm";

            /// <summary>
            /// 获取LocalizationFont的Excel文件绝对路径
            /// </summary>
            public static string ExcelFontFileFullPath = $"{Application.dataPath}/../Docs/Designs/Excels/Localizations/LocalizationFonts.xlsm";
        }
    }
}