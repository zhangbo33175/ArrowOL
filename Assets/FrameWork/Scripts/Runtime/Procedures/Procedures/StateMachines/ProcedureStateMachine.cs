using GameLib;

namespace Honor.Runtime
{
    public class ProcedureStateMachine : StateMachine<ProcedureManager>
    {
        /// <summary>
        /// 流程状态机构造函数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="owner"></param>
        /// <param name="states"></param>
        public ProcedureStateMachine(ProcedureManager owner, params State<ProcedureManager>[] states)
            : base(owner, states)
        {

        }

        /// <summary>
        /// 流程状态机轮询
        /// </summary>
        public override void Update()
        {
            base.Update();

        }

        /// <summary>
        /// 关闭并清理流程状态机。
        /// </summary>
        public override void Shutdown()
        {
            base.Shutdown();

        }


    }
}