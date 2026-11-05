/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  SceneEditModule.Render.cs
 * author:    云毅
 * created:   2026
 * descrip:   场景编辑模块 - 私有方法（场景渲染与绘制系列）
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
    /// 场景编辑模块 - 渲染绘制系列私有方法
    /// </summary>
    public partial class SceneEditModule
    {
        /// <summary>
        /// 场景GUI渲染（每帧由 IMGUI 容器回调）
        /// </summary>
        private void OnSceneGUI()
        {
            Rect rect = m_GuiContainer.contentRect;
            if (rect.width <= 0 || rect.height <= 0) return;
            // 绘制画布背景
            EditorGUI.DrawRect(rect, new Color(0.2f, 0.2f, 0.2f));
            // 计算地图显示区域（受缩放与偏移影响）
            // 左上角原点坐标系：与运行时 JSON 数据格式一致
            Rect mapRect = new Rect(
                m_MapOffset.x,
                m_MapOffset.y,
                m_MapWidth * m_Zoom,
                m_MapHeight * m_Zoom
            );
            DrawBackgroundLayer(mapRect);
            DrawMapLayer(mapRect);
            DrawPlacedObjects(mapRect);
            DrawPendingPlacePreview(mapRect);
            DrawPathGridLayer(mapRect);
            DrawEditTargetOverlay(mapRect);
            DrawRegionMist(mapRect);
            DrawBoundingBoxLayer(mapRect);
            DrawGrid(mapRect);

            // 绘制临时提示消息（放置被拒绝等）
            if (!string.IsNullOrEmpty(m_TempMessage) && Time.realtimeSinceStartup < m_TempMessageTime)
            {
                GUIStyle style = new GUIStyle(GUI.skin.label)
                {
                    normal = { textColor = Color.red },
                    fontSize = 14,
                    alignment = TextAnchor.MiddleCenter
                };
                GUI.Label(new Rect(0, rect.height - 40, rect.width, 30), m_TempMessage, style);
            }

            // 更新状态栏
            UpdateStatusBar();
            // 最后处理鼠标事件，避免影响绘制
            Event e = Event.current;
            HandleMouseEvent(e);
        }

        /// <summary>
        /// 绘制地图背景层
        /// </summary>
        /// <param name="mapRect">地图显示区域</param>
        private void DrawBackgroundLayer(Rect mapRect)
        {
            if (!m_ShowBackgroundLayer) return;
            EditorGUI.DrawRect(mapRect, new Color(0.8f, 0.7f, 0.6f));
        }

        /// <summary>
        /// 绘制地图层（预制体预览图 + 关卡背景图）
        /// </summary>
        /// <param name="mapRect">地图显示区域</param>
        private void DrawMapLayer(Rect mapRect)
        {
            if (!m_ShowMapLayer || m_MapPrefab == null) return;
            // 预制体预览纹理仅获取一次
            Texture previewTexture = GetMapPrefabPreviewTexture();
            // 绘制当前关卡背景图，未配置或加载失败则回退预制体预览图
            Texture bgTexture = LoadRegionBackground(m_CurrentLevelId);
            if (bgTexture != null)
                GUI.DrawTexture(mapRect, bgTexture, ScaleMode.StretchToFill);
            else if (previewTexture != null)
                GUI.DrawTexture(mapRect, previewTexture, ScaleMode.StretchToFill);
        }

        /// <summary>
        /// 绘制已放置物体层（按 Y 排序计算遮挡，按图层开关过滤）
        /// </summary>
        /// <param name="mapRect">地图显示区域</param>
        private void DrawPlacedObjects(Rect mapRect)
        {
            // 排序逻辑：
            // - YSortEnabled=true 的物件：有效SortOrder = 基础SortOrder + Y坐标偏移（Y越大越靠前）
            // - YSortEnabled=false 的物件：用固定 SortOrder
            var sortedObjects = m_PlacedObjects
                .OrderBy(obj => GetEffectiveSortOrder(obj))
                .ThenBy(obj => obj.Position.y)
                .ToList();
            foreach (var obj in sortedObjects)
            {
                // 根据图层开关过滤
                if (obj.Type == MapObjectType.Building && !m_ShowBuildingLayer) continue; // 建筑层
                if (obj.Type == MapObjectType.Character && !m_ShowCharacterLayer) continue; // 人物层
                if (obj.Type == MapObjectType.Prop && !m_ShowObjectLayer) continue; // 物件层
                // 世界坐标转屏幕坐标
                float width = obj.Width * MapToolConstants.GridSize * m_Zoom;
                float height = obj.Height * MapToolConstants.GridSize * m_Zoom;
                Rect objRect = new Rect(WorldToScreen(obj.Position), new Vector2(width, height));
                // 有预览图则绘制预览图，否则绘制类型占位色块
                if (obj.PreviewTexture != null)
                {
                    GUI.DrawTexture(objRect, obj.PreviewTexture, ScaleMode.StretchToFill);
                }
                else
                {
                    EditorGUI.DrawRect(objRect, GetObjectPlaceholderColor(obj.Type));
                }
                // 绘制物体名称
                GUI.Label(new Rect(objRect.x, objRect.y - 20, 100, 20), obj.Name,
                    new GUIStyle(GUI.skin.label) { normal = { textColor = Color.white } });
            }
        }

        /// <summary>
        /// 计算物件的有效排序值（开启YSort时根据Y坐标自动调整）
        /// </summary>
        private float GetEffectiveSortOrder(PlacedObject obj)
        {
            if (obj.YSortEnabled)
            {
                // YSort模式：Y坐标越大（越靠下），排序值越大，越靠前
                // 基础SortOrder + Y坐标 * 系数 + 偏移
                return obj.SortOrder + obj.Position.y * 0.01f + obj.YSortOffset;
            }
            else
            {
                // 固定模式：用基础SortOrder
                return obj.SortOrder;
            }
        }

        /// <summary>
        /// 绘制路径网格层（网格点阵 + 全局路径点 + 连线）
        /// </summary>
        /// <param name="mapRect">地图显示区域</param>
        private void DrawPathGridLayer(Rect mapRect)
        {
            if (!m_ShowPathGrid) return;
            // 路径网格点阵（32px 间距）
            float gridSize = MapToolConstants.GridSize * m_Zoom;
            Handles.color = new Color(0, 1, 1, 0.3f);
            for (float x = mapRect.x; x < mapRect.xMax; x += gridSize)
            {
                for (float y = mapRect.y; y < mapRect.yMax; y += gridSize)
                {
                    Handles.DrawSolidDisc(new Vector3(x, y), Vector3.forward, 2);
                }
            }
            // 全局路径点（历史数据兼容）
            for (int i = 0; i < m_PathPoints.Count; i++)
            {
                Vector2 screenPos = WorldToScreen(m_PathPoints[i]);
                Handles.DrawSolidDisc(new Vector3(screenPos.x, screenPos.y), Vector3.forward, 5);
            }
            // 全局路径连线
            for (int i = 0; i < m_PathPoints.Count - 1; i++)
            {
                Vector2 p1 = WorldToScreen(m_PathPoints[i]);
                Vector2 p2 = WorldToScreen(m_PathPoints[i + 1]);
                Handles.DrawLine(new Vector3(p1.x, p1.y), new Vector3(p2.x, p2.y));
            }
        }

        /// <summary>
        /// 绘制选中编辑目标（人物）的高亮框与行走路径
        /// </summary>
        /// <param name="mapRect">地图显示区域</param>
        private void DrawEditTargetOverlay(Rect mapRect)
        {
            if (m_EditTarget == null) return;
            // 选中物体高亮边框（黄色）
            float selW = m_EditTarget.Width * MapToolConstants.GridSize * m_Zoom;
            float selH = m_EditTarget.Height * MapToolConstants.GridSize * m_Zoom;
            DrawSelectionBorder(new Rect(WorldToScreen(m_EditTarget.Position), new Vector2(selW, selH)));
            // 该物体无路径则直接结束
            if (m_EditTarget.PathPoints.Count == 0) return;
            Handles.color = new Color(1f, 0.55f, 0f, 0.95f);
            // 路径点（列表选中项画大圆） + 序号
            for (int i = 0; i < m_EditTarget.PathPoints.Count; i++)
            {
                Vector2 screenPos = WorldToScreen(m_EditTarget.PathPoints[i]);
                float discRadius = (i == m_SelectedPathPointIndex) ? 8 : 5;
                Handles.DrawSolidDisc(new Vector3(screenPos.x, screenPos.y), Vector3.forward, discRadius);
                GUI.Label(new Rect(screenPos.x + 6, screenPos.y - 12, 40, 16), (i + 1).ToString(),
                    new GUIStyle(GUI.skin.label) { normal = { textColor = Color.yellow }, fontSize = 11 });
            }
            // 路径连线 + 方向箭头
            for (int i = 0; i < m_EditTarget.PathPoints.Count - 1; i++)
            {
                Vector2 p1 = WorldToScreen(m_EditTarget.PathPoints[i]);
                Vector2 p2 = WorldToScreen(m_EditTarget.PathPoints[i + 1]);
                Handles.DrawLine(new Vector3(p1.x, p1.y), new Vector3(p2.x, p2.y));
                DrawPathArrow(p1, p2);
            }
        }

        /// <summary>
        /// 绘制选中物体高亮边框（黄色矩形）
        /// </summary>
        /// <param name="rect">物体屏幕矩形</param>
        private void DrawSelectionBorder(Rect rect)
        {
            EditorGUI.DrawRect(new Rect(rect.x - 2, rect.y - 2, rect.width + 4, 2), Color.yellow);
            EditorGUI.DrawRect(new Rect(rect.x - 2, rect.y + rect.height, rect.width + 4, 2), Color.yellow);
            EditorGUI.DrawRect(new Rect(rect.x - 2, rect.y, 2, rect.height), Color.yellow);
            EditorGUI.DrawRect(new Rect(rect.x + rect.width, rect.y, 2, rect.height), Color.yellow);
        }

        /// <summary>
        /// 绘制路径方向箭头（指向下一路径点）
        /// </summary>
        /// <param name="p1">起点屏幕坐标</param>
        /// <param name="p2">终点屏幕坐标</param>
        private void DrawPathArrow(Vector2 p1, Vector2 p2)
        {
            Vector2 dir = (p2 - p1).normalized;
            Vector2 right = new Vector2(-dir.y, dir.x);
            const float arrowLength = 10f;
            Vector2 tip = p2;
            Handles.DrawLine(new Vector3(tip.x, tip.y),
                new Vector3(tip.x - dir.x * arrowLength + right.x * arrowLength * 0.5f,
                    tip.y - dir.y * arrowLength + right.y * arrowLength * 0.5f));
            Handles.DrawLine(new Vector3(tip.x, tip.y),
                new Vector3(tip.x - dir.x * arrowLength - right.x * arrowLength * 0.5f,
                    tip.y - dir.y * arrowLength - right.y * arrowLength * 0.5f));
        }

        /// <summary>
        /// 绘制包围盒层（整图绿色边界）
        /// </summary>
        /// <param name="mapRect">地图显示区域</param>
        private void DrawBoundingBoxLayer(Rect mapRect)
        {
            if (!m_ShowBoundingBox) return;
            // 整图四周边界
            EditorGUI.DrawRect(new Rect(mapRect.x, mapRect.y, mapRect.width, 2), Color.green);
            EditorGUI.DrawRect(new Rect(mapRect.x, mapRect.yMax - 2, mapRect.width, 2), Color.green);
            EditorGUI.DrawRect(new Rect(mapRect.x, mapRect.y, 2, mapRect.height), Color.green);
            EditorGUI.DrawRect(new Rect(mapRect.xMax - 2, mapRect.y, 2, mapRect.height), Color.green);
        }

        /// <summary>
        /// 绘制网格
        /// </summary>
        private void DrawGrid(Rect mapRect)
        {
            // 摆放网格层
            if (m_ShowPlaceGrid)
            {
                // 统一核心：MapGridDrawUtil.ComputeGridLinePositions（与游戏同几何）
                var style = GameLib.MapGridDrawUtil.DefaultStyle();
                Handles.color = style.lineColor;
                float mapW = mapRect.width / m_Zoom;
                float mapH = mapRect.height / m_Zoom;
                int gridCols = Mathf.RoundToInt(mapW / MapToolConstants.GridSize);
                GameLib.MapGridDrawUtil.ComputeGridLinePositions(mapW, mapH, gridCols,
                    out float[] vLines, out float[] hLines, out Rect mapRectOut);
                foreach (float w in vLines)
                    Handles.DrawLine(new Vector3(mapRect.x + w * m_Zoom, mapRect.y),
                                     new Vector3(mapRect.x + w * m_Zoom, mapRect.yMax));
                foreach (float h in hLines)
                    Handles.DrawLine(new Vector3(mapRect.x, mapRect.y + h * m_Zoom),
                                     new Vector3(mapRect.xMax, mapRect.y + h * m_Zoom));
            }
        }

        /// <summary>
        /// 绘制迷雾覆盖（单关卡模式：无分区迷雾，空实现）
        /// </summary>
        /// <param name="mapRect">地图区域矩形</param>
        private void DrawRegionMist(Rect mapRect)
        {
            // 单关卡=单张地图，无分区解锁/迷雾概念
        }

        /// <summary>
        /// 获取地图预制体的预览纹理（依次尝试 SpriteRenderer / RawImage / Image）
        /// </summary>
        /// <returns>预制体预览纹理，未找到返回 null</returns>
        private Texture GetMapPrefabPreviewTexture()
        {
            var spriteRenderer = m_MapPrefab.GetComponentInChildren<UnityEngine.SpriteRenderer>();
            if (spriteRenderer != null && spriteRenderer.sprite != null)
                return spriteRenderer.sprite.texture;
            var rawImage = m_MapPrefab.GetComponentInChildren<UnityEngine.UI.RawImage>();
            if (rawImage != null && rawImage.texture != null)
                return rawImage.texture;
            var image = m_MapPrefab.GetComponentInChildren<UnityEngine.UI.Image>();
            if (image != null && image.sprite != null)
                return image.sprite.texture;
            return null;
        }

        /// <summary>
        /// 加载分区背景图（按分区ID查背景名，拼接资源路径加载）
        /// </summary>
        /// <param name="regionId">分区ID</param>
        /// <returns>背景纹理，未配置或加载失败返回 null</returns>
        private Texture LoadRegionBackground(string regionId)
        {
            if (!m_LevelBgDict.TryGetValue(regionId, out string bgName))
                return null;
            string bgPath = $"{MapToolConstants.LevelBackgroundDir}/{bgName}.png";
            return UnityEditor.AssetDatabase.LoadAssetAtPath<Texture2D>(bgPath);
        }

        /// <summary>
        /// 获取物体未加载预览图时的占位颜色
        /// </summary>
        /// <param name="type">物体类型</param>
        /// <returns>占位色</returns>
        private Color GetObjectPlaceholderColor(MapObjectType type)
        {
            switch (type)
            {
                case MapObjectType.Building: return new Color(0, 1, 1, 0.5f);  // 建筑：青色
                case MapObjectType.Character: return new Color(1, 1, 0, 0.5f); // 人物：黄色
                case MapObjectType.Prop: return new Color(0, 1, 0, 0.5f);      // 物件：绿色
                default: return Color.cyan;
            }
        }

        /// <summary>
        /// 绘制待放置物体预览（鼠标跟随半透明框）
        /// </summary>
        /// <param name="mapRect">地图显示区域</param>
        private void DrawPendingPlacePreview(Rect mapRect)
        {
            if (m_PendingPlace == null) return;

            // 世界坐标转屏幕坐标
            float width = m_PendingPlace.Width * MapToolConstants.GridSize * m_Zoom;
            float height = m_PendingPlace.Height * MapToolConstants.GridSize * m_Zoom;
            Rect previewRect = new Rect(WorldToScreen(m_PendingPlace.Position), new Vector2(width, height));

            // 根据是否合法选择颜色
            Color previewColor = m_PreviewValid
                ? new Color(0, 1, 1, 0.3f)   // 合法：半透明青色
                : new Color(1, 0, 0, 0.5f);  // 不合法：半透明红色

            EditorGUI.DrawRect(previewRect, previewColor);

            // 绘制边框
            Handles.color = m_PreviewValid ? Color.cyan : Color.red;
            Handles.DrawSolidRectangleWithOutline(previewRect, Color.clear, m_PreviewValid ? Color.cyan : Color.red);

            // 绘制物体名称
            GUI.Label(new Rect(previewRect.x, previewRect.y - 20, 100, 20),
                m_PendingPlace.Name,
                new GUIStyle(GUI.skin.label) { normal = { textColor = m_PreviewValid ? Color.white : Color.red } });
        }

        /// <summary>
        /// 世界坐标转屏幕坐标（中心坐标系：世界原点=地图中心）
        /// </summary>
        /// <param name="worldPos">世界坐标</param>
        /// <returns>屏幕坐标</returns>
        private Vector2 WorldToScreen(Vector2 worldPos)
        {
            return new Vector2(
                m_MapOffset.x + worldPos.x * m_Zoom,
                m_MapOffset.y + worldPos.y * m_Zoom
            );
        }

    }
}
