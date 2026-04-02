using System.Collections.Generic;

namespace Honor.Runtime
{

    public delegate void HonorEventHandler<TEventArgs>(object sender, object userData, TEventArgs e);

    public sealed partial class EventPool<T> where T : EventParams
    {
        /// <summary>
        /// 处理事件结点。
        /// </summary>
        /// <param name="sender">事件源。</param>
        /// <param name="e">事件参数。</param>
        private void HandleEvent(object sender, T e)
        {
            GameLinkedListRange<Dictionary<object, HonorEventHandler<T>>> range = null;
            if (m_SubscribedEventHandlers.TryGetValue(e.Cmd, out range))
            {
                LinkedListNode<Dictionary<object, HonorEventHandler<T>>> current = range.First;
                while (current != null && current != range.Terminal)
                {
                    foreach (KeyValuePair<object, HonorEventHandler<T>> itr in current.Value)
                    {
                        itr.Value(sender, itr.Key, e);
                    }
                    current = m_CachedNodes[e] = current.Next != range.Terminal ? current.Next : null;
                }
                m_CachedNodes.Remove(e);
            }

            // 转发事件到Lua层全局接口
            if (m_LuaComponent.LuaReceiveEventCSEventDelegate != null)
            {
                m_LuaComponent.LuaReceiveEventCSEventDelegate(e);
            }
        }
    }

}


