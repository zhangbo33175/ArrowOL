#if EASY_TOUCH_ENABLE
namespace Honor.Runtime
{
    using HedgehogTeam.EasyTouch;
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Rendering;
    using XLua;

    public sealed partial class Gestures2D : MonoBehaviour
    {
        /// <summary>
        /// 清理缓存数据
        /// </summary>
        private void CleanCaches()
        {
            m_SwipePosition = Vector2.zero;
            m_SwipeOffset = Vector2.zero;
            m_IsFingerSwiping = false;
            m_IsSwipeStableCallbackOver = true;
            m_PinchScreenCenterPosition = Vector3.zero;
            m_PinchWorldCenterPosition = Vector3.zero;
            m_PinchFingersOldDistance = 0f;
            m_SkipFirstPinchFrame = true;
            m_TargetScaleElasticValue = -1f;
            m_IsPinchStableCallbackOver = true;
            m_AlreadySwipedOrPinchedOnThisRound = false;
            m_SafeTimeCounterOnGestureOver = 0f;
            m_SelectedObj = null;
            m_SelectedObjType = null;
            m_SelectedObjGesture = null;
            m_CanEnterSelectReboundInSelectHoldMode = true;
            m_IsDraging = false;
            m_LastWorldPosition = Vector3.zero;
            m_IgnoreSelectObjBySwipe = false;
            m_IgnoreSelectObjByPinch = false;
        }

        /// <summary>
        /// 滑动到弹性区（松手释放时）
        /// 当已经进入弹性区时，则不需要惯性速度了，只需要设置一个反弹速度
        /// </summary>
        private void SwipeToElasticOnReleased()
        {
            m_IsFingerSwiping = false;

            m_SwipePosition = Vector2.zero;

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
            
            // 已经进入左侧弹性区，则不需要惯性速度了，只需要设置一个反弹速度
            if(m_SpaceHorizontalEdgeMoveElasticLength > 0)
            {
                if (m_SceneCamera.transform.position.x <= spaceHorizontalLeftEdgeXWithElasticLength)
                {
                    m_SwipeOffset.x = -0.05f;
                }
            }

            // 已经进入右侧弹性区，则不需要惯性速度了，只需要设置一个反弹速度
            if(m_SpaceHorizontalEdgeMoveElasticLength > 0)
            {
                if (m_SceneCamera.transform.position.x >= spaceHorizontalRightEdgeXWithElasticLength)
                {
                    m_SwipeOffset.x = 0.05f;
                }
            }

            // 已经进入上侧弹性区，则不需要惯性速度了，只需要设置一个反弹速度
            if(m_SpaceVerticalEdgeMoveElasticLength > 0)
            {
                if (m_SceneCamera.transform.position.y >= spaceVerticalTopEdgeYWithElasticLength)
                {
                    m_SwipeOffset.y = 0.05f;
                }
            }

            // 已经进入下侧弹性区，则不需要惯性速度了，只需要设置一个反弹速度
            if (m_SpaceVerticalEdgeMoveElasticLength > 0)
            {
                if (m_SceneCamera.transform.position.y <= spaceVerticalBottomEdgeYWithElasticLength)
                {
                    m_SwipeOffset.y = -0.05f;
                }
            }

        }

