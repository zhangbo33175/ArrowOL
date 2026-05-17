/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  LuaBTComposite.cs
 * author:    云毅
 * created:   2026
 * descrip:   行为树Lua组合节点桥接类，承接Behavior Designer组合节点生命周期
 *            自动将节点回调转发至Lua层，实现C#与Lua逻辑解耦
 ***************************************************************/

using Honor.Runtime;
using UnityEngine;
using XLua;

#if BEHAVIOR_DESIGNER_ENABLE
//=========================================================================
// 命名空间：BehaviorDesigner.Runtime.Tasks
// 描述：Behavior Designer 行为树任务核心命名空间
//=========================================================================
namespace BehaviorDesigner.Runtime.Tasks
{
    #region 行为树Lua组合节点桥接类
    /// <summary>
    /// Honor自定义Lua层行为树Composite节点组件
    /// 该组件在合适的时机将自动触发行为树节点的生命周期函数与各种回调到Lua脚本中，
    /// 以确保行为树节点的逻辑通过调度Lua层代码及时完成。
    /// </summary>
    [TaskDescription("Honor自定义Lua层行为树Composite节点组件，该组件在合适的时机将自动触发行为树节点的生命周期函数与各种回调到Lua脚本中，以确保行为树节点的逻辑通过调度Lua层代码及时完成。")]
    public partial class LuaBTComposite : Composite
    {
        #region 生命周期方法
        /// <summary>
        /// 行为树节点唤醒
        /// 自动获取LuaBehaviour组件，创建Lua组合节点实例，绑定所有Lua回调函数
        /// </summary>
        public override void OnAwake()
        {
            // 自动获取LuaBehaviour组件
            if (m_LuaBehaviour == null)
            {
                m_LuaBehaviour = gameObject.GetComponent<LuaBehaviour>();
            }
            
            // 初始化LuaBTComposite并绑定Lua层对应生命周期函数与回调方法
            if (m_LuaBehaviour != null)
            {
                LuaFunction createBTAction;
                m_LuaBehaviour.luaClass.Get("CreateBTComposite", out createBTAction);

                LuaTable args = Root.Lua.Env.NewTable();
                args.Set("luaBTName", m_LuaBTCompositeName);
                args.Set("csClass", this);

                // 创建Lua层组合节点实例
                m_LuaClass = createBTAction.Func<LuaTable, LuaTable, LuaTable>(m_LuaBehaviour.luaClass, args);
                if(m_LuaClass == null)
                {
                    Honor.Runtime.Log.Fatal("行为树Lua脚本：{0} 创建失败。", m_LuaBTCompositeName);
                    return;
                }

                // 绑定所有Lua生命周期与回调方法
                m_LuaClass.Get("OnAwakeBT", out m_OnAwakeBT);
                m_LuaClass.Get("OnStartBT", out m_OnStartBT);
                m_LuaClass.Get("OnUpdateBT", out m_OnUpdateBT);
                m_LuaClass.Get("OnPauseBT", out m_OnPauseBT);
                m_LuaClass.Get("OnResetBT", out m_OnResetBT);
                m_LuaClass.Get("OnEndBT", out m_OnEndBT);
                m_LuaClass.Get("OnFixedUpdateBT", out m_OnFixedUpdateBT);
                m_LuaClass.Get("OnLateUpdateBT", out m_OnLateUpdateBT);
                m_LuaClass.Get("OnBehaviorCompleteBT", out m_OnBehaviorCompleteBT);
                m_LuaClass.Get("OnBehaviorRestartBT", out m_OnBehaviorRestartBT);
                m_LuaClass.Get("GetPriorityBT", out m_GetPriorityBT);
                m_LuaClass.Get("GetUtilityBT", out m_GetUtilityBT);
                m_LuaClass.Get("OnAnimatorIKBT", out m_OnAnimatorIKBT);
                m_LuaClass.Get("OnCollisionEnterBT", out m_OnCollisionEnterBT);
                m_LuaClass.Get("OnCollisionEnter2DBT", out m_OnCollisionEnter2DBT);
                m_LuaClass.Get("OnCollisionExitBT", out m_OnCollisionExitBT);
                m_LuaClass.Get("OnCollisionExit2DBT", out m_OnCollisionExit2DBT);
                m_LuaClass.Get("OnConditionalAbort0BT", out m_OnConditionalAbort0BT);
                m_LuaClass.Get("OnControllerColliderHitBT", out m_OnControllerColliderHitBT);
                m_LuaClass.Get("OnDrawGizmosBT", out m_OnDrawGizmosBT);
                m_LuaClass.Get("OnDrawNodeTextBT", out m_OnDrawNodeTextBT);
                m_LuaClass.Get("OnTriggerEnterBT", out m_OnTriggerEnterBT);
                m_LuaClass.Get("OnTriggerEnter2DBT", out m_OnTriggerEnter2DBT);
                m_LuaClass.Get("OnTriggerExitBT", out m_OnTriggerExitBT);
                m_LuaClass.Get("OnTriggerExit2DBT", out m_OnTriggerExit2DBT);
                m_LuaClass.Get("CanExecuteBT", out m_CanExecuteBT);
                m_LuaClass.Get("CanReevaluateBT", out m_CanReevaluateBT);
                m_LuaClass.Get("CanRunParallelChildrenBT", out m_CanRunParallelChildrenBT);
                m_LuaClass.Get("CurrentChildIndexBT", out m_CurrentChildIndexBT);
                m_LuaClass.Get("DecorateBT", out m_DecorateBT);
                m_LuaClass.Get("MaxChildrenBT", out m_MaxChildrenBT);
                m_LuaClass.Get("OnChildExecuted2BT", out m_OnChildExecuted2BT);
                m_LuaClass.Get("OnChildExecuted1BT", out m_OnChildExecuted1BT);
                m_LuaClass.Get("OnChildStarted0BT", out m_OnChildStarted0BT);
                m_LuaClass.Get("OnChildStarted1BT", out m_OnChildStarted1BT);
                m_LuaClass.Get("OnConditionalAbort1BT", out m_OnConditionalAbort1BT);
                m_LuaClass.Get("OverrideStatus1BT", out m_OverrideStatus1BT);
                m_LuaClass.Get("OverrideStatus0BT", out m_OverrideStatus0BT);
                m_LuaClass.Get("OnReevaluationEndedBT", out m_OnReevaluationEndedBT);
                m_LuaClass.Get("OnReevaluationStartedBT", out m_OnReevaluationStartedBT);
            }

            // 执行Lua层唤醒回调
            if(m_OnAwakeBT != null)
            {
                m_OnAwakeBT.Action(m_LuaClass);
            }
            else
            {
                base.OnAwake();
            }
        }

