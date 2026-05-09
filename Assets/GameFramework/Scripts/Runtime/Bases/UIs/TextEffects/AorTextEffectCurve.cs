using UnityEngine;
using UnityEngine.UI;

namespace Honor.Runtime
{
    /// <summary>
    /// 文本曲线弯曲特效
    /// 作用：将UGUI文本/图像按照圆形弧度进行弯曲排列，实现弧形文字效果
    /// 基于UGUI BaseMeshEffect 网格修改基类实现
    /// </summary>
    [AddComponentMenu("UI/Honor/文本曲线弯曲特效")]
    public class AorTextEffectCurve : BaseMeshEffect
    {
        /// <summary>
        /// 弯曲圆弧半径
        /// 值越大，弯曲弧度越小；值越小，弯曲越明显
        /// </summary>
        [Header("圆弧弯曲半径")]
        public int radius = 100;

        /// <summary>
        /// 字符间距系数
        /// 用于调整弯曲后文本的字符间距，1为默认间距
        /// </summary>
        [Header("字符间距系数")]
        public float spaceCoff = 1f;

        /// <summary>
        /// 重写网格修改方法，对UI顶点进行弧形变换
        /// </summary>
        /// <param name="vh">UI顶点辅助器，用于获取/修改顶点数据</param>
        public override void ModifyMesh(VertexHelper vh)
        {
            // 组件未激活 或 弯曲半径为0，不执行任何效果
            if (!IsActive() || radius == 0)
            {
                return;
            }

            // 定义单个字符的四个顶点：左下、左上、右上、右下
            UIVertex lb = new UIVertex();
            UIVertex lt = new UIVertex();
            UIVertex rt = new UIVertex();
            UIVertex rb = new UIVertex();

            // 文本每个字符占用4个顶点，循环处理所有字符
            int charCount = vh.currentVertCount / 4;
            for (int i = 0; i < charCount; i++)
            {
                // 获取当前字符的四个顶点数据
                vh.PopulateUIVertex(ref lb, i * 4);
                vh.PopulateUIVertex(ref lt, i * 4 + 1);
                vh.PopulateUIVertex(ref rt, i * 4 + 2);
                vh.PopulateUIVertex(ref rb, i * 4 + 3);

                // 计算当前字符的中心点
                Vector3 center = Vector3.Lerp(lb.position, rt.position, 0.5f);
                
                // 位移矩阵：将字符中心点移至坐标原点
                Matrix4x4 move = Matrix4x4.TRS(center * -1, Quaternion.identity, Vector3.one);
                
                // 计算当前字符在圆弧上对应的弧度
                float rad = Mathf.PI / 2 - center.x * spaceCoff / radius;

                // 计算圆弧上的目标位置
                Vector3 pos = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0) * radius;
                
                // 计算字符旋转角度（使字符朝向圆弧中心）
                Quaternion rotation = Quaternion.Euler(0, 0, rad * 180 / Mathf.PI - 90);
                
                // 旋转矩阵
                Matrix4x4 rotate = Matrix4x4.TRS(Vector3.zero, rotation, Vector3.one);
                
                // 放置矩阵：将字符放置到圆弧目标位置
                Matrix4x4 place = Matrix4x4.TRS(pos, Quaternion.identity, Vector3.one);
                
                // 组合最终变换矩阵：位移 → 旋转 → 放置
                Matrix4x4 transform = place * rotate * move;

                // 对四个顶点应用矩阵变换
                lb.position = transform.MultiplyPoint(lb.position);
                lt.position = transform.MultiplyPoint(lt.position);
                rt.position = transform.MultiplyPoint(rt.position);
                rb.position = transform.MultiplyPoint(rb.position);

                // Y轴偏移校正，统一文本基线位置
                float offsetY = -radius + center.y;
                lb.position.y += offsetY;
                lt.position.y += offsetY;
                rt.position.y += offsetY;
                rb.position.y += offsetY;

                // 将修改后的顶点数据设置回网格
                vh.SetUIVertex(lb, i * 4);
                vh.SetUIVertex(lt, i * 4 + 1);
                vh.SetUIVertex(rt, i * 4 + 2);
                vh.SetUIVertex(rb, i * 4 + 3);
            }
        }
    }
}