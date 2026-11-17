/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  SceneEditModule.Data.cs
 * author:    云毅
 * created:   2026
 * descrip:   场景编辑模块 - 私有方法（配置表加载/分区计算/JSON数据构建）
 ***************************************************************/

using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Editor.MapEditor;
using System.Linq;

namespace Honor.Editor.MapTool
{
    /// <summary>
    /// 场景编辑模块 - 数据加载与构建私有方法
    /// </summary>
    public partial class SceneEditModule
    {
        /// <summary>
        /// 构建建筑 / 物件序列化数据
        /// </summary>
        /// <param name="obj">已放置物体</param>
        /// <returns>JSON 数据对象</returns>
        private MapObjectData BuildMapObjectData(PlacedObject obj)
        {
            var cell = MapToolEditorUtils.WorldToCell(obj.Position); // ---TODO 网格信息：格子坐标（一次计算，供 gridX/gridY 共用）
            return new MapObjectData
            {
                id = obj.Id,
                templateId = obj.TemplateId,
                name = obj.Name,
                posX = obj.Position.x,
                posY = obj.Position.y,
                gridX = cell.x, // ---TODO 网格信息：格子坐标
                gridY = cell.y, // ---TODO 网格信息：格子坐标
                width = obj.Width,
                height = obj.Height,
                pixelWidth = obj.PixelWidth,
                pixelHeight = obj.PixelHeight,
                stretch = obj.Stretch,
                layerType = obj.LayerType,
                displayLayer = obj.DisplayLayer,
                image = MapToolEditorUtils.GetObjectImagePath(obj.PreviewTexture, obj.Id),
                anim = obj.AnimName,
                animEnabled = MapToolEditorUtils.IsAnimEnabled(obj),
                loop = obj.Loop,
                speed = obj.Speed,
                interactable = obj.Interactable,
                interactType = obj.InteractType,
                sortOrder = obj.SortOrder,
                ySortEnabled = obj.YSortEnabled,
                ySortOffset = obj.YSortOffset,
                rotation = obj.Rotation,
                scale = obj.Scale,
                collisionEnabled = obj.CollisionEnabled,
                dialogBubble = obj.DialogBubble,
                shadowEnabled = obj.ShadowEnabled,
                isCollectible = obj.IsCollectible,
                respawnTime = obj.RespawnTime,
                bubbleId = obj.bubbleId, // ---TODO 气泡配置表通用化：物件/建筑引用 Tables.Bubble
                isMovable = obj.IsMovable,
                loopRestartDelay = obj.LoopRestartDelay,
                hideWhenOffMap = obj.HideWhenOffMap,
                isTeleport = obj.IsTeleport,
                teleportTargetScene = obj.TeleportTargetScene,
                teleportTargetX = obj.TeleportTargetX,
                teleportTargetY = obj.TeleportTargetY,
            };
        }

        /// <summary>
        /// 归一绑定灯 ID 列表：去空去重；旧单值字段兜底补入；返回独立新列表
        /// </summary>
        /// <param name="ids">多选灯 ID 列表（可空）</param>
        /// <param name="legacyId">旧版单值绑定灯 ID（可空）</param>
        /// <returns>归一后的灯 ID 列表</returns>
        private static List<string> NormalizeBoundLightIds(List<string> ids, string legacyId)
        {
            var result = new List<string>();
            if (ids != null)
            {
                foreach (var id in ids)
                {
                    if (!string.IsNullOrEmpty(id) && !result.Contains(id)) result.Add(id);
                }
            }
            if (!string.IsNullOrEmpty(legacyId) && !result.Contains(legacyId)) result.Add(legacyId);
            return result;
        }

