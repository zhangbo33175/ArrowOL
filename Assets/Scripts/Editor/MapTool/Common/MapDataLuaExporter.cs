/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  MapDataLuaExporter.cs
 * author:    云毅
 * created:   2026
 * descrip:   地图 Lua 数据导出器（单关卡模式，纯字符串格式化 + System.IO 写盘）
 ***************************************************************/

using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Editor.MapEditor;
using UnityEngine;

namespace Honor.Editor.MapTool
{
    /// <summary>
    /// 章节导出上下文（Lua 导出输入，由 SceneEditModule.SaveMapConfig 组装）
    /// </summary>
    public class MapChapterExportContext
    {
        /// <summary>章节ID（字符串，如 "3"）</summary>
        public string ChapterId = string.Empty;

        /// <summary>章节名称（TableChapterEditor.ChapterTitle，空则 sceneName）</summary>
        public string ChapterName = string.Empty;

        /// <summary>地图名（TableChapterEditor.MapName，空则 sceneName）</summary>
        public string MapName = string.Empty;

        /// <summary>当前关卡ID</summary>
        public string LevelId = string.Empty;

        /// <summary>LevelId 基准：本章节之前各章节关卡数之和</summary>
        public int LevelIdBase = 0;
    }

    /// <summary>
    /// 地图数据 Lua 导出器（单关卡模式）
    /// 每个关卡一个 Lua 文件 + 重写主表索引
    /// 纯字符串格式化 + System.IO 写盘（UTF8 无 BOM）
    /// </summary>
    public static class MapDataLuaExporter
    {
        #region 常量与路径

        /// <summary>Lua 关卡数据表根目录（Assets 相对路径）</summary>
        private const string k_LevelsRoot = "Assets/LuaScripts/Game/Tables/Levels";

        /// <summary>主表索引文件名（固定，每次保存整体重写）</summary>
        private const string k_MainTableName = "TableLevelData.lua.txt";

        #endregion

        #region 对外入口

        /// <summary>
        /// 导出关卡 Lua 数据：单关卡文件 + 重写主表索引
        /// </summary>
        /// <param name="data">已组装好的 JSON 配置</param>
        /// <param name="ctx">章节上下文（章节ID/名称/地图名/关卡ID/LevelIdBase）</param>
        /// <param name="allChapters">配置表全部章节列表（主表索引覆盖全部章节）</param>
        /// <param name="levelsRootOverride">可选：Lua Levels 根目录绝对路径覆盖</param>
        /// <returns>本次生成的 Lua 文件绝对路径列表（含主表索引）</returns>
        public static List<string> Export(MapConfigData data, MapChapterExportContext ctx, List<TableChapterEditor> allChapters, string levelsRootOverride = null)
        {
            List<string> generatedFiles = new List<string>();
            if (data == null || ctx == null)
                return generatedFiles;

            // 输出根目录
            string levelsRoot;
            if (!string.IsNullOrEmpty(levelsRootOverride))
            {
                levelsRoot = levelsRootOverride.Replace("/", Path.DirectorySeparatorChar.ToString());
            }
            else
            {
                string projectRoot = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));
                levelsRoot = Path.Combine(projectRoot, k_LevelsRoot.Replace("/", Path.DirectorySeparatorChar.ToString()));
            }
            string timeNow = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            string chapterId = ctx.ChapterId ?? string.Empty;
            string chapterName = ctx.ChapterName ?? string.Empty;
            string mapName = ctx.MapName ?? string.Empty;
            string levelIdStr = ctx.LevelId ?? "1";

            // 单关卡模式：直接从 data 读取
            int levelId = ctx.LevelIdBase + int.Parse(levelIdStr);
            string fileName = string.Format(CultureInfo.InvariantCulture, "TableLevelData_{0}_{1}", chapterId, levelIdStr);

            string dir = Path.Combine(levelsRoot, chapterId);
            Directory.CreateDirectory(dir);
            string filePath = Path.Combine(dir, fileName + ".lua.txt");

