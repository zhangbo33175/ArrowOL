/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  MapBuildEditor.Core.cs
 * author:    云毅
 * created:   2026
 * descrip:   地图编辑器 - 界面/预览/加载/保存核心模块
 ***************************************************************/

using System;
using System.IO;
using Editor.Utility;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using Object = UnityEngine.Object;

namespace Editor.MapEditor
{
    /// <summary>
    /// 地图编辑器 - 界面/预览/加载/保存核心模块
    /// 负责：右侧面板绘制、地图预览渲染、Prefab加载、物体添加、数据保存/加载
    /// </summary>
    public sealed partial class MapBuildEditor
    {
        //=========================================================================
        // 核心成员变量
        //=========================================================================
        #region Core Fields
        /// <summary>
        /// 预览渲染相机
        /// </summary>
        private static Camera m_RenderCam;

        /// <summary>
        /// 预览渲染纹理
        /// </summary>
        private static RenderTexture m_RenderTexture;

        /// <summary>
        /// 地图拖动偏移（相机平移）
        /// </summary>
        private static Vector2 m_DragOffset;

        /// <summary>
        /// 当前加载的地图预制体
        /// </summary>
        private static Object m_CurrentMapPrefab;

        /// <summary>
        /// 地图在世界空间中的边界尺寸
        /// </summary>
        private static Vector2 m_MapBoundsSize;

        /// <summary>
        /// 当前选中的物体
        /// </summary>
        private static GameObject m_SelectedObject;

        /// <summary>
        /// 地图根节点（用于挂载物体 + 坐标转换）
        /// </summary>
        private static Transform m_MapRoot;

        /// <summary>
        /// 物体原始颜色（取消选中时恢复）
        /// </summary>
        private static Color m_OriginalObjectColor;

        /// <summary>
        /// 是否正在拖拽物体
        /// </summary>
        private bool m_IsDraggingObject;

        /// <summary>
        /// 物体拖拽偏移（UI本地坐标，保证不漂移）
        /// </summary>
        private Vector2 m_DragObjectLocalOffset;

        /// <summary>
        /// 实时显示添加物体的坐标
        /// </summary>
        private Vector2 m_ShowAddObjectPosition;

        /// <summary>
        /// 是否正在拖动地图（相机平移）
        /// </summary>
        private bool m_IsDraggingMap;

        /// <summary>
        /// 上一帧鼠标位置（计算平移增量）
        /// </summary>
        private Vector2 m_LastMousePos;
        #endregion

        //=========================================================================
        // 右侧面板UI绘制
        //=========================================================================
        #region Right Panel UI
        /// <summary>
        /// 绘制右侧面板：预览窗口 + 状态栏 + 功能按钮
        /// </summary>
        private void SetRightPanel()
        {
            //=========================================================================
            // 右侧总容器
            //=========================================================================
            GUILayout.BeginVertical(GUILayout.Width(1133));

            // 工具栏标签
            GUILayout.BeginHorizontal(GUILayout.Height(30));
            if (GUILayout.Button("场景编辑", EditorStyles.toolbarButton))
            {
            }

            GUI.backgroundColor = Color.white;
            GUILayout.EndHorizontal();

            //=========================================================================
            // 地图预览区域
            //=========================================================================
            SetPreviewRect();

            // 黑色分割线
            GUI.backgroundColor = Color.black;
            GUILayout.Box("", GUILayout.Height(5), GUILayout.ExpandWidth(true));
            GUILayout.Space(2);

            //=========================================================================
            // 状态栏（缩放、鼠标坐标显示）
            //=========================================================================
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
            GUI.backgroundColor = Color.black;
            GUILayout.Box("", GUILayout.Height(2), GUILayout.ExpandWidth(true));
            GUILayout.Space(2);

            //=========================================================================
            // 底部功能按钮栏
            //=========================================================================
            GUILayout.BeginHorizontal();

            // 左侧按钮组
            GUILayout.BeginVertical(GUILayout.Width(120));
            if (GUILayout.Button("更新地图"))
            {
            }

            if (GUILayout.Button("保存地图"))
            {
                SaveData();
            }

            if (GUILayout.Button("自动匹配背景图片并对齐"))
            {
                AutoAlignAllObjectsToBackground();
            }

            if (GUILayout.Button("删除选中物体", GUILayout.Height(30)))
            {
                DeleteSelectedItemObject();
            }

            GUILayout.EndVertical();

            // 分割线
            GUILayout.Box("", GUILayout.Width(1), GUILayout.ExpandHeight(true));

            // 场景名 / 场景ID
            GUILayout.BeginVertical(GUILayout.Width(200));
            GUILayout.BeginHorizontal();
            GUILayout.Label("场景名：", GUILayout.Width(40));
            mapName = EditorGUILayout.TextField(mapName, GUILayout.Width(80));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("场景ID：", GUILayout.Width(40));
            mapId = EditorGUILayout.TextField(mapId, GUILayout.Width(80));
            GUILayout.EndHorizontal();
            GUILayout.Space(5);
            if (GUILayout.Button("更换地图背景"))
            {
                LoadReplaceBackgroundToImage();
            }

            GUILayout.EndVertical();

            // 分割线
            GUILayout.Box("", GUILayout.Width(1), GUILayout.ExpandHeight(true));

            // 地图宽 / 高
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
            //=========================================================================
            // 状态栏结束
            //=========================================================================

            GUILayout.EndVertical();
            //=========================================================================
            // 右侧容器结束
            //=========================================================================
        }
        #endregion

