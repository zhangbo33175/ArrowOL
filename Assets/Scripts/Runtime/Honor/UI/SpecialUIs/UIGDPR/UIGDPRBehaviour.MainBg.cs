/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  UIGDPRBehaviour_Main.cs
 * author:    云毅
 * created:   2026
 * descrip:   GDPR隐私政策弹窗 - 主界面分部类
 *            授权开关、协议链接、开始游戏、界面动画逻辑
 ***************************************************************/

using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Honor.Runtime
{
    /// <summary>
    /// GDPR 隐私政策弹窗 - 主界面部分
    /// 功能：GDPR / CCPA / COPPA 授权开关、隐私协议链接、开始游戏、了解更多
    /// </summary>
    public sealed partial class UIGDPRBehaviour : MonoBehaviour
    {
        #region 序列化UI引用字段
        //=========================================================================
        // 序列化UI引用字段
        //=========================================================================
        /// <summary>
        /// 主界面画布组（控制显隐+动画）
        /// </summary>
        [SerializeField] private CanvasGroup m_MainBg;

        /// <summary>
        /// 主标题（UGUI Text）
        /// </summary>
        [SerializeField] private Text m_MainTitleText;

        /// <summary>
        /// 主标题（TextMeshProUGUI）
        /// </summary>
        [SerializeField] private TextMeshProUGUI m_MainTitleTextTMP;

        /// <summary>
        /// 主描述文本（UGUI Text）
        /// </summary>
        [SerializeField] private Text m_MainDescText;

        /// <summary>
        /// 主描述文本（TextMeshProUGUI）
        /// </summary>
        [SerializeField] private TextMeshProUGUI m_MainDescTextTMP;

        /// <summary>
        /// GDPR 数据授权开关
        /// </summary>
        [SerializeField] private Toggle m_GDPRToggle;

        /// <summary>
        /// CCPA 加州隐私授权开关
        /// </summary>
        [SerializeField] private Toggle m_CCPAToggle;

        /// <summary>
        /// COPPA 儿童隐私授权开关
        /// </summary>
        [SerializeField] private Toggle m_COPPAToggle;

        /// <summary>
        /// GDPR 说明文字（UGUI Text）
        /// </summary>
        [SerializeField] private Text m_GDPRText;

        /// <summary>
        /// GDPR 说明文字（TMP）
        /// </summary>
        [SerializeField] private TextMeshProUGUI m_GDPRTextTMP;

        /// <summary>
        /// CCPA 说明文字（UGUI Text）
        /// </summary>
        [SerializeField] private Text m_CCPAText;

        /// <summary>
        /// CCPA 说明文字（TMP）
        /// </summary>
        [SerializeField] private TextMeshProUGUI m_CCPATextTMP;

        /// <summary>
        /// COPPA 说明文字（UGUI Text）
        /// </summary>
        [SerializeField] private Text m_COPPAText;

        /// <summary>
        /// COPPA 说明文字（TMP）
        /// </summary>
        [SerializeField] private TextMeshProUGUI m_COPPATextTMP;

        /// <summary>
        /// GDPR 隐私政策链接按钮
        /// </summary>
        [SerializeField] private Button m_GDPRLinkButton;

        /// <summary>
        /// CCPA 隐私政策链接按钮
        /// </summary>
        [SerializeField] private Button m_CCPALinkButton;

        /// <summary>
        /// COPPA 隐私政策链接按钮
        /// </summary>
        [SerializeField] private Button m_COPPALinkButton;

        /// <summary>
        /// 开始游戏按钮
        /// </summary>
        [SerializeField] private Button m_StartGameButton;

        /// <summary>
        /// 开始游戏按钮文字（UGUI Text）
        /// </summary>
        [SerializeField] private Text m_StartGameButtonText;

        /// <summary>
        /// 开始游戏按钮文字（TMP）
        /// </summary>
        [SerializeField] private TextMeshProUGUI m_StartGameButtonTextTMP;

        /// <summary>
        /// 了解更多按钮
        /// </summary>
        [SerializeField] private Button m_MoreButton;

        /// <summary>
        /// 了解更多按钮文字（UGUI Text）
        /// </summary>
        [SerializeField] private Text m_MoreButtonText;

        /// <summary>
        /// 了解更多按钮文字（TMP）
        /// </summary>
        [SerializeField] private TextMeshProUGUI m_MoreButtonTextTMP;
        #endregion

        #region 私有字段 & 公共属性
        //=========================================================================
        // 私有字段 & 公共属性
        //=========================================================================
        /// <summary>
        /// GDPR 链接点击回调（外部）
        /// </summary>
        private Action m_OnGDPRLinkButtonClickedCallback;

        /// <summary>
        /// CCPA 链接点击回调（外部）
        /// </summary>
        private Action m_OnCCPALinkButtonClickedCallback;

        /// <summary>
        /// COPPA 链接点击回调（外部）
        /// </summary>
        private Action m_OnCOPPALinkButtonClickedCallback;

        /// <summary>
        /// GDPR 链接点击回调（外部）
        /// </summary>
        public Action OnGDPRLinkButtonClickedCallback
        {
            set => m_OnGDPRLinkButtonClickedCallback = value;
            get => m_OnGDPRLinkButtonClickedCallback;
        }

        /// <summary>
        /// CCPA 链接点击回调（外部）
        /// </summary>
        public Action OnCCPALinkButtonClickedCallback
        {
            set => m_OnCCPALinkButtonClickedCallback = value;
            get => m_OnCCPALinkButtonClickedCallback;
        }

        /// <summary>
        /// COPPA 链接点击回调（外部）
        /// </summary>
        public Action OnCOPPALinkButtonClickedCallback
        {
            set => m_OnCOPPALinkButtonClickedCallback = value;
            get => m_OnCOPPALinkButtonClickedCallback;
        }
        #endregion

        #region 主界面初始化
        //=========================================================================
        // 主界面初始化
        //=========================================================================
        /// <summary>
        /// 初始化主界面：多语言、开关状态、布局刷新
        /// </summary>
        private void InitMainBg()
        {
            // 设置多语言标题
            if (m_MainTitleText != null)
                m_MainTitleText.text = AorTxt.Format(GameMainRoot.Localization.GetDefaultData("GDPR_MainTitle_Text"),
                    Application.productName);

            if (m_MainTitleTextTMP != null)
                m_MainTitleTextTMP.text = AorTxt.Format(GameMainRoot.Localization.GetDefaultData("GDPR_MainTitle_Text"),
                    Application.productName);

            // 设置多语言描述
            if (m_MainDescText != null)
                m_MainDescText.text = AorTxt.Format(GameMainRoot.Localization.GetDefaultData("GDPR_MainDesc_Text"),
                    Application.productName, Application.productName, Application.productName);

            if (m_MainDescTextTMP != null)
                m_MainDescTextTMP.text = AorTxt.Format(GameMainRoot.Localization.GetDefaultData("GDPR_MainDesc_Text"),
                    Application.productName, Application.productName, Application.productName);

            // GDPR 文字
            if (m_GDPRText != null)
                m_GDPRText.text = AorTxt.Format(GameMainRoot.Localization.GetDefaultData("GDPR_GDPRDesc_Text"));

            if (m_GDPRTextTMP != null)
                m_GDPRTextTMP.text = AorTxt.Format(GameMainRoot.Localization.GetDefaultData("GDPR_GDPRDesc_Text"));

            // CCPA 文字
            if (m_CCPAText != null)
                m_CCPAText.text = AorTxt.Format(GameMainRoot.Localization.GetDefaultData("GDPR_CCPADesc_Text"));

            if (m_CCPATextTMP != null)
                m_CCPATextTMP.text = AorTxt.Format(GameMainRoot.Localization.GetDefaultData("GDPR_CCPADesc_Text"));

            // COPPA 文字
            if (m_COPPAText != null)
                m_COPPAText.text = AorTxt.Format(GameMainRoot.Localization.GetDefaultData("GDPR_COPPADesc_Text"),
                    Application.productName);

            if (m_COPPATextTMP != null)
                m_COPPATextTMP.text = AorTxt.Format(GameMainRoot.Localization.GetDefaultData("GDPR_COPPADesc_Text"),
                    Application.productName);

            // 开始游戏按钮文字
            if (m_StartGameButtonText != null)
                m_StartGameButtonText.text =
                    AorTxt.Format(GameMainRoot.Localization.GetDefaultData("GDPR_StartGameButton_Text"));

            if (m_StartGameButtonTextTMP != null)
                m_StartGameButtonTextTMP.text =
                    AorTxt.Format(GameMainRoot.Localization.GetDefaultData("GDPR_StartGameButton_Text"));

            // 了解更多按钮文字
            if (m_MoreButtonText != null)
                m_MoreButtonText.text = AorTxt.Format(GameMainRoot.Localization.GetDefaultData("GDPR_MoreButton_Text"));

            if (m_MoreButtonTextTMP != null)
                m_MoreButtonTextTMP.text =
                    AorTxt.Format(GameMainRoot.Localization.GetDefaultData("GDPR_MoreButton_Text"));

            // 从持久化数据读取开关状态
            if (m_GDPRToggle != null)
            {
                m_GDPRToggle.isOn = m_PersistComponent.GetBool(GameConstants.Persist.GDPR.WayType,
                    GameConstants.Persist.GDPR.ClassifyName, GameConstants.Persist.GDPR.ItemKey.HasUserConsent, true);
            }

            if (m_CCPAToggle != null)
            {
                m_CCPAToggle.isOn = m_PersistComponent.GetBool(GameConstants.Persist.GDPR.WayType,
                    GameConstants.Persist.GDPR.ClassifyName, GameConstants.Persist.GDPR.ItemKey.IsSell, true);
            }

            if (m_COPPAToggle != null)
            {
                m_COPPAToggle.isOn = m_PersistComponent.GetBool(GameConstants.Persist.GDPR.WayType,
                    GameConstants.Persist.GDPR.ClassifyName, GameConstants.Persist.GDPR.ItemKey.IsAgeReachStandard,
                    true);
            }

            // 强制刷新布局
            LayoutRebuilder.ForceRebuildLayoutImmediate(m_MainBg.rectTransform());
            m_MainBg.gameObject.SetActive(!InGame);
        }
        #endregion

        #region UI交互事件
        //=========================================================================
        // UI交互事件
        //=========================================================================
        /// <summary>
        /// COPPA 开关变化时控制开始按钮是否可点击
        /// </summary>
        public void OnCOPPAToggleChanged()
        {
            m_StartGameButton.interactable = m_COPPAToggle.isOn;
        }

        /// <summary>
        /// 打开 GDPR 隐私政策链接
        /// </summary>
        public void OnGDPRLinkButtonClicked()
        {
            Application.OpenURL(GameMainRoot.Config.GetString("GDPRPrivacyUrl", true));
        }

        /// <summary>
        /// 打开 CCPA 隐私政策链接
        /// </summary>
        public void OnCCPALinkButtonClicked()
        {
            Application.OpenURL(GameMainRoot.Config.GetString("CCPAPrivacyUrl", true));
        }

        /// <summary>
        /// 打开 COPPA 隐私政策链接
        /// </summary>
        public void OnCOPPALinkButtonClicked()
        {
            Application.OpenURL(GameMainRoot.Config.GetString("COPPAPrivacyUrl", true));
        }

        /// <summary>
        /// 开始游戏按钮：保存授权状态 → 关闭界面 → 执行回调
        /// </summary>
        public void OnStartGameButtonClicked()
        {
            // 保存 GDPR 开关
            if (m_GDPRToggle != null)
            {
                m_PersistComponent.SetBool(GameConstants.Persist.GDPR.WayType, GameConstants.Persist.GDPR.ClassifyName,
                    GameConstants.Persist.GDPR.ItemKey.HasUserConsent, m_GDPRToggle.isOn);
            }

            // 保存 CCPA 开关
            if (m_CCPAToggle != null)
            {
                m_PersistComponent.SetBool(GameConstants.Persist.GDPR.WayType, GameConstants.Persist.GDPR.ClassifyName,
                    GameConstants.Persist.GDPR.ItemKey.IsSell, m_CCPAToggle.isOn);
            }

            // 保存 COPPA 开关
            if (m_COPPAToggle != null)
            {
                m_PersistComponent.SetBool(GameConstants.Persist.GDPR.WayType, GameConstants.Persist.GDPR.ClassifyName,
                    GameConstants.Persist.GDPR.ItemKey.IsAgeReachStandard, m_COPPAToggle.isOn);
            }

            // 持久化保存
            m_PersistComponent.Save(GameConstants.Persist.GDPR.WayType);

            // 关闭界面
            GameMainRoot.UI.CloseUIByGO(gameObject, true);

            // 执行外部完成回调
            if (m_OnOverButtonClickedCallback != null)
            {
                m_OnOverButtonClickedCallback();
            }

            // 埋点：开始游戏
            /*Root.SDK.TGAHelper.Track("Honor_gdpr_startgame");*/
        }

        /// <summary>
        /// 了解更多按钮：切换到详细说明页
        /// </summary>
        public void OnMoreButtonClicked()
        {
            SetMainBgAnimationVisible(false);
            SetMoreBgAnimationVisible(true);
        }
        #endregion

        #region 界面动画控制
        //=========================================================================
        // 界面动画控制
        //=========================================================================
        /// <summary>
        /// 主界面显隐动画（滑入/滑出）
        /// </summary>
        public void SetMainBgAnimationVisible(bool visible)
        {
            m_TopMaskLayer.raycastTarget = true;

            if (visible)
            {
                // 显示动画：从左侧滑入 + 淡入
                if (m_MainBg != null)
                {
                    m_MainBg.gameObject.SetActive(true);
                    m_MainBg.alpha = 0f;
                    m_MainBg.transform.rectTransform().anchoredPosition = new Vector2(-800, 0);

                    DOTween.Sequence()
                        .Insert(0, DOTween.To(() => m_MainBg.alpha, a => m_MainBg.alpha = a, 1f, 1f))
                        .Insert(0,
                            DOTween.To(() => m_MainBg.transform.rectTransform().anchoredPosition,
                                pos => m_MainBg.transform.rectTransform().anchoredPosition = pos, Vector2.zero, 1f))
                        .AppendCallback(() => { m_TopMaskLayer.raycastTarget = false; });
                }
            }
            else
            {
                // 隐藏动画：向左侧滑出 + 淡出
                if (m_MainBg != null)
                {
                    m_MainBg.gameObject.SetActive(true);
                    m_MainBg.alpha = 1f;
                    m_MainBg.transform.rectTransform().anchoredPosition = Vector2.zero;

                    DOTween.Sequence()
                        .Insert(0, DOTween.To(() => m_MainBg.alpha, a => m_MainBg.alpha = a, 0f, 1f))
                        .Insert(0,
                            DOTween.To(() => m_MainBg.transform.rectTransform().anchoredPosition,
                                pos => m_MainBg.transform.rectTransform().anchoredPosition = pos,
                                m_MainBg.transform.rectTransform().anchoredPosition + new Vector2(-800, 0), 1f))
                        .AppendCallback(() =>
                        {
                            m_MainBg.gameObject.SetActive(false);
                            m_TopMaskLayer.raycastTarget = false;
                        });
                }
            }
        }
        #endregion
    }
}