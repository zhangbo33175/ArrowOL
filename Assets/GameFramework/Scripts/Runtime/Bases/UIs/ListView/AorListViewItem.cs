using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Honor.Runtime
{
    /// <summary>
    /// 滚动列表项基类
    /// 所有 AorListView 列表的子项必须继承此类，提供列表项的通用属性、边界计算、数据存储
    /// </summary>
    public class AorListViewItem : MonoBehaviour
    {
        /// <summary>
        /// 列表项在列表中的索引
        /// 若 itemTotalCount = -1，索引范围为 int 极值；若 >=0，索引范围 0 ~ itemTotalCount-1
        /// </summary>
        int mItemIndex = -1;

        /// <summary>
        /// 列表项唯一ID
        /// 创建/从对象池获取时赋值，回收至对象池前保持不变
        /// </summary>
        int mItemId = -1;

        /// <summary>
        /// 归属的父级滚动列表组件
        /// </summary>
        AorListView _mParentAorListView = null;

        /// <summary>
        /// 初始化回调是否已执行
        /// </summary>
        bool mIsInitHandlerCalled = false;

        /// <summary>
        /// 列表项预制体名称
        /// </summary>
        string mItemPrefabName;

        /// <summary>
        /// 缓存的 RectTransform 组件，优化性能避免重复获取
        /// </summary>
        RectTransform mCachedRectTransform;

        /// <summary>
        /// 列表项之间的间距
        /// </summary>
        float mPadding;

        /// <summary>
        /// 当前项与视口对齐中心的距离
        /// </summary>
        float mDistanceWithViewPortSnapCenter = 0;

        /// <summary>
        /// 列表项创建检查帧计数
        /// </summary>
        int mItemCreatedCheckFrameCount = 0;

        /// <summary>
        /// 起始位置偏移量
        /// </summary>
        float mStartPosOffset = 0;

        /// <summary>
        /// 用户自定义对象数据
        /// </summary>
        object mUserObjectData = null;

        /// <summary>
        /// 用户自定义整型数据1
        /// </summary>
        int mUserIntData1 = 0;

        /// <summary>
        /// 用户自定义整型数据2
        /// </summary>
        int mUserIntData2 = 0;

        /// <summary>
        /// 用户自定义字符串数据1
        /// </summary>
        string mUserStringData1 = null;

        /// <summary>
        /// 用户自定义字符串数据2
        /// </summary>
        string mUserStringData2 = null;

        /// <summary>
        /// 用户自定义对象数据，外部可读写
        /// </summary>
        public object UserObjectData
        {
            get { return mUserObjectData; }
            set { mUserObjectData = value; }
        }

        /// <summary>
        /// 用户自定义整型数据1，外部可读写
        /// </summary>
        public int UserIntData1
        {
            get { return mUserIntData1; }
            set { mUserIntData1 = value; }
        }

        /// <summary>
        /// 用户自定义整型数据2，外部可读写
        /// </summary>
        public int UserIntData2
        {
            get { return mUserIntData2; }
            set { mUserIntData2 = value; }
        }

        /// <summary>
        /// 用户自定义字符串数据1，外部可读写
        /// </summary>
        public string UserStringData1
        {
            get { return mUserStringData1; }
            set { mUserStringData1 = value; }
        }

        /// <summary>
        /// 用户自定义字符串数据2，外部可读写
        /// </summary>
        public string UserStringData2
        {
            get { return mUserStringData2; }
            set { mUserStringData2 = value; }
        }

        /// <summary>
        /// 与视口对齐中心的距离，外部可读写
        /// </summary>
        public float DistanceWithViewPortSnapCenter
        {
            get { return mDistanceWithViewPortSnapCenter; }
            set { mDistanceWithViewPortSnapCenter = value; }
        }

        /// <summary>
        /// 起始位置偏移量，外部可读写
        /// </summary>
        public float StartPosOffset
        {
            get { return mStartPosOffset; }
            set { mStartPosOffset = value; }
        }

        /// <summary>
        /// 列表项创建检查帧计数，外部可读写
        /// </summary>
        public int ItemCreatedCheckFrameCount
        {
            get { return mItemCreatedCheckFrameCount; }
            set { mItemCreatedCheckFrameCount = value; }
        }

        /// <summary>
        /// 列表项间距，外部可读写
        /// </summary>
        public float Padding
        {
            get { return mPadding; }
            set { mPadding = value; }
        }

        /// <summary>
        /// 缓存的 RectTransform 组件，自动获取并缓存
        /// </summary>
        public RectTransform CachedRectTransform
        {
            get
            {
                if (mCachedRectTransform == null)
                {
                    mCachedRectTransform = gameObject.GetComponent<RectTransform>();
                }

                return mCachedRectTransform;
            }
        }

        /// <summary>
        /// 列表项预制体名称，外部可读写
        /// </summary>
        public string ItemPrefabName
        {
            get { return mItemPrefabName; }
            set { mItemPrefabName = value; }
        }

        /// <summary>
        /// 列表项索引，外部可读写
        /// </summary>
        public int ItemIndex
        {
            get { return mItemIndex; }
            set { mItemIndex = value; }
        }

        /// <summary>
        /// 列表项唯一ID，外部可读写
        /// </summary>
        public int ItemId
        {
            get { return mItemId; }
            set { mItemId = value; }
        }

        /// <summary>
        /// 初始化回调是否已调用，外部可读写
        /// </summary>
        public bool IsInitHandlerCalled
        {
            get { return mIsInitHandlerCalled; }
            set { mIsInitHandlerCalled = value; }
        }

        /// <summary>
        /// 父级滚动列表，外部可读写
        /// </summary>
        public AorListView ParentAorListView
        {
            get { return _mParentAorListView; }
            set { _mParentAorListView = value; }
        }

        /// <summary>
        /// 根据列表排列方式，获取列表项顶部Y坐标
        /// </summary>
        public float TopY
        {
            get
            {
                ListItemArrangeType arrageType = ParentAorListView.ArrangeType;
                if (arrageType == ListItemArrangeType.TopToBottom)
                {
                    return CachedRectTransform.anchoredPosition3D.y;
                }
                else if (arrageType == ListItemArrangeType.BottomToTop)
                {
                    return CachedRectTransform.anchoredPosition3D.y + CachedRectTransform.rect.height;
                }

                return 0;
            }
        }

        /// <summary>
        /// 根据列表排列方式，获取列表项底部Y坐标
        /// </summary>
        public float BottomY
        {
            get
            {
                ListItemArrangeType arrageType = ParentAorListView.ArrangeType;
                if (arrageType == ListItemArrangeType.TopToBottom)
                {
                    return CachedRectTransform.anchoredPosition3D.y - CachedRectTransform.rect.height;
                }
                else if (arrageType == ListItemArrangeType.BottomToTop)
                {
                    return CachedRectTransform.anchoredPosition3D.y;
                }

                return 0;
            }
        }

        /// <summary>
        /// 根据列表排列方式，获取列表项左侧X坐标
        /// </summary>
        public float LeftX
        {
            get
            {
                ListItemArrangeType arrageType = ParentAorListView.ArrangeType;
                if (arrageType == ListItemArrangeType.LeftToRight)
                {
                    return CachedRectTransform.anchoredPosition3D.x;
                }
                else if (arrageType == ListItemArrangeType.RightToLeft)
                {
                    return CachedRectTransform.anchoredPosition3D.x - CachedRectTransform.rect.width;
                }

                return 0;
            }
        }

        /// <summary>
        /// 根据列表排列方式，获取列表项右侧X坐标
        /// </summary>
        public float RightX
        {
            get
            {
                ListItemArrangeType arrageType = ParentAorListView.ArrangeType;
                if (arrageType == ListItemArrangeType.LeftToRight)
                {
                    return CachedRectTransform.anchoredPosition3D.x + CachedRectTransform.rect.width;
                }
                else if (arrageType == ListItemArrangeType.RightToLeft)
                {
                    return CachedRectTransform.anchoredPosition3D.x;
                }

                return 0;
            }
        }

        /// <summary>
        /// 列表项尺寸：垂直列表取高度，水平列表取宽度
        /// </summary>
        public float ItemSize
        {
            get
            {
                if (ParentAorListView.IsVertList)
                {
                    return CachedRectTransform.rect.height;
                }
                else
                {
                    return CachedRectTransform.rect.width;
                }
            }
        }

        /// <summary>
        /// 包含间距的列表项总尺寸
        /// </summary>
        public float ItemSizeWithPadding
        {
            get { return ItemSize + mPadding; }
        }
    }
}