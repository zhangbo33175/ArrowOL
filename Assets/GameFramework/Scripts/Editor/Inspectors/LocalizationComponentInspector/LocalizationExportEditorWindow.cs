/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Editor
 * descrip:   多语言工具窗口（Honor 菜单入口）
 *            完整迁移原 Localization Inspector 全部内容
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

            var increaseToggle = Q<Toggle>("increaseToggle");
            increaseToggle.value = m_EnableIncrease;
            increaseToggle.RegisterValueChangedCallback(evt =>
            {
                m_EnableIncrease = evt.newValue;
                LocalizationComponentInspector.SaveLocalizationSettings(m_EnableIncrease, m_SingleVersion, "");
                RequestRepaint();
            });

            var singleToggle = Q<Toggle>("singleVersionToggle");
            singleToggle.value = m_SingleVersion;
            singleToggle.RegisterValueChangedCallback(evt =>
            {
                m_SingleVersion = evt.newValue;
                LocalizationComponentInspector.SaveLocalizationSettings(m_EnableIncrease, m_SingleVersion, "");
            });

            // 字体自动适配开关（纯 C# 单例，直接访问 Instance）
            var autoFontToggle = Q<Toggle>("autoFontToggle");
            autoFontToggle.value = Honor.Runtime.LocalizationComponent.Instance != null;
            autoFontToggle.RegisterValueChangedCallback(evt =>
            {
                // 纯 C# 单例，无 SerializedObject
            });

            BindImgui("mainListContainer", DrawMainList);
            BindImgui("defaultListContainer", DrawDefaultList);
            BindImgui("fontListContainer", DrawFontList);

            // IMGUIContainer 不自动撑高，显式设置高度（11行 × 20px + padding）
            Q<IMGUIContainer>("mainListContainer").style.height = 250;
            Q<IMGUIContainer>("defaultListContainer").style.height = 250;
            Q<IMGUIContainer>("fontListContainer").style.height = 240;
            RequestRepaint();
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
            string[] luaFiles = Directory.GetFiles(EditorPath.Localization.LuaScriptsFolderFullPath,
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
