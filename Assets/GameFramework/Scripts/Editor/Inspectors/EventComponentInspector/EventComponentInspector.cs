/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Editor
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  EventComponentInspector.cs
 * author:    云毅
 * created:   2026
 * descrip:   事件组件编辑器面板
 *            运行时实时查看事件注册、待派发数量，用于事件系统调试
 ***************************************************************/

using Honor.Runtime;
using UnityEditor;
using UnityEngine;

namespace Honor.Editor
{
    /// <summary>
    /// 【事件组件编辑器面板】
    /// 功能：运行时实时提示事件系统状态，用于调试事件系统
    /// 作用：监控 EventComponent / EventManager 运行状态
    /// </summary>
    // [CustomEditor(typeof(EventComponent))] // EventComponent 已改为纯 C# 组件（由门面 EventManager 创建持有）
    public class EventComponentInspector : HonorComponentInspector
    {
        #region 【编辑器生命周期】
        /// <summary>
        /// 绘制 Inspector 面板
        /// <para>仅运行时显示提示，编辑态仅显示提示</para>
        /// </summary>
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            // 非运行模式下只显示提示
            if (!EditorApplication.isPlaying)
            {
                EditorGUILayout.HelpBox("仅在运行时有效。", MessageType.Info);
                return;
            }
        }
        #endregion
    }
}
