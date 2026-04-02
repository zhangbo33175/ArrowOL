using System.Collections.Generic;
using System.IO;
using ExcelDataReader.Log;
using Honor.Editor;
using UnityEngine;
using UnityEditor;
using Log = Honor.Runtime.Log;

namespace Editor.MapEditor
{
    /// <summary>
    /// 
    /// </summary>
    public sealed partial class MapBuildEditor : EditorWindow
    {
        [MenuItem("Tools/地图编辑器")]
        public static void OpenWindow()
        {
            var window = GetWindow<MapBuildEditor>("地图编辑器");
            window.maxSize = new Vector2(1334, 860);
            window.minSize = new Vector2(1334, 860);
            window.Show();
        }
        private void OnGUI()
        {
            if (!m_IsStylesInitialized)
            {
                LoadConfig.LoadConfigs();
                InitStyles();
                m_IsStylesInitialized = true;
            }
            // 监听ESC键取消选中
            if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Escape)
            {
                DeselectCurrentObject();
                Event.current.Use();
                Repaint();
            }
            
            //初始化相机信息
            SetShowCameraView.InitRenderUtility();
            // 全局深色背景
            //EditorGUI.DrawRect(new Rect(0, 0, position.width, position.height), new Color(0.118f, 0.118f, 0.118f));
            // 根容器：横向布局
            GUILayout.BeginHorizontal(GUILayout.Width(1336), GUILayout.Height(860));
            {
                //====================== 左边布局 ======================
                SetLeftPanel();
                // ====================== 右侧布局 ======================
                SetRightPanel();
            }
            GUILayout.EndHorizontal();
        }
        
        /// <summary>
        /// 移除的时候清理资源信息
        /// </summary>
        private void OnDisable()
        {
            ClearAddedObjects();
            if (m_UiInstance != null) DestroyImmediate(m_UiInstance);
            if (m_RenderCam != null) DestroyImmediate(m_RenderCam.gameObject);
            if (m_RenderTexture != null) DestroyImmediate(m_RenderTexture);
        }
    }
}