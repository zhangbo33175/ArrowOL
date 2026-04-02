#if EASY_TOUCH_ENABLE
namespace Honor.Runtime
{
    using HedgehogTeam.EasyTouch;
    using System.Collections.Generic;
    using UnityEngine;

    public sealed partial class Gestures2D : MonoBehaviour
    {
        /// <summary>
        /// 场景相机在场景相机列表中的index
        /// </summary>
        [SerializeField]
        [HonorTitle("场景相机索引值")]
        private int m_SceneCameraIndex;
        public int SceneCameraIndex
        {
            set
            {
                if (value < 0 || value >= Root.Scene.SceneCameras.Count)
                {
                    Log.Fatal("SceneCameraIndex 无效。");
                    return;
                }
                m_SceneCameraIndex = value;
                m_SceneCamera = Root.Scene.SceneCameras[m_SceneCameraIndex];
                IsOrthographic = m_IsOrthographic;
            }
            get
            {
                return m_SceneCameraIndex;
            }
        }

        /// <summary>
        /// 是否为正交场景相机类型
        /// </summary>
        [SerializeField]
        [HonorTitle("正交场景相机类型(IsOrthographic)")]
        private bool m_IsOrthographic;
        public bool IsOrthographic
        {
            set
            {
                m_IsOrthographic = value;
                m_SceneCamera.orthographic = m_IsOrthographic;
            }
            get
            {
                return m_IsOrthographic;
            }
        }

        /// <summary>
        /// 手势距离透视相机Z轴距离
        /// </summary>
        [SerializeField]
        [HonorTitle("手势距离相机Z轴距离(GestureCameraDistance)")]
        private float m_GestureCameraDistance;
        public float GestureCameraDistance
        {
            set
            {
                m_GestureCameraDistance = value;
            }
            get
            {
                return m_GestureCameraDistance;
            }
        }

        /// <summary>
        /// 总开关
        /// </summary>
        [SerializeField]
        [HonorTitle("总开关 (EnableSwitch)")]
        private bool m_EnableSwitch;
        public bool EnableSwitch
        {
            set
            {
                m_EnableSwitch = value;
                enabled = m_EnableSwitch;
            }
            get
            {
                return m_EnableSwitch;
            }
        }

        /// <summary>
        ///【滑动】滑动开关
        /// </summary>
        [SerializeField]
        [HonorTitle("滑动开关 (SwipeSwitch)")]
        private bool m_SwipeSwitch;
        public bool SwipeSwitch
        {
            set
            {
                m_SwipeSwitch = value;
            }
            get
            {
                return m_SwipeSwitch;
            }
        }

        /// <summary>
        ///【缩放】缩放开关
        /// </summary>
        [SerializeField]
        [HonorTitle("缩放开关 (PinchSwitch)")]
        private bool m_PinchSwitch;
        public bool PinchSwitch
        {
            set
            {
                m_PinchSwitch = value;
            }
            get
            {
                return m_PinchSwitch;
            }
        }

        /// <summary>
        ///【缩放】鼠标滚轮缩放开关
        /// </summary>
        [SerializeField]
        [HonorTitle("鼠标滚轮缩放开关 (MouseWheelPinchSwitch)")]
        private bool m_MouseWheelPinchSwitch;
        public bool MouseWheelPinchSwitch
        {
            set
            {
                m_MouseWheelPinchSwitch = value;
            }
            get
            {
                return m_MouseWheelPinchSwitch;
            }
        }

        /// <summary>
        ///【选中】选中开关
        /// </summary>
        [SerializeField]
        [HonorTitle("选中开关 (SelectSwitch)")]
        private bool m_SelectSwitch;
        public bool SelectSwitch
        {
            set
            {
                m_SelectSwitch = value;
            }
            get
            {
                return m_SelectSwitch;
            }
        }

        /// <summary>
        ///【拖拽】拖拽开关
        /// </summary>
        [SerializeField]
        [HonorTitle("拖拽开关 (DragSwitch)")]
        private bool m_DragSwitch;
        public bool DragSwitch
        {
            set
            {
                m_DragSwitch = value;
            }
            get
            {
                return m_DragSwitch;
            }
        }

        /// <summary>
        /// 有效空间内中心位置
        /// </summary>
        [SerializeField]
        [HonorTitle("有效空间内中心位置 (SpaceCenterPosition)")]
        private Vector2 m_SpaceCenterPosition = Vector2.zero;
        public Vector2 SpaceCenterPosition
        {
            set
            {
                m_SpaceCenterPosition = value;
            }
            get
            {
                return m_SpaceCenterPosition;
            }
        }

