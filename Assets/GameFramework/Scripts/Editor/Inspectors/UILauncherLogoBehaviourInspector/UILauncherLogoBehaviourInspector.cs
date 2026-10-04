/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  UILauncherLogoBehaviourInspector.cs
 * author:    云毅
 * created:   2026
 * descrip:   启动器Logo行为编辑器面板
 *            以分组行形式可视化配置启动闪屏的进入/退出过渡与闪屏时序（黑屏/淡入/停留/淡出）
 ***************************************************************/

using Honor.Runtime;
using UnityEditor;
using UnityEngine;

namespace Honor.Editor
{
    /// <summary>
    /// 【启动器Logo行为编辑器面板】
    /// 功能：启动闪屏（Logo）的切换过渡（进入/退出）与闪屏时序的可视化配置
    /// 作用：在 Inspector 上以框式行呈现启动闪屏的过渡时长与阻塞射线，以及黑屏/淡入/停留/淡出时序
    /// 属于：框架 -> UI(SpecialUIs) 启动闪屏 -> 编辑器扩展
    /// </summary>
    [CustomEditor(typeof(UILauncherLogoBehaviour))]
    [CanEditMultipleObjects]
    internal sealed class UILauncherLogoBehaviourInspector : HonorComponentInspector
    {
        #region 序列化属性
        //=========================================================================
        // 序列化属性
        //=========================================================================

        // 图片引用
        private SerializedProperty m_BgImage;
        private SerializedProperty m_BottomMaskLayer;
        private SerializedProperty m_TopMaskLayer;
        private SerializedProperty m_SplashImage;
        private SerializedProperty m_SplashSprite;

        // 进入/退出过渡
        private SerializedProperty m_BlockRaycastOnEntering;
        private SerializedProperty m_BlockRaycastOnExiting;
        private SerializedProperty m_EnterDuration;
        private SerializedProperty m_ExitDuration;

        // 闪屏时序
        private SerializedProperty m_SplashBlackHoldDuration;
        private SerializedProperty m_SplashFadeInDuration;
        private SerializedProperty m_SplashStayDuration;
        private SerializedProperty m_SplashFadeOutDuration;
        #endregion

        #region 生命周期
        //=========================================================================
        // 生命周期
        //=========================================================================

        /// <summary>
        /// 启用：绑定所有序列化属性
        /// </summary>
        private void OnEnable()
        {
            m_BgImage                  = serializedObject.FindProperty("m_BgImage");
            m_BottomMaskLayer          = serializedObject.FindProperty("m_BottomMaskLayer");
            m_TopMaskLayer             = serializedObject.FindProperty("m_TopMaskLayer");
            m_SplashImage              = serializedObject.FindProperty("m_SplashImage");
            m_SplashSprite             = serializedObject.FindProperty("m_SplashSprite");

            m_BlockRaycastOnEntering   = serializedObject.FindProperty("m_BlockRaycastOnEntering");
            m_BlockRaycastOnExiting    = serializedObject.FindProperty("m_BlockRaycastOnExiting");
            m_EnterDuration            = serializedObject.FindProperty("m_EnterDuration");
            m_ExitDuration             = serializedObject.FindProperty("m_ExitDuration");

            m_SplashBlackHoldDuration  = serializedObject.FindProperty("m_SplashBlackHoldDuration");
            m_SplashFadeInDuration     = serializedObject.FindProperty("m_SplashFadeInDuration");
            m_SplashStayDuration       = serializedObject.FindProperty("m_SplashStayDuration");
            m_SplashFadeOutDuration    = serializedObject.FindProperty("m_SplashFadeOutDuration");
        }

        /// <summary>
        /// 绘制 Inspector 面板
        /// </summary>
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();

            DrawImageFields();
            EditorGUILayout.Space();
            DrawTransitionSection();
            EditorGUILayout.Space();
            DrawSplashTimingSection();

            serializedObject.ApplyModifiedProperties();
        }
        #endregion

        #region 绘制
        //=========================================================================
        // 绘制
        //=========================================================================

