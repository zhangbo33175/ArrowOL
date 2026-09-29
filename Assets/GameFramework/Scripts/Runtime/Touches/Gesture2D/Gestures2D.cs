/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  Gestures2D.cs
 * author:    云毅
 * created:   2026
 * descrip:   2D相机手势控制器，基于EasyTouch实现触摸/滑动/缩放/拖拽
 ***************************************************************/
#if EASY_TOUCH_ENABLE
using UnityEngine.EventSystems;
namespace Honor.Runtime
{
    using DG.Tweening;
    using HedgehogTeam.EasyTouch;
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using XLua;

    //=========================================================================
    // 2D 相机手势控制器
    //=========================================================================
    /// <summary>
    /// 2D 相机手势控制器
    /// 基于 EasyTouch 实现：单指滑动、双指缩放、边界弹性、对象选中与拖拽
    /// 支持 Lua 回调，适用于 2D/UI 相机控制
    /// </summary>
    public sealed partial class Gestures2D : MonoBehaviour
    {
        #region 生命周期初始化
        /// <summary>
        /// 初始化：创建所有 Lua 回调列表
        /// </summary>
        void Awake()
        {
            m_TouchBeginCallbacks = new List<LuaTable>();
            m_TouchDownCallbacks = new List<LuaTable>();
            m_TouchEndCallbacks = new List<LuaTable>();
            m_TouchesBeginCallbacks = new List<LuaTable>();
            m_TouchesDownCallbacks = new List<LuaTable>();
            m_TouchesEndCallbacks = new List<LuaTable>();
            m_SwipeBeginCallbacks = new List<LuaTable>();
            m_SwipeCallbacks = new List<LuaTable>();
            m_SwipeEndCallbacks = new List<LuaTable>();
            m_SwipeStableCallbacks = new List<LuaTable>();
            m_PinchCallbacks = new List<LuaTable>();
            m_PinchStableCallbacks = new List<LuaTable>();
            m_SelectedObjCallbacks = new List<LuaTable>();
            m_UpdateSelectedObjCallbacks = new List<LuaTable>();
            m_UnselectedObjCallbacks = new List<LuaTable>();
            m_SelectedObjDragBeginCallbacks = new List<LuaTable>();
            m_SelectedObjDragCallbacks = new List<LuaTable>();
            m_SelectedObjDragEndCallbacks = new List<LuaTable>();
        }

        /// <summary>
        /// 启动：获取 Lua 环境组件
        /// </summary>
        void Start()
        {
            m_LuaComponent = GameComponentsGroup.GetComponent<LuaComponent>();
            if (m_LuaComponent == null)
            {
                Log.Fatal("Lua Component 无效。");
                return;
            }
        }

        /// <summary>
        /// 启用时注册所有手势事件
        /// </summary>
        void OnEnable()
        {
            // 清理缓存数据
            CleanCaches();

            // 单指-点击手势-回调注册
            EasyTouch.On_TouchStart += OnTouchBegin;
            EasyTouch.On_TouchDown += OnTouchDown;
            EasyTouch.On_TouchUp += OnTouchEnd;

            // 双指-点击手势-回调注册
            EasyTouch.On_TouchStart2Fingers += OnTouchesBegin;
            EasyTouch.On_TouchDown2Fingers += OnTouchesDown;
            EasyTouch.On_TouchUp2Fingers += OnTouchesEnd;
            EasyTouch.On_Cancel2Fingers += OnTouchesEnd;

            // 单指-滑动手势-回调注册
            EasyTouch.On_SwipeStart += OnSwipeBegin;
            EasyTouch.On_Swipe += OnSwipe;
            EasyTouch.On_SwipeEnd += OnSwipeEnd;

            // 双指-缩放手势-回调注册
            EasyTouch.On_Pinch += OnPinch;
            EasyTouch.On_PinchIn += OnPinchIn;
            EasyTouch.On_PinchOut += OnPinchOut;

            // 拖拽对象-拖拽手势-回调注册
            EasyTouch.On_DragStart += OnDragBegin;
            EasyTouch.On_Drag += OnDrag;
            EasyTouch.On_DragEnd += OnDragEnd;
        }

