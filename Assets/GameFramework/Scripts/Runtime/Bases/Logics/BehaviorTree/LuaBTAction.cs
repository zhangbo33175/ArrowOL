using Honor.Runtime;
using UnityEngine;
using XLua;

#if BEHAVIOR_DESIGNER_ENABLE
namespace BehaviorDesigner.Runtime.Tasks
{
    [TaskDescription("Honor自定义Lua层行为树Action节点组件，该组件在合适的时机将自动触发行为树节点的生命周期函数与各种回调到Lua脚本中，以确保行为树节点的逻辑通过调度Lua层代码及时完成。")]
    public partial class LuaBTAction : Action
    {
        public override void OnAwake()
        {
            if (m_LuaBehaviour == null)
            {
                m_LuaBehaviour = gameObject.GetComponent<LuaBehaviour>();
            }
            
            // 初始化LuaBTAction并绑定Lua层对应生命周期函数与回调方法
            if (m_LuaBehaviour != null)
            {
                LuaFunction createBTAction;
                m_LuaBehaviour.luaClass.Get("CreateBTAction", out createBTAction);

                LuaTable args = Root.Lua.Env.NewTable();
                args.Set("luaBTName", m_LuaBTActionName);
                args.Set("csClass", this);

                m_LuaClass = createBTAction.Func<LuaTable, LuaTable, LuaTable>(m_LuaBehaviour.luaClass, args);
                if(m_LuaClass == null)
                {
                    Honor.Runtime.Log.Fatal("行为树Lua脚本：{0} 创建失败。", m_LuaBTActionName);
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

    }
}

#endif