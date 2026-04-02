
using System;
using System.Collections.Generic;

namespace Honor.Runtime
{
    public class AorTreeData
    {
        /// <summary>
        /// 父结点数据
        /// </summary>
        public AorTreeData Parent;

        /// <summary>
        /// 孩子结点数据集合
        /// </summary>
        public List<AorTreeData> ChildNodes;

        /// <summary>
        /// 层级索引
        /// A-B-C-D...
        /// </summary>
        public string IndexDesc;

        /// <summary>
        /// 层级数
        /// </summary>
        public int Layer;

        /// <summary>
        /// 名称
        /// </summary>
        public string Name;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="indexDesc">层级索引</param>
        /// <param name="name">名称</param>
        /// <param name="childNodes">孩子结点数据集合</param>
        /// <param name="layer">层级数</param>
        public AorTreeData(string indexDesc, string name, List<AorTreeData> childNodes = null, int layer = 0)
        {
            IndexDesc = indexDesc;
            Name = name;
            Parent = null;
            ChildNodes = childNodes;
            if (ChildNodes == null)
            {
                ChildNodes = new List<AorTreeData>();
            }
            Layer = layer;
            ResetChildren(this);
        }

        /// <summary>
        /// 设置父对象数据
        /// </summary>
        /// <param name="parent">父对象数据</param>
        public void SetParent(AorTreeData parent)
        {
            if (Parent != null)
            {
                Parent.RemoveChild(this);
            }
            Parent = parent;
            Layer = parent.Layer + 1;
            parent.ChildNodes.Add(this);
            ResetChildren(this);
        }

        /// <summary>
        /// 添加孩子结点数据
        /// </summary>
        /// <param name="child">孩子结点数据</param>
        public void AddChild(AorTreeData child)
        {
            AddChild(new AorTreeData[] { child });
        }

        /// <summary>
        /// 添加孩子结点数据集合
        /// </summary>
        /// <param name="children">孩子结点数据集合</param>
        public void AddChild(IEnumerable<AorTreeData> children)
        {
            foreach (AorTreeData child in children)
            {
                child.SetParent(this);
            }
        }

        /// <summary>
        /// 移除孩子结点数据
        /// </summary>
        /// <param name="child">孩子结点数据</param>
        public void RemoveChild(AorTreeData child)
        {
            RemoveChild(new AorTreeData[] { child });
        }

        /// <summary>
        /// 移除孩子结点数据集合
        /// </summary>
        /// <param name="children">孩子结点数据集合</param>
        public void RemoveChild(IEnumerable<AorTreeData> children)
        {
            foreach (AorTreeData child in children)
            {
                for (int i = 0; i < ChildNodes.Count; i++)
                {
                    if (child == ChildNodes[i])
                    {
                        ChildNodes.Remove(ChildNodes[i]);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 清空孩子结点数据集合
        /// </summary>
        public void ClearChildren()
        {
            ChildNodes = null;
        }

        /// <summary>
        /// 复位指定结点的孩子结点数据
        /// </summary>
        /// <param name="aorTreeData"></param>
        private void ResetChildren(AorTreeData aorTreeData)
        {
            for (int i = 0; i < aorTreeData.ChildNodes.Count; i++)
            {
                AorTreeData node = aorTreeData.ChildNodes[i];
                node.Parent = aorTreeData;
                node.Layer = aorTreeData.Layer + 1;
                ResetChildren(node);
            }
        }

        /// <summary>
        /// 是否相等
        /// </summary>
        /// <param name="obj">obj</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            AorTreeData other = obj as AorTreeData;
            if (other == null)
            {
                return false;
            }
            return other.Name.Equals(Name) && other.Layer.Equals(Layer);
        }

        /// <summary>
        /// 获取hash值
        /// </summary>
        /// <returns></returns>
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

    }
}


