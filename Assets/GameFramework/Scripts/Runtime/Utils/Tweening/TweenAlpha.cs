/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * -------------------------------------------------------------
 * filename:  TweenAlpha.cs
 * author:  云毅
 * created:
 * descrip:   透明度补间 - 支持 CanvasGroup 与 Graphic 的 From→To 透明度插值
 * 优化记录: 由旧版 HonorUtils.Tweening.TweenAlpha 重构迁移，合并 CanvasGroup/UGUI 双通道
 ***************************************************************/

using UnityEngine;
using UnityEngine.UI;

namespace Honor.Runtime
{
    /// <summary>
    /// 透明度补间
    /// 功能：优先插值 CanvasGroup.alpha（整体透明度），否则插值 Graphic.color 的 Alpha 通道
    /// </summary>
    public class TweenAlpha : UITweener
    {
        #region 字段

        /// <summary>起始透明度</summary>
        [Header("起始透明度")]
        public float fromAlpha = 0f;

        /// <summary>目标透明度</summary>
        [Header("目标透明度")]
        public float toAlpha = 1f;

        /// <summary>是否强制使用 Graphic 颜色通道</summary>
        [Header("强制使用 Graphic 颜色通道")]
        public bool forceGraphic = false;

        #endregion

        #region 属性

        /// <summary>当前透明度</summary>
        public float Alpha
        {
            get
            {
                if (!forceGraphic)
                {
                    CanvasGroup group = GetComponent<CanvasGroup>();
                    if (group != null)
                    {
                        return group.alpha;
                    }
                }

                Graphic graphic = GetComponent<Graphic>();
                return graphic != null ? graphic.color.a : 1f;
            }
            set
            {
                if (!forceGraphic)
                {
                    CanvasGroup group = GetComponent<CanvasGroup>();
                    if (group != null)
                    {
                        group.alpha = value;
                        return;
                    }
                }

                Graphic graphic = GetComponent<Graphic>();
                if (graphic != null)
                {
                    Color color = graphic.color;
                    color.a = value;
                    graphic.color = color;
                }
            }
        }

        #endregion

        #region 初始化与插值

        /// <summary>
        /// 初始化：以当前透明度作为起始值
        /// </summary>
        protected override void Init()
        {
            fromAlpha = Alpha;
        }

        /// <summary>
        /// 按进度插值设置透明度
        /// </summary>
        /// <param name="factor">曲线修正后的 0~1 进度</param>
        protected override void ApplyValue(float factor)
        {
            Alpha = Mathf.LerpUnclamped(fromAlpha, toAlpha, factor);
        }

        #endregion

        #region 静态工具

        /// <summary>
        /// 播放一个透明度补间并立即开始
        /// </summary>
        /// <param name="target">目标物体</param>
        /// <param name="from">起始透明度（-1 表示当前值）</param>
        /// <param name="to">目标透明度</param>
        /// <param name="duration">时长（秒）</param>
        /// <returns>补间组件</returns>
        public static TweenAlpha Begin(GameObject target, float from, float to, float duration)
        {
            TweenAlpha tween = Begin<TweenAlpha>(target, duration);
            tween.fromAlpha = from < 0f ? tween.Alpha : from;
            tween.toAlpha = to;
            tween.Play();
            return tween;
        }

        #endregion
    }
}
