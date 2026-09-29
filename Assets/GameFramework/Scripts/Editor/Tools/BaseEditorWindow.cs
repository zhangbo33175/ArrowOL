/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Game
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  BaseEditorWindow.cs
 * author:    云毅
 * created:   2026
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
                s_Instance = CreateSingletonWindow();
            }

            BringToFront(s_Instance);
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

            BaseEditorWindow<T> detachedWindow = CreateInstance<T>() as BaseEditorWindow<T>;
            if (detachedWindow == null)
            {
                return null;
            }

            InitializeWindowFrame(detachedWindow);
            BringToFront(detachedWindow);
            return detachedWindow as T;
        }

        /// <summary>
        /// 创建并初始化全局唯一的单例窗口
        /// </summary>
        /// <returns>新建的单例窗口实例</returns>
        private static BaseEditorWindow<T> CreateSingletonWindow()
        {
            BaseEditorWindow<T> singleton = GetWindow<T>() as BaseEditorWindow<T>;
            InitializeWindowFrame(singleton);
            return singleton;
        }

        /// <summary>
        /// 设置窗口标题与最小/最大尺寸
        /// </summary>
        /// <param name="targetWindow">待初始化的窗口实例</param>
        private static void InitializeWindowFrame(BaseEditorWindow<T> targetWindow)
        {
            targetWindow.titleContent = targetWindow.Title;

            if (targetWindow.MinSize != Vector2.zero)
            {
                targetWindow.minSize = targetWindow.MinSize;
            }

            if (targetWindow.MaxSize != Vector2.zero)
            {
                targetWindow.maxSize = targetWindow.MaxSize;
            }
        }

        /// <summary>
        /// 触发窗口显示委托并使其获得焦点
        /// </summary>
        /// <param name="targetWindow">需要前置显示的窗口实例</param>
        private static void BringToFront(BaseEditorWindow<T> targetWindow)
        {
            if (targetWindow == null)
            {
                return;
            }

            targetWindow.ShowCall?.Invoke();
            targetWindow.Focus();
        }

        /// <summary>
        /// GUI绘制
        /// </summary>
        protected virtual void OnGUI()
        {
            // 鼠标点击时清空控件焦点，避免输入框残留
            if (Event.current.type == EventType.MouseDown)
            {
                GUI.FocusControl(null);
            }
        }
    }
    #endregion
}
