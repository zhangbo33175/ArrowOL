/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  LuaComponent.cs
 * author:    云毅
 * created:   2026   2025-12-29
 * descrip:   Lua 虚拟机管理核心组件 —— 环境创建、脚本加载、GC、热重载
 ***************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using XLua;

namespace Honor.Runtime
{
    /// <summary>
    /// Lua 虚拟机核心管理组件
    /// 负责 Lua 环境初始化、脚本加载、资源管理、GC 控制、热重载
    /// </summary>
    [DisallowMultipleComponent]
    public sealed partial class LuaComponent : GameComponent
    {
        //=========================================================================
        #region 生命周期
        //=========================================================================

        /// <summary>
        /// 组件初始化
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            m_LoadedLuaScriptsNames = new List<string>();
            m_LuaScriptsFullPathsMapForLoading = new Dictionary<string, string>();
            m_LuaScriptsABPathsForLoading = new List<string>();
        }

        /// <summary>
        /// 依赖组件获取
        /// </summary>
        private void Start()
        {
            m_LauncherComponent = GameComponentsGroup.GetComponent<LauncherComponent>();
            m_AssetComponent = GameComponentsGroup.GetComponent<AssetComponent>();
            m_ProcedureComponent = GameComponentsGroup.GetComponent<ProcedureComponent>();

            if (m_LauncherComponent == null) Log.Fatal("[Lua] LauncherComponent 获取失败");
            if (m_AssetComponent == null) Log.Fatal("[Lua] AssetComponent 获取失败");
            if (m_ProcedureComponent == null) Log.Fatal("[Lua] ProcedureComponent 获取失败");
        }

        /// <summary>
        /// 定时触发 Lua GC
        /// </summary>
        private void Update()
        {
            if (m_Env == null) return;
            if (Time.time - m_LastGCTime > m_GCInterval)
            {
                m_Env.Tick();
                m_LastGCTime = Time.time;
            }
        }

        /// <summary>
        /// 组件销毁 —— 安全释放 Lua 环境
        /// </summary>
        private void OnDestroy()
        {
            if (m_Env != null)
            {
                m_Env.Tick();
                m_Env.FullGc();
                m_Env.Dispose();
                m_Env = null;
            }
        }

        #endregion

        //=========================================================================
        #region 初始化配置
        //=========================================================================

        /// <summary>
        /// 初始化 Lua 加载路径配置（编辑器 / AB 包）
        /// </summary>
        public void InitLuaConfigs()
        {
            if (m_LauncherComponent.EditorResourceMode)
                InitEditorLuaPathMapping();
            else
                InitAssetBundleLuaPaths();
        }

        /// <summary>
        /// 编辑器模式：初始化 Lua 文件路径映射表
        /// </summary>
        private void InitEditorLuaPathMapping()
        {
            m_LuaScriptsFullPathsMapForLoading.Clear();
            string luaRoot = GamePathUtils.LuaScript.Game.GetRootDirectoryRelativePath();
            string[] files = Directory.GetFiles(luaRoot, "*.lua.txt", SearchOption.AllDirectories);

            foreach (string path in files)
            {
                string cleanPath = path.Replace("\\", "/").Replace(".txt", string.Empty);
                string fileName = cleanPath.Substring(cleanPath.LastIndexOf("/") + 1);

                if (!m_LuaScriptsFullPathsMapForLoading.ContainsKey(fileName))
                    m_LuaScriptsFullPathsMapForLoading[fileName] = cleanPath;
                else
                    Log.Error($"[Lua] 重名脚本：{fileName}");
            }
        }

        /// <summary>
        /// AB 包模式：初始化 Lua AB 路径列表
        /// </summary>
        private void InitAssetBundleLuaPaths()
        {
            m_LuaScriptsABPathsForLoading.Clear();
            string luaABRoot = m_AssetComponent.AssetLoadManager.AssetBundleLoadManager
                .GetABFormatPath(GamePathUtils.LuaScript.Game.GetRootDirectoryRelativePath())
                .Replace(".bundle", string.Empty);

            foreach (string key in m_AssetComponent.DependsDataList.Keys)
            {
                if (key.Contains(luaABRoot))
                {
                    string path = m_AssetComponent.AssetLoadManager.AssetBundleLoadManager.GetABRestoredPath(key);
                    m_LuaScriptsABPathsForLoading.Add(path);
                }
            }
        }

        /// <summary>
        /// 初始化 Lua 虚拟机、加载器、热重载、入口脚本
        /// </summary>
        public void InitLuaEnv()
        {
            m_Env = new LuaEnv();
            m_Env.AddLoader(CustomLuaLoader);

#if UNITY_EDITOR
            if (m_LuaRuntimeProfilerMode)
            {
                m_Env.DoString("__lua_profiler = require('profiler')");
                m_Env.DoString("__lua_profiler.start()");
            }
#endif

            if (m_LauncherComponent.LuaHotReloadMode)
            {
                m_Env.DoString("require 'hot_reload'");
                LuaFileWatcher.CreateLuaFileWatcher(m_Env);
            }

            m_Env.DoString("require 'StartGame'");
            m_LastGCTime = Time.time;
        }

