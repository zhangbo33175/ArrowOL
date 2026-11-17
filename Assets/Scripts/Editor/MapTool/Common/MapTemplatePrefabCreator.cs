/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * -------------------------------------------------------------
 * filename:  MapTemplatePrefabCreator.cs
 * author:    云毅
 * created:   2026
 * descrip:   地图模板预制体生成器 - 一键生成物件/建筑/人物模板预制体
 ***************************************************************/

using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using Honor.Editor.MapTool;
using MapObjectType = Honor.Editor.MapTool.MapObjectType;

namespace Honor.Editor.MapTool
{
    /// <summary>
    /// 地图模板预制体生成器
    /// </summary>
    public class MapTemplatePrefabCreator : EditorWindow
    {
        [MenuItem("Honor/Map Tool/生成模板预制体")]
        public static void ShowWindow()
        {
            var window = GetWindow<MapTemplatePrefabCreator>("模板预制体生成器");
            window.minSize = new Vector2(400, 200);
        }

        private void OnGUI()
        {
            GUILayout.Label("地图模板预制体生成", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            EditorGUILayout.HelpBox("此工具将在 Assets/Res/Prefabs/Model/ 下生成三个基础模板预制体：\n" +
                "1. Prop_BaseTemplate.prefab - 物件模板\n" +
                "2. Building_BaseTemplate.prefab - 建筑模板\n" +
                "3. Character_BaseTemplate.prefab - 人物模板", MessageType.Info);

            EditorGUILayout.Space();

            if (GUILayout.Button("生成所有模板预制体", GUILayout.Height(40)))
            {
                CreateAllTemplates();
            }
        }

        /// <summary>
        /// 生成所有模板预制体
        /// </summary>
        private void CreateAllTemplates()
        {
            EnsureModelPrefabDir();

            // 基础模板预制体路径统一取自 MapToolConstants（与场景同步的兜底模板同源）
            CreateTemplatePrefab(MapToolConstants.PropBaseTemplatePrefab, "物件模板", MapObjectType.Prop, 10, false);
            CreateTemplatePrefab(MapToolConstants.BuildingBaseTemplatePrefab, "建筑模板", MapObjectType.Building, 5, true);
            CreateTemplatePrefab(MapToolConstants.CharacterBaseTemplatePrefab, "人物模板", MapObjectType.Character, 50, true);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            MapToolLog.Info("[MapTemplatePrefabCreator] 所有模板预制体生成完成！");
            MapToolDialog.Success($"模板预制体已生成到 {MapToolConstants.ModelPrefabDir}/ 目录！");
        }

        /// <summary>
        /// 确保基础模板预制体目录存在
        /// </summary>
        private static void EnsureModelPrefabDir()
        {
            if (AssetDatabase.IsValidFolder(MapToolConstants.ModelPrefabDir)) return;

            // 目录名取自常量，父目录按最后一个分隔符切分，避免再写一份路径字面量
            int lastSlash = MapToolConstants.ModelPrefabDir.LastIndexOf('/');
            string parentDir = MapToolConstants.ModelPrefabDir.Substring(0, lastSlash);
            string folderName = MapToolConstants.ModelPrefabDir.Substring(lastSlash + 1);
            AssetDatabase.CreateFolder(parentDir, folderName);
        }

        /// <summary>
        /// 创建单个模板预制体（2D 世界模式：SpriteRenderer + BoxCollider2D）
        /// </summary>
        private void CreateTemplatePrefab(string path, string name, MapObjectType type, int defaultSortOrder, bool defaultCollision)
        {
            // 创建GameObject
            GameObject go = new GameObject(name);

            // 添加 SpriteRenderer（2D 世界模式）
            SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
            sr.sortingOrder = defaultSortOrder;
            sr.sortingLayerName = "Default";

            // 添加 BoxCollider2D（碰撞体）
            BoxCollider2D col = go.AddComponent<BoxCollider2D>();
            col.size = new Vector2(1f, 1f);
            col.enabled = defaultCollision;

            // 添加 Animator（动画）
            Animator anim = go.AddComponent<Animator>();

            // 保存为预制体
            PrefabUtility.SaveAsPrefabAsset(go, path);

            // 删除临时GameObject
            DestroyImmediate(go);

            MapToolLog.Info($"[MapTemplatePrefabCreator] 生成: {path} (2D世界模式)");
        }
    }
}
