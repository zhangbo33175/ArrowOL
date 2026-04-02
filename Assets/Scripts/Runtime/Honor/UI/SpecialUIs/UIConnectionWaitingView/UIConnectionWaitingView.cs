using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Honor.Runtime
{
    public sealed class UIConnectionWaitingView : MonoBehaviour
    {
        /// <summary>
        /// 菊花图片
        /// </summary>
        [SerializeField]
        private Image m_FlowerImage;

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
        /// 顶层遮罩层
        /// </summary>
        [SerializeField]
        private Image m_TopMaskLayer;

        private void Awake()
        {
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

            if (m_DescText != null)
            {
                m_DescText.text = GameMainRoot.Localization.GetDefaultData("Waiting_Desc_Text");
            }

            if (m_DescTextTMP != null)
            {
                m_DescTextTMP.text = GameMainRoot.Localization.GetDefaultData("Waiting_Desc_Text");
            }

            if (m_FlowerImage != null)
            {
                m_FlowerImage.transform.DORotate(new Vector3(0, 0, -360f), 8, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1).SetUpdate(true);
            }

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

        /// <summary>
        /// 可见性设置
        /// </summary>
        public void SetVisible(bool visible)
        {
            gameObject.SetActive(visible);
        }

        /// <summary>
        /// 获取可见性
        /// </summary>
        public bool IsVisible()
        {
            return gameObject.activeSelf;
        }

        /// <summary>
        /// 设置等待界面显示文本
        /// </summary>
        /// <param name="text"></param>
        public void SetWaitingDescText(string text)
        {
            if (m_DescText != null)
            {
                m_DescText.text = text;
            }
            if (m_DescTextTMP != null)
            {
                m_DescTextTMP.text = text;
            }
        }

    }

}


