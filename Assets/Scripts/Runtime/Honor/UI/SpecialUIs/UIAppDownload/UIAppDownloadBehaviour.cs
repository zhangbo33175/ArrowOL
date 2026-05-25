/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  UIAppDownloadBehaviour.cs
 * author:    云毅
 * created:   2026
 * descrip:   APP应用内下载/更新弹窗UI组件
 *            支持强制/非强制更新、多语言文本、Text/TMP兼容显示
 ***************************************************************/

using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Honor.Runtime
{
    /// <summary>
    /// APP 应用内下载/更新弹窗行为脚本
    /// 功能：显示强制/非强制更新弹窗，支持 Text / TextMeshProUGUI
    /// 提供：标题、描述、开始更新、关闭取消 等逻辑
    /// </summary>
    public sealed class UIAppDownloadBehaviour : MonoBehaviour
    {
        #region 序列化字段
        //=========================================================================
        // 序列化UI引用字段
        //=========================================================================
        /// <summary>
        /// 标题文本（UGUI Text）
        /// </summary>
        [SerializeField] private Text m_TitleText;

        /// <summary>
        /// 标题文本（TextMeshProUGUI）
        /// </summary>
        [SerializeField] private TextMeshProUGUI m_TitleTextTMP;

        /// <summary>
        /// 开始/更新按钮
        /// </summary>
        [SerializeField] private Button m_StartButton;

        /// <summary>
        /// 开始按钮文字（UGUI Text）
        /// </summary>
        [SerializeField] private Text m_StartButtonText;

        /// <summary>
        /// 开始按钮文字（TextMeshProUGUI）
        /// </summary>
        [SerializeField] private TextMeshProUGUI m_StartButtonTextTMP;

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
        /// 描述文本（UGUI Text）
        /// </summary>
        [SerializeField] private Text m_DescText;

        /// <summary>
        /// 描述文本（TextMeshProUGUI）
        /// </summary>
        [SerializeField] private TextMeshProUGUI m_DescTextTMP;
        #endregion

        #region 私有字段
        //=========================================================================
        // 私有字段
        //=========================================================================
        /// <summary>
        /// 是否显示关闭按钮（默认显示 = 非强制更新）
        /// </summary>
        private bool isShowCloseBtn = true;

        /// <summary>
        /// 描述内容（如：当前下载的文件名/进度信息）
        /// </summary>
        private string m_DescContent;
        #endregion

        #region 公共属性
        //=========================================================================
        // 公共属性
        //=========================================================================
        /// <summary>
        /// 设置是否显示关闭按钮
        /// </summary>
        public bool ShowCloseButton
        {
            set { m_CloseButton.gameObject.SetActive(value); }
        }

        /// <summary>
        /// 描述内容（如：当前下载的文件名/进度信息）
        /// </summary>
        public string DescContent
        {
            set => m_DescContent = value;
            get => m_DescContent;
        }
        #endregion

        #region 生命周期
        //=========================================================================
        // MonoBehaviour 生命周期
        //=========================================================================
        private void Awake()
        {
            // 初始化预留
        }

        private void Start()
        {
            // 多版本 Text 兼容，自动设置多语言文本
            if (m_TitleText != null)
                m_TitleText.text = GameMainRoot.Localization.GetDefaultData("UpdateTitle");

            if (m_TitleTextTMP != null)
                m_TitleTextTMP.text = GameMainRoot.Localization.GetDefaultData("UpdateTitle");

            if (m_DescText != null)
                m_DescText.text = GameMainRoot.Localization.GetDefaultData("UpdateText");

            if (m_DescTextTMP != null)
                m_DescTextTMP.text = GameMainRoot.Localization.GetDefaultData("UpdateText");

            if (m_StartButtonText != null)
                m_StartButtonText.text = GameMainRoot.Localization.GetDefaultData("UpdateButton");

            if (m_StartButtonTextTMP != null)
                m_StartButtonTextTMP.text = GameMainRoot.Localization.GetDefaultData("UpdateButton");

            if (m_CloseButtonText != null)
                m_CloseButtonText.text = GameMainRoot.Localization.GetDefaultData("App_Download_CloseButton_Text");

            if (m_CloseButtonTextTMP != null)
                m_CloseButtonTextTMP.text = GameMainRoot.Localization.GetDefaultData("App_Download_CloseButton_Text");
        }

        private void OnDestroy()
        {
            // 销毁预留
        }
        #endregion

        #region 按钮点击事件
        //=========================================================================
        // UI按钮点击事件
        //=========================================================================
        /// <summary>
        /// 开始按钮点击（去更新/下载）
        /// </summary>
        public void OnStartButtonClicked()
        {
            // 埋点：应用版本更新 - 点击确认
            /*Root.SDK.TGAHelper.Track("Honor_hotfix_launcher", new Dictionary<string, object>() {
                { "Honor_hotfix_step", "app_version_go" }
            });*/
        }

        /// <summary>
        /// 关闭按钮点击（取消更新）
        /// </summary>
        public void OnCloseButtonClicked()
        {
            // 埋点：应用版本更新 - 取消
            /*Root.SDK.TGAHelper.Track("Honor_hotfix_launcher", new Dictionary<string, object>() {
                { "Honor_hotfix_step", "app_version_cancel" }
            });*/

            // 关闭当前 UI
            GameMainRoot.UI.CloseUIByGO(gameObject, true);

            // 放行流程（非强制更新时）
            GameMainRoot.Event.Fire(this, GameEventCmd.FlowPermit);
        }
        #endregion

        #region 公共方法
        //=========================================================================
        // 公共业务方法
        //=========================================================================
        /// <summary>
        /// 初始化界面显示
        /// </summary>
        /// <param name="isShowCloseBtn">是否显示关闭按钮（非强制更新）</param>
        public void InitView(bool isShowCloseBtn)
        {
            this.isShowCloseBtn = isShowCloseBtn;
            ShowCloseButton = this.isShowCloseBtn;

            // 埋点：更新类型 1=强制 2=非强制
            int updateTypeInt = this.isShowCloseBtn ? 2 : 1;
            /*Root.SDK.TGAHelper.Track("Update", new Dictionary<string, object>() {
                { "update_type", updateTypeInt }
            });*/
        }
        #endregion
    }
}