/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  AorTreeNode.Core.cs
 * author:    云毅
 * created:   2026 2025
 * descrip:   UI 树形列表 - 节点核心逻辑（partial）
 ***************************************************************/

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace Honor.Runtime
{
    /// <summary>
    /// 树形菜单节点（单个TreeItem）
    /// 功能：节点显示、展开/关闭、层级缩进、点击事件、图标切换
    /// </summary>
    public sealed partial class AorTreeNode : UIBehaviour
    {
        //=========================================================================
        // 数据注入 & 初始化
        //=========================================================================
        #region Method - 注入数据
        /// <summary>
        /// 注入树节点数据，初始化节点显示状态
        /// </summary>
        /// <param name="data">节点数据</param>
        public void Inject(AorTreeData data)
        {
            // 自动获取组件信息
            if (m_MyTransform == null)
            {
                GetComponentInfos();
            }

            // 重置组件状态
            ResetComponentInfos();

            // 绑定数据
            _mAorTreeData = data;
            // 设置显示文本
            m_Text.text = data.Name;
            // 默认关闭状态
            m_Toggle.isOn = false;
            // 注册点击事件
            m_ContainerButton.onClick.AddListener(OpenOrClose);

            // 根据层级设置缩进（向右偏移 = 层级 × 单元格高度）
            float cellHeight = _mAorTree.Container.GetComponent<GridLayoutGroup>().cellSize.y;
            m_ContainerButton.transform.localPosition += new Vector3(cellHeight * _mAorTreeData.Layer, 0, 0);

            // 判断是否为叶子节点（无子节点）
            if (data.ChildNodes.Count == 0)
            {
                // 隐藏展开箭头
                m_ToggleTransform.gameObject.SetActive(false);
                // 设置叶子节点图标
                m_Icon.sprite = _mAorTree.FinalIcon;
            }
            else
            {
                // 根据开关状态设置展开/关闭图标
                m_Icon.sprite = m_Toggle.isOn ? _mAorTree.OpenIcon : _mAorTree.CloseIcon;
            }
        }
        #endregion

        //=========================================================================
        // 展开 / 关闭 子节点
        //=========================================================================
        #region Method - 展开关闭逻辑
        /// <summary>
        /// 打开/关闭子节点分支
        /// 切换开关状态、显示/隐藏子节点、切换图标、触发选中回调
        /// </summary>
        public void OpenOrClose()
        {
            // 切换开关状态
            m_Toggle.isOn = !m_Toggle.isOn;

            // 执行展开/关闭逻辑
            if (m_Toggle.isOn)
            {
                OpenChildren();
            }
            else
            {
                CloseChildren();
            }

            // 有子节点时，更新箭头旋转角度和图标
            if (_mAorTreeData.ChildNodes.Count > 0)
            {
                // 箭头旋转：打开0度，关闭90度
                m_ToggleTransform.localEulerAngles = m_Toggle.isOn ? new Vector3(0, 0, 0) : new Vector3(0, 0, 90);
                // 切换打开/关闭图标
                m_Icon.sprite = m_Toggle.isOn ? _mAorTree.OpenIcon : _mAorTree.CloseIcon;
            }

            // 触发节点选中回调（向外传递IndexDesc）
            if (_mAorTree.onChosen != null)
            {
                _mAorTree.onChosen(_mAorTreeData.IndexDesc);
            }
        }
        #endregion
        
    }
}