        /// <summary>
        /// 构建人物序列化数据（含可走路径集合）
        /// </summary>
        /// <param name="obj">已放置人物</param>
        /// <returns>JSON 数据对象</returns>
        private MapCharacterData BuildMapCharacterData(PlacedObject obj)
        {
            var cell = MapToolEditorUtils.WorldToCell(obj.Position); // ---TODO 网格信息：格子坐标（一次计算，供 gridX/gridY 共用）
            MapCharacterData character = new MapCharacterData
            {
                id = obj.Id,
                templateId = obj.TemplateId,
                name = obj.Name,
                posX = obj.Position.x,
                posY = obj.Position.y,
                gridX = cell.x, // ---TODO 网格信息：格子坐标
                gridY = cell.y, // ---TODO 网格信息：格子坐标
                width = obj.Width,
                height = obj.Height,
                pixelWidth = obj.PixelWidth,
                pixelHeight = obj.PixelHeight,
                stretch = obj.Stretch,
                layerType = obj.LayerType,
                displayLayer = obj.DisplayLayer,
                image = MapToolEditorUtils.GetObjectImagePath(obj.PreviewTexture, obj.Id),
                anim = obj.AnimName,
                animEnabled = MapToolEditorUtils.IsAnimEnabled(obj),
                loop = obj.Loop,
                speed = obj.Speed,
                interactable = obj.Interactable,
                interactType = obj.InteractType,
                walkMode = obj.WalkMode,
                facingDirection = obj.FacingDirection,
                dialogBubble = obj.DialogBubble,
                sortOrder = obj.SortOrder,
                ySortEnabled = obj.YSortEnabled,
                ySortOffset = obj.YSortOffset,
                rotation = obj.Rotation,
                scale = obj.Scale,
                shadowEnabled = obj.ShadowEnabled,
                loopRestartDelay = obj.LoopRestartDelay,
                hideWhenOffMap = obj.HideWhenOffMap,
                // ---TODO 交通规则：载具标记、停止线与绑定灯（多选列表 + 旧单值兼容）
                isVehicle = obj.IsVehicle,
                trafficRole = obj.TrafficRole,
                stopLineX = obj.StopLineX,
                stopLineY = obj.StopLineY,
                boundLightId = obj.BoundLightId,
                boundLightIds = NormalizeBoundLightIds(obj.BoundLightIds, obj.BoundLightId),
                yellowTreatAsRed = obj.YellowTreatAsRed,
            };
            foreach (var point in obj.PathPoints)
                character.pathPoints.Add(new MapPathPointData
                {
                    x = point.x,
                    y = point.y,
                    gridX = MapToolEditorUtils.WorldToCell(new Vector2(point.x, point.y)).x, // ---TODO 网格信息：路径点格子坐标
                    gridY = MapToolEditorUtils.WorldToCell(new Vector2(point.x, point.y)).y, // ---TODO 网格信息：路径点格子坐标
                });
            return character;
        }

        /// <summary>
        /// 加载章节配置表（场景名/场景ID/分区配置来源）
        /// 仅首次加载，之后使用缓存
        /// 【取消分区】Excel 配置缺失时回退运行时 Lua 章节表（与游戏运行时同源）
        /// </summary>
        private void LoadChapterConfig()
        {
            if (m_ChapterDataList.Count > 0) return;
            var fromExcel = MapToolEditorUtils.LoadTableData<TableChapterEditor>(MapToolConstants.TableChaptersPath);
            if (fromExcel.Count > 0)
            {
                m_ChapterDataList.AddRange(fromExcel);
                MapToolLog.Info($"加载章节配置表成功，共 {m_ChapterDataList.Count} 个章节");
            }
            else
            {
                m_ChapterDataList.AddRange(MapToolEditorUtils.LoadChaptersFromRuntimeLua());
                MapToolLog.Info($"章节配置表 Excel 缺失，回退运行时 Lua 表，共 {m_ChapterDataList.Count} 个章节");
            }
        }

        /// <summary>
        /// 加载主关卡配置表（关卡 ID -> 背景图名）
        /// </summary>
        private void LoadMainLevelsConfig()
        {
            // 委托给 MapConfigRepository 读配置表，本类只负责更新字典引用
            m_LevelBgDict = MapConfigRepository.LoadLevelBgDict();
        }

