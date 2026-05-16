/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  PlayerPrefsManager.Fields.cs
 * author:    云毅
 * created:
 * descrip:   加密 PlayerPrefs 管理器 - 字段、属性与计数接口
 ***************************************************************/

using System.Collections.Generic;

namespace Honor.Runtime
{
    /// <summary>
    /// 加密 PlayerPrefs 管理器 - 字段与属性模块
    /// </summary>
    public sealed partial class PlayerPrefsManager
    {
        //=========================================================================
        #region 字段 & 属性
        //=========================================================================

        /// <summary>
        /// 分类名称与对应键名的内存索引字典
        /// Key：分类名称
        /// Value：该分类下所有存储键的列表
        /// </summary>
        private readonly SortedDictionary<string, List<string>> m_ItemNameGroups = null;

        /// <summary>
        /// 获取分类与键名索引字典（只读）
        /// </summary>
        public SortedDictionary<string, List<string>> ItemNameGroups
        {
            get { return m_ItemNameGroups; }
        }

        #endregion

        //=========================================================================
        #region 公共方法
        //=========================================================================

        /// <summary>
        /// 获取指定分类下的存储条目数量
        /// </summary>
        /// <param name="classifyName">分类名称</param>
        /// <returns>条目数量</returns>
        public int Count(string classifyName)
        {
            if (m_ItemNameGroups != null && m_ItemNameGroups.ContainsKey(classifyName))
            {
                return m_ItemNameGroups[classifyName].Count;
            }
            return 0;
        }

        #endregion
    }
}