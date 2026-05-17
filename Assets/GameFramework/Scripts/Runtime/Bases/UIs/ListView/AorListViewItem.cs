/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  AorListViewItem.cs
 * author:    云毅
 * created:   2026
 * descrip:   滚动列表项基类，所有 AorListView 子项必须继承
 ***************************************************************/

using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// 滚动列表项基类
    /// 所有 AorListView 列表的子项必须继承此类，提供通用属性、边界计算、数据存储
    /// </summary>
    public class AorListViewItem : MonoBehaviour
    {
        //=========================================================================
        // 字段成员
        //=========================================================================
        #region Field - Index & ID
        /// <summary>
        /// 列表项在列表中的索引
        /// </summary>
        private int mItemIndex = -1;

        /// <summary>
        /// 列表项唯一ID
        /// </summary>
        private int mItemId = -1;
        #endregion

        #region Field - Parent & State
        /// <summary>
        /// 归属的父级滚动列表
        /// </summary>
        private AorListView mParentAorListView;

        /// <summary>
        /// 初始化回调是否已执行
        /// </summary>
        private bool mIsInitHandlerCalled;
        #endregion

        #region Field - Prefab & Component
        /// <summary>
        /// 列表项预制体名称
        /// </summary>
        private string mItemPrefabName;

        /// <summary>
        /// 缓存的 RectTransform
        /// </summary>
        private RectTransform mCachedRectTransform;
        #endregion

        #region Field - Layout & Spacing
        /// <summary>
        /// 项间距
        /// </summary>
        private float mPadding;

        /// <summary>
        /// 起始位置偏移
        /// </summary>
        private float mStartOffset;
        #endregion

        #region Field - Snap & Calculate
        /// <summary>
        /// 与视口对齐中心的距离
        /// </summary>
        private float mDistanceWithViewPortSnapCenter;

        /// <summary>
        /// 项创建帧计数
        /// </summary>
        private int mItemCreatedCheckFrameCount;
        #endregion

        #region Field - User Data
        /// <summary>
        /// 用户自定义对象数据
        /// </summary>
        private object mUserObjectData;

        /// <summary>
        /// 用户自定义整型数据1
        /// </summary>
        private int mUserIntData1;

        /// <summary>
        /// 用户自定义整型数据2
        /// </summary>
        private int mUserIntData2;

        /// <summary>
        /// 用户自定义字符串数据1
        /// </summary>
        private string mUserStringData1;

        /// <summary>
        /// 用户自定义字符串数据2
        /// </summary>
        private string mUserStringData2;
        #endregion

        //=========================================================================
        // 属性成员
        //=========================================================================
        #region Property - User Data
        /// <summary>
        /// 用户自定义对象数据
        /// </summary>
        public object UserObjectData
        {
            get => mUserObjectData;
            set => mUserObjectData = value;
        }

        /// <summary>
        /// 用户自定义整型数据1
        /// </summary>
        public int UserIntData1
        {
            get => mUserIntData1;
            set => mUserIntData1 = value;
        }

        /// <summary>
        /// 用户自定义整型数据2
        /// </summary>
        public int UserIntData2
        {
            get => mUserIntData2;
            set => mUserIntData2 = value;
        }

        /// <summary>
        /// 用户自定义字符串数据1
        /// </summary>
        public string UserStringData1
        {
            get => mUserStringData1;
            set => mUserStringData1 = value;
        }

        /// <summary>
        /// 用户自定义字符串数据2
        /// </summary>
        public string UserStringData2
        {
            get => mUserStringData2;
            set => mUserStringData2 = value;
        }
        #endregion

        #region Property - Snap & Offset
        /// <summary>
        /// 与视口对齐中心的距离
        /// </summary>
        public float DistanceWithViewPortSnapCenter
        {
            get => mDistanceWithViewPortSnapCenter;
            set => mDistanceWithViewPortSnapCenter = value;
        }

        /// <summary>
        /// 起始位置偏移量
        /// </summary>
        public float StartPosOffset
        {
            get => mStartOffset;
            set => mStartOffset = value;
        }

        /// <summary>
        /// 项创建帧计数
        /// </summary>
        public int ItemCreatedCheckFrameCount
        {
            get => mItemCreatedCheckFrameCount;
            set => mItemCreatedCheckFrameCount = value;
        }
        #endregion

        #region Property - Layout
        /// <summary>
        /// 项间距
        /// </summary>
        public float Padding
        {
            get => mPadding;
            set => mPadding = value;
        }
        #endregion

        #region Property - Cached Component
        /// <summary>
        /// 缓存的 RectTransform，自动获取
        /// </summary>
        public RectTransform CachedRectTransform
        {
            get
            {
                if (mCachedRectTransform == null)
                    mCachedRectTransform = GetComponent<RectTransform>();
                
                return mCachedRectTransform;
            }
        }
        #endregion

        #region Property - Identity
        /// <summary>
        /// 预制体名称
        /// </summary>
        public string ItemPrefabName
        {
            get => mItemPrefabName;
            set => mItemPrefabName = value;
        }

        /// <summary>
        /// 项索引
        /// </summary>
        public int ItemIndex
        {
            get => mItemIndex;
            set => mItemIndex = value;
        }

        /// <summary>
        /// 项唯一ID
        /// </summary>
        public int ItemId
        {
            get => mItemId;
            set => mItemId = value;
        }
        #endregion

        #region Property - State
        /// <summary>
        /// 初始化回调是否已调用
        /// </summary>
        public bool IsInitHandlerCalled
        {
            get => mIsInitHandlerCalled;
            set => mIsInitHandlerCalled = value;
        }
        #endregion

        #region Property - Parent ListView
        /// <summary>
        /// 父级滚动列表
        /// </summary>
        public AorListView ParentAorListView
        {
            get => mParentAorListView;
            set => mParentAorListView = value;
        }
        #endregion

        //=========================================================================
        // 边界计算属性
        //=========================================================================
        #region Property - Boundary (Y Axis)
        /// <summary>
        /// 项顶部Y坐标（根据排列方式自动计算）
        /// </summary>
        public float TopY
        {
            get
            {
                var arrangeType = ParentAorListView.ArrangeType;
                
                if (arrangeType == ListItemArrangeType.TopToBottom)
                    return CachedRectTransform.anchoredPosition3D.y;
                
                if (arrangeType == ListItemArrangeType.BottomToTop)
                    return CachedRectTransform.anchoredPosition3D.y + CachedRectTransform.rect.height;
                
                return 0;
            }
        }

        /// <summary>
        /// 项底部Y坐标（根据排列方式自动计算）
        /// </summary>
        public float BottomY
        {
            get
            {
                var arrangeType = ParentAorListView.ArrangeType;
                
                if (arrangeType == ListItemArrangeType.TopToBottom)
                    return CachedRectTransform.anchoredPosition3D.y - CachedRectTransform.rect.height;
                
                if (arrangeType == ListItemArrangeType.BottomToTop)
                    return CachedRectTransform.anchoredPosition3D.y;
                
                return 0;
            }
        }
        #endregion

        #region Property - Boundary (X Axis)
        /// <summary>
        /// 项左侧X坐标（根据排列方式自动计算）
        /// </summary>
        public float LeftX
        {
            get
            {
                var arrangeType = ParentAorListView.ArrangeType;
                
                if (arrangeType == ListItemArrangeType.LeftToRight)
                    return CachedRectTransform.anchoredPosition3D.x;
                
                if (arrangeType == ListItemArrangeType.RightToLeft)
                    return CachedRectTransform.anchoredPosition3D.x - CachedRectTransform.rect.width;
                
                return 0;
            }
        }

        /// <summary>
        /// 项右侧X坐标（根据排列方式自动计算）
        /// </summary>
        public float RightX
        {
            get
            {
                var arrangeType = ParentAorListView.ArrangeType;
                
                if (arrangeType == ListItemArrangeType.LeftToRight)
                    return CachedRectTransform.anchoredPosition3D.x + CachedRectTransform.rect.width;
                
                if (arrangeType == ListItemArrangeType.RightToLeft)
                    return CachedRectTransform.anchoredPosition3D.x;
                
                return 0;
            }
        }
        #endregion

        //=========================================================================
        // 尺寸计算属性
        //=========================================================================
        #region Property - Size
        /// <summary>
        /// 项尺寸：垂直列表=高度，水平列表=宽度
        /// </summary>
        public float ItemSize
        {
            get
            {
                return ParentAorListView.IsVertList 
                    ? CachedRectTransform.rect.height 
                    : CachedRectTransform.rect.width;
            }
        }

        /// <summary>
        /// 包含间距的项总尺寸
        /// </summary>
        public float ItemSizeWithPadding => ItemSize + mPadding;
        #endregion
    }
}