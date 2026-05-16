/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Game
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  TableComponentInspector.cs
 * author:    云毅
 * created:   2026
 * descrip:   Honor框架 配置表系统编辑器扩展
 *            可视化管理Excel配置表，支持搜索、批量导出、自定义导出
 ***************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Honor.Runtime;
using UnityEditor;
using UnityEngine;

namespace Honor.Editor
{
    #region 配置表系统编辑器面板
    /// <summary>
    /// 【配置表系统编辑器面板】
    /// 功能：可视化管理 Excel 配置表，支持一键导出 Lua/Json、搜索、自定义导出、目录树展示
    /// 作用：让策划/程序直接在 Unity 里操作配置表，不用手动找文件、手动导出
    /// 属于：框架 -> 配置表系统 -> 编辑器扩展
    /// </summary>
    [CustomEditor(typeof(TableComponent))]
    internal sealed class TableComponentInspector : HonorComponentInspector
    {
        #region 目录结构记录类
        /// <summary>
        /// 【目录结构记录类】
        /// 递归存储 Excel 文件夹结构、文件列表、深度、路径
        /// </summary>
        [Serializable]
        public class DirectoryContentsRecorder
        {
            public DirectoryContentsRecorder()
            {
                DirectoryContentsRecorders = new List<DirectoryContentsRecorder>();
                ExcelFileNames = new List<string>();
                DirectoryName = string.Empty;
            }

            /// <summary>
            /// 子目录列表
            /// </summary>
            public List<DirectoryContentsRecorder> DirectoryContentsRecorders;
            
            /// <summary>
            /// 当前目录下的 Excel 文件名
            /// </summary>
            public List<string> ExcelFileNames;
            
            /// <summary>
            /// 目录名
            /// </summary>
            public string DirectoryName;
            
            /// <summary>
            /// 完整路径
            /// </summary>
            public string DirectoryFullPath;
            
            /// <summary>
            /// 层级深度（用于缩进显示）
            /// </summary>
            public int Depth = 0;
        }
        #endregion

        #region 私有字段
        /// <summary>
        /// 自定义导出用路径
        /// </summary>
        private string m_customExcelPath = null;
        private string m_customLuaPath = null;

        /// <summary>
        /// 目录树根节点
        /// </summary>
        private DirectoryContentsRecorder m_DirectoryContentsRecorder;
        
        /// <summary>
        /// Excel 文件总数
        /// </summary>
        private int m_ExcelFileCount = 0;

        /// <summary>
        /// 已生成的 Lua 表名缓存
        /// </summary>
        private Dictionary<string, bool> m_LuaFileNames;
        
        /// <summary>
        /// 折叠面板状态
        /// </summary>
        private Dictionary<string, bool> m_ExcelFoldoutSections;

        /// <summary>
        /// 框架路径（Lua/Excel 根目录）
        /// </summary>
        private string m_LuaDirectoryPath = string.Empty;
        private string m_ExcelDirectoryPath = string.Empty;

        /// <summary>
        /// 搜索相关
        /// </summary>
        private string m_SearchExcelName = string.Empty;
        private Dictionary<string, DirectoryContentsRecorder> m_SearchExcelDetailInfo;
        #endregion

        #region 初始化
        /// <summary>
        /// 初始化：加载目录、缓存文件列表
        /// </summary>
        private void OnEnable()
        {
            m_LuaFileNames = new Dictionary<string, bool>();
            m_ExcelFoldoutSections = new Dictionary<string, bool>();
            m_SearchExcelDetailInfo = new Dictionary<string, DirectoryContentsRecorder>();

            m_LuaDirectoryPath = GamePathUtils.Table.GetLuaScriptRootDirectoryFullPath();
            m_ExcelDirectoryPath = GamePathUtils.Table.GetExcelRootDirectoryFullPath();

            // 刷新目录结构 & Lua 文件列表
            UpdateExcelDirectoryContentsRecorder();
            UpdateLuaFileNames();
        }
        #endregion

