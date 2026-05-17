/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  AorTree.cs
 * author:    云毅
 *created:   2026
 * descrip: UI 树形折叠列表核心管理类
 ***************************************************************/

using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Honor.Runtime
{
    /// <summary>
    /// UI 树形折叠列表核心管理类
    /// 功能：JSON数据解析、树形结构构建、节点对象池管理、UI节点创建与回收
    /// 基于对象池实现高性能树形菜单，支持多级折叠/展开
    /// </summary>
    public sealed partial class AorTree : UIBehaviour
    {
        //=========================================================================
        // 生命周期
        //=========================================================================
        #region MonoBehaviour
        /// <summary>
        /// 生命周期：Awake
        /// 初始化节点对象池
        /// </summary>
        protected override void Awake()
        {
            // 初始化节点缓存池
            m_Pool = new List<GameObject>();
        }
        #endregion

        //=========================================================================
        // 组件获取
        //=========================================================================
        #region Method - 组件获取
        /// <summary>
        /// 获取UI组件信息
        /// 查找容器、根节点模板，校验节点模板是否存在
        /// </summary>
        private void GetComponentInfos()
        {
            // 查找滚动视图内容容器
            m_Container = transform.Find("Viewport/Content");

            // 容器内必须有一个节点模板，否则抛出异常
            if (m_Container.childCount.Equals(0))
            {
                throw new GameException("TreeNode Template 不可为空。需要创建一个 Template 。");
            }

            // 获取根节点组件
            _mAorTreeRootNode = m_Container.GetChild(0).GetComponent<AorTreeNode>();
        }
        #endregion

        //=========================================================================
        // 树形数据构建
        //=========================================================================
        #region Method - 数据解析
        /// <summary>
        /// 递归创建树形数据结构
        /// 将JSON数组解析为树节点数据，自动构建层级关系
        /// </summary>
        /// <param name="curIndexCount">当前层级索引计数器</param>
        /// <param name="jArray">JSON树形结构数据</param>
        /// <param name="indexDesc">层级路径描述（如：1_1_2）</param>
        /// <returns>构建完成的树形数据节点</returns>
        private AorTreeData CreateTreeData(ref int curIndexCount, JArray jArray, string indexDesc = null)
        {
            string frameName = null;
            string ownIndexDesc = string.Empty;

            // 构建层级路径描述
            if (string.IsNullOrEmpty(indexDesc))
            {
                indexDesc = "1";
                ownIndexDesc = AorTxt.Format("{0}", indexDesc);
            }
            else
            {
                ownIndexDesc = AorTxt.Format("{0}_{1}", indexDesc, ++curIndexCount);
            }

            // 遍历JSON数据，递归构建子节点
            foreach (var frameData in jArray)
            {
                // 字符串类型为节点名称
                if (frameData.Type == JTokenType.String)
                {
                    frameName = frameData.ToString();

                    // 无子节点，直接创建叶子节点
                    if (jArray.Count == 1)
                    {
                        return new AorTreeData(ownIndexDesc, frameName);
                    }
                }
                // 数组类型为子节点
                else
                {
                    // 保存上层索引，递归处理子节点
                    int upLayerIndexCount = curIndexCount;
                    curIndexCount = 0;

                    List<AorTreeData> children = new List<AorTreeData>();
                    foreach (var jd in frameData)
                    {
                        children.Add(CreateTreeData(ref curIndexCount, jd.ToObject<JArray>(), ownIndexDesc));
                    }

                    // 恢复上层索引
                    curIndexCount = upLayerIndexCount;
                    return new AorTreeData(ownIndexDesc, frameName, children);
                }
            }

            return null;
        }
        #endregion

        //=========================================================================
        // 数据注入
        //=========================================================================
        #region Method - 注入
        /// <summary>
        /// 注入树形数据到根节点
        /// 开始构建整个树形UI
        /// </summary>
        /// <param name="rootData">根节点数据</param>
        private void Inject(AorTreeData rootData)
        {
            // 自动获取组件信息
            if (null == m_Container)
            {
                GetComponentInfos();
            }

            // 数据注入根节点
            _mAorTreeRootNode.Inject(rootData);
        }
        #endregion

        //=========================================================================
        // 节点克隆
        //=========================================================================
        #region Method - 克隆节点
        /// <summary>
        /// 克隆树节点预制体
        /// 创建新的UI节点并设置到容器中
        /// </summary>
        /// <returns>克隆后的节点对象</returns>
        private GameObject CloneTreeNode()
        {
            GameObject result = GameObject.Instantiate(NodePrefab) as GameObject;
            result.transform.SetParent(m_Container, false);
            return result;
        }
        #endregion
    }
}