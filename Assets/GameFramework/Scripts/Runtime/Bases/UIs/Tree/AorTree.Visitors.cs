
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace Honor.Runtime
{
    public sealed partial class AorTree : UIBehaviour
    {
        /// <summary>
        /// 打开式Icon
        /// </summary>
        public Sprite OpenIcon;

        /// <summary>
        /// 关闭式Icon
        /// </summary>
        public Sprite CloseIcon;

        /// <summary>
        /// 叶子结点Icon
        /// </summary>
        public Sprite FinalIcon;

        /// <summary>
        /// 树形目录根结点
        /// </summary>
        [HideInInspector]
        private AorTreeNode _mAorTreeRootNode;
        public AorTreeNode AorTreeRootNode
        {
            get
            {
                return _mAorTreeRootNode;
            }
            set
            {
                _mAorTreeRootNode = value;
            }
        }

        /// <summary>
        /// 树形目录容器
        /// </summary>
        private Transform m_Container;
        public Transform Container
        {
            get
            {
                return m_Container;
            }
        }

        /// <summary>
        /// 结点模板对象
        /// </summary>
        private GameObject m_NodePrefab = null;

        /// <summary>
        /// 对象缓存池
        /// </summary>
        private List<GameObject> m_Pool;

        /// <summary>
        /// 对象缓存池父结点
        /// </summary>
        private Transform m_PoolParent = null;

        /// <summary>
        /// 结点模板对象
        /// 用来实例化与克隆
        /// </summary>
        public GameObject NodePrefab
        {
            get
            {
                return m_NodePrefab ?? (m_NodePrefab = m_Container.GetChild(0).gameObject);
            }
            set
            {
                m_NodePrefab = value;
            }
        }

        /// <summary>
        /// 选中事件回调
        /// 回调到Lua层的事件监听
        /// </summary>
        public Action<string> onChosen;

    }
}


