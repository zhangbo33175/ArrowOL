using System;
using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace Honor.Runtime
{
    /// <summary>
    /// Lua 逻辑挂载脚本（MVVM 模式）
    /// 负责 Lua 脚本加载、生命周期管理、UI 数据绑定
    /// </summary>
    public partial class LuaBehaviour : MonoBehaviour
    {
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

        // ==============================================
        // Lua 生命周期回调（MVVM 多脚本支持）
        // ==============================================
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
    }
}