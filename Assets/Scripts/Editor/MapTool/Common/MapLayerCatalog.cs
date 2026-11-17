/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  MapLayerCatalog.cs
 * author:    云毅
 * created:   2026
 * descrip:   地图分层目录/排序/类型映射目录（对齐截图 L0–L6 功能分桶 + 气泡置顶）
 *            Background / Greenery / Road / Buildings / Props / Characters / Foreground / Bubbles
 *            旧键 ground / cat / effect 作为别名归一到 greenery / character / foreground，
 *            旧 JSON/Lua 配置与旧预制体节点读取保持兼容，不做数据迁移。
 ***************************************************************/

using System;
using System.Collections.Generic;

namespace Honor.Editor.MapTool
{
    /// <summary>
    /// 地图分层映射目录：类型键 ↔ 目录名 ↔ SortOrder ↔ 编辑器枚举
    /// </summary>
    /// <remarks>
    /// 类型键统一为小写字符串，贯穿 Python 切片脚本、C# 导入模块、JSON/Lua 配置与运行时挂载。
    /// 层分桶对齐截图功能层：L0 远景背景 / L1 建筑房屋 / L2 绿化 / L3 道路市政 /
    /// L4 道具家具 / L5 角色·载具动态 / L6 前景近景；气泡层独立置顶，UI 层不参与地图 Z 序。
    /// 绿化/道路/前景/气泡在编辑器枚举中归并到 Prop，避免破坏既有枚举分支，层级区分由 LayerType 承载。
    /// </remarks>
    public static class MapLayerCatalog
    {
        #region 类型键常量

        /// <summary>类型键：L0 远景背景层（围墙边界、远景屋顶轮廓、狗屋、远景水池花坛）</summary>
        public const string Background = "background";

        /// <summary>类型键：L2 绿化层（乔木、灌木、绿篱、花丛、向日葵、猫爪造型绿篱）</summary>
        public const string Greenery = "greenery";

        /// <summary>类型键：L3 道路市政层（人行道、车道、斑马线、路灯、红绿灯、路牌、拱门、消防栓）</summary>
        public const string Road = "road";

        /// <summary>类型键：L1 建筑房屋层（蛋糕店、餐厅、披萨店、小屋、浴缸店、候车亭、鸟屋）</summary>
        public const string Building = "building";

        /// <summary>类型键：L4 道具家具层（遮阳伞、桌椅躺椅、餐车摊位、甜品柜、气球、风车、立牌）</summary>
        public const string Prop = "prop";

        /// <summary>类型键：L5 角色/载具动态层（步行/坐姿/睡觉猫，出租车、巴士、货车、轿车）</summary>
        public const string Character = "character";

        /// <summary>类型键：L6 前景近景层（最下缘树冠、摊位前沿、近景装饰；兼收挂父物件的前景特效）</summary>
        public const string Foreground = "foreground";

        /// <summary>类型键：气泡置顶层（思考/睡觉 Z 字/对话气泡，挂父物件并独立置顶，UI 之下）</summary>
        public const string Bubble = "bubble";

        #region 旧键兼容常量（仅作引用兼容，读取时由 Normalize 归一到新键，禁止新增等值比较）

        /// <summary>旧键：地面装饰层 → 归一到 <see cref="Greenery"/>（grass_/plant_ 归绿化，road_ 归 <see cref="Road"/>）</summary>
        public const string Ground = "ground";

        /// <summary>旧键：猫猫层 → 归一到 <see cref="Character"/>（猫并入角色/载具动态层）</summary>
        public const string Cat = "cat";

        /// <summary>旧键：特效层 → 归一到 <see cref="Foreground"/>（前景/特效合并为前景近景层）</summary>
        public const string Effect = "effect";

        #endregion

        #endregion

        #region 目录名常量（对齐截图层级表）

        /// <summary>L0 背景层节点/目录名</summary>
        public const string DirBackground = "Background";

        /// <summary>L2 绿化层节点/目录名</summary>
        public const string DirGreenery = "Greenery";

        /// <summary>L3 道路市政层节点/目录名</summary>
        public const string DirRoad = "Road";

