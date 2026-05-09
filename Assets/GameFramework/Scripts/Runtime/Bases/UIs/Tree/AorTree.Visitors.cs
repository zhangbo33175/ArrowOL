using System;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace Honor.Runtime
{
    /// <summary>
    /// UI 树形折叠菜单 - 字段定义部分
    /// 管理树形结构的图标、节点、容器、对象池、事件回调
    /// </summary>
    public sealed partial class AorTree : UIBehaviour
    {
        /// <summary>
        /// 节点展开状态图标
        /// </summary>
        public Sprite OpenIcon;

        /// <summary>
        /// 节点关闭状态图标
        /// </summary>
        public Sprite CloseIcon;

        /// <summary>
        /// 叶子节点（无子节点）状态图标
        /// </summary>
        public Sprite FinalIcon;

        /// <summary>
        /// 树形结构根节点（顶层节点）
        /// </summary>
        [HideInInspector]
        private AorTreeNode _mAorTreeRootNode;
        
        /// <summary>
        /// 树形结构根节点（外部访问属性）
        /// </summary>
        public AorTreeNode AorTreeRootNode
        {
            get { return _mAorTreeRootNode; }
            set { _mAorTreeRootNode = value; }
        }

        /// <summary>
        /// 树节点容器（Viewport/Content）
        /// </summary>
        private Transform m_Container;
        
        /// <summary>
        /// 树节点容器（只读属性）
        /// </summary>
        public Transform Container
        {
            get { return m_Container; }
        }

        /// <summary>
        /// 树节点预制体模板
        /// </summary>
        private GameObject m_NodePrefab = null;

        /// <summary>
        /// 节点对象池（复用隐藏的节点，减少GC）
        /// </summary>
        private List<GameObject> m_Pool;

        /// <summary>
        /// 对象池缓存父物体（统一存放隐藏节点）
        /// </summary>
        private Transform m_PoolParent = null;

        /// <summary>
        /// 树节点预制体（自动获取容器下第一个子节点作为模板）
        /// </summary>
        public GameObject NodePrefab
        {
            get
            {
                if (m_NodePrefab == null)
                {
                    m_NodePrefab = m_Container.GetChild(0).gameObject;
                }
                return m_NodePrefab;
            }
            set
            {
                m_NodePrefab = value;
            }
        }

        /// <summary>
        /// 节点选中回调事件
        /// 向外传递选中节点的索引描述字符串，用于Lua层逻辑处理
        /// </summary>
        public Action<string> onChosen;
    }
}