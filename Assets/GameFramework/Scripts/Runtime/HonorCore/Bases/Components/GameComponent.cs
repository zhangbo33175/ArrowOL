/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  GameComponent.cs
 * author:    云毅
 * created:   2026
 * descrip:   游戏框架组件基类，实现组件自动注册到GameComponentsGroup的核心功能
 *            所有框架MonoBehaviour组件统一继承该基类，规范生命周期管理
 ***************************************************************/

using UnityEngine;

namespace Honor.Runtime
{
    #region 游戏框架组件基类
    /// <summary>
    /// 游戏框架组件基类
    /// </summary>
    /// <remarks>
    /// 所有需要自动注册到 GameComponentsGroup 的 MonoBehaviour 组件都应继承此类
    /// 提供统一的生命周期注册机制，是框架组件的基础父类
    /// </remarks>
    public abstract class GameComponent : MonoBehaviour
    {
        //=========================================================================
        // Unity 生命周期函数
        //=========================================================================
        /// <summary>
        /// Unity 内置生命周期函数：Awake
        /// </summary>
        /// <remarks>
        /// 组件初始化阶段执行，第一时间将自身注册到游戏组件管理组
        /// 子类重写时，必须调用 base.Awake() 保证注册逻辑执行
        /// </remarks>
        protected virtual void Awake()
        {
            // 注册当前组件实例到全局组件管理组，供其他模块调用
            GameComponentsGroup.RegisterComponent(this);
        }
    }
    #endregion
}