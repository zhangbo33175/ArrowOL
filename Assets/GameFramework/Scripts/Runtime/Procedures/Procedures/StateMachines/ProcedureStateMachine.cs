namespace Honor.Runtime
{
    public class ProcedureStateMachine : StateMachine<ProcedureComponent>
    {
        /// <summary>
        /// 流程状态机构造函数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="owner"></param>
        /// <param name="states"></param>
        public ProcedureStateMachine(ProcedureComponent owner, params State<ProcedureComponent>[] states)
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


