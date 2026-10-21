/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  HonorUtilsManager.cs
 * author:    云毅
 * created:   2026
 * descrip:   工具模块门面管理器 - HonorUtils 分类统一入口（主文件）
 *            统一管理触摸组件与通用工具，外部工具调用统一走本管理器
 *            组件引用与工具清单见 HonorUtilsManager.Visitors.cs；分类能力说明见 HonorUtilsManager.Methods.cs
 ***************************************************************/

using System;

namespace Honor.Runtime
{
    /// <summary>
    /// 工具模块门面管理器（HonorUtils 分类统一入口）
    /// 功能：集中管理触摸输入组件 TouchComponent，并汇总本分类下通用工具类清单及访问方式
    /// 外部调用：HonorUtilsManager.Instance.Touch.xxx 访问触摸，工具类通过静态方式访问
    /// 说明：采用纯代码单例，首次访问 Instance 自动调用 Init()
    /// </summary>
    public sealed partial class HonorUtilsManager : Singleton<HonorUtilsManager>
    {
        //=========================================================================

        #region 生命周期

        //=========================================================================

        /// <summary>
        /// 初始化：创建并初始化触摸组件实例
        /// </summary>
        public override void Init()
        {
            Touch = new TouchComponent();
            Touch.Init();
        }

        /// <summary>
        /// 释放：释放触摸组件并置空引用
        /// </summary>
        public override void Dispose()
        {
            try { Touch?.Dispose(); } catch (Exception e) { Log.Error(e); }
            Touch = null;
        }

        #endregion
    }
}
