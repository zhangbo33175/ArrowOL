using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Editor.MapEditor
{
    public sealed partial class MapBuildEditor
    {
        private static Camera m_RenderCam;
        private static RenderTexture m_RenderTexture;
        private static Vector2 m_DragOffset;

        private static Object m_CurrentMapPrefab;

        // 新增：地图在世界空间中的边界尺寸
        private static Vector2 m_MapBoundsSize;

        /// <summary>
        /// 选中物体拖动相关变量
        /// </summary>
        private static GameObject m_SelectedObject;

        /// <summary>
        /// 地图的Root Transform（用于挂载和坐标转换）
        /// </summary>
        private static Transform m_MapRoot;

        /// <summary>
        /// 
        /// </summary>
        private static Color m_OriginalObjectColor;

        /// <summary>
        /// 
        /// </summary>
        private bool m_IsDraggingObject;

        /// <summary>
        /// 核心修复：使用本地坐标偏移，而不是屏幕坐标偏移
        /// </summary>
        private Vector2 m_DragObjectLocalOffset;

        /// <summary>
        /// 显示物体坐标
        /// </summary>
        private Vector2 m_ShowAddObjectPosition;
        // 地图拖动（相机平移）专用变量
        /// <summary>
        /// 地图拖动中
        /// </summary>
        private bool m_IsDraggingMap;   
        /// <summary>
        /// 上一帧鼠标位置（用来算稳定 delta）
        /// </summary>
        private Vector2 m_LastMousePos; 
        private void SetRightPanel()
        {
            // ===================== 右侧容器 =====================
            GUILayout.BeginVertical(GUILayout.Width(1133));
            // 标签头
            GUILayout.BeginHorizontal(GUILayout.Height(30));
            if (GUILayout.Button("场景编辑", EditorStyles.toolbarButton))
            {
            }

            GUI.backgroundColor = Color.white;
            GUILayout.EndHorizontal();

            // ===================== 预览区域 (map-panel) =====================
            SetPreviewRect();
                
            // 黑色分割线
            GUILayout.Box("", GUILayout.Height(5), GUILayout.ExpandWidth(true));
            GUI.backgroundColor = Color.black;
            GUILayout.Space(2);

            // ===================== 状态栏 =====================
            GUILayout.BeginHorizontal(GUILayout.Height(39));
            GUILayout.Space(15);
            GUILayout.Label("属性", EditorStyles.boldLabel, GUILayout.Width(40));
            GUILayout.Label("缩放：", EditorStyles.boldLabel, GUILayout.Width(50));
            m_MapZoom = EditorGUILayout.FloatField(m_MapZoom, GUILayout.Width(60));
            GUILayout.Label("%", EditorStyles.boldLabel, GUILayout.Width(15));

            GUILayout.Label("mouseX：", EditorStyles.boldLabel, GUILayout.Width(50));
            int mouseX = (int)Event.current.mousePosition.x;
            mouseX = EditorGUILayout.IntField(mouseX, GUILayout.Width(60));

            GUILayout.Label("mouseY：", EditorStyles.boldLabel, GUILayout.Width(50));
            int mouseY = (int)Event.current.mousePosition.y;
            mouseY = EditorGUILayout.IntField(mouseY, GUILayout.Width(60));
            GUILayout.EndHorizontal();

            // 分割线
            GUILayout.Box("", GUILayout.Height(2), GUILayout.ExpandWidth(true));
            GUI.backgroundColor = Color.black;
            GUILayout.Space(2);

            // 底部功能栏
            GUILayout.BeginHorizontal();

            // 按钮组
            GUILayout.BeginVertical(GUILayout.Width(120));
            if (GUILayout.Button("更新地图"))
            {
                
            }

            if (GUILayout.Button("保存地图"))
            {
                SaveTheMap();
            }
            
            if (GUILayout.Button("自动匹配背景图片并对齐"))
            {
                AutoAlignAllObjectsToBackground();
            }
            GUILayout.EndVertical();

            // 分割线
            GUILayout.Box("", GUILayout.Width(1), GUILayout.ExpandHeight(true));

            // 场景名 / ID
            GUILayout.BeginVertical(GUILayout.Width(200));
            GUILayout.BeginHorizontal();
            GUILayout.Label("场景名：", GUILayout.Width(40));
            mapName = EditorGUILayout.TextField(mapName, GUILayout.Width(80));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("场景ID：", GUILayout.Width(40));
            mapId = EditorGUILayout.TextField(mapId, GUILayout.Width(80));
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            // 分割线
            GUILayout.Box("", GUILayout.Width(1), GUILayout.ExpandHeight(true));

            // 宽 / 高
            GUILayout.BeginVertical(GUILayout.Width(200));
            GUILayout.BeginHorizontal();
            GUILayout.Label("宽：", GUILayout.Width(40));
            m_MapWidth = EditorGUILayout.IntField(m_MapWidth, GUILayout.Width(80));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("高：", GUILayout.Width(40));
            m_MapHeight = EditorGUILayout.IntField(m_MapHeight, GUILayout.Width(80));
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

            // 分割线
            GUILayout.Box("", GUILayout.Width(1), GUILayout.ExpandHeight(true));
            GUILayout.FlexibleSpace();

            GUILayout.EndHorizontal();
            // ===================== 状态栏结束 =====================

            GUILayout.EndVertical();
            // ===================== 右侧容器结束 =====================
            //m_IsInitIconList = true;
        }
        /// <summary>
        /// 保存地图
        /// </summary>
        private void SaveTheMap()
        {
            MapAssetData mapAssetData= Utils.CollectMapData();
            string json = JsonUtility.ToJson(mapAssetData, true);
            string path = EditorUtility.SaveFilePanel("保存地图数据", Application.dataPath, "MapData", "json");

            if (!string.IsNullOrEmpty(path))
            {
                File.WriteAllText(path, json);
                Debug.Log("✅ 地图数据保存成功：" + path);
            }
        }
        //加载
        private void LoadMapData(string path)
        {
            if (!File.Exists(path)) return;

            string json = File.ReadAllText(path);
            MapAssetData assetData = JsonUtility.FromJson<MapAssetData>(json);

            ClearAddedObjects();

            m_MapWidth = assetData.m_MapWidth;
            m_MapHeight = assetData.m_MapHeight;

            foreach (var objData in assetData.m_MapObjectData)
            {
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(objData.m_IconPath);
                if (prefab != null)
                {
                    AddPrefabToMap(prefab, objData.position);
                }
            }

            Debug.Log("✅ 地图数据加载完成");
        }
        /// <summary>
        /// /获取带背景的GUIStyle
        /// </summary>
        /// <returns></returns>
        private GUIStyle GetBackgroundStyle()
        {
            GUIStyle style = new GUIStyle(GUI.skin.box);
            // 背景颜色（深灰色，你可以自己改）
            style.normal.background = MakeTex(2, 2, new Color(0.15f, 0.15f, 0.15f));
            style.border = new RectOffset(2, 2, 2, 2);
            return style;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        private void LoadPrefabAndExtractImages(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            string assetPath = FileUtil.GetProjectRelativePath(path);
            m_UiInstance = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
            if (m_UiInstance == null)
            {
                Debug.LogError("无效的Prefab文件");
                return;
            }

            LoadMapPrefab(m_UiInstance, () => { SetPreviewRect(); });
        }
 
        /// <summary>
        /// 
        /// </summary>
        private void SetPreviewRect()
        {
            // 如果没有预制体，显示提示
            if (m_UiInstance == null)
            {
                EditorGUI.DrawRect(GUILayoutUtility.GetRect(0, 0, GUILayout.ExpandWidth(true), GUILayout.Height(650)),
                    new Color(0.933f, 0.737f, 0.737f, 0.5f));
            }
            else
            {
                m_PreviewRect = GUILayoutUtility.GetRect(0, 0, GUILayout.ExpandWidth(true), GUILayout.Height(650));

                if (m_PreviewRect.width > 0 && m_PreviewRect.height > 0)
                {
                    Event e = Event.current;
                    Vector2 mousePos = e.mousePosition;
                    // ====================== 核心修复 ======================
                    if (m_PreviewRect.Contains(mousePos))
                    {
                        if (e.button == 1 && !m_IsDraggingObject)
                        {
                            if (e.type == EventType.MouseDown)
                            {
                                m_IsDraggingMap = true;
                                m_LastMousePos = mousePos;
                                e.Use();
                                Repaint();
                            }
                            else if (e.type == EventType.MouseDrag && m_IsDraggingMap)
                            {
                                Vector2 delta = mousePos - m_LastMousePos;
                                m_LastMousePos = mousePos;
                                float moveScale = (1f / m_MapZoom) * 0.01f;
                                m_DragOffset -= new Vector2(delta.x, -delta.y) * moveScale;
                                e.Use();
                                Repaint();
                            }
                            else if (e.type == EventType.MouseUp)
                            {
                                m_IsDraggingMap = false;
                                e.Use();
                                Repaint();
                            }
                        }
                        if (!m_IsDraggingMap)
                        {
                            HandlePreviewInput(m_PreviewRect);
                        }
                    }
                    // ======================================================
                    int rtWidth = Mathf.Max((int)m_PreviewRect.width, 1);
                    int rtHeight = Mathf.Max((int)m_PreviewRect.height, 1);
                    if (m_RenderTexture == null || m_RenderTexture.width != rtWidth ||
                        m_RenderTexture.height != rtHeight)
                    {
                        if (m_RenderTexture != null) DestroyImmediate(m_RenderTexture);
                        m_RenderTexture = new RenderTexture(rtWidth, rtHeight, 24, RenderTextureFormat.ARGB32);
                        m_RenderTexture.Create();
                    }

                    UpdatePreviewCamera(m_PreviewRect);
                    Canvas.ForceUpdateCanvases();

                    m_RenderCam.targetTexture = m_RenderTexture;
                    m_RenderCam.Render();
                    m_RenderCam.targetTexture = null;

                    GUI.DrawTexture(m_PreviewRect, m_RenderTexture, ScaleMode.StretchToFill, false);
                    Repaint();
                }
                else
                {
                    EditorGUI.DrawRect(m_PreviewRect, Color.clear);
                }
            }
        }

       

        #region 地图预制体加载

        private void LoadMapPrefab(GameObject originalPrefab, Action _LoadMapPrefab)
        {
            ClearAddedObjects();
            // 检查是否是场景/临时对象（不是资源文件）
            if (!EditorUtility.IsPersistent(m_UiInstance))
            {
                DestroyImmediate(m_UiInstance);
                m_UiInstance = null;
            }
            else
            {
                // 如果是资源文件，跳过销毁，避免报错
                Debug.LogWarning($"跳过销毁资源文件：{m_UiInstance.name}，防止数据丢失");
            }

            if (originalPrefab == null) return;

            m_UiInstance = (GameObject)PrefabUtility.InstantiatePrefab(originalPrefab);
            m_UiInstance.name = originalPrefab.name + "_PreviewInstance";
            m_UiInstance.hideFlags = HideFlags.HideAndDontSave;
            m_UiInstance.SetActive(false);

            Canvas canvas = m_UiInstance.GetComponent<Canvas>();
            if (canvas == null)
            {
                DestroyImmediate(m_UiInstance);
                m_UiInstance = null;
                return;
            }
            
            if (m_RenderCam!=null)
            {
                m_RenderCam.orthographic = true;
                m_RenderCam.orthographicSize = 10.24f;
            }
            canvas.enabled = false;
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = m_RenderCam;
            canvas.overrideSorting = true;
            canvas.sortingOrder = 0;

            CanvasScaler scaler = canvas.GetComponent<CanvasScaler>();
            if (scaler != null) scaler.enabled = false;

            RectTransform canvasRect = canvas.GetComponent<RectTransform>();
            
            canvasRect.anchoredPosition = Vector2.zero;
            canvasRect.offsetMin = Vector2.zero;
            canvasRect.offsetMax = Vector2.zero;
         
            canvasRect.anchorMin = new Vector2(0.5f, 0.5f);
            canvasRect.anchorMax = new Vector2(0.5f, 0.5f);
            canvasRect.pivot = new Vector2(0.5f, 0.5f);
            
            canvasRect.sizeDelta = new Vector2(2048, 2048);
            canvasRect.localScale = Vector3.one * 0.005f;
            
            canvasRect.position = Vector3.zero;
            canvasRect.rotation = Quaternion.identity;

            m_MapBoundsSize = new Vector2(10.24f, 10.24f);

            Transform rootTrans = m_UiInstance.transform.Find("Root");
            m_MapRoot = rootTrans != null ? rootTrans : canvasRect;

            RawImage bgImage = m_UiInstance.GetComponentInChildren<RawImage>();
            if (bgImage != null)
            {
                bgImage.material = Canvas.GetDefaultCanvasMaterial();
                bgImage.color = Color.white;
                bgImage.raycastTarget = false;
                bgImage.SetAllDirty();
            }

            m_UiInstance.SetActive(true);
            canvas.enabled = true;
            Canvas.ForceUpdateCanvases();

            m_DragOffset = Vector2.zero;
            m_MapZoom = 1f;

            Debug.Log($"==================== 地图加载完成，根节点：{m_MapRoot.name} ====================");
            Repaint();
            _LoadMapPrefab.Invoke();
        }

        #endregion

        #region 添加预制体到地图

        /// <summary>
        /// 添加预制体到背景界面
        /// </summary>
        /// <param name="prefab">需要添加的预制体</param>
        /// <param name="localPosition">添加的初始位置</param>
        private void AddPrefabToMap(GameObject prefab, Vector2 localPosition)
        {
            if (prefab == null || m_MapRoot == null)
            {
                EditorUtility.DisplayDialog("提示", "请先选择要添加的预制体", "确定");
                return;
            }

            GameObject newObj = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            newObj.name = prefab.name + "_Instance" + m_AddObjects.Count;
            newObj.hideFlags = HideFlags.HideAndDontSave;

            newObj.transform.SetParent(m_MapRoot, false);
            newObj.transform.localScale = Vector3.one;
            newObj.transform.localRotation = Quaternion.identity;
            newObj.transform.localPosition = new Vector3(localPosition.x, localPosition.y, 0);

            RectTransform rectTrans = newObj.GetComponent<RectTransform>();
            if (rectTrans != null)
            {
                rectTrans.anchoredPosition = localPosition;
                rectTrans.localScale = Vector3.one;
                rectTrans.localRotation = Quaternion.identity;
                rectTrans.anchorMin = new Vector2(0.5f, 0.5f);
                rectTrans.anchorMax = new Vector2(0.5f, 0.5f);
                rectTrans.pivot = new Vector2(0.5f, 0.5f);
            }

            Canvas canvas = newObj.GetComponent<Canvas>();
            if (canvas == null)
            {
                canvas = newObj.AddComponent<Canvas>();
            }

            canvas.overrideSorting = true;
            canvas.sortingOrder = 2000 + m_AddObjects.Count; // 关键：每个物体设置唯一的层级，避免渲染重叠
            newObj.AddComponent<GraphicRaycaster>();

            Graphic graphic = newObj.GetComponent<Graphic>();
            if (graphic != null)
            {
                graphic.raycastTarget = true;
                graphic.material = Canvas.GetDefaultCanvasMaterial();
            }

            Graphic[] allGraphics = newObj.GetComponentsInChildren<Graphic>(true);
            foreach (Graphic g in allGraphics)
            {
                g.raycastTarget = true;
            }
            
            m_AddObjects.Add(newObj);
            Canvas.ForceUpdateCanvases();
            // 重置选中状态，避免残留
            DeselectCurrentObject();

            Debug.Log($"✅ 成功添加物体：{newObj.name}，坐标：{newObj.transform.localPosition}");
            Repaint();
        }

        #endregion

        

    }
}