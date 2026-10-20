/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  TouchComponent.cs
 * author:    云毅
 * created:   2026
 * descrip:   触摸输入管理组件，基于EasyTouch封装，统一手势交互管理
 ***************************************************************/
#if EASY_TOUCH_ENABLE
using HedgehogTeam.EasyTouch;
#endif
using UnityEngine;

namespace Honor.Runtime
{
    //=========================================================================
    // 触摸输入管理组件
    //=========================================================================
    /// <summary>
    /// 触摸输入管理组件
    /// 基于 EasyTouch 插件进行封装，统一管理 2D / 3D / UI 相机手势交互
    /// 通过 EASY_TOUCH_ENABLE 宏开关控制功能启用状态
    /// </summary>
    [DisallowMultipleComponent]
    public sealed partial class TouchComponent : Singleton<TouchComponent>
    {
        #region 私有成员字段
        /// <summary>
        /// 组件初始化
        /// 缓存所有手势相关组件实例，并刷新组件开关状态
        /// </summary>
        public void Init()
        {
#if EASY_TOUCH_ENABLE
            m_TouchGo = new GameObject("[TouchComponent]");
            Object.DontDestroyOnLoad(m_TouchGo);
            m_EasyTouch = m_TouchGo.AddComponent<EasyTouch>();
            m_Gestures2D = m_TouchGo.AddComponent<Gestures2D>();
            m_Gestures3D = m_TouchGo.AddComponent<Gestures3D>();
            m_GesturesUI = m_TouchGo.AddComponent<GesturesUI>();
#endif
        }

        #endregion

        public override void Dispose()
        {
            throw new System.NotImplementedException();
        }
    }
}