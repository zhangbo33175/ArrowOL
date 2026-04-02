using System;
using XLua;

namespace Honor.Runtime
{
    public abstract class ProcedureState : State<ProcedureComponent>
    {
        /// <summary>
        /// 切换至下一个流程时需要等待最大帧数
        /// </summary>
        private const int MAX_NEXT_PROCEDURE_WAIT_FRAME_NUM = 5;

        /// <summary>
        /// 是否复位(复位Lua，会自动重新加载PreloadProcedure流程)
        /// </summary>
        public static bool IsReset;

        /// <summary>
        /// 所归属的流程状态机
        /// </summary>
        protected ProcedureStateMachine m_OwnerMachine = null;

        /// <summary>
        /// 流程名称
        /// </summary>
        protected string m_Name = null;
        public string Name { get => m_Name; }
        
        /// <summary>
        /// 对应的lua脚本名称
        /// </summary>
        protected string m_LuaScriptName;

        /// <summary>
        /// Lua脚本独立环境
        /// 为每个脚本设置一个独立的环境，可一定程度上防止脚本间全局变量、函数冲突
        /// </summary>
        protected LuaTable m_OwnEnv;

        /// <summary>
        /// 流程状态进入lua绑定回调
        /// </summary>
        protected Action<StateMachine<ProcedureComponent>> m_LuaOnEnter;

        /// <summary>
        /// 流程状态心跳更新lua绑定回调
        /// </summary>
        protected Action<StateMachine<ProcedureComponent>> m_LuaOnUpdate;

        /// <summary>
        /// 流程状态离开lua绑定回调
        /// </summary>
        protected Action<StateMachine<ProcedureComponent>> m_LuaOnLeave;

        /// <summary>
        /// 上一个流程的类型
        /// </summary>
        protected Type m_LastProcedureType;

        /// <summary>
        /// 当前流程的类型
        /// </summary>
        protected Type m_CurProcedureType;

        /// <summary>
        /// 准备切换到的下一个流程的类型
        /// 用于记录提前预缓存流程类型
        /// </summary>
        protected Type m_PrepareNextProcedureType;

        /// <summary>
        /// 下一个流程的类型
        /// </summary>
        protected Type m_NextProcedureType;

        /// <summary>
        /// 准备切换到下一个流程时传入的自定义参数
        /// </summary>
        protected LuaTable m_PrepareArgsFromChanging;
        public LuaTable PrepareArgsFromChanging
        {
            get
            {
                return m_PrepareArgsFromChanging;
            }
            set
            {
                m_PrepareArgsFromChanging = value;
            }
        }

        /// <summary>
        /// 切换过程中传入的自定义参数
        /// </summary>
        protected LuaTable m_ArgsFromChanging;
        public LuaTable ArgsFromChanging
        {
            get
            {
                return m_ArgsFromChanging;
            }
            set
            {
                m_ArgsFromChanging = value;
            }
        }

        /// <summary>
        /// 流程进入结束标记
        /// </summary>
        protected bool m_EnterOver;
        public bool EnterOver { set => m_EnterOver = value; get => m_EnterOver; }

        /// <summary>
        /// 下一个流程切换前等待帧数
        /// </summary>
        protected int m_NextProcedureWaitFrameCount;

        /// <summary>
        /// 在流程切换时是否需要移除所有内容
        /// </summary>
        protected bool m_RemoveAllContentsOnProcedureTransition;
        public bool RemoveAllContentsOnProcedureTransition
        {
            set
            {
                m_RemoveAllContentsOnProcedureTransition = value;
            }
            get
            {
                return m_RemoveAllContentsOnProcedureTransition;
            }
        }

        /// <summary>
        /// 公开暴露的cs层的lua环境
        /// 可以在lua层中通过cs.lua来访问当前挂载的Lua环境
        /// </summary>
        public LuaTable lua
        {
            get
            {
                return m_OwnEnv;
            }
        }

        /// <summary>
        /// 初始化lua绑定
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

            // 获取全局Lua环境
            LuaEnv luaEnv = luaComponent.Env;

            // 实例化Lua脚本的独立环境
            m_OwnEnv = luaEnv.NewTable();
            LuaTable meta = luaEnv.NewTable();
            meta.Set("__index", luaEnv.Global);
            m_OwnEnv.SetMetaTable(meta);
            meta.Dispose();

            // 向Lua脚本独立环境中注入所有必需对象
            m_OwnEnv.Set("lua", m_OwnEnv);
            m_OwnEnv.Set("cs", this);

