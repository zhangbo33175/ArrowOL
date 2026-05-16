/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  UIFader.cs
 * author:    云毅
 * created:
 * descrip:   UI 全局淡入淡出控制器 - 事件驱动、DOTween 动画、全屏遮罩
 ***************************************************************/

using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.RectTransform;

namespace Honor.Runtime
{
    #region 全局 UI 淡入淡出事件定义
    /// <summary>
    /// UI 淡入事件（全局事件）
    /// </summary>
    public static class UIFadeInEvent
    {
        private static readonly GameEventCmd cmd = GameEventCmd.UIFadeIn;

        /// <summary>
        /// 订阅事件
        /// </summary>
        public static void Subscribe(object userData, HonorEventHandler<EventParams> handler)
        {
            GameMainRoot.Event.Subscribe(cmd, userData, handler);
        }

        /// <summary>
        /// 取消订阅
        /// </summary>
        public static void Unsubscribe(object userData, HonorEventHandler<EventParams> handler)
        {
            GameMainRoot.Event.Unsubscribe(cmd, userData, handler);
        }

        /// <summary>
        /// 派发事件（异步）
        /// </summary>
        public static void Fire(object sender, int id)
        {
            GameMainRoot.Event.Fire(sender, cmd, new Dictionary<string, object>() { { "ID", id } });
        }

        /// <summary>
        /// 立即派发事件（同步）
        /// </summary>
        public static void FireNow(object sender, int id)
        {
            GameMainRoot.Event.FireNow(sender, cmd, new Dictionary<string, object>() { { "ID", id } });
        }
    }

    /// <summary>
    /// UI 淡出事件（全局事件）
    /// </summary>
    public static class UIFadeOutEvent
    {
        private static readonly GameEventCmd cmd = GameEventCmd.UIFadeOut;

        public static void Subscribe(object userData, HonorEventHandler<EventParams> handler)
        {
            GameMainRoot.Event.Subscribe(cmd, userData, handler);
        }

        public static void Unsubscribe(object userData, HonorEventHandler<EventParams> handler)
        {
            GameMainRoot.Event.Unsubscribe(cmd, userData, handler);
        }

        public static void Fire(object sender, int id)
        {
            GameMainRoot.Event.Fire(sender, cmd, new Dictionary<string, object>() { { "ID", id } });
        }

        public static void FireNow(object sender, int id)
        {
            GameMainRoot.Event.FireNow(sender, cmd, new Dictionary<string, object>() { { "ID", id } });
        }
    }

    /// <summary>
    /// UI 停止淡入淡出事件（全局事件）
    /// </summary>
    public static class UIFadeStopEvent
    {
        private static readonly GameEventCmd cmd = GameEventCmd.UIFadeStop;

        public static void Subscribe(object userData, HonorEventHandler<EventParams> handler)
        {
            GameMainRoot.Event.Subscribe(cmd, userData, handler);
        }

        public static void Unsubscribe(object userData, HonorEventHandler<EventParams> handler)
        {
            GameMainRoot.Event.Unsubscribe(cmd, userData, handler);
        }

        public static void Fire(object sender, int id)
        {
            GameMainRoot.Event.Fire(sender, cmd, new Dictionary<string, object>() { { "ID", id } });
        }

        public static void FireNow(object sender, int id)
        {
            GameMainRoot.Event.FireNow(sender, cmd, new Dictionary<string, object>() { { "ID", id } });
        }
    }
    #endregion

    /// <summary>
    /// UI 淡入淡出控制器
    /// 功能：全屏遮罩淡入淡出、事件驱动、DOTween 动画、射线阻塞控制
    /// </summary>
    [AddComponentMenu("Honor Core/UI/UIFader")]
    public partial class UIFader : MonoBehaviour
    {
        /// <summary>
        /// 初始化状态枚举
        /// </summary>
        public enum InitState
        { 
            /// <summary>
            /// 无初始状态
            /// </summary>
            None = 0,
            /// <summary>
            /// 初始为显示状态
            /// </summary>
            Active,
            /// <summary>
            /// 初始为隐藏状态
            /// </summary>
            Inactive
        }

        //=========================================================================
        #region 序列化字段（Inspector 配置）
        //=========================================================================

        [GameHeader("标识")]
        [GameTitle("ID")]
        [Tooltip("唯一标识，用于事件匹配")]
        public int ID = 0;

