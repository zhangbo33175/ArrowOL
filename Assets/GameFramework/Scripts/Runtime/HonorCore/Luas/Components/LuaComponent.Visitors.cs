/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  LuaComponent.Visitors.cs
 * author:    云毅
 * created:   2026
 * descrip:   LuaComponent 字段、常量、属性定义模块
 ***************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace Honor.Runtime
{
    /// <summary>
    /// Lua 核心组件 - 字段与属性定义部分
    /// 包含 Lua 环境、配置、缓存、委托、全局状态等所有成员定义
    /// </summary>
    public sealed partial class LuaComponent 
    {
        //=========================================================================
        #region 常量 & 静态数据
        //=========================================================================

        /// <summary>
        /// Luac 字节码加密密钥
        /// </summary>
        public static byte[] s_LuacEncrytionKey =
        {
            110, 52, 63, 5, 7, 13, 245, 206, 178, 221, 143, 135, 53, 41, 29, 3, 197, 77, 103, 82
        };

        /// <summary>
        /// Lua GC 调用间隔（秒）
        /// </summary>
        internal const float s_GCInterval = 1f;

        #endregion

        //=========================================================================
        #region 运行时状态
        //=========================================================================

        /// <summary>
        /// 最近一次 Lua GC 调用时间
        /// </summary>
        internal static float s_LastGCTime;

        #endregion

        //=========================================================================
        #region 序列化配置字段
        //=========================================================================

        /// <summary>
        /// Lua 运行时性能分析模式（仅编辑器生效）
        /// </summary>
        [SerializeField]
        private bool m_LuaRuntimeProfilerMode;

        #endregion

        //=========================================================================
        #region 公开属性
        //=========================================================================

        /// <summary>
        /// Lua 运行时性能分析开关
        /// </summary>
        public bool LuaRuntimeProfilerMode
        {
            get { return m_LuaRuntimeProfilerMode; }
        }

        /// <summary>
        /// 全局唯一 Lua 运行环境
        /// </summary>
        public LuaEnv Env
        {
            get { return m_Env; }
        }

        /// <summary>
        /// 已加载 Lua 脚本名称列表
        /// </summary>
        public List<string> LoadedLuaScriptsNames
        {
            get { return m_LoadedLuaScriptsNames; }
        }

        #endregion

        //=========================================================================
        #region 私有成员
        //=========================================================================

        /// <summary>
        /// 全局唯一 Lua 虚拟机实例
        /// </summary>
        private LuaEnv m_Env;

        /// <summary>
        /// 已加载 Lua 脚本名称缓存
        /// </summary>
        private List<string> m_LoadedLuaScriptsNames;

        /// <summary>
        /// 所有 Lua 脚本的绝对路径映射（仅 Editor 资源模式）
        /// </summary>
        private Dictionary<string, string> m_LuaScriptsFullPathsMapForLoading;

        /// <summary>
        /// 可加载 AB 路径列表（仅 AB 包模式）
        /// </summary>
        private List<string> m_LuaScriptsABPathsForLoading;

        /// <summary>
        /// Lua 模块名 → 首个命中的 AB 路径缓存（仅 AB 包模式）
        /// 首次 require 全量扫描后记录，后续直接命中，避免每次 require 全扫 AB 路径列表
        /// 不区分大小写：预取阶段从全小写 AB 路径枚举，require 名为驼峰，需统一匹配
        /// </summary>
        private readonly Dictionary<string, string> m_LuaModuleABPathCache =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// 已确认在全部 Lua AB 路径中均不存在的模块名（负缓存，仅 AB 包模式）
        /// 避免对确实缺失的模块反复全量扫描（如 StartGame 重试场景）；不区分大小写同上
        /// </summary>
        private readonly HashSet<string> m_LuaModuleNotFoundCache =
            new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// 资源管理器的 AB 包加载管理器（收敛深层访问，简化调用）
        /// </summary>
        private AssetBundleLoadManager AssetBundleLoadManager =>
            HonorAssetManager.Instance.AssetLoadManager.AssetBundleLoadManager;

        /// <summary>
        /// Lua 脚本资源类型名（Luac 字节码模式 / 文本模式）
        /// </summary>
        private string LuaAssetTypeName => HonorUIManager.Instance.LuacMode ? "LuacAsset" : "LuaAsset";

        #endregion
    }
}