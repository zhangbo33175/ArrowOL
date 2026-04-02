using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Honor.Runtime
{
    [DisallowMultipleComponent]
    public sealed partial class EventComponent : GameComponent
    {
        /// <summary>
        /// 游戏框架组件初始化。
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            // 实例化Event管理器
            m_EventManager = new EventManager();
            if (m_EventManager == null)
            {
                Log.Error("EventManager 无效。");
                return;
            }

        }

        private void Start()
        {

        }

        private void Update()
        {
            if (m_EventManager != null)
            {
                m_EventManager.Update();
            }
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
            return m_EventManager.Check(cmd, userData, handler);
        }

        /// <summary>
        /// 注册事件处理回调函数。
        /// </summary>
        /// <param name="cmd">事件类型编号。</param>
        /// <param name="userData">用户数据。</param>
        /// <param name="handler">要注册的事件处理回调函数。</param>
        public void Subscribe(GameEventCmd cmd, object userData, HonorEventHandler<EventParams> handler)
        {
            m_EventManager.Subscribe(cmd, userData, handler);
        }

        /// <summary>
        /// 注销事件处理回调函数。
        /// </summary>
        /// <param name="cmd">事件类型编号。</param>
        /// <param name="userData">用户数据。</param>
        /// <param name="handler">要注销的事件处理回调函数。</param>
        public void Unsubscribe(GameEventCmd cmd, object userData, HonorEventHandler<EventParams> handler)
        {
            m_EventManager.Unsubscribe(cmd, userData, handler);
        }

        /// <summary>
        /// 抛出事件，这个操作是线程安全的，即使不在主线程中抛出，也可保证在主线程中回调事件处理函数，但事件会在抛出后的下一帧分发。
        /// </summary>
        /// <param name="sender">事件发送者。</param>
        /// <param name="cmd">事件类型。</param>
        /// <param name="objects">事件参数。</param>
        public void Fire(object sender, GameEventCmd cmd, Dictionary<string, object> objects = null)
        {
            m_EventManager.Fire(sender, new EventParams(cmd, objects));
        }

        /// <summary>
        /// 抛出事件立即模式，这个操作不是线程安全的，事件会立刻分发。
        /// </summary>
        /// <param name="sender">事件发送者。</param>
        /// <param name="cmd">事件类型。</param>
        /// <param name="objects">事件参数。</param>
        public void FireNow(object sender, GameEventCmd cmd, Dictionary<string, object> objects = null)
        {
            m_EventManager.FireNow(sender, new EventParams(cmd, objects));
        }


    }
}


