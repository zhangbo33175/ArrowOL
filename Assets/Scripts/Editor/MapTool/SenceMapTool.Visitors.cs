/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  SenceMapTool.Visitors.cs
 * author:    云毅
 * created:   2026
 * descrip:   场景地图编辑器主窗口 - 字段与属性声明部分
 ***************************************************************/

using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Honor.Editor.MapTool
{
    /// <summary>
    /// 场景地图编辑器窗口（UIToolkit） - 字段与属性声明
    /// </summary>
    public partial class SenceMapTool : EditorWindow
    {
        #region 字段

        /// <summary>
        /// 窗口视觉树资源
        /// </summary>
        [SerializeField]
        private VisualTreeAsset m_VisualTreeAsset = default;

        /// <summary>
        /// 窗口样式表
        /// </summary>
        [SerializeField]
        private StyleSheet m_StyleSheet = default;

        // 切页按钮引用
        /// <summary>
        /// 场景切页按钮
        /// </summary>
        private Button m_TabScene;
        /// <summary>
        /// 物品元件切页按钮
        /// </summary>
        private Button m_TabItemElement;
        /// <summary>
        /// 迷雾切页按钮
        /// </summary>
        private Button m_TabFog;

        // 模块引用
        /// <summary>
        /// 左侧面板模块
        /// </summary>
        private LeftPanelModule m_LeftPanelModule;
        /// <summary>
        /// 场景编辑模块
        /// </summary>
        private SceneEditModule m_SceneModule;
        /// <summary>
        /// 物品元件编辑模块
        /// </summary>
        private ItemElementEditModule m_ItemElementModule;
        /// <summary>
        /// 迷雾编辑模块
        /// </summary>
        private FogEditModule m_FogModule;

        #endregion
    }
}
