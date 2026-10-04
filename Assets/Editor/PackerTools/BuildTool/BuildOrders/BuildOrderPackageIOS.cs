/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Editor
 * All Rights Reserved.
 * filename:  BuildOrderPackageIOS.cs
 * author:    Honor.Editor
 * created:   2026
 * descrip:   打包流水线订单 - 导出 iOS Xcode 工程
 *            失败约定：BuildPlayer_iOS.Build 返回 false 时 throw。
 ***************************************************************/

using System;
using UnityEditor;

namespace Honor.Editor.BuildTool
{
    /// <summary>
    /// 打 iOS Xcode 工程订单。
    /// 说明：需在 macOS 上执行；Windows 下玩家包打包器内部已捕获并返回 false。
    /// </summary>
    public class BuildOrderPackageIOS : BuildOrder
    {
        /// <summary>
        /// 构造订单：初始化订单类型 / 名称 / 提示 / 执行委托。
        /// </summary>
        public BuildOrderPackageIOS()
        {
            orderType = OrderType.PackageIOS;
            Name = "打iOS包";
            tips = "BuildPipeline 导出 iOS Xcode 工程（需 macOS/Xcode 环境）";
            action = OnAction;
        }

        /// <summary>
        /// 订单执行体：导出 iOS Xcode 工程，失败抛中文异常终止流水线。
        /// </summary>
        private void OnAction(BuildContext ctx)
        {
            BuildPlatformContext platformCtx = ctx.GetContextObject<BuildPlatformContext>();
            if (!BuildPlayer_iOS.Build(platformCtx))
            {
                throw new Exception("iOS 打包失败，请查看上方日志");
            }
        }
    }
}
