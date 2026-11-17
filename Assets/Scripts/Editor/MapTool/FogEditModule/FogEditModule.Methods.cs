/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  FogEditModule.Methods.cs
 * author:    云毅
 * created:   2026
 * descrip:   迷雾编辑模块 - 私有方法部分（事件绑定/按钮回调）
 ***************************************************************/

using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace Honor.Editor.MapTool
{
    /// <summary>
    /// 迷雾编辑模块 - 私有方法
    /// </summary>
    public partial class FogEditModule
    {
        /// <summary>
        /// 绑定事件
        /// </summary>
        private void BindEvents()
        {
            m_ClearFogBtn = m_Root.QByName<Button>(MapToolUiNames.BtnClearFog);
            if (m_ClearFogBtn != null)
                m_ClearFogBtn.clicked += OnClearFogClicked;
            m_ResetFogBtn = m_Root.QByName<Button>(MapToolUiNames.BtnResetFog);
            if (m_ResetFogBtn != null)
                m_ResetFogBtn.clicked += OnResetFogClicked;
        }

        /// <summary>
        /// 清除迷雾按钮点击
        /// </summary>
        private void OnClearFogClicked()
        {
            MapToolLog.Info("清除迷雾");
        }

        /// <summary>
        /// 重置迷雾按钮点击
        /// </summary>
        private void OnResetFogClicked()
        {
            MapToolLog.Info("重置迷雾");
        }

        /// <summary>
        /// 查找UI元素
        /// </summary>
        private void FindElements()
        {
            m_FogEnableToggle = m_Root.Q<Toggle>(className: MapToolStyleClasses.FogEnableToggle);
            m_FogDensitySlider = m_Root.Q<Slider>(className: MapToolStyleClasses.FogDensitySlider);
            m_FogColorField = m_Root.Q<ColorField>(className: MapToolStyleClasses.FogColorField);
        }

    }
}
