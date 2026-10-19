/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  Protocol.pb
 * author:    云毅
 * created:   2026
 * descrip:   C# ⇄ Lua 统一通信协议配置源（SDK 消息风格）
 *            每个协议为"自包含块"：① cmd 定义 → ② 方法（委托）→ ③ 数据类
 *              ① cmd：      P_<模块><语义名>CSEventDelegate = <固定 cmd 值>,
 *              ② 方法：     [CSharpCallLua] public delegate <返回> <方法>(int cmd, <数据类>);
 *              ③ 数据类：   <数据类> { <类型> <字段> = <编号>, ... }（proto 风格）
 *            本文件为"协议配置源"（cmd/数据类为 proto 风格标记，不可直接编译），
 *            运行 Tools/协议注册表生成器 自动生成可编译产物：
 *              - C# 命令枚举   LuaProtocolCmd.cs   （收集各块 cmd，对齐 SGSDKProtocol）
 *              - C# 数据类     LuaProtocolData.cs   （对齐 SGSDKMap 的 CS/SC 类）
 *              - C# 映射表     LuaProtocolDataMap.cs（对齐 SGSDKDataBaseMap）
 *              - C# 注册表     LuaProtocolReg.cs
 *              - Lua 命令注册表 ProtocolCmd.lua.txt
 ***************************************************************/

using UnityEngine;
using XLua;

namespace Honor.Runtime
{
    //=========================================================================
    #region ◤ 编辑指南：如何添加一个新协议 ◢
    //=========================================================================
    // 新增一个 C# ⇄ Lua 协议只需在一个"自包含块"内完成 3 步：
    //   ① cmd 定义（必须为固定 int，跨进程一致；不可用运行时随机码）
    //        P_<模块><语义名>CSEventDelegate = <固定 cmd 值>,
    //   ② 方法定义（统一 int cmd 首参，第二参为强类型数据类）
    //        [CSharpCallLua]
    //        public delegate <返回类型> <方法名>(int cmd, <数据类名>);
    //   ③ 数据类定义（proto 风格，字段编号从 1 递增）
    //        <数据类名> {
    //            <字段类型> <字段名> = <编号>,
    //        }
    //   完成后运行 Tools → 协议注册表生成器，自动生成 enum/数据类/映射/路由
    //
    // 数据类字段类型（proto → C#）：
    //   LuaTable→LuaTable | string→string | bool→bool | int→int
    //   byte[]→byte[]     | object→object
    //
    // cmd 分配规则（当前为示例占位，需与服务器确认后固化）：
    //   1001-1010 类创建/生命周期 | 1011-1020 应用/输入 | 1021-1030 登录授权
    //   1031-1040 事件/资源/本地化 | 1041-1050 WebSocket
    //=========================================================================

    //=========================================================================
    #region 协议定义区（每协议自包含：cmd + 方法 + 数据类）
    //=========================================================================

    // ---------------- ① 类创建 / 生命周期 1001 - 1010 ----------------

    /// <summary>创建 LuaBehaviour 类，绑定生命周期</summary>
    P_LuaCreateLuaClassFromCSEventDelegate = 1001,   // cmd（① enum 定义）
    [CSharpCallLua]
    public delegate LuaTable LuaCreateLuaClassFromCSEventDelegate(int cmd, CreateLuaClass);  // 方法（② 委托，int cmd 首参）
    CreateLuaClass {                                 // 数据类（③ proto 风格字段编号）
        LuaTable env = 1,
        string luaScriptName = 2
    }

    /// <summary>创建流程（Procedure）Lua 类</summary>
    P_LuaCreateProcedureLuaClassFromCSEventDelegate = 1002,   // cmd（① enum 定义）
    [CSharpCallLua]
    public delegate LuaTable LuaCreateProcedureLuaClassFromCSEventDelegate(int cmd, CreateProcedureLuaClass);  // 方法（② 委托，int cmd 首参）
    CreateProcedureLuaClass {                        // 数据类（③ proto 风格字段编号）
        LuaTable env = 1,
        string luaScriptName = 2
    }

    // ---------------- ② 应用 / 输入 1011 - 1020 ----------------

    /// <summary>应用暂停/唤醒回调</summary>
    P_LuaApplicationPauseFromCSEventDelegate = 1011,   // cmd（① enum 定义）
    [CSharpCallLua]
    public delegate void LuaApplicationPauseFromCSEventDelegate(int cmd, ApplicationPause);  // 方法（② 委托，int cmd 首参）
    ApplicationPause {                               // 数据类（③ proto 风格字段编号）
        bool pause = 1
    }

    /// <summary>应用退出回调</summary>
    P_LuaApplicationQuitFromCSEventDelegate = 1012,   // cmd（① enum 定义）
    [CSharpCallLua]
    public delegate void LuaApplicationQuitFromCSEventDelegate(int cmd, ApplicationQuit);  // 方法（② 委托，int cmd 首参）
    ApplicationQuit {                                // 数据类（③ proto 风格字段编号）
    }

    /// <summary>键盘按键抬起回调</summary>
    P_LuaKeysUpFromCSEventDelegate = 1013,   // cmd（① enum 定义）
    [CSharpCallLua]
    public delegate void LuaKeysUpFromCSEventDelegate(int cmd, KeysUp);  // 方法（② 委托，int cmd 首参）
    KeysUp {                                       // 数据类（③ proto 风格字段编号）
        int keyCode = 1
    }

