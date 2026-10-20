/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Editor
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  ResDefExportEditorWindow.cs
 * author:    云毅
 * created:   2026
 * descrip:   资源配置导出工具 - UI Toolkit 界面（分部类）
 *            外壳由 UXML/USS 描述，动态列表通过 IMGUIContainer 托管
 ***************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using Honor.Runtime;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;
using Path = System.IO.Path;

namespace Honor.Editor
{
    public partial class ResDefExportEditorWindow : BaseToolkitEditorWindow<ResDefExportEditorWindow>
    {
        #region 基类抽象属性
        /// <summary>UXML/USS 资源所在目录（相对工程根）</summary>
        protected override string UIAssetDir =>
            "Assets/GameFramework/Scripts/Editor/Inspectors/ResDefExportEditorWindow/";

        /// <summary>UXML 文件名（不含扩展名）</summary>
        protected override string UxmlName => "ResDefExportEditorWindowTool";
        #endregion

        #region 视觉样式（IMGUI 行渲染用，懒加载缓存）
        private static GUIStyle _tableHeaderLabel;
        private static GUIStyle _logRed;
        private static GUIStyle _logYellow;
        private static GUIStyle _logCyan;
        private static GUIStyle _logNormal;

        private static readonly Color ButtonBlue = new Color(0.30f, 0.55f, 0.78f);
        private static readonly Color ButtonRed  = new Color(0.66f, 0.27f, 0.24f);

        private static GUIStyle TableHeaderLabel => _tableHeaderLabel ??= new GUIStyle(EditorStyles.boldLabel)
        {
            normal = { textColor = new Color(0.90f, 0.90f, 0.90f) },
            alignment = TextAnchor.MiddleLeft,
        };

        private static GUIStyle LogRedStyle => _logRed ??= new GUIStyle(EditorStyles.label) { normal = { textColor = new Color(1f, 0.45f, 0.45f) } };
        private static GUIStyle LogYellowStyle => _logYellow ??= new GUIStyle(EditorStyles.label) { normal = { textColor = new Color(1f, 0.85f, 0.40f) } };
        private static GUIStyle LogCyanStyle => _logCyan ??= new GUIStyle(EditorStyles.label) { normal = { textColor = new Color(0.55f, 0.85f, 1f) } };
        private static GUIStyle LogNormalStyle => _logNormal ??= new GUIStyle(EditorStyles.label);
        #endregion

        #region 基类钩子
        /// <summary>数据初始化（在构建 UI 前调用）</summary>
        protected override void OnInitData()
        {
            m_FindFileFullPathList = new Dictionary<string, FileUseState>();
            m_TempResDefItems = new List<ResDefItem>();
            m_AllShowFoldout = new Dictionary<string, TopItemInfo>();
            m_AllDefaultTextures = new List<Texture2D>();
            m_SearchResDefItemInfo = new List<ResDefItem>();
            m_ErrorResDefItemInfo = new List<ResDefItem>();

            Array.ForEach(Enum.GetNames(typeof(GameDefinitions.AssetType)),
                assetType => m_AllShowFoldout[assetType] = new TopItemInfo());
            m_AllShowFoldout.Add(m_SearchTag, new TopItemInfo { OnePageCount = 16, IsShowFoldout = true });
            m_AllShowFoldout.Add(m_ErrorTag, new TopItemInfo { OnePageCount = 16, IsShowFoldout = true });

            ResDefInfos.ConvertJson();
            m_CurMaxResID = ResDefInfos.GetMaxResID();
            LoadABConfigs();
            UpdateResultInfo();
            LoadDefaultTextures();
        }

        /// <summary>UXML 克隆完成后绑定静态控件与 IMGUI 容器</summary>
        protected override void OnBindUI()
        {
            SetupImportSection();
            SetupExportConfigSection();
            SetupDeleteSection();
            SetupSearchSection();
            SetupButtons();
            SetupImguiContainers();
            RequestRepaint();
        }
        #endregion

