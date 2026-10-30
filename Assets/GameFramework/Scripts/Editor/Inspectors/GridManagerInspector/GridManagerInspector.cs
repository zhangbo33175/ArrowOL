/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  GridManagerInspector.cs
 * author:    云毅
 * created:   2026
 * descrip:   网格管理组件自定义编辑器
 *            管理网格配置、调试参数的 Inspector 可视化编辑
 ***************************************************************/

using UnityEditor;
using UnityEngine;

namespace Honor.Editor
{
    /// <summary>
    /// 【网格管理组件编辑器】
    /// 功能：网格核心配置面板，管理网格原点、尺寸、调试参数
    /// 作用：Inspector 下可视化编辑网格各项参数，支持分组折叠与调试
    /// </summary>
    [CustomEditor(typeof(Honor.Runtime.GridManager))]
    public class GridManagerInspector : HonorComponentInspector
    {
        #region 常量配置

        private const string k_TitleGridConfig = "网格配置";
        private const string k_TitleDebugConfig = "调试配置";

        #endregion

        #region 序列化字段

        private SerializedProperty m_GridOrigin;
        private SerializedProperty m_GridUnitSize;
        private SerializedProperty m_DrawDebugGrid;
        private SerializedProperty m_DebugDrawMode;
        private SerializedProperty m_DebugGridSize;
        private SerializedProperty m_CellBorderColor;
        private SerializedProperty m_InnerColor;

        #endregion

        #region 初始化

        protected void OnEnable()
        {
            m_GridOrigin = serializedObject.FindProperty("GridOrigin");
            m_GridUnitSize = serializedObject.FindProperty("GridUnitSize");
            m_DrawDebugGrid = serializedObject.FindProperty("DrawDebugGrid");
            m_DebugDrawMode = serializedObject.FindProperty("DebugDrawMode");
            m_DebugGridSize = serializedObject.FindProperty("DebugGridSize");
            m_CellBorderColor = serializedObject.FindProperty("CellBorderColor");
            m_InnerColor = serializedObject.FindProperty("InnerColor");
        }

        #endregion

        #region 绘制 Inspector

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawGridConfig();
            DrawDebugConfig();

            serializedObject.ApplyModifiedProperties();
        }

        /// <summary>绘制网格基础配置</summary>
        private void DrawGridConfig()
        {
            EditorGUILayout.LabelField(k_TitleGridConfig, EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(m_GridOrigin, new GUIContent("网格原点 (GridOrigin)"));
            EditorGUILayout.PropertyField(m_GridUnitSize, new GUIContent("格子尺寸 (GridUnitSize)"));
            EditorGUILayout.Space();
        }

        /// <summary>绘制调试配置</summary>
        private void DrawDebugConfig()
        {
            EditorGUILayout.LabelField(k_TitleDebugConfig, EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(m_DrawDebugGrid, new GUIContent("绘制调试网格 (DrawDebugGrid)"));

            if (m_DrawDebugGrid.boolValue)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(m_DebugDrawMode, new GUIContent("2D/3D模式 (DebugDrawMode)"));
                EditorGUILayout.PropertyField(m_DebugGridSize, new GUIContent("网格数量 (DebugGridSize)"));
                EditorGUILayout.PropertyField(m_CellBorderColor, new GUIContent("网格线颜色 (CellBorderColor)"));
                EditorGUILayout.PropertyField(m_InnerColor, new GUIContent("网格填充颜色 (InnerColor)"));
                EditorGUI.indentLevel--;
            }
        }

        #endregion
    }
}
