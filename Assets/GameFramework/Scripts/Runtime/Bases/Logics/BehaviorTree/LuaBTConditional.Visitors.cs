
using Honor.Runtime;
using XLua;

#if BEHAVIOR_DESIGNER_ENABLE
namespace BehaviorDesigner.Runtime.Tasks
{
    public partial class LuaBTConditional : Conditional
    {
        /// <summary>
        /// LuaBehaviour组件
        /// </summary>
        [Tooltip("LuaBehaviour组件")]
        public LuaBehaviour m_LuaBehaviour;

        /// <summary>
        /// Lua脚本名称
        /// </summary>
        [Tooltip("LuaBTConditional脚本名称")]
        public string m_LuaBTConditionalName;

        /// <summary>
        /// Lua层BTConditional对应LuaClass
        /// </summary>
        private LuaTable m_LuaClass;

        /// <summary>
        /// LuaClass成员方法:OnAwakeBT
        /// </summary>
        private LuaFunction m_OnAwakeBT;

        /// <summary>
        /// LuaClass成员方法:OnStartBT
        /// </summary>
        private LuaFunction m_OnStartBT;

        /// <summary>
        /// LuaClass成员方法:OnUpdateBT
        /// </summary>
        private LuaFunction m_OnUpdateBT;

        /// <summary>
        /// LuaClass成员方法:OnPauseBT
        /// </summary>
        private LuaFunction m_OnPauseBT;

        /// <summary>
        /// LuaClass成员方法:OnResetBT
        /// </summary>
        private LuaFunction m_OnResetBT;

        /// <summary>
        /// LuaClass成员方法:OnEndBT
        /// </summary>
        private LuaFunction m_OnEndBT;

        /// <summary>
        /// LuaClass成员方法:OnFixedUpdateBT
        /// </summary>
        private LuaFunction m_OnFixedUpdateBT;

        /// <summary>
        /// LuaClass成员方法:OnLateUpdateBT
        /// </summary>
        private LuaFunction m_OnLateUpdateBT;

        /// <summary>
        /// LuaClass成员方法:OnBehaviorCompleteBT
        /// </summary>
        private LuaFunction m_OnBehaviorCompleteBT;

        /// <summary>
        /// LuaClass成员方法:OnBehaviorRestartBT
        /// </summary>
        private LuaFunction m_OnBehaviorRestartBT;

        /// <summary>
        /// LuaClass成员方法:GetPriorityBT
        /// </summary>
        private LuaFunction m_GetPriorityBT;

        /// <summary>
        /// LuaClass成员方法:GetUtilityBT
        /// </summary>
        private LuaFunction m_GetUtilityBT;

        /// <summary>
        /// LuaClass成员方法:OnAnimatorIKBT
        /// </summary>
        private LuaFunction m_OnAnimatorIKBT;

        /// <summary>
        /// LuaClass成员方法:OnCollisionEnterBT
        /// </summary>
        private LuaFunction m_OnCollisionEnterBT;

        /// <summary>
        /// LuaClass成员方法:OnCollisionEnter2DBT
        /// </summary>
        private LuaFunction m_OnCollisionEnter2DBT;

        /// <summary>
        /// LuaClass成员方法:m_OnCollisionExitBT
        /// </summary>
        private LuaFunction m_OnCollisionExitBT;

        /// <summary>
        /// LuaClass成员方法:OnCollisionExit2DBT
        /// </summary>
        private LuaFunction m_OnCollisionExit2DBT;

        /// <summary>
        /// LuaClass成员方法:OnConditionalAbort0BT
        /// </summary>
        private LuaFunction m_OnConditionalAbort0BT;

        /// <summary>
        /// LuaClass成员方法:OnControllerColliderHitBT
        /// </summary>
        private LuaFunction m_OnControllerColliderHitBT;

        /// <summary>
        /// LuaClass成员方法:OnDrawGizmosBT
        /// </summary>
        private LuaFunction m_OnDrawGizmosBT;

        /// <summary>
        /// LuaClass成员方法:OnDrawNodeTextBT
        /// </summary>
        private LuaFunction m_OnDrawNodeTextBT;

        /// <summary>
        /// LuaClass成员方法:OnTriggerEnterBT
        /// </summary>
        private LuaFunction m_OnTriggerEnterBT;

        /// <summary>
        /// LuaClass成员方法:OnTriggerEnter2DBT
        /// </summary>
        private LuaFunction m_OnTriggerEnter2DBT;

        /// <summary>
        /// LuaClass成员方法:OnTriggerExitBT
        /// </summary>
        private LuaFunction m_OnTriggerExitBT;

        /// <summary>
        /// LuaClass成员方法:OnTriggerExit2DBT
        /// </summary>
        private LuaFunction m_OnTriggerExit2DBT;

    }
}

#endif