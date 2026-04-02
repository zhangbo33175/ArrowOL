using UnityEditor;
using UnityEngine;
using UnityEngine.Windows;

namespace Honor.Editor
{
    public static partial class EditorPath
    {
        /// <summary>
        /// Editor编辑器相关路径信息
        /// </summary>
        /// <summary>
        /// Editor编辑器相关路径信息
        /// </summary>
        public static class Editor
        {
            /// <summary>
            /// 获取ProjectSetting目录的绝对路径
            /// </summary>
            /// <returns></returns>
            public static string ProjectSettingFolderFullPath()
            {
                string projectSettingFolderFullPath = $"{Application.dataPath}/../ProjectSettings/Honor";
                if (!Directory.Exists(projectSettingFolderFullPath))
                {
                    Directory.CreateDirectory(projectSettingFolderFullPath);
                    AssetDatabase.Refresh();
                }

                return projectSettingFolderFullPath;
            }

            /// <summary>
            /// 获取Library目录的绝对路径
            /// </summary>
            /// <returns></returns>
            public static string LibraryFolderFullPath()
            {
                string libraryFolderFullPath = $"{Application.dataPath}/../Library/Honor";
                if (!Directory.Exists(libraryFolderFullPath))
                {
                    Directory.CreateDirectory(libraryFolderFullPath);
                    AssetDatabase.Refresh();
                }

                return libraryFolderFullPath;
            }

            /// <summary>
            /// 获取Packages目录的绝对路径
            /// </summary>
            /// <returns></returns>
            public static string PackagesFolderFullPath()
            {
                return $"{Application.dataPath}/../Packages";
            }

            /// <summary>
            /// 获取Package目录中manifest.json文件的绝对路径
            /// </summary>
            /// <returns></returns>
            public static string PackageManifestFileFullPath()
            {
                return $"{PackagesFolderFullPath()}/manifest.json";
            }

            /// <summary>
            /// 获取共享OSS配置文件的绝对路径
            /// </summary>
            /// <returns></returns>
            public static string SharedOSSProjectSettingFileFullPath()
            {
                return $"{ProjectSettingFolderFullPath()}/SharedOSSSettings.json";
            }

            /// <summary>
            /// 项目工程GitLab地址
            /// </summary>
            public static string GitLabUrl = "https://gitlab.int.nbt.ren/taoye/Honor";

            /// <summary>
            /// GitLab-Icon图片资源位置
            /// </summary>
            public static string GitLabIconFilePath = "Assets/Framework/Textures/PicsForEditor/GitLab.png";
            
            /// <summary>
            /// About相关信息
            /// </summary>
            public static class AboutHonor
            {
                /// <summary>
                /// Logo-Icon图片资源位置
                /// </summary>
                public static string LogoIconFilePath = "Assets/Framework/Textures/PicsForEditor/Splash.png";

                /// <summary>
                /// HonorAni-Icon图片资源位置
                /// </summary>
                public static string[] HonorAniIconFilePaths =
                {
                    "Assets/Framework/Textures/PicsForEditor/HonorAnis/A/{0}.png",
                    "Assets/Framework/Textures/PicsForEditor/HonorAnis/B/{0}.png"
                };
            }

            /// <summary>
            /// XLua相关路径信息
            /// </summary>
            public static class XLua
            {
                /// <summary>
                /// 获取XLua自动生成开关配置文件的绝对路径
                /// </summary>
                public static string ProjectSettingFileFullPath =
                    $"{ProjectSettingFolderFullPath()}/XLuaAutoGenSettings.json";
            }

            /// <summary>
            /// ABGeneration相关路径信息
            /// </summary>
            public static class ABGeneration
            {
                /// <summary>
                /// 获取AB生成配置文件的绝对路径
                /// </summary>
                public static string ProjectSettingFileFullPath =
                    $"{ProjectSettingFolderFullPath()}/HotfixABSettings.json";
            }

