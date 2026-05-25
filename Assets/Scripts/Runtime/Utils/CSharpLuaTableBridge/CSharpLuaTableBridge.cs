/***************************************************************
 * (c) copyright 2026 - 2030, GameLib
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  CSharpLuaTableBridge.cs
 * author:    云毅
 * created:   2026
 * descrip:   C# 与 Lua 配置表桥接工具
 *            运行时/编辑器通用，读取 Lua 表格配置，供地图编辑器使用
 ***************************************************************/

using System;
using System.Collections.Generic;
using Honor.Runtime;
using UnityEngine;
using XLua;

namespace GameLib
{
    /// <summary>
    /// C# 与 Lua 配置表桥接工具
    /// 功能：编辑器/运行时通用，从 Lua 环境中读取表格配置
    /// 作用：让地图编辑器可以直接读取游戏 Lua 表格数据
    /// </summary>
    public static class CSharpLuaTableBridge
    {
        #region 公共属性
        //=========================================================================
        // 公共属性
        //=========================================================================
        /// <summary>
        /// 获取当前 Lua 环境（运行时/编辑器 自动适配）
        /// 运行时：使用游戏主 LuaEnv
        /// 编辑器：新建环境并配置 Lua 路径
        /// </summary>
        public static LuaEnv GetCurrEnv
        {
            get
            {
                // 运行时：直接使用游戏主 Lua 环境
                LuaEnv env = Application.isPlaying ? GameMainRoot.Lua.Env : new LuaEnv();

                // 编辑器模式：手动配置 Lua 搜索路径
                if (!Application.isPlaying)
                {
                    // 追加 Lua 脚本搜索目录
                    env.DoString($"package.path = package.path..\";{Application.dataPath}/LuaScripts/Game/?.lua.txt\"");
                    // 加载总表格 Tables
                    env.DoString("require('Tables/Tables')");
                }

                return env;
            }
        }
        #endregion

        #region 公共读取方法
        //=========================================================================
        // 公共读取方法
        //=========================================================================
        /// <summary>
        /// 从 Lua 表中获取指定 ID 的配置项，并转为 C# 类
        /// </summary>
        /// <typeparam name="T">C# 数据结构</typeparam>
        /// <param name="tablePath">Lua 表路径（如：Tables.LevelData）</param>
        /// <param name="requireDefault">不存在时执行的 Lua 代码</param>
        /// <param name="id">配置ID</param>
        public static T GetLuaTableItem<T>(string tablePath, string requireDefault, string id) where T : class
        {
            LuaTable tables = GetTableInGlobal<LuaTable>(GetCurrEnv, tablePath, requireDefault);
            return tables?.GetInPath<T>(id);
        }

        /// <summary>
        /// 获取整张 Lua 表（返回 LuaTable）
        /// </summary>
        public static LuaTable GetLuaTable(string tablePath, string requireDefault)
        {
            return GetTableInGlobal<LuaTable>(GetCurrEnv, tablePath, requireDefault);
        }

        /// <summary>
        /// 获取整张 Lua 表，并直接转换为 C# 结构体
        /// </summary>
        public static T GetLuaTableCSharp<T>(string tablePath, string requireDefault)
        {
            return GetTableInGlobal<T>(GetCurrEnv, tablePath, requireDefault);
        }
        #endregion

        #region 核心私有方法
        //=========================================================================
        // 核心私有方法
        //=========================================================================
        /// <summary>
        /// 【核心】从 Lua 全局环境中获取表数据
        /// 运行时：直接读取
        /// 编辑器：执行初始化代码后读取
        /// </summary>
        private static T GetTableInGlobal<T>(LuaEnv luaEnv, string tableSearchKey, string generateString)
        {
            // 运行时：直接从 Lua 全局获取表
            if (Application.isPlaying)
            {
                return luaEnv.Global.GetInPath<T>(tableSearchKey);
            }
            // 编辑器：执行生成代码，再读取
            else
            {
                object[] ret = luaEnv.DoString(generateString);
                if (ret == null)
                {
                    Debug.LogError($"{tableSearchKey} 表不存在！");
                    return default;
                }

                return luaEnv.Global.GetInPath<T>(tableSearchKey);
            }
        }
        #endregion
    }
}