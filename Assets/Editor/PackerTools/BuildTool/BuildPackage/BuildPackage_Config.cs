/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Editor
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  BuildPackage_Config.cs
 * author:    云毅
 * created:   2026
 * descrip:   AssetBundle 打包工具 - 打包配置模型与默认分组
 *            每个配置项定义：源目录、目录粒度（format）、文件后缀过滤（pattern）、
 *            以及子目录细分（SubPackConfigInfo），与参照 BuildPackage_CfgAB 架构一致
 ***************************************************************/

using System.Collections.Generic;

namespace Honor.Editor.BuildPackage
{
    //=========================================================================
    // 打包粒度枚举
    // 决定同一 bundle 内聚到哪一级目录（参照框架 format 概念，适配 @ 命名）
    //=========================================================================
    /// <summary>
    /// 打包粒度枚举
    /// 说明：决定资源归入 bundle 时的目录粒度；粒度越小（越靠文件），bundle 越细
    /// </summary>
    public enum BuildPackageFormat
    {
        /// <summary>每个文件单独打包（bundle 名 = 文件相对路径）</summary>
        EachFile = 0,

        /// <summary>当前文件所在目录单独打包（bundle 名 = 父目录相对路径）</summary>
        CurrentDir = 1,

        /// <summary>一级子目录单独打包（bundle 名 = 一级目录相对路径）</summary>
        Level1Dir = 2,

        /// <summary>二级子目录单独打包（bundle 名 = 二级目录相对路径）</summary>
        Level2Dir = 3,

        /// <summary>三级子目录单独打包（bundle 名 = 三级目录相对路径）</summary>
        Level3Dir = 4,

        /// <summary>文件所在目录单独打包（同 CurrentDir，供子配置使用）</summary>
        FileDir = 6,
    }

    //=========================================================================
    // 子目录细分配置
    // 对配置目录下的特定子目录应用不同粒度/后缀，优先于父配置
    //=========================================================================
    /// <summary>
    /// 子目录细分打包配置
    /// 说明：对配置目录下匹配 <see cref="SubDirStr"/> 的子目录单独应用
    ///       <see cref="SubFormat"/> 粒度与 <see cref="SubPattern"/> 后缀过滤
    /// </summary>
    [System.Serializable]
    public class BuildPackageSubConfig
    {
        /// <summary>匹配的子目录片段（路径包含即命中，如 "/UI/"）</summary>
        public string SubDirStr = string.Empty;

        /// <summary>子目录应用的后缀过滤（分号分隔，如 ".prefab;.mat"）</summary>
        public string SubPattern = string.Empty;

        /// <summary>子目录应用的打包粒度</summary>
        public BuildPackageFormat SubFormat = BuildPackageFormat.EachFile;
    }

    //=========================================================================
    // 打包配置项
    //=========================================================================
    /// <summary>
    /// 打包配置项
    /// 说明：描述一个源目录的打包规则（路径、粒度、后缀、子目录细分）
    /// </summary>
    [System.Serializable]
    public class BuildPackageConfig
    {
        /// <summary>源目录（相对 Assets 的路径，如 "Assets/Res/Textures"）</summary>
        public string Path = string.Empty;

        /// <summary>打包粒度</summary>
        public BuildPackageFormat Format = BuildPackageFormat.EachFile;

        /// <summary>文件后缀过滤（分号分隔，为空表示不过滤）</summary>
        public string Pattern = string.Empty;

        /// <summary>是否启用该配置（Inspector 可勾选）</summary>
        public bool Enabled = true;

        /// <summary>子目录细分配置列表</summary>
        public List<BuildPackageSubConfig> SubConfigs = new List<BuildPackageSubConfig>();

        /// <summary>
        /// 解析后缀过滤为字符串数组
        /// </summary>
        /// <returns>小写后缀数组（无匹配返回空数组）</returns>
        public string[] GetPatterns()
        {
            if (string.IsNullOrEmpty(Pattern))
                return System.Array.Empty<string>();

            List<string> list = new List<string>();
            foreach (string p in Pattern.Split(';'))
            {
                string t = p.Trim().ToLower();
                if (!string.IsNullOrEmpty(t))
                    list.Add(t);
            }

            return list.ToArray();
        }
    }