        /// <summary>
        /// 绘制图片引用字段（默认样式）
        /// </summary>
        private void DrawImageFields()
        {
            EditorGUILayout.PropertyField(m_BgImage,        new GUIContent("背景图"));
            EditorGUILayout.PropertyField(m_BottomMaskLayer, new GUIContent("底部遮罩层"));
            EditorGUILayout.PropertyField(m_TopMaskLayer,    new GUIContent("顶部遮罩层"));
            EditorGUILayout.PropertyField(m_SplashImage,     new GUIContent("闪屏图"));
            EditorGUILayout.PropertyField(m_SplashSprite,    new GUIContent("闪屏Sprite"));
        }

        /// <summary>
        /// 绘制进入/退出切换过渡（框式行，时间 + 阻塞射线）
        /// </summary>
        private void DrawTransitionSection()
        {
            EditorGUILayout.LabelField("切换过渡（进入/退出）", EditorStyles.boldLabel);

            DrawTransitionRow("进入式-切换过渡", m_EnterDuration, m_BlockRaycastOnEntering);
            DrawTransitionRow("退出式-切换过渡", m_ExitDuration,  m_BlockRaycastOnExiting);
        }

        /// <summary>
        /// 绘制一条切换过渡行
        /// </summary>
        /// <param name="label">行标签</param>
        /// <param name="duration">时长属性</param>
        /// <param name="blockRaycast">阻塞射线属性</param>
        private void DrawTransitionRow(string label, SerializedProperty duration, SerializedProperty blockRaycast)
        {
            EditorGUILayout.BeginHorizontal("box", GUILayout.Width(500));
            {
                EditorGUILayout.LabelField(label, GUILayout.Width(210));

                GUILayout.FlexibleSpace();
                EditorGUILayout.LabelField("时间", GUILayout.Width(40));
                duration.floatValue = EditorGUILayout.FloatField(duration.floatValue, GUILayout.Width(60));

                blockRaycast.boolValue =
                    EditorGUILayout.ToggleLeft("阻塞射线", blockRaycast.boolValue, GUILayout.Width(100));
            }
            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// 绘制闪屏时序（框式行，黑屏/淡入/停留/淡出），并附派生时长只读提示
        /// </summary>
        private void DrawSplashTimingSection()
        {
            EditorGUILayout.LabelField("闪屏时序（秒）", EditorStyles.boldLabel);

            DrawTimingRow("黑屏保持", m_SplashBlackHoldDuration);
            DrawTimingRow("淡入",     m_SplashFadeInDuration);
            DrawTimingRow("停留",     m_SplashStayDuration);
            DrawTimingRow("淡出",     m_SplashFadeOutDuration);

            // 派生时长只读提示（由上述配置即时计算，便于核对衔接点）
            float show      = m_SplashFadeInDuration.floatValue + m_SplashStayDuration.floatValue;
            float fadeStart = m_SplashBlackHoldDuration.floatValue + show;
            float total     = fadeStart + m_SplashFadeOutDuration.floatValue;

            EditorGUILayout.HelpBox(
                $"闪屏显示 {show:0.##}s · 淡出起始 {fadeStart:0.##}s · 完整序列 {total:0.##}s",
                MessageType.Info);
        }

        /// <summary>
        /// 绘制一条闪屏时序行
        /// </summary>
        /// <param name="label">行标签</param>
        /// <param name="prop">时长属性</param>
        private void DrawTimingRow(string label, SerializedProperty prop)
        {
            EditorGUILayout.BeginHorizontal("box", GUILayout.Width(500));
            {
                EditorGUILayout.LabelField(label, GUILayout.Width(210));

                GUILayout.FlexibleSpace();
                EditorGUILayout.LabelField("时间", GUILayout.Width(40));
                prop.floatValue = EditorGUILayout.FloatField(prop.floatValue, GUILayout.Width(60));
            }
            EditorGUILayout.EndHorizontal();
        }
        #endregion
    }
}
