using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// 
    /// </summary>
    public static class GamePathUtils
    {
        /// <summary>
        /// 平台名称
        /// </summary>
#if UNITY_IOS
        public static string PlatformName = "iOS";
#elif UNITY_WEBGL
        public static string PlatformName = "WebGL";
#else
        public static string PlatformName = "Android";
#endif
        /// <summary>
        /// 大版本更新相关路径信息
        /// </summary>
        public static class AppDownload
        {
            /// <summary>
            /// 服务器uri相关路径信息
            /// </summary>
            public static class Uri
            {
                /*/// <summary>
                /// 获取App商店的uri
                /// </summary>
                public static string GetStoreUri()
                {
                    return Root.Config.GetString("StoreUrl", true);
                }*/
            }
        }

        /// <summary>
        /// AssetBundle相关路径信息
        /// </summary>
        public static class AssetBundleAB
        {
            /// <summary>
            /// AB文件夹前缀
            /// </summary>
            public static string DirectoryPrefix = AorTxt.Format("AssetBundles/{0}", PlatformName);

            /// <summary>
            /// 版本列表文件名称
            /// </summary>
            public static string VersionFileName = "version.txt";

            /// <summary>
            /// 灰度版本列表文件名称
            /// </summary>
            private static string s_GrayVersionFileName = "versiongray_{0}.txt";

            /// <summary>
            /// 获取ABConfigs的Excel根目录的绝对路径
            /// 编辑器工具类
            /// </summary>
            /// <returns></returns>
            public static string Get_Excel_Root_Directory_Full_Path()
            {
                return AorTxt.Format("{0}/{1}", Application.dataPath, "../Docs/Designs/Excels/ABConfigs");
            }

            /// <summary>
            /// 获取ABConfigs的Excel文件的绝对路径
            /// 编辑器工具类
            /// </summary>
            /// <returns></returns>
            public static string GetExcelFileFullPath()
            {
                return AorTxt.Format("{0}/{1}", Application.dataPath,
                    "../Docs/Designs/Excels/ABConfigs/ABConfigs.xlsm");
            }

            /// <summary>
            /// 获取灰度version文件名称
            /// </summary>
            /// <param name="appVersion">应用版本号：x.y.z</param>
            /// <returns></returns>
            public static string GetVersionGrayFileName(string appVersion)
            {
                return AorTxt.Format(s_GrayVersionFileName, appVersion);
            }

            /// <summary>
            /// AssetBundle生成路径（用来提交到服务器）
            /// </summary>
            public static class ForServer
            {
                /// <summary>
                /// 获取生成的AB平台根目录的绝对路径
                /// </summary>
                /// <param name="platformName">平台名称</param>
                /// <returns></returns>
                public static string GetPlatformFolderFullPath(string platformName)
                {
                    return $"{Application.dataPath}/../AssetBundles/{platformName}".Replace('\\', '/');
                }

                /// <summary>
                /// 获取生成的AB根目录的绝对路径
                /// </summary>
                /// <param name="platformName">平台名称</param>
                /// <param name="appVersion">APP版本号</param>
                /// <param name="resMinor">资源小版本号</param>
                /// <returns></returns>
                public static string GetRootDirectoryFullPath(string platformName, string appVersion, int resMinor)
                {
                    return AorTxt.Format("{0}/../AssetBundles/{1}/{2}.{3}", Application.dataPath, platformName,
                        appVersion,
                        resMinor).Replace('\\', '/');
                }

                /// <summary>
                /// 获取生成的AB根目录下version文件的绝对路径
                /// </summary>
                /// <param name="platformName">平台名称</param>
                /// <param name="appVersion">APP版本号</param>
                /// <param name="resMinor">资源小版本号</param>
                /// <returns></returns>
                public static string GetVersionFileFullPath(string platformName, string appVersion, int resMinor)
                {
                    return AorTxt.Format("{0}/../AssetBundles/{1}/{2}.{3}/{4}", Application.dataPath, platformName,
                        appVersion, resMinor, VersionFileName).Replace('\\', '/');
                }

                /// <summary>
                /// 获取生成的AB根目录下version文件的绝对路径
                /// </summary>
                /// <param name="platformName">平台名称</param>
                /// <returns></returns>
                public static string GetVersionFileFullPath(string platformName, string version)
                {
                    return $"{Application.dataPath}/../AssetBundles/{platformName}/{version}/{VersionFileName}"
                        .Replace('\\', '/');
                }

                /// <summary>
                /// 获取生成的AB文件的绝对路径
                /// </summary>
                /// <param name="platformName">平台名称</param>
                /// <param name="appVersion">APP版本号</param>
                /// <param name="resMinor">资源小版本号</param>
                /// <param name="formatPath">ab的格式化路径（从asset/开始的全小写路径信息）</param>
                /// <returns></returns>
                public static string GetFileFullPath(string platformName, string appVersion, int resMinor,
                    string formatPath)
                {
                    return System.IO.Path
                        .Combine(GetRootDirectoryFullPath(platformName, appVersion, resMinor), formatPath)
                        .Replace('\\', '/');
                }
            }

            /// <summary>
            /// AssetBundle可写路径下相关路径信息
            /// </summary>
            public static class Persistent
            {
                /// <summary>
                /// 获取可写路径下AB根目录的绝对路径
                /// </summary>
                /// <returns></returns>
                public static string GetRootDirectoryFullPath(string platformName = null)
                {
                    if (string.IsNullOrEmpty(platformName))
                    {
                        return System.IO.Path.Combine(Application.persistentDataPath, DirectoryPrefix)
                            .Replace('\\', '/');
                    }
                    else
                    {
                        return System.IO.Path
                            .Combine(Application.persistentDataPath, AorTxt.Format("AssetBundles/{0}", platformName))
                            .Replace('\\', '/');
                    }
                }

                /// <summary>
                /// 获取可写路径下AB根目录下version文件的绝对路径
                /// </summary>
                /// <returns></returns>
                public static string GetVersionFileFullPath()
                {
                    return System.IO.Path.Combine(Application.persistentDataPath, DirectoryPrefix, VersionFileName)
                        .Replace('\\', '/');
                }

                /// <summary>
                /// 获取可写路径下AB文件的绝对路径
                /// </summary>
                /// <param name="formatPath">ab的格式化路径（从asset/开始的全小写路径信息）</param>
                /// <returns></returns>
                public static string GetFileFullPath(string formatPath)
                {
                    return System.IO.Path.Combine(GetRootDirectoryFullPath(), formatPath).Replace('\\', '/');
                }
            }

            /// <summary>
            /// AssetBundle临时可写路径下相关路径信息
            /// </summary>
            public static class PersistentTmp
            {
                /// <summary>
                /// 获取可写路径下临时AB根目录的绝对路径
                /// </summary>
                /// <returns></returns>
                public static string GetRootDirectoryFullPath()
                {
                    return System.IO.Path.Combine(Application.persistentDataPath, DirectoryPrefix, "___Tmp___")
                        .Replace('\\', '/');
                }

                /// <summary>
                /// 获取可写路径下临时AB根目录下version文件的绝对路径
                /// </summary>
                /// <returns></returns>
                public static string GetVersionFileFullPath()
                {
                    return System.IO.Path
                        .Combine(Application.persistentDataPath, DirectoryPrefix, "___Tmp___", VersionFileName)
                        .Replace('\\', '/');
                }

                /// <summary>
                /// 获取可写路径下临时AB文件的绝对路径
                /// </summary>
                /// <param name="formatPath">ab的格式化路径（从asset/开始的全小写路径信息）</param>
                /// <returns></returns>
                public static string GetFileFullPath(string formatPath)
                {
                    return System.IO.Path.Combine(GetRootDirectoryFullPath(), formatPath).Replace('\\', '/');
                }
            }

            /// <summary>
            /// AssetBundle缓存路径下相关路径信息
            /// </summary>
            public static class Caching
            {
                /// <summary>
                /// 获取缓存路径下AB根目录的绝对路径
                /// </summary>
                /// <returns></returns>
                public static string GetRootDirectoryFullPath()
                {
                    return System.IO.Path.Combine(UnityEngine.Caching.currentCacheForWriting.path).Replace('\\', '/');
                }

                /// <summary>
                /// 获取缓存路径下AB文件的绝对路径
                /// </summary>
                /// <param name="formatPath">ab的格式化路径（从asset/开始的全小写路径信息）</param>
                /// <returns></returns>
                public static string GetFileFullPath(string formatPath)
                {
                    return System.IO.Path.Combine(GetRootDirectoryFullPath(), formatPath).Replace('\\', '/');
                }
            }

            /// <summary>
            /// AssetBundle只读路径下相关路径信息
            /// </summary>
            public static class Streaming
            {
                /// <summary>
                /// 获取只读路径下AB根目录的绝对路径
                /// </summary>
                /// <returns></returns>
                public static string GetRootDirectoryFullPath(string platformName = null)
                {
#if UNITY_IOS && !UNITY_EDITOR
                    if (string.IsNullOrEmpty(platformName))
                    {
                        return Txt.Format("{0}{1}{2}", Application.dataPath, "/Raw/", DirectoryPrefix).Replace('\\', '/');
                    }
                    else
                    {
                        return Txt.Format("{0}{1}{2}", Application.dataPath, "/Raw/", Txt.Format("AssetBundles/{0}", platformName)).Replace('\\', '/');
                    }
#elif UNITY_WEBGL && UNITY_EDITOR
                    if (string.IsNullOrEmpty(platformName))
                    {
                        return Txt.Format("{0}{1}{2}", Application.dataPath, "/StreamingAssets/", DirectoryPrefix).Replace('\\', '/');
                    }
                    else
                    {
                        return Txt.Format("{0}{1}{2}", Application.dataPath, "/StreamingAssets/", Txt.Format("AssetBundles/{0}", platformName)).Replace('\\', '/');
                    }
#else
                    if (string.IsNullOrEmpty(platformName))
                    {
                        return System.IO.Path.Combine(Application.streamingAssetsPath, DirectoryPrefix)
                            .Replace('\\', '/');
                    }
                    else
                    {
                        return System.IO.Path
                            .Combine(Application.streamingAssetsPath, AorTxt.Format("AssetBundles/{0}", platformName))
                            .Replace('\\', '/');
                    }
#endif
                }

                /// <summary>
                /// 获取只读路径下AB根目录下version文件的绝对路径
                /// </summary>
                /// <returns></returns>
                public static string GetVersionFileFullPath()
                {
#if UNITY_IOS && !UNITY_EDITOR
                    return Txt.Format("{0}{1}{2}{3}{4}{5}", "file://", Application.dataPath, "/Raw/", DirectoryPrefix, "/", VersionFileName);
#elif (UNITY_ANDROID || UNITY_WEBGL) && !UNITY_EDITOR
                    return System.IO.Path.Combine(Application.streamingAssetsPath, DirectoryPrefix, VersionFileName).Replace('\\', '/');
#else
                    return AorTxt.Format("{0}{1}{2}{3}{4}{5}", "file://", Application.dataPath, "/StreamingAssets/",
                        DirectoryPrefix, "/", VersionFileName);
#endif
                }

                /// <summary>
                /// 获取只读路径下AB文件的绝对路径
                /// </summary>
                /// <param name="formatPath">ab的格式化路径（从asset/开始的全小写路径信息）</param>
                /// <returns></returns>
                public static string GetFileFullPath(string formatPath)
                {
                    return System.IO.Path.Combine(GetRootDirectoryFullPath(), formatPath).Replace('\\', '/');
                }
            }

            /// <summary>
            /// 服务器uri相关路径信息
            /// </summary>
            public static class Uri
            {
                /// <summary>
                /// 获取指定热更新资源文件的uri
                /// </summary>
                /// <param name="fileName">uri对应的文件名称（带后缀名）</param>
                /// <returns></returns>
                public static string GetHotfixFileUri(string fileName, string versionno = "")
                {
                    /*string uriConfig = Root.Config.GetHotfixUrl(); //Root.Config.GetString("HotfixUrl", true);
                    if (!string.IsNullOrEmpty(uriConfig))
                    {
                        return System.IO.Path.Combine(uriConfig, DirectoryPrefix, versionno, fileName)
                            .Replace('\\', '/');
                    }*/

                    return null;
                }
            }
        }


        /// <summary>
        /// 持久化文件片段相关路径信息
        /// </summary>
        public static class FileFragment
        {
            /// <summary>
            /// 获取持久化文件片段根目录路径（绝对路径）
            /// </summary>
            public static string GetRootDirectoryFullPath()
            {
                return System.IO.Path.Combine(Application.persistentDataPath, "PersistFileFragments")
                    .Replace('\\', '/');
            }
        }

        public static class Json
        {
            /// <summary>
            /// 获取JSON根目录的相对路径
            /// </summary>
            /// <returns></returns>
            public static string GetRootDirectoryRelativePath()
            {
                return "Assets/LuaScripts/Config";
            }

            /// <summary>
            /// 获取JSON根目录的绝对路径
            /// 编辑器工具类
            /// </summary>
            /// <returns></returns>
            public static string GetRootDirectoryFullPath()
            {
                return AorTxt.Format("{0}/{1}", Application.dataPath, "LuaScripts/Config");
            }
        }

        /// <summary>
        /// Lua脚本相关路径信息
        /// </summary>
        public static class LuaScript
        {
            public static class Framework
            {
                /// <summary>
                /// 获取Lua脚本根目录的相对路径
                /// </summary>
                /// <param name="isEditorTool">是否为编辑器工具调用</param>
                /// <returns></returns>
                public static string GetRootDirectoryRelativePath(bool isEditorTool = false)
                {
                    if (isEditorTool)
                    {
                        return "Assets/Framework/LuaScripts";
                    }
                    else
                    {
                        return "Assets/Framework/LuaScripts";
                    }
                }

                /// <summary>
                /// 获取Lua脚本根目录的绝对路径
                /// </summary>
                /// <param name="isEditorTool">是否为编辑器工具调用</param>
                /// <returns></returns>
                public static string GetRootDirectoryFullPath(bool isEditorTool = false)
                {
                    if (isEditorTool)
                    {
                        return AorTxt.Format("{0}/{1}", Application.dataPath, "Game/LuaScripts/XLua");
                    }
                    else
                    {
                        return AorTxt.Format("{0}/{1}", Application.dataPath, "Game/LuaScripts/XLua");
                    }
                }
            }

            public static class Game
            {
                /// <summary>
                /// 获取Lua脚本根目录的相对路径
                /// </summary>
                /// <param name="isEditorTool">是否为编辑器工具调用</param>
                /// <returns></returns>
                public static string GetRootDirectoryRelativePath(bool isEditorTool = false)
                {
                    if (isEditorTool)
                    {
                        return "Assets/Game/LuaScripts";
                    }
                    else
                    {
                        return "Assets/Game/LuaScripts";
                    }
                }

                /// <summary>
                /// 获取Lua脚本根目录的绝对路径
                /// </summary>
                /// <param name="isEditorTool">是否为编辑器工具调用</param>
                /// <returns></returns>
                public static string GetRootDirectoryFullPath(bool isEditorTool = false)
                {
                    if (isEditorTool)
                    {
                        return AorTxt.Format("{0}/{1}", Application.dataPath, "Game/LuaScripts");
                    }
                    else
                    {
                        return AorTxt.Format("{0}/{1}", Application.dataPath, "Game/LuaScripts");
                    }
                }
            }
        }
        /// <summary>
        /// Prefab相关路径信息
        /// </summary>
        public static class Prefab
        {
            /// <summary>
            /// 获取Prefab根目录的相对路径
            /// </summary>
            /// <returns></returns>
            public static string GetRootDirectoryRelativePath()
            {
                return "Assets/Res/Prefabs";
            }

            /// <summary>
            /// 获取Prefab根目录的绝对路径
            /// </summary>
            /// <returns></returns>
            public static string GetRootDirectoryFullPath()
            {
                return AorTxt.Format("{0}/{1}", Application.dataPath, "Res/Prefabs");
            }

            /// <summary>
            /// 获取Solar框架Prefab根目录的相对路径
            /// </summary>
            /// <returns></returns>
            public static string GetFrameworkRootDirectoryRelativePath()
            {
                return "Assets/Res/Prefabs";
            }

            /// <summary>
            /// 获取Solar框架Prefab根目录的绝对路径
            /// </summary>
            /// <returns></returns>
            public static string GetFrameworkRootDirectoryFullPath()
            {
                return AorTxt.Format("{0}/{1}", Application.dataPath, "Res/Prefabs");
            }

        }
        
        /// <summary>
        /// IDE调试相关路径信息
        /// </summary>
        public static class IDEDebugger
        {
            /// <summary>
            /// 获取IDE的Lua调试开关在Library目录下配置文件的绝对路径
            /// 编辑器工具类
            /// </summary>
            /// <returns></returns>
            public static string GetLuaDebugModeLibraryConfigFileFullPath()
            {
                return AorTxt.Format("{0}/{1}", System.IO.Path.GetFullPath("."), "Library/LuaDebugMode.dat").Replace("\\", "/");
            }

        }
    }
}