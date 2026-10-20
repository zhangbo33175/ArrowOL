/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Editor
 * descrip:   NetworkComponent Inspector（功能已迁移至 Honor → 网络协议工具）
 ***************************************************************/

using UnityEditor;
using UnityEngine;

namespace Honor.Editor
{
    [CustomEditor(typeof(Honor.Runtime.NetworkComponent))]
    public class NetworkComponentInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox("网络协议导出与超时配置已迁移至菜单：Honor → 网络协议工具", MessageType.Info);
        }
    }
}
