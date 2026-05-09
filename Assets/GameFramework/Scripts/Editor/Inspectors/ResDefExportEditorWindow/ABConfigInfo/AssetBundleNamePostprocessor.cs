using System.Collections.Generic;
using System.IO;
using Honor.Runtime;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;

namespace Honor.Editor
{
    public class AssetBundleNamePostprocessor : AssetPostprocessor
    {
        /// <summary>
        /// AB配置信息
        /// <Path, ABConfigInfo>
        /// </summary>
        public static Dictionary<string, ABConfigInfo> s_ABConfigs = new Dictionary<string, ABConfigInfo>();

        /// <summary>
        /// 所有AB是否为增量包的信息
        /// <abName, 是否为增量包>
        /// </summary>
        private static Dictionary<string, bool> s_ABIsIncreaser = new Dictionary<string, bool>();

        /// <summary>
        /// 所有AB是否为公用增量包的信息
        /// <abName, 是否为公用增量包>
        /// </summary>
        private static Dictionary<string, bool> s_ABIsCommonIncreaser = new Dictionary<string, bool>();

        /// <summary>
        /// 所有AB对应的分组名称
        /// <abName, 分组名称>
        /// </summary>
        private static Dictionary<string, string> s_ABGroupName = new Dictionary<string, string>();

        /// <summary>
        /// 收集ABConfig信息
        /// </summary>
        public static void RefreshABConfigs()
        {
            string filePathList = Runtime.GamePathUtils.Json.GetRootDirectoryFullPath() + "/ABConfigs.json";
            string content = File.ReadAllText(filePathList);
            if (string.IsNullOrEmpty(content))
            {
                return;
            }
            JObject jObject = JObject.Parse(content);

            s_ABConfigs.Clear();

            foreach (var itr in jObject)
            {
                JToken data = itr.Value;
                string path = data["Path"].ToString(); // 取出路径
                if (!s_ABConfigs.ContainsKey(path))
                {
                    s_ABConfigs.Add(path,
                        new ABConfigInfo(int.Parse(data["ID"].ToString()), path,
                            int.Parse(data["PackageMeasureType"].ToString()), data["Rename"].ToString(),
                            data["GroupName"].ToString(), bool.Parse(data["IsIncreaserGroup"].ToString()),
                            bool.Parse(data["IsCommonIncreaserGroup"].ToString())));
                }
            }
        }

