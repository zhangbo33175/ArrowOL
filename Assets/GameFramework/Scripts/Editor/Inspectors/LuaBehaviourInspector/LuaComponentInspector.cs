/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Editor
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  LuaComponentInspector.cs
 * author:    云毅
 * created:   2026
 * descrip:   LuaComponent 自定义Inspector面板
 *            提供：XLua脚本生成、性能分析、快捷创建Lua模板
 ***************************************************************/

using CSObjectWrapEditor;
using System;
using System.Text;
using Honor.Runtime;
using UnityEditor;
using UnityEngine;

namespace Honor.Editor
{
    [CustomEditor(typeof(LuaComponent))]
    internal sealed class LuaComponentInspector : HonorComponentInspector
    {
        #region 序列化字段
        /// <summary>
        /// Lua运行时分析模式
        /// </summary>
        private SerializedProperty m_LuaRuntimeProfilerMode;
        #endregion

        #region 编辑器配置字段
        /// <summary>
        /// 自定义生成Lua脚本名称
        /// </summary>
        private string m_CustomLuaScriptName = string.Empty;

        /// <summary>
        /// 自定义生成Lua脚本的父类名称
        /// </summary>
        private string m_CustomLuaSuperScriptName = string.Empty;

        /// <summary>
        /// 使用Proc心跳
        /// </summary>
        private bool m_UseProc = true;

        /// <summary>
        /// Lua脚本作者
        /// </summary>
        private string m_LuaAuthorName = string.Empty;

        /// <summary>
        /// Lua脚本描述
        /// </summary>
        private string m_LuaDescript = string.Empty;

        /// <summary>
        /// 自定义Lua是否可以自动生成
        /// </summary>
        private bool m_LuaCanGenerate;
        #endregion

        #region 生命周期
        private void OnEnable()
        {
            m_LuaRuntimeProfilerMode = serializedObject.FindProperty("m_LuaRuntimeProfilerMode");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();

            DrawRuntimeTools();
            EditorGUILayout.Separator();
            DrawLuaFileGenerator();

            serializedObject.ApplyModifiedProperties();
            Repaint();
        }

        protected override void OnCompileStart()
        {
            base.OnCompileStart();
        }

        protected override void OnCompileComplete()
        {
            base.OnCompileComplete();
        }
        #endregion

        #region 运行时工具（性能分析 + 脚本重载）
        /// <summary>
        /// 绘制运行时工具区域
        /// </summary>
        private void DrawRuntimeTools()
        {
            EditorGUI.BeginDisabledGroup(Application.isPlaying);
            {
                if (GUILayout.Button("重新生成XLua脚本"))
                {
                    Generator.ClearAll();
                    GUIUtility.ExitGUI();
                }

                m_LuaRuntimeProfilerMode.boolValue = EditorGUILayout.Toggle("Lua运行时分析模式", m_LuaRuntimeProfilerMode.boolValue);
            }
            EditorGUI.EndDisabledGroup();

            if (!Application.isPlaying)
                return;

            LuaComponent luaComp = (LuaComponent)target;
            if (m_LuaRuntimeProfilerMode.boolValue)
            {
                if (GUILayout.Button("导出Lua运行时性能数据到 CSV 文件"))
                {
                    ExportProfilerCSV(luaComp);
                }
            }

            EditorGUILayout.Separator();
            EditorGUILayout.LabelField("已加载Lua脚本名称列表");

            EditorGUILayout.BeginVertical("box");
            foreach (string scriptName in luaComp.LoadedLuaScriptsNames)
            {
                EditorGUILayout.LabelField(scriptName);
            }
            EditorGUILayout.EndVertical();
        }

