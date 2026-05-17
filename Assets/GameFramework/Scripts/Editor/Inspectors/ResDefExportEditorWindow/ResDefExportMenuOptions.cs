/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Editor
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  ResDefExportMenuOptions.cs
 * author:    云毅
 * created:   2026
 * descrip:   资源导出工具 - 菜单栏注册入口
 ***************************************************************/

using UnityEditor;
using UnityEngine;

namespace Honor.Editor
{
    /// <summary>
    /// 资源导出工具菜单注册类
    /// 提供 Unity 编辑器菜单栏入口，打开资源信息导出窗口
    /// </summary>
    public class ResDefExportMenuOptions
    {
        /// <summary>
        /// 打开资源信息导出工具窗口
        /// </summary>
        [MenuItem("Tools/资源导出工具", false, 357)]
        private static void OpenCodeLoader()
        {
            // 禁止在运行、编译、刷新时打开窗口
            if (Application.isPlaying || EditorApplication.isCompiling || EditorApplication.isUpdating)
            {
                EditorUtility.DisplayDialog("Honor提示", "Unity正在处理刷新中，请稍后再试。", "确定");
                return;
            }

            ResDefExportEditorWindow.Open();
        }
    }
}