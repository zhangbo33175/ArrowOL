/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  LuaProtocolDispatcher.cs
 * author:    云毅
 * created:   2026
 * descrip:   协议分发中心（对齐 SDK SGSdkBase）
 *            请求队列（一对多）+ 推送表 + 跨语言 Lua 广播
 ***************************************************************/

using System.Collections.Generic;
using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// 协议分发中心（对齐 SGSdkBase）：
    /// 按 cmd 分发请求回调 / 推送回调 / 跨语言 Lua 通知
    /// </summary>
    public class LuaProtocolDispatcher
    {
        //=========================================================================
        #region 字段
        //=========================================================================

        /// <summary>
        /// 扩展请求队列（对齐 SGSdkBase.extendRequests）：
        /// cmd → 请求回调队列（一对多，网络类消息）
        /// </summary>
        private readonly Dictionary<int, Queue<LuaProtocolSendFun>> m_ExtendRequests = new Dictionary<int, Queue<LuaProtocolSendFun>>();

        /// <summary>
        /// 推送回调表（对齐 SGSdkBase.pushMetods）：cmd → 推送回调
        /// </summary>
        private readonly Dictionary<int, LuaProtocolPushFun> m_PushMethods = new Dictionary<int, LuaProtocolPushFun>();

        #endregion

        //=========================================================================
        #region 请求 / 接收
        //=========================================================================

        /// <summary>
        /// 发送请求：入队回调并下发（对齐 SGSdkBase.Request）
        /// </summary>
        /// <param name="cmd">协议号</param>
        /// <param name="content">请求内容</param>
        /// <param name="requestFun">请求回调</param>
        public virtual void Request(int cmd, object content, LuaProtocolSendFun requestFun)
        {
            if (requestFun != null)
            {
                if (m_ExtendRequests.TryGetValue(cmd, out Queue<LuaProtocolSendFun> queue))
                {
                    queue.Enqueue(requestFun);
                }
                else
                {
                    Queue<LuaProtocolSendFun> newQueue = new Queue<LuaProtocolSendFun>();
                    newQueue.Enqueue(requestFun);
                    m_ExtendRequests.Add(cmd, newQueue);
                }
            }

            OnRequestSend(cmd, content);
        }

        /// <summary>
        /// 请求下发展开点：子类可重写，默认直接广播到 Lua
        /// </summary>
        /// <param name="cmd">协议号</param>
        /// <param name="content">请求内容</param>
        protected virtual void OnRequestSend(int cmd, object content)
        {
            LuaProtocolUtil.BroadcastToLua(cmd, content);
        }

        /// <summary>
        /// 接收成功（对齐 SGSdkBase.ReceiveSuccess）
        /// </summary>
        /// <param name="cmd">协议号</param>
        /// <param name="content">内容</param>
        public virtual void ReceiveSuccess(int cmd, object content)
        {
            Receive(cmd, content, (int)LuaProtocolReceiveType.Success);
        }

        /// <summary>
        /// 接收失败（对齐 SGSdkBase.ReceiveFail）
        /// </summary>
        /// <param name="cmd">协议号</param>
        /// <param name="content">内容</param>
        public virtual void ReceiveFail(int cmd, object content)
        {
            Receive(cmd, content, (int)LuaProtocolReceiveType.Fail);
        }

        /// <summary>
        /// 消费请求队列中对应 cmd 的回调（对齐 SGSdkBase.Receive 私有方法）
        /// </summary>
        /// <param name="cmd">协议号</param>
        /// <param name="content">内容</param>
        /// <param name="state">状态</param>
        private void Receive(int cmd, object content, int state)
        {
            if (m_ExtendRequests.TryGetValue(cmd, out Queue<LuaProtocolSendFun> queue))
            {
                LuaProtocolUtil.SafeInvoke(queue.Dequeue(), cmd, content, state);

                // 队列消费完则移除该 cmd
                if (queue.Count <= 0)
                {
                    m_ExtendRequests.Remove(cmd);
                }
            }
            else
            {
                Debug.LogError($"[LuaProtocolDispatcher] 未找到匹配的请求信息 cmd: {cmd}");
            }
        }

        #endregion

        //=========================================================================
        #region 推送
        //=========================================================================

        /// <summary>
        /// 绑定 C# 推送监听（对齐 SGSdkBase.BindPushMethod）
        /// </summary>
        /// <param name="cmd">协议号</param>
        /// <param name="bind">推送回调</param>
        public virtual void BindPushMethod(int cmd, LuaProtocolPushFun bind)
        {
            if (!m_PushMethods.ContainsKey(cmd))
            {
                m_PushMethods.Add(cmd, bind);
            }
        }

        /// <summary>
        /// 解除推送监听（对齐 SGSdkBase.RemovePushMethod）
        /// </summary>
        /// <param name="cmd">协议号</param>
        public virtual void RemovePushMethod(int cmd)
        {
            m_PushMethods.Remove(cmd);
        }

        /// <summary>
        /// 执行推送：有 C# 监听则回调，否则广播到 Lua（对齐 SGSdkBase.CallPushMethod）
        /// </summary>
        /// <param name="cmd">协议号</param>
        /// <param name="content">内容</param>
        public virtual void CallPushMethod(int cmd, object content)
        {
            if (m_PushMethods.TryGetValue(cmd, out LuaProtocolPushFun func))
            {
                LuaProtocolUtil.SafeInvoke(func, cmd, content);
            }
            else
            {
                LuaProtocolUtil.BroadcastToLua(cmd, content);
            }
        }

        #endregion

        //=========================================================================
        #region 清理 / 枚举
        //=========================================================================

        /// <summary>
        /// 清理全部请求队列与推送表（对齐 SGSdkBase.ClearRequests）
        /// </summary>
        public virtual void ClearRequests()
        {
            m_ExtendRequests.Clear();
            m_PushMethods.Clear();
        }

        /// <summary>
        /// 协议接收类型（对齐 SGSdkBase.eSDKReceiveType）
        /// </summary>
        public enum LuaProtocolReceiveType : byte
        {
            /// <summary>失败</summary>
            Fail = 0,

            /// <summary>成功</summary>
            Success = 1,
        }

        #endregion
    }
}