        #region UI 控件绑定
        /// <summary>绑定"资源导入"区：类型下拉 + 目标对象</summary>
        private void SetupImportSection()
        {
            var enumNames = Enum.GetNames(typeof(GameDefinitions.AssetType));
            var typeDropdown = rootVisualElement.Q<DropdownField>("assetTypeDropdown");
            typeDropdown.choices = new List<string>(enumNames);
            typeDropdown.value = enumNames[m_SelectResType];
            typeDropdown.RegisterValueChangedCallback(evt =>
            {
                m_SelectResType = Array.IndexOf(enumNames, evt.newValue);
                FindTargetAsset = null;
                m_FindFileFullPathList.Clear();
            });

            var objectField = rootVisualElement.Q<ObjectField>("targetAssetField");
            objectField.objectType = typeof(Object);
            objectField.value = m_FindTargetAsset;
            objectField.RegisterValueChangedCallback(evt => { FindTargetAsset = evt.newValue; });
        }

        /// <summary>绑定"导出设置"区：Lua 路径 + 每页数量</summary>
        private void SetupExportConfigSection()
        {
            var luaPathField = rootVisualElement.Q<TextField>("luaPathField");
            luaPathField.value = ResDefInfos.LuaExportFolderPath;
            luaPathField.RegisterValueChangedCallback(evt =>
            {
                if (!evt.newValue.Equals(ResDefInfos.LuaExportFolderPath))
                {
                    ResDefInfos.LuaExportFolderPath = evt.newValue;
                    ResDefInfos.WriteJson();
                }
            });

            rootVisualElement.Q<Button>("defaultLuaBtn").clicked += () =>
            {
                ResDefInfos.LuaExportFolderPath = Runtime.GamePathUtils.Editor.ResDef.LuaFolderPath;
                luaPathField.value = ResDefInfos.LuaExportFolderPath;
                ResDefInfos.WriteJson();
            };

            var oneSheetField = rootVisualElement.Q<IntegerField>("oneSheetField");
            oneSheetField.value = m_OneSheetMaxCount;
            oneSheetField.RegisterValueChangedCallback(evt =>
            {
                m_OneSheetMaxCount = evt.newValue;
                ResDefInfos.WriteJson();
            });
        }

        /// <summary>绑定"删除与校准"区</summary>
        private void SetupDeleteSection()
        {
            var delStart = rootVisualElement.Q<IntegerField>("delStartField");
            var delEnd = rootVisualElement.Q<IntegerField>("delEndField");
            delStart.value = m_InputDelStartID;
            delEnd.value = m_InputDelEndID;
            delStart.RegisterValueChangedCallback(evt => m_InputDelStartID = evt.newValue);
            delEnd.RegisterValueChangedCallback(evt => m_InputDelEndID = evt.newValue);

            rootVisualElement.Q<Button>("deleteBtn").clicked +=
                () => DeleteResInfo(m_InputDelStartID, m_InputDelEndID);
            rootVisualElement.Q<Button>("deleteAllBtn").clicked +=
                () => DeleteResInfo(0, Int32.MaxValue);
            rootVisualElement.Q<Button>("deleteAllInvalidBtn").clicked += DeleteAllInvalidResInfo;
            rootVisualElement.Q<Button>("autoFixBtn").clicked += AutoRefreshAllInvalidResInfo;
        }

        /// <summary>绑定右侧搜索栏</summary>
        private void SetupSearchSection()
        {
            var searchField = rootVisualElement.Q<TextField>("searchField");
            searchField.value = m_SearchResName;
            searchField.RegisterValueChangedCallback(evt =>
            {
                m_SearchResName = evt.newValue;
                if (!string.IsNullOrEmpty(m_SearchResName))
                    UpdateSearchResDefItemInfo(m_SearchResName);
            });

            rootVisualElement.Q<Button>("delSearchBtn").clicked += () =>
            {
                m_SearchResName = string.Empty;
                searchField.value = string.Empty;
            };

            var onlyErrorToggle = rootVisualElement.Q<Toggle>("onlyShowErrorToggle");
            onlyErrorToggle.value = m_OnlyShowError;
            onlyErrorToggle.RegisterValueChangedCallback(evt =>
            {
                m_OnlyShowError = evt.newValue;
                UpdateErrorResDefItemInfo();
                if (m_SearchResName != string.Empty)
                    UpdateSearchResDefItemInfo(m_SearchResName);
            });
        }