        #region Inspector 绘制
        /// <summary>
        /// 绘制 Inspector 面板
        /// </summary>
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();

            DrawInfoStatistics();
            DrawBatchOperateButtons();
            DrawSearchBar();
            DrawDirectoryOrSearchResult();
            DrawCustomExportTools();

            serializedObject.ApplyModifiedProperties();
            Repaint();
        }
        #endregion

        #region 编译回调
        protected override void OnCompileStart()
        {
        }

        protected override void OnCompileComplete()
        {
        }
        #endregion

        #region 绘制 - 信息统计
        /// <summary>
        /// 绘制文件数量统计信息
        /// </summary>
        private void DrawInfoStatistics()
        {
            EditorGUILayout.BeginHorizontal("box");
            {
                EditorGUILayout.LabelField("Excel数据表数量", m_ExcelFileCount.ToString());
                EditorGUILayout.LabelField("Lua数据表数量", (m_LuaFileNames.Count).ToString());
            }
            EditorGUILayout.EndHorizontal();
        }
        #endregion

        #region 绘制 - 批量操作
        /// <summary>
        /// 绘制批量导出、打开目录等按钮
        /// </summary>
        private void DrawBatchOperateButtons()
        {
            EditorGUILayout.BeginHorizontal("box");
            {
                if (GUILayout.Button("导出所有数据表Excel到Lua"))
                {
                    RunDirectoryContentsRecorderToLua(m_DirectoryContentsRecorder);
                    UpdateExcelDirectoryContentsRecorder();
                    UpdateLuaFileNames();
                    GUIUtility.ExitGUI();
                }

                if (GUILayout.Button("打开数据表Excel所在文件夹"))
                {
                    TableExportEditorUtility.OpenDirectory(m_ExcelDirectoryPath);
                    GUIUtility.ExitGUI();
                }
            }
            EditorGUILayout.EndHorizontal();
        }
        #endregion

        #region 绘制 - 搜索框
        /// <summary>
        /// 绘制 Excel 搜索框
        /// </summary>
        private void DrawSearchBar()
        {
            GUILayout.Space(5);
            EditorGUILayout.BeginHorizontal("box");
            {
                string searchText = EditorGUILayout.TextField("搜索Excel名字：", m_SearchExcelName);
                if (searchText != m_SearchExcelName)
                {
                    m_SearchExcelName = searchText;
                    m_SearchExcelDetailInfo.Clear();
                    
                    if (!string.IsNullOrEmpty(m_SearchExcelName))
                        UpdateSearchExcelDetailInfo(m_DirectoryContentsRecorder, m_SearchExcelName.ToLower());
                }

                if (GUILayout.Button("Del", GUILayout.Width(30)))
                {
                    m_SearchExcelName = string.Empty;
                    GUIUtility.keyboardControl = 0;
                }
            }
            EditorGUILayout.EndHorizontal();
        }
        #endregion

        #region 绘制 - 目录树/搜索结果
        /// <summary>
        /// 绘制目录结构或搜索结果区域
        /// </summary>
        private void DrawDirectoryOrSearchResult()
        {
            GUILayout.Space(5);
            EditorGUILayout.BeginVertical("box");
            {
                if (string.IsNullOrEmpty(m_SearchExcelName))
                    CreateDirectoryContentsRecorderView(m_DirectoryContentsRecorder);
                else
                    CreateSearchExcelView();
            }
            EditorGUILayout.EndVertical();
        }
        #endregion