            /// <summary>
            /// Table相关路径信息
            /// </summary>
            public static class Table
            {
                /// <summary>
                /// 获取Tables信息的配置文件绝对路径
                /// </summary>
                public static string ProjectSettingFileFullPath =
                    $"{ProjectSettingFolderFullPath()}/TablesSettings.json";
            }

            /// <summary>
            /// Localization相关路径信息
            /// </summary>
            public static class Localization
            {
                /// <summary>
                /// 获取Localization信息的配置文件绝对路径
                /// </summary>
                public static string ProjectSettingFileFullPath =
                    $"{ProjectSettingFolderFullPath()}/LocalizationSettings.json";
            }

            /// <summary>
            /// UI相关路径信息
            /// </summary>
            public static class UI
            {
                /// <summary>
                /// 获取UIs信息的配置文件绝对路径
                /// </summary>
                public static string ProjectSettingFileFullPath = $"{ProjectSettingFolderFullPath()}/UIsSettings.json";
            }

            /// <summary>
            /// Network相关路径信息
            /// </summary>
            public static class Network
            {
                /// <summary>
                /// 获取Network信息的配置文件绝对路径
                /// </summary>
                public static string ProjectSettingFileFullPath =
                    $"{ProjectSettingFolderFullPath()}/NetworkSettings.json";
            }

            /// <summary>
            /// 资源信息定义路径信息
            /// </summary>
            public static class ResDef
            {
                /// <summary>
                /// 获取资源信息定义Lua脚本相对路径
                /// </summary>
                public static string LuaFolderPath = $"Assets/Game/LuaScripts/ResDefs";

                /// <summary>
                /// 获取资源信息导出工具设置文件绝对路径
                /// </summary>
                public static string ProjectSettingFileFullPath =
                    $"{ProjectSettingFolderFullPath()}/ResDefExportSettings.json";
            }

            /// <summary>
            /// Hierarchy展开规则设置工具路径信息
            /// </summary>
            public static class HierarchyExpandSettings
            {
                /// <summary>
                /// 获取Hierarchy展开规则设置工具的配置文件绝对路径
                /// </summary>
                public static string ProjectSettingFileFullPath =
                    $"{ProjectSettingFolderFullPath()}/HierarchyExpandSettings.json";
            }

            /// <summary>
            /// 本地化多语言字符集导出配置
            /// </summary>
            public static class LocalizationFontTMPExportSettings
            {
                /// <summary>
                /// 本地化多语言字符集导出配置文件路径
                /// </summary>
                public static string ProjectSettingFileFullPath =
                    $"{ProjectSettingFolderFullPath()}/FontTMPCharsExportSettings.json";
            }

            /// <summary>
            /// ChatGPT存档路径信息
            /// </summary>
            public static class ChatGPT
            {
                /// <summary>
                /// 获取ChatGPT在Project目录下设置文件的绝对路径
                /// </summary>
                public static string ProjectSettingFileFullPath =
                    $"{ProjectSettingFolderFullPath()}/ChatGPTSettings.json";

                /// <summary>
                /// 获取ChatGPT在Project目录下翻译配置文件的绝对路径
                /// </summary>
                public static string ProjectTranslationConfigFileFullPath =
                    $"{ProjectSettingFolderFullPath()}/ChatGPTTranslationConfigs.json";

                /// <summary>
                /// 获取ChatGPT在Library目录下存档配置文件的绝对路径
                /// </summary>
                public static string LibraryConfigFileFullPath = $"{LibraryFolderFullPath()}/ChatGPTConfig.json";

                /// <summary>
                /// 获取ChatGPT在Library目录下历史对话记录文件的绝对路径
                /// </summary>
                public static string LibraryChatHistoryFileFullPath =
                    $"{LibraryFolderFullPath()}/ChatGPTChatHistory.json";

