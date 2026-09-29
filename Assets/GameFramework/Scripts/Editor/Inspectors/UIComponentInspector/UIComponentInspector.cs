/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Game
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  UIComponentInspector.cs
 * author:    云毅
 * created:   2026
 * descrip:   Honor框架 UI组件编辑器扩展
 *            提供UI配置、Excel导出、运行时调试、画布/相机管理
 ***************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using Honor.Runtime;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Honor.Editor
{
    #region UI组件编辑器面板
    /// <summary>
    /// 【UI 系统编辑器面板】
    /// 功能：提供 UI 框架的可视化配置、调试、Excel 导出、运行时监控
    /// 作用：让开发者在 Inspector 面板直接配置/调试整个 UI 框架
    /// 属于：框架 -> UI 系统 -> 编辑器扩展
    /// </summary>
    [CustomEditor(typeof(UIComponent))]
    internal sealed class UIComponentInspector : HonorComponentInspector
    {
        #region 常量定义
        /// <summary>
        /// 等待界面（Loading）默认配置
        /// </summary>
        private readonly string m_WaitingABPathDefault = GamePathUtils.Prefab.GetFrameworkRootDirectoryRelativePath();
        private readonly string m_WaitingAssetNameDefault = "UIWaitingDefault";

        /// <summary>
        /// 飘字界面默认配置
        /// </summary>
        private readonly string m_FloatWordsABPathDefault = GamePathUtils.Prefab.GetFrameworkRootDirectoryRelativePath();
        private readonly string m_FloatWordsAssetNameDefault = "UIFloatWordsDefault";
        #endregion

        #region 序列化字段
        private SerializedProperty m_ScreenDesignedResolution = null;
        private SerializedProperty m_ScreenWidthHeightMatchValue = null;
        private SerializedProperty m_DestroyMaxNumPerFrame = null;

        private SerializedProperty m_WaitingUIABPath = null;
        private SerializedProperty m_WaitingUIAssetName = null;

        private SerializedProperty m_FloatWordsUIABPath = null;
        private SerializedProperty m_FloatWordsUIAssetName = null;
        private SerializedProperty m_FloatWordsDuration = null;

        private SerializedProperty m_ButtonInteractDuration = null;

        private SerializedProperty m_ScreenUICameras = null;
        private SerializedProperty m_SceneUICameras = null;

        private SerializedProperty m_ScreenUICanvas = null;
        private SerializedProperty m_SceneUICanvas = null;
        private SerializedProperty m_WebUICanvas = null;
        private SerializedProperty m_CheckOrientationState = null;

        private SerializedProperty m_CheckTextLocalizings = null;
        #endregion

        #region 私有变量
        /// <summary>
        /// 折叠面板状态缓存（运行时调试面板）
        /// </summary>
        private readonly HashSet<string> m_OpenedItems = new HashSet<string>();
        #endregion

        #region 初始化
        /// <summary>
        /// 初始化：绑定序列化属性
        /// </summary>
        private void OnEnable()
        {
            m_ScreenDesignedResolution      = serializedObject.FindProperty("m_ScreenDesignedResolution");
            m_ScreenWidthHeightMatchValue   = serializedObject.FindProperty("m_ScreenWidthHeightMatchValue");
            m_DestroyMaxNumPerFrame         = serializedObject.FindProperty("m_DestroyMaxNumPerFrame");

            m_WaitingUIABPath               = serializedObject.FindProperty("m_WaitingUIABPath");
            m_WaitingUIAssetName            = serializedObject.FindProperty("m_WaitingUIAssetName");

            m_FloatWordsUIABPath            = serializedObject.FindProperty("m_FloatWordsUIABPath");
            m_FloatWordsUIAssetName         = serializedObject.FindProperty("m_FloatWordsUIAssetName");
            m_FloatWordsDuration            = serializedObject.FindProperty("m_FloatWordsDuration");

            m_ButtonInteractDuration        = serializedObject.FindProperty("m_ButtonInteractDuration");

            m_ScreenUICameras               = serializedObject.FindProperty("m_ScreenUICameras");
            m_SceneUICameras                = serializedObject.FindProperty("m_SceneUICameras");

            m_ScreenUICanvas                = serializedObject.FindProperty("m_ScreenUICanvas");
            m_SceneUICanvas                 = serializedObject.FindProperty("m_SceneUICanvas");

            m_WebUICanvas                   = serializedObject.FindProperty("m_WebUICanvas");
            m_CheckOrientationState         = serializedObject.FindProperty("m_CheckOrientationState");

            m_CheckTextLocalizings          = serializedObject.FindProperty("m_CheckTextLocalizings");
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

            DrawExcelToolBar();
            EditorGUILayout.Separator();

            DrawUICameraAndCanvas();
            EditorGUILayout.Separator();

            DrawUICoreSettings();
            EditorGUILayout.Separator();

            if (Application.isPlaying)
            {
                DrawRuntimeDebugPanel();
            }

            serializedObject.ApplyModifiedProperties();
            Repaint();
        }
        #endregion

        #region 绘制 - Excel 工具栏
        /// <summary>
        /// 绘制表格操作按钮区域
        /// </summary>
        private void DrawExcelToolBar()
        {
            EditorGUILayout.BeginVertical("box");
            {
                EditorGUILayout.BeginHorizontal("box");
                {
                    DrawToolbarButton("打开UI表Excel",
                        () => TableExportEditorUtility.OpenExcel(GamePathUtils.UI.GetExcelFileFullPath()));

                    DrawToolbarButton("导出UI表Excel到Lua",
                        () => ExportExcelToLuaFromUI(Path.GetFileNameWithoutExtension(GamePathUtils.UI.GetExcelFileFullPath())));

                    DrawToolbarButton("打开UI表Excel所在文件夹",
                        () => TableExportEditorUtility.OpenDirectory(GamePathUtils.UI.GetExcelRootDirectoryFullPath()));
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
        }

        /// <summary>
        /// 绘制单个表格操作按钮，点击后执行回调并退出本次 GUI 事件
        /// </summary>
        /// <param name="buttonText">按钮显示文案</param>
        /// <param name="clickAction">点击时执行的业务动作</param>
        private static void DrawToolbarButton(string buttonText, Action clickAction)
        {
            if (!GUILayout.Button(buttonText))
            {
                return;
            }

            clickAction();
            GUIUtility.ExitGUI();
        }
        #endregion

        #region 绘制 - UI相机与画布
        /// <summary>
        /// 绘制UI相机与画布配置
        /// </summary>
        private void DrawUICameraAndCanvas()
        {
            EditorGUILayout.BeginVertical("box");
            {
                EditorGUILayout.PropertyField(m_ScreenUICameras, new GUIContent("屏幕UI相机"));
                EditorGUILayout.PropertyField(m_SceneUICameras, new GUIContent("场景UI相机"));

                m_ScreenUICanvas.objectReferenceValue = EditorGUILayout.ObjectField(
                    "屏幕UI画布", m_ScreenUICanvas.objectReferenceValue, typeof(Canvas), true);

                m_SceneUICanvas.objectReferenceValue = EditorGUILayout.ObjectField(
                    "场景UI画布", m_SceneUICanvas.objectReferenceValue, typeof(Canvas), true);

                EditorGUILayout.HelpBox(
                    "可根据项目实际情况对上述各类'画布'进行修改。（可修改为Honor树形结构外的其他节点）",
                    MessageType.Info);
            }
            EditorGUILayout.EndVertical();
        }
        #endregion

        #region 绘制 - UI核心配置
        /// <summary>
        /// 绘制UI系统核心配置项
        /// </summary>
        private void DrawUICoreSettings()
        {
            EditorGUILayout.BeginVertical("box");
            {
                // 设计分辨率
                m_ScreenDesignedResolution.vector2Value =
                    EditorGUILayout.Vector2Field("屏幕设计分辨率", m_ScreenDesignedResolution.vector2Value);

                if (m_ScreenUICanvas.objectReferenceValue != null)
                {
                    ((Canvas)m_ScreenUICanvas.objectReferenceValue).GetComponent<CanvasScaler>().referenceResolution =
                        m_ScreenDesignedResolution.vector2Value;
                }

                // 宽高适配
                m_ScreenWidthHeightMatchValue.floatValue = EditorGUILayout.Slider(
                    "屏幕宽高适配比例阀值（W-----H）", m_ScreenWidthHeightMatchValue.floatValue, 0f, 1f);

                if (m_ScreenUICanvas.objectReferenceValue != null)
                {
                    ((Canvas)m_ScreenUICanvas.objectReferenceValue).GetComponent<CanvasScaler>().matchWidthOrHeight =
                        m_ScreenWidthHeightMatchValue.floatValue;
                }

                // 每帧销毁数量
                m_DestroyMaxNumPerFrame.intValue =
                    EditorGUILayout.IntField("每帧最多销毁UI数量", m_DestroyMaxNumPerFrame.intValue);

                // 屏幕方向监测
                m_CheckOrientationState.boolValue =
                    EditorGUILayout.Toggle("监测屏幕方向变动", m_CheckOrientationState.boolValue);

                EditorGUILayout.HelpBox(
                    "启用监测后，请在Lua中监听事件：FwEventCmd.ScreenOrientationChanged，屏幕翻转时将告知前次与当次的屏幕方向，以及屏幕画布中刘海的尺寸。",
                    MessageType.Info);

                // TextLocalizing 监测
                m_CheckTextLocalizings.boolValue =
                    EditorGUILayout.Toggle("监测TextLocalizing脚本", m_CheckTextLocalizings.boolValue);

                EditorGUILayout.HelpBox(
                    "启用监测后，运行游戏时未挂载TextLocalizing脚本的文本对象所在路径将以错误信息的形式输出。\n注意：该选项有性能损耗，上线前请做关闭处理。",
                    MessageType.Info);

                EditorGUILayout.Separator();

                // 等待界面配置
                m_WaitingUIABPath.stringValue =
                    EditorGUILayout.TextField("菊花等待界面AB路径", m_WaitingUIABPath.stringValue);
                m_WaitingUIAssetName.stringValue =
                    EditorGUILayout.TextField("菊花等待界面Asset资源名称", m_WaitingUIAssetName.stringValue);

                if (GUILayout.Button("使用框架默认菊花等待界面"))
                {
                    m_WaitingUIABPath.stringValue = m_WaitingABPathDefault;
                    m_WaitingUIAssetName.stringValue = m_WaitingAssetNameDefault;
                    serializedObject.ApplyModifiedProperties();
                    GUIUtility.ExitGUI();
                }

                EditorGUILayout.Separator();

                // 飘字界面配置
                m_FloatWordsUIABPath.stringValue =
                    EditorGUILayout.TextField("飘字界面AB路径", m_FloatWordsUIABPath.stringValue);
                m_FloatWordsUIAssetName.stringValue =
                    EditorGUILayout.TextField("飘字界面Asset资源名称", m_FloatWordsUIAssetName.stringValue);
                m_FloatWordsDuration.floatValue =
                    EditorGUILayout.FloatField("飘字界面持续时间", m_FloatWordsDuration.floatValue);

                if (GUILayout.Button("使用框架默认飘字界面"))
                {
                    m_FloatWordsUIABPath.stringValue = m_FloatWordsABPathDefault;
                    m_FloatWordsUIAssetName.stringValue = m_FloatWordsAssetNameDefault;
                    serializedObject.ApplyModifiedProperties();
                    GUIUtility.ExitGUI();
                }

                EditorGUILayout.Separator();

                // 按钮间隔
                m_ButtonInteractDuration.floatValue =
                    EditorGUILayout.FloatField("按钮点击有效间隔", m_ButtonInteractDuration.floatValue);
                EditorGUILayout.HelpBox(
                    "可有效响应按钮连续点击的最小时间间隔，可起到UI按钮防爆击效果，默认值为0",
                    MessageType.Info);
            }
            EditorGUILayout.EndVertical();
        }
        #endregion

        #region 绘制 - 运行时调试面板
        /// <summary>
        /// 绘制运行时调试信息
        /// </summary>
        private void DrawRuntimeDebugPanel()
        {
            UIComponent targetUI = (UIComponent)target;

            // 等待界面引用计数
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField("[屏幕UI-菊花等待]UI引用计数", targetUI.WaitingUIRefCount.ToString());
            EditorGUILayout.EndVertical();

            // 模态UI
            DrawModalUIInfo(targetUI);
            // 非模态UI
            DrawUnModalUIList(targetUI);
            // 场景UI
            DrawSceneUIList(targetUI);
            // 附加UI
            DrawSubUIList(targetUI);
            // 卸载列表
            DrawUnloadUIList(targetUI);
        }
        #endregion

        #region 绘制 - 各类调试列表
        /// <summary>
        /// 绘制当前模态 UI 的详细信息
        /// </summary>
        private void DrawModalUIInfo(UIComponent targetUI)
        {
            if (targetUI.CurModalUI == null)
            {
                return;
            }

            UIFlagBehaviour modal = targetUI.CurModalUI;
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.ObjectField("[屏幕UI-模态]当前UI实例对象", modal.gameObject, typeof(GameObject), true);
            EditorGUILayout.LabelField("[屏幕UI-模态]当前UI实例ID", modal.PrefabInstanceGOBehaviour.InstanceID.ToString());
            EditorGUILayout.LabelField("[屏幕UI-模态]当前UI实例Asset资源名称", modal.UIInfo.AssetName);
            EditorGUILayout.LabelField("[屏幕UI-模态]当前UI实例AB资源路径", modal.UIInfo.ABPath);
            EditorGUILayout.LabelField("[屏幕UI-模态]当前UI实例ZOrder层级", modal.UIInfo.ZOrder.ToString());
            EditorGUILayout.EndVertical();
        }

        private void DrawUnModalUIList(UIComponent targetUI)
        {
            if (targetUI.UnModalUIList == null)
            {
                return;
            }

            string listName = "[屏幕UI-非模态]UI实例队列";
            if (!DrawFoldoutHeader(listName, targetUI.UnModalUIList.Count))
            {
                return;
            }

            DrawFoldoutBody(targetUI.UnModalUIList.Count, () =>
            {
                for (int index = 0; index < targetUI.UnModalUIList.Count; index++)
                {
                    DrawPlainUIInstanceRow(index, targetUI.UnModalUIList[index]);
                }
            });
        }

        private void DrawSceneUIList(UIComponent targetUI)
        {
            if (targetUI.SceneUIList == null)
            {
                return;
            }

            string listName = "[场景UI-普通]UI实例队列";
            if (!DrawFoldoutHeader(listName, targetUI.SceneUIList.Count))
            {
                return;
            }

            DrawFoldoutBody(targetUI.SceneUIList.Count, () =>
            {
                for (int index = 0; index < targetUI.SceneUIList.Count; index++)
                {
                    DrawPlainUIInstanceRow(index, targetUI.SceneUIList[index]);
                }
            });
        }

        private void DrawSubUIList(UIComponent targetUI)
        {
            if (targetUI.SubUIList == null)
            {
                return;
            }

            foreach (KeyValuePair<UIType, List<UIFlagBehaviour>> group in targetUI.SubUIList)
            {
                string listName = group.Key == UIType.Screen ? "[附加UI-屏幕]UI实例队列" : "[附加UI-场景]UI实例队列";
                if (!DrawFoldoutHeader(listName, group.Value.Count))
                {
                    continue;
                }

                DrawFoldoutBody(group.Value.Count, () =>
                {
                    for (int index = 0; index < group.Value.Count; index++)
                    {
                        DrawTolerantUIInstanceRow(index, group.Value[index]);
                    }
                });
            }
        }

        private void DrawUnloadUIList(UIComponent targetUI)
        {
            if (targetUI.UnloadUIList == null)
            {
                return;
            }

            string listName = "UI卸载列表";
            if (!DrawFoldoutHeader(listName, targetUI.UnloadUIList.Count))
            {
                return;
            }

            DrawFoldoutBody(targetUI.UnloadUIList.Count, () =>
            {
                for (int index = 0; index < targetUI.UnloadUIList.Count; index++)
                {
                    DrawUnloadUIInstanceRow(index, targetUI.UnloadUIList[index]);
                }
            });
        }

        /// <summary>
        /// 绘制标准 UI 实例调试行（非模态/场景列表，字段必定存在）
        /// </summary>
        private static void DrawPlainUIInstanceRow(int index, UIFlagBehaviour flag)
        {
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.ObjectField(AorTxt.Format("[{0}] UI实例对象", index), flag.gameObject, typeof(GameObject), true);
            EditorGUILayout.LabelField(AorTxt.Format("[{0}] UI实例ID", index), flag.PrefabInstanceGOBehaviour.InstanceID.ToString());
            EditorGUILayout.LabelField(AorTxt.Format("[{0}] Asset资源名称", index), flag.UIInfo.AssetName);
            EditorGUILayout.LabelField(AorTxt.Format("[{0}] AB资源路径", index), flag.UIInfo.ABPath);
            EditorGUILayout.LabelField(AorTxt.Format("[{0}] ZOrder层级", index), flag.GetComponent<Canvas>().sortingOrder.ToString());
            EditorGUILayout.EndVertical();
        }

        /// <summary>
        /// 绘制附加 UI 实例调试行（字段可能缺失，缺失时以 "---" 占位）
        /// </summary>
        private static void DrawTolerantUIInstanceRow(int index, UIFlagBehaviour flag)
        {
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.ObjectField(AorTxt.Format("[{0}] UI实例对象", index), flag.gameObject, typeof(GameObject), true);
            EditorGUILayout.LabelField(AorTxt.Format("[{0}] UI实例ID", index), flag.PrefabInstanceGOBehaviour ? flag.PrefabInstanceGOBehaviour.InstanceID.ToString() : "---");
            EditorGUILayout.LabelField(AorTxt.Format("[{0}] Asset资源名称", index), flag.UIInfo != null ? flag.UIInfo.AssetName : "---");
            EditorGUILayout.LabelField(AorTxt.Format("[{0}] AB资源路径", index), flag.UIInfo != null ? flag.UIInfo.ABPath : "---");
            EditorGUILayout.LabelField(AorTxt.Format("[{0}] ZOrder层级", index), flag.GetComponent<Canvas>() ? flag.GetComponent<Canvas>().sortingOrder.ToString() : "---");
            EditorGUILayout.EndVertical();
        }

        /// <summary>
        /// 绘制卸载列表 UI 实例调试行（无 ZOrder 信息）
        /// </summary>
        private static void DrawUnloadUIInstanceRow(int index, UIFlagBehaviour flag)
        {
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.ObjectField(AorTxt.Format("[{0}] UI实例对象", index), flag.gameObject, typeof(GameObject), true);
            EditorGUILayout.LabelField(AorTxt.Format("[{0}] UI实例ID", index), flag.PrefabInstanceGOBehaviour.InstanceID.ToString());
            EditorGUILayout.LabelField(AorTxt.Format("[{0}] Asset资源名称", index), flag.UIInfo.AssetName);
            EditorGUILayout.LabelField(AorTxt.Format("[{0}] AB资源路径", index), flag.UIInfo.ABPath);
            EditorGUILayout.EndVertical();
        }

        /// <summary>
        /// 绘制折叠标题并同步展开状态缓存，返回当前是否展开
        /// </summary>
        /// <param name="listName">折叠项唯一名称</param>
        /// <param name="entryCount">列表条目数量</param>
        private bool DrawFoldoutHeader(string listName, int entryCount)
        {
            bool lastState = m_OpenedItems.Contains(listName);
            bool currentState = EditorGUILayout.Foldout(lastState, AorTxt.Format("{0}({1})", listName, entryCount));
            UpdateFoldoutState(listName, lastState, currentState);
            return currentState;
        }

        /// <summary>
        /// 绘制折叠内容容器：空列表显示占位文案，否则调用回调逐行绘制
        /// </summary>
        /// <param name="entryCount">列表条目数量</param>
        /// <param name="drawRows">逐行绘制回调</param>
        private void DrawFoldoutBody(int entryCount, Action drawRows)
        {
            EditorGUILayout.BeginVertical("box");
            if (entryCount <= 0)
            {
                GUILayout.Label("列表为空 ...");
                EditorGUILayout.EndVertical();
                return;
            }

            drawRows();
            EditorGUILayout.EndVertical();
        }

        /// <summary>
        /// 更新折叠面板状态
        /// </summary>
        private void UpdateFoldoutState(string listName, bool lastState, bool currentState)
        {
            if (currentState != lastState)
            {
                if (currentState)
                {
                    m_OpenedItems.Add(listName);
                }
                else
                {
                    m_OpenedItems.Remove(listName);
                }
            }
        }
        #endregion

        #region 表格导出
        /// <summary>
        /// 【核心导出】UI Excel 表 → Lua 配置文件
        /// 自动生成 UIs.lua，供 Lua 层直接使用
        /// </summary>
        public bool ExportExcelToLuaFromUI(string excelFileName)
        {
            DataSet excelData = TableExportEditorUtility.GetExcelData(GamePathUtils.UI.GetExcelFileFullPath());
            string luaFileName = "UIs";
            string luaRootDirectory = GamePathUtils.UI.GetLuaScriptRootDirectoryFullPath();
            string luaFilePath = AorTxt.Format("{0}/{1}", luaRootDirectory, "UIs.lua.txt");

            if (!Directory.Exists(luaRootDirectory))
            {
                Directory.CreateDirectory(luaRootDirectory);
            }

            string tableDetail = excelData.Tables[0].Rows[0][1].ToString();
            StringBuilder luaBuilder = new StringBuilder();

            // 文件头
            luaBuilder
                .AppendLine("--=====================================================================================================")
                .AppendLine("-- (c) copyright 2026 - 2030, Honor.Game")
                .AppendLine("-- All Rights Reserved.")
                .AppendLine("-- ----------------------------------------------------------------------------------------------------")
                .AppendLine(AorTxt.Format("-- filename:  {0}.lua", luaFileName))
                .AppendLine(AorTxt.Format("-- descrip:   {0}", tableDetail))
                .AppendLine("-- notices:   该文件自动生成，请不要手动修改！")
                .AppendLine("--=====================================================================================================")
                .AppendLine();

            luaBuilder.AppendLine("---@class Tables.UIs_Item @UI配置项条目");

            int columnCount = excelData.Tables[0].Columns.Count;
            int rowCount = excelData.Tables[0].Rows.Count;
            List<string> columnKeys = new List<string>();
            List<string> columnTypes = new List<string>();

            for (int col = 1; col < columnCount; col++)
            {
                string columnKey = excelData.Tables[0].Rows[1][col].ToString();
                string columnType = excelData.Tables[0].Rows[2][col].ToString();
                string columnDesc = excelData.Tables[0].Rows[3][col].ToString().Replace("\r", "").Replace("\n", "");

                columnKeys.Add(columnKey);
                columnTypes.Add(columnType);
                luaBuilder.AppendLine($"---@field {columnKey} {columnType} @{columnDesc}");
            }

            luaBuilder.AppendLine();
            luaBuilder.AppendLine("---@class UIs @界面配置汇总");

            for (int rowIndex = 4; rowIndex < rowCount; rowIndex++)
            {
                string entryName = excelData.Tables[0].Rows[rowIndex][1].ToString();
                string entryDesc = excelData.Tables[0].Rows[rowIndex][2].ToString();
                luaBuilder.AppendLine("---@field " + entryName + " Tables.UIs_Item @" + entryDesc);
            }

            luaBuilder.AppendLine("UIs = {");
            for (int rowIndex = 4; rowIndex < rowCount; rowIndex++)
            {
                string entryName = excelData.Tables[0].Rows[rowIndex][1].ToString();
                luaBuilder.AppendLine($"    {entryName} = " + "{");

                for (int col = 1; col < columnCount; col++)
                {
                    string rawValue = excelData.Tables[0].Rows[rowIndex][col].ToString();
                    string luaValue = TableExportEditorUtility.GetLuaTypeFromExcel(rawValue, columnTypes[col - 1]);
                    luaBuilder.AppendLine($"        {columnKeys[col - 1]} = {luaValue},");
                }

                luaBuilder.AppendLine("    },");
            }

            luaBuilder.AppendLine("}");
            File.WriteAllText(luaFilePath, luaBuilder.ToString(), new UTF8Encoding(false));

            Log.Info("表格 " + excelFileName + " 数据导出完成");
            AssetDatabase.Refresh();
            Log.Info("UI 表导出完成：" + luaFilePath);

            return true;
        }
        #endregion
    }
    #endregion
}
