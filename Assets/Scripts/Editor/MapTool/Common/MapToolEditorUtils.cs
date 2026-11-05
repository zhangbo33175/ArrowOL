/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  MapToolEditorUtils.cs
 * author:    云毅
 * created:   2026
 * descrip:   地图编辑器公共工具（配置表加载/分区ID解析/物体预览图与图片路径解析/类型格式化）
 ***************************************************************/

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using Editor.MapEditor;

namespace Honor.Editor.MapTool
{
    /// <summary>
    /// 地图编辑器公共工具
    /// </summary>
    public static class MapToolEditorUtils
    {
        /// <summary>
        /// 加载配置表数据（路径相对 Application.dataPath）
        /// </summary>
        /// <typeparam name="T">配置行数据类型（引用类型）</typeparam>
        /// <param name="tableRelativePath">配置表相对路径</param>
        /// <returns>配置数据列表（失败返回空列表）</returns>
        public static List<T> LoadTableData<T>(string tableRelativePath) where T : class, new()
        {
            try
            {
                string absolutePath = System.IO.Path.Combine(Application.dataPath, tableRelativePath);
                Debug.Log($"加载配置表: {absolutePath}");
                return TableExportEditorUtility.GetExcelData(absolutePath).ToList<T>();
            }
            catch (System.Exception e)
            {
                Debug.LogError($"加载配置表失败 {tableRelativePath}: {e.Message}");
                return new List<T>();
            }
        }

        /// <summary>
        /// 解析分区ID配置
        /// 格式如 {"1","2","3"}，有几个元素就有几个分区
        /// 【取消分区】章节=整图单分区：配置为空时默认单分区 {"1"}（不再默认 9 分区）
        /// </summary>
        /// <param name="chapterInfoId">配置字符串（空则默认单分区）</param>
        /// <returns>分区ID数组</returns>
        public static string[] ParseRegionIds(string chapterInfoId)
        {
            if (string.IsNullOrEmpty(chapterInfoId))
            {
                return new[] { "1" };
            }

            // 去掉大括号和引号，按逗号分割并去空白
            string cleaned = chapterInfoId.Replace("{", "").Replace("}", "").Replace("\"", "");
            string[] ids = cleaned.Split(',');
            for (int i = 0; i < ids.Length; i++)
            {
                ids[i] = ids[i].Trim();
            }
            return ids;
        }

        /// <summary>
        /// 从运行时 Lua 章节表加载章节（Excel 配置缺失时的兜底数据源）
        /// 解析 Assets/LuaScripts/Game/Tables/AlTables/LevelConfigs/TableChapters.lua.txt，
        /// 与游戏运行时同源；表结构为每章一行：["N"] = {ID="N", ChapterTitle=..., ChapterInfoID={"N"}, ...}
        /// </summary>
        /// <returns>章节配置列表（文件缺失/解析失败返回空列表）</returns>
        public static List<TableChapterEditor> LoadChaptersFromRuntimeLua()
        {
            var result = new List<TableChapterEditor>();
            string luaPath = Path.Combine(Application.dataPath,"LuaScripts/Game/Tables/AlTables/LevelConfigs/TableChapters.lua.txt");
            if (!File.Exists(luaPath))
                return result;
            try
            {
                foreach (var line in File.ReadAllLines(luaPath, Encoding.UTF8))
                {
                    var entry = Regex.Match(line, @"\[""(?<id>[^""]+)""\]\s*=\s*\{");
                    if (!entry.Success)
                        continue;
                    string id = entry.Groups["id"].Value;
                    string title = Regex.Match(line, @"ChapterTitle\s*=\s*""(?<v>[^""]*)""").Groups["v"].Value;
                    string mapName = Regex.Match(line, @"MapName\s*=\s*""(?<v>[^""]*)""").Groups["v"].Value;
                    string infoBody = Regex.Match(line, @"ChapterInfoID\s*=\s*\{(?<v>[^}]*)\}").Groups["v"].Value;
                    var ids = Regex.Matches(infoBody, @"""(?<v>[^""]*)""")
                        .Cast<Match>()
                        .Select(x => x.Groups["v"].Value)
                        .ToList();
                    string chapterInfoId = ids.Count > 0
                        ? "{" + string.Join(",", ids.Select(x => "\"" + x + "\"")) + "}"
                        : string.Empty;
                    result.Add(new TableChapterEditor
                    {
                        ID = id,
                        ChapterTitle = title,
                        ChapterInfoID = chapterInfoId,
                        MapName = mapName,
                    });
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"加载运行时章节表失败: {e.Message}");
            }
            return result;
        }

        /// <summary>
        /// 加载物体预览图（依次尝试两个候选资源目录）
        /// </summary>
        /// <param name="objectId">物体ID</param>
        /// <returns>预览图，未找到返回 null</returns>
        public static Texture LoadObjectPreview(string objectId)
        {
            Texture tex = AssetDatabase.LoadAssetAtPath<Texture2D>(
                $"{MapToolConstants.ObjectPreviewPrefabsDir}/{objectId}.png");
            if (tex != null) return tex;

            tex = AssetDatabase.LoadAssetAtPath<Texture2D>(
                $"{MapToolConstants.ObjectPreviewTexturesDir}/{objectId}.png");
            if (tex != null) return tex;

            return null;
        }

        /// <summary>
        /// 获取物体图片资源路径
        /// 优先取预览图的真实资源路径，否则按 ID 推断默认路径
        /// </summary>
        /// <param name="previewTexture">物体预览图</param>
        /// <param name="objectId">物体ID</param>
        /// <returns>图片资源路径</returns>
        public static string GetObjectImagePath(Texture previewTexture, string objectId)
        {
            if (previewTexture != null)
            {
                string path = AssetDatabase.GetAssetPath(previewTexture);
                if (!string.IsNullOrEmpty(path))
                    return path;
            }
            return $"{MapToolConstants.ObjectPreviewTexturesDir}/{objectId}.png";
        }

        /// <summary>
        /// 获取物体类型的中文名称
        /// </summary>
        /// <param name="type">物体类型</param>
        /// <returns>中文类型名</returns>
        public static string FormatObjectTypeName(MapObjectType type)
        {
            switch (type)
            {
                case MapObjectType.Building: return "建筑";
                case MapObjectType.Character: return "人物";
                case MapObjectType.Prop: return "物件";
                default: return "未知";
            }
        }

        /// <summary>
        /// 获取物体类型的默认尺寸（格子数）
        /// </summary>
        /// <param name="type">物体类型</param>
        /// <returns>宽 x 高</returns>
        public static Vector2Int GetDefaultObjectSize(MapObjectType type)
        {
            switch (type)
            {
                case MapObjectType.Building: return MapToolConstants.BuildingDefaultSize;
                case MapObjectType.Character: return MapToolConstants.CharacterDefaultSize;
                case MapObjectType.Prop: return MapToolConstants.PropDefaultSize;
                default: return Vector2Int.one;
            }
        }
    }
}