        #endregion

        //=========================================================================
        #region 自定义加载器
        //=========================================================================

        /// <summary>
        /// 自定义 Lua 脚本加载器
        /// </summary>
        public byte[] CustomLuaLoader(ref string fileName)
        {
            byte[] bytes = null;

#if UNITY_EDITOR
            if (fileName == "emmy_core") return null;
#endif

            if (fileName.EndsWith(".lua"))
            {
                Log.Error($"[Lua] 禁止传入带后缀：{fileName}");
                return null;
            }

            fileName = $"{fileName}.lua";
            string assetType = m_LauncherComponent.LuacMode ? "LuacAsset" : "LuaAsset";
            m_AssetComponent.StrictCheck = false;

            try
            {
                TextAsset asset = LoadLuaTextAsset(fileName, assetType);
                if (asset != null)
                {
                    m_LoadedLuaScriptsNames.Add(fileName);
                    SetDebugFilePath(ref fileName);

                    bytes = m_LauncherComponent.LuacMode
                        ? Encryption.GetQuickXorBytes(asset.bytes, s_LuacEncrytionKey)
                        : asset.bytes;

                    m_AssetComponent.UnloadAsset(asset);
                    TryHotReloadOverride(ref bytes, fileName);
                }
                else
                {
                    Log.Error($"[Lua] 脚本加载失败：{fileName}");
                }
            }
            catch (Exception e)
            {
                Log.Error($"[Lua] 加载异常：{e.Message}");
            }
            finally
            {
                m_AssetComponent.StrictCheck = true;
            }

            return bytes;
        }

        /// <summary>
        /// 加载 Lua 脚本资源
        /// </summary>
        private TextAsset LoadLuaTextAsset(string fileName, string assetType)
        {
            TextAsset asset = null;

            if (m_LauncherComponent.EditorResourceMode)
            {
                if (m_LuaScriptsFullPathsMapForLoading.TryGetValue(fileName, out string path))
                {
                    asset = m_AssetComponent.LoadAssetSync(assetType, path, fileName) as TextAsset;
                    if (asset == null)
                        m_AssetComponent.AssetLoadManager.AssetBundleLoadManager.Unload(path);
                }
            }
            else
            {
                foreach (string abPath in m_LuaScriptsABPathsForLoading)
                {
                    if (!CanABLoad(abPath)) continue;

                    asset = m_AssetComponent.LoadAssetSync(assetType, abPath, fileName) as TextAsset;
                    if (asset != null) break;

                    m_AssetComponent.AssetLoadManager.AssetBundleLoadManager.Unload(abPath);
                }
            }

            return asset;
        }

        /// <summary>
        /// 设置编辑器调试文件路径
        /// </summary>
        private void SetDebugFilePath(ref string fileName)
        {
#if UNITY_EDITOR_WIN
            if (m_LuaScriptsFullPathsMapForLoading.TryGetValue(fileName, out string debugPath))
            {
                fileName = debugPath + ".txt";
            }
#endif
        }

        /// <summary>
        /// 热重载模式：直接读取本地文件覆盖
        /// </summary>
        private void TryHotReloadOverride(ref byte[] bytes, string fileName)
        {
            if (!m_LauncherComponent.EditorResourceMode || !m_LauncherComponent.LuaHotReloadMode)
                return;

            if (LuaFileWatcher.TryGetReloadFile(fileName, out string fullPath))
            {
                bytes = FileOperation.SafeReadAllBytes(fullPath);
            }
        }

        #endregion

        //=========================================================================
        #region 公共接口
        //=========================================================================

        /// <summary>
        /// 获取 Lua 全局变量
        /// </summary>
        public T GetGlobalValue<T>(string name) => m_Env.Global.Get<T>(name);

        /// <summary>
        /// 重启 Lua 环境
        /// </summary>
        public void ResetLua()
        {
            ProcedureState.IsReset = true;
            GameMainRoot.Procedure.CurrentProcedure.PrepareToNextProcedure(typeof(ProcedurePreload));
        }

        /// <summary>
        /// 清空 Lua 环境与缓存数据
        /// </summary>
        public void Clear()
        {
            if (m_Env != null)
            {
                m_Env.Dispose();
                m_Env = null;
            }

            m_LoadedLuaScriptsNames.Clear();
            m_LuaScriptsFullPathsMapForLoading.Clear();
            m_LuaScriptsABPathsForLoading.Clear();
        }

        #endregion
    }
}