/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  Gestures3D.cs
 * author:    云毅
 * created:   2026
 * descrip:   3D相机手势控制器 - 生命周期与公共接口
 ***************************************************************/
#if EASY_TOUCH_ENABLE
namespace Honor.Runtime
{
    using UnityEngine;

    //=========================================================================
    // 3D 相机手势控制器 - 生命周期与公共方法
    //=========================================================================
    public sealed partial class Gestures3D : MonoBehaviour
    {
        #region Unity 生命周期
        private void Awake()
        {
            
        }

        private void Start()
        {
            
        }

        private void OnEnable()
        {
            // 清理缓存数据
            CleanCaches();
        }

        private void OnDisable()
        {
            
        }
        #endregion

        #region 公共重置接口
        /// <summary>
        /// 重置滑动状态
        /// </summary>
        public void ResetSwipe()
        {
            
        }

        /// <summary>
        /// 重置缩放状态
        /// </summary>
        public void ResetPinch()
        {
            
        }
        #endregion
    }
}
#endif