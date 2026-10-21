/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  HonorNetManager.Visitors.cs
 * author:    云毅
 * created:   2026
 * descrip:   网络模块门面管理器 - 便捷状态与访问（partial）
 *            提供门面就绪状态等便捷属性，提高调用可读性
 ***************************************************************/

namespace Honor.Runtime
{
    public sealed partial class HonorNetManager
    {
        //=========================================================================

        #region 组件引用

        //=========================================================================

        /// <summary>
        /// 网络组件（socket、http、websocket、消息收发）
        /// </summary>
        public NetworkComponent Network { get; private set; }

        #endregion

        //=========================================================================

        #region 便捷状态与访问

        //=========================================================================

        /// <summary>
        /// 网络组件是否已就绪
        /// </summary>
        public bool IsNetworkReady => Network != null;

        #endregion
    }
}
