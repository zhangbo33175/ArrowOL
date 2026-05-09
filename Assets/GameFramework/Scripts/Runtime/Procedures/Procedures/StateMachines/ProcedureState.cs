using System;
using XLua;

namespace Honor.Runtime
{
    /// <summary>
    /// 流程状态抽象基类
    /// 所有游戏流程（启动/热更/预加载/游戏中）均继承此类
    /// 提供：Lua绑定、状态切换、过渡动画、事件监听、生命周期管理
    /// </summary>
    public abstract class ProcedureState : State<ProcedureComponent>
    {
        /// <summary>
        /// 切换流程时最大等待帧数（防止帧等待死循环）
        /// </summary>
        private const int MAX_NEXT_PROCEDURE_WAIT_FRAME_NUM = 5;

        /// <summary>
        /// 全局复位标记（置true会自动重启到Preload流程）
        /// </summary>
        public static bool IsReset;

        /// <summary>
        /// 所属状态机
        /// </summary>
        protected ProcedureStateMachine m_OwnerMachine = null;

        /// <summary>
        /// 流程名称
        /// </summary>
        protected string m_Name = null;
        public string Name { get => m_Name; }

        /// <summary>
        /// 绑定的Lua脚本名
        /// </summary>
        protected string m_LuaScriptName;

        /// <summary>
        /// 独立Lua环境（每个流程一个环境，防止变量污染）
        /// </summary>
        protected LuaTable m_OwnEnv;

        /// <summary>
        /// Lua绑定：流程进入回调
        /// </summary>
        protected Action<StateMachine<ProcedureComponent>> m_LuaOnEnter;

        /// <summary>
        /// Lua绑定：流程更新回调
        /// </summary>
        protected Action<StateMachine<ProcedureComponent>> m_LuaOnUpdate;

        /// <summary>
        /// Lua绑定：流程离开回调
        /// </summary>
        protected Action<StateMachine<ProcedureComponent>> m_LuaOnLeave;

        /// <summary>
        /// 上一个流程类型
        /// </summary>
        protected Type m_LastProcedureType;

        /// <summary>
        /// 当前流程类型
        /// </summary>
        protected Type m_CurProcedureType;

        /// <summary>
        /// 预准备切换的下一个流程类型（提前缓存）
        /// </summary>
        protected Type m_PrepareNextProcedureType;

        /// <summary>
        /// 真正要切换的下一个流程类型
        /// </summary>
        protected Type m_NextProcedureType;

        /// <summary>
        /// 准备切换时携带的自定义参数（LuaTable）
        /// </summary>
        protected LuaTable m_PrepareArgsFromChanging;
        public LuaTable PrepareArgsFromChanging
        {
            get => m_PrepareArgsFromChanging;
            set => m_PrepareArgsFromChanging = value;
        }

        /// <summary>
        /// 切换完成后传入的自定义参数
        /// </summary>
        protected LuaTable m_ArgsFromChanging;
        public LuaTable ArgsFromChanging
        {
            get => m_ArgsFromChanging;
            set => m_ArgsFromChanging = value;
        }

        /// <summary>
        /// 流程进入完成标记（过渡动画结束）
        /// </summary>
        protected bool m_EnterOver;
        public bool EnterOver { set => m_EnterOver = value; get => m_EnterOver; }

        /// <summary>
        /// 切换流程前等待的帧数
        /// </summary>
        protected int m_NextProcedureWaitFrameCount;

        /// <summary>
        /// 流程切换时是否清空所有UI/场景资源
        /// </summary>
        protected bool m_RemoveAllContentsOnProcedureTransition;
        public bool RemoveAllContentsOnProcedureTransition
        {
            set => m_RemoveAllContentsOnProcedureTransition = value;
            get => m_RemoveAllContentsOnProcedureTransition;
        }

        /// <summary>
        /// 公开给Lua访问的CS环境（lua.cs = this）
        /// </summary>
        public LuaTable lua
        {
            get => m_OwnEnv;
        }

