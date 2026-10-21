/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Editor
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  BuildOrderPackageAPK.cs
 * author:    云毅
 * created:   2026
 * descrip:   打包流水线订单 - 打 Android APK 玩家包
 *            失败约定：BuildPlayer_Android.Build 返回 false 时 throw，
 *            流水线据此停止并将 CI 退出码置 1。
 ***************************************************************/

using System;
using UnityEditor;

namespace Honor.Editor.BuildTool
{
    /// <summary>
    /// 打 Android APK 包订单。
    /// 说明：从 BuildContext 取出 BuildPlatformContext，调用玩家包打包器；
    /// 资源 AB 须由前置订单（PackageRes/PackageAll）打入 StreamingAssets。
    /// </summary>
    public class BuildOrderPackageAPK : BuildOrder
    {
        /// <summary>
        /// 构造订单：初始化订单类型 / 名称 / 提示 / 执行委托。
        /// </summary>
        public BuildOrderPackageAPK()
        {
            orderType = OrderType.PackageAPK;
            Name = "打APK包";
            tips = "BuildPipeline 打包 Android APK（LZ4/开发选项/keystore）";
            action = OnAction;
        }

        /// <summary>
        /// 订单执行体：打 APK，失败抛中文异常终止流水线。
        /// </summary>
        private void OnAction(BuildContext ctx)
        {
            BuildPlatformContext platformCtx = ctx.GetContextObject<BuildPlatformContext>();
            if (!BuildPlayer_Android.Build(platformCtx))
            {
                throw new Exception("APK 打包失败，请查看上方日志");
            }
        }
    }
}