        /// <summary>
        /// 检查选中逻辑
        /// </summary>
        /// <param name="gesture"></param>
        private void CheckSelect(Gesture gesture)
        {
            if (!m_SelectSwitch) return;

            if(m_SafeTimeCounterOnGestureOver > 0)
            {
                return;
            }

            // 处理选中状态回调
            if (gesture.touchCount == 1)
            {
                if (m_IgnoreSelectObjBySwipe == false && m_IgnoreSelectObjByPinch == false)
                {
                    if (gesture.type == EasyTouch.EvtType.On_TouchStart)
                    {
                        if (gesture.actionTime >= m_PressTimeOfSelectingObj)  // 响应选中操作
                        {
                            if ((gesture.pickedObject != null && !m_SelectHoldMode && m_SelectedObj == null) || m_SelectHoldMode)
                            {
                                // 需要被射线检测到的对象自身具有rigidBody2D或者collider2D
                                Ray ray = m_SceneCamera.ScreenPointToRay(gesture.position);

                                List<GameObject> goHits = new List<GameObject>();

                                if (m_ColliderDetectMode == Definitions.DimensionMode.Two)
                                {
                                    RaycastHit2D[] hits2D = Physics2D.GetRayIntersectionAll(ray);

                                    // 沿着y坐标大小进行由前向后的排序。
                                    if (m_PosYSortSelectSwitch)
                                    {
                                        Array.Sort(hits2D, (v1, v2) => v1.transform.position.y.CompareTo(v2.transform.position.y));
                                    }

                                    // 沿着ray被击中的距离进行由近到远的排序(间接考虑到了z轴的因素)。
                                    if (m_RayDistanceSortSelectSwitch)
                                    {
                                        Array.Sort(hits2D, (v1, v2) => v1.distance.CompareTo(v2.distance));
                                    }

                                    // 根据sortingLayer进行排序（层级高的靠前）+ 根据相同sortingLayer的order进行排序（order大的靠前）
                                    if (m_SortingLayerOrderSortSelectSwitch)
                                    {
                                        Array.Sort(hits2D, (v1, v2) => v2.transform.GetComponent<Renderer>().sortingLayerID.CompareTo(v1.transform.GetComponent<Renderer>().sortingLayerID));
                                        Array.Sort(hits2D, (v1, v2) =>
                                        {
                                            if (v1.transform.GetComponent<Renderer>().sortingLayerID == v2.transform.GetComponent<Renderer>().sortingLayerID)
                                            {
                                                return v2.transform.GetComponent<Renderer>().sortingOrder.CompareTo(v1.transform.GetComponent<Renderer>().sortingOrder);
                                            }
                                            return 0;
                                        });
                                    }

                                    // 收集排序后的go集合
                                    for (int index = 0; index < hits2D.Length; index++)
                                    {
                                        goHits.Add(hits2D[index].transform.gameObject);
                                    }
                                }
                                else if (m_ColliderDetectMode == Definitions.DimensionMode.Three)
                                {
                                    RaycastHit[] hits3D = Physics.RaycastAll(ray);

                                    // 沿着y坐标大小进行由前向后的排序。
                                    if (m_PosYSortSelectSwitch)
                                    {
                                        Array.Sort(hits3D, (v1, v2) => v1.transform.position.y.CompareTo(v2.transform.position.y));
                                    }

                                    // 沿着ray被击中的距离进行由近到远的排序(间接考虑到了z轴的因素)。
                                    if (m_RayDistanceSortSelectSwitch)
                                    {
                                        Array.Sort(hits3D, (v1, v2) => v1.distance.CompareTo(v2.distance));
                                    }

                                    // 根据sortingLayer进行排序（层级高的靠前）+ 根据相同sortingLayer的order进行排序（order大的靠前）
                                    if (m_SortingLayerOrderSortSelectSwitch)
                                    {
                                        Array.Sort(hits3D, (v1, v2) => v2.transform.GetComponent<Renderer>().sortingLayerID.CompareTo(v1.transform.GetComponent<Renderer>().sortingLayerID));
                                        Array.Sort(hits3D, (v1, v2) =>
                                        {
                                            if (v1.transform.GetComponent<Renderer>().sortingLayerID == v2.transform.GetComponent<Renderer>().sortingLayerID)
                                            {
                                                return v2.transform.GetComponent<Renderer>().sortingOrder.CompareTo(v1.transform.GetComponent<Renderer>().sortingOrder);
                                            }
                                            return 0;
                                        });
                                    }

                                    // 收集排序后的go集合
                                    for (int index = 0; index < hits3D.Length; index++)
                                    {
                                        goHits.Add(hits3D[index].transform.gameObject);
                                    }
                                }

                                // 检查当前响应的对象类型是否为指定的对象类型
                                for (int index = 0; index < goHits.Count; index++)
                                {
                                    Component component = null;
                                    string selectingTypeName = null;
                                    // 在自身中查询
                                    if (component == null && m_FindSelectingTypesOnSelf)
                                    {
                                        selectingTypeName = m_SelectingTypes.Find((name) =>
                                        {
                                            if(goHits[index].transform.GetLua(name))
                                            {
                                                return true;
                                            }
                                            return false;
                                        });
                                        if(!string.IsNullOrEmpty(selectingTypeName))
                                        {
                                            component = goHits[index].transform.GetLua(selectingTypeName);
                                        }
                                    }
                                    // 在父对象中查询
                                    if (component == null && m_FindSelectingTypesOnParent)
                                    {
                                        selectingTypeName = m_SelectingTypes.Find((name) =>
                                        {
                                            if (goHits[index].transform.GetLuaInParent(name))
                                            {
                                                return true;
                                            }
                                            return false;
                                        });
                                        if (!string.IsNullOrEmpty(selectingTypeName))
                                        {
                                            component = goHits[index].transform.GetLuaInParent(selectingTypeName);
                                        }
                                    }
                                    // 在子对象中查询
                                    if (component == null && m_FindSelectingTypesOnChildren)
                                    {
                                        selectingTypeName = m_SelectingTypes.Find((name) =>
                                        {
                                            if (goHits[index].transform.GetLuaInChildren(name))
                                            {
                                                return true;
                                            }
                                            return false;
                                        });
                                        if (!string.IsNullOrEmpty(selectingTypeName))
                                        {
                                            component = goHits[index].transform.GetLuaInChildren(selectingTypeName);
                                        }
                                    }
                                    // 查到符合条件的控件时表示需要响应选中逻辑
                                    if (component != null && !string.IsNullOrEmpty(selectingTypeName))
                                    {
                                        Vector3 worldPosition = m_SceneCamera.ScreenToWorldPoint(new Vector3(gesture.position.x, gesture.position.y, m_GestureCameraDistance));

                                        // 常选中模式下切换选中对象
                                        if (m_SelectHoldMode)
                                        {
                                            if(m_SelectedObj == null)
                                            {
                                                //Log.Info("{0} is Selected!", m_SelectingTypes[index]);
                                                m_SelectedObjType = selectingTypeName;
                                                m_SelectedObj = component.gameObject;
                                                m_SelectedObjGesture = gesture;

                                                foreach (var callback in m_SelectedObjCallbacks)
                                                {
                                                    LuaTable args = m_LuaComponent.Env.NewTable();
                                                    args.Set("gesture", gesture);
                                                    args.Set("worldPosition", worldPosition);
                                                    args.Set("selectedObjType", m_SelectedObjType);
                                                    args.Set("selectedObj", m_SelectedObj);
                                                    LuaHandler.Callback(callback, args);
                                                }
                                                m_LastWorldPosition = worldPosition;
                                                m_CanEnterSelectReboundInSelectHoldMode = true;
                                            }
                                            else if (m_SelectedObj != component.gameObject)
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


                                                //Log.Info("{0} is Selected!", m_SelectingTypes[index]);
                                                m_SelectedObjType = selectingTypeName;
                                                m_SelectedObj = component.gameObject;
                                                m_SelectedObjGesture = gesture;

                                                foreach (var callback in m_SelectedObjCallbacks)
                                                {
                                                    LuaTable args = m_LuaComponent.Env.NewTable();
                                                    args.Set("gesture", gesture);
                                                    args.Set("worldPosition", worldPosition);
                                                    args.Set("selectedObjType", m_SelectedObjType);
                                                    args.Set("selectedObj", m_SelectedObj);
                                                    LuaHandler.Callback(callback, args);
                                                }
                                                m_LastWorldPosition = worldPosition;
                                                m_CanEnterSelectReboundInSelectHoldMode = true;
                                            }
                                        }
                                        else
                                        {
                                            //Log.Info("{0} is Selected!", m_SelectingTypes[index]);
                                            m_SelectedObjType = selectingTypeName;
                                            m_SelectedObj = component.gameObject;
                                            m_SelectedObjGesture = gesture;

                                            foreach (var callback in m_SelectedObjCallbacks)
                                            {
                                                LuaTable args = m_LuaComponent.Env.NewTable();
                                                args.Set("gesture", gesture);
                                                args.Set("worldPosition", worldPosition);
                                                args.Set("selectedObjType", m_SelectedObjType);
                                                args.Set("selectedObj", m_SelectedObj);
                                                LuaHandler.Callback(callback, args);
                                            }
                                            m_LastWorldPosition = worldPosition;
                                        }
                                        return;
                                    }
                                }
                            }
                            
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 修复有可能存在的特殊机型上事件丢失问题导致的逻辑异常
        /// </summary>
        private void FixSpecialDevicesEventLoses()
        {
            // 个别三星手机上存在SwipeEnd回调丢失问题，进而导致选中对象逻辑异常，这里需要进行复位，确保逻辑正常。
            if(m_SafeTimeCounterOnGestureOver == 0)
            {
                ResetSwipe();
                m_SafeTimeCounterOnGestureOver = 0;
            }
        }
    }
}
#endif
