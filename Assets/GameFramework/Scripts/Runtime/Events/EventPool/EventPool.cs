/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  EventPool.cs
 * author:    云毅
 * created: 2025
 * descrip:   泛型事件池 - 核心事件驱动模块
 ***************************************************************/

using System.Collections.Generic;

namespace Honor.Runtime
{
    /// <summary>
    /// 泛型事件池（核心事件驱动模块）
    /// 负责事件的订阅、取消订阅、线程安全入队、主线程派发
    /// </summary>
    /// <typeparam name="T">事件参数类型，必须继承自 EventParams</typeparam>
    public sealed partial class EventPool<T> where T : EventParams
    {
        //=========================================================================
        // 构造函数
        //=========================================================================
        #region Constructor
        /// <summary>
        /// 初始化事件池实例
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
        #endregion

        //=========================================================================
        // 生命周期 & 队列驱动
        //=========================================================================
        #region MonoBehaviour
        /// <summary>
        /// 事件池帧更新（主线程派发队列事件）
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
        #endregion

        //=========================================================================
        // 公共管理接口
        //=========================================================================
        #region Public Methods
        /// <summary>
        /// 关闭并完全清理事件池
        /// </summary>
        public void Shutdown()
        {
            Clear();
            m_SubscribedEventHandlers.Clear();
            m_CachedNodes.Clear();
            m_TempNodes.Clear();
        }

        /// <summary>
        /// 清空待派发事件队列
        /// </summary>
        public void Clear()
        {
            lock (m_EventsForFire)
            {
                m_EventsForFire.Clear();
            }
        }
        #endregion

        //=========================================================================
        // 订阅 / 取消订阅 / 检查
        //=========================================================================
        #region Subscribe & Unsubscribe
        /// <summary>
        /// 检查指定事件是否已注册对应回调
        /// </summary>
        /// <param name="cmd">事件命令</param>
        /// <param name="userData">用户数据</param>
        /// <param name="handler">事件回调</param>
        /// <returns>是否已注册</returns>
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
        /// 订阅事件
        /// </summary>
        /// <param name="cmd">事件命令</param>
        /// <param name="userData">用户数据</param>
        /// <param name="handler">事件回调</param>
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
        /// 取消订阅事件
        /// </summary>
        /// <param name="cmd">事件命令</param>
        /// <param name="userData">用户数据</param>
        /// <param name="handler">事件回调</param>
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
        #endregion

        //=========================================================================
        // 事件派发
        //=========================================================================
        #region Fire Events
        /// <summary>
        /// 线程安全抛出事件（入队，下一帧派发）
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        public void Fire(object sender, T e)
        {
            if (e == null)
            {
                throw new GameException("事件参数无效。");
            }

            Event eventNode = Event.Create(sender, e);
            lock (m_EventsForFire)
            {
                m_EventsForFire.Enqueue(eventNode);
            }
        }

        /// <summary>
        /// 立即抛出事件（同步执行，非线程安全）
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        public void FireNow(object sender, T e)
        {
            if (e == null)
            {
                throw new GameException("事件参数无效。");
            }

            HandleEvent(sender, e);
        }
        #endregion

    }
}