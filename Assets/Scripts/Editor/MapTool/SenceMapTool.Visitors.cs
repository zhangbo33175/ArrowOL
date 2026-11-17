/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  SenceMapTool.Visitors.cs
 * author:    云毅
 * created:   2026
 * descrip:   场景地图编辑器主窗口 - 字段与属性声明部分
 ***************************************************************/

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Honor.Editor.MapTool.ResourceOrganize;

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

        // 编辑模式 Tab 注册表（Tab 按钮 ↔ 模块 ↔ 激活后动作），由 InitModules 统一装配
        /// <summary>
        /// 编辑模式 Tab 绑定项
        /// </summary>
        private sealed class EditTabBinding
        {
            /// <summary>Tab 按钮</summary>
            public Button Button;
            /// <summary>绑定的编辑模块</summary>
            public IMapEditModule Module;
            /// <summary>激活后的刷新动作（可空）</summary>
            public System.Action OnActivated;
        }

        /// <summary>
        /// 全部编辑模式 Tab 绑定项
        /// </summary>
        private readonly List<EditTabBinding> m_EditTabs = new List<EditTabBinding>();

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

        /// <summary>
        /// 资源整理模块
        /// </summary>
        private ResourceOrganizeModule m_ResourceOrganizeModule;

        #endregion
    }
}
