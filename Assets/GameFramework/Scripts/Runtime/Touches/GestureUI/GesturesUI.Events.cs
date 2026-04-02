#if EASY_TOUCH_ENABLE
namespace Honor.Runtime
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using XLua;

    public sealed partial class GesturesUI : MonoBehaviour
    {
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

        /// <summary>
        /// 【自定义回调】手势覆盖UI元素中
        /// LuaTable：Lua中handler
        /// 回调参数：Gesture, Vector3
        /// </summary>
        private List<LuaTable> m_UITouchCoverCallbacks;
        public List<LuaTable> UITouchCoverCallbacks
        {
            get
            {
                return m_UITouchCoverCallbacks;
            }
        }

        /// <summary>
        /// 【自定义回调】手势从UI元素中抬起
        /// LuaTable：Lua中handler
        /// 回调参数：Gesture, Vector3
        /// </summary>
        private List<LuaTable> m_UITouchEndCallbacks;
        public List<LuaTable> UITouchEndCallbacks
        {
            get
            {
                return m_UITouchEndCallbacks;
            }
        }

    }
}
#endif