    // ---------------- ③ 登录授权 1021 - 1030 ----------------

    /// <summary>Apple/Google 登录结果回调（Google 复用同委托类型）</summary>
    P_LuaSignInWithAppleCSEventDelegate = 1021,   // cmd（① enum 定义）
    [CSharpCallLua]
    public delegate void LuaSignInWithAppleCSEventDelegate(int cmd, SignInWithApple);  // 方法（② 委托，int cmd 首参）
    SignInWithApple {                              // 数据类（③ proto 风格字段编号）
        LuaTable resultTable = 1
    }

    /// <summary>登录状态查询回调</summary>
    P_LuaSignInWithAppleStateCSEventDelegate = 1022,   // cmd（① enum 定义）
    [CSharpCallLua]
    public delegate void LuaSignInWithAppleStateCSEventDelegate(int cmd, SignInWithAppleState);  // 方法（② 委托，int cmd 首参）
    SignInWithAppleState {                          // 数据类（③ proto 风格字段编号）
        LuaTable stateTable = 1
    }

    // ---------------- ④ 事件 / 资源 / 本地化 1031 - 1040 ----------------

    /// <summary>C# 事件派发回调（EventParams 承载）</summary>
    P_LuaReceiveEventCSEventDelegate = 1031,   // cmd（① enum 定义）
    [CSharpCallLua]
    public delegate void LuaReceiveEventCSEventDelegate(int cmd, ReceiveEvent);  // 方法（② 委托，int cmd 首参）
    ReceiveEvent {                               // 数据类（③ proto 风格字段编号）
        int eventCmd = 1,
        object eventObj = 2
    }

    /// <summary>本地化语言表数据关联回调</summary>
    P_LuaRelateLocalizationTableDataFromCSEventDelegate = 1032,   // cmd（① enum 定义）
    [CSharpCallLua]
    public delegate void LuaRelateLocalizationTableDataFromCSEventDelegate(int cmd, RelateLocalizationTableData);  // 方法（② 委托，int cmd 首参）
    RelateLocalizationTableData {                  // 数据类（③ proto 风格字段编号）
    }

    /// <summary>获取资源定义信息回调</summary>
    P_LuaGetResDefInfoEventDelegate = 1033,   // cmd（① enum 定义）
    [CSharpCallLua]
    public delegate LuaTable LuaGetResDefInfoEventDelegate(int cmd, GetResDefInfo);  // 方法（② 委托，int cmd 首参）
    GetResDefInfo {                              // 数据类（③ proto 风格字段编号）
        string name = 1
    }

    /// <summary>UI 本地化文本获取回调</summary>
    P_LuaLocalizingCSEventDelegate = 1034,   // cmd（① enum 定义）
    [CSharpCallLua]
    public delegate string LuaLocalizingCSEventDelegate(int cmd, Localizing);  // 方法（② 委托，int cmd 首参）
    Localizing {                                 // 数据类（③ proto 风格字段编号）
        string localizingKey = 1
    }

    // ---------------- ⑤ WebSocket 1041 - 1050 ----------------

    /// <summary>WebSocket 建立成功回调</summary>
    P_LuaWebSocketOpenCSEventDelegate = 1041,   // cmd（① enum 定义）
    [CSharpCallLua]
    public delegate void LuaWebSocketOpenCSEventDelegate(int cmd, WebSocketOpen);  // 方法（② 委托，int cmd 首参）
    WebSocketOpen {                              // 数据类（③ proto 风格字段编号）
    }

    /// <summary>WebSocket 文本消息接收回调</summary>
    P_LuaWebSocketMessageReceivedCSEventDelegate = 1042,   // cmd（① enum 定义）
    [CSharpCallLua]
    public delegate void LuaWebSocketMessageReceivedCSEventDelegate(int cmd, WebSocketMessageReceived);  // 方法（② 委托，int cmd 首参）
    WebSocketMessageReceived {                   // 数据类（③ proto 风格字段编号）
        string message = 1
    }

    /// <summary>WebSocket 字节流消息接收回调</summary>
    P_LuaWebSocketBinaryReceivedCSEventDelegate = 1043,   // cmd（① enum 定义）
    [CSharpCallLua]
    public delegate void LuaWebSocketBinaryReceivedCSEventDelegate(int cmd, WebSocketBinaryReceived);  // 方法（② 委托，int cmd 首参）
    WebSocketBinaryReceived {                    // 数据类（③ proto 风格字段编号）
        byte[] datas = 1
    }

    /// <summary>WebSocket 关闭回调</summary>
    P_LuaWebSocketClosedCSEventDelegate = 1044,   // cmd（① enum 定义）
    [CSharpCallLua]
    public delegate void LuaWebSocketClosedCSEventDelegate(int cmd, WebSocketClosed);  // 方法（② 委托，int cmd 首参）
    WebSocketClosed {                            // 数据类（③ proto 风格字段编号）
        int code = 1,
        string message = 2
    }

    /// <summary>WebSocket 错误回调</summary>
    P_LuaWebSocketErrorCSEventDelegate = 1045,   // cmd（① enum 定义）
    [CSharpCallLua]
    public delegate void LuaWebSocketErrorCSEventDelegate(int cmd, WebSocketError);  // 方法（② 委托，int cmd 首参）
    WebSocketError {                             // 数据类（③ proto 风格字段编号）
        string error = 1
    }

    #endregion
}
