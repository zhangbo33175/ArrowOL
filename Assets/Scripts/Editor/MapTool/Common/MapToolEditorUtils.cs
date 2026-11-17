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
                string absolutePath = MapToolPathHelper.ToAbsolutePath(tableRelativePath);
                MapToolLog.Info($"加载配置表: {absolutePath}");
                return TableExportEditorUtility.GetExcelData(absolutePath).ToList<T>();
            }
            catch (System.Exception e)
            {
                MapToolLog.Error($"加载配置表失败 {tableRelativePath}: {e.Message}");
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
            string luaPath = MapToolPathHelper.ToAbsolutePath(MapToolConstants.RuntimeChaptersLuaPath);
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
                MapToolLog.Error($"加载运行时章节表失败: {e.Message}");
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
        /// 拆分资源键（格式：TemplateId_TemplateName）：首个下划线前为模板ID，其余为名称；
        /// 无下划线（或下划线在首位）时 ID 与名称同为原字符串。
        /// 左侧资源列表选中、配置表回填等多处共用同一拆分口径。
        /// </summary>
        /// <param name="key">资源键</param>
        /// <param name="templateId">输出：模板ID</param>
        /// <param name="templateName">输出：模板名称</param>
        public static void SplitTemplateKey(string key, out string templateId, out string templateName)
        {
            templateId = key ?? string.Empty;
            templateName = key ?? string.Empty;
            if (string.IsNullOrEmpty(key)) return;

            int underscoreIndex = key.IndexOf('_');
            if (underscoreIndex <= 0) return;

            templateId = key.Substring(0, underscoreIndex);
            templateName = key.Substring(underscoreIndex + 1);
        }

        /// <summary>
        /// 获取物体类型的中文名称
        /// </summary>
        /// <param name="type">物体类型</param>
        /// <returns>中文类型名</returns>
        public static string FormatObjectTypeName(MapObjectType type)
        {
            return ObjectTypeNames.Of(type);
        }

        /// <summary>
        /// 获取物体类型的中文名称；载具（汽车等）虽走角色交通链路，显示上单独标注为「载具」，不并入人物。
        /// </summary>
        /// <param name="type">物体类型</param>
        /// <param name="isVehicle">是否为载具</param>
        /// <returns>中文类型名</returns>
        public static string FormatObjectTypeName(MapObjectType type, bool isVehicle)
        {
            if (isVehicle) return MapToolConstants.TrafficRoleVehicleDisplay;
            return FormatObjectTypeName(type);
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

        #region 层级遮挡排序公共口径（编辑器绘制 / 命中 / 刷新层级 / 预览窗口共用）

        /// <summary>
        /// 计算物件基础像素尺寸（未乘缩放倍率）。
        /// 尺寸优先级：预览贴图原始尺寸 → PSD 像素宽高 → 格子数 × 网格尺寸；
        /// 绘制、命中、排序三处共用本口径，避免命中框与可见图大小不一致。
        /// </summary>
        /// <param name="obj">摆放物件</param>
        /// <param name="width">输出：基础像素宽</param>
        /// <param name="height">输出：基础像素高</param>
        public static void GetObjectPixelSize(PlacedObject obj, out float width, out float height)
        {
            if (obj.PreviewTexture != null)
            {
                width = obj.PreviewTexture.width;
                height = obj.PreviewTexture.height;
            }
            else if (obj.PixelWidth > 0 && obj.PixelHeight > 0)
            {
                width = obj.PixelWidth;
                height = obj.PixelHeight;
            }
            else
            {
                width = obj.Width * MapToolConstants.GridSize;
                height = obj.Height * MapToolConstants.GridSize;
            }
        }

        /// <summary>
        /// 计算物件脚底 Y 坐标（左上原点坐标系：脚底 Y = 顶部 Y + 高度）。
        /// 脚底 Y 越大越靠下，YSort 中越靠下（近处）渲染在上层。
        /// </summary>
        /// <param name="obj">摆放物件</param>
        /// <returns>脚底 Y 像素值</returns>
        public static float GetObjectBottomY(PlacedObject obj)
        {
            GetObjectPixelSize(obj, out _, out float height);
            return obj.Position.y + height;
        }

        /// <summary>
        /// 解析物件有效显示层基线 SortOrder：DisplayLayer 为空时回退归属层 LayerType。
        /// 与运行时 CharacterGenerationManager 的显示档解析口径一致。
        /// </summary>
        /// <param name="obj">摆放物件</param>
        /// <returns>层契约基线 SortOrder</returns>
        public static int GetDisplayBaseline(PlacedObject obj)
        {
            string effectiveLayer = MapLayerCatalog.ResolveDisplayLayer(obj.LayerType, obj.DisplayLayer);
            return MapLayerCatalog.GetSortOrder(effectiveLayer);
        }

        /// <summary>
        /// 把层内槽位叠回全局键：[基线, 基线 + (LayerSlotCount-1) * step]
        /// 层内档位换算的唯一实现，避免各方法各自写「baseline + slot」
        /// </summary>
        /// <param name="baseline">层契约基线 SortOrder</param>
        /// <param name="slot">层内槽位</param>
        /// <param name="step">槽位等差步长（默认 1，uGUI 全局键传 LayerOrderSeqStep）</param>
        /// <returns>全局 SortOrder 键</returns>
        private static int GlobalFromSlot(int baseline, int slot, int step = 1)
        {
            return baseline + slot * step;
        }

        /// <summary>
        /// 从全局键抽取层内槽位（相对指定基线，钳到 [0, LayerSlotCount-1]）
        /// </summary>
        /// <param name="baseline">该全局键所属层的基线 SortOrder</param>
        /// <param name="globalOrder">全局 SortOrder 键</param>
        /// <returns>层内槽位</returns>
        private static int SlotFromGlobal(int baseline, int globalOrder)
        {
            return Mathf.Clamp(globalOrder - baseline, 0, MapToolConstants.LayerSlotCount - 1);
        }

        /// <summary>
        /// 把全局键钳制到本层槽位区间 [基线, 基线 + LayerSlotCount - 1]
        /// </summary>
        /// <param name="baseline">层契约基线 SortOrder</param>
        /// <param name="globalOrder">待校验的 SortOrder</param>
        /// <returns>钳制后的合法 SortOrder</returns>
        private static int ClampToLayerSlot(int baseline, int globalOrder)
        {
            return Mathf.Clamp(globalOrder, baseline, baseline + MapToolConstants.LayerSlotCount - 1);
        }

        /// <summary>
        /// 把层内手动 SortOrder 钳制到所属显示层档位区间：[基线, 基线 + LayerSlotCount - 1]，
        /// 避免手填越界值压到相邻层基线。
        /// </summary>
        /// <param name="obj">摆放物件</param>
        /// <param name="order">待校验的层内档位值</param>
        /// <returns>钳制后的合法 SortOrder</returns>
        public static int ClampSortOrderToSlot(PlacedObject obj, int order)
        {
            return ClampToLayerSlot(GetDisplayBaseline(obj), order);
        }

        /// <summary>取模板类型对应归属层基线 SortOrder（模板无独立显示档，基线由类型推导）</summary>
        public static int GetTemplateBaseline(ObjectTemplate template)
        {
            return MapLayerCatalog.GetSortOrder(template.Type);
        }

        /// <summary>把模板 SortOrder 钳到所属类型层区间 [基线, 基线+LayerSlotCount-1]，避免压到相邻层基线</summary>
        public static int ClampTemplateSortOrder(ObjectTemplate template, int order)
        {
            return ClampToLayerSlot(GetTemplateBaseline(template), order);
        }

        /// <summary>
        /// 旧版 100 步长全局基线表（0/100/200/…/700）→ 新版 10 步长基线表的层序映射。
        /// 旧存量 JSON 落盘的是旧全局键，读取时按旧基线所在层序回落到当前物件归属/显示层基线，避免旧大值越界压层。
        /// </summary>
        private static readonly int[] LegacyBaselines = { 0, 100, 200, 300, 400, 500, 600, 700 };

        /// <summary>
        /// 是否启用动画播放（有动画名且不是「无动画」占位值）
        /// 序列化落盘与运行时判定共用同一口径
        /// </summary>
        /// <param name="obj">摆放物件</param>
        /// <returns>启用返回 true</returns>
        public static bool IsAnimEnabled(PlacedObject obj)
        {
            if (obj == null) return false;
            return !string.IsNullOrEmpty(obj.AnimName) && obj.AnimName != MapToolConstants.NoAnimName;
        }

        /// <summary>
        /// 尝试把旧版 100 步长全局 SortOrder 键解析为层内槽位
        /// </summary>
        /// <param name="raw">原始 SortOrder</param>
        /// <param name="slot">命中时输出层内槽位</param>
        /// <returns>是否命中旧版基线区间</returns>
        private static bool TryMatchLegacySlot(int raw, out int slot)
        {
            for (int i = 0; i < LegacyBaselines.Length; i++)
            {
                if (raw >= LegacyBaselines[i] && raw < LegacyBaselines[i] + MapToolConstants.LayerSlotCount * 10)
                {
                    slot = raw - LegacyBaselines[i];
                    return true;
                }
            }
            slot = 0;
            return false;
        }

        /// <summary>
        /// 把旧版 100 步长全局 SortOrder 键换算到新版 10 步长坐标系：
        /// ① 命中旧基线（含其层内槽位）时，按旧层序映射到当前物件所属层新基线，层内槽位钳到 LayerSlotCount；
        /// ② 已是新坐标系的值则直接钳到当前层有效槽位区间；无法判定层序的裸小值按层内槽位处理。
        /// </summary>
        /// <param name="obj">摆放物件（提供当前归属/显示层基线）</param>
        /// <param name="raw">读取到的原始 SortOrder</param>
        /// <returns>新坐标系下合法的 SortOrder</returns>
        public static int NormalizeLegacySortOrder(PlacedObject obj, int raw)
        {
            int baseline = GetDisplayBaseline(obj);
            if (!TryMatchLegacySlot(raw, out int slot))
            {
                // 非旧全局键：视为层内裸槽位（旧三档值/新层内小值），直接钳到层内槽位区间
                slot = raw;
            }
            slot = Mathf.Clamp(slot, 0, MapToolConstants.LayerSlotCount - 1);
            return GlobalFromSlot(baseline, slot, MapToolConstants.LayerOrderSeqStep);
        }

        /// <summary>
        /// 旧口径裸值归一为模板全局键：旧值小于层槽位容量视为层内槽位叠到类型基线；
        /// 已是全局键则按模板基线钳制，与实例八层全局键同坐标系。
        /// </summary>
        public static int NormalizeTemplateSortOrder(ObjectTemplate template, int raw)
        {
            return NormalizeTemplateSortOrder(template.Type, raw);
        }

        /// <summary>按类型基线归一旧口径裸值为模板全局键（重载，供尚未绑定模板对象的新建流程使用）</summary>
        public static int NormalizeTemplateSortOrder(MapObjectType type, int raw)
        {
            int baseline = MapLayerCatalog.GetSortOrder(type);
            // 旧 100 步长全局键：按旧基线识别其层内槽位，避免旧大值被误当成裸槽位
            if (!TryMatchLegacySlot(raw, out int slot))
            {
                slot = raw < MapToolConstants.LayerSlotCount ? raw : raw - baseline;
            }
            slot = Mathf.Clamp(slot, 0, MapToolConstants.LayerSlotCount - 1);
            return baseline + slot;
        }

        /// <summary>
        /// 模板全局键 → 实例全局键：抽取模板层内槽位，叠到实例所属显示档基线；
        /// 模板归属层与实例显示档不同时，保留层内相对次序，不把模板基线串进实例层。
        /// </summary>
        public static int SortOrderFromTemplate(PlacedObject obj, ObjectTemplate template)
        {
            int slot = SlotFromGlobal(GetTemplateBaseline(template), template.SortOrder);
            return GlobalFromSlot(GetDisplayBaseline(obj), slot);
        }

        /// <summary>
        /// 实例全局键 → 模板全局键：抽取实例层内槽位，叠到模板类型基线；
        /// 回写模板时不把实例显示档基线带进模板层。
        /// </summary>
        public static int SortOrderFromInstance(ObjectTemplate template, PlacedObject obj)
        {
            int slot = SlotFromGlobal(GetDisplayBaseline(obj), obj.SortOrder);
            return GlobalFromSlot(GetTemplateBaseline(template), slot);
        }

        /// <summary>
        /// 计算物件在 uGUI 场景实例上使用的全局排序键：层契约基线 + 层内槽位等差步长。
        /// 独立 overrideSorting Canvas 按 sortingOrder 全局比较，必须使用跨层可比的全局键，
        /// 不能直接用层内小值或旧三档值；与 IMGUI 三级键第一、二键口径一致。
        /// </summary>
        /// <param name="obj">摆放物件</param>
        /// <returns>跨层可比的全局 SortOrder 键</returns>
        public static int GetGlobalSortOrder(PlacedObject obj)
        {
            int baseline = GetDisplayBaseline(obj);
            int slot = SlotFromGlobal(baseline, obj.SortOrder);
            return GlobalFromSlot(baseline, slot, MapToolConstants.LayerOrderSeqStep);
        }

        #endregion

        /// <summary>
        /// 扫描指定目录下的纹理资源路径（AssetDatabase 查询 + 扩展名过滤，按路径升序）
        /// 场景/资源整理/图集三处的「扫目录取图片」统一入口
        /// </summary>
        /// <param name="folder">Assets 相对目录</param>
        /// <param name="extensions">允许的扩展名（小写含点，取 MapToolConstants.ImageExtensions 或单个 Ext*）</param>
        /// <returns>资源路径列表；目录无效时返回空列表</returns>
        public static List<string> ScanTextureAssets(string folder, params string[] extensions)
        {
            var result = new List<string>();
            if (string.IsNullOrEmpty(folder) || !AssetDatabase.IsValidFolder(folder)) return result;

            string[] guids = AssetDatabase.FindAssets(MapToolConstants.TextureAssetFilter, new[] { folder });
            foreach (var guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                if (string.IsNullOrEmpty(path)) continue;

                if (extensions != null && extensions.Length > 0)
                {
                    string ext = Path.GetExtension(path).ToLowerInvariant();
                    if (System.Array.IndexOf(extensions, ext) < 0) continue;
                }
                result.Add(path);
            }

            result.Sort(System.StringComparer.Ordinal);
            return result;
        }

        /// <summary>
        /// 按文件名在资源目录中查找纹理（递归搜索，支持 Sprite 类型）
        /// </summary>
        /// <param name="imageName">图片文件名（不含扩展名）</param>
        /// <returns>纹理对象，未找到返回 null</returns>
        public static Texture LoadTextureByName(string imageName)
        {
            if (string.IsNullOrEmpty(imageName)) return null;

            string[] guids = AssetDatabase.FindAssets(
                $"{imageName} {MapToolConstants.TextureAssetFilter}",
                new[] { MapToolConstants.ResourceScanDir });

            if (guids.Length == 0) return null;

            string texPath = AssetDatabase.GUIDToAssetPath(guids[0]);
            // 先尝试 Texture2D，失败再尝试 Sprite
            Texture tex = AssetDatabase.LoadAssetAtPath<Texture2D>(texPath);
            if (tex == null)
            {
                var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(texPath);
                if (sprite != null) tex = sprite.texture;
            }
            return tex;
        }

        /// <summary>
        /// 根据图片文件名前缀判断物体类型
        /// </summary>
        /// <param name="imageName">图片文件名（不含扩展名）</param>
        /// <returns>物体类型，无法识别返回 Prop</returns>
        public static MapObjectType GetObjectTypeByImageName(string imageName)
        {
            if (string.IsNullOrEmpty(imageName)) return MapObjectType.Prop;

            string lowerName = imageName.ToLower();
            if (lowerName.StartsWith(MapToolConstants.PrefixBuilding)) return MapObjectType.Building;
            if (lowerName.StartsWith(MapToolConstants.PrefixCharacter)) return MapObjectType.Character;
            if (lowerName.StartsWith(MapToolConstants.PrefixCat)) return MapObjectType.Cat;
            if (lowerName.StartsWith(MapToolConstants.PrefixBg)) return MapObjectType.Prop; // 背景图归为 Prop
            return MapObjectType.Prop;
        }

        /// <summary>
        /// 按图片名前缀取资源类型显示名（背景 / 建筑 / 人物 / 物件 / 猫猫；无法识别返回「未知」）
        /// 分类前缀与显示名共用 s_CategoryRules 规则表，避免各处重复写前缀字面量
        /// </summary>
        /// <param name="fileNameOrPath">图片文件名或资产路径</param>
        /// <returns>中文类型显示名</returns>
        public static string GetResourceTypeDisplayName(string fileNameOrPath)
        {
            string fileName = GetFileNameOnly(fileNameOrPath);
            foreach (var rule in s_CategoryRules)
            {
                if (fileName.StartsWith(rule.Prefix)) return rule.Display;
            }
            return ObjectTypeNames.Unknown;
        }

        /// <summary>
        /// 按资源分类过滤文件名（未配置资源列表用；与配置表分类口径分开：这里只按文件名前缀判断）
        /// </summary>
        /// <param name="fileNameOrPath">文件名（不含扩展名）或资产路径</param>
        /// <param name="filter">分类键（MapToolConstants.FilterXxx）</param>
        /// <returns>命中返回 true；FilterAll 恒为 true</returns>
        public static bool MatchesCategoryPrefix(string fileNameOrPath, string filter)
        {
            if (filter == MapToolConstants.FilterAll) return true;

            string fileName = GetFileNameOnly(fileNameOrPath);
            foreach (var rule in s_CategoryRules)
            {
                if (rule.Filter == filter) return fileName.StartsWith(rule.Prefix);
            }
            return true;
        }

        /// <summary>
        /// 资源分类规则表：筛选键 → 文件名前缀 → 中文显示名
        /// 前缀过滤（MatchesCategoryPrefix）与显示名解析（GetResourceTypeDisplayName）共用同一张表，
        /// 新增/调整分类只改这一处，避免两处 switch 分支漂移
        /// </summary>
        private static readonly (string Filter, string Prefix, string Display)[] s_CategoryRules =
        {
            (MapToolConstants.FilterBg, MapToolConstants.PrefixBg, ObjectTypeNames.Background),
            (MapToolConstants.FilterBuilding, MapToolConstants.PrefixBuilding, ObjectTypeNames.Building),
            (MapToolConstants.FilterCharacter, MapToolConstants.PrefixCharacter, ObjectTypeNames.Character),
            (MapToolConstants.FilterProp, MapToolConstants.PrefixProp, ObjectTypeNames.Prop),
            (MapToolConstants.FilterCat, MapToolConstants.PrefixCat, ObjectTypeNames.Cat),
        };

        /// <summary>
        /// 取文件名（不含目录与扩展名）
        /// </summary>
        /// <param name="fileNameOrPath">文件名或路径</param>
        /// <returns>文件名</returns>
        private static string GetFileNameOnly(string fileNameOrPath)
        {
            if (string.IsNullOrEmpty(fileNameOrPath)) return string.Empty;

            // 统一走 Path 口径（分隔符与扩展名裁剪只有一套实现）
            return Path.GetFileNameWithoutExtension(fileNameOrPath).ToLowerInvariant();
        }

        /// <summary>
        /// 根据物体类型获取对应的配置表文件名
        /// </summary>
        /// <param name="type">物体类型</param>
        /// <returns>配置表文件名</returns>
        public static string GetConfigFileNameByType(MapObjectType type)
        {
            switch (type)
            {
                case MapObjectType.Building: return MapToolConstants.TableBuildingFileName;
                case MapObjectType.Character: return MapToolConstants.TableCharacterFileName;
                case MapObjectType.Prop: return MapToolConstants.TablePropFileName;
                case MapObjectType.Cat: return MapToolConstants.TableCatFileName;
                default: return MapToolConstants.TablePropFileName;
            }
        }

        #region 网格换算（借鉴 Tilemap GridLayout：世界像素 ↔ 格子坐标，全编辑器统一口径）

        /// <summary>世界坐标 → 格子坐标（左下取整，等价 Tilemap CellToWorld 的逆运算）</summary>
        /// <param name="worldPos">世界像素坐标</param>
        /// <returns>格子行列坐标（gx, gy）</returns>
        public static Vector2Int WorldToCell(Vector2 worldPos)
        {
            int gx = Mathf.FloorToInt(worldPos.x / MapToolConstants.GridSize);
            int gy = Mathf.FloorToInt(worldPos.y / MapToolConstants.GridSize);
            return new Vector2Int(gx, gy);
        }

        /// <summary>格子坐标 → 世界坐标（返回格子左下角世界像素）</summary>
        /// <param name="cell">格子行列坐标</param>
        /// <returns>格子左下角世界像素坐标</returns>
        public static Vector2 CellToWorld(Vector2Int cell)
        {
            return new Vector2(cell.x * MapToolConstants.GridSize, cell.y * MapToolConstants.GridSize);
        }

        /// <summary>世界坐标吸附到最近格子（Round 取整，等价 Tilemap 网格吸附）</summary>
        /// <param name="worldPos">原始世界像素坐标</param>
        /// <returns>吸附后的世界像素坐标</returns>
        public static Vector2 SnapToGrid(Vector2 worldPos)
        {
            return new Vector2(
                Mathf.Round(worldPos.x / MapToolConstants.GridSize) * MapToolConstants.GridSize,
                Mathf.Round(worldPos.y / MapToolConstants.GridSize) * MapToolConstants.GridSize);
        }

        /// <summary>按地图像素宽高计算网格列数（不足一格按 0 计）</summary>
        public static int GridCols(int mapWidth)
        {
            return mapWidth > 0 ? mapWidth / MapToolConstants.GridSize : 0;
        }

        /// <summary>按地图像素宽高计算网格行数（不足一格按 0 计）</summary>
        public static int GridRows(int mapHeight)
        {
            return mapHeight > 0 ? mapHeight / MapToolConstants.GridSize : 0;
        }

        /// <summary>格子坐标编码为去重键（gy * CellEncodeMultiplier + gx）</summary>
        public static long EncodeCell(int gx, int gy)
        {
            return ((long)gy) * MapToolConstants.CellEncodeMultiplier + gx;
        }

        /// <summary>
        /// 计算等距菱形（45° 斜格）网格的格点中心世界坐标（未缩放、未偏移）。
        /// 格点与正交模式格子中心一一对应，落盘格子坐标口径不变；仅供编辑器背景网格视觉辅助。
        /// </summary>
        /// <param name="mapWidth">地图总宽度（像素）</param>
        /// <param name="mapHeight">地图总高度（像素）</param>
        /// <returns>格点中心世界坐标数组</returns>
        public static Vector2[] ComputeDiamondCellCenters(int mapWidth, int mapHeight)
        {
            float cell = MapToolConstants.GridSize;
            if (cell <= 0f || mapWidth <= 0 || mapHeight <= 0) return System.Array.Empty<Vector2>();

            int cols = Mathf.Max(1, Mathf.CeilToInt(mapWidth / cell));
            int rows = Mathf.Max(1, Mathf.CeilToInt(mapHeight / cell));
            var list = new List<Vector2>(cols * rows);
            for (int gy = 0; gy < rows; gy++)
            {
                for (int gx = 0; gx < cols; gx++)
                {
                    list.Add(new Vector2(gx * cell + cell * 0.5f,
                                         gy * cell + cell * 0.5f));
                }
            }
            return list.ToArray();
        }

        /// <summary>统计一组摆放物件占用的去重格子键集合（按物件宽高跨格展开）</summary>
        /// <param name="objects">摆放物件列表</param>
        /// <returns>去重后的格子键集合</returns>
        public static HashSet<long> CollectOccupiedCells(IEnumerable<PlacedObject> objects)
        {
            var keys = new HashSet<long>();
            if (objects == null) return keys;
            foreach (var obj in objects)
            {
                if (obj == null) continue;
                Vector2Int cell = WorldToCell(obj.Position);
                int spanX = Mathf.Max(1, obj.Width);
                int spanY = Mathf.Max(1, obj.Height);
                for (int dy = 0; dy < spanY; dy++)
                {
                    for (int dx = 0; dx < spanX; dx++)
                    {
                        keys.Add(EncodeCell(cell.x + dx, cell.y + dy));
                    }
                }
            }
            return keys;
        }

        #endregion
    }
}
