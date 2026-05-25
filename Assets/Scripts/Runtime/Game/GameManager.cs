/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  GameManager.cs
 * author:    云毅
 * created:   2026
 * descrip:   游戏全局管理器
 *            核心单例，管理游戏生命周期、全局状态、场景切换
 ***************************************************************/

using Honor.Runtime;
using UnityEngine;

namespace GameLib
{
    /// <summary>
    /// 游戏全局管理器
    /// 核心单例，负责管理游戏整体生命周期、全局状态控制
    /// </summary>
    public class GameManager : MonoSingleton<GameManager>
    {
        #region 生命周期
        /// <summary>
        /// 初始化
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
        }
        #endregion
    }
}