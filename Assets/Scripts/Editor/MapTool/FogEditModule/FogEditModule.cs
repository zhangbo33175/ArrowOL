/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  FogEditModule.cs
 * author:    云毅
 * created:   2026
 * descrip:   迷雾编辑模块 - 公有接口部分（初始化/显隐）
 ***************************************************************/

using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace Honor.Editor.MapTool
{
    /// <summary>
    /// 迷雾编辑模块 - 公有接口
    /// </summary>
    public partial class FogEditModule
    {
        /// <summary>
        /// 初始化模块
        /// </summary>
        /// <param name="root">根节点</param>
        public void Init(VisualElement root)
        {
            m_Root = root;
            FindElements();
            BindEvents();
        }

        /// <summary>
        /// 显示模块
        /// </summary>
        public void Show()
        {
            m_Root.style.display = DisplayStyle.Flex;
        }

        /// <summary>
        /// 隐藏模块
        /// </summary>
        public void Hide()
        {
            m_Root.style.display = DisplayStyle.None;
        }

    }
}