        /// <summary>L1 建筑层节点/目录名</summary>
        public const string DirBuildings = "Buildings";

        /// <summary>L4 道具层节点/目录名</summary>
        public const string DirProps = "Props";

        /// <summary>L5 角色/载具动态层节点/目录名</summary>
        public const string DirCharacters = "Characters";

        /// <summary>L6 前景近景层节点/目录名</summary>
        public const string DirForeground = "Foreground";

        /// <summary>气泡置顶层节点/目录名</summary>
        public const string DirBubbles = "Bubbles";

        #region 旧节点名兼容常量（旧预制体/旧数据兜底查找用，新挂载一律走上述新目录名）

        /// <summary>旧节点名：Ground（绿化/道路旧合并节点，保留用于兜底查找）</summary>
        public const string DirGround = "Ground";

        /// <summary>旧节点名：Cats（猫旧独立节点，保留用于兜底查找）</summary>
        public const string DirCats = "Cats";

        /// <summary>旧节点名：Effects（特效旧节点，保留用于兜底查找）</summary>
        public const string DirEffects = "Effects";

        #endregion

        #endregion

        /// <summary>
        /// 全部层级类型键（自底向上渲染顺序）：
        /// background 0 / greenery 10 / road 20 / building 30 / prop 40 / character 50 / foreground 60 / bubble 70。
        /// 层节点 sibling 顺序、目录派生、显隐排序均以本数组为准；基线步长见 <see cref="MapToolConstants.LayerBaselineStep"/>。
        /// </summary>
        public static readonly string[] AllLayerTypes =
        {
            Background, Greenery, Road, Building, Prop, Character, Foreground, Bubble
        };

        /// <summary>
        /// 类型键归一化：兼容旧单数目录名、大小写、旧键与别名；无法识别时回退道具层。
        /// 旧键映射：ground/grass/plant → <see cref="Greenery"/>；road → <see cref="Road"/>；
        /// cat/cats → <see cref="Character"/>；effect/effects/fx → <see cref="Foreground"/>。
        /// </summary>
        /// <param name="layerType">原始类型键/目录名</param>
        /// <returns>归一化后的类型键</returns>
        public static string Normalize(string layerType)
        {
            if (string.IsNullOrEmpty(layerType)) return Prop;
            switch (layerType.Trim().ToLowerInvariant())
            {
                case Background:
                case "bg":
                    return Background;
                case Greenery:
                case Ground:
                case "grass":
                case "plant":
                case "foliage":
                case "lawn":
                case "shrub":
                case "flower":
                    return Greenery;
                case Road:
                case "street":
                case "sidewalk":
                case "crosswalk":
                    return Road;
                case Building:
                case DirBuildings:
                    return Building;
                case Prop:
                case DirProps:
                case "object":
                case "objects":
                    return Prop;
                case Character:
                case DirCharacters:
                case Cat:
                case DirCats:
                case "vehicle":
                case "car":
                case "taxi":
                case "bus":
                case "truck":
                    return Character;
                case Foreground:
                case Effect:
                case DirEffects:
                case "fx":
                case "front":
                    return Foreground;
                case Bubble:
                case DirBubbles:
                    return Bubble;
                default:
                    return Prop;
            }
        }

        #region 模板名关键词 → 归属层（导入/存量归类共用）

        /// <summary>
        /// 模板名英文关键词 → 归属层键。仅收录可稳定识别的英文前缀；PSD 编号名（图层/组/形状/圆角矩形/椭圆等无英文前缀）不在表内，回退 <see cref="Prop"/>，由编辑器人工调整层级档。
        /// </summary>
        private static readonly Dictionary<string, string> NameKeywordToLayer = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            // 绿化：乔木/灌木/草丛/花丛/景观石/树桩
            { "tree", Greenery }, { "shrub", Greenery }, { "grass", Greenery }, { "plant", Greenery },
            { "rock", Greenery }, { "stump", Greenery },
            // 道路市政：路灯/围栏/垃圾桶
            { "lamp", Road }, { "fence", Road }, { "trashcan", Road },
            // 角色/载具：轿车/船/出租车/巴士/货车
            { "car", Character }, { "boat", Character }, { "taxi", Character }, { "bus", Character },
            { "truck", Character }, { "vehicle", Character },
            // 背景：水池/温泉
            { "pond", Background }, { "hotspring", Background },
        };

