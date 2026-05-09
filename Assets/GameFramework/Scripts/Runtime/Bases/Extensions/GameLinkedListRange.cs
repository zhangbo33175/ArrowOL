using System;
using System.Collections;
using System.Collections.Generic;

namespace Honor.Runtime
{
    /// <summary>
    /// 游戏框架链表范围
    /// 表示链表中从 First 到 Terminal（不包含）的一段区间
    /// </summary>
    /// <typeparam name="T">指定链表范围的元素类型</typeparam>
    public class GameLinkedListRange<T> : IEnumerable<T>, IEnumerable
    {
        private readonly LinkedListNode<T> m_First;
        private readonly LinkedListNode<T> m_Terminal;

        /// <summary>
        /// 初始化游戏框架链表范围的新实例
        /// </summary>
        /// <param name="first">链表范围的开始结点</param>
        /// <param name="terminal">链表范围的终结标记结点（不包含）</param>
        public GameLinkedListRange(LinkedListNode<T> first, LinkedListNode<T> terminal)
        {
            if (first == null || terminal == null || first == terminal)
            {
                throw new GameException("链表范围无效，节点不能为空且不能是同一个节点。");
            }

            m_First = first;
            m_Terminal = terminal;
        }

        /// <summary>
        /// 获取链表范围是否有效
        /// </summary>
        public bool IsValid => m_First != null && m_Terminal != null && m_First != m_Terminal;

        /// <summary>
        /// 获取链表范围的开始结点
        /// </summary>
        public LinkedListNode<T> First => m_First;

        /// <summary>
        /// 获取链表范围的终结标记结点（不包含）
        /// </summary>
        public LinkedListNode<T> Terminal => m_Terminal;

        /// <summary>
        /// 获取链表范围的结点数量
        /// </summary>
        public int Count
        {
            get
            {
                if (!IsValid)
                    return 0;

                int count = 0;
                for (LinkedListNode<T> current = m_First;
                     current != null && current != m_Terminal;
                     current = current.Next)
                {
                    count++;
                }

                return count;
            }
        }

        /// <summary>
        /// 检查是否包含指定值
        /// </summary>
        /// <param name="value">要检查的值</param>
        /// <returns>是否包含指定值</returns>
        public bool Contains(T value)
        {
            if (!IsValid)
                return false;

            for (LinkedListNode<T> current = m_First; current != null && current != m_Terminal; current = current.Next)
            {
                if (current.Value == null && value == null)
                    return true;

                if (current.Value != null && current.Value.Equals(value))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 返回循环访问集合的枚举数
        /// </summary>
        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// 循环访问集合的枚举数（无GC）
        /// </summary>
        public struct Enumerator : IEnumerator<T>, IEnumerator
        {
            private readonly GameLinkedListRange<T> m_Range;
            private LinkedListNode<T> m_Current;
            private T m_CurrentValue;

            internal Enumerator(GameLinkedListRange<T> range)
            {
                if (range == null || !range.IsValid)
                    throw new GameException("链表范围无效，无法创建枚举器。");

                m_Range = range;
                m_Current = m_Range.m_First;
                m_CurrentValue = default;
            }

            /// <summary>
            /// 获取当前结点
            /// </summary>
            public T Current => m_CurrentValue;

            /// <summary>
            /// 获取当前的枚举数
            /// </summary>
            object IEnumerator.Current => m_CurrentValue;

            /// <summary>
            /// 清理枚举数
            /// </summary>
            public void Dispose()
            {
            }

            /// <summary>
            /// 获取下一个结点
            /// </summary>
            public bool MoveNext()
            {
                if (m_Current == null || m_Current == m_Range.Terminal)
                {
                    m_CurrentValue = default;
                    return false;
                }

                m_CurrentValue = m_Current.Value;
                m_Current = m_Current.Next;
                return true;
            }

            /// <summary>
            /// 重置枚举数
            /// </summary>
            void IEnumerator.Reset()
            {
                m_Current = m_Range.First;
                m_CurrentValue = default;
            }
        }
    }
}