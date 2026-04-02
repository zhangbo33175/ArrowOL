using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Honor.Runtime
{
    [DisallowMultipleComponent]
    public sealed partial class NetworkComponent : GameComponent
    {
        protected override void Awake()
        {
            base.Awake();

            m_NetworkManager = new NetworkManager(m_ConnectTimeout, m_RequestTimeout);
            if (m_NetworkManager == null)
            {
                Log.Fatal("NetworkManager 无效。");
                return;
            }

        }

        private void Start()
        {

        }

        /// <summary>
        /// 检查网络激活的状态
        /// </summary>
        /// <returns></returns>
        public bool CheckNetworkActive()
        {
            return m_NetworkManager.CheckNetworkActive();
        }

        /// <summary>
        /// Url编码
        /// </summary>
        /// <param name="str">待转换的字符串</param>
        /// <returns></returns>
        public string UrlEncode(string str)
        {
            StringBuilder sb = new StringBuilder();
            byte[] byStr = System.Text.Encoding.UTF8.GetBytes(str);
            for (int i = 0; i < byStr.Length; i++)
            {
                sb.Append(@"%" + Convert.ToString(byStr[i], 16));
            }
            return (sb.ToString());
        }

        /// <summary>
        /// 从url中获取文本内容
        /// </summary>
        /// <param name="url"></param>
        /// <param name="finishCallback"></param>
        /// <param name="timeout"></param>
        public void GetTextFromUrl(string url, Action<string> finishCallback = null, int timeout = -1)
        {
            IEnumerator DownloadFromUrl(string url, Action<string> finishCallback, int timeout)
            {
                var uwr = UnityWebRequest.Get(url);
                if (timeout >= 0)
                {
                    uwr.timeout = timeout;
                }
                uwr.SendWebRequest();
                while (!uwr.isDone)
                {
                    yield return null;
                }
                if (uwr.result == UnityWebRequest.Result.ConnectionError || uwr.result == UnityWebRequest.Result.ProtocolError)
                {
                    Log.Warning("[Hotfix] 下载文件 {0} 时出错！", url);
                    if (finishCallback != null)
                    {
                        finishCallback(null);
                    }
                }
                else
                {
                    if (finishCallback != null)
                    {
                        finishCallback(uwr.downloadHandler.text);
                    }
                }
                uwr.Dispose();
            }
            StartCoroutine(DownloadFromUrl(url, finishCallback, timeout));
        }
        
    }

}


