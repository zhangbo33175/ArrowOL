/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  NetworkManager.Http.cs
 * author:    云毅
 * created:   2026
 * descrip:   网络底层管理器 - HTTP 请求实现（基于 BestHTTP）
 ***************************************************************/

#if BEST_HTTP_ENABLE
using BestHTTP;
using LitJson;
using Newtonsoft.Json.Linq;
#endif
using System;

namespace Honor.Runtime
{
    /// <summary>
    /// 网络底层管理器 - HTTP 请求模块
    /// </summary>
    public sealed partial class NetworkManager
    {
        //=========================================================================
#if BEST_HTTP_ENABLE
        //=========================================================================
        #region HTTP 请求实现（BestHTTP）
        //=========================================================================

        /// <summary>
        /// GET 方式 HTTP 请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="finishedCallback">请求完成回调</param>
        /// <param name="keepAlive">是否长连接</param>
        /// <param name="requestTimeout">请求超时</param>
        /// <param name="connectTimeout">连接超时</param>
        /// <param name="headerInfos">请求头 Json 字符串</param>
        public void HttpRequestOnGet(string url, OnRequestFinishedDelegate finishedCallback, bool keepAlive, float requestTimeout, float connectTimeout, string headerInfos)
        {
            Log.Info("请求网络 HttpRequestOnGet url: " + url);
            HTTPRequest request = new HTTPRequest(new Uri(url), HTTPMethods.Get, WrapResponseCallback(url, "HttpRequestOnGet", finishedCallback));

            ApplyCommonSettings(request, keepAlive, requestTimeout, connectTimeout);
            AttachCustomHeaders(request, headerInfos);

            request.Send();
        }

        /// <summary>
        /// POST 方式 HTTP 请求（字符串内容）
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="contentString">请求体字符串</param>
        /// <param name="finishedCallback">完成回调</param>
        /// <param name="keepAlive">是否长连接</param>
        /// <param name="requestTimeout">请求超时</param>
        /// <param name="connectTimeout">连接超时</param>
        /// <param name="headerInfos">请求头 Json</param>
        public void HttpRequestOnPost(string url, string contentString, OnRequestFinishedDelegate finishedCallback, bool keepAlive, float requestTimeout, float connectTimeout, string headerInfos)
        {
            Log.Info("请求网络 HttpRequestOnPost url: " + url + " contentString: " + contentString);
            HTTPRequest request = new HTTPRequest(new Uri(url), HTTPMethods.Post, WrapResponseCallback(url, "HttpRequestOnPost", finishedCallback));

            request.RawData = Converter.GetBytesByString(contentString);
            ApplyCommonSettings(request, keepAlive, requestTimeout, connectTimeout);
            AttachCustomHeaders(request, headerInfos);

            request.Send();
        }

        /// <summary>
        /// POST 方式 HTTP 请求（原始字节流）
        /// 用于二进制/加密数据请求
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="contentBytes">字节数据</param>
        /// <param name="finishedCallback">回调</param>
        /// <param name="keepAlive">长连接</param>
        /// <param name="requestTimeout">超时</param>
        /// <param name="connectTimeout">连接超时</param>
        /// <param name="headerInfos">头信息</param>
        public void HttpRequestOnPostWithRawData(string url, byte[] contentBytes, OnRequestFinishedDelegate finishedCallback, bool keepAlive, float requestTimeout, float connectTimeout, string headerInfos)
        {
            Log.Info("请求网络 HttpRequestOnPostWithRawData url: " + url);
            HTTPRequest request = new HTTPRequest(new Uri(url), HTTPMethods.Post, WrapResponseCallback(url, "HttpRequestOnPostWithRawData", finishedCallback));

            request.RawData = contentBytes;
            ApplyCommonSettings(request, keepAlive, requestTimeout, connectTimeout);
            AttachCustomHeaders(request, headerInfos);

            request.Send();
        }

        /// <summary>
        /// POST 方式 HTTP 请求（上传文件 + 表单数据）
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="customJsonData">自定义表单 Json</param>
        /// <param name="fileBytes">文件字节</param>
        /// <param name="fileName">文件名</param>
        /// <param name="finishedCallback">完成回调</param>
        /// <param name="keepAlive">长连接</param>
        /// <param name="requestTimeout">超时</param>
        /// <param name="connectTimeout">连接超时</param>
        /// <param name="headerInfos">头信息</param>
        public void HttpRequestOnPostWithFile(string url, string customJsonData, byte[] fileBytes, string fileName, OnRequestFinishedDelegate finishedCallback = null, bool keepAlive = true, float requestTimeout = -1f, float connectTimeout = -1f, string headerInfos = null)
        {
            HTTPRequest request = new HTTPRequest(new Uri(url), HTTPMethods.Post, (req, resp) =>
            {
                HandlePostFileRequestFinished(req, resp, finishedCallback);
            });

            // 构建表单：添加自定义字段 + 文件数据
            request.SetForm(BuildPostForm(customJsonData, fileBytes, fileName));

            // 超时设置
            request.IsKeepAlive = keepAlive;
            request.Timeout = TimeSpan.FromSeconds(requestTimeout == -1f ? 30f : requestTimeout);
            request.ConnectTimeout = TimeSpan.FromSeconds(connectTimeout == -1f ? 10f : connectTimeout);

            // 默认开启 gzip 压缩 + 自定义请求头
            ApplyRequestHeaders(request, headerInfos);

            // 发送
            request.Send();
        }

