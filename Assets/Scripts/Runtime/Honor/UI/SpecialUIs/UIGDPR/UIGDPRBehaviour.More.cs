/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  UIGDPRBehaviour_More.cs
 * author:    云毅
 * created:   2026
 * descrip:   GDPR隐私政策弹窗 - 了解更多详情页
 *            详细隐私说明、返回主界面、滑入滑出动画
 ***************************************************************/

using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Honor.Runtime
{
    /// <summary>
    /// GDPR 隐私政策弹窗 - 了解更多详情页
    /// 功能：展示详细隐私政策说明、返回主界面、左右滑入滑出动画
    /// </summary>
    public sealed partial class UIGDPRBehaviour : MonoBehaviour
    {
        #region 序列化UI引用字段
        //=========================================================================
        // 序列化UI引用字段
        //=========================================================================
        /// <summary>
        /// 了解更多界面画布组（动画+显隐控制）
        /// </summary>
        [SerializeField] private CanvasGroup m_MoreBg;

        /// <summary>
        /// 详情页标题（UGUI Text）
        /// </summary>
        [SerializeField] private Text m_MoreTitleText;

        /// <summary>
        /// 详情页标题（TextMeshProUGUI）
        /// </summary>
        [SerializeField] private TextMeshProUGUI m_MoreTitleTextTMP;

        /// <summary>
        /// 顶部描述文本（UGUI Text）
        /// </summary>
        [SerializeField] private Text m_MoreStartDescText;

        /// <summary>
        /// 顶部描述文本（TMP）
        /// </summary>
        [SerializeField] private TextMeshProUGUI m_MoreStartDescTextTMP;

        /// <summary>
        /// 详细描述段落1（UGUI Text）
        /// </summary>
        [SerializeField] private Text m_MoreDesc1Text;

        /// <summary>
        /// 详细描述段落1（TMP）
        /// </summary>
        [SerializeField] private TextMeshProUGUI m_MoreDesc1TextTMP;

        /// <summary>
        /// 详细描述段落2（UGUI Text）
        /// </summary>
        [SerializeField] private Text m_MoreDesc2Text;

        /// <summary>
        /// 详细描述段落2（TMP）
        /// </summary>
        [SerializeField] private TextMeshProUGUI m_MoreDesc2TextTMP;

        /// <summary>
        /// 详细描述段落3（UGUI Text）
        /// </summary>
        [SerializeField] private Text m_MoreDesc3Text;

        /// <summary>
        /// 详细描述段落3（TMP）
        /// </summary>
        [SerializeField] private TextMeshProUGUI m_MoreDesc3TextTMP;

        /// <summary>
        /// 底部总结描述（UGUI Text）
        /// </summary>
        [SerializeField] private Text m_MoreFinalDescText;

        /// <summary>
        /// 底部总结描述（TMP）
        /// </summary>
        [SerializeField] private TextMeshProUGUI m_MoreFinalDescTextTMP;

        /// <summary>
        /// 返回按钮（回到主界面）
        /// </summary>
        [SerializeField] private Button m_MoreBackButton;

        /// <summary>
        /// 返回按钮文字（UGUI Text）
        /// </summary>
        [SerializeField] private Text m_MoreBackButtonText;

        /// <summary>
        /// 返回按钮文字（TMP）
        /// </summary>
        [SerializeField] private TextMeshProUGUI m_MoreBackButtonTextTMP;
        #endregion

        #region 私有字段 & 公共属性
        //=========================================================================
        // 私有字段 & 公共属性
        //=========================================================================
        /// <summary>
        /// 返回按钮点击回调（外部注册）
        /// </summary>
        private Action m_OnBackButtonClickedCallback;

        /// <summary>
        /// 返回按钮点击回调（外部注册）
        /// </summary>
        public Action OnBackButtonClickedCallback
        {
            set => m_OnBackButtonClickedCallback = value;
            get => m_OnBackButtonClickedCallback;
        }
        #endregion

        #region 详情页初始化
        //=========================================================================
        // 详情页初始化
        //=========================================================================
        /// <summary>
        /// 初始化详情页：多语言文本、布局刷新、默认隐藏
        /// </summary>
        private void InitMoreBg()
        {
            // 标题多语言
            if (m_MoreTitleText != null)
                m_MoreTitleText.text = AorTxt.Format(GameMainRoot.Localization.GetDefaultData("GDPR_MoreTitle_Text"),
                    Application.productName);

            if (m_MoreTitleTextTMP != null)
                m_MoreTitleTextTMP.text = AorTxt.Format(GameMainRoot.Localization.GetDefaultData("GDPR_MoreTitle_Text"),
                    Application.productName);

            // 顶部说明
            if (m_MoreStartDescText != null)
                m_MoreStartDescText.text =
                    AorTxt.Format(GameMainRoot.Localization.GetDefaultData("GDPR_MoreStartDesc_Text"));

            if (m_MoreStartDescTextTMP != null)
                m_MoreStartDescTextTMP.text =
                    AorTxt.Format(GameMainRoot.Localization.GetDefaultData("GDPR_MoreStartDesc_Text"));

            // 段落1
            if (m_MoreDesc1Text != null)
                m_MoreDesc1Text.text = AorTxt.Format(GameMainRoot.Localization.GetDefaultData("GDPR_MoreDesc1_Text"));

            if (m_MoreDesc1TextTMP != null)
                m_MoreDesc1TextTMP.text =
                    AorTxt.Format(GameMainRoot.Localization.GetDefaultData("GDPR_MoreDesc1_Text"));

            // 段落2
            if (m_MoreDesc2Text != null)
                m_MoreDesc2Text.text = AorTxt.Format(GameMainRoot.Localization.GetDefaultData("GDPR_MoreDesc2_Text"));

            if (m_MoreDesc2TextTMP != null)
                m_MoreDesc2TextTMP.text =
                    AorTxt.Format(GameMainRoot.Localization.GetDefaultData("GDPR_MoreDesc2_Text"));

            // 段落3
            if (m_MoreDesc3Text != null)
                m_MoreDesc3Text.text = AorTxt.Format(GameMainRoot.Localization.GetDefaultData("GDPR_MoreDesc3_Text"));

            if (m_MoreDesc3TextTMP != null)
                m_MoreDesc3TextTMP.text =
                    AorTxt.Format(GameMainRoot.Localization.GetDefaultData("GDPR_MoreDesc3_Text"));

            // 底部总结
            if (m_MoreFinalDescText != null)
                m_MoreFinalDescText.text =
                    AorTxt.Format(GameMainRoot.Localization.GetDefaultData("GDPR_MoreFinalDesc_Text"));

            if (m_MoreFinalDescTextTMP != null)
                m_MoreFinalDescTextTMP.text =
                    AorTxt.Format(GameMainRoot.Localization.GetDefaultData("GDPR_MoreFinalDesc_Text"));

            // 返回按钮
            if (m_MoreBackButtonText != null)
                m_MoreBackButtonText.text =
                    AorTxt.Format(GameMainRoot.Localization.GetDefaultData("GDPR_MoreBackButton_Text"));

            if (m_MoreBackButtonTextTMP != null)
                m_MoreBackButtonTextTMP.text =
                    AorTxt.Format(GameMainRoot.Localization.GetDefaultData("GDPR_MoreBackButton_Text"));

            // 强制刷新布局并默认隐藏
            LayoutRebuilder.ForceRebuildLayoutImmediate(m_MoreBg.rectTransform());
            m_MoreBg.gameObject.SetActive(false);
        }
        #endregion

        #region UI交互事件
        //=========================================================================
        // UI交互事件
        //=========================================================================
        /// <summary>
        /// 返回按钮点击：回到主界面
        /// </summary>
        public void OnMoreBackButtonClicked()
        {
            SetMainBgAnimationVisible(true);
            SetMoreBgAnimationVisible(false);
        }
        #endregion

        #region 界面动画控制
        //=========================================================================
        // 界面动画控制
        //=========================================================================
        /// <summary>
        /// 详情页显隐动画：右侧滑入/滑出 + 淡入淡出
        /// </summary>
        public void SetMoreBgAnimationVisible(bool visible)
        {
            m_TopMaskLayer.raycastTarget = true;

            if (visible)
            {
                // 显示：从右侧滑入 + 淡入
                if (m_MoreBg != null)
                {
                    m_MoreBg.gameObject.SetActive(true);
                    m_MoreBg.alpha = 0f;
                    m_MoreBg.transform.rectTransform().anchoredPosition = new Vector2(800, 0);

                    DOTween.Sequence()
                        .Insert(0, DOTween.To(() => m_MoreBg.alpha, a => m_MoreBg.alpha = a, 1f, 1f))
                        .Insert(0,
                            DOTween.To(() => m_MoreBg.transform.rectTransform().anchoredPosition,
                                pos => m_MoreBg.transform.rectTransform().anchoredPosition = pos, Vector2.zero, 1f))
                        .AppendCallback(() => { m_TopMaskLayer.raycastTarget = false; });
                }
            }
            else
            {
                // 隐藏：向右侧滑出 + 淡出
                if (m_MoreBg != null)
                {
                    m_MoreBg.gameObject.SetActive(true);
                    m_MoreBg.alpha = 1f;
                    m_MoreBg.transform.rectTransform().anchoredPosition = Vector2.zero;

                    DOTween.Sequence()
                        .Insert(0, DOTween.To(() => m_MoreBg.alpha, a => m_MoreBg.alpha = a, 0f, 1f))
                        .Insert(0,
                            DOTween.To(() => m_MoreBg.transform.rectTransform().anchoredPosition,
                                pos => m_MoreBg.transform.rectTransform().anchoredPosition = pos,
                                m_MoreBg.transform.rectTransform().anchoredPosition + new Vector2(800, 0), 1f))
                        .AppendCallback(() =>
                        {
                            m_MoreBg.gameObject.SetActive(false);
                            m_TopMaskLayer.raycastTarget = false;
                        });
                }
            }
        }
        #endregion
    }
}