        //=========================================================================
        // 数据保存与加载
        //=========================================================================
        #region Data Save & Load
        /// <summary>
        /// 保存地图数据（JSON + Lua双格式）
        /// </summary>
        private void SaveData()
        {
            // 获取当前关卡配置
            RMapChapterTypeData data = Utils.GetCurLevelTypeEditorData();
            if (data == null)
            {
                EditorUtility.DisplayDialog("错误", "未获取到关卡数据！", "确定");
                return;
            }

            // 保存JSON
            string jsonPath = Path.Combine(SaveConfig.LevelSavePath, $"{data.ChapterId}_{data.LevelId}.json");
            MapLevelDataUtil.SaveDataJson(data, jsonPath);
            ShowNotification(new GUIContent($"保存关卡 {data.ChapterId}_{data.LevelId} 文件：{jsonPath}"));

            // 创建保存目录
            if (!Directory.Exists(levelLuaSavePath))
                Directory.CreateDirectory(levelLuaSavePath);

            string chapterDir = Path.Combine(levelLuaSavePath, data.ChapterId);
            if (!Directory.Exists(chapterDir))
                Directory.CreateDirectory(chapterDir);

            // 自定义保存目录
            if (!string.IsNullOrEmpty(data.SavePath))
            {
                string customDir = Path.Combine(chapterDir, data.SavePath);
                Directory.CreateDirectory(customDir);
            }

            // 保存Lua配置
            SaveConfig.MapSaveLua(data);
        }

        /// <summary>
        /// 从JSON文件加载地图数据
        /// </summary>
        /// <param name="path">文件路径</param>
        private void LoadMapData(string path)
        {
            if (!File.Exists(path)) return;

            string json = File.ReadAllText(path);
            RMapChapterTypeData chapterTypeData = JsonUtility.FromJson<RMapChapterTypeData>(json);

            // 清空旧物体
            ClearAddedObjects();

            m_MapWidth = chapterTypeData.m_MapWidth;
            m_MapHeight = chapterTypeData.m_MapHeight;

            // 遍历加载所有物体
            foreach (var objData in chapterTypeData.m_MapObjectData)
            {
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(objData.m_Sprite);
                if (prefab != null)
                {
                    AddPrefabToMap(prefab, objData.m_Position, null);
                }
            }

            Debug.Log("✅ 地图数据加载完成");
        }

        /// <summary>
        /// 加载地图Prefab并提取图片资源
        /// </summary>
        /// <param name="path">Prefab路径</param>
        private void LoadPrefabAndExtractImages(string path)
        {
            if (string.IsNullOrEmpty(path)) return;

            string assetPath = FileUtil.GetProjectRelativePath(path);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);

            if (prefab == null)
            {
                Debug.LogError("无效的Prefab文件");
                return;
            }

            // 加载预制体并刷新预览
            LoadMapPrefab(prefab, SetPreviewRect);
        }
        #endregion

