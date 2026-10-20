/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Editor
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  LocalizationComponentInspector.Export.cs
 * author:    云毅
 * created:   2026
 * descrip:   LocalizationComponentInspector 静态导出方法分部
 ***************************************************************/

using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using GameLib;
using Honor.Runtime;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;

namespace Honor.Editor
{
    internal sealed partial class LocalizationComponentInspector
    {
        private static readonly List<string> m_AllKey = new List<string>();
        private static readonly List<string> m_RepeatKey = new List<string>();

        public static void LoadLocalizationSettings(out bool enableExportLuaIncrease,
            out bool enableExportLuaIncreaseSingleVersion, out string exportLuaIncreasePath)
        {
            enableExportLuaIncrease = false;
            enableExportLuaIncreaseSingleVersion = false;
            exportLuaIncreasePath = string.Empty;
            // TODO: 加固健壮性——设置文件首次运行可能尚不存在，原 File.ReadAllText 会抛异常，缺失时走默认值
            string settingPath = EditorPath.Editor.Localization.ProjectSettingFileFullPath;
            if (!File.Exists(settingPath))
            {
                exportLuaIncreasePath = EditorPath.Localization.LuaIncreaseFolderName;
                return;
            }

            string content = File.ReadAllText(settingPath);
            JObject j = JObject.Parse(content);
            if (j.ContainsKey("EnableExportLuaIncrease")) enableExportLuaIncrease = (bool)j["EnableExportLuaIncrease"];
            if (j.ContainsKey("EnableExportLuaIncreaseSingleVersion")) enableExportLuaIncreaseSingleVersion = (bool)j["EnableExportLuaIncreaseSingleVersion"];
            if (j.ContainsKey("ExportLuaIncreasePath")) exportLuaIncreasePath = (string)j["ExportLuaIncreasePath"];
            if (string.IsNullOrEmpty(exportLuaIncreasePath)) exportLuaIncreasePath = EditorPath.Localization.LuaIncreaseFolderName;
        }

        public static void SaveLocalizationSettings(bool enableExportLuaIncrease,
            bool enableExportLuaIncreaseSingleVersion, string exportLuaIncreasePath)
        {
            JObject j = new JObject();
            j["EnableExportLuaIncrease"] = enableExportLuaIncrease;
            j["EnableExportLuaIncreaseSingleVersion"] = enableExportLuaIncreaseSingleVersion;
            j["ExportLuaIncreasePath"] = exportLuaIncreasePath;
            File.WriteAllText(EditorPath.Editor.Localization.ProjectSettingFileFullPath, j.ToString());
        }

        public static bool ExportExcelFontsToJson()
        {
            string name = Path.GetFileNameWithoutExtension(EditorPath.Localization.ExcelFontFileFullPath);
            string excelPath = $"{EditorPath.Localization.ExcelFolderFullPath}/{name}.xlsm";
            string jsonPath = EditorPath.Json.FolderFullPath + "/" + name + ".json";
            if (Directory.Exists(EditorPath.Json.FolderFullPath) && File.Exists(jsonPath))
            {
                File.Delete(jsonPath);
                File.Delete(jsonPath + ".meta");
            }
            else Directory.CreateDirectory(EditorPath.Json.FolderFullPath);
            return TableExportEditorUtility.ExportExcelToJson(excelPath, jsonPath);
        }

        public static bool ExportExcelDefaultLanguageToJson()
        {
            string name = Path.GetFileNameWithoutExtension(EditorPath.Localization.ExcelDefaultFileFullPath);
            string excelPath = $"{EditorPath.Localization.ExcelFolderFullPath}/{name}.xlsm";
            DataSet result = TableExportEditorUtility.GetExcelData(excelPath);
            string root = EditorPath.Json.FolderFullPath;
            if (Directory.Exists(root))
            {
                foreach (var f in Directory.GetFiles(root, "LocalizationDefault*.json", SearchOption.AllDirectories))
                { File.Delete(f); File.Delete(f + ".meta"); }
            }
            else Directory.CreateDirectory(root);

            int columns = result.Tables[0].Columns.Count;
            int rows = result.Tables[0].Rows.Count;
            var keyList = new string[columns];
            var typeList = new string[columns];
            for (int c = 1; c < columns; c++)
            {
                keyList[c] = result.Tables[0].Rows[1][c].ToString();
                typeList[c] = result.Tables[0].Rows[2][c].ToString();
            }

            var main = new StringBuilder("[");
            for (int c = 6; c < columns; c++)
            {
                if (c > 6) main.Append(",");
                main.Append("\"" + keyList[c] + "\"");
                var sb = new StringBuilder();
                sb.AppendLine("{");
                for (int r = 4; r < rows; r++)
                {
                    string key = result.Tables[0].Rows[r][1]?.ToString();
                    if (string.IsNullOrEmpty(key)) continue;
                    string val = result.Tables[0].Rows[r][c]?.ToString() ?? "";
                    string line = typeList[c] == "table"
                        ? $"    \"{key}\" : {val}"
                        : $"    \"{key}\" : \"{val}\"";
                    if (r < rows - 1) line += ",";
                    sb.AppendLine(line);
                }
                sb.AppendLine("}");
                string fp = root + "/LocalizationDefault" + keyList[c] + ".json";
                if (File.Exists(fp)) File.Delete(fp);
                File.WriteAllText(fp, sb.ToString(), new UTF8Encoding(false));
            }
            main.AppendLine("]");
            string mainFp = root + "/LocalizationDefaultLanguages.json";
            if (File.Exists(mainFp)) File.Delete(mainFp);
            File.WriteAllText(mainFp, main.ToString(), new UTF8Encoding(false));
            AssetDatabase.Refresh();
            return true;
        }