        /// <summary>
        /// 背景图尺寸兜底：当前宽高非法时，按已加载背景纹理的原始像素宽高回填；
        /// 背景缺失时回退单关卡基准尺寸。宽高合法时保留配置坐标系（摆放物坐标口径不变），
        /// 背景按 mapRect 拉伸铺满，网格随 mapRect 等比铺满整张背景。
        /// </summary>
        private void ApplyBackgroundSizeFallback()
        {
            Texture bg = LoadRegionBackground(m_CurrentLevelId);
            int bgW = bg != null ? bg.width : 0;
            int bgH = bg != null ? bg.height : 0;

            if (m_MapWidth <= 0) m_MapWidth = bgW > 0 ? bgW : MapToolConstants.LevelPixelSize;
            if (m_MapHeight <= 0) m_MapHeight = bgH > 0 ? bgH : MapToolConstants.LevelPixelSize;

            SyncConfigMapSize();
        }

        /// <summary>
        /// 读取输入框值（为空时用默认值）
        /// </summary>
        private string GetFieldValue(TextField field, string def)
        {
            return field != null && !string.IsNullOrEmpty(field.value) ? field.value : def;
        }

        /// <summary>
        /// 解析整数（失败时用默认值）
        /// </summary>
        private int ParseInt(string s, int def)
        {
            return int.TryParse(s, out int v) ? v : def;
        }

        /// <summary>
        /// 加载物体预览图（依次尝试候选资源目录）
        /// </summary>
        /// <param name="objectId">物体ID</param>
        /// <returns>预览图，未找到返回 null</returns>
        private Texture LoadObjectPreview(string objectId)
        {
            return MapToolEditorUtils.LoadObjectPreview(objectId);
        }

        /// <summary>
        /// 放置合法性检查：越界 / 与已有物体重叠
        /// 【取消分区】移除分区解锁检查
        /// </summary>
        /// <param name="obj">待放置物体</param>
        /// <param name="reason">失败原因</param>
        /// <returns>是否可以放置</returns>
        private bool CanPlaceObject(PlacedObject obj, out string reason)
        {
            reason = null;
            // 碰撞矩形优先 PSD 像素尺寸；无像素记录时回退格子数×网格尺寸
            float pixelWidth = obj.PixelWidth > 0 ? obj.PixelWidth : obj.Width * MapToolConstants.GridSize;
            float pixelHeight = obj.PixelHeight > 0 ? obj.PixelHeight : obj.Height * MapToolConstants.GridSize;
            Rect objRect = new Rect(obj.Position.x, obj.Position.y, pixelWidth, pixelHeight);

            // 1. 越界检查（左上角原点坐标系：与 JSON 数据格式一致）
            if (objRect.xMin < 0 || objRect.yMin < 0 ||
                objRect.xMax > m_MapWidth || objRect.yMax > m_MapHeight)
            {
                reason = "位置越界，无法放置";
                return false;
            }

            // 2. 重叠检查（建筑和物件不能重叠，人物可以重叠——因为会移动）
            if (!obj.AllowOverlap)
            {
                foreach (var exist in m_PlacedObjects)
                {
                    if (exist == obj) continue;
                    // 人物类型可以重叠（运行时再处理碰撞）
                    if (obj.Type == MapObjectType.Character && exist.Type == MapObjectType.Character) continue;
                    // 只检查建筑和物件的重叠
                    if (exist.Type != obj.Type && obj.Type != MapObjectType.Character) continue;
                    float existW = exist.PixelWidth > 0 ? exist.PixelWidth : exist.Width * MapToolConstants.GridSize;
                    float existH = exist.PixelHeight > 0 ? exist.PixelHeight : exist.Height * MapToolConstants.GridSize;
                    Rect existRect = new Rect(exist.Position.x, exist.Position.y, existW, existH);
                    if (objRect.Overlaps(existRect))
                    {
                        reason = "与同图层物体重叠，无法放置";
                        return false;
                    }
                }
            }

            return true;
        }

