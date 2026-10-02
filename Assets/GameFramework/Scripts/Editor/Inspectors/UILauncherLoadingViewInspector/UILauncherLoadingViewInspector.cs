/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  UILauncherLoadingViewInspector.cs
 * author:    云毅
 * created:   2026
 * descrip:   启动加载界面行为编辑器面板
 *            以框式行形式可视化配置进入/退出切换过渡（启用勾选 + 过渡时间），
 *            并以中文标签绘制全部序列化UI引用字段
 ***************************************************************/

using Honor.Runtime;
using UnityEditor;
using UnityEngine;

namespace Honor.Editor
{
    /// <summary>
    /// 【启动加载界面行为编辑器面板】
    /// 功能：加载界面的切换过渡（进入式/退出式）可视化配置 + 序列化UI引用字段的中文标签绘制
    /// 作用：在 Inspector 上以框式行呈现进入/退出切换过渡的启用开关与过渡时长；
    ///       将 Start/Retry/Close 等英文字段名替换为中文标签，便于识别与调校
    /// 属于：框架 -> UI(SpecialUIs) 启动加载界面 -> 编辑器扩展
    /// </summary>
    [CustomEditor(typeof(UILauncherLoadingView))]
    [CanEditMultipleObjects]
    internal sealed class UILauncherLoadingViewInspector : HonorComponentInspector
    {
        #region 序列化属性
        //=========================================================================
        // 序列化属性（UI引用字段 + 切换过渡字段）
        //=========================================================================

        // --- 按钮 ---
        private SerializedProperty m_StartButton;      // 开始按钮
        private SerializedProperty m_StartButtonText;  // 开始按钮文字（UGUI）
        private SerializedProperty m_StartButtonTextTMP; // 开始按钮文字（TMP）
        private SerializedProperty m_RetryButton;      // 重试按钮
        private SerializedProperty m_RetryButtonText;  // 重试按钮文字（UGUI）
        private SerializedProperty m_RetryButtonTextTMP; // 重试按钮文字（TMP）
        private SerializedProperty m_CloseButton;      // 关闭按钮
        private SerializedProperty m_CloseButtonText;  // 关闭按钮文字（UGUI）
        private SerializedProperty m_CloseButtonTextTMP; // 关闭按钮文字（TMP）

        // --- 进度 ---
        private SerializedProperty m_ProgressSlider;   // 进度条
        private SerializedProperty m_ProgressText;     // 进度百分比文字（UGUI）
        private SerializedProperty m_ProgressTextTMP;  // 进度百分比文字（TMP）

        // --- 字节/文件/描述 ---
        private SerializedProperty m_BytesNumText;     // 字节数字文本（UGUI）
        private SerializedProperty m_BytesNumTextTMP;  // 字节数字文本（TMP）
        private SerializedProperty m_FileNumText;      // 文件数文本（UGUI）
        private SerializedProperty m_FileNumTextTMP;   // 文件数文本（TMP）
        private SerializedProperty m_DescText;         // 描述文本（UGUI）
        private SerializedProperty m_DescTextTMP;      // 描述文本（TMP）

        // --- 切换过渡 ---
        private SerializedProperty m_EnterTransitionEnabled;  // 进入式-切换过渡 启用
        private SerializedProperty m_EnterTransitionDuration; // 进入过渡时长
        private SerializedProperty m_ExitTransitionEnabled;   // 退出式-切换过渡 启用
        private SerializedProperty m_ExitTransitionDuration;  // 退出过渡时长
        #endregion

        #region 生命周期
        //=========================================================================
        // 生命周期
        //=========================================================================

        /// <summary>
        /// 启用：绑定全部序列化属性（UI引用字段 + 切换过渡字段）
        /// </summary>
        private void OnEnable()
        {
            // 按钮
            m_StartButton          = serializedObject.FindProperty("m_StartButton");
            m_StartButtonText      = serializedObject.FindProperty("m_StartButtonText");
            m_StartButtonTextTMP   = serializedObject.FindProperty("m_StartButtonTextTMP");
            m_RetryButton          = serializedObject.FindProperty("m_RetryButton");
            m_RetryButtonText      = serializedObject.FindProperty("m_RetryButtonText");
            m_RetryButtonTextTMP   = serializedObject.FindProperty("m_RetryButtonTextTMP");
            m_CloseButton          = serializedObject.FindProperty("m_CloseButton");
            m_CloseButtonText      = serializedObject.FindProperty("m_CloseButtonText");
            m_CloseButtonTextTMP   = serializedObject.FindProperty("m_CloseButtonTextTMP");

            // 进度
            m_ProgressSlider       = serializedObject.FindProperty("m_ProgressSlider");
            m_ProgressText         = serializedObject.FindProperty("m_ProgressText");
            m_ProgressTextTMP      = serializedObject.FindProperty("m_ProgressTextTMP");

            // 字节/文件/描述
            m_BytesNumText         = serializedObject.FindProperty("m_BytesNumText");
            m_BytesNumTextTMP      = serializedObject.FindProperty("m_BytesNumTextTMP");
            m_FileNumText          = serializedObject.FindProperty("m_FileNumText");
            m_FileNumTextTMP       = serializedObject.FindProperty("m_FileNumTextTMP");
            m_DescText             = serializedObject.FindProperty("m_DescText");
            m_DescTextTMP          = serializedObject.FindProperty("m_DescTextTMP");

            // 切换过渡
            m_EnterTransitionEnabled  = serializedObject.FindProperty("m_EnterTransitionEnabled");
            m_EnterTransitionDuration = serializedObject.FindProperty("m_EnterTransitionDuration");
            m_ExitTransitionEnabled   = serializedObject.FindProperty("m_ExitTransitionEnabled");
            m_ExitTransitionDuration  = serializedObject.FindProperty("m_ExitTransitionDuration");
        }

