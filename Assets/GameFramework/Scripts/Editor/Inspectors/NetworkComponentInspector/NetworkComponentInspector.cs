/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  NetworkComponentInspector.cs
 * author:    云毅
 * created:   2026
 * descrip:   Network组件编辑器面板定制
 ***************************************************************/
using ExcelDataReader;
using Honor.Runtime;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using XLua;

namespace Honor.Editor
{
    [CustomEditor(typeof(NetworkComponent))]
    internal sealed class NetworkComponentInspector : HonorComponentInspector
    {
        private SerializedProperty m_ConnectTimeout = null;
        private SerializedProperty m_RequestTimeout = null;

        private static bool s_IsCurEnum = false;
        private static List<string> s_CurClsName = new List<string>();
        private static List<string> m_EnumFields = new List<string>();
        private static List<string> m_EnumMembers = new List<string>();

        private void OnEnable()
        {
            m_ConnectTimeout = serializedObject.FindProperty("m_ConnectTimeout");
            m_RequestTimeout = serializedObject.FindProperty("m_RequestTimeout");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            EditorGUILayout.LabelField("网络协议配置");

            if (GUILayout.Button("导出网络协议Proto与Excel到Lua"))
            {
                // 生成所有网络proto.lua脚本
                GenerateProtosToLuaDir();

                // 生成所有网络Proto协议到EmmyLua注释
                GenerateProtoToEmmyLua();

                string filePath = GamePathUtils.Network.GetExcelFileFullPath();
                string tableFullName = filePath.Substring(filePath.LastIndexOf('/') + 1);
                string tableName = tableFullName.Substring(0, tableFullName.Length - ".xlsm".Length);
                ExportExcelToLua(tableName);

                GUIUtility.ExitGUI();
            }

            EditorGUILayout.BeginHorizontal("box");
            if (GUILayout.Button("打开网络协议表Excel"))
            {
                string filePath = GamePathUtils.Network.GetExcelFileFullPath();
                switch (Application.platform)
                {
                    case RuntimePlatform.WindowsEditor:
                        Process.Start(filePath);
                        break;
                    case RuntimePlatform.OSXEditor:
                        Process.Start("open", filePath);
                        break;
                    default:
                        throw new GameException(string.Format("Not support open file on '{0}' platform.", Application.platform.ToString()));
                }
                GUIUtility.ExitGUI();
            }

            if (GUILayout.Button("打开网络协议表Excel所在文件夹"))
            {
                string folder = string.Format("\"{0}\"", GamePathUtils.Network.GetExcelRootDirectoryFullPath());
                switch (Application.platform)
                {
                    case RuntimePlatform.WindowsEditor:
                        Process.Start("Explorer.exe", folder.Replace('/', '\\'));
                        break;

                    case RuntimePlatform.OSXEditor:
                        Process.Start("open", folder);
                        break;

                    default:
                        throw new GameException(string.Format("Not support open folder on '{0}' platform.", Application.platform.ToString()));
                }
                GUIUtility.ExitGUI();
            }

            if (GUILayout.Button("打开网络Proto文件夹"))
            {
                OpenProtoFolder();
                GUIUtility.ExitGUI();
            }

            EditorGUILayout.EndHorizontal();

            m_ConnectTimeout.floatValue = EditorGUILayout.FloatField("默认网络连接超时时间", m_ConnectTimeout.floatValue);
            m_RequestTimeout.floatValue = EditorGUILayout.FloatField("默认网络请求超时时间", m_RequestTimeout.floatValue);

            serializedObject.ApplyModifiedProperties();

            Repaint();
        }

        protected override void OnCompileStart()
        {
            base.OnCompileStart();

        }

        protected override void OnCompileComplete()
        {
            base.OnCompileComplete();

        }

        /// <summary>
        /// 打开网络协议文件夹。
        /// </summary>
        private void OpenProtoFolder()
        {
            string folderPath = GamePathUtils.Proto.Net.GetRootDirectoryFullPath();
            switch (Application.platform)
            {
                case RuntimePlatform.WindowsEditor:
                    Process.Start(folderPath);
                    break;
                case RuntimePlatform.OSXEditor:
                    Process.Start("open", folderPath);
                    break;
                default:
                    throw new GameException(string.Format("Not support open folder on '{0}' platform.", Application.platform.ToString()));
            }
        }