        /// <summary>
        /// 清除所有AssetBundle名称
        /// </summary>
        public static void CleanAllAsssetBundleNames()
        {
            RefreshABConfigs();
            string[] assetBundleNames = AssetDatabase.GetAllAssetBundleNames();
            for (int j = 0; j < assetBundleNames.Length; j++)
            {
                AssetDatabase.RemoveAssetBundleName(assetBundleNames[j], true);
            }

            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 刷新所有AssetBundle名称
        /// </summary>
        /// <param name="forceCheckPath">是否进行路径的强制检测</param>
        public static void RefreshAllAssetBundleNames(bool forceCheckPath = false)
        {
            CleanAllAsssetBundleNames();

            s_ABIsIncreaser.Clear();
            s_ABIsCommonIncreaser.Clear();
            s_ABGroupName.Clear();

            foreach (var config in s_ABConfigs)
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
                            if (!string.IsNullOrEmpty(info.Rename))
                            {
                                importer.assetBundleName = info.Rename.ToLower();
                            }
                            else
                            {
                                int indexOfSuffixFlag = info.Path.LastIndexOf('.');
                                indexOfSuffixFlag = indexOfSuffixFlag < 0 ? info.Path.Length : indexOfSuffixFlag;
                                importer.assetBundleName = AorTxt.Format("{0}.bundle",
                                    info.Path.Substring(0, indexOfSuffixFlag).Replace('/', '@').ToLower());
                            }

                            if (!s_ABIsIncreaser.ContainsKey(importer.assetBundleName))
                            {
                                s_ABIsIncreaser.Add(importer.assetBundleName, info.IsIncreaserGroup);
                            }

                            if (!s_ABIsCommonIncreaser.ContainsKey(importer.assetBundleName))
                            {
                                s_ABIsCommonIncreaser.Add(importer.assetBundleName, info.IsCommonIncreaserGroup);
                            }

                            if (!s_ABGroupName.ContainsKey(importer.assetBundleName))
                            {
                                s_ABGroupName.Add(importer.assetBundleName, info.GroupName);
                            }
                        }
                    }
                    else
                    {
                        if (forceCheckPath)
                        {
                            Log.Error(
                                $"[Editor] RefreshAllAssetBundleNames() 中 ID : {info.ID} 的 Path : {info.Path} 并不是一个已存在的文件或目录。");
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
                                    importer.assetBundleName = AorTxt.Format("{0}.bundle",
                                        filePath.Substring(0, indexOfSuffixFlag).Replace('/', '@').ToLower());
                                    if (!s_ABIsIncreaser.ContainsKey(importer.assetBundleName))
                                    {
                                        s_ABIsIncreaser.Add(importer.assetBundleName, info.IsIncreaserGroup);
                                    }

                                    if (!s_ABIsCommonIncreaser.ContainsKey(importer.assetBundleName))
                                    {
                                        s_ABIsCommonIncreaser.Add(importer.assetBundleName,
                                            info.IsCommonIncreaserGroup);
                                    }

                                    if (!s_ABGroupName.ContainsKey(importer.assetBundleName))
                                    {
                                        s_ABGroupName.Add(importer.assetBundleName, info.GroupName);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (forceCheckPath)
                        {
                            Log.Error(
                                $"[Editor] RefreshAllAssetBundleNames() 中 ID : {info.ID} 的 Path : {info.Path} 并不是一个已存在的目录。");
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
                                    importer.assetBundleName = AorTxt.Format("{0}.bundle",
                                        directoryPath.Substring(0, indexOfSuffixFlag).Replace('/', '@').ToLower());
                                    if (!s_ABIsIncreaser.ContainsKey(importer.assetBundleName))
                                    {
                                        s_ABIsIncreaser.Add(importer.assetBundleName, info.IsIncreaserGroup);
                                    }

                                    if (!s_ABIsCommonIncreaser.ContainsKey(importer.assetBundleName))
                                    {
                                        s_ABIsCommonIncreaser.Add(importer.assetBundleName,
                                            info.IsCommonIncreaserGroup);
                                    }

                                    if (!s_ABGroupName.ContainsKey(importer.assetBundleName))
                                    {
                                        s_ABGroupName.Add(importer.assetBundleName, info.GroupName);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (forceCheckPath)
                        {
                            Log.Error(
                                $"[Editor] RefreshAllAssetBundleNames() 中 ID : {info.ID} 的 Path : {info.Path} 并不是一个已存在的目录。");
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
                                    importer.assetBundleName = AorTxt.Format("{0}.bundle",
                                        filePath.Substring(0, indexOfSuffixFlag).Replace('/', '@').ToLower());
                                    if (!s_ABIsIncreaser.ContainsKey(importer.assetBundleName))
                                    {
                                        s_ABIsIncreaser.Add(importer.assetBundleName, info.IsIncreaserGroup);
                                    }

                                    if (!s_ABIsCommonIncreaser.ContainsKey(importer.assetBundleName))
                                    {
                                        s_ABIsCommonIncreaser.Add(importer.assetBundleName,
                                            info.IsCommonIncreaserGroup);
                                    }

                                    if (!s_ABGroupName.ContainsKey(importer.assetBundleName))
                                    {
                                        s_ABGroupName.Add(importer.assetBundleName, info.GroupName);
                                    }
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
                                    importer.assetBundleName = AorTxt.Format("{0}.bundle",
                                        directoryPath.Substring(0, indexOfSuffixFlag).Replace('/', '@').ToLower());
                                    if (!s_ABIsIncreaser.ContainsKey(importer.assetBundleName))
                                    {
                                        s_ABIsIncreaser.Add(importer.assetBundleName, info.IsIncreaserGroup);
                                    }

                                    if (!s_ABIsCommonIncreaser.ContainsKey(importer.assetBundleName))
                                    {
                                        s_ABIsCommonIncreaser.Add(importer.assetBundleName,
                                            info.IsCommonIncreaserGroup);
                                    }

                                    if (!s_ABGroupName.ContainsKey(importer.assetBundleName))
                                    {
                                        s_ABGroupName.Add(importer.assetBundleName, info.GroupName);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (forceCheckPath)
                        {
                            Log.Error(
                                $"[Editor] RefreshAllAssetBundleNames() 中 ID : {info.ID} 的 Path : {info.Path} 并不是一个已存在的目录。");
                        }
                    }
                }
                // 小游戏打包类型（小游戏下每个文件夹单独打包ab，其中Lua相关文件夹仅打包LuacScripts为ab，Textures下每个文件夹单独成ab且仅打包非Atlas文件夹）
                else if (info.PackageMeasureType == 4)
                {
                    if (Directory.Exists(info.Path))
                    {
                        string miniGameFullPath = System.IO.Path
                            .Combine(Application.dataPath.Substring(0, Application.dataPath.Length - "Assets".Length),
                                info.Path).Replace('\\', '/');
                        RemoveDirectoryMetaFileAssetBundleName(miniGameFullPath, true);
                        string assetBundleName = AorTxt.Format("{0}.bundle", info.Path.Replace('/', '@').ToLower());
                        string[] fullPaths = Directory.GetDirectories(miniGameFullPath);
                        foreach (string fullPath in fullPaths)
                        {
                            if (Directory.Exists(fullPath))
                            {
                                string formatFullPath = fullPath.Replace('\\', '/');
                                string directoryPath = AorTxt.Format("Assets/{0}",
                                    formatFullPath.Substring(Application.dataPath.Length + 1,
                                        formatFullPath.Length - (Application.dataPath.Length + 1)));
                                string directoryName = System.IO.Path.GetFileNameWithoutExtension(directoryPath);

                                if (directoryName == "LuaScripts") //  lua不添加到bundle里
                                {
                                    continue;
                                }
                                else if (directoryName == "Common")
                                {
                                    string[] commonFullPaths = Directory.GetDirectories(formatFullPath);
                                    foreach (string commonFullPath in commonFullPaths)
                                    {
                                        if (Directory.Exists(commonFullPath))
                                        {
                                            string commonFormatFullPath = commonFullPath.Replace('\\', '/');
                                            string commonDirectoryPath = AorTxt.Format("Assets/{0}",
                                                commonFormatFullPath.Substring(Application.dataPath.Length + 1,
                                                    commonFormatFullPath.Length - (Application.dataPath.Length + 1)));
                                            string commonDirectoryName =
                                                System.IO.Path.GetFileNameWithoutExtension(commonDirectoryPath);
                                            if (commonDirectoryName == "LuaScripts") //  lua不添加到bundle里
                                            {
                                                continue;
                                            }

                                            AssetImporter importer1 = AssetImporter.GetAtPath(commonDirectoryPath);
                                            if (importer1)
                                            {
                                                importer1.assetBundleName = assetBundleName;
                                            }
                                        }
                                    }

                                    continue;
                                }
                                else if (directoryName == "Textures") // textures要去除Atlas目录
                                {
                                    string[] texturesFullPaths = Directory.GetDirectories(formatFullPath);
                                    foreach (string texturesFullPath in texturesFullPaths)
                                    {
                                        if (Directory.Exists(texturesFullPath))
                                        {
                                            string textureFormatFullPath = texturesFullPath.Replace('\\', '/');
                                            string textureDirectoryPath = AorTxt.Format("Assets/{0}",
                                                textureFormatFullPath.Substring(Application.dataPath.Length + 1,
                                                    textureFormatFullPath.Length - (Application.dataPath.Length + 1)));
                                            string textureDirectoryName =
                                                System.IO.Path.GetFileNameWithoutExtension(textureDirectoryPath);
                                            if (textureDirectoryName == "Atlas") //  合图文件不添加到bundle里
                                            {
                                                continue;
                                            }

                                            AssetImporter importer1 = AssetImporter.GetAtPath(textureDirectoryPath);
                                            if (importer1)
                                            {
                                                importer1.assetBundleName = assetBundleName;
                                            }
                                        }
                                    }

                                    continue;
                                }

                                AssetImporter importer2 = AssetImporter.GetAtPath(directoryPath);
                                if (importer2)
                                {
                                    importer2.assetBundleName = assetBundleName;
                                }
                            }
                        }

                        if (!s_ABIsIncreaser.ContainsKey(assetBundleName))
                        {
                            s_ABIsIncreaser.Add(assetBundleName, info.IsIncreaserGroup);
                        }

                        if (!s_ABIsCommonIncreaser.ContainsKey(assetBundleName))
                        {
                            s_ABIsCommonIncreaser.Add(assetBundleName, info.IsCommonIncreaserGroup);
                        }

                        if (!s_ABGroupName.ContainsKey(assetBundleName))
                        {
                            s_ABGroupName.Add(assetBundleName, info.GroupName);
                        }
                    }
                    else
                    {
                        if (forceCheckPath)
                        {
                            Log.Error(
                                $"[Editor] RefreshAllAssetBundleNames() 中 ID : {info.ID} 的 Path : {info.Path} 并不是一个已存在的目录。");
                        }
                    }
                }
            }

            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 是否为增量捆绑包
        /// </summary>
        /// <param name="assetBundleName"></param>
        /// <returns></returns>
        public static bool IsIncreaserAssetBundle(string assetBundleName)
        {
            if (!string.IsNullOrEmpty(assetBundleName))
            {
                if (s_ABIsIncreaser.Count == 0)
                {
                    RefreshAllAssetBundleNames();
                }

                if (s_ABIsIncreaser.ContainsKey(assetBundleName))
                {
                    return s_ABIsIncreaser[assetBundleName];
                }
            }

            return false;
        }

        /// <summary>
        /// 是否为公用增量捆绑包
        /// </summary>
        /// <param name="assetBundleName"></param>
        /// <returns></returns>
        public static bool IsCommonIncreaserAssetBundle(string assetBundleName)
        {
            if (!string.IsNullOrEmpty(assetBundleName))
            {
                if (s_ABIsCommonIncreaser.Count == 0)
                {
                    RefreshAllAssetBundleNames();
                }

                if (s_ABIsCommonIncreaser.ContainsKey(assetBundleName))
                {
                    return s_ABIsCommonIncreaser[assetBundleName];
                }
            }

            return false;
        }

        /// <summary>
        /// 是否为相同分组
        /// </summary>
        /// <param name="assetBundleName1"></param>
        /// <param name="assetBundleName2"></param>
        /// <returns></returns>
        public static bool IsAssetBundleInSameGroup(string assetBundleName1, string assetBundleName2)
        {
            if (!string.IsNullOrEmpty(assetBundleName1) && !string.IsNullOrEmpty(assetBundleName2))
            {
                if (s_ABGroupName.Count == 0)
                {
                    RefreshAllAssetBundleNames();
                }

                string groupName1 = s_ABGroupName.ContainsKey(assetBundleName1)
                    ? s_ABGroupName[assetBundleName1]
                    : string.Empty;
                string groupName2 = s_ABGroupName.ContainsKey(assetBundleName2)
                    ? s_ABGroupName[assetBundleName2]
                    : string.Empty;
                bool isSameGroupName = groupName1.Equals(groupName2);
                return isSameGroupName;
            }

            return false;
        }

        /// <summary>
        /// 删除meta文件的AssetBundleName
        /// </summary>
        /// <param name="mateFilePath"></param>
        private static void RemoveMetaFileAssetBundleName(string mateFilePath)
        {
            if (File.Exists(mateFilePath))
            {
                string matchString = "assetBundleName:";
                string metaContent = File.ReadAllText(mateFilePath);
                int startIndex = metaContent.IndexOf(matchString);

                if (startIndex >= 0)
                {
                    int endIndex = metaContent.IndexOf('\n', startIndex);
                    if (endIndex - startIndex - matchString.Length > 2)
                    {
                        metaContent = metaContent.Remove(startIndex + matchString.Length,
                            endIndex - startIndex - matchString.Length);
                        File.WriteAllText(mateFilePath, metaContent);
                    }
                }
            }
        }


        /// <summary>
        /// 删除文件夹里所有meta文件的AssetBundleName
        /// </summary>
        /// <param name="directoryPath">目标文件夹</param>
        /// <param name="isIncludeSelf">是否包含删除自身</param>
        private static void RemoveDirectoryMetaFileAssetBundleName(string directoryPath, bool isIncludeSelf = false)
        {
            var matchFileArray = Directory.GetFiles(directoryPath, "*.meta", System.IO.SearchOption.AllDirectories);
            foreach (var mateFilePath in matchFileArray)
            {
                RemoveMetaFileAssetBundleName(mateFilePath);
            }

            if (isIncludeSelf)
            {
                RemoveMetaFileAssetBundleName(directoryPath + ".meta");
            }
        }

        ///// <summary>
        ///// 个别资源变动回调
        ///// </summary>
        ///// <param name="imported">导入资源路径集合</param>
        ///// <param name="deleted">删除资源路径集合</param>
        ///// <param name="moved">移动资源路径集合</param>
        ///// <param name="movedFromAssetPaths">移动资源来源路径集合</param>
        //public static void OnPostprocessAllAssets(string[] imported, string[] deleted, string[] moved, string[] movedFromAssetPaths)
        //{
        //    RefreshABConfigs();

        //    string abConfigsFilePathList = Runtime.Path.Json.GetRootDirectoryRelativePath() + "/ABConfigs.json";
        //    for (int index = 0; index < imported.Length; index++)
        //    {
        //        if(imported[index] == abConfigsFilePathList)
        //        {
        //            RefreshAllAsssetBundleNames();
        //            AssetDatabase.Refresh();
        //            return;
        //        }
        //    }

        //    // 清除需要被清除的assetbundleName
        //    List<string> needRemovePaths = new List<string>();
        //    for(int index = 0; index < deleted.Length; index++)
        //    {
        //        if(!needRemovePaths.Contains(deleted[index]))
        //        {
        //            needRemovePaths.Add(deleted[index]);
        //        }
        //    }
        //    for (int index = 0; index < movedFromAssetPaths.Length; index++)
        //    {
        //        if (!needRemovePaths.Contains(movedFromAssetPaths[index]))
        //        {
        //            needRemovePaths.Add(movedFromAssetPaths[index]);
        //        }
        //    }

        //    List<string> assetBundleNames = new List<string>(AssetDatabase.GetAllAssetBundleNames());
        //    needRemovePaths.ForEach((needRemovePath) =>
        //    {
        //        int indexOfSuffixFlag = needRemovePath.LastIndexOf('.');
        //        indexOfSuffixFlag = indexOfSuffixFlag < 0 ? needRemovePath.Length : indexOfSuffixFlag;
        //        string assetBundleName = needRemovePath.Substring(0, indexOfSuffixFlag).Replace('/', '@').ToLower();
        //        assetBundleNames.ForEach((checkBundleName) =>
        //        {
        //            if (checkBundleName == assetBundleName)
        //            {
        //                AssetDatabase.RemoveAssetBundleName(assetBundleName, true);
        //            }
        //        });
        //    });

        //    List<string> modifiedPaths = new List<string>();
        //    for (int index = 0; index < imported.Length; index++)
        //    {
        //        if (!modifiedPaths.Contains(imported[index]))
        //        {
        //            modifiedPaths.Add(imported[index]);
        //        }
        //    }
        //    for (int index = 0; index < moved.Length; index++)
        //    {
        //        if (!modifiedPaths.Contains(moved[index]))
        //        {
        //            modifiedPaths.Add(moved[index]);
        //        }
        //    }

        //    if (modifiedPaths.Count > 0)
        //    {
        //        RefreshAllAsssetBundleNames();
        //    }

        //    AssetDatabase.Refresh();
        //}
    }
}