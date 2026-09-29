/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  NetworkComponent.WebSocket.cs
 * author:    云毅
 * created:   2026
 * descrip:   网络组件 - WebSocket 接口层（基于 BestHTTP）
 ***************************************************************/

using System;
#if BEST_HTTP_ENABLE
using BestHTTP.WebSocket;
#endif

namespace Honor.Runtime
{
    /// <summary>
    /// 网络组件 - WebSocket 长连接接口模块
    /// </summary>
    public sealed partial class NetworkComponent : GameComponent
    {
        //=========================================================================
        #region WebSocket 接口（BestHTTP）
        //=========================================================================

#if BEST_HTTP_ENABLE

        /// <summary>
        /// 校验连接名是否有效
        /// </summary>
        /// <param name="wsName">连接名称</param>
        /// <returns>有效返回 true，否则记录错误并返回 false</returns>
        private bool EnsureSocketName(string wsName)
        {
            if (!string.IsNullOrEmpty(wsName))
            {
                return true;
            }

            Log.Error("NetworkComponent WebSocket 连接名 wsName 无效。");
            return false;
        }

        /// <summary>
        /// 创建并建立 WebSocket 长连接
        /// 底层交由 NetworkManager 管理
        /// </summary>
        /// <param name="wsName">WebSocket 唯一名称（用于区分不同连接）</param>
        /// <param name="url">WebSocket 服务器地址</param>
        /// <returns>WebSocket 实例</returns>
        public WebSocket CreateWebSocketConnection(string wsName, string url)
        {
            if (!EnsureSocketName(wsName))
            {
                return null;
            }
            if (string.IsNullOrEmpty(url))
            {
                Log.Error("NetworkComponent.CreateWebSocketConnection url 无效。");
                return null;
            }
            return m_Network.CreateWebSocketConnection(wsName, url);
        }

        /// <summary>
        /// 关闭指定的 WebSocket 连接
        /// </summary>
        /// <param name="wsName">WebSocket 唯一名称</param>
        /// <param name="code">关闭状态码（默认 1000 正常关闭）</param>
        /// <param name="message">关闭附带消息</param>
        public void CloseWebSocketConnection(string wsName, UInt16 code = default(UInt16), string message = null)
        {
            if (!EnsureSocketName(wsName))
            {
                return;
            }
            m_Network.CloseWebSocketConnection(wsName, code, message);
        }

        /// <summary>
        /// 通过 WebSocket 发送文本消息
        /// </summary>
        /// <param name="wsName">WebSocket 唯一名称</param>
        /// <param name="message">文本内容</param>
        public void SendWebSocketMessage(string wsName, string message)
        {
            if (!EnsureSocketName(wsName))
            {
                return;
            }
            m_Network.SendWebSocketMessage(wsName, message);
        }

        /// <summary>
        /// 通过 WebSocket 发送二进制数据
        /// </summary>
        /// <param name="wsName">WebSocket 唯一名称</param>
        /// <param name="datas">二进制字节数组</param>
        public void SendWebSocketBinary(string wsName, byte[] datas)
        {
            if (!EnsureSocketName(wsName))
            {
                return;
            }
            m_Network.SendWebSocketBinary(wsName, datas);
        }

        /// <summary>
        /// 获取已创建的 WebSocket 实例
        /// </summary>
        /// <param name="wsName">WebSocket 唯一名称</param>
        /// <returns>WebSocket 实例，不存在则返回 null</returns>
        public WebSocket GetWebSocket(string wsName)
        {
            if (!EnsureSocketName(wsName))
            {
                return null;
            }
            return m_Network.GetWebSocket(wsName);
        }

#endif

        #endregion
    }
}
