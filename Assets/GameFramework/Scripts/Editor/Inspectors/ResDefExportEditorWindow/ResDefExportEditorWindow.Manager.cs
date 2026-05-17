/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Editor
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  ResDefExportEditorWindow.cs
 * author:    云毅
 * created:   2026
 * descrip:   资源配置导出工具 - 核心逻辑实现（分部类）
 ***************************************************************/

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
        #region 通用提示
        /// <summary>
        /// 显示通知
        /// </summary>
        public void ShowNotification(string notification)
        {
            ShowNotification(new GUIContent(notification));
        }
        #endregion

        #region Lua 导出
        /// <summary>
        /// 输出 Lua 资源定义文件
        /// </summary>
        public static void WriteOutputGDefsFile(List<List<ResDefItem>> allResultDetailInfo)
        {
            // 清理旧文件
            string[] oldFiles = Directory.GetFiles(Application.dataPath, "LoadResDefs*.lua.txt", SearchOption.AllDirectories);
            foreach (string path in oldFiles)
            {
                File.Delete(path);
            }

            if (string.IsNullOrEmpty(ResDefInfos.LuaExportFolderPath))
            {
                ResDefInfos.LuaExportFolderPath = GamePathUtils.Editor.ResDef.LuaFolderPath;
            }

            if (!Directory.Exists(ResDefInfos.LuaExportFolderPath))
            {
                Directory.CreateDirectory(ResDefInfos.LuaExportFolderPath);
            }

            StringBuilder sb = new StringBuilder();
            int resIndex = 0;
            int sheet = 0;

            allResultDetailInfo.ForEach(detailList =>
            {
                detailList.ForEach(item =>
                {
                    if (resIndex == 0)
                    {
                        sb.AppendLine("--=====================================================================================================")
                          .AppendLine("-- (c) copyright 2026 - 2030, GDResources")
                          .AppendLine("-- All Rights Reserved.")
                          .AppendLine("-- ----------------------------------------------------------------------------------------------------")
                          .AppendLine($"-- filename:  LoadResDefs_{sheet + 1}.lua")
                          .AppendLine($"-- descrip:   资源全局定义")
                          .AppendLine("-- notices:   该文件自动生成，请不要手动修改！")
                          .AppendLine("--=====================================================================================================")
                          .AppendLine();
                    }

                    sb.AppendLine(AorTxt.Format("---@field {0} LOAD_RES_DEF_ITEM @文件 {1}", item.AliasName, item.AssetName));
                    sb.AppendLine(AorTxt.Format("LoadRes[\"{0}\"] = {{ Name = \"{1}\", TypeName = \"{2}\", ABPath = \"{3}\", AssetName = \"{4}\" }}",
                        item.AliasName, item.AliasName, item.ResType, item.ABPath, item.AssetName));
                    sb.AppendLine();

                    resIndex++;
                    if (resIndex >= m_OneSheetMaxCount)
                    {
                        string path = $"{ResDefInfos.LuaExportFolderPath}/LoadResDefs_{sheet + 1}.lua.txt";
                        File.WriteAllText(path, sb.ToString(), new UTF8Encoding(false));
                        sb.Clear();
                        resIndex = 0;
                        sheet++;
                    }
                });
            });

            if (!string.IsNullOrEmpty(sb.ToString()))
            {
                string path = $"{ResDefInfos.LuaExportFolderPath}/LoadResDefs_{sheet + 1}.lua.txt";
                File.WriteAllText(path, sb.ToString(), new UTF8Encoding(false));
            }
            else
            {
                sheet--;
            }

            // 生成主入口
            sb.Clear();
            sb.AppendLine("--=====================================================================================================")
              .AppendLine("-- (c) copyright 2026 - 2030, GDResources")
              .AppendLine("-- All Rights Reserved.")
              .AppendLine("-- ----------------------------------------------------------------------------------------------------")
              .AppendLine("-- filename:  LoadResDefs.lua")
              .AppendLine("-- descrip:   资源全局定义")
              .AppendLine("-- notices:   该文件自动生成，请不要手动修改！")
              .AppendLine("--=====================================================================================================")
              .AppendLine("---@class LOAD_RES_DEF_ITEM @资源条目定义")
              .AppendLine("---@field Name string @资源名称")
              .AppendLine("---@field TypeName string @类型名称")
              .AppendLine("---@field ABPath string @AB包路径")
              .AppendLine("---@field AssetName string @资源名称")
              .AppendLine()
              .AppendLine("---@class LoadRES @资源全局定义")
              .AppendLine("---@type table<string, LOAD_RES_DEF_ITEM>")
              .AppendLine()
              .AppendLine("LoadRes = {}");

            for (int i = 0; i <= sheet; i++)
            {
                sb.AppendLine($"require('LoadResDefs_{i + 1}')");
            }

            string mainPath = $"{ResDefInfos.LuaExportFolderPath}/LoadResDefs.lua.txt";
            File.WriteAllText(mainPath, sb.ToString(), new UTF8Encoding(false));
            Log.Debug($"[Editor] 导出完成：{mainPath}");
            AssetDatabase.Refresh();
        }
        #endregion

        #region AB 路径检查
        /// <summary>
        /// 检查文件对应的 AB 路径
        /// </summary>
        public bool CheckFileABPath(string fullPath, out string fileABPath)
        {
            fileABPath = string.Empty;
            string abName = AssetDatabase.GetImplicitAssetBundleName(fullPath);
            if (s_ABConfigs.ContainsKey(abName))
            {
                fileABPath = s_ABConfigs[abName];
                return true;
            }
            return false;
        }
        #endregion

        #region 字符串正则匹配
        /// <summary>
        /// 截取开始与结束字符串之间的内容
        /// </summary>
        public string GetMatchString(string mathString, string startString, string endString)
        {
            string regx = string.Format("(?<=({0}))[.\\s\\S]*?(?=({1}))", startString, endString);
            if (string.IsNullOrWhiteSpace(mathString)) 
                return string.Empty;

            return Regex.IsMatch(mathString, regx) 
                ? Regex.Match(mathString, regx).Value.Trim() 
                : string.Empty;
        }
        #endregion

        #region 文件查找与收集
        /// <summary>
        /// 更新查找的资源文件
        /// </summary>
        public bool UpdateFindAllFileData(Object findTargetAsset)
        {
            string fullPath = AssetDatabase.GetAssetPath(findTargetAsset);
            bool isHaveAbPath = false;
            string suffix = GameDefinitions.AssetSuffix[(GameDefinitions.AssetType)m_SelectResType];

            if (Directory.Exists(fullPath))
            {
                string[] files = Directory.GetFiles(fullPath, "*" + suffix, SearchOption.AllDirectories);
                foreach (string filePath in files)
                {
                    if (CheckFileABPath(filePath, out string abPath))
                    {
                        m_FindFileFullPathList.Add(filePath, FileUseState.LoadSuccess);
                        isHaveAbPath = true;
                    }
                }
            }
            else
            {
                string fileName = Path.GetFileName(fullPath);
                if (fileName.Contains(suffix))
                {
                    if (CheckFileABPath(fullPath, out string abPath))
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
        /// 批量添加选择的对象
        /// </summary>
        public void UpdateAllSelectedFileData(Object[] objects)
        {
            m_FindFileFullPathList.Clear();
            foreach (var obj in objects)
            {
                SetFindTargetAsset(obj, false);
            }
        }
        #endregion

        #region 删除资源
        /// <summary>
        /// 删除指定 ID 范围的资源
        /// </summary>
        public void DeleteResInfo(int startID, int endID)
        {
            m_TempResDefItems.RemoveAll(x => x.ID >= startID && x.ID <= endID);
            int removeCount = ResDefInfos.ConvertData.RemoveAll(item => item.ID >= startID && item.ID <= endID);
            
            Log.Debug($"[Editor] 删除 ID {startID} - {endID}，共 {removeCount} 条");
            ShowNotification($"删除成功：{removeCount} 条");
            
            ResDefInfos.WriteJson();
            UpdateResultInfo();
        }

        /// <summary>
        /// 删除所有失效资源
        /// </summary>
        public void DeleteAllInvalidResInfo()
        {
            if (m_ResultInvalidPath == null) 
                return;

            foreach (var dic in m_ResultInvalidPath.Values.ToList())
            {
                foreach (int id in dic.Keys.ToList())
                {
                    ResDefItem item = m_TempResDefItems.Find(x => x.ID == id);
                    if (item != null)
                        m_TempResDefItems.Remove(item);
                    else
                        ResDefInfos.ConvertData.RemoveAll(x => x.ID == id);
                }
                dic.Clear();
            }

            ResDefInfos.WriteJson();
            UpdateResultInfo();
        }
        #endregion

        #region 重名检查
        /// <summary>
        /// 获取所有重复别名
        /// </summary>
        public Dictionary<string, bool> GetAllSameResInfo()
        {
            var allItems = m_TempResDefItems.Concat(ResDefInfos.ConvertData).ToList();
            return allItems
                .GroupBy(m => m.AliasName)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToDictionary(k => k, _ => true);
        }

        /// <summary>
        /// 检查别名是否重复
        /// </summary>
        public bool IsHaveSameAliasName(string aliasName)
        {
            return m_TempResDefItems.Any(x => x.AliasName == aliasName) 
                || ResDefInfos.ConvertData.Any(x => x.AliasName == aliasName);
        }
        #endregion

        #region 结果数据构建
        /// <summary>
        /// 获取失效路径
        /// </summary>
        public Dictionary<string, Dictionary<int, bool>> GetResultInvalidPathInfo()
        {
            var result = new Dictionary<string, Dictionary<int, bool>>
            {
                { m_SearchTag, new Dictionary<int, bool>() },
                { m_ErrorTag, new Dictionary<int, bool>() }
            };

            foreach (var item in ResDefInfos.ConvertData)
            {
                if (!result.ContainsKey(item.ResType))
                    result[item.ResType] = new Dictionary<int, bool>();

                // 别名异常
                if (!string.IsNullOrEmpty(item.AliasName))
                {
                    string[] arr = item.AliasName.Split("_", 2);
                    if (arr != null && arr.Length == 2 && !arr[1].Equals(item.AssetName) 
                        && !result[item.ResType].ContainsKey(item.ID))
                    {
                        result[item.ResType][item.ID] = true;
                        result[m_SearchTag][item.ID] = true;
                        result[m_ErrorTag][item.ID] = true;
                    }
                }

                // 路径/文件异常
                if (Directory.Exists(item.ABPath))
                {
                    string[] files = Directory.GetFiles(item.ABPath, 
                        item.AssetName + GetAssetsSuffixByType(item.ResType), SearchOption.AllDirectories);

                    if (files.Length == 0)
                    {
                        result[item.ResType][item.ID] = true;
                        result[m_SearchTag][item.ID] = true;
                        result[m_ErrorTag][item.ID] = true;
                    }
                    else
                    {
                        CheckFileABPath(files[0], out string newAbPath);
                        if (item.ABPath != newAbPath)
                        {
                            result[item.ResType][item.ID] = true;
                            result[m_SearchTag][item.ID] = true;
                            result[m_ErrorTag][item.ID] = true;
                        }
                    }
                }
                else
                {
                    if (!File.Exists(item.ABPath + GetAssetsSuffixByType(item.ResType)))
                    {
                        result[item.ResType][item.ID] = true;
                        result[m_SearchTag][item.ID] = true;
                        result[m_ErrorTag][item.ID] = true;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 获取结果详情
        /// </summary>
        public Dictionary<string, List<ResDefItem>> GetResultDetailInfo()
        {
            var result = new Dictionary<string, List<ResDefItem>>();
            foreach (var item in ResDefInfos.ConvertData)
            {
                if (!result.ContainsKey(item.ResType))
                    result[item.ResType] = new List<ResDefItem>();
                
                result[item.ResType].Add(item);
            }

            foreach (var pair in result)
            {
                if (m_AllShowFoldout.ContainsKey(pair.Key))
                {
                    m_AllShowFoldout[pair.Key].PageCount = Mathf.CeilToInt(pair.Value.Count / 
                        (float)m_AllShowFoldout[pair.Key].OnePageCount);
                }
            }

            foreach (var list in result.Values)
            {
                list.Sort((a, b) => StrCmpLogicalW(a.AssetName, b.AssetName));
            }

            return result;
        }
        #endregion

        #region 资源类型工具
        /// <summary>
        /// 根据类型获取资源后缀
        /// </summary>
        public string GetAssetsSuffixByType(string typeName)
        {
            if (Enum.TryParse(typeof(GameDefinitions.AssetType), typeName, out object typeObj))
            {
                return GameDefinitions.AssetSuffix[(GameDefinitions.AssetType)typeObj];
            }
            return string.Empty;
        }
        #endregion

        #region AB 配置加载
        /// <summary>
        /// 加载 AB 配置映射
        /// </summary>
        public static void LoadABConfigs()
        {
            s_ABConfigs.Clear();
            string jsonPath = GamePathUtils.Json.GetRootDirectoryFullPath() + "/ABConfigs.json";
            
            if (!File.Exists(jsonPath)) 
                return;

            string content = File.ReadAllText(jsonPath);
            if (string.IsNullOrEmpty(content)) 
                return;

            JObject jObj = JObject.Parse(content);
            Dictionary<string, ABConfigInfo> configs = new Dictionary<string, ABConfigInfo>();

            foreach (var item in jObj)
            {
                JToken data = item.Value;
                string path = data["Path"].ToString();

                if (!configs.ContainsKey(path))
                {
                    configs.Add(path, new ABConfigInfo(
                        int.Parse(data["ID"].ToString()),
                        path,
                        int.Parse(data["PackageMeasureType"].ToString()),
                        data["Rename"].ToString(),
                        data["GroupName"].ToString(),
                        bool.Parse(data["IsIncreaserGroup"].ToString()),
                        bool.Parse(data["IsCommonIncreaserGroup"].ToString())
                    ));
                }
            }

            // 构建 ABName -> Path 映射
            foreach (var pair in configs)
            {
                ABConfigInfo info = pair.Value;

                if (info.PackageMeasureType == 0)
                {
                    if ((info.IsPlatformManifest || File.Exists(info.Path) || Directory.Exists(info.Path)))
                    {
                        AssetImporter importer = AssetImporter.GetAtPath(info.Path);
                        if (importer != null && string.IsNullOrEmpty(info.Rename))
                        {
                            string abName = info.Path.Replace('/', '@').ToLower() + ".bundle";
                            if (!s_ABConfigs.ContainsKey(abName))
                                s_ABConfigs[abName] = info.Path;
                        }
                    }
                }
                else if (info.PackageMeasureType == 1)
                {
                    if (Directory.Exists(info.Path))
                    {
                        string root = Path.Combine(Application.dataPath[..^6], info.Path).Replace('\\', '/');
                        foreach (string fullPath in Directory.GetFiles(root))
                        {
                            if (fullPath.EndsWith(".meta")) continue;

                            string assetPath = "Assets/" + fullPath[(Application.dataPath.Length + 1)..];
                            AssetImporter importer = AssetImporter.GetAtPath(assetPath);
                            if (importer == null) continue;

                            int dotIdx = assetPath.LastIndexOf('.');
                            dotIdx = dotIdx < 0 ? assetPath.Length : dotIdx;
                            string name = assetPath.Substring(0, dotIdx).Replace('/', '@').ToLower() + ".bundle";
                            if (!s_ABConfigs.ContainsKey(name))
                                s_ABConfigs[name] = assetPath.Substring(0, dotIdx);
                        }
                    }
                }
                else if (info.PackageMeasureType == 2)
                {
                    if (Directory.Exists(info.Path))
                    {
                        string root = Path.Combine(Application.dataPath[..^6], info.Path).Replace('\\', '/');
                        foreach (string fullPath in Directory.GetDirectories(root))
                        {
                            string assetPath = "Assets/" + fullPath[(Application.dataPath.Length + 1)..];
                            AssetImporter importer = AssetImporter.GetAtPath(assetPath);
                            if (importer == null) continue;

                            int dotIdx = assetPath.LastIndexOf('.');
                            dotIdx = dotIdx < 0 ? assetPath.Length : dotIdx;
                            string name = assetPath.Substring(0, dotIdx).Replace('/', '@').ToLower() + ".bundle";
                            if (!s_ABConfigs.ContainsKey(name))
                                s_ABConfigs[name] = assetPath.Substring(0, dotIdx);
                        }
                    }
                }
                else if (info.PackageMeasureType == 3)
                {
                    if (Directory.Exists(info.Path))
                    {
                        string root = Path.Combine(Application.dataPath[..^6], info.Path).Replace('\\', '/');

                        // 文件
                        foreach (string fullPath in Directory.GetFiles(root))
                        {
                            if (fullPath.EndsWith(".meta")) continue;

                            string assetPath = "Assets/" + fullPath[(Application.dataPath.Length + 1)..];
                            AssetImporter importer = AssetImporter.GetAtPath(assetPath);
                            if (importer == null) continue;

                            int dotIdx = assetPath.LastIndexOf('.');
                            dotIdx = dotIdx < 0 ? assetPath.Length : dotIdx;
                            string name = assetPath.Substring(0, dotIdx).Replace('/', '@').ToLower() + ".bundle";
                            if (!s_ABConfigs.ContainsKey(name))
                                s_ABConfigs[name] = assetPath.Substring(0, dotIdx);
                        }

                        // 目录
                        foreach (string fullPath in Directory.GetDirectories(root))
                        {
                            string assetPath = "Assets/" + fullPath[(Application.dataPath.Length + 1)..];
                            AssetImporter importer = AssetImporter.GetAtPath(assetPath);
                            if (importer == null) continue;

                            int dotIdx = assetPath.LastIndexOf('.');
                            dotIdx = dotIdx < 0 ? assetPath.Length : dotIdx;
                            string name = assetPath.Substring(0, dotIdx).Replace('/', '@').ToLower() + ".bundle";
                            if (!s_ABConfigs.ContainsKey(name))
                                s_ABConfigs[name] = assetPath.Substring(0, dotIdx);
                        }
                    }
                }
            }
        }
        #endregion

        #region 路径工具
        /// <summary>
        /// 获取 AB 对应搜索目录
        /// </summary>
        public string GetAbPathFullDirPath(string abPath)
        {
            string searchDir = Application.dataPath + abPath.Substring(6);
            while (!string.IsNullOrEmpty(searchDir))
            {
                if (Directory.Exists(searchDir))
                    return searchDir;
                
                searchDir = Path.GetDirectoryName(searchDir);
            }
            return Application.dataPath;
        }
        #endregion

        #region Windows 自然排序
        [DllImport("Shlwapi.dll", CharSet = CharSet.Unicode)]
        private static extern int StrCmpLogicalW(string param1, string param2);
        #endregion

        #region 内置图标加载
        /// <summary>
        /// 加载默认按钮图标
        /// </summary>
        private void LoadDefaultTextures()
        {
            List<string> names = new List<string> { "DefaultLeft", "DefaultRight", "DefaultDown", "DefaultTop" };
            foreach (string name in names)
            {
                string path = $"Assets/Res/Textures/PicsForEditor/{name}.png";
                Texture2D tex = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
                m_AllDefaultTextures.Add(tex);
            }
        }
        #endregion

        #region 结果刷新
        /// <summary>
        /// 更新所有结果数据
        /// </summary>
        private void UpdateResultInfo()
        {
            m_ResultInvalidPath = GetResultInvalidPathInfo();
            m_ResultDetailInfo = GetResultDetailInfo();
            m_ResultSameResInfo = GetAllSameResInfo();

            if (m_OnlyShowError)
                UpdateErrorResDefItemInfo();

            if (!string.IsNullOrEmpty(m_SearchResName))
                UpdateSearchResDefItemInfo(m_SearchResName);
        }
        #endregion

        #region 自动修复失效链接
        /// <summary>
        /// 自动刷新所有失效链接
        /// </summary>
        private void AutoRefreshAllInvalidResInfo()
        {
            int fixCount = 0;
            if (m_ResultInvalidPath.TryGetValue(m_ErrorTag, out var errorDic))
            {
                foreach (int id in errorDic.Keys.ToList())
                {
                    int index = ResDefInfos.ConvertData.FindIndex(x => x.ID == id);
                    if (index < 0) continue;

                    ResDefItem item = ResDefInfos.ConvertData[index];
                    if (string.IsNullOrEmpty(item.AssetGUID)) continue;

                    string path = AssetDatabase.GUIDToAssetPath(item.AssetGUID);
                    if (string.IsNullOrEmpty(path)) continue;

                    string fileName = Path.GetFileNameWithoutExtension(path);
                    string suffix = GetAssetsSuffixByType(item.ResType);

                    if (path.EndsWith(suffix) && CheckFileABPath(path, out string newAbPath))
                    {
                        item.ABPath = newAbPath;
                        item.AssetName = fileName;
                        fixCount++;
                    }
                }
            }

            if (fixCount > 0)
            {
                ShowNotification($"自动修复完成：{fixCount} 个");
                m_ResultInvalidPath = GetResultInvalidPathInfo();
            }
        }

        /// <summary>
        /// 自动矫正单个资源
        /// </summary>
        public bool AutoRectifyInvalidResInfo(string fileName, string fileType, string abPath, ResDefItem item, bool autoSave)
        {
            if (string.IsNullOrEmpty(abPath))
                return false;

            string[] arr = item.AliasName.Split("_", 2);
            if (arr != null && arr.Length == 2 && !arr[1].Equals(fileName))
            {
                fileName = arr[1];
            }

            string suffix = GetAssetsSuffixByType(fileType);
            string[] guids = AssetDatabase.FindAssets(fileName, new[] { abPath });
            if (guids == null || guids.Length == 0)
                guids = AssetDatabase.FindAssets(fileName);

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                string ext = Path.GetExtension(path);
                string name = Path.GetFileNameWithoutExtension(path);

                if (!name.Equals(fileName) || !ext.Equals(suffix))
                    continue;

                if (CheckFileABPath(path, out string newAbPath))
                {
                    item.AssetName = name;
                    item.ABPath = newAbPath;
                    if (autoSave) ResDefInfos.WriteJson();
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region 错误/搜索列表
        /// <summary>
        /// 更新错误列表
        /// </summary>
        private void UpdateErrorResDefItemInfo()
        {
            m_ErrorResDefItemInfo.Clear();
            foreach (var item in ResDefInfos.ConvertData)
            {
                if (m_ResultInvalidPath[m_ErrorTag].ContainsKey(item.ID))
                    m_ErrorResDefItemInfo.Add(item);
            }

            if (m_AllShowFoldout.ContainsKey(m_ErrorTag))
            {
                m_AllShowFoldout[m_ErrorTag].CurPageIndex = 1;
                m_AllShowFoldout[m_ErrorTag].PageCount = Mathf.CeilToInt(m_ErrorResDefItemInfo.Count / 
                    (float)m_AllShowFoldout[m_ErrorTag].OnePageCount);
            }
        }

        /// <summary>
        /// 更新搜索结果
        /// </summary>
        private void UpdateSearchResDefItemInfo(string searchName)
        {
            m_SearchResDefItemInfo.Clear();
            string key = searchName.ToLower();

            if (m_OnlyShowError)
            {
                m_SearchResDefItemInfo.AddRange(m_ErrorResDefItemInfo.Where(x => 
                    x.AssetName.ToLower().Contains(key)));
            }
            else
            {
                foreach (var list in m_ResultDetailInfo.Values)
                {
                    m_SearchResDefItemInfo.AddRange(list.Where(x => 
                        x.AssetName.ToLower().Contains(key)));
                }
            }

            if (m_AllShowFoldout.ContainsKey(m_SearchTag))
            {
                m_AllShowFoldout[m_SearchTag].CurPageIndex = 1;
                m_AllShowFoldout[m_SearchTag].PageCount = Mathf.CeilToInt(m_SearchResDefItemInfo.Count / 
                    (float)m_AllShowFoldout[m_SearchTag].OnePageCount);
            }
        }
        #endregion

        #region 查找目标设置
        /// <summary>
        /// 设置查找对象
        /// </summary>
        private void SetFindTargetAsset(Object findTargetAsset, bool isClear = true)
        {
            if (isClear)
                m_FindFileFullPathList.Clear();

            if (findTargetAsset != null)
            {
                if (!UpdateFindAllFileData(findTargetAsset))
                {
                    ShowNotification("当前目录/文件未配置AB");
                }
            }
            else
            {
                ShowNotification("对象为空");
            }
        }
        #endregion

        #region Excel 导出
        /// <summary>
        /// 从 Excel 导出 ABConfig 配置
        /// </summary>
        public static bool ExportExcelToJsonFromABConfig(string excelName)
        {
            string jsonDir = GamePathUtils.Json.GetRootDirectoryFullPath();
            string jsonPath = $"{jsonDir}/{excelName}.json";

            if (!Directory.Exists(jsonDir))
                Directory.CreateDirectory(jsonDir);

            return TableExportEditorUtility.ExportExcelToJson(GamePathUtils.AB.GetExcelFileFullPath(), jsonPath);
        }
        #endregion
    }
}