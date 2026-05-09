using UnityEngine;

namespace Honor.Runtime
{
    public sealed partial class NetworkComponent : GameComponent
    {
        /// <summary>
        /// 网络连接超时时间（单位：秒）
        /// 用于建立网络连接时的超时判断
        /// </summary>
        [SerializeField]
        private float m_ConnectTimeout = 20f;
        
        /// <summary>
        /// 获取连接超时时间
        /// </summary>
        public float ConnectTimeout
        {
            get
            {
                return m_ConnectTimeout;
            }
        }

        /// <summary>
        /// 网络请求超时时间（单位：秒）
        /// 用于数据请求/下载时的超时判断
        /// </summary>
        [SerializeField]
        private float m_RequestTimeout = 60f;
        
        /// <summary>
        /// 获取请求超时时间
        /// </summary>
        public float RequestTimeout
        {
            get
            {
                return m_RequestTimeout;
            }
        }

        /// <summary>
        /// 底层网络管理实例
        /// 负责网络状态、请求、连接的实际管理
        /// </summary>
        private NetworkManager m_NetworkManager;
    }
}