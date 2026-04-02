using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Honor.Runtime;
using UnityEditor;

namespace Honor.Editor
{
    internal sealed partial class PersistComponentInspector
    {
         /// <summary>
        /// 生成所有存档Proto协议到Lua数据结构
        /// </summary>
        private static void GenerateLuaSaveDatas(List<List<string>> luaFilesDeclaresLines)
        {
            // 生成存档Root结构关键信息的收集
            List<string> fullNamesUnderRoot = new List<string>();
            List<string> tidyNamesUnderRoot = new List<string>();
            List<string> typeNamesUnderRoot = new List<string>();
            List<string> notesUnderRoot = new List<string>();

            for (int indexOfList = 0; indexOfList < luaFilesDeclaresLines.Count; indexOfList++)
            {
                List<string> luaFileDeclareLines = luaFilesDeclaresLines[indexOfList];

                string saveDataFileName = string.Empty;
                string saveDataFileTidyName = string.Empty;
                string saveDataFileNote = string.Empty;

                // 开始清理冗余内容，获取SaveData文件名称与文件描述
                for (int index = luaFileDeclareLines.Count - 1; index >= 0; index--)
                {
                    string content = luaFileDeclareLines[index];
                    if (content.StartsWith("--=====================================================================================================") ||
                        content.StartsWith("-- (c)copyright") ||
                        content.StartsWith("-- All Rights Reserved.") ||
                        content.StartsWith("-- ----------------------------------------------------------------------------------------------------") ||
                        content.StartsWith("-- filename:") ||
                        content.StartsWith("-- descrip:") ||
                        content.StartsWith("-- notices:"))
                    {
                        if (content.StartsWith("-- filename:"))
                        {
                            saveDataFileName = content.Split(' ')[2].Replace("Declare", string.Empty).Replace(".lua", string.Empty);
                            saveDataFileTidyName = content.Split(' ')[2].Replace("DeclareSave", string.Empty).Replace(".lua", string.Empty);
                        }
                        else if (content.StartsWith("-- descrip:"))
                        {
                            saveDataFileNote = content.Split(' ')[2];
                        }
                        luaFileDeclareLines.RemoveAt(index);
                    }
                }

                // 解析所有类型的成员定义
                Dictionary<string, string> classNoteInfos = new Dictionary<string, string>();  // <className, classNote>
                Dictionary<string, Dictionary<string, string>> classFieldTypes = new Dictionary<string, Dictionary<string, string>>();  // <className, <fieldName, fieldType>>
                Dictionary<string, Dictionary<string, string>> classFieldNotes = new Dictionary<string, Dictionary<string, string>>();  // <className, <fieldName, fieldNote>>
                string classNote = string.Empty;
                string className = string.Empty;  // SavePBMsgDef.XXXX.YYYY or 基础类型
                string classTidyName = string.Empty;

                for (int index = 0; index < luaFileDeclareLines.Count; index++)
                {
                    string content = luaFileDeclareLines[index];

                    // 注释
                    if (!content.StartsWith("-- 协议描述") && content.StartsWith("-- "))
                    {
                        classNote = content.Split(' ')[1];
                    }
                    else if (content.StartsWith("---@class")) // 类型定义
                    {
                        string[] strs = content.Split(' ');
                        className = strs[1];
                        classTidyName = className.Split('.')[2];
                        classNoteInfos.Add(className, classNote);
                        classFieldTypes.Add(className, new Dictionary<string, string>());
                        classFieldNotes.Add(className, new Dictionary<string, string>());
                    }
                    else if (content.StartsWith("---@field"))  // 成员类型
                    {
                        string[] strs = content.Split(' ');
                        string fieldName = strs[1];
                        string fieldType = strs[2];
                        string fieldNote = strs[4];
                        classFieldTypes[className].Add(fieldName, fieldType);
                        classFieldNotes[className].Add(fieldName, fieldNote);
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(className))
                        {
                            classNote = string.Empty;
                            className = string.Empty;
                        }
                    }
                }

                // 生成每个文件的存档主结构lua文件
                foreach (var classNoteInfo in classNoteInfos)
                {
                    if (classNoteInfo.Value.StartsWith("[主结构]"))
                    {
                        className = classNoteInfo.Key;
                        classNote = classNoteInfo.Value;

                        // 采集存档Root结构关键信息
                        fullNamesUnderRoot.Add(className.Split('.')[1]);
                        tidyNamesUnderRoot.Add(className.Split('.')[2]);
                        typeNamesUnderRoot.Add(className);
                        notesUnderRoot.Add(classNote);

                        // 开始添加文件头
                        StringBuilder stringBuilder = new StringBuilder();
                        stringBuilder.AppendLine("--=====================================================================================================")
                                     .AppendLine("-- (c) copyright 2026 - 2030, Honor.GameLib")
                                     .AppendLine("-- All Rights Reserved.")
                                     .AppendLine("-- ----------------------------------------------------------------------------------------------------")
                                     .AppendLine(AorTxt.Format("-- filename:  {0}.lua", saveDataFileName))
                                     .AppendLine(AorTxt.Format("-- descrip:   {0}", saveDataFileNote))
                                     .AppendLine("-- notices:   该文件自动生成，请不要手动修改！ 5541424142")
                                     .AppendLine("--=====================================================================================================")
                                     .AppendLine("");
                        
                        stringBuilder.AppendLine("PbRootData = PbRootData or {}");
                        stringBuilder.AppendLine(AorTxt.Format("PbRootData.{0} = PbRootData.{1} or {{}}",classTidyName,classTidyName));
                        stringBuilder.AppendLine(AorTxt.Format("---@type {0} @{1}", className, classNote));
                        stringBuilder.AppendLine(AorTxt.Format("PbRootData.{0} = {1}", saveDataFileTidyName, "{}"));
                        stringBuilder.AppendLine(AorTxt.Format(""));

                        string content = string.Empty;

                        // 结构声明内容（递归）
                        content = _RecurseFieldDefines("", classTidyName, className, classFieldTypes, classFieldNotes);
                        content += "\n";

                        // 自动化-load-接口
                        content += AorTxt.Format("PbIO = PbIO and PbIO or {0}\n", "{}");
                        content += AorTxt.Format("PbIO.{0} = {1}\n", classTidyName, "{}");
                        content += "----------------------------------------------------------------------------------------------\n";
                        content += _RecurseLoadAPIs("", "", saveDataFileName, classTidyName, className, classFieldTypes);

                        // 自动化-save-接口
                        content += "----------------------------------------------------------------------------------------------\n";
                        content += _RecurseSaveAPIs("", "", saveDataFileName, classTidyName, className, classFieldTypes);

                        // 自动化-序列化-接口
                        content += "----------------------------------------------------------------------------------------------\n";
                        content += _RecurseSerializeAPIs("", "", saveDataFileName, classTidyName, className, classFieldTypes);

                        // 自动化-反序列化-接口
                        content += "----------------------------------------------------------------------------------------------\n";
                        content += _RecurseDeserializeAPIs("", "", saveDataFileName, classTidyName, className, classFieldTypes);

                        // 返回代码
                        content += "----------------------------------------------------------------------------------------------\n";

                        string path = Runtime.GamePathUtils.Save.GetLuaScriptRootDirectoryFullPath() + "/" + saveDataFileName + ".lua.txt";
                        File.WriteAllText(path, stringBuilder.ToString() + content, new System.Text.UTF8Encoding(false));
                        Log.Debug("生成 " + path + " 文件成功。");

                        break;
                    }
                }
            }

