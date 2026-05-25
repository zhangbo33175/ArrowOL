/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  UIAppFeedbackBehaviour.cs
 * author:    云毅
 * created:   2026
 * descrip:   APP内用户反馈界面UI行为脚本
 *            支持评分、邮箱/问题输入、提交反馈、界面关闭，兼容UGUI/TMP
 ***************************************************************/

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Honor.Runtime
{
    /// <summary>
    /// APP 内用户反馈界面 UI 行为脚本
    /// 功能：评分、输入邮箱、输入问题描述、提交反馈、关闭界面
    /// 兼容 UGUI Text / TextMeshProUGUI / InputField 双版本
    /// </summary>
    public sealed class UIAppFeedbackBehaviour : MonoBehaviour
    {
        #region 序列化UI引用字段
        //=========================================================================
        // 序列化UI引用字段
        //=========================================================================
        /// <summary>
        /// 标题文本（UGUI）
        /// </summary>
        [SerializeField] private Text m_TitleText;

        /// <summary>
        /// 标题文本（TMP）
        /// </summary>
        [SerializeField] private TextMeshProUGUI m_TitleTextTMP;

        /// <summary>
        /// 提示文本（UGUI）
        /// </summary>
        [SerializeField] private Text m_TipText;

        /// <summary>
        /// 提示文本（TMP）
        /// </summary>
        [SerializeField] private TextMeshProUGUI m_TipTextTMP;

        /// <summary>
        /// 关闭按钮
        /// </summary>
        [SerializeField] private Button m_CloseButton;

        /// <summary>
        /// 关闭按钮文本（UGUI）
        /// </summary>
        [SerializeField] private Text m_CloseButtonText;

        /// <summary>
        /// 关闭按钮文本（TMP）
        /// </summary>
        [SerializeField] private TextMeshProUGUI m_CloseButtonTextTMP;

        /// <summary>
        /// 提交按钮
        /// </summary>
        [SerializeField] private Button m_SubmitButton;

        /// <summary>
        /// 提交按钮文本（UGUI）
        /// </summary>
        [SerializeField] private Text m_SubmitButtonText;

        /// <summary>
        /// 提交按钮文本（TMP）
        /// </summary>
        [SerializeField] private TextMeshProUGUI m_SubmitButtonTextTMP;

        /// <summary>
        /// 邮箱输入框（UGUI）
        /// </summary>
        [SerializeField] private InputField m_EmailInputField;

        /// <summary>
        /// 邮箱占位符文本（UGUI）
        /// </summary>
        [SerializeField] private Text m_EmailInputPlaceholderText;

        /// <summary>
        /// 邮箱输入框（TMP）
        /// </summary>
        [SerializeField] private TMP_InputField m_EmailInputFieldTMP;

        /// <summary>
        /// 邮箱占位符文本（TMP）
        /// </summary>
        [SerializeField] private TextMeshProUGUI m_EmailInputPlaceholderTextTMP;

        /// <summary>
        /// 问题描述输入框（UGUI）
        /// </summary>
        [SerializeField] private InputField m_IssueInputField;

        /// <summary>
        /// 问题描述占位符文本（UGUI）
        /// </summary>
        [SerializeField] private Text m_IssueInputPlaceholderText;

        /// <summary>
        /// 问题描述输入框（TMP）
        /// </summary>
        [SerializeField] private TMP_InputField m_IssueInputFieldTMP;

        /// <summary>
        /// 问题描述占位符文本（TMP）
        /// </summary>
        [SerializeField] private TextMeshProUGUI m_IssueInputPlaceholderTextTMP;
        #endregion

        #region 私有字段
        //=========================================================================
        // 私有字段
        //=========================================================================
        /// <summary>
        /// 评分星星数量
        /// </summary>
        private int m_StarNum;

        /// <summary>
        /// 打点/位置描述（用于反馈定位）
        /// </summary>
        private string m_LocationDescForDot;

        /// <summary>
        /// 邮箱内容
        /// </summary>
        private string m_EmailContent;

        /// <summary>
        /// 问题描述内容
        /// </summary>
        private string m_IssueContent;

        /// <summary>
        /// 是否已提交
        /// </summary>
        private bool m_IsSubmit;
        #endregion

        #region 公共属性
        //=========================================================================
        // 公共属性
        //=========================================================================
        /// <summary>
        /// 评分星星数量
        /// </summary>
        public int StarNum
        {
            set => m_StarNum = value;
            get => m_StarNum;
        }

        /// <summary>
        /// 打点/位置描述（用于反馈定位）
        /// </summary>
        public string LocationDescForDot
        {
            set => m_LocationDescForDot = value;
            get => m_LocationDescForDot;
        }

        /// <summary>
        /// 邮箱内容
        /// </summary>
        public string EmailContent
        {
            get => m_EmailContent;
        }

        /// <summary>
        /// 问题描述内容
        /// </summary>
        public string IssueContent
        {
            get => m_IssueContent;
        }
        #endregion

        #region 生命周期
        //=========================================================================
        // MonoBehaviour 生命周期
        //=========================================================================
        private void Awake()
        {
        }

        private void Start()
        {
            // 多语言自动赋值（兼容 UGUI / TMP）
            if (m_TitleText != null)
                m_TitleText.text = GameMainRoot.Localization.GetDefaultData("App_Feedback_Title_Text");

            if (m_TitleTextTMP != null)
                m_TitleTextTMP.text = GameMainRoot.Localization.GetDefaultData("App_Feedback_Title_Text");

            if (m_CloseButtonText != null)
                m_CloseButtonText.text = GameMainRoot.Localization.GetDefaultData("App_Feedback_CloseButton_Text");

            if (m_CloseButtonTextTMP != null)
                m_CloseButtonTextTMP.text = GameMainRoot.Localization.GetDefaultData("App_Feedback_CloseButton_Text");

            if (m_SubmitButtonText != null)
                m_SubmitButtonText.text = GameMainRoot.Localization.GetDefaultData("App_Feedback_SubmitButton_Text");

            if (m_SubmitButtonTextTMP != null)
                m_SubmitButtonTextTMP.text = GameMainRoot.Localization.GetDefaultData("App_Feedback_SubmitButton_Text");

            if (m_EmailInputPlaceholderText != null)
                m_EmailInputPlaceholderText.text =
                    GameMainRoot.Localization.GetDefaultData("App_Feedback_EmailInputPlaceholder_Text");

            if (m_EmailInputPlaceholderTextTMP != null)
                m_EmailInputPlaceholderTextTMP.text =
                    GameMainRoot.Localization.GetDefaultData("App_Feedback_EmailInputPlaceholder_Text");

            if (m_IssueInputPlaceholderText != null)
                m_IssueInputPlaceholderText.text =
                    GameMainRoot.Localization.GetDefaultData("App_Feedback_IssueInputPlaceholder_Text");

            if (m_IssueInputPlaceholderTextTMP != null)
                m_IssueInputPlaceholderTextTMP.text =
                    GameMainRoot.Localization.GetDefaultData("App_Feedback_IssueInputPlaceholder_Text");

            // 初始时提交按钮不可点击
            m_SubmitButton.interactable = false;
            m_IsSubmit = false;
        }

        private void OnDestroy()
        {
        }
        #endregion

        #region UI按钮点击事件
        //=========================================================================
        // UI按钮点击事件
        //=========================================================================
        /// <summary>
        /// 提交按钮点击
        /// </summary>
        public void OnSubmitButtonClicked()
        {
            m_IsSubmit = true;

            // 调用 Lua 层上传反馈数据
            GameMainRoot.Lua.Env.DoString(
                $"AccountHelper:UploadUserAppFeedbackData({m_StarNum}, '{m_EmailContent}', '{m_IssueContent}')");

            // 关闭界面
            GameMainRoot.UI.CloseUIByGO(gameObject);
        }

        /// <summary>
        /// 关闭按钮点击
        /// </summary>
        public void OnCloseButtonClicked()
        {
            GameMainRoot.UI.CloseUIByGO(gameObject);
        }
        #endregion

        #region 输入框回调事件
        //=========================================================================
        // 输入框回调事件
        //=========================================================================
        /// <summary>
        /// 邮箱输入结束回调
        /// </summary>
        /// <param name="valueEndEdit">输入完成的邮箱内容</param>
        public void OnEmailInputValueEndEdit(string valueEndEdit)
        {
            m_EmailContent = valueEndEdit;

            // 问题描述长度 >=3 才可提交
            m_SubmitButton.interactable = !(string.IsNullOrEmpty(m_IssueContent) || m_IssueContent.Length < 3);
        }

        /// <summary>
        /// 问题描述输入结束回调
        /// </summary>
        /// <param name="valueEndEdit">输入完成的问题内容</param>
        public void OnIssueInputValueEndEdit(string valueEndEdit)
        {
            m_IssueContent = valueEndEdit;

            // 问题描述长度 >=3 才可提交
            m_SubmitButton.interactable = !(string.IsNullOrEmpty(m_IssueContent) || m_IssueContent.Length < 3);
        }
        #endregion
    }
}