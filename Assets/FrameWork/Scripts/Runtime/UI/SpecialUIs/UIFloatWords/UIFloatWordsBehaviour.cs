using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Honor.Runtime
{
    public sealed class UIFloatWordsBehaviour : MonoBehaviour
    {
        /// <summary>
        /// 文字组件
        /// </summary>
        [SerializeField] private Text m_WordsText;

        public Text WordsText
        {
            get { return m_WordsText; }
        }

        /// <summary>
        /// 文字组件
        /// </summary>
        [SerializeField] private TextMeshProUGUI m_WordsTextTMP;

        public TextMeshProUGUI WordsTextTMP
        {
            get { return m_WordsTextTMP; }
        }

        /// <summary>
        /// Bottom遮罩层
        /// </summary>
        [SerializeField] private Image m_BottomMaskLayer;

        public Image BottomMaskLayer
        {
            get { return m_BottomMaskLayer; }
        }

        /// <summary>
        /// Top遮罩层
        /// </summary>
        [SerializeField] private Image m_TopMaskLayer;

        public Image TopMaskLayer
        {
            get { return m_TopMaskLayer; }
        }

        /// <summary>
        /// 持续时间
        /// </summary>
        private float m_Duration;

        public float Duration
        {
            set { m_Duration = value; }
            get { return m_Duration; }
        }

        /// <summary>
        /// 是否阻塞UI触摸
        /// </summary>
        private bool m_BlockUITouches;

        public bool BlockUITouches
        {
            set { m_BlockUITouches = value; }
        }

        /// <summary>
        /// 回调函数
        /// </summary>
        private Action m_OverCallback;

        public Action OverCallback
        {
            set { m_OverCallback = value; }
        }

        private void Awake()
        {
            if (m_WordsText == null && m_WordsTextTMP == null)
            {
                Log.Fatal("UIFloatWordsBehaviour WordsText 无效。");
                return;
            }

            if (m_Duration < 0)
            {
                Log.Fatal("UIFloatWordsBehaviour Duration 无效。");
                return;
            }
        }

        private void Start()
        {
            // 是否需要UI触摸屏蔽
            m_TopMaskLayer.gameObject.SetActive(m_BlockUITouches);
            m_TopMaskLayer.raycastTarget = m_BlockUITouches;

            // 动画
            float fadeDuration = 0.5f;
            if (m_WordsText && m_WordsText.enabled)
            {
                m_WordsText.color = new Color(m_WordsText.color.r, m_WordsText.color.g, m_WordsText.color.b, 0f);
                m_WordsText.transform.rectTransform().anchoredPosition = new Vector2(0f, -60f);
                DOTween.Sequence().Append(m_WordsText.DOColor(
                        new Color(m_WordsText.color.r, m_WordsText.color.g, m_WordsText.color.b, 1f), fadeDuration))
                    .Insert(0, m_WordsText.transform.rectTransform().DOLocalMoveY(0, fadeDuration));
                DOTween.Sequence().AppendInterval(fadeDuration + m_Duration)
                    .Append(m_WordsText.DOColor(
                        new Color(m_WordsText.color.r, m_WordsText.color.g, m_WordsText.color.b, 0f), fadeDuration))
                    .AppendCallback(() =>
                    {
                        if (m_OverCallback != null)
                        {
                            m_OverCallback();
                        }

                        UIManager.Instance.CloseUIByGO(gameObject, true);
                    });
            }
            else if (m_WordsTextTMP && m_WordsTextTMP.enabled)
            {
                m_WordsTextTMP.color = new Color(m_WordsTextTMP.color.r, m_WordsTextTMP.color.g, m_WordsTextTMP.color.b,
                    0f);
                m_WordsTextTMP.transform.rectTransform().anchoredPosition = new Vector2(0f, -60f);
                DOTween.Sequence().Append(m_WordsTextTMP.DOColor(
                        new Color(m_WordsTextTMP.color.r, m_WordsTextTMP.color.g, m_WordsTextTMP.color.b, 1f),
                        fadeDuration))
                    .Insert(0, m_WordsTextTMP.transform.rectTransform().DOLocalMoveY(0, fadeDuration))
                    .SetUpdate(true);
                DOTween.Sequence().AppendInterval(fadeDuration + m_Duration)
                    .Append(m_WordsTextTMP.DOColor(
                        new Color(m_WordsTextTMP.color.r, m_WordsTextTMP.color.g, m_WordsTextTMP.color.b, 0f),
                        fadeDuration)).AppendCallback(() =>
                    {
                        if (m_OverCallback != null)
                        {
                            m_OverCallback();
                        }

                        UIManager.Instance.CloseUIByGO(gameObject, true);
                    })
                    .SetUpdate(true);
            }
        }

        private void OnDestroy()
        {
        }
    }
}