/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  Gestures2D.Callbacks.cs
 * author:    云毅
 * created:   2026
 * descrip:   2D相机手势控制器 - 回调事件定义分部类
 ***************************************************************/
#if EASY_TOUCH_ENABLE

namespace Honor.Runtime
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using XLua;

    //=========================================================================
    // 2D 相机手势控制器 - 回调事件分部类
    //=========================================================================
    public sealed partial class Gestures2D : MonoBehaviour
    {
        #region 触摸回调事件
        /// <summary>
        ///【自定义回调】单指点击开始
        /// LuaTable：Lua中handler
        /// 回调参数：Gesture, Vector3
        /// </summary>
        private List<LuaTable> m_TouchBeginCallbacks;
        public List<LuaTable> TouchBeginCallbacks
        {
            get => m_TouchBeginCallbacks;
        }

        /// <summary>
        ///【自定义回调】单指按下
        /// LuaTable：Lua中handler
        /// 回调参数：Gesture, Vector3
        /// </summary>
        private List<LuaTable> m_TouchDownCallbacks;
        public List<LuaTable> TouchDownCallbacks
        {
            get => m_TouchDownCallbacks;
        }

        /// <summary>
        ///【自定义回调】单指抬起
        /// LuaTable：Lua中handler
        /// 回调参数：Gesture, Vector3
        /// </summary>
        private List<LuaTable> m_TouchEndCallbacks;
        public List<LuaTable> TouchEndCallbacks
        {
            get => m_TouchEndCallbacks;
        }

        /// <summary>
        ///【自定义回调】双指点击开始
        /// LuaTable：Lua中handler
        /// 回调参数：Gesture, Vector3
        /// </summary>
        private List<LuaTable> m_TouchesBeginCallbacks;
        public List<LuaTable> TouchesBeginCallbacks
        {
            get => m_TouchesBeginCallbacks;
        }

        /// <summary>
        ///【自定义回调】双指按下
        /// LuaTable：Lua中handler
        /// 回调参数：Gesture, Vector3
        /// </summary>
        private List<LuaTable> m_TouchesDownCallbacks;
        public List<LuaTable> TouchesDownCallbacks
        {
            get => m_TouchesDownCallbacks;
        }

        /// <summary>
        ///【自定义回调】双指抬起
        /// LuaTable：Lua中handler
        /// 回调参数：Gesture, Vector3
        /// </summary>
        private List<LuaTable> m_TouchesEndCallbacks;
        public List<LuaTable> TouchesEndCallbacks
        {
            get => m_TouchesEndCallbacks;
        }
        #endregion

        #region 滑动回调事件
        /// <summary>
        ///【自定义回调】单指滑动开始
        /// LuaTable：Lua中handler
        /// 回调参数：Gesture, Vector3
        /// </summary>
        private List<LuaTable> m_SwipeBeginCallbacks;
        public List<LuaTable> SwipeBeginCallbacks
        {
            get => m_SwipeBeginCallbacks;
        }

        /// <summary>
        ///【自定义回调】单指滑动中
        /// LuaTable：Lua中handler
        /// 回调参数：Gesture, Vector3
        /// </summary>
        private List<LuaTable> m_SwipeCallbacks;
        public List<LuaTable> SwipeCallbacks
        {
            get => m_SwipeCallbacks;
        }

        /// <summary>
        ///【自定义回调】单指滑动结束
        /// LuaTable：Lua中handler
        /// 回调参数：Gesture, Vector3
        /// </summary>
        private List<LuaTable> m_SwipeEndCallbacks;
        public List<LuaTable> SwipeEndCallbacks
        {
            get => m_SwipeEndCallbacks;
        }

        /// <summary>
        ///【自定义回调】单指滑动惯性稳定
        /// 每次惯性稳定后仅回调一次
        /// LuaTable：Lua中handler
        /// 回调参数：Gesture
        /// </summary>
        private List<LuaTable> m_SwipeStableCallbacks;
        public List<LuaTable> SwipeStableCallbacks
        {
            get => m_SwipeStableCallbacks;
        }
        #endregion

        #region 缩放回调事件
        /// <summary>
        ///【自定义回调】缩放
        /// LuaTable：Lua中handler
        /// 回调参数：Gesture
        /// </summary>
        private List<LuaTable> m_PinchCallbacks;
        public List<LuaTable> PinchCallbacks
        {
            get => m_PinchCallbacks;
        }

        /// <summary>
        ///【自定义回调】缩放惯性稳定
        /// 每次惯性稳定后仅回调一次
        /// LuaTable：Lua中handler
        /// 回调参数：Gesture
        /// </summary>
        private List<LuaTable> m_PinchStableCallbacks;
        public List<LuaTable> PinchStableCallbacks
        {
            get => m_PinchStableCallbacks;
        }
        #endregion

        #region 对象选中与拖拽回调事件
        /// <summary>
        ///【自定义回调】选中对象
        /// LuaTable：Lua中handler
        /// 回调参数：Gesture, Vector3, string, GameObject
        /// </summary>
        private List<LuaTable> m_SelectedObjCallbacks;
        public List<LuaTable> SelectedObjCallbacks
        {
            get => m_SelectedObjCallbacks;
        }

        /// <summary>
        ///【自定义回调】持续选中对象
        /// LuaTable：Lua中handler
        /// 回调参数：Gesture, Vector3, string, GameObject
        /// </summary>
        private List<LuaTable> m_UpdateSelectedObjCallbacks;
        public List<LuaTable> UpdateSelectedObjCallbacks
        {
            get => m_UpdateSelectedObjCallbacks;
        }

        /// <summary>
        ///【自定义回调】释放选中对象
        /// LuaTable：Lua中handler
        /// 回调参数：Gesture, Vector3, string, GameObject
        /// </summary>
        private List<LuaTable> m_UnselectedObjCallbacks;
        public List<LuaTable> UnselectedObjCallbacks
        {
            get => m_UnselectedObjCallbacks;
        }

        /// <summary>
        ///【自定义回调】选中对象拖拽开始
        /// LuaTable：Lua中handler
        /// 回调参数：Gesture, Vector3, Vector3, string, GameObject
        /// </summary>
        private List<LuaTable> m_SelectedObjDragBeginCallbacks;
        public List<LuaTable> SelectedObjDragBeginCallbacks
        {
            get => m_SelectedObjDragBeginCallbacks;
        }

        /// <summary>
        ///【自定义回调】选中对象拖拽中
        /// LuaTable：Lua中handler
        /// 回调参数：Gesture, Vector3, Vector3, string, GameObject
        /// </summary>
        private List<LuaTable> m_SelectedObjDragCallbacks;
        public List<LuaTable> SelectedObjDragCallbacks
        {
            get => m_SelectedObjDragCallbacks;
        }

        /// <summary>
        /// 【自定义回调】选中对象拖拽结束
        /// LuaTable：Lua中handler
        /// 回调参数：Gesture, Vector3, Vector3, string, GameObject
        /// </summary>
        private List<LuaTable> m_SelectedObjDragEndCallbacks;
        public List<LuaTable> SelectedObjDragEndCallbacks
        {
            get => m_SelectedObjDragEndCallbacks;
        }
        #endregion
    }
}
#endif