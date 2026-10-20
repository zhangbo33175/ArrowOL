/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Editor
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  TableExportEditorWindow.cs
 * author:    云毅
 * created:   2026
 * descrip:   配置表导出工具窗口（Honor 菜单入口）
 *            UXML/USS 外壳 + IMGUIContainer 托管目录树
 ***************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Honor.Runtime;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Honor.Editor
{
    /// <summary>
    /// 配置表导出工具窗口。通过 Honor → 数据表工具 打开。
    /// </summary>
    public class TableExportEditorWindow : BaseToolkitEditorWindow<TableExportEditorWindow>
    {
        #region 目录结构记录
        [Serializable]
        private class DirectoryContentsRecorder
        {
            public List<DirectoryContentsRecorder> Children = new List<DirectoryContentsRecorder>();
            public List<string> ExcelFileNames = new List<string>();
            public string DirectoryName = string.Empty;
            public string DirectoryFullPath = string.Empty;
            public int Depth;
        }
        #endregion

        #region 字段
        private string m_CustomExcelPath;
        private DirectoryContentsRecorder m_Root;
        private int m_ExcelFileCount;
        private readonly Dictionary<string, bool> m_LuaFileNames = new Dictionary<string, bool>();
        private readonly Dictionary<string, bool> m_Foldouts = new Dictionary<string, bool>();
        private string m_LuaDir;
        private string m_ExcelDir;
        private string m_SearchName = string.Empty;
        private readonly Dictionary<string, DirectoryContentsRecorder> m_SearchResults = new Dictionary<string, DirectoryContentsRecorder>();
        private Vector2 m_TreeScroll;

        private static GUIStyle _richLabel;
        private static GUIStyle RichLabel => _richLabel ??= new GUIStyle(EditorStyles.label) { richText = true };
        #endregion

        #region 基类抽象属性
        /// <inheritdoc/>
        protected override string UIAssetDir =>
            "Assets/GameFramework/Scripts/Editor/Inspectors/TableComponentInspector/";

        /// <inheritdoc/>
        protected override string UxmlName => "TableExportEditorWindowTool";

        /// <inheritdoc/>
        protected override GUIContent Title => new GUIContent("数据表工具");

        /// <inheritdoc/>
        protected override Vector2 MinSize => new Vector2(750, 500);

        /// <inheritdoc/>
        protected override Vector2 MaxSize => new Vector2(750, 500);
        #endregion

        #region 菜单入口
        [MenuItem("Honor/数据表工具", false, 360)]
        private static void OpenWindow() => Open();
        #endregion

        #region 基类钩子
        /// <inheritdoc/>
        protected override void OnInitData()
        {
            m_LuaDir = GamePathUtils.Table.GetLuaScriptRootDirectoryFullPath();
            m_ExcelDir = GamePathUtils.Table.GetExcelRootDirectoryFullPath();
            RefreshAll();
        }

        /// <inheritdoc/>
        protected override void OnBindUI()
        {
            UpdateHeaderLabels();

            // 批量操作
            BindClick("exportAllBtn", () =>
            {
                ExportAllToLua(m_Root);
                RefreshAll();
                UpdateHeaderLabels();
                RequestRepaint();
            });
            BindClick("openExcelDirBtn", () => TableExportEditorUtility.OpenDirectory(m_ExcelDir));

            // 搜索
            var searchField = rootVisualElement.Q<TextField>("searchField");
            // TODO: 加固健壮性——searchField 查不到时原写法直接空引用，加判空
            if (searchField != null)
            {
                searchField.RegisterValueChangedCallback(evt =>
                {
                    m_SearchName = evt.newValue;
                    m_SearchResults.Clear();
                    if (!string.IsNullOrEmpty(m_SearchName))
                        BuildSearchResults(m_Root, m_SearchName.ToLower());
                    RequestRepaint();
                });
            }
            BindClick("clearSearchBtn", () =>
            {
                m_SearchName = string.Empty;
                if (searchField != null) searchField.value = string.Empty;
                m_SearchResults.Clear();
                RequestRepaint();
            });

            // 自定义导出
            BindClick("selectCustomBtn", () =>
            {
                string path = EditorUtility.OpenFilePanel("选择 Excel", Application.dataPath, "xlsm");
                if (!string.IsNullOrEmpty(path))
                {
                    m_CustomExcelPath = path;
                    rootVisualElement.Q<Label>("customPathLabel").text = path;
                }
            });
            BindClick("openCustomBtn", () =>
            {
                if (File.Exists(m_CustomExcelPath))
                    TableExportEditorUtility.OpenExcel(m_CustomExcelPath);
            });
            BindClick("openCustomDirBtn", () =>
            {
                string dir = Path.GetDirectoryName(m_CustomExcelPath);
                if (Directory.Exists(dir))
                    TableExportEditorUtility.OpenDirectory(dir);
            });
            BindClick("exportCustomLuaBtn", () =>
            {
                if (string.IsNullOrEmpty(m_CustomExcelPath)) return;
                string to = EditorUtility.SaveFilePanel("导出 Lua", Application.dataPath,
                    Path.GetFileNameWithoutExtension(m_CustomExcelPath), "lua.txt");
                if (!string.IsNullOrEmpty(to))
                    TableExportEditorUtility.ExportExcelToLua(m_CustomExcelPath, to);
            });
            BindClick("exportCustomJsonBtn", () =>
            {
                if (string.IsNullOrEmpty(m_CustomExcelPath)) return;
                string to = EditorUtility.SaveFilePanel("导出 Json", Application.dataPath,
                    Path.GetFileNameWithoutExtension(m_CustomExcelPath), "json");
                if (!string.IsNullOrEmpty(to))
                    TableExportEditorUtility.ExportExcelToJson(m_CustomExcelPath, to);
            });

            BindImgui("treeContainer", DrawTreeIMGUIContent);
            EditorApplication.update += LateTick;
            RequestRepaint();
        }
        #endregion

        #region UI 更新
        private bool m_NeedsHeaderUpdate;

        private void UpdateHeaderLabels()
        {
            // TODO: 加固健壮性——头部 Label 查不到时原写法直接空引用，加判空
            var excelCountLabel = rootVisualElement.Q<Label>("excelCountLabel");
            if (excelCountLabel != null) excelCountLabel.text = $"Excel 数量: {m_ExcelFileCount}";
            var luaCountLabel = rootVisualElement.Q<Label>("luaCountLabel");
            if (luaCountLabel != null) luaCountLabel.text = $"Lua 数量: {m_LuaFileNames.Count}";
        }

        /// <summary>标记需要在下一帧更新头部标签（避免在 IMGUI 事件中直接修改视觉树）</summary>
        private void MarkHeaderDirty()
        {
            m_NeedsHeaderUpdate = true;
        }

        private void LateTick()
        {
            if (m_NeedsHeaderUpdate)
            {
                m_NeedsHeaderUpdate = false;
                UpdateHeaderLabels();
            }
        }
        #endregion

        #region IMGUI 绘制（目录树）
        private void DrawTreeIMGUIContent()
        {
            m_TreeScroll = EditorGUILayout.BeginScrollView(m_TreeScroll);
            {
                if (string.IsNullOrEmpty(m_SearchName))
                    DrawDirectory(m_Root);
                else
                    DrawSearchResults();
            }
            EditorGUILayout.EndScrollView();
        }

        private void DrawDirectory(DirectoryContentsRecorder dir)
        {
            foreach (var child in dir.Children)
            {
                m_Foldouts[child.DirectoryFullPath] =
                    EditorGUILayout.Foldout(m_Foldouts[child.DirectoryFullPath], child.DirectoryName);
                if (m_Foldouts[child.DirectoryFullPath])
                {
                    using (new EditorGUI.IndentLevelScope(1))
                        DrawDirectory(child);
                }
            }

            foreach (string excel in dir.ExcelFileNames)
                DrawExcelRow(excel, dir, false);
        }

        private void DrawExcelRow(string excelName, DirectoryContentsRecorder dir, bool isSearch)
        {
            using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox))
            {
                string indent = isSearch ? string.Empty : new string(' ', dir.Depth * 2);
                EditorGUILayout.LabelField($"{indent}{excelName}.xlsm", GUILayout.MinWidth(180), GUILayout.Height(22));

                if (GUILayout.Button("打开", GUILayout.Width(60), GUILayout.Height(22)))
                    TableExportEditorUtility.OpenExcel($"{dir.DirectoryFullPath}/{excelName}.xlsm");

                if (GUILayout.Button("导出Lua", GUILayout.Width(60), GUILayout.Height(22)))
                {
                    ExportSingleToLua(excelName, dir.DirectoryFullPath);
                    RefreshLuaList();
                    MarkHeaderDirty();
                    RequestRepaint();
                }

                string tip = m_LuaFileNames.ContainsKey(excelName)
                    ? "<color=green>●</color>"
                    : "<color=red>●</color>";
                EditorGUILayout.LabelField(tip, RichLabel, GUILayout.Width(24), GUILayout.Height(22));
            }
        }

        private void DrawSearchResults()
        {
            foreach (var pair in m_SearchResults)
                DrawExcelRow(pair.Key, pair.Value, true);
        }
        #endregion

        #region 数据刷新
        private void RefreshAll()
        {
            m_ExcelFileCount = 0;
            m_Root = ScanDirectory(m_ExcelDir);
            RefreshLuaList();
        }

        private DirectoryContentsRecorder ScanDirectory(string fullPath)
        {
            var info = new DirectoryContentsRecorder
            {
                DirectoryFullPath = fullPath.Replace("\\", "/"),
                DirectoryName = Path.GetFileName(fullPath)
            };

            info.ExcelFileNames = Directory.GetFiles(fullPath, "*.xlsm")
                .Where(f => !Path.GetFileName(f).StartsWith("~$"))
                .Select(Path.GetFileNameWithoutExtension)
                .ToList();

            m_ExcelFileCount += info.ExcelFileNames.Count;

            string rel = Path.GetRelativePath(m_ExcelDir, fullPath);
            info.Depth = rel == "." ? 0 : rel.Split(Path.DirectorySeparatorChar).Length;

            if (!m_Foldouts.ContainsKey(info.DirectoryFullPath))
                m_Foldouts[info.DirectoryFullPath] = false;

            foreach (string sub in Directory.GetDirectories(fullPath))
                info.Children.Add(ScanDirectory(sub));

            return info;
        }

        private void RefreshLuaList()
        {
            m_LuaFileNames.Clear();
            foreach (string f in Directory.GetFiles(m_LuaDir, "*.lua.txt", SearchOption.AllDirectories))
            {
                string name = Path.GetFileName(f);
                if (name.EndsWith(".lua.txt"))
                    m_LuaFileNames[name[..^8]] = true;
            }
        }

        private void BuildSearchResults(DirectoryContentsRecorder dir, string key)
        {
            foreach (string name in dir.ExcelFileNames)
            {
                if (name.ToLower().Contains(key) && !m_SearchResults.ContainsKey(name))
                    m_SearchResults[name] = dir;
            }
            foreach (var child in dir.Children)
                BuildSearchResults(child, key);
        }
        #endregion

        #region 导出
        private void ExportAllToLua(DirectoryContentsRecorder dir)
        {
            if (!Directory.Exists(m_LuaDir))
                Directory.CreateDirectory(m_LuaDir);

            foreach (string excel in dir.ExcelFileNames)
                ExportSingleToLua(excel, dir.DirectoryFullPath);

            foreach (var child in dir.Children)
                ExportAllToLua(child);
        }

        private void ExportSingleToLua(string excelName, string excelDir)
        {
            string excelPath = $"{excelDir}/{excelName}.xlsm";
            string subDir = excelDir.Replace(m_ExcelDir, "");
            string luaPath = $"{m_LuaDir}/{subDir}/{excelName}.lua.txt";
            TableExportEditorUtility.ExportExcelToLua(excelPath, luaPath);
        }
        #endregion
    }
}
