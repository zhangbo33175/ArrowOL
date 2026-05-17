/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  Trigger3DLifeCyclesBehaviour.cs
 * author:    云毅
 *created:   2026
 * descrip:   3D Trigger 事件生命周期转发脚本，负责转发至 Lua 层
 ***************************************************************/

using System;
using UnityEngine;
using XLua;

namespace Honor.Runtime
{
    /// <summary>
    /// 3D Trigger 事件生命周期转发脚本
    /// 负责将 Unity 3D 触发事件绑定并转发到 Lua 层
    /// </summary>
    public partial class Trigger3DLifeCyclesBehaviour : MonoBehaviour
    {
        //=========================================================================
        // 私有字段
        //=========================================================================
        #region Private Fields
        /// <summary>
        /// 触发进入 Lua 回调
        /// </summary>
        private Action<Collider> m_OnTriggerEnter3DCallback;

        /// <summary>
        /// 触发停留 Lua 回调
        /// </summary>
        private Action<Collider> m_OnTriggerStay3DCallback;

        /// <summary>
        /// 触发退出 Lua 回调
        /// </summary>
        private Action<Collider> m_OnTriggerExit3DCallback;
        #endregion

        //=========================================================================
        // 公共方法
        //=========================================================================
        #region Public Methods
        /// <summary>
        /// 绑定 Lua 函数
        /// </summary>
        /// <param name="luaEnv">Lua 表（脚本实例）</param>
        public void LuaBinding(LuaTable luaEnv)
        {
            luaEnv.Get(nameof(OnTriggerEnter), out m_OnTriggerEnter3DCallback);
            luaEnv.Get(nameof(OnTriggerStay), out m_OnTriggerStay3DCallback);
            luaEnv.Get(nameof(OnTriggerExit), out m_OnTriggerExit3DCallback);
        }

        /// <summary>
        /// 解除 Lua 绑定，防止内存泄漏
        /// </summary>
        public void LuaUnBinding()
        {
            m_OnTriggerEnter3DCallback = null;
            m_OnTriggerStay3DCallback = null;
            m_OnTriggerExit3DCallback = null;
        }
        #endregion

        //=========================================================================
        // 触发消息函数
        //=========================================================================
        #region Trigger Messages
        private void OnTriggerEnter(Collider other)
        {
            m_OnTriggerEnter3DCallback?.Invoke(other);
        }

        private void OnTriggerStay(Collider other)
        {
            m_OnTriggerStay3DCallback?.Invoke(other);
        }

        private void OnTriggerExit(Collider other)
        {
            m_OnTriggerExit3DCallback?.Invoke(other);
        }
        #endregion
    }
}