/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  SceneEditModule.Methods.cs
 * author:    云毅
 * created:   2026
 * descrip:   场景编辑模块 - 私有方法（UI查找/事件绑定/状态栏/保存与导出入口）
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
    /// 场景编辑模块 - 私有方法（UI查找/事件绑定/状态栏/保存导出）
    /// </summary>
    public partial class SceneEditModule
    {
        /// <summary>
        /// 绑定事件
        /// </summary>
        private void BindEvents()
        {
            // 更新地图按钮
            var updateBtn = m_Root.Q<Button>(name: "btn-update-map");
            if (updateBtn != null)
                updateBtn.clicked += OnUpdateMapClicked;
            // 导出切片按钮
            var exportBtn = m_Root.Q<Button>(name: "btn-export-slice");
            if (exportBtn != null)
                exportBtn.clicked += OnExportSliceClicked;
            // 保存按钮
            var saveBtn = m_Root.Q<Button>(name: "btn-save");
            if (saveBtn != null)
                saveBtn.clicked += OnSaveClicked;
            // 对象编辑面板事件（可编辑路径 / 编辑动画 切页）
            if (m_TabEditPath != null)
                m_TabEditPath.clicked += OnTabEditPathClicked;
            if (m_TabEditAnim != null)
                m_TabEditAnim.clicked += OnTabEditAnimClicked;
            if (m_BtnPathDraw != null)
                m_BtnPathDraw.clicked += OnPathDrawClicked;
            if (m_BtnPathUndo != null)
                m_BtnPathUndo.clicked += OnPathUndoClicked;
            if (m_BtnPathClear != null)
                m_BtnPathClear.clicked += OnPathClearClicked;
            if (m_BtnClearTarget != null)
                m_BtnClearTarget.clicked += OnClearTargetClicked;
            // 动画切页字段变更 → 写回当前编辑目标
            if (m_AnimNameField != null)
                m_AnimNameField.RegisterValueChangedCallback(evt =>
                {
                    if (m_EditTarget != null)
                        m_EditTarget.AnimName = evt.newValue;
                });
            if (m_AnimLoopToggle != null)
                m_AnimLoopToggle.RegisterValueChangedCallback(evt =>
                {
                    if (m_EditTarget != null)
                        m_EditTarget.Loop = evt.newValue;
                });
            if (m_AnimSpeedField != null)
                m_AnimSpeedField.RegisterValueChangedCallback(evt =>
                {
                    if (m_EditTarget != null && float.TryParse(evt.newValue, out float speed))
                        m_EditTarget.Speed = speed;
                });
        }

        /// <summary>
        /// 更新地图按钮点击
        /// </summary>
        private void OnUpdateMapClicked()
        {
            Debug.Log("更新地图");
        }

        /// <summary>
        /// 导出切片按钮点击
        /// </summary>
        private void OnExportSliceClicked()
        {
            Debug.Log("导出切片");
        }

        /// <summary>
        /// 保存按钮点击
        /// </summary>
        private void OnSaveClicked()
        {
            Debug.Log("保存地图");
            SaveMapConfig();
        }

        /// <summary>
        /// 编辑器每帧更新，请求重绘
        /// </summary>
        private void OnEditorUpdate()
        {
            if (m_GuiContainer == null) return;
            // 请求重绘
            m_GuiContainer.MarkDirtyRepaint();
        }

        /// <summary>
        /// 地图预制体 ObjectField 变化
        /// </summary>
        /// <param name="evt">变化事件</param>
        private void OnMapPrefabFieldChanged(ChangeEvent<Object> evt)
        {
            m_MapPrefab = evt.newValue as GameObject;
            if (m_MapPrefab != null)
            {
                string prefabPath = UnityEditor.AssetDatabase.GetAssetPath(m_MapPrefab);
                Debug.Log($"预制体被修改: {prefabPath}");
                OnMapPrefabChanged?.Invoke(prefabPath);
            }
            else
            {
                OnMapPrefabChanged?.Invoke("");
            }
        }

        /// <summary>
        /// 更新状态栏（仅在值变化时刷新，避免每帧重建 UI）
        /// </summary>
        private void UpdateStatusBar()
        {
            bool needUpdate = false;
            float currentZoom = m_Zoom * 100;
            if (!Mathf.Approximately(currentZoom, m_LastZoom))
            {
                m_LastZoom = currentZoom;
                needUpdate = true;
            }
            if (!Mathf.Approximately(m_MousePos.x, m_LastMouseX))
            {
                m_LastMouseX = m_MousePos.x;
                needUpdate = true;
            }
            if (!Mathf.Approximately(m_MousePos.y, m_LastMouseY))
            {
                m_LastMouseY = m_MousePos.y;
                needUpdate = true;
            }
            if (m_TileX != m_LastTileX)
            {
                m_LastTileX = m_TileX;
                needUpdate = true;
            }
            if (m_TileY != m_LastTileY)
            {
                m_LastTileY = m_TileY;
                needUpdate = true;
            }
            if (m_MapSavePath != m_LastSavePath)
            {
                m_LastSavePath = m_MapSavePath;
                needUpdate = true;
            }
            if (!needUpdate) return;
            if (m_ZoomLabel != null)
                m_ZoomLabel.text = $"{m_Zoom * 100:F0}%";
            if (m_MouseXLabel != null)
                m_MouseXLabel.text = m_MousePos.x.ToString("F0");
            if (m_MouseYLabel != null)
                m_MouseYLabel.text = m_MousePos.y.ToString("F0");
            if (m_TileXLabel != null)
                m_TileXLabel.text = m_TileX.ToString();
            if (m_TileYLabel != null)
                m_TileYLabel.text = m_TileY.ToString();
            if (m_TileCountLabel != null)
                m_TileCountLabel.text = (m_TileX * m_TileY).ToString();
            if (m_SavePathBtn != null)
                m_SavePathBtn.text = m_MapSavePath;
        }

        /// <summary>
        /// 保存路径按钮点击
        /// </summary>
        private void OnSavePathClicked()
        {
            string path = UnityEditor.EditorUtility.OpenFolderPanel("选择地图保存文件夹", m_MapSavePath, "");
            if (!string.IsNullOrEmpty(path))
            {
                // 转换为相对路径
                if (path.StartsWith(Application.dataPath))
                {
                    path = "Assets" + path.Substring(Application.dataPath.Length);
                }
                m_MapSavePath = path;
                UpdateStatusBar();
            }
        }

        /// <summary>
        /// 查找UI元素
        /// </summary>
        private void FindElements()
        {
            m_SceneView = m_Root.Q<VisualElement>(className: "scene-view");
            // 查找状态栏 Labels
            m_ZoomLabel = m_Root.Q<Label>(name: "status-zoom");
            m_MouseXLabel = m_Root.Q<Label>(name: "status-mouse-x");
            m_MouseYLabel = m_Root.Q<Label>(name: "status-mouse-y");
            m_TileXLabel = m_Root.Q<Label>(name: "status-tile-x");
            m_TileYLabel = m_Root.Q<Label>(name: "status-tile-y");
            m_TileCountLabel = m_Root.Q<Label>(name: "status-tile-count");
            m_SavePathBtn = m_Root.Q<Button>(name: "status-save-path");
            // 查找地图预制体 ObjectField 容器
            var prefabRow = m_Root.Q<VisualElement>(name: "map-prefab-row");
            Debug.Log($"查找 map-prefab-row: {prefabRow != null}");
            if (prefabRow != null)
            {
                m_MapPrefabField = new ObjectField();
                m_MapPrefabField.objectType = typeof(GameObject);
                m_MapPrefabField.style.width = 200;
                m_MapPrefabField.RegisterValueChangedCallback(OnMapPrefabFieldChanged);
                prefabRow.Add(m_MapPrefabField);
                Debug.Log("ObjectField 已创建");
            }
            // 查找图层 Toggle
            m_PlaceGridToggle = m_Root.Q<Toggle>(name: "layer-place-grid");
            m_MapLayerToggle = m_Root.Q<Toggle>(name: "layer-map");
            m_BuildingLayerToggle = m_Root.Q<Toggle>(name: "layer-building");
            m_CharacterLayerToggle = m_Root.Q<Toggle>(name: "layer-character");
            m_ObjectLayerToggle = m_Root.Q<Toggle>(name: "layer-object");
            m_BackgroundLayerToggle = m_Root.Q<Toggle>(name: "layer-background");
            m_PathGridToggle = m_Root.Q<Toggle>(name: "layer-path-grid");
            m_BoundingBoxToggle = m_Root.Q<Toggle>(name: "layer-bounding-box");
            // 对象编辑面板（选中人物后显示：可编辑路径 / 编辑动画）
            m_ObjectEditPanel = m_Root.Q<VisualElement>(name: "object-edit-panel");
            if (m_ObjectEditPanel != null)
            {
                m_ObjEditTitle = m_ObjectEditPanel.Q<Label>(name: "obj-edit-title");
                m_TabEditPath = m_ObjectEditPanel.Q<Button>(name: "tab-edit-path");
                m_TabEditAnim = m_ObjectEditPanel.Q<Button>(name: "tab-edit-anim");
                m_PathEditContent = m_ObjectEditPanel.Q<VisualElement>(name: "path-edit-content");
                m_AnimEditContent = m_ObjectEditPanel.Q<VisualElement>(name: "anim-edit-content");
                m_BtnPathDraw = m_ObjectEditPanel.Q<Button>(name: "btn-path-draw");
                m_BtnPathUndo = m_ObjectEditPanel.Q<Button>(name: "btn-path-undo");
                m_BtnPathClear = m_ObjectEditPanel.Q<Button>(name: "btn-path-clear");
                m_BtnClearTarget = m_ObjectEditPanel.Q<Button>(name: "btn-clear-target");
                m_PathPointList = m_ObjectEditPanel.Q<ScrollView>(name: "path-point-list");
                // 默认隐藏，选中物体后显示
                m_ObjectEditPanel.style.display = DisplayStyle.None;
            }
            // 动画切页字段
            m_AnimNameField = m_Root.Q<TextField>(name: "anim-name-field");
            m_AnimLoopToggle = m_Root.Q<Toggle>(name: "anim-loop-toggle");
            m_AnimSpeedField = m_Root.Q<TextField>(name: "anim-speed-field");
            // 场景信息字段（保存配置时读取）
            m_SceneNameField = m_Root.Q<TextField>(name: "scene-name-field");
            m_SceneIdField = m_Root.Q<TextField>(name: "scene-id-field");
            m_WidthField = m_Root.Q<TextField>(name: "map-width-field");
            m_HeightField = m_Root.Q<TextField>(name: "map-height-field");
            // 绑定保存路径按钮点击事件
            if (m_SavePathBtn != null)
                m_SavePathBtn.clicked += OnSavePathClicked;
            // 绑定图层 Toggle 事件
            if (m_PlaceGridToggle != null)
                m_PlaceGridToggle.RegisterValueChangedCallback(evt => m_ShowPlaceGrid = evt.newValue);
            if (m_MapLayerToggle != null)
                m_MapLayerToggle.RegisterValueChangedCallback(evt => m_ShowMapLayer = evt.newValue);
            if (m_BuildingLayerToggle != null)
                m_BuildingLayerToggle.RegisterValueChangedCallback(evt => m_ShowBuildingLayer = evt.newValue);
            if (m_CharacterLayerToggle != null)
                m_CharacterLayerToggle.RegisterValueChangedCallback(evt => m_ShowCharacterLayer = evt.newValue);
            if (m_ObjectLayerToggle != null)
                m_ObjectLayerToggle.RegisterValueChangedCallback(evt => m_ShowObjectLayer = evt.newValue);
            if (m_BackgroundLayerToggle != null)
                m_BackgroundLayerToggle.RegisterValueChangedCallback(evt => m_ShowBackgroundLayer = evt.newValue);
            if (m_PathGridToggle != null)
                m_PathGridToggle.RegisterValueChangedCallback(evt => m_ShowPathGrid = evt.newValue);
            if (m_BoundingBoxToggle != null)
                m_BoundingBoxToggle.RegisterValueChangedCallback(evt => m_ShowBoundingBox = evt.newValue);
            // 添加IMGUI容器用于地图渲染
            if (m_SceneView != null)
            {
                m_GuiContainer = new IMGUIContainer(OnSceneGUI);
                m_GuiContainer.style.flexGrow = 1;
                m_GuiContainer.style.overflow = Overflow.Hidden;
                m_GuiContainer.style.backgroundColor = Color.clear;
                m_GuiContainer.focusable = false;
                m_SceneView.Add(m_GuiContainer);
            }
        }

    }
}
