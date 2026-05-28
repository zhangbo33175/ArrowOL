/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  MapBuildEditor.Interaction.cs
 * author:    云毅
 * created:   2026
 * descrip:   地图编辑器 - 鼠标交互/物体选中拖拽/自动对齐核心模块
 ***************************************************************/

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Editor.MapEditor
{
    /// <summary>
    /// 地图编辑器核心功能 partial 类
    /// 负责：鼠标交互、物体选中/拖拽、坐标转换、自动对齐、删除、清空
    /// </summary>
    public sealed partial class MapBuildEditor
    {
        //=========================================================================
        // 鼠标交互处理（核心：统一坐标转换 + 防抖精准判断）
        //=========================================================================
        #region Mouse Interaction Handling
        /// <summary>
        /// 处理预览面板的所有鼠标输入：点击、拖拽、缩放、选中
        /// </summary>
        /// <param name="previewRect">预览面板区域</param>
        private void HandlePreviewInput(Rect previewRect)
        {
            Event currentEvent = Event.current;

            // 鼠标不在预览区 → 不处理
            if (!previewRect.Contains(currentEvent.mousePosition))
                return;

            // 正在拖动地图 → 不处理物体逻辑
            if (m_IsDraggingMap)
                return;

            bool isCtrlHeld = (currentEvent.modifiers & EventModifiers.Control) != 0;

            switch (currentEvent.type)
            {
                // 鼠标左键按下
                case EventType.MouseDown:
                    if (currentEvent.button == 0)
                    {
                        if (isCtrlHeld)
                        {
                            // Ctrl + 左键：选中物体
                            HandleObjectSelection(currentEvent, previewRect);
                        }
                        else
                        {
                            // 直接左键：添加预制体
                            HandlePrefabPlacement(currentEvent, previewRect);
                        }
                    }

                    break;

                // 鼠标拖拽
                case EventType.MouseDrag:
                    if (currentEvent.button == 0 && m_IsDraggingObject && m_SelectedObject != null)
                    {
                        HandleObjectDrag(currentEvent, previewRect);
                    }

                    break;

                // 鼠标抬起
                case EventType.MouseUp:
                    if (currentEvent.button == 0)
                    {
                        m_IsDraggingObject = false;
                        currentEvent.Use();
                    }

                    break;

                // 滚轮缩放
                case EventType.ScrollWheel:
                    m_MapZoom = Mathf.Clamp(m_MapZoom * (1 - currentEvent.delta.y * 0.1f), 1f, 5f);
                    currentEvent.Use();
                    break;
            }
        }

        /// <summary>
        /// 处理物体选中逻辑
        /// </summary>
        private void HandleObjectSelection(Event currentEvent, Rect previewRect)
        {
            GameObject hitObj = GetClickedObject(currentEvent.mousePosition, previewRect);
            if (hitObj != null)
            {
                DeselectCurrentObject();
                m_SelectedObject = hitObj;
                SelectObject(hitObj);
                m_IsDraggingObject = true;

                // 计算拖拽偏移，保证拖拽不飘
                Vector2 mouseLocalPos = ScreenToUILocalPoint(currentEvent.mousePosition, previewRect);
                Vector2 objLocalPos = hitObj.GetComponent<RectTransform>().anchoredPosition;
                m_DragObjectLocalOffset = mouseLocalPos - objLocalPos;

                Debug.Log($"✅ 选中物体：{hitObj.name}");
                currentEvent.Use();
                Repaint();
            }
        }

        /// <summary>
        /// 处理预制体放置逻辑
        /// </summary>
        private void HandlePrefabPlacement(Event currentEvent, Rect previewRect)
        {
            DeselectCurrentObject();
            if (m_PrefabToAdd != null)
            {
                Vector2 pos = ScreenToUILocalPoint(currentEvent.mousePosition, previewRect);
                AddPrefabToMap(m_PrefabToAdd as GameObject, pos, null);
                currentEvent.Use();
            }
        }

        /// <summary>
        /// 处理物体拖拽逻辑
        /// </summary>
        private void HandleObjectDrag(Event currentEvent, Rect previewRect)
        {
            Vector2 mouseLocalPos = ScreenToUILocalPoint(currentEvent.mousePosition, previewRect);
            Vector2 targetPos = mouseLocalPos - m_DragObjectLocalOffset;

            m_SelectedObject.GetComponent<RectTransform>().anchoredPosition = targetPos;
            m_ShowAddObjectPosition = targetPos;

            EditorUtility.SetDirty(m_SelectedObject);
            currentEvent.Use();
            Repaint();
        }

        /// <summary>
        /// 重置物体高亮颜色（恢复白色）
        /// </summary>
        private void ResetObjectHighlight(GameObject obj)
        {
            if (obj == null) return;

            Graphic graphic = obj.GetComponent<Graphic>();
            if (graphic != null)
            {
                graphic.color = Color.white;
            }
        }
        #endregion

        //=========================================================================
        // 点击检测（获取鼠标下物体）
        //=========================================================================
        #region Click Detection
        /// <summary>
        /// 获取鼠标点击的物体（通过距离判断，UI坐标完全统一）
        /// </summary>
        private GameObject GetClickedObject(Vector2 guiMousePos, Rect previewRect)
        {
            if (m_AddObjects == null || m_AddObjects.Count == 0 || m_RenderCam == null)
                return null;

            Vector2 mouseUIPos = ScreenToUILocalPoint(guiMousePos, previewRect);
            GameObject closestObj = null;
            float minDistance = float.MaxValue;

            foreach (GameObject go in m_AddObjects)
            {
                if (go == null) continue;

                RectTransform rt = go.GetComponent<RectTransform>();
                if (rt == null) continue;

                float distance = Vector2.Distance(mouseUIPos, rt.anchoredPosition);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestObj = go;
                }
            }

            return closestObj;
        }

        /// <summary>
        /// 屏幕点转射线（备用射线检测方法）
        /// </summary>
        private Ray ScreenPointToRay(Vector2 guiMousePos, Rect previewRect)
        {
            Vector2 viewportPos = new Vector2(
                (guiMousePos.x - previewRect.x) / previewRect.width,
                1f - (guiMousePos.y - previewRect.y) / previewRect.height
            );
            return m_RenderCam.ViewportPointToRay(viewportPos);
        }
        #endregion

        //=========================================================================
        // 预览相机更新（平移 + 缩放）
        //=========================================================================
        #region Preview Camera Update
        /// <summary>
        /// 更新相机位置与正交大小，实现地图平移、缩放、边界限制
        /// </summary>
        private void UpdatePreviewCamera(Rect previewRect)
        {
            float currentOrthoSize = m_BaseOrthoSize / m_MapZoom;
            float aspectRatio = previewRect.width / previewRect.height;
            float viewHeight = currentOrthoSize * 2f;
            float viewWidth = viewHeight * aspectRatio;

            // 限制平移范围，防止地图移出视野
            float maxOffsetX = Mathf.Max(0, (m_MapBoundsSize.x - viewWidth) / 2f);
            float maxOffsetY = Mathf.Max(0, (m_MapBoundsSize.y - viewHeight) / 2f);

            m_DragOffset.x = Mathf.Clamp(m_DragOffset.x, -maxOffsetX, maxOffsetX);
            m_DragOffset.y = Mathf.Clamp(m_DragOffset.y, -maxOffsetY, maxOffsetY);

            // 应用相机位置与大小
            m_RenderCam.transform.position = m_BaseCameraPos + new Vector3(m_DragOffset.x, m_DragOffset.y, 0);
            m_RenderCam.orthographicSize = currentOrthoSize;
        }
        #endregion

        //=========================================================================
        // 统一坐标转换（核心：保证所有操作坐标不错位）
        //=========================================================================
        #region Coordinate Conversion
        /// <summary>
        /// 编辑器 GUI 鼠标坐标 → 地图 UI 本地坐标
        /// 所有点击、拖拽、添加物体统一使用此方法
        /// </summary>
        private Vector2 ScreenToUILocalPoint(Vector2 guiMousePos, Rect previewRect)
        {
            // 鼠标位置转视口坐标
            Vector2 viewportPos = new Vector2(
                (guiMousePos.x - previewRect.x) / previewRect.width,
                1f - (guiMousePos.y - previewRect.y) / previewRect.height
            );

            // 计算相机视野范围
            float currentOrthoSize = m_BaseOrthoSize / m_MapZoom;
            float aspectRatio = previewRect.width / previewRect.height;
            float viewHeight = currentOrthoSize * 2f;
            float viewWidth = viewHeight * aspectRatio;

            // 计算世界坐标
            Vector3 camPos = m_RenderCam.transform.position;
            Vector3 worldPos = new Vector3(
                camPos.x - viewWidth / 2f + viewportPos.x * viewWidth,
                camPos.y - viewHeight / 2f + viewportPos.y * viewHeight,
                0f
            );

            // 世界坐标 → 地图根节点本地坐标
            return m_MapRoot != null ? m_MapRoot.InverseTransformPoint(worldPos) : worldPos;
        }
        #endregion

        //=========================================================================
        // 物体选中 / 取消选中
        //=========================================================================
        #region Object Selection
        /// <summary>
        /// 选中物体：变色高亮 + 同步坐标显示
        /// </summary>
        private void SelectObject(GameObject obj)
        {
            DeselectCurrentObject();
            m_SelectedObject = obj;

            Graphic graphic = obj.GetComponent<Graphic>();
            if (graphic != null)
            {
                m_OriginalObjectColor = graphic.color;
                graphic.color = new Color(0.2f, 0.6f, 1f, 0.8f);
            }

            m_ShowAddObjectPosition = obj.GetComponent<RectTransform>().anchoredPosition;
        }

        /// <summary>
        /// 取消选中：恢复颜色 + 清空状态
        /// </summary>
        private void DeselectCurrentObject()
        {
            if (m_SelectedObject == null) return;

            Graphic graphic = m_SelectedObject.GetComponent<Graphic>();
            if (graphic != null)
            {
                graphic.color = m_OriginalObjectColor;
            }

            m_ShowAddObjectPosition = Vector2.zero;
            m_SelectedObject = null;
            m_IsDraggingObject = false;
        }
        #endregion

        //=========================================================================
        // 清空所有添加的物体
        //=========================================================================
        #region Clear Objects
        /// <summary>
        /// 清空地图上所有添加的物体，并同步清除数据记录
        /// </summary>
        private void ClearAddedObjects()
        {
            DeselectCurrentObject();

            foreach (GameObject obj in m_AddObjects)
            {
                if (obj != null)
                {
                    m_RMapDatas.Remove(obj);
                    DestroyImmediate(obj);
                }
            }

            m_RMapDatas.Clear();
            m_AddObjects.Clear();
            Repaint();
        }
        #endregion

        //=========================================================================
        // 删除选中物体功能
        //=========================================================================
        #region Delete Selected Object
        /// <summary>
        /// 删除当前选中物体（带确认弹窗 + 数据同步清理）
        /// </summary>
        public void DeleteSelectedItemObject()
        {
            if (m_SelectedObject == null)
            {
                EditorUtility.DisplayDialog("提示", "请先选中一个要删除的物体！", "确定");
                return;
            }

            if (!EditorUtility.DisplayDialog("确认删除", $"确定要删除：{m_SelectedObject.name}？", "删除", "取消"))
                return;

            // 从列表移除
            m_AddObjects?.Remove(m_SelectedObject);
            m_RMapDatas?.Remove(m_SelectedObject);

            // 销毁物体
            DestroyImmediate(m_SelectedObject);
            DeselectCurrentObject();

            Debug.Log($"<color=red>已删除选中物体</color>");
            Repaint();
        }
        #endregion

        //=========================================================================
        // 自动匹配背景图片 + 自动对齐重合（坐标100%精准版）
        //=========================================================================
        #region Auto Align Objects
        private bool _isAligning; // 是否正在自动对齐
        private int _currentObjIndex; // 当前处理的物体索引
        private int _x, _y; // 背景图遍历坐标
        private int _dx, _dy; // 图标遍历子像素
        private float _bestScore; // 最佳匹配分数
        private int _bestX, _bestY; // 最佳匹配位置
        private Texture2D _bgTex; // 背景图纹理
        private Texture2D _iconTex; // 当前物体图标纹理
        private Color[] _bgPixels; // 背景像素数组
        private int _bgW, _bgH; // 背景宽高
        private int _tW, _tH; // 图标宽高
        private float _score; // 当前匹配分数
        private int _count; // 有效像素计数
        private List<GameObject> _pendingObjects; // 待对齐物体列表

        /// <summary>
        /// 启动自动对齐：逐像素对比图标与背景，找到最相似位置
        /// </summary>
        public void AutoAlignAllObjectsToBackground()
        {
            if (_isAligning)
            {
                EditorUtility.DisplayDialog("提示", "正在执行自动对齐，请稍候...", "确定");
                return;
            }

            if (m_AddObjects == null || m_AddObjects.Count == 0)
            {
                Debug.LogWarning("没有需要对齐的物体");
                return;
            }

            // 获取背景图
            RawImage backgroundRaw = m_UiInstance.transform.Find("RawImage")?.GetComponent<RawImage>();
            if (backgroundRaw == null || backgroundRaw.texture == null)
            {
                Debug.LogError("找不到背景图 RawImage 或纹理为空");
                return;
            }

            _bgTex = backgroundRaw.texture as Texture2D;
            if (_bgTex == null || !_bgTex.isReadable)
            {
                Debug.LogError("背景图未开启 Read/Write Enabled，无法读取像素");
                return;
            }

            // 初始化
            _isAligning = true;
            _pendingObjects = new List<GameObject>(m_AddObjects);
            _currentObjIndex = 0;
            _bgPixels = _bgTex.GetPixels();
            _bgW = _bgTex.width;
            _bgH = _bgTex.height;

            EditorApplication.update += OnTick;
        }

        /// <summary>
        /// 逐帧匹配：避免卡顿
        /// </summary>
        private void OnTick()
        {
            if (!_isAligning || _pendingObjects.Count == 0)
            {
                EditorApplication.update -= OnTick;
                EditorUtility.ClearProgressBar();
                _isAligning = false;
                return;
            }

            PrepareCurrentIconTexture();
            ProcessPixelMatching();
            UpdateProgressBar();
        }

        /// <summary>
        /// 准备当前物体的图标纹理
        /// </summary>
        private void PrepareCurrentIconTexture()
        {
            if (_iconTex != null) return;

            GameObject obj = _pendingObjects[_currentObjIndex];
            Graphic graphic = obj.GetComponentInChildren<Graphic>();

            if (graphic == null)
            {
                NextObject();
                return;
            }

            _iconTex = GetTexture2DFromGraphic(graphic);
            if (_iconTex == null || !_iconTex.isReadable)
            {
                Debug.LogWarning($"图标 {obj.name} 未开启 Read/Write Enabled，跳过");
                NextObject();
                return;
            }

            _tW = _iconTex.width;
            _tH = _iconTex.height;
            _bestScore = 0;
            _bestX = _bestY = 0;
            _y = 0;
        }

        /// <summary>
        /// 执行逐像素匹配逻辑
        /// </summary>
        private void ProcessPixelMatching()
        {
            bool continueSearch = true;
            while (continueSearch)
            {
                if (_y > _bgH - _tH)
                {
                    ApplyBestMatchPosition();
                    NextObject();
                    continueSearch = false;
                    break;
                }

                ProcessCurrentPosition();
                continueSearch = false;
            }
        }

        /// <summary>
        /// 处理单个位置的像素匹配
        /// </summary>
        private void ProcessCurrentPosition()
        {
            if (_x == 0)
            {
                _score = 0;
                _count = 0;
                _dx = _dy = 0;
            }

            if (_x > _bgW - _tW)
            {
                _y++;
                _x = 0;
                return;
            }

            if (_dy < _tH)
            {
                CompareSinglePixel();
            }
            else
            {
                CalculatePositionScore();
                _x++;
                _dy = _dx = 0;
            }
        }

        /// <summary>
        /// 对比单个像素
        /// </summary>
        private void CompareSinglePixel()
        {
            if (_dx < _tW)
            {
                Color bgColor = _bgPixels[(_y + _dy) * _bgW + (_x + _dx)];
                Color iconColor = _iconTex.GetPixel(_dx, _dy);

                if (iconColor.a > 0.1f)
                {
                    float diff = Mathf.Abs(bgColor.r - iconColor.r)
                                 + Mathf.Abs(bgColor.g - iconColor.g)
                                 + Mathf.Abs(bgColor.b - iconColor.b);
                    _score += 1f - diff;
                    _count++;
                }

                _dx++;
            }
            else
            {
                _dy++;
                _dx = 0;
            }
        }

        /// <summary>
        /// 计算当前位置匹配分数
        /// </summary>
        private void CalculatePositionScore()
        {
            if (_count <= 0) return;

            float finalScore = _score / _count;
            if (finalScore > _bestScore)
            {
                _bestScore = finalScore;
                _bestX = _x;
                _bestY = _y;
            }
        }

        /// <summary>
        /// 应用最佳匹配位置到物体
        /// </summary>
        private void ApplyBestMatchPosition()
        {
            if (_bestScore <= 0.1f)
            {
                Debug.LogWarning($"未找到匹配：{_iconTex.name}");
                return;
            }

            // 计算中心点
            float centerX = _bestX + _tW / 2f;
            float centerY = _bestY + _tH / 2f;

            // 转UV
            float uvX = centerX / _bgW;
            float uvY = centerY / _bgH;

            // 转UI坐标
            RectTransform bgRt = m_UiInstance.transform.Find("RawImage").GetComponent<RectTransform>();
            float uiX = (uvX - 0.5f) * bgRt.rect.width;
            float uiY = (uvY - 0.5f) * bgRt.rect.height;

            // 设置物体位置
            GameObject obj = _pendingObjects[_currentObjIndex];
            obj.GetComponent<RectTransform>().anchoredPosition = new Vector2(uiX, uiY);
            EditorUtility.SetDirty(obj);
            Debug.Log($"<color=green>对齐成功：{obj.name}</color>");
        }

        /// <summary>
        /// 处理下一个物体
        /// </summary>
        private void NextObject()
        {
            _iconTex = null;
            _x = _y = _dx = _dy = _count = 0;
            _score = 0;
            _currentObjIndex++;

            if (_currentObjIndex >= _pendingObjects.Count)
            {
                EditorApplication.update -= OnTick;
                EditorUtility.ClearProgressBar();
                _isAligning = false;
                Debug.Log($"<color=yellow>自动对齐完成！共处理：{_pendingObjects.Count} 个</color>");
            }
        }

        /// <summary>
        /// 更新进度条
        /// </summary>
        private void UpdateProgressBar()
        {
            float progress = Mathf.Clamp01((float)(_currentObjIndex + (float)_y / _bgH) / _pendingObjects.Count);
            EditorUtility.DisplayProgressBar("自动对齐中", $"正在匹配：{_iconTex?.name ?? ""}", progress);
        }

        /// <summary>
        /// 从 Image / RawImage 中获取 Texture2D
        /// </summary>
        private Texture2D GetTexture2DFromGraphic(Graphic graphic)
        {
            if (graphic is Image img && img.sprite != null)
                return img.sprite.texture;

            if (graphic is RawImage raw && raw.texture != null)
                return raw.texture as Texture2D;

            return null;
        }
        #endregion
    }
}