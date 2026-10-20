/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  LuaComponent.cs
 * author:    云毅
 * created:   2026
 * descrip:   Lua 虚拟机管理核心组件 —— 环境创建、脚本加载、GC、热重载
 ***************************************************************/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using XLua;

namespace Honor.Runtime
{
    /// <summary>
    /// Lua 虚拟机核心管理组件
    /// 负责 Lua 环境初始化、脚本加载、资源管理、GC 控制、热重载
    /// </summary>
    [DisallowMultipleComponent]
    public sealed partial class LuaComponent : Singleton<LuaComponent>
    {
        //=========================================================================
        #region 生命周期
        //=========================================================================

        /// <summary>
        /// 组件初始化
        /// </summary>
        public void Init()
        {
            m_LauncherComponent = GameComponentsGroup.GetComponent<LauncherComponent>();
            m_AssetComponent = AssetComponent.Instance;
            m_LoadedLuaScriptsNames = new List<string>();
            m_LuaScriptsFullPathsMapForLoading = new Dictionary<string, string>();
            m_LuaScriptsABPathsForLoading = new List<string>();

            // 依赖组件获取失败时立即报警，避免后续空引用
            if (m_LauncherComponent == null) Log.Fatal("[Lua] LauncherComponent 获取失败");
            if (m_AssetComponent == null) Log.Fatal("[Lua] AssetComponent 获取失败");
        }

        /// <summary>
        /// 定时触发 Lua GC
        /// <para>LuaComponent 为纯 C# 单例（非 MonoBehaviour），生命周期 Update 不会被 Unity 调用，
        /// 由 GameMainRoot 每帧驱动本方法，保证 Lua GC 定时触发。</para>
        /// </summary>
        internal void DriveUpdate()
        {
            if (m_Env == null) return;
            if (Time.time - s_LastGCTime > s_GCInterval)
            {
                m_Env.Tick();
                s_LastGCTime = Time.time;
            }
        }

        #endregion

        //=========================================================================
        #region 分帧加载状态
        //=========================================================================

        /// <summary>
        /// StartGame 顶层依赖模块清单（分帧加载用）
        /// </summary>
        /// <remarks>
        /// 与 StartGame.lua 的顶层 require 保持一致。
        /// 即使清单与 StartGame.lua 不完全一致也安全：Lua require 幂等，
        /// 未在清单内的模块会由 StartGame 内联 require 兜底加载（仅分帧不完整，不影响正确性）。
        /// </remarks>
        private readonly string[] m_StartGamePreloadModules =
        {
            "SystemApis", "SystemDefs", "SystemFuncs", "SystemBridges",
            "EventCmd", "SystemBootstrap", "GameMain"
        };

        /// <summary>
        /// 分帧加载顶层模块的当前索引
        /// </summary>
        private int m_StartGameModuleIndex;

        /// <summary>
        /// StartGame 主体（GC + 回调定义）是否已执行
        /// </summary>
        private bool m_StartGameEntryDone;

        /// <summary>
        /// 逐文件预取 Lua 脚本的当前索引
        /// 跨帧预热「模块→AB路径」缓存与源码字节缓存，避免重模块 require 时一次性全量扫描全部 AB 路径造成长阻塞
        /// </summary>
        private int m_LuaPrefetchIndex;

        /// <summary>
        /// 预取到的 Lua 源码字节缓存（fileName → bytes），require 时直接命中免去 AB 加载
        /// 不区分大小写：预取从全小写 AB 路径枚举，require 名为驼峰，需统一匹配
        /// </summary>
        private readonly Dictionary<string, byte[]> m_PreloadedLuaBytesCache =
            new Dictionary<string, byte[]>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// 逐文件执行计划（DFS 后序拓扑，依赖先于依赖者）
        /// 把原先"整模块 require（整棵依赖树同步编译+执行）"拆成逐文件执行，消除主线程长阻塞
        /// </summary>
        private readonly List<string> m_LuaPreloadExecutionPlan = new();

