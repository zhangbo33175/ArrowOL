using System;
using System.Collections.Generic;

namespace Honor.Runtime
{
    /// <summary>
    /// 游戏框架有序集合类（有序 + 去重 + O(1)查找）
    /// 特点：
    /// 1. 内部使用 链表 + 字典 组合实现
    /// 2. 保证元素**唯一不重复**
    /// 3. 保证元素**插入顺序**
    /// 4. 查找/添加/删除 时间复杂度接近 O(1)
    /// 适用场景：需要有序、去重、高效操作的游戏数据列表
    /// </summary>
    /// <typeparam name="T">集合中存储的元素类型</typeparam>
	public class GameLinkedSet<T> : IEnumerable<T>
	{
		/// <summary>
		/// 双向链表：维护元素的**插入顺序**，用于有序遍历
		/// </summary>
		private LinkedList<T> m_List;

		/// <summary>
		/// 哈希字典：建立 元素 -> 链表节点 的映射，用于 O(1) 快速查找、去重
		/// </summary>
		private Dictionary<T, LinkedListNode<T>> m_Dictionary;

		/// <summary>
		/// 使用默认相等比较器，初始化 GameLinkedSet 的新实例
		/// </summary>
		public GameLinkedSet()
		{
			m_List = new LinkedList<T>();
			m_Dictionary = new Dictionary<T, LinkedListNode<T>>();
		}

		/// <summary>
		/// 使用自定义相等比较器，初始化 GameLinkedSet 的新实例
		/// 用于自定义元素相等判断逻辑（如自定义类型去重规则）
		/// </summary>
		/// <param name="comparer">元素相等比较器</param>
		public GameLinkedSet(IEqualityComparer<T> comparer)
		{
			m_List = new LinkedList<T>();
			m_Dictionary = new Dictionary<T, LinkedListNode<T>>(comparer);
		}
		
		/// <summary>
		/// 向集合末尾追加元素（自动去重）
		/// </summary>
		/// <param name="t">要添加的元素</param>
		/// <returns>添加成功返回 true（元素不存在），已存在返回 false</returns>
		public bool Add(T t)
		{
			// 元素已存在，不允许重复添加，直接返回失败
			if (m_Dictionary.ContainsKey(t))
            {
				return false;
			}

			// 元素不存在：添加到链表尾部，并记录到字典
			LinkedListNode<T> node = m_List.AddLast(t);
			m_Dictionary.Add(t, node);
			return true;
		}

		/// <summary>
		/// 从集合中移除指定元素
		/// </summary>
		/// <param name="t">要移除的元素</param>
		/// <returns>移除成功返回 true，元素不存在返回 false</returns>
        public bool Remove(T t)
        {
            LinkedListNode<T> node;

			// 先从字典中快速查找对应的链表节点
            if (m_Dictionary.TryGetValue(t, out node))
            {
				// 同时从字典和链表中移除，保持数据一致
                m_Dictionary.Remove(t);
                m_List.Remove(node);
                return true;
            }
            else
            {
				// 未找到元素，移除失败
                return false;
            }
        }

		/// <summary>
		/// 清空集合中所有元素（字典+链表同时清空）
		/// </summary>
        public void Clear()
        {
			m_List.Clear();
			m_Dictionary.Clear();
		}

		/// <summary>
		/// 判断集合是否包含指定元素
		/// </summary>
		/// <param name="t">要判断的元素</param>
		/// <returns>包含返回 true，不包含返回 false</returns>
        public bool Contains(T t) => m_Dictionary.ContainsKey(t);

		/// <summary>
		/// 获取集合中当前的元素数量
		/// </summary>
        public int Count => m_List.Count;

		/// <summary>
		/// 获取强类型枚举器，支持 foreach 遍历（按插入顺序）
		/// </summary>
		/// <returns>枚举器</returns>
		public IEnumerator<T> GetEnumerator() => m_List.GetEnumerator();

		/// <summary>
		/// 显式实现非泛型接口枚举器
		/// </summary>
		/// <returns>非泛型枚举器</returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => m_List.GetEnumerator();
	}
}