/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Editor
 * descrip:   LocalizationComponent Inspector（已迁移至 Honor → 多语言工具）
 ***************************************************************/

using UnityEditor;

namespace Honor.Editor
{
    [CustomEditor(typeof(Honor.Runtime.LocalizationComponent))]
    internal sealed partial class LocalizationComponentInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox("多语言工具已迁移至菜单：Honor → 多语言工具", MessageType.Info);
        }
    }
}
