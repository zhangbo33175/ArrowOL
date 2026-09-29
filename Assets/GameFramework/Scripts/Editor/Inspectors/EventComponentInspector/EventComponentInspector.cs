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
    /// 功能：运行时实时查看事件注册数量、待派发事件数量，用于调试事件系统
    /// 作用：监控 EventComponent / EventManager 运行状态
    /// </summary>
    [CustomEditor(typeof(EventComponent))]
    public class EventComponentInspector : HonorComponentInspector
    {
        #region 【编辑器生命周期】
        /// <summary>
        /// 绘制 Inspector 面板
        /// <para>仅运行时显示已注册事件类型数量与待派发事件数量，编辑态仅显示提示</para>
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

            EventComponent targetComponent = (EventComponent)target;

            // 只有在场景中的实例才显示数据（排除预制体预览）
            if (!IsPrefabInHierarchy(targetComponent.gameObject))
            {
                return;
            }

            DrawRuntimeStatistics(targetComponent);
        }

        /// <summary>
        /// 绘制运行时事件系统的实时统计信息
        /// </summary>
        /// <param name="targetComponent">当前选中的事件组件</param>
        private void DrawRuntimeStatistics(EventComponent targetComponent)
        {
            if (targetComponent.EventManager == null)
            {
                return;
            }

            // 已注册监听的事件类型数量
            EditorGUILayout.LabelField("已注册Event类型数量", targetComponent.SubscribedEventTypeCount.ToString());
            // 队列中等待派发的事件数量
            EditorGUILayout.LabelField("待派发Event数量", targetComponent.EventsForFireCount.ToString());
        }
        #endregion
    }
}
