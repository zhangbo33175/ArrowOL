/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  AorTextEffectOutline.cs
 * author:    云毅
 * created:   2026
 * descrip:   高品质文本描边特效 | 支持字间距 + 多行对齐 + Shader 描边
 ***************************************************************/

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Honor.Runtime
{
    /// <summary>
    /// 自定义文本描边效果（基于Shader + 顶点偏移实现高品质外描边）
    /// 同时支持：文本描边、字间距调整、多行文本对齐适配
    /// 依赖自定义Shader：Honor/UI/UIOutlineShader
    /// </summary>
    [AddComponentMenu("UI/Honor/自定义文本描边效果")]
    public class AorTextEffectOutline : BaseMeshEffect
    {
        //=========================================================================
        // 序列化字段 & 公共配置
        //=========================================================================
        #region Field - 描边配置
        /// <summary>
        /// 描边颜色
        /// </summary>
        public Color OutlineColor = Color.white;

        /// <summary>
        /// 描边宽度（0~8）
        /// </summary>
        [Range(0, 8)] public int OutlineWidth = 0;

        /// <summary>
        /// 文本字符间距（0~50）
        /// </summary>
        [Range(0, 50)] public float Spacing = 0f;
        #endregion

        //=========================================================================
        // 私有成员 & 缓存
        //=========================================================================
        #region Field - 缓存 & 静态
        /// <summary>
        /// 静态顶点缓存列表（减少GC）
        /// </summary>
        private static List<UIVertex> m_VetexList = new List<UIVertex>();

        /// <summary>
        /// 所属Canvas（用于开启Shader通道）
        /// </summary>
        private Canvas m_canvas = null;
        #endregion

        //=========================================================================
        // 嵌套结构 & 枚举
        //=========================================================================
        #region Struct & Enum
        /// <summary>
        /// 文本水平对齐类型
        /// </summary>
        public enum HorizontalAligmentType
        {
            Left,
            Center,
            Right
        }

        /// <summary>
        /// 文本行数据结构（记录一行文字的顶点起始/结束/数量）
        /// </summary>
        public class Line
        {
            /// <summary>
            /// 起点顶点索引
            /// </summary>
            public int StartVertexIndex => _startVertexIndex;
            private int _startVertexIndex = 0;

            /// <summary>
            /// 终点顶点索引
            /// </summary>
            public int EndVertexIndex => _endVertexIndex;
            private int _endVertexIndex = 0;

            /// <summary>
            /// 该行总顶点数量
            /// </summary>
            public int VertexCount => _vertexCount;
            private int _vertexCount = 0;

            /// <summary>
            /// 构造一行文本数据
            /// </summary>
            /// <param name="startVertexIndex">起始顶点索引</param>
            /// <param name="length">字符数量</param>
            public Line(int startVertexIndex, int length)
            {
                _startVertexIndex = startVertexIndex;
                _endVertexIndex = length * 6 - 1 + startVertexIndex;
                _vertexCount = length * 6;
            }
        }
        #endregion

        //=========================================================================
        // 生命周期 & 初始化
        //=========================================================================
        #region MonoBehaviour - 初始化
        /// <summary>
        /// 初始化：获取Canvas、创建材质、设置Shader通道、更新描边参数
        /// </summary>
        protected override void Start()
        {
            m_canvas = graphic.canvas;
            AddMaterial();

            if (CheckShader())
            {
                SetShaderChannels();
                SetParams();
                _Refresh();
            }
        }

        /// <summary>
        /// 销毁时清空材质引用，防止内存泄漏
        /// </summary>
        private void OnDestroy()
        {
            if (graphic)
                graphic.material = null;
        }

#if UNITY_EDITOR
        /// <summary>
        /// 编辑器模式：参数修改时自动更新效果
        /// </summary>
        protected override void OnValidate()
        {
            base.OnValidate();
            if (CheckShader())
            {
                SetParams();
                _Refresh();
            }
        }
#endif
        #endregion

        //=========================================================================
        // 材质 & Shader 管理
        //=========================================================================
        #region Method - 材质与Shader
        /// <summary>
        /// 检查Graphic与Material是否有效
        /// </summary>
        /// <returns>检查结果</returns>
        private bool CheckShader()
        {
            if (graphic == null)
            {
                Debug.LogError("No Graphic Component !");
                return false;
            }

            if (graphic.material == null)
            {
                Debug.LogError("No Material !");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 创建并绑定自定义描边Shader材质
        /// </summary>
        private void AddMaterial()
        {
            var shader1 = Shader.Find("Honor/UI/UIOutlineShader");
            graphic.material = new Material(shader1);
        }

        /// <summary>
        /// 向Shader设置描边颜色与宽度
        /// </summary>
        private void SetParams()
        {
            if (graphic.material != null)
            {
                graphic.material.SetColor("_OutlineColor", OutlineColor);
                graphic.material.SetFloat("_OutlineWidth", OutlineWidth);
            }
        }

        /// <summary>
        /// 开启Canvas所需的额外Shader通道（TexCoord1、TexCoord2）
        /// </summary>
        private void SetShaderChannels()
        {
            if (m_canvas)
            {
                var v1 = m_canvas.additionalShaderChannels;
                var v2 = AdditionalCanvasShaderChannels.TexCoord1;
                if ((v1 & v2) != v2)
                {
                    m_canvas.additionalShaderChannels |= v2;
                }

                v2 = AdditionalCanvasShaderChannels.TexCoord2;
                if ((v1 & v2) != v2)
                {
                    m_canvas.additionalShaderChannels |= v2;
                }
            }
        }
        #endregion

        //=========================================================================
        // 刷新 & 网格更新
        //=========================================================================
        #region Method - 刷新与网格
        /// <summary>
        /// 刷新文本网格（标记顶点为脏）
        /// </summary>
        private void _Refresh()
        {
            graphic.SetVerticesDirty();
        }

        /// <summary>
        /// 重写UGUI网格修改方法
        /// 先调整字间距，再处理描边顶点偏移
        /// </summary>
        /// <param name="vh">顶点辅助器</param>
        public override void ModifyMesh(VertexHelper vh)
        {
            ModifyMeshText(vh);
            vh.GetUIVertexStream(m_VetexList);

            this._ProcessVertices();

            vh.Clear();
            vh.AddUIVertexTriangleStream(m_VetexList);
        }
        #endregion

        //=========================================================================
        // 描边顶点计算
        //=========================================================================
        #region Method - 描边顶点处理
        /// <summary>
        /// 处理所有三角形顶点：计算中心点、方向、UV，执行描边偏移
        /// </summary>
        private void _ProcessVertices()
        {
            for (int i = 0, count = m_VetexList.Count - 3; i <= count; i += 3)
            {
                var v1 = m_VetexList[i];
                var v2 = m_VetexList[i + 1];
                var v3 = m_VetexList[i + 2];

                // 计算三角形中心点
                var minX = _Min(v1.position.x, v2.position.x, v3.position.x);
                var minY = _Min(v1.position.y, v2.position.y, v3.position.y);
                var maxX = _Max(v1.position.x, v2.position.x, v3.position.x);
                var maxY = _Max(v1.position.y, v2.position.y, v3.position.y);
                var posCenter = new Vector2(minX + maxX, minY + maxY) * 0.5f;

                // 计算三角形本地方向与UV方向
                Vector2 triX, triY, uvX, uvY;
                Vector2 pos1 = v1.position;
                Vector2 pos2 = v2.position;
                Vector2 pos3 = v3.position;

                if (Mathf.Abs(Vector2.Dot((pos2 - pos1).normalized, Vector2.right))
                    > Mathf.Abs(Vector2.Dot((pos3 - pos2).normalized, Vector2.right)))
                {
                    triX = pos2 - pos1;
                    triY = pos3 - pos2;
                    uvX = v2.uv0 - v1.uv0;
                    uvY = v3.uv0 - v2.uv0;
                }
                else
                {
                    triX = pos3 - pos2;
                    triY = pos2 - pos1;
                    uvX = v3.uv0 - v2.uv0;
                    uvY = v2.uv0 - v1.uv0;
                }

                // 计算原始UV边界
                var uvMin = _Min(v1.uv0, v2.uv0, v3.uv0);
                var uvMax = _Max(v1.uv0, v2.uv0, v3.uv0);

                // 为每个顶点应用新的位置与UV
                v1 = _SetNewPosAndUV(v1, OutlineWidth, posCenter, triX, triY, uvX, uvY, uvMin, uvMax);
                v2 = _SetNewPosAndUV(v2, OutlineWidth, posCenter, triX, triY, uvX, uvY, uvMin, uvMax);
                v3 = _SetNewPosAndUV(v3, OutlineWidth, posCenter, triX, triY, uvX, uvY, uvMin, uvMax);

                // 回写顶点
                m_VetexList[i] = v1;
                m_VetexList[i + 1] = v2;
                m_VetexList[i + 2] = v3;
            }
        }

        /// <summary>
        /// 设置顶点的新位置与UV（实现描边偏移）
        /// </summary>
        private static UIVertex _SetNewPosAndUV(UIVertex pVertex, int pOutLineWidth,
            Vector2 pPosCenter,
            Vector2 pTriangleX, Vector2 pTriangleY,
            Vector2 pUVX, Vector2 pUVY,
            Vector2 pUVOriginMin, Vector2 pUVOriginMax)
        {
            // 位置偏移
            var pos = pVertex.position;
            var posXOffset = pos.x > pPosCenter.x ? pOutLineWidth : -pOutLineWidth;
            var posYOffset = pos.y > pPosCenter.y ? pOutLineWidth : -pOutLineWidth;
            pos.x += posXOffset;
            pos.y += posYOffset;
            pVertex.position = pos;

            // UV偏移
            Vector4 uv = pVertex.uv0;
            Vector2 tmp1 = pUVX / pTriangleX.magnitude * posXOffset *
                           (Vector2.Dot(pTriangleX, Vector2.right) > 0 ? 1 : -1);
            uv.x += tmp1.x;
            uv.y += tmp1.y;

            Vector2 tmp2 = pUVY / pTriangleY.magnitude * posYOffset *
                           (Vector2.Dot(pTriangleY, Vector2.up) > 0 ? 1 : -1);
            uv.x += tmp2.x;
            uv.y += tmp2.y;

            pVertex.uv0 = uv;
            pVertex.uv1 = pUVOriginMin;
            pVertex.uv2 = pUVOriginMax;

            return pVertex;
        }
        #endregion

        //=========================================================================
        // 数学工具方法
        //=========================================================================
        #region Method - 数学工具
        /// <summary>
        /// 取三个float中的最小值
        /// </summary>
        private static float _Min(float pA, float pB, float pC)
        {
            return Mathf.Min(Mathf.Min(pA, pB), pC);
        }

        /// <summary>
        /// 取三个float中的最大值
        /// </summary>
        private static float _Max(float pA, float pB, float pC)
        {
            return Mathf.Max(Mathf.Max(pA, pB), pC);
        }

        /// <summary>
        /// 取三个Vector2的最小值
        /// </summary>
        private static Vector2 _Min(Vector2 pA, Vector2 pB, Vector2 pC)
        {
            return new Vector2(_Min(pA.x, pB.x, pC.x), _Min(pA.y, pB.y, pC.y));
        }

        /// <summary>
        /// 取三个Vector2的最大值
        /// </summary>
        private static Vector2 _Max(Vector2 pA, Vector2 pB, Vector2 pC)
        {
            return new Vector2(_Max(pA.x, pB.x, pC.x), _Max(pA.y, pB.y, pC.y));
        }
        #endregion

        //=========================================================================
        // 字间距 & 文本对齐
        //=========================================================================
        #region Method - 字间距调整
        /// <summary>
        /// 修改文本网格：根据对齐方式调整字符间距（支持左/中/右对齐）
        /// </summary>
        public void ModifyMeshText(VertexHelper vh)
        {
            if (!IsActive() || vh.currentVertCount == 0)
            {
                return;
            }

            var text = GetComponent<Text>();
            if (text == null)
            {
                Debug.LogError("Missing Text component");
                return;
            }

            // 判断水平对齐方式
            HorizontalAligmentType alignment;
            if (text.alignment == TextAnchor.LowerLeft || text.alignment == TextAnchor.MiddleLeft ||
                text.alignment == TextAnchor.UpperLeft)
            {
                alignment = HorizontalAligmentType.Left;
            }
            else if (text.alignment == TextAnchor.LowerCenter || text.alignment == TextAnchor.MiddleCenter ||
                     text.alignment == TextAnchor.UpperCenter)
            {
                alignment = HorizontalAligmentType.Center;
            }
            else
            {
                alignment = HorizontalAligmentType.Right;
            }

            var vertexs = new List<UIVertex>();
            vh.GetUIVertexStream(vertexs);

            // 按换行符分割行
            var lineTexts = text.text.Split('\n');
            var lines = new Line[lineTexts.Length];

            // 构建每一行的顶点索引范围
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
            for (var i = 0; i < lines.Length; i++)
            {
                for (var j = lines[i].StartVertexIndex; j <= lines[i].EndVertexIndex; j++)
                {
                    if (j < 0 || j >= vertexs.Count) continue;

                    vt = vertexs[j];
                    var charCount = lines[i].EndVertexIndex - lines[i].StartVertexIndex;
                    if (i == lines.Length - 1) charCount += 6;

                    // 根据对齐方式应用间距偏移
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

                    // 回写顶点
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