        /// <summary>
        /// 有效空间内横向总长度
        /// </summary>
        [SerializeField]
        [HonorTitle("有效空间内横向总长度 (SpaceHorizontalLength)")]
        private float m_SpaceHorizontalLength = 30f;
        public float SpaceHorizontalLength
        {
            set
            {
                m_SpaceHorizontalLength = value;
            }
            get
            {
                return m_SpaceHorizontalLength;
            }
        }

        /// <summary>
        /// 有效空间内纵向总长度
        /// </summary>
        [SerializeField]
        [HonorTitle("有效空间内纵向总长度 (SpaceVerticalLength)")]
        private float m_SpaceVerticalLength = 30f;
        public float SpaceVerticalLength
        {
            set
            {
                m_SpaceVerticalLength = value;
            }
            get
            {
                return m_SpaceVerticalLength;
            }
        }

        /// <summary>
        /// 有效空间内水平方向单侧边缘弹性区长度
        /// </summary>
        [SerializeField]
        [HonorTitle("有效空间内水平方向单侧边缘弹性区长度 (SpaceHorizontalEdgeMoveElasticLength)")]
        private float m_SpaceHorizontalEdgeMoveElasticLength = 10f;
        public float SpaceHorizontalEdgeMoveElasticLength
        {
            set
            {
                m_SpaceHorizontalEdgeMoveElasticLength = value;
            }
            get
            {
                return m_SpaceHorizontalEdgeMoveElasticLength;
            }
        }
        
        /// <summary>
        /// 有效空间内垂直方向单侧边缘弹性区长度
        /// </summary>
        [SerializeField]
        [HonorTitle("有效空间内垂直方向单侧边缘弹性区长度 (SpaceVerticalEdgeMoveElasticLength)")]
        private float m_SpaceVerticalEdgeMoveElasticLength = 10f;
        public float SpaceVerticalEdgeMoveElasticLength
        {
            set
            {
                m_SpaceVerticalEdgeMoveElasticLength = value;
            }
            get
            {
                return m_SpaceVerticalEdgeMoveElasticLength;
            }
        }

        /// <summary>
        /// 缩放因子
        /// </summary>
        [SerializeField]
        [HonorTitle("缩放因子 (PinchRatio > 0)")]
        private float m_PinchRatio = 150f;
        public float PinchRatio
        {
            set
            {
                m_PinchRatio = value;
            }
            get
            {
                return m_PinchRatio;
            }
        }

        /// <summary>
        /// 缩放最小值
        /// </summary>
        [SerializeField]
        [HonorTitle("缩放最小值 (PinchMinScale >= 1)")]
        private float m_PinchMinScale = 1f;
        public float PinchMinScale
        {
            set
            {
                m_PinchMinScale = value;
            }
            get
            {
                return m_PinchMinScale;
            }
        }

        /// <summary>
        /// 缩放最大值
        /// </summary>
        [SerializeField]
        [HonorTitle("缩放最大值 (PinchMaxScale >= 1)")]
        private float m_PinchMaxScale = 13f;
        public float PinchMaxScale
        {
            set
            {
                m_PinchMaxScale = value;
            }
            get
            {
                return m_PinchMaxScale;
            }
        }

        /// <summary>
        /// 鼠标滚轮缩放偏移量
        /// </summary>
        [SerializeField]
        [HonorTitle("鼠标滚轮缩放偏移量 (MouseWheelPinchOffset <= 0)")]
        private float m_MouseWheelPinchOffset = -400f;
        public float MouseWheelPinchOffset
        {
            set
            {
                m_MouseWheelPinchOffset = value;
            }
            get
            {
                return m_MouseWheelPinchOffset;
            }
        }

        /// <summary>
        /// 在缩放最小值与最大值边缘的弹性区大小
        /// </summary>
        [SerializeField]
        [HonorTitle("有效空间内缩放最值边缘的弹性区大小 (SpaceEdgeScaleElasticValue)")]
        private float m_SpaceEdgeScaleElasticValue = 1f;
        public float SpaceEdgeScaleElasticValue
        {
            set
            {
                m_SpaceEdgeScaleElasticValue = value;
            }
            get
            {
                return m_SpaceEdgeScaleElasticValue;
            }
        }

        /// <summary>
        /// 滑动或缩放手势结束后的安全时间间隔
        /// </summary>
        [SerializeField]
        [HonorTitle("滑动或缩放手势结束后的安全时间间隔 (SafeTimeOnGestureOver)")]
        private float m_SafeTimeOnGestureOver = 0.04f;
        public float SafeTimeOnGestureOver
        {
            set
            {
                m_SafeTimeOnGestureOver = value;
            }
            get
            {
                return m_SafeTimeOnGestureOver;
            }
        }

