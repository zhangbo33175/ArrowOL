/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  GamePathUtils.cs
 * author:    云毅
 * created:   2026
 * descrip:   游戏全局路径工具类 - 统一管理项目所有路径规则，
 *            自动适配多平台，路径统一使用 '/' 分隔符
 ***************************************************************/
using System.IO;
using UnityEngine;

namespace Honor.Runtime
{
    //=========================================================================
    // 游戏全局路径工具类
    //=========================================================================
    /// <summary>
    /// 游戏全局路径工具类
    /// 统一管理项目中所有路径规则：AB包、Lua脚本、资源、配置表、协议、原生工程、编辑器工具等
    /// 自动适配 Android / iOS / WebGL / Editor 平台，路径统一使用 '/' 分隔符
    /// </summary>
    public static class GamePathUtils
    {
        /// <summary>
        /// 当前运行平台名称（自动根据宏定义切换）
        /// </summary>
#if UNITY_IOS
        public static string PlatformName = "iOS";
#elif UNITY_WEBGL
        public static string PlatformName = "WebGL";
#else
        public static string PlatformName = "Android";
#endif

        #region 应用下载相关路径
        /// <summary>
        /// 应用大版本更新、应用商店下载相关路径
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
                /// <returns>应用商店链接</returns>
                public static string GetStoreUri()
                {
                    return GameMainRoot.Config.GetString("StoreUrl", true);
                }
            }
        }
        #endregion

        #region AssetBundle 相关路径
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
            /// <returns>Excel配置根目录</returns>
            public static string GetExcelRootDirectoryFullPath()
            {
                return AorTxt.Format("{0}/{1}", Application.dataPath, "../Docs/Designs/Excels/ABConfigs");
            }

            /// <summary>
            /// 获取ABConfigs的Excel文件的绝对路径
            /// 编辑器工具类
            /// </summary>
            /// <returns>Excel配置文件路径</returns>
            public static string GetExcelFileFullPath()
            {
                return AorTxt.Format("{0}/{1}", Application.dataPath, "../Docs/Designs/Excels/ABConfigs/ABConfigs.xlsm");
            }

            /// <summary>
            /// 获取灰度version文件名称
            /// </summary>
            /// <param name="appVersion">应用版本号：x.y.z</param>
            /// <returns>灰度版本文件名</returns>
            public static string GetVersionGrayFileName(string appVersion)
            {
                return AorTxt.Format(s_GrayVersionFileName, appVersion);
            }

            #region 服务器打包路径
            /// <summary>
            /// AssetBundle生成路径（用来提交到服务器）
            /// </summary>
            public static class ForServer
            {
                /// <summary>
                /// 获取生成的AB平台根目录的绝对路径
                /// </summary>
                /// <param name="platformName">平台名称</param>
                /// <returns>平台根目录</returns>
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
                /// <returns>AB根目录</returns>
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
                /// <returns>版本文件路径</returns>
                public static string GetVersionFileFullPath(string platformName, string appVersion, int resMinor)
                {
                    return AorTxt.Format("{0}/../AssetBundles/{1}/{2}.{3}/{4}", Application.dataPath, platformName, appVersion, resMinor, VersionFileName).Replace('\\', '/');
                }

                /// <summary>
                /// 获取生成的AB根目录下version文件的绝对路径
                /// </summary>
                /// <param name="platformName">平台名称</param>
                /// <param name="version">版本号</param>
                /// <returns>版本文件路径</returns>
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
                /// <returns>AB文件路径</returns>
                public static string GetFileFullPath(string platformName, string appVersion, int resMinor, string formatPath)
                {
                    return Path.Combine(GetRootDirectoryFullPath(platformName, appVersion, resMinor), formatPath).Replace('\\', '/');
                }
            }
            #endregion

            #region 可读写路径
            /// <summary>
            /// AssetBundle可写路径下相关路径信息
            /// </summary>
            public static class Persistent
            {
                /// <summary>
                /// 获取可写路径下AB根目录的绝对路径
                /// </summary>
                /// <param name="platformName">平台名称</param>
                /// <returns>AB根目录</returns>
                public static string GetRootDirectoryFullPath(string platformName = null)
                {
                    if (string.IsNullOrEmpty(platformName))
                        return Path.Combine(Application.persistentDataPath, DirectoryPrefix).Replace('\\', '/');
                    else
                        return Path.Combine(Application.persistentDataPath, AorTxt.Format("AssetBundles/{0}", platformName)).Replace('\\', '/');
                }

                /// <summary>
                /// 获取可写路径下AB根目录下version文件的绝对路径
                /// </summary>
                /// <returns>版本文件路径</returns>
                public static string GetVersionFileFullPath()
                {
                    return Path.Combine(Application.persistentDataPath, DirectoryPrefix, VersionFileName).Replace('\\', '/');
                }

                /// <summary>
                /// 获取可写路径下AB文件的绝对路径
                /// </summary>
                /// <param name="formatPath">ab的格式化路径（从asset/开始的全小写路径信息）</param>
                /// <returns>AB文件路径</returns>
                public static string GetFileFullPath(string formatPath)
                {
                    return Path.Combine(GetRootDirectoryFullPath(), formatPath).Replace('\\', '/');
                }
            }
            #endregion

            #region 临时可读写路径
            /// <summary>
            /// AssetBundle临时可写路径下相关路径信息
            /// </summary>
            public static class PersistentTmp
            {
                /// <summary>
                /// 获取可写路径下临时AB根目录的绝对路径
                /// </summary>
                /// <returns>临时AB根目录</returns>
                public static string GetRootDirectoryFullPath()
                {
                    return Path.Combine(Application.persistentDataPath, DirectoryPrefix, "___Tmp___").Replace('\\', '/');
                }

                /// <summary>
                /// 获取可写路径下临时AB根目录下version文件的绝对路径
                /// </summary>
                /// <returns>临时版本文件路径</returns>
                public static string GetVersionFileFullPath()
                {
                    return Path.Combine(Application.persistentDataPath, DirectoryPrefix, "___Tmp___", VersionFileName).Replace('\\', '/');
                }

                /// <summary>
                /// 获取可写路径下临时AB文件的绝对路径
                /// </summary>
                /// <param name="formatPath">ab的格式化路径（从asset/开始的全小写路径信息）</param>
                /// <returns>临时AB文件路径</returns>
                public static string GetFileFullPath(string formatPath)
                {
                    return System.IO.Path.Combine(GetRootDirectoryFullPath(), formatPath).Replace('\\', '/');
                }
            }
            #endregion

            #region 缓存路径
            /// <summary>
            /// AssetBundle缓存路径下相关路径信息
            /// </summary>
            public static class Caching
            {
                /// <summary>
                /// 获取缓存路径下AB根目录的绝对路径
                /// </summary>
                /// <returns>缓存根目录</returns>
                public static string GetRootDirectoryFullPath()
                {
                    return System.IO.Path.Combine(UnityEngine.Caching.currentCacheForWriting.path).Replace('\\', '/');
                }

                /// <summary>
                /// 获取缓存路径下AB文件的绝对路径
                /// </summary>
                /// <param name="formatPath">ab的格式化路径（从asset/开始的全小写路径信息）</param>
                /// <returns>缓存AB文件路径</returns>
                public static string GetFileFullPath(string formatPath)
                {
                    return System.IO.Path.Combine(GetRootDirectoryFullPath(), formatPath).Replace('\\', '/');
                }
            }
            #endregion

            #region 只读 StreamingAssets 路径
            /// <summary>
            /// AssetBundle只读路径下相关路径信息
            /// </summary>
            public static class Streaming
            {
                /// <summary>
                /// 获取只读路径下AB根目录的绝对路径
                /// </summary>
                /// <param name="platformName">平台名称</param>
                /// <returns>只读AB根目录</returns>
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
                /// <returns>只读版本文件路径</returns>
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
                /// <returns>只读AB文件路径</returns>
                public static string GetFileFullPath(string formatPath)
                {
                    return System.IO.Path.Combine(GetRootDirectoryFullPath(), formatPath).Replace('\\', '/');
                }
            }
            #endregion

            #region 服务器下载地址
            /// <summary>
            /// 服务器uri相关路径信息
            /// </summary>
            public static class Uri
            {
                /// <summary>
                /// 获取指定热更新资源文件的uri
                /// </summary>
                /// <param name="fileName">uri对应的文件名称（带后缀名）</param>
                /// <param name="versionno">版本号</param>
                /// <returns>热更文件下载地址</returns>
                public static string GetHotfixFileUri(string fileName, string versionno = "")
                {
                    return null;
                }
            }
            #endregion
        }
        #endregion

        #region 文件片段持久化路径
        /// <summary>
        /// 持久化文件片段相关路径信息
        /// </summary>
        public static class FileFragment
        {
            /// <summary>
            /// 获取持久化文件片段根目录路径（绝对路径）
            /// </summary>
            /// <returns>文件片段根目录</returns>
            public static string GetRootDirectoryFullPath()
            {
                return System.IO.Path.Combine(Application.persistentDataPath, "PersistFileFragments").Replace('\\', '/');
            }
        }
        #endregion

        #region Lua 脚本路径
        /// <summary>
        /// Lua脚本相关路径信息
        /// </summary>
        public static class LuaScript
        {
            /// <summary>
            /// 框架Lua脚本
            /// </summary>
            public static class Framework
            {
                /// <summary>
                /// 获取Lua脚本根目录的相对路径
                /// </summary>
                /// <param name="isEditorTool">是否为编辑器工具调用</param>
                /// <returns>相对路径</returns>
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
                /// <returns>绝对路径</returns>
                public static string GetRootDirectoryFullPath(bool isEditorTool = false)
                {
                    if (isEditorTool)
                    {
                        return AorTxt.Format("{0}/{1}", Application.dataPath, "LuaScripts/Game/XLua");
                    }
                    else
                    {
                        bool luacMode = GameMainRoot.Launcher != null ? GameMainRoot.Launcher.LuacMode : GameComponentsGroup.GetComponent<LauncherComponent>().LuacMode;
                        if (luacMode)
                        {
                            return AorTxt.Format("{0}/{1}", Application.dataPath, "LuaScripts/Game/XLua");
                        }
                        else
                        {
                            return AorTxt.Format("{0}/{1}", Application.dataPath, "LuaScripts/Game/XLua");
                        }
                    }
                }
            }

            /// <summary>
            /// 游戏业务Lua脚本
            /// </summary>
            public static class Game
            {
                /// <summary>
                /// 获取Lua脚本根目录的相对路径
                /// </summary>
                /// <param name="isEditorTool">是否为编辑器工具调用</param>
                /// <returns>相对路径</returns>
                public static string GetRootDirectoryRelativePath(bool isEditorTool = false)
                {
                    if (isEditorTool)
                        return "Assets/LuaScripts";
                    
                    bool luacMode = GameMainRoot.Launcher != null ? GameMainRoot.Launcher.LuacMode : GameComponentsGroup.GetComponent<LauncherComponent>().LuacMode;
                    return luacMode ? "Assets/LuacScripts" : "Assets/LuaScripts";
                }

                /// <summary>
                /// 获取Lua脚本根目录的绝对路径
                /// </summary>
                /// <param name="isEditorTool">是否为编辑器工具调用</param>
                /// <returns>绝对路径</returns>
                public static string GetRootDirectoryFullPath(bool isEditorTool = false)
                {
                    if (isEditorTool)
                    {
                       return AorTxt.Format("{0}/{1}", Application.dataPath, "LuaScripts");
                    }
                    bool luacMode = GameMainRoot.Launcher != null ? GameMainRoot.Launcher.LuacMode : GameComponentsGroup.GetComponent<LauncherComponent>().LuacMode;
                    return AorTxt.Format("{0}/{1}", Application.dataPath, "LuaScripts");
                }
            }
        }
        #endregion

        #region JSON 配置路径
        /// <summary>
        /// Json相关路径信息
        /// </summary>
        public static class Json
        {
            /// <summary>
            /// 获取JSON根目录的相对路径
            /// </summary>
            /// <returns>相对路径</returns>
            public static string GetRootDirectoryRelativePath()
            {
                return "Assets/LuaScripts/Config/LuaJson";
            }

            /// <summary>
            /// 获取JSON根目录的绝对路径
            /// 编辑器工具类
            /// </summary>
            /// <returns>绝对路径</returns>
            public static string GetRootDirectoryFullPath()
            {
                return AorTxt.Format("{0}/{1}", Application.dataPath, "LuaScripts/Config/LuaJson");
            }
        }
        #endregion

        #region 字体路径
        /// <summary>
        /// Font相关路径信息
        /// </summary>
        public static class Font
        {
            /// <summary>
            /// 获取Font根目录的相对路径
            /// </summary>
            /// <returns>相对路径</returns>
            public static string GetRootDirectoryRelativePath()
            {
                return "Assets/Res/Fonts";
            }

            /// <summary>
            /// 获取Font根目录的绝对路径
            /// </summary>
            /// <returns>绝对路径</returns>
            public static string GetRootDirectoryFullPath()
            {
                return AorTxt.Format("{0}/{1}", Application.dataPath, "Res/Fonts");
            }
        }
        #endregion

        #region 预制体路径
        /// <summary>
        /// Prefab相关路径信息
        /// </summary>
        public static class Prefab
        {
            /// <summary>
            /// 获取Prefab根目录的相对路径
            /// </summary>
            /// <returns>相对路径</returns>
            public static string GetRootDirectoryRelativePath()
            {
                return "Assets/Res/Prefabs";
            }

            /// <summary>
            /// 获取Prefab根目录的绝对路径
            /// </summary>
            /// <returns>绝对路径</returns>
            public static string GetRootDirectoryFullPath()
            {
                return AorTxt.Format("{0}/{1}", Application.dataPath, "Res/Prefabs");
            }

            /// <summary>
            /// 获取Honor框架Prefab根目录的相对路径
            /// </summary>
            /// <returns>框架预制体相对路径</returns>
            public static string GetFrameworkRootDirectoryRelativePath()
            {
                return "Assets/Res/Prefabs";
            }

            /// <summary>
            /// 获取Honor框架Prefab根目录的绝对路径
            /// </summary>
            /// <returns>框架预制体绝对路径</returns>
            public static string GetFrameworkRootDirectoryFullPath()
            {
                return AorTxt.Format("{0}/{1}", Application.dataPath, "Res/Prefabs");
            }
        }
        #endregion

        #region 图片纹理路径
        /// <summary>
        /// Texture相关路径信息
        /// </summary>
        public static class Texture
        {
            /// <summary>
            /// 获取Texture根目录的相对路径
            /// </summary>
            /// <returns>相对路径</returns>
            public static string GetRootDirectoryRelativePath()
            {
                return "Assets/Res/Textures";
            }

            /// <summary>
            /// 获取Texture根目录的绝对路径
            /// </summary>
            /// <returns>绝对路径</returns>
            public static string GetRootDirectoryFullPath()
            {
                return AorTxt.Format("{0}/{1}", Application.dataPath, "Res/Textures");
            }
        }
        #endregion

        #region 图集碎图路径
        /// <summary>
        /// PicsForAtlas碎图相关路径信息
        /// </summary>
        public static class PicsForAtlas
        {
            /// <summary>
            /// 获取PicsForAtlas碎图根目录的相对路径
            /// </summary>
            /// <returns>相对路径</returns>
            public static string GetRootDirectoryRelativePath()
            {
                return "Assets/Res/Textures/PicsForAtlas";
            }

            /// <summary>
            /// 获取PicsForAtlas碎图根目录的绝对路径
            /// </summary>
            /// <returns>绝对路径</returns>
            public static string GetRootDirectoryFullPath()
            {
                return AorTxt.Format("{0}/{1}", Application.dataPath, "Res/Textures/PicsForAtlas");
            }
        }
        #endregion

        #region 多语言本地化路径
        /// <summary>
        /// Localization相关路径信息
        /// </summary>
        public static class Localization
        {
            /// <summary>
            /// 获取Localizations的Lua脚本根目录的绝对路径
            /// 编辑器工具类
            /// </summary>
            /// <returns>Lua多语言根目录</returns>
            public static string GetLuaScriptRootDirectoryFullPath()
            {
                return AorTxt.Format("{0}/{1}", Application.dataPath, "LuaScripts/Game/Localizations");
            }

            /// <summary>
            /// 获取Localizations的Excel文件绝对路径
            /// 编辑器工具类
            /// </summary>
            /// <returns>多语言Excel路径</returns>
            public static string GetExcelFileFullPath()
            {
                return AorTxt.Format("{0}/{1}", Application.dataPath, "../Docs/Designs/Excels/Localizations/Localizations.xlsm");
            }

            /// <summary>
            /// 获取LocalizationsDefault的Excel文件绝对路径
            /// 编辑器工具类
            /// </summary>
            /// <returns>默认多语言Excel路径</returns>
            public static string GetExcelDefaultFileFullPath()
            {
                return AorTxt.Format("{0}/{1}", Application.dataPath, "../Docs/Designs/Excels/Localizations/LocalizationsDefault.xlsm");
            }

            /// <summary>
            /// 获取LocalizationFont的Excel文件绝对路径
            /// 编辑器工具类
            /// </summary>
            /// <returns>多语言字体Excel路径</returns>
            public static string GetExcelFontFileFullPath()
            {
                return AorTxt.Format("{0}/{1}", Application.dataPath, "../Docs/Designs/Excels/Localizations/LocalizationFonts.xlsm");
            }

            /// <summary>
            /// 获取Localization的Excel根目录的绝对路径
            /// 编辑器工具类
            /// </summary>
            /// <returns>多语言Excel根目录</returns>
            public static string GetExcelRootDirectoryFullPath()
            {
                return AorTxt.Format("{0}/{1}", Application.dataPath, "../Docs/Designs/Excels/Localizations");
            }
        }
        #endregion

        #region 配置表路径
        /// <summary>
        /// Table相关路径信息
        /// </summary>
        public static class Table
        {
            /// <summary>
            /// 获取Tables的Lua脚本根目录的绝对路径
            /// 编辑器工具类
            /// </summary>
            /// <returns>配置表Lua根目录</returns>
            public static string GetLuaScriptRootDirectoryFullPath()
            {
                return AorTxt.Format("{0}/{1}", Application.dataPath, "LuaScripts/Game/Tables");
            }

            /// <summary>
            /// 获取Tables的Excel根目录的绝对路径
            /// 编辑器工具类
            /// </summary>
            /// <returns>配置表Excel根目录</returns>
            public static string GetExcelRootDirectoryFullPath()
            {
                return AorTxt.Format("{0}/{1}", Application.dataPath, "../Docs/Designs/Excels/Tables");
            }
        }
        #endregion

        #region Proto 协议路径
        /// <summary>
        /// Proto协议相关路径信息
        /// </summary>
        public static class Proto
        {
            /// <summary>
            /// 网络协议
            /// </summary>
            public static class Net
            {
                /// <summary>
                /// 获取NetProto根目录的绝对路径
                /// 编辑器工具类
                /// </summary>
                /// <returns>网络协议根目录</returns>
                public static string GetRootDirectoryFullPath()
                {
                    return AorTxt.Format("{0}/{1}", Application.dataPath, "../Docs/Programs/NetProtos");
                }

                /// <summary>
                /// 获取Net-pb的Lua脚本根目录的绝对路径
                /// 编辑器工具类
                /// </summary>
                /// <returns>网络协议Lua目录</returns>
                public static string GetLuaScriptRootDirectoryFullPath()
                {
                    return AorTxt.Format("{0}/{1}", Application.dataPath, "LuaScripts/Game/RScripts/NetCmds/Protos");
                }

                /// <summary>
                /// 获取Net-Definition的Lua脚本目录的绝对路径
                /// 编辑器工具类
                /// </summary>
                /// <returns>网络协议定义目录</returns>
                public static string GetLuaScriptDeclarationsDirectoryFullPath()
                {
                    return AorTxt.Format("{0}/{1}", Application.dataPath, "LuaScripts/Game/RScripts/NetCmds/Declarations");
                }
            }

            /// <summary>
            /// 存档协议
            /// </summary>
            public static class Save
            {
                /// <summary>
                /// 获取SaveProto根目录的绝对路径
                /// 编辑器工具类
                /// </summary>
                /// <returns>存档协议根目录</returns>
                public static string GetRootDirectoryFullPath()
                {
                    return AorTxt.Format("{0}/{1}", Application.dataPath, "../Docs/ProgramConfig/ProtoPB");
                }

                /// <summary>
                /// 获取Save-Proto的Lua脚本根目录的绝对路径
                /// 编辑器工具类
                /// </summary>
                /// <returns>存档协议Lua目录</returns>
                public static string GetLuaScriptProtosDirectoryFullPath()
                {
                    return AorTxt.Format("{0}/{1}", Application.dataPath, "LuaScripts/Honor/PB/ProtoDataPb");
                }

                /// <summary>
                /// 获取Save-Definition的Lua脚本目录的绝对路径
                /// 编辑器工具类
                /// </summary>
                /// <returns>存档协议定义目录</returns>
                public static string GetLuaScriptDeclarationsDirectoryFullPath()
                {
                    return AorTxt.Format("{0}/{1}", Application.dataPath, "LuaScripts/Honor/PB/Declarations");
                }
            }
        }
        #endregion

        #region 网络配置路径
        /// <summary>
        /// Network相关路径信息
        /// </summary>
        public static class Network
        {
            /// <summary>
            /// 获取Networks的Excel根目录的绝对路径
            /// 编辑器工具类
            /// </summary>
            /// <returns>网络配置Excel根目录</returns>
            public static string GetExcelRootDirectoryFullPath()
            {
                return AorTxt.Format("{0}/{1}", Application.dataPath, "../Docs/Designs/Excels/Networks");
            }

            /// <summary>
            /// 获取Networks的Excel文件的绝对路径
            /// 编辑器工具类
            /// </summary>
            /// <returns>网络命令Excel文件</returns>
            public static string GetExcelFileFullPath()
            {
                return AorTxt.Format("{0}/{1}", Application.dataPath, "../Docs/Designs/Excels/Networks/NetCmds.xlsm");
            }

            /// <summary>
            /// 获取Network-Cmd的Lua脚本根目录的绝对路径
            /// 编辑器工具类
            /// </summary>
            /// <returns>网络命令Lua目录</returns>
            public static string GetLuaScriptRootDirectoryFullPath()
            {
                return AorTxt.Format("{0}/{1}", Application.dataPath, "LuaScripts/Game/RScripts/NetCmds");
            }
        }
        #endregion

        #region 存档路径
        /// <summary>
        /// Save相关路径信息
        /// </summary>
        public static class Save
        {
            /// <summary>
            /// 获取Save的Lua脚本根目录的绝对路径
            /// 编辑器工具类
            /// </summary>
            /// <returns>存档Lua目录</returns>
            public static string GetLuaScriptRootDirectoryFullPath()
            {
                return AorTxt.Format("{0}/{1}", Application.dataPath, "LuaScripts/Honor/PB/ProtoDataPb");
            }
        }
        #endregion

        #region 项目配置路径
        /// <summary>
        /// Config相关路径信息
        /// </summary>
        public static class Config
        {
            /// <summary>
            /// 获取Configs的Excel根目录的绝对路径
            /// 编辑器工具类
            /// </summary>
            /// <returns>项目配置Excel根目录</returns>
            public static string GetExcelRootDirectoryFullPath()
            {
                return AorTxt.Format("{0}/{1}", Application.dataPath, "../Docs/Designs/Excels/Configs");
            }

            /// <summary>
            /// 获取Configs的Excel文件的绝对路径
            /// 编辑器工具类
            /// </summary>
            /// <returns>项目配置Excel文件</returns>
            public static string GetExcelFileFullPath()
            {
                return AorTxt.Format("{0}/{1}", Application.dataPath, "../Docs/Designs/Excels/Configs/Configs.xlsm");
            }
        }
        #endregion

        #region UI 路径
        /// <summary>
        /// UI相关路径信息
        /// </summary>
        public static class UI
        {
            /// <summary>
            /// 获取UI的Excel根目录的绝对路径
            /// 编辑器工具类
            /// </summary>
            /// <returns>UI配置Excel根目录</returns>
            public static string GetExcelRootDirectoryFullPath()
            {
                return AorTxt.Format("{0}/{1}", Application.dataPath, "../Docs/Designs/Excels/UIs");
            }

            /// <summary>
            /// 获取UIs的Excel文件的绝对路径
            /// 编辑器工具类
            /// </summary>
            /// <returns>UI配置Excel文件</returns>
            public static string GetExcelFileFullPath()
            {
                return AorTxt.Format("{0}/{1}", Application.dataPath, "../Docs/Designs/Excels/UIs/UIConfigs.xlsm");
            }

            /// <summary>
            /// 获取UIs的Lua脚本根目录的绝对路径
            /// 编辑器工具类
            /// </summary>
            /// <returns>UI逻辑Lua目录</returns>
            public static string GetLuaScriptRootDirectoryFullPath()
            {
                return AorTxt.Format("{0}/{1}", Application.dataPath, "LuaScripts/Game/UIScripts/UIs");
            }

            /// <summary>
            /// 获取UI组件库根目录的绝对路径
            /// </summary>
            /// <returns>UI组件库目录</returns>
            public static string GetComponentLibRootDirectoryFullPath()
            {
                return AorTxt.Format("{0}/{1}", Application.dataPath, "Res/Prefabs/UILibs");
            }
        }
        #endregion

        #region 音效路径
        /// <summary>
        /// Sound相关路径信息
        /// </summary>
        public static class Sound
        {
            /// <summary>
            /// 获取Sounds的Excel根目录的绝对路径
            /// 编辑器工具类
            /// </summary>
            /// <returns>音效Excel根目录</returns>
            public static string GetExcelRootDirectoryFullPath()
            {
                return AorTxt.Format("{0}/{1}", Application.dataPath, "../Docs/Designs/Excels/Sounds");
            }

            /// <summary>
            /// 获取Sounds的Excel文件的绝对路径
            /// 编辑器工具类
            /// </summary>
            /// <returns>音效配置Excel文件</returns>
            public static string GetExcelFileFullPath()
            {
                return AorTxt.Format("{0}/{1}", Application.dataPath, "../Docs/Designs/Excels/Sounds/Sounds.xlsm");
            }
        }
        #endregion

        #region 震动路径
        /// <summary>
        /// Vibrate相关路径信息
        /// </summary>
        public static class Vibrate
        {
            /// <summary>
            /// 获取Vibrates的Excel根目录的绝对路径
            /// 编辑器工具类
            /// </summary>
            /// <returns>震动Excel根目录</returns>
            public static string GetExcelRootDirectoryFullPath()
            {
                return AorTxt.Format("{0}/{1}", Application.dataPath, "../Docs/Designs/Excels/Vibrates");
            }

            /// <summary>
            /// 获取Vibrates的Excel文件的绝对路径
            /// 编辑器工具类
            /// </summary>
            /// <returns>震动配置Excel文件</returns>
            public static string GetExcelFileFullPath()
            {
                return AorTxt.Format("{0}/{1}", Application.dataPath, "../Docs/Designs/Excels/Vibrates/Vibrates.xlsm");
            }
        }
        #endregion

        #region IDE 调试路径
        /// <summary>
        /// IDE调试相关路径信息
        /// </summary>
        public static class IDEDebugger
        {
            /// <summary>
            /// 获取IDE的Lua调试开关在Library目录下配置文件的绝对路径
            /// 编辑器工具类
            /// </summary>
            /// <returns>Lua调试配置文件</returns>
            public static string GetLuaDebugModeLibraryConfigFileFullPath()
            {
                return AorTxt.Format("{0}/{1}", System.IO.Path.GetFullPath("."), "Library/LuaDebugMode.dat").Replace("\\", "/");
            }
        }
        #endregion

        #region 调试器路径
        /// <summary>
        /// Debugger相关路径信息
        /// </summary>
        public static class Debugger
        {
            /// <summary>
            /// Debugger在Library目录下配置文件的绝对路径
            /// 编辑器工具类
            /// </summary>
            /// <returns>AAB工具配置文件</returns>
            public static string GetLibraryAABToolsConfigFileFullPath()
            {
                return AorTxt.Format("{0}/{1}", System.IO.Path.GetFullPath("."), "Library/AABTools.dat").Replace("\\", "/");
            }

            /// <summary>
            /// Debugger在Library目录下生成的日志根目录的绝对路径
            /// 编辑器工具类
            /// </summary>
            /// <returns>日志根目录</returns>
            public static string GetLibraryLogsRootDirectoryFullPath()
            {
                return AorTxt.Format("{0}/{1}", System.IO.Path.GetFullPath("."), "Library/LogFiles").Replace("\\", "/");
            }
        }
        #endregion

        #region 内购路径
        /// <summary>
        /// Purchases相关路径信息
        /// </summary>
        public static class Purchase
        {
            /// <summary>
            /// 获取Purchases的Excel根目录的绝对路径
            /// 编辑器工具类
            /// </summary>
            /// <returns>内购Excel根目录</returns>
            public static string GetExcelRootDirectoryFullPath()
            {
                return AorTxt.Format("{0}/{1}", Application.dataPath, "../Docs/Designs/Excels/Purchases");
            }

            /// <summary>
            /// 获取Purchases的Excel文件的绝对路径
            /// 编辑器工具类
            /// </summary>
            /// <returns>内购配置Excel文件</returns>
            public static string GetExcelFileFullPath()
            {
                return AorTxt.Format("{0}/{1}", Application.dataPath, "../Docs/Designs/Excels/Purchases/Purchases.xlsm");
            }
        }
        #endregion

        #region 数据埋点路径
        /// <summary>
        /// Track相关路径信息
        /// </summary>
        public static class Track
        {
            /// <summary>
            /// 获取Tracks的Excel根目录的绝对路径
            /// 编辑器工具类
            /// </summary>
            /// <returns>埋点Excel根目录</returns>
            public static string GetExcelRootDirectoryFullPath()
            {
                return AorTxt.Format("{0}/{1}", Application.dataPath, "../Docs/Designs/Excels/Tracks");
            }

            /// <summary>
            /// 获取Tracks的Excel文件的绝对路径
            /// 编辑器工具类
            /// </summary>
            /// <returns>埋点配置Excel文件</returns>
            public static string GetExcelFileFullPath()
            {
                return AorTxt.Format("{0}/{1}", Application.dataPath, "../Docs/Designs/Excels/Tracks/Tracks.xlsm");
            }
        }
        #endregion

        #region 包信息缓存路径
        /// <summary>
        /// 缓存的包名路径信息
        /// </summary>
        public static class CachedPackageInfo
        {
            /// <summary>
            /// 获取缓存的包名信息在ProjectSettings目录下配置文件的绝对路径
            /// 编辑器工具类
            /// </summary>
            /// <returns>包信息缓存文件</returns>
            public static string GetCachedPackageInfoProjectSettingsConfigFileFullPath()
            {
                return AorTxt.Format("{0}/{1}", System.IO.Path.GetFullPath("."), "ProjectSettings/CachedPackageInfo.dat").Replace("\\", "/");
            }
        }
        #endregion

        #region 工程路径
        /// <summary>
        /// Project相关路径信息
        /// </summary>
        public static class Project
        {
            /// <summary>
            /// 获取Project根目录的绝对路径
            /// </summary>
            /// <returns>工程根目录</returns>
            public static string GetRootDirectoryFullPath()
            {
                return AorTxt.Format("{0}/../", Application.dataPath).Replace('\\', '/');
            }

            /// <summary>
            /// 获取VS-Projrect文件的绝对路径
            /// </summary>
            /// <returns>VS解决方案文件</returns>
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
        #endregion

        #region 原生平台路径
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
                /// <returns>iOS原生工程目录</returns>
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
                /// <returns>Android原生工程目录</returns>
                public static string GetProjectRootDirectoryFullPath()
                {
                    return AorTxt.Format("{0}/{1}", Application.dataPath, "../Natives/Android");
                }

                /// <summary>
                /// 获取Native签名文件的绝对路径
                /// 编辑器工具类
                /// </summary>
                /// <returns>安卓签名文件目录</returns>
                public static string GetSignatureDirectoryFullPath()
                {
                    return AorTxt.Format("{0}/{1}", Application.dataPath, "../Docs/Programs/Certificates/Android");
                }
            }
        }
        #endregion

        #region 工具路径
        /// <summary>
        /// Tool相关路径信息
        /// </summary>
        public static class Tool
        {
            /// <summary>
            /// 获取Tool根目录的绝对路径
            /// </summary>
            /// <returns>工具根目录</returns>
            public static string GetRootDirectoryFullPath()
            {
                return AorTxt.Format("{0}/../Tools", Application.dataPath).Replace('\\', '/');
            }

            /// <summary>
            /// 获取BMFont工具目录的绝对路径
            /// </summary>
            /// <returns>BMFont工具目录</returns>
            public static string GetBMFontDirectoryFullPath()
            {
                return AorTxt.Format("{0}/{1}", GetRootDirectoryFullPath(), "BMFont");
            }
        }
        #endregion

        #region 编辑器路径
        /// <summary>
        /// Editor编辑器相关路径信息
        /// </summary>
        public static class Editor
        {
            /// <summary>
            /// 获取ProjectSetting目录的绝对路径
            /// </summary>
            /// <returns>项目设置目录</returns>
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
                /// <returns>资源定义Lua文件</returns>
                public static string GetResDefLuaFullPath()
                {
                   return AorTxt.Format("{0}/{1}", LuaScript.Game.GetRootDirectoryFullPath(true) + "/Config", "LoadResDefs.lua.txt");
                }

                /// <summary>
                /// 获取资源信息定义Lua脚本相对路径
                /// </summary>
                public static string LuaFolderPath = "Assets/LuaScripts/Config/LoadResDefs";

                /// <summary>
                /// 获取资源信息导出工具配置文件绝对路径
                /// </summary>
                /// <returns>资源导出工具配置</returns>
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
                /// <returns>层级展开配置</returns>
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
                /// <returns>字体导出配置</returns>
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
                /// <returns>AB打包配置</returns>
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
                /// <returns>ChatGPT配置</returns>
                public static string GetLibrarySaveFileFullPath()
                {
                    return $"{System.IO.Path.GetFullPath(".")}/Library/ChatGPTConfig.dat".Replace("\\", "/");
                }

                /// <summary>
                /// 获取ChatGPT在Library目录下历史对话记录文件的绝对路径
                /// 编辑器工具类
                /// </summary>
                /// <returns>对话历史</returns>
                public static string GetLibraryChatHistoryFileFullPath()
                {
                    return $"{System.IO.Path.GetFullPath(".")}/Library/ChatGPTChatHistory.dat".Replace("\\", "/");
                }

                /// <summary>
                /// 获取ChatGPT在Library目录下历史Code迭代记录文件的绝对路径
                /// 编辑器工具类
                /// </summary>
                /// <returns>代码历史</returns>
                public static string GetLibraryCodeHistoryFileFullPath()
                {
                    return $"{System.IO.Path.GetFullPath(".")}/Library/ChatGPTCodeHistory.dat".Replace("\\", "/");
                }

                /// <summary>
                /// 获取ChatGPT在Library目录下历史图片创建记录文件的绝对路径
                /// 编辑器工具类
                /// </summary>
                /// <returns>图片创建历史</returns>
                public static string GetLibraryImageCreateHistoryFileFullPath()
                {
                    return $"{System.IO.Path.GetFullPath(".")}/Library/ChatGPTImageCreateHistory.dat".Replace("\\", "/");
                }

                /// <summary>
                /// 获取ChatGPT在Library目录下历史图片编辑记录文件的绝对路径
                /// 编辑器工具类
                /// </summary>
                /// <returns>图片编辑历史</returns>
                public static string GetLibraryImageEditHistoryFileFullPath()
                {
                    return $"{System.IO.Path.GetFullPath(".")}/Library/ChatGPTImageEditHistory.dat".Replace("\\", "/");
                }

                /// <summary>
                /// 获取ChatGPT在Library目录下历史图片变种记录文件的绝对路径
                /// 编辑器工具类
                /// </summary>
                /// <returns>图片变种历史</returns>
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
                /// <returns>CDN配置</returns>
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
                /// <returns>AB浏览器配置</returns>
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
        #endregion

        #region 图片获取工具
        /// <summary>
        /// 获取图片
        /// </summary>
        public static class GetImage
        {
            /// <summary>
            /// 根据名称获取精灵图片
            /// </summary>
            /// <param name="_spriteName">精灵名称</param>
            /// <returns>Sprite对象</returns>
            public static Sprite GetImageByName(string _spriteName)
            {
                return null;
            }
        }
        #endregion
    }
}