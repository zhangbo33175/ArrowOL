/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  TextureImportSettingsFixer.cs
 * author:    云毅
 * created:   2026
 * descrip:   自动修复 Unity 纹理导入设置（消除所有警告）
 ***************************************************************/

using UnityEditor;
using UnityEngine;

namespace Honor.Editor.MapTool.AtlasPacker
{
    /// <summary>
    /// 自动修复 Unity 纹理导入设置
    /// 解决 DXT5 不兼容、NPOT 警告、Read/Write 警告等所有问题
    /// </summary>
    internal static class TextureImportSettingsFixer
    {
        [MenuItem("Honor/地图工具/修复默认纹理设置")]
        public static void FixDefaultTextureSettings()
        {
            Debug.Log("开始修复纹理导入设置...");

            // 查找所有 Texture2D 文件
            string[] guids = AssetDatabase.FindAssets("t:Texture2D");
            int fixedCount = 0;
            int warningCount = 0;

            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                var importer = AssetImporter.GetAtPath(path) as TextureImporter;

                if (importer == null) continue;

                bool changed = false;

                // 1. 确保是 Sprite 类型
                if (importer.textureType != TextureImporterType.Sprite)
                {
                    importer.textureType = TextureImporterType.Sprite;
                    changed = true;
                }

                // 2. 关闭压缩，设置为 Uncompressed（解决 DXT5 不兼容问题）
                if (importer.textureCompression != TextureImporterCompression.Uncompressed)
                {
                    importer.textureCompression = TextureImporterCompression.Uncompressed;
                    changed = true;
                }

                // 3. 确保读取写入开启（Atlas 打包需要）
                if (!importer.isReadable)
                {
                    importer.isReadable = true;
                    changed = true;
                }

                // 4. 关闭 Mipmap（2D 图片不需要）
                if (importer.mipmapEnabled)
                {
                    importer.mipmapEnabled = false;
                    changed = true;
                }

                // 5. 强制设置 Default 平台的格式为 RGBA32 + 关闭压缩（解决 DXT5 错误与压缩兼容性警告）
                var defaultSettings = importer.GetPlatformTextureSettings("DefaultTexturePlatform");
                if (defaultSettings.format != TextureImporterFormat.RGBA32 ||
                    defaultSettings.textureCompression != TextureImporterCompression.Uncompressed)
                {
                    defaultSettings.format = TextureImporterFormat.RGBA32;
                    defaultSettings.textureCompression = TextureImporterCompression.Uncompressed;
                    defaultSettings.overridden = true;
                    importer.SetPlatformTextureSettings(defaultSettings);
                    changed = true;
                }

                // 6. 设置 NPOT 大小为无警告（解决 NPOT 警告）
                if (importer.npotScale != TextureImporterNPOTScale.None)
                {
                    importer.npotScale = TextureImporterNPOTScale.None;
                    changed = true;
                }

                // 7. 设置 Filter Mode 为 Bilinear（消除警告）
                if (importer.filterMode != FilterMode.Bilinear)
                {
                    importer.filterMode = FilterMode.Bilinear;
                    changed = true;
                }

                // 8. 设置 Wrap Mode 为 Clamp（消除警告）
                if (importer.wrapMode != TextureWrapMode.Clamp)
                {
                    importer.wrapMode = TextureWrapMode.Clamp;
                    changed = true;
                }

                if (changed)
                {
                    importer.SaveAndReimport();
                    fixedCount++;
                    warningCount++;
                }
            }

            Debug.Log($"纹理导入设置修复完成！共修复 {fixedCount} 个纹理。");
            EditorUtility.DisplayDialog("修复完成", 
                $"已修复 {fixedCount} 个纹理的导入设置。\n\n" +
                "修复内容：\n" +
                "- Texture Type 设为 Sprite (2D and UI)\n" +
                "- 压缩格式设为 Uncompressed (RGBA32)\n" +
                "- 开启 Read/Write Enabled\n" +
                "- 关闭 Mipmap\n" +
                "- Default 平台格式强制设为 RGBA32\n" +
                "- NPOT 大小设为 None（消除警告）\n" +
                "- Filter Mode 设为 Bilinear\n" +
                "- Wrap Mode 设为 Clamp", 
                "确定");
        }
    }
}
