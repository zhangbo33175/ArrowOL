#if EASY_TOUCH_ENABLE
namespace Honor.Runtime
{
    using UnityEngine;

    public sealed partial class Gestures3D : MonoBehaviour
    {
        void Awake()
        {

        }

        void Start()
        {

        }

        void OnEnable()
        {
            // 清理缓存数据
            CleanCaches();
        }

        void OnDisable()
        {

        }

        public void ResetSwipe()
        {

        }

        public void ResetPinch()
        {

        }

    }
}
#endif
