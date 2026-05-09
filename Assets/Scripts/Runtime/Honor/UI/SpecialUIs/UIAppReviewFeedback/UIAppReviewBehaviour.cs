using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Honor.Runtime
{
    /// <summary>
    /// APP 应用内评分界面 UI 行为脚本
    /// 功能：五星评分、提交评分、根据星级跳转到应用商店/反馈界面
    /// 兼容 UGUI Text / TextMeshProUGUI
    /// </summary>
    public sealed class UIAppReviewBehaviour : MonoBehaviour
    {
        /// <summary>
        /// 标题文本（UGUI Text）
        /// </summary>
        [SerializeField] private Text m_TitleText;

        /// <summary>
        /// 标题文本（TextMeshProUGUI）
        /// </summary>
        [SerializeField] private TextMeshProUGUI m_TitleTextTMP;

        /// <summary>
        /// 关闭按钮
        /// </summary>
        [SerializeField] private Button m_CloseButton;

        /// <summary>
        /// 关闭按钮文字（UGUI Text）
        /// </summary>
        [SerializeField] private Text m_CloseButtonText;

        /// <summary>
        /// 关闭按钮文字（TextMeshProUGUI）
        /// </summary>
        [SerializeField] private TextMeshProUGUI m_CloseButtonTextTMP;

        /// <summary>
        /// 五星评分按钮数组
        /// </summary>
        [SerializeField] private Button[] m_StarButtons;

        /// <summary>
        /// 五星显示图片数组
        /// </summary>
        [SerializeField] private Image[] m_StarImages;

        /// <summary>
        /// 描述文本（UGUI Text）
        /// </summary>
        [SerializeField] private Text m_DescText;

        /// <summary>
        /// 描述文本（TextMeshProUGUI）
        /// </summary>
        [SerializeField] private TextMeshProUGUI m_DescTextTMP;

        /// <summary>
        /// 提交按钮
        /// </summary>
        [SerializeField] private Button m_SubmitButton;

        /// <summary>
        /// 提交按钮文字（UGUI Text）
        /// </summary>
        [SerializeField] private Text m_SubmitButtonText;

        /// <summary>
        /// 提交按钮文字（TextMeshProUGUI）
        /// </summary>
        [SerializeField] private TextMeshProUGUI m_SubmitButtonTextTMP;

        /// <summary>
        /// 描述内容（备用扩展字段）
        /// </summary>
        private string m_DescContent;

        public string DescContent
        {
            set => m_DescContent = value;
            get => m_DescContent;
        }

        private void Awake()
        {
        }

        private void Start()
        {
            // 自动设置多语言文本（兼容 UGUI / TMP）
            if (m_TitleText != null)
                m_TitleText.text = GameMainRoot.Localization.GetDefaultData("App_Review_Title_Text");

            if (m_TitleTextTMP != null)
                m_TitleTextTMP.text = GameMainRoot.Localization.GetDefaultData("App_Review_Title_Text");

            if (m_CloseButtonText != null)
                m_CloseButtonText.text = GameMainRoot.Localization.GetDefaultData("App_Review_CloseButton_Text");

            if (m_CloseButtonTextTMP != null)
                m_CloseButtonTextTMP.text = GameMainRoot.Localization.GetDefaultData("App_Review_CloseButton_Text");

            if (m_DescText != null)
                m_DescText.text = GameMainRoot.Localization.GetDefaultData("App_Review_Desc_Text");

            if (m_DescTextTMP != null)
                m_DescTextTMP.text = GameMainRoot.Localization.GetDefaultData("App_Review_Desc_Text");

            if (m_SubmitButtonText != null)
                m_SubmitButtonText.text = GameMainRoot.Localization.GetDefaultData("App_Review_SubmitButton_Text");

            if (m_SubmitButtonTextTMP != null)
                m_SubmitButtonTextTMP.text = GameMainRoot.Localization.GetDefaultData("App_Review_SubmitButton_Text");

            // 埋点：展示评分界面
            /*Root.SDK.TGAHelper.Track("Honor_rating_show");*/
        }

        private void OnDestroy()
        {
            // 埋点：关闭评分界面
            /*Root.SDK.TGAHelper.Track("Honor_rating_close");*/
        }

        /// <summary>
        /// 星星按钮点击（点亮星级）
        /// </summary>
        /// <param name="index">点击的星星索引</param>
        public void OnStarButtonClicked(int index)
        {
            for (int starIndex = 0; starIndex < m_StarImages.Length; starIndex++)
            {
                m_StarImages[starIndex].gameObject.SetActive(starIndex <= index);
            }
        }

        /// <summary>
        /// 提交按钮点击
        /// </summary>
        public void OnSubmitButtonClicked()
        {
            int starNum = ComputeStarNum();

            // 5星 → 跳转到应用商店评分
            if (starNum == 5)
            {
                /*Root.SDK.ReviewHelper.RequestReview();*/
            }
            // 非5星 → 打开反馈界面
            else
            {
                /*if (Root.SDK.OpenCustomReviewFeedback)
                {
                    Root.UI.ShowAppFeedback(starNum);
                }
                else
                {
                    Root.UI.ShowFloatWords(Root.Localization.GetDefaultData("App_Review_Thanks_Text"));
                }*/
            }

            // 埋点：提交评分
            /*Root.SDK.TGAHelper.Track("Honor_rating_click", new Dictionary<string, object>() {
                { "Honor_star_number", starNum }
            });*/

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

        /// <summary>
        /// 计算当前点亮的星星数量
        /// </summary>
        private int ComputeStarNum()
        {
            int starNum = 0;
            for (int starIndex = 0; starIndex < m_StarImages.Length; starIndex++)
            {
                if (m_StarImages[starIndex].gameObject.activeSelf)
                {
                    starNum++;
                }
            }

            return starNum;
        }
    }
}