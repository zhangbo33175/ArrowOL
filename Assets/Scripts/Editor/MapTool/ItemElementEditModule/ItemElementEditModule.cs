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
            // 示例元件数据（后续可接入配置表动态加载）
            string[] itemNames = { "钥匙", "猫咪零食", "金币", "碎片", "道具-锤子", "道具-刷子" };
            string[] itemDescs = { "Key - 点击收集", "CatFood - 自动拾取", "Coin - 点击收集", "Piece - 隐藏物品", "Hammer - 触发剧情", "Brush - 隐藏物品" };
            bool[] showFlags = { true, true, true, false, true, false };
            for (int i = 0; i < itemNames.Length; i++)
            {
                var item = CreateItemElement(itemNames[i], itemDescs[i], showFlags[i], i);
                m_ItemList.Add(item);
                m_ItemItems.Add(item);
            }
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