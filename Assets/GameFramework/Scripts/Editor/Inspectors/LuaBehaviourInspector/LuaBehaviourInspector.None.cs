/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  LuaBehaviourInspector.cs
 * author:    云毅
 * created:   2026
 * descrip:   LuaBehaviour 编辑器面板拓展 - None模式配置、Lua代码生成与刷新
 ***************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using Honor.Runtime;
using UnityEditor;
using UnityEngine;

namespace Honor.Editor
{
    /// <summary>
    /// LuaBehaviour 编辑器检视面板
    /// <remarks>partial 分部类，仅包含 None 模式核心逻辑</remarks>
    /// </summary>
    internal sealed partial class LuaBehaviourInspector : HonorComponentInspector
    {
        #region 序列化字段定义
        /// <summary>
        /// None模式Lua脚本名称数组
        /// </summary>
        private SerializedProperty m_LuaScriptNamesNone;

        /// <summary>
        /// None模式Lua父类脚本名称数组
        /// </summary>
        private SerializedProperty m_LuaSuperScriptNamesNone;

        /// <summary>
        /// 编辑器文本缓存：Lua父类脚本名称
        /// </summary>
        private List<string> m_TextLuaSuperScriptNamesNone;
        #endregion

        #region 初始化模块 - None 模式
        /// <summary>
        /// 初始化 None 设计模式
        /// <para>初始化序列化数组、设置默认值、同步数据到缓存</para>
        /// </summary>
        private void InitPatternNone()
        {
            m_LuaScriptNamesNone = serializedObject.FindProperty("m_LuaScriptNamesNone");
            if (m_LuaScriptNamesNone.arraySize == 0)
            {
                for (int i = 0; i < (int)NonePatternType.TotalNum; i++)
                {
                    m_LuaScriptNamesNone.InsertArrayElementAtIndex(i);
                    m_LuaScriptNamesNone.GetArrayElementAtIndex(i).stringValue = string.Empty;
                }
            }

            m_LuaSuperScriptNamesNone = serializedObject.FindProperty("m_LuaSuperScriptNamesNone");
            if (m_LuaSuperScriptNamesNone.arraySize > (int)NonePatternType.TotalNum)
            {
                m_LuaSuperScriptNamesNone.ClearArray();
            }

            if (m_LuaSuperScriptNamesNone.arraySize == 0)
            {
                for (int i = 0; i < (int)NonePatternType.TotalNum; i++)
                {
                    m_LuaSuperScriptNamesNone.InsertArrayElementAtIndex(i);
                }
            }

            for (int i = 0; i < (int)NonePatternType.TotalNum; i++)
            {
                if (string.IsNullOrEmpty(m_LuaSuperScriptNamesNone.GetArrayElementAtIndex(i).stringValue))
                {
                    m_LuaSuperScriptNamesNone.GetArrayElementAtIndex(i).stringValue = "LuaBehaviourSuper";
                }
            }

            m_TextLuaSuperScriptNamesNone = new List<string>();
            for (int i = 0; i < (int)NonePatternType.TotalNum; i++)
            {
                m_TextLuaSuperScriptNamesNone.Add(m_LuaSuperScriptNamesNone.GetArrayElementAtIndex(i).stringValue);
            }

            serializedObject.ApplyModifiedProperties();
        }
        #endregion

        #region 面板绘制模块 - None 模式
        /// <summary>
        /// None 模式 Lua 名称配置界面绘制
        /// <para>绘制脚本名称/父类输入框，提供非法输入红色提示</para>
        /// </summary>
        private void OnPatternNoneLuaScriptNameInspectorGUI()
        {
            SerializedProperty luaScript = m_LuaScriptNamesNone.GetArrayElementAtIndex((int)NonePatternType.Default);

            if (string.IsNullOrEmpty(luaScript.stringValue))
                GUI.color = Color.red;

            luaScript.stringValue = EditorGUILayout.TextField("Lua脚本名称", luaScript.stringValue);
            GUI.color = Color.white;

            string superName = m_TextLuaSuperScriptNamesNone[(int)NonePatternType.Default];
            if (string.IsNullOrEmpty(superName))
                GUI.color = Color.red;

            superName = EditorGUILayout.TextField("Lua脚本名称（父类）", superName);
            GUI.color = Color.white;

            m_TextLuaSuperScriptNamesNone[(int)NonePatternType.Default] = superName;
            m_LuaSuperScriptNamesNone.GetArrayElementAtIndex((int)NonePatternType.Default).stringValue = superName;

            // 合法性检查
            m_LuaCanGenerate = !string.IsNullOrEmpty(luaScript.stringValue)
                            && !luaScript.stringValue.EndsWith(".lua")
                            && !string.IsNullOrEmpty(superName)
                            && !superName.EndsWith(".lua");
        }
        #endregion

