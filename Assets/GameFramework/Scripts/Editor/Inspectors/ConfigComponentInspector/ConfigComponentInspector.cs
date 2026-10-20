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
    // ConfigComponent 为纯 C# 单例（Singleton<ConfigComponent>），非 UnityEngine.Component，
    // [CustomEditor] 无法绑定到非 Component 类型，该属性无效且会触发 Unity 校验警告。
    // 配置表导出已迁移至「Honor → 配置表工具」（ConfigExportEditorWindow），故注释禁用此检视器。
    // [CustomEditor(typeof(Honor.Runtime.ConfigComponent))]
    public class ConfigComponentInspector : HonorComponentInspector
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.HelpBox("配置表导出工具已迁移至菜单：Honor → 配置表工具", MessageType.Info);
        }
    }
}
