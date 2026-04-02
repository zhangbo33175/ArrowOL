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
    public delegate void ReloadDelegate(string path);

    public class LuaFileWatcher
    {
        private static ReloadDelegate s_ReloadFunction;

        private static HashSet<string> s_ChangedFiles = new HashSet<string>();
        
        private static Dictionary<string, string> s_ReloadFiles = new();

        public static void CreateLuaFileWatcher(LuaEnv luaEnv)
        {
            System.Environment.SetEnvironmentVariable("MONO_MANAGED_WATCHER", "enabled");

            //new DirectoryWatcher(Path.LuaScript.Framework.GetRootDirectoryRelativePath(true), "*.lua.txt", new FileSystemEventHandler(LuaFileOnChanged));
            new DirectoryWatcher(GamePathUtils.LuaScript.Game.GetRootDirectoryRelativePath(true), "*.lua.txt", new FileSystemEventHandler(LuaFileOnChanged));
            s_ReloadFunction = luaEnv.Global.Get<ReloadDelegate>("__RELOAD_LUA_HOTFIX__");
#if UNITY_EDITOR
            EditorApplication.update -= Reload;
            EditorApplication.update += Reload;
#endif
        }

        private static void LuaFileOnChanged(object obj, FileSystemEventArgs args)
        {
            var fullPath = args.FullPath;
            var requirePath = fullPath.Replace(".lua.txt", "").Replace("\\", "/");
            var luaScriptIndex = requirePath.LastIndexOf("/") + 1;
            requirePath = requirePath.Substring(luaScriptIndex);
            s_ChangedFiles.Add(requirePath);
            var fileName = System.IO.Path.GetFileNameWithoutExtension(fullPath);
            s_ReloadFiles[fileName] = fullPath;
        }

        private static void Reload()
        {
#if UNITY_EDITOR
            if (EditorApplication.isPlaying == false)
            {
                return;
            }
            if (s_ChangedFiles.Count == 0)
            {
                return;
            }

            foreach (var file in s_ChangedFiles)
            {
                s_ReloadFunction(file);
                Log.Debug("Reload file:{0}", file);
            }
            s_ChangedFiles.Clear();
#endif
        }

        public static bool TryGetReloadFile(string fileName, out string fullPath)
        {
            if(s_ReloadFiles.TryGetValue(fileName, out fullPath))
            {
                s_ReloadFiles.Remove(fileName);
                return true;
            }
            return false;
        }
    }
}