        /// <summary>
        /// 禁用时注销所有手势事件
        /// </summary>
        void OnDisable()
        {
            // 单指手势回调注销
            EasyTouch.On_TouchStart -= OnTouchBegin;
            EasyTouch.On_TouchDown -= OnTouchDown;
            EasyTouch.On_TouchUp -= OnTouchEnd;

            // 双指手势回调注销
            EasyTouch.On_TouchStart2Fingers -= OnTouchesBegin;
            EasyTouch.On_TouchDown2Fingers -= OnTouchesDown;
            EasyTouch.On_TouchUp2Fingers -= OnTouchesEnd;
            EasyTouch.On_Cancel2Fingers -= OnTouchesEnd;

            // 单指-滑动手势-回调注销
            EasyTouch.On_SwipeStart -= OnSwipeBegin;
            EasyTouch.On_Swipe -= OnSwipe;
            EasyTouch.On_SwipeEnd -= OnSwipeEnd;

            // 双指-缩放手势-回调注销
            EasyTouch.On_Pinch -= OnPinch;
            EasyTouch.On_PinchIn -= OnPinchIn;
            EasyTouch.On_PinchOut -= OnPinchOut;

            // 拖拽对象-拖拽手势-回调注销
            EasyTouch.On_DragStart -= OnDragBegin;
            EasyTouch.On_Drag -= OnDrag;
            EasyTouch.On_DragEnd -= OnDragEnd;
        }
        #endregion

        #region 公开重置接口
        /// <summary>
        /// 手动复位滑动状态
        /// </summary>
        public void ResetSwipe()
        {
            m_IsFingerSwiping = false;
            m_SwipePosition = Vector2.zero;
            m_SwipeOffset = Vector2.zero;
            m_IgnoreSelectObjBySwipe = false;
            m_SafeTimeCounterOnGestureOver = m_SafeTimeOnGestureOver;
        }

        /// <summary>
        /// 手动复位缩放状态
        /// </summary>
        public void ResetPinch()
        {
            m_TargetScaleElasticValue = -1f;
            m_PinchFingersOldDistance = 0f;
            m_PinchScreenCenterPosition = Vector3.zero;
            m_PinchWorldCenterPosition = m_SceneCamera.ViewportToWorldPoint(m_SceneCamera.ScreenToViewportPoint(m_PinchScreenCenterPosition));
            m_IgnoreSelectObjByPinch = false;
            m_SafeTimeCounterOnGestureOver = m_SafeTimeOnGestureOver;
        }
        #endregion

        #region 单指触摸事件
        /// <summary>
        /// 单指触摸开始
        /// </summary>
        /// <param name="gesture">手势数据</param>
        private void OnTouchBegin(Gesture gesture)
        {
            // 修复特殊机型事件丢失问题
            FixSpecialDevicesEventLoses();

            DOTween.Kill(GameDOTweenTypes.CameraAnimation);
            DOTween.Kill(GameDOTweenTypes.CameraMoveAnimation);
            DOTween.Kill(GameDOTweenTypes.CameraRotateAnimation);
            DOTween.Kill(GameDOTweenTypes.CameraScaleAnimation);
            if (gesture.touchCount == 1)
            {
                Vector3 worldPosition = ScreenToGestureWorld(gesture.position);
                DispatchCallbacks(m_TouchBeginCallbacks, args =>
                {
                    args.Set("gesture", gesture);
                    args.Set("worldPosition", worldPosition);
                });
                CheckSelect(gesture);
            }
        }

        /// <summary>
        /// 单指按住中
        /// </summary>
        /// <param name="gesture">手势数据</param>
        private void OnTouchDown(Gesture gesture)
        {
            if (gesture.touchCount == 1)
            {
                Vector3 worldPosition = ScreenToGestureWorld(gesture.position);
                DispatchCallbacks(m_TouchDownCallbacks, args =>
                {
                    args.Set("gesture", gesture);
                    args.Set("worldPosition", worldPosition);
                });
            }
        }

        /// <summary>
        /// 单指触摸结束
        /// 处理选中/取消选中逻辑
        /// </summary>
        /// <param name="gesture">手势数据</param>
        private void OnTouchEnd(Gesture gesture)
        {
            if (gesture.touchCount == 1)
            {
                Vector3 worldPosition = ScreenToGestureWorld(gesture.position);
                DispatchCallbacks(m_TouchEndCallbacks, args =>
                {
                    args.Set("gesture", gesture);
                    args.Set("worldPosition", worldPosition);
                });

                if (m_SelectedObj != null && !string.IsNullOrEmpty(m_SelectedObjType))
                {
                    // 设置常选中模式下的反弹状态
                    if (m_SelectHoldMode && m_SelectReboundInSelectHoldMode)
                    {
                        if (m_IsDraging)
                        {
                            m_CanEnterSelectReboundInSelectHoldMode = false;
                        }
                        else
                        {
                            m_CanEnterSelectReboundInSelectHoldMode = !m_CanEnterSelectReboundInSelectHoldMode;
                        }
                    }

                    // 判断是否需要取消选中
                    if ((m_SelectHoldMode && m_SelectReboundInSelectHoldMode && m_CanEnterSelectReboundInSelectHoldMode) ||
                        (m_SelectHoldMode && m_SelectReboundAfterDragEndInSelectHoldMode && m_IsDraging) ||
                        (!m_SelectHoldMode))
                    {
                        m_IsFingerSwiping = false;

                        DispatchCallbacks(m_UnselectedObjCallbacks, args =>
                        {
                            args.Set("gesture", gesture);
                            args.Set("worldPosition", worldPosition);
                            args.Set("selectedObjType", m_SelectedObjType);
                            args.Set("selectedObj", m_SelectedObj);
                        });
                        m_SelectedObjType = null;
                        m_SelectedObj = null;
                        m_SelectedObjGesture = null;
                        if (m_SelectHoldMode)
                        {
                            m_CanEnterSelectReboundInSelectHoldMode = true;
                        }
                    }

                    m_IsDraging = false;
                }
            }
        }
        #endregion

