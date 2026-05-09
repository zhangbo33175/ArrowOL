using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using XLua;

namespace Honor.Runtime
{
    /// <summary>
    /// Lua 回调处理工具类
    /// 提供 C# 调用 Lua 函数的统一封装，用于 XLua 框架下的跨语言回调
    /// </summary>
    public static class LuaHandler
    {
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
    }
}