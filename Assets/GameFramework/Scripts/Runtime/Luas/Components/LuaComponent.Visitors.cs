using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace Honor.Runtime
{
    public sealed partial class LuaComponent : GameComponent
    {
        /// <summary>
        /// Luac 字节码加密密钥
        /// </summary>
        public static byte[] s_LuacEncrytionKey = { 110, 52, 63, 5, 7, 13, 245, 206, 178, 221, 143, 135, 53, 41, 29, 3, 197, 77, 103, 82 };

        /// <summary>
        /// Lua 运行时性能分析模式（仅编辑器生效）
        /// </summary>
        [SerializeField]
        private bool m_LuaRuntimeProfilerMode;
        public bool LuaRuntimeProfilerMode
        {
            get
            {
                return m_LuaRuntimeProfilerMode;
            }
        }

        /// <summary>
        /// 最近一次 Lua GC 调用时间
        /// </summary>
        internal static float m_LastGCTime;

        /// <summary>
        /// Lua GC 调用间隔（秒）
        /// </summary>
        internal const float m_GCInterval = 1f;

        /// <summary>
        /// 已加载 Lua 脚本名称列表
        /// </summary>
        private List<string> m_LoadedLuaScriptsNames;
        public List<string> LoadedLuaScriptsNames
        {
            get
            {
                return m_LoadedLuaScriptsNames;
            }
        }


        /// <summary>
        /// 全局唯一 Lua 运行环境
        /// </summary>
        private LuaEnv m_Env;
        public LuaEnv Env
        {
            get
            {
                return m_Env;
            }
        }


        /// <summary>
        /// Launcher组件
        /// </summary>
        private LauncherComponent m_LauncherComponent;

        /// <summary>
        /// Asset组件
        /// </summary>
        private AssetComponent m_AssetComponent;

        /// <summary>
        /// Procedure组件
        /// </summary>
        private ProcedureComponent m_ProcedureComponent;

        /// <summary>
        /// 所有Lua脚本的绝对路径映射（仅Editor资源加载模式下有效）
        /// </summary>
        private Dictionary<string, string> m_LuaScriptsFullPathsMapForLoading;
        
        /// <summary>
        /// 可加载ABPath队列（仅AB资源加载模式下有效）
        /// </summary>
        private List<string> m_LuaScriptsABPathsForLoading;

    }

}


