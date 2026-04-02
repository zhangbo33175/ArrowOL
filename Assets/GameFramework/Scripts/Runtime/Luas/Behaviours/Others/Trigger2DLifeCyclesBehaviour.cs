using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace Honor.Runtime
{
    public partial class Trigger2DLifeCyclesBehaviour : MonoBehaviour
    {
        /// <summary>
        /// 进入碰撞Lua回调
        /// </summary>
        private Action<Collider2D> m_OnTriggerEnter2DCallback;

        /// <summary>
        /// 停留碰撞Lua回调
        /// </summary>
        private Action<Collider2D> m_OnTriggerStay2DCallback;

        /// <summary>
        /// 退出碰撞Lua回调
        /// </summary>
        private Action<Collider2D> m_OnTriggerExit2DCallback;

        /// <summary>
        /// 绑定Lua
        /// </summary>
        /// <param name="luaEnv">Lua环境</param>
        public void LuaBinding(LuaTable luaEnv)
        {
            luaEnv.Get("OnTriggerEnter2D", out m_OnTriggerEnter2DCallback);
            luaEnv.Get("OnTriggerStay2D", out m_OnTriggerStay2DCallback);
            luaEnv.Get("OnTriggerExit2D", out m_OnTriggerExit2DCallback);
        }

        /// <summary>
        /// 进入碰撞
        /// </summary>
        /// <param name="other">对方碰撞器</param>
        void OnTriggerEnter2D(Collider2D other)
        {
            if (m_OnTriggerEnter2DCallback != null)
            {
                m_OnTriggerEnter2DCallback(other);
            }
        }

        /// <summary>
        /// 停留碰撞
        /// </summary>
        /// <param name="other">对方碰撞器</param>
        void OnTriggerStay2D(Collider2D other)
        {
            if (m_OnTriggerStay2DCallback != null)
            {
                m_OnTriggerStay2DCallback(other);
            }
        }

        /// <summary>
        /// 退出碰撞
        /// </summary>
        /// <param name="other">对方碰撞器</param>
        void OnTriggerExit2D(Collider2D other)
        {
            if (m_OnTriggerExit2DCallback != null)
            {
                m_OnTriggerExit2DCallback(other);
            }
        }

    }
}


