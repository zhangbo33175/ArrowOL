/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  UILauncherView.cs
 * author:    云毅
 * created:   2026
 * descrip:   启动器 Logo 展示界面
 *            延时展示 Logo 画面，时间结束后触发回调进入下一个流程
 ***************************************************************/

using DG.Tweening;
using System;
using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// 启动器 Logo 展示界面
    /// 功能：延时展示 Logo 画面，时间结束后触发回调进入下一个流程
    /// </summary>
    public sealed class UILauncherView : MonoBehaviour
    {
        #region 公共委托 & 字段
        //=========================================================================
        // 公共委托 & 字段
        //=========================================================================
        /// <summary>
        /// 延时展示结束后的外部回调
        /// </summary>
        public Action DurationOverCallback;
        #endregion

        #region 生命周期
        //=========================================================================
        // 生命周期
        //=========================================================================
        private void Awake()
        {
            // 初始化回调为空，防止引用残留
            DurationOverCallback = null;
        }

        private void Start()
        {
            // 播放延时动画：等待配置的时间 → 触发回调
            DOTween.Sequence()
                .AppendInterval(GameMainRoot.Procedure.SplashProcedureDuration)
                .AppendCallback(() => { DurationOverCallback?.Invoke(); })
                .id = GameDOTweenTypes.SplashDurationAnimation;
        }

        private void OnDestroy()
        {
        }
        #endregion
    }
}