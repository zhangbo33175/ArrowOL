using System;
using UnityEditor;
using UnityEngine;

namespace Honor.Editor
{
    public abstract class BaseEditorWindow<T> : EditorWindow where T : EditorWindow
    {
        /// <summary>
        /// 静态实例
        /// </summary>
        protected static BaseEditorWindow<T> s_Instance;

        /// <summary>
        /// 窗口标题
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
        /// 显示回调
        /// </summary>
        protected virtual Action ShowCall => Show;

        /// <summary>
        /// 打开窗口
        /// </summary>
        /// <returns></returns>
        public static T Open()
        {
            if(s_Instance == null) {
                s_Instance = GetWindow<T>() as BaseEditorWindow<T>;
                s_Instance.titleContent = s_Instance.Title;
                
                if(s_Instance.MinSize != Vector2.zero)
                {
                    s_Instance.minSize = s_Instance.MinSize;
                }
                
                if(s_Instance.MaxSize!= Vector2.zero) 
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
        /// 打开窗口
        /// </summary>
        /// <param name="multiInstance">是否为多实例</param>
        /// <returns></returns>
        public static T Open(bool multiInstance)
        {
            if (!multiInstance)
            {
                return Open();
            }
            
            var win = CreateInstance<T>() as BaseEditorWindow<T>;

            if (win != null)
            {
                win.titleContent = win.Title;
                
                if(win.MinSize != Vector2.zero) 
                {
                    win.minSize = win.MinSize;
                }
                    
                if(win.MaxSize!= Vector2.zero) 
                {
                    win.maxSize = win.MaxSize;
                }
        
                win.ShowCall?.Invoke();
                win.Focus();
            }

            return win as T;
        }

        protected virtual void OnGUI()
        {
            // 当鼠标按下时取消焦点
            Event e = Event.current;
            if (e.type == EventType.MouseDown)
            {
                GUI.FocusControl(null);
            }
        }

    }

}