using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace Honor.Runtime
{
    public partial class Trigger3DLifeCyclesBehaviour : MonoBehaviour
    {
        /// <summary>
        /// 进入碰撞Lua回调
        /// </summary>
        private Action<Collider> m_OnTriggerEnter3DCallback;

        /// <summary>
        /// 停留碰撞Lua回调
        /// </summary>
        private Action<Collider> m_OnTriggerStay3DCallback;

        /// <summary>
        /// 退出碰撞Lua回调
        /// </summary>
        private Action<Collider> m_OnTriggerExit3DCallback;

        /// <summary>
        /// 绑定Lua
        /// </summary>
        /// <param name="luaEnv">Lua环境</param>
        public void LuaBinding(LuaTable luaEnv)
        {
            luaEnv.Get("OnTriggerEnter3D", out m_OnTriggerEnter3DCallback);
            luaEnv.Get("OnTriggerStay3D", out m_OnTriggerStay3DCallback);
            luaEnv.Get("OnTriggerExit3D", out m_OnTriggerExit3DCallback);
        }

        /// <summary>
        /// 进入碰撞
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
        /// 退出碰撞
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