        #region 绘制 - 自定义导出工具
        /// <summary>
        /// 绘制自定义 Excel 导出功能区
        /// </summary>
        private void DrawCustomExportTools()
        {
            EditorGUILayout.Separator();
            EditorGUILayout.LabelField("自定义数据表Excel导出配置", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal("box");
            {
                EditorGUILayout.LabelField("当前选择Excel:", m_customExcelPath ?? "未选择");
            }
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("选择自定义Excel表格"))
            {
                m_customExcelPath = EditorUtility.OpenFilePanel("选择Excel", Application.dataPath, "xlsm");
                GUIUtility.ExitGUI();
            }

            if (GUILayout.Button("打开自定义Excel"))
            {
                if (File.Exists(m_customExcelPath))
                    TableExportEditorUtility.OpenExcel(m_customExcelPath);
                GUIUtility.ExitGUI();
            }

            if (GUILayout.Button("打开自定义Excel所在文件夹"))
            {
                string dir = Path.GetDirectoryName(m_customExcelPath);
                if (Directory.Exists(dir))
                    TableExportEditorUtility.OpenDirectory(dir);
                GUIUtility.ExitGUI();
            }

            if (GUILayout.Button("导出自定义Excel → Lua"))
            {
                string toPath = EditorUtility.SaveFilePanel("导出Lua", Application.dataPath, 
                    Path.GetFileNameWithoutExtension(m_customExcelPath), "lua.txt");
                
                if (!string.IsNullOrEmpty(toPath))
                    TableExportEditorUtility.ExportExcelToLua(m_customExcelPath, toPath);
                
                GUIUtility.ExitGUI();
            }

            if (GUILayout.Button("导出自定义Excel → Json"))
            {
                string toPath = EditorUtility.SaveFilePanel("导出Json", Application.dataPath, 
                    Path.GetFileNameWithoutExtension(m_customExcelPath), "json");
                
                if (!string.IsNullOrEmpty(toPath))
                    TableExportEditorUtility.ExportExcelToJson(m_customExcelPath, toPath);
                
                GUIUtility.ExitGUI();
            }
        }
        #endregion

        #region 目录数据更新
        /// <summary>
        /// 递归刷新目录结构
        /// </summary>
        private DirectoryContentsRecorder UpdateDirectoryData(string fullPath)
        {
            DirectoryContentsRecorder info = new DirectoryContentsRecorder();
            info.DirectoryFullPath = fullPath.Replace("\\", "/");
            info.DirectoryName = Path.GetFileName(fullPath);

            // 读取所有 .xlsm 文件
            info.ExcelFileNames = Directory.GetFiles(fullPath, "*.xlsm")
                .Where(f => !Path.GetFileName(f).StartsWith("~$"))
                .Select(Path.GetFileNameWithoutExtension)
                .ToList();

            m_ExcelFileCount += info.ExcelFileNames.Count;

            // 计算深度
            string relative = Path.GetRelativePath(m_ExcelDirectoryPath, fullPath);
            info.Depth = relative == "." ? 0 : relative.Split(Path.DirectorySeparatorChar).Length;

            // 折叠状态
            if (!m_ExcelFoldoutSections.ContainsKey(info.DirectoryFullPath))
                m_ExcelFoldoutSections[info.DirectoryFullPath] = false;

            // 子目录
            foreach (string sub in Directory.GetDirectories(fullPath))
                info.DirectoryContentsRecorders.Add(UpdateDirectoryData(sub));

            return info;
        }

        /// <summary>
        /// 刷新目录结构
        /// </summary>
        private void UpdateExcelDirectoryContentsRecorder()
        {
            m_ExcelFileCount = 0;
            m_DirectoryContentsRecorder = UpdateDirectoryData(m_ExcelDirectoryPath);
        }

        /// <summary>
        /// 刷新已生成 Lua 表列表
        /// </summary>
        private void UpdateLuaFileNames()
        {
            m_LuaFileNames.Clear();
            
            foreach (string f in Directory.GetFiles(m_LuaDirectoryPath, "*.lua.txt", SearchOption.AllDirectories))
            {
                string name = Path.GetFileName(f);
                if (name.EndsWith(".lua.txt"))
                    m_LuaFileNames[name[0..^8]] = true;
            }
        }
        #endregion

