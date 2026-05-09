using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameLib
{
    /// <summary>
    /// 通用点击交互组件（支持单击、长按、连续长按、选中/取消选中）
    /// 可挂载在任意UI上实现高级点击逻辑
    /// </summary>
    public class AorClickItem : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler,
        ISelectHandler, IDeselectHandler
    {
        /// <summary>
        /// 单击回调
        /// </summary>
        public Action onClick;

        /// <summary>
        /// 长按回调
        /// </summary>
        public Action onLongClick;

        /// <summary>
        /// 指针按下回调
        /// </summary>
        public Action onPointerDown;

        /// <summary>
        /// 指针抬起回调
        /// </summary>
        public Action onPointerUp;

        /// <summary>
        /// 选中回调
        /// </summary>
        public Action onSelect;

        /// <summary>
        /// 取消选中回调
        /// </summary>
        public Action onDeselect;

        [Header("基础设置")]
        /// <summary>
        /// 点击间隔（防连点）
        /// </summary>
        public float clickInterval = 0;

        /// <summary>
        /// 是否可交互
        /// </summary>
        public bool canInteraction = true;

        [Header("长按设置")]
        /// <summary>
        /// 是否持续触发长按（按住一直触发）
        /// </summary>
        [SerializeField]
        private bool isContinuousTriggerLongClick = false;

        /// <summary>
        /// 上次点击时间（用于间隔判断）
        /// </summary>
        private float lastClickTimeTemp;

        /// <summary>
        /// 长按触发阈值（默认0.3s）
        /// </summary>
        private float longClickTriggerTime = 0.3f;

        /// <summary>
        /// 连续长按触发间隔
        /// </summary>
        private float longClickTriggerInterval = 0.1f;

        /// <summary>
        /// 是否正在按住
        /// </summary>
        private bool isPointerDown;

        /// <summary>
        /// 按住累计时间
        /// </summary>
        private float tempPointerDownTime;

        /// <summary>
        /// 是否已触发过长按
        /// </summary>
        private bool isTriggerLongClick;

        /// <summary>
        /// 禁用时重置状态
        /// </summary>
        public void OnDisable()
        {
            ResetLongClickState();
            isPointerDown = false;
        }

        /// <summary>
        /// 点击（抬起时触发）
        /// </summary>
        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (!canInteraction) return;

            // 点击间隔限制
            if (Time.realtimeSinceStartup - lastClickTimeTemp >= clickInterval)
            {
                onClick?.Invoke();
                lastClickTimeTemp = Time.realtimeSinceStartup;
            }
        }

        /// <summary>
        /// 按下触发
        /// </summary>
        public virtual void OnPointerDown(PointerEventData eventData)
        {
            if (!canInteraction) return;

            isPointerDown = true;

            // 设置为选中对象
            if (EventSystem.current != null)
            {
                EventSystem.current.SetSelectedGameObject(gameObject, eventData);
            }

            ResetLongClickState();
            onPointerDown?.Invoke();
        }

        /// <summary>
        /// 抬起触发
        /// </summary>
        public virtual void OnPointerUp(PointerEventData eventData)
        {
            if (!canInteraction) return;

            isPointerDown = false;
            ResetLongClickState();
            onPointerUp?.Invoke();
        }

        /// <summary>
        /// 长按触发时间（安全属性，自动修正非法值）
        /// </summary>
        public float LongClickTriggerTime
        {
            get { return longClickTriggerTime <= 0 ? 0.3f : longClickTriggerTime; }
            set { longClickTriggerTime = value <= 0 ? 0.3f : value; }
        }

        /// <summary>
        /// 重置长按状态
        /// </summary>
        public void ResetLongClickState()
        {
            tempPointerDownTime = 0f;
            isTriggerLongClick = false;
        }

        /// <summary>
        /// 长按检测（每帧判断）
        /// </summary>
        public virtual void Update()
        {
            if (!canInteraction) return;

            // 按住状态才检测长按
            if (isPointerDown)
            {
                tempPointerDownTime += Time.unscaledDeltaTime;

                // 达到长按时间
                if (tempPointerDownTime >= longClickTriggerTime)
                {
                    // 非连续模式：只触发一次
                    if (!isContinuousTriggerLongClick && isTriggerLongClick)
                        return;

                    // 连续模式：重置计时循环触发
                    tempPointerDownTime = longClickTriggerTime - longClickTriggerInterval;
                    isTriggerLongClick = true;
                    onLongClick?.Invoke();
                }
            }
        }

        /// <summary>
        /// 选中触发
        /// </summary>
        public void OnSelect(BaseEventData eventData)
        {
            onSelect?.Invoke();
        }

        /// <summary>
        /// 取消选中触发
        /// </summary>
        public void OnDeselect(BaseEventData eventData)
        {
            onDeselect?.Invoke();
        }

        /// <summary>
        /// 强制设置为选中状态（用于弹窗自动选中）
        /// </summary>
        public void ForceSetSelected()
        {
            EventSystem.current.SetSelectedGameObject(gameObject);
        }
    }
}