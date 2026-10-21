/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Editor
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  SoundComponentInspector.cs
 * author:    云毅
 * created:   2026
 * descrip:   SoundComponent Inspector（已迁移至 Honor → 声音表工具）
 ***************************************************************/

using UnityEditor;

namespace Honor.Editor
{
    // SoundComponent 为纯 C# 组件（由门面 HonorSoundManager 创建持有），非 UnityEngine.Component，
    // [CustomEditor] 无法绑定到非 Component 类型，该属性无效且会触发 Unity 校验警告。
    // 音频配置已迁移至「Honor → 声音表工具」（SoundExportEditorWindow），故注释禁用此检视器。
    // [CustomEditor(typeof(Honor.Runtime.SoundComponent))]
    public class SoundComponentInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.HelpBox("声音表导出与音频配置已迁移至菜单：Honor → 声音表工具", MessageType.Info);
        }
    }
}