        /// <summary>
        /// 常选中模式
        /// 常选中模式：具体流程：选中->开始拖拽->拖拽中->结束拖拽->切换目标->结束选中
        /// 非常选中模式：具体流程：选中->开始拖拽->拖拽中->结束拖拽->结束选中
        /// </summary>
        [SerializeField]
        [HonorTitle("常选中模式（SelectHoldMode）")]
        private bool m_SelectHoldMode;
        public bool SelectHoldMode
        {
            set
            {
                m_SelectHoldMode = value;
            }
            get
            {
                return m_SelectHoldMode;
            }
        }

        /// <summary>
        /// 常选中模式下选中反弹
        /// 常选中模式下：当对象处于被选中状态时，再次点击可以将选中状态反弹回未选中状态
        /// </summary>
        [SerializeField]
        [HonorTitle("常选中模式下选中反弹机制（SelectReboundInSelectHoldMode）")]
        private bool m_SelectReboundInSelectHoldMode;
        public bool SelectReboundInSelectHoldMode
        {
            set
            {
                m_SelectReboundInSelectHoldMode = value;
            }
            get
            {
                return m_SelectReboundInSelectHoldMode;
            }
        }

        /// <summary>
        /// 常选中模式下拖拽结束后反弹
        /// 常选中模式下：当对象拖拽结束时，可以将选中状态反弹回未选中状态
        /// </summary>
        [SerializeField]
        [HonorTitle("常选中模式下拖拽结束后反弹机制（SelectReboundAfterDragEndInSelectHoldMode）")]
        private bool m_SelectReboundAfterDragEndInSelectHoldMode;
        public bool SelectReboundAfterDragEndInSelectHoldMode
        {
            set
            {
                m_SelectReboundAfterDragEndInSelectHoldMode = value;
            }
            get
            {
                return m_SelectReboundAfterDragEndInSelectHoldMode;
            }
        }

        /// <summary>
        /// 选中对象时所需按压等待时长
        /// </summary>
        [SerializeField]
        [HonorTitle("选中对象时所需按压等待时长 (PressTimeOfSelectingObj)")]
        private float m_PressTimeOfSelectingObj = 0f;
        public float PressTimeOfSelectingObj
        {
            set
            {
                m_PressTimeOfSelectingObj = value;
            }
            get
            {
                return m_PressTimeOfSelectingObj;
            }
        }

        /// <summary>
        /// 2D/3D碰撞器选中检测
        /// </summary>
        [SerializeField]
        [HonorTitle("2D/3D碰撞器选中检测 (ColliderDetectMode)")]
        private Definitions.DimensionMode m_ColliderDetectMode;
        public Definitions.DimensionMode ColliderDetectMode
        {
            set
            {
                m_ColliderDetectMode = value;
            }
            get
            {
                return m_ColliderDetectMode;
            }
        }

        /// <summary>
        /// 开启Y坐标选中排序
        /// </summary>
        [SerializeField]
        [HonorTitle("开启Y坐标选中排序 (PosYSortSelectSwitch)")]
        private bool m_PosYSortSelectSwitch = true;
        public bool PosYSortSelectSwitch
        {
            set
            {
                m_PosYSortSelectSwitch = value;
            }
            get
            {
                return m_PosYSortSelectSwitch;
            }
        }

        /// <summary>
        /// 开启RayDistance选中排序
        /// </summary>
        [SerializeField]
        [HonorTitle("开启RayDistance选中排序 (RayDistanceSortSelectSwitch)")]
        private bool m_RayDistanceSortSelectSwitch = true;
        public bool RayDistanceSortSelectSwitch
        {
            set
            {
                m_RayDistanceSortSelectSwitch = value;
            }
            get
            {
                return m_RayDistanceSortSelectSwitch;
            }
        }

        /// <summary>
        /// 开启SortingLayerOrder选中排序
        /// </summary>
        [SerializeField]
        [HonorTitle("开启SortingLayerOrder选中排序 (SortingLayerOrderSortSelectSwitch)")]
        private bool m_SortingLayerOrderSortSelectSwitch = true;
        public bool SortingLayerOrderSortSelectSwitch
        {
            set
            {
                m_SortingLayerOrderSortSelectSwitch = value;
            }
            get
            {
                return m_SortingLayerOrderSortSelectSwitch;
            }
        }

        /// <summary>
        /// 选中拖拽行为的LuaBehaviour中的Lua类名集合
        /// 必须按照响应优先级从高到底的顺序设置
        /// </summary>
        [SerializeField]
        [HonorTitle("选中拖拽行为的Lua类名 (SelectingTypes)")]
        private List<string> m_SelectingTypes;
        public List<string> SelectingTypes
        {
            set
            {
                m_SelectingTypes = value;
            }
            get
            {
                return m_SelectingTypes;
            }
        }