        /// <summary>
        /// 初始化Lua绑定：创建独立环境、加载脚本、绑定生命周期
        /// </summary>
        public void InitLuaBindings(string luaScriptName)
        {
            LuaComponent luaComponent = GameComponentsGroup.GetComponent<LuaComponent>();
            if (luaComponent == null)
            {
                Log.Fatal("Lua Component 无效。");
                return;
            }

            m_LuaScriptName = luaScriptName;
            LuaEnv luaEnv = luaComponent.Env;

            // 创建独立环境并绑定元表
            m_OwnEnv = luaEnv.NewTable();
            LuaTable meta = luaEnv.NewTable();
            meta.Set("__index", luaEnv.Global);
            m_OwnEnv.SetMetaTable(meta);
            meta.Dispose();

            // 向Lua注入自身与环境
            m_OwnEnv.Set("lua", m_OwnEnv);
            m_OwnEnv.Set("cs", this);

            // 加载Lua流程类
            luaComponent.LuaCreateProcedureLuaClassFromCSEventDelegate(m_OwnEnv, m_LuaScriptName);

            // 绑定生命周期
            m_OwnEnv.Get("OnEnter", out m_LuaOnEnter);
            m_OwnEnv.Get("OnUpdate", out m_LuaOnUpdate);
            m_OwnEnv.Get("OnLeave", out m_LuaOnLeave);
        }

        /// <summary>
        /// 状态初始化（只执行一次）
        /// </summary>
        public override void OnInit(StateMachine<ProcedureComponent> ownerMachine)
        {
            base.OnInit(ownerMachine);
            m_OwnerMachine = (ProcedureStateMachine)ownerMachine;
        }

        /// <summary>
        /// 状态销毁
        /// </summary>
        public override void OnDestroy(StateMachine<ProcedureComponent> ownerMachine)
        {
            base.OnDestroy(ownerMachine);
        }

        /// <summary>
        /// 进入流程：记录上一流程、绑定事件、播放进入过渡
        /// </summary>
        public override void OnEnter(StateMachine<ProcedureComponent> ownerMachine)
        {
            base.OnEnter(ownerMachine);
            Log.Info("进入流程 '{0}' 。", GetType());

            // 记录上一流程与参数
            if (ownerMachine.LastState != null)
            {
                m_LastProcedureType = ownerMachine.LastState.GetType();
                m_ArgsFromChanging = ((ProcedureState)ownerMachine.LastState).PrepareArgsFromChanging;
                ((ProcedureState)ownerMachine.LastState).PrepareArgsFromChanging = null;
                ((ProcedureState)ownerMachine.LastState).ArgsFromChanging = null;
            }

            // 重置状态
            m_CurProcedureType = GetType();
            m_PrepareNextProcedureType = null;
            m_PrepareArgsFromChanging = null;
            m_NextProcedureType = null;
            m_NextProcedureWaitFrameCount = 0;
            m_EnterOver = false;

            // 注册过渡动画事件
            GameMainRoot.Event.Subscribe(GameEventCmd.ProcedureTransitionEnterOver, this, OnProcedureTransitionEnterOverEventCallback);
            GameMainRoot.Event.Subscribe(GameEventCmd.ProcedureTransitionExitOver, this, OnProcedureTransitionExitOverEventCallback);

            // 播放流程进入过渡
            ShowProcedureTransitionEnter(!GameMainRoot.Procedure.CurrentProcedureTransitionEnterFlag, 
                GameMainRoot.Procedure.CurrentProcedureTransitionEnterDuration, 
                GameMainRoot.Procedure.CurrentProcedureTransitionEnterBlockRaycast);
        }

        /// <summary>
        /// 流程更新：执行等待帧逻辑，满足条件则切换流程
        /// </summary>
        public override void OnUpdate(StateMachine<ProcedureComponent> ownerMachine)
        {
            base.OnUpdate(ownerMachine);

            // 等待指定帧数后切换流程
            if (m_NextProcedureType != null)
            {
                m_NextProcedureWaitFrameCount++;
                if (m_NextProcedureWaitFrameCount > MAX_NEXT_PROCEDURE_WAIT_FRAME_NUM)
                {
                    ChangeState(m_OwnerMachine, m_NextProcedureType);
                    GameMainRoot.UI.LoadFonts();
                    GameMainRoot.UI.RefreshFontsForUI();
                }
            }
        }

