/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  AorListView.cs
 * author:    云毅
 * created:   2026
 * descrip:   高性能循环滚动列表 - 模块化#region版
 ***************************************************************/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Honor.Runtime
{
    #region 数据结构
    /// <summary>
    /// 列表项预制体配置数据：描述一种列表项预制体的创建与布局参数
    /// </summary>
    [System.Serializable]
    public class ItemPrefabConfData
    {
        /// <summary>列表项预制体</summary>
        [Header("列表项预制体")] public GameObject mItemPrefab = null;
        /// <summary>项间距（相邻列表项之间的间隔）</summary>
        [Header("项间距")] public float mPadding = 0;
        /// <summary>初始创建数量：对象池预热时预创建的项数</summary>
        [Header("初始创建数量")] public int mInitCreateCount = 0;
        /// <summary>起始位置偏移：该项相对于列表起点的位置偏移</summary>
        [Header("起始位置偏移")] public float mStartPosOffset = 0;
    }

    /// <summary>
    /// 列表初始化参数：回收/创建距离、吸附阻尼、默认项大小等可调参数集合
    /// </summary>
    public class ListViewInitParam
    {
        /// <summary>纵向回收距离阈值0</summary>
        [Header("回收/创建距离配置")] public float mDistanceForRecycle0 = 300;
        /// <summary>纵向新建距离阈值0</summary>
        public float mDistanceForNew0 = 200;
        /// <summary>横向回收距离阈值1</summary>
        public float mDistanceForRecycle1 = 300;
        /// <summary>横向新建距离阈值1</summary>
        public float mDistanceForNew1 = 200;

        /// <summary>吸附平滑阻尼系数</summary>
        [Header("吸附平滑阻尼")] public float mSmoothDumpRate = 0.3f;
        /// <summary>吸附完成判定阈值（距离误差小于该值即视为吸附完成）</summary>
        public float mSnapFinishThreshold = 0.01f;
        /// <summary>吸附速度阈值（小于该速度触发吸附）</summary>
        public float mSnapVecThreshold = 145;

        /// <summary>默认项大小（含间距）</summary>
        [Header("默认项大小（含间距）")] public float mItemDefaultWithPaddingSize = 20;

        /// <summary>
        /// 拷贝一份默认初始化参数
        /// </summary>
        /// <returns>默认参数实例</returns>
        public static ListViewInitParam CopyDefaultInitParam()
        {
            return new ListViewInitParam();
        }
    }

    /// <summary>
    /// 分页视图指示器（圆点）元素结构
    /// </summary>
    public class DotElem
    {
        /// <summary>圆点根节点</summary>
        public GameObject mDotElemRoot;
        /// <summary>未选中状态小圆点</summary>
        public GameObject mDotSmall;
        /// <summary>选中状态大圆点</summary>
        public GameObject mDotBig;
    }
    #endregion

    #region 委托
    /// <summary>根据列表项索引获取列表项的回调委托</summary>
    /// <param name="index">列表项索引</param>
    /// <returns>对应的列表项实例</returns>
    public delegate AorListViewItem OnListViewGetItemByIndex(int index);
    /// <summary>列表启动回调委托</summary>
    public delegate void OnListViewStart();
    /// <summary>列表项吸附完成回调委托</summary>
    /// <param name="item">吸附完成时居中的列表项</param>
    public delegate void OnListViewSnapItemFinished(AorListViewItem item);
    /// <summary>吸附最近项变化回调委托</summary>
    /// <param name="item">当前最近的列表项</param>
    public delegate void OnListViewSnapNearestChanged(AorListViewItem item);
    #endregion

    /// <summary>
    /// 高性能循环滚动列表
    /// 作用：仅创建可见区域内的列表项，滑出屏幕的项自动回收复用，支持横/纵布局、分页、吸附
    /// </summary>
    /// <remarks>
    /// 特性：对象池复用列表项、按距离动态回收/创建、可选分页视图与圆点指示器、
    /// 可选吸附对齐（Snap）。对外通过 InitListView 初始化，SetListItemCount 设置数据量。
    /// </remarks>
    public class AorListView : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        #region 内部类型
        /// <summary>吸附状态机状态枚举</summary>
        private enum SnapStatus
        {
            /// <summary>未设置吸附目标</summary>
            NoTargetSet,
            /// <summary>已设置吸附目标</summary>
            TargetHasSet,
            /// <summary>吸附移动中</summary>
            SnapMoving,
            /// <summary>吸附移动完成</summary>
            SnapMoveFinish
        }

        /// <summary>
        /// 吸附过程数据：记录当前吸附目标、进度与速度等中间状态
        /// </summary>
        private class SnapData
        {
            /// <summary>当前吸附状态</summary>
            public SnapStatus mSnapStatus = SnapStatus.NoTargetSet;
            /// <summary>吸附目标项索引</summary>
            public int mSnapTargetIndex = 0;
            /// <summary>目标吸附位置值</summary>
            public float mTargetSnapVal = 0;
            /// <summary>当前吸附位置值</summary>
            public float mCurSnapVal = 0;
            /// <summary>是否为强制吸附到指定项</summary>
            public bool mIsForceSnapTo = false;
            /// <summary>是否为临时吸附目标</summary>
            public bool mIsTempTarget = false;
            /// <summary>临时吸附目标索引</summary>
            public int mTempTargetIndex = -1;
            /// <summary>吸附移动允许的最大绝对速度</summary>
            public float mMoveMaxAbsVec = -1;

            /// <summary>
            /// 清空吸附状态，恢复到未设置目标的初始状态
            /// </summary>
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
        /// <summary>对象池字典：预制体名 -> 对象池</summary>
        private Dictionary<string, ItemPool> mItemPoolDict = new Dictionary<string, ItemPool>();
        /// <summary>对象池列表（与字典一一对应，便于遍历）</summary>
        private List<ItemPool> mItemPoolList = new List<ItemPool>();
        /// <summary>列表项预制体配置列表（序列化，在 Inspector 中配置）</summary>
        [SerializeField] private List<ItemPrefabConfData> mItemPrefabDataList = new List<ItemPrefabConfData>();
        #endregion

        #region 字段 - 布局方向
        /// <summary>列表项排列方向（序列化，Inspector 可配置）</summary>
        [SerializeField] private ListItemArrangeType mArrangeType = ListItemArrangeType.TopToBottom;
        /// <summary>列表项排列方向</summary>
        public ListItemArrangeType ArrangeType
        {
            get { return mArrangeType; }
            set { mArrangeType = value; }
        }
        #endregion

        #region 字段 - 核心组件
        /// <summary>当前实际显示中的列表项集合</summary>
        private List<AorListViewItem> mItemList = new List<AorListViewItem>();
        /// <summary>容器（content）节点，列表项挂在其下</summary>
        private RectTransform mContainerTrans;
        /// <summary>ScrollRect 组件引用</summary>
        private ScrollRect mScrollRect = null;
        /// <summary>ScrollRect 的 RectTransform</summary>
        private RectTransform mScrollRectTransform = null;
        /// <summary>视口（viewport）的 RectTransform</summary>
        private RectTransform mViewPortRectTransform = null;
        #endregion

        #region 字段 - 布局参数
        /// <summary>默认项大小（含间距）</summary>
        private float mItemDefaultWithPaddingSize = 20;
        /// <summary>列表项总数</summary>
        private int mItemTotalCount = 0;
        /// <summary>是否为垂直列表</summary>
        private bool mIsVertList = false;
        #endregion

        #region 字段 - 业务回调
        /// <summary>外部设置的"按索引获取列表项"回调</summary>
        private OnListViewGetItemByIndex OnGetItemByIndex;
        #endregion

        #region 字段 - 计算缓存
        /// <summary>列表项世界坐标四角缓存数组</summary>
        private Vector3[] mItemWorldCorners = new Vector3[4];
        /// <summary>视口本地坐标四角缓存数组</summary>
        private Vector3[] mViewPortRectLocalCorners = new Vector3[4];
        /// <summary>列表项位置管理器（按索引计算每项位置与总尺寸）</summary>
        private AorItemPosMgr m_AorItemPosMgr = null;
        #endregion

        #region 字段 - 回收/创建距离
        /// <summary>纵向回收距离阈值0</summary>
        private float mDistanceForRecycle0 = 300;
        /// <summary>纵向新建距离阈值0</summary>
        private float mDistanceForNew0 = 200;
        /// <summary>横向回收距离阈值1</summary>
        private float mDistanceForRecycle1 = 300;
        /// <summary>横向新建距离阈值1</summary>
        private float mDistanceForNew1 = 200;
        #endregion

        #region 字段 - 滚动条
        /// <summary>是否支持滚动条（数据量大于等于0时启用）</summary>
        [SerializeField] private bool mSupportScrollBar = true;
        #endregion

        #region 字段 - 拖拽
        /// <summary>当前是否正在拖拽</summary>
        private bool mIsDraging = false;
        /// <summary>开始拖拽时的外部回调</summary>
        public System.Action mOnBeginDragAction = null;
        /// <summary>拖拽中的外部回调</summary>
        public System.Action mOnDragingAction = null;
        /// <summary>结束拖拽时的外部回调</summary>
        public System.Action mOnEndDragAction = null;
        #endregion

        #region 字段 - 吸附
        /// <summary>上一次设置尺寸的项索引</summary>
        private int mLastItemIndex = 0;
        /// <summary>上一次设置尺寸的项间距</summary>
        private float mLastItemPadding = 0;
        /// <summary>吸附平滑阻尼速率</summary>
        private float mSmoothDumpRate = 0.3f;
        /// <summary>吸附完成判定阈值</summary>
        private float mSnapFinishThreshold = 0.1f;
        /// <summary>吸附速度阈值</summary>
        private float mSnapVecThreshold = 145;
        /// <summary>吸附移动默认最大绝对速度</summary>
        private float mSnapMoveDefaultMaxAbsVec = 3400f;
        /// <summary>是否启用项吸附（序列化，Inspector 可配置）</summary>
        [SerializeField] private bool mItemSnapEnable = false;
        /// <summary>上一帧容器位置（用于计算位移增量）</summary>
        private Vector3 mLastFrameContainerPos = Vector3.zero;
        /// <summary>吸附项完成回调</summary>
        public OnListViewSnapItemFinished OnListViewSnapItemFinished = null;
        /// <summary>吸附最近项变化回调</summary>
        public OnListViewSnapNearestChanged OnListViewSnapNearestChanged = null;
        /// <summary>当前吸附最近的项索引</summary>
        private int mCurSnapNearestItemIndex = -1;
        /// <summary>经调整后的吸附速度向量</summary>
        private Vector2 mAdjustedVec;
        /// <summary>视口吸附轴心（序列化）</summary>
        [SerializeField] private Vector2 mViewPortSnapPivot = Vector2.zero;
        /// <summary>列表项吸附轴心（序列化）</summary>
        [SerializeField] private Vector2 mItemSnapPivot = Vector2.zero;
        /// <summary>当前吸附过程数据</summary>
        private SnapData mCurSnapData = new SnapData();
        /// <summary>上一次吸附检查时的容器位置</summary>
        private Vector3 mLastSnapCheckPos = Vector3.zero;
        #endregion

        #region 字段 - 初始化状态
        /// <summary>列表是否已完成初始化</summary>
        private bool mListViewInited = false;
        /// <summary>列表是否已完成初始化</summary>
        public bool IsInited => mListViewInited;
        /// <summary>列表启动回调</summary>
        public OnListViewStart OnListViewStart = null;
        #endregion

        #region 字段 - 分页视图
        /// <summary>每页最大项数（序列化）</summary>
        [SerializeField] private int mMaxItemNum = 3;
        /// <summary>每页最大项数</summary>
        public int MaxItemNum => mMaxItemNum;
        /// <summary>是否为分页视图模式（序列化）</summary>
        [SerializeField] private bool mIsPageView;
        /// <summary>是否为分页视图模式</summary>
        public bool IsPageView => mIsPageView;
        /// <summary>圆点指示器根节点（序列化）</summary>
        [SerializeField] private Transform mDotsRoot;
        /// <summary>圆点指示器元素列表</summary>
        private List<DotElem> mDotElemList = new List<DotElem>();
        #endregion

        #region 属性
        /// <summary>列表项预制体配置列表</summary>
        public List<ItemPrefabConfData> ItemPrefabDataList => mItemPrefabDataList;
        /// <summary>当前显示中的列表项集合</summary>
        public List<AorListViewItem> ItemList => mItemList;
        /// <summary>是否为垂直列表</summary>
        public bool IsVertList => mIsVertList;
        /// <summary>列表项总数</summary>
        public int ItemTotalCount => mItemTotalCount;
        /// <summary>容器（content）节点</summary>
        public RectTransform ContainerTrans => mContainerTrans;
        /// <summary>ScrollRect 组件</summary>
        public ScrollRect ScrollRect => mScrollRect;
        /// <summary>当前是否正在拖拽</summary>
        public bool IsDraging => mIsDraging;
        /// <summary>是否启用项吸附</summary>
        public bool ItemSnapEnable { get => mItemSnapEnable; set => mItemSnapEnable = value; }
        /// <summary>是否支持滚动条</summary>
        public bool SupportScrollBar { get => mSupportScrollBar; set => mSupportScrollBar = value; }
        /// <summary>吸附移动默认最大绝对速度</summary>
        public float SnapMoveDefaultMaxAbsVec { get => mSnapMoveDefaultMaxAbsVec; set => mSnapMoveDefaultMaxAbsVec = value; }
        /// <summary>当前吸附最近的项索引</summary>
        public int CurSnapNearestItemIndex => mCurSnapNearestItemIndex;
        /// <summary>当前显示中的项数量</summary>
        public int ShownItemCount => mItemList.Count;
        /// <summary>视口尺寸（垂直取高、水平取宽）</summary>
        public float ViewPortSize => mIsVertList ? mViewPortRectTransform.rect.height : mViewPortRectTransform.rect.width;
        /// <summary>视口宽度</summary>
        public float ViewPortWidth => mViewPortRectTransform.rect.width;
        /// <summary>视口高度</summary>
        public float ViewPortHeight => mViewPortRectTransform.rect.height;
        #endregion

        #region 公共方法
        /// <summary>
        /// 按预制体名获取其配置数据
        /// </summary>
        /// <param name="prefabName">列表项预制体名</param>
        /// <returns>匹配的配置数据；未找到返回 null</returns>
        public ItemPrefabConfData GetItemPrefabConfData(string prefabName)
        {
            foreach (var data in mItemPrefabDataList)
            {
                if (data.mItemPrefab == null) continue;
                if (data.mItemPrefab.name == prefabName) return data;
            }
            return null;
        }

        /// <summary>
        /// 列表项预制体内容变更后重建对应对象池，并尝试恢复到首个显示项与位置
        /// </summary>
        /// <param name="prefabName">发生变更的预制体名</param>
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

        /// <summary>
        /// 初始化列表：绑定参数、核心组件、对象池与滚动条监听
        /// </summary>
        /// <param name="itemTotalCount">列表项总数</param>
        /// <param name="onGetItemByIndex">按索引获取列表项的回调</param>
        /// <param name="param">可选的自定义初始化参数，为 null 时使用默认值</param>
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
            m_AorItemPosMgr = new AorItemPosMgr(mItemDefaultWithPaddingSize);
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
            m_AorItemPosMgr.SetItemMaxCount(mSupportScrollBar ? mItemTotalCount : 0);
            UpdateContentSize();
        }

        /// <summary>
        /// 重置列表视口缓存位置，必要时将容器复位到原点
        /// </summary>
        /// <param name="resetPos">是否同时将容器位置复位为原点</param>
        public void ResetListView(bool resetPos = true)
        {
            mViewPortRectTransform.GetLocalCorners(mViewPortRectLocalCorners);
            if (resetPos) mContainerTrans.anchoredPosition3D = Vector3.zero;
            ForceSnapUpdateCheck();
        }

        /// <summary>
        /// 设置列表项总数并按需复位位置或刷新当前显示
        /// </summary>
        /// <param name="count">列表项总数</param>
        /// <param name="resetPos">是否复位到首项位置</param>
        public void SetListItemCount(int count, bool resetPos = true)
        {
            mCurSnapData.Clear();
            mItemTotalCount = count;
            mSupportScrollBar = count >= 0;
            m_AorItemPosMgr.SetItemMaxCount(mSupportScrollBar ? count : 0);

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

        /// <summary>
        /// 按项索引获取当前正在显示的列表项
        /// </summary>
        /// <param name="index">项索引</param>
        /// <returns>对应列表项；不在当前显示范围内返回 null</returns>
        public AorListViewItem GetShownItemByItemIndex(int index)
        {
            if (mItemList.Count == 0) return null;
            var first = mItemList[0].ItemIndex;
            var last = mItemList[^1].ItemIndex;
            if (index < first || index > last) return null;
            return mItemList[index - first];
        }

        /// <summary>
        /// 从对象池取出一个指定预制体的列表项并挂到容器下
        /// </summary>
        /// <param name="prefabName">列表项预制体名</param>
        /// <returns>取出的列表项；无对应对象池返回 null</returns>
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

        /// <summary>
        /// 某列表项尺寸变化后更新位置缓存并刷新内容尺寸与显示位置
        /// </summary>
        /// <param name="index">发生尺寸变化的项索引</param>
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

        /// <summary>
        /// 移动面板使指定索引项显示在起始位置（清空现有显示后重建）
        /// </summary>
        /// <param name="index">目标项索引</param>
        /// <param name="offset">位置偏移</param>
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

        /// <summary>
        /// 刷新当前所有显示中的列表项数据（保持首项索引不变）
        /// </summary>
        public void RefreshAllShownItem()
        {
            if (mItemList.Count == 0) return;
            RefreshAllShownItemWithFirstIndex(mItemList[0].ItemIndex);
        }

        /// <summary>
        /// 以指定首项索引重建显示中的列表项
        /// </summary>
        /// <param name="first">首项索引</param>
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

        /// <summary>
        /// 以指定首项索引与位置重建显示中的单个列表项
        /// </summary>
        /// <param name="first">首项索引</param>
        /// <param name="pos">起始位置</param>
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

        /// <summary>
        /// 强制触发一次吸附更新检查
        /// </summary>
        /// <remarks>
        /// 原实现仅复位内部帧计数 mLeftSnapUpdateExtraCount；该字段经确认从未被读取（CS0414），
        /// 已随未使用字段清理删除。方法对外保留、签名不变，行为等价于空操作。
        /// </remarks>
        public void ForceSnapUpdateCheck()
        {
            // 遗留方法：原仅复位未被读取的内部帧计数，现保持空操作以维持对外调用契约
        }

        /// <summary>
        /// 清空当前吸附数据
        /// </summary>
        public void ClearSnapData() => mCurSnapData.Clear();

        /// <summary>
        /// 设置吸附目标项索引（自动夹紧到合法范围）
        /// </summary>
        /// <param name="index">目标项索引</param>
        /// <param name="maxVec">吸附移动允许的最大绝对速度，默认 -1 表示不限制</param>
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
        /// <summary>
        /// 将单个列表项回收到其所属对象池
        /// </summary>
        /// <param name="item">待回收的列表项</param>
        private void RecycleItemTmp(AorListViewItem item)
        {
            if (item == null || string.IsNullOrEmpty(item.ItemPrefabName)) return;
            if (mItemPoolDict.TryGetValue(item.ItemPrefabName, out var pool)) pool.RecycleItem(item);
        }

        /// <summary>
        /// 清空所有对象池的临时回收列表
        /// </summary>
        private void ClearAllTmpRecycledItem()
        {
            foreach (var p in mItemPoolList) p.ClearTmpRecycledItem();
        }

        /// <summary>
        /// 回收当前所有显示中的列表项并清空显示列表
        /// </summary>
        private void RecycleAllItem()
        {
            foreach (var i in mItemList) RecycleItemTmp(i);
            mItemList.Clear();
        }

        /// <summary>
        /// 依据预制体配置初始化所有对象池（含预热创建与锚点/轴心调整）
        /// </summary>
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
        /// <summary>
        /// 按排列方向调整容器轴心（pivot）
        /// </summary>
        /// <param name="rt">待调整的 RectTransform</param>
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

        /// <summary>
        /// 调整轴心（复用容器轴心调整逻辑）
        /// </summary>
        /// <param name="rt">待调整的 RectTransform</param>
        private void AdjustPivot(RectTransform rt) => AdjustContainerPivot(rt);

        /// <summary>
        /// 按排列方向调整锚点（anchorMin / anchorMax）
        /// </summary>
        /// <param name="rt">待调整的 RectTransform</param>
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
        /// <summary>
        /// 开始拖拽回调（仅响应左键）
        /// </summary>
        /// <param name="e">拖拽事件数据</param>
        public void OnBeginDrag(PointerEventData e)
        {
            if (e.button != PointerEventData.InputButton.Left) return;
            mIsDraging = true;
            mCurSnapData.Clear();
            mOnBeginDragAction?.Invoke();
        }

        /// <summary>
        /// 结束拖拽回调（仅响应左键）
        /// </summary>
        /// <param name="e">拖拽事件数据</param>
        public void OnEndDrag(PointerEventData e)
        {
            if (e.button != PointerEventData.InputButton.Left) return;
            mIsDraging = false;
            mOnEndDragAction?.Invoke();
            ForceSnapUpdateCheck();
        }

        /// <summary>
        /// 拖拽中回调（仅响应左键）
        /// </summary>
        /// <param name="e">拖拽事件数据</param>
        public void OnDrag(PointerEventData e)
        {
            if (e.button != PointerEventData.InputButton.Left) return;
            mOnDragingAction?.Invoke();
        }
        #endregion

        #region 私有方法 - 项 & 位置
        /// <summary>
        /// 按索引获取一个新列表项（含边界校验并写入其项索引）
        /// </summary>
        /// <param name="index">项索引</param>
        /// <returns>列表项；越界或回调未提供时返回 null</returns>
        private AorListViewItem GetNewItemByIndex(int index)
        {
            if (mSupportScrollBar && index < 0) return null;
            if (mItemTotalCount > 0 && index >= mItemTotalCount) return null;
            var item = OnGetItemByIndex?.Invoke(index);
            if (item != null) item.ItemIndex = index;
            return item;
        }

        /// <summary>
        /// 设置指定项的尺寸（含间距）并记录最近一次设置的项索引与间距
        /// </summary>
        /// <param name="index">项索引</param>
        /// <param name="size">项尺寸</param>
        /// <param name="padding">项间距</param>
        private void SetItemSize(int index, float size, float padding)
        {
            m_AorItemPosMgr.SetItemSize(index, size + padding);
            mLastItemIndex = index;
            mLastItemPadding = padding;
        }

        /// <summary>
        /// 获取列表项指定角点在视口本地坐标系中的位置
        /// </summary>
        /// <param name="item">列表项</param>
        /// <param name="corner">角点枚举</param>
        /// <returns>视口本地坐标</returns>
        public Vector3 GetItemCornerPosInViewPort(AorListViewItem item, ItemCornerEnum corner = ItemCornerEnum.LeftBottom)
        {
            item.CachedRectTransform.GetWorldCorners(mItemWorldCorners);
            return mViewPortRectTransform.InverseTransformPoint(mItemWorldCorners[(int)corner]);
        }
        #endregion

        #region 私有方法 - 分页视图
        /// <summary>
        /// 圆点被点击：若目标页与当前页不同则吸附过去
        /// </summary>
        /// <param name="index">目标页索引</param>
        private void OnDotClicked(int index)
        {
            int cur = CurSnapNearestItemIndex;
            if (cur == index) return;
            SetSnapTargetItemIndex(index);
        }

        /// <summary>
        /// 分页视图结束拖拽：按速度决定吸附到当前页或相邻页
        /// </summary>
        private void OnPageEndDrag()
        {
            var v = ScrollRect.velocity.x;
            var idx = CurSnapNearestItemIndex;
            if (Mathf.Abs(v) < 50) SetSnapTargetItemIndex(idx);
            else SetSnapTargetItemIndex(v > 0 ? idx - 1 : idx + 1);
        }

        /// <summary>
        /// 根据当前最近项索引刷新所有圆点的选中态
        /// </summary>
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
        /// <summary>
        /// 为列表滚动条按下/弹起绑定吸附清理与检查回调
        /// </summary>
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
        /// <summary>
        /// 吸附移动总入口：按布局方向分发到垂直/水平吸附
        /// </summary>
        /// <param name="immediate">是否立即吸附到位</param>
        /// <param name="force">是否强制吸附</param>
        private void UpdateSnapMove(bool immediate = false, bool force = false)
        {
            if (!mItemSnapEnable) return;
            if (mIsVertList) UpdateSnapVertical(immediate, force);
            else UpdateSnapHorizontal(immediate, force);
        }

        /// <summary>
        /// 垂直列表吸附移动（预留实现）
        /// </summary>
        /// <param name="imm">是否立即吸附到位</param>
        /// <param name="force">是否强制吸附</param>
        private void UpdateSnapVertical(bool imm = false, bool force = false) { }

        /// <summary>
        /// 水平列表吸附移动（预留实现）
        /// </summary>
        /// <param name="imm">是否立即吸附到位</param>
        /// <param name="force">是否强制吸附</param>
        private void UpdateSnapHorizontal(bool imm = false, bool force = false) { }
        #endregion

        #region 私有方法 - 列表更新
        /// <summary>
        /// 驱动列表更新循环：按回收/创建距离反复迭代直到稳定
        /// </summary>
        /// <param name="dr0">纵向回收距离</param>
        /// <param name="dr1">横向回收距离</param>
        /// <param name="dn0">纵向新建距离</param>
        /// <param name="dn1">横向新建距离</param>
        public void UpdateListView(float dr0, float dr1, float dn0, float dn1)
        {
            int loop = 0;
            bool cont;
            do { cont = mIsVertList ? UpdateForVertList(dr0, dr1, dn0, dn1) : UpdateForHorizontalList(dr0, dr1, dn0, dn1); }
            while (cont && loop++ < 1000);
        }

        /// <summary>
        /// 垂直列表单步更新（预留实现，暂不改变显示）
        /// </summary>
        /// <returns>是否需要继续迭代</returns>
        private bool UpdateForVertList(float dr0, float dr1, float dn0, float dn1) => false;

        /// <summary>
        /// 水平列表单步更新（预留实现，暂不改变显示）
        /// </summary>
        /// <returns>是否需要继续迭代</returns>
        private bool UpdateForHorizontalList(float dr0, float dr1, float dn0, float dn1) => false;

        /// <summary>
        /// 刷新所有显示中列表项的位置（当前仅实现垂直布局）
        /// </summary>
        private void UpdateAllShownItemsPos()
        {
            if (mItemList.Count == 0) return;
            if (mIsVertList)
            {
                float y = mSupportScrollBar ? -m_AorItemPosMgr.GetItemPos(mItemList[0].ItemIndex) : 0;
                foreach (var i in mItemList)
                {
                    i.CachedRectTransform.anchoredPosition3D = new Vector3(i.StartPosOffset, y, 0);
                    y -= i.CachedRectTransform.rect.height + i.Padding;
                }
            }
        }

        /// <summary>
        /// 依据内容尺寸更新容器大小
        /// </summary>
        private void UpdateContentSize()
        {
            float size = GetContentPanelSize();
            if (mIsVertList) mContainerTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size);
            else mContainerTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size);
        }

        /// <summary>
        /// 计算内容面板总尺寸
        /// </summary>
        /// <returns>内容总尺寸</returns>
        private float GetContentPanelSize()
        {
            if (mSupportScrollBar) return Mathf.Max(m_AorItemPosMgr.mTotalSize - mLastItemPadding, 0);
            if (mItemList.Count == 0) return 0;
            float s = 0;
            for (int i = 0; i < mItemList.Count - 1; i++) s += mItemList[i].ItemSizeWithPadding;
            s += mItemList[^1].ItemSize;
            return s;
        }
        #endregion

        #region 生命周期
        /// <summary>
        /// 启动：分页视图模式下绑定圆点点击并注册结束拖拽回调，最后触发启动事件
        /// </summary>
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

        /// <summary>
        /// 每帧更新：驱动位置缓存、吸附与列表回收/创建，最后清理临时回收项
        /// </summary>
        private void Update()
        {
            if (!mListViewInited) return;
            if (mSupportScrollBar) m_AorItemPosMgr.Update(false);
            UpdateSnapMove();
            UpdateListView(mDistanceForRecycle0, mDistanceForRecycle1, mDistanceForNew0, mDistanceForNew1);
            ClearAllTmpRecycledItem();
        }
        #endregion
    }
}