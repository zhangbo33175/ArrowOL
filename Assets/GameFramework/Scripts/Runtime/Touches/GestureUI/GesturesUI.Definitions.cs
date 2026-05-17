/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  GesturesUI.Enums.cs
 * author:    云毅
 * created:   2026
 * descrip:   UI手势交互控制器 - 枚举定义分部类
 ***************************************************************/
#if EASY_TOUCH_ENABLE
namespace Honor.Runtime
{
    using HedgehogTeam.EasyTouch;
    using System.Collections.Generic;
    using UnityEngine;

    //=========================================================================
    // UI 手势交互控制器 - 枚举定义
    //=========================================================================
    public sealed partial class GesturesUI : MonoBehaviour
    {
        #region 局部枚举定义
        /// <summary>
        /// 局部拖拽状态定义
        /// </summary>
        private enum DragState
        { 
            /// <summary>
            /// 待机（已选中）
            /// </summary>
            None = 0,

            /// <summary>
            /// 拖拽开始
            /// </summary>
            Begin,

            /// <summary>
            /// 拖拽中
            /// </summary>
            OnGoing,

            /// <summary>
            /// 拖拽结束
            /// </summary>
            End,
        }
        #endregion
    }
}
#endif