/***************************************************************
 * (c) copyright 2026 - 2030, GameLib
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  AorInterfaceAdaptation.cs
 * author:    云毅
 * created:   2026
 * descrip:   界面统一适配组件
 *            1. 背景铺满全屏（拉伸铺满 / 保持宽高比裁边铺满）
 *            2. 目标节点紧贴参考节点指定边（地图画面 ↔ Bg 严丝合缝，自动计算偏移）
 *            3. 提供屏幕/世界矩形工具与正交相机对齐辅助（地图相机适配）
 *            可绑定到任意界面节点（启动界面、寻物主界面等），按配置自动适配
 ***************************************************************/

using UnityEngine;
using UnityEngine.UI;

namespace GameLib
{
    /// <summary>
    /// 界面统一适配组件
    /// 统一处理界面适配问题：全屏背景铺满、节点紧贴参考边自动对齐、地图相机对齐。
    /// 绑定到目标节点后按配置自动执行，也可通过静态方法在 Lua/代码中按需调用。
    /// </summary>
    [DisallowMultipleComponent]
    public class AorInterfaceAdaptation : MonoBehaviour
    {
        #region 枚举定义
        //=========================================================================
        // 枚举定义
        //=========================================================================

        /// <summary>
        /// 适配模式
        /// </summary>
        public enum AdaptMode
        {
            /// <summary>不处理（仅提供静态方法）</summary>
            None = 0,

            /// <summary>铺满全屏背景</summary>
            FillScreen = 1,

            /// <summary>目标紧贴参考节点上方（目标在参考下方区域）</summary>
            AlignAboveReference = 2,

            /// <summary>目标紧贴参考节点下方（目标在参考上方区域）</summary>
            AlignBelowReference = 3,
        }

        /// <summary>
        /// 铺满方式
        /// </summary>
        public enum FillMode
        {
            /// <summary>拉伸铺满父物体（不保持宽高比，画面可能变形）</summary>
            Stretch = 0,

            /// <summary>保持宽高比，放大至铺满父物体（超出部分被裁掉，无变形）</summary>
            Cover = 1,
        }

        /// <summary>
        /// 对齐边（target 哪一条边与 reference 对齐）
        /// </summary>
        public enum AlignSide
        {
            /// <summary>target 下边紧贴 reference 上边</summary>
            BottomToTop = 0,

            /// <summary>target 上边紧贴 reference 下边</summary>
            TopToBottom = 1,

            /// <summary>target 左边紧贴 reference 右边</summary>
            LeftToRight = 2,

            /// <summary>target 右边紧贴 reference 左边</summary>
            RightToLeft = 3,
        }
        #endregion

        #region 序列化字段
        //=========================================================================
        // 序列化字段
        //=========================================================================

        [Header("目标节点")]
        [Tooltip("需要进行适配的节点（默认为本组件所在节点）")]
        public RectTransform target;

        [Header("参考节点")]
        [Tooltip("Align 模式下的参考节点（如 UIMainPlaying 的 Bg）")]
        public RectTransform reference;

        [Header("适配模式")]
        [Tooltip("选择适配行为：铺满全屏 / 紧贴参考边")]
        public AdaptMode mode = AdaptMode.None;

        [Header("铺满方式")]
        [Tooltip("FillScreen 模式下使用：Stretch 拉伸 / Cover 保持宽高比裁边")]
        public FillMode fillMode = FillMode.Stretch;

        [Header("对齐边")]
        [Tooltip("Align 模式下 target 与 reference 的对齐边")]
        public AlignSide alignSide = AlignSide.BottomToTop;

        [Header("边距")]
        [Tooltip("Align 模式下的预留边距（参考单位，正数向外扩）")]
        public float margin;

        [Header("自动适配")]
        [Tooltip("Start 时是否自动执行适配")]
        public bool adaptOnStart = true;
        #endregion

        #region 生命周期
        //=========================================================================
        // 生命周期
        //=========================================================================

        /// <summary>
        /// 初始化引用：默认以本组件节点为目标
        /// </summary>
        private void Awake()
        {
            if (target == null)
            {
                target = transform as RectTransform;
            }
        }

        /// <summary>
        /// 按配置执行适配（延迟到 Start，确保 Canvas 已完成布局）
        /// </summary>
        private void Start()
        {
            if (adaptOnStart)
            {
                Apply();
            }
        }
        #endregion

        #region 组件公开接口
        //=========================================================================
        // 组件公开接口
        //=========================================================================

        /// <summary>
        /// 按当前配置执行适配
        /// </summary>
        public void Apply()
        {
            if (target == null) return;

            switch (mode)
            {
                case AdaptMode.FillScreen:
                    FillScreen(target, fillMode);
                    break;

                case AdaptMode.AlignAboveReference:
                    AlignToReference(target, reference, AlignSide.BottomToTop, margin);
                    break;

                case AdaptMode.AlignBelowReference:
                    AlignToReference(target, reference, AlignSide.TopToBottom, margin);
                    break;
            }
        }
        #endregion

