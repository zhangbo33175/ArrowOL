/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  AtlasLayoutCalculator.cs
 * author:    云毅
 * created:   2026
 * descrip:   Atlas 排布算法计算（MaxRects 算法，对齐 TexturePacker 规则）
 ***************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Honor.Editor.MapTool.AtlasPacker
{
    /// <summary>
    /// Atlas 排布算法计算器
    /// 负责计算最优排布位置，和预览/导出解耦
    /// </summary>
    internal class AtlasLayoutCalculator
    {
        // ===== 启发式策略编号（对齐 MaxRects 经典 5 种 Fit 策略）=====
        private const int StrategyShortSide = 0;   // Best Short Side Fit
        private const int StrategyLongSide = 1;    // Best Long Side Fit
        private const int StrategyAreaFit = 2;     // Best Area Fit
        private const int StrategyBottomLeft = 3;  // Bottom Left
        private const int StrategyContactPoint = 4; // Contact Point

        // ===== 打包前排序键编号（Best 模式下枚举各排序键，按选优分级挑最紧凑者）=====
        private const int SortMaxSide = 0;    // 长边降序 → 面积降序 → 名称（原默认）
        private const int SortArea = 1;       // 面积降序 → 长边降序 → 名称
        private const int SortLongSide = 2;   // 长边降序 → 名称
        private const int SortShortSide = 3;  // 短边降序 → 面积降序 → 名称
        private const int SortWidth = 4;      // 宽度降序 → 高度降序 → 名称
        private const int SortHeight = 5;     // 高度降序 → 宽度降序 → 名称

        // 纹理加载委托（由外部传入，避免依赖具体实现）
        public Func<string, Texture2D> TextureLoader { get; set; }

        /// <summary>
        /// 上一次排布中被排除（超尺寸 / 加载失败）的图片名，供外部提示
        /// </summary>
        public List<string> ExcludedNames { get; } = new List<string>();

        /// <summary>上一次参与排布的有效图片总面积（像素），失败时供提示用</summary>
        public long LastValidArea { get; private set; }

        /// <summary>上一次有效图片数，失败时供提示用</summary>
        public int LastValidCount { get; private set; }

        /// <summary>
        /// 计算最优排布
        /// </summary>
        /// <param name="items">所有图片项</param>
        /// <param name="maxW">最大宽度</param>
        /// <param name="maxH">最大高度</param>
        /// <param name="padding">边框间隙</param>
        /// <param name="shapePadding">图片间隙</param>
        /// <param name="allowRotation">是否允许图片旋转 90° 放置</param>
        /// <param name="heuristic">Heuristics 下拉值（Best 为遍历全部策略选最优）</param>
        /// <param name="atlasWidth">输出：最终图集宽度</param>
        /// <param name="atlasHeight">输出：最终图集高度</param>
        /// <param name="positions">输出：每个可放入图片的位置（与 sortedItems 对应）</param>
        /// <param name="innerPadding">图片内侧留白（并入图间间距）</param>
        /// <param name="divisorX">图集宽度对齐除数（≤1 不对齐）</param>
        /// <param name="divisorY">图集高度对齐除数（≤1 不对齐）</param>
        /// <param name="fixedW">固定图集宽度（0 表示自适应）</param>
        /// <param name="fixedH">固定图集高度（0 表示自适应）</param>
        /// <param name="forceSquare">强制图集宽高相等</param>
        /// <param name="packMode">Pack 方向偏好：Best / Width / Height</param>
        /// <param name="enableAlias">是否开启 auto alias：像素重复（含 90° 旋转等价）的精灵共享同一槽位，不重复占位</param>
        /// <param name="sizeConstraints">尺寸约束：POT (Power of 2) 向上取 2 的幂；NPOT 保持原始占用尺寸</param>
        /// <param name="forceWordAligned">强制图集宽度按 4 字节（RGBA8888 下 4 像素）对齐</param>
        /// <param name="extrude">精灵四周向外扩边像素数（占位与拷贝均预留该边距）</param>
        /// <returns>是否成功（至少有一张图可放入并排布完成）</returns>
        public bool CalculateBestLayout(
            List<SpriteItemInfo> items,
            int maxW, int maxH,
            int padding, int shapePadding,
            bool allowRotation, string heuristic,
            out int atlasWidth, out int atlasHeight,
            out List<Vector2Int> positions,
            int innerPadding = 0,
            int divisorX = 1, int divisorY = 1,
            int fixedW = 0, int fixedH = 0,
            bool forceSquare = false,
            string packMode = "Best",
            bool enableAlias = true,
            string sizeConstraints = "POT",
            bool forceWordAligned = false,
            int extrude = 0)
        {
            atlasWidth = 0;
            atlasHeight = 0;
            positions = null;
            ExcludedNames.Clear();

            if (items == null || items.Count == 0) return false;

            // 前置分类：把超图集尺寸、纹理加载失败的图排除，不参与排布（对齐 TexturePacker：单张失败不拖垮整批）
            var validItems = new List<SpriteItemInfo>();
            long totalArea = 0;
            int maxSide = 0;
            int contentW = maxW - padding * 2;
            int contentH = maxH - padding * 2;

            foreach (var item in items)
            {
                var tex = LoadTexture(item.Path);
                if (tex == null)
                {
                    item.Excluded = true;
                    item.ExclusionReason = "纹理加载失败";
                    item.PosX = 0;
                    item.PosY = 0;
                    item.Rotated = false;
                    ExcludedNames.Add(item.Name);
                    continue;
                }

                // 允许旋转时，超尺寸判定取宽高可互换后的最大值
                int fitW = allowRotation ? Math.Max(tex.width, tex.height) : tex.width;
                int fitH = allowRotation ? Math.Min(tex.width, tex.height) : tex.height;
                if (fitW > contentW || fitH > contentH)
                {
                    item.Excluded = true;
                    item.ExclusionReason = $"尺寸 {tex.width}x{tex.height} 超过图集上限 {contentW}x{contentH}";
                    item.PosX = 0;
                    item.PosY = 0;
                    item.Rotated = false;
                    ExcludedNames.Add(item.Name);
                    continue;
                }

                item.Excluded = false;
                item.ExclusionReason = null;
                item.Rotated = false;
                totalArea += (long)tex.width * tex.height;
                maxSide = Math.Max(maxSide, Math.Max(tex.width, tex.height));
                validItems.Add(item);
            }

            // auto alias：像素重复（含 90° 旋转等价）的精灵归为一组，仅主项参与排布，别名复用主项槽位。
            var layoutItems = BuildAliasGroups(validItems, enableAlias);

            // 容量估算只统计实际占位的主项（别名不重复占用空间）
            totalArea = 0;
            maxSide = 0;
            foreach (var item in layoutItems)
            {
                var tex = LoadTexture(item.Path);
                if (tex == null) continue;
                totalArea += (long)tex.width * tex.height;
                maxSide = Math.Max(maxSide, Math.Max(tex.width, tex.height));
            }

            LastValidArea = totalArea;
            LastValidCount = layoutItems.Count;

            if (layoutItems.Count == 0)
                return false;

            // 图片内侧留白并入图间间距（Inner Padding 在图四周额外留出空隙，视觉等价加大图间间距）
            int effectiveShapePadding = shapePadding + Math.Max(0, innerPadding);

            // 生成候选图集尺寸（宽×高 POT 组合：正方形 + 宽/高一档错位，按面积升序尝试）
            var candidateAtlases = GenerateCandidateAtlases(
                maxSide, totalArea, maxW, maxH, padding,
                fixedW, fixedH, forceSquare);

            // 打包前排序：枚举多种排序键（长边 / 面积 / 短边 / 宽 / 高），与 Fit 策略、候选尺寸组合后按选优分级挑最紧凑者。
            int[] sortKeys = ResolveSortKeys();
            var orderedByKey = new Dictionary<int, List<SpriteItemInfo>>();
            foreach (int sortKey in sortKeys)
            {
                orderedByKey[sortKey] = OrderItems(layoutItems, sortKey);
            }

            // 解析本次需要尝试的启发式策略集合
            int[] fitStrategies = ResolveStrategies(heuristic);

            int bestPacked = -1;
            long bestFinalArea = long.MaxValue;
            float bestSquareRatio = -1f;
            int bestTryW = 0;
            int bestTryH = 0;
            int bestFinalW = 0;
            int bestFinalH = 0;
            int bestUsedW = 0;
            int bestUsedH = 0;
            int bestStrategy = 0;
            int bestSortKey = SortMaxSide;
            List<Vector2Int> bestPositions = null;

            foreach (var atlas in candidateAtlases)
            {
                foreach (int sortKey in sortKeys)
                {
                    var sortedItems = orderedByKey[sortKey];
                    foreach (int strategy in fitStrategies)
                    {
                        foreach (var it in layoutItems) { it.Excluded = false; it.Rotated = false; }
                        ExcludedNames.Clear();

                        var pos = TryMaxRectsLayout(sortedItems, atlas.w, atlas.h, padding, effectiveShapePadding, allowRotation, strategy,
                            extrude, out int usedW, out int usedH);
                    if (pos == null) continue;

                    // 装入数 = 主项装入数 + 别名数（别名随主项共享槽位，视为已装入）
                    int packed = layoutItems.Count - ExcludedNames.Count + (validItems.Count - layoutItems.Count);
                    bool pot = !string.Equals(sizeConstraints, "NPOT", StringComparison.OrdinalIgnoreCase);
                    int rawW = pot ? NextPOT(usedW) : usedW;
                    int rawH = pot ? NextPOT(usedH) : usedH;
                    NormalizeAtlasSize(rawW, rawH, fixedW, fixedH, forceSquare, divisorX, divisorY, forceWordAligned,
                        out int finalW, out int finalH);
                    long area = (long)finalW * finalH;
                    // 方正度：短边/长边，越接近 1 越接近方形
                    float squareRatio = Math.Min(finalW, finalH) / (float)Math.Max(finalW, finalH);
                    long usedBBox = (long)usedW * usedH;

                    if (IsBetterLayout(packed, area, usedBBox, squareRatio, finalW, finalH, packMode,
                        bestPacked, bestFinalArea, (long)bestUsedW * bestUsedH, bestSquareRatio, bestFinalW, bestFinalH))
                    {
                        bestPacked = packed;
                        bestFinalArea = area;
                        bestSquareRatio = squareRatio;
                        bestTryW = atlas.w;
                        bestTryH = atlas.h;
                        bestFinalW = finalW;
                        bestFinalH = finalH;
                        bestUsedW = usedW;
                        bestUsedH = usedH;
                        bestStrategy = strategy;
                        bestSortKey = sortKey;
                        atlasWidth = finalW;
                        atlasHeight = finalH;
                        bestPositions = pos;
                    }
                    }
                }
            }

            // 用最终选定的参数（含最优排序键）重跑一次，确保 ExcludedNames、PosX/PosY、Rotated 正确
            var finalOrdered = orderedByKey[bestSortKey];
            foreach (var it in layoutItems) { it.Excluded = false; it.PosX = 0; it.PosY = 0; it.Rotated = false; }
            ExcludedNames.Clear();
            bestPositions = TryMaxRectsLayout(finalOrdered, bestTryW, bestTryH, padding, effectiveShapePadding, allowRotation, bestStrategy,
                extrude, out _, out _);

            if (bestPositions == null)
                return false;

            // 把排布位置与旋转标志写回图片项，避免外部二次按索引配对导致错位
            for (int i = 0; i < finalOrdered.Count && i < bestPositions.Count; i++)
            {
                finalOrdered[i].PosX = bestPositions[i].x;
                finalOrdered[i].PosY = bestPositions[i].y;
            }

            // 别名精灵复用主项槽位：把主项的坐标、旋转标志拷贝到别名项
            foreach (var item in validItems)
            {
                if (item.AliasOf == null) continue;
                item.PosX = item.AliasOf.PosX;
                item.PosY = item.AliasOf.PosY;
                item.Rotated = item.AliasOf.Rotated;
                item.Excluded = false;
                item.ExclusionReason = null;
            }

            positions = bestPositions;
            return true;
        }

        /// <summary>
        /// 按像素指纹对有效精灵分组：指纹相同（含 90° 旋转等价）的精灵只保留首张作为主项参与排布，
        /// 其余标记为别名并指向主项。返回实际参与排布的主项列表。
        /// </summary>
        private List<SpriteItemInfo> BuildAliasGroups(List<SpriteItemInfo> validItems, bool enableAlias)
        {
            var masters = new List<SpriteItemInfo>();
            if (!enableAlias)
            {
                foreach (var it in validItems) it.AliasOf = null;
                masters.AddRange(validItems);
                return masters;
            }

            var fingerprintToMaster = new Dictionary<string, SpriteItemInfo>();
            foreach (var item in validItems)
            {
                var tex = LoadTexture(item.Path);
                string[] keys = tex != null ? ComputeFingerprints(tex) : new[] { $"path:{item.Path}" };

                SpriteItemInfo master = null;
                foreach (var key in keys)
                {
                    if (fingerprintToMaster.TryGetValue(key, out var existing)) { master = existing; break; }
                }

                if (master != null)
                {
                    item.AliasOf = master;
                }
                else
                {
                    item.AliasOf = null;
                    foreach (var key in keys) fingerprintToMaster[key] = item;
                    masters.Add(item);
                }
            }
            return masters;
        }

        /// <summary>
        /// 计算纹理指纹：原图指纹 + 顺时针 90° 旋转等价指纹（方形图只取原图）。
        /// </summary>
        private string[] ComputeFingerprints(Texture2D tex)
        {
            var pixels = tex.GetPixels32();
            string normal = Md5Bytes(PixelsToBytes(pixels));
            if (tex.width == tex.height) return new[] { normal };

            int w = tex.width;
            int h = tex.height;
            var rotated = new Color32[pixels.Length];
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    // 顺时针 90°：源 (x,y) -> 目标 (nx=y, ny=w-1-x)，新图宽 h、高 w
                    rotated[(w - 1 - x) * h + y] = pixels[y * w + x];
                }
            }
            string rot = Md5Bytes(PixelsToBytes(rotated));
            return new[] { normal, rot };
        }

        /// <summary>Color32 数组转 RGBA 字节序列</summary>
        private static byte[] PixelsToBytes(Color32[] pixels)
        {
            var bytes = new byte[pixels.Length * 4];
            for (int i = 0; i < pixels.Length; i++)
            {
                bytes[i * 4] = pixels[i].r;
                bytes[i * 4 + 1] = pixels[i].g;
                bytes[i * 4 + 2] = pixels[i].b;
                bytes[i * 4 + 3] = pixels[i].a;
            }
            return bytes;
        }

        /// <summary>计算字节序列的 MD5 指纹（Base64）</summary>
        private static string Md5Bytes(byte[] data)
        {
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                return Convert.ToBase64String(md5.ComputeHash(data));
            }
        }

        /// <summary>
        /// 解析 Heuristics 下拉值，映射为需要尝试的策略编号集合
        /// </summary>
        private int[] ResolveStrategies(string heuristic)
        {
            if (string.IsNullOrEmpty(heuristic))
                return new[] { StrategyShortSide, StrategyLongSide, StrategyAreaFit, StrategyBottomLeft, StrategyContactPoint };

            switch (heuristic.Trim().ToLower())
            {
                case "shortside":
                case "minwidth":
                    return new[] { StrategyShortSide };
                case "longside":
                    return new[] { StrategyLongSide };
                case "minarea":
                case "areafit":
                    return new[] { StrategyAreaFit };
                case "maxaspect":
                case "bottomleft":
                    return new[] { StrategyBottomLeft };
                case "contactpoint":
                case "contact":
                    return new[] { StrategyContactPoint };
                default:
                    // Best 或未知值：遍历全部策略取最优
                    return new[] { StrategyShortSide, StrategyLongSide, StrategyAreaFit, StrategyBottomLeft, StrategyContactPoint };
            }
        }

        /// <summary>
        /// 参与枚举的打包前排序键集合（内部自动选优，不暴露到下拉）。
        /// </summary>
        private int[] ResolveSortKeys()
        {
            return new[] { SortMaxSide, SortArea, SortLongSide, SortShortSide, SortWidth, SortHeight };
        }

        /// <summary>
        /// 按指定排序键对主项列表排序；先投影缓存宽/高/面积，避免比较器内反复加载纹理。
        /// </summary>
        private List<SpriteItemInfo> OrderItems(List<SpriteItemInfo> source, int sortKey)
        {
            var probe = source.Select(it =>
            {
                var tex = LoadTexture(it.Path);
                int w = tex != null ? tex.width : 0;
                int h = tex != null ? tex.height : 0;
                return new
                {
                    Item = it,
                    W = w,
                    H = h,
                    Area = (long)w * h,
                    Max = Math.Max(w, h),
                    Min = Math.Min(w, h)
                };
            }).ToList();

            switch (sortKey)
            {
                case SortArea:
                    return probe.OrderByDescending(p => p.Area).ThenByDescending(p => p.Max)
                        .ThenBy(p => p.Item.Name, StringComparer.OrdinalIgnoreCase).Select(p => p.Item).ToList();
                case SortLongSide:
                    return probe.OrderByDescending(p => p.Max)
                        .ThenBy(p => p.Item.Name, StringComparer.OrdinalIgnoreCase).Select(p => p.Item).ToList();
                case SortShortSide:
                    return probe.OrderByDescending(p => p.Min).ThenByDescending(p => p.Area)
                        .ThenBy(p => p.Item.Name, StringComparer.OrdinalIgnoreCase).Select(p => p.Item).ToList();
                case SortWidth:
                    return probe.OrderByDescending(p => p.W).ThenByDescending(p => p.H)
                        .ThenBy(p => p.Item.Name, StringComparer.OrdinalIgnoreCase).Select(p => p.Item).ToList();
                case SortHeight:
                    return probe.OrderByDescending(p => p.H).ThenByDescending(p => p.W)
                        .ThenBy(p => p.Item.Name, StringComparer.OrdinalIgnoreCase).Select(p => p.Item).ToList();
                case SortMaxSide:
                default:
                    return probe.OrderByDescending(p => p.Max).ThenByDescending(p => p.Area)
                        .ThenBy(p => p.Item.Name, StringComparer.OrdinalIgnoreCase).Select(p => p.Item).ToList();
            }
        }

        /// <summary>
        /// 生成候选图集尺寸（内容可用宽×高）。
        /// 以 POT 档位为基础：正方形 S×S，外加宽一档 2S×S、高一档 S×2S，覆盖方形与横/竖矩形，按面积升序。
        /// Fixed size 给定宽/高时只产出固定尺寸；Force squared 时只产出正方形。
        /// </summary>
        private List<(int w, int h)> GenerateCandidateAtlases(
            int maxSide, long totalArea, int maxW, int maxH, int padding,
            int fixedW, int fixedH, bool forceSquare)
        {
            var result = new List<(int w, int h)>();
            var seen = new HashSet<long>();

            void AddAtlas(int w, int h)
            {
                if (w > maxW || h > maxH) return;
                int cw = w - padding * 2;
                int ch = h - padding * 2;
                if (cw < maxSide || ch < maxSide) return;
                long key = (long)cw * 1000003L + ch;
                if (seen.Add(key)) result.Add((cw, ch));
            }

            // 固定尺寸：直接使用给定宽高（0 的一侧仍自适应）
            if (fixedW > 0 || fixedH > 0)
            {
                int fw = fixedW > 0 ? fixedW : maxW;
                int fh = fixedH > 0 ? fixedH : maxH;
                AddAtlas(fw, fh);
                return result;
            }

            var sides = new List<int>();
            int startSide = NextPOT(Math.Max(maxSide + padding * 2, 64));
            int capSide = Math.Max(maxW, maxH);
            for (int s = startSide; s <= capSide; s *= 2)
            {
                sides.Add(s);
            }

            // 总面积估算的边长兜底，确保候选覆盖总面积容量档位
            int idealSide = NextPOT((int)Math.Ceiling(Math.Sqrt(totalArea)));
            if (sides.Count == 0 || idealSide > sides[sides.Count - 1])
            {
                sides.Add(idealSide);
            }

            foreach (int s in sides)
            {
                AddAtlas(s, s);
                if (!forceSquare)
                {
                    if (s * 2 <= maxW) AddAtlas(s * 2, s);
                    if (s * 2 <= maxH) AddAtlas(s, s * 2);
                }
            }

            return result.OrderBy(a => (long)a.w * a.h).ToList();
        }

        /// <summary>
        /// 规整最终图集尺寸：固定尺寸 → 强制方形 → 对齐到除数倍数。
        /// </summary>
        private void NormalizeAtlasSize(int rawW, int rawH, int fixedW, int fixedH, bool forceSquare,
            int divisorX, int divisorY, bool forceWordAligned, out int finalW, out int finalH)
        {
            finalW = fixedW > 0 ? fixedW : rawW;
            finalH = fixedH > 0 ? fixedH : rawH;

            if (forceSquare)
            {
                int side = Math.Max(finalW, finalH);
                finalW = side;
                finalH = side;
            }

            // Force word aligned：宽度按 4 字节对齐（RGBA8888 下每像素 4 字节，即 4 像素）
            if (forceWordAligned) finalW = AlignUp(finalW, 4);

            finalW = AlignUp(finalW, divisorX);
            finalH = AlignUp(finalH, divisorY);
        }

        /// <summary>
        /// 把数值向上对齐到除数的倍数（除数 ≤1 时原样返回）
        /// </summary>
        private int AlignUp(int value, int divisor)
        {
            if (divisor <= 1 || value <= 0) return value;
            int rem = value % divisor;
            return rem == 0 ? value : value + (divisor - rem);
        }

        /// <summary>
        /// 比较两组排布结果是否更优：装入数量优先 → POT 面积 → Pack 方向偏好 → 取向兜底（窄宽竖优先）→ 同取向同尺寸下比实际占用 bbox 面积（越小越紧凑）。
        /// </summary>
        private bool IsBetterLayout(
            int packed, long area, long usedBBox, float squareRatio, int finalW, int finalH, string packMode,
            int bestPacked, long bestArea, long bestBBox, float bestRatio, int bestW, int bestH)
        {
            if (packed != bestPacked) return packed > bestPacked;
            if (area != bestArea) return area < bestArea;

            // 先按 Pack 方向偏好（方正 / 宽 / 高）分流取向；偏好平手时按窄宽竖优先兜底定取向。
            float preference = PackPreference(finalW, finalH, packMode);
            float bestPreference = PackPreference(bestW, bestH, packMode);
            if (Math.Abs(preference - bestPreference) > 1e-6f) return preference > bestPreference;

            // 取向兜底：Height 模式优先高度更大；其余优先宽度更小（窄高竖条）。
            bool heightMode = string.Equals(packMode, "Height", StringComparison.OrdinalIgnoreCase);
            int orientCmp = heightMode ? finalH.CompareTo(bestH) : finalW.CompareTo(bestW);
            if (orientCmp != 0) return heightMode ? finalH > bestH : finalW < bestW;

            // 取向与尺寸完全相同时，再比较实际占用包围盒面积，越小越紧凑（内部缝隙/底部留白越少）。
            if (usedBBox != bestBBox) return usedBBox < bestBBox;

            return false;
        }

        /// <summary>
        /// 计算 Pack 方向偏好分：Best 越接近方形越高；Width 越宽越高；Height 越高越高。
        /// </summary>
        private float PackPreference(int w, int h, string packMode)
        {
            if (w <= 0 || h <= 0) return 0f;
            if (string.Equals(packMode, "Width", StringComparison.OrdinalIgnoreCase))
                return (float)w / h;
            if (string.Equals(packMode, "Height", StringComparison.OrdinalIgnoreCase))
                return (float)h / w;
            // Best：短边/长边，越接近 1 越方正
            return Math.Min(w, h) / (float)Math.Max(w, h);
        }

        /// <summary>
        /// MaxRects 排布算法
        /// </summary>
        /// <param name="items">已排序图片项</param>
        /// <param name="availWidth">候选图集可用内容宽度</param>
        /// <param name="availHeight">候选图集可用内容高度</param>
        /// <param name="padding">边框间隙</param>
        /// <param name="shapePadding">图片间隙</param>
        /// <param name="allowRotation">是否允许旋转 90°</param>
        /// <param name="fitStrategy">启发式策略编号</param>
        /// <param name="usedW">输出：实际占用宽度</param>
        /// <param name="usedH">输出：实际占用高度</param>
        private List<Vector2Int> TryMaxRectsLayout(
            List<SpriteItemInfo> items,
            int availWidth, int availHeight,
            int padding, int shapePadding,
            bool allowRotation,
            int fitStrategy,
            int extrude,
            out int usedW, out int usedH)
        {
            usedW = padding;
            usedH = padding;

            var freeRects = new List<RectInt>();
            freeRects.Add(new RectInt(padding, padding, availWidth, availHeight));
            var placedRects = new List<RectInt>();

            var positions = new List<Vector2Int>();

            foreach (var item in items)
            {
                var tex = LoadTexture(item.Path);
                if (tex == null)
                {
                    positions.Add(Vector2Int.zero);
                    continue;
                }

                // 朝向候选：原始朝向；允许旋转时追加 90° 旋转朝向
                var orientations = allowRotation
                    ? new[] { (w: tex.width, h: tex.height, rotated: false), (w: tex.height, h: tex.width, rotated: true) }
                    : new[] { (w: tex.width, h: tex.height, rotated: false) };

                int bestIndex = -1;
                bool bestRotated = false;
                int bestW = 0;
                int bestH = 0;
                float bestScore = float.MaxValue;
                RectInt bestRect = default;

                for (int o = 0; o < orientations.Length; o++)
                {
                    int ow = orientations[o].w + shapePadding + extrude * 2;
                    int oh = orientations[o].h + shapePadding + extrude * 2;

                    for (int i = 0; i < freeRects.Count; i++)
                    {
                        var rect = freeRects[i];
                        if (rect.width >= ow && rect.height >= oh)
                        {
                            int leftoverW = rect.width - ow;
                            int leftoverH = rect.height - oh;
                            float score = CalculateFitScore(leftoverW, leftoverH, rect, ow, oh, placedRects, fitStrategy);

                            if (score < bestScore)
                            {
                                bestScore = score;
                                bestIndex = i;
                                bestRotated = orientations[o].rotated;
                                bestW = ow;
                                bestH = oh;
                                bestRect = rect;
                            }
                        }
                    }
                }

                if (bestIndex < 0)
                {
                    item.Excluded = true;
                    item.ExclusionReason = "图集空间不足";
                    item.PosX = 0;
                    item.PosY = 0;
                    item.Rotated = false;
                    ExcludedNames.Add(item.Name);
                    positions.Add(Vector2Int.zero);
                    continue;
                }

                item.Rotated = bestRotated;
                positions.Add(new Vector2Int(bestRect.x, bestRect.y));
                placedRects.Add(new RectInt(bestRect.x, bestRect.y, bestW, bestH));
                usedW = Math.Max(usedW, bestRect.x + bestW);
                usedH = Math.Max(usedH, bestRect.y + bestH);

                SplitAndMergeFreeRects(freeRects, bestRect.x, bestRect.y, bestW, bestH);
            }

            return positions;
        }

        /// <summary>
        /// 计算评分分数（分数越低越优）
        /// </summary>
        private float CalculateFitScore(int leftoverW, int leftoverH, RectInt rect, int w, int h, List<RectInt> placedRects, int fitStrategy)
        {
            switch (fitStrategy)
            {
                case StrategyShortSide: // Best Short Side Fit
                    return Math.Min(leftoverW, leftoverH) * 1000f + Math.Max(leftoverW, leftoverH);
                case StrategyLongSide: // Best Long Side Fit
                    return Math.Max(leftoverW, leftoverH) * 1000f + Math.Min(leftoverW, leftoverH);
                case StrategyAreaFit: // Best Area Fit
                    return (leftoverW * leftoverH) + Math.Min(leftoverW, leftoverH) * 100;
                case StrategyBottomLeft: // Bottom Left
                    return rect.y * 10000f + rect.x;
                case StrategyContactPoint: // Contact Point：与已放置矩形贴边接触长度越大越好
                    float contact = CalculateContactLength(new RectInt(rect.x, rect.y, w, h), placedRects);
                    // 接触越多分数越低（越好），辅以短边优先
                    return -contact * 0.1f + Math.Min(leftoverW, leftoverH) * 1000f + Math.Max(leftoverW, leftoverH);
                default:
                    return Math.Min(leftoverW, leftoverH);
            }
        }

        /// <summary>
        /// 计算候选矩形与已放置矩形的贴边接触总长度（相邻边重叠跨度，非重叠面积）
        /// </summary>
        private float CalculateContactLength(RectInt candidate, List<RectInt> placedRects)
        {
            if (placedRects == null || placedRects.Count == 0) return 0f;

            float contact = 0;
            foreach (var p in placedRects)
            {
                // 左右贴边：候选左边贴 p 右边 / 候选右边贴 p 左边，统计 Y 方向重叠跨度
                if (candidate.x == p.xMax || candidate.xMax == p.x)
                {
                    int overlapY = Math.Min(candidate.yMax, p.yMax) - Math.Max(candidate.yMin, p.yMin);
                    if (overlapY > 0) contact += overlapY;
                }
                // 上下贴边：候选上边贴 p 下边 / 候选下边贴 p 上边，统计 X 方向重叠跨度
                if (candidate.y == p.yMax || candidate.yMax == p.y)
                {
                    int overlapX = Math.Min(candidate.xMax, p.xMax) - Math.Max(candidate.xMin, p.xMin);
                    if (overlapX > 0) contact += overlapX;
                }
            }
            return contact;
        }

        /// <summary>
        /// 分割并合并空闲矩形
        /// </summary>
        private void SplitAndMergeFreeRects(List<RectInt> freeRects, int splitX, int splitY, int splitW, int splitH)
        {
            var newFreeRects = new List<RectInt>();
            for (int i = freeRects.Count - 1; i >= 0; i--)
            {
                var rect = freeRects[i];
                if (!RectOverlaps(rect, splitX, splitY, splitW, splitH))
                {
                    newFreeRects.Add(rect);
                    continue;
                }
                SplitRect(rect, splitX, splitY, splitW, splitH, newFreeRects);
                freeRects.RemoveAt(i);
            }
            freeRects.AddRange(newFreeRects);
            MergeFreeRects(freeRects);
            // 按面积从小到大排序，优先使用小矩形
            freeRects.Sort((a, b) => (a.width * a.height).CompareTo(b.width * b.height));
        }

        /// <summary>
        /// 检测矩形是否重叠
        /// </summary>
        private bool RectOverlaps(RectInt a, int x, int y, int w, int h)
        {
            return a.x < x + w && a.x + a.width > x &&
                   a.y < y + h && a.y + a.height > y;
        }

        /// <summary>
        /// 分割矩形
        /// </summary>
        private void SplitRect(RectInt rect, int splitX, int splitY, int splitW, int splitH, List<RectInt> result)
        {
            // 左边剩余
            if (splitX > rect.x)
            {
                result.Add(new RectInt(rect.x, rect.y, splitX - rect.x, rect.height));
            }
            // 右边剩余
            if (splitX + splitW < rect.x + rect.width)
            {
                result.Add(new RectInt(splitX + splitW, rect.y, rect.x + rect.width - splitX - splitW, rect.height));
            }
            // 上边剩余
            if (splitY > rect.y)
            {
                result.Add(new RectInt(rect.x, rect.y, rect.width, splitY - rect.y));
            }
            // 下边剩余
            if (splitY + splitH < rect.y + rect.height)
            {
                result.Add(new RectInt(rect.x, splitY + splitH, rect.width, rect.y + rect.height - splitY - splitH));
            }
        }

        /// <summary>
        /// 合并相邻的空闲矩形并去重
        /// </summary>
        private void MergeFreeRects(List<RectInt> freeRects)
        {
            // 先去掉被其他矩形完全包含的矩形
            for (int i = freeRects.Count - 1; i >= 0; i--)
            {
                var a = freeRects[i];
                for (int j = 0; j < freeRects.Count; j++)
                {
                    if (i == j) continue;
                    var b = freeRects[j];
                    if (a.x >= b.x && a.y >= b.y &&
                        a.x + a.width <= b.x + b.width &&
                        a.y + a.height <= b.y + b.height)
                    {
                        freeRects.RemoveAt(i);
                        break;
                    }
                }
            }

            // 再合并相邻的矩形
            for (int i = 0; i < freeRects.Count; i++)
            {
                for (int j = i + 1; j < freeRects.Count; j++)
                {
                    var a = freeRects[i];
                    var b = freeRects[j];
                    // 水平合并
                    if (a.height == b.height && a.y == b.y && a.x + a.width == b.x)
                    {
                        freeRects[i] = new RectInt(a.x, a.y, a.width + b.width, a.height);
                        freeRects.RemoveAt(j);
                        j--;
                    }
                    // 垂直合并
                    else if (a.width == b.width && a.x == b.x && a.y + a.height == b.y)
                    {
                        freeRects[i] = new RectInt(a.x, a.y, a.width, a.height + b.height);
                        freeRects.RemoveAt(j);
                        j--;
                    }
                }
            }
        }

        /// <summary>
        /// 计算下一个 2 的幂（POT 对齐）
        /// </summary>
        private int NextPOT(int value)
        {
            if (value <= 0) return 1;
            value--;
            value |= value >> 1;
            value |= value >> 2;
            value |= value >> 4;
            value |= value >> 8;
            value |= value >> 16;
            return value + 1;
        }

        /// <summary>
        /// 加载纹理（通过委托）
        /// </summary>
        private Texture2D LoadTexture(string assetPath)
        {
            return TextureLoader?.Invoke(assetPath);
        }
    }
}
