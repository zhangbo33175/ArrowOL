/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  MapDataLuaExporter.cs
 * author:    云毅
 * created:   2026
 * descrip:   地图 Lua 数据导出器（拆分优化版）
 *            主文件 + 角色/建筑/物件/路径 4 个子文件，按需加载
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
    /// 地图数据 Lua 导出器（拆分优化版）
    /// 每个关卡生成 5 个文件：主文件 + 角色/建筑/物件/路径子文件
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
        /// 导出关卡 Lua 数据：主文件 + 4 个子文件 + 重写主表索引
        /// </summary>
        /// <param name="data">已组装好的 JSON 配置</param>
        /// <param name="ctx">章节上下文（章节ID/名称/地图名/关卡ID/LevelIdBase）</param>
        /// <param name="allChapters">配置表全部章节列表（主表索引覆盖全部章节）</param>
        /// <param name="levelsRootOverride">可选：Lua Levels 根目录绝对路径覆盖</param>
        /// <returns>本次生成的 Lua 文件绝对路径列表</returns>
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
            string baseFileName = string.Format(CultureInfo.InvariantCulture, "TableLevelData_{0}_{1}", chapterId, levelIdStr);

            string dir = Path.Combine(levelsRoot, chapterId);
            Directory.CreateDirectory(dir);

            // ========== 1. 主文件 ==========
            string mainPath = Path.Combine(dir, baseFileName + ".lua.txt");
            string mainContent = BuildMainFile(baseFileName, chapterId, chapterName, levelIdStr,
                levelId, data, mapName, data.width, data.height, timeNow);
            File.WriteAllText(mainPath, mainContent, new UTF8Encoding(false));
            generatedFiles.Add(mainPath);

            // ========== 2. 角色子文件（和主文件同目录，按章节/关卡组织） ==========
            string charPath = Path.Combine(dir, baseFileName + "_Characters.lua.txt");
            string charContent = BuildCharactersFile(baseFileName, data.characters);
            File.WriteAllText(charPath, charContent, new UTF8Encoding(false));
            generatedFiles.Add(charPath);

            // ========== 3. 建筑子文件 ==========
            string buildingPath = Path.Combine(dir, baseFileName + "_Buildings.lua.txt");
            string buildingContent = BuildBuildingsFile(baseFileName, data.buildings);
            File.WriteAllText(buildingPath, buildingContent, new UTF8Encoding(false));
            generatedFiles.Add(buildingPath);

            // ========== 4. 物件子文件 ==========
            string objPath = Path.Combine(dir, baseFileName + "_Objects.lua.txt");
            string objContent = BuildObjectsFile(baseFileName, data.objects);
            File.WriteAllText(objPath, objContent, new UTF8Encoding(false));
            generatedFiles.Add(objPath);

            // ========== 5. 路径子文件 ==========
            string pathPath = Path.Combine(dir, baseFileName + "_Paths.lua.txt");
            string pathContent = BuildPathsFile(baseFileName, data.characters);
            File.WriteAllText(pathPath, pathContent, new UTF8Encoding(false));
            generatedFiles.Add(pathPath);

            // 重写主表索引（覆盖全部章节）
            string mainTablePath = Path.Combine(levelsRoot, k_MainTableName);
            Directory.CreateDirectory(levelsRoot);
            File.WriteAllText(mainTablePath, BuildMainTable(allChapters), new UTF8Encoding(false));
            generatedFiles.Add(mainTablePath);

            return generatedFiles;
        }

        #endregion

        #region 1. 主文件生成

        /// <summary>
        /// 组装主文件（Meta + Defaults + Counts + SubTables）
        /// </summary>
        private static string BuildMainFile(string fileName, string chapterId, string chapterName,
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
            sb.AppendLine("-- descrip:   章节" + chapterId + "关卡" + levelIdStr + "地图数据（自动生成·拆分优化版）");
            sb.AppendLine("-- notices:   该文件由地图编辑器自动生成，禁止手动修改！");
            sb.AppendLine("-- @brief     关卡地图数据表（主入口）");
            sb.AppendLine("-- @desc      拆分优化：主文件只存 Meta/Defaults/Counts/SubTables，实体按需加载");
            sb.AppendLine("-- @dependency Tables 注册表（Tables.lua）");
            sb.AppendLine("--=====================================================================================================");
            sb.AppendLine();

            // ===== 表体 =====
            sb.AppendLine("---@type Tables.LevelData_Item");
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "Tables.LevelData_{0}_{1} = {{", chapterId, levelIdStr));

            // --- Meta 块 ---
            sb.AppendLine("    -- ==========================================================================");
            sb.AppendLine("    -- 1. 关卡元数据");
            sb.AppendLine("    -- ==========================================================================");
            sb.AppendLine("    Meta = {");
            sb.AppendLine("        ChapterId     = \"" + Escape(chapterId) + "\",");
            sb.AppendLine("        ChapterName   = \"" + Escape(chapterName) + "\",");
            sb.AppendLine("        LevelId       = \"" + Escape(levelIdStr) + "\",");
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "        LevelIndex    = {0},", levelId));
            sb.AppendLine("        MapName       = \"" + Escape(mapName) + "\",");
            sb.AppendLine("        BgName        = \"" + Escape(data.bgName) + "\",");
            sb.AppendLine("        SavePath      = \"Assets/GameData/MapDatas/Map_" + Escape(chapterId) + "_" + Escape(levelIdStr) + ".json\",");
            sb.AppendLine("        Time          = \"" + Escape(time) + "\",");
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "        Width         = {0},", width));
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "        Height        = {0},", height));

            int gridSize = MapToolConstants.GridSize;
            int gridCols = width > 0 ? width / gridSize : 0;
            int gridRows = height > 0 ? height / gridSize : 0;
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "        GridSize      = {0},", gridSize));
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "        GridCols      = {0},", gridCols));
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "        GridRows      = {0},", gridRows));
            sb.AppendLine("    },");
            sb.AppendLine();

            // --- Defaults 块 ---
            sb.AppendLine("    -- ==========================================================================");
            sb.AppendLine("    -- 2. 全局默认配置（所有实体共享，实体自身字段优先）");
            sb.AppendLine("    -- ==========================================================================");
            sb.AppendLine("    Defaults = {");
            sb.AppendLine("        SortOrder        = 50,");
            sb.AppendLine("        ShadowEnabled    = true,");
            sb.AppendLine("        YSortEnabled     = true,");
            sb.AppendLine("        YSortOffset      = 0.0,");
            sb.AppendLine("        Loop             = true,");
            sb.AppendLine("        Speed            = 1.0,");
            sb.AppendLine("        FacingDirection  = \"down\",");
            sb.AppendLine("        HideWhenOffMap   = true,");
            sb.AppendLine("        LoopRestartDelay = 3.0,");
            sb.AppendLine("    },");
            sb.AppendLine();

            // --- Counts 块 ---
            sb.AppendLine("    -- ==========================================================================");
            sb.AppendLine("    -- 3. 实体数量统计");
            sb.AppendLine("    -- ==========================================================================");
            sb.AppendLine("    Counts = {");
            int charCount = data.characters != null ? data.characters.Count : 0;
            int buildingCount = data.buildings != null ? data.buildings.Count : 0;
            int objCount = data.objects != null ? data.objects.Count : 0;
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "        Characters = {0},", charCount));
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "        Buildings  = {0},", buildingCount));
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "        Objects    = {0},", objCount));
            sb.AppendLine("    },");
            sb.AppendLine();

            // --- SubTables 块 ---
            sb.AppendLine("    -- ==========================================================================");
            sb.AppendLine("    -- 4. 子表索引（运行时按需 require 加载）");
            sb.AppendLine("    -- ==========================================================================");
            // XLua 自定义加载器自动转 PascalCase，文件名和 require 路径都用大写开头
            string charTable = fileName + "_Characters";
            string buildingTable = fileName + "_Buildings";
            string objTable = fileName + "_Objects";
            string pathTable = fileName + "_Paths";
            sb.AppendLine("    SubTables = {");
            sb.AppendLine("        Characters = \"" + charTable + "\",");
            sb.AppendLine("        Buildings  = \"" + buildingTable + "\",");
            sb.AppendLine("        Objects    = \"" + objTable + "\",");
            sb.AppendLine("        Paths      = \"" + pathTable + "\",");
            sb.AppendLine("    },");

            sb.AppendLine("}");
            return sb.ToString();
        }

        #endregion

        #region 2. 角色子文件生成

        /// <summary>
        /// 组装角色子文件（只存差异字段，默认值走主表 Defaults）
        /// </summary>
        private static string BuildCharactersFile(string baseFileName, List<MapCharacterData> characters)
        {
            StringBuilder sb = new StringBuilder();
            // 去掉 "Table" 前缀，和主表命名保持一致：LevelData_1_1_Characters
            string mainTableName = baseFileName.StartsWith("Table") ? baseFileName.Substring(5) : baseFileName;
            string tableName = mainTableName + "_Characters";

            sb.AppendLine("--=====================================================================================================");
            sb.AppendLine("-- filename:  " + tableName);
            sb.AppendLine("-- descrip:   角色列表（自动生成）");
            sb.AppendLine("-- notices:   由地图编辑器自动生成，禁止手动修改");
            sb.AppendLine("-- @brief     关卡角色实体列表（只存差异字段）");
            sb.AppendLine("--=====================================================================================================");
            sb.AppendLine();
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "Tables.{0} = {{", tableName));

            if (characters == null || characters.Count == 0)
            {
                sb.AppendLine("    -- 空列表");
            }
            else
            {
                for (int i = 0; i < characters.Count; i++)
                {
                    MapCharacterData c = characters[i];
                    string pathId = "path_" + c.id;

                    sb.AppendLine(string.Format(CultureInfo.InvariantCulture,
                        "    [{0}] = {{\n" +
                        "        Uid         = \"{1}\",\n" +
                        "        TemplateId  = \"{2}\",\n" +
                        "        Name        = \"{3}\",\n" +
                        "        GridX       = {4},\n" +
                        "        GridY       = {5},\n" +
                        "        Width       = {6},\n" +
                        "        Height      = {7},\n" +
                        "        Image       = \"{8}\",\n" +
                        "        Anim        = \"{9}\",\n" +
                        "        PatrolLoop  = \"{10}\",\n" +
                        "        Interactable = {11},\n" +
                        "        InteractType = \"{12}\",\n" +
                        "        DialogBubble = \"{13}\",\n" +
                        "        PathId      = \"{14}\",\n" +
                        "    }},",
                        i + 1,
                        Escape(c.id),
                        Escape(c.templateId),
                        Escape(c.name),
                        c.gridX,
                        c.gridY,
                        c.width,
                        c.height,
                        Escape(c.image),
                        Escape(c.anim),
                        Escape(c.walkMode),
                        BoolToLua(c.interactable),
                        Escape(c.interactType),
                        Escape(c.dialogBubble),
                        Escape(pathId)));
                }
            }

            sb.AppendLine("}");
            return sb.ToString();
        }

        #endregion

        #region 3. 建筑子文件生成

        /// <summary>
        /// 组装建筑子文件
        /// </summary>
        private static string BuildBuildingsFile(string baseFileName, List<MapObjectData> buildings)
        {
            StringBuilder sb = new StringBuilder();
            string mainTableName = baseFileName.StartsWith("Table") ? baseFileName.Substring(5) : baseFileName;
            string tableName = mainTableName + "_Buildings";

            sb.AppendLine("--=====================================================================================================");
            sb.AppendLine("-- filename:  " + tableName);
            sb.AppendLine("-- descrip:   建筑列表（自动生成）");
            sb.AppendLine("-- notices:   由地图编辑器自动生成，禁止手动修改");
            sb.AppendLine("-- @brief     关卡建筑实体列表");
            sb.AppendLine("--=====================================================================================================");
            sb.AppendLine();
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "Tables.{0} = {{", tableName));

            if (buildings == null || buildings.Count == 0)
            {
                sb.AppendLine("    -- 空列表");
            }
            else
            {
                for (int i = 0; i < buildings.Count; i++)
                {
                    MapObjectData o = buildings[i];
                    sb.AppendLine(string.Format(CultureInfo.InvariantCulture,
                        "    [{0}] = {{\n" +
                        "        Uid        = \"{1}\",\n" +
                        "        TemplateId = \"{2}\",\n" +
                        "        Name       = \"{3}\",\n" +
                        "        GridX      = {4},\n" +
                        "        GridY      = {5},\n" +
                        "        Width      = {6},\n" +
                        "        Height     = {7},\n" +
                        "        Image      = \"{8}\",\n" +
                        "        Anim       = \"{9}\",\n" +
                        "    }},",
                        i + 1,
                        Escape(o.id),
                        Escape(o.templateId),
                        Escape(o.name),
                        o.gridX,
                        o.gridY,
                        o.width,
                        o.height,
                        Escape(o.image),
                        Escape(o.anim)));
                }
            }

            sb.AppendLine("}");
            return sb.ToString();
        }

        #endregion

        #region 4. 物件子文件生成

        /// <summary>
        /// 组装物件子文件（数组压缩格式）
        /// 格式：{ TemplateId, GridX, GridY, ImageName, CollectId }
        /// </summary>
        private static string BuildObjectsFile(string baseFileName, List<MapObjectData> objects)
        {
            StringBuilder sb = new StringBuilder();
            string mainTableName = baseFileName.StartsWith("Table") ? baseFileName.Substring(5) : baseFileName;
            string tableName = mainTableName + "_Objects";

            sb.AppendLine("--=====================================================================================================");
            sb.AppendLine("-- filename:  " + tableName);
            sb.AppendLine("-- descrip:   待收集物件列表（自动生成·数组压缩格式）");
            sb.AppendLine("-- notices:   由地图编辑器自动生成，禁止手动修改");
            sb.AppendLine("-- @brief     关卡待收集物件列表");
            sb.AppendLine("-- @desc      数组格式：{ TemplateId, GridX, GridY, ImageName, CollectId }");
            sb.AppendLine("--=====================================================================================================");
            sb.AppendLine();
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "Tables.{0} = {{", tableName));

            if (objects == null || objects.Count == 0)
            {
                sb.AppendLine("    -- 空列表");
            }
            else
            {
                for (int i = 0; i < objects.Count; i++)
                {
                    MapObjectData o = objects[i];
                    string collectId = "collect_" + o.id;
                    sb.AppendLine(string.Format(CultureInfo.InvariantCulture,
                        "    {{ \"{0}\", {1}, {2}, \"{3}\", \"{4}\" }},",
                        Escape(o.templateId),
                        o.gridX,
                        o.gridY,
                        Escape(o.image),
                        Escape(collectId)));
                }
            }

            sb.AppendLine("}");
            return sb.ToString();
        }

        #endregion

        #region 5. 路径子文件生成

        /// <summary>
        /// 组装路径子文件（按 PathId 索引，纯数组路径点）
        /// 路径点格式：{ gridX, gridY }
        /// </summary>
        private static string BuildPathsFile(string baseFileName, List<MapCharacterData> characters)
        {
            StringBuilder sb = new StringBuilder();
            string mainTableName = baseFileName.StartsWith("Table") ? baseFileName.Substring(5) : baseFileName;
            string tableName = mainTableName + "_Paths";

            sb.AppendLine("--=====================================================================================================");
            sb.AppendLine("-- filename:  " + tableName);
            sb.AppendLine("-- descrip:   路径数据（自动生成）");
            sb.AppendLine("-- notices:   由地图编辑器自动生成，禁止手动修改");
            sb.AppendLine("-- @brief     所有角色路径点，按 PathId 索引");
            sb.AppendLine("-- @desc      路径点格式：{ gridX, gridY } 纯数组");
            sb.AppendLine("--=====================================================================================================");
            sb.AppendLine();
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "Tables.{0} = {{", tableName));

            if (characters != null)
            {
                foreach (var c in characters)
                {
                    if (c == null || c.pathPoints == null || c.pathPoints.Count == 0)
                        continue;

                    string pathId = "path_" + c.id;
                    sb.AppendLine("    [\"" + Escape(pathId) + "\"] = {");

                    foreach (var p in c.pathPoints)
                    {
                        int gx = Mathf.FloorToInt(p.x / MapToolConstants.GridSize);
                        int gy = Mathf.FloorToInt(p.y / MapToolConstants.GridSize);
                        sb.AppendLine(string.Format(CultureInfo.InvariantCulture,
                            "        {{{0}, {1}}},", gx, gy));
                    }

                    sb.AppendLine("    },");
                }
            }

            sb.AppendLine("}");
            return sb.ToString();
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
            {
                result[i] = "\"" + Escape(ids[i]) + "\"";
            }
            return result;
        }

        #endregion
    }
}