        /// <summary>
        /// 行为树节点开始执行
        /// </summary>
        public override void OnStart()
        {
            if (m_OnStartBT != null)
            {
                m_OnStartBT.Action(m_LuaClass);
            }
            else
            {
                base.OnStart();
            }
        }

        /// <summary>
        /// 行为树节点帧更新
        /// </summary>
        /// <returns>任务执行状态</returns>
        public override TaskStatus OnUpdate()
        {
            if (m_OnUpdateBT != null)
            {
                return m_OnUpdateBT.Func<LuaTable, TaskStatus>(m_LuaClass);
            }
            return base.OnUpdate();
        }

        /// <summary>
        /// 行为树节点暂停/恢复
        /// </summary>
        /// <param name="paused">是否暂停</param>
        public override void OnPause(bool paused)
        {
            if (m_OnPauseBT != null)
            {
                m_OnPauseBT.Action(m_LuaClass, paused);
            }
            else
            {
                base.OnPause(paused);
            }
        }

        /// <summary>
        /// 行为树节点重置
        /// </summary>
        public override void OnReset()
        {
            if (m_OnResetBT != null)
            {
                m_OnResetBT.Action(m_LuaClass);
            }
            else
            {
                base.OnReset();
            }
        }

        /// <summary>
        /// 行为树节点执行结束
        /// </summary>
        public override void OnEnd()
        {
            if (m_OnEndBT != null)
            {
                m_OnEndBT.Action(m_LuaClass);
            }
            else
            {
                base.OnEnd();
            }
        }

        /// <summary>
        /// 固定时间步更新
        /// </summary>
        public override void OnFixedUpdate()
        {
            if (m_OnFixedUpdateBT != null)
            {
                m_OnFixedUpdateBT.Action(m_LuaClass);
            }
            else
            {
                base.OnFixedUpdate();
            }
        }

