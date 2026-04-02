
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace Honor.Runtime
{
    public sealed partial class AorTreeNode : UIBehaviour
    {
        /// <summary>
        /// 结点数据
        /// </summary>
        private AorTreeData _mAorTreeData;

        /// <summary>
        /// 树形目录对象
        /// </summary>
        private AorTree _mAorTree;

        /// <summary>
        /// Toggle勾选项
        /// </summary>
        private Toggle m_Toggle;

        /// <summary>
        /// Icon图标
        /// </summary>
        private Image m_Icon;

        /// <summary>
        /// 文本内容
        /// </summary>
        private Text m_Text;

        /// <summary>
        /// Toggle的Transform
        /// </summary>
        private Transform m_ToggleTransform;

        /// <summary>
        /// 自身Transform
        /// </summary>
        private Transform m_MyTransform;

        /// <summary>
        /// 容器(Button)
        /// 用于挂载Toggle/Image/Text
        /// </summary>
        private Button m_ContainerButton;

        /// <summary>
        /// 孩子结点对象集合
        /// </summary>
        private List<GameObject> m_Children;

    }
}


