/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  EventManager.Stats.cs
 * author:    云毅
 * created:   2026
 * descrip:   全局事件管理器 - 统计查询接口（partial）
 ***************************************************************/

namespace Honor.Runtime
{
    /// <summary>
    /// 事件管理器（统计查询接口部分）
    /// 提供事件注册数量、队列数量等只读属性查询
    /// </summary>
    public sealed partial class EventManager
    {
        //=========================================================================
        // 私有字段
        //=========================================================================
        #region Field
        /// <summary>
        /// 底层事件池实例
        /// </summary>
        private readonly EventPool<EventParams> m_EventPool;
        #endregion

        //=========================================================================
        // 公共属性 & 统计接口
        //=========================================================================
        #region Property & Method
        /// <summary>
        /// 获取当前已注册的**事件类型总数**
        /// </summary>
        public int SubscribedEventTypeCount => m_EventPool.SubscribedEventTypeCount;

        /// <summary>
        /// 获取当前等待主线程派发的**事件队列数量**
        /// </summary>
        public int EventsForFireCount => m_EventPool.EventsForFireCount;

        /// <summary>
        /// 获取指定事件已注册的**回调函数总数**
        /// </summary>
        /// <param name="cmd">事件命令 ID</param>
        /// <returns>注册的处理函数数量</returns>
        public int SubscribedEventCount(GameEventCmd cmd)
        {
            return m_EventPool.SubscribedEventCount(cmd);
        }
        #endregion
    }
}