        #region 双指触摸事件
        /// <summary>
        /// 双指触摸开始（中点坐标）
        /// </summary>
        /// <param name="gesture">手势数据</param>
        private void OnTouchesBegin(Gesture gesture)
        {
            if (gesture.touchCount == 2)
            {
                Vector3 worldPosition = ScreenToGestureWorld(gesture.position);
                DispatchCallbacks(m_TouchesBeginCallbacks, args =>
                {
                    args.Set("gesture", gesture);
                    args.Set("worldPosition", worldPosition);
                });

                m_IsFingerSwiping = false;
                m_SwipePosition = gesture.position;
                m_SwipeOffset = Vector2.zero;

                m_PinchFingersOldDistance = gesture.twoFingerDistance;
                m_PinchScreenCenterPosition = gesture.position;
                m_PinchWorldCenterPosition = m_SceneCamera.ViewportToWorldPoint(m_SceneCamera.ScreenToViewportPoint(m_PinchScreenCenterPosition));

                m_SkipFirstPinchFrame = true;
            }
        }

        /// <summary>
        /// 双指按住中（中点坐标）
        /// </summary>
        /// <param name="gesture">手势数据</param>
        private void OnTouchesDown(Gesture gesture)
        {
            if (gesture.touchCount == 2)
            {
                Vector3 worldPosition = ScreenToGestureWorld(gesture.position);
                DispatchCallbacks(m_TouchesDownCallbacks, args =>
                {
                    args.Set("gesture", gesture);
                    args.Set("worldPosition", worldPosition);
                });
            }
        }

        /// <summary>
        /// 双指触摸结束（中点坐标）
        /// </summary>
        /// <param name="gesture">手势数据</param>
        private void OnTouchesEnd(Gesture gesture)
        {
            if (gesture.touchCount == 2)
            {
                Vector3 worldPosition = ScreenToGestureWorld(gesture.position);
                DispatchCallbacks(m_TouchesEndCallbacks, args =>
                {
                    args.Set("gesture", gesture);
                    args.Set("worldPosition", worldPosition);
                });

                // 松手时回弹到边界
                SwipeToElasticOnReleased();

                m_PinchFingersOldDistance = gesture.twoFingerDistance;
                m_PinchScreenCenterPosition = gesture.position;
                m_PinchWorldCenterPosition = m_SceneCamera.ViewportToWorldPoint(m_SceneCamera.ScreenToViewportPoint(m_PinchScreenCenterPosition));

                m_SkipFirstPinchFrame = true;
            }
        }
        #endregion

        #region 滑动手势事件
        /// <summary>
        /// 单指滑动开始
        /// </summary>
        /// <param name="gesture">手势数据</param>
        private void OnSwipeBegin(Gesture gesture)
        {
            if (!m_SwipeSwitch) return;

            if (gesture.touchCount == 1)
            {
                m_SwipePosition = gesture.position;
                m_SwipeOffset = Vector2.zero;
                m_IsFingerSwiping = true;

                Vector3 worldPosition = ScreenToGestureWorld(gesture.position);
                DispatchCallbacks(m_SwipeBeginCallbacks, args =>
                {
                    args.Set("gesture", gesture);
                    args.Set("worldPosition", worldPosition);
                });
            }
        }

        /// <summary>
        /// 单指滑动中
        /// 实现边界弹性阻尼 + 相机移动
        /// </summary>
        /// <param name="gesture">手势数据</param>
        private void OnSwipe(Gesture gesture)
        {
            if (!m_SwipeSwitch) return;
            if (!m_IsFingerSwiping) return;
            if (m_SelectedObj != null) return;
            if (gesture.touchCount != 1) return;

            // 计算滑动偏移
            Vector2 startPos = ScreenToGestureWorld(m_SwipePosition);
            Vector2 curPos = ScreenToGestureWorld(gesture.position);
            m_SwipeOffset = curPos - startPos;

            SpaceEdgeBounds bounds = CalcSpaceEdgeBounds();
            DecaySwipeOffsetByEdges(ref m_SwipeOffset, bounds);

            // 限制相机在有效区域内
            Vector3 sceneCameraPos = m_SceneCamera.transform.position - (Vector3)m_SwipeOffset;
            sceneCameraPos.x = Mathf.Clamp(sceneCameraPos.x, bounds.Left, bounds.Right);
            sceneCameraPos.y = Mathf.Clamp(sceneCameraPos.y, bounds.Bottom, bounds.Top);
            m_SceneCamera.transform.SetPositionX(sceneCameraPos.x);
            m_SceneCamera.transform.SetPositionY(sceneCameraPos.y);
            m_SceneCamera.transform.SetPositionZ(sceneCameraPos.z);

            m_SwipePosition = gesture.position;
            m_IgnoreSelectObjBySwipe = true;

            // 触发滑动回调
            FireSwipeCallback(gesture);
        }

