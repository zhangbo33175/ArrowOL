using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameLib
{
    /// <summary>
    /// 荣誉按钮点击动画（缩放效果）
    /// 继承通用点击组件，实现按下缩小、抬起还原的动画效果
    /// </summary>
    public class HonorClickItemAnimation : AorClickItem
    {
        /// <summary>
        /// 按下时的目标缩放值
        /// </summary>
        [SerializeField] protected Vector3 scale = Vector3.one * 0.95f;

        /// <summary>
        /// 缩放动画曲线
        /// </summary>
        [SerializeField] protected Ease scaleEase = Ease.Linear;

        /// <summary>
        /// 缩放动画时长
        /// </summary>
        [SerializeField] protected float scaleDuration = 0.1f;

        /// <summary>
        /// 启用时重置缩放为原始大小
        /// </summary>
        private void OnEnable()
        {
            transform.localScale = Vector3.one;
        }

        /// <summary>
        /// 销毁时杀死当前物体的所有动画，防止内存泄漏
        /// </summary>
        private void OnDestroy()
        {
            DOTween.Kill(transform);
        }

        /// <summary>
        /// 重写点击事件
        /// </summary>
        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            if (!canInteraction) return;
        }

        /// <summary>
        /// 重写按下事件：播放缩小动画
        /// </summary>
        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            if (!canInteraction) return;

            // 播放缩小动画，不受游戏暂停影响
            transform.DOScale(scale, scaleDuration).SetEase(scaleEase).SetUpdate(true);
        }

        /// <summary>
        /// 重写抬起事件：播放还原动画
        /// </summary>
        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            if (!canInteraction) return;

            // 播放还原动画，不受游戏暂停影响
            transform.DOScale(Vector3.one, scaleDuration).SetEase(scaleEase).SetUpdate(true);
        }
    }
}