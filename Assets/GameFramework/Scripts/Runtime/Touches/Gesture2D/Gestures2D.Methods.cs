/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  Gestures2D.Methods.cs
 * author:    云毅
 * created:   2026
 * descrip:   2D相机手势控制器 - 核心逻辑分部类
 ***************************************************************/
#if EASY_TOUCH_ENABLE
namespace Honor.Runtime
{
    using HedgehogTeam.EasyTouch;
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Rendering;
    using XLua;

    //=========================================================================
    // 2D 相机手势控制器 - 核心逻辑分部类
    //=========================================================================
    public sealed partial class Gestures2D : MonoBehaviour
    {
        #region 缓存与状态管理
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
        #endregion

        #region 滑动弹性逻辑
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
            if (m_SpaceHorizontalEdgeMoveElasticLength > 0)
            {
                if (m_SceneCamera.transform.position.x <= spaceHorizontalLeftEdgeXWithElasticLength)
                {
                    m_SwipeOffset.x = -0.05f;
                }
            }

            // 已经进入右侧弹性区，则不需要惯性速度了，只需要设置一个反弹速度
            if (m_SpaceHorizontalEdgeMoveElasticLength > 0)
            {
                if (m_SceneCamera.transform.position.x >= spaceHorizontalRightEdgeXWithElasticLength)
                {
                    m_SwipeOffset.x = 0.05f;
                }
            }

            // 已经进入上侧弹性区，则不需要惯性速度了，只需要设置一个反弹速度
            if (m_SpaceVerticalEdgeMoveElasticLength > 0)
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
        #endregion

        #region 对象选中逻辑
        /// <summary>
        /// 检查选中逻辑
        /// </summary>
        /// <param name="gesture">手势数据</param>
        private void CheckSelect(Gesture gesture)
        {
            if (!m_SelectSwitch) return;
            if (m_SafeTimeCounterOnGestureOver > 0) return;
            if (gesture.touchCount != 1) return;
            if (m_IgnoreSelectObjBySwipe || m_IgnoreSelectObjByPinch) return;
            if (gesture.type != EasyTouch.EvtType.On_TouchStart) return;
            if (gesture.actionTime < m_PressTimeOfSelectingObj) return;
            bool canEnterSelect = (gesture.pickedObject != null && !m_SelectHoldMode && m_SelectedObj == null) || m_SelectHoldMode;
            if (!canEnterSelect) return;

            // 需要被射线检测到的对象自身具有rigidBody2D或者collider2D
            Ray ray = m_SceneCamera.ScreenPointToRay(gesture.position);
            List<GameObject> goHits = new List<GameObject>();
            CollectSelectRaycastHits(ray, goHits);
            ProcessSelectHits(goHits, gesture);
        }

        /// <summary>
        /// 根据碰撞检测模式执行射线检测并按配置排序后收集命中的GameObject
        /// </summary>
        /// <param name="ray">由屏幕点生成的射线</param>
        /// <param name="goHits">收集命中对象的输出列表</param>
        private void CollectSelectRaycastHits(Ray ray, List<GameObject> goHits)
        {
            if (m_ColliderDetectMode == GameDefinitions.DimensionMode.Two)
            {
                CollectSelectHits2D(ray, goHits);
            }
            else if (m_ColliderDetectMode == GameDefinitions.DimensionMode.Three)
            {
                CollectSelectHits3D(ray, goHits);
            }
        }

        /// <summary>
        /// 2D射线检测：按配置排序后收集命中的GameObject
        /// </summary>
        /// <param name="ray">由屏幕点生成的射线</param>
        /// <param name="goHits">收集命中对象的输出列表</param>
        private void CollectSelectHits2D(Ray ray, List<GameObject> goHits)
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

        /// <summary>
        /// 3D射线检测：按配置排序后收集命中的GameObject
        /// </summary>
        /// <param name="ray">由屏幕点生成的射线</param>
        /// <param name="goHits">收集命中对象的输出列表</param>
        private void CollectSelectHits3D(Ray ray, List<GameObject> goHits)
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

        /// <summary>
        /// 遍历命中对象，查找第一个匹配选中类型的组件并应用选中逻辑
        /// </summary>
        /// <param name="goHits">排序后的命中对象列表</param>
        /// <param name="gesture">手势数据</param>
        private void ProcessSelectHits(List<GameObject> goHits, Gesture gesture)
        {
            // 检查当前响应的对象类型是否为指定的对象类型
            for (int index = 0; index < goHits.Count; index++)
            {
                Component component = TryFindSelectingComponent(goHits[index], out string selectingTypeName);

                // 查到符合条件的控件时表示需要响应选中逻辑
                if (component != null && !string.IsNullOrEmpty(selectingTypeName))
                {
                    Vector3 worldPosition = m_SceneCamera.ScreenToWorldPoint(new Vector3(gesture.position.x, gesture.position.y, m_GestureCameraDistance));
                    ApplySelectionResult(gesture, component, selectingTypeName, worldPosition);
                    return;
                }
            }
        }

