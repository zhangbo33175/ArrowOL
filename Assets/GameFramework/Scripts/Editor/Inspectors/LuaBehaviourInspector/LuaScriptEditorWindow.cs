/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Editor
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  LuaScriptEditorWindow.cs
 * author:    云毅
 * created:   2026
 * descrip:   XLua 脚本工具窗口（Honor 菜单入口）
 ***************************************************************/

using CSObjectWrapEditor;
using System;
using System.Text;
using Honor.Runtime;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Honor.Editor
{
    /// <summary>XLua 脚本工具窗口。Honor → XLua工具。</summary>
    public class LuaScriptEditorWindow : BaseToolkitEditorWindow<LuaScriptEditorWindow>
    {
        #region 字段
        private string m_ScriptName = "";
        private string m_SuperName = "";
        private bool m_UseProc = true;
        private string m_Author = "";
        private string m_Desc = "";
        private TextField m_NameField, m_SuperField, m_AuthorField, m_DescField;
        private Toggle m_ProcToggle;
        private HelpBox m_HelpBox;
        #endregion

        protected override string UIAssetDir =>
            "Assets/GameFramework/Scripts/Editor/Inspectors/LuaBehaviourInspector/";
        protected override string UxmlName => "LuaScriptEditorWindowTool";
        protected override GUIContent Title => new GUIContent("XLua工具");
        protected override Vector2 MinSize => new Vector2(620, 420);
        protected override Vector2 MaxSize => new Vector2(620, 420);

        [MenuItem("Honor/XLua工具", false, 364)]
        private static void OpenWindow() => Open();

        protected override void OnInitData() { }

        protected override void OnBindUI()
        {
            BindClick("regenBtn", () => Generator.ClearAll());

            var profilerToggle = Q<Toggle>("profilerToggle");
            // TODO: 加固健壮性——profilerToggle 查不到时原写法直接空引用，加判空
            if (profilerToggle != null)
            {
                profilerToggle.value = false;
                profilerToggle.RegisterValueChangedCallback(evt => { });
            }

            m_NameField = Q<TextField>("nameField");
            m_SuperField = Q<TextField>("superField");
            m_ProcToggle = Q<Toggle>("procToggle");
            m_AuthorField = Q<TextField>("authorField");
            m_DescField = Q<TextField>("descField");
            m_HelpBox = Q<HelpBox>("helpBox");

            BindClick("genBtn", OnGenerate);
            RequestRepaint();
        }

        private void OnGenerate()
        {
            // TODO: 加固健壮性——UXML 元素缺失时下方字段访问会空引用，统一在入口拦截
            if (m_NameField == null || m_SuperField == null || m_ProcToggle == null
                || m_AuthorField == null || m_DescField == null || m_HelpBox == null)
            {
                Debug.LogWarning("[LuaScriptEditorWindow] 部分 UI 元素未找到，请检查 UXML 布局。");
                return;
            }

            m_ScriptName = m_NameField.value;
            m_SuperName = m_SuperField.value;
            m_UseProc = m_ProcToggle.value;
            m_Author = m_AuthorField.value;
            m_Desc = m_DescField.value;

            if (string.IsNullOrEmpty(m_ScriptName) || m_ScriptName.EndsWith(".lua"))
            {
                m_HelpBox.text = "请勿写入 .lua 后缀";
                return;
            }

            string luaRoot = $"{Application.dataPath.Substring(0, Application.dataPath.Length - "Assets".Length)}{GamePathUtils.LuaScript.Game.GetRootDirectoryRelativePath(true)}";
            string[] files = System.IO.Directory.GetFiles(luaRoot, $"{m_ScriptName}.lua.txt", System.IO.SearchOption.AllDirectories);

            if (files.Length == 0) CreateNew(luaRoot);
            else if (files.Length == 1) RefreshExisting(files[0]);
            else Log.Error($"目录 {luaRoot} 下存在多个 {m_ScriptName}.lua.txt！");
        }

        private void CreateNew(string root)
        {
            string savePath = EditorUtility.SaveFilePanel($"生成 {m_ScriptName}.lua.txt", root, $"{m_ScriptName}.lua.txt", "");
            if (string.IsNullOrEmpty(savePath)) return;
            string content = BuildHeader() + BuildTemplate();
            string tmp = savePath + ".tmp";
            System.IO.File.WriteAllText(tmp, content, new UTF8Encoding(false));
            System.IO.File.Replace(tmp, savePath, null);
            AssetDatabase.Refresh();
        }

        private void RefreshExisting(string path)
        {
            string content = System.IO.File.ReadAllText(path);
            // 简化：重新生成头部，保留原代码体
            string newContent = BuildHeader() + content;
            string tmp = path + ".tmp";
            System.IO.File.WriteAllText(tmp, newContent, new UTF8Encoding(false));
            System.IO.File.Replace(tmp, path, null);
            AssetDatabase.Refresh();
        }

        private string BuildHeader()
        {
            return "--=====================================================================================================\n" +
                   "-- (c) copyright 2026 - 2030, Honor.Runtime\n" +
                   "-- All Rights Reserved.\n" +
                   $"-- filename:  {m_ScriptName}\n" +
                   $"-- author:    {m_Author}\n" +
                   $"-- descrip:   {m_Desc}\n" +
                   "--=====================================================================================================\n\n";
        }

        private string BuildTemplate()
        {
            var sb = new StringBuilder();
            string s = m_ScriptName, sup = m_SuperName, d = m_Desc;
            if (!string.IsNullOrEmpty(sup))
            {
                sb.AppendLine($"---@class {s} : {sup} @{d}");
                sb.AppendLine($"local {s} = class('{s}', import('{sup}'))");
            }
            else
            {
                sb.AppendLine($"---@class {s} @{d}");
                sb.AppendLine($"local {s} = class('{s}')");
            }
            sb.AppendLine();
            sb.AppendLine($"function {s}:ctor(args)");
            if (!string.IsNullOrEmpty(sup)) sb.AppendLine($"    {s}.super.ctor(self, args)");
            sb.AppendLine("end\n");
            sb.AppendLine($"function {s}:Create(args) return {s}.new(args) end\n");
            sb.AppendLine($"function {s}:Init()");
            if (!string.IsNullOrEmpty(sup)) sb.AppendLine($"    {s}.super.Init(self)");
            sb.AppendLine("end\n");
            if (m_UseProc)
            {
                sb.AppendLine($"function {s}:Proc()");
                if (!string.IsNullOrEmpty(sup)) sb.AppendLine($"    {s}.super.Proc(self)");
                sb.AppendLine("end\n");
            }
            sb.AppendLine($"function {s}:Destroy()");
            if (!string.IsNullOrEmpty(sup)) sb.AppendLine($"    {s}.super.Destroy(self)");
            sb.AppendLine("end\n");
            sb.AppendLine($"return {s}");
            return sb.ToString();
        }
    }
}