    //=========================================================================
    // 后缀过滤预设表
    // 提供常用后缀类型下拉选项，避免手填后缀时输错（如漏点/写错扩展名）
    //=========================================================================
    /// <summary>
    /// 后缀过滤预设表
    /// 说明：EditorWindow 用下拉选择预设；选中后回填 <see cref="BuildPackageConfig.Pattern"/>
    /// </summary>
    public static class BuildPackage_PatternPreset
    {
        /// <summary>
        /// 预设显示名（与 <see cref="Patterns"/> 下标一一对应）
        /// </summary>
        public static readonly string[] Labels =
        {
            "全部(不过滤)",
            "图片(.png;.jpg;.tga)",
            "图标(.png;.jpg)",
            "预制体(.prefab;.asset)",
            "音频(.wav;.ogg;.mp3)",
            "字体(.ttf;.otf;.fontsettings)",
            "字体+资源(.ttf;.otf;.fontsettings;.asset;.mat)",
            "配置(.json;.txt;.bytes)",
            "配置+资源(.json;.txt;.bytes;.asset)",
        };

        /// <summary>
        /// 预设对应后缀字符串（与 <see cref="Labels"/> 下标一一对应）
        /// </summary>
        public static readonly string[] Patterns =
        {
            string.Empty,
            ".png;.jpg;.tga",
            ".png;.jpg",
            ".prefab;.asset",
            ".wav;.ogg;.mp3",
            ".ttf;.otf;.fontsettings",
            ".ttf;.otf;.fontsettings;.asset;.mat",
            ".json;.txt;.bytes",
            ".json;.txt;.bytes;.asset",
        };

        /// <summary>
        /// 查找当前后缀字符串命中的预设下标
        /// </summary>
        /// <param name="pattern">后缀字符串</param>
        /// <returns>命中预设下标；未命中返回 0（全部，仅用于展示，不回写数据）</returns>
        public static int IndexOf(string pattern)
        {
            string p = string.IsNullOrEmpty(pattern) ? string.Empty : pattern.Trim();
            for (int i = 0; i < Patterns.Length; i++)
            {
                if (string.Equals(Patterns[i], p, System.StringComparison.OrdinalIgnoreCase))
                    return i;
            }

            return 0;
        }
    }

