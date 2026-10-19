/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Editor
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  AorInterfaceAdaptationEditor.cs
 * author:    云毅
 * created:   2026
 * descrip:   AorInterfaceAdaptation 组件的自定义 Inspector
 *            适配模式 / 铺满方式 / 对齐边 三个下拉框改为中文显示，
 *            其余字段保留原语义并补充中文 Tooltip。
 ***************************************************************/

using GameLib;
using UnityEditor;
using UnityEngine;

namespace GameLib.EditorTools
{
    /// <summary>
    /// AorInterfaceAdaptation 组件的自定义 Inspector（下拉框中文显示）
    /// 说明：Unity 默认枚举下拉框显示英文枚举名（Fill Screen / Stretch / Bottom To Top），
    ///       此处用中文显示名映射到同一枚举值，提升美术/策划在 Inspector 的配置可读性。
    /// </summary>
    [CustomEditor(typeof(AorInterfaceAdaptation))]
    public class AorInterfaceAdaptationEditor : UnityEditor.Editor
    {
        #region 枚举中文显示名
        //=========================================================================
        // 枚举中文显示名（数组索引 = 枚举整数值，顺序必须与枚举定义一致）
        //=========================================================================

        /// <summary>
        /// 适配模式中文名（顺序对应 <see cref="AorInterfaceAdaptation.AdaptMode"/> 枚举值）
        /// </summary>
        private static readonly string[] ModeDisplayNames =
        {
            "无",
            "铺满全屏",
            "紧贴参考上方",
            "紧贴参考下方",
        };

        /// <summary>
        /// 铺满方式中文名（顺序对应 <see cref="AorInterfaceAdaptation.FillMode"/> 枚举值）
        /// </summary>
        private static readonly string[] FillModeDisplayNames =
        {
            "拉伸（可能变形）",
            "保持宽高比裁边",
        };

        /// <summary>
        /// 对齐边中文名（顺序对应 <see cref="AorInterfaceAdaptation.AlignSide"/> 枚举值）
        /// </summary>
        private static readonly string[] AlignSideDisplayNames =
        {
            "下边贴参考上边",
            "上边贴参考下边",
            "左边贴参考右边",
            "右边贴参考左边",
        };
        #endregion

        #region 序列化属性
        //=========================================================================
        // 序列化属性引用
        //=========================================================================

        /// <summary>目标节点</summary>
        private SerializedProperty m_TargetProp;

        /// <summary>参考节点</summary>
        private SerializedProperty m_ReferenceProp;

        /// <summary>适配模式</summary>
        private SerializedProperty m_ModeProp;

        /// <summary>铺满方式</summary>
        private SerializedProperty m_FillModeProp;

        /// <summary>对齐边</summary>
        private SerializedProperty m_AlignSideProp;

        /// <summary>边距</summary>
        private SerializedProperty m_MarginProp;

        /// <summary>自动适配</summary>
        private SerializedProperty m_AdaptOnStartProp;
        #endregion

        #region 初始化
        //=========================================================================
        // 初始化：缓存序列化属性
        //=========================================================================

        /// <summary>
        /// 缓存序列化属性引用（按字段名查找，字段名须与 AorInterfaceAdaptation.cs 一致）
        /// </summary>
        private void OnEnable()
        {
            m_TargetProp = serializedObject.FindProperty("target");
            m_ReferenceProp = serializedObject.FindProperty("reference");
            m_ModeProp = serializedObject.FindProperty("mode");
            m_FillModeProp = serializedObject.FindProperty("fillMode");
            m_AlignSideProp = serializedObject.FindProperty("alignSide");
            m_MarginProp = serializedObject.FindProperty("margin");
            m_AdaptOnStartProp = serializedObject.FindProperty("adaptOnStart");
        }
        #endregion

        #region Inspector 绘制
        //=========================================================================
        // Inspector 绘制：三个下拉框中文显示，其余字段保留
        //=========================================================================

        /// <summary>
        /// 绘制自定义 Inspector（下拉框中文显示）
        /// </summary>
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(m_TargetProp,
                new GUIContent("目标节点", "需要进行适配的节点（默认本组件所在节点）"));
            EditorGUILayout.PropertyField(m_ReferenceProp,
                new GUIContent("参考节点", "Align 模式下的参考节点（如 UIMainPlaying 的 Bg）"));

            // 适配模式：中文下拉（数组索引 = 枚举值）
            m_ModeProp.intValue = EditorGUILayout.Popup(
                new GUIContent("适配模式", "选择适配行为：铺满全屏 / 紧贴参考边"),
                m_ModeProp.intValue, ModeDisplayNames);

            // 铺满方式：中文下拉
            m_FillModeProp.intValue = EditorGUILayout.Popup(
                new GUIContent("铺满方式", "FillScreen 模式下使用：拉伸 / 保持宽高比裁边"),
                m_FillModeProp.intValue, FillModeDisplayNames);

            // 对齐边：中文下拉
            m_AlignSideProp.intValue = EditorGUILayout.Popup(
                new GUIContent("对齐边", "Align 模式下 target 与 reference 的对齐边"),
                m_AlignSideProp.intValue, AlignSideDisplayNames);

            EditorGUILayout.PropertyField(m_MarginProp,
                new GUIContent("边距", "Align 模式下的预留边距（正数向外扩）"));
            EditorGUILayout.PropertyField(m_AdaptOnStartProp,
                new GUIContent("自动适配", "Start 时是否自动执行适配"));

            serializedObject.ApplyModifiedProperties();
        }
        #endregion
    }
}
