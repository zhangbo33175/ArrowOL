/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  State.cs
 * author:    云毅
 * created:   2026
 * descrip:   有限状态机 - 抽象状态基类
 *            定义所有状态通用行为接口，供业务状态继承实现
 ***************************************************************/

using System;
using Honor.Runtime;

namespace GameLib
{
    //=========================================================================
    // 有限状态机状态基类
    //=========================================================================
    /// <summary>
    /// 有限状态机状态基类
    /// <para>所有业务状态必须继承此类实现具体逻辑</para>
    /// </summary>
    /// <typeparam name="T">有限状态机持有者类型</typeparam>
    public abstract class State<T> where T : class
    {
        #region 构造函数
        //=========================================================================
        // 构造函数
        //=========================================================================
        /// <summary>
        /// 初始化有限状态机状态基类的新实例
        /// </summary>
        public State()
        {
        }
        #endregion

        #region 状态生命周期方法
        //=========================================================================
        // 状态生命周期方法
        //=========================================================================
        /// <summary>
        /// 有限状态机状态初始化时调用
        /// </summary>
        /// <param name="ownerMachine">有限状态机引用</param>
        public virtual void OnInit(StateMachine<T> ownerMachine)
        {
        }

        /// <summary>
        /// 有限状态机状态销毁时调用
        /// </summary>
        /// <param name="ownerMachine">有限状态机引用</param>
        public virtual void OnDestroy(StateMachine<T> ownerMachine)
        {
        }

        /// <summary>
        /// 进入当前状态时调用
        /// </summary>
        /// <param name="ownerMachine">有限状态机引用</param>
        public virtual void OnEnter(StateMachine<T> ownerMachine)
        {
        }

        /// <summary>
        /// 状态每帧轮询更新时调用
        /// </summary>
        /// <param name="ownerMachine">有限状态机引用</param>
        public virtual void OnUpdate(StateMachine<T> ownerMachine)
        {
        }

        /// <summary>
        /// 离开当前状态时调用
        /// </summary>
        /// <param name="ownerMachine">有限状态机引用</param>
        /// <param name="isShutdown">是否是关闭有限状态机时触发</param>
        public virtual void OnLeave(StateMachine<T> ownerMachine, bool isShutdown)
        {
        }
        #endregion

        #region 状态切换工具方法
        //=========================================================================
        // 状态切换工具方法
        //=========================================================================
        /// <summary>
        /// 切换当前有限状态机状态
        /// </summary>
        /// <param name="ownerMachine">有限状态机引用</param>
        /// <param name="stateType">要切换到的目标状态类型</param>
        /// <exception cref="Exception">状态机或状态类型无效时抛出异常</exception>
        public virtual void ChangeState(StateMachine<T> ownerMachine, Type stateType)
        {
            State<T> stateMachineImplement = ownerMachine as State<T>;
            if (stateMachineImplement == null)
            {
                throw new Exception("状态机无效。");
            }

            if (stateType == null)
            {
                throw new Exception("状态类型不能为空。");
            }

            if (!typeof(State<T>).IsAssignableFrom(stateType))
            {
                throw new Exception(AorTxt.Format("状态类型 '{0}' 无效。", stateType.FullName));
            }

            StateMachine<T> stateMachine = ownerMachine as StateMachine<T>;
            stateMachine?.ChangeState(stateType);
        }
        #endregion
    }
}