        /// <summary>
        /// 逐文件执行计划的当前索引
        /// </summary>
        private int m_LuaPreloadExecIndex;

        /// <summary>
        /// 拓扑排序访问标记（防止环与重复）；不区分大小写，兼容预取小写名与 require 驼峰名
        /// </summary>
        private readonly HashSet<string> m_LuaPlanVisited =
            new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// 依赖图：模块名 → 其直接 require 的项目内模块列表（不区分大小写，见上）
        /// </summary>
        private readonly Dictionary<string, List<string>> m_LuaDependencyGraph =
            new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// 聚合器模块集合（顶层强制创建全局 `X = {}`，其依赖写入/访问该全局）。
        /// 此类模块须"依赖者先"执行（前序）以先创建全局，避免依赖提前访问 nil
        /// 或被依赖者重置清空写入（如 SaveRootData 重置 SaveRootData 清空 GameDataPB 的写入）。
        /// 非聚合器仍按依赖先后序执行，保持逐文件平滑。
        /// </summary>
        private readonly HashSet<string> m_LuaAggregatorModules =
            new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// 聚合器识别正则：顶层（列 0，无缩进）`X = {`（强制创建全局，无 `or` 保留语义）
        /// </summary>
        private static readonly Regex s_AggregatorRegex = new Regex(
            @"(?m)^[A-Za-z_]\w*\s*=\s*\{",
            RegexOptions.Compiled);

        #endregion

        //=========================================================================
        #region 初始化配置
        //=========================================================================

        /// <summary>
        /// 初始化 Lua 加载路径配置（编辑器 / AB 包）
        /// </summary>
        public void InitLuaConfigs()
        {
            if (m_LauncherComponent == null) m_LauncherComponent = GameComponentsGroup.GetComponent<LauncherComponent>();
            if (m_AssetComponent == null) m_AssetComponent = AssetComponent.Instance;
            Stopwatch sw = Stopwatch.StartNew();
            if (m_LauncherComponent.EditorResourceMode)
                InitEditorLuaPathMapping();
            else
                InitAssetBundleLuaPaths();
            Log.Info($"[启动耗时] InitLuaConfigs({(m_LauncherComponent.EditorResourceMode ? "Editor" : "AB")}) = {sw.ElapsedMilliseconds}ms");
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
            m_LuaModuleABPathCache.Clear();
            m_LuaModuleNotFoundCache.Clear();
            string luaABRoot = AssetBundleLoadManager
                .GetABFormatPath(GamePathUtils.LuaScript.Game.GetRootDirectoryRelativePath())
                .Replace(".bundle", string.Empty);

            foreach (string key in m_AssetComponent.DependsDataList.Keys)
            {
                if (key.Contains(luaABRoot))
                {
                    string path = AssetBundleLoadManager.GetABRestoredPath(key);
                    m_LuaScriptsABPathsForLoading.Add(path);
                }
            }
        }

        /// <summary>
        /// 初始化 Lua 虚拟机、加载器、热重载（入口脚本改为分帧加载，见 <see cref="RequireNextStartGameModule"/>）
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

            // 重置分帧加载状态（入口脚本在 Loading 进度中分帧 require，避免一次性阻塞主线程）
            m_StartGameModuleIndex = 0;
            m_StartGameEntryDone = false;
            ResetPreloadState();
            s_LastGCTime = Time.time;
        }

        /// <summary>
        /// StartGame 顶层依赖模块数量（供 Preload 逐模块注册加载步骤，使进度随模块加载推进）
        /// </summary>
        public int StartGamePreloadModuleCount => m_StartGamePreloadModules.Length;

