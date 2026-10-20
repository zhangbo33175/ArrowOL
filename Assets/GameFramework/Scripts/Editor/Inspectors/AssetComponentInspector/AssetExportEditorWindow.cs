/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Editor
 * descrip:   资源管理工具窗口（Honor 菜单入口）
 ***************************************************************/

using Honor.Runtime;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Honor.Editor
{
    /// <summary>资源管理工具窗口。Honor → 资源管理工具。</summary>
    public class AssetExportEditorWindow : BaseToolkitEditorWindow<AssetExportEditorWindow>
    {
        protected override string UIAssetDir =>
            "Assets/GameFramework/Scripts/Editor/Inspectors/AssetComponentInspector/";
        protected override string UxmlName => "AssetExportEditorWindowTool";
        protected override GUIContent Title => new GUIContent("资源管理工具");
        protected override Vector2 MinSize => new Vector2(500, 200);
        protected override Vector2 MaxSize => new Vector2(500, 200);

        [MenuItem("Honor/资源管理工具", false, 366)]
        private static void OpenWindow() => Open();

        protected override void OnInitData() { }

        protected override void OnBindUI()
        {
            // 纯 C# 单例，无 SerializedObject
            var delaySlider = Q<Slider>("delaySlider");
            delaySlider.lowValue = 0;
            delaySlider.highValue = 36000;
            delaySlider.value = 0;
            delaySlider.RegisterValueChangedCallback(evt => { });

            var gcSlider = Q<Slider>("gcSlider");
            gcSlider.lowValue = 0;
            gcSlider.highValue = 1000;
            gcSlider.value = 0;
            gcSlider.RegisterValueChangedCallback(evt => { });

            RequestRepaint();
        }
    }
}
