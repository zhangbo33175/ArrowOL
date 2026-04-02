using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.RectTransform;

namespace Honor.Runtime
{
    public static class UIFadeInEvent
    {
        private static readonly GameEventCmd cmd = GameEventCmd.UIFadeIn;

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

    public static class UIFadeOutEvent
    {
        private static new readonly GameEventCmd cmd = GameEventCmd.UIFadeOut;

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

    public static class UIFadeStopEvent
    {
        private static new readonly GameEventCmd cmd = GameEventCmd.UIFadeStop;

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


    [AddComponentMenu("Honor Core/UI/UIFader")]
    public partial class UIFader : MonoBehaviour
    {
        /// <summary>
        /// 初始化状态
        /// </summary>
        public enum InitState
        { 
            /// <summary>
            /// 无
            /// </summary>
            None = 0,
            /// <summary>
            /// 活跃
            /// </summary>
            Active,
            /// <summary>
            /// 未活跃
            /// </summary>
            Inactive
        }

        [GameHeader("标识")]

        [GameTitle("ID")]
        [Tooltip("唯一标识。")]
        public int ID = 0;

        [GameHeader("色值")]

        [GameTitle("遮罩层颜色")]
        [Tooltip("遮罩层颜色值。")]
        public Color ImageColor = Color.black;

        [GameTitle("未活跃透明度")]
        [Tooltip("未活跃状态下的透明度渐变值。")]
        public float InactiveAlpha = 0f;

        [GameTitle("活跃透明度")]
        [Tooltip("活跃状态下的透明度渐变值。")]
        public float ActiveAlpha = 1f;

        [GameTitle("初始状态")]
        [Tooltip("初始状态。")]
        public InitState InitialState = InitState.Inactive;

        [GameHeader("时间")]

        [GameTitle("渐变时长")]
        [Tooltip("FadeIn 与 FadeOut 的渐变时长。")]
        public float Duration = 0.2f;

        [GameTitle("加速度类型")]
        [Tooltip("渐变过程中的加速度类型")]
        public DG.Tweening.Ease TweenEase = DG.Tweening.Ease.Linear;

        [GameHeader("交互")]

        [GameTitle("阻塞射线")]
        [Tooltip("当可见时是否阻塞射线。")]
        public bool ShouldBlockRaycasts = false;

        [GameInspectorButton("TestFadeIn")]
        public bool TestFadeInButton;
        [GameInspectorButton("TestFadeOut")]
        public bool TestFadeOutButton;
        [GameInspectorButton("TestFadeStop")]
        public bool TestFaderStopButton;
        [GameInspectorButton("TestFadeReset")]
        public bool TestFaderResetButton;

        /// <summary>
        /// 必须组件-CanvasGroup
        /// </summary>
        protected CanvasGroup m_CanvasGroup;

        /// <summary>
        /// 必须组件-Image
        /// </summary>
        protected Image m_Image;

        /// <summary>
        /// 初始透明度渐变值
        /// </summary>
        protected float m_InitialAlpha;

        /// <summary>
        /// 当前目标透明度渐变值
        /// </summary>
        protected float m_CurrentTargetAlpha;

        /// <summary>
        /// 当前渐变时长
        /// </summary>
        protected float m_CurrentDuration;

        /// <summary>
        /// 当前加速度类型
        /// </summary>
        protected DG.Tweening.Ease m_CurrentTweenEase;

        /// <summary>
        /// 是否正在渐变中
        /// </summary>
        protected bool m_IsFading = false;

        protected virtual void OnEnable()
        {
            UIFadeStopEvent.Subscribe(this, EventCallback);
            UIFadeInEvent.Subscribe(this, EventCallback);
            UIFadeOutEvent.Subscribe(this, EventCallback);
        }

        protected virtual void OnDisable()
        {
            UIFadeStopEvent.Unsubscribe(this, EventCallback);
            UIFadeInEvent.Unsubscribe(this, EventCallback);
            UIFadeOutEvent.Unsubscribe(this, EventCallback);
        }

        protected virtual void Awake()
        {
            m_CanvasGroup = gameObject.GetOrAddComponent<CanvasGroup>();
            m_Image = gameObject.GetOrAddComponent<Image>();
            m_Image.rectTransform().SetSizeWithCurrentAnchors(Axis.Horizontal, Screen.width);
            m_Image.rectTransform().SetSizeWithCurrentAnchors(Axis.Vertical, Screen.height);
            m_Image.color = ImageColor;

            if (InitialState == InitState.Inactive)
            {
                m_CanvasGroup.alpha = InactiveAlpha;
                m_Image.enabled = false;
            }
            else if (InitialState == InitState.Active)
            {
                m_CanvasGroup.alpha = ActiveAlpha;
                m_Image.enabled = true;
            }
        }

