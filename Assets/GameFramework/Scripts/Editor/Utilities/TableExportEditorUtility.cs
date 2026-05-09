using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using ExcelDataReader;
using Honor.Runtime;
using UnityEditor;
using UnityEngine;

namespace Honor.Editor
{
    /// <summary>
    /// 【表格导出工具类】
    /// 功能：读取 Excel(.xlsm/.csv)，自动导出为 Lua 配置表 / Json 配置表
    /// 支持：数据校验、加密导出、注释生成、注册文件自动刷新
    /// 属于：游戏框架 -> 配置表系统 -> 编辑器导出工具
    /// </summary>
    public class TableExportEditorUtility
    {
        /// <summary>
        /// 打开系统文件夹
        /// </summary>
        /// <param name="directoryPath">绝对路径</param>
        public static void OpenDirectory(string directoryPath)
        {
            if (Directory.Exists(directoryPath))
            {
                switch (Application.platform)
                {
                    case RuntimePlatform.WindowsEditor:
                        Process.Start("Explorer.exe", directoryPath.Replace('/', '\\'));
                        break;
                    case RuntimePlatform.OSXEditor:
                        Process.Start("open", directoryPath);
                        break;
                    default:
                        throw new GameException(AorTxt.Format("Not support open folder on '{0}' platform.",
                            Application.platform));
                }
            }
            else
            {
                Log.Info("文件夹 {0} 不存在，无法打开。", directoryPath);
            }
        }

        /// <summary>
        /// 删除文件夹
        /// </summary>
        public static void DeleteDirectory(string directoryPath)
        {
            if (Directory.Exists(directoryPath))
            {
                Directory.Delete(directoryPath, true);
                Log.Info("文件夹 {0} 删除完毕。", directoryPath);
            }
            else
            {
                Log.Info("文件夹 {0} 不存在，无需删除。", directoryPath);
            }
        }

        /// <summary>
        /// 打开 Excel 文件（仅限 .xlsm）
        /// </summary>
        public static void OpenExcel(string excelPath)
        {
            string ext = Path.GetExtension(excelPath);
            if (ext == ".xlsm")
            {
                switch (Application.platform)
                {
                    case RuntimePlatform.WindowsEditor:
                        Process.Start(excelPath);
                        break;
                    case RuntimePlatform.OSXEditor:
                        Process.Start("open", excelPath);
                        break;
                    default:
                        throw new GameException(AorTxt.Format("Not support open file on '{0}' platform.",
                            Application.platform));
                }
            }
            else
            {
                Log.Error("指定的文件不是 xlsm 文件");
            }
        }

        /// <summary>
        /// 判断路径是否在 Lua 脚本目录
        /// </summary>
        public static bool IsDirectoryInLuaDirectory(string checkPath)
        {
            if (string.IsNullOrEmpty(checkPath)) return false;
            checkPath = checkPath.Replace("\\", "/");
            return checkPath.Contains("Assets/Framework/LuaScripts/") || checkPath.Contains("Assets/LuaScripts/Game/");
        }

