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
            return new MapObjectData
            {
                id = obj.Id,
                templateId = obj.TemplateId,
                name = obj.Name,
                posX = obj.Position.x,
                posY = obj.Position.y,
                gridX = Mathf.FloorToInt(obj.Position.x / MapToolConstants.GridSize), // ---TODO 网格信息：格子坐标
                gridY = Mathf.FloorToInt(obj.Position.y / MapToolConstants.GridSize), // ---TODO 网格信息：格子坐标
                width = obj.Width,
                height = obj.Height,
                image = MapToolEditorUtils.GetObjectImagePath(obj.PreviewTexture, obj.Id),
                anim = obj.AnimName,
                animEnabled = !string.IsNullOrEmpty(obj.AnimName) && obj.AnimName != "none",
                loop = obj.Loop,
                speed = obj.Speed,
                interactable = obj.Interactable,
                interactType = obj.InteractType,
                sortOrder = obj.SortOrder,
                ySortEnabled = obj.YSortEnabled,
                ySortOffset = obj.YSortOffset,
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
        /// 构建人物序列化数据（含可走路径集合）
        /// </summary>
        /// <param name="obj">已放置人物</param>
        /// <returns>JSON 数据对象</returns>
        private MapCharacterData BuildMapCharacterData(PlacedObject obj)
        {
            MapCharacterData character = new MapCharacterData
            {
                id = obj.Id,
                templateId = obj.TemplateId,
                name = obj.Name,
                posX = obj.Position.x,
                posY = obj.Position.y,
                gridX = Mathf.FloorToInt(obj.Position.x / MapToolConstants.GridSize), // ---TODO 网格信息：格子坐标
                gridY = Mathf.FloorToInt(obj.Position.y / MapToolConstants.GridSize), // ---TODO 网格信息：格子坐标
                width = obj.Width,
                height = obj.Height,
                image = MapToolEditorUtils.GetObjectImagePath(obj.PreviewTexture, obj.Id),
                anim = obj.AnimName,
                animEnabled = !string.IsNullOrEmpty(obj.AnimName) && obj.AnimName != "none",
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
                shadowEnabled = obj.ShadowEnabled,
                loopRestartDelay = obj.LoopRestartDelay,
                hideWhenOffMap = obj.HideWhenOffMap,
            };
            foreach (var point in obj.PathPoints)
                character.pathPoints.Add(new MapPathPointData
                {
                    x = point.x,
                    y = point.y,
                    gridX = Mathf.FloorToInt(point.x / MapToolConstants.GridSize), // ---TODO 网格信息：路径点格子坐标
                    gridY = Mathf.FloorToInt(point.y / MapToolConstants.GridSize), // ---TODO 网格信息：路径点格子坐标
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
                Debug.Log($"加载章节配置表成功，共 {m_ChapterDataList.Count} 个章节");
            }
            else
            {
                m_ChapterDataList.AddRange(MapToolEditorUtils.LoadChaptersFromRuntimeLua());
                Debug.Log($"章节配置表 Excel 缺失，回退运行时 Lua 表，共 {m_ChapterDataList.Count} 个章节");
            }
        }

        /// <summary>
        /// 加载主关卡配置表（关卡 ID -> 背景图名）
        /// </summary>
        private void LoadMainLevelsConfig()
        {
            m_LevelBgDict.Clear();
            var dataList = MapToolEditorUtils.LoadTableData<TableMainLevelsEditor>(MapToolConstants.TableMainLevelsPath);
            foreach (var data in dataList)
            {
                if (!string.IsNullOrEmpty(data.ID) && !string.IsNullOrEmpty(data.Level_bg))
                    m_LevelBgDict[data.ID] = data.Level_bg;
            }
            Debug.Log($"关卡配置表加载完成，共 {m_LevelBgDict.Count} 条");
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
            float pixelWidth = obj.Width * 32f;
            float pixelHeight = obj.Height * 32f;
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
                    float existW = exist.Width * 32f;
                    float existH = exist.Height * 32f;
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
        /// <summary>
        /// 从已保存的 JSON 地图配置还原摆放物 / 背景图
        /// 【取消分区】单关卡=单张地图，直接还原整图数据
        /// </summary>
        private void LoadMapConfig()
        {
            if (string.IsNullOrEmpty(m_MapSavePath)) return;
            string relativePath = MapToolPathHelper.NormalizeToAssetRelative(m_MapSavePath);
            string fullPath = MapToolPathHelper.ToAbsolutePath(relativePath);
            if (!File.Exists(fullPath))
            {
                Debug.Log($"未找到已保存的地图配置: {relativePath}，加载空场景");
                return;
            }

            MapConfigData data;
            try
            {
                data = JsonUtility.FromJson<MapConfigData>(File.ReadAllText(fullPath));
            }
            catch (System.Exception e)
            {
                Debug.LogError($"读取地图配置失败: {e.Message}");
                return;
            }
            if (data == null) return;

            // 整图宽高以 JSON 为准
            if (data.width > 0) m_MapWidth = data.width;
            if (data.height > 0) m_MapHeight = data.height;

            // 还原背景图
            if (!string.IsNullOrEmpty(data.bgName))
            {
                string regionId = m_CurrentLevelId;
                m_LevelBgDict[regionId] = data.bgName;
            }

            // 还原摆放物（人物 / 建筑 / 物件）
            m_PlacedObjects.Clear();
            if (data.characters != null)
            {
                foreach (var c in data.characters)
                {
                    var p = PlacedObjectFromCharacter(c);
                    if (p != null) m_PlacedObjects.Add(p);
                }
            }
            if (data.buildings != null)
            {
                foreach (var b in data.buildings)
                {
                    var p = PlacedObjectFromMapObject(b, MapObjectType.Building);
                    if (p != null) m_PlacedObjects.Add(p);
                }
            }
            if (data.objects != null)
            {
                foreach (var o in data.objects)
                {
                    var p = PlacedObjectFromMapObject(o, MapObjectType.Prop);
                    if (p != null) m_PlacedObjects.Add(p);
                }
            }

            // 加载后自动把视口定位到摆放物区域
            if (m_PlacedObjects.Count > 0 && m_GuiContainer != null)
            {
                float minX = float.MaxValue, minY = float.MaxValue, maxX = float.MinValue, maxY = float.MinValue;
                foreach (var obj in m_PlacedObjects)
                {
                    minX = Mathf.Min(minX, obj.Position.x);
                    minY = Mathf.Min(minY, obj.Position.y);
                    maxX = Mathf.Max(maxX, obj.Position.x + obj.Width * MapToolConstants.GridSize);
                    maxY = Mathf.Max(maxY, obj.Position.y + obj.Height * MapToolConstants.GridSize);
                }
                Rect view = m_GuiContainer.contentRect;
                float boundsW = Mathf.Max(1f, maxX - minX);
                float boundsH = Mathf.Max(1f, maxY - minY);
                if (view.width > 0 && view.height > 0)
                {
                    m_Zoom = Mathf.Clamp(Mathf.Min(view.width / boundsW, view.height / boundsH), MapToolConstants.MinZoom, MapToolConstants.AutoFitMaxZoom);
                    m_MapOffset = new Vector2(
                        view.width / 2 - (minX + maxX) / 2 * m_Zoom,
                        view.height / 2 - (minY + maxY) / 2 * m_Zoom);
                }
            }

            // 触发场景重绘
            if (m_GuiContainer != null)
                m_GuiContainer.MarkDirtyRepaint();

            Debug.Log($"已加载关卡地图配置: {relativePath}（摆放物 {m_PlacedObjects.Count}）");
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
                ShadowEnabled = c.shadowEnabled,
                LoopRestartDelay = c.loopRestartDelay,
                HideWhenOffMap = c.hideWhenOffMap,
            };
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
                PreviewTexture = LoadPreviewFromImage(o.image),
                AnimName = o.anim,
                Loop = o.loop,
                Speed = o.speed,
                Interactable = o.interactable,
                InteractType = o.interactType,
                SortOrder = o.sortOrder,
                YSortEnabled = o.ySortEnabled,
                YSortOffset = o.ySortOffset,
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
