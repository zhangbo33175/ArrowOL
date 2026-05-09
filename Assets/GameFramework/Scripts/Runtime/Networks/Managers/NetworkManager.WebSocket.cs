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
        /// 创建并建立 WebSocket 连接
        /// 自动去重、绑定事件、开启心跳、管理连接池
        /// </summary>
        /// <param name="wsName">连接唯一名称</param>
        /// <param name="url">服务器地址</param>
        /// <returns>WebSocket 实例</returns>
        public WebSocket CreateWebSocketConnection(string wsName, string url)
        {
            // 连接已存在，直接返回
            if(m_WebSockets.ContainsKey(wsName))
            {
                Log.Warning($"[Network]WebSocket连接({wsName})已经存在，无需重建，直接返回。");
                return m_WebSockets[wsName];
            }

            // 创建 WebSocket 实例
            WebSocket ws = new WebSocket(new Uri(url));
            ws.Name = wsName;

#if !UNITY_WEBGL || UNITY_EDITOR
            // 非 WebGL 平台开启独立线程 Ping 保活
            ws.StartPingThread = true;

#if !BESTHTTP_DISABLE_PROXY
            // 代理配置
            if (HTTPManager.Proxy != null)
            {
                ws.OnInternalRequestCreated = (ws, internalRequest) => internalRequest.Proxy = new HTTPProxy(HTTPManager.Proxy.Address, HTTPManager.Proxy.Credentials, false);
            }
#endif
#endif
            // 注册所有生命周期事件
            ws.OnOpen += OnWebSocketOpen;
            ws.OnMessage += OnWebSocketMessageReceived;
            ws.OnBinary += OnWebSocketBinaryReceived;
            ws.OnClosed += OnWebSocketClosed;
            ws.OnError += OnWebSocketError;

            // 发起连接
            ws.Open();

            // 存入连接池
            m_WebSockets.Add(wsName, ws);

            return ws;
        }

        /// <summary>
        /// 关闭指定 WebSocket 连接
        /// 支持自定义关闭码与消息
        /// </summary>
        /// <param name="wsName">连接名称</param>
        /// <param name="code">关闭状态码</param>
        /// <param name="message">关闭消息</param>
        public void CloseWebSocketConnection(string wsName, UInt16 code, string message)
        {
            if (!m_WebSockets.ContainsKey(wsName))
            {
                Log.Warning($"[Network]WebSocket连接({wsName})不存在。");
                return;
            }

            // 使用默认或自定义参数关闭
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
        /// 发送文本消息
        /// </summary>
        /// <param name="wsName">连接名称</param>
        /// <param name="message">文本内容</param>
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
        /// 发送二进制数据
        /// </summary>
        /// <param name="wsName">连接名称</param>
        /// <param name="datas">字节数组</param>
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
        /// 获取已存在的 WebSocket 实例
        /// </summary>
        /// <param name="wsName">连接名称</param>
        /// <returns>WebSocket 实例</returns>
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
        /// WebSocket 连接成功回调
        /// </summary>
        private void OnWebSocketOpen(WebSocket ws)
        {
            Log.Info($"[Network]WebSocket连接({ws.Name})建立成功。");
            // 通知 Lua 层
            if(m_LuaComponent.LuaWebSocketOpenCSEventDelegate != null)
            {
                m_LuaComponent.LuaWebSocketOpenCSEventDelegate(ws);
            }
        }

        /// <summary>
        /// 收到文本消息回调
        /// </summary>
        private void OnWebSocketMessageReceived(WebSocket ws, string message)
        {
            Log.Info($"[Network]WebSocket连接({ws.Name})收到文本信息。Message = {message}。");
            if (m_LuaComponent.LuaWebSocketMessageReceivedCSEventDelegate != null)
            {
                m_LuaComponent.LuaWebSocketMessageReceivedCSEventDelegate(ws, message);
            }
        }

        /// <summary>
        /// 收到二进制消息回调
        /// </summary>
        private void OnWebSocketBinaryReceived(WebSocket ws, byte[] datas)
        {
            Log.Info($"[Network]WebSocket连接({ws.Name})收到字节流信息。Datas = {datas}。");
            if (m_LuaComponent.LuaWebSocketBinaryReceivedCSEventDelegate != null)
            {
                m_LuaComponent.LuaWebSocketBinaryReceivedCSEventDelegate(ws, datas);
            }
        }

        /// <summary>
        /// WebSocket 正常关闭回调
        /// 自动从连接池移除
        /// </summary>
        private void OnWebSocketClosed(WebSocket ws, UInt16 code, string message)
        {
            Log.Info($"[Network]WebSocket连接({ws.Name})连接关闭。Code = {code}，Message = {message}。");
            // 从管理字典中移除
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
        /// WebSocket 错误回调
        /// 自动断开并从连接池移除
        /// </summary>
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