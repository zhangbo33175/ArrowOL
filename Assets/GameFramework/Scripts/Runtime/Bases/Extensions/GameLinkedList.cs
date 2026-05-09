using System;
using System.Collections;
using System.Collections.Generic;

namespace Honor.Runtime
{
    /// <summary>
    /// 游戏框架链表类（带节点缓存，减少GC）
    /// </summary>
    /// <typeparam name="T">指定链表的元素类型</typeparam>
    public sealed class GameLinkedList<T> : ICollection<T>, IEnumerable<T>, ICollection, IEnumerable
    {
        private readonly LinkedList<T> m_LinkedList;
        private readonly Queue<LinkedListNode<T>> m_CachedNodes;

        /// <summary>
        /// 初始化游戏框架链表类的新实例
        /// </summary>
        public GameLinkedList()
        {
            m_LinkedList = new LinkedList<T>();
            m_CachedNodes = new Queue<LinkedListNode<T>>();
        }

        /// <summary>
        /// 获取链表中实际包含的结点数量
        /// </summary>
        public int Count => m_LinkedList.Count;

        /// <summary>
        /// 获取链表结点缓存数量
        /// </summary>
        public int CachedNodeCount => m_CachedNodes.Count;

        /// <summary>
        /// 获取链表的第一个结点
        /// </summary>
        public LinkedListNode<T> First => m_LinkedList.First;

        /// <summary>
        /// 获取链表的最后一个结点
        /// </summary>
        public LinkedListNode<T> Last => m_LinkedList.Last;

        /// <summary>
        /// 获取一个值，该值指示 ICollection`1 是否为只读
        /// </summary>
        public bool IsReadOnly => ((ICollection<T>)m_LinkedList).IsReadOnly;

        /// <summary>
        /// 获取可用于同步对 ICollection 的访问的对象
        /// </summary>
        public object SyncRoot => ((ICollection)m_LinkedList).SyncRoot;

        /// <summary>
        /// 获取一个值，该值指示是否同步对 ICollection 的访问（线程安全）
        /// </summary>
        public bool IsSynchronized => ((ICollection)m_LinkedList).IsSynchronized;

        /// <summary>
        /// 将值添加到 ICollection`1 的结尾处
        /// </summary>
        public void Add(T value)
        {
            AddLast(value);
        }

        /// <summary>
        /// 在链表中指定的现有结点后添加包含指定值的新结点
        /// </summary>
        public LinkedListNode<T> AddAfter(LinkedListNode<T> node, T value)
        {
            LinkedListNode<T> newNode = AcquireNode(value);
            m_LinkedList.AddAfter(node, newNode);
            return newNode;
        }

        /// <summary>
        /// 在链表中指定的现有结点后添加指定的新结点
        /// </summary>
        public void AddAfter(LinkedListNode<T> node, LinkedListNode<T> newNode)
        {
            m_LinkedList.AddAfter(node, newNode);
        }

        /// <summary>
        /// 在链表中指定的现有结点前添加包含指定值的新结点
        /// </summary>
        public LinkedListNode<T> AddBefore(LinkedListNode<T> node, T value)
        {
            LinkedListNode<T> newNode = AcquireNode(value);
            m_LinkedList.AddBefore(node, newNode);
            return newNode;
        }

        /// <summary>
        /// 在链表中指定的现有结点前添加指定的新结点
        /// </summary>
        public void AddBefore(LinkedListNode<T> node, LinkedListNode<T> newNode)
        {
            m_LinkedList.AddBefore(node, newNode);
        }

        /// <summary>
        /// 在链表的开头处添加包含指定值的新结点
        /// </summary>
        public LinkedListNode<T> AddFirst(T value)
        {
            LinkedListNode<T> node = AcquireNode(value);
            m_LinkedList.AddFirst(node);
            return node;
        }

        /// <summary>
        /// 在链表的开头处添加指定的新结点
        /// </summary>
        public void AddFirst(LinkedListNode<T> node)
        {
            m_LinkedList.AddFirst(node);
        }

        /// <summary>
        /// 在链表的结尾处添加包含指定值的新结点
        /// </summary>
        public LinkedListNode<T> AddLast(T value)
        {
            LinkedListNode<T> node = AcquireNode(value);
            m_LinkedList.AddLast(node);
            return node;
        }

        /// <summary>
        /// 在链表的结尾处添加指定的新结点
        /// </summary>
        public void AddLast(LinkedListNode<T> node)
        {
            m_LinkedList.AddLast(node);
        }