        /// <summary>
        /// 单指滑动结束
        /// 触发弹性回弹
        /// </summary>
        /// <param name="gesture">手势数据</param>
        private void OnSwipeEnd(Gesture gesture)
        {
            if (!m_SwipeSwitch) return;
            if (m_SelectedObj == null)
            {
                if (gesture.touchCount == 1)
                {
                    SwipeToElasticOnReleased();
                    m_IgnoreSelectObjBySwipe = false;
                    m_SafeTimeCounterOnGestureOver = m_SafeTimeOnGestureOver;

                    Vector3 worldPosition = ScreenToGestureWorld(gesture.position);
                    DispatchCallbacks(m_SwipeEndCallbacks, args =>
                    {
                        args.Set("gesture", gesture);
                        args.Set("worldPosition", worldPosition);
                    });
                }
            }
        }
        #endregion

        #region 缩放手势事件
        /// <summary>
        /// 双指缩放（支持焦点缩放 + 边界限制 + 弹性）
        /// </summary>
        /// <param name="gesture">手势数据</param>
        private void OnPinch(Gesture gesture)
        {
            if (!m_PinchSwitch) return;
            if (m_SelectedObj) return;
            if (gesture.touchCount != 2) return;

            if (m_SwipeSwitch)
            {
                // 双指同时移动相机
                ProcessPinchWithCameraMove(gesture);
            }
            else
            {
                // 仅缩放，不移动
                ProcessPinchScaleOnly(gesture);
            }

            // 触发缩放回调
            FirePinchCallback(gesture);
        }

        /// <summary>
        /// 双指缩放：同时根据滑动偏移移动相机焦点
        /// </summary>
        /// <param name="gesture">手势数据</param>
        private void ProcessPinchWithCameraMove(Gesture gesture)
        {
            // 双指同时移动相机
            Vector2 startPos = ScreenToGestureWorld(m_SwipePosition);
            Vector2 curPos = ScreenToGestureWorld(gesture.position);
            Vector2 swipeOffset = curPos - startPos;

            SpaceEdgeBounds bounds = CalcSpaceEdgeBounds();
            DecaySwipeOffsetByEdges(ref swipeOffset, bounds);

            var x = m_SceneCamera.transform.position.x - swipeOffset.x;
            var y = m_SceneCamera.transform.position.y - swipeOffset.y;
            m_SwipePosition = gesture.position;

            // 计算缩放
            float scale = ComputeClampedPinchScale(gesture.twoFingerDistance);
            UpdatePinchScaleElasticValue(scale);
            ApplyScaleToCamera(scale);

            // 对焦偏移
            Vector3 nowWorldCenter = m_SceneCamera.ViewportToWorldPoint(m_SceneCamera.ScreenToViewportPoint(m_PinchScreenCenterPosition));
            Vector3 worldCenterOffset = m_PinchWorldCenterPosition - nowWorldCenter;
            x += worldCenterOffset.x;
            y += worldCenterOffset.y;

            // 限制位置
            x = Mathf.Clamp(x, bounds.Left, bounds.Right);
            y = Mathf.Clamp(y, bounds.Bottom, bounds.Top);

            if (!m_SkipFirstPinchFrame)
            {
                m_SceneCamera.transform.position = new Vector3(x, y, m_SceneCamera.transform.position.z);
            }
            else
            {
                m_SkipFirstPinchFrame = false;
            }

            // 保存状态
            SavePinchState(gesture);
            m_IgnoreSelectObjByPinch = false;
            m_SafeTimeCounterOnGestureOver = m_SafeTimeOnGestureOver;
        }

        /// <summary>
        /// 双指缩放：仅做缩放，不移动相机
        /// </summary>
        /// <param name="gesture">手势数据</param>
        private void ProcessPinchScaleOnly(Gesture gesture)
        {
            float scale = ComputeClampedPinchScale(gesture.twoFingerDistance);
            UpdatePinchScaleElasticValue(scale);
            ApplyScaleToCamera(scale);

            SavePinchState(gesture);
            m_IgnoreSelectObjByPinch = false;
            m_SafeTimeCounterOnGestureOver = m_SafeTimeOnGestureOver;
        }

        /// <summary>
        /// 双指向内缩放（占位）
        /// </summary>
        private void OnPinchIn(Gesture gesture)
        {
            if (!m_PinchSwitch) return;
            if (m_SelectedObj) return;
        }

