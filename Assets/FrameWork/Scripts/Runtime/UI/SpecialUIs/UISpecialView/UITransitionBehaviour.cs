using UnityEngine;
using UnityEngine.UI;

namespace Honor.Runtime
{
    public class UITransitionBehaviour: MonoBehaviour
    {
        /// <summary>
        /// 背景图
        /// </summary>
        [SerializeField]
        protected Image m_BgImage;

        /// <summary>
        /// 底部遮罩层
        /// </summary>
        [SerializeField]
        protected Image m_BottomMaskLayer;

        /// <summary>
        /// 顶部遮罩层
        /// </summary>
        [SerializeField]
        protected Image m_TopMaskLayer;

        /// <summary>
        /// 进入时阻塞射线
        /// </summary>
        [SerializeField]
        protected bool m_BlockRaycastOnEntering = false;
        public bool BlockRaycastOnEntering { set => m_BlockRaycastOnEntering = value; get => m_BlockRaycastOnEntering; }

        /// <summary>
        /// 退出时阻塞射线
        /// </summary>
        [SerializeField]
        protected bool m_BlockRaycastOnExiting = true;
        public bool BlockRaycastOnExiting { set => m_BlockRaycastOnExiting = value; get => m_BlockRaycastOnExiting; }

        /// <summary>
        /// 进入时间
        /// </summary>
        [SerializeField]
        protected float m_EnterDuration;
        public float EnterDuration { set => m_EnterDuration = value; get => m_EnterDuration; }

        /// <summary>
        /// 退出时间
        /// </summary>
        [SerializeField]
        protected float m_ExitDuration;
        public float ExitDuration { set => m_ExitDuration = value; get => m_ExitDuration; }

        /// <summary>
        /// 进入切换
        /// </summary>
        public virtual void Enter()
        {
            gameObject.SetActive(true);
            m_TopMaskLayer.raycastTarget = m_BlockRaycastOnEntering;
        }

        /// <summary>
        /// 进入切换结束
        /// </summary>
        public virtual void EnterOver()
        {
            m_TopMaskLayer.raycastTarget = false;
            gameObject.SetActive(false);
            EventManager.Instance.DispatchEventNow(this, EventCmd.ProcedureTransitionEnterOver);
        }

        /// <summary>
        /// 退出切换
        /// </summary>
        public virtual void Exit()
        {
            gameObject.SetActive(true);
            m_TopMaskLayer.raycastTarget = m_BlockRaycastOnExiting;
        }

        /// <summary>
        /// 退出切换结束
        /// </summary>
        public virtual void ExitOver()
        {
            gameObject.SetActive(true);
            m_TopMaskLayer.raycastTarget = false;
           EventManager.Instance.DispatchEventNow(this, EventCmd.ProcedureTransitionExitOver);
        }
 
    }
}