/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Game
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  BaseEditorWindow.cs
 * author:    云毅
 * created:
 * descrip:   Honor框架 编辑器窗口基类
 *            提供单例/多实例窗口创建、尺寸、标题统一管理
 ***************************************************************/

using System;
using UnityEditor;
using UnityEngine;

namespace Honor.Editor
{
    #region 编辑器窗口基类
    /// <summary>
    /// 编辑器窗口泛型基类
    /// 提供单例/多实例创建、标题、尺寸、焦点管理的通用封装
    /// </summary>
    /// <typeparam name="T">继承自EditorWindow的窗口类型</typeparam>
    public abstract class BaseEditorWindow<T> : EditorWindow where T : EditorWindow
    {
        /// <summary>
        /// 静态单例实例
        /// </summary>
        protected static BaseEditorWindow<T> s_Instance;

        /// <summary>
        /// 窗口标题内容
        /// </summary>
        protected abstract GUIContent Title { get; }

        /// <summary>
        /// 窗口最小尺寸
        /// </summary>
        protected abstract Vector2 MinSize { get; }

        /// <summary>
        /// 窗口最大尺寸
        /// </summary>
        protected abstract Vector2 MaxSize { get; }

        /// <summary>
        /// 窗口显示委托（可重写）
        /// </summary>
        protected virtual Action ShowCall => Show;

        /// <summary>
        /// 打开单例窗口（全局唯一）
        /// </summary>
        public static T Open()
        {
            if (s_Instance == null)
            {
                s_Instance = GetWindow<T>() as BaseEditorWindow<T>;
                s_Instance.titleContent = s_Instance.Title;

                if (s_Instance.MinSize != Vector2.zero)
                {
                    s_Instance.minSize = s_Instance.MinSize;
                }

                if (s_Instance.MaxSize != Vector2.zero)
                {
                    s_Instance.maxSize = s_Instance.MaxSize;
                }
            }

            if (s_Instance != null)
            {
                s_Instance.ShowCall?.Invoke();
                s_Instance.Focus();
            }

            return s_Instance as T;
        }

        /// <summary>
        /// 打开窗口（支持单例/多实例）
        /// </summary>
        /// <param name="multiInstance">是否创建多实例</param>
        public static T Open(bool multiInstance)
        {
            if (!multiInstance)
            {
                return Open();
            }

            BaseEditorWindow<T> win = CreateInstance<T>() as BaseEditorWindow<T>;

            if (win != null)
            {
                win.titleContent = win.Title;

                if (win.MinSize != Vector2.zero)
                {
                    win.minSize = win.MinSize;
                }

                if (win.MaxSize != Vector2.zero)
                {
                    win.maxSize = win.MaxSize;
                }

                win.ShowCall?.Invoke();
                win.Focus();
            }

            return win as T;
        }

        /// <summary>
        /// GUI绘制
        /// </summary>
        protected virtual void OnGUI()
        {
            // 鼠标点击时清空控件焦点，避免输入框残留
            Event currentEvent = Event.current;
            if (currentEvent.type == EventType.MouseDown)
            {
                GUI.FocusControl(null);
            }
        }
    }
    #endregion
}