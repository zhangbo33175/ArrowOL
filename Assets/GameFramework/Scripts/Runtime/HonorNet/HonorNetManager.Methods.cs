/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  HonorNetManager.Methods.cs
 * author:    云毅
 * created:   2026
 * descrip:   网络模块门面管理器 - 业务便捷方法转发（partial）
 *            收敛网络基础能力对外方法，外部网络调用统一走本管理器
 ***************************************************************/

using System;

namespace Honor.Runtime
{
    public sealed partial class HonorNetManager
    {
        //=========================================================================

        #region 网络基础能力

        //=========================================================================

        /// <summary>
        /// 检查当前网络是否可用
        /// </summary>
        /// <returns>可用返回true</returns>
        public bool CheckNetworkActive()
        {
            return Network.CheckNetworkActive();
        }

        /// <summary>
        /// URL 编码
        /// </summary>
        /// <param name="str">待编码字符串</param>
        /// <returns>编码后字符串</returns>
        public string UrlEncode(string str)
        {
            return Network.UrlEncode(str);
        }

        /// <summary>
        /// 从 URL 获取文本内容
        /// </summary>
        /// <param name="url">目标URL</param>
        /// <param name="finishCallback">完成回调，携带文本内容</param>
        /// <param name="timeout">超时时间（秒），默认-1不限制</param>
        public void GetTextFromUrl(string url, Action<string> finishCallback = null, int timeout = -1)
        {
            Network.GetTextFromUrl(url, finishCallback, timeout);
        }

        // 说明：HTTP 请求（Get/Post/上传文件）与 WebSocket 长连接已收敛为本门面方法，
        // 直接通过本管理器调用即可（下方方法在 BEST_HTTP_ENABLE 宏下生效）：
        //   HonorNetManager.Instance.HttpRequestOnGet(...) / CreateWebSocketConnection(...)

#if BEST_HTTP_ENABLE

        /// <summary>
        /// GET 请求（转发自 NetworkComponent）
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="finishedCallback">请求完成回调，可选</param>
        /// <param name="keepAlive">是否长连接，默认true</param>
        /// <param name="requestTimeout">请求超时（秒），默认-1不限制</param>
        /// <param name="connectTimeout">连接超时（秒），默认-1不限制</param>
        /// <param name="headerInfos">请求头（JSON 字符串），可选</param>
        public void HttpRequestOnGet(
            string url,
            BestHTTP.OnRequestFinishedDelegate finishedCallback = null,
            bool keepAlive = true,
            float requestTimeout = -1f,
            float connectTimeout = -1f,
            string headerInfos = null)
        {
            Network.HttpRequestOnGet(url, finishedCallback, keepAlive, requestTimeout, connectTimeout, headerInfos);
        }

        /// <summary>
        /// POST 请求（字符串数据，如 JSON，转发自 NetworkComponent）
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="contentString">请求内容字符串</param>
        /// <param name="finishedCallback">请求完成回调，可选</param>
        /// <param name="keepAlive">是否长连接，默认true</param>
        /// <param name="requestTimeout">请求超时（秒），默认-1不限制</param>
        /// <param name="connectTimeout">连接超时（秒），默认-1不限制</param>
        /// <param name="headerInfos">请求头（JSON 字符串），可选</param>
        public void HttpRequestOnPost(
            string url,
            string contentString,
            BestHTTP.OnRequestFinishedDelegate finishedCallback = null,
            bool keepAlive = true,
            float requestTimeout = -1f,
            float connectTimeout = -1f,
            string headerInfos = null)
        {
            Network.HttpRequestOnPost(url, contentString, finishedCallback, keepAlive, requestTimeout, connectTimeout, headerInfos);
        }

        /// <summary>
        /// POST 请求（上传文件 + 表单数据，转发自 NetworkComponent）
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="customJsonData">自定义 JSON 数据</param>
        /// <param name="fileBytes">文件字节流</param>
        /// <param name="fileName">文件名</param>
        /// <param name="finishedCallback">请求完成回调，可选</param>
        /// <param name="keepAlive">是否长连接，默认true</param>
        /// <param name="requestTimeout">请求超时（秒），默认-1不限制</param>
        /// <param name="connectTimeout">连接超时（秒），默认-1不限制</param>
        /// <param name="headerInfos">请求头（JSON 字符串），可选</param>
        public void HttpRequestOnPostWithFile(
            string url,
            string customJsonData,
            byte[] fileBytes,
            string fileName,
            BestHTTP.OnRequestFinishedDelegate finishedCallback = null,
            bool keepAlive = true,
            float requestTimeout = -1f,
            float connectTimeout = -1f,
            string headerInfos = null)
        {
            Network.HttpRequestOnPostWithFile(url, customJsonData, fileBytes, fileName, finishedCallback,
                keepAlive, requestTimeout, connectTimeout, headerInfos);
        }

        /// <summary>
        /// 创建并建立 WebSocket 长连接（转发自 NetworkComponent）
        /// </summary>
        /// <param name="wsName">WebSocket 唯一名称</param>
        /// <param name="url">WebSocket 服务器地址</param>
        /// <returns>WebSocket 实例</returns>
        public BestHTTP.WebSocket.WebSocket CreateWebSocketConnection(string wsName, string url)
        {
            return Network.CreateWebSocketConnection(wsName, url);
        }

        /// <summary>
        /// 获取已创建的 WebSocket 实例（转发自 NetworkComponent）
        /// </summary>
        /// <param name="wsName">WebSocket 唯一名称</param>
        /// <returns>WebSocket 实例，不存在返回null</returns>
        public BestHTTP.WebSocket.WebSocket GetWebSocket(string wsName)
        {
            return Network.GetWebSocket(wsName);
        }

#endif

        #endregion
    }
}
