using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Honor.Runtime;
using UnityEditor;
using UnityEngine;

namespace Honor.Editor
{
    [CustomEditor(typeof(TableComponent))]
    internal sealed class TableComponentInspector : HonorComponentInspector
    {
        /// <summary>
        /// 文件夹内容读取类
        /// </summary>
        public class DirectoryContentsRecorder
        {
            public DirectoryContentsRecorder()
            {
                DirectoryContentsRecorders = new List<DirectoryContentsRecorder>();
                ExcelFileNames = new List<string>();
                DirectoryName = string.Empty;
            }

            /// <summary>
            /// 文件夹的目录信息
            /// </summary>
            public List<DirectoryContentsRecorder> DirectoryContentsRecorders;

            /// <summary>
            /// 当前文件包含的所有Excel文件
            /// </summary>
            public List<string> ExcelFileNames;

            /// <summary>
            /// 当前文件夹的名字
            /// </summary>
            public string DirectoryName;

            /// <summary>
            /// 文件夹的完整目录
            /// </summary>
            public string DirectoryFullPath;

            /// <summary>
            /// 目录层级深度
            /// </summary>
            public int Depth = 0;
        }

        /// <summary>
        /// 自定义ExcelPath
        /// </summary>
        private string m_customExcelPath = null;

        /// <summary>
        /// 自定义luaPath
        /// </summary>
        private string m_customLuaPath = null;

        /// <summary>
        /// 目录文件信息
        /// </summary>
        private DirectoryContentsRecorder m_DirectoryContentsRecorder;

        /// <summary>
        /// Excel文件数量
        /// </summary>
        private int m_ExcelFileCount = 0;

        /// <summary>
        /// Lua Table目录的文件记录<文件名，true>
        /// </summary>
        private Dictionary<string, bool> m_LuaFileNames;

        /// <summary>
        /// 折叠控件的 展开/关闭 记录
        /// </summary>
        private Dictionary<string, bool> m_ExcelFoldoutSections;

        /// <summary>
        /// 获取Tables的Lua脚本根目录的绝对路径
        /// </summary>
        private string m_LuaDirectoryPath = String.Empty;

        /// <summary>
        /// 获取Tables的Excel根目录的绝对路径
        /// </summary>
        private string m_ExcelDirectoryPath = String.Empty;

        /// <summary>
        /// 搜索excel名字
        /// </summary>
        private string m_SearchExcelName = string.Empty;

        private Dictionary<string, DirectoryContentsRecorder> m_SearchExcelDetailInfo;

