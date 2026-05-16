using System;
using System.Collections.Generic;
using UnityEngine;

namespace Honor.Runtime
{
    #region 全局计时器组件
    /// <summary>
    /// 全局计时器组件
    /// 负责管理所有延时计时器，基于 MonoBehaviour 更新驱动
    /// 继承 GameComponent 自动注册到框架
    /// </summary>
    public class TimerComponent : GameComponent
    {
        //=========================================================================
        // 私有成员变量
        //=========================================================================
        /// <summary>
        /// 存储所有正在运行的计时器
        /// </summary>
        private readonly List<TimerCounter> _timers = new List<TimerCounter>();

        //=========================================================================
        // 公共清空方法
        //=========================================================================
        /// <summary>
        /// 清空所有计时器
        /// </summary>
        public void Clear()
        {
            _timers.Clear();
        }

        //=========================================================================
        // 生命周期更新
        //=========================================================================
        /// <summary>
        /// 每帧更新所有计时器
        /// 倒序遍历，防止移除元素导致的索引越界
        /// </summary>
        private void Update()
        {
            // 倒序遍历，安全删除元素
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

                // 累加时间
                timer.DeltaTime += Time.deltaTime;

                // 时间到达，触发回调
                if (timer.DeltaTime >= timer.DelayTime)
                {
                    timer.Del?.Invoke(timer.Owner);
                    _timers.RemoveAt(i);
                }
            }
        }

        //=========================================================================
        // 移除指定计时器
        //=========================================================================
        /// <summary>
        /// 根据所有者标识移除计时器
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
        // 获取指定计时器
        //=========================================================================
        /// <summary>
        /// 根据所有者获取计时器
        /// </summary>
        /// <param name="owner">唯一标识</param>
        /// <returns>找到的计时器，没有则返回null</returns>
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
        // 添加/复用计时器
        //=========================================================================
        /// <summary>
        /// 添加/复用一个延时计时器
        /// 相同owner会复用，不会重复创建
        /// </summary>
        /// <param name="time">延迟时间（秒）</param>
        /// <param name="del">回调委托</param>
        /// <param name="obj">绑定的GameObject（物体销毁则计时器自动失效）</param>
        /// <param name="owner">唯一标识，用于查找/删除</param>
        /// <returns>创建或复用的计时器</returns>
        public TimerCounter AddTimerCounter(float time, Action<string> del, GameObject obj, string owner = "")
        {
            if (obj == null)
                return null;

            // 尝试复用已有计时器
            TimerCounter timerCounter = null;
            if (!string.IsNullOrEmpty(owner))
            {
                timerCounter = GetTimerCounter(owner);
            }

            if (timerCounter != null)
            {
                // 复用：重置参数
                timerCounter.DelayTime = time;
                timerCounter.Del = del;
                timerCounter.DelObj = obj;
                timerCounter.DeltaTime = 0f;
            }
            else
            {
                // 新建计时器
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
    /// 存储延时、回调、绑定对象、唯一标识等信息
    /// </summary>
    [Serializable]
    public class TimerCounter
    {
        /// <summary>
        /// 计时结束回调
        /// </summary>
        public Action<string> Del;

        /// <summary>
        /// 延迟时间（秒）
        /// </summary>
        public float DelayTime;

        /// <summary>
        /// 当前已计时时间
        /// </summary>
        public float DeltaTime;

        /// <summary>
        /// 所有者标识（唯一ID）
        /// </summary>
        public string Owner = string.Empty;

        /// <summary>
        /// 绑定的GameObject（物体销毁则自动停止计时）
        /// </summary>
        public GameObject DelObj;

        //=========================================================================
        // 公共方法
        //=========================================================================
        /// <summary>
        /// 获取剩余时间
        /// </summary>
        public float GetLeftTime()
        {
            return DelayTime - DeltaTime;
        }

        /// <summary>
        /// 设置回调方法
        /// </summary>
        public void SetCallBack(Action<string> func)
        {
            Del = func;
        }
    }
    #endregion
}