            // 生成存档Root文件content（存档主干）
            StringBuilder rootStringBuilder = new StringBuilder();

            // 生成存档Root文件（存档主干）
            rootStringBuilder.AppendLine("--=====================================================================================================")
                         .AppendLine("-- (c) copyright 2026 - 2030, Honor.GameLib")
                         .AppendLine("-- All Rights Reserved.")
                         .AppendLine("-- ----------------------------------------------------------------------------------------------------")
                         .AppendLine(AorTxt.Format("-- filename:  PbRootData.lua"))
                         .AppendLine(AorTxt.Format("-- descrip:   存档主干数据"))
                         .AppendLine("-- notices:   该文件自动生成，请不要手动修改！")
                         .AppendLine("--=====================================================================================================")
                         .AppendLine("");

            rootStringBuilder.AppendLine(AorTxt.Format("---@class PbRootData @存档主干数据"));
            for (int index = 0; index < tidyNamesUnderRoot.Count; index++)
            {
                rootStringBuilder.AppendLine(AorTxt.Format("---@field {0} {1} @{2}", tidyNamesUnderRoot[index], typeNamesUnderRoot[index], notesUnderRoot[index]));
            }
            rootStringBuilder.AppendLine("PbRootData = {}");

            rootStringBuilder.AppendLine(AorTxt.Format(""));

