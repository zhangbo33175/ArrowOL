/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  AorTextEffectGradient.cs
 * author:    云毅
 * created:   2026   2025
 * descrip:   UI 垂直渐变特效 | 支持 Text/Image 顶点颜色渐变
 ***************************************************************/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Honor.Runtime
{
    /// <summary>
    /// UI文本/图像垂直渐变效果组件
    /// 基于UGUI BaseMeshEffect扩展，实现自上而下的颜色渐变
    /// 可挂载在Text、Image等任意MaskableGraphic组件上
    /// </summary>
    [AddComponentMenu("UI/Honor/UI文本/图像垂直渐变效果组件")]
    public class AorTextEffectGradient : BaseMeshEffect
    {
        //=========================================================================
        // 序列化字段
        //=========================================================================
        #region Field - 渐变颜色
        /// <summary>
        /// 渐变顶部颜色
        /// </summary>
        [SerializeField]
        private Color32 topColor = Color.white;

        /// <summary>
        /// 渐变底部颜色
        /// </summary>
        [SerializeField]
        private Color32 bottomColor = Color.black;
        #endregion

        //=========================================================================
        // 重写方法 - 网格修改
        //=========================================================================
        #region Method - 渐变顶点计算
        /// <summary>
        /// 重写网格修改方法，修改顶点颜色实现渐变效果
        /// </summary>
        /// <param name="vh">顶点辅助器，用于获取和修改UI网格顶点数据</param>
        public override void ModifyMesh(VertexHelper vh)
        {
            // 组件未激活时不执行效果
            if (!IsActive())
                return;

            // 获取当前网格总顶点数量
            int count = vh.currentVertCount;
            if (count == 0)
                return;

            // 存储所有顶点数据
            List<UIVertex> vertexs = new List<UIVertex>();
            for (int i = 0; i < count; i++)
            {
                UIVertex vertex = new UIVertex();
                // 从顶点辅助器中读取对应索引的顶点数据
                vh.PopulateUIVertex(ref vertex, i);
                vertexs.Add(vertex);
            }

            // 遍历顶点，计算文本的顶部和底部Y坐标边界
            float topY = vertexs[0].position.y;
            float bottomY = vertexs[0].position.y;

            for (int i = 1; i < count; i++)
            {
                float y = vertexs[i].position.y;
                if (y > topY)
                {
                    topY = y;
                }
                else if (y < bottomY)
                {
                    bottomY = y;
                }
            }

            // 计算UI元素总高度
            float height = topY - bottomY;

            // 根据顶点Y轴坐标插值计算渐变颜色，并重新设置顶点
            for (int i = 0; i < count; i++)
            {
                UIVertex vertex = vertexs[i];
                // 根据高度比例在底部颜色和顶部颜色之间插值
                Color32 color = Color32.Lerp(bottomColor, topColor, (vertex.position.y - bottomY) / height);
                vertex.color = color;

                // 将修改后的顶点数据写回网格
                vh.SetUIVertex(vertex, i);
            }
        }
        #endregion
    }
}