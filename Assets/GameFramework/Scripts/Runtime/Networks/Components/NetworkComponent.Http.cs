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
        #region 参数校验辅助
        //=========================================================================

#if BEST_HTTP_ENABLE
        /// <summary>
        /// 校验请求地址是否非空
        /// </summary>
        /// <param name="url">待校验地址</param>
        /// <returns>地址有效返回 true，否则记录错误并返回 false</returns>
        private bool EnsureRequestUrl(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                return true;
            }

            Log.Error("NetworkComponent HTTP 请求地址 url 无效。");
            return false;
        }

        /// <summary>
        /// 校验引用型参数是否非空
        /// </summary>
        /// <param name="arg">待校验参数</param>
        /// <param name="argName">参数名（用于错误日志）</param>
        /// <returns>非空返回 true，否则记录错误并返回 false</returns>
        private bool EnsureArgNotNull(object arg, string argName)
        {
            if (arg != null)
            {
                return true;
            }

            Log.Error($"NetworkComponent HTTP 请求参数 {argName} 无效。");
            return false;
        }
#endif

        #endregion

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
            if (!EnsureRequestUrl(url))
            {
                return;
            }

            m_Network.HttpRequestOnGet(
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
            if (!EnsureRequestUrl(url))
            {
                return;
            }
            if (string.IsNullOrEmpty(contentString))
            {
                Log.Error("NetworkComponent HTTP 请求内容 contentString 无效。");
                return;
            }

            m_Network.HttpRequestOnPost(
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
            if (!EnsureRequestUrl(url))
            {
                return;
            }
            if (!EnsureArgNotNull(contentBytes, "contentBytes"))
            {
                return;
            }

            m_Network.HttpRequestOnPostWithRawData(
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
            if (!EnsureRequestUrl(url))
            {
                return;
            }
            if (!EnsureArgNotNull(fileBytes, "fileBytes"))
            {
                return;
            }
            if (string.IsNullOrEmpty(fileName))
            {
                Log.Error("NetworkComponent HTTP 上传文件名 fileName 无效。");
                return;
            }

            m_Network.HttpRequestOnPostWithFile(
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
