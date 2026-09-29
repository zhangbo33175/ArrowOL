/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * -------------------------------------------------------------
 * filename:  ResPaths.cs
 * author:  云毅
 * created:
 * descrip:   资源路径工具类 - 统一资源根目录、路径拼接与可写目录获取
 * 优化记录: 由旧版 HonorUtils.Paths 迁移，统一命名空间与资源枚举；补齐 AB 后缀与目录说明
 ***************************************************************/

using System;
using System.IO;
using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// 资源路径工具类
    /// 功能：声明全部 AB 资源根目录常量，提供资源路径拼接与持久化目录获取
    /// 注意：路径统一使用 '/' 分隔，便于跨平台与 AssetBundle 名称匹配
    /// </summary>
    [Serializable]
    public static class ResPaths
    {
        #region 常量

        /// <summary>是否加载 AB（由启动流程根据运行模式设置）</summary>
        public static bool LoadAB;

        /// <summary>AB 资源根目录（编辑器相对 Assets 的路径）</summary>
        private const string EditorResRoot = "/ABRes/";

        /// <summary>AB 文件后缀</summary>
        private const string FileSuffix = ".ab";

        /// <summary>字体目录</summary>
        private const string FontRoot = "Font/";

        /// <summary>图集目录</summary>
        private const string AtlasRoot = "Atlas/";

        /// <summary>UI 界面目录</summary>
        private const string UIRoot = "ModulesView/";

        /// <summary>模型目录</summary>
        private const string ModelRoot = "Role/";

        /// <summary>音频目录</summary>
        private const string AudioRoot = "Audio/";

        /// <summary>游戏配置目录</summary>
        private const string GameConfigRoot = "Config/GameConfig/";

        /// <summary>战斗配置目录</summary>
        private const string BattleConfigRoot = "Config/BattleConfig/";

        /// <summary>通用配置目录</summary>
        private const string ConfigRoot = "Config/";

        /// <summary>二进制配置目录</summary>
        private const string ConfigByteRoot = "ConfigByte/";

        /// <summary>Lua 脚本目录</summary>
        private const string XLuaRoot = "Xlua/";

        /// <summary>场景目录</summary>
        private const string SceneRoot = "Scene/";

        /// <summary>特效目录</summary>
        private const string EffectRoot = "Effect/";

        /// <summary>贴图目录</summary>
        private const string TextureRoot = "Texture/";

        /// <summary>其他资源目录</summary>
        private const string OtherResRoot = "OtherRes/";

        /// <summary>公共控件目录</summary>
        private const string CommonWidgetsRoot = "CommonWidgets/";

        /// <summary>反射材质目录</summary>
        private const string RefMat = "RefMat/";

        /// <summary>地图目录</summary>
        private const string MapRoot = "Map/";

        #endregion

        #region 资源路径枚举

        /// <summary>
        /// 资源类型枚举（对应 AB 根目录）
        /// </summary>
        public enum PathsEnum
        {
            /// <summary>字体</summary>
            Font,

            /// <summary>图集</summary>
            Atlas,

            /// <summary>UI 界面</summary>
            UI,

            /// <summary>模型</summary>
            Model,

            /// <summary>音频</summary>
            Audio,

            /// <summary>游戏配置</summary>
            GameConfig,

            /// <summary>战斗配置</summary>
            BattleConfig,

            /// <summary>通用配置</summary>
            Config,

            /// <summary>二进制配置</summary>
            ConfigByte,

            /// <summary>Lua 脚本</summary>
            XLua,

            /// <summary>场景</summary>
            Scene,

            /// <summary>特效</summary>
            Effect,

            /// <summary>贴图</summary>
            Texture,

            /// <summary>其他资源</summary>
            OtherRes,

            /// <summary>公共控件</summary>
            CommonWidgets,

            /// <summary>反射材质</summary>
            RefMat,

            /// <summary>地图</summary>
            Map,
        }

        #endregion

        #region 路径获取

        /// <summary>
        /// 根据资源类型与资源名拼接 AB 资源路径
        /// </summary>
        /// <param name="resName">资源名（不含后缀）</param>
        /// <param name="type">资源类型</param>
        /// <returns>形如 "Texture/xxx" 的资源路径，可作为 AB 包名或加载路径</returns>
        public static string GetResourcePath(string resName, PathsEnum type)
        {
            return GetRoot(type) + resName;
        }

        /// <summary>
        /// 根据资源类型获取 AB 根目录
        /// </summary>
        /// <param name="type">资源类型</param>
        /// <returns>根目录字符串</returns>
        public static string GetRoot(PathsEnum type)
        {
            switch (type)
            {
                case PathsEnum.Font:
                    return FontRoot;
                case PathsEnum.Atlas:
                    return AtlasRoot;
                case PathsEnum.UI:
                    return UIRoot;
                case PathsEnum.Model:
                    return ModelRoot;
                case PathsEnum.Audio:
                    return AudioRoot;
                case PathsEnum.GameConfig:
                    return GameConfigRoot;
                case PathsEnum.BattleConfig:
                    return BattleConfigRoot;
                case PathsEnum.Config:
                    return ConfigRoot;
                case PathsEnum.ConfigByte:
                    return ConfigByteRoot;
                case PathsEnum.XLua:
                    return XLuaRoot;
                case PathsEnum.Scene:
                    return SceneRoot;
                case PathsEnum.Effect:
                    return EffectRoot;
                case PathsEnum.Texture:
                    return TextureRoot;
                case PathsEnum.OtherRes:
                    return OtherResRoot;
                case PathsEnum.CommonWidgets:
                    return CommonWidgetsRoot;
                case PathsEnum.RefMat:
                    return RefMat;
                case PathsEnum.Map:
                    return MapRoot;
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// 拼接 AB 文件后缀
        /// </summary>
        /// <param name="assetPath">不带后缀的资源路径</param>
        /// <returns>带 ".ab" 后缀的完整 AB 包名</returns>
        public static string GetFullPathWithSuffix(string assetPath)
        {
            return assetPath + FileSuffix;
        }

        /// <summary>
        /// 获取可写目录下的存档路径（编辑器落在 dataPath/save，真机落在 persistentDataPath/save）
        /// </summary>
        /// <param name="name">文件名</param>
        /// <returns>完整可写路径</returns>
        public static string GetPersistentDataPath(string name)
        {
            if (Platform.IsEditor)
            {
                return Application.dataPath + "/save/" + name;
            }
            else if (Platform.IsIPhone || Platform.IsAndroid)
            {
                return Application.persistentDataPath + "/save/" + name;
            }

            return string.Empty + name;
        }

        /// <summary>
        /// 路径格式化：统一为 '/' 分隔
        /// </summary>
        public static string PathFormat(string path)
        {
            return string.IsNullOrEmpty(path) ? path : path.Replace("\\", "/");
        }

        /// <summary>
        /// 校验文件是否存在（编辑器环境按路径查找，自动忽略 .meta/.tpsheet 等辅助文件）
        /// </summary>
        /// <param name="fullPath">待校验的完整路径（不带后缀）</param>
        /// <returns>是否存在对应资源文件</returns>
        public static bool IsFileExist(string fullPath)
        {
            string normalized = PathFormat(fullPath);
            string dir = Path.GetDirectoryName(normalized);
            if (string.IsNullOrEmpty(dir) || !Directory.Exists(dir))
            {
                return false;
            }

            string[] files = Directory.GetFiles(dir);
            for (int i = 0; i < files.Length; i++)
            {
                string file = PathFormat(files[i]);
                int index = file.LastIndexOf('.');
                if (index == -1)
                {
                    continue;
                }

                string nameWithoutExt = file.Remove(index);
                string ext = file.Remove(0, index);
                if (nameWithoutExt == normalized && ext != ".meta" && ext != ".tpsheet")
                {
                    return true;
                }
            }

            return false;
        }

        #endregion
    }
}
