using UnityEngine;
using UnityEngine.UI;

namespace GameLib
{
    /// <summary>
    /// 全屏图片自适应适配脚本
    /// 作用：使Image保持原始宽高比，自动缩放以完全铺满父物体（无拉伸、无裁剪）
    /// </summary>
    [DisallowMultipleComponent]
    public class AorUIFullScreenImage : MonoBehaviour
    {
        /// <summary>
        /// 需要进行全屏适配的目标Image组件
        /// </summary>
        [Header("目标图片")] [Tooltip("拖拽需要适配的Image组件到此字段")]
        public Image image;

        /// <summary>
        /// 作为适配基准的父物体RectTransform
        /// 图片将根据该物体的尺寸进行缩放适配
        /// </summary>
        [Header("父物体适配区域")] [Tooltip("图片的父物体RectTransform，作为适配的尺寸参考")]
        public RectTransform parentRectTransform;

        /// <summary>
        /// 初始化时执行图片适配逻辑
        /// </summary>
        private void Start()
        {
            // 空值校验，防止引用丢失导致报错
            if (image == null || image.sprite == null || parentRectTransform == null)
            {
                Debug.LogError($"【{nameof(AorUIFullScreenImage)}】组件引用缺失，请检查Image和父物体引用！", gameObject);
                return;
            }

            AdaptToFullScreen();
        }

        /// <summary>
        /// 图片全屏适配核心方法
        /// 计算图片原始宽高比与父物体尺寸比例，自动缩放保证无拉伸铺满父物体
        /// </summary>
        private void AdaptToFullScreen()
        {
            // 获取父物体实际尺寸
            Vector2 parentSize = parentRectTransform.rect.size;
            float parentWidth = parentSize.x;
            float parentHeight = parentSize.y;

            // 获取图片精灵原始设计尺寸
            Vector2 spriteSize = image.sprite.rect.size;
            float spriteWidth = spriteSize.x;
            float spriteHeight = spriteSize.y;

            // 计算宽高缩放比例
            float widthScale = spriteWidth / parentWidth;
            float heightScale = spriteHeight / parentHeight;

            // 取最小比例，保证图片完整显示且铺满父物体
            float minScale = Mathf.Min(widthScale, heightScale);
            float finalScale = 1f / minScale;

            // 应用缩放
            transform.localScale = Vector3.one * finalScale;
        }
    }
}