/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Editor
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  AssetComponentInspector.cs
 * author:    云毅
 * created:   2026
 * descrip:   AssetComponent Inspector（已迁移至 Honor → 资源管理工具）
 ***************************************************************/

using UnityEditor;

namespace Honor.Editor
{
    // AssetComponent 为纯 C# 组件（由门面 HonorAssetManager 创建持有），非 UnityEngine.Component，
    // [CustomEditor] 无法绑定到非 Component 类型，该属性无效且会触发 Unity 校验警告。
    // 资源加载配置已迁移至「Honor → 资源管理工具」（AssetExportEditorWindow），故注释禁用此检视器。
    // [CustomEditor(typeof(Honor.Runtime.AssetComponent))]
    public class AssetComponentInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox("资源加载配置已迁移至菜单：Honor → 资源管理工具", MessageType.Info);
        }
    }
}