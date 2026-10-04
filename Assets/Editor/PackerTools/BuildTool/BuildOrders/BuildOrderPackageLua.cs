/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Editor
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  BuildOrderPackageLua.cs
 * author:    云毅
 * created:   2026
 * descrip:   打包流水线订单 - 打 Assets/LuaScripts Lua 资源 AssetBundle
 *            收集 Lua 脚本打包为 AB，并把 lua 条目追加进 respathtab.txt
 ***************************************************************/

using System;
using System.Collections.Generic;
using Honor.Editor.BuildPackage;
using UnityEngine;

namespace Honor.Editor.BuildTool
{
    //=========================================================================
    // 订单：打 Lua 资源 AB
    //=========================================================================
    /// <summary>
    /// 打 Lua 资源 AB 订单
    /// 说明：收集 Assets/LuaScripts 下 *.lua.txt / *.json / *.bytes 打包 Lua AB，
    ///       输出到 StreamingAssets/AssetBundles/{平台}，并追加 respathtab 映射
    /// </summary>
    public class BuildOrderPackageLua : BuildOrder
    {
        #region 构造与执行
        /// <summary>
        /// 构造订单：初始化名称/提示/订单类型并绑定执行回调
        /// </summary>
        public BuildOrderPackageLua()
        {
            Name = "打Lua资源AB";
            tips = "收集 Assets/LuaScripts 打包 Lua AB 并追加 respathtab";
            orderType = OrderType.PackageLua;
            action = OnBuild;
        }

        /// <summary>
        /// 执行打包：收集 lua 分组 -> 调用 Lua-only 打包器（追加 respathtab）
        /// </summary>
        /// <param name="context">打包上下文</param>
        /// <exception cref="Exception">打包失败时抛出，中断流水线</exception>
        private void OnBuild(BuildContext context)
        {
            BuildPlatformContext ctx = context.GetContextObject<BuildPlatformContext>();

            Debug.Log($"[打包订单] 打Lua资源AB，平台={ctx.Target}，增量={ctx.Incremental}");

            // Lua 加密为可选预留：当前仅记录日志，不改变产物
            if (ctx.LuaEncrypt)
                Debug.Log("[打包订单] Lua 加密预留，未启用");

            Dictionary<string, List<string>> luaMap = BuildPackage_Lua.CollectLuaBundleMap();
            if (luaMap == null || luaMap.Count == 0)
            {
                Debug.LogWarning("[打包订单] 未收集到任何 Lua 资源，跳过 Lua AB 打包。");
                return;
            }

            bool ok = BuildPackage_Builder.BuildLuaOnly(ctx.Target, luaMap, ctx.Incremental);
            if (!ok)
                throw new Exception("[AB打包] Lua 资源打包失败");
        }
        #endregion
    }
}
