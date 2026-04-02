using System.Collections.Generic;

namespace Honor.Runtime
{
    public sealed partial class EventPool<T> where T : EventParams
    {
        /// <summary>
        /// 初始化事件池的新实例。
        /// </summary>
        public EventPool()
        {
            m_LuaComponent = GameComponentsGroup.GetComponent<LuaComponent>();
            if (m_LuaComponent == null)
            {
                Log.Fatal("Lua Component 无效。");
                return;
            }

            m_SubscribedEventHandlers = new GameMultiDictionary<GameEventCmd, Dictionary<object, HonorEventHandler<T>>>();
            m_EventsForFire = new Queue<Event>();
            m_CachedNodes = new Dictionary<T, LinkedListNode<Dictionary<object, HonorEventHandler<T>>>>();
            m_TempNodes = new Dictionary<T, LinkedListNode<Dictionary<object, HonorEventHandler<T>>>>();
        }

        /// <summary>
        /// 事件池轮询。
        /// </summary>
        public void Update()
        {
            while (m_EventsForFire.Count > 0)
            {
                Event eventNode = null;
                lock (m_EventsForFire)
                {
                    eventNode = m_EventsForFire.Dequeue();
                    HandleEvent(eventNode.Sender, eventNode.EventParams);
                }
            }
        }

        /// <summary>
        /// 关闭并清理事件池。
        /// </summary>
        public void Shutdown()
        {
            Clear();
            m_SubscribedEventHandlers.Clear();
            m_CachedNodes.Clear();
            m_TempNodes.Clear();
        }

        /// <summary>
        /// 清理事件。
        /// </summary>
        public void Clear()
        {
            lock (m_EventsForFire)
            {
                m_EventsForFire.Clear();
            }
        }

        /// <summary>
        /// 检查是否存在事件处理函数。
        /// </summary>
        /// <param name="cmd">事件类型编号。</param>
        /// <param name="userData">用户数据。</param>
        /// <param name="handler">要检查的事件处理函数。</param>
        /// <returns>是否存在事件处理函数。</returns>
        public bool Check(GameEventCmd cmd, object userData, HonorEventHandler<T> handler)
        {
            if (handler == null)
            {
                throw new GameException("事件处理函数无效。");
            }

            if (m_SubscribedEventHandlers.Contains(cmd))
            {
                GameLinkedListRange<Dictionary<object, HonorEventHandler<T>>> range = null;
                if (m_SubscribedEventHandlers.TryGetValue(cmd, out range))
                {
                    LinkedListNode<Dictionary<object, HonorEventHandler<T>>> current = range.First;
                    while (current != null && current != range.Terminal)
                    {
                        if (current.Value.ContainsKey(userData))
                        {
                            return current.Value[userData] == handler;
                        }
                        current = current.Next != range.Terminal ? current.Next : null;
                    }
                }
            }
            return false;

        }

        /// <summary>
        /// 注册事件处理函数。
        /// </summary>
        /// <param name="cmd">事件类型编号。</param>
        /// <param name="userData">用户数据。</param>
        /// <param name="handler">要注册的事件处理函数。</param>
        public void Subscribe(GameEventCmd cmd, object userData, HonorEventHandler<T> handler)
        {
            if (handler == null)
            {
                throw new GameException("事件处理函数无效。");
            }

            if (Check(cmd, userData, handler))
            {
                throw new GameException(AorTxt.Format("不允许事件 '{0}' 有重复处理函数。", cmd.ToString()));
            }
            else
            {
                Dictionary<object, HonorEventHandler<T>> item = new Dictionary<object, HonorEventHandler<T>>();
                item.Add(userData, handler);
                m_SubscribedEventHandlers.Add(cmd, item);
            }

        }

        /// <summary>
        /// 注销事件处理函数。
        /// </summary>
        /// <param name="cmd">事件类型编号。</param>
        /// <param name="userData">用户数据。</param>
        /// <param name="handler">要取消注册的事件处理函数。</param>
        public void Unsubscribe(GameEventCmd cmd, object userData, HonorEventHandler<T> handler)
        {
            if (handler == null)
            {
                throw new GameException("事件处理函数无效。");
            }

            if (m_CachedNodes.Count > 0)
            {
                foreach (KeyValuePair<T, LinkedListNode<Dictionary<object, HonorEventHandler<T>>>> cachedNode in m_CachedNodes)
                {
                    if (cachedNode.Value != null)
                    {
                        foreach (var itr in cachedNode.Value.Value)
                        {
                            if (itr.Key == userData && itr.Value == handler)
                            {
                                m_TempNodes.Add(cachedNode.Key, cachedNode.Value.Next);
                            }
                        }
                    }
                }

                if (m_TempNodes.Count > 0)
                {
                    foreach (KeyValuePair<T, LinkedListNode<Dictionary<object, HonorEventHandler<T>>>> cachedNode in m_TempNodes)
                    {
                        m_CachedNodes[cachedNode.Key] = cachedNode.Value;
                    }
                    m_TempNodes.Clear();
                }
            }

            if (m_SubscribedEventHandlers.Contains(cmd))
            {
                GameLinkedListRange<Dictionary<object, HonorEventHandler<T>>> range = null;
                if (m_SubscribedEventHandlers.TryGetValue(cmd, out range))
                {
                    LinkedListNode<Dictionary<object, HonorEventHandler<T>>> current = range.First;
                    while (current != null && current != range.Terminal)
                    {
                        if (current.Value.ContainsKey(userData))
                        {
                            if (current.Value[userData] == handler)
                            {
                                current.Value.Remove(userData);
                                if (current.Value.Count == 0)
                                {
                                    m_SubscribedEventHandlers.Remove(cmd, current.Value);
                                    return;
                                }
                            }
                        }
                        current = current.Next != range.Terminal ? current.Next : null;
                    }
                }
            }
        }

        /// <summary>
        /// 抛出事件，这个操作是线程安全的，即使不在主线程中抛出，也可保证在主线程中回调事件处理函数，但事件会在抛出后的下一帧分发。
        /// </summary>
        /// <param name="sender">事件源。</param>
        /// <param name="e">事件参数。</param>
        public void Fire(object sender, T e)
        {
            if (e == null)
            {
                throw new GameException("事件参数无效。");
            }

            // 创建事件暂时缓存到事件队列中等待在Update中进行事件派发
            Event eventNode = Event.Create(sender, e);
            lock (m_EventsForFire)
            {
                m_EventsForFire.Enqueue(eventNode);
            }
        }

        /// <summary>
        /// 抛出事件立即模式，这个操作不是线程安全的，事件会立刻分发。
        /// </summary>
        /// <param name="sender">事件源。</param>
        /// <param name="e">事件参数。</param>
        public void FireNow(object sender, T e)
        {
            if (e == null)
            {
                throw new GameException("事件参数无效。");
            }

            // 立即抛出事件时需要创建Event，直接派发即可。
            HandleEvent(sender, e);
        }

    }

}


