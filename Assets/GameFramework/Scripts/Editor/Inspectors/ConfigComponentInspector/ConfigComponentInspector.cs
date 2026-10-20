/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Editor
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  ConfigComponentInspector.cs
 * author:    云毅
 * created:   2026
 * descrip:   ConfigComponent Inspector
 *            配置表导出功能已迁移至 Honor → 配置表工具 独立窗口
 ***************************************************************/

using UnityEditor;

namespace Honor.Editor
{
    [CustomEditor(typeof(Honor.Runtime.ConfigComponent))]
    public class ConfigComponentInspector : HonorComponentInspector
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.HelpBox("配置表导出工具已迁移至菜单：Honor → 配置表工具", MessageType.Info);
        }
    }
}
