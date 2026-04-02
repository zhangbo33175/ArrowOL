using System;
using System.Collections.Generic;
using GameLib;


namespace Honor.Runtime
{
    public class ProcedurePreload : ProcedureState
    {
        /// <summary>
        /// 最后一次游戏登陆模式
        /// </summary>
        private const string LAST_GAME_SERVER_MODE = "lastGameServerMode";

        /// <summary>
        /// 游戏模式类名
        /// </summary>
        private const string GAME_MODE_CLASS_NAME = "gameModeClaseName";

        /// <summary>
        /// 闪屏UI界面组件
        /// </summary>
        private UILauncherView _mUILauncherView;

        /// <summary>
        /// 预加载UI界面组件
        /// </summary>
        private UILauncherLoadingView _mUILauncherLoadingView;

        /// <summary>
        /// 预加载步骤队列
        /// </summary>
        private List<Action> m_Steps;

        /// <summary>
        /// 预加载当前步骤索引
        /// </summary>
        private int m_CurStepIndex;

        /// <summary>
        /// 开始加载
        /// </summary>
        private bool m_StartLoading;

        public bool StartLoading
        {
            get => m_StartLoading;
            set => m_StartLoading = value;
        }

        public override void OnInit(StateMachine<ProcedureComponent> ownerMachine)
        {
            base.OnInit(ownerMachine);

            m_Name = "ProcedurePreload";

            m_Steps = new List<Action>();
            m_CurStepIndex = 0;
            m_StartLoading = false;
        }

        public override void OnEnter(StateMachine<ProcedureComponent> ownerMachine)
        {
            base.OnEnter(ownerMachine);

            // Preload流程需要预加载下一个流程中的游戏内容，因此切换到下一个流程时不要移除所有内容，防止新流程内容被移除（可在Lua层控制）
            RemoveAllContentsOnProcedureTransition = false;

            // 清空步骤缓存
            m_Steps.Clear();
            m_CurStepIndex = 0;
            m_StartLoading = false;

            CheckGameMode();

            // 进入
            DoOnEnter();
        }

        public override void OnUpdate(StateMachine<ProcedureComponent> ownerMachine)
        {
            if (!m_EnterOver) return;
            if (m_StartLoading)
            {
                if (m_Steps.Count > 0)
                {
                    if (m_CurStepIndex < m_Steps.Count)
                    {
                        m_Steps[m_CurStepIndex]();
                        m_CurStepIndex++;

                        // 创建事件参数并广播进度
                        Dictionary<string, object> jd = new Dictionary<string, object>();
                        jd["progress"] = m_CurStepIndex * 1.0f / m_Steps.Count;
                        jd["descContent"] = string.Empty;
                        GameMainRoot.Event.FireNow(this, GameEventCmd.LoadProgress, jd);
                    }
                }

                if (m_LuaOnUpdate != null)
                {
                    m_LuaOnUpdate(ownerMachine);
                }
            }

            base.OnUpdate(ownerMachine);
        }

        public override void OnLeave(StateMachine<ProcedureComponent> ownerMachine, bool isShutdown)
        {
            if (IsLaunch() || IsReset)
            {
                // 初始化SDK-TGA-Lua（完成用户属性配置、静态/动态公共事件属性的配置）（注：请确保在此之前已完成登录）
               /* Root.SDK.TGAHelper.InitFromLua();*/
            }

            if (m_LuaOnLeave != null)
            {
                m_LuaOnLeave(ownerMachine);
            }

            // 销毁闪屏显示对象
            if (_mUILauncherView != null)
            {
                GameMainRoot.UI.CloseUIByGO(_mUILauncherView.gameObject, true);
                _mUILauncherView = null;
            }

            // 销毁预加载显示对象
            if (_mUILauncherLoadingView)
            {
                GameMainRoot.UI.HideLoading(_mUILauncherLoadingView);
                _mUILauncherLoadingView = null;
            }

            IsReset = false;

            base.OnLeave(ownerMachine, isShutdown);
        }

