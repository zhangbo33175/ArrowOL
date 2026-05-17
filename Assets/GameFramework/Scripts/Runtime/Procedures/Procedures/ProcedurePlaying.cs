/***************************************************************
 * (c) copyright 2026 - 2025, Honor.Runtime
 * -------------------------------------------------------------
 * filename:  ProcedurePlaying.cs
 * author: 云毅
 * created:   2026
 * descrip:   游戏主运行流程 - 游戏核心逻辑流程，Lua热更为主，C#仅做生命周期转发
 ***************************************************************/

using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// 游戏主运行流程
    /// 游戏真正的核心逻辑流程（主界面、战斗、玩法等均在此流程内由Lua驱动）
    /// 完全基于XLua热更逻辑运行，C#层仅做生命周期转发
    /// </summary>
    public class ProcedurePlaying : ProcedureState
    {
        //=========================================================================
        #region 生命周期
        //=========================================================================

        /// <summary>
        /// 初始化流程：设置流程名称
        /// </summary>
        public override void OnInit(StateMachine<ProcedureComponent> ownerMachine)
        {
            base.OnInit(ownerMachine);
            m_Name = "ProcedurePlaying";
        }

        /// <summary>
        /// 进入游戏主流程
        /// 启用资源清空标记，并调用Lua层的OnEnter逻辑
        /// </summary>
        public override void OnEnter(StateMachine<ProcedureComponent> ownerMachine)
        {
            base.OnEnter(ownerMachine);

            // 切换流程时自动清空所有UI、场景、资源
            RemoveAllContentsOnProcedureTransition = true;

            // 执行Lua绑定的进入逻辑
            m_LuaOnEnter?.Invoke(ownerMachine);
        }

        /// <summary>
        /// 主流程每帧更新
        /// 等待过渡动画结束后，转发Update给Lua逻辑
        /// </summary>
        public override void OnUpdate(StateMachine<ProcedureComponent> ownerMachine)
        {
            // 等待流程过渡动画完成
            if (!m_EnterOver) 
                return;

            // 执行Lua绑定的帧更新逻辑
            m_LuaOnUpdate?.Invoke(ownerMachine);

            base.OnUpdate(ownerMachine);
        }

        /// <summary>
        /// 离开游戏主流程
        /// 调用Lua层退出逻辑，然后执行基类清理
        /// </summary>
        public override void OnLeave(StateMachine<ProcedureComponent> ownerMachine, bool isShutdown)
        {
            // 执行Lua绑定的离开逻辑
            m_LuaOnLeave?.Invoke(ownerMachine);

            base.OnLeave(ownerMachine, isShutdown);
        }

        #endregion
    }
}