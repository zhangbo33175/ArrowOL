/***************************************************************
 * (c) copyright 2021 - 2025, Honor.Game
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  NetworkManager.cs
 * author:    taoye
 * created:   2022/10/12
 * descrip:   Network管理器
 ***************************************************************/
#if BEST_HTTP_ENABLE
using BestHTTP.WebSocket;
#endif
using System.Collections.Generic;
using UnityEngine;

namespace Honor.Runtime
{
    public sealed partial class NetworkManager
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="connectTimeout">链接超时</param>
        /// <param name="requestTimeout">请求超时</param>
        public NetworkManager(float connectTimeout, float requestTimeout)
        {
            m_LuaComponent = GameComponentsGroup.GetComponent<LuaComponent>();
            if (m_LuaComponent == null)
            {
                Log.Fatal("Lua Component 无效。");
                return;
            }

            m_ConnectTimeout = connectTimeout;
            m_RequestTimeout = requestTimeout;
#if BEST_HTTP_ENABLE
            m_WebSockets = new Dictionary<string, WebSocket>();
#endif
        }

        /// <summary>
        /// 检查网络激活的状态
        /// </summary>
        /// <returns></returns>
        public bool CheckNetworkActive()
        {
            // 没有网络
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                return false;
            }
            // 2345G网络
            else if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
            {
                return true;
            }
            // wifi网络
            else if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
            {
                return true;
            }
            return false;
        }
    }

}


