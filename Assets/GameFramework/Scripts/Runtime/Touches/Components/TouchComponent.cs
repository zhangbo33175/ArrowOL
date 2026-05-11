#if EASY_TOUCH_ENABLE
using HedgehogTeam.EasyTouch;
#endif
using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// 触摸输入管理组件
    /// 基于 EasyTouch 插件进行封装，统一管理 2D / 3D / UI 相机手势交互
    /// 通过 EASY_TOUCH_ENABLE 宏开关控制功能启用状态
    /// </summary>
    [DisallowMultipleComponent]
    public sealed partial class TouchComponent : GameComponent
    {
        /// <summary>
        /// 组件初始化
        /// 缓存所有手势相关组件实例，并刷新组件开关状态
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

#if EASY_TOUCH_ENABLE
            // 获取当前对象上挂载的各类手势控制器
            m_Gestures2D = GetComponent<Gestures2D>();
            m_Gestures3D = GetComponent<Gestures3D>();
            m_GesturesUI = GetComponent<GesturesUI>();
            m_EasyTouch = GetComponent<EasyTouch>();

            // 重新赋值启用开关，触发组件内部初始化逻辑
            m_Gestures2D.EnableSwitch = m_Gestures2D.EnableSwitch;
            m_Gestures3D.EnableSwitch = m_Gestures3D.EnableSwitch;
            m_GesturesUI.EnableSwitch = m_GesturesUI.EnableSwitch;
#endif
        }

        /// <summary>
        /// 组件启动逻辑（预留扩展）
        /// </summary>
        private void Start()
        {
        }

        /// <summary>
        /// 组件销毁逻辑（预留扩展）
        /// </summary>
        private void OnDestroy()
        {
        }
    }
}