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

    public sealed partial class Gestures2D : MonoBehaviour
    {
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
        /// 启用回调
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
        /// 禁用回调
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
        
        /// <summary>
        /// 复位滑动参数
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
        /// 复位缩放参数
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

        /// <summary>
        /// 单指点击开始
        /// </summary>
        /// <param name="gesture">手势</param>
        private void OnTouchBegin(Gesture gesture)
        {
            // 修复有可能存在的特殊机型上事件丢失问题导致的逻辑异常
            FixSpecialDevicesEventLoses();

            //Log.Info("OnTouchBegin Gesture Pos = ({0:N2}, {1:N2})", gesture.position.x, gesture.position.y);
            DOTween.Kill(DOTweenTypes.CameraAnimation);
            DOTween.Kill(DOTweenTypes.CameraMoveAnimation);
            DOTween.Kill(DOTweenTypes.CameraRotateAnimation);
            DOTween.Kill(DOTweenTypes.CameraScaleAnimation);
            if (gesture.touchCount == 1)
            {
                Vector3 worldPosition = m_SceneCamera.ScreenToWorldPoint(new Vector3(gesture.position.x, gesture.position.y, m_GestureCameraDistance));
                foreach (var callback in m_TouchBeginCallbacks)
                {
                    LuaTable args = m_LuaComponent.Env.NewTable();
                    args.Set("gesture", gesture);
                    args.Set("worldPosition", worldPosition);
                    LuaHandler.Callback(callback, args);
                }
                m_AlreadySwipedOrPinchedOnThisRound = false;
                CheckSelect(gesture);
            }
        }

        /// <summary>
        /// 单指按下
        /// </summary>
        /// <param name="gesture">手势</param>
        private void OnTouchDown(Gesture gesture)
        {
            //Log.Info("OnTouchDown Gesture Pos = ({0:N2}, {1:N2})", gesture.position.x, gesture.position.y);
            if (gesture.touchCount == 1)
            {
                Vector3 worldPosition = m_SceneCamera.ScreenToWorldPoint(new Vector3(gesture.position.x, gesture.position.y, m_GestureCameraDistance));
                foreach (var callback in m_TouchDownCallbacks)
                {
                    LuaTable args = m_LuaComponent.Env.NewTable();
                    args.Set("gesture", gesture);
                    args.Set("worldPosition", worldPosition);
                    LuaHandler.Callback(callback, args);
                }
            }

        }

        /// <summary>
        /// 单指抬起
        /// </summary>
        /// <param name="gesture"></param>
        private void OnTouchEnd(Gesture gesture)
        {
            //Log.Info("OnTouchEnd Gesture Pos = ({0:N2}, {1:N2})", gesture.position.x, gesture.position.y);
            if (gesture.touchCount == 1)
            {
                Vector3 worldPosition = m_SceneCamera.ScreenToWorldPoint(new Vector3(gesture.position.x, gesture.position.y, m_GestureCameraDistance));
                foreach (var callback in m_TouchEndCallbacks)
                {
                    LuaTable args = m_LuaComponent.Env.NewTable();
                    args.Set("gesture", gesture);
                    args.Set("worldPosition", worldPosition);
                    LuaHandler.Callback(callback, args);
                }

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

                    // 常选中模式下：当对象处于被选中状态时，再次点击可以将选中状态反弹回未选中状态
                    // 非常选中模式下：自动切换回未选中状态
                    if ((m_SelectHoldMode && m_SelectReboundInSelectHoldMode && m_CanEnterSelectReboundInSelectHoldMode) ||
                        (m_SelectHoldMode && m_SelectReboundAfterDragEndInSelectHoldMode && m_IsDraging) ||
                        (!m_SelectHoldMode))
                    {
                        m_IsFingerSwiping = false;

                        //Log.Info("{0} is UnSelected!", m_SelectedObjType);
                        foreach (var callback in m_UnselectedObjCallbacks)
                        {
                            LuaTable args = m_LuaComponent.Env.NewTable();
                            args.Set("gesture", gesture);
                            args.Set("worldPosition", worldPosition);
                            args.Set("selectedObjType", m_SelectedObjType);
                            args.Set("selectedObj", m_SelectedObj);
                            LuaHandler.Callback(callback, args);
                        }
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

        /// <summary>
        /// 双指点击开始
        /// 坐标为双指中点位置
        /// </summary>
        /// <param name="gesture">手势</param>
        private void OnTouchesBegin(Gesture gesture)
        {
            //Log.Info("OnTouchesBegin Gesture Pos = ({0:N2}, {1:N2})", gesture.position.x, gesture.position.y);
            if (gesture.touchCount == 2)
            {
                Vector3 worldPosition = m_SceneCamera.ScreenToWorldPoint(new Vector3(gesture.position.x, gesture.position.y, m_GestureCameraDistance));
                foreach (var callback in m_TouchesBeginCallbacks)
                {
                    LuaTable args = m_LuaComponent.Env.NewTable();
                    args.Set("gesture", gesture);
                    args.Set("worldPosition", worldPosition);
                    LuaHandler.Callback(callback, args);
                }

                m_IsFingerSwiping = false;
                m_SwipePosition = gesture.position;
                m_SwipeOffset = Vector2.zero;

                m_PinchFingersOldDistance = gesture.twoFingerDistance;
                m_PinchScreenCenterPosition = gesture.position;
                m_PinchWorldCenterPosition = m_SceneCamera.ViewportToWorldPoint(m_SceneCamera.ScreenToViewportPoint(m_PinchScreenCenterPosition));

                m_SkipFirstPinchFrame = true;

                m_AlreadySwipedOrPinchedOnThisRound = false;
            }

        }

        /// <summary>
        /// 双指按下
        /// 坐标为双指中点位置
        /// </summary>
        /// <param name="gesture">手势</param>
        private void OnTouchesDown(Gesture gesture)
        {
            //Log.Info("OnTouchesDown Gesture Pos = ({0:N2}, {1:N2})", gesture.position.x, gesture.position.y);

            if (gesture.touchCount == 2)
            {
                Vector3 worldPosition = m_SceneCamera.ScreenToWorldPoint(new Vector3(gesture.position.x, gesture.position.y, m_GestureCameraDistance));
                foreach (var callback in m_TouchesDownCallbacks)
                {
                    LuaTable args = m_LuaComponent.Env.NewTable();
                    args.Set("gesture", gesture);
                    args.Set("worldPosition", worldPosition);
                    LuaHandler.Callback(callback, args);
                }
            }

        }

        /// <summary>
        /// 双指抬起
        /// 坐标为双指中点位置
        /// </summary>
        /// <param name="gesture"></param>
        private void OnTouchesEnd(Gesture gesture)
        {
            //Log.Info("OnTouchesEnd Gesture Pos = ({0:N2}, {1:N2})", gesture.position.x, gesture.position.y);

            if (gesture.touchCount == 2)
            {
                Vector3 worldPosition = m_SceneCamera.ScreenToWorldPoint(new Vector3(gesture.position.x, gesture.position.y, m_GestureCameraDistance));
                foreach (var callback in m_TouchesEndCallbacks)
                {
                    LuaTable args = m_LuaComponent.Env.NewTable();
                    args.Set("gesture", gesture);
                    args.Set("worldPosition", worldPosition);
                    LuaHandler.Callback(callback, args);
                }

                // 滑动到弹性区（松手释放时）
                SwipeToElasticOnReleased();

                m_PinchFingersOldDistance = gesture.twoFingerDistance;
                m_PinchScreenCenterPosition = gesture.position;
                m_PinchWorldCenterPosition = m_SceneCamera.ViewportToWorldPoint(m_SceneCamera.ScreenToViewportPoint(m_PinchScreenCenterPosition));

                m_SkipFirstPinchFrame = true;
            }

        }

        /// <summary>
        /// 单指滑动开始
        /// </summary>
        /// <param name="gesture">手势</param>
        private void OnSwipeBegin(Gesture gesture)
        {
            //Log.Info("OnSwipeBegin Gesture Pos = ({0:N2}, {1:N2})", gesture.position.x, gesture.position.y);
            //Log.Info("OnSwipeBegin");

            if (!m_SwipeSwitch) return;

            if (gesture.touchCount == 1)
            {
                m_SwipePosition = gesture.position;
                m_SwipeOffset = Vector2.zero;
                m_IsFingerSwiping = true;

                Vector3 worldPosition = m_SceneCamera.ScreenToWorldPoint(new Vector3(gesture.position.x, gesture.position.y, m_GestureCameraDistance));
                foreach (var callback in m_SwipeBeginCallbacks)
                {
                    LuaTable args = m_LuaComponent.Env.NewTable();
                    args.Set("gesture", gesture);
                    args.Set("worldPosition", worldPosition);
                    LuaHandler.Callback(callback, args);
                }
            }

        }

        /// <summary>
        /// 单指滑动
        /// </summary>
        /// <param name="gesture">手势</param>
        private void OnSwipe(Gesture gesture)
        {
            //Log.Info("OnSwipe Gesture Pos = ({0:N2}, {1:N2})", gesture.position.x, gesture.position.y);

            if (!m_SwipeSwitch) return;
            if (!m_IsFingerSwiping) return;

            if (m_SelectedObj == null)
            {
                if (gesture.touchCount == 1)
                {
                    // 记录当前滑动手势所在的场景位置 　　  
                    Vector2 startPos = m_SceneCamera.ScreenToWorldPoint(new Vector3(m_SwipePosition.x, m_SwipePosition.y, m_GestureCameraDistance));
                    Vector2 curPos = m_SceneCamera.ScreenToWorldPoint(new Vector3(gesture.position.x, gesture.position.y, m_GestureCameraDistance));

                    // 计算得到当前滑动手势在场景中的偏移量
                    m_SwipeOffset = curPos - startPos;

                    // 有效空间内水平方向左侧边缘X坐标值
                    float spaceHorizontalLeftEdgeX = m_SpaceCenterPosition.x - m_SpaceHorizontalLength / 2;
                    // 有效空间内水平方向右侧边缘X坐标值
                    float spaceHorizontalRightEdgeX = m_SpaceCenterPosition.x + m_SpaceHorizontalLength / 2;
                    // 有效空间内垂直方向上方边缘Y坐标值
                    float spaceVerticalTopEdgeY = m_SpaceCenterPosition.y + m_SpaceVerticalLength / 2;
                    // 有效空间内垂直方向下方边缘Y坐标值
                    float spaceVerticalBottomEdgeY = m_SpaceCenterPosition.y - m_SpaceVerticalLength / 2;
                    // 有效空间内水平方向左侧边缘弹性区X坐标值
                    float spaceHorizontalLeftEdgeXWithElasticLength = spaceHorizontalLeftEdgeX + m_SpaceHorizontalEdgeMoveElasticLength;
                    // 有效空间内水平方向右侧边缘弹性区X坐标值
                    float spaceHorizontalRightEdgeXWithElasticLength = spaceHorizontalRightEdgeX - m_SpaceHorizontalEdgeMoveElasticLength;
                    // 有效空间内垂直方向上方边缘弹性区Y坐标值
                    float spaceVerticalTopEdgeYWithElasticLength = spaceVerticalTopEdgeY - m_SpaceVerticalEdgeMoveElasticLength;
                    // 有效空间内垂直方向下方边缘弹性区Y坐标值
                    float spaceVerticalBottomEdgeYWithElasticLength = spaceVerticalBottomEdgeY + m_SpaceVerticalEdgeMoveElasticLength;
                    
                    if(m_SpaceHorizontalEdgeMoveElasticLength > 0)
                    {
                        // 进入左侧弹性区时，滑动强度开始进行衰减
                        if (m_SwipeOffset.x > 0)
                        {
                            if (m_SceneCamera.transform.position.x <= spaceHorizontalLeftEdgeXWithElasticLength)
                            {
                                m_SwipeOffset.x *= (m_SceneCamera.transform.position.x - spaceHorizontalLeftEdgeX) / m_SpaceHorizontalEdgeMoveElasticLength;
                            }
                        }
                        // 进入右侧弹性区时，滑动强度开始进行衰减
                        if (m_SwipeOffset.x < 0)
                        {
                            if (m_SceneCamera.transform.position.x >= spaceHorizontalRightEdgeXWithElasticLength)
                            {
                                m_SwipeOffset.x *= (spaceHorizontalRightEdgeX - m_SceneCamera.transform.position.x) / m_SpaceHorizontalEdgeMoveElasticLength;
                            }
                        }
                    }
                    if(m_SpaceVerticalEdgeMoveElasticLength > 0)
                    {
                        // 进入上侧弹性区时，滑动强度开始进行衰减
                        if (m_SwipeOffset.y < 0)
                        {
                            if (m_SceneCamera.transform.position.y >= spaceVerticalTopEdgeYWithElasticLength)
                            {
                                m_SwipeOffset.y *= (spaceVerticalTopEdgeY - m_SceneCamera.transform.position.y) / m_SpaceVerticalEdgeMoveElasticLength;
                            }
                        }
                        // 进入下侧弹性区时，滑动强度开始进行衰减
                        if (m_SwipeOffset.y > 0)
                        {
                            if (m_SceneCamera.transform.position.y <= spaceVerticalBottomEdgeYWithElasticLength)
                            {
                                m_SwipeOffset.y *= (m_SceneCamera.transform.position.y - spaceVerticalBottomEdgeY) / m_SpaceVerticalEdgeMoveElasticLength;
                            }
                        }
                    }

                    // 保证滑动过程中始终在有效空间内
                    Vector3 sceneCameraPos = m_SceneCamera.transform.position - (Vector3)m_SwipeOffset;
                    sceneCameraPos.x = Mathf.Clamp(sceneCameraPos.x, spaceHorizontalLeftEdgeX, spaceHorizontalRightEdgeX);
                    sceneCameraPos.y = Mathf.Clamp(sceneCameraPos.y, spaceVerticalBottomEdgeY, spaceVerticalTopEdgeY);
                    m_SceneCamera.transform.SetPositionX(sceneCameraPos.x);
                    m_SceneCamera.transform.SetPositionY(sceneCameraPos.y);
                    m_SceneCamera.transform.SetPositionZ(sceneCameraPos.z);

                    m_SwipePosition = gesture.position;
                    m_IgnoreSelectObjBySwipe = true;
                    m_AlreadySwipedOrPinchedOnThisRound = true;

                    Vector3 worldPosition = m_SceneCamera.ScreenToWorldPoint(new Vector3(gesture.position.x, gesture.position.y, m_GestureCameraDistance));
                    foreach (var callback in m_SwipeCallbacks)
                    {
                        LuaTable args = m_LuaComponent.Env.NewTable();
                        args.Set("gesture", gesture);
                        args.Set("worldPosition", worldPosition);
                        LuaHandler.Callback(callback, args);
                    }

                }
            }

        }

        /// <summary>
        /// 单指滑动抬起
        /// </summary>
        /// <param name="gesture">手势</param>
        private void OnSwipeEnd(Gesture gesture)
        {
            //Log.Info("OnSwipeEnd Gesture Pos = ({0:N2}, {1:N2})", gesture.position.x, gesture.position.y);
            if (!m_SwipeSwitch) return;
            if (m_SelectedObj == null)
            {
                if (gesture.touchCount == 1)
                {
                    // 滑动到弹性区（松手释放时）
                    SwipeToElasticOnReleased();

                    m_IgnoreSelectObjBySwipe = false;
                    m_SafeTimeCounterOnGestureOver = m_SafeTimeOnGestureOver;

                    Vector3 worldPosition = m_SceneCamera.ScreenToWorldPoint(new Vector3(gesture.position.x, gesture.position.y, m_GestureCameraDistance));
                    foreach (var callback in m_SwipeEndCallbacks)
                    {
                        LuaTable args = m_LuaComponent.Env.NewTable();
                        args.Set("gesture", gesture);
                        args.Set("worldPosition", worldPosition);
                        LuaHandler.Callback(callback, args);
                    }

                }
            }

        }

        /// <summary>
        /// 触摸缩放
        /// </summary>
        /// <param name="gesture">手势</param>
        private void OnPinch(Gesture gesture)
        {
            if (!m_PinchSwitch) return;

            if (m_SelectedObj)
            {
                return;
            }

            if (gesture.touchCount == 2)
            {
                //GFuncs.SLog("On_Pinch");

                if (m_SwipeSwitch)
                {
                    // 根据双指中央的位置移动相机

                    // 记录鼠标拖动的位置 　　  
                    Vector2 startPos = m_SceneCamera.ScreenToWorldPoint(new Vector3(m_SwipePosition.x, m_SwipePosition.y, m_GestureCameraDistance));
                    Vector2 curPos = m_SceneCamera.ScreenToWorldPoint(new Vector3(gesture.position.x, gesture.position.y, m_GestureCameraDistance));

                    // 计算得到当前滑动手势在场景中的偏移量
                    Vector2 swipeOffset = curPos - startPos;

                    // 有效空间内水平方向左侧边缘X坐标值
                    float spaceHorizontalLeftEdgeX = m_SpaceCenterPosition.x - m_SpaceHorizontalLength / 2;
                    // 有效空间内水平方向右侧边缘X坐标值
                    float spaceHorizontalRightEdgeX = m_SpaceCenterPosition.x + m_SpaceHorizontalLength / 2;
                    // 有效空间内垂直方向上方边缘Y坐标值
                    float spaceVerticalTopEdgeY = m_SpaceCenterPosition.y + m_SpaceVerticalLength / 2;
                    // 有效空间内垂直方向下方边缘Y坐标值
                    float spaceVerticalBottomEdgeY = m_SpaceCenterPosition.y - m_SpaceVerticalLength / 2;
                    // 有效空间内水平方向左侧边缘弹性区X坐标值
                    float spaceHorizontalLeftEdgeXWithElasticLength = spaceHorizontalLeftEdgeX + m_SpaceHorizontalEdgeMoveElasticLength;
                    // 有效空间内水平方向右侧边缘弹性区X坐标值
                    float spaceHorizontalRightEdgeXWithElasticLength = spaceHorizontalRightEdgeX - m_SpaceHorizontalEdgeMoveElasticLength;
                    // 有效空间内垂直方向上方边缘弹性区Y坐标值
                    float spaceVerticalTopEdgeYWithElasticLength = spaceVerticalTopEdgeY - m_SpaceVerticalEdgeMoveElasticLength;
                    // 有效空间内垂直方向下方边缘弹性区Y坐标值
                    float spaceVerticalBottomEdgeYWithElasticLength = spaceVerticalBottomEdgeY + m_SpaceVerticalEdgeMoveElasticLength;
                    
                    if(m_SpaceHorizontalEdgeMoveElasticLength > 0)
                    {
                        // 进入左侧弹性区
                        if (swipeOffset.x > 0)
                        {
                            if (m_SceneCamera.transform.position.x <= spaceHorizontalLeftEdgeXWithElasticLength)
                            {
                                swipeOffset.x = swipeOffset.x * ((m_SceneCamera.transform.position.x - spaceHorizontalLeftEdgeX) / m_SpaceHorizontalEdgeMoveElasticLength);
                            }
                        }
                        // 进入右侧弹性区
                        if (swipeOffset.x < 0)
                        {
                            if (m_SceneCamera.transform.position.x >= spaceHorizontalRightEdgeXWithElasticLength)
                            {
                                swipeOffset.x = swipeOffset.x * ((spaceHorizontalRightEdgeX - m_SceneCamera.transform.position.x) / m_SpaceHorizontalEdgeMoveElasticLength);
                            }
                        }
                    }
                    if (m_SpaceVerticalEdgeMoveElasticLength > 0)
                    {
                        // 进入上侧弹性区
                        if (swipeOffset.y < 0)
                        {
                            if (m_SceneCamera.transform.position.y >= spaceVerticalTopEdgeYWithElasticLength)
                            {
                                swipeOffset.y = swipeOffset.y * ((spaceVerticalTopEdgeY - m_SceneCamera.transform.position.y) / m_SpaceVerticalEdgeMoveElasticLength);
                            }
                        }
                        // 进入下侧弹性区
                        if (swipeOffset.y > 0)
                        {
                            if (m_SceneCamera.transform.position.y <= spaceVerticalBottomEdgeYWithElasticLength)
                            {
                                swipeOffset.y = swipeOffset.y * ((m_SceneCamera.transform.position.y - spaceVerticalBottomEdgeY) / m_SpaceVerticalEdgeMoveElasticLength);
                            }
                        }
                    }

                    var x = m_SceneCamera.transform.position.x - swipeOffset.x;
                    var y = m_SceneCamera.transform.position.y - swipeOffset.y;

                    m_SwipePosition = gesture.position;

                    // 根据双指手势缩放相机

                    // 新旧距离之差
                    float distanceOffset = m_PinchFingersOldDistance - gesture.twoFingerDistance;

                    // 缩放因子
                    float scaleFactor = distanceOffset / m_PinchRatio;
                    float localScale = m_SceneCamera.orthographic ? m_SceneCamera.orthographicSize : m_SceneCamera.fieldOfView;
                    float scale = localScale + scaleFactor;

                    scale = Mathf.Clamp(scale, m_PinchMinScale, m_PinchMaxScale);

                    // 进入最小缩放弹性区
                    if (scale <= m_PinchMinScale)
                    {
                        m_TargetScaleElasticValue = m_PinchMinScale + m_SpaceEdgeScaleElasticValue;
                    }
                    // 进入最大缩放弹性区
                    else if (scale >= m_PinchMaxScale)
                    {
                        m_TargetScaleElasticValue = m_PinchMaxScale - m_SpaceEdgeScaleElasticValue;
                    }
                    else
                    {
                        m_TargetScaleElasticValue = -1f;
                    }

                    // 刷新缩放与位置（一边缩放一边向焦点靠近，给人的视觉效果就是对焦缩放！！！）
                    if (!m_SkipFirstPinchFrame)
                    {
                        if(m_SceneCamera.orthographic)
                        {
                            m_SceneCamera.orthographicSize = scale;
                        }
                        else
                        {
                            m_SceneCamera.fieldOfView = scale;
                        }
                    }

                    // 刷新位置
                    Vector3 nowWorldCenter = m_SceneCamera.ViewportToWorldPoint(m_SceneCamera.ScreenToViewportPoint(m_PinchScreenCenterPosition));
                    Vector3 worldCenterOffset = m_PinchWorldCenterPosition - nowWorldCenter;

                    x += worldCenterOffset.x;
                    y += worldCenterOffset.y;

                    // 保证缩放过程中始终在包围盒以内
                    x = Mathf.Clamp(x, spaceHorizontalLeftEdgeX, spaceHorizontalRightEdgeX);
                    y = Mathf.Clamp(y, spaceVerticalBottomEdgeY, spaceVerticalTopEdgeY);

                    if (!m_SkipFirstPinchFrame)
                    {
                        m_SceneCamera.transform.position = new Vector3(x, y, m_SceneCamera.transform.position.z);
                    }
                    else
                    {
                        m_SkipFirstPinchFrame = false;
                    }

                    // 记住最新的触摸点，下次使用   
                    m_PinchFingersOldDistance = gesture.twoFingerDistance;
                    m_PinchScreenCenterPosition = gesture.position;
                    m_PinchWorldCenterPosition = m_SceneCamera.ViewportToWorldPoint(m_SceneCamera.ScreenToViewportPoint(m_PinchScreenCenterPosition));

                    m_IgnoreSelectObjByPinch = false;
                    m_SafeTimeCounterOnGestureOver = m_SafeTimeOnGestureOver;
                }
                else
                {
                    // 新旧距离之差
                    float offset = m_PinchFingersOldDistance - gesture.twoFingerDistance;

                    // 缩放因子
                    float scaleFactor = offset / m_PinchRatio;
                    float localScale = m_SceneCamera.orthographic ? m_SceneCamera.orthographicSize : m_SceneCamera.fieldOfView;
                    float scale = localScale + scaleFactor;

                    scale = Mathf.Clamp(scale, m_PinchMinScale, m_PinchMaxScale);

                    // 进入最小缩放弹性区
                    if (scale <= m_PinchMinScale)
                    {
                        m_TargetScaleElasticValue = m_PinchMinScale + m_SpaceEdgeScaleElasticValue;
                    }
                    // 进入最大缩放弹性区
                    else if (scale >= m_PinchMaxScale)
                    {
                        m_TargetScaleElasticValue = m_PinchMaxScale - m_SpaceEdgeScaleElasticValue;
                    }
                    else
                    {
                        m_TargetScaleElasticValue = -1f;
                    }

                    // 刷新缩放与位置（一边缩放一边向焦点靠近，给人的视觉效果就是对焦缩放！！！）
                    if (!m_SkipFirstPinchFrame)
                    {
                        if(m_SceneCamera.orthographic)
                        {
                            m_SceneCamera.orthographicSize = scale;
                        }
                        else
                        {
                            m_SceneCamera.fieldOfView = scale;
                        }
                    }
                    else
                    {
                        m_SkipFirstPinchFrame = false;
                    }

                    // 记住最新的触摸点，下次使用   
                    m_PinchFingersOldDistance = gesture.twoFingerDistance;
                    m_PinchScreenCenterPosition = gesture.position;
                    m_PinchWorldCenterPosition = m_SceneCamera.ViewportToWorldPoint(m_SceneCamera.ScreenToViewportPoint(m_PinchScreenCenterPosition));

                    m_IgnoreSelectObjByPinch = false;
                    m_SafeTimeCounterOnGestureOver = m_SafeTimeOnGestureOver;
                }

                m_AlreadySwipedOrPinchedOnThisRound = true;

                foreach (var callback in m_PinchCallbacks)
                {
                    LuaTable args = m_LuaComponent.Env.NewTable();
                    args.Set("gesture", gesture);
                    LuaHandler.Callback(callback, args);
                }
            }

        }

        /// <summary>
        /// 触摸缩放In(双指靠拢：缩小)
        /// </summary>
        /// <param name="gesture">手势</param>
        private void OnPinchIn(Gesture gesture)
        {
            if (!m_PinchSwitch) return;

            if (m_SelectedObj)
            {
                return;
            }
        }

        /// <summary>
        /// 触摸缩放Out(双指远离：放大)
        /// </summary>
        /// <param name="gesture">手势</param>
        private void OnPinchOut(Gesture gesture)
        {
            if (!m_PinchSwitch) return;

            if (m_SelectedObj)
            {
                return;
            }
        }

        /// <summary>
        /// 鼠标滚轮缩放
        /// </summary>
        /// <param name="offset">偏移值</param>
        private void MouseWheelPinch(float offset)
        {
            if (!m_MouseWheelPinchSwitch) return;
            if (offset == 0) return;
            if (m_SelectedObj)
            {
                return;
            }

            // 缩放因子
            float scaleFactor = offset / m_PinchRatio;
            float localScale = m_SceneCamera.orthographic ? m_SceneCamera.orthographicSize : m_SceneCamera.fieldOfView;
            float scale = localScale + scaleFactor;

            scale = Mathf.Clamp(scale, m_PinchMinScale, m_PinchMaxScale);

            // 进入最小缩放弹性区
            if (scale <= m_PinchMinScale)
            {
                m_TargetScaleElasticValue = m_PinchMinScale + m_SpaceEdgeScaleElasticValue;
            }
            // 进入最大缩放弹性区
            else if (scale >= m_PinchMaxScale)
            {
                m_TargetScaleElasticValue = m_PinchMaxScale - m_SpaceEdgeScaleElasticValue;
            }
            else
            {
                m_TargetScaleElasticValue = -1f;
            }

            // 刷新缩放与位置（一边缩放一边向焦点靠近，给人的视觉效果就是对焦缩放！！！）
            if (!m_SkipFirstPinchFrame)
            {
                if (m_SceneCamera.orthographic)
                {
                    m_SceneCamera.orthographicSize = scale;
                }
                else
                {
                    m_SceneCamera.fieldOfView = scale;
                }
            }
            else
            {
                m_SkipFirstPinchFrame = false;
            }

            m_IgnoreSelectObjByPinch = false;
            m_SafeTimeCounterOnGestureOver = m_SafeTimeOnGestureOver;
        }

        /// <summary>
        /// 对象拖拽开始
        /// </summary>
        /// <param name="gesture">手势</param>
        private void OnDragBegin(Gesture gesture)
        {
            if (!m_DragSwitch) return;
            if (gesture.touchCount == 1)
            {
                if (m_SelectedObj != null && !string.IsNullOrEmpty(m_SelectedObjType))
                {
                    //Log.Info("Drag Begin!");
                    m_IsDraging = true;

                    Vector3 worldPosition = m_SceneCamera.ScreenToWorldPoint(new Vector3(gesture.position.x, gesture.position.y, m_GestureCameraDistance));
                    Vector3 deltaWorldPosition = Vector3.zero;
                    foreach (var callback in m_SelectedObjDragBeginCallbacks)
                    {
                        LuaTable args = m_LuaComponent.Env.NewTable();
                        args.Set("gesture", gesture);
                        args.Set("worldPosition", worldPosition);
                        args.Set("deltaWorldPosition", deltaWorldPosition);
                        args.Set("selectedObjType", m_SelectedObjType);
                        args.Set("selectedObj", m_SelectedObj);
                        LuaHandler.Callback(callback, args);
                    }
                    m_LastWorldPosition = worldPosition;
                }
            }
        }

        /// <summary>
        /// 对象拖拽中
        /// </summary>
        /// <param name="gesture">手势</param>
        private void OnDrag(Gesture gesture)
        {
            if (!m_DragSwitch) return;
            if (gesture.touchCount == 1)
            {
                if (m_SelectedObj != null && !string.IsNullOrEmpty(m_SelectedObjType))
                {
                    //Log.Info("Drag!");
                    m_SelectedObjGesture = gesture;
                    Vector3 worldPosition = m_SceneCamera.ScreenToWorldPoint(new Vector3(gesture.position.x, gesture.position.y, m_GestureCameraDistance));
                    Vector3 deltaWorldPosition = worldPosition - m_LastWorldPosition;
                    foreach (var callback in m_SelectedObjDragCallbacks)
                    {
                        LuaTable args = m_LuaComponent.Env.NewTable();
                        args.Set("gesture", gesture);
                        args.Set("worldPosition", worldPosition);
                        args.Set("deltaWorldPosition", deltaWorldPosition);
                        args.Set("selectedObjType", m_SelectedObjType);
                        args.Set("selectedObj", m_SelectedObj);
                        LuaHandler.Callback(callback, args);
                    }
                    m_LastWorldPosition = worldPosition;
                }
            }
        }

        /// <summary>
        /// 对象拖拽结束
        /// </summary>
        /// <param name="gesture">手势</param>
        private void OnDragEnd(Gesture gesture)
        {
            if (!m_DragSwitch) return;

            if (m_SelectedObj != null && !string.IsNullOrEmpty(m_SelectedObjType))
            {
                Vector3 worldPosition = m_SceneCamera.ScreenToWorldPoint(new Vector3(gesture.position.x, gesture.position.y, m_GestureCameraDistance));

                if (m_IsDraging)
                {
                    //Log.Info("Drag End!");                    
                    Vector3 deltaWorldPosition = worldPosition - m_LastWorldPosition;
                    foreach (var callback in m_SelectedObjDragEndCallbacks)
                    {
                        LuaTable args = m_LuaComponent.Env.NewTable();
                        args.Set("gesture", gesture);
                        args.Set("worldPosition", worldPosition);
                        args.Set("deltaWorldPosition", deltaWorldPosition);
                        args.Set("selectedObjType", m_SelectedObjType);
                        args.Set("selectedObj", m_SelectedObj);
                        LuaHandler.Callback(callback, args);
                    }
                    m_LastWorldPosition = Vector3.zero;
                }

                if (!m_SelectHoldMode)
                {
                    //Log.Info("{0} is UnSelected!", m_SelectedObjType);
                    foreach (var callback in m_UnselectedObjCallbacks)
                    {
                        LuaTable args = m_LuaComponent.Env.NewTable();
                        args.Set("gesture", gesture);
                        args.Set("worldPosition", worldPosition);
                        args.Set("selectedObjType", m_SelectedObjType);
                        args.Set("selectedObj", m_SelectedObj);
                        LuaHandler.Callback(callback, args);
                    }
                    m_SelectedObjType = null;
                    m_SelectedObj = null;
                    m_SelectedObjGesture = null;
                }

            }
        }

        void Update()
        {
            if(m_SceneCamera == null)
            {
                return;
            }

            // 手势刚刚结束后的安全时间内不响应Select事件
            if (m_SafeTimeCounterOnGestureOver > 0)
            {
                m_SafeTimeCounterOnGestureOver -= Time.deltaTime;
                if (m_SafeTimeCounterOnGestureOver <= 0f)
                {
                    m_SafeTimeCounterOnGestureOver = 0f;
                }
            }

            // 持续选中回调
            if (m_SelectedObj != null && !string.IsNullOrEmpty(m_SelectedObjType))
            {
                Vector3 worldPosition = m_SceneCamera.ScreenToWorldPoint(new Vector3(m_SelectedObjGesture.position.x, m_SelectedObjGesture.position.y, m_GestureCameraDistance));
                foreach (var callback in m_UpdateSelectedObjCallbacks)
                {
                    LuaTable args = m_LuaComponent.Env.NewTable();
                    args.Set("gesture", m_SelectedObjGesture);
                    args.Set("worldPosition", worldPosition);
                    args.Set("selectedObjType", m_SelectedObjType);
                    args.Set("selectedObj", m_SelectedObj);
                    LuaHandler.Callback(callback, args);
                }
            }

            // 鼠标滚轮缩放控制
            MouseWheelPinch(Input.GetAxis("Mouse ScrollWheel") * m_MouseWheelPinchOffset);

        }

        void LateUpdate()
        {
            if (m_SceneCamera == null)
            {
                return;
            }

            if (m_SelectedObj != null)
            {
                return;
            }

            if (m_IsFingerSwiping == false)
            {
                if (m_SwipeOffset != Vector2.zero)
                {
                    m_IsSwipeStableCallbackOver = false;

                    // 对滑动偏移量做差值运算，进而联动场景相机做差值减速运动
                    m_SwipeOffset = Vector2.Lerp(m_SwipeOffset, Vector2.zero, 0.15f);

                    Vector3 sceneCameraPos = m_SceneCamera.transform.position - (Vector3)m_SwipeOffset;
                    
                    // 有效空间内水平方向左侧边缘X坐标值
                    float spaceHorizontalLeftEdgeX = m_SpaceCenterPosition.x - m_SpaceHorizontalLength / 2;
                    // 有效空间内水平方向右侧边缘X坐标值
                    float spaceHorizontalRightEdgeX = m_SpaceCenterPosition.x + m_SpaceHorizontalLength / 2;
                    // 有效空间内垂直方向上方边缘Y坐标值
                    float spaceVerticalTopEdgeY = m_SpaceCenterPosition.y + m_SpaceVerticalLength / 2;
                    // 有效空间内垂直方向下方边缘Y坐标值
                    float spaceVerticalBottomEdgeY = m_SpaceCenterPosition.y - m_SpaceVerticalLength / 2;
                    // 有效空间内水平方向左侧边缘弹性区X坐标值
                    float spaceHorizontalLeftEdgeXWithElasticLength = spaceHorizontalLeftEdgeX + m_SpaceHorizontalEdgeMoveElasticLength;
                    // 有效空间内水平方向右侧边缘弹性区X坐标值
                    float spaceHorizontalRightEdgeXWithElasticLength = spaceHorizontalRightEdgeX - m_SpaceHorizontalEdgeMoveElasticLength;
                    // 有效空间内垂直方向上方边缘弹性区Y坐标值
                    float spaceVerticalTopEdgeYWithElasticLength = spaceVerticalTopEdgeY - m_SpaceVerticalEdgeMoveElasticLength;
                    // 有效空间内垂直方向下方边缘弹性区Y坐标值
                    float spaceVerticalBottomEdgeYWithElasticLength = spaceVerticalBottomEdgeY + m_SpaceVerticalEdgeMoveElasticLength;
                    
                    // 进入左侧弹性区
                    if (sceneCameraPos.x <= spaceHorizontalLeftEdgeXWithElasticLength)
                    {
                        sceneCameraPos.x += (spaceHorizontalLeftEdgeXWithElasticLength - sceneCameraPos.x) / 10f;
                    }
                    // 进入右侧弹性区
                    else if (sceneCameraPos.x >= spaceHorizontalRightEdgeXWithElasticLength)
                    {
                        sceneCameraPos.x += (spaceHorizontalRightEdgeXWithElasticLength - sceneCameraPos.x) / 10f;
                    }
                    // 进入上方弹性区
                    if (sceneCameraPos.y >= spaceVerticalTopEdgeYWithElasticLength)
                    {
                        sceneCameraPos.y += (spaceVerticalTopEdgeYWithElasticLength - sceneCameraPos.y) / 10f;
                    }
                    // 进入下方弹性区
                    else if (sceneCameraPos.y <= spaceVerticalBottomEdgeYWithElasticLength)
                    {
                        sceneCameraPos.y += (spaceVerticalBottomEdgeYWithElasticLength - sceneCameraPos.y) / 10f;
                    }

                    // 保证滑动过程中始终在包围盒以内
                    sceneCameraPos.x = Mathf.Clamp(sceneCameraPos.x, spaceHorizontalLeftEdgeX, spaceHorizontalRightEdgeX);
                    sceneCameraPos.y = Mathf.Clamp(sceneCameraPos.y, spaceVerticalBottomEdgeY, spaceVerticalTopEdgeY);
                    m_SceneCamera.transform.SetPositionX(sceneCameraPos.x);
                    m_SceneCamera.transform.SetPositionY(sceneCameraPos.y);
                    m_SceneCamera.transform.SetPositionZ(sceneCameraPos.z);
                }
                else
                {
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

            if (m_SkipFirstPinchFrame && m_TargetScaleElasticValue != -1f)
            {
                float scale = 0f;
                if(m_SceneCamera.orthographic)
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
    }
}
#endif
