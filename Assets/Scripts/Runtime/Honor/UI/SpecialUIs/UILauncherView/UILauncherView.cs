/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  UILauncherView.cs
 * author:    云毅
 * created:   2026
 * descrip:   启动器 Logo 展示界面
 *            首帧渲染后提前触发重活（Lua 初始化），最短展示时长与初始化就绪同时满足后进入下一步
 ***************************************************************/

using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// 启动器 Logo 展示界面
    /// 功能：
    /// 1. 首帧渲染后立即触发 <see cref="InitStartCallback"/>，提前启动重活（如 Lua 初始化），与闪屏展示重叠
    /// 2. 最短展示时长（SplashProcedureDuration）与初始化就绪（<see cref="SetReady"/>）同时满足后，
    ///    触发 <see cref="DurationOverCallback"/> 进入下一步
    /// 目的：将“固定闪屏等待”与“同步 Lua 启动”由串行改为并行，缩短启动黑屏时长
    /// </summary>
    public sealed class UILauncherView : MonoBehaviour
    {
        #region 公共委托 & 字段
        //=========================================================================
        // 公共委托 & 字段
        //=========================================================================
        /// <summary>
        /// 首帧渲染后的初始化启动回调
        /// 外部在回调内执行重活（如 Lua 初始化），完成后调用 <see cref="SetReady"/>
        /// </summary>
        public Action InitStartCallback;

        /// <summary>
        /// 最短展示时长与初始化就绪均满足后的进入回调
        /// </summary>
        public Action DurationOverCallback;
        #endregion

        #region 私有字段
        //=========================================================================
        // 私有字段
        //=========================================================================
        /// <summary>
        /// 初始化（重活）是否完成
        /// </summary>
        private bool m_Ready;

        /// <summary>
        /// 最短展示时长是否到达
        /// </summary>
        private bool m_MinTimeElapsed;
        #endregion

        #region 生命周期
        //=========================================================================
        // 生命周期
        //=========================================================================
        /// <summary>
        /// 唤醒时重置字段与回调，防止引用残留
        /// </summary>
        private void Awake()
        {
            InitStartCallback = null;
            DurationOverCallback = null;
            m_Ready = false;
            m_MinTimeElapsed = false;
        }

        /// <summary>
        /// 启动：并行处理最短展示时长计时 与 首帧后提前初始化
        /// 二者都满足后再触发进入回调，避免固定时长与 Lua 初始化串行叠加
        /// </summary>
        private void Start()
        {
            // 最短展示时长计时（与 Lua 初始化并行）
            DOTween.Sequence()
                .AppendInterval(GameMainRoot.Procedure.SplashProcedureDuration)
                .AppendCallback(() =>
                {
                    m_MinTimeElapsed = true;
                    TryFinish();
                })
                .id = GameDOTweenTypes.SplashShowTween;

            // 等闪屏首帧渲染后，立即触发重活（提前进入 Lua 初始化）
            StartCoroutine(TriggerInitAfterFirstFrame());
        }

        /// <summary>
        /// 销毁（预留）
        /// </summary>
        private void OnDestroy()
        {
        }
        #endregion

        #region 公共接口
        //=========================================================================
        // 公共接口
        //=========================================================================
        /// <summary>
        /// 外部在初始化（重活）完成后调用，标记就绪
        /// </summary>
        public void SetReady()
        {
            m_Ready = true;
            TryFinish();
        }
        #endregion

        #region 私有方法
        //=========================================================================
        // 私有方法
        //=========================================================================
        /// <summary>
        /// 等待 1 帧确保闪屏已渲染可见，再触发初始化启动回调
        /// </summary>
        private IEnumerator TriggerInitAfterFirstFrame()
        {
            yield return null;
            InitStartCallback?.Invoke();
        }

        /// <summary>
        /// 最短展示时长与初始化就绪均满足时，触发进入回调
        /// </summary>
        private void TryFinish()
        {
            if (m_Ready && m_MinTimeElapsed)
                DurationOverCallback?.Invoke();
        }
        #endregion
    }
}
