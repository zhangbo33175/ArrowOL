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
    public partial class ItemElementEditModule : IMapEditModule
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

            // 列表项直接持有模板引用，选中时按 TemplateId 反查，不依赖字典快照的位置下标。
            int filteredCount = 0;
            foreach (var template in allTemplates)
            {
                if (template.Type != m_CurrentTypeTab) continue;

                var item = CreateItemElement(template);
                m_ItemList.Add(item);
                m_ItemItems.Add(item);
                filteredCount++;
            }

            MapToolLog.Info($"[ItemElementEditModule] 刷新元件列表，类型={m_CurrentTypeTab}，共加载 {filteredCount} 个模板");
        }

        /// <summary>
        /// 显示模块
        /// </summary>
        public void Show()
        {
            m_Root.SetVisible(true);
        }

        /// <summary>
        /// 隐藏模块
        /// </summary>
        public void Hide()
        {
            m_Root.SetVisible(false);
        }

    }
}
