/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Editor
 * descrip:   存档工具窗口（Honor 菜单入口）
 ***************************************************************/

using Honor.Runtime;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Honor.Editor
{
    public class PersistExportEditorWindow : BaseToolkitEditorWindow<PersistExportEditorWindow>
    {
        private UnityEditor.Editor m_PersistEditor;

        protected override string UIAssetDir =>
            "Assets/GameFramework/Scripts/Editor/Inspectors/PersistComponentInspector/";
        protected override string UxmlName => "PersistExportEditorWindowTool";
        protected override GUIContent Title => new GUIContent("存档工具");
        protected override Vector2 MinSize => new Vector2(600, 500);
        protected override Vector2 MaxSize => new Vector2(600, 500);

        [MenuItem("Honor/存档工具", false, 368)]
        private static void OpenWindow() => Open();

        protected override void OnInitData()
        {
            // 纯 C# 单例，无 Unity Editor
        }

        protected override void OnBindUI()
        {
            BindClick("exportBtn", () =>
            {
                PersistComponentInspector.GenerateAllSaveProtos();
                EditorUtility.DisplayDialog("存档工具", "存档协议导出完成", "确定");
            });

            BindClick("openFolderBtn", () =>
                PersistComponentInspector.OpenProtoFolderStatic());

            BindImgui("debugContainer", DrawFullInspector);
            Q<IMGUIContainer>("debugContainer").style.flexGrow = 1;

            RequestRepaint();
        }

        private void DrawFullInspector()
        {
            if (m_PersistEditor == null)
            {
                GUILayout.Label("未找到 PersistComponent，请确认场景中有 PersistManager 物体。");
                return;
            }
            ((PersistComponentInspector)m_PersistEditor).DrawDebugPanels();
        }

        private void OnDisable()
        {
            if (m_PersistEditor != null) UnityEngine.Object.DestroyImmediate(m_PersistEditor);
        }
    }
}
