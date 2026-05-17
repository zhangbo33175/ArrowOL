/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  ItemPool.cs
 * author:    云毅
 * created:   2026
 * descrip:   列表项对象池，负责AorListViewItem的创建、获取、回收、销毁，优化滚动列表性能
 ***************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Honor.Runtime
{
    //=========================================================================
    // 列表项对象池
    //=========================================================================
    /// <summary>
    /// 列表项对象池
    /// 负责列表项（AorListViewItem）的创建、获取、回收、销毁
    /// 用于优化滚动列表频繁创建/销毁UI对象带来的性能消耗
    /// </summary>
    public class ItemPool
    {
        #region 私有字段
        /// <summary>
        /// 项预制体
        /// </summary>
        private GameObject mPrefabObj;

        /// <summary>
        /// 预制体名称（用于标识）
        /// </summary>
        private string mPrefabName;

        /// <summary>
        /// 初始化时默认创建的项数量
        /// </summary>
        private int mInitCreateCount = 1;

        /// <summary>
        /// 项间距
        /// </summary>
        private float mPadding = 0;

        /// <summary>
        /// 项起始位置偏移
        /// </summary>
        private float mStartPosOffset = 0;

        /// <summary>
        /// 临时回收列表（等待帧结束统一回收）
        /// </summary>
        private readonly List<AorListViewItem> mTmpPooledItemList = new List<AorListViewItem>();

        /// <summary>
        /// 常驻对象池列表
        /// </summary>
        private readonly List<AorListViewItem> mPooledItemList = new List<AorListViewItem>();

        /// <summary>
        /// 全局唯一项ID自增计数器
        /// </summary>
        private static int mCurItemIdCount = 0;

        /// <summary>
        /// 项父节点（RectTransform）
        /// </summary>
        private RectTransform mItemParent = null;
        #endregion

        #region 构造函数
        public ItemPool()
        {
        }
        #endregion

        #region 初始化
        /// <summary>
        /// 初始化对象池
        /// </summary>
        /// <param name="prefabObj">列表项预制体</param>
        /// <param name="padding">项间距</param>
        /// <param name="startPosOffset">起始位置偏移</param>
        /// <param name="createCount">初始创建数量</param>
        /// <param name="parent">父节点</param>
        public void Init(GameObject prefabObj, float padding, float startPosOffset, int createCount, RectTransform parent)
        {
            mPrefabObj = prefabObj;
            mPrefabName = prefabObj.name;
            mInitCreateCount = createCount;
            mPadding = padding;
            mStartPosOffset = startPosOffset;
            mItemParent = parent;

            // 预制体默认隐藏
            mPrefabObj.SetActive(false);

            // 给预制体添加UI标记，跟随父节点销毁
            UIFlagBehaviour uiFlagBehaviour = mPrefabObj.GetOrAddComponent<UIFlagBehaviour>();
            if (uiFlagBehaviour != null)
            {
                uiFlagBehaviour.FollowParentDestroy = true;
            }

            // 预创建对象池项
            for (int i = 0; i < mInitCreateCount; ++i)
            {
                AorListViewItem item = CreateItem();
                RecycleItemReal(item);
            }
        }
        #endregion

        #region 获取与创建项
        /// <summary>
        /// 从对象池获取一个可用项
        /// 优先从临时回收池取，再从常驻池取，无可用对象则创建新项
        /// </summary>
        /// <returns>可用的列表项</returns>
        public AorListViewItem GetItem()
        {
            mCurItemIdCount++;
            AorListViewItem item = null;

            // 优先从临时回收列表获取（刚被回收的）
            if (mTmpPooledItemList.Count > 0)
            {
                int lastIndex = mTmpPooledItemList.Count - 1;
                item = mTmpPooledItemList[lastIndex];
                mTmpPooledItemList.RemoveAt(lastIndex);
                item.gameObject.SetActive(true);
            }
            // 从常驻对象池获取
            else
            {
                if (mPooledItemList.Count == 0)
                {
                    // 无可用对象，创建新项
                    item = CreateItem();
                }
                else
                {
                    int lastIndex = mPooledItemList.Count - 1;
                    item = mPooledItemList[lastIndex];
                    mPooledItemList.RemoveAt(lastIndex);
                    item.gameObject.SetActive(true);
                }
            }

            // 设置项基础参数
            item.Padding = mPadding;
            item.ItemId = mCurItemIdCount;
            return item;
        }

        /// <summary>
        /// 创建新的列表项
        /// </summary>
        /// <returns>新创建的列表项</returns>
        public AorListViewItem CreateItem()
        {
            // 实例化GameObject
            GameObject go = GameMainRoot.Asset.InstantiateGO(mItemParent, mPrefabObj);
            go.SetActive(true);

            // 重置Transform
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.identity;
            go.transform.localScale = Vector3.one;

            // 添加UI标记，跟随父节点销毁
            UIFlagBehaviour uiFlagBehaviour = go.GetOrAddComponent<UIFlagBehaviour>();
            if (uiFlagBehaviour != null)
            {
                uiFlagBehaviour.FollowParentDestroy = true;
            }

            // 重置RectTransform
            RectTransform rect = go.GetComponent<RectTransform>();
            if (rect != null)
            {
                rect.anchoredPosition3D = Vector3.zero;
                rect.localEulerAngles = Vector3.zero;
            }

            // 初始化列表项数据
            AorListViewItem listItem = go.GetComponent<AorListViewItem>();
            listItem.ItemPrefabName = mPrefabName;
            listItem.StartPosOffset = mStartPosOffset;

            return listItem;
        }
        #endregion

        #region 回收项
        /// <summary>
        /// 真正执行回收（隐藏并加入常驻池）
        /// </summary>
        /// <param name="item">待回收项</param>
        private void RecycleItemReal(AorListViewItem item)
        {
            if (item == null) return;
            
            item.gameObject.SetActive(false);
            mPooledItemList.Add(item);
        }

        /// <summary>
        /// 回收项到临时列表（延迟统一回收，避免频繁操作）
        /// </summary>
        /// <param name="item">待回收项</param>
        public void RecycleItem(AorListViewItem item)
        {
            if (item == null) return;
            
            mTmpPooledItemList.Add(item);
        }

        /// <summary>
        /// 清空临时回收列表，将所有项移入常驻对象池
        /// </summary>
        public void ClearTmpRecycledItem()
        {
            int count = mTmpPooledItemList.Count;
            if (count == 0) return;

            for (int i = 0; i < count; ++i)
            {
                RecycleItemReal(mTmpPooledItemList[i]);
            }

            mTmpPooledItemList.Clear();
        }
        #endregion

        #region 销毁管理
        /// <summary>
        /// 销毁池内所有项（清空对象池）
        /// </summary>
        public void DestroyAllItem()
        {
            // 先清空临时回收项
            ClearTmpRecycledItem();

            // 销毁常驻池所有对象
            int count = mPooledItemList.Count;
            for (int i = 0; i < count; ++i)
            {
                GameObject.DestroyImmediate(mPooledItemList[i].gameObject);
            }

            mPooledItemList.Clear();
        }
        #endregion
    }
}