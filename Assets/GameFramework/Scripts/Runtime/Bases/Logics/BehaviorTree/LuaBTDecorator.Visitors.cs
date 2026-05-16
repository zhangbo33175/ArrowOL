/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  LuaBTDecorator.cs
 * author:    云毅
 * created:   2026   2026年
 * descrip:   行为树Lua装饰节点字段定义类，存储Lua组件引用、Lua实例及所有回调函数
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
    #region Lua行为树装饰节点字段定义
    /// <summary>
    /// 行为树Lua装饰节点桥接字段类
    /// 存储Lua组件引用、Lua实例对象、所有Lua层生命周期与回调函数
    /// </summary>
    public partial class LuaBTDecorator : Decorator
    {
        #region 公共序列化字段
        /// <summary>
        /// LuaBehaviour组件
        /// 绑定Lua运行环境与脚本实例
        /// </summary>
        [Tooltip("LuaBehaviour组件")]
        public LuaBehaviour m_LuaBehaviour;

        /// <summary>
        /// LuaBTDecorator脚本名称
        /// 用于加载对应Lua层装饰节点逻辑脚本
        /// </summary>
        [Tooltip("LuaBTDecorator脚本名称")]
        public string m_LuaBTDecoratorName;
        #endregion

        #region 私有Lua实例对象
        /// <summary>
        /// Lua层BTDecorator对应类表对象
        /// Lua装饰节点实例对象
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

        /// <summary>
        /// Lua回调：是否可执行
        /// </summary>
        private LuaFunction m_CanExecuteBT;

        /// <summary>
        /// Lua回调：是否可重新评估
        /// </summary>
        private LuaFunction m_CanReevaluateBT;

        /// <summary>
        /// Lua回调：是否允许子节点并行
        /// </summary>
        private LuaFunction m_CanRunParallelChildrenBT;

        /// <summary>
        /// Lua回调：当前子节点索引
        /// </summary>
        private LuaFunction m_CurrentChildIndexBT;

        /// <summary>
        /// Lua回调：装饰任务状态
        /// </summary>
        private LuaFunction m_DecorateBT;

        /// <summary>
        /// Lua回调：最大子节点数量
        /// </summary>
        private LuaFunction m_MaxChildrenBT;

        /// <summary>
        /// Lua回调：子节点执行完成（索引+状态）
        /// </summary>
        private LuaFunction m_OnChildExecuted2BT;

        /// <summary>
        /// Lua回调：子节点执行完成（仅状态）
        /// </summary>
        private LuaFunction m_OnChildExecuted1BT;

        /// <summary>
        /// Lua回调：子节点开始（无参）
        /// </summary>
        private LuaFunction m_OnChildStarted0BT;

        /// <summary>
        /// Lua回调：子节点开始（带索引）
        /// </summary>
        private LuaFunction m_OnChildStarted1BT;

        /// <summary>
        /// Lua回调：条件中止（带索引）
        /// </summary>
        private LuaFunction m_OnConditionalAbort1BT;

        /// <summary>
        /// Lua回调：重写状态（带参数）
        /// </summary>
        private LuaFunction m_OverrideStatus1BT;

        /// <summary>
        /// Lua回调：重写状态（无参）
        /// </summary>
        private LuaFunction m_OverrideStatus0BT;
        #endregion
    }
    #endregion
}
#endif