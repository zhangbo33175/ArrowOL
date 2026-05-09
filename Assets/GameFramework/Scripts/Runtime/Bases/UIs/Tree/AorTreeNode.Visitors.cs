using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace Honor.Runtime
{
    /// <summary>
    /// 树形菜单节点 - 字段定义部分
    /// 存储节点数据、UI组件引用、子节点对象、树管理器引用
    /// </summary>
    public sealed partial class AorTreeNode : UIBehaviour
    {
        /// <summary>
        /// 当前节点绑定的数据模型（名称、层级、父子关系）
        /// </summary>
        private AorTreeData _mAorTreeData;

        /// <summary>
        /// 所属的树形菜单根管理器
        /// </summary>
        private AorTree _mAorTree;

        /// <summary>
        /// 展开/关闭 开关组件
        /// </summary>
        private Toggle m_Toggle;

        /// <summary>
        /// 节点状态图标（打开/关闭/叶子节点）
        /// </summary>
        private Image m_Icon;

        /// <summary>
        /// 节点显示文本
        /// </summary>
        private Text m_Text;

        /// <summary>
        /// 箭头图标对象（用于旋转动画）
        /// </summary>
        private Transform m_ToggleTransform;

        /// <summary>
        /// 自身节点 Transform 缓存
        /// </summary>
        private Transform m_MyTransform;

        /// <summary>
        /// 节点容器按钮（点击区域）
        /// 承载 Toggle、Icon、Text 等子元素
        /// </summary>
        private Button m_ContainerButton;

        /// <summary>
        /// 子节点 UI 对象列表
        /// </summary>
        private List<GameObject> m_Children;
    }
}