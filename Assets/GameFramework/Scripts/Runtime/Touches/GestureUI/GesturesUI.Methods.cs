/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  GesturesUI.Methods.cs
 * author:    云毅
 * created:   2026
 * descrip:   UI手势交互控制器 - 核心工具逻辑分部类
 ***************************************************************/
#if EASY_TOUCH_ENABLE
namespace Honor.Runtime
{
    using HedgehogTeam.EasyTouch;
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using XLua;

    //=========================================================================
    // UI 手势交互控制器 - 核心工具逻辑
    //=========================================================================
    /// <summary>
    /// UI 手势交互控制器 - 核心工具逻辑分部类（缓存清理与选中检测）
    /// </summary>
    public sealed partial class GesturesUI : MonoBehaviour
    {
        #region 缓存与状态管理
        /// <summary>
        /// 清理缓存数据
        /// </summary>
        private void CleanCaches()
        {
            m_SelectedObj = null;
            m_SelectedObjType = null;
            m_SelectedObjGesture = null;
            m_CanEnterSelectReboundInSelectHoldMode = true;
            m_IsDraging = false;
            m_CurDragStateOnThisRound = DragState.None;
            m_LastWorldPosition = Vector3.zero;
        }
        #endregion

        #region UI选中检测逻辑
        /// <summary>
        /// 检查选中逻辑
        /// </summary>
        /// <param name="gesture">手势数据</param>
        private void CheckSelect(Gesture gesture)
        {
            if (!m_SelectSwitch) return;
            if (gesture.touchCount != 1) return;
            if (gesture.type != EasyTouch.EvtType.On_OverUIElement) return;
            if (gesture.actionTime < m_PressTimeOfSelectingObj) return;
            bool canEnterSelect = (gesture.pickedUIElement != null && !m_SelectHoldMode && m_SelectedObj == null) || m_SelectHoldMode;
            if (!canEnterSelect) return;

            GameObject[] goHits2D = CollectSortedUIHits(gesture);
            ProcessUIHits(goHits2D, gesture);
        }

        /// <summary>
        /// 对UI元素执行2D射线检测，按父子/兄弟层级排序后返回命中数组
        /// </summary>
        /// <param name="gesture">手势数据</param>
        /// <returns>排序后的命中GameObject数组</returns>
        private GameObject[] CollectSortedUIHits(Gesture gesture)
        {
            // 需要被射线检测到的对象自身具有rigidBody2D或者collider2D
            Ray ray = m_UICamera.ScreenPointToRay(gesture.position);
            RaycastHit2D[] hits2D = Physics2D.GetRayIntersectionAll(ray);

            // 筛选射线检测到的GameObject并排序
            List<GameObject> hits2DList = new List<GameObject>();
            for (int index = 0; index < hits2D.Length; index++)
            {
                if (hits2D[index].transform != null)
                {
                    hits2DList.Add(hits2D[index].transform.gameObject);
                }
            }
            GameObject[] goHits2D = hits2DList.ToArray();
            // 排序规则：依次逐级到互为兄弟节点的父节点后根据兄弟节点的SiblingIndex进行排序。
            Transform root = transform.root;
            Array.Sort(goHits2D, (v1, v2) =>
            {
                // IsChildOf为递归式接口，可返回N级判定
                if (v2.transform.IsChildOf(v1.transform))
                {
                    return 1;
                }
                else if (v1.transform.IsChildOf(v2.transform))
                {
                    return -1;
                }
                else
                {
                    Transform v2Checked = v2.transform;
                    Transform v2CheckedParent = v2Checked.parent;
                    while (v2CheckedParent != root)
                    {
                        Transform v1Checked = v1.transform;
                        Transform v1CheckedParent = v1Checked.parent;
                        while (v1CheckedParent != v2CheckedParent && v1CheckedParent != root)
                        {
                            v1Checked = v1CheckedParent;
                            v1CheckedParent = v1Checked.parent;
                        }
                        if (v1CheckedParent == v2CheckedParent)
                        {
                            return v2Checked.transform.GetSiblingIndex().CompareTo(v1Checked.transform.GetSiblingIndex());
                        }
                        v2Checked = v2CheckedParent;
                        v2CheckedParent = v2CheckedParent.parent;
                    }
                    return 0;
                }
            });
            return goHits2D;
        }

        /// <summary>
        /// 遍历命中对象，查找第一个匹配选中类型的组件并应用选中逻辑
        /// </summary>
        /// <param name="goHits2D">排序后的命中对象数组</param>
        /// <param name="gesture">手势数据</param>
        private void ProcessUIHits(GameObject[] goHits2D, Gesture gesture)
        {
            // 检查当前响应的对象类型是否为指定的对象类型
            for (int index = 0; index < goHits2D.Length; index++)
            {
                Component component = TryFindSelectingComponent(goHits2D[index], out string selectingTypeName);

                // 查到符合条件的控件时表示需要响应选中逻辑
                if (component != null && !string.IsNullOrEmpty(selectingTypeName))
                {
                    Vector3 worldPosition = m_UICamera.ScreenToWorldPoint(new Vector3(gesture.position.x, gesture.position.y, m_UICamera.nearClipPlane));
                    ApplyUISelectionResult(gesture, component, selectingTypeName, worldPosition);
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
        private void ApplyUISelectionResult(Gesture gesture, Component component, string selectingTypeName, Vector3 worldPosition)
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
                    m_CurDragStateOnThisRound = DragState.None;
                    m_CanEnterSelectReboundInSelectHoldMode = true;
                }
                else if (m_SelectedObj != component.gameObject)
                {
                    //Log.Info("{0} is UnSelected!", m_SelectedObjType);
                    FireUnselectedObjCallbacks(gesture, worldPosition);
                    m_LastWorldPosition = Vector3.zero;

                    //Log.Info("{0} is Selected!", m_SelectingTypes[index]);
                    m_SelectedObjType = selectingTypeName;
                    m_SelectedObj = component.gameObject;
                    m_SelectedObjGesture = gesture;
                    FireSelectedObjCallbacks(gesture, worldPosition);
                    m_LastWorldPosition = worldPosition;
                    m_CurDragStateOnThisRound = DragState.None;
                    m_CanEnterSelectReboundInSelectHoldMode = true;
                }
                else  // 继续选中了自身
                {
                    if (m_IsDraging == false)
                    {
                        m_CurDragStateOnThisRound = DragState.None;
                    }
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
                m_CurDragStateOnThisRound = DragState.None;
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
    }
}
#endif