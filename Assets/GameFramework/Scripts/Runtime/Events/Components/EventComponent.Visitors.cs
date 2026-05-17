/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  EventComponent.Fields.cs
 * author:    云毅
 *  created:   2026
 * descrip:   全局事件系统组件 - 字段与属性（partial）
 ***************************************************************/

namespace Honor.Runtime
{
    /// <summary>
    /// 事件系统组件（扩展部分）
    /// 提供事件管理器、事件数量查询等只读属性
    /// </summary>
    public sealed partial class EventComponent : GameComponent
    {
        //=========================================================================
        // 私有字段
        //=========================================================================
        #region Field
        /// <summary>
        /// 事件管理器实例
        /// </summary>
        private EventManager m_EventManager;
        #endregion

        //=========================================================================
        // 公共属性 & 接口
        //=========================================================================
        #region Property
        /// <summary>
        /// 获取事件管理器实例（只读）
        /// </summary>
        public EventManager EventManager => m_EventManager;

        /// <summary>
        /// 已注册的事件类型数量（只读）
        /// </summary>
        public int SubscribedEventTypeCount => m_EventManager.SubscribedEventTypeCount;

        /// <summary>
        /// 等待派发的事件数量（事件队列长度）
        /// </summary>
        public int EventsForFireCount => m_EventManager.EventsForFireCount;
        #endregion

        //=========================================================================
        // 公共方法
        //=========================================================================
        #region Method
        /// <summary>
        /// 获取指定事件已注册的回调函数数量
        /// </summary>
        /// <param name="cmd">事件命令ID</param>
        /// <returns>回调数量</returns>
        public int SubscribedEventCount(GameEventCmd cmd)
        {
            return m_EventManager.SubscribedEventCount(cmd);
        }
        #endregion
    }
}