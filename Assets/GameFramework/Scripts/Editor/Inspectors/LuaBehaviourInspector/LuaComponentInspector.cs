using CSObjectWrapEditor;
using System;
using System.Text;
using Honor.Runtime;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;

namespace Honor.Editor
{
    [CustomEditor(typeof(LuaComponent))]
    internal sealed class LuaComponentInspector : HonorComponentInspector
    {
        /// <summary>
        /// Lua运行时分析模式
        /// </summary>
        private SerializedProperty m_LuaRuntimeProfilerMode = null;

        /// <summary>
        /// 自定义生成Lua脚本名称
        /// </summary>
        private string m_CustomLuaScriptName = string.Empty;

        /// <summary>
        /// 自定义生成Lua脚本的父类名称
        /// </summary>
        private string m_CustomLuaSuperScriptName = string.Empty;

        /// <summary>
        /// 使用Proc
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
        private bool m_LuaCanGenerate = false;

        private void OnEnable()
        {
            m_LuaRuntimeProfilerMode = serializedObject.FindProperty("m_LuaRuntimeProfilerMode");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();
            
            EditorGUI.BeginDisabledGroup(Application.isPlaying);
            if (GUILayout.Button("重新生成XLua脚本"))
            {
                Generator.ClearAll();
                GUIUtility.ExitGUI();
            }
            m_LuaRuntimeProfilerMode.boolValue = EditorGUILayout.Toggle("Lua运行时分析模式", m_LuaRuntimeProfilerMode.boolValue);
            EditorGUI.EndDisabledGroup();

            if (Application.isPlaying)
            {
                LuaComponent t = (LuaComponent)target;
                if (m_LuaRuntimeProfilerMode.boolValue)
                {
                    if (GUILayout.Button("导出Lua运行时性能数据到 CSV 文件"))
                    {
                        string exportFileName = EditorUtility.SaveFilePanel("导出 CSV 数据", string.Empty, AorTxt.Format("Lua运行时性能数据 {0}.csv", DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss")), string.Empty);
                        if (!string.IsNullOrEmpty(exportFileName))
                        {
                            try
                            {
                                var results = t.Env.DoString("return __lua_profiler.report()");
                                System.IO.File.WriteAllText(exportFileName, results[0].ToString(), new System.Text.UTF8Encoding(false));
                                Log.Debug(AorTxt.Format("导出 CSV 数据到 '{0}' 成功。", exportFileName));
                            }
                            catch (Exception exception)
                            {
                                Log.Error(AorTxt.Format("导出 CSV 数据到 '{0}' 失败, 异常信息： '{1}'.", exportFileName, exception.ToString()));
                            }
                        }
                        GUIUtility.ExitGUI();
                    }
                }
                
                EditorGUILayout.Separator();

                EditorGUILayout.LabelField("已加载Lua脚本名称列表");
                EditorGUILayout.BeginVertical("box");
                foreach (var name in t.LoadedLuaScriptsNames)
                {
                    EditorGUILayout.LabelField(name);
                }
                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.Separator();

            EditorGUILayout.LabelField("[自定义Lua脚本-快捷创建入口]");
            m_CustomLuaScriptName = EditorGUILayout.TextField("自定义Lua脚本名称", m_CustomLuaScriptName);
            if (!string.IsNullOrEmpty(m_CustomLuaScriptName))
            {
                if (!m_CustomLuaScriptName.StartsWith(".lua") && !m_CustomLuaScriptName.EndsWith(".lua"))
                {
                    m_LuaCanGenerate = true;
                }
                else
                {
                    m_LuaCanGenerate = false;
                    EditorGUILayout.HelpBox("请勿写入.lua命名Lua脚本名称", MessageType.Error);
                }
            }

            m_CustomLuaSuperScriptName = EditorGUILayout.TextField("自定义Lua脚本名称（父类）", m_CustomLuaSuperScriptName);
            if (!string.IsNullOrEmpty(m_CustomLuaSuperScriptName))
            {
                if (!m_CustomLuaSuperScriptName.StartsWith(".lua") && !m_CustomLuaSuperScriptName.EndsWith(".lua"))
                {
                    m_LuaCanGenerate = true;
                }
                else
                {
                    m_LuaCanGenerate = false;
                    EditorGUILayout.HelpBox("请勿写入.lua命名Lua脚本名称", MessageType.Error);
                }
            }

            m_UseProc = EditorGUILayout.Toggle("使用Proc", m_UseProc);
            m_LuaAuthorName = EditorGUILayout.TextField("Lua脚本作者", m_LuaAuthorName);
            m_LuaDescript = EditorGUILayout.TextField("Lua脚本描述", m_LuaDescript);

            if (m_LuaCanGenerate)
            {
                EditorGUILayout.HelpBox("说明：Lua脚本不存在时生成脚本，存在时刷新脚本。", MessageType.Info);
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

        /// <summary>
        /// 生成/刷新Lua文件
        /// </summary>
        private void CreateOrRefreshLuaFile()
        {
            string luaScriptName = m_CustomLuaScriptName;
            string luaRootPath = AorTxt.Format("{0}{1}", Application.dataPath.Substring(0, Application.dataPath.Length - "Assets".Length), GamePathUtils.LuaScript.Game.GetRootDirectoryRelativePath(true));
            string[] fileFullPaths = System.IO.Directory.GetFiles(luaRootPath, AorTxt.Format("{0}.lua.txt", luaScriptName), System.IO.SearchOption.AllDirectories);
            // luaRootPath目录及其子目录中没有该名称的lua文件时需要调起保存窗口
            if (fileFullPaths.Length == 0) // 生成文件
            {
                string exportFileName = EditorUtility.SaveFilePanel(AorTxt.Format("生成{0}.lua.txt文件", luaScriptName), luaRootPath, AorTxt.Format("{0}.lua.txt", luaScriptName), string.Empty);
                if (!string.IsNullOrEmpty(exportFileName))
                {
                    try
                    {
                        StringBuilder stringBuilderCommentLines = GenerateCommentLines();
                        StringBuilder stringBuilderEmptyCodeLines = GenerateEmptyCodeLines();
                        string content = AorTxt.Format("{0}{1}", stringBuilderCommentLines.ToString(), stringBuilderEmptyCodeLines.ToString());
                        System.IO.File.WriteAllText(exportFileName, content, new System.Text.UTF8Encoding(false));

                        Log.Debug(AorTxt.Format("生成{0}文件成功。", exportFileName));

                        AssetDatabase.Refresh();
                    }
                    catch (Exception exception)
                    {
                        Log.Error(AorTxt.Format("生成{0}文件失败, 异常信息： '{1}'.", exportFileName, exception.ToString()));
                    }
                }

            }
            // luaRootPath目录及其子目录中存在该名称的lua文件时需要刷新文件内容
            else if (fileFullPaths.Length == 1)  // 刷新文件
            {
                try
                {
                    StringBuilder stringBuilderCommentLines = GenerateCommentLines();
                    StringBuilder stringBuilderCodeLines = GenerateCodeLines(fileFullPaths[0]);
                    string content = AorTxt.Format("{0}{1}", stringBuilderCommentLines.ToString(), stringBuilderCodeLines.ToString());
                    System.IO.File.WriteAllText(fileFullPaths[0], content, new System.Text.UTF8Encoding(false));

                    Log.Debug(AorTxt.Format("刷新{0}文件成功。", fileFullPaths[0]));

                    AssetDatabase.Refresh();
                }
                catch (Exception exception)
                {
                    Log.Error(AorTxt.Format("刷新{0}文件失败, 异常信息： '{1}'.", fileFullPaths[0], exception.ToString()));
                }
            }
            else
            {
                Log.Error(AorTxt.Format("目录 {0} 及其子目录中遍历到多个文件 {1}.txt ！", luaRootPath, luaScriptName));
            }

        }

        /// <summary>
        /// 生成Lua注释行信息
        /// </summary>
        private StringBuilder GenerateCommentLines()
        {
            StringBuilder stringBuilder = new StringBuilder();
            string luaScriptName = m_CustomLuaScriptName;
            string luaAuthor = m_LuaAuthorName;
            string luaDescript = m_LuaDescript;
            stringBuilder.AppendLine(AorTxt.Format("--====================================================================================================="))
                         .AppendLine(AorTxt.Format("-- (c) copyright 2026 - 2030, Honor.Game"))
                         .AppendLine(AorTxt.Format("-- All Rights Reserved."))
                         .AppendLine(AorTxt.Format("-- ----------------------------------------------------------------------------------------------------"))
                         .AppendLine(AorTxt.Format("-- filename:  {0}", luaScriptName))
                         .AppendLine(AorTxt.Format("-- author:    {0}", luaAuthor))
                         .AppendLine(AorTxt.Format("-- descrip:   {0}", luaDescript))
                         .AppendLine(AorTxt.Format("--====================================================================================================="))
                         .AppendLine(AorTxt.Format(""));
            return stringBuilder;
        }

        /// <summary>
        /// 生成Lua空行
        /// </summary>
        private StringBuilder GenerateEmptyCodeLines()
        {
            StringBuilder stringBuilder = new StringBuilder();

            string luaScriptName = m_CustomLuaScriptName;
            string luaSuperScriptName = m_CustomLuaSuperScriptName;
            string luaDescript = m_LuaDescript;
            string luaName = luaScriptName;

            if (!string.IsNullOrEmpty(luaSuperScriptName))
            {
                stringBuilder.AppendLine(AorTxt.Format("---@class {0} : {1} @{2}", luaName, luaSuperScriptName, luaDescript));
                stringBuilder.AppendLine(AorTxt.Format("local {0} = class('{1}', import('{2}'))", luaName, luaName, luaSuperScriptName));
            }
            else
            {
                stringBuilder.AppendLine(AorTxt.Format("---@class {0} @{1}", luaName, luaDescript));
                stringBuilder.AppendLine(AorTxt.Format("local {0} = class('{1}')", luaName, luaName));
            }

            stringBuilder.AppendLine(AorTxt.Format(""));
            stringBuilder.AppendLine(AorTxt.Format("---构造函数"));
            stringBuilder.AppendLine(AorTxt.Format("---@type fun(args:table):void"));
            stringBuilder.AppendLine(AorTxt.Format("---@param args table @自定义参数"));
            stringBuilder.AppendLine(AorTxt.Format("function {0}:ctor(args)", luaName));
            if (!string.IsNullOrEmpty(luaSuperScriptName))
            {
                stringBuilder.AppendLine(AorTxt.Format("    {0}.super.ctor(self, args)", luaName));
            }
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

            stringBuilder.AppendLine(AorTxt.Format("---初始化"));
            stringBuilder.AppendLine(AorTxt.Format("---@type fun():void"));
            stringBuilder.AppendLine(AorTxt.Format("function {0}:Init()", luaName));
            if (!string.IsNullOrEmpty(luaSuperScriptName))
            {
                stringBuilder.AppendLine(AorTxt.Format("    {0}.super.Init(self)", luaName));
            }
            stringBuilder.AppendLine(AorTxt.Format(""));
            stringBuilder.AppendLine(AorTxt.Format("end"));
            stringBuilder.AppendLine(AorTxt.Format(""));

            if (m_UseProc)
            {
                stringBuilder.AppendLine(AorTxt.Format("---心跳"));
                stringBuilder.AppendLine(AorTxt.Format("---@type fun():void"));
                stringBuilder.AppendLine(AorTxt.Format("function {0}:Proc()", luaName));
                if (!string.IsNullOrEmpty(luaSuperScriptName))
                {
                    stringBuilder.AppendLine(AorTxt.Format("    {0}.super.Proc(self)", luaName));
                }
                stringBuilder.AppendLine(AorTxt.Format(""));
                stringBuilder.AppendLine(AorTxt.Format("end"));
                stringBuilder.AppendLine(AorTxt.Format(""));
            }

            stringBuilder.AppendLine(AorTxt.Format("---销毁"));
            stringBuilder.AppendLine(AorTxt.Format("---@type fun():void"));
            stringBuilder.AppendLine(AorTxt.Format("function {0}:Destroy()", luaName));
            if (!string.IsNullOrEmpty(luaSuperScriptName))
            {
                stringBuilder.AppendLine(AorTxt.Format("    {0}.super.Destroy(self)", luaName));
            }
            stringBuilder.AppendLine(AorTxt.Format(""));
            stringBuilder.AppendLine(AorTxt.Format("end"));
            stringBuilder.AppendLine(AorTxt.Format(""));

            stringBuilder.AppendLine(AorTxt.Format("return {0}", luaName));
            stringBuilder.AppendLine(AorTxt.Format(""));

            return stringBuilder;
        }

        /// <summary>
        /// 截取Lua现有有效代码行
        /// </summary>
        /// <param name="fullPath">全路径</param>
        /// <returns></returns>
        private StringBuilder GenerateCodeLines(string fullPath)
        {
            string luaScriptName = m_CustomLuaScriptName;
            string luaSuperScriptName = m_CustomLuaSuperScriptName;
            string luaName = luaScriptName;
            string luaDescript = m_LuaDescript;

            string commentStr = "--=====================================================================================================";

            string content = System.IO.File.ReadAllText(fullPath);

            int endCommentStrIndex = content.LastIndexOf(commentStr) + commentStr.Length + 4;

            // 追加新的Function定义
            int returnRowIndex = content.LastIndexOf(AorTxt.Format("return {0}", luaName));
            string functionDef = string.Empty;

            if (m_UseProc)
            {
                if (!content.Contains(AorTxt.Format("function {0}:Proc()", luaName)))
                {
                    if (!string.IsNullOrEmpty(luaSuperScriptName))
                    {
                        functionDef = AorTxt.Format("{0}{1}\n{2}\n{3}\n{4}\n\nend\n\n", functionDef, AorTxt.Format("---心跳"), AorTxt.Format("---@type fun():void"), AorTxt.Format("function {0}:Proc()", luaName), AorTxt.Format("    {0}.super.Proc(self)", luaName));
                    }
                    else
                    {
                        functionDef = AorTxt.Format("{0}{1}\n{2}\n{3}\nend\n\n", functionDef, AorTxt.Format("---心跳"), AorTxt.Format("---@type fun():void"), AorTxt.Format("function {0}:Proc()", luaName));
                    }
                }
            }

            content = content.Insert(returnRowIndex, functionDef);

            // 更新类的头部注释
            int classCommentDefIndex = content.LastIndexOf(AorTxt.Format("---@class {0}", luaName));
            int charCount = 0;
            while (content[classCommentDefIndex + charCount] != '\n')
            {
                charCount++;
            }
            content = content.Remove(classCommentDefIndex, charCount + 1);

            // 更新类的头部定义
            int classNameDefIndex = content.LastIndexOf(AorTxt.Format("local {0} = class('{1}'", luaName, luaName));
            charCount = 0;
            while (content[classNameDefIndex + charCount] != '\n')
            {
                charCount++;
            }
            content = content.Remove(classNameDefIndex, charCount);

            if (!string.IsNullOrEmpty(luaSuperScriptName))
            {
                content = content.Insert(classNameDefIndex, AorTxt.Format("---@class {0} : {1} @{2}\n", luaName, luaSuperScriptName, luaDescript));
                classNameDefIndex = content.LastIndexOf(AorTxt.Format("---@class {0} : {1} @{2}\n", luaName, luaSuperScriptName, luaDescript));
                charCount = 0;
                while (content[classNameDefIndex + charCount] != '\n')
                {
                    charCount++;
                }
                content = content.Insert(classNameDefIndex + charCount + 1, AorTxt.Format("local {0} = class('{1}', import('{2}'))", luaName, luaName, luaSuperScriptName));

                int ctorDefIndex = content.LastIndexOf(AorTxt.Format("    {0}.super.ctor(self, args)", luaName));
                if (ctorDefIndex == -1)
                {
                    string ctorDefString = AorTxt.Format("function {0}:ctor(args)", luaName);
                    ctorDefIndex = content.LastIndexOf(ctorDefString);
                    content = content.Insert(ctorDefIndex + ctorDefString.Length, AorTxt.Format("\n    {0}.super.ctor(self, args)", luaName));
                }

                int initDefIndex = content.LastIndexOf(AorTxt.Format("    {0}.super.Init(self)", luaName));
                if (initDefIndex == -1)
                {
                    string initDefString = AorTxt.Format("function {0}:Init()", luaName);
                    initDefIndex = content.LastIndexOf(initDefString);
                    content = content.Insert(initDefIndex + initDefString.Length, AorTxt.Format("\n    {0}.super.Init(self)", luaName));
                }

                int procDefIndex = content.LastIndexOf(AorTxt.Format("    {0}.super.Proc(self)", luaName));
                if (procDefIndex == -1)
                {
                    string procDefString = AorTxt.Format("function {0}:Proc()", luaName);
                    procDefIndex = content.LastIndexOf(procDefString);
                    content = content.Insert(procDefIndex + procDefString.Length, AorTxt.Format("\n    {0}.super.Proc(self)", luaName));
                }

                int destroyDefIndex = content.LastIndexOf(AorTxt.Format("    {0}.super.Destroy(self)", luaName));
                if (destroyDefIndex == -1)
                {
                    string destroyDefString = AorTxt.Format("function {0}:Destroy()", luaName);
                    destroyDefIndex = content.LastIndexOf(destroyDefString);
                    content = content.Insert(destroyDefIndex + destroyDefString.Length, AorTxt.Format("\n    {0}.super.Destroy(self)", luaName));
                }

            }
            else
            {
                content = content.Insert(classNameDefIndex, AorTxt.Format("---@class {0} @{1}\n", luaName, luaDescript));
                classNameDefIndex = content.LastIndexOf(AorTxt.Format("---@class {0} @{1}\n", luaName, luaDescript));
                charCount = 0;
                while (content[classNameDefIndex + charCount] != '\n')
                {
                    charCount++;
                }
                content = content.Insert(classNameDefIndex + charCount + 1, AorTxt.Format("local {0} = class('{1}')", luaName, luaName));

                int ctorDefIndex = content.LastIndexOf(AorTxt.Format("    {0}.super.ctor(self, args)", luaName));
                if (ctorDefIndex != -1)
                {
                    charCount = 0;
                    while (content[ctorDefIndex + charCount] != '\n')
                    {
                        charCount++;
                    }
                    content = content.Remove(ctorDefIndex, charCount + 1);
                }

                int initDefIndex = content.LastIndexOf(AorTxt.Format("    {0}.super.Init(self)", luaName));
                if (initDefIndex != -1)
                {
                    charCount = 0;
                    while (content[initDefIndex + charCount] != '\n')
                    {
                        charCount++;
                    }
                    content = content.Remove(initDefIndex, charCount + 1);
                }

                int procDefIndex = content.LastIndexOf(AorTxt.Format("    {0}.super.Proc(self)", luaName));
                if (procDefIndex != -1)
                {
                    charCount = 0;
                    while (content[procDefIndex + charCount] != '\n')
                    {
                        charCount++;
                    }
                    content = content.Remove(procDefIndex, charCount + 1);
                }

                int destroyDefIndex = content.LastIndexOf(AorTxt.Format("    {0}.super.Destroy(self)", luaName));
                if (destroyDefIndex != -1)
                {
                    charCount = 0;
                    while (content[destroyDefIndex + charCount] != '\n')
                    {
                        charCount++;
                    }
                    content = content.Remove(destroyDefIndex, charCount + 1);
                }

            }

            StringBuilder stringBuilderCodeLines = new StringBuilder(content, endCommentStrIndex, content.Length - endCommentStrIndex, content.Length * 2);
            return stringBuilderCodeLines;
        }

    }
}