        /// <summary>
        /// 加载下一个 StartGame 顶层依赖模块（每步一个，供 Preload 步骤逐模块推进加载进度）
        /// </summary>
        /// <returns>是否加载完成一个模块；全部模块已加载完成或主体已执行后返回 false</returns>
        public bool LoadNextStartGameModule()
        {
            if (m_StartGameEntryDone || m_StartGameModuleIndex >= m_StartGamePreloadModules.Length)
                return false;

            string moduleName = m_StartGamePreloadModules[m_StartGameModuleIndex++];
            Stopwatch sw = Stopwatch.StartNew();
            m_Env.DoString($"require '{moduleName}'");
            Log.Info($"[启动耗时] 加载Lua模块 {moduleName} = {sw.ElapsedMilliseconds}ms");
            return true;
        }

        /// <summary>
        /// 执行 StartGame 主体（GC 优化 + 全局回调定义），应在所有顶层模块加载完成后调用一次
        /// </summary>
        /// <returns>是否本次执行了主体；已执行过返回 false</returns>
        public bool ExecuteStartGameBody()
        {
            if (m_StartGameEntryDone)
                return true; // 已执行过，视为完成，避免 Preload 步骤永久重试卡住

            Log.Info("[LuaExec] 开始执行 StartGame 主体(GC/回调定义)");
            Stopwatch sw = Stopwatch.StartNew();
            try
            {
                // 其内联 require 因模块已加载均为 no-op，仅执行 GC 优化与全局回调定义
                m_Env.DoString("require 'StartGame'");
                m_StartGameEntryDone = true;
            }
            catch (Exception e)
            {
                Log.Error($"[Lua] StartGame 主体执行异常：{e.Message}");
            }
            Log.Info($"[启动耗时] StartGame 主体(GC/回调定义) = {sw.ElapsedMilliseconds}ms");
            return true;
        }

        /// <summary>
        /// 分帧加载全部 StartGame 顶层依赖模块，直至 StartGame 主体完成（每帧调用一次）
        /// 保留给 XLua wrap / Lua 侧调用（Honor_Runtime_LuaComponentWrap 已导出此方法）；
        /// C# 启动链路（ProcedurePreload）改用 <see cref="LoadNextStartGameModule"/> +
        /// <see cref="ExecuteStartGameBody"/> 逐模块拆分，以推进加载进度。
        /// </summary>
        /// <returns>是否全部加载完成</returns>
        public bool RequireNextStartGameModule()
        {
            if (!m_StartGameEntryDone)
            {
                if (LoadNextStartGameModule())
                    return false;

                ExecuteStartGameBody();
            }

            return true;
        }

        /// <summary>
        /// 待预取 Lua 脚本总数（供 ProcedurePreload 逐文件注册步骤）
        /// </summary>
        public int LuaPrefetchModuleCount => m_LuaScriptsABPathsForLoading.Count;

        /// <summary>
        /// 确保 Lua 脚本 AB 路径已枚举（供 Preload 在注册预取步骤前取得数量）。
        /// 注册时机早于"初始化Lua配置"步骤执行，故此处需先行构建路径列表；
        /// 配置步骤执行时会再次重建（幂等），不影响正确性。
        /// </summary>
        public void EnsureLuaScriptPathsBuilt()
        {
            if (m_LuaScriptsABPathsForLoading.Count == 0)
                InitAssetBundleLuaPaths();
        }

