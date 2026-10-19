/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  UILauncherLogoBehaviour.cs
 * author:    云毅
 * created:   2026
 * descrip:   启动器 Logo 界面行为脚本
 *            控制启动 Logo 的淡入、淡出全屏过渡动画
 ***************************************************************/

using DG.Tweening;
using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// 启动器 Logo 界面行为脚本
    /// 功能：控制启动 Logo 的淡入、淡出全屏过渡动画
    /// 父类：UILauncherView 基础视图组件
    /// </summary>
    public sealed class UILauncherLogoBehaviour : UILauncherLogoView
    {
        #region 公共动画方法
        //=========================================================================
        // 公共动画方法
        //=========================================================================
        /// <summary>
        /// 进入动画：闪屏（__SplashImage）淡入 → 停留 → 闪屏+背景淡出（露出加载界面）
        /// 无黑屏保持段：启动即平滑淡入闪屏，闪屏显示共 3.0 秒
        /// 时序：淡入 0.5s → 停留 2.5s → 淡出 0.6s
        /// 淡入淡出均带缓动曲线（Ease.InOutQuad），使启动过渡更丝滑；
        /// 淡出起始点与 Loading 提前拉起时机（ProcedurePreload 的 SplashFadeOutStart）对齐，
        /// 实现“闪屏淡出、加载界面随之浮现”的流畅衔接
        /// </summary>
        public override void Enter()
        {
            base.Enter();

            // 初始状态：背景纯黑（变黑），闪屏透明
            m_BgImage.color = new Color(0, 0, 0, 1);
            bool hasSplash = m_SplashImage != null;
            if (hasSplash)
                m_SplashImage.color = new Color(1, 1, 1, 0);

            // 先杀死可能存在的旧动画，防止冲突
            DOTween.Kill(GameDOTweenTypes.ProcedureTransitionInTween);

            Sequence seq = DOTween.Sequence().SetUpdate(true); // 不受游戏暂停影响

            if (hasSplash)
            {
                // 阶段0：黑屏保持（可设为 0 去掉黑屏，启动即淡入闪屏）
                if (SplashBlackHoldDuration > 0)
                    seq.AppendInterval(SplashBlackHoldDuration);

                // 阶段1：闪屏淡入（黑屏背景上，带缓动）→ 阶段2：停留展示
                seq.Append(DOTween.To(
                        () => m_SplashImage.color,
                        color => m_SplashImage.color = color,
                        new Color(1, 1, 1, 1),
                        SplashFadeInDuration)
                        .SetEase(Ease.InOutQuad))
                    .AppendInterval(SplashStayDuration)
                    // 阶段3：闪屏淡出 + 背景淡出（同步、带缓动，露出下方加载界面）
                    .Append(DOTween.To(
                        () => m_SplashImage.color,
                        color => m_SplashImage.color = color,
                        new Color(1, 1, 1, 0),
                        SplashFadeOutDuration)
                        .SetEase(Ease.InOutQuad))
                    .Join(DOTween.To(
                        () => m_BgImage.color,
                        color => m_BgImage.color = color,
                        new Color(0, 0, 0, 0),
                        SplashFadeOutDuration)
                        .SetEase(Ease.InOutQuad));
            }
            else
            {
                // 无闪屏：背景黑 → 透明（快速淡出，带缓动）
                seq.Append(DOTween.To(
                    () => m_BgImage.color,
                    color => m_BgImage.color = color,
                    new Color(0, 0, 0, 0),
                    SplashFadeOutDuration)
                    .SetEase(Ease.InOutQuad));
            }

            seq.AppendCallback(() =>
                {
                    // 动画结束 → 通知进入完成
                    EnterOver();
                })
                .id = GameDOTweenTypes.ProcedureTransitionInTween; // 设置动画ID，方便管理
        }

        /// <summary>
        /// 进入动画结束回调
        /// </summary>
        public override void EnterOver()
        {
            base.EnterOver();
        }

        /// <summary>
        /// 退出动画：背景从透明 渐黑 → 纯黑
        /// </summary>
        public override void Exit()
        {
            base.Exit();

            // 杀死可能存在的旧动画
            DOTween.Kill(GameDOTweenTypes.ProcedureTransitionOutTween);

            // 播放淡出动画：透明 → 黑色
            DOTween.Sequence()
                .Append(DOTween.To(
                    () => m_BgImage.color,
                    color => m_BgImage.color = color,
                    new Color(0, 0, 0, 1),
                    m_ExitDuration))
                .SetUpdate(true) // 不受游戏暂停影响
                .AppendCallback(() =>
                {
                    // 动画结束 → 通知退出完成
                    ExitOver();
                })
                .id = GameDOTweenTypes.ProcedureTransitionOutTween;
        }

        /// <summary>
        /// 退出动画结束回调
        /// </summary>
        public override void ExitOver()
        {
            base.ExitOver();
        }
        #endregion
    }
}