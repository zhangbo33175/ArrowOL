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

    public sealed partial class GesturesUI : MonoBehaviour
    {
        void Awake()
        {
            m_UITouchCoverCallbacks = new List<LuaTable>();
            m_UITouchEndCallbacks = new List<LuaTable>();
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

            // UI对象-触摸-回调注册
            EasyTouch.On_OverUIElement += OnUIElementTouchCover;
            EasyTouch.On_UIElementTouchUp += OnUIElementTouchEnd;

        }

        /// <summary>
        /// 禁用回调
        /// </summary>
        void OnDisable()
        {
            // UI对象-触摸-回调注销
            EasyTouch.On_OverUIElement -= OnUIElementTouchCover;
            EasyTouch.On_UIElementTouchUp -= OnUIElementTouchEnd;
        }

        /// <summary>
        /// UI元素覆盖中
        /// 仅作用于开启RaycastTarget的UI元素
        /// </summary>
        /// <param name="gesture">手势</param>
        public void OnUIElementTouchCover(Gesture gesture)
        {
            if (!m_UISwitch) return;
            if (gesture.touchCount == 1)
            {
                Vector3 worldPosition = m_UICamera.ScreenToWorldPoint(new Vector3(gesture.position.x, gesture.position.y, m_UICamera.nearClipPlane));
                foreach (var callback in m_UITouchCoverCallbacks)
                {
                    LuaTable args = m_LuaComponent.Env.NewTable();
                    args.Set("gesture", gesture);
                    args.Set("worldPosition", worldPosition);
                    LuaHandler.Callback(callback, args);
                }
                CheckSelect(gesture);
            }

        }

        /// <summary>
        /// 从UI元素中抬起
        /// 仅作用于开启RaycastTarget的UI元素
        /// </summary>
        /// <param name="gesture">手势</param>
        public void OnUIElementTouchEnd(Gesture gesture)
        {
            if (!m_UISwitch) return;

            if (gesture.touchCount == 1)
            {
                Vector3 worldPosition = m_UICamera.ScreenToWorldPoint(new Vector3(gesture.position.x, gesture.position.y, m_UICamera.nearClipPlane));
                foreach (var callback in m_UITouchEndCallbacks)
                {
                    LuaTable args = m_LuaComponent.Env.NewTable();
                    args.Set("gesture", gesture);
                    args.Set("worldPosition", worldPosition);
                    LuaHandler.Callback(callback, args);
                }
            }

            if (m_SelectedObj != null && !string.IsNullOrEmpty(m_SelectedObjType))
            {
                Vector3 worldPosition = m_UICamera.ScreenToWorldPoint(new Vector3(gesture.position.x, gesture.position.y, m_UICamera.nearClipPlane));

                // 拖拽结束
                if (m_IsDraging == true)
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
                    m_LastWorldPosition = worldPosition;
                    m_CurDragStateOnThisRound = DragState.End;
                }

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
                // 常选中模式下：当对象拖拽结束时，可以将选中状态反弹回未选中状态
                // 非常选中模式下：自动切换回未选中状态
                if ((m_SelectHoldMode && m_SelectReboundInSelectHoldMode && m_CanEnterSelectReboundInSelectHoldMode) ||
                    (m_SelectHoldMode && m_SelectReboundAfterDragEndInSelectHoldMode && m_IsDraging) ||
                    (!m_SelectHoldMode))
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

                    m_LastWorldPosition = Vector3.zero;

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

        void Update()
        {
            if (m_SelectedObj != null && !string.IsNullOrEmpty(m_SelectedObjType))
            {
                // 持续选中回调
                Vector3 worldPosition = m_UICamera.ScreenToWorldPoint(new Vector3(m_SelectedObjGesture.position.x, m_SelectedObjGesture.position.y, m_UICamera.nearClipPlane));
                foreach (var callback in m_UpdateSelectedObjCallbacks)
                {
                    LuaTable args = m_LuaComponent.Env.NewTable();
                    args.Set("gesture", m_SelectedObjGesture);
                    args.Set("worldPosition", worldPosition);
                    args.Set("selectedObjType", m_SelectedObjType);
                    args.Set("selectedObj", m_SelectedObj);
                    LuaHandler.Callback(callback, args);
                }

                Gesture gesture = EasyTouch.current;
                if (gesture != null && gesture.touchCount == 1)
                {
                    if (m_DragSwitch)
                    {
                        worldPosition = m_UICamera.ScreenToWorldPoint(new Vector3(gesture.position.x, gesture.position.y, m_UICamera.nearClipPlane));
                        if (gesture.deltaPosition != Vector2.zero)
                        {
                            // 拖拽开始回调
                            if (m_CurDragStateOnThisRound == DragState.None && m_IsDraging == false)
                            {
                                //Log.Info("Drag Begin!");
                                m_IsDraging = true;

                                Vector3 deltaWorldPosition = Vector3.zero;
                                foreach (var callback in m_SelectedObjDragBeginCallbacks)
                                {
                                    LuaTable args = m_LuaComponent.Env.NewTable();
                                    args.Set("gesture", m_SelectedObjGesture);
                                    args.Set("worldPosition", worldPosition);
                                    args.Set("deltaWorldPosition", deltaWorldPosition);
                                    args.Set("selectedObjType", m_SelectedObjType);
                                    args.Set("selectedObj", m_SelectedObj);
                                    LuaHandler.Callback(callback, args);
                                }
                                m_LastWorldPosition = worldPosition;
                                m_CurDragStateOnThisRound = DragState.Begin;
                            }
                            else if (m_CurDragStateOnThisRound == DragState.Begin || m_CurDragStateOnThisRound == DragState.OnGoing) // 拖拽中回调
                            {
                                //Log.Info("Drag!");
                                m_SelectedObjGesture = gesture;

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
                                m_CurDragStateOnThisRound = DragState.OnGoing;
                            }
                        }
                    }
                }
            }
        }
    }
}

#endif