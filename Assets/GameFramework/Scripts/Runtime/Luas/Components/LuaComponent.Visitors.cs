/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  LuaComponent.Fields.cs
 * author:    云毅
 * created:   2026
 * descrip:   LuaComponent 字段、常量、属性定义模块
 ***************************************************************/

using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace Honor.Runtime
{
    /// <summary>
    /// Lua 核心组件 - 字段与属性定义部分
    /// 包含 Lua 环境、配置、缓存、委托、全局状态等所有成员定义
    /// </summary>
    public sealed partial class LuaComponent : GameComponent
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
        internal const float m_GCInterval = 1f;

        #endregion

        //=========================================================================
        #region 运行时状态
        //=========================================================================

        /// <summary>
        /// 最近一次 Lua GC 调用时间
        /// </summary>
        internal static float m_LastGCTime;

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
        /// 启动器组件引用
        /// </summary>
        private LauncherComponent m_LauncherComponent;

        /// <summary>
        /// 资源管理组件引用
        /// </summary>
        private AssetComponent m_AssetComponent;

        /// <summary>
        /// 流程管理组件引用
        /// </summary>
        private ProcedureComponent m_ProcedureComponent;

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

        #endregion
    }
}