        //=========================================================================
        // 预览渲染与相机控制
        //=========================================================================
        #region Preview Render & Camera
        /// <summary>
        /// 绘制地图预览区域 + 处理鼠标交互（拖动、缩放、点击）
        /// </summary>
        private void SetPreviewRect()
        {
            // 无预制体时显示红色提示框
            if (m_UiInstance == null)
            {
                Rect emptyRect = GUILayoutUtility.GetRect(0, 0, GUILayout.ExpandWidth(true), GUILayout.Height(650));
                EditorGUI.DrawRect(emptyRect, new Color(0.933f, 0.737f, 0.737f, 0.5f));
                return;
            }

            m_PreviewRect = GUILayoutUtility.GetRect(0, 0, GUILayout.ExpandWidth(true), GUILayout.Height(650));
            if (m_PreviewRect.width <= 0 || m_PreviewRect.height <= 0)
            {
                EditorGUI.DrawRect(m_PreviewRect, Color.clear);
                return;
            }

            HandlePreviewCameraDrag(m_PreviewRect);
            HandlePreviewInput(m_PreviewRect);
            UpdatePreviewRenderTexture();
            DrawPreviewToEditor();
        }

        /// <summary>
        /// 处理右键拖动地图（相机平移）
        /// </summary>
        private void HandlePreviewCameraDrag(Rect previewRect)
        {
            Event e = Event.current;
            Vector2 mousePos = e.mousePosition;

            if (!previewRect.Contains(mousePos) || m_IsDraggingObject)
                return;

            if (e.button == 1)
            {
                switch (e.type)
                {
                    case EventType.MouseDown:
                        m_IsDraggingMap = true;
                        m_LastMousePos = mousePos;
                        e.Use();
                        break;

                    case EventType.MouseDrag:
                        if (m_IsDraggingMap)
                        {
                            Vector2 delta = mousePos - m_LastMousePos;
                            m_LastMousePos = mousePos;
                            float moveScale = (1f / m_MapZoom) * 0.01f;
                            m_DragOffset -= new Vector2(delta.x, -delta.y) * moveScale;
                            e.Use();
                        }

                        break;

                    case EventType.MouseUp:
                        m_IsDraggingMap = false;
                        e.Use();
                        break;
                }
            }
        }

        /// <summary>
        /// 更新渲染纹理尺寸
        /// </summary>
        private void UpdatePreviewRenderTexture()
        {
            int rtWidth = Mathf.Max((int)m_PreviewRect.width, 1);
            int rtHeight = Mathf.Max((int)m_PreviewRect.height, 1);

            if (m_RenderTexture == null || m_RenderTexture.width != rtWidth || m_RenderTexture.height != rtHeight)
            {
                DestroyImmediate(m_RenderTexture);
                m_RenderTexture = new RenderTexture(rtWidth, rtHeight, 24, RenderTextureFormat.ARGB32);
                m_RenderTexture.Create();
            }
        }

        /// <summary>
        /// 渲染并绘制预览画面
        /// </summary>
        private void DrawPreviewToEditor()
        {
            UpdatePreviewCamera(m_PreviewRect);
            Canvas.ForceUpdateCanvases();

            m_RenderCam.targetTexture = m_RenderTexture;
            m_RenderCam.Render();
            m_RenderCam.targetTexture = null;

            GUI.DrawTexture(m_PreviewRect, m_RenderTexture, ScaleMode.StretchToFill, false);
            Repaint();
        }
        #endregion

