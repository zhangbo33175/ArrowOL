/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * -------------------------------------------------------------
 * filename:  TweenScale.cs
 * author:  云毅
 * created:
 * descrip:   缩放补间 - 本地缩放 From→To 平滑缩放
 * 优化记录: 由旧版 HonorUtils.Tweening.TweenScale 重构迁移
 ***************************************************************/

using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// 缩放补间
    /// 功能：在 From 与 To 两个本地缩放之间插值
    /// </summary>
    public class TweenScale : UITweener
    {
        #region 字段

        /// <summary>起始本地缩放（留空则取当前缩放）</summary>
        [Header("起始本地缩放")]
        public Vector3 fromScale = Vector3.one;

        /// <summary>目标本地缩放</summary>
        [Header("目标本地缩放")]
        public Vector3 toScale = Vector3.one;

        #endregion

        #region 初始化与插值

        /// <summary>
        /// 初始化：以当前本地缩放作为起始缩放
        /// </summary>
        protected override void Init()
        {
            fromScale = transform.localScale;
        }

        /// <summary>
        /// 按进度插值设置本地缩放
        /// </summary>
        /// <param name="factor">曲线修正后的 0~1 进度</param>
        protected override void ApplyValue(float factor)
        {
            transform.localScale = Vector3.LerpUnclamped(fromScale, toScale, factor);
        }

        #endregion

        #region 静态工具

        /// <summary>
        /// 播放一个缩放补间并立即开始
        /// </summary>
        /// <param name="target">目标物体</param>
        /// <param name="from">起始缩放（null 表示当前缩放）</param>
        /// <param name="to">目标缩放</param>
        /// <param name="duration">时长（秒）</param>
        /// <returns>补间组件</returns>
        public static TweenScale Begin(GameObject target, Vector3? from, Vector3 to, float duration)
        {
            TweenScale tween = Begin<TweenScale>(target, duration);
            tween.fromScale = from.HasValue ? from.Value : tween.transform.localScale;
            tween.toScale = to;
            tween.Play();
            return tween;
        }

        #endregion
    }
}
