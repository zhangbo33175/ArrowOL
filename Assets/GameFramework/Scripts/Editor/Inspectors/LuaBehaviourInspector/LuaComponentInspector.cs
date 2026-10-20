/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Editor
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  LuaComponentInspector.cs
 * author:    云毅
 * created:   2026
 * descrip:   LuaComponent Inspector（已迁移至 Honor → XLua工具）
 ***************************************************************/

using UnityEditor;

namespace Honor.Editor
{
    // LuaComponent 为纯 C# 单例（Singleton<LuaComponent>），非 UnityEngine.Component，
    // [CustomEditor] 无法绑定到非 Component 类型，该属性无效且会触发 Unity 校验警告。
    // XLua 脚本生成与运行时配置已迁移至「Honor → XLua工具」（LuaScriptEditorWindow），故注释禁用此检视器。
    // [CustomEditor(typeof(Honor.Runtime.LuaComponent))]
    public class LuaComponentInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox("XLua 脚本生成与运行时配置已迁移至菜单：Honor → XLua工具", MessageType.Info);
        }
    }
}