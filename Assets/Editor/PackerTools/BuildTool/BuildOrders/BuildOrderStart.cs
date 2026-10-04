/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Editor
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  BuildOrderStart.cs
 * author:    云毅
 * created:   2026
 * descrip:   打包流水线 - 起始订单
 *            所有流水线的首步：保底持久化设置，并向日志输出开始信息。
 *            tips 留空，不占用进度条步。
 ***************************************************************/

namespace Honor.Editor.BuildTool
{
    /// <summary>
    /// 起始订单（OrderType.Start）
    /// 说明：仅做保底初始化，不做任何资源/玩家打包。
    /// </summary>
    public class BuildOrderStart : BuildOrder
    {
        #region 构造
        /// <summary>注册订单元数据与执行动作</summary>
        public BuildOrderStart()
        {
            Name = "Start";
            tips = "";
            orderType = OrderType.Start;
            forceSelect = true;
            action = DoAction;
        }
        #endregion

        #region 执行
        /// <summary>
        /// 起始动作：保底 Save 设置 + 打印开始日志
        /// </summary>
        private void DoAction(BuildContext context)
        {
            // 保底持久化，确保窗口未点保存时设置也已落盘
            BuildSettingsStore.Save();

            BuildPlatformContext ctx = context.GetContextObject<BuildPlatformContext>();
            BuildLog.Log($"开始打包，平台:{ctx.PlatformName}");
        }
        #endregion
    }
}
