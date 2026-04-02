using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Honor.Runtime
{
    public sealed class UIAppFeedbackBehaviour : MonoBehaviour
    {
        /// <summary>
        /// 标题文本
        /// </summary>
        [SerializeField]
        private Text m_TitleText;

        /// <summary>
        /// 标题文本
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI m_TitleTextTMP;

        /// <summary>
        /// 提示文本
        /// </summary>
        [SerializeField]
        private Text m_TipText;

        /// <summary>
        /// 提示文本
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI m_TipTextTMP;

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

        /// <summary>
        /// 提交按钮
        /// </summary>
        [SerializeField]
        private Button m_SubmitButton;

        /// <summary>
        /// 提交按钮文字
        /// </summary>
        [SerializeField]
        private Text m_SubmitButtonText;

        /// <summary>
        /// 提交按钮文字
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI m_SubmitButtonTextTMP;

        /// <summary>
        /// Email输入框
        /// </summary>
        [SerializeField]
        private InputField m_EmailInputField;

        /// <summary>
        /// Email占位文本
        /// </summary>
        [SerializeField]
        private Text m_EmailInputPlaceholderText;

        /// <summary>
        /// Email输入框
        /// </summary>
        [SerializeField]
        private TMP_InputField m_EmailInputFieldTMP;

        /// <summary>
        /// Email占位文本
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI m_EmailInputPlaceholderTextTMP;

        /// <summary>
        /// Issue输入框
        /// </summary>
        [SerializeField]
        private InputField m_IssueInputField;

        /// <summary>
        /// Issue占位文本
        /// </summary>
        [SerializeField]
        private Text m_IssueInputPlaceholderText;

        /// <summary>
        /// Issue输入框
        /// </summary>
        [SerializeField]
        private TMP_InputField m_IssueInputFieldTMP;

        /// <summary>
        /// Issue占位文本
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI m_IssueInputPlaceholderTextTMP;

        /// <summary>
        /// 星星数量
        /// </summary>
        private int m_StarNum;
        public int StarNum
        {
            set
            {
                m_StarNum = value;
            }
            get
            {
                return m_StarNum;
            }
        }

        /// <summary>
        /// 打点位置描述
        /// </summary>
        private string m_LocationDescForDot;
        public string LocationDescForDot
        {
            set
            {
                m_LocationDescForDot = value;
            }
            get
            {
                return m_LocationDescForDot;
            }
        }

        /// <summary>
        /// Emial地址内容
        /// </summary>
        private string m_EmailContent;
        public string EmailContent
        {
            get
            {
                return m_EmailContent;
            }
        }

        /// <summary>
        /// Issue描述内容
        /// </summary>
        private string m_IssueContent;
        public string IssueContent
        {
            get
            {
                return m_IssueContent;
            }
        }

        /// <summary>
        /// 是否已经提交
        /// </summary>
        private bool m_IsSubmit;

        private void Awake()
        {

        }

        private void Start()
        {
            if (m_TitleText != null)
            {
                m_TitleText.text = GameMainRoot.Localization.GetDefaultData("App_Feedback_Title_Text");
            }

            if (m_TitleTextTMP != null)
            {
                m_TitleTextTMP.text = GameMainRoot.Localization.GetDefaultData("App_Feedback_Title_Text");
            }

            if (m_CloseButtonText != null)
            {
                m_CloseButtonText.text = GameMainRoot.Localization.GetDefaultData("App_Feedback_CloseButton_Text");
            }

            if (m_CloseButtonTextTMP != null)
            {
                m_CloseButtonTextTMP.text = GameMainRoot.Localization.GetDefaultData("App_Feedback_CloseButton_Text");
            }

            if (m_SubmitButtonText != null)
            {
                m_SubmitButtonText.text = GameMainRoot.Localization.GetDefaultData("App_Feedback_SubmitButton_Text");
            }

            if (m_SubmitButtonTextTMP != null)
            {
                m_SubmitButtonTextTMP.text = GameMainRoot.Localization.GetDefaultData("App_Feedback_SubmitButton_Text");
            }

            if (m_EmailInputPlaceholderText != null)
            {
                m_EmailInputPlaceholderText.text = GameMainRoot.Localization.GetDefaultData("App_Feedback_EmailInputPlaceholder_Text");
            }

            if (m_EmailInputPlaceholderTextTMP != null)
            {
                m_EmailInputPlaceholderTextTMP.text = GameMainRoot.Localization.GetDefaultData("App_Feedback_EmailInputPlaceholder_Text");
            }

            if (m_IssueInputPlaceholderText != null)
            {
                m_IssueInputPlaceholderText.text = GameMainRoot.Localization.GetDefaultData("App_Feedback_IssueInputPlaceholder_Text");
            }

            if (m_IssueInputPlaceholderTextTMP != null)
            {
                m_IssueInputPlaceholderTextTMP.text = GameMainRoot.Localization.GetDefaultData("App_Feedback_IssueInputPlaceholder_Text");
            }

            m_SubmitButton.interactable = false;
            m_IsSubmit = false;

          /*  Root.SDK.TGAHelper.Track("Honor_feedback_show", new Dictionary<string, object>() {
                {"Honor_feedback_location", m_LocationDescForDot}
            });*/

        }

        private void OnDestroy()
        {
           /* Root.SDK.TGAHelper.Track("Honor_feedback_close", new Dictionary<string, object>() {
                {"Honor_feedback_location", m_LocationDescForDot},
                {"Honor_feedback_result", m_IsSubmit?"yes":"no"}
            });*/
        }

        public void OnSubmitButtonClicked()
        {
            m_IsSubmit = true;

            // 提交到服务器
            GameMainRoot.Lua.Env.DoString($"AccountHelper:UploadUserAppFeedbackData({m_StarNum}, '{m_EmailContent}', '{m_IssueContent}')");

           /* Root.SDK.TGAHelper.Track("Honor_feedback_click", new Dictionary<string, object>() {
                {"Honor_feedback_location", m_LocationDescForDot},
            });*/

            GameMainRoot.UI.CloseUIByGO(gameObject);
        }

        public void OnCloseButtonClicked()
        {
            GameMainRoot.UI.CloseUIByGO(gameObject);
        }

        public void OnEmailInputValueEndEdit(string valueEndEdit)
        {
            m_EmailContent = valueEndEdit;
            m_SubmitButton.interactable = !(string.IsNullOrEmpty(m_IssueContent) || m_IssueContent.Length < 3);
        }

        public void OnIssueInputValueEndEdit(string valueEndEdit)
        {
            m_IssueContent = valueEndEdit;
            m_SubmitButton.interactable = !(string.IsNullOrEmpty(m_IssueContent) || m_IssueContent.Length < 3);
        }

    }

}