        [GameHeader("色值")]
        [GameTitle("遮罩层颜色")]
        [Tooltip("遮罩层颜色值")]
        public Color ImageColor = Color.black;

        [GameTitle("未活跃透明度")]
        [Tooltip("隐藏状态下的透明度")]
        public float InactiveAlpha = 0f;

        [GameTitle("活跃透明度")]
        [Tooltip("显示状态下的透明度")]
        public float ActiveAlpha = 1f;

        [GameTitle("初始状态")]
        [Tooltip("组件启动时的默认状态")]
        public InitState InitialState = InitState.Inactive;

        [GameHeader("时间")]
        [GameTitle("渐变时长")]
        [Tooltip("淡入淡出动画持续时间")]
        public float Duration = 0.2f;

        [GameTitle("加速度类型")]
        [Tooltip("动画曲线类型")]
        public Ease TweenEase = Ease.Linear;

        [GameHeader("交互")]
        [GameTitle("阻塞射线")]
        [Tooltip("显示时是否拦截点击事件")]
        public bool ShouldBlockRaycasts = false;

        [GameInspectorButton("TestFadeIn")]
        public bool TestFadeInButton;
        [GameInspectorButton("TestFadeOut")]
        public bool TestFadeOutButton;
        [GameInspectorButton("TestFadeStop")]
        public bool TestFaderStopButton;
        [GameInspectorButton("TestFadeReset")]
        public bool TestFaderResetButton;

        #endregion

        //=========================================================================
        #region 保护变量
        //=========================================================================

        /// <summary>
        /// 画布组，用于控制整体透明度与射线
        /// </summary>
        protected CanvasGroup m_CanvasGroup;

        /// <summary>
        /// 遮罩图片
        /// </summary>
        protected Image m_Image;

        /// <summary>
        /// 动画起始透明度
        /// </summary>
        protected float m_InitialAlpha;

        /// <summary>
        /// 动画目标透明度
        /// </summary>
        protected float m_CurrentTargetAlpha;

        /// <summary>
        /// 当前动画时长
        /// </summary>
        protected float m_CurrentDuration;

        /// <summary>
        /// 当前动画曲线
        /// </summary>
        protected Ease m_CurrentTweenEase;

        /// <summary>
        /// 是否正在播放动画
        /// </summary>
        protected bool m_IsFading;

        #endregion

        //=========================================================================
        #region 生命周期
        //=========================================================================

        /// <summary>
        /// 启用时订阅事件
        /// </summary>
        protected virtual void OnEnable()
        {
            UIFadeStopEvent.Subscribe(this, EventCallback);
            UIFadeInEvent.Subscribe(this, EventCallback);
            UIFadeOutEvent.Subscribe(this, EventCallback);
        }

        /// <summary>
        /// 禁用时取消订阅
        /// </summary>
        protected virtual void OnDisable()
        {
            UIFadeStopEvent.Unsubscribe(this, EventCallback);
            UIFadeInEvent.Unsubscribe(this, EventCallback);
            UIFadeOutEvent.Unsubscribe(this, EventCallback);
        }

        /// <summary>
        /// 初始化组件、尺寸、颜色、初始状态
        /// </summary>
        protected virtual void Awake()
        {
            m_CanvasGroup = gameObject.GetOrAddComponent<CanvasGroup>();
            m_Image = gameObject.GetOrAddComponent<Image>();
            
            m_Image.rectTransform().SetSizeWithCurrentAnchors(Axis.Horizontal, Screen.width);
            m_Image.rectTransform().SetSizeWithCurrentAnchors(Axis.Vertical, Screen.height);
            m_Image.color = ImageColor;

            switch (InitialState)
            {
                case InitState.Inactive:
                    m_CanvasGroup.alpha = InactiveAlpha;
                    m_Image.enabled = false;
                    break;
                case InitState.Active:
                    m_CanvasGroup.alpha = ActiveAlpha;
                    m_Image.enabled = true;
                    break;
            }
        }

        protected virtual void Start() { }

        #endregion

        //=========================================================================
        #region 事件处理
        //=========================================================================

