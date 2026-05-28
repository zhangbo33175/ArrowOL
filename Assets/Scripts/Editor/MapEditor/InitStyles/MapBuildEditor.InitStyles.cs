/***************************************************************
(c) copyright 2026 - 2030, Honor.Runtime
All Rights Reserved.
filename: MapBuildEditor.cs
author: 云毅
created: 2026
descrip: 地图编辑器 - 界面样式定义与初始化模块
***************************************************************/

using UnityEditor;
using UnityEngine;

namespace Editor.MapEditor
{
    /// <summary>
    /// 地图编辑器 partial 类 - 界面 GUI 样式模块
    /// 负责编辑器所有自定义 GUIStyle 的定义、初始化与管理
    /// </summary>
    public sealed partial class MapBuildEditor
    {
        #region 样式定义字段

        /// <summary>
        /// 左侧面板整体背景样式（用于左侧功能面板区域）
        /// </summary>
        private GUIStyle leftPanelStyle;

        /// <summary>
        /// 通用按钮样式（编辑器内所有功能按钮）
        /// </summary>
        private GUIStyle buttonStyle;

        /// <summary>
        /// 标签页【选中 / 激活】状态样式（当前打开的功能标签）
        /// </summary>
        private GUIStyle tabActiveStyle;

        /// <summary>
        /// 滚动视图样式（左侧长列表使用）
        /// </summary>
        private GUIStyle scrollViewStyle;

        /// <summary>
        /// 底部状态栏样式（显示提示、状态信息）
        /// </summary>
        private GUIStyle statusBarStyle;

        /// <summary>
        /// 分割线样式（界面区域之间的分隔线）
        /// </summary>
        private GUIStyle separatorStyle;

        /// <summary>
        /// 深色盒子背景样式（用于模块分组、深色背景区域）
        /// </summary>
        private GUIStyle darkBoxStyle;

        /// <summary>
        /// 文本标签样式（普通文字说明）
        /// </summary>
        private GUIStyle labelStyle;

        /// <summary>
        /// 列表项样式（左侧预制体 / 物体列表的每一项）
        /// </summary>
        private GUIStyle itemStyle;

        /// <summary>
        /// 列表项【选中】样式（当前选中的物体 / 预制体高亮样式）
        /// </summary>
        private GUIStyle _selectedStyle;

        /// <summary>
        /// 列表项【正常 / 未选中】样式（未选中时的默认样式）
        /// </summary>
        private GUIStyle _normalStyle;

        #endregion

        #region 样式初始化

        /// <summary>
        /// 初始化编辑器所有自定义 GUI 样式
        /// 不依赖系统默认皮肤，避免跨平台 / 跨线程显示异常
        /// </summary>
        private void InitStyles()
        {
// 左侧面板背景样式
            leftPanelStyle = new GUIStyle();
            leftPanelStyle.normal.background = MakeTex(2, 2, new Color(200f, 186f, 186f, 97f)); // #C8BABA
            leftPanelStyle.border = new RectOffset(0, 1, 0, 0);
            leftPanelStyle.padding = new RectOffset(0, 0, 0, 0);
// 通用按钮样式
            buttonStyle = new GUIStyle();
            buttonStyle.normal.background = MakeTex(2, 2, new Color(0.176f, 0.176f, 0.188f)); // #2D2D30
            buttonStyle.hover.background = MakeTex(2, 2, new Color(0.227f, 0.227f, 0.239f)); // #3A3A3D
            buttonStyle.active.background = MakeTex(2, 2, new Color(0.055f, 0.388f, 0.612f)); // #0E639C
            buttonStyle.normal.textColor = Color.white;
            buttonStyle.hover.textColor = Color.white;
            buttonStyle.active.textColor = Color.white;
            buttonStyle.border = new RectOffset(1, 1, 1, 1);
            buttonStyle.padding = new RectOffset(15, 15, 0, 0);
            buttonStyle.fixedHeight = 24;
            buttonStyle.alignment = TextAnchor.MiddleCenter;
// 激活标签页样式（继承按钮样式）
            tabActiveStyle = new GUIStyle(buttonStyle);
            tabActiveStyle.normal.background = MakeTex(2, 2, new Color(0.055f, 0.388f, 0.612f)); // #0E639C
            tabActiveStyle.fixedHeight = 30;
// 滚动视图背景样式
            scrollViewStyle = new GUIStyle();
            scrollViewStyle.normal.background = MakeTex(2, 2, new Color(0.118f, 0.118f, 0.118f, 0.38f));
// 底部状态栏样式
            statusBarStyle = new GUIStyle();
            statusBarStyle.normal.background = MakeTex(2, 2, new Color(0.145f, 0.145f, 0.149f)); // #252526
            statusBarStyle.border = new RectOffset(0, 0, 1, 0);
            statusBarStyle.padding = new RectOffset(10, 10, 0, 0);
// 界面分割线样式
            separatorStyle = new GUIStyle();
            separatorStyle.normal.background = MakeTex(2, 2, new Color(0.259f, 0.259f, 0.259f)); // #424242
            separatorStyle.fixedHeight = 3;
            separatorStyle.margin = new RectOffset(0, 0, 5, 5);
// 深色盒子背景样式
            darkBoxStyle = new GUIStyle();
            darkBoxStyle.normal.background = MakeTex(2, 2, new Color(0.455f, 0.439f, 0.439f)); // #746F6F
            darkBoxStyle.padding = new RectOffset(0, 0, 0, 0);
// 普通文本标签样式
            labelStyle = new GUIStyle();
            labelStyle.normal.textColor = Color.white;
            labelStyle.padding = new RectOffset(2, 2, 0, 0);
// 列表项基础样式
            itemStyle = new GUIStyle(EditorStyles.helpBox);
            itemStyle.padding = new RectOffset(10, 10, 10, 10);
            itemStyle.margin = new RectOffset(5, 5, 5, 5);
// 列表项正常 / 选中状态样式
            _normalStyle = new GUIStyle("Box");
            _selectedStyle = new GUIStyle("Box");
            _selectedStyle.normal.background = MakeTex(2, 2, new Color(0.2f, 0.5f, 0.8f, 0.3f));
        }

        #endregion

        #region 工具方法

        /// <summary>
        /// 【编辑器 GUI 工具】创建指定大小、指定颜色的纯色纹理
        /// 用于给 GUIStyle 制作背景、边框、色块等视觉效果（无需外部图片）
        /// </summary>
        /// <param name="width">纯色纹理的宽度（像素）</param>
        /// <param name="height">纯色纹理的高度（像素）</param>
        /// <param name="col">填充的纯色</param>
        /// <returns>生成好的只读纯色 Texture2D（自动设置为不压缩、可读）</returns>
        private Texture2D MakeTex(int width, int height, Color col)
        {
            Color[] pix = new Color[width * height];
            for (int i = 0; i < pix.Length; i++)
                pix[i] = col;
            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();
            return result;
        }

        #endregion
    }
}