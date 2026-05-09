using System;
using System.Collections.Generic;
using GameLib;

namespace Honor.Runtime
{
    /// <summary>
    /// 游戏预加载流程（核心中间流程）
    /// 功能：闪屏展示、Lua环境初始化、资源预加载、进度条更新、游戏模式检查、热更后重置
    /// 完成后自动跳转到游戏主流程 ProcedurePlaying
    /// </summary>
    public class ProcedurePreload : ProcedureState
    {
        /// <summary>
        /// 持久化Key：最后一次游戏运行模式（开发/发布）
        /// </summary>
        private const string LAST_GAME_SERVER_MODE = "lastGameServerMode";

        /// <summary>
        /// 持久化Key：游戏模式分类名
        /// </summary>
        private const string GAME_MODE_CLASS_NAME = "gameModeClaseName";

        /// <summary>
        /// 闪屏 UI 组件
        /// </summary>
        private UILauncherView _mUILauncherView;

        /// <summary>
        /// 预加载 Loading 界面组件
        /// </summary>
        private UILauncherLoadingView _mUILauncherLoadingView;

        /// <summary>
        /// 预加载步骤队列（按顺序执行）
        /// </summary>
        private List<Action> m_Steps;

        /// <summary>
        /// 当前执行到第几步
        /// </summary>
        private int m_CurStepIndex;

        /// <summary>
        /// 是否开始执行加载
        /// </summary>
        private bool m_StartLoading;

        /// <summary>
        /// 外部控制是否开始加载
        /// </summary>
        public bool StartLoading
        {
            get => m_StartLoading;
            set => m_StartLoading = value;
        }

        /// <summary>
        /// 流程初始化：设置名称、初始化步骤队列
        /// </summary>
        public override void OnInit(StateMachine<ProcedureComponent> ownerMachine)
        {
            base.OnInit(ownerMachine);
            m_Name = "ProcedurePreload";
            m_Steps = new List<Action>();
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

            // 检查游戏模式（开发/发布）是否变化
            CheckGameMode();

            // 执行进入逻辑
            DoOnEnter();
        }

        /// <summary>
        /// 预加载每帧更新
        /// 按步骤执行预加载 → 发布进度事件 → 执行Lua更新
        /// </summary>
        public override void OnUpdate(StateMachine<ProcedureComponent> ownerMachine)
        {
            if (!m_EnterOver) return;

            if (m_StartLoading)
            {
                // 依次执行预加载步骤
                if (m_Steps.Count > 0 && m_CurStepIndex < m_Steps.Count)
                {
                    m_Steps[m_CurStepIndex]();
                    m_CurStepIndex++;

                    // 广播加载进度（给UI进度条使用）
                    Dictionary<string, object> progressData = new Dictionary<string, object>();
                    progressData["progress"] = m_CurStepIndex * 1.0f / m_Steps.Count;
                    progressData["descContent"] = string.Empty;
                    GameMainRoot.Event.FireNow(this, GameEventCmd.LoadProgress, progressData);
                }
            }

            // 执行Lua层逻辑
            if (m_LuaOnUpdate != null)
            {
                m_LuaOnUpdate(ownerMachine);
            }

            base.OnUpdate(ownerMachine);
        }

        /// <summary>
        /// 离开预加载流程
        /// 执行Lua退出、关闭UI、重置标记、清理界面
        /// </summary>
        public override void OnLeave(StateMachine<ProcedureComponent> ownerMachine, bool isShutdown)
        {
            // 启动/重置时初始化SDK（注释备用）
            if (IsLaunch() || IsReset)
            {
            }

            // 执行Lua离开逻辑
            if (m_LuaOnLeave != null)
            {
                m_LuaOnLeave(ownerMachine);
            }

            // 关闭闪屏界面
            if (_mUILauncherView != null)
            {
                GameMainRoot.UI.CloseUIByGO(_mUILauncherView.gameObject, true);
                _mUILauncherView = null;
            }

            // 关闭加载界面
            if (_mUILauncherLoadingView)
            {
                GameMainRoot.UI.HideLoading(_mUILauncherLoadingView);
                _mUILauncherLoadingView = null;
            }

            // 重置全局复位标记
            IsReset = false;

            base.OnLeave(ownerMachine, isShutdown);
        }

