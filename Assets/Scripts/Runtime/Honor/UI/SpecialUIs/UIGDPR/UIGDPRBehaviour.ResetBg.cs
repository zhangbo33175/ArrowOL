using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Honor.Runtime
{
    public sealed partial class UIGDPRBehaviour : MonoBehaviour
    {
        /// <summary>
        /// 重设-界面
        /// </summary>
        [SerializeField]
        private CanvasGroup m_ResetBg;

        /// <summary>
        /// 标题文本
        /// </summary>
        [SerializeField]
        private Text m_ResetTitleText;

        /// <summary>
        /// 标题文本
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI m_ResetTitleTextTMP;

        /// <summary>
        /// 描述文本
        /// </summary>
        [SerializeField]
        private Text m_ResetDescText;

        /// <summary>
        /// 描述文本
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI m_ResetDescTextTMP;

        /// <summary>
        /// GDPR开关
        /// </summary>
        [SerializeField]
        private Toggle m_ResetGDPRToggle;

        /// <summary>
        /// CCPA开关
        /// </summary>
        [SerializeField]
        private Toggle m_ResetCCPAToggle;

        /// <summary>
        /// GDPR描述
        /// </summary>
        [SerializeField]
        private Text m_ResetGDPRText;

        /// <summary>
        /// GDPR描述
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI m_ResetGDPRTextTMP;

        /// <summary>
        /// CCPA描述
        /// </summary>
        [SerializeField]
        private Text m_ResetCCPAText;

        /// <summary>
        /// CCPA描述
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI m_ResetCCPATextTMP;

        /// <summary>
        /// GDPR隐私链接
        /// </summary>
        [SerializeField]
        private Button m_ResetGDPRLinkButton;

        /// <summary>
        /// CCPA隐私链接
        /// </summary>
        [SerializeField]
        private Button m_ResetCCPALinkButton;

        /// <summary>
        /// 保存按钮
        /// </summary>
        [SerializeField]
        private Button m_SaveButton;

        /// <summary>
        /// 保存按钮文字
        /// </summary>
        [SerializeField]
        private Text m_SaveButtonText;

        /// <summary>
        /// 保存按钮文字
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI m_SaveButtonTextTMP;

        /// <summary>
        /// 关闭按钮
        /// </summary>
        [SerializeField]
        private Button m_CloseButton;

        /// <summary>
        /// 关闭按钮文字
        /// </summary>
        [SerializeField]
        private Text m_CloseButtonText;

        /// <summary>
        /// 关闭按钮文字
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI m_CloseButtonTextTMP;

        private void InitResetBg()
        {
            if (m_ResetTitleText != null)
            {
                m_ResetTitleText.text = AorTxt.Format(GameMainRoot.Localization.GetDefaultData("GDPR_ResetTitle_Text"), Application.productName);
            }

            if (m_ResetTitleTextTMP != null)
            {
                m_ResetTitleTextTMP.text = AorTxt.Format(GameMainRoot.Localization.GetDefaultData("GDPR_ResetTitle_Text"), Application.productName);
            }

            if (m_ResetDescText != null)
            {
                m_ResetDescText.text = AorTxt.Format(GameMainRoot.Localization.GetDefaultData("GDPR_ResetDesc_Text"));
            }

            if (m_ResetDescTextTMP != null)
            {
                m_ResetDescTextTMP.text = AorTxt.Format(GameMainRoot.Localization.GetDefaultData("GDPR_ResetDesc_Text"));
            }

            if (m_ResetGDPRText != null)
            {
                m_ResetGDPRText.text = AorTxt.Format(GameMainRoot.Localization.GetDefaultData("GDPR_ResetGDPRDesc_Text"));
            }

            if (m_ResetGDPRTextTMP != null)
            {
                m_ResetGDPRTextTMP.text = AorTxt.Format(GameMainRoot.Localization.GetDefaultData("GDPR_ResetGDPRDesc_Text"));
            }

            if (m_ResetCCPAText != null)
            {
                m_ResetCCPAText.text = AorTxt.Format(GameMainRoot.Localization.GetDefaultData("GDPR_ResetCCPADesc_Text"));
            }

            if (m_ResetCCPATextTMP != null)
            {
                m_ResetCCPATextTMP.text = AorTxt.Format(GameMainRoot.Localization.GetDefaultData("GDPR_ResetCCPADesc_Text"));
            }

            if (m_SaveButtonText != null)
            {
                m_SaveButtonText.text = AorTxt.Format(GameMainRoot.Localization.GetDefaultData("GDPR_SaveButton_Text"));
            }

            if (m_SaveButtonTextTMP != null)
            {
                m_SaveButtonTextTMP.text = AorTxt.Format(GameMainRoot.Localization.GetDefaultData("GDPR_SaveButton_Text"));
            }

            if (m_CloseButtonText != null)
            {
                m_CloseButtonText.text = AorTxt.Format(GameMainRoot.Localization.GetDefaultData("GDPR_CloseButton_Text"));
            }

            if (m_CloseButtonTextTMP != null)
            {
                m_CloseButtonTextTMP.text = AorTxt.Format(GameMainRoot.Localization.GetDefaultData("GDPR_CloseButton_Text"));
            }

            if (m_ResetGDPRToggle != null)
            {
                m_ResetGDPRToggle.isOn = m_PersistComponent.GetBool(GameConstants.Persist.GDPR.WayType, GameConstants.Persist.GDPR.ClassifyName, GameConstants.Persist.GDPR.ItemKey.HasUserConsent, true);
                //m_ResetGDPRToggle.isOn = MaxSdk.IsUserConsentSet() ? MaxSdk.HasUserConsent():false;
            }
            if (m_ResetCCPAToggle != null)
            {
                m_ResetCCPAToggle.isOn = m_PersistComponent.GetBool(GameConstants.Persist.GDPR.WayType, GameConstants.Persist.GDPR.ClassifyName, GameConstants.Persist.GDPR.ItemKey.IsSell, true);
                //m_ResetCCPAToggle.isOn = MaxSdk.IsDoNotSellSet() ? !MaxSdk.IsDoNotSell():false;
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(m_ResetBg.rectTransform());
            m_ResetBg.gameObject.SetActive(InGame);
            if(InGame)
            {
                m_ResetBg.rectTransform().anchoredPosition = Vector2.zero;
            }
        }

        /// <summary>
        /// GDPR链接按钮回调
        /// </summary>
        public void OnResetGDPRLinkButtonClicked()
        {
            Application.OpenURL(GameMainRoot.Config.GetString("GDPRPrivacyUrl", true));
        }

        /// <summary>
        /// CCPA链接按钮回调
        /// </summary>
        public void OnResetCCPALinkButtonClicked()
        {
            Application.OpenURL(GameMainRoot.Config.GetString("CCPAPrivacyUrl", true));
        }

        /// <summary>
        /// 保存按钮回调
        /// </summary>
        public void OnSaveButtonClicked()
        {
            if (m_ResetGDPRToggle != null)
            {
                m_PersistComponent.SetBool(GameConstants.Persist.GDPR.WayType, GameConstants.Persist.GDPR.ClassifyName, GameConstants.Persist.GDPR.ItemKey.HasUserConsent, m_ResetGDPRToggle.isOn);
                //MaxSdk.SetHasUserConsent(m_ResetGDPRToggle.isOn);
            }
            if (m_ResetCCPAToggle != null)
            {
                m_PersistComponent.SetBool(GameConstants.Persist.GDPR.WayType, GameConstants.Persist.GDPR.ClassifyName, GameConstants.Persist.GDPR.ItemKey.IsSell, m_ResetCCPAToggle.isOn);
                //MaxSdk.SetDoNotSell(!m_ResetCCPAToggle.isOn);
            }
            m_PersistComponent.Save(GameConstants.Persist.GDPR.WayType);

            GameMainRoot.UI.CloseUIByGO(gameObject, true);

            if (m_OnOverButtonClickedCallback != null)
            {
                m_OnOverButtonClickedCallback();
            }
        }

        /// <summary>
        /// 关闭按钮回调
        /// </summary>
        public void OnCloseButtonClicked()
        {
            GameMainRoot.UI.CloseUIByGO(gameObject, true);

            if (m_OnOverButtonClickedCallback != null)
            {
                m_OnOverButtonClickedCallback();
            }
        }

    }

}