                /// <summary>
                /// 获取ChatGPT在Library目录下历史Code迭代记录文件的绝对路径
                /// </summary>
                public static string LibraryCodeHistoryFileFullPath =
                    $"{LibraryFolderFullPath()}/ChatGPTCodeHistory.json";

                /// <summary>
                /// 获取ChatGPT在Library目录下翻译设置文件的绝对路径
                /// </summary>
                public static string LibraryTranslationFileFullPath =
                    $"{LibraryFolderFullPath()}/ChatGPTTranslationSetting.json";

                /// <summary>
                /// 获取ChatGPT在Library目录下历史图片创建记录文件的绝对路径
                /// </summary>
                public static string LibraryImageCreateHistoryFileFullPath =
                    $"{LibraryFolderFullPath()}/ChatGPTImageCreateHistory.json";

                /// <summary>
                /// 获取ChatGPT在Library目录下历史图片编辑记录文件的绝对路径
                /// </summary>
                public static string LibraryImageEditHistoryFileFullPath =
                    $"{LibraryFolderFullPath()}/ChatGPTImageEditHistory.json";

                /// <summary>
                /// 获取ChatGPT在Library目录下历史图片变种记录文件的绝对路径
                /// </summary>
                public static string LibraryImageVariationHistoryFileFullPath =
                    $"{LibraryFolderFullPath()}/ChatGPTImageVariationHistory.json";
            }

            /// <summary>
            /// CDN存档路径信息
            /// </summary>
            public static class CDN
            {
                /// <summary>
                /// 获取CDN在ProjectSetting目录下存档配置文件的绝对路径
                /// </summary>
                public static string ProjectSettingFileFullPath = $"{ProjectSettingFolderFullPath()}/CDNSettings.json";
            }

            /// <summary>
            /// 日志拉取器路径信息
            /// </summary>
            public static class LogFetcher
            {
                /// <summary>
                /// 获取日志文件在Library目录下文件夹的绝对路径
                /// </summary>
                public static string LibraryLocalLogFolderFullPath = $"{LibraryFolderFullPath()}/FetchedLogs";
            }

            /// <summary>
            /// Lua云脚本存档路径信息
            /// </summary>
            public static class LuaCloudScript
            {
                /// <summary>
                /// 获取Lua云脚本在ProjectSetting目录下存档配置文件的绝对路径
                /// </summary>
                public static string ProjectSettingFileFullPath =
                    $"{ProjectSettingFolderFullPath()}/LuaCloudScriptSettings.json";

                /// <summary>
                /// 获取Lua脚本在Library目录下文件的绝对路径
                /// </summary>
                public static string LibraryLocalLuaFileFullPath = $"{LibraryFolderFullPath()}/LuaScript.lua.txt";
            }

            /// <summary>
            /// CodeExecuter存档路径信息
            /// </summary>
            public static class CodeExecuter
            {
                /// <summary>
                /// 获取CodeExecuter在Library目录下存档配置文件的绝对路径
                /// </summary>
                public static string LibraryFileFullPath = $"{LibraryFolderFullPath()}/CodeExecuterConfigs.json";
            }

            /// <summary>
            /// 缓存的包名路径信息
            /// </summary>
            public static class CachedPackageInfo
            {
                /// <summary>
                /// 获取缓存的包名信息在ProjectSettings目录下配置文件的绝对路径
                /// </summary>
                public static string ProjectConfigFileFullPath =
                    $"{ProjectSettingFolderFullPath()}/CachedPackageInfo.json";
            }

            /// <summary>
            /// 版本升级检查器路径信息
            /// </summary>
            public static class VersionUpdateCheckerEditorWindows
            {
                /// <summary>
                /// 获取 VersionUpdateChecker 在Library目录下存档配置文件的绝对路径
                /// </summary>
                public static string LibraryFileFullPath = $"{LibraryFolderFullPath()}/VersionUpdateChecker.json";
            }

