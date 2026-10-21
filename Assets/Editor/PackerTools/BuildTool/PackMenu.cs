/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Editor
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  PackMenu.cs
 * author:    云毅
 * created:   2026
 * descrip:   Honor 打包工具菜单入口（Honor/打包工具/...）
 *            只保留"打开打包窗口"菜单入口；
 *            各打包动作（打AB/打Lua/打Res/打APK/打PC/打iOS/打全平台AB/一键全平台）
 *            已统一收口到 <see cref="Honor.Editor.BuildTool.PackWindow"/> 窗口内的按钮，
 *            不再在菜单中重复暴露（避免入口散落、误点无窗口上下文）。
 ***************************************************************/

using UnityEditor;

namespace Honor.Editor.BuildTool
{
    //=========================================================================
    // 打包工具菜单入口
    //=========================================================================
    /// <summary>
    /// Honor 打包工具菜单
    /// 说明：
    /// 1. 菜单仅保留"打开打包窗口"入口，打包动作全部在打包工具窗口内操作；
    /// 2. 动作转发到 <see cref="BuildOrderHandler.Inst"/>，由流水线执行。
    /// </summary>
    public static class PackMenu
    {
        /// <summary>
        /// 菜单：Honor/打包工具/打开打包窗口
        /// 打开（或聚焦）Honor 打包工具主窗口（AB资源打包 / 玩家包打包 / Lua可视化）
        /// </summary>
        [MenuItem("Honor/打包工具/打开打包窗口", false, 100)]
        public static void OpenWindow()
        {
            PackWindow.Open();
        }
    }
}