        #region 全屏背景铺满
        //=========================================================================
        // 全屏背景铺满
        //=========================================================================

        /// <summary>
        /// 将目标节点铺满父物体（全屏背景）
        /// <para>Stretch：拉伸填满，不保持宽高比；Cover：保持宽高比放大铺满，裁掉溢出部分</para>
        /// </summary>
        /// <param name="target">目标 RectTransform</param>
        /// <param name="fillMode">铺满方式</param>
        public static void FillScreen(RectTransform target, FillMode fillMode = FillMode.Stretch)
        {
            if (target == null) return;

            RectTransform parent = target.parent as RectTransform;
            if (parent == null)
            {
                Debug.LogError($"[{nameof(AorInterfaceAdaptation)}] 目标节点没有父节点，无法铺满全屏。", target);
                return;
            }

            // 归一：先拉伸铺满父物体，保证区域占满
            StretchToParent(target);

            if (fillMode == FillMode.Stretch)
            {
                // 拉伸铺满：已通过 StretchToParent 完成
                return;
            }

            // Cover：保持 Image 精灵原始宽高比，放大至铺满，裁掉溢出
            Image image = target.GetComponent<Image>();
            if (image == null || image.sprite == null)
            {
                // 无精灵时保持拉伸铺满
                return;
            }

            Vector2 parentSize = parent.rect.size;
            Vector2 spriteSize = image.sprite.rect.size;
            if (spriteSize.x <= 0 || spriteSize.y <= 0 || parentSize.x <= 0 || parentSize.y <= 0)
            {
                return;
            }

            // 取较大缩放，保证短边铺满父物体（长边溢出被裁掉），无变形
            float scale = Mathf.Max(parentSize.x / spriteSize.x, parentSize.y / spriteSize.y);
            target.anchorMin = target.anchorMax = new Vector2(0.5f, 0.5f);
            target.pivot = new Vector2(0.5f, 0.5f);
            target.sizeDelta = spriteSize;
            target.anchoredPosition = Vector2.zero;
            target.localScale = Vector3.one * scale;
        }

        /// <summary>
        /// 拉伸目标节点铺满父物体（anchor 全拉伸，offset 归零）
        /// </summary>
        /// <param name="target">目标 RectTransform</param>
        public static void StretchToParent(RectTransform target)
        {
            if (target == null) return;

            target.anchorMin = Vector2.zero;
            target.anchorMax = Vector2.one;
            target.offsetMin = Vector2.zero;
            target.offsetMax = Vector2.zero;
            target.localScale = Vector3.one;
            target.rotation = Quaternion.identity;
        }
        #endregion

        #region 节点紧贴参考边对齐
        //=========================================================================
        // 节点紧贴参考边对齐（地图画面 ↔ Bg 严丝合缝，自动计算偏移）
        //=========================================================================

        /// <summary>
        /// 让目标节点某一边紧贴参考节点对应边，自动计算偏移（严丝合缝）
        /// <para>基于世界坐标计算位移，再转换到目标父空间应用，支持不同缩放/锚点的父子结构</para>
        /// </summary>
        /// <param name="target">目标节点</param>
        /// <param name="reference">参考节点（如 Bg）</param>
        /// <param name="alignSide">对齐方式</param>
        /// <param name="margin">边距（正数向外扩）</param>
        public static void AlignToReference(RectTransform target, RectTransform reference,
            AlignSide alignSide, float margin)
        {
            if (target == null || reference == null)
            {
                Debug.LogWarning($"[{nameof(AorInterfaceAdaptation)}] AlignToReference 目标或参考节点为空，跳过。");
                return;
            }

            // 缓存角点数组，避免多次分配
            Vector3[] refCorners = new Vector3[4];
            Vector3[] tgtCorners = new Vector3[4];
            reference.GetWorldCorners(refCorners); // 0=左下 1=左上 2=右上 3=右下
            target.GetWorldCorners(tgtCorners);

            Vector3 worldDelta = Vector3.zero;
            switch (alignSide)
            {
                case AlignSide.BottomToTop:
                    // target 下边（tgtCorners[0]）紧贴 reference 上边（refCorners[1]）
                    worldDelta.y = refCorners[1].y - tgtCorners[0].y + margin;
                    break;

                case AlignSide.TopToBottom:
                    // target 上边（tgtCorners[1]）紧贴 reference 下边（refCorners[0]）
                    worldDelta.y = refCorners[0].y - tgtCorners[1].y - margin;
                    break;

                case AlignSide.LeftToRight:
                    // target 左边（tgtCorners[0]）紧贴 reference 右边（refCorners[2]）
                    worldDelta.x = refCorners[2].x - tgtCorners[0].x + margin;
                    break;

                case AlignSide.RightToLeft:
                    // target 右边（tgtCorners[2]）紧贴 reference 左边（refCorners[0]）
                    worldDelta.x = refCorners[0].x - tgtCorners[2].x - margin;
                    break;
            }

            RectTransform parent = target.parent as RectTransform;
            if (parent == null)
            {
                target.position += worldDelta;
                return;
            }

            // 世界位移 → 父局部空间位移（父无旋转/缩放时等价于直接减）
            Vector3 localDelta = parent.InverseTransformVector(worldDelta);
            target.localPosition += localDelta;
        }
        #endregion