        //=========================================================================
        // 背景图片替换
        //=========================================================================
        #region Background Replace
        /// <summary>
        /// 获取地图背景替换指定地图的背景图
        /// </summary>
        private void LoadReplaceBackgroundToImage()
        {
            // 1. 校验：必须先加载地图实例才能替换背景
            if (m_UiInstance == null)
            {
                EditorUtility.DisplayDialog("提示", "请先加载地图Prefab，再更换背景图片！", "确定");
                return;
            }

            // 2. 打开文件选择面板（只允许选择图片格式）
            string imagePath = EditorUtility.OpenFilePanel(
                "选择地图背景图片",
                Application.dataPath,
                "png,jpg,jpeg,bmp" // 支持的图片格式
            );

            // 3. 如果用户取消选择，直接返回
            if (string.IsNullOrEmpty(imagePath))
                return;

            try
            {
                // 4. 转换为 Unity 工程相对路径（必须用相对路径加载资源）
                string projectRelativePath = FileUtil.GetProjectRelativePath(imagePath);

                // 5. 校验路径是否在 Assets 目录内
                if (string.IsNullOrEmpty(projectRelativePath))
                {
                    EditorUtility.DisplayDialog("错误", "请选择项目 Assets 目录内的图片文件！", "确定");
                    return;
                }

                // 6. 加载 Texture2D 资源（Unity 标准加载方式）
                Texture2D backgroundTex = AssetDatabase.LoadAssetAtPath<Texture2D>(projectRelativePath);
                if (backgroundTex == null)
                {
                    EditorUtility.DisplayDialog("错误", "加载图片失败，请检查文件是否为有效图片！", "确定");
                    return;
                }

                // 7. 查找地图中的 RawImage 背景组件
                RawImage bgRawImage = m_UiInstance.GetComponentInChildren<RawImage>(true);
                if (bgRawImage == null)
                {
                    EditorUtility.DisplayDialog("错误", "地图Prefab中未找到 RawImage 背景组件！", "确定");
                    return;
                }

                // 8. 替换背景图片（核心逻辑）
                bgRawImage.texture = backgroundTex;
                bgRawImage.color = Color.white; // 恢复白色，防止颜色叠加

                // 9. 刷新画布 + 提示
                Canvas.ForceUpdateCanvases();
                EditorUtility.DisplayDialog("成功", $"已替换背景图片：\n{backgroundTex.name}", "确定");
                Debug.Log($"✅ 背景图片替换完成：{projectRelativePath}");

                // 10. 刷新编辑器预览
                Repaint();
            }
            catch (Exception e)
            {
                EditorUtility.DisplayDialog("异常", $"替换背景失败：{e.Message}", "确定");
                Debug.LogError($"❌ 替换背景图片异常：{e}");
            }
        }
        #endregion

        //=========================================================================
        // 地图预制体加载
        //=========================================================================
        #region Map Prefab Load
        /// <summary>
        /// 加载地图Prefab并初始化预览环境
        /// </summary>
        /// <param name="originalPrefab">原始预制体</param>
        /// <param name="onLoaded">加载完成回调</param>
        private void LoadMapPrefab(GameObject originalPrefab, Action onLoaded)
        {
            ClearAddedObjects();

            // 销毁旧实例
            if (!EditorUtility.IsPersistent(m_UiInstance))
            {
                DestroyImmediate(m_UiInstance);
            }

            m_UiInstance = null;

            if (originalPrefab == null) return;

            // 实例化预览对象
            m_UiInstance = (GameObject)PrefabUtility.InstantiatePrefab(originalPrefab);
            m_UiInstance.name = $"{originalPrefab.name}_PreviewInstance";
            m_UiInstance.hideFlags = HideFlags.HideAndDontSave;
            m_UiInstance.SetActive(false);

            // 校验Canvas
            Canvas canvas = m_UiInstance.GetComponent<Canvas>();
            if (canvas == null)
            {
                DestroyImmediate(m_UiInstance);
                m_UiInstance = null;
                Debug.LogError("Prefab 缺少 Canvas 组件");
                return;
            }

            // 配置相机
            if (m_RenderCam != null)
            {
                m_RenderCam.orthographic = true;
                m_RenderCam.orthographicSize = m_BaseOrthoSize;
            }

            // 配置Canvas
            ConfigureCanvasForPreview(canvas);

            // 初始化地图根节点
            m_MapRoot = m_UiInstance.transform.Find("Root") ?? canvas.GetComponent<RectTransform>();
            m_MapBoundsSize = new Vector2(m_BaseOrthoSize * 2, m_BaseOrthoSize * 2);

            // 初始化背景图
            RawImage bgImage = m_UiInstance.GetComponentInChildren<RawImage>();
            if (bgImage != null)
            {
                bgImage.material = Canvas.GetDefaultCanvasMaterial();
                bgImage.color = Color.white;
                bgImage.raycastTarget = false;
            }

            // 显示对象
            m_UiInstance.SetActive(true);
            canvas.enabled = true;
            Canvas.ForceUpdateCanvases();

            // 重置视图
            m_DragOffset = Vector2.zero;
            m_MapZoom = 1f;

            Debug.Log($"==================== 地图加载完成，根节点：{m_MapRoot.name} ====================");
            Repaint();
            onLoaded?.Invoke();
        }

