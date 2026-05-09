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

        /// <summary>
        /// 绑定 Lua 函数
        /// </summary>
        /// <param name="luaEnv">Lua 表（脚本实例）</param>
        public void LuaBinding(LuaTable luaEnv)
        {
            luaEnv.Get("OnCollisionEnter3D", out m_OnCollisionEnter3DCallback);
            luaEnv.Get("OnCollisionStay3D", out m_OnCollisionStay3DCallback);
            luaEnv.Get("OnCollisionExit3D", out m_OnCollisionExit3DCallback);
        }

        /// <summary>
        /// 解除 Lua 绑定，防止内存泄漏
        /// </summary>
        void OnCollisionEnter(Collision collision)
        {
            if (m_OnCollisionEnter3DCallback != null)
            {
                m_OnCollisionEnter3DCallback(collision);
            }
        }

        /// <summary>
        /// 停留碰撞
        /// </summary>
        /// <param name="collision">碰撞</param>
        void OnCollisionStay(Collision collision)
        {
            if (m_OnCollisionStay3DCallback != null)
            {
                m_OnCollisionStay3DCallback(collision);
            }
        }

        /// <summary>
        /// 销毁时自动解绑
        /// </summary>
        /// <param name="collision">碰撞</param>
        void OnCollisionExit(Collision collision)
        {
            if (m_OnCollisionExit3DCallback != null)
            {
                m_OnCollisionExit3DCallback(collision);
            }
        }

    }
}


