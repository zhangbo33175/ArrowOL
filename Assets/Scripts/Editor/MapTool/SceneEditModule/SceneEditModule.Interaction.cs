/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  SceneEditModule.Interaction.cs
 * author:    云毅
 * created:   2026
 * descrip:   场景编辑模块 - 私有方法（鼠标交互/缩放/路径编辑输入处理）
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
    /// 场景编辑模块 - 鼠标交互与输入处理私有方法
    /// </summary>
    public partial class SceneEditModule
    {
        /// <summary>
        /// 鼠标按下事件
        /// </summary>
        private void OnMouseDown(MouseDownEvent evt)
        {
            if (evt.button == (int)MouseButton.LeftMouse)
            {
                m_IsDragging = true;
                m_DragStartPos = evt.mousePosition;
                m_DragStartOffset = m_MapOffset;
            }
        }

        /// <summary>
        /// 鼠标移动事件
        /// </summary>
        private void OnMouseMove(MouseMoveEvent evt)
        {
            if (m_IsDragging)
            {
                Rect rect = m_GuiContainer.contentRect;
                Vector2 localPos = m_GuiContainer.WorldToLocal(evt.mousePosition);
                m_MapOffset = m_DragStartOffset + (localPos - m_GuiContainer.WorldToLocal(m_DragStartPos));
                // 限制边界
                float mapWidthScaled = m_MapWidth * m_Zoom;
                float mapHeightScaled = m_MapHeight * m_Zoom;
                m_MapOffset.x = Mathf.Clamp(m_MapOffset.x, -mapWidthScaled + 100, rect.width - 100);
                m_MapOffset.y = Mathf.Clamp(m_MapOffset.y, -mapHeightScaled + 100, rect.height - 100);
                // 手动触发 IMGUI 重绘
                m_GuiContainer.MarkDirtyRepaint();
                evt.StopPropagation();
            }
        }

        /// <summary>
        /// 鼠标抬起事件
        /// </summary>
        private void OnMouseUp(MouseUpEvent evt)
        {
            if (evt.button == (int)MouseButton.LeftMouse)
            {
                m_IsDragging = false;
            }
        }

        /// <summary>
        /// 处理鼠标事件
        /// </summary>
        private void HandleMouseEvent(Event e)
        {
            Rect rect = m_GuiContainer.contentRect;
            // 判断鼠标是否在地图显示区域内
            bool isMouseInRect = rect.Contains(e.mousePosition);
            if (isMouseInRect)
            {
                m_MousePos = e.mousePosition;
                m_TileX = Mathf.FloorToInt((m_MousePos.x - m_MapOffset.x) / MapToolConstants.GridSize);
                m_TileY = Mathf.FloorToInt((m_MousePos.y - m_MapOffset.y) / MapToolConstants.GridSize);

                // 更新待放置预览位置
                if (m_PendingPlace != null)
                {
                    float worldX = (e.mousePosition.x - m_MapOffset.x) / m_Zoom;
                    float worldY = (e.mousePosition.y - m_MapOffset.y) / m_Zoom;
                    // 吸附到网格中心
                    float gridSize = MapToolConstants.GridSize;
                    worldX = Mathf.Floor(worldX / gridSize) * gridSize;
                    worldY = Mathf.Floor(worldY / gridSize) * gridSize;
                    m_PendingPlace.Position = new Vector2(worldX, worldY);
                    // 检查是否可以放置
                    m_PreviewValid = CanPlaceObject(m_PendingPlace, out m_PreviewReason);
                    GUI.changed = true;
                }
            }
            else
            {
                // 鼠标不在区域内，清空坐标
                m_MousePos = Vector2.zero;
                m_TileX = -1;
                m_TileY = -1;
            }
            // Escape 键取消选中资源
            if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Escape)
            {
                if (m_PendingPlace != null)
                {
                    m_PendingPlace = null;
                    m_SelectedObjectId = null;
                    Debug.Log("取消选中资源");
                    e.Use();
                    return;
                }
            }

            // 鼠标滚轮缩放
            if (e.type == EventType.ScrollWheel && isMouseInRect)
            {
                OnScrollZoom(e);
            }
            // 路径编辑：Ctrl+左键添加路径点 / Ctrl+右键删除路径点
            if (isMouseInRect && e.type == EventType.MouseDown && e.control)
            {
                OnCtrlPathEditing(e);
            }
            // 鼠标右键：删除物体 或 拖动地图
            if (isMouseInRect && e.type == EventType.MouseDown && e.button == 1)
            {
                // 检查是否点击了某个物体
                PlacedObject clickedObj = GetObjectAtPosition(e.mousePosition);
                if (clickedObj != null)
                {
                    // 删除物体
                    m_PlacedObjects.Remove(clickedObj);
                    Debug.Log($"删除物体: {clickedObj.Name}");
                    RefreshSceneObjects(); // 同步到场景
                    e.Use();
                    return;
                }
            }
            // 鼠标左键：放置物体、拖动物体 或 切换分区解锁
            if (isMouseInRect && e.type == EventType.MouseDown && e.button == 0)
            {
                // 如果有待放置预览，就放置物体
                if (m_PendingPlace != null)
                {
                    // 检查是否可以放置
                    if (!m_PreviewValid)
                    {
                        Debug.LogWarning($"无法放置: {m_PreviewReason}");
                        ShowMessage(m_PreviewReason);
                        e.Use();
                        return;
                    }

                    // 创建新物体（复制预览对象的所有属性）
                    PlacedObject newObj = new PlacedObject
                    {
                        Id = m_PendingPlace.Id,
                        TemplateId = m_PendingPlace.TemplateId,
                        Name = m_PendingPlace.Name,
                        Type = m_PendingPlace.Type,
                        Position = m_PendingPlace.Position,
                        Width = m_PendingPlace.Width,
                        Height = m_PendingPlace.Height,
                        PreviewTexture = m_PendingPlace.PreviewTexture,
                        // 复制模板属性
                        AnimName = m_PendingPlace.AnimName,
                        Loop = m_PendingPlace.Loop,
                        Speed = m_PendingPlace.Speed,
                        Interactable = m_PendingPlace.Interactable,
                        InteractType = m_PendingPlace.InteractType,
                        SortOrder = m_PendingPlace.SortOrder,
                        CollisionEnabled = m_PendingPlace.CollisionEnabled,
                        DialogBubble = m_PendingPlace.DialogBubble,
                        FacingDirection = m_PendingPlace.FacingDirection,
                        ShadowEnabled = m_PendingPlace.ShadowEnabled,
                        IsCollectible = m_PendingPlace.IsCollectible,
                        RespawnTime = m_PendingPlace.RespawnTime,
                    };

                    m_PlacedObjects.Add(newObj);
                    Debug.Log($"放置物体: {newObj.Name} 位置: ({newObj.Position.x}, {newObj.Position.y})");
                    RefreshSceneObjects(); // 同步到场景

                    // 放置人物后自动选中为编辑目标
                    if (newObj.Type == MapObjectType.Character)
                    {
                        SelectEditTarget(newObj);
                    }

                    // 放置后保持预览（可以连续放置）
                    // m_PendingPlace = null;

                    e.Use();
                    return;
                }
                // 检查是否点击了某个物体，开始拖动（并记录潜在点击目标，抬起时若未拖动则选中）
                PlacedObject clickedObj = GetObjectAtPosition(e.mousePosition);
                if (clickedObj != null)
                {
                    m_DraggingObject = clickedObj;
                    m_PotentialClickObject = clickedObj;
                    m_HasMoved = false;
                    m_MouseDownPos = e.mousePosition;
                    float worldX = (e.mousePosition.x - m_MapOffset.x) / m_Zoom;
                    float worldY = (e.mousePosition.y - m_MapOffset.y) / m_Zoom;
                    m_DragOffset = new Vector2(worldX - clickedObj.Position.x, worldY - clickedObj.Position.y);
                    e.Use();
                    return;
                }
                // 否则切换分区解锁状态
                if (m_RegionUnlocked != null)
                {
                    Rect mapRect = new Rect(m_MapOffset.x, m_MapOffset.y, m_MapWidth * m_Zoom, m_MapHeight * m_Zoom);
                    float regionWidth = mapRect.width / m_RegionCols;
                    float regionHeight = mapRect.height / m_RegionRows;
                    // 计算点击位置在哪个分区
                    float localX = e.mousePosition.x - mapRect.x;
                    float localY = e.mousePosition.y - mapRect.y;
                    int col = Mathf.FloorToInt(localX / regionWidth);
                    int row = Mathf.FloorToInt(localY / regionHeight);
                    // 如果在分区范围内，切换解锁状态
                    int index = row * m_RegionCols + col;
                    if (row >= 0 && row < m_RegionRows && col >= 0 && col < m_RegionCols && index < m_RegionCount)
                    {
                        m_RegionUnlocked[row, col] = !m_RegionUnlocked[row, col];
                        Debug.Log($"切换分区解锁: 行={row}, 列={col}, 解锁={m_RegionUnlocked[row, col]}");
                        // 解锁/锁定分区后，同步更新包围盒宽高
                        UpdateMapBoundsFromUnlockedRegions();
                        e.Use();
                        return;
                    }
                }
            }
            // 左键拖动物体
            if (isMouseInRect && m_DraggingObject != null)
            {
                if (e.type == EventType.MouseDrag && e.button == 0)
                {
                    // 移动超过阈值视为拖动（不是点击选中）
                    if (!m_HasMoved && Vector2.Distance(e.mousePosition, m_MouseDownPos) > MapToolConstants.DragThreshold)
                        m_HasMoved = true;
                    float worldX = (e.mousePosition.x - m_MapOffset.x) / m_Zoom - m_DragOffset.x;
                    float worldY = (e.mousePosition.y - m_MapOffset.y) / m_Zoom - m_DragOffset.y;
                    m_DraggingObject.Position = new Vector2(worldX, worldY);
                    GUI.changed = true;
                    e.Use();
                }
                else if (e.type == EventType.MouseUp && e.button == 0)
                {
                    if (m_HasMoved) RefreshSceneObjects(); // 拖动结束后同步位置到场景
                    m_DraggingObject = null;
                    // 未发生拖动 → 视为单击选中，成为路径编辑目标
                    if (!m_HasMoved && m_PotentialClickObject != null)
                    {
                        SelectEditTarget(m_PotentialClickObject);
                    }
                    m_PotentialClickObject = null;
                    e.Use();
                }
            }
            // 鼠标右键拖动地图
            if (isMouseInRect)
            {
                if (e.type == EventType.MouseDown && e.button == 1)
                {
                    m_IsDragging = true;
                    m_DragStartPos = e.mousePosition;
                    m_DragStartOffset = m_MapOffset;
                    e.Use();
                }
                else if (e.type == EventType.MouseDrag && m_IsDragging && e.button == 1)
                {
                    m_MapOffset = m_DragStartOffset + (e.mousePosition - m_DragStartPos);
                    // 限制边界
                    float mapWidthScaled = m_MapWidth * m_Zoom;
                    float mapHeightScaled = m_MapHeight * m_Zoom;
                    m_MapOffset.x = Mathf.Clamp(m_MapOffset.x, -mapWidthScaled + 100, rect.width - 100);
                    m_MapOffset.y = Mathf.Clamp(m_MapOffset.y, -mapHeightScaled + 100, rect.height - 100);
                    GUI.changed = true;
                    e.Use();
                }
                else if (e.type == EventType.MouseUp && e.button == 1)
                {
                    m_IsDragging = false;
                    e.Use();
                }
            }
            // 消费事件，防止冒泡（只对鼠标事件类型调用 Use，Repaint/Layout 不能调）
            if (m_IsDragging && e.type != EventType.Repaint && e.type != EventType.Layout)
                e.Use();
        }

        /// <summary>
        /// 处理鼠标滚轮缩放（限制最小缩放为地图刚好填满视口，最大缩放为常量上限）
        /// </summary>
        /// <param name="e">鼠标事件</param>
        private void OnScrollZoom(Event e)
        {
            Rect rect = m_GuiContainer.contentRect;
            float oldZoom = m_Zoom;
            m_Zoom -= e.delta.y * 0.1f;
            // 最小缩放：地图刚好填满显示框
            float minZoomX = rect.width / m_MapWidth;
            float minZoomY = rect.height / m_MapHeight;
            float minZoom = Mathf.Max(minZoomX, minZoomY);
            m_Zoom = Mathf.Clamp(m_Zoom, minZoom, MapToolConstants.MaxZoom);
            // 缩放被限制在地图填满视口时，调整偏移让地图居中
            if (m_Zoom == minZoom && oldZoom > minZoom)
            {
                m_MapOffset = new Vector2(
                    (rect.width - m_MapWidth * m_Zoom) / 2,
                    (rect.height - m_MapHeight * m_Zoom) / 2
                );
            }
        }

        /// <summary>
        /// 处理 Ctrl + 鼠标路径编辑：左键添加路径点，右键删除最后一个路径点
        /// 选中物体且开启绘制时编辑物体路径，否则编辑全局路径
        /// </summary>
        /// <param name="e">鼠标事件</param>
        private void OnCtrlPathEditing(Event e)
        {
            // 路径点吸附到 32px 网格（路径网格结合人物使用）
            Vector2 worldPos = SnapToGrid(ScreenToWorld(e.mousePosition));
            if (e.button == 0) // Ctrl+左键：添加路径点
            {
                if (m_EditTarget != null && m_IsPathDrawing)
                {
                    m_EditTarget.PathPoints.Add(worldPos);
                    Debug.Log($"为 {m_EditTarget.Name} 添加路径点: {worldPos}, 总数: {m_EditTarget.PathPoints.Count}");
                    RefreshPathPointList();
                    RefreshSceneObjects();
                }
                else
                {
                    m_PathPoints.Add(worldPos);
                    Debug.Log($"添加全局路径点: {worldPos}, 总数: {m_PathPoints.Count}");
                    RefreshSceneObjects();
                }
            }
            else if (e.button == 1) // Ctrl+右键：删除最后一个路径点
            {
                if (m_EditTarget != null && m_IsPathDrawing && m_EditTarget.PathPoints.Count > 0)
                {
                    m_EditTarget.PathPoints.RemoveAt(m_EditTarget.PathPoints.Count - 1);
                    Debug.Log($"删除 {m_EditTarget.Name} 最后路径点, 剩余: {m_EditTarget.PathPoints.Count}");
                    RefreshPathPointList();
                    RefreshSceneObjects();
                }
                else if (m_PathPoints.Count > 0)
                {
                    m_PathPoints.RemoveAt(m_PathPoints.Count - 1);
                    Debug.Log($"删除最后一个全局路径点, 剩余: {m_PathPoints.Count}");
                    RefreshSceneObjects();
                }
            }
            e.Use();
        }

        /// <summary>
        /// 屏幕坐标转世界坐标
        /// </summary>
        /// <param name="screenPos">屏幕坐标</param>
        /// <returns>世界坐标</returns>
        private Vector2 ScreenToWorld(Vector2 screenPos)
        {
            return new Vector2(
                (screenPos.x - m_MapOffset.x) / m_Zoom,
                (screenPos.y - m_MapOffset.y) / m_Zoom
            );
        }

        /// <summary>
        /// 世界坐标吸附到网格（GridSize 整数倍）
        /// </summary>
        /// <param name="worldPos">原始世界坐标</param>
        /// <returns>吸附后的世界坐标</returns>
        private Vector2 SnapToGrid(Vector2 worldPos)
        {
            return new Vector2(
                Mathf.Round(worldPos.x / MapToolConstants.GridSize) * MapToolConstants.GridSize,
                Mathf.Round(worldPos.y / MapToolConstants.GridSize) * MapToolConstants.GridSize
            );
        }

        /// <summary>
        /// 获取指定位置的物体
        /// </summary>
        private PlacedObject GetObjectAtPosition(Vector2 screenPos)
        {
            // 从后往前遍历（后面绘制的在最上面）
            for (int i = m_PlacedObjects.Count - 1; i >= 0; i--)
            {
                var obj = m_PlacedObjects[i];
                float screenX = m_MapOffset.x + obj.Position.x * m_Zoom;
                float screenY = m_MapOffset.y + obj.Position.y * m_Zoom;
                float width = obj.Width * 32 * m_Zoom;
                float height = obj.Height * MapToolConstants.GridSize * m_Zoom;
                Rect objRect = new Rect(screenX, screenY, width, height);
                if (objRect.Contains(screenPos))
                {
                    return obj;
                }
            }
            return null;
        }

    }
}
