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
        /// 处理鼠标事件：位置回写 → Esc 取消 → 滚轮缩放 → Ctrl 路径编辑 → 左键（交通/放置/选中） → 拖动/平移
        /// </summary>
        /// <param name="e">IMGUI 事件</param>
        private void HandleMouseEvent(Event e)
        {
            Rect rect = m_GuiContainer.contentRect;
            bool isMouseInRect = rect.Contains(e.mousePosition);

            UpdateMousePosition(e.mousePosition, isMouseInRect);

            // Escape 键取消选中资源（不依赖鼠标是否在区域内）
            if (HandleEscapeCancel(e))
                return;

            // 鼠标滚轮缩放
            if (e.type == EventType.ScrollWheel && isMouseInRect)
                OnScrollZoom(e);

            // 路径编辑：Ctrl+左键添加路径点 / Ctrl+右键删除路径点
            if (isMouseInRect && e.type == EventType.MouseDown && e.control)
                OnCtrlPathEditing(e);

            // 鼠标左键：交通工具 > 放置物体 > 选中并准备拖动
            if (isMouseInRect && e.type == EventType.MouseDown && e.button == 0)
            {
                if (HandlePrimaryMouseDown(e))
                    return;
            }

            if (isMouseInRect)
            {
                // 左键拖动物体（抬起时按移动阈值判定拖动/单击选中）
                HandleObjectDrag(e);
                // 鼠标右键：拖动地图
                HandleMapPan(e, rect);
            }

            // 消费事件，防止冒泡（只对鼠标事件类型调用 Use，Repaint/Layout 不能调）
            if (m_IsDragging && e.type != EventType.Repaint && e.type != EventType.Layout)
                e.Use();
        }

        /// <summary>
        /// 回写鼠标位置与命中格子；鼠标离开显示区域时清空坐标，并刷新待放置预览
        /// </summary>
        /// <param name="mousePosition">鼠标屏幕坐标</param>
        /// <param name="isMouseInRect">鼠标是否在地图显示区域内</param>
        private void UpdateMousePosition(Vector2 mousePosition, bool isMouseInRect)
        {
            if (!isMouseInRect)
            {
                // 鼠标不在区域内，清空坐标
                m_MousePos = Vector2.zero;
                m_TileX = -1;
                m_TileY = -1;
                return;
            }

            m_MousePos = mousePosition;
            // 网格坐标统一走公共换算（借鉴 Tilemap GridLayout 口径）
            Vector2 mouseWorld = ScreenToWorld(mousePosition);
            Vector2Int mouseCell = MapToolEditorUtils.WorldToCell(mouseWorld);
            m_TileX = mouseCell.x;
            m_TileY = mouseCell.y;

            // 更新待放置预览位置
            if (m_PendingPlace != null)
            {
                // 吸附到最近格子（公共网格吸附口径）
                m_PendingPlace.Position = MapToolEditorUtils.SnapToGrid(mouseWorld);
                // 检查是否可以放置
                m_PreviewValid = CanPlaceObject(m_PendingPlace, out m_PreviewReason);
                GUI.changed = true;
            }
        }

        /// <summary>
        /// Escape 键取消当前选中的待放置资源
        /// </summary>
        /// <param name="e">IMGUI 事件</param>
        /// <returns>是否已消费该事件</returns>
        private bool HandleEscapeCancel(Event e)
        {
            if (e.type != EventType.KeyDown || e.keyCode != KeyCode.Escape || m_PendingPlace == null)
                return false;

            m_PendingPlace = null;
            m_SelectedObjectId = null;
            MapToolLog.Info("取消选中资源");
            e.Use();
            return true;
        }

        /// <summary>
        /// 左键按下处理：交通工具优先，其次放置预览物体，最后命中物体开始拖动
        /// </summary>
        /// <param name="e">IMGUI 事件</param>
        /// <returns>是否已消费该事件</returns>
        private bool HandlePrimaryMouseDown(Event e)
        {
            // ---TODO 交通规则：交通工具模式（刷格 / 画横道 / 放灯）优先于放置与选中
            if (m_TrafficTool != TrafficTool.None)
            {
                OnTrafficToolClick(e);
                e.Use();
                return true;
            }

            // 如果有待放置预览，就放置物体
            if (m_PendingPlace != null)
            {
                if (!m_PreviewValid)
                {
                    MapToolLog.Warn($"无法放置: {m_PreviewReason}");
                    m_StatusBar.ShowMessage(m_PreviewReason);
                    e.Use();
                    return true;
                }

                PlacedObject newObj = CreatePlacedObjectFromPreview(m_PendingPlace);
                m_PlacedObjects.Add(newObj);
                MapToolLog.Info($"放置物体: {newObj.Name} 位置: ({newObj.Position.x}, {newObj.Position.y})");
                RefreshSceneObjects(); // 同步到场景

                // 放置人物后自动选中为编辑目标
                if (newObj.Type == MapObjectType.Character)
                {
                    SelectEditTarget(newObj);
                }

                // 放置后保持预览（可以连续放置）
                e.Use();
                return true;
            }

            // 检查是否点击了某个物体，开始拖动（并记录潜在点击目标，抬起时若未拖动则选中）
            PlacedObject clickedObj = GetObjectAtPosition(e.mousePosition);
            if (clickedObj != null)
            {
                m_DraggingObject = clickedObj;
                m_PotentialClickObject = clickedObj;
                m_HasMoved = false;
                m_MouseDownPos = e.mousePosition;
                Vector2 worldPos = ScreenToWorld(e.mousePosition);
                m_DragOffset = new Vector2(worldPos.x - clickedObj.Position.x, worldPos.y - clickedObj.Position.y);
                e.Use();
                return true;
            }

            return false;
        }

        /// <summary>
        /// 由预览对象生成待入库的摆放物（模板属性一并继承）
        /// </summary>
        /// <param name="preview">预览对象</param>
        /// <returns>新的摆放物实例</returns>
        private static PlacedObject CreatePlacedObjectFromPreview(PlacedObject preview)
        {
            return new PlacedObject
            {
                Id = preview.Id,
                TemplateId = preview.TemplateId,
                Name = preview.Name,
                Type = preview.Type,
                LayerType = preview.LayerType,
                Position = preview.Position,
                Width = preview.Width,
                Height = preview.Height,
                PixelWidth = preview.PixelWidth,
                PixelHeight = preview.PixelHeight,
                Stretch = preview.Stretch,
                PreviewTexture = preview.PreviewTexture,
                // 复制模板属性
                AnimName = preview.AnimName,
                Loop = preview.Loop,
                Speed = preview.Speed,
                Interactable = preview.Interactable,
                InteractType = preview.InteractType,
                SortOrder = preview.SortOrder,
                CollisionEnabled = preview.CollisionEnabled,
                DialogBubble = preview.DialogBubble,
                FacingDirection = preview.FacingDirection,
                ShadowEnabled = preview.ShadowEnabled,
                IsCollectible = preview.IsCollectible,
                RespawnTime = preview.RespawnTime,
                // 复制交通默认分类（模板类别继承而来，实例右栏可再调整）
                TrafficRole = preview.TrafficRole,
                IsVehicle = preview.IsVehicle,
            };
        }

        /// <summary>
        /// 左键拖动已命中物体：按下超过阈值才算拖动，抬起未移动则视为单击选中
        /// </summary>
        /// <param name="e">IMGUI 事件</param>
        private void HandleObjectDrag(Event e)
        {
            if (m_DraggingObject == null) return;

            if (e.type == EventType.MouseDrag && e.button == 0)
            {
                // 移动超过阈值视为拖动（不是点击选中）
                if (!m_HasMoved && Vector2.Distance(e.mousePosition, m_MouseDownPos) > MapToolConstants.DragThreshold)
                    m_HasMoved = true;

                Vector2 worldPos = ScreenToWorld(e.mousePosition);
                m_DraggingObject.Position = new Vector2(worldPos.x - m_DragOffset.x, worldPos.y - m_DragOffset.y);
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

        /// <summary>
        /// 右键拖动地图并限制边界
        /// </summary>
        /// <param name="e">IMGUI 事件</param>
        /// <param name="rect">可视区域</param>
        private void HandleMapPan(Event e, Rect rect)
        {
            if (e.type == EventType.MouseDown && e.button == 1)
            {
                m_IsDragging = true;
                m_DragStartPos = e.mousePosition;
                m_DragStartOffset = m_MapOffset;
                e.Use();
                return;
            }

            if (e.type == EventType.MouseDrag && m_IsDragging && e.button == 1)
            {
                m_MapOffset = m_DragStartOffset + (e.mousePosition - m_DragStartPos);
                // 限制边界
                float mapWidthScaled = m_MapWidth * m_Zoom;
                float mapHeightScaled = m_MapHeight * m_Zoom;
                m_MapOffset.x = Mathf.Clamp(m_MapOffset.x, -mapWidthScaled + 100, rect.width - 100);
                m_MapOffset.y = Mathf.Clamp(m_MapOffset.y, -mapHeightScaled + 100, rect.height - 100);
                GUI.changed = true;
                e.Use();
                return;
            }

            if (e.type == EventType.MouseUp && e.button == 1)
            {
                m_IsDragging = false;
                e.Use();
            }
        }

        /// <summary>
        /// 处理鼠标滚轮缩放（下限保证整图完整落在视口内，最大缩放为常量上限）
        /// </summary>
        /// <param name="e">鼠标事件</param>
        private void OnScrollZoom(Event e)
        {
            Rect rect = m_GuiContainer.contentRect;
            float oldZoom = m_Zoom;
            m_Zoom -= e.delta.y * 0.1f;
            // 最小缩放：取宽向/高向的较小值，保证整图完整落在视口内（高度主导），
            // 避免宽扁视口按宽度铺满后上下被裁，把边缘菱形切成半格窄条。
            float minZoomX = rect.width / m_MapWidth;
            float minZoomY = rect.height / m_MapHeight;
            float minZoom = Mathf.Min(minZoomX, minZoomY);
            m_Zoom = Mathf.Clamp(m_Zoom, minZoom, MapToolConstants.MaxZoom);
            // 缩放被限制在整图可见的下限时，调整偏移让地图在视口内居中
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
                    MapToolLog.Info($"为 {m_EditTarget.Name} 添加路径点: {worldPos}, 总数: {m_EditTarget.PathPoints.Count}");
                    RefreshPathPointList();
                    RefreshSceneObjects();
                }
                else
                {
                    m_PathPoints.Add(worldPos);
                    MapToolLog.Info($"添加全局路径点: {worldPos}, 总数: {m_PathPoints.Count}");
                    RefreshSceneObjects();
                }
            }
            else if (e.button == 1) // Ctrl+右键：删除最后一个路径点
            {
                if (m_EditTarget != null && m_IsPathDrawing && m_EditTarget.PathPoints.Count > 0)
                {
                    m_EditTarget.PathPoints.RemoveAt(m_EditTarget.PathPoints.Count - 1);
                    MapToolLog.Info($"删除 {m_EditTarget.Name} 最后路径点, 剩余: {m_EditTarget.PathPoints.Count}");
                    RefreshPathPointList();
                    RefreshSceneObjects();
                }
                else if (m_PathPoints.Count > 0)
                {
                    m_PathPoints.RemoveAt(m_PathPoints.Count - 1);
                    MapToolLog.Info($"删除最后一个全局路径点, 剩余: {m_PathPoints.Count}");
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
        /// 世界坐标吸附到网格（转发公共网格吸附口径，GridSize 整数倍）
        /// </summary>
        /// <param name="worldPos">原始世界坐标</param>
        /// <returns>吸附后的世界坐标</returns>
        private Vector2 SnapToGrid(Vector2 worldPos)
        {
            return MapToolEditorUtils.SnapToGrid(worldPos);
        }

        /// <summary>
        /// 获取指定位置的物体
        /// </summary>
        private PlacedObject GetObjectAtPosition(Vector2 screenPos)
        {
            // 与绘制共用 GetPlacedObjectsSorted 同一排序序列：从最上层（序列末尾）向前取
            // 第一个命中项，避免点到被上层物件压住的下层物件。
            var sorted = GetPlacedObjectsSorted();
            for (int i = sorted.Count - 1; i >= 0; i--)
            {
                var obj = sorted[i];
                // 与绘制同一口径：被顶部层开关隐藏的物件不可被点中（所见即可点）
                if (IsLayerSwitchHidden(obj)) continue;
                // 命中尺寸口径与绘制/排序一致：预览贴图 → PSD 像素 → 格子数 × 网格尺寸
                GetObjectPixelSize(obj, out float baseW, out float baseH);
                Rect objRect = new Rect(
                    m_MapOffset.x + obj.Position.x * m_Zoom,
                    m_MapOffset.y + obj.Position.y * m_Zoom,
                    baseW * m_Zoom,
                    baseH * m_Zoom);
                if (objRect.Contains(screenPos))
                {
                    return obj;
                }
            }
            return null;
        }

    }
}
