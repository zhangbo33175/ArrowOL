/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  AorTextEffectSpacing.cs
 * author:    云毅
 * created:   2026   2025
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
            public int StartVertexIndex => _startVertexIndex;
            private int _startVertexIndex = 0;

            /// <summary>
            /// 该行文本结束顶点索引
            /// </summary>
            public int EndVertexIndex => _endVertexIndex;
            private int _endVertexIndex = 0;

            /// <summary>
            /// 该行文本总顶点数量
            /// </summary>
            public int VertexCount => _vertexCount;
            private int _vertexCount = 0;

            /// <summary>
            /// 构造函数：初始化一行文本的顶点信息
            /// </summary>
            /// <param name="startVertexIndex">起始顶点索引</param>
            /// <param name="length">当前行字符数量</param>
            public Line(int startVertexIndex, int length)
            {
                _startVertexIndex = startVertexIndex;
                _endVertexIndex = length * 6 - 1 + startVertexIndex;
                _vertexCount = length * 6;
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
            HorizontalAligmentType alignment;
            if (text.alignment is TextAnchor.LowerLeft or TextAnchor.MiddleLeft or TextAnchor.UpperLeft)
            {
                alignment = HorizontalAligmentType.Left;
            }
            else if (text.alignment is TextAnchor.LowerCenter or TextAnchor.MiddleCenter or TextAnchor.UpperCenter)
            {
                alignment = HorizontalAligmentType.Center;
            }
            else
            {
                alignment = HorizontalAligmentType.Right;
            }

            // 获取所有顶点数据
            var vertexs = new List<UIVertex>();
            vh.GetUIVertexStream(vertexs);

            // 根据换行符，将文本分割为多行
            var lineTexts = text.text.Split('\n');
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
                    if (alignment == HorizontalAligmentType.Left)
                    {
                        vt.position += new Vector3(Spacing * ((j - lines[i].StartVertexIndex) / 6), 0, 0);
                    }
                    else if (alignment == HorizontalAligmentType.Right)
                    {
                        vt.position += new Vector3(Spacing * (-(charCount - j + lines[i].StartVertexIndex) / 6 + 1), 0, 0);
                    }
                    else if (alignment == HorizontalAligmentType.Center)
                    {
                        var offset = (charCount / 6) % 2 == 0 ? 0.5f : 0f;
                        vt.position += new Vector3(Spacing * ((j - lines[i].StartVertexIndex) / 6 - charCount / 12 + offset), 0, 0);
                    }

                    vertexs[j] = vt;

                    // 将修改后的顶点回写到网格（处理Text顶点索引规则）
                    if (j % 6 <= 2)
                        vh.SetUIVertex(vt, (j / 6) * 4 + j % 6);

                    if (j % 6 == 4)
                        vh.SetUIVertex(vt, (j / 6) * 4 + j % 6 - 1);
                }
            }
        }
        #endregion
    }
}