        /// <summary>
        /// 生成网络协议文件到lua。
        /// </summary>
        private void GenerateProtosToLuaDir()
        {
            ClearAllLuaProtos();
            string[] fileFullPaths = System.IO.Directory.GetFiles(GamePathUtils.Proto.Net.GetRootDirectoryFullPath(), "*.proto", System.IO.SearchOption.AllDirectories);
            for (int index = 0; index < fileFullPaths.Length; index++)
            {
                string path = fileFullPaths[index].Replace('\\', '/');
                string fileName = path.Substring(path.LastIndexOf("/") + 1, path.Length - path.LastIndexOf("/") - 1);
                string content = System.IO.File.ReadAllText(path);
                string desc = string.Empty;
                string startStr = "协议描述";
                if (content.Contains(startStr))
                {
                    string input = string.Format(@"(?<=({0}))\S*(?=(\r\n))", startStr);
                    MatchCollection matchCollection = Regex.Matches(content, input);
                    if (matchCollection.Count > 0)
                    {
                        desc = matchCollection[0].ToString().Replace(":", string.Empty).Replace("：", string.Empty);
                    }
                }

                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine(string.Format("-- ====================================================================================================="))
                             .AppendLine(string.Format("-- (c) copyright 2026 - 2030, Honor.Runtime"))
                             .AppendLine(string.Format("-- All Rights Reserved."))
                             .AppendLine(string.Format("-- ----------------------------------------------------------------------------------------------------"))
                             .AppendLine(string.Format("-- filename:  {0}.lua", fileName));
                if (!string.IsNullOrEmpty(desc))
                {
                    stringBuilder.AppendLine(string.Format("-- descrip:   {0}", desc));
                }
                stringBuilder.AppendLine(string.Format("-- notices:   该文件自动生成，请不要手动修改！"));
                stringBuilder.AppendLine(string.Format("--====================================================================================================="))
                             .AppendLine(string.Format(""));
                stringBuilder.AppendLine(string.Format("-- proto.Schema结构"))
                             .AppendLine(string.Format(""))
                             .AppendLine(string.Format("return [["))
                             .AppendLine(string.Format(""))
                             .Append(content)
                             .AppendLine("]]");

                string luaScriptName = string.Format("{0}.lua.txt", fileName);
                string outputPath = string.Format("{0}/{1}", GamePathUtils.Proto.Net.GetLuaScriptRootDirectoryFullPath(), luaScriptName);
                System.IO.File.WriteAllText(outputPath, stringBuilder.ToString(), new System.Text.UTF8Encoding(false));

                Log.Debug(string.Format("生成{0}文件成功。", outputPath));

            }

            AssetDatabase.Refresh();

        }

