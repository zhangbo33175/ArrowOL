using System;
using UnityEngine;

namespace Honor.Runtime
{
    public class UISplashBehaviour:MonoBehaviour
    {
        /// <summary>
        /// 延迟计数结束回调
        /// </summary>
        public Action DurationOverCallback;

        private void Awake()
        {
            DurationOverCallback = null;
        }

        private void Start()
        {
            /*DOTween.Sequence().AppendInterval(Root.Procedure.SplashProcedureDuration).AppendCallback(()=> { 
                if(DurationOverCallback != null)
                {
                    DurationOverCallback();
                }
            }).stringId = DOTweenTypes.SplashDurationAnimation;*/
        }

        private void OnDestroy()
        {

        }

    }
}