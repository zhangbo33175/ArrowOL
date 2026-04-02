namespace Honor.Runtime
{
    public sealed partial class EventManager
    {
        /// <summary>
        /// 初始化事件管理器的新实例。
        /// </summary>
        public EventManager()
        {
            m_EventPool = new EventPool<EventParams>();
        }

        /// <summary>
        /// 管理器心跳
        /// </summary>
        public void Update()
        {
            m_EventPool.Update();
        }

        /// <summary>
        /// 关闭并清理管理器。
        /// </summary>
        public void Shutdown()
        {
            m_EventPool.Shutdown();
        }

        /// <summary>
        /// 检查是否存在事件处理函数。
        /// </summary>
        /// <param name="cmd">事件类型编号。</param>
        /// <param name="userData">用户数据。</param>
        /// <param name="handler">要检查的事件处理函数。</param>
        /// <returns>是否存在事件处理函数。</returns>
        public bool Check(GameEventCmd cmd, object userData, HonorEventHandler<EventParams> handler)
        {
            return m_EventPool.Check(cmd, userData, handler);
        }

        /// <summary>
        /// 注册事件处理函数。
        /// </summary>
        /// <param name="cmd">事件类型编号。</param>
        /// <param name="userData">用户数据。</param>
        /// <param name="handler">要注册的事件处理函数。</param>
        public void Subscribe(GameEventCmd cmd, object userData, HonorEventHandler<EventParams> handler)
        {
            m_EventPool.Subscribe(cmd, userData, handler);
        }

        /// <summary>
        /// 注销事件处理函数。
        /// </summary>
        /// <param name="cmd">事件类型编号。</param>
        /// <param name="userData">用户数据。</param>
        /// <param name="handler">要取消注册的事件处理函数。</param>
        public void Unsubscribe(GameEventCmd cmd, object userData, HonorEventHandler<EventParams> handler)
        {
            m_EventPool.Unsubscribe(cmd, userData, handler);
        }

        /// <summary>
        /// 抛出事件，这个操作是线程安全的，即使不在主线程中抛出，也可保证在主线程中回调事件处理函数，但事件会在抛出后的下一帧分发。
        /// </summary>
        /// <param name="sender">事件源。</param>
        /// <param name="e">事件参数。</param>
        public void Fire(object sender, EventParams e)
        {
            m_EventPool.Fire(sender, e);
        }

        /// <summary>
        /// 抛出事件立即模式，这个操作不是线程安全的，事件会立刻分发。
        /// </summary>
        /// <param name="sender">事件源。</param>
        /// <param name="e">事件参数。</param>
        public void FireNow(object sender, EventParams e)
        {
            m_EventPool.FireNow(sender, e);
        }

    }
}