            /// <summary>
            /// 版本更新路径信息
            /// </summary>
            public static class VersionUpdaterEditorWindow
            {
                /// <summary>
                /// 下载的最新版CHANGELOG文件日志绝对路径
                /// </summary>
                public static string NewChangeLogFileDownloadedFullPath = $"{LibraryFolderFullPath()}/NewCHANGELOG.md";

                /// <summary>
                /// 下载的Unitypackage文件绝对路径
                /// </summary>
                public static string UnitypackageFileDownloadedFullPath = $"{LibraryFolderFullPath()}/{{0}}";

                /// <summary>
                /// 云端文件绝对路径
                /// </summary>
                public static string ServerFileFullPath =
                    "https://Honor-framework.oss-cn-beijing.aliyuncs.com/HonorDevelop/Unitypackages/{0}";

                /// <summary>
                /// 云端文件测试绝对路径
                /// </summary>
                ///public static string ServerFileFullPath = "https://Honor-framework.oss-cn-beijing.aliyuncs.com/HonorDevelop/UnitypackagesTest/{0}";
                /// <summary>
                /// 获取 VersionUpdater 在Library目录下存档配置文件的绝对路径
                /// </summary>
                public static string LibraryFileFullPath = $"{LibraryFolderFullPath()}/VersionUpdaterSetting.json";

                /// <summary>
                /// 导出package过程中生成的zip文件名称
                /// </summary>
                public static string PackageZipFileName = "__ExternalFiles__";

                /// <summary>
                /// __ExternalFiles__解压后存放的临时文件夹
                /// </summary>
                public static string LibraryUnzipFolderFullPath = $"{LibraryFolderFullPath()}/{PackageZipFileName}";

                /// <summary>
                /// __ExternalFiles__解压后Packages文件夹
                /// </summary>
                public static string LibraryUnzipPackagesFolderFullPath = $"{LibraryUnzipFolderFullPath}/Packages";

                /// <summary>
                /// __ExternalFiles__解压后Game/Luas文件夹
                /// </summary>
                public static string LibraryUnzipGameLuasFolderFullPath = $"{LibraryFolderFullPath()}/GameLuas";
            }

            /// <summary>
            /// 版本发布路径信息
            /// </summary>
            public static class VersionReleaseEditorWindow
            {
                /// <summary>
                /// 获取版本发布信息在ProjectSettings目录下配置文件的绝对路径
                /// </summary>
                public static string ProjectSettingFileFullPath =
                    $"{ProjectSettingFolderFullPath()}/VersionReleaseSettings.json";

                /// <summary>
                /// CHANGELOG文件日志绝对路径
                /// </summary>
                public static string ChangeLogFileFullPath = $"{Application.dataPath}/Framework/CHANGELOG.md";

                /// <summary>
                /// README文件绝对路径
                /// </summary>
                public static string ReadmeFileFullPath = $"{Application.dataPath}/../README.md";

                /// <summary>
                /// Package版本文件绝对路径
                /// </summary>
                public static string PackageJsonFileFullPath = $"{Application.dataPath}/Framework/package.json";

                /// <summary>
                /// 版本号定义所在代码文件绝对路径
                /// </summary>
                public static string VersionSrcFileFullPath =
                    $"{Application.dataPath}/Framework/Scripts/Runtime/Bases/Definitions/Constants.cs";

                /// <summary>
                /// 导出版本时携带的存放外部文件的文件夹绝对路径
                /// </summary>
                public static string AssetsExternalFolderFullPath = $"{LibraryFolderFullPath()}/__ExternalFiles__";

                /// <summary>
                /// 云端上传的unitypackage包文件绝对路径
                /// </summary>
                public static string unitypackageFileUploadFullPath = "{0}/HonorDevelop/Unitypackages/{1}";

                /// <summary>
                /// 本地max环境下的bundle文件路径（unity默认不包含xxx.bundle文件夹）
                /// </summary>
                public static string[] AssetsBundleFolderFullPaths = new[]
                {
                    $"{Application.dataPath}/Plugins/Mac/xlua.bundle",
                    $"{Application.dataPath}/Plugins/Android/Honor.androidlib",
                };
            }

