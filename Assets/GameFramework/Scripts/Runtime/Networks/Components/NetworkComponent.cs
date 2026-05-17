/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  NetworkComponent.cs
 * author:    云毅
 * created:   2026
 * descrip:   全局网络管理组件，提供网络检测、URL编码、Web请求下载
 ***************************************************************/

using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Honor.Runtime
{
    /// <summary>
    /// 网络管理组件
    /// 提供网络状态检测、URL 编码、Web 请求下载等基础网络功能
    /// 禁止挂载多个，单例模式组件
    /// </summary>
    [DisallowMultipleComponent]
    public sealed partial class NetworkComponent : GameComponent
    {
        //=========================================================================
        #region 生命周期
        //=========================================================================

        /// <summary>
        /// 组件初始化
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            // 初始化底层网络管理器
            m_NetworkManager = new NetworkManager(m_ConnectTimeout, m_RequestTimeout);
            if (m_NetworkManager == null)
            {
                Log.Fatal("NetworkManager 无效。");
                return;
            }
        }

        /// <summary>
        /// 启动逻辑
        /// </summary>
        private void Start()
        {

        }

        #endregion

        //=========================================================================
        #region 公共方法
        //=========================================================================

        /// <summary>
        /// 检查网络是否可用（连接/数据状态）
        /// </summary>
        /// <returns>true 网络正常，false 不可用</returns>
        public bool CheckNetworkActive()
        {
            return m_NetworkManager.CheckNetworkActive();
        }

        /// <summary>
        /// UTF-8 格式 URL 编码
        /// </summary>
        /// <param name="str">需要编码的原始字符串</param>
        /// <returns>URL 编码后的结果</returns>
        public string UrlEncode(string str)
        {
            StringBuilder sb = new StringBuilder();
            byte[] byStr = System.Text.Encoding.UTF8.GetBytes(str);
            for (int i = 0; i < byStr.Length; i++)
            {
                sb.Append(@"%" + Convert.ToString(byStr[i], 16));
            }
            return sb.ToString();
        }

        /// <summary>
        /// 从 URL 地址下载文本内容（异步）
        /// 使用 UnityWebRequest 加载文本数据
        /// </summary>
        /// <param name="url">下载地址</param>
        /// <param name="finishCallback">下载完成回调（返回文本）</param>
        /// <param name="timeout">超时时间，-1 使用默认值</param>
        public void GetTextFromUrl(string url, Action<string> finishCallback = null, int timeout = -1)
        {
            // 局部协程：下载文本并回调
            IEnumerator DownloadFromUrl(string url, Action<string> finishCallback, int timeout)
            {
                UnityWebRequest uwr = UnityWebRequest.Get(url);
                
                // 设置超时
                if (timeout >= 0)
                {
                    uwr.timeout = timeout;
                }

                // 发送请求并等待完成
                yield return uwr.SendWebRequest();
                while (!uwr.isDone)
                {
                    yield return null;
                }

                // 网络错误判断
                if (uwr.result == UnityWebRequest.Result.ConnectionError || uwr.result == UnityWebRequest.Result.ProtocolError)
                {
                    Log.Warning("[Hotfix] 下载文件 {0} 时出错！", url);
                    finishCallback?.Invoke(null);
                }
                else
                {
                    // 成功返回文本
                    finishCallback?.Invoke(uwr.downloadHandler.text);
                }

                // 释放 WebRequest 资源
                uwr.Dispose();
            }

            StartCoroutine(DownloadFromUrl(url, finishCallback, timeout));
        }

        #endregion
    }
}