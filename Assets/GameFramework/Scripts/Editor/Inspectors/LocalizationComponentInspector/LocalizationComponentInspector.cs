/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Editor
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  LocalizationComponentInspector.cs
 * author:    云毅
 * created:   2026
 * descrip:   LocalizationComponent Inspector（已迁移至 Honor → 多语言工具）
 ***************************************************************/

using UnityEditor;

namespace Honor.Editor
{
    // LocalizationComponent 为纯 C# 单例（Singleton<LocalizationComponent>），非 UnityEngine.Component，
    // [CustomEditor] 无法绑定到非 Component 类型，该属性无效且会触发 Unity 校验警告。
    // 多语言工具已迁移至「Honor → 多语言工具」（LocalizationExportEditorWindow），故注释禁用此检视器。
    // [CustomEditor(typeof(Honor.Runtime.LocalizationComponent))]
    internal sealed partial class LocalizationComponentInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox("多语言工具已迁移至菜单：Honor → 多语言工具", MessageType.Info);
        }
    }
}