using System.Collections.Generic;

namespace Honor.Runtime
{
    public sealed partial class EventPool<T> where T : EventParams
    {
        /// <summary>
        /// Lua组件
        /// </summary>
        private LuaComponent m_LuaComponent;

        /// <summary>
        /// 所有已注册事件类型的事件处理集合
        /// <事件类型ID, <事件处理函数宿主对象，事件处理函数集合>>
        /// 注意：事件类型与事件处理函数为一对多的关系
        /// </summary>
        private readonly GameMultiDictionary<GameEventCmd, Dictionary<object, HonorEventHandler<T>>> m_SubscribedEventHandlers;

        /// <summary>
        /// 下一帧待派发的事件集合
        /// </summary>
        private readonly Queue<Event> m_EventsForFire;

        /// <summary>
        /// 缓存节点集合
        /// <事件参数，<事件处理函数宿主对象, 事件处理函数>>
        /// </summary>
        private readonly Dictionary<T, LinkedListNode<Dictionary<object, HonorEventHandler<T>>>> m_CachedNodes;

        /// <summary>
        /// 临时节点集合
        /// <事件参数，<事件处理函数宿主对象, 事件处理函数>>
        /// </summary>
        private readonly Dictionary<T, LinkedListNode<Dictionary<object, HonorEventHandler<T>>>> m_TempNodes;

        /// <summary>
        /// 获取已注册事件类型数量。
        /// </summary>
        public int SubscribedEventTypeCount
        {
            get
            {
                return m_SubscribedEventHandlers.Count;
            }
        }

        /// <summary>
        /// 获取已注册指定事件类型的事件处理函数的数量。
        /// </summary>
        /// <param name="cmd">事件类型编号。</param>
        /// <returns>事件处理函数的数量。</returns>
        public int SubscribedEventCount(GameEventCmd cmd)
        {
            GameLinkedListRange<Dictionary<object, HonorEventHandler<T>>> range = null;
            if (m_SubscribedEventHandlers.TryGetValue(cmd, out range))
            {
                return range.Count;
            }
            return 0;
        }

        /// <summary>
        /// 获取待派发的事件数量。
        /// </summary>
        public int EventsForFireCount
        {
            get
            {
                return m_EventsForFire.Count;
            }
        }

    }

}


