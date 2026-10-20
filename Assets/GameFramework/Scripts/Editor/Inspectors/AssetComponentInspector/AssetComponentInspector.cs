/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Editor
 * descrip:   AssetComponent Inspector（已迁移至 Honor → 资源管理工具）
 ***************************************************************/

using UnityEditor;

namespace Honor.Editor
{
    [CustomEditor(typeof(Honor.Runtime.AssetComponent))]
    public class AssetComponentInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox("资源加载配置已迁移至菜单：Honor → 资源管理工具", MessageType.Info);
        }
    }
}
