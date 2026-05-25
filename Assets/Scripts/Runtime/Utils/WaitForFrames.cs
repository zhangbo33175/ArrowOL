/***************************************************************
 * (c) copyright 2026 - 2030, GameLib
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  WaitForFrames.cs
 * author:    云毅
 * created:   2026
 * descrip:   自定义协程指令 - 等待指定帧数
 *            不受 timeScale 影响，适用于帧精准延迟逻辑
 ***************************************************************/

using UnityEngine;

namespace GameLib
{
    /// <summary>
    /// 自定义协程指令：等待指定帧数后再继续执行
    /// 用途：延迟几帧执行逻辑（比WaitForSeconds更精准、不受Time.scale影响）
    /// </summary>
    public class WaitForFrames : CustomYieldInstruction
    {
        #region 私有字段
        //=========================================================================
        // 私有字段
        //=========================================================================
        /// <summary>
        /// 目标帧计数（到达此帧就结束等待）
        /// </summary>
        private int _targetFrameCount;

        /// <summary>
        /// 需要等待的总帧数（缓存，用于Reset）
        /// </summary>
        private int _numberOfFrames;
        #endregion

        #region 构造方法
        //=========================================================================
        // 构造方法
        //=========================================================================
        /// <summary>
        /// 构造函数：传入要等待的帧数
        /// </summary>
        /// <param name="numberOfFrames">等待帧数</param>
        public WaitForFrames(int numberOfFrames)
        {
            _numberOfFrames = numberOfFrames;
            // 计算目标帧 = 当前帧 + 等待帧数
            _targetFrameCount = Time.frameCount + numberOfFrames;
        }
        #endregion

        #region 核心等待逻辑
        //=========================================================================
        // 核心等待逻辑
        //=========================================================================
        /// <summary>
        /// 【核心】保持等待的条件：当前帧 ＜ 目标帧
        /// 返回 true = 继续等待
        /// 返回 false = 结束等待
        /// </summary>
        public override bool keepWaiting => Time.frameCount < _targetFrameCount;
        #endregion

        #region 重置复用
        //=========================================================================
        // 重置复用
        //=========================================================================
        /// <summary>
        /// 重置等待状态（重用此对象时调用）
        /// </summary>
        public override void Reset()
        {
            _targetFrameCount = Time.frameCount + _numberOfFrames;
        }
        #endregion
    }
}