using System;
using System.Collections.Generic;
using UnityEngine;

namespace Honor.Runtime
{
    public class TimerComponent : GameComponent
    {
        // 管理的所有时间触发器
        List<TimerCounter> _Timers = new List<TimerCounter>();

        public void Clear()
        {
            _Timers.Clear();
        }

        void Update()
        {
            for (int i = _Timers.Count - 1; i >= 0; i--)
            {
                if (i >= _Timers.Count)
                {
                    break;
                }

                var timer = _Timers[i];
                if (timer != null)
                {
                    if (timer._DelObj == null || timer._DelObj.Equals(null))
                    {
                        _Timers.Remove(timer);
                        continue;
                    }

                    timer._DeltaTime += Time.deltaTime;
                    // 计时器触发了
                    if (timer._DeltaTime >= timer._DelayTime)
                    {
                        // 通知刷新
                        if (timer._Del != null)
                        {
                            timer._Del(timer._Owner);
                        }

                        _Timers.Remove(timer);
                    }
                }
            }
        }

        /// <summary>
        /// 移除一个计时器
        /// </summary>
        /// <param name="counter"></param>
        public void RemoveTimer(string owner)
        {
            if (string.IsNullOrEmpty(owner))
            {
                return;
            }

            for (int i = 0; i < _Timers.Count; i++)
            {
                if (_Timers[i]._Owner == owner)
                {
                    _Timers.Remove(_Timers[i]);
                    break;
                }
            }
        }

        public TimerCounter GetTimerCounter(string owner)
        {
            for (int i = 0; i < _Timers.Count; i++)
            {
                if (_Timers[i]._Owner == owner)
                {
                    return _Timers[i];
                }
            }

            return null;
        }

        public TimerCounter AddTimerCounter(float time, Action<string> del, GameObject obj, string owner = "")
        {
            if (obj == null)
            {
                return null;
            }

            TimerCounter timerCounter = null;
            if (!string.IsNullOrEmpty(owner))
            {
                timerCounter = GetTimerCounter(owner);
            }

            if (timerCounter != null)
            {
                timerCounter._DelayTime = time;
                timerCounter._Del = del;
                timerCounter._DelObj = obj;
                timerCounter._DeltaTime = 0f;
            }
            else
            {
                timerCounter = new TimerCounter();
                timerCounter._Owner = owner;
                timerCounter._Del = del;
                timerCounter._DelayTime = time;
                timerCounter._DelObj = obj;
                _Timers.Add(timerCounter);
            }

            return timerCounter;
        }
    }

    public class TimerCounter
    {
        public Action<string> _Del = null;
        public float _DelayTime; //持续时间
        public float _DeltaTime; //已经被计时的时间
        public string _Owner = string.Empty;
        public GameObject _DelObj = null;

        public float GetLeftTime()
        {
            return _DelayTime - _DeltaTime;
        }

        public void SetCallBack(Action<string> func)
        {
            _Del = func;
        }
    }
}