        /// <summary>
        /// 按物件模板名/英文名前缀推断归属层键（导入归类与存量迁移共用同一规则）。
        /// 第一段为类别前缀（prop/char/building）时跳过；首段 char/character 归角色、building 归建筑；
        /// 其余英文段命中关键词表则归对应层；PSD 编号名等无法识别的回退 <see cref="Prop"/>。
        /// </summary>
        /// <param name="templateId">模板ID（优先）</param>
        /// <param name="name">物件名（templateId 为空时兜底）</param>
        /// <returns>推断出的归一化归属层键</returns>
        public static string ResolveLayerByTemplateName(string templateId, string name)
        {
            string source = !string.IsNullOrWhiteSpace(templateId) ? templateId : (name ?? string.Empty);
            if (string.IsNullOrWhiteSpace(source)) return Prop;

            var parts = source.ToLowerInvariant().Split('_', ' ', '-');
            if (parts.Length > 0)
            {
                if (parts[0] == "char" || parts[0] == "character") return Character;
                if (parts[0] == "building") return Building;
            }

            int start = parts.Length > 1 && (parts[0] == "prop" || parts[0] == "char" || parts[0] == "building") ? 1 : 0;
            for (int i = start; i < parts.Length; i++)
            {
                var token = parts[i].Trim();
                if (NameKeywordToLayer.TryGetValue(token, out var layer)) return layer;
            }
            return Prop;
        }

        #endregion

        /// <summary>
        /// 类型键 → 层级目录/节点名（截图层级表命名）
        /// </summary>
        public static string GetLayerDir(string layerType)
        {
            switch (Normalize(layerType))
            {
                case Background: return DirBackground;
                case Greenery: return DirGreenery;
                case Road: return DirRoad;
                case Building: return DirBuildings;
                case Prop: return DirProps;
                case Character: return DirCharacters;
                case Foreground: return DirForeground;
                case Bubble: return DirBubbles;
                default: return DirProps;
            }
        }

        /// <summary>
        /// 类型键 → 渲染层级 SortOrder 槽位基线（自底向上，每层预留 LayerSlotCount 个层内排序档位）：
        /// background 0 / greenery 10 / road 20 / building 30 / prop 40 / character 50 / foreground 60 / bubble 70。
        /// 基线按 <see cref="MapToolConstants.LayerBaselineStep"/> 等差拉开；层内次序按 LayerOrderSeqStep 等差叠加；
        /// 同档物件共享基线并以脚底 Y 排序，超出档位容量的物件共享最高档，不越界压到相邻层。
        /// 遮挡主次：贴地绿化/道路在下，建筑压贴地层，道具/角色依次在前，前景近景最前，气泡置顶。
        /// </summary>
        public static int GetSortOrder(string layerType)
        {
            switch (Normalize(layerType))
            {
                case Background: return 0;
                case Greenery: return MapToolConstants.LayerBaselineStep * 1;
                case Road: return MapToolConstants.LayerBaselineStep * 2;
                case Building: return MapToolConstants.LayerBaselineStep * 3;
                case Prop: return MapToolConstants.LayerBaselineStep * 4;
                case Character: return MapToolConstants.LayerBaselineStep * 5;
                case Foreground: return MapToolConstants.LayerBaselineStep * 6;
                case Bubble: return MapToolConstants.LayerBaselineStep * 7;
                default: return MapToolConstants.LayerBaselineStep * 4;
            }
        }

        /// <summary>
        /// 编辑器枚举 → 归属层键：建筑归建筑层，人物/猫猫归角色·载具层，其余道具归道具层。
        /// 模板无独立显示档，默认归属层由类型推导，与实例层契约基线同一套八层划分。
        /// </summary>
        public static string ResolveLayerKey(MapObjectType type)
        {
            switch (type)
            {
                case MapObjectType.Building: return Building;
                case MapObjectType.Character:
                case MapObjectType.Cat: return Character;
                default: return Prop;
            }
        }