            string content = BuildLevelFile(fileName, chapterId, chapterName, levelIdStr,
                levelId, data, mapName, data.width, data.height, timeNow);
            File.WriteAllText(filePath, content, new UTF8Encoding(false));
            generatedFiles.Add(filePath);

            // 重写主表索引（覆盖全部章节）
            string mainTablePath = Path.Combine(levelsRoot, k_MainTableName);
            Directory.CreateDirectory(levelsRoot);
            File.WriteAllText(mainTablePath, BuildMainTable(allChapters), new UTF8Encoding(false));
            generatedFiles.Add(mainTablePath);

            return generatedFiles;
        }

        #endregion

        #region 关卡文件生成

        /// <summary>
        /// 组装单个关卡 Lua 文件文本
        /// </summary>
        private static string BuildLevelFile(string fileName, string chapterId, string chapterName,
            string levelIdStr, int levelId, MapConfigData data,
            string mapName, int width, int height, string time)
        {
            StringBuilder sb = new StringBuilder();

            // ===== 文件头 =====
            sb.AppendLine("--=====================================================================================================");
            sb.AppendLine("-- (c) copyright 2026 - 2030, Honor.Runtime");
            sb.AppendLine("-- All Rights Reserved.");
            sb.AppendLine("-- ----------------------------------------------------------------------------------------------------");
            sb.AppendLine("-- filename:  " + fileName);
            sb.AppendLine("-- descrip:   章节" + chapterId + "关卡" + levelIdStr + "地图数据（自动生成）");
            sb.AppendLine("-- notices:   该文件由地图编辑器自动生成，禁止手动修改！");
            sb.AppendLine("-- @brief     关卡地图数据表");
            sb.AppendLine("-- @desc      由 Honor.Editor.MapTool 保存时自动生成");
            sb.AppendLine("-- @core      关卡信息（背景/物件/人物含路径点/建筑）");
            sb.AppendLine("-- @dependency Tables 注册表（Tables.lua）");
            sb.AppendLine("--=====================================================================================================");
            sb.AppendLine();

            // ===== 表体 =====
            sb.AppendLine("---@type Tables.LevelData_Item");
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "Tables.LevelData_{0}_{1} = {{", chapterId, levelIdStr));
            sb.AppendLine("    ChapterId = \"" + Escape(chapterId) + "\",");
            sb.AppendLine("    ChapterName = \"" + Escape(chapterName) + "\",");
            sb.AppendLine("    LevelId = \"" + Escape(levelIdStr) + "\",");
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "    LevelIndex = {0},", levelId));
            sb.AppendLine("    BgName = \"" + Escape(data.bgName) + "\",");
            sb.AppendLine("    MapName = \"" + Escape(mapName) + "\",");
            sb.AppendLine("    SavePath = \"Assets/GameData/MapDatas/Map_" + Escape(chapterId) + "_" + Escape(levelIdStr) + ".json\",");
            sb.AppendLine("    Time = \"" + Escape(time) + "\",");
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "    Width = {0},", width));
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "    Height = {0},", height));

            // 网格信息
            int gridSize = MapToolConstants.GridSize;
            int gridCols = width > 0 ? width / gridSize : 0;
            int gridRows = height > 0 ? height / gridSize : 0;
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "    GridSize = {0},", gridSize));
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "    GridCols = {0},", gridCols));
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "    GridRows = {0},", gridRows));
            sb.AppendLine("    OccupiedCells = {");
            AppendOccupiedCells(sb, data);
            sb.AppendLine("    },");

            // Characters：人物（含 PathPoints）
            sb.AppendLine("    Characters = {");
            AppendCharacterEntries(sb, data.characters);
            sb.AppendLine("    },");

            // Buildings：建筑
            sb.AppendLine("    Buildings = {");
            AppendObjectEntries(sb, data.buildings);
            sb.AppendLine("    },");

            // Objects：物件
            sb.AppendLine("    Objects = {");
            AppendObjectEntries(sb, data.objects);
            sb.AppendLine("    },");

            sb.AppendLine("}");
            return sb.ToString();
        }

        /// <summary>
        /// 追加 Characters 条目（人物全字段，含 PathPoints）
        /// </summary>
        private static void AppendCharacterEntries(StringBuilder sb, List<MapCharacterData> characters)
        {
            if (characters == null || characters.Count == 0)
            {
                sb.AppendLine("    -- 空列表");
                return;
            }
            for (int i = 0; i < characters.Count; i++)
            {
                MapCharacterData c = characters[i];
                sb.AppendLine(string.Format(CultureInfo.InvariantCulture,
                    "        [{0}] = {{\n" +
                    "            InstanceId = \"{1}\",\n" +
                    "            Id = {2},\n" +
                    "            TemplateId = \"{3}\",\n" +
                    "            Name = \"{4}\",\n" +
                    "            GridX = {5},\n" +
                    "            GridY = {6},\n" +
                    "            Width = {7},\n" +
                    "            Height = {8},\n" +
                    "            Image = \"{9}\",\n" +
                    "            Anim = \"{10}\",\n" +
                    "            Loop = {11},\n" +
                    "            Speed = {12},\n" +
                    "            Interactable = {13},\n" +
                    "            InteractType = \"{14}\",\n" +
                    "            PatrolLoop = {15},\n" +
                    "            FacingDirection = \"{16}\",\n" +
                    "            DialogBubble = \"{17}\",\n" +
                    "            SortOrder = {18},\n" +
                    "            ShadowEnabled = {19},\n" +
                    "            PathPoints = {{\n{20}            }},\n" +
                    "        }},",
                    i + 1,
                    Escape(c.id),
                    IdToLua(c.templateId, c.id),
                    Escape(c.templateId),
                    Escape(c.name),
                    c.gridX,
                    c.gridY,
                    c.width,
                    c.height,
                    Escape(c.image),
                    Escape(c.anim),
                    BoolToLua(c.loop),
                    c.speed.ToString("F2", CultureInfo.InvariantCulture),
                    BoolToLua(c.interactable),
                    Escape(c.interactType),
                    "\"" + Escape(c.walkMode) + "\"",
                    Escape(c.facingDirection),
                    Escape(c.dialogBubble),
                    c.sortOrder,
                    BoolToLua(c.shadowEnabled),
                    BuildPathPointsBlock(c.pathPoints)));
            }
        }

        /// <summary>
        /// 追加 Buildings / Objects 条目
        /// </summary>
        private static void AppendObjectEntries(StringBuilder sb, List<MapObjectData> objects)
        {
            if (objects == null || objects.Count == 0)
            {
                sb.AppendLine("    -- 空列表");
                return;
            }
            for (int i = 0; i < objects.Count; i++)
            {
                MapObjectData o = objects[i];
                string bubbleLine = o.bubbleId > 0
                    ? string.Format(CultureInfo.InvariantCulture, "            BubbleId = {0},\n", o.bubbleId)
                    : string.Empty;
                sb.AppendLine(string.Format(CultureInfo.InvariantCulture,
                    "        [{0}] = {{\n" +
                    "            InstanceId = \"{1}\",\n" +
                    "            Id = {2},\n" +
                    "            TemplateId = \"{3}\",\n" +
                    "            Name = \"{4}\",\n" +
                    "            GridX = {5},\n" +
                    "            GridY = {6},\n" +
                    "            Width = {7},\n" +
                    "            Height = {8},\n" +
                    "            Image = \"{9}\",\n" +
                    "            Anim = \"{10}\",\n" +
                    "            Loop = {11},\n" +
                    "            Speed = {12},\n" +
                    "            Interactable = {13},\n" +
                    "            InteractType = \"{14}\",\n" +
                    "            SortOrder = {15},\n" +
                    "            CollisionEnabled = {16},\n" +
                    "            DialogBubble = \"{17}\",\n" +
                    "            ShadowEnabled = {18},\n" +
                    "            IsCollectible = {19},\n" +
                    "            RespawnTime = {20},\n" +
                    "{21}" +
                    "        }},",
                    i + 1,
                    Escape(o.id),
                    IdToLua(o.templateId, o.id),
                    Escape(o.templateId),
                    Escape(o.name),
                    o.gridX,
                    o.gridY,
                    o.width,
                    o.height,
                    Escape(o.image),
                    Escape(o.anim),
                    BoolToLua(o.loop),
                    o.speed.ToString("F2", CultureInfo.InvariantCulture),
                    BoolToLua(o.interactable),
                    Escape(o.interactType),
                    o.sortOrder,
                    BoolToLua(o.collisionEnabled),
                    Escape(o.dialogBubble),
                    BoolToLua(o.shadowEnabled),
                    BoolToLua(o.isCollectible),
                    o.respawnTime.ToString("F2", CultureInfo.InvariantCulture),
                    bubbleLine));
            }
        }

        /// <summary>
        /// 组装 PathPoints 块（只保存格子坐标）
        /// </summary>
        private static string BuildPathPointsBlock(List<MapPathPointData> pathPoints)
        {
            if (pathPoints == null || pathPoints.Count == 0)
                return string.Empty;
            StringBuilder psb = new StringBuilder();
            foreach (var p in pathPoints)
            {
                int gx = Mathf.FloorToInt(p.x / MapToolConstants.GridSize);
                int gy = Mathf.FloorToInt(p.y / MapToolConstants.GridSize);
                psb.AppendLine(string.Format(CultureInfo.InvariantCulture,
                    "                {{ GridX = {0}, GridY = {1} }},",
                    gx,
                    gy));
            }
            return psb.ToString();
        }

        /// <summary>
        /// 追加占用格子块（从人物/建筑/物件推导覆盖格子，行列去重）
        /// </summary>
        private static void AppendOccupiedCells(StringBuilder sb, MapConfigData data)
        {
            var set = new HashSet<long>();
            if (data != null)
            {
                CollectOccupiedCharacters(set, data.characters);
                CollectOccupiedObjects(set, data.buildings);
                CollectOccupiedObjects(set, data.objects);
            }
            if (set.Count == 0)
            {
                sb.AppendLine("        -- 无占用");
                return;
            }
            int index = 0;
            foreach (var key in set)
            {
                index++;
                int gx = (int)(key % 100000);
                int gy = (int)(key / 100000);
                sb.AppendLine(string.Format(CultureInfo.InvariantCulture,
                    "        [{0}] = {{ X = {1}, Y = {2} }},", index, gx, gy));
            }
        }

        /// <summary>
        /// 收集人物占用格子
        /// </summary>
        private static void CollectOccupiedCharacters(HashSet<long> set, List<MapCharacterData> characters)
        {
            if (characters == null) return;
            foreach (var c in characters)
            {
                if (c == null) continue;
                int gx = (int)(c.posX / MapToolConstants.GridSize);
                int gy = (int)(c.posY / MapToolConstants.GridSize);
                AddSpan(set, gx, gy, c.width, c.height);
            }
        }

        /// <summary>
        /// 收集建筑 / 物件占用格子
        /// </summary>
        private static void CollectOccupiedObjects(HashSet<long> set, List<MapObjectData> objects)
        {
            if (objects == null) return;
            foreach (var o in objects)
            {
                if (o == null) continue;
                int gx = (int)(o.posX / MapToolConstants.GridSize);
                int gy = (int)(o.posY / MapToolConstants.GridSize);
                AddSpan(set, gx, gy, o.width, o.height);
            }
        }

        /// <summary>
        /// 将物体覆盖的格子范围写入集合（行列打包去重）
        /// </summary>
        private static void AddSpan(HashSet<long> set, int gx, int gy, int width, int height)
        {
            int spanX = Mathf.Max(1, width);
            int spanY = Mathf.Max(1, height);
            for (int dy = 0; dy < spanY; dy++)
            {
                for (int dx = 0; dx < spanX; dx++)
                {
                    set.Add(((long)(gy + dy)) * 100000 + (gx + dx));
                }
            }
        }

        #endregion

        #region 主表索引生成

        /// <summary>
        /// 重写主表索引（覆盖配置表全部章节，每次保存整体重写）
        /// </summary>
        private static string BuildMainTable(List<TableChapterEditor> allChapters)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("--=====================================================================================================");
            sb.AppendLine("-- (c) copyright 2026 - 2030, Honor.Runtime");
            sb.AppendLine("-- All Rights Reserved.");
            sb.AppendLine("-- ----------------------------------------------------------------------------------------------------");
            sb.AppendLine("-- filename:  TableLevelData");
            sb.AppendLine("-- descrip:   章节地图索引（自动生成）");
            sb.AppendLine("-- notices:   该文件由地图编辑器自动生成，禁止手动修改！");
            sb.AppendLine("-- @brief     章节地图数据索引");
            sb.AppendLine("-- @dependency Tables 注册表（Tables.lua）");
            sb.AppendLine("--=====================================================================================================");
            sb.AppendLine();
            sb.AppendLine("---@class Tables.LevelData @章节地图数据索引");
            sb.AppendLine("Tables.LevelData = {");
            if (allChapters != null)
            {
                foreach (var chapter in allChapters)
                {
                    if (chapter == null) continue;
                    string[] levelIds = MapToolEditorUtils.ParseRegionIds(chapter.ChapterInfoID);
                    string levels = "{ " + string.Join(", ", ToQuotedArray(levelIds)) + " }";
                    sb.AppendLine(string.Format(CultureInfo.InvariantCulture,
                        "    [\"{0}\"] = {{ ChapterId = \"{0}\", ChapterName = \"{1}\", LevelCount = {2}, Levels = {3} }},",
                        Escape(chapter.ID),
                        Escape(chapter.ChapterTitle),
                        levelIds.Length,
                        levels));
                }
            }
            sb.AppendLine("}");
            return sb.ToString();
        }

        #endregion

        #region 格式辅助

        /// <summary>
        /// Id 输出规则：纯数字模板ID输出裸数字；否则输出带引号字符串
        /// </summary>
        private static string IdToLua(string templateId, string fallbackInstanceId)
        {
            if (!string.IsNullOrEmpty(templateId))
            {
                string trimmed = templateId.Trim();
                if (IsAllDigits(trimmed))
                    return trimmed;
                return "\"" + Escape(templateId) + "\"";
            }
            return "\"" + Escape(string.IsNullOrEmpty(fallbackInstanceId) ? string.Empty : fallbackInstanceId) + "\"";
        }

        /// <summary>
        /// 判断字符串是否为纯数字
        /// </summary>
        private static bool IsAllDigits(string s)
        {
            if (string.IsNullOrEmpty(s)) return false;
            foreach (char c in s)
            {
                if (c < '0' || c > '9') return false;
            }
            return true;
        }

        /// <summary>
        /// 布尔转 Lua 小写
        /// </summary>
        private static string BoolToLua(bool v)
        {
            return v ? "true" : "false";
        }

        /// <summary>
        /// 字符串转义：\ 与 " 转义，控制字符剔除；中文原样输出
        /// </summary>
        private static string Escape(string s)
        {
            if (string.IsNullOrEmpty(s)) return string.Empty;
            StringBuilder esb = new StringBuilder(s.Length);
            foreach (char c in s)
            {
                if (char.IsControl(c)) continue;
                if (c == '\\') { esb.Append("\\\\"); continue; }
                if (c == '"') { esb.Append("\\\""); continue; }
                esb.Append(c);
            }
            return esb.ToString();
        }

        /// <summary>
        /// 字符串数组批量加引号
        /// </summary>
        private static string[] ToQuotedArray(string[] ids)
        {
            if (ids == null) return new string[0];
            string[] result = new string[ids.Length];
            for (int i = 0; i < ids.Length; i++)
                result[i] = "\"" + Escape(ids[i]) + "\"";
            return result;
        }

        #endregion
    }
}
