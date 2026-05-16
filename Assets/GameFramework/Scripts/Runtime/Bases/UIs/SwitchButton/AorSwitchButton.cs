/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  AorSwitchButton.cs
 * author:    云毅
 * created:   2026   2025
 * descrip:   自定义开关切换按钮 | 基于UGUI Toggle扩展 | 无动画 | 支持Text/TMP
 ***************************************************************/

using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Honor.Runtime
{
    /// <summary>
    /// 自定义开关切换按钮 (基于UGUI Toggle扩展)
    /// 具备固定样式，不实现开关切换动画效果，不处理Navigation导航排版逻辑
    /// 同时支持UGUI文本与TextMeshPro文本显示
    /// </summary>
    public class AorSwitchButton : Toggle
    {
        //=========================================================================
        // 字段成员
        //=========================================================================
        #region Field - Text Label
        /// <summary>
        /// UGUI 文本标签，用于显示开关文字
        /// </summary>
        [SerializeField]
        private Text m_Label;
        #endregion

        #region Field - TMP Text Label
        /// <summary>
        /// TextMeshPro 文本标签，用于显示开关文字
        /// </summary>
        [SerializeField]
        private TextMeshProUGUI m_TMPLabel;
        #endregion

        //=========================================================================
        // 属性成员
        //=========================================================================
        #region Property - UGUI Text Label
        /// <summary>
        /// 外部访问 UGUI 文本标签
        /// </summary>
        public Text Label
        {
            get => m_Label;
            set => m_Label = value;
        }
        #endregion

        #region Property - TMP Text Label
        /// <summary>
        /// 外部访问 TextMeshPro 文本标签
        /// </summary>
        public TextMeshProUGUI TMPLabel
        {
            get => m_TMPLabel;
            set => m_TMPLabel = value;
        }
        #endregion

        //=========================================================================
        // 生命周期方法
        //=========================================================================
        #region MonoBehaviour - Awake
        /// <summary>
        /// 重写 Awake 方法，初始化开关基础配置
        /// </summary>
        protected override void Awake()
        {
            // 调用父类 Awake 逻辑
            base.Awake();

            // 禁用 UGUI 默认的状态过渡动画（颜色/缩放/精灵切换）
            transition = Transition.None;

            // 禁用 Toggle 自带的切换过渡效果
            toggleTransition = ToggleTransition.None;
        }
        #endregion
    }
}