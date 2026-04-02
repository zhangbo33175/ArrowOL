
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Honor.Runtime
{
    public sealed partial class AorTree : UIBehaviour
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="jsonTreeInfos">树形目录结构信息（json格式）</param>
        public void Init(string jsonTreeInfos)
        {
            if (string.IsNullOrEmpty(jsonTreeInfos))
            {
                return;
            }

            int curIndexCount = 0;
            JArray jArray = JArray.Parse(jsonTreeInfos);
            AorTreeData data = CreateTreeData(ref curIndexCount, jArray);
            Inject(data);
        }

        /// <summary>
        /// 展开分支
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="siblingIndex"></param>
        /// <returns></returns>
        public List<GameObject> Pop(List<AorTreeData> datas, int siblingIndex)
        {
            List<GameObject> result = new List<GameObject>();
            for (int i = datas.Count - 1; i >= 0; i--)
            {
                result.Add(Pop(datas[i], siblingIndex));
            }
            return result;
        }

        /// <summary>
        /// 展开分支
        /// </summary>
        /// <param name="data"></param>
        /// <param name="siblingIndex"></param>
        /// <returns></returns>
        public GameObject Pop(AorTreeData data, int siblingIndex)
        {
            GameObject treeNode = null;
            if (m_Pool.Count > 0)
            {
                treeNode = m_Pool[0];
                m_Pool.RemoveAt(0);
            }
            else
            {
                treeNode = CloneTreeNode();
            }
            treeNode.transform.SetParent(m_Container, false);
            treeNode.SetActive(true);
            treeNode.GetComponent<AorTreeNode>().Inject(data);
            treeNode.transform.SetSiblingIndex(siblingIndex + 1);
            return treeNode;
        }

        /// <summary>
        /// 合并分支
        /// </summary>
        /// <param name="treeNodes"></param>
        public void Push(List<GameObject> treeNodes)
        {
            foreach (GameObject node in treeNodes)
            {
                Push(node);
            }
        }

        /// <summary>
        /// 合并分支
        /// </summary>
        /// <param name="treeNode"></param>
        public void Push(GameObject treeNode)
        {
            if (null == m_PoolParent)
            {
                m_PoolParent = new GameObject("CachePool").transform;
            }
            treeNode.transform.SetParent(m_PoolParent, false);
            treeNode.SetActive(false);
            m_Pool.Add(treeNode);
        }

    }
}