            /// <summary>
            /// AssetBundleBrowser存档路径信息
            /// </summary>
            public static class AssetBundleBrowserEditorWindow
            {
                /// <summary>
                /// 获取AssetBundleBrowser在ProjectSetting目录下存档配置文件的绝对路径
                /// </summary>
                public static string ProjectSettingFileFullPath =
                    $"{ProjectSettingFolderFullPath()}/AssetBundleBrowserSettings.json";
            }

            /// <summary>
            /// Android平台打包出的AAB存档文件夹
            /// </summary>
            public static class Build
            {
                /// <summary>
                /// 获取Native文件夹
                /// </summary>
                /// <returns></returns>
                public static string BuildNativeFolderFullPath()
                {
                    string nativePath = Application.dataPath.Replace("Assets", "Natives");
                    if (!Directory.Exists(nativePath))
                    {
                        Directory.CreateDirectory(nativePath);
                        AssetDatabase.Refresh();
                    }

                    return nativePath;
                }

                /// <summary>
                /// 获取Android平台AAB文件默认生成文件夹
                /// </summary>
                /// <returns></returns>
                public static string BuildAABFolderFullPath()
                {
                    string exportAABsPath = $"{BuildNativeFolderFullPath()}/AAB";
                    if (!Directory.Exists(exportAABsPath))
                    {
                        Directory.CreateDirectory(exportAABsPath);
                        AssetDatabase.Refresh();
                    }

                    return exportAABsPath;
                }

                /// <summary>
                /// 获取Android平台AAB文件默认生成文件夹
                /// </summary>
                /// <returns></returns>
                public static string BuildAPKFolderFullPath()
                {
                    string exportAPKsPath = $"{BuildNativeFolderFullPath()}/APK";
                    if (!Directory.Exists(exportAPKsPath))
                    {
                        Directory.CreateDirectory(exportAPKsPath);
                        AssetDatabase.Refresh();
                    }

                    return exportAPKsPath;
                }
            }

            /// <summary>
            /// 代码裁切相关路径信息
            /// </summary>
            public static class CodesClippingEditorWindow
            {
                /// <summary>
                /// 云端代码裁切信息列表配置文件绝对路径
                /// </summary>
                private static string CodeVersionsServerUrl =
                    "https://Honor-framework.oss-cn-beijing.aliyuncs.com/HonorDevelop/CodeVersions";

                /// <summary>
                /// 获取云端指定文件绝对路径
                /// </summary>
                /// <param name="filePathRelativeToCodeVersions">相对于CodeVersions的文件路径（携带后缀名）</param>
                /// <returns></returns>
                public static string GetServerFileFullPath(string filePathRelativeToCodeVersions)
                {
                    return $"{CodeVersionsServerUrl}/{filePathRelativeToCodeVersions}";
                }

                /// <summary>
                /// 获取本地代码配置文件夹绝对路径
                /// </summary>
                /// <returns></returns>
                public static string GetCodeConfigsLocalFolderFullPath()
                {
                    return $"{Application.dataPath}/Framework/ThirdLibs/CodeConfigs";
                }

                /// <summary>
                /// 获取本地代码配置文件绝对路径
                /// </summary>
                /// <param name="configFileName">配置文件名称（携带.json后缀名）</param>
                /// <returns></returns>
                public static string GetCodeConfigLocalFileFullPath(string configFileName)
                {
                    return $"{GetCodeConfigsLocalFolderFullPath()}/{configFileName}";
                }

                /// <summary>
                /// 获取下载代码所在文件夹绝对路径
                /// </summary>
                /// <param name="codeName">代码名称</param>
                /// <returns></returns>
                public static string GetDownloadedCodeFolderFullPath(string codeName)
                {
                    return $"{PackagesFolderFullPath()}/{codeName}";
                }
            }
        }
    }
}