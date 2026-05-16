/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  NetworkComponent.Fields.cs
 * author:    云毅
 * created:
 * descrip:   网络组件 - 字段、属性定义分部类
 ***************************************************************/

using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// 网络管理组件 - 字段与属性模块
    /// </summary>
    public sealed partial class NetworkComponent : GameComponent
    {
        //=========================================================================
        #region 序列化字段
        //=========================================================================

        /// <summary>
        /// 网络连接超时时间（单位：秒）
        /// 用于建立网络连接时的超时判断
        /// </summary>
        [SerializeField]
        private float m_ConnectTimeout = 20f;

        /// <summary>
        /// 网络请求超时时间（单位：秒）
        /// 用于数据请求/下载时的超时判断
        /// </summary>
        [SerializeField]
        private float m_RequestTimeout = 60f;

        #endregion

        //=========================================================================
        #region 公开属性
        //=========================================================================

        /// <summary>
        /// 获取连接超时时间
        /// </summary>
        public float ConnectTimeout
        {
            get { return m_ConnectTimeout; }
        }

        /// <summary>
        /// 获取请求超时时间
        /// </summary>
        public float RequestTimeout
        {
            get { return m_RequestTimeout; }
        }

        #endregion

        //=========================================================================
        #region 私有成员
        //=========================================================================

        /// <summary>
        /// 底层网络管理实例
        /// 负责网络状态、请求、连接的实际管理
        /// </summary>
        private NetworkManager m_NetworkManager;

        #endregion
    }
}