        protected virtual void Start() { }

        /// <summary>
        /// 事件监听回调
        /// </summary>
        /// <param name="sender">派发者</param>
        /// <param name="userData">用户数据</param>
        /// <param name="e">事件参数</param>
        protected virtual void EventCallback(object sender, object userData, EventParams e)
        {
            int id = e.GetInt("ID");
            if(id == ID)
            {
                switch (e.Cmd)
                {
                    case GameEventCmd.UIFadeStop: StopFading(); break;
                    case GameEventCmd.UIFadeIn: FadeIn(); break;
                    case GameEventCmd.UIFadeOut: FadeOut(); break;
                }
            }
        }

        /// <summary>
        /// 渐变到活跃状态
        /// </summary>
        protected virtual void FadeIn()
        {
            StartFading(m_CanvasGroup.alpha, ActiveAlpha, Duration, TweenEase);
        }

        /// <summary>
        /// 渐变到非活跃状态
        /// </summary>
        protected virtual void FadeOut()
        {
            StartFading(m_CanvasGroup.alpha, InactiveAlpha, Duration, TweenEase);
        }

        /// <summary>
        /// 停止渐变
        /// </summary>
        protected virtual void StopFading()
        {
            DOTween.Kill(GameDOTweenTypes.UIFader + GetInstanceID());
            m_CanvasGroup.alpha = m_CurrentTargetAlpha;
            m_IsFading = false;
            if (m_CanvasGroup.alpha == InactiveAlpha)
            {
                DisableFader();
            }
        }

        /// <summary>
        /// 开始渐变
        /// </summary>
        /// <param name="initialAlpha">初始透明度渐变值</param>
        /// <param name="endAlpha">结束透明度渐变值</param>
        /// <param name="duration">间隔时长</param>
        /// <param name="tweenEase">加速度类型</param>
        protected virtual void StartFading(float initialAlpha, float endAlpha, float duration, DG.Tweening.Ease tweenEase)
        {
            EnableFader();
            m_IsFading = true;
            m_InitialAlpha = initialAlpha;
            m_CurrentTargetAlpha = endAlpha;
            m_CurrentTweenEase = tweenEase;
            m_CurrentDuration = duration;

            m_CanvasGroup.alpha = m_InitialAlpha;
            DOTween.Kill(GameDOTweenTypes.UIFader + GetInstanceID());
            DOTween.To(()=>m_CanvasGroup.alpha, alpha => m_CanvasGroup.alpha = alpha, m_CurrentTargetAlpha, m_CurrentDuration).SetEase(m_CurrentTweenEase).OnComplete(()=> {
                StopFading();
            }).id = GameDOTweenTypes.UIFader + GetInstanceID();
        }

        /// <summary>
        /// 启用渐变-Enable
        /// </summary>
        protected virtual void EnableFader()
        {
            m_Image.enabled = true;
            if (ShouldBlockRaycasts)
            {
                m_CanvasGroup.blocksRaycasts = true;
            }
        }

        /// <summary>
        /// 禁用渐变-Disable
        /// </summary>
        protected virtual void DisableFader()
        {
            m_Image.enabled = false;
            if (ShouldBlockRaycasts)
            {
                m_CanvasGroup.blocksRaycasts = false;
            }
        }

        /// <summary>
        /// 测试方法：渐变进入
        /// </summary>
        protected virtual void TestFadeIn()
        {
            if (!Application.isPlaying) return;
            UIFadeInEvent.FireNow(this, ID);
        }

        /// <summary>
        /// 测试方法：渐变退出
        /// </summary>
        protected virtual void TestFadeOut()
        {
            if (!Application.isPlaying) return;
            UIFadeOutEvent.FireNow(this, ID);
        }

        /// <summary>
        /// 测试方法：渐变停止
        /// </summary>
        protected virtual void TestFadeStop()
        {
            if (!Application.isPlaying) return;
            UIFadeStopEvent.FireNow(this, ID);
        }

        /// <summary>
        /// 测试方法：复位渐变器
        /// </summary>
        protected virtual void TestFadeReset()
        {
            if (!Application.isPlaying) return;
            DOTween.Kill(GameDOTweenTypes.UIFader + GetInstanceID());
            m_CanvasGroup.alpha = InactiveAlpha;
        }

    }

}


