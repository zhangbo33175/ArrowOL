/***************************************************************
 * (c) copyright 2026 - 2030, Honor.Runtime
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  MapToolConstants.cs
 * author:    云毅
 * created:   2026
 * descrip:   地图编辑器全局常量定义（配置表路径/资源路径/尺寸常量/EditorPrefs键）
 ***************************************************************/

using UnityEditor;
using UnityEngine;

namespace Honor.Editor.MapTool
{
    /// <summary>
    /// 地图编辑器全局常量
    /// </summary>
    public static class MapToolConstants
    {
        // ==================== EditorPrefs 键 ====================

        /// <summary>
        /// 全局模板预制体路径的 EditorPrefs 键
        /// </summary>
        public const string TemplatePrefabPrefsKey = "MapTool_TemplatePrefabPath";

        /// <summary>
        /// 地图配置窗口 UXML 资源路径
        /// </summary>
        public const string MapConfigWindowUxmlPath = "Assets/Scripts/Editor/MapTool/MapConfigWindow/MapConfigWindow.uxml";

        /// <summary>
        /// 地图配置窗口 USS 资源路径
        /// </summary>
        public const string MapConfigWindowUssPath = "Assets/Scripts/Editor/MapTool/MapConfigWindow/MapConfigWindow.uss";

        // ==================== 配置表路径（相对 Application.dataPath） ====================

        /// <summary>
        /// 章节配置表路径的 EditorPrefs 键
        /// </summary>
        public const string TableChaptersPathPrefsKey = "MapTool_Path_TableChapters";

        /// <summary>
        /// 章节配置表路径默认值（场景名 / 场景ID / 分区配置来源）
        /// </summary>
        private const string k_TableChaptersPathDefault = "../Docs/Designs/Excels/Tables/AlTables/LevelConfigs/TableChapters.xlsm";

        /// <summary>
        /// 章节配置表路径（可在地图配置弹窗中修改，EditorPrefs 持久化）
        /// </summary>
        public static string TableChaptersPath => GetConfigPath(TableChaptersPathPrefsKey);

        /// <summary>
        /// 主关卡配置表路径的 EditorPrefs 键
        /// </summary>
        public const string TableMainLevelsPathPrefsKey = "MapTool_Path_TableMainLevels";

        /// <summary>
        /// 主关卡配置表路径默认值（分区背景图来源）
        /// </summary>
        private const string k_TableMainLevelsPathDefault = "../Docs/Designs/Excels/Tables/AlTables/LevelConfigs/TableMainLevels.xlsm";

        /// <summary>
        /// 主关卡配置表路径（可在地图配置弹窗中修改，EditorPrefs 持久化）
        /// </summary>
        public static string TableMainLevelsPath => GetConfigPath(TableMainLevelsPathPrefsKey);

        // ==================== 资源目录（相对 Assets） ====================

        /// <summary>
        /// 物体预览图目录一的 EditorPrefs 键
        /// </summary>
        public const string ObjectPreviewPrefabsDirPrefsKey = "MapTool_Path_ObjectPreviewPrefabs";

        /// <summary>
        /// 物体预览图目录一默认值（Prefabs 下）
        /// </summary>
        private const string k_ObjectPreviewPrefabsDirDefault = "Assets/Res/Prefabs/Map/Objects";

        /// <summary>
        /// 物体预览图目录一（可在地图配置弹窗中修改，EditorPrefs 持久化）
        /// </summary>
        public static string ObjectPreviewPrefabsDir => GetConfigPath(ObjectPreviewPrefabsDirPrefsKey);

        /// <summary>
        /// 物体预览图目录二的 EditorPrefs 键
        /// </summary>
        public const string ObjectPreviewTexturesDirPrefsKey = "MapTool_Path_ObjectPreviewTextures";

        /// <summary>
        /// 物体预览图目录二默认值（Textures 下）
        /// </summary>
        private const string k_ObjectPreviewTexturesDirDefault = "Assets/Res/Textures/Map/Objects";

        /// <summary>
        /// 物体预览图目录二（可在地图配置弹窗中修改，EditorPrefs 持久化）
        /// </summary>
        public static string ObjectPreviewTexturesDir => GetConfigPath(ObjectPreviewTexturesDirPrefsKey);

        /// <summary>
        /// 分区背景图目录的 EditorPrefs 键
        /// </summary>
        public const string LevelBackgroundDirPrefsKey = "MapTool_Path_LevelBackground";

        /// <summary>
        /// 分区背景图目录默认值（拼接 {bgName}.png）
        /// </summary>
        private const string k_LevelBackgroundDirDefault = "Assets/Res/Textures/Map/LevelBackground";

        /// <summary>
        /// 分区背景图目录（可在地图配置弹窗中修改，EditorPrefs 持久化）
        /// </summary>
        public static string LevelBackgroundDir => GetConfigPath(LevelBackgroundDirPrefsKey);

