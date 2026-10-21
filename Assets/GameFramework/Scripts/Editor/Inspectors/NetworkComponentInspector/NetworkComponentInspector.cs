/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Editor
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  NetworkComponentInspector.cs
 * author:    云毅
 * created:   2026
 * descrip:   NetworkComponent Inspector（功能已迁移至 Honor → 网络协议工具）
 ***************************************************************/

using UnityEditor;
using UnityEngine;

namespace Honor.Editor
{
    // NetworkComponent 为纯 C# 组件（由门面 HonorNetworkManager 创建持有），非 UnityEngine.Component，
    // [CustomEditor] 无法绑定到非 Component 类型，该属性无效且会触发 Unity 校验警告。
    // 网络协议导出与超时配置已迁移至「Honor → 网络协议工具」（NetworkExportEditorWindow），故注释禁用此检视器。
    // [CustomEditor(typeof(Honor.Runtime.NetworkComponent))]
    public class NetworkComponentInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox("网络协议导出与超时配置已迁移至菜单：Honor → 网络协议工具", MessageType.Info);
        }
    }
}