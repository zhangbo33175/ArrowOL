/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Editor
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  LocalizationExportEditorWindow.cs
 * author:    云毅
 * created:   2026
 * descrip:   多语言工具窗口（Honor 菜单入口）
 ***************************************************************/

using System.Collections.Generic;
using System.IO;
using GameLib;
using Honor.Runtime;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Honor.Editor
{
    /// <summary>多语言工具窗口。Honor → 多语言工具。</summary>
    public class LocalizationExportEditorWindow : BaseToolkitEditorWindow<LocalizationExportEditorWindow>
    {
        private bool m_EnableIncrease;
        private bool m_SingleVersion;
        private bool m_AutoFontAdapt;

        protected override string UIAssetDir =>
            "Assets/GameFramework/Scripts/Editor/Inspectors/LocalizationComponentInspector/";
        protected override string UxmlName => "LocalizationExportEditorWindowTool";
        protected override GUIContent Title => new GUIContent("多语言工具");
        protected override Vector2 MinSize => new Vector2(680, 600);
        protected override Vector2 MaxSize => new Vector2(680, 600);

        [MenuItem("Honor/多语言工具", false, 365)]
        private static void OpenWindow() => Open();

        protected override void OnInitData()
        {
            LocalizationComponentInspector.LoadLocalizationSettings(
                out m_EnableIncrease, out m_SingleVersion, out _);
        }

        protected override void OnBindUI()
        {
            BindClick("openMainExcelBtn", () =>
                TableExportEditorUtility.OpenExcel(EditorPath.Localization.ExcelFileFullPath));
            BindClick("exportMainBtn", () =>
            {
                var customs = new List<string>(
                    Directory.GetFiles(EditorPath.Localization.ExcelFolderFullPath, "*.xlsm", SearchOption.TopDirectoryOnly));
                LocalizationComponentInspector.ExportAllExcelsLanguageToLua(customs, m_EnableIncrease, m_SingleVersion);
            });
            BindClick("openMainDirBtn", () =>
                TableExportEditorUtility.OpenDirectory(EditorPath.Localization.ExcelFolderFullPath));

            BindClick("openDefaultExcelBtn", () =>
                TableExportEditorUtility.OpenExcel(EditorPath.Localization.ExcelDefaultFileFullPath));
            BindClick("exportDefaultBtn", () =>
                LocalizationComponentInspector.ExportExcelDefaultLanguageToJson());
            BindClick("openDefaultDirBtn", () =>
                TableExportEditorUtility.OpenDirectory(EditorPath.Localization.ExcelFolderFullPath));

            BindClick("openFontExcelBtn", () =>
                TableExportEditorUtility.OpenExcel(EditorPath.Localization.ExcelFontFileFullPath));
            BindClick("exportFontBtn", () =>
                LocalizationComponentInspector.ExportExcelFontsToJson());
            BindClick("openFontDirBtn", () =>
                TableExportEditorUtility.OpenDirectory(EditorPath.Localization.ExcelFolderFullPath));

            BindClick("chatgptBtn", () =>
                EditorUtility.DisplayDialog("提示", "ChatGPT 翻译工具暂未集成", "确定"));

            // TODO: 加固健壮性——下方若干 Q<Toggle>/Q<IMGUIContainer> 查不到元素时原写法直接空引用，统一判空
            var increaseToggle = Q<Toggle>("increaseToggle");
            if (increaseToggle != null)
            {
                increaseToggle.value = m_EnableIncrease;
                increaseToggle.RegisterValueChangedCallback(evt =>
                {
                    m_EnableIncrease = evt.newValue;
                    LocalizationComponentInspector.SaveLocalizationSettings(m_EnableIncrease, m_SingleVersion, "");
                    RequestRepaint();
                });
            }

            var singleToggle = Q<Toggle>("singleVersionToggle");
            if (singleToggle != null)
            {
                singleToggle.value = m_SingleVersion;
                singleToggle.RegisterValueChangedCallback(evt =>
                {
                    m_SingleVersion = evt.newValue;
                    LocalizationComponentInspector.SaveLocalizationSettings(m_EnableIncrease, m_SingleVersion, "");
                });
            }

            // 字体自动适配开关（纯 C# 组件，经门面 HonorCoreManager.Instance.Localization 访问）
            var autoFontToggle = Q<Toggle>("autoFontToggle");
            if (autoFontToggle != null)
            {
                autoFontToggle.value = Honor.Runtime.HonorCoreManager.Instance.Localization != null;
                autoFontToggle.RegisterValueChangedCallback(evt =>
                {
                    // 纯 C# 组件，无 SerializedObject 绑定
                });
            }

            BindImgui("mainListContainer", DrawMainList);
            BindImgui("defaultListContainer", DrawDefaultList);
            BindImgui("fontListContainer", DrawFontList);

            // IMGUIContainer 不自动撑高，显式设置高度（11行 × 20px + padding）
            SetContainerHeight("mainListContainer", 250);
            SetContainerHeight("defaultListContainer", 250);
            SetContainerHeight("fontListContainer", 240);
            RequestRepaint();
        }

        /// <summary>
        /// 设置指定 IMGUIContainer 的固定高度（元素不存在时安全跳过）。
        /// </summary>
        /// <param name="containerName">UXML 中 IMGUIContainer 的 name（与布局绑定，不可改动）</param>
        /// <param name="height">固定高度（像素）</param>
        private void SetContainerHeight(string containerName, float height)
        {
            var container = Q<IMGUIContainer>(containerName);
            if (container != null)
            {
                container.style.height = height;
            }
        }

        private static readonly Dictionary<string, string> LangDesc = new Dictionary<string, string>
        {
            { "ChineseSimplified", "简体中文" }, { "ChineseTraditional", "繁体中文" },
            { "English", "英语" }, { "French", "法语" }, { "German", "德语" },
            { "Italian", "意大利语" }, { "Japanese", "日语" }, { "Korean", "韩语" },
            { "PortuguesePortugal", "葡萄牙语" }, { "Russian", "俄语" }, { "Spanish", "西班牙语" },
        };

        private void DrawMainList()
        {
            // TODO: 加固健壮性——目录不存在时 Directory.GetFiles 会抛异常，与 DrawDefaultList 保持一致先判存在
            string luaRoot = EditorPath.Localization.LuaScriptsFolderFullPath;
            if (!Directory.Exists(luaRoot))
            {
                EditorGUILayout.LabelField("多语言类型数量", "0", EditorStyles.boldLabel);
                return;
            }

            string[] luaFiles = Directory.GetFiles(luaRoot,
                "Localization*.lua.txt", SearchOption.AllDirectories);
            EditorGUILayout.LabelField("多语言类型数量", luaFiles.Length.ToString(), EditorStyles.boldLabel);
            foreach (string f in luaFiles)
            {
                string name = Path.GetFileNameWithoutExtension(f);
                if (name.EndsWith(".lua")) name = name.Substring(0, name.Length - 4);
                if (name is "Localizations" or "LocalizationFonts" || name.EndsWith("Increase")) continue;
                string key = name.Replace("Localization", "");
                LangDesc.TryGetValue(key, out string desc);
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(name, GUILayout.Width(260));
                EditorGUILayout.LabelField(desc);
                EditorGUILayout.EndHorizontal();
            }
        }

        private void DrawDefaultList()
        {
            string jsonDir = EditorPath.Json.FolderFullPath;
            if (!Directory.Exists(jsonDir)) return;
            var files = Directory.GetFiles(jsonDir, "LocalizationDefault*.json");
            EditorGUILayout.LabelField("默认多语言类型数量", files.Length.ToString(), EditorStyles.boldLabel);
            foreach (string f in files)
            {
                string name = Path.GetFileNameWithoutExtension(f);
                if (name == "LocalizationDefaultLanguages") continue;
                string key = name.Replace("LocalizationDefault", "");
                LangDesc.TryGetValue(key, out string desc);
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(name, GUILayout.Width(260));
                EditorGUILayout.LabelField(desc);
                EditorGUILayout.EndHorizontal();
            }
        }

        private void DrawFontList()
        {
            EditorGUILayout.LabelField("字体类型数量", "11", EditorStyles.boldLabel);
            foreach (var kv in LangDesc)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(kv.Key, GUILayout.Width(160));
                EditorGUILayout.LabelField(kv.Value, GUILayout.Width(100));
                EditorGUILayout.LabelField("主字体：ArrowGame_SDF_Static");
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}