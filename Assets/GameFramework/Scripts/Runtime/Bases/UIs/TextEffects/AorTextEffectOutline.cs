using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Honor.Runtime
{
    [AddComponentMenu("UI/Honor/TextEffectOutline")]
    public class AorTextEffectOutline : BaseMeshEffect
    {
        public Color OutlineColor = Color.white;
        [Range(0, 8)]
        public int OutlineWidth = 0;
        [Range(0, 50)]
        public float Spacing = 0f;

        private static List<UIVertex> m_VetexList = new List<UIVertex>();
        
        private Canvas m_canvas = null;

        #region Struct

        public enum HorizontalAligmentType
        {
            Left,
            Center,
            Right
        }

        public class Line
        {
            // 起点索引
            public int StartVertexIndex { get { return _startVertexIndex; } }
            private int _startVertexIndex = 0;

            // 终点索引
            public int EndVertexIndex { get { return _endVertexIndex; } }
            private int _endVertexIndex = 0;

            // 该行占的点数目
            public int VertexCount { get { return _vertexCount; } }
            private int _vertexCount = 0;

            public Line(int startVertexIndex, int length)
            {
                _startVertexIndex = startVertexIndex;
                _endVertexIndex = length * 6 - 1 + startVertexIndex;
                _vertexCount = length * 6;
            }
        }

        #endregion
        

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


        bool CheckShader()
        {
            if (graphic == null)
            {
                Log.Error("No Graphic Component !");
                return false;
            }
            if (graphic.material == null)
            {
                Log.Error("No Material !");
                return false;
            }
            return true;
        }

        void SetParams()
        {
            if (graphic.material != null)
            {
               graphic.material.SetColor("_OutlineColor", OutlineColor);
               graphic.material.SetFloat("_OutlineWidth", OutlineWidth);

            }
        }

        void SetShaderChannels()
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
        
        private void _Refresh()
        {
           graphic.SetVerticesDirty();
        }
        
#if UNITY_EDITOR
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


        public override void ModifyMesh(VertexHelper vh)
        {
            ModifyMeshText(vh);
            vh.GetUIVertexStream(m_VetexList);

            this._ProcessVertices();

            vh.Clear();
            vh.AddUIVertexTriangleStream(m_VetexList);
        }


        private void _ProcessVertices()
        {
            for (int i = 0, count = m_VetexList.Count - 3; i <= count; i += 3)
            {
                var v1 = m_VetexList[i];
                var v2 = m_VetexList[i + 1];
                var v3 = m_VetexList[i + 2];
                // 计算原顶点坐标中心点
                //
                var minX = _Min(v1.position.x, v2.position.x, v3.position.x);
                var minY = _Min(v1.position.y, v2.position.y, v3.position.y);
                var maxX = _Max(v1.position.x, v2.position.x, v3.position.x);
                var maxY = _Max(v1.position.y, v2.position.y, v3.position.y);
                var posCenter = new Vector2(minX + maxX, minY + maxY) * 0.5f;
                // 计算原始顶点坐标和UV的方向
                //
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
                // 计算原始UV框
                var uvMin = _Min(v1.uv0, v2.uv0, v3.uv0);
                var uvMax = _Max(v1.uv0, v2.uv0, v3.uv0);
                //OutlineColor 和 OutlineWidth 也传入，避免出现不同的材质球
                //var col_rg = new Vector2(OutlineColor.r, OutlineColor.g);       //描边颜色 用uv3 和 tangent的 zw传递
                //var col_ba = new Vector4(0, 0, OutlineColor.b, OutlineColor.a);
                //var normal = new Vector3(0, 0, OutlineWidth);                   //描边的宽度 用normal的z传递

                // 为每个顶点设置新的Position和UV，并传入原始UV框
                v1 = _SetNewPosAndUV(v1, this.OutlineWidth, posCenter, triX, triY, uvX, uvY, uvMin, uvMax);
                v2 = _SetNewPosAndUV(v2, this.OutlineWidth, posCenter, triX, triY, uvX, uvY, uvMin, uvMax);
                v3 = _SetNewPosAndUV(v3, this.OutlineWidth, posCenter, triX, triY, uvX, uvY, uvMin, uvMax);

                // 应用设置后的UIVertex
                //
                m_VetexList[i] = v1;
                m_VetexList[i + 1] = v2;
                m_VetexList[i + 2] = v3;
            }
        }


        private static UIVertex _SetNewPosAndUV(UIVertex pVertex, int pOutLineWidth,
            Vector2 pPosCenter,
            Vector2 pTriangleX, Vector2 pTriangleY,
            Vector2 pUVX, Vector2 pUVY,
            Vector2 pUVOriginMin, Vector2 pUVOriginMax)
        {
            // Position
            var pos = pVertex.position;
            var posXOffset = pos.x > pPosCenter.x ? pOutLineWidth : -pOutLineWidth;
            var posYOffset = pos.y > pPosCenter.y ? pOutLineWidth : -pOutLineWidth;
            pos.x += posXOffset;
            pos.y += posYOffset;
            pVertex.position = pos;

            // UV
            Vector4 uv = pVertex.uv0;
            Vector2 tmp1 = pUVX / pTriangleX.magnitude * posXOffset * (Vector2.Dot(pTriangleX, Vector2.right) > 0 ? 1 : -1);
            uv.x += tmp1.x; uv.y += tmp1.y;

            Vector2 tmp2 = pUVY / pTriangleY.magnitude * posYOffset * (Vector2.Dot(pTriangleY, Vector2.up) > 0 ? 1 : -1);
            uv.x += tmp2.x; uv.y += tmp2.y;

            pVertex.uv0 = uv;

            pVertex.uv1 = pUVOriginMin;     //uv1 uv2 可用  tangent  normal 在缩放情况 会有问题
            pVertex.uv2 = pUVOriginMax;

            return pVertex;
        }


        private static float _Min(float pA, float pB, float pC)
        {
            return Mathf.Min(Mathf.Min(pA, pB), pC);
        }


        private static float _Max(float pA, float pB, float pC)
        {
            return Mathf.Max(Mathf.Max(pA, pB), pC);
        }


        private static Vector2 _Min(Vector2 pA, Vector2 pB, Vector2 pC)
        {
            return new Vector2(_Min(pA.x, pB.x, pC.x), _Min(pA.y, pB.y, pC.y));
        }


        private static Vector2 _Max(Vector2 pA, Vector2 pB, Vector2 pC)
        {
            return new Vector2(_Max(pA.x, pB.x, pC.x), _Max(pA.y, pB.y, pC.y));
        }

        private void OnDestroy()
        {
            if(graphic) graphic.material = null;
        }

        private void AddMaterial()
        {
            var shader1 = Shader.Find("Honor/UI/UIOutlineShader");
            graphic.material = new Material(shader1);

        }


        public void ModifyMeshText(VertexHelper vh)
        {
            if (!IsActive() || vh.currentVertCount == 0)
            {
                return;
            }

            var text = GetComponent<Text>();

            if (text == null)
            {
                Log.Error("Missing Text component");
                return;
            }

            // 水平对齐方式
            HorizontalAligmentType alignment;
            if (text.alignment == TextAnchor.LowerLeft || text.alignment == TextAnchor.MiddleLeft || text.alignment == TextAnchor.UpperLeft)
            {
                alignment = HorizontalAligmentType.Left;
            }
            else if (text.alignment == TextAnchor.LowerCenter || text.alignment == TextAnchor.MiddleCenter || text.alignment == TextAnchor.UpperCenter)
            {
                alignment = HorizontalAligmentType.Center;
            }
            else
            {
                alignment = HorizontalAligmentType.Right;
            }

            var vertexs = new List<UIVertex>();
            vh.GetUIVertexStream(vertexs);
            // var indexCount = vh.currentIndexCount;

            var lineTexts = text.text.Split('\n');

            var lines = new Line[lineTexts.Length];

            // 根据lines数组中各个元素的长度计算每一行中第一个点的索引，每个字、字母、空母均占6个点
            for (var i = 0; i < lines.Length; i++)
            {
                // 除最后一行外，vertexs对于前面几行都有回车符占了6个点
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
                    if (j < 0 || j >= vertexs.Count)
                    {
                        continue;
                    }

                    vt = vertexs[j];

                    var charCount = lines[i].EndVertexIndex - lines[i].StartVertexIndex;
                    if (i == lines.Length - 1)
                    {
                        charCount += 6;
                    }

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
                    // 以下注意点与索引的对应关系
                    if (j % 6 <= 2)
                    {
                        vh.SetUIVertex(vt, (j / 6) * 4 + j % 6);
                    }

                    if (j % 6 == 4)
                    {
                        vh.SetUIVertex(vt, (j / 6) * 4 + j % 6 - 1);
                    }
                }
            }
        }
    }
}