/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  FogEditModule.Visitors.cs
 * author:    云毅
 * created:   2026
 * descrip:   迷雾编辑模块 - 字段与属性声明部分
 ***************************************************************/

using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace Honor.Editor.MapTool
{
    /// <summary>
    /// 迷雾编辑模块 - 字段与属性声明
    /// </summary>
    public partial class FogEditModule
    {
        #region 字段
        /// <summary>
        /// 模块根节点
        /// </summary>
        private VisualElement m_Root;
        // 迷雾参数控件（预留：后续绑定值变化事件驱动地图迷雾预览）
        /// <summary>
        /// 迷雾启用开关
        /// </summary>
        private Toggle m_FogEnableToggle;
        /// <summary>
        /// 迷雾浓度滑杆
        /// </summary>
        private Slider m_FogDensitySlider;
        /// <summary>
        /// 迷雾颜色选择器
        /// </summary>
        private ColorField m_FogColorField;
        // 操作按钮
        /// <summary>
        /// 清除迷雾按钮
        /// </summary>
        private Button m_ClearFogBtn;
        /// <summary>
        /// 重置迷雾按钮
        /// </summary>
        private Button m_ResetFogBtn;
        #endregion

        #region 属性
        /// <summary>
        /// 模块根节点
        /// </summary>
        public VisualElement Root => m_Root;
        #endregion

    }
}