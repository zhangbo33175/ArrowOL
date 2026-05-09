using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Honor.Runtime
{
    /// <summary>
    /// 通用点击事件监听器
    /// 为GameObject统一提供单击、双击、按下、抬起的UI事件监听功能
    /// 采用组件化设计，动态添加到目标对象上使用
    /// </summary>
    public class AorClickEventListener : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
    {
        /// <summary>
        /// 获取/创建 目标游戏对象上的点击事件监听器
        /// 静态工具方法，简化组件获取与添加流程
        /// </summary>
        /// <param name="obj">需要添加监听的游戏对象</param>
        /// <returns>目标对象上的AorClickEventListener实例</returns>
        public static AorClickEventListener Get(GameObject obj)
        {
            AorClickEventListener listener = obj.GetComponent<AorClickEventListener>();
            if (listener == null)
            {
                listener = obj.AddComponent<AorClickEventListener>();
            }
            return listener;
        }

        /// <summary>
        /// 单击回调委托
        /// 参数：触发事件的当前游戏对象
        /// </summary>
        private System.Action<GameObject> mClickedHandler = null;

        /// <summary>
        /// 双击回调委托
        /// 参数：触发事件的当前游戏对象
        /// </summary>
        private System.Action<GameObject> mDoubleClickedHandler = null;

        /// <summary>
        /// 指针按下回调委托
        /// 参数：触发事件的当前游戏对象
        /// </summary>
        private System.Action<GameObject> mOnPointerDownHandler = null;

        /// <summary>
        /// 指针抬起回调委托
        /// 参数：触发事件的当前游戏对象
        /// </summary>
        private System.Action<GameObject> mOnPointerUpHandler = null;

        /// <summary>
        /// 标记当前是否处于按下状态
        /// </summary>
        private bool mIsPressed = false;

        /// <summary>
        /// 获取当前是否按下
        /// </summary>
        public bool IsPressd
        {
            get { return mIsPressed; }
        }

        /// <summary>
        /// 实现IPointerClickHandler接口
        /// 处理点击/双击事件分发
        /// </summary>
        /// <param name="eventData">指针事件数据</param>
        public void OnPointerClick(PointerEventData eventData)
        {
            // 双击事件
            if (eventData.clickCount == 2)
            {
                mDoubleClickedHandler?.Invoke(gameObject);
            }
            // 单击事件
            else
            {
                mClickedHandler?.Invoke(gameObject);
            }
        }

        /// <summary>
        /// 设置单击事件回调
        /// </summary>
        /// <param name="handler">单击回调方法</param>
        public void SetClickEventHandler(System.Action<GameObject> handler)
        {
            mClickedHandler = handler;
        }

        /// <summary>
        /// 设置双击事件回调
        /// </summary>
        /// <param name="handler">双击回调方法</param>
        public void SetDoubleClickEventHandler(System.Action<GameObject> handler)
        {
            mDoubleClickedHandler = handler;
        }

        /// <summary>
        /// 设置指针按下事件回调
        /// </summary>
        /// <param name="handler">指针按下回调方法</param>
        public void SetPointerDownHandler(System.Action<GameObject> handler)
        {
            mOnPointerDownHandler = handler;
        }

        /// <summary>
        /// 设置指针抬起事件回调
        /// </summary>
        /// <param name="handler">指针抬起回调方法</param>
        public void SetPointerUpHandler(System.Action<GameObject> handler)
        {
            mOnPointerUpHandler = handler;
        }

        /// <summary>
        /// 实现IPointerDownHandler接口
        /// 处理指针按下事件
        /// </summary>
        /// <param name="eventData">指针事件数据</param>
        public void OnPointerDown(PointerEventData eventData)
        {
            mIsPressed = true;
            mOnPointerDownHandler?.Invoke(gameObject);
        }

        /// <summary>
        /// 实现IPointerUpHandler接口
        /// 处理指针抬起事件
        /// </summary>
        /// <param name="eventData">指针事件数据</param>
        public void OnPointerUp(PointerEventData eventData)
        {
            mIsPressed = false;
            mOnPointerUpHandler?.Invoke(gameObject);
        }
    }
}