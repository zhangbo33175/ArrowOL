/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  LuaProtocolCmd.cs
 * author:    云毅
 * created:   2026
 * descrip:   协议命令枚举（对齐 SDK SGSDKProtocol）
 *            cmd 定义链接 ID，固定 int，按业务区间分组
 *            与 Docs/ProgramConfig/ProtoPB/Protocol.pb 同步
 ***************************************************************/

using XLua;

namespace Honor.Runtime
{
    /// <summary>
    /// C# ⇄ Lua 协议命令（cmd 定义链接 ID）
    /// </summary>
    [CSharpCallLua]
    public enum LuaProtocolCmd
    {
        // ---------------- ① 类创建 / 生命周期 1001 - 1010 ----------------

        /// <summary>创建 LuaBehaviour 类，绑定生命周期</summary>
        P_LuaCreateLuaClassFromCSEventDelegate = 1001,

        /// <summary>创建流程（Procedure）Lua 类</summary>
        P_LuaCreateProcedureLuaClassFromCSEventDelegate = 1002,

        // ---------------- ② 应用 / 输入 1011 - 1020 ----------------

        /// <summary>应用暂停 / 唤醒回调</summary>
        P_LuaApplicationPauseFromCSEventDelegate = 1011,

        /// <summary>应用退出回调</summary>
        P_LuaApplicationQuitFromCSEventDelegate = 1012,

        /// <summary>键盘按键抬起回调</summary>
        P_LuaKeysUpFromCSEventDelegate = 1013,

        // ---------------- ③ 登录授权 1021 - 1030 ----------------

        /// <summary>Apple / Google 登录结果回调（Google 复用同委托类型）</summary>
        P_LuaSignInWithAppleCSEventDelegate = 1021,

        /// <summary>登录状态查询回调</summary>
        P_LuaSignInWithAppleStateCSEventDelegate = 1022,

        // ---------------- ④ 事件 / 资源 / 本地化 1031 - 1040 ----------------

        /// <summary>C# 事件派发回调（EventParams 承载）</summary>
        P_LuaReceiveEventCSEventDelegate = 1031,

        /// <summary>本地化语言表数据关联回调</summary>
        P_LuaRelateLocalizationTableDataFromCSEventDelegate = 1032,

        /// <summary>获取资源定义信息回调</summary>
        P_LuaGetResDefInfoEventDelegate = 1033,

        /// <summary>UI 本地化文本获取回调</summary>
        P_LuaLocalizingCSEventDelegate = 1034,

        // ---------------- ⑤ WebSocket 1041 - 1050 ----------------

        /// <summary>WebSocket 建立成功回调</summary>
        P_LuaWebSocketOpenCSEventDelegate = 1041,

        /// <summary>WebSocket 文本消息接收回调</summary>
        P_LuaWebSocketMessageReceivedCSEventDelegate = 1042,

        /// <summary>WebSocket 字节流消息接收回调</summary>
        P_LuaWebSocketBinaryReceivedCSEventDelegate = 1043,

        /// <summary>WebSocket 关闭回调</summary>
        P_LuaWebSocketClosedCSEventDelegate = 1044,

        /// <summary>WebSocket 错误回调</summary>
        P_LuaWebSocketErrorCSEventDelegate = 1045,
    }
}
