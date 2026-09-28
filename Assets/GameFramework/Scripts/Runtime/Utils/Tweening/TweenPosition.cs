/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * -------------------------------------------------------------
 * filename:  TweenPosition.cs
 * author:  云毅
 * created:
 * descrip:   位移补间 - 本地坐标 From→To 平滑移动
 * 优化记录: 由旧版 HonorUtils.Tweening.TweenPosition 重构迁移
 ***************************************************************/

using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// 位移补间
    /// 功能：在 From 与 To 两个本地坐标之间插值移动
    /// </summary>
    public class TweenPosition : UITweener
    {
        #region 字段

        /// <summary>起始本地坐标（留空则取当前坐标）</summary>
        [Header("起始本地坐标")]
        public Vector3 fromPosition = Vector3.zero;

        /// <summary>目标本地坐标</summary>
        [Header("目标本地坐标")]
        public Vector3 toPosition = Vector3.zero;

        /// <summary>是否使用世界坐标</summary>
        [Header("是否使用世界坐标")]
        public bool worldSpace = false;

        #endregion

        #region 属性

        /// <summary>当前坐标</summary>
        public Vector3 Position
        {
            get { return worldSpace ? transform.position : transform.localPosition; }
            set
            {
                if (worldSpace)
                {
                    transform.position = value;
                }
                else
                {
                    transform.localPosition = value;
                }
            }
        }

        #endregion

        #region 初始化与插值

        protected override void Init()
        {
            // from 未赋值时以当前位置为起点
            fromPosition = Position;
        }

        protected override void ApplyValue(float factor)
        {
            Position = Vector3.LerpUnclamped(fromPosition, toPosition, factor);
        }

        #endregion

        #region 静态工具

        /// <summary>
        /// 播放一个位移补间并立即开始
        /// </summary>
        /// <param name="target">目标物体</param>
        /// <param name="from">起始坐标（null 表示当前坐标）</param>
        /// <param name="to">目标坐标</param>
        /// <param name="duration">时长（秒）</param>
        /// <returns>补间组件</returns>
        public static TweenPosition Begin(GameObject target, Vector3? from, Vector3 to, float duration)
        {
            TweenPosition tween = Begin<TweenPosition>(target, duration);
            tween.fromPosition = from.HasValue ? from.Value : tween.Position;
            tween.toPosition = to;
            tween.Play();
            return tween;
        }

        #endregion
    }
}
