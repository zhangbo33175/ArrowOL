#if EASY_TOUCH_ENABLE

namespace Honor.Runtime
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using XLua;

    public sealed partial class Gestures2D : MonoBehaviour
    {
        /// <summary>
        ///【自定义回调】单指点击开始
        /// LuaTable：Lua中handler
        /// 回调参数：Gesture, Vector3
        /// </summary>
        private List<LuaTable> m_TouchBeginCallbacks;
        public List<LuaTable> TouchBeginCallbacks
        {
            get
            {
                return m_TouchBeginCallbacks;
            }
        }

        /// <summary>
        ///【自定义回调】单指按下
        /// LuaTable：Lua中handler
        /// 回调参数：Gesture, Vector3
        /// </summary>
        private List<LuaTable> m_TouchDownCallbacks;
        public List<LuaTable> TouchDownCallbacks
        {
            get
            {
                return m_TouchDownCallbacks;
            }
        }

        /// <summary>
        ///【自定义回调】单指抬起
        /// LuaTable：Lua中handler
        /// 回调参数：Gesture, Vector3
        /// </summary>
        private List<LuaTable> m_TouchEndCallbacks;
        public List<LuaTable> TouchEndCallbacks
        {
            get
            {
                return m_TouchEndCallbacks;
            }
        }

        /// <summary>
        ///【自定义回调】双指点击开始
        /// LuaTable：Lua中handler
        /// 回调参数：Gesture, Vector3
        /// </summary>
        private List<LuaTable> m_TouchesBeginCallbacks;
        public List<LuaTable> TouchesBeginCallbacks
        {
            get
            {
                return m_TouchesBeginCallbacks;
            }
        }

        /// <summary>
        ///【自定义回调】双指按下
        /// LuaTable：Lua中handler
        /// 回调参数：Gesture, Vector3
        /// </summary>
        private List<LuaTable> m_TouchesDownCallbacks;
        public List<LuaTable> TouchesDownCallbacks
        {
            get
            {
                return m_TouchesDownCallbacks;
            }
        }

        /// <summary>
        ///【自定义回调】双指抬起
        /// LuaTable：Lua中handler
        /// 回调参数：Gesture, Vector3
        /// </summary>
        private List<LuaTable> m_TouchesEndCallbacks;
        public List<LuaTable> TouchesEndCallbacks
        {
            get
            {
                return m_TouchesEndCallbacks;
            }
        }

        /// <summary>
        ///【自定义回调】单指滑动开始
        /// LuaTable：Lua中handler
        /// 回调参数：Gesture, Vector3
        /// </summary>
        private List<LuaTable> m_SwipeBeginCallbacks;
        public List<LuaTable> SwipeBeginCallbacks
        {
            get
            {
                return m_SwipeBeginCallbacks;
            }
        }

        /// <summary>
        ///【自定义回调】单指滑动中
        /// LuaTable：Lua中handler
        /// 回调参数：Gesture, Vector3
        /// </summary>
        private List<LuaTable> m_SwipeCallbacks;
        public List<LuaTable> SwipeCallbacks
        {
            get
            {
                return m_SwipeCallbacks;
            }
        }

        /// <summary>
        ///【自定义回调】单指滑动结束
        /// LuaTable：Lua中handler
        /// 回调参数：Gesture, Vector3
        /// </summary>
        private List<LuaTable> m_SwipeEndCallbacks;
        public List<LuaTable> SwipeEndCallbacks
        {
            get
            {
                return m_SwipeEndCallbacks;
            }
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
            get
            {
                return m_SwipeStableCallbacks;
            }
        }

        /// <summary>
        ///【自定义回调】缩放
        /// LuaTable：Lua中handler
        /// 回调参数：Gesture
        /// </summary>
        private List<LuaTable> m_PinchCallbacks;
        public List<LuaTable> PinchCallbacks
        {
            get
            {
                return m_PinchCallbacks;
            }
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
            get
            {
                return m_PinchStableCallbacks;
            }
        }

        /// <summary>
        ///【自定义回调】选中对象
        /// LuaTable：Lua中handler
        /// 回调参数：Gesture, Vector3, string, GameObject
        /// </summary>
        private List<LuaTable> m_SelectedObjCallbacks;
        public List<LuaTable> SelectedObjCallbacks
        {
            get
            {
                return m_SelectedObjCallbacks;
            }
        }

        /// <summary>
        ///【自定义回调】持续选中对象
        /// LuaTable：Lua中handler
        /// 回调参数：Gesture, Vector3, string, GameObject
        /// </summary>
        private List<LuaTable> m_UpdateSelectedObjCallbacks;
        public List<LuaTable> UpdateSelectedObjCallbacks
        {
            get
            {
                return m_UpdateSelectedObjCallbacks;
            }
        }

        /// <summary>
        ///【自定义回调】释放选中对象
        /// LuaTable：Lua中handler
        /// 回调参数：Gesture, Vector3, string, GameObject
        /// </summary>
        private List<LuaTable> m_UnselectedObjCallbacks;
        public List<LuaTable> UnselectedObjCallbacks
        {
            get
            {
                return m_UnselectedObjCallbacks;
            }
        }

        /// <summary>
        ///【自定义回调】选中对象拖拽开始
        /// LuaTable：Lua中handler
        /// 回调参数：Gesture, Vector3, Vector3, string, GameObject
        /// </summary>
        private List<LuaTable> m_SelectedObjDragBeginCallbacks;
        public List<LuaTable> SelectedObjDragBeginCallbacks
        {
            get
            {
                return m_SelectedObjDragBeginCallbacks;
            }
        }

        /// <summary>
        ///【自定义回调】选中对象拖拽中
        /// LuaTable：Lua中handler
        /// 回调参数：Gesture, Vector3, Vector3, string, GameObject
        /// </summary>
        private List<LuaTable> m_SelectedObjDragCallbacks;
        public List<LuaTable> SelectedObjDragCallbacks
        {
            get
            {
                return m_SelectedObjDragCallbacks;
            }
        }

        /// <summary>
        /// 【自定义回调】选中对象拖拽结束
        /// LuaTable：Lua中handler
        /// 回调参数：Gesture, Vector3, Vector3, string, GameObject
        /// </summary>
        private List<LuaTable> m_SelectedObjDragEndCallbacks;
        public List<LuaTable> SelectedObjDragEndCallbacks
        {
            get
            {
                return m_SelectedObjDragEndCallbacks;
            }
        }
    }
}
#endif
