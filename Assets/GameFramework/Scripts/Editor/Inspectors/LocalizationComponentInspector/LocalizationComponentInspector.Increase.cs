/***************************************************************
 * (c) copyright 2026 - 2030, Honor
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  LocalizationComponentInspector.Increase.cs
 * author:    云毅
 * created:   2026   2026
 * descrip:   本地化组件编辑器 - 增量导出扩展分部类
 *            负责：增量对比、版本管理、Lua增量文件生成、Key查重
 ***************************************************************/
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;

namespace Honor.Editor
{
    /// <summary>
    /// 本地化组件编辑器检视面板 扩展类
    /// 负责本地化文件的增量更新、文件查找、内容解析、版本管理等核心逻辑
    /// </summary>
    internal sealed partial class LocalizationComponentInspector
    {
        /// <summary>
        /// 读取增量文件中的翻译键值对并存储到字典中
        /// 匹配文件格式：LocalPart_xxxx_{keyFileName}.lua.txt 与 Localization{keyFileName}_*Increase.lua.txt
        /// </summary>
        /// <param name="res">存储解析后的键值对结果字典</param>
        /// <param name="searchPattern">文件搜索匹配规则</param>
        private static void GetIncreaseFileContent(Dictionary<string, string> res, string searchPattern)
        {
            string pattern = @"^(?:[\w]+)\.([\w]+) = ""(.*)""$";
            string nilPattern = @"^(?:[\w]+)\.([\w]+)";
            
            string[] files = Directory.GetFiles(
                EditorPath.Localization.LuaScriptsFolderFullPath, 
                searchPattern, 
                SearchOption.AllDirectories
            );

            if (files == null || files.Length == 0)
                return;

            // 按文件名长度 + 字典序排序，保证版本顺序正确
            List<string> sortedFiles = new List<string>(files);
            sortedFiles.Sort((x, y) =>
            {
                if (x.Length != y.Length) 
                    return x.Length - y.Length;
                
                return string.Compare(x, y);
            });

            foreach (string filePath in sortedFiles)
            {
                FileInfo fileInfo = new FileInfo(filePath);
                using (StreamReader reader = fileInfo.OpenText())
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (!line.Contains(" = \"")) 
                            continue;

                        Match match = Regex.Match(line, pattern);
                        if (match.Success)
                        {
                            string key = match.Groups[1].Value;
                            string value = match.Groups[2].Value;
                            res[key] = value;
                        }
                        else
                        {
                            match = Regex.Match(line, nilPattern);
                            if (match.Success)
                            {
                                string key = match.Groups[1].Value;
                                res[key] = line.Contains("nil") ? "nil" : string.Empty;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 查找指定语言的增量更新文件
        /// </summary>
        /// <param name="keyFileName">语言标识文件名</param>
        /// <returns>匹配到的增量文件名称数组</returns>
        private static string[] FindIncrementalFile(string keyFileName)
        {
            string[] res = Directory.GetFiles(
                EditorPath.Localization.LuaScriptsFolderFullPath,
                $"Localization{keyFileName}_*Increase.lua.txt",
                SearchOption.AllDirectories
            );

            if (res == null || res.Length == 0)
                return res;

            for (int i = 0; i < res.Length; i++)
            {
                res[i] = Path.GetFileName(res[i]).Replace(".lua.txt", string.Empty);
            }

            return res;
        }

        /// <summary>
        /// 计算下一个增量文件的版本号
        /// </summary>
        /// <param name="keyFileName">语言标识文件名</param>
        /// <returns>下一个可用的增量版本号</returns>
        private static int GetNextIncreaseFileVersion(string keyFileName)
        {
            string pattern = @"_(\d+)Increase";
            string[] files = Directory.GetFiles(
                EditorPath.Localization.LuaScriptsFolderFullPath,
                $"Localization{keyFileName}_*Increase.lua.txt",
                SearchOption.AllDirectories
            );

            int maxVersion = 0;
            if (files != null && files.Length > 0)
            {
                foreach (string file in files)
                {
                    Match match = Regex.Match(file, pattern);
                    if (match.Success && int.TryParse(match.Groups[1].Value, out int version))
                    {
                        if (version > maxVersion)
                            maxVersion = version;
                    }
                }
            }

            return maxVersion + 1;
        }

        /// <summary>
        /// 获取下一个可用的增量文件夹编号（自动递增）
        /// </summary>
        /// <param name="exportLuaIncreasePath">增量文件导出根路径</param>
        /// <returns>下一个文件夹编号</returns>
        private static int GetNextIncreaseDirector(string exportLuaIncreasePath)
        {
            int folderIndex = 1;
            string currentDir = Path.Combine(exportLuaIncreasePath, folderIndex.ToString());
            
            while (Directory.Exists(currentDir))
            {
                folderIndex++;
                currentDir = Path.Combine(exportLuaIncreasePath, folderIndex.ToString());
            }

            return folderIndex;
        }

        /// <summary>
        /// 收集翻译键并检查是否重复
        /// </summary>
        /// <param name="key">待检查的翻译键</param>
        private static void CollectAndCheckKeyRepeat(string key)
        {
            if (m_AllKey.Contains(key))
                m_RepeatKey.Add(key);
            else
                m_AllKey.Add(key);
        }

        /// <summary>
        /// 收集并对比新旧翻译数据，筛选出新增、修改、删除的翻译项
        /// </summary>
        private static void FindIncrease(
            int columns, 
            List<string> fullPaths, 
            string[] keyList, 
            string tableDetailTidy, 
            string[] langInfo, 
            bool exportLuaIncreaseSingleVersion, 
            string exportLuaIncreasePath
        )
        {
            string targetDir = exportLuaIncreasePath;

            // 单版本模式：清空旧增量目录
            if (exportLuaIncreaseSingleVersion && Directory.Exists(targetDir))
            {
                Directory.Delete(targetDir, true);
                AssetDatabase.Refresh();
            }

            int folderVersion = GetNextIncreaseDirector(exportLuaIncreasePath);

            // 遍历所有语言列
            for (int col = 6; col < columns; col++)
            {
                if (string.IsNullOrEmpty(keyList[col]))
                    continue;

                // 1. 读取最新 Excel 数据（新数据）
                Dictionary<string, DataRow> newDataDict = new Dictionary<string, DataRow>();
                foreach (string excelPath in fullPaths)
                {
                    DataSet excelData = TableExportEditorUtility.GetExcelData(excelPath);
                    int rowCount = excelData.Tables[0].Rows.Count;

                    for (int row = 4; row < rowCount; row++)
                    {
                        object keyObj = excelData.Tables[0].Rows[row][1];
                        if (keyObj == null) 
                            continue;

                        string key = keyObj.ToString();
                        newDataDict[key] = excelData.Tables[0].Rows[row];

                        // 仅在第一语言列检查 Key 重复
                        if (col == 6)
                            CollectAndCheckKeyRepeat(key);
                    }
                }

                // 2. 读取已导出的旧数据（历史 + 增量）
                Dictionary<string, string> oldDataDict = new Dictionary<string, string>();
                foreach (string excelPath in fullPaths)
                {
                    string fileName = Path.GetFileNameWithoutExtension(excelPath);
                    GetIncreaseFileContent(oldDataDict, $"LocalPart_{fileName}_{keyList[col]}.lua.txt");
                }
                GetIncreaseFileContent(oldDataDict, $"Localization{keyList[col]}_*Increase.lua.txt");

                // 3. 对比：新增 / 修改
                Dictionary<string, DataRow> addOrChangeDict = new Dictionary<string, DataRow>();
                foreach (var pair in newDataDict)
                {
                    if (oldDataDict.TryGetValue(pair.Key, out string oldValue))
                    {
                        // 内容不一致 = 修改
                        if (oldValue != pair.Value[col].ToString())
                            addOrChangeDict[pair.Key] = pair.Value;
                    }
                    else
                    {
                        // 不存在 = 新增
                        addOrChangeDict[pair.Key] = pair.Value;
                    }
                }

                // 4. 对比：删除（Excel 中已不存在的 Key）
                List<string> removeKeyList = new List<string>();
                foreach (var pair in oldDataDict)
                {
                    if (!newDataDict.ContainsKey(pair.Key) && pair.Value != "nil")
                    {
                        if (!removeKeyList.Contains(pair.Key))
                            removeKeyList.Add(pair.Key);
                    }
                }

                // 5. 有变化则生成增量文件
                if (addOrChangeDict.Count > 0 || removeKeyList.Count > 0)
                {
                    MakeIncreaseFile(
                        addOrChangeDict, 
                        removeKeyList, 
                        keyList[col], 
                        tableDetailTidy, 
                        langInfo, 
                        col, 
                        folderVersion, 
                        exportLuaIncreasePath
                    );
                }
            }
        }

        /// <summary>
        /// 根据对比结果生成Lua格式的增量更新文件
        /// </summary>
        private static void MakeIncreaseFile(
            Dictionary<string, DataRow> addOrChangeDict,
            List<string> removeKeyList,
            string keyFileName,
            string tableDetailTidy,
            string[] langInfo,
            int col,
            int versionDir,
            string exportLuaIncreasePath
        )
        {
            int fileVersion = GetNextIncreaseFileVersion(keyFileName);
            string saveFolder = Path.Combine(exportLuaIncreasePath, versionDir.ToString());

            if (!Directory.Exists(saveFolder))
                Directory.CreateDirectory(saveFolder);

            string fileName = $"Localization{keyFileName}_{fileVersion}Increase.lua.txt";
            string savePath = Path.Combine(saveFolder, fileName);

            StringBuilder sb = new StringBuilder();
            MakeLuaFileTitle(sb, fileName, null, $"{tableDetailTidy}: {langInfo[col]}");

            string lowerLangName = keyFileName.ToLower();
            sb.AppendLine($"---@type Localizations.{keyFileName} @{tableDetailTidy}: {langInfo[col]}");
            sb.AppendLine($"local {lowerLangName} = Localizations.{keyFileName}");
            sb.AppendLine();

            // 写入新增/修改
            foreach (var pair in addOrChangeDict)
            {
                DataRow row = pair.Value;
                sb.AppendLine($"---@field {row[1]} string @{row[2]}");
                sb.AppendLine($"{lowerLangName}.{row[1]} = \"{row[col]}\"");
                sb.AppendLine();
            }

            // 写入删除（置空 nil）
            foreach (string key in removeKeyList)
            {
                sb.AppendLine($"{lowerLangName}.{key} = nil");
                sb.AppendLine();
            }

            sb.AppendLine($"---@type Localizations.{keyFileName} @{tableDetailTidy}: {langInfo[col]}");
            sb.AppendLine($"Localizations.{keyFileName} = {lowerLangName}");
            sb.AppendLine();

            // 保存 UTF8 无 BOM
            File.WriteAllText(savePath, sb.ToString(), new UTF8Encoding(false));
        }
    }
}