namespace Honor.Runtime
{
    /// <summary>
    /// 事件系统组件（扩展部分）
    /// 提供事件管理器、事件数量查询等只读属性
    /// </summary>
    public sealed partial class EventComponent : GameComponent
    {
        /// <summary>
        /// 事件管理器实例
        /// </summary>
        private EventManager m_EventManager = null;

        /// <summary>
        /// 获取事件管理器实例（只读）
        /// </summary>
        public EventManager EventManager
        {
            get
            {
                return m_EventManager;
            }
        }

        /// <summary>
        /// 已注册的事件类型数量（只读）
        /// </summary>
        public int SubscribedEventTypeCount
        {
            get
            {
                return m_EventManager.SubscribedEventTypeCount;
            }
        }

        /// <summary>
        /// 获取指定事件已注册的回调函数数量
        /// </summary>
        /// <param name="cmd">事件命令ID</param>
        /// <returns>回调数量</returns>
        public int SubscribedEventCount(GameEventCmd cmd)
        {
            return m_EventManager.SubscribedEventCount(cmd);
        }

        /// <summary>
        /// 等待派发的事件数量（事件队列长度）
        /// </summary>
        public int EventsForFireCount
        {
            get
            {
                return m_EventManager.EventsForFireCount;
            }
        }
    }
}