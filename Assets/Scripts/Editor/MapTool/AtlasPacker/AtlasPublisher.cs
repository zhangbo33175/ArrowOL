/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  AtlasPublisher.cs
 * author:    云毅
 * created:   2026
 * descrip:   发布器（导出 PNG + JSON 文件）
 ***************************************************************/

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Honor.Editor.MapTool.AtlasPacker
{
    /// <summary>
    /// 发布器
    /// 负责导出图集 PNG 和 JSON 描述文件
    /// </summary>
    internal class AtlasPublisher
    {
        // UI 元素引用（由外部传入）
        public TextField TxtTexturePath { get; set; }
        public TextField TxtDataFilename { get; set; }
        public TextField TxtTextureFile { get; set; }
        public DropdownField PopupMaxWidth { get; set; }
        public DropdownField PopupMaxHeight { get; set; }
        public IntegerField IntBorderPadding { get; set; }
        public IntegerField IntShapePadding { get; set; }

        // 数据
        public List<SpriteItemInfo> Items { get; set; }

        // 委托：计算最优排布
        public Func<int, int, int, int, (bool success, int width, int height, List<Vector2Int> positions)> CalculateLayout { get; set; }

        // 委托：加载纹理
        public Func<string, Texture2D> TextureLoader { get; set; }

        // 委托：路径转换
        public Func<string, string> AssetToFullPath { get; set; }

        /// <summary>
        /// 发布导出
        /// </summary>
        public void Publish()
        {
            if (Items == null || Items.Count == 0)
            {
                EditorUtility.DisplayDialog("提示", "请先添加图片", "确定");
                return;
            }

            // 读取设置
            int maxW = 1024;
            int maxH = 1024;
            int.TryParse(PopupMaxWidth?.value, out maxW);
            int.TryParse(PopupMaxHeight?.value, out maxH);
            int padding = IntBorderPadding?.value ?? 2;
            int shapePadding = IntShapePadding?.value ?? 2;

            // 计算最优排布
            var layoutResult = CalculateLayout?.Invoke(maxW, maxH, padding, shapePadding);
            if (layoutResult == null || !layoutResult.Value.success)
            {
                EditorUtility.DisplayDialog("错误", "无法排布，请检查图片数量和尺寸", "确定");
                return;
            }
            int atlasW = layoutResult.Value.width;
            int atlasH = layoutResult.Value.height;
            var positions = layoutResult.Value.positions;

            // 应用位置到 items
            ApplyPositions(positions);

            // 输出路径
            string outputPath = TxtTexturePath?.value ?? "Assets/Res/Atlas";
            string textureFilename = TxtTextureFile?.value ?? "atlas.png";
            string dataFilename = TxtDataFilename?.value ?? "atlas.txt";

            // 确保目录存在
            string fullDir = AssetToFullPath?.Invoke(outputPath);
            if (!string.IsNullOrEmpty(fullDir))
            {
                System.IO.Directory.CreateDirectory(fullDir);
            }

            // 生成 PNG
            var exporter = new AtlasExporter
            {
                TextureLoader = TextureLoader,
                AssetPathToFullPath = AssetToFullPath
            };
            var atlasTex = exporter.GenerateAtlasPng(Items, atlasW, atlasH, padding, shapePadding);

            // 保存 PNG
            string pngPath = System.IO.Path.Combine(outputPath, textureFilename);
            byte[] pngBytes = atlasTex.EncodeToPNG();
            System.IO.File.WriteAllBytes(AssetToFullPath?.Invoke(pngPath) ?? pngPath, pngBytes);

            // 生成 JSON
            string json = exporter.GenerateJson(Items, textureFilename, atlasW, atlasH, padding);
            string jsonPath = System.IO.Path.Combine(outputPath, dataFilename);
            System.IO.File.WriteAllText(AssetToFullPath?.Invoke(jsonPath) ?? jsonPath, json);

            // 刷新 AssetDatabase
            AssetDatabase.Refresh();

            // 自动设置导出的 PNG 的导入设置（避免 DXT5 不兼容错误）
            SetAtlasTextureImportSettings(pngPath);

            EditorUtility.DisplayDialog("成功", $"导出完成！\nPNG: {pngPath}\nJSON: {jsonPath}", "确定");
        }

        /// <summary>
        /// 设置导出的 Atlas PNG 的导入设置
        /// 避免 DXT5 压缩格式与 Sprite 类型不兼容的错误
        /// </summary>
        private void SetAtlasTextureImportSettings(string assetPath)
        {
            var importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;
            if (importer == null) return;

            // 设置为 Sprite 类型
            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Single;

            // 关闭压缩，使用 RGBA32
            importer.textureCompression = TextureImporterCompression.Uncompressed;

            // 开启 Read/Write（如果需要运行时读取）
            importer.isReadable = true;

            // 关闭 Mipmap
            importer.mipmapEnabled = false;

            // 应用修改
            importer.SaveAndReimport();
        }

        /// <summary>
        /// 应用排布位置到 items
        /// </summary>
        private void ApplyPositions(List<Vector2Int> positions)
        {
            for (int i = 0; i < Items.Count && i < positions.Count; i++)
            {
                Items[i].PosX = positions[i].x;
                Items[i].PosY = positions[i].y;
            }
        }
    }
}