        // ---TODO 编辑器加载还原：有已保存 JSON 地图配置时优先还原摆放物/背景（SaveMapConfig 反向）
        private void LoadMapConfig()
        {
            // 委托给 MapConfigRepository 读 JSON，本类只负责把数据还原到 UI 状态
            MapConfigData data = MapConfigRepository.LoadFromJson(m_MapSavePath);
            if (data == null) return;

            ApplyLoadedMapMeta(data);
            RestorePlacedObjects(data);
            RestoreTrafficData(data);
            FocusViewportOnPlacedObjects();

            // 触发场景重绘
            if (m_GuiContainer != null)
                m_GuiContainer.MarkDirtyRepaint();

            MapToolLog.Info($"已加载关卡地图配置: {m_MapSavePath}（摆放物 {m_PlacedObjects.Count}）");
        }

        /// <summary>
        /// 还原整图宽高与背景图名（宽高以 JSON 为准，缺失时回退背景图像素）
        /// </summary>
        /// <param name="data">地图配置数据</param>
        private void ApplyLoadedMapMeta(MapConfigData data)
        {
            if (data.width > 0) m_MapWidth = data.width;
            if (data.height > 0) m_MapHeight = data.height;
            ApplyBackgroundSizeFallback();

            if (!string.IsNullOrEmpty(data.bgName))
                m_LevelBgDict[m_CurrentLevelId] = data.bgName;
        }

        /// <summary>
        /// 还原摆放物（人物 / 建筑 / 物件）
        /// </summary>
        /// <param name="data">地图配置数据</param>
        private void RestorePlacedObjects(MapConfigData data)
        {
            m_PlacedObjects.Clear();

            AddRestoredPlacedObjects(data.characters, c => PlacedObjectFromCharacter(c));
            AddRestoredPlacedObjects(data.buildings, b => PlacedObjectFromMapObject(b, MapObjectType.Building));
            AddRestoredPlacedObjects(data.objects, o => PlacedObjectFromMapObject(o, MapObjectType.Prop));
        }

        /// <summary>
        /// 批量还原一类摆放物（源集合为空时跳过，单条转换失败时忽略）
        /// </summary>
        /// <typeparam name="TSource">源数据类型</typeparam>
        /// <param name="source">源数据集合</param>
        /// <param name="converter">单条转换函数（失败返回 null）</param>
        private void AddRestoredPlacedObjects<TSource>(List<TSource> source, System.Func<TSource, PlacedObject> converter)
        {
            if (source == null) return;

            foreach (var item in source)
            {
                var placed = converter(item);
                if (placed != null) m_PlacedObjects.Add(placed);
            }
        }

        /// <summary>
        /// 还原交通规则数据（红绿灯 / 斑马线 / 区域 / 人行道格 / 道路格，旧地图缺省为空集合）
        /// </summary>
        /// <param name="data">地图配置数据</param>
        private void RestoreTrafficData(MapConfigData data)
        {
            m_TrafficLights.Clear();
            m_Crosswalks.Clear();
            m_Regions.Clear();
            m_SidewalkCells.Clear();
            m_RoadCells.Clear();

            if (data.trafficLights != null) m_TrafficLights.AddRange(data.trafficLights);
            if (data.crosswalks != null) m_Crosswalks.AddRange(data.crosswalks);
            if (data.regions != null) m_Regions.AddRange(data.regions);
            if (data.sidewalkCells != null) m_SidewalkCells.AddRange(data.sidewalkCells);
            if (data.roadCells != null) m_RoadCells.AddRange(data.roadCells);
        }

