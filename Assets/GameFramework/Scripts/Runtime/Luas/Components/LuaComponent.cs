using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using XLua;

namespace Honor.Runtime
{
    /// <summary>
    /// Lua脚本管理组件
    /// 负责Lua环境初始化、脚本加载、热重载、资源管理、生命周期管理
    /// 继承自GameComponent，为游戏核心运行时组件
    /// </summary>
    [DisallowMultipleComponent]
    public sealed partial class LuaComponent : GameComponent
    {
        /// <summary>
        /// 初始化组件基础数据
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            // 初始化已加载的Lua脚本名称列表
            m_LoadedLuaScriptsNames = new List<string>();
            // 初始化编辑器模式下Lua脚本全路径映射表
            m_LuaScriptsFullPathsMapForLoading = new Dictionary<string, string>();
            // 初始化AB包模式下Lua脚本AB路径列表
            m_LuaScriptsABPathsForLoading = new List<string>();
        }

        /// <summary>
        /// 获取游戏核心依赖组件
        /// </summary>
        private void Start()
        {
            // 获取启动器组件
            m_LauncherComponent = GameComponentsGroup.GetComponent<LauncherComponent>();
            if (m_LauncherComponent == null)
            {
                Log.Fatal("LauncherComponent 获取失败");
                return;
            }

            // 获取资源管理组件
            m_AssetComponent = GameComponentsGroup.GetComponent<AssetComponent>();
            if (m_AssetComponent == null)
            {
                Log.Fatal("AssetComponent 获取失败");
                return;
            }

            // 获取流程管理组件
            m_ProcedureComponent = GameComponentsGroup.GetComponent<ProcedureComponent>();
            if (m_ProcedureComponent == null)
            {
                Log.Fatal("ProcedureComponent 获取失败");
                return;
            }
        }

        /// <summary>
        /// 定时执行Lua环境GC与逻辑更新
        /// </summary>
        private void Update()
        {
            // 定时触发Lua环境Tick，控制GC频率
            if (m_Env != null && Time.time - m_LastGCTime > m_GCInterval)
            {
                m_Env.Tick();
                m_LastGCTime = Time.time;
            }
        }

        /// <summary
        /// 组件销毁时释放Lua环境，防止内存泄漏
        /// </summary>
        private void OnDestroy()
        {
            // 必须释放 LuaEnv，否则会严重泄漏
            if (m_Env != null)
            {
                m_Env.Dispose();
                m_Env = null;
            }
        }

        /// <summary>
        /// 初始化Lua配置
        /// 区分编辑器模式/AB包模式，构建Lua脚本加载路径映射
        /// </summary>
        public void InitLuaConfigs()
        {
            if (m_LauncherComponent.EditorResourceMode)
            {
                // 编辑器模式：遍历本地Lua文件，构建文件名-全路径映射
                m_LuaScriptsFullPathsMapForLoading.Clear();
                List<string> luaFilePaths = new List<string>();
                luaFilePaths.AddRange(System.IO.Directory.GetFiles(
                    GamePathUtils.LuaScript.Game.GetRootDirectoryRelativePath(), "*.lua.txt",
                    SearchOption.AllDirectories));
                foreach (string path in luaFilePaths)
                {
                    // 格式化路径，去除后缀与斜杠
                    string luaFileFullPath = path.Replace("\\", "/").Replace(".txt", string.Empty);
                    string luaFileTidyName = luaFileFullPath.Substring(luaFileFullPath.LastIndexOf("/") + 1);
                    
                    // 检测重名Lua脚本，避免加载冲突
                    if (!m_LuaScriptsFullPathsMapForLoading.ContainsKey(luaFileTidyName))
                    {
                        m_LuaScriptsFullPathsMapForLoading[luaFileTidyName] = luaFileFullPath;
                    }
                    else
                    {
                        Log.Error($"[Lua] 发现重名Lua脚本，请检查路径：{m_LuaScriptsFullPathsMapForLoading[luaFileTidyName]}，路径2：{luaFileFullPath}。");
                    }
                }
            }
            else
            {
                // AB包模式：从依赖数据中获取Lua脚本AB包路径
                m_LuaScriptsABPathsForLoading.Clear();
                string formatABPathUnderGame = m_AssetComponent.AssetLoadManager.AssetBundleLoadManager
                    .GetABFormatPath(GamePathUtils.LuaScript.Game.GetRootDirectoryRelativePath())
                    .Replace(".bundle", string.Empty);
                List<string> allPathsUnderGame = new List<string>();
                
                // 筛选所有Lua相关AB包路径
                foreach (string key in m_AssetComponent.DependsDataList.Keys)
                {
                    if (key.Contains(formatABPathUnderGame))
                    {
                        allPathsUnderGame.Add(m_AssetComponent.AssetLoadManager.AssetBundleLoadManager.GetABRestoredPath(key));
                    }
                }

                m_LuaScriptsABPathsForLoading.AddRange(allPathsUnderGame);
            }
        }

        /// <summary>
        /// 初始化Lua运行环境
        /// 包含加载器注册、性能分析、热重载、入口脚本执行
        /// </summary>
        public void InitLuaEnv()
        {
            // 创建Lua虚拟机环境
            m_Env = new LuaEnv();
            // 注册自定义Lua脚本加载器
            m_Env.AddLoader(CustomLuaLoader);

#if UNITY_EDITOR
            // 编辑器模式下：开启Lua性能分析器
            if (m_LuaRuntimeProfilerMode)
            {
                m_Env.DoString("__lua_profiler = require('profiler')");
                m_Env.DoString("__lua_profiler.start()");
            }
#endif

            // 开启Lua热重载模式
            if (m_LauncherComponent.LuaHotReloadMode)
            {
                m_Env.DoString(AorTxt.Format("require 'hot_reload'"));
                // 创建Lua文件监听，实现代码热重载
                LuaFileWatcher.CreateLuaFileWatcher(m_Env);
            }

            // 执行Lua游戏入口脚本
            m_Env.DoString(AorTxt.Format("require 'StartGame'"));

            // 记录首次GC时间
            m_LastGCTime = Time.time;
        }

