/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  NetworkComponent.Http.cs
 * author:    云毅
 * created:   2026
 * descrip:   网络组件 - HTTP 请求接口层（基于 BestHTTP）
 ***************************************************************/

#if BEST_HTTP_ENABLE
using BestHTTP;
#endif

namespace Honor.Runtime
{
    /// <summary>
    /// 网络组件（HTTP 请求接口层）
    /// 提供对外 HTTP 调用入口，基于 BestHTTP 插件实现
    /// 组件层只做参数校验 + 转发，真正逻辑在 NetworkManager
    /// </summary>
    public sealed partial class NetworkComponent : GameComponent
    {
        //=========================================================================
        #region HTTP 请求接口（BestHTTP）
        //=========================================================================

#if BEST_HTTP_ENABLE

        /// <summary>
        /// GET 请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="finishedCallback">请求完成回调</param>
        /// <param name="keepAlive">是否长连接</param>
        /// <param name="requestTimeout">请求超时</param>
        /// <param name="connectTimeout">连接超时</param>
        /// <param name="headerInfos">请求头（JSON 字符串）</param>
        public void HttpRequestOnGet(
            string url,
            OnRequestFinishedDelegate finishedCallback = null,
            bool keepAlive = true,
            float requestTimeout = -1f,
            float connectTimeout = -1f,
            string headerInfos = null)
        {
            if (string.IsNullOrEmpty(url))
            {
                Log.Error("NetworkComponent.HttpRequestOnGet url 无效。");
                return;
            }

            m_NetworkManager.HttpRequestOnGet(
                url,
                finishedCallback,
                keepAlive,
                requestTimeout,
                connectTimeout,
                headerInfos);
        }

        /// <summary>
        /// POST 请求（字符串数据，如 JSON）
        /// </summary>
        public void HttpRequestOnPost(
            string url,
            string contentString,
            OnRequestFinishedDelegate finishedCallback = null,
            bool keepAlive = true,
            float requestTimeout = -1f,
            float connectTimeout = -1f,
            string headerInfos = null)
        {
            if (string.IsNullOrEmpty(url))
            {
                Log.Error("NetworkComponent.HttpRequestOnPostWithData url 无效。");
                return;
            }
            if (string.IsNullOrEmpty(contentString))
            {
                Log.Error("NetworkComponent.HttpRequestOnPostWithData jsonString 无效。");
                return;
            }

            m_NetworkManager.HttpRequestOnPost(
                url,
                contentString,
                finishedCallback,
                keepAlive,
                requestTimeout,
                connectTimeout,
                headerInfos);
        }

        /// <summary>
        /// POST 请求（原始字节流）
        /// 用于二进制数据、加密数据上传
        /// </summary>
        public void HttpRequestOnPostWithRawData(
            string url,
            byte[] contentBytes,
            OnRequestFinishedDelegate finishedCallback = null,
            bool keepAlive = true,
            float requestTimeout = -1f,
            float connectTimeout = -1f,
            string headerInfos = null)
        {
            if (string.IsNullOrEmpty(url))
            {
                Log.Error("NetworkComponent.HttpRequestOnPostWithRawData url 无效。");
                return;
            }
            if (contentBytes == null)
            {
                Log.Error("NetworkComponent.HttpRequestOnPostWithRawData contentBytes 无效。");
                return;
            }

            m_NetworkManager.HttpRequestOnPostWithRawData(
                url,
                contentBytes,
                finishedCallback,
                keepAlive,
                requestTimeout,
                connectTimeout,
                headerInfos);
        }

        /// <summary>
        /// POST 请求（上传文件 + 表单数据）
        /// 用于图片、日志、存档上传
        /// </summary>
        public void HttpRequestOnPostWithFile(
            string url,
            string customJsonData,
            byte[] fileBytes,
            string fileName,
            OnRequestFinishedDelegate finishedCallback = null,
            bool keepAlive = true,
            float requestTimeout = -1f,
            float connectTimeout = -1f,
            string headerInfos = null)
        {
            if (string.IsNullOrEmpty(url))
            {
                Log.Error("NetworkComponent.HttpRequestOnPostWithFile url 无效。");
                return;
            }
            if (fileBytes == null)
            {
                Log.Error("NetworkComponent.HttpRequestOnPostWithFile fileBytes 无效。");
                return;
            }
            if (string.IsNullOrEmpty(fileName))
            {
                Log.Error("NetworkComponent.HttpRequestOnPostWithFile fileName 无效。");
                return;
            }

            m_NetworkManager.HttpRequestOnPostWithFile(
                url,
                customJsonData,
                fileBytes,
                fileName,
                finishedCallback,
                keepAlive,
                requestTimeout,
                connectTimeout,
                headerInfos);
        }

#endif

        #endregion
    }
}