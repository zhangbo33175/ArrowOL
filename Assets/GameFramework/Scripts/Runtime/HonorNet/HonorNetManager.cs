/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  HonorNetManager.cs
 * author:    云毅
 * created:   2026
 * descrip:   网络模块门面管理器 - HonorNet 分类统一入口（主文件）
 *            统一管理网络组件，外部网络调用统一走本管理器
 *            组件引用与便捷状态见 HonorNetManager.Visitors.cs；便捷业务方法见 HonorNetManager.Methods.cs
 ***************************************************************/

using System;

namespace Honor.Runtime
{
    /// <summary>
    /// 网络模块门面管理器（HonorNet 分类统一入口）
    /// 功能：集中管理网络组件 NetworkComponent（socket、http、websocket、消息收发）
    /// 外部调用：HonorNetManager.Instance.CheckNetworkActive()，或 .Network.xxx 访问组件完整能力
    /// 说明：采用纯代码单例，首次访问 Instance 自动调用 Init()
    /// </summary>
    public sealed partial class HonorNetManager : Singleton<HonorNetManager>
    {
        //=========================================================================

        #region 生命周期

        //=========================================================================

        /// <summary>
        /// 初始化：创建并初始化网络组件实例
        /// </summary>
        public override void Init()
        {
            Network = new NetworkComponent();
            Network.Init();
        }

        /// <summary>
        /// 释放：释放网络组件并置空引用
        /// </summary>
        public override void Dispose()
        {
            try { Network?.Dispose(); } catch (Exception e) { Log.Error(e); }
            Network = null;
        }

        #endregion
    }
}
