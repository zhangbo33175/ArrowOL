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
                "1. ObjectTemplate.prefab - 物件模板\n" +
                "2. BuildingTemplate.prefab - 建筑模板\n" +
                "3. CharacterTemplate.prefab - 人物模板", MessageType.Info);

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
            string savePath = "Assets/Res/Prefabs/Model/";

            // 创建文件夹
            if (!AssetDatabase.IsValidFolder(savePath))
            {
                AssetDatabase.CreateFolder("Assets/Res/Prefabs", "Model");
            }

            // 1. 物件模板
            CreateTemplatePrefab(savePath + "ObjectTemplate.prefab", "物件模板", MapObjectType.Prop, 10, false);

            // 2. 建筑模板
            CreateTemplatePrefab(savePath + "BuildingTemplate.prefab", "建筑模板", MapObjectType.Building, 5, true);

            // 3. 人物模板
            CreateTemplatePrefab(savePath + "CharacterTemplate.prefab", "人物模板", MapObjectType.Character, 50, true);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log("[MapTemplatePrefabCreator] 所有模板预制体生成完成！");
            EditorUtility.DisplayDialog("成功", "模板预制体已生成到 Assets/Res/Prefabs/Model/ 目录！", "确定");
        }

        /// <summary>
        /// 创建单个模板预制体
        /// </summary>
        private void CreateTemplatePrefab(string path, string name, MapObjectType type, int defaultSortOrder, bool defaultCollision)
        {
            // 创建GameObject
            GameObject go = new GameObject(name);

            // 添加SpriteRenderer
            SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
            sr.sortingOrder = defaultSortOrder;

            // 添加BoxCollider2D
            BoxCollider2D col = go.AddComponent<BoxCollider2D>();
            col.size = new Vector2(1f, 1f);
            col.enabled = defaultCollision;

            // 添加Animator
            Animator anim = go.AddComponent<Animator>();

            // 添加模板基类脚本（暂未实现，先注释掉）
            // MapObjectTemplateBase template = go.AddComponent<MapObjectTemplateBase>();
            // template.templateName = name;
            // template.objectType = (Honor.Runtime.Map.MapObjectType)type;
            // template.sortOrder = defaultSortOrder;
            // template.collisionEnabled = defaultCollision;
            // template.spriteRenderer = sr;
            // template.animator = anim;
            // template.collider = col;

            // 保存为预制体
            PrefabUtility.SaveAsPrefabAsset(go, path);

            // 删除临时GameObject
            DestroyImmediate(go);

            Debug.Log($"[MapTemplatePrefabCreator] 生成: {path}");
        }
    }
}