        /// <summary>
        /// 清空Lua-Proto协议脚本
        /// </summary>
        private void ClearAllLuaProtos()
        {
            if (System.IO.Directory.Exists(GamePathUtils.Proto.Net.GetLuaScriptRootDirectoryFullPath()))
            {
                System.IO.Directory.Delete(GamePathUtils.Proto.Net.GetLuaScriptRootDirectoryFullPath(), true);
            }
            System.IO.Directory.CreateDirectory(GamePathUtils.Proto.Net.GetLuaScriptRootDirectoryFullPath());
            Log.Debug(string.Format("清空{0}文件夹成功。", GamePathUtils.Proto.Net.GetLuaScriptRootDirectoryFullPath()));
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 导出Cmd表格到lua
        /// </summary>
        /// <param name="openExcelNamePre"></param>
        /// <returns></returns>
        private bool ExportExcelToLua(string openExcelNamePre)
        {
            //获取表格内容
            DataSet result = TableExportEditorUtility.GetExcelData(GamePathUtils.Network.GetExcelFileFullPath());
            string luaRootPath = GamePathUtils.Network.GetLuaScriptRootDirectoryFullPath();
            string luaPath = luaRootPath + "/" + openExcelNamePre + ".lua.txt";

            int rows = result.Tables[0].Rows.Count;

            // 数据key与desc
            List<string> dataKeyList = new List<string>();
            List<string> dataDescList = new List<string>();
            List<string> protoTidyName = new List<string>();
            List<string> protoRequestName = new List<string>();
            List<string> protoResponseName = new List<string>();
            for (int i = 4; i < rows; i++)
            {
                dataKeyList.Add(result.Tables[0].Rows[i][2].ToString());
                dataDescList.Add(result.Tables[0].Rows[i][3].ToString());
                protoTidyName.Add(string.IsNullOrEmpty(result.Tables[0].Rows[i][10].ToString()) ? null : result.Tables[0].Rows[i][10].ToString().Replace(".proto", string.Empty));
                protoRequestName.Add(string.IsNullOrEmpty(result.Tables[0].Rows[i][11].ToString()) ? null : result.Tables[0].Rows[i][11].ToString());
                protoResponseName.Add(string.IsNullOrEmpty(result.Tables[0].Rows[i][12].ToString()) ? null : result.Tables[0].Rows[i][12].ToString());
            }

            // 获取表格说明
            string tableDetail = result.Tables[0].Rows[0][1].ToString();
            // 开始添加文件头
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("--=====================================================================================================")
                         .AppendLine("-- (c) copyright 2026 - 2030, Honor.Runtime")
                         .AppendLine("-- All Rights Reserved.")
                         .AppendLine("-- ----------------------------------------------------------------------------------------------------")
                         .AppendLine(string.Format("-- filename:  {0}", openExcelNamePre))
                         .AppendLine(string.Format("-- descrip:   {0}", tableDetail))
                         .AppendLine("-- notices:   该文件自动生成，请不要手动修改！")
                         .AppendLine("--=====================================================================================================")
                         .AppendLine("");

            stringBuilder.AppendLine("-------------------------------------------------------------------------------------------------------");
            stringBuilder.AppendLine("");
            stringBuilder.AppendLine("---@class NetCmd.Cmd_Item_Proto @协议文件信息");
            stringBuilder.AppendLine("---@field ProtoLuaFileName string @协议lua文件名称");
            stringBuilder.AppendLine("---@field ProtoRequestDataName string @协议请求数据名称");
            stringBuilder.AppendLine("---@field ProtoResponseDataName string @协议响应数据名称");
            stringBuilder.AppendLine("");
            stringBuilder.AppendLine("---@class NetCmd.Cmd_Item_Request @请求协议");
            stringBuilder.AppendLine("---@field Head table @请求协议头");
            stringBuilder.AppendLine("---@field Body table @请求协议体");
            stringBuilder.AppendLine("");
            stringBuilder.AppendLine("---@class NetCmd.Cmd_Item_Response @响应协议");
            stringBuilder.AppendLine("---@field Head table @响应协议头");
            stringBuilder.AppendLine("---@field Body table @响应协议体");
            stringBuilder.AppendLine("");
            stringBuilder.AppendLine("---@class NetCmd.Cmd_Item @协议条目");
            stringBuilder.AppendLine("---@field ID number @协议ID");
            stringBuilder.AppendLine("---@field Name string @名称");
            stringBuilder.AppendLine("---@field Desc string @备注");
            stringBuilder.AppendLine("---@field Way string @网络方式");
            stringBuilder.AppendLine("---@field DevelopURL string @开发环境-访问的URL");
            stringBuilder.AppendLine("---@field PublishURL string @生产环境-访问的URL");
            stringBuilder.AppendLine("---@field URLParamsFormat string @域值");
            stringBuilder.AppendLine("---@field IsProto boolean @是否为Proto协议");
            stringBuilder.AppendLine("---@field IsGZip boolean @是否需要GZIP");
            stringBuilder.AppendLine("---@field Proto NetCmd.Cmd_Item_Proto @Proto协议");
            stringBuilder.AppendLine("---@field Request NetCmd.Cmd_Item_Request @请求协议");
            stringBuilder.AppendLine("---@field Response NetCmd.Cmd_Item_Response @响应协议");
            stringBuilder.AppendLine("---@field Callback function @协议回调(req, resp, cmd, returnData)");
            stringBuilder.AppendLine("---@field Create function @创建协议");
            stringBuilder.AppendLine("");

            stringBuilder.AppendLine("-------------------------------------------------------------------------------------------------------");
            for (int index = 0; index < dataKeyList.Count; index++)
            {
                stringBuilder.AppendLine("");

                stringBuilder.AppendLine(string.Format("---@class NetCmd.Cmd_Item_Request_{0} : NetCmd.Cmd_Item_Request @'{1}'请求协议", dataKeyList[index], dataDescList[index]));
                stringBuilder.AppendLine(string.Format("---@field Head NetCmd.Cmd_Item_Request_Head_{0} @'{1}'请求协议头", dataKeyList[index], dataDescList[index]));
                stringBuilder.AppendLine(string.Format("---@field Body NetCmd.Cmd_Item_Request_Body_{0} @'{1}'请求协议体", dataKeyList[index], dataDescList[index]));
                stringBuilder.AppendLine(string.Format(""));

                stringBuilder.AppendLine(string.Format("---@class NetCmd.Cmd_Item_Request_Head_{0} @'{1}'请求协议头", dataKeyList[index], dataDescList[index]));
                stringBuilder.AppendLine(string.Format("---@field Content table @'{0}'请求协议头内容", dataDescList[index]));
                stringBuilder.AppendLine(string.Format(""));

                stringBuilder.AppendLine(string.Format("---@class NetCmd.Cmd_Item_Request_Body_{0} @'{1}'请求协议体", dataKeyList[index], dataDescList[index]));
                if(!string.IsNullOrEmpty(protoTidyName[index]) && !string.IsNullOrEmpty(protoRequestName[index]))
                {
                    stringBuilder.AppendLine(string.Format("---@field Content NetMsgDef.{0}.{1} @'{2}'请求协议体内容", protoTidyName[index], protoRequestName[index], dataDescList[index]));
                }
                else
                {
                    stringBuilder.AppendLine(string.Format("---@field Content table @'{0}'请求协议体内容", dataDescList[index]));
                }
                stringBuilder.AppendLine(string.Format(""));

                stringBuilder.AppendLine(string.Format("---@class NetCmd.Cmd_Item_Response_{0} : NetCmd.Cmd_Item_Response @'{1}'响应协议", dataKeyList[index], dataDescList[index]));
                stringBuilder.AppendLine(string.Format("---@field Head NetCmd.Cmd_Item_Response_Head_{0} @'{1}'响应协议头", dataKeyList[index], dataDescList[index]));
                stringBuilder.AppendLine(string.Format("---@field Body NetCmd.Cmd_Item_Response_Body_{0} @'{1}'响应协议体", dataKeyList[index], dataDescList[index]));
                stringBuilder.AppendLine(string.Format(""));

                stringBuilder.AppendLine(string.Format("---@class NetCmd.Cmd_Item_Response_Head_{0} @'{1}'响应协议头", dataKeyList[index], dataDescList[index]));
                stringBuilder.AppendLine(string.Format("---@field Content table @'{0}'响应协议头内容", dataDescList[index]));
                stringBuilder.AppendLine(string.Format(""));

                stringBuilder.AppendLine(string.Format("---@class NetCmd.Cmd_Item_Response_Body_{0} @'{1}'响应协议体", dataKeyList[index], dataDescList[index]));
                if (!string.IsNullOrEmpty(protoTidyName[index]) && !string.IsNullOrEmpty(protoResponseName[index]))
                {
                    stringBuilder.AppendLine(string.Format("---@field Content NetMsgDef.{0}.{1} @'{2}'响应协议体内容", protoTidyName[index], protoResponseName[index], dataDescList[index]));
                }
                else
                {
                    stringBuilder.AppendLine(string.Format("---@field Content table @'{0}'响应协议体内容", dataDescList[index]));
                }
                stringBuilder.AppendLine(string.Format(""));

                stringBuilder.AppendLine(string.Format("---@class NetCmd.Cmd_Item_{0} : NetCmd.Cmd_Item @'{1}'协议", dataKeyList[index], dataDescList[index]));
                stringBuilder.AppendLine(string.Format("---@field Request NetCmd.Cmd_Item_Request_{0} @'{1}'请求协议", dataKeyList[index], dataDescList[index]));
                stringBuilder.AppendLine(string.Format("---@field Response NetCmd.Cmd_Item_Response_{0} @'{1}'响应协议", dataKeyList[index], dataDescList[index]));
                stringBuilder.AppendLine(string.Format(""));
                stringBuilder.AppendLine("-------------------------------------------------------------------------------------------------------");
            }
            stringBuilder.AppendLine(string.Format(""));

            stringBuilder.AppendLine("---@class NetCmd @" + tableDetail);
            for(int index = 0; index < dataKeyList.Count; index++)
            {
                stringBuilder.AppendLine(string.Format("---@field {0} NetCmd.Cmd_Item_{1} @{2}", dataKeyList[index], dataKeyList[index], dataDescList[index]));
            }

            stringBuilder.AppendLine(string.Format(""));

            stringBuilder.AppendLine("NetCmd = {");

            List<string> cmdNames = new List<string>();
            List<string> initProtoFileStrs = new List<string>();
            List<string> initProtoFileNames = new List<string>();
            List<string> declareLuaFileStrs = new List<string>();
            List<string> declareLuaFileNames = new List<string>();
            for (int i = 4; i < rows; i++)
            {
                int ID = int.Parse(result.Tables[0].Rows[i][1].ToString());
                string Name = result.Tables[0].Rows[i][2].ToString();
                string Desc = result.Tables[0].Rows[i][3].ToString();
                string Way = result.Tables[0].Rows[i][4].ToString();
                string DevelopURL = result.Tables[0].Rows[i][5].ToString();
                string PublishURL = result.Tables[0].Rows[i][6].ToString();
                string URLParamsFormat = result.Tables[0].Rows[i][7].ToString();
                bool IsProto = bool.Parse(result.Tables[0].Rows[i][8].ToString());
                bool IsGZip =  bool.Parse(result.Tables[0].Rows[i][9].ToString());
                string ProtoLuaFileName = string.IsNullOrEmpty(result.Tables[0].Rows[i][10].ToString())?"nil": ("'" + result.Tables[0].Rows[i][10].ToString() + "'");
                string ProtoRequestDataName = string.IsNullOrEmpty(result.Tables[0].Rows[i][11].ToString()) ? "nil" : ("'" + result.Tables[0].Rows[i][11].ToString() + "'");
                string ProtoResponseDataName = string.IsNullOrEmpty(result.Tables[0].Rows[i][12].ToString()) ? "nil" : ("'" + result.Tables[0].Rows[i][12].ToString() + "'");
                stringBuilder.AppendLine("      --" + Desc);
                stringBuilder.AppendLine("      " + Name + " = {");
                stringBuilder.AppendLine("          ID = " + ID + ",");
                stringBuilder.AppendLine("          Name = '" + Name + "',");
                stringBuilder.AppendLine("          Desc = '" + Desc + "',");
                stringBuilder.AppendLine("          Way = '" + Way + "',");
                stringBuilder.AppendLine("          DevelopURL = '" + DevelopURL + "',");
                stringBuilder.AppendLine("          PublishURL = '" + PublishURL + "',");
                stringBuilder.AppendLine("          URLParamsFormat = '" + URLParamsFormat + "',");
                stringBuilder.AppendLine("          IsProto = " + IsProto.ToString().ToLower() + ",");
                stringBuilder.AppendLine("          IsGZip = " + IsGZip.ToString().ToLower() + ",");
                stringBuilder.AppendLine("          Proto = {");
                stringBuilder.AppendLine("              ProtoLuaFileName = " + ProtoLuaFileName + ",");
                stringBuilder.AppendLine("              ProtoRequestDataName = " + ProtoRequestDataName + ",");
                stringBuilder.AppendLine("              ProtoResponseDataName = " + ProtoResponseDataName + ",");
                stringBuilder.AppendLine("          },");
                stringBuilder.AppendLine("          Request = {");
                stringBuilder.AppendLine("              Head = {");
                stringBuilder.AppendLine("                 Content = {},");
                stringBuilder.AppendLine("              },");
                stringBuilder.AppendLine("              Body = {");
                stringBuilder.AppendLine("                 Content = {},");
                stringBuilder.AppendLine("              },");
                stringBuilder.AppendLine("          },");
                stringBuilder.AppendLine("          Response = {");
                stringBuilder.AppendLine("              Head = {");
                stringBuilder.AppendLine("                 Content = {},");
                stringBuilder.AppendLine("              },");
                stringBuilder.AppendLine("              Body = {");
                stringBuilder.AppendLine("                 Content = {},");
                stringBuilder.AppendLine("              },");
                stringBuilder.AppendLine("          },");
                stringBuilder.AppendLine("          Callback = nil,");
                stringBuilder.AppendLine("          ---@type NetCmd.Cmd_Item_" + Name);
                stringBuilder.AppendLine("          Create = function()");
                stringBuilder.AppendLine("              return CopyNetCmd(NetCmd." + Name + ")");
                stringBuilder.AppendLine("          end,");
                stringBuilder.AppendLine("      },");

                if (!cmdNames.Contains(Name))
                {
                    cmdNames.Add(Name);
                }

                if (IsProto)
                {
                    if(!initProtoFileNames.Contains(ProtoLuaFileName))
                    {
                        initProtoFileNames.Add(ProtoLuaFileName);
                        initProtoFileStrs.Add(string.Format("InitProtoFile({0})", ProtoLuaFileName));
                    }
                    string declareLuaFileName = ProtoLuaFileName.Replace(".proto", "").Replace("'", "");
                    if (!declareLuaFileNames.Contains(declareLuaFileName))
                    {
                        declareLuaFileNames.Add(declareLuaFileName);
                        declareLuaFileStrs.Add(string.Format("require('Declare{0}')", declareLuaFileName));
                    }
                }

            }

            stringBuilder.AppendLine("}");
            stringBuilder.AppendLine("");

            // 生成ID与CMD之间的映射关系
            stringBuilder.AppendLine("---CmdID与Cmd之间的映射关系");
            stringBuilder.AppendLine("---@type table<number, NetCmd.Cmd_Item> @数据格式");
            stringBuilder.AppendLine("NetCmdMap = {");
            for (int i = 4; i < rows; i++)
            {
                int ID = int.Parse(result.Tables[0].Rows[i][1].ToString());
                string Name = result.Tables[0].Rows[i][2].ToString();
                stringBuilder.AppendLine($"    [{ID}] = NetCmd.{Name},");
            }
            stringBuilder.AppendLine("}");
            stringBuilder.AppendLine("");

            // 追加Proto文件require
            stringBuilder.AppendLine("-------------------------------------------------------------------------------------------------------");
            stringBuilder.AppendLine("");
            stringBuilder.AppendLine("---初始化Proto文件");
            foreach (var str in initProtoFileStrs)
            {
                stringBuilder.AppendLine(str);
            }

            stringBuilder.AppendLine("");
            stringBuilder.AppendLine("---导入ProtoDeclare文件");
            foreach (var str in declareLuaFileStrs)
            {
                stringBuilder.AppendLine(str);
            }

            if (File.Exists(luaPath))
            {
                File.Delete(luaPath);
            }
            File.WriteAllText(luaPath, stringBuilder.ToString(), new System.Text.UTF8Encoding(false));
            Log.Debug("生成 " + luaPath + " 文件成功。");

            return true;
        }

        /// <summary>
        /// 生成Proto协议到EmmyLua注释
        /// </summary>
        private static void GenerateProtoToEmmyLua()
        {
            string[] paths = Directory.GetFiles(GamePathUtils.Proto.Net.GetRootDirectoryFullPath(), "*.proto");
            string emPbDirecPath = GamePathUtils.Proto.Net.GetLuaScriptDeclarationsDirectoryFullPath();
            if (!Directory.Exists(emPbDirecPath))
            {
                Directory.CreateDirectory(emPbDirecPath);
            }
            
            List<string> finnalLines = new List<string>();

            for (var i = 0; i < paths.Length; i++)
            {
                FileInfo info = new FileInfo(paths[i]);
                string fileName = info.Name.Replace(".proto", ".lua");
                string tidyFileName = info.Name.Replace(".proto", string.Empty);
                string path = emPbDirecPath + "/Declare" + fileName;

                List<string> list = new List<string>();

                s_IsCurEnum = false;
                s_CurClsName.Clear();
                finnalLines.Clear();

                // 当前的proto文件
                string[] lines = File.ReadAllLines(paths[i]);
                SplitLineList2Normal(lines, ref finnalLines);

                string desc = string.Empty;
                string startStr = "协议描述";
                for (int index = 0; index < lines.Length; index++)
                {
                    if(lines[index].Contains(startStr))
                    {
                        string input = string.Format(@"(?<=({0}))\S*(?=())", startStr);
                        MatchCollection matchCollection = Regex.Matches(lines[index], input);
                        if (matchCollection.Count > 0)
                        {
                            desc = matchCollection[0].ToString().Replace(":", string.Empty).Replace("：", string.Empty);
                        }
                    }
                }

                list.Add(string.Format("--====================================================================================================="));
                list.Add(string.Format("-- (c)copyright 2026 - 2030, Honor.Runtime"));
                list.Add(string.Format("-- All Rights Reserved."));
                list.Add(string.Format("-- ----------------------------------------------------------------------------------------------------"));
                list.Add(string.Format("-- filename: Declare{0}", fileName));
                if (!string.IsNullOrEmpty(desc))
                {
                    list.Add(string.Format("-- descrip: {0}", desc));
                }
                list.Add(string.Format("-- notices: 该文件自动生成，请不要手动修改！"));
                list.Add(string.Format("--====================================================================================================="));
                list.Add("");
                
                // 查找出所有message和enum的归属proto <message名字 或 enum名字，Proto名字>
                var tidyFilesLinkInfo = new Dictionary<string, string>();
                GetProtoMessageAndEnumDictionary(finnalLines, tidyFilesLinkInfo, tidyFileName);

                for (var j = 0; j < finnalLines.Count; j++)
                {
                    string finalLineStr = ExportPbToLuaStr(tidyFilesLinkInfo, finnalLines[j]);
                    if (!finalLineStr.Equals(string.Empty))
                    {
                        list.Add(finalLineStr);
                    }
                }

                path += ".txt";
                File.WriteAllLines(path, list.ToArray());
                Log.Debug("生成 " + path + " 文件成功。");
            }
        }

        /// <summary>
        /// 得到message和enum所在的proto集合
        /// </summary>
        /// <param name="lineList"></param>
        /// <param name="tidyFilesLinkInfo"></param>
        /// <param name="tidyFileName"></param>
        private static void GetProtoMessageAndEnumDictionary(List<string> lineList, Dictionary<string, string> tidyFilesLinkInfo, string tidyFileName)
        {
            string curContentTemp = String.Empty;
            for (int index = 0; index < lineList.Count; index++)
            {
                curContentTemp = lineList[index];
                if (curContentTemp.StartsWith("import"))
                {
                    var msgTitleIndex = curContentTemp.IndexOf("import");
                    var clsName = curContentTemp.Substring(msgTitleIndex + 8,curContentTemp.Length - 8 - 1).Trim();
                    string[] fileFullPaths = Directory.GetFiles(GamePathUtils.Proto.Net.GetRootDirectoryFullPath(), clsName);
                    for (var fileIndex = 0; fileIndex < fileFullPaths.Length; fileIndex++)
                    {
                        string importTidyFileName = clsName.Replace(".proto", string.Empty);
                        string[] lines = File.ReadAllLines(fileFullPaths[fileIndex]);
                        List<string> finnalLines = new List<string>();
                        SplitLineList2Normal(lines, ref finnalLines);
                        GetProtoMessageAndEnumDictionary(finnalLines, tidyFilesLinkInfo, importTidyFileName);
                    }

                }
                else if (curContentTemp.StartsWith("message"))
                {
                    var msgTitleIndex = curContentTemp.IndexOf("message");
                    var clsName = curContentTemp.Substring(msgTitleIndex + 7, curContentTemp.IndexOf("{") - msgTitleIndex - 7).Trim();
                    if (tidyFilesLinkInfo.ContainsKey(clsName) == false)
                    {
                        tidyFilesLinkInfo.Add(clsName, tidyFileName);
                    }
                }
                else if (curContentTemp.StartsWith("enum"))
                {
                    var msgTitleIndex = curContentTemp.IndexOf("enum");
                    var clsName = curContentTemp.Substring(msgTitleIndex + 5, curContentTemp.IndexOf("{") - msgTitleIndex - 5).Trim();
                    if (tidyFilesLinkInfo.ContainsKey(clsName) == false)
                    {
                        tidyFilesLinkInfo.Add(clsName, tidyFileName);
                    }
                }
            }
        }

        private static void SplitLineList2Normal(string[] lineList, ref List<string> finnalLines)
        {
            string curContentTemp = String.Empty;
            Dictionary<int, List<string>> cacheLinesDic = new Dictionary<int, List<string>>();
            Dictionary<int, List<string>> finalLinesDic = new Dictionary<int, List<string>>();
            Dictionary<string, string> replaceRuleDic = new Dictionary<string, string>();
            string curClsName = string.Empty;
            int curClsIndex = 0;
            int totalClsCount = 0;
            for (int index = 0; index < lineList.Length; index++)
            {
                curContentTemp = lineList[index];
                curContentTemp = curContentTemp.Replace("\t", " ");
                if (curContentTemp.Contains("{"))
                {
                    curClsIndex = curClsIndex + 1;
                    //是否要替换内部类的名字
                    string clsName = string.Empty;
                    int msgTitleIndex = 0;
                    if (curContentTemp.IndexOf("message") >= 0)
                    {
                        msgTitleIndex = curContentTemp.IndexOf("message");
                        clsName = curContentTemp.Substring(msgTitleIndex + 7, curContentTemp.IndexOf("{") - msgTitleIndex - 7).Trim();
                    }
                    else if (curContentTemp.IndexOf("enum") >= 0)
                    {
                        msgTitleIndex = curContentTemp.IndexOf("enum");
                        clsName = curContentTemp.Substring(msgTitleIndex + 4, curContentTemp.IndexOf("{") - msgTitleIndex - 4).Trim();
                    }
                    if (curClsIndex == 1)
                    {
                        //缓存当前message或者enum的名字
                        curClsName = clsName;
                        replaceRuleDic.Clear();
                        cacheLinesDic.Clear();
                        finalLinesDic.Clear();
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(curClsName))
                        {
                            string nameNew = curClsName + "_" + clsName;
                            curContentTemp = curContentTemp.Replace(clsName, nameNew);
                            //缓存替换规则
                            if (replaceRuleDic.ContainsKey(clsName))
                                replaceRuleDic[clsName] = nameNew;
                            else
                                replaceRuleDic.Add(clsName, nameNew);
                            //当前类的缓存里是否有需要替换的类名
                            foreach (KeyValuePair<int, List<string>> item in cacheLinesDic)
                            {
                                for (int itemIndex = 0; itemIndex < item.Value.Count; itemIndex++)
                                {
                                    string tempValueStr = item.Value[itemIndex];
                                    string[] valueStrList = tempValueStr.Split(' ');
                                    bool isContains = false;
                                    string ruleKey = string.Empty;
                                    string ruleValue = string.Empty;
                                    foreach (KeyValuePair<string, string> itemRule in replaceRuleDic)
                                    {
                                        ruleKey = itemRule.Key;
                                        ruleValue = itemRule.Value;
                                        for (int ruleIndex = 0; ruleIndex < valueStrList.Length; ruleIndex++)
                                        {
                                            if (valueStrList[ruleIndex].CompareTo(ruleKey) == 0)
                                            {
                                                valueStrList[ruleIndex] = ruleValue;
                                                isContains = true;
                                                break;
                                            }
                                        }
                                        if (isContains)
                                            break;
                                    }
                                    //拼接回来字符串
                                    tempValueStr = string.Empty;
                                    for (int strIndex = 0; strIndex < valueStrList.Length; strIndex++)
                                    {
                                        tempValueStr += valueStrList[strIndex] + " ";
                                    }
                                    item.Value[itemIndex] = tempValueStr;
                                }
                            }
                            foreach (KeyValuePair<int, List<string>> item in finalLinesDic)
                            {
                                for (int itemIndex = 0; itemIndex < item.Value.Count; itemIndex++)
                                {
                                    string tempValueStr = item.Value[itemIndex];
                                    string[] valueStrList = tempValueStr.Split(' ');
                                    bool isContains = false;
                                    string ruleKey = string.Empty;
                                    string ruleValue = string.Empty;
                                    foreach (KeyValuePair<string, string> itemRule in replaceRuleDic)
                                    {
                                        ruleKey = itemRule.Key;
                                        ruleValue = itemRule.Value;
                                        for (int ruleIndex = 0; ruleIndex < valueStrList.Length; ruleIndex++)
                                        {
                                            if (valueStrList[ruleIndex].CompareTo(ruleKey) == 0)
                                            {
                                                valueStrList[ruleIndex] = ruleValue;
                                                isContains = true;
                                                break;
                                            }
                                        }
                                        if (isContains)
                                            break;
                                    }
                                    //拼接回来字符串
                                    tempValueStr = string.Empty;
                                    for (int strIndex = 0; strIndex < valueStrList.Length; strIndex++)
                                    {
                                        tempValueStr += valueStrList[strIndex] + " ";
                                    }
                                    item.Value[itemIndex] = tempValueStr;
                                }
                            }
                        }
                    }
                    List<string> temp = new List<string>();
                    temp.Add(curContentTemp);
                    cacheLinesDic.Add(curClsIndex, temp);
                }
                else if (curContentTemp.Contains("}"))
                {
                    cacheLinesDic[curClsIndex].Add(curContentTemp);
                    totalClsCount += 1;
                    finalLinesDic[totalClsCount] = cacheLinesDic[curClsIndex];
                    cacheLinesDic.Remove(curClsIndex);
                    curClsIndex = curClsIndex - 1;
                    if (curClsIndex == 0)
                    {
                        foreach (KeyValuePair<int, List<string>> item in finalLinesDic)
                        {
                            List<string> temp = item.Value;
                            for (int cacheIndex = 0; cacheIndex < temp.Count; cacheIndex++)
                            {
                                finnalLines.Add(temp[cacheIndex]);
                            }
                            item.Value.Clear();
                        }
                        finalLinesDic.Clear();
                        totalClsCount = 0;
                    }
                }
                else
                {
                    if (curClsIndex == 0)
                    {
                        finnalLines.Add(curContentTemp);
                    }
                    else
                    {
                        //是否要替换类名
                        string[] valueStrList = curContentTemp.Split(' ');
                        bool isContains = false;
                        string ruleKey = string.Empty;
                        string ruleValue = string.Empty;
                        foreach (KeyValuePair<string, string> itemRule in replaceRuleDic)
                        {
                            ruleKey = itemRule.Key;
                            ruleValue = itemRule.Value;
                            for (int ruleIndex = 0; ruleIndex < valueStrList.Length; ruleIndex++)
                            {
                                if (valueStrList[ruleIndex].CompareTo(ruleKey) == 0)
                                {
                                    valueStrList[ruleIndex] = ruleValue;
                                    isContains = true;
                                    break;
                                }
                            }
                            if (isContains)
                                break;
                        }
                        //拼接回来字符串
                        curContentTemp = string.Empty;
                        for (int strIndex = 0; strIndex < valueStrList.Length; strIndex++)
                        {
                            curContentTemp += valueStrList[strIndex] + " ";
                        }
                        cacheLinesDic[curClsIndex].Add(curContentTemp);
                    }
                }
            }
        }