        /// <summary>
        /// 把视口缩放与偏移定位到摆放物包围盒
        /// </summary>
        private void FocusViewportOnPlacedObjects()
        {
            if (m_PlacedObjects.Count == 0 || m_GuiContainer == null) return;

            float minX = float.MaxValue, minY = float.MaxValue, maxX = float.MinValue, maxY = float.MinValue;
            foreach (var obj in m_PlacedObjects)
            {
                minX = Mathf.Min(minX, obj.Position.x);
                minY = Mathf.Min(minY, obj.Position.y);
                float objW = obj.PixelWidth > 0 ? obj.PixelWidth : obj.Width * MapToolConstants.GridSize;
                float objH = obj.PixelHeight > 0 ? obj.PixelHeight : obj.Height * MapToolConstants.GridSize;
                maxX = Mathf.Max(maxX, obj.Position.x + objW);
                maxY = Mathf.Max(maxY, obj.Position.y + objH);
            }

            Rect view = m_GuiContainer.contentRect;
            if (view.width <= 0 || view.height <= 0) return;

            float boundsW = Mathf.Max(1f, maxX - minX);
            float boundsH = Mathf.Max(1f, maxY - minY);
            m_Zoom = Mathf.Clamp(Mathf.Min(view.width / boundsW, view.height / boundsH),
                MapToolConstants.MinZoom, MapToolConstants.AutoFitMaxZoom);
            m_MapOffset = new Vector2(
                view.width / 2 - (minX + maxX) / 2 * m_Zoom,
                view.height / 2 - (minY + maxY) / 2 * m_Zoom);
        }

        /// <summary>
        /// 由 MapCharacterData 还原人物摆放对象（含行走路径点）
        /// </summary>
        /// <param name="c">人物 JSON 数据</param>
        /// <returns>还原的摆放对象，数据无效返回 null</returns>
        private PlacedObject PlacedObjectFromCharacter(MapCharacterData c)
        {
            if (c == null) return null;
            var p = new PlacedObject
            {
                TemplateId = c.templateId,
                Name = c.name,
                Type = MapObjectType.Character,
                Position = new Vector2(c.posX, c.posY),
                Width = c.width,
                Height = c.height,
                PixelWidth = c.pixelWidth,
                PixelHeight = c.pixelHeight,
                Stretch = c.stretch,
                LayerType = MapLayerCatalog.Normalize(c.layerType),
                DisplayLayer = c.displayLayer,
                PreviewTexture = LoadPreviewFromImage(c.image),
                AnimName = c.anim,
                Loop = c.loop,
                Speed = c.speed,
                Interactable = c.interactable,
                InteractType = c.interactType,
                WalkMode = c.walkMode,
                FacingDirection = c.facingDirection,
                DialogBubble = c.dialogBubble,
                SortOrder = c.sortOrder,
                YSortEnabled = c.ySortEnabled,
                YSortOffset = c.ySortOffset,
                Rotation = c.rotation,
                Scale = c.scale,
                ShadowEnabled = c.shadowEnabled,
                LoopRestartDelay = c.loopRestartDelay,
                HideWhenOffMap = c.hideWhenOffMap,
                // ---TODO 交通规则：载具字段对称回填（旧地图缺省走模型默认值；旧单值兜底进列表）
                IsVehicle = c.isVehicle,
                TrafficRole = string.IsNullOrEmpty(c.trafficRole) ? TemplateTrafficCategory.Pedestrian : c.trafficRole,
                StopLineX = c.stopLineX,
                StopLineY = c.stopLineY,
                BoundLightId = c.boundLightId,
                BoundLightIds = NormalizeBoundLightIds(c.boundLightIds, c.boundLightId),
                YellowTreatAsRed = c.yellowTreatAsRed,
            };
            // ---TODO [存量归一] 旧 JSON 可能落盘旧 100 步长全局键（0/100/200…）或旧三档裸值；
            // 读取时换算到新 10 步长八层基线坐标系，再钳到有效显示档区间，避免旧大值越界压层
            p.SortOrder = MapToolEditorUtils.NormalizeLegacySortOrder(p, p.SortOrder);
            // 交互类型/朝向归一为 code（存量 JSON 可能存中文显示名）
            p.InteractType = TemplateInteractType.ToCode(p.InteractType);
            p.FacingDirection = TemplateFacing.ToCode(p.FacingDirection);
            if (!string.IsNullOrEmpty(c.id)) p.Id = c.id;
            if (c.pathPoints != null)
            {
                foreach (var pt in c.pathPoints)
                {
                    if (pt != null)
                        p.PathPoints.Add(new Vector2(pt.x, pt.y));
                }
            }
            return p;
        }

