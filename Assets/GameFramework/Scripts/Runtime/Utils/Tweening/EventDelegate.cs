/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * -------------------------------------------------------------
 * filename:  EventDelegate.cs
 * author:  云毅
 * created:
 * descrip:   轻量事件委托 - 支持方法绑定与一次回调，补间结束事件使用
 * 优化记录: 由旧版 HonorUtils.Tweening.EventDelegate 精简迁移，改为泛型 Action 封装
 ***************************************************************/

using System;
using System.Collections.Generic;

namespace Honor.Runtime
{
    /// <summary>
    /// 轻量事件委托
    /// 功能：补间/弹簧动画完成回调的统一封装，支持多回调注册与清空
    /// </summary>
    [Serializable]
    public class EventDelegate
    {
        #region 字段

        /// <summary>绑定的回调</summary>
        private Action m_Callback;

        /// <summary>一次性回调（执行后自动移除）</summary>
        private Action m_OneShotCallback;

        /// <summary>目标对象（弱引用语义，用于判空）</summary>
        private object m_Target;

        #endregion

        #region 绑定

        /// <summary>
        /// 绑定回调
        /// </summary>
        public void Set(Action callback)
        {
            m_Callback = callback;
        }

        /// <summary>
        /// 绑定带目标对象的回调（目标销毁后自动跳过）
        /// </summary>
        public void Set(object target, Action callback)
        {
            m_Target = target;
            m_Callback = callback;
        }

        /// <summary>
        /// 添加一次性回调
        /// </summary>
        public void AddOneShot(Action callback)
        {
            m_OneShotCallback += callback;
        }

        /// <summary>
        /// 是否已绑定任何回调
        /// </summary>
        public bool IsValid
        {
            get { return m_Callback != null || m_OneShotCallback != null; }
        }

        #endregion

        #region 执行

        /// <summary>
        /// 执行全部回调
        /// </summary>
        public void Execute()
        {
            // 目标对象已销毁（Unity 假死对象）则跳过
            if (m_Target != null && m_Target is UnityEngine.Object && (UnityEngine.Object)m_Target == null)
            {
                return;
            }

            if (m_Callback != null)
            {
                m_Callback();
            }

            if (m_OneShotCallback != null)
            {
                Action oneShot = m_OneShotCallback;
                m_OneShotCallback = null;
                oneShot();
            }
        }

        /// <summary>
        /// 清空全部回调
        /// </summary>
        public void Clear()
        {
            m_Callback = null;
            m_OneShotCallback = null;
            m_Target = null;
        }

        #endregion
    }
}
