/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  MapTextureImportRules.cs
 * author:    云毅
 * created:   2026
 * descrip:   地图纹理导入设置通用修正规则
 *            统一「Read/Write、mipmap、Filter、Wrap、Default 平台 RGBA32 不压缩」口径，
 *            消除 PSD 导入与图集修复两处各自实现的重复代码
 ***************************************************************/

using UnityEditor;
using UnityEngine;

namespace Honor.Editor.MapTool
{
    /// <summary>
    /// 地图纹理导入设置通用修正规则
    /// </summary>
    public static class MapTextureImportRules
    {
        /// <summary>Default 平台标识</summary>
        public const string DefaultPlatform = "DefaultTexturePlatform";

        /// <summary>
        /// 应用通用修正：开启 Read/Write、关闭 mipmap、Filter=Bilinear、Wrap=Clamp、
        /// Default 平台格式 RGBA32 且不压缩
        /// </summary>
        /// <param name="importer">纹理导入器</param>
        /// <param name="requireOverride">是否要求平台覆盖标记为已启用（PSD 导入侧要求，图集修复侧沿用旧口径）</param>
        /// <returns>是否有任何设置被修改</returns>
        public static bool ApplyCommonBasics(TextureImporter importer, bool requireOverride)
        {
            if (importer == null) return false;

            bool changed = false;

            // Read/Write（图集打包与像素读取需要）
            if (!importer.isReadable)
            {
                importer.isReadable = true;
                changed = true;
            }

            // 关闭 mipmap（2D 图片不需要）
            if (importer.mipmapEnabled)
            {
                importer.mipmapEnabled = false;
                changed = true;
            }

            // Filter / Wrap（消除导入警告）
            if (importer.filterMode != FilterMode.Bilinear)
            {
                importer.filterMode = FilterMode.Bilinear;
                changed = true;
            }

            if (importer.wrapMode != TextureWrapMode.Clamp)
            {
                importer.wrapMode = TextureWrapMode.Clamp;
                changed = true;
            }

            // Default 平台：RGBA32 + 不压缩（解决 DXT5 不兼容）
            var defaultSettings = importer.GetPlatformTextureSettings(DefaultPlatform);
            bool overrideMismatch = requireOverride && !defaultSettings.overridden;
            if (overrideMismatch ||
                defaultSettings.format != TextureImporterFormat.RGBA32 ||
                defaultSettings.textureCompression != TextureImporterCompression.Uncompressed)
            {
                defaultSettings.overridden = true;
                defaultSettings.format = TextureImporterFormat.RGBA32;
                defaultSettings.textureCompression = TextureImporterCompression.Uncompressed;
                importer.SetPlatformTextureSettings(defaultSettings);
                changed = true;
            }

            return changed;
        }
    }
}
