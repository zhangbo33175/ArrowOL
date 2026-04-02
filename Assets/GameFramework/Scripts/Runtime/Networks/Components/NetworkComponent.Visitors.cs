using UnityEngine;

namespace Honor.Runtime
{
    public sealed partial class NetworkComponent : GameComponent
    {
        /// <summary>
        /// 默认网络连接超时时间
        /// 默认时间20s
        /// </summary>
        [SerializeField]
        private float m_ConnectTimeout = 20f;
        public float ConnectTimeout
        {
            get
            {
                return m_ConnectTimeout;
            }
        }

        /// <summary>
        /// 默认网络请求超时时间
        /// 默认时间60s
        /// </summary>
        [SerializeField]
        private float m_RequestTimeout = 60f;
        public float RequestTimeout
        {
            get
            {
                return m_RequestTimeout;
            }
        }

        /// <summary>
        /// 网络管理器
        /// </summary>
        private NetworkManager m_NetworkManager;

    }

}


