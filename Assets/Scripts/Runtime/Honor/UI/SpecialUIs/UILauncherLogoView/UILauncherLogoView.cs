/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  UILauncherLogoView.cs
 * author:    云毅
 * created:   2026
 * descrip:   启动器Logo视图抽象基类
 *            提供Logo界面的基础组件、生命周期、遮罩和事件触发封装
 ***************************************************************/

using UnityEngine;
using UnityEngine.UI;

namespace Honor.Runtime
{
    /// <summary>
    /// 启动器Logo视图抽象基类
    /// 提供Logo界面的基础组件、生命周期、遮罩和事件触发封装
    /// </summary>
    public abstract class UILauncherLogoView : MonoBehaviour
    {
        #region 序列化字段
        //=========================================================================
        // 序列化字段
        //=========================================================================
        /// <summary>
        /// 背景图（用于淡入淡出动画）
        /// </summary>
        [SerializeField] protected Image m_BgImage;

        /// <summary>
        /// 底部遮罩层
        /// </summary>
        [SerializeField] protected Image m_BottomMaskLayer;

        /// <summary>
        /// 顶部遮罩层（用于阻挡点击）
        /// </summary>
        [SerializeField] protected Image m_TopMaskLayer;

        /// <summary>
        /// 闪屏内容图（显示 SplashIcon 等启动画面，可在 Inspector 指定）
        /// </summary>
        [SerializeField] protected Image m_SplashImage;

        /// <summary>
        /// 闪屏 Sprite（Inspector 指定，或通过 <see cref="SetSplashSprite"/> 动态设置）
        /// </summary>
        [SerializeField] protected Sprite m_SplashSprite;

        /// <summary>
        /// 进入动画期间是否阻塞射线（点击）
        /// </summary>
        [SerializeField] protected bool m_BlockRaycastOnEntering = false;

        /// <summary>
        /// 退出动画期间是否阻塞射线（点击）
        /// </summary>
        [SerializeField] protected bool m_BlockRaycastOnExiting = true;

        /// <summary>
        /// 进入动画时长
        /// </summary>
        [SerializeField] protected float m_EnterDuration;

        /// <summary>
        /// 退出动画时长
        /// </summary>
        [SerializeField] protected float m_ExitDuration;
        #endregion

        #region 闪屏时序配置（Inspector 可调）
        //=========================================================================
        // 闪屏时序配置（可在 Inspector 逐项调整，默认：黑屏0 / 淡入0.5 / 停留1.5 / 淡出0.6）
        // 时序：闪屏淡入 → 停留 → 淡出；无黑屏保持段，启动即平滑淡入闪屏
        //=========================================================================

        /// <summary>
        /// 黑屏保持时长（秒）
        /// </summary>
        [Header("闪屏时序（秒）")]
        [SerializeField]
        [Tooltip("黑屏保持时长（秒），设为0去掉黑屏，启动立即淡入闪屏")]
        private float m_SplashBlackHoldDuration = 0f;

        /// <summary>
        /// 闪屏淡入时长（秒）
        /// </summary>
        [SerializeField]
        [Tooltip("闪屏淡入时长（秒）")]
        private float m_SplashFadeInDuration = 0.5f;

        /// <summary>
        /// 闪屏完全显示停留时长（秒）
        /// </summary>
        [SerializeField]
        [Tooltip("闪屏完全显示停留时长（秒）")]
        private float m_SplashStayDuration = 1.5f;

        /// <summary>
        /// 闪屏+背景淡出时长（秒）
        /// </summary>
        [SerializeField]
        [Tooltip("闪屏+背景淡出时长（秒）")]
        private float m_SplashFadeOutDuration = 0.6f;

        /// <summary>
        /// 黑屏保持时长（秒）
        /// </summary>
        public float SplashBlackHoldDuration => m_SplashBlackHoldDuration;

        /// <summary>
        /// 闪屏淡入时长（秒）
        /// </summary>
        public float SplashFadeInDuration => m_SplashFadeInDuration;

        /// <summary>
        /// 闪屏完全显示停留时长（秒）
        /// </summary>
        public float SplashStayDuration => m_SplashStayDuration;

        /// <summary>
        /// 闪屏+背景淡出时长（秒）
        /// </summary>
        public float SplashFadeOutDuration => m_SplashFadeOutDuration;

        /// <summary>
        /// 闪屏显示总时长（淡入+停留）
        /// </summary>
        public float SplashShowDuration => m_SplashFadeInDuration + m_SplashStayDuration;

        /// <summary>
        /// 闪屏淡出起始时刻（黑屏+淡入+停留），Loading 在此刻出现以实现丝滑衔接
        /// </summary>
        public float SplashFadeOutStart => m_SplashBlackHoldDuration + m_SplashFadeInDuration + m_SplashStayDuration;

