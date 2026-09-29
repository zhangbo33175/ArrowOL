/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * -------------------------------------------------------------
 * filename:  TweenRotation.cs
 * author:  云毅
 * created:
 * descrip:   旋转补间 - 本地欧拉角 From→To 平滑旋转
 * 优化记录: 由旧版 HonorUtils.Tweening.TweenRotation 重构迁移
 ***************************************************************/

using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// 旋转补间
    /// 功能：在 From 与 To 两个本地欧拉角之间插值
    /// </summary>
    public class TweenRotation : UITweener
    {
        #region 字段

        /// <summary>起始本地欧拉角（留空则取当前角度）</summary>
        [Header("起始本地欧拉角")]
        public Vector3 fromRotation = Vector3.zero;

        /// <summary>目标本地欧拉角</summary>
        [Header("目标本地欧拉角")]
        public Vector3 toRotation = Vector3.zero;

        #endregion

        #region 初始化与插值

        /// <summary>
        /// 初始化：以当前本地欧拉角作为起始角度
        /// </summary>
        protected override void Init()
        {
            fromRotation = transform.localEulerAngles;
        }

        /// <summary>
        /// 按进度插值设置本地欧拉角
        /// </summary>
        /// <param name="factor">曲线修正后的 0~1 进度</param>
        protected override void ApplyValue(float factor)
        {
            transform.localEulerAngles = Vector3.LerpUnclamped(fromRotation, toRotation, factor);
        }

        #endregion

        #region 静态工具

        /// <summary>
        /// 播放一个旋转补间并立即开始
        /// </summary>
        /// <param name="target">目标物体</param>
        /// <param name="from">起始角度（null 表示当前角度）</param>
        /// <param name="to">目标角度</param>
        /// <param name="duration">时长（秒）</param>
        /// <returns>补间组件</returns>
        public static TweenRotation Begin(GameObject target, Vector3? from, Vector3 to, float duration)
        {
            TweenRotation tween = Begin<TweenRotation>(target, duration);
            tween.fromRotation = from.HasValue ? from.Value : tween.transform.localEulerAngles;
            tween.toRotation = to;
            tween.Play();
            return tween;
        }

        #endregion
    }
}