        /// <summary>
        /// 延迟帧更新
        /// </summary>
        public override void OnLateUpdate()
        {
            if (m_OnLateUpdateBT != null)
            {
                m_OnLateUpdateBT.Action(m_LuaClass);
            }
            else
            {
                base.OnLateUpdate();
            }
        }

        /// <summary>
        /// 整个行为树执行完成回调
        /// </summary>
        public override void OnBehaviorComplete()
        {
            if (m_OnBehaviorCompleteBT != null)
            {
                m_OnBehaviorCompleteBT.Action(m_LuaClass);
            }
            else
            {
                base.OnBehaviorComplete();
            }
        }

        /// <summary>
        /// 行为树重启回调
        /// </summary>
        public override void OnBehaviorRestart()
        {
            if (m_OnBehaviorRestartBT != null)
            {
                m_OnBehaviorRestartBT.Action(m_LuaClass);
            }
            else
            {
                base.OnBehaviorRestart();
            }
        }
        #endregion

        #region 优先级 & 效用值
        /// <summary>
        /// 获取节点优先级
        /// </summary>
        /// <returns>优先级数值</returns>
        public override float GetPriority()
        {
            if (m_GetPriorityBT != null)
            {
                return m_GetPriorityBT.Func<LuaTable, float>(m_LuaClass);
            }
            return base.GetPriority();
        }

        /// <summary>
        /// 获取节点效用值
        /// </summary>
        /// <returns>效用值数值</returns>
        public override float GetUtility()
        {
            if (m_GetUtilityBT != null)
            {
                return m_GetUtilityBT.Func<LuaTable, float>(m_LuaClass);
            }
            return base.GetUtility();
        }
        #endregion

        #region 动画 & 碰撞 & 触发回调
        /// <summary>
        /// 动画IK回调
        /// </summary>
        public override void OnAnimatorIK()
        {
            if (m_OnAnimatorIKBT != null)
            {
                m_OnAnimatorIKBT.Action(m_LuaClass);
            }
            else
            {
                base.OnAnimatorIK();
            }
        }

        /// <summary>
        /// 3D碰撞进入
        /// </summary>
        /// <param name="collision">碰撞信息</param>
        public override void OnCollisionEnter(Collision collision)
        {
            if (m_OnCollisionEnterBT != null)
            {
                m_OnCollisionEnterBT.Action(m_LuaClass, collision);
            }
            else
            {
                base.OnCollisionEnter(collision);
            }
        }

        /// <summary>
        /// 2D碰撞进入
        /// </summary>
        /// <param name="collision">2D碰撞信息</param>
        public override void OnCollisionEnter2D(Collision2D collision)
        {
            if (m_OnCollisionEnter2DBT != null)
            {
                m_OnCollisionEnter2DBT.Action(m_LuaClass, collision);
            }
            else
            {
                base.OnCollisionEnter2D(collision);
            }
        }

        /// <summary>
        /// 3D碰撞退出
        /// </summary>
        /// <param name="collision">碰撞信息</param>
        public override void OnCollisionExit(Collision collision)
        {
            if (m_OnCollisionExitBT != null)
            {
                m_OnCollisionExitBT.Action(m_LuaClass, collision);
            }
            else
            {
                base.OnCollisionExit(collision);
            }
        }

        /// <summary>
        /// 2D碰撞退出
        /// </summary>
        /// <param name="collision">2D碰撞信息</param>
        public override void OnCollisionExit2D(Collision2D collision)
        {
            if (m_OnCollisionExit2DBT != null)
            {
                m_OnCollisionExit2DBT.Action(m_LuaClass, collision);
            }
            else
            {
                base.OnCollisionExit2D(collision);
            }
        }

        /// <summary>
        /// 条件中止（无参）
        /// </summary>
        public override void OnConditionalAbort()
        {
            if (m_OnConditionalAbort0BT != null)
            {
                m_OnConditionalAbort0BT.Action(m_LuaClass);
            }
            else
            {
                base.OnConditionalAbort();
            }
        }

