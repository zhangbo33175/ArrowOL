using UnityEngine;
using UnityEngine.UI;

namespace Honor.Runtime
{
    /// <summary>
    /// RectTransform 扩展方法
    /// 提供 UI 矩形变换快捷操作：边距、锚点位置、布局重建
    /// </summary>
    public static partial class GameExtensionForUnity
    {
        /// <summary>
        /// 设置 RectTransform 左侧偏移量
        /// </summary>
        /// <param name="rectTransform">目标 RectTransform</param>
        /// <param name="left">左侧偏移</param>
        public static void SetLeft(this RectTransform rectTransform, float left)
        {
            if (rectTransform == null) return;
            rectTransform.offsetMin = new Vector2(left, rectTransform.offsetMin.y);
        }

        /// <summary>
        /// 设置 RectTransform 右侧偏移量
        /// </summary>
        /// <param name="rectTransform">目标 RectTransform</param>
        /// <param name="right">右侧偏移</param>
        public static void SetRight(this RectTransform rectTransform, float right)
        {
            if (rectTransform == null) return;
            rectTransform.offsetMax = new Vector2(-right, rectTransform.offsetMax.y);
        }

        /// <summary>
        /// 设置 RectTransform 顶部偏移量
        /// </summary>
        /// <param name="rectTransform">目标 RectTransform</param>
        /// <param name="top">顶部偏移</param>
        public static void SetTop(this RectTransform rectTransform, float top)
        {
            if (rectTransform == null) return;
            rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, -top);
        }

        /// <summary>
        /// 设置 RectTransform 底部偏移量
        /// </summary>
        /// <param name="rectTransform">目标 RectTransform</param>
        /// <param name="bottom">底部偏移</param>
        public static void SetBottom(this RectTransform rectTransform, float bottom)
        {
            if (rectTransform == null) return;
            rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, bottom);
        }

        /// <summary>
        /// 设置 3D 锚点位置的 X 轴
        /// </summary>
        public static void SetAnchoredPositionX3D(this RectTransform rectTransform, float newValue)
        {
            if (rectTransform == null) return;
            Vector3 v = rectTransform.anchoredPosition3D;
            v.x = newValue;
            rectTransform.anchoredPosition3D = v;
        }

        /// <summary>
        /// 设置 3D 锚点位置的 Y 轴
        /// </summary>
        public static void SetAnchoredPositionY3D(this RectTransform rectTransform, float newValue)
        {
            if (rectTransform == null) return;
            Vector3 v = rectTransform.anchoredPosition3D;
            v.y = newValue;
            rectTransform.anchoredPosition3D = v;
        }

        /// <summary>
        /// 设置 3D 锚点位置的 Z 轴
        /// </summary>
        public static void SetAnchoredPositionZ3D(this RectTransform rectTransform, float newValue)
        {
            if (rectTransform == null) return;
            Vector3 v = rectTransform.anchoredPosition3D;
            v.z = newValue;
            rectTransform.anchoredPosition3D = v;
        }

        /// <summary>
        /// 增加 3D 锚点位置的 X 轴值
        /// </summary>
        public static void AddAnchoredPositionX3D(this RectTransform rectTransform, float deltaValue)
        {
            if (rectTransform == null) return;
            Vector3 v = rectTransform.anchoredPosition3D;
            v.x += deltaValue;
            rectTransform.anchoredPosition3D = v;
        }

        /// <summary>
        /// 增加 3D 锚点位置的 Y 轴值
        /// </summary>
        public static void AddAnchoredPositionY3D(this RectTransform rectTransform, float deltaValue)
        {
            if (rectTransform == null) return;
            Vector3 v = rectTransform.anchoredPosition3D;
            v.y += deltaValue;
            rectTransform.anchoredPosition3D = v;
        }

        /// <summary>
        /// 增加 3D 锚点位置的 Z 轴值
        /// </summary>
        public static void AddAnchoredPositionZ3D(this RectTransform rectTransform, float deltaValue)
        {
            if (rectTransform == null) return;
            Vector3 v = rectTransform.anchoredPosition3D;
            v.z += deltaValue;
            rectTransform.anchoredPosition3D = v;
        }

        /// <summary>
        /// 设置锚点位置的 X 轴
        /// </summary>
        public static void SetAnchoredPositionX(this RectTransform rectTransform, float newValue)
        {
            if (rectTransform == null) return;
            Vector2 v = rectTransform.anchoredPosition;
            v.x = newValue;
            rectTransform.anchoredPosition = v;
        }

        /// <summary>
        /// 设置锚点位置的 Y 轴
        /// </summary>
        public static void SetAnchoredPositionY(this RectTransform rectTransform, float newValue)
        {
            if (rectTransform == null) return;
            Vector2 v = rectTransform.anchoredPosition;
            v.y = newValue;
            rectTransform.anchoredPosition = v;
        }

        /// <summary>
        /// 增加锚点位置的 X 轴值
        /// </summary>
        public static void AddAnchoredPositionX(this RectTransform rectTransform, float deltaValue)
        {
            if (rectTransform == null) return;
            Vector2 v = rectTransform.anchoredPosition;
            v.x += deltaValue;
            rectTransform.anchoredPosition = v;
        }

        /// <summary>
        /// 增加锚点位置的 Y 轴值
        /// </summary>
        public static void AddAnchoredPositionY(this RectTransform rectTransform, float deltaValue)
        {
            if (rectTransform == null) return;
            Vector2 v = rectTransform.anchoredPosition;
            v.y += deltaValue;
            rectTransform.anchoredPosition = v;
        }

        /// <summary>
        /// 强制立即重建 UI 布局
        /// </summary>
        public static void ForceRebuildLayoutImmediate(this RectTransform rectTransform)
        {
            if (rectTransform == null) return;
            LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
        }
    }
}