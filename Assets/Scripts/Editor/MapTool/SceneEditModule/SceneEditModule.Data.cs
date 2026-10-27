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
                collisionEnabled = obj.CollisionEnabled,
                dialogBubble = obj.DialogBubble,
                shadowEnabled = obj.ShadowEnabled,
                isCollectible = obj.IsCollectible,
                respawnTime = obj.RespawnTime,
                bubbleId = obj.bubbleId, // ---TODO 气泡配置表通用化：物件/建筑引用 Tables.Bubble
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
                patrolLoop = obj.PatrolLoop,
                facingDirection = obj.FacingDirection,
                dialogBubble = obj.DialogBubble,
                sortOrder = obj.SortOrder,
                shadowEnabled = obj.ShadowEnabled,
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
        /// </summary>
        private void LoadChapterConfig()
        {
            if (m_ChapterDataList.Count > 0) return;
            m_ChapterDataList.AddRange(
                MapToolEditorUtils.LoadTableData<TableChapterEditor>(MapToolConstants.TableChaptersPath));
            Debug.Log($"加载章节配置表成功，共 {m_ChapterDataList.Count} 个章节");
        }

        /// <summary>
        /// 根据已解锁分区包围盒更新配置宽高（并同步显示到宽/高输入框）
        /// 宽 = 包围盒列数 × 单区尺寸，高 = 包围盒行数 × 单区尺寸
        /// </summary>
        private void UpdateMapBoundsFromUnlockedRegions()
        {
            if (TryGetUnlockedBounds(out int minRow, out int maxRow, out int minCol, out int maxCol))
            {
                m_ConfigMapWidth = (maxCol - minCol + 1) * MapToolConstants.RegionPixelSize;
                m_ConfigMapHeight = (maxRow - minRow + 1) * MapToolConstants.RegionPixelSize;
            }
            else
            {
                m_ConfigMapWidth = 0;
                m_ConfigMapHeight = 0;
            }
            // 同步更新输入框显示
            if (m_WidthField != null)
                m_WidthField.SetValueWithoutNotify(m_ConfigMapWidth.ToString());
            if (m_HeightField != null)
                m_HeightField.SetValueWithoutNotify(m_ConfigMapHeight.ToString());
            Debug.Log($"包围盒宽高已更新: {m_ConfigMapWidth} x {m_ConfigMapHeight}（已解锁分区 {minRow},{minCol} ~ {maxRow},{maxCol}）");
        }

        /// <summary>
        /// 加载主关卡配置表（分区 ID -> 背景图名）
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
            Debug.Log($"主关卡配置表加载完成，共 {m_LevelBgDict.Count} 条");
        }

        /// <summary>
        /// 根据物体位置计算其所在分区索引
        /// 分区布局：m_RegionRows 行 x m_RegionCols 列，按左上角坐标定位
        /// </summary>
        private int GetRegionIndexForObject(PlacedObject obj)
        {
            if (m_RegionRows <= 0 || m_RegionCols <= 0 || m_RegionCount <= 0 || m_MapWidth <= 0 || m_MapHeight <= 0)
                return 0;
            float regionWidth = (float)m_MapWidth / m_RegionCols;
            float regionHeight = (float)m_MapHeight / m_RegionRows;
            int col = Mathf.FloorToInt(obj.Position.x / regionWidth);
            int row = Mathf.FloorToInt(obj.Position.y / regionHeight);
            col = Mathf.Clamp(col, 0, m_RegionCols - 1);
            row = Mathf.Clamp(row, 0, m_RegionRows - 1);
            int index = row * m_RegionCols + col;
            return Mathf.Min(index, m_RegionCount - 1);
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
        /// 计算已解锁分区的包围盒（行列范围）
        /// </summary>
        private bool TryGetUnlockedBounds(out int minRow, out int maxRow, out int minCol, out int maxCol)
        {
            minRow = maxRow = minCol = maxCol = 0;
            if (m_RegionUnlocked == null || m_RegionRows <= 0 || m_RegionCols <= 0)
                return false;
            bool found = false;
            for (int row = 0; row < m_RegionRows; row++)
            {
                for (int col = 0; col < m_RegionCols; col++)
                {
                    if (!m_RegionUnlocked[row, col]) continue;
                    if (!found)
                    {
                        minRow = maxRow = row;
                        minCol = maxCol = col;
                        found = true;
                    }
                    else
                    {
                        minRow = Mathf.Min(minRow, row);
                        maxRow = Mathf.Max(maxRow, row);
                        minCol = Mathf.Min(minCol, col);
                        maxCol = Mathf.Max(maxCol, col);
                    }
                }
            }
            return found;
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
        /// 放置合法性检查：越界 / 分区未解锁 / 与已有物体重叠
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

            // 1. 越界检查
            if (objRect.xMin < 0 || objRect.yMin < 0 ||
                objRect.xMax > m_MapWidth || objRect.yMax > m_MapHeight)
            {
                reason = "位置越界，无法放置";
                return false;
            }

            // 2. 分区未解锁检查（物体中心所在分区必须已解锁）
            Vector2 center = new Vector2(obj.Position.x + pixelWidth / 2, obj.Position.y + pixelHeight / 2);
            int regionIdx = GetRegionIndexAtPosition(center);
            if (regionIdx >= 0 && m_RegionUnlocked != null)
            {
                int row = regionIdx / m_RegionCols;
                int col = regionIdx % m_RegionCols;
                if (!m_RegionUnlocked[row, col])
                {
                    reason = "该分区未解锁，无法放置";
                    return false;
                }
            }

            // 3. 重叠检查（AllowOverlap=true 的物体可以重叠）
            if (!obj.AllowOverlap)
            {
                foreach (var exist in m_PlacedObjects)
                {
                    if (exist == obj) continue;
                    float existW = exist.Width * 32f;
                    float existH = exist.Height * 32f;
                    Rect existRect = new Rect(exist.Position.x, exist.Position.y, existW, existH);
                    if (objRect.Overlaps(existRect))
                    {
                        reason = "与已有物体重叠，无法放置";
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// 根据世界坐标获取分区索引
        /// </summary>
        private int GetRegionIndexAtPosition(Vector2 worldPos)
        {
            if (m_RegionUnlocked == null || m_RegionCount <= 0) return -1;
            float regionW = m_MapWidth / m_RegionCols;
            float regionH = m_MapHeight / m_RegionRows;
            int col = Mathf.FloorToInt(worldPos.x / regionW);
            int row = Mathf.FloorToInt(worldPos.y / regionH);
            if (row < 0 || row >= m_RegionRows || col < 0 || col >= m_RegionCols) return -1;
            return row * m_RegionCols + col;
        }

        /// <summary>
        /// 显示临时提示消息
        /// </summary>
        private string m_TempMessage;
        private float m_TempMessageTime;
        private void ShowMessage(string msg)
        {
            m_TempMessage = msg;
            m_TempMessageTime = Time.realtimeSinceStartup + 1.5f;
        }

        // ---TODO 编辑器加载还原：有已保存 JSON 地图配置时优先还原摆放物/分区解锁/背景（SaveMapConfig 反向）
        /// <summary>
        /// 从已保存的 JSON 地图配置还原摆放物 / 分区解锁 / 背景图
        /// 由 LoadChapterMap 在加载章节后调用；无 JSON 文件则保持空场景不报错
        /// </summary>
        private void LoadMapConfig()
        {
            if (string.IsNullOrEmpty(m_MapSavePath)) return;
            string relativePath = m_MapSavePath.Replace("\\", "/");
            if (!relativePath.StartsWith("Assets/"))
                relativePath = "Assets/" + relativePath.TrimStart('/');
            string fullPath = Path.Combine(Application.dataPath, relativePath.Substring("Assets/".Length));
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
            if (data == null || data.regions == null) return;

            // 还原分区解锁状态与背景图（region 顺序与 m_RegionIds 一致）
            if (m_RegionUnlocked != null && m_RegionCols > 0)
            {
                for (int i = 0; i < data.regions.Count && i < m_RegionCount; i++)
                {
                    int row = i / m_RegionCols;
                    int col = i % m_RegionCols;
                    if (row < m_RegionRows && col < m_RegionCols)
                        m_RegionUnlocked[row, col] = data.regions[i].unlocked;
                    if (!string.IsNullOrEmpty(data.regions[i].bgName))
                        m_LevelBgDict[data.regions[i].regionId] = data.regions[i].bgName;
                }
            }

            // 还原摆放物（人物 / 建筑 / 物件）
            m_PlacedObjects.Clear();
            foreach (var region in data.regions)
            {
                if (region.characters != null)
                {
                    foreach (var c in region.characters)
                    {
                        var p = PlacedObjectFromCharacter(c);
                        if (p != null) m_PlacedObjects.Add(p);
                    }
                }
                if (region.buildings != null)
                {
                    foreach (var b in region.buildings)
                    {
                        var p = PlacedObjectFromMapObject(b, MapObjectType.Building);
                        if (p != null) m_PlacedObjects.Add(p);
                    }
                }
                if (region.objects != null)
                {
                    foreach (var o in region.objects)
                    {
                        var p = PlacedObjectFromMapObject(o, MapObjectType.Prop);
                        if (p != null) m_PlacedObjects.Add(p);
                    }
                }
            }

            // ---TODO 修复：不覆盖地图宽高——m_MapWidth/m_MapHeight 已由 LoadChapterMap 按分区布局正确计算（分区数×2048），
            // 旧 JSON 的 width/height 可能是单分区默认值（2048），若覆盖会把多分区地图（4096）缩成 2048，导致分区 2/3 角色坐标超出地图不可见
            // 地图宽高一律以分区布局为准，仅用于渲染网格/背景，不采用 JSON 值

            // ---TODO 改进：加载后自动把视口定位到摆放物区域，防止角色坐标超出当前视口/地图显示区时不可见
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
                    m_Zoom = Mathf.Clamp(Mathf.Min(view.width / boundsW, view.height / boundsH), 0.1f, 4f);
                    m_MapOffset = new Vector2(
                        view.width / 2 - (minX + maxX) / 2 * m_Zoom,
                        view.height / 2 - (minY + maxY) / 2 * m_Zoom);
                }
            }

            // 触发场景重绘
            if (m_GuiContainer != null)
                m_GuiContainer.MarkDirtyRepaint();

            Debug.Log($"已加载已保存的地图配置: {relativePath}（分区 {data.regions.Count}，摆放物 {m_PlacedObjects.Count}）");
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
                PatrolLoop = c.patrolLoop,
                FacingDirection = c.facingDirection,
                DialogBubble = c.dialogBubble,
                SortOrder = c.sortOrder,
                ShadowEnabled = c.shadowEnabled,
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
                CollisionEnabled = o.collisionEnabled,
                DialogBubble = o.dialogBubble,
                ShadowEnabled = o.shadowEnabled,
                IsCollectible = o.isCollectible,
                RespawnTime = o.respawnTime,
                bubbleId = o.bubbleId,
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