        /// <summary>
        /// 控制器碰撞触发
        /// </summary>
        /// <param name="hit">碰撞信息</param>
        public override void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (m_OnControllerColliderHitBT != null)
            {
                m_OnControllerColliderHitBT.Action(m_LuaClass, hit);
            }
            else
            {
                base.OnControllerColliderHit(hit);
            }
        }

        /// <summary>
        /// Gizmos绘制
        /// </summary>
        public override void OnDrawGizmos()
        {
            if (m_OnDrawGizmosBT != null)
            {
                m_OnDrawGizmosBT.Action(m_LuaClass);
            }
            else
            {
                base.OnDrawGizmos();
            }
        }

        /// <summary>
        /// 绘制节点文本
        /// </summary>
        /// <returns>显示文本</returns>
        public override string OnDrawNodeText()
        {
            if (m_OnDrawNodeTextBT != null)
            {
                return m_OnDrawNodeTextBT.Func<LuaTable, string>(m_LuaClass);
            }
            return base.OnDrawNodeText();
        }

        /// <summary>
        /// 3D触发器进入
        /// </summary>
        /// <param name="other">碰撞体</param>
        public override void OnTriggerEnter(Collider other)
        {
            if (m_OnTriggerEnterBT != null)
            {
                m_OnTriggerEnterBT.Action(m_LuaClass, other);
            }
            else
            {
                base.OnTriggerEnter(other);
            }
        }

        /// <summary>
        /// 2D触发器进入
        /// </summary>
        /// <param name="other">2D碰撞体</param>
        public override void OnTriggerEnter2D(Collider2D other)
        {
            if (m_OnTriggerEnter2DBT != null)
            {
                m_OnTriggerEnter2DBT.Action(m_LuaClass, other);
            }
            else
            {
                base.OnTriggerEnter2D(other);
            }
        }

        /// <summary>
        /// 3D触发器退出
        /// </summary>
        /// <param name="other">碰撞体</param>
        public override void OnTriggerExit(Collider other)
        {
            if (m_OnTriggerExitBT != null)
            {
                m_OnTriggerExitBT.Action(m_LuaClass, other);
            }
            else
            {
                base.OnTriggerExit(other);
            }
        }

        /// <summary>
        /// 2D触发器退出
        /// </summary>
        /// <param name="other">2D碰撞体</param>
        public override void OnTriggerExit2D(Collider2D other)
        {
            if (m_OnTriggerExit2DBT != null)
            {
                m_OnTriggerExit2DBT.Action(m_LuaClass, other);
            }
            else
            {
                base.OnTriggerExit2D(other);
            }
        }
        #endregion

        #region 组合节点核心逻辑
        /// <summary>
        /// 是否可以执行节点
        /// </summary>
        /// <returns>是否可执行</returns>
        public override bool CanExecute()
        {
            if (m_CanExecuteBT != null)
            {
                return m_CanExecuteBT.Func<LuaTable, bool>(m_LuaClass);
            }
            return base.CanExecute();
        }

        /// <summary>
        /// 是否可以重新评估
        </summary>
        /// <returns>是否可重新评估</returns>
        public override bool CanReevaluate()
        {
            if (m_CanReevaluateBT != null)
            {
                return m_CanReevaluateBT.Func<LuaTable, bool>(m_LuaClass);
            }
            return base.CanReevaluate();
        }

        /// <summary>
        /// 是否允许子节点并行运行
        /// </summary>
        /// <returns>是否并行</returns>
        public override bool CanRunParallelChildren()
        {
            if (m_CanRunParallelChildrenBT != null)
            {
                return m_CanRunParallelChildrenBT.Func<LuaTable, bool>(m_LuaClass);
            }
            return base.CanRunParallelChildren();
        }

        /// <summary>
        /// 获取当前子节点索引
        /// </summary>
        /// <returns>子节点索引</returns>
        public override int CurrentChildIndex()
        {
            if (m_CurrentChildIndexBT != null)
            {
               return m_CurrentChildIndexBT.Func<LuaTable, int>(m_LuaClass);
            }
            return base.CurrentChildIndex();
        }

        /// <summary>
        /// 装饰任务状态
        /// </summary>
        /// <param name="status">原始状态</param>
        /// <returns>装饰后状态</returns>
        public override TaskStatus Decorate(TaskStatus status)
        {
            if (m_DecorateBT != null)
            {
                return m_DecorateBT.Func<LuaTable, TaskStatus, TaskStatus>(m_LuaClass, status);
            }
            return base.Decorate(status);
        }