        /// <summary>
        /// 选中拖拽行为的LuaBehaviour中的Lua类名-在自身中查询"
        /// </summary>
        [SerializeField]
        [HonorTitle("选中拖拽行为的Lua类名-在自身中查询 (FindSelectingTypesOnSelf)")]
        private bool m_FindSelectingTypesOnSelf = false;
        public bool FindSelectingTypesOnSelf
        {
            set
            {
                m_FindSelectingTypesOnSelf = value;
            }
            get
            {
                return m_FindSelectingTypesOnSelf;
            }
        }


        /// <summary>
        /// 选中拖拽行为的LuaBehaviour中的Lua类名-在父对象中查询
        /// </summary>
        [SerializeField]
        [HonorTitle("选中拖拽行为的Lua类名-在父对象中查询 (FindSelectingTypesOnParent)")]
        private bool m_FindSelectingTypesOnParent = false;
        public bool FindSelectingTypesOnParent
        {
            set
            {
                m_FindSelectingTypesOnParent = value;
            }
            get
            {
                return m_FindSelectingTypesOnParent;
            }
        }

        /// <summary>
        /// 选中拖拽行为的LuaBehaviour中的Lua类名-在子对象中查询
        /// </summary>
        [SerializeField]
        [HonorTitle("选中拖拽行为的Lua类名-在子对象中查询 (FindSelectingTypesOnChildren)")]
        private bool m_FindSelectingTypesOnChildren = false;
        public bool FindSelectingTypesOnChildren
        {
            set
            {
                m_FindSelectingTypesOnChildren = value;
            }
            get
            {
                return m_FindSelectingTypesOnChildren;
            }
        }

        /// <summary>
        /// Lua组件
        /// </summary>
        private LuaComponent m_LuaComponent;

        /// <summary>
        /// 场景相机
        /// </summary>
        private Camera m_SceneCamera;
        public Camera SceneCamera
        {
            get
            {
                return m_SceneCamera;
            }
        }

        /// <summary>
        ///【滑动】滑动坐标
        /// </summary>
        private Vector2 m_SwipePosition = Vector2.zero;

        /// <summary>
        ///【滑动】滑动偏移量
        /// </summary>
        private Vector2 m_SwipeOffset = Vector2.zero;

        /// <summary>
        ///【滑动】是否正在进行基于手指的滑动中
        /// </summary>
        private bool m_IsFingerSwiping = false;

        /// <summary>
        ///【滑动】滑动惯性稳定回调是否已经结束
        /// </summary>
        private bool m_IsSwipeStableCallbackOver = true;

        /// <summary>
        ///【缩放】缩放的屏幕中心点坐标
        /// </summary>
        private Vector3 m_PinchScreenCenterPosition = Vector3.zero;

        /// <summary>
        ///【缩放】缩放的世界中心点坐标
        /// </summary>
        private Vector3 m_PinchWorldCenterPosition = Vector3.zero;

        /// <summary>
        ///【缩放】缩放双指旧距离长度
        /// </summary>
        private float m_PinchFingersOldDistance = 0f;

        /// <summary>
        ///【缩放】是否跳过第一帧的缩放
        /// </summary>
        private bool m_SkipFirstPinchFrame = true;

        /// <summary>
        ///【缩放】目标弹性缩放值
        /// </summary>
        private float m_TargetScaleElasticValue = -1f;

        /// <summary>
        ///【缩放】缩放惯性稳定回调是否已经结束
        /// </summary>
        private bool m_IsPinchStableCallbackOver = true;

        /// <summary>
        ///【滑动、缩放】本轮是否已经滑动或缩放过
        /// </summary>
        private bool m_AlreadySwipedOrPinchedOnThisRound = false;

        /// <summary>
        ///【滑动、缩放】滑动缩放手势结束后的安全时间间隔计数器
        /// </summary>
        private float m_SafeTimeCounterOnGestureOver = 0f;

        /// <summary>
        /// 当前选中的对象
        /// </summary>
        private GameObject m_SelectedObj;

        /// <summary>
        /// 当前选中的对象的Lua类型
        /// </summary>
        private string m_SelectedObjType;

        /// <summary>
        /// 当前选中的对象的手势
        /// </summary>
        private Gesture m_SelectedObjGesture;

        /// <summary>
        /// 是否可以进入反弹状态
        /// </summary>
        private bool m_CanEnterSelectReboundInSelectHoldMode = true;

        /// <summary>
        /// 正否正在拖拽中
        /// </summary>
        private bool m_IsDraging = false;

        /// <summary>
        /// 上一次拖拽Obj手势所在世界坐标
        /// </summary>
        private Vector3 m_LastWorldPosition = Vector3.zero;

        /// <summary>
        /// 因滑动导致忽略选中物体手势的标记位
        /// </summary>
        private bool m_IgnoreSelectObjBySwipe = false;

        /// <summary>
        /// 因为缩放导致忽略选中物体手势的标记位
        /// </summary>
        private bool m_IgnoreSelectObjByPinch = false;
    }
}
#endif
