using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Honor.Runtime
{
    public sealed class UIAppReviewBehaviour : MonoBehaviour
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
        /// 星星按钮
        /// </summary>
        [SerializeField]
        private Button[] m_StarButtons;

        /// <summary>
        /// 星星图片
        /// </summary>
        [SerializeField]
        private Image[] m_StarImages;

        /// <summary>
        /// 描述文本
        /// </summary>
        [SerializeField]
        private Text m_DescText;

        /// <summary>
        /// 描述文本
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI m_DescTextTMP;

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
        /// 描述性内容
        /// 比如当前正在下载的文件名称等
        /// </summary>
        private string m_DescContent;
        public string DescContent
        {
            set
            {
                m_DescContent = value;
            }
            get
            {
                return m_DescContent;
            }
        }

        private void Awake()
        {

        }

        private void Start()
        {
            if (m_TitleText != null)
            {
                m_TitleText.text = GameMainRoot.Localization.GetDefaultData("App_Review_Title_Text");
            }

            if (m_TitleTextTMP != null)
            {
                m_TitleTextTMP.text = GameMainRoot.Localization.GetDefaultData("App_Review_Title_Text");
            }

            if (m_CloseButtonText != null)
            {
                m_CloseButtonText.text = GameMainRoot.Localization.GetDefaultData("App_Review_CloseButton_Text");
            }

            if (m_CloseButtonTextTMP != null)
            {
                m_CloseButtonTextTMP.text = GameMainRoot.Localization.GetDefaultData("App_Review_CloseButton_Text");
            }

            if (m_DescText != null)
            {
                m_DescText.text = GameMainRoot.Localization.GetDefaultData("App_Review_Desc_Text");
            }

            if (m_DescTextTMP != null)
            {
                m_DescTextTMP.text = GameMainRoot.Localization.GetDefaultData("App_Review_Desc_Text");
            }

            if (m_SubmitButtonText != null)
            {
                m_SubmitButtonText.text = GameMainRoot.Localization.GetDefaultData("App_Review_SubmitButton_Text");
            }

            if (m_SubmitButtonTextTMP != null)
            {
                m_SubmitButtonTextTMP.text = GameMainRoot.Localization.GetDefaultData("App_Review_SubmitButton_Text");
            }

/*            Root.SDK.TGAHelper.Track("Honor_rating_show");*/

        }

        private void OnDestroy()
        {
           /* Root.SDK.TGAHelper.Track("Honor_rating_close");*/
        }

        public void OnStarButtonClicked(int index)
        {
            for(int starIndex = 0; starIndex < m_StarImages.Length; starIndex++)
            {
                m_StarImages[starIndex].gameObject.SetActive(starIndex <= index);
            }
        }

        public void OnSubmitButtonClicked()
        {
            int starNum = ComputeStarNum();
            if(starNum == 5)
            {
                // 调起（Android/iOS）原生平台应用内评价界面
             /*   Root.SDK.ReviewHelper.RequestReview();*/
            }
            else
            {
              /*  if(Root.SDK.OpenCustomReviewFeedback)
                {
                    // 调起自定义反馈界面
                    Root.UI.ShowAppFeedback(starNum);
                }
                else
                {
                    Root.UI.ShowFloatWords(Root.Localization.GetDefaultData("App_Review_Thanks_Text"));
                }*/
            }

           /* Root.SDK.TGAHelper.Track("Honor_rating_click", new Dictionary<string, object>() {
                { "Honor_star_number", starNum }
            });*/

            GameMainRoot.UI.CloseUIByGO(gameObject);
        }

        public void OnCloseButtonClicked()
        {
            GameMainRoot.UI.CloseUIByGO(gameObject);
        }

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