        #region 屏幕/世界矩形工具
        //=========================================================================
        // 屏幕/世界矩形工具（供地图相机对齐等外部计算）
        //=========================================================================

        /// <summary>
        /// 获取目标节点在世界空间的矩形（左下角 + 宽高）
        /// <para>矩形基于 GetWorldCorners，忽略 Z 轴（2D 平面）</para>
        /// </summary>
        /// <param name="target">目标节点</param>
        /// <returns>世界空间矩形（xMin,yMin,width,height）</returns>
        public static Rect GetWorldRect(RectTransform target)
        {
            if (target == null) return default;

            Vector3[] corners = new Vector3[4];
            target.GetWorldCorners(corners); // 0=左下 1=左上 2=右上 3=右下

            float xMin = Mathf.Min(corners[0].x, corners[3].x);
            float xMax = Mathf.Max(corners[0].x, corners[3].x);
            float yMin = Mathf.Min(corners[0].y, corners[1].y);
            float yMax = Mathf.Max(corners[0].y, corners[1].y);
            return new Rect(xMin, yMin, xMax - xMin, yMax - yMin);
        }

        /// <summary>
        /// 获取目标节点在屏幕空间的矩形
        /// <para>通过指定相机（或主相机）把世界角点转换到屏幕像素坐标</para>
        /// </summary>
        /// <param name="target">目标节点</param>
        /// <param name="camera">用于坐标转换的相机；Overlay 画布下可传 null 使用主相机</param>
        /// <returns>屏幕空间矩形（像素）</returns>
        public static Rect GetScreenRect(RectTransform target, Camera camera = null)
        {
            Rect worldRect = GetWorldRect(target);

            Vector3 min = new Vector3(worldRect.xMin, worldRect.yMin, 0);
            Vector3 max = new Vector3(worldRect.xMax, worldRect.yMax, 0);
            Vector3 screenMin = RectTransformUtility.WorldToScreenPoint(camera, min);
            Vector3 screenMax = RectTransformUtility.WorldToScreenPoint(camera, max);

            return new Rect(screenMin.x, screenMin.y, screenMax.x - screenMin.x, screenMax.y - screenMin.y);
        }
        #endregion

        #region 正交相机对齐辅助
        //=========================================================================
        // 正交相机对齐辅助（地图相机可视区域 ↔ UI 参考边，严丝合缝）
        //=========================================================================

        /// <summary>
        /// 调整正交相机，使相机可视区域的上/下边界对齐参考 UI 节点的指定边
        /// <para>用于"地图画面紧贴 Bg 下边"：地图相机可视区域与底部 Bg 严丝合缝，随 Canvas 适配自动计算</para>
        /// </summary>
        /// <param name="mapCamera">地图正交相机</param>
        /// <param name="reference">参考 UI 节点（如 Bg）</param>
        /// <param name="alignTop">true=相机可视上边界对齐 reference 下边；false=相机可视下边界对齐 reference 上边</param>
        /// <param name="margin">边距（世界单位）</param>
        public static void AdaptOrthoCameraToReference(Camera mapCamera, RectTransform reference,
            bool alignTop, float margin = 0f)
        {
            if (mapCamera == null || reference == null) return;

            // 参考节点上边 / 下边在世界空间的 Y
            Vector3[] corners = new Vector3[4];
            reference.GetWorldCorners(corners); // 0=左下 1=左上
            float refTop = corners[1].y;
            float refBottom = corners[0].y;

            // 相机可视区域世界范围（正交）：中心±ortho（垂直半高）、水平 half = ortho*aspect
            float ortho = mapCamera.orthographicSize;
            float camCenterY = mapCamera.transform.position.y;
            float camTop = camCenterY + ortho;
            float camBottom = camCenterY - ortho;

            if (alignTop)
            {
                // 相机可视上边界对齐 reference 下边：调整相机中心Y
                float targetTop = refBottom - margin;
                mapCamera.transform.position = new Vector3(
                    mapCamera.transform.position.x,
                    targetTop - ortho,
                    mapCamera.transform.position.z);
            }
            else
            {
                // 相机可视下边界对齐 reference 上边
                float targetBottom = refTop + margin;
                mapCamera.transform.position = new Vector3(
                    mapCamera.transform.position.x,
                    targetBottom + ortho,
                    mapCamera.transform.position.z);
            }
        }
        #endregion
    }
}
