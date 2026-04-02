#if BEST_HTTP_ENABLE
using BestHTTP;
#endif

namespace Honor.Runtime
{
    public sealed partial class NetworkComponent : GameComponent
    {
#if BEST_HTTP_ENABLE
        /// <summary>
        /// GET方式HTTP请求
        /// </summary>
        /// <param name="url">url链接</param>
        /// <param name="finishedCallback">HTTP结果回调</param>
        /// <param name="keepAlive">是否保活（频繁交互时保活可以降低开销）</param>
        /// <param name="requestTimeout">请求超时时间</param>
        /// <param name="connectTimeout">链接超时时间</param>
        /// <param name="headerInfos">传入的协议头内容（json键值对格式）</param>
        public void HttpRequestOnGet(string url, OnRequestFinishedDelegate finishedCallback = null, bool keepAlive = true, float requestTimeout = -1f, float connectTimeout = -1f, string headerInfos = null)
        {
            if (string.IsNullOrEmpty(url))
            {
                Log.Error("NetworkComponent.HttpRequestOnGet url 无效。");
                return;
            }
            m_NetworkManager.HttpRequestOnGet(url, finishedCallback, keepAlive, requestTimeout, connectTimeout, headerInfos);
        }

        /// <summary>
        /// POST方式HTTP请求
        /// </summary>
        /// <param name="url">url链接</param>
        /// <param name="contentString">字符串</param>
        /// <param name="finishedCallback">HTTP结果回调</param>
        /// <param name="keepAlive">是否保活（频繁交互时保活可以降低开销）</param>
        /// <param name="requestTimeout">请求超时时间</param>
        /// <param name="connectTimeout">链接超时时间</param>
        /// <param name="headerInfos">传入的协议头内容（json键值对格式）</param>
        public void HttpRequestOnPost(string url, string contentString, OnRequestFinishedDelegate finishedCallback = null, bool keepAlive = true, float requestTimeout = -1f, float connectTimeout = -1f, string headerInfos = null)
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
            m_NetworkManager.HttpRequestOnPost(url, contentString, finishedCallback, keepAlive, requestTimeout, connectTimeout, headerInfos);
        }

        /// <summary>
        /// POST方式HTTP请求（字节流）
        /// 明文请求
        /// </summary>
        /// <param name="url">url链接</param>
        /// <param name="contentBytes">字节流</param>
        /// <param name="finishedCallback">HTTP结果回调</param>
        /// <param name="keepAlive">是否保活（频繁交互时保活可以降低开销）</param>
        /// <param name="requestTimeout">请求超时时间</param>
        /// <param name="connectTimeout">链接超时时间</param>
        /// <param name="headerInfos">传入的协议头内容（json键值对格式）</param>
        public void HttpRequestOnPostWithRawData(string url, byte[] contentBytes, OnRequestFinishedDelegate finishedCallback = null, bool keepAlive = true, float requestTimeout = -1f, float connectTimeout = -1f, string headerInfos = null)
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
            m_NetworkManager.HttpRequestOnPostWithRawData(url, contentBytes, finishedCallback, keepAlive, requestTimeout, connectTimeout, headerInfos);
        }
        
        /// <summary>
        /// POST方式HTTP请求（文件）
        /// 明文请求
        /// </summary>
        /// <param name="url">url链接</param>
        /// <param name="appID">应用ID</param>
        /// <param name="userID">用户ID</param>
        /// <param name="fileBytes">文件字节流</param>
        /// <param name="fileName">文件名称</param>
        /// <param name="finishedCallback">HTTP结果回调</param>
        /// <param name="keepAlive">是否保活（频繁交互时保活可以降低开销）</param>
        /// <param name="requestTimeout">请求超时时间</param>
        /// <param name="connectTimeout">链接超时时间</param>
        /// <param name="headerInfos">传入的协议头内容（json键值对格式）</param>
        public void HttpRequestOnPostWithFile(string url, string customJsonData, byte[] fileBytes, string fileName, OnRequestFinishedDelegate finishedCallback = null, bool keepAlive = true, float requestTimeout = -1f, float connectTimeout = -1f, string headerInfos = null)
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
            m_NetworkManager.HttpRequestOnPostWithFile(url, customJsonData, fileBytes, fileName, finishedCallback, keepAlive, requestTimeout, connectTimeout, headerInfos);
        }
        
#endif
    }

}


