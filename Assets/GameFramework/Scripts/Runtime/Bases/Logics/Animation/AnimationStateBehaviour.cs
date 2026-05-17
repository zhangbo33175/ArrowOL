/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  AnimationStateBehaviour.cs
 * author:    云毅
 *  created:   2026
 * descrip:   动画状态机回调适配器 - 转发Animator事件到Lua
 ***************************************************************/
using UnityEngine;
using XLua;

namespace Honor.Runtime
{
    //=========================================================================
    // 
    //=========================================================================
    /// <summary>
    /// 动画状态机回调适配器
    /// 用于将 Animator 的状态事件转发到 Lua 脚本中处理
    /// </summary>
    public class AnimationStateBehaviour : StateMachineBehaviour
    {
        #region 动画状态进入
        //=========================================================================
        // 动画状态进入
        //=========================================================================
        /// <summary>
        /// 动画状态进入时调用
        /// </summary>
        /// <param name="animator">动画控制器</param>
        /// <param name="stateInfo">动画状态信息</param>
        /// <param name="layerIndex">动画层索引</param>
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (animator == null) return;

            LuaBehaviour luaBehaviour = animator.GetComponent<LuaBehaviour>();
            if (luaBehaviour == null) return;

            // 获取有效的Lua表（优先luaClass，否则使用luaClassView）
            LuaTable luaTable = luaBehaviour.luaClass ?? luaBehaviour.luaClassView;
            if (luaTable == null) return;

            // 调用Lua侧的OnAnimationStateEnter
            luaTable.Get("OnAnimationStateEnter", out LuaFunction func);
            if (func != null)
            {
                func.Call(luaTable, stateInfo);
                func.Dispose();
            }
        }
        #endregion

        #region 动画状态更新
        //=========================================================================
        // 动画状态更新
        //=========================================================================
        /// <summary>
        /// 动画状态每帧更新时调用
        /// </summary>
        /// <param name="animator">动画控制器</param>
        /// <param name="stateInfo">动画状态信息</param>
        /// <param name="layerIndex">动画层索引</param>
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // 状态持续期间每帧调用
        }
        #endregion

        #region 动画状态退出
        //=========================================================================
        // 动画状态退出
        //=========================================================================
        /// <summary>
        /// 动画状态退出时调用
        /// </summary>
        /// <param name="animator">动画控制器</param>
        /// <param name="stateInfo">动画状态信息</param>
        /// <param name="layerIndex">动画层索引</param>
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (animator == null) return;

            LuaBehaviour luaBehaviour = animator.GetComponent<LuaBehaviour>();
            if (luaBehaviour == null) return;

            // 获取有效的Lua表
            LuaTable luaTable = luaBehaviour.luaClass ?? luaBehaviour.luaClassView;
            if (luaTable == null) return;

            // 调用Lua侧的OnAnimationStateExit
            luaTable.Get("OnAnimationStateExit", out LuaFunction func);
            if (func != null)
            {
                func.Call(luaTable, stateInfo);
                func.Dispose();
            }
        }
        #endregion

        #region 动画移动与IK
        //=========================================================================
        // 动画移动与IK
        //=========================================================================
        /// <summary>
        /// 动画物体移动时调用（在 Animator.OnAnimatorMove() 之后调用）
        /// </summary>
        /// <param name="animator">动画控制器</param>
        /// <param name="stateInfo">动画状态信息</param>
        /// <param name="layerIndex">动画层索引</param>
        public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // 在 OnAnimatorMove 之后调用，用于处理 root motion 相关逻辑
        }

        /// <summary>
        /// 动画IK（反向动力学）回调，每帧调用
        /// </summary>
        /// <param name="animator">动画控制器</param>
        /// <param name="stateInfo">动画状态信息</param>
        /// <param name="layerIndex">动画层索引</param>
        public override void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // IK 处理回调
            // 注意：只有开启了 IK Pass 的动画层才会触发此方法
            // 常用于：头部瞄准、手部IK、武器瞄准等
        }
        #endregion
    }
}