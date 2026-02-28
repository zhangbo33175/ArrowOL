using UnityEditor;

namespace Honor.Editor
{
    internal sealed partial class MapGridComponentInspector
    {
        /// <summary>
        /// 编辑器模式
        /// </summary>
        public void OnEditorMode()
        {
            EditorGUILayout.Space(6);
            EditorGUILayout.LabelField("地图基本信息设置");
            EditorGUILayout.Space(8);
            m_GridSize.intValue = EditorGUILayout.IntField("地图的大小:", m_GridSize.intValue);
            //EditorGUILayout.HelpBox("开发模式：用于切换开发环境与生产环境的系统配置，模式切换等操作。", MessageType.Info);
            EditorGUILayout.Space(6);
            m_GridWidth.intValue = EditorGUILayout.IntField("地图的宽度(网格列数):", m_GridWidth.intValue);
            EditorGUILayout.Space(6);
            m_GridHeight.intValue = EditorGUILayout.IntField("地图的高度(网格行数):", m_GridHeight.intValue);
            EditorGUILayout.Space(6);
            m_CellWidth.floatValue = EditorGUILayout.FloatField("网格单元宽度:", m_CellWidth.floatValue);
            EditorGUILayout.Space(6);
            m_CellHeight.floatValue = EditorGUILayout.FloatField("网格单元高度:", m_CellHeight.floatValue);
            EditorGUILayout.Space(16);
            EditorGUILayout.LabelField("————————————————————————————————————————————————————————————————————");
            EditorGUILayout.LabelField("网格颜色信息设置");
            EditorGUILayout.Space(6);
            m_NormalColor.colorValue = EditorGUILayout.ColorField("普通的颜色：", m_NormalColor.colorValue);
            EditorGUILayout.Space(6);
            m_WalkableColor.colorValue = EditorGUILayout.ColorField("可行走路径颜色:", m_WalkableColor.colorValue);
            EditorGUILayout.Space(6);
            m_BlockedColor.colorValue = EditorGUILayout.ColorField("不可行走路径颜色:", m_BlockedColor.colorValue);
            EditorGUILayout.Space(6);
            m_ShowGrid.boolValue = EditorGUILayout.Toggle("是否显示网格:", m_ShowGrid.boolValue);
            EditorGUILayout.Space(6);
        }
    }
}