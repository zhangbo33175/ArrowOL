using System;
using Honor.Runtime;

namespace GameLib
{
    /// <summary>
    /// 有限状态机状态基类。
    /// </summary>
    /// <typeparam name="T">有限状态机持有者类型。</typeparam>
    public abstract class State<T> where T : class
    {
        /// <summary>
        /// 初始化有限状态机状态基类的新实例。
        /// </summary>
        public State()
        {
        }

        /// <summary>
        /// 有限状态机状态初始化时调用。
        /// </summary>
        /// <param name="ownerMachine">有限状态机引用。</param>
        public virtual void OnInit(StateMachine<T> ownerMachine)
        {
        }

        /// <summary>
        /// 有限状态机状态销毁时调用。
        /// </summary>
        /// <param name="ownerMachine">有限状态机引用。</param>
        public virtual void OnDestroy(StateMachine<T> ownerMachine)
        {
        }

        /// <summary>
        /// 有限状态机状态进入时调用。
        /// </summary>
        /// <param name="ownerMachine">有限状态机引用。</param>
        public virtual void OnEnter(StateMachine<T> ownerMachine)
        {
        }

        /// <summary>
        /// 有限状态机状态轮询时调用。
        /// </summary>
        /// <param name="ownerMachine">有限状态机引用。</param>
        public virtual void OnUpdate(StateMachine<T> ownerMachine)
        {
        }

        /// <summary>
        /// 有限状态机状态离开时调用。
        /// </summary>
        /// <param name="ownerMachine">有限状态机引用。</param>
        /// <param name="isShutdown">是否是关闭有限状态机时触发。</param>
        public virtual void OnLeave(StateMachine<T> ownerMachine, bool isShutdown)
        {
        }

        /// <summary>
        /// 切换当前有限状态机状态。
        /// </summary>
        /// <param name="ownerMachine">有限状态机引用。</param>
        /// <param name="stateType">要切换到的有限状态机状态类型。</param>
        public virtual void ChangeState(StateMachine<T> ownerMachine, Type stateType)
        {
            StateMachine<T> stateMachineImplement = (StateMachine<T>)ownerMachine;
            if (stateMachineImplement == null)
            {
                throw new Exception("状态机无效。");
            }

            if (stateType == null)
            {
                throw new Exception("State type 无效。");
            }

            if (!typeof(State<T>).IsAssignableFrom(stateType))
            {
                throw new Exception(AorTxt.Format("状态类型 '{0}' 无效。", stateType.FullName));
            }

            stateMachineImplement.ChangeState(stateType);
        }
    }
}