        /// <summary>绑定通用动作按钮</summary>
        private void SetupButtons()
        {
            rootVisualElement.Q<Button>("appendBtn").clicked += AppendFoundFilesToBuffer;

            rootVisualElement.Q<Button>("openExcelBtn").clicked += () =>
            {
                string filePath = Runtime.GamePathUtils.AB.GetExcelFileFullPath();
                TableExportEditorUtility.OpenExcel(filePath);
            };

            rootVisualElement.Q<Button>("exportJsonBtn").clicked += () =>
            {
                string filePath = Runtime.GamePathUtils.AB.GetExcelFileFullPath();
                ExportExcelToJsonFromABConfig(Path.GetFileNameWithoutExtension(filePath));
                AssetBundleNamePostprocessor.RefreshAllAssetBundleNames();
                LoadABConfigs();
            };

            rootVisualElement.Q<Button>("openBuildBtn").clicked +=
                () => BuildPackage.BuildPackage_EditorWindow.OpenWindow();

            rootVisualElement.Q<Button>("exportLuaBtn").clicked += () =>
            {
                WriteOutputGDefsFile(GetResultDetailInfo().Values.ToList());
                ShowNotification($"导出 {Runtime.GamePathUtils.Editor.ResDef.GetResDefLuaFullPath()} 完成。");
            };
        }

        /// <summary>把动态 IMGUI 列表挂到 IMGUIContainer</summary>
        private void SetupImguiContainers()
        {
            rootVisualElement.Q<IMGUIContainer>("logContainer").onGUIHandler = DrawLogIMGUIContent;
            rootVisualElement.Q<IMGUIContainer>("bufferContainer").onGUIHandler = DrawBufferIMGUIContent;
            rootVisualElement.Q<IMGUIContainer>("resListContainer").onGUIHandler = DrawResListIMGUIContent;
        }
        #endregion

        #region IMGUI 容器内容（动态列表）
        /// <summary>处理日志内容</summary>
        private void DrawLogIMGUIContent()
        {
            m_TextScrollViewPosition = EditorGUILayout.BeginScrollView(m_TextScrollViewPosition, GUILayout.ExpandHeight(true));
            {
                foreach (KeyValuePair<string, FileUseState> entry in m_FindFileFullPathList)
                {
                    DrawFileStateLine(entry.Key, entry.Value);
                }
            }
            EditorGUILayout.EndScrollView();
        }

