/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  UIFloatWordsBehaviour.cs
 * author:    云毅
 * created:   2026
 * descrip:   浮动提示文本（飘字）UI组件
 *            淡入上移淡出动画，支持Text/TMP，可阻塞点击、自定义时长、回调
 ***************************************************************/

using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Honor.Runtime
{
    /// <summary>
    /// 浮动提示文本（飘字）UI 行为脚本
    /// 功能：显示短时提示文字，自带淡入、上移、淡出动画
    /// 支持 Text / TextMeshProUGUI，可阻塞点击、自定义时长、结束回调
    /// </summary>
    public sealed class UIFloatWordsBehaviour : MonoBehaviour
    {
        #region 序列化UI引用字段
        //=========================================================================
        // 序列化UI引用字段
        //=========================================================================
        /// <summary>
        /// 提示文字组件（UGUI Text）
        /// </summary>
        [SerializeField] private Text m_WordsText;

        /// <summary>
        /// 提示文字组件（TextMeshProUGUI）
        /// </summary>
        [SerializeField] private TextMeshProUGUI m_WordsTextTMP;

        /// <summary>
        /// 底部背景遮罩
        /// </summary>
        [SerializeField] private Image m_BottomMaskLayer;

        /// <summary>
        /// 顶部点击遮罩（用于阻塞触摸）
        /// </summary>
        [SerializeField] private Image m_TopMaskLayer;
        #endregion

        #region 公共属性
        //=========================================================================
        // 公共属性
        //=========================================================================
        /// <summary>
        /// 提示文字组件（UGUI Text）
        /// </summary>
        public Text WordsText
        {
            get => m_WordsText;
        }

        /// <summary>
        /// 提示文字组件（TextMeshProUGUI）
        /// </summary>
        public TextMeshProUGUI WordsTextTMP
        {
            get => m_WordsTextTMP;
        }

        /// <summary>
        /// 底部背景遮罩
        /// </summary>
        public Image BottomMaskLayer
        {
            get => m_BottomMaskLayer;
        }

        /// <summary>
        /// 顶部点击遮罩
        /// </summary>
        public Image TopMaskLayer
        {
            get => m_TopMaskLayer;
        }

        /// <summary>
        /// 提示持续时间（动画完成后等待多久）
        /// </summary>
        public float Duration
        {
            set => m_Duration = value;
            get => m_Duration;
        }

        /// <summary>
        /// 是否阻塞 UI 触摸事件
        /// </summary>
        public bool BlockUITouches
        {
            set => m_BlockUITouches = value;
        }

        /// <summary>
        /// 动画结束回调
        /// </summary>
        public Action OverCallback
        {
            set => m_OverCallback = value;
        }
        #endregion

        #region 私有字段
        //=========================================================================
        // 私有字段
        //=========================================================================
        /// <summary>
        /// 提示持续时间
        /// </summary>
        private float m_Duration;

        /// <summary>
        /// 是否阻塞UI触摸
        /// </summary>
        private bool m_BlockUITouches;

        /// <summary>
        /// 结束回调
        /// </summary>
        private Action m_OverCallback;
        #endregion

        #region 生命周期
        //=========================================================================
        // MonoBehaviour 生命周期
        //=========================================================================
        private void Awake()
        {
            // 必须至少有一个文本组件
            if (m_WordsText == null && m_WordsTextTMP == null)
            {
                Log.Fatal("UIFloatWordsBehaviour WordsText 无效。");
                return;
            }

            // 持续时间不能为负
            if (m_Duration < 0)
            {
                Log.Fatal("UIFloatWordsBehaviour Duration 无效。");
                return;
            }
        }

        private void Start()
        {
            // 根据配置决定是否开启遮罩阻挡点击
            m_TopMaskLayer.gameObject.SetActive(m_BlockUITouches);
            m_TopMaskLayer.raycastTarget = m_BlockUITouches;

            // 淡入淡出动画时长
            float fadeDuration = 0.5f;

            // 播放 UGUI Text 动画
            if (m_WordsText != null && m_WordsText.enabled)
            {
                // 初始化：透明 + 下移
                m_WordsText.color = new Color(m_WordsText.color.r, m_WordsText.color.g, m_WordsText.color.b, 0f);
                m_WordsText.transform.rectTransform().anchoredPosition = new Vector2(0f, -60f);

                // 淡入 + 上移
                DOTween.Sequence()
                    .Append(m_WordsText.DOColor(
                        new Color(m_WordsText.color.r, m_WordsText.color.g, m_WordsText.color.b, 1f), fadeDuration))
                    .Insert(0, m_WordsText.transform.rectTransform().DOLocalMoveY(0, fadeDuration));

                // 等待 + 淡出 + 关闭
                DOTween.Sequence()
                    .AppendInterval(fadeDuration + m_Duration)
                    .Append(m_WordsText.DOColor(
                        new Color(m_WordsText.color.r, m_WordsText.color.g, m_WordsText.color.b, 0f), fadeDuration))
                    .AppendCallback(() =>
                    {
                        m_OverCallback?.Invoke();
                        GameMainRoot.UI.CloseUIByGO(gameObject, true);
                    });
            }
            // 播放 TMP 动画
            else if (m_WordsTextTMP != null && m_WordsTextTMP.enabled)
            {
                // 初始化：透明 + 下移
                m_WordsTextTMP.color = new Color(m_WordsTextTMP.color.r, m_WordsTextTMP.color.g, m_WordsTextTMP.color.b,
                    0f);
                m_WordsTextTMP.transform.rectTransform().anchoredPosition = new Vector2(0f, -60f);

                // 淡入 + 上移
                DOTween.Sequence()
                    .Append(m_WordsTextTMP.DOColor(
                        new Color(m_WordsTextTMP.color.r, m_WordsTextTMP.color.g, m_WordsTextTMP.color.b, 1f),
                        fadeDuration))
                    .Insert(0, m_WordsTextTMP.transform.rectTransform().DOLocalMoveY(0, fadeDuration))
                    .SetUpdate(true);

                // 等待 + 淡出 + 关闭
                DOTween.Sequence()
                    .AppendInterval(fadeDuration + m_Duration)
                    .Append(m_WordsTextTMP.DOColor(
                        new Color(m_WordsTextTMP.color.r, m_WordsTextTMP.color.g, m_WordsTextTMP.color.b, 0f),
                        fadeDuration))
                    .AppendCallback(() =>
                    {
                        m_OverCallback?.Invoke();
                        GameMainRoot.UI.CloseUIByGO(gameObject, true);
                    })
                    .SetUpdate(true);
            }
        }

        private void OnDestroy()
        {
        }
        #endregion
    }
}