    //=========================================================================
    // 默认打包配置集合
    // 依据工程 Assets/Res 目录结构预置，可在打包窗口内增删改
    //=========================================================================
    /// <summary>
    /// 默认打包配置集合
    /// 说明：提供工程资源目录的默认分组规则，打包窗口加载后可在界面调整
    /// </summary>
    public static class BuildPackage_DefaultConfigs
    {
        /// <summary>
        /// 获取默认打包配置列表（依据 Assets/Res 结构预置）
        /// </summary>
        /// <returns>打包配置列表</returns>
        public static List<BuildPackageConfig> GetDefaults()
        {
            List<BuildPackageConfig> list = new List<BuildPackageConfig>
            {
                // 图集：每个文件单独打包
                new BuildPackageConfig
                {
                    Path = "Assets/Res/Atlas",
                    Format = BuildPackageFormat.EachFile,
                    Pattern = ".png;.jpg;.tga",
                },

                // TMP shader：UI 文字（TMP）材质引用 TextMeshPro/Mobile/Distance Field 等 shader。
                // 若 shader 不打进 AB，则 AB 模式下材质从 bundle 加载后其 shader 引用（按 GUID）
                // 无法解析 → shader 为 null → 文字渲染为空（编辑模式 shader 在项目里所以正常）。
                // 整目录合入一个 shader bundle，作为字体/UI 材质 bundle 的依赖自动加载。
                new BuildPackageConfig
                {
                    Path = "Assets/Plugins/ThirdLibs/TextMesh Pro/Shaders",
                    Format = BuildPackageFormat.FileDir,
                    Pattern = ".shader",
                },

                // 字体：每个文件单独打包
                new BuildPackageConfig
                {
                    Path = "Assets/Res/Fonts",
                    Format = BuildPackageFormat.EachFile,
                    Pattern = ".ttf;.otf;.fontsettings;.asset;.mat",

                    // TMP 字体子目录：TMP 字体 .asset 与其全部变体材质 .mat 合入同一个 bundle
                    // （fonts@tmps.bundle）。outline 等变体材质的 _MainTex 引用字体 .asset 内嵌
                    // 图集子资源，若与字体分属不同 bundle，跨包引用子资源在 AB 下解析失败 →
                    // 材质图集为空 → 文字渲染为空。合入同包后变同包引用，必然解析成功。
                    SubConfigs = new List<BuildPackageSubConfig>
                    {
                        new BuildPackageSubConfig
                        {
                            SubDirStr = "Assets/Res/Fonts/TMPs",
                            SubFormat = BuildPackageFormat.FileDir,
                            SubPattern = ".asset;.mat",
                        },
                    },
                },

                // 图标：每个文件单独打包
                new BuildPackageConfig
                {
                    Path = "Assets/Res/Icon",
                    Format = BuildPackageFormat.EachFile,
                    Pattern = ".png;.jpg",
                },

                // 配置（json 等）：每个文件单独打包
                new BuildPackageConfig
                {
                    Path = "Assets/Res/json",
                    Format = BuildPackageFormat.EachFile,
                    Pattern = ".json;.txt;.bytes;.asset",
                },

                // 预制体/UI：每个文件单独打包（UI 通常按资源名精确加载）
                new BuildPackageConfig
                {
                    Path = "Assets/Res/Prefabs",
                    Format = BuildPackageFormat.EachFile,
                    Pattern = ".prefab;.asset",
                },

                // 音频：按 ABPath 目录粒度合包（与配置表 TableSounds 的 ABPath 目录级保持一致）
                // 说明：TableSounds 的 ABPath 为目录级（如 "Assets/Res/Sounds/map1"、".../common"、
                //      ".../ActivityGroup/GoldenTicket"、"Assets/Res/Sounds/UISounds"），运行时
                //      AssetBundleLoadManager.GetABFormatPath 取其相对 Res 的路径转 bundle 名
                //      （"Assets/Res/Sounds/map1" → "sounds@map1.bundle"）。
                //      若沿用 EachFile（每文件打包），产出为 "sounds@map1@musics@bgm_map1.bundle"，
                //      与 ABPath 目录级不匹配 → 运行时加载 AB 失败 → 打包后无声音（编辑器直接加载项目资源正常）。
                //      故改用 Level2Dir：取 "Sounds/{目录}" 两级（map1/common/UISounds 均匹配）；
                //      ActivityGroup/GoldenTicket 为 4 级目录，用 Level3Dir 子配置单独合包，
                //      "Sounds/ActivityGroup/GoldenTicket" → sounds@activitygroup@goldenticket.bundle（匹配 ABPath）。
                new BuildPackageConfig
                {
                    Path = "Assets/Res/Sounds",
                    Format = BuildPackageFormat.Level2Dir,
                    Pattern = ".wav;.ogg;.mp3",

                    SubConfigs = new List<BuildPackageSubConfig>
                    {
                        new BuildPackageSubConfig
                        {
                            // ActivityGroup/GoldenTicket 为 4 级目录，ABPath 为 Sounds/ActivityGroup/GoldenTicket，
                            // 用 Level3Dir 合包使 bundle 名与 ABPath 匹配（含其下 Musics 与 UISounds 子目录）
                            SubDirStr = "/ActivityGroup/GoldenTicket",
                            SubFormat = BuildPackageFormat.Level3Dir,
                            SubPattern = ".wav;.ogg;.mp3",
                        },
                    },
                },

                // 贴图：按每文件粒度打包（独立 bundle，与 ResDef 的 ABPath/AssetName（含资源名）匹配，
                //      加载方式同 UI/Prefabs(EachFile) 已验证正常；副作用是包体碎片增多）
                new BuildPackageConfig
                {
                    Path = "Assets/Res/Textures",
                    Format = BuildPackageFormat.EachFile,
                    Pattern = ".png;.jpg;.tga",
                },
            };

            return list;
        }
    }
}
