
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace Honor.Runtime
{
    public sealed partial class AorTreeNode : UIBehaviour
    {
        /// <summary>
        /// 唤醒
        /// </summary>
        protected override void Awake()
        {
            m_Children = new List<GameObject>();
        }

        /// <summary>
        /// 获取组件信息
        /// </summary>
        private void GetComponentInfos()
        {
            m_MyTransform = this.transform;
            m_ContainerButton = m_MyTransform.Find("ContainerButton").GetComponent<Button>();
            m_Toggle = m_ContainerButton.transform.Find("Toggle").GetComponent<Toggle>();
            m_Icon = m_ContainerButton.transform.Find("IconContainer/Icon").GetComponent<Image>();
            m_Text = m_ContainerButton.transform.Find("Text").GetComponent<Text>();
            m_ToggleTransform = m_Toggle.transform.Find("Image");
            _mAorTree = m_MyTransform.parent.parent.parent.GetComponent<AorTree>();
        }

        /// <summary>
        /// 复位组件信息
        /// </summary>
        private void ResetComponentInfos()
        {
            m_ContainerButton.transform.localPosition = new Vector3(0, m_ContainerButton.transform.localPosition.y, 0);
            m_ToggleTransform.localEulerAngles = new Vector3(0, 0, 90);
            m_ToggleTransform.gameObject.SetActive(true);
        }

        /// <summary>
        /// 打开孩子分支
        /// </summary>
        private void OpenChildren()
        {
            m_Children = _mAorTree.Pop(_mAorTreeData.ChildNodes, transform.GetSiblingIndex());
        }

        /// <summary>
        /// 关闭孩子分支
        /// </summary>
        protected void CloseChildren()
        {
            for (int i = 0; i < m_Children.Count; i++)
            {
                AorTreeNode node = m_Children[i].GetComponent<AorTreeNode>();
                node.RemoveListener();
                node.CloseChildren();
            }
            _mAorTree.Push(m_Children);
            m_Children = new List<GameObject>();
        }

        /// <summary>
        /// 移除Button监听
        /// </summary>
        private void RemoveListener()
        {
            m_ContainerButton.onClick.RemoveListener(OpenOrClose);
        }

    }
}