        /// <summary>
        /// 双指向外缩放（占位）
        /// </summary>
        private void OnPinchOut(Gesture gesture)
        {
            if (!m_PinchSwitch) return;
            if (m_SelectedObj) return;
        }

        /// <summary>
        /// 鼠标滚轮缩放
        /// </summary>
        /// <param name="offset">滚轮增量</param>
        private void MouseWheelPinch(float offset)
        {
            if (!m_MouseWheelPinchSwitch) return;
            if (offset == 0) return;
            if (m_SelectedObj) return;

            float scaleFactor = offset / m_PinchRatio;
            float localScale = m_SceneCamera.orthographic ? m_SceneCamera.orthographicSize : m_SceneCamera.fieldOfView;
            float scale = localScale + scaleFactor;
            scale = Mathf.Clamp(scale, m_PinchMinScale, m_PinchMaxScale);

            if (scale <= m_PinchMinScale)
            {
                m_TargetScaleElasticValue = m_PinchMinScale + m_SpaceEdgeScaleElasticValue;
            }
            else if (scale >= m_PinchMaxScale)
            {
                m_TargetScaleElasticValue = m_PinchMaxScale - m_SpaceEdgeScaleElasticValue;
            }
            else
            {
                m_TargetScaleElasticValue = -1f;
            }

            if (!m_SkipFirstPinchFrame)
            {
                if (m_SceneCamera.orthographic)
                    m_SceneCamera.orthographicSize = scale;
                else
                    m_SceneCamera.fieldOfView = scale;
            }
            else
            {
                m_SkipFirstPinchFrame = false;
            }

            m_IgnoreSelectObjByPinch = false;
            m_SafeTimeCounterOnGestureOver = m_SafeTimeOnGestureOver;
        }
        #endregion

        #region 对象拖拽事件
        /// <summary>
        /// 选中对象拖拽开始
        /// </summary>
        private void OnDragBegin(Gesture gesture)
        {
            if (!m_DragSwitch) return;
            if (gesture.touchCount == 1)
            {
                if (m_SelectedObj != null && !string.IsNullOrEmpty(m_SelectedObjType))
                {
                    m_IsDraging = true;

                    Vector3 worldPosition = ScreenToGestureWorld(gesture.position);
                    Vector3 deltaWorldPosition = Vector3.zero;
                    DispatchCallbacks(m_SelectedObjDragBeginCallbacks, args =>
                    {
                        args.Set("gesture", gesture);
                        args.Set("worldPosition", worldPosition);
                        args.Set("deltaWorldPosition", deltaWorldPosition);
                        args.Set("selectedObjType", m_SelectedObjType);
                        args.Set("selectedObj", m_SelectedObj);
                    });
                    m_LastWorldPosition = worldPosition;
                }
            }
        }

        /// <summary>
        /// 选中对象拖拽中
        /// </summary>
        private void OnDrag(Gesture gesture)
        {
            if (!m_DragSwitch) return;
            if (gesture.touchCount == 1)
            {
                if (m_SelectedObj != null && !string.IsNullOrEmpty(m_SelectedObjType))
                {
                    m_SelectedObjGesture = gesture;
                    Vector3 worldPosition = ScreenToGestureWorld(gesture.position);
                    Vector3 deltaWorldPosition = worldPosition - m_LastWorldPosition;
                    DispatchCallbacks(m_SelectedObjDragCallbacks, args =>
                    {
                        args.Set("gesture", gesture);
                        args.Set("worldPosition", worldPosition);
                        args.Set("deltaWorldPosition", deltaWorldPosition);
                        args.Set("selectedObjType", m_SelectedObjType);
                        args.Set("selectedObj", m_SelectedObj);
                    });
                    m_LastWorldPosition = worldPosition;
                }
            }
        }

        /// <summary>
        /// 选中对象拖拽结束
        /// </summary>
        private void OnDragEnd(Gesture gesture)
        {
            if (!m_DragSwitch) return;

            if (m_SelectedObj != null && !string.IsNullOrEmpty(m_SelectedObjType))
            {
                Vector3 worldPosition = ScreenToGestureWorld(gesture.position);

                if (m_IsDraging)
                {
                    Vector3 deltaWorldPosition = worldPosition - m_LastWorldPosition;
                    DispatchCallbacks(m_SelectedObjDragEndCallbacks, args =>
                    {
                        args.Set("gesture", gesture);
                        args.Set("worldPosition", worldPosition);
                        args.Set("deltaWorldPosition", deltaWorldPosition);
                        args.Set("selectedObjType", m_SelectedObjType);
                        args.Set("selectedObj", m_SelectedObj);
                    });
                    m_LastWorldPosition = Vector3.zero;
                }

                // 非常选中模式下自动取消选中
                if (!m_SelectHoldMode)
                {
                    DispatchCallbacks(m_UnselectedObjCallbacks, args =>
                    {
                        args.Set("gesture", gesture);
                        args.Set("worldPosition", worldPosition);
                        args.Set("selectedObjType", m_SelectedObjType);
                        args.Set("selectedObj", m_SelectedObj);
                    });
                    m_SelectedObjType = null;
                    m_SelectedObj = null;
                    m_SelectedObjGesture = null;
                }
            }
        }
        #endregion

