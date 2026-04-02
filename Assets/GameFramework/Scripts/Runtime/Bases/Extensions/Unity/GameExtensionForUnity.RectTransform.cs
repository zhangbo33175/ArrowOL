using UnityEngine;
using UnityEngine.UI;

namespace Honor.Runtime
{
    public static partial class GameExtensionForUnity
    {
        /// <summary>
        /// 设置RectTransform左侧偏移量
        /// </summary>
        /// <param name="rectTransform"><see cref="RectTransform" /> 对象。</param>
        /// <param name="left">左侧偏移量</param>
        public static void SetLeft(this RectTransform rectTransform, float left)
        {
            rectTransform.offsetMin = new Vector2(left, rectTransform.offsetMin.y);
        }

        /// <summary>
        /// 设置RectTransform右侧偏移量
        /// </summary>
        /// <param name="rectTransform"><see cref="RectTransform" /> 对象。</param>
        /// <param name="right">右侧偏移量</param>
        public static void SetRight(this RectTransform rectTransform, float right)
        {
            rectTransform.offsetMax = new Vector2(-right, rectTransform.offsetMax.y);
        }

        /// <summary>
        /// 设置RectTransform顶端偏移量
        /// </summary>
        /// <param name="rectTransform"><see cref="RectTransform" /> 对象。</param>
        /// <param name="top">顶端偏移量</param>
        public static void SetTop(this RectTransform rectTransform, float top)
        {
            rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, -top);
        }

        /// <summary>
        /// 设置RectTransform底端偏移量
        /// </summary>
        /// <param name="rectTransform"><see cref="RectTransform" /> 对象。</param>
        /// <param name="bottom">底端偏移量</param>
        public static void SetBottom(this RectTransform rectTransform, float bottom)
        {
            rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, bottom);
        }

        /// <summary>
        /// 设置Anchored位置的3d x 坐标。
        /// </summary>
        /// <param name="rectTransform"><see cref="RectTransform" /> 对象。</param>
        /// <param name="newValue">x 坐标值。</param>
        public static void SetAnchoredPositionX3D(this RectTransform rectTransform, float newValue)
        {
            Vector3 v = rectTransform.anchoredPosition3D;
            v.x = newValue;
            rectTransform.anchoredPosition3D = v;
        }

        /// <summary>
        /// 设置Anchored位置的3d y 坐标。
        /// </summary>
        /// <param name="rectTransform"><see cref="RectTransform" /> 对象。</param>
        /// <param name="newValue">y 坐标值。</param>
        public static void SetAnchoredPositionY3D(this RectTransform rectTransform, float newValue)
        {
            Vector3 v = rectTransform.anchoredPosition3D;
            v.y = newValue;
            rectTransform.anchoredPosition3D = v;
        }

        /// <summary>
        /// 设置Anchored位置的3d z 坐标。
        /// </summary>
        /// <param name="rectTransform"><see cref="RectTransform" /> 对象。</param>
        /// <param name="newValue">z 坐标值。</param>
        public static void SetAnchoredPositionZ3D(this RectTransform rectTransform, float newValue)
        {
            Vector3 v = rectTransform.anchoredPosition3D;
            v.z = newValue;
            rectTransform.anchoredPosition3D = v;
        }

        /// <summary>
        /// 增加Anchored位置的3d x 坐标。
        /// </summary>
        /// <param name="rectTransform"><see cref="RectTransform" /> 对象。</param>
        /// <param name="deltaValue">x 坐标值增量。</param>
        public static void AddAnchoredPositionX3D(this RectTransform rectTransform, float deltaValue)
        {
            Vector3 v = rectTransform.anchoredPosition3D;
            v.x += deltaValue;
            rectTransform.anchoredPosition3D = v;
        }

        /// <summary>
        /// 增加Anchored位置的3d y 坐标。
        /// </summary>
        /// <param name="rectTransform"><see cref="RectTransform" /> 对象。</param>
        /// <param name="deltaValue">y 坐标值增量。</param>
        public static void AddAnchoredPositionY3D(this RectTransform rectTransform, float deltaValue)
        {
            Vector3 v = rectTransform.anchoredPosition3D;
            v.y += deltaValue;
            rectTransform.anchoredPosition3D = v;
        }

        /// <summary>
        /// 增加Anchored位置的3d y 坐标。
        /// </summary>
        /// <param name="rectTransform"><see cref="RectTransform" /> 对象。</param>
        /// <param name="deltaValue">y 坐标值增量。</param>
        public static void AddAnchoredPositionZ3D(this RectTransform rectTransform, float deltaValue)
        {
            Vector3 v = rectTransform.anchoredPosition3D;
            v.z += deltaValue;
            rectTransform.anchoredPosition3D = v;
        }

        /// <summary>
        /// 设置Anchored位置的 x 坐标。
        /// </summary>
        /// <param name="rectTransform"><see cref="RectTransform" /> 对象。</param>
        /// <param name="newValue">x 坐标值。</param>
        public static void SetAnchoredPositionX(this RectTransform rectTransform, float newValue)
        {
            Vector2 v = rectTransform.anchoredPosition;
            v.x = newValue;
            rectTransform.anchoredPosition = v;
        }

        /// <summary>
        /// 设置Anchored位置的 y 坐标。
        /// </summary>
        /// <param name="rectTransform"><see cref="RectTransform" /> 对象。</param>
        /// <param name="newValue">y 坐标值。</param>
        public static void SetAnchoredPositionY(this RectTransform rectTransform, float newValue)
        {
            Vector2 v = rectTransform.anchoredPosition;
            v.y = newValue;
            rectTransform.anchoredPosition = v;
        }

        /// <summary>
        /// 增加Anchored位置的 x 坐标。
        /// </summary>
        /// <param name="rectTransform"><see cref="RectTransform" /> 对象。</param>
        /// <param name="deltaValue">x 坐标值增量。</param>
        public static void AddAnchoredPositionX(this RectTransform rectTransform, float deltaValue)
        {
            Vector2 v = rectTransform.anchoredPosition;
            v.x += deltaValue;
            rectTransform.anchoredPosition = v;
        }

        /// <summary>
        /// 增加Anchored位置的 y 坐标。
        /// </summary>
        /// <param name="rectTransform"><see cref="RectTransform" /> 对象。</param>
        /// <param name="deltaValue">y 坐标值增量。</param>
        public static void AddAnchoredPositionY(this RectTransform rectTransform, float deltaValue)
        {
            Vector2 v = rectTransform.anchoredPosition;
            v.y += deltaValue;
            rectTransform.anchoredPosition = v;
        }

        /// <summary>
        /// 强制刷新ui的布局
        /// </summary>
        /// <param name="rectTransform"></param>
        public static void ForceRebuildLayoutImmediate(this RectTransform rectTransform)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
        }

    }


}


