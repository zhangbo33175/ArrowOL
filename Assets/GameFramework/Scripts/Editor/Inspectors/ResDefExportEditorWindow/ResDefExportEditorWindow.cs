using System;
using System.Collections.Generic;
using System.Linq;
using Honor.Runtime;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;
using Path = System.IO.Path;

namespace Honor.Editor
{
    public partial class ResDefExportEditorWindow : BaseEditorWindow<ResDefExportEditorWindow>
    {
        /// <summary>
        /// 启用
        /// </summary>
        protected void OnEnable()
        {
            m_FindFileFullPathList = new Dictionary<string, FileUseState>();
            m_TempResDefItems = new List<ResDefItem>();
            m_AllShowFoldout = new Dictionary<string, TopItemInfo>();
            m_AllDefaultTextures = new List<Texture2D>();
            m_SearchResDefItemInfo = new List<ResDefItem>();
            m_ErrorResDefItemInfo = new List<ResDefItem>();
            Array.ForEach(Enum.GetNames(typeof(GameDefinitions.AssetType)),
                (assetType) => m_AllShowFoldout[assetType] = new TopItemInfo());
            m_AllShowFoldout.Add(m_SearchTag, new TopItemInfo() { OnePageCount = 16, IsShowFoldout = true });
            m_AllShowFoldout.Add(m_ErrorTag, new TopItemInfo() { OnePageCount = 16, IsShowFoldout = true });

            ResDefInfos.ConvertJson();
            m_CurMaxResID = ResDefInfos.GetMaxResID();
            LoadABConfigs();
            UpdateResultInfo();
            LoadDefaultTextures();
        }

