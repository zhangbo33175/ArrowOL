/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Editor
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  AssetExportEditorWindow.cs
 * author:    云毅
 * created:   2026
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
            // 纯 C# 组件，无 SerializedObject
            // TODO: 抽离重复 Slider 配置并加判空——Q<> 查不到元素时原写法会空引用崩溃
            SetupRangeSlider("delaySlider", 0f, 36000f);
            SetupRangeSlider("gcSlider", 0f, 1000f);

            RequestRepaint();
        }

        /// <summary>
        /// 初始化一个取值范围 Slider（查找不到对应元素时安全跳过）。
        /// </summary>
        /// <param name="sliderName">UXML 中 Slider 的 name（与布局绑定，不可改动）</param>
        /// <param name="lowValue">下限</param>
        /// <param name="highValue">上限</param>
        private void SetupRangeSlider(string sliderName, float lowValue, float highValue)
        {
            var slider = Q<Slider>(sliderName);
            if (slider == null)
            {
                return;
            }

            slider.lowValue = lowValue;
            slider.highValue = highValue;
            slider.value = 0;
            slider.RegisterValueChangedCallback(evt => { });
        }
    }
}