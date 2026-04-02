using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Honor.Runtime
{
    public sealed class UIAppDownloadBehaviour : MonoBehaviour
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
        /// 开始按钮
        /// </summary>
        [SerializeField]
        private Button m_StartButton;

        /// <summary>
        /// 开始按钮文字
        /// </summary>
        [SerializeField]
        private Text m_StartButtonText;

        /// <summary>
        /// 开始按钮文字
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI m_StartButtonTextTMP;

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
        /// 是否显示关闭按钮,默认显示,显示时就是非强制更新
        /// </summary>
        private bool isShowCloseBtn = true;
        /// <summary>
        /// 是否显示关闭按钮
        /// </summary>
        public bool ShowCloseButton
        {
            set
            {
                m_CloseButton.gameObject.SetActive(value);
            }
        }

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
                m_TitleText.text = GameMainRoot.Localization.GetDefaultData("UpdateTitle");
            }

            if (m_TitleTextTMP != null)
            {
                m_TitleTextTMP.text = GameMainRoot.Localization.GetDefaultData("UpdateTitle");
            }

            if (m_DescText != null)
            {
                m_DescText.text = GameMainRoot.Localization.GetDefaultData("UpdateText");
            }

            if (m_DescTextTMP != null)
            {
                m_DescTextTMP.text = GameMainRoot.Localization.GetDefaultData("UpdateText");
            }

            if (m_StartButtonText != null)
            {
                m_StartButtonText.text = GameMainRoot.Localization.GetDefaultData("UpdateButton");
            }

            if (m_StartButtonTextTMP != null)
            {
                m_StartButtonTextTMP.text = GameMainRoot.Localization.GetDefaultData("UpdateButton");
            }

            if (m_CloseButtonText != null)
            {
                m_CloseButtonText.text = GameMainRoot.Localization.GetDefaultData("App_Download_CloseButton_Text");
            }

            if (m_CloseButtonTextTMP != null)
            {
                m_CloseButtonTextTMP.text = GameMainRoot.Localization.GetDefaultData("App_Download_CloseButton_Text");
            }

        }

        private void OnDestroy()
        {

        }

        public void OnStartButtonClicked()
        {
   /*         // 应用版本更新-点击
            Root.SDK.TGAHelper.Track("Honor_hotfix_launcher", new Dictionary<string, object>() {
                { "Honor_hotfix_step", "app_version_go" }
            });*/

            
        }

        public void OnCloseButtonClicked()
        {
            // 应用版本更新-取消
          /*  Root.SDK.TGAHelper.Track("Honor_hotfix_launcher", new Dictionary<string, object>() {
                { "Honor_hotfix_step", "app_version_cancel" }
            });*/

            GameMainRoot.UI.CloseUIByGO(gameObject, true);
            // 非“大版本更新取消时强制退出游戏”时进行流程放行
            GameMainRoot.Event.Fire(this, GameEventCmd.FlowPermit);
        }

        /// <summary>
        /// 初始化界面
        /// </summary>
        /// <param name="isShowCloseBtn"></param>
        public void InitView(bool isShowCloseBtn)
        {
            this.isShowCloseBtn = isShowCloseBtn;
            ShowCloseButton = this.isShowCloseBtn;
            
            var updateTypeInt = this.isShowCloseBtn ? 2 : 1;
        /*    Root.SDK.TGAHelper.Track("Update", new Dictionary<string, object>() {
                { "update_type", updateTypeInt }
            });*/
        }
    }

}


