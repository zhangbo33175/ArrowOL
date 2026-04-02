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
        /// 了解更多-界面
        /// </summary>
        [SerializeField]
        private CanvasGroup m_MoreBg;

        /// <summary>
        /// 标题文本
        /// </summary>
        [SerializeField]
        private Text m_MoreTitleText;

        /// <summary>
        /// 标题文本
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI m_MoreTitleTextTMP;

        /// <summary>
        /// 描述文本
        /// </summary>
        [SerializeField]
        private Text m_MoreStartDescText;

        /// <summary>
        /// 描述文本
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI m_MoreStartDescTextTMP;

        /// <summary>
        /// 更多描述1
        /// </summary>
        [SerializeField]
        private Text m_MoreDesc1Text;

        /// <summary>
        /// 更多描述1
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI m_MoreDesc1TextTMP;

        /// <summary>
        /// 更多描述2
        /// </summary>
        [SerializeField]
        private Text m_MoreDesc2Text;

        /// <summary>
        /// 更多描述2
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI m_MoreDesc2TextTMP;

        /// <summary>
        /// 更多描述3
        /// </summary>
        [SerializeField]
        private Text m_MoreDesc3Text;

        /// <summary>
        /// 更多描述3
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI m_MoreDesc3TextTMP;

        /// <summary>
        /// 最后描述文本
        /// </summary>
        [SerializeField]
        private Text m_MoreFinalDescText;

        /// <summary>
        /// 最后描述文本
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI m_MoreFinalDescTextTMP;

        /// <summary>
        /// 返回按钮
        /// </summary>
        [SerializeField]
        private Button m_MoreBackButton;

        /// <summary>
        /// 返回按钮文字
        /// </summary>
        [SerializeField]
        private Text m_MoreBackButtonText;

        /// <summary>
        /// 返回按钮文字
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI m_MoreBackButtonTextTMP;

        /// <summary>
        /// 开始游戏按钮点击回调
        /// 外部回调
        /// </summary>
        private Action m_OnBackButtonClickedCallback;
        public Action OnBackButtonClickedCallback
        {
            set
            {
                m_OnBackButtonClickedCallback = value;
            }
            get
            {
                return m_OnBackButtonClickedCallback;
            }
        }

        private void InitMoreBg()
        {
            if (m_MoreTitleText != null)
            {
                m_MoreTitleText.text = AorTxt.Format(GameMainRoot.Localization.GetDefaultData("GDPR_MoreTitle_Text"), Application.productName);
            }

            if (m_MoreTitleTextTMP != null)
            {
                m_MoreTitleTextTMP.text = AorTxt.Format(GameMainRoot.Localization.GetDefaultData("GDPR_MoreTitle_Text"), Application.productName);
            }

            if (m_MoreStartDescText != null)
            {
                m_MoreStartDescText.text = AorTxt.Format(GameMainRoot.Localization.GetDefaultData("GDPR_MoreStartDesc_Text"));
            }

            if (m_MoreStartDescTextTMP != null)
            {
                m_MoreStartDescTextTMP.text = AorTxt.Format(GameMainRoot.Localization.GetDefaultData("GDPR_MoreStartDesc_Text"));
            }

            if (m_MoreDesc1Text != null)
            {
                m_MoreDesc1Text.text = AorTxt.Format(GameMainRoot.Localization.GetDefaultData("GDPR_MoreDesc1_Text"));
            }

            if (m_MoreDesc1TextTMP != null)
            {
                m_MoreDesc1TextTMP.text = AorTxt.Format(GameMainRoot.Localization.GetDefaultData("GDPR_MoreDesc1_Text"));
            }

            if (m_MoreDesc2Text != null)
            {
                m_MoreDesc2Text.text = AorTxt.Format(GameMainRoot.Localization.GetDefaultData("GDPR_MoreDesc2_Text"));
            }

            if (m_MoreDesc2TextTMP != null)
            {
                m_MoreDesc2TextTMP.text = AorTxt.Format(GameMainRoot.Localization.GetDefaultData("GDPR_MoreDesc2_Text"));
            }

            if (m_MoreDesc3Text != null)
            {
                m_MoreDesc3Text.text = AorTxt.Format(GameMainRoot.Localization.GetDefaultData("GDPR_MoreDesc3_Text"));
            }

            if (m_MoreDesc3TextTMP != null)
            {
                m_MoreDesc3TextTMP.text = AorTxt.Format(GameMainRoot.Localization.GetDefaultData("GDPR_MoreDesc3_Text"));
            }

            if (m_MoreFinalDescText != null)
            {
                m_MoreFinalDescText.text = AorTxt.Format(GameMainRoot.Localization.GetDefaultData("GDPR_MoreFinalDesc_Text"));
            }

            if (m_MoreFinalDescTextTMP != null)
            {
                m_MoreFinalDescTextTMP.text = AorTxt.Format(GameMainRoot.Localization.GetDefaultData("GDPR_MoreFinalDesc_Text"));
            }

            if (m_MoreBackButtonText != null)
            {
                m_MoreBackButtonText.text = AorTxt.Format(GameMainRoot.Localization.GetDefaultData("GDPR_MoreBackButton_Text"));
            }

            if (m_MoreBackButtonTextTMP != null)
            {
                m_MoreBackButtonTextTMP.text = AorTxt.Format(GameMainRoot.Localization.GetDefaultData("GDPR_MoreBackButton_Text"));
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(m_MoreBg.rectTransform());
            m_MoreBg.gameObject.SetActive(false);
        }

        /// <summary>
        /// 返回按钮回调
        /// </summary>
        public void OnMoreBackButtonClicked()
        {
            SetMainBgAnimationVisible(true);
            SetMoreBgAnimationVisible(false);
        }

        /// <summary>
        /// 设置更多界面可见性
        /// </summary>
        /// <param name="visible"></param>
        public void SetMoreBgAnimationVisible(bool visible)
        {
            m_TopMaskLayer.raycastTarget = true;
            if (visible)
            {
                if (m_MoreBg != null)
                {
                    m_MoreBg.gameObject.SetActive(true);
                    m_MoreBg.alpha = 0f;
                    m_MoreBg.transform.rectTransform().anchoredPosition = new Vector2(800, 0);
                    DOTween.Sequence().Insert(0, DOTween.To(() => m_MoreBg.alpha, (a) => m_MoreBg.alpha = a, 1f, 1f))
                                      .Insert(0, DOTween.To(() => m_MoreBg.transform.rectTransform().anchoredPosition, (pos) => m_MoreBg.transform.rectTransform().anchoredPosition = pos, Vector2.zero, 1f))
                                      .AppendCallback(() => {
                                          m_TopMaskLayer.raycastTarget = false;
                                      });
                }
            }
            else
            {
                if (m_MoreBg != null)
                {
                    m_MoreBg.gameObject.SetActive(true);
                    m_MoreBg.alpha = 1f;
                    m_MoreBg.transform.rectTransform().anchoredPosition = Vector2.zero;
                    DOTween.Sequence().Insert(0, DOTween.To(() => m_MoreBg.alpha, (a) => m_MoreBg.alpha = a, 0f, 1f))
                                      .Insert(0, DOTween.To(() => m_MoreBg.transform.rectTransform().anchoredPosition, (pos) => m_MoreBg.transform.rectTransform().anchoredPosition = pos, m_MoreBg.transform.rectTransform().anchoredPosition + new Vector2(800, 0), 1f))
                                      .AppendCallback(() => {
                                          m_MoreBg.gameObject.SetActive(false);
                                          m_TopMaskLayer.raycastTarget = false;
                                      });
                }
            }
        }

    }

}
