using System;
using System.Collections;
using System.Collections.Generic;
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

        /// <summary>
        /// 绑定 Lua 函数
        /// </summary>
        /// <param name="luaEnv">Lua 表（脚本实例）</param>
        public void LuaBinding(LuaTable luaEnv)
        {
            luaEnv.Get("OnTriggerEnter3D", out m_OnTriggerEnter3DCallback);
            luaEnv.Get("OnTriggerStay3D", out m_OnTriggerStay3DCallback);
            luaEnv.Get("OnTriggerExit3D", out m_OnTriggerExit3DCallback);
        }

        /// <summary>
        /// 解除 Lua 绑定，防止内存泄漏
        /// </summary>
        /// <param name="other">对方碰撞器</param>
        private void OnTriggerEnter(Collider other)
        {
            if (m_OnTriggerEnter3DCallback != null)
            {
                m_OnTriggerEnter3DCallback(other);
            }
        }

        /// <summary>
        /// 停留碰撞
        /// </summary>
        /// <param name="other">对方碰撞器</param>
        void OnTriggerStay(Collider other)
        {
            if (m_OnTriggerStay3DCallback != null)
            {
                m_OnTriggerStay3DCallback(other);
            }
        }

        /// <summary>
        /// 销毁时自动解绑
        /// </summary>
        /// <param name="other">对方碰撞器</param>
        void OnTriggerExit(Collider other)
        {
            if (m_OnTriggerExit3DCallback != null)
            {
                m_OnTriggerExit3DCallback(other);
            }
        }

    }
}


