#if EASY_TOUCH_ENABLE
using HedgehogTeam.EasyTouch;
#endif
using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// 触摸输入组件
    /// 基于 EasyTouch 插件封装，统一管理 2D/3D/UI 手势交互
    /// 依赖 EASY_TOUCH_ENABLE 宏定义控制启用
    /// </summary>
    [DisallowMultipleComponent]
    public sealed partial class TouchComponent : GameComponent
    {
        /// <summary>
        /// 初始化：获取并缓存所有 EasyTouch 相关组件
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

#if EASY_TOUCH_ENABLE
            // 获取当前物体上所有手势组件
            m_Gestures2D = GetComponent<Gestures2D>();
            m_Gestures3D = GetComponent<Gestures3D>();
            m_GesturesUI = GetComponent<GesturesUI>();
            m_EasyTouch = GetComponent<EasyTouch>();

            // 刷新开关状态（触发组件内部逻辑）
            m_Gestures2D.EnableSwitch = m_Gestures2D.EnableSwitch;
            m_Gestures3D.EnableSwitch = m_Gestures3D.EnableSwitch;
            m_GesturesUI.EnableSwitch = m_GesturesUI.EnableSwitch;
#endif
        }

        /// <summary>
        /// 启动逻辑（预留）
        /// </summary>
        private void Start()
        {
        }

        /// <summary>
        /// 销毁逻辑（预留）
        /// </summary>
        private void OnDestroy()
        {
        }
    }
}