        #region 代码生成模块 - 注释头部
        /// <summary>
        /// 生成 None 模式 Lua 文件头部注释
        /// </summary>
        /// <returns>拼接完成的注释字符串构建器</returns>
        private StringBuilder GeneratePatternNoneCommentLines()
        {
            StringBuilder sb = new StringBuilder();
            string scriptName = m_LuaScriptNamesNone.GetArrayElementAtIndex((int)NonePatternType.Default).stringValue;
            string author = m_LuaAuthorName.stringValue;
            string desc = m_LuaDescript.stringValue;

            sb.AppendLine("--=====================================================================================================")
              .AppendLine("-- (c) copyright 2026 - 2030, Honor.Game")
              .AppendLine("-- All Rights Reserved.")
              .AppendLine("-- ----------------------------------------------------------------------------------------------------")
              .AppendLine($"-- filename:  {scriptName}.lua")
              .AppendLine($"-- author:    {author}")
              .AppendLine($"-- descrip:   {desc}")
              .AppendLine("--=====================================================================================================")
              .AppendLine();

            return sb;
        }
        #endregion

        #region 代码生成模块 - 全新 Lua 模板
        /// <summary>
        /// 生成 None 模式完整空白 Lua 代码模板
        /// <para>包含类定义、生命周期、注入字段、UI监听、碰撞函数等</para>
        /// </summary>
        /// <returns>完整Lua代码字符串构建器</returns>
        private StringBuilder GeneratePatternNoneEmptyCodeLines()
        {
            StringBuilder sb = new StringBuilder();
            string scriptName = m_LuaScriptNamesNone.GetArrayElementAtIndex((int)NonePatternType.Default).stringValue;
            string superName = m_LuaSuperScriptNamesNone.GetArrayElementAtIndex((int)NonePatternType.Default).stringValue;
            string desc = m_LuaDescript.stringValue;

            // 类声明
            sb.AppendLine($"---@class {scriptName} : {superName}");
            sb.AppendLine("---@field cs Honor.Runtime.LuaBehaviour @LuaBehaviour");

            // 采集注入信息
            CollectInfoExInfos(out List<string> injectNames, out List<string> injectComments,
                out List<string> funcNames, out List<string> funcParams, out List<string> cmds);

            // 注入字段
            if (m_Injections != null && m_Injections.arraySize > 0)
            {
                for (int i = 0; i < m_Injections.arraySize; i++)
                {
                    string comment = m_InterInjectionComments[i].stringValue ?? "";
                    string field = m_InterInjectionNames[i].stringValue;
                    string type = "";
                    string valid = "";
                    string infoEx = "";

                    if (m_InterInjectionIsArrays[i].boolValue)
                    {
                        type = $"{LuaInjection.LuaInjectionType[(int)m_InterInjectionTypeNames[i].enumValueIndex]}[]";
                        valid = "√";
                        for (int j = 0; j < m_InterInjectionElementsObjs[i].arraySize; j++)
                        {
                            if (m_InterInjectionElementsObjs[i].GetArrayElementAtIndex(j).objectReferenceValue == null)
                            {
                                valid = "×";
                                break;
                            }
                        }
                    }
                    else
                    {
                        type = LuaInjection.LuaInjectionType[(int)m_InterInjectionTypeNames[i].enumValueIndex];
                        valid = (m_InterInjectionTypeNames[i].enumValueIndex is < (int)LuaInjection.InjectionType.Int32 or > (int)LuaInjection.InjectionType.Boolean)
                            ? (m_InterInjectionObjs[i].objectReferenceValue != null ? "√" : "×")
                            : (string.IsNullOrEmpty(m_InterInjectionVariants[i].stringValue) ? "×" : m_InterInjectionVariants[i].stringValue);

                        infoEx = m_InterInjectionInfoExs[i].stringValue;
                    }

                    // 类型修正
                    if (type == "UnityEngine.GameObject" && !string.IsNullOrEmpty(infoEx))
                    {
                        Type t = Type.GetType(infoEx);
                        type = t?.FullName ?? "any";
                    }
                    else if (type == "Honor.Runtime.LuaBehaviour" && !string.IsNullOrEmpty(infoEx))
                    {
                        type = infoEx;
                    }

                    // 对齐排版
                    while (field.Length < 35) field += " ";
                    while (type.Length < 30) type += " ";
                    while (valid.Length < 10) valid += " ";
                    while (infoEx.Length < 15) infoEx += " ";

                    sb.AppendLine($"---@field {field}{type}{valid}{infoEx}{comment}");
                }
            }

            // 类定义
            sb.AppendLine($"local {scriptName} = class('{scriptName}', import('{superName}'))");
            sb.AppendLine();

            // 构造
            sb.AppendLine("---构造函数")
              .AppendLine("---@type fun(args:table):void")
              .AppendLine("---@param args table @自定义参数")
              .AppendLine($"function {scriptName}:ctor(args)")
              .AppendLine($"    {scriptName}.super.ctor(self, args)")
              .AppendLine()
              .AppendLine("end")
              .AppendLine();

            // Create
            sb.AppendLine("---创建函数")
              .AppendLine($"---@type fun(args:table):{scriptName}")
              .AppendLine("---@param args table @自定义参数")
              .AppendLine($"---@return {scriptName} @实例")
              .AppendLine($"function {scriptName}:Create(args)")
              .AppendLine($"    local obj = {scriptName}.new(args)")
              .AppendLine("    return obj")
              .AppendLine("end")
              .AppendLine();

            // 监听
            sb.AppendLine("---注册监听（自动注销）")
              .AppendLine("---@type fun():void")
              .AppendLine($"function {scriptName}:OnAddListeners()")
              .AppendLine($"    {scriptName}.super.OnAddListeners(self)")
              .AppendLine()
              .AppendLine("end")
              .AppendLine();

            // Awake
            sb.AppendLine("---唤醒")
              .AppendLine("---@type fun():void")
              .AppendLine($"function {scriptName}:Awake()")
              .AppendLine($"    {scriptName}.super.Awake(self)")
              .AppendLine("-- 2>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");

            if (funcNames.Count > 0)
            {
                for (int i = 0; i < funcNames.Count; i++)
                {
                    sb.AppendLine($"    AddUIListenerFunction(self.{injectNames[i]}, '{cmds[i]}', handler(self, self.{funcNames[i]}))");
                }
            }
            else
            {
                sb.AppendLine("-- 无自动注册内容。");
            }

            sb.AppendLine("-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<2")
              .AppendLine()
              .AppendLine("end")
              .AppendLine();

            // Start
            sb.AppendLine("---开始")
              .AppendLine("---@type fun():void")
              .AppendLine($"function {scriptName}:Start()")
              .AppendLine($"    {scriptName}.super.Start(self)")
              .AppendLine()
              .AppendLine("end")
              .AppendLine();

            // Proc
            if (m_UseProc.boolValue)
            {
                sb.AppendLine("---心跳（自定义）")
                  .AppendLine("---@type fun():void")
                  .AppendLine($"function {scriptName}:Proc()")
                  .AppendLine($"    {scriptName}.super.Proc(self)")
                  .AppendLine()
                  .AppendLine("end")
                  .AppendLine();
            }

            // Destroy
            sb.AppendLine("---销毁")
              .AppendLine("---@type fun():void")
              .AppendLine($"function {scriptName}:OnDestroy()")
              .AppendLine($"    {scriptName}.super.OnDestroy(self)")
              .AppendLine("-- 3>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");

            if (funcNames.Count > 0)
            {
                for (int i = 0; i < funcNames.Count; i++)
                {
                    sb.AppendLine($"    OnRemoveListener(self.{injectNames[i]}, '{cmds[i]}', handler(self, self.{funcNames[i]}))");
                }
            }
            else
            {
                sb.AppendLine("-- 无自动注销内容。");
            }

            sb.AppendLine("-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<3")
              .AppendLine()
              .AppendLine("end")
              .AppendLine();

            // 动画
            sb.AppendLine("---播放打开动画（可重写）")
              .AppendLine("---@type fun():void")
              .AppendLine($"function {scriptName}:OnPlayOpenAnimation()")
              .AppendLine($"    {scriptName}.super.OnPlayOpenAnimation(self)")
              .AppendLine("end")
              .AppendLine();

            sb.AppendLine("---播放关闭动画（可重写）")
              .AppendLine("---@type fun():void")
              .AppendLine($"function {scriptName}:OnPlayCloseAnimation()")
              .AppendLine($"    {scriptName}.super.OnPlayCloseAnimation(self)")
              .AppendLine("end")
              .AppendLine();

            // UI 专用
            if (m_PrefabType.enumValueIndex == (int)Runtime.PrefabType.UI)
            {
                sb.AppendLine("---子UI销毁回调")
                  .AppendLine("---@type fun(luaClass:XLua.LuaTable):void")
                  .AppendLine("---@param luaClass XLua.LuaTable")
                  .AppendLine($"function {scriptName}:OnAddedUIDestroyed(luaClass)")
                  .AppendLine($"    {scriptName}.super.OnAddedUIDestroyed(self, luaClass)")
                  .AppendLine("end")
                  .AppendLine();
            }

            // 2D 碰撞
            if (m_UseCollider2DLifeCycles.boolValue)
            {
                sb.AppendLine("---进入碰撞2D")
                  .AppendLine("---@type fun(c:UnityEngine.Collision2D):void")
                  .AppendLine($"function {scriptName}:OnCollisionEnter2D(c)")
                  .AppendLine("end")
                  .AppendLine();

                sb.AppendLine("---停留碰撞2D")
                  .AppendLine("---@type fun(c:UnityEngine.Collision2D):void")
                  .AppendLine($"function {scriptName}:OnCollisionStay2D(c)")
                  .AppendLine("end")
                  .AppendLine();

                sb.AppendLine("---退出碰撞2D")
                  .AppendLine("---@type fun(c:UnityEngine.Collision2D):void")
                  .AppendLine($"function {scriptName}:OnCollisionExit2D(c)")
                  .AppendLine("end")
                  .AppendLine();
            }

            // 3D 碰撞
            if (m_UseCollider3DLifeCycles.boolValue)
            {
                sb.AppendLine("---进入碰撞3D")
                  .AppendLine("---@type fun(c:UnityEngine.Collision):void")
                  .AppendLine($"function {scriptName}:OnCollisionEnter3D(c)")
                  .AppendLine("end")
                  .AppendLine();

                sb.AppendLine("---停留碰撞3D")
                  .AppendLine("---@type fun(c:UnityEngine.Collision):void")
                  .AppendLine($"function {scriptName}:OnCollisionStay3D(c)")
                  .AppendLine("end")
                  .AppendLine();

                sb.AppendLine("---退出碰撞3D")
                  .AppendLine("---@type fun(c:UnityEngine.Collision):void")
                  .AppendLine($"function {scriptName}:OnCollisionExit3D(c)")
                  .AppendLine("end")
                  .AppendLine();
            }

            // 2D 触发
            if (m_UseTrigger2DLifeCycles.boolValue)
            {
                sb.AppendLine("---进入触发2D")
                  .AppendLine("---@type fun(o:UnityEngine.Collider2D):void")
                  .AppendLine($"function {scriptName}:OnTriggerEnter2D(o)")
                  .AppendLine("end")
                  .AppendLine();

                sb.AppendLine("---停留触发2D")
                  .AppendLine("---@type fun(o:UnityEngine.Collider2D):void")
                  .AppendLine($"function {scriptName}:OnTriggerStay2D(o)")
                  .AppendLine("end")
                  .AppendLine();

                sb.AppendLine("---退出触发2D")
                  .AppendLine("---@type fun(o:UnityEngine.Collider2D):void")
                  .AppendLine($"function {scriptName}:OnTriggerExit2D(o)")
                  .AppendLine("end")
                  .AppendLine();
            }

            // 3D 触发
            if (m_UseTrigger3DLifeCycles.boolValue)
            {
                sb.AppendLine("---进入触发3D")
                  .AppendLine("---@type fun(o:UnityEngine.Collider):void")
                  .AppendLine($"function {scriptName}:OnTriggerEnter3D(o)")
                  .AppendLine("end")
                  .AppendLine();

                sb.AppendLine("---停留触发3D")
                  .AppendLine("---@type fun(o:UnityEngine.Collider):void")
                  .AppendLine($"function {scriptName}:OnTriggerStay3D(o)")
                  .AppendLine("end")
                  .AppendLine();

                sb.AppendLine("---退出触发3D")
                  .AppendLine("---@type fun(o:UnityEngine.Collider):void")
                  .AppendLine($"function {scriptName}:OnTriggerExit3D(o)")
                  .AppendLine("end")
                  .AppendLine();
            }

            // UI 方法
            for (int i = 0; i < funcNames.Count; i++)
            {
                bool repeat = false;
                for (int j = 0; j < i; j++)
                {
                    if (funcNames[j] == funcNames[i])
                    {
                        repeat = true;
                        break;
                    }
                }
                if (repeat) continue;

                string[] paramArr = funcParams[i].Replace(" ", "").Split(',');
                string paramDesc = string.Join(", ", Array.ConvertAll(paramArr, p => $"{p}:any"));
                string paramList = string.Join(", ", paramArr);

                sb.AppendLine($"---{injectComments[i]}")
                  .AppendLine($"---@type fun({paramDesc}):void")
                  .AppendLine($"function {scriptName}:{funcNames[i]}({paramList})");

                if (funcNames[i].EndsWith("GettingItem"))
                {
                    sb.AppendLine($"    if itemIndex < 0 or itemIndex >= self.{injectNames[i]}.MaxItemNum then return nil end")
                      .AppendLine($"    local item = self.{injectNames[i]}:NewListViewItem('Item')")
                      .AppendLine($"    if not item.IsInitHandlerCalled then item.IsInitHandlerCalled = true end")
                      .AppendLine("    return item");
                }
                else
                {
                    sb.AppendLine();
                }

                sb.AppendLine("end")
                  .AppendLine();
            }

            sb.AppendLine($"return {scriptName}");
            return sb;
        }
        #endregion

