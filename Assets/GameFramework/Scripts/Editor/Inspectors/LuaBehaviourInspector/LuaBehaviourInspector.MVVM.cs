
using System;
using System.Collections.Generic;
using System.Text;
using Honor.Runtime;
using UnityEditor;
using UnityEngine;

namespace Honor.Editor
{
    internal sealed partial class LuaBehaviourInspector : HonorComponentInspector
    {
        /// <summary>
        /// Lua脚本公共名称
        /// </summary>
        private SerializedProperty m_LuaScriptCommonNameMVVM = null;

        /// <summary>
        /// Lua脚本全称（列表）
        /// </summary>
        private SerializedProperty m_LuaScriptNamesMVVM = null;

        /// <summary>
        /// Lua脚本父类全称（列表）
        /// </summary>
        private SerializedProperty m_LuaSuperScriptNamesMVVM = null;

        /// <summary>
        /// 绑定型数据（数组）
        /// </summary>
        private SerializedProperty m_BindValues = null;

        /// <summary>
        /// 所有绑定数据的注释集合
        /// </summary>
        private List<SerializedProperty> m_InterBindValueComments = null;

        /// <summary>
        /// 所有绑定数据的类型名称集合
        /// </summary>
        private List<SerializedProperty> m_InterBindValueTypeNames = null;

        /// <summary>
        /// 所有绑定数据的名称集合
        /// </summary>
        private List<SerializedProperty> m_InterBindValueNames = null;

        /// <summary>
        /// 所有绑定数据的默认值集合
        /// </summary>
        private List<SerializedProperty> m_InterBindValueVariants = null;

        /// <summary>
        /// 所有绑定数据对应的注入对象集合
        /// </summary>
        private List<SerializedProperty> m_InterBindValueOnInjections = null;

        /// <summary>
        /// 绑定数据当前插入的位置索引
        /// 设置后在刷新界面时将对具体显示列表进行刷新
        /// </summary>
        private int m_InnerBindValueInsertPosIndex = -1;

        /// <summary>
        /// 绑定数据当前删除的位置索引
        /// 设置后在刷新界面时将对具体显示列表进行刷新
        /// </summary>
        private int m_InnerBindValueDeletePosIndex = -1;

        /// <summary>
        /// 绑定数据当前向上移动的原始位置索引
        /// 设置后在刷新界面时将对具体显示列表进行刷新
        /// </summary>
        private int m_InnerBindValueUpwardOriPosIndex = -1;

        /// <summary>
        /// 绑定数据当前向上移动的目标位置索引
        /// 设置后在刷新界面时将对具体显示列表进行刷新
        /// </summary>
        private int m_InnerBindValueUpwardTargetPosIndex = -1;

        /// <summary>
        /// 绑定数据当前向下移动的原始位置索引
        /// 设置后在刷新界面时将对具体显示列表进行刷新
        /// </summary>
        private int m_InnerBindValueDownwardOriPosIndex = -1;

        /// <summary>
        /// 绑定数据当前向下移动的目标位置索引
        /// 设置后在刷新界面时将对具体显示列表进行刷新
        /// </summary>
        private int m_InnerBindValueDownwardTargetPosIndex = -1;

        /// <summary>
        /// 初始化MVVM设计模式
        /// </summary>
        private void InitPatternMVVM()
        {
            m_LuaScriptCommonNameMVVM = serializedObject.FindProperty("m_LuaScriptCommonNameMVVM");
            m_LuaScriptNamesMVVM = serializedObject.FindProperty("m_LuaScriptNamesMVVM");
            if (m_LuaScriptNamesMVVM.arraySize == 0)
            {
                for (int index = 0; index < (int)MVVMPatternType.TotalNum; index++)
                {
                    m_LuaScriptNamesMVVM.InsertArrayElementAtIndex(index);
                    m_LuaScriptNamesMVVM.GetArrayElementAtIndex(index).stringValue = string.Empty;
                }
            }
            m_LuaSuperScriptNamesMVVM = serializedObject.FindProperty("m_LuaSuperScriptNamesMVVM");
            if (m_LuaSuperScriptNamesMVVM.arraySize > (int)MVVMPatternType.TotalNum)
            {
                m_LuaSuperScriptNamesMVVM.ClearArray();
            }
            if (m_LuaSuperScriptNamesMVVM.arraySize == 0)
            {
                for (int index = 0; index < (int)MVVMPatternType.TotalNum; index++)
                {
                    m_LuaSuperScriptNamesMVVM.InsertArrayElementAtIndex(index);
                }
            }
            if (string.IsNullOrEmpty(m_LuaSuperScriptNamesMVVM.GetArrayElementAtIndex((int)MVVMPatternType.View).stringValue))
            {
                m_LuaSuperScriptNamesMVVM.GetArrayElementAtIndex((int)MVVMPatternType.View).stringValue = "ViewSuper";
            }
            if (string.IsNullOrEmpty(m_LuaSuperScriptNamesMVVM.GetArrayElementAtIndex((int)MVVMPatternType.ViewModel).stringValue))
            {
                m_LuaSuperScriptNamesMVVM.GetArrayElementAtIndex((int)MVVMPatternType.ViewModel).stringValue = "ViewModelSuper";
            }

            // 获取绑定数据集合属性
            m_InterBindValueComments = new List<SerializedProperty>();
            m_InterBindValueTypeNames = new List<SerializedProperty>();
            m_InterBindValueNames = new List<SerializedProperty>();
            m_InterBindValueVariants = new List<SerializedProperty>();
            m_InterBindValueOnInjections = new List<SerializedProperty>();

            // 根据绑定数据集合属性进行程序内部的分类分组存放
            m_BindValues = serializedObject.FindProperty("m_BindValues");
            for (int index = 0; index < m_BindValues.arraySize; index++)
            {
                SerializedProperty bindValue = m_BindValues.GetArrayElementAtIndex(index);
                m_InterBindValueComments.Add(bindValue.FindPropertyRelative("Comment"));
                m_InterBindValueTypeNames.Add(bindValue.FindPropertyRelative("BindValueTypeName"));
                m_InterBindValueNames.Add(bindValue.FindPropertyRelative("Name"));
                m_InterBindValueVariants.Add(bindValue.FindPropertyRelative("Variant"));
                m_InterBindValueOnInjections.Add(bindValue.FindPropertyRelative("OnInjections"));
            }

            // 应用变化的属性
            serializedObject.ApplyModifiedProperties();

        }

        /// <summary>
        /// 处理绑定数据的行操作
        /// </summary>
        private void DisposeBindValueLineOperation()
        {
            // 处理中间插入行为的队列变化
            if (m_InnerBindValueInsertPosIndex != -1)
            {
                m_BindValues.InsertArrayElementAtIndex(m_InnerBindValueInsertPosIndex);
                SerializedProperty bindValue = m_BindValues.GetArrayElementAtIndex(m_InnerBindValueInsertPosIndex);
                bindValue.FindPropertyRelative("Comment").stringValue = string.Empty;
                bindValue.FindPropertyRelative("BindValueTypeName").enumValueIndex = (int)LuaBindValue.BindValueType.Int32;
                bindValue.FindPropertyRelative("Name").stringValue = string.Empty;
                bindValue.FindPropertyRelative("Variant").stringValue = string.Empty;
                bindValue.FindPropertyRelative("OnInjections").arraySize = 0;
                OnEnable();
                m_InnerBindValueInsertPosIndex = -1;
            }
            else if (m_InnerBindValueDeletePosIndex != -1) // 处理中间删除行为的队列变化
            {
                m_BindValues.DeleteArrayElementAtIndex(m_InnerBindValueDeletePosIndex);
                OnEnable();
                m_InnerBindValueDeletePosIndex = -1;
            }
            else if (m_InnerBindValueUpwardTargetPosIndex != -1) // 处理向上移动行为的队列变化
            {
                m_BindValues.MoveArrayElement(m_InnerBindValueUpwardOriPosIndex, m_InnerBindValueUpwardTargetPosIndex);
                OnEnable();
                m_InnerBindValueUpwardOriPosIndex = -1;
                m_InnerBindValueUpwardTargetPosIndex = -1;
            }
            else if (m_InnerBindValueDownwardTargetPosIndex != -1) // 处理向下移动行为的队列变化
            {
                m_BindValues.MoveArrayElement(m_InnerBindValueDownwardOriPosIndex, m_InnerBindValueDownwardTargetPosIndex);
                OnEnable();
                m_InnerBindValueDownwardOriPosIndex = -1;
                m_InnerBindValueDownwardTargetPosIndex = -1;
            }

            serializedObject.Update();
        }

        /// <summary>
        /// MVVM设计模式Lua名称GUI
        /// </summary>
        private void OnPatternMVVMLuaScriptNameInspectorGUI()
        {
            if (string.IsNullOrEmpty(m_LuaScriptCommonNameMVVM.stringValue)) GUI.color = Color.red;
            m_LuaScriptCommonNameMVVM.stringValue = EditorGUILayout.TextField("Lua脚本公有名称", m_LuaScriptCommonNameMVVM.stringValue);
            GUI.color = Color.white;

            if (!string.IsNullOrEmpty(m_LuaScriptCommonNameMVVM.stringValue))
            {
                m_LuaScriptNamesMVVM.GetArrayElementAtIndex((int)MVVMPatternType.View).stringValue = AorTxt.Format("{0}View", m_LuaScriptCommonNameMVVM.stringValue);
                m_LuaScriptNamesMVVM.GetArrayElementAtIndex((int)MVVMPatternType.ViewModel).stringValue = AorTxt.Format("{0}ViewModel", m_LuaScriptCommonNameMVVM.stringValue);

                EditorGUILayout.LabelField("Lua-View脚本名称", m_LuaScriptNamesMVVM.GetArrayElementAtIndex((int)MVVMPatternType.View).stringValue);
                EditorGUILayout.LabelField("Lua-ViewModel脚本名称", m_LuaScriptNamesMVVM.GetArrayElementAtIndex((int)MVVMPatternType.ViewModel).stringValue);

                if (string.IsNullOrEmpty(m_LuaSuperScriptNamesMVVM.GetArrayElementAtIndex((int)MVVMPatternType.View).stringValue)) GUI.color = Color.red;
                m_LuaSuperScriptNamesMVVM.GetArrayElementAtIndex((int)MVVMPatternType.View).stringValue = EditorGUILayout.TextField("Lua-View脚本名称（父类）", m_LuaSuperScriptNamesMVVM.GetArrayElementAtIndex((int)MVVMPatternType.View).stringValue);
                GUI.color = Color.white;

                if (string.IsNullOrEmpty(m_LuaSuperScriptNamesMVVM.GetArrayElementAtIndex((int)MVVMPatternType.ViewModel).stringValue)) GUI.color = Color.red;
                m_LuaSuperScriptNamesMVVM.GetArrayElementAtIndex((int)MVVMPatternType.ViewModel).stringValue = EditorGUILayout.TextField("Lua-ViewModel脚本名称（父类）", m_LuaSuperScriptNamesMVVM.GetArrayElementAtIndex((int)MVVMPatternType.ViewModel).stringValue);
                GUI.color = Color.white;
            }

            if (!string.IsNullOrEmpty(m_LuaScriptCommonNameMVVM.stringValue) && !m_LuaScriptCommonNameMVVM.stringValue.StartsWith(".lua") && !m_LuaScriptCommonNameMVVM.stringValue.EndsWith(".lua") &&
                !string.IsNullOrEmpty(m_LuaSuperScriptNamesMVVM.GetArrayElementAtIndex((int)MVVMPatternType.View).stringValue) && !m_LuaSuperScriptNamesMVVM.GetArrayElementAtIndex((int)MVVMPatternType.View).stringValue.StartsWith(".lua") && !m_LuaSuperScriptNamesMVVM.GetArrayElementAtIndex((int)MVVMPatternType.View).stringValue.EndsWith(".lua") &&
                !string.IsNullOrEmpty(m_LuaSuperScriptNamesMVVM.GetArrayElementAtIndex((int)MVVMPatternType.ViewModel).stringValue) && !m_LuaSuperScriptNamesMVVM.GetArrayElementAtIndex((int)MVVMPatternType.ViewModel).stringValue.StartsWith(".lua") && !m_LuaSuperScriptNamesMVVM.GetArrayElementAtIndex((int)MVVMPatternType.ViewModel).stringValue.EndsWith(".lua"))
            {
                m_LuaCanGenerate = true;
            }
        }

        /// <summary>
        /// 展示MVVM设计模式下的绑定数据表头信息
        /// </summary>
        private void OnPatternMVVMBindValueListHeaderInspectorGUI()
        {
            // 绑定数据表表头信息
            GUILayout.BeginHorizontal("box");
            {
                EditorGUILayout.LabelField("绑定数据数量", m_BindValues.arraySize.ToString());
                AddBindValueToListEndButtonInspectorGUI();
                DeleteBindValueFromListEndButtonInspectorGUI();
            }
            GUILayout.EndHorizontal();
        }

        /// <summary>
        /// 绑定数据格式说明GUI
        /// </summary>
        private void OnPatterMVVMBindValueIntroductionsInspectorGUI()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("说明：类型/名称/值内容/选择绑定的注入对象/当前已经选择的绑定对象集合。\n");
            stringBuilder.Append("根据当前已经选择的绑定对象可生成相关注册代码，多个绑定对象使用','隔开。");
            EditorGUILayout.HelpBox(stringBuilder.ToString(), MessageType.Info);
        }

        /// <summary>
        /// 向绑定数据列表末尾添加绑定数据条目按钮
        /// </summary>
        private void AddBindValueToListEndButtonInspectorGUI()
        {
            if (GUILayout.Button("+"))
            {
                m_BindValues.arraySize++;

                SerializedProperty bindValue = m_BindValues.GetArrayElementAtIndex(m_BindValues.arraySize - 1);
                bindValue.FindPropertyRelative("Comment").stringValue = string.Empty;
                bindValue.FindPropertyRelative("BindValueTypeName").enumValueIndex = (int)LuaBindValue.BindValueType.Int32;
                bindValue.FindPropertyRelative("Name").stringValue = string.Empty;
                bindValue.FindPropertyRelative("Variant").stringValue = string.Empty;
                bindValue.FindPropertyRelative("OnInjections").arraySize = 0;

                m_InterBindValueComments.Add(bindValue.FindPropertyRelative("Comment"));
                m_InterBindValueTypeNames.Add(bindValue.FindPropertyRelative("BindValueTypeName"));
                m_InterBindValueNames.Add(bindValue.FindPropertyRelative("Name"));
                m_InterBindValueVariants.Add(bindValue.FindPropertyRelative("Variant"));
                m_InterBindValueOnInjections.Add(bindValue.FindPropertyRelative("OnInjections"));

                serializedObject.ApplyModifiedProperties();
                GUIUtility.ExitGUI();
            }
        }

        /// <summary>
        /// 从绑定数据列表末尾删除绑定数据条目按钮
        /// </summary>
        private void DeleteBindValueFromListEndButtonInspectorGUI()
        {
            if (GUILayout.Button("-"))
            {
                if (m_BindValues.arraySize > 0)
                {
                    m_InterBindValueComments.RemoveAt(m_BindValues.arraySize - 1);
                    m_InterBindValueTypeNames.RemoveAt(m_BindValues.arraySize - 1);
                    m_InterBindValueNames.RemoveAt(m_BindValues.arraySize - 1);
                    m_InterBindValueVariants.RemoveAt(m_BindValues.arraySize - 1);
                    m_InterBindValueOnInjections.RemoveAt(m_BindValues.arraySize - 1);
                    m_BindValues.DeleteArrayElementAtIndex(m_BindValues.arraySize - 1);
                    serializedObject.ApplyModifiedProperties();
                }
                GUIUtility.ExitGUI();
            }
        }

