/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Editor
 * descrip:   SoundComponent Inspector（已迁移至 Honor → 声音表工具）
 ***************************************************************/

using UnityEditor;

namespace Honor.Editor
{
    [CustomEditor(typeof(Honor.Runtime.SoundComponent))]
    public class SoundComponentInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox("声音表导出与音频配置已迁移至菜单：Honor → 声音表工具", MessageType.Info);
        }
    }
}
