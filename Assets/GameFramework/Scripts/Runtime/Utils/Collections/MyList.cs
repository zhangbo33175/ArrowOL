/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * -------------------------------------------------------------
 * filename:  MyList.cs
 * author:  云毅
 * created:
 * descrip:   谓词查找增强列表 - 基于 BetterList 提供 Find/FindAll/RemoveAll 等 LINQ 式能力
 * 优化记录: 由旧版 HonorUtils.MyList 迁移，统一命名空间，补齐交换删除重载
 ***************************************************************/

using System;

namespace Honor.Runtime
{
    /// <summary>
    /// 谓词查找增强列表
    /// 功能：在 BetterList 基础上提供 Predicate 查找、批量移除、区间拷贝等高频操作
    /// </summary>
    /// <typeparam name="T">元素类型</typeparam>
    public class MyList<T> : BetterList<T>
    {
        #region 查找

        /// <summary>
        /// 是否存在满足条件的元素
        /// </summary>
        public bool Exists(Predicate<T> match)
        {
            for (int i = 0; i < size; i++)
            {
                if (match(buffer[i]))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 查找第一个满足条件的元素
        /// </summary>
        public T Find(Predicate<T> match)
        {
            for (int i = 0; i < size; i++)
            {
                if (match(buffer[i]))
                {
                    return buffer[i];
                }
            }

            return default(T);
        }

        /// <summary>
        /// 从末尾向前查找第一个满足条件的索引，不存在返回 -1
        /// </summary>
        public int FindLastIndex(Predicate<T> match)
        {
            for (int i = size - 1; i >= 0; i--)
            {
                if (match(buffer[i]))
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// 查找全部满足条件的元素组成新列表
        /// </summary>
        public MyList<T> FindAll(Predicate<T> match)
        {
            MyList<T> result = new MyList<T>();
            for (int i = 0; i < size; i++)
            {
                if (match(buffer[i]))
                {
                    result.Add(buffer[i]);
                }
            }

            return result;
        }

        /// <summary>
        /// 移除全部满足条件的元素（从后往前遍历，保持顺序稳定）
        /// </summary>
        public MyList<T> RemoveAll(Predicate<T> match)
        {
            for (int i = size - 1; i >= 0; i--)
            {
                if (match(buffer[i]))
                {
                    RemoveAt(i);
                }
            }

            return this;
        }

        /// <summary>
        /// 空值安全的元素索引查找
        /// </summary>
        public int IndexOf2(T item)
        {
            if (buffer == null)
            {
                return -1;
            }

            for (int i = 0; i < size; ++i)
            {
                if (buffer[i] == null)
                {
                    continue;
                }

                if (buffer[i].Equals(item))
                {
                    return i;
                }
            }

            return -1;
        }

        #endregion

        #region 区间与批量

        /// <summary>
        /// 追加另一个 MyList 的全部元素
        /// </summary>
        public MyList<T> AddRange(MyList<T> list)
        {
            for (int i = 0; i < list.size; i++)
            {
                Add(list[i]);
            }

            return this;
        }

        /// <summary>
        /// 拷贝指定区间组成新列表
        /// </summary>
        public MyList<T> GetRange(int index, int count)
        {
            MyList<T> result = new MyList<T>();
            for (int i = index; i < size; i++)
            {
                if (count <= 0)
                {
                    break;
                }

                result.Add(buffer[i]);
                count--;
            }

            return result;
        }

        /// <summary>
        /// 交换删除指定元素（O(1)）
        /// </summary>
        public bool RemoveAt1(T item)
        {
            int index = IndexOf(item);
            return index >= 0 ? RemoveAt1(index) : false;
        }

        /// <summary>
        /// 交换删除指定索引元素（O(1)）
        /// </summary>
        public bool RemoveAt1(int index)
        {
            return RemoveAtSwap(index);
        }

        #endregion
    }
}
