/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  LuaBehaviour.Normal.cs
 * author:    云毅
 *created:   2026
 * descrip:   LuaBehaviour - 标准模式（None）专用字段与声明
 ***************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace Honor.Runtime
{
    public partial class LuaBehaviour
    {
        //=========================================================================
        // 标准模式 - 序列化字段
        //=========================================================================
        #region Normal Serialized Fields
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
        #endregion

        //=========================================================================
        // 标准模式 - 运行时数据
        //=========================================================================
        #region Normal Runtime Data
        /// <summary>
        /// 标准模式：Lua 独立运行环境（隔离作用域）
        /// 每个脚本独立环境，防止变量/函数冲突
        /// </summary>
        private LuaTable[] m_OwnLuaEnvsNone;

        /// <summary>
        /// 标准模式：Lua Class 实例
        /// </summary>
        private LuaTable[] m_OwnLuaClassesNone;
        #endregion

        //=========================================================================
        // 标准模式 - Lua 生命周期回调
        //=========================================================================
        #region Normal Lua Lifecycle Callbacks
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
        #endregion
    }
}