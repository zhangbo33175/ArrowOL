/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * -------------------------------------------------------------
 * filename:  ProcedurePreload.cs
 * author:    云毅
 * created:   2026
 * descrip:   游戏预加载流程 - Loading 展示、Lua 初始化（拆分至加载进度）、资源预加载、进度管理、模式检查
 *            设计：进入即显示 Loading，将同步 Lua 启动拆分为多个带描述的加载步骤随进度条展示，避免启动黑屏/卡顿
 ***************************************************************/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using GameLib;
using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// 游戏预加载流程（核心中间流程）
    /// 功能：Loading 展示、Lua 环境初始化（拆分至加载步骤并展示进度）、资源预加载、进度条更新、游戏模式检查、热更后重置
    /// 设计：
    /// 1. 进入流程即显示 Loading，将同步 Lua 启动（清理环境/加载配置/require StartGame/绑定回调）拆分为多个步骤，
    ///    随进度条逐步展示，避免主线程阻塞时黑屏无反馈；
    /// 2. Lua OnEnter 作为环境就绪后的后续步骤执行，追加真实预加载步骤；
    /// 3. 每步骤执行带 Stopwatch 耗时打点，便于定位启动瓶颈。
    /// 完成后自动跳转到游戏主流程 ProcedurePlaying
    /// </summary>
    public class ProcedurePreload : ProcedureState
    {
        #region 常量
        //=========================================================================
        // 常量
        //=========================================================================

        /// <summary>
        /// 持久化Key：最后一次游戏运行模式（开发/发布）
        /// </summary>
        private const string LAST_GAME_SERVER_MODE = "lastGameServerMode";

        /// <summary>
        /// 持久化Key：游戏模式分类名
        /// </summary>
        private const string GAME_MODE_CLASS_NAME = "gameModeClaseName";

        /// <summary>
        /// 等待闪屏过渡完成（EnterOver）的超时缓冲（秒）
        /// 在过渡时长基础上额外缓冲，避免过渡异常导致 Loading 永久不启动
        /// </summary>
        private const float ENTER_OVER_WAIT_BUFFER = 3f;

        #endregion

        #region 私有变量
        //=========================================================================
        // 私有变量
        //=========================================================================

        /// <summary>
        /// 预加载 Loading 界面组件
        /// </summary>
        private UILauncherLoadingView m_UILauncherLoadingView;

        /// <summary>
        /// 预加载步骤队列（按顺序执行，每步含回调与进度描述）
        /// </summary>
        private List<PreloadStep> m_Steps;

        /// <summary>
        /// 当前执行到第几步
        /// </summary>
        private int m_CurStepIndex;

        /// <summary>
        /// 是否开始执行加载
        /// </summary>
        private bool m_StartLoading;

        /// <summary>
        /// Lua 层 OnEnter 是否已执行
        /// 用于门控 Lua OnUpdate：确保 Lua OnEnter（含 HonorTimerHelper 等状态初始化）先于 Lua OnUpdate 执行，
        /// 避免启动流程中“OnUpdate 先于 OnEnter 触发”崩溃
        /// </summary>
        private bool m_LuaOnEnterDone;

        /// <summary>
        /// 等待 Logo 过渡完成（EnterOver）的起始时间（用于超时兜底与提前拉起进度判断）
        /// </summary>
        private float m_EnterOverWaitStartTime;

        /// <summary>
        /// 等待 Logo 过渡完成（EnterOver）的最大时长（过渡时长 + 缓冲）
        /// </summary>
        private float m_EnterOverMaxWait;

        /// <summary>
        /// Logo 进入过渡时长（用于按播放比例提前拉起 Loading）
        /// </summary>
        private float m_LogoEnterDuration;

        #endregion

        #region 步骤模型
        //=========================================================================
        // 步骤模型
        //=========================================================================

        /// <summary>
        /// 预加载步骤（回调 + 进度描述）
        /// 用于将启动/加载工作拆分为进度条可展示的步骤
        /// </summary>
        private sealed class PreloadStep
        {
            /// <summary>
            /// 步骤回调（返回 false 表示本步骤未完成，下一帧重试；用于分帧加载）
            /// </summary>
            public readonly Func<bool> Callback;

            /// <summary>
            /// 进度描述文案（展示在 Loading 界面，可为空）
            /// </summary>
            public readonly string Description;

            /// <summary>
            /// 构造步骤
            /// </summary>
            /// <param name="callback">步骤回调（返回是否完成）</param>
            /// <param name="description">进度描述文案（可为空）</param>
            public PreloadStep(Func<bool> callback, string description)
            {
                Callback = callback;
                Description = description;
            }

            /// <summary>
            /// 构造步骤（一次性 Action 包装为已完成）
            /// </summary>
            /// <param name="callback">步骤回调</param>
            /// <param name="description">进度描述文案（可为空）</param>
            public PreloadStep(Action callback, string description) : this(() => { callback?.Invoke(); return true; }, description)
            {
            }
        }

        #endregion

        #region 公共属性
        //=========================================================================
        // 公共属性
        //=========================================================================

        /// <summary>
        /// 外部控制是否开始加载
        /// </summary>
        public bool StartLoading
        {
            get => m_StartLoading;
            set => m_StartLoading = value;
        }

        /// <summary>
        /// 当前过渡闪屏的淡出起始时刻（读取实例化配置，支持 Inspector 调整）
        /// 过渡 UI 尚未加载时回退到默认 2.0s（黑屏0 + 淡入0.5 + 停留1.5），避免空引用
        /// </summary>
        private float CurrentSplashFadeOutStart => GameMainRoot.UI.ProcedureTransitionUI?.SplashFadeOutStart ?? 2.0f;

        #endregion

        #region 生命周期
        //=========================================================================
        // 生命周期
        //=========================================================================

        /// <summary>
        /// 流程初始化：设置名称、初始化步骤队列
        /// </summary>
        public override void OnInit(StateMachine<ProcedureComponent> ownerMachine)
        {
            base.OnInit(ownerMachine);
            m_Name = "ProcedurePreload";
            m_Steps = new List<PreloadStep>();
            m_CurStepIndex = 0;
            m_StartLoading = false;
        }

        /// <summary>
        /// 进入预加载流程
        /// 不清理资源（因为要预加载下一流程内容）、重置状态、检查游戏模式、执行初始化
        /// </summary>
        public override void OnEnter(StateMachine<ProcedureComponent> ownerMachine)
        {
            base.OnEnter(ownerMachine);

            // Preload 用于预加载内容，切换流程时不清空资源
            RemoveAllContentsOnProcedureTransition = false;

            // 重置加载状态
            m_Steps.Clear();
            m_CurStepIndex = 0;
            m_StartLoading = false;
            m_LuaOnEnterDone = false;

            // 检查游戏模式（开发/发布）是否变化
            CheckGameMode();

            // 执行进入逻辑
            DoOnEnter();
        }

        /// <summary>
        /// 预加载每帧更新
        /// 等待 Logo 过渡完成（EnterOver）后显示 Loading → 按步骤执行预加载 → 发布进度事件（含步骤描述）→ 执行Lua更新
        /// </summary>
        public override void OnUpdate(StateMachine<ProcedureComponent> ownerMachine)
        {
            // 等待闪屏播放到淡出起始点或完成（EnterOver 事件，m_EnterOver 置 true）后再开始 Loading 进度
            // 使流程更丝滑：闪屏停留约 3s 后（淡出起始点）提前淡入 Loading，闪屏完全隐藏后 Loading 已就绪；
            // 带超时兜底避免过渡异常导致永久卡住
            if (!m_StartLoading)
            {
                float elapsed = Time.time - m_EnterOverWaitStartTime;

                // 提前拉起：闪屏播放到淡出起始点（停留约 3s 后）即提前显示 Loading，与闪屏淡出衔接
                bool splashFading = elapsed >= CurrentSplashFadeOutStart;
                // 闪屏已隐藏（EnterOver 事件）或超时兜底
                bool logoOver = m_EnterOver || splashFading || elapsed >= m_EnterOverMaxWait;

                if (logoOver)
                {
                    if (GameMainRoot.Procedure.UseUIPreload)
                    {
                        m_UILauncherLoadingView = GameMainRoot.UI.ShowLoading(UILauncherLoadingView.LoadingMode.Preload);
                    }
                    m_StartLoading = true;
                }
            }

            if (m_StartLoading)
            {
                // 依次执行预加载步骤（每帧执行一个步骤；未完成的分帧步骤下一帧重试）
                if (m_Steps.Count > 0 && m_CurStepIndex < m_Steps.Count)
                {
                    PreloadStep step = m_Steps[m_CurStepIndex];

                    // 步骤描述（为空时兜底为通用文案）
                    string stepDesc = step.Description ?? "预加载步骤";

                    // 耗时打点：仅步骤完成时输出，量化各启动/加载环节耗时（分帧步骤的逐模块耗时见 LuaComponent 打点）
                    Stopwatch sw = Stopwatch.StartNew();
                    bool completed = step.Callback();
                    sw.Stop();
                    if (completed)
                    {
                        m_CurStepIndex++;
                        Log.Info($"[启动耗时] {stepDesc} = {sw.ElapsedMilliseconds}ms");
                    }

                    // 广播加载进度（给UI进度条使用，附带当前步骤描述；分帧步骤未完成时进度保持当前值，下一帧继续）
                    Dictionary<string, object> progressData = new Dictionary<string, object>
                    {
                        ["progress"] = m_CurStepIndex * 1.0f / m_Steps.Count,
                        ["descContent"] = stepDesc
                    };
                    GameMainRoot.Event.FireNow(this, GameEventCmd.LoadProgress, progressData);
                }
            }

            // 过渡进入未完成：不执行 Lua 更新与基类流程更新，避免在过渡期间提前推进
            if (!m_EnterOver) return;

            // 执行Lua层逻辑（门控：仅在 Lua OnEnter 执行之后调用，确保 Lua 侧状态已初始化）
            if (m_LuaOnEnterDone)
                m_LuaOnUpdate?.Invoke(ownerMachine);

            base.OnUpdate(ownerMachine);
        }

        /// <summary>
        /// 离开预加载流程
        /// 执行Lua退出、关闭UI、重置标记
        /// </summary>
        public override void OnLeave(StateMachine<ProcedureComponent> ownerMachine, bool isShutdown)
        {
            // 执行Lua离开逻辑
            m_LuaOnLeave?.Invoke(ownerMachine);

            // 关闭加载界面
            if (m_UILauncherLoadingView)
            {
                GameMainRoot.UI.HideLoading(m_UILauncherLoadingView);
                m_UILauncherLoadingView = null;
            }

            // 重置全局复位标记
            IsReset = false;

            base.OnLeave(ownerMachine, isShutdown);
        }

        #endregion

        #region 核心逻辑
        //=========================================================================
        // 核心逻辑
        //=========================================================================

        /// <summary>
        /// 是否从启动流程进入（首次启动游戏）
        /// </summary>
        public bool IsLaunch()
        {
            return m_LastProcedureType == typeof(ProcedureLaunch);
        }

        /// <summary>
        /// 执行流程进入的核心逻辑
        /// 排队启动加载步骤，等待 Logo 过渡完成后再开始展示 Loading 进度
        /// </summary>
        private void DoOnEnter()
        {
            // 全局重置（热更/重启）：先清空UI与未使用资源
            if (IsReset)
            {
                GameMainRoot.UI.CloseAllUIs(UIType.Scene, true);
                GameMainRoot.UI.CloseAllUIs(UIType.Screen, true);
                GameMainRoot.Asset.ForceUnloadUnusedAssets();
            }

            // 通用进入逻辑：排队启动步骤；Loading 进度条等 Logo 过渡（EnterOver）完成后启动
            void __OnEnter()
            {
                // 首次启动/重置：Lua 环境需重新初始化，拆分为加载步骤展示进度
                if (IsLaunch() || IsReset)
                {
                    EnqueueLuaInitSteps();
                }
                // 普通进入：Lua 环境已存在，仅执行 Lua OnEnter 追加真实预加载步骤
                else
                {
                    EnqueueLuaEnterStep();
                }

                // 记录等待闪屏过渡完成（EnterOver）的超时
                // 过渡完成（OnUpdate 中 m_EnterOver 置 true）或闪屏淡出起始点（SplashFadeOutStart）时开始显示 Loading 并执行步骤
                m_EnterOverWaitStartTime = Time.time;
                m_LogoEnterDuration = GameMainRoot.Procedure.CurrentProcedureTransitionEnterDuration;
                // 超时兜底：至少覆盖闪屏固定序列，避免闪屏未结束前超时提前触发 Loading
                m_EnterOverMaxWait = Mathf.Max(m_LogoEnterDuration, CurrentSplashFadeOutStart) + ENTER_OVER_WAIT_BUFFER;
            }

            __OnEnter();
        }

        /// <summary>
        /// 添加一个预加载步骤（无进度描述，一次性执行）
        /// 兼容 Lua 侧 self.cs:AddStep(function() ... end) 与生成的 XLua wrap
        /// </summary>
        /// <param name="callback">步骤回调</param>
        public void AddStep(Action callback)
        {
            if (callback == null) return;
            AddStep(null, callback);
        }

        /// <summary>
        /// 添加一个预加载步骤（含进度描述，一次性执行）
        /// </summary>
        /// <param name="description">进度描述文案（展示在Loading界面，可为空）</param>
        /// <param name="callback">步骤回调</param>
        public void AddStep(string description, Action callback)
        {
            if (callback == null) return;
            AddStep(description, () => { callback(); return true; });
        }

        /// <summary>
        /// 添加一个预加载步骤（含进度描述，支持分帧）
        /// 回调返回 false 时本步骤保持当前进度，下一帧重试（用于分帧加载，避免单帧阻塞）
        /// </summary>
        /// <param name="description">进度描述文案（展示在Loading界面，可为空）</param>
        /// <param name="callback">步骤回调（返回是否完成）</param>
        public void AddStep(string description, Func<bool> callback)
        {
            if (callback == null) return;

            if (GameMainRoot.Procedure.UseUIPreload)
            {
                m_Steps.Add(new PreloadStep(callback, description));
            }
            else
            {
                // 非UI模式：无进度条可展示，同步执行直到本步骤完成（分帧步骤在此一次性跑完）
                int guard = 0;
                while (!callback() && guard++ < 10000) { }
            }
        }

        /// <summary>
        /// 排队 Lua 启动步骤（将同步 Lua 初始化拆分到加载进度中展示）
        /// 顺序：配置/多语言 → 清理环境 → 配置 → 环境 → 分帧加载入口(顶层模块→StartGame主体) → 绑定回调 → Lua OnEnter
        /// </summary>
        private void EnqueueLuaInitSteps()
        {
            // 前置：配置表与多语言（原在 ProcedureLaunch 同步执行造成启动黑屏，移入此处分帧后台加载并展示进度）
            EnqueueLaunchInitSteps();

            AddStep("清理Lua环境", () => GameMainRoot.Lua.Clear());
            AddStep("初始化Lua配置", () => GameMainRoot.Lua.InitLuaConfigs());
            AddStep("初始化Lua环境", () => GameMainRoot.Lua.InitLuaEnv());
            // 分帧加载 Lua 入口：每帧 require 一个顶层模块，直到 StartGame 主体（回调定义）完成
            AddStep("加载Lua入口脚本", () => GameMainRoot.Lua.RequireNextStartGameModule());
            AddStep("绑定框架回调", () => GameMainRoot.Lua.InitLuaBindings());
            AddStep("绑定游戏回调", () => GameMainRoot.Lua.InitGameLuaBindings());
            AddStep("绑定流程回调", () => GameMainRoot.Procedure.InitLuaBindings());
            EnqueueLuaEnterStep();
        }

        /// <summary>
        /// 排队启动前置加载步骤（配置表/多语言等重活，含耗时打点）
        /// 原在 ProcedureLaunch 中同步执行导致启动黑屏，现移入此处随 Loading 分帧加载；
        /// 必须位于 Lua 初始化步骤之前（Lua 游戏代码依赖配置表与语言数据）
        /// </summary>
        private void EnqueueLaunchInitSteps()
        {
            AddStep("加载全局配置表", () =>
            {
                Stopwatch sw = Stopwatch.StartNew();
                GameMainRoot.Config.LoadConfigs();
                Log.Info($"[启动耗时] 加载全局配置表 = {sw.ElapsedMilliseconds}ms");
            });
            AddStep("加载语言列表", () =>
            {
                Stopwatch sw = Stopwatch.StartNew();
                GameMainRoot.Localization.LoadDefaultLanguages();
                Log.Info($"[启动耗时] 加载语言列表 = {sw.ElapsedMilliseconds}ms");
            });
            AddStep("初始化当前语言", () =>
            {
                Stopwatch sw = Stopwatch.StartNew();
                GameMainRoot.Localization.InitCurLanguage();
                Log.Info($"[启动耗时] 初始化当前语言 = {sw.ElapsedMilliseconds}ms");
            });
            AddStep("加载默认语言数据", () =>
            {
                Stopwatch sw = Stopwatch.StartNew();
                GameMainRoot.Localization.LoadDefaultDatas();
                Log.Info($"[启动耗时] 加载默认语言数据 = {sw.ElapsedMilliseconds}ms");
            });
            AddStep("加载字体配置", () =>
            {
                Stopwatch sw = Stopwatch.StartNew();
                GameMainRoot.Localization.LoadFontDatas();
                Log.Info($"[启动耗时] 加载字体配置 = {sw.ElapsedMilliseconds}ms");
            });
            AddStep("设置语言", () =>
            {
                Stopwatch sw = Stopwatch.StartNew();
                GameMainRoot.Localization.SetLanguage(GameMainRoot.Localization.Language, true);
                Log.Info($"[启动耗时] 设置语言 = {sw.ElapsedMilliseconds}ms");
            });
        }

        /// <summary>
        /// 排队 Lua OnEnter 步骤（Lua 环境就绪后执行，追加真实预加载步骤）
        /// </summary>
        private void EnqueueLuaEnterStep()
        {
            AddStep("初始化游戏流程", () =>
            {
                m_LuaOnEnter?.Invoke(m_OwnerMachine);
                m_LuaOnEnterDone = true;
            });
        }

        #endregion

        #region 过渡动画重写
        //=========================================================================
        // 过渡动画重写
        //=========================================================================

        /// <summary>
        /// 重写：显示流程进入过渡（直接调用UI层）
        /// </summary>
        public override void ShowProcedureTransitionEnter(bool forceOver, float duration, bool blockRaycast)
        {
            GameMainRoot.UI.ShowProcedureTransitionEnter(forceOver, duration, blockRaycast);
        }

        /// <summary>
        /// 重写：显示流程退出过渡（直接调用UI层）
        /// </summary>
        public override void ShowProcedureTransitionExit(bool forceOver, float duration, bool blockRaycast)
        {
            GameMainRoot.UI.ShowProcedureTransitionExit(forceOver, duration, blockRaycast);
        }

        #endregion

        #region 游戏模式检查（开发/发布）
        //=========================================================================
        // 游戏模式检查（开发/发布）
        //=========================================================================

        /// <summary>
        /// 检查游戏模式是否变更，变更则清空所有存档
        /// </summary>
        private void CheckGameMode()
        {
            if (IsGameServerChanged())
            {
                GameMainRoot.Persist.RemoveAllItems(PersistWayType.FileFragment);
                GameMainRoot.Persist.RemoveAllItems(PersistWayType.PlayerPrefs);
                SetLastGameMode();
            }
        }

        /// <summary>
        /// 判断游戏模式（开发/发布）是否发生变化
        /// </summary>
        private bool IsGameServerChanged()
        {
            EGameMode lastMode = GetLastGameMode();
            bool isDev = GameMainRoot.Launcher.DevelopMode;
            EGameMode currentMode = isDev ? EGameMode.EDevelopMode : EGameMode.EPublishMode;

            if (lastMode == EGameMode.ENone)
            {
                SetLastGameMode();
                return false;
            }

            return currentMode != lastMode;
        }

        /// <summary>
        /// 获取上一次保存的游戏模式
        /// </summary>
        private EGameMode GetLastGameMode()
        {
            int mode = GameMainRoot.Persist.GetInt(PersistWayType.FileFragment, GAME_MODE_CLASS_NAME, LAST_GAME_SERVER_MODE);
            return (EGameMode)mode;
        }

        /// <summary>
        /// 保存当前游戏模式到本地
        /// </summary>
        private void SetLastGameMode()
        {
            bool isDev = GameMainRoot.Launcher.DevelopMode;
            EGameMode currentMode = isDev ? EGameMode.EDevelopMode : EGameMode.EPublishMode;

            GameMainRoot.Persist.SetInt(PersistWayType.FileFragment, GAME_MODE_CLASS_NAME, LAST_GAME_SERVER_MODE, (int)currentMode);
            GameMainRoot.Persist.Save(PersistWayType.FileFragment, GAME_MODE_CLASS_NAME);
            Log.Info($"保存游戏模式: {currentMode}");
        }

        #endregion
    }
}
