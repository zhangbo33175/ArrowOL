using UnityEditor;
using UnityEngine;

namespace Honor.Editor
{
    public class ResDefExportMenuOptions
    {
        /// <summary>
        /// 打开资源信息导出工具窗口
        /// </summary>
        [MenuItem("Tools/资源导出工具", false, 357)]
        static void OpenCodeLoader()
        {
            if (Application.isPlaying || EditorApplication.isCompiling || EditorApplication.isUpdating)
            {
                EditorUtility.DisplayDialog("Honor提示", "Unity正在处理刷新中，请稍后再试。", "确定");
                return;
            }

            ResDefExportEditorWindow.Open();
        }
    }
}