        protected override void OnGUI()
        {
            base.OnGUI();

            if (EditorApplication.isPlaying || EditorApplication.isCompiling)
            {
                EditorGUILayout.LabelField("Updating......");
                Repaint();
                return;
            }

            GUILayout.BeginHorizontal();
            {
                // 左侧Layout
                GUILayout.BeginVertical(new[]
                    { GUILayout.Width(MinSize.x * m_SubPercenttage), GUILayout.Height(MinSize.y) });
                {
                    // 左上方查找模块
                    FindingView();
                    // 左上方配置模块
                    ExportConfigView();
                    // 左上方删除模块
                    DeletingView();
                    // 文本结果显示区域
                    ResultTextView();
                    // 拖动区域
                    DragFilesView();
                    // 显示临时缓冲区域
                    TempFilesView();
                }
                GUILayout.EndVertical();

                // 右侧Layout
                GUILayout.BeginVertical(new[]
                    { GUILayout.Width(MinSize.x * (1 - m_SubPercenttage)), GUILayout.Height(MinSize.y) });
                {
                    // 右侧的资源详情界面
                    AllResDetailsView();
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();

            Repaint();
        }

        /// <summary>
        /// 查找模块View
        /// </summary>
        public void FindingView()
        {
            EditorGUILayout.Space(5);
            GUILayout.BeginHorizontal("box",
                new[] { GUILayout.Width(MinSize.x * m_SubPercenttage * 0.8f), GUILayout.Height(20) });
            {
                EditorGUILayout.LabelField("导入资源类型：", new[] { GUILayout.Width(100), GUILayout.Height(20) });

                // 数据的类型，选择类型有变化，则更新显示信息
                Array array = Enum.GetValues(typeof(GameDefinitions.AssetType));
                int[] assetTypeValues = new int[array.Length];
                for (int index = 0; index < array.Length; index++) assetTypeValues[index] = (int)array.GetValue(index);
                int selectResType = EditorGUILayout.IntPopup((int)m_SelectResType,
                    Enum.GetNames(typeof(GameDefinitions.AssetType)), assetTypeValues,
                    new[] { GUILayout.Width(150), GUILayout.Height(20) });
                if (selectResType != m_SelectResType)
                {
                    m_SelectResType = selectResType;
                    FindTargetAsset = null;
                    m_FindFileFullPathList.Clear();
                }

                // 查找到的数据资源
                FindTargetAsset = EditorGUILayout.ObjectField(FindTargetAsset, typeof(Object), false);

                if (GUILayout.Button("追加", new[] { GUILayout.Width(90), GUILayout.Height(20) }))
                {
                    // 追加成功的数据条目数
                    int successAddCount = 0;
                    // 因同名追加失败的数据条目数
                    int failedAddCount = 0;

                    foreach (var fullPath in m_FindFileFullPathList.Keys.ToList())
                    {
                        var fileName = Path.GetFileNameWithoutExtension(fullPath);
                        var aliasName = AorTxt.Format("{0}_{1}",
                            ((GameDefinitions.AssetType)m_SelectResType).ToString(), fileName);
                        CheckFileABPath(fullPath, out string fileABPath);
                        if (IsHaveSameAliasName(aliasName) == false)
                        {
                            var resDefItem = new ResDefItem();
                            resDefItem.ID = CurMaxResID;
                            resDefItem.ResType = ((GameDefinitions.AssetType)m_SelectResType).ToString();
                            resDefItem.AliasName = aliasName;
                            resDefItem.ABPath = fileABPath;
                            resDefItem.AssetName = fileName;
                            resDefItem.AssetGUID = AssetDatabase.AssetPathToGUID(fullPath);
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
                        $"共追加数据：{m_FindFileFullPathList.Keys.Count} 条，其中成功导出 {successAddCount} 条，失败导出 {failedAddCount} 条");
                    m_FindTargetAsset = null;
                }
            }
            GUILayout.EndHorizontal();
        }

        /// <summary>
        /// 配置模块View
        /// </summary>
        public void ExportConfigView()
        {
            GUILayout.BeginHorizontal("box",
                new[] { GUILayout.Width(MinSize.x * m_SubPercenttage * 0.8f), GUILayout.Height(20) });
            {
                EditorGUILayout.LabelField("Lua导出目录路径：", new[] { GUILayout.Width(100), GUILayout.Height(20) });
                string tmpPath = EditorGUILayout.TextField(ResDefInfos.LuaExportFolderPath);
                if (GUILayout.Button("默认", new[] { GUILayout.Width(90), GUILayout.Height(20) }))
                {
                    tmpPath = Runtime.GamePathUtils.Editor.ResDef.LuaFolderPath;
                }

                if (!tmpPath.Equals(ResDefInfos.LuaExportFolderPath))
                {
                    ResDefInfos.LuaExportFolderPath = tmpPath;
                    ResDefInfos.WriteJson();
                }

                EditorGUILayout.Space(10);
                EditorGUILayout.LabelField("每页导出 LUA 数据量：", new[] { GUILayout.Width(140), GUILayout.Height(20) });
                var tempOneSheetMaxCount = EditorGUILayout.IntField(m_OneSheetMaxCount,
                    new[] { GUILayout.Width(60), GUILayout.Height(20) });
                if (tempOneSheetMaxCount != m_OneSheetMaxCount)
                {
                    m_OneSheetMaxCount = tempOneSheetMaxCount;
                    ResDefInfos.WriteJson();
                }
            }
            GUILayout.EndHorizontal();
        }

        /// <summary>
        /// 删除模块
        /// </summary>
        public void DeletingView()
        {
            var options = new[] { GUILayout.Width(60), GUILayout.Height(20) };
            GUILayout.BeginHorizontal(new GUIStyle("box"),
                new[] { GUILayout.Width(MinSize.x * m_SubPercenttage * 0.8f), GUILayout.Height(20) });
            {
                EditorGUILayout.LabelField("删除:", new[] { GUILayout.Width(100), GUILayout.Height(20) });
                GUILayout.FlexibleSpace();
                m_InputDelStartID = EditorGUILayout.IntField(m_InputDelStartID, options);
                EditorGUILayout.LabelField(" ~ ", new[] { GUILayout.Width(20), GUILayout.Height(20) });
                m_InputDelEndID = EditorGUILayout.IntField(m_InputDelEndID, options);
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("删除", new[] { GUILayout.Width(90), GUILayout.Height(20) }))
                {
                    DeleteResInfo(m_InputDelStartID, m_InputDelEndID);
                }

                if (GUILayout.Button("删除所有", new[] { GUILayout.Width(90), GUILayout.Height(20) }))
                {
                    DeleteResInfo(0, Int32.MaxValue);
                }

                if (GUILayout.Button("删除所有失效", new[] { GUILayout.Width(90), GUILayout.Height(20) }))
                {
                    DeleteAllInvalidResInfo();
                }

                if (GUILayout.Button("自动校准失效", new[] { GUILayout.Width(90), GUILayout.Height(20) }))
                {
                    AutoRefreshAllInvalidResInfo();
                }

                GUILayout.Space(80);

                if (GUILayout.Button("打开AB配置表Excel", new[] { GUILayout.Width(130), GUILayout.Height(20) }))
                {
                    string filePath = Runtime.GamePathUtils.AB.GetExcelFileFullPath();
                    TableExportEditorUtility.OpenExcel(filePath);
                }

                if (GUILayout.Button("导出AB配置表Excel到Json&刷新AB名称", new[] { GUILayout.Width(240), GUILayout.Height(20) }))
                {
                    string filePath = Runtime.GamePathUtils.AB.GetExcelFileFullPath();
                    ExportExcelToJsonFromABConfig(System.IO.Path.GetFileNameWithoutExtension(filePath));
                    AssetBundleNamePostprocessor.RefreshAllAssetBundleNames();
                }
            }
            GUILayout.EndHorizontal();
        }

        /// <summary>
        /// 文本显示区域View
        /// </summary>
        public void ResultTextView()
        {
            EditorGUILayout.Space(10);
            GUILayout.BeginVertical("box",
                new[] { GUILayout.Width(MinSize.x * m_SubPercenttage), GUILayout.Height(100) });
            {
                m_TextScrollViewPosition = GUILayout.BeginScrollView(m_TextScrollViewPosition, false, true);
                {
                    foreach (var keyValuePairs in m_FindFileFullPathList)
                    {
                        var fileFullPath = keyValuePairs.Key;
                        if (keyValuePairs.Value == FileUseState.LoadSuccess)
                        {
                            EditorGUILayout.LabelField(fileFullPath.Replace("\\", "/") + " 加载成功");
                        }
                        else if (keyValuePairs.Value == FileUseState.ExportSuccess)
                        {
                            EditorGUILayout.LabelField(fileFullPath.Replace("\\", "/") + " 导出成功");
                        }
                        else if (keyValuePairs.Value == FileUseState.ExportFailedToSameName)
                        {
                            EditorGUILayout.LabelField(fileFullPath.Replace("\\", "/") + " 导出失败",
                                new GUIStyle() { normal = new GUIStyleState() { textColor = Color.red } });
                        }
                    }
                }
                GUILayout.EndScrollView();
                GUILayout.FlexibleSpace();
            }
            GUILayout.EndVertical();
        }

        /// <summary>
        /// 拖动区域
        /// </summary>
        public void DragFilesView()
        {
            Event evt = Event.current;
            Rect dropArea = GUILayoutUtility.GetLastRect();
            switch (evt.type)
            {
                case EventType.DragUpdated:
                case EventType.DragPerform:
                    if (!dropArea.Contains(evt.mousePosition))
                        return;

                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                    if (evt.type == EventType.DragPerform)
                    {
                        DragAndDrop.AcceptDrag();
                        UpdateAllSelectedFileData(DragAndDrop.objectReferences);
                    }

                    break;
            }
        }

        /// <summary>
        /// 临时缓冲区域View
        /// </summary>
        public void TempFilesView()
        {
            EditorGUILayout.Space(30);
            EditorGUILayout.LabelField("缓冲区:");
            // 重名的资源ID
            var sameAliasName = GetAllSameResInfo();
            // ScrollView的初始化
            GUILayout.BeginVertical("box",
                new[] { GUILayout.Width(MinSize.x * m_SubPercenttage), GUILayout.Height(530) });
            {
                CreateTempScrollViewItem("ID", "别名", "AB路径", "资源名字", "资源类型", "删除按钮", null);
                // 设置scrollView的移动模式
                m_LeftScrollViewPosition = GUILayout.BeginScrollView(m_LeftScrollViewPosition, false, false,
                    new[] { GUILayout.Width(MinSize.x * m_SubPercenttage), GUILayout.Height(530) });
                {
                    for (int idx = 0; idx < m_TempResDefItems.Count; idx++)
                    {
                        var resDefItem = m_TempResDefItems[idx];
                        CreateTempScrollViewItem(Convert.ToString(resDefItem.ID), resDefItem.AliasName,
                            resDefItem.ABPath, resDefItem.AssetName, resDefItem.ResType, string.Empty, resDefItem,
                            sameAliasName.ContainsKey(resDefItem.AliasName));
                    }
                }
                GUILayout.EndScrollView();
                GUILayout.FlexibleSpace();
            }
            GUILayout.EndVertical();

            // 居中对齐
            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.FlexibleSpace();
                if (sameAliasName.Count > 0 || m_TempResDefItems.Count == 0)
                {
                    EditorGUI.BeginDisabledGroup(true);
                }

                if (GUILayout.Button("确认添加", new[] { GUILayout.Width(80), GUILayout.Height(30) }))
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

                EditorGUI.EndDisabledGroup();
                GUILayout.FlexibleSpace();
            }
            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// 创建临时缓冲区ScrollView的item
        /// </summary>
        public void CreateTempScrollViewItem(string id, string alias, string abPath, string fileName, string fileType,
            string delString, ResDefItem resDefItem, bool isSameName = false)
        {
            var layoutOptions = new[] { GUILayout.Width(MinSize.x * m_SubPercenttage - 22), GUILayout.Height(20) };
            var optionsID = new[] { GUILayout.Width(80), GUILayout.Height(20) };
            var optionsAlias = new[] { GUILayout.Width(220), GUILayout.Height(20) };
            var optionsAbPath = new[] { GUILayout.Width(450), GUILayout.Height(20) };
            var optionsFileName = new[] { GUILayout.Width(140), GUILayout.Height(20) };
            var optionsFileType = new[] { GUILayout.Width(100), GUILayout.Height(20) };
            var optionsDelBtn = new[] { GUILayout.Width(50), GUILayout.Height(20) };

            if (isSameName) GUI.backgroundColor = Color.red;

            EditorGUILayout.BeginVertical("box");
            {
                GUI.backgroundColor = Color.white;
                EditorGUILayout.BeginHorizontal(layoutOptions);
                {
                    EditorGUILayout.LabelField(id, optionsID);
                    if (delString != String.Empty)
                    {
                        EditorGUILayout.LabelField(alias, optionsAlias);
                        EditorGUILayout.LabelField(abPath, optionsAbPath);
                    }
                    else
                    {
                        resDefItem.AliasName = EditorGUILayout.TextField(resDefItem.AliasName, optionsAlias);
                        EditorGUILayout.LabelField(
                            new GUIContent(abPath.Substring(0, Math.Min(abPath.Length, 80)), abPath), optionsAbPath);
                    }

                    EditorGUILayout.LabelField(fileName, optionsFileName);
                    EditorGUILayout.LabelField(fileType, optionsFileType);
                    if (delString == String.Empty)
                    {
                        if (GUILayout.Button("删除", optionsDelBtn))
                            m_TempResDefItems.Remove(resDefItem);
                    }
                    else
                    {
                        EditorGUILayout.LabelField(delString, optionsDelBtn);
                    }
                }
                EditorGUILayout.EndHorizontal();

                if (isSameName)
                {
                    EditorGUILayout.BeginHorizontal(layoutOptions);
                    {
                        EditorGUILayout.LabelField("别名：" + resDefItem.AliasName + " 重名，请手动修改。",
                            new GUIStyle() { normal = { textColor = Color.red } }, layoutOptions);
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }
            EditorGUILayout.EndVertical();
        }

        /// <summary>
        /// 所有资源详情界面
        /// </summary>
        public void AllResDetailsView()
        {
            if (m_ResultDetailInfo == null)
            {
                return;
            }

            // ScrollView的初始化
            GUILayout.BeginVertical("box",
                new[] { GUILayout.Width(MinSize.x * (1 - m_SubPercenttage)), GUILayout.Height(MinSize.y - 50) });
            {
                CreateSearchView();
                CreateScrollViewItem("ID", "别名", "AB路径", "资源名字", string.Empty, "删除按钮", "矫正路径", null);
                // 设置scrollView的移动模式
                m_RightScrollViewPosition = GUILayout.BeginScrollView(m_RightScrollViewPosition, false, false,
                    new[]
                    {
                        GUILayout.Width(MinSize.x * (1 - m_SubPercenttage) - 25), GUILayout.Height(MinSize.y - 120)
                    });
                {
                    if (m_SearchResName == String.Empty)
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

                GUILayout.EndScrollView();
            }
            GUILayout.EndVertical();

            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.FlexibleSpace();
                m_ResultInvalidPath.Values.ToList().ForEach(m =>
                {
                    if (m.Count > 0)
                    {
                        EditorGUI.BeginDisabledGroup(true);
                    }
                });
                if (m_ResultSameResInfo.Count > 0)
                {
                    EditorGUI.BeginDisabledGroup(true);
                }

                if (GUILayout.Button("导出LUA", new[] { GUILayout.Width(80), GUILayout.Height(30) }))
                {
                    WriteOutputGDefsFile(GetResultDetailInfo().Values.ToList());
                    ShowNotification($"导出 {Runtime.GamePathUtils.Editor.ResDef.GetResDefLuaFullPath()} 完成。");
                }

                EditorGUI.EndDisabledGroup();
                GUILayout.FlexibleSpace();
            }
            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// 创建Res的显示View
        /// </summary>
        public void CreateResView(string titleName, List<ResDefItem> resDefItems)
        {
            var tempStyle = EditorStyles.foldout;
            var redColor = m_OnlyShowError ||
                           (m_ResultInvalidPath[titleName].Count > 0 || m_ResultSameResInfo.ContainsKey(titleName));
            tempStyle.normal.textColor = redColor ? Color.red : Color.white;
            tempStyle.onNormal.textColor = tempStyle.normal.textColor;

            UpdateFoldoutTitle(titleName, resDefItems.Count);
            if (m_AllShowFoldout[titleName].IsShowFoldout || titleName == m_SearchTag)
            {
                int startIndex = 0;
                int endIndex = resDefItems.Count;
                if (m_AllShowFoldout[titleName].IsShowAll == false)
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

            tempStyle.normal.textColor = Color.white;
            tempStyle.onNormal.textColor = Color.white;
        }

        /// <summary>
        /// 搜索模块
        /// </summary>
        public void CreateSearchView()
        {
            GUILayout.Space(5);
            EditorGUILayout.BeginHorizontal("box");
            {
                // 搜索框
                var tempSearchExcelName = EditorGUILayout.TextField("搜索资源名字：", m_SearchResName, GUILayout.Width(300));
                if (tempSearchExcelName != m_SearchResName)
                {
                    m_SearchResName = tempSearchExcelName;
                    if (m_SearchResName != string.Empty)
                    {
                        UpdateSearchResDefItemInfo(m_SearchResName);
                    }
                }

                // 删除搜索框里面的内容
                if (GUILayout.Button("Del", GUILayout.Width(30)))
                {
                    m_SearchResName = string.Empty;
                    GUIUtility.keyboardControl = 0;
                }

                GUILayout.FlexibleSpace();
                var tempOnlyShowError = GUILayout.Toggle(m_OnlyShowError, "是否仅显示错误信息     ");
                if (tempOnlyShowError != m_OnlyShowError)
                {
                    m_OnlyShowError = tempOnlyShowError;
                    UpdateErrorResDefItemInfo();
                    if (m_SearchResName != String.Empty)
                    {
                        UpdateSearchResDefItemInfo(m_SearchResName);
                    }
                }
            }
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(5);
        }

        /// <summary>
        /// 创建主仓库ScrollView的item
        /// </summary>
        public void CreateScrollViewItem(string id, string alias, string abPath, string fileName, string fileType,
            string delString, string resetPath, ResDefItem resDefItem, bool invalidPath = false,
            bool isSameName = false)
        {
            var layoutOptions = new[]
                { GUILayout.Width(MinSize.x * (1 - m_SubPercenttage) - 55), GUILayout.Height(20) };
            var optionsID = new[] { GUILayout.Width(80), GUILayout.Height(20) };
            var optionsAlias = new[] { GUILayout.Width(190), GUILayout.Height(20) };
            var optionsFileName = new[] { GUILayout.Width(120), GUILayout.Height(20) };
            var optionsDelBtn = new[] { GUILayout.Width(50), GUILayout.Height(20) };
            var optionsResetBtn = new[] { GUILayout.Width(70), GUILayout.Height(20) };
            var redTextString = string.Empty;

            if (invalidPath || isSameName)
            {
                GUI.backgroundColor = Color.red;
                redTextString = invalidPath ? "文件不存在或abPath路径已失效,请点击矫正按钮进行矫正" : $"别名：{resDefItem.AliasName} 重名，请手动修改。";
            }

            EditorGUILayout.BeginVertical("box");
            {
                GUI.backgroundColor = Color.white;
                EditorGUILayout.BeginHorizontal("box", layoutOptions);
                {
                    EditorGUILayout.LabelField(id, optionsID);
                    if (delString == String.Empty)
                    {
                        var newAlias = EditorGUILayout.TextField(alias, optionsAlias);
                        if (GUI.changed && newAlias.Length > 0)
                        {
                            resDefItem.AliasName = newAlias;
                            ResDefInfos.WriteJson();
                        }

                        EditorGUILayout.LabelField(new GUIContent(fileName, "AB路径 = " + abPath), optionsFileName);

                        if (GUILayout.Button("矫正路径", optionsResetBtn))
                        {
                            var fillFullPath = EditorUtility.OpenFilePanel("寻找文件", GetAbPathFullDirPath(abPath),
                                GetAssetsSuffixByType(fileType).Replace(".", ""));
                            if (fillFullPath.Length > 0)
                            {
                                var fullPath = fillFullPath.Substring(Application.dataPath.Length - 6);
                                if (CheckFileABPath(fullPath, out var newABPath))
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

                        if (invalidPath && GUILayout.Button("Auto矫正", optionsResetBtn))
                        {
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

                        if (GUILayout.Button("删除", optionsDelBtn))
                        {
                            ResDefInfos.ConvertData.Remove(resDefItem);
                            ResDefInfos.WriteJson();
                            UpdateResultInfo();
                            GUIUtility.ExitGUI();
                        }
                    }
                    else
                    {
                        EditorGUILayout.LabelField(alias, optionsAlias);
                        EditorGUILayout.LabelField(fileName, optionsFileName);
                        EditorGUILayout.LabelField(resetPath, optionsResetBtn);
                        EditorGUILayout.LabelField(delString, optionsDelBtn);
                    }
                }
                EditorGUILayout.EndHorizontal();

                if (invalidPath || isSameName)
                {
                    EditorGUILayout.BeginHorizontal(layoutOptions);
                    {
                        EditorGUILayout.LabelField(redTextString, new GUIStyle() { normal = { textColor = Color.red } },
                            layoutOptions);
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }
            EditorGUILayout.EndVertical();
        }

        /// <summary>
        /// 更新title数据
        /// </summary>
        public void UpdateFoldoutTitle(string key, int count)
        {
            var layoutOptions = new[]
                { GUILayout.Width(MinSize.x * (1 - m_SubPercenttage) - 55), GUILayout.Height(20) };
            var labelOptions = new[] { GUILayout.Width(28) };
            var buttonOptions = new[] { GUILayout.Width(20), GUILayout.Height(20) };
            var disableLabelOptions = new[] { GUILayout.Width(50), GUILayout.Height(20) };

            EditorGUILayout.BeginHorizontal("box", layoutOptions);
            {
                if (key == m_SearchTag || key == m_ErrorTag)
                    EditorGUILayout.LabelField(key);
                else
                    m_AllShowFoldout[key].IsShowFoldout =
                        EditorGUILayout.Foldout(m_AllShowFoldout[key].IsShowFoldout, key);

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

                            var tempIndex = EditorGUILayout.IntField(m_AllShowFoldout[key].CurPageIndex, labelOptions);
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
    }
}