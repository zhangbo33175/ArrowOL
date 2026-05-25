/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  UIGDPRBehaviour_Reset.cs
 * author:    云毅
 * created:   2026
 * descrip:   GDPR隐私政策弹窗 - 重置/设置界面
 *            游戏内重新修改隐私授权、保存设置、关闭界面
 ***************************************************************/

using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Honor.Runtime
{
    /// <summary>
    /// GDPR 隐私政策弹窗 - 重置/设置界面
    /// 功能：游戏内重新修改隐私授权、保存设置、关闭界面
    /// </summary>
    public sealed partial class UIGDPRBehaviour : MonoBehaviour
    {
        #region 序列化UI引用字段
        //=========================================================================
        // 序列化UI引用字段
        //=========================================================================
        /// <summary>
        /// 重置界面画布组（动画+显隐控制）
        /// </summary>
        [SerializeField] private CanvasGroup m_ResetBg;

        /// <summary>
        /// 重置页标题（UGUI Text）
        /// </summary>
        [SerializeField] private Text m_ResetTitleText;

        /// <summary>
        /// 重置页标题（TextMeshProUGUI）
        /// </summary>
        [SerializeField] private TextMeshProUGUI m_ResetTitleTextTMP;

        /// <summary>
        /// 重置页描述文本（UGUI Text）
        /// </summary>
        [SerializeField] private Text m_ResetDescText;

        /// <summary>
        /// 重置页描述文本（TMP）
        /// </summary>
        [SerializeField] private TextMeshProUGUI m_ResetDescTextTMP;

        /// <summary>
        /// 重置页 GDPR 授权开关
        /// </summary>
        [SerializeField] private Toggle m_ResetGDPRToggle;

        /// <summary>
        /// 重置页 CCPA 授权开关
        /// </summary>
        [SerializeField] private Toggle m_ResetCCPAToggle;

        /// <summary>
        /// 重置页 GDPR 说明文字（UGUI Text）
        /// </summary>
        [SerializeField] private Text m_ResetGDPRText;

        /// <summary>
        /// 重置页 GDPR 说明文字（TMP）
        /// </summary>
        [SerializeField] private TextMeshProUGUI m_ResetGDPRTextTMP;

        /// <summary>
        /// 重置页 CCPA 说明文字（UGUI Text）
        /// </summary>
        [SerializeField] private Text m_ResetCCPAText;

        /// <summary>
        /// 重置页 CCPA 说明文字（TMP）
        /// </summary>
        [SerializeField] private TextMeshProUGUI m_ResetCCPATextTMP;

        /// <summary>
        /// 重置页 GDPR 隐私政策链接按钮
        /// </summary>
        [SerializeField] private Button m_ResetGDPRLinkButton;

        /// <summary>
        /// 重置页 CCPA 隐私政策链接按钮
        /// </summary>
        [SerializeField] private Button m_ResetCCPALinkButton;

        /// <summary>
        /// 保存设置按钮
        /// </summary>
        [SerializeField] private Button m_SaveButton;

        /// <summary>
        /// 保存按钮文字（UGUI Text）
        /// </summary>
        [SerializeField] private Text m_SaveButtonText;

        /// <summary>
        /// 保存按钮文字（TMP）
        /// </summary>
        [SerializeField] private TextMeshProUGUI m_SaveButtonTextTMP;

        /// <summary>
        /// 关闭按钮
        /// </summary>
        [SerializeField] private Button m_CloseButton;

        /// <summary>
        /// 关闭按钮文字（UGUI Text）
        /// </summary>
        [SerializeField] private Text m_CloseButtonText;

        /// <summary>
        /// 关闭按钮文字（TMP）
        /// </summary>
        [SerializeField] private TextMeshProUGUI m_CloseButtonTextTMP;
        #endregion

        #region 重置界面初始化
        //=========================================================================
        // 重置界面初始化
        //=========================================================================
        /// <summary>
        /// 初始化重置界面：多语言、开关状态、布局刷新
        /// </summary>
        private void InitResetBg()
        {
            // 标题多语言
            if (m_ResetTitleText != null)
                m_ResetTitleText.text = AorTxt.Format(GameMainRoot.Localization.GetDefaultData("GDPR_ResetTitle_Text"),
                    Application.productName);

            if (m_ResetTitleTextTMP != null)
                m_ResetTitleTextTMP.text =
                    AorTxt.Format(GameMainRoot.Localization.GetDefaultData("GDPR_ResetTitle_Text"),
                        Application.productName);

            // 描述文本
            if (m_ResetDescText != null)
                m_ResetDescText.text = AorTxt.Format(GameMainRoot.Localization.GetDefaultData("GDPR_ResetDesc_Text"));

            if (m_ResetDescTextTMP != null)
                m_ResetDescTextTMP.text =
                    AorTxt.Format(GameMainRoot.Localization.GetDefaultData("GDPR_ResetDesc_Text"));

            // GDPR 说明文字
            if (m_ResetGDPRText != null)
                m_ResetGDPRText.text =
                    AorTxt.Format(GameMainRoot.Localization.GetDefaultData("GDPR_ResetGDPRDesc_Text"));

            if (m_ResetGDPRTextTMP != null)
                m_ResetGDPRTextTMP.text =
                    AorTxt.Format(GameMainRoot.Localization.GetDefaultData("GDPR_ResetGDPRDesc_Text"));

            // CCPA 说明文字
            if (m_ResetCCPAText != null)
                m_ResetCCPAText.text =
                    AorTxt.Format(GameMainRoot.Localization.GetDefaultData("GDPR_ResetCCPADesc_Text"));

            if (m_ResetCCPATextTMP != null)
                m_ResetCCPATextTMP.text =
                    AorTxt.Format(GameMainRoot.Localization.GetDefaultData("GDPR_ResetCCPADesc_Text"));

            // 保存按钮文字
            if (m_SaveButtonText != null)
                m_SaveButtonText.text = AorTxt.Format(GameMainRoot.Localization.GetDefaultData("GDPR_SaveButton_Text"));

            if (m_SaveButtonTextTMP != null)
                m_SaveButtonTextTMP.text =
                    AorTxt.Format(GameMainRoot.Localization.GetDefaultData("GDPR_SaveButton_Text"));

            // 关闭按钮文字
            if (m_CloseButtonText != null)
                m_CloseButtonText.text =
                    AorTxt.Format(GameMainRoot.Localization.GetDefaultData("GDPR_CloseButton_Text"));

            if (m_CloseButtonTextTMP != null)
                m_CloseButtonTextTMP.text =
                    AorTxt.Format(GameMainRoot.Localization.GetDefaultData("GDPR_CloseButton_Text"));

            // 从持久化数据读取开关状态
            if (m_ResetGDPRToggle != null)
            {
                m_ResetGDPRToggle.isOn = m_PersistComponent.GetBool(GameConstants.Persist.GDPR.WayType,
                    GameConstants.Persist.GDPR.ClassifyName, GameConstants.Persist.GDPR.ItemKey.HasUserConsent, true);
            }

            if (m_ResetCCPAToggle != null)
            {
                m_ResetCCPAToggle.isOn = m_PersistComponent.GetBool(GameConstants.Persist.GDPR.WayType,
                    GameConstants.Persist.GDPR.ClassifyName, GameConstants.Persist.GDPR.ItemKey.IsSell, true);
            }

            // 强制刷新布局
            LayoutRebuilder.ForceRebuildLayoutImmediate(m_ResetBg.rectTransform());
            // 游戏内打开时显示重置界面，否则隐藏
            m_ResetBg.gameObject.SetActive(InGame);
            // 游戏内界面重置位置
            if (InGame)
            {
                m_ResetBg.rectTransform().anchoredPosition = Vector2.zero;
            }
        }
        #endregion

        #region UI交互事件
        //=========================================================================
        // UI交互事件
        //=========================================================================
        /// <summary>
        /// 打开 GDPR 隐私政策链接
        /// </summary>
        public void OnResetGDPRLinkButtonClicked()
        {
            Application.OpenURL(GameMainRoot.Config.GetString("GDPRPrivacyUrl", true));
        }

        /// <summary>
        /// 打开 CCPA 隐私政策链接
        /// </summary>
        public void OnResetCCPALinkButtonClicked()
        {
            Application.OpenURL(GameMainRoot.Config.GetString("CCPAPrivacyUrl", true));
        }

        /// <summary>
        /// 保存按钮：保存授权设置 → 关闭界面 → 执行回调
        /// </summary>
        public void OnSaveButtonClicked()
        {
            // 保存 GDPR 授权
            if (m_ResetGDPRToggle != null)
            {
                m_PersistComponent.SetBool(GameConstants.Persist.GDPR.WayType, GameConstants.Persist.GDPR.ClassifyName,
                    GameConstants.Persist.GDPR.ItemKey.HasUserConsent, m_ResetGDPRToggle.isOn);
            }

            // 保存 CCPA 授权
            if (m_ResetCCPAToggle != null)
            {
                m_PersistComponent.SetBool(GameConstants.Persist.GDPR.WayType, GameConstants.Persist.GDPR.ClassifyName,
                    GameConstants.Persist.GDPR.ItemKey.IsSell, m_ResetCCPAToggle.isOn);
            }

            // 持久化保存
            m_PersistComponent.Save(GameConstants.Persist.GDPR.WayType);

            // 关闭界面
            GameMainRoot.UI.CloseUIByGO(gameObject, true);

            // 执行外部完成回调
            if (m_OnOverButtonClickedCallback != null)
            {
                m_OnOverButtonClickedCallback();
            }
        }

        /// <summary>
        /// 关闭按钮：直接关闭界面 → 执行回调
        /// </summary>
        public void OnCloseButtonClicked()
        {
            GameMainRoot.UI.CloseUIByGO(gameObject, true);

            if (m_OnOverButtonClickedCallback != null)
            {
                m_OnOverButtonClickedCallback();
            }
        }
        #endregion
    }
}