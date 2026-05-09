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
    /// 地图编辑器主窗口
    /// 负责：窗口创建、GUI绘制、左右面板布局、资源释放、全局事件监听
    /// </summary>
    public sealed partial class MapBuildEditor : EditorWindow
    {
        /// <summary>
        /// 【编辑器入口】打开地图编辑器窗口
        /// 菜单路径：Tools / 地图编辑器
        /// </summary>
        [MenuItem("Tools/地图编辑器")]
        public static void OpenWindow()
        {
            // 创建并显示窗口
            MapBuildEditor window = GetWindow<MapBuildEditor>("地图编辑器");

            // 固定窗口大小（不可拉伸）
            window.maxSize = new Vector2(1334, 860);
            window.minSize = new Vector2(1334, 860);

            window.Show();
        }

        /// <summary>
        /// 编辑器GUI主绘制方法（每帧执行）
        /// 初始化样式、监听事件、绘制左右面板
        /// </summary>
        private void OnGUI()
        {
            // 首次打开：初始化配置 + GUI样式
            if (!m_IsStylesInitialized)
            {
                LoadConfig.LoadConfigs(); // 读取配置表
                InitStyles(); // 初始化编辑器皮肤样式
                m_IsStylesInitialized = true;
            }

            // 全局监听 ESC 键：一键取消选中物体
            if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Escape)
            {
                DeselectCurrentObject();
                Event.current.Use();
                Repaint();
            }

            // 初始化预览渲染相机（确保渲染环境正常）
            SetShowCameraView.InitRenderUtility();

            // ====================== 根布局：左右面板横向排列 ======================
            GUILayout.BeginHorizontal(GUILayout.Width(1336), GUILayout.Height(860));

            // 左侧面板：地图列表、物件列表、操作按钮
            SetLeftPanel();

            // 右侧面板：地图预览、状态栏、属性编辑
            SetRightPanel();

            GUILayout.EndHorizontal();
            // ====================================================================
        }

        /// <summary>
        /// 窗口关闭/禁用时：释放所有临时资源，防止内存泄漏
        /// 销毁预览实例、相机、渲染纹理
        /// </summary>
        private void OnDisable()
        {
            // 清空所有添加的物体
            ClearAddedObjects();

            // 销毁地图预览实例
            if (m_UiInstance != null)
                DestroyImmediate(m_UiInstance);

            // 销毁渲染相机
            if (m_RenderCam != null)
                DestroyImmediate(m_RenderCam.gameObject);

            // 销毁渲染纹理
            if (m_RenderTexture != null)
                DestroyImmediate(m_RenderTexture);
        }
    }
}