            luaComponent.LuaCreateProcedureLuaClassFromCSEventDelegate(m_OwnEnv, m_LuaScriptName);

            // 绑定Lua中必需的声明周期函数到C#
            m_OwnEnv.Get("OnEnter", out m_LuaOnEnter);
            m_OwnEnv.Get("OnUpdate", out m_LuaOnUpdate);
            m_OwnEnv.Get("OnLeave", out m_LuaOnLeave);

        }

        /// <summary>
        /// 状态初始化时调用。
        /// </summary>
        /// <param name="ownerMachine">流程持有者。</param>
        public override void OnInit(StateMachine<ProcedureComponent> ownerMachine)
        {
            base.OnInit(ownerMachine);

            m_OwnerMachine = (ProcedureStateMachine)ownerMachine;

        }

        /// <summary>
        /// 状态销毁时调用。
        /// </summary>
        /// <param name="ownerMachine">流程持有者。</param>
        public override void OnDestroy(StateMachine<ProcedureComponent> ownerMachine)
        {
            base.OnDestroy(ownerMachine);

        }

        /// <summary>
        /// 进入状态时调用。
        /// </summary>
        /// <param name="ownerMachine">流程持有者。</param>
        public override void OnEnter(StateMachine<ProcedureComponent> ownerMachine)
        {
            base.OnEnter(ownerMachine);

            Log.Info("进入流程 '{0}' 。", GetType());

            if (ownerMachine.LastState != null)
            {
                m_LastProcedureType = ownerMachine.LastState.GetType();
                m_ArgsFromChanging = ((ProcedureState)ownerMachine.LastState).PrepareArgsFromChanging;
                ((ProcedureState)ownerMachine.LastState).PrepareArgsFromChanging = null;
                ((ProcedureState)ownerMachine.LastState).ArgsFromChanging = null;
            }
            m_CurProcedureType = GetType();
            m_PrepareNextProcedureType = null;
            m_PrepareArgsFromChanging = null;
            m_NextProcedureType = null;
            m_NextProcedureWaitFrameCount = 0;
            m_EnterOver = false;

            // 注册事件监听-切换过渡进入结束
            GameMainRoot.Event.Subscribe(GameEventCmd.ProcedureTransitionEnterOver, this, OnProcedureTransitionEnterOverEventCallback);

            // 注册事件监听-切换过渡退出结束
            GameMainRoot.Event.Subscribe(GameEventCmd.ProcedureTransitionExitOver, this, OnProcedureTransitionExitOverEventCallback);

            // 显示流程切换过渡进入效果
            ShowProcedureTransitionEnter(!GameMainRoot.Procedure.CurrentProcedureTransitionEnterFlag, GameMainRoot.Procedure.CurrentProcedureTransitionEnterDuration, GameMainRoot.Procedure.CurrentProcedureTransitionEnterBlockRaycast);

        }

        /// <summary>
        /// 状态轮询时调用。
        /// </summary>
        /// <param name="ownerMachine">流程持有者。</param>
        public override void OnUpdate(StateMachine<ProcedureComponent> ownerMachine)
        {
            base.OnUpdate(ownerMachine);

            // 切换至下一个流程
            if (m_NextProcedureType != null)
            {
                m_NextProcedureWaitFrameCount++;
                if (m_NextProcedureWaitFrameCount > MAX_NEXT_PROCEDURE_WAIT_FRAME_NUM)
                {
                    ChangeState(m_OwnerMachine, m_NextProcedureType);
                    // 加载字体资源集合
                    GameMainRoot.UI.LoadFonts();
                    // 刷新当前语言的字体到所有UI
                    GameMainRoot.UI.RefreshFontsForUI();
                }
            }

        }

        /// <summary>
        /// 离开状态时调用。
        /// </summary>
        /// <param name="ownerMachine">流程持有者。</param>
        /// <param name="isShutdown">是否是关闭状态机时触发。</param>
        public override void OnLeave(StateMachine<ProcedureComponent> ownerMachine, bool isShutdown)
        {
            base.OnLeave(ownerMachine, isShutdown);

            Log.Info("离开流程 '{0}'，该流程持续时间 {1:N2}秒。", GetType(), ownerMachine.CurrentStateTime);

            // 注销事件监听-切换过渡进入结束
            GameMainRoot.Event.Unsubscribe(GameEventCmd.ProcedureTransitionEnterOver, this, OnProcedureTransitionEnterOverEventCallback);

            // 注销事件监听-切换过渡退出结束
            GameMainRoot.Event.Unsubscribe(GameEventCmd.ProcedureTransitionExitOver, this, OnProcedureTransitionExitOverEventCallback);

        }

