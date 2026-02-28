using System;
using System.Collections.Generic;

namespace Honor.Runtime
{
    public sealed partial class GameDefinitions
    {
        /// <summary>
        /// 2D/3D类型
        /// </summary>
        public enum DimensionMode
        {
            /// <summary>
            /// 2D模式
            /// </summary>
            Two,

            /// <summary>
            /// 3D模式
            /// </summary>
            Three,
        };

        /// <summary>
        /// Asset类型
        /// </summary>
        public enum AssetType
        {
            GameObject,
            Texture2D,
            Texture,
            Sprite,
            ScriptableObject,
            TileBase,
            Font,
            FontTMP,
            Shader,
            Material,
            TextAsset,
            JsonAsset,
            BinaryAsset,
            LuaAsset,
            LuacAsset,
            Scene,
            Sound,
            Model,
            AnimatorController,
        }

        /// <summary>
        /// Asset后缀名
        /// </summary>
        public static Dictionary<AssetType, string> AssetSuffix = new Dictionary<AssetType, string>()
        {
            { AssetType.GameObject, ".prefab" },
            { AssetType.Texture2D, ".png" },
            { AssetType.Texture, ".png" },
            { AssetType.Sprite, ".png" },
            { AssetType.ScriptableObject, ".asset" },
            { AssetType.TileBase, ".asset" },
            { AssetType.Font, ".ttf" },
            { AssetType.FontTMP, ".asset" },
            { AssetType.Shader, ".shader" },
            { AssetType.Material, ".mat" },
            { AssetType.TextAsset, ".txt" },
            { AssetType.JsonAsset, ".json" },
            { AssetType.BinaryAsset, ".bytes" },
            { AssetType.LuaAsset, ".txt" },
            { AssetType.LuacAsset, ".bytes" },
            { AssetType.Scene, ".unity" },
            { AssetType.Sound, ".ogg" },
            { AssetType.Model, ".fbx" },
            { AssetType.AnimatorController, ".controller" },
        };

        /// <summary>
        /// 路径类型
        /// </summary>
        public enum PathType
        {
            /// <summary>
            /// 可写路径
            /// </summary>
            Persist,

            /// <summary>
            /// 只读路径
            /// </summary>
            Streaming,

            /// <summary>
            /// 服务器路径
            /// </summary>
            Server,

            /// <summary>
            /// 灰度服务器路径
            /// </summary>
            ServerGray,
        };

        /// <summary>
        /// 下载步骤
        /// </summary>
        [Flags]
        public enum DownloadStep : byte
        {
            /// <summary>
            /// 无效
            /// </summary>
            None = 0,

            /// <summary>
            /// 待机
            /// </summary>
            Idle,

            /// <summary>
            /// 进行中
            /// </summary>
            Processing,

            /// <summary>
            /// 全部完成
            /// </summary>
            AllOver,

            /// <summary>
            /// 跳过
            /// </summary>
            Skip,

            /// <summary>
            /// 异常
            /// </summary>
            Error,

            /// <summary>
            /// 检测异常
            /// </summary>
            CheckError,
        };

        /// <summary>
        /// 调试模式
        /// </summary>
        public enum DebugMode
        {
            /// <summary>
            /// 非调试模式
            /// </summary>
            None = 0,

            /// <summary>
            /// 调试模式-IDE优先启动模式
            /// </summary>
            IDEFirst,

            /// <summary>
            /// 调试模式-Unity优先启动模式
            /// </summary>
            UnityFirst,
        };

        /// <summary>
        /// 游戏语言类型
        /// </summary>
        public enum Language : byte
        {
            // 未指定
            Unspecified = 0,

            // 南非荷兰语
            Afrikaans,

            // 阿尔巴尼亚语
            Albanian,

            // 阿拉伯语
            Arabic,

            // 巴斯克语
            Basque,

            // 白俄罗斯语
            Belarusian,

            // 保加利亚语
            Bulgarian,

            // 加泰罗尼亚语
            Catalan,

            // 简体中文
            ChineseSimplified,

            // 繁体中文
            ChineseTraditional,

            // 克罗地亚语
            Croatian,

            // 捷克语
            Czech,

            // 丹麦语
            Danish,

            // 荷兰语
            Dutch,

            // 英语
            English,

            // 爱沙尼亚语
            Estonian,

            // 法罗语
            Faroese,

            // 芬兰语
            Finnish,

            // 法语
            French,

            // 格鲁吉亚语
            Georgian,

            // 德语
            German,

            // 希腊语
            Greek,

            // 希伯来语
            Hebrew,

            // 匈牙利语
            Hungarian,

            // 冰岛语
            Icelandic,

            // 印尼语
            Indonesian,

            // 意大利语
            Italian,

            // 日语
            Japanese,

            // 韩语
            Korean,

            // 拉脱维亚语
            Latvian,

            // 立陶宛语
            Lithuanian,

            // 马其顿语
            Macedonian,

            // 马拉雅拉姆语
            Malayalam,

            // 挪威语
            Norwegian,

            // 波斯语
            Persian,

            // 波兰语
            Polish,

            // 葡萄牙语
            PortuguesePortugal,

            // 罗马尼亚语
            Romanian,

            // 俄语
            Russian,

            // 塞尔维亚克罗地亚语
            SerboCroatian,

            // 塞尔维亚西里尔语
            SerbianCyrillic,

            // 塞尔维亚拉丁语
            SerbianLatin,

            // 斯洛伐克语
            Slovak,

            // 斯洛文尼亚语
            Slovenian,

            // 西班牙语
            Spanish,

            // 瑞典语
            Swedish,

            // 泰语
            Thai,

            // 土耳其语
            Turkish,

            // 乌克兰语
            Ukrainian,

            // 越南语
            Vietnamese,

            // 总数量
            TotalNum,
        };

        /// <summary>
        /// 游戏语言类型描述
        /// </summary>
        public static readonly string[] LanguageDesc = new string[(int)Language.TotalNum]
        {
            "未指定",

            "南非荷兰语",

            "阿尔巴尼亚语",

            "阿拉伯语",

            "巴斯克语",

            "白俄罗斯语",

            "保加利亚语",

            "加泰罗尼亚语",

            "简体中文",

            "繁体中文",

            "克罗地亚语",

            "捷克语",

            "丹麦语",

            "荷兰语",

            "英语",

            "爱沙尼亚语",

            "法罗语",

            "芬兰语",

            "法语",

            "格鲁吉亚语",

            "德语",

            "希腊语",

            "希伯来语",

            "匈牙利语",

            "冰岛语",

            "印尼语",

            "意大利语",

            "日语",

            "韩语",

            "拉脱维亚语",

            "立陶宛语",

            "马其顿语",

            "马拉雅拉姆语",

            "挪威语",

            "波斯语",

            "波兰语",

            "葡萄牙语",

            "罗马尼亚语",

            "俄语",

            "塞尔维亚克罗地亚语",

            "塞尔维亚西里尔语",

            "塞尔维亚拉丁语",

            "斯洛伐克语",

            "斯洛文尼亚语",

            "西班牙语",

            "瑞典语",

            "泰语",

            "土耳其语",

            "乌克兰语",

            "越南语",
        };


        /// <summary>
        /// Debug窗口显示模式
        /// </summary>
        public enum DebugWindowModel
        {
            // 最小化
            MiniWindow,

            // 显示帧率的窗口
            FPSWindow,

            // 全屏窗口
            FullWindow,

            // 弹出窗口
            PopWindow,
        }
    }
}