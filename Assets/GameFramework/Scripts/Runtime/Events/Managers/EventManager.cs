/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  EventManager.cs
 * author:    云毅
 * created: 2025
 * descrip:   全局事件管理器 - 对外入口层
 ***************************************************************/

namespace Honor.Runtime
{
    /// <summary>
    /// 事件管理器（全局入口）
    /// 提供事件订阅、取消、派发、线程安全发送等接口，封装底层 EventPool
    /// </summary>
    public sealed partial class EventManager
    {
        //=========================================================================
        // 构造函数
        //=========================================================================
        #region Constructor
        /// <summary>
        /// 初始化事件管理器
        /// </summary>
        public EventManager()
        {
            m_EventPool = new EventPool<EventParams>();
        }
        #endregion

        //=========================================================================
        // 生命周期
        //=========================================================================
        #region Lifecycle
        /// <summary>
        /// 帧更新，驱动事件池执行事件派发
        /// </summary>
        public void Update()
        {
            m_EventPool.Update();
        }

        /// <summary>
        /// 关闭并清理事件管理器
        /// </summary>
        public void Shutdown()
        {
            m_EventPool.Shutdown();
        }
        #endregion

        //=========================================================================
        // 公共事件接口
        //=========================================================================
        #region Public Event Methods
        /// <summary>
        /// 检查指定事件是否已注册对应的回调
        /// </summary>
        /// <param name="cmd">事件命令 ID</param>
        /// <param name="userData">用户数据</param>
        /// <param name="handler">事件回调</param>
        /// <returns>是否已注册</returns>
        public bool Check(GameEventCmd cmd, object userData, HonorEventHandler<EventParams> handler)
        {
            return m_EventPool.Check(cmd, userData, handler);
        }

        /// <summary>
        /// 订阅事件
        /// </summary>
        /// <param name="cmd">事件命令 ID</param>
        /// <param name="userData">订阅时携带的用户数据</param>
        /// <param name="handler">事件回调</param>
        public void Subscribe(GameEventCmd cmd, object userData, HonorEventHandler<EventParams> handler)
        {
            m_EventPool.Subscribe(cmd, userData, handler);
        }

        /// <summary>
        /// 取消订阅事件
        /// </summary>
        /// <param name="cmd">事件命令 ID</param>
        /// <param name="userData">订阅时的用户数据</param>
        /// <param name="handler">要取消的回调</param>
        public void Unsubscribe(GameEventCmd cmd, object userData, HonorEventHandler<EventParams> handler)
        {
            m_EventPool.Unsubscribe(cmd, userData, handler);
        }

        /// <summary>
        /// 线程安全抛出事件（下一帧执行）
        /// 可在子线程安全调用
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        public void Fire(object sender, EventParams e)
        {
            m_EventPool.Fire(sender, e);
        }

        /// <summary>
        /// 立即抛出事件（同步执行，非线程安全）
        /// 只能在主线程调用
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">事件参数</param>
        public void FireNow(object sender, EventParams e)
        {
            m_EventPool.FireNow(sender, e);
        }
        #endregion
    }
}