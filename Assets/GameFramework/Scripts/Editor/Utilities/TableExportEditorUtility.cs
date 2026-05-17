/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Game
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  TableExportEditorUtility.cs
 * author:    云毅
 *  created:   2026
 * descrip:   Honor框架 配置表导出工具类
 *            功能：Excel转Lua/Json、数据校验、加密导出、注释生成、自动注册
 ***************************************************************/

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
    #region 表格导出工具类
    /// <summary>
    /// 【表格导出工具类】
    /// 功能：读取 Excel(.xlsm/.csv)，自动导出为 Lua 配置表 / Json 配置表
    /// 支持：数据校验、加密导出、注释生成、注册文件自动刷新
    /// 属于：游戏框架 -> 配置表系统 -> 编辑器导出工具
    /// </summary>
    public static class TableExportEditorUtility
    {
        #region 文件夹操作
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
        /// <param name="directoryPath">文件夹路径</param>
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
        #endregion

        #region Excel 文件操作
        /// <summary>
        /// 打开 Excel 文件（仅限 .xlsm）
        /// </summary>
        /// <param name="excelPath">Excel文件路径</param>
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
        /// 读取 Excel 文件，返回 DataSet
        /// 支持 .xlsm / .csv
        /// </summary>
        /// <param name="excelAbsolutePath">Excel绝对路径</param>
        /// <returns>Excel数据集</returns>
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
        #endregion

        #region 路径与命名处理
        /// <summary>
        /// 判断路径是否在 Lua 脚本目录
        /// </summary>
        /// <param name="checkPath">待检测路径</param>
        /// <returns>是否为Lua目录</returns>
        public static bool IsDirectoryInLuaDirectory(string checkPath)
        {
            if (string.IsNullOrEmpty(checkPath)) return false;
            checkPath = checkPath.Replace("\\", "/");
            return checkPath.Contains("Assets/Framework/LuaScripts/") || checkPath.Contains("Assets/LuaScripts/Game/");
        }

        /// <summary>
        /// 获取表格名（去掉 Table 前缀 + 后缀）
        /// 例：TableItem.xlsm → Item
        /// </summary>
        /// <param name="excelFileName">Excel文件名</param>
        /// <returns>格式化表格名</returns>
        public static string GetExcelName(string excelFileName)
        {
            if (string.IsNullOrEmpty(excelFileName)) return null;
            string name = Path.GetFileNameWithoutExtension(excelFileName);
            if (name.StartsWith("Table")) name = name.Substring(5);
            return name;
        }
        #endregion

        #region 加密与表定义
        /// <summary>
        /// 判断表格是否需要加密（包含 [encrypt] 标记）
        /// </summary>
        /// <param name="desc">表格描述</param>
        /// <returns>是否需要加密</returns>
        public static bool IsTableNeedEncrypt(string desc)
        {
            return !string.IsNullOrEmpty(desc) && desc.Contains("[encrypt]");
        }

        /// <summary>
        /// 获取 Lua 表名：Tables.XXX
        /// </summary>
        /// <param name="excelFileName">Excel文件名</param>
        /// <param name="needChange">是否格式化名称</param>
        /// <returns>Lua表名</returns>
        public static string GetLuaTableDefine(string excelFileName, bool needChange = false)
        {
            string name = needChange ? GetExcelName(excelFileName) : excelFileName;
            return $"Tables.{name}";
        }

        /// <summary>
        /// 获取 Lua 行结构名：Tables.XXX_Item
        /// </summary>
        /// <param name="excelFileName">Excel文件名</param>
        /// <param name="needChange">是否格式化名称</param>
        /// <returns>Lua行结构名</returns>
        public static string GetLuaTableItemDefine(string excelFileName, bool needChange = false)
        {
            string name = needChange ? GetExcelName(excelFileName) : excelFileName;
            return $"Tables.{name}_Item";
        }
        #endregion

        #region Lua 代码生成
        /// <summary>
        /// 生成导出 Lua 文件头部注释（版权、说明、类型）
        /// </summary>
        /// <param name="excelFileName">Excel文件名</param>
        /// <param name="desc">表格描述</param>
        /// <returns>注释字符串</returns>
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
        /// <param name="tableName">表名</param>
        /// <param name="itemType">行结构类型</param>
        /// <returns>元表代码</returns>
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
        #endregion

        #region 类型转换与校验
        /// <summary>
        /// Excel 类型 → Lua 类型
        /// </summary>
        /// <param name="typeStr">Excel类型</param>
        /// <returns>Lua类型</returns>
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
        /// <param name="str">原始字符串</param>
        /// <returns>清理后字符串</returns>
        public static string DelNewLineFlag(string str)
        {
            if (string.IsNullOrEmpty(str)) return " ";
            return str.Replace("\r\n", " ").Replace("\n", " ").Replace("\r", " ");
        }

        /// <summary>
        /// 把 Excel 单元格内容转为 Lua 字面量
        /// </summary>
        /// <param name="value">单元格值</param>
        /// <param name="type">数据类型</param>
        /// <returns>Lua格式值</returns>
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
                "vector3" => $"Unity.Vector3({value})",
                "vector4" => $"UnityVector4({value})",
                _ => value
            };
        }

        /// <summary>
        /// 检查 Excel 填写内容是否符合类型规则
        /// </summary>
        /// <param name="defType">定义类型</param>
        /// <param name="content">单元格内容</param>
        /// <returns>是否合法</returns>
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
        /// <param name="str">待检测字符串</param>
        /// <returns>是否为数字</returns>
        public static bool CheckStringIsNumber(string str)
        {
            if (string.IsNullOrEmpty(str)) return false;
            Regex numReg = new Regex(@"^[-+]?(\d+(\.\d*)?|\.\d+)$");
            return numReg.IsMatch(str);
        }

        /// <summary>
        /// 检查是否是合法 Vector (x,y,z,w)
        /// </summary>
        /// <param name="str">向量字符串</param>
        /// <param name="count">分量数量</param>
        /// <returns>是否合法</returns>
        public static bool CheckVector(string str, int count)
        {
            var parts = str.Split(',');
            return parts.Length == count && parts.All(x => CheckStringIsNumber(x.Trim()));
        }
        #endregion

        #region 注册文件刷新
        /// <summary>
        /// 刷新 Tables.lua 注册文件（自动懒加载）
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="desc">表格描述</param>
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
        #endregion

        #region 核心导出：Excel -> Lua
        /// <summary>
        /// 【核心导出】Excel → Lua
        /// 支持：类型检查、加密、注释、自动注册
        /// </summary>
        /// <param name="excelPath">Excel路径</param>
        /// <param name="luaPath">导出Lua路径</param>
        /// <returns>是否导出成功</returns>
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
        #endregion

        #region 导出：Excel -> Json
        /// <summary>
        /// 导出 Excel → 普通 Json
        /// </summary>
        /// <param name="exclePath">Excel绝对路径</param>
        /// <param name="jsonPath">Json导出路径</param>
        /// <param name="EncrytionKey">加密密钥</param>
        /// <returns>是否导出成功</returns>
        public static bool ExportExcelToJson(string exclePath, string jsonPath, byte[] EncrytionKey = null)
        {
            if (string.IsNullOrEmpty(jsonPath))
            {
                Log.Error("Excel, 导出路径为空，导出json文件失败");
                return false;
            }
            bool exportToEncryteJson = (EncrytionKey != null);
            string jsonExtension = Path.GetExtension(jsonPath);
            
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

            // 获取Excel数据
            DataSet result = GetExcelData(exclePath);

            // 构建Json内容
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
                if (cellKey[keyi] != string.Empty && typeList[keyi] == string.Empty)
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
                            thisRow += "\"" + cellKey[j] + "\":" + cellValue;
                        }
                        else if (typeList[j] == "string" || typeList[j] == "table")
                        {
                            thisRow += "\"" + cellKey[j] + "\":" + "\"" + cellValue + "\"";
                        }
                        else if (typeList[j] == "boolean")
                        {
                            thisRow += "\"" + cellKey[j] + "\":" + cellValue;
                        }
                        
                        if (j < columns - 1)
                        {
                            thisRow += ",";
                        }
                    }
                }
                
                thisRow += "}";
                if (i < rows - 1)
                {
                    thisRow += ",";
                }
                stringBuilder.AppendLine(thisRow);
            }
            
            stringBuilder.AppendLine("}");

            // 写入文件
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
                File.WriteAllText(jsonPath, stringBuilder.ToString(), new UTF8Encoding(false));
            }
            
            Runtime.Log.Info($"{exclePath} 导出 {jsonPath}完成");
            AssetDatabase.Refresh();

            return true;
        }

        /// <summary>
        /// 导出表格数据到json文件，格式为JArray
        /// </summary>
        /// <param name="exclePath">Excel绝对路径</param>
        /// <param name="jsonPath">Json导出路径</param>
        /// <param name="EncrytionKey">加密密钥</param>
        /// <returns>是否导出成功</returns>
        public static bool ExportExcelToJsonArrayList(string exclePath, string jsonPath, byte[] EncrytionKey = null)
        {
            if (string.IsNullOrEmpty(jsonPath))
            {
                Log.Error("Excel, 导出路径为空，导出json文件失败");
                return false;
            }
            
            bool exportToEncryteJson = (EncrytionKey != null);
            string jsonExtension = Path.GetExtension(jsonPath);
            
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

            // 获取Excel数据
            DataSet result = GetExcelData(exclePath);

            // 构建Json数组内容
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
                if (cellKey[keyi] != string.Empty && typeList[keyi] == string.Empty)
                {
                    Runtime.Log.Error(exclePath + "表格错误, " + cellKey[keyi] + " 没有类型");
                    return false;
                }
            }
            
            for (int i = 4; i < rows; i++)
            {
                string thisRow = "  {";
                
                for (int j = 1; j < columns; j++)
                {
                    if (cellKey[j] != string.Empty)
                    {
                        string cellValue = result.Tables[0].Rows[i][j].ToString();
                        if (typeList[j] == "number")
                        {
                            thisRow += "\"" + cellKey[j] + "\":" + cellValue;
                        }
                        else if (typeList[j] == "string" || typeList[j] == "table")
                        {
                            thisRow += "\"" + cellKey[j] + "\":" + "\"" + cellValue + "\"";
                        }
                        else if (typeList[j] == "boolean")
                        {
                            thisRow += "\"" + cellKey[j] + "\":" + cellValue;
                        }
                        
                        if (j < columns - 1)
                        {
                            thisRow += ",";
                        }
                    }
                }
                
                thisRow += "}";
                if (i < rows - 1)
                {
                    thisRow += ",";
                }
                stringBuilder.AppendLine(thisRow);
            }
            
            stringBuilder.AppendLine("]");

            // 写入文件
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
                File.WriteAllText(jsonPath, stringBuilder.ToString(), new UTF8Encoding(false));
            }
            
            Runtime.Log.Info($"{exclePath} 导出 {jsonPath}完成");
            AssetDatabase.Refresh();

            return true;
        }
        #endregion

        #region TextAsset 创建
        /// <summary>
        /// 使用table导出的string创建asset文件
        /// </summary>
        /// <param name="exportDirectory">存放文件夹</param>
        /// <param name="mulToOne">是否合并到一个文件</param>
        /// <param name="exportOneFileName">合并文件名</param>
        /// <param name="assetChars">文件内容字典</param>
        /// <returns>是否创建成功</returns>
        public static bool CreateTableExportTextAsset(string exportDirectory, bool mulToOne, string exportOneFileName, Dictionary<string, List<string>> assetChars)
        {
            if (string.IsNullOrEmpty(exportDirectory))
            {
                Log.Error("CreateTextAssetFile 时传入的路径为空");
                return false;
            }

            // 路径处理
            string assetFilePath = exportDirectory.Substring(exportDirectory.IndexOf("/Assets") + 1);
            string oneFilePath = $"{assetFilePath}/{exportOneFileName}.asset";
            
            // 删除旧文件
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

            // 导出逻辑
            if (mulToOne)
            {
                StringBuilder tmpChars = new StringBuilder();
                foreach (var item in assetChars)
                {
                    List<string> assetStringArray = item.Value.Distinct().ToList();
                    tmpChars.Append(string.Join("", assetStringArray));
                }
                
                TextAsset instance = new TextAsset(tmpChars.ToString());
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
        #endregion
    }
    #endregion
}