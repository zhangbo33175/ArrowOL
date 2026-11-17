/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  TextureLoader.cs
 * author:    云毅
 * created:   2026
 * descrip:   纹理加载器（带缓存）
 ***************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Honor.Editor.MapTool.AtlasPacker
{
    /// <summary>
    /// 纹理加载器（带缓存，避免重复加载磁盘纹理）
    /// </summary>
    internal class TextureLoader
    {
        private Dictionary<string, Texture2D> m_Cache = new Dictionary<string, Texture2D>();

        /// <summary>
        /// 从磁盘加载纹理（带缓存）
        /// </summary>
        public Texture2D LoadFromDisk(string assetPath, Func<string, string> assetPathToFullPath)
        {
            string fullPath = assetPathToFullPath(assetPath);
            if (!File.Exists(fullPath)) return null;

            // 缓存命中
            if (m_Cache.TryGetValue(fullPath, out var cached))
            {
                return cached;
            }

            try
            {
                byte[] bytes = File.ReadAllBytes(fullPath);
                var tex = new Texture2D(2, 2);
                tex.LoadImage(bytes);
                tex.hideFlags = HideFlags.HideAndDontSave;
                m_Cache[fullPath] = tex;
                return tex;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 清空缓存
        /// </summary>
        public void ClearCache()
        {
            m_Cache.Clear();
        }

        /// <summary>
        /// 计算纹理的实际边界（裁剪透明边缘）
        /// </summary>
        public RectInt GetTextureBounds(Texture2D tex)
        {
            if (tex == null) return default;

            int minX = tex.width;
            int minY = tex.height;
            int maxX = 0;
            int maxY = 0;
            bool hasContent = false;

            // 采样像素（步长为 2，提高性能）
            for (int y = 0; y < tex.height; y += 2)
            {
                for (int x = 0; x < tex.width; x += 2)
                {
                    var pixel = tex.GetPixel(x, y);
                    if (pixel.a > 0.1f)
                    {
                        hasContent = true;
                        if (x < minX) minX = x;
                        if (y < minY) minY = y;
                        if (x > maxX) maxX = x;
                        if (y > maxY) maxY = y;
                    }
                }
            }

            if (!hasContent) return new RectInt(0, 0, tex.width, tex.height);

            // 扩大一点，避免裁剪过头
            minX = Mathf.Max(0, minX - 1);
            minY = Mathf.Max(0, minY - 1);
            maxX = Mathf.Min(tex.width - 1, maxX + 1);
            maxY = Mathf.Min(tex.height - 1, maxY + 1);

            return new RectInt(minX, minY, maxX - minX + 1, maxY - minY + 1);
        }
    }
}
