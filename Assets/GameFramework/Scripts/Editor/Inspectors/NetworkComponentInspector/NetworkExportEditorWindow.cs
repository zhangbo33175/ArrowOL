/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Editor
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  NetworkExportEditorWindow.cs
 * author:    云毅
 * created:   2026
 * descrip:   网络协议导出工具窗口（Honor 菜单入口）
 ***************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Honor.Runtime;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Honor.Editor
{
    /// <summary>
    /// 网络协议导出工具窗口。Honor → 网络协议工具。
    /// </summary>
    public class NetworkExportEditorWindow : BaseToolkitEditorWindow<NetworkExportEditorWindow>
    {
        #region 字段
        private static bool s_IsCurEnum;
        private static readonly List<string> s_CurClsName = new List<string>();
        private static readonly List<string> m_EnumFields = new List<string>();
        private static readonly List<string> m_EnumMembers = new List<string>();
        #endregion

        #region 基类抽象属性
        /// <inheritdoc/>
        protected override string UIAssetDir =>
            "Assets/GameFramework/Scripts/Editor/Inspectors/NetworkComponentInspector/";

        /// <inheritdoc/>
        protected override string UxmlName => "NetworkExportEditorWindowTool";

        /// <inheritdoc/>
        protected override GUIContent Title => new GUIContent("网络协议工具");

        /// <inheritdoc/>
        protected override Vector2 MinSize => new Vector2(600, 300);

        /// <inheritdoc/>
        protected override Vector2 MaxSize => new Vector2(600, 300);
        #endregion

        #region 菜单入口
        [MenuItem("Honor/网络协议工具", false, 362)]
        private static void OpenWindow() => Open();
        #endregion

        #region 基类钩子
        /// <inheritdoc/>
        protected override void OnInitData() { }

        /// <inheritdoc/>
        protected override void OnBindUI()
        {
            BindClick("exportAllBtn", () =>
            {
                GenerateProtosToLuaDir();
                GenerateProtoToEmmyLua();
                string filePath = GamePathUtils.Network.GetExcelFileFullPath();
                string tableFullName = filePath.Substring(filePath.LastIndexOf('/') + 1);
                string tableName = tableFullName.Substring(0, tableFullName.Length - ".xlsm".Length);
                ExportExcelToLua(tableName);
            });

            BindClick("openExcelBtn", () =>
                Process.Start(GamePathUtils.Network.GetExcelFileFullPath()));

            BindClick("openDirBtn", () =>
                Process.Start("Explorer.exe", GamePathUtils.Network.GetExcelRootDirectoryFullPath().Replace('/', '\\')));

            BindClick("openProtoDirBtn", () =>
                Process.Start(GamePathUtils.Proto.Net.GetRootDirectoryFullPath()));

            // 超时配置（纯 C# 单例，直接访问）
            var connectField = Q<FloatField>("connectTimeoutField");
            var requestField = Q<FloatField>("requestTimeoutField");
            connectField.RegisterValueChangedCallback(evt => { });
            requestField.RegisterValueChangedCallback(evt => { });

            RequestRepaint();
        }
        #endregion

        #region Proto 生成
        private void GenerateProtosToLuaDir()
        {
            ClearAllLuaProtos();
            string[] files = Directory.GetFiles(GamePathUtils.Proto.Net.GetRootDirectoryFullPath(), "*.proto", SearchOption.AllDirectories);
            foreach (string path in files)
            {
                string p = path.Replace('\\', '/');
                string fileName = p.Substring(p.LastIndexOf("/") + 1);
                string content = File.ReadAllText(p);
                string desc = string.Empty;
                if (content.Contains("协议描述"))
                {
                    var matches = Regex.Matches(content, @"(?<=(协议描述))\S*(?=(\r\n))");
                    if (matches.Count > 0)
                        desc = matches[0].ToString().Replace(":", "").Replace("：", "");
                }

                var sb = new StringBuilder();
                sb.AppendLine("-- ====================================================================================================")
                  .AppendLine("-- (c) copyright 2026 - 2030, Honor.Runtime")
                  .AppendLine("-- All Rights Reserved.")
                  .AppendLine("-- filename:  " + fileName)
                  .AppendLine("-- notices:   该文件自动生成，请不要手动修改！")
                  .AppendLine("--====================================================================================================")
                  .AppendLine("")
                  .AppendLine("-- proto.Schema结构")
                  .AppendLine("")
                  .AppendLine("return [[")
                  .AppendLine("")
                  .Append(content)
                  .AppendLine("]]");

                string outputPath = $"{GamePathUtils.Proto.Net.GetLuaScriptRootDirectoryFullPath()}/{Path.GetFileNameWithoutExtension(fileName)}.lua.txt";
                File.WriteAllText(outputPath, sb.ToString(), new UTF8Encoding(false));
            }
            AssetDatabase.Refresh();
        }

        private void ClearAllLuaProtos()
        {
            string dir = GamePathUtils.Proto.Net.GetLuaScriptRootDirectoryFullPath();
            if (Directory.Exists(dir)) Directory.Delete(dir, true);
            Directory.CreateDirectory(dir);
            AssetDatabase.Refresh();
        }

        private void GenerateProtoToEmmyLua()
        {
            string[] paths = Directory.GetFiles(GamePathUtils.Proto.Net.GetRootDirectoryFullPath(), "*.proto");
            string emPath = GamePathUtils.Proto.Net.GetLuaScriptDeclarationsDirectoryFullPath();
            if (!Directory.Exists(emPath)) Directory.CreateDirectory(emPath);

            foreach (string path in paths)
            {
                var info = new FileInfo(path);
                string fileName = info.Name.Replace(".proto", ".lua");
                string tidyName = info.Name.Replace(".proto", "");
                string outPath = emPath + "/Declare" + fileName + ".txt";

                var list = new List<string>();
                s_IsCurEnum = false;
                s_CurClsName.Clear();
                var finnalLines = new List<string>();

                string[] lines = File.ReadAllLines(path);
                SplitLineList2Normal(lines, ref finnalLines);

                string desc = "";
                foreach (string line in lines)
                {
                    if (!line.Contains("协议描述")) continue;
                    var m = Regex.Matches(line, @"(?<=(协议描述))\S*(?=())");
                    if (m.Count > 0) desc = m[0].ToString().Replace(":", "").Replace("：", "");
                }

                list.Add("--====================================================================================================");
                list.Add("-- (c)copyright 2026 - 2030, Honor.Runtime");
                list.Add("-- All Rights Reserved.");
                list.Add("-- filename: Declare" + fileName);
                if (!string.IsNullOrEmpty(desc)) list.Add("-- descrip: " + desc);
                list.Add("-- notices: 该文件自动生成，请不要手动修改！");
                list.Add("--====================================================================================================");
                list.Add("");

                var linkInfo = new Dictionary<string, string>();
                GetProtoMessageAndEnumDictionary(finnalLines, linkInfo, tidyName);

                foreach (string l in finnalLines)
                {
                    string r = ExportPbToLuaStr(linkInfo, l);
                    if (!string.IsNullOrEmpty(r)) list.Add(r);
                }
                File.WriteAllLines(outPath, list.ToArray());
            }
        }
        #endregion

        #region Excel → Lua
        private bool ExportExcelToLua(string excelName)
        {
            DataSet result = TableExportEditorUtility.GetExcelData(GamePathUtils.Network.GetExcelFileFullPath());
            string luaPath = GamePathUtils.Network.GetLuaScriptRootDirectoryFullPath() + "/" + excelName + ".lua.txt";
            int rows = result.Tables[0].Rows.Count;

            var dataKeyList = new List<string>();
            var dataDescList = new List<string>();
            var protoTidy = new List<string>();
            var protoReq = new List<string>();
            var protoResp = new List<string>();
            for (int i = 4; i < rows; i++)
            {
                dataKeyList.Add(result.Tables[0].Rows[i][2].ToString());
                dataDescList.Add(result.Tables[0].Rows[i][3].ToString());
                protoTidy.Add(string.IsNullOrEmpty(result.Tables[0].Rows[i][10].ToString()) ? null : result.Tables[0].Rows[i][10].ToString().Replace(".proto", ""));
                protoReq.Add(string.IsNullOrEmpty(result.Tables[0].Rows[i][11].ToString()) ? null : result.Tables[0].Rows[i][11].ToString());
                protoResp.Add(string.IsNullOrEmpty(result.Tables[0].Rows[i][12].ToString()) ? null : result.Tables[0].Rows[i][12].ToString());
            }

            string tableDetail = result.Tables[0].Rows[0][1].ToString();
            var sb = new StringBuilder();
            sb.AppendLine("--=====================================================================================================")
              .AppendLine("-- (c) copyright 2026 - 2030, Honor.Runtime")
              .AppendLine("-- filename:  " + excelName)
              .AppendLine("-- descrip:   " + tableDetail)
              .AppendLine("--=====================================================================================================")
              .AppendLine("");

            for (int i = 0; i < dataKeyList.Count; i++)
            {
                sb.AppendLine($"---@class NetCmd.Cmd_Item_{dataKeyList[i]} : NetCmd.Cmd_Item @'{dataDescList[i]}'协议");
                sb.AppendLine($"---@field Request NetCmd.Cmd_Item_Request_{dataKeyList[i]} @'{dataDescList[i]}'请求协议");
                sb.AppendLine($"---@field Response NetCmd.Cmd_Item_Response_{dataKeyList[i]} @'{dataDescList[i]}'响应协议");
                sb.AppendLine("");
            }

            sb.AppendLine("---@class NetCmd @" + tableDetail);
            for (int i = 0; i < dataKeyList.Count; i++)
                sb.AppendLine($"---@field {dataKeyList[i]} NetCmd.Cmd_Item_{dataKeyList[i]} @{dataDescList[i]}");
            sb.AppendLine("");
            sb.AppendLine("NetCmd = {");

            for (int i = 4; i < rows; i++)
            {
                int id = int.Parse(result.Tables[0].Rows[i][1].ToString());
                string name = result.Tables[0].Rows[i][2].ToString();
                string desc = result.Tables[0].Rows[i][3].ToString();
                sb.AppendLine($"    --{desc}");
                sb.AppendLine($"    {name} = {{");
                sb.AppendLine($"        ID = {id},");
                sb.AppendLine($"        Name = '{name}',");
                sb.AppendLine($"        Desc = '{desc}',");
                sb.AppendLine($"        Create = function() return CopyNetCmd(NetCmd.{name}) end,");
                sb.AppendLine("    },");
            }
            sb.AppendLine("}");
            sb.AppendLine("");

            sb.AppendLine("---CmdID与Cmd之间的映射关系");
            sb.AppendLine("NetCmdMap = {");
            for (int i = 4; i < rows; i++)
            {
                int id = int.Parse(result.Tables[0].Rows[i][1].ToString());
                string name = result.Tables[0].Rows[i][2].ToString();
                sb.AppendLine($"    [{id}] = NetCmd.{name},");
            }
            sb.AppendLine("}");

            if (File.Exists(luaPath)) File.Delete(luaPath);
            File.WriteAllText(luaPath, sb.ToString(), new UTF8Encoding(false));
            return true;
        }
        #endregion

        #region Proto 解析工具
        private static void GetProtoMessageAndEnumDictionary(List<string> lineList, Dictionary<string, string> info, string tidyName)
        {
            foreach (string line in lineList)
            {
                if (line.StartsWith("import"))
                {
                    string cls = line.Substring(line.IndexOf("import") + 8).Trim();
                    foreach (string f in Directory.GetFiles(GamePathUtils.Proto.Net.GetRootDirectoryFullPath(), cls))
                    {
                        string n = cls.Replace(".proto", "");
                        var sub = new List<string>();
                        SplitLineList2Normal(File.ReadAllLines(f), ref sub);
                        GetProtoMessageAndEnumDictionary(sub, info, n);
                    }
                }
                else if (line.StartsWith("message"))
                {
                    string cls = line.Substring(line.IndexOf("message") + 7, line.IndexOf("{") - line.IndexOf("message") - 7).Trim();
                    if (!info.ContainsKey(cls)) info.Add(cls, tidyName);
                }
                else if (line.StartsWith("enum"))
                {
                    string cls = line.Substring(line.IndexOf("enum") + 5, line.IndexOf("{") - line.IndexOf("enum") - 5).Trim();
                    if (!info.ContainsKey(cls)) info.Add(cls, tidyName);
                }
            }
        }

        private static void SplitLineList2Normal(string[] lineList, ref List<string> finnalLines)
        {
            var cache = new Dictionary<int, List<string>>();
            var final = new Dictionary<int, List<string>>();
            var rules = new Dictionary<string, string>();
            string curCls = "";
            int depth = 0;
            int total = 0;

            foreach (string raw in lineList)
            {
                string line = raw.Replace("\t", " ");
                if (line.Contains("{"))
                {
                    depth++;
                    string cls = "";
                    if (line.IndexOf("message") >= 0)
                        cls = line.Substring(line.IndexOf("message") + 7, line.IndexOf("{") - line.IndexOf("message") - 7).Trim();
                    else if (line.IndexOf("enum") >= 0)
                        cls = line.Substring(line.IndexOf("enum") + 4, line.IndexOf("{") - line.IndexOf("enum") - 4).Trim();

                    if (depth == 1)
                    {
                        curCls = cls;
                        rules.Clear();
                        cache.Clear();
                        final.Clear();
                    }
                    else if (!string.IsNullOrEmpty(curCls))
                    {
                        string newName = curCls + "_" + cls;
                        line = line.Replace(cls, newName);
                        if (!rules.ContainsKey(cls)) rules[cls] = newName;
                    }
                    cache[depth] = new List<string> { line };
                }
                else if (line.Contains("}"))
                {
                    cache[depth].Add(line);
                    total++;
                    final[total] = cache[depth];
                    cache.Remove(depth);
                    depth--;
                    if (depth == 0)
                    {
                        foreach (var item in final)
                        {
                            finnalLines.AddRange(item.Value);
                            item.Value.Clear();
                        }
                        final.Clear();
                        total = 0;
                    }
                }
                else
                {
                    if (depth == 0) finnalLines.Add(line);
                    else cache[depth].Add(line);
                }
            }
        }

        private static string ExportPbToLuaStr(Dictionary<string, string> info, string str)
        {
            if (string.IsNullOrEmpty(str)) return "";
            if (str.Contains("import") || str.Contains("java_package") || str.Contains("syntax")) return "";

            if (str.Contains("{"))
            {
                bool isMsg = str.Contains("message");
                bool isEnum = str.Contains("enum");
                if (isMsg || isEnum)
                {
                    string note = "";
                    if (str.Contains("//"))
                    {
                        note = str.Substring(str.IndexOf("//") + 2);
                        str = str.Substring(0, str.IndexOf("//"));
                    }
                    string cls = "";
                    if (isMsg)
                        cls = str.Substring(str.IndexOf("message") + 7, str.IndexOf("{") - str.IndexOf("message") - 7).Trim();
                    else
                        cls = str.Substring(str.IndexOf("enum") + 4, str.IndexOf("{") - str.IndexOf("enum") - 4).Trim();

                    if (isMsg)
                        str = $"---@class NetMsgDef.{info[cls]}.{cls} : nil";
                    else
                        str = $"---@class NetMsgDef.{info[cls]}.{cls} : nil";

                    if (!string.IsNullOrEmpty(note)) str = "--" + note + "\n" + str;
                    s_CurClsName.Add(cls);
                }
            }
            else if (str.Contains("}"))
            {
                str = "";
                if (s_CurClsName.Count > 0) s_CurClsName.RemoveAt(s_CurClsName.Count - 1);
            }
            else
            {
                str = PbMsgContent2Lua(info, str);
            }
            return str;
        }

        private static string PbMsgContent2Lua(Dictionary<string, string> info, string str)
        {
            if (str.Trim().StartsWith("//")) return str.Trim().Replace("//", "--");
            string[] parts = str.Split(' ');
            int fieldIdx = -1;
            for (int i = 0; i < parts.Length; i++)
            {
                if (parts[i] is "required" or "optional" or "repeated") { fieldIdx = i; break; }
                if (!parts[i].Contains("//") && !string.IsNullOrEmpty(parts[i])) { fieldIdx = i - 1; break; }
            }
            if (fieldIdx < 0) return str;

            string type = parts[fieldIdx];
            type = type switch
            {
                "int32" or "int64" or "float" or "double" or "uint32" or "uint64" => "number",
                "bool" => "boolean",
                "bytes" or "string" => "string",
                _ when info.ContainsKey(type) => "NetMsgDef." + info[type] + "." + type,
                _ => type
            };

            string field = "";
            for (int i = fieldIdx + 1; i < parts.Length; i++)
            {
                if (!string.IsNullOrEmpty(parts[i]) && !parts[i].Contains("<")) { field = parts[i]; break; }
            }
            return string.IsNullOrEmpty(field) ? str : $"---@field {field} {type}";
        }
        #endregion
    }
}