        /// <summary>
        /// 是否为启动
        /// </summary>
        /// <returns></returns>
        public bool IsLaunch()
        {
            if (m_LastProcedureType == typeof(ProcedureLaunch))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 进入时执行
        /// </summary>
        private void DoOnEnter()
        {
            // 启动进入
            void __OnLaunchEnter()
            {
                // 清理缓存
                GameMainRoot.Lua.Clear();

                // 初始化Lua配置
                GameMainRoot.Lua.InitLuaConfigs();

                // 初始化Lua环境
                GameMainRoot.Lua.InitLuaEnv();

                // 初始化Lua绑定接口
                GameMainRoot.Lua.InitLuaBindings();

                // 初始化游戏Lua绑定
                GameMainRoot.Lua.InitGameLuaBindings();

                // 开始对后续流程进行Lua绑定
                GameMainRoot.Procedure.InitLuaBindings();
            }

            // 非启动进入
            void __OnEnter()
            {
                m_StartLoading = true;

                if (GameMainRoot.Procedure.UseUIPreload)
                {
                    _mUILauncherLoadingView = GameMainRoot.UI.ShowLoading(UILauncherLoadingView.LoadingMode.Preload);
                }

                // 进入Lua层Enter处理，切换到下一流程的具体代码由Lua层调用，注：若有登录逻辑请写到ProcedurePreload的Lua层并在切换到下一流程前确保登录完毕
                if (m_LuaOnEnter != null)
                {
                    m_LuaOnEnter(m_OwnerMachine);
                }
            }

            // 启动进入时需要进行整个生命周期范围内的唯一性初始化
            if (IsLaunch())
            {
                _mUILauncherView = GameMainRoot.UI.ShowSplash(() =>
                {
                    __OnLaunchEnter();
                    __OnEnter();
                });
            }
            else if (IsReset)
            {
                GameMainRoot.UI.CloseAllUIs(UIType.Scene, true);
                GameMainRoot.UI.CloseAllUIs(UIType.Screen, true);
                GameMainRoot.Asset.ForceUnloadUnusedAssets();
                __OnLaunchEnter();
                __OnEnter();
            }
            else
            {
                __OnEnter();
            }
        }

        /// <summary>
        /// 添加预加载步骤回调
        /// </summary>
        /// <param name="callback"></param>
        public void AddStep(Action callback)
        {
            if (callback != null)
            {
                if (GameMainRoot.Procedure.UseUIPreload)
                {
                    m_Steps.Add(callback);
                }
                else
                {
                    callback();
                }
            }
        }

        /// <summary>
        /// 显示流程切换过渡进入效果
        /// </summary>
        /// <param name="forceOver">强制结束</param>
        /// <param name="duration">过渡时间</param>
        /// <param name="blockRaycast">阻塞触摸</param>
        public override void ShowProcedureTransitionEnter(bool forceOver, float duration, bool blockRaycast)
        {
            GameMainRoot.UI.ShowProcedureTransitionEnter(forceOver, duration, blockRaycast);
        }

        /// <summary>
        /// 显示流程切换过渡退出效果
        /// </summary>
        /// <param name="forceOver">强制结束</param>
        /// <param name="duration">过渡时间</param>
        /// <param name="blockRaycast">阻塞触摸</param>
        public override void ShowProcedureTransitionExit(bool forceOver, float duration, bool blockRaycast)
        {
            GameMainRoot.UI.ShowProcedureTransitionExit(forceOver, duration, blockRaycast);
        }

        /// <summary>
        /// 检查游戏模式
        /// </summary>
        private void CheckGameMode()
        {
            if (IsGameServerChanged())
            {
                GameMainRoot.Persist.RemoveAllItems(PersistWayType.FileFragment);
                GameMainRoot.Persist.RemoveAllItems(PersistWayType.PlayerPrefs);
                SetLastGameMode(); // 游戏模式改变
            }
        }

        /// <summary>
        /// 游戏服务器是否改变
        /// </summary>
        private bool IsGameServerChanged()
        {
            var lastGameMode = GetLastGameMode();
            var isDevMode = GameMainRoot.Launcher.DevelopMode;
            var nowGameMode = isDevMode ? EGameMode.EDevelopMode : EGameMode.EPublishMode;
            if (lastGameMode == EGameMode.ENone)
            {
                SetLastGameMode(); //首次运行游戏，初始化游戏模式
                return false;
            }

            return nowGameMode != lastGameMode;
        }

        /// <summary>
        /// 最近登陆游戏模式
        /// </summary>
        /// <returns></returns>
        private EGameMode GetLastGameMode()
        {
            var lastGameMode =
                GameMainRoot.Persist.GetInt(PersistWayType.FileFragment, GAME_MODE_CLASS_NAME, LAST_GAME_SERVER_MODE);
            return (EGameMode)lastGameMode;
        }

        /// <summary>
        /// 保存最后一次登陆游戏模式
        /// </summary>
        private void SetLastGameMode()
        {
            var isDevMode = GameMainRoot.Launcher.DevelopMode;
            var nowGameMode = isDevMode ? EGameMode.EDevelopMode : EGameMode.EPublishMode;
            GameMainRoot.Persist.SetInt(PersistWayType.FileFragment, GAME_MODE_CLASS_NAME, LAST_GAME_SERVER_MODE,
                (int)nowGameMode);
            GameMainRoot.Persist.Save(PersistWayType.FileFragment, GAME_MODE_CLASS_NAME);
            Log.Info($"保存游戏模式: {nowGameMode}");
        }
    }
}