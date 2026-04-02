
namespace Honor.Runtime
{
    public sealed partial class EventComponent : GameComponent
    {
        /// <summary>
        /// 事件管理器
        /// </summary>
        private EventManager m_EventManager = null;
        public EventManager EventManager
        {
            get
            {
                return m_EventManager;
            }
        }

        /// <summary>
        /// 获取已注册事件类型数量。
        /// </summary>
        public int SubscribedEventTypeCount
        {
            get
            {
                return m_EventManager.SubscribedEventTypeCount;
            }
        }

        /// <summary>
        /// 获取已注册指定事件类型的处理函数的数量。
        /// </summary>
        /// <param name="cmd">事件类型编号。</param>
        /// <returns>事件处理函数的数量。</returns>
        public int SubscribedEventCount(GameEventCmd cmd)
        {
            return m_EventManager.SubscribedEventCount(cmd);
        }

        /// <summary>
        /// 获取待派发的事件数量。
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


