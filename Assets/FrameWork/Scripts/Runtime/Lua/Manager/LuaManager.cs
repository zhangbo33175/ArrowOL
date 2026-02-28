using System.Collections.Generic;
using System.IO;
using GameLib;
using UnityEngine;
using XLua;

namespace Honor.Runtime
{
    public partial class LuaManager : Singleton<LuaManager>
    {
        /// <summary>
        /// 初始化Lua配置
        /// </summary>
        public void InitLuaConfigs()
        {
            if (LauncherManager.Instance.EditorResourceMode)
            {
                m_LuaScriptsFullPathsMapForLoading.Clear();
                List<string> luaFilePaths = new List<string>();
                luaFilePaths.AddRange(System.IO.Directory.GetFiles(
                    GamePathUtils.LuaScript.Game.GetRootDirectoryRelativePath(), "*.lua.txt",
                    SearchOption.AllDirectories));

                foreach (string path in luaFilePaths)
                {
                    string luaFileFullPath = path.Replace("\\", "/").Replace(".txt", string.Empty);
                    string luaFileTidyName = luaFileFullPath.Substring(luaFileFullPath.LastIndexOf("/") + 1);
                    if (!m_LuaScriptsFullPathsMapForLoading.ContainsKey(luaFileTidyName))
                    {
                        m_LuaScriptsFullPathsMapForLoading[luaFileTidyName] = luaFileFullPath;
                    }
                    else
                    {
                        Log.Error(
                            $"[Lua] 发现重名Lua脚本，请检查，路径1：{m_LuaScriptsFullPathsMapForLoading[luaFileTidyName]}，路径2：{luaFileFullPath}。");
                    }
                }
            }
            else
            {
                m_LuaScriptsABPathsForLoading.Clear();
                string formatABPathUnderGame = AssetManager.Instance.m_LoadABComponent
                    .GetABFormatPath(GamePathUtils.LuaScript.Game.GetRootDirectoryRelativePath())
                    .Replace(".bundle", string.Empty);
                List<string> allPathsUnderGame = new List<string>();
                foreach (string key in AssetManager.Instance.m_DependsDataABList.Keys)
                {
                    if (key.Contains(formatABPathUnderGame))
                    {
                        allPathsUnderGame.Add(AssetManager.Instance.m_LoadABComponent.GetABRestoredPath(key));
                    }
                }

                m_LuaScriptsABPathsForLoading.AddRange(allPathsUnderGame);
            }
        }

        /// <summary>
        /// 初始化Lua环境
        /// </summary>
        public void InitLuaEnv()
        {
            // 初始化Lua环境
            m_Env = new LuaEnv();

            // 添加自定义加载器
            m_Env.AddLoader(Custom_Lua_Loader);

#if UNITY_EDITOR
            // 开启Lua运行时分析模式
            if (m_LuaRuntimeProfilerMode)
            {
                m_Env.DoString("__lua_profiler = require('profiler')");
                m_Env.DoString("__lua_profiler.start()");
            }
#endif
            // // 开启Lua热重载模式
            // if (m_LauncherComponent.LuaHotReloadMode)
            // {
            //     m_Env.DoString(AorTxt.Format("require 'hot_reload'"));
            //     LuaFileWatcher.CreateLuaFileWatcher(m_Env);
            // }

            // 初始化器-Lua入口
            m_Env.DoString(AorTxt.Format("require 'Initializer'"));

            // 默认最近一次的GC调用时间
            m_LastGCTime = Time.time;
        }

        /// <summary>
        /// 自定义Lua加载器
        /// </summary>
        /// <param name="fileName"> lua文件名称</param>
        /// <returns></returns>
        public byte[] Custom_Lua_Loader(ref string fileName)
        {
            byte[] bytes = null;

#if UNITY_EDITOR
            if (fileName == "emmy_core")
            {
                return bytes;
            }
#endif

            if (!fileName.EndsWith(".lua"))
            {
                string typeName = LauncherManager.Instance.LuacMode ? "LuacAsset" : "LuaAsset";
                fileName = AorTxt.Format("{0}.lua", fileName);

                TextAsset asset = null;

                GameConfig.instance.m_StrictCheck = false;

                if (LauncherManager.Instance.EditorResourceMode)
                {
                    string abPath = m_LuaScriptsFullPathsMapForLoading[fileName];
                    asset = (TextAsset)AssetManager.Instance.LoadAssetSync(typeName, abPath, fileName);
                    if (asset == null)
                    {
                        // 将尝试加载失败的AB进行卸载，确保AB引用计数正确
                        AssetManager.Instance.m_LoadABComponent.UnloadAB(abPath);
                    }
                }
                else
                {
                    foreach (string abPath in m_LuaScriptsABPathsForLoading)
                    {
                        if (Can_AB_Load(abPath))
                        {
                            asset = (TextAsset)AssetManager.Instance.LoadAssetSync(typeName, abPath, fileName);
                            if (asset)
                            {
                                break;
                            }
                            else
                            {
                                // 将尝试加载失败的AB进行卸载，确保AB引用计数正确
                                AssetManager.Instance.m_LoadABComponent.UnloadAB(abPath);
                            }
                        }
                    }
                }

                if (asset != null)
                {
                    m_LoadedLuaScriptsNames.Add(fileName);
                    if (LauncherManager.Instance.LuacMode)
                    {
                        bytes = Encryption.GetQuickXorBytes(asset.bytes, s_LuacEncrytionKey);
                    }
                    else
                    {
                        bytes = new List<byte>(asset.bytes).ToArray();
                    }

                    AssetManager.Instance.UnloadAsset(asset);
                }
                else
                {
                    Log.Error(AorTxt.Format("Lua脚本 {0} 加载失败，请检查路径！", fileName));
                }

                /*// 为配合Lua的热重载机制，这里使用文件的直接读取方式对lua进行重载
                if (m_LauncherComponent.EditorResourceMode && m_LauncherComponent.LuaHotReloadMode)
                {
                    if (LuaFileWatcher.TryGetReloadFile(fileName, out var fullPath))
                    {
                        bytes = FileOperation.SafeReadAllBytes(fullPath);
                    }
                }*/

                AssetManager.Instance.StrictCheck = true;
            }
            else
            {
                Log.Error(AorTxt.Format("Lua脚本 {0} 加载失败，require时请勿以.lua结尾！", fileName));
            }

            return bytes;
        }

        /// <summary>
        /// 获取Lua全局Value
        /// </summary>
        /// <typeparam name="T">保存到C#层的数据类型</typeparam>
        /// <param name="name">Lua层的全局Value的名称</param>
        /// <returns></returns>
        public T GetGlobalValue<T>(string name)
        {
            return m_Env.Global.Get<T>(name);
        }

        /// <summary>
        /// 复位Lua（会自动重新加载PreloadProcedure流程）
        /// </summary>
        public void ResetLua()
        {
            ProcedureState.IsReset = true;
            ProcedureManager.Instance.CurrentProcedure.PrepareToNextProcedure(typeof(ProcedurePreload));
        }

        /// <summary>
        /// 清理缓存
        /// </summary>
        public void Clear()
        {
            m_Env = null;
            m_LuaScriptsFullPathsMapForLoading.Clear();
            m_LuaScriptsABPathsForLoading.Clear();
        }


        public override void Dispose()
        {
            Log.Debug("LuaManager");
        }
    }
}