        /// <summary>
        /// 从链表中移除所有结点
        /// </summary>
        public void Clear()
        {
            LinkedListNode<T> current = m_LinkedList.First;
            while (current != null)
            {
                LinkedListNode<T> temp = current;
                current = current.Next;
                ReleaseNode(temp);
            }

            m_LinkedList.Clear();
        }

        /// <summary>
        /// 清除链表结点缓存
        /// </summary>
        public void ClearCachedNodes()
        {
            m_CachedNodes.Clear();
        }

        /// <summary>
        /// 确定某值是否在链表中
        /// </summary>
        public bool Contains(T value)
        {
            return m_LinkedList.Contains(value);
        }

        /// <summary>
        /// 从目标数组的指定索引处开始将整个链表复制到兼容的一维数组
        /// </summary>
        public void CopyTo(T[] array, int index)
        {
            m_LinkedList.CopyTo(array, index);
        }

        /// <summary>
        /// 从特定的 ICollection 索引开始，将数组的元素复制到一个数组中
        /// </summary>
        public void CopyTo(Array array, int index)
        {
            ((ICollection)m_LinkedList).CopyTo(array, index);
        }

        /// <summary>
        /// 查找包含指定值的第一个结点
        /// </summary>
        public LinkedListNode<T> Find(T value)
        {
            return m_LinkedList.Find(value);
        }

        /// <summary>
        /// 查找包含指定值的最后一个结点
        /// </summary>
        public LinkedListNode<T> FindLast(T value)
        {
            return m_LinkedList.FindLast(value);
        }

        /// <summary>
        /// 从链表中移除指定值的第一个匹配项
        /// </summary>
        public bool Remove(T value)
        {
            LinkedListNode<T> node = m_LinkedList.Find(value);
            if (node != null)
            {
                m_LinkedList.Remove(node);
                ReleaseNode(node);
                return true;
            }

            return false;
        }

        /// <summary>
        /// 从链表中移除指定的结点
        /// </summary>
        public void Remove(LinkedListNode<T> node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));
            
            m_LinkedList.Remove(node);
            ReleaseNode(node);
        }

        /// <summary>
        /// 移除位于链表开头处的结点
        /// </summary>
        public void RemoveFirst()
        {
            LinkedListNode<T> first = m_LinkedList.First;
            if (first == null)
            {
                throw new GameException("开头节点无效，链表为空。");
            }

            m_LinkedList.RemoveFirst();
            ReleaseNode(first);
        }

        /// <summary>
        /// 移除位于链表结尾处的结点
        /// </summary>
        public void RemoveLast()
        {
            LinkedListNode<T> last = m_LinkedList.Last;
            if (last == null)
            {
                throw new GameException("尾部节点无效，链表为空。");
            }

            m_LinkedList.RemoveLast();
            ReleaseNode(last);
        }

        /// <summary>
        /// 返回循环访问集合的枚举数
        /// </summary>
        public Enumerator GetEnumerator()
        {
            return new Enumerator(m_LinkedList);
        }

        private LinkedListNode<T> AcquireNode(T value)
        {
            LinkedListNode<T> node;
            if (m_CachedNodes.Count > 0)
            {
                node = m_CachedNodes.Dequeue();
                node.Value = value;
            }
            else
            {
                node = new LinkedListNode<T>(value);
            }

            return node;
        }

        private void ReleaseNode(LinkedListNode<T> node)
        {
            if (node == null) return;
            
            node.Value = default;
            m_CachedNodes.Enqueue(node);
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
        /// 循环访问集合的枚举数
        /// </summary>
        public struct Enumerator : IEnumerator<T>, IEnumerator
        {
            private LinkedList<T>.Enumerator m_Enumerator;

            internal Enumerator(LinkedList<T> linkedList)
            {
                if (linkedList == null)
                {
                    throw new GameException("链表无效。");
                }

                m_Enumerator = linkedList.GetEnumerator();
            }

            /// <summary>
            /// 获取当前结点
            /// </summary>
            public T Current => m_Enumerator.Current;

            /// <summary>
            /// 获取当前的枚举数
            /// </summary>
            object IEnumerator.Current => m_Enumerator.Current;

            /// <summary>
            /// 清理枚举数
            /// </summary>
            public void Dispose()
            {
                m_Enumerator.Dispose();
            }

            /// <summary>
            /// 获取下一个结点
            /// </summary>
            public bool MoveNext()
            {
                return m_Enumerator.MoveNext();
            }

            /// <summary>
            /// 重置枚举数
            /// </summary>
            void IEnumerator.Reset()
            {
                ((IEnumerator)m_Enumerator).Reset();
            }
        }
    }
}