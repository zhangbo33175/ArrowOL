/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  HonorAssetManager.cs
 * author:    云毅
 * created:   2026
 * descrip:   资源模块门面管理器 - HonorAsset 分类统一入口（主文件）
 *            统一管理资源加载组件，外部资源调用统一走本管理器
 *            组件引用与便捷状态见 HonorAssetManager.Visitors.cs；便捷业务方法见 HonorAssetManager.Methods.cs
 ***************************************************************/

using System;

namespace Honor.Runtime
{
    /// <summary>
    /// 资源模块门面管理器（HonorAsset 分类统一入口）
    /// 功能：集中管理资源加载组件 AssetComponent（AB包、普通资源、预制体、场景加载/卸载）
    /// 外部调用：HonorAssetManager.Instance.LoadAssetAsync(...)，或 .Asset.xxx 直接访问组件
    /// 说明：采用纯代码单例，首次访问 Instance 自动调用 Init()
    /// </summary>
    public sealed partial class HonorAssetManager : Singleton<HonorAssetManager>
    {
        //=========================================================================

        #region 生命周期

        //=========================================================================

        /// <summary>
        /// 初始化：创建并初始化资源组件实例
        /// </summary>
        public override void Init()
        {
            Asset = new AssetComponent();
            Asset.Init();
        }

        /// <summary>
        /// 释放：释放资源组件并置空引用
        /// </summary>
        public override void Dispose()
        {
            try { Asset?.Dispose(); } catch (Exception e) { Log.Error(e); }
            Asset = null;
        }

        /// <summary>
        /// 每帧驱动：推进资源组件异步加载/卸载队列
        /// <para>纯代码单例组件自身 Update 不会被 Unity 调用，由 GameMainRoot.Update 每帧调用本方法驱动</para>
        /// </summary>
        public void DriveUpdate()
        {
            if (Asset != null)
            {
                Asset.DriveUpdate();
            }
        }

        #endregion
    }
}
