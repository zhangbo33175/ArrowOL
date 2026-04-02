
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
        private SerializedProperty m_LuaScriptNamesNone = null;
        private SerializedProperty m_LuaSuperScriptNamesNone = null;
        private List<string> m_TextLuaSuperScriptNamesNone = null;

        /// <summary>
        /// 初始化None设计模式
        /// </summary>
        private void InitPatternNone()
        {
            m_LuaScriptNamesNone = serializedObject.FindProperty("m_LuaScriptNamesNone");
            if (m_LuaScriptNamesNone.arraySize == 0)
            {
                for (int index = 0; index < (int)NonePatternType.TotalNum; index++)
                {
                    m_LuaScriptNamesNone.InsertArrayElementAtIndex(index);
                    m_LuaScriptNamesNone.GetArrayElementAtIndex(index).stringValue = string.Empty;
                }
            }
            m_LuaSuperScriptNamesNone = serializedObject.FindProperty("m_LuaSuperScriptNamesNone");
            if (m_LuaSuperScriptNamesNone.arraySize > (int)NonePatternType.TotalNum)
            {
                m_LuaSuperScriptNamesNone.ClearArray();
            }
            if (m_LuaSuperScriptNamesNone.arraySize == 0)
            {
                for (int index = 0; index < (int)NonePatternType.TotalNum; index++)
                {
                    m_LuaSuperScriptNamesNone.InsertArrayElementAtIndex(index);
                }
            }
            for (int index = 0; index < (int)NonePatternType.TotalNum; index++)
            {
                if (string.IsNullOrEmpty(m_LuaSuperScriptNamesNone.GetArrayElementAtIndex(index).stringValue))
                {
                    m_LuaSuperScriptNamesNone.GetArrayElementAtIndex(index).stringValue = "LuaBehaviourSuper";
                }
            }
            m_TextLuaSuperScriptNamesNone= new List<string>();
            for (int index = 0; index < (int)NonePatternType.TotalNum; index++)
            {
                m_TextLuaSuperScriptNamesNone.Add(m_LuaSuperScriptNamesNone.GetArrayElementAtIndex(index).stringValue);
            }

            serializedObject.ApplyModifiedProperties();

        }

        /// <summary>
        /// None设计模式Lua名称GUI
        /// </summary>
        private void OnPatternNoneLuaScriptNameInspectorGUI()
        {
            SerializedProperty luaScriptName = m_LuaScriptNamesNone.GetArrayElementAtIndex((int)NonePatternType.Default);

            if (string.IsNullOrEmpty(luaScriptName.stringValue)) GUI.color = Color.red;
            luaScriptName.stringValue = EditorGUILayout.TextField("Lua脚本名称", luaScriptName.stringValue);
            GUI.color = Color.white;

            if (string.IsNullOrEmpty(m_TextLuaSuperScriptNamesNone[(int)NonePatternType.Default])) GUI.color = Color.red;
            string luaSuperScriptName = EditorGUILayout.TextField("Lua脚本名称（父类）", m_TextLuaSuperScriptNamesNone[(int)NonePatternType.Default]);
            GUI.color = Color.white;

            m_TextLuaSuperScriptNamesNone[(int)NonePatternType.Default] = luaSuperScriptName;
            m_LuaSuperScriptNamesNone.GetArrayElementAtIndex((int)NonePatternType.Default).stringValue = luaSuperScriptName;

            if (!string.IsNullOrEmpty(luaScriptName.stringValue) && !luaScriptName.stringValue.StartsWith(".lua") && !luaScriptName.stringValue.EndsWith(".lua") &&
                !string.IsNullOrEmpty(luaSuperScriptName) && !luaSuperScriptName.StartsWith(".lua") && !luaSuperScriptName.EndsWith(".lua"))
            {
                m_LuaCanGenerate = true;
            }
        }

        /// <summary>
        /// 生成None设计模式下Lua注释行信息
        /// </summary>
        private StringBuilder GeneratePatternNoneCommentLines()
        {
            StringBuilder stringBuilder = new StringBuilder();

            string luaScriptName = m_LuaScriptNamesNone.GetArrayElementAtIndex((int)NonePatternType.Default).stringValue;
            string luaAuthor = m_LuaAuthorName.stringValue;
            string luaDescript = m_LuaDescript.stringValue;

            stringBuilder.AppendLine(AorTxt.Format("--====================================================================================================="))
                         .AppendLine(AorTxt.Format("-- (c) copyright 2026 - 2030, Honor.Game"))
                         .AppendLine(AorTxt.Format("-- All Rights Reserved."))
                         .AppendLine(AorTxt.Format("-- ----------------------------------------------------------------------------------------------------"))
                         .AppendLine(AorTxt.Format("-- filename:  {0}.lua", luaScriptName))
                         .AppendLine(AorTxt.Format("-- author:    {0}", luaAuthor))
                         .AppendLine(AorTxt.Format("-- descrip:   {0}", AorTxt.Format("{0}{1}", luaDescript, string.Empty)))
                         .AppendLine(AorTxt.Format("--====================================================================================================="))
                         .AppendLine(AorTxt.Format(""));
            return stringBuilder;
        }

        /// <summary>
        /// 生成None设计模式下Lua空行
        /// </summary>
        private StringBuilder GeneratePatternNoneEmptyCodeLines()
        {
            StringBuilder stringBuilder = new StringBuilder();

            string luaScriptName = m_LuaScriptNamesNone.GetArrayElementAtIndex((int)NonePatternType.Default).stringValue;
            string luaSuperScriptName = m_LuaSuperScriptNamesNone.GetArrayElementAtIndex((int)NonePatternType.Default).stringValue;
            string luaAuthor = m_LuaAuthorName.stringValue;
            string luaDescript = m_LuaDescript.stringValue;
            string luaName = luaScriptName;

            stringBuilder.AppendLine(AorTxt.Format("---@class {0} : {1}", luaName, luaSuperScriptName));
            stringBuilder.AppendLine(AorTxt.Format("---@field cs Honor.Runtime.LuaBehaviour @LuaBehaviour"));

            // 采集InfoEx辅助信息
            List<string> luaInjectNames = null;
            List<string> luaInjectComments = null;
            List<string> luaInjectFunctionNames = null;
            List<string> luaInjectFunctionParams = null;
            List<string> luaInjectCmds = null;
            CollectInfoExInfos(out luaInjectNames, out luaInjectComments, out luaInjectFunctionNames, out luaInjectFunctionParams, out luaInjectCmds);

            // 生成注入对象的定义声明
            if (m_Injections != null && m_Injections.arraySize > 0)
            {
                for (int index = 0; index < m_Injections.arraySize; index++)
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

                    stringBuilder.AppendLine(AorTxt.Format("---@field {0}{1}{2}{3}{4}", fieldName, typeName, isValid, infoEx, comment));
                }
            }

            stringBuilder.AppendLine(AorTxt.Format("local {0} = class('{1}', import('{2}'))", luaName, luaName, luaSuperScriptName));
            stringBuilder.AppendLine(AorTxt.Format(""));
            stringBuilder.AppendLine(AorTxt.Format("---构造函数"));
            stringBuilder.AppendLine(AorTxt.Format("---@type fun(args:table):void"));
            stringBuilder.AppendLine(AorTxt.Format("---@param args table @自定义参数"));
            stringBuilder.AppendLine(AorTxt.Format("function {0}:ctor(args)", luaName));
            stringBuilder.AppendLine(AorTxt.Format("    {0}.super.ctor(self, args)", luaName));
            stringBuilder.AppendLine(AorTxt.Format(""));
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
            stringBuilder.AppendLine(AorTxt.Format("---注册监听代码处（注：所有成员函数的注册监听代码都写在这里，成员函数作为监听回调时不需要手动注销，父类会自动辅助完成事件的注销操作。）"));
            stringBuilder.AppendLine(AorTxt.Format("---@type fun():void"));
            stringBuilder.AppendLine(AorTxt.Format("function {0}:OnAddListeners()", luaName));
            stringBuilder.AppendLine(AorTxt.Format("    {0}.super.OnAddListeners(self)", luaName));
            stringBuilder.AppendLine(AorTxt.Format(""));
            stringBuilder.AppendLine(AorTxt.Format("end"));
            stringBuilder.AppendLine(AorTxt.Format(""));
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
                         .AppendLine(AorTxt.Format(""))
                         .AppendLine(AorTxt.Format("---开始"))
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

            stringBuilder.AppendLine(AorTxt.Format("---播放打开动画回调（可重写该父类方法）"))
                            .AppendLine(AorTxt.Format("---@type fun():void"))
                            .AppendLine(AorTxt.Format("function {0}:OnPlayOpenAnimation()", luaName))
                            .AppendLine(AorTxt.Format("    {0}.super.OnPlayOpenAnimation(self)", luaName))
                            .AppendLine(AorTxt.Format("end"))
                            .AppendLine(AorTxt.Format(""));
            stringBuilder.AppendLine(AorTxt.Format("---播放关闭动画回调（可重写该父类方法）"))
                            .AppendLine(AorTxt.Format("---@type fun():void"))
                            .AppendLine(AorTxt.Format("function {0}:OnPlayCloseAnimation()", luaName))
                            .AppendLine(AorTxt.Format("    {0}.super.OnPlayCloseAnimation(self)", luaName))
                            .AppendLine(AorTxt.Format("end"))
                            .AppendLine(AorTxt.Format(""));

            if (m_PrefabType.enumValueIndex == (int)Runtime.PrefabType.UI)
            {
                stringBuilder.AppendLine(AorTxt.Format("---被追加的N层UI子节点销毁回调"))
                                .AppendLine(AorTxt.Format("---@type fun(luaClass: XLua.LuaTable):void"))
                                .AppendLine(AorTxt.Format("---@param luaClass XLua.LuaTable @luaClass"))
                                .AppendLine(AorTxt.Format("function {0}:OnAddedUIDestroyed(luaClass)", luaName))
                                .AppendLine(AorTxt.Format("    {0}.super.OnAddedUIDestroyed(self, luaClass)", luaName))
                                .AppendLine(AorTxt.Format("end"))
                                .AppendLine(AorTxt.Format(""));
            }

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
                             .AppendLine(AorTxt.Format("---@type fun({0}):void", funcDesc))
                             .AppendLine(AorTxt.Format("function {0}:{1}({2})", luaName, luaInjectFunctionNames[index], funcParams));

                if (luaInjectFunctionNames[index].EndsWith("GettingItem"))
                {
                    stringBuilder.AppendLine(AorTxt.Format($"    if itemIndex < 0 or itemIndex >= self.{luaInjectNames[index]}.MaxItemNum then return nil end"));
                    stringBuilder.AppendLine(AorTxt.Format($"    local item = self.{luaInjectNames[index]}:NewListViewItem('Item')"));
                    stringBuilder.AppendLine(AorTxt.Format($"    if item.IsInitHandlerCalled == false then item.IsInitHandlerCalled = true end"));
                    stringBuilder.AppendLine(AorTxt.Format($"    return item"));
                }
                else
                {
                    stringBuilder.AppendLine(AorTxt.Format(""));
                }
                stringBuilder.AppendLine(AorTxt.Format("end"))
                             .AppendLine(AorTxt.Format(""));
            }

            stringBuilder.AppendLine(AorTxt.Format("return {0}", luaName));
            stringBuilder.AppendLine(AorTxt.Format(""));

            return stringBuilder;
        }

        /// <summary>
        /// 刷新None设计模式下已有Lua代码行
        /// </summary>
        /// <param name="fullPath">全路径</param>
        /// <returns></returns>
        private StringBuilder GeneratePatternNoneCodeLines(string fullPath)
        {
            string luaScriptName = m_LuaScriptNamesNone.GetArrayElementAtIndex((int)NonePatternType.Default).stringValue;
            string luaSuperScriptName = m_LuaSuperScriptNamesNone.GetArrayElementAtIndex((int)NonePatternType.Default).stringValue;
            string luaAuthor = m_LuaAuthorName.stringValue;
            string luaDescript = m_LuaDescript.stringValue;
            string luaName = luaScriptName;

            string commentStr = "--=====================================================================================================";
            string end1Str = AorTxt.Format("local {0} = class", luaName);
            string start2RightStr = "-- 2>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>";
            string end2Str = "-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<2";
            string start3RightStr = "-- 3>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>";
            string end3Str = "-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<3";

            string content = System.IO.File.ReadAllText(fullPath);

            int endCommentStrIndex = content.LastIndexOf(commentStr) + commentStr.Length + 4;
            int start1RightStrIndex = endCommentStrIndex;
            int end1LeftStrIndex = content.LastIndexOf(end1Str);
            int end1RightStrIndex = content.LastIndexOf(end1Str) + end1Str.Length + 4;
            int start2RightStrIndex = content.LastIndexOf(start2RightStr) + start2RightStr.Length + 1;
            int end2LeftStrIndex = content.LastIndexOf(end2Str);
            int start3RightStrIndex = content.LastIndexOf(start3RightStr) + start3RightStr.Length + 1;
            int end3LeftStrIndex = content.LastIndexOf(end3Str);

            // 移除1号区域的信息
            content = content.Remove(start1RightStrIndex, end1LeftStrIndex - start1RightStrIndex);

            // 刷新标记的位置
            endCommentStrIndex = content.LastIndexOf(commentStr) + commentStr.Length + 4;
            start1RightStrIndex = endCommentStrIndex;
            end1LeftStrIndex = content.LastIndexOf(end1Str);
            end1RightStrIndex = content.LastIndexOf(end1Str) + end1Str.Length + 4;
            start2RightStrIndex = content.LastIndexOf(start2RightStr) + start2RightStr.Length + 1;
            end2LeftStrIndex = content.LastIndexOf(end2Str);
            start3RightStrIndex = content.LastIndexOf(start3RightStr) + start3RightStr.Length + 1;
            end3LeftStrIndex = content.LastIndexOf(end3Str);

            // 移除3号区域的信息
            content = content.Remove(start3RightStrIndex, end3LeftStrIndex - start3RightStrIndex);

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
            start3RightStrIndex = content.LastIndexOf(start3RightStr) + start3RightStr.Length + 1;
            end3LeftStrIndex = content.LastIndexOf(end3Str);

            // 采集InfoEx辅助信息
            List<string> luaInjectNames = null;
            List<string> luaInjectComments = null;
            List<string> luaInjectFunctionNames = null;
            List<string> luaInjectFunctionParams = null;
            List<string> luaInjectCmds = null;
            CollectInfoExInfos(out luaInjectNames, out luaInjectComments, out luaInjectFunctionNames, out luaInjectFunctionParams, out luaInjectCmds);

            // 先插入3号区域的注销代码
            if (luaInjectFunctionNames.Count > 0)
            {
                for (int index = 0; index < luaInjectFunctionNames.Count; index++)
                {
                    content = content.Insert(end3LeftStrIndex, AorTxt.Format("    RemoveListener(self.{0}, '{1}', handler(self, self.{2}))\n", luaInjectNames[index], luaInjectCmds[index], luaInjectFunctionNames[index]));
                    end3LeftStrIndex = content.LastIndexOf(end3Str);
                }
            }
            else
            {
                content = content.Insert(end3LeftStrIndex, "-- 无自动注销内容。\n");
                end3LeftStrIndex = content.LastIndexOf(end3Str);
            }

            // 再插入2号区域的注册代码
            if (luaInjectFunctionNames.Count > 0)
            {
                for (int index = 0; index < luaInjectFunctionNames.Count; index++)
                {
                    content = content.Insert(end2LeftStrIndex, AorTxt.Format("    AddUIListenerFunction(self.{0}, '{1}', handler(self, self.{2}))\n", luaInjectNames[index], luaInjectCmds[index], luaInjectFunctionNames[index]));
                    end2LeftStrIndex = content.LastIndexOf(end2Str);
                }
            }
            else
            {
                content = content.Insert(end2LeftStrIndex, "-- 无自动注册内容。\n");
                end2LeftStrIndex = content.LastIndexOf(end2Str);
            }

            // 再插入1号区域的注入提示信息（先2后1，1的标记位置不变）
            if (m_Injections != null && m_Injections.arraySize > 0)
            {
                // 刷新标记的位置
                endCommentStrIndex = content.LastIndexOf(commentStr) + commentStr.Length + 4;
                start1RightStrIndex = endCommentStrIndex;
                end1LeftStrIndex = content.LastIndexOf(end1Str);
                end1RightStrIndex = content.LastIndexOf(end1Str) + end1Str.Length + 4;
                start2RightStrIndex = content.LastIndexOf(start2RightStr) + start2RightStr.Length + 1;
                end2LeftStrIndex = content.LastIndexOf(end2Str);
                start3RightStrIndex = content.LastIndexOf(start3RightStr) + start3RightStr.Length + 1;
                end3LeftStrIndex = content.LastIndexOf(end3Str);

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

            // 追加新的Function定义
            int returnRowIndex = content.LastIndexOf(AorTxt.Format("return {0}", luaName));
            string functionDef = string.Empty;
            if (m_UseProc.boolValue)
            {
                if (!content.Contains(AorTxt.Format("function {0}:Proc()", luaName)))
                {
                    functionDef = AorTxt.Format("{0}{1}\n{2}\n{3}\n{4}\n\nend\n\n", functionDef, AorTxt.Format("---心跳（自定义）"), AorTxt.Format("---@type fun():void"), AorTxt.Format("function {0}:Proc()", luaName), AorTxt.Format("    {0}.super.Proc(self)", luaName));
                }
            }

            // 判断OnAddedUIDestroyed是否存在
            if (m_PrefabType.enumValueIndex == (int)Runtime.PrefabType.UI)
            {
                if (!content.Contains(AorTxt.Format("function {0}:OnAddedUIDestroyed(luaClass)", luaName)))
                {
                    functionDef = AorTxt.Format("{0}{1}\n{2}\n{3}\n{4}\n{5}\n\nend\n\n", functionDef, AorTxt.Format("---被追加的N层UI子节点销毁回调"), AorTxt.Format("---@type fun(luaClass:XLua.LuaTable):void"), AorTxt.Format("---@param luaClass XLua.LuaTable @luaClass"), AorTxt.Format("function {0}:OnAddedUIDestroyed(luaClass)", luaName), AorTxt.Format("    {0}.super.OnAddedUIDestroyed(self, luaClass)", luaName));
                }
            }

            // UI交互回调
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
                    if (luaInjectFunctionNames[index].EndsWith("GettingItem"))
                    {
                        functionDef = AorTxt.Format("{0}{1}\n{2}\n{3}\n", functionDef, AorTxt.Format("---{0}", luaInjectComments[index]), AorTxt.Format("---@type fun({0}):void", funcDesc), checkContent);
                        functionDef = AorTxt.Format("{0}{1}\n", functionDef, $"    if itemIndex < 0 or itemIndex >= self.{luaInjectNames[index]}.MaxItemNum then return nil end");
                        functionDef = AorTxt.Format("{0}{1}\n", functionDef, $"    local item = self.{luaInjectNames[index]}:NewListViewItem('Item')");
                        functionDef = AorTxt.Format("{0}{1}\n", functionDef, $"    if item.IsInitHandlerCalled == false then item.IsInitHandlerCalled = true end");
                        functionDef = AorTxt.Format("{0}{1}\n", functionDef, $"    return item");
                        functionDef = AorTxt.Format("{0}end\n\n", functionDef);
                    }
                    else
                    {
                        functionDef = AorTxt.Format("{0}{1}\n{2}\n{3}\n\nend\n\n", functionDef, AorTxt.Format("---{0}", luaInjectComments[index]), AorTxt.Format("---@type fun({0}):void", funcDesc), checkContent);
                    }
                }
            }

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

            content = content.Insert(returnRowIndex, functionDef);

            // 更新类的头部定义
            int classNameDefIndex = content.LastIndexOf(AorTxt.Format("local {0} = class('{1}', import", luaName, luaName));
            int defCharCount = 0;
            while (content[classNameDefIndex + defCharCount] != '\n')
            {
                defCharCount++;
            }
            content = content.Remove(classNameDefIndex, defCharCount);
            content = content.Insert(classNameDefIndex, AorTxt.Format("local {0} = class('{1}', import('{2}'))", luaName, luaName, luaSuperScriptName));

            StringBuilder stringBuilderCodeLines = new StringBuilder(content, endCommentStrIndex, content.Length - endCommentStrIndex, content.Length * 2);
            return stringBuilderCodeLines;
        }

    }
}
