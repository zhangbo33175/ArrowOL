using System;
using UnityEngine;
using XLua;

namespace Honor.Runtime
{
    public partial class Collider2DLifeCyclesBehaviour : MonoBehaviour
    {
        /// <summary>
        /// 进入碰撞Lua回调
        /// </summary>
        private Action<Collision2D> m_OnCollisionEnter2DCallback;

        /// <summary>
        /// 停留碰撞Lua回调
        /// </summary>
        private Action<Collision2D> m_OnCollisionStay2DCallback;

        /// <summary>
        /// 退出碰撞Lua回调
        /// </summary>
        private Action<Collision2D> m_OnCollisionExit2DCallback;

        /// <summary>
        /// 绑定Lua
        /// </summary>
        /// <param name="luaEnv">Lua环境</param>
        public void LuaBinding(LuaTable luaEnv)
        {
            luaEnv.Get("OnCollisionEnter2D", out m_OnCollisionEnter2DCallback);
            luaEnv.Get("OnCollisionStay2D", out m_OnCollisionStay2DCallback);
            luaEnv.Get("OnCollisionExit2D", out m_OnCollisionExit2DCallback);
        }

        /// <summary>
        /// 进入碰撞
        /// </summary>
        /// <param name="collision">碰撞</param>
        void OnCollisionEnter2D(Collision2D collision)
        {
            if(m_OnCollisionEnter2DCallback != null)
            {
                m_OnCollisionEnter2DCallback(collision);
            }
        }

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

    }
}


