/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  LuaProtocolUtil.cs
 * author:    云毅
 * created:   2026
 * descrip:   协议公共工具类（对齐 SDK SGSDKDataBaseMap / SGSDKBrige 工具层）
 *            收拢 cmd 校验 / 数据构建 / Lua 广播 / 委托安全调用等公共方法
 *            供 Bridge / Dispatcher / DataMap 等协议模块复用
 ***************************************************************/

using System;

namespace Honor.Runtime
{
    /// <summary>
    /// 协议公共工具类：收拢各协议模块（Bridge / Dispatcher / DataMap / RunTime）复用的公共方法
    /// </summary>
    public static class LuaProtocolUtil
    {
        //=========================================================================
        #region cmd 校验
        //=========================================================================

        /// <summary>
        /// 校验协议号是否已注册强类型数据类
        /// </summary>
        /// <param name="cmd">协议号</param>
        /// <returns>是否有效（已注册数据类）</returns>
        public static bool IsValidCmd(int cmd)
        {
            return LuaProtocolDataMap.ContainsCmd(cmd);
        }

        #endregion

        //=========================================================================
        #region 数据构建
        //=========================================================================

        /// <summary>
        /// 按协议号构建强类型数据类实例（对齐 SDK SGSDKDataBaseMap 反序列化思路）
        /// </summary>
        /// <param name="cmd">协议号</param>
        /// <returns>数据类实例；未注册返回 null</returns>
        public static LuaProtocolDataBase BuildData(int cmd)
        {
            Type type = LuaProtocolDataMap.GetDataTypeByCmd(cmd);
            return type == null ? null : (LuaProtocolDataBase)Activator.CreateInstance(type);
        }

        #endregion

        //=========================================================================
        #region Lua 广播
        //=========================================================================

        /// <summary>
        /// 以成功状态广播到 Lua（收拢 Bridge / Dispatcher 重复的 NotifyLua(Success) 调用）
        /// </summary>
        /// <param name="cmd">协议号</param>
        /// <param name="content">内容</param>
        public static void BroadcastToLua(int cmd, object content)
        {
            LuaProtocolRunTime.NotifyLua(cmd, content, (int)LuaProtocolDispatcher.LuaProtocolReceiveType.Success);
        }

        #endregion

        //=========================================================================
        #region 委托安全调用
        //=========================================================================

        /// <summary>
        /// 安全调用推送回调（推送委托签名：cmd + content）
        /// </summary>
        /// <param name="handler">推送回调</param>
        /// <param name="cmd">协议号</param>
        /// <param name="content">内容</param>
        public static void SafeInvoke(LuaProtocolPushFun handler, int cmd, object content)
        {
            handler?.Invoke(cmd, content);
        }

        /// <summary>
        /// 安全调用发送回调（发送委托签名：cmd + content + state）
        /// </summary>
        /// <param name="handler">发送回调</param>
        /// <param name="cmd">协议号</param>
        /// <param name="content">内容</param>
        /// <param name="state">状态</param>
        public static void SafeInvoke(LuaProtocolSendFun handler, int cmd, object content, int state)
        {
            handler?.Invoke(cmd, content, state);
        }

        #endregion
    }
}
