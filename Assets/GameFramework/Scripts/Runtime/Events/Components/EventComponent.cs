/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  EventComponent.cs
 * author:    云毅
 *  created:   2026
 * descrip:   全局事件系统组件 - 核心逻辑
 ***************************************************************/

using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// 全局事件系统组件
    /// 负责管理游戏内所有事件的订阅、分发、线程安全发送
    /// 挂载于游戏入口，随框架初始化
    /// </summary>
    [DisallowMultipleComponent]
    public sealed partial class EventComponent : GameComponent
    {
        //=========================================================================
        // 生命周期
        //=========================================================================
        #region MonoBehaviour
        /// <summary>
        /// 框架初始化
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            // 实例化事件管理器
            m_EventManager = new EventManager();
            if (m_EventManager == null)
            {
                Log.Error("EventComponent -> EventManager 初始化失败！");
                return;
            }
        }

        /// <summary>
        /// 生命周期 Start（暂未使用）
        /// </summary>
        private void Start()
        {
        }

        /// <summary>
        /// 帧更新，驱动事件队列执行
        /// </summary>
        private void Update()
        {
            m_EventManager?.Update();
        }
        #endregion

        //=========================================================================
        // 公共接口 - 事件管理
        //=========================================================================
        #region Method - 事件接口
        /// <summary>
        /// 检查指定事件是否已注册对应的回调
        /// </summary>
        /// <param name="cmd">事件命令</param>
        /// <param name="userData">用户数据</param>
        /// <param name="handler">回调函数</param>
        /// <returns>是否已注册</returns>
        public bool Check(GameEventCmd cmd, object userData, HonorEventHandler<EventParams> handler)
        {
            return m_EventManager.Check(cmd, userData, handler);
        }

        /// <summary>
        /// 订阅事件
        /// </summary>
        /// <param name="cmd">事件命令</param>
        /// <param name="userData">订阅时携带的用户数据</param>
        /// <param name="handler">事件回调</param>
        public void Subscribe(GameEventCmd cmd, object userData, HonorEventHandler<EventParams> handler)
        {
            m_EventManager.Subscribe(cmd, userData, handler);
        }

        /// <summary>
        /// 取消订阅事件
        /// </summary>
        /// <param name="cmd">事件命令</param>
        /// <param name="userData">订阅时的用户数据</param>
        /// <param name="handler">要取消的回调</param>
        public void Unsubscribe(GameEventCmd cmd, object userData, HonorEventHandler<EventParams> handler)
        {
            m_EventManager.Unsubscribe(cmd, userData, handler);
        }

        /// <summary>
        /// 线程安全地抛出事件（下一帧执行）
        /// 可在子线程调用
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="cmd">事件命令</param>
        /// <param name="objects">事件参数（可选）</param>
        public void Fire(object sender, GameEventCmd cmd, Dictionary<string, object> objects = null)
        {
            m_EventManager.Fire(sender, new EventParams(cmd, objects));
        }

        /// <summary>
        /// 立即抛出事件（当前帧同步执行）
        /// 非线程安全
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="cmd">事件命令</param>
        /// <param name="objects">事件参数（可选）</param>
        public void FireNow(object sender, GameEventCmd cmd, Dictionary<string, object> objects = null)
        {
            m_EventManager.FireNow(sender, new EventParams(cmd, objects));
        }
        #endregion
    }
}