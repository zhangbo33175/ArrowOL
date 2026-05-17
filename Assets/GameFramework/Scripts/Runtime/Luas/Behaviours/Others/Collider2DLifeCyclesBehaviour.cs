/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  Collider2DLifeCyclesBehaviour.cs
 * author:    云毅
 * created:   2026
 * descrip:   2D 碰撞事件生命周期转发脚本，负责转发至 Lua 层
 ***************************************************************/

using System;
using UnityEngine;
using XLua;

namespace Honor.Runtime
{
    /// <summary>
    /// 2D 碰撞事件生命周期转发脚本
    /// 负责将 Unity 2D 碰撞事件绑定并转发到 Lua 层
    /// </summary>
    public partial class Collider2DLifeCyclesBehaviour : MonoBehaviour
    {
        //=========================================================================
        // 私有字段
        //=========================================================================
        #region Private Fields
        /// <summary>
        /// 碰撞进入 Lua 回调
        /// </summary>
        private Action<Collision2D> m_OnCollisionEnter2DCallback;

        /// <summary>
        /// 碰撞停留 Lua 回调
        /// </summary>
        private Action<Collision2D> m_OnCollisionStay2DCallback;

        /// <summary>
        /// 碰撞退出 Lua 回调
        /// </summary>
        private Action<Collision2D> m_OnCollisionExit2DCallback;
        #endregion

        //=========================================================================
        // 公共方法
        //=========================================================================
        #region Public Methods
        /// <summary>
        /// 绑定 Lua 函数（自动匹配 OnCollisionEnter2D / Stay / Exit）
        /// </summary>
        /// <param name="luaEnv">Lua 表（通常是 Lua 脚本实例）</param>
        public void LuaBinding(LuaTable luaEnv)
        {
            luaEnv.Get("OnCollisionEnter2D", out m_OnCollisionEnter2DCallback);
            luaEnv.Get("OnCollisionStay2D", out m_OnCollisionStay2DCallback);
            luaEnv.Get("OnCollisionExit2D", out m_OnCollisionExit2DCallback);
        }

        /// <summary>
        /// 解除 Lua 绑定（防止内存泄漏）
        /// </summary>
        /// <param name="collision">碰撞</param>
        void OnCollisionEnter2D(Collision2D collision)
        {
            if(m_OnCollisionEnter2DCallback != null)
            {
                m_OnCollisionEnter2DCallback(collision);
            }
        }
        #endregion
        //=========================================================================
        // 碰撞方法
        //=========================================================================
        #region Public Methods
        /// <summary>
        /// 停留碰撞
        /// </summary>
        /// <param name="collision">碰撞</param>
        void OnCollisionStay2D(Collision2D collision)
        {
            if (m_OnCollisionStay2DCallback != null)
            {
                m_OnCollisionStay2DCallback(collision);
            }
        }

        /// <summary>
        /// 退出碰撞
        /// </summary>
        /// <param name="collision">碰撞</param>
        void OnCollisionExit2D(Collision2D collision)
        {
            if (m_OnCollisionExit2DCallback != null)
            {
                m_OnCollisionExit2DCallback(collision);
            }
        }

        #endregion
    }
}