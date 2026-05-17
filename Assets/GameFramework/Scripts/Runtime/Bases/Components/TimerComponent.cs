/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  TimerComponent.cs
 * author:    云毅
 * created:   2026
 * descrip:   全局计时器管理组件，提供延时、复用、自动销毁、查找删除等计时器功能
 *            基于MonoBehaviour驱动，继承框架组件自动注册管理
 ***************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Honor.Runtime
{
    #region 全局计时器组件
    /// <summary>
    /// 全局计时器组件
    /// </summary>
    /// <remarks>
    /// 负责管理所有延时计时器，基于 MonoBehaviour 更新驱动
    /// 继承 GameComponent 自动注册到框架，支持自动销毁、复用、查找删除
    /// </remarks>
    public class TimerComponent : GameComponent
    {
        //=========================================================================
        // 私有成员变量
        //=========================================================================
        /// <summary>
        /// 运行中的计时器集合
        /// </summary>
        private readonly List<TimerCounter> _timers = new List<TimerCounter>();

        //=========================================================================
        // 公共管理方法
        //=========================================================================
        /// <summary>
        /// 清空所有运行中的计时器
        /// </summary>
        public void Clear()
        {
            _timers.Clear();
        }

        //=========================================================================
        // Unity 生命周期
        //=========================================================================
        /// <summary>
        /// Unity 每帧更新
        /// </summary>
        /// <remarks>驱动所有计时器计时，倒序遍历保证安全删除元素</remarks>
        private void Update()
        {
            // 倒序遍历，防止移除元素导致的索引越界
            for (int i = _timers.Count - 1; i >= 0; i--)
            {
                TimerCounter timer = _timers[i];

                // 计时器为空，直接移除
                if (timer == null)
                {
                    _timers.RemoveAt(i);
                    continue;
                }

                // 绑定物体已销毁，自动清理计时器
                if (timer.DelObj == null || timer.DelObj.Equals(null))
                {
                    _timers.RemoveAt(i);
                    continue;
                }

                // 累加帧时间
                timer.DeltaTime += Time.deltaTime;

                // 时间到达触发回调，并移除计时器
                if (timer.DeltaTime >= timer.DelayTime)
                {
                    timer.Del?.Invoke(timer.Owner);
                    _timers.RemoveAt(i);
                }
            }
        }

        //=========================================================================
        // 计时器移除
        //=========================================================================
        /// <summary>
        /// 根据唯一标识移除指定计时器
        /// </summary>
        /// <param name="owner">计时器唯一标识</param>
        public void RemoveTimer(string owner)
        {
            if (string.IsNullOrEmpty(owner))
                return;

            for (int i = _timers.Count - 1; i >= 0; i--)
            {
                if (_timers[i].Owner == owner)
                {
                    _timers.RemoveAt(i);
                    break;
                }
            }
        }

        //=========================================================================
        // 计时器查找
        //=========================================================================
        /// <summary>
        /// 根据唯一标识获取计时器实例
        /// </summary>
        /// <param name="owner">计时器唯一标识</param>
        /// <returns>匹配的计时器实例，未找到返回null</returns>
        public TimerCounter GetTimerCounter(string owner)
        {
            foreach (var timer in _timers)
            {
                if (timer.Owner == owner)
                    return timer;
            }

            return null;
        }

        //=========================================================================
        // 计时器创建与复用
        //=========================================================================
        /// <summary>
        /// 添加或复用延时计时器
        /// </summary>
        /// <param name="time">延迟执行时间（秒）</param>
        /// <param name="del">计时完成回调委托</param>
        /// <param name="obj">绑定的GameObject，物体销毁则计时器自动失效</param>
        /// <param name="owner">计时器唯一标识，用于复用/查找/删除</param>
        /// <returns>创建或复用后的计时器实例</returns>
        public TimerCounter AddTimerCounter(float time, Action<string> del, GameObject obj, string owner = "")
        {
            if (obj == null)
                return null;

            TimerCounter timerCounter = null;
            
            // 存在唯一标识时尝试复用
            if (!string.IsNullOrEmpty(owner))
            {
                timerCounter = GetTimerCounter(owner);
            }

            // 复用已有计时器
            if (timerCounter != null)
            {
                timerCounter.DelayTime = time;
                timerCounter.Del = del;
                timerCounter.DelObj = obj;
                timerCounter.DeltaTime = 0f;
            }
            // 创建新计时器
            else
            {
                timerCounter = new TimerCounter
                {
                    Owner = owner,
                    Del = del,
                    DelayTime = time,
                    DelObj = obj,
                    DeltaTime = 0f
                };

                _timers.Add(timerCounter);
            }

            return timerCounter;
        }
    }
    #endregion

    #region 计时器数据结构
    /// <summary>
    /// 计时器数据结构
    /// </summary>
    /// <remarks>存储计时器的延时、回调、绑定对象、唯一标识等核心数据</remarks>
    [Serializable]
    public class TimerCounter
    {
        /// <summary>
        /// 计时完成回调委托
        /// </summary>
        public Action<string> Del;

        /// <summary>
        /// 目标延迟时间（秒）
        /// </summary>
        public float DelayTime;

        /// <summary>
        /// 当前累计计时时间
        /// </summary>
        public float DeltaTime;

        /// <summary>
        /// 计时器唯一标识
        /// </summary>
        public string Owner = string.Empty;

        /// <summary>
        /// 绑定的GameObject
        /// </summary>
        public GameObject DelObj;

        //=========================================================================
        // 公共工具方法
        //=========================================================================
        /// <summary>
        /// 获取当前计时器剩余时间
        /// </summary>
        /// <returns>剩余时间（秒）</returns>
        public float GetLeftTime()
        {
            return DelayTime - DeltaTime;
        }

        /// <summary>
        /// 设置计时器完成回调
        /// </summary>
        /// <param name="func">新的回调委托</param>
        public void SetCallBack(Action<string> func)
        {
            Del = func;
        }
    }
    #endregion
}