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
        #region HTTP 请求实现（BestHTTP）
        //=========================================================================

#if BEST_HTTP_ENABLE

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
            HTTPRequest request = new HTTPRequest(new Uri(url), HTTPMethods.Get, (HTTPRequest req, HTTPResponse resp) =>
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

                // 执行上层回调
                if (finishedCallback != null)
                {
                    Log.Info("请求网络 返回 HttpRequestOnGet url: " + url);
                    finishedCallback(req, resp);
                }
            });

            // 基础配置
            request.IsKeepAlive = keepAlive;
            request.Timeout = TimeSpan.FromSeconds(requestTimeout == -1f ? m_RequestTimeout : requestTimeout);
            request.ConnectTimeout = TimeSpan.FromSeconds(connectTimeout == -1f ? m_ConnectTimeout : connectTimeout);

            // 解析并添加请求头
            if (headerInfos != null)
            {
                JObject headerJson = JObject.Parse(headerInfos);
                foreach (var itr in headerJson)
                {
                    request.AddHeader(itr.Key, itr.Value.ToString());
                }
            }

            // 发送请求
            if (request != null)
            {
                request.Send();
            }
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
            HTTPRequest request = new HTTPRequest(new Uri(url), HTTPMethods.Post, (HTTPRequest req, HTTPResponse resp) =>
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

                if (finishedCallback != null)
                {
                    Log.Info("请求网络 返回 HttpRequestOnPost url: " + url);
                    finishedCallback(req, resp);
                }
            });

            // 设置原始数据
            request.RawData = Converter.GetBytesByString(contentString);
            request.IsKeepAlive = keepAlive;
            request.Timeout = TimeSpan.FromSeconds(requestTimeout == -1f ? m_RequestTimeout : requestTimeout);
            request.ConnectTimeout = TimeSpan.FromSeconds(connectTimeout == -1f ? m_ConnectTimeout : connectTimeout);

            // 添加请求头
            if (headerInfos != null)
            {
                JObject headerJson = JObject.Parse(headerInfos);
                foreach (var itr in headerJson)
                {
                    request.AddHeader(itr.Key, itr.Value.ToString());
                }
            }

            if (request != null)
            {
                request.Send();
            }
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
            HTTPRequest request = new HTTPRequest(new Uri(url), HTTPMethods.Post, (HTTPRequest req, HTTPResponse resp) =>
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

                if (finishedCallback != null)
                {
                    Log.Info("请求网络 返回 HttpRequestOnPostWithRawData url: " + url);
                    finishedCallback(req, resp);
                }
            });

            request.RawData = contentBytes;
            request.IsKeepAlive = keepAlive;
            request.Timeout = TimeSpan.FromSeconds(requestTimeout == -1f ? m_RequestTimeout : requestTimeout);
            request.ConnectTimeout = TimeSpan.FromSeconds(connectTimeout == -1f ? m_ConnectTimeout : connectTimeout);

            if (headerInfos != null)
            {
                JObject headerJson = JObject.Parse(headerInfos);
                foreach (var itr in headerJson)
                {
                    request.AddHeader(itr.Key, itr.Value.ToString());
                }
            }

            if (request != null)
            {
                request.Send();
            }
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
            HTTPRequest request = new HTTPRequest(new Uri(url), HTTPMethods.Post, (HTTPRequest req, HTTPResponse resp) =>
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

                if (finishedCallback != null)
                {
                    finishedCallback(req, resp);
                }
            });

            // 构建表单：添加自定义字段 + 文件数据
            HTTPMultiPartForm form = new HTTPMultiPartForm();
            if (!string.IsNullOrEmpty(customJsonData))
            {
                JObject jObject = JObject.Parse(customJsonData);
                if (jObject != null)
                {
                    foreach (var itr in jObject)
                    {
                        form.AddField(itr.Key, itr.Value.ToString());
                    }
                }
            }
            form.AddBinaryData("file", fileBytes, fileName, "multipart/form-data");
            request.SetForm(form);

            // 超时设置
            request.IsKeepAlive = keepAlive;
            request.Timeout = TimeSpan.FromSeconds(requestTimeout == -1f ? 30f : requestTimeout);
            request.ConnectTimeout = TimeSpan.FromSeconds(connectTimeout == -1f ? 10f : connectTimeout);

            // 默认开启 gzip 压缩
            request.AddHeader("Accept-Encoding", "gzip");

            // 自定义请求头
            if (!string.IsNullOrEmpty(headerInfos))
            {
                JObject headerJson = JObject.Parse(headerInfos);
                foreach (var itr in headerJson)
                {
                    request.AddHeader(itr.Key, itr.Value.ToString());
                }
            }

            // 发送
            request.Send();
        }

#endif

        #endregion
    }
}