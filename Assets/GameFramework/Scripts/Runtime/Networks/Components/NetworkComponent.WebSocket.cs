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
        /// 创建并建立 WebSocket 长连接
        /// 底层交由 NetworkManager 管理
        /// </summary>
        /// <param name="wsName">WebSocket 唯一名称（用于区分不同连接）</param>
        /// <param name="url">WebSocket 服务器地址</param>
        /// <returns>WebSocket 实例</returns>
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
        /// 关闭指定的 WebSocket 连接
        /// </summary>
        /// <param name="wsName">WebSocket 唯一名称</param>
        /// <param name="code">关闭状态码（默认 1000 正常关闭）</param>
        /// <param name="message">关闭附带消息</param>
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
        /// 通过 WebSocket 发送文本消息
        /// </summary>
        /// <param name="wsName">WebSocket 唯一名称</param>
        /// <param name="message">文本内容</param>
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
        /// 通过 WebSocket 发送二进制数据
        /// </summary>
        /// <param name="wsName">WebSocket 唯一名称</param>
        /// <param name="datas">二进制字节数组</param>
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
        /// 获取已创建的 WebSocket 实例
        /// </summary>
        /// <param name="wsName">WebSocket 唯一名称</param>
        /// <returns>WebSocket 实例，不存在则返回 null</returns>
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