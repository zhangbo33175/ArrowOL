
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace Honor.Runtime
{
    public sealed partial class AorTreeNode : UIBehaviour
    {
        /// <summary>
        /// 注入树形数据
        /// </summary>
        /// <param name="data"></param>
        public void Inject(AorTreeData data)
        {
            if (m_MyTransform == null)
            {
                GetComponentInfos();
            }
            ResetComponentInfos();
            _mAorTreeData = data;
            m_Text.text = data.Name;
            m_Toggle.isOn = false;
            m_ContainerButton.onClick.AddListener(OpenOrClose);

            m_ContainerButton.transform.localPosition += new Vector3(_mAorTree.Container.GetComponent<GridLayoutGroup>().cellSize.y * _mAorTreeData.Layer, 0, 0);
            if (data.ChildNodes.Count.Equals(0))
            {
                m_ToggleTransform.gameObject.SetActive(false);
                m_Icon.sprite = _mAorTree.FinalIcon;
            }
            else
            {
                m_Icon.sprite = m_Toggle.isOn ? _mAorTree.OpenIcon : _mAorTree.CloseIcon;
            }
        }

        /// <summary>
        /// 打开或关闭分支
        /// </summary>
        public void OpenOrClose()
        {
            m_Toggle.isOn = !m_Toggle.isOn;

            if (m_Toggle.isOn)
            {
                OpenChildren();
            }
            else
            {
                CloseChildren();
            }

            if (_mAorTreeData.ChildNodes.Count > 0)
            {
                m_ToggleTransform.localEulerAngles = m_Toggle.isOn ? new Vector3(0, 0, 0) : new Vector3(0, 0, 90);
                m_Icon.sprite = m_Toggle.isOn ? _mAorTree.OpenIcon : _mAorTree.CloseIcon;
            }

            if (_mAorTree.onChosen != null)
            {
                _mAorTree.onChosen(_mAorTreeData.IndexDesc);
            }

        }

    }
}


