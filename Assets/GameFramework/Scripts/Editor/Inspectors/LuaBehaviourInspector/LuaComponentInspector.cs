/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Editor
 * descrip:   LuaComponent Inspector（已迁移至 Honor → XLua工具）
 ***************************************************************/

using UnityEditor;

namespace Honor.Editor
{
    [CustomEditor(typeof(Honor.Runtime.LuaComponent))]
    public class LuaComponentInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox("XLua 脚本生成与运行时配置已迁移至菜单：Honor → XLua工具", MessageType.Info);
        }
    }
}