        /// <summary>
        /// 配置Canvas用于编辑器预览
        /// </summary>
        private void ConfigureCanvasForPreview(Canvas canvas)
        {
            canvas.enabled = false;
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = m_RenderCam;
            canvas.overrideSorting = true;
            canvas.sortingOrder = 0;

            // 关闭自适应缩放
            if (canvas.TryGetComponent(out CanvasScaler scaler))
                scaler.enabled = false;

            // 设置RectTransform居中
            RectTransform rect = canvas.GetComponent<RectTransform>();
            rect.anchoredPosition = Vector2.zero;
            rect.anchorMin = rect.anchorMax = rect.pivot = new Vector2(0.5f, 0.5f);
            rect.sizeDelta = new Vector2(2048, 2048);
            rect.localScale = Vector3.one * 0.005f;
            rect.position = Vector3.zero;
            rect.rotation = Quaternion.identity;
        }
        #endregion

        //=========================================================================
        // 物体添加与配置
        //=========================================================================
        #region Object Add & Configure
        /// <summary>
        /// 添加预制体到地图预览界面
        /// </summary>
        /// <param name="prefab">目标预制体</param>
        /// <param name="localPosition">UI本地坐标</param>
        /// <param name="dataEditor">图标数据</param>
        private void AddPrefabToMap(GameObject prefab, Vector2 localPosition, ItemIconDataEditor dataEditor)
        {
            if (prefab == null || m_MapRoot == null)
            {
                EditorUtility.DisplayDialog("提示", "请先选择要添加的预制体", "确定");
                return;
            }

            // 实例化物体
            GameObject newObj = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            newObj.name = $"{prefab.name}_Instance_{m_AddObjects.Count}";
            newObj.hideFlags = HideFlags.HideAndDontSave;

            // 设置父子与坐标
            newObj.transform.SetParent(m_MapRoot, false);
            SetObjectTransformForPreview(newObj, localPosition);

            // 配置层级与射线
            ConfigureObjectForInteract(newObj);

            // 绑定地图数据
            RMapData mapData = new RMapData();
            if (dataEditor != null)
            {
                mapData.m_Id = dataEditor.m_ID;
                mapData.m_IsChoose = dataEditor.m_IsChoose;
                mapData.m_Type = dataEditor.m_Type;
                mapData.m_Sprite = dataEditor.m_IconAssetPath;
                mapData.m_Name = dataEditor.m_IconName;
            }

            m_AddObjects.Add(newObj);
            m_RMapDatas.Add(newObj, mapData);
            Canvas.ForceUpdateCanvases();
            DeselectCurrentObject();

            Debug.Log($"✅ 成功添加物体：{newObj.name}，坐标：{localPosition}");
            Repaint();
        }

        /// <summary>
        /// 设置物体Transform适配编辑器预览
        /// </summary>
        private void SetObjectTransformForPreview(GameObject obj, Vector2 localPos)
        {
            obj.transform.localScale = Vector3.one;
            obj.transform.localRotation = Quaternion.identity;
            obj.transform.localPosition = new Vector3(localPos.x, localPos.y, 0);

            if (obj.TryGetComponent(out RectTransform rect))
            {
                rect.anchoredPosition = localPos;
                rect.localScale = Vector3.one;
                rect.localRotation = Quaternion.identity;
                rect.anchorMin = rect.anchorMax = rect.pivot = new Vector2(0.5f, 0.5f);
            }
        }

        /// <summary>
        /// 配置物体可交互、可选中
        /// </summary>
        private void ConfigureObjectForInteract(GameObject obj)
        {
            // 增加层级防止遮挡
            Canvas canvas = obj.GetOrAddComponent<Canvas>();
            canvas.overrideSorting = true;
            canvas.sortingOrder = 2000 + m_AddObjects.Count;

            obj.GetOrAddComponent<GraphicRaycaster>();

            // 开启射线检测
            foreach (Graphic graphic in obj.GetComponentsInChildren<Graphic>(true))
            {
                graphic.raycastTarget = true;
                graphic.material = Canvas.GetDefaultCanvasMaterial();
            }
        }
        #endregion
    }
}