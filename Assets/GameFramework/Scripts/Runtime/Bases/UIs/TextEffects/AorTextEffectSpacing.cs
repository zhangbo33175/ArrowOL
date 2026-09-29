/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  AorTextEffectSpacing.cs
 * author:    云毅
 * created:   2026
 * descrip:   文本字符间距调整组件 | 支持左/中/右对齐 | 基于UGUI网格修改
 ***************************************************************/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Honor.Runtime
{
    /// <summary>
    /// 自定义文本字符间距调整组件
    /// 基于UGUI BaseMeshEffect实现，支持左对齐、居中、右对齐三种模式下的字符间距调整
    /// 仅适用于UGUI Text组件
    /// </summary>
    [AddComponentMenu("UI/Honor/自定义文本字符间距调整组件")]
    public class AorTextEffectSpacing : BaseMeshEffect
    {
        //=========================================================================
        // 枚举 & 结构定义
        //=========================================================================
        #region Enum & Struct
        /// <summary>
        /// 文本水平对齐类型
        /// </summary>
        public enum HorizontalAligmentType
        {
            Left,       // 左对齐
            Center,     // 居中对齐
            Right       // 右对齐
        }

        /// <summary>
        /// 文本行数据结构
        /// 用于记录一行文本对应的顶点起始索引、结束索引、总顶点数量
        /// </summary>
        public class Line
        {
            /// <summary>
            /// 该行文本起始顶点索引
            /// </summary>
            public int StartVertexIndex => m_StartVertexIndex;
            private int m_StartVertexIndex = 0;

            /// <summary>
            /// 该行文本结束顶点索引
            /// </summary>
            public int EndVertexIndex => m_EndVertexIndex;
            private int m_EndVertexIndex = 0;

            /// <summary>
            /// 该行文本总顶点数量
            /// </summary>
            public int VertexCount => m_VertexCount;
            private int m_VertexCount = 0;

            /// <summary>
            /// 构造函数：初始化一行文本的顶点信息
            /// </summary>
            /// <param name="startVertexIndex">起始顶点索引</param>
            /// <param name="length">当前行字符数量</param>
            public Line(int startVertexIndex, int length)
            {
                m_StartVertexIndex = startVertexIndex;
                m_EndVertexIndex = length * 6 - 1 + startVertexIndex;
                m_VertexCount = length * 6;
            }
        }
        #endregion

        //=========================================================================
        // 公共字段
        //=========================================================================
        #region Field - 间距设置
        /// <summary>
        /// 字符间距值
        /// 正数增大间距，负数缩小间距
        /// </summary>
        public float Spacing = 1f;
        #endregion

        //=========================================================================
        // 重写方法 - 网格修改
        //=========================================================================
        #region Method - 网格顶点调整
        /// <summary>
        /// 重写网格修改方法，调整文本字符顶点位置实现间距效果
        /// </summary>
        /// <param name="vh">顶点辅助器，用于获取和修改UI网格顶点数据</param>
        public override void ModifyMesh(VertexHelper vh)
        {
            // 组件未激活或无顶点数据时，不执行逻辑
            if (!IsActive() || vh.currentVertCount == 0)
                return;

            // 获取挂载的Text组件
            var text = GetComponent<Text>();
            if (text == null)
            {
                Debug.LogError("Missing Text component");
                return;
            }

            // 根据Text的对齐方式，确定当前水平对齐类型
            HorizontalAligmentType alignment = ResolveAlignment(text.alignment);

            // 获取所有顶点数据
            var vertexs = new List<UIVertex>();
            vh.GetUIVertexStream(vertexs);

            // 根据换行符，将文本分割为多行并计算每行顶点范围
            var lineTexts = text.text.Split('\n');
            var lines = BuildLines(lineTexts);

            // 逐行逐顶点应用字符间距并回写网格
            ApplySpacingToVertices(vh, vertexs, lines, alignment);
        }

        /// <summary>
        /// 根据Text锚点对齐方式解析水平对齐类型
        /// </summary>
        /// <param name="anchor">Text的对齐锚点</param>
        /// <returns>水平对齐类型</returns>
        private HorizontalAligmentType ResolveAlignment(TextAnchor anchor)
        {
            if (anchor is TextAnchor.LowerLeft or TextAnchor.MiddleLeft or TextAnchor.UpperLeft)
            {
                return HorizontalAligmentType.Left;
            }
            else if (anchor is TextAnchor.LowerCenter or TextAnchor.MiddleCenter or TextAnchor.UpperCenter)
            {
                return HorizontalAligmentType.Center;
            }
            else
            {
                return HorizontalAligmentType.Right;
            }
        }

        /// <summary>
        /// 按换行文本构建每行对应的顶点范围（每个字符占6个顶点）
        /// </summary>
        /// <param name="lineTexts">按换行符切分的文本行</param>
        /// <returns>每行顶点范围数组</returns>
        private Line[] BuildLines(string[] lineTexts)
        {
            var lines = new Line[lineTexts.Length];

            // 计算每一行文本对应的顶点范围（每个字符占6个顶点）
            for (var i = 0; i < lines.Length; i++)
            {
                if (i == 0)
                {
                    lines[i] = new Line(0, lineTexts[i].Length + 1);
                }
                else if (i > 0 && i < lines.Length - 1)
                {
                    lines[i] = new Line(lines[i - 1].EndVertexIndex + 1, lineTexts[i].Length + 1);
                }
                else
                {
                    lines[i] = new Line(lines[i - 1].EndVertexIndex + 1, lineTexts[i].Length);
                }
            }

            return lines;
        }

        /// <summary>
        /// 逐行遍历顶点，按对齐方式计算水平偏移并回写到网格
        /// </summary>
        /// <param name="vh">顶点辅助器</param>
        /// <param name="vertexs">顶点列表</param>
        /// <param name="lines">每行顶点范围数组</param>
        /// <param name="alignment">水平对齐类型</param>
        private void ApplySpacingToVertices(VertexHelper vh, List<UIVertex> vertexs, Line[] lines, HorizontalAligmentType alignment)
        {
            UIVertex vt;
            // 遍历所有行，逐行调整字符间距
            for (var i = 0; i < lines.Length; i++)
            {
                // 遍历当前行所有顶点
                for (var j = lines[i].StartVertexIndex; j <= lines[i].EndVertexIndex; j++)
                {
                    if (j < 0 || j >= vertexs.Count)
                        continue;

                    vt = vertexs[j];
                    var charCount = lines[i].EndVertexIndex - lines[i].StartVertexIndex;

                    // 最后一行补充顶点数量
                    if (i == lines.Length - 1)
                        charCount += 6;

                    // 根据不同对齐方式，计算顶点偏移量
                    vt.position += new Vector3(CalculateOffsetX(alignment, lines[i], j, charCount), 0, 0);

                    vertexs[j] = vt;

                    // 将修改后的顶点回写到网格（处理Text顶点索引规则）
                    if (j % 6 <= 2)
                        vh.SetUIVertex(vt, (j / 6) * 4 + j % 6);

                    if (j % 6 == 4)
                        vh.SetUIVertex(vt, (j / 6) * 4 + j % 6 - 1);
                }
            }
        }

        /// <summary>
        /// 按对齐方式计算单个顶点的水平偏移量
        /// </summary>
        /// <param name="alignment">水平对齐类型</param>
        /// <param name="line">当前行顶点范围</param>
        /// <param name="j">当前顶点索引</param>
        /// <param name="charCount">当前行字符对应的顶点数</param>
        /// <returns>水平偏移量</returns>
        private float CalculateOffsetX(HorizontalAligmentType alignment, Line line, int j, int charCount)
        {
            if (alignment == HorizontalAligmentType.Left)
            {
                return Spacing * ((j - line.StartVertexIndex) / 6);
            }
            else if (alignment == HorizontalAligmentType.Right)
            {
                return Spacing * (-(charCount - j + line.StartVertexIndex) / 6 + 1);
            }
            else
            {
                var offset = (charCount / 6) % 2 == 0 ? 0.5f : 0f;
                return Spacing * ((j - line.StartVertexIndex) / 6 - charCount / 12 + offset);
            }
        }
        #endregion
    }
}