            for (int index = 0; index < tidyNamesUnderRoot.Count; index++)
            {
                rootStringBuilder.AppendLine(AorTxt.Format("require('{1}')", tidyNamesUnderRoot[index], fullNamesUnderRoot[index]));
            }

            rootStringBuilder.AppendLine(AorTxt.Format(""));

            // for (int index = 0; index < tidyNamesUnderRoot.Count; index++)
            // {
            //     rootStringBuilder.AppendLine(AorTxt.Format("InitProtoFile('{0}.proto')", fullNamesUnderRoot[index]));
            // }
            //
            // rootStringBuilder.AppendLine(AorTxt.Format(""));

            for (int index = 0; index < tidyNamesUnderRoot.Count; index++)
            {
                rootStringBuilder.AppendLine(AorTxt.Format("PbIO.{0}.LoadAll()", tidyNamesUnderRoot[index]));
            }

            rootStringBuilder.AppendLine(AorTxt.Format(""));

            rootStringBuilder.AppendLine(AorTxt.Format("return PbRootData"));


            string saveRootPath = Runtime.GamePathUtils.Save.GetLuaScriptRootDirectoryFullPath() + "/PbRootData.lua.txt";
            File.WriteAllText(saveRootPath, rootStringBuilder.ToString(), new System.Text.UTF8Encoding(false));
            Log.Debug("生成 " + saveRootPath + " 文件成功。");

        }

