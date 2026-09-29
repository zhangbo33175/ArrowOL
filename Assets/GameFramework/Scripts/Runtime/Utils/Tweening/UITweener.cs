/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * -------------------------------------------------------------
 * filename:  UITweener.cs
 * author:  云毅
 * created:
 * descrip:   UI 补间基类 - 不依赖 DOTween 的轻量补间系统（From/To/播放/暂停/回调）
 * 优化记录: 由旧版 HonorUtils.Tweening.UITweener 重构迁移，
 *           去掉 NGUI 残留依赖，改为纯 Unity + Action 回调，支持循环与反向
 ***************************************************************/

using System;
using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// UI 补间基类（轻量补间系统）
    /// 功能：驱动子类实现 From→To 的插值动画，支持延迟、循环、反向、完成回调
    /// 使用：挂载到 UI 节点或代码调用 TweenXxx.Begin(...)，播放模式自动开始
    /// </summary>
    public abstract class UITweener : MonoBehaviour
    {
        #region 字段

        /// <summary>起始值（是否使用取决于子类）</summary>
        [HideInInspector]
        public object from;

        /// <summary>目标值</summary>
        [HideInInspector]
        public object to;

        /// <summary>动画时长（秒）</summary>
        [Header("动画时长（秒）")]
        public float duration = 1f;

        /// <summary>延迟启动（秒）</summary>
        [Header("延迟启动（秒）")]
        public float delay = 0f;

        /// <summary>播放模式</summary>
        [Header("播放模式")]
        public AnimationCurve animationCurve = new AnimationCurve(new Keyframe(0f, 0f, 0f, 1f), new Keyframe(1f, 1f, 1f, 0f));

        /// <summary>是否启用循环（往返）</summary>
        [Header("是否往返循环")]
        public bool loop = false;

        /// <summary>是否自动播放（OnEnable 时）</summary>
        [Header("激活时自动播放")]
        public bool playOnEnable = true;

        /// <summary>播放方向：false=正向，true=反向</summary>
        [HideInInspector]
        public bool playForward = true;

        /// <summary>完成回调</summary>
        [HideInInspector]
        public EventDelegate onFinished = new EventDelegate();

        /// <summary>当前时间进度（0~1）</summary>
        [HideInInspector]
        public float t = 0f;

        /// <summary>内部状态</summary>
        private bool m_Started = false;
        private bool m_Playing = false;
        private float m_DelayTimer = 0f;
        private float m_LastTime = 0f;

        /// <summary>是否补间中</summary>
        public bool IsPlaying
        {
            get { return m_Playing; }
        }

        #endregion

        #region 生命周期

        /// <summary>
        /// 组件激活时回调：重置真实时间基准，必要时自动播放
        /// </summary>
        protected virtual void OnEnable()
        {
            // 每次激活都使用真实时间基准（避免 timeScale 影响）
            m_LastTime = RealTime.time;

            if (playOnEnable && !m_Started)
            {
                Play();
            }
        }

        /// <summary>
        /// 组件失活时回调：停止播放
        /// </summary>
        protected virtual void OnDisable()
        {
            m_Playing = false;
        }

        /// <summary>
        /// 每帧更新：推进进度、应用插值并处理完成/循环逻辑
        /// </summary>
        protected virtual void Update()
        {
            float delta = RealTime.deltaTime;
            m_LastTime = RealTime.time;

            if (!m_Playing)
            {
                return;
            }

            // 延迟阶段
            if (m_DelayTimer > 0f)
            {
                m_DelayTimer -= delta;
                return;
            }

            // 推进进度
            float time = delta / Mathf.Max(duration, 0.001f);
            t = playForward ? Mathf.Clamp01(t + time) : Mathf.Clamp01(t - time);

            // 应用插值
            float factor = animationCurve != null ? animationCurve.Evaluate(t) : t;
            ApplyValue(factor);

            // 完成处理
            if ((playForward && t >= 1f) || (!playForward && t <= 0f))
            {
                if (loop)
                {
                    // 往返循环：反向并继续
                    playForward = !playForward;
                    t = playForward ? 0f : 1f;
                }
                else
                {
                    m_Playing = false;
                    Sample(t, true);
                    if (onFinished != null)
                    {
                        onFinished.Execute();
                    }
                }
            }
        }

        #endregion

        #region 播放控制

        /// <summary>
        /// 开始播放（正向）
        /// </summary>
        public void Play()
        {
            Play(true);
        }

        /// <summary>
        /// 按方向播放
        /// </summary>
        public void Play(bool forward)
        {
            if (!m_Started)
            {
                m_Started = true;
                Init();
            }

            playForward = forward;
            m_Playing = true;
            m_DelayTimer = delay;
            m_LastTime = RealTime.time;
        }

        /// <summary>
        /// 暂停
        /// </summary>
        public void Pause()
        {
            m_Playing = false;
        }

        /// <summary>
        /// 恢复播放
        /// </summary>
        public void Resume()
        {
            m_Playing = true;
            m_LastTime = RealTime.time;
        }

        /// <summary>
        /// 重置到起始状态（不自动播放）
        /// </summary>
        public void ResetToBeginning()
        {
            if (!m_Started)
            {
                m_Started = true;
                Init();
            }

            t = playForward ? 0f : 1f;
            Sample(t, true);
        }

        /// <summary>
        /// 停止并保持当前值
        /// </summary>
        public void Stop()
        {
            m_Playing = false;
        }

        /// <summary>
        /// 跳到指定进度
        /// </summary>
        public void Sample(float factor, bool isFinished)
        {
            t = Mathf.Clamp01(factor);
            ApplyValue(animationCurve != null ? animationCurve.Evaluate(t) : t);
        }

        #endregion

        #region 子类实现

        /// <summary>
        /// 首次播放前初始化（解析 from/to 实际值）
        /// </summary>
        protected virtual void Init()
        {
        }

        /// <summary>
        /// 应用当前进度对应的插值
        /// </summary>
        /// <param name="factor">曲线修正后的 0~1 进度</param>
        protected abstract void ApplyValue(float factor);

        #endregion

        #region 静态工具

        /// <summary>
        /// 获取或创建补间组件并开始播放
        /// </summary>
        /// <typeparam name="T">补间类型</typeparam>
        /// <param name="target">目标物体</param>
        /// <param name="duration">时长</param>
        /// <returns>补间组件</returns>
        public static T Begin<T>(GameObject target, float duration) where T : UITweener
        {
            T tween = target.GetComponent<T>();
            if (tween == null)
            {
                tween = target.AddComponent<T>();
            }

            tween.duration = duration;
            tween.playOnEnable = false;
            tween.onFinished.Clear();
            return tween;
        }

        #endregion
    }
}