        /// <summary>
        /// 章节地图保存目录的 EditorPrefs 键
        /// </summary>
        public const string MapSaveDirPrefsKey = "MapTool_Path_MapSaveDir";

        /// <summary>
        /// 章节地图保存目录默认值（拼接 Map_{chapterId}.json / .prefab）
        /// </summary>
        private const string k_MapSaveDirDefault = "Assets/GameData/Maps";

        /// <summary>
        /// 章节地图保存目录（可在地图配置弹窗中修改，EditorPrefs 持久化）
        /// </summary>
        public static string MapSaveDir => GetConfigPath(MapSaveDirPrefsKey);

        // ---TODO 双格式保存：JSON 新保存目录（预制体路径仍走 MapSaveDir，严禁改其默认值）
        /// <summary>
        /// 章节地图 JSON 保存目录（Map_{chapterId}.json 落此目录；Lua 同源导出）
        /// </summary>
        public const string MapJsonSaveDir = "Assets/GameData/MapDatas";

        /// <summary>
        /// 地图保存目录的父目录（用于按需创建目录结构）
        /// </summary>
        public const string GameDataDir = "Assets/GameData";

        // ==================== 地图尺寸 ====================

        /// <summary>
        /// 单分区像素尺寸（每个分区 2048x2048）
        /// </summary>
        public const int RegionPixelSize = 2048;

        /// <summary>
        /// 摆放 / 路径网格的格子尺寸（像素）
        /// </summary>
        public const int GridSize = 32;

        /// <summary>
        /// 最大缩放倍率
        /// </summary>
        public const float MaxZoom = 5f;

        /// <summary>
        /// 地图边缘拖拽留白（像素）
        /// </summary>
        public const float DragEdgeMargin = 100f;

        /// <summary>
        /// 判断点击 / 拖动的移动阈值（像素）
        /// </summary>
        public const float DragThreshold = 4f;

        // ==================== 物体默认尺寸（格子数） ====================

        /// <summary>
        /// 建筑默认尺寸（宽 x 高）
        /// </summary>
        public static readonly Vector2Int BuildingDefaultSize = new Vector2Int(4, 4);

        /// <summary>
        /// 人物默认尺寸（宽 x 高）
        /// </summary>
        public static readonly Vector2Int CharacterDefaultSize = new Vector2Int(1, 2);

        /// <summary>
        /// 物件默认尺寸（宽 x 高）
        /// </summary>
        public static readonly Vector2Int PropDefaultSize = new Vector2Int(1, 1);

        // ==================== 可配置路径读写 ====================

        /// <summary>
        /// 读取可配置路径：优先返回 EditorPrefs 中保存的值，未配置时回退默认值
        /// </summary>
        /// <param name="prefsKey">EditorPrefs 键</param>
        public static string GetConfigPath(string prefsKey)
        {
            return GetConfigPath(prefsKey, GetConfigPathDefault(prefsKey));
        }

        /// <summary>
        /// 读取可配置路径：优先返回 EditorPrefs 中保存的值，未配置时回退指定默认值
        /// </summary>
        /// <param name="prefsKey">EditorPrefs 键</param>
        /// <param name="defaultValue">默认路径</param>
        public static string GetConfigPath(string prefsKey, string defaultValue)
        {
            string saved = EditorPrefs.GetString(prefsKey, "");
            return string.IsNullOrEmpty(saved) ? defaultValue : saved;
        }

        /// <summary>
        /// 保存可配置路径到 EditorPrefs
        /// </summary>
        /// <param name="prefsKey">EditorPrefs 键</param>
        /// <param name="value">要保存的路径值</param>
        public static void SetConfigPath(string prefsKey, string value)
        {
            EditorPrefs.SetString(prefsKey, value);
        }

        /// <summary>
        /// 获取路径配置的默认值（按 EditorPrefs 键映射）
        /// </summary>
        /// <param name="prefsKey">EditorPrefs 键</param>
        public static string GetConfigPathDefault(string prefsKey)
        {
            if (prefsKey == TableChaptersPathPrefsKey) return k_TableChaptersPathDefault;
            if (prefsKey == TableMainLevelsPathPrefsKey) return k_TableMainLevelsPathDefault;
            if (prefsKey == ObjectPreviewPrefabsDirPrefsKey) return k_ObjectPreviewPrefabsDirDefault;
            if (prefsKey == ObjectPreviewTexturesDirPrefsKey) return k_ObjectPreviewTexturesDirDefault;
            if (prefsKey == LevelBackgroundDirPrefsKey) return k_LevelBackgroundDirDefault;
            if (prefsKey == MapSaveDirPrefsKey) return k_MapSaveDirDefault;
            return "";
        }
    }
}