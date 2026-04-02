#if EASY_TOUCH_ENABLE
namespace Honor.Runtime
{
    using UnityEngine;

    public sealed partial class Gestures3D : MonoBehaviour
    {
        /// <summary>
        /// 总开关
        /// </summary>
        [SerializeField]
        [HonorTitle("总开关 (EnableSwitch)")]
        private bool m_EnableSwitch;
        public bool EnableSwitch
        {
            set
            {
                m_EnableSwitch = value;
                enabled = m_EnableSwitch;
            }
            get
            {
                return m_EnableSwitch;
            }
        }
    }
}
#endif