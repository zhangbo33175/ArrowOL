/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Game
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  AorTextLocalizingComponentInspector.cs
 * author:    云毅
 * created:
 * descrip:   Honor框架 多语言Text组件Inspector扩展
 *            支持多语言Key配置、字体标记下拉选择、实时预览
 ***************************************************************/

using System.Collections.Generic;
using System.IO;
using Honor.Runtime;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;

namespace Honor.Editor.Inspectors.UIs
{
    #region 多语言Text组件编辑器扩展
    /// <summary>
    /// 多语言文本组件（AorTextLocalizing）的编辑器扩展
    /// 功能：在 Inspector 面板可视化配置多语言 Key、字体标记，自动读取字体配置表
    /// 属于：游戏框架 -> 多语言系统 -> 编辑器工具
    /// </summary>
    [CustomEditor(typeof(Runtime.AorTextLocalizing), true)]
    [CanEditMultipleObjects]
    public class AorTextLocalizingComponentInspector : HonorComponentInspector
    {
        #region 序列化字段
        /// <summary>
        /// 多语言 Key 名称（对应多语言表中的字段）
        /// </summary>
        private SerializedProperty m_LocalizingKeyName;

        /// <summary>
        /// 本地化字体标记（用于指定使用哪种字体）
        /// </summary>
        private SerializedProperty m_LocalizingFontMark;

        /// <summary>
        /// 当前语言（用于预览）
        /// </summary>
        private SerializedProperty m_EnableLang;
        #endregion

        #region 下拉列表数据
        /// <summary>
        /// 字体标记下拉列表数据
        /// </summary>
        private string[] m_MarkList;

        /// <summary>
        /// 下拉列表选中索引
        /// </summary>
        private int m_MarkListSelectedIndex;
        #endregion

        #region 编辑器初始化
        /// <summary>
        /// 编辑器激活时：读取配置 + 绑定序列化属性
        /// </summary>
        private void OnEnable()
        {
            // 绑定组件序列化字段
            m_LocalizingKeyName = serializedObject.FindProperty("m_LocalizingKeyName");
            m_LocalizingFontMark = serializedObject.FindProperty("m_LocalizingFontMark");
            m_EnableLang = serializedObject.FindProperty("m_Language");

            // 读取项目多语言字体配置表 LocalizationFonts.json
            string fontConfigPath = Runtime.GamePathUtils.Json.GetRootDirectoryRelativePath() + "/LocalizationFonts.json";
            string localizationFonts = File.ReadAllText(fontConfigPath);
            JObject jsonData = JObject.Parse(localizationFonts);

            // 解析所有字体 Mark 标记，构建下拉列表
            List<string> markList = new List<string>();
            foreach (var data in jsonData)
            {
                int index = 0;
                while (data.Value[$"Mark{index}"] != null)
                {
                    markList.Add(data.Value[$"Mark{index}"].ToString());
                    index++;
                }
                break;
            }

            m_MarkList = markList.ToArray();

            // 设置当前选中项
            m_MarkListSelectedIndex = markList.FindIndex((markName) =>
            {
                return m_LocalizingFontMark.stringValue.Equals(markName);
            });
            m_MarkListSelectedIndex = Mathf.Max(m_MarkListSelectedIndex, 0);
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

            // 显示当前预览语言
            EditorGUILayout.LabelField($"展示多语言: {((GameDefinitions.Language)m_EnableLang.enumValueIndex).ToString()}");

            // 多语言 Key 输入框
            EditorGUILayout.PropertyField(m_LocalizingKeyName, new GUIContent("本地化字段名称"));

            // 字体标记下拉选择框
            m_MarkListSelectedIndex = EditorGUILayout.Popup("本地化字体标记", m_MarkListSelectedIndex, m_MarkList);
            m_LocalizingFontMark.stringValue = m_MarkList[m_MarkListSelectedIndex];

            // 提示信息
            EditorGUILayout.HelpBox("此处请填写本地化字体表中的自定义标记，留空表示使用主字体。", MessageType.Info);

            serializedObject.ApplyModifiedProperties();
        }
        #endregion
    }
    #endregion
}