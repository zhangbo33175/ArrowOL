/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * -------------------------------------------------------------
 * filename:  TweenColor.cs
 * author:  云毅
 * created:
 * descrip:   颜色补间 - Graphic/Renderer 颜色 From→To 平滑渐变
 * 优化记录: 由旧版 HonorUtils.Tweening.TweenColor 重构迁移
 ***************************************************************/

using UnityEngine;
using UnityEngine.UI;

namespace Honor.Runtime
{
    /// <summary>
    /// 颜色补间
    /// 功能：插值 UGUI Graphic 或 Renderer.material 主颜色
    /// </summary>
    public class TweenColor : UITweener
    {
        #region 字段

        /// <summary>起始颜色（留空则取当前颜色）</summary>
        [Header("起始颜色")]
        public Color fromColor = Color.white;

        /// <summary>目标颜色</summary>
        [Header("目标颜色")]
        public Color toColor = Color.white;

        /// <summary>是否强制走 Renderer 材质</summary>
        [Header("强制使用 Renderer 材质")]
        public bool forceRenderer = false;

        #endregion

        #region 属性

        /// <summary>当前颜色</summary>
        public Color Color
        {
            get
            {
                if (!forceRenderer)
                {
                    Graphic graphic = GetComponent<Graphic>();
                    if (graphic != null)
                    {
                        return graphic.color;
                    }
                }

                Renderer renderer = GetComponent<Renderer>();
                if (renderer != null && renderer.material != null)
                {
                    return renderer.material.color;
                }

                return Color.white;
            }
            set
            {
                if (!forceRenderer)
                {
                    Graphic graphic = GetComponent<Graphic>();
                    if (graphic != null)
                    {
                        graphic.color = value;
                        return;
                    }
                }

                Renderer renderer = GetComponent<Renderer>();
                if (renderer != null && renderer.material != null)
                {
                    renderer.material.color = value;
                }
            }
        }

        #endregion

        #region 初始化与插值

        /// <summary>
        /// 初始化：以当前颜色作为起始颜色
        /// </summary>
        protected override void Init()
        {
            fromColor = Color;
        }

        /// <summary>
        /// 按进度插值设置颜色
        /// </summary>
        /// <param name="factor">曲线修正后的 0~1 进度</param>
        protected override void ApplyValue(float factor)
        {
            Color = UnityEngine.Color.LerpUnclamped(fromColor, toColor, factor);
        }

        #endregion

        #region 静态工具

        /// <summary>
        /// 播放一个颜色补间并立即开始
        /// </summary>
        /// <param name="target">目标物体</param>
        /// <param name="from">起始颜色（null 表示当前颜色）</param>
        /// <param name="to">目标颜色</param>
        /// <param name="duration">时长（秒）</param>
        /// <returns>补间组件</returns>
        public static TweenColor Begin(GameObject target, Color? from, Color to, float duration)
        {
            TweenColor tween = Begin<TweenColor>(target, duration);
            tween.fromColor = from.HasValue ? from.Value : tween.Color;
            tween.toColor = to;
            tween.Play();
            return tween;
        }

        #endregion
    }
}
