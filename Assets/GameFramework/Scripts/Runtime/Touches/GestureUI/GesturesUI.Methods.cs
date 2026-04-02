#if EASY_TOUCH_ENABLE
namespace Honor.Runtime
{
    using HedgehogTeam.EasyTouch;
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using XLua;

    public sealed partial class GesturesUI : MonoBehaviour
    {
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

        /// <summary>
        /// 检查选中逻辑
        /// </summary>
        /// <param name="gesture"></param>
        private void CheckSelect(Gesture gesture)
        {
            if (!m_SelectSwitch) return;

            // 处理选中状态回调
            if (gesture.touchCount == 1)
            {
                if (gesture.type == EasyTouch.EvtType.On_OverUIElement)
                {
                    if (gesture.actionTime >= m_PressTimeOfSelectingObj)  // 响应选中操作
                    {
                        if ((gesture.pickedUIElement != null && !m_SelectHoldMode && m_SelectedObj == null) || m_SelectHoldMode)
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
                                if(v2.transform.IsChildOf(v1.transform))
                                {
                                    return 1;
                                }
                                else if(v1.transform.IsChildOf(v2.transform))
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

                            // 检查当前响应的对象类型是否为指定的对象类型
                            for (int index = 0; index < goHits2D.Length; index++)
                            {
                                Component component = null;
                                string selectingTypeName = null;
                                // 在自身中查询
                                if (component == null && m_FindSelectingTypesOnSelf)
                                {
                                    selectingTypeName = m_SelectingTypes.Find((name) =>
                                    {
                                        if (goHits2D[index].transform.GetLua(name))
                                        {
                                            return true;
                                        }
                                        return false;
                                    });
                                    if (!string.IsNullOrEmpty(selectingTypeName))
                                    {
                                        component = goHits2D[index].transform.GetLua(selectingTypeName);
                                    }
                                }
                                // 在父对象中查询
                                if (component == null && m_FindSelectingTypesOnParent)
                                {
                                    selectingTypeName = m_SelectingTypes.Find((name) =>
                                    {
                                        if (goHits2D[index].transform.GetLuaInParent(name))
                                        {
                                            return true;
                                        }
                                        return false;
                                    });
                                    if (!string.IsNullOrEmpty(selectingTypeName))
                                    {
                                        component = goHits2D[index].transform.GetLuaInParent(selectingTypeName);
                                    }
                                }
                                // 在子对象中查询
                                if (component == null && m_FindSelectingTypesOnChildren)
                                {
                                    selectingTypeName = m_SelectingTypes.Find((name) =>
                                    {
                                        if (goHits2D[index].transform.GetLuaInChildren(name))
                                        {
                                            return true;
                                        }
                                        return false;
                                    });
                                    if (!string.IsNullOrEmpty(selectingTypeName))
                                    {
                                        component = goHits2D[index].transform.GetLuaInChildren(selectingTypeName);
                                    }
                                }
                                // 查到符合条件的控件时表示需要响应选中逻辑
                                if (component != null && !string.IsNullOrEmpty(selectingTypeName))
                                {
                                    Vector3 worldPosition = m_UICamera.ScreenToWorldPoint(new Vector3(gesture.position.x, gesture.position.y, m_UICamera.nearClipPlane));

                                    // 常选中模式下切换选中对象
                                    if (m_SelectHoldMode)
                                    {
                                        if (m_SelectedObj == null)
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
                                            m_CurDragStateOnThisRound = DragState.None;
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

                                            m_LastWorldPosition = Vector3.zero;

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
                                            m_CurDragStateOnThisRound = DragState.None;
                                            m_CanEnterSelectReboundInSelectHoldMode = true;
                                        }
                                        else  // 继续选中了自身
                                        {
                                            if(m_IsDraging == false)
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
                                        m_CurDragStateOnThisRound = DragState.None;
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
}
#endif