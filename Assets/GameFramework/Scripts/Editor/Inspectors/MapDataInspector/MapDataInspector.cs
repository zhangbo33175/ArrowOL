/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  MapDataInspector.cs
 * author:    云毅
 * created:   2026
 * descrip:   地图数据组件自定义编辑器
 *            管理地图配置、相机参数、运动配置的 Inspector 可视化编辑
 ***************************************************************/

using UnityEditor;
using UnityEngine;

namespace Honor.Editor
{
    /// <summary>
    /// 【地图数据组件编辑器】
    /// 功能：地图核心配置面板，管理相机参数、缩放配置、红框视口、运动配置
    /// 作用：Inspector 下可视化编辑地图各项参数，支持分组折叠与调试
    /// </summary>
    [CustomEditor(typeof(MapData))]
    public class MapDataInspector : HonorComponentInspector
    {
        #region 常量配置

        private const string k_TitleMapConfig = "地图基础配置";
        private const string k_TitleCamConfig = "相机配置";
        private const string k_TitleZoomConfig = "缩放配置";
        private const string k_TitleViewportConfig = "红框视口配置";
        private const string k_TitleMotionConfig = "相机运动配置";

        #endregion

        #region 序列化字段

        private SerializedProperty m_ChapterId;
        private SerializedProperty m_LevelId;
        private SerializedProperty mapBounds;
        private SerializedProperty m_MapType;
        private SerializedProperty m_IsPad;

        private SerializedProperty m_NormalCamData;
        private SerializedProperty m_PlayCamData;
        private SerializedProperty m_ExitPlayCamData;
        private SerializedProperty m_MainHudMaxHeight;
        private SerializedProperty m_RMapChapterTypeData;

        private SerializedProperty m_MinCamSize;
        private SerializedProperty m_MaxCamSize;
        private SerializedProperty m_ScrollSensitivity;

        private SerializedProperty m_TopHudPixelHeight;
        private SerializedProperty m_BottomHudPixelHeight;
        private SerializedProperty m_MinScaleLockCenter;

        private SerializedProperty m_MotionConfig;

        #endregion

        #region 初始化

        protected void OnEnable()
        {
            m_ChapterId = serializedObject.FindProperty("m_ChapterId");
            m_LevelId = serializedObject.FindProperty("m_LevelId");
            mapBounds = serializedObject.FindProperty("mapBounds");
            m_MapType = serializedObject.FindProperty("m_MapType");
            m_IsPad = serializedObject.FindProperty("m_IsPad");

            m_NormalCamData = serializedObject.FindProperty("m_NormalCamData");
            m_PlayCamData = serializedObject.FindProperty("m_PlayCamData");
            m_ExitPlayCamData = serializedObject.FindProperty("m_ExitPlayCamData");
            m_MainHudMaxHeight = serializedObject.FindProperty("m_MainHudMaxHeight");
            m_RMapChapterTypeData = serializedObject.FindProperty("m_RMapChapterTypeData");

            m_MinCamSize = serializedObject.FindProperty("m_MinCamSize");
            m_MaxCamSize = serializedObject.FindProperty("m_MaxCamSize");
            m_ScrollSensitivity = serializedObject.FindProperty("m_ScrollSensitivity");

            m_TopHudPixelHeight = serializedObject.FindProperty("m_TopHudPixelHeight");
            m_BottomHudPixelHeight = serializedObject.FindProperty("m_BottomHudPixelHeight");
            m_MinScaleLockCenter = serializedObject.FindProperty("m_MinScaleLockCenter");

            m_MotionConfig = serializedObject.FindProperty("m_MotionConfig");
        }

        #endregion

        #region 绘制 Inspector

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawMapConfig();
            DrawCamConfig();
            DrawZoomConfig();
            DrawViewportConfig();
            DrawMotionConfig();

            serializedObject.ApplyModifiedProperties();
        }

        /// <summary>绘制地图基础配置</summary>
        private void DrawMapConfig()
        {
            EditorGUILayout.LabelField(k_TitleMapConfig, EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(m_ChapterId, new GUIContent("章节ID (ChapterId)"));
            EditorGUILayout.PropertyField(m_LevelId, new GUIContent("关卡ID (LevelId)"));
            EditorGUILayout.PropertyField(mapBounds, new GUIContent("地图边界 (MapBounds)"));
            EditorGUILayout.PropertyField(m_MapType, new GUIContent("地图类型 (MapType)"));
            EditorGUILayout.PropertyField(m_IsPad, new GUIContent("是否Pad端 (IsPad)"));
            EditorGUILayout.Space();
        }

        /// <summary>绘制相机配置</summary>
        private void DrawCamConfig()
        {
            EditorGUILayout.LabelField(k_TitleCamConfig, EditorStyles.boldLabel);
            DrawCamDataField(m_NormalCamData, "大厅相机 (NormalCam)");
            DrawCamDataField(m_PlayCamData, "游玩相机 (PlayCam)");
            DrawCamDataField(m_ExitPlayCamData, "退出游玩相机 (ExitPlayCam)");
            EditorGUILayout.PropertyField(m_MainHudMaxHeight, new GUIContent("主HUD最大高度 (MainHudMaxHeight)"));
            EditorGUILayout.PropertyField(m_RMapChapterTypeData, new GUIContent("章节配置数据 (ChapterData)"), true);
            EditorGUILayout.Space();
        }

        /// <summary>绘制单个 MapCamData 字段（手动展开子字段）</summary>
        private void DrawCamDataField(SerializedProperty prop, string label)
        {
            EditorGUILayout.PropertyField(prop, new GUIContent(label), false);
            if (prop.isExpanded)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(prop.FindPropertyRelative("size"), new GUIContent("尺寸 (Size)"));
                EditorGUILayout.PropertyField(prop.FindPropertyRelative("posX"), new GUIContent("位置X (PosX)"));
                EditorGUILayout.PropertyField(prop.FindPropertyRelative("posY"), new GUIContent("位置Y (PosY)"));
                EditorGUI.indentLevel--;
            }
        }

