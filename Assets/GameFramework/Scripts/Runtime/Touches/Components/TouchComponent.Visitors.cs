#if EASY_TOUCH_ENABLE
using HedgehogTeam.EasyTouch;
#endif
using UnityEngine;

namespace Honor.Runtime
{
    public sealed partial class TouchComponent : GameComponent
    {
#if EASY_TOUCH_ENABLE
        /// <summary>
        /// 相机手势操作器(2D相机)
        /// </summary>
        private Gestures2D m_Gestures2D;
        public Gestures2D Gestures2D
        {
            get
            {
                return m_Gestures2D;
            }
        }

        /// <summary>
        /// 相机手势操作器(3D相机)
        /// </summary>
        private Gestures3D m_Gestures3D;
        public Gestures3D Gestures3D
        {
            get
            {
                return m_Gestures3D;
            }
        }

        /// <summary>
        /// 相机手势操作器(UI相机)
        /// </summary>
        private GesturesUI m_GesturesUI;
        public GesturesUI GesturesUI
        {
            get
            {
                return m_GesturesUI;
            }
        }

        /// <summary>
        /// EasyTouch插件
        /// </summary>
        private EasyTouch m_EasyTouch;
        public EasyTouch EasyTouch
        {
            get
            {
                return m_EasyTouch;
            }
        }
#endif

    }

}


