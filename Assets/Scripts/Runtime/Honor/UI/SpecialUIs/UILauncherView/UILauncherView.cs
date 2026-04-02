using DG.Tweening;
using System;
using UnityEngine;

namespace Honor.Runtime
{
    public sealed class UILauncherView : MonoBehaviour
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
            DOTween.Sequence().AppendInterval(GameMainRoot.Procedure.SplashProcedureDuration).AppendCallback(()=> { 
                if(DurationOverCallback != null)
                {
                    DurationOverCallback();
                }
            }).id = GameDOTweenTypes.SplashDurationAnimation;
        }

        private void OnDestroy()
        {

        }

    }

}


