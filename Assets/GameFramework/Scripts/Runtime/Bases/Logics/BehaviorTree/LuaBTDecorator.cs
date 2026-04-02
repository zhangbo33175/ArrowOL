
using Honor.Runtime;
using UnityEngine;
using XLua;

#if BEHAVIOR_DESIGNER_ENABLE
namespace BehaviorDesigner.Runtime.Tasks
{
    [TaskDescription("Honor自定义Lua层行为树Decorator节点组件，该组件在合适的时机将自动触发行为树节点的生命周期函数与各种回调到Lua脚本中，以确保行为树节点的逻辑通过调度Lua层代码及时完成。")]
    public partial class LuaBTDecorator : Decorator
    {
        public override void OnAwake()
        {
            if (m_LuaBehaviour == null)
            {
                m_LuaBehaviour = gameObject.GetComponent<LuaBehaviour>();
            }
            
            // 初始化LuaBTDecorator并绑定Lua层对应生命周期函数与回调方法
            if (m_LuaBehaviour != null)
            {
                LuaFunction createBTAction;
                m_LuaBehaviour.luaClass.Get("CreateBTDecorator", out createBTAction);

                LuaTable args = Root.Lua.Env.NewTable();
                args.Set("luaBTName", m_LuaBTDecoratorName);
                args.Set("csClass", this);

                m_LuaClass = createBTAction.Func<LuaTable, LuaTable, LuaTable>(m_LuaBehaviour.luaClass, args);
                if(m_LuaClass == null)
                {
                    Honor.Runtime.Log.Fatal("行为树Lua脚本：{0} 创建失败。", m_LuaBTDecoratorName);
                    return;
                }

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
                m_LuaClass.Get("OverrideStatus1BT", out m_OverrideStatus1BT);
                m_LuaClass.Get("OverrideStatus0BT", out m_OverrideStatus0BT);
            }

            if(m_OnAwakeBT != null)
            {
                m_OnAwakeBT.Action(m_LuaClass);
            }
            else
            {
                base.OnAwake();
            }
        }

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

        public override TaskStatus OnUpdate()
        {
            if (m_OnUpdateBT != null)
            {
                return m_OnUpdateBT.Func<LuaTable, TaskStatus>(m_LuaClass);
            }
            return base.OnUpdate();
        }

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

        public override float GetPriority()
        {
            if (m_GetPriorityBT != null)
            {
                return m_GetPriorityBT.Func<LuaTable, float>(m_LuaClass);
            }
            return base.GetPriority();
        }

        public override float GetUtility()
        {
            if (m_GetUtilityBT != null)
            {
                return m_GetUtilityBT.Func<LuaTable, float>(m_LuaClass);
            }
            return base.GetUtility();
        }

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

        public override string OnDrawNodeText()
        {
            if (m_OnDrawNodeTextBT != null)
            {
                return m_OnDrawNodeTextBT.Func<LuaTable, string>(m_LuaClass);
            }
            return base.OnDrawNodeText();
        }

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

        public override bool CanExecute()
        {
            if (m_CanExecuteBT != null)
            {
                return m_CanExecuteBT.Func<LuaTable, bool>(m_LuaClass);
            }
            return base.CanExecute();
        }

        public override bool CanReevaluate()
        {
            if (m_CanReevaluateBT != null)
            {
                return m_CanReevaluateBT.Func<LuaTable, bool>(m_LuaClass);
            }
            return base.CanReevaluate();
        }

        public override bool CanRunParallelChildren()
        {
            if (m_CanRunParallelChildrenBT != null)
            {
                return m_CanRunParallelChildrenBT.Func<LuaTable, bool>(m_LuaClass);
            }
            return base.CanRunParallelChildren();
        }

        public override int CurrentChildIndex()
        {
            if (m_CurrentChildIndexBT != null)
            {
               return m_CurrentChildIndexBT.Func<LuaTable, int>(m_LuaClass);
            }
            return base.CurrentChildIndex();
        }

        public override TaskStatus Decorate(TaskStatus status)
        {
            if (m_DecorateBT != null)
            {
                return m_DecorateBT.Func<LuaTable, TaskStatus, TaskStatus>(m_LuaClass, status);
            }
            return base.Decorate(status);
        }

        public override int MaxChildren()
        {
            if (m_MaxChildrenBT != null)
            {
                return m_MaxChildrenBT.Func<LuaTable, int>(m_LuaClass);
            }
            return base.MaxChildren();
        }

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

        public override TaskStatus OverrideStatus(TaskStatus status)
        {
            if (m_OverrideStatus1BT != null)
            {
                return m_OverrideStatus1BT.Func<LuaTable, TaskStatus, TaskStatus>(m_LuaClass, status);
            }
            return base.OverrideStatus(status);
        }

        public override TaskStatus OverrideStatus()
        {
            if (m_OverrideStatus0BT != null)
            {
                return m_OverrideStatus0BT.Func<LuaTable, TaskStatus>(m_LuaClass);
            }
            return base.OverrideStatus();
        }

    }
}

#endif