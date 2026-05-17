/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  Collider3DLifeCyclesBehaviour.cs
 * author:    云毅
 * created:   2026
 * descrip:   3D 碰撞事件生命周期转发脚本，负责转发至 Lua 层
 ***************************************************************/

using System;
using UnityEngine;
using XLua;

namespace Honor.Runtime
{
    /// <summary>
    /// 3D 碰撞事件生命周期转发脚本
    /// 负责将 Unity 3D 碰撞事件绑定并转发到 Lua 层
    /// </summary>
    public partial class Collider3DLifeCyclesBehaviour : MonoBehaviour
    {
        //=========================================================================
        // 私有字段
        //=========================================================================
        #region Private Fields
        /// <summary>
        /// 碰撞进入 Lua 回调
        /// </summary>
        private Action<Collision> m_OnCollisionEnter3DCallback;

        /// <summary>
        /// 碰撞停留 Lua 回调
        /// </summary>
        private Action<Collision> m_OnCollisionStay3DCallback;

        /// <summary>
        /// 碰撞退出 Lua 回调
        /// </summary>
        private Action<Collision> m_OnCollisionExit3DCallback;
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
            luaEnv.Get(nameof(OnCollisionEnter), out m_OnCollisionEnter3DCallback);
            luaEnv.Get(nameof(OnCollisionStay), out m_OnCollisionStay3DCallback);
            luaEnv.Get(nameof(OnCollisionExit), out m_OnCollisionExit3DCallback);
        }

        /// <summary>
        /// 解除 Lua 绑定，防止内存泄漏
        /// </summary>
        public void LuaUnBinding()
        {
            m_OnCollisionEnter3DCallback = null;
            m_OnCollisionStay3DCallback = null;
            m_OnCollisionExit3DCallback = null;
        }
        #endregion

        //=========================================================================
        // 碰撞消息函数
        //=========================================================================
        #region Collision Messages
        private void OnCollisionEnter(Collision collision)
        {
            m_OnCollisionEnter3DCallback?.Invoke(collision);
        }

        private void OnCollisionStay(Collision collision)
        {
            m_OnCollisionStay3DCallback?.Invoke(collision);
        }

        private void OnCollisionExit(Collision collision)
        {
            m_OnCollisionExit3DCallback?.Invoke(collision);
        }
        #endregion
    }
}