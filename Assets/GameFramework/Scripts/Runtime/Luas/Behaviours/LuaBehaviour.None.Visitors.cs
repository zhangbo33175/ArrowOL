using System;
using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace Honor.Runtime
{
    public partial class LuaBehaviour : MonoBehaviour
    {
        /// <summary>
        /// Lua脚本名称
        /// </summary>
        [SerializeField]
        private List<string> m_LuaScriptNamesNone;

        /// <summary>
        /// Lua父类脚本名称
        /// </summary>
        [SerializeField]
        private List<string> m_LuaSuperScriptNamesNone;

        /// <summary>
        /// Lua脚本独立环境
        /// 为每个脚本设置一个独立的环境，可一定程度上防止脚本间全局变量、函数冲突
        /// </summary>
        private LuaTable[] m_OwnLuaEnvsNone;

        /// <summary>
        /// Lua脚本class环境
        /// </summary>
        private LuaTable[] m_OwnLuaClassesNone;

        /// <summary>
        /// Lua生命周期函数：Awake
        /// </summary>
        private Action[] m_LuaAwakesNone;

        /// <summary>
        /// Lua生命周期函数：OnEnable
        /// </summary>
        private Action[] m_LuaOnEnablesNone;

        /// <summary>
        /// Lua生命周期函数：Start
        /// </summary>
        private Action[] m_LuaStartsNone;

        /// <summary>
        /// Lua自定义生命周期函数：Proc
        /// </summary>
        private Action[] m_LuaProcsNone;

        /// <summary>
        /// Lua生命周期函数：OnDisable
        /// </summary>
        private Action[] m_LuaOnDisablesNone;

        /// <summary>
        /// Lua生命周期函数：Destroy
        /// </summary>
        private Action[] m_LuaOnDestroysNone;

    }
}