        #region 代码生成模块 - 刷新现有 Lua
        /// <summary>
        /// 刷新 None 模式已有 Lua 文件
        /// <para>增量更新：保留手写代码，仅更新自动生成区域</para>
        /// </summary>
        /// <param name="fullPath">Lua文件完整路径</param>
        /// <returns>更新后的Lua代码构建器</returns>
        private StringBuilder GeneratePatternNoneCodeLines(string fullPath)
        {
            try
            {
                string scriptName = m_LuaScriptNamesNone.GetArrayElementAtIndex((int)NonePatternType.Default).stringValue;
                string superName = m_LuaSuperScriptNamesNone.GetArrayElementAtIndex((int)NonePatternType.Default).stringValue;
                string commentFlag = "--=====================================================================================================";
                string classFlag = $"local {scriptName} = class";
                string regStart = "-- 2>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>";
                string regEnd = "-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<2";
                string unRegStart = "-- 3>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>";
                string unRegEnd = "-- <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<3";

                string content = System.IO.File.ReadAllText(fullPath);

                // 清理自动生成区域
                int commentEnd = content.LastIndexOf(commentFlag) + commentFlag.Length + 4;
                int classStart = content.LastIndexOf(classFlag);
                int regStartIdx = content.LastIndexOf(regStart) + regStart.Length + 1;
                int regEndIdx = content.LastIndexOf(regEnd);
                int unRegStartIdx = content.LastIndexOf(unRegStart) + unRegStart.Length + 1;
                int unRegEndIdx = content.LastIndexOf(unRegEnd);

                content = content.Remove(commentEnd, classStart - commentEnd);
                content = content.Remove(regStartIdx, regEndIdx - regStartIdx);
                content = content.Remove(unRegStartIdx, unRegEndIdx - unRegStartIdx);

                // 采集注入
                CollectInfoExInfos(out List<string> injectNames, out _, out List<string> funcNames, out _, out List<string> cmds);

                // 插入注销
                if (funcNames.Count > 0)
                {
                    for (int i = 0; i < funcNames.Count; i++)
                    {
                        content = content.Insert(unRegEndIdx, $"    OnRemoveListener(self.{injectNames[i]}, '{cmds[i]}', handler(self, self.{funcNames[i]}))\n");
                    }
                }
                else
                {
                    content = content.Insert(unRegEndIdx, "-- 无自动注销内容。\n");
                }

                // 插入注册
                if (funcNames.Count > 0)
                {
                    for (int i = 0; i < funcNames.Count; i++)
                    {
                        content = content.Insert(regEndIdx, $"    AddUIListenerFunction(self.{injectNames[i]}, '{cmds[i]}', handler(self, self.{funcNames[i]}))\n");
                    }
                }
                else
                {
                    content = content.Insert(regEndIdx, "-- 无自动注册内容。\n");
                }

                // 插入字段
                if (m_Injections != null && m_Injections.arraySize > 0)
                {
                    for (int i = m_Injections.arraySize - 1; i >= 0; i--)
                    {
                        string field = m_InterInjectionNames[i].stringValue;
                        string type = "";
                        string valid = "";
                        string infoEx = "";
                        string comment = m_InterInjectionComments[i].stringValue ?? "";

                        if (m_InterInjectionIsArrays[i].boolValue)
                        {
                            type = $"{LuaInjection.LuaInjectionType[(int)m_InterInjectionTypeNames[i].enumValueIndex]}[]";
                            valid = "√";
                            for (int j = 0; j < m_InterInjectionElementsObjs[i].arraySize; j++)
                            {
                                if (m_InterInjectionElementsObjs[i].GetArrayElementAtIndex(j).objectReferenceValue == null)
                                {
                                    valid = "×";
                                    break;
                                }
                            }
                        }
                        else
                        {
                            type = LuaInjection.LuaInjectionType[(int)m_InterInjectionTypeNames[i].enumValueIndex];
                            valid = (m_InterInjectionTypeNames[i].enumValueIndex is < (int)LuaInjection.InjectionType.Int32 or > (int)LuaInjection.InjectionType.Boolean)
                                ? (m_InterInjectionObjs[i].objectReferenceValue != null ? "√" : "×")
                                : (string.IsNullOrEmpty(m_InterInjectionVariants[i].stringValue) ? "×" : m_InterInjectionVariants[i].stringValue);

                            infoEx = m_InterInjectionInfoExs[i].stringValue;
                        }

                        if (type == "UnityEngine.GameObject" && !string.IsNullOrEmpty(infoEx))
                        {
                            Type t = Type.GetType(infoEx);
                            type = t?.FullName ?? "any";
                        }
                        else if (type == "Honor.Runtime.LuaBehaviour" && !string.IsNullOrEmpty(infoEx))
                        {
                            type = infoEx;
                        }

                        while (field.Length < 35) field += " ";
                        while (type.Length < 30) type += " ";
                        while (valid.Length < 10) valid += " ";
                        while (infoEx.Length < 15) infoEx += " ";

                        content = content.Insert(commentEnd, $"---@field {field}{type}{valid}{infoEx}{comment}\n");
                    }
                }

                // 插入类头
                content = content.Insert(commentEnd, $"---@field cs Honor.Runtime.LuaBehaviour @LuaBehaviour\n");
                content = content.Insert(commentEnd, $"---@class {scriptName} : {superName}\n");

                // 追加方法
                int returnIdx = content.LastIndexOf($"return {scriptName}");
                string funcAppend = "";

                // Proc
                if (m_UseProc.boolValue && !content.Contains($"function {scriptName}:Proc()"))
                {
                    funcAppend += "---心跳（自定义）\n---@type fun():void\n" +
                                  $"function {scriptName}:Proc()\n    {scriptName}.super.Proc(self)\n\nend\n\n";
                }

                // UI 专用
                if (m_PrefabType.enumValueIndex == (int)Runtime.PrefabType.UI &&
                    !content.Contains($"function {scriptName}:OnAddedUIDestroyed"))
                {
                    funcAppend += "---子UI销毁\n---@type fun(luaClass:XLua.LuaTable):void\n" +
                                  $"function {scriptName}:OnAddedUIDestroyed(luaClass)\n    {scriptName}.super.OnAddedUIDestroyed(self, luaClass)\n\nend\n\n";
                }

                // 插入
                if (!string.IsNullOrEmpty(funcAppend))
                    content = content.Insert(returnIdx, funcAppend);

                // 更新类定义
                int classDefIdx = content.LastIndexOf($"local {scriptName} = class('{scriptName}'");
                int lineEnd = content.IndexOf('\n', classDefIdx);
                if (lineEnd > classDefIdx)
                    content = content.Remove(classDefIdx, lineEnd - classDefIdx);

                content = content.Insert(classDefIdx, $"local {scriptName} = class('{scriptName}', import('{superName}'))");

                return new StringBuilder(content, commentEnd, content.Length - commentEnd, content.Length * 2);
            }
            catch
            {
                return new StringBuilder();
            }
        }
        #endregion
    }
}