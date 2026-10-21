/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  UIGDPRBehaviour.cs
 * author:    云毅
 * created:   2026
 * descrip:   GDPR欧盟隐私政策授权弹窗UI组件
 *            显示隐私授权界面、协议确认、授权状态存储管理
 ***************************************************************/

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
        #region 序列化字段 & 公共变量
        //=========================================================================
        // 序列化字段 & 公共变量
        //=========================================================================
        /// <summary>
        /// 顶部遮罩层（防止点击穿透）
        /// </summary>
        [SerializeField] private Image m_TopMaskLayer;

        /// <summary>
        /// 是否为游戏中重复进入（区分首次启动 / 中途打开）
        /// </summary>
        [HideInInspector] public bool InGame;
        #endregion

        #region 私有字段 & 公共属性
        //=========================================================================
        // 私有字段 & 公共属性
        //=========================================================================
        /// <summary>
        /// 完成按钮点击回调（外部注册，关闭后执行）
        /// </summary>
        private Action m_OnOverButtonClickedCallback;

        /// <summary>
        /// 完成按钮点击回调（外部注册，关闭后执行）
        /// </summary>
        public Action OnOverButtonClickedCallback
        {
            set => m_OnOverButtonClickedCallback = value;
            get => m_OnOverButtonClickedCallback;
        }
        #endregion

        #region 生命周期
        //=========================================================================
        // MonoBehaviour 生命周期
        //=========================================================================
        /// <summary>
        /// 唤醒
        /// <para>GDPR 授权状态经门面管理器（HonorCoreManager.Instance.Persist）按需访问，
        /// 外部不直接持有组件实例，统一走门面调用。</para>
        /// </summary>
        private void Awake()
        {
        }

        /// <summary>
        /// 启动时初始化各子界面（主界面 / 详情页 / 重置页）
        /// </summary>
        private void Start()
        {
            // 初始化背景相关界面
            InitMainBg();
            InitMoreBg();
            InitResetBg();

            // 埋点：首次打开 GDPR 界面
            /*if(!InGame) Root.SDK.TGAHelper.Track("Honor_gdpr_open");*/
        }

        /// <summary>
        /// 销毁（预留）
        /// </summary>
        private void OnDestroy()
        {
        }
        #endregion
    }
}