        /// <summary>绘制缩放配置</summary>
        private void DrawZoomConfig()
        {
            EditorGUILayout.LabelField(k_TitleZoomConfig, EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(m_MinCamSize, new GUIContent("最小相机尺寸 (MinCamSize)"));
            EditorGUILayout.PropertyField(m_MaxCamSize, new GUIContent("最大相机尺寸 (MaxCamSize)"));
            EditorGUILayout.PropertyField(m_ScrollSensitivity, new GUIContent("滚轮灵敏度 (ScrollSensitivity)"));
            EditorGUILayout.Space();
        }

        /// <summary>绘制红框视口配置</summary>
        private void DrawViewportConfig()
        {
            EditorGUILayout.LabelField(k_TitleViewportConfig, EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(m_TopHudPixelHeight, new GUIContent("顶部HUD像素高度 (TopHudPixelHeight)"));
            EditorGUILayout.PropertyField(m_BottomHudPixelHeight, new GUIContent("底部HUD像素高度 (BottomHudPixelHeight)"));
            EditorGUILayout.PropertyField(m_MinScaleLockCenter, new GUIContent("最小缩放锁定中心 (MinScaleLockCenter)"));
            EditorGUILayout.Space();
        }

        /// <summary>绘制相机运动配置</summary>
        private void DrawMotionConfig()
        {
            EditorGUILayout.LabelField(k_TitleMotionConfig, EditorStyles.boldLabel);
            DrawMotionConfigField(m_MotionConfig, "运动参数 (MotionConfig)");
        }

        /// <summary>绘制 MapMotionConfig 字段（手动展开子字段）</summary>
        private void DrawMotionConfigField(SerializedProperty prop, string label)
        {
            EditorGUILayout.PropertyField(prop, new GUIContent(label), false);
            if (prop.isExpanded)
            {
                EditorGUI.indentLevel++;

                // Canvas / 视口
                EditorGUILayout.LabelField("Canvas / 视口", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(prop.FindPropertyRelative("m_CanvasDesignHeight"), new GUIContent("Canvas设计高度 (CanvasDesignHeight)"));

                // 缩放控制
                EditorGUILayout.LabelField("缩放控制", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(prop.FindPropertyRelative("m_ZoomSpeed"), new GUIContent("缩放速度 (ZoomSpeed)"));
                EditorGUILayout.PropertyField(prop.FindPropertyRelative("m_MinZoomRatio"), new GUIContent("最小缩放比例 (MinZoomRatio)"));
                EditorGUILayout.PropertyField(prop.FindPropertyRelative("m_DragThreshold"), new GUIContent("拖动阈值 (DragThreshold)"));

                // 滑动 / 惯性
                EditorGUILayout.LabelField("滑动 / 惯性", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(prop.FindPropertyRelative("m_SwipeSmoothMin"), new GUIContent("平滑最小值 (SwipeSmoothMin)"));
                EditorGUILayout.PropertyField(prop.FindPropertyRelative("m_SwipeSmoothMax"), new GUIContent("平滑最大值 (SwipeSmoothMax)"));
                EditorGUILayout.PropertyField(prop.FindPropertyRelative("m_StepAddFactor"), new GUIContent("步进系数 (StepAddFactor)"));
                EditorGUILayout.PropertyField(prop.FindPropertyRelative("m_InertiaDecay"), new GUIContent("惯性衰减 (InertiaDecay)"));
                EditorGUILayout.PropertyField(prop.FindPropertyRelative("m_InertiaSmoothFloor"), new GUIContent("惯性平滑下限 (InertiaSmoothFloor)"));

                // 自动扫描
                EditorGUILayout.LabelField("自动扫描", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(prop.FindPropertyRelative("m_AutoScanOverlap"), new GUIContent("扫描重叠率 (AutoScanOverlap)"));
                EditorGUILayout.PropertyField(prop.FindPropertyRelative("m_AutoScanSegmentDuration"), new GUIContent("段动画时长 (AutoScanSegmentDuration)"));
                EditorGUILayout.PropertyField(prop.FindPropertyRelative("m_AutoScanDefaultOrtho"), new GUIContent("默认正交尺寸 (AutoScanDefaultOrtho)"));
                EditorGUILayout.PropertyField(prop.FindPropertyRelative("m_AutoScanGraceFrames"), new GUIContent("宽限帧数 (AutoScanGraceFrames)"));

                EditorGUI.indentLevel--;
            }
        }

        #endregion
    }
}
