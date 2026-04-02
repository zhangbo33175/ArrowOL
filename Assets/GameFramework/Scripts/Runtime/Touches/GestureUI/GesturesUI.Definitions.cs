#if EASY_TOUCH_ENABLE
namespace Honor.Runtime
{
    using HedgehogTeam.EasyTouch;
    using System.Collections.Generic;
    using UnityEngine;

    public sealed partial class GesturesUI : MonoBehaviour
    {
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

    }

}
#endif