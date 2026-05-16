using UnityEngine;

namespace Honor.Runtime
{
    #region 游戏框架组件基类
    /// <summary>
    /// 游戏框架组件基类
    /// 所有需要自动注册到 GameComponentsGroup 的 MonoBehaviour 组件都应继承此类
    /// 提供统一的生命周期注册机制
    /// </summary>
    public abstract class GameComponent : MonoBehaviour
    {
        //=========================================================================
        // 生命周期函数
        //=========================================================================
        /// <summary>
        /// Unity 生命周期：Awake
        /// 用于组件初始化，第一时间将自身注册到游戏组件管理组
        /// 子类可重写，但必须调用 base.Awake()
        /// </summary>
        protected virtual void Awake()
        {
            // 将当前组件实例注册到全局组件管理组，方便其他模块获取引用
            GameComponentsGroup.RegisterComponent(this);
        }
    }
    #endregion
}