#if BEST_HTTP_ENABLE
using BestHTTP.WebSocket;
#endif
using System.Collections.Generic;
using UnityEngine;

namespace Honor.Runtime
{
    public sealed partial class NetworkManager
    {
        /// <summary>
        /// Lua 组件引用
        /// 用于框架内 Lua 交互
        /// </summary>
        private LuaComponent m_LuaComponent;

        /// <summary>
        /// 网络连接超时时间（单位：秒）
        /// 默认：20 秒
        /// </summary>
        private float m_ConnectTimeout = 20f;

        /// <summary>
        /// 网络请求超时时间（单位：秒）
        /// 默认：60 秒
        /// </summary>
        private float m_RequestTimeout = 60f;

#if BEST_HTTP_ENABLE
        /// <summary>
        /// WebSocket 连接管理字典
        /// Key：连接名称
        /// Value：WebSocket 实例
        /// </summary>
        private Dictionary<string, WebSocket> m_WebSockets;
#endif
    }
}