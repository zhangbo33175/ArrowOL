/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  LuaProtocolBridge.cs
 * author:    云毅
 * created:   2026
 * descrip:   协议桥（对齐 SDK SGSDKBrige 的 cmd → handler 路由思想）
 *            注册 C# 侧 cmd 处理方，未命中则广播到 Lua
 *            作为 C# 事件 → cmd 统一分发的入口
 ***************************************************************/

using System.Collections.Generic;

namespace Honor.Runtime
{
    /// <summary>
    /// 协议桥：
    /// 通过 RegisterHandler 注册 cmd 处理方（C# 侧消费），
    /// SendFuncMessage 按 cmd 路由，未命中回退到 Lua 广播（对齐 SGSDKBrige）
    /// </summary>
    public static class LuaProtocolBridge
    {
        //=========================================================================
        #region 字段
        //=========================================================================

        /// <summary>
        /// cmd → C# 处理回调表（对齐 SGSDKBrige.csHandlerMaps）
        /// </summary>
        private static readonly Dictionary<int, LuaProtocolPushFun> s_Handlers = new Dictionary<int, LuaProtocolPushFun>();

        #endregion

        //=========================================================================
        #region 注册 / 注销
        //=========================================================================

        /// <summary>
        /// 注册 C# 侧 cmd 处理方（对齐 SGSDKBrige.BindCommand 的路由思想）
        /// </summary>
        /// <param name="cmd">协议号</param>
        /// <param name="handler">处理回调</param>
        public static void RegisterHandler(int cmd, LuaProtocolPushFun handler)
        {
            if (handler == null)
            {
                return;
            }

            s_Handlers[cmd] = handler;
        }

        /// <summary>
        /// 注销 C# 侧 cmd 处理方
        /// </summary>
        /// <param name="cmd">协议号</param>
        public static void UnregisterHandler(int cmd)
        {
            s_Handlers.Remove(cmd);
        }

        /// <summary>
        /// 清理全部处理方
        /// </summary>
        public static void ClearHandlers()
        {
            s_Handlers.Clear();
        }

        #endregion

        //=========================================================================
        #region 消息路由
        //=========================================================================

        /// <summary>
        /// 下发命令（对齐 SGSDKBrige.SendFuncMessage）：
        /// 命中 C# 处理方则回调；否则广播到 Lua
        /// </summary>
        /// <param name="cmd">协议号</param>
        /// <param name="content">内容（数据类实例或原始数据）</param>
        /// <returns>是否被 C# 侧消费</returns>
        public static bool SendFuncMessage(int cmd, object content)
        {
            if (s_Handlers.TryGetValue(cmd, out LuaProtocolPushFun handler))
            {
                LuaProtocolUtil.SafeInvoke(handler, cmd, content);
                return true;
            }

            // 未命中 → 回退到 Lua 统一广播（对齐 SGSDKBrige 未命中路径）
            LuaProtocolUtil.BroadcastToLua(cmd, content);
            return false;
        }

        /// <summary>
        /// 接收回调命令（对齐 SGSDKBrige.ReceiveFuncMessage）
        /// </summary>
        /// <param name="cmd">协议号</param>
        /// <param name="content">内容</param>
        /// <returns>是否被 C# 侧消费</returns>
        public static bool ReceiveFuncMessage(int cmd, object content)
        {
            return SendFuncMessage(cmd, content);
        }

        /// <summary>
        /// 按协议号创建强类型数据类实例（转发 LuaProtocolUtil.BuildData）
        /// </summary>
        /// <param name="cmd">协议号</param>
        /// <returns>数据类实例；未注册返回 null</returns>
        public static LuaProtocolDataBase BuildData(int cmd)
        {
            return LuaProtocolUtil.BuildData(cmd);
        }

        #endregion
    }
}
