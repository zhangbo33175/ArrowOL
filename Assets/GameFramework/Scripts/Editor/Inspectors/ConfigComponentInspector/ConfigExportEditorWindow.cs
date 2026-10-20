/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Editor
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  ConfigExportEditorWindow.cs
 * author:    云毅
 * created:   2026
 * descrip:   配置表导出工具窗口（Honor 菜单入口）
 *            UXML/USS 外壳 + IMGUIContainer 托管配置预览
 ***************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using Honor.Runtime;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Honor.Editor
{
    /// <summary>
    /// 配置表导出工具窗口。Honor → 配置表工具。
    /// </summary>
    public class ConfigExportEditorWindow : BaseToolkitEditorWindow<ConfigExportEditorWindow>
    {
        #region 字段
        private Dictionary<string, List<string>> m_ConfigEntryMap = new Dictionary<string, List<string>>();
        private Vector2 m_Scroll;
        #endregion

        #region 基类抽象属性
        /// <inheritdoc/>
        protected override string UIAssetDir =>
            "Assets/GameFramework/Scripts/Editor/Inspectors/ConfigComponentInspector/";

        /// <inheritdoc/>
        protected override string UxmlName => "ConfigExportEditorWindowTool";

        /// <inheritdoc/>
        protected override GUIContent Title => new GUIContent("配置表工具");

        /// <inheritdoc/>
        protected override Vector2 MinSize => new Vector2(700, 500);

        /// <inheritdoc/>
        protected override Vector2 MaxSize => new Vector2(700, 500);
        #endregion

        #region 菜单入口
        [MenuItem("Honor/配置表工具", false, 361)]
        private static void OpenWindow() => Open();
        #endregion

        #region 基类钩子
        /// <inheritdoc/>
        protected override void OnInitData()
        {
            ReadConfigEntries();
        }

        /// <inheritdoc/>
        protected override void OnBindUI()
        {
            BindClick("openExcelBtn", () =>
                TableExportEditorUtility.OpenExcel(GamePathUtils.Config.GetExcelFileFullPath()));

            BindClick("exportBtn", () =>
            {
                ExportCurrentExcel();
                ReadConfigEntries();
                RequestRepaint();
            });

            BindClick("openDirBtn", () =>
                TableExportEditorUtility.OpenDirectory(GamePathUtils.Config.GetExcelRootDirectoryFullPath()));

            BindImgui("previewContainer", DrawPreviewIMGUIContent);
            RequestRepaint();
        }
        #endregion

        #region IMGUI 绘制
        private void DrawPreviewIMGUIContent()
        {
            m_Scroll = EditorGUILayout.BeginScrollView(m_Scroll);
            {
                if (m_ConfigEntryMap.Count == 0)
                {
                    EditorGUILayout.HelpBox("配置文件不存在，请检查是否已生成。", MessageType.Warning);
                }
                else
                {
                    DrawConfigColumn("开发环境配置", 0);
                    DrawConfigColumn("生产环境配置", 1);
                }
            }
            EditorGUILayout.EndScrollView();
        }

        private void DrawConfigColumn(string title, int envIndex)
        {
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField(title, EditorStyles.boldLabel);
            foreach (var entry in m_ConfigEntryMap)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(entry.Key, GUILayout.Width(200));
                EditorGUILayout.LabelField(entry.Value[envIndex]);
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
        }
        #endregion

        #region 配置读取/导出
        private bool ReadConfigEntries()
        {
            m_ConfigEntryMap.Clear();
            string configBytesPath = AorTxt.Format("{0}/{1}/{2}",
                Application.dataPath.Substring(0, Application.dataPath.Length - "Assets".Length),
                GamePathUtils.Json.GetRootDirectoryRelativePath(),
                "Configs.bytes");

            if (!File.Exists(configBytesPath)) return false;

            byte[] rawBytes = File.ReadAllBytes(configBytesPath);
            byte[] decryptedBytes = Encryption.GetQuickXorBytes(rawBytes, ConfigComponent.s_ConfigEncrytionKey);
            string jsonContent = Converter.GetString(decryptedBytes);

            JObject configJson = JObject.Parse(jsonContent);
            foreach (var configEntry in configJson)
            {
                var envValues = new List<string>();
                foreach (var envToken in configEntry.Value)
                    envValues.Add(envToken.ToString());
                m_ConfigEntryMap[configEntry.Key] = envValues;
            }
            return true;
        }

        private void ExportCurrentExcel()
        {
            string excelPath = GamePathUtils.Config.GetExcelFileFullPath();
            string excelName = Path.GetFileNameWithoutExtension(excelPath);
            string fullExcelPath = $"{GamePathUtils.Config.GetExcelRootDirectoryFullPath()}/{excelName}.xlsm";
            DataSet ds = TableExportEditorUtility.GetExcelData(fullExcelPath);

            string outputDir = GamePathUtils.Json.GetRootDirectoryFullPath();
            string outputPath = $"{outputDir}/{excelName}.bytes";
            if (!Directory.Exists(outputDir)) Directory.CreateDirectory(outputDir);

            var sb = new StringBuilder();
            sb.AppendLine("{");
            int rowCount = ds.Tables[0].Rows.Count;
            for (int i = 4; i < rowCount; i++)
            {
                var row = ds.Tables[0].Rows[i];
                string rowText = $"    \"{row[1]}\":[\"{row[3]}\",\"{row[4]}\"]";
                if (i < rowCount - 1) rowText += ",";
                sb.AppendLine(rowText);
            }
            sb.AppendLine("}");

            if (File.Exists(outputPath)) File.Delete(outputPath);
            byte[] plain = Converter.GetBytesByString(sb.ToString());
            byte[] encrypted = Encryption.GetQuickXorBytes(plain, ConfigComponent.s_ConfigEncrytionKey);
            File.WriteAllBytes(outputPath, encrypted);
            AssetDatabase.Refresh();
        }
        #endregion
    }
}
