/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  Gestures3D.Methods.cs
 * author:    云毅
 * created:   2026
 * descrip:   3D相机手势控制器 - 缓存清理逻辑
 ***************************************************************/
#if EASY_TOUCH_ENABLE
namespace Honor.Runtime
{
    using UnityEngine;

    //=========================================================================
    // 3D 相机手势控制器
    //=========================================================================
    /// <summary>
    /// 3D 相机手势控制器 - 内部缓存清理分部类
    /// </summary>
    public sealed partial class Gestures3D : MonoBehaviour
    {
        #region 内部缓存管理
        /// <summary>
        /// 清理缓存数据（当前为空实现，预留 3D 手势状态重置扩展）
        /// </summary>
        private void CleanCaches()
        {

        }
        #endregion
    }
}
#endif