        /// <summary>
        /// 闪屏完整序列总时长（黑屏+淡入+停留+淡出）
        /// </summary>
        public float SplashTotalDuration => SplashFadeOutStart + m_SplashFadeOutDuration;
        #endregion

        #region 公共属性
        //=========================================================================
        // 公共属性
        //=========================================================================
        /// <summary>
        /// 进入动画期间是否阻塞射线（点击）
        /// </summary>
        public bool BlockRaycastOnEntering
        {
            set => m_BlockRaycastOnEntering = value;
            get => m_BlockRaycastOnEntering;
        }

        /// <summary>
        /// 退出动画期间是否阻塞射线（点击）
        /// </summary>
        public bool BlockRaycastOnExiting
        {
            set => m_BlockRaycastOnExiting = value;
            get => m_BlockRaycastOnExiting;
        }

        /// <summary>
        /// 进入动画时长
        /// </summary>
        public float EnterDuration
        {
            set => m_EnterDuration = value;
            get => m_EnterDuration;
        }

        /// <summary>
        /// 退出动画时长
        /// </summary>
        public float ExitDuration
        {
            set => m_ExitDuration = value;
            get => m_ExitDuration;
        }
        #endregion

        #region 生命周期方法
        //=========================================================================
        // 生命周期方法
        //=========================================================================
        /// <summary>
        /// 进入切换（显示界面 + 按需开启遮罩 + 显示闪屏）
        /// 进入阶段若无需拦截点击，则隐藏顶层遮罩，避免其遮挡闪屏
        /// </summary>
        public virtual void Enter()
        {
            gameObject.SetActive(true);
            m_TopMaskLayer.raycastTarget = m_BlockRaycastOnEntering;
            m_TopMaskLayer.gameObject.SetActive(m_BlockRaycastOnEntering);
            ShowSplashContent(true);
        }

        /// <summary>
        /// 进入动画结束
        /// 关闭遮罩、隐藏闪屏与物体、派发进入完成事件
        /// </summary>
        public virtual void EnterOver()
        {
            m_TopMaskLayer.raycastTarget = false;
            ShowSplashContent(false);
            gameObject.SetActive(false);
            GameMainRoot.Event.FireNow(this, GameEventCmd.ProcedureTransitionEnterOver);
        }

        /// <summary>
        /// 退出切换（显示界面 + 按需开启遮罩；退出为黑场过渡，不显示启动闪屏）
        /// 退出阶段需拦截点击时激活顶层遮罩
        /// </summary>
        public virtual void Exit()
        {
            gameObject.SetActive(true);
            m_TopMaskLayer.raycastTarget = m_BlockRaycastOnExiting;
            m_TopMaskLayer.gameObject.SetActive(m_BlockRaycastOnExiting);
            ShowSplashContent(false);
        }

        /// <summary>
        /// 退出动画结束
        /// 关闭遮罩、隐藏闪屏、派发退出完成事件
        /// </summary>
        public virtual void ExitOver()
        {
            gameObject.SetActive(true);
            m_TopMaskLayer.raycastTarget = false;
            m_TopMaskLayer.gameObject.SetActive(false);
            ShowSplashContent(false);
            GameMainRoot.Event.FireNow(this, GameEventCmd.ProcedureTransitionExitOver);
        }
        #endregion

        #region 闪屏内容
        //=========================================================================
        // 闪屏内容（SplashIcon 等启动画面）
        //=========================================================================

        /// <summary>
        /// 设置闪屏 Sprite 并应用（供 Inspector 指定之外，运行时也可动态设置）
        /// 内部方法：避免 XLua 生成 Sprite 参数包装引发重新生成问题；如需 Lua 动态设置请另行登记
        /// </summary>
        /// <param name="sprite">闪屏 Sprite（为空则不修改）</param>
        internal void SetSplashSprite(Sprite sprite)
        {
            if (sprite == null) return;
            m_SplashSprite = sprite;
            if (m_SplashImage != null)
            {
                m_SplashImage.sprite = sprite;
            }
        }

        /// <summary>
        /// 显示/隐藏闪屏内容图
        /// 仅在配置了闪屏图时才生效；显示时应用当前 Sprite
        /// </summary>
        /// <param name="show">true=显示，false=隐藏</param>
        private void ShowSplashContent(bool show)
        {
            if (m_SplashImage == null) return;

            if (show)
            {
                // 应用 Sprite（Inspector 指定或运行时 SetSplashSprite 设置）
                if (m_SplashSprite != null && m_SplashImage.sprite != m_SplashSprite)
                {
                    m_SplashImage.sprite = m_SplashSprite;
                }
                m_SplashImage.gameObject.SetActive(true);
            }
            else
            {
                m_SplashImage.gameObject.SetActive(false);
            }
        }
        #endregion
    }
}