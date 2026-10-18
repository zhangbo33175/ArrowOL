/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  ItemElementEditModule.cs
 * author:    云毅
 * created:   2026
 * descrip:   物品元件编辑模块 - 公有接口部分（初始化/列表刷新/显隐）
 ***************************************************************/

using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

namespace Honor.Editor.MapTool
{
    /// <summary>
    /// 物品元件编辑模块 - 公有接口
    /// </summary>
    public partial class ItemElementEditModule
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
            RefreshItemList();
        }

        /// <summary>
        /// 刷新元件列表
        /// </summary>
        public void RefreshItemList()
        {
            if (m_ItemList == null) return;
            // 清空列表
            m_ItemList.Clear();
            m_ItemItems.Clear();

            // 从模板管理器加载所有模板，按当前Tab类型过滤
            var allTemplates = ObjectTemplateManager.Instance.GetAllTemplates();
            var templates = allTemplates.FindAll(t => t.Type == m_CurrentTypeTab);

            for (int i = 0; i < templates.Count; i++)
            {
                var template = templates[i];
                var item = CreateItemElement(template, i);
                m_ItemList.Add(item);
                m_ItemItems.Add(item);
            }

            Debug.Log($"[ItemElementEditModule] 刷新元件列表，类型={m_CurrentTypeTab}，共加载 {templates.Count} 个模板");
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