        /// <summary>
        /// 是否从启动流程进入（首次启动游戏）
        /// </summary>
        public bool IsLaunch()
        {
            return m_LastProcedureType == typeof(ProcedureLaunch);
        }

        /// <summary>
        /// 执行流程进入的核心逻辑
        /// 分三种情况：首次启动、全局重置、普通进入
        /// </summary>
        private void DoOnEnter()
        {
            // 首次启动/重置：必须初始化Lua全局环境
            void __OnLaunchEnter()
            {
                GameMainRoot.Lua.Clear();
                GameMainRoot.Lua.InitLuaConfigs();
                GameMainRoot.Lua.InitLuaEnv();
                GameMainRoot.Lua.InitLuaBindings();
                GameMainRoot.Lua.InitGameLuaBindings();
                GameMainRoot.Procedure.InitLuaBindings();
            }

            // 通用进入逻辑：显示Loading、执行Lua OnEnter
            void __OnEnter()
            {
                m_StartLoading = true;

                // 显示预加载进度条
                if (GameMainRoot.Procedure.UseUIPreload)
                {
                    _mUILauncherLoadingView = GameMainRoot.UI.ShowLoading(UILauncherLoadingView.LoadingMode.Preload);
                }

                // 执行Lua层进入逻辑
                if (m_LuaOnEnter != null)
                {
                    m_LuaOnEnter(m_OwnerMachine);
                }
            }

            // 首次从Launch进入：显示闪屏 → 初始化Lua → 进入预加载
            if (IsLaunch())
            {
                _mUILauncherView = GameMainRoot.UI.ShowSplash(() =>
                {
                    __OnLaunchEnter();
                    __OnEnter();
                });
            }
            // 全局重置（热更/重启）：清空UI → 重置Lua → 重新加载
            else if (IsReset)
            {
                GameMainRoot.UI.CloseAllUIs(UIType.Scene, true);
                GameMainRoot.UI.CloseAllUIs(UIType.Screen, true);
                GameMainRoot.Asset.ForceUnloadUnusedAssets();
                __OnLaunchEnter();
                __OnEnter();
            }
            // 普通进入
            else
            {
                __OnEnter();
            }
        }

        /// <summary>
        /// 添加一个预加载步骤
        /// 开启预加载界面时加入队列，否则直接执行
        /// </summary>
        public void AddStep(Action callback)
        {
            if (callback == null) return;

            if (GameMainRoot.Procedure.UseUIPreload)
                m_Steps.Add(callback);
            else
                callback();
        }

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

        #region 游戏模式检查（开发/发布）

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
            int mode = GameMainRoot.Persist.GetInt(PersistWayType.FileFragment, GAME_MODE_CLASS_NAME,
                LAST_GAME_SERVER_MODE);
            return (EGameMode)mode;
        }

        /// <summary>
        /// 保存当前游戏模式到本地
        /// </summary>
        private void SetLastGameMode()
        {
            bool isDev = GameMainRoot.Launcher.DevelopMode;
            EGameMode currentMode = isDev ? EGameMode.EDevelopMode : EGameMode.EPublishMode;

            GameMainRoot.Persist.SetInt(PersistWayType.FileFragment, GAME_MODE_CLASS_NAME, LAST_GAME_SERVER_MODE,
                (int)currentMode);
            GameMainRoot.Persist.Save(PersistWayType.FileFragment, GAME_MODE_CLASS_NAME);
            Log.Info($"保存游戏模式: {currentMode}");
        }

        #endregion
    }
}