        #region 帧更新逻辑
        /// <summary>
        /// 每帧更新
        /// 处理安全时间、选中持续回调、鼠标滚轮
        /// </summary>
        void Update()
        {
            if (m_SceneCamera == null) return;

            // 手势结束后的安全时间，不响应选中
            if (m_SafeTimeCounterOnGestureOver > 0)
            {
                m_SafeTimeCounterOnGestureOver -= Time.deltaTime;
                if (m_SafeTimeCounterOnGestureOver <= 0f)
                    m_SafeTimeCounterOnGestureOver = 0f;
            }

            // 持续选中更新回调
            if (m_SelectedObj != null && !string.IsNullOrEmpty(m_SelectedObjType))
            {
                Vector3 worldPosition = ScreenToGestureWorld(m_SelectedObjGesture.position);
                DispatchCallbacks(m_UpdateSelectedObjCallbacks, args =>
                {
                    args.Set("gesture", m_SelectedObjGesture);
                    args.Set("worldPosition", worldPosition);
                    args.Set("selectedObjType", m_SelectedObjType);
                    args.Set("selectedObj", m_SelectedObj);
                });
            }

            // 鼠标滚轮缩放
            MouseWheelPinch(Input.GetAxis("Mouse ScrollWheel") * m_MouseWheelPinchOffset);
        }

        /// <summary>
        /// 延迟更新
        /// 处理滑动惯性、缩放弹性回弹
        /// </summary>
        void LateUpdate()
        {
            if (m_SceneCamera == null) return;
            if (m_SelectedObj != null) return;

            // 滑动减速惯性
            if (m_IsFingerSwiping == false)
            {
                if (m_SwipeOffset != Vector2.zero)
                {
                    m_IsSwipeStableCallbackOver = false;
                    m_SwipeOffset = Vector2.Lerp(m_SwipeOffset, Vector2.zero, 0.15f);
                    Vector3 sceneCameraPos = m_SceneCamera.transform.position - (Vector3)m_SwipeOffset;

                    // 边界弹性回弹
                    float spaceHorizontalLeftEdgeX = m_SpaceCenterPosition.x - m_SpaceHorizontalLength / 2;
                    float spaceHorizontalRightEdgeX = m_SpaceCenterPosition.x + m_SpaceHorizontalLength / 2;
                    float spaceVerticalTopEdgeY = m_SpaceCenterPosition.y + m_SpaceVerticalLength / 2;
                    float spaceVerticalBottomEdgeY = m_SpaceCenterPosition.y - m_SpaceVerticalLength / 2;
                    float spaceHorizontalLeftEdgeXWithElasticLength = spaceHorizontalLeftEdgeX + m_SpaceHorizontalEdgeMoveElasticLength;
                    float spaceHorizontalRightEdgeXWithElasticLength = spaceHorizontalRightEdgeX - m_SpaceHorizontalEdgeMoveElasticLength;
                    float spaceVerticalTopEdgeYWithElasticLength = spaceVerticalTopEdgeY - m_SpaceVerticalEdgeMoveElasticLength;
                    float spaceVerticalBottomEdgeYWithElasticLength = spaceVerticalBottomEdgeY + m_SpaceVerticalEdgeMoveElasticLength;

                    if (sceneCameraPos.x <= spaceHorizontalLeftEdgeXWithElasticLength)
                    {
                        sceneCameraPos.x += (spaceHorizontalLeftEdgeXWithElasticLength - sceneCameraPos.x) / 10f;
                    }
                    else if (sceneCameraPos.x >= spaceHorizontalRightEdgeXWithElasticLength)
                    {
                        sceneCameraPos.x += (spaceHorizontalRightEdgeXWithElasticLength - sceneCameraPos.x) / 10f;
                    }

                    if (sceneCameraPos.y >= spaceVerticalTopEdgeYWithElasticLength)
                    {
                        sceneCameraPos.y += (spaceVerticalTopEdgeYWithElasticLength - sceneCameraPos.y) / 10f;
                    }
                    else if (sceneCameraPos.y <= spaceVerticalBottomEdgeYWithElasticLength)
                    {
                        sceneCameraPos.y += (spaceVerticalBottomEdgeYWithElasticLength - sceneCameraPos.y) / 10f;
                    }

                    // 限制位置
                    sceneCameraPos.x = Mathf.Clamp(sceneCameraPos.x, spaceHorizontalLeftEdgeX, spaceHorizontalRightEdgeX);
                    sceneCameraPos.y = Mathf.Clamp(sceneCameraPos.y, spaceVerticalBottomEdgeY, spaceVerticalTopEdgeY);
                    m_SceneCamera.transform.SetPositionX(sceneCameraPos.x);
                    m_SceneCamera.transform.SetPositionY(sceneCameraPos.y);
                    m_SceneCamera.transform.SetPositionZ(sceneCameraPos.z);
                }
                else
                {
                    // 滑动完全稳定
                    if (m_IsSwipeStableCallbackOver == false)
                    {
                        m_IsSwipeStableCallbackOver = true;
                        foreach (var callback in m_SwipeStableCallbacks)
                        {
                            LuaHandler.Callback(callback);
                        }
                    }
                }
            }

            // 缩放弹性回弹
            if (m_SkipFirstPinchFrame && m_TargetScaleElasticValue != -1f)
            {
                float scale = 0f;
                if (m_SceneCamera.orthographic)
                {
                    scale = Mathf.Lerp(m_SceneCamera.orthographicSize, m_TargetScaleElasticValue, 0.06f);
                    m_SceneCamera.orthographicSize = scale;
                }
                else
                {
                    scale = Mathf.Lerp(m_SceneCamera.fieldOfView, m_TargetScaleElasticValue, 0.06f);
                    m_SceneCamera.fieldOfView = scale;
                }

                if (Mathf.Abs(scale - m_TargetScaleElasticValue) < 0.00001f)
                {
                    // 缩放稳定
                    if (m_IsPinchStableCallbackOver == false)
                    {
                        m_IsPinchStableCallbackOver = true;
                        foreach (var callback in m_PinchStableCallbacks)
                        {
                            LuaHandler.Callback(callback);
                        }
                    }
                }
                else
                {
                    m_IsPinchStableCallbackOver = false;
                }
            }
        }
        #endregion
        #region 边界与缩放辅助计算
        /// <summary>
        /// 有效空间边界与弹性区坐标（值类型，避免堆分配）
        /// </summary>
        private struct SpaceEdgeBounds
        {
            public float Left;
            public float Right;
            public float Top;
            public float Bottom;
            public float LeftElastic;
            public float RightElastic;
            public float TopElastic;
            public float BottomElastic;
        }

