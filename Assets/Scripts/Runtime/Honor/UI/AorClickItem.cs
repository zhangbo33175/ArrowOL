using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Object = UnityEngine.Object;

namespace GameLib
{
    public class AorClickItem : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler,
        ISelectHandler, IDeselectHandler
    {
        public Action onClick;
        public Action onLongClick;
        public Action onPointerDown;
        public Action onPointerUp;
        public Action onSelect;
        public Action onDeselect;

        public float clickInterval = 0;
        public bool canInteraction = true; //是否可以交互
        
        [SerializeField]
        private bool isContinuousTriggerLongClick = false; //是否持续触发长按事件
        private float lastClickTimeTemp; //最后一次点击事件
        private float longClickTriggerTime = 0.3f; //长按生效时间
        private float longClickTriggerInterval = 0.1f; //长按触发间隔
        private bool isPointerDown; //是否已经按下按钮并没有释放
        private float tempPointerDownTime; //缓存按钮已经按下的时间
        private bool isTriggerLongClick; //是否已经触发过长按

        public void OnDisable()
        {
            ResetLongClickState();
            isPointerDown = false;
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (!canInteraction) return;

            if (Time.realtimeSinceStartup - lastClickTimeTemp >= clickInterval)
            {
                onClick?.Invoke();
                lastClickTimeTemp = Time.realtimeSinceStartup;
            }
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            if (!canInteraction) return;

            isPointerDown = true;

            if ((Object)(object)EventSystem.current != (Object)null)
            {
                EventSystem.current.SetSelectedGameObject(gameObject, eventData);
            }

            ResetLongClickState();
            onPointerDown?.Invoke();
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            if (!canInteraction) return;

            isPointerDown = false;
            ResetLongClickState();
            onPointerUp?.Invoke();
        }

        /// <summary>
        /// 设置长按生效时间
        /// </summary>
        public float LongClickTriggerTime
        {
            get
            {
                if (longClickTriggerTime <= 0)
                {
                    return 0.3f;
                }
                else return longClickTriggerTime;
            }
            set
            {
                if (value <= 0) longClickTriggerTime = 0.3f;
                else longClickTriggerTime = value;
            }
        }

        /// <summary>
        /// 重设长按状态，准备下次触发长按效果
        /// </summary>
        public void ResetLongClickState()
        {
            tempPointerDownTime = 0f;
            isTriggerLongClick = false;
        }

        public virtual void Update()
        {
            if (!canInteraction) return;

            if (isPointerDown)
            {
                tempPointerDownTime += Time.unscaledDeltaTime;
                if (tempPointerDownTime >= longClickTriggerTime)
                {
                    if (!isContinuousTriggerLongClick && isTriggerLongClick)
                    {
                        return;
                    }
                    
                    tempPointerDownTime = longClickTriggerTime - longClickTriggerInterval;
                    isTriggerLongClick = true;
                    onLongClick?.Invoke();
                }
            }
        }

        public void OnSelect(BaseEventData eventData)
        {
            onSelect?.Invoke();
        }

        public void OnDeselect(BaseEventData eventData)
        {
            //Debug.Log("OnDeselect : " + eventData.selectedObject?.ToString());
            onDeselect?.Invoke();
        }

        /// <summary>
        /// 强制触发一次选中事件 (主要用于那些自动弹出的UI)
        /// </summary>
        public void ForceSetSelected()
        {
            EventSystem.current.SetSelectedGameObject(gameObject);
        }
    }
}