using UnityEngine;
using UnityEngine.UI;

namespace GameLib
{
    /// <summary>
    /// 扩展Image组件
    /// 功能：支持九宫格图片的水平/垂直进度填充（切片裁剪模式）
    /// 解决原生Image填充模式下九宫格图片拉伸变形的问题
    /// </summary>
    [AddComponentMenu("UI/AorExtendImage")]
    public class AorExtendImage : Image
    {
        /// <summary>
        /// 是否启用九宫格切片填充模式
        /// 启用后：水平/垂直填充时保持九宫格边框不拉伸
        /// </summary>
        [SerializeField] private bool m_SlicedClipMode = true;

        /// <summary>
        /// 重写网格生成方法
        /// 根据图片类型和填充方式选择对应的网格生成逻辑
        /// </summary>
        /// <param name="vh">顶点辅助类，用于生成UI网格</param>
        protected override void OnPopulateMesh(VertexHelper vh)
        {
            switch (type)
            {
                // 满足条件：填充类型 + 切片模式开启 + 水平/垂直填充 + 拥有九宫格边框
                case Type.Filled when m_SlicedClipMode &&
                                      (fillMethod == FillMethod.Horizontal || fillMethod == FillMethod.Vertical) &&
                                      hasBorder:
                    GenerateSlicedSprite(vh);
                    break;
                // 其他情况使用原生Image的网格生成逻辑
                default:
                    base.OnPopulateMesh(vh);
                    break;
            }
        }

        /// <summary>
        /// 临时存储顶点坐标的数组
        /// </summary>
        private Vector2[] s_VertScratch = new Vector2[4];

        /// <summary>
        /// 临时存储UV坐标的数组
        /// </summary>
        private Vector2[] s_UVScratch = new Vector2[4];

