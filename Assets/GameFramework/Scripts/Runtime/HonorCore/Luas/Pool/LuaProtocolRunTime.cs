/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  LuaProtocolRunTime.cs
 * author:    云毅
 * created:   2026
 * descrip:   协议静态运行时门面（对齐 SDK SGSdkRunTime）
 *            跨语言统一入口：静态 Dispatcher + Lua 广播句柄
 ***************************************************************/

using System;
using XLua;

namespace Honor.Runtime
{
    /// <summary>
    /// 协议静态运行时门面（对齐 SGSdkRunTime）：
    /// 持有分发中心实例，并提供静态跨语言转发入口
    /// </summary>
    [CSharpCallLua]
    public static class LuaProtocolRunTime
    {
        /// <summary>
        /// 分发中心实例（对齐 SGSdkRunTime.Sdk）
        /// </summary>
        public static LuaProtocolDispatcher Dispatcher { get; private set; }

        /// <summary>
        /// Lua 消息广播通知句柄（对齐 SGSdkRunTime.luaNotifyHandle）：
        /// 由 Lua 侧绑定全局回调，实现 C# → Lua 广播
        /// </summary>
        public static LuaProtocolSendFun LuaNotifyHandle;

        /// <summary>
        /// Lua 同步调用句柄：
        /// 由 Lua 侧绑定全局回调，调用栈内立即返回结果（创建 Lua 类 / 获取资源信息 / 本地化文本等）
        /// </summary>
        public static LuaProtocolSyncFun LuaSyncHandle;

        /// <summary>
        /// 初始化分发中心（对齐 SGSdkRunTime.Initialize）
        /// </summary>
        /// <param name="dispatcher">分发中心实例</param>
        public static void Initialize(LuaProtocolDispatcher dispatcher)
        {
            if (dispatcher == null)
            {
                throw new ArgumentNullException(nameof(dispatcher));
            }

            if (Dispatcher != null)
            {
                throw new NotSupportedException("[LuaProtocolRunTime] 重复初始化");
            }

            Dispatcher = dispatcher;
        }

        /// <summary>
        /// 卸载并清理（对齐 SGSdkRunTime.Release）
        /// </summary>
        public static void Release()
        {
            LuaNotifyHandle = null;
            LuaSyncHandle = null;
            Dispatcher?.ClearRequests();
        }

        /// <summary>
        /// 发送请求（对齐 SGSdkRunTime.Request）
        /// </summary>
        /// <param name="cmd">协议号</param>
        /// <param name="content">请求内容</param>
        /// <param name="requestFun">请求回调</param>
        public static void Request(int cmd, object content, LuaProtocolSendFun requestFun)
        {
            Dispatcher.Request(cmd, content, requestFun);
        }

        /// <summary>
        /// 接收成功（对齐 SGSdkRunTime.ReceiveSuccess）
        /// </summary>
        /// <param name="cmd">协议号</param>
        /// <param name="content">内容</param>
        public static void ReceiveSuccess(int cmd, object content)
        {
            Dispatcher.ReceiveSuccess(cmd, content);
        }

        /// <summary>
        /// 接收失败（对齐 SGSdkRunTime.ReceiveFail）
        /// </summary>
        /// <param name="cmd">协议号</param>
        /// <param name="content">内容</param>
        public static void ReceiveFail(int cmd, object content)
        {
            Dispatcher.ReceiveFail(cmd, content);
        }

        /// <summary>
        /// 绑定 C# 推送监听（对齐 SGSdkRunTime.BindPushMethod）
        /// </summary>
        /// <param name="cmd">协议号</param>
        /// <param name="bind">推送回调</param>
        public static void BindPushMethod(int cmd, LuaProtocolPushFun bind)
        {
            Dispatcher.BindPushMethod(cmd, bind);
        }

        /// <summary>
        /// 解除推送监听（对齐 SGSdkRunTime.RemovePushMethod）
        /// </summary>
        /// <param name="cmd">协议号</param>
        public static void RemovePushMethod(int cmd)
        {
            Dispatcher.RemovePushMethod(cmd);
        }

        /// <summary>
        /// 执行推送（对齐 SGSdkRunTime.CallPushMethod）
        /// </summary>
        /// <param name="cmd">协议号</param>
        /// <param name="content">内容</param>
        public static void CallPushMethod(int cmd, object content)
        {
            Dispatcher.CallPushMethod(cmd, content);
        }

        /// <summary>
        /// 广播通知到 Lua（对齐 SGSdkRunTime.luaNotifyHandle 调用点）
        /// </summary>
        /// <param name="cmd">协议号</param>
        /// <param name="content">内容</param>
        /// <param name="state">状态</param>
        public static void NotifyLua(int cmd, object content, int state)
        {
            LuaNotifyHandle?.Invoke(cmd, content, state);
        }

        /// <summary>
        /// 同步调用 Lua 并立即获取结果（创建 Lua 类 / 获取资源信息 / 本地化文本等）
        /// </summary>
        /// <param name="cmd">协议号</param>
        /// <param name="content">协议内容（强类型数据类）</param>
        /// <returns>Lua 处理结果（LuaTable / string / 其它 object）；未绑定同步句柄返回 null</returns>
        public static object SyncCall(int cmd, object content)
        {
            return LuaSyncHandle?.Invoke(cmd, content);
        }
    }
}