        /// <summary>
        /// 生成Lua层Protos文件
        /// </summary>
        private void GenerateLuaProtos()
        {
            if (System.IO.Directory.Exists(Runtime.GamePathUtils.Proto.Save.GetLuaScriptProtosDirectoryFullPath()))
            {
                System.IO.Directory.Delete(Runtime.GamePathUtils.Proto.Save.GetLuaScriptProtosDirectoryFullPath(), true);
            }
            System.IO.Directory.CreateDirectory(Runtime.GamePathUtils.Proto.Save.GetLuaScriptProtosDirectoryFullPath());
            Log.Debug(AorTxt.Format("清空{0}文件夹成功。", Runtime.GamePathUtils.Proto.Save.GetLuaScriptProtosDirectoryFullPath()));
            AssetDatabase.Refresh();

            string[] fileFullPaths = System.IO.Directory.GetFiles(Runtime.GamePathUtils.Proto.Save.GetRootDirectoryFullPath(), "*.proto", System.IO.SearchOption.AllDirectories);
            for (int index = 0; index < fileFullPaths.Length; index++)
            {
                string path = fileFullPaths[index].Replace('\\', '/');
                string fileName = path.Substring(path.LastIndexOf("/") + 1, path.Length - path.LastIndexOf("/") - 1);
                string content = System.IO.File.ReadAllText(path);
                string desc = string.Empty;
                string startStr = "协议描述";
                if (content.Contains(startStr))
                {
                    string input = AorTxt.Format(@"(?<=({0}))\S*(?=(\r\n))", startStr);
                    MatchCollection matchCollection = Regex.Matches(content, input);
                    if (matchCollection.Count > 0)
                    {
                        desc = matchCollection[0].ToString().Replace(":", string.Empty).Replace("：", string.Empty);
                    }
                }

                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine(AorTxt.Format("-- ====================================================================================================="))
                             .AppendLine(AorTxt.Format("-- (c) copyright 2026 - 2030, Honor.GameLib"))
                             .AppendLine(AorTxt.Format("-- All Rights Reserved."))
                             .AppendLine(AorTxt.Format("-- ----------------------------------------------------------------------------------------------------"))
                             .AppendLine(AorTxt.Format("-- filename:  {0}.lua", fileName));
                if (!string.IsNullOrEmpty(desc))
                {
                    stringBuilder.AppendLine(AorTxt.Format("-- descrip:   {0}", desc));
                }
                stringBuilder.AppendLine(AorTxt.Format("-- notices:   该文件自动生成，请不要手动修改！"));
                stringBuilder.AppendLine(AorTxt.Format("--====================================================================================================="))
                             .AppendLine(AorTxt.Format(""));
                stringBuilder.AppendLine(AorTxt.Format("-- proto.Schema结构"))
                             .AppendLine(AorTxt.Format(""))
                             .AppendLine(AorTxt.Format("return [["))
                             .AppendLine(AorTxt.Format(""))
                             .Append(content)
                             .AppendLine("]]");

                string luaScriptName = AorTxt.Format("{0}.lua.txt", fileName);
                string outputPath = AorTxt.Format("{0}/{1}", Runtime.GamePathUtils.Proto.Save.GetLuaScriptProtosDirectoryFullPath(), luaScriptName);
                System.IO.File.WriteAllText(outputPath, stringBuilder.ToString(), new System.Text.UTF8Encoding(false));

                Log.Debug(AorTxt.Format("生成{0}文件成功。", outputPath));

            }
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 生成Lua层Declare文件
        /// </summary>
        private static void GenerateLuaDeclares(out List<List<string>> luaFilesDeclaresLines, bool createFiles = true)
        {
            luaFilesDeclaresLines = new List<List<string>>();

            string[] protoSrcPaths = Directory.GetFiles(GamePathUtils.Proto.Save.GetRootDirectoryFullPath(), "*.proto");
            string targetDirectoryPath = Runtime.GamePathUtils.Proto.Save.GetLuaScriptDeclarationsDirectoryFullPath();
            if (!Directory.Exists(targetDirectoryPath))
            {
                Directory.CreateDirectory(targetDirectoryPath);
            }

            List<string> splitedProtoLines = new List<string>();
            for (var i = 0; i < protoSrcPaths.Length; i++)
            {
                s_IsCurEnum = false;
                s_CurClsName.Clear();
                splitedProtoLines.Clear();

                FileInfo info = new FileInfo(protoSrcPaths[i]);
                string fileName = info.Name.Replace(".proto", ".lua");
                string tidyFileName = info.Name.Replace(".proto", string.Empty);

                string[] protoSrcLines = File.ReadAllLines(protoSrcPaths[i]);
                splitedProtoLines.AddRange(_SplitProtoLines(protoSrcLines));

                string desc = string.Empty;
                string startStr = "协议描述";
                for (int index = 0; index < protoSrcLines.Length; index++)
                {
                    if (protoSrcLines[index].Contains(startStr))
                    {
                        string input = AorTxt.Format(@"(?<=({0}))\S*(?=())", startStr);
                        MatchCollection matchCollection = Regex.Matches(protoSrcLines[index], input);
                        if (matchCollection.Count > 0)
                        {
                            desc = matchCollection[0].ToString().Replace(":", string.Empty).Replace("：", string.Empty);
                        }
                    }
                }

                List<string> finalLines = new List<string>();

                if (createFiles)
                {
                    finalLines.Add(AorTxt.Format("--====================================================================================================="));
                    finalLines.Add(AorTxt.Format("-- (c)copyright 2026 - 2030, Honor.GameLib"));
                    finalLines.Add(AorTxt.Format("-- All Rights Reserved."));
                    finalLines.Add(AorTxt.Format("-- ----------------------------------------------------------------------------------------------------"));
                    finalLines.Add(AorTxt.Format("-- filename: Declare{0}", fileName));
                    if (!string.IsNullOrEmpty(desc))
                    {
                        finalLines.Add(AorTxt.Format("-- descrip: {0}", desc));
                    }
                    finalLines.Add(AorTxt.Format("-- notices: 该文件自动生成，请不要手动修改！"));
                    finalLines.Add(AorTxt.Format("--====================================================================================================="));
                    finalLines.Add("");
                }

                for (var j = 0; j < splitedProtoLines.Count; j++)
                {
                    string line = _GenerateLuaDeclareLine(tidyFileName, splitedProtoLines[j]);
                    if (!line.Equals(string.Empty))
                    {
                        finalLines.Add(line);
                    }
                }

                if (createFiles)
                {
                    string targetLuaDeclareFilePath = targetDirectoryPath + "/Declare" + fileName + ".txt";
                    File.WriteAllLines(targetLuaDeclareFilePath, finalLines.ToArray());
                    Log.Debug("生成 " + targetLuaDeclareFilePath + " 文件成功。");
                }

                luaFilesDeclaresLines.Add(finalLines);
            }

        }

        /// <summary>
        /// 字段信息声明（递归）
        /// </summary>
        /// <param name="content"></param>
        /// <param name="saveDataTidyName"></param>
        /// <param name="className"></param>
        /// <param name="classFieldTypes"></param>
        /// <param name="classFieldNotes"></param>
        /// <returns></returns>
        private static string _RecurseFieldDefines(string content, string saveDataTidyName, string className, Dictionary<string, Dictionary<string, string>> classFieldTypes, Dictionary<string, Dictionary<string, string>> classFieldNotes)
        {
            bool isArray = className.EndsWith("[]");
            if (!isArray)
            {
                foreach (var fieldInfo in classFieldTypes[className])
                {
                    string fieldName = fieldInfo.Key;
                    string fieldType = fieldInfo.Value;
                    string fieldNote = classFieldNotes[className][fieldName];
                    string tmpPrefix = AorTxt.Format("{0}.{1}", saveDataTidyName, fieldName);
                    if ((fieldType == "string" && fieldNote.Contains("[json]")) || fieldType.EndsWith("[]"))
                    {
                        fieldType = "table";
                    }
                    string comment = $"---@type {fieldType} @{fieldNote}\n";

                    // 自定义类型
                    if (fieldType.Contains("SavePBMsgDef"))
                    {
                        content += comment;
                        content += $"PbRootData.{saveDataTidyName}.{fieldName} = " + "{}" + "\n\n";
                        content = _RecurseFieldDefines(content, tmpPrefix, fieldType, classFieldTypes, classFieldNotes);
                    }
                    else // 普通类型
                    {
                        string defaultValue = "";
                        switch (fieldType)
                        {
                            case "number": defaultValue = "0"; break;
                            case "string": defaultValue = "''"; break;
                            case "boolean": defaultValue = "false"; break;
                            case "table": defaultValue = "{}"; break;
                        }
                        content += comment;
                        content += $"PbRootData.{saveDataTidyName}.{fieldName} = {defaultValue}\n\n";
                    }
                }
            }
            return content;
        }

        /// <summary>
        /// Load接口生成（递归）
        /// </summary>
        /// <param name="content"></param>
        /// <param name="prefix"></param>
        /// <param name="saveDataFileName"></param>
        /// <param name="saveDataTidyName"></param>
        /// <param name="className"></param>
        /// <param name="classFieldTypes"></param>
        /// <returns></returns>
        private static string _RecurseLoadAPIs(string content, string prefix, string saveDataFileName, string saveDataTidyName, string className, Dictionary<string, Dictionary<string, string>> classFieldTypes)
        {
            // 需要生成LoadAll接口
            if(string.IsNullOrEmpty(prefix))
            {
                content += $"PbIO.{saveDataTidyName}.LoadAll = function()\n";
                foreach (var fieldInfo in classFieldTypes[className])
                {
                    string fieldName = fieldInfo.Key;
                    content += $"    PbIO.{saveDataTidyName}.Load_{fieldName}()\n";
                }
                content += "end\n";
            }
            foreach (var fieldInfo in classFieldTypes[className])
            {
                string fieldName = fieldInfo.Key;
                string fieldType = fieldInfo.Value;
                string tmpPrefix = string.IsNullOrEmpty(prefix) ? $"{fieldName}" : $"{prefix}_{fieldName}";
                string tmpFields = tmpPrefix.Replace('_', '.');  // xxx.yyy.zzz.aaa.sss

                if(fieldType.EndsWith("[]"))
                {
                    fieldType = "table";
                }

                // 自定义类型
                if (fieldType.Contains("SavePBMsgDef"))
                {
                    content += $"PbIO.{saveDataTidyName}.Load_{tmpPrefix} = function()\n";
                    foreach (var itr in classFieldTypes[fieldType])
                    {
                        content += $"    PbIO.{saveDataTidyName}.Load_{tmpPrefix}_{itr.Key}()\n";
                    }
                    content += "end\n";
                }
                else // 普通类型
                {
                    content += $"PbIO.{saveDataTidyName}.Load_{tmpPrefix} = function()\n";
                    content += $"    if GameMainRoot.Persist:HasItem(Honor.PersistWayType.PlayerPrefs, '{saveDataFileName}', '{saveDataFileName}.{tmpFields}') then\n";
                    switch (fieldType)
                    {
                        case "number": content += $"        PbRootData.{saveDataTidyName}.{tmpFields} = tonumber(GameMainRoot.Persist:GetString(Honor.PersistWayType.PlayerPrefs, '{saveDataFileName}', '{saveDataFileName}.{tmpFields}'))\n"; break;
                        case "string": content += $"        PbRootData.{saveDataTidyName}.{tmpFields} = GameMainRoot.Persist:GetString(Honor.PersistWayType.PlayerPrefs, '{saveDataFileName}', '{saveDataFileName}.{tmpFields}')\n"; break;
                        case "table": content += $"         PbRootData.{saveDataTidyName}.{tmpFields} = JsonDecode(GameMainRoot.Persist:GetString(Honor.PersistWayType.PlayerPrefs, '{saveDataFileName}', '{saveDataFileName}.{tmpFields}'))\n"; break;
                        case "boolean": content += $"       PbRootData.{saveDataTidyName}.{tmpFields} = GameMainRoot.Persist:GetString(Honor.PersistWayType.PlayerPrefs, '{saveDataFileName}', '{saveDataFileName}.{tmpFields}') == 'true' and true or false\n"; break;
                    }
                    content += "    end\n";
                    content += "end\n";
                }
            }
            foreach (var fieldInfo in classFieldTypes[className])
            {
                string fieldName = fieldInfo.Key;
                string fieldType = fieldInfo.Value;
                string tmpPrefix = string.IsNullOrEmpty(prefix) ? $"{fieldName}" : $"{prefix}_{fieldName}";
                string tmpFields = tmpPrefix.Replace('_', '.');  // xxx.yyy.zzz.aaa.sss

                if (fieldType.EndsWith("[]"))
                {
                    fieldType = "table";
                }

                // 自定义类型
                if (fieldType.Contains("SavePBMsgDef"))
                {
                    content = _RecurseLoadAPIs(content, tmpPrefix, saveDataFileName, saveDataTidyName, fieldType, classFieldTypes);
                }
            }

            return content;
        }

        /// <summary>
        /// Save接口生成（递归）
        /// </summary>
        /// <param name="content"></param>
        /// <param name="prefix"></param>
        /// <param name="saveDataFileName"></param>
        /// <param name="saveDataTidyName"></param>
        /// <param name="className"></param>
        /// <param name="classFieldTypes"></param>
        /// <returns></returns>
        private static string _RecurseSaveAPIs(string content, string prefix, string saveDataFileName, string saveDataTidyName, string className, Dictionary<string, Dictionary<string, string>> classFieldTypes)
        {
            // 需要生成SaveAll接口
            if (string.IsNullOrEmpty(prefix))
            {
                content += $"PbIO.{saveDataTidyName}.SaveAll = function()\n";
                foreach (var fieldInfo in classFieldTypes[className])
                {
                    string fieldName = fieldInfo.Key;
                    content += $"    PbIO.{saveDataTidyName}.Save_{fieldName}()\n";
                }
                content += "end\n";
            }
            foreach (var fieldInfo in classFieldTypes[className])
            {
                string fieldName = fieldInfo.Key;
                string fieldType = fieldInfo.Value;
                string tmpPrefix = string.IsNullOrEmpty(prefix) ? $"{fieldName}" : $"{prefix}_{fieldName}";
                string tmpFields = tmpPrefix.Replace('_', '.');  // xxx.yyy.zzz.aaa.sss

                if (fieldType.EndsWith("[]"))
                {
                    fieldType = "table";
                }

                // 自定义类型
                if (fieldType.Contains("SavePBMsgDef"))
                {
                    content += $"PbIO.{saveDataTidyName}.Save_{tmpPrefix} = function()\n";
                    foreach (var itr in classFieldTypes[fieldType])
                    {
                        content += $"    PbIO.{saveDataTidyName}.Save_{tmpPrefix}_{itr.Key}()\n";
                    }
                    content += "end\n";
                }
                else // 普通类型
                {
                    content += $"PbIO.{saveDataTidyName}.Save_{tmpPrefix} = function()\n";
                    switch (fieldType)
                    {
                        case "number": content += $"    GameMainRoot.Persist:SetString(Honor.PersistWayType.PlayerPrefs, '{saveDataFileName}', '{saveDataFileName}.{tmpFields}', tostring(PbRootData.{saveDataTidyName}.{tmpFields}))\n"; break;
                        case "string": content += $"    GameMainRoot.Persist:SetString(Honor.PersistWayType.PlayerPrefs, '{saveDataFileName}', '{saveDataFileName}.{tmpFields}', tostring(PbRootData.{saveDataTidyName}.{tmpFields}))\n"; break;
                        case "table": content += $"    GameMainRoot.Persist:SetString(Honor.PersistWayType.PlayerPrefs, '{saveDataFileName}', '{saveDataFileName}.{tmpFields}', JsonEncode(PbRootData.{saveDataTidyName}.{tmpFields}))\n"; break;
                        case "boolean": content += $"    GameMainRoot.Persist:SetString(Honor.PersistWayType.PlayerPrefs, '{saveDataFileName}', '{saveDataFileName}.{tmpFields}', PbRootData.{saveDataTidyName}.{tmpFields} == true and 'true' or 'false')\n"; break;
                    }
                   content += "end\n";
                }
            }
            foreach (var fieldInfo in classFieldTypes[className])
            {
                string fieldName = fieldInfo.Key;
                string fieldType = fieldInfo.Value;
                string tmpPrefix = string.IsNullOrEmpty(prefix) ? $"{fieldName}" : $"{prefix}_{fieldName}";
                string tmpFields = tmpPrefix.Replace('_', '.');  // xxx.yyy.zzz.aaa.sss

                if (fieldType.EndsWith("[]"))
                {
                    fieldType = "table";
                }

                // 自定义类型
                if (fieldType.Contains("SavePBMsgDef"))
                {
                    content = _RecurseSaveAPIs(content, tmpPrefix, saveDataFileName, saveDataTidyName, fieldType, classFieldTypes);
                }
            }

            return content;
        }
        

        /// <summary>
        /// Serialize接口生成（递归）
        /// </summary>
        /// <param name="content"></param>
        /// <param name="prefix"></param>
        /// <param name="saveDataFileName"></param>
        /// <param name="saveDataTidyName"></param>
        /// <param name="className"></param>
        /// <param name="classFieldTypes"></param>
        /// <returns></returns>
        private static string _RecurseSerializeAPIs(string content, string prefix, string saveDataFileName, string saveDataTidyName, string className, Dictionary<string, Dictionary<string, string>> classFieldTypes)
        {
            // 需要生成SerializeAll接口
            if (string.IsNullOrEmpty(prefix))
            {
                content += $"PbIO.{saveDataTidyName}.SerializeAll = function()\n";
                content += $"    return JsonEncode(PbRootData.{saveDataTidyName})\n";
                content += "end\n";
            }
            foreach (var fieldInfo in classFieldTypes[className])
            {
                string fieldName = fieldInfo.Key;
                string fieldType = fieldInfo.Value;
                string tmpPrefix = string.IsNullOrEmpty(prefix) ? $"{fieldName}" : $"{prefix}_{fieldName}";
                string tmpFields = tmpPrefix.Replace('_', '.');  // xxx.yyy.zzz.aaa.sss

                content += $"PbIO.{saveDataTidyName}.Serialize_{tmpPrefix} = function()\n";
                content += $"    return JsonEncode(PbRootData.{saveDataTidyName}.{tmpFields})\n";
                content += "end\n";
            }
            foreach (var fieldInfo in classFieldTypes[className])
            {
                string fieldName = fieldInfo.Key;
                string fieldType = fieldInfo.Value;
                string tmpPrefix = string.IsNullOrEmpty(prefix) ? $"{fieldName}" : $"{prefix}_{fieldName}";
                string tmpFields = tmpPrefix.Replace('_', '.');  // xxx.yyy.zzz.aaa.sss

                if (fieldType.EndsWith("[]"))
                {
                    fieldType = "table";
                }

                // 自定义类型
                if (fieldType.Contains("SavePBMsgDef"))
                {
                    content = _RecurseSerializeAPIs(content, tmpPrefix, saveDataFileName, saveDataTidyName, fieldType, classFieldTypes);
                }
            }

            return content;
        }

        /// <summary>
        /// Deserialize接口生成（递归）
        /// </summary>
        /// <param name="content"></param>
        /// <param name="prefix"></param>
        /// <param name="saveDataFileName"></param>
        /// <param name="saveDataTidyName"></param>
        /// <param name="className"></param>
        /// <param name="classFieldTypes"></param>
        /// <returns></returns>
        private static string _RecurseDeserializeAPIs(string content, string prefix, string saveDataFileName, string saveDataTidyName, string className, Dictionary<string, Dictionary<string, string>> classFieldTypes)
        {
            // 需要生成DeserializeAll接口
            if (string.IsNullOrEmpty(prefix))
            {
                content += $"PbIO.{saveDataTidyName}.DeserializeAll = function(jsonString)\n";
                content += $"    PbRootData.{saveDataTidyName} = JsonDecode(jsonString)\n";
                content += "end\n";
            }
            foreach (var fieldInfo in classFieldTypes[className])
            {
                string fieldName = fieldInfo.Key;
                string fieldType = fieldInfo.Value;
                string tmpPrefix = string.IsNullOrEmpty(prefix) ? $"{fieldName}" : $"{prefix}_{fieldName}";
                string tmpFields = tmpPrefix.Replace('_', '.');  // xxx.yyy.zzz.aaa.sss

                content += $"PbIO.{saveDataTidyName}.Deserialize_{tmpPrefix} = function(jsonString)\n";
                content += $"    PbRootData.{saveDataTidyName}.{tmpFields} = JsonDecode(jsonString)\n";
                content += "end\n";
            }
            foreach (var fieldInfo in classFieldTypes[className])
            {
                string fieldName = fieldInfo.Key;
                string fieldType = fieldInfo.Value;
                string tmpPrefix = string.IsNullOrEmpty(prefix) ? $"{fieldName}" : $"{prefix}_{fieldName}";
                string tmpFields = tmpPrefix.Replace('_', '.');  // xxx.yyy.zzz.aaa.sss

                if (fieldType.EndsWith("[]"))
                {
                    fieldType = "table";
                }

                // 自定义类型
                if (fieldType.Contains("SavePBMsgDef"))
                {
                    content = _RecurseDeserializeAPIs(content, tmpPrefix, saveDataFileName, saveDataTidyName, fieldType, classFieldTypes);
                }
            }

            return content;
        }

        /// <summary>
        /// 拆分Proto源文件数据行
        /// </summary>
        /// <param name="lines"></param>
        private static List<string> _SplitProtoLines(string[] lines)
        {
            List<string> finnalLines = new List<string>();

            string curContentTemp = String.Empty;
            Dictionary<int, List<string>> cacheLinesDic = new Dictionary<int, List<string>>();
            Dictionary<int, List<string>> finalLinesDic = new Dictionary<int, List<string>>();
            Dictionary<string, string> replaceRuleDic = new Dictionary<string, string>();
            string curClsName = string.Empty;
            int curClsIndex = 0;
            int totalClsCount = 0;
            for (int index = 0; index < lines.Length; index++)
            {
                curContentTemp = lines[index];
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
            return finnalLines;
        }

        /// <summary>
        /// 生成Lua的注解行
        /// </summary>
        /// <param name="tidyFileName"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        private static string _GenerateLuaDeclareLine(string tidyFileName, string str)
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
                        str = str.Replace("message ", AorTxt.Format("---@class SavePBMsgDef.{0}.", tidyFileName)).Trim();
                        str = str.Replace("{", " : nil").Trim();
                        s_IsCurEnum = false;
                    }
                    else
                    {
                        string newEnumName = clsName;
                        if (s_CurClsName.Count > 0)
                            newEnumName = s_CurClsName[0] + newEnumName;
                        str = "---@class " + newEnumName + " : nil\n" + newEnumName + "= {";
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
                    str = string.Empty;
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
                    _ConvertEnumLineToDeclareLine(ref str);
                }
                else
                {
                    _ConvertMsgLineToDeclareLine(tidyFileName, ref str);
                }
            }

