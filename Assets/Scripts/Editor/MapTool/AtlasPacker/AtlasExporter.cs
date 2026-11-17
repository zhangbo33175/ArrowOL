/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  AtlasExporter.cs
 * author:    云毅
 * created:   2026
 * descrip:   Atlas 导出功能（PNG + JSON）
 ***************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Honor.Editor.MapTool.AtlasPacker
{
    /// <summary>
    /// Atlas 导出器
    /// 负责生成 PNG 纹理和 JSON 描述文件
    /// </summary>
    internal class AtlasExporter
    {
        // 委托：加载纹理
        public Func<string, Texture2D> TextureLoader { get; set; }

        // 委托：把项目路径转成磁盘全路径
        public Func<string, string> AssetPathToFullPath { get; set; }

        /// <summary>
        /// 生成图集 PNG 纹理
        /// </summary>
        /// <param name="items">已排序的图片项（包含位置）</param>
        /// <param name="atlasWidth">图集宽度</param>
        /// <param name="atlasHeight">图集高度</param>
        /// <param name="padding">边框间隙</param>
        /// <param name="shapePadding">图片间隙</param>
        /// <param name="premultiplyAlpha">是否预乘 alpha（rgb 按 alpha 缩放）</param>
        /// <param name="extrude">精灵四周向外扩边像素数（边缘像素向外复制填充）</param>
        /// <returns>生成的纹理</returns>
        public Texture2D GenerateAtlasPng(
            List<SpriteItemInfo> items,
            int atlasWidth, int atlasHeight,
            int padding, int shapePadding,
            bool premultiplyAlpha = false, int extrude = 0)
        {
            var atlasTex = new Texture2D(atlasWidth, atlasHeight, TextureFormat.RGBA32, false);
            atlasTex.hideFlags = HideFlags.HideAndDontSave;

            FillTransparentBackground(atlasTex, atlasWidth, atlasHeight);

            // 复制每个小图（注意 y 坐标翻转：UI 左上角原点 vs Texture2D 左下角原点）
            foreach (var item in items)
                BlitItemIntoAtlas(atlasTex, atlasHeight, item, premultiplyAlpha, extrude);

            ClearTransparentRgb(atlasTex);

            atlasTex.Apply();
            return atlasTex;
        }

        /// <summary>
        /// 把图集整幅填充为全透明（写盘前底色）
        /// </summary>
        /// <param name="atlasTex">目标纹理</param>
        /// <param name="atlasWidth">宽</param>
        /// <param name="atlasHeight">高</param>
        private static void FillTransparentBackground(Texture2D atlasTex, int atlasWidth, int atlasHeight)
        {
            for (int px = 0; px < atlasWidth; px++)
            {
                for (int py = 0; py < atlasHeight; py++)
                {
                    atlasTex.SetPixel(px, py, Color.clear);
                }
            }
        }

        /// <summary>
        /// 把单张精灵贴进图集：跳过排除项/别名项，旋转与预乘在临时缓冲中处理后再扩边写入
        /// </summary>
        /// <param name="atlasTex">目标图集纹理</param>
        /// <param name="atlasHeight">图集高度（用于 y 翻转）</param>
        /// <param name="item">精灵项（含位置/旋转/排除状态）</param>
        /// <param name="premultiplyAlpha">是否预乘 alpha</param>
        /// <param name="extrude">四周外扩像素数</param>
        private void BlitItemIntoAtlas(Texture2D atlasTex, int atlasHeight, SpriteItemInfo item,
            bool premultiplyAlpha, int extrude)
        {
            // 跳过被排除的超尺寸 / 加载失败图片；别名复用主项槽位，不重复拷贝
            if (item.Excluded || item.IsAlias) return;

            var tex = LoadTexture(item.Path);
            if (tex == null) return;

            int w = tex.width;
            int h = tex.height;
            int placedW = item.Rotated ? h : w;
            int placedH = item.Rotated ? w : h;

            // 占位四周已预留 extrude 边距，贴图主体内缩 extrude 放置。
            // PosX/PosY 已由排布计算器按含 border padding 的左上角坐标给出，这里不再叠加 padding。
            int destX = item.PosX + extrude;
            int destY = atlasHeight - item.PosY - extrude - placedH;

            var body = BuildItemBody(tex, item.Rotated, premultiplyAlpha);
            WriteBodyAndExtrude(atlasTex, body, destX, destY, placedW, placedH, extrude);
        }

        /// <summary>
        /// 把源图读入临时像素缓冲，按需顺时针旋转 90° 并做 alpha 预乘
        /// </summary>
        /// <param name="tex">源纹理</param>
        /// <param name="rotated">是否顺时针旋转 90°</param>
        /// <param name="premultiplyAlpha">是否预乘 alpha</param>
        /// <returns>像素缓冲（宽高为放置后的尺寸）</returns>
        private static Color[,] BuildItemBody(Texture2D tex, bool rotated, bool premultiplyAlpha)
        {
            int w = tex.width;
            int h = tex.height;
            var body = new Color[rotated ? h : w, rotated ? w : h];

            if (rotated)
            {
                // 顺时针旋转 90° 拷贝（对齐 TexturePacker 标准：图集内为原图顺时针 90°）。
                // Texture2D 原点在左下，源 (x,y) -> 目标 (nx=y, ny=w-1-x)，新图宽 h、高 w。
                for (int y = 0; y < h; y++)
                {
                    for (int x = 0; x < w; x++)
                    {
                        body[y, w - 1 - x] = ApplyPremultiply(tex.GetPixel(x, y), premultiplyAlpha);
                    }
                }
            }
            else
            {
                for (int y = 0; y < h; y++)
                {
                    for (int x = 0; x < w; x++)
                    {
                        body[x, y] = ApplyPremultiply(tex.GetPixel(x, y), premultiplyAlpha);
                    }
                }
            }

            return body;
        }

        /// <summary>
        /// 透明像素 RGB 清零：源图透明边常残留非零 RGB，保留会增加颜色种类、降低 PNG 压缩率。
        /// 对齐 TexturePacker 产物口径：alpha=0 的像素 rgb 统一为 0。
        /// </summary>
        /// <param name="atlasTex">图集纹理</param>
        private static void ClearTransparentRgb(Texture2D atlasTex)
        {
            var pixels = atlasTex.GetPixels32();
            for (int i = 0; i < pixels.Length; i++)
            {
                if (pixels[i].a == 0)
                {
                    pixels[i].r = 0;
                    pixels[i].g = 0;
                    pixels[i].b = 0;
                }
            }
            atlasTex.SetPixels32(pixels);
        }

        /// <summary>按开关决定是否对颜色做 alpha 预乘（rgb *= a）。</summary>
        private static Color ApplyPremultiply(Color pixel, bool premultiplyAlpha)
        {
            if (!premultiplyAlpha) return pixel;
            pixel.r *= pixel.a;
            pixel.g *= pixel.a;
            pixel.b *= pixel.a;
            return pixel;
        }

        /// <summary>把主体缓冲写入图集，并在四周 extrude 范围内用最近边缘像素向外延展填充。</summary>
        private static void WriteBodyAndExtrude(Texture2D atlasTex, Color[,] body,
            int destX, int destY, int bodyW, int bodyH, int extrude)
        {
            int atlasW = atlasTex.width;
            int atlasH = atlasTex.height;

            // 主体
            for (int y = 0; y < bodyH; y++)
            {
                for (int x = 0; x < bodyW; x++)
                {
                    int ax = destX + x;
                    int ay = destY + y;
                    if (ax < 0 || ay < 0 || ax >= atlasW || ay >= atlasH) continue;
                    atlasTex.SetPixel(ax, ay, body[x, y]);
                }
            }

            if (extrude <= 0) return;

            // 上下边：取主体顶/底行像素向下/向上延展；左右边取主体左/右列像素向外延展
            for (int d = 1; d <= extrude; d++)
            {
                // 顶部（图集 y 增大方向为上，对应主体上边缘 bodyH-1）
                for (int x = 0; x < bodyW; x++)
                {
                    int ax = destX + x;
                    int ay = destY + bodyH - 1 + d;
                    if (ax >= 0 && ay < atlasH) atlasTex.SetPixel(ax, ay, body[x, bodyH - 1]);
                    ay = destY - d;
                    if (ax >= 0 && ay >= 0) atlasTex.SetPixel(ax, ay, body[x, 0]);
                }
                // 左右列
                for (int y = 0; y < bodyH; y++)
                {
                    int ay = destY + y;
                    int ax = destX - d;
                    if (ay >= 0 && ax >= 0) atlasTex.SetPixel(ax, ay, body[0, y]);
                    ax = destX + bodyW - 1 + d;
                    if (ay < atlasH && ax < atlasW) atlasTex.SetPixel(ax, ay, body[bodyW - 1, y]);
                }
            }
        }

        /// <summary>
        /// 生成 JSON 描述文件内容
        /// </summary>
        /// <param name="items">图片项列表</param>
        /// <param name="textureFilename">纹理文件名</param>
        /// <param name="width">图集宽度</param>
        /// <param name="height">图集高度</param>
        /// <param name="padding">边框间隙</param>
        /// <returns>JSON 字符串</returns>
        public string GenerateJson(
            List<SpriteItemInfo> items,
            string textureFilename,
            int width, int height,
            int padding)
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("{");
            sb.AppendLine($"  \"image\": \"{textureFilename}\",");
            sb.AppendLine($"  \"size\": {{ \"w\": {width}, \"h\": {height} }},");
            sb.AppendLine("  \"sprites\": {");

            var packedItems = new List<SpriteItemInfo>();
            foreach (var item in items)
            {
                if (item.Excluded) continue;
                packedItems.Add(item);
            }

            for (int i = 0; i < packedItems.Count; i++)
            {
                var item = packedItems[i];
                var tex = LoadTexture(item.Path);
                int srcW = tex != null ? tex.width : 0;
                int srcH = tex != null ? tex.height : 0;
                // 旋转 90° 时帧宽高互换
                int frameW = item.Rotated ? srcH : srcW;
                int frameH = item.Rotated ? srcW : srcH;

                // JSON 中的 y 坐标：从顶部开始。PosX/PosY 已含 border padding，不再叠加。
                int jsonY = item.PosY;

                sb.Append($"    \"{item.Name}\": {{ \"x\": {item.PosX}, \"y\": {jsonY}, \"w\": {frameW}, \"h\": {frameH}, \"rotated\": {(item.Rotated ? "true" : "false")} }}");
                if (i < packedItems.Count - 1) sb.AppendLine(",");
                else sb.AppendLine();
            }

            sb.AppendLine("  }");
            sb.AppendLine("}");
            return sb.ToString();
        }

        /// <summary>
        /// 保存文件到磁盘
        /// </summary>
        public void SaveToFile(string path, byte[] bytes)
        {
            File.WriteAllBytes(path, bytes);
        }

        public void SaveToFile(string path, string content)
        {
            File.WriteAllText(path, content);
        }

        /// <summary>
        /// 加载纹理（通过委托）
        /// </summary>
        private Texture2D LoadTexture(string assetPath)
        {
            string fullPath = AssetPathToFullPath?.Invoke(assetPath);
            return TextureLoader?.Invoke(fullPath);
        }
    }
}