        /// <summary>
        /// 预取下一个 Lua 脚本（每步一个，跨帧）
        /// 定位其 AB 路径并缓存源码字节，预热「模块→AB路径」缓存；
        /// 之后入口模块 require 时直接命中缓存，把原先重模块一次性全量扫描全部 AB 路径的耗时
        /// 拆成逐文件平滑进度，避免主线程长阻塞冻结 Loading 界面。
        /// </summary>
        /// <returns>是否已预取一个模块；全部预取完成返回 false</returns>
        public bool PrefetchNextLuaModule()
        {
            if (m_LuaPrefetchIndex >= m_LuaScriptsABPathsForLoading.Count)
                return false;

            string abPath = m_LuaScriptsABPathsForLoading[m_LuaPrefetchIndex++];
            string fileName = DeriveLuaFileName(abPath);
            if (string.IsNullOrEmpty(fileName))
                return true;

            // 已预取（重名/重复路径）跳过
            if (m_PreloadedLuaBytesCache.ContainsKey(fileName))
                return true;

            string assetType = LuaAssetTypeName;
            m_AssetComponent.StrictCheck = false;
            try
            {
                TextAsset asset = m_AssetComponent.LoadAssetSync(assetType, abPath, fileName) as TextAsset;
                if (asset != null)
                {
                    m_PreloadedLuaBytesCache[fileName] = asset.bytes;
                    m_LuaModuleABPathCache[fileName] = abPath;
                    m_AssetComponent.UnloadAsset(asset);
                }
            }
            catch (Exception e)
            {
                // 预取失败不阻断：该脚本后续按需加载（Loader 全量扫描兜底），不影响正确性
                Log.Warning($"[Lua] 预取脚本失败(将按需加载)：{fileName} - {e.Message}");
            }
            finally
            {
                m_AssetComponent.StrictCheck = true;
            }

            return true;
        }

        /// <summary>
        /// 从 AB 还原路径反推 Lua 文件名（require 使用的 fileName，形如 "Gamemain.lua"）
        /// 大小写与 require 名不符或推导失败时返回 null，后续按需加载兜底，不影响正确性。
        /// </summary>
        private string DeriveLuaFileName(string abPath)
        {
            string fileName = Path.GetFileName(HonorUtils.NormalizePath(abPath));
            if (string.IsNullOrEmpty(fileName))
                return null;

            if (fileName.EndsWith(".lua.txt", StringComparison.OrdinalIgnoreCase))
                return fileName.Substring(0, fileName.Length - ".txt".Length);
            if (fileName.EndsWith(".lua", StringComparison.OrdinalIgnoreCase))
                return fileName;
            return null;
        }

        /// <summary>
        /// 构建逐文件执行计划（应在全部预取完成后调用一次）
        /// 解析每个已预取模块源码中的 require，构建依赖图并按 DFS 后序拓扑排序，
        /// 使任何模块执行前其依赖均已加载（require 命中缓存为 no-op），
        /// 从而把重模块"整棵依赖树同步编译+执行"拆成逐文件执行。
        /// 计划为空时安全兜底：StartGame 主体仍会同步 require 全部依赖，不破坏正确性。
        /// </summary>
        public void BuildLuaPreloadExecutionPlan()
        {
            m_LuaDependencyGraph.Clear();
            m_LuaAggregatorModules.Clear();
            m_LuaPlanVisited.Clear();
            m_LuaPreloadExecutionPlan.Clear();

            // 1. 构建依赖图：每个已预取模块 → 其直接 require（仅保留项目内已缓存模块，外部/框架由 require 时正常加载）
            //    同时识别"聚合器"（顶层强制创建全局 X = {}，其依赖写入/访问该全局），该类模块须依赖者先执行
            foreach (KeyValuePair<string, byte[]> kv in m_PreloadedLuaBytesCache)
            {
                string moduleName = StripLuaSuffix(kv.Key);
                if (moduleName == null)
                    continue;
                m_LuaDependencyGraph[moduleName] = ParseRequires(kv.Value);
                if (s_AggregatorRegex.IsMatch(Encoding.UTF8.GetString(kv.Value)))
                    m_LuaAggregatorModules.Add(moduleName);
            }

            // 2. 从入口根模块 DFS 拓扑（混合序）：
            //    聚合器（顶层强制创建全局）前序"依赖者先"，先创建全局再逐层加载依赖，避免依赖提前访问 nil 或被聚合器重置清空写入；
            //    非聚合器后序"依赖先"，依赖命中缓存为 no-op，保持逐文件平滑，避免重模块整棵依赖树同步编译+执行造成主线程长阻塞。
            //    含 StartGame：其独有依赖也逐文件拆解，主体步骤变为 no-op 快速执行
            //    仅覆盖启动必需（根可达）模块；未在本计划的孤立脚本由运行时按需加载，避免执行非启动路径代码
            foreach (string root in m_StartGamePreloadModules)
                VisitForPlan(root);
            VisitForPlan("StartGame");

            m_LuaPreloadExecIndex = 0;
            Log.Info($"[Lua] 执行计划构建完成，共 {m_LuaPreloadExecutionPlan.Count} 个启动必需脚本待逐文件执行");
        }

