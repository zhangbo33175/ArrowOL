using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace Honor.Runtime
{
    /// <summary>
    /// 树形菜单节点 UI 行为类
    /// 负责节点的组件获取、状态重置、子节点展开/关闭、事件监听管理
    /// </summary>
    public sealed partial class AorTreeNode : UIBehaviour
    {
        /// <summary>
        /// 生命周期：Awake
        /// 初始化子节点对象列表
        /// </summary>
        protected override void Awake()
        {
            m_Children = new List<GameObject>();
        }

        /// <summary>
        /// 获取节点所需的所有 UI 组件引用
        /// 包括按钮、Toggle、图标、文本、箭头、根树组件
        /// </summary>
        private void GetComponentInfos()
        {
            m_MyTransform = this.transform;
            
            // 节点容器按钮
            m_ContainerButton = m_MyTransform.Find("ContainerButton").GetComponent<Button>();
            // 展开/关闭开关
            m_Toggle = m_ContainerButton.transform.Find("Toggle").GetComponent<Toggle>();
            // 节点图标
            m_Icon = m_ContainerButton.transform.Find("IconContainer/Icon").GetComponent<Image>();
            // 节点显示文本
            m_Text = m_ContainerButton.transform.Find("Text").GetComponent<Text>();
            // 箭头旋转对象
            m_ToggleTransform = m_Toggle.transform.Find("Image");
            // 获取顶层树管理组件
            _mAorTree = m_MyTransform.parent.parent.parent.GetComponent<AorTree>();
        }

        /// <summary>
        /// 重置节点 UI 状态
        /// 复位位置、箭头旋转、显示箭头
        /// </summary>
        private void ResetComponentInfos()
        {
            // 复位水平位置
            m_ContainerButton.transform.localPosition = new Vector3(0, m_ContainerButton.transform.localPosition.y, 0);
            // 箭头默认旋转90度（关闭状态）
            m_ToggleTransform.localEulerAngles = new Vector3(0, 0, 90);
            // 显示箭头
            m_ToggleTransform.gameObject.SetActive(true);
        }

        /// <summary>
        /// 展开子节点
        /// 调用树管理器的 Pop 方法，创建/复用子节点 UI
        /// </summary>
        private void OpenChildren()
        {
            m_Children = _mAorTree.Pop(_mAorTreeData.ChildNodes, transform.GetSiblingIndex());
        }

        /// <summary <
        /// 关闭子节点
        /// 递归关闭所有子节点，移除监听，回收对象池
        /// </summary>
        protected void CloseChildren()
        {
            // 递归关闭所有子节点
            for (int i = 0; i < m_Children.Count; i++)
            {
                AorTreeNode node = m_Children[i].GetComponent<AorTreeNode>();
                node.RemoveListener();
                node.CloseChildren();
            }

            // 回收所有子节点到对象池
            _mAorTree.Push(m_Children);
            // 清空列表
            m_Children = new List<GameObject>();
        }

        /// <summary>
        /// 移除节点点击事件监听
        /// 防止对象池复用后多次注册
        /// </summary>
        private void RemoveListener()
        {
            m_ContainerButton.onClick.RemoveListener(OpenOrClose);
        }
    }
}