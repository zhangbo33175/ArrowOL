/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  PlayingComponent.cs
 * author:    云毅
 * created:   2026
 * descrip:   游戏运行时核心组件 - 流程控制、状态管理、逻辑驱动
 ***************************************************************/

namespace Honor.Runtime
{
    /// <summary>
    /// 游戏运行时核心组件
    /// 负责游戏进行中的逻辑管理、流程控制、运行时状态维护
    /// </summary>
    public partial class PlayingComponent : GameComponent
    {
        //=========================================================================
        #region 生命周期
        //=========================================================================

        /// <summary>
        /// 初始化：组件唤醒时执行
        /// 用于初始化引用、注册事件、预加载资源
        /// </summary>
        private void Awake()
        {
            
        }

        /// <summary>
        /// 启动：游戏开始时执行
        /// 用于启动游戏逻辑、开启流程、初始化运行时数据
        /// </summary>
        private void Start()
        {
            
        }

        #endregion
    }
}