        /// <summary>
        /// 切换当前有限状态机状态。
        /// </summary>
        /// <param name="ownerMachine">有限状态机引用。</param>
        /// <param name="stateType">要切换到的有限状态机状态类型。</param>
        public override void ChangeState(StateMachine<ProcedureComponent> ownerMachine, Type stateType)
        {
            ownerMachine.Owner.RecordRuntimeProcedureInfos(ownerMachine.CurrentStateName, ownerMachine.CurrentStateTime);
            base.ChangeState(ownerMachine, stateType);
        }

        /// <summary>
        /// 切换当前有限状态机状态。
        /// </summary>
        /// <param name="ownerMachine">有限状态机引用。</param>
        /// <param name="stateName">要切换到的有限状态机状态名称。</param>
        public void ChangeState(StateMachine<ProcedureComponent> ownerMachine, string stateName)
        {
            ChangeState(ownerMachine, Assembly.GetType(AorTxt.Format("Honor.Runtime.{0}", stateName)));
        }

        /// <summary>
        /// 事件监听回调-切换过渡进入结束
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnProcedureTransitionEnterOverEventCallback(object sender, object userData, EventParams e)
        {
            if (userData != this) return;
            m_EnterOver = true;
        }
        
        /// <summary>
        /// 事件监听回调-切换过渡退出结束
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnProcedureTransitionExitOverEventCallback(object sender, object userData, EventParams e)
        {
            if (userData != this) return;
            // 移除游戏中所有内容
            RemoveAllContents();
            // 设置下一个流程
            m_NextProcedureType = m_PrepareNextProcedureType;
        }

        /// <summary>
        /// 准备切换到下一个流程
        /// </summary>
        /// <param name="stateType">要切换到的有限状态机状态类型</param>
        /// <param name="argsFromChanging">换到状态机状态时携带的自定义参数</param>
        public void PrepareToNextProcedure(Type stateType, LuaTable argsFromChanging = null)
        {
            if (stateType == null)
            {
                throw new GameException("State type 无效。");
            }

            m_PrepareNextProcedureType = stateType;
            m_PrepareArgsFromChanging = argsFromChanging;

            ShowProcedureTransitionExit(!GameMainRoot.Procedure.CurrentProcedureTransitionExitFlag, GameMainRoot.Procedure.CurrentProcedureTransitionExitDuration, GameMainRoot.Procedure.CurrentProcedureTransitionExitBlockRaycast);
        }

        /// <summary>
        /// 准备切换到下一个流程
        /// </summary>
        /// <param name="stateName">要切换到的有限状态机状态名称</param>
        /// <param name="argsFromChanging">换到状态机状态时携带的自定义参数</param>
        public void PrepareToNextProcedure(string stateName, LuaTable argsFromChanging = null)
        {
            PrepareToNextProcedure(Assembly.GetType(AorTxt.Format("Honor.Runtime.{0}", stateName)), argsFromChanging);
        }

        /// <summary>
        /// 显示流程切换过渡进入效果
        /// </summary>
        /// <param name="forceOver">强制结束</param>
        /// <param name="duration">过渡时间</param>
        /// <param name="blockRaycast">阻塞触摸</param>
        public virtual void ShowProcedureTransitionEnter(bool forceOver, float duration, bool blockRaycast)
        {
            GameMainRoot.UI.ShowProcedureTransitionEnter(forceOver, duration, blockRaycast);
        }

        /// <summary>
        /// 显示流程切换过渡退出效果
        /// </summary>
        /// <param name="forceOver">强制结束</param>
        /// <param name="duration">过渡时间</param>
        /// <param name="blockRaycast">阻塞触摸</param>
        public virtual void ShowProcedureTransitionExit(bool forceOver, float duration, bool blockRaycast)
        {
            GameMainRoot.UI.ShowProcedureTransitionExit(forceOver, duration, blockRaycast);
        }

        /// <summary>
        /// 移除游戏中所有内容
        /// </summary>
        private void RemoveAllContents()
        {
            if(m_RemoveAllContentsOnProcedureTransition)
            {
                // 关闭所有UI
                GameMainRoot.UI.CloseAllUIs(UIType.Screen, true);
                GameMainRoot.UI.CloseAllUIs(UIType.Scene, true);
                // 销毁所有场景中对象
                GameMainRoot.Scene.DestroyAllSceneGOs();
                // 卸载字体（马上，不等待Asset的过期帧数）
                GameMainRoot.UI.UnloadFonts(true);
            }
        }

    }
}
