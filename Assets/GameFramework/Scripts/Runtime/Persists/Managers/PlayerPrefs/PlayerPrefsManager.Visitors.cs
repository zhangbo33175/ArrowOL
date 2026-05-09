using System.Collections.Generic;

namespace Honor.Runtime
{
    public sealed partial class PlayerPrefsManager
    {
        /// <summary>
        /// 分类名称与对应键名的内存索引字典
        /// Key：分类名称
        /// Value：该分类下所有存储键的列表
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
    }
}