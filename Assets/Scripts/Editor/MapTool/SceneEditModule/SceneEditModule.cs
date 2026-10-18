/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  SceneEditModule.cs
 * author:    云毅
 * created:   2026
 * descrip:   场景编辑模块 - 公有接口部分（初始化/地图保存/物体选择/显隐）
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
    /// 场景编辑模块 - 公有接口（初始化/保存/选择/显隐）
    /// </summary>
    public partial class SceneEditModule
    {
        /// <summary>
        /// 初始化模块
        /// </summary>
        /// <param name="root">根节点</param>
        public void Init(VisualElement root)
        {
            m_Root = root;
            // 初始化物体模板管理器
            ObjectTemplateManager.Instance.Initialize();
            FindElements();
            BindEvents();
        }

        /// <summary>
        /// 保存地图配置为 JSON 文件
        /// 包含分区信息（区域ID/背景名/是否解锁/迷雾是否开启）、
        /// 物件集合、建筑集合、人物集合（含可走路径）
        /// </summary>
        public void SaveMapConfig()
        {
            if (m_RegionIds == null || m_RegionIds.Length == 0)
            {
                Debug.LogWarning("没有分区数据，无法保存地图配置（请先选择章节加载地图）");
                return;
            }
            // 组装配置数据
            MapConfigData data = new MapConfigData();
            data.sceneId = GetFieldValue(m_SceneIdField, "105");
            data.sceneName = GetFieldValue(m_SceneNameField, "105");
            data.width = ParseInt(GetFieldValue(m_WidthField, m_ConfigMapWidth.ToString()), m_ConfigMapWidth);
            data.height = ParseInt(GetFieldValue(m_HeightField, m_ConfigMapHeight.ToString()), m_ConfigMapHeight);
            // 分区信息（每个分区下挂属于自己的建筑/物件/人物集合）
            for (int i = 0; i < m_RegionCount; i++)
            {
                int row = i / m_RegionCols;
                int col = i % m_RegionCols;
                string regionId = m_RegionIds[i];
                bool unlocked = m_RegionUnlocked != null && row < m_RegionRows && col < m_RegionCols && m_RegionUnlocked[row, col];
                string bgName = "";
                if (m_LevelBgDict != null && m_LevelBgDict.TryGetValue(regionId, out string bg))
                    bgName = bg;
                data.regions.Add(new MapRegionData
                {
                    regionId = regionId,
                    bgName = bgName,
                    unlocked = unlocked,
                    fogEnabled = !unlocked, // 未解锁分区迷雾开启
                });
            }
            // 已放置物体按所在分区归组，挂到对应区域下
            foreach (var obj in m_PlacedObjects)
            {
                int regionIndex = GetRegionIndexForObject(obj);
                if (regionIndex < 0 || regionIndex >= data.regions.Count)
                    continue;
                MapRegionData region = data.regions[regionIndex];
                switch (obj.Type)
                {
                    case MapObjectType.Character: // 人物：含可走路径集合
                        region.characters.Add(BuildMapCharacterData(obj));
                        break;
                    case MapObjectType.Building: // 建筑
                        region.buildings.Add(BuildMapObjectData(obj));
                        break;
                    case MapObjectType.Prop: // 物件
                        region.objects.Add(BuildMapObjectData(obj));
                        break;
                }
            }
            // 写入 JSON 文件（m_MapSavePath 为 Assets 相对路径）
            string relativePath = m_MapSavePath.Replace("\\", "/");
            if (!relativePath.StartsWith("Assets/"))
                relativePath = "Assets/" + relativePath.TrimStart('/');
            string fullPath = Path.Combine(Application.dataPath, relativePath.Substring("Assets/".Length));
            try
            {
                string dir = Path.GetDirectoryName(fullPath);
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
                string json = JsonUtility.ToJson(data, true);
                File.WriteAllText(fullPath, json, new UTF8Encoding(false));
                AssetDatabase.Refresh();
                // 统计各区域物体数量
                int totalBuildings = 0, totalObjects = 0, totalCharacters = 0;
                foreach (var r in data.regions)
                {
                    totalBuildings += r.buildings.Count;
                    totalObjects += r.objects.Count;
                    totalCharacters += r.characters.Count;
                }
                Debug.Log($"地图配置已保存: {relativePath}（分区 {data.regions.Count}，建筑 {totalBuildings}，物件 {totalObjects}，人物 {totalCharacters}）");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"保存地图配置失败: {e.Message}");
            }
        }

        /// <summary>
        /// 显示模块
        /// </summary>
        public void Show()
        {
            m_Root.style.display = DisplayStyle.Flex;
        }

        /// <summary>
        /// 隐藏模块
        /// </summary>
        public void Hide()
        {
            m_Root.style.display = DisplayStyle.None;
        }

        /// <summary>
        /// 设置地图保存路径
        /// </summary>
        /// <param name="path">保存路径</param>
        public void SetMapSavePath(string path)
        {
            m_MapSavePath = path;
            UpdateStatusBar();
        }

        /// <summary>
        /// 获取地图保存路径
        /// </summary>
        public string GetMapSavePath()
        {
            return m_MapSavePath;
        }

        /// <summary>
        /// 加载章节地图
        /// </summary>
        /// <param name="chapterId">章节ID</param>
        /// <param name="prefabPath">地图预制体路径</param>
        public void LoadChapterMap(string chapterId, string prefabPath = "", string chapterInfoID = "9")
        {
            Debug.Log($"加载章节地图: {chapterId}, 预制体路径: {prefabPath}, 分区配置: {chapterInfoID}");
            // 加载章节配置表，获取场景名称/场景ID/分区配置
            LoadChapterConfig();
            var chapterData = m_ChapterDataList.FirstOrDefault(c => c.ID == chapterId);
            if (chapterData != null)
            {
                // 场景名：优先用章节标题，其次地图资源名
                string sceneName = !string.IsNullOrEmpty(chapterData.ChapterTitle) ? chapterData.ChapterTitle : chapterData.MapName;
                if (string.IsNullOrEmpty(sceneName))
                    sceneName = chapterId;
                if (m_SceneNameField != null)
                    m_SceneNameField.SetValueWithoutNotify(sceneName);
                if (m_SceneIdField != null)
                    m_SceneIdField.SetValueWithoutNotify(chapterId);
                // 调用方未指定分区配置时，用章节配置表里的分区配置
                if (string.IsNullOrEmpty(chapterInfoID) || chapterInfoID == "9")
                    chapterInfoID = chapterData.ChapterInfoID ?? chapterInfoID;
            }
            else
            {
                // 配置表中无此章节：场景ID用章节ID，场景名用ID兜底
                if (m_SceneNameField != null)
                    m_SceneNameField.SetValueWithoutNotify(chapterId);
                if (m_SceneIdField != null)
                    m_SceneIdField.SetValueWithoutNotify(chapterId);
            }
            // 解析 ChapterInfoID 配置，格式如 {"1","2","3"}
            // 有几个元素就有几个分区
            string[] regionIds = MapToolEditorUtils.ParseRegionIds(chapterInfoID);
            m_RegionIds = regionIds;
            int regionCount = regionIds.Length;
            Debug.Log($"分区数量: {regionCount}");
            // 加载 TableMainLevels 配置表
            LoadMainLevelsConfig();
            // 计算行列数（尽量接近正方形）
            int cols = Mathf.CeilToInt(Mathf.Sqrt(regionCount));
            int rows = Mathf.CeilToInt((float)regionCount / cols);
            m_RegionRows = rows;
            m_RegionCols = cols;
            Debug.Log($"分区布局: {rows}行 x {cols}列");
            // 初始化分区解锁状态
            m_RegionUnlocked = new bool[rows, cols];
            m_RegionCount = regionCount; // 记录实际分区数量
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    int index = row * cols + col;
                    // 只有配置中存在的分区才显示，第一个默认解锁
                    if (index < regionCount)
                    {
                        m_RegionUnlocked[row, col] = (index == 0);
                    }
                    else
                    {
                        m_RegionUnlocked[row, col] = false; // 多余的格子不显示
                    }
                }
            }
            string chapterPrefabPath = $"{MapToolConstants.MapSaveDir}/Map_{chapterId}.prefab";
            // 如果章节预制体不存在，从模板克隆
            if (!UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(chapterPrefabPath))
            {
                if (!string.IsNullOrEmpty(prefabPath))
                {
                    Debug.Log($"章节预制体不存在，从模板克隆: {prefabPath} -> {chapterPrefabPath}");
                    // 确保目录存在（Assets/GameData/Maps）
                    string dir = System.IO.Path.GetDirectoryName(chapterPrefabPath).Replace("\\", "/");
                    if (!UnityEditor.AssetDatabase.IsValidFolder(dir))
                    {
                        if (!UnityEditor.AssetDatabase.IsValidFolder(MapToolConstants.GameDataDir))
                            UnityEditor.AssetDatabase.CreateFolder("Assets", "GameData");
                        UnityEditor.AssetDatabase.CreateFolder(MapToolConstants.GameDataDir, "Maps");
                    }
                    // 克隆模板预制体
                    bool copyResult = UnityEditor.AssetDatabase.CopyAsset(prefabPath, chapterPrefabPath);
                    Debug.Log($"克隆结果: {copyResult}");
                    UnityEditor.AssetDatabase.Refresh();
                }
            }
            // 加载章节预制体
            m_MapPrefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(chapterPrefabPath);
            Debug.Log($"加载章节预制体: {chapterPrefabPath}, 结果: {m_MapPrefab != null}");
            if (m_MapPrefab != null)
            {
                Debug.Log($"加载地图预制体成功: {m_MapPrefab.name}");
                // 同步更新左上角的 ObjectField 显示
                Debug.Log($"m_MapPrefabField 是否为 null: {m_MapPrefabField == null}");
                if (m_MapPrefabField != null)
                {
                    m_MapPrefabField.value = m_MapPrefab;
                    Debug.Log($"已设置 ObjectField 值: {m_MapPrefab.name}");
                }
                // 注意：不能直接修改预制体资源，迷雾覆盖在 IMGUI 中绘制
            }
            else
            {
                Debug.LogWarning("地图预制体加载失败，m_MapPrefab 为 null");
            }
            // 更新保存路径
            m_MapSavePath = $"{MapToolConstants.MapSaveDir}/Map_{chapterId}.json";
            m_MapWidth = MapToolConstants.RegionPixelSize * cols;   // 渲染用全布局宽度
            m_MapHeight = MapToolConstants.RegionPixelSize * rows;  // 渲染用全布局高度
            // 根据已解锁分区包围盒更新配置宽高（解锁新区域时同步变化）
            UpdateMapBoundsFromUnlockedRegions();
            // 重置偏移和缩放
            m_MapOffset = Vector2.zero;
            m_Zoom = 1.0f;
            UpdateStatusBar();
        }

        /// <summary>
        /// 设置选中要放置的物体
        /// </summary>
        /// <param name="objectId">物体ID</param>
        /// <param name="objectName">物体名称</param>
        /// <param name="type">类型：0=建筑 1=人物 2=物件</param>
        public void SetSelectedObject(string objectId, string objectName, int type)
        {
            MapObjectType objectType = (MapObjectType)type;
            m_SelectedObjectId = objectId;
            m_SelectedObjectName = objectName;
            m_SelectedObjectType = objectType;

            // 创建待放置预览对象
            Vector2Int defaultSize = MapToolEditorUtils.GetDefaultObjectSize(objectType);
            Texture previewTex = LoadObjectPreview(objectId);

            // 从模板继承属性
            ObjectTemplate template = ObjectTemplateManager.Instance.GetTemplateById(objectId);
            m_PendingPlace = new PlacedObject
            {
                Id = System.Guid.NewGuid().ToString("N").Substring(0, 8), // 实例ID
                TemplateId = objectId, // 模板ID
                Name = objectName,
                Type = objectType,
                Width = defaultSize.x,
                Height = defaultSize.y,
                PreviewTexture = previewTex,
            };

            // 从模板复制属性
            if (template != null)
            {
                m_PendingPlace.AnimName = template.AnimName;
                m_PendingPlace.Loop = template.Loop;
                m_PendingPlace.Speed = template.Speed;
                m_PendingPlace.Interactable = template.Interactable;
                m_PendingPlace.InteractType = template.InteractType;
                m_PendingPlace.SortOrder = template.SortOrder;
                m_PendingPlace.CollisionEnabled = template.CollisionEnabled;
                m_PendingPlace.DialogBubble = template.DialogBubble;
                m_PendingPlace.FacingDirection = template.FacingDirection;
                m_PendingPlace.ShadowEnabled = template.ShadowEnabled;
                m_PendingPlace.IsCollectible = template.IsCollectible;
                m_PendingPlace.RespawnTime = template.RespawnTime;
            }

            // 选中人物资源时：自动绑定画布上第一个同名实例作为路径编辑目标，底部显示切页
            if (objectType == MapObjectType.Character)
            {
                PlacedObject target = m_PlacedObjects.FirstOrDefault(o => o.Name == objectName);
                if (target != null)
                {
                    SelectEditTarget(target);
                }
                else
                {
                    // 地图上还没有该人物实例：显示面板占位（放置后自动绑定）
                    ShowEditPanelWithName(objectName);
                }
            }
            Debug.Log($"选中物体: {objectName} ({objectId}), 类型: {type}");
        }

        /// <summary>
        /// 设置地图尺寸
        /// </summary>
        /// <param name="width">地图宽度</param>
        /// <param name="height">地图高度</param>
        public void SetMapSize(int width, int height)
        {
            m_MapWidth = width;
            m_MapHeight = height;
        }

        /// <summary>
        /// 获取地图宽度
        /// </summary>
        public int GetMapWidth()
        {
            return m_MapWidth;
        }

        /// <summary>
        /// 获取地图高度
        /// </summary>
        public int GetMapHeight()
        {
            return m_MapHeight;
        }

        /// <summary>
        /// 选中编辑目标物体（单击地图上已放置的人物/物体触发）
        /// </summary>
        /// <param name="obj">目标物体</param>
        public void SelectEditTarget(PlacedObject obj)
        {
            if (obj == null) return;
            m_EditTarget = obj;
            m_SelectedPathPointIndex = -1;

            // 只有人物才进入路径绘制模式
            bool isCharacter = obj.Type == MapObjectType.Character;
            m_IsPathDrawing = isCharacter;

            // 显示对象编辑面板
            if (m_ObjectEditPanel != null)
            {
                m_ObjectEditPanel.style.display = DisplayStyle.Flex;
                // 人物默认切到"可编辑路径"，其他类型切到"编辑动画"
                SetActiveObjectTab(isCharacter ? 0 : 1);
            }

            // 路径绘制按钮只在人物时显示
            if (m_BtnPathDraw != null)
            {
                m_BtnPathDraw.style.display = isCharacter ? DisplayStyle.Flex : DisplayStyle.None;
                m_BtnPathDraw.text = "停止绘制路径";
            }

            // 填充动画切页字段
            if (m_AnimNameField != null)
                m_AnimNameField.SetValueWithoutNotify(obj.AnimName);
            if (m_AnimLoopToggle != null)
                m_AnimLoopToggle.SetValueWithoutNotify(obj.Loop);
            if (m_AnimSpeedField != null)
                m_AnimSpeedField.SetValueWithoutNotify(obj.Speed.ToString("F1"));

            // 更新底部属性面板字段
            if (m_TemplateIdField != null)
                m_TemplateIdField.SetValueWithoutNotify(obj.TemplateId);
            if (m_TagField != null)
                m_TagField.SetValueWithoutNotify(obj.Type.ToString());
            if (m_GridPosField != null)
            {
                int gridX = Mathf.FloorToInt(obj.Position.x / 32f);
                int gridY = Mathf.FloorToInt(obj.Position.y / 32f);
                m_GridPosField.SetValueWithoutNotify($"{gridX}/{gridY}");
            }
            if (m_BuildingTemplateField != null)
                m_BuildingTemplateField.SetValueWithoutNotify($"{obj.Id}#{Mathf.FloorToInt(obj.Position.x / 32f)}");
            if (m_GridPrefixField != null)
                m_GridPrefixField.SetValueWithoutNotify("前缀");
            if (m_InteractStateField != null)
                m_InteractStateField.SetValueWithoutNotify(obj.Interactable ? "点击&移动" : "不可交互");

            // 填充新增属性面板字段
            if (m_SortOrderField != null)
                m_SortOrderField.SetValueWithoutNotify(obj.SortOrder.ToString());
            if (m_InteractTypeField != null)
                m_InteractTypeField.SetValueWithoutNotify(obj.InteractType);
            if (m_CollisionToggle != null)
                m_CollisionToggle.SetValueWithoutNotify(obj.CollisionEnabled);
            if (m_DialogBubbleField != null)
                m_DialogBubbleField.SetValueWithoutNotify(obj.DialogBubble);
            if (m_FacingField != null)
                m_FacingField.SetValueWithoutNotify(obj.FacingDirection);
            if (m_ShadowToggle != null)
                m_ShadowToggle.SetValueWithoutNotify(obj.ShadowEnabled);
            if (m_CollectibleToggle != null)
                m_CollectibleToggle.SetValueWithoutNotify(obj.IsCollectible);

            // 显示底部属性面板
            if (m_PropertyPanel != null)
                m_PropertyPanel.style.display = DisplayStyle.Flex;

            UpdateEditTargetUI();
            Debug.Log($"选中编辑对象: {obj.Name}（类型: {MapToolEditorUtils.FormatObjectTypeName(obj.Type)}，路径点数: {obj.PathPoints.Count}）");
        }

        /// <summary>
        /// 取消选中编辑目标
        /// </summary>
        public void ClearEditTarget()
        {
            m_EditTarget = null;
            m_SelectedPathPointIndex = -1;
            m_IsPathDrawing = false;
            if (m_ObjectEditPanel != null)
                m_ObjectEditPanel.style.display = DisplayStyle.None;
            // 隐藏底部属性面板
            if (m_PropertyPanel != null)
                m_PropertyPanel.style.display = DisplayStyle.None;
            Debug.Log("取消选中编辑对象");
        }

        /// <summary>
        /// 获取当前编辑目标
        /// </summary>
        public PlacedObject GetEditTarget()
        {
            return m_EditTarget;
        }

    }
}