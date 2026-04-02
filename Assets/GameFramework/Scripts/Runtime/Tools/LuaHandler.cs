using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using XLua;

namespace Honor.Runtime
{
    public static class LuaHandler
    {
        public static void Callback(LuaTable luaHandelr, LuaTable args = null)
        {
            if (luaHandelr != null)
            {
                Action<LuaTable> callbackFunc;
                luaHandelr.Get("func", out callbackFunc);
                callbackFunc(args);
            }
        }

    }

}


