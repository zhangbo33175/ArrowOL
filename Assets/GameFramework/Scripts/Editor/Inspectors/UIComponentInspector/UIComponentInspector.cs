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
    /// <summary>
    /// 【UI 系统编辑器面板】
    /// 功能：提供 UI 框架的可视化配置、调试、Excel 导出、运行时监控
    /// 作用：让开发者在 Inspector 面板直接配置/调试整个 UI 框架
    /// 属于：框架 -> UI 系统 -> 编辑器扩展
    /// </summary>
    [CustomEditor(typeof(UIComponent))]
    internal sealed class UIComponentInspector : HonorComponentInspector
    {
        // 等待界面（Loading）默认配置
        private string m_WaitingABPathDefault = GamePathUtils.Prefab.GetFrameworkRootDirectoryRelativePath();
        private string m_WaitingAssetNameDefault = "UIWaitingDefault";

        // 飘字界面默认配置
        private string m_FloatWordsABPathDefault = GamePathUtils.Prefab.GetFrameworkRootDirectoryRelativePath();
        private string m_FloatWordsAssetNameDefault = "UIFloatWordsDefault";

        // 序列化字段（绑定 UIComponent 中的变量）
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

        // 折叠面板状态缓存（运行时调试面板）
        private readonly HashSet<string> m_OpenedItems = new HashSet<string>();

        /// <summary>
        /// 初始化：绑定序列化属性
        /// </summary>
        private void OnEnable()
        {
            m_ScreenDesignedResolution = serializedObject.FindProperty("m_ScreenDesignedResolution");
            m_ScreenWidthHeightMatchValue = serializedObject.FindProperty("m_ScreenWidthHeightMatchValue");
            m_DestroyMaxNumPerFrame = serializedObject.FindProperty("m_DestroyMaxNumPerFrame");

            m_WaitingUIABPath = serializedObject.FindProperty("m_WaitingUIABPath");
            m_WaitingUIAssetName = serializedObject.FindProperty("m_WaitingUIAssetName");

            m_FloatWordsUIABPath = serializedObject.FindProperty("m_FloatWordsUIABPath");
            m_FloatWordsUIAssetName = serializedObject.FindProperty("m_FloatWordsUIAssetName");
            m_FloatWordsDuration = serializedObject.FindProperty("m_FloatWordsDuration");

            m_ButtonInteractDuration = serializedObject.FindProperty("m_ButtonInteractDuration");

            m_ScreenUICameras = serializedObject.FindProperty("m_ScreenUICameras");
            m_SceneUICameras = serializedObject.FindProperty("m_SceneUICameras");

            m_ScreenUICanvas = serializedObject.FindProperty("m_ScreenUICanvas");
            m_SceneUICanvas = serializedObject.FindProperty("m_SceneUICanvas");

            m_WebUICanvas = serializedObject.FindProperty("m_WebUICanvas");
            m_CheckOrientationState = serializedObject.FindProperty("m_CheckOrientationState");

            m_CheckTextLocalizings = serializedObject.FindProperty("m_CheckTextLocalizings");
        }

        /// <summary>
        /// 绘制 Inspector 面板
        /// </summary>
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();

            // ====================== 表格操作按钮 ======================
            EditorGUILayout.BeginVertical("box");
            {
                EditorGUILayout.BeginHorizontal("box");
                {
                    if (GUILayout.Button("打开UI表Excel"))
                    {
                        TableExportEditorUtility.OpenExcel(GamePathUtils.UI.GetExcelFileFullPath());
                        GUIUtility.ExitGUI();
                    }

                    if (GUILayout.Button("导出UI表Excel到Lua"))
                    {
                        ExportExcelToLuaFromUI(
                            System.IO.Path.GetFileNameWithoutExtension(GamePathUtils.UI.GetExcelFileFullPath()));
                        GUIUtility.ExitGUI();
                    }

                    if (GUILayout.Button("打开UI表Excel所在文件夹"))
                    {
                        TableExportEditorUtility.OpenDirectory(GamePathUtils.UI.GetExcelRootDirectoryFullPath());
                        GUIUtility.ExitGUI();
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.Separator();

            // ====================== UI 相机 & 画布 ======================
            EditorGUILayout.BeginVertical("box");
            {
                EditorGUILayout.PropertyField(m_ScreenUICameras, new GUIContent("屏幕UI相机"));
                EditorGUILayout.PropertyField(m_SceneUICameras, new GUIContent("场景UI相机"));

                m_ScreenUICanvas.objectReferenceValue = EditorGUILayout.ObjectField("屏幕UI画布", m_ScreenUICanvas.objectReferenceValue, typeof(Canvas), true);
                m_SceneUICanvas.objectReferenceValue = EditorGUILayout.ObjectField("场景UI画布", m_SceneUICanvas.objectReferenceValue, typeof(Canvas), true);

                EditorGUILayout.HelpBox("可根据项目实际情况对上述各类'画布'进行修改。（可修改为Honor树形结构外的其他节点）", MessageType.Info);
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.Separator();

            // ====================== UI 系统核心配置 ======================
            EditorGUILayout.BeginVertical("box");
            {
                m_ScreenDesignedResolution.vector2Value =EditorGUILayout.Vector2Field("屏幕设计分辨率", m_ScreenDesignedResolution.vector2Value);
                
                ((Canvas)m_ScreenUICanvas.objectReferenceValue).GetComponent<CanvasScaler>().referenceResolution =
                    m_ScreenDesignedResolution.vector2Value;
                
                m_ScreenWidthHeightMatchValue.floatValue = EditorGUILayout.Slider("屏幕宽高适配比例阀值（W-----H）",
                    m_ScreenWidthHeightMatchValue.floatValue, 0f, 1f);
                
                ((Canvas)m_ScreenUICanvas.objectReferenceValue).GetComponent<CanvasScaler>().matchWidthOrHeight =
                    m_ScreenWidthHeightMatchValue.floatValue;
                
                m_DestroyMaxNumPerFrame.intValue =EditorGUILayout.IntField("每帧最多销毁UI数量", m_DestroyMaxNumPerFrame.intValue);
               
                m_CheckOrientationState.boolValue =EditorGUILayout.Toggle("监测屏幕方向变动", m_CheckOrientationState.boolValue);
               
                EditorGUILayout.HelpBox("启用监测后，请在Lua中监听事件：FwEventCmd.ScreenOrientationChanged，屏幕翻转时将告知前次与当次的屏幕方向，以及屏幕画布中刘海的尺寸。",
                    MessageType.Info);
                
                m_CheckTextLocalizings.boolValue =EditorGUILayout.Toggle("监测TextLocalizing脚本", m_CheckTextLocalizings.boolValue);
                
                EditorGUILayout.HelpBox("启用监测后，运行游戏时未挂载TextLocalizing脚本的文本对象所在路径将以错误信息的形式输出。\n注意：该选项有性能损耗，上线前请做关闭处理。",
                    MessageType.Info);

                EditorGUILayout.Separator();

                // 等待界面（Loading）配置
                m_WaitingUIABPath.stringValue = EditorGUILayout.TextField("菊花等待界面AB路径", m_WaitingUIABPath.stringValue);
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

                m_FloatWordsUIABPath.stringValue =EditorGUILayout.TextField("飘字界面AB路径", m_FloatWordsUIABPath.stringValue);
                
                m_FloatWordsUIAssetName.stringValue =EditorGUILayout.TextField("飘字界面Asset资源名称", m_FloatWordsUIAssetName.stringValue);
                
                m_FloatWordsDuration.floatValue =EditorGUILayout.FloatField("飘字界面持续时间", m_FloatWordsDuration.floatValue);

                if (GUILayout.Button("使用框架默认飘字界面"))
                {
                    m_FloatWordsUIABPath.stringValue = m_FloatWordsABPathDefault;
                    m_FloatWordsUIAssetName.stringValue = m_FloatWordsAssetNameDefault;
                    serializedObject.ApplyModifiedProperties();
                    GUIUtility.ExitGUI();
                }

                EditorGUILayout.Separator();

                m_ButtonInteractDuration.floatValue =EditorGUILayout.FloatField("按钮点击有效间隔", m_ButtonInteractDuration.floatValue);
                
                EditorGUILayout.HelpBox("可有效响应按钮连续点击的最小时间间隔，可起到UI按钮防爆击效果，默认值为0", MessageType.Info);
            }
            EditorGUILayout.EndVertical();

            // ====================== 运行时调试面板 ======================
            if (Application.isPlaying)
            {
                UIComponent uiComp = (UIComponent)target;

                // 等待界面引用计数
                EditorGUILayout.BeginVertical("box");
                EditorGUILayout.LabelField("[屏幕UI-菊花等待]UI引用计数", uiComp.WaitingUIRefCount.ToString());
                EditorGUILayout.EndVertical();

                // 模态UI相关信息
                if (uiComp.CurModalUI != null)
                {
                    EditorGUILayout.BeginVertical("box");
                    EditorGUILayout.ObjectField("[屏幕UI-模态]当前UI实例对象", uiComp.CurModalUI.gameObject, typeof(GameObject), true);
                    EditorGUILayout.LabelField("[屏幕UI-模态]当前UI实例ID",
                        uiComp.CurModalUI.PrefabInstanceGOBehaviour.InstanceID.ToString());
                    EditorGUILayout.LabelField("[屏幕UI-模态]当前UI实例Asset资源名称", uiComp.CurModalUI.UIInfo.AssetName);
                    EditorGUILayout.LabelField("[屏幕UI-模态]当前UI实例AB资源路径", uiComp.CurModalUI.UIInfo.ABPath);
                    EditorGUILayout.LabelField("[屏幕UI-模态]当前UI实例ZOrder层级", uiComp.CurModalUI.UIInfo.ZOrder.ToString());
                    EditorGUILayout.EndVertical();
                }

                if (uiComp.ModalUIInfoList != null)
                {
                    string listName = "[屏幕UI-模态]UI信息缓冲队列";
                    bool lastState = m_OpenedItems.Contains(listName);
                    bool currentState = EditorGUILayout.Foldout(lastState,
                        AorTxt.Format("{0}({1})", listName, uiComp.ModalUIInfoList.Count));
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

                    if (currentState)
                    {
                        EditorGUILayout.BeginVertical("box");
                        {
                            if (uiComp.ModalUIInfoList.Count > 0)
                            {
                                for (int index = 0; index < uiComp.ModalUIInfoList.Count; index++)
                                {
                                    EditorGUILayout.BeginVertical("box");
                                    EditorGUILayout.LabelField(AorTxt.Format("[{0}] Asset资源名称", index),
                                        uiComp.ModalUIInfoList[index].AssetName);
                                    EditorGUILayout.LabelField(AorTxt.Format("[{0}] AB资源路径", index),
                                        uiComp.ModalUIInfoList[index].ABPath);
                                    EditorGUILayout.LabelField(AorTxt.Format("[{0}] ZOrder层级", index),
                                        uiComp.ModalUIInfoList[index].ZOrder.ToString());
                                    EditorGUILayout.EndVertical();
                                }
                            }
                            else
                            {
                                GUILayout.Label("列表为空 ...");
                            }
                        }
                        EditorGUILayout.EndVertical();
                    }
                }

                // 非模态UI相关信息
                if (uiComp.UnModalUIList != null)
                {
                    string listName = "[屏幕UI-非模态]UI实例队列";
                    bool lastState = m_OpenedItems.Contains(listName);
                    bool currentState = EditorGUILayout.Foldout(lastState,
                        AorTxt.Format("{0}({1})", listName, uiComp.UnModalUIList.Count));
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

                    if (currentState)
                    {
                        EditorGUILayout.BeginVertical("box");
                        {
                            if (uiComp.UnModalUIList.Count > 0)
                            {
                                for (int index = 0; index < uiComp.UnModalUIList.Count; index++)
                                {
                                    EditorGUILayout.BeginVertical("box");
                                    EditorGUILayout.ObjectField(AorTxt.Format("[{0}] UI实例对象", index),
                                        uiComp.UnModalUIList[index].gameObject, typeof(GameObject), true);
                                    EditorGUILayout.LabelField(AorTxt.Format("[{0}] UI实例ID", index),
                                        uiComp.UnModalUIList[index].PrefabInstanceGOBehaviour.InstanceID.ToString());
                                    EditorGUILayout.LabelField(AorTxt.Format("[{0}] Asset资源名称", index),
                                        uiComp.UnModalUIList[index].UIInfo.AssetName);
                                    EditorGUILayout.LabelField(AorTxt.Format("[{0}] AB资源路径", index),
                                        uiComp.UnModalUIList[index].UIInfo.ABPath);
                                    EditorGUILayout.LabelField(AorTxt.Format("[{0}] ZOrder层级", index),
                                        uiComp.UnModalUIList[index].GetComponent<Canvas>().sortingOrder.ToString());
                                    EditorGUILayout.EndVertical();
                                }
                            }
                            else
                            {
                                GUILayout.Label("列表为空 ...");
                            }
                        }
                        EditorGUILayout.EndVertical();
                    }
                }

                // 场景UI相关信息
                if (uiComp.SceneUIList != null)
                {
                    string listName = "[场景UI-普通]UI实例队列";
                    bool lastState = m_OpenedItems.Contains(listName);
                    bool currentState = EditorGUILayout.Foldout(lastState,
                        AorTxt.Format("{0}({1})", listName, uiComp.SceneUIList.Count));
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

                    if (currentState)
                    {
                        EditorGUILayout.BeginVertical("box");
                        {
                            if (uiComp.SceneUIList.Count > 0)
                            {
                                for (int index = 0; index < uiComp.SceneUIList.Count; index++)
                                {
                                    EditorGUILayout.BeginVertical("box");
                                    EditorGUILayout.ObjectField(AorTxt.Format("[{0}] UI实例对象", index),
                                        uiComp.SceneUIList[index].gameObject, typeof(GameObject), true);
                                    EditorGUILayout.LabelField(AorTxt.Format("[{0}] UI实例ID", index),
                                        uiComp.SceneUIList[index].PrefabInstanceGOBehaviour.InstanceID.ToString());
                                    EditorGUILayout.LabelField(AorTxt.Format("[{0}] Asset资源名称", index),
                                        uiComp.SceneUIList[index].UIInfo.AssetName);
                                    EditorGUILayout.LabelField(AorTxt.Format("[{0}] AB资源路径", index),
                                        uiComp.SceneUIList[index].UIInfo.ABPath);
                                    EditorGUILayout.LabelField(AorTxt.Format("[{0}] ZOrder层级", index),
                                        uiComp.SceneUIList[index].GetComponent<Canvas>().sortingOrder.ToString());
                                    EditorGUILayout.EndVertical();
                                }
                            }
                            else
                            {
                                GUILayout.Label("列表为空 ...");
                            }
                        }
                        EditorGUILayout.EndVertical();
                    }
                }

                // 附加UI相关信息
                if (uiComp.SubUIList != null)
                {
                    foreach (var itr in uiComp.SubUIList)
                    {
                        string listName = itr.Key == UIType.Screen ? "[附加UI-屏幕]UI实例队列" : "[附加UI-场景]UI实例队列";
                        bool lastState = m_OpenedItems.Contains(listName);
                        bool currentState = EditorGUILayout.Foldout(lastState,
                            AorTxt.Format("{0}({1})", listName, itr.Value.Count));
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

                        if (currentState)
                        {
                            EditorGUILayout.BeginVertical("box");
                            {
                                if (itr.Value.Count > 0)
                                {
                                    for (int index = 0; index < itr.Value.Count; index++)
                                    {
                                        EditorGUILayout.BeginVertical("box");
                                        EditorGUILayout.ObjectField(AorTxt.Format("[{0}] UI实例对象", index),
                                            itr.Value[index].gameObject, typeof(GameObject), true);
                                        EditorGUILayout.LabelField(AorTxt.Format("[{0}] UI实例ID", index),
                                            itr.Value[index].PrefabInstanceGOBehaviour
                                                ? itr.Value[index].PrefabInstanceGOBehaviour.InstanceID.ToString()
                                                : "---");
                                        EditorGUILayout.LabelField(AorTxt.Format("[{0}] Asset资源名称", index),
                                            itr.Value[index].UIInfo != null
                                                ? itr.Value[index].UIInfo.AssetName
                                                : "---");
                                        EditorGUILayout.LabelField(AorTxt.Format("[{0}] AB资源路径", index),
                                            itr.Value[index].UIInfo != null ? itr.Value[index].UIInfo.ABPath : "---");
                                        EditorGUILayout.LabelField(AorTxt.Format("[{0}] ZOrder层级", index),
                                            itr.Value[index].GetComponent<Canvas>()
                                                ? itr.Value[index].GetComponent<Canvas>().sortingOrder.ToString()
                                                : "---");
                                        EditorGUILayout.EndVertical();
                                    }
                                }
                                else
                                {
                                    GUILayout.Label("列表为空 ...");
                                }
                            }
                            EditorGUILayout.EndVertical();
                        }
                    }
                }

                // UI卸载列表相关
                if (uiComp.UnloadUIList != null)
                {
                    string listName = "UI卸载列表";
                    bool lastState = m_OpenedItems.Contains(listName);
                    bool currentState = EditorGUILayout.Foldout(lastState,
                        AorTxt.Format("{0}({1})", listName, uiComp.UnloadUIList.Count));
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

                    if (currentState)
                    {
                        EditorGUILayout.BeginVertical("box");
                        {
                            if (uiComp.UnloadUIList.Count > 0)
                            {
                                for (int index = 0; index < uiComp.UnloadUIList.Count; index++)
                                {
                                    EditorGUILayout.BeginVertical("box");
                                    EditorGUILayout.ObjectField(AorTxt.Format("[{0}] UI实例对象", index),
                                        uiComp.UnloadUIList[index].gameObject, typeof(GameObject), true);
                                    EditorGUILayout.LabelField(AorTxt.Format("[{0}] UI实例ID", index),
                                        uiComp.UnloadUIList[index].PrefabInstanceGOBehaviour.InstanceID.ToString());
                                    EditorGUILayout.LabelField(AorTxt.Format("[{0}] Asset资源名称", index),
                                        uiComp.UnloadUIList[index].UIInfo.AssetName);
                                    EditorGUILayout.LabelField(AorTxt.Format("[{0}] AB资源路径", index),
                                        uiComp.UnloadUIList[index].UIInfo.ABPath);
                                    EditorGUILayout.EndVertical();
                                }
                            }
                            else
                            {
                                GUILayout.Label("列表为空 ...");
                            }
                        }
                        EditorGUILayout.EndVertical();
                    }
                }
            }

            serializedObject.ApplyModifiedProperties();

            Repaint();
        }

        protected override void OnCompileStart()
        {
            base.OnCompileStart();
        }

        protected override void OnCompileComplete()
        {
            base.OnCompileComplete();
        }

        /// <summary>
        /// 【核心导出】UI Excel 表 → Lua 配置文件
        /// 自动生成 UIs.lua，供 Lua 层直接使用
        /// </summary>
        /// <param name="openExcelNamePre">UI表格名称</param>
        /// <returns></returns>
        public bool ExportExcelToLuaFromUI(string openExcelNamePre)
        {
            // 获取表格内容
            DataSet result = TableExportEditorUtility.GetExcelData(GamePathUtils.UI.GetExcelFileFullPath());
            string strLuaName = "UIs";
            string directoryPath = GamePathUtils.UI.GetLuaScriptRootDirectoryFullPath();
            string luaPath = AorTxt.Format("{0}/{1}", directoryPath, "UIs.lua.txt");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            // 获取表格说明
            string tableDetail = result.Tables[0].Rows[0][1].ToString();
            // 开始添加文件头
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder
                .AppendLine(
                    "--=====================================================================================================")
                .AppendLine("-- (c) copyright 2026 - 2030, Honor.Game")
                .AppendLine("-- All Rights Reserved.")
                .AppendLine(
                    "-- ----------------------------------------------------------------------------------------------------")
                .AppendLine(AorTxt.Format("-- filename:  {0}.lua", strLuaName))
                .AppendLine(AorTxt.Format("-- descrip:   {0}", tableDetail))
                .AppendLine("-- notices:   该文件自动生成，请不要手动修改！")
                .AppendLine(
                    "--=====================================================================================================")
                .AppendLine("");

            stringBuilder.AppendLine("---@class Tables.UIs_Item @UI配置项条目");

            int columns = result.Tables[0].Columns.Count;
            int rows = result.Tables[0].Rows.Count;
            List<string> colKeys = new List<string>();
            List<string> colTypes = new List<string>();
            for (int col = 1; col < columns; col++)
            {
                string curColKey = result.Tables[0].Rows[1][col].ToString();
                string curColType = result.Tables[0].Rows[2][col].ToString();
                string curColDesc = result.Tables[0].Rows[3][col].ToString().Replace("\r", string.Empty)
                    .Replace("\n", string.Empty);
                colKeys.Add(curColKey);
                colTypes.Add(curColType);
                stringBuilder.AppendLine($"---@field {curColKey} {curColType} @{curColDesc}");
            }

            stringBuilder.AppendLine("");

            stringBuilder.AppendLine("---@class UIs @界面配置汇总");
            for (int curRow = 4; curRow < rows; curRow++)
            {
                string nameStr = result.Tables[0].Rows[curRow][1].ToString();
                string descStr = result.Tables[0].Rows[curRow][2].ToString();
                stringBuilder.AppendLine("---@field " + nameStr + " Tables.UIs_Item @" + descStr);
            }

            stringBuilder.AppendLine("UIs = {");
            for (int curRow = 4; curRow < rows; curRow++)
            {
                string curName = result.Tables[0].Rows[curRow][1].ToString();
                stringBuilder.AppendLine($"    {curName} = " + "{");
                for (int curCol = 1; curCol < columns; curCol++)
                {
                    string curValue = result.Tables[0].Rows[curRow][curCol].ToString();
                    string changedValue = TableExportEditorUtility.GetLuaTypeFromExcel(curValue, colTypes[curCol - 1]);
                    stringBuilder.AppendLine($"        {colKeys[curCol - 1]} = {changedValue},");
                }

                stringBuilder.AppendLine("    },");
            }

            stringBuilder.AppendLine("}");

            File.WriteAllText(luaPath, stringBuilder.ToString(), new System.Text.UTF8Encoding(false));
            Runtime.Log.Info("表格 " + openExcelNamePre + " 数据导出完成");

            AssetDatabase.Refresh();
            Log.Info("UI 表导出完成：" + luaPath);
            return true;
        }
    }
}