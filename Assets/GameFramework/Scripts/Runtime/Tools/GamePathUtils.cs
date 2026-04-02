using System.IO;
using UnityEngine;

namespace Honor.Runtime
{
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
                /// <summary>
                /// 获取App商店的uri
                /// </summary>
                public static string GetStoreUri()
                {
                    return GameMainRoot.Config.GetString("StoreUrl", true);
                }
            }
        }

        /// <summary>
        /// AssetBundle相关路径信息
        /// </summary>
        public static class AB
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
            public static string GetExcelRootDirectoryFullPath()
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
                return AorTxt.Format("{0}/{1}", Application.dataPath, "../Docs/Designs/Excels/ABConfigs/ABConfigs.xlsm");
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
                    return AorTxt.Format("{0}/../AssetBundles/{1}/{2}.{3}", Application.dataPath, platformName, appVersion, resMinor).Replace('\\', '/');
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
                    return AorTxt.Format("{0}/../AssetBundles/{1}/{2}.{3}/{4}", Application.dataPath, platformName, appVersion, resMinor, VersionFileName).Replace('\\', '/');
                }
                
                /// <summary>
                /// 获取生成的AB根目录下version文件的绝对路径
                /// </summary>
                /// <param name="platformName">平台名称</param>
                /// <returns></returns>
                public static string GetVersionFileFullPath(string platformName, string version)
                {
                    return $"{Application.dataPath}/../AssetBundles/{platformName}/{version}/{VersionFileName}".Replace('\\', '/');
                }

                /// <summary>
                /// 获取生成的AB文件的绝对路径
                /// </summary>
                /// <param name="platformName">平台名称</param>
                /// <param name="appVersion">APP版本号</param>
                /// <param name="resMinor">资源小版本号</param>
                /// <param name="formatPath">ab的格式化路径（从asset/开始的全小写路径信息）</param>
                /// <returns></returns>
                public static string GetFileFullPath(string platformName, string appVersion, int resMinor, string formatPath)
                {
                    return System.IO.Path.Combine(GetRootDirectoryFullPath(platformName, appVersion, resMinor), formatPath).Replace('\\', '/');
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
                        return System.IO.Path.Combine(Application.persistentDataPath, DirectoryPrefix).Replace('\\', '/');
                    }
                    else
                    {
                        return System.IO.Path.Combine(Application.persistentDataPath, AorTxt.Format("AssetBundles/{0}", platformName)).Replace('\\', '/');
                    }
                }

                /// <summary>
                /// 获取可写路径下AB根目录下version文件的绝对路径
                /// </summary>
                /// <returns></returns>
                public static string GetVersionFileFullPath()
                {
                    return System.IO.Path.Combine(Application.persistentDataPath, DirectoryPrefix, VersionFileName).Replace('\\', '/');
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
                    return System.IO.Path.Combine(Application.persistentDataPath, DirectoryPrefix, "___Tmp___").Replace('\\', '/');
                }

                /// <summary>
                /// 获取可写路径下临时AB根目录下version文件的绝对路径
                /// </summary>
                /// <returns></returns>
                public static string GetVersionFileFullPath()
                {
                    return System.IO.Path.Combine(Application.persistentDataPath, DirectoryPrefix, "___Tmp___", VersionFileName).Replace('\\', '/');
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
                        return System.IO.Path.Combine(Application.streamingAssetsPath, DirectoryPrefix).Replace('\\', '/');
                    }
                    else
                    {
                        return System.IO.Path.Combine(Application.streamingAssetsPath, AorTxt.Format("AssetBundles/{0}", platformName)).Replace('\\', '/');
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
                    return AorTxt.Format("{0}{1}{2}{3}{4}{5}", "file://", Application.dataPath, "/StreamingAssets/", DirectoryPrefix, "/", VersionFileName);
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
                    // string uriConfig = GameMainRoot.Config.GetHotfixUrl(); //Root.Config.GetString("HotfixUrl", true);
                    // if (!string.IsNullOrEmpty(uriConfig))
                    // {
                    //     return System.IO.Path.Combine(uriConfig, DirectoryPrefix, versionno, fileName).Replace('\\', '/');
                    // }
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
                return System.IO.Path.Combine(Application.persistentDataPath, "PersistFileFragments").Replace('\\', '/');
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
                        bool luacMode = GameMainRoot.Launcher != null ? GameMainRoot.Launcher.LuacMode : GameComponentsGroup.GetComponent<LauncherComponent>().LuacMode;
                        if (luacMode)
                        {
                            return "Assets/Framework/LuacScripts";
                        }
                        else
                        {
                            return "Assets/Framework/LuaScripts";
                        }
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
                        bool luacMode = GameMainRoot.Launcher != null ? GameMainRoot.Launcher.LuacMode : GameComponentsGroup.GetComponent<LauncherComponent>().LuacMode;
                        if (luacMode)
                        {
                            return AorTxt.Format("{0}/{1}", Application.dataPath, "Game/LuaScripts/XLua");
                        }
                        else
                        {
                            return AorTxt.Format("{0}/{1}", Application.dataPath, "Game/LuaScripts/XLua");
                        }
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
                        return "Assets/LuaScripts";
                    }
                    else
                    {
                        bool luacMode = GameMainRoot.Launcher != null ? GameMainRoot.Launcher.LuacMode : GameComponentsGroup.GetComponent<LauncherComponent>().LuacMode;
                        if (luacMode)
                        {
                            return "Assets/LuacScripts";
                        }
                        else
                        {
                            return "Assets/LuaScripts";
                        }
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
                        return AorTxt.Format("{0}/{1}", Application.dataPath, "LuaScripts");
                    }
                    else
                    {
                        bool luacMode = GameMainRoot.Launcher != null ? GameMainRoot.Launcher.LuacMode : GameComponentsGroup.GetComponent<LauncherComponent>().LuacMode;
                        if (luacMode)
                        {
                            return AorTxt.Format("{0}/{1}", Application.dataPath, "LuaScripts");
                        }
                        else
                        {
                            return AorTxt.Format("{0}/{1}", Application.dataPath, "LuaScripts");
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Json相关路径信息
        /// </summary>
        public static class Json
        {
            /// <summary>
            /// 获取JSON根目录的相对路径
            /// </summary>
            /// <returns></returns>
            public static string GetRootDirectoryRelativePath()
            {
                return "Assets/LuaScripts/Config/LuaJson";
            }

            /// <summary>
            /// 获取JSON根目录的绝对路径
            /// 编辑器工具类
            /// </summary>
            /// <returns></returns>
            public static string GetRootDirectoryFullPath()
            {
                return AorTxt.Format("{0}/{1}", Application.dataPath, "LuaScripts/Config/LuaJson");
            }

        }

        /// <summary>
        /// Font相关路径信息
        /// </summary>
        public static class Font
        {
            /// <summary>
            /// 获取Font根目录的相对路径
            /// </summary>
            /// <returns></returns>
            public static string GetRootDirectoryRelativePath()
            {
                return "Assets/Res/Fonts";
            }

            /// <summary>
            /// 获取Font根目录的绝对路径
            /// </summary>
            /// <returns></returns>
            public static string GetRootDirectoryFullPath()
            {
                return AorTxt.Format("{0}/{1}", Application.dataPath, "Res/Fonts");
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
            /// 获取Honor框架Prefab根目录的相对路径
            /// </summary>
            /// <returns></returns>
            public static string GetFrameworkRootDirectoryRelativePath()
            {
                return "Assets/Res/Prefabs";
            }

            /// <summary>
            /// 获取Honor框架Prefab根目录的绝对路径
            /// </summary>
            /// <returns></returns>
            public static string GetFrameworkRootDirectoryFullPath()
            {
                return AorTxt.Format("{0}/{1}", Application.dataPath, "Res/Prefabs");
            }

        }

        /// <summary>
        /// Texture相关路径信息
        /// </summary>
        public static class Texture
        {
            /// <summary>
            /// 获取Texture根目录的相对路径
            /// </summary>
            /// <returns></returns>
            public static string GetRootDirectoryRelativePath()
            {
                return "Assets/Res/Textures";
            }

            /// <summary>
            /// 获取Texture根目录的绝对路径
            /// </summary>
            /// <returns></returns>
            public static string GetRootDirectoryFullPath()
            {
                return AorTxt.Format("{0}/{1}", Application.dataPath, "Res/Textures");
            }

        }

        /// <summary>
        /// PicsForAtlas碎图相关路径信息
        /// </summary>
        public static class PicsForAtlas
        {
            /// <summary>
            /// 获取PicsForAtlas碎图根目录的相对路径
            /// </summary>
            /// <returns></returns>
            public static string GetRootDirectoryRelativePath()
            {
                return "Assets/Res/Textures/PicsForAtlas";
            }

            /// <summary>
            /// 获取PicsForAtlas碎图根目录的绝对路径
            /// </summary>
            /// <returns></returns>
            public static string GetRootDirectoryFullPath()
            {
                return AorTxt.Format("{0}/{1}", Application.dataPath, "Res/Textures/PicsForAtlas");
            }

        }

        /// <summary>
        /// Localization相关路径信息
        /// </summary>
        public static class Localization
        {
            /// <summary>
            /// 获取Localizations的Lua脚本根目录的绝对路径
            /// 编辑器工具类
            /// </summary>
            /// <returns></returns>
            public static string GetLuaScriptRootDirectoryFullPath()
            {
                return AorTxt.Format("{0}/{1}", Application.dataPath, "LuaScripts/Game/Localizations");
            }

            /// <summary>
            /// 获取Localizations的Excel文件绝对路径
            /// 编辑器工具类
            /// </summary>
            /// <returns></returns>
            public static string GetExcelFileFullPath()
            {
                return AorTxt.Format("{0}/{1}", Application.dataPath, "../Docs/Designs/Excels/Localizations/Localizations.xlsm");
            }

            /// <summary>
            /// 获取LocalizationsDefault的Excel文件绝对路径
            /// 编辑器工具类
            /// </summary>
            /// <returns></returns>
            public static string GetExcelDefaultFileFullPath()
            {
                return AorTxt.Format("{0}/{1}", Application.dataPath, "../Docs/Designs/Excels/Localizations/LocalizationsDefault.xlsm");
            }

            /// <summary>
            /// 获取LocalizationFont的Excel文件绝对路径
            /// 编辑器工具类
            /// </summary>
            /// <returns></returns>
            public static string GetExcelFontFileFullPath()
            {
                return AorTxt.Format("{0}/{1}", Application.dataPath, "../Docs/Designs/Excels/Localizations/LocalizationFonts.xlsm");
            }

            /// <summary>
            /// 获取Localization的Excel根目录的绝对路径
            /// 编辑器工具类
            /// </summary>
            /// <returns></returns>
            public static string GetExcelRootDirectoryFullPath()
            {
                return AorTxt.Format("{0}/{1}", Application.dataPath, "../Docs/Designs/Excels/Localizations");
            }

        }

        /// <summary>
        /// Table相关路径信息
        /// </summary>
        public static class Table
        {
            /// <summary>
            /// 获取Tables的Lua脚本根目录的绝对路径
            /// 编辑器工具类
            /// </summary>
            /// <returns></returns>
            public static string GetLuaScriptRootDirectoryFullPath()
            {
                return AorTxt.Format("{0}/{1}", Application.dataPath, "LuaScripts/Game/Tables");
            }

            /// <summary>
            /// 获取Tables的Excel根目录的绝对路径
            /// 编辑器工具类
            /// </summary>
            /// <returns></returns>
            public static string GetExcelRootDirectoryFullPath()
            {
                return AorTxt.Format("{0}/{1}", Application.dataPath, "../Docs/Designs/Excels/Tables");
            }
        }

        /// <summary>
        /// Proto协议相关路径信息
        /// </summary>
        public static class Proto
        {
            public static class Net
            {
                /// <summary>
                /// 获取NetProto根目录的绝对路径
                /// 编辑器工具类
                /// </summary>
                /// <returns></returns>
                public static string GetRootDirectoryFullPath()
                {
                    return AorTxt.Format("{0}/{1}", Application.dataPath, "../Docs/Programs/NetProtos");
                }

                /// <summary>
                /// 获取Net-pb的Lua脚本根目录的绝对路径
                /// 编辑器工具类
                /// </summary>
                /// <returns></returns>
                public static string GetLuaScriptRootDirectoryFullPath()
                {
                    return AorTxt.Format("{0}/{1}", Application.dataPath, "LuaScripts/Game/RScripts/NetCmds/Protos");
                }

                /// <summary>
                /// 获取Net-Definition的Lua脚本目录的绝对路径
                /// 编辑器工具类
                /// </summary>
                /// <returns></returns>
                public static string GetLuaScriptDeclarationsDirectoryFullPath()
                {
                    return AorTxt.Format("{0}/{1}", Application.dataPath, "LuaScripts/Game/RScripts/NetCmds/Declarations");
                }
            }

            public static class Save
            {
                /// <summary>
                /// 获取SaveProto根目录的绝对路径
                /// 编辑器工具类
                /// </summary>
                /// <returns></returns>
                public static string GetRootDirectoryFullPath()
                {
                    return AorTxt.Format("{0}/{1}", Application.dataPath, "../Docs/ProgramConfig/ProtoPB");
                }

                /// <summary>
                /// 获取Save-Proto的Lua脚本根目录的绝对路径
                /// 编辑器工具类
                /// </summary>
                /// <returns></returns>
                public static string GetLuaScriptProtosDirectoryFullPath()
                {
                    return AorTxt.Format("{0}/{1}", Application.dataPath, "LuaScripts/Honor/PB/ProtoDataPb");
                }

                /// <summary>
                /// 获取Save-Definition的Lua脚本目录的绝对路径
                /// 编辑器工具类
                /// </summary>
                /// <returns></returns>
                public static string GetLuaScriptDeclarationsDirectoryFullPath()
                {
                    return AorTxt.Format("{0}/{1}", Application.dataPath, "LuaScripts/Honor/PB/Declarations");
                }

            }
        }

        /// <summary>
        /// Network相关路径信息
        /// </summary>
        public static class Network
        {
            /// <summary>
            /// 获取Networks的Excel根目录的绝对路径
            /// 编辑器工具类
            /// </summary>
            /// <returns></returns>
            public static string GetExcelRootDirectoryFullPath()
            {
                return AorTxt.Format("{0}/{1}", Application.dataPath, "../Docs/Designs/Excels/Networks");
            }

            /// <summary>
            /// 获取Networks的Excel文件的绝对路径
            /// 编辑器工具类
            /// </summary>
            /// <returns></returns>
            public static string GetExcelFileFullPath()
            {
                return AorTxt.Format("{0}/{1}", Application.dataPath, "../Docs/Designs/Excels/Networks/NetCmds.xlsm");
            }

            /// <summary>
            /// 获取Network-Cmd的Lua脚本根目录的绝对路径
            /// 编辑器工具类
            /// </summary>
            /// <returns></returns>
            public static string GetLuaScriptRootDirectoryFullPath()
            {
                return AorTxt.Format("{0}/{1}", Application.dataPath, "LuaScripts/Game/RScripts/NetCmds");
            }

        }

        /// <summary>
        /// Save相关路径信息
        /// </summary>
        public static class Save
        {
            /// <summary>
            /// 获取Save的Lua脚本根目录的绝对路径
            /// 编辑器工具类
            /// </summary>
            /// <returns></returns>
            public static string GetLuaScriptRootDirectoryFullPath()
            {
                return AorTxt.Format("{0}/{1}", Application.dataPath, "LuaScripts/Honor/PB/ProtoDataPb");
            }

        }

        /// <summary>
        /// Config相关路径信息
        /// </summary>
        public static class Config
        {
            /// <summary>
            /// 获取Configs的Excel根目录的绝对路径
            /// 编辑器工具类
            /// </summary>
            /// <returns></returns>
            public static string GetExcelRootDirectoryFullPath()
            {
                return AorTxt.Format("{0}/{1}", Application.dataPath, "../Docs/Designs/Excels/Configs");
            }

            /// <summary>
            /// 获取Configs的Excel文件的绝对路径
            /// 编辑器工具类
            /// </summary>
            /// <returns></returns>
            public static string GetExcelFileFullPath()
            {
                return AorTxt.Format("{0}/{1}", Application.dataPath, "../Docs/Designs/Excels/Configs/Configs.xlsm");
            }
        }

        /// <summary>
        /// UI相关路径信息
        /// </summary>
        public static class UI
        {
            /// <summary>
            /// 获取UI的Excel根目录的绝对路径
            /// 编辑器工具类
            /// </summary>
            /// <returns></returns>
            public static string GetExcelRootDirectoryFullPath()
            {
                return AorTxt.Format("{0}/{1}", Application.dataPath, "../Docs/Designs/Excels/UIs");
            }

            /// <summary>
            /// 获取UIs的Excel文件的绝对路径
            /// 编辑器工具类
            /// </summary>
            /// <returns></returns>
            public static string GetExcelFileFullPath()
            {
                return AorTxt.Format("{0}/{1}", Application.dataPath, "../Docs/Designs/Excels/UIs/UIConfigs.xlsm");
            }

            /// <summary>
            /// 获取UIs的Lua脚本根目录的绝对路径
            /// 编辑器工具类
            /// </summary>
            /// <returns></returns>
            public static string GetLuaScriptRootDirectoryFullPath()
            {
                return AorTxt.Format("{0}/{1}", Application.dataPath, "LuaScripts/Game/UIScripts/UIs");
            }

            /// <summary>
            /// 获取UI组件库根目录的绝对路径
            /// </summary>
            /// <returns></returns>
            public static string GetComponentLibRootDirectoryFullPath()
            {
                return AorTxt.Format("{0}/{1}", Application.dataPath, "Res/Prefabs/UILibs");
            }
        }

        /// <summary>
        /// Sound相关路径信息
        /// </summary>
        public static class Sound
        {
            /// <summary>
            /// 获取Sounds的Excel根目录的绝对路径
            /// 编辑器工具类
            /// </summary>
            /// <returns></returns>
            public static string GetExcelRootDirectoryFullPath()
            {
                return AorTxt.Format("{0}/{1}", Application.dataPath, "../Docs/Designs/Excels/Sounds");
            }

            /// <summary>
            /// 获取Sounds的Excel文件的绝对路径
            /// 编辑器工具类
            /// </summary>
            /// <returns></returns>
            public static string GetExcelFileFullPath()
            {
                return AorTxt.Format("{0}/{1}", Application.dataPath, "../Docs/Designs/Excels/Sounds/Sounds.xlsm");
            }
        }

        /// <summary>
        /// Vibrate相关路径信息
        /// </summary>
        public static class Vibrate
        {
            /// <summary>
            /// 获取Vibrates的Excel根目录的绝对路径
            /// 编辑器工具类
            /// </summary>
            /// <returns></returns>
            public static string GetExcelRootDirectoryFullPath()
            {
                return AorTxt.Format("{0}/{1}", Application.dataPath, "../Docs/Designs/Excels/Vibrates");
            }

            /// <summary>
            /// 获取Vibrates的Excel文件的绝对路径
            /// 编辑器工具类
            /// </summary>
            /// <returns></returns>
            public static string GetExcelFileFullPath()
            {
                return AorTxt.Format("{0}/{1}", Application.dataPath, "../Docs/Designs/Excels/Vibrates/Vibrates.xlsm");
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

        /// <summary>
        /// Debugger相关路径信息
        /// </summary>
        public static class Debugger
        {
            /// <summary>
            /// Debugger在Library目录下配置文件的绝对路径
            /// 编辑器工具类
            /// </summary>
            /// <returns></returns>
            public static string GetLibraryAABToolsConfigFileFullPath()
            {
                return AorTxt.Format("{0}/{1}", System.IO.Path.GetFullPath("."), "Library/AABTools.dat").Replace("\\", "/");
            }

            /// <summary>
            /// Debugger在Library目录下生成的日志根目录的绝对路径
            /// 编辑器工具类
            /// </summary>
            /// <returns></returns>
            public static string GetLibraryLogsRootDirectoryFullPath()
            {
                return AorTxt.Format("{0}/{1}", System.IO.Path.GetFullPath("."), "Library/LogFiles").Replace("\\", "/");
            }
        }

        /// <summary>
        /// Purchases相关路径信息
        /// </summary>
        public static class Purchase
        {
            /// <summary>
            /// 获取Purchases的Excel根目录的绝对路径
            /// 编辑器工具类
            /// </summary>
            /// <returns></returns>
            public static string GetExcelRootDirectoryFullPath()
            {
                return AorTxt.Format("{0}/{1}", Application.dataPath, "../Docs/Designs/Excels/Purchases");
            }

            /// <summary>
            /// 获取Purchases的Excel文件的绝对路径
            /// 编辑器工具类
            /// </summary>
            /// <returns></returns>
            public static string GetExcelFileFullPath()
            {
                return AorTxt.Format("{0}/{1}", Application.dataPath, "../Docs/Designs/Excels/Purchases/Purchases.xlsm");
            }
        }

        /// <summary>
        /// Track相关路径信息
        /// </summary>
        public static class Track
        {
            /// <summary>
            /// 获取Tracks的Excel根目录的绝对路径
            /// 编辑器工具类
            /// </summary>
            /// <returns></returns>
            public static string GetExcelRootDirectoryFullPath()
            {
                return AorTxt.Format("{0}/{1}", Application.dataPath, "../Docs/Designs/Excels/Tracks");
            }

            /// <summary>
            /// 获取Tracks的Excel文件的绝对路径
            /// 编辑器工具类
            /// </summary>
            /// <returns></returns>
            public static string GetExcelFileFullPath()
            {
                return AorTxt.Format("{0}/{1}", Application.dataPath, "../Docs/Designs/Excels/Tracks/Tracks.xlsm");
            }
        }

        /// <summary>
        /// 缓存的包名路径信息
        /// </summary>
        public static class CachedPackageInfo
        {
            /// <summary>
            /// 获取缓存的包名信息在ProjectSettings目录下配置文件的绝对路径
            /// 编辑器工具类
            /// </summary>
            /// <returns></returns>
            public static string GetCachedPackageInfoProjectSettingsConfigFileFullPath()
            {
                return AorTxt.Format("{0}/{1}", System.IO.Path.GetFullPath("."), "ProjectSettings/CachedPackageInfo.dat").Replace("\\", "/");
            }

        }
        /// <summary>
        /// Project相关路径信息
        /// </summary>
        public static class Project
        {
            /// <summary>
            /// 获取Project根目录的绝对路径
            /// </summary>
            /// <returns></returns>
            public static string GetRootDirectoryFullPath()
            {
                return AorTxt.Format("{0}/../", Application.dataPath).Replace('\\', '/');
            }

            /// <summary>
            /// 获取VS-Projrect文件的绝对路径
            /// </summary>
            /// <returns></returns>
            public static string GetVSProjectFileFullPath()
            {
                string[] fileFullPaths = Directory.GetFiles(GetRootDirectoryFullPath(), "*.sln", SearchOption.AllDirectories);
                if (fileFullPaths != null && fileFullPaths.Length > 0)
                {
                    return fileFullPaths[0];
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// Native相关路径信息
        /// </summary>
        public static class Native
        {
            /// <summary>
            /// iOS原生工程相关路径
            /// </summary>
            public static class iOS
            {
                /// <summary>
                /// 获取Native工程根目录的绝对路径
                /// 编辑器工具类
                /// </summary>
                /// <returns></returns>
                public static string GetProjectRootDirectoryFullPath()
                {
                    return AorTxt.Format("{0}/{1}", Application.dataPath, "../Natives/iOS");
                }
            }

            /// <summary>
            /// Android原生工程相关路径
            /// </summary>
            public static class Android
            {
                /// <summary>
                /// 获取Native工程根目录的绝对路径
                /// 编辑器工具类
                /// </summary>
                /// <returns></returns>
                public static string GetProjectRootDirectoryFullPath()
                {
                    return AorTxt.Format("{0}/{1}", Application.dataPath, "../Natives/Android");
                }

                /// <summary>
                /// 获取Native签名文件的绝对路径
                /// 编辑器工具类
                /// </summary>
                /// <returns></returns>
                public static string GetSignatureDirectoryFullPath()
                {
                    return AorTxt.Format("{0}/{1}", Application.dataPath, "../Docs/Programs/Certificates/Android");
                }
            }

        }

        /// <summary>
        /// Tool相关路径信息
        /// </summary>
        public static class Tool
        {
            /// <summary>
            /// 获取Tool根目录的绝对路径
            /// </summary>
            /// <returns></returns>
            public static string GetRootDirectoryFullPath()
            {
                return AorTxt.Format("{0}/../Tools", Application.dataPath).Replace('\\', '/');
            }

            /// <summary>
            /// 获取BMFont工具目录的绝对路径
            /// </summary>
            /// <returns></returns>
            public static string GetBMFontDirectoryFullPath()
            {
                return AorTxt.Format("{0}/{1}", GetRootDirectoryFullPath(), "BMFont");
            }
        }

        /// <summary>
        /// Editor编辑器相关路径信息
        /// </summary>
        public static class Editor
        {
            /// <summary>
            /// 获取ProjectSetting目录的绝对路径
            /// </summary>
            /// <returns></returns>
            public static string GetProjectSettingDirectoryFullPath()
            {
                return AorTxt.Format("{0}/../ProjectSettings", Application.dataPath).Replace('\\', '/');
            }

            /// <summary>
            /// 资源信息定义路径信息
            /// </summary>
            public static class ResDef
            {
                /// <summary>
                /// 获取资源信息定义Lua脚本绝对路径
                /// </summary>
                /// <returns></returns>
                public static string GetResDefLuaFullPath()
                {
                    return AorTxt.Format("{0}/{1}", LuaScript.Game.GetRootDirectoryFullPath(true) + "/RScripts", "ResDefs.lua.txt");
                }
                
                /// <summary>
                /// 获取资源信息定义Lua脚本相对路径
                /// </summary>
                public static string LuaFolderPath = $"Assets/Game/LuaScripts/RScripts/ResDefs";

                /// <summary>
                /// 获取资源信息导出工具配置文件绝对路径
                /// </summary>
                /// <returns></returns>
                public static string GetResDefExportWindowsSettingsFullPath()
                {
                    return AorTxt.Format("{0}/{1}", GetProjectSettingDirectoryFullPath(), "HonorResDefExportSettings.json");
                }
            }

            /// <summary>
            /// Hierarchy展开规则设置工具路径信息
            /// </summary>
            public static class HierarchyExpandSettings
            {
                /// <summary>
                /// 获取Hierarchy展开规则设置工具的配置文件绝对路径
                /// </summary>
                /// <returns></returns>
                public static string GetHierarchyExpandSettingsFullPath()
                {
                    return AorTxt.Format("{0}/{1}", GetProjectSettingDirectoryFullPath(), "HonorHierarchyExpandSettings.json");
                }
            }

            /// <summary>
            /// 本地化多语言字符集导出配置
            /// </summary>
            public static class LocalizationFontTMPExportSettings
            {
                /// <summary>
                /// 本地化多语言字符集导出配置文件路径
                /// </summary>
                /// <returns></returns>
                public static string GetLocalizationFontTMPCharsExportSettingsFullPath()
                {
                    return AorTxt.Format("{0}/../ProjectSettings/HonorFontTMPCharsExportSettings.json", Application.dataPath);
                }
            }

            /// <summary>
            /// ABGeneration相关路径信息
            /// </summary>
            public static class ABGeneration
            {
                /// <summary>
                /// 获取AB生成配置文件的绝对路径
                /// </summary>
                public static string GetABGenerationSettingsFullPath()
                {
                    return AorTxt.Format("{0}/{1}", GetProjectSettingDirectoryFullPath(), "HotfixABSettings.json");
                }
            }

            /// <summary>
            /// ChatGPT存档路径信息
            /// </summary>
            public static class ChatGPT
            {
                /// <summary>
                /// 获取ChatGPT在Library目录下存档配置文件的绝对路径
                /// 编辑器工具类
                /// </summary>
                /// <returns></returns>
                public static string GetLibrarySaveFileFullPath()
                {
                    return $"{System.IO.Path.GetFullPath(".")}/Library/ChatGPTConfig.dat".Replace("\\", "/");
                }

                /// <summary>
                /// 获取ChatGPT在Library目录下历史对话记录文件的绝对路径
                /// 编辑器工具类
                /// </summary>
                /// <returns></returns>
                public static string GetLibraryChatHistoryFileFullPath()
                {
                    return $"{System.IO.Path.GetFullPath(".")}/Library/ChatGPTChatHistory.dat".Replace("\\", "/");
                }

                /// <summary>
                /// 获取ChatGPT在Library目录下历史Code迭代记录文件的绝对路径
                /// 编辑器工具类
                /// </summary>
                /// <returns></returns>
                public static string GetLibraryCodeHistoryFileFullPath()
                {
                    return $"{System.IO.Path.GetFullPath(".")}/Library/ChatGPTCodeHistory.dat".Replace("\\", "/");
                }

                /// <summary>
                /// 获取ChatGPT在Library目录下历史图片创建记录文件的绝对路径
                /// 编辑器工具类
                /// </summary>
                /// <returns></returns>
                public static string GetLibraryImageCreateHistoryFileFullPath()
                {
                    return $"{System.IO.Path.GetFullPath(".")}/Library/ChatGPTImageCreateHistory.dat".Replace("\\", "/");
                }

                /// <summary>
                /// 获取ChatGPT在Library目录下历史图片编辑记录文件的绝对路径
                /// 编辑器工具类
                /// </summary>
                /// <returns></returns>
                public static string GetLibraryImageEditHistoryFileFullPath()
                {
                    return $"{System.IO.Path.GetFullPath(".")}/Library/ChatGPTImageEditHistory.dat".Replace("\\", "/");
                }

                /// <summary>
                /// 获取ChatGPT在Library目录下历史图片变种记录文件的绝对路径
                /// 编辑器工具类
                /// </summary>
                /// <returns></returns>
                public static string GetLibraryImageVariationHistoryFileFullPath()
                {
                    return $"{System.IO.Path.GetFullPath(".")}/Library/ChatGPTImageVariationHistory.dat".Replace("\\", "/");
                }
            }

            /// <summary>
            /// CDN存档路径信息
            /// </summary>
            public static class CDN
            {
                /// <summary>
                /// 获取CDN在Library目录下存档配置文件的绝对路径
                /// 编辑器工具类
                /// </summary>
                /// <returns></returns>
                public static string GetCDNEditorWindowsSettingsFullPath()
                {
                    return $"{GetProjectSettingDirectoryFullPath()}/HonorCDNSettings.json";
                }

            }

            /// <summary>
            /// AssetBundleBrowser存档路径信息
            /// </summary>
            public static class AssetBundleBrowser
            {
                /// <summary>
                /// 获取AssetBundleBrowser在ProjectSettings目录下存档配置文件的绝对路径
                /// 编辑器工具类
                /// </summary>
                /// <returns></returns>
                public static string GetAssetBundleBrowserEditorWindowsSettingsFullPath()
                {
                    return $"{GetProjectSettingDirectoryFullPath()}/HonorAssetBundleBrowserSettings.json";
                }

            }
            
            /// <summary>
            /// Lua云脚本存档路径信息
            /// </summary>
            public static class LuaCloudScript
            {
                /// <summary>
                /// 获取Lua云脚本在ProjectSetting目录下存档配置文件的绝对路径
                /// </summary>
                public static string ProjectSettingFileFullPath = $"{GetProjectSettingDirectoryFullPath()}/LuaCloudScriptSettings.json";
                
                /// <summary>
                /// 获取Lua脚本在Library目录下文件的绝对路径
                /// </summary>
                public static string LibraryLocalLuaFileFullPath = AorTxt.Format("{0}/../Library/LuaScript.lua.txt", Application.dataPath).Replace('\\', '/');
            }

        }
        /// <summary>
        /// 获取图片
        /// </summary>
        public static class GetImage
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="_spriteName">需要加载的名字</param>
            /// <returns></returns>
            public static Sprite GetImageByName(string _spriteName)
            {

                return null;
            }

        }
    }

}


