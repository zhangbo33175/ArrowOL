/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  AorTreeData.cs
 * author:    云毅
 *created:   2026
 * descrip:   UI 树形列表 - 节点数据模型
 ***************************************************************/

using System;
using System.Collections.Generic;

namespace Honor.Runtime
{
    /// <summary>
    /// 树形结构数据模型
    /// 用于存储树节点的层级关系、名称、父子节点引用
    /// 支持动态添加/移除子节点、自动维护层级深度
    /// </summary>
    public class AorTreeData
    {
        //=========================================================================
        // 公共字段
        //=========================================================================
        #region Fields
        /// <summary>
        /// 父节点数据
        /// </summary>
        public AorTreeData Parent;

        /// <summary>
        /// 子节点数据列表
        /// </summary>
        public List<AorTreeData> ChildNodes;

        /// <summary>
        /// 层级索引描述（格式：1_1_2）
        /// 用于唯一标识节点在树中的路径位置
        /// </summary>
        public string IndexDesc;

        /// <summary>
        /// 节点所在层级（深度）
        /// 根节点为 0，每深入一级 +1
        /// </summary>
        public int Layer;

        /// <summary>
        /// 节点显示名称
        /// </summary>
        public string Name;
        #endregion

        //=========================================================================
        // 构造函数
        //=========================================================================
        #region Constructor
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="indexDesc">层级索引描述</param>
        /// <param name="name">节点名称</param>
        /// <param name="childNodes">子节点列表（可为空）</param>
        /// <param name="layer">节点层级深度</param>
        public AorTreeData(string indexDesc, string name, List<AorTreeData> childNodes = null, int layer = 0)
        {
            IndexDesc = indexDesc;
            Name = name;
            Parent = null;
            ChildNodes = childNodes;
            
            // 确保子节点列表不为null
            if (ChildNodes == null)
            {
                ChildNodes = new List<AorTreeData>();
            }
            
            Layer = layer;
            // 递归设置所有子节点的层级与父引用
            ResetChildren(this);
        }
        #endregion

        //=========================================================================
        // 父子节点管理
        //=========================================================================
        #region Parent & Children
        /// <summary>
        /// 设置节点的父节点
        /// 自动处理旧父节点移除、层级更新、子节点添加
        /// </summary>
        /// <param name="parent">新的父节点</param>
        public void SetParent(AorTreeData parent)
        {
            // 如果已有父节点，先从旧父节点的子列表中移除
            if (Parent != null)
            {
                Parent.RemoveChild(this);
            }
            
            Parent = parent;
            Layer = parent.Layer + 1;
            parent.ChildNodes.Add(this);
            
            // 递归更新所有子节点的层级
            ResetChildren(this);
        }

        /// <summary>
        /// 添加单个子节点
        /// </summary>
        /// <param name="child">子节点数据</param>
        public void AddChild(AorTreeData child)
        {
            AddChild(new AorTreeData[] { child });
        }

        /// <summary>
        /// 批量添加子节点
        /// </summary>
        /// <param name="children">子节点集合</param>
        public void AddChild(IEnumerable<AorTreeData> children)
        {
            foreach (AorTreeData child in children)
            {
                child.SetParent(this);
            }
        }

        /// <summary>
        /// 移除单个子节点
        /// </summary>
        /// <param name="child">要移除的子节点</param>
        public void RemoveChild(AorTreeData child)
        {
            RemoveChild(new AorTreeData[] { child });
        }

        /// <summary>
        /// 批量移除子节点
        /// </summary>
        /// <param name="children">要移除的子节点集合</param>
        public void RemoveChild(IEnumerable<AorTreeData> children)
        {
            foreach (AorTreeData child in children)
            {
                for (int i = 0; i < ChildNodes.Count; i++)
                {
                    if (child == ChildNodes[i])
                    {
                        ChildNodes.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 清空所有子节点
        /// </summary>
        public void ClearChildren()
        {
            ChildNodes.Clear();
        }
        #endregion

        //=========================================================================
        // 私有递归方法
        //=========================================================================
        #region Private
        /// <summary>
        /// 递归重置子节点的父引用与层级深度
        /// 保证树结构层级关系正确
        /// </summary>
        /// <param name="aorTreeData">需要重置子节点的根节点</param>
        private void ResetChildren(AorTreeData aorTreeData)
        {
            foreach (AorTreeData node in aorTreeData.ChildNodes)
            {
                node.Parent = aorTreeData;
                node.Layer = aorTreeData.Layer + 1;
                // 递归处理深层子节点
                ResetChildren(node);
            }
        }
        #endregion

        //=========================================================================
        // 重写方法
        //=========================================================================
        #region Override
        /// <summary>
        /// 重写相等判断
        /// 根据【名称 + 层级】判断是否为同一节点
        /// </summary>
        public override bool Equals(object obj)
        {
            AorTreeData other = obj as AorTreeData;
            if (other == null)
                return false;
            
            return other.Name.Equals(Name) && other.Layer.Equals(Layer);
        }

        /// <summary>
        /// 重写哈希码
        /// 用于字典、哈希表等集合类型
        /// </summary>
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Parent != null ? Parent.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (ChildNodes != null ? ChildNodes.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Layer;
                hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                return hashCode;
            }
        }
        #endregion
    }
}