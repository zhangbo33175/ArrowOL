using UnityEngine;

namespace Honor.Runtime
{
    public class ProcedurePlaying : ProcedureState
    {
        public override void OnInit(StateMachine<ProcedureComponent> ownerMachine)
        {
            base.OnInit(ownerMachine);

            m_Name = "ProcedurePlaying";

        }

        public override void OnEnter(StateMachine<ProcedureComponent> ownerMachine)
        {
            base.OnEnter(ownerMachine);

            // 切换到下一个流程时移除所有内容（可在Lua层控制）
            RemoveAllContentsOnProcedureTransition = true;

            if (m_LuaOnEnter != null)
            {
                m_LuaOnEnter(ownerMachine);
            }
        }

        public override void OnUpdate(StateMachine<ProcedureComponent> ownerMachine)
        {
            if (!m_EnterOver) return;

            if (m_LuaOnUpdate != null)
            {
                m_LuaOnUpdate(ownerMachine);
            }

            base.OnUpdate(ownerMachine);
        }

        public override void OnLeave(StateMachine<ProcedureComponent> ownerMachine, bool isShutdown)
        {
            if (m_LuaOnLeave != null)
            {
                m_LuaOnLeave(ownerMachine);
            }

            base.OnLeave(ownerMachine, isShutdown);
        }

    }
}