        private static string ExportPbToLuaStr(Dictionary<string, string> tidyFilesLinkInfo, string str)
        {
            if (string.IsNullOrEmpty(str))
                return string.Empty;
            if (str.Contains("import")
                || str.Contains("package com.gy.server.packet;")
                || str.Contains("option java_package")
                || str.Contains("option java_outer_classname ")
                || str.Contains("syntax = \"proto3\";")
            )
            {
                return string.Empty;
            }

            if (str.Contains("{"))
            {
                if (str.Trim().IndexOf("//") == 0)
                {
                    str = str.Trim().Replace("//", "--");
                    return str;
                }
                //开始组建结构
                //是message还是enum
                bool isMsg = str.Contains("message");
                bool isEnum = str.Contains("enum");
                if (isMsg || isEnum)
                {
                    string curNote = String.Empty;
                    if (str.Contains("//"))
                    {
                        //截取服务器添加的注解
                        curNote = str.Substring(str.IndexOf("//") + 2);
                        str = str.Substring(0, str.IndexOf("//")); //向后截取没
                    }

                    string clsName = string.Empty;
                    int msgTitleIndex = 0;
                    if (isMsg)
                    {
                        msgTitleIndex = str.IndexOf("message");
                        clsName = str.Substring(msgTitleIndex + 7, str.IndexOf("{") - msgTitleIndex - 7).Trim();
                    }
                    else
                    {
                        msgTitleIndex = str.IndexOf("enum");
                        clsName = str.Substring(msgTitleIndex + 4, str.IndexOf("{") - msgTitleIndex - 4).Trim();
                    }
                    if (!string.IsNullOrEmpty(curNote))
                    {
                        str = "--" + curNote + "\n" + str;
                    }
                    if (isMsg)
                    {
                        str = str.Replace("message ", string.Format("---@class NetMsgDef.{0}.", tidyFilesLinkInfo[clsName])).Trim();
                        str = str.Replace("{", " : nil").Trim();
                        s_IsCurEnum = false;
                    }
                    else
                    {
                        string newEnumName = clsName;
                        if (s_CurClsName.Count > 0)
                            newEnumName = s_CurClsName[0] + newEnumName;
                        str = string.Format("---@class NetMsgDef.{0}.{1} : nil", tidyFilesLinkInfo[clsName], newEnumName);
                        s_IsCurEnum = true;
                    }
                    s_CurClsName.Add(clsName);
                }
            }
            else if (str.Contains("}"))
            {
                if (str.Trim().IndexOf("//") == 0)
                {
                    str = str.Trim().Replace("//", "--");
                    return str;
                }
                //组建结构结束
                if (!s_IsCurEnum)
                {
                    //string nameTemp = s_CurClsName[s_CurClsName.Count - 1];
                    //str = "local " + nameTemp + " = {}";
                    str = string.Empty;
                }
                else
                {
                    str = string.Empty;
                    foreach (string enumField in m_EnumFields)
                    {
                        str += enumField;
                    }
                    str += (tidyFilesLinkInfo[s_CurClsName[s_CurClsName.Count - 1]] + "_" + s_CurClsName[s_CurClsName.Count - 1] + " = {\n");
                    foreach (string enumMember in m_EnumMembers)
                    {
                        str += enumMember;
                    }
                    str += ("}\n");

                    m_EnumFields.Clear();
                    m_EnumMembers.Clear();
                }
                s_CurClsName.RemoveAt(s_CurClsName.Count - 1);
                if (str.Contains("//"))
                    str = str.Replace("//", "--").Trim();
                str += "\n";
                s_IsCurEnum = false;
            }
            else
            {
                if (s_IsCurEnum)
                {
                    PbEnumContent2LuaStr(ref str);
                }
                else
                {
                    PbMsgContent2LuaStr(tidyFilesLinkInfo, ref str);
                }
            }
            return str;
        }