            return str;
        }

        /// <summary>
        /// 转换message为Declare注解
        /// </summary>
        /// <param name="tidyFileName">简洁文件名称</param>
        /// <param name="str"></param>
        private static void _ConvertMsgLineToDeclareLine(string tidyFileName, ref string str)
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
            else if (fieldType == "string")
            {
                fieldType = "string";
            }
            else if (fieldType.StartsWith("map"))   // pb中的map结构
            {
                fieldType = "table";
            }
            else
            {
                fieldType = "SavePBMsgDef." + tidyFileName + "." + fieldType;
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
                if (fieldType == "string" && !string.IsNullOrEmpty(curNote) && curNote.Contains("[json]"))
                {
                    fieldType = "table";
                }

                str = string.Format("---@field {0} {1}", field, fieldType);

                if (!string.IsNullOrEmpty(curNote))
                {
                    str += " @" + curNote;
                }
            }
        }

        /// <summary>
        /// 转换枚举为Declare注解
        /// </summary>
        /// <param name="str"></param>
        private static void _ConvertEnumLineToDeclareLine(ref string str)
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
            int indexTemp = str.IndexOf("//");
            if (indexTemp > 0)
            {
                string curNote = str.Substring(indexTemp + 2);
                str = "    " + curEnumName + " = \"" + curEnumName + "\"; --" + curNote;
            }
            else if (indexTemp == 0)
            {
                str = str.Replace("//", "--").Trim();
            }
            else
            {
                str = "    " + curEnumName + " = \"" + curEnumName + "\";";
            }
        }
    }
}