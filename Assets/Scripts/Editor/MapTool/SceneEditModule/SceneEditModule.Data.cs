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
                name = obj.Name,
                posX = obj.Position.x,
                posY = obj.Position.y,
                width = obj.Width,
                height = obj.Height,
                image = MapToolEditorUtils.GetObjectImagePath(obj.PreviewTexture, obj.Id),
                anim = obj.AnimName,
                interactable = obj.Interactable,
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
                name = obj.Name,
                posX = obj.Position.x,
                posY = obj.Position.y,
                width = obj.Width,
                height = obj.Height,
                image = MapToolEditorUtils.GetObjectImagePath(obj.PreviewTexture, obj.Id),
                anim = obj.AnimName,
                interactable = obj.Interactable,
            };
            foreach (var point in obj.PathPoints)
                character.pathPoints.Add(new MapPathPointData { x = point.x, y = point.y });
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

    }
}
