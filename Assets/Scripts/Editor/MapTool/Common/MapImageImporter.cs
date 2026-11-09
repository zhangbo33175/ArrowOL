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
        /// <summary>预览图路径（Assets 相对路径）</summary>
        public string PreviewPath;
    }

    /// <summary>
    /// 从 PNG 序列目录自动生成地图配置
    /// 命名规范：
    /// - bg_*.png → 背景图（只取第一个匹配）
    /// - building_*.png → 建筑物件
    /// - char_*.png → 人物物件
    /// - prop_*.png → 物件
    /// </summary>
    public static class MapImageImporter
    {
        
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
                Debug.LogWarning($"导入目录不存在: {assetDir}");
                return null;
            }

            var result = new MapImageImportResult();
            string[] pngFiles = Directory.GetFiles(fullDir, "*.png", SearchOption.TopDirectoryOnly);
            if (pngFiles.Length == 0)
            {
                Debug.LogWarning($"目录下无 PNG 文件: {assetDir}");
                return null;
            }

            foreach (string fullPath in pngFiles)
            {
                string fileName = Path.GetFileNameWithoutExtension(fullPath);
                string assetPath = MapToolPathHelper.ToAssetRelative(fullPath);

                // 按前缀分类
                if (fileName.StartsWith("bg_"))
                {
                    // 背景图（只取第一个）
                    if (string.IsNullOrEmpty(result.BackgroundPath))
                    {
                        result.BackgroundPath = assetPath;
                        result.BackgroundName = fileName.Substring(3);
                        // 读取背景图尺寸作为地图宽高
                        Texture2D bgTex = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);
                        if (bgTex != null)
                        {
                            result.MapWidth = bgTex.width;
                            result.MapHeight = bgTex.height;
                        }
                    }
                }
                else if (fileName.StartsWith("building_"))
                {
                    result.Objects.Add(new ImportedObjectInfo
                    {
                        ObjectId = fileName.Substring(9),
                        ObjectName = fileName.Substring(9),
                        Type = MapObjectType.Building,
                        PreviewPath = assetPath,
                    });
                }
                else if (fileName.StartsWith("char_"))
                {
                    result.Objects.Add(new ImportedObjectInfo
                    {
                        ObjectId = fileName.Substring(5),
                        ObjectName = fileName.Substring(5),
                        Type = MapObjectType.Character,
                        PreviewPath = assetPath,
                    });
                }
                else if (fileName.StartsWith("prop_"))
                {
                    result.Objects.Add(new ImportedObjectInfo
                    {
                        ObjectId = fileName.Substring(5),
                        ObjectName = fileName.Substring(5),
                        Type = MapObjectType.Prop,
                        PreviewPath = assetPath,
                    });
                }
                else if (fileName.StartsWith("cat_"))
                {
                    result.Objects.Add(new ImportedObjectInfo
                    {
                        ObjectId = fileName.Substring(4),
                        ObjectName = fileName.Substring(4),
                        Type = MapObjectType.Cat,
                        PreviewPath = assetPath,
                    });
                }
                else
                {
                    // 没有前缀的文件 → 默认当作背景图（只取第一个）
                    if (string.IsNullOrEmpty(result.BackgroundPath))
                    {
                        result.BackgroundPath = assetPath;
                        result.BackgroundName = fileName;
                        // 读取背景图尺寸作为地图宽高
                        Texture2D bgTex = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);
                        if (bgTex != null)
                        {
                            result.MapWidth = bgTex.width;
                            result.MapHeight = bgTex.height;
                        }
                        Debug.Log($"[MapImageImporter] 无前缀文件默认作为背景图: {fileName}");
                    }
                }
            }

            Debug.Log($"PNG 序列导入完成: 背景={result.BackgroundName}, 物件={result.Objects.Count}, 尺寸={result.MapWidth}x{result.MapHeight}");
            return result;
        }
    }
}
