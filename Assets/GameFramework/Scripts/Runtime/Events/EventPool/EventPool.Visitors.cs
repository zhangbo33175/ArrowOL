using System.Collections.Generic;

namespace Honor.Runtime
{
    public sealed partial class EventPool<T> where T : EventParams
    {
        /// <summary>
        /// Lua 脚本组件，用于事件转发至 Lua 层
        /// </summary>
        private LuaComponent m_LuaComponent;

        /// <summary>
        /// 已注册的事件处理函数字典
        /// 结构：事件ID -> (用户数据 -> 回调函数)
        /// </summary>
        private readonly GameMultiDictionary<GameEventCmd, Dictionary<object, HonorEventHandler<T>>> m_SubscribedEventHandlers;

        /// <summary>
        /// 线程安全的事件队列，用于在下一帧派发
        /// </summary>
        private readonly Queue<Event> m_EventsForFire;

        /// <summary>
        /// 事件遍历缓存节点，优化事件派发过程
        /// </summary>
        private readonly Dictionary<T, LinkedListNode<Dictionary<object, HonorEventHandler<T>>>> m_CachedNodes;

        /// <summary>
        /// 临时缓存节点，用于安全地取消事件订阅
        /// </summary>
        private readonly Dictionary<T, LinkedListNode<Dictionary<object, HonorEventHandler<T>>>> m_TempNodes;

        /// <summary>
        /// 当前已注册的**事件类型数量**
        /// </summary>
        public int SubscribedEventTypeCount => m_SubscribedEventHandlers.Count;

        /// <summary>
        /// 获取指定事件 ID 注册的回调数量
        /// </summary>
        /// <param name="cmd">事件命令 ID</param>
        /// <returns>注册的回调个数</returns>
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
        /// 当前等待主线程派发的事件数量
        /// </summary>
        public int EventsForFireCount => m_EventsForFire.Count;
    }
}