        #endregion

        //=========================================================================
        #region HTTP 内部辅助
        //=========================================================================

        /// <summary>
        /// 配置保活与超时（-1 表示使用管理器默认值）
        /// </summary>
        private void ApplyCommonSettings(HTTPRequest request, bool keepAlive, float requestTimeout, float connectTimeout)
        {
            request.IsKeepAlive = keepAlive;
            request.Timeout = TimeSpan.FromSeconds(requestTimeout == -1f ? m_ReqTimeoutSec : requestTimeout);
            request.ConnectTimeout = TimeSpan.FromSeconds(connectTimeout == -1f ? m_ConnTimeoutSec : connectTimeout);
        }

        /// <summary>
        /// 解析自定义请求头 Json 并逐条附加到请求
        /// </summary>
        private void AttachCustomHeaders(HTTPRequest request, string headerInfos)
        {
            if (headerInfos == null)
            {
                return;
            }

            JObject headerJson = JObject.Parse(headerInfos);
            foreach (var pair in headerJson)
            {
                request.AddHeader(pair.Key, pair.Value.ToString());
            }
        }

        /// <summary>
        /// 统一包装响应回调：先按状态打印日志，再转发业务回调
        /// </summary>
        private OnRequestFinishedDelegate WrapResponseCallback(string url, string tag, OnRequestFinishedDelegate finishedCallback)
        {
            return (req, resp) =>
            {
                LogResponseState(req);

                if (finishedCallback != null)
                {
                    Log.Info("请求网络 返回 " + tag + " url: " + url);
                    finishedCallback(req, resp);
                }
            };
        }

        /// <summary>
        /// 按请求状态打印告警日志
        /// </summary>
        private static void LogResponseState(HTTPRequest req)
        {
            switch (req.State)
            {
                case HTTPRequestStates.Finished:
                    break;
                case HTTPRequestStates.Error:
                    Log.Warning(AorTxt.Format("Request Finished with Error! {0}", (req.Exception != null ? (req.Exception.Message + "\n" + req.Exception.StackTrace) : "No Exception")));
                    break;
                case HTTPRequestStates.Aborted:
                    Log.Warning("Request Aborted!");
                    break;
                case HTTPRequestStates.ConnectionTimedOut:
                    Log.Warning("Connection Timed Out!");
                    break;
                case HTTPRequestStates.TimedOut:
                    Log.Warning("Processing the request Timed Out!");
                    break;
            }
        }

        /// <summary>
        /// 处理文件上传请求的完成回调：按请求状态打印日志并转发业务回调
        /// </summary>
        /// <param name="req">HTTP请求对象</param>
        /// <param name="resp">HTTP响应对象</param>
        /// <param name="finishedCallback">业务完成回调</param>
        private void HandlePostFileRequestFinished(HTTPRequest req, HTTPResponse resp, OnRequestFinishedDelegate finishedCallback)
        {
            LogResponseState(req);

            if (finishedCallback != null)
            {
                finishedCallback(req, resp);
            }
        }

        /// <summary>
        /// 构建multipart表单：解析自定义Json字段并附加文件二进制数据
        /// </summary>
        /// <param name="customJsonData">自定义表单 Json</param>
        /// <param name="fileBytes">文件字节</param>
        /// <param name="fileName">文件名</param>
        /// <returns>构建好的表单对象</returns>
        private HTTPMultiPartForm BuildPostForm(string customJsonData, byte[] fileBytes, string fileName)
        {
            // 构建表单：添加自定义字段 + 文件数据
            HTTPMultiPartForm form = new HTTPMultiPartForm();
            if (!string.IsNullOrEmpty(customJsonData))
            {
                JObject jObject = JObject.Parse(customJsonData);
                if (jObject != null)
                {
                    foreach (var pair in jObject)
                    {
                        form.AddField(pair.Key, pair.Value.ToString());
                    }
                }
            }
            form.AddBinaryData("file", fileBytes, fileName, "multipart/form-data");
            return form;
        }

        /// <summary>
        /// 应用默认gzip头并解析自定义请求头
        /// </summary>
        /// <param name="request">HTTP请求对象</param>
        /// <param name="headerInfos">自定义请求头Json字符串</param>
        private void ApplyRequestHeaders(HTTPRequest request, string headerInfos)
        {
            // 默认开启 gzip 压缩
            request.AddHeader("Accept-Encoding", "gzip");

            // 自定义请求头
            AttachCustomHeaders(request, headerInfos);
        }

        #endregion
#endif

    }
}
