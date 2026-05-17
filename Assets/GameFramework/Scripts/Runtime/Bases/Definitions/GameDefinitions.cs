/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  GameDefinitions.cs
 * author:    云毅
 * created:   2026
 * descrip:   游戏框架全局枚举与核心定义文件
 *            统一管理维度、资源、路径、语言、状态等全局枚举与映射关系
 ***************************************************************/

using System;
using System.Collections.Generic;

namespace Honor.Runtime
{
    #region 游戏框架全局枚举 & 定义
    /// <summary>
    /// 游戏框架全局枚举 & 定义
    /// </summary>
    /// <remarks>
    /// 统一管理游戏内所有类型、资源、路径、语言、状态等枚举
    /// 属于框架核心常量定义文件，全局共享使用
    /// </remarks>
    public static partial class GameDefinitions
    {
        //=========================================================================
        // 游戏维度类型
        //=========================================================================
        /// <summary>
        /// 游戏维度类型（2D / 3D）
        /// </summary>
        public enum DimensionMode
        {
            /// <summary>
            /// 2D 游戏模式
            /// </summary>
            Two,

            /// <summary>
            /// 3D 游戏模式
            /// </summary>
            Three
        }

        //=========================================================================
        // 资源类型定义
        //=========================================================================
        /// <summary>
        /// 资源（Asset）类型枚举
        /// </summary>
        /// <remarks>对应 Unity 引擎内各类资源文件类型</remarks>
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
            AnimatorController
        }

        /// <summary>
        /// 资源类型与文件后缀名映射表
        /// </summary>
        /// <remarks>用于资源加载、热更新、文件校验时匹配文件后缀</remarks>
        public static readonly Dictionary<AssetType, string> AssetSuffix = new Dictionary<AssetType, string>()
        {
            { AssetType.GameObject,          ".prefab" },
            { AssetType.Texture2D,            ".png" },
            { AssetType.Texture,              ".png" },
            { AssetType.Sprite,               ".png" },
            { AssetType.ScriptableObject,     ".asset" },
            { AssetType.TileBase,             ".asset" },
            { AssetType.Font,                 ".ttf" },
            { AssetType.FontTMP,              ".asset" },
            { AssetType.Shader,               ".shader" },
            { AssetType.Material,             ".mat" },
            { AssetType.TextAsset,            ".txt" },
            { AssetType.JsonAsset,            ".json" },
            { AssetType.BinaryAsset,          ".bytes" },
            { AssetType.LuaAsset,             ".txt" },
            { AssetType.LuacAsset,            ".bytes" },
            { AssetType.Scene,                ".unity" },
            { AssetType.Sound,                ".ogg" },
            { AssetType.Model,                ".fbx" },
            { AssetType.AnimatorController,   ".controller" }
        };

        //=========================================================================
        // 路径类型
        //=========================================================================
        /// <summary>
        /// 路径类型
        /// </summary>
        /// <remarks>区分资源读取的不同路径来源</remarks>
        public enum PathType
        {
            /// <summary>
            /// 可读写持久化路径
            /// </summary>
            Persist,

            /// <summary>
            /// 只读 StreamingAssets 路径
            /// </summary>
            Streaming,

            /// <summary>
            /// 正式服务器路径
            /// </summary>
            Server,

            /// <summary>
            /// 灰度/测试服务器路径
            /// </summary>
            ServerGray
        }

        //=========================================================================
        // 下载步骤状态
        //=========================================================================
        /// <summary>
        /// 下载步骤状态
        /// </summary>
        /// <remarks>支持位标记运算，用于热更新、资源下载流程状态管理</remarks>
        [Flags]
        public enum DownloadStep : byte
        {
            /// <summary>
            /// 无状态 / 无效状态
            /// </summary>
            None        = 0,

            /// <summary>
            /// 待机空闲状态
            /// </summary>
            Idle        = 1,

            /// <summary>
            /// 下载/处理中状态
            /// </summary>
            Processing  = 2,

            /// <summary>
            /// 全部完成状态
            /// </summary>
            AllOver     = 4,

            /// <summary>
            /// 已跳过状态
            /// </summary>
            Skip        = 8,

            /// <summary>
            /// 发生错误状态
            /// </summary>
            Error       = 16,

            /// <summary>
            /// 版本/资源检测错误状态
            /// </summary>
            CheckError  = 32
        }

        //=========================================================================
        // 调试启动模式
        //=========================================================================
        /// <summary>
        /// 调试启动模式
        /// </summary>
        /// <remarks>用于编辑器下区分不同的启动逻辑</remarks>
        public enum DebugMode
        {
            /// <summary>
            /// 非调试模式（正式运行）
            /// </summary>
            None        = 0,

            /// <summary>
            /// 调试模式：IDE 优先启动
            /// </summary>
            IDEFirst,

            /// <summary>
            /// 调试模式：Unity 优先启动
            /// </summary>
            UnityFirst
        }

        //=========================================================================
        // 语言类型定义
        //=========================================================================
        /// <summary>
        /// 游戏支持的语言类型
        /// </summary>
        public enum Language : byte
        {
            Unspecified         = 0,
            Afrikaans,
            Albanian,
            Arabic,
            Basque,
            Belarusian,
            Bulgarian,
            Catalan,
            ChineseSimplified,
            ChineseTraditional,
            Croatian,
            Czech,
            Danish,
            Dutch,
            English,
            Estonian,
            Faroese,
            Finnish,
            French,
            Georgian,
            German,
            Greek,
            Hebrew,
            Hungarian,
            Icelandic,
            Indonesian,
            Italian,
            Japanese,
            Korean,
            Latvian,
            Lithuanian,
            Macedonian,
            Malayalam,
            Norwegian,
            Persian,
            Polish,
            PortuguesePortugal,
            Romanian,
            Russian,
            SerboCroatian,
            SerbianCyrillic,
            SerbianLatin,
            Slovak,
            Slovenian,
            Spanish,
            Swedish,
            Thai,
            Turkish,
            Ukrainian,
            Vietnamese,
            TotalNum
        }

        /// <summary>
        /// 语言名称描述数组
        /// </summary>
        /// <remarks>与 Language 枚举一一对应，用于UI显示、日志打印、调试</remarks>
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
            "越南语"
        };

        //=========================================================================
        // 调试窗口显示模式
        //=========================================================================
        /// <summary>
        /// 调试窗口显示模式
        /// </summary>
        public enum DebugWindowModel
        {
            /// <summary>
            /// 最小化悬浮窗
            /// </summary>
            MiniWindow,

            /// <summary>
            /// 仅显示 FPS 的窗口
            /// </summary>
            FPSWindow,

            /// <summary>
            /// 全屏调试窗口
            /// </summary>
            FullWindow,

            /// <summary>
            /// 弹出式窗口
            /// </summary>
            PopWindow
        }
    }
    #endregion
}