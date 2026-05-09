using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Honor.Runtime
{
    /// <summary>
    /// 树形列表/折叠菜单 管理类（UI 树组件）
    /// 功能：解析JSON数据生成树形结构、节点对象池管理、展开/合并子节点
    /// 基于对象池优化，避免频繁创建/销毁UI节点
    /// </summary>
    public sealed partial class AorTree : UIBehaviour
    {
        /// <summary>
        /// 初始化树形结构
        /// 解析JSON格式的树配置数据，构建树节点数据并注入显示
        /// </summary>
        /// <param name="jsonTreeInfos">树形目录结构JSON字符串</param>
        public void Init(string jsonTreeInfos)
        {
            // 空数据直接返回
            if (string.IsNullOrEmpty(jsonTreeInfos))
            {
                return;
            }

            int curIndexCount = 0;
            // 解析JSON数组
            JArray jArray = JArray.Parse(jsonTreeInfos);
            // 递归构建树数据结构
            AorTreeData data = CreateTreeData(ref curIndexCount, jArray);
            // 注入树结构，生成UI
            Inject(data);
        }

        /// <summary>
        /// 批量展开一组分支节点（逆序展开）
        /// 从列表末尾向前依次创建节点并插入UI
        /// </summary>
        /// <param name="datas">子节点数据列表</param>
        /// <param name="siblingIndex">插入的父节点位置</param>
        /// <returns>创建出的节点对象列表</returns>
        public List<GameObject> Pop(List<AorTreeData> datas, int siblingIndex)
        {
            List<GameObject> result = new List<GameObject>();
            // 倒序遍历，保证UI显示顺序正确
            for (int i = datas.Count - 1; i >= 0; i--)
            {
                result.Add(Pop(datas[i], siblingIndex));
            }
            return result;
        }

        /// <summary>
        /// 展开单个分支（从对象池获取/新建节点）
        /// 激活节点、设置数据、设置UI层级
        /// </summary>
        /// <param name="data">树节点数据</param>
        /// <param name="siblingIndex">节点显示位置</param>
        /// <returns>初始化完成的树节点对象</returns>
        public GameObject Pop(AorTreeData data, int siblingIndex)
        {
            GameObject treeNode = null;

            // 对象池复用：优先使用缓存节点
            if (m_Pool.Count > 0)
            {
                treeNode = m_Pool[0];
                m_Pool.RemoveAt(0);
            }
            // 无缓存则新建节点
            else
            {
                treeNode = CloneTreeNode();
            }

            // 设置父物体、激活、绑定数据
            treeNode.transform.SetParent(m_Container, false);
            treeNode.SetActive(true);
            treeNode.GetComponent<AorTreeNode>().Inject(data);
            // 设置在UI Hierarchy中的显示顺序
            treeNode.transform.SetSiblingIndex(siblingIndex + 1);

            return treeNode;
        }

        /// <summary>
        /// 批量回收一组节点到对象池
        /// </summary>
        /// <param name="treeNodes">需要回收的节点列表</param>
        public void Push(List<GameObject> treeNodes)
        {
            foreach (GameObject node in treeNodes)
            {
                Push(node);
            }
        }

        /// <summary>
        /// 回收单个节点到对象池
        /// 隐藏节点、移至缓存父物体、加入缓存列表
        /// </summary>
        /// <param name="treeNode">需要回收的树节点</param>
        public void Push(GameObject treeNode)
        {
            // 懒加载：首次回收时创建缓存容器
            if (null == m_PoolParent)
            {
                m_PoolParent = new GameObject("CachePool").transform;
            }

            // 移至缓存池、隐藏
            treeNode.transform.SetParent(m_PoolParent, false);
            treeNode.SetActive(false);
            // 加入缓存列表
            m_Pool.Add(treeNode);
        }
    }
}