        #region 导出执行
        /// <summary>
        /// 递归导出整个目录
        /// </summary>
        private void RunDirectoryContentsRecorderToLua(DirectoryContentsRecorder root)
        {
            if (!Directory.Exists(m_LuaDirectoryPath))
                Directory.CreateDirectory(m_LuaDirectoryPath);

            foreach (string excel in root.ExcelFileNames)
                RunExcelToLua(excel, root.DirectoryFullPath, m_ExcelDirectoryPath, m_LuaDirectoryPath);

            foreach (var child in root.DirectoryContentsRecorders)
                RunDirectoryContentsRecorderToLua(child);
        }

        /// <summary>
        /// 单个 Excel → Lua
        /// </summary>
        private void RunExcelToLua(string excelName, string excelDir, string rootExcelDir, string rootLuaDir)
        {
            string excelPath = $"{excelDir}/{excelName}.xlsm";
            string subDir = excelDir.Replace(rootExcelDir, "");
            string luaPath = $"{rootLuaDir}/{subDir}/{excelName}.lua.txt";
            
            TableExportEditorUtility.ExportExcelToLua(excelPath, luaPath);
        }
        #endregion

        #region 绘制 - 目录与文件视图
        /// <summary>
        /// 绘制目录树
        /// </summary>
        private void CreateDirectoryContentsRecorderView(DirectoryContentsRecorder root)
        {
            foreach (var child in root.DirectoryContentsRecorders)
            {
                EditorGUILayout.BeginVertical("box");
                {
                    m_ExcelFoldoutSections[child.DirectoryFullPath] =
                        EditorGUILayout.Foldout(m_ExcelFoldoutSections[child.DirectoryFullPath], child.DirectoryName);
                    
                    if (m_ExcelFoldoutSections[child.DirectoryFullPath])
                        CreateDirectoryContentsRecorderView(child);
                }
                EditorGUILayout.EndVertical();
            }

            foreach (string excel in root.ExcelFileNames)
                CreateExcelView(excel, root);
        }

        /// <summary>
        /// 绘制单个 Excel 行
        /// </summary>
        private void CreateExcelView(string excelName, DirectoryContentsRecorder dir, bool isSearch = false)
        {
            EditorGUILayout.BeginHorizontal("box");
            {
                string space = isSearch ? "" : new string(' ', dir.Depth * 2);
                EditorGUILayout.LabelField($"{space}{excelName}.xlsm", GUILayout.MaxWidth(250));

                if (GUILayout.Button("打开"))
                {
                    TableExportEditorUtility.OpenExcel($"{dir.DirectoryFullPath}/{excelName}.xlsm");
                    GUIUtility.ExitGUI();
                }

                if (GUILayout.Button("导出Lua"))
                {
                    RunExcelToLua(excelName, dir.DirectoryFullPath, m_ExcelDirectoryPath, m_LuaDirectoryPath);
                    UpdateLuaFileNames();
                    GUIUtility.ExitGUI();
                }

                string tip = m_LuaFileNames.ContainsKey(excelName) 
                    ? $"<color=green>{excelName}.lua</color>" 
                    : "<color=red>Lua未生成</color>";
                
                EditorGUILayout.LabelField(tip);
            }
            EditorGUILayout.EndHorizontal();
        }
        #endregion

        #region 搜索功能
        /// <summary>
        /// 搜索匹配 Excel
        /// </summary>
        private void UpdateSearchExcelDetailInfo(DirectoryContentsRecorder root, string key)
        {
            foreach (string name in root.ExcelFileNames)
            {
                if (name.ToLower().Contains(key) && !m_SearchExcelDetailInfo.ContainsKey(name))
                    m_SearchExcelDetailInfo[name] = root;
            }

            foreach (var child in root.DirectoryContentsRecorders)
                UpdateSearchExcelDetailInfo(child, key);
        }

        /// <summary>
        /// 绘制搜索结果
        /// </summary>
        private void CreateSearchExcelView()
        {
            foreach (var pair in m_SearchExcelDetailInfo)
                CreateExcelView(pair.Key, pair.Value, true);
        }
        #endregion
    }
    #endregion
}