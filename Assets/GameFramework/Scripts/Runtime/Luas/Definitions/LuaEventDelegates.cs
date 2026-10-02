/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  LuaEventDelegates.cs
 * author:    云毅
 * created:   2026
 * descrip:   C# 与 Lua 互调全局委托定义（CSharpCallLua）
 ***************************************************************/

#if BEST_HTTP_ENABLE
using BestHTTP.WebSocket;
#endif
using System;
using UnityEngine;
using System.Collections.Generic;
#if IAP_ENABLE
using IAPWrapper;
#endif
using XLua;

namespace Honor.Runtime
{
    //=========================================================================

    #region Lua 面向对象创建委托

    //=========================================================================

    /// <summary>
    /// 【C# → Lua】创建 LuaBehaviour 面向对象类
    /// </summary>
    /// <param name="env">Lua 环境</param>
    /// <param name="luaScriptName">脚本名</param>
    /// <returns>Lua 类实例</returns>
    [CSharpCallLua]
    public delegate LuaTable LuaCreateLuaClassFromCSEventDelegate(LuaTable env, string luaScriptName);

    /// <summary>
    /// 【C# → Lua】创建流程（Procedure）Lua 类
    /// </summary>
    /// <param name="env">Lua 环境</param>
    /// <param name="luaScriptName">脚本名</param>
    /// <returns>Lua 类实例</returns>
    [CSharpCallLua]
    public delegate LuaTable LuaCreateProcedureLuaClassFromCSEventDelegate(LuaTable env, string luaScriptName);

    #endregion

    //=========================================================================

    #region 应用生命周期委托

    //=========================================================================

    /// <summary>
    /// 【C# → Lua】应用暂停/唤醒
    /// </summary>
    /// <param name="pause">是否暂停</param>
    [CSharpCallLua]
    public delegate void LuaApplicationPauseFromCSEventDelegate(bool pause);

    /// <summary>
    /// 【C# → Lua】应用退出
    /// </summary>
    [CSharpCallLua]
    public delegate void LuaApplicationQuitFromCSEventDelegate();

    #endregion

    //=========================================================================

    #region 输入事件委托

    //=========================================================================

    /// <summary>
    /// 【C# → Lua】键盘按键抬起
    /// </summary>
    /// <param name="keyCode">按键</param>
    [CSharpCallLua]
    public delegate void LuaKeysUpFromCSEventDelegate(KeyCode keyCode);

    #endregion

    //=========================================================================

    #region 登录授权委托

    //=========================================================================

    /// <summary>
    /// 【C# → Lua】Apple 登录结果回调
    /// </summary>
    /// <param name="resultTable">结果表</param>
    [CSharpCallLua]
    public delegate void LuaSignInWithAppleCSEventDelegate(LuaTable resultTable);

    /// <summary>
    /// 【C# → Lua】Apple 登录状态查询
    /// </summary>
    /// <param name="stateTable">状态表</param>
    [CSharpCallLua]
    public delegate void LuaSignInWithAppleStateCSEventDelegate(LuaTable stateTable);

    #endregion

    //=========================================================================

    #region 业务事件与本地化

    //=========================================================================

    /// <summary>
    /// 【C# → Lua】关联本地化语言表数据
    /// </summary>
    [CSharpCallLua]
    public delegate void LuaRelateLocalizationTableDataFromCSEventDelegate();

    /// <summary>
    /// 【C# → Lua】接收 C# 事件派发
    /// </summary>
    /// <param name="eventArgs">事件参数</param>
    [CSharpCallLua]
    public delegate void LuaReceiveEventCSEventDelegate(EventParams eventArgs);

    /// <summary>
    /// 【C# → Lua】获取资源定义信息
    /// </summary>
    /// <param name="name">资源名称</param>
    /// <returns>配置表</returns>
    [CSharpCallLua]
    public delegate LuaTable LuaGetResDefInfoEventDelegate(string name);

    /// <summary>
    /// 【C# → Lua】UI 本地化文本获取
    /// </summary>
    /// <param name="localizingKeyName">Key</param>
    /// <returns>本地化文本</returns>
    [CSharpCallLua]
    public delegate string LuaLocalizingCSEventDelegate(string localizingKeyName);

    #endregion

    //=========================================================================

    #region WebSocket 委托

    //=========================================================================

#if BEST_HTTP_ENABLE

    /// <summary>
    /// Lua层WebSocket建立成功回调全局派发
    /// </summary>
    /// <param name="ws">WebSocket连接</param>
    public delegate void LuaWebSocketOpenCSEventDelegate(WebSocket ws);

    /// <summary>
    /// Lua层WebSocket文本信息接收回调全局派发
    /// </summary>
    /// <param name="ws">WebSocket连接</param>
    /// <param name="message">接收的文本信息</param>
    public delegate void LuaWebSocketMessageReceivedCSEventDelegate(WebSocket ws, string message);

    /// <summary>
    /// Lua层WebSocket字节流信息接收回调全局派发
    /// </summary>
    /// <param name="ws">WebSocket连接</param>
    /// <param name="datas">接收的字节流信息</param>
    public delegate void LuaWebSocketBinaryReceivedCSEventDelegate(WebSocket ws, byte[] datas);

    /// <summary>
    /// Lua层WebSocket关闭回调全局派发
    /// </summary>
    /// <param name="ws">WebSocket连接</param>
    /// <param name="code">附带WebSocket-code</param>
    /// <param name="message">附带WebSocket文本信息</param>
    public delegate void LuaWebSocketClosedCSEventDelegate(WebSocket ws, UInt16 code, string message);

    /// <summary>
    /// Lua层WebSocket错误回调全局派发
    /// </summary>
    /// <param name="ws">WebSocket连接</param>
    /// <param name="error">错误信息</param>
    public delegate void LuaWebSocketErrorCSEventDelegate(WebSocket ws, string error);

#endif

    #endregion
}