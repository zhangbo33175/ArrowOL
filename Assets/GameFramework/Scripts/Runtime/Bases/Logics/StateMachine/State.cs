/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  State.cs
 * author:    云毅
 * created:   2026   2025年
 * descrip:   有限状态机 - 抽象状态基类，所有自定义状态必须继承此类
 ***************************************************************/

using System;

namespace Honor.Runtime
{
    //=========================================================================
    // 有限状态机状态基类
    //=========================================================================
    /// <summary>
    /// 有限状态机状态基类
    /// 所有自定义状态都必须继承此类
    /// </summary>
    /// <typeparam name="T">状态机持有者类型（Owner）</typeparam>
    public abstract class State<T> where T : class
    {
        #region 构造函数
        /// <summary>
        /// 状态构造函数
        /// </summary>
        public State()
        {
        }
        #endregion

        #region 状态生命周期方法
        /// <summary>
        /// 状态初始化时调用（状态机创建时执行一次）
        /// </summary>
        /// <param name="ownerMachine">所属状态机</param>
        public virtual void OnInit(StateMachine<T> ownerMachine)
        {
        }

        /// <summary>
        /// 状态销毁时调用（状态机销毁时执行一次）
        /// </summary>
        /// <param name="ownerMachine">所属状态机</param>
        public virtual void OnDestroy(StateMachine<T> ownerMachine)
        {
        }

        /// <summary>
        /// 进入状态时调用
        /// </summary>
        /// <param name="ownerMachine">所属状态机</param>
        public virtual void OnEnter(StateMachine<T> ownerMachine)
        {
        }

        /// <summary>
        /// 状态每帧逻辑更新
        /// </summary>
        /// <param name="ownerMachine">所属状态机</param>
        public virtual void OnUpdate(StateMachine<T> ownerMachine)
        {
        }

        /// <summary>
        /// 离开状态时调用
        /// </summary>
        /// <param name="ownerMachine">所属状态机</param>
        /// <param name="isShutdown">是否为状态机关闭时触发</param>
        public virtual void OnLeave(StateMachine<T> ownerMachine, bool isShutdown)
        {
        }
        #endregion

        #region 状态切换
        /// <summary>
        /// 切换到指定状态
        /// </summary>
        /// <param name="ownerMachine">所属状态机</param>
        /// <param name="stateType">要切换的目标状态类型</param>
        /// <exception cref="GameException">状态机为空/状态类型无效时抛出异常</exception>
        public virtual void ChangeState(StateMachine<T> ownerMachine, Type stateType)
        {
            if (ownerMachine == null)
            {
                throw new GameException("状态机为空，无法切换状态。");
            }

            if (stateType == null)
            {
                throw new GameException("要切换的状态类型为空。");
            }

            if (!typeof(State<T>).IsAssignableFrom(stateType))
            {
                throw new GameException($"状态类型 '{stateType.FullName}' 不是有效的 State<{typeof(T)}> 类型。");
            }

            ownerMachine.ChangeState(stateType);
        }
        #endregion
    }
}