using System.Collections;
using System.Collections.Generic;

namespace Honor.Runtime
{
    /// <summary>
    /// 游戏框架多值字典类
    /// 功能：一个主键对应多个值，底层使用链表+字典组合实现，高效存储与遍历同键多值数据
    /// 适用于游戏中需要一对多关联的数据管理场景
    /// </summary>
    /// <typeparam name="TKey">多值字典的主键类型</typeparam>
    /// <typeparam name="TValue">多值字典的值类型</typeparam>
    public sealed class GameMultiDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, GameLinkedListRange<TValue>>>, IEnumerable
    {
        /// <summary>
        /// 全局存储所有值的双向链表，所有键对应的值都存在这个链表中
        /// </summary>
        private readonly GameLinkedList<TValue> m_LinkedList;

        /// <summary>
        /// 主键映射到链表区间的字典，实现键到多值的快速查找
        /// </summary>
        private readonly Dictionary<TKey, GameLinkedListRange<TValue>> m_Dictionary;

        /// <summary>
        /// 初始化游戏框架多值字典类的新实例
        /// </summary>
        public GameMultiDictionary()
        {
            m_LinkedList = new GameLinkedList<TValue>();
            m_Dictionary = new Dictionary<TKey, GameLinkedListRange<TValue>>();
        }

        /// <summary>
        /// 获取多值字典中实际包含的【主键总数】
        /// </summary>
        public int Count
        {
            get
            {
                return m_Dictionary.Count;
            }
        }

        /// <summary>
        /// 索引器：通过主键获取对应的链表区间
        /// </summary>
        /// <param name="key">要获取的主键</param>
        /// <returns>主键对应的链表区间，不存在则返回默认值</returns>
        public GameLinkedListRange<TValue> this[TKey key]
        {
            get
            {
                GameLinkedListRange<TValue> range = default(GameLinkedListRange<TValue>);
                m_Dictionary.TryGetValue(key, out range);
                return range;
            }
        }

        /// <summary>
        /// 清空多值字典所有数据（清空字典+清空链表）
        /// </summary>
        public void Clear()
        {
            m_Dictionary.Clear();
            m_LinkedList.Clear();
        }

        /// <summary>
        /// 检查字典中是否包含指定主键
        /// </summary>
        /// <param name="key">要检查的主键</param>
        /// <returns>包含返回true，否则false</returns>
        public bool Contains(TKey key)
        {
            return m_Dictionary.ContainsKey(key);
        }

        /// <summary>
        /// 检查指定主键下是否包含指定值
        /// </summary>
        /// <param name="key">目标主键</param>
        /// <param name="value">要检查的值</param>
        /// <returns>存在返回true，否则false</returns>
        public bool Contains(TKey key, TValue value)
        {
            GameLinkedListRange<TValue> range = default(GameLinkedListRange<TValue>);
            if (m_Dictionary.TryGetValue(key, out range))
            {
                return range.Contains(value);
            }

            return false;
        }

        /// <summary>
        /// 尝试根据主键获取对应的链表区间（安全获取，不抛异常）
        /// </summary>
        /// <param name="key">目标主键</param>
        /// <param name="range">获取到的区间（输出参数）</param>
        /// <returns>获取成功返回true，否则false</returns>
        public bool TryGetValue(TKey key, out GameLinkedListRange<TValue> range)
        {
            return m_Dictionary.TryGetValue(key, out range);
        }

        /// <summary>
        /// 向指定主键添加一个值
        /// 规则：主键已存在 → 追加到对应区间；主键不存在 → 新建区间并添加值
        /// </summary>
        /// <param name="key">目标主键</param>
        /// <param name="value">要添加的值</param>
        public void Add(TKey key, TValue value)
        {
            GameLinkedListRange<TValue> range = default(GameLinkedListRange<TValue>);
            if (m_Dictionary.TryGetValue(key, out range))
            {
                // 键已存在，将值添加到区间结束节点之前
                m_LinkedList.AddBefore(range.Terminal, value);
            }
            else
            {
                // 键不存在，创建新的链表节点组：首节点+结束标记节点
                LinkedListNode<TValue> first = m_LinkedList.AddLast(value);
                LinkedListNode<TValue> terminal = m_LinkedList.AddLast(default(TValue));
                // 将新区间存入字典
                m_Dictionary.Add(key, new GameLinkedListRange<TValue>(first, terminal));
            }
        }

