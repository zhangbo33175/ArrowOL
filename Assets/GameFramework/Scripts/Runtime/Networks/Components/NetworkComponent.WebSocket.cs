using System;
#if BEST_HTTP_ENABLE
using BestHTTP.WebSocket;
#endif

namespace Honor.Runtime
{
    public sealed partial class NetworkComponent : GameComponent
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
            if (string.IsNullOrEmpty(wsName))
            {
                Log.Error("NetworkComponent.CreateWebSocketConnection wsName 无效。");
                return null;
            }
            if (string.IsNullOrEmpty(url))
            {
                Log.Error("NetworkComponent.CreateWebSocketConnection url 无效。");
                return null;
            }
            return m_NetworkManager.CreateWebSocketConnection(wsName, url);
        }

        /// <summary>
        /// 关闭WebSocket连接
        /// </summary>
        /// <param name="wsName">WebSocket名称</param>
        /// <param name="code">附带WebSocket-code</param>
        /// <param name="message">附带WebSocket文本信息</param>
        public void CloseWebSocketConnection(string wsName, UInt16 code = default(UInt16), string message = null)
        {
            if (string.IsNullOrEmpty(wsName))
            {
                Log.Error("NetworkComponent.CloseWebSocketConnection wsName 无效。");
                return;
            }
            m_NetworkManager.CloseWebSocketConnection(wsName, code, message);
        }

        /// <summary>
        /// 发送文本信息
        /// </summary>
        /// <param name="wsName">WebSocket名称</param>
        /// <param name="message">发送的文本信息</param>
        public void SendWebSocketMessage(string wsName, string message)
        {
            if (string.IsNullOrEmpty(wsName))
            {
                Log.Error("NetworkComponent.SendWebSocketMessage wsName 无效。");
                return;
            }
            m_NetworkManager.SendWebSocketMessage(wsName, message);
        }

        /// <summary>
        /// 发送字节流信息
        /// </summary>
        /// <param name="wsName">WebSocket名称</param>
        /// <param name="datas">发送的字节流信息</param>
        public void SendWebSocketBinary(string wsName, byte[] datas)
        {
            if (string.IsNullOrEmpty(wsName))
            {
                Log.Error("NetworkComponent.SendWebSocketBinary wsName 无效。");
                return;
            }
            m_NetworkManager.SendWebSocketBinary(wsName, datas);
        }

        /// <summary>
        /// 获取WebSocket连接
        /// </summary>
        /// <param name="wsName">WebSocket名称</param>
        /// <returns></returns>
        public WebSocket GetWebSocket(string wsName)
        {
            if (string.IsNullOrEmpty(wsName))
            {
                Log.Error("NetworkComponent.GetWebSocket wsName 无效。");
                return null;
            }
            return m_NetworkManager.GetWebSocket(wsName);
        }
#endif
    }

}


