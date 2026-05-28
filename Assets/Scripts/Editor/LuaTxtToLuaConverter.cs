/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  LuaTxtToLuaConverter.cs
 * author:    云毅
 * created:   2026
 * descrip:   编辑器工具 - 批量将 .lua 重命名为 .lua.txt
 ***************************************************************/

using UnityEditor;
using System.IO;
using UnityEngine;

/// <summary>
/// Lua 文本转换器
/// 功能：将 .lua 脚本批量重命名为 .lua.txt（用于热更新/文本加载）
/// </summary>
public class LuaTxtToLuaConverter : EditorWindow
{
    //=========================================================================
    // 菜单入口 & 转换逻辑
    //=========================================================================
    #region Menu & Convert Logic
    /// <summary>
    /// 编辑器菜单：Tools -> Convert All .lua to .lua.txt
    /// </summary>
    [MenuItem("Tools/Convert All .lua to .lua.txt")]
    static void Convert()
    {
        // Lua 脚本所在目录
        string luaPath = Application.dataPath + "/LuaScripts";

        // 检查目录是否存在
        if (!Directory.Exists(luaPath))
        {
            EditorUtility.DisplayDialog("错误", "未找到目录：" + luaPath, "OK");
            return;
        }

        // 获取所有子目录下的 .lua 文件
        foreach (string file in Directory.GetFiles(luaPath, "*.lua", SearchOption.AllDirectories))
        {
            // 跳过已经是 .lua.txt 的文件
            if (file.EndsWith(".lua.txt"))
                continue;

            // 将后缀 .lua 替换为 .lua.txt
            string newFile = file.Replace(".lua", ".lua.txt");

            // 执行重命名
            File.Move(file, newFile);
        }

        // 刷新 Unity 资源数据库
        AssetDatabase.Refresh();

        // 弹出完成提示
        EditorUtility.DisplayDialog("完成", "所有 .lua 已转为 .lua.txt！", "OK");
    }
    #endregion
}