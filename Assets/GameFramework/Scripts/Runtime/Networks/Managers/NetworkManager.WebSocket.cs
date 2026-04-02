/***************************************************************
 * (c) copyright 2021 - 2025, Honor.Game
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  NetworkManager.WebSocket.cs
 * author:    taoye
 * created:   2022/10/12
 * descrip:   Network管理器-WebSocket链接接口（长连接）
 ***************************************************************/
#if BEST_HTTP_ENABLE
using BestHTTP;
using BestHTTP.WebSocket;
#endif
using System;

namespace Honor.Runtime
{
    public sealed partial class NetworkManager
    {
#if BEST_HTTP_ENABLE
        /// <summary>
        /// 建立WebSocket连接
        /// </summary>
        /// <param name="wsName">WebSocket名称</param>
        /// <param name="url">服务器url</param>
        /// <returns></returns>
        public WebSocket CreateWebSocketConnection(string wsName, string url)
        {
            if(m_WebSockets.ContainsKey(wsName))
            {
                Log.Warning($"[Network]WebSocket连接({wsName})已经存在，无需重建，直接返回。");
                return m_WebSockets[wsName];
            }

            WebSocket ws = new WebSocket(new Uri(url));

            ws.Name = wsName;

#if !UNITY_WEBGL || UNITY_EDITOR

            // 开启一个新的线程Ping服务端
            ws.StartPingThread = true;

#if !BESTHTTP_DISABLE_PROXY
            if (HTTPManager.Proxy != null)
            {
                ws.OnInternalRequestCreated = (ws, internalRequest) => internalRequest.Proxy = new HTTPProxy(HTTPManager.Proxy.Address, HTTPManager.Proxy.Credentials, false);
            }
#endif
#endif
            // 注册WebSocket事件监听
            ws.OnOpen += OnWebSocketOpen;
            ws.OnMessage += OnWebSocketMessageReceived;
            ws.OnBinary += OnWebSocketBinaryReceived;
            ws.OnClosed += OnWebSocketClosed;
            ws.OnError += OnWebSocketError;

            // 连接服务器
            ws.Open();

            // 记录ws
            m_WebSockets.Add(wsName, ws);

            return ws;
        }

        /// <summary>
        /// 关闭WebSocket连接连接
        /// </summary>
        /// <param name="wsName">WebSocket名称</param>
        /// <param name="code">附带WebSocket-code</param>
        /// <param name="message">附带WebSocket文本信息</param>
        public void CloseWebSocketConnection(string wsName, UInt16 code, string message)
        {
            if (!m_WebSockets.ContainsKey(wsName))
            {
                Log.Warning($"[Network]WebSocket连接({wsName})不存在。");
                return;
            }

            if (code == default(UInt16) && string.IsNullOrEmpty(message))
            {
                m_WebSockets[wsName].Close();
            }
            else
            {
                m_WebSockets[wsName].Close(code, message);
            }
        }

        /// <summary>
        /// 发送文本信息
        /// </summary>
        /// <param name="wsName">WebSocket名称</param>
        /// <param name="message">发送的文本信息</param>
        public void SendWebSocketMessage(string wsName, string message)
        {
            if (!m_WebSockets.ContainsKey(wsName))
            {
                Log.Error($"[Network]WebSocket连接({wsName})不存在。");
                return;
            }
            m_WebSockets[wsName].Send(message);
        }

        /// <summary>
        /// 发送字节流信息
        /// </summary>
        /// <param name="wsName">WebSocket名称</param>
        /// <param name="datas">发送的字节流信息</param>
        public void SendWebSocketBinary(string wsName, byte[] datas)
        {
            if (!m_WebSockets.ContainsKey(wsName))
            {
                Log.Error($"[Network]WebSocket连接({wsName})不存在。");
                return;
            }
            m_WebSockets[wsName].Send(datas);
        }

        /// <summary>
        /// 获取WebSocket连接
        /// </summary>
        /// <param name="wsName">WebSocket名称</param>
        /// <returns></returns>
        public WebSocket GetWebSocket(string wsName)
        {
            if (!m_WebSockets.ContainsKey(wsName))
            {
                Log.Warning($"[Network]WebSocket连接({wsName})不存在。");
                return null;
            }
            return m_WebSockets[wsName];
        }

        /// <summary>
        /// WebSocket连接建立成功回调
        /// </summary>
        /// <param name="ws">WebSocket连接</param>
        private void OnWebSocketOpen(WebSocket ws)
        {
            Log.Info($"[Network]WebSocket连接({ws.Name})建立成功。");
            if(m_LuaComponent.LuaWebSocketOpenCSEventDelegate != null)
            {
                m_LuaComponent.LuaWebSocketOpenCSEventDelegate(ws);
            }
        }

        /// <summary>
        /// 本文信息接收回调
        /// </summary>
        /// <param name="ws">WebSocket连接</param>
        /// <param name="message">接收到的文本信息</param>
        private void OnWebSocketMessageReceived(WebSocket ws, string message)
        {
            Log.Info($"[Network]WebSocket连接({ws.Name})收到文本信息。Message = {message}。");
            if (m_LuaComponent.LuaWebSocketMessageReceivedCSEventDelegate != null)
            {
                m_LuaComponent.LuaWebSocketMessageReceivedCSEventDelegate(ws, message);
            }
        }

        /// <summary>
        /// 字节流信息接收回调
        /// </summary>
        /// <param name="ws">WebSocket连接</param>
        /// <param name="datas">接收到的字节流信息</param>
        private void OnWebSocketBinaryReceived(WebSocket ws, byte[] datas)
        {
            Log.Info($"[Network]WebSocket连接({ws.Name})收到字节流信息。Datas = {datas}。");
            if (m_LuaComponent.LuaWebSocketBinaryReceivedCSEventDelegate != null)
            {
                m_LuaComponent.LuaWebSocketBinaryReceivedCSEventDelegate(ws, datas);
            }
        }

        /// <summary>
        /// WebSocket关闭回调
        /// </summary>
        /// <param name="ws">WebSocket连接</param>
        /// <param name="code">附带WebSocket-code</param>
        /// <param name="message">附带WebSocket文本信息</param>
        private void OnWebSocketClosed(WebSocket ws, UInt16 code, string message)
        {
            Log.Info($"[Network]WebSocket连接({ws.Name})连接关闭。Code = {code}，Message = {message}。");
            if(m_WebSockets.ContainsKey(ws.Name))
            {
                m_WebSockets.Remove(ws.Name);
            }
            if (m_LuaComponent.LuaWebSocketClosedCSEventDelegate != null)
            {
                m_LuaComponent.LuaWebSocketClosedCSEventDelegate(ws, code, message);
            }
        }

        /// <summary>
        /// WebScoket错误回调
        /// </summary>
        /// <param name="ws">WebSocket连接</param>
        /// <param name="error">错误信息</param>
        private void OnWebSocketError(WebSocket ws, string error)
        {
            Log.Warning($"[Network]WebSocket连接({ws.Name})发生错误。Error = {error}。");
            if (m_WebSockets.ContainsKey(ws.Name))
            {
                m_WebSockets.Remove(ws.Name);
            }
            if (m_LuaComponent.LuaWebSocketErrorCSEventDelegate != null)
            {
                m_LuaComponent.LuaWebSocketErrorCSEventDelegate(ws, error);
            }
        }
#endif
    }
}
