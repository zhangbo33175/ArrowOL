/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  HonorUtilsManager.Methods.cs
 * author:    云毅
 * created:   2026
 * descrip:   工具模块门面管理器 - 能力说明（partial）
 *            本门面触摸能力经 Touch 属性访问，通用工具类经静态访问，无独立转发方法
 ***************************************************************/

namespace Honor.Runtime
{
    public sealed partial class HonorUtilsManager
    {
        //=========================================================================

        #region 能力访问说明

        //=========================================================================

        // 触摸输入能力：经 Touch 属性访问完整能力
        //   HonorUtilsManager.Instance.Touch.xxx（点击、手势、输入屏蔽、Gesture2D/3D/UI 控制器）
        //
        // 通用工具类：均为静态类，直接静态调用即可，无需经本管理器实例化。
        // 完整清单见 HonorUtilsManager.Visitors.cs 的「通用工具类清单」区域。

        #endregion
    }
}
