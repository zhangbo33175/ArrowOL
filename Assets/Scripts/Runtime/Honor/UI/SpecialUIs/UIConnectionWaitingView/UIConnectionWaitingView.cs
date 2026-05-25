/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  UIConnectionWaitingView.cs
 * author:    云毅
 * created:   2026
 * descrip:   网络连接等待/加载中弹窗UI组件
 *            显示旋转加载动画、提示文本、顶层遮罩防点击，兼容UGUI/TMP
 ***************************************************************/

using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Honor.Runtime
{
    /// <summary>
    /// 网络连接等待 / 加载中 弹窗 UI 行为脚本
    /// 功能：显示加载动画（菊花旋转）、等待提示文本、顶层遮罩防止点击
    /// 兼容 UGUI Text / TextMeshProUGUI
    /// </summary>
    public sealed class UIConnectionWaitingView : MonoBehaviour
    {
        #region 序列化UI引用字段
        //=========================================================================
        // 序列化UI引用字段
        //=========================================================================
        /// <summary>
        /// 旋转加载动画（菊花图）
        /// </summary>
        [SerializeField] private Image m_FlowerImage;

        /// <summary>
        /// 描述文本（UGUI Text）
        /// </summary>
        [SerializeField] private Text m_DescText;

        /// <summary>
        /// 描述文本（TextMeshProUGUI）
        /// </summary>
        [SerializeField] private TextMeshProUGUI m_DescTextTMP;

        /// <summary>
        /// 顶层遮罩（阻挡点击）
        /// </summary>
        [SerializeField] private Image m_TopMaskLayer;
        #endregion

        #region 生命周期
        //=========================================================================
        // MonoBehaviour 生命周期
        //=========================================================================
        private void Awake()
        {
            // 组件空值校验
            if (m_FlowerImage == null)
            {
                Log.Fatal("UIWaitingBehaviour FlowerImage 无效。");
                return;
            }

            if (m_TopMaskLayer == null)
            {
                Log.Fatal("UIWaitingBehaviour TopMaskLayer 无效。");
                return;
            }

            // 设置多语言默认提示文本
            if (m_DescText != null)
            {
                m_DescText.text = GameMainRoot.Localization.GetDefaultData("Waiting_Desc_Text");
            }

            if (m_DescTextTMP != null)
            {
                m_DescTextTMP.text = GameMainRoot.Localization.GetDefaultData("Waiting_Desc_Text");
            }

            // 播放加载动画：无限循环旋转（-360° / 8秒 / 线性匀速）
            if (m_FlowerImage != null)
            {
                m_FlowerImage.transform.DORotate(new Vector3(0, 0, -360f), 8, RotateMode.FastBeyond360)
                    .SetEase(Ease.Linear)
                    .SetLoops(-1)
                    .SetUpdate(true);
            }

            // 开启遮罩射线阻挡，防止点击穿透
            if (m_TopMaskLayer != null)
            {
                m_TopMaskLayer.raycastTarget = true;
            }
        }

        private void Start()
        {
        }

        private void OnDestroy()
        {
        }
        #endregion

        #region 公共控制方法
        //=========================================================================
        // 公共控制方法
        //=========================================================================
        /// <summary>
        /// 设置界面可见性
        /// </summary>
        /// <param name="visible">是否显示</param>
        public void SetVisible(bool visible)
        {
            gameObject.SetActive(visible);
        }

        /// <summary>
        /// 获取当前是否可见
        /// </summary>
        /// <returns>当前可见状态</returns>
        public bool IsVisible()
        {
            return gameObject.activeSelf;
        }

        /// <summary>
        /// 动态设置等待提示文本
        /// </summary>
        /// <param name="text">提示文本内容</param>
        public void SetWaitingDescText(string text)
        {
            if (m_DescText != null)
                m_DescText.text = text;

            if (m_DescTextTMP != null)
                m_DescTextTMP.text = text;
        }
        #endregion
    }
}