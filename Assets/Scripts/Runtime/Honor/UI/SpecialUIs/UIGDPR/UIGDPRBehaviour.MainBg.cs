using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Honor.Runtime
{
    public sealed partial class UIGDPRBehaviour : MonoBehaviour
    {
        /// <summary>
        /// 主-界面
        /// </summary>
        [SerializeField]
        private CanvasGroup m_MainBg;

        /// <summary>
        /// 标题文本
        /// </summary>
        [SerializeField]
        private Text m_MainTitleText;

        /// <summary>
        /// 标题文本
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI m_MainTitleTextTMP;

        /// <summary>
        /// 描述文本
        /// </summary>
        [SerializeField]
        private Text m_MainDescText;

        /// <summary>
        /// 描述文本
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI m_MainDescTextTMP;

        /// <summary>
        /// GDPR开关
        /// </summary>
        [SerializeField]
        private Toggle m_GDPRToggle;

        /// <summary>
        /// CCPA开关
        /// </summary>
        [SerializeField]
        private Toggle m_CCPAToggle;

        /// <summary>
        /// COPPA开关
        /// </summary>
        [SerializeField]
        private Toggle m_COPPAToggle;

        /// <summary>
        /// GDPR描述
        /// </summary>
        [SerializeField]
        private Text m_GDPRText;

        /// <summary>
        /// GDPR描述
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI m_GDPRTextTMP;

        /// <summary>
        /// CCPA描述
        /// </summary>
        [SerializeField]
        private Text m_CCPAText;

        /// <summary>
        /// CCPA描述
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI m_CCPATextTMP;

        /// <summary>
        /// COPPA描述
        /// </summary>
        [SerializeField]
        private Text m_COPPAText;

        /// <summary>
        /// COPPA描述
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI m_COPPATextTMP;

        /// <summary>
        /// GDPR隐私链接
        /// </summary>
        [SerializeField]
        private Button m_GDPRLinkButton;

        /// <summary>
        /// GDPR跳转链接按钮点击回调
        /// 外部回调
        /// </summary>
        private Action m_OnGDPRLinkButtonClickedCallback;
        public Action OnGDPRLinkButtonClickedCallback
        {
            set
            {
                m_OnGDPRLinkButtonClickedCallback = value;
            }
            get
            {
                return m_OnGDPRLinkButtonClickedCallback;
            }
        }

        /// <summary>
        /// CCPA隐私链接
        /// </summary>
        [SerializeField]
        private Button m_CCPALinkButton;

        /// <summary>
        /// CCPA跳转链接按钮点击回调
        /// 外部回调
        /// </summary>
        private Action m_OnCCPALinkButtonClickedCallback;
        public Action OnCCPALinkButtonClickedCallback
        {
            set
            {
                m_OnCCPALinkButtonClickedCallback = value;
            }
            get
            {
                return m_OnCCPALinkButtonClickedCallback;
            }
        }

        /// <summary>
        /// COPPA隐私链接
        /// </summary>
        [SerializeField]
        private Button m_COPPALinkButton;

        /// <summary>
        /// COPPA跳转链接按钮点击回调
        /// 外部回调
        /// </summary>
        private Action m_OnCOPPALinkButtonClickedCallback;
        public Action OnCOPPALinkButtonClickedCallback
        {
            set
            {
                m_OnCOPPALinkButtonClickedCallback = value;
            }
            get
            {
                return m_OnCOPPALinkButtonClickedCallback;
            }
        }

        /// <summary>
        /// 开始游戏按钮
        /// </summary>
        [SerializeField]
        private Button m_StartGameButton;

        /// <summary>
        /// 开始游戏按钮文字
        /// </summary>
        [SerializeField]
        private Text m_StartGameButtonText;

        /// <summary>
        /// 开始游戏按钮文字
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI m_StartGameButtonTextTMP;

        /// <summary>
        /// 了解更多按钮
        /// </summary>
        [SerializeField]
        private Button m_MoreButton;

        /// <summary>
        /// 了解更多按钮文字
        /// </summary>
        [SerializeField]
        private Text m_MoreButtonText;

        /// <summary>
        /// 了解更多按钮文字
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI m_MoreButtonTextTMP;

        private void InitMainBg()
        {
            if (m_MainTitleText != null)
            {
                m_MainTitleText.text = AorTxt.Format(GameMainRoot.Localization.GetDefaultData("GDPR_MainTitle_Text"), Application.productName);
            }

            if (m_MainTitleTextTMP != null)
            {
                m_MainTitleTextTMP.text = AorTxt.Format(GameMainRoot.Localization.GetDefaultData("GDPR_MainTitle_Text"), Application.productName);
            }

            if (m_MainDescText != null)
            {
                m_MainDescText.text = AorTxt.Format(GameMainRoot.Localization.GetDefaultData("GDPR_MainDesc_Text"), Application.productName, Application.productName, Application.productName);
            }

            if (m_MainDescTextTMP != null)
            {
                m_MainDescTextTMP.text = AorTxt.Format(GameMainRoot.Localization.GetDefaultData("GDPR_MainDesc_Text"), Application.productName, Application.productName, Application.productName);
            }

            if (m_GDPRText != null)
            {
                m_GDPRText.text = AorTxt.Format(GameMainRoot.Localization.GetDefaultData("GDPR_GDPRDesc_Text"));
            }

            if (m_GDPRTextTMP != null)
            {
                m_GDPRTextTMP.text = AorTxt.Format(GameMainRoot.Localization.GetDefaultData("GDPR_GDPRDesc_Text"));
            }

            if (m_CCPAText != null)
            {
                m_CCPAText.text = AorTxt.Format(GameMainRoot.Localization.GetDefaultData("GDPR_CCPADesc_Text"));
            }

            if (m_CCPATextTMP != null)
            {
                m_CCPATextTMP.text = AorTxt.Format(GameMainRoot.Localization.GetDefaultData("GDPR_CCPADesc_Text"));
            }

            if (m_COPPAText != null)
            {
                m_COPPAText.text = AorTxt.Format(GameMainRoot.Localization.GetDefaultData("GDPR_COPPADesc_Text"), Application.productName);
            }

            if (m_COPPATextTMP != null)
            {
                m_COPPATextTMP.text = AorTxt.Format(GameMainRoot.Localization.GetDefaultData("GDPR_COPPADesc_Text"), Application.productName);
            }

            if (m_StartGameButtonText != null)
            {
                m_StartGameButtonText.text = AorTxt.Format(GameMainRoot.Localization.GetDefaultData("GDPR_StartGameButton_Text"));
            }

            if (m_StartGameButtonTextTMP != null)
            {
                m_StartGameButtonTextTMP.text = AorTxt.Format(GameMainRoot.Localization.GetDefaultData("GDPR_StartGameButton_Text"));
            }

            if (m_MoreButtonText != null)
            {
                m_MoreButtonText.text = AorTxt.Format(GameMainRoot.Localization.GetDefaultData("GDPR_MoreButton_Text"));
            }

            if (m_MoreButtonTextTMP != null)
            {
                m_MoreButtonTextTMP.text = AorTxt.Format(GameMainRoot.Localization.GetDefaultData("GDPR_MoreButton_Text"));
            }

            if (m_GDPRToggle != null)
            {
                m_GDPRToggle.isOn = m_PersistComponent.GetBool(GameConstants.Persist.GDPR.WayType, GameConstants.Persist.GDPR.ClassifyName, GameConstants.Persist.GDPR.ItemKey.HasUserConsent, true);
                //MaxSdk.IsUserConsentSet() ? MaxSdk.HasUserConsent():false;
            }
            if (m_CCPAToggle != null)
            {
                m_CCPAToggle.isOn = m_PersistComponent.GetBool(GameConstants.Persist.GDPR.WayType, GameConstants.Persist.GDPR.ClassifyName, GameConstants.Persist.GDPR.ItemKey.IsSell, true);
                // MaxSdk.IsDoNotSellSet() ? !MaxSdk.IsDoNotSell():false;
            }
            if (m_COPPAToggle != null)
            {
                m_COPPAToggle.isOn = m_PersistComponent.GetBool(GameConstants.Persist.GDPR.WayType, GameConstants.Persist.GDPR.ClassifyName, GameConstants.Persist.GDPR.ItemKey.IsAgeReachStandard, true);
                // MaxSdk.IsAgeRestrictedUserSet() ? !MaxSdk.IsAgeRestrictedUser():false;
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(m_MainBg.rectTransform());
            m_MainBg.gameObject.SetActive(!InGame);
        }

        /// <summary>
        /// COPPA开关变化回调
        /// </summary>
        public void OnCOPPAToggleChanged()
        {
            m_StartGameButton.interactable = m_COPPAToggle.isOn;
        }

        /// <summary>
        /// GDPR链接按钮回调
        /// </summary>
        public void OnGDPRLinkButtonClicked()
        {
            Application.OpenURL(GameMainRoot.Config.GetString("GDPRPrivacyUrl", true));
        }

        /// <summary>
        /// CCPA链接按钮回调
        /// </summary>
        public void OnCCPALinkButtonClicked()
        {
            Application.OpenURL(GameMainRoot.Config.GetString("CCPAPrivacyUrl", true));
        }

        /// <summary>
        /// COPPA链接按钮回调
        /// </summary>
        public void OnCOPPALinkButtonClicked()
        {
            Application.OpenURL(GameMainRoot.Config.GetString("COPPAPrivacyUrl", true));
        }


        /// <summary>
        /// 开始游戏按钮回调
        /// </summary>
        public void OnStartGameButtonClicked()
        {
            if (m_GDPRToggle != null)
            {
                m_PersistComponent.SetBool(GameConstants.Persist.GDPR.WayType, GameConstants.Persist.GDPR.ClassifyName, GameConstants.Persist.GDPR.ItemKey.HasUserConsent, m_GDPRToggle.isOn);
                //MaxSdk.SetHasUserConsent(m_GDPRToggle.isOn);
            }
            if (m_CCPAToggle != null)
            {
                m_PersistComponent.SetBool(GameConstants.Persist.GDPR.WayType, GameConstants.Persist.GDPR.ClassifyName, GameConstants.Persist.GDPR.ItemKey.IsSell, m_CCPAToggle.isOn);
                //MaxSdk.SetDoNotSell(!m_CCPAToggle.isOn);
            }
            if (m_COPPAToggle != null)
            {
                m_PersistComponent.SetBool(GameConstants.Persist.GDPR.WayType, GameConstants.Persist.GDPR.ClassifyName, GameConstants.Persist.GDPR.ItemKey.IsAgeReachStandard, m_COPPAToggle.isOn);
                //MaxSdk.SetIsAgeRestrictedUser(!m_COPPAToggle.isOn);
            }
            m_PersistComponent.Save(GameConstants.Persist.GDPR.WayType);

            GameMainRoot.UI.CloseUIByGO(gameObject, true);

            if (m_OnOverButtonClickedCallback != null)
            {
                m_OnOverButtonClickedCallback();
            }
/*
            Root.SDK.TGAHelper.Track("Honor_gdpr_startgame");*/
        }

        /// <summary>
        /// 了解更多按钮回调
        /// </summary>
        public void OnMoreButtonClicked()
        {
            SetMainBgAnimationVisible(false);
            SetMoreBgAnimationVisible(true);
        }

        /// <summary>
        /// 设置主界面可见性
        /// </summary>
        /// <param name="visible"></param>
        public void SetMainBgAnimationVisible(bool visible)
        {
            m_TopMaskLayer.raycastTarget = true;
            if (visible)
            {
                if (m_MainBg != null)
                {
                    m_MainBg.gameObject.SetActive(true);
                    m_MainBg.alpha = 0f;
                    m_MainBg.transform.rectTransform().anchoredPosition = new Vector2(-800, 0);
                    DOTween.Sequence().Insert(0, DOTween.To(() => m_MainBg.alpha, (a) => m_MainBg.alpha = a, 1f, 1f))
                                      .Insert(0, DOTween.To(() => m_MainBg.transform.rectTransform().anchoredPosition, (pos) => m_MainBg.transform.rectTransform().anchoredPosition = pos, Vector2.zero, 1f))
                                      .AppendCallback(() => {
                                          m_TopMaskLayer.raycastTarget = false;
                                      });
                }
            }
            else
            {
                if (m_MainBg != null)
                {
                    m_MainBg.gameObject.SetActive(true);
                    m_MainBg.alpha = 1f;
                    m_MainBg.transform.rectTransform().anchoredPosition = Vector2.zero;
                    DOTween.Sequence().Insert(0, DOTween.To(() => m_MainBg.alpha, (a) => m_MainBg.alpha = a, 0f, 1f))
                                      .Insert(0, DOTween.To(() => m_MainBg.transform.rectTransform().anchoredPosition, (pos) => m_MainBg.transform.rectTransform().anchoredPosition = pos, m_MainBg.transform.rectTransform().anchoredPosition + new Vector2(-800, 0), 1f))
                                      .AppendCallback(() => {
                                          m_MainBg.gameObject.SetActive(false);
                                          m_TopMaskLayer.raycastTarget = false;
                                      });
                }
            }
        }

    }

}