        /// <summary>
        /// 执行下一个 Lua 脚本（每步一个，跨帧；按拓扑序 require，命中预取缓存，单文件编译+执行）
        /// 单个脚本执行失败仅记录错误不中断（该文件留待依赖者 require 时由 Lua 兜底加载），
        /// 避免非启动路径脚本报错拖垮整个加载流程。
        /// </summary>
        /// <returns>是否已执行一个脚本；计划全部执行完成返回 false</returns>
        public bool ExecuteNextLuaPreloadModule()
        {
            if (m_LuaPreloadExecIndex >= m_LuaPreloadExecutionPlan.Count)
                return false;

            string moduleName = m_LuaPreloadExecutionPlan[m_LuaPreloadExecIndex];
            Stopwatch sw = Stopwatch.StartNew();
            // 开始/完成打点：定位逐文件执行卡住的脚本（卡住处最后一条"开始"无对应"完成"）
            Log.Info($"[LuaExec] 开始执行 {moduleName}（{m_LuaPreloadExecIndex + 1}/{m_LuaPreloadExecutionPlan.Count}）");
            try
            {
                m_Env.DoString($"require '{moduleName}'");
            }
            catch (Exception e)
            {
                Log.Error($"[Lua] 逐文件执行脚本失败(由依赖者兜底加载)：{moduleName} - {e.Message}");
            }
            Log.Info($"[LuaExec] 完成执行 {moduleName} = {sw.ElapsedMilliseconds}ms");
            m_LuaPreloadExecIndex++;
            return true;
        }

        /// <summary>
        /// 重置分帧预取/逐文件执行的运行时状态
        /// 初始化（InitLuaEnv）与清空（Clear）均需恢复此状态，抽离以避免重复
        /// </summary>
        private void ResetPreloadState()
        {
            m_LuaPrefetchIndex = 0;
            m_PreloadedLuaBytesCache.Clear();
            m_LuaPreloadExecutionPlan.Clear();
            m_LuaPreloadExecIndex = 0;
            m_LuaPlanVisited.Clear();
            m_LuaDependencyGraph.Clear();
            m_LuaAggregatorModules.Clear();
        }

        /// <summary>
        /// 从 fileName（如 "Gamemain.lua"）反推模块名（"Gamemain"）
        /// </summary>
        private string StripLuaSuffix(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return null;
            if (fileName.EndsWith(".lua", StringComparison.OrdinalIgnoreCase))
                return fileName.Substring(0, fileName.Length - 4);
            return null;
        }

