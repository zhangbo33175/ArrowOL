/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  NetworkManager.cs
 * author:    云毅
 * created:   2026
 * descrip:   网络底层核心管理器，负责网络状态、连接、WebSocket 管理
 ***************************************************************/

#if BEST_HTTP_ENABLE
using BestHTTP.WebSocket;
#endif
using System.Collections.Generic;
using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// 网络底层管理器
    /// 负责网络状态检测、WebSocket管理、请求/连接超时配置
    /// </summary>
    public sealed partial class NetworkManager
    {
        //=========================================================================
        #region 构造函数
        //=========================================================================

        /// <summary>
        /// 构造函数
        /// 初始化Lua组件、超时时间、WebSocket容器
        /// </summary>
        /// <param name="connectTimeout">连接超时（秒）</param>
        /// <param name="requestTimeout">请求超时（秒）</param>
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

        #endregion

        //=========================================================================
        #region 网络状态
        //=========================================================================

        /// <summary>
        /// 检查当前设备网络是否可用
        /// 判断：无网络/移动网络/WiFi
        /// </summary>
        /// <returns>true 网络可用，false 不可用</returns>
        public bool CheckNetworkActive()
        {
            // 无网络
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                return false;
            }
            // 移动数据网络（4/5G）
            else if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
            {
                return true;
            }
            // WiFi 网络
            else if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
            {
                return true;
            }
            
            return false;
        }

        #endregion
    }
}