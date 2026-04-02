using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameLib
{
    public class HonorClickItemAnimation : AorClickItem
    {
        [SerializeField] protected Vector3 scale = Vector3.one * 0.95f;
        [SerializeField] protected Ease scaleEase = Ease.Linear;

        [SerializeField] protected float scaleDuration = 0.1f;

        private void OnEnable()
        {
            transform.localScale = Vector3.one;
        }

        private void OnDestroy()
        {
            DOTween.Kill(transform);
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            if (!canInteraction) return;
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            if (!canInteraction) return;
            transform.DOScale(scale, scaleDuration).SetEase(scaleEase).SetUpdate(true);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            if (!canInteraction) return;
            transform.DOScale(Vector3.one, scaleDuration).SetEase(scaleEase).SetUpdate(true);
        }
    }
}