        /// <summary>
        /// 解析 Lua 源码中的**顶层（加载期）** require 依赖（仅保留项目内已预取缓存的模块）
        /// 只收集顶层作用域的 require，跳过注释、字符串及函数/块内的 require：
        /// 函数内 require 多为运行时按需加载（如 Localizations.Relate_* 按当前语言 require 语言包），
        /// 若纳入执行计划会提前执行"依赖者全局尚未就绪"的模块（访问 nil）或提前触发其顶层副作用，
        /// 故仅顶层 require 视为加载期依赖；运行时 require 由 Lua 在对应时机正常加载。
        /// </summary>
        private List<string> ParseRequires(byte[] bytes)
        {
            List<string> deps = new List<string>();
            if (bytes == null || bytes.Length == 0)
                return deps;

            string source = Encoding.UTF8.GetString(bytes);
            int len = source.Length;
            int depth = 0;              // 块/函数嵌套深度（0 = 顶层）
            bool inBlockComment = false;
            int i = 0;

            while (i < len)
            {
                char c = source[i];

                // 块注释 --[[ ... ]]
                if (inBlockComment)
                {
                    if (c == ']' && i + 1 < len && source[i + 1] == ']')
                    {
                        inBlockComment = false;
                        i += 2;
                        continue;
                    }
                    i++;
                    continue;
                }

                // 行注释 -- 或 块注释开始 --[[
                if (c == '-' && i + 1 < len && source[i + 1] == '-')
                {
                    if (i + 2 < len && source[i + 2] == '[')
                    {
                        inBlockComment = true;
                        i += 3;
                    }
                    else
                    {
                        while (i < len && source[i] != '\n')
                            i++;
                    }
                    continue;
                }

                // 字符串 '...' / "..."（忽略内部内容，避免其中的 require/关键字误判）
                if (c == '\'' || c == '"')
                {
                    char quote = c;
                    i++;
                    while (i < len)
                    {
                        if (source[i] == '\\') { i += 2; continue; }
                        if (source[i] == quote) { i++; break; }
                        i++;
                    }
                    continue;
                }

                // 顶层 require 收集（置于块关键字判断之前，避免 require 名含关键字子串）
                if (depth == 0 && IsWordAt(source, i, "require"))
                {
                    int j = i + "require".Length;
                    while (j < len && char.IsWhiteSpace(source[j]))
                        j++;

                    string dep = null;
                    if (j < len && source[j] == '(')
                    {
                        j++;
                        while (j < len && char.IsWhiteSpace(source[j]))
                            j++;
                        if (j < len && (source[j] == '\'' || source[j] == '"'))
                        {
                            char q = source[j];
                            int k = source.IndexOf(q, j + 1);
                            if (k > j)
                                dep = source.Substring(j + 1, k - j - 1);
                        }
                    }
                    else if (j < len && (char.IsLetter(source[j]) || source[j] == '_'))
                    {
                        int k = j;
                        while (k < len && (char.IsLetterOrDigit(source[k]) || source[k] == '_'))
                            k++;
                        dep = source.Substring(j, k - j);
                    }

                    if (!string.IsNullOrEmpty(dep))
                    {
                        dep = dep.Trim();
                        if (m_PreloadedLuaBytesCache.ContainsKey(dep + ".lua"))
                            deps.Add(dep);
                    }

                    i = j;
                    continue;
                }

                // 块开始关键字（+1）：function / if / do / repeat；while/for 不单独计（其后的 do 负责）
                if (IsWordAt(source, i, "function") || IsWordAt(source, i, "if") ||
                    IsWordAt(source, i, "do") || IsWordAt(source, i, "repeat"))
                {
                    depth++;
                    i++;
                    continue;
                }
                // 块结束关键字（-1）：end / until
                if (IsWordAt(source, i, "end") || IsWordAt(source, i, "until"))
                {
                    if (depth > 0)
                        depth--;
                    i++;
                    continue;
                }

                i++;
            }

            return deps;
        }

        /// <summary>
        /// 判断 source[i] 起是否为关键字 word（前后均为词边界，避免误匹配标识符中的子串，如 myfunction 中的 function）
        /// </summary>
        private static bool IsWordAt(string source, int i, string word)
        {
            if (i < 0 || i + word.Length > source.Length)
                return false;
            for (int k = 0; k < word.Length; k++)
                if (source[i + k] != word[k])
                    return false;
            if (i > 0 && (char.IsLetterOrDigit(source[i - 1]) || source[i - 1] == '_'))
                return false;
            int after = i + word.Length;
            if (after < source.Length && (char.IsLetterOrDigit(source[after]) || source[after] == '_'))
                return false;
            return true;
        }

