/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Editor
 * All Rights Reserved.
 * filename:  BuildOrderPackagePC.cs
 * author:    Honor.Editor
 * created:   2026
 * descrip:   打包流水线订单 - 打 Windows PC exe 玩家包
 *            失败约定：BuildPlayer_PC.Build 返回 false 时 throw。
 ***************************************************************/

using System;
using UnityEditor;

namespace Honor.Editor.BuildTool
{
    /// <summary>
    /// 打 PC exe 包订单。
    /// </summary>
    public class BuildOrderPackagePC : BuildOrder
    {
        /// <summary>
        /// 构造订单：初始化订单类型 / 名称 / 提示 / 执行委托。
        /// </summary>
        public BuildOrderPackagePC()
        {
            orderType = OrderType.PackagePC;
            Name = "打PC包";
            tips = "BuildPipeline 打包 Windows Standalone exe（LZ4/开发选项）";
            action = OnAction;
        }

        /// <summary>
        /// 订单执行体：打 PC 包，失败抛中文异常终止流水线。
        /// </summary>
        private void OnAction(BuildContext ctx)
        {
            BuildPlatformContext platformCtx = ctx.GetContextObject<BuildPlatformContext>();
            if (!BuildPlayer_PC.Build(platformCtx))
            {
                throw new Exception("PC 打包失败，请查看上方日志");
            }
        }
    }
}
