/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Editor
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  BuildOrderPackageRes.cs
 * author:    云毅
 * created:   2026
 * descrip:   打包流水线订单 - 打 Assets/Res 资源 AssetBundle
 *            输出 StreamingAssets/AssetBundles/{平台} 并生成 respathtab.txt
 ***************************************************************/

using System;
using Honor.Editor.BuildPackage;
using UnityEngine;

namespace Honor.Editor.BuildTool
{
    //=========================================================================
    // 订单：打 Res 资源 AB
    //=========================================================================
    /// <summary>
    /// 打 Res 资源 AB 订单
    /// 说明：按默认配置打包 Assets/Res 资源为 AssetBundle，
    ///       输出到 StreamingAssets/AssetBundles/{平台} 并生成 respathtab 资源映射
    /// </summary>
    public class BuildOrderPackageRes : BuildOrder
    {
        #region 构造与执行
        /// <summary>
        /// 构造订单：初始化名称/提示/订单类型并绑定执行回调
        /// </summary>
        public BuildOrderPackageRes()
        {
            Name = "打Res资源AB";
            tips = "按配置打包 Assets/Res 资源为 AssetBundle，输出 StreamingAssets/AssetBundles/{平台} 并生成 respathtab";
            orderType = OrderType.PackageRes;
            action = OnBuild;
        }

        /// <summary>
        /// 执行打包：从上下文取目标平台与增量开关，调用 AB 打包器
        /// </summary>
        /// <param name="context">打包上下文</param>
        /// <exception cref="Exception">打包失败时抛出，中断流水线</exception>
        private void OnBuild(BuildContext context)
        {
            BuildPlatformContext ctx = context.GetContextObject<BuildPlatformContext>();

            Debug.Log($"[打包订单] 打Res资源AB，平台={ctx.Target}，增量={ctx.Incremental}");

            bool ok = BuildPackage_Builder.BuildAll(ctx.Target, null, ctx.Incremental);
            if (!ok)
                throw new Exception("[AB打包] res 资源打包失败");
        }
        #endregion
    }
}