        /// <summary>
        /// 依次在自身/父对象/子对象中查询匹配选中类型的Lua组件
        /// </summary>
        /// <param name="goHit">待查询的命中对象</param>
        /// <param name="selectingTypeName">命中的选中类型名</param>
        /// <returns>匹配到的组件，未命中返回null</returns>
        private Component TryFindSelectingComponent(GameObject goHit, out string selectingTypeName)
        {
            Component component = null;
            selectingTypeName = null;
            // 在自身中查询
            if (component == null && m_FindSelectingTypesOnSelf)
            {
                selectingTypeName = m_SelectingTypes.Find((name) =>
                {
                    if (goHit.transform.GetLua(name))
                    {
                        return true;
                    }
                    return false;
                });
                if (!string.IsNullOrEmpty(selectingTypeName))
                {
                    component = goHit.transform.GetLua(selectingTypeName);
                }
            }
            // 在父对象中查询
            if (component == null && m_FindSelectingTypesOnParent)
            {
                selectingTypeName = m_SelectingTypes.Find((name) =>
                {
                    if (goHit.transform.GetLuaInParent(name))
                    {
                        return true;
                    }
                    return false;
                });
                if (!string.IsNullOrEmpty(selectingTypeName))
                {
                    component = goHit.transform.GetLuaInParent(selectingTypeName);
                }
            }
            // 在子对象中查询
            if (component == null && m_FindSelectingTypesOnChildren)
            {
                selectingTypeName = m_SelectingTypes.Find((name) =>
                {
                    if (goHit.transform.GetLuaInChildren(name))
                    {
                        return true;
                    }
                    return false;
                });
                if (!string.IsNullOrEmpty(selectingTypeName))
                {
                    component = goHit.transform.GetLuaInChildren(selectingTypeName);
                }
            }
            return component;
        }

        /// <summary>
        /// 应用选中结果：根据常选中/非常选中模式更新选中状态并触发对应回调
        /// </summary>
        /// <param name="gesture">手势数据</param>
        /// <param name="component">命中的目标组件</param>
        /// <param name="selectingTypeName">命中的选中类型名</param>
        /// <param name="worldPosition">手势对应的世界坐标</param>
        private void ApplySelectionResult(Gesture gesture, Component component, string selectingTypeName, Vector3 worldPosition)
        {
            // 常选中模式下切换选中对象
            if (m_SelectHoldMode)
            {
                if (m_SelectedObj == null)
                {
                    //Log.Info("{0} is Selected!", m_SelectingTypes[index]);
                    m_SelectedObjType = selectingTypeName;
                    m_SelectedObj = component.gameObject;
                    m_SelectedObjGesture = gesture;
                    FireSelectedObjCallbacks(gesture, worldPosition);
                    m_LastWorldPosition = worldPosition;
                    m_CanEnterSelectReboundInSelectHoldMode = true;
                }
                else if (m_SelectedObj != component.gameObject)
                {
                    //Log.Info("{0} is UnSelected!", m_SelectedObjType);
                    FireUnselectedObjCallbacks(gesture, worldPosition);
                    m_SelectedObjType = null;
                    m_SelectedObj = null;
                    m_SelectedObjGesture = null;

                    //Log.Info("{0} is Selected!", m_SelectingTypes[index]);
                    m_SelectedObjType = selectingTypeName;
                    m_SelectedObj = component.gameObject;
                    m_SelectedObjGesture = gesture;
                    FireSelectedObjCallbacks(gesture, worldPosition);
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
                FireSelectedObjCallbacks(gesture, worldPosition);
                m_LastWorldPosition = worldPosition;
            }
        }

        /// <summary>
        /// 触发指定的Lua回调列表（每次回调新建参数表）
        /// </summary>
        /// <param name="callbacks">待触发的回调列表。</param>
        /// <param name="gesture">手势数据</param>
        /// <param name="worldPosition">手势对应的世界坐标</param>
        private void FireObjCallbacks(List<LuaTable> callbacks, Gesture gesture, Vector3 worldPosition)
        {
            foreach (var callback in callbacks)
            {
                LuaTable args = m_LuaComponent.Env.NewTable();
                args.Set("gesture", gesture);
                args.Set("worldPosition", worldPosition);
                args.Set("selectedObjType", m_SelectedObjType);
                args.Set("selectedObj", m_SelectedObj);
                LuaHandler.Callback(callback, args);
            }
        }

        /// <summary>
        /// 触发选中对象的Lua回调列表（每次回调新建参数表）
        /// </summary>
        /// <param name="gesture">手势数据</param>
        /// <param name="worldPosition">手势对应的世界坐标</param>
        private void FireSelectedObjCallbacks(Gesture gesture, Vector3 worldPosition)
        {
            FireObjCallbacks(m_SelectedObjCallbacks, gesture, worldPosition);
        }

        /// <summary>
        /// 触发取消选中对象的Lua回调列表（每次回调新建参数表）
        /// </summary>
        /// <param name="gesture">手势数据</param>
        /// <param name="worldPosition">手势对应的世界坐标</param>
        private void FireUnselectedObjCallbacks(Gesture gesture, Vector3 worldPosition)
        {
            FireObjCallbacks(m_UnselectedObjCallbacks, gesture, worldPosition);
        }
        #endregion

        #region 设备兼容修复
        /// <summary>
        /// 修复有可能存在的特殊机型上事件丢失问题导致的逻辑异常
        /// </summary>
        private void FixSpecialDevicesEventLoses()
        {
            // 个别三星手机上存在SwipeEnd回调丢失问题，进而导致选中对象逻辑异常，这里需要进行复位，确保逻辑正常。
            if (m_SafeTimeCounterOnGestureOver == 0)
            {
                ResetSwipe();
                m_SafeTimeCounterOnGestureOver = 0;
            }
        }
        #endregion
    }
}
#endif