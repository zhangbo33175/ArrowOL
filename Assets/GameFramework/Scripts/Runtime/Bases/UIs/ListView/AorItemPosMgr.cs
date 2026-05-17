/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  AorItemPosMgr.cs
 * author:    云毅
 * created:   2026
 * descrip:   高性能滚动列表项位置&尺寸管理器，支持动态尺寸、分块计算、二分查找
 ***************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Honor.Runtime
{
    //=========================================================================
    // 列表项尺寸分组
    //=========================================================================
    /// <summary>
    /// 项尺寸分组
    /// 负责管理一组列表项的尺寸、起始位置，提供尺寸修改、位置计算、索引查找等功能
    /// 用于滚动列表/网格布局的分块尺寸管理，提升大数据量列表性能
    /// </summary>
    public class ItemSizeGroup
    {
        #region 公共字段
        /// <summary>
        /// 组内所有项的尺寸数组
        /// </summary>
        public float[] mItemSizeArray = null;

        /// <summary>
        /// 组内所有项的起始位置数组（相对组内坐标）
        /// </summary>
        public float[] mItemStartPosArray = null;

        /// <summary>
        /// 当前组有效项数量
        /// </summary>
        public int mItemCount = 0;

        /// <summary>
        /// 当前组总尺寸（所有有效项尺寸总和）
        /// </summary>
        public float mGroupSize = 0;

        /// <summary>
        /// 组在整体列表中的起始位置（绝对坐标）
        /// </summary>
        public float mGroupStartPos = 0;

        /// <summary>
        /// 组在整体列表中的结束位置（绝对坐标）
        /// </summary>
        public float mGroupEndPos = 0;

        /// <summary>
        /// 组索引
        /// </summary>
        public int mGroupIndex = 0;
        #endregion

        #region 私有字段
        /// <summary>
        /// 脏数据起始索引（从此索引开始需要重新计算位置）
        /// </summary>
        private int mDirtyBeginIndex = AorItemPosMgr.mItemMaxCountPerGroup;

        /// <summary>
        /// 项默认尺寸
        /// </summary>
        private float mItemDefaultSize = 0;

        /// <summary>
        /// 最大非零尺寸项索引（优化二分查找性能）
        /// </summary>
        private int mMaxNoZeroIndex = 0;
        #endregion

        #region 构造与初始化
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="index">组索引</param>
        /// <param name="itemDefaultSize">项默认尺寸</param>
        public ItemSizeGroup(int index, float itemDefaultSize)
        {
            mGroupIndex = index;
            mItemDefaultSize = itemDefaultSize;
            Init();
        }

        /// <summary>
        /// 初始化数组与默认数据
        /// </summary>
        public void Init()
        {
            // 初始化项尺寸数组，设置默认尺寸
            mItemSizeArray = new float[AorItemPosMgr.mItemMaxCountPerGroup];
            if (!Mathf.Approximately(mItemDefaultSize, 0))
            {
                for (int i = 0; i < mItemSizeArray.Length; ++i)
                {
                    mItemSizeArray[i] = mItemDefaultSize;
                }
            }

            // 初始化项起始位置数组
            mItemStartPosArray = new float[AorItemPosMgr.mItemMaxCountPerGroup];
            mItemStartPosArray[0] = 0;

            // 默认项数量 = 每组最大项数
            mItemCount = AorItemPosMgr.mItemMaxCountPerGroup;
            // 计算组默认总尺寸
            mGroupSize = mItemDefaultSize * mItemSizeArray.Length;

            // 设置脏数据起始索引
            if (!Mathf.Approximately(mItemDefaultSize, 0))
            {
                mDirtyBeginIndex = 0;
            }
            else
            {
                mDirtyBeginIndex = AorItemPosMgr.mItemMaxCountPerGroup;
            }
        }
        #endregion

        #region 属性
        /// <summary>
        /// 是否存在脏数据（需要重新计算位置）
        /// </summary>
        public bool IsDirty
        {
            get
            {
                return mDirtyBeginIndex < mItemCount;
            }
        }
        #endregion

        #region 尺寸与位置计算
        /// <summary>
        /// 获取项在整体列表中的绝对起始位置
        /// </summary>
        /// <param name="index">组内项索引</param>
        /// <returns>绝对位置</returns>
        public float GetItemStartPos(int index)
        {
            return mGroupStartPos + mItemStartPosArray[index];
        }

        /// <summary>
        /// 设置组内指定项的尺寸
        /// </summary>
        /// <param name="index">组内项索引</param>
        /// <param name="size">目标尺寸</param>
        /// <returns>尺寸变化值（新尺寸 - 旧尺寸）</returns>
        public float SetItemSize(int index, float size)
        {
            // 更新最大非零尺寸索引
            if (index > mMaxNoZeroIndex && !Mathf.Approximately(size, 0))
            {
                mMaxNoZeroIndex = index;
            }

            float oldSize = mItemSizeArray[index];
            // 尺寸未变化，直接返回
            if (Mathf.Approximately(oldSize, size))
            {
                return 0;
            }

            // 更新尺寸并标记脏数据起始位置
            mItemSizeArray[index] = size;
            if (index < mDirtyBeginIndex)
            {
                mDirtyBeginIndex = index;
            }

            // 更新组总尺寸
            float deltaSize = size - oldSize;
            mGroupSize += deltaSize;
            return deltaSize;
        }

        /// <summary>
        /// 设置当前组有效项数量
        /// </summary>
        /// <param name="count">有效项数</param>
        public void SetItemCount(int count)
        {
            // 修正最大非零索引
            if (count < mMaxNoZeroIndex)
            {
                mMaxNoZeroIndex = count;
            }

            // 数量未变化，直接返回
            if (mItemCount == count)
            {
                return;
            }

            mItemCount = count;
            // 重新计算组总尺寸
            RecalcGroupSize();
        }

        /// <summary>
        /// 重新计算组总尺寸（累加所有有效项尺寸）
        /// </summary>
        public void RecalcGroupSize()
        {
            mGroupSize = 0;
            for (int i = 0; i < mItemCount; ++i)
            {
                mGroupSize += mItemSizeArray[i];
            }
        }

        /// <summary>
        /// 更新所有脏数据项的起始位置
        /// </summary>
        public void UpdateAllItemStartPos()
        {
            // 无脏数据，直接返回
            if (mDirtyBeginIndex >= mItemCount)
            {
                return;
            }

            // 从脏数据起始索引开始计算
            int startIndex = Mathf.Max(mDirtyBeginIndex, 1);
            for (int i = startIndex; i < mItemCount; ++i)
            {
                mItemStartPosArray[i] = mItemStartPosArray[i - 1] + mItemSizeArray[i - 1];
            }

            // 清除脏数据标记
            mDirtyBeginIndex = mItemCount;
        }

        /// <summary>
        /// 清除超出有效项范围的旧数据（置为0）
        /// </summary>
        public void ClearOldData()
        {
            for (int i = mItemCount; i < AorItemPosMgr.mItemMaxCountPerGroup; ++i)
            {
                mItemSizeArray[i] = 0;
            }
        }
        #endregion

        #region 位置查找
        /// <summary>
        /// 根据相对位置查找组内项索引（二分查找优化）
        /// </summary>
        /// <param name="pos">组内相对位置</param>
        /// <returns>组内项索引，未找到返回-1</returns>
        public int GetItemIndexByPos(float pos)
        {
            if (mItemCount == 0)
            {
                return -1;
            }

            int low = 0;
            int high = mItemCount - 1;

            // 动态尺寸优化：仅搜索到最大非零项即可
            if (Mathf.Approximately(mItemDefaultSize, 0f))
            {
                mMaxNoZeroIndex = Mathf.Max(mMaxNoZeroIndex, 0);
                high = mMaxNoZeroIndex;
            }

            // 二分查找定位目标项
            while (low <= high)
            {
                int mid = (low + high) / 2;
                float startPos = mItemStartPosArray[mid];
                float endPos = startPos + mItemSizeArray[mid];

                if (startPos <= pos && endPos >= pos)
                {
                    return mid;
                }
                else if (pos > endPos)
                {
                    low = mid + 1;
                }
                else
                {
                    high = mid - 1;
                }
            }

            return -1;
        }
        #endregion
    }

    //=========================================================================
    // 列表项位置管理器
    //=========================================================================
    /// <summary>
    /// 列表项位置管理器
    /// 核心管理类：分块管理大量列表项的尺寸、位置，提供高效的位置查询与索引查找
    /// 用于高性能滚动列表、网格列表的视图位置计算，支持动态尺寸项
    /// </summary>
    public class AorItemPosMgr
    {
        #region 常量
        /// <summary>
        /// 每个分组最大项数量（分块优化性能）
        /// </summary>
        public const int mItemMaxCountPerGroup = 100;
        #endregion

        #region 公共字段
        /// <summary>
        /// 所有项总尺寸（列表总滚动长度）
        /// </summary>
        public float mTotalSize = 0;

        /// <summary>
        /// 项默认尺寸
        /// </summary>
        public float mItemDefaultSize = 20;
        #endregion

        #region 私有字段
        /// <summary>
        /// 项尺寸分组列表
        /// </summary>
        private readonly List<ItemSizeGroup> mItemSizeGroupList = new List<ItemSizeGroup>();

        /// <summary>
        /// 脏数据起始组索引（从此组开始需要更新位置）
        /// </summary>
        private int mDirtyBeginIndex = int.MaxValue;

        /// <summary>
        /// 最大非空组索引（优化二分查找）
        /// </summary>
        private int mMaxNotEmptyGroupIndex = 0;
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="itemDefaultSize">项默认尺寸</param>
        public AorItemPosMgr(float itemDefaultSize)
        {
            mItemDefaultSize = itemDefaultSize;
        }
        #endregion

        #region 列表配置
        /// <summary>
        /// 设置列表最大项数量（初始化/重置分组）
        /// </summary>
        /// <param name="maxCount">最大项数</param>
        public void SetItemMaxCount(int maxCount)
        {
            mDirtyBeginIndex = 0;
            mTotalSize = 0;

            // 计算所需分组数量与最后一组有效项数
            int remainder = maxCount % mItemMaxCountPerGroup;
            int lastGroupItemCount = remainder == 0 ? mItemMaxCountPerGroup : remainder;
            int needGroupCount = maxCount / mItemMaxCountPerGroup + (remainder > 0 ? 1 : 0);

            int currentGroupCount = mItemSizeGroupList.Count;

            // 调整分组数量：删除多余分组
            if (currentGroupCount > needGroupCount)
            {
                int removeCount = currentGroupCount - needGroupCount;
                mItemSizeGroupList.RemoveRange(needGroupCount, removeCount);
            }
            // 添加缺失分组
            else if (currentGroupCount < needGroupCount)
            {
                // 清空最后一个旧分组的多余数据
                if (currentGroupCount > 0)
                {
                    mItemSizeGroupList[currentGroupCount - 1].ClearOldData();
                }

                int addCount = needGroupCount - currentGroupCount;
                for (int i = 0; i < addCount; ++i)
                {
                    ItemSizeGroup newGroup = new ItemSizeGroup(currentGroupCount + i, mItemDefaultSize);
                    mItemSizeGroupList.Add(newGroup);
                }
            }
            // 分组数量不变，清空最后一组多余数据
            else
            {
                if (currentGroupCount > 0)
                {
                    mItemSizeGroupList[currentGroupCount - 1].ClearOldData();
                }
            }

            // 修正最大非空组索引
            currentGroupCount = mItemSizeGroupList.Count;
            mMaxNotEmptyGroupIndex = Mathf.Clamp(mMaxNotEmptyGroupIndex, 0, currentGroupCount - 1);

            if (currentGroupCount == 0)
            {
                return;
            }

            // 设置每组有效项数量
            for (int i = 0; i < currentGroupCount - 1; ++i)
            {
                mItemSizeGroupList[i].SetItemCount(mItemMaxCountPerGroup);
            }
            mItemSizeGroupList[currentGroupCount - 1].SetItemCount(lastGroupItemCount);

            // 计算总尺寸
            for (int i = 0; i < currentGroupCount; ++i)
            {
                mTotalSize += mItemSizeGroupList[i].mGroupSize;
            }
        }
        #endregion

        #region 项尺寸设置
        /// <summary>
        /// 设置指定项的尺寸
        /// </summary>
        /// <param name="itemIndex">全局项索引</param>
        /// <param name="size">目标尺寸</param>
        public void SetItemSize(int itemIndex, float size)
        {
            // 计算所属分组与组内索引
            int groupIndex = itemIndex / mItemMaxCountPerGroup;
            int indexInGroup = itemIndex % mItemMaxCountPerGroup;

            ItemSizeGroup targetGroup = mItemSizeGroupList[groupIndex];
            float deltaSize = targetGroup.SetItemSize(indexInGroup, size);

            // 尺寸变化，标记脏数据组
            if (!Mathf.Approximately(deltaSize, 0))
            {
                mDirtyBeginIndex = Mathf.Min(mDirtyBeginIndex, groupIndex);
            }

            // 更新总尺寸
            mTotalSize += deltaSize;

            // 更新最大非空组索引
            if (groupIndex > mMaxNotEmptyGroupIndex && !Mathf.Approximately(size, 0))
            {
                mMaxNotEmptyGroupIndex = groupIndex;
            }
        }
        #endregion

        #region 位置查询
        /// <summary>
        /// 获取指定项的绝对起始位置
        /// </summary>
        /// <param name="itemIndex">全局项索引</param>
        /// <returns>绝对位置</returns>
        public float GetItemPos(int itemIndex)
        {
            // 先更新所有脏数据
            Update(true);

            int groupIndex = itemIndex / mItemMaxCountPerGroup;
            int indexInGroup = itemIndex % mItemMaxCountPerGroup;
            return mItemSizeGroupList[groupIndex].GetItemStartPos(indexInGroup);
        }

        /// <summary>
        /// 根据绝对位置查找对应的项索引与项起始位置
        /// </summary>
        /// <param name="pos">查找的绝对位置</param>
        /// <param name="itemIndex">查找到的项索引（输出）</param>
        /// <param name="itemStartPos">项起始位置（输出）</param>
        /// <returns>是否找到有效项</returns>
        public bool GetItemIndexAndPosAtGivenPos(float pos, ref int itemIndex, ref float itemStartPos)
        {
            // 先更新所有脏数据
            Update(true);

            itemIndex = 0;
            itemStartPos = 0f;

            int groupCount = mItemSizeGroupList.Count;
            if (groupCount == 0)
            {
                return true;
            }

            ItemSizeGroup hitGroup = null;
            int low = 0;
            int high = groupCount - 1;

            // 动态尺寸优化：仅搜索到最大非空组
            if (Mathf.Approximately(mItemDefaultSize, 0f))
            {
                mMaxNotEmptyGroupIndex = Mathf.Max(mMaxNotEmptyGroupIndex, 0);
                high = mMaxNotEmptyGroupIndex;
            }

            // 二分查找定位目标组
            while (low <= high)
            {
                int mid = (low + high) / 2;
                ItemSizeGroup midGroup = mItemSizeGroupList[mid];

                if (midGroup.mGroupStartPos <= pos && midGroup.mGroupEndPos >= pos)
                {
                    hitGroup = midGroup;
                    break;
                }
                else if (pos > midGroup.mGroupEndPos)
                {
                    low = mid + 1;
                }
                else
                {
                    high = mid - 1;
                }
            }

            // 未找到目标组
            if (hitGroup == null)
            {
                return false;
            }

            // 在组内查找项索引
            int indexInGroup = hitGroup.GetItemIndexByPos(pos - hitGroup.mGroupStartPos);
            if (indexInGroup < 0)
            {
                return false;
            }

            // 计算全局索引与绝对位置
            itemIndex = indexInGroup + hitGroup.mGroupIndex * mItemMaxCountPerGroup;
            itemStartPos = hitGroup.GetItemStartPos(indexInGroup);
            return true;
        }
        #endregion

        #region 脏数据更新
        /// <summary>
        /// 更新所有脏数据组的位置信息
        /// </summary>
        /// <param name="updateAll">是否更新全部脏数据组（false：仅更新1组提升性能）</param>
        public void Update(bool updateAll)
        {
            int groupCount = mItemSizeGroupList.Count;
            if (groupCount == 0 || mDirtyBeginIndex >= groupCount)
            {
                return;
            }

            int updateLoopCount = 0;
            for (int i = mDirtyBeginIndex; i < groupCount; ++i)
            {
                updateLoopCount++;
                ItemSizeGroup currentGroup = mItemSizeGroupList[i];

                // 更新组内项位置
                currentGroup.UpdateAllItemStartPos();
                mDirtyBeginIndex++;

                // 计算组在整体列表中的绝对位置
                if (i == 0)
                {
                    currentGroup.mGroupStartPos = 0;
                }
                else
                {
                    currentGroup.mGroupStartPos = mItemSizeGroupList[i - 1].mGroupEndPos;
                }
                currentGroup.mGroupEndPos = currentGroup.mGroupStartPos + currentGroup.mGroupSize;

                // 非全量更新：仅更新1组后返回，提升实时性能
                if (!updateAll && updateLoopCount > 1)
                {
                    break;
                }
            }
        }
        #endregion
    }
}