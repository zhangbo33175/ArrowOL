/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  MapImageImporter.cs
 * author:    云毅
 * created:   2026
 * descrip:   从 PNG 序列目录自动生成地图配置
 *            命名规范：bg_背景图 / building_建筑 / char_人物 / prop_物件
 ***************************************************************/

using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Honor.Editor.MapTool
{
    /// <summary>
    /// PNG 序列导入结果
    /// </summary>
    public class MapImageImportResult
    {
        /// <summary>背景图路径（Assets 相对路径）</summary>
        public string BackgroundPath;
        /// <summary>背景图名（不含扩展名）</summary>
        public string BackgroundName;
        /// <summary>自动识别的物件列表</summary>
        public List<ImportedObjectInfo> Objects = new List<ImportedObjectInfo>();
        /// <summary>地图宽度（像素，取背景图尺寸）</summary>
        public int MapWidth;
        /// <summary>地图高度（像素，取背景图尺寸）</summary>
        public int MapHeight;
    }

    /// <summary>
    /// 从 PNG 序列识别出的物件信息
    /// </summary>
    public class ImportedObjectInfo
    {
        /// <summary>物件 ID（从文件名提取）</summary>
        public string ObjectId;
        /// <summary>物件名称（从文件名提取）</summary>
        public string ObjectName;
        /// <summary>物件类型</summary>
        public MapObjectType Type;
        /// <summary>八层类型键（background/greenery/road/building/prop/character/foreground/bubble，见 MapLayerCatalog）</summary>
        public string LayerType;
        /// <summary>预览图路径（Assets 相对路径）</summary>
        public string PreviewPath;
    }

    /// <summary>
    /// 从 PNG 序列目录自动生成地图配置
    /// 命名规范：
    /// - bg_*.png → 背景图（只取第一个匹配）
    /// - grass_/road_/plant_*.png → 地面装饰（草地/道路/绿植）
    /// - building_*.png → 建筑物件
    /// - char_*.png → 人物物件
    /// - prop_*.png → 物件
    /// </summary>
    public static class MapImageImporter
    {
        
        /// <summary>
        /// PNG 文件名前缀 → 导入规则（类型 / 层级 / 是否背景）
        /// 新增前缀只需在本表加一行，无需再写一段 else-if。
        /// </summary>
        private sealed class PrefixRule
        {
            /// <summary>文件名前缀（含下划线）</summary>
            public string Prefix;
            /// <summary>归类后的物件类型</summary>
            public MapObjectType Type;
            /// <summary>地图层级（由 LayerType 区分渲染层）</summary>
            public string LayerType;
            /// <summary>是否作为背景图处理（只取第一个）</summary>
            public bool IsBackground;
        }

        /// <summary>
        /// 前缀规则表（背景 → 道路 → 绿化 → 建筑 → 人物 → 物件 → 猫 → 前景 → 气泡）
        /// </summary>
        private static readonly PrefixRule[] PrefixRules =
        {
            new PrefixRule { Prefix = "bg_",       Type = MapObjectType.Prop,      LayerType = MapLayerCatalog.Background, IsBackground = true },
            new PrefixRule { Prefix = "road_",     Type = MapObjectType.Prop,      LayerType = MapLayerCatalog.Road },
            new PrefixRule { Prefix = "grass_",    Type = MapObjectType.Prop,      LayerType = MapLayerCatalog.Greenery },
            new PrefixRule { Prefix = "plant_",    Type = MapObjectType.Prop,      LayerType = MapLayerCatalog.Greenery },
            new PrefixRule { Prefix = "building_", Type = MapObjectType.Building,  LayerType = MapLayerCatalog.Building },
            new PrefixRule { Prefix = "char_",     Type = MapObjectType.Character, LayerType = MapLayerCatalog.Character },
            new PrefixRule { Prefix = "prop_",     Type = MapObjectType.Prop,      LayerType = MapLayerCatalog.Prop },
            new PrefixRule { Prefix = "cat_",      Type = MapObjectType.Cat,       LayerType = MapLayerCatalog.Character },
            new PrefixRule { Prefix = "eff_",      Type = MapObjectType.Prop,      LayerType = MapLayerCatalog.Foreground },
            new PrefixRule { Prefix = "fg_",       Type = MapObjectType.Prop,      LayerType = MapLayerCatalog.Foreground },
            new PrefixRule { Prefix = "bubble_",   Type = MapObjectType.Prop,      LayerType = MapLayerCatalog.Bubble },
        };

        /// <summary>
        /// 从指定目录导入 PNG 序列
        /// </summary>
        /// <param name="assetDir">Assets 相对目录（如 Assets/Res/Textures/Map/ImportTemp/Map001）</param>
        /// <returns>导入结果；目录不存在或无有效文件返回 null</returns>
        public static MapImageImportResult ImportFromDirectory(string assetDir)
        {
            if (string.IsNullOrEmpty(assetDir)) return null;
            string fullDir = MapToolPathHelper.ToAbsolutePath(assetDir);
            if (!Directory.Exists(fullDir))
            {
                MapToolLog.Warn($"导入目录不存在: {assetDir}");
                return null;
            }

            string[] pngFiles = Directory.GetFiles(fullDir, "*.png", SearchOption.TopDirectoryOnly);
            if (pngFiles.Length == 0)
            {
                MapToolLog.Warn($"目录下无 PNG 文件: {assetDir}");
                return null;
            }

            var result = new MapImageImportResult();
            foreach (string fullPath in pngFiles)
            {
                ImportSingleFile(result, fullPath);
            }

            MapToolLog.Info($"PNG 序列导入完成: 背景={result.BackgroundName}, 物件={result.Objects.Count}, 尺寸={result.MapWidth}x{result.MapHeight}");
            return result;
        }

        /// <summary>
        /// 按文件名前缀分发导入一个 PNG（无前缀时默认作为背景图）
        /// </summary>
        /// <param name="result">导入结果累加器</param>
        /// <param name="fullPath">PNG 磁盘全路径</param>
        private static void ImportSingleFile(MapImageImportResult result, string fullPath)
        {
            string fileName = Path.GetFileNameWithoutExtension(fullPath);
            string assetPath = MapToolPathHelper.ToAssetRelative(fullPath);

            PrefixRule rule = FindPrefixRule(fileName);
            if (rule != null)
            {
                string objectName = fileName.Substring(rule.Prefix.Length);
                if (rule.IsBackground)
                {
                    SetBackgroundIfEmpty(result, assetPath, objectName);
                }
                else
                {
                    result.Objects.Add(CreateObjectInfo(objectName, rule.Type, rule.LayerType, assetPath));
                }
                return;
            }

            // 没有前缀的文件 → 默认当作背景图（只取第一个）
            if (SetBackgroundIfEmpty(result, assetPath, fileName))
            {
                MapToolLog.Info($"[MapImageImporter] 无前缀文件默认作为背景图: {fileName}");
            }
        }

        /// <summary>
        /// 匹配文件名前缀规则（未命中返回 null）
        /// </summary>
        /// <param name="fileName">文件名（不含扩展名）</param>
        /// <returns>命中的规则</returns>
        private static PrefixRule FindPrefixRule(string fileName)
        {
            foreach (var rule in PrefixRules)
            {
                if (fileName.StartsWith(rule.Prefix)) return rule;
            }
            return null;
        }

        /// <summary>
        /// 尚未设置背景时写入背景路径/名称，并读取纹理尺寸作为地图宽高
        /// </summary>
        /// <param name="result">导入结果累加器</param>
        /// <param name="assetPath">背景图资产路径</param>
        /// <param name="backgroundName">背景名称</param>
        /// <returns>实际写入返回 true</returns>
        private static bool SetBackgroundIfEmpty(MapImageImportResult result, string assetPath, string backgroundName)
        {
            if (!string.IsNullOrEmpty(result.BackgroundPath)) return false;

            result.BackgroundPath = assetPath;
            result.BackgroundName = backgroundName;

            // 读取背景图尺寸作为地图宽高
            Texture2D bgTex = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);
            if (bgTex != null)
            {
                result.MapWidth = bgTex.width;
                result.MapHeight = bgTex.height;
            }
            return true;
        }

        /// <summary>
        /// 构建单个导入物件信息
        /// </summary>
        /// <param name="objectName">去掉前缀后的物件名</param>
        /// <param name="type">物件类型</param>
        /// <param name="layerType">地图层级</param>
        /// <param name="assetPath">预览图资产路径</param>
        /// <returns>导入物件信息</returns>
        private static ImportedObjectInfo CreateObjectInfo(string objectName, MapObjectType type, string layerType, string assetPath)
        {
            return new ImportedObjectInfo
            {
                ObjectId = objectName,
                ObjectName = objectName,
                Type = type,
                LayerType = layerType,
                PreviewPath = assetPath,
            };
        }
    }
}