        /// <summary>
        /// message内容生成注解
        /// </summary>
        /// <param name="tidyFilesLinkInfo">简洁文件名称</param>
        /// <param name="str"></param>
        private static void PbMsgContent2LuaStr(Dictionary<string, string> tidyFilesLinkInfo, ref string str)
        {
            int indexTemp = str.Trim().IndexOf("//");
            string curNote = string.Empty;
            if (indexTemp >= 0)
            {
                curNote = str.Trim().Substring(indexTemp + 2);
                if (indexTemp == 0)
                {
                    str = "--" + curNote;
                    return;
                }
            }
            string[] arrSplit = str.Split(' ');
            int index = 0;
            int trueIndex = 0;
            bool isArr = false;
            for (var i = 0; i < arrSplit.Length; i++)
            {
                string sp = arrSplit[i];
                index = index + 1;
                if (sp.Contains("required")
                    || sp.Contains("optional"))
                {
                    trueIndex = index;
                    break;
                }
                else if (sp.Contains("repeated"))
                {
                    trueIndex = index;
                    isArr = true;
                    break;
                }
                else if (!sp.Contains("//") && !string.IsNullOrEmpty(sp))
                {
                    trueIndex = index - 1;
                    break;
                }
            }
            string fieldType = arrSplit[trueIndex];
            if (fieldType == "int32"
                || fieldType == "int64"
                || fieldType == "float"
                || fieldType == "double"
                || fieldType == "uint32"
                || fieldType == "uint64"
                || fieldType == "sint64"
                || fieldType == "fixed32"
                || fieldType == "fixed64"
                || fieldType == "sfixde32"
                || fieldType == "sfixde64"
            )
            {
                fieldType = "number";
            }
            else if (fieldType == "bool")
            {
                fieldType = "boolean";
            }
            else if (fieldType == "bytes")
            {
                fieldType = "string";
            }
            else if(fieldType == "string")
            {
                fieldType = "string";
            }
            else if(fieldType.StartsWith("map"))
            {
                fieldType = "table";
            }
            else
            {
                if (tidyFilesLinkInfo.ContainsKey(fieldType))
                {
                    fieldType = "NetMsgDef." + tidyFilesLinkInfo[fieldType] + "." + fieldType;
                }
            }

            if (isArr)
            {
                fieldType += "[]";
            }
            string field = null;
            for (var i = trueIndex + 1; i < arrSplit.Length; i++)
            {
                if (arrSplit[i] != string.Empty && !arrSplit[i].Contains("<") && !arrSplit[i].Contains(">"))
                {
                    field = arrSplit[i];
                    break;
                }
            }

            if (!string.IsNullOrEmpty(field) && field != " ")
            {
                str = string.Format("---@field {0} {1}", field, fieldType);
                if (!string.IsNullOrEmpty(curNote))
                {
                    str += " @" + curNote;
                }
            }
        }

