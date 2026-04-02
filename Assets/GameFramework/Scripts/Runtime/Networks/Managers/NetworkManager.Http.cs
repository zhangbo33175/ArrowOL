/***************************************************************
 * (c) copyright 2021 - 2025, Honor.Game
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  NetworkManager.Http.cs
 * author:    taoye
 * created:   2022/10/12
 * descrip:   Network管理器-Http链接接口（短连接）
 ***************************************************************/
#if BEST_HTTP_ENABLE
using BestHTTP;
using LitJson;
using Newtonsoft.Json.Linq;
#endif
using System;

namespace Honor.Runtime
{
    public sealed partial class NetworkManager
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
        public void HttpRequestOnGet(string url, OnRequestFinishedDelegate finishedCallback, bool keepAlive, float requestTimeout, float connectTimeout, string headerInfos)
        {
            Log.Info("请求网络 HttpRequestOnGet url: " + url);
            HTTPRequest request = new HTTPRequest(new Uri(url), HTTPMethods.Get, (HTTPRequest req, HTTPResponse resp) =>
            {
                switch (req.State)
                {
                    case HTTPRequestStates.Finished:
                        //Log.Info("Request Finished!");
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
                    Log.Info("请求网络 返回 HttpRequestOnGet url: " + url);
                    finishedCallback(req, resp);
                }
            });

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
        /// POST方式HTTP请求
        /// </summary>
        /// <param name="url">url链接</param>
        /// <param name="contentString">字符串</param>
        /// <param name="finishedCallback">HTTP结果回调</param>
        /// <param name="keepAlive">是否保活（频繁交互时保活可以降低开销）</param>
        /// <param name="requestTimeout">请求超时时间</param>
        /// <param name="connectTimeout">链接超时时间</param>
        /// <param name="headerInfos">传入的协议头内容（json键值对格式）</param>
        public void HttpRequestOnPost(string url, string contentString, OnRequestFinishedDelegate finishedCallback, bool keepAlive, float requestTimeout, float connectTimeout, string headerInfos)
        {
            Log.Info("请求网络 HttpRequestOnPost url: " + url + " contentString: " + contentString);
            HTTPRequest request = new HTTPRequest(new Uri(url), HTTPMethods.Post, (HTTPRequest req, HTTPResponse resp) =>
            {
                switch (req.State)
                {
                    case HTTPRequestStates.Finished:
                        //Log.Info("Request Finished!");
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

            request.RawData = Converter.GetBytesByString(contentString);
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
        public void HttpRequestOnPostWithRawData(string url, byte[] contentBytes, OnRequestFinishedDelegate finishedCallback, bool keepAlive, float requestTimeout, float connectTimeout, string headerInfos)
        {
            Log.Info("请求网络 HttpRequestOnPostWithRawData url: " + url);
            HTTPRequest request = new HTTPRequest(new Uri(url), HTTPMethods.Post, (HTTPRequest req, HTTPResponse resp) =>
            {
                switch (req.State)
                {
                    case HTTPRequestStates.Finished:
                        //Log.Info("Request Finished!");
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
        /// POST方式HTTP请求（文件）
        /// 明文请求
        /// </summary>
        /// <param name="url">url链接</param>
         /// <param name="fileBytes">文件字节流</param>
        /// <param name="fileName">文件名称</param>
        /// <param name="finishedCallback">HTTP结果回调</param>
        /// <param name="keepAlive">是否保活（频繁交互时保活可以降低开销）</param>
        /// <param name="requestTimeout">请求超时时间</param>
        /// <param name="connectTimeout">链接超时时间</param>
        /// <param name="headerInfos">传入的协议头内容（json键值对格式）</param>
        public void HttpRequestOnPostWithFile(string url, string customJsonData, byte[] fileBytes, string fileName, OnRequestFinishedDelegate finishedCallback = null, bool keepAlive = true, float requestTimeout = -1f, float connectTimeout = -1f, string headerInfos = null)
        {
            HTTPRequest request = new HTTPRequest(new Uri(url), HTTPMethods.Post, (HTTPRequest req, HTTPResponse resp) =>
            {
                switch (req.State)
                {
                    case HTTPRequestStates.Finished:
                        //Log.Info("Request Finished!");
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
            
            // 设置超时
            request.IsKeepAlive = keepAlive;
            request.Timeout = TimeSpan.FromSeconds(requestTimeout == -1f ? 30f : requestTimeout);
            request.ConnectTimeout = TimeSpan.FromSeconds(connectTimeout == -1f ? 10f : connectTimeout);

            // 设置请求头
            request.AddHeader("Accept-Encoding", "gzip");
            if (!string.IsNullOrEmpty(headerInfos))
            {
                JObject headerJson = JObject.Parse(headerInfos);
                foreach (var itr in headerJson)
                {
                    request.AddHeader(itr.Key, itr.Value.ToString());
                }
            }

            // 发送请求
            request.Send();
        }
#endif
    }
}
