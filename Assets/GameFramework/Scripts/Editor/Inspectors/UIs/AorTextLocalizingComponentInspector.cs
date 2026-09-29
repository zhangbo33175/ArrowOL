/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Game
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  AorTextLocalizingComponentInspector.cs
 * author:    云毅
 * created:   2026
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
        /// 多语言 Key 名称序列化对象（对应多语言表中的字段）
        /// </summary>
        private SerializedProperty m_KeyNameProperty;

        /// <summary>
        /// 本地化字体标记序列化对象（用于指定使用哪种字体）
        /// </summary>
        private SerializedProperty m_FontMarkProperty;

        /// <summary>
        /// 当前语言序列化对象（用于预览）
        /// </summary>
        private SerializedProperty m_LanguageProperty;
        #endregion

        #region 下拉列表数据
        /// <summary>
        /// 字体标记下拉列表可选项
        /// </summary>
        private string[] m_AvailableMarks;

        /// <summary>
        /// 下拉列表当前选中索引
        /// </summary>
        private int m_SelectedMarkIndex;
        #endregion

        #region 编辑器初始化
        /// <summary>
        /// 编辑器激活时：绑定序列化属性并读取字体配置表
        /// </summary>
        private void OnEnable()
        {
            BindSerializedProperties();
            LoadAvailableFontMarks();
        }

        /// <summary>
        /// 绑定组件上的序列化字段（字段名与运行时组件保持一致）
        /// </summary>
        private void BindSerializedProperties()
        {
            m_KeyNameProperty = serializedObject.FindProperty("m_LocalizingKeyName");
            m_FontMarkProperty = serializedObject.FindProperty("m_LocalizingFontMark");
            m_LanguageProperty = serializedObject.FindProperty("m_Language");
        }

        /// <summary>
        /// 读取 LocalizationFonts.json，构建字体标记下拉列表并定位当前选中项
        /// </summary>
        private void LoadAvailableFontMarks()
        {
            // 读取项目多语言字体配置表 LocalizationFonts.json
            string fontConfigPath = Runtime.GamePathUtils.Json.GetRootDirectoryRelativePath() + "/LocalizationFonts.json";
            string fontConfigText = File.ReadAllText(fontConfigPath);
            JObject fontConfig = JObject.Parse(fontConfigText);

            List<string> markOptions = CollectMarkEntries(fontConfig);
            m_AvailableMarks = markOptions.ToArray();

            // 根据当前已配置的字体标记定位下拉选中项，未命中时回退到第一项
            string currentMark = m_FontMarkProperty.stringValue;
            int selectedIndex = markOptions.FindIndex(mark => mark == currentMark);
            m_SelectedMarkIndex = Mathf.Max(selectedIndex, 0);
        }

        /// <summary>
        /// 从字体配置表第一组分组中顺序收集 Mark0、Mark1… 标记条目
        /// </summary>
        /// <param name="fontConfig">字体配置表 JSON 根对象</param>
        /// <returns>按配置顺序排列的字体标记列表</returns>
        private static List<string> CollectMarkEntries(JObject fontConfig)
        {
            List<string> markOptions = new List<string>();

            // 字体配置表中只有第一组分组内存在 Mark 序列，取其全部条目
            JProperty firstGroup = null;
            foreach (JProperty group in fontConfig.Properties())
            {
                firstGroup = group;
                break;
            }

            if (firstGroup == null)
            {
                return markOptions;
            }

            int markSeq = 0;
            while (firstGroup.Value[$"Mark{markSeq}"] != null)
            {
                markOptions.Add(firstGroup.Value[$"Mark{markSeq}"].ToString());
                markSeq++;
            }

            return markOptions;
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
            EditorGUILayout.LabelField($"展示多语言: {((GameDefinitions.Language)m_LanguageProperty.enumValueIndex).ToString()}");

            // 多语言 Key 输入框
            EditorGUILayout.PropertyField(m_KeyNameProperty, new GUIContent("本地化字段名称"));

            // 字体标记下拉选择框
            m_SelectedMarkIndex = EditorGUILayout.Popup("本地化字体标记", m_SelectedMarkIndex, m_AvailableMarks);
            m_FontMarkProperty.stringValue = m_AvailableMarks[m_SelectedMarkIndex];

            // 提示信息
            EditorGUILayout.HelpBox("此处请填写本地化字体表中的自定义标记，留空表示使用主字体。", MessageType.Info);

            serializedObject.ApplyModifiedProperties();
        }
        #endregion
    }
    #endregion
}