        /// <summary>
        /// DFS 混合序访问构建执行计划：
        /// - 聚合器（顶层强制创建全局）：前序"依赖者先"，先加入计划（先创建全局），再递归依赖；
        /// - 非聚合器：后序"依赖先"，先递归依赖再加入计划（依赖命中缓存为 no-op，逐文件平滑）。
        /// </summary>
        private void VisitForPlan(string moduleName)
        {
            if (string.IsNullOrEmpty(moduleName) || m_LuaPlanVisited.Contains(moduleName))
                return;
            m_LuaPlanVisited.Add(moduleName);

            bool aggregator = m_LuaAggregatorModules.Contains(moduleName);
            if (aggregator)
                m_LuaPreloadExecutionPlan.Add(moduleName); // 聚合器：先执行以创建全局

            if (m_LuaDependencyGraph.TryGetValue(moduleName, out List<string> deps))
            {
                foreach (string dep in deps)
                    VisitForPlan(dep);
            }

            if (!aggregator)
                m_LuaPreloadExecutionPlan.Add(moduleName); // 非聚合器：依赖先，后加入
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
            string assetType = LuaAssetTypeName;
            m_AssetComponent.StrictCheck = false;

            try
            {
                // 预取缓存命中：逐文件预取阶段已缓存源码字节，免去 AB 资源加载与路径扫描
                if (m_PreloadedLuaBytesCache.TryGetValue(fileName, out byte[] cachedBytes))
                {
                    m_LoadedLuaScriptsNames.Add(fileName);
                    SetDebugFilePath(ref fileName);
                    TryHotReloadOverride(ref cachedBytes, fileName);
                    return StripUtf8Bom(cachedBytes);
                }

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

            return StripUtf8Bom(bytes);
        }

        /// <summary>
        /// 去除 Lua 源码字节开头的 UTF-8 BOM（EF BB BF）
        /// 防止带 BOM 的 .lua.txt 被 Lua 解析为非法符号（unexpected symbol near '&lt;\239&gt;'）
        /// </summary>
        /// <param name="bytes">Lua 源码字节</param>
        /// <returns>去掉 BOM 后的字节；无 BOM 时原样返回</returns>
        private static byte[] StripUtf8Bom(byte[] bytes)
        {
            if (bytes == null || bytes.Length < 3) return bytes;
            if (bytes[0] == 0xEF && bytes[1] == 0xBB && bytes[2] == 0xBF)
            {
                byte[] stripped = new byte[bytes.Length - 3];
                Array.Copy(bytes, 3, stripped, 0, bytes.Length - 3);
                return stripped;
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
                        AssetBundleLoadManager.Unload(path);
                }
            }
            else
            {
                // AB 包模式：优先命中模块→AB 路径缓存，避免每次 require 全量扫描 AB 路径列表。
                // 负缓存：该模块已确认在任何 Lua AB 路径中都不存在，直接返回，避免反复全扫（如重试场景）。
                if (m_LuaModuleNotFoundCache.Contains(fileName))
                    return null;

                // 正向缓存：首次全扫已定位到该模块所在 bundle，直接定向加载
                if (m_LuaModuleABPathCache.TryGetValue(fileName, out string cachedAbPath))
                {
                    asset = m_AssetComponent.LoadAssetSync(assetType, cachedAbPath, fileName) as TextAsset;
                    if (asset != null)
                        return asset;

                    // 缓存路径失效（如 bundle 被卸载/重建），移除缓存并回退全量扫描
                    m_LuaModuleABPathCache.Remove(fileName);
                }

                // 首次加载：全量扫描各 Lua AB 路径，命中后记录缓存，未命中记录负缓存
                foreach (string abPath in m_LuaScriptsABPathsForLoading)
                {
                    if (!CanABLoad(abPath)) continue;

                    asset = m_AssetComponent.LoadAssetSync(assetType, abPath, fileName) as TextAsset;
                    if (asset != null)
                    {
                        m_LuaModuleABPathCache[fileName] = abPath;
                        break;
                    }

                    AssetBundleLoadManager.Unload(abPath);
                }

                if (asset == null)
                    m_LuaModuleNotFoundCache.Add(fileName);
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
            m_LuaModuleABPathCache.Clear();
            m_LuaModuleNotFoundCache.Clear();
            ResetPreloadState();
        }

        #endregion

        public override void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}