        /// <summary>编辑器枚举 → 层契约基线 SortOrder（八层划分，与实例全局键同坐标系）</summary>
        public static int GetSortOrder(MapObjectType type)
        {
            return GetSortOrder(ResolveLayerKey(type));
        }

        /// <summary>
        /// 解析有效显示层键：displayLayer 为空时回退归属层 layerType；传入值统一归一化。
        /// 独立显示档只影响渲染前后，不改变物件素材归属与挂载节点。
        /// </summary>
        /// <param name="layerType">归属层键（素材/挂载层）</param>
        /// <param name="displayLayer">显示层键（渲染档位，可为空）</param>
        /// <returns>归一化后的有效显示层键</returns>
        public static string ResolveDisplayLayer(string layerType, string displayLayer)
        {
            return string.IsNullOrWhiteSpace(displayLayer) ? Normalize(layerType) : Normalize(displayLayer);
        }

        /// <summary>
        /// 类型键 → 中文显示名（面板层级档下拉展示用）
        /// </summary>
        public static string GetLayerDisplayName(string layerType)
        {
            switch (Normalize(layerType))
            {
                case Background: return "背景";
                case Greenery: return "绿化";
                case Road: return "道路";
                case Building: return "建筑";
                case Prop: return "道具";
                case Character: return "角色/载具";
                case Foreground: return "前景";
                case Bubble: return "气泡";
                default: return "道具";
            }
        }

        /// <summary>
        /// 中文显示名 → 类型键（下拉回选用；无法识别回退道具层）
        /// </summary>
        public static string GetLayerKeyByDisplayName(string displayName)
        {
            if (string.IsNullOrEmpty(displayName)) return Prop;
            foreach (var key in AllLayerTypes)
            {
                if (GetLayerDisplayName(key) == displayName) return key;
            }
            return Prop;
        }

        /// <summary>
        /// 类型键 → 编辑器物体枚举（绿化/道路/前景/气泡归并到 Prop，避免破坏既有枚举分支）
        /// </summary>
        public static MapObjectType ResolveObjectType(string layerType)
        {
            switch (Normalize(layerType))
            {
                case Building: return MapObjectType.Building;
                case Character: return MapObjectType.Character;
                default: return MapObjectType.Prop;
            }
            // Greenery/Road/Foreground/Bubble 编辑器枚举归并到 Prop，层级区分由 LayerType 承载
        }

        /// <summary>
        /// 全局共享目录（跨章节复用，便于后续打图集）：Map/Shared/&lt;Layer&gt;/
        /// </summary>
        /// <param name="layerType">类型键</param>
        /// <returns>Assets 相对目录路径</returns>
        public static string GetGlobalSharedDir(string layerType)
        {
            return $"{MapToolConstants.MapTextureRoot}/Shared/{GetLayerDir(layerType)}";
        }

        /// <summary>
        /// 章节共享目录（同章节跨关卡复用）：Map/Chapter{ch}/Shared/&lt;Layer&gt;/
        /// </summary>
        /// <param name="chapterId">章节ID</param>
        /// <param name="layerType">类型键</param>
        /// <returns>Assets 相对目录路径</returns>
        public static string GetChapterSharedDir(string chapterId, string layerType)
        {
            return $"{MapToolConstants.MapTextureRoot}/Chapter{chapterId}/Shared/{GetLayerDir(layerType)}";
        }

        /// <summary>
        /// 关卡专属目录：Map/Chapter{ch}/Level{lv}/&lt;Layer&gt;/
        /// </summary>
        /// <param name="chapterId">章节ID</param>
        /// <param name="levelId">关卡ID</param>
        /// <param name="layerType">类型键</param>
        /// <returns>Assets 相对目录路径</returns>
        public static string GetLevelDir(string chapterId, string levelId, string layerType)
        {
            return $"{MapToolConstants.MapTextureRoot}/Chapter{chapterId}/Level{levelId}/{GetLayerDir(layerType)}";
        }
    }
}
