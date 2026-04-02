using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Honor.Runtime
{
    public sealed partial class AorTree : UIBehaviour
    {
        /// <summary>
        /// 唤醒
        /// </summary>
        protected override void Awake()
        {
            m_Pool = new List<GameObject>();
        }

        /// <summary>
        /// 获取组件信息
        /// </summary>
        private void GetComponentInfos()
        {
            m_Container = transform.Find("Viewport/Content");
            if (m_Container.childCount.Equals(0))
            {
                throw new GameException("TreeNode Template 不可为空。需要创建一个 Template 。");
            }
            _mAorTreeRootNode = m_Container.GetChild(0).GetComponent<AorTreeNode>();
        }

        /// <summary>
        /// 递归创建树形目录数据
        /// </summary>
        /// <param name="curIndexCount">当前目录下的层级索引</param>
        /// <param name="jArray">当前树形分支json数据</param>
        /// <param name="indexDesc">当前目录下的层级前缀描述</param>
        /// <returns></returns>
        private AorTreeData CreateTreeData(ref int curIndexCount, JArray jArray, string indexDesc = null)
        {
            string frameName = null;
            string ownIndexDesc = string.Empty;
            if (string.IsNullOrEmpty(indexDesc))
            {
                indexDesc = "1";
                ownIndexDesc = AorTxt.Format("{0}", indexDesc);
            }
            else
            {
                ownIndexDesc = AorTxt.Format("{0}_{1}", indexDesc, ++curIndexCount);
            }

            foreach (var frameData in jArray)
            {
                if (frameData.Type == JTokenType.String)
                {
                    frameName = frameData.ToString();
                    if (jArray.Count == 1)
                    {
                        return new AorTreeData(ownIndexDesc, frameName);
                    }
                }
                else
                {
                    int upLayerIndexCount = curIndexCount;
                    curIndexCount = 0;
                    List<AorTreeData> children = new List<AorTreeData>();
                    foreach (var jd in frameData)
                    {
                        children.Add(CreateTreeData(ref curIndexCount, jd.ToObject<JArray>(), ownIndexDesc));
                    }
                    curIndexCount = upLayerIndexCount;
                    return new AorTreeData(ownIndexDesc, frameName, children);
                }
            }

            return null;
        }

        /// <summary>
        /// 注入树形目录数据
        /// </summary>
        /// <param name="rootData"></param>
        private void Inject(AorTreeData rootData)
        {
            if (null == m_Container)
            {
                GetComponentInfos();
            }
            _mAorTreeRootNode.Inject(rootData);
        }

        /// <summary>
        /// 克隆TreeNode
        /// </summary>
        /// <returns></returns>
        private GameObject CloneTreeNode()
        {
            GameObject result = GameObject.Instantiate(NodePrefab) as GameObject;
            result.transform.SetParent(m_Container, false);
            return result;
        }

    }
}