        /// <summary>
        /// 绘制 Inspector 面板：切换过渡配置 + 中文标签的序列化UI引用字段
        /// </summary>
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();

            DrawTransitionSection();
            EditorGUILayout.Space();
            DrawSerializedFields();

            serializedObject.ApplyModifiedProperties();
        }
        #endregion

        #region 绘制
        //=========================================================================
        // 绘制
        //=========================================================================

        /// <summary>
        /// 绘制进入/退出切换过渡（框式行，启用勾选 + 过渡时间）
        /// </summary>
        private void DrawTransitionSection()
        {
            EditorGUILayout.LabelField("切换过渡（进入/退出）", EditorStyles.boldLabel);

            DrawTransitionRow("进入式-切换过渡", m_EnterTransitionEnabled, m_EnterTransitionDuration);
            DrawTransitionRow("退出式-切换过渡", m_ExitTransitionEnabled, m_ExitTransitionDuration);
        }

        /// <summary>
        /// 绘制一条切换过渡行（勾选未启用时时间禁用）
        /// </summary>
        /// <param name="label">行标签（中文）</param>
        /// <param name="enabled">启用开关属性</param>
        /// <param name="duration">过渡时间属性</param>
        private void DrawTransitionRow(string label, SerializedProperty enabled, SerializedProperty duration)
        {
            EditorGUILayout.BeginHorizontal("box", GUILayout.Width(500));
            {
                enabled.boolValue =
                    EditorGUILayout.ToggleLeft(label, enabled.boolValue, GUILayout.Width(210));

                EditorGUI.BeginDisabledGroup(!enabled.boolValue);
                {
                    GUILayout.FlexibleSpace();
                    EditorGUILayout.LabelField("时间", GUILayout.Width(40));
                    duration.floatValue = EditorGUILayout.FloatField(duration.floatValue, GUILayout.Width(60));
                }
                EditorGUI.EndDisabledGroup();
            }
            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// 以中文标签绘制全部序列化UI引用字段
        /// 说明：截图中的英文命名（Start Button / Progress Slider 等）在此统一替换为中文标签 + Tooltip
        /// </summary>
        private void DrawSerializedFields()
        {
            EditorGUILayout.LabelField("界面引用", EditorStyles.boldLabel);

            // 开始按钮
            EditorGUILayout.PropertyField(m_StartButton,        new GUIContent("开始按钮", "开始游戏按钮（热更模式）"));
            EditorGUILayout.PropertyField(m_StartButtonText,    new GUIContent("开始按钮文字（UGUI）", "开始按钮文字（UGUI Text）"));
            EditorGUILayout.PropertyField(m_StartButtonTextTMP, new GUIContent("开始按钮文字（TMP）", "开始按钮文字（TextMeshProUGUI）"));

            // 重试按钮
            EditorGUILayout.PropertyField(m_RetryButton,        new GUIContent("重试按钮", "重试下载按钮（热更模式）"));
            EditorGUILayout.PropertyField(m_RetryButtonText,    new GUIContent("重试按钮文字（UGUI）", "重试按钮文字（UGUI Text）"));
            EditorGUILayout.PropertyField(m_RetryButtonTextTMP, new GUIContent("重试按钮文字（TMP）", "重试按钮文字（TextMeshProUGUI）"));

            // 关闭按钮
            EditorGUILayout.PropertyField(m_CloseButton,        new GUIContent("关闭按钮", "关闭/退出按钮（热更模式）"));
            EditorGUILayout.PropertyField(m_CloseButtonText,    new GUIContent("关闭按钮文字（UGUI）", "关闭按钮文字（UGUI Text）"));
            EditorGUILayout.PropertyField(m_CloseButtonTextTMP, new GUIContent("关闭按钮文字（TMP）", "关闭按钮文字（TextMeshProUGUI）"));

            // 进度
            EditorGUILayout.PropertyField(m_ProgressSlider,     new GUIContent("进度条", "加载进度条"));
            EditorGUILayout.PropertyField(m_ProgressText,       new GUIContent("进度百分比文字（UGUI）", "进度百分比文本（UGUI Text）"));
            EditorGUILayout.PropertyField(m_ProgressTextTMP,    new GUIContent("进度百分比文字（TMP）", "进度百分比文本（TextMeshProUGUI）"));

            // 字节数
            EditorGUILayout.PropertyField(m_BytesNumText,       new GUIContent("字节数字文本（UGUI）", "下载字节数文本（UGUI Text，热更模式）"));
            EditorGUILayout.PropertyField(m_BytesNumTextTMP,    new GUIContent("字节数字文本（TMP）", "下载字节数文本（TextMeshProUGUI，热更模式）"));

            // 文件数
            EditorGUILayout.PropertyField(m_FileNumText,        new GUIContent("文件数文本（UGUI）", "下载文件数文本（UGUI Text，热更模式）"));
            EditorGUILayout.PropertyField(m_FileNumTextTMP,     new GUIContent("文件数文本（TMP）", "下载文件数文本（TextMeshProUGUI，热更模式）"));

            // 描述
            EditorGUILayout.PropertyField(m_DescText,           new GUIContent("描述文本（UGUI）", "描述文本（UGUI Text）"));
            EditorGUILayout.PropertyField(m_DescTextTMP,        new GUIContent("描述文本（TMP）", "描述文本（TextMeshProUGUI）"));
        }
        #endregion
    }
}
