using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Editor.MapEditor
{
    /// <summary>
    /// 显示工具
    /// </summary>
    public sealed partial class MapBuildEditor
    {
        #region 鼠标交互处理（核心修复：统一坐标转换）

        // ========== 修复鼠标点击逻辑：增加防抖和精准判断 ==========
        private void HandlePreviewInput(Rect previewRect)
        {
            Event currentEvent = Event.current;
            if (!previewRect.Contains(currentEvent.mousePosition)) return;
            // 【加一行】正在拖地图时，不处理物体逻辑
            if (m_IsDraggingMap) return;
            bool isCtrlHeld = (currentEvent.modifiers & EventModifiers.Control) != 0;


            switch (currentEvent.type)
            {
                case EventType.MouseDown:
                    if (currentEvent.button == 0)
                    {
                        if (isCtrlHeld)
                        {
                            // 点到谁，就强制选中谁，强制设置为拖拽目标
                            GameObject hitObj = GetClickedObject(currentEvent.mousePosition, previewRect);

                            if (hitObj != null)
                            {
                                // 强制清空上一个
                                DeselectCurrentObject();

                                // 强制选中当前点击的
                                m_SelectedObject = hitObj;
                                SelectObject(hitObj);

                                // 强制绑定拖拽目标
                                m_IsDraggingObject = true;

                                // 重新计算偏移
                                Vector2 mouseLocalPos = ScreenToUILocalPoint(currentEvent.mousePosition, previewRect);
                                Vector2 objLocalPos = hitObj.GetComponent<RectTransform>().anchoredPosition;
                                m_DragObjectLocalOffset = mouseLocalPos - objLocalPos;

                                Debug.Log($"✅ 点击选中：{hitObj.name}");
                                currentEvent.Use();
                                Repaint();
                            }
                        }
                        else
                        {
                            DeselectCurrentObject();
                            if (m_PrefabToAdd != null)
                            {
                                Vector2 pos = ScreenToUILocalPoint(currentEvent.mousePosition, previewRect);
                                AddPrefabToMap(m_PrefabToAdd as GameObject, pos);
                                currentEvent.Use();
                            }
                        }
                    }

                    break;

                case EventType.MouseDrag:
                    if (currentEvent.button == 0 && m_IsDraggingObject && m_SelectedObject != null)
                    {
                        Vector2 mouseLocalPos = ScreenToUILocalPoint(currentEvent.mousePosition, previewRect);

                        // 强制只移动 _selectedObject，不会动任何其他物体
                        Vector2 targetPos = mouseLocalPos - m_DragObjectLocalOffset;
                        m_SelectedObject.GetComponent<RectTransform>().anchoredPosition = targetPos;
                        m_ShowAddObjectPosition = targetPos;
                        EditorUtility.SetDirty(m_SelectedObject);
                        currentEvent.Use();
                        Repaint();
                    }

                    break;

                case EventType.MouseUp:
                    if (currentEvent.button == 0)
                    {
                        m_IsDraggingObject = false;
                        currentEvent.Use();
                    }

                    break;

                case EventType.ScrollWheel:
                    m_MapZoom *= 1 - currentEvent.delta.y * 0.1f;
                    m_MapZoom = Mathf.Clamp(m_MapZoom, 1f, 5f);
                    currentEvent.Use();
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        private void ResetObjectHighlight(GameObject obj)
        {
            if (obj == null) return;
            Graphic graphic = obj.GetComponent<Graphic>();
            if (graphic != null)
            {
                graphic.color = Color.white; // 恢复默认颜色
            }
        }

        #endregion

        #region 射线检测

        private GameObject GetClickedObject(Vector2 guiMousePos, Rect previewRect)
        {
            if (m_AddObjects == null || m_AddObjects.Count == 0 || m_RenderCam == null)
                return null;

            // 【核心修复】获取 正确的 UI 本地坐标（唯一正确的方式）
            Vector2 mouseUIPos = ScreenToUILocalPoint(guiMousePos, previewRect);

            GameObject closest = null;
            float minDistance = float.MaxValue;

            foreach (GameObject go in m_AddObjects)
            {
                if (go == null) continue;

                RectTransform rt = go.GetComponent<RectTransform>();
                if (rt == null) continue;

                // 【核心修复】使用 anchoredPosition（UI本地坐标），不是世界坐标！
                Vector2 objUIPos = rt.anchoredPosition;

                // 计算距离（完全统一坐标系，绝对不会错位）
                float dist = Vector2.Distance(mouseUIPos, objUIPos);

                if (dist < minDistance)
                {
                    minDistance = dist;
                    closest = go;
                }
            }

            return closest;
        }

        private Ray ScreenPointToRay(Vector2 guiMousePos, Rect previewRect)
        {
            Vector2 viewportPos = new Vector2(
                (guiMousePos.x - previewRect.x) / previewRect.width,
                1f - (guiMousePos.y - previewRect.y) / previewRect.height
            );
            return m_RenderCam.ViewportPointToRay(viewportPos);
        }

        #endregion

        #region 相机更新

        private void UpdatePreviewCamera(Rect previewRect)
        {
            float currentOrthoSize = m_BaseOrthoSize / m_MapZoom;
            float aspectRatio = previewRect.width / previewRect.height;
            float viewHeight = currentOrthoSize * 2f;
            float viewWidth = viewHeight * aspectRatio;

            float maxOffsetX = Mathf.Max(0, (m_MapBoundsSize.x - viewWidth) / 2f);
            float maxOffsetY = Mathf.Max(0, (m_MapBoundsSize.y - viewHeight) / 2f);
            m_DragOffset.x = Mathf.Clamp(m_DragOffset.x, -maxOffsetX, maxOffsetX);
            m_DragOffset.y = Mathf.Clamp(m_DragOffset.y, -maxOffsetY, maxOffsetY);

            m_RenderCam.transform.position = m_BaseCameraPos + new Vector3(m_DragOffset.x, m_DragOffset.y, 0);
            m_RenderCam.orthographicSize = currentOrthoSize;
        }

        #endregion

        #region 坐标转换（统一使用此方法，确保一致性）

        private Vector2 ScreenToUILocalPoint(Vector2 guiMousePos, Rect previewRect)
        {
            Vector2 viewportPos = new Vector2(
                (guiMousePos.x - previewRect.x) / previewRect.width,
                1f - (guiMousePos.y - previewRect.y) / previewRect.height
            );

            float currentOrthoSize = m_BaseOrthoSize / m_MapZoom;
            float aspectRatio = previewRect.width / previewRect.height;
            float viewHeight = currentOrthoSize * 2f;
            float viewWidth = viewHeight * aspectRatio;

            Vector3 camPos = m_RenderCam.transform.position;
            Vector3 worldPos = new Vector3(
                camPos.x - viewWidth / 2f + viewportPos.x * viewWidth,
                camPos.y - viewHeight / 2f + viewportPos.y * viewHeight,
                0f
            );

            if (m_MapRoot != null)
            {
                return m_MapRoot.InverseTransformPoint(worldPos);
            }

            return worldPos;
        }

        #endregion

        #region 物体选中/取消选中

        /// <summary>
        /// 物体选中
        /// </summary>
        /// <param name="obj"></param>
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

            // ====================== 加这一行 ======================
            m_ShowAddObjectPosition = obj.GetComponent<RectTransform>().anchoredPosition;
            // ======================================================
        }

        /// <summary>
        /// 取消选中
        /// </summary>
        private void DeselectCurrentObject()
        {
            if (m_SelectedObject != null)
            {
                Graphic graphic = m_SelectedObject.GetComponent<Graphic>();
                if (graphic != null)
                {
                    graphic.color = m_OriginalObjectColor;
                }

                m_ShowAddObjectPosition = Vector2.zero;
                m_SelectedObject = null;
                m_IsDraggingObject = false;
            }
        }

        #endregion

        #region 清空添加的物体

        private void ClearAddedObjects()
        {
            DeselectCurrentObject();
            foreach (GameObject obj in m_AddObjects)
            {
                if (obj != null)
                {
                    DestroyImmediate(obj);
                }
            }

            m_AddObjects.Clear();
            Repaint();
        }

        #endregion
        
        #region 自动匹配背景图片 + 自动对齐重合

        /// <summary>
        /// 【你要的按钮调用方法】
        /// 自动匹配所有已添加物体 → 找到背景对应图 → 自动重合
        /// </summary>
        public void AutoAlignAllObjectsToBackground()
        {
            if (m_AddObjects == null || m_AddObjects.Count == 0)
            {
                Debug.LogWarning("没有需要对齐的物体");
                return;
            }

            // 1. 获取背景里所有图片
            var allBackgroundImages = GetAllBackgroundImages();
            if (allBackgroundImages.Count == 0)
            {
                Debug.LogWarning("背景里没有找到任何图片");
                return;
            }

            int successCount = 0;

            // 2. 遍历所有放置的物体
            foreach (var obj in m_AddObjects)
            {
                if (obj == null) continue;

                // 获取物体图片
                Graphic objGraphic = obj.GetComponentInChildren<Graphic>();
                if (objGraphic == null) continue;

                Texture objTex = GetGraphicTexture(objGraphic);
                if (objTex == null) continue;

                // 3. 在背景里找 同名/同纹理 的图片
                RectTransform targetRt = FindMatchBackgroundImage(allBackgroundImages, objTex);
                if (targetRt == null) continue;

                // 4. 自动移动过去重合
                RectTransform objRt = obj.GetComponent<RectTransform>();
                objRt.anchoredPosition = targetRt.anchoredPosition;

                // 5. 记录位置信息
                Debug.Log($"<color=green>自动对齐成功：{obj.name} → 位置：{targetRt.anchoredPosition}</color>");
                successCount++;
            }

            Debug.Log($"<color=yellow>自动对齐完成！成功：{successCount} 个</color>");
            Repaint();
        }

        /// <summary>
        /// 获取背景里所有图片
        /// </summary>
        private List<RectTransform> GetAllBackgroundImages()
        {
            List<RectTransform> images = new List<RectTransform>();
            if (m_MapRoot == null) return images;

            var allGraphics = m_MapRoot.GetComponentsInChildren<Graphic>();
            foreach (var g in allGraphics)
            {
                // 排除自己
                if (g.gameObject.name.Contains("PreviewInstance")) continue;
                if (g.GetComponentInParent<Canvas>() != null && g.transform != m_MapRoot)
                {
                    images.Add(g.GetComponent<RectTransform>());
                }
            }

            return images;
        }

        /// <summary>
        /// 获取图片纹理
        /// </summary>
        private Texture GetGraphicTexture(Graphic graphic)
        {
            if (graphic is Image img) return img.sprite?.texture;
            if (graphic is RawImage raw) return raw.texture;
            return null;
        }

        /// <summary>
        /// 寻找匹配的背景图（按纹理匹配）
        /// </summary>
        private RectTransform FindMatchBackgroundImage(List<RectTransform> images, Texture objTex)
        {
            foreach (var rt in images)
            {
                Graphic g = rt.GetComponent<Graphic>();
                Texture bgTex = GetGraphicTexture(g);
                if (bgTex != null && bgTex.name == objTex.name)
                {
                    return rt;
                }
            }

            return null;
        }
        #endregion
    }
}