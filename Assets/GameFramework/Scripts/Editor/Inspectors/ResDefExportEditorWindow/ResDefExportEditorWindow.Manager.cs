using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using Honor.Runtime;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;
using Path = System.IO.Path;

namespace Honor.Editor
{
    public partial class ResDefExportEditorWindow : BaseEditorWindow<ResDefExportEditorWindow>
    {
        /// <summary>
        /// 显示通知
        /// </summary>
        /// <param name="notification"></param>
        public void ShowNotification(string notification)
        {
            ShowNotification(new GUIContent(notification));
        }

        /// <summary>
        /// 输出lua文件
        /// </summary>
        public static void WriteOutputGDefsFile(List<List<ResDefItem>> allResultDetailInfo)
        {
            // 清理历史Table脚本
            string[] oldFilesPaths =Directory.GetFiles(Application.dataPath, "LoadResDefs*.lua.txt", SearchOption.AllDirectories);
            foreach (var path in oldFilesPaths)
            {
                File.Delete(path);
            }

            if (string.IsNullOrEmpty(ResDefInfos.LuaExportFolderPath))
            {
                ResDefInfos.LuaExportFolderPath = Runtime.GamePathUtils.Editor.ResDef.LuaFolderPath;
            }

            if (!Directory.Exists(ResDefInfos.LuaExportFolderPath))
            {
                Directory.CreateDirectory(ResDefInfos.LuaExportFolderPath);
            }

            StringBuilder stringBuilder = new StringBuilder();
            var sheetString = new List<string>();
            var resDefItemIndex = 0;
            var sheetCount = 0;
            allResultDetailInfo.ForEach(detailInfoList =>
            {
                detailInfoList.ForEach(detailInfo =>
                {
                    if (resDefItemIndex == 0)
                    {
                        stringBuilder
                            .AppendLine(
                                "--=====================================================================================================")
                            .AppendLine("-- (c) copyright 2026 - 2030, GDResources")
                            .AppendLine("-- All Rights Reserved.")
                            .AppendLine(
                                "-- ----------------------------------------------------------------------------------------------------")
                            .AppendLine($"-- filename:  LoadResDefs_{sheetCount + 1}.lua")
                            .AppendLine($"-- descrip:   资源全局定义")
                            .AppendLine("-- notices:   该文件自动生成，请不要手动修改！")
                            .AppendLine(
                                "--=====================================================================================================")
                            .AppendLine("");
                    }

                    stringBuilder.AppendLine(AorTxt.Format("---@field {0} LOAD_RES_DEF_ITEM @文件 {1}", detailInfo.AliasName,
                        detailInfo.AssetName));
                    stringBuilder.AppendLine(AorTxt.Format(
                        "LoadRes[\"{0}\"] = {{ Name = \"{1}\", TypeName = \"{2}\", ABPath = \"{3}\", AssetName = \"{4}\" }}",
                        detailInfo.AliasName, detailInfo.AliasName, detailInfo.ResType, detailInfo.ABPath,
                        detailInfo.AssetName));
                    stringBuilder.AppendLine("");
                    resDefItemIndex++;
                    if (resDefItemIndex >= m_OneSheetMaxCount)
                    {
                        string luaExportFilePath =$"{ResDefInfos.LuaExportFolderPath}/LoadResDefs_{sheetCount + 1}.lua.txt";
                        File.WriteAllText(luaExportFilePath, stringBuilder.ToString(), new UTF8Encoding(false));
                        stringBuilder.Clear();
                        resDefItemIndex = 0;
                        sheetCount++;
                    }
                });
            });

            if (stringBuilder.ToString() != string.Empty)
            {
                File.WriteAllText($"{ResDefInfos.LuaExportFolderPath}/LoadResDefs_{sheetCount + 1}.lua.txt",
                    stringBuilder.ToString(), new UTF8Encoding(false));
            }
            else
            {
                sheetCount--;
            }

            stringBuilder.Clear();
            stringBuilder
                .AppendLine(
                    "--=====================================================================================================")
                .AppendLine("-- (c) copyright 2026 - 2030, GDResources")
                .AppendLine("-- All Rights Reserved.")
                .AppendLine(
                    "-- ----------------------------------------------------------------------------------------------------")
                .AppendLine(AorTxt.Format($"-- filename:  LoadResDefs.lua"))
                .AppendLine(AorTxt.Format($"-- descrip:   资源全局定义"))
                .AppendLine("-- notices:   该文件自动生成，请不要手动修改！")
                .AppendLine(
                    "--=====================================================================================================")
                .AppendLine($"---@class LOAD_RES_DEF_ITEM @资源条目定义")
                .AppendLine($"---@field Name string @资源名称")
                .AppendLine($"---@field TypeName string @类型名称")
                .AppendLine($"---@field ABPath string @ab路径")
                .AppendLine($"---@field AssetName string @Asset名称")
                .AppendLine($"")
                .AppendLine($"---@class LoadRES @资源全局定义")
                .AppendLine($"---@type table<string, LOAD_RES_DEF_ITEM> @数据格式")
                .AppendLine("")
                .AppendLine("LoadRes = {}");
            for (int idx = 0; idx <= sheetCount; idx++)
            {
                stringBuilder.AppendLine($"require('LoadResDefs_{idx + 1}')");
            }

            string luaExportFilePath = $"{ResDefInfos.LuaExportFolderPath}/LoadResDefs.lua.txt";
            File.WriteAllText(luaExportFilePath, stringBuilder.ToString(), new UTF8Encoding(false));
            Log.Debug($"[Editor] 导出 {luaExportFilePath} 完成。");
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 检查某个文件的ABPath
        /// </summary>
        /// <param name="fullPath"></param>
        /// <param name="fileABPath"></param>
        /// <returns></returns>
        public bool CheckFileABPath(string fullPath, out string fileABPath)
        {
            fileABPath = String.Empty;
            var assetBundleName = AssetDatabase.GetImplicitAssetBundleName(fullPath);
            if (s_ABConfigs.ContainsKey(assetBundleName))
            {
                fileABPath = s_ABConfigs[assetBundleName];
                return true;
            }

            return false;
        }

        /// <summary>
        /// 匹配首尾中间部分的字符串 
        /// </summary>
        /// <param name="mathString">需要匹配的字符串</param>
        /// <param name="startString">匹配开始字符串</param>
        /// <param name="endString">匹配结束字符串</param>
        /// <returns></returns>
        public string GetMatchString(string mathString, string startString, string endString)
        {
            string regx = string.Format("(?<=({0}))[.\\s\\S]*?(?=({1}))", startString, endString);
            if (string.IsNullOrWhiteSpace(mathString)) return string.Empty;
            bool isMatch = Regex.IsMatch(mathString, regx);
            if (!isMatch) return string.Empty;
            return Regex.Match(mathString, regx).Value.Trim();
        }


        /// <summary>
        /// 更新找到的文本数据信息
        /// </summary>
        public bool UpdateFindAllFileData(Object findTargetAsset)
        {
            string fullPath = AssetDatabase.GetAssetPath(findTargetAsset);
            bool isHaveAbPath = false;
            string suffix = GameDefinitions.AssetSuffix[(GameDefinitions.AssetType)m_SelectResType];
            if (Directory.Exists(fullPath))
            {
                var matchFileArray = Directory.GetFiles(fullPath, "*" + suffix, System.IO.SearchOption.AllDirectories);
                foreach (var fileFullPath in matchFileArray)
                {
                    if (CheckFileABPath(fileFullPath, out string fileABPath))
                    {
                        m_FindFileFullPathList.Add(fileFullPath, FileUseState.LoadSuccess);
                        isHaveAbPath = true;
                    }
                }
            }
            else
            {
                var fileName = System.IO.Path.GetFileName(fullPath);
                if (fileName.Contains(suffix))
                {
                    if (CheckFileABPath(fullPath, out string fileABPath))
                    {
                        m_FindFileFullPathList.Add(fullPath, FileUseState.LoadSuccess);
                        isHaveAbPath = true;
                    }
                    else
                    {
                        m_FindFileFullPathList.Add(fullPath, FileUseState.LoadSuccess);
                        isHaveAbPath = false;
                    }
                }
            }

            if (m_FindFileFullPathList.Count == 0)
            {
                ShowNotification("没有找到匹配的文件");
            }

            return isHaveAbPath;
        }

        /// <summary>
        /// 更新拖拽选择的多文件
        /// </summary>
        /// <param name="objects"></param>
        public void UpdateAllSelectedFileData(Object[] objects)
        {
            m_FindFileFullPathList.Clear();
            foreach (var obj in objects)
            {
                SetFindTargetAsset(obj, false);
            }
        }

        /// <summary>
        /// 删除某个资源信息
        /// </summary>
        /// <param name="startID"></param>
        /// <param name="endID"></param>
        public void DeleteResInfo(int startID, int endID)
        {
            m_TempResDefItems.RemoveAll(x => (x.ID >= startID && x.ID <= endID));
            var removeCount = ResDefInfos.ConvertData.RemoveAll(item => item.ID >= startID && item.ID <= endID);
            Log.Debug($"[Editor] ID 从 {startID} 到 {endID} 共删除: {removeCount} 个数据。");
            ShowNotification($"ID 从 {startID} 到 {endID} 共删除: {removeCount} 个数据。");
            ResDefInfos.WriteJson();
            UpdateResultInfo();
        }

        /// <summary>
        /// 删除所有失效链接
        /// </summary>
        public void DeleteAllInvalidResInfo()
        {
            if (m_ResultInvalidPath != null)
            {
                m_ResultInvalidPath.Values.ToList().ForEach((dicInvalid) =>
                {
                    dicInvalid.Keys.ToList().ForEach(itemId =>
                    {
                        var finResDefItem = m_TempResDefItems.Find(item => item.ID == itemId);
                        if (finResDefItem != null)
                        {
                            m_TempResDefItems.Remove(finResDefItem);
                        }
                        else
                        {
                            ResDefInfos.ConvertData.RemoveAll(item => item.ID == itemId);
                        }
                    });
                    dicInvalid.Clear();
                });

                ResDefInfos.WriteJson();
                UpdateResultInfo();
            }
        }

        /// <summary>
        /// 得到当前面板上和存档里面是否含有重复的别名
        /// </summary>
        public Dictionary<string, bool> GetAllSameResInfo()
        {
            var allCheckResInfo = m_TempResDefItems.Concat(ResDefInfos.ConvertData).ToList();
            return allCheckResInfo.GroupBy(m => m.AliasName).Where(m => m.Count() > 1).Select(m => m.Key)
                .ToDictionary(key => key, value => true);
        }

        /// <summary>
        /// 检测某个别名是否包含
        /// </summary>
        /// <param name="aliasName"></param>
        /// <returns></returns>
        public bool IsHaveSameAliasName(string aliasName)
        {
            foreach (var resDefItem in m_TempResDefItems)
            {
                if (resDefItem.AliasName == aliasName)
                {
                    return true;
                }
            }

            foreach (var resDefItem in ResDefInfos.ConvertData)
            {
                if (resDefItem.AliasName == aliasName)
                {
                    return true;
                }
            }

            return false;
        }


        /// <summary>
        /// 得到结果的面板上所有的丢失路径
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, Dictionary<int, Boolean>> GetResultInvalidPathInfo()
        {
            var resultInvalidPath = new Dictionary<string, Dictionary<int, Boolean>>();
            resultInvalidPath.Add(m_SearchTag, new Dictionary<int, bool>());
            resultInvalidPath.Add(m_ErrorTag, new Dictionary<int, bool>());
            foreach (var resDataValue in ResDefInfos.ConvertData)
            {
                if (resultInvalidPath.ContainsKey(resDataValue.ResType) == false)
                {
                    resultInvalidPath.Add(resDataValue.ResType, new Dictionary<int, bool>());
                }

                if (!string.IsNullOrEmpty(resDataValue.AliasName))
                {
                    var arr = resDataValue.AliasName.Split("_", 2);
                    if (arr != null && arr.Length == 2 && !arr[1].Equals(resDataValue.AssetName) &&
                        resultInvalidPath[resDataValue.ResType].ContainsKey(resDataValue.ID))
                    {
                        // 别名不为空，且别名和资源名字不一致
                        resultInvalidPath[resDataValue.ResType].Add(resDataValue.ID, true);
                        resultInvalidPath[m_SearchTag].Add(resDataValue.ID, true);
                        resultInvalidPath[m_ErrorTag].Add(resDataValue.ID, true);
                    }
                }

                if (Directory.Exists(resDataValue.ABPath)) // 如果abPath是目录
                {
                    var files = Directory.GetFiles(resDataValue.ABPath,
                        resDataValue.AssetName + GetAssetsSuffixByType(resDataValue.ResType),
                        SearchOption.AllDirectories);
                    if (files.Length == 0)
                    {
                        resultInvalidPath[resDataValue.ResType].Add(resDataValue.ID, true);
                        resultInvalidPath[m_SearchTag].Add(resDataValue.ID, true);
                        resultInvalidPath[m_ErrorTag].Add(resDataValue.ID, true);
                    }
                    else
                    {
                        var outAbPath = string.Empty;
                        CheckFileABPath(files[0], out outAbPath);
                        if (resDataValue.ABPath != outAbPath) // 文件虽然存在，但是ABPath目录发生了变化
                        {
                            resultInvalidPath[resDataValue.ResType].Add(resDataValue.ID, true);
                            resultInvalidPath[m_SearchTag].Add(resDataValue.ID, true);
                            resultInvalidPath[m_ErrorTag].Add(resDataValue.ID, true);
                        }
                    }
                }
                else // 不是目录，则是某个文件
                {
                    if (!(File.Exists(AorTxt.Format("{0}{1}", resDataValue.ABPath,
                            GetAssetsSuffixByType(resDataValue.ResType)))))
                    {
                        resultInvalidPath[resDataValue.ResType].Add(resDataValue.ID, true);
                        resultInvalidPath[m_SearchTag].Add(resDataValue.ID, true);
                        resultInvalidPath[m_ErrorTag].Add(resDataValue.ID, true);
                    }
                }
            }

            return resultInvalidPath;
        }

        /// <summary>
        /// 得到结果的面板上所有的存储信息
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, List<ResDefItem>> GetResultDetailInfo()
        {
            var resultDetailInfo = new Dictionary<string, List<ResDefItem>>();
            foreach (var resDataValue in ResDefInfos.ConvertData)
            {
                if (resultDetailInfo.ContainsKey(resDataValue.ResType) == false)
                {
                    resultDetailInfo.Add(resDataValue.ResType, new List<ResDefItem>());
                }

                resultDetailInfo[resDataValue.ResType].Add(resDataValue);
            }

            foreach (var keyValuePair in resultDetailInfo)
            {
                m_AllShowFoldout[keyValuePair.Key].PageCount = Mathf.CeilToInt(keyValuePair.Value.Count * 1.0f /
                                                                               m_AllShowFoldout[keyValuePair.Key]
                                                                                   .OnePageCount);
            }

            resultDetailInfo.Values.ToList().ForEach(list =>
            {
                list.Sort((m1, m2) => { return StrCmpLogicalW(m1.AssetName, m2.AssetName); });
            });

            return resultDetailInfo;
        }


        /// <summary>
        /// 获取某个类型资源的后缀
        /// </summary>
        /// <param name="assetsType"></param>
        /// <returns></returns>
        public string GetAssetsSuffixByType(string typeName)
        {
            string suffix = string.Empty;
            if (Enum.TryParse(typeof(GameDefinitions.AssetType), typeName, out object tmpAssetType))
            {
                suffix = GameDefinitions.AssetSuffix[(GameDefinitions.AssetType)tmpAssetType];
            }

            return suffix;
        }


        /// <summary>
        /// 收集ABConfig信息
        /// </summary>
        /// <summary>
        /// 收集ABConfig信息
        /// </summary>
        /// <summary>
        /// 收集ABConfig信息
        /// </summary>
        public static void LoadABConfigs()
        {
            s_ABConfigs.Clear();
            string filePathList = Runtime.GamePathUtils.Json.GetRootDirectoryFullPath() + "/ABConfigs.json";
            string content = File.ReadAllText(filePathList);
            if (string.IsNullOrEmpty(content))
            {
               return; 
            }
            JObject jObject = JObject.Parse(content);

            var abConfigs = new Dictionary<string, ABConfigInfo>();
            foreach (var itr in jObject)
            {
                JToken data = itr.Value;
                string path = data["Path"].ToString();

                // ========== 这里加安全判断！==========
                if (!abConfigs.ContainsKey(path))
                {
                    abConfigs.Add(path,new ABConfigInfo(int.Parse(data["ID"].ToString()), path,
                            int.Parse(data["PackageMeasureType"].ToString()), data["Rename"].ToString(),
                            data["GroupName"].ToString(), bool.Parse(data["IsIncreaserGroup"].ToString()),
                            bool.Parse(data["IsCommonIncreaserGroup"].ToString())));
                }
            }

            foreach (var config in abConfigs)
            {
                ABConfigInfo info = config.Value;

                // 将Path文件/目录单独作为一个AB进行打包
                if (info.PackageMeasureType == 0)
                {
                    if (info.IsPlatformManifest || File.Exists(info.Path) || Directory.Exists(info.Path))
                    {
                        AssetImporter importer = AssetImporter.GetAtPath(info.Path);
                        if (importer)
                        {
                            if (string.IsNullOrEmpty(info.Rename))
                            {
                                var abName = info.Path.Replace('/', '@').ToLower() + ".bundle";
                                if (!s_ABConfigs.ContainsKey(abName))
                                    s_ABConfigs.Add(abName, info.Path);
                            }
                        }
                    }
                }
                // 将Path目录下每个文件单独作为一个AB进行打包
                else if (info.PackageMeasureType == 1)
                {
                    if (Directory.Exists(info.Path))
                    {
                        string[] fullPaths = Directory.GetFiles(System.IO.Path
                            .Combine(Application.dataPath.Substring(0, Application.dataPath.Length - "Assets".Length),
                                info.Path).Replace('\\', '/'));
                        foreach (string fullPath in fullPaths)
                        {
                            if (!fullPath.EndsWith(".meta"))
                            {
                                string formatFullPath = fullPath.Replace('\\', '/');
                                string filePath = AorTxt.Format("Assets/{0}",
                                    formatFullPath.Substring(Application.dataPath.Length + 1,
                                        formatFullPath.Length - (Application.dataPath.Length + 1)));
                                AssetImporter importer = AssetImporter.GetAtPath(filePath);
                                if (importer)
                                {
                                    int indexOfSuffixFlag = filePath.LastIndexOf('.');
                                    indexOfSuffixFlag = indexOfSuffixFlag < 0 ? filePath.Length : indexOfSuffixFlag;
                                    var assetBundleName = AorTxt.Format("{0}.bundle",
                                        filePath.Substring(0, indexOfSuffixFlag).Replace('/', '@').ToLower());
                                    if (!s_ABConfigs.ContainsKey(assetBundleName))
                                        s_ABConfigs.Add(assetBundleName, filePath.Substring(0, indexOfSuffixFlag));
                                }
                            }
                        }
                    }
                }
                // 将Path目录下每个第一级文件夹单独作为一个AB进行打包
                else if (info.PackageMeasureType == 2)
                {
                    if (Directory.Exists(info.Path))
                    {
                        string[] fullPaths = Directory.GetDirectories(System.IO.Path
                            .Combine(Application.dataPath.Substring(0, Application.dataPath.Length - "Assets".Length),
                                info.Path).Replace('\\', '/'));
                        foreach (string fullPath in fullPaths)
                        {
                            if (Directory.Exists(fullPath))
                            {
                                string formatFullPath = fullPath.Replace('\\', '/');
                                string directoryPath = AorTxt.Format("Assets/{0}",
                                    formatFullPath.Substring(Application.dataPath.Length + 1,
                                        formatFullPath.Length - (Application.dataPath.Length + 1)));
                                AssetImporter importer = AssetImporter.GetAtPath(directoryPath);
                                if (importer)
                                {
                                    int indexOfSuffixFlag = directoryPath.LastIndexOf('.');
                                    indexOfSuffixFlag =
                                        indexOfSuffixFlag < 0 ? directoryPath.Length : indexOfSuffixFlag;
                                    var assetBundleName = AorTxt.Format("{0}.bundle",
                                        directoryPath.Substring(0, indexOfSuffixFlag).Replace('/', '@').ToLower());
                                    if (!s_ABConfigs.ContainsKey(assetBundleName))
                                        s_ABConfigs.Add(assetBundleName, directoryPath.Substring(0, indexOfSuffixFlag));
                                }
                            }
                        }
                    }
                }
                // 将Path目录下每个第一级文件与文件夹单独作为一个AB进行打包
                else if (info.PackageMeasureType == 3)
                {
                    if (Directory.Exists(info.Path))
                    {
                        // 先处理文件
                        string[] fullPaths = Directory.GetFiles(System.IO.Path
                            .Combine(Application.dataPath.Substring(0, Application.dataPath.Length - "Assets".Length),
                                info.Path).Replace('\\', '/'));
                        foreach (string fullPath in fullPaths)
                        {
                            if (!fullPath.EndsWith(".meta"))
                            {
                                string formatFullPath = fullPath.Replace('\\', '/');
                                string filePath = AorTxt.Format("Assets/{0}",
                                    formatFullPath.Substring(Application.dataPath.Length + 1,
                                        formatFullPath.Length - (Application.dataPath.Length + 1)));
                                AssetImporter importer = AssetImporter.GetAtPath(filePath);
                                if (importer)
                                {
                                    int indexOfSuffixFlag = filePath.LastIndexOf('.');
                                    indexOfSuffixFlag = indexOfSuffixFlag < 0 ? filePath.Length : indexOfSuffixFlag;
                                    var assetBundleName = AorTxt.Format("{0}.bundle",
                                        filePath.Substring(0, indexOfSuffixFlag).Replace('/', '@').ToLower());
                                    if (!s_ABConfigs.ContainsKey(assetBundleName))
                                        s_ABConfigs.Add(assetBundleName, filePath.Substring(0, indexOfSuffixFlag));
                                }
                            }
                        }

                        // 再处理文件夹
                        fullPaths = Directory.GetDirectories(System.IO.Path
                            .Combine(Application.dataPath.Substring(0, Application.dataPath.Length - "Assets".Length),
                                info.Path).Replace('\\', '/'));
                        foreach (string fullPath in fullPaths)
                        {
                            if (Directory.Exists(fullPath))
                            {
                                string formatFullPath = fullPath.Replace('\\', '/');
                                string directoryPath = AorTxt.Format("Assets/{0}",
                                    formatFullPath.Substring(Application.dataPath.Length + 1,
                                        formatFullPath.Length - (Application.dataPath.Length + 1)));
                                AssetImporter importer = AssetImporter.GetAtPath(directoryPath);
                                if (importer)
                                {
                                    int indexOfSuffixFlag = directoryPath.LastIndexOf('.');
                                    indexOfSuffixFlag =
                                        indexOfSuffixFlag < 0 ? directoryPath.Length : indexOfSuffixFlag;
                                    var assetBundleName = AorTxt.Format("{0}.bundle",
                                        directoryPath.Substring(0, indexOfSuffixFlag).Replace('/', '@').ToLower());
                                    if (!s_ABConfigs.ContainsKey(assetBundleName))
                                        s_ABConfigs.Add(assetBundleName, directoryPath.Substring(0, indexOfSuffixFlag));
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 根据ABPath 找到有效的搜索路径
        /// </summary>
        /// <param name="abPath"></param>
        /// <returns></returns>
        public string GetAbPathFullDirPath(string abPath)
        {
            var searchDirPath = Application.dataPath + abPath.Substring(6);
            while (searchDirPath != String.Empty)
            {
                if (Directory.Exists(searchDirPath))
                {
                    return searchDirPath;
                }
                else
                {
                    searchDirPath = System.IO.Path.GetDirectoryName(searchDirPath);
                }
            }

            return Application.dataPath;
        }

        //调用windos 的 DLL
        [System.Runtime.InteropServices.DllImport("Shlwapi.dll", CharSet = CharSet.Unicode)]
        private static extern int StrCmpLogicalW(string param1, string param2);


        /// <summary>
        /// 加载所需图片
        /// </summary>
        private void LoadDefaultTextures()
        {
            var fileNames = new List<string>() { "DefaultLeft", "DefaultRight", "DefaultDown", "DefaultTop" };
            foreach (var fileName in fileNames)
            {
                var texture2D =
                    AssetDatabase.LoadAssetAtPath<Texture2D>($"Assets/Res/Textures/PicsForEditor/{fileName}.png");
                m_AllDefaultTextures.Add(texture2D);
            }
        }

        /// <summary>
        /// 更新结果信息
        /// </summary>
        private void UpdateResultInfo()
        {
            m_ResultInvalidPath = GetResultInvalidPathInfo();
            m_ResultDetailInfo = GetResultDetailInfo();
            m_ResultSameResInfo = GetAllSameResInfo();
            if (m_OnlyShowError)
            {
                UpdateErrorResDefItemInfo();
            }

            if (m_SearchResName != String.Empty)
            {
                UpdateSearchResDefItemInfo(m_SearchResName);
            }
        }


        /// <summary>
        /// 刷新所有失效链接
        /// </summary>
        private void AutoRefreshAllInvalidResInfo()
        {
            var autoRefreshFileCount = 0;
            if (m_ResultInvalidPath.ContainsKey(m_ErrorTag))
            {
                m_ResultInvalidPath[m_ErrorTag].Keys.ToList().ForEach(invalidId =>
                {
                    var findIndex = ResDefInfos.ConvertData.FindIndex(item => item.ID == invalidId);
                    if (findIndex >= 0)
                    {
                        var resDefItem = ResDefInfos.ConvertData[findIndex];
                        if (string.IsNullOrEmpty(resDefItem.AssetGUID) == false)
                        {
                            var fullPath = AssetDatabase.GUIDToAssetPath(resDefItem.AssetGUID);
                            if (fullPath != String.Empty) // 如果发现有fullPath，则文件只是修改了目录
                            {
                                var fileName = System.IO.Path.GetFileName(fullPath);
                                var fileExtensionName = System.IO.Path.GetFileNameWithoutExtension(fullPath);
                                string suffix = GameDefinitions.AssetSuffix[
                                    (GameDefinitions.AssetType)Enum.Parse(typeof(GameDefinitions.AssetType),
                                        resDefItem.ResType)];
                                if (fileName.Contains(suffix))
                                {
                                    if (CheckFileABPath(fullPath, out string fileABPath))
                                    {
                                        resDefItem.ABPath = fileABPath;
                                        resDefItem.AssetName = fileExtensionName;
                                        Log.Debug($"[Editor] 矫正了文件： {fileExtensionName} 。");
                                        autoRefreshFileCount++;
                                    }
                                }
                            }
                        }
                    }
                });
            }

            if (autoRefreshFileCount > 0)
            {
                ShowNotification($"自动校准成功： 共校准 {autoRefreshFileCount} 个文件");
                // 更新过后重新刷新失效链接
                m_ResultInvalidPath = GetResultInvalidPathInfo();
            }
        }

        /// <summary>
        /// 自动矫正失效链接
        /// </summary>
        public bool AutoRectifyInvalidResInfo(string fileName, string fileType, string abPath, ResDefItem resDefItem,
            bool autoSaveJson)
        {
            // ====================== 安全判断：路径为空直接返回 ======================
            if (string.IsNullOrEmpty(abPath))
            {
                Debug.LogWarning("自动矫正失败：abPath 路径为空");
                return false;
            }
            var arr = resDefItem.AliasName.Split("_", 2);
            if (arr != null && arr.Length == 2 && !arr[1].Equals(fileName))
            {
                //别名与文件名不一致,用别名修正文件名
                fileName = arr[1];
            }

            var fileTypeName = GetAssetsSuffixByType(fileType);
            // 从abPath中路径中查找文件
            var guids = AssetDatabase.FindAssets(fileName, new string[] { abPath });
            if (guids == null || guids.Length == 0)
            {
                // 从全局查找
                guids = AssetDatabase.FindAssets(fileName);
            }

            foreach (var guid in guids)
            {
                var fullPath = AssetDatabase.GUIDToAssetPath(guid);
                Debug.Log("find name: - " + fullPath);
                var extensionStr = Path.GetExtension(fullPath);
                var tempFileName = Path.GetFileNameWithoutExtension(fullPath);
                if (string.IsNullOrEmpty(extensionStr) || tempFileName.Equals(fileName) == false)
                {
                    continue;
                }

                if (extensionStr.Equals(fileTypeName))
                {
                    var newABPath = string.Empty;
                    if (CheckFileABPath(fullPath, out newABPath))
                    {
                        resDefItem.AssetName = Path.GetFileNameWithoutExtension(fullPath);
                        resDefItem.ABPath = newABPath;
                        if (autoSaveJson)
                        {
                            ResDefInfos.WriteJson();
                        }

                        //ShowNotification("信息矫正成功，文件名字，AB路径都已刷新");
                        //m_ResultInvalidPath = GetResultInvalidPathInfo();
                        return true;
                    }
                    else
                    {
                        // ShowNotification("当前文件没有找到AB配置信息，请检查");
                        return false;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 更新报错相关信息
        /// </summary>
        private void UpdateErrorResDefItemInfo()
        {
            m_ErrorResDefItemInfo.Clear();
            foreach (var resDataValue in ResDefInfos.ConvertData)
            {
                if (m_ResultInvalidPath[m_ErrorTag].ContainsKey(resDataValue.ID))
                {
                    m_ErrorResDefItemInfo.Add(resDataValue);
                }
            }

            if (m_ErrorResDefItemInfo.Count > 0)
            {
                m_AllShowFoldout[m_ErrorTag].CurPageIndex = 1;
                m_AllShowFoldout[m_ErrorTag].PageCount = Mathf.CeilToInt(m_ErrorResDefItemInfo.Count * 1.0f /
                                                                         m_AllShowFoldout[m_ErrorTag].OnePageCount);
            }
            else
            {
                m_AllShowFoldout[m_ErrorTag].CurPageIndex = 1;
                m_AllShowFoldout[m_ErrorTag].PageCount = 0;
            }
        }


        /// <summary>
        /// 更新搜索的列表信息
        /// </summary>
        /// <param name="searchName"></param>
        private void UpdateSearchResDefItemInfo(string searchName)
        {
            m_SearchResDefItemInfo.Clear();
            if (m_OnlyShowError)
            {
                if (m_ErrorResDefItemInfo.Count > 0)
                {
                    m_ErrorResDefItemInfo.ForEach(resDefItem =>
                    {
                        if (resDefItem.AssetName.ToLower().Contains(searchName))
                        {
                            m_SearchResDefItemInfo.Add(resDefItem);
                        }
                    });
                }
            }
            else
            {
                m_ResultDetailInfo.Values.ToList().ForEach(listResDefItem => listResDefItem.ForEach(resDefItem =>
                {
                    if (resDefItem.AssetName.ToLower().Contains(searchName))
                    {
                        m_SearchResDefItemInfo.Add(resDefItem);
                    }
                }));
            }

            if (m_SearchResDefItemInfo.Count > 0)
            {
                m_AllShowFoldout[m_SearchTag].CurPageIndex = 1;
                m_AllShowFoldout[m_SearchTag].PageCount = Mathf.CeilToInt(m_SearchResDefItemInfo.Count * 1.0f /
                                                                          m_AllShowFoldout[m_SearchTag].OnePageCount);
            }
            else
            {
                m_AllShowFoldout[m_SearchTag].CurPageIndex = 1;
                m_AllShowFoldout[m_SearchTag].PageCount = 0;
            }
        }

        /// <summary>
        /// 设置查找到 TargetAsset
        /// </summary>
        /// <param name="findTargetAsset"></param>
        /// <param name="isClear"></param>
        private void SetFindTargetAsset(Object findTargetAsset, bool isClear = true)
        {
            if (isClear)
            {
                m_FindFileFullPathList.Clear();
            }

            if (findTargetAsset != null)
            {
                if (UpdateFindAllFileData(findTargetAsset) == false) //检查是否所有的文件里面包含abPath
                {
                    ShowNotification("当前目录或者文件为空，或没有发现AB配置信息，请检查。");
                }
            }
            else
            {
                ShowNotification("当前目录或者文件为空，或没有发现AB配置信息，请检查。");
            }
        }
        
        
        /// <summary>
        /// 从ABConfig Excel中导出json文件，默认导出第一个sheet
        /// </summary>
        /// <param name="openExcelNamePre"></param>
        /// <returns></returns>
        public static bool ExportExcelToJsonFromABConfig(string openExcelNamePre)
        {
            // 要写入的json文件路径
            string strSubJsonDirectoryPath = Runtime.GamePathUtils.Json.GetRootDirectoryFullPath();
            string strFilePathList = $"{strSubJsonDirectoryPath}/{openExcelNamePre}.json";
            if(!Directory.Exists(strSubJsonDirectoryPath))
            {
                Directory.CreateDirectory(strSubJsonDirectoryPath);
            }
            return TableExportEditorUtility.ExportExcelToJson(Runtime.GamePathUtils.AB.GetExcelFileFullPath(), strFilePathList);
        }

        
    }
}