        /// <summary>
        /// 自定义Lua加载器
        /// 支持编辑器/AB包/加密/热重载多种加载模式
        /// </summary>
        /// <param name="fileName">Lua脚本文件名（自动补全.lua后缀）</param>
        /// <returns>Lua脚本字节数据</returns>
        public byte[] CustomLuaLoader(ref string fileName)
        {
            byte[] bytes = null;

#if UNITY_EDITOR
            // 编辑器下跳过EmmyLua调试插件加载
            if (fileName == "emmy_core")
            {
                return bytes;
            }
#endif

            // 自动补全.lua后缀，禁止手动传入带后缀的文件名
            if (!fileName.EndsWith(".lua"))
            {
                // 根据编译模式选择资源类型
                string typeName = m_LauncherComponent.LuacMode ? "LuacAsset" : "LuaAsset";
                fileName = AorTxt.Format("{0}.lua", fileName);

                TextAsset asset = null;

                // 关闭资源严格校验，适配Lua加载
                m_AssetComponent.StrictCheck = false;

                try
                {
                    if (m_LauncherComponent.EditorResourceMode)
                    {
                        // 编辑器模式：从本地路径加载Lua文件
                        string abPath = m_LuaScriptsFullPathsMapForLoading[fileName];
                        asset = (TextAsset)m_AssetComponent.LoadAssetSync(typeName, abPath, fileName);
                        if (asset == null)
                        {
                            // 加载失败时卸载AB包，保证引用计数正确
                            m_AssetComponent.AssetLoadManager.AssetBundleLoadManager.Unload(abPath);
                        }
                    }
                    else
                    {
                        // AB包模式：遍历所有Lua AB包尝试加载
                        foreach (string abPath in m_LuaScriptsABPathsForLoading)
                        {
                            if (CanABLoad(abPath))
                            {
                                asset = (TextAsset)m_AssetComponent.LoadAssetSync(typeName, abPath, fileName);
                                if (asset)
                                {
                                    break;
                                }
                                else
                                {
                                    // 加载失败时卸载AB包，保证引用计数正确
                                    m_AssetComponent.AssetLoadManager.AssetBundleLoadManager.Unload(abPath);
                                }
                            }
                        }
                    }

                    if (asset != null)
                    {
                        // 记录已加载的Lua脚本
                        m_LoadedLuaScriptsNames.Add(fileName);

#if UNITY_EDITOR_WIN
                        // Windows编辑器：设置调试路径，支持断点调试
                        if (m_LuaScriptsFullPathsMapForLoading.TryGetValue(fileName, out string debugPath))
                        {
                            fileName = debugPath + ".txt";
                        }
#endif
                        // 编译模式：解密Lua字节码
                        if (m_LauncherComponent.LuacMode)
                        {
                            bytes = Encryption.GetQuickXorBytes(asset.bytes, s_LuacEncrytionKey);
                        }
                        else
                        {
                            // 普通模式：直接读取文本数据
                            bytes = new List<byte>(asset.bytes).ToArray();
                        }

                        // 卸载资源，减少内存占用
                        m_AssetComponent.UnloadAsset(asset);
                    }
                    else
                    {
                        Log.Error(AorTxt.Format("Lua脚本 {0} 加载失败，请检查路径！", fileName));
                    }

                    // 编辑器热重载模式：直接读取本地文件覆盖加载
                    if (m_LauncherComponent.EditorResourceMode && m_LauncherComponent.LuaHotReloadMode)
                    {
                        if (LuaFileWatcher.TryGetReloadFile(fileName, out var fullPath))
                        {
                            bytes = FileOperation.SafeReadAllBytes(fullPath);
                        }
                    }

                    // 恢复资源严格校验
                    m_AssetComponent.StrictCheck = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            else
            {
                Log.Error(AorTxt.Format("Lua脚本 {0} 加载失败，require时请勿以.lua结尾！", fileName));
            }

            return bytes;
        }

        /// <summary>
        /// 获取Lua全局变量
        /// </summary>
        /// <typeparam name="T">变量类型</typeparam>
        /// <param name="name">全局变量名称</param>
        /// <returns>全局变量值</returns>
        public T GetGlobalValue<T>(string name)
        {
            return m_Env.Global.Get<T>(name);
        }

        /// <summary>
        /// 重启Lua环境
        /// 切换到预加载流程，重新初始化Lua
        /// </summary>
        public void ResetLua()
        {
            ProcedureState.IsReset = true;
            GameMainRoot.Procedure.CurrentProcedure.PrepareToNextProcedure(typeof(ProcedurePreload));
        }

        /// <summary>
        /// 清理Lua环境与缓存数据
        /// 用于场景切换/完全重置时调用
        /// </summary>
        public void Clear()
        {
            // 释放Lua虚拟机
            if (m_Env != null)
            {
                m_Env.Dispose();
                m_Env = null;
            }

            // 清空所有路径映射与加载记录
            m_LuaScriptsFullPathsMapForLoading.Clear();
            m_LuaScriptsABPathsForLoading.Clear();
            m_LoadedLuaScriptsNames.Clear();
        }
    }
}