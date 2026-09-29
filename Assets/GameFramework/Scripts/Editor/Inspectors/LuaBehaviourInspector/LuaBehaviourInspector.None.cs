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
                for (int i = 0; i < (int)NonePatternType.TotalCount; i++)
                {
                    m_LuaScriptNamesNone.InsertArrayElementAtIndex(i);
                    m_LuaScriptNamesNone.GetArrayElementAtIndex(i).stringValue = string.Empty;
                }
            }

            m_LuaSuperScriptNamesNone = serializedObject.FindProperty("m_LuaSuperScriptNamesNone");
            if (m_LuaSuperScriptNamesNone.arraySize > (int)NonePatternType.TotalCount)
            {
                m_LuaSuperScriptNamesNone.ClearArray();
            }

            if (m_LuaSuperScriptNamesNone.arraySize == 0)
            {
                for (int i = 0; i < (int)NonePatternType.TotalCount; i++)
                {
                    m_LuaSuperScriptNamesNone.InsertArrayElementAtIndex(i);
                }
            }

            for (int i = 0; i < (int)NonePatternType.TotalCount; i++)
            {
                if (string.IsNullOrEmpty(m_LuaSuperScriptNamesNone.GetArrayElementAtIndex(i).stringValue))
                {
                    m_LuaSuperScriptNamesNone.GetArrayElementAtIndex(i).stringValue = "LuaBehaviourSuper";
                }
            }

            m_TextLuaSuperScriptNamesNone = new List<string>();
            for (int i = 0; i < (int)NonePatternType.TotalCount; i++)
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
            AppendNoneInjectionFields(sb);

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
              .AppendLine("-- 2=====================================================================================================");

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

            sb.AppendLine("-- ====================================================================================================2")
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
              .AppendLine("-- 3=====================================================================================================");

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

            sb.AppendLine("-- ====================================================================================================3")
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

            // 2D/3D 碰撞与触发器生命周期函数
            AppendNoneColliderTriggerLifeCycles(sb, scriptName);

            // UI 交互方法
            AppendNoneUIInteractionMethods(sb, scriptName, injectNames, injectComments, funcNames, funcParams);

            sb.AppendLine($"return {scriptName}");
            return sb;
        }

        /// <summary>
        /// 追加None模式注入对象字段声明（---@field）代码
        /// </summary>
        /// <param name="sb">代码构建器</param>
        private void AppendNoneInjectionFields(StringBuilder sb)
        {
            if (m_Injections != null && m_Injections.arraySize > 0)
            {
                for (int i = 0; i < m_Injections.arraySize; i++)
                {
                    // 名称未填写的注入项不生成字段，避免生成空行脏数据
                    if (string.IsNullOrEmpty(m_InterInjectionNames[i].stringValue))
                    {
                        continue;
                    }

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
        }

        /// <summary>
        /// 追加None模式2D/3D碰撞与触发器生命周期函数骨架代码（按开关决定是否输出）
        /// </summary>
        /// <param name="sb">代码构建器</param>
        /// <param name="scriptName">Lua脚本名称</param>
        private void AppendNoneColliderTriggerLifeCycles(StringBuilder sb, string scriptName)
        {
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
        }

        /// <summary>
        /// 追加None模式UI交互监听函数骨架代码（按注入去重，GettingItem生成列表项返回骨架）
        /// </summary>
        /// <param name="sb">代码构建器</param>
        /// <param name="scriptName">Lua脚本名称</param>
        /// <param name="injectNames">注入对象名称列表</param>
        /// <param name="injectComments">注入对象注释列表</param>
        /// <param name="funcNames">注入回调函数名列表</param>
        /// <param name="funcParams">注入回调参数列表</param>
        private void AppendNoneUIInteractionMethods(StringBuilder sb, string scriptName, List<string> injectNames, List<string> injectComments, List<string> funcNames, List<string> funcParams)
        {
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
        }
        #endregion

        #region 代码生成模块 - 刷新现有 Lua
        /// <summary>
        /// 刷新 None 模式已有 Lua 文件
        /// <para>增量更新：保留手写代码，仅更新自动生成区域</para>
        /// <remarks>
        /// 修复说明：
        /// 1. 移除原 "catch { return new StringBuilder(); }" —— 任何异常都会把原文件覆盖为"只有头注释"。
        ///    现在异常向上抛出，由 CreateOrRefreshLuaFile 记录 Log.Error 并保留原文件。
        /// 2. 原实现用"修改前"的索引做多处 Remove/Insert，删除字段区后注册/注销区索引全部漂移，
        ///    导致删除错位或 ArgumentOutOfRange。现在从后往前清理，且每次操作都重新定位锚点。
        /// 3. 刷新时补全缺失的事件回调函数骨架（只补不覆盖），新增按钮刷新后即可直接编辑。
        /// </remarks>
        /// </summary>
        /// <param name="fullPath">Lua文件完整路径</param>
        /// <returns>更新后的Lua代码构建器（不含头部注释，由外层统一重写头部）</returns>
        private StringBuilder GeneratePatternNoneCodeLines(string fullPath)
        {
            string scriptName = m_LuaScriptNamesNone.GetArrayElementAtIndex((int)NonePatternType.Default).stringValue;
            string superName = m_LuaSuperScriptNamesNone.GetArrayElementAtIndex((int)NonePatternType.Default).stringValue;
            string commentFlag = "--=====================================================================================================";
            string classFlag = $"local {scriptName} = class";
            string regStart = "-- 2=====================================================================================================";
            string regEnd = "-- ====================================================================================================2";
            string unRegStart = "-- 3=====================================================================================================";
            string unRegEnd = "-- ====================================================================================================3";

            string content = System.IO.File.ReadAllText(fullPath);

            // ========== 第1步：清理"自动注销"块内部（保留两个标记行，先处理位置靠后的块） ==========
            int unRegStartIdx = content.LastIndexOf(unRegStart);
            int unRegEndIdx = content.LastIndexOf(unRegEnd);
            if (unRegStartIdx < 0 || unRegEndIdx <= unRegStartIdx)
            {
                throw new InvalidOperationException($"[LuaBehaviourInspector] 刷新失败：{fullPath} 未找到注销标记，已中止刷新以保护原文件");
            }
            int unRegInnerStart = SkipLineBreaks(content, unRegStartIdx + unRegStart.Length);
            content = content.Remove(unRegInnerStart, unRegEndIdx - unRegInnerStart);

            // ========== 第2步：清理"自动注册"块内部（重新定位锚点，避免受第1步影响） ==========
            int regStartIdx = content.LastIndexOf(regStart);
            int regEndIdx = content.LastIndexOf(regEnd);
            if (regStartIdx < 0 || regEndIdx <= regStartIdx)
            {
                throw new InvalidOperationException($"[LuaBehaviourInspector] 刷新失败：{fullPath} 未找到注册标记，已中止刷新以保护原文件");
            }
            int regInnerStart = SkipLineBreaks(content, regStartIdx + regStart.Length);
            content = content.Remove(regInnerStart, regEndIdx - regInnerStart);

            // ========== 第3步：清理旧字段注释区（---@class / ---@field），随后统一重建 ==========
            int headerEndIdx = content.LastIndexOf(commentFlag);
            int classDefIdx = content.LastIndexOf(classFlag);
            if (headerEndIdx < 0 || classDefIdx <= headerEndIdx)
            {
                throw new InvalidOperationException($"[LuaBehaviourInspector] 刷新失败：{fullPath} 未找到头部注释或类定义标记，已中止刷新以保护原文件");
            }
            int fieldsStart = SkipLineBreaks(content, headerEndIdx + commentFlag.Length);
            content = content.Remove(fieldsStart, classDefIdx - fieldsStart);

            // 采集注入信息（空名称注入项已在 CollectInfoExInfos 内跳过）
            CollectInfoExInfos(out List<string> injectNames, out List<string> injectComments,
                out List<string> funcNames, out List<string> funcParams, out List<string> cmds);

            // 重建字段注释区
            StringBuilder fieldsBuilder = new StringBuilder();
            fieldsBuilder.AppendLine($"---@class {scriptName} : {superName}");
            fieldsBuilder.AppendLine("---@field cs Honor.Runtime.LuaBehaviour @LuaBehaviour");
            if (m_Injections != null && m_Injections.arraySize > 0)
            {
                for (int i = 0; i < m_Injections.arraySize; i++)
                {
                    // 名称未填写的注入项不生成字段，避免空行脏数据
                    if (string.IsNullOrEmpty(m_InterInjectionNames[i].stringValue))
                    {
                        continue;
                    }

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

                    fieldsBuilder.AppendLine($"---@field {field}{type}{valid}{infoEx}{comment}");
                }
            }
            content = content.Insert(fieldsStart, fieldsBuilder.ToString());

            // ========== 第4步：更新类定义行（父类名可能变化） ==========
            int classLineStart = content.LastIndexOf(classFlag);
            int classLineEnd = content.IndexOf('\n', classLineStart);
            if (classLineEnd > classLineStart)
            {
                content = content.Remove(classLineStart, classLineEnd - classLineStart);
            }
            content = content.Insert(classLineStart, $"local {scriptName} = class('{scriptName}', import('{superName}'))");

            // ========== 第5步：插入注销行（在注销end标记之前，锚点重新定位） ==========
            int unRegInsIdx = content.LastIndexOf(unRegEnd);
            content = content.Insert(unRegInsIdx, BuildNoneListenerLines(false, injectNames, cmds, funcNames));

            // ========== 第6步：插入注册行（在注册end标记之前，锚点重新定位） ==========
            int regInsIdx = content.LastIndexOf(regEnd);
            content = content.Insert(regInsIdx, BuildNoneListenerLines(true, injectNames, cmds, funcNames));

            // ========== 第7步：追加缺失的方法（只补不覆盖，保护手写内容） ==========
            int returnIdx = content.LastIndexOf($"return {scriptName}");
            if (returnIdx < 0)
            {
                throw new InvalidOperationException($"[LuaBehaviourInspector] 刷新失败：{fullPath} 未找到 return 语句，已中止刷新以保护原文件");
            }

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

            // 缺失的事件回调函数骨架（新增按钮/事件刷新后即可直接编辑）
            for (int i = 0; i < funcNames.Count; i++)
            {
                if (content.Contains($"function {scriptName}:{funcNames[i]}("))
                {
                    continue;
                }

                string[] paramArr = funcParams[i].Replace(" ", "").Split(',');
                string paramDesc = string.Join(", ", Array.ConvertAll(paramArr, p => $"{p}:any"));
                string paramList = string.Join(", ", paramArr);

                funcAppend += $"---{injectComments[i]}\n";
                funcAppend += $"---@type fun({paramDesc}):void\n";
                funcAppend += $"function {scriptName}:{funcNames[i]}({paramList})\n";
                if (funcNames[i].EndsWith("GettingItem"))
                {
                    funcAppend += $"    if itemIndex < 0 or itemIndex >= self.{injectNames[i]}.MaxItemNum then return nil end\n";
                    funcAppend += $"    local item = self.{injectNames[i]}:NewListViewItem('Item')\n";
                    funcAppend += $"    if not item.IsInitHandlerCalled then item.IsInitHandlerCalled = true end\n";
                    funcAppend += "    return item\n";
                }
                else
                {
                    funcAppend += "\n";
                }
                funcAppend += "end\n\n";
            }

            if (!string.IsNullOrEmpty(funcAppend))
            {
                content = content.Insert(returnIdx, funcAppend);
            }

            // 返回"头部注释之后"的正文（外层 GeneratePatternNoneCommentLines 会统一重写头部）
            return new StringBuilder(content.Substring(fieldsStart));
        }

        /// <summary>
        /// 构建None模式自动注册/注销监听代码行（注册与注销共用，按方向输出不同函数名）
        /// </summary>
        /// <param name="isRegister">true=注册(AddUIListenerFunction)，false=注销(OnRemoveListener)</param>
        /// <param name="injectNames">注入对象名称列表</param>
        /// <param name="cmds">注入事件命令列表</param>
        /// <param name="funcNames">注入回调函数名列表</param>
        /// <returns>拼接后的监听代码文本</returns>
        private string BuildNoneListenerLines(bool isRegister, List<string> injectNames, List<string> cmds, List<string> funcNames)
        {
            StringBuilder sb = new StringBuilder();
            if (funcNames.Count > 0)
            {
                for (int i = 0; i < funcNames.Count; i++)
                {
                    if (isRegister)
                    {
                        sb.AppendLine($"    AddUIListenerFunction(self.{injectNames[i]}, '{cmds[i]}', handler(self, self.{funcNames[i]}))");
                    }
                    else
                    {
                        sb.AppendLine($"    OnRemoveListener(self.{injectNames[i]}, '{cmds[i]}', handler(self, self.{funcNames[i]}))");
                    }
                }
            }
            else
            {
                sb.AppendLine(isRegister ? "-- 无自动注册内容。" : "-- 无自动注销内容。");
            }
            return sb.ToString();
        }

        /// <summary>
        /// 跳过字符串指定位置开始的换行符（兼容 \r\n 与 \n），返回首个非换行字符下标
        /// </summary>
        /// <param name="content">目标字符串</param>
        /// <param name="start">起始下标</param>
        /// <returns>首个非换行字符下标</returns>
        private static int SkipLineBreaks(string content, int start)
        {
            int idx = start;
            while (idx < content.Length && (content[idx] == '\r' || content[idx] == '\n'))
            {
                idx++;
            }
            return idx;
        }
        #endregion
    }
}
