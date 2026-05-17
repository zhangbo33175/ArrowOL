/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  EventPool.Event.cs
 * author:    云毅
 *  created:   2026
 * descrip:   泛型事件池 - 内部事件节点类（partial）
 ***************************************************************/

namespace Honor.Runtime
{
    /// <summary>
    /// 泛型事件池 - 内部事件节点定义
    /// </summary>
    public sealed partial class EventPool<T> where T : EventParams
    {
        /// <summary>
        /// 事件节点（内部私有类）
        /// 用于封装事件发送者与事件参数，作为事件队列的存储单元
        /// </summary>
        private sealed class Event
        {
            //=========================================================================
            // 私有字段
            //=========================================================================
            #region Fields
            /// <summary>
            /// 事件发送者
            /// </summary>
            private object m_Sender;

            /// <summary>
            /// 事件参数
            /// </summary>
            private T m_EventParams;
            #endregion

            //=========================================================================
            // 构造函数
            //=========================================================================
            #region Constructor
            /// <summary>
            /// 默认构造函数
            /// </summary>
            public Event()
            {
                m_Sender = null;
                m_EventParams = null;
            }
            #endregion

            //=========================================================================
            // 公共属性
            //=========================================================================
            #region Properties
            /// <summary>
            /// 获取事件发送者
            /// </summary>
            public object Sender => m_Sender;

            /// <summary>
            /// 获取事件参数
            /// </summary>
            public T EventParams => m_EventParams;
            #endregion

            //=========================================================================
            // 公共方法
            //=========================================================================
            #region Public Methods
            /// <summary>
            /// 创建事件节点（静态工厂方法）
            /// </summary>
            /// <param name="sender">事件发送者</param>
            /// <param name="e">事件参数</param>
            /// <returns>创建完成的事件节点</returns>
            public static Event Create(object sender, T e)
            {
                Event eventNode = new Event();
                eventNode.m_Sender = sender;
                eventNode.m_EventParams = e;
                return eventNode;
            }

            /// <summary>
            /// 清理事件节点引用，便于复用或回收
            /// </summary>
            public void Clear()
            {
                m_Sender = null;
                m_EventParams = null;
            }
            #endregion
        }
    }
}