        /// <summary>
        /// 导出性能分析数据
        /// </summary>
        private void ExportProfilerCSV(LuaComponent luaComp)
        {
            string fileName = EditorUtility.SaveFilePanel(
                "导出 CSV 数据",
                string.Empty,
                $"Lua运行时性能数据 {DateTime.Now:yyyy-MM-dd HH-mm-ss}.csv",
                string.Empty);

            if (string.IsNullOrEmpty(fileName))
                return;

            try
            {
                var result = luaComp.Env.DoString("return __lua_profiler.report()");
                System.IO.File.WriteAllText(fileName, result[0].ToString(), new UTF8Encoding(false));
                Log.Debug($"导出 CSV 数据到 '{fileName}' 成功。");
            }
            catch (Exception ex)
            {
                Log.Error($"导出 CSV 失败：{ex}");
            }

            GUIUtility.ExitGUI();
        }
        #endregion

        #region Lua文件快捷生成器
        /// <summary>
        /// 绘制Lua文件快捷生成区域
        /// </summary>
        private void DrawLuaFileGenerator()
        {
            EditorGUILayout.LabelField("[自定义Lua脚本-快捷创建入口]");

            m_CustomLuaScriptName = EditorGUILayout.TextField("自定义Lua脚本名称", m_CustomLuaScriptName);
            m_CustomLuaSuperScriptName = EditorGUILayout.TextField("自定义Lua脚本父类", m_CustomLuaSuperScriptName);

            // 合法性检查
            m_LuaCanGenerate = true;
            if (CheckLuaNameInvalid(m_CustomLuaScriptName) || CheckLuaNameInvalid(m_CustomLuaSuperScriptName))
            {
                m_LuaCanGenerate = false;
                EditorGUILayout.HelpBox("请勿写入 .lua 后缀", MessageType.Error);
            }

            m_UseProc = EditorGUILayout.Toggle("使用Proc", m_UseProc);
            m_LuaAuthorName = EditorGUILayout.TextField("Lua脚本作者", m_LuaAuthorName);
            m_LuaDescript = EditorGUILayout.TextField("Lua脚本描述", m_LuaDescript);

            if (m_LuaCanGenerate)
            {
                EditorGUILayout.HelpBox("说明：Lua脚本不存在时生成，存在时刷新。", MessageType.Info);
                if (GUILayout.Button("生成/刷新Lua脚本"))
                {
                    CreateOrRefreshLuaFile();
                    GUIUtility.ExitGUI();
                }
            }
            else
            {
                EditorGUILayout.HelpBox("请先解决输入问题才能生成/刷新Lua脚本。", MessageType.Warning);
            }
        }

        /// <summary>
        /// 检查Lua名称是否包含非法后缀
        /// </summary>
        private bool CheckLuaNameInvalid(string name)
        {
            return !string.IsNullOrEmpty(name) && (name.StartsWith(".lua") || name.EndsWith(".lua"));
        }
        #endregion

        #region Lua文件生成/刷新逻辑
        /// <summary>
        /// 生成或刷新Lua文件
        /// </summary>
        private void CreateOrRefreshLuaFile()
        {
            string fileName = m_CustomLuaScriptName;
            string luaRoot = $"{Application.dataPath.Substring(0, Application.dataPath.Length - "Assets".Length)}{GamePathUtils.LuaScript.Game.GetRootDirectoryRelativePath(true)}";
            string[] files = System.IO.Directory.GetFiles(luaRoot, $"{fileName}.lua.txt", System.IO.SearchOption.AllDirectories);

            // 不存在 → 新建
            if (files.Length == 0)
            {
                CreateNewLuaFile(luaRoot, fileName);
            }
            // 存在一个 → 刷新
            else if (files.Length == 1)
            {
                RefreshExistLuaFile(files[0]);
            }
            // 多个 → 报错
            else
            {
                Log.Error($"目录 {luaRoot} 下存在多个 {fileName}.lua.txt！");
            }
        }

        /// <summary>
        /// 创建新Lua文件
        /// </summary>
        private void CreateNewLuaFile(string root, string fileName)
        {
            string savePath = EditorUtility.SaveFilePanel(
                $"生成 {fileName}.lua.txt",
                root,
                $"{fileName}.lua.txt",
                string.Empty);

            if (string.IsNullOrEmpty(savePath))
                return;

            try
            {
                string comment = GenerateCommentLines().ToString();
                string code = GenerateEmptyCodeLines().ToString();
                System.IO.File.WriteAllText(savePath, comment + code, new UTF8Encoding(false));

                Log.Debug($"生成 {savePath} 成功！");
                AssetDatabase.Refresh();
            }
            catch (Exception ex)
            {
                Log.Error($"生成失败：{ex}");
            }
        }