        /// <summary>
        /// 生成九宫格切片填充的网格数据
        /// 核心逻辑：根据填充进度计算顶点与UV，实现无损九宫格填充效果
        /// </summary>
        /// <param name="toFill">输出的顶点辅助对象</param>
        private void GenerateSlicedSprite(VertexHelper toFill)
        {
            // 获取当前生效的精灵（优先使用覆盖精灵）
            var activeSprite = overrideSprite ?? sprite;

            // 声明UV、内边距、边框参数
            Vector4 outer, inner, padding, border;

            // 从精灵中获取原始的UV、内边距、边框数据
            if (activeSprite != null)
            {
                outer = UnityEngine.Sprites.DataUtility.GetOuterUV(activeSprite);
                inner = UnityEngine.Sprites.DataUtility.GetInnerUV(activeSprite);
                padding = UnityEngine.Sprites.DataUtility.GetPadding(activeSprite);
                border = activeSprite.border;
            }
            // 无精灵时赋默认值
            else
            {
                outer = Vector4.zero;
                inner = Vector4.zero;
                padding = Vector4.zero;
                border = Vector4.zero;
            }

            // 获取像素适配后的矩形区域
            Rect rect = GetPixelAdjustedRect();
            // 计算适配后的九宫格边框
            Vector4 adjustedBorders = GetAdjustedBorders(border / pixelsPerUnit, rect);
            // 单位转换：像素转单位
            padding = padding / pixelsPerUnit;

            // 初始化顶点基础坐标（左/下、右/上）
            s_VertScratch[0] = new Vector2(padding.x, padding.y);
            s_VertScratch[3] = new Vector2(rect.width - padding.z, rect.height - padding.w);

            // 左/下边框顶点
            s_VertScratch[1].x = adjustedBorders.x;
            s_VertScratch[1].y = adjustedBorders.y;

            // 右/上边框顶点
            s_VertScratch[2].x = rect.width - adjustedBorders.z;
            s_VertScratch[2].y = rect.height - adjustedBorders.w;

            // 为所有顶点加上矩形偏移量
            for (int i = 0; i < 4; ++i)
            {
                s_VertScratch[i].x += rect.x;
                s_VertScratch[i].y += rect.y;
            }

            // 初始化UV坐标
            s_UVScratch[0] = new Vector2(outer.x, outer.y);
            s_UVScratch[1] = new Vector2(inner.x, inner.y);
            s_UVScratch[2] = new Vector2(inner.z, inner.w);
            s_UVScratch[3] = new Vector2(outer.z, outer.w);

            // 计算总长度与各段比例，用于填充进度计算
            float xLength = s_VertScratch[3].x - s_VertScratch[0].x;
            float yLength = s_VertScratch[3].y - s_VertScratch[0].y;
            float len1XRatio = (s_VertScratch[1].x - s_VertScratch[0].x) / xLength;
            float len1YRatio = (s_VertScratch[1].y - s_VertScratch[0].y) / yLength;
            float len2XRatio = (s_VertScratch[2].x - s_VertScratch[1].x) / xLength;
            float len2YRatio = (s_VertScratch[2].y - s_VertScratch[1].y) / yLength;
            float len3XRatio = (s_VertScratch[3].x - s_VertScratch[2].x) / xLength;
            float len3YRatio = (s_VertScratch[3].y - s_VertScratch[2].y) / yLength;

            // 右侧边框长度缓存
            var l3 = s_VertScratch[3].x - s_VertScratch[2].x;

            // 网格分段数量
            int xLen = 3, yLen = 3;

            // 水平填充逻辑
            if (fillMethod == FillMethod.Horizontal)
            {
                if (fillAmount >= 1)
                {
                    float ratio = 1 - (1 - (len1XRatio + len2XRatio)) / len3XRatio;
                    s_VertScratch[3].x = s_VertScratch[3].x - (s_VertScratch[3].x - s_VertScratch[2].x) * ratio;
                    s_UVScratch[3].x = s_UVScratch[3].x - (s_UVScratch[3].x - s_UVScratch[2].x) * ratio;
                }
                else if (fillAmount >= len1XRatio)
                {
                    xLen = 2;
                    float ratio = 1 - (fillAmount - len1XRatio) / len2XRatio;
                    s_VertScratch[2].x = s_VertScratch[2].x - (s_VertScratch[2].x - s_VertScratch[1].x) * ratio;

                    var newMidWidth = s_VertScratch[2].x - l3;
                    if (newMidWidth >= s_VertScratch[1].x)
                    {
                        xLen = 3;
                        s_VertScratch[2].x = newMidWidth;

                        float ratio3 = 1 - (1 - (len1XRatio + len2XRatio)) / len3XRatio;
                        s_VertScratch[3].x = s_VertScratch[2].x + l3;
                        s_UVScratch[3].x = s_UVScratch[3].x - (s_UVScratch[3].x - s_UVScratch[2].x) * ratio3;
                    }
                }
                else
                {
                    xLen = 1;
                    float ratio = 1 - fillAmount / len1XRatio;
                    s_VertScratch[1].x = s_VertScratch[1].x - (s_VertScratch[1].x - s_VertScratch[0].x) * ratio;
                    s_UVScratch[1].x = s_UVScratch[1].x - (s_UVScratch[1].x - s_UVScratch[0].x) * ratio;
                }
            }
            // 垂直填充逻辑
            else if (fillMethod == FillMethod.Vertical)
            {
                if (fillAmount >= (len1YRatio + len2YRatio))
                {
                    float ratio = 1 - (fillAmount - (len1YRatio + len2YRatio)) / len3YRatio;
                    s_VertScratch[3].y = s_VertScratch[3].y - (s_VertScratch[3].y - s_VertScratch[2].y) * ratio;
                    s_UVScratch[3].y = s_UVScratch[3].y - (s_UVScratch[3].y - s_UVScratch[2].y) * ratio;
                }
                else if (fillAmount >= len1YRatio)
                {
                    yLen = 2;
                    float ratio = 1 - (fillAmount - len1YRatio) / len2YRatio;
                    s_VertScratch[2].y = s_VertScratch[2].y - (s_VertScratch[2].y - s_VertScratch[1].y) * ratio;
                    s_UVScratch[2].y -= (s_UVScratch[2].y - s_UVScratch[1].y) * ratio;
                }
                else
                {
                    yLen = 1;
                    float ratio = 1 - fillAmount / len1YRatio;
                    s_VertScratch[1].y = s_VertScratch[1].y - (s_VertScratch[1].y - s_VertScratch[0].y) * ratio;
                    s_UVScratch[1].y = s_UVScratch[1].y - (s_UVScratch[1].y - s_UVScratch[0].y) * ratio;
                }
            }

            // 清空原有顶点数据
            toFill.Clear();

            // 循环生成九宫格网格
            for (int x = 0; x < xLen; ++x)
            {
                int x2 = x + 1;

                for (int y = 0; y < yLen; ++y)
                {
                    // 不填充中心区域时跳过中间网格
                    if (!fillCenter && x == 1 && y == 1)
                        continue;

                    int y2 = y + 1;

                    // 添加单个网格（四边形）
                    AddQuad(toFill,
                        new Vector2(s_VertScratch[x].x, s_VertScratch[y].y),
                        new Vector2(s_VertScratch[x2].x, s_VertScratch[y2].y),
                        color,
                        new Vector2(s_UVScratch[x].x, s_UVScratch[y].y),
                        new Vector2(s_UVScratch[x2].x, s_UVScratch[y2].y));
                }
            }
        }