        /// <summary>
        /// 从指定主键中移除【第一个匹配】的值
        /// </summary>
        /// <param name="key">目标主键</param>
        /// <param name="value">要移除的值</param>
        /// <returns>移除成功返回true，未找到返回false</returns>
        public bool Remove(TKey key, TValue value)
        {
            GameLinkedListRange<TValue> range = default(GameLinkedListRange<TValue>);
            if (m_Dictionary.TryGetValue(key, out range))
            {
                // 遍历当前主键对应的链表区间
                for (LinkedListNode<TValue> current = range.First; current != null && current != range.Terminal; current = current.Next)
                {
                    // 值匹配，执行移除逻辑
                    if (current.Value.Equals(value))
                    {
                        // 如果移除的是区间首节点，需要更新区间起始位置
                        if (current == range.First)
                        {
                            LinkedListNode<TValue> next = current.Next;
                            // 移除后区间为空，删除结束节点并从字典移除键
                            if (next == range.Terminal)
                            {
                                m_LinkedList.Remove(next);
                                m_Dictionary.Remove(key);
                            }
                            else
                            {
                                // 更新区间首节点
                                m_Dictionary[key] = new GameLinkedListRange<TValue>(next, range.Terminal);
                            }
                        }

                        // 从全局链表中移除当前节点
                        m_LinkedList.Remove(current);
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 移除指定主键对应的【所有值】及主键本身
        /// </summary>
        /// <param name="key">要移除的主键</param>
        /// <returns>移除成功返回true，主键不存在返回false</returns>
        public bool RemoveAll(TKey key)
        {
            GameLinkedListRange<TValue> range = default(GameLinkedListRange<TValue>);
            if (m_Dictionary.TryGetValue(key, out range))
            {
                // 先从字典中移除主键
                m_Dictionary.Remove(key);

                // 遍历并删除该主键对应的所有链表节点
                LinkedListNode<TValue> current = range.First;
                while (current != null)
                {
                    LinkedListNode<TValue> next = current != range.Terminal ? current.Next : null;
                    m_LinkedList.Remove(current);
                    current = next;
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// 获取自定义枚举器，用于遍历字典中的键值对
        /// </summary>
        /// <returns>自定义枚举器实例</returns>
        public Enumerator GetEnumerator()
        {
            return new Enumerator(m_Dictionary);
        }

        /// <summary>
        /// 泛型接口枚举器实现
        /// </summary>
        /// <returns>枚举器</returns>
        IEnumerator<KeyValuePair<TKey, GameLinkedListRange<TValue>>> IEnumerable<KeyValuePair<TKey, GameLinkedListRange<TValue>>>.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// 非泛型接口枚举器实现
        /// </summary>
        /// <returns>枚举器</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// 多值字典专用枚举器结构体
        /// 封装字典枚举器，提供安全、高效的遍历能力
        /// </summary>
        public struct Enumerator : IEnumerator<KeyValuePair<TKey, GameLinkedListRange<TValue>>>, IEnumerator
        {
            /// <summary>
            /// 内部封装的字典原生枚举器
            /// </summary>
            private Dictionary<TKey, GameLinkedListRange<TValue>>.Enumerator m_Enumerator;

            /// <summary>
            /// 内部构造函数，由字典自身创建枚举器
            /// </summary>
            /// <param name="dictionary">要遍历的源字典</param>
            internal Enumerator(Dictionary<TKey, GameLinkedListRange<TValue>> dictionary)
            {
                if (dictionary == null)
                {
                    throw new GameException("Dictionary 无效。");
                }

                m_Enumerator = dictionary.GetEnumerator();
            }

            /// <summary>
            /// 获取当前遍历到的键值对（泛型版本）
            /// </summary>
            public KeyValuePair<TKey, GameLinkedListRange<TValue>> Current
            {
                get
                {
                    return m_Enumerator.Current;
                }
            }

            /// <summary>
            /// 获取当前遍历到的键值对（非泛型接口版本）
            /// </summary>
            object IEnumerator.Current
            {
                get
                {
                    return m_Enumerator.Current;
                }
            }

            /// <summary>
            /// 释放枚举器资源
            /// </summary>
            public void Dispose()
            {
                m_Enumerator.Dispose();
            }

            /// <summary>
            /// 移动到下一个元素
            /// </summary>
            /// <returns>存在下一个元素返回true，遍历结束返回false</returns>
            public bool MoveNext()
            {
                return m_Enumerator.MoveNext();
            }

            /// <summary>
            /// 重置枚举器到初始位置
            /// </summary>
            void IEnumerator.Reset()
            {
                ((IEnumerator<KeyValuePair<TKey, GameLinkedListRange<TValue>>>)m_Enumerator).Reset();
            }
        }
    }
}