        /// <summary>
        /// 展示MVVM设计模式下的绑定数据列表条目详情
        /// </summary>
        private void OnPatternMVVMBindValueListInspectorGUI()
        {
            GUILayout.BeginVertical("box");
            {
                if (m_BindValues != null && m_BindValues.arraySize > 0)
                {
                    for (int index = 0; index < m_BindValues.arraySize; index++)
                    {
                        GUILayout.BeginVertical(m_BindItemStyle);
                        {
                            GUILayout.BeginHorizontal("box");
                            {
                                GUI.color = Color.cyan;
                                GUILayout.Label($"({index + 1})", new GUILayoutOption[] { GUILayout.Width(30) });
                                GUI.color = Color.white;

                                if (string.IsNullOrEmpty(m_InterBindValueComments[index].stringValue)) GUI.color = Color.red;
                                m_InterBindValueComments[index].stringValue = EditorGUILayout.TextField(m_InterBindValueComments[index].stringValue);
                                GUI.color = Color.white;
                                if (GUILayout.Button("+"))  // 增
                                {
                                    m_InnerBindValueInsertPosIndex = index;
                                    GUIUtility.ExitGUI();
                                }
                                if (GUILayout.Button("-")) // 减
                                {
                                    m_InnerBindValueDeletePosIndex = index;
                                    GUIUtility.ExitGUI();
                                }
                                if (GUILayout.Button('\u25B2'.ToString())) // 上
                                {
                                    m_InnerBindValueUpwardOriPosIndex = index;
                                    m_InnerBindValueUpwardTargetPosIndex = index - 1 < 0 ? 0 : (index - 1);
                                    GUIUtility.ExitGUI();
                                }
                                if (GUILayout.Button('\u25BC'.ToString())) // 下
                                {
                                    m_InnerBindValueDownwardOriPosIndex = index;
                                    m_InnerBindValueDownwardTargetPosIndex = index + 1 >= m_BindValues.arraySize ? (m_BindValues.arraySize - 1) : (index + 1);
                                    GUIUtility.ExitGUI();
                                }
                            }
                            GUILayout.EndHorizontal();

                            GUILayout.BeginHorizontal("box");
                            {
                                int newBindValueTypeNameSelectedIndex = EditorGUILayout.Popup(m_InterBindValueTypeNames[index].enumValueIndex, Enum.GetNames(typeof(LuaBindValue.BindValueType)), new GUILayoutOption[] { GUILayout.Width(70) });
                                if (newBindValueTypeNameSelectedIndex != m_InterBindValueTypeNames[index].enumValueIndex)
                                {
                                    m_InterBindValueVariants[index].stringValue = string.Empty;
                                    m_InterBindValueOnInjections[index].stringValue = string.Empty;
                                }
                                m_InterBindValueTypeNames[index].enumValueIndex = newBindValueTypeNameSelectedIndex;
                                
                                if (string.IsNullOrEmpty(m_InterBindValueNames[index].stringValue)) GUI.color = Color.red;
                                m_InterBindValueNames[index].stringValue = EditorGUILayout.TextField(m_InterBindValueNames[index].stringValue, new GUILayoutOption[] { GUILayout.Width(150) });
                                GUI.color = Color.white;

                                if (m_InterBindValueTypeNames[index].enumValueIndex != (int)LuaBindValue.BindValueType.Any && m_InterBindValueTypeNames[index].enumValueIndex != (int)LuaBindValue.BindValueType.Trigger)
                                {
                                    if (string.IsNullOrEmpty(m_InterBindValueVariants[index].stringValue)) GUI.color = Color.red;
                                    m_InterBindValueVariants[index].stringValue = EditorGUILayout.TextField(m_InterBindValueVariants[index].stringValue, new GUILayoutOption[] { GUILayout.Width(120) });
                                    GUI.color = Color.white;

                                    List<string> interInjectionNames = new List<string>();
                                    interInjectionNames.Add("请选择绑定的注入对象");
                                    // 构建"请选择绑定的注入对象"下拉列表中每类注入对象的绑定方式
                                    for (int i = 0; i < m_InterInjectionNames.Count; i++)
                                    {
                                        interInjectionNames.AddRange(GetBindingInjectionPaths(i));
                                    }
                                    int selectedIndex = EditorGUILayout.Popup(0, interInjectionNames.ToArray(), new GUILayoutOption[] { GUILayout.Width(140) });
                                    if (selectedIndex > 0)
                                    {
                                        List<string> tmpInjectNames = new List<string>(m_InterBindValueOnInjections[index].stringValue.Replace(" ", string.Empty).Split(','));
                                        string info = interInjectionNames[selectedIndex].Replace("/[", "[").Replace('/', '^');
                                        if (!tmpInjectNames.Contains(info))
                                        {
                                            m_InterBindValueOnInjections[index].stringValue += AorTxt.Format("{0}{1}", (string.IsNullOrEmpty(m_InterBindValueOnInjections[index].stringValue) || m_InterBindValueOnInjections[index].stringValue.EndsWith(",")) ? string.Empty : ",", info);
                                        }
                                    }
                                    m_InterBindValueOnInjections[index].stringValue = EditorGUILayout.TextField(m_InterBindValueOnInjections[index].stringValue);
                                }
                            }
                            GUILayout.EndHorizontal();
                        }
                        GUILayout.EndVertical();
                        GUILayout.Space(10);
                    }
                }
            }
            GUILayout.EndVertical();
        }

        /// <summary>
        /// 生成MVVM设计模式下Lua注释行信息
        /// </summary>
        private StringBuilder GeneratePatternMVVMCommentLines(int typeEnumIndex)
        {
            StringBuilder stringBuilder = new StringBuilder();

            string luaScriptName = m_LuaScriptNamesMVVM.GetArrayElementAtIndex(typeEnumIndex).stringValue;
            string luaAuthor = m_LuaAuthorName.stringValue;
            string luaDescript = m_LuaDescript.stringValue;

            stringBuilder.AppendLine(AorTxt.Format("--====================================================================================================="))
                         .AppendLine(AorTxt.Format("-- (c) copyright 2026 - 2030, Honor.Game"))
                         .AppendLine(AorTxt.Format("-- All Rights Reserved."))
                         .AppendLine(AorTxt.Format("-- ----------------------------------------------------------------------------------------------------"))
                         .AppendLine(AorTxt.Format("-- filename:  {0}.lua", luaScriptName))
                         .AppendLine(AorTxt.Format("-- author:    {0}", luaAuthor))
                         .AppendLine(AorTxt.Format("-- descrip:   {0}", AorTxt.Format("{0}{1}", luaDescript, ((MVVMPatternType)typeEnumIndex).ToString())))
                         .AppendLine(AorTxt.Format("--====================================================================================================="))
                         .AppendLine(AorTxt.Format(""));

            return stringBuilder;
        }