        /// <summary>
        /// 向顶点辅助类中添加一个四边形（UI基础绘制单元）
        /// </summary>
        /// <param name="vertexHelper">顶点操作对象</param>
        /// <param name="posMin">四边形左下角坐标</param>
        /// <param name="posMax">四边形右上角坐标</param>
        /// <param name="color">顶点颜色</param>
        /// <param name="uvMin">UV左下角坐标</param>
        /// <param name="uvMax">UV右上角坐标</param>
        static void AddQuad(VertexHelper vertexHelper, Vector2 posMin, Vector2 posMax, Color32 color, Vector2 uvMin,
            Vector2 uvMax)
        {
            int startIndex = vertexHelper.currentVertCount;

            // 添加四个顶点
            vertexHelper.AddVert(new Vector3(posMin.x, posMin.y, 0), color, new Vector2(uvMin.x, uvMin.y));
            vertexHelper.AddVert(new Vector3(posMin.x, posMax.y, 0), color, new Vector2(uvMin.x, uvMax.y));
            vertexHelper.AddVert(new Vector3(posMax.x, posMax.y, 0), color, new Vector2(uvMax.x, uvMax.y));
            vertexHelper.AddVert(new Vector3(posMax.x, posMin.y, 0), color, new Vector2(uvMax.x, uvMin.y));

            // 构建两个三角形组成四边形
            vertexHelper.AddTriangle(startIndex, startIndex + 1, startIndex + 2);
            vertexHelper.AddTriangle(startIndex + 2, startIndex + 3, startIndex);
        }

        /// <summary>
        /// 计算适配当前矩形的九宫格边框
        /// 防止边框超出矩形范围，保证边框比例正确
        /// </summary>
        /// <param name="border">原始边框</param>
        /// <param name="adjustedRect">目标适配矩形</param>
        /// <returns>调整后的边框</returns>
        private Vector4 GetAdjustedBorders(Vector4 border, Rect adjustedRect)
        {
            Rect originalRect = rectTransform.rect;

            // 分别处理X、Y轴方向
            for (int axis = 0; axis <= 1; axis++)
            {
                float borderScaleRatio;

                // 根据矩形缩放比例调整边框
                if (originalRect.size[axis] != 0)
                {
                    borderScaleRatio = adjustedRect.size[axis] / originalRect.size[axis];
                    border[axis] *= borderScaleRatio;
                    border[axis + 2] *= borderScaleRatio;
                }

                // 防止边框总和超过矩形宽度
                float combinedBorders = border[axis] + border[axis + 2];
                if (adjustedRect.size[axis] < combinedBorders && combinedBorders != 0)
                {
                    borderScaleRatio = adjustedRect.size[axis] / combinedBorders;
                    border[axis] *= borderScaleRatio;
                    border[axis + 2] *= borderScaleRatio;
                }
            }

            return border;
        }
    }
}