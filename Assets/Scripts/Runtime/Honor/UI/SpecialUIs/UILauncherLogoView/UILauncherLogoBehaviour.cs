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
        /// 进入动画：背景从黑色 渐亮 → 透明
        /// </summary>
        public override void Enter()
        {
            base.Enter();

            // 初始化背景为纯黑
            m_BgImage.color = new Color(0, 0, 0, 1);

            // 先杀死可能存在的旧动画，防止冲突
            DOTween.Kill(GameDOTweenTypes.ProcedureTransitionEnterAnimation);

            // 播放淡入动画：黑色 → 透明
            DOTween.Sequence()
                .Append(DOTween.To(
                    () => m_BgImage.color,
                    color => m_BgImage.color = color,
                    new Color(0, 0, 0, 0),
                    m_EnterDuration))
                .SetUpdate(true) // 不受游戏暂停影响
                .AppendCallback(() =>
                {
                    // 动画结束 → 通知进入完成
                    EnterOver();
                })
                .id = GameDOTweenTypes.ProcedureTransitionEnterAnimation; // 设置动画ID，方便管理
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
            DOTween.Kill(GameDOTweenTypes.ProcedureTransitionExitAnimation);

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
                .id = GameDOTweenTypes.ProcedureTransitionExitAnimation;
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