        /// <summary>待确认缓冲区内容（含确认添加按钮）</summary>
        private void DrawBufferIMGUIContent()
        {
            var sameAliasName = GetAllSameResInfo();

            CreateTempScrollViewItem("ID", "别名", "AB路径", "资源名字", "资源类型", "删除按钮", null);
            m_LeftScrollViewPosition = EditorGUILayout.BeginScrollView(m_LeftScrollViewPosition, GUILayout.ExpandHeight(true));
            {
                for (int idx = 0; idx < m_TempResDefItems.Count; idx++)
                {
                    var resDefItem = m_TempResDefItems[idx];
                    CreateTempScrollViewItem(Convert.ToString(resDefItem.ID), resDefItem.AliasName,
                        resDefItem.ABPath, resDefItem.AssetName, resDefItem.ResType, string.Empty, resDefItem,
                        sameAliasName.ContainsKey(resDefItem.AliasName));
                }
            }
            EditorGUILayout.EndScrollView();

            using (new EditorGUI.DisabledScope(sameAliasName.Count > 0 || m_TempResDefItems.Count == 0))
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("确认添加", GUILayout.Width(160), GUILayout.Height(28)))
                    {
                        foreach (var resDefItemInfos in m_TempResDefItems)
                        {
                            ResDefInfos.ConvertData.Add(resDefItemInfos);
                            Log.Debug("[Editor] 资源别名: {0} ，添加成功。", resDefItemInfos.AliasName);
                        }

                        ResDefInfos.WriteJson();
                        m_TempResDefItems.Clear();
                        m_FindFileFullPathList.Clear();
                        UpdateResultInfo();
                        ShowNotification("资源添加成功。");
                    }
                    GUILayout.FlexibleSpace();
                }
            }
        }

        /// <summary>资源总表内容（折叠分组 + 分页 + 逐行编辑）</summary>
        private void DrawResListIMGUIContent()
        {
            if (m_ResultDetailInfo == null) return;

            CreateScrollViewItem("ID", "别名", "AB路径", "资源名字", string.Empty, "删除按钮", "矫正路径", null);
            m_RightScrollViewPosition = EditorGUILayout.BeginScrollView(m_RightScrollViewPosition, GUILayout.ExpandHeight(true));
            {
                if (m_SearchResName == string.Empty)
                {
                    if (m_OnlyShowError)
                    {
                        CreateResView(m_ErrorTag, m_ErrorResDefItemInfo);
                    }
                    else
                    {
                        foreach (var keyValue in m_ResultDetailInfo)
                        {
                            CreateResView(keyValue.Key, keyValue.Value);
                        }
                    }
                }
                else
                {
                    CreateResView(m_SearchTag, m_SearchResDefItemInfo);
                }
            }
            if (GUI.changed)
            {
                UpdateResultInfo();
            }
            EditorGUILayout.EndScrollView();
        }
        #endregion

        #region 追加资源逻辑
        /// <summary>
        /// 将当前查找到的文件按"类型_文件名"别名追加到临时缓冲区。
        /// 已存在于配置库的资源标记为"已存在"跳过；别名不冲突的正常追加。
        /// </summary>
        private void AppendFoundFilesToBuffer()
        {
            int successAddCount = 0;
            int failedAddCount = 0;
            int existCount = 0;

            foreach (var fullPath in m_FindFileFullPathList.Keys.ToList())
            {
                var fileName = Path.GetFileNameWithoutExtension(fullPath);
                var aliasName = $"{((GameDefinitions.AssetType)m_SelectResType)}_{fileName}";

                bool existInDb = ResDefInfos.ConvertData.Any(x => x.AliasName == aliasName);
                if (existInDb)
                {
                    m_FindFileFullPathList[fullPath] = FileUseState.ExportFailedExist;
                    existCount++;
                    continue;
                }

                CheckFileABPath(fullPath, out string fileABPath);
                if (!IsHaveSameAliasName(aliasName))
                {
                    var resDefItem = ResDefItem.Create(
                        CurMaxResID,
                        ((GameDefinitions.AssetType)m_SelectResType).ToString(),
                        aliasName, fileABPath, fileName,
                        AssetDatabase.AssetPathToGUID(fullPath));
                    m_TempResDefItems.Add(resDefItem);
                    Log.Debug("[Editor] 资源别名: {0} ，成功追加到缓冲区。", aliasName);
                    m_FindFileFullPathList[fullPath] = FileUseState.ExportSuccess;
                    successAddCount++;
                }
                else
                {
                    Log.Warning("[Editor] 资源别名: {0} ，因别名重名，追加失败。", aliasName);
                    m_FindFileFullPathList[fullPath] = FileUseState.ExportFailedToSameName;
                    failedAddCount++;
                }
            }

            ShowNotification(
                $"共追加数据：{m_FindFileFullPathList.Keys.Count} 条，其中成功导出 {successAddCount} 条，已存在 {existCount} 条，失败导出 {failedAddCount} 条");
            m_FindTargetAsset = null;
        }
        #endregion

        #region IMGUI 行渲染（缓冲区 / 资源总表 / 日志）
        /// <summary>按文件导出状态绘制一行带颜色的日志</summary>
        private static void DrawFileStateLine(string fileFullPath, FileUseState state)
        {
            string normalizedPath = fileFullPath.Replace("\\", "/");
            switch (state)
            {
                case FileUseState.LoadSuccess:
                    EditorGUILayout.LabelField(normalizedPath + " 加载成功", LogNormalStyle);
                    break;
                case FileUseState.ExportSuccess:
                    EditorGUILayout.LabelField(normalizedPath + " 导出成功", LogNormalStyle);
                    break;
                case FileUseState.ExportFailedToSameName:
                    EditorGUILayout.LabelField(normalizedPath + " 导出失败", LogRedStyle);
                    break;
                case FileUseState.LoadFailed:
                    EditorGUILayout.LabelField(normalizedPath + " 无AB映射", LogYellowStyle);
                    break;
                case FileUseState.ExportFailedExist:
                    EditorGUILayout.LabelField(normalizedPath + " 已存在", LogCyanStyle);
                    break;
            }
        }

        /// <summary>创建缓冲区列表项</summary>
        public void CreateTempScrollViewItem(string id, string alias, string abPath, string fileName, string fileType,
            string delString, ResDefItem resDefItem, bool isSameName = false)
        {
            bool isHeader = resDefItem == null;
            var rowStyle = isHeader ? TableHeaderLabel : EditorStyles.label;

            var optionsID = new[] { GUILayout.Width(60), GUILayout.Height(20) };
            var optionsAlias = new[] { GUILayout.Width(160), GUILayout.Height(20) };
            var optionsAbPath = new[] { GUILayout.Width(380), GUILayout.Height(20) };
            var optionsFileName = new[] { GUILayout.Width(110), GUILayout.Height(20) };
            var optionsFileType = new[] { GUILayout.Width(80), GUILayout.Height(20) };
            var optionsDelBtn = new[] { GUILayout.Width(50), GUILayout.Height(20) };

            EditorGUILayout.BeginVertical(isHeader ? EditorStyles.helpBox : "box");
            {
                EditorGUILayout.BeginHorizontal(GUILayout.Height(20));
                {
                    EditorGUILayout.LabelField(id, rowStyle, optionsID);
                    if (delString != string.Empty)
                    {
                        EditorGUILayout.LabelField(alias, rowStyle, optionsAlias);
                        EditorGUILayout.LabelField(abPath, rowStyle, optionsAbPath);
                    }
                    else
                    {
                        string safeAbPath = abPath ?? string.Empty;
                        string safeAlias = resDefItem == null ? string.Empty : resDefItem.AliasName;

                        if (resDefItem == null)
                        {
                            EditorGUILayout.LabelField(safeAlias, rowStyle, optionsAlias);
                            EditorGUILayout.LabelField(safeAbPath, rowStyle, optionsAbPath);
                        }
                        else
                        {
                            resDefItem.AliasName = EditorGUILayout.TextField(safeAlias, optionsAlias);
                            string tooltip = string.IsNullOrEmpty(safeAbPath)
                                ? string.Empty
                                : safeAbPath.Substring(0, Math.Min(safeAbPath.Length, 80));
                            EditorGUILayout.LabelField(new GUIContent(tooltip, safeAbPath), optionsAbPath);
                        }
                    }

                    EditorGUILayout.LabelField(fileName, rowStyle, optionsFileName);
                    EditorGUILayout.LabelField(fileType, rowStyle, optionsFileType);
                    if (delString == string.Empty)
                    {
                        GUI.backgroundColor = ButtonRed;
                        if (GUILayout.Button("删除", optionsDelBtn))
                            m_TempResDefItems.Remove(resDefItem);
                        GUI.backgroundColor = Color.white;
                    }
                    else
                    {
                        EditorGUILayout.LabelField(delString, rowStyle, optionsDelBtn);
                    }
                }
                EditorGUILayout.EndHorizontal();

                if (isSameName && resDefItem != null)
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField($"别名：{resDefItem.AliasName} 重名，请手动修改。", LogRedStyle);
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }
            EditorGUILayout.EndVertical();
        }

        /// <summary>按类型分组绘制资源列表</summary>
        public void CreateResView(string titleName, List<ResDefItem> resDefItems)
        {
            var tempStyle = new GUIStyle(EditorStyles.foldout);
            bool hasError = m_OnlyShowError
                            || m_ResultInvalidPath[titleName].Count > 0
                            || m_ResultSameResInfo.ContainsKey(titleName);
            tempStyle.normal.textColor = hasError ? new Color(1f, 0.45f, 0.45f) : Color.white;
            tempStyle.onNormal.textColor = tempStyle.normal.textColor;

            UpdateFoldoutTitle(titleName, resDefItems.Count);
            if (m_AllShowFoldout[titleName].IsShowFoldout || titleName == m_SearchTag)
            {
                int startIndex = 0;
                int endIndex = resDefItems.Count;
                if (!m_AllShowFoldout[titleName].IsShowAll)
                {
                    startIndex = (m_AllShowFoldout[titleName].CurPageIndex - 1) *
                                 m_AllShowFoldout[titleName].OnePageCount;
                    endIndex = Math.Min(endIndex, startIndex + m_AllShowFoldout[titleName].OnePageCount);
                }

                for (int idx = startIndex; idx < endIndex; idx++)
                {
                    var resDefItem = resDefItems[idx];
                    CreateScrollViewItem(Convert.ToString(resDefItem.ID), resDefItem.AliasName, resDefItem.ABPath,
                        resDefItem.AssetName, resDefItem.ResType, string.Empty, string.Empty, resDefItem,
                        m_ResultInvalidPath[titleName].ContainsKey(resDefItem.ID),
                        m_ResultSameResInfo.ContainsKey(resDefItem.AliasName));
                }
            }
        }

        /// <summary>创建资源列表项</summary>
        public void CreateScrollViewItem(string id, string alias, string abPath, string fileName, string fileType,
            string delString, string resetPath, ResDefItem resDefItem, bool invalidPath = false,
            bool isSameName = false)
        {
            bool isHeader = resDefItem == null;
            var rowStyle = isHeader ? TableHeaderLabel : EditorStyles.label;

            var optionsID = new[] { GUILayout.Width(50), GUILayout.Height(20) };
            var optionsAlias = new[] { GUILayout.Width(130), GUILayout.Height(20) };
            var optionsFileName = new[] { GUILayout.Width(100), GUILayout.Height(20) };
            var optionsDelBtn = new[] { GUILayout.Width(60), GUILayout.Height(20) };
            var optionsResetBtn = new[] { GUILayout.Width(70), GUILayout.Height(20) };

            string redTextString = invalidPath
                ? "文件不存在或abPath路径已失效,请点击矫正按钮进行矫正"
                : isSameName && resDefItem != null
                    ? $"别名：{resDefItem.AliasName} 重名，请手动修改。"
                    : string.Empty;

            EditorGUILayout.BeginVertical(isHeader ? EditorStyles.helpBox : "box");
            {
                EditorGUILayout.BeginHorizontal(GUILayout.Height(20));
                {
                    EditorGUILayout.LabelField(id, rowStyle, optionsID);
                    if (delString == string.Empty)
                    {
                        var newAlias = EditorGUILayout.TextField(alias, optionsAlias);
                        if (GUI.changed && newAlias.Length > 0)
                        {
                            resDefItem.AliasName = newAlias;
                            ResDefInfos.WriteJson();
                        }

                        EditorGUILayout.LabelField(new GUIContent(fileName, "AB路径 = " + abPath), optionsFileName);

                        DrawRectifyButton(optionsResetBtn, abPath, fileType, resDefItem);

                        if (invalidPath)
                        {
                            DrawAutoRectifyButton(optionsResetBtn, fileName, fileType, abPath, resDefItem);
                        }

                        GUI.backgroundColor = ButtonRed;
                        if (GUILayout.Button("删除", optionsDelBtn))
                        {
                            ResDefInfos.ConvertData.Remove(resDefItem);
                            ResDefInfos.WriteJson();
                            UpdateResultInfo();
                            GUIUtility.ExitGUI();
                        }
                        GUI.backgroundColor = Color.white;
                    }
                    else
                    {
                        EditorGUILayout.LabelField(alias, rowStyle, optionsAlias);
                        EditorGUILayout.LabelField(fileName, rowStyle, optionsFileName);
                        EditorGUILayout.LabelField(resetPath, rowStyle, optionsResetBtn);
                        EditorGUILayout.LabelField(delString, rowStyle, optionsDelBtn);
                    }
                }
                EditorGUILayout.EndHorizontal();

                if (!string.IsNullOrEmpty(redTextString))
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField(redTextString, LogRedStyle);
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }
            EditorGUILayout.EndVertical();
        }

        /// <summary>绘制"矫正路径"按钮</summary>
        private void DrawRectifyButton(GUILayoutOption[] options, string abPath, string fileType, ResDefItem resDefItem)
        {
            GUI.backgroundColor = ButtonBlue;
            if (GUILayout.Button("矫正路径", options))
            {
                GUI.backgroundColor = Color.white;
                string suffix = GetAssetsSuffixByType(fileType).Replace(".", "");
                string fillFullPath = EditorUtility.OpenFilePanel("寻找文件", GetAbPathFullDirPath(abPath), suffix);
                if (fillFullPath.Length > 0)
                {
                    string fullPath = fillFullPath.Substring(Application.dataPath.Length - 6);
                    if (CheckFileABPath(fullPath, out string newABPath))
                    {
                        resDefItem.AssetName = Path.GetFileNameWithoutExtension(fullPath);
                        resDefItem.ABPath = newABPath;
                        ResDefInfos.WriteJson();
                        ShowNotification("信息矫正成功，文件名字，AB路径都已刷新");
                        UpdateResultInfo();
                    }
                    else
                    {
                        ShowNotification("当前文件没有找到AB配置信息，请检查");
                    }

                    GUIUtility.ExitGUI();
                }
            }
            GUI.backgroundColor = Color.white;
        }

        /// <summary>绘制"Auto矫正"按钮</summary>
        private void DrawAutoRectifyButton(GUILayoutOption[] options, string fileName, string fileType, string abPath, ResDefItem resDefItem)
        {
            GUI.backgroundColor = ButtonBlue;
            if (GUILayout.Button("Auto矫正", options))
            {
                GUI.backgroundColor = Color.white;
                if (AutoRectifyInvalidResInfo(fileName, fileType, abPath, resDefItem, true))
                {
                    ShowNotification("信息矫正成功，文件名字，AB路径都已刷新");
                    UpdateResultInfo();
                }
                else
                {
                    ShowNotification("当前文件没有找到AB配置信息，请检查");
                }

                GUIUtility.ExitGUI();
            }
            GUI.backgroundColor = Color.white;
        }

        /// <summary>更新折叠栏标题（含分页控件）</summary>
        public void UpdateFoldoutTitle(string key, int count)
        {
            var labelOptions = new[] { GUILayout.Width(28) };
            var buttonOptions = new[] { GUILayout.Width(20), GUILayout.Height(20) };
            var disableLabelOptions = new[] { GUILayout.Width(50), GUILayout.Height(20) };

            EditorGUILayout.BeginHorizontal("box", GUILayout.Height(20));
            {
                if (key == m_SearchTag || key == m_ErrorTag)
                {
                    EditorGUILayout.LabelField(key, TableHeaderLabel);
                }
                else
                {
                    m_AllShowFoldout[key].IsShowFoldout =
                        EditorGUILayout.Foldout(m_AllShowFoldout[key].IsShowFoldout, key);
                }

                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.LabelField($"{count} items ", disableLabelOptions);
                EditorGUI.EndDisabledGroup();

                if (m_AllShowFoldout[key].PageCount > 1)
                {
                    EditorGUI.BeginDisabledGroup(!m_AllShowFoldout[key].IsShowFoldout);
                    {
                        EditorGUI.BeginDisabledGroup(m_AllShowFoldout[key].IsShowAll);
                        {
                            EditorGUI.BeginDisabledGroup(m_AllShowFoldout[key].CurPageIndex == 1);
                            if (GUILayout.Button(m_AllDefaultTextures[0], buttonOptions))
                                m_AllShowFoldout[key].CurPageIndex--;
                            EditorGUI.EndDisabledGroup();

                            int tempIndex = EditorGUILayout.IntField(m_AllShowFoldout[key].CurPageIndex, labelOptions);
                            if (m_AllShowFoldout[key].CurPageIndex != tempIndex && tempIndex > 0 &&
                                tempIndex <= m_AllShowFoldout[key].PageCount)
                                m_AllShowFoldout[key].CurPageIndex = tempIndex;

                            EditorGUILayout.LabelField(" / " + m_AllShowFoldout[key].PageCount, labelOptions);

                            EditorGUI.BeginDisabledGroup(m_AllShowFoldout[key].CurPageIndex ==
                                                         m_AllShowFoldout[key].PageCount);
                            if (GUILayout.Button(m_AllDefaultTextures[1], buttonOptions))
                                m_AllShowFoldout[key].CurPageIndex++;
                            EditorGUI.EndDisabledGroup();
                        }
                        EditorGUI.EndDisabledGroup();

                        if (GUILayout.Button(
                                m_AllShowFoldout[key].IsShowAll ? m_AllDefaultTextures[2] : m_AllDefaultTextures[3],
                                buttonOptions))
                            m_AllShowFoldout[key].IsShowAll = !m_AllShowFoldout[key].IsShowAll;
                    }
                    EditorGUI.EndDisabledGroup();
                }
            }
            EditorGUILayout.EndHorizontal();
        }
        #endregion
    }
}
