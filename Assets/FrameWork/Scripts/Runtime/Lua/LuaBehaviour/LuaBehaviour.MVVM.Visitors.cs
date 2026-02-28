using System;
using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace Honor.Runtime
{
    public partial class LuaBehaviour : MonoBehaviour
    {
        /// <summary>
        /// MVVM模式下Lua脚本公有名称
        /// </summary>
        [SerializeField]
        private string m_LuaScriptCommonNameMVVM;

        /// <summary>
        /// MVVM模式下Lua脚本名称
        /// </summary>
        [SerializeField]
        private List<string> m_LuaScriptNamesMVVM;

        /// <summary>
        /// MVVM模式下Lua父类脚本名称
        /// </summary>
        [SerializeField]
        private List<string> m_LuaSuperScriptNamesMVVM;

        /// <summary>
        /// 绑定数据集合
        /// </summary>
        [SerializeField]
        private List<LuaBindValue> m_BindValues;
        public List<LuaBindValue> BindValues
        {
            get
            {
                return m_BindValues;
            }
        }

        /// <summary>
        /// MVVM模式下的Lua脚本独立环境
        /// 为每个脚本设置一个独立的环境，可一定程度上防止脚本间全局变量、函数冲突
        /// </summary>
        private LuaTable[] m_OwnLuaEnvsMVVM;

        /// <summary>
        /// MVVM模式下的Lua脚本class环境
        /// </summary>
        private LuaTable[] m_OwnLuaClassesMVVM;

        /// <summary>
        /// MVVM模式下的Lua生命周期函数：Awake
        /// </summary>
        private Action[] m_LuaAwakesMVVM;

        /// <summary>
        /// MVVM模式下的Lua生命周期函数：OnEnable
        /// </summary>
        private Action[] m_LuaOnEnablesMVVM;

        /// <summary>
        /// MVVM模式下的Lua生命周期函数：Start
        /// </summary>
        private Action[] m_LuaStartsMVVM;

        /// <summary>
        /// MVVM模式下的Lua自定义生命周期函数：Proc
        /// </summary>
        private Action[] m_LuaProcsMVVM;

        /// <summary>
        /// MVVM模式下的Lua生命周期函数：OnDisable
        /// </summary>
        private Action[] m_LuaOnDisablesMVVM;

        /// <summary>
        /// MVVM模式下的Lua生命周期函数：Destroy
        /// </summary>
        private Action[] m_LuaOnDestroysMVVM;

    }
}