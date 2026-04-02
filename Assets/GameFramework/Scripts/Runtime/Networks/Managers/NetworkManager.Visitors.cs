/***************************************************************
 * (c) copyright 2021 - 2025, Honor.Game
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  NetworkManager.Visitors.cs
 * author:    taoye
 * created:   2022/10/12
 * descrip:   Network管理器-访问器
 ***************************************************************/
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
        /// Lua组件
        /// </summary>
        private LuaComponent m_LuaComponent;

        /// <summary>
        /// 默认网络连接超时时间
        /// 默认时间20s
        /// </summary>
        private float m_ConnectTimeout = 20f;

        /// <summary>
        /// 默认网络请求超时时间
        /// 默认时间60s
        /// </summary>
        private float m_RequestTimeout = 60f;

#if BEST_HTTP_ENABLE
        /// <summary>
        /// WebSocket实例
        /// </summary>
        private Dictionary<string, WebSocket> m_WebSockets;
#endif
    }

}
