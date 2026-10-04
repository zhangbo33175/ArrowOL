/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Editor
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  BuildPackage_Menu.cs
 * author:    云毅
 * created:   2026
 * descrip:   AssetBundle 打包工具 - 菜单与命令行入口
 *            提供批量打包入口（-executeMethod），供 Jenkins/CI 调用
 ***************************************************************/

using UnityEditor;

namespace Honor.Editor.BuildPackage
{
    //=========================================================================
    // 菜单与命令行入口
    //=========================================================================
    /// <summary>
    /// AB 打包菜单与命令行入口
    /// 说明：菜单触发一键打包；提供静态入口供 CI 通过 -executeMethod 调用
    /// </summary>
    public static class BuildPackage_Menu
    {
        #region 打包动作（已从菜单移除，保留方法供代码复用；操作收口到 AB 打包工具界面）
        /// <summary>
        /// 一键打包 AB（原菜单入口；现操作收口到 AB 打包工具界面，保留方法供复用）
        /// </summary>
        public static void BuildAll()
        {
            if (!EditorUtility.DisplayDialog("AB打包", "确定开始打包 AssetBundle 吗？", "开始打包", "取消"))
                return;

            BuildPackage_Builder.BuildAll();
        }

        /// <summary>
        /// 增量打包 AB（无内容变更则跳过；原菜单入口，保留方法供复用）
        /// </summary>
        public static void BuildAllIncremental()
        {
            if (!EditorUtility.DisplayDialog("AB打包", "确定开始增量打包 AssetBundle 吗？", "开始打包", "取消"))
                return;

            BuildPackage_Builder.BuildAll(incremental: true);
        }

        /// <summary>
        /// 清理输出目录（原菜单入口，保留方法供复用）
        /// </summary>
        public static void ClearOutput()
        {
            string outRoot = BuildPackage_Define.BuildOutputFullRoot;
            if (System.IO.Directory.Exists(outRoot))
            {
                System.IO.Directory.Delete(outRoot, true);
                AssetDatabase.Refresh();
                UnityEngine.Debug.Log($"[AB打包] 已清空输出目录：{outRoot}");
            }
        }
        #endregion

        #region 命令行入口（CI）
        /// <summary>
        /// 命令行一键打包 AB（供 CI 调用）
        /// 用法：Unity -batchmode -quit -projectPath <path> -executeMethod Honor.Editor.BuildPackage.BuildPackage_Menu.BuildAllForCI
        /// </summary>
        public static void BuildAllForCI()
        {
            bool ok = BuildPackage_Builder.BuildAll();
            if (!ok)
                EditorApplication.Exit(1);
        }

        /// <summary>
        /// 命令行增量打包 AB（供 CI 调用）
        /// 用法：Unity -batchmode -quit -projectPath <path> -executeMethod Honor.Editor.BuildPackage.BuildPackage_Menu.BuildIncrementalForCI
        /// </summary>
        public static void BuildIncrementalForCI()
        {
            bool ok = BuildPackage_Builder.BuildAll(incremental: true);
            if (!ok)
                EditorApplication.Exit(1);
        }
        #endregion
    }
}