        /// <summary>
        /// 离开流程：注销事件、输出日志
        /// </summary>
        public override void OnLeave(StateMachine<ProcedureComponent> ownerMachine, bool isShutdown)
        {
            base.OnLeave(ownerMachine, isShutdown);
            Log.Info("离开流程 '{0}'，该流程持续时间 {1:N2}秒。", GetType(), ownerMachine.CurrentStateTime);

            GameMainRoot.Event.Unsubscribe(GameEventCmd.ProcedureTransitionEnterOver, this, OnProcedureTransitionEnterOverEventCallback);
            GameMainRoot.Event.Unsubscribe(GameEventCmd.ProcedureTransitionExitOver, this, OnProcedureTransitionExitOverEventCallback);
        }

        /// <summary>
        /// 切换流程：记录运行时信息
        /// </summary>
        public override void ChangeState(StateMachine<ProcedureComponent> ownerMachine, Type stateType)
        {
            ownerMachine.Owner.RecordRuntimeProcedureInfos(ownerMachine.CurrentStateName, ownerMachine.CurrentStateTime);
            base.ChangeState(ownerMachine, stateType);
        }

        /// <summary>
        /// 根据名称切换流程（简化调用）
        /// </summary>
        public void ChangeState(StateMachine<ProcedureComponent> ownerMachine, string stateName)
        {
            ChangeState(ownerMachine, Type.GetType($"Honor.Runtime.{stateName}"));
        }

        /// <summary>
        /// 过渡进入结束事件回调
        /// </summary>
        public void OnProcedureTransitionEnterOverEventCallback(object sender, object userData, EventParams e)
        {
            if (userData != this) return;
            m_EnterOver = true;
        }

        /// <summary>
        /// 过渡退出结束事件回调：清空资源 → 准备切换
        /// </summary>
        public void OnProcedureTransitionExitOverEventCallback(object sender, object userData, EventParams e)
        {
            if (userData != this) return;
            RemoveAllContents();
            m_NextProcedureType = m_PrepareNextProcedureType;
        }

        /// <summary>
        /// 准备切换到下一流程：播放退出过渡
        /// </summary>
        public void PrepareToNextProcedure(Type stateType, LuaTable argsFromChanging = null)
        {
            if (stateType == null)
                throw new GameException("State type 无效。");

            m_PrepareNextProcedureType = stateType;
            m_PrepareArgsFromChanging = argsFromChanging;

            ShowProcedureTransitionExit(!GameMainRoot.Procedure.CurrentProcedureTransitionExitFlag,
                GameMainRoot.Procedure.CurrentProcedureTransitionExitDuration,
                GameMainRoot.Procedure.CurrentProcedureTransitionExitBlockRaycast);
        }

        /// <summary>
        /// 根据名称准备切换流程
        /// </summary>
        public void PrepareToNextProcedure(string stateName, LuaTable argsFromChanging = null)
        {
            PrepareToNextProcedure(Type.GetType($"Honor.Runtime.{stateName}"), argsFromChanging);
        }

        /// <summary>
        /// 显示流程进入过渡动画
        /// </summary>
        public virtual void ShowProcedureTransitionEnter(bool forceOver, float duration, bool blockRaycast)
        {
            GameMainRoot.UI.ShowProcedureTransitionEnter(forceOver, duration, blockRaycast);
        }

        /// <summary>
        /// 显示流程退出过渡动画
        /// </summary>
        public virtual void ShowProcedureTransitionExit(bool forceOver, float duration, bool blockRaycast)
        {
            GameMainRoot.UI.ShowProcedureTransitionExit(forceOver, duration, blockRaycast);
        }

        /// <summary>
        /// 清空所有场景/UI/资源（流程切换时）
        /// </summary>
        private void RemoveAllContents()
        {
            if (m_RemoveAllContentsOnProcedureTransition)
            {
                GameMainRoot.UI.CloseAllUIs(UIType.Screen, true);
                GameMainRoot.UI.CloseAllUIs(UIType.Scene, true);
                GameMainRoot.Scene.DestroyAllSceneGOs();
                GameMainRoot.UI.UnloadFonts(true);
            }
        }
    }
}