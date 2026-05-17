/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  CameraOutlineAnimation.cs
 * author:    云毅
 * created:   2026
 * descrip:   相机描边透明度呼吸动画，实现描边线条 0~1 循环渐变闪烁效果
 ***************************************************************/

using UnityEngine;

namespace Honor.Runtime
{
    //=========================================================================
    // 相机描边透明度动画
    //=========================================================================
    /// <summary>
    /// 相机描边透明度动画
    /// 功能：让描边线条的透明度进行 0~1 循环渐变（呼吸/闪烁效果）
    /// </summary>
    public class CameraOutlineAnimation : MonoBehaviour
    {
        #region 配置字段
        [Header("是否开启透明度循环动画")]
        public bool playAnimation = true;

        [Header("透明度渐变速度")]
        public float fadeSpeed = 1f;
        #endregion

        #region 私有成员
        /// <summary>
        /// 透明度往返标志
        /// </summary>
        private bool m_PingPong;

        /// <summary>
        /// 缓存描边组件，避免每帧 GetComponent 造成性能消耗
        /// </summary>
        private CameraOutlineBuffer m_CameraOutlineBuffer;
        #endregion

        #region 生命周期
        /// <summary>
        /// 初始化：缓存组件
        /// </summary>
        private void Awake()
        {
            // 获取并缓存组件
            m_CameraOutlineBuffer = GetComponent<CameraOutlineBuffer>();
        }

        /// <summary>
        /// 每帧更新透明度动画
        /// </summary>
        private void Update()
        {
            // 组件为空 或 未开启动画时直接退出
            if (m_CameraOutlineBuffer == null || !playAnimation)
                return;

            // 获取当前颜色
            Color color = m_CameraOutlineBuffer.LineColor0;

            // 透明度往返渐变
            if (m_PingPong)
            {
                color.a += Time.deltaTime * fadeSpeed;
                if (color.a >= 1f)
                {
                    m_PingPong = false;
                }
            }
            else
            {
                color.a -= Time.deltaTime * fadeSpeed;
                if (color.a <= 0f)
                {
                    m_PingPong = true;
                }
            }

            // 限制透明度在 0~1 之间
            color.a = Mathf.Clamp01(color.a);

            // 赋值并更新材质
            m_CameraOutlineBuffer.LineColor0 = color;
            m_CameraOutlineBuffer.UpdateMaterialsPublicProperties();
        }
        #endregion
    }
}