/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * -------------------------------------------------------------
 * filename:  TweenActive.cs
 * author:  云毅
 * created:
 * descrip:   激活补间 - 按进度切换 GameObject 激活状态（用于淡入淡出配合）
 * 优化记录: 由旧版 HonorUtils.Tweening.TweenActive 重构迁移
 ***************************************************************/

using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// 激活补间
    /// 功能：播放期间对象保持激活，进度过半/结束切换目标激活状态
    /// 典型用途：配合 TweenAlpha 实现"淡入后显示、淡出前隐藏"
    /// </summary>
    public class TweenActive : UITweener
    {
        #region 字段

        /// <summary>结束时的目标激活状态</summary>
        [Header("结束时的目标激活状态")]
        public bool activeTarget = true;

        /// <summary>进度超过该值即切换状态（默认 0.5）</summary>
        [Header("切换进度阈值")]
        [Range(0f, 1f)]
        public float switchFactor = 0.5f;

        #endregion

        #region 插值

        protected override void ApplyValue(float factor)
        {
            // 正向播放：越过阈值后激活；反向播放：低于阈值后取消激活
            bool shouldActive = playForward ? factor >= switchFactor : factor > switchFactor;
            if (shouldActive != gameObject.activeSelf)
            {
                gameObject.SetActive(shouldActive);
            }
        }

        #endregion

        #region 静态工具

        /// <summary>
        /// 播放一个激活补间并立即开始
        /// </summary>
        /// <param name="target">目标物体</param>
        /// <param name="active">结束时激活状态</param>
        /// <param name="duration">时长（秒）</param>
        /// <returns>补间组件</returns>
        public static TweenActive Begin(GameObject target, bool active, float duration)
        {
            TweenActive tween = Begin<TweenActive>(target, duration);
            tween.activeTarget = active;
            tween.Play();
            return tween;
        }

        #endregion
    }
}