        /// <summary>
        /// 由 MapObjectData 还原物件 / 建筑摆放对象
        /// </summary>
        /// <param name="o">物件或建筑 JSON 数据</param>
        /// <param name="type">目标类型（Prop/Building）</param>
        /// <returns>还原的摆放对象，数据无效返回 null</returns>
        private PlacedObject PlacedObjectFromMapObject(MapObjectData o, MapObjectType type)
        {
            if (o == null) return null;
            var p = new PlacedObject
            {
                TemplateId = o.templateId,
                Name = o.name,
                Type = type,
                Position = new Vector2(o.posX, o.posY),
                Width = o.width,
                Height = o.height,
                PixelWidth = o.pixelWidth,
                PixelHeight = o.pixelHeight,
                Stretch = o.stretch,
                LayerType = MapLayerCatalog.Normalize(o.layerType),
                DisplayLayer = o.displayLayer,
                PreviewTexture = LoadPreviewFromImage(o.image),
                AnimName = o.anim,
                Loop = o.loop,
                Speed = o.speed,
                Interactable = o.interactable,
                InteractType = o.interactType,
                SortOrder = o.sortOrder,
                YSortEnabled = o.ySortEnabled,
                YSortOffset = o.ySortOffset,
                Rotation = o.rotation,
                Scale = o.scale,
                CollisionEnabled = o.collisionEnabled,
                DialogBubble = o.dialogBubble,
                ShadowEnabled = o.shadowEnabled,
                IsCollectible = o.isCollectible,
                RespawnTime = o.respawnTime,
                bubbleId = o.bubbleId,
                IsMovable = o.isMovable,
                LoopRestartDelay = o.loopRestartDelay,
                HideWhenOffMap = o.hideWhenOffMap,
                IsTeleport = o.isTeleport,
                TeleportTargetScene = o.teleportTargetScene,
                TeleportTargetX = o.teleportTargetX,
                TeleportTargetY = o.teleportTargetY,
            };
            // ---TODO [存量归类] 归属层未识别（prop 兜底）时，按模板名英文关键词推断归属层；PSD 编号名无法识别仍回退 prop
            if (p.LayerType == MapLayerCatalog.Prop)
            {
                p.LayerType = MapLayerCatalog.ResolveLayerByTemplateName(p.TemplateId, p.Name);
            }
            // ---TODO [存量归一] 旧 JSON 的 SortOrder 为旧 100 步长全局键或旧三档裸值（10/20/30），未对齐新八层基线；
            // 导入后换算到新 10 步长坐标系并钳到有效显示档区间，避免旧值在场景实例全局排序中压过基线更高的层
            p.SortOrder = MapToolEditorUtils.NormalizeLegacySortOrder(p, p.SortOrder);
            // 交互类型/朝向归一为 code（存量 JSON 可能存中文显示名）
            p.InteractType = TemplateInteractType.ToCode(p.InteractType);
            p.FacingDirection = TemplateFacing.ToCode(p.FacingDirection);
            if (!string.IsNullOrEmpty(o.id)) p.Id = o.id;
            return p;
        }

        /// <summary>
        /// 从 image（图标名或资源路径）加载预览纹理；未找到返回 null（绘制时用占位色块）
        /// </summary>
        /// <param name="image">图片资源名或路径</param>
        /// <returns>预览纹理，未找到返回 null</returns>
        private Texture LoadPreviewFromImage(string image)
        {
            if (string.IsNullOrEmpty(image)) return null;
            if (image.Contains("/"))
            {
                Texture t = AssetDatabase.LoadAssetAtPath<Texture2D>(image);
                if (t != null) return t;
            }
            return MapToolEditorUtils.LoadObjectPreview(image);
        }

    }
}
