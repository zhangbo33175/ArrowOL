using UnityEngine;
using XLua;

namespace Honor.Runtime
{
    public class AnimationStateBehaviour : StateMachineBehaviour
    {
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var luaBehaviour = animator.GetComponent<LuaBehaviour>();
            if(luaBehaviour)
            {
                LuaTable luaTable = luaBehaviour.luaClass != null ? luaBehaviour.luaClass : luaBehaviour.luaClassView;
                luaTable.Get("OnAnimationStateEnter", out LuaFunction func);
                if (func != null) func.Action(luaTable, stateInfo);
            }
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {

        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var luaBehaviour = animator.GetComponent<LuaBehaviour>();
            if (luaBehaviour)
            {
                LuaTable luaTable = luaBehaviour.luaClass != null ? luaBehaviour.luaClass : luaBehaviour.luaClassView;
                luaTable.Get("OnAnimationStateExit", out LuaFunction func);
                if (func != null) func.Action(luaTable, stateInfo);
            }
        }

        // OnStateMove is called right after Animator.OnAnimatorMove()
        override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // 在OnAnimatorMove之前被调用 

        }

        // OnStateIK is called right after Animator.OnAnimatorIK()
        override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // 在OnAnimatorIK之后调用，用于在播放状态时的每一帧的monobehavior。
            // 需要注意的是，OnStateIK只有在状态位于具有IK pass的层上时才会被调用。
            // 默认情况下，图层没有IK通道，所以这个函数不会被调用
            // 关于IK的使用，可以看看这篇文章《Animator使用IK实现头部及身体跟随》
            // https://www.jianshu.com/p/ae6d65563efa

        }
    }
}


