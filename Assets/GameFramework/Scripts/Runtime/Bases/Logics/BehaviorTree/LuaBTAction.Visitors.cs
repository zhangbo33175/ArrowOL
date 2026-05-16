/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  LuaBTAction.cs
 * author:    云毅
 * created:   2026   2026年
 * descrip:   行为树Lua动作节点桥接类，实现C#与Lua层行为树动作逻辑的交互调用
 ***************************************************************/

using Honor.Runtime;
using XLua;

#if BEHAVIOR_DESIGNER_ENABLE
//=========================================================================
// 命名空间：BehaviorDesigner.Runtime.Tasks
// 描述：Behavior Designer行为树任务核心命名空间，Lua行为树动作节点归属
//=========================================================================
namespace BehaviorDesigner.Runtime.Tasks
{
    #region 类定义
    //=========================================================================
    // 类名：LuaBTAction
    // 描述：行为树Lua动作节点 partial 扩展类，作为C#与Lua层的桥接，
    //       存储Lua脚本引用、Lua类对象及所有Lua层回调函数引用
    // 继承：Action(Behavior Designer基础动作任务类)
    //=========================================================================
    /// <summary>
    /// 行为树Lua动作桥接类
    /// 负责关联LuaBehaviour组件、加载Lua行为树动作脚本、缓存Lua层所有回调方法
    /// </summary>
    public partial class LuaBTAction : Action
    {
        #region 公共序列化字段
        /// <summary>
        /// LuaBehaviour组件引用
        /// 用于绑定Lua运行环境与脚本实例
        /// </summary>
        [Tooltip("LuaBehaviour组件")]
        public LuaBehaviour m_LuaBehaviour;

        /// <summary>
        /// Lua行为树动作脚本名称
        /// 用于加载对应Lua层BTAction逻辑脚本
        /// </summary>
        [Tooltip("LuaBTAction脚本名称")]
        public string m_LuaBTActionName;
        #endregion

        #region 私有Lua对象缓存字段
        /// <summary>
        /// Lua层BTAction对应的类表对象
        /// 存储Lua脚本实例化后的类对象
        /// </summary>
        private LuaTable m_LuaClass;
        #endregion

        #region 私有Lua回调方法缓存字段
        /// <summary>
        /// Lua回调：行为树节点唤醒
        /// 对应Lua层OnAwakeBT方法
        /// </summary>
        private LuaFunction m_OnAwakeBT;

        /// <summary>
        /// Lua回调：行为树节点开始执行
        /// 对应Lua层OnStartBT方法
        /// </summary>
        private LuaFunction m_OnStartBT;

        /// <summary>
        /// Lua回调：行为树节点帧更新
        /// 对应Lua层OnUpdateBT方法
        /// </summary>
        private LuaFunction m_OnUpdateBT;

        /// <summary>
        /// Lua回调：行为树节点暂停
        /// 对应Lua层OnPauseBT方法
        /// </summary>
        private LuaFunction m_OnPauseBT;

        /// <summary>
        /// Lua回调：行为树节点重置
        /// 对应Lua层OnResetBT方法
        /// </summary>
        private LuaFunction m_OnResetBT;

        /// <summary>
        /// Lua回调：行为树节点结束执行
        /// 对应Lua层OnEndBT方法
        /// </summary>
        private LuaFunction m_OnEndBT;

        /// <summary>
        /// Lua回调：行为树固定帧更新
        /// 对应Lua层OnFixedUpdateBT方法
        /// </summary>
        private LuaFunction m_OnFixedUpdateBT;

        /// <summary>
        /// Lua回调：行为树延迟帧更新
        /// 对应Lua层OnLateUpdateBT方法
        /// </summary>
        private LuaFunction m_OnLateUpdateBT;

        /// <summary>
        /// Lua回调：行为树执行完成
        /// 对应Lua层OnBehaviorCompleteBT方法
        /// </summary>
        private LuaFunction m_OnBehaviorCompleteBT;

        /// <summary>
        /// Lua回调：行为树重启
        /// 对应Lua层OnBehaviorRestartBT方法
        /// </summary>
        private LuaFunction m_OnBehaviorRestartBT;

        /// <summary>
        /// Lua回调：获取行为树节点优先级
        /// 对应Lua层GetPriorityBT方法
        /// </summary>
        private LuaFunction m_GetPriorityBT;

        /// <summary>
        /// Lua回调：获取行为树效用值
        /// 对应Lua层GetUtilityBT方法
        /// </summary>
        private LuaFunction m_GetUtilityBT;

        /// <summary>
        /// Lua回调：动画IK回调
        /// 对应Lua层OnAnimatorIKBT方法
        /// </summary>
        private LuaFunction m_OnAnimatorIKBT;

        /// <summary>
        /// Lua回调：3D碰撞进入
        /// 对应Lua层OnCollisionEnterBT方法
        /// </summary>
        private LuaFunction m_OnCollisionEnterBT;

        /// <summary>
        /// Lua回调：2D碰撞进入
        /// 对应Lua层OnCollisionEnter2DBT方法
        /// </summary>
        private LuaFunction m_OnCollisionEnter2DBT;

        /// <summary>
        /// Lua回调：3D碰撞退出
        /// 对应Lua层OnCollisionExitBT方法
        /// </summary>
        private LuaFunction m_OnCollisionExitBT;

        /// <summary>
        /// Lua回调：2D碰撞退出
        /// 对应Lua层OnCollisionExit2DBT方法
        /// </summary>
        private LuaFunction m_OnCollisionExit2DBT;

        /// <summary>
        /// Lua回调：条件中止0回调
        /// 对应Lua层OnConditionalAbort0BT方法
        /// </summary>
        private LuaFunction m_OnConditionalAbort0BT;

        /// <summary>
        /// Lua回调：控制器碰撞触发
        /// 对应Lua层OnControllerColliderHitBT方法
        /// </summary>
        private LuaFunction m_OnControllerColliderHitBT;

        /// <summary>
        /// Lua回调：Gizmos绘制
        /// 对应Lua层OnDrawGizmosBT方法
        /// </summary>
        private LuaFunction m_OnDrawGizmosBT;

        /// <summary>
        /// Lua回调：节点文本绘制
        /// 对应Lua层OnDrawNodeTextBT方法
        /// </summary>
        private LuaFunction m_OnDrawNodeTextBT;

        /// <summary>
        /// Lua回调：3D触发器进入
        /// 对应Lua层OnTriggerEnterBT方法
        /// </summary>
        private LuaFunction m_OnTriggerEnterBT;

        /// <summary>
        /// Lua回调：2D触发器进入
        /// 对应Lua层OnTriggerEnter2DBT方法
        /// </summary>
        private LuaFunction m_OnTriggerEnter2DBT;

        /// <summary>
        /// Lua回调：3D触发器退出
        /// 对应Lua层OnTriggerExitBT方法
        /// </summary>
        private LuaFunction m_OnTriggerExitBT;

        /// <summary>
        /// Lua回调：2D触发器退出
        /// 对应Lua层OnTriggerExit2DBT方法
        /// </summary>
        private LuaFunction m_OnTriggerExit2DBT;
        #endregion
    }
    #endregion
}
#endif