        /// <summary>
        /// 刷新已有Lua文件
        /// </summary>
        private void RefreshExistLuaFile(string path)
        {
            try
            {
                string comment = GenerateCommentLines().ToString();
                string code = GenerateCodeLines(path).ToString();

                System.IO.File.WriteAllText(path, comment + code, new UTF8Encoding(false));
                Log.Debug($"刷新 {path} 成功！");
                AssetDatabase.Refresh();
            }
            catch (Exception ex)
            {
                Log.Error($"刷新失败：{ex}");
            }
        }

        /// <summary>
        /// 生成文件头注释
        /// </summary>
        private StringBuilder GenerateCommentLines()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("--=====================================================================================================")
              .AppendLine("-- (c) copyright 2026 - 2030, Honor.Game")
              .AppendLine("-- All Rights Reserved.")
              .AppendLine("-- ----------------------------------------------------------------------------------------------------")
              .AppendLine($"-- filename:  {m_CustomLuaScriptName}")
              .AppendLine($"-- author:    {m_LuaAuthorName}")
              .AppendLine($"-- descrip:   {m_LuaDescript}")
              .AppendLine("--=====================================================================================================")
              .AppendLine();

            return sb;
        }

        /// <summary>
        /// 生成全新标准Lua模板
        /// </summary>
        private StringBuilder GenerateEmptyCodeLines()
        {
            StringBuilder sb = new StringBuilder();
            string scriptName = m_CustomLuaScriptName;
            string superName = m_CustomLuaSuperScriptName;
            string desc = m_LuaDescript;

            // 类定义
            if (!string.IsNullOrEmpty(superName))
            {
                sb.AppendLine($"---@class {scriptName} : {superName} @{desc}");
                sb.AppendLine($"local {scriptName} = class('{scriptName}', import('{superName}'))");
            }
            else
            {
                sb.AppendLine($"---@class {scriptName} @{desc}");
                sb.AppendLine($"local {scriptName} = class('{scriptName}')");
            }

            sb.AppendLine();

            // 构造
            sb.AppendLine("---构造函数")
              .AppendLine("---@type fun(args:table):void")
              .AppendLine("---@param args table @自定义参数")
              .AppendLine($"function {scriptName}:ctor(args)");

            if (!string.IsNullOrEmpty(superName))
                sb.AppendLine($"    {scriptName}.super.ctor(self, args)");

            sb.AppendLine()
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

            // Init
            sb.AppendLine("---初始化")
              .AppendLine("---@type fun():void")
              .AppendLine($"function {scriptName}:Init()");

            if (!string.IsNullOrEmpty(superName))
                sb.AppendLine($"    {scriptName}.super.Init(self)");

            sb.AppendLine()
              .AppendLine("end")
              .AppendLine();

            // Proc
            if (m_UseProc)
            {
                sb.AppendLine("---心跳")
                  .AppendLine("---@type fun():void")
                  .AppendLine($"function {scriptName}:Proc()");

                if (!string.IsNullOrEmpty(superName))
                    sb.AppendLine($"    {scriptName}.super.Proc(self)");

                sb.AppendLine()
                  .AppendLine("end")
                  .AppendLine();
            }

            // Destroy
            sb.AppendLine("---销毁")
              .AppendLine("---@type fun():void")
              .AppendLine($"function {scriptName}:Destroy()");

            if (!string.IsNullOrEmpty(superName))
                sb.AppendLine($"    {scriptName}.super.Destroy(self)");

            sb.AppendLine()
              .AppendLine("end")
              .AppendLine()
              .AppendLine($"return {scriptName}");

            return sb;
        }

