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

    /// <summary>主表文件名前缀（子表名要去掉该前缀，与主表命名保持一致）</summary>
    private const string k_TablePrefix = "Table";

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
                string projectRoot = MapToolPathHelper.ProjectRoot;
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

            // ========== 6. 交通规则子文件（红绿灯 / 斑马线 / 区域 / 人行道格 / 道路格） ==========
            string trafficPath = Path.Combine(dir, baseFileName + "_Traffic.lua.txt");
            string trafficContent = BuildTrafficFile(baseFileName, data);
            File.WriteAllText(trafficPath, trafficContent, new UTF8Encoding(false));
            generatedFiles.Add(trafficPath);

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
        /// 由主表文件名推导子表名：去掉 "Table" 前缀后拼后缀（TableLevelData_1_1 → LevelData_1_1_Characters）
        /// </summary>
        /// <param name="baseFileName">主配置表文件名（如 TableLevelData_1_1）</param>
        /// <param name="suffix">子表后缀（含下划线，如 "_Characters"）</param>
        /// <returns>子表名</returns>
        private static string BuildSubTableName(string baseFileName, string suffix)
        {
            string mainTableName = baseFileName.StartsWith(k_TablePrefix)
                ? baseFileName.Substring(k_TablePrefix.Length)
                : baseFileName;
            return mainTableName + suffix;
        }

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
            sb.AppendLine("        SavePath      = \"" + MapToolConstants.MapJsonSaveDir + "/Map_" + Escape(chapterId) + "_" + Escape(levelIdStr) + ".json\",");
            sb.AppendLine("        Time          = \"" + Escape(time) + "\",");
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "        Width         = {0},", width));
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "        Height        = {0},", height));

            int gridSize = MapToolConstants.GridSize;
            int gridCols = MapToolEditorUtils.GridCols(width);
            int gridRows = MapToolEditorUtils.GridRows(height);
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "        GridSize      = {0},", gridSize));
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "        GridCols      = {0},", gridCols));
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "        GridRows      = {0},", gridRows));

            // 基线步长：与地图 JSON 同源，缺失/非法时回退编辑器默认步长
            int baselineStep = data.baselineStep >= MapToolConstants.LayerBaselineStepMin
                ? data.baselineStep
                : MapToolConstants.LayerBaselineStepDefault;
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "        BaselineStep  = {0},", baselineStep));
            // 绘制形状 / 区域类型：与地图 JSON 同源，缺失/非法时回退默认值
            string drawShape = DrawShape.ToCode(data.drawShape);
            sb.AppendLine("        DrawShape    = \"" + Escape(drawShape) + "\",");
            string regionType = RegionType.ToCode(data.regionType);
            sb.AppendLine("        RegionType   = \"" + Escape(regionType) + "\",");
            sb.AppendLine("    },");
            sb.AppendLine();

            // --- Defaults 块 ---
            sb.AppendLine("    -- ==========================================================================");
            sb.AppendLine("    -- 2. 全局默认配置（所有实体共享，实体自身字段优先）");
            sb.AppendLine("    -- ==========================================================================");
            sb.AppendLine("    Defaults = {");
            sb.AppendLine("        SortOrder        = 40,");
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
            int lightCount = data.trafficLights != null ? data.trafficLights.Count : 0;
            int crosswalkCount = data.crosswalks != null ? data.crosswalks.Count : 0;
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "        Characters = {0},", charCount));
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "        Buildings  = {0},", buildingCount));
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "        Objects    = {0},", objCount));
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "        Lights     = {0},", lightCount));
            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "        Crosswalks = {0},", crosswalkCount));
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
            string trafficTable = fileName + "_Traffic";
            sb.AppendLine("    SubTables = {");
            sb.AppendLine("        Characters = \"" + charTable + "\",");
            sb.AppendLine("        Buildings  = \"" + buildingTable + "\",");
            sb.AppendLine("        Objects    = \"" + objTable + "\",");
            sb.AppendLine("        Paths      = \"" + pathTable + "\",");
            sb.AppendLine("        Traffic    = \"" + trafficTable + "\",");
            sb.AppendLine("    },");

            sb.AppendLine("}");
            return sb.ToString();
        }

        #endregion

        #region Lua 子文件公共头

        /// <summary>数值输出格式（定点两位小数，去除无意义尾零）</summary>
        private const string LuaNumberFormat = "0.##";

        /// <summary>Lua 文件横幅（上下边框）</summary>
        private const string LuaFileBanner = "--=====================================================================================================";

        /// <summary>子文件只读声明（与运行时表头口径一致）</summary>
        private const string LuaFileNotice = "由地图编辑器自动生成，禁止手动修改";

        /// <summary>
        /// 追加 Lua 子文件公共文件头（横幅 / 文件名 / 描述 / 只读声明 / 概述 / 可选补充说明 / 空行）
        /// </summary>
        /// <param name="sb">输出缓冲</param>
        /// <param name="tableName">表名（写入 filename 行）</param>
        /// <param name="descrip">文件用途描述</param>
        /// <param name="brief">内容概述（@brief）</param>
        /// <param name="descDetail">补充说明（@desc，可为空）</param>
        private static void AppendSubFileHeader(StringBuilder sb, string tableName, string descrip, string brief, string descDetail = null)
        {
            sb.AppendLine(LuaFileBanner);
            sb.AppendLine("-- filename:  " + tableName);
            sb.AppendLine("-- descrip:   " + descrip);
            sb.AppendLine("-- notices:   " + LuaFileNotice);
            sb.AppendLine("-- @brief     " + brief);
            if (descDetail != null)
            {
                sb.AppendLine("-- @desc      " + descDetail);
            }
            sb.AppendLine(LuaFileBanner);
            sb.AppendLine();
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
            string tableName = BuildSubTableName(baseFileName, "_Characters");

            AppendSubFileHeader(sb, tableName, "角色列表（自动生成）", "关卡角色实体列表（只存差异字段）");

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
                        "        PosX        = {6},\n" +
                        "        PosY        = {7},\n" +
                        "        Width       = {8},\n" +
                        "        Height      = {9},\n" +
                        "        PixelWidth  = {10},\n" +
                        "        PixelHeight = {11},\n" +
                        "        Stretch     = {12},\n" +
                        "        LayerType   = \"{13}\",\n" +
                        "        Image       = \"{14}\",\n" +
                        "        Anim        = \"{15}\",\n" +
                        "        PatrolLoop  = \"{16}\",\n" +
                        "        Interactable = {17},\n" +
                        "        InteractType = \"{18}\",\n" +
                        "        DialogBubble = \"{19}\",\n" +
                        "        SortOrder    = {21},\n" +
                        "        YSortEnabled = {22},\n" +
                        "        YSortOffset  = {23},\n" +
                        "        Rotation      = {24},\n" +
                        "        Scale         = {25},\n" +
                        "        PathId      = \"{20}\",\n" +
                        "        DisplayLayer = \"{26}\",\n" +
                        "        IsVehicle     = {27},\n" +
                        "        TrafficRole  = \"{28}\",\n" +
                        "        StopLineX    = {29},\n" +
                        "        StopLineY    = {30},\n" +
                        "        BoundLightId = \"{31}\",\n" +
                        "        BoundLightIds = {33},\n" +
                        "        YellowTreatAsRed = {32},\n" +
                        "    }},",
                        i + 1,
                        Escape(c.id),
                        Escape(c.templateId),
                        Escape(c.name),
                        c.gridX,
                        c.gridY,
                        c.posX.ToString(LuaNumberFormat, CultureInfo.InvariantCulture),
                        c.posY.ToString(LuaNumberFormat, CultureInfo.InvariantCulture),
                        c.width,
                        c.height,
                        c.pixelWidth.ToString(LuaNumberFormat, CultureInfo.InvariantCulture),
                        c.pixelHeight.ToString(LuaNumberFormat, CultureInfo.InvariantCulture),
                        BoolToLua(c.stretch),
                        Escape(c.layerType),
                        Escape(c.image),
                        Escape(c.anim),
                        Escape(c.walkMode),
                        BoolToLua(c.interactable),
                        Escape(c.interactType),
                        Escape(c.dialogBubble),
                        Escape(pathId),
                        c.sortOrder,
                        BoolToLua(c.ySortEnabled),
                        c.ySortOffset.ToString(LuaNumberFormat, CultureInfo.InvariantCulture),
                        c.rotation.ToString(LuaNumberFormat, CultureInfo.InvariantCulture),
                        c.scale.ToString(LuaNumberFormat, CultureInfo.InvariantCulture),
                        Escape(c.displayLayer),
                        BoolToLua(c.isVehicle),
                        Escape(c.trafficRole),
                        c.stopLineX.ToString(LuaNumberFormat, CultureInfo.InvariantCulture),
                        c.stopLineY.ToString(LuaNumberFormat, CultureInfo.InvariantCulture),
                        Escape(c.boundLightId),
                        BoolToLua(c.yellowTreatAsRed),
                        BuildLightIdArray(c.boundLightIds, c.boundLightId)));
                }
            }

            sb.AppendLine("}");
            return sb.ToString();
        }

        #endregion

        #region 3. 建筑子文件生成

        #region 物件条目 Lua 模板（建筑列表与物件列表共用）

        /// <summary>单条物件条目的公共前缀（{0}=序号 … {14}=图片）</summary>
        private const string ObjectEntryCommonPrefix =
            "    [{0}] = {{\n" +
            "        Uid        = \"{1}\",\n" +
            "        TemplateId = \"{2}\",\n" +
            "        Name       = \"{3}\",\n" +
            "        GridX      = {4},\n" +
            "        GridY      = {5},\n" +
            "        PosX      = {6},\n" +
            "        PosY      = {7},\n" +
            "        Width      = {8},\n" +
            "        Height     = {9},\n" +
            "        PixelWidth  = {10},\n" +
            "        PixelHeight = {11},\n" +
            "        Stretch     = {12},\n" +
            "        LayerType   = \"{13}\",\n" +
            "        DisplayLayer = \"{21}\",\n" +
            "        Image      = \"{14}\",\n";

        /// <summary>单条物件条目的公共后缀（{16}=排序值 … {21}=显示层）</summary>
        private const string ObjectEntryCommonSuffix =
            "        SortOrder    = {16},\n" +
            "        YSortEnabled = {17},\n" +
            "        YSortOffset  = {18},\n" +
            "        Rotation      = {19},\n" +
            "        Scale         = {20},\n" +
            "    }},";

        #endregion

        /// <summary>
        /// 组装建筑子文件
        /// </summary>
        private static string BuildBuildingsFile(string baseFileName, List<MapObjectData> buildings)
        {
            StringBuilder sb = new StringBuilder();
            string tableName = BuildSubTableName(baseFileName, "_Buildings");

            AppendSubFileHeader(sb, tableName, "建筑列表（自动生成）", "关卡建筑实体列表");

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
                        ObjectEntryCommonPrefix +
                        "        Anim       = \"{15}\",\n" +
                        ObjectEntryCommonSuffix,
                        i + 1,
                        Escape(o.id),
                        Escape(o.templateId),
                        Escape(o.name),
                        o.gridX,
                        o.gridY,
                        o.posX.ToString(LuaNumberFormat, CultureInfo.InvariantCulture),
                        o.posY.ToString(LuaNumberFormat, CultureInfo.InvariantCulture),
                        o.width,
                        o.height,
                        o.pixelWidth.ToString(LuaNumberFormat, CultureInfo.InvariantCulture),
                        o.pixelHeight.ToString(LuaNumberFormat, CultureInfo.InvariantCulture),
                        BoolToLua(o.stretch),
                        Escape(o.layerType),
                        Escape(o.image),
                        Escape(o.anim),
                        o.sortOrder,
                        BoolToLua(o.ySortEnabled),
                        o.ySortOffset.ToString(LuaNumberFormat, CultureInfo.InvariantCulture),
                        o.rotation.ToString(LuaNumberFormat, CultureInfo.InvariantCulture),
                        o.scale.ToString(LuaNumberFormat, CultureInfo.InvariantCulture),
                        Escape(o.displayLayer)));
                }
            }

            sb.AppendLine("}");
            return sb.ToString();
        }

        #endregion

        #region 4. 物件子文件生成

        /// <summary>
        /// 组装物件子文件（命名块格式，与 Buildings 条目字段对齐，便于运行时按 PSD 像素位置/尺寸/拉伸还原）
        /// </summary>
        private static string BuildObjectsFile(string baseFileName, List<MapObjectData> objects)
        {
            StringBuilder sb = new StringBuilder();
            string tableName = BuildSubTableName(baseFileName, "_Objects");

            AppendSubFileHeader(sb, tableName, "待收集物件列表（自动生成·命名块格式）", "关卡待收集物件列表", "字段与 Buildings 对齐：GridX/GridY 兼容旧换算，PosX/PosY/PixelWidth/PixelHeight/Stretch/LayerType 按 PSD 还原");

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
                        ObjectEntryCommonPrefix +
                        "        CollectId  = \"{15}\",\n" +
                        ObjectEntryCommonSuffix,
                        i + 1,
                        Escape(o.id),
                        Escape(o.templateId),
                        Escape(o.name),
                        o.gridX,
                        o.gridY,
                        o.posX.ToString(LuaNumberFormat, CultureInfo.InvariantCulture),
                        o.posY.ToString(LuaNumberFormat, CultureInfo.InvariantCulture),
                        o.width,
                        o.height,
                        o.pixelWidth.ToString(LuaNumberFormat, CultureInfo.InvariantCulture),
                        o.pixelHeight.ToString(LuaNumberFormat, CultureInfo.InvariantCulture),
                        BoolToLua(o.stretch),
                        Escape(o.layerType),
                        Escape(o.image),
                        Escape(collectId),
                        o.sortOrder,
                        BoolToLua(o.ySortEnabled),
                        o.ySortOffset.ToString(LuaNumberFormat, CultureInfo.InvariantCulture),
                        o.rotation.ToString(LuaNumberFormat, CultureInfo.InvariantCulture),
                        o.scale.ToString(LuaNumberFormat, CultureInfo.InvariantCulture),
                        Escape(o.displayLayer)));
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
            string tableName = BuildSubTableName(baseFileName, "_Paths");

            AppendSubFileHeader(sb, tableName, "路径数据（自动生成）", "所有角色路径点，按 PathId 索引", "路径点格式：{ gridX, gridY } 纯数组");

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

        #region 6. 交通规则子文件生成

        /// <summary>
        /// 组装交通规则子文件（红绿灯 / 斑马线 / 矩形区域 / 人行道格 / 道路格）
        /// 纯数据导出，运行时由门面 TrafficManager 消费；C# 层不写交通规则
        /// </summary>
        private static string BuildTrafficFile(string baseFileName, MapConfigData data)
        {
            StringBuilder sb = new StringBuilder();
            string tableName = BuildSubTableName(baseFileName, "_Traffic");

            AppendSubFileHeader(sb, tableName, "交通规则数据（自动生成）", "红绿灯 / 斑马线 / 区域 / 人行道格 / 道路格");

            sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "Tables.{0} = {{", tableName));

            // ===== 红绿灯 =====
            sb.AppendLine("    Lights = {");
            if (data.trafficLights != null)
            {
                for (int i = 0; i < data.trafficLights.Count; i++)
                {
                    var l = data.trafficLights[i];
                    sb.AppendLine(string.Format(CultureInfo.InvariantCulture,
                        "        [{0}] = {{\n" +
                        "            Uid                = \"{1}\",\n" +
                        "            TemplateId         = \"{2}\",\n" +
                        "            Name               = \"{3}\",\n" +
                        "            GridX              = {4},\n" +
                        "            GridY              = {5},\n" +
                        "            PosX               = {6},\n" +
                        "            PosY               = {7},\n" +
                        "            Direction          = \"{8}\",\n" +
                        "            Side               = \"{9}\",\n" +
                        "            IntersectionId     = \"{10}\",\n" +
                        "            RedDuration        = {11},\n" +
                        "            GreenDuration       = {12},\n" +
                        "            YellowDuration      = {13},\n" +
                        "            OffsetPhase         = {14},\n" +
                        "            BoundCrosswalkId   = \"{15}\",\n" +
                        "            InitialPhase        = \"{16}\",\n" +
                        "            Image               = \"{17}\",\n" +
                        "            Anim                = \"{18}\",\n" +
                        "        }},",
                        i + 1,
                        Escape(l.id), Escape(l.templateId), Escape(l.name),
                        l.gridX, l.gridY,
                        l.posX.ToString(LuaNumberFormat, CultureInfo.InvariantCulture),
                        l.posY.ToString(LuaNumberFormat, CultureInfo.InvariantCulture),
                        Escape(l.direction), Escape(l.side), Escape(l.intersectionId),
                        l.redDuration.ToString(LuaNumberFormat, CultureInfo.InvariantCulture),
                        l.greenDuration.ToString(LuaNumberFormat, CultureInfo.InvariantCulture),
                        l.yellowDuration.ToString(LuaNumberFormat, CultureInfo.InvariantCulture),
                        l.offsetPhase.ToString(LuaNumberFormat, CultureInfo.InvariantCulture),
                        Escape(l.boundCrosswalkId), Escape(l.initialPhase),
                        Escape(l.image), Escape(l.anim)));
                }
            }
            sb.AppendLine("    },");

            // ===== 斑马线 =====
            sb.AppendLine("    Crosswalks = {");
            if (data.crosswalks != null)
            {
                for (int i = 0; i < data.crosswalks.Count; i++)
                {
                    var cw = data.crosswalks[i];
                    var a = cw.pointA ?? new MapPathPointData();
                    var b = cw.pointB ?? new MapPathPointData();
                    sb.AppendLine(string.Format(CultureInfo.InvariantCulture,
                        "        [{0}] = {{\n" +
                        "            Uid           = \"{1}\",\n" +
                        "            Name          = \"{2}\",\n" +
                        "            PointA        = {{ X = {3}, Y = {4} }},\n" +
                        "            PointB        = {{ X = {5}, Y = {6} }},\n" +
                        "            Width         = {7},\n" +
                        "            Direction     = \"{8}\",\n" +
                        "            BoundLightId  = \"{9}\",\n" +
                        "        }},",
                        i + 1,
                        Escape(cw.id), Escape(cw.name),
                        a.x.ToString(LuaNumberFormat, CultureInfo.InvariantCulture),
                        a.y.ToString(LuaNumberFormat, CultureInfo.InvariantCulture),
                        b.x.ToString(LuaNumberFormat, CultureInfo.InvariantCulture),
                        b.y.ToString(LuaNumberFormat, CultureInfo.InvariantCulture),
                        cw.width.ToString(LuaNumberFormat, CultureInfo.InvariantCulture),
                        Escape(cw.direction), Escape(cw.boundLightId)));
                }
            }
            sb.AppendLine("    },");

            // ===== 矩形区域回显 =====
            sb.AppendLine("    Regions = {");
            if (data.regions != null)
            {
                for (int i = 0; i < data.regions.Count; i++)
                {
                    var r = data.regions[i];
                    sb.AppendLine(string.Format(CultureInfo.InvariantCulture,
                        "        [{0}] = {{\n" +
                        "            Uid     = \"{1}\",\n" +
                        "            Type    = \"{2}\",\n" +
                        "            X       = {3},\n" +
                        "            Y       = {4},\n" +
                        "            Width   = {5},\n" +
                        "            Height  = {6},\n" +
                        "        }},",
                        i + 1, Escape(r.id), Escape(r.type),
                        r.x.ToString(LuaNumberFormat, CultureInfo.InvariantCulture),
                        r.y.ToString(LuaNumberFormat, CultureInfo.InvariantCulture),
                        r.width.ToString(LuaNumberFormat, CultureInfo.InvariantCulture),
                        r.height.ToString(LuaNumberFormat, CultureInfo.InvariantCulture)));
                }
            }
            sb.AppendLine("    },");

            // ===== 人行道可走格 =====
            sb.AppendLine("    SidewalkCells = {");
            AppendCellArray(sb, data.sidewalkCells);
            sb.AppendLine("    },");

            // ===== 车辆行驶格 =====
            sb.AppendLine("    RoadCells = {");
            AppendCellArray(sb, data.roadCells);
            sb.AppendLine("    },");

            sb.AppendLine("}");
            return sb.ToString();
        }

        /// <summary>
        /// 追加格子集合纯数组条目：{ gridX, gridY }
        /// </summary>
        private static void AppendCellArray(StringBuilder sb, List<MapGridCellData> cells)
        {
            if (cells == null) return;
            foreach (var cell in cells)
            {
                if (cell == null) continue;
                sb.AppendLine(string.Format(CultureInfo.InvariantCulture, "        {{ {0}, {1} }},", cell.x, cell.y));
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

        /// <summary>
        /// 构造绑定灯 ID 的 Lua 数组字面量；去空去重，旧单值字段兜底补入
        /// </summary>
        /// <param name="ids">多选灯 ID 列表（可空）</param>
        /// <param name="legacyId">旧版单值绑定灯 ID（可空）</param>
        /// <returns>如 { "tl0001", "tl0002" }，空集合返回 {}</returns>
        private static string BuildLightIdArray(List<string> ids, string legacyId)
        {
            var list = new List<string>();
            if (ids != null)
            {
                foreach (var id in ids)
                {
                    if (!string.IsNullOrEmpty(id) && !list.Contains(id)) list.Add(id);
                }
            }
            if (!string.IsNullOrEmpty(legacyId) && !list.Contains(legacyId)) list.Add(legacyId);
            if (list.Count == 0) return "{}";
            var parts = new string[list.Count];
            for (int i = 0; i < list.Count; i++) parts[i] = "\"" + Escape(list[i]) + "\"";
            return "{ " + string.Join(", ", parts) + " }";
        }

        #endregion
    }
}
