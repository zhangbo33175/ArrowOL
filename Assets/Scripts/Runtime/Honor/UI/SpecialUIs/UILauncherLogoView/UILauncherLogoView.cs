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

        #region 公共属性
        //=========================================================================
        // 公共属性
        //=========================================================================
        public bool BlockRaycastOnEntering
        {
            set => m_BlockRaycastOnEntering = value;
            get => m_BlockRaycastOnEntering;
        }

        public bool BlockRaycastOnExiting
        {
            set => m_BlockRaycastOnExiting = value;
            get => m_BlockRaycastOnExiting;
        }

        public float EnterDuration
        {
            set => m_EnterDuration = value;
            get => m_EnterDuration;
        }

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
        /// 进入切换（显示界面 + 开启遮罩）
        /// </summary>
        public virtual void Enter()
        {
            gameObject.SetActive(true);
            m_TopMaskLayer.raycastTarget = m_BlockRaycastOnEntering;
        }

        /// <summary>
        /// 进入动画结束
        /// 关闭遮罩、隐藏物体、派发进入完成事件
        /// </summary>
        public virtual void EnterOver()
        {
            m_TopMaskLayer.raycastTarget = false;
            gameObject.SetActive(false);
            GameMainRoot.Event.FireNow(this, GameEventCmd.ProcedureTransitionEnterOver);
        }

        /// <summary>
        /// 退出切换（显示界面 + 开启遮罩）
        /// </summary>
        public virtual void Exit()
        {
            gameObject.SetActive(true);
            m_TopMaskLayer.raycastTarget = m_BlockRaycastOnExiting;
        }

        /// <summary>
        /// 退出动画结束
        /// 关闭遮罩、派发退出完成事件
        /// </summary>
        public virtual void ExitOver()
        {
            gameObject.SetActive(true);
            m_TopMaskLayer.raycastTarget = false;
            GameMainRoot.Event.FireNow(this, GameEventCmd.ProcedureTransitionExitOver);
        }
        #endregion
    }
}