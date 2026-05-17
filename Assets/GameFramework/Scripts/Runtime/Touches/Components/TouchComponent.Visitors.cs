/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  TouchComponent.cs
 * author:    云毅
 * created:   2026
 * descrip:   触摸输入组件，管理游戏内所有触摸、手势相关功能
 ***************************************************************/
#if EASY_TOUCH_ENABLE
using HedgehogTeam.EasyTouch;
#endif
using UnityEngine;

namespace Honor.Runtime
{
    //=========================================================================
    // 触摸输入组件 - 手势管理分部类
    //=========================================================================
    /// <summary>
    /// 触摸输入组件，负责管理游戏内所有触摸、手势相关功能
    /// </summary>
    public sealed partial class TouchComponent : GameComponent
    {
        #region 手势控制器成员（EasyTouch 启用时生效）
#if EASY_TOUCH_ENABLE
        /// <summary>
        /// 2D 相机手势控制器
        /// 用于处理 2D 相机的移动、缩放、拖拽等手势操作
        /// </summary>
        private Gestures2D m_Gestures2D;

        /// <summary>
        /// 2D 相机手势控制器（只读属性）
        /// </summary>
        public Gestures2D Gestures2D
        {
            get => m_Gestures2D;
        }

        /// <summary>
        /// 3D 相机手势控制器
        /// 用于处理 3D 相机的旋转、缩放、平移等手势操作
        /// </summary>
        private Gestures3D m_Gestures3D;

        /// <summary>
        /// 3D 相机手势控制器（只读属性）
        /// </summary>
        public Gestures3D Gestures3D
        {
            get => m_Gestures3D;
        }

        /// <summary>
        /// UI 相机手势控制器
        /// 用于处理 UI 相机及界面相关的手势交互
        /// </summary>
        private GesturesUI m_GesturesUI;

        /// <summary>
        /// UI 相机手势控制器（只读属性）
        /// </summary>
        public GesturesUI GesturesUI
        {
            get => m_GesturesUI;
        }

        /// <summary>
        /// EasyTouch 插件核心实例
        /// 提供底层触摸、手势事件支持
        /// </summary>
        private EasyTouch m_EasyTouch;

        /// <summary>
        /// EasyTouch 插件核心实例（只读属性）
        /// </summary>
        public EasyTouch EasyTouch
        {
            get => m_EasyTouch;
        }
#endif
        #endregion
    }
}