/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Editor
 * descrip:   声音表导出工具窗口（Honor 菜单入口）
 ***************************************************************/

using System.IO;
using Honor.Runtime;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Honor.Editor
{
    /// <summary>声音表导出工具窗口。Honor → 声音表工具。</summary>
    public class SoundExportEditorWindow : BaseToolkitEditorWindow<SoundExportEditorWindow>
    {
        private SerializedObject m_SoundSO;

        protected override string UIAssetDir =>
            "Assets/GameFramework/Scripts/Editor/Inspectors/SoundComponentInspector/";
        protected override string UxmlName => "SoundExportEditorWindowTool";
        protected override GUIContent Title => new GUIContent("声音表工具");
        protected override Vector2 MinSize => new Vector2(600, 380);
        protected override Vector2 MaxSize => new Vector2(600, 380);

        [MenuItem("Honor/声音表工具", false, 363)]
        private static void OpenWindow() => Open();

        protected override void OnInitData()
        {
            // 纯 C# 单例，无 SerializedObject
        }

        protected override void OnBindUI()
        {
            BindClick("openExcelBtn", () =>
                TableExportEditorUtility.OpenExcel(GamePathUtils.Sound.GetExcelFileFullPath()));

            BindClick("exportBtn", () =>
            {
                string f = GamePathUtils.Sound.GetExcelFileFullPath();
                string name = Path.GetFileNameWithoutExtension(f);
                string excelPath = $"{GamePathUtils.Sound.GetExcelRootDirectoryFullPath()}/{name}.xlsm";
                string luaPath = Path.Combine(GamePathUtils.Table.GetLuaScriptRootDirectoryFullPath(), "AlTables", $"Table{name}.lua.txt");
                TableExportEditorUtility.ExportExcelToLua(excelPath, luaPath);
            });

            BindClick("openDirBtn", () =>
                TableExportEditorUtility.OpenDirectory(GamePathUtils.Sound.GetExcelRootDirectoryFullPath()));

            if (m_SoundSO != null)
            {
                var mixerField = Q<ObjectField>("audioMixerField");
                mixerField.value = m_SoundSO.FindProperty("m_AudioMixer").objectReferenceValue;
                mixerField.RegisterValueChangedCallback(evt =>
                {
                    m_SoundSO.FindProperty("m_AudioMixer").objectReferenceValue = evt.newValue;
                    m_SoundSO.ApplyModifiedProperties();
                });

                BindImgui("groupShellsContainer", DrawGroupShells);
            }

            RequestRepaint();
        }

        private void DrawGroupShells()
        {
            if (m_SoundSO == null) return;
            m_SoundSO.Update();
            var prop = m_SoundSO.FindProperty("m_SoundGroupShells");
            // 只渲染数组头，不自动展开子项，保持界面紧凑
            EditorGUILayout.PropertyField(prop, includeChildren: true);
            m_SoundSO.ApplyModifiedProperties();
        }
    }
}