        /// <summary>
        /// 读取 Excel 文件，返回 DataSet
        /// 支持 .xlsm / .csv
        /// </summary>
        public static DataSet GetExcelData(string excelAbsolutePath)
        {
            if (string.IsNullOrEmpty(excelAbsolutePath))
            {
                Log.Error("Excel 打开路径为 null");
                return null;
            }

            string ext = Path.GetExtension(excelAbsolutePath);
            bool isCsv = ext == ".csv";
            if (ext != ".xlsm" && !isCsv)
            {
                Log.Error("仅支持 xlsm / csv 文件");
                return null;
            }

            FileStream stream = null;
            try
            {
                stream = File.Open(excelAbsolutePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            }
            catch
            {
                Log.Error("表格 {0} 打开失败，请检查是否被占用", excelAbsolutePath);
                throw;
            }

            try
            {
                IExcelDataReader reader = null;
                if (isCsv)
                {
                    var cfg = new ExcelReaderConfiguration();
                    cfg.FallbackEncoding = Encoding.GetEncoding("GB2312");
                    reader = ExcelReaderFactory.CreateCsvReader(stream, cfg);
                }
                else
                {
                    reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                }

                DataSet data = reader?.AsDataSet();
                stream.Close();
                reader.Close();
                return data;
            }
            catch
            {
                Log.Error("Excel {0} 存在无法解析的数据", excelAbsolutePath);
                throw;
            }
        }

        /// <summary>
        /// 获取表格名（去掉 Table 前缀 + 后缀）
        /// 例：TableItem.xlsm → Item
        /// </summary>
        public static string GetExcelName(string excelFileName)
        {
            if (string.IsNullOrEmpty(excelFileName)) return null;
            string name = Path.GetFileNameWithoutExtension(excelFileName);
            if (name.StartsWith("Table")) name = name.Substring(5);
            return name;
        }

        /// <summary>
        /// 判断表格是否需要加密（包含 [encrypt] 标记）
        /// </summary>
        public static bool IsTableNeedEncrypt(string desc)
        {
            return !string.IsNullOrEmpty(desc) && desc.Contains("[encrypt]");
        }

        /// <summary>
        /// 获取 Lua 表名：Tables.XXX
        /// </summary>
        public static string GetLuaTableDefine(string excelFileName, bool needChange = false)
        {
            string name = needChange ? GetExcelName(excelFileName) : excelFileName;
            return $"Tables.{name}";
        }

        /// <summary>
        /// 获取 Lua 行结构名：Tables.XXX_Item
        /// </summary>
        public static string GetLuaTableItemDefine(string excelFileName, bool needChange = false)
        {
            string name = needChange ? GetExcelName(excelFileName) : excelFileName;
            return $"Tables.{name}_Item";
        }

        /// <summary>
        /// 生成导出 Lua 文件头部注释（版权、说明、类型）
        /// </summary>
        public static string GetExcelToLuaDetailInfo(string excelFileName, string desc)
        {
            string table = GetExcelName(excelFileName);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(
                    "--=====================================================================================================")
                .AppendLine("-- (c) copyright 2026 - 2030, Honor.Game")
                .AppendLine("-- All Rights Reserved.")
                .AppendLine(
                    "-- ----------------------------------------------------------------------------------------------------")
                .AppendLine(AorTxt.Format($"-- filename:  Table{table}"))
                .AppendLine(AorTxt.Format($"-- descrip:   {desc}"))
                .AppendLine("-- notices:   该文件自动生成，请不要手动修改！")
                .AppendLine(
                    "--=====================================================================================================")
                .AppendLine("")
                .AppendLine($"---@class Tables.{table}_Item @{desc}条目");
            return sb.ToString();
        }

        /// <summary>
        /// 生成加密表格的元表逻辑（__index / __pairs / __len）
        /// 实现访问时自动解密
        /// </summary>
        public static string GetEncryptTableMetaFunc(string tableName, string itemType)
        {
            string encryptTable = $"table{tableName}Encrypt";
            string realTable = $"Tables.{tableName}";

            StringBuilder sb = new StringBuilder();
            sb.AppendLine()
                .AppendLine($"---@type fun(inTable: table, key: any):{itemType}")
                .AppendLine("local pairfunc = function(inTable, key)")
                .AppendLine($"    local nk, nv = next({encryptTable}, key)")
                .AppendLine("    if nk then")
                .AppendLine($"        nv = {realTable}[nk]")
                .AppendLine("    end")
                .AppendLine("    return nk, nv")
                .AppendLine("end")
                .AppendLine()
                .AppendLine("local mt = { }")
                .AppendLine()
                .AppendLine("mt.__index = function(t, key)")
                .AppendLine($"    if {encryptTable}[key] ~= nil then")
                .AppendLine($"        return AESDecodeFromBase64({encryptTable}[key], \"table\")")
                .AppendLine("    end")
                .AppendLine("end")
                .AppendLine()
                .AppendLine("mt.__len = function()")
                .AppendLine($"    return #{encryptTable}")
                .AppendLine("end")
                .AppendLine()
                .AppendLine("mt.__pairs = function(t)")
                .AppendLine("    return pairfunc, t, nil")
                .AppendLine("end")
                .AppendLine()
                .AppendLine($"setmetatable({realTable}, mt)");
            return sb.ToString();
        }

        /// <summary>
        /// Excel 类型 → Lua 类型
        /// </summary>
        public static string GetTypeFromExcel(string typeStr)
        {
            return typeStr switch
            {
                "color32" => "UnityEngine.Color32",
                "color" => "UnityEngine.Color",
                "datetime" => "System.DateTime",
                "quaternion" => "UnityEngine.Quaternion",
                "rect" => "UnityEngine.Rect",
                "vector2" => "UnityEngine.Vector2",
                "vector3" => "UnityEngine.Vector3",
                "vector4" => "UnityEngine.Vector4",
                "int" or "float" => "number",
                _ => typeStr
            };
        }

        /// <summary>
        /// 清除注释中的换行符
        /// </summary>
        public static string DelNewLineFlag(string str)
        {
            if (string.IsNullOrEmpty(str)) return " ";
            return str.Replace("\r\n", " ").Replace("\n", " ").Replace("\r", " ");
        }

        /// <summary>
        /// 把 Excel 单元格内容转为 Lua 字面量
        /// </summary>
        public static string GetLuaTypeFromExcel(string value, string type)
        {
            if (string.IsNullOrEmpty(value)) return "";
            return type switch
            {
                "string" => $"\"{value}\"",
                "color32" => $"UnityColor32({value})",
                "color" => $"UnityColor({value})",
                "datetime" => $"CS.System.DateTime.Parse(\"{value}\")",
                "quaternion" => $"UnityQuaternion({value})",
                "rect" => $"UnityRect({value})",
                "vector2" => $"UnityVector2({value})",
                "vector3" => $"UnityVector3({value})",
                "vector4" => $"UnityVector4({value})",
                _ => value
            };
        }

        /// <summary>
        /// 检查 Excel 填写内容是否符合类型规则
        /// </summary>
        public static bool CheckLuaTypeInExcel(string defType, string content)
        {
            if (string.IsNullOrEmpty(content)) return true;
            switch (defType)
            {
                case "number":
                    return CheckStringIsNumber(content);
                case "boolean":
                    return content is "true" or "false";
                case "table":
                    return content.StartsWith("{") && content.EndsWith("}");
                case "string":
                    return true;
                case "color32":
                    return CheckVector(content, 4) && content.Split(',').All(x => byte.TryParse(x.Trim(), out _));
                case "color":
                    return CheckVector(content, 4) && content.Split(',').All(x => float.TryParse(x.Trim(), out _));
                case "vector2":
                    return CheckVector(content, 2);
                case "vector3":
                    return CheckVector(content, 3);
                case "vector4":
                    return CheckVector(content, 4);
                case "rect":
                case "quaternion":
                    return CheckVector(content, 4);
                case "datetime":
                    return DateTime.TryParse(content, out _);
                default:
                    return true;
            }
        }

        /// <summary>
        /// 检查字符串是否是合法数字
        /// </summary>
        public static bool CheckStringIsNumber(string str)
        {
            if (string.IsNullOrEmpty(str)) return false;
            Regex numReg = new Regex(@"^[-+]?(\d+(\.\d*)?|\.\d+)$");
            return numReg.IsMatch(str);
        }

        /// <summary>
        /// 检查是否是合法 Vector (x,y,z,w)
        /// </summary>
        public static bool CheckVector(string str, int count)
        {
            var parts = str.Split(',');
            return parts.Length == count && parts.All(x => CheckStringIsNumber(x.Trim()));
        }

        /// <summary>
        /// 刷新 Tables.lua 注册文件（自动懒加载）
        /// </summary>
        public static void RefreshTableRegisterFile(string fileName, string desc)
        {
            string luaDir = GamePathUtils.Table.GetLuaScriptRootDirectoryFullPath();
            string path = Path.Combine(luaDir, "Tables.lua.txt");

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(
                    "--=====================================================================================================")
                .AppendLine("-- (c) copyright 2026 - 2030, GameLib")
                .AppendLine("-- All Rights Reserved.")
                .AppendLine(
                    "-- ----------------------------------------------------------------------------------------------------")
                .AppendLine("-- filename:  Tables.lua")
                .AppendLine("-- descrip:   数据表自动注册")
                .AppendLine("-- notices:   自动生成，禁止修改")
                .AppendLine(
                    "--=====================================================================================================")
                .AppendLine()
                .AppendLine("---@class Tables")
                .AppendLine("Tables = {}")
                .AppendLine("local rawget = rawget")
                .AppendLine(
                    "setmetatable(Tables, { __index = function(t, k) require('Table'..k) return rawget(t, k) end })");

            File.WriteAllText(path, sb.ToString(), new UTF8Encoding(false));
            Log.Info("Tables.lua 注册完成");
        }

        /// <summary>
        /// 【核心导出】Excel → Lua
        /// 支持：类型检查、加密、注释、自动注册
        /// </summary>
        public static bool ExportExcelToLua(string excelPath, string luaPath)
        {
            if (string.IsNullOrEmpty(luaPath)) return false;
            string dir = Path.GetDirectoryName(luaPath);
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

            DataSet data = GetExcelData(excelPath);
            DataTable table = data.Tables[0];
            string shortName = GetExcelName(excelPath);
            string desc = table.Rows[0][1].ToString();
            bool isEncrypt = IsTableNeedEncrypt(desc);
            int colCount = table.Columns.Count;
            int rowCount = table.Rows.Count;

            // 字段/类型数组
            string[] keys = new string[colCount];
            string[] types = new string[colCount];
            for (int i = 1; i < colCount; i++)
            {
                keys[i] = table.Rows[1][i].ToString();
                types[i] = table.Rows[2][i].ToString();
            }

            // 数据校验
            bool hasError = false;
            for (int c = 1; c < colCount; c++)
            {
                if (!string.IsNullOrEmpty(keys[c]) && string.IsNullOrEmpty(types[c]))
                {
                    Log.Error("表格 {0} 缺少类型定义", excelPath);
                    hasError = true;
                }
            }

            for (int r = 4; r < rowCount; r++)
            {
                for (int c = 1; c < colCount; c++)
                {
                    if (!CheckLuaTypeInExcel(types[c], table.Rows[r][c].ToString()))
                    {
                        Log.Error("表格 {0} 单元格格式错误", excelPath);
                        hasError = true;
                    }
                }
            }

            if (hasError) return false;

            // 生成 Lua
            StringBuilder sb = new StringBuilder();
            sb.Append(GetExcelToLuaDetailInfo(excelPath, desc));

            // 生成 EmmyLua 注释
            for (int c = 1; c < colCount; c++)
            {
                if (!string.IsNullOrEmpty(keys[c]))
                {
                    string note = DelNewLineFlag(table.Rows[3][c].ToString());
                    sb.AppendLine($"---@field {keys[c]} {GetTypeFromExcel(types[c])} @{note}");
                }
            }

            sb.AppendLine();
            sb.AppendLine($"---@class {GetLuaTableDefine(shortName)} @{desc}");

            // 生成数据
            if (isEncrypt)
            {
                sb.AppendLine($"Tables.{shortName} = {{ }}");
                sb.AppendLine($"local table{shortName}Encrypt = {{ ");
            }
            else
            {
                sb.AppendLine($"Tables.{shortName} = {{");
            }

            for (int r = 4; r < rowCount; r++)
            {
                string id = GetLuaTypeFromExcel(table.Rows[r][1].ToString(), types[1]);
                string line = "";
                for (int c = 1; c < colCount; c++)
                {
                    if (string.IsNullOrEmpty(keys[c])) continue;
                    string val = GetLuaTypeFromExcel(table.Rows[r][c].ToString(), types[c]);
                    line += string.IsNullOrEmpty(line) ? $"{keys[c]} = {val}" : $", {keys[c]} = {val}";
                }

                if (isEncrypt)
                {
                    string enc = AESEncrypt.EncodeToBase64($"{{{line}}}");
                    sb.AppendLine($"    [{id}] = \"{enc}\",");
                }
                else
                {
                    sb.AppendLine($"    [{id}] = {{{line}}},");
                }
            }

            sb.AppendLine("}");

            // 写入文件
            if (File.Exists(luaPath)) File.Delete(luaPath);
            if (isEncrypt)
            {
                string meta = GetEncryptTableMetaFunc(shortName, GetLuaTableItemDefine(shortName));
                File.WriteAllText(luaPath, sb + meta, new UTF8Encoding(false));
            }
            else
            {
                File.WriteAllText(luaPath, sb.ToString(), new UTF8Encoding(false));
            }

            RefreshTableRegisterFile(Path.GetFileNameWithoutExtension(excelPath), desc);
            AssetDatabase.Refresh();
            Log.Info("导出完成：{0}", luaPath);
            return true;
        }

        /// <summary>
        /// 导出 Excel → 普通 Json
        /// </summary>
        /// <param name="exclePath">需要导出数据的excel表格绝对路径</param>
        /// <param name="jsonPath">要导出的json文件的绝对路径</param>
        /// <returns></returns>
        public static bool ExportExcelToJson(string exclePath, string jsonPath, byte[] EncrytionKey = null)
        {
            if (string.IsNullOrEmpty(jsonPath))
            {
                Log.Error("Excel, 导出路径为空，导出json文件失败");
                return false;
            }
            bool exportToEncryteJson = (EncrytionKey != null) ? true : false;
            string jsonExtension = System.IO.Path.GetExtension(jsonPath);
            if (exportToEncryteJson)
            {
                if (jsonExtension != ".bytes")
                {
                    Log.Error("Excel, 导出加密json文件需要扩展名为.bytes");
                    return false;
                }
            }
            else
            {
                if (jsonExtension != ".json")
                {
                    Log.Error("Excel, 导出json文件需要扩展名为.json");
                    return false;
                }
            }

            // 要打开的excel根路径
            DataSet result = GetExcelData(exclePath);

            // 开始添加文件头
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("{");

            int columns = result.Tables[0].Columns.Count;
            int rows = result.Tables[0].Rows.Count;
            string[] cellKey = new string[columns];
            string[] typeList = new string[columns];
            for (int keyi = 0; keyi < columns; keyi++)
            {
                cellKey[keyi] = result.Tables[0].Rows[1][keyi].ToString();
                typeList[keyi] = result.Tables[0].Rows[2][keyi].ToString();
                if (cellKey[keyi] != string.Empty & typeList[keyi] == string.Empty)
                {
                    Runtime.Log.Error(exclePath + "表格错误, " + cellKey[keyi] + " 没有类型");
                    return false;
                }
            }
            for (int i = 4; i < rows; i++)
            {
                string Key = result.Tables[0].Rows[i][1].ToString();
                string thisRow = "    \"" + Key + "\" : {";
                for (int j = 1; j < columns; j++)
                {
                    if (cellKey[j] != string.Empty)
                    {
                        string cellValue = result.Tables[0].Rows[i][j].ToString();
                        if (typeList[j] == "number")
                        {
                            thisRow = thisRow + "\"" + cellKey[j] + "\":" + cellValue;
                        }
                        else if (typeList[j] == "string" || typeList[j] == "table")
                        {
                            thisRow = thisRow + "\"" + cellKey[j] + "\":" + "\"" + cellValue + "\"";
                        }
                        else if (typeList[j] == "boolean")
                        {
                            thisRow = thisRow + "\"" + cellKey[j] + "\":" + cellValue;
                        }
                        if (j < columns - 1)
                        {
                            thisRow = thisRow + ",";
                        }
                    }
                }
                thisRow = thisRow + "}";
                if (i < rows - 1)
                {
                    thisRow = thisRow + ",";
                }
                stringBuilder.AppendLine(thisRow);
            }
            stringBuilder.AppendLine("}");
            // 开始写入数据
            if (File.Exists(jsonPath))
            {
                File.Delete(jsonPath);
            }
            if (exportToEncryteJson)
            {
                File.WriteAllBytes(jsonPath, Encryption.GetQuickXorBytes(Converter.GetBytesByString(stringBuilder.ToString()), EncrytionKey));
            }
            else
            {
                File.WriteAllText(jsonPath, stringBuilder.ToString(), new System.Text.UTF8Encoding(false));
            }
            Runtime.Log.Info($"{exclePath} 导出 {jsonPath}完成");

            AssetDatabase.Refresh();

            return true;
        }

        /// <summary>
        /// 导出表格数据到json文件，格式为JArray
        /// </summary>
        /// <param name="exclePath">需要导出数据的excel表格绝对路径</param>
        /// <param name="jsonPath">要导出的json文件的绝对路径</param>
        /// <returns></returns>
        public static bool ExportExcelToJsonArrayList(string exclePath, string jsonPath, byte[] EncrytionKey = null)
        {
            if (string.IsNullOrEmpty(jsonPath))
            {
                Log.Error("Excel, 导出路径为空，导出json文件失败");
                return false;
            }
            bool exportToEncryteJson = (EncrytionKey != null) ? true : false;
            string jsonExtension = System.IO.Path.GetExtension(jsonPath);
            if (exportToEncryteJson)
            {
                if (jsonExtension != ".bytes")
                {
                    Log.Error("Excel, 导出加密json文件需要扩展名为.bytes");
                    return false;
                }
            }
            else
            {
                if (jsonExtension != ".json")
                {
                    Log.Error("Excel, 导出json文件需要扩展名为.json");
                    return false;
                }
            }

            // 要打开的excel根路径
            DataSet result = GetExcelData(exclePath);

            // 开始添加文件头
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("[");

            int columns = result.Tables[0].Columns.Count;
            int rows = result.Tables[0].Rows.Count;
            string[] cellKey = new string[columns];
            string[] typeList = new string[columns];
            for (int keyi = 0; keyi < columns; keyi++)
            {
                cellKey[keyi] = result.Tables[0].Rows[1][keyi].ToString();
                typeList[keyi] = result.Tables[0].Rows[2][keyi].ToString();
                if (cellKey[keyi] != string.Empty & typeList[keyi] == string.Empty)
                {
                    Runtime.Log.Error(exclePath + "表格错误, " + cellKey[keyi] + " 没有类型");
                    return false;
                }
            }
            for (int i = 4; i < rows; i++)
            {
                string Key = result.Tables[0].Rows[i][1].ToString();
                string thisRow = "  {";
                for (int j = 1; j < columns; j++)
                {
                    if (cellKey[j] != string.Empty)
                    {
                        string cellValue = result.Tables[0].Rows[i][j].ToString();
                        if (typeList[j] == "number")
                        {
                            thisRow = thisRow + "\"" + cellKey[j] + "\":" + cellValue;
                        }
                        else if (typeList[j] == "string" || typeList[j] == "table")
                        {
                            thisRow = thisRow + "\"" + cellKey[j] + "\":" + "\"" + cellValue + "\"";
                        }
                        else if (typeList[j] == "boolean")
                        {
                            thisRow = thisRow + "\"" + cellKey[j] + "\":" + cellValue;
                        }
                        if (j < columns - 1)
                        {
                            thisRow = thisRow + ",";
                        }
                    }
                }
                thisRow = thisRow + "}";
                if (i < rows - 1)
                {
                    thisRow = thisRow + ",";
                }
                stringBuilder.AppendLine(thisRow);
            }
            stringBuilder.AppendLine("]");
            // 开始写入数据
            if (File.Exists(jsonPath))
            {
                File.Delete(jsonPath);
            }
            if (exportToEncryteJson)
            {
                File.WriteAllBytes(jsonPath, Encryption.GetQuickXorBytes(Converter.GetBytesByString(stringBuilder.ToString()), EncrytionKey));
            }
            else
            {
                File.WriteAllText(jsonPath, stringBuilder.ToString(), new System.Text.UTF8Encoding(false));
            }
            Runtime.Log.Info($"{exclePath} 导出 {jsonPath}完成");

            AssetDatabase.Refresh();

            return true;
        }

        /// <summary>
        /// 使用table导出的string创建asset文件
        /// </summary>
        /// <param name="exportDirectory">创建asset文件需要存放的文件夹</param>
        /// <param name="mulToOne">是否将assetChars的内容导出到一个文件</param>
        /// <param name="exportOneFileName">是否将assetChars的内容导出到一个文件</param>
        /// <param name="assetChars">Dictionary<string, List<string>> 要导出的文件名/导出文件内容</param>
        public static bool CreateTableExportTextAsset(string exportDirectory, bool mulToOne, string exportOneFileName, Dictionary<string, List<string>> assetChars)
        {
            // 检查传入路径是否是以.asset后缀名结尾
            if (string.IsNullOrEmpty(exportDirectory))
            {
                Log.Error("CreateTextAssetFile 时传入的路径为空");
                return false;
            }
            // 删除当前文件夹下面原有的asset文件
            string assetFilePath = exportDirectory.Substring(exportDirectory.IndexOf("/Assets") + 1);
            string oneFilePath = $"{assetFilePath}/{exportOneFileName}.asset";
            if (File.Exists(oneFilePath))
            {
                Log.Info($"CreateTextAssetFile 删除旧的TMP字符集文件{oneFilePath}");
                File.Delete(oneFilePath);
            }
            foreach (var item in assetChars)
            {
                string perFilePath = $"{assetFilePath}/{item.Key}.asset";
                if (File.Exists(perFilePath))
                {
                    Log.Info($"CreateTextAssetFile 删除旧的TMP字符集文件{perFilePath}");
                    File.Delete(perFilePath);
                }
            }

            if (mulToOne)
            {
                string tmpChars = "";
                foreach (var item in assetChars)
                {
                    // 对要制作字库的文字内容进行去重
                    List<string> assetStringArray = item.Value.Distinct().ToList();
                    tmpChars = $"{tmpChars}{string.Join("", assetStringArray)}";
                }
                TextAsset instance = new TextAsset(tmpChars);
                instance.name = "exportFileName";
                AssetDatabase.CreateAsset(instance, oneFilePath);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
            else
            {
                foreach (var item in assetChars)
                {
                    string perFilePath = $"{assetFilePath}/{item.Key}.asset";
                    List<string> assetStringArray = item.Value.Distinct().ToList();
                    TextAsset instance = new TextAsset(string.Join("", assetStringArray));
                    instance.name = item.Key;
                    AssetDatabase.CreateAsset(instance, perFilePath);
                    AssetDatabase.SaveAssets();
                }
                AssetDatabase.Refresh();
            }
            return true;
        }
    }
}