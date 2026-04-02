using System;
using UnityEngine;
using UnityEngine.UI;

namespace Honor.Runtime
{
    public sealed partial class UIGDPRBehaviour : MonoBehaviour
    {
        /// <summary>
        /// 顶部遮罩层
        /// </summary>
        [SerializeField]
        private Image m_TopMaskLayer;

        /// <summary>
        /// 是否为游戏中重复进入
        /// </summary>
        [HideInInspector]
        public bool InGame;

        /// <summary>
        /// 结束按钮点击回调
        /// 外部回调
        /// </summary>
        private Action m_OnOverButtonClickedCallback;
        public Action OnOverButtonClickedCallback
        {
            set
            {
                m_OnOverButtonClickedCallback = value;
            }
            get
            {
                return m_OnOverButtonClickedCallback;
            }
        }

        /// <summary>
        /// Persist持久化组件
        /// </summary>
        private PersistComponent m_PersistComponent;

        private void Awake()
        {
            m_PersistComponent = GameComponentsGroup.GetComponent<PersistComponent>();
            if (m_PersistComponent == null)
            {
                Log.Fatal("Persist Component 无效。");
                return;
            }
        }

        private void Start()
        {
            InitMainBg();
            InitMoreBg();
            InitResetBg();
       /*     if(!InGame) Root.SDK.TGAHelper.Track("Honor_gdpr_open");*/
        }

        private void OnDestroy()
        {

        }

    }

}
