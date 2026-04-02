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
    /// Lua创建LuaBahaviour到Lua层的面向对象Class全局事件派发
    /// C#回调到Lua
    /// </summary>
    /// <param name="env">xlua提供的lua环境</param>
    /// <param name="luaScriptName">luaClass的类名称</param>
    /// <returns></returns>
    public delegate LuaTable LuaCreateLuaClassFromCSEventDelegate(LuaTable env, string luaScriptName);

    /// <summary>
    /// Lua层本地化语言表数据关联回调全局事件派发
    /// </summary>
    public delegate void LuaRelateLocalizationTableDataFromCSEventDelegate();

    /// <summary>
    /// Lua创建Lua层的Procedure面向对象Class全局事件派发
    /// C#回调到Lua
    /// </summary>
    /// <param name="env">xlua提供的lua环境</param>
    /// <param name="luaScriptName">luaClass的类名称</param>
    /// <returns></returns>
    public delegate LuaTable LuaCreateProcedureLuaClassFromCSEventDelegate(LuaTable env, string luaScriptName);

    /// <summary>
    /// Lua层ApplicationPause回调全局事件派发
    /// C#回调到Lua
    /// </summary>
    /// <param name="pause">是否暂停</param>
    public delegate void LuaApplicationPauseFromCSEventDelegate(bool pause);

    /// <summary>
    /// Lua层ApplicationQuit回调全局事件派发
    /// C#回调到Lua
    /// </summary>
    public delegate void LuaApplicationQuitFromCSEventDelegate();

    /// <summary>
    /// Lua层原生按键弹起回调全局事件派发
    /// C#回调到Lua
    /// </summary>
    /// <param name="keyCode">按键编号</param>
    public delegate void LuaKeysUpFromCSEventDelegate(KeyCode keyCode);

   
    /// <summary>
    /// Lua层Apple登陆失败回调全局事件派发
    /// C#回调到Lua
    /// </summary>
    /// <param name="resultTable">结果数据</param>
    public delegate void LuaSignInWithAppleCSEventDelegate(LuaTable resultTable);

    /// <summary>
    /// Lua层Apple登陆状态查询全局事件派发
    /// C#回调到Lua
    /// </summary>
    /// <param name="statueTable">状态数据</param>
    public delegate void LuaSignInWithAppleStateCSEventDelegate(LuaTable statueTable);

    /// <summary>
    /// Lua层接收C#事件回调全局派发
    /// </summary>
    /// <param name="eventArgs">C#事件参数</param>
    public delegate void LuaReceiveEventCSEventDelegate(EventParams eventArgs);
 
    /// <summary>
    /// c#获取lua层的ResDefInfo事件全局派发
    /// </summary>
    public delegate LuaTable LuaGetResDefInfoEventDelegate(string name);

    /// <summary>
    /// Lua层Localizing-UI本地化全局事件派发
    /// </summary>
    /// <param name="localizingKeyName">本地化Key字段名称</param>
    /// <returns></returns>
    public delegate string LuaLocalizingCSEventDelegate(string localizingKeyName);

}
