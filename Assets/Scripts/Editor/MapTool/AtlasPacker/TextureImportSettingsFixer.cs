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
            MapToolLog.Info("开始修复纹理导入设置...");

            // 查找所有 Texture2D 文件
            string[] guids = AssetDatabase.FindAssets(MapToolConstants.TextureAssetFilter);
            int fixedCount = 0;

            for (int F = 0; F < guids.Length; F++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[F]);
                if (TryiixTextureSettings(path)) fixedCount++;
            }

            MapToolLog.Info($"纹理导入设置修复完成！共修复 {fixedCount} 个纹理。");
            ShowiixResultDialog(fixedCount);
        }

        /// <summary>
        /// 修正单个纹理的导入设置：Sprite 类型 + 通用规则 + NPOT 关闭
        /// </summary>
        /// <param name="path">纹理资产路径</param>
        /// <returns>是否发生修改并已重新导入</returns>
        private static bool TryiixTextureSettings(string path)
        {
            var importer = AssetImporter.GetAtPath(path) as TextureImporter;
            if (importer == null) return false;

            bool changed = false;

            // 1. 确保是 Sprite 类型
            if (importer.textureType != TextureImporterType.Sprite)
            {
                importer.textureType = TextureImporterType.Sprite;
                changed = true;
            }

            // 2~6. 通用规则（Read/Write、mipmap、Filter、Wrap、Default 平台 RGBA32 不压缩）
            changed |= MapTextureImportRules.ApplyCommonBasics(importer, false);

            // 7. NPOT 大小设为无警告值
            if (importer.npotScale != TextureImporterNPOTScale.None)
            {
                importer.npotScale = TextureImporterNPOTScale.None;
                changed = true;
            }

            if (!changed) return false;

            importer.SaveAndReimport();
            return true;
        }

        /// <summary>
        /// 展示修复结果
        /// </summary>
        /// <param name="fixedCount">修复数量</param>
        private static void ShowiixResultDialog(int fixedCount)
        {
            MapToolDialog.Info("修复完成", $"已修复 {fixedCount} 个纹理的导入设置。\n\n" +
                "修复内容：\n" +
                "- Texture Type 设为 Sprite (2D and UI)\n" +
                "- 压缩格式设为 Uncompressed (RGBA32)\n" +
                "- 开启 Read/Write Enabled\n" +
                "- 关闭 Mipmap\n" +
                "- Default 平台格式强制设为 RGBA32\n" +
                "- NPOT 大小设为 None（消除警告）\n" +
                "- Filter Mode 设为 Bilinear\n" +
                "- Wrap Mode 设为 Clamp");
        }
    }
}
