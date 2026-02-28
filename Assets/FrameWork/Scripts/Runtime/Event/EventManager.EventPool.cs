using System;
using System.Collections.Generic;

namespace Honor.Runtime
{
    public sealed partial class EventManager
    {
        public delegate void GameEventHandler<TEventArgs>(object sender, object userData, TEventArgs e);

        /// <summary>
        /// 事件对象池
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class EventPool<T> where T : EventParams
        {
            /// <summary>
            /// 所有已注册事件类型的事件处理集合
            /// <事件类型ID, <事件处理函数宿主对象，事件处理函数集合>>
            /// 注意：事件类型与事件处理函数为一对多的关系
            /// </summary>
            public static GameMultiDictionary<EventCmd, Dictionary<object, GameEventHandler<T>>>
                m_SubscribedEventHandlers;

            /// <summary>
            /// 下一帧待派发的事件集合
            /// </summary>
            public static Queue<PoolEvent> m_EventsForFire;

            /// <summary>
            /// 缓存节点集合
            /// <事件参数，<事件处理函数宿主对象, 事件处理函数>>
            /// </summary>
            public static Dictionary<T, LinkedListNode<Dictionary<object, GameEventHandler<T>>>> m_CachedNodes;

            /// <summary>
            /// 临时节点集合
            /// <事件参数，<事件处理函数宿主对象, 事件处理函数>>
            /// </summary>
            public static Dictionary<T, LinkedListNode<Dictionary<object, GameEventHandler<T>>>> m_TempNodes;

            /// <summary>
            /// 初始化事件池的新实例。
            /// </summary>
            public EventPool()
            {
                m_SubscribedEventHandlers =new GameMultiDictionary<EventCmd, Dictionary<object, GameEventHandler<T>>>();
                m_EventsForFire = new Queue<PoolEvent>();
                m_CachedNodes = new Dictionary<T, LinkedListNode<Dictionary<object, GameEventHandler<T>>>>();
                m_TempNodes = new Dictionary<T, LinkedListNode<Dictionary<object, GameEventHandler<T>>>>();
            }

            /// <summary>
            /// 事件池轮询。
            /// </summary>
            public void Update()
            {
                while (m_EventsForFire.Count > 0)
                {
                    PoolEvent eventNode = null;
                    lock (m_EventsForFire)
                    {
                        eventNode = m_EventsForFire.Dequeue();
                        Handle_Event(eventNode.Sender, eventNode.EventParams);
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
            public bool On_Check_Event(EventCmd cmd, object userData, GameEventHandler<T> handler)
            {
                if (handler == null)
                {
                    throw new Exception("事件处理函数无效。");
                }

                if (m_SubscribedEventHandlers.Contains(cmd))
                {
                    GameLinkedListRange<Dictionary<object, GameEventHandler<T>>> range = null;
                    if (m_SubscribedEventHandlers.TryGetValue(cmd, out range))
                    {
                        LinkedListNode<Dictionary<object, GameEventHandler<T>>> current = range.First;
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
            public void Add_Event(EventCmd cmd, object userData, GameEventHandler<T> handler)
            {
                if (handler == null)
                {
                    throw new Exception("事件处理函数无效。");
                }

                if (On_Check_Event(cmd, userData, handler))
                {
                    throw new Exception(AorTxt.Format("不允许事件 '{0}' 有重复处理函数。", cmd.ToString()));
                }
                else
                {
                    Dictionary<object, GameEventHandler<T>> EventItem = new Dictionary<object, GameEventHandler<T>>();
                    EventItem.Add(userData, handler);
                    m_SubscribedEventHandlers.Add(cmd, EventItem);
                }
            }

            /// <summary>
            /// 注销事件处理函数。
            /// </summary>
            /// <param name="cmd">事件类型编号。</param>
            /// <param name="userData">用户数据。</param>
            /// <param name="handler">要取消注册的事件处理函数。</param>
            public void Remove_Event(EventCmd cmd, object userData, GameEventHandler<T> handler)
            {
                if (handler == null)
                {
                    throw new Exception("事件处理函数无效。");
                }

                if (m_CachedNodes.Count > 0)
                {
                    foreach (var cachedNode in m_CachedNodes)
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
                        foreach (var cachedNode in m_TempNodes)
                        {
                            m_CachedNodes[cachedNode.Key] = cachedNode.Value;
                        }

                        m_TempNodes.Clear();
                    }
                }

                if (m_SubscribedEventHandlers.Contains(cmd))
                {
                    GameLinkedListRange<Dictionary<object, GameEventHandler<T>>> range = null;
                    if (m_SubscribedEventHandlers.TryGetValue(cmd, out range))
                    {
                        LinkedListNode<Dictionary<object, GameEventHandler<T>>> current = range.First;
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
            public void Dispatch_Event(object sender, T e)
            {
                if (e == null)
                {
                    throw new Exception("事件参数无效。");
                }

                // 创建事件暂时缓存到事件队列中等待在Update中进行事件派发
                PoolEvent eventNode = PoolEvent.Create(sender, e);
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
            public void Dispatch_Event_Now(object sender, T e)
            {
                if (e == null)
                {
                    throw new Exception("事件参数无效。");
                }

                // 立即抛出事件时需要创建Event，直接派发即可。
                Handle_Event(sender, e);
            }

            /// <summary>
            /// 处理事件结点。
            /// </summary>
            /// <param name="sender">事件源。</param>
            /// <param name="e">事件参数。</param>
            private void Handle_Event(object sender, T e)
            {
                GameLinkedListRange<Dictionary<object, GameEventHandler<T>>> range = null;
                if (m_SubscribedEventHandlers.TryGetValue(e.EventId, out range))
                {
                    LinkedListNode<Dictionary<object, GameEventHandler<T>>> current = range.First;
                    while (current != null && current != range.Terminal)
                    {
                        foreach (KeyValuePair<object, GameEventHandler<T>> itr in current.Value)
                        {
                            itr.Value(sender, itr.Key, e);
                        }

                        current = m_CachedNodes[e] = current.Next != range.Terminal ? current.Next : null;
                    }

                    m_CachedNodes.Remove(e);
                }

                /*// 转发事件到Lua层全局接口
                if (m_LuaComponent.LuaReceiveEventCSEventDelegate != null)
                {
                    m_LuaComponent.LuaReceiveEventCSEventDelegate(e);
                }*/
            }

            /// <summary>
            /// 获取已注册事件类型数量。
            /// </summary>
            public static int Subscribed_Event_Type_Count
            {
                get { return m_SubscribedEventHandlers.Count; }
            }

            /// <summary>
            /// 获取已注册指定事件类型的事件处理函数的数量。
            /// </summary>
            /// <param name="cmd">事件类型编号。</param>
            /// <returns>事件处理函数的数量。</returns>
            public static int Subscribed_Event_Count(EventCmd cmd)
            {
                GameLinkedListRange<Dictionary<object, GameEventHandler<T>>> range = null;
                if (m_SubscribedEventHandlers.TryGetValue(cmd, out range))
                {
                    return range.Count;
                }

                return 0;
            }

            /// <summary>
            /// 获取待派发的事件数量。
            /// </summary>
            public static int Events_For_Fire_Count
            {
                get { return m_EventsForFire.Count; }
            }

            #region PoolEvent

            public class PoolEvent
            {
                private object m_Sender;
                private T m_EventParams;

                public PoolEvent()
                {
                    m_Sender = null;
                    m_EventParams = null;
                }

                public object Sender
                {
                    get { return m_Sender; }
                }

                public T EventParams
                {
                    get { return m_EventParams; }
                }

                public static PoolEvent Create(object sender, T e)
                {
                    PoolEvent poolEventNode = new PoolEvent();
                    poolEventNode.m_Sender = sender;
                    poolEventNode.m_EventParams = e;
                    return poolEventNode;
                }

                public void Clear()
                {
                    m_Sender = null;
                    m_EventParams = null;
                }
            }

            #endregion
        }
    }
}