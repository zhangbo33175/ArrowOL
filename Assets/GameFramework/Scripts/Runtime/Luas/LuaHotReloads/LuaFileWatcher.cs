using System.Collections.Generic;
using System.IO;
using System.Text;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using XLua;

namespace Honor.Runtime
{
    /// <summary>
    /// Lua 文件热重载监听
    /// 监听文件变化并自动通知 Lua 层重载
    /// </summary>
    public delegate void ReloadDelegate(string path);

    public class LuaFileWatcher
    {
        private static ReloadDelegate s_ReloadFunction;
        private static readonly HashSet<string> s_ChangedFiles = new HashSet<string>();
        private static readonly Dictionary<string, string> s_ReloadFiles = new Dictionary<string, string>();

        /// <summary>
        /// 创建 Lua 文件监听器
        /// </summary>
        public static void CreateLuaFileWatcher(LuaEnv luaEnv)
        {
            System.Environment.SetEnvironmentVariable("MONO_MANAGED_WATCHER", "enabled");

            // 监听游戏业务 Lua 脚本
            new DirectoryWatcher(GamePathUtils.LuaScript.Game.GetRootDirectoryRelativePath(true),
                "*.lua.txt",
                LuaFileOnChanged);

            // 获取 Lua 层重载函数
            s_ReloadFunction = luaEnv.Global.Get<ReloadDelegate>("__RELOAD_LUA_HOTFIX__");

#if UNITY_EDITOR
            EditorApplication.update -= Reload;
            EditorApplication.update += Reload;
#endif
        }

        /// <summary>
        /// 文件变化回调
        /// </summary>
        private static void LuaFileOnChanged(object obj, FileSystemEventArgs args)
        {
            string fullPath = args.FullPath;
            string requirePath = fullPath.Replace(".lua.txt", "").Replace("\\", "/");
            int luaScriptIndex = requirePath.LastIndexOf("/") + 1;

            if (luaScriptIndex > 0)
                requirePath = requirePath.Substring(luaScriptIndex);

            s_ChangedFiles.Add(requirePath);
            string fileName = Path.GetFileNameWithoutExtension(fullPath);
            s_ReloadFiles[fileName] = fullPath;
        }

        /// <summary>
        /// 编辑器主线程重载
        /// </summary>
        private static void Reload()
        {
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying)
                return;

            if (s_ChangedFiles.Count == 0)
                return;

            foreach (string file in s_ChangedFiles)
            {
                if (s_ReloadFunction != null)
                {
                    s_ReloadFunction(file);
                    Log.Debug("[LuaHotReload] 重载: {0}", file);
                }
            }

            s_ChangedFiles.Clear();
#endif
        }

        /// <summary>
        /// 获取需要重载的文件路径
        /// </summary>
        public static bool TryGetReloadFile(string fileName, out string fullPath)
        {
            if (s_ReloadFiles.TryGetValue(fileName, out fullPath))
            {
                s_ReloadFiles.Remove(fileName);
                return true;
            }

            fullPath = null;
            return false;
        }
    }
}