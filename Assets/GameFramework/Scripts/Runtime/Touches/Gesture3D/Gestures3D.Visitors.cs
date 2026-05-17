/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  Gestures3D.Config.cs
 * author:    云毅
 * created:   2026
 * descrip:   3D相机手势控制器 - 配置与字段分部类
 ***************************************************************/
#if EASY_TOUCH_ENABLE
namespace Honor.Runtime
{
    using UnityEngine;

    //=========================================================================
    // 3D 相机手势控制器 - 配置与私有字段
    //=========================================================================
    public sealed partial class Gestures3D : MonoBehaviour
    {
        #region 总开关与功能开关
        /// <summary>
        /// 总开关
        /// </summary>
        [SerializeField]
        [GameTitle("总开关 (EnableSwitch)")]
        private bool m_EnableSwitch;
        public bool EnableSwitch
        {
            set
            {
                m_EnableSwitch = value;
                enabled = m_EnableSwitch;
            }
            get
            {
                return m_EnableSwitch;
            }
        }
        #endregion
    }
}
#endif