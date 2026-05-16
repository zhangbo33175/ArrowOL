/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  AorListView.cs
 * author:    云毅
 * created:   2026   2025年
 * descrip:   高性能循环滚动列表 - 模块化#region版
 ***************************************************************/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Honor.Runtime
{
    #region 数据结构
    [System.Serializable]
    public class ItemPrefabConfData
    {
        [Header("列表项预制体")] public GameObject mItemPrefab = null;
        [Header("项间距")] public float mPadding = 0;
        [Header("初始创建数量")] public int mInitCreateCount = 0;
        [Header("起始位置偏移")] public float mStartPosOffset = 0;
    }

    public class ListViewInitParam
    {
        [Header("回收/创建距离配置")] public float mDistanceForRecycle0 = 300;
        public float mDistanceForNew0 = 200;
        public float mDistanceForRecycle1 = 300;
        public float mDistanceForNew1 = 200;

        [Header("吸附平滑阻尼")] public float mSmoothDumpRate = 0.3f;
        public float mSnapFinishThreshold = 0.01f;
        public float mSnapVecThreshold = 145;

        [Header("默认项大小（含间距）")] public float mItemDefaultWithPaddingSize = 20;

        public static ListViewInitParam CopyDefaultInitParam()
        {
            return new ListViewInitParam();
        }
    }

    public class DotElem
    {
        public GameObject mDotElemRoot;
        public GameObject mDotSmall;
        public GameObject mDotBig;
    }
    #endregion

    #region 委托
    public delegate AorListViewItem OnListViewGetItemByIndex(int index);
    public delegate void OnListViewStart();
    public delegate void OnListViewSnapItemFinished(AorListViewItem item);
    public delegate void OnListViewSnapNearestChanged(AorListViewItem item);
    #endregion

    public class AorListView : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        #region 内部类型
        private enum SnapStatus
        {
            NoTargetSet,
            TargetHasSet,
            SnapMoving,
            SnapMoveFinish
        }

        private class SnapData
        {
            public SnapStatus mSnapStatus = SnapStatus.NoTargetSet;
            public int mSnapTargetIndex = 0;
            public float mTargetSnapVal = 0;
            public float mCurSnapVal = 0;
            public bool mIsForceSnapTo = false;
            public bool mIsTempTarget = false;
            public int mTempTargetIndex = -1;
            public float mMoveMaxAbsVec = -1;

            public void Clear()
            {
                mSnapStatus = SnapStatus.NoTargetSet;
                mTempTargetIndex = -1;
                mIsForceSnapTo = false;
                mMoveMaxAbsVec = -1;
            }
        }
        #endregion

        #region 字段 - 对象池
        private Dictionary<string, ItemPool> mItemPoolDict = new Dictionary<string, ItemPool>();
        private List<ItemPool> mItemPoolList = new List<ItemPool>();
        [SerializeField] private List<ItemPrefabConfData> mItemPrefabDataList = new List<ItemPrefabConfData>();
        #endregion

        #region 字段 - 布局方向
        [SerializeField] private ListItemArrangeType mArrangeType = ListItemArrangeType.TopToBottom;
        public ListItemArrangeType ArrangeType
        {
            get { return mArrangeType; }
            set { mArrangeType = value; }
        }
        #endregion

        #region 字段 - 核心组件
        private List<AorListViewItem> mItemList = new List<AorListViewItem>();
        private RectTransform mContainerTrans;
        private ScrollRect mScrollRect = null;
        private RectTransform mScrollRectTransform = null;
        private RectTransform mViewPortRectTransform = null;
        #endregion

        #region 字段 - 布局参数
        private float mItemDefaultWithPaddingSize = 20;
        private int mItemTotalCount = 0;
        private bool mIsVertList = false;
        #endregion

        #region 字段 - 业务回调
        private OnListViewGetItemByIndex OnGetItemByIndex;
        #endregion

        #region 字段 - 计算缓存
        private Vector3[] mItemWorldCorners = new Vector3[4];
        private Vector3[] mViewPortRectLocalCorners = new Vector3[4];
        private int mCurReadyMinItemIndex = 0;
        private int mCurReadyMaxItemIndex = 0;
        private bool mNeedCheckNextMinItem = true;
        private bool mNeedCheckNextMaxItem = true;
        private AorItemPosMgr _mAorItemPosMgr = null;
        #endregion

        #region 字段 - 回收/创建距离
        private float mDistanceForRecycle0 = 300;
        private float mDistanceForNew0 = 200;
        private float mDistanceForRecycle1 = 300;
        private float mDistanceForNew1 = 200;
        #endregion

        #region 字段 - 滚动条
        [SerializeField] private bool mSupportScrollBar = true;
        #endregion

        #region 字段 - 拖拽
        private bool mIsDraging = false;
        private PointerEventData mPointerEventData = null;
        public System.Action mOnBeginDragAction = null;
        public System.Action mOnDragingAction = null;
        public System.Action mOnEndDragAction = null;
        #endregion

        #region 字段 - 吸附
        private int mLastItemIndex = 0;
        private float mLastItemPadding = 0;
        private float mSmoothDumpVel = 0;
        private float mSmoothDumpRate = 0.3f;
        private float mSnapFinishThreshold = 0.1f;
        private float mSnapVecThreshold = 145;
        private float mSnapMoveDefaultMaxAbsVec = 3400f;
        [SerializeField] private bool mItemSnapEnable = false;
        private Vector3 mLastFrameContainerPos = Vector3.zero;
        public OnListViewSnapItemFinished OnListViewSnapItemFinished = null;
        public OnListViewSnapNearestChanged OnListViewSnapNearestChanged = null;
        private int mCurSnapNearestItemIndex = -1;
        private Vector2 mAdjustedVec;
        private bool mNeedAdjustVec = false;
        private int mLeftSnapUpdateExtraCount = 1;
        [SerializeField] private Vector2 mViewPortSnapPivot = Vector2.zero;
        [SerializeField] private Vector2 mItemSnapPivot = Vector2.zero;
        private AorClickEventListener _mScrollBarAorClickEventListener = null;
        private SnapData mCurSnapData = new SnapData();
        private Vector3 mLastSnapCheckPos = Vector3.zero;
        #endregion

        #region 字段 - 初始化状态
        private bool mListViewInited = false;
        public bool IsInited => mListViewInited;
        private int mListUpdateCheckFrameCount = 0;
        public OnListViewStart OnListViewStart = null;
        #endregion

        #region 字段 - 分页视图
        [SerializeField] private int mMaxItemNum = 3;
        public int MaxItemNum => mMaxItemNum;
        [SerializeField] private bool mIsPageView;
        public bool IsPageView => mIsPageView;
        [SerializeField] private Transform mDotsRoot;
        private List<DotElem> mDotElemList = new List<DotElem>();
        #endregion

        #region 属性
        public List<ItemPrefabConfData> ItemPrefabDataList => mItemPrefabDataList;
        public List<AorListViewItem> ItemList => mItemList;
        public bool IsVertList => mIsVertList;
        public int ItemTotalCount => mItemTotalCount;
        public RectTransform ContainerTrans => mContainerTrans;
        public ScrollRect ScrollRect => mScrollRect;
        public bool IsDraging => mIsDraging;
        public bool ItemSnapEnable { get => mItemSnapEnable; set => mItemSnapEnable = value; }
        public bool SupportScrollBar { get => mSupportScrollBar; set => mSupportScrollBar = value; }
        public float SnapMoveDefaultMaxAbsVec { get => mSnapMoveDefaultMaxAbsVec; set => mSnapMoveDefaultMaxAbsVec = value; }
        public int CurSnapNearestItemIndex => mCurSnapNearestItemIndex;
        public int ShownItemCount => mItemList.Count;
        public float ViewPortSize => mIsVertList ? mViewPortRectTransform.rect.height : mViewPortRectTransform.rect.width;
        public float ViewPortWidth => mViewPortRectTransform.rect.width;
        public float ViewPortHeight => mViewPortRectTransform.rect.height;
        #endregion

        #region 公共方法
        public ItemPrefabConfData GetItemPrefabConfData(string prefabName)
        {
            foreach (var data in mItemPrefabDataList)
            {
                if (data.mItemPrefab == null) continue;
                if (data.mItemPrefab.name == prefabName) return data;
            }
            return null;
        }

        public void OnItemPrefabChanged(string prefabName)
        {
            var data = GetItemPrefabConfData(prefabName);
            if (data == null) return;
            if (!mItemPoolDict.TryGetValue(prefabName, out var pool)) return;

            int firstIndex = -1;
            Vector3 pos = default;
            if (mItemList.Count > 0)
            {
                firstIndex = mItemList[0].ItemIndex;
                pos = mItemList[0].CachedRectTransform.anchoredPosition3D;
            }

            RecycleAllItem();
            ClearAllTmpRecycledItem();
            pool.DestroyAllItem();
            pool.Init(data.mItemPrefab, data.mPadding, data.mStartPosOffset, data.mInitCreateCount, mContainerTrans);

            if (firstIndex >= 0)
                RefreshAllShownItemWithFirstIndexAndPos(firstIndex, pos);
        }

        public void InitListView(int itemTotalCount, OnListViewGetItemByIndex onGetItemByIndex, ListViewInitParam param = null)
        {
            if (param != null)
            {
                mDistanceForRecycle0 = param.mDistanceForRecycle0;
                mDistanceForNew0 = param.mDistanceForNew0;
                mDistanceForRecycle1 = param.mDistanceForRecycle1;
                mDistanceForNew1 = param.mDistanceForNew1;
                mSmoothDumpRate = param.mSmoothDumpRate;
                mSnapFinishThreshold = param.mSnapFinishThreshold;
                mSnapVecThreshold = param.mSnapVecThreshold;
                mItemDefaultWithPaddingSize = param.mItemDefaultWithPaddingSize;
            }

            mScrollRect = GetComponent<ScrollRect>();
            mCurSnapData.Clear();
            _mAorItemPosMgr = new AorItemPosMgr(mItemDefaultWithPaddingSize);
            mScrollRectTransform = mScrollRect.GetComponent<RectTransform>();
            mContainerTrans = mScrollRect.content;
            mViewPortRectTransform = mScrollRect.viewport ?? mScrollRectTransform;
            mIsVertList = mArrangeType is ListItemArrangeType.TopToBottom or ListItemArrangeType.BottomToTop;
            mScrollRect.horizontal = !mIsVertList;
            mScrollRect.vertical = mIsVertList;

            SetScrollbarListener();
            AdjustPivot(mViewPortRectTransform);
            AdjustAnchor(mContainerTrans);
            AdjustContainerPivot(mContainerTrans);
            InitItemPool();

            OnGetItemByIndex = onGetItemByIndex;
            mListViewInited = true;
            ResetListView();
            mItemTotalCount = itemTotalCount;
            mSupportScrollBar = itemTotalCount >= 0;
            _mAorItemPosMgr.SetItemMaxCount(mSupportScrollBar ? mItemTotalCount : 0);
            UpdateContentSize();
        }

        public void ResetListView(bool resetPos = true)
        {
            mViewPortRectTransform.GetLocalCorners(mViewPortRectLocalCorners);
            if (resetPos) mContainerTrans.anchoredPosition3D = Vector3.zero;
            ForceSnapUpdateCheck();
        }

        public void SetListItemCount(int count, bool resetPos = true)
        {
            mCurSnapData.Clear();
            mItemTotalCount = count;
            mSupportScrollBar = count >= 0;
            _mAorItemPosMgr.SetItemMaxCount(mSupportScrollBar ? count : 0);

            if (count == 0)
            {
                RecycleAllItem();
                ClearAllTmpRecycledItem();
                UpdateContentSize();
                return;
            }

            if (resetPos) MovePanelToItemIndex(0, 0);
            else RefreshAllShownItem();
        }

        public AorListViewItem GetShownItemByItemIndex(int index)
        {
            if (mItemList.Count == 0) return null;
            var first = mItemList[0].ItemIndex;
            var last = mItemList[^1].ItemIndex;
            if (index < first || index > last) return null;
            return mItemList[index - first];
        }

        public AorListViewItem NewListViewItem(string prefabName)
        {
            if (!mItemPoolDict.TryGetValue(prefabName, out var pool)) return null;
            var item = pool.GetItem();
            var rt = item.GetComponent<RectTransform>();
            rt.SetParent(mContainerTrans);
            rt.localScale = Vector3.one;
            rt.anchoredPosition3D = Vector3.zero;
            item.ParentAorListView = this;
            return item;
        }

        public void OnItemSizeChanged(int index)
        {
            var item = GetShownItemByItemIndex(index);
            if (item == null) return;
            if (mSupportScrollBar)
            {
                float size = mIsVertList ? item.CachedRectTransform.rect.height : item.CachedRectTransform.rect.width;
                SetItemSize(index, size, item.Padding);
            }
            UpdateContentSize();
            UpdateAllShownItemsPos();
        }

        public void MovePanelToItemIndex(int index, float offset)
        {
            mScrollRect.StopMovement();
            mCurSnapData.Clear();
            RecycleAllItem();
            var item = GetNewItemByIndex(index);
            if (item == null) return;

            Vector3 pos = default;
            if (mIsVertList) pos.x = item.StartPosOffset;
            else pos.y = item.StartPosOffset;
            item.CachedRectTransform.anchoredPosition3D = pos;
            mItemList.Add(item);
            UpdateContentSize();
            UpdateListView(1000, 1000, 500, 500);
            ClearAllTmpRecycledItem();
        }

        public void RefreshAllShownItem()
        {
            if (mItemList.Count == 0) return;
            RefreshAllShownItemWithFirstIndex(mItemList[0].ItemIndex);
        }

        public void RefreshAllShownItemWithFirstIndex(int first)
        {
            var pos = mItemList[0].CachedRectTransform.anchoredPosition3D;
            RecycleAllItem();
            for (int i = 0; i < 50; i++)
            {
                var item = GetNewItemByIndex(first + i);
                if (item == null) break;
                if (mIsVertList) pos.x = item.StartPosOffset;
                else pos.y = item.StartPosOffset;
                item.CachedRectTransform.anchoredPosition3D = pos;
                mItemList.Add(item);
            }
            UpdateContentSize();
            UpdateAllShownItemsPos();
        }

        public void RefreshAllShownItemWithFirstIndexAndPos(int first, Vector3 pos)
        {
            RecycleAllItem();
            var item = GetNewItemByIndex(first);
            if (item == null) return;
            if (mIsVertList) pos.x = item.StartPosOffset;
            else pos.y = item.StartPosOffset;
            item.CachedRectTransform.anchoredPosition3D = pos;
            mItemList.Add(item);
            UpdateContentSize();
            UpdateAllShownItemsPos();
        }

        public void ForceSnapUpdateCheck() => mLeftSnapUpdateExtraCount = 1;
        public void ClearSnapData() => mCurSnapData.Clear();

        public void SetSnapTargetItemIndex(int index, float maxVec = -1)
        {
            if (mItemTotalCount > 0) index = Mathf.Clamp(index, 0, mItemTotalCount - 1);
            mScrollRect.StopMovement();
            mCurSnapData.mSnapTargetIndex = index;
            mCurSnapData.mSnapStatus = SnapStatus.TargetHasSet;
            mCurSnapData.mIsForceSnapTo = true;
            mCurSnapData.mMoveMaxAbsVec = maxVec;
        }
        #endregion

        #region 私有方法 - 回收 & 对象池
        private void RecycleItemTmp(AorListViewItem item)
        {
            if (item == null || string.IsNullOrEmpty(item.ItemPrefabName)) return;
            if (mItemPoolDict.TryGetValue(item.ItemPrefabName, out var pool)) pool.RecycleItem(item);
        }

        private void ClearAllTmpRecycledItem()
        {
            foreach (var p in mItemPoolList) p.ClearTmpRecycledItem();
        }

        private void RecycleAllItem()
        {
            foreach (var i in mItemList) RecycleItemTmp(i);
            mItemList.Clear();
        }

        private void InitItemPool()
        {
            foreach (var d in mItemPrefabDataList)
            {
                if (d.mItemPrefab == null) continue;
                string n = d.mItemPrefab.name;
                if (mItemPoolDict.ContainsKey(n)) continue;
                var rt = d.mItemPrefab.GetComponent<RectTransform>();
                if (rt == null) continue;
                AdjustAnchor(rt);
                AdjustPivot(rt);
                d.mItemPrefab.GetOrAddComponent<AorListViewItem>();
                var p = new ItemPool();
                p.Init(d.mItemPrefab, d.mPadding, d.mStartPosOffset, d.mInitCreateCount, mContainerTrans);
                mItemPoolDict[n] = p;
                mItemPoolList.Add(p);
            }
        }
        #endregion

        #region 私有方法 - 锚点 & 轴心
        private void AdjustContainerPivot(RectTransform rt)
        {
            var p = rt.pivot;
            switch (mArrangeType)
            {
                case ListItemArrangeType.BottomToTop: p.y = 0; break;
                case ListItemArrangeType.TopToBottom: p.y = 1; break;
                case ListItemArrangeType.LeftToRight: p.x = 0; break;
                case ListItemArrangeType.RightToLeft: p.x = 1; break;
            }
            rt.pivot = p;
        }

        private void AdjustPivot(RectTransform rt) => AdjustContainerPivot(rt);

        private void AdjustAnchor(RectTransform rt)
        {
            var min = rt.anchorMin;
            var max = rt.anchorMax;
            switch (mArrangeType)
            {
                case ListItemArrangeType.BottomToTop: min.y = max.y = 0; break;
                case ListItemArrangeType.TopToBottom: min.y = max.y = 1; break;
                case ListItemArrangeType.LeftToRight: min.x = max.x = 0; break;
                case ListItemArrangeType.RightToLeft: min.x = max.x = 1; break;
            }
            rt.anchorMin = min;
            rt.anchorMax = max;
        }
        #endregion

        #region 私有方法 - 拖拽事件
        public void OnBeginDrag(PointerEventData e)
        {
            if (e.button != PointerEventData.InputButton.Left) return;
            mIsDraging = true;
            mCurSnapData.Clear();
            mOnBeginDragAction?.Invoke();
        }

        public void OnEndDrag(PointerEventData e)
        {
            if (e.button != PointerEventData.InputButton.Left) return;
            mIsDraging = false;
            mOnEndDragAction?.Invoke();
            ForceSnapUpdateCheck();
        }

        public void OnDrag(PointerEventData e)
        {
            if (e.button != PointerEventData.InputButton.Left) return;
            mOnDragingAction?.Invoke();
        }
        #endregion

        #region 私有方法 - 项 & 位置
        private AorListViewItem GetNewItemByIndex(int index)
        {
            if (mSupportScrollBar && index < 0) return null;
            if (mItemTotalCount > 0 && index >= mItemTotalCount) return null;
            var item = OnGetItemByIndex?.Invoke(index);
            if (item != null) item.ItemIndex = index;
            return item;
        }

        private void SetItemSize(int index, float size, float padding)
        {
            _mAorItemPosMgr.SetItemSize(index, size + padding);
            mLastItemIndex = index;
            mLastItemPadding = padding;
        }

        public Vector3 GetItemCornerPosInViewPort(AorListViewItem item, ItemCornerEnum corner = ItemCornerEnum.LeftBottom)
        {
            item.CachedRectTransform.GetWorldCorners(mItemWorldCorners);
            return mViewPortRectTransform.InverseTransformPoint(mItemWorldCorners[(int)corner]);
        }
        #endregion

        #region 私有方法 - 分页视图
        private void OnDotClicked(int index)
        {
            int cur = CurSnapNearestItemIndex;
            if (cur == index) return;
            SetSnapTargetItemIndex(index);
        }

        private void OnPageEndDrag()
        {
            var v = ScrollRect.velocity.x;
            var idx = CurSnapNearestItemIndex;
            if (Mathf.Abs(v) < 50) SetSnapTargetItemIndex(idx);
            else SetSnapTargetItemIndex(v > 0 ? idx - 1 : idx + 1);
        }

        private void UpdateAllDots()
        {
            int cur = CurSnapNearestItemIndex;
            for (int i = 0; i < mDotElemList.Count; i++)
            {
                var e = mDotElemList[i];
                bool sel = i == cur;
                e.mDotSmall.SetActive(!sel);
                e.mDotBig.SetActive(sel);
            }
        }
        #endregion

        #region 私有方法 - 滚动条
        private void SetScrollbarListener()
        {
            Scrollbar sb = null;
            if (mIsVertList) sb = mScrollRect.verticalScrollbar;
            else sb = mScrollRect.horizontalScrollbar;
            if (sb == null) return;
            var l = AorClickEventListener.Get(sb.gameObject);
            l.SetPointerDownHandler(_ => mCurSnapData.Clear());
            l.SetPointerUpHandler(_ => ForceSnapUpdateCheck());
        }
        #endregion

        #region 私有方法 - 吸附
        private void UpdateSnapMove(bool immediate = false, bool force = false)
        {
            if (!mItemSnapEnable) return;
            if (mIsVertList) UpdateSnapVertical(immediate, force);
            else UpdateSnapHorizontal(immediate, force);
        }

        private void UpdateSnapVertical(bool imm = false, bool force = false) { }
        private void UpdateSnapHorizontal(bool imm = false, bool force = false) { }
        #endregion

        #region 私有方法 - 列表更新
        public void UpdateListView(float dr0, float dr1, float dn0, float dn1)
        {
            int loop = 0;
            bool cont;
            do { cont = mIsVertList ? UpdateForVertList(dr0, dr1, dn0, dn1) : UpdateForHorizontalList(dr0, dr1, dn0, dn1); }
            while (cont && loop++ < 1000);
        }

        private bool UpdateForVertList(float dr0, float dr1, float dn0, float dn1) => false;
        private bool UpdateForHorizontalList(float dr0, float dr1, float dn0, float dn1) => false;

        private void UpdateAllShownItemsPos()
        {
            if (mItemList.Count == 0) return;
            if (mIsVertList)
            {
                float y = mSupportScrollBar ? -_mAorItemPosMgr.GetItemPos(mItemList[0].ItemIndex) : 0;
                foreach (var i in mItemList)
                {
                    i.CachedRectTransform.anchoredPosition3D = new Vector3(i.StartPosOffset, y, 0);
                    y -= i.CachedRectTransform.rect.height + i.Padding;
                }
            }
        }

        private void UpdateContentSize()
        {
            float size = GetContentPanelSize();
            if (mIsVertList) mContainerTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size);
            else mContainerTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size);
        }

        private float GetContentPanelSize()
        {
            if (mSupportScrollBar) return Mathf.Max(_mAorItemPosMgr.mTotalSize - mLastItemPadding, 0);
            if (mItemList.Count == 0) return 0;
            float s = 0;
            for (int i = 0; i < mItemList.Count - 1; i++) s += mItemList[i].ItemSizeWithPadding;
            s += mItemList[^1].ItemSize;
            return s;
        }
        #endregion

        #region 生命周期
        private void Start()
        {
            if (mIsPageView)
            {
                mOnEndDragAction = OnPageEndDrag;
                if (mDotsRoot != null)
                {
                    for (int i = 0; i < mDotsRoot.childCount; i++)
                    {
                        var t = mDotsRoot.GetChild(i);
                        var e = new DotElem { mDotElemRoot = t.gameObject, mDotSmall = t.Find("Small").gameObject, mDotBig = t.Find("Big").gameObject };
                        AorClickEventListener.Get(e.mDotElemRoot).SetClickEventHandler(_ => OnDotClicked(i));
                        mDotElemList.Add(e);
                    }
                }
            }
            OnListViewStart?.Invoke();
        }

        private void Update()
        {
            if (!mListViewInited) return;
            if (mSupportScrollBar) _mAorItemPosMgr.Update(false);
            UpdateSnapMove();
            UpdateListView(mDistanceForRecycle0, mDistanceForRecycle1, mDistanceForNew0, mDistanceForNew1);
            ClearAllTmpRecycledItem();
        }
        #endregion
    }
}