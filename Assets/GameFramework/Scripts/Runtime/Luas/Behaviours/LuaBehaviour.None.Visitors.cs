using System;
using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace Honor.Runtime
{
    public partial class LuaBehaviour : MonoBehaviour
    {
        /// <summary>
        /// 标准模式：Lua 脚本名称列表
        /// </summary>
        [SerializeField]
        private List<string> m_LuaScriptNamesNone;

        /// <summary>
        /// 标准模式：Lua 父类脚本名称列表
        /// </summary>
        [SerializeField]
        private List<string> m_LuaSuperScriptNamesNone;

        /// <summary>
        /// 标准模式：Lua 独立运行环境（隔离作用域）
        /// 每个脚本独立环境，防止变量/函数冲突
        /// </summary>
        private LuaTable[] m_OwnLuaEnvsNone;

        /// <summary>
        /// 标准模式：Lua Class 实例
        /// </summary>
        private LuaTable[] m_OwnLuaClassesNone;

        // ==============================================
        // Lua 生命周期回调（标准模式）
        // ==============================================
        /// <summary>
        /// Lua 生命周期：Awake
        /// </summary>
        private Action[] m_LuaAwakesNone;

        /// <summary>
        /// Lua 生命周期：OnEnable
        /// </summary>
        private Action[] m_LuaOnEnablesNone;

        /// <summary>
        /// Lua 生命周期：Start
        /// </summary>
        private Action[] m_LuaStartsNone;

        /// <summary>
        /// Lua 自定义逻辑：Proc（逻辑帧更新）
        /// </summary>
        private Action[] m_LuaProcsNone;

        /// <summary>
        /// Lua 生命周期：OnDisable
        /// </summary>
        private Action[] m_LuaOnDisablesNone;

        /// <summary>
        /// Lua 生命周期：OnDestroy
        /// </summary>
        private Action[] m_LuaOnDestroysNone;
    }
}