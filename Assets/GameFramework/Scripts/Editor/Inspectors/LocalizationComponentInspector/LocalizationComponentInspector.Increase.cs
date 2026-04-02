using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;

namespace Honor.Editor
{
    internal sealed partial class LocalizationComponentInspector
    {
         /// <summary>
        /// 查找导出文件中的翻译  LocalPart_xxxx_{keyFileName}.lua.txt 与  Localization{keyFileName}_*Increase.lua.txt
        /// </summary>
        private static void GetIncreaseFileContent(Dictionary<string, string> res, string searchPattern)
        {
            string pattern = @"^(?:[\w]+)\.([\w]+) = ""(.*)""$";;
            string nilPattern = @"^(?:[\w]+)\.([\w]+)";
            var files = Directory.GetFiles(EditorPath.Localization.LuaScriptsFolderFullPath, searchPattern, SearchOption.AllDirectories);
            if (files != null && files.Length > 0)
            {
                List<string> toSort = new List<string>(files);
                toSort.Sort((x, y) =>
                {
                    if (x.Length != y.Length) return x.Length - y.Length;
                    else return string.Compare(x, y);
                });
                files = toSort.ToArray();
                for (int i = 0; i < files.Length; i++)
                {
                    FileInfo fi = new FileInfo(files[i]);
                    // 使用 StreamReader 逐行读取文件内容
                    using (StreamReader reader = fi.OpenText())
                    {
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            if (line.Contains(" = \""))
                            {
                                Match match = Regex.Match(line, pattern);
                                if (match.Success)
                                {
                                    // 输出匹配到的结果
                                    string key = match.Groups[1].Value;
                                    string value = match.Groups[2].Value;
                                    res[key] = value;
                                }
                                else
                                {
                                    match = Regex.Match(line, nilPattern);
                                    if (match.Success)
                                    {
                                        // 输出匹配到的结果
                                        string key = match.Groups[1].Value;
                                        if (line.Contains("nil")) res[key] = "nil";
                                        else res[key] = "";
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        
        /// <summary>
        /// 查看是否有增量更新文件
        /// </summary>
        private static string[] FindIncrementalFile(string keyFileName)
        {
            var res = Directory.GetFiles(EditorPath.Localization.LuaScriptsFolderFullPath, $"Localization{keyFileName}_*Increase.lua.txt", SearchOption.AllDirectories);
            if (res != null && res.Length > 0)
            {
                for (int i = 0; i < res.Length; i++)
                {
                    var fileName = Path.GetFileName(res[i]).Replace(".lua.txt", "");
                    res[i] = fileName;
                }
            }

            return res;
        }

        /// <summary>
        /// 计算下一个增量版本是多少
        /// </summary>
        /// <returns></returns>
        private static int GetNextIncreaseFileVersion(string keyFileName)
        {
            // 匹配"_"和"Increase"之间的数字
            string pattern = @"_(\d+)Increase";
            var files = Directory.GetFiles(EditorPath.Localization.LuaScriptsFolderFullPath, $"Localization{keyFileName}_*Increase.lua.txt", SearchOption.AllDirectories);
            int res = 0;
            if (res != null && files.Length > 0)
            {
                for (int i = 0; i < files.Length; i++)
                {
                    Match match = Regex.Match(files[i], pattern);

                    if (match.Success && int.TryParse(match.Groups[1].Value, out int version) && version > res)
                    {
                        res = version;
                    }
                }
            }
            res++;
            return res;
        }
        
        private static int GetNextIncreaseDirector(string exportLuaIncreasePath)
        {
            int res = 1;
            var rootPath = exportLuaIncreasePath;//EditorPath.Localization.LuaIncreaseFolder;
            var dir = rootPath + "/" + res;
            while (Directory.Exists(dir))
            {
                res++;
                dir = rootPath + "/" + res;
            }
            return res;
        }

        private static void CollectAndCheckKeyRepeat(string key)
        {
            if(m_AllKey.Contains(key))
                m_RepeatKey.Add(key);
            else
                m_AllKey.Add(key);
        }
        
        /// <summary>
        /// 收集需要增量的翻译文本
        /// </summary>
        private static void FindIncrease(int columns, List<string> fullPaths, string[] keyList, string tableDetailTidy, string[] langInfo, bool exportLuaIncreaseSingleVersion, string exportLuaIncreasePath)
        {
            var targetDir = exportLuaIncreasePath;
            if (exportLuaIncreaseSingleVersion && Directory.Exists(targetDir))
            {
                Directory.Delete(targetDir, true);
            }
            AssetDatabase.Refresh();
            var version = GetNextIncreaseDirector(exportLuaIncreasePath);
            for (int i = 6; i < columns; i++)
            {
                if (keyList[i] != string.Empty)
                {
                    //最新配置表的翻译 - 新的
                    Dictionary<string, DataRow> newDatas = new Dictionary<string, DataRow>();
                    for (int index = 0; index < fullPaths.Count; index++)
                    {
                        DataSet excelDatas = TableExportEditorUtility.GetExcelData(fullPaths[index]);
                        int rows = excelDatas.Tables[0].Rows.Count;
                        for (int row = 4; row < rows; row++)
                        {
                            if (excelDatas.Tables[0].Rows[row][1] != null)
                            {
                                var key = excelDatas.Tables[0].Rows[row][1].ToString();
                                var cell = excelDatas.Tables[0].Rows[row];
                                newDatas[key] = cell;
                                if (i == 6)
                                {
                                    CollectAndCheckKeyRepeat(key);
                                }
                            }
                        }
                    }

                    //已经导出过的翻译 - 旧的
                    Dictionary<string, string> oldDatas = new Dictionary<string, string>();
                    for (int index = 0; index < fullPaths.Count; index++)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(fullPaths[index]);
                        GetIncreaseFileContent(oldDatas, $"LocalPart_{fileName}_{keyList[i]}.lua.txt");
                    }
                    GetIncreaseFileContent(oldDatas, $"Localization{keyList[i]}_*Increase.lua.txt");
                    
                    Dictionary<string, DataRow> resAddChange = new Dictionary<string, DataRow>();
                    List<string> resRemove = new List<string>();
                    foreach (var newData in newDatas)
                    {
                        if (oldDatas.TryGetValue(newData.Key, out var oldData))
                        {
                            if (oldData != newData.Value[i].ToString()) //修改了
                            {
                                resAddChange.Add(newData.Key, newData.Value);
                            }
                        }
                        else//新增
                        {
                            resAddChange.Add(newData.Key, newData.Value);
                        }
                    }

                    foreach (var oldData in oldDatas)
                    {
                        if (!newDatas.ContainsKey(oldData.Key) && oldData.Value != "nil")//删除了key
                        {
                            if (!resRemove.Contains(oldData.Key))
                            {
                                resRemove.Add(oldData.Key);
                            }
                        }
                    }

                    if (resAddChange.Count > 0 || resRemove.Count > 0)
                    {
                        MakeIncreaseFile(resAddChange, resRemove, keyList[i], tableDetailTidy, langInfo, i, version, exportLuaIncreasePath);
                    }
                }
            }
        }

        /// <summary>
        /// 生成增量文件
        /// </summary>
        private static void MakeIncreaseFile(Dictionary<string, DataRow> resAddChange, List<string> resRemove, string keyFileName, string tableDetailTidy, string[] langInfo, int col, int versionDir, string exportLuaIncreasePath)
        {
            var version = GetNextIncreaseFileVersion(keyFileName);
            var folder = exportLuaIncreasePath;//EditorPath.Localization.LuaIncreaseFolder;
            folder = $"{folder}/{versionDir}";
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            string fileName = "Localization" + keyFileName + $"_{version}Increase.lua.txt";
            string filePath = folder + "/" + fileName;
            StringBuilder stringBuilder = new StringBuilder();
            MakeLuaFileTitle(stringBuilder, fileName, null, $"{tableDetailTidy}: {langInfo[col]}");

            string lowerKeyFileName = keyFileName.ToLower();
            stringBuilder.AppendLine($"---@type Localizations.{keyFileName} @{tableDetailTidy}: {langInfo[col]}");
            stringBuilder.AppendLine($"local {lowerKeyFileName} = Localizations.{keyFileName}");
            stringBuilder.AppendLine("");
            
            foreach (var iter in resAddChange)
            {
                stringBuilder.AppendLine($"---@field {iter.Value[1]} string @{iter.Value[2]}");
                stringBuilder.AppendLine($"{lowerKeyFileName}.{iter.Value[1]} = \"{iter.Value[col]}\"");
                stringBuilder.AppendLine("");
            }

            foreach (var keyRemove in resRemove)
            {
                stringBuilder.AppendLine($"{lowerKeyFileName}.{keyRemove} = nil");
                stringBuilder.AppendLine("");
            }
            
            stringBuilder.AppendLine($"---@type Localizations.{keyFileName} @{tableDetailTidy}: {langInfo[col]}");
            stringBuilder.AppendLine($"Localization.{keyFileName} = {lowerKeyFileName}");
            stringBuilder.AppendLine("");
            
            File.WriteAllText(filePath, stringBuilder.ToString(), new System.Text.UTF8Encoding(false));
        }
    }
}