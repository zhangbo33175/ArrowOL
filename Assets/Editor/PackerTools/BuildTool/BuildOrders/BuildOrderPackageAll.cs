/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Editor
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  BuildOrderPackageAll.cs
 * author:    云毅
 * created:   2026
 * descrip:   打包流水线订单 - 一次 BuildAssetBundles 打包 res 与 lua
 *            生成单一 manifest（同时含 res+lua bundle）与完整 respathtab
 ***************************************************************/

using System;
using System.Collections.Generic;
using Honor.Editor.BuildPackage;
using UnityEngine;

namespace Honor.Editor.BuildTool
{
    //=========================================================================
    // 订单：打全部 AB（res + lua）
    //=========================================================================
    /// <summary>
    /// 打全部 AB（res+lua）订单
    /// 说明：一次 BuildAssetBundles 同时打包 res 与 lua，
    ///       保证产出单一 manifest（运行时 LuaComponent 才能在同一份 manifest 中扫到 lua bundle）
    /// </summary>
    public class BuildOrderPackageAll : BuildOrder
    {
        #region 构造与执行
        /// <summary>
        /// 构造订单：初始化名称/提示/订单类型并绑定执行回调
        /// </summary>
        public BuildOrderPackageAll()
        {
            Name = "打全部AB(res+lua)";
            tips = "一次 BuildAssetBundles 打包 res 与 lua，生成单一 manifest 与 respathtab";
            orderType = OrderType.PackageAll;
            action = OnBuild;
        }

        /// <summary>
        /// 执行打包：res 映射（静态 BundleMap）+ lua 分组合并，一次性打包
        /// </summary>
        /// <param name="context">打包上下文</param>
        /// <exception cref="Exception">打包失败时抛出，中断流水线</exception>
        private void OnBuild(BuildContext context)
        {
            BuildPlatformContext ctx = context.GetContextObject<BuildPlatformContext>();

            Debug.Log($"[打包订单] 打全部AB(res+lua)，平台={ctx.Target}，增量={ctx.Incremental}");

            // res 映射：BuildMap 填充静态 BundleMap（BuildAllWithExtraBundles 内部会按 configs 重建并与 lua 合并）
            BuildPackage_AssetMap.BuildMap(BuildPackage_DefaultConfigs.GetDefaults());

            // lua 分组
            Dictionary<string, List<string>> luaMap = BuildPackage_Lua.CollectLuaBundleMap();

            // 一次性 BuildAssetBundles（res+lua 合并，单一 manifest）
            bool ok = BuildPackage_Builder.BuildAllWithExtraBundles(ctx.Target, null, luaMap, ctx.Incremental);
            if (!ok)
                throw new Exception("[AB打包] res+lua 全量 AB 打包失败");
        }
        #endregion
    }
}
