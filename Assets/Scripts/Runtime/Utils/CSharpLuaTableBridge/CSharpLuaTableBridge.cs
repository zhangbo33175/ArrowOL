using System;
using System.Collections.Generic;
using Honor.Runtime;
using UnityEngine;
using XLua;

namespace GameLib
{
    //=>沟通Lua与CSharp之间的表单数据
    public static class CSharpLuaTableBridge
    {
        public static LuaEnv GetCurrEnv
        {
            get
            {
                var env = Application.isPlaying ? GameMainRoot.Lua.Env  : new LuaEnv();
                if (!Application.isPlaying)
                {
                    env.DoString($"package.path = package.path..\";{Application.dataPath}/LuaScripts/Game/?.lua.txt\"");
                    env.DoString("require('Tables/Tables')");
                }
                return env;
            }
        }

        //=>获取表单配置数据
        public static T GetLuaTableItem<T>(string tablePath, string requireDefault, string id) where T : class
        {
            var tables = GetTableInGlobal<LuaTable>(GetCurrEnv, tablePath, requireDefault);
            return tables?.GetInPath<T>(id);
        }

        //=>获取这张表
        public static LuaTable GetLuaTable(string tablePath, string requireDefault)
        {
            return GetTableInGlobal<LuaTable>(GetCurrEnv, tablePath, requireDefault);
        }

        //=>获取这张表并转换为C#结构
        public static T GetLuaTableCSharp<T>(string tablePath, string requireDefault)
        {
            return GetTableInGlobal<T>(GetCurrEnv, tablePath, requireDefault);
        }
        
        //=>用于全局访问一张表，如果不存在，则调用generateString生成，生成的逻辑应该只在非运行时今昔
        private static T GetTableInGlobal<T>(LuaEnv luaEnv,string tableSearchKey,string generateString)
        {
            if (Application.isPlaying)
            {
                return luaEnv.Global.GetInPath<T>(tableSearchKey);
            }
            else
            {
                var ret = luaEnv.DoString(generateString);
                if (ret == null)
                {
                    Debug.LogError($"{tableSearchKey} is not exist...");
                    return default(T);
                }
                return luaEnv.Global.GetInPath<T>(tableSearchKey);
            }
        }
    }
}