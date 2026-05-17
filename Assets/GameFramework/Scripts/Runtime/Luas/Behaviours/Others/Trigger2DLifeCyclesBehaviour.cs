/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  Trigger2DLifeCyclesBehaviour.cs
 * author:    云毅
 *created:   2026
 * descrip:   2D Trigger 事件生命周期转发脚本，负责转发至 Lua 层
 ***************************************************************/

using System;
using UnityEngine;
using XLua;

namespace Honor.Runtime
{
    /// <summary>
    /// 2D Trigger 事件生命周期转发脚本
    /// 负责将 Unity 2D 触发事件绑定并转发到 Lua 层
    /// </summary>
    public partial class Trigger2DLifeCyclesBehaviour : MonoBehaviour
    {
        //=========================================================================
        // 私有字段
        //=========================================================================
        #region Private Fields
        /// <summary>
        /// 触发进入 Lua 回调
        /// </summary>
        private Action<Collider2D> m_OnTriggerEnter2DCallback;

        /// <summary>
        /// 触发停留 Lua 回调
        /// </summary>
        private Action<Collider2D> m_OnTriggerStay2DCallback;

        /// <summary>
        /// 触发退出 Lua 回调
        /// </summary>
        private Action<Collider2D> m_OnTriggerExit2DCallback;
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
            luaEnv.Get(nameof(OnTriggerEnter2D), out m_OnTriggerEnter2DCallback);
            luaEnv.Get(nameof(OnTriggerStay2D), out m_OnTriggerStay2DCallback);
            luaEnv.Get(nameof(OnTriggerExit2D), out m_OnTriggerExit2DCallback);
        }

        /// <summary>
        /// 解除 Lua 绑定，防止内存泄漏
        /// </summary>
        public void LuaUnBinding()
        {
            m_OnTriggerEnter2DCallback = null;
            m_OnTriggerStay2DCallback = null;
            m_OnTriggerExit2DCallback = null;
        }
        #endregion

        //=========================================================================
        // 触发消息函数
        //=========================================================================
        #region Trigger Messages
        private void OnTriggerEnter2D(Collider2D other)
        {
            m_OnTriggerEnter2DCallback?.Invoke(other);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            m_OnTriggerStay2DCallback?.Invoke(other);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            m_OnTriggerExit2DCallback?.Invoke(other);
        }
        #endregion
    }
}