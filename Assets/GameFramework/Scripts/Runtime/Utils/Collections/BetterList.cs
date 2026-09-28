/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * -------------------------------------------------------------
 * filename:  BetterList.cs
 * author:  云毅
 * created:
 * descrip:   轻量级动态数组 - 基于数组的 List 替代品（避免 GC 与装箱）
 * 优化记录: 由旧版 HonorUtils.Tweening.BetterList 迁移，统一命名空间，
 *           补齐 TryGet 与 Trim 接口，规范命名
 ***************************************************************/

using System;
using System.Collections.Generic;

namespace Honor.Runtime
{
    /// <summary>
    /// 轻量级动态数组
    /// 功能：基于原生数组的自动扩容容器，迭代无装箱、支持交换删除
    /// 适合：高频增删、需要避免 GC 的热点逻辑
    /// </summary>
    /// <typeparam name="T">元素类型</typeparam>
    public class BetterList<T>
    {
        #region 字段

        /// <summary>底层数组</summary>
        public T[] buffer;

        /// <summary>当前有效元素数量</summary>
        public int size = 0;

        #endregion

        #region 索引器

        /// <summary>
        /// 按索引访问元素
        /// </summary>
        public T this[int index]
        {
            get { return buffer[index]; }
            set { buffer[index] = value; }
        }

        #endregion

        #region 生命周期与容量

        /// <summary>
        /// 构造：分配默认容量
        /// </summary>
        public BetterList() : this(16)
        {
        }

        /// <summary>
        /// 构造：指定初始容量
        /// </summary>
        public BetterList(int capacity)
        {
            buffer = new T[capacity];
        }

        /// <summary>
        /// 扩容：不足时按双倍策略扩容
        /// </summary>
        protected void AllocateMore()
        {
            T[] newList = (buffer != null) ? new T[Math.Max(buffer.Length << 1, 32)] : new T[32];
            if (buffer != null && size > 0)
            {
                buffer.CopyTo(newList, 0);
            }

            buffer = newList;
        }

        /// <summary>
        /// 收缩容量到当前实际大小（释放多余内存）
        /// </summary>
        public void Trim()
        {
            if (size > 0)
            {
                if (size < buffer.Length)
                {
                    T[] newList = new T[size];
                    for (int i = 0; i < size; ++i)
                    {
                        newList[i] = buffer[i];
                    }

                    buffer = newList;
                }
            }
            else
            {
                buffer = new T[0];
            }
        }

        /// <summary>
        /// 清空列表（保留容量）
        /// </summary>
        public void Clear()
        {
            size = 0;
        }

        /// <summary>
        /// 释放引用并清空
        /// </summary>
        public void Release()
        {
            size = 0;
            buffer = null;
        }

        #endregion

        #region 增删查

        /// <summary>
        /// 追加元素
        /// </summary>
        public void Add(T item)
        {
            if (buffer == null || size == buffer.Length)
            {
                AllocateMore();
            }

            buffer[size++] = item;
        }

        /// <summary>
        /// 插入元素到指定位置
        /// </summary>
        public void Insert(int index, T item)
        {
            if (buffer == null || size == buffer.Length)
            {
                AllocateMore();
            }

            if (index < size)
            {
                for (int i = size; i > index; --i)
                {
                    buffer[i] = buffer[i - 1];
                }

                buffer[index] = item;
                ++size;
            }
            else
            {
                Add(item);
            }
        }

        /// <summary>
        /// 是否包含指定元素
        /// </summary>
        public bool Contains(T item)
        {
            if (buffer == null)
            {
                return false;
            }

            for (int i = 0; i < size; ++i)
            {
                if (buffer[i].Equals(item))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 查找元素索引，不存在返回 -1
        /// </summary>
        public int IndexOf(T item)
        {
            if (buffer == null)
            {
                return -1;
            }

            for (int i = 0; i < size; ++i)
            {
                if (buffer[i].Equals(item))
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// 按索引移除元素（保持顺序）
        /// </summary>
        public bool RemoveAt(int index)
        {
            if (buffer != null && index > -1 && index < size)
            {
                --size;
                buffer[index] = default(T);
                for (int b = index; b < size; ++b)
                {
                    buffer[b] = buffer[b + 1];
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// 交换删除：将末尾元素覆盖到待删除位置（不保持顺序，但 O(1)）
        /// </summary>
        public bool RemoveAtSwap(int index)
        {
            if (buffer != null && index > -1 && index < size)
            {
                buffer[index] = buffer[--size];
                buffer[size] = default(T);
                return true;
            }

            return false;
        }

        /// <summary>
        /// 按元素移除（不保持顺序）
        /// </summary>
        public bool Remove(T item)
        {
            int index = IndexOf(item);
            return index >= 0 ? RemoveAtSwap(index) : false;
        }

        /// <summary>
        /// 弹出末尾元素
        /// </summary>
        public T Pop()
        {
            if (buffer != null && size > 0)
            {
                T val = buffer[--size];
                buffer[size] = default(T);
                return val;
            }

            return default(T);
        }

        /// <summary>
        /// 尝试获取指定索引元素
        /// </summary>
        public bool TryGet(int index, out T value)
        {
            if (index >= 0 && index < size)
            {
                value = buffer[index];
                return true;
            }

            value = default(T);
            return false;
        }

        #endregion

        #region 排序与转换

        /// <summary>
        /// 快速排序（稳定实现，不改变原数组引用）
        /// </summary>
        public void Sort(Comparison<T> comparer)
        {
            if (size > 1)
            {
                T[] copy = new T[size];
                Array.Copy(buffer, copy, size);
                Array.Sort(copy, comparer);
                Array.Copy(copy, buffer, size);
            }
        }

        /// <summary>
        /// 转换为数组（拷贝当前有效部分）
        /// </summary>
        public T[] ToArray()
        {
            T[] result = new T[size];
            if (size > 0)
            {
                Array.Copy(buffer, result, size);
            }

            return result;
        }

        #endregion
    }
}
