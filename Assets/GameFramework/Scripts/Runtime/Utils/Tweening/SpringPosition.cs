/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * -------------------------------------------------------------
 * filename:  SpringPosition.cs
 * author:  云毅
 * created:
 * descrip:   弹性位移 - 带阻尼弹簧效果的平滑归位（接近目标时自然减速）
 * 优化记录: 由旧版 HonorUtils.Tweening.SpringPosition 重构迁移
 ***************************************************************/

using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// 弹性位移
    /// 功能：以弹簧阻尼模型驱动对象平滑移动到目标位置，适合"回到原位/吸附"类表现
    /// </summary>
    public class SpringPosition : MonoBehaviour
    {
        #region 字段

        /// <summary>目标位置（本地坐标）</summary>
        [Header("目标本地坐标")]
        public Vector3 target = Vector3.zero;

        /// <summary>弹簧强度（越大越快）</summary>
        [Header("弹簧强度")]
        public float strength = 10f;

        /// <summary>停止阈值（与目标距离小于该值即停止）</summary>
        [Header("停止阈值")]
        public float threshold = 0.001f;

        /// <summary>是否使用世界坐标</summary>
        [Header("是否使用世界坐标")]
        public bool worldSpace = false;

        /// <summary>完成回调</summary>
        [HideInInspector]
        public EventDelegate onFinished = new EventDelegate();

        /// <summary>是否播放中</summary>
        private bool m_Playing = false;

        #endregion

        #region 生命周期

        private void Update()
        {
            if (!m_Playing)
            {
                return;
            }

            // 弹簧逼近：每帧向目标移动剩余距离的一部分
            Vector3 current = Position;
            Vector3 delta = target - current;
            if (delta.sqrMagnitude < threshold * threshold)
            {
                Position = target;
                m_Playing = false;
                if (onFinished != null)
                {
                    onFinished.Execute();
                }

                return;
            }

            Position = current + delta * Mathf.Clamp01(strength * RealTime.deltaTime);
        }

        #endregion

        #region 属性与方法

        /// <summary>当前坐标</summary>
        private Vector3 Position
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

        /// <summary>
        /// 开始弹性移动
        /// </summary>
        public void Play()
        {
            m_Playing = true;
        }

        /// <summary>
        /// 停止弹性移动
        /// </summary>
        public void Stop()
        {
            m_Playing = false;
        }

        #endregion

        #region 静态工具

        /// <summary>
        /// 对目标执行一次弹性位移并开始播放
        /// </summary>
        /// <param name="target">目标物体</param>
        /// <param name="to">目标坐标</param>
        /// <param name="strength">弹簧强度</param>
        /// <returns>弹性组件</returns>
        public static SpringPosition Begin(GameObject target, Vector3 to, float strength)
        {
            SpringPosition spring = target.GetComponent<SpringPosition>();
            if (spring == null)
            {
                spring = target.AddComponent<SpringPosition>();
            }

            spring.target = to;
            spring.strength = strength;
            spring.onFinished.Clear();
            spring.Play();
            return spring;
        }

        #endregion
    }
}
