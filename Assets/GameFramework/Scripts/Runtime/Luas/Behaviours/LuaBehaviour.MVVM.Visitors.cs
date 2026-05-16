/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  LuaBehaviour.MVVM.cs
 * author:    云毅
 * created:   2026 2025
 * descrip:   LuaBehaviour - MVVM 模式专用字段与声明
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
        // MVVM 模式 - 序列化字段
        //=========================================================================
        #region MVVM Serialized Fields
        /// <summary>
        /// MVVM 模式：Lua 脚本公共名称（配置用）
        /// </summary>
        [SerializeField]
        private string m_LuaScriptCommonNameMVVM;

        /// <summary>
        /// MVVM 模式：Lua 脚本名称列表
        /// </summary>
        [SerializeField]
        private List<string> m_LuaScriptNamesMVVM;

        /// <summary>
        /// MVVM 模式：Lua 父类脚本名称列表
        /// </summary>
        [SerializeField]
        private List<string> m_LuaSuperScriptNamesMVVM;

        /// <summary>
        /// 绑定数据集合（Inspector 配置）
        /// </summary>
        [SerializeField]
        private List<LuaBindValue> m_BindValues;
        #endregion

        //=========================================================================
        // MVVM 模式 - 运行时数据
        //=========================================================================
        #region MVVM Runtime Data
        /// <summary>
        /// 绑定数据集合（只读）
        /// </summary>
        public List<LuaBindValue> BindValues => m_BindValues;

        /// <summary>
        /// MVVM 模式：Lua 独立运行环境（隔离作用域）
        /// 每个脚本独立环境，防止变量/函数冲突
        /// </summary>
        private LuaTable[] m_OwnLuaEnvsMVVM;

        /// <summary>
        /// MVVM 模式：Lua Class 实例
        /// </summary>
        private LuaTable[] m_OwnLuaClassesMVVM;
        #endregion

        //=========================================================================
        // MVVM 模式 - Lua 生命周期回调
        //=========================================================================
        #region MVVM Lua Lifecycle Callbacks
        /// <summary>
        /// Lua 生命周期：Awake
        /// </summary>
        private Action[] m_LuaAwakesMVVM;

        /// <summary>
        /// Lua 生命周期：OnEnable
        /// </summary>
        private Action[] m_LuaOnEnablesMVVM;

        /// <summary>
        /// Lua 生命周期：Start
        /// </summary>
        private Action[] m_LuaStartsMVVM;

        /// <summary>
        /// Lua 自定义逻辑：Proc（逻辑帧更新）
        /// </summary>
        private Action[] m_LuaProcsMVVM;

        /// <summary>
        /// Lua 生命周期：OnDisable
        /// </summary>
        private Action[] m_LuaOnDisablesMVVM;

        /// <summary>
        /// Lua 生命周期：OnDestroy
        /// </summary>
        private Action[] m_LuaOnDestroysMVVM;
        #endregion
    }
}