        /// <summary>
        /// 提取现有Lua代码并刷新结构
        /// </summary>
        private StringBuilder GenerateCodeLines(string fullPath)
        {
            string scriptName = m_CustomLuaScriptName;
            string superName = m_CustomLuaSuperScriptName;
            string commentFlag = "--=====================================================================================================";

            string content = System.IO.File.ReadAllText(fullPath);
            int commentEnd = content.LastIndexOf(commentFlag) + commentFlag.Length + 4;

            // 自动补全 Proc
            int returnIndex = content.LastIndexOf($"return {scriptName}");
            string procFunc = string.Empty;

            if (m_UseProc && !content.Contains($"function {scriptName}:Proc()"))
            {
                procFunc = !string.IsNullOrEmpty(superName)
                    ? $"---心跳\n---@type fun():void\nfunction {scriptName}:Proc()\n    {scriptName}.super.Proc(self)\n\nend\n\n"
                    : $"---心跳\n---@type fun():void\nfunction {scriptName}:Proc()\n\nend\n\n";
            }

            if (!string.IsNullOrEmpty(procFunc))
                content = content.Insert(returnIndex, procFunc);

            // 替换类注释
            int classCommentIdx = content.LastIndexOf($"---@class {scriptName}");
            if (classCommentIdx != -1)
            {
                int lineEnd = content.IndexOf('\n', classCommentIdx);
                if (lineEnd > classCommentIdx)
                    content = content.Remove(classCommentIdx, lineEnd - classCommentIdx + 1);
            }

            // 替换类定义
            int classDefIdx = content.LastIndexOf($"local {scriptName} = class('{scriptName}'");
            if (classDefIdx != -1)
            {
                int lineEnd = content.IndexOf('\n', classDefIdx);
                if (lineEnd > classDefIdx)
                    content = content.Remove(classDefIdx, lineEnd - classDefIdx);
            }

            // 重新插入类定义
            if (!string.IsNullOrEmpty(superName))
            {
                content = content.Insert(classDefIdx, $"---@class {scriptName} : {superName} @{m_LuaDescript}\nlocal {scriptName} = class('{scriptName}', import('{superName}'))");
                AutoInsertSuperCall(ref content, scriptName, "ctor");
                AutoInsertSuperCall(ref content, scriptName, "Init");
                AutoInsertSuperCall(ref content, scriptName, "Proc");
                AutoInsertSuperCall(ref content, scriptName, "Destroy");
            }
            else
            {
                content = content.Insert(classDefIdx, $"---@class {scriptName} @{m_LuaDescript}\nlocal {scriptName} = class('{scriptName}')");
                AutoRemoveSuperCall(ref content, scriptName, "ctor");
                AutoRemoveSuperCall(ref content, scriptName, "Init");
                AutoRemoveSuperCall(ref content, scriptName, "Proc");
                AutoRemoveSuperCall(ref content, scriptName, "Destroy");
            }

            return new StringBuilder(content, commentEnd, content.Length - commentEnd, content.Length * 2);
        }

        /// <summary>
        /// 自动插入父类调用
        /// </summary>
        private void AutoInsertSuperCall(ref string content, string script, string func)
        {
            string funcLine = $"function {script}:{func}(";
            int idx = content.LastIndexOf(funcLine);
            if (idx == -1) return;

            string superLine = $"{script}.super.{func}(self, args)";
            if (func == "Init" || func == "Proc" || func == "Destroy")
                superLine = $"{script}.super.{func}(self)";

            if (!content.Contains(superLine))
            {
                int lineEnd = content.IndexOf('\n', idx);
                content = content.Insert(lineEnd + 1, $"    {superLine}\n");
            }
        }

        /// <summary>
        /// 自动移除父类调用
        /// </summary>
        private void AutoRemoveSuperCall(ref string content, string script, string func)
        {
            string superLine = $"{script}.super.{func}(self";
            int idx = content.LastIndexOf(superLine);
            if (idx == -1) return;

            int lineEnd = content.IndexOf('\n', idx);
            if (lineEnd > idx)
                content = content.Remove(idx, lineEnd - idx + 1);
        }
        #endregion
    }
}