/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  EventPool.Handle.cs
 * author:    云毅
 *  created:   2026
 * descrip:   泛型事件池 - 事件派发核心逻辑（partial）
 ***************************************************************/

using System.Collections.Generic;

namespace Honor.Runtime
{
    /// <summary>
    /// 通用事件处理委托
    /// </summary>
    /// <typeparam name="TEventArgs">事件参数类型</typeparam>
    /// <param name="sender">事件发送者</param>
    /// <param name="userData">订阅时传入的用户数据</param>
    /// <param name="e">事件参数</param>
    public delegate void HonorEventHandler<TEventArgs>(object sender, object userData, TEventArgs e);

    /// <summary>
    /// 泛型事件池 - 事件派发处理部分
    /// </summary>
    public sealed partial class EventPool<T> where T : EventParams
    {
        //=========================================================================
        // 事件派发核心逻辑
        //=========================================================================
        #region Private Methods - HandleEvent
        /// <summary>
        /// 事件派发核心方法
        /// 遍历执行所有已注册的事件回调，并将事件转发至 Lua 脚本层
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        private void HandleEvent(object sender, T e)
        {
            // 获取当前事件对应的回调链表
            if (m_SubscribedEventHandlers.TryGetValue(e.Cmd, out GameLinkedListRange<Dictionary<object, HonorEventHandler<T>>> range))
            {
                LinkedListNode<Dictionary<object, HonorEventHandler<T>>> current = range.First;

                // 遍历所有回调并执行
                while (current != null && current != range.Terminal)
                {
                    foreach (KeyValuePair<object, HonorEventHandler<T>> itr in current.Value)
                    {
                        itr.Value(sender, itr.Key, e);
                    }

                    // 更新缓存节点，支持安全遍历
                    current = m_CachedNodes[e] = current.Next != range.Terminal ? current.Next : null;
                }

                // 清理当前事件的缓存节点
                m_CachedNodes.Remove(e);
            }

            // 将事件转发到 Lua 全局接收接口
            if (m_LuaComponent != null && m_LuaComponent.LuaReceiveEventCSEventDelegate != null)
            {
                m_LuaComponent.LuaReceiveEventCSEventDelegate(e);
            }
        }
        #endregion
    }
}