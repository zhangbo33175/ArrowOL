/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  MapPreviewWindow.cs
 * author:    云毅
 * created:   2026
 * descrip:   地图预览窗口 - 模拟运行时效果
 ***************************************************************/

using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace Honor.Editor.MapTool
{
    /// <summary>
    /// 地图预览窗口 - 弹出式预览当前编辑的地图
    /// </summary>
    public class MapPreviewWindow : EditorWindow
    {
        private List<PlacedObject> m_Objects;
        private List<ObjectTemplate> m_Templates;
        private Texture m_Background;
        private string m_MapName = "";
        private int m_MapWidth = 4096;
        private int m_MapHeight = 4096;
        private bool m_IsPlaying = false;
        private float m_PlayTime = 0f;
        private double m_LastTime = 0f;
        private float m_Zoom = 0.25f;
        private bool m_ShowPathPoints = true;
        private Vector2 m_PanOffset = Vector2.zero;
        private bool m_IsDragging = false;
        private Vector2 m_DragStartPos = Vector2.zero;
        private Vector2 m_DragStartOffset = Vector2.zero;
        private float m_EffectiveZoom = 0.25f; // 当前实际生效的缩放比例

        // 物件播放状态（每个物件当前在路径上的位置）
        private Dictionary<PlacedObject, int> m_CurrentPathIndex = new Dictionary<PlacedObject, int>();
        private Dictionary<PlacedObject, float> m_PathProgress = new Dictionary<PlacedObject, float>();
        private const float MoveSpeed = 200f; // 移动速度（像素/秒）

        private GUIStyle m_ToolbarStyle;
        private GUIStyle m_ButtonStyle;
        private GUIStyle m_LabelStyle;
        private GUIStyle m_PathLabelStyle;

        /// <summary>
        /// 打开预览窗口
        /// </summary>
        public static void ShowWindow(List<PlacedObject> objects, List<ObjectTemplate> templates, Texture background, string mapName, int mapWidth = 4096, int mapHeight = 4096)
        {
            var window = GetWindow<MapPreviewWindow>("地图预览");
            window.minSize = new Vector2(900, 650);
            window.m_Objects = objects;
            window.m_Templates = templates;
            window.m_Background = background;
            window.m_MapName = mapName;
            window.m_MapWidth = mapWidth;
            window.m_MapHeight = mapHeight;
            window.m_IsPlaying = false;
            window.m_PlayTime = 0f;
            window.m_CurrentPathIndex.Clear();
            window.m_PathProgress.Clear();

            // 自动从模板同步所有物件的配置
            window.SyncObjectsFromTemplates();

            window.InitStyles();
            window.Show();
        }

        /// <summary>
        /// 从模板同步所有物件的配置
        /// </summary>
        private void SyncObjectsFromTemplates()
        {
            if (m_Objects == null || m_Templates == null) return;

            // 建立模板ID到模板的映射
            Dictionary<string, ObjectTemplate> templateMap = new Dictionary<string, ObjectTemplate>();
            foreach (var tpl in m_Templates)
            {
                if (!string.IsNullOrEmpty(tpl.TemplateId))
                {
                    templateMap[tpl.TemplateId] = tpl;
                }
            }

            // 同步每个物件
            foreach (var obj in m_Objects)
            {
                if (!string.IsNullOrEmpty(obj.TemplateId) && templateMap.ContainsKey(obj.TemplateId))
                {
                    obj.SyncFromTemplate(templateMap[obj.TemplateId]);
                }
            }
        }

        private void InitStyles()
        {
            m_ToolbarStyle = new GUIStyle(EditorStyles.toolbar)
            {
                normal = { background = MakeColorTexture(new Color(0.15f, 0.15f, 0.15f, 1f)) },
                fixedHeight = 32
            };

            m_ButtonStyle = new GUIStyle(EditorStyles.miniButton)
            {
                normal = { background = MakeColorTexture(new Color(0.25f, 0.25f, 0.25f, 1f)), textColor = Color.white },
                hover = { background = MakeColorTexture(new Color(0.35f, 0.35f, 0.35f, 1f)), textColor = Color.white },
                fixedHeight = 24,
                fontSize = 12
            };

            m_LabelStyle = new GUIStyle(GUI.skin.label)
            {
                normal = { textColor = new Color(0.9f, 0.9f, 0.9f) },
                fontSize = 12
            };

            m_PathLabelStyle = new GUIStyle(GUI.skin.label)
            {
                normal = { textColor = new Color(1f, 0.8f, 0.3f) },
                fontSize = 9,
                alignment = TextAnchor.MiddleCenter
            };
        }

        private Texture2D MakeColorTexture(Color color)
        {
            var tex = new Texture2D(1, 1);
            tex.SetPixel(0, 0, color);
            tex.Apply();
            return tex;
        }

        private void OnEnable()
        {
            EditorApplication.update += OnEditorUpdate;
        }

        private void OnDisable()
        {
            EditorApplication.update -= OnEditorUpdate;
        }

        private void OnEditorUpdate()
        {
            if (m_IsPlaying)
            {
                double now = EditorApplication.timeSinceStartup;
                float deltaTime = 0f;
                if (m_LastTime > 0)
                {
                    deltaTime = (float)(now - m_LastTime);
                    m_PlayTime += deltaTime;
                }
                m_LastTime = now;

                // 更新所有物件的播放状态
                if (m_Objects != null && deltaTime > 0)
                {
                    foreach (var obj in m_Objects)
                    {
                        UpdateObjectPlayState(obj, deltaTime);
                    }
                }

                Repaint();
            }
        }

        private void OnGUI()
        {
            if (m_ToolbarStyle == null) InitStyles();

            // 顶部工具栏
            DrawToolbar();

            // 预览区域
            DrawPreviewArea();

            // 底部状态栏
            DrawStatusBar();
        }

        /// <summary>
        /// 绘制顶部工具栏
        /// </summary>
        private void DrawToolbar()
        {
            EditorGUILayout.BeginHorizontal(m_ToolbarStyle);

            GUILayout.Space(8);

            // 播放/暂停按钮
            string playText = m_IsPlaying ? "⏸ 暂停" : "▶ 播放";
            if (GUILayout.Button(playText, m_ButtonStyle, GUILayout.Width(80)))
            {
                m_IsPlaying = !m_IsPlaying;
                if (m_IsPlaying)
                {
                    // 开始播放时记录当前时间
                    m_LastTime = EditorApplication.timeSinceStartup;
                }
            }

            GUILayout.Space(8);

            // 重置按钮
            if (GUILayout.Button("↺ 重置", m_ButtonStyle, GUILayout.Width(60)))
            {
                m_PlayTime = 0f;
                m_PanOffset = Vector2.zero;
                m_Zoom = 0.25f;
                m_CurrentPathIndex.Clear();
                m_PathProgress.Clear();
            }

            GUILayout.Space(8);

            // 路径点开关
            m_ShowPathPoints = GUILayout.Toggle(m_ShowPathPoints, "显示路径", m_ButtonStyle, GUILayout.Width(80));

            GUILayout.FlexibleSpace();

            // 缩放控制
            GUILayout.Label("缩放:", m_LabelStyle);
            m_Zoom = GUILayout.HorizontalSlider(m_Zoom, 0.05f, 1f, GUILayout.Width(120));
            GUILayout.Label($"{(m_Zoom * 100):F0}%", m_LabelStyle, GUILayout.Width(40));

            GUILayout.Space(8);

            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// 绘制预览区域
        /// </summary>
        private void DrawPreviewArea()
        {
            // 顶部工具栏高 36，底部状态栏高 28
            Rect areaRect = new Rect(0, 36, position.width, position.height - 64);

            // 处理鼠标拖动
            HandleMouseDrag(areaRect);

            // 绘制裁剪：只在预览区域内绘制
            GUI.BeginClip(areaRect);

            // 计算自适应缩放：地图太小就自动放大到填满预览区域
            float fitZoomX = areaRect.width / m_MapWidth;
            float fitZoomY = areaRect.height / m_MapHeight;
            float fitZoom = Mathf.Min(fitZoomX, fitZoomY) * 0.95f; // 留5%边距

            // 有效缩放 = 用户缩放和自适应缩放的较大值
            float effectiveZoom = Mathf.Max(m_Zoom, fitZoom);
            m_EffectiveZoom = effectiveZoom;

            // 计算地图实际显示大小
            float mapWidth = m_MapWidth * effectiveZoom;
            float mapHeight = m_MapHeight * effectiveZoom;
            float offsetX = (areaRect.width - mapWidth) * 0.5f + m_PanOffset.x;
            float offsetY = (areaRect.height - mapHeight) * 0.5f + m_PanOffset.y;
            Rect mapRect = new Rect(offsetX, offsetY, mapWidth, mapHeight);

            // 绘制背景
            if (m_Background != null)
            {
                GUI.DrawTexture(mapRect, m_Background, ScaleMode.StretchToFill);
            }
            else
            {
                EditorGUI.DrawRect(mapRect, new Color(0.2f, 0.2f, 0.2f));
                var style = new GUIStyle(GUI.skin.label);
                style.normal.textColor = Color.gray;
                GUI.Label(new Rect(mapRect.x + 10, mapRect.y + 10, 200, 20), "（暂无背景图）", style);
            }

            // 绘制网格（淡淡的）
            DrawGrid(mapRect);

            // 绘制物件（按YSort排序）
            if (m_Objects != null)
            {
                var sortedObjects = GetSortedObjects();

                // 先绘制路径点（在物件下面）
                if (m_ShowPathPoints)
                {
                    foreach (var obj in sortedObjects)
                    {
                        DrawPathPoints(obj, mapRect);
                    }
                }

                // 再绘制物件
                foreach (var obj in sortedObjects)
                {
                    DrawPreviewObject(obj, mapRect);
                }
            }

            GUI.EndClip();
        }

        /// <summary>
        /// 处理鼠标拖动
        /// </summary>
        private void HandleMouseDrag(Rect areaRect)
        {
            Event evt = Event.current;

            // 鼠标滚轮缩放
            if (evt.type == EventType.ScrollWheel && areaRect.Contains(evt.mousePosition))
            {
                float scroll = evt.delta.y * 0.1f;
                float newZoom = Mathf.Clamp(m_Zoom + scroll, 0.05f, 2f);

                // 以鼠标位置为中心缩放
                Vector2 mousePos = evt.mousePosition;
                float mapWidth = m_MapWidth * m_Zoom;
                float mapHeight = m_MapHeight * m_Zoom;
                float centerX = (areaRect.width - mapWidth) * 0.5f + m_PanOffset.x;
                float centerY = (areaRect.height - mapHeight) * 0.5f + m_PanOffset.y;

                // 计算鼠标在地图上的相对位置
                float relX = (mousePos.x - centerX) / mapWidth;
                float relY = (mousePos.y - centerY) / mapHeight;

                // 应用新缩放
                m_Zoom = newZoom;

                // 调整平移，保持鼠标位置不变
                float newMapWidth = m_MapWidth * m_Zoom;
                float newMapHeight = m_MapHeight * m_Zoom;
                m_PanOffset.x = mousePos.x - (areaRect.width - newMapWidth) * 0.5f - relX * newMapWidth;
                m_PanOffset.y = mousePos.y - (areaRect.height - newMapHeight) * 0.5f - relY * newMapHeight;

                evt.Use();
                Repaint();
            }

            // 鼠标按下：开始拖动
            if (evt.type == EventType.MouseDown && areaRect.Contains(evt.mousePosition))
            {
                if (evt.button == 0 || evt.button == 2) // 左键或中键拖动
                {
                    m_IsDragging = true;
                    m_DragStartPos = evt.mousePosition;
                    m_DragStartOffset = m_PanOffset;
                    evt.Use();
                }
            }

            // 鼠标拖动中
            if (evt.type == EventType.MouseDrag && m_IsDragging)
            {
                Vector2 delta = evt.mousePosition - m_DragStartPos;
                m_PanOffset = m_DragStartOffset + delta;
                evt.Use();
                Repaint();
            }

            // 鼠标释放：结束拖动
            if (evt.type == EventType.MouseUp && m_IsDragging)
            {
                m_IsDragging = false;
                evt.Use();
            }

            // 鼠标样式
            if (m_IsDragging)
            {
                EditorGUIUtility.AddCursorRect(areaRect, MouseCursor.MoveArrow);
            }
            else if (areaRect.Contains(evt.mousePosition))
            {
                EditorGUIUtility.AddCursorRect(areaRect, MouseCursor.Pan);
            }
        }

        /// <summary>
        /// 绘制淡淡的网格
        /// </summary>
        private void DrawGrid(Rect mapRect)
        {
            Handles.color = new Color(1, 1, 1, 0.05f);
            float gridSize = 32 * m_Zoom;
            for (float x = mapRect.x; x < mapRect.xMax; x += gridSize)
            {
                Handles.DrawLine(new Vector3(x, mapRect.y), new Vector3(x, mapRect.yMax));
            }
            for (float y = mapRect.y; y < mapRect.yMax; y += gridSize)
            {
                Handles.DrawLine(new Vector3(mapRect.x, y), new Vector3(mapRect.xMax, y));
            }
        }

        /// <summary>
        /// 获取排序后的物件列表（YSort）
        /// </summary>
        private List<PlacedObject> GetSortedObjects()
        {
            var list = new List<PlacedObject>(m_Objects);
            list.Sort((a, b) =>
            {
                float orderA = GetEffectiveSortOrder(a);
                float orderB = GetEffectiveSortOrder(b);
                return orderA.CompareTo(orderB);
            });
            return list;
        }

        /// <summary>
        /// 计算有效排序值
        /// </summary>
        private float GetEffectiveSortOrder(PlacedObject obj)
        {
            if (obj.YSortEnabled)
            {
                return obj.SortOrder + obj.Position.y * 0.01f + obj.YSortOffset;
            }
            return obj.SortOrder;
        }

        /// <summary>
        /// 绘制路径点
        /// </summary>
        private void DrawPathPoints(PlacedObject obj, Rect mapRect)
        {
            if (obj.PathPoints == null || obj.PathPoints.Count < 2) return;

            // 绘制路径连线
            Handles.color = new Color(1f, 0.6f, 0.2f, 0.6f);
            for (int i = 0; i < obj.PathPoints.Count - 1; i++)
            {
                Vector2 p1 = WorldToMapRect(obj.PathPoints[i], mapRect);
                Vector2 p2 = WorldToMapRect(obj.PathPoints[i + 1], mapRect);
                Handles.DrawLine(p1, p2);
            }

            // 绘制路径点
            for (int i = 0; i < obj.PathPoints.Count; i++)
            {
                Vector2 pt = WorldToMapRect(obj.PathPoints[i], mapRect);
                float radius = 4f;

                // 绘制圆点
                Handles.color = new Color(1f, 0.6f, 0.2f, 0.9f);
                Handles.DrawSolidDisc(pt, Vector3.forward, radius);

                // 绘制序号
                GUI.Label(new Rect(pt.x - 10, pt.y - 10, 20, 20), (i + 1).ToString(), m_PathLabelStyle);
            }
        }

        /// <summary>
        /// 世界坐标转地图区域坐标（在裁剪区域内）
        /// </summary>
        private Vector2 WorldToMapRect(Vector2 worldPos, Rect mapRect)
        {
            return new Vector2(
                mapRect.x + worldPos.x * m_EffectiveZoom,
                mapRect.y + worldPos.y * m_EffectiveZoom);
        }

        /// <summary>
        /// 获取物件当前播放位置
        /// </summary>
        private Vector2 GetObjectPlayPosition(PlacedObject obj)
        {
            // 如果没有路径或不在播放，返回原始位置
            if (!m_IsPlaying || obj.PathPoints == null || obj.PathPoints.Count < 2)
            {
                return obj.Position;
            }

            // 初始化播放状态
            if (!m_CurrentPathIndex.ContainsKey(obj))
            {
                m_CurrentPathIndex[obj] = 0;
                m_PathProgress[obj] = 0f;
            }

            int currentIndex = m_CurrentPathIndex[obj];
            float progress = m_PathProgress[obj];

            // 计算当前段的起点和终点
            Vector2 start = obj.PathPoints[currentIndex];
            Vector2 end = obj.PathPoints[(currentIndex + 1) % obj.PathPoints.Count];

            // 计算当前位置
            return Vector2.Lerp(start, end, progress);
        }

        /// <summary>
        /// 更新物件播放状态
        /// </summary>
        private void UpdateObjectPlayState(PlacedObject obj, float deltaTime)
        {
            if (!m_IsPlaying || obj.PathPoints == null || obj.PathPoints.Count < 2)
            {
                return;
            }

            if (!m_CurrentPathIndex.ContainsKey(obj))
            {
                m_CurrentPathIndex[obj] = 0;
                m_PathProgress[obj] = 0f;
            }

            int currentIndex = m_CurrentPathIndex[obj];
            float progress = m_PathProgress[obj];

            // 计算本帧要移动的距离
            float moveDistance = MoveSpeed * deltaTime;

            // 循环沿着路径移动
            while (moveDistance > 0)
            {
                int nextIndex = (currentIndex + 1) % obj.PathPoints.Count;
                Vector2 start = obj.PathPoints[currentIndex];
                Vector2 end = obj.PathPoints[nextIndex];
                float segmentLength = Vector2.Distance(start, end);

                if (segmentLength < 0.01f)
                {
                    // 两点距离为0，直接跳到下一个
                    currentIndex = nextIndex;
                    continue;
                }

                // 当前段剩余距离
                float remaining = segmentLength * (1f - progress);

                if (moveDistance < remaining)
                {
                    // 还在当前段内
                    progress += moveDistance / segmentLength;
                    moveDistance = 0;
                }
                else
                {
                    // 走完当前段，进入下一段
                    moveDistance -= remaining;
                    currentIndex = nextIndex;
                    progress = 0f;
                }
            }

            m_CurrentPathIndex[obj] = currentIndex;
            m_PathProgress[obj] = progress;
        }

        /// <summary>
        /// 绘制单个预览物件
        /// </summary>
        private void DrawPreviewObject(PlacedObject obj, Rect mapRect)
        {
            // 获取当前播放位置
            Vector2 pos = GetObjectPlayPosition(obj);

            float width = obj.Width * 32 * m_EffectiveZoom;
            float height = obj.Height * 32 * m_EffectiveZoom;
            width = Mathf.Max(width, 24f);
            height = Mathf.Max(height, 24f);

            Vector2 drawPos = WorldToMapRect(pos, mapRect);
            Rect objRect = new Rect(drawPos.x, drawPos.y, width, height);

            // 绘制物件
            if (obj.PreviewTexture != null)
            {
                GUI.DrawTexture(objRect, obj.PreviewTexture, ScaleMode.StretchToFill);
            }
            else
            {
                // 按类型绘制不同颜色的色块
                Color color = GetObjectColor(obj.Type);
                EditorGUI.DrawRect(objRect, color);
            }

            // 绘制边框（半透明的）
            Color borderColor = obj.Type == MapObjectType.Character ? Color.yellow :
                                obj.Type == MapObjectType.Building ? Color.green : Color.cyan;
            Handles.color = borderColor;
            float borderWidth = 2f;
            Handles.DrawLine(new Vector3(objRect.x, objRect.y), new Vector3(objRect.xMax, objRect.y));
            Handles.DrawLine(new Vector3(objRect.x, objRect.yMax), new Vector3(objRect.xMax, objRect.yMax));
            Handles.DrawLine(new Vector3(objRect.x, objRect.y), new Vector3(objRect.x, objRect.yMax));
            Handles.DrawLine(new Vector3(objRect.xMax, objRect.y), new Vector3(objRect.xMax, objRect.yMax));

            // 绘制名称标签（带背景）
            string label = obj.Name;
            Vector2 labelSize = m_LabelStyle.CalcSize(new GUIContent(label));
            Rect labelRect = new Rect(objRect.x, objRect.y - labelSize.y - 2, labelSize.x + 4, labelSize.y);

            // 标签背景
            EditorGUI.DrawRect(labelRect, new Color(0, 0, 0, 0.7f));
            GUI.Label(labelRect, label, m_LabelStyle);
        }

        /// <summary>
        /// 获取物件类型颜色
        /// </summary>
        private Color GetObjectColor(MapObjectType type)
        {
            switch (type)
            {
                case MapObjectType.Building: return new Color(0.3f, 0.5f, 0.3f, 0.6f);
                case MapObjectType.Character: return new Color(0.6f, 0.4f, 0.2f, 0.6f);
                case MapObjectType.Prop: return new Color(0.3f, 0.4f, 0.6f, 0.6f);
                default: return new Color(0.5f, 0.5f, 0.5f, 0.6f);
            }
        }

        /// <summary>
        /// 绘制底部状态栏
        /// </summary>
        private void DrawStatusBar()
        {
            EditorGUILayout.BeginHorizontal(m_ToolbarStyle);

            GUILayout.Space(8);
            GUILayout.Label($"📁 {m_MapName}", m_LabelStyle);
            GUILayout.FlexibleSpace();

            // 物件统计
            int buildingCount = 0, characterCount = 0, propCount = 0;
            if (m_Objects != null)
            {
                foreach (var obj in m_Objects)
                {
                    if (obj.Type == MapObjectType.Building) buildingCount++;
                    else if (obj.Type == MapObjectType.Character) characterCount++;
                    else propCount++;
                }
            }
            GUILayout.Label($"🏠 {buildingCount}  🧑 {characterCount}  📦 {propCount}", m_LabelStyle);
            GUILayout.FlexibleSpace();

            GUILayout.Label($"状态: {(m_IsPlaying ? "▶ 播放中" : "⏸ 已暂停")}", m_LabelStyle);
            GUILayout.Space(16);
            GUILayout.Label($"⏱ {m_PlayTime:F1}s", m_LabelStyle);

            GUILayout.Space(8);
            EditorGUILayout.EndHorizontal();
        }
    }
}
