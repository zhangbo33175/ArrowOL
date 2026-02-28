using System.Collections.Generic;

namespace Honor.Runtime
{
    public sealed partial class EventManager:Singleton<EventManager>
    {
         /// <summary>
        /// 事件池
        /// </summary>
        public static EventPool<EventParams> m_EventPool;

        /// <summary>
        ///  实例化对象池
        /// </summary>
        public EventManager()
        {
            m_EventPool = new EventPool<EventParams>();
        }
        
        /// <summary>
        /// 检查是否存在事件处理函数。
        /// </summary>
        /// <param name="cmd">事件类型编号。</param>
        /// <param name="userData">用户数据。</param>
        /// <param name="handler">要检查的事件处理函数。</param>
        /// <returns>是否存在事件处理函数。</returns>
        public bool Check(EventCmd cmd, object userData, GameEventHandler<EventParams> handler)
        {
            return m_EventPool.On_Check_Event(cmd, userData, handler);
        }

        /// <summary>
        /// 注册事件处理回调函数。
        /// </summary>
        /// <param name="cmd">事件类型编号。</param>
        /// <param name="userData">用户数据。</param>
        /// <param name="handler">要注册的事件处理回调函数。</param>
        public void AddEvent(EventCmd cmd, object userData, GameEventHandler<EventParams> handler)
        {
            m_EventPool.Add_Event(cmd, userData, handler);
        }

        /// <summary>
        /// 注销事件处理回调函数。
        /// </summary>
        /// <param name="cmd">事件类型编号。</param>
        /// <param name="userData">用户数据。</param>
        /// <param name="handler">要注销的事件处理回调函数。</param>
        public void RemoveEvent(EventCmd cmd, object userData, GameEventHandler<EventParams> handler)
        {
            m_EventPool.Remove_Event(cmd, userData, handler);
        }

        /// <summary>
        /// 抛出事件，这个操作是线程安全的，即使不在主线程中抛出，也可保证在主线程中回调事件处理函数，但事件会在抛出后的下一帧分发。
        /// </summary>
        /// <param name="sender">事件发送者。</param>
        /// <param name="cmd">事件类型。</param>
        /// <param name="objects">事件参数。</param>
        public void DispatchEvent(object sender, EventCmd cmd, Dictionary<string, object> objects = null)
        {
            m_EventPool.Dispatch_Event(sender, new EventParams(cmd, objects));
        }

        /// <summary>
        /// 抛出事件立即模式，这个操作不是线程安全的，事件会立刻分发。
        /// </summary>
        /// <param name="sender">事件发送者。</param>
        /// <param name="cmd">事件类型。</param>
        /// <param name="objects">事件参数。</param>
        public void DispatchEventNow(object sender, EventCmd cmd, Dictionary<string, object> objects = null)
        {
            m_EventPool.Dispatch_Event_Now(sender, new EventParams(cmd, objects));
        }

        /// <summary>
        /// 游戏框架模块轮询。
        /// </summary>
        public void Update()
        {
            m_EventPool.Update();
        }

        /// <summary>
        /// 关闭并清理游戏框架模块。
        /// </summary>
        public void Shutdown()
        {
            m_EventPool.Shutdown();
        }

        public override void Dispose()
        {
            
        }
    }
}