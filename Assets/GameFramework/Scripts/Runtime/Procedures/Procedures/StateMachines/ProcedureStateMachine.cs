namespace Honor.Runtime
{
    /// <summary>
    /// 流程状态机
    /// 游戏流程的核心驱动者，统一管理所有流程的切换、更新、生命周期
    /// </summary>
    public class ProcedureStateMachine : StateMachine<ProcedureComponent>
    {
        /// <summary>
        /// 流程状态机构造函数
        /// </summary>
        /// <param name="owner">所属的流程组件</param>
        /// <param name="states">需要托管的所有流程状态</param>
        public ProcedureStateMachine(ProcedureComponent owner, params State<ProcedureComponent>[] states)
            : base(owner, states)
        {

        }

        /// <summary>
        /// 状态机每帧更新
        /// 驱动当前流程的 OnUpdate 逻辑
        /// </summary>
        public override void Update()
        {
            base.Update();
        }

        /// <summary>
        /// 关闭并清理状态机
        /// 释放所有流程、注销回调、销毁资源
        /// </summary>
        public override void Shutdown()
        {
            base.Shutdown();
        }
    }
}