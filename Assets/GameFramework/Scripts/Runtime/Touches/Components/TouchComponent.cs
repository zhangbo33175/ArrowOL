
#if EASY_TOUCH_ENABLE
using HedgehogTeam.EasyTouch;
#endif
using UnityEngine;

namespace Honor.Runtime
{
    [DisallowMultipleComponent]
    public sealed partial class TouchComponent : GameComponent
    {
        protected override void Awake()
        {
            base.Awake();

#if EASY_TOUCH_ENABLE

            m_Gestures2D = GetComponent<Gestures2D>();
            m_Gestures3D = GetComponent<Gestures3D>();
            m_GesturesUI = GetComponent<GesturesUI>();
            m_EasyTouch = GetComponent<EasyTouch>();

            // 开关刷新
            m_Gestures2D.EnableSwitch = m_Gestures2D.EnableSwitch;
            m_Gestures3D.EnableSwitch = m_Gestures3D.EnableSwitch;
            m_GesturesUI.EnableSwitch = m_GesturesUI.EnableSwitch;
#endif

        }

        private void Start()
        {

        }

        private void OnDestroy()
        {

        }

    }

}


