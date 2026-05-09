using System;
using UnityEngine;
using UnityEngine.UI;

namespace Honor.Runtime
{
    /// <summary>
    /// GDPR 欧盟隐私政策授权弹窗 UI 行为脚本
    /// 功能：显示 GDPR 授权界面、用户协议确认、隐私政策设置
    /// </summary>
    public sealed partial class UIGDPRBehaviour : MonoBehaviour
    {
        /// <summary>
        /// 顶部遮罩层（防止点击穿透）
        /// </summary>
        [SerializeField] private Image m_TopMaskLayer;

        /// <summary>
        /// 是否为游戏中重复进入（区分首次启动 / 中途打开）
        /// </summary>
        [HideInInspector] public bool InGame;

        /// <summary>
        /// 完成按钮点击回调（外部注册，关闭后执行）
        /// </summary>
        private Action m_OnOverButtonClickedCallback;

        public Action OnOverButtonClickedCallback
        {
            set => m_OnOverButtonClickedCallback = value;
            get => m_OnOverButtonClickedCallback;
        }

        /// <summary>
        /// 持久化数据组件（存储 GDPR 授权状态）
        /// </summary>
        private PersistComponent m_PersistComponent;

        private void Awake()
        {
            // 获取全局持久化数据组件
            m_PersistComponent = GameComponentsGroup.GetComponent<PersistComponent>();
            if (m_PersistComponent == null)
            {
                Log.Fatal("Persist Component 无效。");
                return;
            }
        }

        private void Start()
        {
            // 初始化背景相关界面
            InitMainBg();
            InitMoreBg();
            InitResetBg();

            // 埋点：首次打开 GDPR 界面
            /*if(!InGame) Root.SDK.TGAHelper.Track("Honor_gdpr_open");*/
        }

        private void OnDestroy()
        {
        }
    }
}