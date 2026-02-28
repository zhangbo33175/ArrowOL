/***************************************************************
 * (c) copyright 2026 - 2031, Honor.Editor
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  MapGridComponentInspector.cs
 * author:    zhangbo
 * created:   2026/1/1
 * descrip:   地图编辑器面板定制
 ***************************************************************/

using GameLib;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Honor.Editor
{
    [CustomEditor(typeof(GridMapManager))]
    internal sealed partial class MapGridComponentInspector : HonorComponentInspector
    {
        public Grid m_Grid;
        /// <summary>
        ///  地图的大小
        /// </summary>
        private SerializedProperty m_GridSize = null;

        /// <summary>
        /// 地图的宽度，网格列数
        /// </summary>
        private SerializedProperty m_GridWidth = null;

        /// <summary>
        /// 地图的高度，网格行数
        /// </summary>
        private SerializedProperty m_GridHeight = null;

        /// <summary>
        /// 网格单元宽度
        /// </summary>
        private SerializedProperty m_CellWidth = null;

        /// <summary>
        /// 网格单元高度
        /// </summary>
        private SerializedProperty m_CellHeight = null;

        /// <summary>
        /// 普通的颜色
        /// </summary>
        private SerializedProperty m_NormalColor = null;

        /// <summary>
        /// 可行走路径颜色
        /// </summary>
        private SerializedProperty m_WalkableColor = null;

        /// <summary>
        /// 不可行走路径颜色
        /// </summary>
        private SerializedProperty m_BlockedColor = null;

        /// <summary>
        /// 是否显示网格
        /// </summary>
        private SerializedProperty m_ShowGrid = null;

        private void OnEnable()
        {
            m_GridSize = serializedObject.FindProperty("m_GridSize");
            m_GridWidth = serializedObject.FindProperty("m_GridWidth");
            m_GridHeight = serializedObject.FindProperty("m_GridHeight");
            m_CellWidth = serializedObject.FindProperty("m_CellWidth");
            m_CellHeight = serializedObject.FindProperty("m_CellHeight");
            m_NormalColor = serializedObject.FindProperty("m_NormalColor");
            m_WalkableColor = serializedObject.FindProperty("m_WalkableColor");
            m_BlockedColor = serializedObject.FindProperty("m_BlockedColor");
            m_ShowGrid = serializedObject.FindProperty("m_ShowGrid");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();
            OnEditorMode();

            OnShowButton();
       

            serializedObject.ApplyModifiedProperties();
            Repaint();
        }

        public void OnShowButton()
        {
            EditorGUILayout.Space(3);
            if (GUILayout.Button("保存地图资源"))
            {
                SaveMap();
                GUIUtility.ExitGUI();
            } 
        }


        public void SaveMap()
        {
            m_Grid = Object.FindObjectsOfType<Grid>()[0];
            GridMapManager gridMap = (GridMapManager)target;
            gridMap.SaveTilemap(m_Grid,"");
        }
    }
}