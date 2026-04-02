using DG.Tweening;
using UnityEngine;

namespace Honor.Runtime
{
    public sealed class UILauncherLogoBehaviour : UILauncherLogoView
    {
        /// <summary>
        /// 进入切换（渐亮）
        /// </summary>
        public override void Enter()
        {
            base.Enter();

            m_BgImage.color = new Color(0, 0, 0, 1);
            DOTween.Kill(GameDOTweenTypes.ProcedureTransitionEnterAnimation);
            DOTween.Sequence().Append(DOTween.To(() => m_BgImage.color, (color) => m_BgImage.color = color, new Color(0, 0, 0, 0), m_EnterDuration)).
                SetUpdate(true).
                AppendCallback(() =>
                               {
                                   EnterOver();
                               }).id = GameDOTweenTypes.ProcedureTransitionEnterAnimation;
        }

        /// <summary>
        /// 进入切换（渐亮）结束
        /// </summary>
        public override void EnterOver()
        {
            base.EnterOver();
        }

        /// <summary>
        /// 退出切换（渐黑）
        /// </summary>
        public override void Exit()
        {
            base.Exit();

            DOTween.Kill(GameDOTweenTypes.ProcedureTransitionExitAnimation);
            DOTween.Sequence().Append(DOTween.To(() => m_BgImage.color, (color) => m_BgImage.color = color, new Color(0, 0, 0, 1), m_ExitDuration)).
                SetUpdate(true).
                AppendCallback(() =>
            {
                ExitOver();
            }).id = GameDOTweenTypes.ProcedureTransitionExitAnimation;
        }

        /// <summary>
        /// 退出切换（渐黑）结束
        /// </summary>
        public override void ExitOver()
        {
            base.ExitOver();
        }

    }

}


