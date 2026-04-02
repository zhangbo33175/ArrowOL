using System.Collections.Generic;

namespace Honor.Runtime
{
    public sealed partial class PlayerPrefsManager
    {
        /// <summary>
        /// 存储条目名称分类集合
        /// <classifyName，当前分类下的所有条目名称key>
        /// </summary>
        private readonly SortedDictionary<string, List<string>> m_ItemNameGroups = null;
        public SortedDictionary<string, List<string>> ItemNameGroups
        {
            get
            {
                return m_ItemNameGroups;
            }
        }

        /// <summary>
        /// 获取指定分类名称的条目数量。
        /// </summary>
        /// <param name="classifyName">分类名称</param>
        /// <returns>指定分类名称的条目数量</returns>
        public int Count(string classifyName)
        {
            if (m_ItemNameGroups != null && m_ItemNameGroups.ContainsKey(classifyName))
            {
                return m_ItemNameGroups[classifyName].Count;
            }
            return 0;
        }
    }

}