        /// <summary>
        /// 全局事件回调
        /// 根据 ID 匹配并执行淡入/淡出/停止
        /// </summary>
        protected virtual void EventCallback(object sender, object userData, EventParams e)
        {
            int id = e.GetInt("ID");
            if (id != ID) return;
            
            switch (e.Cmd)
            {
                case GameEventCmd.UIFadeStop:
                    StopFading();
                    break;
                case GameEventCmd.UIFadeIn:
                    FadeIn();
                    break;
                case GameEventCmd.UIFadeOut:
                    FadeOut();
                    break;
            }
        }

        #endregion

        //=========================================================================
        #region 淡入淡出控制
        //=========================================================================

        /// <summary>
        /// 执行淡入（显示）
        /// </summary>
        protected virtual void FadeIn()
        {
            StartFading(m_CanvasGroup.alpha, ActiveAlpha, Duration, TweenEase);
        }

        /// <summary>
        /// 执行淡出（隐藏）
        /// </summary>
        protected virtual void FadeOut()
        {
            StartFading(m_CanvasGroup.alpha, InactiveAlpha, Duration, TweenEase);
        }

        /// <summary>
        /// 停止当前动画并设置到目标状态
        /// </summary>
        protected virtual void StopFading()
        {
            DOTween.Kill(GameDOTweenTypes.UIFader + GetInstanceID());
            m_CanvasGroup.alpha = m_CurrentTargetAlpha;
            m_IsFading = false;
            
            if (Mathf.Approximately(m_CanvasGroup.alpha, InactiveAlpha))
            {
                DisableFader();
            }
        }

        /// <summary>
        /// 启动淡入淡出动画
        /// </summary>
        /// <param name="initialAlpha">起始透明度</param>
        /// <param name="endAlpha">目标透明度</param>
        /// <param name="duration">时长</param>
        /// <param name="tweenEase">动画曲线</param>
        protected virtual void StartFading(float initialAlpha, float endAlpha, float duration, Ease tweenEase)
        {
            EnableFader();
            m_IsFading = true;
            m_InitialAlpha = initialAlpha;
            m_CurrentTargetAlpha = endAlpha;
            m_CurrentTweenEase = tweenEase;
            m_CurrentDuration = duration;

            m_CanvasGroup.alpha = m_InitialAlpha;
            DOTween.Kill(GameDOTweenTypes.UIFader + GetInstanceID());
            
            DOTween.To(() => m_CanvasGroup.alpha, alpha => m_CanvasGroup.alpha = alpha, m_CurrentTargetAlpha, m_CurrentDuration)
                .SetEase(m_CurrentTweenEase)
                .OnComplete(StopFading)
                .id = GameDOTweenTypes.UIFader + GetInstanceID();
        }

        #endregion

        //=========================================================================
        #region 状态开关
        //=========================================================================

        /// <summary>
        /// 启用遮罩、开启射线拦截（如配置）
        /// </summary>
        protected virtual void EnableFader()
        {
            m_Image.enabled = true;
            m_CanvasGroup.blocksRaycasts = ShouldBlockRaycasts;
        }

        /// <summary>
        /// 禁用遮罩、关闭射线拦截
        /// </summary>
        protected virtual void DisableFader()
        {
            m_Image.enabled = false;
            m_CanvasGroup.blocksRaycasts = false;
        }

        #endregion

        //=========================================================================
        #region 编辑器测试方法
        //=========================================================================

        /// <summary>
        /// 测试：淡入
        /// </summary>
        protected virtual void TestFadeIn()
        {
            if (!Application.isPlaying) return;
            UIFadeInEvent.FireNow(this, ID);
        }

        /// <summary>
        /// 测试：淡出
        /// </summary>
        protected virtual void TestFadeOut()
        {
            if (!Application.isPlaying) return;
            UIFadeOutEvent.FireNow(this, ID);
        }

        /// <summary>
        /// 测试：停止动画
        /// </summary>
        protected virtual void TestFadeStop()
        {
            if (!Application.isPlaying) return;
            UIFadeStopEvent.FireNow(this, ID);
        }

        /// <summary>
        /// 测试：重置为隐藏状态
        /// </summary>
        protected virtual void TestFadeReset()
        {
            if (!Application.isPlaying) return;
            DOTween.Kill(GameDOTweenTypes.UIFader + GetInstanceID());
            m_CanvasGroup.alpha = InactiveAlpha;
        }

        #endregion
    }
}