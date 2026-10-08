/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  LuaProtocolCmd.cs
 * author:    云毅
 * created:   2026
 * descrip:   C# ⇄ Lua 通信委托协议链接表（cmd 定义链接 ID）
 *            为 Luas 目录内所有 [CSharpCallLua] 通信委托分配固定 cmd，
 *            每个委托对应一个 cmd（链接 ID），用于统一协议分发与自动生成链接。
 *            枚举可编译（供 C# 分发中心 / Lua 注册表使用）；
 *            委托签名以注释与枚举项配对（对齐工程现有委托，避免重复定义）。
 ***************************************************************/

namespace Honor.Runtime
{
    //=========================================================================
    #region C# ⇄ Lua 通信委托协议链接 ID（cmd）
    //=========================================================================
    // cmd 分配规则：
    //   - 固定 int（服务器/客户端约定，跨进程一致；不可用运行时 UniqueEventCode）
    //   - 区间：1001-1010 类创建/生命周期 | 1011-1020 应用/输入 | 1021-1030 登录授权
    //           1031-1040 事件/资源/本地化 | 1041-1050 WebSocket
    //   - 实际 cmd 值需与服务器/后台确认后固化，此处为示例占位
    //=========================================================================

    /// <summary>
    /// C# ⇄ Lua 通信委托协议命令 ID
    /// </summary>
    public enum LuaProtocolCmd
    {
        /// <summary>无效 / 默认</summary>
        None = 0,

        // --- 类创建 / 生命周期（1001-1010） ---
        // P_LuaCreateLuaClassFromCSEventDelegate = 1001;
        // public delegate LuaTable LuaCreateLuaClassFromCSEventDelegate(LuaTable env, string luaScriptName);
        P_LuaCreateLuaClassFromCSEventDelegate = 1001,

        // P_LuaCreateProcedureLuaClassFromCSEventDelegate = 1002;
        // public delegate LuaTable LuaCreateProcedureLuaClassFromCSEventDelegate(LuaTable env, string luaScriptName);
        P_LuaCreateProcedureLuaClassFromCSEventDelegate = 1002,

        // --- 应用 / 输入（1011-1020） ---
        // P_LuaApplicationPauseFromCSEventDelegate = 1011;
        // public delegate void LuaApplicationPauseFromCSEventDelegate(bool pause);
        P_LuaApplicationPauseFromCSEventDelegate = 1011,

        // P_LuaApplicationQuitFromCSEventDelegate = 1012;
        // public delegate void LuaApplicationQuitFromCSEventDelegate();
        P_LuaApplicationQuitFromCSEventDelegate = 1012,

        // P_LuaKeysUpFromCSEventDelegate = 1013;
        // public delegate void LuaKeysUpFromCSEventDelegate(KeyCode keyCode);
        P_LuaKeysUpFromCSEventDelegate = 1013,

        // --- 登录授权（1021-1030） ---
        // P_LuaSignInWithAppleCSEventDelegate = 1021;
        // public delegate void LuaSignInWithAppleCSEventDelegate(LuaTable resultTable);
        P_LuaSignInWithAppleCSEventDelegate = 1021,

        // P_LuaSignInWithAppleStateCSEventDelegate = 1022;
        // public delegate void LuaSignInWithAppleStateCSEventDelegate(LuaTable stateTable);
        P_LuaSignInWithAppleStateCSEventDelegate = 1022,

        // --- 事件 / 资源 / 本地化（1031-1040） ---
        // P_LuaReceiveEventCSEventDelegate = 1031;
        // public delegate void LuaReceiveEventCSEventDelegate(EventParams eventArgs);
        P_LuaReceiveEventCSEventDelegate = 1031,

        // P_LuaRelateLocalizationTableDataFromCSEventDelegate = 1032;
        // public delegate void LuaRelateLocalizationTableDataFromCSEventDelegate();
        P_LuaRelateLocalizationTableDataFromCSEventDelegate = 1032,

        // P_LuaGetResDefInfoEventDelegate = 1033;
        // public delegate LuaTable LuaGetResDefInfoEventDelegate(string name);
        P_LuaGetResDefInfoEventDelegate = 1033,

        // P_LuaLocalizingCSEventDelegate = 1034;
        // public delegate string LuaLocalizingCSEventDelegate(string localizingKeyName);
        P_LuaLocalizingCSEventDelegate = 1034,

        // --- WebSocket（1041-1050） ---
        // P_LuaWebSocketOpenCSEventDelegate = 1041;
        // public delegate void LuaWebSocketOpenCSEventDelegate(WebSocket ws);
        P_LuaWebSocketOpenCSEventDelegate = 1041,

        // P_LuaWebSocketMessageReceivedCSEventDelegate = 1042;
        // public delegate void LuaWebSocketMessageReceivedCSEventDelegate(WebSocket ws, string message);
        P_LuaWebSocketMessageReceivedCSEventDelegate = 1042,

        // P_LuaWebSocketBinaryReceivedCSEventDelegate = 1043;
        // public delegate void LuaWebSocketBinaryReceivedCSEventDelegate(WebSocket ws, byte[] datas);
        P_LuaWebSocketBinaryReceivedCSEventDelegate = 1043,

        // P_LuaWebSocketClosedCSEventDelegate = 1044;
        // public delegate void LuaWebSocketClosedCSEventDelegate(WebSocket ws, UInt16 code, string message);
        P_LuaWebSocketClosedCSEventDelegate = 1044,

        // P_LuaWebSocketErrorCSEventDelegate = 1045;
        // public delegate void LuaWebSocketErrorCSEventDelegate(WebSocket ws, string error);
        P_LuaWebSocketErrorCSEventDelegate = 1045,
    }

    #endregion
}