        /// <summary>
        /// 生成MVVM设计模式下Lua空行
        /// </summary>
        private StringBuilder GeneratePatternMVVMEmptyCodeLines(int typeEnumIndex)
        {
            StringBuilder stringBuilder = new StringBuilder();

            string luaScriptName = m_LuaScriptNamesMVVM.GetArrayElementAtIndex(typeEnumIndex).stringValue;
            string luaSuperScriptName = m_LuaSuperScriptNamesMVVM.GetArrayElementAtIndex(typeEnumIndex).stringValue;
            string luaName = luaScriptName;

            stringBuilder.AppendLine(AorTxt.Format("---@class {0} : {1}", luaName, luaSuperScriptName));
            if ((MVVMPatternType)typeEnumIndex == MVVMPatternType.View)
            {
                stringBuilder.AppendLine(AorTxt.Format("---@field cs Honor.Runtime.LuaBehaviour @LuaBehaviour"));
            }

            // 采集InfoEx辅助信息
            List<string> luaInjectNames = null;
            List<string> luaInjectComments = null;
            List<string> luaInjectFunctionNames = null;
            List<string> luaInjectFunctionParams = null;
            List<string> luaInjectCmds = null;
            CollectInfoExInfos(out luaInjectNames, out luaInjectComments, out luaInjectFunctionNames, out luaInjectFunctionParams, out luaInjectCmds);

            // 采集绑定数据与注入对象之间的各类信息
            SortedDictionary<string, List<string>> luaBindValueOnInjectionNames = null;
            SortedDictionary<string, List<string>> luaBindValueOnInjectionWays = null;
            SortedDictionary<string, List<string>> luaBindValueOnInjectionSides = null;
            SortedDictionary<string, string> luaBindValueComments = null;
            SortedDictionary<string, string> luaBindValueFunctionNames = null;
            SortedDictionary<string, string> luaBindValueTypeNames = null;
            CollectBindingInfosOnInjections(out luaBindValueOnInjectionNames, out luaBindValueOnInjectionWays, out luaBindValueOnInjectionSides, out luaBindValueComments, out luaBindValueFunctionNames, out luaBindValueTypeNames);

            if ((MVVMPatternType)typeEnumIndex == MVVMPatternType.View)
            {
                if (m_Injections != null && m_Injections.arraySize > 0)
                {
                    for (int index = 0; index < m_Injections.arraySize; index++)
                    {
                        string comment = !string.IsNullOrEmpty(m_InterInjectionComments[index].stringValue) ? m_InterInjectionComments[index].stringValue : string.Empty;
                        string fieldName = m_InterInjectionNames[index].stringValue;
                        string typeName = string.Empty;
                        string isValid = string.Empty;
                        string infoEx = string.Empty;
                        if(m_InterInjectionIsArrays[index].boolValue)
                        {
                            typeName = $"{LuaInjection.LuaInjectionType[(int)m_InterInjectionTypeNames[index].enumValueIndex]}[]";
                            isValid = "√";
                            infoEx = string.Empty;
                            for (int elementIndex = 0; elementIndex < m_InterInjectionElementsObjs[index].arraySize; elementIndex++)
                            {
                                if(m_InterInjectionElementsObjs[index].GetArrayElementAtIndex(elementIndex).objectReferenceValue == null)
                                {
                                    isValid = "×";
                                    infoEx = string.Empty;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            typeName = LuaInjection.LuaInjectionType[(int)m_InterInjectionTypeNames[index].enumValueIndex];
                            isValid = (m_InterInjectionTypeNames[index].enumValueIndex < (int)LuaInjection.InjectionType.Int32 || m_InterInjectionTypeNames[index].enumValueIndex > (int)LuaInjection.InjectionType.Boolean) ? (m_InterInjectionObjs[index].objectReferenceValue != null ? "√" : "×") : (string.IsNullOrEmpty(m_InterInjectionVariants[index].stringValue) ? "×" : m_InterInjectionVariants[index].stringValue);
                            infoEx = m_InterInjectionInfoExs[index].stringValue;
                        }

                        if (typeName.Equals("UnityEngine.GameObject") && !string.IsNullOrEmpty(infoEx))
                        {
                            var findTypeName = Assembly.GetType(infoEx);
                            typeName = findTypeName == null ? "any" : findTypeName.FullName;
                        }
                        else if (typeName.Equals("Honor.Runtime.LuaBehaviour") && !string.IsNullOrEmpty(infoEx))
                        {
                            typeName = infoEx;
                        }

                        while (fieldName.Length < 35) fieldName += " ";
                        while (typeName.Length < 30) typeName += " ";
                        while (isValid.Length < 10) isValid += " ";
                        while (infoEx.Length < 15) infoEx += " ";

                        stringBuilder.AppendLine(AorTxt.Format("---@field {0}{1}{2}{3}{4}", fieldName, typeName, isValid, infoEx, comment));
                    }
                }
            }

            stringBuilder.AppendLine(AorTxt.Format("local {0} = class('{1}', import('{2}'))", luaName, luaName, luaSuperScriptName));
            stringBuilder.AppendLine(AorTxt.Format(""));

            stringBuilder.AppendLine(AorTxt.Format("---绑定值Key名称"));
            stringBuilder.AppendLine(AorTxt.Format("---@class {0}.BVKey", luaName));
            string bvkeyContent = "{";
            foreach (var bindValueName in m_InterBindValueNames)
            {
                bvkeyContent = AorTxt.Format("{0}{1} = \"{2}\", ", bvkeyContent, bindValueName.stringValue, bindValueName.stringValue);
            }
            bvkeyContent += "}";
            stringBuilder.AppendLine(AorTxt.Format("{0}.BVKey = {1}", luaName, bvkeyContent));
            stringBuilder.AppendLine(AorTxt.Format(""));

            stringBuilder.AppendLine(AorTxt.Format("---构造函数"));
            stringBuilder.AppendLine(AorTxt.Format("---@type fun(args:table):void"));
            stringBuilder.AppendLine(AorTxt.Format("---@param args table @自定义参数"));
            stringBuilder.AppendLine(AorTxt.Format("function {0}:ctor(args)", luaName));
            stringBuilder.AppendLine(AorTxt.Format("    {0}.super.ctor(self, args)", luaName));
            if ((MVVMPatternType)typeEnumIndex == MVVMPatternType.ViewModel)
            {
                stringBuilder.AppendLine("-- 2>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
                if(m_InterBindValueNames.Count > 0)
                {
                    for (int index = 0; index < m_InterBindValueNames.Count; index++)
                    {
                        if (!string.IsNullOrEmpty(m_InterBindValueComments[index].stringValue))
                        {
                            stringBuilder.AppendLine(AorTxt.Format("    -- 添加绑定数据成员：{0}，数据类型：{1}", m_InterBindValueComments[index].stringValue, GetBindValueTypeByName(m_InterBindValueNames[index].stringValue)));
                        }

                        if (string.IsNullOrEmpty(m_InterBindValueVariants[index].stringValue))
                        {                           
                            if ((LuaBindValue.BindValueType)m_InterBindValueTypeNames[index].enumValueIndex == LuaBindValue.BindValueType.Trigger)
                            {
                                stringBuilder.AppendLine(AorTxt.Format("    self:AddBindValue(self.BVKey.{0}, false, true)", m_InterBindValueNames[index].stringValue));
                            }
                            else
                            {
                                stringBuilder.AppendLine(AorTxt.Format("    self:AddBindValue(self.BVKey.{0})", m_InterBindValueNames[index].stringValue));
                            }
                        }
                        else
                        {
                            switch ((LuaBindValue.BindValueType)m_InterBindValueTypeNames[index].enumValueIndex)
                            {
                                case LuaBindValue.BindValueType.Int32: stringBuilder.AppendLine(AorTxt.Format("    self:AddBindValue(self.BVKey.{0}, tonumber(self._args.env.cs.BindValues[{1}].Variant))", m_InterBindValueNames[index].stringValue, index)); break;
                                case LuaBindValue.BindValueType.Float: stringBuilder.AppendLine(AorTxt.Format("    self:AddBindValue(self.BVKey.{0}, tonumber(self._args.env.cs.BindValues[{1}].Variant))", m_InterBindValueNames[index].stringValue, index)); break;
                                case LuaBindValue.BindValueType.String: stringBuilder.AppendLine(AorTxt.Format("    self:AddBindValue(self.BVKey.{0}, self._args.env.cs.BindValues[{1}].Variant)", m_InterBindValueNames[index].stringValue, index)); break;
                                case LuaBindValue.BindValueType.Boolean: stringBuilder.AppendLine(AorTxt.Format("    self:AddBindValue(self.BVKey.{0}, self._args.env.cs.BindValues[{1}].Variant == 'true')", m_InterBindValueNames[index].stringValue, index)); break;
                                case LuaBindValue.BindValueType.Table: stringBuilder.AppendLine(AorTxt.Format("    self:AddBindValue(self.BVKey.{0}, TableDecode(self._args.env.cs.BindValues[{1}].Variant))", m_InterBindValueNames[index].stringValue, index)); break;
                                case LuaBindValue.BindValueType.Array: stringBuilder.AppendLine(AorTxt.Format("    self:AddBindValue(self.BVKey.{0}, TableDecode(self._args.env.cs.BindValues[{1}].Variant))", m_InterBindValueNames[index].stringValue, index)); break;
                                case LuaBindValue.BindValueType.Any: stringBuilder.AppendLine(AorTxt.Format("    self:AddBindValue(self.BVKey.{0})", m_InterBindValueNames[index].stringValue)); break;
                                case LuaBindValue.BindValueType.Trigger: stringBuilder.AppendLine(AorTxt.Format("    self:AddBindValue(self.BVKey.{0}, false, true)", m_InterBindValueNames[index].stringValue)); break;
                            }
                        }
                    }
                }
                else
                {
                    stringBuilder.AppendLine(AorTxt.Format("    -- 无添加绑定数据成员"));
                }
                stringBuilder.AppendLine("-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<2");
            }
            stringBuilder.AppendLine(AorTxt.Format("end"));
            stringBuilder.AppendLine(AorTxt.Format(""));
            stringBuilder.AppendLine(AorTxt.Format("---创建函数"));
            stringBuilder.AppendLine(AorTxt.Format("---@type fun(args:table):{0}", luaName));
            stringBuilder.AppendLine(AorTxt.Format("---@param args table @自定义参数"));
            stringBuilder.AppendLine(AorTxt.Format("---@return {0} @{1}实例", luaName, luaName));
            stringBuilder.AppendLine(AorTxt.Format("function {0}:Create(args)", luaName));
            stringBuilder.AppendLine(AorTxt.Format("    local obj = {0}.new(args)", luaName));
            stringBuilder.AppendLine(AorTxt.Format("    return obj"));
            stringBuilder.AppendLine(AorTxt.Format("end"));
            stringBuilder.AppendLine(AorTxt.Format(""));

            if ((MVVMPatternType)typeEnumIndex == MVVMPatternType.View)
            {
                stringBuilder.AppendLine(AorTxt.Format("---唤醒"));
                stringBuilder.AppendLine(AorTxt.Format("---@type fun():void"));
                stringBuilder.AppendLine(AorTxt.Format("function {0}:Awake()", luaName));
                stringBuilder.AppendLine(AorTxt.Format("    {0}.super.Awake(self)", luaName));
                stringBuilder.AppendLine("-- 2>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
                if (luaInjectFunctionNames.Count > 0)
                {
                    for (int index = 0; index < luaInjectFunctionNames.Count; index++)
                    {
                        stringBuilder.AppendLine(AorTxt.Format("    AddUIListenerFunction(self.{0}, '{1}', handler(self, self.{2}))", luaInjectNames[index], luaInjectCmds[index], luaInjectFunctionNames[index]));
                    }
                }
                else
                {
                    stringBuilder.AppendLine(AorTxt.Format("-- 无自动注册内容。"));
                }
                stringBuilder.AppendLine("-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<2");
                stringBuilder.AppendLine(AorTxt.Format(""))
                             .AppendLine(AorTxt.Format("end"))
                             .AppendLine(AorTxt.Format(""));

                stringBuilder.AppendLine(AorTxt.Format("---初始化回调（在Awake中ViewSuper首次调用SetViewModel时触发，该方法执行后才会调用为View设置ViewModel，因此请勿在该方法中调用GetViewModel。）"))
                             .AppendLine(AorTxt.Format("---@type fun(viewModel:{0}):void", m_LuaScriptNamesMVVM.GetArrayElementAtIndex((int)MVVMPatternType.ViewModel).stringValue))
                             .AppendLine(AorTxt.Format("---@param viewModel {0} @ViewModel", m_LuaScriptNamesMVVM.GetArrayElementAtIndex((int)MVVMPatternType.ViewModel).stringValue))
                             .AppendLine(AorTxt.Format("function {0}:OnInit(viewModel)", luaName))
                             .AppendLine(AorTxt.Format("    {0}.super.OnInit(self, viewModel)", luaName))
                             .AppendLine("-- 4>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
                if (m_InterBindValueNames.Count > 0)
                {
                    for (int index = 0; index < m_InterBindValueNames.Count; index++)
                    {
                        if (!string.IsNullOrEmpty(m_InterBindValueComments[index].stringValue))
                        {
                            stringBuilder.AppendLine(AorTxt.Format("    -- 添加数据变化监听：{0}，数据类型：{1}", m_InterBindValueComments[index].stringValue, GetBindValueTypeByName(m_InterBindValueNames[index].stringValue)));
                        }
                        stringBuilder.AppendLine(AorTxt.Format("    self:AddBindHandler(self.BVKey.{0}, handler(self, self.{1}))", m_InterBindValueNames[index].stringValue, luaBindValueFunctionNames[m_InterBindValueNames[index].stringValue]));
                    }
                }
                else
                {
                    stringBuilder.AppendLine(AorTxt.Format("    -- 无数据变化监听"));
                }
                stringBuilder.AppendLine("-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<4")
                             .AppendLine(AorTxt.Format("end"))
                             .AppendLine(AorTxt.Format(""));

                stringBuilder.AppendLine(AorTxt.Format("---开始"))
                             .AppendLine(AorTxt.Format("---@type fun():void"))
                             .AppendLine(AorTxt.Format("function {0}:Start()", luaName))
                             .AppendLine(AorTxt.Format("    {0}.super.Start(self)", luaName))
                             .AppendLine(AorTxt.Format(""))
                             .AppendLine(AorTxt.Format("end"))
                             .AppendLine(AorTxt.Format(""));
                if (m_UseProc.boolValue)
                {
                    stringBuilder.AppendLine(AorTxt.Format("---心跳（自定义）"))
                                    .AppendLine(AorTxt.Format("---@type fun():void"))
                                    .AppendLine(AorTxt.Format("function {0}:Proc()", luaName))
                                    .AppendLine(AorTxt.Format("    {0}.super.Proc(self)", luaName))
                                    .AppendLine(AorTxt.Format(""))
                                    .AppendLine(AorTxt.Format("end"))
                                    .AppendLine(AorTxt.Format(""));
                }
                stringBuilder.AppendLine(AorTxt.Format("---销毁"))
                             .AppendLine(AorTxt.Format("---@type fun():void"))
                             .AppendLine(AorTxt.Format("function {0}:OnDestroy()", luaName))
                             .AppendLine(AorTxt.Format("    {0}.super.OnDestroy(self)", luaName));
                stringBuilder.AppendLine(AorTxt.Format("-- 3>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>"));
                if (luaInjectFunctionNames.Count > 0)
                {
                    for (int index = 0; index < luaInjectFunctionNames.Count; index++)
                    {
                        stringBuilder.AppendLine(AorTxt.Format("    OnRemoveListener(self.{0}, '{1}', handler(self, self.{2}))", luaInjectNames[index], luaInjectCmds[index], luaInjectFunctionNames[index]));
                    }
                }
                else
                {
                    stringBuilder.AppendLine(AorTxt.Format("-- 无自动注销内容。"));
                }
                stringBuilder.AppendLine(AorTxt.Format("-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<3"));

                stringBuilder.AppendLine(AorTxt.Format(""));
                stringBuilder.AppendLine(AorTxt.Format("end"));
                stringBuilder.AppendLine(AorTxt.Format(""));

                stringBuilder.AppendLine(AorTxt.Format("---注册监听代码处（注：所有成员函数的注册监听代码都写在这里，成员函数作为监听回调时不需要手动注销，父类会自动辅助完成事件的注销操作。）"))
                             .AppendLine(AorTxt.Format("---@type fun():void"))
                             .AppendLine(AorTxt.Format("function {0}:OnAddListeners()", luaName))
                             .AppendLine(AorTxt.Format("    {0}.super.OnAddListeners(self)", luaName))
                             .AppendLine(AorTxt.Format(""))
                             .AppendLine(AorTxt.Format("end"))
                             .AppendLine(AorTxt.Format(""));

                stringBuilder.AppendLine(AorTxt.Format("---获取ViewModel"))
                             .AppendLine(AorTxt.Format("---@type fun():{0}Model", luaName))
                             .AppendLine(AorTxt.Format("---@return {0}Model @ViewModel", luaName))
                             .AppendLine(AorTxt.Format("function {0}:GetViewModel()", luaName))
                             .AppendLine(AorTxt.Format("    return {0}.super.GetViewModel(self)", luaName))
                             .AppendLine(AorTxt.Format("end"))
                             .AppendLine(AorTxt.Format(""));

                stringBuilder.AppendLine(AorTxt.Format("---被追加的N层UI子节点销毁回调"))
                             .AppendLine(AorTxt.Format("---@type fun(luaClass: XLua.LuaTable):void"))
                             .AppendLine(AorTxt.Format("---@param luaClass XLua.LuaTable @luaClass"))
                             .AppendLine(AorTxt.Format("function {0}:OnAddedUIDestroyed(luaClass)", luaName))
                             .AppendLine(AorTxt.Format("    {0}.super.OnAddedUIDestroyed(self, luaClass)", luaName))
                             .AppendLine(AorTxt.Format("end"))
                             .AppendLine(AorTxt.Format(""));

                // Collider2D/3D碰撞器生命周期函数
                if (m_UseCollider2DLifeCycles.boolValue)
                {
                    stringBuilder.AppendLine(AorTxt.Format("---进入碰撞2D"))
                                 .AppendLine(AorTxt.Format("---@type fun(collision2D:UnityEngine.Collision2D):void"))
                                 .AppendLine(AorTxt.Format("---@param collision2D UnityEngine.Collision2D @碰撞2D"))
                                 .AppendLine(AorTxt.Format("function {0}:OnCollisionEnter2D(collision2D)", luaName))
                                 .AppendLine(AorTxt.Format("end"))
                                 .AppendLine(AorTxt.Format(""));
                    stringBuilder.AppendLine(AorTxt.Format("---停留碰撞2D"))
                                 .AppendLine(AorTxt.Format("---@type fun(collision2D:UnityEngine.Collision2D):void"))
                                 .AppendLine(AorTxt.Format("---@param collision2D UnityEngine.Collision2D @碰撞2D"))
                                 .AppendLine(AorTxt.Format("function {0}:OnCollisionStay2D(collision2D)", luaName))
                                 .AppendLine(AorTxt.Format("end"))
                                 .AppendLine(AorTxt.Format(""));
                    stringBuilder.AppendLine(AorTxt.Format("---退出碰撞2D"))
                                 .AppendLine(AorTxt.Format("---@type fun(collision2D:UnityEngine.Collision2D):void"))
                                 .AppendLine(AorTxt.Format("---@param collision2D UnityEngine.Collision2D @碰撞2D"))
                                 .AppendLine(AorTxt.Format("function {0}:OnCollisionExit2D(collision2D)", luaName))
                                 .AppendLine(AorTxt.Format("end"))
                                 .AppendLine(AorTxt.Format(""));
                }
                if (m_UseCollider3DLifeCycles.boolValue)
                {
                    stringBuilder.AppendLine(AorTxt.Format("---进入碰撞3D"))
                                 .AppendLine(AorTxt.Format("---@type fun(collision3D:UnityEngine.Collision):void"))
                                 .AppendLine(AorTxt.Format("---@param collision3D UnityEngine.Collision @碰撞3D"))
                                 .AppendLine(AorTxt.Format("function {0}:OnCollisionEnter3D(collision3D)", luaName))
                                 .AppendLine(AorTxt.Format("end"))
                                 .AppendLine(AorTxt.Format(""));
                    stringBuilder.AppendLine(AorTxt.Format("---停留碰撞3D"))
                                 .AppendLine(AorTxt.Format("---@type fun(collision3D:UnityEngine.Collision):void"))
                                 .AppendLine(AorTxt.Format("---@param collision3D UnityEngine.Collision @碰撞3D"))
                                 .AppendLine(AorTxt.Format("function {0}:OnCollisionStay3D(collision3D)", luaName))
                                 .AppendLine(AorTxt.Format("end"))
                                 .AppendLine(AorTxt.Format(""));
                    stringBuilder.AppendLine(AorTxt.Format("---退出碰撞3D"))
                                 .AppendLine(AorTxt.Format("---@type fun(collision3D:UnityEngine.Collision):void"))
                                 .AppendLine(AorTxt.Format("---@param collision3D UnityEngine.Collision @碰撞3D"))
                                 .AppendLine(AorTxt.Format("function {0}:OnCollisionExit3D(collision3D)", luaName))
                                 .AppendLine(AorTxt.Format("end"))
                                 .AppendLine(AorTxt.Format(""));
                }

                // Trigger2D/3D触发器生命周期函数
                if (m_UseTrigger2DLifeCycles.boolValue)
                {
                    stringBuilder.AppendLine(AorTxt.Format("---进入触发2D"))
                                 .AppendLine(AorTxt.Format("---@type fun(other2D:UnityEngine.Collider2D):void"))
                                 .AppendLine(AorTxt.Format("---@param other2D UnityEngine.Collider2D @对方碰撞器2D"))
                                 .AppendLine(AorTxt.Format("function {0}:OnTriggerEnter2D(other2D)", luaName))
                                 .AppendLine(AorTxt.Format("end"))
                                 .AppendLine(AorTxt.Format(""));
                    stringBuilder.AppendLine(AorTxt.Format("---停留触发2D"))
                                 .AppendLine(AorTxt.Format("---@type fun(other2D:UnityEngine.Collider2D):void"))
                                 .AppendLine(AorTxt.Format("---@param other2D UnityEngine.Collider2D @对方碰撞器2D"))
                                 .AppendLine(AorTxt.Format("function {0}:OnTriggerStay2D(other2D)", luaName))
                                 .AppendLine(AorTxt.Format("end"))
                                 .AppendLine(AorTxt.Format(""));
                    stringBuilder.AppendLine(AorTxt.Format("---退出触发2D"))
                                 .AppendLine(AorTxt.Format("---@type fun(other2D:UnityEngine.Collider2D):void"))
                                 .AppendLine(AorTxt.Format("---@param other2D UnityEngine.Collider2D @对方碰撞器2D"))
                                 .AppendLine(AorTxt.Format("function {0}:OnTriggerExit2D(other2D)", luaName))
                                 .AppendLine(AorTxt.Format("end"))
                                 .AppendLine(AorTxt.Format(""));
                }
                if (m_UseTrigger3DLifeCycles.boolValue)
                {
                    stringBuilder.AppendLine(AorTxt.Format("---进入触发3D"))
                                 .AppendLine(AorTxt.Format("---@type fun(other3D:UnityEngine.Collider):void"))
                                 .AppendLine(AorTxt.Format("---@param other3D UnityEngine.Collider @对方碰撞器3D"))
                                 .AppendLine(AorTxt.Format("function {0}:OnTriggerEnter3D(other3D)", luaName))
                                 .AppendLine(AorTxt.Format("end"))
                                 .AppendLine(AorTxt.Format(""));
                    stringBuilder.AppendLine(AorTxt.Format("---停留触发3D"))
                                 .AppendLine(AorTxt.Format("---@type fun(other3D:UnityEngine.Collider):void"))
                                 .AppendLine(AorTxt.Format("---@param other3D UnityEngine.Collider @对方碰撞器3D"))
                                 .AppendLine(AorTxt.Format("function {0}:OnTriggerStay3D(other3D)", luaName))
                                 .AppendLine(AorTxt.Format("end"))
                                 .AppendLine(AorTxt.Format(""));
                    stringBuilder.AppendLine(AorTxt.Format("---退出触发3D"))
                                 .AppendLine(AorTxt.Format("---@type fun(other3D:UnityEngine.Collider):void"))
                                 .AppendLine(AorTxt.Format("---@param other3D UnityEngine.Collider @对方碰撞器3D"))
                                 .AppendLine(AorTxt.Format("function {0}:OnTriggerExit3D(other3D)", luaName))
                                 .AppendLine(AorTxt.Format("end"))
                                 .AppendLine(AorTxt.Format(""));
                }

                // UI交互监听
                stringBuilder.AppendLine(AorTxt.Format("-- 5>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>"));
                stringBuilder.AppendLine(AorTxt.Format(""));
                for (int index = 0; index < luaInjectFunctionNames.Count; index++)
                {
                    bool hasContained = false;
                    for (int checkIdx = 0; checkIdx < index; checkIdx++)
                    {
                        if (luaInjectFunctionNames[checkIdx] == luaInjectFunctionNames[index])
                        {
                            hasContained = true;
                            break;
                        }
                    }
                    if (hasContained)
                    {
                        continue;
                    }

                    string[] paramsArray = luaInjectFunctionParams[index].Replace(" ", string.Empty).Split(',');
                    string funcDesc = string.Empty;
                    string funcParams = string.Empty;
                    foreach (var param in paramsArray)
                    {
                        funcDesc += ((string.IsNullOrEmpty(funcDesc) ? "" : ", ") + param + ":any");
                        funcParams += ((string.IsNullOrEmpty(funcParams) ? "" : ", ") + param);
                    }
                    stringBuilder.AppendLine(AorTxt.Format("---{0}", luaInjectComments[index]))
                                 .AppendLine(AorTxt.Format("---@type fun({0}):void", funcDesc));
                    if (!string.IsNullOrEmpty(luaInjectFunctionParams[index]))
                    {
                        foreach(var param in paramsArray)
                        {
                            stringBuilder.AppendLine(AorTxt.Format("---@param {0} any @UI交互内容", param));
                        }
                    }
                    stringBuilder.AppendLine(AorTxt.Format("function {0}:{1}({2})", luaName, luaInjectFunctionNames[index], funcParams));
                    foreach (var injectionName in luaBindValueOnInjectionNames)
                    {
                        if (injectionName.Value.Contains(luaInjectNames[index]))
                        {
                            List<int> indexes = new List<int>();
                            for (int i = 0; i < injectionName.Value.Count; i++)
                            {
                                if (injectionName.Value[i] == luaInjectNames[index])
                                {
                                    indexes.Add(i);
                                }
                            }
                            for (int i = 0; i < indexes.Count; i++)
                            {
                                // 只有双向绑定的数据才会在UI交互监听回调中对相关的绑定数据进行赋值
                                string side = luaBindValueOnInjectionSides[injectionName.Key][indexes[i]];
                                if (side == "Two")
                                {
                                    string value = AorTxt.Format((luaBindValueTypeNames[injectionName.Key] == "table" || luaBindValueTypeNames[injectionName.Key] == "array") ? "TableDecode({0})" : "{0}", luaInjectFunctionParams[index]);
                                    if (!string.IsNullOrEmpty(value))
                                    {
                                        stringBuilder.AppendLine(AorTxt.Format("    self:GetViewModel():SetBindValue(self.BVKey.{0}, {1})", injectionName.Key, value));
                                    }
                                }
                            }
                        }
                    }

                    if(luaInjectFunctionNames[index].EndsWith("GettingItem"))
                    {
                        stringBuilder.AppendLine(AorTxt.Format($"    if itemIndex < 0 or itemIndex >= self.{luaInjectNames[index]}.MaxItemNum then return nil end"));
                        stringBuilder.AppendLine(AorTxt.Format($"    local item = self.{luaInjectNames[index]}:NewListViewItem('Item')"));
                        stringBuilder.AppendLine(AorTxt.Format($"    if item.IsInitHandlerCalled == false then item.IsInitHandlerCalled = true end"));
                        stringBuilder.AppendLine(AorTxt.Format($"    return item"));
                    }

                    stringBuilder.AppendLine(AorTxt.Format("end"));
                    stringBuilder.AppendLine(AorTxt.Format(""));
                }
                stringBuilder.AppendLine(AorTxt.Format("-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<5"));
                stringBuilder.AppendLine(AorTxt.Format(""));

                // 绑定数据变化监听
                stringBuilder.AppendLine(AorTxt.Format("-- 6>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>"));
                stringBuilder.AppendLine(AorTxt.Format(""));
                foreach (var luaBindValueFunctionName in luaBindValueFunctionNames)
                {
                    string bindValueName = luaBindValueFunctionName.Key;
                    if (luaBindValueTypeNames[bindValueName] == "trigger")
                    {
                        stringBuilder.AppendLine(AorTxt.Format("---{0}", luaBindValueComments[bindValueName]));
                        stringBuilder.AppendLine(AorTxt.Format("---@type fun():void"))
                                     .AppendLine(AorTxt.Format("function {0}:{1}()", luaName, luaBindValueFunctionName.Value));
                    }
                    else
                    {
                        stringBuilder.AppendLine(AorTxt.Format("---{0}", luaBindValueComments[bindValueName]));
                        stringBuilder.AppendLine(AorTxt.Format("---@type fun(old:{0}, new:{1}, operate:string, valueItem:any):void", luaBindValueTypeNames[bindValueName] == "array" ? "table" : luaBindValueTypeNames[bindValueName], luaBindValueTypeNames[bindValueName] == "array" ? "table" : luaBindValueTypeNames[bindValueName]))
                                     .AppendLine(AorTxt.Format("---@param old {0} @旧值内容", luaBindValueTypeNames[bindValueName] == "array" ? "table" : luaBindValueTypeNames[bindValueName]))
                                     .AppendLine(AorTxt.Format("---@param new {0} @新值内容", luaBindValueTypeNames[bindValueName] == "array" ? "table" : luaBindValueTypeNames[bindValueName]))
                                     .AppendLine(AorTxt.Format("---@param operate string @绑定数据操作类型"))
                                     .AppendLine(AorTxt.Format("---@param valueItem any @动态数据集合Item值内容"))
                                     .AppendLine(AorTxt.Format("function {0}:{1}(old, new, operate, valueItem)", luaName, luaBindValueFunctionName.Value));
                        // 对当前bindValue所绑定的所有ui对象进行ui指定绑定方式的更新
                        for (int checkIndex = 0; checkIndex < luaBindValueOnInjectionNames[bindValueName].Count; checkIndex++)
                        {
                            string bindedInjectionName = luaBindValueOnInjectionNames[bindValueName][checkIndex];
                            string bindedInjectionWay = luaBindValueOnInjectionWays[bindValueName][checkIndex];
                            if (bindedInjectionWay == "Active")
                            {
                                stringBuilder.AppendLine(AorTxt.Format("    self.{0}.gameObject:SetActive(new)", bindedInjectionName));
                            }
                            else if (bindedInjectionWay == "Interactable")
                            {
                                stringBuilder.AppendLine(AorTxt.Format("    self.{0}.interactable = new", bindedInjectionName));
                            }
                            else if (bindedInjectionWay == "Content")
                            {
                                string injectionTypeName = GetInjectionTypeByName(bindedInjectionName);
                                switch (injectionTypeName)
                                {
                                    case "UnityEngine.UI.Text": stringBuilder.AppendLine(AorTxt.Format("    self.{0}.text = {1}", bindedInjectionName, (luaBindValueTypeNames[bindValueName] == "table" || luaBindValueTypeNames[bindValueName] == "array") ? "TableEncode(new)" : "tostring(new)")); break;
                                    case "Honor.Runtime.TextPicMixed": stringBuilder.AppendLine(AorTxt.Format("    self.{0}.text = {1}", bindedInjectionName, (luaBindValueTypeNames[bindValueName] == "table" || luaBindValueTypeNames[bindValueName] == "array") ? "TableEncode(new)" : "tostring(new)")); break;
                                    case "TMPro.TextMeshProUGUI": stringBuilder.AppendLine(AorTxt.Format("    self.{0}.text = {1}", bindedInjectionName, (luaBindValueTypeNames[bindValueName] == "table" || luaBindValueTypeNames[bindValueName] == "array") ? "TableEncode(new)" : "tostring(new)")); break;
                                    case "UnityEngine.UI.Toggle": stringBuilder.AppendLine(AorTxt.Format("    self.{0}.isOn = new", bindedInjectionName)); break;
                                    case "UnityEngine.UI.Slider": stringBuilder.AppendLine(AorTxt.Format("    self.{0}.value = new", bindedInjectionName)); break;
                                    case "UnityEngine.UI.Dropdown": stringBuilder.AppendLine(AorTxt.Format("    self.{0}.value = new", bindedInjectionName)); break;
                                    case "UnityEngine.UI.InputField": stringBuilder.AppendLine(AorTxt.Format("    self.{0}.text = {1}", bindedInjectionName, (luaBindValueTypeNames[bindValueName] == "table" || luaBindValueTypeNames[bindValueName] == "array") ? "TableEncode(new)" : "tostring(new)")); break;
                                    case "Honor.Runtime.SwitchButton": stringBuilder.AppendLine(AorTxt.Format("    self.{0}.isOn = new", bindedInjectionName)); break;
                                }
                            }
                            else if (bindedInjectionWay == "anchoredPosition")
                            {
                                stringBuilder.AppendLine(AorTxt.Format("    if type(new) == \"table\" and new.x ~= nil and new.y ~= nil then"));
                                stringBuilder.AppendLine(AorTxt.Format("        self.{0}:rectTransform().anchoredPosition = Unity.Vector2(new.x, new.y)", bindedInjectionName));
                                stringBuilder.AppendLine(AorTxt.Format("    end"));
                            }
                            else if (bindedInjectionWay == "anchoredPosition3D")
                            {
                                stringBuilder.AppendLine(AorTxt.Format("    if type(new) == \"table\" and new.x ~= nil and new.y ~= nil  and new.z ~= nil then"));
                                stringBuilder.AppendLine(AorTxt.Format("        self.{0}:rectTransform().anchoredPosition3D = Unity.Vector3(new.x, new.y, new.z)", bindedInjectionName));
                                stringBuilder.AppendLine(AorTxt.Format("    end"));
                            }
                            else if (bindedInjectionWay == "offsetMin")
                            {
                                stringBuilder.AppendLine(AorTxt.Format("    if type(new) == \"table\" and new.x ~= nil and new.y ~= nil then"));
                                stringBuilder.AppendLine(AorTxt.Format("        self.{0}:rectTransform().offsetMin = Unity.Vector2(new.x, new.y)", bindedInjectionName));
                                stringBuilder.AppendLine(AorTxt.Format("    end"));
                            }
                            else if (bindedInjectionWay == "offsetMax")
                            {
                                stringBuilder.AppendLine(AorTxt.Format("    if type(new) == \"table\" and new.x ~= nil and new.y ~= nil then"));
                                stringBuilder.AppendLine(AorTxt.Format("        self.{0}:rectTransform().offsetMax = Unity.Vector2(new.x, new.y)", bindedInjectionName));
                                stringBuilder.AppendLine(AorTxt.Format("    end"));
                            }
                            else if (bindedInjectionWay == "anchorMin")
                            {
                                stringBuilder.AppendLine(AorTxt.Format("    if type(new) == \"table\" and new.x ~= nil and new.y ~= nil then"));
                                stringBuilder.AppendLine(AorTxt.Format("        self.{0}:rectTransform().anchorMin = Unity.Vector2(new.x, new.y)", bindedInjectionName));
                                stringBuilder.AppendLine(AorTxt.Format("    end"));
                            }
                            else if (bindedInjectionWay == "anchorMax")
                            {
                                stringBuilder.AppendLine(AorTxt.Format("    if type(new) == \"table\" and new.x ~= nil and new.y ~= nil then"));
                                stringBuilder.AppendLine(AorTxt.Format("        self.{0}:rectTransform().anchorMax = Unity.Vector2(new.x, new.y)", bindedInjectionName));
                                stringBuilder.AppendLine(AorTxt.Format("    end"));
                            }
                            else if (bindedInjectionWay == "pivot")
                            {
                                stringBuilder.AppendLine(AorTxt.Format("    if type(new) == \"table\" and new.x ~= nil and new.y ~= nil then"));
                                stringBuilder.AppendLine(AorTxt.Format("        self.{0}:rectTransform().pivot = Unity.Vector2(new.x, new.y)", bindedInjectionName));
                                stringBuilder.AppendLine(AorTxt.Format("    end"));
                            }
                            else if (bindedInjectionWay == "sizeDelta")
                            {
                                stringBuilder.AppendLine(AorTxt.Format("    if type(new) == \"table\" and new.x ~= nil and new.y ~= nil then"));
                                stringBuilder.AppendLine(AorTxt.Format("        self.{0}:rectTransform().sizeDelta = Unity.Vector2(new.x, new.y)", bindedInjectionName));
                                stringBuilder.AppendLine(AorTxt.Format("    end"));
                            }
                            else if (bindedInjectionWay == "localEulerAngles")
                            {
                                stringBuilder.AppendLine(AorTxt.Format("    if type(new) == \"table\" and new.x ~= nil and new.y ~= nil and new.z ~= nil then"));
                                stringBuilder.AppendLine(AorTxt.Format("        self.{0}.transform.localEulerAngles = Unity.Vector3(new.x, new.y, new.z)", bindedInjectionName));
                                stringBuilder.AppendLine(AorTxt.Format("    end"));
                            }
                            else if (bindedInjectionWay == "eulerAngles")
                            {
                                stringBuilder.AppendLine(AorTxt.Format("    if type(new) == \"table\" and new.x ~= nil and new.y ~= nil and new.z ~= nil then"));
                                stringBuilder.AppendLine(AorTxt.Format("        self.{0}.transform.eulerAngles = Unity.Vector3(new.x, new.y, new.z)", bindedInjectionName));
                                stringBuilder.AppendLine(AorTxt.Format("    end"));
                            }
                            else if (bindedInjectionWay == "localScale")
                            {
                                stringBuilder.AppendLine(AorTxt.Format("    if type(new) == \"table\" and new.x ~= nil and new.y ~= nil and new.z ~= nil then"));
                                stringBuilder.AppendLine(AorTxt.Format("        self.{0}.transform.localScale = Unity.Vector3(new.x, new.y, new.z)", bindedInjectionName));
                                stringBuilder.AppendLine(AorTxt.Format("    end"));
                            }
                            else if (bindedInjectionWay == "ItemTotalNum")
                            {
                                stringBuilder.AppendLine(AorTxt.Format("    local operation = function()"));
                                stringBuilder.AppendLine(AorTxt.Format("        self.{0}:SetListItemCount(new, false)", bindedInjectionName));
                                stringBuilder.AppendLine(AorTxt.Format("    end"));
                                stringBuilder.AppendLine(AorTxt.Format("    if self.{0}.IsInited == false then CoroutineHelper:Start(handler(self, function(target) coroutine.yield(nil) operation() end)) else operation() end", bindedInjectionName));
                            }
                            else if (bindedInjectionWay == "ScrollTo")
                            {
                                stringBuilder.AppendLine(AorTxt.Format("    local operation = function()"));
                                stringBuilder.AppendLine(AorTxt.Format("        self.{0}:MovePanelToItemIndex(new, 0)", bindedInjectionName));
                                stringBuilder.AppendLine(AorTxt.Format("    end"));
                                stringBuilder.AppendLine(AorTxt.Format("    if self.{0}.IsInited == false then CoroutineHelper:Start(handler(self, function(target) coroutine.yield(nil) operation() end)) else operation() end", bindedInjectionName));
                            }
                            else if (bindedInjectionWay == "AddItemNum")
                            {
                                stringBuilder.AppendLine(AorTxt.Format("    local operation = function()"));
                                stringBuilder.AppendLine(AorTxt.Format("        self.{0}:SetListItemCount(self.{1}.ItemTotalCount + new, false)", bindedInjectionName, bindedInjectionName));
                                stringBuilder.AppendLine(AorTxt.Format("    end"));
                                stringBuilder.AppendLine(AorTxt.Format("    if self.{0}.IsInited == false then CoroutineHelper:Start(handler(self, function(target) coroutine.yield(nil) operation() end)) else operation() end", bindedInjectionName));
                            }
                        }
                    }
                    stringBuilder.AppendLine(AorTxt.Format("end"));
                    stringBuilder.AppendLine(AorTxt.Format(""));
                }
                stringBuilder.AppendLine(AorTxt.Format("-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<6"));
                stringBuilder.AppendLine(AorTxt.Format(""));
            }
            else if((MVVMPatternType)typeEnumIndex == MVVMPatternType.ViewModel)
            {
                stringBuilder.AppendLine(AorTxt.Format("---唤醒"))
                             .AppendLine(AorTxt.Format("---@type fun():void"))
                             .AppendLine(AorTxt.Format("function {0}:Awake()", luaName))
                             .AppendLine(AorTxt.Format("    {0}.super.Awake(self)", luaName))
                             .AppendLine(AorTxt.Format(""))
                             .AppendLine(AorTxt.Format("end"))
                             .AppendLine(AorTxt.Format(""));

                stringBuilder.AppendLine(AorTxt.Format("---初始化回调（在Awake中ViewSuper首次调用SetViewModel时触发，该方法执行后才会调用为View设置ViewModel，因此请勿在该方法中调用GetViewModel。）"))
                             .AppendLine(AorTxt.Format("---@type fun():void"))
                             .AppendLine(AorTxt.Format("function {0}:OnInit()", luaName))
                             .AppendLine(AorTxt.Format("    {0}.super.OnInit(self)", luaName))
                             .AppendLine(AorTxt.Format(""))
                             .AppendLine(AorTxt.Format("end"))
                             .AppendLine(AorTxt.Format(""));

                stringBuilder.AppendLine(AorTxt.Format("---开始"))
                             .AppendLine(AorTxt.Format("---@type fun():void"))
                             .AppendLine(AorTxt.Format("function {0}:Start()", luaName))
                             .AppendLine(AorTxt.Format("    {0}.super.Start(self)", luaName))
                             .AppendLine(AorTxt.Format(""))
                             .AppendLine(AorTxt.Format("end"))
                             .AppendLine(AorTxt.Format(""));

                if (m_UseProc.boolValue)
                {
                    stringBuilder.AppendLine(AorTxt.Format("---心跳（自定义）"))
                                 .AppendLine(AorTxt.Format("---@type fun():void"))
                                 .AppendLine(AorTxt.Format("function {0}:Proc()", luaName))
                                 .AppendLine(AorTxt.Format("    {0}.super.Proc(self)", luaName))
                                 .AppendLine(AorTxt.Format(""))
                                 .AppendLine(AorTxt.Format("end"))
                                 .AppendLine(AorTxt.Format(""));
                }

                stringBuilder.AppendLine(AorTxt.Format("---销毁"))
                             .AppendLine(AorTxt.Format("---@type fun():void"))
                             .AppendLine(AorTxt.Format("function {0}:OnDestroy()", luaName))
                             .AppendLine(AorTxt.Format("    {0}.super.OnDestroy(self)", luaName))
                             .AppendLine(AorTxt.Format(""))
                             .AppendLine(AorTxt.Format("end"))
                             .AppendLine(AorTxt.Format(""));

                stringBuilder.AppendLine(AorTxt.Format("---注册监听代码处（注：所有成员函数的注册监听代码都写在这里，成员函数作为监听回调时不需要手动注销，父类会自动辅助完成事件的注销操作。）"))
                             .AppendLine(AorTxt.Format("---@type fun():void"))
                             .AppendLine(AorTxt.Format("function {0}:OnAddListeners()", luaName))
                             .AppendLine(AorTxt.Format("    {0}.super.OnAddListeners(self)", luaName))
                             .AppendLine(AorTxt.Format(""))
                             .AppendLine(AorTxt.Format("end"))
                             .AppendLine(AorTxt.Format(""));
            }

            stringBuilder.AppendLine(AorTxt.Format("return {0}", luaName));
            stringBuilder.AppendLine(AorTxt.Format(""));

            return stringBuilder;
        }

        /// <summary>
        /// 刷新MVVM设计模式下已有Lua代码行
        /// </summary>
        /// <param name="fullPath">全路径</param>
        /// <returns></returns>
        private StringBuilder GeneratePatternMVVMCodeLines(string fullPath, int typeEnumIndex)
        {
            string luaScriptName = m_LuaScriptNamesMVVM.GetArrayElementAtIndex(typeEnumIndex).stringValue;
            string luaSuperScriptName = m_LuaSuperScriptNamesMVVM.GetArrayElementAtIndex(typeEnumIndex).stringValue;
            string luaName = luaScriptName;

            // 采集InfoEx辅助信息
            List<string> luaInjectNames = null;
            List<string> luaInjectComments = null;
            List<string> luaInjectFunctionNames = null;
            List<string> luaInjectFunctionParams = null;
            List<string> luaInjectCmds = null;
            CollectInfoExInfos(out luaInjectNames, out luaInjectComments, out luaInjectFunctionNames, out luaInjectFunctionParams, out luaInjectCmds);

            // 采集绑定数据与注入对象之间的各类信息
            SortedDictionary<string, List<string>> luaBindValueOnInjectionNames = null;
            SortedDictionary<string, List<string>> luaBindValueOnInjectionWays = null;
            SortedDictionary<string, List<string>> luaBindValueOnInjectionSides = null;
            SortedDictionary<string, string> luaBindValueComments = null;
            SortedDictionary<string, string> luaBindValueFunctionNames = null;
            SortedDictionary<string, string> luaBindValueTypeNames = null;
            CollectBindingInfosOnInjections(out luaBindValueOnInjectionNames, out luaBindValueOnInjectionWays, out luaBindValueOnInjectionSides, out luaBindValueComments, out luaBindValueFunctionNames, out luaBindValueTypeNames);

            if ((MVVMPatternType)typeEnumIndex == MVVMPatternType.View)
            {
                string content = System.IO.File.ReadAllText(fullPath);
                int endCommentStrIndex = 0, start1RightStrIndex = 0, end1LeftStrIndex = 0, end1RightStrIndex = 0;
                int start2RightStrIndex = 0, end2LeftStrIndex = 0, start3RightStrIndex = 0, end3LeftStrIndex = 0;
                int start4RightStrIndex = 0, end4LeftStrIndex = 0, start5LeftStrIndex = 0, start5RightStrIndex = 0, end5LeftStrIndex = 0;
                int start6RightStrIndex = 0, end6LeftStrIndex = 0;

                // 刷新标记的位置
                RefreshIndexes(luaName, content, out endCommentStrIndex, out start1RightStrIndex, out end1LeftStrIndex, out end1RightStrIndex, out start2RightStrIndex, out end2LeftStrIndex, out start3RightStrIndex, out end3LeftStrIndex, out start4RightStrIndex, out end4LeftStrIndex, out start5LeftStrIndex, out start5RightStrIndex, out end5LeftStrIndex, out start6RightStrIndex, out end6LeftStrIndex);

                // 移除3号区域的信息
                content = content.Remove(start3RightStrIndex, end3LeftStrIndex - start3RightStrIndex);

                // 移除4号区域的信息
                content = content.Remove(start4RightStrIndex, end4LeftStrIndex - start4RightStrIndex);

                // 移除2号区域的信息
                content = content.Remove(start2RightStrIndex, end2LeftStrIndex - start2RightStrIndex);

                // 移除1号区域的信息
                content = content.Remove(start1RightStrIndex, end1LeftStrIndex - start1RightStrIndex);

                // 刷新标记的位置
                RefreshIndexes(luaName, content, out endCommentStrIndex, out start1RightStrIndex, out end1LeftStrIndex, out end1RightStrIndex, out start2RightStrIndex, out end2LeftStrIndex, out start3RightStrIndex, out end3LeftStrIndex, out start4RightStrIndex, out end4LeftStrIndex, out start5LeftStrIndex, out start5RightStrIndex, out end5LeftStrIndex, out start6RightStrIndex, out end6LeftStrIndex);

                // 先插入3号区域的注销代码
                if (luaInjectFunctionNames.Count > 0)
                {
                    for (int index = 0; index < luaInjectFunctionNames.Count; index++)
                    {
                        content = content.Insert(end3LeftStrIndex, AorTxt.Format("    OnRemoveListener(self.{0}, '{1}', handler(self, self.{2}))\n", luaInjectNames[index], luaInjectCmds[index], luaInjectFunctionNames[index]));
                        // 刷新标记的位置
                        RefreshIndexes(luaName, content, out endCommentStrIndex, out start1RightStrIndex, out end1LeftStrIndex, out end1RightStrIndex, out start2RightStrIndex, out end2LeftStrIndex, out start3RightStrIndex, out end3LeftStrIndex, out start4RightStrIndex, out end4LeftStrIndex, out start5LeftStrIndex, out start5RightStrIndex, out end5LeftStrIndex, out start6RightStrIndex, out end6LeftStrIndex);
                    }
                }
                else
                {
                    content = content.Insert(end3LeftStrIndex, "-- 无自动注销内容。\n");
                    // 刷新标记的位置
                    RefreshIndexes(luaName, content, out endCommentStrIndex, out start1RightStrIndex, out end1LeftStrIndex, out end1RightStrIndex, out start2RightStrIndex, out end2LeftStrIndex, out start3RightStrIndex, out end3LeftStrIndex, out start4RightStrIndex, out end4LeftStrIndex, out start5LeftStrIndex, out start5RightStrIndex, out end5LeftStrIndex, out start6RightStrIndex, out end6LeftStrIndex);
                }

                // 再插入4号区域的注册代码
                if (m_InterBindValueNames.Count > 0)
                {
                    for (int index = 0; index < m_InterBindValueNames.Count; index++)
                    {
                        if (!string.IsNullOrEmpty(m_InterBindValueComments[index].stringValue))
                        {
                            content = content.Insert(end4LeftStrIndex, AorTxt.Format("    -- 添加数据变化监听：{0}，数据类型：{1}\n", m_InterBindValueComments[index].stringValue, GetBindValueTypeByName(m_InterBindValueNames[index].stringValue)));
                            // 刷新标记的位置
                            RefreshIndexes(luaName, content, out endCommentStrIndex, out start1RightStrIndex, out end1LeftStrIndex, out end1RightStrIndex, out start2RightStrIndex, out end2LeftStrIndex, out start3RightStrIndex, out end3LeftStrIndex, out start4RightStrIndex, out end4LeftStrIndex, out start5LeftStrIndex, out start5RightStrIndex, out end5LeftStrIndex, out start6RightStrIndex, out end6LeftStrIndex);
                        }
                        content = content.Insert(end4LeftStrIndex, AorTxt.Format("    self:AddBindHandler(self.BVKey.{0}, handler(self, self.{1}))\n", m_InterBindValueNames[index].stringValue, luaBindValueFunctionNames[m_InterBindValueNames[index].stringValue]));
                        // 刷新标记的位置
                        RefreshIndexes(luaName, content, out endCommentStrIndex, out start1RightStrIndex, out end1LeftStrIndex, out end1RightStrIndex, out start2RightStrIndex, out end2LeftStrIndex, out start3RightStrIndex, out end3LeftStrIndex, out start4RightStrIndex, out end4LeftStrIndex, out start5LeftStrIndex, out start5RightStrIndex, out end5LeftStrIndex, out start6RightStrIndex, out end6LeftStrIndex);
                    }
                }
                else
                {
                    content = content.Insert(end4LeftStrIndex, AorTxt.Format("    -- 无数据变化监听\n"));
                    // 刷新标记的位置
                    RefreshIndexes(luaName, content, out endCommentStrIndex, out start1RightStrIndex, out end1LeftStrIndex, out end1RightStrIndex, out start2RightStrIndex, out end2LeftStrIndex, out start3RightStrIndex, out end3LeftStrIndex, out start4RightStrIndex, out end4LeftStrIndex, out start5LeftStrIndex, out start5RightStrIndex, out end5LeftStrIndex, out start6RightStrIndex, out end6LeftStrIndex);
                }

                // 再插入2号区域的注册代码
                if (luaInjectFunctionNames.Count > 0)
                {
                    for (int index = 0; index < luaInjectFunctionNames.Count; index++)
                    {
                        content = content.Insert(end2LeftStrIndex, AorTxt.Format("    AddUIListenerFunction(self.{0}, '{1}', handler(self, self.{2}))\n", luaInjectNames[index], luaInjectCmds[index], luaInjectFunctionNames[index]));
                        // 刷新标记的位置
                        RefreshIndexes(luaName, content, out endCommentStrIndex, out start1RightStrIndex, out end1LeftStrIndex, out end1RightStrIndex, out start2RightStrIndex, out end2LeftStrIndex, out start3RightStrIndex, out end3LeftStrIndex, out start4RightStrIndex, out end4LeftStrIndex, out start5LeftStrIndex, out start5RightStrIndex, out end5LeftStrIndex, out start6RightStrIndex, out end6LeftStrIndex);
                    }
                }
                else
                {
                    content = content.Insert(end2LeftStrIndex, "-- 无自动注册内容。\n");
                    // 刷新标记的位置
                    RefreshIndexes(luaName, content, out endCommentStrIndex, out start1RightStrIndex, out end1LeftStrIndex, out end1RightStrIndex, out start2RightStrIndex, out end2LeftStrIndex, out start3RightStrIndex, out end3LeftStrIndex, out start4RightStrIndex, out end4LeftStrIndex, out start5LeftStrIndex, out start5RightStrIndex, out end5LeftStrIndex, out start6RightStrIndex, out end6LeftStrIndex);
                }

                // 再插入1号区域的注入提示信息（先2后1，1的标记位置不变）
                if (m_Injections != null && m_Injections.arraySize > 0)
                {
                    // 刷新标记的位置
                    RefreshIndexes(luaName, content, out endCommentStrIndex, out start1RightStrIndex, out end1LeftStrIndex, out end1RightStrIndex, out start2RightStrIndex, out end2LeftStrIndex, out start3RightStrIndex, out end3LeftStrIndex, out start4RightStrIndex, out end4LeftStrIndex, out start5LeftStrIndex, out start5RightStrIndex, out end5LeftStrIndex, out start6RightStrIndex, out end6LeftStrIndex);

                    // 从后往前插入，逆向插入
                    for (int index = m_Injections.arraySize - 1; index >= 0; index--)
                    {
                        string comment = !string.IsNullOrEmpty(m_InterInjectionComments[index].stringValue) ? m_InterInjectionComments[index].stringValue : string.Empty;
                        string fieldName = m_InterInjectionNames[index].stringValue;
                        string typeName = string.Empty;
                        string isValid = string.Empty;
                        string infoEx = string.Empty;

                        if (m_InterInjectionIsArrays[index].boolValue)
                        {
                            typeName = $"{LuaInjection.LuaInjectionType[(int)m_InterInjectionTypeNames[index].enumValueIndex]}[]";
                            isValid = "√";
                            infoEx = string.Empty;
                            for (int elementIndex = 0; elementIndex < m_InterInjectionElementsObjs[index].arraySize; elementIndex++)
                            {
                                if (m_InterInjectionElementsObjs[index].GetArrayElementAtIndex(elementIndex).objectReferenceValue == null)
                                {
                                    isValid = "×";
                                    infoEx = string.Empty;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            typeName = LuaInjection.LuaInjectionType[(int)m_InterInjectionTypeNames[index].enumValueIndex];
                            isValid = (m_InterInjectionTypeNames[index].enumValueIndex < (int)LuaInjection.InjectionType.Int32 || m_InterInjectionTypeNames[index].enumValueIndex > (int)LuaInjection.InjectionType.Boolean) ? (m_InterInjectionObjs[index].objectReferenceValue != null ? "√" : "×") : (string.IsNullOrEmpty(m_InterInjectionVariants[index].stringValue) ? "×" : m_InterInjectionVariants[index].stringValue);
                            infoEx = m_InterInjectionInfoExs[index].stringValue;
                        }

                        if (typeName.Equals("UnityEngine.GameObject") && !string.IsNullOrEmpty(infoEx))
                        {
                            var findTypeName = Assembly.GetType(infoEx);
                            typeName = findTypeName == null ? "any" : findTypeName.FullName;
                        }
                        else if (typeName.Equals("Honor.Runtime.LuaBehaviour") && !string.IsNullOrEmpty(infoEx))
                        {
                            typeName = infoEx;
                        }

                        while (fieldName.Length < 35) fieldName += " ";
                        while (typeName.Length < 30) typeName += " ";
                        while (isValid.Length < 10) isValid += " ";
                        while (infoEx.Length < 15) infoEx += " ";

                        content = content.Insert(start1RightStrIndex, AorTxt.Format("---@field {0}{1}{2}{3}{4}\n", fieldName, typeName, isValid, infoEx, comment));
                    }
                }
                content = content.Insert(start1RightStrIndex, AorTxt.Format("---@field cs Honor.Runtime.LuaBehaviour @LuaBehaviour\n"));
                content = content.Insert(start1RightStrIndex, AorTxt.Format("---@class {0} : {1}\n", luaName, luaSuperScriptName));

                // 刷新标记的位置
                RefreshIndexes(luaName, content, out endCommentStrIndex, out start1RightStrIndex, out end1LeftStrIndex, out end1RightStrIndex, out start2RightStrIndex, out end2LeftStrIndex, out start3RightStrIndex, out end3LeftStrIndex, out start4RightStrIndex, out end4LeftStrIndex, out start5LeftStrIndex, out start5RightStrIndex, out end5LeftStrIndex, out start6RightStrIndex, out end6LeftStrIndex);

                // 追加新的Function定义
                string functionDef = string.Empty;
                if (m_UseProc.boolValue)
                {
                    if (!content.Contains(AorTxt.Format("function {0}:Proc()", luaName)))
                    {
                        functionDef = AorTxt.Format("{0}{1}\n{2}\n{3}\n{4}\n\nend\n\n", functionDef, AorTxt.Format("---心跳（自定义）"), AorTxt.Format("---@type fun():void"), AorTxt.Format("function {0}:Proc()", luaName), AorTxt.Format("    {0}.super.Proc(self)", luaName));
                    }
                }

                // 判断OnAddedUIDestroyed是否存在
                if (!content.Contains(AorTxt.Format("function {0}:OnAddedUIDestroyed(luaClass)", luaName)))
                {
                    functionDef = AorTxt.Format("{0}{1}\n{2}\n{3}\n{4}\n{5}\n\nend\n\n", functionDef, AorTxt.Format("---被追加的N层UI子节点销毁回调"), AorTxt.Format("---@type fun(luaClass:XLua.LuaTable):void"), AorTxt.Format("---@param luaClass XLua.LuaTable @luaClass"), AorTxt.Format("function {0}:OnAddedUIDestroyed(luaClass)", luaName), AorTxt.Format("    {0}.super.OnAddedUIDestroyed(self, luaClass)", luaName));
                }

                content = content.Insert(start5LeftStrIndex, functionDef);

                // 刷新标记的位置
                RefreshIndexes(luaName, content, out endCommentStrIndex, out start1RightStrIndex, out end1LeftStrIndex, out end1RightStrIndex, out start2RightStrIndex, out end2LeftStrIndex, out start3RightStrIndex, out end3LeftStrIndex, out start4RightStrIndex, out end4LeftStrIndex, out start5LeftStrIndex, out start5RightStrIndex, out end5LeftStrIndex, out start6RightStrIndex, out end6LeftStrIndex);

                functionDef = string.Empty;

                // UI交互监听，5号区域（已存在的func不做处理，仅针对不存在的func进行插入生成）
                for (int index = 0; index < luaInjectFunctionNames.Count; index++)
                {
                    string[] paramsArray = luaInjectFunctionParams[index].Replace(" ", string.Empty).Split(',');
                    string funcDesc = string.Empty;
                    string funcParams = string.Empty;
                    foreach (var param in paramsArray)
                    {
                        funcDesc += ((string.IsNullOrEmpty(funcDesc) ? "" : ", ") + param + ":any");
                        funcParams += ((string.IsNullOrEmpty(funcParams) ? "" : ", ") + param);
                    }
                    string checkContent = AorTxt.Format("function {0}:{1}({2})", luaName, luaInjectFunctionNames[index], funcParams);
                    if (!content.Contains(checkContent) && !functionDef.Contains(checkContent))
                    {
                        string tmp = string.Empty;
                        foreach (var param in paramsArray)
                        {
                            tmp += AorTxt.Format("---@param {0} any @UI交互内容\n", param);
                        }

                        functionDef = AorTxt.Format("{0}{1}\n{2}\n{3}{4}\n", 
                                                functionDef, 
                                                AorTxt.Format("---{0}", luaInjectComments[index]), 
                                                AorTxt.Format("---@type fun({0}):void", funcDesc),
                                                tmp,
                                                checkContent);

                        foreach (var injectionName in luaBindValueOnInjectionNames)
                        {
                            List<int> indexes = new List<int>();
                            for (int i = 0; i < injectionName.Value.Count; i++)
                            {
                                if (injectionName.Value[i] == luaInjectNames[index])
                                {
                                    indexes.Add(i);
                                }
                            }
                            for (int i = 0; i < indexes.Count; i++)
                            {
                                // 只有双向绑定的数据才会在UI交互监听回调中对相关的绑定数据进行赋值
                                string side = luaBindValueOnInjectionSides[injectionName.Key][indexes[i]];
                                if (side == "Two")
                                {
                                    string value = AorTxt.Format((luaBindValueTypeNames[injectionName.Key] == "table" || luaBindValueTypeNames[injectionName.Key] == "array") ? "TableDecode({0})" : "{0}", luaInjectFunctionParams[index]);
                                    if (!string.IsNullOrEmpty(value))
                                    {
                                        functionDef = AorTxt.Format("{0}    self:GetViewModel():SetBindValue(self.BVKey.{1}, {2})\n", functionDef, injectionName.Key, value);
                                    }
                                }
                            }
                        }

                        if (luaInjectFunctionNames[index].EndsWith("GettingListItem"))
                        {
                            functionDef = AorTxt.Format("{0}{1}\n", functionDef, $"    if itemIndex < 0 or itemIndex >= self.{luaInjectNames[index]}.MaxItemNum then return nil end");
                            functionDef = AorTxt.Format("{0}{1}\n", functionDef, $"    local item = self.{luaInjectNames[index]}:NewListViewItem('Item')");
                            functionDef = AorTxt.Format("{0}{1}\n", functionDef, $"    if item.IsInitHandlerCalled == false then item.IsInitHandlerCalled = true end");
                            functionDef = AorTxt.Format("{0}{1}\n", functionDef, $"    return item");
                        }
                        else if (luaInjectFunctionNames[index].EndsWith("GettingGridItem"))
                        {
                            functionDef = AorTxt.Format("{0}{1}\n", functionDef, $"    if itemIndex < 0 or itemIndex >= self.{luaInjectNames[index]}.MaxItemNum then return nil end");
                            functionDef = AorTxt.Format("{0}{1}\n", functionDef, $"    local item = self.{luaInjectNames[index]}:NewGridViewItem('Item')");
                            functionDef = AorTxt.Format("{0}{1}\n", functionDef, $"    if item.IsInitHandlerCalled == false then item.IsInitHandlerCalled = true end");
                            functionDef = AorTxt.Format("{0}{1}\n", functionDef, $"    return item");
                        }

                        functionDef = AorTxt.Format("{0}end\n\n", functionDef);
                    }
                }
                content = content.Insert(end5LeftStrIndex, functionDef);

                // 刷新标记的位置
                RefreshIndexes(luaName, content, out endCommentStrIndex, out start1RightStrIndex, out end1LeftStrIndex, out end1RightStrIndex, out start2RightStrIndex, out end2LeftStrIndex, out start3RightStrIndex, out end3LeftStrIndex, out start4RightStrIndex, out end4LeftStrIndex, out start5LeftStrIndex, out start5RightStrIndex, out end5LeftStrIndex, out start6RightStrIndex, out end6LeftStrIndex);

                functionDef = string.Empty;

                // 绑定数据变化监听，6号区域（已存在的func不做处理，仅针对不存在的func进行插入生成）
                foreach (var luaBindValueFunctionName in luaBindValueFunctionNames)
                {
                    string bindValueName = luaBindValueFunctionName.Key;                    
                    if(luaBindValueTypeNames[bindValueName] == "trigger")
                    {
                        string checkContent = AorTxt.Format("function {0}:{1}()", luaName, luaBindValueFunctionName.Value);
                        if(!content.Contains(checkContent))
                        {
                            functionDef = AorTxt.Format("{0}{1}\n{2}\n{3}\nend\n\n",
                                            functionDef,
                                            AorTxt.Format("---{0}", luaBindValueComments[bindValueName]),
                                            AorTxt.Format("---@type fun():void"),
                                            checkContent);
                        }
                    }
                    else
                    {
                        string checkContent = AorTxt.Format("function {0}:{1}(old, new, operate, valueItem)", luaName, luaBindValueFunctionName.Value);
                        if (!content.Contains(checkContent))
                        {
                            string innerContent = string.Empty;
                            // 对当前bindValue所绑定的所有ui对象进行ui指定绑定方式的更新
                            for (int checkIndex = 0; checkIndex < luaBindValueOnInjectionNames[bindValueName].Count; checkIndex++)
                            {
                                string bindedInjectionName = luaBindValueOnInjectionNames[bindValueName][checkIndex];
                                string bindedInjectionWay = luaBindValueOnInjectionWays[bindValueName][checkIndex];
                                if (bindedInjectionWay == "Active")
                                {
                                    innerContent = AorTxt.Format("{0}    self.{1}.gameObject:SetActive(new)\n", innerContent, bindedInjectionName);
                                }
                                else if (bindedInjectionWay == "Interactable")
                                {
                                    innerContent = AorTxt.Format("{0}    self.{1}.interactable = new\n", innerContent, bindedInjectionName);
                                }
                                else if (bindedInjectionWay == "Content")
                                {
                                    string injectionTypeName = GetInjectionTypeByName(bindedInjectionName);
                                    switch (injectionTypeName)
                                    {
                                        case "UnityEngine.UI.Text": innerContent = AorTxt.Format("{0}    self.{1}.text = {2}\n", innerContent, bindedInjectionName, (luaBindValueTypeNames[bindValueName] == "table" || luaBindValueTypeNames[bindValueName] == "array") ? "TableEncode(new)" : "tostring(new)"); break;
                                        case "Honor.Runtime.TextPicMixed": innerContent = AorTxt.Format("{0}    self.{1}.text = {2}\n", innerContent, bindedInjectionName, (luaBindValueTypeNames[bindValueName] == "table" || luaBindValueTypeNames[bindValueName] == "array") ? "TableEncode(new)" : "tostring(new)"); break;
                                        case "TMPro.TextMeshProUGUI": innerContent = AorTxt.Format("{0}    self.{1}.text = {2}\n", innerContent, bindedInjectionName, (luaBindValueTypeNames[bindValueName] == "table" || luaBindValueTypeNames[bindValueName] == "array") ? "TableEncode(new)" : "tostring(new)"); break;
                                        case "UnityEngine.UI.Toggle": innerContent = AorTxt.Format("{0}    self.{1}.isOn = new\n", innerContent, bindedInjectionName); break;
                                        case "UnityEngine.UI.Slider": innerContent = AorTxt.Format("{0}    self.{1}.value = new\n", innerContent, bindedInjectionName); break;
                                        case "UnityEngine.UI.Dropdown": innerContent = AorTxt.Format("{0}    self.{1}.value = new\n", innerContent, bindedInjectionName); break;
                                        case "UnityEngine.UI.InputField": innerContent = AorTxt.Format("{0}    self.{1}.text = {2}\n", innerContent, bindedInjectionName, (luaBindValueTypeNames[bindValueName] == "table" || luaBindValueTypeNames[bindValueName] == "array") ? "TableEncode(new)" : "tostring(new)"); break;
                                        case "Honor.Runtime.SwitchButton": innerContent = AorTxt.Format("{0}    self.{1}.isOn = new\n", innerContent, bindedInjectionName); break;
                                    }
                                }
                                else if (bindedInjectionWay == "anchoredPosition")
                                {
                                    innerContent = AorTxt.Format("{0}    if type(new) == \"table\" and new.x ~= nil and new.y ~= nil then\n", innerContent);
                                    innerContent = AorTxt.Format("{0}        self.{1}:rectTransform().anchoredPosition = Unity.Vector2(new.x, new.y)\n", innerContent, bindedInjectionName);
                                    innerContent = AorTxt.Format("{0}    end\n", innerContent);
                                }
                                else if (bindedInjectionWay == "anchoredPosition3D")
                                {
                                    innerContent = AorTxt.Format("{0}    if type(new) == \"table\" and new.x ~= nil and new.y ~= nil and new.z ~= nil then\n", innerContent);
                                    innerContent = AorTxt.Format("{0}        self.{1}:rectTransform().anchoredPosition3D = Unity.Vector3(new.x, new.y, new.z)\n", innerContent, bindedInjectionName);
                                    innerContent = AorTxt.Format("{0}    end\n", innerContent);
                                }
                                else if (bindedInjectionWay == "offsetMin")
                                {
                                    innerContent = AorTxt.Format("{0}    if type(new) == \"table\" and new.x ~= nil and new.y ~= nil then\n", innerContent);
                                    innerContent = AorTxt.Format("{0}        self.{1}:rectTransform().offsetMin = Unity.Vector2(new.x, new.y)\n", innerContent, bindedInjectionName);
                                    innerContent = AorTxt.Format("{0}    end\n", innerContent);
                                }
                                else if (bindedInjectionWay == "offsetMax")
                                {
                                    innerContent = AorTxt.Format("{0}    if type(new) == \"table\" and new.x ~= nil and new.y ~= nil then\n", innerContent);
                                    innerContent = AorTxt.Format("{0}        self.{1}:rectTransform().offsetMax = Unity.Vector2(new.x, new.y)\n", innerContent, bindedInjectionName);
                                    innerContent = AorTxt.Format("{0}    end\n", innerContent);
                                }
                                else if (bindedInjectionWay == "anchorMin")
                                {
                                    innerContent = AorTxt.Format("{0}    if type(new) == \"table\" and new.x ~= nil and new.y ~= nil then\n", innerContent);
                                    innerContent = AorTxt.Format("{0}        self.{1}:rectTransform().anchorMin = Unity.Vector2(new.x, new.y)\n", innerContent, bindedInjectionName);
                                    innerContent = AorTxt.Format("{0}    end\n", innerContent);
                                }
                                else if (bindedInjectionWay == "anchorMax")
                                {
                                    innerContent = AorTxt.Format("{0}    if type(new) == \"table\" and new.x ~= nil and new.y ~= nil then\n", innerContent);
                                    innerContent = AorTxt.Format("{0}        self.{1}:rectTransform().anchorMax = Unity.Vector2(new.x, new.y)\n", innerContent, bindedInjectionName);
                                    innerContent = AorTxt.Format("{0}    end\n", innerContent);
                                }
                                else if (bindedInjectionWay == "pivot")
                                {
                                    innerContent = AorTxt.Format("{0}    if type(new) == \"table\" and new.x ~= nil and new.y ~= nil then\n", innerContent);
                                    innerContent = AorTxt.Format("{0}        self.{1}:rectTransform().pivot = Unity.Vector2(new.x, new.y)\n", innerContent, bindedInjectionName);
                                    innerContent = AorTxt.Format("{0}    end\n", innerContent);
                                }
                                else if (bindedInjectionWay == "sizeDelta")
                                {
                                    innerContent = AorTxt.Format("{0}    if type(new) == \"table\" and new.x ~= nil and new.y ~= nil then\n", innerContent);
                                    innerContent = AorTxt.Format("{0}        self.{1}:rectTransform().sizeDelta = Unity.Vector2(new.x, new.y)\n", innerContent, bindedInjectionName);
                                    innerContent = AorTxt.Format("{0}    end\n", innerContent);
                                }
                                else if (bindedInjectionWay == "localEulerAngles")
                                {
                                    innerContent = AorTxt.Format("{0}    if type(new) == \"table\" and new.x ~= nil and new.y ~= nil and new.z ~= nil then\n", innerContent);
                                    innerContent = AorTxt.Format("{0}        self.{1}.transform.localEulerAngles = Unity.Vector3(new.x, new.y, new.z)\n", innerContent, bindedInjectionName);
                                    innerContent = AorTxt.Format("{0}    end\n", innerContent);
                                }
                                else if (bindedInjectionWay == "eulerAngles")
                                {
                                    innerContent = AorTxt.Format("{0}    if type(new) == \"table\" and new.x ~= nil and new.y ~= nil and new.z ~= nil then\n", innerContent);
                                    innerContent = AorTxt.Format("{0}        self.{1}.transform.eulerAngles = Unity.Vector3(new.x, new.y, new.z)\n", innerContent, bindedInjectionName);
                                    innerContent = AorTxt.Format("{0}    end\n", innerContent);
                                }
                                else if (bindedInjectionWay == "localScale")
                                {
                                    innerContent = AorTxt.Format("{0}    if type(new) == \"table\" and new.x ~= nil and new.y ~= nil and new.z ~= nil then\n", innerContent);
                                    innerContent = AorTxt.Format("{0}        self.{1}.transform.localScale = Unity.Vector3(new.x, new.y, new.z)\n", innerContent, bindedInjectionName);
                                    innerContent = AorTxt.Format("{0}    end\n", innerContent);
                                }
                                else if (bindedInjectionWay == "ItemTotalNum")
                                {
                                    innerContent = AorTxt.Format("{0}    local operation = function()\n", innerContent);
                                    innerContent = AorTxt.Format("{0}        self.{1}:SetListItemCount(new, false)\n", innerContent, bindedInjectionName);
                                    innerContent = AorTxt.Format("{0}    end\n", innerContent);
                                    innerContent = AorTxt.Format("{0}    if self.{1}.IsInited == false then CoroutineHelper:Start(handler(self, function(target) coroutine.yield(nil) operation() end)) else operation() end\n", innerContent, bindedInjectionName);
                                }
                                else if (bindedInjectionWay == "ScrollTo")
                                {
                                    innerContent = AorTxt.Format("{0}    local operation = function()\n", innerContent);
                                    innerContent = AorTxt.Format("{0}        self.{1}:MovePanelToItemIndex(new, 0)\n", innerContent, bindedInjectionName);
                                    innerContent = AorTxt.Format("{0}    end\n", innerContent);
                                    innerContent = AorTxt.Format("{0}    if self.{1}.IsInited == false then CoroutineHelper:Start(handler(self, function(target) coroutine.yield(nil) operation() end)) else operation() end\n", innerContent, bindedInjectionName);
                                }
                                else if (bindedInjectionWay == "AddItemNum")
                                {
                                    innerContent = AorTxt.Format("{0}    local operation = function()\n", innerContent);
                                    innerContent = AorTxt.Format("{0}        self.{1}:SetListItemCount(self.{2}.ItemTotalCount + new, false)\n", innerContent, bindedInjectionName, bindedInjectionName);
                                    innerContent = AorTxt.Format("{0}    end\n", innerContent);
                                    innerContent = AorTxt.Format("{0}    if self.{1}.IsInited == false then CoroutineHelper:Start(handler(self, function(target) coroutine.yield(nil) operation() end)) else operation() end\n", innerContent, bindedInjectionName);
                                }
                            }
                            functionDef = AorTxt.Format("{0}{1}\n{2}\n{3}\n{4}\n{5}\n{6}\n{7}\n{8}end\n\n",
                                                        functionDef,
                                                        AorTxt.Format("---{0}", luaBindValueComments[bindValueName]),
                                                        AorTxt.Format("---@type fun(old:{0}, new:{1}, operate:string, valueItem:any):void", luaBindValueTypeNames[bindValueName] == "array" ? "table" : luaBindValueTypeNames[bindValueName], luaBindValueTypeNames[bindValueName] == "array" ? "table" : luaBindValueTypeNames[bindValueName]),
                                                        AorTxt.Format("---@param old {0} @旧值内容", luaBindValueTypeNames[bindValueName] == "array" ? "table" : luaBindValueTypeNames[bindValueName]),
                                                        AorTxt.Format("---@param new {0} @新值内容", luaBindValueTypeNames[bindValueName] == "array" ? "table" : luaBindValueTypeNames[bindValueName]),
                                                        AorTxt.Format("---@param operate string @绑定数据操作类型"),
                                                        AorTxt.Format("---@param valueItem any @动态数据集合Item值内容"),
                                                        checkContent,
                                                        innerContent);
                        }
                    }   
                }

                content = content.Insert(end6LeftStrIndex, functionDef);

                // 刷新标记的位置
                RefreshIndexes(luaName, content, out endCommentStrIndex, out start1RightStrIndex, out end1LeftStrIndex, out end1RightStrIndex, out start2RightStrIndex, out end2LeftStrIndex, out start3RightStrIndex, out end3LeftStrIndex, out start4RightStrIndex, out end4LeftStrIndex, out start5LeftStrIndex, out start5RightStrIndex, out end5LeftStrIndex, out start6RightStrIndex, out end6LeftStrIndex);

                functionDef = string.Empty;

                // Collider2D/3D碰撞器生命周期函数
                if (m_UseCollider2DLifeCycles.boolValue)
                {
                    string checkContent = AorTxt.Format("function {0}:OnCollisionEnter2D(collision2D)", luaName);
                    if (!content.Contains(checkContent))
                    {
                        functionDef = AorTxt.Format("{0}{1}\n{2}\n{3}\n{4}\n\nend\n\n", functionDef, AorTxt.Format("---进入碰撞2D"), AorTxt.Format("---@type fun(collision2D:UnityEngine.Collision2D):void"), AorTxt.Format("---@param collision2D UnityEngine.Collision2D @碰撞2D"), checkContent);
                    }
                    checkContent = AorTxt.Format("function {0}:OnCollisionStay2D(collision2D)", luaName);
                    if (!content.Contains(checkContent))
                    {
                        functionDef = AorTxt.Format("{0}{1}\n{2}\n{3}\n{4}\n\nend\n\n", functionDef, AorTxt.Format("---停留碰撞2D"), AorTxt.Format("---@type fun(collision2D:UnityEngine.Collision2D):void"), AorTxt.Format("---@param collision2D UnityEngine.Collision2D @碰撞2D"), checkContent);
                    }
                    checkContent = AorTxt.Format("function {0}:OnCollisionExit2D(collision2D)", luaName);
                    if (!content.Contains(checkContent))
                    {
                        functionDef = AorTxt.Format("{0}{1}\n{2}\n{3}\n{4}\n\nend\n\n", functionDef, AorTxt.Format("---退出碰撞2D"), AorTxt.Format("---@type fun(collision2D:UnityEngine.Collision2D):void"), AorTxt.Format("---@param collision2D UnityEngine.Collision2D @碰撞2D"), checkContent);
                    }
                }
                if (m_UseCollider3DLifeCycles.boolValue)
                {
                    string checkContent = AorTxt.Format("function {0}:OnCollisionEnter3D(collision3D)", luaName);
                    if (!content.Contains(checkContent))
                    {
                        functionDef = AorTxt.Format("{0}{1}\n{2}\n{3}\n{4}\n\nend\n\n", functionDef, AorTxt.Format("---进入碰撞3D"), AorTxt.Format("---@type fun(collision3D:UnityEngine.Collision):void"), AorTxt.Format("---@param collision3D UnityEngine.Collision @碰撞3D"), checkContent);
                    }
                    checkContent = AorTxt.Format("function {0}:OnCollisionStay3D(collision3D)", luaName);
                    if (!content.Contains(checkContent))
                    {
                        functionDef = AorTxt.Format("{0}{1}\n{2}\n{3}\n{4}\n\nend\n\n", functionDef, AorTxt.Format("---停留碰撞3D"), AorTxt.Format("---@type fun(collision3D:UnityEngine.Collision):void"), AorTxt.Format("---@param collision3D UnityEngine.Collision @碰撞3D"), checkContent);
                    }
                    checkContent = AorTxt.Format("function {0}:OnCollisionExit3D(collision3D)", luaName);
                    if (!content.Contains(checkContent))
                    {
                        functionDef = AorTxt.Format("{0}{1}\n{2}\n{3}\n{4}\n\nend\n\n", functionDef, AorTxt.Format("---退出碰撞3D"), AorTxt.Format("---@type fun(collision3D:UnityEngine.Collision):void"), AorTxt.Format("---@param collision3D UnityEngine.Collision @碰撞3D"), checkContent);
                    }
                }

                // Trigger2D/3D触发器生命周期函数
                if (m_UseTrigger2DLifeCycles.boolValue)
                {
                    string checkContent = AorTxt.Format("function {0}:OnTriggerEnter2D(other2D)", luaName);
                    if (!content.Contains(checkContent))
                    {
                        functionDef = AorTxt.Format("{0}{1}\n{2}\n{3}\n{4}\n\nend\n\n", functionDef, AorTxt.Format("---进入触发2D"), AorTxt.Format("---@type fun(other2D:UnityEngine.Collider2D):void"), AorTxt.Format("---@param other2D UnityEngine.Collider2D @对方碰撞器2D"), checkContent);
                    }
                    checkContent = AorTxt.Format("function {0}:OnTriggerStay2D(other2D)", luaName);
                    if (!content.Contains(checkContent))
                    {
                        functionDef = AorTxt.Format("{0}{1}\n{2}\n{3}\n{4}\n\nend\n\n", functionDef, AorTxt.Format("---停留触发2D"), AorTxt.Format("---@type fun(other2D:UnityEngine.Collider2D):void"), AorTxt.Format("---@param other2D UnityEngine.Collider2D @对方碰撞器2D"), checkContent);
                    }
                    checkContent = AorTxt.Format("function {0}:OnTriggerExit2D(other2D)", luaName);
                    if (!content.Contains(checkContent))
                    {
                        functionDef = AorTxt.Format("{0}{1}\n{2}\n{3}\n{4}\n\nend\n\n", functionDef, AorTxt.Format("---退出触发2D"), AorTxt.Format("---@type fun(other2D:UnityEngine.Collider2D):void"), AorTxt.Format("---@param other2D UnityEngine.Collider2D @对方碰撞器2D"), checkContent);
                    }
                }
                if (m_UseTrigger3DLifeCycles.boolValue)
                {
                    string checkContent = AorTxt.Format("function {0}:OnTriggerEnter3D(other3D)", luaName);
                    if (!content.Contains(checkContent))
                    {
                        functionDef = AorTxt.Format("{0}{1}\n{2}\n{3}\n{4}\n\nend\n\n", functionDef, AorTxt.Format("---进入触发3D"), AorTxt.Format("---@type fun(other3D:UnityEngine.Collider):void"), AorTxt.Format("---@param other3D UnityEngine.Collider @对方碰撞器3D"), checkContent);
                    }
                    checkContent = AorTxt.Format("function {0}:OnTriggerStay3D(other3D)", luaName);
                    if (!content.Contains(checkContent))
                    {
                        functionDef = AorTxt.Format("{0}{1}\n{2}\n{3}\n{4}\n\nend\n\n", functionDef, AorTxt.Format("---停留触发3D"), AorTxt.Format("---@type fun(other3D:UnityEngine.Collider):void"), AorTxt.Format("---@param other3D UnityEngine.Collider @对方碰撞器3D"), checkContent);
                    }
                    checkContent = AorTxt.Format("function {0}:OnTriggerExit3D(other3D)", luaName);
                    if (!content.Contains(checkContent))
                    {
                        functionDef = AorTxt.Format("{0}{1}\n{2}\n{3}\n{4}\n\nend\n\n", functionDef, AorTxt.Format("---退出触发3D"), AorTxt.Format("---@type fun(other3D:UnityEngine.Collider):void"), AorTxt.Format("---@param other3D UnityEngine.Collider @对方碰撞器3D"), checkContent);
                    }
                }

                int returnRowIndex = content.LastIndexOf(AorTxt.Format("return {0}", luaName));
                content = content.Insert(returnRowIndex, functionDef);

                // 刷新标记的位置
                RefreshIndexes(luaName, content, out endCommentStrIndex, out start1RightStrIndex, out end1LeftStrIndex, out end1RightStrIndex, out start2RightStrIndex, out end2LeftStrIndex, out start3RightStrIndex, out end3LeftStrIndex, out start4RightStrIndex, out end4LeftStrIndex, out start5LeftStrIndex, out start5RightStrIndex, out end5LeftStrIndex, out start6RightStrIndex, out end6LeftStrIndex);

                // 更新绑定值Key名称定义
                int bvKeyDefIndex = content.LastIndexOf(AorTxt.Format("{0}.BVKey = ", luaName));
                int defCharCount = 0;
                while (content[bvKeyDefIndex + defCharCount] != '\n')
                {
                    defCharCount++;
                }
                content = content.Remove(bvKeyDefIndex, defCharCount);
                string bvkeyContent = "{";
                foreach (var bindValueName in m_InterBindValueNames)
                {
                    bvkeyContent = AorTxt.Format("{0}{1} = \"{2}\", ", bvkeyContent, bindValueName.stringValue, bindValueName.stringValue);
                }
                bvkeyContent += "}";
                content = content.Insert(bvKeyDefIndex, AorTxt.Format("{0}.BVKey = {1}", luaName, bvkeyContent));

                // 更新类的头部定义
                int classNameDefIndex = content.LastIndexOf(AorTxt.Format("local {0} = class('{1}', import", luaName, luaName));
                defCharCount = 0;
                while (content[classNameDefIndex + defCharCount] != '\n')
                {
                    defCharCount++;
                }
                content = content.Remove(classNameDefIndex, defCharCount);
                content = content.Insert(classNameDefIndex, AorTxt.Format("local {0} = class('{1}', import('{2}'))", luaName, luaName, luaSuperScriptName));

                StringBuilder stringBuilderCodeLines = new StringBuilder(content, endCommentStrIndex, content.Length - endCommentStrIndex, content.Length * 2);
                return stringBuilderCodeLines;
            }
            else if((MVVMPatternType)typeEnumIndex == MVVMPatternType.ViewModel)
            {
                string commentStr = "--=====================================================================================================";
                string end1Str = AorTxt.Format("local {0} = class", luaName);
                string start2RightStr = "-- 2>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>";
                string end2Str = "-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<2";
                
                string content = System.IO.File.ReadAllText(fullPath);

                int endCommentStrIndex = content.LastIndexOf(commentStr) + commentStr.Length + 4;
                int start1RightStrIndex = endCommentStrIndex;
                int end1LeftStrIndex = content.LastIndexOf(end1Str);
                int end1RightStrIndex = content.LastIndexOf(end1Str) + end1Str.Length + 4;
                int start2RightStrIndex = content.LastIndexOf(start2RightStr) + start2RightStr.Length + 1;
                int end2LeftStrIndex = content.LastIndexOf(end2Str);

                // 移除2号区域的信息
                content = content.Remove(start2RightStrIndex, end2LeftStrIndex - start2RightStrIndex);

                // 移除1号区域的信息
                content = content.Remove(start1RightStrIndex, end1LeftStrIndex - start1RightStrIndex);

                // 刷新标记的位置
                endCommentStrIndex = content.LastIndexOf(commentStr) + commentStr.Length + 4;
                start1RightStrIndex = endCommentStrIndex;
                end1LeftStrIndex = content.LastIndexOf(end1Str);
                end1RightStrIndex = content.LastIndexOf(end1Str) + end1Str.Length + 4;
                start2RightStrIndex = content.LastIndexOf(start2RightStr) + start2RightStr.Length + 1;
                end2LeftStrIndex = content.LastIndexOf(end2Str);

                // 插入2号区域的注册代码
                if (m_InterBindValueNames.Count > 0)
                {
                    for (int index = 0; index < m_InterBindValueNames.Count; index++)
                    {
                        if (!string.IsNullOrEmpty(m_InterBindValueComments[index].stringValue))
                        {
                            content = content.Insert(end2LeftStrIndex, AorTxt.Format("    -- 添加绑定数据成员：{0}，数据类型：{1}\n", m_InterBindValueComments[index].stringValue, GetBindValueTypeByName(m_InterBindValueNames[index].stringValue)));
                            end2LeftStrIndex = content.LastIndexOf(end2Str);
                        }
                        if(string.IsNullOrEmpty(m_InterBindValueVariants[index].stringValue))
                        {
                            if ((LuaBindValue.BindValueType)m_InterBindValueTypeNames[index].enumValueIndex == LuaBindValue.BindValueType.Trigger)
                            {
                                content = content.Insert(end2LeftStrIndex, AorTxt.Format("    self:AddBindValue(self.BVKey.{0}, false, true)\n", m_InterBindValueNames[index].stringValue));
                            }
                            else
                            {
                                content = content.Insert(end2LeftStrIndex, AorTxt.Format("    self:AddBindValue(self.BVKey.{0})\n", m_InterBindValueNames[index].stringValue));
                            }
                        }
                        else
                        {
                            switch ((LuaBindValue.BindValueType)m_InterBindValueTypeNames[index].enumValueIndex)
                            {
                                case LuaBindValue.BindValueType.Int32: content = content.Insert(end2LeftStrIndex, AorTxt.Format("    self:AddBindValue(self.BVKey.{0}, tonumber(self._args.env.cs.BindValues[{1}].Variant))\n", m_InterBindValueNames[index].stringValue, index)); break;
                                case LuaBindValue.BindValueType.Float: content = content.Insert(end2LeftStrIndex, AorTxt.Format("    self:AddBindValue(self.BVKey.{0}, tonumber(self._args.env.cs.BindValues[{1}].Variant))\n", m_InterBindValueNames[index].stringValue, index)); break;
                                case LuaBindValue.BindValueType.String: content = content.Insert(end2LeftStrIndex, AorTxt.Format("    self:AddBindValue(self.BVKey.{0}, self._args.env.cs.BindValues[{1}].Variant)\n", m_InterBindValueNames[index].stringValue, index)); break;
                                case LuaBindValue.BindValueType.Boolean: content = content.Insert(end2LeftStrIndex, AorTxt.Format("    self:AddBindValue(self.BVKey.{0}, self._args.env.cs.BindValues[{1}].Variant == 'true')\n", m_InterBindValueNames[index].stringValue, index)); break;
                                case LuaBindValue.BindValueType.Table: content = content.Insert(end2LeftStrIndex, AorTxt.Format("    self:AddBindValue(self.BVKey.{0}, TableDecode(self._args.env.cs.BindValues[{1}].Variant))\n", m_InterBindValueNames[index].stringValue, index)); break;
                                case LuaBindValue.BindValueType.Array: content = content.Insert(end2LeftStrIndex, AorTxt.Format("    self:AddBindValue(self.BVKey.{0}, TableDecode(self._args.env.cs.BindValues[{1}].Variant))\n", m_InterBindValueNames[index].stringValue, index)); break;
                                case LuaBindValue.BindValueType.Any: content = content.Insert(end2LeftStrIndex, AorTxt.Format("    self:AddBindValue(self.BVKey.{0})\n", m_InterBindValueNames[index].stringValue)); break;
                                case LuaBindValue.BindValueType.Trigger: content = content.Insert(end2LeftStrIndex, AorTxt.Format("    self:AddBindValue(self.BVKey.{0}, false, true)\n", m_InterBindValueNames[index].stringValue)); break;
                            }
                        }
                        end2LeftStrIndex = content.LastIndexOf(end2Str);
                    }
                }
                else
                {
                    content = content.Insert(end2LeftStrIndex, AorTxt.Format("    -- 无添加绑定数据成员\n"));
                    end2LeftStrIndex = content.LastIndexOf(end2Str);
                }

                // 再插入1号区域的注入提示信息（先2后1，1的标记位置不变）
                content = content.Insert(start1RightStrIndex, AorTxt.Format("---@class {0} : {1}\n", luaName, luaSuperScriptName));

                // 更新绑定值Key名称定义
                int bvKeyDefIndex = content.LastIndexOf(AorTxt.Format("{0}.BVKey = ", luaName));
                int defCharCount = 0;
                while (content[bvKeyDefIndex + defCharCount] != '\n')
                {
                    defCharCount++;
                }
                content = content.Remove(bvKeyDefIndex, defCharCount);
                string bvkeyContent = "{";
                foreach (var bindValueName in m_InterBindValueNames)
                {
                    bvkeyContent = AorTxt.Format("{0}{1} = \"{2}\", ", bvkeyContent, bindValueName.stringValue, bindValueName.stringValue);
                }
                bvkeyContent += "}";
                content = content.Insert(bvKeyDefIndex, AorTxt.Format("{0}.BVKey = {1}", luaName, bvkeyContent));

                // 更新类的头部定义
                int classNameDefIndex = content.LastIndexOf(AorTxt.Format("local {0} = class('{1}', import", luaName, luaName));
                defCharCount = 0;
                while (content[classNameDefIndex + defCharCount] != '\n')
                {
                    defCharCount++;
                }
                content = content.Remove(classNameDefIndex, defCharCount);
                content = content.Insert(classNameDefIndex, AorTxt.Format("local {0} = class('{1}', import('{2}'))", luaName, luaName, luaSuperScriptName));

                // 追加Proc定义
                if (m_UseProc.boolValue)
                {
                    if (!content.Contains(AorTxt.Format("function {0}:Proc()", luaName)))
                    {
                        int onDestroyDefIndex = content.LastIndexOf(AorTxt.Format("---销毁\r\n---@type fun():void\r\nfunction {0}:OnDestroy()\r\n", luaName));
                        string procDef = AorTxt.Format("{0}\n{1}\n{2}\n{3}\n\nend\n\n", AorTxt.Format("---心跳（自定义）"), AorTxt.Format("---@type fun():void"), AorTxt.Format("function {0}:Proc()", luaName), AorTxt.Format("    {0}.super.Proc(self)", luaName));
                        content = content.Insert(onDestroyDefIndex, procDef);
                    }
                }

                StringBuilder stringBuilderCodeLines = new StringBuilder(content, endCommentStrIndex, content.Length - endCommentStrIndex, content.Length * 2);
                return stringBuilderCodeLines;
            }
            return null;
        }


        /// <summary>
        /// 刷新index
        /// </summary>
        /// <param name="luaName"></param>
        /// <param name="content"></param>
        /// <param name="endCommentStrIndex"></param>
        /// <param name="start1RightStrIndex"></param>
        /// <param name="end1LeftStrIndex"></param>
        /// <param name="end1RightStrIndex"></param>
        /// <param name="start2RightStrIndex"></param>
        /// <param name="end2LeftStrIndex"></param>
        /// <param name="start3RightStrIndex"></param>
        /// <param name="end3LeftStrIndex"></param>
        /// <param name="start4RightStrIndex"></param>
        /// <param name="end4LeftStrIndex"></param>
        /// <param name="start5LeftStrIndex"></param>
        /// <param name="start5RightStrIndex"></param>
        /// <param name="end5LeftStrIndex"></param>
        /// <param name="start6RightStrIndex"></param>
        /// <param name="end6LeftStrIndex"></param>
        void RefreshIndexes(string luaName, string content, 
            out int endCommentStrIndex, out int start1RightStrIndex, out int end1LeftStrIndex, out int end1RightStrIndex, 
            out int start2RightStrIndex, out int end2LeftStrIndex, out int start3RightStrIndex, out int end3LeftStrIndex, 
            out int start4RightStrIndex, out int end4LeftStrIndex, out int start5LeftStrIndex, out int start5RightStrIndex, out int end5LeftStrIndex, 
            out int start6RightStrIndex, out int end6LeftStrIndex)
        {
            string commentStr = "--=====================================================================================================";
            string end1Str = AorTxt.Format("local {0} = class", luaName);
            string start2RightStr = "-- 2>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>";
            string end2Str = "-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<2";
            string start3RightStr = "-- 3>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>";
            string end3Str = "-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<3";
            string start4RightStr = "-- 4>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>";
            string end4Str = "-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<4";
            string start5RightStr = "-- 5>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>";
            string end5Str = "-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<5";
            string start6RightStr = "-- 6>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>";
            string end6Str = "-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<6";

            endCommentStrIndex = content.LastIndexOf(commentStr) + commentStr.Length + 4;
            start1RightStrIndex = endCommentStrIndex;
            end1LeftStrIndex = content.LastIndexOf(end1Str);
            end1RightStrIndex = content.LastIndexOf(end1Str) + end1Str.Length + 4;
            start2RightStrIndex = content.LastIndexOf(start2RightStr) + start2RightStr.Length + 1;
            end2LeftStrIndex = content.LastIndexOf(end2Str);
            start3RightStrIndex = content.LastIndexOf(start3RightStr) + start3RightStr.Length + 1;
            end3LeftStrIndex = content.LastIndexOf(end3Str);
            start4RightStrIndex = content.LastIndexOf(start4RightStr) + start4RightStr.Length + 1;
            end4LeftStrIndex = content.LastIndexOf(end4Str);
            start5RightStrIndex = content.LastIndexOf(start5RightStr) + start5RightStr.Length + 1;
            start5LeftStrIndex = start5RightStrIndex - start5RightStr.Length - 1;
            end5LeftStrIndex = content.LastIndexOf(end5Str);
            start6RightStrIndex = content.LastIndexOf(start6RightStr) + start6RightStr.Length + 1;
            end6LeftStrIndex = content.LastIndexOf(end6Str);
        }

        /// <summary>
        /// 根据注入对象名称获取其类型名称
        /// </summary>
        /// <param name="injectionName"></param>
        /// <returns></returns>
        private string GetInjectionTypeByName(string injectionName)
        {
            if(injectionName.IndexOf("[") >= 0)
            {
                injectionName = injectionName.Substring(0, injectionName.IndexOf("["));
            }
            for (int index = 0; index < m_InterInjectionNames.Count; index++)
            {
                if(m_InterInjectionNames[index].stringValue == injectionName)
                {
                    return LuaInjection.LuaInjectionType[m_InterInjectionTypeNames[index].enumValueIndex];
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 根据绑定数据名称获取其类型名称
        /// </summary>
        /// <param name="bindValueName"></param>
        /// <returns></returns>
        private string GetBindValueTypeByName(string bindValueName)
        {
            for (int index = 0; index < m_InterBindValueNames.Count; index++)
            {
                if (m_InterBindValueNames[index].stringValue == bindValueName)
                {
                    return LuaBindValue.LuaBindValueType[m_InterBindValueTypeNames[index].enumValueIndex];
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 采集绑定数据与注入对象之间的各类信息
        /// </summary>
        /// <param name="luaBindValueOnInjectionNames"></param>
        /// <param name="luaBindValueOnInjectionWays"></param>
        /// <param name="luaBindValueOnInjectionSides"></param>
        /// <param name="luaBindValueComments"></param>
        /// <param name="luaBindValueFunctionNames"></param>
        /// <param name="luaBindValueTypeNames"></param>
        private void CollectBindingInfosOnInjections(out SortedDictionary<string, List<string>> luaBindValueOnInjectionNames, out SortedDictionary<string, List<string>> luaBindValueOnInjectionWays, out SortedDictionary<string, List<string>> luaBindValueOnInjectionSides, out SortedDictionary<string, string> luaBindValueComments, out SortedDictionary<string, string> luaBindValueFunctionNames, out SortedDictionary<string, string> luaBindValueTypeNames)
        {
            // 对绑定数据与注入对象之间对应名称信息进行采集<bindValueName, 该值绑定的所有injection的名称集合>
            luaBindValueOnInjectionNames = new SortedDictionary<string, List<string>>();
            // 对绑定数据与注入对象之间对应绑定方式信息进行采集<bindValueName, 该值绑定的所有injection的绑定方式集合>
            luaBindValueOnInjectionWays = new SortedDictionary<string, List<string>>();
            // 对绑定数据与注入对象之间对应绑定单双项信息进行采集<bindValueName, 该值绑定的所有injection的绑定单双向集合>
            luaBindValueOnInjectionSides = new SortedDictionary<string, List<string>>();
            // 对绑定数据监听变化回调方法中的注释内容进行采集
            luaBindValueComments = new SortedDictionary<string, string>();
            // 对绑定数据监听变化回调方法中的方法名称进行采集
            luaBindValueFunctionNames = new SortedDictionary<string, string>();
            // 对绑定数据监听变化回调方法中的参数类型进行采集
            luaBindValueTypeNames = new SortedDictionary<string, string>();
            for (int index = 0; index < m_InterBindValueOnInjections.Count; index++)
            {
                luaBindValueOnInjectionNames.Add(m_InterBindValueNames[index].stringValue, new List<string>());
                luaBindValueOnInjectionWays.Add(m_InterBindValueNames[index].stringValue, new List<string>());
                luaBindValueOnInjectionSides.Add(m_InterBindValueNames[index].stringValue, new List<string>());
                if (!string.IsNullOrEmpty(m_InterBindValueOnInjections[index].stringValue))
                {
                    List<string> names = new List<string>();
                    List<string> ways = new List<string>();
                    List<string> sides = new List<string>();
                    string[] injectionDescs = m_InterBindValueOnInjections[index].stringValue.Replace(" ", string.Empty).Split(',');
                    for (int descIndex = 0; descIndex < injectionDescs.Length; descIndex++)
                    {
                        string[] infos = injectionDescs[descIndex].Split('^');
                        names.Add(infos[0]);
                        ways.Add(infos[1]);
                        sides.Add(infos[2]);
                    }
                    luaBindValueOnInjectionNames[m_InterBindValueNames[index].stringValue].AddRange(names);
                    luaBindValueOnInjectionWays[m_InterBindValueNames[index].stringValue].AddRange(ways);
                    luaBindValueOnInjectionSides[m_InterBindValueNames[index].stringValue].AddRange(sides);
                }
            }
            for (int index = 0; index < m_InterBindValueNames.Count; index++)
            {
                string bindValueName = m_InterBindValueNames[index].stringValue;
                string bindValueComment = m_InterBindValueComments[index].stringValue;
                luaBindValueComments.Add(bindValueName, $"绑定数据-{bindValueName}-{bindValueComment}-变化监听回调");
                luaBindValueTypeNames.Add(bindValueName, GetBindValueTypeByName(bindValueName));
                luaBindValueFunctionNames.Add(bindValueName, $"On{bindValueName}BindValueChanged");
            }
        }

        /// <summary>
        /// 获取数据绑定时用到的单个注入条目的所有路径信息
        /// </summary>
        /// <param name="index">注入条目index</param>
        /// <returns></returns>
        private List<string> GetBindingInjectionPaths(int index)
        {
            List<string> paths = new List<string>();
            List<string> middlePathDescs = new List<string>();
            if (m_InterInjectionIsArrays[index].boolValue)
            {
                for (int elementIndex = 1; elementIndex <= m_InterInjectionElementsObjs[index].arraySize; elementIndex++)
                {
                    middlePathDescs.Add($"/[{elementIndex}]");
                }
            }
            else
            {
                middlePathDescs.Add(string.Empty);
            }

            switch ((LuaInjection.InjectionType)m_InterInjectionTypeNames[index].enumValueIndex)
            {
                case LuaInjection.InjectionType.GameObject:
                    foreach(string middlePathDesc in middlePathDescs)
                    {
                        paths.Add(AorTxt.Format("{0}{1}/Active/One", m_InterInjectionNames[index].stringValue, middlePathDesc));
                    }
                    break;
                case LuaInjection.InjectionType.Transform:
                    break;
                case LuaInjection.InjectionType.RectTransform:
                    break;
                case LuaInjection.InjectionType.UI_Text:
                    foreach (string middlePathDesc in middlePathDescs)
                    {
                        paths.Add(AorTxt.Format("{0}{1}/Active/One", m_InterInjectionNames[index].stringValue, middlePathDesc));
                        paths.Add(AorTxt.Format("{0}{1}/Content/One", m_InterInjectionNames[index].stringValue, middlePathDesc));
                        paths.Add(AorTxt.Format("{0}{1}/Color/One", m_InterInjectionNames[index].stringValue, middlePathDesc));
                    }
                    break;
                case LuaInjection.InjectionType.UI_Text_TextMeshPro:
                    foreach (string middlePathDesc in middlePathDescs)
                    {
                        paths.Add(AorTxt.Format("{0}{1}/Active/One", m_InterInjectionNames[index].stringValue, middlePathDesc));
                        paths.Add(AorTxt.Format("{0}{1}/Content/One", m_InterInjectionNames[index].stringValue, middlePathDesc));
                        paths.Add(AorTxt.Format("{0}{1}/Color/One", m_InterInjectionNames[index].stringValue, middlePathDesc));
                    }
                    break;
                case LuaInjection.InjectionType.UI_Dropdown_TextMeshPro:
                    foreach (string middlePathDesc in middlePathDescs)
                    {
                        paths.Add(AorTxt.Format("{0}{1}/Active/One", m_InterInjectionNames[index].stringValue, middlePathDesc));
                        paths.Add(AorTxt.Format("{0}{1}/Interactable/One", m_InterInjectionNames[index].stringValue, middlePathDesc));
                        paths.Add(AorTxt.Format("{0}{1}/Content/One", m_InterInjectionNames[index].stringValue, middlePathDesc));
                        paths.Add(AorTxt.Format("{0}{1}/Content/Two", m_InterInjectionNames[index].stringValue, middlePathDesc));
                        paths.Add(AorTxt.Format("{0}{1}/Color/One", m_InterInjectionNames[index].stringValue, middlePathDesc));
                    }
                    break;
                case LuaInjection.InjectionType.UI_InputField_TextMeshPro:
                    foreach (string middlePathDesc in middlePathDescs)
                    {
                        paths.Add(AorTxt.Format("{0}{1}/Active/One", m_InterInjectionNames[index].stringValue, middlePathDesc));
                        paths.Add(AorTxt.Format("{0}{1}/Interactable/One", m_InterInjectionNames[index].stringValue, middlePathDesc));
                        paths.Add(AorTxt.Format("{0}{1}/Content/One", m_InterInjectionNames[index].stringValue, middlePathDesc));
                        paths.Add(AorTxt.Format("{0}{1}/Content/Two", m_InterInjectionNames[index].stringValue, middlePathDesc));
                        paths.Add(AorTxt.Format("{0}{1}/Color/One", m_InterInjectionNames[index].stringValue, middlePathDesc));
                    }
                    break;
                case LuaInjection.InjectionType.UI_TextPicMixed:
                    foreach (string middlePathDesc in middlePathDescs)
                    {
                        paths.Add(AorTxt.Format("{0}{1}/Active/One", m_InterInjectionNames[index].stringValue, middlePathDesc));
                        paths.Add(AorTxt.Format("{0}{1}/Content/One", m_InterInjectionNames[index].stringValue, middlePathDesc));
                    }
                    break;
                case LuaInjection.InjectionType.UI_Image:
                    foreach (string middlePathDesc in middlePathDescs)
                    {
                        paths.Add(AorTxt.Format("{0}{1}/Active/One", m_InterInjectionNames[index].stringValue, middlePathDesc));
                        paths.Add(AorTxt.Format("{0}{1}/Content/One", m_InterInjectionNames[index].stringValue, middlePathDesc));
                    }
                    break;
                case LuaInjection.InjectionType.UI_Button:
                    foreach (string middlePathDesc in middlePathDescs)
                    {
                        paths.Add(AorTxt.Format("{0}{1}/Active/One", m_InterInjectionNames[index].stringValue, middlePathDesc));
                        paths.Add(AorTxt.Format("{0}{1}/Interactable/One", m_InterInjectionNames[index].stringValue, middlePathDesc));
                        paths.Add(AorTxt.Format("{0}{1}/Content/One", m_InterInjectionNames[index].stringValue, middlePathDesc));
                    }
                    break;
                case LuaInjection.InjectionType.UI_Scrollbar:
                    foreach (string middlePathDesc in middlePathDescs)
                    {
                        paths.Add(AorTxt.Format("{0}{1}/Active/One", m_InterInjectionNames[index].stringValue, middlePathDesc));
                        paths.Add(AorTxt.Format("{0}{1}/Interactable/One", m_InterInjectionNames[index].stringValue, middlePathDesc));
                    }
                    break;
                case LuaInjection.InjectionType.UI_ScrollRect:
                    foreach (string middlePathDesc in middlePathDescs)
                    {
                        paths.Add(AorTxt.Format("{0}{1}/Active/One", m_InterInjectionNames[index].stringValue, middlePathDesc));
                    }
                    break;
                case LuaInjection.InjectionType.UI_Toggle:
                    foreach (string middlePathDesc in middlePathDescs)
                    {
                        paths.Add(AorTxt.Format("{0}{1}/Active/One", m_InterInjectionNames[index].stringValue, middlePathDesc));
                        paths.Add(AorTxt.Format("{0}{1}/Interactable/One", m_InterInjectionNames[index].stringValue, middlePathDesc));
                        paths.Add(AorTxt.Format("{0}{1}/Content/One", m_InterInjectionNames[index].stringValue, middlePathDesc));
                        paths.Add(AorTxt.Format("{0}{1}/Content/Two", m_InterInjectionNames[index].stringValue, middlePathDesc));
                    }
                    break;
                case LuaInjection.InjectionType.UI_Slider:
                    foreach (string middlePathDesc in middlePathDescs)
                    {
                        paths.Add(AorTxt.Format("{0}{1}/Active/One", m_InterInjectionNames[index].stringValue, middlePathDesc));
                        paths.Add(AorTxt.Format("{0}{1}/Interactable/One", m_InterInjectionNames[index].stringValue, middlePathDesc));
                        paths.Add(AorTxt.Format("{0}{1}/Content/One", m_InterInjectionNames[index].stringValue, middlePathDesc));
                        paths.Add(AorTxt.Format("{0}{1}/Content/Two", m_InterInjectionNames[index].stringValue, middlePathDesc));
                    }
                    break;
                case LuaInjection.InjectionType.UI_Dropdown:
                    foreach (string middlePathDesc in middlePathDescs)
                    {
                        paths.Add(AorTxt.Format("{0}{1}/Active/One", m_InterInjectionNames[index].stringValue, middlePathDesc));
                        paths.Add(AorTxt.Format("{0}{1}/Interactable/One", m_InterInjectionNames[index].stringValue, middlePathDesc));
                        paths.Add(AorTxt.Format("{0}{1}/Content/One", m_InterInjectionNames[index].stringValue, middlePathDesc));
                        paths.Add(AorTxt.Format("{0}{1}/Content/Two", m_InterInjectionNames[index].stringValue, middlePathDesc));
                        paths.Add(AorTxt.Format("{0}{1}/Color/One", m_InterInjectionNames[index].stringValue, middlePathDesc));
                    }
                    break;
                case LuaInjection.InjectionType.UI_InputField:
                    foreach (string middlePathDesc in middlePathDescs)
                    {
                        paths.Add(AorTxt.Format("{0}{1}/Active/One", m_InterInjectionNames[index].stringValue, middlePathDesc));
                        paths.Add(AorTxt.Format("{0}{1}/Interactable/One", m_InterInjectionNames[index].stringValue, middlePathDesc));
                        paths.Add(AorTxt.Format("{0}{1}/Content/One", m_InterInjectionNames[index].stringValue, middlePathDesc));
                        paths.Add(AorTxt.Format("{0}{1}/Content/Two", m_InterInjectionNames[index].stringValue, middlePathDesc));
                        paths.Add(AorTxt.Format("{0}{1}/Color/One", m_InterInjectionNames[index].stringValue, middlePathDesc));
                    }
                    break;
                case LuaInjection.InjectionType.UI_Tree:
                    foreach (string middlePathDesc in middlePathDescs)
                    {
                        paths.Add(AorTxt.Format("{0}{1}/Active/One", m_InterInjectionNames[index].stringValue, middlePathDesc));
                    }
                    break;
                case LuaInjection.InjectionType.UI_ListView:
                    foreach (string middlePathDesc in middlePathDescs)
                    {
                        paths.Add(AorTxt.Format("{0}{1}/Active/One", m_InterInjectionNames[index].stringValue, middlePathDesc));
                        paths.Add(AorTxt.Format("{0}{1}/Interactable/One", m_InterInjectionNames[index].stringValue, middlePathDesc));
                        paths.Add(AorTxt.Format("{0}{1}/ItemTotalNum/One", m_InterInjectionNames[index].stringValue, middlePathDesc));
                        paths.Add(AorTxt.Format("{0}{1}/ScrollTo/One", m_InterInjectionNames[index].stringValue, middlePathDesc));
                        paths.Add(AorTxt.Format("{0}{1}/AddItemNum/One", m_InterInjectionNames[index].stringValue, middlePathDesc));
                    }
                    break;
                case LuaInjection.InjectionType.UI_SwitchButton:
                    foreach (string middlePathDesc in middlePathDescs)
                    {
                        paths.Add(AorTxt.Format("{0}{1}/Active/One", m_InterInjectionNames[index].stringValue, middlePathDesc));
                        paths.Add(AorTxt.Format("{0}{1}/Interactable/One", m_InterInjectionNames[index].stringValue, middlePathDesc));
                        paths.Add(AorTxt.Format("{0}{1}/Content/One", m_InterInjectionNames[index].stringValue, middlePathDesc));
                        paths.Add(AorTxt.Format("{0}{1}/Content/Two", m_InterInjectionNames[index].stringValue, middlePathDesc));
                    }
                    break;
                default: break;
            }
            foreach (string middlePathDesc in middlePathDescs)
            {
                paths.Add(AorTxt.Format("{0}{1}/anchoredPosition/One", m_InterInjectionNames[index].stringValue, middlePathDesc));
                paths.Add(AorTxt.Format("{0}{1}/anchoredPosition3D/One", m_InterInjectionNames[index].stringValue, middlePathDesc));
                paths.Add(AorTxt.Format("{0}{1}/offsetMin/One", m_InterInjectionNames[index].stringValue, middlePathDesc));
                paths.Add(AorTxt.Format("{0}{1}/offsetMax/One", m_InterInjectionNames[index].stringValue, middlePathDesc));
                paths.Add(AorTxt.Format("{0}{1}/anchorMin/One", m_InterInjectionNames[index].stringValue, middlePathDesc));
                paths.Add(AorTxt.Format("{0}{1}/anchorMax/One", m_InterInjectionNames[index].stringValue, middlePathDesc));
                paths.Add(AorTxt.Format("{0}{1}/pivot/One", m_InterInjectionNames[index].stringValue, middlePathDesc));
                paths.Add(AorTxt.Format("{0}{1}/sizeDelta/One", m_InterInjectionNames[index].stringValue, middlePathDesc));
                paths.Add(AorTxt.Format("{0}{1}/localEulerAngles/One", m_InterInjectionNames[index].stringValue, middlePathDesc));
                paths.Add(AorTxt.Format("{0}{1}/eulerAngles/One", m_InterInjectionNames[index].stringValue, middlePathDesc));
                paths.Add(AorTxt.Format("{0}{1}/localScale/One", m_InterInjectionNames[index].stringValue, middlePathDesc));
            }
            return paths;
        }
    }
}