        /// <summary>
        /// 枚举内容生成注解
        /// </summary>
        /// <param name="str"></param>
        private static void PbEnumContent2LuaStr(ref string str)
        {
            if (string.IsNullOrEmpty(str.Trim()))
            {
                return;
            }
            if (str.Trim().IndexOf("//") == 0)
            {
                str = str.Trim().Replace("//", "--");
                return;
            }

            string curEnumName = str.Substring(0, str.IndexOf("=")).Trim();
            string curNote = string.Empty;
            int indexTemp = str.IndexOf("//");
            if (indexTemp > 0)
            {
                curNote = str.Substring(indexTemp + 2);
            }

            if (!string.IsNullOrEmpty(curNote))
            {
                m_EnumFields.Add(string.Format("----@field {0} string @{1}\n", curEnumName, curNote));
            }
            else
            {
                m_EnumFields.Add(string.Format("----@field {0} string\n", curEnumName));
            }

            if (indexTemp > 0)
            {
                m_EnumMembers.Add("    " + curEnumName + " = \"" + curEnumName + "\", --" + curNote + "\n");
            }
            else if (indexTemp == 0)
            {
                m_EnumMembers.Add(str.Replace("//", "--").Trim() + "\n");
            }
            else
            {
                m_EnumMembers.Add("    " + curEnumName + " = \"" + curEnumName + "\",\n");
            }

            str = string.Empty;
        }

    }
}
