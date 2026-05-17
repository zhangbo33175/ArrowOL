/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  LuaHandler.cs
 * author:    云毅
 * created:   2026
 * descrip:   XLua框架回调处理工具，C#调用Lua函数统一封装
 ***************************************************************/
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using XLua;

namespace Honor.Runtime
{
    //=========================================================================
    // Lua 回调处理工具类
    //=========================================================================
    /// <summary>
    /// Lua 回调处理工具类
    /// 提供 C# 调用 Lua 函数的统一封装，用于 XLua 框架下的跨语言回调
    /// </summary>
    public static class LuaHandler
    {
        #region 公开回调执行接口
        /// <summary>
        /// 执行 Lua 回调函数
        /// 从 LuaTable 中取出名为 "func" 的委托并执行
        /// </summary>
        /// <param name="luaHandler">存储回调函数的 Lua Table</param>
        /// <param name="args">传递给 Lua 函数的参数 Table（可为空）</param>
        public static void Callback(LuaTable luaHandler, LuaTable args = null)
        {
            // 校验 Lua Table 不为空
            if (luaHandler != null)
            {
                // 从 Lua Table 中获取委托函数
                Action<LuaTable> callbackFunc;
                luaHandler.Get("func", out callbackFunc);

                // 执行 Lua 回调，传递参数
                callbackFunc(args);
            }
        }
        #endregion
    }
}