        private void OnEnable()
        {
            m_LuaFileNames = new Dictionary<string, bool>();
            m_ExcelFoldoutSections = new Dictionary<string, bool>();
            m_SearchExcelDetailInfo = new Dictionary<string, DirectoryContentsRecorder>();
            m_LuaDirectoryPath = GamePathUtils.Table.GetLuaScriptRootDirectoryFullPath();
            m_ExcelDirectoryPath = GamePathUtils.Table.GetExcelRootDirectoryFullPath();
            UpdateExcelDirectoryContentsRecorder();
            UpdateLuaFileNames();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            EditorGUILayout.BeginHorizontal("box");
            {
                EditorGUILayout.LabelField("Excel数据表数量", m_ExcelFileCount.ToString());
                if (m_LuaFileNames.Count == 0)
                {
                    EditorGUILayout.LabelField("Lua数据表数量", "0");
                }
                else
                {
                    EditorGUILayout.LabelField("Lua数据表数量", (m_LuaFileNames.Count - 1).ToString());
                }

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.BeginHorizontal("box");
            {
                // Excel导出（一键批处理导出所有Table目录下的Excel到Lua）
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

                EditorGUILayout.EndHorizontal();
            }

            GUILayout.Space(5);
            EditorGUILayout.BeginHorizontal("box");
            {
                // 搜索框
                var tempSearchExcelName = EditorGUILayout.TextField("搜索Excel名字：", m_SearchExcelName);
                if (tempSearchExcelName != m_SearchExcelName)
                {
                    m_SearchExcelName = tempSearchExcelName;
                    m_SearchExcelDetailInfo.Clear();
                    if (m_SearchExcelName != string.Empty)
                    {
                        UpdateSearchExcelDetailInfo(m_DirectoryContentsRecorder, m_SearchExcelName.ToLower());
                    }
                }

                // 删除搜索框里面的内容
                if (GUILayout.Button("Del", GUILayout.Width(30)))
                {
                    m_SearchExcelName = string.Empty;
                    GUIUtility.keyboardControl = 0; // 强制焦点切换,不然会有输入框文字不清空需要点击下空白地方才能清空问题
                }

                EditorGUILayout.EndHorizontal();
            }

            GUILayout.Space(5);
            EditorGUILayout.BeginVertical("box");
            {
                if (m_SearchExcelName == String.Empty)
                {
                    // Tables的Excel根目录的下目录信息
                    CreateDirectoryContentsRecorderView(m_DirectoryContentsRecorder);
                }
                else
                {
                    // 创建搜索框内名字符合的Excel
                    CreateSearchExcelView();
                }

                EditorGUILayout.EndVertical();
            }

            if (m_LuaFileNames.Count == 0)
            {
                EditorGUILayout.HelpBox("缺少Tables.lua脚本！请生成任意lua数据表脚本来修复此问题！", MessageType.Error);
                return;
            }

            EditorGUILayout.Separator();

            // 自定义导出
            EditorGUILayout.LabelField("自定义数据表Excel导出配置");

            EditorGUILayout.BeginHorizontal("box");
            {
                EditorGUILayout.LabelField("数据表Excel位置:", m_customExcelPath != null ? m_customExcelPath : "");
                EditorGUILayout.EndHorizontal();
            }


            EditorGUI.BeginDisabledGroup(false);
            {
                if (GUILayout.Button("选择自定义Excel表格"))
                {
                    m_customExcelPath = EditorUtility.OpenFilePanel("选择要导出的数据表Excel", Application.dataPath, "xlsm");
                    GUIUtility.ExitGUI();
                }

                EditorGUI.EndDisabledGroup();
            }


            EditorGUI.BeginDisabledGroup(string.IsNullOrEmpty(m_customExcelPath));
            {
                if (GUILayout.Button("打开自定义Excel"))
                {
                    if (string.IsNullOrEmpty(m_customExcelPath))
                    {
                        Log.Error("打开自定义Excel失败，未设置需要导出的Excel表格");
                    }
                    else
                    {
                        if (File.Exists(m_customExcelPath))
                        {
                            TableExportEditorUtility.OpenExcel(m_customExcelPath);
                        }
                        else
                        {
                            Log.Error($"要打开的数据表Excel {m_customExcelPath}不存在");
                        }
                    }

                    GUIUtility.ExitGUI();
                }

                EditorGUI.EndDisabledGroup();
            }


            EditorGUI.BeginDisabledGroup(string.IsNullOrEmpty(m_customExcelPath));
            {
                if (GUILayout.Button("打开自定义Excel文件夹"))
                {
                    if (string.IsNullOrEmpty(m_customExcelPath))
                    {
                        Log.Error("打开自定义Excel失败，未设置需要导出的Excel表格");
                    }
                    else
                    {
                        string directoryPath = System.IO.Path.GetDirectoryName(m_customExcelPath);
                        if (Directory.Exists(directoryPath))
                        {
                            TableExportEditorUtility.OpenDirectory(directoryPath);
                        }
                        else
                        {
                            Log.Error($"要打开的文件夹{directoryPath}不存在");
                        }
                    }

                    GUIUtility.ExitGUI();
                }

                EditorGUI.EndDisabledGroup();
            }


            EditorGUI.BeginDisabledGroup(string.IsNullOrEmpty(m_customExcelPath));
            {
                if (GUILayout.Button("导出自定义数据表Excel到Lua"))
                {
                    if (string.IsNullOrEmpty(m_customExcelPath))
                    {
                        Log.Error("导出自定义数据表Excel到Lua失败，未设置需要导出的Excel表格");
                    }
                    else if (!File.Exists(m_customExcelPath))
                    {
                        Log.Error("导出自定义数据表Excel到Lua失败，选中的Excel表格不存在");
                    }
                    else
                    {
                        string excelName = System.IO.Path.GetFileNameWithoutExtension(m_customExcelPath);
                        //bool needReg = TableExportEditorUtility.IsDirectoryInLuaDirectory(m_customLuaPath);
                        string toLuaPath = EditorUtility.SaveFilePanel("导出自定义数据表Excel到Lua", Application.dataPath,
                            excelName, "lua.txt");
                        if (!string.IsNullOrEmpty(toLuaPath))
                        {
                            TableExportEditorUtility.ExportExcelToLua(m_customExcelPath, toLuaPath);
                        }
                        else
                        {
                            Log.Error("自定义数据表Excel导出lua失败，未设置导出路径");
                        }
                    }

                    GUIUtility.ExitGUI();
                }

                EditorGUI.EndDisabledGroup();
            }


            EditorGUI.BeginDisabledGroup(string.IsNullOrEmpty(m_customExcelPath));
            {
                if (GUILayout.Button("导出自定义数据表Excel到Json"))
                {
                    if (string.IsNullOrEmpty(m_customExcelPath))
                    {
                        Log.Error("导出自定义数据表Excel到Json失败，未设置需要导出的Excel表格");
                    }
                    else if (!File.Exists(m_customExcelPath))
                    {
                        Log.Error("导出自定义数据表Excel到Json失败，选中的Excel表格不存在");
                    }
                    else
                    {
                        string excelName = System.IO.Path.GetFileNameWithoutExtension(m_customExcelPath);
                        string toJsonPath = EditorUtility.SaveFilePanel("导出自定义数据表Excel到Json", Application.dataPath,
                            excelName, "json");
                        if (!string.IsNullOrEmpty(toJsonPath))
                        {
                            TableExportEditorUtility.ExportExcelToJson(m_customExcelPath, toJsonPath);
                        }
                        else
                        {
                            Log.Error("自定义数据表Excel导出json失败，未设置导出路径");
                        }
                    }

                    GUIUtility.ExitGUI();
                }

                EditorGUI.EndDisabledGroup();
            }

            serializedObject.ApplyModifiedProperties();

            Repaint();
        }

        /// <summary>
        /// 更新当前表格目录结构
        /// </summary>
        /// <param name="directoryFullPath"></param>
        /// <param name="depth"></param>
        /// <returns></returns>
        private DirectoryContentsRecorder UpdateDirectoryData(string directoryFullPath)
        {
            if (string.IsNullOrEmpty(directoryFullPath))
            {
                return null;
            }
            string relativePath = System.IO.Path.GetRelativePath(m_ExcelDirectoryPath, directoryFullPath);
            var directoryDetailInfo = new DirectoryContentsRecorder();
            directoryDetailInfo.ExcelFileNames = Directory.GetFiles(directoryFullPath, "*.xlsm").Where(file =>
                    !System.IO.Path.GetFileName(file).StartsWith("~$", StringComparison.Ordinal))
                .Select(filePath => System.IO.Path.GetFileNameWithoutExtension(filePath)).ToList();
            directoryDetailInfo.DirectoryName = System.IO.Path.GetFileNameWithoutExtension(directoryFullPath);
            directoryDetailInfo.DirectoryFullPath = directoryFullPath.Replace("\\", "/");
            m_ExcelFileCount += directoryDetailInfo.ExcelFileNames.Count;
            if (m_ExcelDirectoryPath == directoryFullPath)
            {
                directoryDetailInfo.Depth = 0;
            }
            else
            {
                directoryDetailInfo.Depth = relativePath.Split(System.IO.Path.DirectorySeparatorChar).Length;
            }

            if (m_ExcelFoldoutSections.ContainsKey(directoryDetailInfo.DirectoryFullPath) == false)
            {
                m_ExcelFoldoutSections.Add(directoryDetailInfo.DirectoryFullPath, false);
            }

            string[] subDirectories = Directory.GetDirectories(directoryFullPath);
            if (subDirectories.Length > 0)
            {
                foreach (var subDirectory in subDirectories)
                {
                    directoryDetailInfo.DirectoryContentsRecorders.Add(UpdateDirectoryData(subDirectory));
                }
            }

            return directoryDetailInfo;
        }


        /// <summary>
        /// 更新Excel目录文件信息
        /// </summary>
        private void UpdateExcelDirectoryContentsRecorder()
        {
            m_ExcelFileCount = 0;
            m_DirectoryContentsRecorder = UpdateDirectoryData(m_ExcelDirectoryPath);
        }

        /// <summary>
        /// 更新已经存在的lua文件
        /// </summary>
        private void UpdateLuaFileNames()
        {
            m_LuaFileNames.Clear();
            Directory.GetFiles(m_LuaDirectoryPath, "*.lua.txt", SearchOption.AllDirectories).ToList().ForEach(file =>
            {
                string fileNames = System.IO.Path.GetFileName(file);
                if (fileNames.StartsWith("Table", StringComparison.Ordinal))
                {
                    m_LuaFileNames.Add(fileNames.Substring(0, fileNames.Length - 8), true);
                }
            });
        }

        /// <summary>
        /// 一键批处理导出所有Table目录下的Excel到Lua
        /// </summary>
        /// <param name="directoryContentsRecorder"></param>
        private void RunDirectoryContentsRecorderToLua(DirectoryContentsRecorder directoryContentsRecorder)
        {
            if (!Directory.Exists(m_LuaDirectoryPath))
            {
                Directory.CreateDirectory(m_LuaDirectoryPath);
            }

            directoryContentsRecorder.ExcelFileNames.ForEach(excelFileName =>
            {
                RunExcelToLua(excelFileName, directoryContentsRecorder.DirectoryFullPath, m_ExcelDirectoryPath,
                    m_LuaDirectoryPath);
            });

            foreach (var recorder in directoryContentsRecorder.DirectoryContentsRecorders)
            {
                RunDirectoryContentsRecorderToLua(recorder);
            }
        }

        /// <summary>
        /// 执行单个excel到lua
        /// </summary>
        /// <param name="excelFileName"></param>
        /// <param name="DirectoryFullPath"></param>
        /// <param name="excelDirectoryPath"></param>
        /// <param name="luaDirectoryPath"></param>
        private void RunExcelToLua(string excelFileName, string DirectoryFullPath, string excelDirectoryPath,
            string luaDirectoryPath)
        {
            string excelPath = $"{DirectoryFullPath}/{excelFileName}.xlsm";
            string distancePath = string.Empty;
            if (DirectoryFullPath != excelDirectoryPath)
            {
                distancePath = $"{DirectoryFullPath.Replace(excelDirectoryPath, string.Empty)}/";
            }

            string luaPath = $"{luaDirectoryPath}/{distancePath}{excelFileName}.lua.txt";
            TableExportEditorUtility.ExportExcelToLua(excelPath, luaPath);
        }


        /// <summary>
        /// 创建excel列表显示条目
        /// </summary>
        /// <param name="directoryContentsRecorder"></param>
        /// <param name="isFirst"></param>
        private void CreateDirectoryContentsRecorderView(DirectoryContentsRecorder directoryContentsRecorder)
        {
            if (directoryContentsRecorder == null)
            {
                return;
            }

            foreach (var recorder in directoryContentsRecorder.DirectoryContentsRecorders)
            {
                EditorGUILayout.BeginVertical("box");
                {
                    m_ExcelFoldoutSections[recorder.DirectoryFullPath] =
                        EditorGUILayout.Foldout(m_ExcelFoldoutSections[recorder.DirectoryFullPath],
                            recorder.DirectoryName);
                    if (m_ExcelFoldoutSections[recorder.DirectoryFullPath])
                    {
                        CreateDirectoryContentsRecorderView(recorder);
                    }
                }
                EditorGUILayout.EndVertical();
            }

            // 创建Excel的显示view
            directoryContentsRecorder.ExcelFileNames.ForEach(excelFileName =>
                CreateExcelView(excelFileName, directoryContentsRecorder));
        }

        /// <summary>
        /// 创建单个Excel的显示条目
        /// </summary>
        /// <param name="excelFileName">excel名字</param>
        /// <param name="directoryContentsRecorder">excel所在的目录信息</param>
        /// <param name="isSearchView">是否是搜索view</param>
        private void CreateExcelView(string excelFileName, DirectoryContentsRecorder directoryContentsRecorder,
            bool isSearchView = false)
        {
            EditorGUILayout.BeginHorizontal("box");
            {
                var spaceString = isSearchView ? "" : new string(' ', directoryContentsRecorder.Depth * 2);
                EditorGUILayout.LabelField(string.Concat(spaceString, $"{excelFileName}.xlsm"),
                    GUILayout.MaxWidth(250));
                if (GUILayout.Button("打开数据表Excel"))
                {
                    TableExportEditorUtility.OpenExcel(
                        $"{directoryContentsRecorder.DirectoryFullPath}/{excelFileName}.xlsm");
                    GUIUtility.ExitGUI();
                }

                if (GUILayout.Button("导出数据表Excel到Lua"))
                {
                    if (!Directory.Exists(m_LuaDirectoryPath))
                    {
                        Directory.CreateDirectory(m_LuaDirectoryPath);
                    }

                    RunExcelToLua(excelFileName, directoryContentsRecorder.DirectoryFullPath, m_ExcelDirectoryPath,
                        m_LuaDirectoryPath);
                    UpdateLuaFileNames();
                    GUIUtility.ExitGUI();
                }

                if (m_LuaFileNames.ContainsKey(excelFileName))
                {
                    EditorGUILayout.LabelField(AorTxt.Format("===> {0}.lua", excelFileName));
                }
                else
                {
                    EditorGUILayout.LabelField("===> Lua文件未生成!!!");
                }

                EditorGUILayout.EndHorizontal();
            }
        }


        /// <summary>
        /// 更新符合名字的条目信息
        /// </summary>
        /// <param name="directoryContentsRecorder"></param>
        /// <param name="searchName"></param>
        private void UpdateSearchExcelDetailInfo(DirectoryContentsRecorder directoryContentsRecorder, string searchName)
        {
            directoryContentsRecorder.ExcelFileNames.ForEach(fileName =>
            {
                if (fileName.ToLower().Contains(searchName))
                {
                    if (m_SearchExcelDetailInfo.ContainsKey(fileName) == false)
                    {
                        m_SearchExcelDetailInfo.Add(fileName, directoryContentsRecorder);
                    }
                }
            });
            directoryContentsRecorder.DirectoryContentsRecorders.ForEach(recorder =>
                UpdateSearchExcelDetailInfo(recorder, searchName));
        }

        /// <summary>
        /// 创建搜索框的view
        /// </summary>
        private void CreateSearchExcelView()
        {
            m_SearchExcelDetailInfo.Keys.ToList().ForEach(fileName =>
                CreateExcelView(fileName, m_SearchExcelDetailInfo[fileName], true));
        }


        protected override void OnCompileStart()
        {
            base.OnCompileStart();
        }

        protected override void OnCompileComplete()
        {
            base.OnCompileComplete();
        }
    }
}