        /// <summary>
        /// 获取最大子节点数量
        /// </summary>
        /// <returns>子节点数量上限</returns>
        public override int MaxChildren()
        {
            if (m_MaxChildrenBT != null)
            {
                return m_MaxChildrenBT.Func<LuaTable, int>(m_LuaClass);
            }
            return base.MaxChildren();
        }

        /// <summary>
        /// 子节点执行完成（带索引+状态）
        /// </summary>
        /// <param name="childIndex">子节点索引</param>
        /// <param name="childStatus">子节点状态</param>
        public override void OnChildExecuted(int childIndex, TaskStatus childStatus)
        {
            if (m_OnChildExecuted2BT != null)
            {
                m_OnChildExecuted2BT.Action(m_LuaClass, childIndex, childStatus);
            }
            else
            {
                base.OnChildExecuted(childIndex, childStatus);
            }
        }

        /// <summary>
        /// 子节点执行完成（仅状态）
        /// </summary>
        /// <param name="childStatus">子节点状态</param>
        public override void OnChildExecuted(TaskStatus childStatus)
        {
            if (m_OnChildExecuted1BT != null)
            {
                m_OnChildExecuted1BT.Action(m_LuaClass, childStatus);
            }
            else
            {
                base.OnChildExecuted(childStatus);
            }
        }

        /// <summary>
        /// 子节点开始执行（无参）
        /// </summary>
        public override void OnChildStarted()
        {
            if (m_OnChildStarted0BT != null)
            {
                m_OnChildStarted0BT.Action(m_LuaClass);
            }
            else
            {
                base.OnChildStarted();
            }
        }

        /// <summary>
        /// 子节点开始执行（带索引）
        /// </summary>
        /// <param name="childIndex">子节点索引</param>
        public override void OnChildStarted(int childIndex)
        {
            if (m_OnChildStarted1BT != null)
            {
                m_OnChildStarted1BT.Action(m_LuaClass, childIndex);
            }
            else
            {
                base.OnChildStarted(childIndex);
            }
        }

        /// <summary>
        /// 条件中止（带子节点索引）
        /// </summary>
        /// <param name="childIndex">子节点索引</param>
        public override void OnConditionalAbort(int childIndex)
        {
            if (m_OnConditionalAbort1BT != null)
            {
                m_OnConditionalAbort1BT.Action(m_LuaClass, childIndex);
            }
            else
            {
                base.OnConditionalAbort(childIndex);
            }
        }

        /// <summary>
        /// 重写任务状态（带参数）
        /// </summary>
        /// <param name="status">原始状态</param>
        /// <returns>重写后状态</returns>
        public override TaskStatus OverrideStatus(TaskStatus status)
        {
            if (m_OverrideStatus1BT != null)
            {
                return m_OverrideStatus1BT.Func<LuaTable, TaskStatus, TaskStatus>(m_LuaClass, status);
            }
            return base.OverrideStatus(status);
        }

        /// <summary>
        /// 重写任务状态（无参）
        /// </summary>
        /// <returns>重写后状态</returns>
        public override TaskStatus OverrideStatus()
        {
            if (m_OverrideStatus0BT != null)
            {
                return m_OverrideStatus0BT.Func<LuaTable, TaskStatus>(m_LuaClass);
            }
            return base.OverrideStatus();
        }

        /// <summary>
        /// 重新评估结束
        /// </summary>
        /// <param name="status">任务状态</param>
        public override void OnReevaluationEnded(TaskStatus status)
        {
            if (m_OnReevaluationEndedBT != null)
            {
                m_OnReevaluationEndedBT.Action(m_LuaClass, status);
            }
            else
            {
                base.OnReevaluationEnded(status);
            }
        }

        /// <summary>
        /// 重新评估开始
        /// </summary>
        /// <returns>是否允许重新评估</returns>
        public override bool OnReevaluationStarted()
        {
            if (m_OnReevaluationStartedBT != null)
            {
                return m_OnReevaluationStartedBT.Func<LuaTable, bool>(m_LuaClass);
            }
            return base.OnReevaluationStarted();
        }
        #endregion
    }
    #endregion
}
#endif