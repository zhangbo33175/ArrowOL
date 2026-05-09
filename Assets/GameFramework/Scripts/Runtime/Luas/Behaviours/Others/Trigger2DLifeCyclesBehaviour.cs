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

        /// <summary>
        /// 绑定 Lua 函数
        /// </summary>
        /// <param name="luaEnv">Lua 表（脚本实例）</param>
        public void LuaBinding(LuaTable luaEnv)
        {
            luaEnv.Get("OnTriggerEnter2D", out m_OnTriggerEnter2DCallback);
            luaEnv.Get("OnTriggerStay2D", out m_OnTriggerStay2DCallback);
            luaEnv.Get("OnTriggerExit2D", out m_OnTriggerExit2DCallback);
        }

        /// <summary>
        /// 解除 Lua 绑定，防止内存泄漏
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
        /// 销毁时自动解绑
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


