/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  AtlasPreviewRenderer.cs
 * author:    云毅
 * created:   2026
 * descrip:   预览渲染器（绘制预览图、选中高亮、缩放平移）
 ***************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Honor.Editor.MapTool.AtlasPacker
{
    /// <summary>
    /// 预览渲染器
    /// 负责预览区的绘制、选中高亮、缩放平移等
    /// </summary>
    internal class AtlasPreviewRenderer
    {
        // UI 元素引用（由外部传入）
        public VisualElement PreviewContainer { get; set; }
        public Label AtlasSizeLabel { get; set; }
        public Label EmptyHintLabel { get; set; }
        public Label ZoomLabel { get; set; }

        // 状态
        public int AtlasWidth { get; private set; }
        public int AtlasHeight { get; private set; }
        public float ZoomScale { get; private set; } = 1.0f;
        public int SelectedIndex { get; private set; } = -1;
        public VisualElement SelectedHighlightBox { get; private set; }

        // 委托：加载纹理
        public Func<string, Texture2D> TextureLoader { get; set; }

        // 委托：选中回调
        public Action<int> OnSpriteClicked { get; set; }

        /// <summary>
        /// 绘制预览图
        /// </summary>
        public void RenderPreview(
            List<SpriteItemInfo> items,
            int atlasWidth, int atlasHeight,
            int padding, int shapePadding)
        {
            AtlasWidth = atlasWidth;
            AtlasHeight = atlasHeight;

            if (items == null || items.Count == 0)
            {
                ShowEmptyHint();
                return;
            }

            // 隐藏提示，显示预览
            if (EmptyHintLabel != null) EmptyHintLabel.visible = false;
            if (PreviewContainer != null) PreviewContainer.visible = true;

            // 清空之前的预览
            PreviewContainer?.Clear();
            SelectedHighlightBox = null;

            // 预览容器大小
            if (PreviewContainer != null)
            {
                PreviewContainer.style.width = atlasWidth;
                PreviewContainer.style.height = atlasHeight;
            }

            // 生成棋盘格背景纹理
            var checkerTex = CreateCheckerTexture(atlasWidth, atlasHeight);
            var bgImage = new Image { image = checkerTex };
            bgImage.style.width = atlasWidth;
            bgImage.style.height = atlasHeight;
            PreviewContainer?.Add(bgImage);

            // 绘制每个小图
            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];
                var tex = TextureLoader?.Invoke(item.Path);
                if (tex == null) continue;

                var imgElement = new Image { image = tex };
                imgElement.style.position = Position.Absolute;
                imgElement.style.left = item.PosX;
                imgElement.style.top = item.PosY;
                imgElement.style.width = tex.width;
                imgElement.style.height = tex.height;

                int index = i;
                imgElement.RegisterCallback<ClickEvent>(evt =>
                {
                    OnSpriteClicked?.Invoke(index);
                });

                PreviewContainer?.Add(imgElement);
            }

            // 更新状态栏
            UpdateStatusLabel();
        }

        /// <summary>
        /// 设置缩放比例
        /// </summary>
        public void SetZoom(float scale)
        {
            ZoomScale = Mathf.Clamp(scale, 0.1f, 3.0f);
            if (PreviewContainer != null)
            {
                PreviewContainer.style.scale = new Scale(new Vector3(ZoomScale, ZoomScale, 1));
            }
            UpdateZoomLabel();
        }

        /// <summary>
        /// 设置选中项
        /// </summary>
        public void SetSelected(int index, List<SpriteItemInfo> items)
        {
            SelectedIndex = index;
            ShowHighlightBox(items);
        }

        /// <summary>
        /// 显示空提示
        /// </summary>
        private void ShowEmptyHint()
        {
            if (EmptyHintLabel != null) EmptyHintLabel.visible = true;
            if (PreviewContainer != null) PreviewContainer.visible = false;
            if (AtlasSizeLabel != null) AtlasSizeLabel.text = "Empty texture";
        }

        /// <summary>
        /// 创建棋盘格背景纹理
        /// </summary>
        private Texture2D CreateCheckerTexture(int width, int height)
        {
            var tex = new Texture2D(width, height, TextureFormat.RGBA32, false);
            tex.hideFlags = HideFlags.HideAndDontSave;

            int checkerSize = 8;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    bool isEven = ((x / checkerSize) + (y / checkerSize)) % 2 == 0;
                    Color color = isEven ? new Color(0.2f, 0.2f, 0.2f) : new Color(0.25f, 0.25f, 0.25f);
                    tex.SetPixel(x, y, color);
                }
            }
            tex.Apply();
            return tex;
        }

        /// <summary>
        /// 显示选中高亮框
        /// </summary>
        private void ShowHighlightBox(List<SpriteItemInfo> items)
        {
            if (SelectedHighlightBox != null)
            {
                SelectedHighlightBox.RemoveFromHierarchy();
                SelectedHighlightBox = null;
            }

            if (SelectedIndex < 0 || SelectedIndex >= items.Count) return;
            if (PreviewContainer == null) return;

            var item = items[SelectedIndex];
            var tex = TextureLoader?.Invoke(item.Path);
            if (tex == null) return;

            // 高亮框放在图片下面（Insert 0）
            SelectedHighlightBox = new VisualElement();
            SelectedHighlightBox.style.position = Position.Absolute;
            SelectedHighlightBox.style.left = item.PosX + 2;
            SelectedHighlightBox.style.top = item.PosY + 2;
            SelectedHighlightBox.style.width = tex.width + 4;
            SelectedHighlightBox.style.height = tex.height + 4;
            SelectedHighlightBox.style.backgroundColor = Color.clear;
            SelectedHighlightBox.style.borderTopWidth = 2;
            SelectedHighlightBox.style.borderBottomWidth = 2;
            SelectedHighlightBox.style.borderLeftWidth = 2;
            SelectedHighlightBox.style.borderRightWidth = 2;
            SelectedHighlightBox.style.borderTopColor = Color.black;
            SelectedHighlightBox.style.borderBottomColor = Color.black;
            SelectedHighlightBox.style.borderLeftColor = Color.black;
            SelectedHighlightBox.style.borderRightColor = Color.black;
            // TODO: 2022.3 UIElements 不支持虚线边框（无 border-style），此处暂为实线；
            //       虚线效果由窗口主链路 AtlasPackerWindow 用背景色短划线拼装实现。
            //       注意：本渲染器 SetSelected 当前无调用点，属遗留未接线链路。

            PreviewContainer.Add(SelectedHighlightBox);
        }

        /// <summary>
        /// 更新状态标签
        /// </summary>
        private void UpdateStatusLabel()
        {
            if (AtlasSizeLabel == null) return;
            int memKB = AtlasWidth * AtlasHeight * 4 / 1024;
            AtlasSizeLabel.text = $"Size: {AtlasWidth}x{AtlasHeight} ({memKB}kB)";
        }

        /// <summary>
        /// 更新缩放标签
        /// </summary>
        private void UpdateZoomLabel()
        {
            if (ZoomLabel == null) return;
            ZoomLabel.text = $"Zoom: {(ZoomScale * 100):F0}%";
        }
    }
}
