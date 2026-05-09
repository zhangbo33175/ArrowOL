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
    /// <summary>
    /// 【C# → Lua】创建 LuaBehaviour 面向对象类
    /// </summary>
    /// <param name="env">Lua 环境</param>
    /// <param name="luaScriptName">脚本名</param>
    /// <returns>Lua 类实例</returns>
    [CSharpCallLua]
    public delegate LuaTable LuaCreateLuaClassFromCSEventDelegate(LuaTable env, string luaScriptName);

    /// <summary>
    /// 【C# → Lua】关联本地化语言表数据
    /// </summary>
    [CSharpCallLua]
    public delegate void LuaRelateLocalizationTableDataFromCSEventDelegate();

    /// <summary>
    /// 【C# → Lua】创建流程（Procedure）Lua 类
    /// </summary>
    /// <param name="env">Lua 环境</param>
    /// <param name="luaScriptName">脚本名</param>
    /// <returns>Lua 类实例</returns>
    [CSharpCallLua]
    public delegate LuaTable LuaCreateProcedureLuaClassFromCSEventDelegate(LuaTable env, string luaScriptName);

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

    /// <summary>
    /// 【C# → Lua】键盘按键抬起
    /// </summary>
    /// <param name="keyCode">按键</param>
    [CSharpCallLua]
    public delegate void LuaKeysUpFromCSEventDelegate(KeyCode keyCode);

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
}