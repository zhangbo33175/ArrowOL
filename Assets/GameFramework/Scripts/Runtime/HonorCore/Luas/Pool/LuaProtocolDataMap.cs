/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  LuaProtocolDataMap.cs
 * author:    云毅
 * created:   2026
 * descrip:   协议 cmd → 强类型数据类 映射（对齐 SDK SGSDKDataBaseMap）
 *            按 cmd 自动路由到对应数据类 Type
 ***************************************************************/

using System;
using System.Collections.Generic;

namespace Honor.Runtime
{
    /// <summary>
    /// 协议命令 → 数据类类型 映射表（对齐 SGSDKDataBaseMap）
    /// </summary>
    public static class LuaProtocolDataMap
    {
        /// <summary>
        /// cmd → 数据类 Type 映射（与 Protocol.pb 各协议块关联）
        /// </summary>
        private static readonly Dictionary<int, Type> m_DataMap = new Dictionary<int, Type>()
        {
            // ---- ① 类创建 / 生命周期 ----
            [(int)LuaProtocolCmd.P_LuaCreateLuaClassFromCSEventDelegate] = typeof(CreateLuaClass),
            [(int)LuaProtocolCmd.P_LuaCreateProcedureLuaClassFromCSEventDelegate] = typeof(CreateProcedureLuaClass),

            // ---- ② 应用 / 输入 ----
            [(int)LuaProtocolCmd.P_LuaApplicationPauseFromCSEventDelegate] = typeof(ApplicationPause),
            [(int)LuaProtocolCmd.P_LuaApplicationQuitFromCSEventDelegate] = typeof(ApplicationQuit),
            [(int)LuaProtocolCmd.P_LuaKeysUpFromCSEventDelegate] = typeof(KeysUp),

            // ---- ③ 登录授权 ----
            [(int)LuaProtocolCmd.P_LuaSignInWithAppleCSEventDelegate] = typeof(SignInWithApple),
            [(int)LuaProtocolCmd.P_LuaSignInWithAppleStateCSEventDelegate] = typeof(SignInWithAppleState),

            // ---- ④ 事件 / 资源 / 本地化 ----
            [(int)LuaProtocolCmd.P_LuaReceiveEventCSEventDelegate] = typeof(ReceiveEvent),
            [(int)LuaProtocolCmd.P_LuaRelateLocalizationTableDataFromCSEventDelegate] = typeof(RelateLocalizationTableData),
            [(int)LuaProtocolCmd.P_LuaGetResDefInfoEventDelegate] = typeof(GetResDefInfo),
            [(int)LuaProtocolCmd.P_LuaLocalizingCSEventDelegate] = typeof(Localizing),

            // ---- ⑤ WebSocket ----
            [(int)LuaProtocolCmd.P_LuaWebSocketOpenCSEventDelegate] = typeof(WebSocketOpen),
            [(int)LuaProtocolCmd.P_LuaWebSocketMessageReceivedCSEventDelegate] = typeof(WebSocketMessageReceived),
            [(int)LuaProtocolCmd.P_LuaWebSocketBinaryReceivedCSEventDelegate] = typeof(WebSocketBinaryReceived),
            [(int)LuaProtocolCmd.P_LuaWebSocketClosedCSEventDelegate] = typeof(WebSocketClosed),
            [(int)LuaProtocolCmd.P_LuaWebSocketErrorCSEventDelegate] = typeof(WebSocketError),
        };

        /// <summary>
        /// 按协议号获取对应数据类类型（对齐 SGSDKDataBaseMap.GetSCTypeByProtoNum）
        /// </summary>
        /// <param name="cmd">协议号</param>
        /// <returns>数据类 Type；未注册返回 null</returns>
        public static Type GetDataTypeByCmd(int cmd)
        {
            m_DataMap.TryGetValue(cmd, out Type result);
            return result;
        }

        /// <summary>
        /// 是否存在该协议号
        /// </summary>
        /// <param name="cmd">协议号</param>
        /// <returns>是否存在</returns>
        public static bool ContainsCmd(int cmd)
        {
            return m_DataMap.ContainsKey(cmd);
        }
    }
}