        /// <summary>
        /// 将屏幕坐标转换为手势平面世界坐标（沿相机方向取 m_GestureCameraDistance 深度）
        /// </summary>
        /// <param name="screenPosition">屏幕坐标</param>
        /// <returns>对应的世界坐标</returns>
        private Vector3 ScreenToGestureWorld(Vector2 screenPosition)
        {
            return m_SceneCamera.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, m_GestureCameraDistance));
        }

        /// <summary>
        /// 统一派发 Lua 回调列表：新建参数表 -> 调用填充委托 -> 逐个回调
        /// </summary>
        /// <param name="callbacks">目标回调列表</param>
        /// <param name="fillArgs">向参数表写入具体字段的委托</param>
        private void DispatchCallbacks(List<LuaTable> callbacks, Action<LuaTable> fillArgs)
        {
            if (callbacks == null)
            {
                return;
            }

            foreach (LuaTable callback in callbacks)
            {
                LuaTable args = m_LuaComponent.Env.NewTable();
                fillArgs(args);
                LuaHandler.Callback(callback, args);
            }
        }

        /// <summary>
        /// 计算有效空间四周边界与对应弹性区坐标
        /// </summary>
        /// <returns>边界坐标结构体</returns>
        private SpaceEdgeBounds CalcSpaceEdgeBounds()
        {
            SpaceEdgeBounds bounds;
            bounds.Left = m_SpaceCenterPosition.x - m_SpaceHorizontalLength / 2;
            bounds.Right = m_SpaceCenterPosition.x + m_SpaceHorizontalLength / 2;
            bounds.Top = m_SpaceCenterPosition.y + m_SpaceVerticalLength / 2;
            bounds.Bottom = m_SpaceCenterPosition.y - m_SpaceVerticalLength / 2;
            bounds.LeftElastic = bounds.Left + m_SpaceHorizontalEdgeMoveElasticLength;
            bounds.RightElastic = bounds.Right - m_SpaceHorizontalEdgeMoveElasticLength;
            bounds.TopElastic = bounds.Top - m_SpaceVerticalEdgeMoveElasticLength;
            bounds.BottomElastic = bounds.Bottom + m_SpaceVerticalEdgeMoveElasticLength;
            return bounds;
        }

        /// <summary>
        /// 根据相机当前位置对滑动偏移做边界弹性衰减（原地修改偏移量）
        /// </summary>
        /// <param name="swipeOffset">当前滑动偏移</param>
        /// <param name="bounds">有效空间边界坐标</param>
        private void DecaySwipeOffsetByEdges(ref Vector2 swipeOffset, SpaceEdgeBounds bounds)
        {
            // 水平边界弹性衰减
            if (m_SpaceHorizontalEdgeMoveElasticLength > 0)
            {
                if (swipeOffset.x > 0 && m_SceneCamera.transform.position.x <= bounds.LeftElastic)
                {
                    swipeOffset.x *= (m_SceneCamera.transform.position.x - bounds.Left) / m_SpaceHorizontalEdgeMoveElasticLength;
                }
                if (swipeOffset.x < 0 && m_SceneCamera.transform.position.x >= bounds.RightElastic)
                {
                    swipeOffset.x *= (bounds.Right - m_SceneCamera.transform.position.x) / m_SpaceHorizontalEdgeMoveElasticLength;
                }
            }

            // 垂直边界弹性衰减
            if (m_SpaceVerticalEdgeMoveElasticLength > 0)
            {
                if (swipeOffset.y < 0 && m_SceneCamera.transform.position.y >= bounds.TopElastic)
                {
                    swipeOffset.y *= (bounds.Top - m_SceneCamera.transform.position.y) / m_SpaceVerticalEdgeMoveElasticLength;
                }
                if (swipeOffset.y > 0 && m_SceneCamera.transform.position.y <= bounds.BottomElastic)
                {
                    swipeOffset.y *= (m_SceneCamera.transform.position.y - bounds.Bottom) / m_SpaceVerticalEdgeMoveElasticLength;
                }
            }
        }

        /// <summary>
        /// 根据双指距离差计算并夹取缩放值
        /// </summary>
        /// <param name="twoFingerDistance">当前双指距离</param>
        /// <returns>夹取后的缩放值</returns>
        private float ComputeClampedPinchScale(float twoFingerDistance)
        {
            float distanceOffset = m_PinchFingersOldDistance - twoFingerDistance;
            float scaleFactor = distanceOffset / m_PinchRatio;
            float localScale = m_SceneCamera.orthographic ? m_SceneCamera.orthographicSize : m_SceneCamera.fieldOfView;
            float scale = localScale + scaleFactor;
            return Mathf.Clamp(scale, m_PinchMinScale, m_PinchMaxScale);
        }

        /// <summary>
        /// 根据缩放值更新缩放弹性回弹目标值
        /// </summary>
        /// <param name="scale">夹取后的缩放值</param>
        private void UpdatePinchScaleElasticValue(float scale)
        {
            // 缩放弹性区
            if (scale <= m_PinchMinScale)
            {
                m_TargetScaleElasticValue = m_PinchMinScale + m_SpaceEdgeScaleElasticValue;
            }
            else if (scale >= m_PinchMaxScale)
            {
                m_TargetScaleElasticValue = m_PinchMaxScale - m_SpaceEdgeScaleElasticValue;
            }
            else
            {
                m_TargetScaleElasticValue = -1f;
            }
        }

        /// <summary>
        /// 将缩放值应用到相机（首帧跳过并翻转跳过标记）
        /// </summary>
        /// <param name="scale">目标缩放值</param>
        private void ApplyScaleToCamera(float scale)
        {
            // 应用缩放
            if (!m_SkipFirstPinchFrame)
            {
                if (m_SceneCamera.orthographic)
                    m_SceneCamera.orthographicSize = scale;
                else
                    m_SceneCamera.fieldOfView = scale;
            }
            else
            {
                m_SkipFirstPinchFrame = false;
            }
        }

        /// <summary>
        /// 保存缩放后的双指状态
        /// </summary>
        /// <param name="gesture">手势数据</param>
        private void SavePinchState(Gesture gesture)
        {
            m_PinchFingersOldDistance = gesture.twoFingerDistance;
            m_PinchScreenCenterPosition = gesture.position;
            m_PinchWorldCenterPosition = m_SceneCamera.ViewportToWorldPoint(m_SceneCamera.ScreenToViewportPoint(m_PinchScreenCenterPosition));
        }

        /// <summary>
        /// 触发单指滑动的Lua回调列表（每次回调新建参数表）
        /// </summary>
        /// <param name="gesture">手势数据</param>
        private void FireSwipeCallback(Gesture gesture)
        {
            Vector3 worldPosition = ScreenToGestureWorld(gesture.position);
            DispatchCallbacks(m_SwipeCallbacks, args =>
            {
                args.Set("gesture", gesture);
                args.Set("worldPosition", worldPosition);
            });
        }

        /// <summary>
        /// 触发双指缩放的Lua回调列表（每次回调新建参数表）
        /// </summary>
        /// <param name="gesture">手势数据</param>
        private void FirePinchCallback(Gesture gesture)
        {
            DispatchCallbacks(m_PinchCallbacks, args =>
            {
                args.Set("gesture", gesture);
            });
        }
        #endregion
    }
}
#endif