        public static bool ExportAllExcelsLanguageToLua(List<string> customExcels,
            bool enableIncrease = false, bool singleVersion = false, string increasePath = "")
        {
            m_AllKey.Clear();
            m_RepeatKey.Clear();
            CheckDefaultConfigKeyRepeat();

            if (!enableIncrease)
            {
                foreach (var p in Directory.GetFiles(Application.dataPath, "Localization*.lua.txt", SearchOption.AllDirectories))
                    File.Delete(p);
                foreach (var p in Directory.GetFiles(Application.dataPath, "LocalPart_*.lua.txt", SearchOption.AllDirectories))
                    File.Delete(p);
            }

            var fullPaths = new List<string> { EditorPath.Localization.ExcelFileFullPath };
            var fileNames = new List<string> { Path.GetFileNameWithoutExtension(EditorPath.Localization.ExcelFileFullPath) };
            foreach (var c in customExcels)
            {
                string n = c.Replace("\\", "/");
                fullPaths.Add(n);
                fileNames.Add(Path.GetFileNameWithoutExtension(n));
            }

            DataSet mainDs = TableExportEditorUtility.GetExcelData(EditorPath.Localization.ExcelFileFullPath);
            string detail = mainDs.Tables[0].Rows[0][1].ToString();
            int columns = mainDs.Tables[0].Columns.Count;
            var keyList = new string[columns];
            var langInfo = new string[columns];
            for (int c = 0; c < columns; c++)
            {
                keyList[c] = mainDs.Tables[0].Rows[1][c]?.ToString() ?? "";
                langInfo[c] = mainDs.Tables[0].Rows[3][c]?.ToString() ?? "";
            }

            var exportPaths = new Dictionary<string, string> { ["Main"] = EditorPath.Localization.LuaFolderFullPath };
            for (int c = 6; c < columns; c++)
                if (!string.IsNullOrEmpty(keyList[c]) && !exportPaths.ContainsKey(keyList[c]))
                    exportPaths[keyList[c]] = EditorPath.Localization.LuaFolderFullPath;
            foreach (var p in exportPaths)
                if (!Directory.Exists(p.Value)) Directory.CreateDirectory(p.Value);

            // 简化：直接生成每语言 Lua
            for (int c = 6; c < columns; c++)
            {
                string lang = keyList[c];
                if (string.IsNullOrEmpty(lang)) continue;
                string dir = exportPaths[lang] + "/Localization" + lang;
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                var sb = new StringBuilder();
                sb.AppendLine("--=====================================================================================================");
                sb.AppendLine("-- (c) copyright 2026 - 2030, Honor");
                sb.AppendLine("-- notices:   该文件自动生成，请不要手动修改！");
                sb.AppendLine("--=====================================================================================================");
                sb.AppendLine($"Localizations.{lang} = {{}}");

                for (int fi = 0; fi < fullPaths.Count; fi++)
                {
                    DataSet ds = TableExportEditorUtility.GetExcelData(fullPaths[fi]);
                    int rows = ds.Tables[0].Rows.Count;
                    sb.AppendLine($"-- From {fileNames[fi]}");
                    for (int r = 4; r < rows; r++)
                    {
                        string k = ds.Tables[0].Rows[r][1]?.ToString();
                        string v = ds.Tables[0].Rows[r][c]?.ToString() ?? "";
                        if (string.IsNullOrEmpty(k)) continue;
                        sb.AppendLine($"Localizations.{lang}.{k} = \"{v}\"");
                    }
                }
                string fp = $"{dir}/Localization{lang}.lua.txt";
                File.WriteAllText(fp, sb.ToString(), new UTF8Encoding(false));
            }

            if (m_RepeatKey.Count > 0)
            {
                string res = string.Join("、", m_RepeatKey);
                EditorUtility.DisplayDialog("检查到配置表Key重复", $"重复key:{res}", "确定");
            }

            AssetDatabase.Refresh();
            return true;
        }

        private static void CheckDefaultConfigKeyRepeat()
        {
            DataSet ds = TableExportEditorUtility.GetExcelData(EditorPath.Localization.ExcelDefaultFileFullPath);
            for (int i = 4; i < ds.Tables[0].Rows.Count; i++)
            {
                var k = ds.Tables[0].Rows[i][1]?.ToString();
                if (!string.IsNullOrEmpty(k)) CollectAndCheckKeyRepeat(k);
            }
        }

        internal static void MakeLuaFileTitle(StringBuilder sb, string fileName, string keyName, string descrip)
        {
            sb.AppendLine("--=====================================================================================================")
              .AppendLine("-- (c) copyright 2026 - 2030, Honor")
              .AppendLine("-- All Rights Reserved.")
              .AppendLine("-- notices:   该文件自动生成，请不要手动修改！")
              .AppendLine("--=====================================================================================================")
              .AppendLine("");
        }
    }
}