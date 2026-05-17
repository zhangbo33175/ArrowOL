/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  LuaBTConditional.cs
 * author:    云毅
 * created:   2026
 * descrip:   行为树Lua条件节点字段定义类，存储Lua组件引用、Lua实例及所有回调函数
 ***************************************************************/

using Honor.Runtime;
using XLua;

#if BEHAVIOR_DESIGNER_ENABLE
//=========================================================================
// 命名空间：BehaviorDesigner.Runtime.Tasks
// 描述：Behavior Designer行为树任务核心命名空间
//=========================================================================
namespace BehaviorDesigner.Runtime.Tasks
{
    #region Lua行为树条件节点字段定义
    /// <summary>
    /// 行为树Lua条件节点桥接字段类
    /// 存储Lua组件引用、Lua实例对象、所有Lua层生命周期与回调函数
    /// </summary>
    public partial class LuaBTConditional : Conditional
    {
        #region 公共序列化字段
        /// <summary>
        /// LuaBehaviour组件
        /// 绑定Lua运行环境与脚本实例
        /// </summary>
        [Tooltip("LuaBehaviour组件")]
        public LuaBehaviour m_LuaBehaviour;

        /// <summary>
        /// LuaBTConditional脚本名称
        /// 用于加载对应Lua层条件节点逻辑脚本
        /// </summary>
        [Tooltip("LuaBTConditional脚本名称")]
        public string m_LuaBTConditionalName;
        #endregion

        #region 私有Lua实例对象
        /// <summary>
        /// Lua层BTConditional对应类表对象
        /// Lua条件节点实例对象
        /// </summary>
        private LuaTable m_LuaClass;
        #endregion

        #region 私有Lua回调方法缓存
        /// <summary>
        /// Lua回调：节点唤醒
        /// </summary>
        private LuaFunction m_OnAwakeBT;

        /// <summary>
        /// Lua回调：节点开始
        /// </summary>
        private LuaFunction m_OnStartBT;

        /// <summary>
        /// Lua回调：节点帧更新
        /// </summary>
        private LuaFunction m_OnUpdateBT;

        /// <summary>
        /// Lua回调：节点暂停
        /// </summary>
        private LuaFunction m_OnPauseBT;

        /// <summary>
        /// Lua回调：节点重置
        /// </summary>
        private LuaFunction m_OnResetBT;

        /// <summary>
        /// Lua回调：节点结束
        /// </summary>
        private LuaFunction m_OnEndBT;

        /// <summary>
        /// Lua回调：固定帧更新
        /// </summary>
        private LuaFunction m_OnFixedUpdateBT;

        /// <summary>
        /// Lua回调：延迟帧更新
        /// </summary>
        private LuaFunction m_OnLateUpdateBT;

        /// <summary>
        /// Lua回调：行为树执行完成
        /// </summary>
        private LuaFunction m_OnBehaviorCompleteBT;

        /// <summary>
        /// Lua回调：行为树重启
        /// </summary>
        private LuaFunction m_OnBehaviorRestartBT;

        /// <summary>
        /// Lua回调：获取节点优先级
        /// </summary>
        private LuaFunction m_GetPriorityBT;

        /// <summary>
        /// Lua回调：获取节点效用值
        /// </summary>
        private LuaFunction m_GetUtilityBT;

        /// <summary>
        /// Lua回调：动画IK
        /// </summary>
        private LuaFunction m_OnAnimatorIKBT;

        /// <summary>
        /// Lua回调：3D碰撞进入
        /// </summary>
        private LuaFunction m_OnCollisionEnterBT;

        /// <summary>
        /// Lua回调：2D碰撞进入
        /// </summary>
        private LuaFunction m_OnCollisionEnter2DBT;

        /// <summary>
        /// Lua回调：3D碰撞退出
        /// </summary>
        private LuaFunction m_OnCollisionExitBT;

        /// <summary>
        /// Lua回调：2D碰撞退出
        /// </summary>
        private LuaFunction m_OnCollisionExit2DBT;

        /// <summary>
        /// Lua回调：条件中止（无参）
        /// </summary>
        private LuaFunction m_OnConditionalAbort0BT;

        /// <summary>
        /// Lua回调：控制器碰撞
        /// </summary>
        private LuaFunction m_OnControllerColliderHitBT;

        /// <summary>
        /// Lua回调：Gizmos绘制
        /// </summary>
        private LuaFunction m_OnDrawGizmosBT;

        /// <summary>
        /// Lua回调：节点文本绘制
        /// </summary>
        private LuaFunction m_OnDrawNodeTextBT;

        /// <summary>
        /// Lua回调：3D触发器进入
        /// </summary>
        private LuaFunction m_OnTriggerEnterBT;

        /// <summary>
        /// Lua回调：2D触发器进入
        /// </summary>
        private LuaFunction m_OnTriggerEnter2DBT;

        /// <summary>
        /// Lua回调：3D触发器退出
        /// </summary>
        private LuaFunction m_OnTriggerExitBT;

        /// <summary>
        /// Lua回调：2D触发器退出
        /// </summary>
        private LuaFunction m_OnTriggerExit2DBT;
        #endregion
    }
    #endregion
}
#endif