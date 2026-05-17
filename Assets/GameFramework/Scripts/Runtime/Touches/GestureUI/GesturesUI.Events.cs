/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  GesturesUI.Callbacks.cs
 * author:    云毅
 * created:   2026
 * descrip:   UI手势交互控制器 - 回调事件定义分部类
 ***************************************************************/
#if EASY_TOUCH_ENABLE
namespace Honor.Runtime
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using XLua;

    //=========================================================================
    // UI 手势交互控制器 - 回调事件定义
    //=========================================================================
    public sealed partial class GesturesUI : MonoBehaviour
    {
        #region UI 触摸回调
        /// <summary>
        /// 【自定义回调】手势覆盖UI元素中
        /// LuaTable：Lua中handler
        /// 回调参数：Gesture, Vector3
        /// </summary>
        private List<LuaTable> m_UITouchCoverCallbacks;
        public List<LuaTable> UITouchCoverCallbacks
        {
            get => m_UITouchCoverCallbacks;
        }

        /// <summary>
        /// 【自定义回调】手势从UI元素中抬起
        /// LuaTable：Lua中handler
        /// 回调参数：Gesture, Vector3
        /// </summary>
        private List<LuaTable> m_UITouchEndCallbacks;
        public List<LuaTable> UITouchEndCallbacks
        {
            get => m_UITouchEndCallbacks;
        }
        #endregion

        #region 对象选中与拖拽回调
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