/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Editor
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  LuaScriptCreateMenu.cs
 * author:    云毅
 * created:   2026
 * descrip:   Unity 右键 Assets/Create 快捷创建 .lua.txt 脚本
 *            生成 Honor 工程标准文件头 + None 模式 Lua 骨架
 *            使用：Project 窗口右键 → Create → Lua Script (.lua.txt)
 ***************************************************************/

using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Honor.Editor
{
    /// <summary>
    /// Lua 脚本快捷创建菜单（.lua.txt）
    /// </summary>
    public static class LuaScriptCreateMenu
    {
        private const string MenuPath = "Assets/Create/Lua Script (.lua.txt)";
        private const int MenuPriority = 80;

        [MenuItem(MenuPath, priority = MenuPriority)]
        private static void CreateLuaScript()
        {
            // 确定目标目录（当前选中的目录，未选中则 Assets 根）
            string dir = "Assets";
            if (Selection.activeObject != null)
            {
                string selectedPath = AssetDatabase.GetAssetPath(Selection.activeObject);
                if (Directory.Exists(selectedPath))
                {
                    dir = selectedPath;
                }
                else if (!string.IsNullOrEmpty(selectedPath))
                {
                    dir = Path.GetDirectoryName(selectedPath);
                }
            }

            // 生成唯一资源路径（自动处理重名）
            string assetPath = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(dir, "NewLuaScript.lua.txt"));
            string fullPath = Path.Combine(Directory.GetCurrentDirectory(), assetPath);

            string scriptName = Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(assetPath));
            File.WriteAllText(fullPath, BuildTemplate(scriptName), new UTF8Encoding(false));

            AssetDatabase.Refresh();
            Object asset = AssetDatabase.LoadAssetAtPath<Object>(assetPath);
            if (asset != null)
            {
                EditorGUIUtility.PingObject(asset);
            }
        }

        [MenuItem(MenuPath, true)]
        private static bool ValidateCreateLuaScript()
        {
            // 始终可用
            return true;
        }

        /// <summary>
        /// 生成标准 Lua 模板（Honor None 模式骨架）
        /// </summary>
        private static string BuildTemplate(string name)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("--=====================================================================================================");
            sb.AppendLine("-- (c) copyright 2026 - 2030, Honor.Game");
            sb.AppendLine("-- All Rights Reserved.");
            sb.AppendLine("-- ----------------------------------------------------------------------------------------------------");
            sb.AppendLine($"-- filename:  {name}.lua");
            sb.AppendLine($"-- author:    云毅");
            sb.AppendLine($"-- created:   {System.DateTime.Now.Year}");
            sb.AppendLine("-- descrip:   ");
            sb.AppendLine("--=====================================================================================================");
            sb.AppendLine();
            sb.AppendLine($"---@class {name} : LuaBehaviourSuper");
            sb.AppendLine($"---@field cs Honor.Runtime.LuaBehaviour @LuaBehaviour");
            sb.AppendLine($"local {name} = class('{name}', import('LuaBehaviourSuper'))");
            sb.AppendLine();
            sb.AppendLine("---构造函数");
            sb.AppendLine("---@type fun(args:table):void");
            sb.AppendLine("---@param args table @自定义参数");
            sb.AppendLine($"function {name}:ctor(args)");
            sb.AppendLine($"    {name}.super.ctor(self, args)");
            sb.AppendLine("end");
            sb.AppendLine();
            sb.AppendLine("---创建函数");
            sb.AppendLine($"---@type fun(args:table):{name}");
            sb.AppendLine("---@param args table @自定义参数");
            sb.AppendLine($"---@return {name} @实例");
            sb.AppendLine($"function {name}:Create(args)");
            sb.AppendLine($"    local obj = {name}.new(args)");
            sb.AppendLine("    return obj");
            sb.AppendLine("end");
            sb.AppendLine();
            sb.AppendLine("---注册监听（自动注销）");
            sb.AppendLine("---@type fun():void");
            sb.AppendLine($"function {name}:OnAddListeners()");
            sb.AppendLine($"    {name}.super.OnAddListeners(self)");
            sb.AppendLine("end");
            sb.AppendLine();
            sb.AppendLine("---唤醒");
            sb.AppendLine("---@type fun():void");
            sb.AppendLine($"function {name}:Awake()");
            sb.AppendLine($"    {name}.super.Awake(self)");
            sb.AppendLine("end");
            sb.AppendLine();
            sb.AppendLine("---开始");
            sb.AppendLine("---@type fun():void");
            sb.AppendLine($"function {name}:Start()");
            sb.AppendLine($"    {name}.super.Start(self)");
            sb.AppendLine("end");
            sb.AppendLine();
            sb.AppendLine("---销毁");
            sb.AppendLine("---@type fun():void");
            sb.AppendLine($"function {name}:OnDestroy()");
            sb.AppendLine($"    {name}.super.OnDestroy(self)");
            sb.AppendLine("end");
            sb.AppendLine();
            sb.AppendLine($"return {name}");
            return sb.ToString();
        }
    }
}
