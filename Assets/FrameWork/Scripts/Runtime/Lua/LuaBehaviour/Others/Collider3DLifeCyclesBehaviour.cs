using System;
using UnityEngine;
using XLua;

namespace Honor.Runtime
{
    public partial class Collider3DLifeCyclesBehaviour : MonoBehaviour
    {
        /// <summary>
        /// 进入碰撞Lua回调
        /// </summary>
        private Action<Collision> m_OnCollisionEnter3DCallback;

        /// <summary>
        /// 停留碰撞Lua回调
        /// </summary>
        private Action<Collision> m_OnCollisionStay3DCallback;

        /// <summary>
        /// 退出碰撞Lua回调
        /// </summary>
        private Action<Collision> m_OnCollisionExit3DCallback;

        /// <summary>
        /// 绑定Lua
        /// </summary>
        /// <param name="luaEnv">Lua环境</param>
        public void LuaBinding(LuaTable luaEnv)
        {
            luaEnv.Get("OnCollisionEnter3D", out m_OnCollisionEnter3DCallback);
            luaEnv.Get("OnCollisionStay3D", out m_OnCollisionStay3DCallback);
            luaEnv.Get("OnCollisionExit3D", out m_OnCollisionExit3DCallback);
        }

        /// <summary>
        /// 进入碰撞
        /// </summary>
        /// <param name="collision">碰撞</param>
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
        /// 退出碰撞
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